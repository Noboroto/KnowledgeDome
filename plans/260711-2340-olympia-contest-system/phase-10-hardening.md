---
phase: 10
title: "Hardening, Ops & Release"
status: pending
priority: P1
dependencies: [7, 8, 9]
---

# Phase 10: Hardening, Ops & Release

## Overview
Biến hệ thống chạy được thành hệ thống tin cậy được: load test, security audit, monitoring, xuất kết quả, thống kê, tài liệu triển khai.

## Requirements
- Functional: xuất kết quả trận PDF (bảng điểm, timeline sự kiện chính, QR verify); thống kê câu hỏi (% đúng, thời gian trả lời TB) cập nhật về kho đề; replay timeline trận (P2); dashboard monitoring cho admin.
- Non-functional: chịu tải mục tiêu DEFERED D11 (≤500 viewer + 2 contest song song); pass security checklist; backup DB tự động; một lệnh deploy.

## Related Code Files
- Create: `apps/api/src/reports/` (pdf.service — @react-pdf hoặc puppeteer, stats.service)
- Create: `apps/api/src/monitoring/` (metrics: Socket.IO ping histogram, connection count, event rate — Prometheus format)
- Create: `deploy/` (compose.prod.yml, **nginx** reverse proxy + TLS certbot, backup script pg_dump + MinIO mirror)
- Create: `docs/deployment.md` (kèm NTP/chrony, staging compose thứ 2 — gap 3.1/5.1), `docs/runbook.md` (xử lý sự cố giữa trận: pause → chẩn đoán → restore; crew tối thiểu 2 người; checklist ngày thi; quy trình xác minh danh tính thí sinh offline)
- Create: `docs/guide-admin.md`, `docs/guide-setter.md`, `docs/guide-contestant.md` — hướng dẫn NGƯỜI DÙNG cuối (giáo viên/học sinh, không phải dev — gap 3.2); guide contestant tích hợp vào màn lobby; kèm mẫu consent phụ huynh cho livestream (gap 1.2)

## Implementation Steps
1. Load test (k6 + socket.io client): kịch bản preset O26 (4 thí sinh) + kịch bản `TEAM_12@1` (12 thiết bị thí sinh) + 500 viewer, buzzer storm 50 đồng thời; đo p95 latency event < 200ms LAN / < 500ms Internet; tìm bottleneck.
1b. **Chaos drills giữa trận** (red-team C1/C3/M8): (a) kill instance đang own match → instance kia giành lease, trận PAUSE rồi resume đúng state; (b) kill Redis (AOF everysec bật) → engine tự PAUSE, Redis về → khôi phục từ PG, resume; (c) restore drill từ backup pg_dump + MinIO mirror ra môi trường sạch — trận cũ replay được. Backup chưa restore thử = không có backup.
2. Security pass: quét rò đáp án mọi endpoint/socket payload (test tự động từ Phase 3 chạy lại toàn hệ); rate limits; helmet/CSP; dependency audit; kiểm tra IDOR trên contest/question id (cuid đủ, thêm authz check test); **Bull Board/pg-boss dashboard sau auth admin, không expose ở prod** (gap-sweep L-F4).
3. Monitoring + alert đơn giản (lag/error spike hiện trên admin UI) + **disk usage với ngưỡng cảnh báo** (kiến trúc persist-trước-broadcast nghĩa là disk đầy = trận đứng hình — gap 5.2) + logging chuẩn hoá **pino** (native Fastify) qua `nestjs-pino` (request-context trong mọi log) + `pino-roll` rotation (đã check Context7 12/07 — cả 3 reputation High) + trạng thái backup gần nhất (backup fail phải NHÌN THẤY, không âm thầm — gap 3.4); chaos drill bổ sung: disk 95%.
4. PDF kết quả + stats câu hỏi ghi ngược metadata kho đề.
5. Replay timeline (đọc MatchEvent log, UI scrub) — P2, làm nếu còn thời gian.
6. Deploy — **2 profile (user chốt 12/07, chi tiết `research/queue-decision.md`)**: (a) **`compose.prod.yml`** — api ×2 + BullMQ/Redis adapter + MinIO + **proxy nginx là port DUY NHẤT expose (80/443, TLS qua certbot/lego)**, mọi service khác internal; backup cron; (b) **portable Windows không Docker cho LAN**: bundle 1 thư mục (node runtime + app build + **Postgres portable binaries zip** + pg-boss + in-process queue + filesystem storage — qua abstraction layer Phase 1), script `start.bat`, không cần cài đặt. Runbook sự cố cho cả 2 profile.
7. UAT: tổ chức 1 trận thật end-to-end với người thật + OBS livestream thử.

## Success Criteria
- [ ] Load test đạt chỉ tiêu; không memory leak sau soak 2h.
- [ ] **Portable (gap-sweep H-F5/M-F4)**: kill process giữa VCNV → `start.bat` → trận resume đúng; `start.bat` gồm `prisma migrate deploy` + mở Windows Firewall rule (netsh) + in URL/QR IP LAN + tắt PG sạch khi đóng; script backup 1-click (pg_dump + copy thư mục storage); load test LAN đạt target: 12 thí sinh + 30 viewer (✅ D19.2 — thực tế user cho biết <10 viewer, 3× headroom); runbook khuyến cáo Ethernet cho thí sinh, chuột rời đồng nhất.
- [ ] Bộ test "không rò đáp án" pass trên toàn bộ API + socket (bao gồm invariant submission chéo — red-team H10).
- [ ] Chaos drills 1b pass cả 3 kịch bản (failover lease, Redis chết, restore từ backup) — không event nào client đã thấy bị mất (red-team H8).
- [ ] PDF kết quả đúng với bảng điểm; UAT trận thật thành công.

## Risk Assessment
- UAT lộ vấn đề UX thật (độ trễ cảm nhận, âm thanh) → chừa buffer 1 tuần sau UAT trước release.
- Windows host cho OBS + server cùng máy khi thi thật → khuyến nghị tách máy, ghi vào runbook.
