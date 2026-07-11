---
phase: 8
title: "Admin Control UI"
status: pending
priority: P1
dependencies: [6]
---

# Phase 8: Admin Control UI

## Overview
Bàn điều khiển trận cho admin: điều khiển vòng thi/câu hỏi/timer, chấm điểm, can thiệp (adjust/undo/skip/pause), duyệt viewer, giám sát kết nối. Port design từ `public/admin.html`.

## Requirements
- Functional: stepper 5 phần thi; hiển thị câu hỏi + đáp án (chỉ admin); chấm Đúng/Sai + xác nhận kết quả auto-match; chỉnh điểm tay (kèm lý do bắt buộc); undo; skip/thay câu dự phòng; start/pause/reset timer + chỉnh thời gian nhanh; duyệt viewer; pre-flight checklist trước start; sự kiện log realtime (chuông/đáp án kèm ms); MC cue script theo vòng (P2); practice mode (P2).
- Non-functional: mọi hành động phá huỷ (reset, force-end, undo) có confirm; thao tác thường có hotkey (`C` đúng, `X` sai, `N` next...); hiển thị trạng thái kết nối từng thí sinh (ping, online).

## Architecture
- Route `/admin/match/:id`; cùng `matchStore` pattern nhưng subscribe thêm channel admin (`match:{id}:admin`) nhận dữ liệu nhạy cảm (đáp án, log chi tiết, ping).
- Mọi nút = gửi `AdminEvent` lên engine (Phase 6) — UI không tính toán luật, chỉ hiển thị kết quả từ server (single source of truth).
- Layout 3 cột như demo: trái (điều hướng vòng + phòng + viewer queue), giữa (câu hỏi/đáp án/chấm/timer), phải (bảng điểm + event log).

## Related Code Files
- Create: `apps/web/src/pages/admin/` (MatchControl, RoundStepper, JudgingPanel, TimerControl, ScoreAdjustDialog, ViewerQueue, EventLog, PreflightChecklist, ConnectionMonitor)
- Tham chiếu design: `public/admin.html`

## Implementation Steps
1. Khung 3 cột + RoundStepper + trạng thái match (LOBBY/LIVE/PAUSED...) + **control lock**: hiển thị ai đang cầm quyền điều khiển, nút "Take control" (người còn lại read-only — red-team H1).
2. JudgingPanel: câu hỏi + media preview + đáp án + nút Đúng/Sai/hotkey + kết quả auto-match chờ confirm.
3. TimerControl (start/pause/+10s/−10s/reset, đổi duration câu hiện tại) + ScoreAdjustDialog (delta + reason bắt buộc) + Undo (chọn event trong log).
4. ViewerQueue realtime approve/reject (một-nhấp) + đếm viewer đang xem. **Duyệt viewer nằm NGOÀI control lock** — co-host duyệt viewer song song trong khi host chính chấm điểm (1 người ôm hết là single point of failure con người; runbook khuyến nghị crew ≥2: host chấm + kỹ thuật OBS/viewer — gap 2.4).
5. PreflightChecklist (kết quả validate đề + tech-check 4 thí sinh) — nút Start chỉ mở khi đủ điều kiện (force-start có confirm 2 bước).
6. EventLog ảo hoá (react-virtuoso) + filter; ConnectionMonitor ping từng ghế.
7. E2E: admin điều khiển trọn trận với 4 contestant giả.

## Success Criteria
- [ ] Admin chạy trọn trận không rời trang; các can thiệp (adjust/undo/skip/pause) phản ánh đúng xuống mọi client < 300ms.
- [ ] Không thể start khi pre-flight fail (trừ force có confirm).
- [ ] Sự kiện chuông hiển thị ms và thứ hạng — đủ để phân xử khiếu nại tại chỗ.

## Risk Assessment
- Admin bấm nhầm giữa trận → confirm cho hành động phá huỷ + undo được hầu hết thao tác.
- Quá tải thông tin → tham khảo demo đã được user chỉnh design; ưu tiên keyboard flow cho thao tác lặp (chấm điểm).
