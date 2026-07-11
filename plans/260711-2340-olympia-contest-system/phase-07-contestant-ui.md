---
phase: 7
title: "Contestant UI"
status: pending
priority: P1
dependencies: [6]
---

# Phase 7: Contestant UI

## Overview
Giao diện thí sinh: tối giản, phản hồi tức thì, dùng được hoàn toàn bằng bàn phím với hotkey dễ nhớ. Port design từ `public/contestant.html` (đã được user duyệt/sửa) sang React.

## Requirements
- Functional: login → nhập room code → lobby tech-check → thi đủ 5 phần; bấm chuông (`Space`), nhập + gửi đáp án (`Enter`), chọn hàng ngang VCNV (`1-4`), xoá input (`Esc`); hiển thị điểm, trạng thái chuông, timer, kết quả chấm; phát sound cue.
- Non-functional: input-to-feedback cục bộ < 50ms (optimistic UI: nút chuông phản hồi ngay, server confirm sau); bundle nhỏ, không animation nặng; hoạt động tốt trên laptop yếu; fullscreen mềm + log rời tab (P2, theo `research/ux-gaps.md`).

## Architecture
- Route `/contestant`; Zustand store `matchStore` nhận delta qua socket, `state-sync` khi (re)connect.
- Timer render từ `{remainingMs, serverNow}` tick + `requestAnimationFrame` interpolation — không tự đếm bằng clock client.
- Optimistic buzz: bấm → UI khoá nút + hiệu ứng ngay → server trả `buzz-result` (thắng/thua/muộn) → cập nhật. Mọi kết quả cuối theo server.
- Hotkey qua 1 hook `useHotkeys` toàn màn (không phụ thuộc focus input, trừ khi đang gõ đáp án); hint hotkey in trên nút.
- Audio: preload toàn bộ SFX khi vào lobby (unlock audio context bằng cú click "sẵn sàng").

## Related Code Files
- Create: `apps/web/src/pages/contestant/` (MatchScreen, round views: KhoiDong, Vcnv, TangToc, VeDich, TieBreak; BuzzButton, AnswerInput, TimerBar, ScoreStrip)
- Create: `apps/web/src/stores/matchStore.ts`, `apps/web/src/hooks/useHotkeys.ts`, `useServerTimer.ts`, `useSoundCues.ts`
- Tham chiếu design: `public/contestant.html`, `public/assets/tokens.css`

## Implementation Steps
1. Dựng matchStore + socket binding + state-sync/reconnect banner ("đang kết nối lại...").
2. Layout khung + TimerBar + ScoreStrip + BuzzButton (optimistic) theo demo.
3. Round views theo thứ tự engine; VCNV có bàn phím chọn hàng ngang + ô nhập đáp án đếm ký tự.
4. Hotkeys + focus management (auto-focus ô đáp án khi được quyền trả lời); test keyboard-only đi trọn một trận.
5. Sound cues + setting tắt/bật âm lượng.
6. Fullscreen request khi vào trận + log `visibilitychange` gửi server (soft anti-cheat).
7. E2E (Playwright): mô phỏng trận với 4 browser context.

## Success Criteria
- [ ] Đi trọn 1 trận chỉ bằng bàn phím, không cần chuột.
- [ ] Buzz phản hồi thị giác < 50ms cục bộ; kết quả server hiển thị rõ thắng/thua chuông.
- [ ] Rớt mạng 30s giữa câu → banner reconnect → tự khôi phục đúng trạng thái.
- [ ] Lighthouse performance ≥ 90 cho route contestant.

## Risk Assessment
- Autoplay audio bị browser chặn → unlock audio context qua nút "Sẵn sàng" ở lobby.
- Hotkey xung đột khi đang gõ đáp án → scope hotkey theo trạng thái (Space chỉ là chuông khi không focus input).
