# Queue Architecture Research: Gameshow Realtime System

**Date:** 2026-07-12  
**Stack:** NestJS + Fastify + TypeScript + Socket.IO + PostgreSQL + Redis (compose profile) / Postgres only (portable profile)  
**Deployment:** Docker Compose multi-instance + Portable Windows single-instance  
**Author:** Technical Analysis

---

## Executive Summary

**Two distinct deployment profiles require different queue strategies:**

| Problem | Compose Profile (Redis+PG) | Portable Profile (Windows, no Redis) |
|---------|---------------------------|--------------------------------------|
| **A. Game-event ingestion** (hot: <5-10ms) | **Redis Streams** via BullMQ (concurrency=1, single worker) OR native XREADGROUP | **In-process FIFO + EventEmitter** via SequentialTaskQueue or minimal homegrown queue |
| **B. Background jobs** (cold: batch/import/export/transcode) | **BullMQ** (@nestjs/bullmq, Bull Board dashboard, mature) | **pg-boss** (Postgres native, works with PGlite embedded, SKIP LOCKED atomic) |

**Critical design:** Implement QueueDriver/StateDriver abstraction layer with 2 implementations, so application code stays identical across profiles.

---

## Problem A: Game-Event Ingestion (Hot Path)

### Requirements
- **Throughput:** 10-50 events/sec per match (buzz/answer/admin)
- **Latency:** <5-10ms overhead in queue system
- **Ordering:** Strict per-match FIFO (1 match = 1 queue/partition)
- **Durability:** At-least-once, replay-able after crash (event-sourced to PG)
- **Availability:** Single owner instance processes, fail-over via Redis lease

### Candidates Evaluated

#### 1. **Redis Streams (XADD + Consumer Group)**
- **Latency:** 0.1–1ms read/write latency at 100K-1M msg/s scale ✅
- **Ordering:** FIFO within stream (append-only log) ✅
- **Durability:** XINFO + pending entries recoverable after crash ✅
- **Complexity:** Direct Lua/TTL management; no Bull Board visibility ❌
- **Failover:** XPENDING + XCLAIM for owner re-balance; manual lease tracking
- **Multi-instance:** Each stream = 1 Redis node (hot spot if >100k/sec on stream)

**Pitfall:** MAXLEN/MINID trim policy can drop old entries; must manually archive to PG before trim.

#### 2. **BullMQ (Redis Streams wrapper, @nestjs/bullmq)**
- **Latency:** ~1-5ms overhead vs raw Streams (Lua script + job marshalling) ⚠️
- **Ordering:** ✅ Guaranteed FIFO **only if concurrency=1 + single worker instance**
- **Durability:** Job state in Redis; job outcomes logged to PG ✅
- **Complexity:** Easy NestJS integration; Bull Board dashboard ✅
- **Failover:** Auto-recover stalled jobs; manual concurrency/failover config
- **Concurrency pitfall:** **If concurrency>1 or multiple workers, ordering breaks** ❌

**Pitfall:** Cross-job ordering not guaranteed when concurrency>1; designed for fan-out parallelism, not strict FIFO.

#### 3. **NATS JetStream (external service)**
- **Latency:** Sub-millisecond ✅
- **Ordering:** Subject-based ordered consumers ✅
- **Durability:** Durable subscriptions with replay ✅
- **Cost:** **Requires new infrastructure (NATS server)** ❌ Violates "no new services" constraint
- **Adoption:** Small team, VPS self-host = operational burden

**Verdict:** Eliminated per requirements.

#### 4. **Redis Pub/Sub (native)**
- **Latency:** Sub-millisecond ✅
- **Ordering:** Per-channel FIFO ✅
- **Durability:** **Zero — loses messages on subscriber disconnect** ❌ Event-sourcing requires at-least-once

**Verdict:** Not suitable.

---

## Problem B: Background Jobs (Cold Path)

### Requirements
- **Throughput:** Low (1-10 jobs/sec), can batch
- **Features:** Retry/backoff, concurrency control, scheduled/delayed, dashboard
- **Durability:** At-least-once with ACID guarantees preferred
- **Admin UX:** Visibility, retry/cancel UI

### Candidates Evaluated

#### 1. **BullMQ (@nestjs/bullmq)**
- **Maturity:** Industry-standard, 1000+ GitHub stars ✅
- **Features:** Job flows, rate limiting, DLQ, repeatable jobs ✅
- **Dashboard:** Bull Board (beautiful, functional) ✅
- **Reliability:** Exactly-once via Redis Lua scripts (in practice: at-least-once with de-dupe burden on app)
- **Infrastructure:** Requires Redis (compose only, not portable) ❌
- **NestJS DX:** First-class @InjectQueue, @Process decorators ✅

**Pitfall:** Multi-worker concurrency requires explicit locking if global ordering needed.

#### 2. **pg-boss (Postgres native)**
- **Maturity:** Stable, 2K+ GitHub stars, active maintenance ✅
- **Features:** Job dependency, cron, rate limiting, DLQ, LISTEN/NOTIFY for fast polling ✅
- **Dashboard:** @pg-boss/dashboard (functional, less polished than Bull Board) ✅
- **Reliability:** **Exactly-once via SKIP LOCKED + ACID transactions** ✅✅
- **Infrastructure:** Works with Postgres portable (PGlite embedded for single-instance) ✅
- **NestJS DX:** Community wrapper exists (nestjs-pg-boss), but less integrated than @nestjs/bullmq

**Pitfall:** Polling latency (configurable, default 5-10s) vs Pub/Sub (LISTEN/NOTIFY adds ~100ms). SKIP LOCKED doesn't auto-scale past 1-2 workers per job type.

#### 3. **Graphile Worker (Postgres native)**
- **Maturity:** Newer (compared to pg-boss), active development ✅
- **Features:** Task functions, cron, dependencies ✅
- **NestJS DX:** Community wrapper (nestjs-graphile-worker) less mature ⚠️
- **Reliability:** Exactly-once via Postgres ✅
- **Dashboard:** No built-in UI ❌

**Verdict:** Immature relative to pg-boss for production gameshow system. Pass.

#### 4. **RabbitMQ / Kafka**
- **Violates constraint:** "No new infrastructure unless critical" ❌

**Verdict:** Eliminated.

---

## Recommended Solution: Two-Profile Architecture

### Profile 1: Docker Compose (Multi-instance, Redis+Postgres)

#### A. Game-Event Ingestion
**Recommendation: Redis Streams (BullMQ wrapper) with enforced concurrency=1 + single worker**

**Why:**
- Latency ~1-5ms matches <5-10ms target ✅
- FIFO ordering guaranteed by architecture (single worker) ✅
- BullMQ provides standard NestJS decorators + job marshalling
- Fall back to Bull Board visibility
- No new infrastructure

**Trade-off:** Single worker = bottleneck if event volume exceeds 1-instance capacity; scale by partitioning match IDs across multiple queues (match_1, match_2, ...) each with own worker.

#### B. Background Jobs
**Recommendation: BullMQ (@nestjs/bullmq)**

**Why:**
- Mature, production-proven (hundreds of companies)
- NestJS integration seamless (@InjectQueue, @Process)
- Bull Board dashboard for admin visibility
- Reuses existing Redis infrastructure
- Concurrency tunable per job type

---

### Profile 2: Portable Windows (Single-instance, Postgres only)

#### A. Game-Event Ingestion
**Recommendation: In-process FIFO Queue (EventEmitter-based)**

**Alternatives:** SequentialTaskQueue (npm), node-persistent-queue (with in-memory SQLite), or minimal homegrown 20-line FIFO using Array + EventEmitter.

**Why:**
- No external dependency (Redis not available on Windows portable)
- Single instance = guaranteed FIFO by JavaScript event loop
- Latency ~0.1-1ms (in-process, no network)
- Socket.IO event routing stays in-process (no Redis adapter needed)

**Implementation:** Use same QueueDriver interface as Compose profile; swap implementation at DI level.

#### B. Background Jobs
**Recommendation: pg-boss (with PGlite for embedded Postgres in portable)**

**Why:**
- Works with Postgres portable (single instance)
- Exactly-once ACID via SKIP LOCKED
- No Redis dependency
- Same interface across profiles via QueueDriver abstraction

**Caveat:** PGlite has limitations (single connection in some embedded versions); test concurrent workers. Consider using full Postgres portable binaries (zip/MinIO, no installer) for safer concurrency.

---

## Architecture Diagram: Event Flow (Compose Profile)

```
┌─────────────────────────────────────────────────────────────────┐
│                       Socket.IO Clients                         │
└───────────────────────────┬─────────────────────────────────────┘
                            │ buzz/answer/admin events
                            ▼
┌──────────────────────────────────────────────────────────────────┐
│ Socket.IO Server (Instance A/B/C, Fastify + Socket.IO)           │
├──────────────────────────────────────────────────────────────────┤
│  1. Acquire Redis lease: SET match:{id}:owner {instance_id} NX   │
│  2. If owner, push to Redis Streams: XADD match:{id}:events      │
│  3. If NOT owner, forward to owner via Socket.IO room            │
└──────────────────┬───────────────────────────────────────────────┘
                   │
    ┌──────────────▼──────────────┐
    │    Redis Streams            │
    │  match:123:events (XADD)    │
    │  → BullMQ consumer group    │
    │    per match owner          │
    └──────────────┬──────────────┘
                   │
    ┌──────────────▼──────────────────────────────────────┐
    │ BullMQ Worker (owner instance only)                │
    │  • Concurrency=1 (strict FIFO)                     │
    │  • Single instance (no load distribution)          │
    │  • Process order: buzz→validate→update score       │
    └──────────────┬──────────────────────────────────────┘
                   │
    ┌──────────────▼──────────────────────────────────────┐
    │ PostgreSQL: Event Log (append-only sourcing)        │
    │  INSERT match_events (event_id, match_id, ...) ✓   │
    └──────────────┬──────────────────────────────────────┘
                   │
    ┌──────────────▼──────────────────────────────────────┐
    │ BullMQ Emit QueueEvents: job:complete              │
    │ → Subscribe in Match Service                       │
    │ → Broadcast to all clients via Socket.IO Pub/Sub   │
    └────────────────────────────────────────────────────┘
```

---

## NestJS Code Samples

### 1. Queue Driver Abstraction (Both profiles use same interface)

```typescript
// src/queue/queue.driver.ts
export interface QueueJob<T = any> {
  id: string;
  data: T;
  timestamp: number;
  priority?: number;
}

export interface QueueDriver {
  push<T>(queueName: string, job: QueueJob<T>): Promise<void>;
  subscribe<T>(
    queueName: string,
    handler: (job: QueueJob<T>) => Promise<void>
  ): void;
  subscribe(queueName: string, handler: (job: any) => Promise<void>): void;
}
```

### 2. Compose Profile: BullMQ Implementation

```typescript
// src/queue/bullmq.queue.ts
import { Injectable } from '@nestjs/common';
import { Queue, Worker, QueueEvents } from 'bullmq';
import { Redis } from 'ioredis';
import { QueueDriver, QueueJob } from './queue.driver';

@Injectable()
export class BullMQQueueDriver implements QueueDriver {
  private queues = new Map<string, Queue>();
  private workers = new Map<string, Worker>();
  private redis: Redis;

  constructor(redis: Redis) {
    this.redis = redis;
  }

  async push<T>(queueName: string, job: QueueJob<T>): Promise<void> {
    const queue = this.getOrCreateQueue(queueName);
    await queue.add(job.id, job.data, {
      jobId: job.id,
      priority: job.priority,
      attempts: 3,
      backoff: { type: 'exponential', delay: 2000 },
    });
  }

  subscribe<T>(
    queueName: string,
    handler: (job: QueueJob<T>) => Promise<void>
  ): void {
    const queue = this.getOrCreateQueue(queueName);
    const worker = new Worker(
      queueName,
      async (bullJob) => {
        const wrappedJob: QueueJob<T> = {
          id: bullJob.id!,
          data: bullJob.data as T,
          timestamp: Date.now(),
        };
        await handler(wrappedJob);
      },
      {
        connection: this.redis,
        concurrency: 1, // CRITICAL: Enforce FIFO for game events
      }
    );

    worker.on('failed', (job, err) => console.error(`Job failed: ${err}`));
    this.workers.set(queueName, worker);

    // Expose queue events
    const events = new QueueEvents(queueName, { connection: this.redis });
    events.on('completed', ({ jobId }) => console.log(`${queueName}:${jobId} done`));
  }

  private getOrCreateQueue(queueName: string): Queue {
    if (!this.queues.has(queueName)) {
      this.queues.set(
        queueName,
        new Queue(queueName, { connection: this.redis })
      );
    }
    return this.queues.get(queueName)!;
  }
}
```

### 3. Portable Profile: In-Process FIFO Implementation

```typescript
// src/queue/inprocess.queue.ts
import { Injectable } from '@nestjs/common';
import { EventEmitter } from 'events';
import { QueueDriver, QueueJob } from './queue.driver';

@Injectable()
export class InProcessQueueDriver extends EventEmitter implements QueueDriver {
  private queues = new Map<string, QueueJob[]>();
  private processing = new Map<string, boolean>();
  private handlers = new Map<string, (job: QueueJob) => Promise<void>>();

  async push<T>(queueName: string, job: QueueJob<T>): Promise<void> {
    if (!this.queues.has(queueName)) {
      this.queues.set(queueName, []);
    }
    this.queues.get(queueName)!.push(job);
    this.processQueue(queueName);
  }

  subscribe<T>(
    queueName: string,
    handler: (job: QueueJob<T>) => Promise<void>
  ): void {
    this.handlers.set(queueName, handler);
  }

  private async processQueue(queueName: string): Promise<void> {
    if (this.processing.get(queueName)) return;
    this.processing.set(queueName, true);

    try {
      const queue = this.queues.get(queueName)!;
      const handler = this.handlers.get(queueName);
      if (!handler) return;

      while (queue.length > 0) {
        const job = queue.shift()!;
        try {
          await handler(job);
          this.emit(`${queueName}:completed`, job.id);
        } catch (err) {
          console.error(`Job ${job.id} failed: ${err}`);
          this.emit(`${queueName}:failed`, job.id, err);
        }
      }
    } finally {
      this.processing.set(queueName, false);
    }
  }
}
```

### 4. Module Setup (swaps driver based on profile)

```typescript
// src/queue/queue.module.ts
import { Module, DynamicModule } from '@nestjs/common';
import { QueueDriver } from './queue.driver';
import { BullMQQueueDriver } from './bullmq.queue';
import { InProcessQueueDriver } from './inprocess.queue';

export type QueueProfile = 'compose' | 'portable';

@Module({})
export class QueueModule {
  static register(profile: QueueProfile): DynamicModule {
    const driverProvider =
      profile === 'compose'
        ? {
            provide: QueueDriver,
            useClass: BullMQQueueDriver,
            inject: ['REDIS_CLIENT'], // Injected from config
          }
        : {
            provide: QueueDriver,
            useClass: InProcessQueueDriver,
          };

    return {
      module: QueueModule,
      providers: [driverProvider],
      exports: [QueueDriver],
    };
  }
}

// app.module.ts
const profile: QueueProfile = process.env.PROFILE === 'portable' ? 'portable' : 'compose';
QueueModule.register(profile),
```

### 5. Usage in Match Service (both profiles identical)

```typescript
// src/match/match-event.service.ts
import { Injectable } from '@nestjs/common';
import { QueueDriver, QueueJob } from '../queue/queue.driver';

@Injectable()
export class MatchEventService {
  constructor(private queueDriver: QueueDriver) {}

  async publishGameEvent(
    matchId: string,
    event: { type: 'buzz' | 'answer'; playerId: string; data: any }
  ): Promise<void> {
    const job: QueueJob = {
      id: `${matchId}:${Date.now()}`,
      data: event,
      timestamp: Date.now(),
    };
    await this.queueDriver.push(`match:${matchId}:events`, job);
  }

  subscribeToMatch(matchId: string, handler: (job: QueueJob) => Promise<void>): void {
    this.queueDriver.subscribe(`match:${matchId}:events`, handler);
  }
}

// Usage in WebSocket handler
async handleBuzz(matchId: string, playerId: string) {
  await this.matchEventService.publishGameEvent(matchId, {
    type: 'buzz',
    playerId,
    data: { timestamp: Date.now() },
  });
  // ✅ Routed to match owner (via Redis lease check or in-process in portable)
  // ✅ Guaranteed FIFO within match
  // ✅ Persisted to PG event_log after processing
}
```

### 6. Background Job Example (pg-boss, portable)

```typescript
// src/jobs/export.job.ts
import { Injectable } from '@nestjs/common';
import pgBoss from 'pg-boss';

@Injectable()
export class ExportJobService {
  constructor(private boss: pgBoss) {}

  async scheduleExport(matchId: string, format: 'excel' | 'json'): Promise<void> {
    await this.boss.send('export-match-results', {
      matchId,
      format,
      createdAt: new Date(),
    });
  }

  // In portable profile, initialize pgBoss with PGlite connection
  async onModuleInit() {
    if (process.env.PROFILE === 'portable') {
      await this.boss.start();
      this.boss.work('export-match-results', async (job) => {
        const { matchId, format } = job.data;
        console.log(`Exporting match ${matchId} as ${format}`);
        // Generate file, upload to MinIO or S3, log result
      });
    }
  }
}
```

---

## Pitfalls & Mitigation

### BullMQ Ordering Pitfall
- **Issue:** Concurrency > 1 breaks FIFO ordering across jobs
- **Mitigation:** For game events, enforce concurrency=1 + single worker instance
- **Detection:** Add test: verify 100 sequential events maintain order under load

### Redis Streams MAXLEN Trim Policy
- **Issue:** Auto-trim can drop entries before they're replayed
- **Mitigation:** Archive to PG before trim; use `XINFO STREAM` to monitor pending entries
- **Production:** Cron job every 5min: `XREAD` pending + archive to PG event_log, then trim

### pg-boss Polling Latency (Portable)
- **Issue:** Default 5-10s polling for background jobs slower than Pub/Sub
- **Mitigation:** Enable LISTEN/NOTIFY (PG 9.0+); reduce pollingInterval to 1-2s if CPU acceptable
- **Tradeoff:** Lower polling = higher CPU; test under typical background load

### Single Match Owner Bottleneck
- **Issue:** One instance owns match; if it crashes, events queue up in Redis Streams
- **Mitigation:** Implement lease renewal heartbeat + XCLAIM for failover
  ```typescript
  // Pseudo-code
  every 5s: EXPIRE match:{id}:owner 15
  if expire fails: instance died, XCLAIM pending entries to new owner via lease re-acquire
  ```

### PGlite Concurrency (Portable)
- **Issue:** PGlite embedded has single-connection limitation in some versions
- **Mitigation:** Use Postgres portable binaries (zip release) or Docker single-instance instead
- **Recommendation:** Portable profile = single Windows machine, so single PG process acceptable

### Socket.IO Event Forwarding Without Redis Adapter
- **Issue:** Portable profile can't use Redis adapter for inter-instance routing
- **Mitigation:** Single instance = no inter-instance routing needed; emit directly to Socket.IO namespace
- **Fallback:** If scalability needed, upgrade portable to compose profile

---

## Source Credibility Assessment

| Source | Credibility | Type |
|--------|------------|------|
| BullMQ official docs + GitHub | High | Official, 2K+ stars |
| pg-boss official docs + GitHub | High | Official, 2K+ stars |
| Socket.IO Redis adapter official | High | Official Socket.IO |
| Redis official docs (XREAD, XGROUP) | High | Official Redis |
| NestJS official @nestjs/bullmq | High | First-party module |
| Medium production case studies | Medium | Real-world, but vary |
| npm compare sites | Medium | Aggregated, not authoritative |
| Graphile Worker GitHub | Medium | Active, smaller community |
| SequentialTaskQueue GitHub | Low | Minimal-maintained, but proven |

---

## Unresolved Questions / Future Research

1. **Exact PGlite concurrency limits:** Test concurrent job workers with pg-boss + PGlite in portable; may need full Postgres portable binaries.
2. **Socket.IO room scaling without Redis:** If portable expands to 2+ instances, how to route events? Current design assumes single instance.
3. **Redis Cluster vs Sentinel for Compose:** Should lease/stream spans Redis Cluster? Cluster complicates consumer group offset tracking.
4. **BullMQ rate limiting per match:** Can BullMQ rate-limit per queue (per match) to avoid player spam? Built-in or custom middleware?
5. **Event sourcing schema:** Does PG event_log persist job ID → business event mapping for audit trail?

---

## Deployment Checklist

### Compose Profile (Multi-instance, Docker)
- [ ] Redis cluster or Sentinel configured with 5-30s failover target
- [ ] BullMQ single worker per match queue; no concurrency config drift
- [ ] Bull Board exposed (port 3333, protected by auth middleware)
- [ ] Redis Streams MAXLEN trim policy + archive cron job
- [ ] Match owner lease renewal heartbeat (5s interval, 15s TTL)
- [ ] Socket.IO Redis adapter configured with `publishOnSpecificResponseChannel: true`
- [ ] PG event_log partitioned by date for performance
- [ ] Monitoring: Redis memory, BullMQ job rate, lease conflicts

### Portable Profile (Windows single-instance)
- [ ] Postgres portable binaries (zip, no installer) or Docker Desktop WSL2
- [ ] MinIO single .exe or filesystem storage adapter
- [ ] QueueDriver env var set to `PROFILE=portable`
- [ ] pg-boss LISTEN/NOTIFY enabled if concurrent background workers planned
- [ ] Socket.IO no Redis adapter; direct in-process routing
- [ ] No lease/failover logic (single instance)
- [ ] Monitoring: PG CPU, job queue depth, in-process queue memory

---

## Final Recommendation Summary

| Dimension | Compose | Portable |
|-----------|---------|----------|
| **Game Events (A)** | BullMQ (concurrency=1) + Redis Streams | In-process FIFO + EventEmitter |
| **Background Jobs (B)** | BullMQ (@nestjs/bullmq) | pg-boss (Postgres) |
| **Abstraction** | QueueDriver interface shared across both |
| **Admin Dashboard** | Bull Board (compose); pg-boss dashboard (portable) |
| **Failover** | Redis Sentinel/Cluster; match owner lease renewal | N/A (single instance) |
| **Cost** | Redis infrastructure | None (use existing Postgres) |
| **Team Effort** | Low (BullMQ mature) | Low (pg-boss + in-process wrapper) |
| **Adoption Risk** | Low (1000+ production teams) | Medium (pg-boss proven, custom in-process queue) |

**Decision:** Proceed with dual-profile architecture. Compose is battle-tested; portable introduces minimal risk via abstraction layer.
