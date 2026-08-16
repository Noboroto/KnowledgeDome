# Feature Specification: Vận hành và hai hồ sơ triển khai

**Feature Branch**: `012-van-hanh-hai-ho-so-trien-khai`

**Created**: 2026-08-06

**Status**: Draft

**Input**: EPIC-012 — Vận hành và hai hồ sơ triển khai (`docs/PRD.md` §11 dòng 539-550, §12 mục EPIC-012)

**Nguồn**: `docs/PRD.md` (EPIC-012 §11; `PRD-REQ-085`, `PRD-REQ-106`, `PRD-REQ-098`, `PRD-REQ-086`, `PRD-REQ-087`, `PRD-REQ-107` §12 mục EPIC-012; `PRD-REQ-019` — requirement khai **Related epic** gồm EPIC-012, nằm ở §12 mục EPIC-003; JOURNEY-003, JOURNEY-004 §10; §14 FS-36; §15.1 `NFR-06`, §15.2 `NFR-11`, §15.4 `NFR-20`, §15.6 `NFR-30`, `NFR-31`, `NFR-32`, §15.7 `NFR-35b`, §15.8 `NFR-36`, `NFR-38`; §17 NON-GOAL-006, NON-GOAL-019; §18 `ASSUMPTION-002`, `ASSUMPTION-008`, `RISK-003`, `RISK-007`, `RISK-008`, `RISK-010`; §20 MVP status; §21 chuỗi chặn; ma trận truy nguyên §22) · `docs/game-rules.md` (`GR-037` ở vế **cấu trúc kênh public**; `GR-036` qua JOURNEY-004) · `docs/game-state-machine.md` (không state, không transition nào thuộc feature này — xem §Phạm vi) · `docs/decisions.md` (`QĐ-067`, `QĐ-084`, `QĐ-086`, `QĐ-087`, `QĐ-088`, `QĐ-089`, `QĐ-091`, `QĐ-096`, `QĐ-102`, `QĐ-128`, `QĐ-129`, `QĐ-148`) · `docs/permissions.md` (`PERM-049` nút khoá cổng; §8 kênh public) · `docs/glossary.md` (`TERM-007`, `TERM-015`, `TERM-048`, `TERM-049`) · `docs/traceability.md` · `.specify/memory/constitution.md` bản 1.4.0 · `CLAUDE.md` §Phạm vi phiên bản, §Kiến trúc và bảo mật, §UX, §Quy ước code.

## Phạm vi

EPIC-012 phủ **mọi thứ làm cho cùng một sản phẩm chạy được ở hai nơi rất khác nhau**: hồ sơ máy chủ dựng bằng container, hồ sơ portable chạy LAN offline trên Windows, tính đầy đủ của bản portable khi không có Internet, danh sách **năm hiện vật** phải xuất được ở cả hai hồ sơ, các **ngưỡng vận hành cấu hình được**, và **tài liệu vận hành** nói thẳng những ca mà sản phẩm cố ý không tự giải.

Feature này sở hữu **một trục duy nhất — hồ sơ triển khai**, không sở hữu một bề mặt nào của trận. Mọi requirement ở đây trả lời câu *"tính chất này có đúng ở CẢ HAI hồ sơ không"* và *"con số này đặt ở đâu"*, không trả lời câu *"tính năng này làm gì"*. Hệ quả kiểm được: **0** state và **0** transition của máy trạng thái trận thuộc feature này, và **0** transition nào đọc một giá trị của nó (FR-030).

Actor chính: **ACTOR-001 admin** *(và người cài đặt — `docs/PRD.md` §11 khai hai vai này đi cùng nhau ở epic này)*. Journey: **JOURNEY-003 — Chuyển sang bản portable** và **JOURNEY-004 — Vào phòng**.

**Mục tiêu của epic** (`docs/PRD.md` §11): *"Cùng một sản phẩm chạy được ở phòng máy chủ và ở hội trường không có Internet"*, với **user value**: *"Giải `PS-2` — không bị khoá vào LAN, cũng không phụ thuộc Internet ngày thi"*.

**Epic này là NHÁNH SONG SONG sau EPIC-002** (`docs/PRD.md` §21 chuỗi chặn), không nằm trên chuỗi chặn của trận. Nó phụ thuộc **EPIC-003** — vì gói contest là vật mang duy nhất đưa một contest sang bản portable, và tài khoản admin ngày thi đi trong chính gói đó.

**MVP status: v1** (`docs/PRD.md` §20). Toàn bộ epic thuộc v1, không có mục nào hoãn.

**Không thuộc phạm vi feature này** (xem §Out of Scope): hành vi của nút khoá cổng và của giới hạn tần suất (EPIC-001); hai đường tạo tài khoản admin đầu tiên (EPIC-001); cấu trúc một chiều của kênh public (EPIC-001, EPIC-009); cơ chế xuất/nhập gói contest và cơ chế ngưỡng kích thước media (EPIC-003); nội dung và định dạng của biên bản, gói kết quả, bản kê câu đã dùng, gói rút gọn (EPIC-003, EPIC-010); hạn lưu trữ và nhật ký thao tác (EPIC-011); nhiều tổ chức trên một bản cài (NON-GOAL-006); trình hướng dẫn cài đặt trên trình duyệt (NON-GOAL-019).

## User Scenarios & Testing *(mandatory)*

### US-001 — Cùng một sản phẩm, hai hồ sơ triển khai (Priority: P1)

- **Title**: Bản portable không phải một bản rút gọn — nó là cùng sản phẩm ở một chỗ khác
- **Actor**: ACTOR-001 admin *(và người cài đặt, ở vai người dựng máy)*
- **Intent**: Triển khai sản phẩm theo hồ sơ phù hợp với chỗ mình có — máy chủ dựng bằng container khi có phòng máy chủ, bản portable chạy LAN khi ra hội trường — mà **không** phải chấp nhận một tập tính năng hẹp hơn ở hồ sơ thứ hai.
- **User value**: Đây là toàn bộ `PS-2`. Một sản phẩm chỉ chạy trên máy chủ thì khoá đơn vị tổ chức vào một đường mạng mà hội trường thường không có; một sản phẩm chỉ chạy LAN thì khoá họ vào một cái máy. Hai hồ sơ ngang tính năng là cách duy nhất không phải chọn giữa hai chỗ hỏng đó.
- **Priority**: P1
- **Priority rationale**: `PRD-REQ-085` là P1 với lý do *"ngày thi ở hội trường thường không có mạng ổn định; đây là điều kiện để giải `PS-2` mà không quay về mô hình chỉ-LAN"*. Nó cũng là **tiền đề** của năm slice còn lại: US-002 chạy trên bản portable, US-003 đếm hiện vật *ở cả hai hồ sơ*, US-004 đặt ngưỡng *theo từng hồ sơ*. Không có hai hồ sơ thì năm slice kia không có chủ ngữ.
- **Independent test**: Dựng **cả hai** hồ sơ từ cùng một bản phát hành. Trên hồ sơ portable, **ngắt Internet ở máy chủ**, rồi chạy hết một danh sách bề mặt kiểm được: đăng nhập mọi vai, dựng và sửa contest, mở phòng, cho khán giả và lớp phủ vào bằng đường dẫn, chạy trọn một trận đủ năm vòng, xuất đủ **năm** hiện vật. Xác nhận **0** bề mặt nào từ chối vì thiếu mạng và **0** bề mặt nào chỉ có ở hồ sơ kia.
- **Related PRD requirements**: `PRD-REQ-085`
- **Related game rules**: — *(EPIC-012 khai `Related game rules: —`)* · `NFR-30`, `NFR-31`, `NFR-32`
- **Related journey**: JOURNEY-003

---

### US-002 — Ngày thi ở hội trường: nhập gói là dùng được (Priority: P1)

- **Title**: Người vận hành ngày thi là giáo viên trước giờ phát sóng, không phải người dựng máy
- **Actor**: ACTOR-001 admin *(ở vai người vận hành ngày thi)*
- **Intent**: Mang gói contest sang bản portable, nhập nó, rồi **bắt đầu làm việc ngay** — đăng nhập bằng tài khoản admin đã đi trong gói, kiểm lại, chạy trận — mà không phải mở một cửa sổ dòng lệnh nào ở hội trường.
- **User value**: `QĐ-087` chỉ đúng chỗ hỏng thật: *"bắt người thứ hai làm việc của người thứ nhất mới là chỗ hỏng của `PRD-REQ-085` — không phải bản thân việc dùng dòng lệnh"*. Slice này là chỗ mà hai hồ sơ triển khai **trở nên dùng được bởi người thật**.
- **Priority**: P1
- **Priority rationale**: `PRD-REQ-106` là P1. Nó cũng là **phép kiểm nghiệm thu mạnh nhất** của `PRD-REQ-085` — `QĐ-087` §Hệ quả khai thẳng rằng phép kiểm của `PRD-REQ-085` *"mạnh lên"* nhờ ràng buộc **không có bước dòng lệnh nào ở giữa**. Một sản phẩm thoả `PRD-REQ-085` mà trượt slice này thì vẫn hỏng đúng ngày thi.
- **Independent test**: Trên một bản cài portable **trống**, đã ngắt Internet: nhập một gói contest **có kèm danh sách người tham gia**, rồi đăng nhập bằng tài khoản admin dựng lại từ gói và chạy trọn một trận tới lúc xuất biên bản. Đếm số lần phải mở dòng lệnh trong toàn chuỗi và xác nhận bằng **0**. Sau đó lặp lại với một gói **không kèm** danh sách trên một bản cài trống khác và xác nhận ca đó **vẫn cần** dòng lệnh — và rằng ca đó được tài liệu vận hành nêu thẳng (US-006).
- **Related PRD requirements**: `PRD-REQ-106`, `PRD-REQ-085`
- **Related game rules**: — · `NFR-30`, `NFR-32`
- **Related journey**: JOURNEY-003, JOURNEY-004

---

### US-003 — Năm hiện vật xuất được ở cả hai hồ sơ (Priority: P2)

- **Title**: Bản portable là nơi trận thật chạy — dữ liệu của nó không được mắc kẹt trong máy
- **Actor**: ACTOR-001 admin
- **Intent**: Sau một trận chạy trên bản portable đã ngắt Internet, xuất được **đủ năm** hiện vật — gói contest, gói toàn bộ kết quả và nhật ký sự kiện, bản kê câu đã dùng, gói kết quả rút gọn, **và biên bản trận** — với nội dung đầy đủ y như trên hồ sơ máy chủ.
- **User value**: `PRD-REQ-098` nêu rationale trực tiếp: *"bản portable là nơi trận thật chạy; nếu nó xuất được ít hơn thì dữ liệu của chính những trận quan trọng nhất bị mắc kẹt trong máy"*. `QĐ-129` chỉ ra hệ quả **một chiều**: gói kết quả chỉ có chiều ra, nên trận chạy trên portable mà không xuất được biên bản tại chỗ thì **không bao giờ** có biên bản ở bất kỳ đâu.
- **Priority**: P2
- **Priority rationale**: `PRD-REQ-098` là P2 — trận **vẫn chạy được** nếu chưa xuất được, nên nó không phải P1 theo định nghĩa của `docs/PRD.md` §12. Nhưng nó phải vào v1 vì chỗ hỏng là **không hồi phục được**: hiện vật không xuất tại chỗ thì không có đường thứ hai để lấy ra sau.
- **Independent test**: Chạy một trận trọn vẹn trên bản portable đã ngắt Internet, sau đó xuất **cả năm** hiện vật và đối chiếu từng cái với hiện vật cùng loại xuất từ một trận tương đương trên hồ sơ máy chủ. Xác nhận **0** hiện vật nào vắng mặt, **0** hiện vật nào nghèo nội dung hơn, và **0** lần gọi nào cần một dịch vụ ngoài.
- **Related PRD requirements**: `PRD-REQ-098`, `PRD-REQ-085`
- **Related game rules**: — · `NFR-30`
- **Related journey**: JOURNEY-003, JOURNEY-007 *(`PRD-REQ-098` khai `Related journey: JOURNEY-003, JOURNEY-007`)*

---

### US-004 — Ngưỡng vận hành đặt theo hồ sơ triển khai (Priority: P2)

- **Title**: Một laptop ở hội trường và một máy chủ trong phòng máy không chịu được cùng một con số
- **Actor**: ACTOR-001 admin *(và người cài đặt)*
- **Intent**: Đặt các ngưỡng vận hành — kích thước media tải lên, tần suất vào cổng khán giả, quy mô viewer và ngưỡng độ trễ — bằng **cấu hình của bản triển khai**, khác nhau được giữa hồ sơ máy chủ và hồ sơ portable, mà không phải sửa code và không phải dựng hai bản phát hành.
- **User value**: Phần cứng của hai hồ sơ khác nhau về hạng. `PRD-REQ-019` nêu thẳng: *"bản portable chạy trên máy tính xách tay; media không giới hạn làm gói contest không mang đi được"*. Một con số cứng dùng chung sẽ hoặc bóp nghẹt hồ sơ máy chủ, hoặc làm hồ sơ portable sập giữa trận.
- **Priority**: P2
- **Priority rationale**: `PRD-REQ-019`, `PRD-REQ-086` và `PRD-REQ-087` đều là P2 — một trận **vẫn chạy được** nếu ngưỡng chưa được tinh chỉnh, vì mọi ngưỡng đều có mặc định. Nhưng chúng phải vào v1 vì đây là **hình dạng** của cấu hình: dựng sai hình dạng ở v1 thì mọi chỗ đọc con số phải sửa lại sau.
- **Independent test**: Trên **cả hai** hồ sơ, đọc ra giá trị đang áp của từng ngưỡng, đổi từng ngưỡng bằng cấu hình triển khai và xác nhận giá trị mới có hiệu lực **mà không sửa một dòng code nào**. Sau đó đặt hai hồ sơ ở hai giá trị **khác nhau** cho cùng một ngưỡng và xác nhận cả hai chạy đúng giá trị của mình. Cuối cùng rà toàn bộ sản phẩm và xác nhận **0** chỗ nào viết cứng một con số thuộc ba nhóm này.
- **Related PRD requirements**: `PRD-REQ-019`, `PRD-REQ-086`, `PRD-REQ-087`
- **Related game rules**: — · `NFR-06`
- **Related journey**: JOURNEY-004 *(cổng khán giả)*; JOURNEY-001 *(ngưỡng media — `PRD-REQ-019` khai `Related journey: JOURNEY-001`)*

---

### US-005 — Con số vận hành không được lọt vào luật (Priority: P2)

- **Title**: Một giá trị định cỡ biến thành một cam kết là lúc nó lọt vào một rule
- **Actor**: ACTOR-001 admin *(người hưởng lợi)*; chủ thể bị ràng buộc là **chính đặc tả**
- **Intent**: Giữ cho mọi con số vận hành đứng **ngoài** luật chơi và ngoài máy trạng thái trận: không rule nào đọc, không transition nào đọc, không đặc tả nào được viết dựa trên một con số cụ thể — và mục tiêu định cỡ số trận song song **không** biến thành một chặn cứng.
- **User value**: `QĐ-067` nêu cái giá: *"chốt bừa một con số đắt hơn là để trống: nó biến một giá trị cấu hình thành một cam kết, và cam kết sai thì phải viết lại cả mục tiêu kiểm thử tải"*. `QĐ-089` nêu cái giá của chiều ngược lại: một hạn ngạch thêm *"một ca hỏng mới — từ chối mở trận vì lý do không liên quan tới luật chơi — để đổi lấy đúng con số không"*.
- **Priority**: P2
- **Priority rationale**: `PRD-REQ-087` là P2. Slice này tách khỏi US-004 vì nó là một **ràng buộc nghịch** — nó nói về những gì hệ thống **MUST NOT** làm với con số, và nó kiểm thử được bằng một phép rà đặc tả và một phép thử vượt ngưỡng, **không** cần bề mặt cấu hình của US-004 tồn tại.
- **Independent test**: Rà **100%** đặc tả luật `GR-001` → `GR-037` và **100%** bảng chuyển trạng thái, xác nhận **0** chỗ nào đọc một con số vận hành. Sau đó mở **trận thứ bảy** đồng thời trên một bản cài và xác nhận hệ thống **không** đếm và **không** từ chối. Cuối cùng, tăng số khán giả trong một phòng lên nhiều lần và xác nhận thứ tự chuông, đồng hồ và thứ hạng tốc độ **không đổi**.
- **Related PRD requirements**: `PRD-REQ-087`, `PRD-REQ-107`
- **Related game rules**: `GR-037` *(chỉ ở vế cấu trúc kênh public)* · `NFR-06`, `NFR-11`, `NFR-20`
- **Related journey**: JOURNEY-004

---

### US-006 — Tài liệu vận hành nói thẳng những gì sản phẩm không tự giải (Priority: P2)

- **Title**: Ranh giới đã chấp nhận chỉ an toàn khi người vận hành biết nó tồn tại
- **Actor**: ACTOR-001 admin *(và người cài đặt, ở vai người đọc)*
- **Intent**: Có một tài liệu vận hành nêu **thẳng** những ca mà sản phẩm cố ý không tự giải — ca còn phải dùng dòng lệnh tại hội trường, việc đường dẫn phòng phát ra thì không thu lại được, việc gói đã xuất nằm ngoài tầm với của hạn lưu trữ — để người vận hành xử lý chúng bằng quy trình thay vì phát hiện lúc đang hỏng.
- **User value**: Ba ranh giới này đều là **quyết định có chủ đích**, không phải chỗ thiếu đặc tả. Nhưng một ranh giới đã chấp nhận mà người vận hành không biết thì hành xử **y hệt** một lỗi. `docs/PRD.md` phát biểu cả ba bằng `MUST` đặt lên tài liệu vận hành, không đặt lên phần mềm.
- **Priority**: P2
- **Priority rationale**: Trận **vẫn chạy được** nếu tài liệu chưa có, nên không phải P1. Nhưng nó không phải P3: ca *"bản cài trống và gói không kèm danh sách"* là **ca duy nhất còn lại có thể xảy ra ở hội trường** (`PRD-REQ-106` §Note), và người phát hiện nó lúc 19h ngày phát sóng là người không chuẩn bị được gì. Đây đúng hình dạng P2 — cần, nhưng trận không dừng vì thiếu nó.
- **Independent test**: Rà tài liệu vận hành đi kèm bản phát hành và xác nhận nó nêu **đủ ba** ca: ca còn phải dùng dòng lệnh, tính chất không thu hồi được của đường dẫn phòng, và việc gói đã xuất nằm ngoài hạn lưu trữ nên phải xoá sau ngày thi. Với mỗi ca, kiểm rằng tài liệu nói **hệ quả** chứ không chỉ nhắc tên.
- **Related PRD requirements**: `PRD-REQ-106`, `PRD-REQ-001` *(§Note — tính chất không thu hồi được của đường dẫn phòng)*, `PRD-REQ-085`
- **Related game rules**: — · `NFR-35b`
- **Related journey**: JOURNEY-003, JOURNEY-004

---

## Requirements *(mandatory)*

### Functional Requirements

#### Nhóm A — Hai hồ sơ triển khai (US-001)

- **FR-001**: Sản phẩm MUST triển khai được theo **đúng hai** hồ sơ: một **hồ sơ máy chủ dựng bằng container**, và một **hồ sơ portable chạy LAN offline trên Windows**. *(US-001 · `PRD-REQ-085` · — · AC-001)*
- **FR-002**: Hồ sơ portable MUST hoạt động **đầy đủ** khi máy chủ **không có kết nối Internet**: mọi bề mặt của sản phẩm MUST dùng được, và **0** bề mặt nào MUST từ chối một thao tác vì lý do thiếu kết nối ra ngoài. *(US-001 · `PRD-REQ-085` · `NFR-30` · AC-001, AC-002)*
- **FR-003**: Hai hồ sơ MUST cùng phủ **một** tập tính năng: **0** tính năng nào của sản phẩm MUST chỉ tồn tại ở một hồ sơ. *(US-001 · `PRD-REQ-085` · `NFR-30` · AC-002, AC-003)*
- **FR-004**: Tính chất **không có đường ghi** của hai kênh public MUST đúng như nhau ở **cả hai** hồ sơ; hệ thống MUST NOT mở một đường ghi ở một hồ sơ để bù cho một ràng buộc triển khai. *(US-001, US-005 · `PRD-REQ-107` · `GR-037` *(vế cấu trúc kênh)*, `NFR-20` · AC-004)*
- **FR-005**: Hồ sơ triển khai MUST NOT đổi kết cục của bất kỳ phép quyết định nào của luật chơi: cùng một chuỗi thao tác trên cùng một cấu hình MUST cho cùng một bảng điểm và cùng một thứ hạng ở cả hai hồ sơ. *(US-001 · `PRD-REQ-085` · `NFR-28` · AC-003, AC-005)*
- **FR-006**: `v1` MUST NOT có luồng nào chạy mà không có admin, ở **cả hai** hồ sơ — hồ sơ portable MUST NOT được coi là ca miễn trừ. *(US-001 · `PRD-REQ-085` · `NFR-32` · AC-006)*
- **FR-007**: Một bản cài MUST phục vụ **một** tổ chức; hệ thống MUST NOT có bề mặt nào cho nhiều tổ chức trên cùng một bản cài, ở cả hai hồ sơ. *(US-001 · `PRD-REQ-085` *(NON-GOAL-006)* · — · AC-007)*
- **FR-008**: Ranh giới **server sập là mất khả năng cứu** MUST được coi là ranh giới **đã chấp nhận** ở cả hai hồ sơ; hệ thống MUST NOT khai một cơ chế cứu ở hồ sơ này mà không có ở hồ sơ kia. *(US-001 · `PRD-REQ-085` · `NFR-31`, `RISK-009` · AC-008)*

#### Nhóm B — Ngày thi trên bản portable (US-002)

- **FR-009**: Trên một bản cài **trống**, việc **nhập một gói contest có kèm danh sách người tham gia** MUST làm bản cài đó dùng được ngay — tài khoản admin của contest đăng nhập được — **mà không** cần một thao tác dòng lệnh nào sau bước nhập. *(US-002 · `PRD-REQ-106` · — · AC-009, AC-010)*
- **FR-010**: Chuỗi *nhập gói → đăng nhập admin → chạy trọn một trận → xuất hiện vật* MUST hoàn thành được trên bản portable **đã ngắt Internet**, với **0** bước dòng lệnh ở giữa. *(US-002 · `PRD-REQ-106`, `PRD-REQ-085` · `NFR-30` · AC-010, AC-011)*
- **FR-011**: Ca **bản cài trống cộng gói KHÔNG kèm danh sách người tham gia** MUST vẫn cần đường dòng lệnh; hệ thống MUST NOT dựng một đường thứ ba để lấp ca này ở v1. *(US-002 · `PRD-REQ-106` · — · AC-012)*
- **FR-012**: `v1` MUST NOT có trình hướng dẫn cài đặt lần đầu chạy trên trình duyệt, ở **cả hai** hồ sơ: **0** bề mặt nào của một bản cài trống MUST cho tạo một tài khoản toàn quyền mà không xác thực. *(US-002 · `PRD-REQ-106` *(NON-GOAL-019)* · — · AC-013)*
- **FR-013**: Nhập **lại** cùng một gói vào cùng một bản cài MUST NOT làm bản cài rơi vào trạng thái không đăng nhập được: kết cục MUST là một kết cục đã được `specs/003` đặc tả *(nối, tạo mới có hậu tố, hoặc huỷ)*, và MUST NOT là mất đường vào của tài khoản admin đang dùng. *(US-002 · `PRD-REQ-106` · — · AC-014)*
- **FR-014**: Một lần nhập **bị từ chối** *(sai cụm mật khẩu, gói bị sửa)* MUST để bản cài **nguyên trạng**: **0** tài khoản nào được dựng, **0** contest nào được tạo, và bản cài trống MUST vẫn là bản cài trống. *(US-002 · `PRD-REQ-106` · `NFR-24b` · AC-015)*

#### Nhóm C — Năm hiện vật ở cả hai hồ sơ (US-003)

- **FR-015**: Cả hai hồ sơ triển khai MUST xuất được **đủ năm** hiện vật: **(1)** gói contest · **(2)** gói toàn bộ kết quả và nhật ký sự kiện · **(3)** bản kê câu đã dùng · **(4)** gói kết quả rút gọn · **(5)** **biên bản trận**. *(US-003 · `PRD-REQ-098` · `QĐ-084`, `QĐ-129` · AC-016, AC-017)*
- **FR-016**: Việc xuất **cả năm** hiện vật MUST NOT phụ thuộc một dịch vụ ngoài và MUST NOT phụ thuộc kết nối Internet. *(US-003 · `PRD-REQ-098` · `NFR-30` · AC-016, AC-018)*
- **FR-017**: Nội dung của mỗi hiện vật xuất trên hồ sơ portable MUST đầy đủ **như** hiện vật cùng loại xuất trên hồ sơ máy chủ; hệ thống MUST NOT có biến thể nghèo nội dung hơn theo hồ sơ. *(US-003 · `PRD-REQ-098` · `QĐ-129` · AC-017)*
- **FR-018**: Danh sách năm hiện vật ở FR-015 MUST là danh sách **đầy đủ** các hiện vật chịu ràng buộc *"có ở cả hai hồ sơ"*; khi một hiện vật mới được thêm vào sản phẩm, nó MUST được thêm vào **danh sách này** chứ không chỉ vào spec sở hữu nó. *(US-003 · `PRD-REQ-098` · `QĐ-129` · AC-019)*
- **FR-019**: Ràng buộc *"không có đường mang **kết quả** từ portable về máy chủ trung tâm"* MUST được giữ nguyên là **ranh giới đã chấp nhận**; feature này MUST NOT mở một đường đồng bộ nào. Thứ duy nhất quay ngược được là **bản kê câu đã dùng**. *(US-003 · `PRD-REQ-098` · `QĐ-084`, `QĐ-129` §Không đổi gì · AC-020)*
- **FR-020**: Gói đã rời hệ thống MUST được hiểu là nằm **ngoài** tầm với của hạn lưu trữ, ở cả hai hồ sơ; hệ thống MUST NOT khai một thực thể nào theo dõi vòng đời của một hiện vật đã xuất. *(US-003, US-006 · `PRD-REQ-098` · `NFR-35b`, `QĐ-128` · AC-021)*

#### Nhóm D — Ngưỡng vận hành cấu hình được (US-004)

- **FR-021**: Ngưỡng kích thước media MUST là **một bộ giá trị duy nhất** ở phạm vi **bản cài**, một ngưỡng riêng cho mỗi loại media *(ảnh · video · âm thanh)*. Hệ thống MUST NOT tách ngưỡng theo hồ sơ triển khai: hai hồ sơ MUST dùng **cùng** bộ mặc định, vì gói contest đi **từ** hồ sơ máy chủ **sang** hồ sơ portable, nên một ngưỡng rộng hơn ở đầu này sẽ tạo ra đúng những gói đầu kia không nhận được. *(US-004 · `PRD-REQ-019` · `QĐ-159` · AC-022, AC-023)*
- **FR-022**: Ngưỡng **giới hạn tần suất** của cổng khán giả MUST là một giá trị cấu hình ở phạm vi **bản cài**. Hai bản cài MUST đặt được hai giá trị khác nhau — ngưỡng này phụ thuộc sức chứa của chính máy đang chạy và **không** đi theo gói contest, nên nó **không** chịu ràng buộc dùng chung của FR-021. *(US-004 · `PRD-REQ-086`, `PRD-REQ-087` · `QĐ-067` §Hệ quả · AC-022, AC-024)*
- **FR-023**: **Mục tiêu quy mô viewer** và **ngưỡng độ trễ** MUST là mặc định cấu hình được ở phạm vi **bản cài**, đặt được khác nhau giữa hai bản cài vì cùng lý do với FR-022. *(US-004 · `PRD-REQ-087` · `NFR-06`, `QĐ-067` · AC-022, AC-024)*
- **FR-024**: Mọi ngưỡng ở FR-021 → FR-023 MUST đổi được **mà không sửa code** và **không** dựng lại một bản phát hành khác. *(US-004 · `PRD-REQ-019`, `PRD-REQ-087` · `CLAUDE.md` §Quy ước code · AC-023, AC-025)*
- **FR-025**: Mọi ngưỡng ở FR-021 → FR-023 MUST NOT được viết cứng ở bất kỳ đâu trong sản phẩm — không trong logic, không trong giao diện, và không trong thông điệp từ chối. *(US-004, US-005 · `PRD-REQ-019` §Note, `PRD-REQ-087` · `CLAUDE.md` §Quy ước code · AC-026)*
- **FR-026**: Thông điệp từ chối vì vượt ngưỡng MUST đọc ngưỡng **đang áp** từ cấu hình của chính bản triển khai đó. *(US-004 · `PRD-REQ-019` · — · AC-026, AC-027)*
- **FR-027**: Một bản triển khai **không khai** giá trị cho một ngưỡng MUST chạy bằng **mặc định** của ngưỡng đó; hệ thống MUST NOT từ chối khởi chạy vì thiếu một giá trị ngưỡng, và MUST NOT chạy **không** ngưỡng. *(US-004 · `PRD-REQ-019`, `PRD-REQ-087` · — · AC-028)*
- **FR-028**: Đổi một ngưỡng MUST NOT làm đổi dữ liệu đã có: media đã tải lên trước khi ngưỡng bị hạ MUST vẫn dùng được, và **0** hiện vật đã xuất nào bị đụng tới. *(US-004 · `PRD-REQ-019` · — · AC-029)*

#### Nhóm E — Con số là định cỡ, không phải luật (US-005)

- **FR-029**: **0** rule, **0** transition và **0** đặc tả nào MUST được viết dựa trên một con số viewer đồng thời hay một ngưỡng độ trễ cụ thể. *(US-005 · `PRD-REQ-087` · `NFR-06`, `QĐ-067` · AC-030)*
- **FR-030**: **0** rule và **0** transition nào MUST đọc một ngưỡng của Nhóm D hay mục tiêu định cỡ của FR-031. *(US-005 · `PRD-REQ-087` · `NFR-06`, `NFR-11` · AC-030, AC-031)*
- **FR-031**: Mỗi hồ sơ triển khai MUST có mục tiêu **định cỡ và kiểm thử tải** riêng về số trận chạy đồng thời: **6 trận** cho **hồ sơ máy chủ dựng bằng container**, **1 trận** cho **hồ sơ portable**. Hệ thống MUST NOT dùng một con số chung cho hai hồ sơ. *(US-005 · `PRD-REQ-087` · `NFR-11`, `QĐ-089`, `QĐ-148` · AC-031, AC-031a)*
- **FR-031a**: Cả hai con số ở FR-031 MUST là **mục tiêu vận hành**, MUST NOT là chặn cứng: hệ thống MUST NOT đếm số trận đang chạy và MUST NOT từ chối một trận vì vượt mục tiêu — ở **cả hai** hồ sơ. Mở trận thứ **hai** trên hồ sơ portable MUST thành công như mở trận thứ **bảy** trên hồ sơ máy chủ: trận mới mở bình thường, **0** cảnh báo hạn ngạch phát ra, và các trận đang chạy **không bị ảnh hưởng**. *(US-005 · `PRD-REQ-087` · `NFR-11`, `QĐ-089`, `QĐ-148` · AC-031, AC-031a, AC-032)*
- **FR-032**: Số khán giả trong một phòng MUST NOT chạm tới thứ tự chuông, đồng hồ hay thứ hạng tốc độ. *(US-005 · `PRD-REQ-087`, `PRD-REQ-107` · `NFR-06`, `NFR-20`, `QĐ-088` · AC-033)*
- **FR-033**: Mỗi người xem MUST được hiểu là vẫn giữ **một kết nối mở**; hệ thống MUST NOT đặc tả quy mô viewer như một chi phí bằng không. *(US-005 · `PRD-REQ-087` §Note · `QĐ-088` · AC-034)*
- **FR-034**: Hai mục tiêu định cỡ ở FR-031 MUST NOT trở thành chỗ chặn cứng thứ tư. Tập chặn cứng của sản phẩm MUST giữ nguyên **đúng ba** chỗ ở cả hai hồ sơ — cửa sổ cướp quyền Về đích **≥2** người, Câu hỏi phụ **≥2** người, và **cửa vào vòng thiếu câu** — và số trận đang chạy MUST NOT là một chỗ thứ tư. *(US-005 · `PRD-REQ-087` · `NFR-11`, `QĐ-089`, `INV-014` · AC-032)*

#### Nhóm F — Tài liệu vận hành (US-006)

- **FR-035**: Bản phát hành MUST đi kèm **tài liệu vận hành**, và tài liệu đó MUST nêu **thẳng** ca **bản cài trống cộng gói không kèm danh sách người tham gia vẫn phải dùng dòng lệnh** — kèm hệ quả rằng đây là ca duy nhất còn lại có thể xảy ra ở hội trường. *(US-006 · `PRD-REQ-106` §Note · — · AC-035)*
- **FR-036**: Tài liệu vận hành MUST nói thẳng rằng **đường dẫn phòng phát ra thì không thu lại được**, và rằng hàng rào còn lại chỉ gồm giới hạn tần suất và nút khoá cổng. *(US-006 · `PRD-REQ-001` §Note, `PRD-REQ-086` · `QĐ-096`, `RISK-007` · AC-036)*
- **FR-037**: Tài liệu vận hành MUST nhắc **xoá gói đã xuất sau ngày thi**, gồm cả gói mang danh sách người tham gia, vì gói đã xuất nằm ngoài tầm với của hạn lưu trữ. *(US-006 · `PRD-REQ-098` · `NFR-35b`, `RISK-010` · AC-037)*
- **FR-038**: Ba mục ở FR-035 → FR-037 MUST nêu **hệ quả**, MUST NOT chỉ nhắc tên ca. *(US-006 · `PRD-REQ-106` §Note · — · AC-035, AC-036, AC-037)*
- **FR-039**: Tài liệu vận hành MUST đi kèm **cả hai** hồ sơ triển khai; hệ thống MUST NOT phát hành hồ sơ portable mà không kèm tài liệu đó. *(US-006 · `PRD-REQ-085`, `PRD-REQ-106` · — · AC-038)*
- **FR-040**: Tài liệu vận hành MUST NOT được coi là nguồn của một requirement nghiệp vụ; nó MUST chỉ **nói lại** những ranh giới mà `docs/` đã khai. *(US-006 · `PRD-REQ-106` · `.specify/memory/constitution.md` §I · AC-039)*

#### Nhóm G — Ràng buộc xuyên suốt

- **FR-041**: Mọi bề mặt của feature này MUST hiển thị **trạng thái đang xử lý** cho thao tác bất đồng bộ — đặc biệt là nhập gói và xuất hiện vật, hai thao tác dài nhất của epic. *(mọi US · `NFR-36`, `CLAUDE.md` §UX · AC-040)*
- **FR-042**: Mọi từ chối của feature này MUST NOT làm đổi bất kỳ dữ liệu nào. *(mọi US · `NFR-18` · AC-015, AC-027, AC-029)*
- **FR-043**: Mọi chuỗi giao diện của feature này MUST tách ra file hằng số và MUST chỉ dùng **tiếng Việt**; mọi mốc thời gian hiển thị MUST theo **UTC+7**, ở cả hai hồ sơ. *(mọi US · `NFR-38`, `CLAUDE.md` §Quy ước code · AC-041)*
- **FR-044**: Hai hồ sơ MUST dựng từ **cùng một** bản phát hành nguồn; hệ thống MUST NOT duy trì hai nhánh sản phẩm song song. *(US-001 · `PRD-REQ-085` · `CLAUDE.md` §Quy ước code *(DRY)* · AC-003, AC-042)*

### Key Entities

- **Hồ sơ triển khai**: một cách dựng **cùng một** sản phẩm. Đúng **hai** giá trị: **hồ sơ máy chủ dựng bằng container** và **hồ sơ portable chạy LAN offline trên Windows**. Là tính chất **của một bản cài**, không phải của một contest hay một trận — nên **0** state và **0** transition nào của máy trạng thái trận đọc nó. Nó **không** đổi tập tính năng (FR-003) và **không** đổi kết cục của luật chơi (FR-005); nó chỉ đổi **giá trị của các ngưỡng vận hành** (Nhóm D). *(`docs/glossary.md` **chưa có** `TERM-*` cho khái niệm này — xem `OQ-003`.)*
- **Ngưỡng vận hành**: một giá trị cấu hình ở mức **bản triển khai**. Ba nhóm: **kích thước media** *(một ngưỡng riêng cho mỗi loại — ảnh, video, âm thanh)*, **giới hạn tần suất cổng khán giả**, và **quy mô viewer cùng ngưỡng độ trễ**. Cả ba mang cùng một hình dạng: có mặc định, đổi được không sửa code, **không** hard-code, và **0** rule hay transition nào đọc. Khác hẳn **RuleConfig** — thứ đó là giá trị **luật**, đóng băng theo trận, và engine đọc.
- **Mục tiêu định cỡ**: một con số dùng để **chọn phần cứng và dựng bài kiểm thử tải**, không dùng để từ chối gì. **Mỗi hồ sơ một giá trị**: **6 trận** chạy đồng thời cho hồ sơ máy chủ *(trường hợp thường trực 1-2)*, **1 trận** cho hồ sơ portable — một hội trường chạy một trận. Nó **không** là một hạn ngạch ở bất kỳ hồ sơ nào: hệ thống không đếm và không chặn, nên trận thứ hai trên portable vẫn mở được.
- **Năm hiện vật xuất được**: tập đóng các tệp phải xuất được ở **cả hai** hồ sơ — gói contest · gói toàn bộ kết quả và nhật ký sự kiện · bản kê câu đã dùng · gói kết quả rút gọn · biên bản trận. Nội dung và định dạng của từng cái thuộc `specs/003` và `specs/010`; ở đây chúng là **các phần tử của một danh sách phải đầy đủ**. Sau khi rời hệ thống, mỗi cái trở thành **hiện vật đã xuất** — nằm ngoài tầm với của hạn lưu trữ và không phải thực thể hệ thống theo dõi.
- **Tài liệu vận hành**: hiện vật **đi kèm bản phát hành**, không phải một bề mặt của sản phẩm. Chứa đúng ba mục bắt buộc của Nhóm F. **Không** là nguồn của requirement nghiệp vụ nào (FR-040).

## Acceptance Scenarios *(mandatory)*

**AC-001 — Happy path: bản portable chạy đầy đủ khi ngắt Internet**
- *US*: US-001 · *FR*: FR-001, FR-002 · *PRD*: `PRD-REQ-085` · *GR*: —
- **Given** một bản cài theo **hồ sơ portable**, dựng từ bản phát hành hiện hành, đã có một contest đầy đủ đề và một trận ở `LOBBY`; máy chủ portable **đã ngắt kết nối Internet**; bốn ghế thí sinh, một khán giả và một lớp phủ đều ở trong cùng mạng LAN
- **When** admin chạy hết chuỗi bề mặt kiểm được: đăng nhập mọi vai · sửa contest · mở phòng · chạy trọn năm vòng · chốt trận · xuất năm hiện vật
- **Then** **100%** thao tác hoàn thành · **0** thao tác nào bị từ chối vì thiếu kết nối ra ngoài · bảng điểm và thứ hạng cuối được tính đủ · năm hiện vật xuất ra đủ · **0** dữ liệu nào rời mạng LAN

**AC-002 — Ngang tính năng: không bề mặt nào chỉ có ở một hồ sơ**
- *US*: US-001 · *FR*: FR-002, FR-003 · *PRD*: `PRD-REQ-085` · *GR*: —
- **Given** hai bản cài dựng từ **cùng một** bản phát hành, một theo hồ sơ máy chủ và một theo hồ sơ portable; cùng dữ liệu contest được đưa vào cả hai
- **When** người kiểm rà toàn bộ danh mục bề mặt của sản phẩm trên cả hai bản cài
- **Then** hai danh mục **trùng nhau** · **0** bề mặt nào chỉ có ở một bên · **0** bề mặt nào ở hồ sơ portable hiện trạng thái *"không khả dụng ở hồ sơ này"*

**AC-003 — Cùng đầu vào, cùng kết cục ở hai hồ sơ**
- *US*: US-001 · *FR*: FR-003, FR-005, FR-044 · *PRD*: `PRD-REQ-085` · *GR*: —
- **Given** cùng một contest với cùng cấu hình luật đã đóng băng, cùng danh sách câu, và một kịch bản thao tác admin **giống hệt nhau từng bước**, chạy trên cả hai hồ sơ
- **When** cả hai trận chạy hết và được chốt
- **Then** bảng điểm cuối **trùng nhau từng ghế** · thứ hạng trùng nhau · nhật ký sự kiện trùng nhau về **thứ tự và nội dung** · **0** chênh lệch nào quy được về hồ sơ triển khai

**AC-004 — No-change guarantee: kênh public vẫn không có đường ghi ở hồ sơ portable**
- *US*: US-001, US-005 · *FR*: FR-004 · *PRD*: `PRD-REQ-107` · *GR*: `GR-037` *(vế cấu trúc kênh)*
- **Given** một bản cài **portable** đang chạy một trận, với một màn khán giả và một lớp phủ đang xem
- **When** người kiểm thử gửi mọi dạng sự kiện ghi đã biết từ cả hai kênh public tới server
- **Then** **không tồn tại đường nào để gửi** — đây là tính chất **cấu trúc** của kênh, không phải một guard bị kích hoạt · **0** ghế nào đổi điểm · **0** trạng thái trận nào đổi · kết cục **giống hệt** trên hồ sơ máy chủ

**AC-005 — Boundary: hồ sơ triển khai không đổi phép quyết định của luật**
- *US*: US-001 · *FR*: FR-005 · *PRD*: `PRD-REQ-085` · *GR*: —
- **Given** một câu Về đích trên bản portable, người thi chính vừa bị chấm Sai, cửa sổ cướp quyền đang mở, và ba ghế còn lại cùng bấm chuông trong cùng một mili-giây theo đồng hồ server
- **When** cửa sổ đóng và admin chấm người cướp
- **Then** thứ tự được phân xử bằng **đúng** quy tắc mà `specs/006` đặc tả · **0** phép ưu tiên nào theo hồ sơ triển khai tồn tại · kết cục tất định và dựng lại được

**AC-006 — Invalid state: hồ sơ portable không phải ca miễn trừ của "luôn có admin"**
- *US*: US-001 · *FR*: FR-006 · *PRD*: `PRD-REQ-085` · *GR*: —
- **Given** một bản cài **portable** ở hội trường, một trận đang ở `LOBBY`, và **không** phiên admin nào đang giữ quyền điều khiển
- **When** người kiểm tìm một đường để trận tự chạy tiếp mà không có admin
- **Then** **0** đường nào tồn tại · trận đứng yên chờ admin · hệ thống MUST NOT tự chấm, tự mở vòng hay tự chốt · **0** dữ liệu nào đổi

**AC-007 — Rejection: không có bề mặt nhiều tổ chức trên một bản cài**
- *US*: US-001 · *FR*: FR-007 · *PRD*: `PRD-REQ-085` *(NON-GOAL-006)* · *GR*: —
- **Given** một bản cài ở **một trong hai** hồ sơ, đã có contest và tài khoản của một tổ chức
- **When** người kiểm rà toàn bộ bề mặt tìm đường tạo một tổ chức thứ hai hoặc chia dữ liệu theo tổ chức
- **Then** **0** đường nào tồn tại ở cả hai hồ sơ · **0** trường dữ liệu nào mang ý nghĩa *"thuộc tổ chức nào"* · **0** dữ liệu nào đổi

**AC-008 — Ranh giới đã chấp nhận: server sập, hai hồ sơ như nhau**
- *US*: US-001 · *FR*: FR-008 · *PRD*: `PRD-REQ-085` · *GR*: —
- **Given** một trận đang chạy giữa vòng, trên **mỗi** hồ sơ một lần
- **When** máy chủ dừng đột ngột
- **Then** kết cục **giống nhau** ở hai hồ sơ · **0** cơ chế cứu nào có ở hồ sơ này mà thiếu ở hồ sơ kia · đây được ghi nhận là ranh giới **đã chấp nhận**, không phải một nhánh lỗi phải đặc tả thêm

**AC-009 — Happy path: nhập gói có danh sách là dùng được ngay**
- *US*: US-002 · *FR*: FR-009 · *PRD*: `PRD-REQ-106` · *GR*: —
- **Given** một bản cài **portable trống** — **0** tài khoản, **0** contest — đã ngắt Internet; và một gói contest hợp lệ **có kèm** danh sách người tham gia cùng cụm mật khẩu đúng
- **When** người vận hành nhập gói và khai cụm mật khẩu
- **Then** contest được dựng ở dạng nháp · tài khoản admin của contest **đăng nhập được ngay** · ghế và tài khoản các vai khác đã dựng lại · **0** thao tác dòng lệnh nào được yêu cầu

**AC-010 — Happy path: trọn ngày thi, không một bước dòng lệnh**
- *US*: US-002 · *FR*: FR-009, FR-010 · *PRD*: `PRD-REQ-106`, `PRD-REQ-085` · *GR*: —
- **Given** trạng thái cuối của AC-009: bản portable đã nhập gói, đã ngắt Internet, bốn ghế và một lớp phủ đã sẵn sàng trong LAN
- **When** người vận hành đăng nhập admin, chạy trọn một trận đủ năm vòng, chốt trận, rồi xuất năm hiện vật
- **Then** toàn chuỗi hoàn thành · số lần phải mở dòng lệnh = **0** · năm hiện vật xuất ra đủ · biên bản có nội dung đầy đủ

**AC-011 — Boundary: ngắt Internet đúng lúc đang chạy trận**
- *US*: US-002 · *FR*: FR-010, FR-002 · *PRD*: `PRD-REQ-085` · *GR*: —
- **Given** một trận đang chạy trên bản portable, và máy chủ portable **đang** có kết nối Internet dư thừa *(không cần dùng)*
- **When** kết nối Internet bị ngắt giữa vòng
- **Then** trận chạy tiếp **không gián đoạn** · đồng hồ không đổi · **0** thao tác nào bị từ chối · **0** ghế nào bị ngắt · kết cục vòng không đổi

**AC-012 — Rejection: bản cài trống cộng gói không kèm danh sách**
- *US*: US-002 · *FR*: FR-011 · *PRD*: `PRD-REQ-106` §Note · *GR*: —
- **Given** một bản cài **portable trống** và một gói contest hợp lệ **không kèm** danh sách người tham gia
- **When** người vận hành nhập gói
- **Then** contest được dựng · **0** tài khoản nào được dựng · bản cài **vẫn không có đường đăng nhập** · đường duy nhất còn lại là **dòng lệnh**, và tài liệu vận hành đã nêu thẳng ca này (AC-035) · **0** đường thứ ba nào được sản phẩm mở ra

**AC-013 — Rejection: không có trình hướng dẫn cài đặt trên trình duyệt**
- *US*: US-002 · *FR*: FR-012 · *PRD*: `PRD-REQ-106` *(NON-GOAL-019)* · *GR*: —
- **Given** một bản cài trống ở **mỗi** hồ sơ, chưa có tài khoản nào
- **When** người kiểm mở mọi đường vào đã biết của bản cài qua trình duyệt
- **Then** **0** bề mặt nào cho tạo tài khoản toàn quyền mà không xác thực · **0** trình hướng dẫn cài đặt nào tồn tại · kết cục giống nhau ở hai hồ sơ

**AC-014 — Repeated action: nhập lại cùng một gói**
- *US*: US-002 · *FR*: FR-013 · *PRD*: `PRD-REQ-106` · *GR*: —
- **Given** một bản portable **đã nhập** một gói và đang đăng nhập bằng tài khoản admin dựng từ gói đó
- **When** người vận hành nhập **lại đúng gói đó** lần thứ hai, rồi lần thứ ba
- **Then** mỗi lần cho ra một trong các kết cục mà `specs/003` đã đặc tả cho ca **trùng tên đăng nhập** *(nối · tạo mới có hậu tố · huỷ)* · **0** lần nào làm tài khoản admin đang dùng mất đường vào · phiên đang mở **không** bị ngắt

**AC-015 — Rejection + no-change: gói bị sửa hoặc sai cụm mật khẩu**
- *US*: US-002 · *FR*: FR-014, FR-042 · *PRD*: `PRD-REQ-106` · *GR*: —
- **Given** một bản cài **portable trống**; và hai gói thử — một gói bị sửa nội dung, một gói nguyên vẹn nhưng khai **sai** cụm mật khẩu
- **When** người vận hành nhập lần lượt hai gói đó
- **Then** cả hai lần bị **từ chối toàn bộ**, kèm lý do · **0** contest nào được tạo · **0** tài khoản nào được dựng · **0** media nào được ghi · bản cài **vẫn trống**, kiểm được từng lớp dữ liệu

**AC-016 — Happy path: xuất đủ năm hiện vật trên bản portable đã ngắt mạng**
- *US*: US-003 · *FR*: FR-015, FR-016 · *PRD*: `PRD-REQ-098` · *GR*: —
- **Given** một trận đã `FINISHED` trên bản **portable** đã ngắt Internet, có đủ nhật ký sự kiện, ít nhất một cú hoàn nguyên và một vòng bị bỏ
- **When** admin gọi xuất lần lượt cả năm hiện vật
- **Then** cả **năm** hiện vật được tạo ra · **0** lần gọi nào cần một dịch vụ ngoài · **0** lần gọi nào cần Internet · biên bản in theo **lần chạy** kèm nhãn như `specs/010` đặc tả

**AC-017 — Đối chiếu nội dung: portable không nghèo hơn máy chủ**
- *US*: US-003 · *FR*: FR-015, FR-017 · *PRD*: `PRD-REQ-098` · *GR*: —
- **Given** hai trận **tương đương** — cùng contest, cùng chuỗi thao tác — một chạy trên hồ sơ máy chủ, một trên hồ sơ portable, cả hai đã `FINISHED`
- **When** người kiểm xuất cả năm hiện vật ở cả hai bên và đối chiếu từng cặp
- **Then** mỗi cặp **tương đương về nội dung** · **0** trường nào có ở bản máy chủ mà thiếu ở bản portable · **0** hiện vật nào bị cắt bớt trong im lặng

**AC-018 — Rejection: lần xuất không được rơi vào một phụ thuộc ngoài**
- *US*: US-003 · *FR*: FR-016 · *PRD*: `PRD-REQ-098` · *GR*: —
- **Given** một bản cài **hồ sơ máy chủ**, có Internet, đang chạy bình thường; một trận đã `FINISHED`
- **When** người kiểm **chặn toàn bộ** đường ra Internet của bản cài đó rồi gọi xuất cả năm hiện vật
- **Then** cả năm lần gọi **vẫn thành công** · **0** lần gọi nào chờ hết thời gian vì một dịch vụ ngoài · nội dung năm hiện vật **không đổi** so với lần xuất khi còn mạng

**AC-019 — Boundary: danh sách hiện vật là danh sách đầy đủ**
- *US*: US-003 · *FR*: FR-018 · *PRD*: `PRD-REQ-098` · *GR*: —
- **Given** danh sách năm hiện vật của FR-015 và toàn bộ các spec sở hữu từng hiện vật *(`specs/003`, `specs/010`)*
- **When** người kiểm đối chiếu hai chiều: mọi hiện vật xuất được của sản phẩm có nằm trong danh sách không, và mọi mục trong danh sách có chủ sở hữu không
- **Then** hai chiều **khớp** · **0** hiện vật nào xuất được mà vắng khỏi danh sách · **0** mục nào trong danh sách không có spec sở hữu

**AC-020 — No-change guarantee: không mở đường mang kết quả về**
- *US*: US-003 · *FR*: FR-019 · *PRD*: `PRD-REQ-098` · *GR*: —
- **Given** một trận đã `FINISHED` trên bản portable và một bản cài máy chủ trung tâm chứa cùng contest
- **When** người kiểm tìm mọi đường đưa **kết quả** từ portable về máy chủ trung tâm
- **Then** **0** đường nào tồn tại · thứ duy nhất nhập lại được là **bản kê câu đã dùng** · nhập bản kê đó chỉ đặt cờ đã dùng theo chiều **một chiều**, và **0** điểm số nào ở máy chủ trung tâm bị đụng tới

**AC-021 — No-change guarantee: hiện vật đã xuất nằm ngoài hạn lưu trữ**
- *US*: US-003, US-006 · *FR*: FR-020 · *PRD*: `PRD-REQ-098` · *GR*: —
- **Given** năm hiện vật đã xuất ra khỏi một bản cài **ở mỗi hồ sơ**, và bước dọn dữ liệu theo hạn lưu trữ sắp chạy trên dữ liệu đã quá hạn
- **When** bước dọn chạy tới hết
- **Then** **100%** hiện vật đã xuất còn nguyên · **0** thực thể nào trong hệ thống theo dõi vòng đời của chúng · kết cục giống nhau ở hai hồ sơ

**AC-022 — Happy path: hai bản cài chạy hai giá trị tần suất và quy mô viewer khác nhau**
- *US*: US-004 · *FR*: FR-022, FR-023 · *PRD*: `PRD-REQ-086`, `PRD-REQ-087` · *GR*: —
- **Given** hai bản cài dựng từ cùng bản phát hành — một hồ sơ máy chủ đặt giới hạn tần suất cổng khán giả và mục tiêu quy mô viewer ở giá trị **cao**, một hồ sơ portable đặt ở giá trị **thấp**; mỗi bên đang chạy một trận
- **When** lưu lượng vào cổng khán giả tăng vượt ngưỡng của bản thấp nhưng chưa vượt ngưỡng của bản cao
- **Then** bản portable **áp giới hạn** · bản máy chủ **không** áp · **0** bản cài nào đọc ngưỡng của bản kia · **0** khán giả đang xem nào bị ngắt ở cả hai bên · thứ tự chuông và đồng hồ **không đổi** ở cả hai bên

**AC-022a — Ngưỡng media DÙNG CHUNG: cùng một file, cùng một kết cục ở hai hồ sơ**
- *US*: US-004 · *FR*: FR-021 · *PRD*: `PRD-REQ-019` · *GR*: —
- **Given** hai bản cài dựng từ cùng bản phát hành, một theo hồ sơ máy chủ và một theo hồ sơ portable, **cả hai chạy bộ ngưỡng media mặc định**; một file video **vượt** ngưỡng và một file video **dưới** ngưỡng
- **When** cả hai file được tải lên ở **cả hai** bản cài
- **Then** file vượt ngưỡng bị **từ chối ở cả hai**, file dưới ngưỡng được **chấp nhận ở cả hai** · hai thông điệp từ chối nêu **cùng một** con số · **0** bề mặt cấu hình nào cho đặt ngưỡng media riêng theo hồ sơ · **0** dữ liệu nào đổi ở lần bị từ chối

**AC-023 — Boundary: file đúng bằng ngưỡng**
- *US*: US-004 · *FR*: FR-021, FR-024 · *PRD*: `PRD-REQ-019` · *GR*: —
- **Given** một bản cài với ngưỡng ảnh đặt ở một giá trị `N`
- **When** một file ảnh có kích thước **đúng bằng** `N` được tải lên, rồi một file `N + 1` đơn vị được tải lên
- **Then** file `N` được **chấp nhận** *(biên đóng, theo `specs/003` FR-011)* · file `N + 1` bị **từ chối kèm ngưỡng** · **0** dữ liệu nào đổi ở lần bị từ chối

**AC-024 — Ngưỡng cổng khán giả và quy mô viewer đọc đúng cấu hình của mình**
- *US*: US-004 · *FR*: FR-022, FR-023 · *PRD*: `PRD-REQ-086`, `PRD-REQ-087` · *GR*: —
- **Given** hai bản cài đặt hai giá trị **khác nhau** cho giới hạn tần suất cổng khán giả; một trận đang chạy ở mỗi bên
- **When** lưu lượng vào cổng khán giả tăng vượt ngưỡng của bản thấp nhưng chưa vượt ngưỡng của bản cao
- **Then** bản thấp áp giới hạn · bản cao **không** áp · **0** khán giả đang xem nào bị ngắt ở cả hai bên · thứ tự chuông và đồng hồ **không đổi** ở cả hai bên

**AC-025 — Đổi ngưỡng mà không sửa code**
- *US*: US-004 · *FR*: FR-024 · *PRD*: `PRD-REQ-019`, `PRD-REQ-087` · *GR*: —
- **Given** một bản cài đang chạy với bộ ngưỡng mặc định
- **When** người cài đặt đổi lần lượt **cả ba** nhóm ngưỡng bằng cấu hình triển khai, rồi khởi chạy lại theo quy trình vận hành thông thường
- **Then** **100%** giá trị mới có hiệu lực · **0** dòng code nào bị sửa · **0** bản phát hành nào phải dựng lại

**AC-026 — Rejection: thông điệp phải đọc ngưỡng đang áp**
- *US*: US-004, US-005 · *FR*: FR-025, FR-026 · *PRD*: `PRD-REQ-019` §Note · *GR*: —
- **Given** hai bản cài, một chạy bộ ngưỡng media **mặc định** và một đã được đặt lại ngưỡng ảnh xuống giá trị **thấp hơn**
- **When** cùng một file ảnh vượt ngưỡng của bản thứ hai nhưng dưới ngưỡng của bản thứ nhất được tải lên ở cả hai bên
- **Then** bản thứ nhất **chấp nhận**; bản thứ hai **từ chối** kèm thông điệp nêu **đúng con số đang áp của chính nó** · **0** con số nào viết cứng trong chuỗi giao diện · **0** dữ liệu nào đổi ở lần bị từ chối

**AC-027 — Rejection + no-change: file vượt ngưỡng**
- *US*: US-004 · *FR*: FR-026, FR-042 · *PRD*: `PRD-REQ-019` · *GR*: —
- **Given** một câu hỏi trong kho đề đã có một ảnh hợp lệ đính kèm
- **When** người ra đề tải lên một ảnh **vượt ngưỡng** để thay thế
- **Then** lần tải bị **từ chối kèm ngưỡng** · ảnh cũ **còn nguyên** · **0** byte nào của file mới được ghi lại · trạng thái câu hỏi không đổi

**AC-028 — Invalid state: bản triển khai không khai giá trị ngưỡng**
- *US*: US-004 · *FR*: FR-027 · *PRD*: `PRD-REQ-019`, `PRD-REQ-087` · *GR*: —
- **Given** một bản cài mới, cấu hình triển khai **không khai** giá trị nào cho ba nhóm ngưỡng
- **When** bản cài khởi chạy và một file media được tải lên
- **Then** bản cài khởi chạy **bình thường** · mỗi ngưỡng chạy bằng **mặc định** của nó · hệ thống MUST NOT chạy **không** ngưỡng · **0** lần từ chối khởi chạy nào vì thiếu giá trị

**AC-029 — No-change guarantee: hạ ngưỡng không đụng dữ liệu đã có**
- *US*: US-004 · *FR*: FR-028, FR-042 · *PRD*: `PRD-REQ-019` · *GR*: —
- **Given** một kho đề đã có nhiều media hợp lệ theo ngưỡng cũ, trong đó ít nhất một file **vượt** ngưỡng mới sắp đặt
- **When** người cài đặt hạ ngưỡng xuống dưới kích thước file đó
- **Then** **100%** media đã có vẫn dùng được trong trận · **0** file nào bị xoá hay bị đánh dấu không hợp lệ · ngưỡng mới chỉ áp cho **lần tải lên tiếp theo** · **0** hiện vật đã xuất nào bị đụng tới

**AC-030 — Rà đặc tả: không rule, không transition nào đọc con số vận hành**
- *US*: US-005 · *FR*: FR-029, FR-030 · *PRD*: `PRD-REQ-087` · *GR*: —
- **Given** toàn bộ `docs/game-rules.md` (`GR-001` → `GR-037`) và toàn bộ bảng chuyển trạng thái của `docs/game-state-machine.md`
- **When** người kiểm rà từng rule và từng transition tìm mọi tham chiếu tới quy mô viewer, ngưỡng độ trễ, ngưỡng kích thước media, giới hạn tần suất, hay mục tiêu định cỡ
- **Then** **0** tham chiếu nào tồn tại · **0** guard nào đọc một con số vận hành · **0** đặc tả nào được viết dựa trên một con số cụ thể

**AC-031 — Boundary: trận thứ bảy trên hồ sơ máy chủ vẫn mở được**
- *US*: US-005 · *FR*: FR-030, FR-031, FR-031a · *PRD*: `PRD-REQ-087` · *GR*: —
- **Given** một bản cài **hồ sơ máy chủ** đang có **6** trận chạy đồng thời — đúng bằng mục tiêu định cỡ của hồ sơ đó
- **When** admin mở **trận thứ bảy**
- **Then** trận thứ bảy **mở được bình thường** · **0** cảnh báo nào về hạn ngạch · **0** phép đếm số trận đang chạy nào tồn tại trong hệ thống · sáu trận đang chạy **không bị ảnh hưởng**

**AC-031a — Boundary: trận thứ hai trên hồ sơ portable vẫn mở được**
- *US*: US-005 · *FR*: FR-031, FR-031a · *PRD*: `PRD-REQ-087` · *GR*: —
- **Given** một bản cài **hồ sơ portable** đang có **1** trận chạy — đúng bằng mục tiêu định cỡ của hồ sơ đó
- **When** admin mở **trận thứ hai**
- **Then** trận thứ hai **mở được bình thường** · **0** cảnh báo nào về hạn ngạch · **0** phép đếm số trận đang chạy nào tồn tại trong hệ thống · trận đang chạy **không bị ảnh hưởng** · **0** dữ liệu nào của trận đang chạy bị đụng tới

**AC-032 — No-change guarantee: mục tiêu định cỡ không phải chặn cứng thứ tư**
- *US*: US-005 · *FR*: FR-031a, FR-034 · *PRD*: `PRD-REQ-087` · *GR*: —
- **Given** tập chặn cứng của sản phẩm như `INV-014` khai
- **When** người kiểm rà toàn bộ đường mở trận tìm một cửa từ chối dựa trên số trận đang chạy
- **Then** **0** cửa nào tồn tại · tập chặn cứng **giữ nguyên số lượng** · mục tiêu định cỡ chỉ xuất hiện ở tài liệu vận hành và bài kiểm thử tải

**AC-033 — Concurrency: đông khán giả không đụng công bằng trận**
- *US*: US-005 · *FR*: FR-032 · *PRD*: `PRD-REQ-087`, `PRD-REQ-107` · *GR*: `GR-037` *(vế cấu trúc kênh)*
- **Given** một trận đang ở vòng Về đích với cửa sổ cướp quyền sắp mở; số khán giả trong phòng được nâng lên nhiều lần mức thường trực; lớp phủ vẫn đang nhận
- **When** ba ghế bấm chuông cướp quyền trong cùng một cửa sổ, và số khán giả tiếp tục tăng trong lúc đó
- **Then** thứ tự chuông phân xử theo **server timestamp**, không đổi so với ca ít khán giả · đồng hồ không lệch · thứ hạng tốc độ không đổi · **0** kết cục nào của luật chơi phụ thuộc số khán giả

**AC-034 — Boundary: mỗi người xem vẫn giữ một kết nối mở**
- *US*: US-005 · *FR*: FR-033 · *PRD*: `PRD-REQ-087` §Note · *GR*: —
- **Given** một phòng đang có số khán giả tăng dần tới mức mục tiêu định cỡ của hồ sơ đang chạy
- **When** người kiểm đo tài nguyên kết nối mà máy chủ đang giữ
- **Then** số kết nối mở tăng **tuyến tính theo số người xem** · **0** đặc tả nào của sản phẩm mô tả quy mô viewer như chi phí bằng không · con số này được ghi nhận là **giá trị định cỡ**, không phải cam kết sản phẩm

**AC-035 — Happy path: tài liệu vận hành nêu ca còn phải dùng dòng lệnh**
- *US*: US-006 · *FR*: FR-035, FR-038 · *PRD*: `PRD-REQ-106` §Note · *GR*: —
- **Given** bản phát hành hiện hành cùng tài liệu vận hành đi kèm
- **When** người kiểm tra mục nói về cài đặt lần đầu
- **Then** tài liệu nêu **thẳng** ca *bản cài trống cộng gói không kèm danh sách* · nêu rằng đây là ca **duy nhất còn lại** có thể xảy ra ở hội trường · nêu **hệ quả**: phải chuẩn bị đường dòng lệnh trước ngày thi

**AC-036 — Happy path: tài liệu nói thẳng đường dẫn phòng không thu lại được**
- *US*: US-006 · *FR*: FR-036, FR-038 · *PRD*: `PRD-REQ-001` §Note, `PRD-REQ-086` · *GR*: —
- **Given** tài liệu vận hành đi kèm bản phát hành
- **When** người kiểm tra mục nói về cổng khán giả
- **Then** tài liệu nói thẳng rằng **đường dẫn phòng phát ra thì không thu lại được** · liệt kê hàng rào còn lại là giới hạn tần suất và nút khoá cổng · nêu hệ quả rằng ai có đường dẫn đều thấy tên và trường lớp của thí sinh

**AC-037 — Happy path: tài liệu nhắc xoá gói sau ngày thi**
- *US*: US-006 · *FR*: FR-037, FR-038 · *PRD*: `PRD-REQ-098` · *GR*: —
- **Given** tài liệu vận hành đi kèm bản phát hành
- **When** người kiểm tra mục nói về hiện vật đã xuất
- **Then** tài liệu nhắc **xoá gói sau ngày thi**, nêu rõ gói mang danh sách người tham gia · nêu hệ quả rằng gói đã xuất nằm **ngoài** tầm với của hạn lưu trữ nên hệ thống không tự dọn nó

**AC-038 — Rejection: phát hành hồ sơ portable không kèm tài liệu**
- *US*: US-006 · *FR*: FR-039 · *PRD*: `PRD-REQ-085`, `PRD-REQ-106` · *GR*: —
- **Given** quy trình phát hành của sản phẩm và một bản dựng hồ sơ portable
- **When** người kiểm rà nội dung bản phát hành
- **Then** tài liệu vận hành **có mặt** · **0** hồ sơ nào được phát hành thiếu nó · nội dung tài liệu **giống nhau về ba mục bắt buộc** ở cả hai hồ sơ

**AC-039 — No-change guarantee: tài liệu không sinh requirement mới**
- *US*: US-006 · *FR*: FR-040 · *PRD*: `PRD-REQ-106` · *GR*: —
- **Given** tài liệu vận hành và toàn bộ `docs/`
- **When** người kiểm đối chiếu từng phát biểu ràng buộc trong tài liệu với nguồn của nó trong `docs/`
- **Then** **100%** phát biểu tra được về một mục trong `docs/` · **0** hành vi nghiệp vụ nào ra đời ở tài liệu vận hành · sửa tài liệu **không** làm đổi một requirement nào

**AC-040 — Trạng thái đang xử lý cho nhập gói và xuất hiện vật**
- *US*: mọi US · *FR*: FR-041 · *PRD*: `PRD-REQ-085` · *GR*: —
- **Given** một bản cài ở **mỗi** hồ sơ, một gói contest lớn và một trận đã `FINISHED` có nhật ký sự kiện dài
- **When** admin gọi nhập gói, rồi gọi xuất từng hiện vật
- **Then** mỗi thao tác hiện **trạng thái đang xử lý** rõ ràng trong suốt thời gian chạy · kết thúc bằng thông báo **thành công hoặc thất bại** · **0** khoảng nào giao diện im lặng · nút gọi **không bị disable** để chống gửi lại

**AC-041 — Tiếng Việt và UTC+7 ở cả hai hồ sơ**
- *US*: mọi US · *FR*: FR-043 · *PRD*: `PRD-REQ-085` · *GR*: —
- **Given** hai bản cài ở hai hồ sơ, mỗi bản có ít nhất một trận đã chạy và một lần nhập gói
- **When** người kiểm rà mọi chuỗi giao diện và mọi mốc thời gian hiển thị của feature này
- **Then** **100%** chuỗi bằng tiếng Việt và đến từ file hằng số · **100%** mốc thời gian hiển thị theo **UTC+7** · **0** thiết lập múi giờ theo người dùng nào tồn tại · kết cục giống nhau ở hai hồ sơ

**AC-042 — Một nguồn, hai hồ sơ**
- *US*: US-001 · *FR*: FR-044 · *PRD*: `PRD-REQ-085` · *GR*: —
- **Given** quy trình dựng của sản phẩm
- **When** người kiểm truy nguyên hai hồ sơ về nguồn của chúng
- **Then** cả hai dựng từ **cùng một** bản phát hành nguồn · **0** nhánh sản phẩm song song nào tồn tại · **0** logic nghiệp vụ nào chỉ có ở một nhánh dựng

## Edge Cases

**Boundary cases**

- File media có kích thước **đúng bằng** ngưỡng — biên **đóng**, được chấp nhận (AC-023, `specs/003` FR-011).
- Bản cài **hồ sơ máy chủ** đang có **đúng 6** trận chạy đồng thời, mở trận thứ bảy — mở được, không đếm, không chặn (AC-031).
- Bản cài **hồ sơ portable** đang có **đúng 1** trận chạy, mở trận thứ hai — mở được, không đếm, không chặn (AC-031a). Mục tiêu định cỡ của portable là **1**, và nó vẫn không phải hạn ngạch.
- Số khán giả tăng tới **đúng** mục tiêu định cỡ của hồ sơ, rồi vượt — không kết cục luật chơi nào đổi (AC-033, AC-034).
- Gói contest có kích thước sát ngưỡng mang được của vật mang — **chưa nguồn nào khai một ngưỡng cho toàn gói**, chỉ có ngưỡng cho từng file media (xem `OQ-004`).

**Invalid state**

- Bản cài **portable trống** cộng gói **không kèm** danh sách người tham gia ⇒ không có đường đăng nhập, còn đúng đường dòng lệnh (AC-012).
- Bản cài đang chạy trận mà **không** phiên admin nào giữ quyền điều khiển ⇒ trận đứng yên, hệ thống không tự chạy (AC-006).
- Cấu hình triển khai **không khai** một ngưỡng ⇒ chạy bằng mặc định, không từ chối khởi chạy, không chạy không ngưỡng (AC-028).
- Bản portable **thiếu một thành phần** khiến một trong năm hiện vật không xuất được ⇒ **ca này không tồn tại về cấu trúc**. Hai hồ sơ dựng từ cùng một bản phát hành (FR-044) và phủ cùng một tập tính năng (FR-003), nên đây là **bản dựng hỏng**, không phải trạng thái sản phẩm. Sản phẩm **không** đặc tả nhánh phục hồi nào và **không** có bước kiểm trước-trận nào về khả năng xuất; phép bảo đảm nằm ở AC-042.

**Repeated action**

- Nhập **lại** cùng một gói vào cùng bản cài — kết cục theo ca *trùng tên đăng nhập* của `specs/003`, và không lần nào làm mất đường vào của admin đang dùng (AC-014).
- Gọi xuất **cùng một** hiện vật nhiều lần — mỗi lần gọi cho ra một hiện vật hoàn chỉnh hoặc không cho ra gì; **xuất không phải một trạng thái** (`QĐ-128`, `specs/010`).
- Nhập **bản kê câu đã dùng** nhiều lần — phép **hợp** một chiều, nhập lại là **no-op** (`QĐ-084`, `specs/003`).
- Đổi một ngưỡng nhiều lần liên tiếp — giá trị cuối có hiệu lực; dữ liệu đã có không đổi (AC-029).

**Stale state**

- Bản portable mang cấu hình ngưỡng của **lần triển khai trước** trong khi gói contest được dựng ở bản cài có ngưỡng cao hơn ⇒ media trong gói vẫn nhập được *(ngưỡng áp cho **lần tải lên**, không áp cho lần nhập — theo FR-028, AC-029)*.
- Tài liệu vận hành của một bản phát hành cũ đi kèm một bản dựng mới ⇒ FR-039 đòi tài liệu **đi kèm bản phát hành**, nhưng **chưa nguồn nào khai cơ chế kiểm khớp phiên bản** — ranh giới quy trình, không phải hành vi sản phẩm.

**Duplicate event**

- Hai lần gọi xuất **song song** cùng một hiện vật — thuộc `specs/010`; feature này chỉ đòi kết cục **giống nhau ở hai hồ sơ**.
- Hai phiên cùng đổi một ngưỡng — ngưỡng là **cấu hình triển khai**, không phải bề mặt sản phẩm, nên không có ca tranh chấp trong sản phẩm.

**Partial failure**

- Nhập gói **bị từ chối giữa chừng** *(sai cụm mật khẩu, gói bị sửa)* ⇒ **từ chối toàn bộ**, bản cài nguyên trạng (AC-015, `NFR-24b`).
- Mất kết nối Internet **giữa trận** trên bản portable ⇒ không ảnh hưởng, vì portable vốn không dùng (AC-011).
- Mất kết nối **LAN** giữa trận trên bản portable ⇒ thuộc `GR-036` và `specs/007`; hồ sơ triển khai không đổi kết cục đó.
- Một trong năm hiện vật xuất thất bại trên portable trong khi bốn cái kia thành công ⇒ **gọi lại là cách xử lý duy nhất** (`QĐ-128`); feature này không thêm nhánh phục hồi.

**Conflicting rule**

- `specs/001` FR-035 đặc tả **hành vi** nút khoá cổng, còn `specs/009` §Edge cases viết *"cơ chế của nút này thuộc EPIC-012"* ⇒ ranh giới chủ sở hữu chưa nhất quán giữa hai spec ⇒ `OQ-002` **còn mở**.

**Missing source behavior**

- Không nguồn nào trong `docs/` khai **giá trị mặc định** của ngưỡng media theo từng hồ sơ, dù `specs/003` §Out of Scope đẩy phần đó sang EPIC-012 ⇒ `OQ-004`.
- Không nguồn nào khai **quy mô viewer tối đa** cho hồ sơ portable LAN, cũng không khai hồ sơ máy chủ có cần con số cao hơn không ⇒ `QĐ-067` để mở hai vế này ⇒ `OQ-006`.
- `docs/glossary.md` **không có** `TERM-*` cho *hồ sơ triển khai*, *hồ sơ máy chủ dựng bằng container* hay *hồ sơ portable* ⇒ `OQ-003`.

## Out of Scope

Danh sách này ghi rõ những gì **không** thuộc feature này, và spec nào sở hữu chúng. Requirement nào nằm ở đây thì spec này **không** đặc tả lại, kể cả khi nó xuất hiện trong một acceptance scenario với vai trò tiền đề.

Bốn requirement dùng chung với epic khác vào feature này ở **phần hẹp**, và chỉ ở phần đó: `PRD-REQ-019` vào ở vế **giá trị mặc định của ngưỡng media theo từng hồ sơ** · `PRD-REQ-086` vào ở vế **ngưỡng tần suất là giá trị vận hành đặt theo hồ sơ** · `PRD-REQ-106` vào ở vế **kết cục vận hành ngày thi — nhập gói là dùng được, không mở dòng lệnh** · `PRD-REQ-107` vào ở vế **tính chất không có đường ghi giữ nguyên ở cả hai hồ sơ**. `PRD-REQ-098` là requirement duy nhất **đếm ra** hiện vật, và feature này sở hữu **toàn bộ danh sách năm hiện vật** cùng trách nhiệm rằng danh sách đó không thiếu mục nào; nội dung và định dạng của từng hiện vật thuộc spec sở hữu nó.

`GR-037` chạm tới feature này qua đúng một đường — `PRD-REQ-107` — và chỉ ở vế **cấu trúc kênh public**. Bảng quyết định của `GR-037` *(C1 → C10, mốc câu khép, cờ `revealAnswerAfterJudge`)* thuộc `specs/006`, `specs/007`, `specs/009`, `specs/011`; spec này **không** đặc tả lại và **không** có acceptance scenario nào cho một outcome của bảng đó.

| Hạng mục | Chủ sở hữu |
|---|---|
| Hành vi của nút **khoá cổng** *(khán giả mới không vào được, khán giả đang xem không bị ngắt, lớp phủ luôn vào được)* và của **giới hạn tần suất** | EPIC-001 — `specs/001` FR-034, FR-035. Feature này chỉ sở hữu vế **ngưỡng là giá trị vận hành đặt theo hồ sơ triển khai** |
| Hai đường tạo **tài khoản admin đầu tiên** — đường dòng lệnh và đường tài khoản đi theo gói — cùng phiếu tài khoản và quy tắc cấp mật khẩu mới | EPIC-001 — `specs/001` FR-036 → FR-039. Feature này chỉ sở hữu vế **kết cục vận hành ngày thi** |
| **Cấu trúc một chiều** của hai kênh public, mã phòng, URL vào phòng, catalog permission | EPIC-001 — `specs/001` FR-005 → FR-009; EPIC-009 — `specs/009` FR-002 → FR-004. Feature này chỉ sở hữu vế **tính chất đó giữ nguyên ở cả hai hồ sơ** |
| **Cơ chế** ngưỡng kích thước media *(ngưỡng tồn tại, biên đóng, thông điệp từ chối)* | EPIC-003 — `specs/003` FR-011, FR-012. Feature này chỉ sở hữu **giá trị mặc định theo từng hồ sơ** |
| Cơ chế **xuất và nhập gói contest**, xử lý mật khẩu, cụm mật khẩu mã hoá, ca trùng tên đăng nhập, cờ `everPublic` và cờ đã dùng | EPIC-003 — `specs/003` |
| **Nội dung và định dạng** của biên bản trận, gói kết quả và nhật ký sự kiện, bản kê câu đã dùng, gói kết quả rút gọn; quy tắc *xuất không phải một trạng thái* | EPIC-010 — `specs/010`; EPIC-003 — `specs/003` cho gói contest và bản kê |
| **Hạn lưu trữ**, bước dọn dữ liệu, nhật ký thao tác và màn đọc nhật ký | EPIC-011 — `specs/011`. Feature này chỉ dùng vế *hiện vật đã xuất nằm ngoài hạn lưu trữ* như một tiền đề |
| **Luật chơi** — mọi rule `GR-001` → `GR-037`, engine điểm, hàng đợi tín hiệu, máy trạng thái trận | EPIC-005, EPIC-006, EPIC-007. Feature này chỉ đòi **kết cục không đổi theo hồ sơ** (FR-005) |
| **Nhiều tổ chức trên một bản cài** | NON-GOAL-006 — không thuộc v1 và không thuộc spec nào |
| **Trình hướng dẫn cài đặt lần đầu trên trình duyệt** | NON-GOAL-019 — không thuộc v1 |
| **Job dọn dữ liệu tự động** | v1.5 — `roadmap-post-v1.md` §8.1 |
| **Đồng bộ kết quả từ portable về máy chủ trung tâm** | **Ranh giới đã chấp nhận có chủ đích** (`QĐ-084`, `QĐ-129`) — không thuộc spec nào và không phải chỗ thiếu đặc tả |

## Open Questions

**Không có.** Bốn mục từng mở của feature này đã được phân xử: chủ sở hữu nút khoá cổng — đó là thao tác của **admin đang điều khiển trận**, hành vi ở `specs/001` FR-035 (`QĐ-102`); ngưỡng media **dùng chung** cho cả hai hồ sơ (`QĐ-159`); *hồ sơ triển khai*, *bản cài* và *bản triển khai* là **một** khái niệm (`QĐ-160`, `TERM-064`); và quy mô viewer vẫn là **mặc định cấu hình được**, con số cụ thể chốt khi định cỡ máy thật (`QĐ-067`).

## Assumptions

Các giả định dưới đây được ghi ra để phân biệt với requirement có nguồn. Không giả định nào ở đây được dùng làm cơ sở cho một FR.

- **Bản phát hành là một hiện vật đơn nhất mà hai hồ sơ cùng dựng từ đó.** FR-044 phát biểu điều này như một ràng buộc, dựa trên `PRD-REQ-085` *("cùng một sản phẩm")* và nguyên tắc DRY của `CLAUDE.md`. Không nguồn nào mô tả quy trình dựng, nên hình dạng cụ thể của *"bản phát hành nguồn"* là giả định.
- **Tài liệu vận hành là một hiện vật đi kèm bản phát hành, không phải một bề mặt trong sản phẩm.** `docs/PRD.md` đặt ba `MUST` lên *"tài liệu vận hành"* mà không khai nó nằm ở đâu. Spec này đọc nó là tài liệu đi kèm; nếu chủ dự án muốn nó là một màn trong sản phẩm thì Nhóm F phải viết lại.
- **Ngưỡng vận hành áp tại thời điểm hành động, không áp hồi tố.** FR-028 và AC-029 dựa trên cách đọc này. Không nguồn nào khai kết cục của việc hạ ngưỡng xuống dưới kích thước media đã có; cách đọc *"không hồi tố"* được chọn vì nó là cách duy nhất không phá dữ liệu đã hợp lệ, và vì `specs/003` FR-011 đặt ngưỡng ở đường **tải lên**.
- **"Đầy đủ không cần Internet" nghiệm thu bằng bài kiểm ngắt mạng, không bằng một cơ chế tự phát hiện.** `PRD-REQ-085` §Acceptance intent mô tả đúng bài kiểm đó. Spec này **không** đòi sản phẩm tự phát hiện và từ chối một phụ thuộc ngoài.

## Traceability Matrix

| User story | PRD requirement | Game rules | Functional requirements | Acceptance scenarios |
|---|---|---|---|---|
| **US-001** — Hai hồ sơ triển khai | `PRD-REQ-085`, `PRD-REQ-107` *(vế cấu trúc kênh giữ nguyên ở hai hồ sơ)* | `GR-037` *(chỉ vế cấu trúc kênh public)* · `NFR-28`, `NFR-30`, `NFR-31`, `NFR-32` | FR-001 → FR-008, FR-044 | AC-001 → AC-008, AC-042 |
| **US-002** — Ngày thi trên bản portable | `PRD-REQ-106`, `PRD-REQ-085` | — · `NFR-24b`, `NFR-30`, `NFR-32` | FR-009 → FR-014 | AC-009 → AC-015 |
| **US-003** — Năm hiện vật ở cả hai hồ sơ | `PRD-REQ-098`, `PRD-REQ-085` | — · `NFR-30`, `NFR-35b` | FR-015 → FR-020 | AC-016 → AC-021 |
| **US-004** — Ngưỡng vận hành theo hồ sơ | `PRD-REQ-019`, `PRD-REQ-086`, `PRD-REQ-087` | — · `NFR-06` | FR-021 → FR-028 | AC-022 → AC-029 |
| **US-005** — Con số không lọt vào luật | `PRD-REQ-087`, `PRD-REQ-107` | `GR-037` *(chỉ vế cấu trúc kênh public)* · `NFR-06`, `NFR-11`, `NFR-20` · `INV-014` | FR-029 → FR-031, FR-031a, FR-032 → FR-034 | AC-030 → AC-031, AC-031a, AC-032 → AC-034 |
| **US-006** — Tài liệu vận hành | `PRD-REQ-106` §Note, `PRD-REQ-001` §Note, `PRD-REQ-098`, `PRD-REQ-085` | — · `NFR-35b` | FR-035 → FR-040 | AC-035 → AC-039 |
| *(xuyên suốt)* | `PRD-REQ-085` | — · `NFR-18`, `NFR-36`, `NFR-38` | FR-041 → FR-044 | AC-040 → AC-042 |

### Truy nguyên ngược — mỗi PRD requirement về đâu

| PRD requirement | Phần thuộc feature này | Phần thuộc spec khác |
|---|---|---|
| `PRD-REQ-085` — Hai hồ sơ triển khai | **Toàn bộ** — FR-001 → FR-008 | — |
| `PRD-REQ-098` — Năm hiện vật ở cả hai hồ sơ | **Toàn bộ danh sách** — FR-015 → FR-020 | Nội dung từng hiện vật: `specs/003`, `specs/010` |
| `PRD-REQ-087` — Con số là mặc định cấu hình được | **Toàn bộ** — FR-023, FR-029 → FR-034 | — |
| `PRD-REQ-019` — Giới hạn kích thước media | **Giá trị mặc định theo hồ sơ** — FR-021, FR-024 → FR-028 | Cơ chế ngưỡng: `specs/003` FR-011, FR-012 |
| `PRD-REQ-106` — Cài đặt lần đầu | **Kết cục vận hành ngày thi** — FR-009 → FR-014, FR-035 | Hai đường tạo tài khoản admin: `specs/001` FR-036 → FR-039 |
| `PRD-REQ-086` — Rate-limit và khoá cổng | **Ngưỡng đặt theo hồ sơ** — FR-022 | Hành vi: `specs/001` FR-034, FR-035 |
| `PRD-REQ-107` — Kênh public không có đường ghi | **Giữ nguyên ở cả hai hồ sơ** — FR-004, FR-032 | Cấu trúc kênh: `specs/001` FR-006, FR-007; `specs/009` FR-002 → FR-004 |

### Truy nguyên quyết định

| Quyết định | Ảnh hưởng tới |
|---|---|
| `QĐ-067` — Quy mô viewer và ngưỡng độ trễ: hoãn, không treo | FR-022, FR-023, FR-029; `OQ-006` |
| `QĐ-084` — Bốn loại gói xuất, có đủ ở cả hai hồ sơ | FR-015, FR-019 |
| `QĐ-086` — Gói contest mang danh sách người tham gia, luôn mã hoá | FR-009, FR-014, FR-037 |
| `QĐ-087` — Cài đặt lần đầu: hai đường cho hai thời điểm | FR-009 → FR-012, FR-035 |
| `QĐ-088` — Hai kênh public nhận đẩy một chiều | FR-004, FR-032, FR-033 |
| `QĐ-089` — Số trận song song: mục tiêu định cỡ, không phải chặn cứng | FR-031, FR-031a, FR-034 |
| `QĐ-148` — Mục tiêu định cỡ theo hồ sơ: **6** máy chủ, **1** portable | FR-031, FR-031a, FR-034; AC-031, AC-031a; SC-017, SC-017a |
| `QĐ-096` — Khán giả và lớp phủ vào bằng đúng URL | FR-036 |
| `QĐ-102` — Khoá cổng chỉ chặn màn khán giả | FR-022 *(nền)*; hành vi ở `specs/001` FR-035 |
| `QĐ-128` — Xuất không phải một trạng thái | FR-020; §Edge cases |
| `QĐ-129` — Biên bản trận có đủ ở cả hai hồ sơ | FR-015, FR-017, FR-018, FR-019 |

## Success Criteria *(mandatory)*

- **SC-001**: **100%** bề mặt của sản phẩm dùng được trên hồ sơ portable đã ngắt Internet — rà **đủ** danh mục bề mặt trên cả hai hồ sơ, chạy ít nhất **3** trận đầy đủ năm vòng trên bản portable; và **0** bề mặt nào từ chối một thao tác vì thiếu kết nối ra ngoài.
- **SC-002**: **0** tính năng nào chỉ tồn tại ở một hồ sơ — đối chiếu hai danh mục bề mặt từng mục một; và **0** mục nào lệch.
- **SC-003**: **100%** cặp trận chạy cùng kịch bản trên hai hồ sơ cho **cùng** bảng điểm và **cùng** thứ hạng — thử **10** cặp, mỗi cặp ít nhất **60** sự kiện và có ít nhất một cú hoàn nguyên; và **0** cặp nào lệch một ghế.
- **SC-004**: Số lần phải mở dòng lệnh trong chuỗi *nhập gói → đăng nhập admin → chạy trọn trận → xuất hiện vật* trên bản portable trống đã ngắt Internet bằng **0** — thử **10** lần với **10** gói khác nhau, mỗi gói có ít nhất **4** ghế và **1** tài khoản MC.
- **SC-005**: **100%** lần nhập bị từ chối để bản cài **nguyên trạng** — thử **20** lần trộn hai loại *(gói bị sửa · sai cụm mật khẩu)*; và **0** tài khoản, **0** contest, **0** byte media nào được ghi ở **100%** số lần.
- **SC-006**: **100%** lần nhập lại cùng một gói giữ nguyên đường vào của tài khoản admin đang dùng — thử **15** lần trên **5** bản cài; và **0** lần nào phiên đang mở bị ngắt.
- **SC-007**: **100%** số lần xuất trên bản portable đã ngắt Internet cho ra **đủ năm** hiện vật — thử **10** trận đã `FINISHED`, trong đó ≥ **3** trận có vòng bị bỏ và ≥ **3** trận có cú hoàn nguyên; và **0** lần gọi nào cần một dịch vụ ngoài.
- **SC-008**: **100%** cặp hiện vật cùng loại xuất từ hai hồ sơ **tương đương về nội dung** — đối chiếu **50** cặp trải đủ năm loại; và **0** trường nào có ở bản máy chủ mà thiếu ở bản portable.
- **SC-009**: **0** hiện vật xuất được nào của sản phẩm vắng khỏi danh sách năm mục — rà **100%** đường xuất có thật trên cả hai hồ sơ.
- **SC-010**: **0** đường mang **kết quả** từ portable về máy chủ trung tâm tồn tại — rà **100%** đường nhập của bản cài máy chủ; và **100%** lần nhập bản kê câu đã dùng chỉ đặt cờ theo chiều **một chiều**, thử **20** lần gồm ≥ **5** lần nhập lại cùng file.
- **SC-011**: **100%** cặp bản cài đặt hai giá trị khác nhau cho **giới hạn tần suất** hoặc **quy mô viewer** đều áp **đúng giá trị của mình** — thử **6** cặp; và **0** bản cài nào đọc giá trị của bản kia.
- **SC-011a**: **100%** số file media cho ra **cùng một kết cục** ở hai hồ sơ khi cả hai chạy bộ ngưỡng mặc định — thử **20** file trải đủ **3** loại, một nửa vượt ngưỡng; và **0** bề mặt cấu hình nào cho đặt ngưỡng media riêng theo hồ sơ.
- **SC-012**: **100%** ngưỡng đổi được **mà không sửa code** và **không** dựng lại bản phát hành — thử **12** lần đổi trải đủ ba nhóm trên cả hai hồ sơ.
- **SC-013**: **0** con số ngưỡng nào viết cứng trong sản phẩm — rà **100%** chuỗi giao diện, thông điệp từ chối và logic đọc ngưỡng; và **100%** thông điệp từ chối nêu **đúng** ngưỡng đang áp, đối chiếu **20** thông điệp trên hai bản cài đặt hai giá trị ngưỡng media khác nhau.
- **SC-014**: **100%** bản triển khai không khai ngưỡng đều khởi chạy bằng **mặc định** — thử **6** cấu hình khuyết trải đủ ba nhóm; và **0** lần từ chối khởi chạy, **0** lần chạy không ngưỡng.
- **SC-015**: **100%** media đã có vẫn dùng được sau khi hạ ngưỡng xuống dưới kích thước của nó — thử **10** lần hạ ngưỡng, mỗi lần có ≥ **5** file vượt ngưỡng mới; và **0** file nào bị xoá hay bị đánh dấu không hợp lệ.
- **SC-016**: **0** rule và **0** transition nào đọc một con số vận hành — rà **100%** đặc tả luật `GR-001` → `GR-037` và **100%** bảng chuyển trạng thái, cho **cả bốn** loại con số *(kích thước media · tần suất · quy mô viewer và độ trễ · mục tiêu định cỡ)*.
- **SC-017**: **100%** số lần mở trận vượt mục tiêu định cỡ đều **thành công**, ở **cả hai** hồ sơ — thử mở trận thứ **7** → thứ **10** trên hồ sơ máy chủ *(mục tiêu 6)* và trận thứ **2** → thứ **4** trên hồ sơ portable *(mục tiêu 1)*; và **0** phép đếm số trận đang chạy nào tồn tại, **0** cảnh báo hạn ngạch nào phát ra ở cả hai bên.
- **SC-017a**: Mỗi hồ sơ có **đúng một** mục tiêu định cỡ về số trận đồng thời và hai giá trị **khác nhau** — đọc ra được **6** cho hồ sơ máy chủ và **1** cho hồ sơ portable; và **0** chỗ nào trong sản phẩm đọc hai con số đó để quyết định gì.
- **SC-018**: **0** kết cục luật chơi nào đổi khi số khán giả tăng — thử **10** cửa sổ cướp quyền và **10** vòng Tăng tốc ở hai mức khán giả cách nhau ít nhất **10** lần; và **0** lần nào thứ tự chuông, đồng hồ hay thứ hạng tốc độ lệch.
- **SC-019**: **100%** bản phát hành ở **cả hai** hồ sơ đi kèm tài liệu vận hành có **đủ ba** mục bắt buộc, mỗi mục nêu **hệ quả** chứ không chỉ nhắc tên — rà **3** bản phát hành liên tiếp.
- **SC-020**: **100%** phát biểu ràng buộc trong tài liệu vận hành tra được về một mục trong `docs/` — rà **100%** phát biểu; và **0** hành vi nghiệp vụ nào ra đời ở tài liệu đó.
- **SC-021**: **100%** thao tác nhập gói và xuất hiện vật hiện **trạng thái đang xử lý** liên tục cho tới khi kết thúc — thử **20** thao tác trên cả hai hồ sơ với gói lớn; và **0** khoảng nào giao diện im lặng, **0** nút gọi nào bị disable.
- **SC-022**: **100%** chuỗi giao diện của feature này bằng **tiếng Việt** và đến từ file hằng số; **100%** mốc thời gian hiển thị theo **UTC+7** — đối chiếu **50** mốc trên cả hai hồ sơ; và **0** thiết lập múi giờ theo người dùng nào tồn tại.
