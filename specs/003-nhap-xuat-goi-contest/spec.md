# Feature Specification: EPIC-003 — Nhập/xuất và gói contest

**Feature Directory**: `specs/003-nhap-xuat-goi-contest`

**Created**: 2026-07-30

**Status**: Draft

**Input**: User description: `/speckit.specify` — tạo feature specification cho **EPIC-003 — Nhập/xuất và gói contest**, chỉ dùng product requirement / user journey / game rule / state thuộc EPIC-003, không đưa công nghệ triển khai vào spec.

---

## 0. Nguồn đã đọc và ghi chú về nguồn

| Nguồn yêu cầu đọc | Trạng thái |
|---|---|
| `docs/PRD.md` | ✅ đã đọc — §7 Actors (`ACTOR-001` admin, `ACTOR-002` setter), §10 `JOURNEY-003`, `JOURNEY-007`, §11 EPIC-003, §12 EPIC-003 (`PRD-REQ-016`…`019`, `094`, `095`, `104`) cộng phần EPIC-003 của `PRD-REQ-111`, §14 FS-08, FS-08b, §19, §20 ma trận truy nguyên, §12 EPIC-010 *(để xác nhận `X2`/`X4` KHÔNG thuộc EPIC-003)* |
| `docs/glossary.md` | ✅ đã đọc — `TERM-048` (`usedInContest`), `TERM-049` (`everPublic`) |
| `docs/game-rules.md` | ✅ đã đọc — `GR-031` đầy đủ *(chỉ vế "cờ đi theo câu vs đi theo contest" và bảng quyết định thuộc feature này; phần rút đề/kiểm kho lúc chạy trận đã phủ ở spec EPIC-002 và thuộc EPIC-006)* |
| `docs/game-state-machine.md` | ✅ đã đọc §9 — xác nhận: **không** có `STATE-*`/`EVENT-*`/`T-*` nào thuộc trực tiếp EPIC-003; nhập/xuất là thao tác cấp **contest** hoặc cấp **hệ thống**, không phải một trong bảy thang bậc trạng thái của một trận |
| `docs/traceability.md` | ✅ đã đọc — bảng đối chiếu giá trị luật và biến thể bị loại |

Ngoài ra đã đọc `docs/decisions.md` (`QĐ-071`, `QĐ-084`, `QĐ-086`, `QĐ-087`) và `docs/product-discovery.md` §2 A-2, §4 J-3, §5 E-3 vì `docs/PRD.md` khai chúng là nguồn của các `PRD-REQ-*` thuộc EPIC-003.

---

## 1. Phạm vi feature

**Goal (nguyên văn EPIC-003)**: *"Soạn ở nơi có Internet, thi ở nơi không có."* — `docs/PRD.md` §11 EPIC-003.

**Product requirement trong phạm vi**:

| Requirement | Tiêu đề | Priority (PRD) |
|---|---|---|
| `PRD-REQ-016` | Xuất và nhập gói contest trọn vẹn | P1 |
| `PRD-REQ-017` | Cờ theo câu đi qua nhập/xuất, cờ theo contest thì đặt lại | P1 |
| `PRD-REQ-018` | Nhập/xuất phần đề của một người ra đề | P3 |
| `PRD-REQ-019` | Giới hạn kích thước media | P2 — *gán **EPIC-003, EPIC-012***, phần thuộc EPIC-012 (giá trị mặc định theo từng hồ sơ triển khai) nằm ở §8 |
| `PRD-REQ-094` | Đánh dấu "đã dùng" hàng loạt bằng tay | P2 |
| `PRD-REQ-095` | Xuất và nhập bản kê câu đã dùng | P2 |
| `PRD-REQ-104` | Gói contest mang danh sách người tham gia, luôn ở dạng đã mã hoá | P1 |
| `PRD-REQ-111` | Vai tuỳ biến do đơn vị tự định nghĩa | P2 — *gán **EPIC-001, EPIC-003***, chỉ vế **xuất kèm định nghĩa vai trong gói** thuộc feature này; vế **định nghĩa/tạo vai tuỳ biến** thuộc EPIC-001, xem §8 |

**Game rule trong phạm vi** (đúng những rule mà EPIC-003 và các `PRD-REQ-*` trên tham chiếu): `GR-031` *(rút đề và không lặp câu — chỉ vế "`everPublic` đi theo câu, `usedInContest` đi theo contest và đặt lại khi nhập vào contest khác", và bảng quyết định của cơ chế đánh dấu đã dùng hàng loạt/bản kê câu đã dùng theo `QĐ-084`)*.

**State và transition liên quan trực tiếp**: **không có.** Xuất/nhập gói, đánh dấu đã dùng hàng loạt, và xuất/nhập bản kê đều là thao tác cấp **contest** (đọc/ghi cấu hình và cờ) hoặc cấp **hệ thống** (tài khoản), không thuộc bảy thang bậc trạng thái của một trận (`TERM-018`). `PRD-REQ-104` có chạm `STATE-008` FINISHED một cách gián tiếp qua ghi chú của `QĐ-084` về `X2`/`X4`, nhưng hai gói đó **không** thuộc EPIC-003 — xem §8.

**Actor**: `ACTOR-001` admin *(chính — xuất/nhập gói contest, đánh dấu hàng loạt, xuất/nhập bản kê, quản lý danh sách người tham gia)* · `ACTOR-002` setter *(nhập/xuất phần đề mình phụ trách, chịu giới hạn kích thước media)*.

**Journey**: `JOURNEY-003` Chuyển sang bản portable *(toàn bộ)* · `JOURNEY-007` Sau trận *(chỉ vế "mang bản kê câu đã dùng về máy chủ trung tâm và nhập vào contest tương ứng" — phần chốt trận, tie-break, xuất biên bản/kết quả thuộc EPIC-006/EPIC-010, xem §8)*.

---

## 2. User Scenarios & Testing *(mandatory)*

### US-001 — Xuất và nhập gói contest trọn vẹn (Priority: P1)

- **Actor**: `ACTOR-001` admin
- **Intent**: Xuất một contest đã dựng — câu hỏi, metadata media, media theo từng vòng, cấu hình luật — thành một gói duy nhất, mang sang bản cài khác, nhập lại thành contest dùng được ngay.
- **User value**: Giải `PS-2` cho ngày thi — soạn ở nơi có Internet, mang trọn contest sang máy portable không có Internet ở hội trường.
- **Why this priority**: `PRD-REQ-016` là P1 — không có cơ chế xuất/nhập trọn vẹn thì toàn bộ giá trị "trọn gói" của `GOAL-009` không tồn tại.
- **Independent Test**: Dựng một contest đầy đủ trên bản có Internet, xuất gói, nhập vào một bản cài trống. Chạy pre-flight kiểm kho đề — đạt mà không cần sửa gì. Không cần story nào khác.
- **Related PRD requirements**: `PRD-REQ-016`
- **Related game rules**: `GR-031`
- **Related journey**: `JOURNEY-003`

**Acceptance Scenarios**: AC-001 → AC-004, AC-027

---

### US-002 — Giữ đúng chủ sở hữu của hai cờ khi nhập/xuất (Priority: P1)

- **Actor**: `ACTOR-001` admin
- **Intent**: Biết chắc câu đã từng lộ vẫn bị chặn ở contest đích, nhưng câu đã dùng ở contest cũ thì rút được lại ở contest mới.
- **User value**: `everPublic` và `usedInContest` có hai chủ sở hữu khác nhau — câu và contest. Nhầm chủ sở hữu làm hoặc rò đề, hoặc khoá nhầm cả kho đề ở contest mới.
- **Why this priority**: `PRD-REQ-017` là P1 — đây là hàng rào bảo mật kế thừa trực tiếp từ EPIC-002, sai một trong hai chiều là hỏng cơ chế chống rò đề.
- **Independent Test**: Xuất một câu mang `everPublic = true` và `usedInContest = true` (ở contest nguồn), nhập vào một contest đích khác. Xác nhận câu vẫn bị hàng rào `everPublic` chặn khi thêm vào trận official ở contest đích, nhưng rút được lại bình thường vì `usedInContest` đã đặt lại.
- **Related PRD requirements**: `PRD-REQ-017`
- **Related game rules**: `GR-031`
- **Related journey**: `JOURNEY-003`

**Acceptance Scenarios**: AC-005, AC-006

---

### US-003 — Nhập/xuất phần đề của một người ra đề (Priority: P3)

- **Actor**: `ACTOR-002` setter
- **Intent**: Xuất đúng phần câu hỏi mình phụ trách, nhập vào bản cài khác mà không đụng phần của người khác.
- **User value**: Nhiều người soạn song song rồi gộp lại là cách làm việc thực tế của một ban đề.
- **Why this priority**: `PRD-REQ-018` là P3 — tiện ích cho quy trình soạn đề nhóm, không chặn việc chạy một trận đơn lẻ.
- **Independent Test**: Hai setter cùng có câu trong kho. Setter A xuất phần của mình, nhập vào một bản cài khác. Xác nhận chỉ câu của A xuất hiện, câu của B không có mặt và không bị đổi.
- **Related PRD requirements**: `PRD-REQ-018`
- **Related game rules**: —
- **Related journey**: `JOURNEY-001`

**Acceptance Scenarios**: AC-007, AC-008

---

### US-004 — Giới hạn kích thước media cấu hình được (Priority: P2)

- **Actor**: `ACTOR-002` setter
- **Intent**: Biết ngay khi tải một file vượt ngưỡng cho phép, kèm thông điệp nêu đúng ngưỡng đang áp.
- **User value**: Bản portable chạy trên máy tính xách tay mang tới hội trường; media không giới hạn làm gói contest phình to tới mức không mang đi được.
- **Why this priority**: `PRD-REQ-019` là P2 — không có ngưỡng thì soạn đề vẫn chạy được, nhưng rủi ro dồn sang bước xuất gói ở `JOURNEY-003`.
- **Independent Test**: Cấu hình một ngưỡng kích thước ảnh. Tải một file vượt ngưỡng — bị từ chối kèm thông điệp nêu đúng ngưỡng. Tải một file trong ngưỡng — thành công. Không cần contest hay gói nào.
- **Related PRD requirements**: `PRD-REQ-019`
- **Related game rules**: —
- **Related journey**: `JOURNEY-001`

**Acceptance Scenarios**: AC-009 → AC-011

---

### US-005 — Đánh dấu "đã dùng" hàng loạt bằng tay (Priority: P2)

- **Actor**: `ACTOR-001` admin
- **Intent**: Chọn nhiều câu trong danh sách đã gán của một contest, xác nhận một lần, đặt cờ đã dùng cho tất cả cùng lúc.
- **User value**: Trận chạy trên bản portable không có đường mang kết quả về tự động; đây là cách thủ công giữ kho đề ở máy chủ trung tâm đúng với thực tế đã lên sóng.
- **Why this priority**: `PRD-REQ-094` là P2 — không có nó thì admin phải đánh dấu từng câu một, không dùng được ở quy mô một trận thật.
- **Independent Test**: Chọn 12 câu trong danh sách đã gán của một contest, xác nhận qua dialog. Xác nhận cả 12 chuyển sang đã dùng, không có đường đảo ngược, và nhật ký ghi đủ người/thời điểm.
- **Related PRD requirements**: `PRD-REQ-094`
- **Related game rules**: `GR-031`
- **Related journey**: `JOURNEY-007`

**Acceptance Scenarios**: AC-012 → AC-014

---

### US-006 — Xuất và nhập bản kê câu đã dùng (Priority: P2)

- **Actor**: `ACTOR-001` admin
- **Intent**: Xuất bản kê các câu đã dùng của một contest chạy trên bản portable, mang về máy chủ trung tâm, nhập vào — chỉ đặt cờ, không bao giờ xoá cờ đã có.
- **User value**: Đây là đường DUY NHẤT mang thông tin "câu nào đã lộ" từ bản portable về trung tâm, ở quy mô nhiều trận — làm tay từng câu không dùng được.
- **Why this priority**: `PRD-REQ-095` là P2 — cùng nhóm giá trị với US-005, nhưng phục vụ quy mô lớn hơn (nhiều trận, nhiều máy portable).
- **Independent Test**: Xuất bản kê từ một contest, nhập vào một contest khác (mô phỏng "về trung tâm"). Xác nhận bản xem trước tách ba nhóm đúng, và nhập lại cùng bản kê lần thứ hai không đổi gì thêm.
- **Related PRD requirements**: `PRD-REQ-095`
- **Related game rules**: `GR-031`
- **Related journey**: `JOURNEY-007`

**Acceptance Scenarios**: AC-015 → AC-018, AC-028

---

### US-007 — Gói contest mang danh sách người tham gia, luôn mã hoá (Priority: P1)

- **Actor**: `ACTOR-001` admin
- **Intent**: Xuất gói contest kèm (tuỳ chọn) danh sách người tham gia — thí sinh, MC, và tài khoản admin của contest — luôn ở dạng mã hoá bằng một cụm mật khẩu truyền qua kênh khác; nhập đúng cụm mới dựng lại được tài khoản, sai cụm hoặc gói bị sửa thì không tạo bản ghi nào.
- **User value**: Bản có Internet và bản portable là hai hệ tài khoản độc lập. Không mang được danh sách thì nhập gói xong **không ai đăng nhập được**, và admin phải gõ lại toàn bộ tài khoản cùng phép gán ghế ngay tại hội trường — mất gần hết giá trị "trọn gói" của `GOAL-009`.
- **Why this priority**: `PRD-REQ-104` là P1 — đây là điều kiện để một contest nhập xong **dùng được ngay**, không chỉ "có dữ liệu".
- **Independent Test**: Xuất gói kèm danh sách người tham gia (gồm một vai tuỳ biến), đặt cụm mật khẩu. Nhập vào một bản cài trống với đúng cụm — mọi vai đăng nhập được, vai tuỳ biến giữ đúng quyền, không cần admin tạo tay tài khoản nào. Thử lại với cụm sai — không bản ghi nào được tạo.
- **Related PRD requirements**: `PRD-REQ-104`, `PRD-REQ-111` *(phần xuất kèm định nghĩa vai)*
- **Related game rules**: —
- **Related journey**: `JOURNEY-003`

**Acceptance Scenarios**: AC-019 → AC-026

---

## 3. Acceptance Scenarios *(mandatory)*

> Quy ước: **Given** nêu đủ actor, trạng thái dữ liệu, dữ liệu ban đầu và tiền điều kiện · **When** chỉ chứa một hành động hoặc một sự kiện · **Then** kiểm kết quả nghiệp vụ, chuyển trạng thái, tác dụng phụ, kết quả lỗi, và **những gì KHÔNG được đổi**.

### US-001 — Xuất và nhập gói contest trọn vẹn

**AC-001 — Happy path: xuất rồi nhập, kiểm kho đề đạt không cần sửa**
- **US**: US-001 · **FR**: FR-001, FR-002, FR-003 · **GR**: `GR-031`
- **Given** admin có một contest A trên bản có Internet, đã có câu hỏi `ACTIVE`, media đầy đủ, cấu hình luật đã đặt; bản đích (portable) đang trống, chưa có contest nào
- **When** admin xuất gói contest của A rồi nhập gói đó vào bản đích
- **Then** contest A xuất hiện ở bản đích dưới dạng **nháp**, đầy đủ câu hỏi, metadata media, media theo từng vòng, cấu hình luật · pre-flight kiểm kho đề **đạt** mà không cần thao tác sửa nào

**AC-002 — Partial acceptance: thiếu media không chặn cả gói, câu thiếu được đánh dấu**
- **US**: US-001 · **FR**: FR-006, FR-006b
- **Given** một gói contest đã chuẩn bị gồm 10 câu, nhưng thiếu file media của đúng 1 câu (do sao chép thiếu)
- **When** admin nhập gói này vào bản đích
- **Then** contest nháp được tạo với đủ **10** câu · hệ thống báo lỗi/cảnh báo nêu rõ đúng câu thiếu media · câu đó được đánh dấu tách biệt để admin tìm và bổ sung sau · việc nhập **không** bị chặn vì lý do thiếu media này

**AC-003 — Boundary: định danh câu giữ nguyên qua xuất/nhập**
- **US**: US-001 · **FR**: FR-004
- **Given** một câu trong contest nguồn mang định danh ổn định cố định
- **When** câu này được xuất trong gói rồi nhập vào bản đích
- **Then** câu ở bản đích mang **đúng** định danh đó — hệ thống không sinh định danh mới cho câu này

**AC-004 — Repeated action: nhập cùng gói hai lần tạo hai contest nháp độc lập**
- **US**: US-001 · **FR**: FR-005
- **Given** gói contest của A đã nhập thành công một lần, tạo contest nháp B1 ở bản đích
- **When** admin nhập lại đúng gói đó lần thứ hai vào cùng bản đích
- **Then** hệ thống tạo một contest nháp **thứ hai** B2, độc lập với B1 — không ghi đè, không hợp nhất; đây là hành vi thiết kế của gói contest (mỗi lần nhập là một bản sao chép mới), khác hẳn cơ chế hợp nhất của bản kê câu đã dùng (US-006)

**AC-027 — Partial failure: nhập bị ngắt giữa chừng, tất cả-hoặc-không**
- **US**: US-001 · **FR**: FR-029
- **Given** admin đang nhập một gói contest; kết nối mạng của admin bị ngắt **trước** khi server hoàn tất ghi dữ liệu
- **When** admin kết nối lại và kiểm tra bản đích
- **Then** **không** có contest dở dang nào được tạo — hệ thống ở đúng trạng thái như trước khi bắt đầu nhập; admin nhập lại từ đầu là đường xử lý duy nhất, không có bản ghi nửa vời nào cần dọn tay

### US-002 — Chủ sở hữu hai cờ khi nhập/xuất

**AC-005 — Happy path: everPublic đi theo câu, usedInContest đặt lại theo contest**
- **US**: US-002 · **FR**: FR-007, FR-008 · **GR**: `GR-031`
- **Given** contest nguồn có câu Q mang `everPublic = true` *(đã từng lộ)* và `usedInContest = true` *(đã dùng ở contest nguồn)*
- **When** câu Q được xuất và nhập vào một contest đích khác
- **Then** ở contest đích, câu Q **giữ** `everPublic = true` · `usedInContest` của câu Q **trong phạm vi contest đích** đặt lại thành `false` — câu rút được lại ở contest đích, nhưng vẫn bị hàng rào `everPublic` chặn khi thêm vào một trận `official` *(ép qua được bằng xác nhận hai bước, theo EPIC-002)*

**AC-006 — No-change guarantee: everPublic không đặt lại dù đổi contest nhiều lần**
- **US**: US-002 · **FR**: FR-007
- **Given** cùng câu Q ở AC-005, đã nhập vào contest đích thứ hai
- **When** câu Q tiếp tục được xuất từ contest đích thứ hai và nhập vào một contest thứ ba
- **Then** `everPublic` của câu Q **vẫn** `true` ở contest thứ ba — dấu vết này không bao giờ đặt lại, bất kể nhập vào bao nhiêu contest

### US-003 — Nhập/xuất phần đề của một setter

**AC-007 — Happy path: xuất đúng phần của mình**
- **US**: US-003 · **FR**: FR-009
- **Given** kho đề có câu của Setter A và câu của Setter B, mỗi câu gắn với đúng một người soạn
- **When** Setter A xuất phần đề của mình
- **Then** gói xuất ra **chỉ** chứa câu của Setter A · câu của Setter B **không** có trong gói

**AC-008 — No-change guarantee: nhập phần của A không đụng phần của B**
- **US**: US-003 · **FR**: FR-010
- **Given** bản đích đã có sẵn câu của Setter B
- **When** nhập gói phần đề của Setter A vào bản đích đó
- **Then** câu của Setter A được thêm vào · câu của Setter B **không đổi**

### US-004 — Giới hạn kích thước media

**AC-009 — Happy path: file trong ngưỡng được chấp nhận**
- **US**: US-004 · **FR**: FR-011
- **Given** ngưỡng kích thước ảnh được cấu hình là **X**; setter đang tải một ảnh có kích thước nhỏ hơn X
- **When** setter tải ảnh lên
- **Then** tải thành công, ảnh gắn vào câu hỏi

**AC-010 — Rejection: file vượt ngưỡng bị từ chối kèm thông điệp nêu ngưỡng**
- **US**: US-004 · **FR**: FR-011, FR-012
- **Given** cùng ngưỡng **X**; setter tải một ảnh lớn hơn X
- **When** setter tải ảnh lên
- **Then** hệ thống từ chối · thông điệp nêu **đúng** giá trị X đang áp · câu hỏi **không** gắn media này

**AC-011 — Boundary: file đúng bằng ngưỡng được chấp nhận (biên đóng)**
- **US**: US-004 · **FR**: FR-011
- **Given** ngưỡng **X**; setter tải một file có kích thước **đúng bằng** X
- **When** setter tải file lên
- **Then** tải thành công · file gắn vào câu hỏi — biên đóng, tương đương "≤ X"

### US-005 — Đánh dấu "đã dùng" hàng loạt bằng tay

**AC-012 — Happy path: chọn nhiều câu, xác nhận, cả loạt chuyển đã dùng**
- **US**: US-005 · **FR**: FR-013, FR-015 · **GR**: `GR-031`
- **Given** contest có danh sách câu đã gán, trong đó 12 câu đang `usedInContest = false`
- **When** admin chọn đúng 12 câu này và xác nhận qua dialog
- **Then** cả 12 câu chuyển `usedInContest = true` · nhật ký ghi đủ người thực hiện và thời điểm cho thao tác này

**AC-013 — Invalid state: không có đường đảo ngược**
- **US**: US-005 · **FR**: FR-014
- **Given** các câu ở AC-012 đã được đánh dấu đã dùng
- **When** admin tìm cách đặt lại một trong số đó về chưa dùng, qua chính công cụ đánh dấu hàng loạt
- **Then** **không tồn tại** thao tác nào cho việc đó — công cụ chỉ hỗ trợ **một chiều** *(chưa dùng → đã dùng)*, cờ của các câu **không đổi**

**AC-014 — Rejection: huỷ ở dialog xác nhận, không đổi gì**
- **US**: US-005 · **FR**: FR-015
- **Given** admin đã chọn nhiều câu và dialog xác nhận đang hiện
- **When** admin bấm **No**
- **Then** **không** câu nào đổi cờ · dữ liệu không đổi

### US-006 — Xuất và nhập bản kê câu đã dùng

**AC-015 — Happy path: xem trước ba nhóm, chỉ nhóm đầu được áp**
- **US**: US-006 · **FR**: FR-016, FR-017, FR-018 · **GR**: `GR-031`
- **Given** một contest chạy trên bản portable có bản kê câu đã dùng xuất ra gồm N mục *(định danh câu, thời điểm dùng, trận đã dùng)*
- **When** admin nhập bản kê này vào contest tương ứng ở máy chủ trung tâm
- **Then** hệ thống hiện bản xem trước tách ba nhóm — **sẽ chuyển đã dùng** · **đã ở trạng thái đó** · **không thuộc danh sách câu đã gán của contest đích** — sau khi admin xác nhận, **chỉ** nhóm đầu tiên được áp, đặt `usedInContest = true` cho các câu đó

**AC-016 — Idempotency: nhập cùng bản kê hai lần**
- **US**: US-006 · **FR**: FR-019
- **Given** bản kê ở AC-015 đã nhập thành công một lần
- **When** admin nhập lại **đúng** bản kê đó lần thứ hai
- **Then** **không** câu nào đổi trạng thái ở lần thứ hai — kết quả giống hệt sau lần đầu, hệ thống không báo lỗi, xem như không có tác dụng

**AC-017 — Boundary/no-change: nhóm thứ ba không tự áp**
- **US**: US-006 · **FR**: FR-018
- **Given** bản kê có một mục mang định danh câu **không** nằm trong danh sách câu đã gán của contest đích
- **When** admin xem bản xem trước rồi xác nhận áp
- **Then** mục đó nằm trong nhóm thứ ba, hiển thị rõ ràng, **và không** bị đặt cờ đã dùng dù admin đã xác nhận — chỉ hai nhóm đầu được áp

**AC-018 — No-change guarantee: phép hợp không bao giờ đảo ngược cờ**
- **US**: US-006 · **FR**: FR-017
- **Given** một câu ở contest đích đang `usedInContest = true`
- **When** admin nhập một bản kê **không** chứa câu này *(bản kê từ một trận khác, không đụng câu này)*
- **Then** câu đó **vẫn giữ** `usedInContest = true` — phép hợp không bao giờ đặt cờ từ `true` về `false`

**AC-028 — Concurrency: hai phiên admin cùng nhập bản kê vào cùng một contest đích**
- **US**: US-006 · **FR**: FR-019b · **GR**: `GR-031`
- **Given** hai phiên admin cùng mở màn nhập bản kê câu đã dùng cho cùng một contest đích, mỗi phiên có một bản kê chứa câu Q ở dạng "sẽ chuyển đã dùng"
- **When** cả hai phiên xác nhận áp gần như cùng lúc
- **Then** câu Q chuyển `usedInContest = true` — kết quả cuối cùng giống hệt nhau bất kể phiên nào tới trước; không phiên nào bị từ chối, không cần khoá hay hàng đợi

### US-007 — Gói contest mang danh sách người tham gia, luôn mã hoá

**AC-019 — Happy path: xuất kèm danh sách, nhập đúng cụm mật khẩu**
- **US**: US-007 · **FR**: FR-020, FR-021, FR-023, FR-024
- **Given** admin xuất gói contest, chọn kèm danh sách người tham gia *(thí sinh, MC, tài khoản admin của contest)*, đặt một cụm mật khẩu, chọn phương án "tạo mật khẩu mới lúc nhập kèm phiếu tài khoản"
- **When** bên nhận nhập gói vào một bản cài trống, khai **đúng** cụm mật khẩu
- **Then** mọi vai trong danh sách dựng lại được và đăng nhập được ngay mà **không** cần admin tạo tay tài khoản nào · phiếu tài khoản in được sinh cho mỗi tài khoản mới · tài khoản admin của contest **luôn** theo phương án "tạo mật khẩu mới", bất kể phương án chọn cho các vai khác trong cùng gói

**AC-020 — Rejection + repeated action: sai cụm mật khẩu → từ chối toàn bộ, thử lại không giới hạn**
- **US**: US-007 · **FR**: FR-022
- **Given** cùng gói ở AC-019
- **When** bên nhận nhập gói nhưng khai **sai** cụm mật khẩu, liên tiếp nhiều lần
- **Then** mỗi lần đều bị từ chối toàn bộ · **không** bản ghi tài khoản nào được tạo ở bất kỳ lần nào — không có trạng thái nhập nửa vời · hệ thống **không** khoá thao tác hay giới hạn số lần thử tiếp theo ở tầng nghiệp vụ

**AC-021 — Rejection: gói bị sửa (dù một byte) → từ chối toàn bộ**
- **US**: US-007 · **FR**: FR-022
- **Given** gói đã bị sửa đổi sau khi xuất
- **When** bên nhận nhập gói, kể cả khi khai đúng cụm mật khẩu
- **Then** toàn bộ việc nhập bị từ chối · **không** bản ghi nào được tạo

**AC-022 — Boundary: gói không kèm danh sách vẫn hợp lệ**
- **US**: US-007 · **FR**: FR-020
- **Given** admin xuất gói contest, **không** chọn kèm danh sách người tham gia
- **When** bên nhận nhập gói
- **Then** contest nháp được dựng · **không** có tài khoản nào đi kèm · **không** hỏi cụm mật khẩu nào

**AC-023 — Decision path: trùng tên đăng nhập ở bên nhận**
- **US**: US-007 · **FR**: FR-025
- **Given** bên nhận đã có sẵn một tài khoản trùng tên đăng nhập với một mục trong danh sách đang nhập
- **When** hệ thống phát hiện trùng tên lúc nhập
- **Then** hệ thống **hỏi** người nhập, đưa ra đúng ba lựa chọn — **nối vào tài khoản sẵn có** · **tạo tài khoản mới có hậu tố** · **huỷ nhập mục đó** — hệ thống **không** tự động nối

**AC-024 — No-change guarantee: chỉ người đã gán vào contest được xuất**
- **US**: US-007 · **FR**: FR-026
- **Given** bản cài nguồn có danh bạ 50 tài khoản, nhưng chỉ 8 người được gán vào contest đang xuất
- **When** admin xuất gói kèm danh sách người tham gia
- **Then** gói chỉ chứa **đúng** 8 tài khoản đã gán · 42 tài khoản còn lại của bản cài **không** xuất hiện trong gói

**AC-025 — Side effect: vai tuỳ biến xuất kèm định nghĩa đầy đủ**
- **US**: US-007 · **FR**: FR-027
- **Given** contest nguồn có một vai tuỳ biến đã gán cho một tài khoản trong danh sách đang xuất
- **When** admin xuất gói kèm danh sách người tham gia
- **Then** gói mang theo **định nghĩa đầy đủ** của vai tuỳ biến đó *(tập permission)*, không chỉ tên vai · khi nhập vào bản đích, tài khoản đó dựng lại **đúng quyền** như ở bản nguồn

**AC-026 — Side effect: xuất/nhập danh sách ghi vào nhật ký riêng**
- **US**: US-007 · **FR**: FR-028
- **Given** admin thực hiện xuất gói kèm danh sách người tham gia
- **When** thao tác xuất hoàn tất
- **Then** một dòng nhật ký **riêng** được ghi cho việc xuất danh sách người tham gia, tách khỏi nhật ký của lần xuất câu hỏi/gói — vì đây là một lần dữ liệu cá nhân rời hệ thống

---
**AC-029 — Xuất lại nhiều lần: mỗi lần khai lại cụm mật khẩu**
- **FR**: FR-030 · `NFR-24b`
- **Given** một contest đã từng được xuất kèm danh sách người tham gia với một cụm mật khẩu,
- **When** admin xuất lại chính contest đó lần thứ hai,
- **Then** hệ thống đòi khai **lại** cụm mật khẩu từ đầu · **0** ô *ghi nhớ cụm* nào tồn tại · **0** gợi ý cụm cũ nào hiện ra · admin khai **cùng** cụm cũ hoặc một cụm **khác** đều được chấp nhận · **0** phép kiểm nào so cụm mới với cụm cũ

**AC-030 — Gói rỗng bị chặn, gói một câu được nhận**
- **FR**: FR-031 · `GR-031` C3
- **Given** hai contest — một contest **chưa gán câu nào**, một contest gán **đúng 1** câu *(chưa đủ cho bất kỳ vòng nào)*,
- **When** admin xuất cả hai, rồi nhập gói xuất được vào một bản cài khác,
- **Then** contest rỗng bị **từ chối ở bước xuất** kèm thông điệp *gói không có câu hỏi nào* · contest 1 câu **xuất được và nhập được bình thường** · một gói rỗng dựng tay cũng bị **từ chối ở bước nhập** · **0** dữ liệu nào đổi ở hai lần bị từ chối

**AC-031 — Xuất và nhập gói contest khi đã ngắt Internet**
- **FR**: FR-032 · `PRD-REQ-098`
- **Given** một bản cài có contest đầy đủ đề và media, **đã chặn toàn bộ đường ra Internet**,
- **When** admin xuất gói contest rồi nhập lại gói đó vào một bản cài khác cũng đã ngắt mạng,
- **Then** cả hai chiều **hoàn thành bình thường** · **0** lần gọi nào chờ hết thời gian vì một dịch vụ ngoài · nội dung gói **không đổi** so với lần xuất khi còn mạng · **0** dữ liệu nào bị mất qua vòng xuất-nhập


## 4. Functional Requirements *(mandatory)*

> Mỗi FR là một phát biểu kiểm thử được, tham chiếu US-NNN, PRD-REQ-NNN, GR-NNN và AC-NNN. Không FR nào nêu framework, cơ sở dữ liệu, đường API, lớp, hàm hay thuật toán.

### Nhóm A — Xuất/nhập gói contest trọn vẹn

- **FR-001**: Hệ thống MUST xuất được một contest thành một gói duy nhất gồm câu hỏi, metadata media, media theo từng vòng, và cấu hình luật. · US-001 · `PRD-REQ-016` · `GR-031` · AC-001
- **FR-002**: Hệ thống MUST nhập được gói đó trên một bản cài khác, dựng lại contest ở dạng **nháp**. · US-001 · `PRD-REQ-016` · `GR-031` · AC-001
- **FR-003**: Sau khi nhập một gói đầy đủ và hợp lệ, contest MUST qua được phép kiểm kho đề mà không cần thao tác sửa nào. · US-001 · `PRD-REQ-016` · `GR-031` · AC-001
- **FR-004**: Gói MUST mang định danh ổn định của từng câu hỏi; bên nhập MUST giữ nguyên định danh đó, MUST NOT sinh định danh mới cho câu đã có trong gói. · US-001 · `PRD-REQ-016` · — · AC-003
- **FR-005**: Mỗi lần nhập gói MUST tạo một contest nháp **mới**, độc lập; hệ thống MUST NOT tự động hợp nhất với một contest đã nhập trước đó từ cùng gói. · US-001 · `PRD-REQ-016` · — · AC-004
- **FR-006**: Hệ thống MUST đánh giá một lần nhập gói theo thứ tự: **(1)** gói giải nén và đọc được không — không ⇒ từ chối toàn bộ; **(2)** gói có kèm danh sách người tham gia không, và nếu có thì cụm mật khẩu cùng tính toàn vẹn của gói có đạt không *(FR-022)* — không đạt ⇒ từ chối toàn bộ, không tạo bản ghi nào; **(3)** mọi bước trên đạt ⇒ tạo contest nháp với **toàn bộ** câu hỏi trong gói. Ở bước (3), việc **thiếu file media** của một hay nhiều câu MUST NOT chặn thao tác nhập: câu thiếu media vẫn được tạo, và hệ thống MUST báo rõ **đúng những câu nào** thiếu media. Thiếu media là nội dung không đầy đủ nhưng vẫn hợp lệ để dựng contest; nó khác hẳn ca gói hỏng hoặc sai cụm mật khẩu ở bước (1) và (2), nơi không có gì đọc được để nhập. · US-001 · `PRD-REQ-016` · — · AC-002
- **FR-006b**: Câu thiếu media sau khi nhập MUST mang một dấu hiệu tách biệt với câu đã đầy đủ, đủ để admin lọc ra và bổ sung media sau. · US-001 · `PRD-REQ-016` · — · AC-002

### Nhóm B — Chủ sở hữu hai cờ khi nhập/xuất

- **FR-007**: Dấu vết "đã từng public" (`everPublic`) của một câu MUST đi theo câu qua nhập/xuất, giữ nguyên giá trị bất kể câu vào contest nào. · US-002 · `PRD-REQ-017` · `GR-031` · AC-005, AC-006
- **FR-008**: Cờ "đã dùng trong contest" (`usedInContest`) MUST đặt lại thành chưa-dùng khi câu vào một contest **khác** qua nhập, vì cờ này là thuộc tính của contest, không phải của câu. · US-002 · `PRD-REQ-017` · `GR-031` · AC-005

### Nhóm C — Nhập/xuất phần đề của một setter

- **FR-009**: Người ra đề MUST xuất được đúng phần câu hỏi mình phụ trách thành một gói riêng. · US-003 · `PRD-REQ-018` · — · AC-007
- **FR-010**: Nhập một gói phần đề của một setter MUST NOT đụng tới câu hỏi của setter khác đã có ở bản đích. · US-003 · `PRD-REQ-018` · — · AC-008

### Nhóm D — Giới hạn kích thước media

- **FR-011**: Hệ thống MUST áp một ngưỡng kích thước **riêng cho từng loại media** — ảnh, video, audio. Mỗi ngưỡng MUST cấu hình được theo từng bản triển khai và MUST NOT hard-code ở bất kỳ đâu trong hệ thống hay giao diện. Khi nhận một file, hệ thống MUST so kích thước file với ngưỡng của **đúng loại media đó** và MUST áp **biên đóng**: file có kích thước **nhỏ hơn hoặc đúng bằng** ngưỡng ⇒ chấp nhận và gắn vào câu hỏi; chỉ file **lớn hơn** ngưỡng mới bị từ chối. Phép so này MUST chạy ở cả giao diện *(để báo sớm)* lẫn server *(để cưỡng chế)*. · US-004 · `PRD-REQ-019` · — · AC-009, AC-010, AC-011
- **FR-012**: File lớn hơn ngưỡng MUST bị từ chối, và thông điệp từ chối MUST nêu **giá trị ngưỡng đang áp cho loại media đó**, đọc từ cấu hình tại thời điểm từ chối — MUST NOT nêu một con số cố định viết sẵn. Lần từ chối MUST không tác dụng phụ: câu hỏi **không** gắn media này, không media nào khác của câu bị đụng, và các trường còn lại của câu **không đổi**. · US-004 · `PRD-REQ-019` · — · AC-010

### Nhóm E — Đánh dấu "đã dùng" hàng loạt bằng tay

- **FR-013**: Admin MUST đánh dấu được nhiều câu cùng lúc là đã dùng trong một contest, chọn từ danh sách câu đã gán. · US-005 · `PRD-REQ-094` · `GR-031` · AC-012
- **FR-014**: Thao tác này MUST một chiều — chỉ đặt cờ từ chưa-dùng sang đã-dùng; hệ thống MUST NOT cung cấp đường ngược lại. · US-005 · `PRD-REQ-094` · `GR-031` · AC-013
- **FR-015**: Thao tác này MUST đi qua dialog xác nhận, và MUST vào nhật ký thao tác kèm người thực hiện và thời điểm. · US-005 · `PRD-REQ-094` · — · AC-012, AC-014

### Nhóm F — Xuất/nhập bản kê câu đã dùng

- **FR-016**: Hệ thống MUST xuất được bản kê các câu đã dùng của một contest, gồm định danh câu, thời điểm dùng, và trận đã dùng. · US-006 · `PRD-REQ-095` · `GR-031` · AC-015
- **FR-017**: Hệ thống MUST nhập được bản kê đó vào một contest trên bản cài khác; phép nhập MUST là **phép hợp** — chỉ đặt cờ đã-dùng, MUST NOT xoá cờ nào đã có. · US-006 · `PRD-REQ-095` · `GR-031` · AC-015, AC-018
- **FR-018**: Trước khi áp một bản kê, hệ thống MUST xếp từng mục vào đúng một trong ba nhóm và MUST hiện bản xem trước theo ba nhóm đó:
  1. **Sẽ chuyển đã dùng** — câu thuộc danh sách đã gán của contest đích và đang chưa-dùng. Sau khi admin xác nhận, nhóm này MUST được đặt cờ đã-dùng.
  2. **Đã ở trạng thái đó** — câu thuộc danh sách đã gán và đã mang cờ đã-dùng. Áp lên nhóm này MUST không đổi gì.
  3. **Không thuộc danh sách câu đã gán của contest đích** — MUST hiển thị rõ và MUST NOT được đặt cờ, kể cả sau khi admin đã xác nhận.

  Admin huỷ ở bước xác nhận ⇒ **không** cờ nào đổi. · US-006 · `PRD-REQ-095` · `GR-031` · AC-015, AC-017
- **FR-019**: Nhập cùng một bản kê nhiều lần MUST cho cùng một kết quả; các lần nhập sau lần đầu MUST NOT đổi thêm gì. · US-006 · `PRD-REQ-095` · — · AC-016
- **FR-019b**: Hai phiên admin cùng nhập bản kê câu đã dùng vào **cùng một contest đích** gần như đồng thời MUST NOT cần khoá hay hàng đợi phân xử: phép hợp chỉ đặt cờ đã-dùng theo một chiều, nên hai thao tác cùng đặt cờ cho một câu MUST cho ra cùng một kết quả cuối bất kể thứ tự hay xen kẽ, và MUST NOT có phiên nào bị từ chối. · US-006 · `PRD-REQ-095` · `GR-031` · AC-028

### Nhóm G — Danh sách người tham gia, luôn mã hoá

- **FR-020**: Gói contest MUST cho phép kèm hoặc không kèm danh sách người tham gia — thí sinh, MC, và tài khoản admin của contest — khi xuất. · US-007 · `PRD-REQ-104` · — · AC-019, AC-022
- **FR-021**: Khi kèm, danh sách MUST luôn ở dạng đã mã hoá, kể cả khi không mang mật khẩu nào; chìa khoá MUST NOT nằm trong gói. · US-007 · `PRD-REQ-104` · — · AC-019
- **FR-022**: Khi gói có kèm danh sách người tham gia, hệ thống MUST đánh giá theo thứ tự: **(1)** hỏi bên nhập cụm mật khẩu; **(2)** giải mã danh sách bằng cụm đó — thất bại ⇒ từ chối; **(3)** kiểm tính toàn vẹn của gói — gói đã bị sửa dù chỉ một byte ⇒ từ chối, kể cả khi cụm mật khẩu đúng; **(4)** cả hai đạt ⇒ dựng tài khoản theo FR-023 → FR-025.

  Từ chối ở bước (2) hoặc (3) MUST là **từ chối toàn bộ**: MUST NOT tạo bản ghi tài khoản nào, MUST NOT tạo phép gán vai nào, MUST NOT tạo contest nháp nào, và MUST NOT để lại trạng thái nhập nửa vời cần dọn tay. Bên nhập MUST thử lại được ngay; hệ thống MUST NOT khoá thao tác và MUST NOT giới hạn số lần thử sai liên tiếp ở tầng nghiệp vụ — sức chống dò nằm ở **độ mạnh của cụm mật khẩu lúc xuất**, không ở số lần thử nhập. · US-007 · `PRD-REQ-104` · — · AC-020, AC-021
- **FR-023**: Cách xử lý mật khẩu khi nhập MUST là lựa chọn của người xuất, giữa hai phương án — tạo mật khẩu mới kèm phiếu tài khoản in được *(mặc định)*, hoặc giữ mật khẩu hiện tại — và lựa chọn MUST đi trong gói. · US-007 · `PRD-REQ-104` · — · AC-019
- **FR-024**: Tài khoản admin của contest MUST luôn theo phương án "tạo mật khẩu mới", bất kể phương án chọn cho các vai khác trong cùng gói. · US-007 · `PRD-REQ-104` · — · AC-019
- **FR-025**: Trùng tên đăng nhập ở bên nhận MUST khiến hệ thống hỏi người nhập, với đúng ba lựa chọn — nối vào tài khoản sẵn có, tạo tài khoản mới có hậu tố, hoặc huỷ; hệ thống MUST NOT tự động nối. · US-007 · `PRD-REQ-104` · — · AC-023
- **FR-026**: Chỉ người **đã được gán** vào contest đang xuất MUST xuất hiện trong danh sách; hệ thống MUST NOT xuất toàn bộ danh bạ của bản cài. · US-007 · `PRD-REQ-104` · — · AC-024
- **FR-027**: Vai tuỳ biến gán cho một tài khoản trong danh sách MUST xuất kèm định nghĩa đầy đủ của vai đó *(tập permission)*, không chỉ tên vai. · US-007 · `PRD-REQ-104`, `PRD-REQ-111` · — · AC-025
- **FR-028**: Mỗi lần xuất hoặc nhập danh sách người tham gia MUST ghi một dòng nhật ký riêng, tách khỏi nhật ký của lần xuất/nhập câu hỏi. · US-007 · `PRD-REQ-104` · — · AC-026

### Nhóm H — Toàn vẹn khi bị ngắt giữa chừng

- **FR-029**: Mọi thao tác nhập *(gói contest, bản kê câu đã dùng, danh sách người tham gia)* MUST là **tất cả-hoặc-không**: nếu thao tác bị ngắt trước khi hoàn tất ở server, hệ thống MUST NOT để lại bất kỳ bản ghi dở dang nào — kết quả quan sát được chỉ có hai khả năng, **đã hoàn tất đầy đủ** hoặc **chưa hề xảy ra**. Hệ thống MUST NOT phơi ra trạng thái nhập dở dang, tiến độ từng phần, hay thao tác resume; đường xử lý duy nhất khi bị ngắt là **nhập lại từ đầu**, và MUST NOT có bản ghi nửa vời nào cần dọn tay. · US-001 · `PRD-REQ-016` · — · AC-027

---
- **FR-030**: Cụm mật khẩu bảo vệ danh sách người tham gia MUST NOT được lưu ở bất kỳ đâu trong hệ thống — nó là chìa mã hoá, không phải một thiết lập của contest. **Mỗi** lần xuất MUST đòi người xuất khai lại cụm mật khẩu. Hệ thống MUST NOT có ô *ghi nhớ cụm mật khẩu*, MUST NOT gợi ý lại cụm cũ, và MUST NOT kiểm rằng cụm mới khác cụm cũ. *(`QĐ-156` · `NFR-24b` · AC-029)*
- **FR-031**: Gói contest có **0 câu hỏi** MUST bị chặn ở **cả** bước xuất lẫn bước nhập, kèm thông điệp nêu rõ *gói không có câu hỏi nào*. Gói có **từ 1 câu trở lên** MUST được chấp nhận, kể cả khi số câu chưa đủ để chạy bất kỳ vòng nào — phép kiểm đủ đề cho một vòng là việc của **cửa vào vòng**, không phải của bước xuất. *(`QĐ-157` · `GR-031` C3 · AC-030)*
- **FR-032**: Việc **xuất** và **nhập** gói contest MUST NOT phụ thuộc một dịch vụ ngoài và MUST NOT phụ thuộc kết nối Internet; cả hai chiều MUST chạy được trên một bản cài đã ngắt đường ra ngoài. *(`PRD-REQ-098` vế EPIC-003 · `QĐ-084` · AC-031)*

## 5. Key Entities

- **Gói contest** *(X1, `QĐ-084`)*: câu hỏi + metadata media + media theo từng vòng + cấu hình luật + *(tuỳ chọn)* danh sách người tham gia đã mã hoá. Chiều **ra và vào**; mỗi lần nhập tạo một contest **nháp mới**, không hợp nhất.
- **Bản kê câu đã dùng** *(X3, `QĐ-084`)*: danh sách các mục *(định danh câu, thời điểm dùng, trận đã dùng)*. Chiều **ra và vào**; phép nhập là **phép hợp** — chỉ đặt cờ, không bao giờ xoá cờ, và **idempotent**.
- **Danh sách người tham gia**: thí sinh, MC, và tài khoản admin của contest, kèm phép gán vai — bao gồm định nghĩa đầy đủ của mọi vai tuỳ biến liên quan. Chỉ chứa người **đã được gán** vào contest đang xuất. Luôn ở dạng mã hoá khi xuất kèm; cụm mật khẩu truyền qua kênh khác, không nằm trong gói.
- **Cờ `everPublic`** *(`TERM-049`, thuộc **câu**)*: dấu vết một chiều, đi theo câu qua mọi lần nhập/xuất, không bao giờ đặt lại.
- **Cờ `usedInContest`** *(`TERM-048`, thuộc cặp **câu — contest**)*: đặt lại thành chưa-dùng khi câu vào một contest khác qua nhập gói contest; chỉ đặt (không bao giờ xoá) khi nhập bản kê câu đã dùng vào cùng một contest.
- **Định danh câu ổn định**: sống sót qua mọi lần xuất/nhập — điều kiện cần để bản kê câu đã dùng ghép đúng vào dữ liệu bên nhận, và để `everPublic` bám đúng câu.

---

## 6. Success Criteria *(mandatory)*

- **SC-001**: Admin nhập một gói contest trọn vẹn vào một bản cài trống và chạy pre-flight kiểm kho đề đạt ngay lần thử đầu tiên, không cần sửa thủ công nào.
- **SC-002**: 100% câu mang dấu vết "đã từng public" trong gói thử nghiệm vẫn bị hàng rào chặn ở contest đích sau khi nhập.
- **SC-003**: Nhập cùng một bản kê câu đã dùng hai lần cho ra đúng một kết quả — 0% sai khác giữa trạng thái sau lần một và sau lần hai.
- **SC-004**: Nhập một gói contest kèm danh sách người tham gia vào một bản cài trống, 100% vai trong danh sách đăng nhập được ngay lần thử đầu tiên mà không cần admin tạo tay bất kỳ tài khoản nào.
- **SC-005**: Nhập gói với cụm mật khẩu sai hoặc gói đã bị sửa cho ra đúng **0** bản ghi tài khoản mới được tạo, đo trên toàn bộ các ca thử.

---

## 7. Edge Cases

**Giá trị biên**
- File media có kích thước **đúng bằng** ngưỡng cấu hình — biên đóng, được chấp nhận *(AC-011)*.
- Bản kê câu đã dùng rỗng *(0 mục)* được nhập — không có gì để hợp, hệ thống không đổi gì và không báo lỗi.

**Trạng thái không hợp lệ**
- Tìm đường đảo `usedInContest` từ đã-dùng về chưa-dùng qua công cụ đánh dấu hàng loạt: không tồn tại *(AC-013)*.
- Nhập một bản kê mà `questionId` không thuộc danh sách gán của contest đích: rơi vào nhóm thứ ba, không tự áp *(AC-017)*.
- Gói thiếu media của một hoặc nhiều câu: **không** phải trạng thái không hợp lệ chặn cứng — contest nháp vẫn tạo đủ mọi câu, câu thiếu media chỉ bị đánh dấu để bổ sung sau *(AC-002)*. Khác hẳn ca gói bị sửa hoặc sai cụm mật khẩu, nơi gói không giải mã hay xác thực được nên không có gì để nhập *(AC-020, AC-021)*.

**Thao tác lặp**
- Nhập cùng một gói contest nhiều lần: mỗi lần tạo một contest nháp mới, độc lập *(AC-004)*.
- Nhập cùng một bản kê câu đã dùng nhiều lần: idempotent, không đổi thêm từ lần thứ hai *(AC-016)*.
- Xuất gói kèm danh sách người tham gia nhiều lần liên tiếp cho cùng một contest — mỗi lần có bắt buộc một cụm mật khẩu mới hay dùng lại được cụm cũ chưa được nguồn nào quy định *(xem `OQ-006`)*.

**Trạng thái cũ / sự kiện trùng**
- Hai phiên admin cùng nhập gói contest (X1) vào cùng một bản đích gần như đồng thời: mỗi phiên tạo một contest nháp riêng, không đụng nhau *(AC-004)*.
- Hai phiên admin cùng nhập bản kê câu đã dùng (X3) vào CÙNG một contest đích gần như đồng thời: không cần phân xử, phép hợp một chiều cho cùng kết quả bất kể thứ tự *(AC-028)*.
- Danh sách người tham gia được xuất trước khi ghế/vai của contest đổi thêm — gói mang một ảnh chụp tại thời điểm xuất, không tự đồng bộ lại sau đó.

**Hỏng một phần**
- Quá trình nhập hoặc xuất gói bị ngắt giữa chừng *(mất kết nối, mất điện)*: **tất cả-hoặc-không** — thao tác đã hoàn tất ở server thì bản ghi đầy đủ, chưa hoàn tất thì không để lại bản ghi dở dang nào *(AC-027, FR-029)*.

**Hành vi nguồn không quy định** — hai mục còn mở: `OQ-006` (cụm mật khẩu mới hay dùng lại khi xuất lại danh sách), `OQ-007` (gói với kho đề rỗng) ở §9; spec này không tự điền các hành vi đó.

---

## 8. Out of Scope

**Requirement thuộc epic khác, không đưa vào feature này**

| Ngoài phạm vi | Thuộc về |
|---|---|
| Xuất toàn bộ kết quả + nhật ký sự kiện của contest (`X2`), xuất kết quả rút gọn (`X4`) | EPIC-010 *(`PRD-REQ-096`, `PRD-REQ-097`)* — hai gói này **chỉ có chiều ra**, không thuộc "trọn gói" nhập/xuất được của EPIC-003, dù cùng nằm trong bảng bốn loại gói của `QĐ-084` |
| Đồng bộ tự động kết quả từ bản portable về máy chủ trung tâm | **Không làm ở bất kỳ epic nào** — `QĐ-084` chốt đây là ranh giới chấp nhận có chủ đích, không phải chỗ thiếu đặc tả |
| Định nghĩa, tạo, sửa vai tuỳ biến và tập permission của nó | EPIC-001 *(`PRD-REQ-111` phần định nghĩa)* — feature này chỉ sở hữu việc **xuất kèm** định nghĩa đã có trong gói *(FR-027)* |
| Xác thực, đăng nhập, mô hình phiên, quyền điều khiển | EPIC-001 |
| Chọn danh sách câu cho MỘT contest cụ thể, cấu hình luật, mode trả lời | EPIC-004 |
| Duyệt đề `DRAFT` → `ACTIVE`, cờ hiển thị dẫn xuất, bộ đề | EPIC-002 |
| Thống kê ghi ngược kho đề sau khi trận đóng sổ | EPIC-010 *(`PRD-REQ-081`)* — feature này chỉ đóng góp **cờ đã dùng** qua bản kê (US-006), không đóng góp số liệu thống kê cho trận chạy trên bản portable |
| Biên bản trận in theo lần chạy, xuất biên bản trước khi dọn dữ liệu | EPIC-010 *(`PRD-REQ-079`, `080`)* |
| Cài đặt lần đầu qua dòng lệnh trên bản cài trống | EPIC-001/EPIC-012 — feature này chỉ sở hữu đường **tài khoản admin đi theo gói contest** *(FR-024)*, không sở hữu đường dòng lệnh |
| Giá trị mặc định của ngưỡng kích thước media theo từng hồ sơ triển khai *(container vs portable)* | EPIC-012 — feature này chỉ yêu cầu ngưỡng **tồn tại và cấu hình được** *(FR-011)* |

**Non-goal của chính EPIC-003** *(`docs/PRD.md` §11)*
- Đồng bộ tự động kết quả portable → trung tâm — `QĐ-084` chốt là **không làm**.

---

## 9. Open Questions

**Không có.** Hai mục từng mở đã được phân xử: cụm mật khẩu **không bao giờ được lưu** nên mỗi lần xuất là một lần khai lại (`QĐ-156`), và gói contest bị chặn **chỉ khi kho đề rỗng** (`QĐ-157`).

## 10. Traceability Matrix

| User story | PRD requirement | Game rules | Functional requirements | Acceptance scenarios |
|---|---|---|---|---|
| **US-001** — Xuất/nhập gói contest trọn vẹn | `PRD-REQ-016` | `GR-031` | FR-001 → FR-006, FR-006b, FR-029 | AC-001 → AC-004, AC-027 |
| **US-002** — Chủ sở hữu hai cờ khi nhập/xuất | `PRD-REQ-017` | `GR-031` | FR-007, FR-008 | AC-005, AC-006 |
| **US-003** — Nhập/xuất phần đề của một setter | `PRD-REQ-018` | — | FR-009, FR-010 | AC-007, AC-008 |
| **US-004** — Giới hạn kích thước media | `PRD-REQ-019` | — | FR-011, FR-012 | AC-009 → AC-011 |
| **US-005** — Đánh dấu "đã dùng" hàng loạt bằng tay | `PRD-REQ-094` | `GR-031` | FR-013 → FR-015 | AC-012 → AC-014 |
| **US-006** — Xuất/nhập bản kê câu đã dùng | `PRD-REQ-095` | `GR-031` | FR-016 → FR-019, FR-019b | AC-015 → AC-018, AC-028 |
| **US-007** — Danh sách người tham gia, luôn mã hoá | `PRD-REQ-104`, `PRD-REQ-111` | — | FR-020 → FR-028 | AC-019 → AC-026 |

### Phủ bảng quyết định của game rule (phần thuộc EPIC-003)

| Rule / quyết định · ca | Nội dung ca | Acceptance scenario |
|---|---|---|
| `GR-031` §"Không đổi gì" — `everPublic` đi theo câu | Cờ giữ nguyên qua mọi lần nhập/xuất | AC-005, AC-006 |
| `QĐ-084` — X3 là phép hợp, một chiều | Nhập bản kê chỉ đặt cờ `true`, không bao giờ ngược lại | AC-018 |
| `QĐ-084` — X3 idempotent | Nhập lại cùng file là no-op | AC-016 |
| `QĐ-084` — nhóm thứ ba của bản xem trước không tự áp | Câu không thuộc danh sách gán của contest đích | AC-017 |
| `QĐ-086` vế 3 — sai cụm mật khẩu hoặc gói bị sửa ⇒ từ chối toàn bộ | Không nhập nửa vời | AC-020, AC-021 |
| `QĐ-086` vế 4 — tài khoản admin luôn theo phương án tạo mật khẩu mới | Không phụ thuộc phương án chọn cho vai khác | AC-019 |
| `QĐ-086` vế 5 — chỉ người đã gán vào contest được xuất | Không xuất toàn bộ danh bạ | AC-024 |
| `QĐ-086` vế 6 — trùng tên đăng nhập thì hỏi, không tự nối | Ba lựa chọn | AC-023 |
| `QĐ-086` vế 7 — vai tuỳ biến xuất kèm định nghĩa | Không chỉ xuất tên vai | AC-025 |

> **Ghi chú phủ có chủ đích.** Các ca còn lại của bảng quyết định `GR-031` *(rút đề, kiểm kho tại cửa vào từng vòng, cấu trúc Bộ VCNV)* thuộc EPIC-002 và EPIC-006, đã phủ ở `specs/002-kho-de-bo-de`. Feature này chỉ phủ vế **nhập/xuất và gói contest** mà `GR-031` cùng các quyết định liên quan (`QĐ-084`, `QĐ-086`, `QĐ-087`) đọc, và đó là toàn bộ giao diện giữa EPIC-003 với các nguồn đó.

---

## 11. Assumptions

Các giá trị mặc định dưới đây được chọn cho **chi tiết không quan trọng**; mọi thứ quan trọng mà nguồn không nói đều nằm ở §9.

- **Định dạng đóng gói cụ thể** *(cấu trúc file, cách nén, cách mã hoá)* không được chốt trong spec này — đây là quyết định kỹ thuật thuộc `plan.md`. Spec chỉ chốt **nội dung** gói phải mang gì và **hành vi quan sát được** khi xuất/nhập.
- **Cơ chế mã hoá cụ thể** cho danh sách người tham gia *(thuật toán, độ dài khoá)* không được nguồn nào quy định — spec chỉ yêu cầu *luôn mã hoá, chìa khoá không nằm trong gói, sai cụm thì từ chối toàn bộ*.
- **Nhật ký ở FR-015, FR-026, FR-028** được hiểu là ghi vào cùng nhật ký thao tác chung mà EPIC-011 sở hữu; spec này không đặc tả cấu trúc của nhật ký đó.
- **Ràng buộc kỹ thuật toàn repo** — zero-trust, DRY, chỉ tiếng Việt, dialog xác nhận phía admin cho thao tác không hoàn tác được — áp cho feature này theo `CLAUDE.md`; spec **tham chiếu, không sao chép**.
