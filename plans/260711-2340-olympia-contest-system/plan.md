---
title: "Olympia Contest System — Quản lý & mô phỏng Đường lên đỉnh Olympia (luật 2026)"
status: in-progress
created: 2026-07-11
mode: deep
blockedBy: []
blocks: []
---

# Olympia Contest System

Nền tảng web tổ chức thi đấu gameshow kiến thức **tuỳ biến hoàn toàn** theo mô hình Đường lên đỉnh Olympia: round playlist tuỳ ý (số vòng/loại vòng/thứ tự), 1-12 thí sinh hoặc theo đội, mọi timer/điểm là config (luật O26 chỉ là preset mặc định), kho đề + bộ đề bảo mật có chia sẻ, thi đấu realtime độ trễ thấp, overlay OBS + màn MC cho livestream, theming đồ hoạ/âm thanh per contest. *(Scope mở rộng 12/07 từ "mô phỏng O26" — chi tiết `research/ruleconfig-v2-spec.md`.)*

## Tài liệu

| File | Nội dung |
|---|---|
| `research/ruleconfig-v2-spec.md` | **Spec engine chính**: RuleConfig v2 — round playlist, 1-12 ghế + đội, mọi timer/điểm tuỳ biến, rút đề, sound slots, theming (12/07) |
| `research/rules-2026.md` | Luật gốc O26 + edge-cases (nguồn cho preset `O26_DEFAULT`; D8 chờ user xác nhận) |
| `research/tech-architecture.md` | Research kiến trúc: Better-auth, game state, buzzer, MinIO, import/export (§1 Fastify đã superseded — D9 chốt Express) |
| `research/sound-cues.md` | **Cue map event-driven** từ 36 cue của Athena + chương trình thật (12/07 — D10) |
| `research/athena-scout.md` | Bài học từ bản C#/WPF cũ (Athena) |
| `research/ux-gaps.md` | Các khía cạnh UX/vận hành bị bỏ quên (sound cues, pause/undo, grace reconnect...) |
| `research/red-team.md` | Findings red-team + trạng thái xử lý (C1-C3, H1-H10, M, L) |
| `research/product-gaps.md` | Gap-analysis vòng 2: pháp lý/privacy, quy trình con người, vòng đời sản phẩm |
| `research/queue-decision.md` | Queue + 2 deployment profile: BullMQ (compose) / in-process + pg-boss (portable), quy ước compose.yml/compose.prod.yml (12/07) |
| `PRD.md` / `user-stories.md` | Yêu cầu sản phẩm + user stories theo epic (map về phase) |
| `DEFERED.md` | Các quyết định chờ user chốt (D1-D21; nhiều mục đã ✅) |
| `../../public/` | Demo tĩnh mock data + animation (deliverable của giai đoạn planning, host Vercel được) |

## Stack (đã chốt, không đổi)

- **Backend**: TypeScript, NestJS trên **Express adapter** (✅ D9 chốt 12/07 — đổi từ Fastify để tương thích Socket.IO gateway + Better-auth), Zod, Prisma + PostgreSQL, Redis, Better-auth, Socket.IO
- **Frontend**: TypeScript, React + Vite, MUI, Motion for React, Zustand, TanStack Query, Socket.IO client, Zod
- **Storage**: MinIO (S3-compatible) cho media đề
- **Cấu trúc dự kiến khi code**: monorepo pnpm — `apps/api`, `apps/web`, `packages/shared` (Zod schemas + RuleConfig dùng chung)

## Kiến trúc tổng quan

```
                    ┌─────────────────────────────────────────┐
                    │              apps/web (React)           │
                    │  /contestant  /viewer  /overlay  /admin │
                    │  /questions  /mc  (route theo role)     │
                    └──────┬──────────────────────┬───────────┘
                     HTTP (TanStack Query)   Socket.IO (websocket-only)
                    ┌──────┴──────────────────────┴───────────┐
                    │        apps/api (NestJS + Express)      │
                    │  REST: auth, users, questions, contests │
                    │  WS Gateway: match rooms, buzzer, timer │
                    │  Game Engine: state machine per round   │
                    └──┬───────────┬───────────┬──────────────┘
                       │           │           │
                  PostgreSQL     Redis       MinIO
                  (Prisma)   (match state,  (media đề,
                              socket adapter, presigned URL)
                              buzzer order)
```

Nguyên tắc xương sống (rút từ nghiên cứu + bài học Athena):

1. **Server-authoritative tuyệt đối**: timer, thứ tự bấm chuông (server timestamp), điểm số đều tính ở server; client chỉ gửi intent và render.
2. **Đáp án chỉ rời server tới admin + MC** (authenticated + audit); ngoại lệ revealAnswerAfterJudge (per-match, default tắt ở official) đẩy xuống SAU khi chấm; media qua presigned URL TTL ngắn; audit log truy cập đề.
3. **RuleConfig JSON (Zod-validated)** — mọi timer/điểm là config per-contest với preset "O26 chuẩn"; không hard-code như Athena.
4. **Event-sourced match log**: mọi sự kiện trận (chuông, đáp án, chấm điểm, chỉnh tay) được append vào log → undo/sửa điểm có dấu vết, phân xử khiếu nại, replay.
5. **Snapshot đề vào trận**: gán đề là copy tại thời điểm gán; sửa kho đề không phá trận.

## Phases

| # | Phase | Status | Phụ thuộc |
|---|-------|--------|-----------|
| 1 | [Foundation & Infrastructure](phase-01-foundation.md) | pending | — |
| 2 | [Auth & User Management](phase-02-auth-users.md) | pending | 1 |
| 3 | [Question Bank (kho đề)](phase-03-question-bank.md) | pending | 2 |
| 4 | [Import / Export đề](phase-04-import-export.md) | pending | 3 |
| 5 | [Contest & Room Management](phase-05-contest-room.md) | pending | 2 |
| 6 | [Game Engine (luật 2026)](phase-06-game-engine.md) | pending | 3, 5 |
| 7 | [Contestant UI](phase-07-contestant-ui.md) | pending | 6 |
| 8 | [Admin Control UI](phase-08-admin-ui.md) | pending | 6 |
| 9 | [Viewer UI + OBS Overlay](phase-09-viewer-overlay.md) | pending | 6 |
| 10 | [Hardening, Ops & Release](phase-10-hardening.md) | pending | 7, 8, 9 |

Ghi chú thứ tự: Phase 3-4 (kho đề) và Phase 5 (contest/room) có thể chạy song song sau Phase 2. Phase 7-9 song song sau Phase 6, tái sử dụng design từ `public/`.

**Gate trước Phase 7-9 (gap-analysis 6.1):** đem demo `public/` (host Vercel) cho **người thật dùng thử** — tối thiểu 1 giáo viên tạo thử câu hỏi trên màn kho đề + 2 học sinh thi thử màn thí sinh, 30 phút. Toàn bộ review đến nay là AI-review-AI; phản hồi người thật rẻ nhất tại thời điểm này, đắt dần theo mỗi phase code thật.

## Rủi ro chính

| Rủi ro | Mức | Giảm thiểu |
|---|---|---|
| ~~NestJS Fastify + Socket.IO incompatibility~~ | ĐÃ HOÁ GIẢI | D9 chốt Express adapter (12/07) — gateway + Better-auth đều đường chính thống |
| Engine stateful × multi-instance (race, timer đôi) | Cao | Single-writer per match qua Redis lease; failover restore từ snapshot (phase-06, red-team C1) |
| Redis chết giữa trận live | Cao | AOF everysec + degraded mode auto-pause + khôi phục từ Postgres; chaos drill Phase 10 (red-team C3) |
| Luật 2026 sai chi tiết (nguồn mâu thuẫn) | Trung | Mọi giá trị là RuleConfig; user confirm D8; sửa preset không sửa code |
| Better-auth + NestJS là package cộng đồng | Trung | Pin version, wrap sau interface `AuthService` riêng để thay được |
| Buzzer fairness qua Internet | Trung | Server timestamp + websocket-only; khuyến cáo LAN khi thi thật |
| Overlay lag trong OBS | Thấp | Chỉ transform/opacity; test OBS thật ở Phase 9 |

## Demo

`public/` đã build xong và **được verify bằng Playwright** (6 trang render đúng, console 0 error, luật điểm mock áp đúng: +10/−5 khởi động, VCNV 80/60/40/20, tăng tốc 40/30/20/10). Lưu ý: demo hard-code default D8 — nếu user chốt D8 khác, cập nhật `public/assets/engine.js` (RULES) trước khi dùng demo làm design reference cho Phase 7-9 (red-team L2).

## Open questions

Xem `DEFERED.md` (cùng thư mục). **Đã chốt:** D1 (1-12 ghế + đội), D2 (chỉ tiếng Việt + UTC+7), D3 (admin điều khiển + RBAC/CASL), D4 (admin chấm miệng, auto-match câu gõ), D5 (viewer public theo mã phòng), D9 (**Express adapter** — bỏ Fastify), D15-MC, D17.2 (last-wins), D18 (không cắt scope), D19 (portable HTTP, <10 viewer), D20 (skip rỗng + trim). **Còn chờ:** D6 (giới hạn media), **D7 (tên + bản quyền + license — trước commit code)**, **D8 (mâu thuẫn luật O26)**, D10 (cue map đang research — admin upload sau), D11, D12, D13, D14, D15.1, D16, **D17.1/.3/.4 (ngữ nghĩa thi đội — cần trước Phase 6)**, D21 (dual-purpose). Mỗi mục có khuyến nghị tạm để không chặn tiến độ.
