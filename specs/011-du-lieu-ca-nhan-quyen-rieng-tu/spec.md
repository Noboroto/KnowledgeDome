# Feature Specification: Dữ liệu cá nhân và quyền riêng tư

**Feature Branch**: `011-du-lieu-ca-nhan-quyen-rieng-tu`

**Created**: 2026-08-04

**Status**: Draft

**Input**: EPIC-011 — Dữ liệu cá nhân và quyền riêng tư (`docs/PRD.md` §11 dòng 525-536, §12 mục EPIC-011)

**Nguồn**: `docs/PRD.md` (EPIC-011 §11; `PRD-REQ-082`, `PRD-REQ-083`, `PRD-REQ-116` §12 mục EPIC-011; `PRD-REQ-080` và `PRD-REQ-104` — hai requirement khai **Related epic** gồm EPIC-011; JOURNEY-003 và JOURNEY-007 §10; §14 FS-35, FS-35b; §15.3 `NFR-14`, `NFR-15`, `NFR-17`; §15.4 `NFR-18`, `NFR-19`, `NFR-21`, `NFR-24b`; §15.7 `NFR-33`, `NFR-34`, `NFR-35`, `NFR-35b`; §15.8 `NFR-36`, `NFR-37`; §18 `RISK-010`; NON-GOAL-018; ma trận truy nguyên §22) · `docs/game-rules.md` (`GR-037` — rule **duy nhất** mà epic này khai, và chỉ ở **mặt audit** của nó) · `docs/game-state-machine.md` (`T-021`, `T-022` — hai transition mang giá trị retention mặc định; `INV-001` ở đúng vế *nhật ký không xoá*) · `docs/decisions.md` (`QĐ-040`, `QĐ-074`, `QĐ-077`, `QĐ-086`, `QĐ-087`, `QĐ-090`, `QĐ-091`, `QĐ-094`, `QĐ-128`, `QĐ-130`) · `docs/permissions.md` (`PERM-015` `audit.read`, `PERM-045` `match.readAnswer`, `PERM-044`; §2.2, §2.4, §3 túi permission bốn vai dựng sẵn) · `docs/glossary.md` (`TERM-004`, `TERM-007`, `TERM-015`, `TERM-021`, `TERM-028`, `TERM-029`, `TERM-057`, `TERM-061`, `TERM-062`) · `docs/traceability.md` · `.specify/memory/constitution.md` bản 1.4.0 · `CLAUDE.md` §Kiến trúc và bảo mật, §UX, §Quy ước code.

**Lưu ý về đầu vào**: lệnh gọi yêu cầu đọc `docs/rule-traceability.md` và `docs/reviews/prd-review.md` — **cả hai đường dẫn không tồn tại** trong repo. Tương ứng gần nhất của cái thứ nhất là `docs/traceability.md` (đã dùng). Không có tài liệu nào thay thế cái thứ hai; `docs/reviews/` chỉ chứa hồ sơ rà **luật chơi**, và theo `.specify/memory/constitution.md` §*Nguồn đã migrate xong* thì `docs/reviews/**` **không thoả cổng truy nguyên** nên dù có cũng không dùng làm nguồn requirement. Đây là chênh **đường dẫn đầu vào**, không phải một mâu thuẫn nghiệp vụ. Cùng cách xử lý đã dùng ở `specs/005` → `specs/010`.

**Quy ước truy nguyên của feature này** — sáu điểm cần biết trước khi đọc bảng truy nguyên:

1. **Feature này sở hữu MỘT BẢNG, cả hai chiều.** `QĐ-130` chốt: đường **ghi** vào nhật ký thao tác và đường **đọc** ra khỏi nó **cùng chủ**, vì chia một bảng cho hai epic thì mọi thay đổi lược đồ phải đi qua hai spec. Vì vậy `PRD-REQ-082` *(ghi)* và `PRD-REQ-116` *(đọc)* nằm cùng spec này.
2. **Mọi epic khác GHI VÀO nhật ký này, và không epic nào khác đọc ra.** `specs/001` → `specs/010` đều có FR khai *"thao tác X để lại một dòng nhật ký"*. Những FR đó **thuộc spec sở hữu thao tác X**, không được đặc tả lại ở đây. Spec này đặc tả **hình dạng của bảng, phủ sóng bắt buộc, và bề mặt đọc** — tức thứ mà mọi dòng đều phải thoả bất kể ai ghi.
3. **Nhật ký thao tác ≠ nhật ký sự kiện trận.** `TERM-021` `MatchEvent` là nhật ký **của một trận**, nguồn để tính điểm, thuộc `specs/006`. Nhật ký thao tác là bảng **cấp bản cài**, cho **mọi vai và mọi thao tác**, không tham gia phép tính điểm nào. Hai thứ khác chủ, khác vòng đời, khác người đọc. Quan hệ giữa chúng là **tham chiếu một chiều**: nhật ký thao tác trỏ tới nhật ký sự kiện, không chép nội dung sang (`QĐ-132`, FR-007).
4. **`GR-037` vào spec này ở ĐÚNG MỘT MẶT: audit.** Rule khai *"mỗi lần xem đều **vào audit**"* (C1, C2) và *"**(6)** ghi audit nếu có trả"* (§Thứ tự đánh giá) và *"xem nhiều lần: mỗi lần một dòng audit"* (§Bấm trùng). Toàn bộ phần **trả hay không trả đáp án** — C3 → C10, mốc câu khép, cờ `revealAnswerAfterJudge` — thuộc `specs/006`, `specs/007`, `specs/009`; spec này **không** đặc tả lại và **không** đổi nghĩa một ca nào.
5. **Hai requirement dùng chung với epic khác vào đây ở phần HẸP.** `PRD-REQ-104` *(danh sách người tham gia)*: cơ chế xuất/nhập thuộc `specs/003`; spec này nhận đúng vế mà §11 EPIC-011 §Scope khai — **mã hoá bắt buộc cho dữ liệu cá nhân rời hệ thống**, ở dạng một **bất biến phải đúng trên mọi đường**, cộng ràng buộc *dòng nhật ký riêng* của `QĐ-086` §Hệ quả. `PRD-REQ-080` *(dọn dữ liệu)*: vế **hiện vật đã xuất và xuất không phải một state** thuộc `specs/010`; spec này nhận vế **ràng buộc đặt lên bước dọn theo hạn lưu trữ**, vì hạn lưu trữ là của epic này.
6. **Job dọn dữ liệu tự động NGOÀI PHẠM VI, ràng buộc đặt lên nó thì KHÔNG.** `docs/PRD.md` §11 EPIC-011 §Out of scope khai thẳng: job thuộc v1.5 (`roadmap-post-v1.md` §8.1), nhưng hai ràng buộc *(cảnh báo trước · không đụng hiện vật đã xuất)* thuộc v1 *"vì nó là điều kiện đặt lên job, không phải job"*. Spec này đặc tả **điều kiện**, không đặc tả **bộ hẹn giờ**.

## Phạm vi

EPIC-011 phủ **mọi thứ liên quan tới dấu vết và vòng đời của dữ liệu người thật**: nhật ký thao tác chung chỉ-thêm cho **mọi thao tác của mọi vai**, **màn đọc** nhật ký đó gác bằng `PERM-015` `audit.read`, **hạn lưu trữ cấu hình được khác nhau theo mục đích trận**, ràng buộc đặt lên bước dọn dữ liệu, và **mã hoá bắt buộc cho dữ liệu cá nhân rời hệ thống**.

Actor chính: **ACTOR-001 admin** *(và đơn vị tự host, với tư cách bên chịu trách nhiệm pháp lý)*. Mọi actor còn lại — ACTOR-002 người ra đề, ACTOR-003 thí sinh, ACTOR-004 MC, ACTOR-005 khán giả, ACTOR-006 máy dựng stream — xuất hiện ở đây với tư cách **chủ thể bị ghi nhật ký**, không phải người dùng bề mặt. Journey: **JOURNEY-003 — Chuyển sang bản portable** và **JOURNEY-007 — Sau trận**.

**Mục tiêu của epic** (`docs/PRD.md` §11): *"Dữ liệu cá nhân của người tham gia được bảo mật (mã hoá khi rời hệ thống), và mọi thao tác trên hệ thống đều có dấu vết (audit log)"*, với **user value** là *"đơn vị tổ chức chịu được trách nhiệm pháp lý về dữ liệu"*.

**Epic này chạy SONG SONG từ EPIC-001** (`docs/PRD.md` §21 chuỗi chặn), không nằm trên chuỗi chặn của trận. Nó chỉ phụ thuộc **EPIC-001** — vì không có tài khoản và permission thì không có *"người thực hiện"* để ghi và không có `audit.read` để gác.

**Job dọn dữ liệu tự động NẰM NGOÀI phạm vi** — `docs/PRD.md` §11 EPIC-011 khai thẳng ở trường **Out of scope**, và §20 xếp EPIC-011 là *"thuộc phạm vi, **trừ job dọn tự động**"*. Hai thứ khác là **non-goal của sản phẩm**, không phải hạng mục hoãn: **tuỳ chọn hiển thị biệt danh** (NON-GOAL-018) và **disclaimer bản quyền âm thanh** (NON-GOAL-020) — xem FR-055.

**Không thuộc phạm vi feature này** (xem §Out of Scope): tài khoản, vai, permission catalog và cửa kiểm quyền (EPIC-001); cơ chế xuất/nhập gói contest và danh sách người tham gia (EPIC-003); mốc câu khép và phép quyết định *trả hay không trả* đáp án (EPIC-006, EPIC-007, EPIC-009); nhật ký sự kiện trận `MatchEvent` (EPIC-006); biên bản, gói kết quả, hiện vật đã xuất (EPIC-010); giới hạn kích thước media và hai hồ sơ triển khai (EPIC-012).

## User Scenarios & Testing *(mandatory)*

### US-001 — Nhật ký thao tác chung, chỉ thêm, phủ mọi thao tác của mọi vai (Priority: P1)

- **Title**: Một bảng, không ngoại lệ — thiếu một loại thao tác là thiếu ở đúng lúc cần
- **Actor**: ACTOR-001 admin *(người chịu trách nhiệm)*; chủ thể bị ghi là **mọi actor**
- **Intent**: Có **một** nơi duy nhất chứa dấu vết của mọi thao tác đã xảy ra trên bản cài, đủ để sau này dựng lại được chuyện gì đã xảy ra, ai làm, lúc nào, từ đâu.
- **User value**: Đây là điều kiện để đơn vị tổ chức **chịu được trách nhiệm pháp lý** — không có dấu vết thì mọi khiếu nại đều là lời khai chống lời khai. Sáu slice còn lại của epic này đều đứng trên slice này.
- **Priority**: P1
- **Priority rationale**: `PRD-REQ-082` là P1 với lý do *"đây là điều kiện để phân xử khiếu nại và để chứng minh đáp án không rò"*. Nó cũng là **cấu trúc dữ liệu** mà `PRD-REQ-116` đọc ra và mà `PRD-REQ-104` ghi thêm một dòng riêng vào — hai slice đó không dựng được nếu slice này sai. Ngoài ra dấu vết là thứ **không vá được sau**: một thao tác không ghi lúc nó xảy ra thì vĩnh viễn không có dòng nào.
- **Independent test**: Chạy đủ **một vòng đời bản cài** — đăng nhập đúng, đăng nhập sai, tạo và sửa một câu trong kho đề, tạo một contest, chạy một trận có ít nhất một phán quyết và một cú hoàn nguyên, đuổi một khán giả, nhập một gói và xuất một gói — rồi kiểm rằng mỗi thao tác trong danh sách để lại ít nhất một dòng mang đủ **năm trường**: người thực hiện, hành động, đối tượng, thời điểm, địa chỉ nguồn. Kiểm thêm hai vế: sự kiện trong trận tra được qua **đường tham chiếu** chứ không phải qua một bản sao, và việc khán giả **vào phòng** **không** sinh dòng nào. Sau đó thử **sửa** và **xoá** một dòng qua mọi bề mặt có thật và xác nhận không có đường nào.
- **Related PRD requirements**: `PRD-REQ-082`
- **Related game rules**: `GR-037` *(ở mặt audit — xem US-002)* · `NFR-14`, `NFR-15`, `NFR-17` · `INV-001` *(vế nhật ký không xoá)*
- **Related journey**: JOURNEY-007 *(và **mọi journey** — `PRD-REQ-082` khai `Related journey: mọi journey`)*

---

### US-002 — Mỗi lần xem đáp án để lại dấu vết (Priority: P1)

- **Title**: Hàng rào chống rò đề chỉ chứng minh được nếu nó có sổ
- **Actor**: ACTOR-001 admin và ACTOR-004 MC *(ở bốn vai dựng sẵn — nhưng cửa kiểm hỏi **permission**, không hỏi tên vai)*
- **Intent**: Mỗi lần một phiên nhận được đáp án chuẩn của câu đang chạy, hệ thống ghi lại việc đó — kể cả khi cùng một người xem đi xem lại nhiều lần.
- **User value**: `PRD-REQ-082` đặt ra hai lý do tồn tại, và đây là lý do thứ hai: *"chứng minh đáp án không rò"*. Không có dòng nào cho việc xem đáp án thì `GR-037` là một lời hứa không kiểm được — và đề rò thì không có cách nào truy ra ai.
- **Priority**: P1
- **Priority rationale**: `GR-037` là **rule duy nhất** mà EPIC-011 khai, và mặt audit là **toàn bộ** phần của rule đó thuộc epic này. `CLAUDE.md` §Phạm vi hiển thị đáp án xếp hàng rào này là *"sai ở đây là hỏng sản phẩm"*. Slice này tách khỏi US-001 vì nó có **cửa kiểm riêng**, **thứ tự đánh giá riêng**, và một ca lặp mà `GR-037` §Bấm trùng khai tường minh — nó kiểm thử độc lập được mà không cần sáu loại thao tác kia tồn tại.
- **Independent test**: Với một trận đang chạy và một câu **chưa khép**, cho một phiên giữ `PERM-045` mở đáp án **ba lần liên tiếp**; xác nhận có **ba** dòng nhật ký riêng biệt, mỗi dòng đủ năm trường, và **0** event nào được sinh trong nhật ký sự kiện của trận. Lặp lại với một phiên giữ `PERM-045` nhưng **không** giữ quyền điều khiển *(vai MC dựng sẵn)* và xác nhận vẫn ghi đủ.
- **Related PRD requirements**: `PRD-REQ-082`
- **Related game rules**: `GR-037` C1, C2, §Thứ tự đánh giá bước (6), §Bấm trùng, §Không đổi gì · `NFR-14`, `NFR-21`
- **Related journey**: JOURNEY-005 *(nơi thao tác xảy ra)*; JOURNEY-007 *(nơi dấu vết được đọc)*

---

### US-003 — Màn ĐỌC nhật ký thao tác, chỉ đọc, gác bằng `audit.read` (Priority: P2)

- **Title**: Một bảng chỉ-ghi không phân xử được gì
- **Actor**: ACTOR-001 admin *(chính xác hơn: phiên giữ `PERM-015`)*
- **Intent**: Mở một màn tra cứu, lọc theo **người thực hiện · hành động · đối tượng · khoảng thời gian**, và dựng lại chuỗi thao tác quanh một mốc bị khiếu nại — mà không có bất kỳ đường nào để sửa hay xoá thứ mình đang đọc.
- **User value**: Đây là chỗ mà giá trị của US-001 **trở nên dùng được**. `QĐ-130` khai thẳng: không có bề mặt đọc thì bảng là **chỉ-ghi**, `PERM-015` **không gác gì**, và vế *"chứng minh đáp án không rò"* của `PRD-REQ-082` **không thực hiện được**.
- **Priority**: P2
- **Priority rationale**: `PRD-REQ-116` là P2 — một trận **vẫn chạy được** nếu chưa có màn đọc, nên nó không phải P1 theo định nghĩa của `docs/PRD.md` §12. Nhưng nó **phải vào v1** vì nếu không thì một requirement P1 *(`PRD-REQ-082`)* và một permission *(`PERM-015`)* mất nghĩa. Đây đúng hình dạng của P2: cần, nhưng trận không dừng vì thiếu nó.
- **Independent test**: Trên một bản cài đã có sẵn nhật ký của ít nhất **ba** loại thao tác khác nhau, mở màn đọc bằng một tài khoản **có** `audit.read` và tra ra được đủ chuỗi quanh một mốc cho trước bằng cả bốn trục lọc; kiểm mọi mốc thời gian hiển thị theo **UTC+7**. Sau đó mở cùng màn bằng một tài khoản **không** có permission đó và xác nhận không vào được; và bằng một tài khoản chỉ giữ permission phạm vi **`CONTEST`**, xác nhận nó tra được dòng trong contest của mình và **không** thấy dòng của contest khác. Cuối cùng rà **toàn bộ** bề mặt của màn và xác nhận **0** thao tác nào sửa hay xoá được một dòng, kể cả khi gửi thẳng lệnh tới server.
- **Related PRD requirements**: `PRD-REQ-116`, `PRD-REQ-082`
- **Related game rules**: `GR-037` *(là loại dòng mà màn này phải tra ra được)* · `NFR-17`, `NFR-18`, `NFR-37` · `PERM-015`
- **Related journey**: JOURNEY-007

---

### US-004 — Hạn lưu trữ cấu hình được, khác nhau theo mục đích trận (Priority: P2)

- **Title**: Bên chịu trách nhiệm pháp lý phải là bên quyết, còn hệ thống mặc định về phía giữ ít hơn
- **Actor**: ACTOR-001 admin *(đại diện đơn vị tự host)*
- **Intent**: Đặt được hạn lưu trữ, **riêng cho từng mục đích trận**, mà không phải sửa code — với hai giá trị mặc định đã có sẵn khi bản cài dựng lên.
- **User value**: Đơn vị tự host là bên chịu trách nhiệm pháp lý về dữ liệu học sinh; nó phải là bên quyết giữ bao lâu. Không có con số thì bước dọn không dựng được và hai ràng buộc của `PRD-REQ-080` **không có gì để kiểm**.
- **Priority**: P2
- **Priority rationale**: `PRD-REQ-083` là P2 — trận chạy được mà không cần biết hạn lưu trữ. Nhưng `QĐ-091` khai đây là *"lựa chọn về quyền riêng tư, không phải về phần cứng"*, tức **không cần đo gì** để chốt, nên nó không có lý do bị hoãn. Slice này độc lập với ba slice trên: nó không đọc nhật ký và không ghi nhật ký nào ngoài dòng ghi lại chính cú đổi cấu hình.
- **Independent test**: Trên một bản cài mới dựng, đọc hai giá trị mặc định của trục **mục đích trận** và xác nhận **12 tháng** cho trận chính thức, **3 tháng** cho trận luyện tập. Đổi cả hai sang giá trị khác nhau **mà không sửa code**, đọc lại và xác nhận đã đổi; xác nhận cú đổi để lại một dòng nhật ký. Xác nhận tồn tại một trục thứ hai — **hạn riêng của nhật ký thao tác**, một giá trị, đặt độc lập với `matchPurpose`. Chạy bước dọn trên một trận quá hạn và xác nhận dòng nhật ký thuộc trận đó **còn nguyên**. Sau đó rà **toàn bộ** đặc tả luật và bảng chuyển trạng thái và xác nhận **0** rule, **0** transition nào đọc bất kỳ giá trị nào của cả hai trục.
- **Related PRD requirements**: `PRD-REQ-083`
- **Related game rules**: — *(requirement khai `Related game rules: —`)* · `NFR-33`, `NFR-34` · `T-021`, `T-022` *(nơi giá trị mặc định được nhắc, đọc là **mặc định** chứ không phải hằng số — `QĐ-091` §Hệ quả)*
- **Related journey**: JOURNEY-007

---

### US-005 — Dữ liệu cá nhân rời hệ thống thì luôn mã hoá, và luôn để lại dấu vết riêng (Priority: P2)

- **Title**: Đây là `RISK-007` ở dạng thứ hai — không lộ qua mã phòng, mà lộ qua vật mang
- **Actor**: ACTOR-001 admin
- **Intent**: Bảo đảm rằng **không tồn tại đường nào** để danh sách người tham gia rời khỏi hệ thống ở dạng đọc được, và mỗi lần nó rời đi hay quay về đều có một dòng nhật ký **riêng**, tách khỏi dòng của lần xuất/nhập câu hỏi.
- **User value**: Nội dung này là **dữ liệu cá nhân của học sinh** — tên, trường, lớp — đi trên một chiếc USB, và ở lựa chọn *giữ mật khẩu hiện tại* nó mang thêm dấu vết mật khẩu. Đây là `RISK-010` trong `docs/PRD.md` §18, hạng **Cao**, cùng hạng pháp lý với `RISK-007`.
- **Priority**: P2
- **Priority rationale**: `PRD-REQ-104` là P1 **ở phần cơ chế** — không mang danh sách thì nhập xong không ai đăng nhập được — nhưng phần cơ chế đó thuộc `specs/003`. Phần **thuộc epic này** là bất biến quyền riêng tư đặt lên cơ chế ấy, và nó xếp P2 vì nó **không thêm khả năng nào**: nó thu hẹp một khả năng đã có. Nó vẫn không hoãn được sang v1.5 vì `QĐ-086` khai *"để tuỳ chọn nghĩa là sẽ có người tắt nó đúng vào lần cần nhất"*.
- **Independent test**: Xuất một gói contest **có kèm** danh sách người tham gia, ở **cả hai** lựa chọn xử lý mật khẩu; khám nội dung tệp và xác nhận **0** byte nào đọc được tên người, và **0** byte nào là chìa khoá giải mã. Xác nhận mỗi lần xuất và mỗi lần nhập danh sách để lại **một dòng nhật ký riêng**, phân biệt được với dòng của lần xuất/nhập câu hỏi. Cuối cùng rà **mọi** đường mà danh sách có thể rời hệ thống và xác nhận không đường nào bỏ qua bước mã hoá.
- **Related PRD requirements**: `PRD-REQ-104` *(vế EPIC-011)*, `PRD-REQ-082`
- **Related game rules**: — *(requirement khai `Related game rules: —`)* · `NFR-24b`, `NFR-14` · `RISK-010`
- **Related journey**: JOURNEY-003

---

### US-006 — Chạy bước dọn dữ liệu theo hạn lưu trữ (Priority: P2)

- **Title**: Hạn lưu trữ chỉ có nghĩa khi có người thi hành được nó
- **Actor**: ACTOR-001 admin *(chính xác hơn: phiên giữ `PERM-063`)*
- **Intent**: Mở một màn dọn dữ liệu, thấy **đúng những gì sắp bị xoá**, bấm chạy, và nhận một dialog xác nhận trước khi một byte nào biến mất — đồng thời biết chắc bước dọn **không đụng** vào hiện vật mà đơn vị đã chủ động xuất ra và đang giữ.
- **User value**: Đây là chỗ toàn bộ trục hạn lưu trữ **trở nên có thật**. Không có bề mặt này thì `PRD-REQ-083` đặt được hạn mà không ai thi hành, và dữ liệu cá nhân của học sinh nằm lại vĩnh viễn — ngược đúng mục tiêu *"đơn vị tổ chức chịu được trách nhiệm pháp lý về dữ liệu"*.
- **Priority**: P2
- **Priority rationale**: `PRD-REQ-117` là P2 — một trận **vẫn chạy được** nếu chưa có màn dọn, nên nó không phải P1. Nhưng nó phải vào v1 vì thiếu nó thì bốn quyết định về hạn lưu trữ *(`QĐ-091`, `QĐ-131`, `QĐ-136`, `QĐ-140`)* đều nói về một giá trị không ai đọc. Đây đúng hình dạng P2: cần, nhưng trận không dừng vì thiếu nó.
- **Independent test**: Đặt hạn lưu trữ ngắn, tạo dữ liệu quá hạn ở cả hai trục, xuất một biên bản. Mở màn dọn bằng một tài khoản **có** `PERM-063` và xác nhận bản kê phạm vi khớp đúng tập dữ liệu quá hạn. Bấm chạy, nhận dialog, bấm **No** ⇒ xác nhận **0** byte bị xoá. Bấm lại, bấm **Yes** ⇒ xác nhận dữ liệu quá hạn biến mất, hiện vật đã xuất **còn nguyên**, và có một dòng nhật ký cho lần chạy. Cuối cùng mở cùng màn bằng một tài khoản **không** có `PERM-063` và xác nhận không vào được.
- **Related PRD requirements**: `PRD-REQ-117`, `PRD-REQ-080` *(vế ràng buộc lên bước dọn)*, `PRD-REQ-083`
- **Related game rules**: — · `NFR-35`, `NFR-35b` · `PERM-063`
- **Related journey**: JOURNEY-007

---

### Edge Cases

**Giá trị biên**

- Nhật ký của một bản cài **vừa dựng xong**, chưa ai đăng nhập: bảng **rỗng hợp lệ**. Màn đọc mở được và hiện trạng thái rỗng, **không** phải một lỗi.
- **Đăng nhập hỏng** để lại dòng có *người thực hiện* là **tên đăng nhập được khai**, không phải một tài khoản đã xác thực — đây là ca duy nhất mà trường người thực hiện trỏ tới thứ có thể không tồn tại.
- Hạn lưu trữ đặt **rất dài**: chấp nhận có chủ đích (`QĐ-091` §Rủi ro). Hệ thống **không** có biên trên và **không** cảnh báo về độ dài — nó chỉ mặc định về phía giữ ít hơn.
- Hạn lưu trữ đặt bằng **giá trị nhỏ nhất mà giao diện nhận**: hệ thống **không** có biên dưới, cùng khuôn với việc không có biên trên — nó chỉ mặc định về phía giữ ít hơn.
- Gói contest xuất **không kèm** danh sách người tham gia: hợp lệ, không có gì để mã hoá, và **không** sinh dòng nhật ký riêng cho danh sách *(vẫn có dòng cho lần xuất gói — dòng đó thuộc `specs/003`)*.
- Một câu được xem đáp án **một lần duy nhất** ⇒ đúng một dòng; xem **N** lần ⇒ đúng **N** dòng (`GR-037` §Bấm trùng).
- Khoảng thời gian tra cứu **rỗng** *(không dòng nào rơi vào)* ⇒ kết quả rỗng, **không** phải lỗi.

**Trạng thái không hợp lệ**

- Sửa hoặc xoá một dòng nhật ký qua giao diện, hoặc gửi thẳng lệnh tới server ⇒ **không có đường nào tồn tại**, lệnh bị từ chối, và **0** dòng nào đổi (`PRD-REQ-116`).
- Mở màn đọc nhật ký bằng phiên **không** giữ `PERM-015` ⇒ không vào được, và **0** dòng nội dung nhật ký nào rời server.
- Gói kèm danh sách nhưng **sai cụm mật khẩu** hoặc **bị sửa một byte** ⇒ từ chối **toàn bộ**, **0** bản ghi nào được tạo (`NFR-24b`; cơ chế thuộc `specs/003`).
- Yêu cầu xem đáp án **bị từ chối** *(`GR-037` C3 → C10)* ⇒ server **im lặng**, không trả một byte đáp án nào, **và** để lại **đúng một** dòng mang kết quả *bị từ chối*.
- Một dòng nhật ký **trỏ tới** một sự kiện trong trận mà nhật ký sự kiện đó **đã bị bước dọn xoá** ⇒ dòng hiện nguyên năm trường, đường đi tiếp **vô hiệu hoá**, dòng mang nhãn ***"nguồn đã hết hạn"***. Ở cấu hình mặc định *(nhật ký 24 tháng, `official` 12 tháng)* đây là **trạng thái thường trực** cho mọi trận quá 12 tháng, không phải một ca hiếm.
- Ca ngược lại — admin đặt hạn nhật ký **ngắn hơn** hạn trục 1 ⇒ sự kiện trận còn mà **không còn dòng nhật ký nào** trỏ tới. Hệ thống đã **cảnh báo** ở cú đặt (FR-029b) và **không** chặn; đây là ranh giới đã chấp nhận.
- Phiên giữ `PERM-062` tra một khoảng thời gian phủ cả dòng ngoài contest đó ⇒ chỉ nhận dòng **có đối tượng thuộc** contest; dòng ngoài phạm vi **không** rời server, và phản hồi **không** tiết lộ chúng tồn tại.
- Một dòng **không thuộc contest nào** *(đăng nhập, kho đề, quản lý tài khoản)* với một phiên chỉ giữ `PERM-062` ⇒ **không thấy**. Kể cả khi người thực hiện dòng đó **đã được gán** vào contest — phân loại theo **đối tượng**, không theo người.
- Khoảng thời gian tra **không hợp lệ** *(mốc kết thúc sớm hơn mốc bắt đầu)* ⇒ **từ chối kèm lý do**; hệ thống **không** tự hoán đổi và **không** chạy truy vấn.
- Khoảng thời gian tra **vượt biên cứng** ⇒ **từ chối kèm lý do nêu rõ biên**; hệ thống **không** trả một tập bị cắt bớt trong im lặng.
- Bước dọn dữ liệu chạm phải một hiện vật đã xuất ⇒ **không đụng vào**, và bản thân việc gặp nó **không** làm bước dọn dừng.
- Đặt hạn lưu trữ cho một mục đích trận **chưa được bật ở v1** *(`practice`)* ⇒ **nhận bình thường**: ô hiện từ v1, giá trị lưu được và có hiệu lực ngay khi v1.5 bật luồng practice; hệ thống **không** từ chối và **không** ẩn ô.

**Thao tác lặp**

- Mở màn đọc nhật ký nhiều lần, hoặc chạy cùng một truy vấn lọc nhiều lần trên cùng một tập dữ liệu ⇒ **cùng một kết quả**; đọc là thao tác không đổi trạng thái. Mỗi lần mở màn và mỗi lần tra vẫn để lại **một dòng nhật ký của chính nó** (FR-025) — đó là khác biệt duy nhất giữa hai lần chạy.
- Xem đáp án cùng một câu nhiều lần bởi cùng một phiên ⇒ **mỗi lần một dòng**, và **trạng thái trận không đổi** (`GR-037` §Bấm trùng, §Không đổi gì).
- Đổi hạn lưu trữ về **đúng giá trị đang có** ⇒ vẫn là một thao tác, vẫn để lại một dòng nhật ký — cùng khuôn với `SCORE_ADJUST` `delta = 0` của `GR-029` C3: kênh ghi xuất xứ một quyết định không phụ thuộc vào việc giá trị có đổi hay không.
- Tải **cùng một** file âm thanh lên hai lần ⇒ cả hai lần hoàn tất bình thường; **0** disclaimer bản quyền nào hiện ra ở lần nào (FR-055).
- Xuất **cùng một** gói kèm danh sách hai lần ⇒ hai dòng nhật ký riêng cho danh sách; hai lần mã hoá độc lập.

**Trạng thái cũ · sự kiện trùng · hỏng một phần**

- Một dòng nhật ký trỏ tới một **đối tượng đã bị xoá mềm** *(một câu hỏi chẳng hạn)* ⇒ dòng **vẫn còn** và vẫn đọc được; nhật ký không theo vòng đời của đối tượng nó trỏ tới.
- Một dòng nhật ký trỏ tới một **tài khoản đã bị vô hiệu hoá** ⇒ dòng vẫn còn, vẫn hiện tên người thực hiện.
- Hai thao tác xảy ra ở **cùng một mốc thời gian** ⇒ hai dòng riêng biệt; server time là mốc chuẩn tắc duy nhất và thứ tự do server quyết (`NFR-06` khuôn chung, `GR-035`).
- Bước dọn dữ liệu **hỏng giữa chừng** ⇒ hành vi **chưa được `docs/` khai**; job thuộc v1.5 nên ca này ngoài phạm vi v1. Ràng buộc *"không đụng hiện vật đã xuất"* vẫn phải đúng ở mọi thời điểm của lần chạy hỏng đó.
- Một dòng nhật ký được ghi khi **đối tượng chưa tồn tại xong** *(thao tác thất bại giữa chừng)* ⇒ dòng vẫn mang kết quả của lần gọi. Tiền lệ đã có: `NFR-14` khai mỗi lần gọi xuất để lại **đúng một** dòng mang kết quả, gồm cả lần **bị từ chối** lẫn lần **thất bại**.

## Requirements *(mandatory)*

### Functional Requirements

#### Nhóm A — Nhật ký thao tác chung (US-001)

- **FR-001**: Hệ thống MUST duy trì **một** nhật ký thao tác chung ở phạm vi bản cài, dùng cho **mọi vai** và **mọi thao tác**; hệ thống MUST NOT dựng nhiều nhật ký thao tác song song theo vai hay theo khu vực chức năng. *(US-001 · `PRD-REQ-082` · `NFR-14` · AC-001)*
- **FR-002**: Nhật ký thao tác MUST là **chỉ thêm**: một dòng đã ghi MUST NOT sửa được và MUST NOT xoá được qua bất kỳ bề mặt nào của sản phẩm. Ràng buộc này là **tuyệt đối trên bề mặt sản phẩm**; đường **duy nhất** mà một dòng rời hệ thống là bước dọn theo hạn lưu trữ riêng của FR-029a, và đường đó MUST NOT đi qua một bề mặt nào. *(US-001 · `PRD-REQ-082`, `PRD-REQ-116` · `NFR-12` khuôn chung, `INV-001` · AC-002, AC-016, AC-028)*
- **FR-003**: Mỗi dòng nhật ký MUST mang đủ **năm trường**: **người thực hiện** · **hành động** · **đối tượng** · **thời điểm** · **địa chỉ nguồn**. Hệ thống MUST NOT ghi một dòng thiếu bất kỳ trường nào trong năm trường đó. *(US-001 · `PRD-REQ-082` · AC-001, AC-003)*
- **FR-004**: Hệ thống MUST ghi một dòng cho **đăng nhập thành công**, cho **đăng xuất**, và cho **đăng nhập thất bại**. *(US-001 · `PRD-REQ-082` · AC-003)*
- **FR-005**: Hệ thống MUST ghi một dòng cho mọi thao tác **tạo, sửa, xoá** trên kho đề và trên bộ đề. *(US-001 · `PRD-REQ-082` · AC-004)*
- **FR-006**: Hệ thống MUST ghi một dòng cho mọi thao tác **quản lý contest**. *(US-001 · `PRD-REQ-082` · AC-004)*
- **FR-007**: Vế *"mọi sự kiện trong trận"* của `PRD-REQ-082` MUST được thoả bằng **THAM CHIẾU**: nhật ký thao tác MUST giữ đường trỏ tới nhật ký sự kiện của trận, và MUST NOT **nhân bản** nội dung sự kiện sang bảng của mình. *(US-001 · `PRD-REQ-082` · `TERM-021`, `NFR-12`, `INV-001` · AC-005)*
- **FR-007a**: Nội dung của một sự kiện trong trận MUST có **đúng một** nguồn sự thật — nhật ký sự kiện của trận đó. Hệ thống MUST NOT giữ hai bản của cùng một sự kiện ở hai bảng, và MUST NOT chép một bản tóm tắt sang nhật ký thao tác ở bất kỳ thời điểm nào, **kể cả lúc bước dọn xoá dữ liệu trận**. *(US-001 · `PRD-REQ-082` · `GR-028`, `NFR-12`, `INV-001` · AC-005)*
- **FR-007b**: Khi một dòng nhật ký trỏ tới một nhật ký sự kiện **đã bị bước dọn xoá**, dòng đó MUST vẫn hiện **đủ năm trường**; đường đi tiếp MUST bị **vô hiệu hoá**; và dòng MUST mang nhãn ***"nguồn đã hết hạn"*** — phân biệt được với ca *trận không có sự kiện nào*. Hệ thống MUST NOT trình bày hai ca đó giống nhau. Ở trạng thái này `NFR-17` MUST được coi là **không còn thoả** cho mốc đó, và bề mặt đọc MUST nói ra điều đó. *(US-001, US-003 · `PRD-REQ-082`, `PRD-REQ-083`, `PRD-REQ-116` · `NFR-17` · AC-005a)*
- **FR-008**: Hệ thống MUST ghi một dòng khi một khán giả **bị đuổi**. Dòng đó MUST mang **admin đã bấm** ở trường *người thực hiện* và **khán giả bị đuổi** ở trường *đối tượng*. *(US-001 · `PRD-REQ-082` · `TERM-007` · AC-006)*
- **FR-008a**: Hệ thống MUST NOT ghi dòng cho việc một **khán giả vào phòng**. Hệ thống MUST NOT bịa một chủ ngữ cho hai kênh public — không giá trị ẩn danh, không định danh phiên mới, không chủ ngữ *"hệ thống"*. *(US-001 · `PRD-REQ-082`, `PRD-REQ-001` · `TERM-007`, `QĐ-088` · AC-006)*
- **FR-009**: Hệ thống MUST ghi một dòng cho mỗi lần **nhập** và mỗi lần **xuất**. Mỗi lần gọi MUST để lại **đúng một** dòng mang **kết quả** — *thành công* · *bị từ chối* · *thất bại*. *(US-001 · `PRD-REQ-082`, `PRD-REQ-080` · `NFR-14`, `QĐ-128` · AC-007)*
- **FR-010**: Hệ thống MUST ghi một dòng cho mỗi lần **xem đáp án** — xem Nhóm B. *(US-001, US-002 · `PRD-REQ-082` · `GR-037` C1, C2 · AC-008, AC-009)*
- **FR-011**: Thời điểm trong mỗi dòng MUST là **server time**; hệ thống MUST NOT nhận mốc thời gian do client khai. *(US-001 · `PRD-REQ-082` · `GR-035`, `CLAUDE.md` §Server-authoritative · AC-003)*
- **FR-012**: Một dòng nhật ký MUST tồn tại độc lập với vòng đời của **đối tượng** mà nó trỏ tới: đối tượng bị xoá mềm, bị vô hiệu hoá, hay không còn tồn tại MUST NOT làm dòng đó biến mất hay trở nên không đọc được. *(US-001 · `PRD-REQ-082`, `PRD-REQ-116` · `NFR-17` · AC-010)*

#### Nhóm B — Dấu vết của mỗi lần xem đáp án (US-002)

- **FR-013**: Mỗi lần server **trả đáp án chuẩn** cho một phiên vì phiên đó giữ `PERM-045`, hệ thống MUST ghi **một** dòng nhật ký. *(US-002 · `PRD-REQ-082` · `GR-037` C1, C2, §Thứ tự đánh giá bước (6) · AC-008)*
- **FR-014**: Việc ghi ở FR-013 MUST xảy ra bất kể phiên đó **có** hay **không** đang giữ quyền điều khiển; hệ thống MUST NOT phân biệt theo **tên vai**. *(US-002 · `PRD-REQ-082` · `GR-037` C1, C2, §Bước (2) cấm hỏi tên vai, `QĐ-094` · AC-009)*
- **FR-015**: Xem đáp án **nhiều lần** MUST cho ra **mỗi lần một dòng**; hệ thống MUST NOT gộp các lần xem liên tiếp của cùng một phiên trên cùng một câu thành một dòng. *(US-002 · `PRD-REQ-082` · `GR-037` §Bấm trùng · AC-011)*
- **FR-016**: Việc xem đáp án MUST NOT sinh event điểm nào và MUST NOT làm đổi trạng thái trận. *(US-002 · `PRD-REQ-082` · `GR-037` §Không đổi gì · AC-008, AC-011)*
- **FR-017**: **Mọi** yêu cầu xem đáp án MUST để lại **đúng một** dòng nhật ký mang **kết quả** — *đã trả* hoặc *bị từ chối* — gồm cả yêu cầu rơi vào `GR-037` C3, C4, C6, C8, C8b và C10. Dòng của lần bị từ chối MUST cùng **loại dòng** với lần đã trả, phân biệt nhau bằng trường *kết quả*. *(US-002 · `PRD-REQ-082` · `GR-037` §Thứ tự đánh giá bước (6), `NFR-14`, `QĐ-133` · AC-012)*
- **FR-017a**: Việc ghi dòng ở FR-017 MUST NOT làm đổi kết cục của yêu cầu: yêu cầu bị từ chối MUST vẫn **im lặng** và MUST NOT trả một byte đáp án nào; yêu cầu được phép MUST vẫn trả đủ. Ghi nhật ký MUST là **bước cuối**, sau mọi phép kiểm. *(US-002 · `PRD-REQ-082` · `GR-037` §Điều kiện, §Thứ tự đánh giá · `NFR-21` · AC-012)*
- **FR-017b**: Cú **đẩy đáp án hàng loạt** của engine tại mốc câu khép — tới thí sinh, khán giả và overlay khi cờ `revealAnswerAfterJudge` bật (`GR-037` C5, C7) — MUST sinh **một** sự kiện trong **nhật ký sự kiện của trận**, mang mốc thời gian và câu bị lộ. Sự kiện đó MUST tra ra được từ nhật ký thao tác qua đường **tham chiếu** của FR-007. Hệ thống MUST NOT sinh một dòng nhật ký thao tác **riêng** cho cú đẩy, MUST NOT sinh một dòng cho **từng người nhận**, và MUST NOT bịa một chủ ngữ *"hệ thống"* ở trường *người thực hiện*. Cú đẩy **không** phải một *yêu cầu của một phiên* nên FR-017 MUST NOT áp cho nó. *(US-001, US-002 · `PRD-REQ-082` · `GR-037` C5, C7 · `TERM-021`, `QĐ-132`, `QĐ-134` · AC-012b)*

#### Nhóm C — Màn ĐỌC nhật ký thao tác (US-003)

- **FR-018**: Hệ thống MUST có một bề mặt đọc nhật ký thao tác **ở phạm vi bản cài**, gác bằng `PERM-015` `audit.read`. Phiên không giữ permission đó MUST NOT mở được bề mặt này, và **0** nội dung dòng nhật ký nào MUST rời server cho phiên đó. *(US-003 · `PRD-REQ-116` · `PERM-015`, `NFR-18` · AC-013, AC-014)*
- **FR-018a**: Hệ thống MUST có một permission **thứ hai**, **tách bạch** với `PERM-015`, ở phạm vi **`CONTEST`** (`PERM-062`), cho phép tra cứu nhật ký **trong phạm vi một contest**. Hai permission MUST NOT lồng nhau và MUST NOT suy ra nhau: giữ permission phạm vi contest MUST NOT cho đọc dòng ngoài contest đó, và giữ `PERM-015` MUST NOT đòi phải có thêm permission kia. Permission này MUST mang tên chuẩn tắc **`audit.readContest`**. *(US-003 · `PRD-REQ-116` · `PERM-015`, `PERM-062`, `QĐ-094` · AC-014a, AC-014c)*
- **FR-018c**: Vai dựng sẵn **Quản trị** MUST giữ **cả** `PERM-015` và `PERM-062` ngay ở bản cài mới dựng; ba vai dựng sẵn còn lại — Người ra đề, MC, Thí sinh — MUST NOT giữ permission nào trong hai. Hệ thống MUST NOT đòi đơn vị dựng một vai tuỳ biến trước khi có đường đọc nhật ký đầu tiên. Việc vai Quản trị giữ `PERM-015` MUST NOT được coi là thay thế được `PERM-062` ở cửa kiểm của bề mặt phạm vi contest — hai cửa vẫn không suy ra nhau (FR-018a). *(US-003 · `PRD-REQ-116`, `PRD-REQ-082` · `PERM-015`, `PERM-062`, `QĐ-130`, `QĐ-135` · AC-014c)*
- **FR-018b**: Một dòng nhật ký MUST được coi là **thuộc một contest** khi và chỉ khi **đối tượng** của nó thuộc contest đó — trận của contest, cấu hình contest, ghế của contest, lần xuất hoặc nhập gói của contest, lần xem đáp án trong một trận của contest. Dòng **đăng nhập**, dòng **kho đề**, và dòng **quản lý tài khoản** MUST NOT thuộc contest nào. Hệ thống MUST NOT phân loại theo **người thực hiện**: một phiên chỉ giữ `PERM-062` MUST NOT đọc được dòng đăng nhập của một người đã gán vào contest đó. *(US-003 · `PRD-REQ-116`, `PRD-REQ-082` · `PERM-062` · AC-014a, AC-014b)*
- **FR-019**: Cửa kiểm ở FR-018 và FR-018a MUST hỏi **permission**, MUST NOT hỏi **tên vai**, và MUST trả lời bằng **có hoặc không** mà không phải tra thêm một trục thứ hai. *(US-003 · `PRD-REQ-116` · `QĐ-094` · AC-014, AC-014a)*
- **FR-020**: Bề mặt đọc MUST cho lọc và tra theo cả **bốn** trục: **người thực hiện** · **hành động** · **đối tượng** · **khoảng thời gian**; và MUST cho kết hợp nhiều trục trong một lần tra. *(US-003 · `PRD-REQ-116` · `NFR-17` · AC-015, AC-017)*
- **FR-021**: Kết quả tra MUST đủ để **dựng lại chuỗi thao tác** quanh một sự kiện bị khiếu nại — tức phải trả về các dòng theo **thứ tự thời gian**, phải bao gồm các dòng do **mọi** vai ghi *(không lọc bỏ theo vai của người tra)*, và với dòng trỏ tới một sự kiện trong trận thì phải cho **đi tiếp tới** nhật ký sự kiện của trận đó (FR-007). Phép lọc theo **phạm vi permission** ở FR-018a là ngoại lệ **duy nhất**: phiên giữ permission phạm vi contest chỉ nhận dòng thuộc contest đó. *(US-003 · `PRD-REQ-116`, `PRD-REQ-082` · `NFR-17` · AC-015, AC-014a)*
- **FR-022**: Bề mặt đọc MUST là **chỉ đọc**: MUST NOT tồn tại đường sửa, đường xoá, hay đường đánh dấu vô hiệu một dòng nào — kể cả khi lệnh được gửi thẳng tới server. *(US-003 · `PRD-REQ-116` · `NFR-18`, `CLAUDE.md` §Zero-trust · AC-016)*
- **FR-023**: Mọi mốc thời gian trên bề mặt đọc MUST hiển thị theo **UTC+7**. *(US-003 · `PRD-REQ-116` · `CLAUDE.md` §Quy ước code · AC-018)*
- **FR-024**: Bề mặt đọc MUST tra ra được các dòng do **các epic khác ghi vào** — gồm dòng của lần in biên bản và lần xuất gói *(EPIC-010)*, dòng của lần xuất và nhập danh sách người tham gia *(EPIC-003, và FR-039 của spec này)*, và dòng của mỗi lần xem đáp án gồm cả lần **bị từ chối** *(FR-013, FR-017)*. Phép tra này MUST tuân phạm vi permission của FR-018a. *(US-003 · `PRD-REQ-116`, `PRD-REQ-082` · `QĐ-130` · AC-015, AC-017)*
- **FR-025**: Việc **mở** bề mặt đọc và việc **chạy một lần tra** MUST để lại dòng nhật ký như mọi thao tác khác — chúng là thao tác của một vai, và FR-001 không có ngoại lệ. *(US-003 · `PRD-REQ-082` · `NFR-14` · AC-019)*
- **FR-026**: Một lần tra MUST NOT làm đổi bất kỳ dữ liệu nào ngoài dòng nhật ký của chính nó ở FR-025; chạy lại cùng một truy vấn trên cùng một tập dữ liệu MUST cho cùng một kết quả. *(US-003 · `PRD-REQ-116` · AC-020)*
- **FR-027**: Bề mặt đọc MUST hiện **trạng thái đang xử lý** trong lúc tra, và MUST hiện một **trạng thái rỗng đọc được** khi không dòng nào khớp — trạng thái rỗng MUST NOT trình bày như một lỗi. *(US-003 · `PRD-REQ-116` · `NFR-36`, `CLAUDE.md` §UX · AC-021)*
- **FR-028**: Khi **khoảng thời gian không hợp lệ** — mốc kết thúc sớm hơn mốc bắt đầu — hệ thống MUST **từ chối kèm lý do nêu rõ**, MUST NOT chạy truy vấn, và MUST NOT tự hoán đổi hai mốc. **0** dữ liệu nào được đổi. *(US-003 · `PRD-REQ-116` · `CLAUDE.md` §Mô hình ADVISORY · AC-022)*
- **FR-028a**: Hệ thống MUST có một **biên cứng cho độ dài khoảng thời gian** của một lần tra, **cấu hình được** và MUST NOT hard-code. Yêu cầu vượt biên MUST bị **từ chối kèm lý do nêu rõ biên**, và MUST NOT trả về một tập kết quả bị cắt bớt trong im lặng. Giá trị **mặc định** MUST là **24 tháng** — bằng đúng hạn lưu trữ mặc định của nhật ký thao tác (FR-029a). *(US-003 · `PRD-REQ-116` · `NFR-11b`, `NFR-37` · AC-022a)*
- **FR-028b**: Trong phạm vi biên của FR-028a, hệ thống MUST NOT đặt thêm giới hạn nghiệp vụ nào cho **số dòng** trả về; tập kết quả lớn MUST được xử lý bằng **phân trang**, không bằng cắt bớt. *(US-003 · `PRD-REQ-116` · `NFR-37`, `CLAUDE.md` §UX · AC-022a, AC-043)*
- **FR-028c**: Biên của FR-028a MUST là **một** giá trị ở phạm vi **BẢN CÀI**; hệ thống MUST áp **cùng** biên đó cho phiên giữ `PERM-015` và cho phiên giữ `PERM-062`, và MUST NOT đặt một biên riêng theo cửa vào. Cửa kiểm MUST NOT phải tra *"phiên vào bằng permission nào"* để biết biên áp cho nó. *(US-003 · `PRD-REQ-116` · `NFR-11b`, `QĐ-094`, `QĐ-135` · AC-022a)*

#### Nhóm D — Hạn lưu trữ (US-004)

- **FR-029**: Hạn lưu trữ theo **mục đích trận** MUST là **giá trị cấu hình ở phạm vi BẢN CÀI**, đặt được bởi admin, **riêng cho từng mục đích trận**; hệ thống MUST NOT dùng một hạn chung cho mọi mục đích trận, và MUST NOT cho một contest **ghi đè** giá trị của bản cài — trách nhiệm pháp lý nằm ở cấp đơn vị, không ở cấp contest. Hạn này MUST áp cho **dữ liệu trận** và **media do người dùng tải lên**. *(US-004 · `PRD-REQ-083` · `NFR-33`, `QĐ-040`, `QĐ-091` · AC-023)*
- **FR-029a**: **Nhật ký thao tác MUST có một hạn lưu trữ RIÊNG** — **một** giá trị, **không** đi theo trục mục đích trận, mặc định **24 tháng**. Hệ thống MUST NOT tính hạn của một dòng nhật ký từ `matchPurpose`, vì phần lớn dòng **không thuộc trận nào**. Bước dọn theo hạn này MUST là **đường duy nhất** mà một dòng rời hệ thống, và MUST NOT hiện ra thành một thao tác trên bề mặt sản phẩm (FR-002). *(US-004 · `PRD-REQ-082`, `PRD-REQ-083` · `NFR-17`, `NFR-33b` · AC-028)*
- **FR-029b**: Khi admin đặt hạn nhật ký **thấp hơn** hạn dài nhất của trục mục đích trận, hệ thống MUST **cảnh báo** và MUST NÊU cái giá — chuỗi phân xử sẽ hết hạn trước dữ liệu mà nó mô tả. Hệ thống MUST NOT từ chối giá trị đó và MUST NOT có biên nào cưỡng chế quan hệ giữa hai trục: đơn vị tự host là bên chịu trách nhiệm pháp lý nên là bên quyết. *(US-004 · `PRD-REQ-083` · `NFR-17`, `NFR-34`, `QĐ-091` §Rủi ro · AC-028a)*
- **FR-029c**: Cả hai giá trị của trục mục đích trận MUST hiện ở bề mặt cấu hình **ngay từ v1**, gồm cả ô `practice`, và ô `practice` MUST đặt được dù chưa trận nào mang giá trị đó. Hệ thống MUST NOT ẩn ô đó ở v1 rồi hiện lại ở v1.5. *(US-004 · `PRD-REQ-083` · `TERM-015`, `CLAUDE.md` §Phạm vi phiên bản · AC-023)*
- **FR-030**: Giá trị **mặc định** MUST là **12 tháng** cho trận chính thức và **3 tháng** cho trận luyện tập. *(US-004 · `PRD-REQ-083` · `NFR-34`, `QĐ-091` · AC-023)*
- **FR-031**: Hai giá trị ở FR-030 MUST là **mặc định cấu hình**, MUST NOT được viết cứng ở bất kỳ đâu trong sản phẩm, và MUST đổi được **mà không sửa code**. *(US-004 · `PRD-REQ-083` · `NFR-34`, `CLAUDE.md` §Quy ước code · AC-024)*
- **FR-032**: **0** rule và **0** transition nào MUST đọc hai giá trị này; hạn lưu trữ MUST NOT tham gia vào bất kỳ phép quyết định nào của luật chơi hay của máy trạng thái trận. *(US-004 · `PRD-REQ-083` · `QĐ-091`, `T-022` · AC-025)*
- **FR-033**: Hệ thống MUST NOT đặt biên trên cho hạn lưu trữ và MUST NOT từ chối một giá trị vì nó dài; việc admin đặt được rất dài là ranh giới **đã chấp nhận**. *(US-004 · `PRD-REQ-083` · `QĐ-091` §Rủi ro · AC-026)*
- **FR-034**: Mỗi lần đổi hạn lưu trữ MUST để lại một dòng nhật ký, **kể cả** khi giá trị mới trùng giá trị cũ. *(US-004 · `PRD-REQ-082`, `PRD-REQ-083` · `NFR-14` · AC-027)*
- **FR-035**: Hệ thống MUST có **đúng hai trục** hạn lưu trữ và MUST NOT trộn chúng: **(1)** hạn theo **mục đích trận**, áp cho **dữ liệu trận** và **media do người dùng tải lên** (FR-029); **(2)** hạn **riêng của nhật ký thao tác**, một giá trị, độc lập với mục đích trận (FR-029a). **Cả hai trục MUST đặt ở phạm vi BẢN CÀI.** Lớp dữ liệu nào không nằm ở một trong hai trục đó MUST NOT bị bước dọn đụng tới. *(US-004 · `PRD-REQ-083`, `PRD-REQ-082` · `NFR-33`, `NFR-33b`, `NFR-34` · AC-028)*

#### Nhóm E — Dữ liệu cá nhân rời hệ thống (US-005)

- **FR-036**: Khi danh sách người tham gia rời hệ thống, nó MUST **luôn** ở dạng đã mã hoá — **kể cả** khi nó không mang mật khẩu nào. Hệ thống MUST NOT có biến thể không mã hoá, và MUST NOT có thiết lập nào tắt được bước mã hoá. *(US-005 · `PRD-REQ-104` · `NFR-24b`, `QĐ-086` vế 2 · AC-029, AC-030)*
- **FR-037**: Chìa khoá giải mã MUST NOT nằm trong gói. *(US-005 · `PRD-REQ-104` · `NFR-24b`, `QĐ-086` vế 3 · AC-029)*
- **FR-038**: Bất biến ở FR-036 MUST đúng trên **mọi** đường mà danh sách có thể rời hệ thống; hệ thống MUST NOT có đường nào bỏ qua bước mã hoá vì tiện. *(US-005 · `PRD-REQ-104` · `NFR-24b`, `RISK-010` · AC-030)*
- **FR-039**: Mỗi lần **xuất** và mỗi lần **nhập** danh sách người tham gia MUST để lại một dòng nhật ký **riêng**, phân biệt được với dòng của lần xuất hoặc nhập câu hỏi — đây là một lần **dữ liệu cá nhân rời hệ thống**, không cùng hạng với xuất câu hỏi. *(US-005 · `PRD-REQ-082`, `PRD-REQ-104` · `QĐ-086` §Hệ quả · AC-031)*
- **FR-040**: Một gói **không kèm** danh sách người tham gia MUST là gói hợp lệ, và MUST NOT sinh dòng nhật ký riêng của FR-039. *(US-005 · `PRD-REQ-104` · `QĐ-086` vế 1 · AC-032)*
- **FR-041**: Gói đã rời hệ thống MUST được hiểu là nằm **ngoài** tầm với của hạn lưu trữ — gồm cả gói mang danh sách người tham gia. Hệ thống MUST NOT khai một thực thể nào để theo dõi vòng đời của một gói đã xuất. *(US-005, US-006 · `PRD-REQ-080` · `NFR-35b`, `QĐ-091` §Hệ quả, `QĐ-128` · AC-033)*

#### Nhóm F — Bước dọn dữ liệu theo hạn lưu trữ (US-006)

- **FR-041a**: Hệ thống MUST có một **bề mặt trong sản phẩm** để admin chạy bước dọn dữ liệu theo hạn lưu trữ. Bề mặt này MUST được gác bằng `PERM-063` `retention.purge`; phiên không giữ permission đó MUST NOT mở được, và **0** nội dung bản kê phạm vi nào MUST rời server cho phiên đó. *(US-006 · `PRD-REQ-117` · `PERM-063`, `NFR-18` · AC-033a, AC-033b)*
- **FR-041b**: `PERM-063` MUST tách bạch với `PERM-021` `retention.manage`: `PERM-021` cho **đặt** hạn lưu trữ, `PERM-063` cho **thi hành** nó. Hai permission MUST NOT suy ra nhau. *(US-006 · `PRD-REQ-117` · `PERM-021`, `PERM-063` · AC-033b)*
- **FR-041c**: Trước khi bấm chạy, bề mặt MUST hiện **bản kê phạm vi sắp bị xoá** — đủ để người bấm biết mình đang đồng ý xoá cái gì, và khớp đúng tập dữ liệu quá hạn theo **cả hai trục** hạn lưu trữ. *(US-006 · `PRD-REQ-117` · `NFR-35` · AC-033a, AC-038)*
- **FR-041d**: Bước dọn theo hạn lưu trữ MUST là đường **duy nhất** một dòng dữ liệu quá hạn rời hệ thống; hệ thống MUST NOT có đường xoá nào khác trên bề mặt sản phẩm. *(US-006 · `PRD-REQ-117`, `PRD-REQ-082` · AC-016, AC-028)*
- **FR-041e**: `v1` MUST NOT có **job dọn tự động** — không bộ hẹn giờ nào tự chạy bước dọn theo lịch. Bước dọn ở v1 MUST chỉ chạy khi một phiên giữ `PERM-063` bấm. *(US-006 · `PRD-REQ-117` · AC-033c)*

- **FR-042**: Bước dọn dữ liệu theo hạn lưu trữ MUST **cảnh báo trước** khi bắt đầu. *(US-006 · `PRD-REQ-080` · `NFR-35`, `QĐ-077` · AC-034)*
- **FR-043**: Bước dọn dữ liệu MUST NOT đụng vào **hiện vật đã xuất** — biên bản, gói kết quả, gói rút gọn, gói contest. *(US-006 · `PRD-REQ-080` · `NFR-35`, `NFR-35b` · AC-035)*
- **FR-044**: Ràng buộc ở FR-042 và FR-043 MUST đúng ở **mọi** lần chạy bước dọn, gồm cả lần chạy kết thúc bất thường. *(US-006 · `PRD-REQ-080` · `NFR-35` · AC-036)*
- **FR-045**: Mỗi lần chạy bước dọn MUST để lại dòng nhật ký. *(US-006 · `PRD-REQ-082` · `NFR-14` · AC-037)*
- **FR-046**: Cảnh báo ở FR-042 MUST là một **dialog xác nhận Yes/No** phát ra **tại mốc kích hoạt** bước dọn, tới **phiên đang bấm** — không phát tới phiên khác, không phát trước theo lịch. Dialog MUST **liệt kê phạm vi sắp bị xoá** đủ để người bấm biết mình đang đồng ý xoá cái gì, và bước dọn MUST NOT xoá một byte nào trước khi nhận Yes. *(US-006 · `PRD-REQ-080` · `NFR-35`, `QĐ-077`, `CLAUDE.md` §Dialog xác nhận · AC-038)*
- **FR-046a**: Ở v1, hệ thống MUST NOT đặc tả và MUST NOT dựng một tầng cảnh báo **theo lịch trình** *(báo trước N ngày, banner, thông báo ngoài luồng)* — v1 không có bộ hẹn giờ nào để báo trước. Khi job dọn tự động của v1.5 được dựng, một tầng cảnh báo lịch trình MUST được đặc tả **thêm** và MUST NOT thay thế dialog của FR-046. *(US-006 · `PRD-REQ-080` · `NFR-35` · AC-038)*

#### Nhóm G — Ràng buộc xuyên suốt

- **FR-051**: Mọi cửa kiểm của feature này MUST được cưỡng chế ở **server**; ẩn nút ở giao diện MUST NOT được coi là một biện pháp. *(mọi US · `NFR-18`, `NFR-19`, `CLAUDE.md` §Zero-trust · AC-014, AC-016)*
- **FR-052**: Mọi từ chối của feature này MUST NOT làm đổi bất kỳ dữ liệu nào. *(mọi US · `NFR-18` · AC-014, AC-016, AC-030)*
- **FR-053**: Mọi chuỗi giao diện của feature này MUST tách ra file hằng số, MUST NOT viết cứng trong thành phần hiển thị; và toàn bộ chỉ dùng **tiếng Việt**. *(mọi US · `CLAUDE.md` §Quy ước code · AC-042)*
- **FR-054**: Nội dung chính của bề mặt đọc nhật ký MUST nằm gọn trong **một** khung nhìn tham chiếu; bảng dài MUST ưu tiên phân trang. *(US-003 · `NFR-37`, `CLAUDE.md` §UX · AC-043)*
- **FR-055**: Hệ thống MUST NOT hiện **disclaimer bản quyền** ở bất kỳ bề mặt nào — không ở luồng tải file âm thanh lên, không ở đường nhập gói contest, không ở màn cấu hình khe âm thanh. Hệ thống MUST NOT dựng ô tích, nút *"Tôi hiểu"*, hay bất kỳ bước xác nhận pháp lý nào cho âm thanh; MUST NOT giữ một trạng thái *"đã đọc disclaimer"*; và MUST NOT ghi dòng nhật ký nào về việc đọc disclaimer. Ba luồng — tải lên, tải lại cùng file, nhập gói có kèm âm thanh — MUST hoàn tất bình thường mà không có bước chen giữa nào. *(mọi US · NON-GOAL-020 · `QĐ-144` · AC-039a)*

### Key Entities

- **Dòng nhật ký thao tác**: một bản ghi **chỉ thêm**, ở phạm vi bản cài, cho **một** thao tác đã xảy ra. Năm trường bắt buộc: **người thực hiện** · **hành động** · **đối tượng** · **thời điểm** *(server time)* · **địa chỉ nguồn**. Mang thêm **kết quả** *(thành công · bị từ chối · thất bại)* ở các loại thao tác mà `NFR-14` đòi, **và** ở mọi yêu cầu xem đáp án (FR-017). Với sự kiện trong trận, dòng mang một **đường trỏ tới** nhật ký sự kiện chứ **không** chép nội dung (FR-007). **Không** tham gia phép tính nào; **không** theo vòng đời của đối tượng nó trỏ tới. Có **vòng đời riêng** — hạn lưu trữ của nó độc lập với hạn của dữ liệu trận. Khác hẳn `TERM-021` `MatchEvent` — thứ đó là nhật ký **của một trận** và là nguồn để tính điểm.
- **Hạn lưu trữ**: **hai trục độc lập**, không trộn vào nhau, **cả hai đặt ở phạm vi BẢN CÀI** — contest không ghi đè được. **Trục 1 — theo mục đích trận** (`TERM-015`): một giá trị cho mỗi `matchPurpose`, mặc định `official` = 12 tháng và `practice` = 3 tháng, áp cho **dữ liệu trận** và **media do người dùng tải lên**; **cả hai ô hiện từ v1**. **Trục 2 — riêng của nhật ký thao tác**: **một** giá trị, mặc định **24 tháng**, **không** theo mục đích trận. Quan hệ giữa hai trục **không bị cưỡng chế**: đặt trục 2 thấp hơn trục 1 thì hệ thống **cảnh báo** chứ không chặn. Cả hai **không** phải giá trị luật: **0** rule và **0** transition đọc chúng; không có biên trên.
- **Danh sách người tham gia đã mã hoá**: phần dữ liệu cá nhân đi kèm gói contest — thí sinh, MC, và tài khoản admin của contest. **Luôn** ở dạng đã mã hoá; chìa khoá **không** nằm trong gói. Cơ chế xuất/nhập thuộc `specs/003`; ở đây nó là **đối tượng của một bất biến** và là **chủ thể của một dòng nhật ký riêng**.
- **Bước dọn dữ liệu**: thao tác **thủ công** của một phiên giữ `PERM-063` `retention.purge`, xoá dữ liệu đã quá hạn theo **cả hai trục** hạn lưu trữ. Là đường **duy nhất** một dòng dữ liệu quá hạn rời hệ thống. Trước cú bấm có **bản kê phạm vi**; tại cú bấm có **dialog Yes/No**; không byte nào biến mất trước khi nhận Yes. **Không** thuộc bảy thao tác phá huỷ của `QĐ-078` — bảy thao tác đó đổi kết quả một trận, còn bước dọn là vòng đời dữ liệu cấp bản cài — nên nó **không** đòi nhập lý do. `v1` **không** có bộ hẹn giờ tự chạy nó.
- **Hiện vật đã xuất**: một tệp đã rời hệ thống. Nằm **ngoài** tầm với của hạn lưu trữ, và **không** phải một thực thể mà hệ thống theo dõi vòng đời (`QĐ-128`). Ở spec này nó xuất hiện đúng một lần: như thứ mà bước dọn **không được đụng vào**.

## Success Criteria *(mandatory)*

- **SC-001**: **100%** số loại thao tác trong danh sách bắt buộc của `PRD-REQ-082` đều để lại ít nhất một dòng nhật ký — rà đủ **tám** loại *(đăng nhập thành công · đăng nhập hỏng · đăng xuất · kho đề · contest · sự kiện trong trận **qua đường tham chiếu** · khán giả **bị đuổi** · nhập và xuất · xem đáp án)*, mỗi loại thử ít nhất **3** lần. Và **0** dòng nào được ghi cho việc khán giả **vào phòng** — thử **20** lượt vào phòng từ **3** địa chỉ nguồn khác nhau.
- **SC-002**: **100%** số dòng nhật ký mang đủ **năm trường** — lấy mẫu **200** dòng trải qua đủ tám loại thao tác, và **0** dòng nào thiếu một trường.
- **SC-003**: **0** đường sửa và **0** đường xoá một dòng nhật ký tồn tại trên toàn bề mặt — rà **100%** thao tác của màn đọc, cộng gửi thẳng tới server ít nhất **5** dạng lệnh sửa/xoá khác nhau; và **0** dòng nào đổi sau khi thử.
- **SC-004**: **100%** số **yêu cầu** xem đáp án đều để lại **đúng một** dòng mang **kết quả** — thử **80** yêu cầu trải trên cả **năm** vòng: **50** yêu cầu được phép *(gồm ít nhất 10 lần lặp cùng một câu bởi cùng một phiên, và 10 lần bởi phiên không giữ quyền điều khiển)* và **30** yêu cầu **bị từ chối** *(phủ đủ sáu ca `GR-037` C3, C4, C6, C8, C8b, C10)*. Đối chiếu số yêu cầu với số dòng và xác nhận bằng nhau; và **0** byte đáp án nào rời server ở **100%** số lần bị từ chối.
- **SC-004a**: Một chuỗi **10** yêu cầu xem đáp án bị từ chối liên tiếp từ **cùng một** phiên tra ra được thành một chuỗi liền mạch trên màn đọc ở **100%** số lần thử — thử **5** chuỗi ở các vòng khác nhau; và **0** chuỗi nào vô hình.
- **SC-004b**: **100%** số cú **đẩy đáp án hàng loạt** tại mốc câu khép sinh **đúng một** sự kiện trong nhật ký sự kiện trận và **0** dòng nhật ký thao tác — thử **20** cú đẩy trải trên **5** vòng, mỗi cú có ít nhất **30** điểm nhận đang kết nối; đối chiếu số cú đẩy với số sự kiện và xác nhận bằng nhau *(không phải bằng số điểm nhận)*; và **0** chủ ngữ *"hệ thống"* nào được sinh ra.
- **SC-005**: **0** event nào được sinh trong nhật ký sự kiện trận bởi việc **một phiên YÊU CẦU** xem đáp án *(đường kéo — khác hẳn cú đẩy hàng loạt của SC-004b)* — rà **100%** số lần thử của SC-004, và điểm của cả bốn ghế không đổi ở **100%** số lần.
- **SC-006**: **0** phiên thiếu `audit.read` mở được màn đọc cấp bản cài — thử **cả bốn** vai dựng sẵn cộng ít nhất **2** vai tuỳ biến không có permission đó, gửi cả qua giao diện lẫn thẳng tới server; và **0** byte nội dung dòng nhật ký nào rời server ở **100%** số lần thử.
- **SC-006a**: **0** dòng ngoài phạm vi contest được cấp rời server cho một phiên chỉ giữ `PERM-062` — thử trên bản cài có **3** contest và ≥ **1000** dòng, chạy **20** truy vấn phủ khoảng thời gian của cả ba; và **0** phản hồi nào tiết lộ sự tồn tại của dòng ngoài phạm vi *(đối chiếu nội dung, mã lỗi và thời gian phản hồi)*.
- **SC-006b**: **0** dòng **không thuộc contest nào** *(đăng nhập, kho đề, quản lý tài khoản)* lọt vào kết quả của một phiên chỉ giữ `PERM-062` — thử **30** dòng thuộc ba loại đó, trong đó ≥ **10** dòng có **người thực hiện đã được gán** vào chính contest được cấp; và **100%** bị loại đúng vì phân loại theo **đối tượng**.
- **SC-006c**: **100%** số truy vấn có **khoảng thời gian không hợp lệ** hoặc **vượt biên** đều bị **từ chối kèm lý do** — thử **20** truy vấn trộn hai loại; **0** truy vấn nào chạy, **0** tập kết quả bị cắt bớt trong im lặng, và **0** lần hệ thống tự hoán đổi hai mốc.
- **SC-006d**: Trên một bản cài mới dựng, **100%** số lần đọc biên độ dài khoảng thời gian cho ra **24 tháng**; và **100%** số cặp truy vấn giống hệt nhau chạy bởi một phiên `PERM-015` và một phiên `PERM-062` cho ra **cùng** kết cục về biên — thử **10** cặp, trong đó **5** cặp vượt biên; và **0** cặp nào lệch.
- **SC-007**: **100%** số truy vấn trên **cả bốn** trục lọc đều trả ra đúng tập dòng kỳ vọng — thử ít nhất **20** truy vấn trên một nhật ký có ít nhất **1000** dòng trải qua đủ tám loại thao tác, gồm ít nhất **5** truy vấn kết hợp từ hai trục trở lên.
- **SC-008**: Với một mốc bị khiếu nại cho trước, người tra dựng lại được **đủ** chuỗi thao tác quanh mốc đó ở **100%** số ca thử — thử **10** ca, mỗi ca gồm ít nhất một dòng do **mỗi** trong ba nguồn ghi: thao tác của admin, lần xem đáp án, và lần in biên bản hoặc xuất gói của EPIC-010.
- **SC-009**: **100%** mốc thời gian hiển thị trên màn đọc theo **UTC+7** — đối chiếu **50** dòng với mốc gốc, và **0** dòng nào lệch múi giờ.
- **SC-010**: Trên một bản cài **v1** mới dựng, **100%** số lần đọc giá trị mặc định cho ra **ba** con số đúng: **12 tháng** *(chính thức)*, **3 tháng** *(luyện tập)*, **24 tháng** *(nhật ký thao tác)*; **0** ô nào bị ẩn ở v1; và **0** bề mặt nào cho contest ghi đè — thử trên một bản cài có **3** contest.
- **SC-010a**: **100%** số lần đặt hạn nhật ký thấp hơn hạn dài nhất của trục mục đích trận đều **phát cảnh báo** và **vẫn được chấp nhận** — thử **6** cặp giá trị; và **0** lần nào bị hệ thống từ chối.
- **SC-010b**: **100%** số dòng nhật ký trỏ tới nhật ký sự kiện đã bị dọn đều mang nhãn ***"nguồn đã hết hạn"*** và có đường đi tiếp **vô hiệu hoá** — thử **50** dòng trải qua **5** trận quá hạn; và **0** dòng nào bị trình bày giống ca *trận không có sự kiện nào*.
- **SC-011**: **100%** số lần đổi hạn lưu trữ thành công **mà không sửa code** — thử ít nhất **6** cặp giá trị khác nhau, gồm một giá trị rất dài; và **0** lần nào bị từ chối vì độ dài.
- **SC-011a**: **0** dòng nhật ký thao tác nào bị dọn theo hạn của **trận** — thử **10** lần chạy bước dọn trên trận đã quá hạn, mỗi lần có ít nhất **500** dòng nhật ký trong đó ≥ **60%** thuộc trận bị dọn; và **100%** số dòng đó còn nguyên sau khi bước dọn kết thúc.
- **SC-011b**: **0** bản sao nội dung sự kiện trận nào tồn tại trong nhật ký thao tác — rà **100%** dòng của một trận có ít nhất **60** sự kiện; và **100%** số dòng trỏ tới sự kiện đều đi tiếp được tới nhật ký sự kiện của trận.
- **SC-012**: **0** rule và **0** transition nào đọc giá trị hạn lưu trữ — rà **100%** đặc tả luật `GR-001` → `GR-037` và **100%** bảng chuyển trạng thái, cho **cả hai** trục hạn.
- **SC-013**: **0** byte đọc được của dữ liệu cá nhân có trong một gói contest kèm danh sách người tham gia — khám nội dung tệp, thử **cả hai** lựa chọn xử lý mật khẩu, trên một contest có ít nhất **10** người được gán; và **0** byte nào là chìa khoá giải mã.
- **SC-014**: **100%** số lần xuất và nhập danh sách người tham gia để lại một dòng nhật ký **riêng**, phân biệt được với dòng của lần xuất/nhập câu hỏi — thử **20** lần trộn cả hai chiều.
- **SC-015a**: **100%** số lần mở màn dọn bởi phiên **không** giữ `PERM-063` đều bị từ chối — thử **cả bốn** vai dựng sẵn cộng một vai chỉ giữ `PERM-021`, gửi cả qua giao diện lẫn thẳng tới server; và **0** byte nội dung bản kê nào rời server.
- **SC-015b**: **100%** số lần bấm **No** ở dialog để lại **0** byte bị xoá — thử **10** lần trên bản cài có ≥ **500** dòng dữ liệu quá hạn; và bản kê phạm vi khớp đúng tập quá hạn ở **100%** số lần thử.
- **SC-015c**: **0** byte dữ liệu quá hạn nào rời hệ thống mà không qua một cú bấm của phiên giữ `PERM-063` — chạy bản cài qua **30** ngày mô phỏng phủ mọi mốc lịch có thể; và **0** lần chạy tự động nào xảy ra.
- **SC-015**: **100%** số hiện vật đã xuất còn nguyên sau khi bước dọn chạy trên dữ liệu quá hạn — thử **10** lần chạy, mỗi lần có ít nhất **3** hiện vật thuộc **3** loại khác nhau; và bước dọn phát cảnh báo **trước** khi bắt đầu ở **100%** số lần chạy.
- **SC-016**: **0** disclaimer bản quyền nào hiện ra trên **100%** số luồng thử: **20** lần tải âm thanh lên trải trên ít nhất **3** khe *(gồm 5 lần lặp cùng một file)* cộng **5** lần nhập gói contest có kèm âm thanh; và **0** dấu vết *"đã đọc disclaimer"* nào tồn tại trong nhật ký.
- **SC-017**: **100%** số lần chạy lại cùng một truy vấn lọc trên cùng một tập dữ liệu cho **cùng một** kết quả — thử **10** truy vấn, mỗi truy vấn chạy **5** lần.
- **SC-018**: Nội dung chính của màn đọc nhật ký nằm gọn trong **một** khung nhìn tham chiếu ở **100%** số trạng thái thử — thử với tập kết quả rỗng, **1** dòng, và một trang đầy.

## Out of Scope

Danh sách này ghi rõ những gì **không** thuộc feature này, và spec nào sở hữu chúng. Requirement nào nằm ở đây thì spec này **không** đặc tả lại, kể cả khi nó xuất hiện trong một acceptance scenario với vai trò tiền đề.

| Hạng mục | Chủ sở hữu |
|---|---|
| Tài khoản, vai, túi permission, catalog permission, cửa kiểm quyền nói chung, và bản thân việc gán `PERM-015` cho một vai | EPIC-001 — `specs/001`. Feature này chỉ **dùng** `PERM-015` như một cửa đã có |
| Cơ chế xuất/nhập gói contest, phép hỏi khi trùng tên đăng nhập, phiếu tài khoản in được, hai lựa chọn xử lý mật khẩu, phép kiểm độ mạnh cụm mật khẩu | EPIC-003 — `specs/003` (`PRD-REQ-104` FR-020 → FR-028). Feature này giữ đúng **bất biến mã hoá** và **dòng nhật ký riêng** |
| Mốc **câu khép**, cờ `revealAnswerAfterJudge`, và toàn bộ phép quyết định *trả hay không trả* đáp án — `GR-037` C3 → C10, §Mốc câu khép theo vòng, §Cấm, §Biên | EPIC-006, EPIC-007, EPIC-009 — `specs/006`, `specs/007`, `specs/009`. Feature này chỉ nhận mặt **audit** của rule |
| Quyền **công bố** đáp án (`PERM-044`), cú mở và cú đóng hiển thị bằng tay | EPIC-007 — `specs/007` |
| Nhật ký sự kiện trận `MatchEvent`, tính chất linear/append-only của nó, và phép hoàn nguyên bằng event đảo ngược | EPIC-006 — `specs/006`. Là **bảng khác**; nhật ký thao tác chỉ **trỏ tới** nó, không chép nội dung sang (FR-007) |
| Biên bản trận, gói kết quả và nhật ký sự kiện, gói kết quả rút gọn, thống kê ghi ngược kho đề, và vế *"xuất không phải một state"* của `PRD-REQ-080` | EPIC-010 — `specs/010`. Feature này nhận vế **ràng buộc lên bước dọn** của cùng requirement |
| **Job dọn dữ liệu tự động** — bộ hẹn giờ, lịch chạy, cơ chế thực thi | Ngoài phạm vi v1 (`docs/PRD.md` §11 EPIC-011 §Out of scope; `roadmap-post-v1.md` §8.1 xếp mốc ship là v1.5). Feature này đặc tả **điều kiện đặt lên nó**, không đặc tả nó |
| **Tuỳ chọn hiển thị biệt danh** | **Non-goal của sản phẩm** — NON-GOAL-018, `QĐ-090`. Hệ thống không có tuỳ chọn này ở phiên bản nào |
| Giới hạn kích thước media theo loại, rate-limit cổng khán giả, nút khoá cổng, hai hồ sơ triển khai | EPIC-012 — `specs/012` *(chưa dựng)* |
| Các khe âm thanh, nguồn phát, và việc gán file cho khe | EPIC-009 — `specs/009`. Feature này **không** có điểm chạm nào với âm thanh ngoài ràng buộc nghịch của FR-055 |
| **Disclaimer bản quyền âm thanh** | **Non-goal của sản phẩm** — NON-GOAL-020, `QĐ-144`. Feature này giữ đúng **ràng buộc nghịch** ở FR-055: hệ thống không hiện disclaimer ở bề mặt nào |
| Nội dung của từng dòng nhật ký mà một epic khác ghi *(hành động nào, đối tượng nào)* | Spec sở hữu thao tác đó. Feature này đặc tả **hình dạng chung** và **phủ sóng bắt buộc** |

## Traceability Matrix

| User story | PRD requirement | Game rules | Functional requirements | Acceptance scenarios |
|---|---|---|---|---|
| US-001 | `PRD-REQ-082`, `PRD-REQ-001` *(vế kênh public không định danh)* | `GR-037` *(mặt audit, qua FR-010)* · `NFR-14`, `NFR-15`, `NFR-17` · `INV-001` | FR-001 → FR-012 **+ FR-007a, FR-007b, FR-008a** | AC-001 → AC-010 **+ AC-005a** |
| US-002 | `PRD-REQ-082` | `GR-037` C1, C2, C5, C7, §Thứ tự đánh giá bước (6), §Điều kiện, §Bấm trùng, §Không đổi gì · `NFR-14`, `NFR-21` | FR-013 → FR-017 **+ FR-017a, FR-017b** | AC-008, AC-009, AC-011, AC-012, AC-012a, AC-012b |
| US-003 | `PRD-REQ-116`, `PRD-REQ-082` | `GR-037` *(loại dòng phải tra ra được)* · `NFR-11`, `NFR-17`, `NFR-18`, `NFR-36`, `NFR-37` · `PERM-015`, `PERM-062` | FR-018 → FR-028 **+ FR-018a, FR-018b, FR-018c, FR-028a, FR-028b, FR-028c, FR-007b, FR-051, FR-052, FR-054** | AC-013 → AC-022 **+ AC-014a, AC-014b, AC-014c, AC-022a, AC-005a, AC-043** |
| US-004 | `PRD-REQ-083`, `PRD-REQ-082` *(vế hạn riêng của nhật ký)* | — *(requirement khai `Related game rules: —`)* · `NFR-17`, `NFR-33`, `NFR-33b`, `NFR-34` · `T-021`, `T-022` | FR-029 → FR-035 **+ FR-029a, FR-029b, FR-029c, FR-002** | AC-023 → AC-028 **+ AC-028a** |
| US-005 | `PRD-REQ-104` *(vế EPIC-011)*, `PRD-REQ-082` | — *(requirement khai `Related game rules: —`)* · `NFR-24b`, `NFR-14`, `NFR-35b` · `RISK-010` | FR-036 → FR-041 **+ FR-052** | AC-029 → AC-033 |
| US-006 | `PRD-REQ-117`, `PRD-REQ-080` *(vế ràng buộc lên bước dọn)*, `PRD-REQ-083` | — *(requirement khai `Related game rules: —`)* · `NFR-35`, `NFR-35b`, `NFR-14` | FR-041, FR-041a → FR-041e, FR-042 → FR-046 | AC-033a → AC-033c, AC-034 → AC-038 |
| *(xuyên suốt)* | `PRD-REQ-082` · NON-GOAL-020 · `CLAUDE.md` §Zero-trust, §UX, §Quy ước code | `NFR-18`, `NFR-19`, `NFR-36`, `NFR-37` | FR-051 → FR-055 | AC-039a, AC-042, AC-043 |

### Bản đồ phủ bảng quyết định

> `GR-037` là **rule duy nhất** mà EPIC-011 khai. Feature này sở hữu **mặt audit** của nó; toàn bộ mặt *trả hay không trả đáp án* thuộc spec khác và **không** được đặc tả lại ở đây. Bảng dưới liệt kê **đủ mười ca** của bảng quyết định để cổng phủ kiểm được, kèm chủ sở hữu của từng ca.

| Ca `GR-037` | Kết quả trả đáp án *(chủ: `specs/006`, `007`, `009`)* | Mặt audit *(chủ: spec này)* | Acceptance scenarios |
|---|---|---|---|
| C1 — giữ `PERM-045`, phiên đang điều khiển | Trả đáp án; **ghi audit** | **Thuộc spec này** — một dòng mỗi lần trả | AC-008, AC-011 |
| C2 — giữ `PERM-045`, phiên không điều khiển | Trả đáp án; **ghi audit** | **Thuộc spec này** — ghi bất kể quyền điều khiển và bất kể tên vai | AC-009 |
| C3 — không giữ `PERM-045`, reveal TẮT | Không trả | **Thuộc spec này** — một dòng mang kết quả *bị từ chối* (FR-017) | AC-012 |
| C4 — không giữ `PERM-045`, reveal BẬT, câu chưa khép | Không trả | **Thuộc spec này** — một dòng mang kết quả *bị từ chối* | AC-012 |
| C5 — không giữ `PERM-045`, reveal BẬT, câu đã khép | Trả đáp án | **Hai đường tách bạch.** Yêu cầu **của một phiên** ⇒ FR-017 phủ, dòng nhật ký thao tác mang kết quả *đã trả*. Cú **đẩy hàng loạt** của engine tại mốc câu khép ⇒ **một** sự kiện trong nhật ký sự kiện trận (FR-017b), tra qua đường tham chiếu của FR-007; **0** dòng nhật ký thao tác | AC-012, AC-012b |
| C6 — Về đích, cửa sổ cướp đang mở | Không trả | **Thuộc spec này** — một dòng mang kết quả *bị từ chối*; cửa sổ cướp không đổi | AC-012a |
| C7 — câu bị bỏ qua, reveal BẬT | Trả đáp án | Như C5 — cú đẩy sinh **một** sự kiện trong nhật ký trận (FR-017b) | AC-012, AC-012b |
| C8 — câu khép bằng *Huỷ kết quả* | Không tự trả; admin mở tay được | Yêu cầu bị từ chối ⇒ dòng mang kết quả *bị từ chối* (FR-017); cú **mở tay** của admin là một thao tác riêng ⇒ FR-001 phủ | AC-012a |
| C8b — đang chờ kích hoạt tay sau *Huỷ kết quả* | Không trả | **Thuộc spec này** — một dòng mang kết quả *bị từ chối* | AC-012 |
| C9 — đáp án Chướng ngại vật | Ngoài phạm vi rule về **thời điểm** — theo `GR-012` | Lần xem của phiên giữ `PERM-045` vẫn ghi theo FR-013; cú lộ theo `GR-012` thuộc `specs/006` | AC-008 |
| C10 — câu khép, cờ BẬT, admin đang giữ cú **đóng tay** | Không trả | **Thuộc spec này** — một dòng mang kết quả *bị từ chối*; cú **đóng tay** của admin là một thao tác riêng ⇒ FR-001 phủ | AC-012 |
| §Bấm trùng — xem nhiều lần | *(không đổi)* | **Thuộc spec này** — mỗi lần một dòng; trạng thái trận không đổi | AC-011 |
| §Không đổi gì — xem đáp án không sinh event điểm | *(không đổi)* | **Thuộc spec này** — kiểm ở mặt *không có side effect* | AC-008, AC-011 |
| §Thứ tự đánh giá bước (6) | — | **Thuộc spec này** — ghi audit là **bước cuối**, sau mọi phép kiểm | AC-008, AC-012 |

> **Không rule nào khác vào bảng này.** Các requirement còn lại của epic (`PRD-REQ-083`, `PRD-REQ-116`, `PRD-REQ-080`, `PRD-REQ-104`) đều khai `Related game rules: —` hoặc chỉ khai `GR-037`, nên không có bảng quyết định nào khác cần phủ.
>

### Acceptance Scenarios (chi tiết)

> Mọi kịch bản giả định mốc thời gian là **server time** và hiển thị theo **UTC+7**. Trừ khi nói khác: bản cài là **hồ sơ máy chủ**, một phiên admin đang giữ quyền điều khiển, trận là trận **chính thức** bốn ghế ở mode **sân khấu**, và hạn lưu trữ đang ở giá trị mặc định.

#### Nhóm A — Nhật ký thao tác chung (US-001)

**AC-001** — *US*: US-001 · *FR*: FR-001, FR-003 · *GR*: `NFR-14`
**Given** một bản cài vừa dựng xong với đúng một tài khoản admin, nhật ký thao tác **rỗng**, và không contest nào tồn tại,
**When** admin đăng nhập thành công, tạo một contest, rồi đăng xuất,
**Then** nhật ký thao tác có **đúng ba** dòng theo thứ tự thời gian; cả ba nằm trong **cùng một** bảng, không phải ba bảng riêng; mỗi dòng mang đủ **năm trường** *(người thực hiện · hành động · đối tượng · thời điểm · địa chỉ nguồn)*; và **0** dòng nào thiếu một trường.

**AC-002** — *US*: US-001 · *FR*: FR-002 · *GR*: `INV-001`
**Given** một nhật ký thao tác đang có **50** dòng, và một phiên admin giữ đủ mọi permission có trong catalog,
**When** phiên đó thử sửa nội dung một dòng và thử xoá một dòng, cả qua bề mặt sản phẩm lẫn bằng lệnh gửi thẳng tới server,
**Then** **không** đường nào tồn tại trên bề mặt; mọi lệnh gửi thẳng đều **bị từ chối**; nhật ký vẫn có đúng **50** dòng với nội dung **không đổi**; và bản thân các lần thử vẫn để lại dòng nhật ký của chính chúng.

**AC-003** — *US*: US-001 · *FR*: FR-003, FR-004, FR-011 · *GR*: `GR-035`
**Given** một bản cài có một tài khoản `admin01`, và một máy khách ở một địa chỉ nguồn xác định,
**When** máy khách gửi một lần đăng nhập **sai mật khẩu** rồi một lần đăng nhập **đúng**,
**Then** nhật ký có **hai** dòng riêng, một mang hành động *đăng nhập thất bại* và một mang *đăng nhập thành công*; cả hai mang đúng **địa chỉ nguồn** của máy khách; thời điểm của cả hai là **server time**, không phải mốc do máy khách khai; và dòng thất bại vẫn có trường *người thực hiện* mang **tên đăng nhập đã khai**.

**AC-004** — *US*: US-001 · *FR*: FR-005, FR-006
**Given** một bản cài có sẵn một contest và một kho đề chứa **10** câu, admin đang đăng nhập,
**When** admin sửa nội dung một câu, xoá mềm một câu khác, và đổi tên contest,
**Then** nhật ký có **ba** dòng mới, mỗi dòng trỏ đúng đối tượng bị tác động; **0** thao tác nào trong ba thao tác đó không có dòng; và trạng thái của kho đề cùng contest đổi đúng như thao tác yêu cầu.

**AC-005** — *US*: US-001 · *FR*: FR-007, FR-007a · *GR*: `TERM-021`, `INV-001`
**Given** một trận đã chạy trọn một vòng Khởi động với ít nhất **12** sự kiện trong nhật ký sự kiện trận, gồm một cú hoàn nguyên, và nhật ký thao tác đang có `N` dòng,
**When** người tra mở nhật ký thao tác và tìm dấu vết của các sự kiện đó,
**Then** nhật ký thao tác giữ đường **trỏ tới** nhật ký sự kiện của trận và từ đó tra tiếp được **đủ 12** sự kiện; **0** bản sao nội dung sự kiện nào tồn tại trong nhật ký thao tác; nội dung của mỗi sự kiện có **đúng một** nguồn sự thật; và cú tra **không** làm đổi một sự kiện nào trong nhật ký sự kiện của trận.

**AC-005a** — *US*: US-001, US-003 · *FR*: FR-007b · *GR*: `NFR-17` · `QĐ-131`
**Given** một bản cài ở cấu hình mặc định — hạn nhật ký **24 tháng**, hạn trận `official` **12 tháng**; một trận đã đóng sổ **14 tháng** trước với **40** sự kiện, bước dọn đã chạy và **đã xoá** nhật ký sự kiện của trận đó; nhật ký thao tác vẫn giữ các dòng thuộc trận ấy; và `auditor01` đang mở màn đọc,
**When** `auditor01` tra khoảng thời gian phủ mốc của trận đó và mở một dòng trỏ tới một sự kiện,
**Then** dòng hiện **đủ năm trường**; đường đi tiếp tới nhật ký sự kiện **bị vô hiệu hoá**; dòng mang nhãn ***"nguồn đã hết hạn"***; nhãn đó **phân biệt được** với ca một trận **không có sự kiện nào**; và **0** dữ liệu nào bị đổi bởi cú tra.

**AC-006** — *US*: US-001 · *FR*: FR-008, FR-008a · *GR*: `TERM-007`, `PRD-REQ-001`
**Given** một trận đang chạy với cổng khán giả đang mở, nhật ký thao tác đang có `N` dòng, và một khán giả ẩn danh mở đường dẫn phòng từ một địa chỉ nguồn xác định,
**When** khán giả vào phòng, rồi sau đó admin đuổi khán giả đó,
**Then** nhật ký có **đúng một** dòng mới — dòng của cú **đuổi**, mang **admin đã bấm** ở trường *người thực hiện* và **khán giả bị đuổi** ở trường *đối tượng*; **0** dòng nào được ghi cho việc khán giả **vào phòng**; và **0** định danh mới, **0** giá trị ẩn danh, **0** chủ ngữ *"hệ thống"* nào được sinh ra cho kênh public.

**AC-007** — *US*: US-001 · *FR*: FR-009 · *GR*: `NFR-14`, `QĐ-128`
**Given** một contest đã dựng xong, admin có quyền xuất, và một thư mục đích không ghi được *(gây thất bại ở giữa quá trình xuất)*,
**When** admin gọi xuất **ba** lần: lần một thành công vào một đích ghi được, lần hai bị **từ chối** vì phiên gọi thiếu permission, lần ba **thất bại** vì đích không ghi được,
**Then** nhật ký có **đúng ba** dòng mới — không nhiều hơn, không ít hơn; mỗi dòng mang **kết quả** tương ứng *(thành công · bị từ chối · thất bại)*; và lần bị từ chối **không** làm đổi dữ liệu nào của contest.

**AC-008** — *US*: US-001, US-002 · *FR*: FR-010, FR-013, FR-016 · *GR*: `GR-037` C1, §Không đổi gì
**Given** một trận đang chạy ở vòng Khởi động, một câu **chưa khép** đang hiển thị, điểm bốn ghế là `A=20 · B=10 · C=10 · D=0`, và một phiên giữ `PERM-045` **đang giữ quyền điều khiển**,
**When** phiên đó mở đáp án chuẩn của câu đang chạy,
**Then** server trả đáp án; nhật ký có **đúng một** dòng mới mang hành động *xem đáp án* và trỏ đúng câu đó; **0** event nào được sinh trong nhật ký sự kiện của trận; điểm bốn ghế **không đổi**; và trạng thái trận **không đổi**.

**AC-009** — *US*: US-002 · *FR*: FR-013, FR-014 · *GR*: `GR-037` C2, `QĐ-094`
**Given** cùng trận và cùng câu chưa khép của AC-008, và một phiên vai **MC** — giữ `PERM-045` nhưng **không** giữ quyền điều khiển và **không** giữ `PERM-044`,
**When** phiên MC mở đáp án chuẩn của câu đang chạy,
**Then** server trả đáp án cho **riêng phiên đó**; nhật ký có **đúng một** dòng mới, phân biệt được người thực hiện là phiên MC; việc ghi xảy ra **không** phụ thuộc vào việc phiên có giữ quyền điều khiển; và **0** màn nào khác nhận đáp án — đây là hành vi **đọc**, không phải công bố.

**AC-010** — *US*: US-001 · *FR*: FR-012 · *GR*: `NFR-17`
**Given** một nhật ký có một dòng trỏ tới câu `Q-041` và một dòng trỏ tới tài khoản `setter03`,
**When** admin **xoá mềm** câu `Q-041` và **vô hiệu hoá** tài khoản `setter03`,
**Then** cả hai dòng nhật ký cũ **vẫn còn** và vẫn đọc được đủ năm trường; **0** dòng nào biến mất hay trở thành không đọc được; và hai thao tác vừa làm để lại **hai dòng mới**.

#### Nhóm B — Dấu vết của mỗi lần xem đáp án (US-002)

**AC-011** — *US*: US-002 · *FR*: FR-015, FR-016 · *GR*: `GR-037` §Bấm trùng, §Không đổi gì
**Given** cùng trận và cùng câu chưa khép của AC-008, phiên giữ `PERM-045`, và nhật ký đang có `N` dòng,
**When** phiên đó mở đáp án của **cùng một câu ba lần liên tiếp** trong vòng vài giây,
**Then** nhật ký có **`N+3`** dòng — mỗi lần một dòng, **không** gộp; ba dòng có ba mốc thời gian khác nhau; **0** event nào được sinh trong nhật ký sự kiện trận; và điểm cùng trạng thái trận **không đổi** sau cả ba lần.

**AC-012** — *US*: US-002 · *FR*: FR-017, FR-017a · *GR*: `GR-037` C3, C4, C6, C8, C8b, C10, §Thứ tự đánh giá bước (6), §Điều kiện
**Given** một trận đang chạy với một câu **chưa khép**, cờ `revealAnswerAfterJudge` **BẬT**, một phiên thí sinh **không** giữ `PERM-045`, và nhật ký đang có `N` dòng,
**When** phiên thí sinh gửi **ba** yêu cầu xem đáp án chuẩn của câu đang chạy,
**Then** server **im lặng** cả ba lần và **0** byte đáp án nào rời server; nhật ký có **`N+3`** dòng, mỗi dòng mang kết quả *bị từ chối*; ba dòng **cùng loại** với dòng của một lần xem thành công, phân biệt nhau bằng trường *kết quả*; chuỗi ba lần từ chối liên tiếp **tra ra được** trên màn đọc; và điểm cùng trạng thái trận **không đổi**.

**AC-012a** — *US*: US-002 · *FR*: FR-017 · *GR*: `GR-037` C6, C8
**Given** vòng Về đích, người thi chính vừa bị chấm **Sai** nên cửa sổ cướp quyền **đang mở** *(`GR-037` C6 — câu **chưa** khép)*, và một phiên khán giả cùng một phiên thí sinh đều **không** giữ `PERM-045`,
**When** cả hai phiên gửi yêu cầu xem đáp án trong lúc cửa sổ cướp còn mở,
**Then** cả hai **bị từ chối** và **0** byte đáp án nào rời server — cơ chế cướp quyền còn nguyên; nhật ký có **hai** dòng mang kết quả *bị từ chối*, phân biệt được hai phiên; và cửa sổ cướp quyền cùng mọi con số của luật **không đổi**.

**AC-012b** — *US*: US-001, US-002 · *FR*: FR-017b, FR-007 · *GR*: `GR-037` C5, C7 · `QĐ-132`, `QĐ-134`
**Given** một trận có cờ `revealAnswerAfterJudge` **bật**, một câu Về đích vừa được chấm và cửa sổ cướp quyền đã đóng nên câu **đã khép**, cùng lúc có **4** phiên thí sinh, **30** phiên khán giả và **1** overlay đang kết nối; nhật ký thao tác đang có `N` dòng và nhật ký sự kiện trận đang có `M` sự kiện,
**When** engine đẩy đáp án tới toàn bộ 35 điểm nhận đó tại mốc câu khép,
**Then** nhật ký sự kiện trận có **đúng một** sự kiện mới cho cú đẩy, mang mốc thời gian và câu bị lộ — **không** phải 35 sự kiện; nhật ký thao tác vẫn có **đúng `N`** dòng, **0** dòng riêng nào cho cú đẩy và **0** chủ ngữ *"hệ thống"* nào được sinh ra; sự kiện đó tra ra được từ nhật ký thao tác qua đường **tham chiếu** của FR-007; và nếu ngay sau đó một phiên giữ `PERM-045` **yêu cầu** xem đáp án của cùng câu thì yêu cầu đó vẫn để lại **một** dòng nhật ký thao tác riêng theo FR-017 — hai đường **kéo** và **đẩy** không trộn vào nhau.

#### Nhóm C — Màn ĐỌC nhật ký thao tác (US-003)

**AC-013** — *US*: US-003 · *FR*: FR-018
**Given** một bản cài có nhật ký chứa ít nhất **200** dòng trải qua ít nhất **năm** loại thao tác khác nhau, và một tài khoản `auditor01` được gán một vai **có** `PERM-015` `audit.read`,
**When** `auditor01` mở bề mặt đọc nhật ký thao tác,
**Then** màn mở được và hiện các dòng nhật ký; **0** dòng nào bị lọc bỏ vì vai của người tra; và việc mở màn **không** làm đổi một dòng nào đang có.

**AC-014** — *US*: US-003 · *FR*: FR-018, FR-019, FR-051, FR-052 · *GR*: `NFR-18`, `QĐ-094`
**Given** cùng bản cài của AC-013, và **bốn** phiên: một vai *Người ra đề*, một vai *MC*, một vai *Thí sinh*, và một vai tuỳ biến giữ nhiều permission nhưng **không** giữ `PERM-015`,
**When** cả bốn phiên thử mở bề mặt đọc nhật ký, mỗi phiên thử cả qua giao diện lẫn bằng lệnh gửi thẳng tới server,
**Then** cả bốn **đều bị từ chối**; **0** byte nội dung dòng nhật ký nào rời server ở bất kỳ lần thử nào; phép từ chối được quyết bởi **permission**, không bởi tên vai — chứng minh bằng cách gán `PERM-015` cho **vai MC** và xác nhận phiên MC lúc đó vào được; và **0** dữ liệu nào đổi ở mọi lần bị từ chối.

**AC-014a** — *US*: US-003 · *FR*: FR-018a, FR-018b, FR-019, FR-021 · *GR*: `QĐ-094`
**Given** một bản cài chạy **hai** contest `C1` và `C2`, nhật ký chứa dòng thuộc `C1`, dòng thuộc `C2`, và dòng **không thuộc contest nào** *(đăng nhập, sửa kho đề, tạo tài khoản)*; một phiên `owner01` giữ `PERM-062` **chỉ trên `C1`**, và **không** giữ `PERM-015`,
**When** `owner01` tra một khoảng thời gian phủ **cả ba** loại dòng,
**Then** kết quả chứa **đúng** các dòng có **đối tượng** thuộc `C1` — trận, cấu hình, ghế, lần xuất/nhập gói, lần xem đáp án trong trận của `C1`; **0** dòng thuộc `C2` và **0** dòng không thuộc contest nào rời server, và phản hồi **không** tiết lộ chúng tồn tại; `owner01` **không** mở được bề mặt đọc cấp bản cài của FR-018; ngược lại một phiên chỉ giữ `PERM-015` **không** bị đòi thêm `PERM-062`.

**AC-014b** — *US*: US-003 · *FR*: FR-018b
**Given** cùng bản cài của AC-014a; thí sinh `ts07` **đã được gán** vào contest `C1`; nhật ký chứa **ba** dòng đăng nhập của `ts07` — một trong lúc trận của `C1` đang chạy, hai ở ngày không có trận nào,
**When** `owner01` *(chỉ giữ `PERM-062` trên `C1`)* tra khoảng thời gian phủ cả ba dòng đó,
**Then** **0** trong ba dòng đăng nhập lọt vào kết quả — phân loại theo **đối tượng**, không theo **người thực hiện**, kể cả khi người đó đã gán vào contest; và mốc thời gian của một dòng nằm trong lúc trận đang chạy **không** làm nó thuộc contest.

**AC-014c** — *US*: US-003 · *FR*: FR-018a, FR-018c · `QĐ-130`, `QĐ-135`
**Given** một bản cài **vừa dựng xong**, chưa ai tạo vai tuỳ biến nào, với đúng một tài khoản mang vai dựng sẵn **Quản trị** và ba tài khoản mang ba vai dựng sẵn còn lại *(Người ra đề, MC, Thí sinh)*,
**When** cả bốn tài khoản lần lượt mở bề mặt đọc **cấp bản cài** rồi bề mặt tra cứu **phạm vi contest**,
**Then** tài khoản Quản trị mở được **cả hai** bề mặt — nó giữ `PERM-015` **và** `PERM-062` `audit.readContest` ngay từ lúc dựng, **0** vai tuỳ biến nào phải tạo trước; ba tài khoản còn lại **không** mở được bề mặt nào trong hai, và **0** byte nội dung dòng nhật ký nào rời server cho chúng; và cửa kiểm của bề mặt phạm vi contest hỏi **`PERM-062`**, **không** chấp nhận `PERM-015` thay thế.

**AC-015** — *US*: US-003 · *FR*: FR-020, FR-021, FR-024 · *GR*: `NFR-17`
**Given** một nhật ký chứa, quanh mốc `14:30:00` ngày `2026-08-04` (UTC+7), một chuỗi gồm: một dòng *xem đáp án* của admin, hai dòng phán quyết trong trận, một dòng *in biên bản* do EPIC-010 ghi, và một dòng *xuất danh sách người tham gia*; cùng khoảng **300** dòng nhiễu ở các mốc khác,
**When** `auditor01` tra bằng khoảng thời gian `14:25` → `14:35` cùng ngày,
**Then** kết quả chứa **đủ năm** dòng của chuỗi đó, xếp theo **thứ tự thời gian**; các dòng do **các epic khác** ghi vào đều tra ra được; và **0** dòng nào của chuỗi bị thiếu.

**AC-016** — *US*: US-003 · *FR*: FR-022, FR-051, FR-052 · *GR*: `NFR-18`
**Given** `auditor01` đang mở bề mặt đọc với **50** dòng đang hiển thị,
**When** `auditor01` rà toàn bộ thao tác có trên màn, rồi gửi thẳng tới server năm dạng lệnh khác nhau nhằm sửa, xoá, hoặc đánh dấu vô hiệu một dòng,
**Then** **0** thao tác sửa/xoá/vô hiệu nào tồn tại trên bề mặt; cả năm lệnh gửi thẳng đều **bị từ chối** ở server; nội dung **50** dòng **không đổi**; và tổng số dòng của nhật ký **không đổi** ngoài các dòng ghi lại chính năm lần thử đó.

**AC-017** — *US*: US-003 · *FR*: FR-020, FR-024
**Given** một nhật ký chứa dòng của **ba** người thực hiện khác nhau, **sáu** loại hành động khác nhau, và trỏ tới **bốn** đối tượng khác nhau,
**When** `auditor01` tra bằng một truy vấn **kết hợp ba trục** — một người thực hiện, một loại hành động, và một khoảng thời gian,
**Then** kết quả chứa **đúng** tập dòng thoả cả ba điều kiện; **0** dòng nào thoả một hoặc hai điều kiện lọt vào; và cùng truy vấn chạy trên trục *đối tượng* thay cho trục *hành động* cũng cho tập đúng.

**AC-018** — *US*: US-003 · *FR*: FR-023
**Given** một dòng nhật ký có thời điểm gốc là `07:30:00` UTC ngày `2026-08-04`,
**When** `auditor01` xem dòng đó trên bề mặt đọc,
**Then** mốc hiển thị là `14:30:00` ngày `2026-08-04` **theo UTC+7**; **0** thiết lập múi giờ theo người dùng tồn tại; và mọi dòng khác trên cùng màn cũng hiển thị theo UTC+7.

**AC-019** — *US*: US-003 · *FR*: FR-025 · *GR*: `NFR-14`
**Given** một nhật ký đang có `N` dòng và `auditor01` chưa mở bề mặt đọc lần nào trong phiên hiện tại,
**When** `auditor01` mở bề mặt đọc rồi chạy **hai** truy vấn lọc khác nhau,
**Then** nhật ký có thêm dòng cho **chính các thao tác đó** — mở màn và mỗi lần tra đều là thao tác của một vai và không có ngoại lệ nào ở FR-001; các dòng mới mang đủ năm trường; và chúng xuất hiện trong kết quả của lần tra tiếp theo nếu khoảng thời gian phủ tới chúng.

**AC-020** — *US*: US-003 · *FR*: FR-026
**Given** một nhật ký **không** có thao tác nào mới xảy ra giữa hai lần tra, và `auditor01` vừa chạy một truy vấn cho ra **17** dòng,
**When** `auditor01` chạy **lại đúng** truy vấn đó,
**Then** kết quả là **đúng 17 dòng giống hệt**; **0** dữ liệu nghiệp vụ nào đổi bởi hai lần tra; và khác biệt duy nhất trong hệ thống là dòng nhật ký ghi lại chính hai lần tra đó (FR-025).

**AC-021** — *US*: US-003 · *FR*: FR-027 · *GR*: `NFR-36`
**Given** một nhật ký trong đó **không** dòng nào rơi vào khoảng `01:00` → `02:00` ngày `2026-08-04`,
**When** `auditor01` tra bằng đúng khoảng đó,
**Then** trong lúc tra, màn hiện **trạng thái đang xử lý** rõ ràng; khi xong, màn hiện một **trạng thái rỗng đọc được** nói rõ không dòng nào khớp; trạng thái rỗng **không** được trình bày như một lỗi; và các trục lọc vẫn dùng lại được ngay.

**AC-022** — *US*: US-003 · *FR*: FR-028
**Given** `auditor01` đang mở bề mặt đọc trên một nhật ký có **1000** dòng, và nhật ký đang ở trạng thái ổn định,
**When** `auditor01` tra với mốc kết thúc **sớm hơn** mốc bắt đầu,
**Then** yêu cầu **bị từ chối kèm lý do nêu rõ** rằng khoảng thời gian không hợp lệ; hệ thống **không** tự hoán đổi hai mốc và **không** chạy truy vấn; **0** dòng kết quả nào rời server; **0** dữ liệu nào đổi; và các trục lọc vẫn dùng lại được ngay để sửa đầu vào.

**AC-022a** — *US*: US-003 · *FR*: FR-028a, FR-028b, FR-028c
**Given** một bản cài mới dựng — biên độ dài khoảng thời gian ở giá trị mặc định **24 tháng**, hạn nhật ký ở mặc định 24 tháng — và nhật ký chứa dòng trải dài hơn 24 tháng *(vì admin đã nâng hạn nhật ký lên 36 tháng)*,
**When** `auditor01` *(giữ `PERM-015`)* tra một khoảng **30 tháng**; rồi tra một khoảng **24 tháng** cho ra nhiều dòng hơn một trang; rồi `owner01` *(chỉ giữ `PERM-062`)* tra cùng khoảng **30 tháng** đó,
**Then** yêu cầu thứ nhất **bị từ chối kèm lý do nêu rõ biên**, và **0** tập kết quả bị cắt bớt nào được trả về trong im lặng; yêu cầu thứ hai **thành công** và trả **đủ** số dòng qua **phân trang**, **không** bị giới hạn số dòng nào ngoài biên thời gian; yêu cầu thứ ba bị từ chối bởi **cùng** biên và **cùng** lý do — biên **không** khác nhau theo cửa vào.

#### Nhóm D — Hạn lưu trữ (US-004)

**AC-023** — *US*: US-004 · *FR*: FR-029, FR-029c, FR-030, FR-035 · *GR*: `NFR-33`, `NFR-33b`, `NFR-34`
**Given** một bản cài **v1 vừa dựng xong**, chưa ai đổi cấu hình gì, và luồng `matchPurpose: practice` **chưa được bật** ở phiên bản này,
**When** admin mở phần cấu hình hạn lưu trữ và đọc giá trị hiện tại,
**Then** bề mặt hiện **ba** ô ở **phạm vi bản cài**: trận **chính thức** = **12 tháng**, trận **luyện tập** = **3 tháng**, và **nhật ký thao tác** = **24 tháng**; ô `practice` **hiện và đặt được ngay** dù chưa trận nào mang giá trị đó; **0** giá trị chung nào được dùng cho nhiều trục; và **0** bề mặt nào cho một contest ghi đè giá trị của bản cài.

**AC-024** — *US*: US-004 · *FR*: FR-031
**Given** một bản cài đang ở giá trị mặc định của AC-023, và mã nguồn không được sửa,
**When** admin đổi hạn trận chính thức thành **24 tháng** và hạn trận luyện tập thành **1 tháng**, rồi đọc lại,
**Then** hai giá trị mới có hiệu lực; việc đổi hoàn tất **không cần** một lần build hay một lần sửa mã nào; và hai con số `12` cùng `3` **không** xuất hiện dưới dạng hằng số trong sản phẩm — chúng chỉ là giá trị khởi tạo của cấu hình.

**AC-025** — *US*: US-004 · *FR*: FR-032 · *GR*: `QĐ-091`, `T-022`
**Given** một bản cài với hạn lưu trữ đặt ở một giá trị bất thường *(ví dụ 1 tháng cho trận chính thức)*, và một trận đang chạy giữa vòng Tăng tốc,
**When** trận chạy tiếp qua hết vòng và được chốt,
**Then** **0** phép quyết định nào của luật hay của máy trạng thái đọc giá trị hạn lưu trữ; điểm, thứ hạng, phép phân định hoà và mọi chuyển trạng thái ra kết quả **giống hệt** một trận chạy trên bản cài để giá trị mặc định; và giá trị hạn lưu trữ **không** xuất hiện ở bất kỳ ràng buộc chuyển trạng thái nào.

**AC-026** — *US*: US-004 · *FR*: FR-033 · *GR*: `QĐ-091` §Rủi ro
**Given** một bản cài ở giá trị mặc định,
**When** admin đặt hạn lưu trữ trận chính thức thành một giá trị **rất dài** *(ví dụ 120 tháng)*,
**Then** hệ thống **chấp nhận** giá trị đó; **0** biên trên nào từ chối nó; **0** cảnh báo bắt buộc nào chặn cú đặt; và giá trị mới có hiệu lực ngay.

**AC-027** — *US*: US-004 · *FR*: FR-034 · *GR*: `NFR-14`
**Given** hạn lưu trữ trận chính thức đang là **12 tháng**, và nhật ký đang có `N` dòng,
**When** admin bấm lưu với giá trị **vẫn là 12 tháng** — tức không đổi gì,
**Then** nhật ký có **`N+1`** dòng; dòng mới mang hành động *đổi hạn lưu trữ* và đủ năm trường; và giá trị cấu hình vẫn là 12 tháng — dòng nhật ký ghi lại **quyết định**, không chỉ ghi lại **thay đổi**.

**AC-028** — *US*: US-004 · *FR*: FR-002, FR-029, FR-029a, FR-035
**Given** một bản cài ở **cấu hình mặc định** — trận chính thức **12 tháng**, nhật ký thao tác **24 tháng** — và dữ liệu gồm: một trận đã đóng sổ **13 tháng** trước, media do người dùng tải lên cho trận đó, tài khoản dựng từ một gói nhập, và **1000** dòng nhật ký thao tác trong đó **600** dòng thuộc trận đó và **400** dòng **không thuộc trận nào**,
**When** bước dọn chạy,
**Then** dữ liệu trận và media của nó **bị dọn**; **cả 1000** dòng nhật ký thao tác **còn nguyên** — chúng đi theo **hạn riêng 24 tháng**, không theo hạn của trận, kể cả 600 dòng thuộc trận vừa bị dọn; tài khoản dựng từ gói nhập **không** bị đụng tới; **0** đường sửa hay xoá một dòng nào xuất hiện trên bề mặt sản phẩm trong suốt lần chạy; bước dọn để lại dòng nhật ký của chính nó; và từ lúc này các dòng trỏ tới nhật ký sự kiện của trận đó mang nhãn ***"nguồn đã hết hạn"*** (AC-005a).

**AC-028a** — *US*: US-004 · *FR*: FR-029b · *GR*: `NFR-17`, `QĐ-091` §Rủi ro
**Given** một bản cài với hạn trận chính thức đặt ở **18 tháng**, và hạn nhật ký thao tác đang ở mặc định **24 tháng**,
**When** admin đổi hạn nhật ký thao tác xuống **6 tháng** — thấp hơn hẳn hạn dài nhất của trục mục đích trận,
**Then** hệ thống **cảnh báo** và **nêu cái giá** — chuỗi phân xử sẽ hết hạn trước dữ liệu mà nó mô tả; nhưng hệ thống **chấp nhận** giá trị đó; **0** biên nào cưỡng chế quan hệ giữa hai trục; giá trị mới có hiệu lực; và cú đổi để lại một dòng nhật ký.

#### Nhóm E — Dữ liệu cá nhân rời hệ thống (US-005)

**AC-029** — *US*: US-005 · *FR*: FR-036, FR-037 · *GR*: `NFR-24b`
**Given** một contest đã gán **10** người — 4 thí sinh, 1 MC, 1 tài khoản admin của contest, 4 người khác — và admin chọn xuất gói **có kèm** danh sách người tham gia với lựa chọn *"tạo mật khẩu mới lúc nhập"*, đặt một cụm mật khẩu,
**When** admin hoàn tất lần xuất,
**Then** gói ra được; khám nội dung tệp cho thấy **0** byte nào đọc được tên, trường hay lớp của bất kỳ ai trong mười người; **0** byte nào là chìa khoá giải mã; và cùng kết quả khi lặp lại với lựa chọn *"giữ mật khẩu hiện tại"*.

**AC-030** — *US*: US-005 · *FR*: FR-036, FR-038, FR-052 · *GR*: `RISK-010`
**Given** cùng contest của AC-029,
**When** người vận hành rà **mọi** đường mà danh sách người tham gia có thể rời hệ thống, và thử tìm một thiết lập, một cờ, hay một chế độ nào tắt được bước mã hoá,
**Then** **0** đường nào cho ra danh sách ở dạng đọc được; **0** thiết lập nào tắt được mã hoá; **0** biến thể *"để nguyên chữ cho dễ đọc"* tồn tại; và mọi thử nghiệm đều **không** làm đổi dữ liệu của contest.

**AC-031** — *US*: US-005 · *FR*: FR-039 · *GR*: `QĐ-086` §Hệ quả
**Given** cùng contest của AC-029, nhật ký đang có `N` dòng,
**When** admin xuất một gói **có kèm** danh sách, rồi nhập gói đó vào một bản cài khác,
**Then** ở bản nguồn có **một dòng riêng** cho lần xuất danh sách, **phân biệt được** với dòng của lần xuất câu hỏi; ở bản đích có **một dòng riêng** cho lần nhập danh sách, phân biệt được với dòng của lần nhập câu hỏi; và cả hai dòng mang đủ năm trường.

**AC-032** — *US*: US-005 · *FR*: FR-040 · *GR*: `QĐ-086` vế 1
**Given** cùng contest của AC-029, nhật ký đang có `N` dòng,
**When** admin xuất một gói **không kèm** danh sách người tham gia,
**Then** gói ra được và là gói **hợp lệ**; bên nhập **không** bị hỏi cụm mật khẩu nào; nhật ký có dòng cho lần xuất gói *(thuộc `specs/003`)* nhưng **0** dòng riêng cho danh sách; và **0** dữ liệu cá nhân nào có trong gói.

**AC-033** — *US*: US-005, US-006 · *FR*: FR-041 · *GR*: `NFR-35b`, `QĐ-128`
**Given** một gói contest kèm danh sách đã được xuất ra một ổ USB **3 tháng trước**, và hạn lưu trữ của bản cài là **1 tháng**,
**When** thời điểm quá hạn của dữ liệu nguồn tới và bước dọn chạy,
**Then** gói trên USB **không** bị đụng tới — nó nằm ngoài hệ thống; hệ thống **0** bản ghi nào theo dõi vòng đời của gói đó; và **không** có trạng thái *"gói đã xuất"* nào tồn tại để phân loại.

#### Nhóm F — Bước dọn dữ liệu theo hạn lưu trữ (US-006)

**AC-033a** — *US*: US-006 · *FR*: FR-041a, FR-041c, FR-042, FR-046 · *GR*: `NFR-35` · `PRD-REQ-117`
**Given** một bản cài có hạn lưu trữ đặt ngắn và đã có dữ liệu quá hạn ở **cả hai trục** — dữ liệu trận cùng media quá hạn theo mục đích trận, và dòng nhật ký thao tác quá hạn theo trục riêng; một biên bản đã được xuất ra khỏi hệ thống; một phiên giữ `PERM-063`,
**When** phiên đó mở màn dọn dữ liệu, đối chiếu bản kê, bấm chạy, rồi bấm **Yes** ở dialog,
**Then** bản kê hiện **đúng** tập dữ liệu quá hạn của cả hai trục; sau khi nhận Yes, dữ liệu quá hạn bị xoá; **hiện vật đã xuất còn nguyên**; một dòng nhật ký được ghi cho lần chạy; và dữ liệu **chưa** quá hạn **không đổi**.

**AC-033b** — *US*: US-006 · *FR*: FR-041a, FR-041b · *GR*: `NFR-18` · `PRD-REQ-117`
**Given** cùng bản cài của AC-033a; hai phiên — một phiên chỉ giữ `PERM-021` `retention.manage`, một phiên không giữ permission nào trong hai,
**When** cả hai yêu cầu mở màn dọn dữ liệu, cả qua giao diện lẫn gửi thẳng tới server,
**Then** cả hai bị **từ chối**; **0** nội dung bản kê phạm vi nào rời server; **0** byte dữ liệu nào bị xoá; và phiên giữ `PERM-021` **vẫn** đặt được hạn lưu trữ như thường — hai permission không suy ra nhau.

**AC-033c** — *US*: US-006 · *FR*: FR-041e, FR-046a · `PRD-REQ-117`
**Given** một bản cài có dữ liệu quá hạn ở cả hai trục, và **không** phiên nào bấm chạy bước dọn,
**When** hệ thống chạy liên tục qua mọi mốc thời gian mà một lịch trình có thể kích hoạt,
**Then** **0** byte dữ liệu nào bị xoá; **0** dòng nhật ký nào cho một lần chạy bước dọn; **0** thông báo theo lịch nào được phát; và dữ liệu quá hạn chỉ rời hệ thống khi có người bấm.

**AC-034** — *US*: US-006 · *FR*: FR-042 · *GR*: `NFR-35`
**Given** một bản cài có dữ liệu đã quá hạn lưu trữ và một bước dọn kích hoạt được bằng tay,
**When** bước dọn được kích hoạt,
**Then** một **cảnh báo phát ra trước khi bước dọn bắt đầu xoá bất cứ thứ gì**; **0** dữ liệu nào bị xoá trước mốc cảnh báo; và bước dọn chỉ tiếp tục sau mốc đó.

**AC-035** — *US*: US-006 · *FR*: FR-043 · *GR*: `NFR-35`, `NFR-35b`
**Given** một trận đã đóng sổ và đã quá hạn lưu trữ, và **ba** hiện vật đã xuất từ trận đó đang nằm trên đĩa: một biên bản, một gói kết quả, một gói rút gọn,
**When** bước dọn chạy tới hết,
**Then** cả **ba** hiện vật **còn nguyên**, đủ byte, mở được và đọc được; **0** hiện vật nào bị sửa, bị cắt hay bị xoá; và việc gặp chúng **không** làm bước dọn dừng lại.

**AC-036** — *US*: US-006 · *FR*: FR-044
**Given** cùng tình huống của AC-035, nhưng bước dọn bị ngắt giữa chừng,
**When** bước dọn kết thúc bất thường,
**Then** **ba** hiện vật đã xuất vẫn **còn nguyên** — ràng buộc *không đụng hiện vật đã xuất* đúng ở **mọi** thời điểm của lần chạy, không chỉ ở lần chạy thành công; và lần chạy hỏng đó **không** để lại một trạng thái dở dang nào mà lần chạy sau phải dọn.

**AC-037** — *US*: US-006 · *FR*: FR-045 · *GR*: `NFR-14`
**Given** một bản cài có dữ liệu quá hạn và nhật ký đang có `N` dòng,
**When** bước dọn chạy một lần,
**Then** nhật ký có ít nhất **một** dòng mới cho lần chạy đó, mang đủ năm trường; và dòng đó tra ra được từ bề mặt đọc của US-003.

**AC-038** — *US*: US-006 · *FR*: FR-046, FR-046a · *GR*: `NFR-35`, `CLAUDE.md` §Dialog xác nhận
**Given** một bản cài có dữ liệu quá hạn thuộc **hai** trục — dữ liệu của một trận `official` quá 12 tháng và các dòng nhật ký quá 24 tháng — và **hai** phiên admin đang đăng nhập,
**When** phiên thứ nhất bấm kích hoạt bước dọn,
**Then** một **dialog Yes/No** hiện trên **riêng** phiên đó, liệt kê **phạm vi sắp bị xoá** đủ để nhận ra thuộc trục nào; **0** byte nào bị xoá trong lúc dialog đang mở; bấm **No** ⇒ **0** dữ liệu nào đổi; bấm **Yes** ⇒ bước dọn chạy; **0** cảnh báo nào phát tới phiên thứ hai; và **0** thông báo báo-trước-theo-lịch nào tồn tại trên bất kỳ bề mặt nào của v1.

#### Nhóm G — Ràng buộc xuyên suốt

**AC-039a** — *US*: xuyên suốt · *FR*: FR-055 · NON-GOAL-020
**Given** một contest đang dựng với khe *đếm giờ Khởi động* đang trống, và một gói contest **có kèm** ba file âm thanh,
**When** admin tải một file âm thanh lên cho khe đó, tải **cùng file** đó lên lần nữa, rồi nhập gói contest kia,
**Then** **0** disclaimer bản quyền nào hiện ra ở bất kỳ mốc nào trong ba luồng; **0** ô tích, **0** nút *"Tôi hiểu"*, **0** dòng nhật ký nào về việc đọc disclaimer tồn tại; và cả ba luồng hoàn tất bình thường.

**AC-042** — *US*: xuyên suốt · *FR*: FR-053 · *GR*: `CLAUDE.md` §Quy ước code
**Given** toàn bộ bề mặt giao diện của feature này — màn đọc nhật ký và phần cấu hình hạn lưu trữ,
**When** người rà kiểm toàn bộ chuỗi hiển thị,
**Then** **100%** chuỗi nằm ở file hằng số, **0** chuỗi nào viết cứng trong thành phần hiển thị; toàn bộ là **tiếng Việt**; và **0** cơ chế i18n nào tồn tại.

**AC-043** — *US*: US-003 · *FR*: FR-054 · *GR*: `NFR-37`, `CLAUDE.md` §UX
**Given** bề mặt đọc nhật ký ở ba trạng thái: kết quả **rỗng**, **một** dòng, và một tập kết quả lớn hơn số dòng vừa một màn,
**When** người rà xem cả ba trạng thái trên khung nhìn tham chiếu của dự án,
**Then** ở cả ba, nội dung chính — bộ lọc, tiêu đề bảng, các dòng, và bộ điều hướng trang — nằm **gọn trong một khung nhìn**; **0** phần tử quan trọng nào phải cuộn cả trang mới thấy; và tập kết quả lớn được xử lý bằng **phân trang** thay vì kéo dài trang.

## Assumptions

- **Feature này không giữ trạng thái nghiệp vụ của trận.** Mọi thứ nó ghi là dấu vết của một thao tác đã xảy ra ở nơi khác; mọi thứ nó đọc là dấu vết đó. Ngoại lệ duy nhất là **hạn lưu trữ** — một giá trị cấu hình cấp bản cài, không tham gia phép quyết định nào của luật.
- **Nhật ký thao tác được giả định là MỘT bảng chung**, theo đúng chữ của `PRD-REQ-082` *("một nhật ký chung")* và `CLAUDE.md` §Nhật ký thao tác *("bảng `AuditLog` chung, append-only")*. Spec này không giả định gì thêm về cách bảng đó được tổ chức bên trong.
- **Mọi epic khác đã có FR ghi vào nhật ký này.** Spec này giả định các FR đó tồn tại và đúng, và không đặc tả lại chúng. Nếu một epic bỏ sót một loại thao tác thì lỗi nằm ở spec của epic đó, không ở đây — nhưng **SC-001** của spec này là chỗ phát hiện ra.
- **Hai cửa đọc nhật ký là hai permission tách bạch, không phải giả định của spec này**: `PERM-015` `audit.read` ở phạm vi **bản cài**, `PERM-062` `audit.readContest` ở phạm vi **`CONTEST`**. Vai dựng sẵn **Quản trị** giữ **cả hai** ngay ở bản cài mới dựng; ba vai dựng sẵn còn lại không giữ cái nào (FR-018c). Việc gán hai permission đó cho một **vai tuỳ biến** là thao tác của EPIC-001.
- **Nhật ký thao tác và nhật ký sự kiện trận có HAI vòng đời độc lập.** Trừ AC-005a và AC-028, mọi kịch bản giả định **chưa** tới mốc mà một trong hai bị dọn. Ở cấu hình mặc định *(nhật ký 24 tháng, `official` 12 tháng)*, trạng thái *nguồn đã hết hạn* là **thường trực** từ năm thứ hai — không phải một ca biên.
- **Bước dọn dữ liệu được giả định kích hoạt được bằng tay ở v1** cho mục đích kiểm thử hai ràng buộc của US-006. `docs/` khai **job tự động** thuộc v1.5 nhưng khai **ràng buộc** thuộc v1; một ràng buộc không kiểm được ở v1 thì không phải một ràng buộc. Đây là giả định **nhỏ nhất** đủ để hai FR của US-006 kiểm thử được, và nó không mở thêm khả năng nào ngoài phạm vi.
- **Mode trả lời, số ghế, và `matchPurpose` không ảnh hưởng tới feature này** ngoài đúng một chỗ: hạn lưu trữ đọc `matchPurpose` để chọn giá trị. Mọi kịch bản khác đứng ở mặc định của nguồn *(trận official, bốn ghế, mode sân khấu)*.
- **Định dạng trình bày của mỗi bề mặt là quyết định thiết kế**, không phải yêu cầu chuẩn tắc. `docs/` khai **nội dung** *(năm trường, bốn trục lọc, UTC+7)* và **ràng buộc** *(chỉ đọc, một khung nhìn)*; spec này không khai bố cục, kiểu bảng, hay hình thức phân trang cụ thể.
- **Thuật ngữ chuẩn tắc của bảng này là `TERM-063` — Nhật ký thao tác · AuditLog.** Spec này dùng cụm *"nhật ký thao tác"* nhất quán và **không bao giờ** gọi nó là *"event log"* hay *"nhật ký sự kiện"*, vì hai cụm sau đã thuộc `TERM-021`.
