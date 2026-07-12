---
phase: 1
title: "Foundation & Infrastructure"
status: pending
priority: P1
dependencies: []
---

# Phase 1: Foundation & Infrastructure

## Overview
Dựng monorepo, hạ tầng dev (Postgres/Redis/MinIO), bootstrap NestJS+Fastify và **spike xác minh Socket.IO chạy được trên Fastify adapter** — rủi ro kỹ thuật số 1 của dự án.

## Requirements
- Functional: monorepo chạy `pnpm dev` là lên đủ api + web; Socket.IO echo test hoạt động.
- Non-functional: hot-reload cả 2 app; type-safe end-to-end qua `packages/shared`.

## Architecture

```mermaid
flowchart LR
  subgraph monorepo [pnpm workspace]
    shared[packages/shared<br/>Zod schemas, RuleConfig,<br/>socket event types]
    api[apps/api<br/>NestJS + Fastify]
    web[apps/web<br/>React + Vite]
    api --> shared
    web --> shared
  end
  subgraph docker [docker-compose.dev]
    pg[(PostgreSQL 16)]
    redis[(Redis 7)]
    minio[(MinIO)]
  end
  api --> pg & redis & minio
```

## Related Code Files
- Create: `pnpm-workspace.yaml`, `package.json`, `tsconfig.base.json`, `.editorconfig`, `docker-compose.dev.yml`
- Create: `apps/api/` (NestJS scaffold, FastifyAdapter, config module đọc env qua Zod)
- Create: `apps/web/` (Vite scaffold, MUI theme từ design tokens của `public/assets/tokens.css`)
- Create: `packages/shared/` (Zod schemas, hằng số socket event names)
- Create: `.github/workflows/ci.yml` (lint + typecheck + test)

## Implementation Steps
0. **Abstraction layer hạ tầng ngay từ đầu (user chốt 12/07 — 2 deployment profile, chi tiết `research/queue-decision.md`)**: interface `StateDriver` / `QueueDriver` / `StorageDriver` / `SocketAdapterFactory` — profile compose: BullMQ + Redis + MinIO; profile **portable Windows không Docker (LAN, 1 instance)**: in-process FIFO + pg-boss + filesystem. Mọi code nghiệp vụ chỉ gọi qua interface; chọn bằng env `INFRA_PROFILE=compose|portable`.
   - **`StorageDriver` contract phải TRỪU TƯỢNG, không theo presigned-URL** (gap-sweep H-F2): `getUploadTarget()` (presigned PUT với MinIO / API upload endpoint với filesystem) + `getReadUrl(ttl)` (presigned GET / **HMAC-signed route** có auth, **hỗ trợ Range request + streaming** cho video — không đọc nguyên Buffer). Mọi mô tả "presigned URL" ở phase-03/spec hiểu là contract này.
   - **Quy ước đọc plan**: mọi chỗ plan ghi "Redis X" (INCR, SETNX, TTL, sliding window, cache) hiểu là **StateDriver** — compose = Redis, portable = in-memory cùng semantics. Contract StateDriver tối thiểu: atomicIncr, setNx, expire/TTL, slidingWindow (gap-sweep M-F2 — cấm code đường tắt bypass driver).
   - **Cấm API chỉ-có-trong-secure-context ở apps/web** (`crypto.randomUUID`, `navigator.clipboard`...) — portable chạy http LAN; thêm ESLint restrict (gap-sweep L-F7).
1. Init pnpm workspace 3 package; tsconfig strict, path alias `@shared/*`.
2. Compose theo quy ước user (12/07): **`compose.yml`** cho dev — postgres:16, redis:7, minio (+bucket init), api, web, **proxy nginx**, map TOÀN BỘ port service ra host **bind `127.0.0.1`** (tiện debug nhưng không mở ra LAN/Wi-Fi công cộng với default credentials — gap-sweep M-F3); **`compose.prod.yml`** — cùng services nhưng **chỉ expose 1 port của proxy** (80/443), còn lại internal network. Dev cũng đi qua proxy để đồng dạng prod (cùng domain/cookie).
3. Scaffold NestJS với `FastifyAdapter`; module `ConfigModule` validate env bằng Zod; healthcheck `/healthz`.
4. **SPIKE (timebox 1 ngày — DEFERED D9, mở rộng theo red-team H7)**: trên FastifyAdapter phải PASS đủ CẢ HAI: (a) Socket.IO qua custom `IoAdapter` trên `app.getHttpServer()` — connect + echo + room broadcast; (b) **Better-auth** mount handler + đăng nhập username/password + đọc session từ cookie. Fail bất kỳ cái nào → chuyển `ExpressAdapter` (chỉ đổi `main.ts` + dependency), ghi journal lý do.
5. Cài `@socket.io/redis-adapter` + verify broadcast giữa 2 instance api (chạy 2 port); thử `createShardedAdapter` (Redis 7) — không chạy được với Redis đang dùng thì fallback adapter thường (red-team L5).
6. Scaffold Vite React app: router (`/login`, `/contestant`, `/viewer`, `/overlay`, `/admin`, `/questions`, `/mc`), MUI theme, Zustand store rỗng, socket client singleton (websocket-only transport).
7. CI: pnpm install cache, lint (eslint+prettier), `tsc --noEmit`, vitest. ESLint rule cấm literal string tiếng Việt trong JSX (`react/jsx-no-literals` giới hạn ở apps/web) để giữ kỷ luật tách string ra `vi.ts` (DEFERED D2b, red-team L7).

## Success Criteria
- [ ] `docker compose up` + `pnpm dev` → api :3000, web :5173 chạy.
- [ ] Spike Socket.IO trên Fastify PASS (hoặc quyết định fallback được ghi lại) — client connect, join room, nhận broadcast qua Redis adapter với 2 instance.
- [ ] CI xanh trên PR đầu tiên.

## Risk Assessment
- **nest#14953** (cao): đã có kế hoạch spike + fallback ở step 4.
- Windows dev environment (docker/pnpm path issues): dùng WSL2 nếu gặp vấn đề, ghi vào README.
