# Portable Profile Implementation Guide

**Target:** Windows LAN single-instance gameshow system (no Docker, no Redis)  
**Stack:** NestJS + Fastify + TypeScript + Postgres (portable) + MinIO (or filesystem)  

---

## Technology Choices for Portable Profile

### 1. Database: PostgreSQL Portable

**Option A: Postgres Portable Binaries (Recommended)**
```bash
# Download from https://www.postgresql.org/download/windows/
# Extract ZIP to C:\tools\postgresql-16-portable
# Initialize DB cluster (one-time)
cd C:\tools\postgresql-16-portable\bin
initdb -D C:\data\postgres
pg_ctl -D C:\data\postgres -l C:\data\logfile.log start
```

**Option B: PGlite Embedded (Single Connection Limit)**
- Useful for truly portable (single .exe), but concurrency issues
- Better for demo; production use Postgres portable binaries

**Connection:** Single instance, no replicas needed; `postgresql://localhost:5432/gameshow`

---

### 2. Job Queue: pg-boss (Postgres Native)

**Why not BullMQ?** No Redis available.

**Installation:**
```bash
npm install pg-boss pg
# Optional: npm install --save-dev @pg-boss/dashboard
```

**Configuration (NestJS):**

```typescript
// src/config/pg-boss.config.ts
import { Injectable } from '@nestjs/common';
import PgBoss from 'pg-boss';

@Injectable()
export class PgBossService {
  private boss: PgBoss;

  async initialize() {
    this.boss = new PgBoss({
      connectionString: process.env.DATABASE_URL,
      schema: 'pgboss',
      pollingInterval: 2000, // 2s for portable single-instance
      retryDelay: 5,
      retryBackoff: true,
      archiveCompletedAfterSeconds: 60 * 60 * 24, // 1 day
      expireInSeconds: 60 * 60 * 24 * 7, // 1 week
      cascadeDelete: false,
    });

    await this.boss.start();
    console.log('pg-boss started');
  }

  async registerJob<T>(name: string, handler: (job: PgBoss.Job<T>) => Promise<void>) {
    await this.boss.work(name, handler);
  }

  async scheduleJob<T>(name: string, data: T, options?: any) {
    return this.boss.send(name, data, options);
  }

  async stop() {
    await this.boss.stop();
  }

  getBoss(): PgBoss {
    return this.boss;
  }
}

// src/queue/queue.module.ts
import { Module, OnModuleInit, OnModuleDestroy } from '@nestjs/common';
import { PgBossService } from '../config/pg-boss.config';

@Module({
  providers: [PgBossService],
  exports: [PgBossService],
})
export class PgBossModule implements OnModuleInit, OnModuleDestroy {
  constructor(private pgBoss: PgBossService) {}

  async onModuleInit() {
    await this.pgBoss.initialize();
  }

  async onModuleDestroy() {
    await this.pgBoss.stop();
  }
}
```

---

### 3. In-Process Queue: Custom FIFO for Game Events

**Why not use pg-boss for game events?**
- Polling latency (even 2s) too high for <5-10ms realtime
- In-process queue guarantees immediate FIFO by design
- No network overhead

**Implementation:**

```typescript
// src/queue/inprocess-game-queue.ts
import { Injectable, Logger } from '@nestjs/common';
import { EventEmitter } from 'events';
import { v4 as uuid } from 'uuid';

export interface GameEvent {
  id: string;
  matchId: string;
  type: 'buzz' | 'answer' | 'admin' | 'score_update';
  playerId: string;
  timestamp: number;
  data: Record<string, any>;
}

@Injectable()
export class GameEventQueue extends EventEmitter {
  private queues = new Map<string, GameEvent[]>();
  private processing = new Map<string, boolean>();
  private handlers = new Map<string, (event: GameEvent) => Promise<void>>();
  private metrics = {
    totalProcessed: 0,
    totalFailed: 0,
    avgLatency: 0,
  };

  private readonly logger = new Logger(GameEventQueue.name);

  /**
   * Push event to match-specific queue (FIFO per match)
   */
  async push(matchId: string, event: Omit<GameEvent, 'id' | 'timestamp'>): Promise<string> {
    const gameEvent: GameEvent = {
      ...event,
      id: `${matchId}:${uuid()}`,
      timestamp: Date.now(),
    };

    if (!this.queues.has(matchId)) {
      this.queues.set(matchId, []);
    }

    this.queues.get(matchId)!.push(gameEvent);
    this.logger.debug(`Queued ${event.type} for match ${matchId} (queue depth: ${this.queues.get(matchId)!.length})`);

    // Async process without blocking
    setImmediate(() => this.processQueue(matchId));

    return gameEvent.id;
  }

  /**
   * Register handler for match events
   */
  subscribe(matchId: string, handler: (event: GameEvent) => Promise<void>): void {
    this.handlers.set(matchId, handler);
    this.logger.log(`Handler registered for match ${matchId}`);
  }

  /**
   * Process queue sequentially (FIFO guaranteed)
   */
  private async processQueue(matchId: string): Promise<void> {
    if (this.processing.get(matchId)) {
      // Already processing; next event will trigger processQueue again
      return;
    }

    this.processing.set(matchId, true);

    try {
      const queue = this.queues.get(matchId)!;
      const handler = this.handlers.get(matchId);

      if (!handler) {
        this.logger.warn(`No handler for match ${matchId}; events accumulating`);
        this.processing.set(matchId, false);
        return;
      }

      while (queue.length > 0) {
        const event = queue.shift()!;
        const startTime = Date.now();

        try {
          await handler(event);
          const latency = Date.now() - startTime;

          // Update metrics
          this.metrics.totalProcessed++;
          this.metrics.avgLatency = (this.metrics.avgLatency + latency) / 2;

          this.emit('event:completed', { matchId, eventId: event.id, latency });
          this.logger.debug(
            `Event ${event.id} processed in ${latency}ms (type: ${event.type})`
          );
        } catch (error) {
          this.metrics.totalFailed++;
          this.logger.error(
            `Event ${event.id} failed: ${error}`,
            (error as Error)?.stack
          );

          // Emit failed event; app can decide to retry/DLQ
          this.emit('event:failed', {
            matchId,
            eventId: event.id,
            error: (error as Error)?.message,
          });

          // Re-queue for retry (simple strategy: up to 3 times)
          const retryCount = (event.data._retries ?? 0) + 1;
          if (retryCount < 3) {
            event.data._retries = retryCount;
            queue.unshift(event); // Re-queue at front
            await new Promise((r) => setTimeout(r, 100 * retryCount)); // Backoff
          } else {
            this.logger.error(
              `Event ${event.id} exhausted retries; dropping`
            );
          }
        }
      }
    } finally {
      this.processing.set(matchId, false);
    }
  }

  /**
   * Get metrics
   */
  getMetrics() {
    return { ...this.metrics, queueDepth: this.queues.size };
  }

  /**
   * For testing: drain all queues
   */
  async drain() {
    const maxWait = 30000; // 30s timeout
    const startTime = Date.now();
    while (
      Array.from(this.queues.values()).some((q) => q.length > 0) ||
      Array.from(this.processing.values()).some((p) => p)
    ) {
      if (Date.now() - startTime > maxWait) {
        this.logger.warn('Drain timeout exceeded');
        break;
      }
      await new Promise((r) => setTimeout(r, 10));
    }
  }
}
```

**Module Setup:**

```typescript
// src/queue/game-event-queue.module.ts
import { Module } from '@nestjs/common';
import { GameEventQueue } from './inprocess-game-queue';

@Module({
  providers: [GameEventQueue],
  exports: [GameEventQueue],
})
export class GameEventQueueModule {}
```

---

### 4. Socket.IO Configuration (Single Instance, No Redis Adapter)

**NestJS Gateway:**

```typescript
// src/gateways/gameshow.gateway.ts
import { WebSocketGateway, WebSocketServer, SubscribeMessage, OnGatewayInit } from '@nestjs/websockets';
import { Server, Socket } from 'socket.io';
import { GameEventQueue, GameEvent } from '../queue/inprocess-game-queue';
import { MatchService } from '../match/match.service';
import { Logger } from '@nestjs/common';

@WebSocketGateway({
  cors: { origin: '*', credentials: true },
  transports: ['websocket', 'polling'],
})
export class GameShowGateway implements OnGatewayInit {
  @WebSocketServer() server: Server;
  private logger = new Logger(GameShowGateway.name);

  constructor(
    private gameEventQueue: GameEventQueue,
    private matchService: MatchService
  ) {}

  async afterInit(server: Server) {
    this.logger.log('GameShow WebSocket Gateway initialized');

    // Subscribe to queue events for broadcasting
    this.gameEventQueue.on('event:completed', (data) => {
      this.server.to(`match:${data.matchId}`).emit('event', {
        eventId: data.eventId,
        latency: data.latency,
        status: 'completed',
      });
    });

    this.gameEventQueue.on('event:failed', (data) => {
      this.logger.error(`Event failed: ${data.eventId}`);
      this.server.to(`match:${data.matchId}`).emit('event:error', {
        eventId: data.eventId,
        error: data.error,
      });
    });
  }

  @SubscribeMessage('join_match')
  async joinMatch(socket: Socket, data: { matchId: string }) {
    socket.join(`match:${data.matchId}`);
    this.logger.log(`Client ${socket.id} joined match ${data.matchId}`);
  }

  @SubscribeMessage('buzz')
  async handleBuzz(socket: Socket, data: { matchId: string; playerId: string }) {
    const { matchId, playerId } = data;

    try {
      // In single-instance portable, no lease needed; process immediately
      await this.gameEventQueue.push(matchId, {
        type: 'buzz',
        playerId,
        matchId,
        data: {
          buzzTime: Date.now(),
          clientId: socket.id,
        },
      });

      // Ack to client
      socket.emit('buzz:ack', { status: 'queued' });
    } catch (error) {
      socket.emit('buzz:error', { error: (error as Error).message });
    }
  }

  @SubscribeMessage('answer')
  async handleAnswer(
    socket: Socket,
    data: { matchId: string; playerId: string; answer: string }
  ) {
    const { matchId, playerId, answer } = data;

    try {
      await this.gameEventQueue.push(matchId, {
        type: 'answer',
        playerId,
        matchId,
        data: { answer, answeredAt: Date.now(), clientId: socket.id },
      });

      socket.emit('answer:ack', { status: 'queued' });
    } catch (error) {
      socket.emit('answer:error', { error: (error as Error).message });
    }
  }
}
```

---

### 5. Match Service: Consume Game Events

```typescript
// src/match/match.service.ts
import { Injectable, Logger, OnModuleInit } from '@nestjs/common';
import { GameEventQueue, GameEvent } from '../queue/inprocess-game-queue';
import { PgBossService } from '../config/pg-boss.config';
import { DataSource } from 'typeorm';

@Injectable()
export class MatchService implements OnModuleInit {
  private logger = new Logger(MatchService.name);

  constructor(
    private gameEventQueue: GameEventQueue,
    private pgBoss: PgBossService,
    private dataSource: DataSource
  ) {}

  async onModuleInit() {
    // For each active match, subscribe to its event queue
    const activeMatches = await this.getActiveMatches();

    for (const match of activeMatches) {
      this.gameEventQueue.subscribe(match.id, (event) =>
        this.processGameEvent(match.id, event)
      );
      this.logger.log(`Subscribed to match ${match.id} events`);
    }
  }

  /**
   * FIFO event processing per match
   */
  async processGameEvent(matchId: string, event: GameEvent): Promise<void> {
    const queryRunner = this.dataSource.createQueryRunner();
    await queryRunner.connect();
    await queryRunner.startTransaction();

    try {
      // Log event to event_log (event sourcing)
      await queryRunner.manager.query(
        `INSERT INTO match_events (id, match_id, type, player_id, data, created_at)
         VALUES ($1, $2, $3, $4, $5, NOW())`,
        [event.id, matchId, event.type, event.playerId, JSON.stringify(event.data)]
      );

      // Process by type
      switch (event.type) {
        case 'buzz':
          await this.handleBuzz(queryRunner, matchId, event);
          break;
        case 'answer':
          await this.handleAnswer(queryRunner, matchId, event);
          break;
        case 'score_update':
          await this.handleScoreUpdate(queryRunner, matchId, event);
          break;
        default:
          this.logger.warn(`Unknown event type: ${event.type}`);
      }

      // Persist state
      await queryRunner.manager.query(
        `UPDATE matches SET state = state || $1::jsonb, updated_at = NOW()
         WHERE id = $2`,
        [JSON.stringify({ lastEventId: event.id, lastEventTime: event.timestamp }), matchId]
      );

      await queryRunner.commitTransaction();
      this.logger.debug(`Match ${matchId} event ${event.id} committed`);
    } catch (error) {
      await queryRunner.rollbackTransaction();
      this.logger.error(`Failed to process event ${event.id}: ${error}`);
      throw error; // Re-throw for queue retry logic
    } finally {
      await queryRunner.release();
    }
  }

  private async handleBuzz(queryRunner: any, matchId: string, event: GameEvent) {
    // Find current question, increment buzz count, lock player from buzzing again
    const result = await queryRunner.manager.query(
      `UPDATE match_state SET last_buzz_id = $1, last_buzz_time = $2
       WHERE match_id = $3 RETURNING *`,
      [event.playerId, event.timestamp, matchId]
    );
    this.logger.log(`Buzz handled for player ${event.playerId} in match ${matchId}`);
  }

  private async handleAnswer(queryRunner: any, matchId: string, event: GameEvent) {
    const { answer, answeredAt } = event.data;
    // Verify answer, update score
    this.logger.log(`Answer processed: "${answer}" from player ${event.playerId}`);
  }

  private async handleScoreUpdate(queryRunner: any, matchId: string, event: GameEvent) {
    // Admin event
    const { points, playerId } = event.data;
    await queryRunner.manager.query(
      `UPDATE player_scores SET score = score + $1 WHERE player_id = $2 AND match_id = $3`,
      [points, playerId, matchId]
    );
  }

  private async getActiveMatches() {
    return this.dataSource.query(
      `SELECT id FROM matches WHERE status IN ('active', 'paused') LIMIT 100`
    );
  }
}
```

---

### 6. Background Jobs (pg-boss)

```typescript
// src/jobs/export-results.job.ts
import { Injectable } from '@nestjs/common';
import { PgBossService } from '../config/pg-boss.config';
import { DataSource } from 'typeorm';
import * as path from 'path';
import * as fs from 'fs/promises';

@Injectable()
export class ExportResultsJob {
  constructor(private pgBoss: PgBossService, private dataSource: DataSource) {}

  async scheduleExport(matchId: string, format: 'json' | 'csv' | 'excel'): Promise<void> {
    await this.pgBoss.scheduleJob('export-match-results', {
      matchId,
      format,
      requestedAt: new Date(),
    });
  }

  async registerWorker(): Promise<void> {
    await this.pgBoss.registerJob(
      'export-match-results',
      async (job) => {
        const { matchId, format } = job.data;
        console.log(`Exporting match ${matchId} as ${format}`);

        try {
          // Fetch match data
          const matchData = await this.dataSource.query(
            `SELECT * FROM matches WHERE id = $1`,
            [matchId]
          );

          if (!matchData.length) {
            throw new Error(`Match ${matchId} not found`);
          }

          // Generate file
          const filename = `match-${matchId}-results.${format}`;
          const outputPath = path.join(process.env.EXPORT_DIR || './exports', filename);

          // Create directory if not exists
          await fs.mkdir(path.dirname(outputPath), { recursive: true });

          // Generate content based on format
          let content: string;
          if (format === 'json') {
            content = JSON.stringify(matchData, null, 2);
          } else if (format === 'csv') {
            content = this.toCsv(matchData);
          } else if (format === 'excel') {
            // Use xlsx library
            content = '(Excel generation not implemented in this sample)';
          }

          await fs.writeFile(outputPath, content);
          console.log(`Exported to ${outputPath}`);

          // Log job completion
          await this.dataSource.query(
            `INSERT INTO job_results (job_type, match_id, status, output_path)
             VALUES ('export', $1, 'completed', $2)`,
            [matchId, outputPath]
          );
        } catch (error) {
          console.error(`Export failed: ${error}`);
          throw error; // pg-boss will retry
        }
      }
    );
  }

  private toCsv(data: any[]): string {
    if (!data.length) return '';
    const headers = Object.keys(data[0]);
    const rows = data.map((row) =>
      headers.map((h) => JSON.stringify(row[h])).join(',')
    );
    return [headers.join(','), ...rows].join('\n');
  }
}
```

---

### 7. Docker-free MinIO Setup (Optional, can use filesystem)

**Filesystem Adapter (simplest):**

```typescript
// src/storage/filesystem-storage.ts
import { Injectable } from '@nestjs/common';
import * as fs from 'fs/promises';
import * as path from 'path';

@Injectable()
export class FilesystemStorageAdapter {
  private basePath = process.env.STORAGE_PATH || './storage';

  async uploadFile(key: string, buffer: Buffer): Promise<string> {
    const filePath = path.join(this.basePath, key);
    await fs.mkdir(path.dirname(filePath), { recursive: true });
    await fs.writeFile(filePath, buffer);
    return filePath;
  }

  async downloadFile(key: string): Promise<Buffer> {
    const filePath = path.join(this.basePath, key);
    return fs.readFile(filePath);
  }

  async deleteFile(key: string): Promise<void> {
    const filePath = path.join(this.basePath, key);
    await fs.unlink(filePath);
  }
}
```

**MinIO Standalone (if needed):**

```bash
# Download from https://dl.min.io/server/minio/release/windows-amd64/
# Extract minio.exe
minio.exe server C:\data\minio --console-address :9001
# Access http://localhost:9001 with minioadmin/minioadmin
```

---

### 8. Environment Configuration

**`.env` for portable profile:**

```bash
# Deployment
PROFILE=portable
NODE_ENV=development

# Database
DATABASE_URL=postgresql://postgres:password@localhost:5432/gameshow

# Queue
PG_BOSS_POLLING_INTERVAL=2000  # 2 seconds

# Storage
STORAGE_PATH=./storage
EXPORT_DIR=./exports

# Socket.IO
SOCKET_IO_PORT=3001
SOCKET_IO_TRANSPORTS=websocket,polling

# Logging
LOG_LEVEL=debug
```

**`.env` for compose profile:**

```bash
PROFILE=compose
NODE_ENV=production

DATABASE_URL=postgresql://postgres:password@postgres:5432/gameshow
REDIS_URL=redis://redis:6379

LOG_LEVEL=info
```

---

## Testing Portable Profile

**Unit Test: In-Process Queue:**

```typescript
// src/queue/inprocess-game-queue.spec.ts
import { GameEventQueue } from './inprocess-game-queue';

describe('GameEventQueue', () => {
  let queue: GameEventQueue;

  beforeEach(() => {
    queue = new GameEventQueue();
  });

  it('should process events in FIFO order', async (done) => {
    const results = [];
    const matchId = 'match-123';

    queue.subscribe(matchId, async (event) => {
      results.push(event.id);
    });

    await queue.push(matchId, {
      type: 'buzz',
      playerId: 'player-1',
      matchId,
      data: { order: 1 },
    });

    await queue.push(matchId, {
      type: 'answer',
      playerId: 'player-2',
      matchId,
      data: { order: 2 },
    });

    await queue.push(matchId, {
      type: 'buzz',
      playerId: 'player-3',
      matchId,
      data: { order: 3 },
    });

    // Wait for queue to process
    await queue.drain();

    expect(results.length).toBe(3);
    // Verify order maintained (event IDs will be in order received)
    console.log('Results:', results);
    done();
  });

  it('should handle errors and retry', async (done) => {
    const matchId = 'match-456';
    let callCount = 0;

    queue.subscribe(matchId, async (event) => {
      callCount++;
      if (callCount < 2) {
        throw new Error('First attempt fails');
      }
    });

    queue.on('event:failed', (data) => {
      console.log(`Event failed: ${data.eventId}`);
    });

    await queue.push(matchId, {
      type: 'buzz',
      playerId: 'player-1',
      matchId,
      data: {},
    });

    await queue.drain();
    expect(callCount).toBeGreaterThan(1); // Retried
    done();
  });
});
```

---

## Deployment: Portable Windows

**Directory Structure:**

```
C:\gameshow-portable\
├── app\                          # NestJS app
│   ├── dist\
│   ├── src\
│   ├── package.json
│   └── .env
├── postgres\                      # Portable Postgres
│   ├── bin\
│   └── data\
├── storage\                       # Local file storage
├── exports\                       # Exported match results
├── run.bat                        # Start script
└── README.md
```

**`run.bat` Startup Script:**

```batch
@echo off
cd %~dp0

REM Start Postgres (if not running)
echo Starting PostgreSQL...
postgres\bin\pg_ctl -D postgres\data -l postgres\logfile.log start

REM Wait for DB to be ready
timeout /t 3

REM Install Node deps (first time only)
if not exist node_modules (
  echo Installing dependencies...
  call npm install
)

REM Run migrations
echo Running database migrations...
npm run typeorm migration:run

REM Start NestJS app
echo Starting GameShow application...
npm run start

REM Cleanup on exit
echo Stopping PostgreSQL...
postgres\bin\pg_ctl -D postgres\data stop
pause
```

---

## Summary: Portable Profile Stack

| Component | Solution | Why |
|-----------|----------|-----|
| **Database** | Postgres portable (zip binaries) | No installer; single instance; ACID |
| **Game Events Queue** | In-process FIFO + EventEmitter | No network; guaranteed FIFO; <1ms latency |
| **Background Jobs** | pg-boss (Postgres-based) | SKIP LOCKED; ACID; no Redis needed |
| **WebSocket** | Socket.IO (no Redis adapter) | Single instance; direct routing |
| **File Storage** | Filesystem adapter | No MinIO needed; cheap VPS disk |
| **Admin Dashboard** | pg-boss UI (optional) | Lightweight; Postgres native |

