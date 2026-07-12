---
phase: 8
title: "Admin Control UI"
status: pending
priority: P1
dependencies: [6]
---

# Phase 8: Admin Control UI

## Overview
Bàn điều khiển trận cho admin: điều khiển vòng thi/câu hỏi/timer, chấm điểm, can thiệp (adjust/undo/skip/pause), quản lý viewer (khoá cổng/kick — viewer public không duyệt), giám sát kết nối. Port design từ `public/admin.html`.

**Bổ sung 12/07:**
- **Contest builder UI**: trình dựng round playlist (thêm/xoá/sắp xếp vòng, form config từng loại vòng theo RuleConfig v2, chọn preset rồi tuỳ biến), setup seats/teams (1-12, kéo thả vào đội), theme editor (màu theo design tokens, upload logo/ảnh/video hình hiệu), sound editor (gán file cho từng cue slot theo thành phần, playlist nhạc nền).
- **Soundboard** trong trận: phát/dừng/next nhạc nền, volume/duck, mute nhanh — nằm ngoài control lock (co-host phụ trách được).
- Stepper vòng thi render theo playlist (không cứng 5 bước); panel rút đề hiển thị kết quả `QUESTIONS_DRAWN` + pool còn lại.

## Requirements
- Functional: stepper theo round playlist (không cứng số vòng); hiển thị câu hỏi + đáp án (chỉ admin); chấm Đúng/Sai + xác nhận kết quả auto-match; chỉnh điểm tay (kèm lý do bắt buộc); undo; skip/thay câu dự phòng; start/pause/reset timer + chỉnh thời gian nhanh; ViewerPanel (đếm/kick/khoá cổng); ConnectionMonitor (thí sinh theo ghế 1,2,3... — online/ping; viewer/MC chỉ số lượng); pre-flight checklist trước start; sự kiện log realtime (chuông/đáp án kèm ms); MC cue script theo vòng (P2); tạo match matchPurpose='practice' + nút Rematch (v1 — dual-purpose 12/07).
- Non-functional: mọi hành động phá huỷ (reset, force-end, undo) có confirm; thao tác thường có hotkey (`C` đúng, `X` sai, `N` next...); hiển thị trạng thái kết nối từng thí sinh (ping, online).

## Architecture
- Route `/admin/match/:id`; cùng `matchStore` pattern nhưng subscribe thêm channel admin (`match:{id}:admin`) nhận dữ liệu nhạy cảm (đáp án, log chi tiết, ping).
- Mọi nút = gửi `AdminEvent` lên engine (Phase 6) — UI không tính toán luật, chỉ hiển thị kết quả từ server (single source of truth).
- Layout 3 cột như demo: trái (điều hướng vòng + phòng + ViewerPanel), giữa (câu hỏi/đáp án/chấm/timer), phải (bảng điểm + event log).

## Related Code Files
- Create: `apps/web/src/pages/admin/` (MatchControl, RoundStepper, JudgingPanel, TimerControl, ScoreAdjustDialog, ViewerPanel, EventLog, PreflightChecklist, ConnectionMonitor)
- Tham chiếu design: `public/admin.html`

## Implementation Steps
1. Khung 3 cột + RoundStepper + trạng thái match (LOBBY/LIVE/PAUSED...) + **control lock**: hiển thị ai đang cầm quyền điều khiển, nút "Take control" (người còn lại read-only — red-team H1).
2. JudgingPanel: câu hỏi + media preview + đáp án + nút Đúng/Sai/hotkey + kết quả auto-match chờ confirm. Riêng tăng tốc ranked-speed: **TangTocJudgingGrid** — sau TIME_UP hiện grid N bản-cuối (per đơn vị điểm) với kết quả auto-match, admin confirm/override hàng loạt; realtime trước TIME_UP chỉ hiện bản mới nhất per seat (collapse — gap-sweep H-F4d).
3. TimerControl (start/pause/+10s/−10s/reset, đổi duration câu hiện tại) + ScoreAdjustDialog (delta + reason bắt buộc) + Undo (chọn event trong log).
4. **ViewerPanel** (thay hàng chờ duyệt viewer cũ — user chốt 12/07 viewer public không cần duyệt): đếm viewer đang xem + danh sách kick được + nút "khoá cổng viewer"; nằm NGOÀI control lock — co-host phụ trách song song (gap 2.4, crew ≥2).
4b. **ConnectionMonitor** (user chốt 12/07): admin/role có permission `contest.control` thấy realtime — **thí sinh theo GHẾ số 1, 2, 3...** (online/offline, ping ms, trạng thái tech-check); **viewer + MC chỉ hiện SỐ LƯỢNG online/offline**, không phân biệt thứ tự; cảnh báo nổi khi thí sinh rớt mạng giữa trận.
5. PreflightChecklist (kết quả validate đề theo spec v2 §12 + tech-check mọi thí sinh) — nút Start chỉ mở khi đủ điều kiện (force-start có confirm 2 bước; riêng everPublic-block force cần confirm 2 bước + audit).
6. EventLog ảo hoá (react-virtuoso) + filter; ConnectionMonitor ping từng ghế.
7. E2E: admin điều khiển trọn trận với 4 contestant giả.

## Success Criteria
- [ ] Admin chạy trọn trận không rời trang; các can thiệp (adjust/undo/skip/pause) phản ánh đúng xuống mọi client < 300ms.
- [ ] Không thể start khi pre-flight fail (trừ force có confirm).
- [ ] Sự kiện chuông hiển thị ms và thứ hạng — đủ để phân xử khiếu nại tại chỗ.

## Risk Assessment
- Admin bấm nhầm giữa trận → confirm cho hành động phá huỷ + undo được hầu hết thao tác.
- Quá tải thông tin → tham khảo demo đã được user chỉnh design; ưu tiên keyboard flow cho thao tác lặp (chấm điểm).
