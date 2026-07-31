# Feature Specification: EPIC-002 — Kho đề và bộ đề

**Feature Directory**: `specs/002-kho-de-bo-de`

**Created**: 2026-07-30

**Status**: Draft

**Input**: User description: `/speckit.specify` — tạo feature specification cho **EPIC-002 — Kho đề và bộ đề**, chỉ dùng product requirement / user journey / game rule / state thuộc EPIC-002, không đưa công nghệ triển khai vào spec.

---

## Clarifications

### Session 2026-07-30

- Q: `OQ-001` — "Bộ đề" là entity riêng có quan hệ N-N với câu hỏi, hay chỉ là tên khác của "kho đề"? → A: **Bộ đề là entity riêng**, N-N với câu hỏi; mỗi bộ đề tự mang `public`/`private`; `visibility` của câu = OR của các bộ đề nó thuộc. `TERM-046` (liệt "bộ đề" như tên khác của "kho đề") được coi là diễn đạt lỏng của glossary, cần sửa lại ở lần cập nhật `docs/glossary.md` kế tiếp — spec này không tự sửa tài liệu nguồn.
- Q: `OQ-004` — Sửa một câu `ACTIVE` đã từng hiển thị trong ít nhất một trận thì xử lý thế nào? → A: **Cho sửa tự do, không khoá** — nhưng **mỗi lần câu được hiển thị trong một trận, hệ thống MUST ghi log nội dung/phiên bản của câu tại đúng thời điểm hiển thị đó**, gắn với trận tương ứng, để phát lại trận cũ đúng với những gì thí sinh đã thấy thật, độc lập với bản mới nhất của câu. Đây là **một dòng nhật ký gắn với sự kiện hiển thị**, không phải một hệ thống quản lý phiên bản đầy đủ cho setter — "đánh phiên bản khi sửa câu đã duyệt" (product-discovery.md §8) vẫn đứng ngoài phạm vi v1 theo đúng nghĩa hẹp của cụm đó (không có UI xem lịch sử phiên bản, không có diff, không có rollback).
- Q: `OQ-006` — Hai phiên admin cùng thao tác Duyệt/Trả về gần như đồng thời trên cùng một câu `DRAFT` — ai thắng? → A: **Server-timestamp-first-wins**, cùng khuôn mẫu đã dùng ở chỗ khác trong repo (tranh chấp phiên MC, tranh chấp nhận quyền điều khiển TRỐNG): quyết định tới trước có hiệu lực, quyết định tới sau bị từ chối và ghi nhật ký, không lật kết quả đã phân giải. Đồng thời, **mọi lần sửa một câu hỏi** (không riêng gì thao tác duyệt) MUST ghi log nội dung đã đổi dạng khác biệt (diff) từng trường, kèm người thực hiện — đây là yêu cầu audit chung, không phải cơ chế giải quyết tranh chấp riêng cho việc duyệt.
- Q: `OQ-002` — `displayId` có bắt buộc duy nhất không? → A: **Bắt buộc duy nhất toàn hệ thống**, MUST mang một **tiền tố ngắn (2 ký tự) chỉ loại câu** để người đọc phân biệt được nhanh. Ngoài `displayId`, hệ thống còn giữ một **định danh nội bộ ổn định, riêng biệt với `displayId`**, dùng để tham chiếu câu xuyên hệ thống (nhập/xuất, event log) mà không phụ thuộc việc `displayId` có bị đổi hay không. *(Thuật toán sinh định danh cụ thể — kể cả việc dùng UUIDv7 — là quyết định kỹ thuật thuộc `plan.md`, không phải nội dung của spec; xem §11 Assumptions.)*
- Q: `OQ-005` — Có tồn tại thao tác xoá câu hỏi khỏi kho không, áp dụng khi nào? → A: **Xoá mềm (soft-delete), ở mọi trạng thái** — cả `DRAFT` lẫn `ACTIVE`. Câu đã xoá mềm biến mất khỏi tìm kiếm và khỏi danh sách chọn cho contest, nhưng dữ liệu — kể cả `everPublic` và log các lần hiển thị đã ghi theo `OQ-004`/FR-004b — MUST NOT bị xoá hay thay đổi.

---

## 0. Nguồn đã đọc và ghi chú về nguồn

| Nguồn yêu cầu đọc | Trạng thái |
|---|---|
| `docs/PRD.md` | ✅ đã đọc — §7 Actors (`ACTOR-002` setter, `ACTOR-001` admin), §10 `JOURNEY-001`, §11 EPIC-002, §12 EPIC-002 (`PRD-REQ-007`…`015`) cộng phần EPIC-002 của `PRD-REQ-047` và `PRD-REQ-092`, §14 FS-05…FS-07, §16, §19, §20 ma trận truy nguyên |
| `docs/glossary.md` | ✅ đã đọc — `TERM-044`…`TERM-048`, `TERM-049`, `TERM-050`, `TERM-052`, `TERM-053`, `TERM-058` |
| `docs/game-rules.md` | ✅ đã đọc — `GR-006`, `GR-019`, `GR-027`, `GR-031` đầy đủ; `GR-007`…`GR-011` đọc để tách phần **thuộc kho đề** (cấu trúc Bộ VCNV) khỏi phần **thuộc game engine** (lượt chọn, chấm điểm khi chạy) |
| `docs/game-state-machine.md` | ✅ đã đọc §9 (bảy thang trạng thái) để xác nhận: **không** có `STATE-*`/`EVENT-*`/`T-*` nào thuộc trực tiếp EPIC-002 — xem §1 |
| `docs/rule-traceability.md` | ⚠️ **KHÔNG TỒN TẠI**. File thật là `docs/traceability.md` — đã đọc file này thay thế |
| `docs/reviews/prd-review.md` | ⚠️ **KHÔNG TỒN TẠI**. `docs/reviews/` chỉ có `game-rules-*.md` và `README.md`; theo `docs/reviews/README.md` đây là **kho lưu, không phải requirement**, nên việc thiếu file này không tạo lỗ hổng truy nguyên |

Ngoài ra đã đọc `docs/decisions.md` (`QĐ-041`…`QĐ-044`, `QĐ-063`, `QĐ-064`, `QĐ-066`, `QĐ-071`, `QĐ-082`, `QĐ-084`) và `docs/product-discovery.md` §5 E-2 vì `docs/PRD.md` khai chúng là nguồn của các `PRD-REQ-*` thuộc EPIC-002.

---

## 1. Phạm vi feature

**Goal (nguyên văn EPIC-002)**: *"Câu hỏi soạn một lần, dùng lại nhiều lần, không rò và không lặp."* — `docs/PRD.md` §11 EPIC-002.

**Product requirement trong phạm vi**:

| Requirement | Tiêu đề | Priority (PRD) |
|---|---|---|
| `PRD-REQ-007` | Soạn câu hỏi với metadata và media | P1 |
| `PRD-REQ-008` | Thời lượng suy nghĩ là metadata của TỪNG CÂU | P1 |
| `PRD-REQ-009` | Ba kiểu nhập đáp án, đáp án luôn là chuỗi | P1 |
| `PRD-REQ-010` | Câu thực hành là kênh trả lời thứ tư | P2 |
| `PRD-REQ-011` | Vòng duyệt `DRAFT` → `ACTIVE` do admin thực hiện | P2 |
| `PRD-REQ-012` | Cờ hiển thị của câu là giá trị DẪN XUẤT, chỉ đọc | P2 |
| `PRD-REQ-013` | Dấu vết "đã từng public" một chiều, chặn trận official | P1 |
| `PRD-REQ-014` | Tìm kiếm toàn văn, lọc và sắp xếp trên kho đề | P1 |
| `PRD-REQ-015` | Cắt khoảng trắng đầu cuối ở cả hai đầu | P2 — *gán **EPIC-002, EPIC-008***, phần thuộc EPIC-008 (bài làm lúc thi) nằm ở §8 |
| `PRD-REQ-047` | Rút đề trong danh sách đã gán; câu đã HIỂN THỊ không bao giờ trả lại kho | P1 — *gán **EPIC-006, EPIC-002***, chỉ vế **dữ liệu/cấu trúc** *(cờ `usedInContest` gắn trên câu, ranh giới "đã dùng")* thuộc feature này; vế **thực thi lúc chạy trận** *(rút ngẫu nhiên, phát `QUESTIONS_DRAWN`)* thuộc EPIC-006, xem §8 |
| `PRD-REQ-092` | Vượt chướng ngại vật chọn và rút theo BỘ | P1 — *gán **EPIC-002, EPIC-004, EPIC-006***, chỉ vế **soạn theo bộ và kiểm kho theo bộ** thuộc feature này; vế **chọn tay lúc chơi, chấm điểm hàng ngang** thuộc EPIC-006, xem §8 |

**Game rule trong phạm vi** (đúng những rule mà EPIC-002 và các `PRD-REQ-*` trên tham chiếu): `GR-031` *(rút đề và không lặp câu — chỉ vế thuộc dữ liệu kho đề: cờ `usedInContest`, cấu trúc Bộ VCNV, phép kiểm "bộ nguyên vẹn")* · `GR-027` *(chuẩn hoá và tô nổi bật đáp án — chỉ vế "đáp án luôn là chuỗi thống nhất cho cả ba kiểu nhập")* · `GR-019` *(câu hỏi thực hành — toàn bộ mô hình dữ liệu năm trường)* · `GR-006`, `GR-018`, `GR-013` *(tham chiếu gián tiếp qua `PRD-REQ-008`, `PRD-REQ-015` — dùng để minh hoạ cách `timeSeconds`/đáp án được tiêu thụ lúc chạy, không phải rule EPIC-002 sở hữu)*.

**State và transition liên quan trực tiếp**: **không có.** Bảy thang bậc trạng thái của `game-state-machine.md` (`STATE-001`…`043`) đều thuộc vòng đời một **trận** đang chạy; vòng đời `DRAFT` → `ACTIVE` của một câu hỏi là trạng thái **riêng của kho đề**, không thuộc thang bậc nào trong bảy thang đó (`TERM-018`). Câu hỏi có chạm `STATE-017` *"đã rút chưa hiển thị"* (`GR-031`) nhưng đó là trạng thái cấp **câu trong một trận đang chạy** — thuộc EPIC-006, xem §8.

**Actor**: `ACTOR-002` người ra đề (setter) *(chính)* · `ACTOR-001` admin *(duyệt đề, tìm kiếm/chọn câu, xác nhận hàng rào `everPublic`)*.

**Journey**: `JOURNEY-001` Chuẩn bị kho đề *(toàn bộ)* · `JOURNEY-002` Dựng contest *(chỉ vế tìm kiếm kho đề — phần "chọn danh sách câu cho MỘT contest cụ thể" thuộc EPIC-004)* · `JOURNEY-003` Chuyển sang bản portable *(chỉ vế `everPublic` đi theo câu qua nhập/xuất — phần cơ chế nhập/xuất thuộc EPIC-003)*.

---

## 2. User Scenarios & Testing *(mandatory)*

### US-001 — Soạn câu hỏi với metadata đầy đủ, thời lượng độc lập theo câu (Priority: P1)

- **Actor**: `ACTOR-002` setter
- **Intent**: Soạn một câu hỏi kèm đủ metadata — mã hiển thị, lĩnh vực, số chữ đáp án, giải thích, ghi chú, thời lượng suy nghĩ, mức điểm, gợi ý, media — và biết chắc thời lượng của câu này không bị suy ra từ mức điểm của nó.
- **User value**: Câu hỏi dùng lại được nhiều lần thay vì file rời trên đĩa — giải `PS-3`; hai câu cùng mức điểm vẫn có thể mang thời lượng khác nhau, đúng ý đồ tổng quát hoá vượt nguồn của dự án.
- **Why this priority**: `PRD-REQ-007` và `PRD-REQ-008` đều P1 — không có câu hỏi soạn sẵn thì không epic nào phía sau (contest builder, game engine) có gì để chạy.
- **Independent Test**: Setter tạo một câu với đầy đủ trường tuỳ chọn, lưu, mở lại — mọi trường giữ nguyên, media phát được. Tạo thêm một câu cùng mức điểm nhưng khác thời lượng — cả hai câu giữ đúng thời lượng riêng khi xem lại. Không cần trận, không cần contest.
- **Related PRD requirements**: `PRD-REQ-007`, `PRD-REQ-008`
- **Related game rules**: `GR-031`, `GR-018`, `GR-013`
- **Related journey**: `JOURNEY-001`

**Acceptance Scenarios**: AC-001, AC-001c, AC-002, AC-002b, AC-003, AC-004, AC-004b

---

### US-002 — Ba kiểu nhập đáp án, đáp án luôn lưu dưới dạng chuỗi (Priority: P1)

- **Actor**: `ACTOR-002` setter
- **Intent**: Chọn đúng widget trả lời cho câu hỏi của mình — gõ chữ, chọn một phương án, hoặc sắp thứ tự — mà không phải quan tâm việc chấm sẽ khác nhau giữa ba kiểu.
- **User value**: Máy không chấm nên không cần đánh giá "một lựa chọn" hay "một thứ tự" — giữ mọi đáp án dưới dạng chuỗi khiến cơ chế tô khác biệt ký tự của `GR-027` chạy nguyên cho cả ba kiểu, không cần nhánh riêng nào.
- **Why this priority**: `PRD-REQ-009` là P1 — không có kiểu nhập thì thí sinh mode nhập liệu không có gì để bấm ở Khởi động, Về đích, Câu hỏi phụ.
- **Independent Test**: Soạn ba câu cùng nội dung câu hỏi, khác `answerInputKind`; xem preview bài làm mẫu của cả ba đều hiện dưới dạng chuỗi cạnh đáp án, tô khác biệt theo cùng một cơ chế.
- **Related PRD requirements**: `PRD-REQ-009`
- **Related game rules**: `GR-027`
- **Related journey**: `JOURNEY-001`

**Acceptance Scenarios**: AC-005, AC-006

---

### US-003 — Soạn câu hỏi thực hành, kênh trả lời thứ tư (Priority: P2)

- **Actor**: `ACTOR-002` setter
- **Intent**: Khai một câu là "thực hành" — thí sinh thao tác dụng cụ thay vì gõ hay chọn — kèm hai bộ thời lượng khác nhau cho người thi chính và người cướp quyền, ghi chú dụng cụ, và tiêu chí để admin phán quyết "đạt".
- **User value**: Giữ đúng luật gốc Về đích, nơi câu thực hành có hai pha thời gian và hai bộ số khác nhau tuỳ ai đang thao tác.
- **Why this priority**: `PRD-REQ-010` là P2 — chỉ ảnh hưởng một kho trong bốn; trận vẫn chạy được nếu không có câu thực hành nào.
- **Independent Test**: Setter tạo một câu `isPractical = true` trong kho Về đích — thành công, lưu đủ năm trường riêng. Thử tạo cùng cờ đó ở một kho khác — bị chặn ngay tại kho đề, không phải lỗi lúc chạy trận.
- **Related PRD requirements**: `PRD-REQ-010`
- **Related game rules**: `GR-019`
- **Related journey**: `JOURNEY-001`

**Acceptance Scenarios**: AC-007, AC-008, AC-009, AC-010

---

### US-004 — Admin duyệt câu nháp thành câu dùng được (Priority: P2)

- **Actor**: `ACTOR-001` admin *(người duyệt)* · `ACTOR-002` setter *(người nhận phản hồi)*
- **Intent**: Xem hàng chờ câu mới soạn, duyệt cho dùng được hoặc trả về kèm lý do.
- **User value**: Một cửa kiểm duy nhất trước khi câu vào được danh sách chọn của bất kỳ contest nào, mà không phải phình thêm một vai riêng chỉ để duyệt.
- **Why this priority**: `PRD-REQ-011` là P2 — không có duyệt thì mọi câu setter lưu coi như dùng được ngay, mất một lớp kiểm nhưng không chặn được việc chạy trận.
- **Independent Test**: Câu mới lưu ở DRAFT — thử tìm nó trong màn chọn câu cho một contest, không thấy. Admin duyệt — câu xuất hiện được. Admin trả về kèm ghi chú ở một câu khác — câu vẫn DRAFT, ghi chú tới được setter, các trường khác không đổi.
- **Related PRD requirements**: `PRD-REQ-011`
- **Related game rules**: `GR-031`
- **Related journey**: `JOURNEY-001`

**Acceptance Scenarios**: AC-011, AC-012, AC-013, AC-014, AC-014b, AC-014c, AC-014d

---

### US-005 — Bộ đề, cờ hiển thị dẫn xuất, và hàng rào đề đã từng public (Priority: P1)

- **Actor**: `ACTOR-002` setter *(gom câu vào bộ đề)* · `ACTOR-001` admin *(xác nhận hàng rào)*
- **Intent**: Làm một câu thành public bằng cách đưa nó vào một bộ đề public — không có nút "đặt public" nào khác — và biết chắc câu đã từng lộ không bao giờ lọt vào một trận chính thức mà không ai hay biết.
- **User value**: Giải `PS-6` — đây là hàng rào chống rò đề cốt lõi: nếu ai cũng gỡ được nhãn public khỏi một câu đã lộ, hàng rào vô nghĩa; nếu câu đã lộ lọt vào trận thật một cách âm thầm, đề không còn kín.
- **Why this priority**: `PRD-REQ-013` là P1 — đây là cơ chế bảo mật, không phải tiện ích.
- **Independent Test**: Đưa một câu vào một bộ đề đang public — cờ hiển thị của câu chuyển PUBLIC ngay, và dấu vết "đã từng public" bật, không bao giờ tắt lại dù gỡ câu khỏi mọi bộ đề public sau đó. Thử thêm câu đó vào danh sách của một trận `official` — hệ thống chặn, đòi xác nhận hai bước, ghi nhật ký. Thử lại với trận `practice` — không chặn.
- **Related PRD requirements**: `PRD-REQ-012`, `PRD-REQ-013`
- **Related game rules**: `GR-031`
- **Related journey**: `JOURNEY-001`, `JOURNEY-002`, `JOURNEY-003`

**Acceptance Scenarios**: AC-015 → AC-021

---

### US-006 — Tìm kiếm, lọc và sắp xếp trên kho đề (Priority: P1)

- **Actor**: `ACTOR-001` admin
- **Intent**: Thu hẹp hàng nghìn câu trong kho về đúng tập cần cho một trận, bằng tìm kiếm toàn văn kết hợp lọc và sắp xếp.
- **User value**: Hệ thống cố ý không tự lấy đề (`GR-031`), nên toàn bộ gánh nặng chọn đề đổ lên công cụ tìm kiếm này — không có nó, chọn đề nghĩa là cuộn tay qua toàn kho.
- **Why this priority**: `PRD-REQ-014` là P1 — nó là điều kiện để `JOURNEY-002` (dựng contest) khả thi ở quy mô thật.
- **Independent Test**: Với một kho đề đủ lớn trải nhiều lĩnh vực và mức điểm, dùng tìm kiếm + lọc + sắp xếp để dựng đủ danh sách câu cho một trận chuẩn mà không cuộn qua toàn bộ kho. Không cần contest nào tồn tại — công cụ tìm kiếm tự nó kiểm được.
- **Related PRD requirements**: `PRD-REQ-014`
- **Related game rules**: `GR-031`
- **Related journey**: `JOURNEY-002`

**Acceptance Scenarios**: AC-022

---

### US-007 — Cắt khoảng trắng đầu-cuối khi soạn câu hỏi và đáp án (Priority: P2)

- **Actor**: `ACTOR-002` setter
- **Intent**: Gõ nội dung câu và đáp án mà không phải tự tay dọn khoảng trắng thừa ở hai đầu.
- **User value**: Khoảng trắng thừa phá việc đối chiếu ở `GR-027` và tạo báo động giả trên màn chấm của admin lúc chạy trận.
- **Why this priority**: `PRD-REQ-015` là P2 — một khoảng trắng thừa không chặn trận chạy, nhưng làm nhiễu bước chấm.
- **Independent Test**: Soạn một câu với đáp án có khoảng trắng thừa ở đầu/cuối, lưu — giá trị lưu trong kho đã được cắt, độc lập với mọi câu khác.
- **Related PRD requirements**: `PRD-REQ-015`
- **Related game rules**: `GR-027`, `GR-006`
- **Related journey**: `JOURNEY-001`

**Acceptance Scenarios**: AC-023

---

### US-008 — Soạn và kiểm kho Vượt chướng ngại vật theo BỘ nguyên vẹn (Priority: P1)

- **Actor**: `ACTOR-002` setter *(soạn theo bộ)* · `ACTOR-001` admin *(thấy kết quả kiểm kho)*
- **Intent**: Soạn nội dung VCNV như một khối gắn kết — 1 Chướng ngại vật + 4 hàng ngang mang số cố định + 1 câu ô trung tâm — và biết chắc hệ thống chỉ tính vòng VCNV chơi được khi có ít nhất một bộ còn nguyên vẹn, chứ không phải khi đủ số câu rời rạc.
- **User value**: Hàng ngang là gợi ý của chính Chướng ngại vật đó, mang số thứ tự ứng với một miếng ghép ở một góc cố định — phối một hàng ngang bất kỳ với một Chướng ngại vật bất kỳ vẫn "chạy trót lọt về kỹ thuật" nhưng phá mất thứ để thí sinh suy luận, và không phép kiểm runtime nào bắt được lỗi này; nó phải chặn ngay từ dữ liệu.
- **Why this priority**: `PRD-REQ-092` (phần thuộc EPIC-002) là P1 theo chính PRD — sai chỗ này không dựng lại được bằng một bản vá runtime.
- **Independent Test**: Soạn đủ một bộ VCNV (6 thành phần) — kiểm kho báo 1 bộ nguyên vẹn khả dụng. Đánh dấu một hàng ngang của bộ đó là đã dùng (mô phỏng việc nó bị mượn làm câu hỏi phụ ở một trận trước) — kiểm kho báo 0 bộ nguyên vẹn, dù năm thành phần kia còn nguyên và dù tổng số câu rời cộng lại vẫn đủ.
- **Related PRD requirements**: `PRD-REQ-092`
- **Related game rules**: `GR-031`
- **Related journey**: `JOURNEY-001`, `JOURNEY-002`

**Acceptance Scenarios**: AC-024 → AC-028

---

## 3. Acceptance Scenarios *(mandatory)*

> Quy ước: **Given** nêu đủ actor, trạng thái dữ liệu, dữ liệu ban đầu và tiền điều kiện · **When** chỉ chứa một hành động hoặc một sự kiện · **Then** kiểm kết quả nghiệp vụ, chuyển trạng thái, tác dụng phụ, kết quả lỗi, và **những gì KHÔNG được đổi**.

### US-001 — Soạn câu hỏi với metadata, thời lượng độc lập theo câu

**AC-001 — Happy path: soạn đủ mọi trường, kể cả tuỳ chọn**
- **US**: US-001 · **FR**: FR-001 · **GR**: `GR-031`
- **Given** setter đã đăng nhập, đang ở màn soạn câu mới, kho đề chưa có ràng buộc nào chặn
- **When** setter nhập đủ mã hiển thị, lĩnh vực, số chữ đáp án, giải thích, ghi chú, thời lượng suy nghĩ, mức điểm, danh sách gợi ý, tải lên media, rồi lưu
- **Then** câu được lưu ở `DRAFT` với đúng mọi giá trị đã nhập · media phát lại được khi mở xem · kho đề không câu nào khác bị đổi

**AC-001c — Rejection: mã hiển thị trùng bị từ chối**
- **US**: US-001 · **FR**: FR-001b
- **Given** kho đề đã có một câu mang mã hiển thị `KD-001`
- **When** setter soạn một câu mới và khai lại đúng mã hiển thị `KD-001`, rồi lưu
- **Then** hệ thống từ chối lưu · báo trùng mã · câu mới không được tạo · câu `KD-001` gốc **không đổi**

**AC-002 — No-change guarantee: chỉ điền trường bắt buộc, mở lại giữ nguyên**
- **US**: US-001 · **FR**: FR-001, FR-002
- **Given** setter đang soạn câu mới
- **When** setter chỉ điền mã hiển thị, lĩnh vực, số chữ đáp án, giải thích, ghi chú — bỏ trống thời lượng, mức điểm, gợi ý — rồi lưu, sau đó mở lại câu
- **Then** câu lưu ở `DRAFT` · các trường tuỳ chọn hiện đúng là **trống**, không phát sinh giá trị giả định nào ở tầng dữ liệu câu hỏi · mở lại lần nữa cho kết quả y hệt

**AC-002b — Side effect: mọi lần sửa ghi log diff và người thực hiện**
- **US**: US-001 · **FR**: FR-002b
- **Given** một câu đã lưu, giải thích đang là `"giải thích cũ"`
- **When** setter sửa trường giải thích thành `"giải thích mới"` rồi lưu
- **Then** hệ thống ghi một dòng nhật ký nêu đúng trường đã đổi (`explanation`), giá trị cũ, giá trị mới, và tài khoản đã thực hiện sửa · các trường không đổi của câu **không** xuất hiện trong dòng nhật ký này

**AC-003 — Boundary: cùng mức điểm, khác thời lượng, không suy ra nhau**
- **US**: US-001 · **FR**: FR-003 · **GR**: `GR-018`, `GR-013`
- **Given** kho đề có hai câu cùng mức điểm 20 — một câu khai `timeSeconds = 15`, một câu khai `timeSeconds = 40`
- **When** admin mở lại cả hai câu trong kho đề
- **Then** mỗi câu hiển thị đúng thời lượng riêng đã khai · không câu nào bị ghi đè bởi mức điểm hay bởi câu còn lại

**AC-004 — Rejection/default: câu không khai thời lượng riêng dùng mặc định preset**
- **US**: US-001 · **FR**: FR-004
- **Given** một câu không khai `timeSeconds`
- **When** contest áp preset `O26_DEFAULT@1`
- **Then** câu dùng thời lượng mặc định của preset ứng với mức điểm của nó · không báo lỗi thiếu dữ liệu · một câu khác có `timeSeconds` riêng trong cùng kho **không** bị đổi theo mặc định này

**AC-004b — Side effect: sửa câu đã hiển thị không khoá, nhưng mỗi lần hiển thị ghi log phiên bản đã dùng**
- **US**: US-001 · **FR**: FR-004b · **GR**: `GR-031`
- **Given** một câu `ACTIVE` đã từng hiển thị trong Trận A với nội dung đáp án `"Huế"`; sau đó setter sửa đáp án thành `"Đà Nẵng"`
- **When** setter lưu bản sửa
- **Then** câu lưu thành công, không bị khoá · nhật ký gắn với lần hiển thị ở Trận A vẫn ghi đúng `"Huế"` — nội dung đã dùng lúc đó — **không đổi theo** bản sửa mới · lần hiển thị kế tiếp ở một trận khác (nếu có) sẽ dùng và ghi log theo nội dung `"Đà Nẵng"` hiện tại

### US-002 — Ba kiểu nhập đáp án, đáp án luôn là chuỗi

**AC-005 — Happy path: ba kiểu nhập chọn độc lập, chỉ đổi widget**
- **US**: US-002 · **FR**: FR-005 · **GR**: `GR-027`
- **Given** setter soạn ba câu mới, cùng cấu trúc câu hỏi, khác đáp án
- **When** setter lần lượt chọn `answerInputKind` = `text`, `choice`, `ordering`, khai `options[]` cho hai câu sau, rồi lưu
- **Then** cả ba câu lưu thành công · câu `choice`/`ordering` có `options[]` khác rỗng · câu `text` không yêu cầu `options[]`

**AC-006 — No-change/invariant: đáp án luôn lưu dạng chuỗi bất kể kiểu nhập**
- **US**: US-002 · **FR**: FR-006 · **GR**: `GR-027`
- **Given** một câu `ordering` với `options[] = ["A","B","C","D"]`, đáp án đúng là thứ tự B, D, A, C
- **When** setter lưu đáp án chuẩn cho câu này
- **Then** đáp án lưu dưới dạng chuỗi `"B, D, A, C"` · cùng kiểu dữ liệu với đáp án của một câu `text` · không tồn tại trường lưu "thứ tự" hay "lựa chọn" nào khác ngoài chuỗi này

### US-003 — Câu hỏi thực hành, kênh trả lời thứ tư

**AC-007 — Happy path: câu thực hành hợp lệ ở kho Về đích**
- **US**: US-003 · **FR**: FR-007 · **GR**: `GR-019`
- **Given** setter đang soạn một câu mới trong kho Về đích
- **When** setter bật `isPractical = true`, điền đủ năm trường thực hành, rồi lưu
- **Then** câu lưu ở `DRAFT` với năm trường thực hành · không cảnh báo cấu hình nào

**AC-008 — Rejection: câu thực hành ngoài kho Về đích bị chặn tại kho đề**
- **US**: US-003 · **FR**: FR-007 · **GR**: `GR-019`
- **Given** setter đang soạn một câu mới trong kho Khởi động
- **When** setter bật `isPractical = true` rồi bấm lưu
- **Then** hệ thống từ chối lưu ngay tại thời điểm soạn, không phải lúc chạy trận · câu không được tạo · kho đề không đổi

**AC-009 — Boundary: hai bộ thời lượng thực hành tách biệt, không ghi đè nhau**
- **US**: US-003 · **FR**: FR-008 · **GR**: `GR-019`
- **Given** một câu thực hành mức điểm 30 đang soạn
- **When** setter khai `practiceSeconds = 60`, `stealPracticeSeconds = 40`, ghi chú dụng cụ, và tiêu chí đạt, rồi lưu và mở lại
- **Then** cả hai giá trị thời lượng lưu tách biệt · không giá trị nào bị ghi đè bởi giá trị kia khi mở lại

**AC-010 — No-change guarantee: tiêu chí đạt cùng mức bảo mật với đáp án**
- **US**: US-003 · **FR**: FR-009 · **GR**: `GR-019`
- **Given** một câu thực hành đã `ACTIVE`; một phiên mang vai Thí sinh không giữ quyền xem đáp án đang truy vấn thông tin câu này
- **When** phiên đó đọc dữ liệu câu qua bất kỳ đường nào hệ thống cung cấp cho vai Thí sinh
- **Then** trường `acceptanceCriteria` **không** xuất hiện trong dữ liệu trả về · các trường không nhạy cảm (ghi chú dụng cụ hiển thị cho ban tổ chức chuẩn bị, không cho thí sinh trong trận) vẫn theo đúng quy tắc riêng của chúng

### US-004 — Duyệt đề

**AC-011 — Invalid state: câu DRAFT không chọn được cho contest**
- **US**: US-004 · **FR**: FR-010
- **Given** một câu vừa được setter lưu, đang ở `DRAFT`
- **When** admin mở màn chọn danh sách câu cho một contest và tìm câu này
- **Then** câu **không** xuất hiện trong danh sách chọn được · không có cách ép qua từ màn này

**AC-012 — Happy path: duyệt câu chuyển ACTIVE**
- **US**: US-004 · **FR**: FR-011, FR-012
- **Given** một câu `DRAFT`, đang hiện trong hàng chờ duyệt của admin
- **When** admin bấm Duyệt
- **Then** câu chuyển `ACTIVE` · biến mất khỏi hàng chờ duyệt · xuất hiện được trong danh sách chọn cho contest từ thời điểm này

**AC-013 — Rejection: trả về DRAFT kèm ghi chú, không mất dữ liệu**
- **US**: US-004 · **FR**: FR-011
- **Given** một câu `DRAFT` khác, đang hiện trong hàng chờ duyệt
- **When** admin bấm Trả về kèm ghi chú lý do
- **Then** câu vẫn `DRAFT` · ghi chú lưu và hiển thị lại cho setter · mọi trường khác của câu **không đổi**

**AC-014 — Repeated action: duyệt lặp lại một câu đã ACTIVE**
- **US**: US-004 · **FR**: FR-013
- **Given** một câu đã `ACTIVE` từ trước
- **When** admin bấm Duyệt thêm lần nữa (do bấm nhầm hoặc màn chưa kịp cập nhật)
- **Then** câu vẫn `ACTIVE` · không sinh bản ghi duyệt trùng có ý nghĩa nghiệp vụ khác · trạng thái không đổi

**AC-014b — Concurrency: hai phiên admin thao tác gần như đồng thời trên cùng một câu DRAFT**
- **US**: US-004 · **FR**: FR-013b
- **Given** một câu đang `DRAFT`, hai phiên admin cùng đang mở màn duyệt của câu này
- **When** phiên A bấm Duyệt và phiên B bấm Trả về gần như cùng lúc, với yêu cầu của phiên A tới server trước theo timestamp
- **Then** câu chuyển `ACTIVE` theo quyết định của phiên A · yêu cầu Trả về của phiên B bị từ chối, **không** lật câu về `DRAFT` · cả hai thao tác đều ghi vào nhật ký, thao tác của B ghi kèm trạng thái bị từ chối

**AC-014c — Happy path: xoá mềm ở mọi trạng thái, biến mất khỏi tìm kiếm và chọn contest**
- **US**: US-004 · **FR**: FR-013c
- **Given** hai câu — một câu `DRAFT`, một câu `ACTIVE` — đều đang xuất hiện trong kết quả tìm kiếm kho đề
- **When** admin xoá mềm cả hai câu
- **Then** cả hai câu biến mất khỏi kết quả tìm kiếm · biến mất khỏi danh sách chọn được cho bất kỳ contest nào · không còn hiện ở hàng chờ duyệt nếu trước đó đang `DRAFT`

**AC-014d — No-change guarantee: xoá mềm không đụng dữ liệu lịch sử**
- **US**: US-004 · **FR**: FR-013d
- **Given** một câu `ACTIVE`, `everPublic = true`, đã có log hiển thị gắn với một trận trước đó (theo FR-004b)
- **When** admin xoá mềm câu này
- **Then** `everPublic` của câu **không đổi** · log hiển thị gắn với trận trước đó **không đổi**, trận đó vẫn phát lại đúng nội dung đã ghi

### US-005 — Bộ đề, cờ hiển thị dẫn xuất, hàng rào `everPublic`

**AC-015 — Happy path: visibility PUBLIC khi thuộc ≥1 bộ đề public**
- **US**: US-005 · **FR**: FR-014, FR-015
- **Given** một câu `ACTIVE`, hiện chưa thuộc bộ đề nào, `visibility = PRIVATE`
- **When** setter thêm câu vào một bộ đề đang public
- **Then** `visibility` của câu chuyển `PUBLIC` ngay lập tức

**AC-016 — No-change guarantee: visibility về PRIVATE khi không còn ở bộ đề public nào**
- **US**: US-005 · **FR**: FR-014, FR-015
- **Given** một câu đang `PUBLIC` vì thuộc đúng một bộ đề public
- **When** setter gỡ câu khỏi bộ đề public đó, và câu không thuộc bộ đề public nào khác
- **Then** `visibility` chuyển về `PRIVATE` · dấu vết "đã từng public" của câu **không đổi** — vẫn giữ nguyên như trước thao tác gỡ

**AC-017 — Invalid state: không có đường đặt trực tiếp visibility**
- **US**: US-005 · **FR**: FR-015b
- **Given** một vai bất kỳ đang xem chi tiết một câu hỏi
- **When** vai đó gọi thẳng một yêu cầu đổi trực tiếp trường `visibility`, bỏ qua thao tác gán/gỡ bộ đề
- **Then** server từ chối yêu cầu · `visibility` của câu **không đổi**

**AC-018 — No-change guarantee: everPublic bật một chiều, không tắt lại được**
- **US**: US-005 · **FR**: FR-016
- **Given** một câu `ACTIVE`, `everPublic = false`
- **When** câu được thêm vào một bộ đề public lần đầu tiên, sau đó bị gỡ khỏi mọi bộ đề public
- **Then** `everPublic` chuyển `true` ngay khi thêm lần đầu · vẫn giữ `true` sau khi gỡ, không có thao tác nào đưa nó về `false`

**AC-019 — Rejection có thể ép qua: hàng rào chặn tại pre-flight, ép được bằng xác nhận hai bước**
- **US**: US-005 · **FR**: FR-017
- **Given** một contest `official` đang ở `LOBBY`; một câu `everPublic = true` được admin thêm vào danh sách câu đã gán
- **When** admin bấm thêm câu này vào danh sách
- **Then** hệ thống chặn và cảnh báo · admin xác nhận qua đúng hai bước riêng biệt thì câu vẫn thêm được · toàn bộ diễn biến (chặn, xác nhận, hoặc thêm thành công) ghi vào nhật ký

**AC-020 — Boundary/idempotency: hàng rào áp nhất quán ở mọi cửa vào, kể cả nhập gói**
- **US**: US-005 · **FR**: FR-017
- **Given** một gói contest được nhập vào hệ thống, trong đó danh sách gán cho trận `official` có một câu `everPublic = true`
- **When** admin thực hiện nhập gói
- **Then** cùng hàng rào chặn + xác nhận hai bước + ghi nhật ký được áp — không có đường nhập lách qua hàng rào so với thêm thủ công ở `LOBBY`

**AC-021 — No-change guarantee: hàng rào không áp cho trận practice**
- **US**: US-005 · **FR**: FR-018
- **Given** một trận `practice`, danh sách câu gán gồm một câu `everPublic = true`
- **When** admin thêm câu này vào danh sách của trận practice
- **Then** hệ thống thêm câu bình thường · không cảnh báo · không đòi xác nhận hai bước · không ghi nhật ký dạng "ép qua hàng rào"

### US-006 — Tìm kiếm kho đề

**AC-022 — Happy path: tìm kiếm + lọc + sắp xếp đủ dựng danh sách cho một trận chuẩn**
- **US**: US-006 · **FR**: FR-020 · **GR**: `GR-031`
- **Given** kho đề có đủ số câu tối thiểu cho một trận chuẩn theo `game-rules.md` §2.1, trải trên nhiều lĩnh vực và mức điểm
- **When** admin dùng tìm kiếm toàn văn kết hợp lọc theo lĩnh vực/mức điểm và sắp xếp theo mã hiển thị
- **Then** admin thu hẹp và chọn đủ số câu cho từng vòng của một trận chuẩn mà không phải cuộn qua toàn bộ danh sách kho đề

### US-007 — Trim khoảng trắng đầu-cuối

**AC-023 — Happy path: khoảng trắng thừa bị cắt ở cả UI và server**
- **US**: US-007 · **FR**: FR-021 · **GR**: `GR-027`, `GR-006`
- **Given** setter soạn một câu, gõ nội dung câu hỏi và đáp án có khoảng trắng thừa ở đầu và cuối (`"  Huế  "`)
- **When** setter bấm lưu
- **Then** giá trị lưu trong kho là `"Huế"` — khoảng trắng bị cắt cả trước khi gửi (UI) lẫn khi lưu (server), không phụ thuộc riêng vào một tầng

### US-008 — Bộ VCNV nguyên vẹn

**AC-024 — Happy path: bộ là đơn vị soạn, không tách rời hàng ngang**
- **US**: US-008 · **FR**: FR-022 · **GR**: `GR-031`
- **Given** setter đang soạn nội dung cho vòng VCNV, chưa có bộ nào tồn tại
- **When** setter tạo một bộ mới
- **Then** hệ thống yêu cầu đủ sáu thành phần trong cùng một bộ — 1 Chướng ngại vật, 4 hàng ngang, 1 câu ô trung tâm · không cho lưu một hàng ngang độc lập ngoài một bộ nào

**AC-025 — Invalid state: số thứ tự hàng ngang cố định, không hoán đổi**
- **US**: US-008 · **FR**: FR-023 · **GR**: `GR-031`
- **Given** một bộ VCNV đã lưu, hàng ngang 1-4 mỗi số gắn với một miếng ghép góc cố định
- **When** setter hoặc admin thử đổi số thứ tự của hàng ngang 2 thành 3 trong cùng bộ, hoặc gán hàng ngang số 1 của bộ này sang một bộ khác
- **Then** hệ thống từ chối cả hai thao tác · số thứ tự và bộ sở hữu của mỗi hàng ngang **không đổi**

**AC-026 — Boundary: bộ nguyên vẹn khả dụng khi đủ sáu thành phần chưa dùng**
- **US**: US-008 · **FR**: FR-024 · **GR**: `GR-031`
- **Given** một bộ VCNV có cả sáu thành phần đều chưa đánh dấu đã dùng (`usedInContest = false`) cho contest đang xét
- **When** hệ thống kiểm kho tại cửa vào vòng VCNV của contest đó
- **Then** bộ được tính là khả dụng, đóng góp vào số bộ nguyên vẹn còn dùng được

**AC-027 — Boundary/no-change: mất một thành phần thì vỡ bộ, dù năm thành phần còn nguyên**
- **US**: US-008 · **FR**: FR-024 · **GR**: `GR-031`
- **Given** một bộ VCNV có đúng một hàng ngang đã bị đánh dấu đã dùng (ví dụ đã mượn làm Câu hỏi phụ ở một trận trước của cùng contest), năm thành phần còn lại chưa dùng
- **When** hệ thống kiểm kho tại cửa vào vòng VCNV cho contest đó
- **Then** bộ này được tính là **không khả dụng** (vỡ bộ) cho vòng VCNV · năm thành phần còn lại **không** bị đếm riêng vào số câu rời rạc còn dùng được — chúng đơn thuần không thuộc bộ nào còn nguyên vẹn

**AC-028 — Invalid state: kiểm kho VCNV đếm bộ, không đếm câu**
- **US**: US-008 · **FR**: FR-025 · **GR**: `GR-031`
- **Given** kho VCNV của một contest có 4 hàng ngang rời và 1 Chướng ngại vật rời, đủ số lượng nhưng không cùng thuộc một bộ nguyên vẹn nào (mỗi bộ liên quan đều vỡ vì thiếu ít nhất một thành phần)
- **When** hệ thống kiểm kho tại cửa vào vòng VCNV
- **Then** kết quả kiểm là **0 bộ nguyên vẹn khả dụng**, dù tổng số câu rời cộng lại đủ 4 hàng ngang + 1 Chướng ngại vật — phép kiểm không quy đổi số câu rời thành một bộ

---

## 4. Functional Requirements *(mandatory)*

> Mỗi FR là một phát biểu kiểm thử được, tham chiếu US-NNN, PRD-REQ-NNN, GR-NNN và AC-NNN. Không FR nào nêu framework, cơ sở dữ liệu, đường API, lớp, hàm hay thuật toán.

### Nhóm A — Soạn câu hỏi và metadata

- **FR-001**: Hệ thống MUST cho phép người ra đề tạo và lưu câu hỏi với các trường: mã hiển thị, lĩnh vực, số chữ đáp án, giải thích, ghi chú, thời lượng suy nghĩ *(tuỳ chọn)*, mức điểm *(tuỳ chọn)*, danh sách gợi ý *(tuỳ chọn)*, và media. · US-001 · `PRD-REQ-007` · `GR-031` · AC-001, AC-002
- **FR-001b**: Mã hiển thị (`displayId`) của một câu hỏi MUST **duy nhất trên toàn hệ thống**; hệ thống MUST từ chối lưu một câu mới hoặc một bản sửa mang mã hiển thị đã tồn tại ở một câu khác. Mã hiển thị MUST mang một tiền tố ngắn chỉ loại câu. *(`OQ-002`, xác nhận 2026-07-30)* · US-001 · `PRD-REQ-007` · — · AC-001c
- **FR-002**: Câu hỏi đã lưu và mở lại MUST giữ nguyên toàn bộ giá trị của mọi trường đã nhập, kể cả khi các trường tuỳ chọn để trống. · US-001 · `PRD-REQ-007` · — · AC-002
- **FR-002b**: Mỗi lần một câu hỏi bị sửa, hệ thống MUST ghi một dòng nhật ký nêu **nội dung đã đổi** (dạng khác biệt giữa bản cũ và bản mới, cho từng trường) và **ai đã sửa**. *(`OQ-006`, xác nhận 2026-07-30)* · US-001 · `PRD-REQ-007` · — · AC-002b
- **FR-003**: Thời lượng suy nghĩ của một câu hỏi MUST là thuộc tính lưu riêng cho câu đó; hệ thống MUST NOT suy thời lượng từ mức điểm của câu. Hệ thống chọn câu theo mức điểm, MUST lấy thời lượng theo từng câu. · US-001 · `PRD-REQ-008` · `GR-018`, `GR-013` · AC-003
- **FR-004**: Khi một câu không khai thời lượng riêng, hệ thống MUST dùng giá trị mặc định của preset đang áp cho contest; giá trị khai riêng của một câu MUST luôn thắng giá trị mặc định của preset. · US-001 · `PRD-REQ-008` · `GR-018` · AC-004
- **FR-004b**: Một câu `ACTIVE` MUST sửa được tự do kể cả sau khi đã từng hiển thị trong một trận; hệ thống MUST NOT khoá sửa dựa trên lịch sử hiển thị. Mỗi lần câu được hiển thị trong một trận, hệ thống MUST ghi một dòng nhật ký nêu nội dung/phiên bản của câu tại đúng thời điểm hiển thị đó, gắn với trận tương ứng — đủ để phát lại trận cũ đúng với nội dung đã dùng thật, độc lập với bản mới nhất của câu. *(`OQ-004`, xác nhận qua `/speckit-clarify` 2026-07-30 — chưa vào `docs/decisions.md`; đây KHÔNG phải một hệ thống quản lý phiên bản cho setter; xem §8)* · US-001 · `PRD-REQ-007` · `GR-031` · AC-004b

### Nhóm B — Ba kiểu nhập đáp án, đáp án luôn là chuỗi

- **FR-005**: Hệ thống MUST hỗ trợ đúng ba kiểu nhập đáp án — `text`, `choice`, `ordering` — và kiểu nhập MUST chỉ quyết định widget hiển thị trên máy thí sinh, MUST NOT ảnh hưởng cách lưu trữ. · US-002 · `PRD-REQ-009` · `GR-027` · AC-005
- **FR-006**: Đáp án và bài làm MUST được lưu dưới dạng chuỗi ở cả ba kiểu nhập; hệ thống MUST NOT có nhánh xử lý hay lưu trữ riêng theo từng kiểu nhập. · US-002 · `PRD-REQ-009` · `GR-027` · AC-006

### Nhóm C — Câu hỏi thực hành

- **FR-007**: Câu hỏi khai `isPractical = true` MUST chỉ hợp lệ khi thuộc kho Về đích; hệ thống MUST chặn việc lưu một câu thực hành thuộc kho khác ngay tại thời điểm soạn, MUST NOT để lỗi này trôi tới lúc chạy trận. · US-003 · `PRD-REQ-010` · `GR-019` · AC-007, AC-008
- **FR-008**: Câu hỏi thực hành MUST lưu đủ năm trường riêng — cờ thực hành, thời lượng thực hành của người thi chính, thời lượng thực hành của người cướp quyền, ghi chú dụng cụ, và tiêu chí đạt — và hai bộ thời lượng MUST lưu tách biệt, không ghi đè lẫn nhau. · US-003 · `PRD-REQ-010` · `GR-019` · AC-009
- **FR-009**: Trường tiêu chí đạt của câu thực hành MUST chịu cùng mức bảo mật với đáp án của câu thường — MUST NOT rời server tới vai không giữ quyền xem đáp án. · US-003 · `PRD-REQ-010` · `GR-019` · AC-010

### Nhóm D — Duyệt đề

- **FR-010**: Câu hỏi mới soạn MUST ở trạng thái nháp (`DRAFT`) và MUST NOT chọn được cho danh sách câu của bất kỳ contest nào cho tới khi admin duyệt. · US-004 · `PRD-REQ-011` · `GR-031` · AC-011
- **FR-011**: Hệ thống MUST hiển thị hàng chờ duyệt câu nháp trên bảng điều khiển của admin; admin MUST duyệt được (chuyển `ACTIVE`) hoặc trả về nháp kèm ghi chú lý do. · US-004 · `PRD-REQ-011` · — · AC-012, AC-013
- **FR-012**: Hệ thống MUST NOT có vai riêng cho việc duyệt câu hỏi — chỉ vai admin thực hiện được thao tác này. · US-004 · `PRD-REQ-011` · — · AC-012
- **FR-013**: Duyệt lặp lại một câu đã `ACTIVE` MUST NOT đổi trạng thái câu và MUST NOT sinh tác dụng phụ nghiệp vụ nào khác ngoài lần duyệt đầu tiên. · US-004 · `PRD-REQ-011` · — · AC-014
- **FR-013b**: Khi hai phiên admin cùng thao tác Duyệt và Trả về gần như đồng thời trên cùng một câu đang `DRAFT`, quyết định **tới server trước** theo server timestamp MUST có hiệu lực; quyết định **tới sau** MUST bị từ chối và MUST NOT lật kết quả đã phân giải. Quyết định tới sau MUST vẫn được ghi vào nhật ký kèm trạng thái bị từ chối. *(`OQ-006`, xác nhận 2026-07-30)* · US-004 · `PRD-REQ-011` · — · AC-014b
- **FR-013c**: Hệ thống MUST hỗ trợ **xoá mềm** một câu hỏi ở bất kỳ trạng thái nào — `DRAFT` hoặc `ACTIVE`. Câu đã xoá mềm MUST NOT xuất hiện trong kết quả tìm kiếm (FR-020) hay trong danh sách chọn được cho một contest. *(`OQ-005`, xác nhận 2026-07-30)* · US-004 · `PRD-REQ-011` · — · AC-014c
- **FR-013d**: Xoá mềm một câu hỏi MUST NOT xoá hay thay đổi bất kỳ dữ liệu nào đã ghi cho câu đó — `everPublic`, và mọi log nội dung/phiên bản đã gắn với các lần hiển thị trước đó (FR-004b). *(`OQ-005`, xác nhận 2026-07-30)* · US-004 · `PRD-REQ-011` · — · AC-014d

### Nhóm E — Bộ đề, cờ hiển thị dẫn xuất, hàng rào `everPublic`

- **FR-014**: **Bộ đề** MUST là một entity riêng, độc lập với kho đề toàn hệ thống, mang quan hệ thành viên **nhiều-nhiều** với câu hỏi — một câu thuộc được nhiều bộ đề, một bộ đề chứa nhiều câu; mỗi bộ đề MUST tự mang thuộc tính `public`/`private` của chính nó (`OQ-001`, xác nhận **2026-07-30**). · US-005 · `PRD-REQ-012` · `GR-031` · AC-015, AC-016
- **FR-015**: Cờ hiển thị (`visibility`) của một câu hỏi MUST là giá trị dẫn xuất, đúng bằng `PUBLIC` khi và chỉ khi câu đang thuộc ít nhất một bộ đề `public` (theo FR-014), ngược lại `PRIVATE`; giá trị này MUST cập nhật ngay khi quan hệ thành viên với bộ đề đổi. · US-005 · `PRD-REQ-012` · `GR-031` · AC-015, AC-016
- **FR-015b**: Hệ thống MUST NOT cung cấp bất kỳ đường nào — giao diện hay server — để đặt trực tiếp cờ hiển thị của một câu hỏi; vai người ra đề chỉ đổi cờ này bằng cách thêm hoặc gỡ câu khỏi một bộ đề. · US-005 · `PRD-REQ-012` · `GR-031` · AC-017
- **FR-016**: Ngay khi một câu hỏi lần đầu thuộc về một bộ đề public, hệ thống MUST đặt dấu vết "đã từng public" cho câu đó; dấu vết này MUST một chiều và vĩnh viễn, MUST NOT có thao tác nào đưa nó về trạng thái ban đầu. · US-005 · `PRD-REQ-013` · — · AC-018
- **FR-017**: Bất cứ khi nào một câu mang dấu vết "đã từng public" được thêm vào danh sách câu đã gán của một trận `official` — dù ở bước kiểm trước trận, sửa danh sách tại `LOBBY`, hay nhập gói contest — hệ thống MUST áp cùng một hàng rào: chặn mặc định, cho ép qua bằng xác nhận hai bước, và ghi nhật ký. · US-005 · `PRD-REQ-013` · — · AC-019, AC-020
- **FR-018**: Hàng rào ở FR-017 MUST NOT áp dụng cho trận `practice`. · US-005 · `PRD-REQ-013` · — · AC-021
- **FR-019**: Dấu vết "đã từng public" MUST đi theo câu hỏi qua các thao tác nhập/xuất kho đề. · US-005 · `PRD-REQ-013` · — · *(kiểm ở EPIC-003, xem §8)*

### Nhóm F — Tìm kiếm kho đề

- **FR-020**: Hệ thống MUST cho phép tìm câu hỏi trong kho đề bằng tìm kiếm toàn văn kết hợp bộ lọc và sắp xếp, đủ để dựng danh sách câu cho một trận chuẩn mà không phải cuộn qua toàn bộ kho. · US-006 · `PRD-REQ-014` · `GR-031` · AC-022

### Nhóm G — Vệ sinh dữ liệu văn bản

- **FR-021**: Mọi nội dung văn bản của câu hỏi và đáp án — bao gồm cả khi setter nhập ở giao diện lẫn khi server lưu — MUST được cắt khoảng trắng đầu-cuối. · US-007 · `PRD-REQ-015` · `GR-027`, `GR-006` · AC-023

### Nhóm H — Bộ VCNV

- **FR-022**: Đơn vị soạn và đơn vị chọn/rút của vòng VCNV MUST là một bộ gồm đúng 1 Chướng ngại vật, 4 hàng ngang, và 1 câu ô trung tâm; hệ thống MUST NOT cho soạn hoặc lưu một hàng ngang như một câu độc lập ngoài một bộ. · US-008 · `PRD-REQ-092` · `GR-031` · AC-024
- **FR-023**: Bốn hàng ngang trong một bộ VCNV MUST mang số thứ tự cố định 1 đến 4, mỗi số ứng với một miếng ghép ở một góc cố định; hệ thống MUST NOT cho hoán đổi số thứ tự giữa các hàng ngang, dù trong cùng bộ hay giữa các bộ khác nhau. · US-008 · `PRD-REQ-092` · `GR-031` · AC-025
- **FR-024**: Hệ thống MUST tính một bộ VCNV là khả dụng cho vòng VCNV khi và chỉ khi cả sáu thành phần của bộ đều chưa được đánh dấu đã dùng; mất một thành phần MUST làm toàn bộ bộ đó không khả dụng cho vòng VCNV, dù năm thành phần còn lại chưa dùng. · US-008 · `PRD-REQ-092` · `GR-031` · AC-026, AC-027
- **FR-025**: Phép kiểm kho cho vòng VCNV MUST đếm số bộ nguyên vẹn khả dụng; MUST NOT đếm số câu hàng ngang hay số câu ô trung tâm rời rạc như điều kiện đủ. · US-008 · `PRD-REQ-092` · `GR-031` · AC-028
- **FR-026**: Nhập và xuất kho đề MUST giữ nguyên cấu trúc bộ VCNV — một bộ MUST luôn nhập/xuất trọn vẹn cùng nhau với đủ sáu thành phần, MUST NOT tách rời một phần của bộ sang một thao tác nhập/xuất khác. · US-008 · `PRD-REQ-092` · `GR-031` · *(kiểm ở EPIC-003, xem §8)*

---

## 5. Key Entities

- **Câu hỏi** (`Question`, `TERM-044`): đơn vị đề thi. Trường cốt lõi — mã hiển thị *(`displayId`, duy nhất toàn hệ thống, mang tiền tố chỉ loại câu)*, lĩnh vực, số chữ đáp án, giải thích, ghi chú, thời lượng suy nghĩ *(tuỳ chọn)*, mức điểm *(tuỳ chọn)*, gợi ý *(tuỳ chọn)*, media, `answerInputKind`, `options[]`, đáp án chuẩn *(luôn là chuỗi)*, trạng thái `DRAFT`/`ACTIVE`, `visibility` *(dẫn xuất)*, `everPublic` *(một chiều)*. Năm trường thực hành *(`isPractical`, `practiceSeconds`, `stealPracticeSeconds`, `equipmentNote`, `acceptanceCriteria`)* chỉ hợp lệ khi câu thuộc kho Về đích. Ngoài `displayId`, câu còn mang một **định danh nội bộ ổn định**, riêng biệt với `displayId`, dùng để tham chiếu xuyên hệ thống — không đổi kể cả khi `displayId` bị sửa *(`OQ-002`)*. Câu còn mang một cờ **xoá mềm**, đặt được ở bất kỳ trạng thái `DRAFT`/`ACTIVE` nào; câu đã xoá mềm ẩn khỏi tìm kiếm và khỏi danh sách chọn cho contest nhưng không mất dữ liệu *(`OQ-005`)*.
- **Bộ đề** (xác nhận `OQ-001` **2026-07-30**): entity riêng, độc lập với kho đề toàn hệ thống. Mang quan hệ **nhiều-nhiều** với câu hỏi — một câu thuộc được nhiều bộ đề, một bộ đề chứa nhiều câu. Mỗi bộ đề tự mang thuộc tính `public`/`private` của chính nó; `visibility` của một câu là **OR** trên tập bộ đề nó thuộc (FR-014, FR-015).
- **Bộ VCNV** (`obstacleSet`, `TERM-058`): đơn vị soạn/chọn/rút của vòng Vượt chướng ngại vật — 1 Chướng ngại vật *(từ khoá ẩn + hình ảnh 5 miếng ghép)* + 4 hàng ngang *(số thứ tự cố định 1-4)* + 1 câu ô trung tâm. Khả dụng cho vòng VCNV **khi và chỉ khi** cả sáu thành phần đều chưa đánh dấu đã dùng.
- **Cờ `usedInContest`** (`TERM-048`): đánh dấu một câu đã hiển thị cho thí sinh trong một contest cụ thể; phạm vi **(câu, contest)**, không đặt lại kể cả khi vòng bị bỏ. Feature này chỉ sở hữu việc câu **mang** được cờ này; việc **đặt/đọc** cờ lúc rút đề và hiển thị thuộc EPIC-006 *(xem §8)*.
- **Kho đề · Pool** (`TERM-046`): tập câu hỏi khả dụng ở phạm vi toàn hệ thống, phân biệt với **pool đã gán cho một contest** *(snapshot chọn trước khi start — thuộc EPIC-004)* và **pool còn lại sau no-repeat** *(thuộc EPIC-006)*.

---

## 6. Success Criteria *(mandatory)*

- **SC-001**: Người ra đề soạn xong một câu hỏi đầy đủ metadata trong một phiên thao tác liên tục, không mất dữ liệu khi mở lại — đo bằng so khớp từng trường giữa lúc lưu và lúc mở lại, tỉ lệ sai khác = 0%.
- **SC-002**: Admin dựng đủ danh sách câu cho một trận chuẩn *(Khởi động 24 lượt riêng + 12 lượt chung, 1 bộ VCNV, Tăng tốc 4, Về đích 12)* bằng tìm kiếm/lọc mà **không** phải cuộn qua toàn bộ kho đề.
- **SC-003**: 0% câu mang dấu vết "đã từng public" lọt vào danh sách gán của một trận `official` mà không đi qua xác nhận hai bước và không để lại dòng nhật ký tương ứng — đo trên toàn bộ ba cửa vào *(pre-flight, sửa ở `LOBBY`, nhập gói)*.
- **SC-004**: 100% bộ VCNV được hệ thống đánh giá đúng tình trạng nguyên vẹn *(đủ 6/6 thành phần chưa dùng)* tại mọi lần kiểm kho — đo bằng bài kiểm lặp lại phép kiểm ở nhiều tổ hợp thiếu 1-6 thành phần.
- **SC-005**: 0 đường trong giao diện hoặc server cho phép đặt trực tiếp cờ hiển thị của một câu hỏi ngoài thao tác gán/gỡ bộ đề — đo bằng rà soát toàn bộ điểm vào ghi dữ liệu câu hỏi.

---

## 7. Edge Cases

**Giá trị biên**
- Câu không khai bất kỳ trường tuỳ chọn nào (thời lượng, mức điểm, gợi ý) vẫn phải lưu và mở lại được — không trường nào được phép sinh giá trị mặc định giả định ở tầng dữ liệu câu hỏi *(AC-002)*.
- Bộ VCNV vừa đủ 6/6 thành phần chưa dùng chuyển ngay sang "vỡ bộ" khi đúng một thành phần đổi trạng thái *(AC-026, AC-027)*.

**Trạng thái không hợp lệ**
- Soạn câu `isPractical = true` ngoài kho Về đích *(AC-008)*.
- Gọi thẳng thao tác đặt trực tiếp `visibility` *(AC-017)*.
- Soạn một hàng ngang VCNV độc lập, ngoài một bộ *(AC-024)*.
- Đổi số thứ tự hàng ngang trong bộ hoặc chuyển hàng ngang giữa các bộ *(AC-025)*.

**Thao tác lặp**
- Bấm Duyệt một câu đã `ACTIVE` nhiều lần *(AC-014)*.
- Thêm cùng một câu vào cùng một bộ đề nhiều lần — hệ thống MUST giữ quan hệ thành viên là tập hợp, không nhân bản.
- Gỡ rồi thêm lại một câu vào cùng một bộ đề public nhiều lần — `everPublic` chỉ bật một lần và không bao giờ tắt, bất kể số lần lặp *(AC-018)*.

**Trạng thái cũ / sự kiện trùng**
- Hai phiên admin cùng thao tác trên một câu đang chờ duyệt: quyết định tới trước theo server timestamp thắng, quyết định tới sau bị từ chối và ghi nhật ký, không lật kết quả *(AC-014b)*.
- Câu `ACTIVE` bị sửa sau khi đã hiển thị ở một trận trước: bản sửa **không** viết lại log của lần hiển thị cũ — log đó đứng yên theo nội dung tại thời điểm hiển thị, độc lập với bản mới nhất của câu *(AC-004b)*.
- Câu bị xoá mềm trong khi đang là thành phần của một bộ đề hoặc một bộ VCNV: quan hệ thành viên **không tự động gỡ** *(chưa xác nhận tường minh)*, nhưng câu vẫn biến mất khỏi tìm kiếm và khỏi danh sách chọn cho contest *(AC-014c)* — nghĩa là một bộ VCNV có thành phần bị xoá mềm nhiều khả năng không còn chọn/kiểm được như một bộ nguyên vẹn cho contest mới, dù bản thân thao tác xoá mềm không định nghĩa tường minh phép cộng dồn này. Thao tác **phục hồi** câu đã xoá mềm chưa được xác nhận là có tồn tại hay không.

**Hỏng một phần**
- Tải media thất bại giữa chừng khi lưu câu — hành vi cụ thể (giữ câu ở trạng thái gì, có cho lưu câu không kèm media hay không) chưa được nguồn nào quy định *(xem `OQ-007`)*.

**Luật xung đột — đã phân xử**
- `TERM-046` liệt "bộ đề" như một tên khác của "kho đề" (pool); spec này **đọc theo** `PRD-REQ-012`/`PRD-REQ-013` và `QĐ-063` — "bộ đề" là một entity riêng có thành viên (xác nhận `OQ-001`, **2026-07-30**, xem §Clarifications). `TERM-046` cần sửa ở lần cập nhật `docs/glossary.md` kế tiếp; đây không phải việc của spec này.

**Hành vi nguồn không quy định** — hai mục còn mở: `OQ-003` (giới hạn số lượng trường mảng), `OQ-007` (hỏng media giữa chừng) ở §9; spec này không tự điền các hành vi đó.

---

## 8. Out of Scope

**Requirement thuộc epic khác, không đưa vào feature này**

| Ngoài phạm vi | Thuộc về |
|---|---|
| Chọn danh sách câu cụ thể cho MỘT contest, chụp danh sách vào cấu hình trận, cấu hình luật, mode trả lời | EPIC-004 *(`JOURNEY-002`)* |
| Xuất/nhập gói contest — cơ chế nhập/xuất, giữ định danh câu ổn định qua nhập/xuất, đánh dấu "đã dùng" hàng loạt bằng tay, bản kê câu đã dùng | EPIC-003 *(`PRD-REQ-016`…`018`, `094`, `095`)* — feature này chỉ yêu cầu `everPublic` và cấu trúc Bộ VCNV **đi theo** câu qua nhập/xuất *(FR-019, FR-026)*, không sở hữu cơ chế nhập/xuất |
| Rút đề ngẫu nhiên lúc chạy trận, phát event `QUESTIONS_DRAWN`, đặt cờ `usedInContest` tại mốc hiển thị, kiểm kho tại cửa vào từng vòng **lúc đang chạy trận** | EPIC-006 *(`PRD-REQ-047`, `GR-031` phần thực thi)* — feature này chỉ sở hữu việc câu **mang** được cờ và **cấu trúc** để phép kiểm đó chạy đúng |
| Lượt chọn hàng ngang, chấm điểm hàng ngang, bấm chuông giải Chướng ngại vật, gợi ý cuối — toàn bộ bảng quyết định `GR-007`…`GR-011` ngoài vế cấu trúc bộ | EPIC-006 *(`PRD-REQ-092` phần thực thi)* |
| Phán quyết Đúng/Sai một bài làm cụ thể lúc chạy trận, cơ chế tô nổi bật khi CHẤM | EPIC-006, EPIC-007 — feature này chỉ sở hữu việc đáp án được **lưu** dưới dạng chuỗi thống nhất *(FR-006)*, không sở hữu thao tác chấm |
| Xác thực, vai, permission, phiên giữ quyền điều khiển | EPIC-001 |
| Bảo mật đáp án khi phát tới client trong một trận đang chạy, mốc câu khép, `PERM-045` | EPIC-006, EPIC-009 — feature này chỉ tham chiếu để đặt đúng mức bảo mật cho `acceptanceCriteria` lúc soạn *(FR-009)* |
| Đóng gói, giới hạn kích thước media theo env config | EPIC-012 |

**Non-goal của chính EPIC-002** *(`docs/PRD.md` §11, §20.1)*
- Đánh phiên bản khi sửa câu đã duyệt — cắt khỏi v1, theo nghĩa hẹp: không có UI xem lịch sử phiên bản, không diff, không rollback. Vẫn có log nội dung/phiên bản gắn với từng lần hiển thị trong trận (FR-004b, `OQ-004`), vì đó là điều kiện để phát lại trận đúng tất định (`INV-001`, `INV-002`) — hai thứ khác mục đích, không mâu thuẫn.
- Chặn sao chép đề — NON-GOAL-007.

---

## 9. Open Questions

> Giữ nguyên theo đúng yêu cầu — spec này không tự trả lời bất kỳ mục nào dưới đây, trừ mục đã đóng bằng phiên `/speckit-clarify` **2026-07-30** (xem §Clarifications).

**~~OQ-001~~ — ĐÃ ĐÓNG 2026-07-30.** "Bộ đề" là entity riêng, N-N với câu hỏi — xem §Clarifications và FR-014, FR-015.

**~~OQ-002~~ — ĐÃ ĐÓNG 2026-07-30.** `displayId` bắt buộc duy nhất toàn hệ thống, mang tiền tố chỉ loại câu; có định danh nội bộ ổn định riêng biệt — xem §Clarifications, FR-001b.

**OQ-003 — NEEDS CLARIFICATION: giới hạn số lượng cho các trường mảng**
Không nguồn nào quy định số lượng tối thiểu/tối đa cho `clues[]` (gợi ý) hay cho `options[]` của kiểu `choice`/`ordering`.

**~~OQ-004~~ — ĐÃ ĐÓNG 2026-07-30.** Sửa tự do, không khoá; mỗi lần hiển thị trong một trận ghi log nội dung/phiên bản đã dùng — xem §Clarifications và FR-004b.

**~~OQ-005~~ — ĐÃ ĐÓNG 2026-07-30.** Xoá mềm, ở mọi trạng thái; dữ liệu lịch sử (`everPublic`, log hiển thị) không đổi — xem §Clarifications, FR-013c, FR-013d. **Còn hở**: chưa xác nhận có thao tác **phục hồi** câu đã xoá mềm hay không, và tương tác chính xác với tư cách thành viên bộ đề/bộ VCNV — xem ghi chú ở §7 Edge Cases.

**~~OQ-006~~ — ĐÃ ĐÓNG 2026-07-30.** Server-timestamp-first-wins cho tranh chấp Duyệt/Trả về đồng thời; cộng thêm yêu cầu chung — mọi lần sửa một câu ghi log diff + người thực hiện — xem §Clarifications, FR-002b, FR-013b.

**OQ-007 — MISSING (partial failure): tải media thất bại giữa chừng lúc lưu câu**
Không nguồn nào quy định hành vi khi việc tải media lên thất bại giữa chừng trong lúc setter lưu một câu hỏi — có cho lưu câu ở dạng thiếu media không, và có cơ chế thử lại nào không.

---

## 10. Traceability Matrix

| User story | PRD requirement | Game rules | Functional requirements | Acceptance scenarios |
|---|---|---|---|---|
| **US-001** — Soạn câu hỏi, thời lượng độc lập theo câu | `PRD-REQ-007`, `PRD-REQ-008` | `GR-031`, `GR-018`, `GR-013` | FR-001, FR-001b → FR-004, FR-002b, FR-004b | AC-001, AC-001c → AC-004, AC-002b, AC-004b |
| **US-002** — Ba kiểu nhập đáp án, đáp án luôn là chuỗi | `PRD-REQ-009` | `GR-027` | FR-005, FR-006 | AC-005, AC-006 |
| **US-003** — Câu hỏi thực hành | `PRD-REQ-010` | `GR-019` | FR-007 → FR-009 | AC-007 → AC-010 |
| **US-004** — Duyệt đề `DRAFT` → `ACTIVE` | `PRD-REQ-011` | `GR-031` | FR-010 → FR-013, FR-013b → FR-013d | AC-011 → AC-014, AC-014b → AC-014d |
| **US-005** — Bộ đề, visibility dẫn xuất, `everPublic` | `PRD-REQ-012`, `PRD-REQ-013` | `GR-031` | FR-014, FR-015, FR-015b → FR-019 | AC-015 → AC-021 |
| **US-006** — Tìm kiếm kho đề | `PRD-REQ-014` | `GR-031` | FR-020 | AC-022 |
| **US-007** — Trim khoảng trắng | `PRD-REQ-015` | `GR-027`, `GR-006` | FR-021 | AC-023 |
| **US-008** — Bộ VCNV nguyên vẹn | `PRD-REQ-092` | `GR-031` | FR-022 → FR-026 | AC-024 → AC-028 |

### Phủ bảng quyết định của game rule (phần thuộc EPIC-002)

| Rule · ca | Nội dung ca | Acceptance scenario |
|---|---|---|
| `GR-031` C3b | Đủ 4 hàng ngang + 1 CNV nhưng không cùng bộ ⇒ vòng không mở được | AC-028 |
| `GR-031` C3c | Một hàng ngang của bộ X bị dùng làm Câu hỏi phụ ⇒ bộ X vỡ dù 5 thành phần kia còn nguyên | AC-027 |
| `GR-027` §Điều kiện | Đáp án và bài làm luôn là chuỗi ở cả ba kiểu nhập | AC-006 |
| `GR-019` §Ngoại lệ và cảnh báo | `isPractical` ngoài kho Về đích ⇒ chặn ở kho đề, không phải lỗi lúc chạy | AC-008 |
| `GR-019` §Bảo mật | `acceptanceCriteria` cùng mức với đáp án — chỉ phiên giữ `PERM-045` | AC-010 |

> **Ghi chú phủ có chủ đích.** Các ca còn lại của `GR-031` (C1, C2, C4…C9, liên quan rút đề/kiểm kho **lúc chạy trận**) và toàn bộ bảng quyết định của `GR-007`…`GR-011` (lượt chọn, chấm điểm hàng ngang, bấm chuông giải Chướng ngại vật) thuộc EPIC-006 *(xem §8)*, không phải EPIC-002. Feature này chỉ phủ vế **dữ liệu và cấu trúc kho đề** mà các rule đó đọc, và đó là toàn bộ giao diện giữa EPIC-002 với những rule này.

---

## 11. Assumptions

Các giá trị mặc định dưới đây được chọn cho **chi tiết không quan trọng**; mọi thứ quan trọng mà nguồn không nói đều nằm ở §9.

- **Định dạng và giới hạn dung lượng của media** không được chốt trong spec này — `CLAUDE.md` §Quy ước khác khai đây là **env config theo từng loại media**, không hard-code; spec chỉ yêu cầu *media lưu và phát lại được* (FR-001).
- **Thuật toán sinh định danh nội bộ ổn định** (FR-001b, Key Entities) — kể cả việc dùng UUIDv7 mà chủ dự án nêu lúc xác nhận `OQ-002` — là lựa chọn kỹ thuật thuộc `plan.md`, không phải nội dung của spec; spec chỉ chốt **tính chất quan sát được**: `displayId` duy nhất + mang tiền tố loại câu, và tồn tại một định danh nội bộ tách biệt, ổn định qua sửa `displayId`.
- **Cấu trúc chính xác của "ghi chú duyệt"** ở FR-011 (độ dài, có bắt buộc hay không khi trả về) không được nguồn nào quy định; spec chỉ yêu cầu *có ghi chú tới được setter*.
- **Nhật ký ở FR-017** được hiểu là ghi vào cùng nhật ký thao tác chung mà EPIC-011 sở hữu; spec này không đặc tả cấu trúc của nhật ký đó.
- **Ràng buộc kỹ thuật toàn repo** — DRY, zero-trust, chỉ tiếng Việt, trim ở cả hai đầu, string UI tách file constants — áp cho feature này theo `CLAUDE.md`; spec **tham chiếu, không sao chép**.
