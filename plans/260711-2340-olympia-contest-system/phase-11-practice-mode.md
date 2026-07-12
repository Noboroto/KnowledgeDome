---
phase: 11
title: "Practice Mode (release v1.5)"
status: pending
priority: P1
dependencies: [10]
---

# Phase 11: Practice Mode — release v1.5

## Overview
Kích hoạt mục đích thứ hai của hệ thống: **luyện tập/rehearsal** ngang hàng contest chính thức (user chốt 12/07, spec §13). Toàn bộ SCHEMA đã có sẵn từ v1 (`Match.matchPurpose`, `Question.visibility`, `everPublic`, ACL bộ đề, retention fields — nguyên tắc DB-đủ-từ-v1 của D18); phase này bật engine-path, policy bundle và UI tương ứng. Engine/luật/điểm KHÔNG đổi — practice chỉ khác policy.

## Requirements
- Functional:
  - `matchPurpose: 'practice'` chọn lúc tạo match (immutable sau start, default `official`), áp **policy bundle spec §13**: pre-flight thiếu câu/media/tech-check chỉ WARNING (vẫn chặn lỗi cấu trúc Zod); `revealAnswerAfterJudge` default ON (tắt được); lobby optional + nút **Rematch** (giữ seats + room code, về LOBBY rút gọn); anti-cheat (fullscreen/visibilitychange) OFF; không ghi ngược thống kê kho đề, không vào podium/kết quả chính thức; PDF watermark "LUYỆN TẬP", không QR (✅ D21.3).
  - **Trainer role** (✅ D21.2): tạo practice match cần `contest.create` — admin gán role "trainer" bằng RBAC sẵn có (seed thêm 1 role, 0 công engine).
  - **Retention riêng** (✅ D21.1): background job (queue phase-01) tự anonymize/xoá practice match sau **3 tháng** (config); official giữ **12 tháng** (config) — dùng chức năng anonymize sẵn từ phase-06; trạng thái job hiện trên admin monitoring (phase-10).
  - **Bộ đề PUBLIC + share-link** (✅ D16 — schema từ phase-03, nay bật tính năng): set PUBLIC xem/tải tự do kèm đáp án theo setting per-set (default có), cảnh báo + confirm trước khi public, cờ `everPublic` một chiều + pre-flight hard-block cho match official (spec §9 — đã vá C-v2-1); share-link password + TTL + revoke, mặc định KHÔNG kèm đáp án, rate-limit + audit token/IP.
  - **UI luyện tập solo** (D16): contest cá nhân + practice match 1 ghế từ đề public — tự bấm giờ/tự chấm, không cần admin điều khiển (chế độ auto-advance đơn giản của orchestrator).
- Non-functional: mọi nhánh policy theo `matchPurpose` là **config check, không fork code engine** (spec §13: "purpose KHÔNG phải mode engine riêng"); viewer public/zero-trust/server-authoritative/TRIM/last-wins/preload encrypted GIỮ NGUYÊN giữa 2 purpose.

## Related Code Files
- Modify: `apps/api/src/engine/match-orchestrator` (policy bundle theo purpose; auto-advance mode cho solo), `apps/api/src/reports/pdf.service` (watermark), `apps/api/src/questions/` (public set + share-link endpoints), `packages/shared` (không đổi schema — đã đủ từ v1)
- Create: `apps/api/src/retention/` (retention.job — quét match quá hạn theo purpose, gọi anonymize), `apps/web/src/pages/practice/` (tạo practice match nhanh, solo mode, rematch)
- Seed: role `trainer` (permission `contest.create` giới hạn practice)

## Implementation Steps
1. Policy bundle §13 trong orchestrator + pre-flight (warning-mode) + reveal default theo purpose; test: cùng 1 RuleConfig chạy official vs practice chỉ khác policy, điểm y hệt.
2. Rematch (giữ seats + room code, reset state về LOBBY rút gọn) + audit.
3. Retention job (queue): practice 3 tháng / official 12 tháng (env config), gọi anonymize phase-06, log kết quả + hiển thị monitoring.
4. Bật public set + share-link (endpoints + UI kho đề): confirm 2 bước, everPublic, ACL/rate-limit/audit theo spec §9; test tự động "không rò đáp án" chạy lại toàn bộ.
5. UI luyện tập: tạo practice match nhanh (trainer), solo 1 ghế auto-advance, hiển thị đáp án sau chấm (revealAnswerAfterJudge).
6. PDF watermark "LUYỆN TẬP" không QR; chặn ghi stats/podium từ practice match (assert trong reducer/stats service).

## Success Criteria
- [ ] Cùng kịch bản event: official vs practice ra điểm giống hệt nhau; chỉ khác policy (reveal, pre-flight, anti-cheat, stats).
- [ ] Practice match quá 3 tháng bị anonymize tự động (test time-travel); official không bị đụng trước 12 tháng.
- [ ] Set public tải được kèm đáp án theo setting; câu everPublic bị pre-flight chặn khỏi match official (hard-block force = confirm 2 bước + audit).
- [ ] Solo 1 ghế: đi trọn playlist không cần admin; đáp án hiện sau chấm.
- [ ] Bộ test "không rò đáp án" pass lại toàn hệ sau khi mở public/share-link.

## Risk Assessment
- Footgun spec P12: câu CHƯA everPublic + reveal ON + cổng viewer mở → cảnh báo "khuyến nghị khoá cổng viewer" (đã thiết kế, phải có test UI).
- Share-link là bề mặt public mới → rate-limit + audit token/IP bắt buộc trước khi release v1.5.
