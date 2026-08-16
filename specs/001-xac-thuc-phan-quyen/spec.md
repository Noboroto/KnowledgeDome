# Feature Specification: EPIC-001 — Xác thực và phân quyền

**Feature Directory**: `specs/001-xac-thuc-phan-quyen`

**Created**: 2026-07-29

**Status**: Draft

**Input**: User description: `/speckit.specify` — tạo feature specification cho **EPIC-001 — Xác thực và phân quyền**, chỉ dùng product requirement / user journey / game rule / state thuộc EPIC-001, không đưa công nghệ triển khai vào spec.

---

## 0. Nguồn đã đọc và ghi chú về nguồn

| Nguồn yêu cầu đọc | Trạng thái |
|---|---|
| `docs/PRD.md` | ✅ đã đọc — §7 Actors, §10 Journeys, §11 EPIC-001, §12 EPIC-001 (`PRD-REQ-001`…`006`), §12 EPIC-012 (`PRD-REQ-086`, `106`, `107`), §14 FS-01…FS-04, §15 NFR, §19 ma trận truy nguyên |
| `docs/glossary.md` | ✅ đã đọc — `TERM-004`…`TERM-008`, `TERM-057` |
| `docs/game-rules.md` | ✅ đã đọc — `GR-026`, `GR-029`, `GR-030`, `GR-036`, `GR-037` |
| `docs/game-state-machine.md` | ✅ đã đọc — `STATE-001` LOBBY, thang trạng thái ghế, `EVENT-028`…`EVENT-036`, bảng transition và bảng invalid-state |
| `docs/traceability.md` | ✅ đã đọc — bảng đối chiếu giá trị luật và biến thể bị loại |

Ngoài ra đã đọc `docs/decisions.md` (`QĐ-008`, `QĐ-051`, `QĐ-060`, `QĐ-064`, `QĐ-065`, `QĐ-070`, `QĐ-078`, `QĐ-086`, `QĐ-087`, `QĐ-088`) vì `docs/PRD.md` khai chúng là nguồn của các `PRD-REQ-*` thuộc EPIC-001.

---

## 1. Phạm vi feature

**Goal (nguyên văn EPIC-001)**: *"Đúng người thấy đúng thứ, và một trận luôn có đúng một người điều khiển."* — `docs/PRD.md` §11 EPIC-001.

**Product requirement trong phạm vi**:

| Requirement | Tiêu đề | Priority (PRD) |
|---|---|---|
| `PRD-REQ-001` | Truy cập public bằng đúng một URL cho khán giả và lớp phủ | P1 |
| `PRD-REQ-002` | Xác thực bắt buộc cho mọi vai có thể ghi hoặc thấy đáp án | P1 |
| `PRD-REQ-003` | Mỗi tài khoản mang đúng một vai | P1 |
| `PRD-REQ-004` | Đúng một phiên giữ quyền điều khiển, chuyển giao được | P1 |
| `PRD-REQ-005` | Permission riêng cho từng thao tác phá huỷ | P2 |
| `PRD-REQ-006` | Vai vận hành gán theo contest, tách khỏi vai hệ thống | P2 |
| `PRD-REQ-086` | Rate-limit và khoá cổng phòng khán giả | P2 — *`docs/PRD.md` gán **EPIC-012, EPIC-001*** |
| `PRD-REQ-106` | Cài đặt lần đầu: dòng lệnh khi dựng máy, tài khoản admin theo gói | P1 — *gán **EPIC-012, EPIC-001*** |
| `PRD-REQ-107` | Kênh public không có đường ghi | P1 — *gán **EPIC-012, EPIC-009, EPIC-001*** |
| `PRD-REQ-074` | Màn MC chữ lớn, chỉ đọc trừ đúng một ngoại lệ | P1 — *gán **EPIC-009, EPIC-001***. Chỉ nhận **mệnh đề quyền ghi**: bề mặt ghi duy nhất của MC là prompt duyệt cú giành quyền, và server từ chối mọi sự kiện ghi khác từ vai này. **Cách render màn MC** thuộc EPIC-009 — xem §8 |
| `PRD-REQ-108` | Giành quyền điều khiển khi phiên đang giữ mất kết nối; MC duyệt | P1 |
| `PRD-REQ-109` | Phân quyền theo vai, kiểm bằng permission | P1 |
| `PRD-REQ-110` | Phạm vi của phép gán vai | P2 |
| `PRD-REQ-111` | Vai tuỳ biến do đơn vị tự định nghĩa | P2 |
| `PRD-REQ-112` | Không thu hồi quyền của tài khoản mà trận đang cần | P1 |

> **Ba requirement `086`, `106`, `107`** được đưa vào vì `docs/PRD.md` §12 khai chúng thuộc **cả** EPIC-001. Phần thuộc epic khác của chính ba requirement đó (hồ sơ triển khai, màn khán giả, gói xuất) nằm ở §8 Out of scope.
>
> **`PRD-REQ-074`** cùng dạng: nó khai `EPIC-009, EPIC-001`, và spec này chỉ nhận **mặt EPIC-001** của nó.

**Game rule trong phạm vi** (đúng những rule mà EPIC-001 và các `PRD-REQ-*` trên tham chiếu): `GR-037` *(phạm vi hiển thị đáp án)* · `GR-026` *(phán quyết của admin — chỉ ở vế "ai được bấm")* · `GR-029`, `GR-030` *(chỉ ở vế "thao tác nào cần permission riêng")*.

**State và transition liên quan trực tiếp**: `STATE-001` LOBBY *(nơi khán giả và lớp phủ join bằng mã phòng — `game-state-machine.md` §LOBBY "Cho phép")* · `STATE-008` FINISHED *(cửa niêm phong mà permission phá huỷ đâm vào — bảng invalid-state)*.

**Actor**: `ACTOR-001` admin · `ACTOR-002` setter · `ACTOR-003` thí sinh · `ACTOR-004` MC · `ACTOR-005` khán giả · `ACTOR-006` máy dựng stream.

**Journey**: `JOURNEY-001` *(chuẩn bị kho đề — chỉ vế vai setter/admin)* · `JOURNEY-002` *(dựng contest — vế gán vai)* · `JOURNEY-004` *(vào phòng)* · `JOURNEY-006` *(xử lý sự cố — vế chuyển quyền điều khiển)*.

---

## 2. User Scenarios & Testing *(mandatory)*

### US-001 — Đăng nhập và được server kiểm quyền cho mọi thao tác (Priority: P1)

- **Actor**: `ACTOR-001` admin · `ACTOR-002` setter · `ACTOR-003` thí sinh · `ACTOR-004` MC
- **Intent**: Vào được phần việc của mình bằng tài khoản, và không chạm được vào phần việc không phải của mình.
- **User value**: Đáp án và quyền điều khiển không rơi vào tay người không có quyền — đây là điều kiện tồn tại của mọi vai còn lại.
- **Why this priority**: `PRD-REQ-002` là P1 và mọi story khác của EPIC-001 đều đứng trên nó — không có danh tính thì không có vai, không có phiên, không có permission. Hoàn thành riêng story này đã tạo một MVP dùng được: bốn vai đăng nhập được và server từ chối đúng những gì phải từ chối.
- **Independent Test**: Dựng bốn tài khoản, mỗi tài khoản một vai. Đăng nhập từng tài khoản, gọi một thao tác thuộc vai mình *(đi qua)* và một thao tác thuộc vai khác *(bị từ chối ở server)*. Không cần trận, không cần contest, không cần story nào khác.
- **Related PRD requirements**: `PRD-REQ-002`
- **Related game rules**: `GR-037`
- **Related journey**: `JOURNEY-004`

**Acceptance Scenarios**: AC-001, AC-002, AC-003, AC-004, AC-005

---

### US-002 — Vào phòng công khai bằng một URL, không có đường ghi (Priority: P1)

- **Actor**: `ACTOR-005` khán giả · `ACTOR-006` máy dựng stream
- **Intent**: Xem trận ngay, không tài khoản, không chờ duyệt — và không có cách nào tác động ngược vào trận.
- **User value**: Khán giả là số đông, không thể phát tài khoản cho từng người; đồng thời ban tổ chức không phải tin vào việc client công khai cư xử đúng.
- **Why this priority**: `PRD-REQ-001` và `PRD-REQ-107` đều P1. Đây là bề mặt duy nhất mở với người lạ, nên nó là chỗ hỏng đắt nhất nếu làm sau.
- **Independent Test**: Mở màn khán giả và lớp phủ bằng một mã phòng hợp lệ, không đăng nhập. Xác nhận thấy được trạng thái trận, và không tồn tại đường nào để hai màn này gửi một sự kiện lên server. Chạy được khi chỉ có US-001 hoặc thậm chí chưa có US-001.
- **Related PRD requirements**: `PRD-REQ-001`, `PRD-REQ-107`
- **Related game rules**: `GR-037`
- **Related journey**: `JOURNEY-004`

**Acceptance Scenarios**: AC-006 → AC-017

---

### US-003 — Mỗi tài khoản một vai, gán theo phạm vi (Priority: P1)

- **Actor**: `ACTOR-001` admin *(người gán)*; ảnh hưởng tới `ACTOR-002`, `ACTOR-003`, `ACTOR-004`
- **Intent**: Xếp đúng người vào đúng vai của **một contest cụ thể**, và biết chắc không ai vừa ngồi ghế thi vừa thấy đáp án.
- **User value**: Gian lận có cấu trúc *(thí sinh kiêm admin/MC)* trở thành thứ **không dựng nổi**, thay vì một hàng rào phải nhớ kiểm ở mọi cửa gán.
- **Why this priority**: `PRD-REQ-003` là P1 vì nó đóng một lỗ hổng của chính mô hình quyền. Gộp `PRD-REQ-006` vào cùng story vì cả hai cùng phục vụ một mục tiêu người dùng *(xếp vai cho một contest)* và không kiểm thử tách rời có nghĩa được.
- **Independent Test**: Với một contest và một tài khoản, thử lần lượt: gán vai thứ hai cho một tài khoản đã có vai *(bị từ chối)*; gán cùng một vai ở hai contest *(đi qua)*; gán làm MC ở contest A rồi kiểm quyền MC ở contest B *(không có)*.
- **Related PRD requirements**: `PRD-REQ-003`, `PRD-REQ-006`
- **Related game rules**: `GR-037` *(lý do tồn tại của ràng buộc: admin và MC thấy đáp án)*
- **Related journey**: `JOURNEY-002`, `JOURNEY-001`

**Acceptance Scenarios**: AC-018 → AC-022, AC-041

---

### US-004 — Một phiên giữ quyền điều khiển, chuyển giao được khi đổi người (Priority: P1)

- **Actor**: `ACTOR-001` admin
- **Intent**: Chạy trận với đúng một người bấm, và thay được người đó giữa buổi mà không mất trận.
- **User value**: Admin ốm, máy hỏng hay đổi ca không còn là sự cố không lối thoát; đồng thời không bao giờ có hai luồng thao tác song song trên cùng một trận.
- **Why this priority**: `PRD-REQ-004` là P1 và nó là vế thứ hai của chính goal EPIC-001 *("một trận luôn có đúng một người điều khiển")*.
- **Independent Test**: Mở hai phiên admin trên cùng một contest. Xác nhận chỉ một phiên bấm được, phiên kia thấy đủ nhưng không bấm được; thực hiện chuyển quyền và xác nhận hai vai trò đảo lại kèm một dòng nhật ký.
- **Related PRD requirements**: `PRD-REQ-004`, `PRD-REQ-108`
- **Related game rules**: `GR-026` *(vế "điểm chỉ chốt khi ADMIN bấm" và "mỗi contest chỉ có MỘT admin")*, `GR-036` *(vế "mất kết nối là điều kiện quan sát được của server")*
- **Related journey**: `JOURNEY-006`

**Acceptance Scenarios**: AC-023 → AC-034, AC-048, AC-056, AC-057, AC-060, AC-061, AC-062

---

### US-005 — Cấp quyền theo vai, tách riêng từng thao tác phá huỷ (Priority: P2)

- **Actor**: `ACTOR-001` admin *(người cấp quyền và người bị giới hạn)*
- **Intent**: Giao cho một người quyền chạy trận mà không đồng thời giao cho họ quyền xoá một vòng đã chạy.
- **User value**: Ban tổ chức phân được trách nhiệm; sai lầm nặng nhất chỉ nằm trong tay người được chỉ định.
- **Why this priority**: `PRD-REQ-005` là P2 — trận vẫn chạy được nếu mọi thao tác phá huỷ đều đi kèm vai admin, nhưng khi đó không có cách nào cấp quyền hẹp hơn.
- **Independent Test**: Cấp một tài khoản quyền vận hành trận nhưng **không** quyền bỏ vòng. Đăng nhập, xác nhận nút bỏ vòng không dùng được và một yêu cầu bỏ vòng gửi thẳng tới server vẫn bị từ chối.
- **Related PRD requirements**: `PRD-REQ-005`, `PRD-REQ-109`, `PRD-REQ-110`, `PRD-REQ-111`, `PRD-REQ-112`
- **Related game rules**: `GR-029`, `GR-030` *(chỉ ở vế "thao tác nào thuộc hạng phá huỷ")*, `GR-032` *(vế "ai duyệt tín hiệu")*, `GR-026` *(vế "phán quyết là điều kiện chuyển câu")*
- **Related journey**: `JOURNEY-006`, `JOURNEY-002`

**Acceptance Scenarios**: AC-035 → AC-048, AC-058

---

### US-006 — Giữ cổng khán giả trước lưu lượng bất thường (Priority: P2)

- **Actor**: `ACTOR-001` admin · ảnh hưởng tới `ACTOR-005` khán giả
- **Intent**: Chặn được dòng người vào phòng khi đường dẫn bị phát tán, mà không đuổi những người đang xem.
- **User value**: Mã phòng là công khai, nên đây là van duy nhất của người vận hành trên bề mặt mở nhất của hệ thống.
- **Why this priority**: `PRD-REQ-086` là P2 — trận vẫn chạy khi không có van này, nhưng người vận hành mất khả năng phản ứng.
- **Independent Test**: Mở phòng, cho vài phiên khán giả vào, bấm khoá cổng. Xác nhận phiên mới bị từ chối và phiên đang xem vẫn nhận cập nhật.
- **Related PRD requirements**: `PRD-REQ-086`
- **Related game rules**: — *(`docs/PRD.md` §12 khai `Related game rules: —`)*
- **Related journey**: `JOURNEY-004`

**Acceptance Scenarios**: AC-049, AC-050, AC-051, AC-059

---

### US-007 — Có tài khoản admin dùng được trên một bản cài trống (Priority: P2)

- **Actor**: `ACTOR-001` admin *(và người dựng máy)*
- **Intent**: Đăng nhập điều khiển được ngay sau khi dựng máy, hoặc ngay sau khi nhập gói contest ở hội trường.
- **User value**: Người vận hành ngày thi không phải làm việc của người dựng máy trước giờ phát sóng.
- **Why this priority**: `PRD-REQ-106` là P1 ở PRD, nhưng trong phạm vi **feature này** nó đứng sau US-001…US-004: nó là cách *tạo ra* tài khoản admin đầu tiên, còn ba story kia định nghĩa tài khoản đó *làm được gì*. Đặt P2 vì nó kiểm thử được độc lập và không chặn MVP của EPIC-001.
- **Independent Test**: Trên một bản cài trống: (a) tạo tài khoản admin qua đường dòng lệnh rồi đăng nhập; (b) nhập một gói contest có kèm danh sách người tham gia rồi đăng nhập bằng tài khoản admin của contest — không mở dòng lệnh.
- **Related PRD requirements**: `PRD-REQ-106`
- **Related game rules**: — *(`docs/PRD.md` §12 khai `Related game rules: —`)*
- **Related journey**: `JOURNEY-003`, `JOURNEY-004`

**Acceptance Scenarios**: AC-052, AC-053, AC-054, AC-055

---

## 3. Acceptance Scenarios *(mandatory)*

> Quy ước: **Given** nêu đủ actor, trạng thái trận, dữ liệu ban đầu và tiền điều kiện · **When** chỉ chứa một hành động hoặc một sự kiện · **Then** kiểm kết quả nghiệp vụ, chuyển trạng thái, tác dụng phụ, kết quả lỗi, và **những gì KHÔNG được đổi**.

### US-001 — Đăng nhập và kiểm quyền ở server

**AC-001 — Happy path: bốn vai đăng nhập được**
- **US**: US-001 · **FR**: FR-001, FR-002 · **GR**: `GR-037`
- **Given** một bản cài đã có bốn tài khoản, lần lượt mang vai hệ thống thí sinh, MC, admin, setter; trận chưa tạo hoặc đang ở `STATE-001` LOBBY; chưa phiên nào đăng nhập
- **When** mỗi tài khoản đăng nhập bằng thông tin đúng của mình
- **Then** cả bốn phiên được xác lập danh tính · mỗi phiên chỉ thấy bề mặt của vai mình · trạng thái trận **không đổi** · không sinh sự kiện điểm nào

**AC-002 — Rejection: yêu cầu vượt quyền bị từ chối ở server dù đã vào phòng**
- **US**: US-001 · **FR**: FR-002, FR-003 · **GR**: `GR-037`
- **Given** một phiên **thí sinh** đã đăng nhập và đã vào đúng phòng của trận đang chạy; trận có nhật ký sự kiện gồm N mục
- **When** phiên đó gửi một yêu cầu thuộc thẩm quyền admin *(ví dụ: phán quyết một câu)* trực tiếp tới server, bỏ qua giao diện
- **Then** server **từ chối** · **không** sinh sự kiện nào · nhật ký sự kiện vẫn đúng N mục · điểm mọi ghế **không đổi** · trạng thái câu và trạng thái vòng **không đổi**

**AC-003 — Rejection: giao diện ẩn nút không phải là lớp bảo vệ**
- **US**: US-001 · **FR**: FR-002, FR-003
- **Given** một phiên **MC** đã đăng nhập; màn MC theo đặc tả là read-only nên không render nút điều khiển nào
- **When** phiên đó gửi một yêu cầu ghi tới server bằng đường khác giao diện
- **Then** server từ chối vì **thiếu quyền**, không phải vì giao diện không có nút · không có dữ liệu nào thay đổi

**AC-004 — Boundary: phiên chưa xác thực**
- **US**: US-001 · **FR**: FR-001, FR-002
- **Given** một phiên chưa đăng nhập, đang mở đúng đường dẫn phòng hợp lệ
- **When** phiên đó gọi một thao tác dành cho vai đã xác thực *(ví dụ: gửi đáp án)*
- **Then** server từ chối · không sinh sự kiện · **không** vì thế mà mất quyền xem công khai đã có theo US-002

**AC-005 — Repeated action: đăng nhập sai nhiều lần rồi đúng**
- **US**: US-001 · **FR**: FR-001, FR-004
- **Given** một tài khoản thí sinh hợp lệ; chưa có phiên nào của tài khoản này
- **When** đăng nhập sai ba lần liên tiếp rồi đăng nhập đúng một lần
- **Then** ba lần đầu bị từ chối và **không** cấp danh tính · lần thứ tư thành công · mỗi lần *(cả thất bại lẫn thành công)* đều được ghi lại · vai và quyền của tài khoản **không đổi** vì các lần thất bại

### US-002 — Truy cập công khai và phạm vi hiển thị đáp án

**AC-006 — Happy path: vào phòng công khai bằng đúng một URL**
- **US**: US-002 · **FR**: FR-005 · **GR**: `GR-037`
- **Given** một trận ở `STATE-001` LOBBY; đường dẫn phòng đã phát, mã 6 số nằm trong đường dẫn; cổng khán giả **chưa** bị khoá; người xem không có tài khoản
- **When** người xem mở đúng đường dẫn đó
- **Then** thấy phòng **ngay**, **không** màn đăng nhập, **không** bước nhập mã, **không** chờ duyệt · nhận được ảnh chụp trạng thái trận lúc vào · trạng thái trận **không đổi**

**AC-007 — Happy path: lớp phủ vào cùng đường dẫn, cùng mô hình truy cập**
- **US**: US-002 · **FR**: FR-005, FR-006
- **Given** cùng trận như AC-006; máy dựng stream chưa kết nối; phần mềm dựng hình chỉ nhận được **một URL**, không có chỗ nhập gì thêm
- **When** máy dựng stream mở đường dẫn lớp phủ
- **Then** vào được, không tài khoản và không vai · nhận cập nhật một chiều · trạng thái trận **không đổi**

**AC-008 — No-change guarantee: kênh công khai không có đường ghi**
- **US**: US-002 · **FR**: FR-006, FR-007 · **GR**: `GR-037`
- **Given** một phiên khán giả và một phiên lớp phủ đang xem một trận đang chạy; nhật ký sự kiện gồm N mục
- **When** hai phiên đó cố gửi bất kỳ sự kiện nào lên server
- **Then** **không tồn tại đường nào để gửi** — đây là tính chất cấu trúc của kênh, không phải một luật server phải cưỡng chế *(`PRD-REQ-107`)* · nhật ký vẫn đúng N mục · không ghế nào đổi điểm

**AC-009 — Invalid state: kênh hai chiều không mở cho vai công khai**
- **US**: US-002 · **FR**: FR-007
- **Given** một phiên không xác thực đang xem bằng mã phòng
- **When** phiên đó cố mở kênh hai chiều vốn dành cho admin, thí sinh và MC
- **Then** bị từ chối vì **chưa xác thực** · phiên xem công khai hiện có **không** bị ngắt

**AC-010 — Giữ permission đọc đáp án + đang điều khiển ⇒ trả đáp án, ghi audit**
- **US**: US-002 · **FR**: FR-008 · **GR**: `GR-037` C1
- **Given** một câu đang mở, **chưa** tới mốc câu khép; một phiên đã xác thực **giữ permission đọc đáp án của câu đang chạy** *(vai Quản trị dựng sẵn)*; cờ `revealAnswerAfterJudge` bất kỳ
- **When** phiên đó yêu cầu xem đáp án chuẩn
- **Then** server **trả** đáp án · ghi một dòng audit cho lần xem · **không** sinh sự kiện điểm · trạng thái trận **không đổi**

**AC-011 — Giữ permission đọc đáp án mà không điều khiển ⇒ vẫn trả đáp án**
- **US**: US-002 · **FR**: FR-008 · **GR**: `GR-037` C2
- **Given** cùng tiền điều kiện AC-010; phiên mang **vai MC** — giữ đúng permission đọc đáp án, **không** giữ quyền điều khiển và **không** giữ permission công bố
- **When** phiên đó yêu cầu xem đáp án chuẩn
- **Then** server **trả** đáp án · ghi audit · phiên đó **không** công bố được đáp án cho ai khác, và **không** nhận thêm quyền ghi nào

**AC-012 — Rejection: đổi tên vai không ảnh hưởng, gỡ permission thì mất quyền đọc**
- **US**: US-002 · **FR**: FR-008 · **GR**: `GR-037` C1
- **Given** một vai tuỳ biến **không** chứa permission đọc đáp án của câu đang chạy; một phiên mang vai đó, đang giữ quyền điều khiển trận; một câu đang mở **chưa khép**; cờ `revealAnswerAfterJudge` **TẮT**
- **When** phiên đó yêu cầu xem đáp án chuẩn
- **Then** server **không trả** — dù phiên đó đang **điều khiển** trận · chứng tỏ cửa kiểm hỏi **permission**, không hỏi vai và không hỏi việc có giữ quyền điều khiển hay không

**AC-013 — Cờ reveal TẮT ⇒ ba vai không giữ permission không nhận**
- **US**: US-002 · **FR**: FR-008 · **GR**: `GR-037` C3
- **Given** một trận có `revealAnswerAfterJudge` **TẮT** *(giá trị đã chụp vào trận lúc bắt đầu)*; một câu **đã khép**; ba phiên: thí sinh, khán giả, lớp phủ
- **When** cả ba yêu cầu đáp án chuẩn
- **Then** server **không trả** cho cả ba · không có thông báo rò rỉ nội dung đáp án · quyền của admin và MC **không đổi**

**AC-014 — Reveal BẬT nhưng câu chưa khép ⇒ không trả và chưa từng đẩy**
- **US**: US-002 · **FR**: FR-008, FR-009 · **GR**: `GR-037` C4
- **Given** một trận có `revealAnswerAfterJudge` **BẬT**; một câu đang ở trạng thái **chưa khép**; ba phiên thí sinh, khán giả, lớp phủ
- **When** cả ba yêu cầu đáp án chuẩn
- **Then** server **không trả** · và server **chưa từng đẩy** đáp án xuống ba kênh này từ trước *(kiểm ở phía truyền, không phải ở phía hiển thị — `GR-037` §Cấm)*

**AC-015 — Câu đã khép *(chấm xong hoặc bị bỏ qua)*, reveal BẬT ⇒ công bố**
- **US**: US-002 · **FR**: FR-008 · **GR**: `GR-037` C5, C7
- **Given** một trận có `revealAnswerAfterJudge` **BẬT**; hai câu: một câu **đã khép do admin chấm xong**, một câu **bị bỏ qua vì hết cửa sổ chuông không ai bấm**
- **When** thí sinh, khán giả và lớp phủ nhận cập nhật cho hai câu đó
- **Then** đáp án của **cả hai** câu được công bố tới cả ba vai · điểm **không** vì thế mà đổi · trạng thái vòng **không đổi**

**AC-016 — Cửa sổ cướp quyền Về đích đang mở ⇒ câu chưa khép, không trả**
- **US**: US-002 · **FR**: FR-008 · **GR**: `GR-037` C6
- **Given** vòng Về đích, `revealAnswerAfterJudge` **BẬT**; người thi chính vừa bị admin chấm **Sai**; cửa sổ cướp quyền **đang mở** và chưa có ai được chấm
- **When** thí sinh, khán giả hoặc lớp phủ yêu cầu đáp án
- **Then** server **không trả** — câu **chưa khép** · cửa sổ cướp quyền và mọi con số của luật **không đổi**

**AC-017 — Huỷ kết quả và đáp án Chướng ngại vật: hai ca biên không tự công bố**
- **US**: US-002 · **FR**: FR-008 · **GR**: `GR-037` C8, C9
- **Given** `revealAnswerAfterJudge` **BẬT**; một câu vừa khép bằng phán quyết **Huỷ kết quả**; và một đáp án **Chướng ngại vật**
- **When** thí sinh, khán giả và lớp phủ nhận cập nhật
- **Then** đáp án của câu Huỷ kết quả **không tự công bố** *(admin vẫn mở tay được)* · đáp án Chướng ngại vật **không đi theo cơ chế này** mà theo `GR-012` — nằm ngoài phạm vi feature · không sinh sự kiện điểm nào

**AC-017a — Đang chờ admin kích hoạt tay một tín hiệu khác ⇒ mốc câu khép lùi**
- **US**: US-002 · **FR**: FR-008, FR-009 · **GR**: `GR-037` C8b · **PRD**: `PRD-REQ-113`
- **Given** `revealAnswerAfterJudge` **BẬT**; một vòng có giành quyền bằng chuông; người đang giữ quyền vừa bị chấm **Huỷ kết quả**; hàng đợi còn tín hiệu hợp lệ và admin **chưa** kích hoạt tay ai
- **When** thí sinh, khán giả hoặc lớp phủ yêu cầu đáp án chuẩn của câu đó
- **Then** server **không trả** — câu **chưa khép** vì mốc đã **lùi** tới sau khi người được kích hoạt được chấm · server cũng **chưa từng đẩy** đáp án xuống ba kênh này từ trước *(kiểm ở phía truyền)* · **cơ chế** kích hoạt tay thuộc EPIC-006 *(xem §8)*; feature này chỉ phủ vế **đáp án không rời server**

### US-003 — Vai và ràng buộc loại trừ

**AC-017b — Admin đang giữ cú đóng hiển thị bằng tay ⇒ cờ reveal BẬT vẫn không công bố**
- **US**: US-002 · **FR**: FR-008 · **GR**: `GR-037` C10
- **Given** một trận có `revealAnswerAfterJudge` **BẬT**; một câu **đã khép**; admin giữ `PERM-044` và đã bấm **đóng hiển thị bằng tay** cho chính câu đó; ba phiên thí sinh, khán giả và lớp phủ đang kết nối
- **When** engine tới mốc công bố của câu đó, và cả ba phiên nhận cập nhật
- **Then** server **không trả** đáp án cho cả ba — **thao tác tay thắng cờ tự động** · đáp án giữ kín **cho tới khi admin tự mở lại** · **0** byte đáp án nào rời server tới ba kênh này · điểm của mọi ghế **không đổi** · trạng thái trận **không đổi** · phiên giữ permission đọc đáp án **vẫn** xem được như thường

**AC-018 — Rejection: không gán được vai thứ hai**
- **US**: US-003 · **FR**: FR-010 · **GR**: `GR-037`
- **Given** một tài khoản đã mang vai **Người ra đề** ở phạm vi hệ thống; hai contest A và B tồn tại
- **When** admin gán thêm cho tài khoản đó vai **Quản trị**, ở bất kỳ phạm vi nào
- **Then** phép gán bị **từ chối ở server** · tài khoản vẫn mang đúng vai Người ra đề · quyền hiện có **không đổi** · muốn người đó vừa soạn đề vừa điều khiển trận thì phải dùng **hai tài khoản**

**AC-019 — Loại trừ theo CẢ HAI CHIỀU, do cấu trúc**
- **US**: US-003 · **FR**: FR-010, FR-011 · **GR**: `GR-037`
- **Given** contest A tồn tại; tài khoản U mang vai **Thí sinh** và đang ngồi ghế của một trận thuộc contest A; tài khoản V mang vai **Quản trị** phạm vi contest A
- **When** admin thử gán U làm Quản trị / MC / Người ra đề *(chiều thứ nhất)*, rồi thử xếp V vào một ghế thí sinh của contest A *(chiều thứ hai)*
- **Then** **cả hai chiều đều bị từ chối**, và lý do là **cùng một lý do** — một tài khoản không mang được hai vai · U vẫn là thí sinh, ghế và điểm **không đổi** · V vẫn là Quản trị, không ghế nào được gán · **không** phép kiểm riêng nào ở cửa ghế phải tồn tại để điều này đúng

**AC-020 — Happy path: một vai, nhiều phạm vi**
- **US**: US-003 · **FR**: FR-012
- **Given** tài khoản U mang vai **MC**, đã được gán ở **contest A**; contest B tồn tại và U chưa có phép gán nào ở B
- **When** admin gán U làm **MC của contest B**
- **Then** phép gán **đi qua** — cùng một vai, thêm một phạm vi · U dẫn được cả hai contest · phép gán ở contest A **không đổi**

**AC-021 — Vai vận hành có phạm vi contest**
- **US**: US-003 · **FR**: FR-013
- **Given** tài khoản U đã được gán vai vận hành **MC** của **contest A**; contest B tồn tại, U không có vai gì ở B
- **When** U mở màn MC của **contest B**
- **Then** bị từ chối vì **không có quyền MC ở contest B** · quyền MC của U ở contest A **không đổi**

**AC-022 — Repeated action: gán lại đúng vai đã có**
- **US**: US-003 · **FR**: FR-010, FR-013
- **Given** tài khoản U đã là MC của contest A
- **When** admin gán U làm MC của contest A một lần nữa
- **Then** trạng thái quyền của U **giống hệt** trước thao tác — không nhân đôi phép gán, không mất phép gán · U vẫn mang **đúng một** vai

### US-004 — Phiên giữ quyền điều khiển

**AC-023 — Happy path: hai phiên admin, một phiên bấm được**
- **US**: US-004 · **FR**: FR-014, FR-015 · **GR**: `GR-026`
- **Given** contest A có **hai tài khoản** mang vai admin; trận đang ở `STATE-001` LOBBY; phiên P1 đang giữ quyền điều khiển; phiên P2 vừa đăng nhập
- **When** P2 thử một thao tác ghi *(ví dụ: mở một vòng)*
- **Then** P2 **không bấm được** — xem đủ, ghi không được · trạng thái trận **không đổi** · P1 vẫn bấm được bình thường

**AC-024 — Happy path: chuyển quyền điều khiển**
- **US**: US-004 · **FR**: FR-016, FR-017
- **Given** cùng tiền điều kiện AC-023; nhật ký có N dòng
- **When** thực hiện thao tác chuyển quyền điều khiển từ P1 sang P2
- **Then** P2 **bấm được**, P1 chuyển sang xem-không-bấm · nhật ký có **N+1** dòng, dòng mới ghi lần chuyển · điểm, nhật ký sự kiện trận và trạng thái vòng **không đổi**

**AC-025 — Truy nguyên: mỗi sự kiện mang đúng một actor**
- **US**: US-004 · **FR**: FR-018
- **Given** quyền điều khiển vừa chuyển từ P1 sang P2 *(AC-024)*; một câu đang chờ phán quyết
- **When** P2 bấm phán quyết
- **Then** sự kiện sinh ra mang actor là **P2** · sự kiện cũ do P1 sinh vẫn mang actor **P1**, **không bị viết lại**

**AC-026 — Concurrency: không tồn tại hai luồng thao tác admin song song**
- **US**: US-004 · **FR**: FR-015 · **GR**: `GR-026` §Đồng thời
- **Given** P1 giữ quyền, P2 xem-không-bấm; cùng một câu đang chờ phán quyết
- **When** P1 và P2 gửi phán quyết cho cùng câu đó gần như cùng lúc
- **Then** chỉ yêu cầu của **P1** có hiệu lực · yêu cầu của P2 bị từ chối vì **không giữ quyền** · câu đi qua **đúng một** phán quyết *(`GR-026`)* · điểm cộng đúng **một** lần

**AC-027 — Idempotency: bấm chuyển quyền hai lần**
- **US**: US-004 · **FR**: FR-016, FR-017
- **Given** quyền vừa được chuyển từ P1 sang P2; P2 đang giữ quyền
- **When** lệnh chuyển quyền *"từ P1 sang P2"* tới server lần thứ hai
- **Then** quyền vẫn ở P2, **không** bật ngược về P1 · trạng thái trận **không đổi** · nhật ký phản ánh trung thực điều đã xảy ra, không tạo ra một lần chuyển giả

**AC-028 — Sự cố: phiên giữ quyền mất kết nối**
- **US**: US-004 · **FR**: FR-019
- **Given** P1 đang giữ quyền và đang giữa một vòng có đồng hồ đang chạy; P2 đang xem-không-bấm
- **When** P1 mất kết nối
- **Then** **đồng hồ vẫn chạy**, trận **không tự tạm dừng**, **không** có vai dự phòng tự động nhận quyền · thí sinh vẫn bị khoá theo giờ như thường · P1 quay lại thì khôi phục trạng thái từ server bằng đúng cơ chế của một client thường

**AC-029 — Rejection: không giành được khi phiên đang giữ còn kết nối**
- **US**: US-004 · **FR**: FR-020, FR-021 · **GR**: `GR-036`
- **Given** P1 đang giữ quyền và **đang kết nối bình thường**; P2 là một phiên admin khác của cùng contest; contest có MC
- **When** P2 thử giành quyền điều khiển
- **Then** thao tác **không tồn tại** — nút không bật, và yêu cầu gửi thẳng tới server bị từ chối · **không** prompt nào lên màn MC · quyền vẫn ở P1 · trạng thái trận, điểm và đồng hồ **không đổi**

**AC-030 — Happy path: không có MC ⇒ giành có hiệu lực ngay và âm thầm**
- **US**: US-004 · **FR**: FR-020, FR-022
- **Given** contest **không có phiên MC nào đang kết nối** *(dù có hay không có phép gán vai MC)*; P1 đang giữ quyền và **đã mất kết nối**; một câu đang mở với đồng hồ đang chạy; P2 là phiên admin khác
- **When** P2 giành quyền điều khiển
- **Then** quyền chuyển sang P2 **ngay**, không dialog xác nhận · **không** thông báo nào tới máy thí sinh, màn khán giả hay lớp phủ · hạn chót của câu đang mở **không đặt lại và không dừng** · điểm mọi ghế **không đổi** · một dòng nhật ký được ghi

**AC-031 — Happy path: có MC ⇒ quyền chỉ đổi sau khi MC duyệt**
- **US**: US-004 · **FR**: FR-021, FR-023 · **GR**: `GR-026`
- **Given** contest có **một phiên MC đang kết nối**; P1 đang giữ quyền và **đã mất kết nối**; một câu đang mở, đồng hồ đang chạy; P2 là phiên admin khác
- **When** P2 giành quyền, sau đó MC bấm **Duyệt**
- **Then** ngay khi P2 bấm: prompt lên màn MC, **không có nút đóng**, và quyền **chưa** đổi · đồng hồ **vẫn chạy** và vòng **vẫn tiếp** suốt thời gian chờ · sau khi MC duyệt: quyền ở P2, prompt tắt · cả cú giành lẫn cú duyệt đều vào nhật ký

**AC-032 — Rejection path: MC từ chối**
- **US**: US-004 · **FR**: FR-021, FR-023
- **Given** cùng tiền điều kiện AC-031; prompt đang chờ trên màn MC
- **When** MC bấm **Từ chối**
- **Then** quyền **ở nguyên** P1 · prompt tắt · cú giành giữ **vĩnh viễn** trong lịch sử ở trạng thái bị từ chối, **không bị xoá** · cả cú giành lẫn cú từ chối đều vào nhật ký · P2 giành lại được nếu P1 **vẫn** mất kết nối · điểm và đồng hồ **không đổi**

**AC-033 — Concurrency: nhiều phiên cùng giành**
- **US**: US-004 · **FR**: FR-022
- **Given** contest **không có MC**; P1 đang giữ quyền và đã mất kết nối; P2 là **chủ contest**, P3 là một admin thường; cả hai cùng giành trong một cửa sổ rất ngắn
- **When** P2 và P3 gửi cú giành gần như cùng lúc
- **Then** **P2 thắng** vì là chủ contest, **bất kể** server timestamp của ai tới trước · P3 không nhận quyền · chỉ **một** lần chuyển quyền được ghi là có hiệu lực; cú giành của P3 vẫn vào nhật ký

**AC-034 — No-change guarantee: cú giành mất đối tượng**
- **US**: US-004 · **FR**: FR-021, FR-023
- **Given** contest có MC; P2 đã giành quyền và prompt đang chờ trên màn MC; MC **chưa** bấm gì
- **When** P1 **kết nối lại** trước khi MC quyết
- **Then** prompt **tắt**, cú giành **mất đối tượng** — không xoá và không cảnh báo · quyền **ở nguyên** P1 · P1 khôi phục đúng màn đang thi · điểm, nhật ký sự kiện trận và đồng hồ **không đổi** · cú giành vẫn còn trong nhật ký

**AC-056 — State transition: quyền điều khiển TRỐNG thì nhận ngay, không qua MC**
- **US**: US-004 · **FR**: FR-015, FR-020b · **GR**: `GR-026`
- **Given** contest A có hai tài khoản admin và có **một phiên MC đang kết nối**; trận vừa tạo, **chưa phiên admin nào vào**; nhật ký có N dòng
- **When** phiên P1 vào trận *(nhận quyền lần đầu)*, sau đó P1 **đăng xuất chủ động**, rồi phiên P2 vào trận
- **Then** P1 nhận quyền **ngay** khi vào · đăng xuất của P1 **nhả** quyền, để trận ở trạng thái quyền **TRỐNG** · P2 nhận quyền **ngay**, **không** prompt nào lên màn MC dù contest có MC đang kết nối, **không** dialog xác nhận · nhật ký có **N+3** dòng · điểm, trạng thái vòng và đồng hồ **không đổi** ở cả ba thao tác

**AC-057 — Error outcome: phiên MC đang giữ prompt mất kết nối**
- **US**: US-004 · **FR**: FR-021, FR-021b, FR-022 · **GR**: `GR-036`
- **Given** contest có **một phiên MC đang kết nối**; P1 đang giữ quyền và **đã mất kết nối**; một câu đang mở, đồng hồ đang chạy; P2 đã giành quyền và prompt **đang chờ** trên màn MC
- **When** phiên MC **mất kết nối** trước khi bấm Duyệt hoặc Từ chối
- **Then** cú giành **rơi về nhánh không-MC** và có hiệu lực **ngay** — quyền chuyển sang P2 · **không** ngưỡng thời gian nào được chờ hết · một dòng nhật ký ghi việc cú giành chuyển nhánh vì MC mất kết nối · hạn chót của câu đang mở **không đặt lại và không dừng** · điểm mọi ghế **không đổi** · **không** thông báo nào tới thí sinh, khán giả hoặc lớp phủ

**AC-062 — Concurrency: hai phiên admin cùng nhận một quyền TRỐNG**
- **US**: US-004 · **FR**: FR-015, FR-020b · **GR**: `GR-026` · **QĐ**: `QĐ-103`
- **Given** contest A có ba tài khoản admin và **một phiên MC đang kết nối**; trận đang chạy, quyền điều khiển ở trạng thái **TRỐNG** *(holder vừa đăng xuất chủ động)*; một câu đang mở, đồng hồ đang chạy; P2 là **chủ contest**, P3 và P4 là admin thường
- **When** P3 và P4 cùng gửi yêu cầu nhận quyền, server timestamp của P3 **sớm hơn 1 ms**; trong một lần chạy khác, hai timestamp **bằng nhau ở mili-giây**
- **Then** lần đầu: **P3 thắng** vì tới trước — việc P2 là chủ contest **không** cho P2 hay ai ưu tiên, và P2 thậm chí không tham gia · lần hai: server **bốc ngẫu nhiên** giữa P3 và P4, kết quả **có hiệu lực ngay**, **không** ai xác nhận, và được ghi thành **một sự kiện** mang nhãn chỉ rõ thắng do bốc · cả hai lần: **không** prompt nào lên màn MC · đúng **một** phiên giữ quyền sau đó · đồng hồ, điểm và trạng thái vòng **không đổi**

**AC-060 — Concurrency: nhiều phiên MC, quyết định đầu tiên thắng**
- **US**: US-004 · **FR**: FR-021a, FR-021b · **GR**: `GR-026`
- **Given** contest có **ba phiên MC** đang kết nối (M1, M2, M3); P1 đang giữ quyền và **đã mất kết nối**; một câu đang mở, đồng hồ đang chạy; P2 vừa giành quyền
- **When** prompt lên cả ba màn; M1 bấm **Duyệt** và M2 bấm **Từ chối** gần như cùng lúc, M1 tới server trước; sau đó M3 bấm **Từ chối**
- **Then** quyết định của **M1** có hiệu lực — quyền chuyển sang P2 · server **đóng prompt** ở cả M2 và M3 · quyết định của M2 và M3 **bị từ chối** và **không lật** kết quả · cả hai vẫn vào nhật ký kèm trạng thái bị từ chối · không phiên MC nào là phiên chính, và **không** thứ tự ưu tiên nào được áp — chỉ server timestamp · đồng hồ và điểm **không đổi**

**AC-061 — Boundary: fallback chỉ khi phiên MC CUỐI CÙNG rớt**
- **US**: US-004 · **FR**: FR-021b, FR-021a · **GR**: `GR-036`
- **Given** cùng tiền điều kiện AC-060 với **hai** phiên MC (M1, M2); prompt đang chờ trên cả hai; chưa ai bấm
- **When** M1 mất kết nối, sau đó M2 cũng mất kết nối
- **Then** khi M1 rớt: prompt **vẫn chờ** trên M2, quyền **chưa** đổi · khi M2 rớt: cú giành **rơi về nhánh không-MC** và có hiệu lực ngay, quyền sang P2, một dòng nhật ký · hạn chót câu đang mở **không đặt lại và không dừng** · điểm **không đổi**

### US-005 — Permission cho thao tác phá huỷ

**AC-035 — Happy path: quyền hẹp vẫn chạy được trận**
- **US**: US-005 · **FR**: FR-024, FR-025 · **GR**: `GR-030`
- **Given** một tài khoản được cấp quyền vận hành trận nhưng **không** có permission *bỏ vòng*; trận đang chạy một vòng
- **When** tài khoản đó thực hiện các thao tác vận hành thường *(hiển thị câu, start timer, phán quyết)*
- **Then** mọi thao tác vận hành **đi qua** bình thường

**AC-036 — Rejection: thiếu permission thì cả giao diện lẫn server đều chặn**
- **US**: US-005 · **FR**: FR-024, FR-025 · **GR**: `GR-030`
- **Given** cùng tài khoản ở AC-035; nhật ký sự kiện có N mục; điểm các ghế đã biết
- **When** tài khoản đó bấm *bỏ vòng*, rồi gửi thẳng một yêu cầu bỏ vòng tới server
- **Then** nút **không dùng được** · server **từ chối** yêu cầu gửi thẳng · nhật ký vẫn N mục, **không** sinh sự kiện đảo ngược nào · điểm mọi ghế **không đổi** · vòng vẫn đang chạy

**AC-037 — Boundary: permission tách riêng cho từng thao tác**
- **US**: US-005 · **FR**: FR-024 · **GR**: `GR-029`, `GR-030`
- **Given** một tài khoản được cấp permission *điều chỉnh điểm* nhưng **không** có permission *huỷ trận*
- **When** tài khoản đó thực hiện một điều chỉnh điểm hợp lệ, sau đó thử huỷ trận
- **Then** điều chỉnh điểm **đi qua** · huỷ trận **bị từ chối** · việc có permission này **không** suy ra permission kia, và **không** suy ra từ việc *"là admin"*

**AC-038 — Invalid state chồng lên permission: trận đã đóng sổ**
- **US**: US-005 · **FR**: FR-024, FR-025 · **GR**: `GR-029` C6
- **Given** một tài khoản **có đủ** permission *điều chỉnh điểm*; trận ở `STATE-008` FINISHED *(cả hai nhãn: hoàn thành hoặc bỏ dở)*
- **When** tài khoản đó thử điều chỉnh điểm
- **Then** thao tác **không thực hiện được** vì trận đã niêm phong — đây là **invalid state**, **không ép được**, và độc lập với việc có permission · biên bản đã đóng **không đổi**

**AC-039 — No-change guarantee: đổi tên vai không đổi quyền**
- **US**: US-005 · **FR**: FR-026
- **Given** một tài khoản mang một vai có tập permission P; tài khoản đang vận hành một trận
- **When** người quản trị **đổi tên** vai đó *(tập permission P giữ nguyên)*
- **Then** mọi thao tác tài khoản đó làm được **trước** và **sau** là **y hệt nhau** — chứng tỏ không đường kiểm quyền nào đọc tên vai · trạng thái trận **không đổi**

**AC-040 — Happy path: đổi tập permission của vai có hiệu lực ngay**
- **US**: US-005 · **FR**: FR-026, FR-027
- **Given** một vai tuỳ biến **không** chứa permission *bỏ vòng*; một tài khoản mang vai đó đang vận hành một trận
- **When** người quản trị **thêm** permission *bỏ vòng* vào vai đó
- **Then** tài khoản kia bỏ vòng được, **không** phải sửa mã nguồn và **không** phải cấp gì thẳng cho tài khoản · trước thao tác này, mọi cố gắng bỏ vòng đều bị server từ chối

**AC-041 — Rejection: phép gán có phạm vi contest**
- **US**: US-005, US-003 · **FR**: FR-028
- **Given** tài khoản U được gán một vai chứa permission *phán quyết* với phạm vi **contest A**; contest B tồn tại và U không có phép gán nào ở B
- **When** U gửi một yêu cầu phán quyết cho một trận thuộc **contest B**
- **Then** server **từ chối** · quyền của U ở contest A **không đổi** · không sự kiện nào sinh ra ở cả hai contest

**AC-042 — Happy path: vai tuỳ biến bỏ bảy thao tác phá huỷ**
- **US**: US-005 · **FR**: FR-029
- **Given** một bản cài chỉ có bốn vai dựng sẵn
- **When** người quản trị tạo một vai tuỳ biến gồm mọi permission vận hành trận **trừ** bảy permission phá huỷ, rồi gán cho một tài khoản
- **Then** tài khoản đó chạy được **trọn một trận** — hiển thị câu, start timer, phán quyết, chốt trận · và **cả bảy** thao tác phá huỷ đều bị từ chối ở server

**AC-043 — Happy path: vai tuỳ biến đi theo gói contest**
- **US**: US-005 · **FR**: FR-029
- **Given** bản nguồn có một vai tuỳ biến đang được gán cho một tài khoản trong contest; gói contest xuất **có kèm** danh sách người tham gia
- **When** xuất gói rồi nhập vào một bản cài trống
- **Then** vai tuỳ biến được dựng lại **kèm đúng tập permission của nó** · tài khoản mang vai đó ở bản đích làm được đúng những gì làm được ở bản nguồn

**AC-044 — Rejection: không thu hồi được quyền khi trận chưa đóng sổ**
- **US**: US-005 · **FR**: FR-030 · **GR**: `GR-026`
- **Given** contest#42 có một trận **đang chạy** ở giữa một vòng; tài khoản U được gán vai *Quản trị* phạm vi contest#42 và đang giữ quyền điều khiển; một câu đang mở, đồng hồ đang chạy
- **When** người quản trị thử **thu hồi phép gán vai** của U, rồi thử **gỡ `PERM-041` khỏi tập của vai**, rồi thử **vô hiệu hoá tài khoản** U
- **Then** cả **ba** thao tác đều **không dùng được** — hiển thị ở dạng invalid state, **không ép được**; yêu cầu gửi thẳng tới server cũng bị từ chối · quyền của U **không đổi**, U vẫn chấm được câu đang mở · trận **không kẹt** · điểm và đồng hồ **không đổi**

**AC-045 — Boundary: LOBBY vẫn khoá, FINISHED thì mở**
- **US**: US-005 · **FR**: FR-030
- **Given** cùng tài khoản U ở AC-044; trận đã về `STATE-001` LOBBY giữa hai vòng, **chưa chốt**
- **When** người quản trị thử thu hồi phép gán vai của U; sau đó chốt trận cho tới `STATE-008` FINISHED rồi thử lại
- **Then** lần đầu **bị chặn** — LOBBY vẫn là trận chưa đóng sổ · lần thứ hai **đi qua** · sau khi thu hồi, U mất quyền **ngay**, không phải chờ đăng nhập lại

**AC-046 — Boundary: phạm vi phép gán quyết định trận nào khoá**
- **US**: US-005 · **FR**: FR-030, FR-032
- **Given** contest#42 có một trận **đang chạy**; contest#77 **không** có trận nào chạy; tài khoản V được gán vai phạm vi **contest#77**; tài khoản W được gán vai phạm vi **HỆ THỐNG**
- **When** người quản trị thu hồi phép gán của V, rồi thu hồi phép gán của W
- **Then** thu hồi của **V đi qua** — phép gán của V không dính trận nào đang chạy · thu hồi của **W bị chặn** — phép gán phạm vi hệ thống chạm mọi contest, gồm cả contest#42 đang có trận

**AC-047 — No-change guarantee: cấp thêm quyền không bị luật này chặn**
- **US**: US-005 · **FR**: FR-031
- **Given** một trận **đang chạy**; tài khoản U đang giữ quyền điều khiển với một vai **không** chứa `PERM-033 round.skip`
- **When** người quản trị **thêm** `PERM-033` vào tập permission của vai đó
- **Then** thao tác **đi qua** — luật *"quyền không co lại"* là **một chiều**, chỉ chặn chiều giảm · U bỏ vòng được **ngay**, không phải đăng nhập lại · không quyền nào khác của U bị đổi

**AC-048 — Rejection: MC không duyệt được tín hiệu của thí sinh**
- **US**: US-005, US-004 · **FR**: FR-033 · **GR**: `GR-032`
- **Given** một trận đang ở vòng VCNV; hàng đợi có một tín hiệu chọn hàng ngang **đang chờ duyệt**; một phiên MC đã đăng nhập
- **When** MC gửi một yêu cầu duyệt tín hiệu đó tới server; sau đó lần lượt gửi thêm một yêu cầu **phán quyết Đúng/Sai** và một yêu cầu **mở đáp án**
- **Then** server **từ chối cả ba** — vai MC không giữ permission nào trong số đó, và việc từ chối MUST đến từ cửa kiểm **permission** chứ không từ việc giao diện có render nút hay không · tín hiệu **vẫn ở trạng thái chờ duyệt**, không đổi vị trí trong hàng đợi · thí sinh **không mất lượt** · điểm và trạng thái câu **không đổi** · quyền duy nhất của MC *(duyệt cú giành quyền)* **không đổi**

**AC-058 — Boundary: FR-030 không chạm tài khoản thí sinh**
- **US**: US-005 · **FR**: FR-030, FR-030b
- **Given** contest#42 có một trận **đang chạy** ở giữa một vòng, câu đang mở và đồng hồ đang chạy; tài khoản U mang vai **Thí sinh** và đang ngồi một ghế của trận đó; tài khoản A đang giữ quyền điều khiển
- **When** người quản trị thu hồi phép gán vai của U, rồi vô hiệu hoá tài khoản U; sau đó thử vô hiệu hoá tài khoản A
- **Then** cả hai thao tác trên **U đi qua** — trận không cần quyền của U để chạy tiếp · thao tác trên **A bị chặn** theo FR-030 · điểm của ghế U **không đổi** và vẫn trên mọi bảng · trận **không kẹt**, admin vẫn chấm được câu đang mở · đồng hồ **không đổi**

### US-006 — Cổng khán giả

**AC-049 — Happy path: khoá cổng không đuổi người đang xem**
- **US**: US-006 · **FR**: FR-035
- **Given** một trận đang chạy; ba phiên khán giả đang xem; cổng **chưa** khoá
- **When** admin bấm **khoá cổng**
- **Then** ba phiên đang xem **vẫn nhận cập nhật bình thường** · một phiên khán giả **mới** nhập đúng mã phòng thì **không vào được** · trạng thái trận **không đổi**

**AC-050 — Repeated action: mở lại cổng**
- **US**: US-006 · **FR**: FR-035
- **Given** cổng đang bị khoá; một phiên khán giả mới vừa bị từ chối
- **When** admin mở lại cổng và phiên đó thử lại
- **Then** phiên vào được · các phiên đang xem **không** bị gián đoạn bởi cả hai thao tác

**AC-051 — Boundary: ngưỡng tần suất là cấu hình, không phải hằng số**
- **US**: US-006 · **FR**: FR-034
- **Given** một bản cài với ngưỡng tần suất vào cổng khán giả đặt ở một giá trị X
- **When** người vận hành đổi ngưỡng sang giá trị Y qua cấu hình triển khai, không sửa mã nguồn
- **Then** cổng áp ngưỡng Y ở lần vào tiếp theo · **không** có rule, transition hay đặc tả nào của trận đọc con số này *(`PRD-REQ-087`)*

**AC-059 — Boundary: khoá cổng không chạm lớp phủ**
- **US**: US-006 · **FR**: FR-035
- **Given** một trận đang chạy; cổng khán giả **đang khoá**; một máy dựng stream đang hiển thị lớp phủ
- **When** lớp phủ **mất kết nối rồi mở lại** đúng đường dẫn của nó, và song song đó một phiên khán giả mới thử vào
- **Then** lớp phủ **vào lại được** và tiếp tục nhận cập nhật · phiên khán giả mới **bị từ chối** · trạng thái trận **không đổi** · không thao tác mở cổng nào phải thực hiện để lớp phủ vào lại

### US-007 — Tài khoản admin đầu tiên

**AC-052 — Happy path: đường dòng lệnh trên bản cài trống**
- **US**: US-007 · **FR**: FR-036
- **Given** một bản cài **trống**: không tài khoản nào, không contest nào
- **When** người dựng máy tạo tài khoản admin đầu tiên bằng đường dòng lệnh
- **Then** tài khoản đó đăng nhập được và mang vai admin · không tài khoản nào khác được tạo kèm

**AC-053 — Happy path: đường gói contest, không mở dòng lệnh**
- **US**: US-007 · **FR**: FR-037
- **Given** một bản cài **trống**; một gói contest **có kèm danh sách người tham gia**, trong đó có tài khoản admin của contest; người nhập biết cụm mật khẩu đúng
- **When** nhập gói đó
- **Then** tài khoản admin của contest được dựng lại và **đăng nhập được ngay** · **không** cần thao tác dòng lệnh nào · mọi vai khác trong danh sách cũng đăng nhập được

**AC-054 — Rejection: gói không kèm danh sách trên bản cài trống**
- **US**: US-007 · **FR**: FR-036, FR-037
- **Given** một bản cài **trống**; một gói contest **không** kèm danh sách người tham gia
- **When** nhập gói đó
- **Then** contest được dựng nhưng **không tài khoản nào** được tạo · vẫn phải dùng đường dòng lệnh để có admin đầu tiên — đây là ca đã biết và tài liệu vận hành phải nêu thẳng *(`PRD-REQ-106` §Note)*

**AC-055 — Ràng buộc: tài khoản admin trong gói luôn được cấp mật khẩu mới**
- **US**: US-007 · **FR**: FR-038
- **Given** người xuất chọn phương án **giữ mật khẩu hiện tại** cho danh sách người tham gia
- **When** gói được xuất rồi nhập ở bản đích
- **Then** các ghế thí sinh và MC được dựng theo lựa chọn của người xuất · **riêng tài khoản admin** vẫn được cấp **mật khẩu mới** kèm phiếu tài khoản in được · dấu vết mật khẩu admin **không** rời hệ thống nguồn

---

## 4. Functional Requirements *(mandatory)*

> Mỗi FR là một phát biểu kiểm thử được, tham chiếu US-NNN, PRD-REQ-NNN, GR-NNN và AC-NNN. Không FR nào nêu framework, cơ sở dữ liệu, đường API, lớp, hàm hay thuật toán.

### Nhóm A — Xác thực và kiểm quyền ở server

- **FR-001**: Hệ thống MUST yêu cầu tài khoản đã xác thực cho bốn vai **thí sinh, MC, admin, người ra đề**; hai vai **khán giả** và **máy dựng stream** MUST NOT cần tài khoản. · US-001 · `PRD-REQ-002` · `GR-037` · AC-001, AC-004, AC-005
- **FR-002**: Server MUST đánh giá **mọi** yêu cầu — REST lẫn sự kiện socket — theo đúng thứ tự sau, và MUST dừng ở bước đầu tiên không đạt:
  1. **Phiên đã xác thực chưa.** Chưa ⇒ từ chối. Việc phiên đó đang mở đúng đường dẫn phòng, đã nhận mã phòng hợp lệ, hay đang xem kênh công khai đều **không** thay thế bước này.
  2. **Yêu cầu đòi permission nào.** Server tra permission tương ứng với chính thao tác được yêu cầu.
  3. **Tài khoản của phiên có giữ permission đó trong đúng phạm vi của đối tượng bị tác động không** *(phạm vi `HỆ THỐNG`, hoặc đúng contest chứa đối tượng)*. Không giữ ⇒ từ chối.
  4. **Thao tác có đòi quyền điều khiển không.** Có, mà phiên không phải phiên đang giữ ⇒ từ chối *(FR-015)*.
  5. **Trạng thái đích có cho phép thao tác không** *(ví dụ trận đã niêm phong ở `STATE-008` FINISHED)*. Không ⇒ từ chối, và bước này **không ép qua được** kể cả khi bước 3 đạt.
  6. Mọi bước đạt ⇒ thực hiện, và mọi input MUST được validate lại ở server.

  Cửa kiểm này MUST độc lập hoàn toàn với giao diện: việc màn hình của một vai không render nút tương ứng MUST NOT là một lớp bảo vệ, và việc nó có render nút MUST NOT làm cửa nới ra. · US-001 · `PRD-REQ-002` · `GR-037` · AC-001, AC-002, AC-003, AC-004
- **FR-003**: Yêu cầu bị từ chối ở bất kỳ bước nào của FR-002 MUST là **không tác dụng phụ** — MUST NOT tạo, sửa hay xoá bất kỳ dữ liệu nào: nhật ký sự kiện trận giữ nguyên số mục, điểm mọi ghế giữ nguyên, trạng thái câu và trạng thái vòng giữ nguyên, đồng hồ **không** dừng và **không** đặt lại, vai và tập permission của tài khoản gửi yêu cầu **không đổi**. · US-001 · `PRD-REQ-002` · `GR-037` · AC-002, AC-003
- **FR-004**: Hệ thống MUST ghi lại mỗi lần đăng nhập, cả thành công lẫn thất bại. · US-001 · `PRD-REQ-002` · — · AC-005

### Nhóm B — Truy cập công khai và phạm vi hiển thị đáp án

- **FR-005**: Khán giả và máy dựng stream MUST vào được phòng bằng **đúng một URL**, với mã phòng nằm trong URL — không tài khoản, không vai, không bước nhập mã, không bước chờ duyệt. · US-002 · `PRD-REQ-001` · `GR-037` · AC-006, AC-007
- **FR-006**: Màn khán giả và lớp phủ MUST nhận cập nhật qua kênh **một chiều server → client**, cộng một lời gọi đọc để lấy ảnh chụp trạng thái lúc vào phòng; hai kênh này MUST NOT có đường gửi sự kiện lên server. · US-002 · `PRD-REQ-107` · `GR-037` · AC-007, AC-008
- **FR-007**: Kênh hai chiều MUST chỉ dành cho vai đã xác thực — admin, thí sinh, MC. · US-002 · `PRD-REQ-107` · `GR-037` · AC-008, AC-009
- **FR-008**: Đáp án chuẩn của một câu MUST rời server **chỉ** theo bảng quyết định dưới đây. Cửa kiểm MUST hỏi **permission đọc đáp án của câu đang chạy** *(`PERM-045`)* và MUST NOT hỏi tên vai, MUST NOT hỏi việc phiên đó có đang giữ quyền điều khiển hay không. Thứ tự đánh giá:
  1. **Phiên có giữ `PERM-045` trong phạm vi contest của trận không.** Có ⇒ trả đáp án, ghi một dòng nhật ký cho lần xem, dừng tại đây — không phụ thuộc cờ nào và không phụ thuộc mốc nào.
  2. Không giữ ⇒ hỏi **đáp án này có thuộc Chướng ngại vật không**. Có ⇒ nằm ngoài rule này *(thời điểm lộ do `GR-012` quy định; khi lộ thì tới cả khán giả lẫn lớp phủ)*.
  3. Hỏi **cờ `revealAnswerAfterJudge` của trận có BẬT không**. TẮT ⇒ không trả.
  4. Hỏi **câu đã tới mốc CÂU KHÉP chưa**. Chưa ⇒ không trả.
  5. Hỏi **câu có khép bằng phán quyết *Huỷ kết quả* không**. Có ⇒ không tự công bố.
  6. Hỏi **admin có đang giữ một cú ĐÓNG hiển thị bằng tay cho chính câu này không**. Có ⇒ không trả, tới khi admin tự mở lại; hiệu lực ở phạm vi một câu.
  7. Mọi bước trên đạt ⇒ công bố tới thí sinh, khán giả và lớp phủ.

  **Mốc CÂU KHÉP** của bước 4 xác định theo vòng: Khởi động lượt riêng, VCNV và Tăng tốc ⇒ khi admin chấm xong; Khởi động lượt chung ⇒ khi admin chấm xong **hoặc** hết cửa sổ chuông mà không ai bấm; **Về đích** ⇒ khi người thi chính được chấm **Đúng**, **hoặc** cửa sổ cướp quyền đã đóng **và** người cướp đã được chấm, **hoặc** hết cửa sổ cướp mà không ai bấm. Khi admin còn có thể **kích hoạt tay** một tín hiệu khác sau một cú *Huỷ kết quả*, mốc câu khép **lùi** tới sau khi người được kích hoạt đã được chấm.

  **Bảng quyết định hiệu lực**

  | Điều kiện | Kết quả |
  |---|---|
  | Phiên giữ `PERM-045` | Trả đáp án cho **riêng phiên đó**; ghi một dòng nhật ký cho lần xem. Không kèm quyền công bố cho ai khác |
  | Không giữ `PERM-045`, cờ reveal **TẮT** | Không trả |
  | Không giữ `PERM-045`, cờ **BẬT**, câu **chưa khép** | Không trả |
  | Không giữ `PERM-045`, cờ **BẬT**, câu **đã khép** *(gồm cả câu bị bỏ qua vì không ai bấm chuông)* | Công bố tới thí sinh, khán giả và lớp phủ |
  | Về đích — người thi chính vừa bị chấm **Sai**, cửa sổ cướp quyền **đang mở** | Không trả — câu chưa khép |
  | Đang chờ admin **kích hoạt tay** một tín hiệu khác sau một cú *Huỷ kết quả* | Không trả — mốc câu khép đã lùi |
  | Câu khép bằng phán quyết **Huỷ kết quả** | Không tự công bố; chỉ ra khi admin mở tay |
  | Câu khép, cờ **BẬT**, nhưng admin đang giữ một cú **đóng hiển thị bằng tay** cho câu này | Không trả, tới khi admin tự mở lại |
  | Đáp án **Chướng ngại vật** | Ngoài rule này về **thời điểm**; người nhận không đổi |

  Mọi ca **không trả** MUST là im lặng ở phía truyền: server MUST NOT phát nội dung đáp án, và MUST NOT phát bất kỳ thông báo nào để lộ nội dung đó. · US-002 · `PRD-REQ-001`, `PRD-REQ-002`, `PRD-REQ-113` · `GR-037`, `PERM-045` · AC-010…AC-017, AC-017a
- **FR-009**: Server MUST đẩy đáp án **chỉ tại đúng mốc câu khép**; MUST NOT đẩy sớm xuống client rồi dựa vào một cờ hiển thị phía client để giấu. · US-002 · `PRD-REQ-002` · `GR-037` §Cấm · AC-014

### Nhóm C — Vai hệ thống, vai vận hành, ràng buộc loại trừ

- **FR-010**: Mỗi tài khoản MUST mang **đúng một** vai; mọi phép gán của một tài khoản MUST mang **cùng một vai**. Hệ thống MUST NOT có đường nào dựng được một tài khoản giữ hai vai. · US-003 · `PRD-REQ-003` · `GR-037` · AC-018, AC-022
- **FR-011**: Tổ hợp *ngồi ghế thí sinh* và *giữ vai admin / MC / người ra đề* MUST là bất khả thi **do cấu trúc** — hệ quả trực tiếp của FR-010, không phải một phép kiểm riêng ở cửa gán. Điều này MUST đúng theo **cả hai chiều** và ở **mọi phạm vi**. · US-003 · `PRD-REQ-003` · `GR-037` · AC-019
- **FR-012**: Một tài khoản MUST gán được vai của mình ở **nhiều phạm vi** — cùng một MC dẫn được nhiều contest. · US-003 · `PRD-REQ-003`, `PRD-REQ-110` · — · AC-020
- **FR-013**: **MC**, **người phụ trách luyện tập** và **host** MUST là quyền gán theo từng contest, MUST NOT là vai seed của hệ thống; quyền gán ở contest A MUST NOT có hiệu lực ở contest B. · US-003 · `PRD-REQ-006` · — · AC-021, AC-022

### Nhóm D — Phiên giữ quyền điều khiển

- **FR-014**: Một contest MUST gán được vai admin cho **nhiều tài khoản**. · US-004 · `PRD-REQ-004` · `GR-026` · AC-023
- **FR-015**: Tại một thời điểm, MUST có **nhiều nhất một phiên** giữ quyền ghi trên một trận; các phiên admin khác MUST thấy cùng bề mặt hiển thị nhưng MUST NOT thực hiện được thao tác ghi nào. Quyền điều khiển MUST được xác lập khi **phiên admin đầu tiên** vào trận, và holder **đăng xuất chủ động** MUST nhả quyền — để lại trạng thái quyền **TRỐNG**. · US-004 · `PRD-REQ-004`, `QĐ-097` · `GR-026` · AC-023, AC-026, AC-056
- **FR-016**: Hệ thống MUST có thao tác **chuyển quyền điều khiển** giữa hai phiên admin. · US-004 · `PRD-REQ-004` · `GR-026` · AC-024, AC-027
- **FR-017**: Mỗi lần chuyển quyền điều khiển MUST được ghi vào nhật ký. · US-004 · `PRD-REQ-004` · — · AC-024, AC-027
- **FR-018**: Mỗi sự kiện trận MUST mang đúng **một** người thực hiện, là phiên đang giữ quyền tại thời điểm bấm; sự kiện đã ghi MUST NOT bị viết lại khi quyền chuyển sang phiên khác. · US-004 · `PRD-REQ-004` · `GR-026` · AC-025
- **FR-019**: Khi phiên giữ quyền mất kết nối, hệ thống MUST NOT đóng băng đồng hồ, MUST NOT tự tạm dừng trận, và MUST NOT có vai dự phòng nhận quyền tự động; phiên quay lại MUST khôi phục trạng thái từ server bằng cùng cơ chế của một client thường. · US-004 · `PRD-REQ-004` · — · AC-028
- **FR-020**: Một phiên admin khác MUST **giành** được quyền điều khiển khi phiên đang giữ **mất kết nối**, không cần phiên đó đồng ý. Khi phiên đang giữ **còn kết nối**, thao tác giành MUST NOT tồn tại — nút không bật và server từ chối; đổi người khi đó chỉ đi qua FR-016. · US-004 · `PRD-REQ-108` · `GR-036` · AC-029, AC-030
- **FR-020b**: Trạng thái quyền **TRỐNG** MUST NOT đi qua đường giành của FR-020 và FR-021: bất kỳ phiên admin nào của contest MUST nhận được quyền **ngay**, MUST NOT chờ MC duyệt và MUST NOT sinh dialog xác nhận — trống không phải giành, không có phiên nào để bảo vệ. Mỗi lần nhận quyền từ trạng thái trống MUST được ghi vào nhật ký. Nhiều phiên admin cùng nhận trong một cửa sổ ⇒ server MUST phân xử theo đúng hai bước: **(1)** phiên có **server timestamp sớm hơn** thắng; **(2)** chỉ khi hai timestamp **bằng nhau ở mili-giây**, server **bốc ngẫu nhiên**, và kết quả bốc MUST được ghi thành một **sự kiện** mang nhãn chỉ rõ thắng do bốc, để phát lại trận vẫn tất định. Ở đường nhận quyền từ trạng thái TRỐNG, việc một phiên là **chủ contest** MUST NOT cho phiên đó bất kỳ ưu tiên nào. · US-004 · `PRD-REQ-004`, `PRD-REQ-108`, `QĐ-097`, `QĐ-103` · `GR-026` · AC-056, AC-062
- **FR-021**: Khi contest **có MC**, cú giành MUST chờ **MC duyệt** qua một prompt phía MC mà **MC không tự tắt được** — chỉ server đóng nó, và chỉ khi cú giành đã được phân giải. Điều kiện *"có MC"* MUST đo bằng **có phiên MC đang kết nối tại thời điểm cú giành**, MUST NOT đo bằng việc contest có phép gán vai MC *(`QĐ-098`)*. Đây MUST là bề mặt quyền ghi **duy nhất** của MC; MC MUST NOT duyệt tín hiệu của thí sinh, MUST NOT phán quyết Đúng/Sai và MUST NOT mở đáp án. MC từ chối ⇒ quyền ở nguyên chỗ cũ; phiên đang giữ kết nối lại trước khi MC quyết ⇒ cú giành **mất đối tượng**. · US-004 · `PRD-REQ-108`, `PRD-REQ-074` *(mệnh đề quyền ghi)* · `GR-026` · AC-029, AC-031, AC-032
- **FR-021a**: Khi có **nhiều phiên MC** đang kết nối, prompt MUST lên **tất cả**, và **bất kỳ phiên nào trong số đó** MUST quyết được — không có phiên MC chính, không có thứ tự ưu tiên. Quyết định **tới server đầu tiên** theo server timestamp MUST là quyết định có hiệu lực, **bất kể** nó là Duyệt hay Từ chối; server MUST đóng prompt ở mọi phiên còn lại. Quyết định tới **sau** MUST bị từ chối và MUST NOT lật kết quả đã phân giải. Mọi quyết định tới sau MUST vẫn được ghi vào nhật ký kèm trạng thái bị từ chối. · US-004 · `PRD-REQ-108`, `QĐ-100` · `GR-026` · AC-060, AC-061
- **FR-021b**: Hệ thống MUST NOT đặt ngưỡng thời gian cho prompt duyệt cú giành. Khi **phiên MC cuối cùng còn kết nối** mất kết nối trong lúc prompt đang chờ, cú giành MUST rơi về nhánh không-MC của FR-022 và có hiệu lực **ngay**, kèm một dòng nhật ký — sao cho không tồn tại trạng thái cú giành chờ vô hạn mà không phiên nào trên hệ thống giải được. Còn **ít nhất một** phiên MC kết nối ⇒ prompt MUST tiếp tục chờ. · US-004 · `PRD-REQ-108`, `QĐ-099` · `GR-036` · AC-057, AC-061
- **FR-022**: Khi contest **không có MC** *(không phiên MC nào đang kết nối)*, cú giành MUST có hiệu lực **ngay**, không dialog xác nhận, và MUST NOT phát bất kỳ thông báo nào tới thí sinh, khán giả hoặc lớp phủ. Nhiều phiên cùng giành ⇒ **chủ contest** MUST thắng; ngoài ra phân xử theo server timestamp. · US-004 · `PRD-REQ-108` · `GR-026` · AC-030, AC-033, AC-057
- **FR-023**: Trong lúc một cú giành đang chờ MC quyết, hệ thống MUST NOT dừng đồng hồ và MUST NOT tạm dừng trận. Mọi cú giành, mọi lần MC duyệt và mọi lần MC từ chối MUST được ghi vào nhật ký — kể cả cú giành bị từ chối và cú giành mất đối tượng. · US-004 · `PRD-REQ-108` · — · AC-031, AC-032, AC-034

### Nhóm E — Permission cho thao tác phá huỷ

- **FR-024**: Bảy thao tác phá huỷ — **bỏ vòng** *(`PERM-033`)* · **chạy lại vòng** *(`PERM-034`)* · **kết thúc vòng khẩn cấp** *(`PERM-035`)* · **huỷ trận** *(`PERM-037`)* · **điều chỉnh điểm** *(`PERM-043`)* · **sửa danh sách đề** *(`PERM-026`)* · **ép qua hàng rào đề đã từng public** *(`PERM-027`)* — MUST mỗi thứ là một permission riêng đứng độc lập. Giữ một trong bảy permission này MUST NOT suy ra bất kỳ permission nào trong sáu cái còn lại, và MUST NOT suy ra từ việc một tài khoản mang vai admin hay đang giữ quyền điều khiển. · US-005 · `PRD-REQ-005` · `GR-029`, `GR-030` · AC-035, AC-036, AC-037
- **FR-025**: Một yêu cầu thực hiện thao tác phá huỷ MUST được đánh giá theo thứ tự: **(1)** trạng thái trận có cho phép thao tác không — trận đã niêm phong ở `STATE-008` FINISHED thì **không**, và ca này MUST NOT ép qua được kể cả với đủ permission; **(2)** phiên có giữ đúng permission của **chính thao tác đó** trong phạm vi contest của trận không; **(3)** thao tác có đòi quyền điều khiển không. Thiếu ở bất kỳ bước nào ⇒ **từ chối, không tác dụng phụ** theo FR-003. Song song đó, khi phiên thiếu permission, giao diện MUST hiển thị thao tác ở trạng thái không dùng được — nhưng đó là chỉ dẫn cho người dùng, MUST NOT là lớp cưỡng chế: một yêu cầu gửi thẳng tới server bằng đường khác MUST vẫn bị từ chối ở đúng cửa kiểm trên. · US-005 · `PRD-REQ-005` · `GR-029`, `GR-030` · AC-036, AC-038
- **FR-026**: Ở cửa kiểm quyền, server MUST quyết dựa trên việc tài khoản **có giữ một permission cụ thể trong một phạm vi cụ thể hay không**, và MUST NOT quyết dựa trên **tên vai** mà tài khoản mang. · US-005 · `PRD-REQ-109` · — · AC-039, AC-040
- **FR-027**: Permission MUST tới người dùng **chỉ qua vai**; hệ thống MUST NOT có đường cấp một permission thẳng cho một tài khoản. Đổi tập permission của một vai MUST đổi ngay quyền của mọi tài khoản đang mang vai đó. · US-005 · `PRD-REQ-109` · — · AC-040
- **FR-028**: Mỗi phép gán vai MUST mang một phạm vi — **toàn hệ thống** hoặc **đúng một contest**. Vai gán ở contest A MUST NOT có hiệu lực ở contest B. Cùng một vai MUST gán được ở cả hai phạm vi khi tập permission của nó có nghĩa ở đó. · US-005, US-003 · `PRD-REQ-110` · — · AC-021, AC-041
- **FR-029**: Người vận hành MUST định nghĩa được **vai tuỳ biến** với tập permission tự chọn. Bốn vai dựng sẵn MUST đủ để chạy trọn một trận mà không phải định nghĩa thêm vai nào. Vai tuỳ biến MUST được xuất kèm **định nghĩa của nó** trong gói contest. · US-005 · `PRD-REQ-111` · — · AC-042, AC-043
- **FR-030**: Trong lúc một trận **chưa đóng sổ**, hệ thống MUST NOT cho thực hiện thao tác quản trị nào làm **giảm** quyền hiệu dụng của một tài khoản mà **trận cần quyền của nó để chạy tiếp** — gỡ permission khỏi tập của một vai, thu hồi phép gán vai, hoặc vô hiệu hoá tài khoản. Tập tài khoản được bảo vệ MUST gồm đúng: phiên **đang giữ** quyền điều khiển, phiên admin **có thể nhận** quyền điều khiển, và **MC** *(người duyệt cú giành)*. Thao tác bị chặn MUST hiển thị ở dạng không dùng được và MUST NOT ép qua được. · US-005 · `PRD-REQ-112`, `QĐ-101` · `GR-026` · AC-044, AC-045, AC-046
- **FR-030b**: Thu hồi phép gán vai hoặc vô hiệu hoá một **tài khoản thí sinh** MUST vẫn thực hiện được trong lúc trận chưa đóng sổ — trận không cần quyền của tài khoản đó để chạy tiếp, nên không đường nào của luật chơi bị khoá chết. Van thoát để cắt một **ghế** giữa trận là **vô hiệu hoá ghế**, thuộc EPIC-006 và EPIC-008 *(xem §8)*, không phải vô hiệu hoá tài khoản. · US-005 · `PRD-REQ-112`, `QĐ-101` · — · AC-058
- **FR-031**: Thao tác **cấp thêm** quyền MUST vẫn thực hiện được trong lúc trận chưa đóng sổ và MUST có hiệu lực ngay. Ngoài phạm vi một trận chưa đóng sổ, thu hồi quyền MUST có hiệu lực ngay với cả phiên đang mở. · US-005 · `PRD-REQ-112` · — · AC-040, AC-047
- **FR-032**: Trận bị khoá bởi FR-030 MUST xác định theo **phạm vi** của phép gán: phép gán ở một contest khoá theo trận của contest đó; phép gán ở phạm vi toàn hệ thống khoá theo **bất kỳ** trận nào chưa đóng sổ. · US-005 · `PRD-REQ-112`, `PRD-REQ-110` · — · AC-046
- **FR-033**: Vai **MC** dựng sẵn MUST giữ đúng **một** permission ghi — duyệt cú giành quyền điều khiển — và MUST NOT giữ bất kỳ permission ghi nào khác của trận, đặc biệt là duyệt tín hiệu của thí sinh. Server MUST từ chối **mọi** sự kiện ghi khác đến từ vai này, bất kể giao diện có render nút hay không. · US-005, US-004 · `PRD-REQ-109`, `PRD-REQ-074` *(mệnh đề quyền ghi)* · `GR-032` · AC-048

### Nhóm F — Cổng khán giả

- **FR-034**: Cổng vào của khán giả MUST có giới hạn tần suất **cấu hình được ở mức triển khai**; MUST NOT có rule, transition hay đặc tả nào của trận đọc con số này. · US-006 · `PRD-REQ-086` · — · AC-051
- **FR-035**: Admin MUST có thao tác **khoá cổng** phòng; khi cổng bị khoá, phiên khán giả **mới** MUST không vào được và phiên **đang xem** MUST NOT bị ngắt. Khoá cổng MUST áp **chỉ cho màn khán giả**; **lớp phủ** MUST vào được kể cả khi cổng đang khoá, và một lần lớp phủ kết nối lại giữa buổi phát sóng MUST NOT bị khoá cổng chặn. · US-006 · `PRD-REQ-086`, `QĐ-102` · — · AC-049, AC-050, AC-059

### Nhóm G — Tài khoản admin đầu tiên

- **FR-036**: Hệ thống MUST tạo được tài khoản admin đầu tiên trên một bản cài trống bằng **đường dòng lệnh**. · US-007 · `PRD-REQ-106` · — · AC-052, AC-054
- **FR-037**: Nhập một gói contest **có kèm danh sách người tham gia** MUST dựng lại được tài khoản admin của contest đó, sao cho bản đích dùng được ngay sau khi nhập mà **không** cần thao tác dòng lệnh nào. · US-007 · `PRD-REQ-106` · — · AC-053, AC-054
- **FR-038**: Tài khoản admin đi trong gói contest MUST luôn được cấp **mật khẩu mới lúc nhập** kèm phiếu tài khoản in được, kể cả khi người xuất chọn phương án giữ mật khẩu hiện tại cho các tài khoản khác. · US-007 · `PRD-REQ-106` · — · AC-055
- **FR-039**: v1 MUST NOT có trình hướng dẫn cài đặt lần đầu chạy trên trình duyệt. · US-007 · `PRD-REQ-106` *(NON-GOAL-019)* · — · AC-052, AC-053

---

## 5. Key Entities

- **Tài khoản** (`TERM-008`): danh tính xác thực được. Mang **đúng một vai** *(`QĐ-065`)*, gán được ở nhiều phạm vi. Ràng buộc loại trừ vì thế là tính chất **cấu trúc**, không phải một phép kiểm.
- **Vai** (`TERM-062`): một **tập permission có tên**. Bốn vai dựng sẵn — Quản trị, Người ra đề, MC, Thí sinh — cộng vai tuỳ biến.
- **Phép gán**: bộ ba *(tài khoản, vai, phạm vi)*, phạm vi ∈ {`HỆ THỐNG`, một contest}. *Vai hệ thống* và *vai vận hành* của `PRD-REQ-006` là **hai phạm vi gán của cùng một cơ chế**, **không** phải hai loại vai *(`QĐ-094`)*.
- **Permission** (`TERM-061`): đơn vị quyền nhỏ nhất, và là **thứ duy nhất server kiểm**. Bảy thao tác phá huỷ mỗi thứ có một permission riêng.
- **Phiên** (`QĐ-008`): một lần đăng nhập đang hoạt động. **Quyền điều khiển gắn với phiên, không với tài khoản.**
- **Quyền điều khiển**: thuộc tính nhiều-nhất-một-giá-trị trên một trận, trỏ tới **một** phiên admin hoặc **TRỐNG**. Trống là trạng thái hợp lệ — trước khi phiên admin đầu tiên vào, và sau khi holder đăng xuất chủ động *(FR-015, FR-020b)*.
- **Mã phòng** (`TERM-007`): chuỗi 6 số, là toàn bộ điều kiện vào của hai kênh công khai.
- **Cờ `revealAnswerAfterJudge`**: cờ **cấp trận**, mặc định BẬT, chụp vào trận lúc bắt đầu; đầu vào của `GR-037`.
- **Mốc câu khép** (`TERM-057`): thời điểm không còn ai được trả lời một câu nữa; là mốc công bố đáp án cho thí sinh, khán giả và lớp phủ.

---

## 6. Success Criteria *(mandatory)*

- **SC-001**: 100% yêu cầu ghi phát ra từ một phiên thiếu quyền bị từ chối ở server, kể cả khi phiên đó đã vào đúng phòng — đo bằng bộ kiểm chạy đủ ma trận *(6 vai × danh mục thao tác)*, tỉ lệ lọt = 0.
- **SC-002**: 0 trường hợp đáp án chuẩn rời server tới thí sinh, khán giả hoặc lớp phủ trước mốc câu khép — đo trên toàn bộ 9 ca của bảng quyết định `GR-037`, quan sát ở **phía truyền**, không phải phía hiển thị.
- **SC-003**: Khán giả không có tài khoản thấy được phòng trong **dưới 5 giây** kể từ lúc mở đường dẫn, với **0 thao tác** ngoài chính việc mở đường dẫn — không màn đăng nhập, không bước nhập mã, không bước chờ duyệt.
- **SC-004**: 0 trường hợp hai phiên admin cùng ghi thành công trên một trận — đo bằng bài kiểm gửi đồng thời từ hai phiên trên cùng một câu, lặp ít nhất 100 lần; mỗi câu vẫn đi qua đúng một phán quyết.
- **SC-005**: Chuyển quyền điều khiển từ phiên này sang phiên khác hoàn tất trong **dưới 10 giây** thao tác của người, và 100% lần chuyển có một dòng nhật ký tương ứng.
- **SC-006**: Cấp được một tài khoản chạy trọn một trận mà **không** thực hiện được bất kỳ thao tác nào trong bảy thao tác phá huỷ — nghiệm thu bằng một lần chạy trận đầy đủ với tài khoản đó.
- **SC-007**: Trên một bản cài trống, người vận hành đăng nhập điều khiển được sau khi nhập gói contest có kèm danh sách, với **0 lệnh dòng lệnh** phải gõ tại chỗ.
- **SC-008**: Khoá cổng khán giả có hiệu lực với phiên mới trong **dưới 5 giây**, và 100% phiên đang xem tiếp tục nhận cập nhật không gián đoạn.

---

## 7. Edge Cases

**Giá trị biên**
- Mã phòng đúng 6 ký tự số — độ dài khác hoặc ký tự khác số không phải mã hợp lệ.
- Cờ `revealAnswerAfterJudge` được **chụp vào trận lúc bắt đầu**: sửa cấu hình contest sau đó không đụng trận đang chạy — hai giá trị khác nhau cho hai trận cùng phòng là kịch bản hợp lệ.
- Đúng mốc câu khép: `GR-037` đặt mốc công bố **tại** câu khép, không sớm hơn; ở Về đích mốc này đến **sau** cú bấm Đúng/Sai của người thi chính.
- **Quyền điều khiển TRỐNG** là trạng thái hợp lệ, không phải lỗi: nó tồn tại trước khi phiên admin đầu tiên vào trận, và sau khi holder đăng xuất chủ động. Nó **không** đi qua đường giành quyền *(AC-056)*.
- **Ranh giới "contest có MC"** đo ở đúng thời điểm cú giành, bằng **phiên đang kết nối** — không bằng phép gán. Cùng một contest vì thế rẽ hai nhánh khác nhau ở hai thời điểm khác nhau *(AC-030, AC-031, AC-057)*.

**Trạng thái không hợp lệ**
- Thao tác phá huỷ khi trận ở `STATE-008` FINISHED: **invalid state**, không ép được, độc lập với việc tài khoản có đủ permission *(AC-038)*.
- Phiên chưa xác thực cố mở kênh hai chiều *(AC-009)*.
- Gán vai vi phạm ràng buộc loại trừ *(AC-019)*.

**Thao tác lặp**
- Đăng nhập sai nhiều lần rồi đúng *(AC-005)*.
- Gán lại một vai đã có *(AC-022)*.
- Bấm chuyển quyền điều khiển hai lần *(AC-027)*.
- Khoá cổng rồi mở lại *(AC-050)*.

**Trạng thái cũ / sự kiện trùng**
- Một phiên admin bị mất quyền điều khiển nhưng giao diện chưa kịp cập nhật, vẫn gửi một thao tác ghi: server MUST từ chối theo trạng thái quyền **hiện tại**, không theo trạng thái giao diện của phiên đó.
- Sự kiện chuyển quyền tới lần thứ hai do trễ đường truyền *(AC-027)*.
- Phiên MC mất kết nối trong lúc đang giữ prompt duyệt cú giành: cú giành **chuyển nhánh**, không treo chờ *(AC-057)*. Có nhiều phiên MC thì mốc chuyển nhánh là lúc **phiên cuối cùng** rớt *(AC-061)*.
- Hai phiên MC quyết **ngược nhau** gần như cùng lúc: server timestamp phân xử, **không** có luật "từ chối thắng" *(AC-060)*.
- Hai phiên admin cùng nhận một quyền **TRỐNG** với timestamp **bằng nhau ở mili-giây**: server bốc ngẫu nhiên, kết quả thành sự kiện *(AC-062, `QĐ-103`)*. Đây là chỗ **duy nhất** trong feature này mà máy dùng ngẫu nhiên — và nó phân xử **thứ tự**, không phân xử đúng/sai.

**Hỏng một phần**
- Nhập gói contest có kèm danh sách nhưng sai cụm mật khẩu hoặc gói bị sửa: **từ chối toàn bộ**, không tạo bản ghi tài khoản nào — không có trạng thái nhập nửa vời *(`NFR-24b`)*.
- Trùng tên đăng nhập ở bản đích: hệ thống **hỏi**, không tự nối *(`QĐ-086` vế 6)* — luồng chi tiết thuộc EPIC-003, xem §8.
- Server sập: **mất khả năng cứu** — đây là ranh giới đã chấp nhận *(`NFR-31`)*, không phải chỗ thiếu đặc tả.

**Hành vi nguồn không quy định** — spec này **không** tự điền **mã lỗi** cho từng ca từ chối; nguồn không định nghĩa mã lỗi nào, và spec không tạo mới.

---

## 8. Out of Scope

**Requirement thuộc epic khác, không đưa vào feature này**

| Ngoài phạm vi | Thuộc về |
|---|---|
| Vòng duyệt đề `DRAFT` → `ACTIVE`, cờ hiển thị dẫn xuất, dấu vết `everPublic` | EPIC-002 *(`PRD-REQ-011`…`014`)* |
| Xuất/nhập gói contest, mã hoá danh sách người tham gia, xử lý trùng tên đăng nhập, phiếu tài khoản | EPIC-003 *(`PRD-REQ-104`, `NFR-24b`)* — feature này chỉ dùng **kết quả** của nó ở FR-037, FR-038 |
| Gán ghế và vị trí, chọn mode trả lời, cấu hình luật | EPIC-004 |
| Vòng đời trận, mã phòng được **sinh ra** như thế nào, đóng băng cấu hình | EPIC-005 |
| Nội dung của bảy thao tác phá huỷ *(bỏ vòng làm gì, chạy lại làm gì)* và toàn bộ bảng quyết định `GR-029`, `GR-030` ngoài vế permission | EPIC-006, EPIC-007 |
| Phán quyết Đúng/Sai và bảng quyết định `GR-026` ngoài vế *"ai được bấm"* | EPIC-007 |
| Cơ chế **kích hoạt tay** một tín hiệu sau một cú *Huỷ kết quả* — tập ứng viên, ba tham số, giới hạn số lần | EPIC-006 *(`PRD-REQ-113`, `specs/006` FR-034a→FR-034e)* — feature này chỉ chạm hệ quả **mốc câu khép lùi** ở `GR-037` C8b *(FR-008, AC-017a)* |
| Màn khán giả và lớp phủ **trông như thế nào**; **cách render** màn MC chữ lớn — cỡ chữ, bố cục, việc nó hiển thị câu hỏi và đáp án | EPIC-009 *(`PRD-REQ-073`, `074`)*. ⚠️ **Mệnh đề quyền ghi** của `PRD-REQ-074` — *bề mặt ghi duy nhất của MC là prompt duyệt cú giành; server từ chối mọi sự kiện ghi khác từ vai này* — **thuộc feature này**, xem FR-021 và FR-033 |
| Nhật ký thao tác chung cho mọi vai, hạn lưu trữ, xuất biên bản trước khi dọn dữ liệu | EPIC-011 *(`PRD-REQ-081`…)* — feature này chỉ yêu cầu ghi nhật ký ở đúng ba chỗ mà `PRD-REQ-002`, `004` và `GR-037` tự đòi |
| Hai hồ sơ triển khai, giới hạn kích thước media | EPIC-012 *(`PRD-REQ-085`, `098`)* |
| Mất kết nối của **ghế thí sinh**, giữ ghế 120 giây, vô hiệu hoá ghế | EPIC-006, EPIC-008 *(`GR-036`)* — feature này chỉ chạm vế **admin** mất kết nối *(FR-019)* |

**Non-goal của chính EPIC-001** *(`docs/PRD.md` §11)*
- Nhiều tổ chức trên một bản cài — NON-GOAL-006.
- Vai người duyệt đề riêng — `QĐ-064`: admin duyệt, không có vai reviewer.

**Non-goal chung có chạm feature này**
- NON-GOAL-019 — trình hướng dẫn cài đặt lần đầu trên trình duyệt *(đã thành FR-039 ở dạng phủ định)*.
- NON-GOAL-005 — đa ngôn ngữ.

---

## 9. Open Questions

Không có. Hai chi tiết cố ý để ngoài spec — **mã lỗi** cho từng ca từ chối, và **cơ chế xác thực cụ thể** — nằm ở §11 Assumptions vì chúng là lựa chọn kỹ thuật, không phải requirement nghiệp vụ.

---

## 10. Traceability Matrix

| User story | PRD requirement | Game rules | Functional requirements | Acceptance scenarios |
|---|---|---|---|---|
| **US-001** — Đăng nhập và kiểm quyền ở server | `PRD-REQ-002` | `GR-037` | FR-001 → FR-004 | AC-001 → AC-005 |
| **US-002** — Vào phòng công khai bằng URL, không đường ghi | `PRD-REQ-001`, `PRD-REQ-107`, `PRD-REQ-113` *(vế mốc câu khép lùi)* | `GR-037` | FR-005 → FR-009 | AC-006 → AC-017, AC-017a, AC-017b |
| **US-003** — Mỗi tài khoản một vai, gán theo phạm vi | `PRD-REQ-003`, `PRD-REQ-006` | `GR-037` | FR-010 → FR-013, FR-028 | AC-018 → AC-022, AC-041 |
| **US-004** — Một phiên giữ quyền; chuyển giao và giành lại được | `PRD-REQ-004`, `PRD-REQ-108`, **`PRD-REQ-074`** *(mệnh đề quyền ghi)* | `GR-026`, `GR-036` | FR-014 → FR-023 *(gồm FR-020b, FR-021a, FR-021b)*, FR-033 | AC-023 → AC-034, AC-048, AC-056, AC-057, AC-060 → AC-062 |
| **US-005** — Phân quyền theo vai và permission cho thao tác phá huỷ | `PRD-REQ-005`, `109`, `110`, `111`, `112`, **`074`** *(mệnh đề quyền ghi)* | `GR-026`, `GR-029`, `GR-030`, `GR-032` | FR-024 → FR-033 *(gồm FR-030b)* | AC-035 → AC-048, AC-058 |
| **US-006** — Cổng khán giả | `PRD-REQ-086` | — | FR-034, FR-035 | AC-049 → AC-051, AC-059 |
| **US-007** — Tài khoản admin đầu tiên | `PRD-REQ-106` | — | FR-036 → FR-039 | AC-052 → AC-055 |

### Phủ bảng quyết định của game rule

| Rule · ca | Nội dung ca | Acceptance scenario |
|---|---|---|
| `GR-037` C1 | Admin ⇒ trả đáp án, ghi audit | AC-010 |
| `GR-037` C2 | MC ⇒ trả đáp án, ghi audit | AC-011 |
| `GR-037` C3 | Thí sinh/viewer/overlay, reveal TẮT ⇒ không trả | AC-013 |
| `GR-037` C4 | reveal BẬT, câu **chưa** khép ⇒ không trả | AC-014 |
| `GR-037` C5 | reveal BẬT, câu **đã** khép ⇒ trả | AC-015 |
| `GR-037` C6 | Về đích, cửa sổ cướp đang mở ⇒ không trả | AC-016 |
| `GR-037` C7 | Câu bị bỏ qua, reveal BẬT ⇒ trả | AC-015 |
| `GR-037` C8 | Câu khép bằng Huỷ kết quả ⇒ không tự trả | AC-017 |
| `GR-037` C8b | Đang chờ admin **kích hoạt tay** một tín hiệu khác ⇒ không trả, câu chưa khép | AC-017a |
| `GR-037` C10 | Admin đang giữ cú **đóng hiển thị bằng tay** ⇒ không trả dù cờ reveal BẬT | AC-017b |
| `GR-037` C9 | Đáp án Chướng ngại vật ⇒ ngoài rule này | AC-017 |
| `GR-026` §Đồng thời | Không có hai luồng thao tác admin song song | AC-026 |
| `GR-026` C1/C2 §"điểm chỉ chốt khi admin bấm" | Chỉ phiên giữ quyền mới sinh được sự kiện phán quyết | AC-025, AC-026 |
| `GR-029` C6 | Trận `FINISHED` ⇒ điều chỉnh điểm là invalid state, không ép được | AC-038 |
| `GR-030` §Điều kiện | Ba cửa ra thuộc hạng phá huỷ ⇒ cần permission riêng | AC-035, AC-036, AC-037 |

> **Ghi chú phủ có chủ đích.** Các ca còn lại của `GR-026` *(C3, C4, C5)*, `GR-029` *(C1…C5)* và `GR-030` *(C1…C5)* mô tả **hệ quả nghiệp vụ** của thao tác — điểm cộng bao nhiêu, event đảo ngược sinh ra sao. Chúng thuộc EPIC-006 và EPIC-007 *(xem §8)*, không phải EPIC-001. Feature này chỉ phủ vế **ai được phép bấm**, và đó là toàn bộ giao diện giữa EPIC-001 với ba rule đó. Ghi rõ ở đây để chỗ thiếu là một quyết định phạm vi, không phải một chỗ bỏ sót.

---

## 11. Assumptions

Các giá trị mặc định dưới đây được chọn cho **chi tiết không quan trọng** *(theo Nguyên tắc III)*; mọi thứ quan trọng mà nguồn không nói đều nằm ở §9.

- **Ngưỡng tần suất cụ thể của cổng khán giả** không được chốt trong spec này. `PRD-REQ-087` khai thẳng rằng không đặc tả nào được viết dựa trên một con số cụ thể, nên FR-034 chỉ yêu cầu *có ngưỡng cấu hình được*.
- **Cơ chế xác thực cụ thể** *(mật khẩu, thời hạn phiên, chính sách khoá tài khoản sau N lần sai)* không được nguồn nào quy định và **không** thuộc spec này — đây là lựa chọn kỹ thuật thuộc `plan.md`. FR-001 và FR-004 chỉ yêu cầu *có tài khoản* và *có ghi nhật ký đăng nhập*.
- **Nhật ký ở FR-004, FR-017 và ở `GR-037`** được hiểu là ghi vào cùng một nhật ký thao tác chung mà EPIC-011 sở hữu. Spec này không đặc tả cấu trúc nhật ký đó; nó chỉ yêu cầu **có dòng ghi** ở đúng ba chỗ mà nguồn của EPIC-001 tự đòi.
- **Mã phòng được sinh ra như thế nào, và đường dẫn phòng có hình dạng gì** thuộc EPIC-005. Spec này giả định đường dẫn đã tồn tại và hợp lệ khi khán giả mở nó.
- **Ràng buộc kỹ thuật toàn repo** — zero-trust, server-authoritative, chỉ tiếng Việt, múi giờ UTC+7, phản hồi tức thì cho thao tác bất đồng bộ — áp cho feature này theo `CLAUDE.md`; spec **tham chiếu, không sao chép**.
