# Feature Specification: EPIC-002 — Kho đề và bộ đề

**Feature Directory**: `specs/002-kho-de-bo-de`

**Created**: 2026-07-30

**Status**: Draft

**Input**: User description: `/speckit.specify` — tạo feature specification cho **EPIC-002 — Kho đề và bộ đề**, chỉ dùng product requirement / user journey / game rule / state thuộc EPIC-002, không đưa công nghệ triển khai vào spec.

---

## 0. Nguồn đã đọc và ghi chú về nguồn

| Nguồn yêu cầu đọc | Trạng thái |
|---|---|
| `docs/PRD.md` | ✅ đã đọc — §7 Actors (`ACTOR-002` setter, `ACTOR-001` admin), §10 `JOURNEY-001`, §11 EPIC-002, §12 EPIC-002 (`PRD-REQ-007`…`015`) cộng phần EPIC-002 của `PRD-REQ-047` và `PRD-REQ-092`, §14 FS-05…FS-07, §16, §19, §20 ma trận truy nguyên |
| `docs/glossary.md` | ✅ đã đọc — `TERM-044`…`TERM-048`, `TERM-049`, `TERM-050`, `TERM-052`, `TERM-053`, `TERM-058` |
| `docs/game-rules.md` | ✅ đã đọc — `GR-006`, `GR-019`, `GR-027`, `GR-031` đầy đủ; `GR-007`…`GR-011` đọc để tách phần **thuộc kho đề** (cấu trúc Bộ VCNV) khỏi phần **thuộc game engine** (lượt chọn, chấm điểm khi chạy) |
| `docs/game-state-machine.md` | ✅ đã đọc §9 (bảy thang trạng thái) để xác nhận: **không** có `STATE-*`/`EVENT-*`/`T-*` nào thuộc trực tiếp EPIC-002 — xem §1 |
| `docs/traceability.md` | ✅ đã đọc — bảng đối chiếu giá trị luật và biến thể bị loại |

Ngoài ra đã đọc `docs/decisions.md` (`QĐ-041`…`QĐ-044`, `QĐ-063`, `QĐ-064`, `QĐ-066`, `QĐ-071`, `QĐ-082`, `QĐ-084`) và `docs/product-discovery.md` §5 E-2 vì `docs/PRD.md` khai chúng là nguồn của các `PRD-REQ-*` thuộc EPIC-002.

---

## 1. Phạm vi feature

**Goal (nguyên văn EPIC-002)**: *"Câu hỏi soạn một lần, dùng lại nhiều lần, không rò và không lặp."* — `docs/PRD.md` §11 EPIC-002.

**Product requirement trong phạm vi**:

| Requirement | Tiêu đề | Priority (PRD) |
|---|---|---|
| `PRD-REQ-007` | Soạn câu hỏi với metadata và media | P1 |
| `PRD-REQ-008` | Thời lượng suy nghĩ là metadata của TỪNG CÂU | P1 |
| `PRD-REQ-009` | Một kiểu nhập đáp án, đáp án luôn là chuỗi | P1 |
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

### US-002 — Một kiểu nhập đáp án; format yêu cầu nằm trong đề (Priority: P1)

- **Actor**: `ACTOR-002` setter
- **Intent**: Soạn được câu hỏi lựa chọn và câu hỏi sắp xếp mà không phải khai một danh sách phương án nào — nêu yêu cầu format ngay trong phần đề, và để thí sinh gõ theo.
- **User value**: Máy không chấm, nên một thứ tự kéo thả và một chuỗi gõ tay đều đi vào cùng một chỗ — mắt admin. Một ô nhập duy nhất bỏ được cả một trục dữ liệu mà không mất gì luật đòi.
- **Why this priority**: `PRD-REQ-009` là P1 — không có ô nhập thì thí sinh ở mode nhập liệu không trả lời được, và toàn bộ vòng Tăng tốc cùng VCNV không chạy được vì hai vòng đó luôn gõ máy.
- **Independent Test**: Soạn ba câu — một câu thường, một câu lựa chọn, một câu sắp xếp — qua cùng một biểu mẫu; xem preview bài làm mẫu của cả ba đều hiện dưới dạng chuỗi cạnh đáp án, tô khác biệt theo cùng một cơ chế.
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

**AC-004b — Side effect: sửa câu đã hiển thị không khoá, mỗi cú Save sinh một phiên bản bất biến**
- **US**: US-001 · **FR**: FR-004b · **GR**: `GR-031`
- **Given** một câu `ACTIVE` đang ở phiên bản V1 với đáp án `"Huế"`; Trận A đã hiển thị câu này và tham chiếu tới V1
- **When** setter sửa đáp án thành `"Đà Nẵng"` rồi bấm Save
- **Then** câu lưu thành công, **không** bị khoá vì lịch sử hiển thị · một phiên bản **V2** mới được tạo, mang nội dung `"Đà Nẵng"`, người sửa và mốc thời gian · **V1 không đổi** và vẫn xem lại được nguyên nội dung `"Huế"` · tham chiếu của Trận A vẫn trỏ tới **V1**, nên phát lại Trận A vẫn ra `"Huế"` · một trận **bắt đầu sau** thao tác này hiển thị câu theo phiên bản hiện hành **V2**

### US-002 — Một kiểu nhập đáp án, đáp án luôn là chuỗi

**AC-005 — Happy path: câu lựa chọn và câu sắp xếp soạn như câu thường**
- **US**: US-002 · **FR**: FR-005 · **GR**: `GR-027`
- **Given** setter soạn ba câu mới — một câu hỏi thường, một **câu lựa chọn** *(đề nêu bốn phương án A/B/C/D và yêu cầu chọn một)*, một **câu sắp xếp** *(đề nêu bốn mục và yêu cầu viết thứ tự, cách nhau bởi dấu phẩy)*
- **When** setter lưu cả ba
- **Then** cả ba lưu thành công qua **cùng một** biểu mẫu · **0** trường danh sách phương án nào phải khai · **0** lựa chọn *kiểu nhập* nào hiện ra trong biểu mẫu · yêu cầu format của hai câu sau nằm trong **phần đề**, không nằm ở một trường riêng

**AC-006 — No-change/invariant: đáp án luôn là chuỗi, một đường so khớp duy nhất**
- **US**: US-002 · **FR**: FR-006 · **GR**: `GR-027`
- **Given** một **câu sắp xếp** mà đề yêu cầu *"viết thứ tự, cách nhau bởi dấu phẩy"*, đáp án đúng là thứ tự B, D, A, C
- **When** setter lưu đáp án chuẩn, rồi trong trận một thí sinh gõ `"B, D, A, C"` và một thí sinh khác gõ `"B,D,A,C"`
- **Then** đáp án lưu dưới dạng chuỗi `"B, D, A, C"`, **cùng kiểu dữ liệu** với đáp án của một câu thường · **0** trường lưu *thứ tự* hay *lựa chọn* nào tồn tại · cả hai bài làm đi qua **cùng một** phép chuẩn hoá và tô khác biệt của `GR-027` · hệ thống **không** tự sửa format và **không** phát thông điệp riêng nào cho *sai format* · **admin phán quyết** cả hai

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
- **Given** một câu `ACTIVE`, `everPublic = true`, có hai phiên bản đã lưu, và một trận trước đó tham chiếu tới phiên bản V1
- **When** admin xoá mềm câu này
- **Then** `everPublic` của câu **không đổi** · cả hai phiên bản **không đổi** và vẫn xem lại được · tham chiếu của trận cũ vẫn trỏ V1, trận đó vẫn phát lại đúng nội dung đã hiển thị · câu chỉ **ẩn** khỏi tìm kiếm và danh sách chọn contest

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
**AC-029 — Câu Chướng ngại vật: đúng một gợi ý, không phương án**
- **US**: US-008 · **FR**: FR-027 · **GR**: `GR-009`, `GR-011`
- **Given** người ra đề đang soạn một câu Chướng ngại vật,
- **When** lần lượt thử lưu với **0** gợi ý, với **2** gợi ý, rồi với **đúng 1** gợi ý,
- **Then** hai lần đầu bị **từ chối** kèm lý do nêu rõ ràng buộc *(0 gợi ý · từ 2 gợi ý trở lên)* · bản có **đúng 1** gợi ý lưu được · **0** dữ liệu nào đổi ở hai lần bị từ chối

**AC-030 — Xoá mềm là một chiều và gỡ tư cách thành viên**
- **US**: US-004, US-008 · **FR**: FR-028, FR-029 · **GR**: `GR-031` C3b, C3c · `INV-022`
- **Given** một câu `ACTIVE` đang là thành phần của một bộ Chướng ngại vật nguyên vẹn, và câu đó đã được hiển thị trong một trận đã chạy xong,
- **When** admin xoá mềm câu đó,
- **Then** hệ thống **cảnh báo trước** và nêu rõ bộ nào sẽ vỡ · sau khi xác nhận, câu biến khỏi tìm kiếm và khỏi mọi danh sách chọn · bộ Chướng ngại vật đó **vỡ** · nhật ký sự kiện của trận đã chạy **không đổi** và vẫn tra ra nội dung câu đã lên sóng · **0** đường phục hồi câu tồn tại, kể cả khi gửi thẳng lệnh tới server

**AC-031 — Bộ vỡ vì xoá mềm thì phép kiểm kho đề chặn vòng VCNV**
- **US**: US-008 · **FR**: FR-029 · **GR**: `GR-031` C3, C3b
- **Given** một contest mà bộ Chướng ngại vật duy nhất đã vỡ vì một thành phần bị xoá mềm,
- **When** admin chạy phép kiểm kho đề rồi thử mở vòng VCNV,
- **Then** phép kiểm báo **thiếu bộ nguyên vẹn**, không báo thiếu số câu · vòng VCNV **không mở được** · các vòng khác **vẫn mở bình thường** · **0** điểm nào và **0** trạng thái trận nào đổi

**AC-032 — Lưu câu thiếu media: cảnh báo, không chặn, không thử lại**
- **US**: US-001 · **FR**: FR-030 · **GR**: `PRD-REQ-019`
- **Given** người ra đề đang lưu một câu có phần chữ hợp lệ và một tệp media đính kèm,
- **When** việc tải tệp media lên thất bại giữa chừng,
- **Then** câu **được lưu** với phần chữ nguyên vẹn, ở dạng **thiếu media** · một **cảnh báo** hiện ra nêu rõ tệp nào chưa lên được · câu ở `DRAFT` · **0** lần thử lại tự động nào xảy ra · **0** phần chữ nào bị mất


## 4. Functional Requirements *(mandatory)*

> Mỗi FR là một phát biểu kiểm thử được, tham chiếu US-NNN, PRD-REQ-NNN, GR-NNN và AC-NNN. Không FR nào nêu framework, cơ sở dữ liệu, đường API, lớp, hàm hay thuật toán.

### Nhóm A — Soạn câu hỏi và metadata

- **FR-001**: Hệ thống MUST cho phép người ra đề tạo và lưu câu hỏi với các trường: mã hiển thị, lĩnh vực, số chữ đáp án, giải thích, ghi chú, thời lượng suy nghĩ *(tuỳ chọn)*, mức điểm *(tuỳ chọn)*, danh sách gợi ý *(tuỳ chọn)*, và media. · US-001 · `PRD-REQ-007` · `GR-031` · AC-001, AC-002
- **FR-001b**: Mã hiển thị (`displayId`) của một câu hỏi MUST **duy nhất trên toàn hệ thống**; hệ thống MUST từ chối lưu một câu mới hoặc một bản sửa mang mã hiển thị đã tồn tại ở một câu khác. Mã hiển thị MUST mang một tiền tố ngắn chỉ loại câu. Câu MUST mang thêm một **định danh nội bộ ổn định**, tách khỏi `displayId` và không đổi khi `displayId` bị sửa; định danh nội bộ MUST là thứ đi trong gói xuất/nhập. · US-001 · **`PRD-REQ-115`**, `PRD-REQ-007` · `QĐ-107` · AC-001c
- **FR-002**: Câu hỏi đã lưu và mở lại MUST giữ nguyên toàn bộ giá trị của mọi trường đã nhập, kể cả khi các trường tuỳ chọn để trống. · US-001 · `PRD-REQ-007` · — · AC-002
- **FR-002b**: Mỗi lần một câu hỏi bị sửa, hệ thống MUST ghi một dòng nhật ký nêu **nội dung đã đổi** (dạng khác biệt giữa bản cũ và bản mới, cho từng trường) và **ai đã sửa**. · US-001 · `PRD-REQ-007` · — · AC-002b
- **FR-003**: Thời lượng suy nghĩ của một câu hỏi MUST là thuộc tính lưu riêng cho câu đó; hệ thống MUST NOT suy thời lượng từ mức điểm của câu. Hệ thống chọn câu theo mức điểm, MUST lấy thời lượng theo từng câu. · US-001 · `PRD-REQ-008` · `GR-018`, `GR-013` · AC-003
- **FR-004**: Khi một câu không khai thời lượng riêng, hệ thống MUST dùng giá trị mặc định của preset đang áp cho contest; giá trị khai riêng của một câu MUST luôn thắng giá trị mặc định của preset. · US-001 · `PRD-REQ-008` · `GR-018` · AC-004
- **FR-004b**: Một câu `ACTIVE` MUST sửa được tự do kể cả sau khi đã từng hiển thị trong một trận; hệ thống MUST NOT khoá sửa dựa trên lịch sử hiển thị. **Mỗi cú bấm Save MUST tạo một phiên bản mới** của câu, mang nội dung câu tại thời điểm đó, người sửa và mốc thời gian; phiên bản đã tạo MUST là bất biến và MUST xem lại được nguyên nội dung. Một trận đã chạy MUST tham chiếu tới **đúng phiên bản đã hiển thị** cho thí sinh, nên một bản sửa sau đó MUST NOT làm đổi thứ mà trận cũ phát lại. *(Việc trận **ghi** tham chiếu đó vào nhật ký sự kiện thuộc EPIC-006 — xem §8; feature này chỉ bảo đảm phiên bản **tồn tại, bất biến và tham chiếu được**.)* Màn **so sánh diff** giữa hai phiên bản và thao tác **quay về bản cũ** MUST NOT thuộc phạm vi v1. · US-001 · **`PRD-REQ-115`** · `GR-031`, `INV-022` · AC-004b

### Nhóm B — Một kiểu nhập đáp án, đáp án luôn là chuỗi

- **FR-005**: Mọi câu hỏi có ô nhập MUST dùng **một** ô nhập chữ duy nhất. Hệ thống MUST NOT có trường chọn kiểu nhập và MUST NOT có trường danh sách phương án. **Câu hỏi lựa chọn** và **câu hỏi sắp xếp** của luật gốc MUST soạn được như câu hỏi thường, với **yêu cầu format nằm trong phần đề**. · US-002 · `PRD-REQ-009` · `QĐ-162` · `GR-027` · AC-005
- **FR-006**: Đáp án và bài làm MUST được lưu dưới dạng **chuỗi**, và MUST đi qua **đúng một** đường chuẩn hoá và so khớp của `GR-027`. Hệ thống MUST NOT có nhánh xử lý hay lưu trữ riêng theo loại câu, MUST NOT tự sửa format bài làm, và MUST NOT phát thông điệp riêng cho ca *sai format* — bài làm lệch format là ca **admin phán quyết**, cùng hạng với sai chính tả. · US-002 · `PRD-REQ-009` · `QĐ-162` · `GR-027` · AC-006

### Nhóm C — Câu hỏi thực hành

- **FR-007**: Câu hỏi khai `isPractical = true` MUST chỉ hợp lệ khi thuộc kho Về đích; hệ thống MUST chặn việc lưu một câu thực hành thuộc kho khác ngay tại thời điểm soạn, MUST NOT để lỗi này trôi tới lúc chạy trận. · US-003 · `PRD-REQ-010` · `GR-019` · AC-007, AC-008
- **FR-008**: Câu hỏi thực hành MUST lưu đủ năm trường riêng — cờ thực hành, thời lượng thực hành của người thi chính, thời lượng thực hành của người cướp quyền, ghi chú dụng cụ, và tiêu chí đạt — và hai bộ thời lượng MUST lưu tách biệt, không ghi đè lẫn nhau. · US-003 · `PRD-REQ-010` · `GR-019` · AC-009
- **FR-009**: Trường tiêu chí đạt của câu thực hành MUST chịu cùng mức bảo mật với đáp án của câu thường — MUST NOT rời server tới vai không giữ quyền xem đáp án. · US-003 · `PRD-REQ-010` · `GR-019` · AC-010

### Nhóm D — Duyệt đề

- **FR-010**: Một câu hỏi MUST mang đúng một trong hai trạng thái vòng đời — **`DRAFT`** hoặc **`ACTIVE`** — và MUST khởi tạo ở `DRAFT` ngay khi setter lưu lần đầu. Câu ở `DRAFT` MUST NOT xuất hiện trong danh sách chọn câu của bất kỳ contest nào, và MUST NOT có đường ép một câu `DRAFT` vào danh sách đó từ màn chọn câu. · US-004 · `PRD-REQ-011` · `GR-031` · AC-011
- **FR-011**: Hệ thống MUST hiển thị hàng chờ duyệt gồm mọi câu đang `DRAFT` chưa xoá mềm trên bảng điều khiển của admin, và admin MUST có đúng hai thao tác trên một mục của hàng chờ:
  - **Duyệt** ⇒ câu chuyển `DRAFT` → `ACTIVE`, biến khỏi hàng chờ, và từ thời điểm đó chọn được cho danh sách câu của contest. Nội dung câu **không đổi** vì thao tác này.
  - **Trả về kèm ghi chú lý do** ⇒ câu **giữ nguyên** `DRAFT`, ghi chú được lưu và hiển thị lại cho setter, mọi trường khác của câu **không đổi**, và câu **vẫn** ở hàng chờ.

  Hàng chờ chỉ chứa câu `DRAFT`, nên cả hai thao tác đều chỉ tác động lên câu ở trạng thái đó; nguồn không quy định đường đưa một câu `ACTIVE` ngược về `DRAFT`, và spec này không tự tạo ra đường đó. · US-004 · `PRD-REQ-011` · — · AC-012, AC-013
- **FR-012**: Thao tác duyệt MUST chỉ thực hiện được bởi vai admin; hệ thống MUST NOT có vai riêng cho việc duyệt câu hỏi. · US-004 · `PRD-REQ-011` · — · AC-012
- **FR-013**: Bấm Duyệt trên một câu đã `ACTIVE` MUST là **không tác dụng** — trạng thái câu giữ `ACTIVE`, nội dung không đổi, và MUST NOT sinh thêm bản ghi duyệt mang ý nghĩa nghiệp vụ khác. · US-004 · `PRD-REQ-011` · — · AC-014
- **FR-013b**: Khi hai phiên admin cùng gửi Duyệt và Trả về trên cùng một câu đang `DRAFT` trong một cửa sổ rất ngắn, server MUST phân xử theo **server timestamp**: quyết định tới **trước** có hiệu lực và quyết định tới **sau** MUST bị từ chối, MUST NOT lật kết quả đã phân giải. Quyết định bị từ chối MUST vẫn được ghi vào nhật ký kèm trạng thái bị từ chối, và MUST NOT đổi trạng thái hay nội dung của câu. · US-004 · `PRD-REQ-011` · — · AC-014b
- **FR-013c**: Hệ thống MUST hỗ trợ **xoá mềm** một câu hỏi ở cả hai trạng thái `DRAFT` và `ACTIVE`. Câu đã xoá mềm MUST biến khỏi kết quả tìm kiếm kho đề, khỏi danh sách chọn câu của **mọi** contest, và khỏi hàng chờ duyệt nếu trước đó nó đang `DRAFT`. · US-004 · **`PRD-REQ-115`**, `PRD-REQ-011` · — · AC-014c
- **FR-013d**: Xoá mềm MUST chỉ đặt cờ ẩn của câu; nó MUST NOT xoá hay thay đổi bất kỳ dữ liệu nào đã ghi cho câu đó — dấu vết "đã từng public", cờ đã-dùng, mọi phiên bản đã lưu, và mọi tham chiếu từ nhật ký của các trận cũ đều giữ nguyên, và một trận cũ vẫn phát lại được đúng nội dung đã hiển thị. · US-004 · **`PRD-REQ-115`**, `PRD-REQ-011` · — · AC-014d

### Nhóm E — Bộ đề, cờ hiển thị dẫn xuất, hàng rào `everPublic`

- **FR-014**: **Bộ đề** MUST là một entity riêng, độc lập với kho đề toàn hệ thống, mang quan hệ thành viên **nhiều-nhiều** với câu hỏi — một câu thuộc được nhiều bộ đề, một bộ đề chứa nhiều câu; mỗi bộ đề MUST tự mang thuộc tính `public`/`private` của chính nó. · US-005 · `PRD-REQ-012` · `GR-031` · AC-015, AC-016
- **FR-015**: Cờ hiển thị (`visibility`) của một câu hỏi MUST là giá trị dẫn xuất, đúng bằng `PUBLIC` khi và chỉ khi câu đang thuộc ít nhất một bộ đề `public` (theo FR-014), ngược lại `PRIVATE`; giá trị này MUST cập nhật ngay khi quan hệ thành viên với bộ đề đổi. · US-005 · `PRD-REQ-012` · `GR-031` · AC-015, AC-016
- **FR-015b**: Hệ thống MUST NOT cung cấp bất kỳ đường nào — giao diện hay server — để đặt trực tiếp cờ hiển thị của một câu hỏi; vai người ra đề chỉ đổi cờ này bằng cách thêm hoặc gỡ câu khỏi một bộ đề. · US-005 · `PRD-REQ-012` · `GR-031` · AC-017
- **FR-016**: Ngay tại lần đầu tiên một câu hỏi trở thành thành viên của một bộ đề đang `public`, hệ thống MUST đặt dấu vết **"đã từng public"** cho câu đó. Dấu vết này MUST một chiều và vĩnh viễn: gỡ câu khỏi bộ đề public, đặt bộ đề đó về `private`, xoá bộ đề, xoá mềm câu, hay xuất/nhập câu sang bản cài khác đều MUST NOT đưa dấu vết về trạng thái ban đầu, và hệ thống MUST NOT có thao tác nào — giao diện hay server — làm việc đó. Lặp lại chu trình thêm–gỡ nhiều lần MUST NOT đổi kết quả: dấu vết chỉ bật một lần rồi giữ nguyên. · US-005 · `PRD-REQ-013` · — · AC-018
- **FR-017**: Khi một câu được thêm vào danh sách câu đã gán của một trận, hệ thống MUST đánh giá theo thứ tự: **(1)** trận này là `official` hay `practice` — `practice` ⇒ thêm bình thường theo FR-018; **(2)** câu có mang dấu vết "đã từng public" không — không ⇒ thêm bình thường; **(3)** cả hai đúng ⇒ **chặn mặc định** và cảnh báo, **(4)** admin đi qua **hai bước xác nhận riêng biệt** thì câu vẫn thêm được, **(5)** mọi diễn biến — lần chặn, các bước xác nhận, và kết quả cuối *(thêm thành công hay bỏ)* — MUST được ghi vào nhật ký. Không hoàn tất đủ hai bước xác nhận ⇒ câu **không** vào danh sách và dữ liệu **không đổi**. Hàng rào này MUST áp **y hệt nhau** ở cả ba cửa vào — bước kiểm trước trận, sửa danh sách tại `LOBBY`, và nhập gói contest — MUST NOT có cửa nào lách qua được. · US-005 · `PRD-REQ-013` · — · AC-019, AC-020
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
- **FR-027**: Câu Chướng ngại vật MUST mang **đúng một** gợi ý — `clues[]` có **đúng 1** phần tử. Hệ thống MUST từ chối một câu Chướng ngại vật có **0** hoặc từ **2** gợi ý trở lên, và MUST NOT đổi dữ liệu nào khi từ chối. *(US-008 · `QĐ-153` · `GR-009`, `GR-011` · AC-029)*
- **FR-028**: Xoá mềm một câu hỏi MUST là thao tác **một chiều**: hệ thống MUST NOT có đường phục hồi một câu đã xoá mềm, trên bất kỳ bề mặt nào và kể cả khi gửi thẳng lệnh tới server. Dữ liệu lịch sử của các trận đã chạy MUST giữ nguyên và vẫn tra ra được nội dung câu đã lên sóng. *(US-004 · `QĐ-154` · `INV-022` · AC-030)*
- **FR-029**: Xoá mềm một câu hỏi MUST **gỡ tư cách thành viên** của câu đó khỏi mọi bộ đề và mọi bộ Chướng ngại vật đang chứa nó. Một bộ Chướng ngại vật mất thành phần vì xoá mềm MUST được coi là **vỡ** và MUST NOT dùng được cho vòng VCNV. Trước khi xoá mềm một câu đang là thành phần của một bộ, hệ thống MUST cảnh báo và MUST nêu rõ **bộ nào** sẽ vỡ. *(US-004, US-008 · `QĐ-154` · `GR-031` C3b, C3c · AC-030, AC-031)*
- **FR-030**: Khi việc tải media lên **thất bại giữa chừng** lúc lưu một câu, hệ thống MUST **vẫn lưu câu** ở dạng thiếu media và MUST hiện **cảnh báo** nêu rõ tệp nào chưa lên được. Hệ thống MUST NOT tự thử lại và MUST NOT chặn cú lưu. Câu đó MUST ở `DRAFT` và MUST đi qua cửa duyệt như mọi câu khác. *(US-001 · `QĐ-155` · `PRD-REQ-019` · AC-032)*

## 5. Key Entities

- **Câu hỏi** (`Question`, `TERM-044`): đơn vị đề thi. Trường cốt lõi — mã hiển thị *(`displayId`, duy nhất toàn hệ thống, mang tiền tố chỉ loại câu)*, lĩnh vực, số chữ đáp án, giải thích, ghi chú, thời lượng suy nghĩ *(tuỳ chọn)*, mức điểm *(tuỳ chọn)*, gợi ý *(tuỳ chọn)*, media, đáp án chuẩn *(luôn là chuỗi)*, trạng thái `DRAFT`/`ACTIVE`, `visibility` *(dẫn xuất)*, `everPublic` *(một chiều)*. Năm trường thực hành *(`isPractical`, `practiceSeconds`, `stealPracticeSeconds`, `equipmentNote`, `acceptanceCriteria`)* chỉ hợp lệ khi câu thuộc kho Về đích. Ngoài `displayId`, câu còn mang một **định danh nội bộ ổn định**, riêng biệt với `displayId`, dùng để tham chiếu xuyên hệ thống — không đổi kể cả khi `displayId` bị sửa. Câu còn mang một cờ **xoá mềm**, đặt được ở cả `DRAFT` lẫn `ACTIVE`; câu đã xoá mềm ẩn khỏi tìm kiếm và khỏi danh sách chọn cho contest nhưng không mất dữ liệu *(FR-013c, FR-013d)*.
- **Phiên bản câu hỏi** *(`PRD-REQ-115`)*: mỗi cú bấm **Save** sinh một bản ghi **bất biến** mang nội dung câu tại thời điểm đó, người sửa và mốc thời gian. Phạm vi v1 gồm **lưu và xem lại nội dung từng bản**; trận tham chiếu tới **bản đã hiển thị** *(tham chiếu do EPIC-006 ghi vào nhật ký sự kiện)*. So sánh diff và quay về bản cũ **ngoài phạm vi v1**.
- **Bộ đề**: entity riêng, độc lập với kho đề toàn hệ thống. Mang quan hệ **nhiều-nhiều** với câu hỏi — một câu thuộc được nhiều bộ đề, một bộ đề chứa nhiều câu. Mỗi bộ đề tự mang thuộc tính `public`/`private` của chính nó; `visibility` của một câu là **OR** trên tập bộ đề nó thuộc (FR-014, FR-015).
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
- Câu `ACTIVE` bị sửa sau khi đã hiển thị ở một trận trước: cú Save sinh một phiên bản mới, còn phiên bản mà trận cũ tham chiếu **đứng yên** — phát lại trận cũ vẫn ra đúng nội dung thí sinh đã thấy *(AC-004b)*.
- Câu bị xoá mềm trong khi đang là thành phần của một bộ đề hoặc một bộ VCNV: câu biến khỏi tìm kiếm và khỏi danh sách chọn cho contest *(AC-014c)*, nên một bộ VCNV có thành phần bị xoá mềm không còn chọn được cho contest mới. Việc quan hệ thành viên có bị gỡ theo hay không, và việc có tồn tại thao tác **phục hồi** câu đã xoá mềm hay không, chưa được nguồn nào quy định *(xem `OQ-005`)*.

**Hỏng một phần**
- Tải media thất bại giữa chừng khi lưu câu — hành vi cụ thể (giữ câu ở trạng thái gì, có cho lưu câu không kèm media hay không) chưa được nguồn nào quy định *(xem `OQ-007`)*.

**Hành vi nguồn không quy định** — ba mục còn mở ở §9: `OQ-003` (giới hạn số lượng trường mảng), `OQ-005` (phục hồi câu đã xoá mềm), `OQ-007` (hỏng media giữa chừng); spec này không tự điền các hành vi đó.

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
- Màn **so sánh diff** giữa hai phiên bản của một câu, và thao tác **quay về bản cũ** — ngoài phạm vi v1. Thuộc phạm vi v1: mỗi cú Save sinh một phiên bản bất biến và xem lại được nội dung từng bản *(FR-004b)*.
- Chặn sao chép đề — NON-GOAL-007.

---

## 9. Open Questions

**Không có.** Câu hỏi về biên của danh sách phương án **tự tan**: trường đó không còn tồn tại (`QĐ-162`).

## 10. Traceability Matrix

| User story | PRD requirement | Game rules | Functional requirements | Acceptance scenarios |
|---|---|---|---|---|
| **US-001** — Soạn câu hỏi, thời lượng độc lập theo câu | `PRD-REQ-007`, `PRD-REQ-008`, `PRD-REQ-115` | `GR-031`, `GR-018`, `GR-013`, `INV-022` | FR-001, FR-001b → FR-004, FR-002b, FR-004b | AC-001, AC-001c → AC-004, AC-002b, AC-004b |
| **US-002** — Một kiểu nhập đáp án, đáp án luôn là chuỗi | `PRD-REQ-009` | `GR-027` | FR-005, FR-006 | AC-005, AC-006 |
| **US-003** — Câu hỏi thực hành | `PRD-REQ-010` | `GR-019` | FR-007 → FR-009 | AC-007 → AC-010 |
| **US-004** — Duyệt đề `DRAFT` → `ACTIVE` | `PRD-REQ-011`, `PRD-REQ-115` | `GR-031` | FR-010 → FR-013, FR-013b → FR-013d | AC-011 → AC-014, AC-014b → AC-014d |
| **US-005** — Bộ đề, visibility dẫn xuất, `everPublic` | `PRD-REQ-012`, `PRD-REQ-013` | `GR-031` | FR-014, FR-015, FR-015b → FR-019 | AC-015 → AC-021 |
| **US-006** — Tìm kiếm kho đề | `PRD-REQ-014` | `GR-031` | FR-020 | AC-022 |
| **US-007** — Trim khoảng trắng | `PRD-REQ-015` | `GR-027`, `GR-006` | FR-021 | AC-023 |
| **US-008** — Bộ VCNV nguyên vẹn | `PRD-REQ-092` | `GR-031` | FR-022 → FR-026 | AC-024 → AC-028 |

### Phủ bảng quyết định của game rule (phần thuộc EPIC-002)

| Rule · ca | Nội dung ca | Acceptance scenario |
|---|---|---|
| `GR-031` C3b | Đủ 4 hàng ngang + 1 CNV nhưng không cùng bộ ⇒ vòng không mở được | AC-028 |
| `GR-031` C3c | Một hàng ngang của bộ X bị dùng làm Câu hỏi phụ ⇒ bộ X vỡ dù 5 thành phần kia còn nguyên | AC-027 |
| `GR-027` §Điều kiện | Đáp án và bài làm luôn là chuỗi; một đường chuẩn hoá duy nhất | AC-006 |
| `GR-019` §Ngoại lệ và cảnh báo | `isPractical` ngoài kho Về đích ⇒ chặn ở kho đề, không phải lỗi lúc chạy | AC-008 |
| `GR-019` §Bảo mật | `acceptanceCriteria` cùng mức với đáp án — chỉ phiên giữ `PERM-045` | AC-010 |

> **Ghi chú phủ có chủ đích.** Các ca còn lại của `GR-031` (C1, C2, C4…C9, liên quan rút đề/kiểm kho **lúc chạy trận**) và toàn bộ bảng quyết định của `GR-007`…`GR-011` (lượt chọn, chấm điểm hàng ngang, bấm chuông giải Chướng ngại vật) thuộc EPIC-006 *(xem §8)*, không phải EPIC-002. Feature này chỉ phủ vế **dữ liệu và cấu trúc kho đề** mà các rule đó đọc, và đó là toàn bộ giao diện giữa EPIC-002 với những rule này.

---

## 11. Assumptions

Các giá trị mặc định dưới đây được chọn cho **chi tiết không quan trọng**; mọi thứ quan trọng mà nguồn không nói đều nằm ở §9.

- **Định dạng và giới hạn dung lượng của media** không được chốt trong spec này — `CLAUDE.md` §Quy ước khác khai đây là **env config theo từng loại media**, không hard-code; spec chỉ yêu cầu *media lưu và phát lại được* (FR-001).
- **Thuật toán sinh định danh nội bộ ổn định** (FR-001b, Key Entities) là lựa chọn kỹ thuật thuộc `plan.md`, không phải nội dung của spec; spec chỉ chốt **tính chất quan sát được**: `displayId` duy nhất + mang tiền tố loại câu, và tồn tại một định danh nội bộ tách biệt, ổn định qua sửa `displayId`.
- **Cấu trúc chính xác của "ghi chú duyệt"** ở FR-011 (độ dài, có bắt buộc hay không khi trả về) không được nguồn nào quy định; spec chỉ yêu cầu *có ghi chú tới được setter*.
- **Nhật ký ở FR-017** được hiểu là ghi vào cùng nhật ký thao tác chung mà EPIC-011 sở hữu; spec này không đặc tả cấu trúc của nhật ký đó.
- **Ràng buộc kỹ thuật toàn repo** — DRY, zero-trust, chỉ tiếng Việt, trim ở cả hai đầu, string UI tách file constants — áp cho feature này theo `CLAUDE.md`; spec **tham chiếu, không sao chép**.
