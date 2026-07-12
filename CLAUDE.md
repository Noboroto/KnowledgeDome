# KnowledgeDome — Olympia Contest System

Nền tảng web tổ chức thi đấu gameshow kiến thức tuỳ biến (mô hình Đường lên đỉnh Olympia). Planning tại `plans/260711-2340-olympia-contest-system/` (spec engine: `research/ruleconfig-v2-spec.md`); quyết định chờ chốt: `plans/DEFERED.md`; demo tĩnh: `public/`.

Stack (đã chốt, không đổi): NestJS+Fastify, Zod, Prisma+Postgres, Redis, Better-auth, Socket.IO, @casl/ability · React+Vite, MUI, Motion for React, Zustand, TanStack Query · MinIO.

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
  - **Dashboard**: bố cục theo **Z / F reading model**; đưa info liên quan nhất của từng role lên trước (admin → điều khiển trận + hàng chờ duyệt; setter → câu hỏi của mình + trạng thái duyệt; thí sinh → trạng thái thi + điểm; viewer → sân khấu + bảng điểm).

## Điều hướng (demo `public/` và app)

- Mọi màn có nút **Back** rõ ràng về màn trước/menu; `Esc` = back (đóng modal trước nếu đang mở), `H` = về hub/menu chính. *Ngoại lệ màn thi đấu của thí sinh: Esc CHỈ xoá ô nhập, không back (browser dùng Esc thoát fullscreen — tránh văng fullscreen giữa trận).*

## Quy ước khác

- Server-authoritative tuyệt đối: timer/điểm/chuông tính ở server, client chỉ render (xem spec).
- Đáp án không rời server ngoài kênh admin + MC (authenticated, audit).
- Mọi timer/điểm là RuleConfig — KHÔNG hard-code luật trong code/UI.
- String UI tiếng Việt tách file constants (`vi.ts`), không hard-code trong JSX.
- Commit theo Conventional Commits, KHÔNG AI attribution.
