# User Stories — Olympia Contest System

> Định dạng: **US-x.y** · *Là [role], tôi muốn [hành động] để [giá trị]* · AC = Acceptance Criteria · Phase = nơi hiện thực (xem `plan.md`).
> Nguyên tắc điều khiển (user đã chốt): **admin điều khiển các bước cuộc thi là chính** — thí sinh/viewer chỉ phản ứng theo trạng thái server phát ra, như mô hình AIServer của Athena cũ.

## Epic 1 — Tài khoản & Đăng nhập

| ID | User story | AC chính | Phase |
|---|---|---|---|
| US-1.1 | Là **admin**, tôi muốn tạo tài khoản cho setter/thí sinh (username + password + role) để kiểm soát ai vào hệ thống | Tạo/disable/reset password; không có self-signup; user disabled không đăng nhập được | 2 |
| US-1.2 | Là **thí sinh**, tôi muốn đăng nhập chỉ bằng username+password để vào thi nhanh, không cần email | Form tối giản, autofocus, Enter submit; sai 5 lần/phút bị khoá tạm | 2 |
| US-1.3 | Là **admin**, tôi muốn thí sinh chỉ có 1 phiên hoạt động để tránh nhờ người thi hộ song song | Login mới đá phiên cũ kèm thông báo trên máy cũ | 2, 5 |
| US-1.4 | Là **admin**, tôi muốn logout toàn bộ phiên thí sinh sau trận vì máy trường dùng chung | Nút "logout mọi phiên thí sinh"; session thí sinh tự hết hạn 24h | 2 |
| US-1.5 | Là **admin**, tôi muốn tạo role mới từ danh sách permission và gán nhiều role cho một người để phân quyền linh hoạt (vd "trọng tài" = contest.control + question.viewAnswer) | Role builder chọn permission theo nhóm; 1 user nhiều role; 4 role mặc định không xoá được; đổi role có hiệu lực ngay (cache invalidate) | 2 |

## Epic 2 — Kho đề (Setter + Admin)

| ID | User story | AC chính | Phase |
|---|---|---|---|
| US-2.1 | Là **setter**, tôi muốn tạo câu hỏi đúng loại vòng thi (khởi động/VCNV/tăng tốc/về đích/câu phụ) kèm đáp án và các đáp án chấp nhận được | Form theo loại vòng (VCNV set = ảnh + CNV + 4 hàng ngang có số chữ; về đích có mức 20/30); validate Zod | 3 |
| US-2.2 | Là **setter**, tôi muốn gắn metadata (độ khó 1-5, chủ đề, lớp/kiến thức) để lọc và cân bằng đề | Filter/search theo mọi metadata; chip hiển thị như demo `public/questions.html` | 3 |
| US-2.3 | Là **setter**, tôi muốn đính kèm ảnh/video/audio cho câu hỏi | Upload có progress + preview; chặn file sai loại/quá dung lượng (D6); media hỏng báo ngay lúc upload | 3 |
| US-2.4 | Là **admin**, tôi muốn đáp án không bao giờ lộ cho người không có quyền để bảo mật đề tuyệt đối | Mọi API/socket cho thí sinh/viewer không chứa field đáp án (test tự động); audit log khi admin xem đáp án | 3, 10 |
| US-2.5 | Là **setter**, tôi muốn sửa câu hỏi đã ACTIVE mà không phá trận đã dùng nó | Sửa tạo version mới; trận cũ giữ snapshot | 3, 5 |
| US-2.6 | Là **admin/setter**, tôi muốn export bộ đề ra file và import lại được để chia sẻ/backup | ZIP bundle roundtrip không mất dữ liệu; import báo lỗi từng dòng + preview trước khi ghi; import Excel với mapping cột | 4 |

## Epic 3 — Tạo contest & Phòng thi (Admin)

| ID | User story | AC chính | Phase |
|---|---|---|---|
| US-3.1 | Là **admin**, tôi muốn tạo contest với luật tuỳ chỉnh (timer, điểm từng vòng) từ preset O26 để phù hợp giải của mình | Clone preset → sửa từng tham số qua UI; validate ràng buộc; luật freeze khi trận start | 5, 6 |
| US-3.2 | Là **admin**, tôi muốn gán 4 ghế thí sinh (được để trống ghế) và bộ đề cho contest | Ghế trống hợp lệ (D1b); gán đề = snapshot | 5 |
| US-3.3 | Là **admin**, tôi muốn hệ thống phát mã phòng 6 số để thí sinh/viewer vào đúng phòng | Mã unique, không reuse 24h, hiện to trên bàn điều khiển | 5 |
| US-3.4 | Là **admin**, tôi muốn pre-flight check trước khi start để không "chết trên sân khấu" | Chặn start khi: đề thiếu câu theo RuleConfig, media hỏng, thí sinh chưa tech-check (force-start có confirm 2 bước) | 5, 8 |
| US-3.5 | Là **thí sinh**, tôi muốn lobby có thử chuông/âm thanh/đo ping để yên tâm trước giờ thi | Bấm thử chuông thấy round-trip ms; trạng thái sẵn sàng hiện cho admin | 5 |
| US-3.6 | Là **viewer**, tôi muốn vào xem chỉ với mã phòng + nickname, không cần tài khoản | Guest join → hàng chờ → admin duyệt mới thấy nội dung; bị từ chối thì disconnect | 5 |
| US-3.7 | Là **admin**, tôi muốn duyệt/từ chối viewer và khoá cổng viewer khi cần | Queue realtime 1-nhấp; rate-limit join; nút khoá cổng | 5, 8 |

## Epic 4 — Điều khiển trận đấu (Admin — luồng chính, như Athena)

| ID | User story | AC chính | Phase |
|---|---|---|---|
| US-4.1 | Là **admin**, tôi muốn điều khiển tuần tự các bước trận (mở vòng → hiện câu → chạy timer → chấm → câu tiếp) để làm chủ nhịp chương trình | Stepper 5 phần thi; mọi client đổi trạng thái theo lệnh admin <300ms; không có bước nào tự trôi ngoài các timer luật định | 6, 8 |
| US-4.2 | Là **admin**, tôi muốn thấy câu hỏi + đáp án (chỉ mình thấy) và chấm Đúng/Sai bằng 1 phím | Hotkey C/X; câu gõ text hiện kết quả auto-match chờ confirm; override được | 6, 8 |
| US-4.3 | Là **admin**, tôi muốn chỉnh điểm tay và undo thao tác chấm sai để sửa nhầm lẫn ngay trong trận | Chỉnh điểm bắt buộc nhập lý do; undo event chấm gần nhất; điểm tự tính lại đúng; tất cả vào audit log | 6, 8 |
| US-4.4 | Là **admin**, tôi muốn pause/resume trận và skip/thay câu hỏi lỗi để xử lý sự cố mà không hỏng trận | Pause đóng băng timer + khoá input; viewer thấy "tạm dừng kỹ thuật"; câu thay chạy lại từ đầu | 6, 8 |
| US-4.5 | Là **admin**, tôi muốn chỉnh timer đang chạy (±giây, đặt lại) theo tình huống thực tế | TimerControl start/pause/reset/±10s/đặt giá trị; áp cho mọi client ngay | 6, 8 |
| US-4.6 | Là **admin**, tôi muốn thấy log chuông/đáp án chính xác tới ms để phân xử khiếu nại "em bấm trước" tại chỗ | Event log realtime kèm server-timestamp + thứ hạng chuông; xuất được sau trận | 6, 8, 10 |
| US-4.7 | Là **admin**, tôi muốn hệ thống tự pause khi tôi rớt mạng để trận không tự trôi khi không ai chấm | `autoPauseOnHostDisconnect` mặc định bật | 6 |
| US-4.8 | Là **admin thứ hai** (nếu có), tôi muốn cơ chế "take control" rõ ràng để hai người không giẫm lệnh nhau | 1 host active tại một thời điểm; người còn lại read-only | 6, 8 |

## Epic 5 — Thi đấu (Thí sinh)

| ID | User story | AC chính | Phase |
|---|---|---|---|
| US-5.1 | Là **thí sinh**, tôi muốn giao diện tối giản phản hồi tức thì để tập trung thi | Phản hồi cục bộ <50ms (optimistic); không animation nặng; Lighthouse ≥90 | 7 |
| US-5.2 | Là **thí sinh**, tôi muốn thi trọn trận chỉ bằng bàn phím với hotkey dễ nhớ | Space chuông · Enter gửi · 1-4 hàng ngang · Esc xoá; hint in trên nút; đi trọn trận không cần chuột (E2E) | 7 |
| US-5.3 | Là **thí sinh**, tôi muốn biết ngay mình thắng/thua chuông và đúng/sai | Flash xanh giành chuông, rung đỏ khi sai/bị từ chối, sound cue | 7 |
| US-5.4 | Là **thí sinh**, tôi muốn quay lại trận đúng trạng thái nếu rớt mạng ngắn | Reconnect trong 120s → tự về đúng vòng/câu/điểm; banner "đang kết nối lại" | 5, 7 |
| US-5.5 | Là **thí sinh** ở vòng Về đích, tôi muốn chọn gói điểm và dùng Ngôi sao hy vọng đúng luật | Chọn gói trước lượt; NSHV chọn trước khi câu hiện, dùng 1 lần; UI khoá sau khi chọn | 6, 7 |

## Epic 6 — Xem trận (Viewer + Livestream)

| ID | User story | AC chính | Phase |
|---|---|---|---|
| US-6.1 | Là **viewer**, tôi muốn xem trận với animation sân khấu đẹp, mượt, ít thao tác | 60fps máy phổ thông; animation-queue tuần tự, skip-to-latest khi tụt; đủ hiệu ứng mọi vòng như demo `public/viewer.html` | 9 |
| US-6.2 | Là **viewer**, tôi muốn chỉnh cỡ chữ/giảm animation để xem thoải mái từ xa/trên mobile | Settings cỡ chữ, reduced-motion, âm lượng; contrast AA | 9 |
| US-6.3 | Là **người dựng stream**, tôi muốn thêm overlay câu hỏi/điểm vào OBS đè lên khung quay thí sinh | Browser Source 1920×1080 nền trong suốt; <50% CPU 1 core; phần tử bật/tắt từ admin | 9 |
| US-6.4 | Là **người dựng stream**, tôi muốn URL overlay không trở thành lỗ hổng nếu lộ trên hình | Bootstrap token 1 lần, revoke/re-issue được, chết khi trận kết thúc | 5 |
| US-6.5 | Là **viewer/overlay**, tôi tuyệt đối không gửi được lệnh gì lên trận | Namespace read-only, server drop mọi event từ đây (test) | 5, 9 |

## Epic 7 — Sau trận & Vận hành

| ID | User story | AC chính | Phase |
|---|---|---|---|
| US-7.1 | Là **admin**, tôi muốn xuất kết quả PDF để công bố/lưu hồ sơ | Bảng điểm cuối + sự kiện chính + QR verify; khớp 100% điểm hệ thống | 10 |
| US-7.2 | Là **admin**, tôi muốn thống kê câu hỏi (% đúng, thời gian TB) để cải thiện kho đề | Ghi ngược metadata sau trận; xem trên trang kho đề | 10 |
| US-7.3 | Là **admin**, tôi muốn trận sống sót qua sự cố server/Redis giữa chừng | Chaos drills pass: failover lease, Redis chết → auto-pause → khôi phục, không mất event đã công bố | 6, 10 |
| US-7.4 | Là **admin**, tôi muốn xem lại timeline trận (P2) để rút kinh nghiệm/giải khiếu nại muộn | Scrub theo event log, trạng thái tại từng thời điểm | 10 |

## Ngoài phạm vi v1 (đã ghi nhận, không cam kết)

- Giải đấu nhiều trận / bracket / season leaderboard (P3).
- Practice mode solo cho thí sinh tự luyện (P2 — đang chờ gap-analysis bổ sung).
- Speech-to-text tự chấm; stream video trực tiếp từ hệ thống; đa ngôn ngữ.
