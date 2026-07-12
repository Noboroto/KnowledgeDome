# KnowledgeDome — Olympia Contest System

Nền tảng web tổ chức thi đấu gameshow kiến thức tuỳ biến (mô hình Đường lên đỉnh Olympia). Planning tại `plans/260711-2340-olympia-contest-system/` (spec engine: `research/ruleconfig-v2-spec.md`); sổ quyết định: `plans/260711-2340-olympia-contest-system/DEFERED.md` (D1-D23, đa số đã chốt); demo tĩnh: `public/`.

Stack (đã chốt): NestJS + **Express adapter**, Zod, Prisma+Postgres, Redis, Better-auth, Socket.IO, @casl/ability · React+Vite, MUI, Motion for React, Zustand, TanStack Query · MinIO.

## Lộ trình version (✅ D18 chốt 12/07)

- **v1 — Solo contest** (Phase 1-10): contest chính thức, thí sinh CÁ NHÂN 1-12 ghế, đủ loại vòng + biến thể, kho đề, viewer/overlay/MC/admin, 2 profile deploy (compose + portable Windows).
- **v1.5 — Practice** (Phase 11): `matchPurpose: practice`, bộ đề PUBLIC + share-link, UI luyện tập solo, trainer role, retention riêng (practice 3 tháng / official 12 tháng).
- **v2 — Teams** (Phase 12): thi đội — buzz cá nhân, điểm về đội (semantics spec §2b).
- **DB + Zod schema chuẩn bị ĐẦY ĐỦ ngay từ v1** (Team/seat.teamId/scoringUnit, matchPurpose, visibility/everPublic, ACL, retention) — KHÔNG để dành schema cho version sau, tránh migrate; v1 chỉ chưa bật UI/engine-path tương ứng.

## Luật chơi & đề thi (chốt 12/07)

- **Source of truth luật O26 = Fandom wiki** ([Luật chơi/Olympia 26](https://duong-len-dinh-olympia.fandom.com/vi/wiki/Lu%E1%BA%ADt_ch%C6%A1i/Olympia_26)) — bảng giá trị đã đối chiếu: `plans/.../research/rules-2026.md`. Mọi giá trị vẫn là RuleConfig custom được; contest builder có nút **"Áp dụng luật 2026"** áp preset `O26_DEFAULT@1`.
- **Điểm ĐỘC LẬP thời gian**: `timeSeconds` là metadata TỪNG CÂU HỎI (cùng mức 20đ có thể câu 15s và 40s) — hệ thống chọn câu theo MỨC ĐIỂM, thời gian lấy theo câu; preset chỉ đặt default.
- **Người tạo contest PHẢI chọn danh sách câu hỏi trước khi start** (full-text search + filter + sort trên kho đề); hệ thống KHÔNG tự lấy đề — draw chỉ RANDOM TRONG danh sách đã gán (snapshot). Pre-flight chặn start khi thiếu.
- **Contest config import/export trọn gói** (D23): ZIP = Excel câu hỏi (default; nhận CSV/Google Sheet) + JSON metadata media + media theo subfolder từng vòng — use-case soạn trên bản Internet → import vào portable.

## UX — BẮT BUỘC

Áp cho MỌI UI trong repo này (app React lẫn demo tĩnh `public/`):

- **Visibility of System Status**: Mọi thao tác async (submit, save, validate, load) PHẢI hiển thị trạng thái rõ ràng — spinner/loading state, progress indicator, hoặc skeleton. Không để UI im lặng khi đang xử lý.
- **Immediate Feedback**: Phản hồi người dùng ngay lập tức (≤100ms cho UI, ≤1s cho kết quả đầu tiên). Toast/message thành công hoặc thất bại phải xuất hiện sau mỗi action. Luồng async theo pattern `loading → success/error` (MUI: `Snackbar`/`Alert`; demo tĩnh: toast component chung).
- **KHÔNG chặn gửi lại (rule chống double-submit đã bị chủ dự án gỡ bỏ)**: nút action chỉ hiện trạng thái loading, KHÔNG disable; người dùng gửi lại được — server nhận **bản cuối cùng** trước timeout. Dedup/idempotency là việc của SERVER (event log), không phải của UI. *Ngoại lệ: khoá-theo-LUẬT-CHƠI (chuông bị khoá khi sai, NSHV đã dùng, không tới lượt) vẫn disable bình thường — đó là trạng thái game, không phải chống double-submit.*
- **Nút bấm chuông CHỈ nhận click chuột** — không gán hotkey cho chuông (tránh bấm nhầm khi gõ đáp án); các hotkey khác (Enter gửi, 1-8 chọn hàng...) giữ nguyên.
- **Tăng tốc: nhận MỌI lần trả lời đến khi hết giờ, tính BẢN CUỐI CÙNG** — không khoá input/nút gửi sau khi trả lời; ranking theo server-received timestamp của bản cuối.
- **Server time là source of truth DUY NHẤT và là quyết định cuối cùng**: timeout, thứ tự chuông, thứ hạng tốc độ đều theo đồng hồ server; client chỉ hiển thị.
- **Viewport-Conscious Balanced Layout**: Design chủ đích theo kích thước màn hình. Giữ **nội dung chính của mỗi page/tab trong 1 viewport** (tham chiếu: laptop **1440×900**, ~844px usable dưới top-bar 56px) — không thứ gì quan trọng phải scroll mới thấy — nhưng **không được nhồi nhét**: giữ breathing room và whitespace dễ đọc. Cân bằng cả hai chiều hỏng:
  - **Không ép scroll** — nếu phần tử quan trọng nằm dưới fold, sửa layout (bỏ page header trùng breadcrumb; nén hàng stat-card "hero" thành metric strip mỏng; form 2 cột compact thay vì card xếp dọc từng field). **Bảng dài ưu tiên pagination** với page size theo viewport (~10-12 dòng) để header + toolbar + rows + pager vừa 1 màn **không scroll trang** — đây là cách sửa chính; table body scroll nội bộ + sticky header + pager ghim là fallback khi ranh giới trang bất tiện (vd ma trận cố định). Chỉ giữ scroll cả trang khi không còn cách hợp lý, và phải giải thích được.
  - **Không phí không gian / không quá đặc** — không để mảng trống lớn hoặc info giá trị thấp chiếm chỗ đẹp; cũng không nén chặt đến rối. Ưu tiên info quan trọng với **role hiện tại**.
  - **Dashboard**: bố cục theo **Z / F reading model**; đưa info liên quan nhất của từng role lên trước (admin → điều khiển trận + hàng chờ duyệt ĐỀ (DRAFT→ACTIVE); setter → câu hỏi của mình + trạng thái duyệt; thí sinh → trạng thái thi + điểm; viewer → sân khấu + bảng điểm).

## Điều hướng (demo `public/` và app)

- Mọi màn có nút **Back** rõ ràng về màn trước/menu; `Esc` = back (đóng modal trước nếu đang mở), `H` = về hub/menu chính. *Ngoại lệ màn thi đấu của thí sinh: Esc CHỈ xoá ô nhập, không back (browser dùng Esc thoát fullscreen — tránh văng fullscreen giữa trận).*

## Nguyên tắc code — BẮT BUỘC toàn repo

- **DRY**: không lặp logic/hằng số/schema — Zod schemas, RuleConfig, permission catalog, socket event contracts đều ở `packages/shared` dùng chung FE+BE; validation viết MỘT lần (Zod) chạy cả hai đầu; component/hook/util lặp ≥2 lần phải trích xuất.
- **Zero-trust security**: KHÔNG BAO GIỜ tin client — mọi request/socket event đều verify auth + permission (CASL) ở server bất kể client là ai, đã join room gì, UI có ẩn nút hay không; viewer/overlay là public nhưng server vẫn enforce read-only (drop mọi event ghi từ namespace này); mọi input validate lại ở server (validate FE chỉ là UX); **đáp án chỉ rời server tới admin + MC (authenticated + audit); viewer/thí sinh/overlay không hiển thị đáp án — TRỪ khi contest bật `revealAnswerAfterJudge` (config per-match theo matchPurpose: official default TẮT, practice default BẬT, dùng cho luyện tập: chỉ đẩy đáp án SAU khi chấm xong)**; timer/điểm/chuông chỉ tính ở server.

## Mô hình truy cập (chốt 12/07)

- **Public (chỉ cần MÃ PHÒNG 6 số, không account, không duyệt)**: màn viewer + overlay OBS (frame stream) — read-only tuyệt đối, có rate-limit + nút "khoá cổng" của admin.
- **Cần AUTH (username+password + permission)**: thí sinh, MC, admin/setter — mọi giao diện có thể gửi event hoặc thấy đáp án.

## Quy ước khác

- Server-authoritative tuyệt đối: timer/điểm/chuông tính ở server, client chỉ render; **server time là source of truth duy nhất**.
- **Audit log MỌI thao tác, MỌI role** (user chốt 12/07): auth (login/logout/fail), CRUD kho đề/bộ đề, contest management, mọi event trong trận (đã có MatchEvent log), viewer join/kick, xuất/nhập, xem đáp án — bảng AuditLog chung append-only (actor, action, target, ts, ip) ngoài các log chuyên biệt.
- **TRIM mọi input text ở CẢ frontend lẫn backend** (đề, đáp án, submission — tránh space đầu/cuối phá so khớp).
- **Chỉ tiếng Việt** (không i18n); **múi giờ thống nhất UTC+7** — DB lưu UTC, mọi hiển thị/log/PDF format theo UTC+7, không có timezone setting per-user.
- **Stack: NestJS + Express adapter** (✅ D9 12/07 — không dùng Fastify).
- Mọi timer/điểm là RuleConfig — KHÔNG hard-code luật trong code/UI.
- **Animation là module ĐỘC LẬP với engine/rule** (D22): engine chỉ emit semantic event; mapping event→animation là config client-side — sửa rule (vd cách chọn câu hỏi) không đụng animation và ngược lại.
- **Media thí sinh preload MÃ HOÁ qua service worker** (D12b): key phát đúng lúc reveal theo server time; fallback reveal-only khi SW không khả dụng. Viewer/overlay preload URL thường.
- **Sound**: engine emit `sound-cue {slot}`; admin tự upload file per slot (slot trống = silent, không có bộ SFX default); client pre-download toàn bộ SFX khi vào phòng.
- Giới hạn media (D6, env config): ảnh ≤10MB, video ≤200MB, audio ≤20MB.
- String UI tiếng Việt tách file constants (`vi.ts`), không hard-code trong JSX.
- Commit theo Conventional Commits, KHÔNG AI attribution.
