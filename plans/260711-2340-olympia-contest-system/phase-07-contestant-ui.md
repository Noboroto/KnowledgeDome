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
- Functional: login → nhập room code → lobby tech-check → thi mọi loại vòng trong playlist; **bấm chuông CHỈ bằng click chuột (user chốt 12/07 — không hotkey chuông, tránh bấm nhầm khi gõ)**; nhập + gửi đáp án (`Enter`), chọn hàng ngang VCNV (`1-8`), xoá input (`Esc`); **tăng tốc: gửi lại tự do, tính bản cuối trước server-timeout, không khoá nút** (rule chống double-submit đã gỡ toàn hệ thống — dedup là việc server); hiển thị điểm, trạng thái chuông, timer, kết quả chấm; phát sound cue.
- Non-functional: input-to-feedback cục bộ < 50ms (optimistic UI: nút chuông phản hồi ngay, server confirm sau); bundle nhỏ, không animation nặng; hoạt động tốt trên laptop yếu; fullscreen mềm + log rời tab (P2, theo `research/ux-gaps.md`).

## Architecture
- Route `/contestant`; Zustand store `matchStore` nhận delta qua socket, `state-sync` khi (re)connect.
- Render theo RuleConfig v2: round view lấy từ playlist (không cứng 5 loại màn); tăng tốc clue-buzz hiện dữ kiện mở dần + chuông; chế độ đội hiển thị tên đội + điểm đội; nhạc nền mặc định TẮT ở contestant (setting).
- Timer render từ `{remainingMs, serverNow}` tick + `requestAnimationFrame` interpolation — không tự đếm bằng clock client.
- Optimistic buzz: bấm → UI khoá nút + hiệu ứng ngay → server trả `buzz-result` (thắng/thua/muộn) → cập nhật. Mọi kết quả cuối theo server.
- Hotkey qua 1 hook `useHotkeys` toàn màn (không phụ thuộc focus input, trừ khi đang gõ đáp án); hint hotkey in trên nút.
- Audio: pre-download toàn bộ SFX khi vào lobby (✅ D10 — file nhỏ, phát tức thì theo `sound-cue`, không phụ thuộc mạng lúc cue; unlock audio context bằng cú click "sẵn sàng").
- **Media mã hoá (✅ D12b)**: service worker cache blob mã hoá câu N+1; nhận `MEDIA_KEY` qua socket lúc reveal → decrypt + play; SW không khả dụng (browser cũ/lỗi) → client tự khai báo, nhận URL lúc reveal (fallback reveal-only) — trận không bao giờ đứng vì SW.

## Related Code Files
- Create: `apps/web/src/pages/contestant/` (MatchScreen, round views: KhoiDong, Vcnv, TangToc, VeDich, TieBreak; BuzzButton, AnswerInput, TimerBar, ScoreStrip)
- Create: `apps/web/src/stores/matchStore.ts`, `apps/web/src/hooks/useHotkeys.ts`, `useServerTimer.ts`, `useSoundCues.ts`
- Tham chiếu design: `public/contestant.html`, `public/assets/tokens.css`

## Implementation Steps
1. Dựng matchStore + socket binding + state-sync/reconnect banner ("đang kết nối lại...").
2. Layout khung + TimerBar + ScoreStrip + BuzzButton (optimistic) theo demo.
3. Round views theo thứ tự engine; VCNV có bàn phím chọn hàng ngang + ô nhập đáp án đếm ký tự.
4. Hotkeys + focus management (auto-focus ô đáp án khi được quyền trả lời); test keyboard-only cho mọi thao tác trừ chuông (chuông chỉ click chuột).
5. Sound cues + setting tắt/bật âm lượng.
6. Fullscreen request khi vào trận + log `visibilitychange` gửi server (soft anti-cheat).
7. E2E (Playwright): mô phỏng trận với 4 browser context.
8. Hiển thị đáp án sau chấm khi match bật revealAnswerAfterJudge (practice — spec §13); anti-cheat (fullscreen/visibilitychange) chỉ chạy khi matchPurpose=official.

## Success Criteria
- [ ] Đi trọn 1 trận bằng bàn phím cho MỌI thao tác TRỪ chuông (chuông = click chuột theo quyết định 12/07; trade-off accessibility đã ghi nhận ở PRD NFR-5).
- [ ] Buzz phản hồi thị giác < 50ms cục bộ; kết quả server hiển thị rõ thắng/thua chuông.
- [ ] Rớt mạng 30s giữa câu → banner reconnect → tự khôi phục đúng trạng thái.
- [ ] Lighthouse performance ≥ 90 cho route contestant.

## Risk Assessment
- Autoplay audio bị browser chặn → unlock audio context qua nút "Sẵn sàng" ở lobby.
- Hotkey xung đột khi đang gõ đáp án → scope hotkey theo trạng thái; chuông KHÔNG có hotkey (chỉ click chuột — quyết định 12/07). **Trong trận Esc chỉ xoá input, tắt Esc=back** — browser dành Esc thoát fullscreen, tránh văng fullscreen + ghi oan log anti-cheat (gap-sweep M-F6); anti-cheat phân biệt fullscreen-exit-do-Esc.
- Touchpad laptop trường (tap debounce, palm-rejection) làm chuông kém nhạy so với chuột rời → runbook ngày thi: **chuột rời đồng nhất cho mọi thí sinh**; lobby tech-check đo cả click latency (gap-sweep M-F9).
