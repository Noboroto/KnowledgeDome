# User Stories — Olympia Contest System (sắp xếp theo ROLE)

> Định dạng: **US-x.y** · *Là [role], tôi muốn [hành động] để [giá trị]* · AC = Acceptance Criteria · Phase = nơi hiện thực (xem `plan.md`). Mã US giữ nguyên từ bản theo-epic cũ để truy vết.
> Nguyên tắc điều khiển (user đã chốt): **admin điều khiển các bước cuộc thi là chính** — thí sinh/viewer chỉ phản ứng theo trạng thái server phát ra, như mô hình AIServer của Athena cũ.
> **Lộ trình version (✅ D18):** Phase 1-10 = **v1** (solo contest); Phase 11 = **v1.5** (practice); Phase 12 = **v2** (teams). Story gắn Phase 11/12 thuộc mốc tương ứng; schema DB cho tất cả dựng từ v1.

## 👑 ADMIN

### Quản trị hệ thống & phân quyền

| ID | User story | AC chính | Phase |
|---|---|---|---|
| US-1.1 | Là **admin**, tôi muốn tạo tài khoản cho setter/thí sinh (username + password + role) để kiểm soát ai vào hệ thống | Tạo/disable/reset password; không có self-signup; user disabled không đăng nhập được | 2 |
| US-1.3 | Là **admin**, tôi muốn thí sinh chỉ có 1 phiên hoạt động để tránh nhờ người thi hộ song song | Login mới đá phiên cũ kèm thông báo trên máy cũ | 2, 5 |
| US-1.4 | Là **admin**, tôi muốn logout toàn bộ phiên thí sinh sau trận vì máy trường dùng chung | Nút "logout mọi phiên thí sinh"; session thí sinh tự hết hạn 24h | 2 |
| US-1.5 | Là **admin**, tôi muốn tạo role mới từ danh sách permission và gán nhiều role cho một người để phân quyền linh hoạt (vd "trọng tài" = contest.control + question.viewAnswer) | Role builder chọn permission theo nhóm; 1 user nhiều role; **5 role mặc định** (gồm REVIEWER — ✅ D15.1) không xoá được; đổi role có hiệu lực ngay (cache invalidate) | 2 |
| US-2.4 | Là **admin**, tôi muốn đáp án không bao giờ lộ cho người không có quyền để bảo mật đề tuyệt đối | Mọi API/socket cho thí sinh/viewer không chứa field đáp án (test tự động); audit log khi admin xem đáp án; trừ khi revealAnswerAfterJudge bật và câu đã chấm — test phủ cả 2 trạng thái | 3, 10 |

### Dựng contest (người tạo contest)

| ID | User story | AC chính | Phase |
|---|---|---|---|
| US-3.1 | Là **admin**, tôi muốn tạo contest với luật tuỳ chỉnh (timer, số câu, điểm từng vòng đều CUSTOM) và nút **"Áp dụng luật 2026"** (✅ D8 — preset theo Fandom wiki) | Clone preset → sửa từng tham số qua UI; nút 1-click reset về `O26_DEFAULT@1`; validate ràng buộc; luật freeze khi trận start | 5, 6, 8 |
| US-8.1 | Là **admin**, tôi muốn dựng trận bằng round playlist (số vòng, loại vòng, thứ tự tuỳ ý) thay vì 4 vòng cứng | Contest builder thêm/xoá/sắp xếp vòng; mỗi vòng form config riêng; chọn preset (`O26_DEFAULT@1`...) rồi tuỳ biến; Zod validate + pre-flight | 5, 6, 8 |
| US-3.2 | Là **admin**, tôi muốn gán 1-12 ghế thí sinh cá nhân (được để trống ghế) và bộ đề cho contest | Ghế trống hợp lệ; gán đề = snapshot; *(gộp đội → US-10.4, Phase 12/v2)* | 5 |
| US-8.2 | Là **admin**, tôi muốn tổ chức 1-12 thí sinh cá nhân (phổ điểm mảng theo số đơn vị điểm) | Ghế 1-12 adaptive; *(gộp đội → mục v2 bên dưới; schema Team/scoringUnit dựng sẵn từ v1)* | 5, 6 |
| US-3.9 | Là **người tạo contest**, tôi PHẢI chọn danh sách câu hỏi trước khi contest bắt đầu — hệ thống không tự lấy đề, draw chỉ random trong danh sách tôi chọn (✅ D22) | QuestionPicker: full-text search + filter (pool/lĩnh vực/mức điểm/độ khó/tags/người thực hiện/trạng thái) + sort (mới nhất/mức điểm/độ khó/lần dùng/tần suất); preview không lộ đáp án ngoài quyền | 3, 5, 8 |
| US-3.10 | Là **người tạo contest**, tôi muốn tick checkbox "câu này được dùng ở phần nào" và chọn DƯ câu để có dự phòng (✅ D22.3) | Checkbox chỉ hiện phần hợp lệ theo pool + điều kiện điểm (KV value 20 → Khởi động/Về đích; TT → Tăng tốc); pool dư hợp lệ; câu đã hỏi (`usedInContest`) KHÔNG lặp lại trong cùng contest — draw tự loại, pre-flight đếm phần còn lại | 5, 6, 8 |
| US-3.3 | Là **admin**, tôi muốn hệ thống phát mã phòng 6 số để thí sinh/viewer vào đúng phòng | Mã unique, không reuse 24h, hiện to trên bàn điều khiển | 5 |
| US-3.4 | Là **admin**, tôi muốn pre-flight check trước khi start để không "chết trên sân khấu" | Chặn start khi: đề thiếu câu theo RuleConfig, media hỏng, thí sinh chưa tech-check (force-start có confirm 2 bước) | 5, 8 |
| US-2.8 | Là **admin**, tôi muốn export/import CONTEST CONFIG trọn gói để soạn trên bản Internet rồi mang sang bản portable ngày thi (✅ D23) | ZIP = contest.json + questions.xlsx (default Excel, nhận CSV/Google Sheet) + media-meta.json + media theo subfolder từng vòng + assets theme/sound; roundtrip không mất dữ liệu; import tạo contest nháp + chạy pre-flight | 4 |

### Tuỳ biến luật từng vòng (spec: `research/ruleconfig-v2-spec.md`)

| ID | User story | AC chính | Phase |
|---|---|---|---|
| US-8.3 | Là **admin**, tôi muốn khởi động có 4 kiểu lượt (cá nhân/chung × giới hạn thời gian/số câu) với số lượt tuỳ ý | 4 turn kinds đúng spec §4; tham số thời gian/số câu tự do trong bounds + preset chips | 6 |
| US-8.4 | Là **admin**, tôi muốn rút đề ngẫu nhiên theo lĩnh vực cho khởi động (X câu Toán + Y câu RANDOM...), xáo được thứ tự — **chỉ random TRONG danh sách đề tôi đã gán (✅ D22), câu đã dùng trong CONTEST không lặp lại** | drawConfig slots theo Field taxonomy; rút server-side thành event replay được; loại câu `usedInContest`; pre-flight kiểm pool còn lại đủ worst-case | 3, 5, 6 |
| US-8.5 | Là **admin**, tôi muốn VCNV 4-8 hàng ngang, bật/tắt gợi ý ký tự trong hàng ngang (kiểu O11-O12) | rowCount 4-8, revealCharHints; ảnh chia miếng theo số hàng | 6, 9 |
| US-8.6 | Là **admin**, tôi muốn tăng tốc chọn format ranked-speed hoặc clue-buzz (3-4 dữ kiện mở dần, bấm chuông), thời gian là thuộc tính từng câu | 2 format đúng spec §6; câu clue-buzz validate có clues; timeSeconds per-question override default | 3, 6, 7, 9 |
| US-8.7 | Là **admin**, tôi muốn về đích dùng gói điểm preset tự quy định (30/50/70/90...) hoặc thí sinh tự build gói từ các mốc, thời gian từng câu tự chỉnh | packageMode preset/custom-build; defaultTimeByValue + per-question timeSeconds | 6, 7, 8 |
| US-8.8 | Là **admin**, tôi muốn đổi theme trận: màu đồ hoạ, logo, ảnh thí sinh, video hình hiệu | Theme editor map design tokens; asset MinIO; viewer/overlay/MC áp theme; video phát ở intro/INTERMISSION | 8, 9 |
| US-8.9 | Là **admin**, tôi muốn gán nhạc cho TỪNG thành phần (cue slot × loại vòng) và có playlist nhạc nền điều khiển bằng soundboard | Sound editor per-slot (admin tự upload, slot trống = silent — ✅ D10); soundboard phát/dừng/next/volume/duck, ngoài control lock; contestant mặc định tắt nhạc nền | 8, 9 |

### Điều khiển trận (luồng chính, như Athena)

| ID | User story | AC chính | Phase |
|---|---|---|---|
| US-4.1 | Là **admin**, tôi muốn điều khiển tuần tự các bước trận (mở vòng → hiện câu → chạy timer → chấm → câu tiếp) để làm chủ nhịp chương trình | Stepper theo round playlist; mọi client đổi trạng thái theo lệnh admin <300ms; không có bước nào tự trôi ngoài các timer luật định | 6, 8 |
| US-4.2 | Là **admin**, tôi muốn thấy câu hỏi + đáp án (chỉ mình thấy) và chấm Đúng/Sai bằng 1 phím; **chấm tự động là toggle per-contest, DEFAULT TẮT (✅ D4)** | Hotkey C/X; `autoJudge` tắt → auto-match chỉ là GỢI Ý (badge mờ), điểm chốt khi tôi bấm; bật → câu gõ tự chốt + override được; normalize BẮT BUỘC case-insensitive + trim + collapse space thừa; câu miệng luôn chấm tay | 6, 8 |
| US-4.3 | Là **admin**, tôi muốn chỉnh điểm tay và undo thao tác chấm sai để sửa nhầm lẫn ngay trong trận | Chỉnh điểm bắt buộc nhập lý do; undo event chấm gần nhất; điểm tự tính lại đúng; tất cả vào audit log | 6, 8 |
| US-4.4 | Là **admin**, tôi muốn pause/resume trận và skip/thay câu hỏi lỗi để xử lý sự cố mà không hỏng trận | Pause đóng băng timer + khoá input; viewer thấy "tạm dừng kỹ thuật"; câu thay chạy lại từ đầu; VCNV cả 4 bị loại → nút mở miếng ghép THỦ CÔNG (✅ D13.1) | 6, 8 |
| US-4.5 | Là **admin**, tôi muốn chỉnh timer đang chạy (±giây, đặt lại) theo tình huống thực tế | TimerControl start/pause/reset/±10s/đặt giá trị; áp cho mọi client ngay | 6, 8 |
| US-4.6 | Là **admin**, tôi muốn thấy log chuông/đáp án chính xác tới ms để phân xử khiếu nại "em bấm trước" tại chỗ | Event log realtime kèm server-timestamp + thứ hạng chuông; xuất được sau trận | 6, 8, 10 |
| US-4.7 | Là **admin**, tôi muốn hệ thống tự pause khi tôi rớt mạng để trận không tự trôi khi không ai chấm | `autoPauseOnHostDisconnect` mặc định bật | 6 |
| US-4.8 | Là **admin thứ hai** (nếu có), tôi muốn cơ chế "take control" rõ ràng để hai người không giẫm lệnh nhau | 1 host active tại một thời điểm; người còn lại read-only | 6, 8 |
| US-3.7 | Là **admin**, tôi muốn khoá cổng viewer và kick viewer khi cần | Nút khoá cổng chặn join mới; kick 1-nhấp; đếm viewer online | 5, 8 |
| US-3.8 | Là **admin/host**, tôi muốn thấy trạng thái kết nối từng máy trong trận (✅ chốt 12/07) | Thí sinh theo ghế 1,2,3... (online/offline, ping ms); viewer/MC chỉ số lượng; cảnh báo khi thí sinh rớt | 5, 8 |
| US-8.12 | Là **admin**, tôi muốn bật revealAnswerAfterJudge ở match official khi thật sự cần (có kiểm soát) | Cảnh báo + confirm 2 bước + audit; đáp án chỉ đẩy xuống SAU khi chấm | 6, 8 |

### Sau trận & vận hành

| ID | User story | AC chính | Phase |
|---|---|---|---|
| US-7.1 | Là **admin**, tôi muốn xuất kết quả PDF để công bố/lưu hồ sơ | Bảng điểm cuối + sự kiện chính + QR verify; khớp 100% điểm hệ thống | 10 |
| US-7.2 | Là **admin**, tôi muốn thống kê câu hỏi (% đúng, thời gian TB) để cải thiện kho đề | Ghi ngược metadata sau trận; xem trên trang kho đề | 10 |
| US-7.3 | Là **admin**, tôi muốn trận sống sót qua sự cố server/Redis giữa chừng | Chaos drills pass: failover lease, Redis chết → auto-pause → khôi phục, không mất event đã công bố; tải mục tiêu ✅ D11: <50 viewer/trận × ~5 trận song song | 6, 10 |
| US-7.4 | Là **admin**, tôi muốn xem lại timeline trận (P2) để rút kinh nghiệm/giải khiếu nại muộn | Scrub theo event log, trạng thái tại từng thời điểm | 10 |
| US-10.3 | Là **admin**, tôi muốn dữ liệu practice tự dọn để không phình DB và đúng privacy *(mốc v1.5)* | Retention job: practice anonymize sau 3 tháng, official 12 tháng (config — ✅ D21.1); trạng thái job hiện trên monitoring | 11 |

## ✍️ SETTER (người ra đề, kiêm chủ bộ đề)

| ID | User story | AC chính | Phase |
|---|---|---|---|
| US-2.1 | Là **setter**, tôi muốn tạo câu hỏi theo **3 pool (✅ D24): KV (chung Khởi động + Về đích + câu phụ), TT (tăng tốc), CN (bộ đề VCNV trọn)** kèm đáp án và acceptedAnswers | Form theo pool (CN = ảnh + CNV + hàng ngang có số chữ; KV có `value` tuỳ chọn — điều kiện dùng ở về đích); **displayId = `<POOL>-<4 hex đầu UUID>-<4 hex cuối UUID>` uppercase (vd `KV-A1B2-7890`; hàng ngang `-R1..R8`), unique, immutable**; validate Zod | 3 |
| US-2.2 | Là **setter**, tôi muốn gắn metadata (độ khó 1-5, chủ đề, lớp/kiến thức) để lọc và cân bằng đề | Filter/search theo mọi metadata; chip hiển thị như demo `public/questions.html` | 3 |
| US-2.3 | Là **setter**, tôi muốn đính kèm ảnh/video/audio cho câu hỏi | Upload có progress + preview; chặn file sai loại/quá dung lượng (✅ D6: ảnh ≤10MB, video ≤200MB, audio ≤20MB — env config); media hỏng báo ngay lúc upload | 3 |
| US-2.5 | Là **setter**, tôi muốn sửa câu hỏi đã ACTIVE mà không phá trận đã dùng nó | Sửa tạo version mới; trận cũ giữ snapshot | 3, 5 |
| US-2.6 | Là **admin/setter**, tôi muốn export bộ đề ra file và import lại được để chia sẻ/backup | ZIP bundle roundtrip không mất dữ liệu; import báo lỗi từng dòng + preview trước khi ghi; import Excel với mapping cột | 4 |
| US-9.1 | Là **setter/admin**, tôi muốn tạo bộ đề có mã ID, gom câu từ kho theo displayId (format pool D24) hoặc nhập tay (tuỳ chọn lưu vào kho) | SetItem reference/inline; checkbox "lưu vào kho"; bộ đề có displayId | 3 |
| US-9.2 | Là **setter/admin**, tôi muốn quản lý nhãn lĩnh vực và gán cho câu hỏi/bộ đề | Field taxonomy CRUD; filter theo lĩnh vực | 3 |
| US-9.5 | Là **setter/admin**, tôi muốn tra cứu câu hỏi bằng full-text search + filter + sort (✅ D22) | Postgres FTS (unaccent) trên nội dung/giải thích/tags; filter pool/lĩnh vực/mức điểm/độ khó/người thực hiện/trạng thái; sort mới nhất/mức điểm/độ khó/lần dùng/tần suất; search theo đáp án yêu cầu quyền viewAnswer | 3 |
| US-9.6 | Là **setter/admin**, tôi muốn nhập bộ đề từ Excel theo template quy ước và xuất ngược ra Excel | Template theo pool/vòng; roundtrip; kèm/không kèm đáp án theo quyền | 4 |
| US-9.3 | Là **chủ bộ đề**, tôi muốn để bộ đề private (chỉ tôi + người được cấp) và chia sẻ có kiểm soát | Owner+ACL; share-link password+TTL revoke được (đã chốt) | 3 |
| US-9.4 | Là **chủ bộ đề**, tôi muốn public bộ đề cho cộng đồng tải/dùng *(mốc v1.5 — schema everPublic/visibility từ v1)* | Public = link/download tự do, kèm đáp án theo setting per-set (default có) + cảnh báo; chặn public khi gắn contest chưa diễn (everPublic hard-block) | 11 |

## 🔍 REVIEWER (duyệt đề — ✅ D15.1)

| ID | User story | AC chính | Phase |
|---|---|---|---|
| US-2.7 | Là **reviewer** (role seed, permission `question.review`), tôi muốn duyệt đề DRAFT→ACTIVE tách khỏi admin để ban đề đông người vẫn kiểm soát chất lượng | Activate gate bằng permission (không theo tên role); reviewer xem được đáp án câu đang duyệt; mọi lượt duyệt + xem đáp án vào audit log | 2, 3 |

## 🎓 THÍ SINH

| ID | User story | AC chính | Phase |
|---|---|---|---|
| US-1.2 | Là **thí sinh**, tôi muốn đăng nhập chỉ bằng username+password để vào thi nhanh, không cần email | Form tối giản, autofocus, Enter submit; sai 5 lần/phút bị khoá tạm | 2 |
| US-3.5 | Là **thí sinh**, tôi muốn lobby có thử chuông/âm thanh/đo ping để yên tâm trước giờ thi | Bấm thử chuông thấy round-trip ms; trạng thái sẵn sàng hiện cho admin | 5 |
| US-5.1 | Là **thí sinh**, tôi muốn giao diện tối giản phản hồi tức thì để tập trung thi | Phản hồi cục bộ <50ms (optimistic); không animation nặng; Lighthouse ≥90 | 7 |
| US-5.2 | Là **thí sinh**, tôi muốn hotkey dễ nhớ cho nhập liệu, còn chuông là nút click chuột riêng biệt để không bấm nhầm | Chuông CHỈ click chuột (không hotkey); Enter gửi · 1-8 hàng ngang · Esc xoá; hint in trên nút; tăng tốc gửi lại tự do — tính bản cuối (E2E) | 7 |
| US-5.3 | Là **thí sinh**, tôi muốn biết ngay mình thắng/thua chuông và đúng/sai | Flash xanh giành chuông, rung đỏ khi sai/bị từ chối, sound cue | 7 |
| US-5.4 | Là **thí sinh**, tôi muốn quay lại trận đúng trạng thái nếu rớt mạng ngắn | Reconnect trong 120s → tự về đúng vòng/câu/điểm; banner "đang kết nối lại"; rớt đúng lượt riêng → engine pause + admin quyết (✅ D13.4) | 5, 7 |
| US-5.5 | Là **thí sinh** ở vòng Về đích, tôi muốn chọn gói điểm và dùng Ngôi sao hy vọng đúng luật | Chọn gói trước lượt; NSHV chọn trước khi câu hiện, dùng 1 lần; UI khoá sau khi chọn | 6, 7 |
| US-10.2 | Là **thí sinh CLB**, tôi muốn luyện tập SOLO với đề public — tự bấm giờ, thấy đáp án sau chấm, không cần admin điều khiển *(mốc v1.5)* | Contest cá nhân + practice match 1 ghế, orchestrator auto-advance; revealAnswerAfterJudge ON | 11 |

## 📺 VIEWER (khán giả — public theo mã phòng)

| ID | User story | AC chính | Phase |
|---|---|---|---|
| US-3.6 | Là **viewer**, tôi muốn vào xem NGAY chỉ với mã phòng, không tài khoản, không chờ duyệt (✅ chốt 12/07) | Nhập mã 6 số → xem luôn; read-only enforced server-side; rate-limit IP | 5 |
| US-6.1 | Là **viewer**, tôi muốn xem trận với animation sân khấu đẹp, mượt, ít thao tác | 60fps máy phổ thông; animation-queue tuần tự, skip-to-latest khi tụt; mỗi animation là module độc lập engine (✅ D22); đủ hiệu ứng mọi vòng như demo `public/viewer.html` | 9 |
| US-6.2 | Là **viewer**, tôi muốn chỉnh cỡ chữ/giảm animation để xem thoải mái từ xa/trên mobile | Settings cỡ chữ, reduced-motion, âm lượng; contrast AA; responsive ≥360px (đa số viewer là điện thoại) | 9 |
| US-6.5 | Là **viewer/overlay**, tôi tuyệt đối không gửi được lệnh gì lên trận | Namespace read-only, server drop mọi event từ đây (test) | 5, 9 |
| US-10.6 | Là **viewer trận đội**, tôi muốn scoreboard/podium hiển thị theo đội mà vẫn thấy ai vừa bấm/trả lời *(mốc v2)* | Điểm đội to + thành viên nhỏ; adaptive ≤4 đội sân khấu đầy đủ, 5-12 grid gọn | 12 |

## 🎙️ MC

| ID | User story | AC chính | Phase |
|---|---|---|---|
| US-8.10 | Là **MC**, tôi muốn màn riêng chữ rất to hiện câu hỏi + đáp án + tóm tắt kết quả để dẫn chương trình | Route `/mc` read-only; permission match.viewAnswer theo contest; audit log | 9 |

## 🎬 NGƯỜI DỰNG STREAM (OBS)

| ID | User story | AC chính | Phase |
|---|---|---|---|
| US-6.3 | Là **người dựng stream**, tôi muốn thêm overlay câu hỏi/điểm vào OBS đè lên khung quay thí sinh | Browser Source 1920×1080 nền trong suốt; <50% CPU 1 core; phần tử bật/tắt từ admin | 9 |
| US-6.4 | Là **người dựng stream**, tôi muốn URL overlay chỉ chứa mã phòng, hết hiệu lực khi contest đóng | Không token dài hạn; lộ mã trên hình → admin khoá cổng/kick; khoá cổng chặn kết nối mới ngay | 5 |

## 🏋️ TRAINER (CLB — mốc v1.5)

| ID | User story | AC chính | Phase |
|---|---|---|---|
| US-8.11 | Là **admin/trainer**, tôi muốn tạo practice match (matchPurpose='practice') và Rematch nhanh để CLB luyện tập hàng tuần | Purpose chọn lúc tạo, immutable sau start; practice pre-flight = warning (vẫn start được); reveal default ON; nút Rematch giữ seats + room code; trainer = role có `contest.create` (✅ D21.2) | 11 |
| US-10.1 | Là **trainer**, tôi muốn tạo practice match từ đề public và Rematch nhanh (chi tiết US-8.11) | Xem US-8.11; policy bundle spec §13 | 11 |

## 🛡️ BTC / TRỌNG TÀI GIẢI

| ID | User story | AC chính | Phase |
|---|---|---|---|
| US-5.6 | Là **BTC**, tôi muốn thí sinh KHÔNG xem trước được đề qua DevTools dù media được preload cho mượt (✅ D12b) | Thí sinh preload blob MÃ HOÁ qua service worker; key chỉ tới lúc reveal (server time); bytes cached không decode được (test tự động); SW hỏng → tự fallback nhận URL lúc reveal, trận không đứng | 6, 7 |
| US-10.4 | Là **admin**, tôi muốn gộp 1-12 thí sinh thành đội (bấm chuông cá nhân, điểm về đội) *(mốc v2)* | Team setup kéo thả; scoringUnit; individualTurnMode all-members/representative; phổ điểm theo số đơn vị điểm | 12 |
| US-10.5 | Là **BTC giải đội**, tôi muốn luật đội công bằng đã định nghĩa rõ từng vòng (✅ D17) *(mốc v2)* | Khởi động/về đích: khoá CÁ NHÂN khi sai (`teamLockout: false` default, option true); VCNV sai CNV loại CẢ ĐỘI; NSHV 1/đội; CẤM same-team steal (server reject, test); tie-break đội cử 1 người | 12 |

## Ngoài phạm vi (đã ghi nhận, không cam kết)

- Giải đấu nhiều trận / bracket / season leaderboard (P3).
- Speech-to-text tự chấm; stream video trực tiếp từ hệ thống; đa ngôn ngữ.
