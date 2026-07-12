# Research — Các khía cạnh UX/vận hành thường bị bỏ quên

> Tổng hợp researcher 07/2026, theo yêu cầu user "nghiên cứu những khía cạnh tôi bỏ quên". Đã lồng vào các phase tương ứng.

## Top items đưa vào v1 (P1)

| Khía cạnh | Vì sao | Đưa vào |
|---|---|---|
| Sound cues (đúng/sai/chuông/hết giờ) + nhạc nền từng vòng, preload SFX client | Feedback tức thì là linh hồn gameshow; lag >100ms gây ức chế | Phase 6 (engine phát event) + 7/8/9 (client phát âm thanh) |
| Pause/resume trận + undo/sửa điểm hồi tố | Sự cố giữa trận là chắc chắn xảy ra; sửa điểm phải có dấu vết | Phase 6 (event-sourced match log) + 8 (UI admin) |
| Grace period reconnect (mặc định 120s) | Thí sinh rớt mạng là chuyện thường; kick ngay là bất công | Phase 5/6 |
| Pre-match lobby + tech check (thử chuông, âm thanh, mạng) + validate đề đủ câu trước khi start | Tránh "chết trên sân khấu" | Phase 5 (lobby) + 8 (pre-flight checklist) |
| Preload media câu KẾ TIẾP xuống client trước khi hiển thị (không kèm đáp án) + fallback media hỏng (skip/thay câu dự phòng) | Video lag giữa trận = thảm hoạ livestream | Phase 6 + 9 |
| Audit log ms-precision cho buzzer/chấm điểm + export kết quả PDF | Phân xử khiếu nại "em bấm trước" | Phase 6 + 10 |
| Backup/restore state trận (snapshot Redis→disk chu kỳ ngắn) khi server restart | Server hiccup không được giết trận | Phase 6/10 |
| Monitoring realtime (latency, connections, error) cho admin | Admin mù thì không cứu được sự cố | Phase 10 |

## P2 (nên có, có thể dời v1.1)

- Practice match (matchPurpose='practice' — không vào kết quả/thống kê chính thức) — đã nâng lên v1 — Phase 8.
- Fullscreen enforcement mềm + log rời tab (`visibilitychange`) cho thí sinh; 1 phiên đăng nhập duy nhất — Phase 7.
- MC cue/script hiển thị cho admin theo vòng — Phase 8.
- Skip câu lỗi / thay câu dự phòng giữa trận — Phase 6/8 (đưa lên P1 phần skip).
- Thống kê câu hỏi sau trận (% đúng, thời gian trả lời trung bình) — Phase 10.
- Xử lý thí sinh dropout quá grace: config auto-forfeit vs giữ ghế — Phase 6 (RuleConfig).
- Viewer: chỉnh cỡ chữ, contrast AA, dark/light — Phase 9.
- Replay timeline trận từ event log — Phase 10.

## P3 (để sau)

- Giải đấu nhiều trận (bracket tuần→tháng→quý→năm), season leaderboard.
- Chặn copy đề (giá trị thấp, dễ bypass).
- Soundboard hotkey global (ngoài focus browser).
- Multi-region / CDN cho viewer số lượng lớn.

## Câu hỏi mở → đã ghi DEFERED

- Bản quyền nhạc hiệu Olympia (D10).
- Quy mô viewer đồng thời mục tiêu (D11).

## Nguồn chính

- https://www.mux.com/articles/live-streaming-analytics-the-metrics-that-actually-matter
- https://pro.harman.com/insights/harman-pro/audio-and-video-challenges-in-esports/
- https://ericjinks.com/blog/2025/event-sourcing/
- https://docs.moodle.org/502/en/Quiz_settings (grace period)
- https://help.wayground.com/support/solutions/articles/158000411388 (anti-cheat focus mode)
- https://pusher.com/blog/websockets-realtime-gaming-low-latency/
