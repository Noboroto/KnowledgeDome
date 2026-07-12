# Research — Kiến trúc kỹ thuật realtime quiz (stack đã chốt)

> Tổng hợp từ researcher (07/2026). Stack hiện hành: NestJS + **Express adapter** (✅ D9 12/07 — thay Fastify), Zod, Prisma+Postgres, Redis, Better-auth, Socket.IO / React+Vite, MUI, Motion, Zustand, TanStack Query / MinIO.

## 1. NestJS + Fastify + Socket.IO — ✅ SUPERSEDED (12/07): D9 chốt ĐỔI EXPRESS ADAPTER, mục này chỉ còn giá trị lịch sử

- **Issue mở nestjs/nest#14953**: `@nestjs/platform-socket.io` không expose endpoint đúng khi dùng FastifyAdapter (tình trạng đến 2025-2026 vẫn mở).
- Hướng xử lý (xem DEFERED D9): (a) custom `IoAdapter` gắn Socket.IO vào `http.Server` bên dưới Fastify — **khuyến nghị, cần spike Phase 1**; (b) fallback Express adapter; (c) tách socket ra port/process riêng.
- Scale ngang: `@socket.io/redis-adapter`; Redis 7 dùng `createShardedAdapter` (~3x throughput pub/sub).
- Latency tuning: `transports: ['websocket']` (bỏ polling — polling gây variance không công bằng cho buzzer), `maxHttpBufferSize` nhỏ (10KB), pingInterval/pingTimeout ngắn (5s/3s) để phát hiện rớt nhanh.
- Không emit full state mỗi tick — chỉ delta + state-sync khi join/reconnect.

## 2. Better-auth trong NestJS

- Package cộng đồng `@thallesp/nestjs-better-auth` (yêu cầu better-auth ≥ 1.5) — hoạt động tốt nhưng KHÔNG official → pin version, test khi upgrade.
- Better-auth có **username plugin** chính thức cho username+password.
- Session qua HttpOnly cookie; **WebSocket handshake auth**: đọc cookie từ `socket.handshake.headers.cookie` → `auth.api.getSession()` → gắn `socket.data.user`, disconnect nếu fail.
- RBAC: bảng Role riêng (ADMIN / SETTER / CONTESTANT / VIEWER), guard đọc metadata; quyền điều khiển trận là permission theo contest (xem DEFERED D3).

## 2b. @casl/ability cho lớp RBAC permission (bổ sung 12/07 theo yêu cầu user)

Đánh giá: **NÊN DÙNG** — khớp chính xác mô hình đã chốt (role = tập permission, user nhiều role, role động trong DB):

- **Cookbook "roles with persisted permissions"** của CASL đúng y bài toán này: load permission rows từ DB → `AbilityBuilder` build `Ability` per-user → check `ability.can('update', subject)`.
- **Attribute-based conditions** giải quyết gọn các quyền có điều kiện mà permission-key thuần không tả được: "setter chỉ sửa câu CỦA MÌNH" = `can('update', 'Question', { createdById: user.id })`; "host của contest X" = `can('control', 'Contest', { id: contestId })`.
- **`@casl/prisma`**: `accessibleBy(ability).Question` sinh Prisma `where` tự động — list kho đề của setter tự lọc theo quyền, không viết filter tay (nhớ `$extends(createCaslExtension())`).
- **Isomorphic + `@casl/react`**: định nghĩa action/subject types trong `packages/shared`; server `packRules()` gửi rules đã nén cho client sau login → FE dùng `<Can I="create" a="Question">` ẩn/hiện nút — một nguồn sự thật quyền cho cả 2 đầu (check thật vẫn ở server).
- Lưu ý: (1) output `packRules` là format private — chỉ dùng qua `unpackRules`, không tự parse; (2) cache Ability per-user ở Redis, invalidate khi đổi role (đã có trong plan); (3) WS guard dùng chung `ability` gắn vào `socket.data`.

Nguồn: https://casl.js.org (cookbook roles-with-persisted-permissions, package casl-prisma, casl-react — tra qua Context7 /stalniy/casl).

## 3. Server-authoritative game state

- State trận trong Redis (key `match:{id}:state`, TTL), server là nguồn sự thật duy nhất; client chỉ gửi intent.
- Timer chạy ở server (interval tick ~100-250ms broadcast `remainingMs` + server timestamp); client chỉ render mượt bằng interpolation, KHÔNG tự đếm.
- Validate mọi submit: đúng phase, đúng quyền, answer id hợp lệ, ghi server timestamp.
- Reconnect: lưu session `user:{id}:match` TTL grace 2 phút; khi connect lại → join room + emit `state-sync` đầy đủ.

## 4. Buzzer fairness

- **Xếp hạng bấm chuông theo server-received timestamp**, tuyệt đối không dùng client timestamp.
- Redis `INCR` counter làm thứ tự nguyên tử; `SADD` chống bấm lặp.
- Latency compensation hoàn hảo là bất khả thi qua Internet — chấp nhận, giảm thiểu bằng websocket-only + hạ tầng gần thí sinh (LAN khi thi thật).

## 5. Bảo mật kho đề

- **Đáp án không bao giờ rời server trước lúc công bố** — DTO gửi client tách riêng khỏi entity, không có field answer.
- Cân nhắc mã hoá cột đáp án ở tầng app (AES) — lưu ý key rotation phải migrate tay.
- Media: MinIO **presigned GET URL TTL 30-60 phút**, bucket private, Content-Disposition inline.
- Audit log: ai xem/sửa/xuất câu hỏi, khi nào.

## 6. OBS Browser Source overlay

- Trang 1920×1080, `background: transparent`, OBS Browser Source bật Hardware Acceleration, "Shutdown source when not visible".
- Animation chỉ dùng `transform`/`opacity` (GPU), `will-change`; tránh blur/box-shadow động; 30fps đủ nếu nội dung ít thay đổi.
- Overlay là client Socket.IO read-only (join room như viewer đặc biệt), tự reconnect.

## 7. Import/export đề

- Khuyến nghị: **ZIP bundle** = `manifest.json` (version, metadata) + `questions.json` (schema Zod riêng của hệ thống) + thư mục `media/`.
- Hỗ trợ thêm: import Excel/CSV (mapping cột linh hoạt, không hard-code vùng ô như Athena cũ); cân nhắc Moodle XML ở v2 nếu cần trao đổi với LMS.
- Export phải roundtrip được (export → import ra kết quả tương đương).

## 8. Prisma schema patterns

- Question versioning: pattern `supersededBy` (tạo bản ghi mới thay vì update in-place), status DRAFT/ACTIVE/ARCHIVED.
- Metadata: difficulty (int), topics (string[]), type, round-type.
- Contest snapshot: khi gán đề vào trận, snapshot câu hỏi (không reference sống) để sửa kho đề không phá trận đã diễn ra.
- Multi-tenant (organization) là YAGNI cho v1 — một tổ chức duy nhất; giữ cột `createdBy` + RBAC là đủ.

## Checklist test hiệu năng (đưa vào phase cuối)

- 100+ client Socket.IO đồng thời không mất gói; 50 buzzer đồng thời xếp hạng đúng theo server timestamp.
- Reconnect giữa trận → state sync đúng trong grace 2 phút.
- Overlay 1920×1080 chạy trong OBS < 50% CPU.
- Export → import roundtrip giữ nguyên dữ liệu.

## Nguồn chính

- https://github.com/nestjs/nest/issues/14953
- https://socket.io/docs/v4/ , https://deepwiki.com/socketio/socket.io-redis-adapter
- https://better-auth.com/docs/integrations/nestjs , https://github.com/thallesp/nestjs-better-auth
- https://www.gabrielgambetta.com/client-side-prediction-server-reconciliation.html
- https://websocket.org/guides/reconnection/
- https://docs.moodle.org/501/en/Import_questions
