# Thuật ngữ

> **Mục đích**: mỗi khái niệm **một tên chuẩn, một nghĩa**. Tài liệu khác dùng đúng tên ở đây.
>
> **Nguyên tắc**: không tự sáng tạo định nghĩa. Từ nào miền này **không có** thì **không có mục** — đừng đi tìm.

## Cách đọc

| Trường | Nghĩa |
|---|---|
| **Định nghĩa** | Nghĩa **duy nhất** được dùng trong repo này |
| **Tên khác** | Tên đồng nghĩa, gồm cả tên tiếng Anh dùng khi đặt định danh trong code |
| **Đừng nhầm với** | Khái niệm lân cận dễ lẫn. Đây là trường **hay cứu bug nhất** — phần lớn là những chỗ một từ từng mang nhiều nghĩa |
| **Nguồn** | Luật gốc, hoặc `QĐ-*` trong `decisions.md` |

**Mọi mục ở đây đều đã chốt**, và cả dự án cũng không còn mục treo nào — thứ duy nhất ở `decisions.md` §N là một con số phi chức năng **đã hoãn có chủ đích**, không phải một thuật ngữ.

**Nhãn phạm vi**: `[v1.5]` · `[v2]` — khái niệm đã có tên và có chỗ trong schema, nhưng luật chưa ship.

---

# A. Con người và vai trò

### TERM-001 — Thí sinh

**Định nghĩa.** Người dự thi, ngồi một ghế, tự trả lời và tự phát tín hiệu. Phía thí sinh **không có dialog xác nhận, không rút lại được** — *"thí sinh cần tốc độ, và tự chịu trách nhiệm sai lầm của mình"*.

- **Tên khác**: player · contestant · TS
- **Giá trị**: luật v1 đặc tả cho **đúng 4**; lưu trữ và UI hỗ trợ **1-12**
- **Đừng nhầm với**: **Ghế** (chỗ ngồi, tồn tại cả khi trống) · **Đơn vị điểm** (ở v2 một đội là một đơn vị điểm nhưng nhiều thí sinh)
- **Nguồn**: `QĐ-005`, `QĐ-007`

### TERM-002 — Ghế · Vị trí

**Định nghĩa.** **Ghế** = chỗ dự thi trong một trận. **Vị trí** = số thứ tự `1..N` gán cho ghế, **gán thủ công trước trận** (thực tế chương trình: ban tổ chức bốc thăm thứ tự xuất phát).

- **Tên khác**: seat · seat position · *"vị trí đứng"* (nguyên văn luật gốc)
- **Giá trị**: 1-12; vị trí là số nguyên dương liên tục
- **Đừng nhầm với**: **Thứ hạng** — xếp theo điểm và đổi liên tục trong trận. Luật gốc dùng cụm *"vị trí đứng thấp nhất"*; ở repo này **vị trí luôn là số ghế**, không bao giờ là thứ hạng. Hai chỗ dùng nó — thứ tự chọn hàng ngang ở VCNV và phá hoà thứ tự lượt Về đích — đều là số ghế
- **Nguồn**: luật gốc §Về đích · `QĐ-002`

### TERM-003 — Đơn vị điểm

**Định nghĩa.** Chủ thể được cộng/trừ điểm. v1: **1 đơn vị điểm = 1 thí sinh**. v2 thi đội: **1 đơn vị điểm = 1 đội** — buzz vẫn theo cá nhân, điểm về đội.

- **Tên khác**: `scoringUnit`
- **Giá trị**: `individual` · `team`
- **Đừng nhầm với**: **Thí sinh** — hai khái niệm **trùng nhau ở v1 và tách nhau ở v2**. Mọi mảng RuleConfig (thang điểm Tăng tốc) đánh theo **đơn vị điểm**, không theo người
- **Nguồn**: `QĐ-007`

### TERM-004 — Admin

**Định nghĩa.** **Cảm biến và cơ cấu chấp hành DUY NHẤT của hệ thống.** Máy không quan sát được sân khấu, nên mọi mốc thời gian mà luật gốc mô tả bằng hành vi của MC đều ánh xạ thành **một cú bấm của admin**. Admin bấm Đúng/Sai, start timer, mở đáp án và ô chữ, chọn vòng và lượt, duyệt tín hiệu.

- **Tên khác**: người vận hành · operator
- **Giá trị**: **đúng MỘT admin cho mỗi contest** — không tồn tại hai luồng thao tác đồng thời
- **Đừng nhầm với**: **MC** — MC phán quyết bằng **lời**, admin thi hành bằng **bấm**; màn `/mc` là read-only
- **Nguồn**: `QĐ-001`, `QĐ-008`, `QĐ-027`

### TERM-005 — MC

**Định nghĩa.** Người dẫn chương trình — **thẩm quyền phán quyết trên sân khấu**, thực hiện bằng **nói**. Thao tác hệ thống **đúng một thứ**: duyệt cú **giành quyền điều khiển** khi phiên admin đang giữ mất kết nối (`QĐ-093`). Ngoài đó, màn `/mc` là read-only.

- **Quyền ghi duy nhất**: `PERM-054` duyệt / từ chối `EVENT-050`. MUST NOT duyệt tín hiệu của **thí sinh** — hàng đợi VCNV vẫn thuộc admin
- **Đọc đáp án qua `PERM-045`**, không qua kho đề: vai MC gán ở phạm vi `CONTEST` nên không giữ được permission của kho đề
- **Tên khác**: người dẫn chương trình (nguyên văn luật gốc) · **host**
- **Đừng nhầm với**: từ **`host`** trong luật gốc nghĩa là **MC**. Nó từng được dùng cho **admin** ở đúng một tên cấu hình, nay đã bãi bỏ — nghĩa đó **không còn chỗ bám**, đừng dựng lại
- **Nguồn**: luật gốc §Khởi động, §Câu hỏi phụ · `QĐ-001`

### TERM-006 — Setter

**Định nghĩa.** Người soạn câu hỏi và bộ đề trong kho đề.

- **Tên khác**: người soạn đề
- **Đừng nhầm với**: **Admin** — setter không điều khiển trận; đề của setter phải qua duyệt `DRAFT` → `ACTIVE`

### TERM-007 — Viewer

**Định nghĩa.** Người xem, truy cập **public bằng đúng một URL** *(mã phòng 6 số nằm trong URL)* — không account, không vai, không bước nhập mã, không duyệt. **Read-only tuyệt đối**: kênh này là **một chiều server → client, không có đường ghi** — read-only là tính chất **cấu trúc**, không phải một luật server phải cưỡng chế.

- **Tên khác**: khán giả · người xem
- **Đừng nhầm với**: **Overlay** — cùng mô hình truy cập nhưng là frame stream cho OBS, không phải màn người xem. Overlay **nhận đáp án cùng lúc và cùng điều kiện với viewer**, từ mốc **câu khép** theo cờ reveal (`QĐ-080`), **và cả đáp án Chướng ngại vật** khi nó lộ theo `GR-012` (`QĐ-118`) — đứng ngoài `GR-037` chỉ đổi **thời điểm**, không đổi **người nhận**
- **Âm thanh**: viewer **không bao giờ là nguồn phát**; nguồn phát của nhóm khe hướng khán giả là **admin hoặc overlay**, mặc định **overlay** (`QĐ-120`)
- **Nguồn**: `QĐ-015`, `QĐ-051`, `QĐ-080`, `QĐ-088`

### TERM-008 — User

**Định nghĩa.** **Tài khoản xác thực.** Các vai cần auth: thí sinh · MC · admin · setter (v1.5 thêm trainer). Viewer và overlay là **public**, không có tài khoản.

**Mỗi tài khoản mang ĐÚNG MỘT vai** — *Quản trị*, *Người ra đề*, *MC*, hoặc *Thí sinh*.

- **Một vai, nhiều phạm vi**: mọi phép gán của một tài khoản mang **cùng một vai**, nhưng tài khoản gán được ở nhiều phạm vi — một MC dẫn được nhiều contest
- **Loại trừ là tính chất CẤU TRÚC**: tài khoản mang vai *Thí sinh* **không thể** mang vai *Quản trị* / *MC* / *Người ra đề*, vì hai vai không cùng tồn tại trên một tài khoản. Không còn cửa nào phải kiểm
- **Ràng buộc TÀI KHOẢN, không ràng buộc CON NGƯỜI**: một người dựng hai tài khoản thì hệ thống không biết
- **Đừng nhầm với**: **Thí sinh** (vai trò trong một trận) · **Đơn vị điểm** (chủ thể tính điểm)
- **Nguồn**: `QĐ-065`, `QĐ-094`

### TERM-061 — Permission

**Định nghĩa.** **Nguyên tử quyền**, và là **thứ duy nhất server kiểm** ở cửa phân quyền. Catalog: `permissions.md` `PERM-001`→`PERM-061`.

- **Đừng nhầm với**: **Vai** (`TERM-062`) — vai chỉ là cái túi chứa permission. Server hỏi *"có permission X không"*, **không bao giờ** hỏi *"mang vai nào"* (`QĐ-078`, `QĐ-094`)
- **Không suy lẫn nhau**: giữ một permission không kéo theo permission nào khác
- **Có permission là điều kiện CẦN, không phải ĐỦ** — còn bốn cổng ngoài RBAC: phiên giữ quyền · ràng buộc loại trừ · chủ contest · trạng thái game
- **Nguồn**: `QĐ-094`, `QĐ-078`

### TERM-062 — Vai

**Định nghĩa.** Một **tập permission có tên**. Bốn vai dựng sẵn: *Quản trị · Người ra đề · MC · Thí sinh*; đơn vị tự định nghĩa thêm **vai tuỳ biến**.

- **Mỗi tài khoản mang đúng MỘT vai** (`QĐ-065`) — ràng buộc đặt lên **cột *vai*** của phép gán, không lên số phép gán
- **Phép gán mang phạm vi**: `(tài khoản, vai, phạm vi)`, phạm vi ∈ {`HỆ THỐNG`, một contest}. *Vai hệ thống* và *vai vận hành* là **hai phạm vi gán**, không phải hai loại vai
- **Đừng nhầm với**: **Permission** (`TERM-061`) — vai không phải thứ được kiểm · **Chủ contest** (`TERM-059`) — không phải một vai
- **Nguồn**: `QĐ-094`, `QĐ-065`, `QĐ-086`

### TERM-059 — Chủ contest

**Định nghĩa.** **Tài khoản đã tạo contest.** Thuộc tính **cố định** của contest, ghi một lần lúc tạo.

- **Tác dụng duy nhất ở v1**: **ưu tiên** khi nhiều admin cùng **giành quyền điều khiển** (`TERM-060`) trong một cửa sổ
- **Đừng nhầm với**: **vai vận hành** (MC · trainer · host) — chủ contest **không** phải một vai, admin **không gán lại được** · **permission** (`QĐ-078`) — nó cũng không phải một mục trong catalog quyền · **`host`** — chữ đó nghĩa là **MC** (`TERM-005`)
- **Nguồn**: `QĐ-093`

### TERM-060 — Giành quyền điều khiển

**Định nghĩa.** Một phiên admin lấy quyền điều khiển **mà phiên đang giữ không đồng ý**. Chỉ mở khi phiên đang giữ **mất kết nối**.

- **Đừng nhầm với**: **chuyển quyền điều khiển** (`QĐ-008`) — thao tác **hợp tác**, người **đang giữ** là người bấm, làm được mọi lúc trận chưa đóng sổ. Hai đường khác nhau về ai bấm và về điều kiện mở
- **Có MC** ⇒ MC duyệt (`TERM-005`). **Không MC** ⇒ có hiệu lực ngay và **âm thầm**
- **Nguồn**: `QĐ-093`

### TERM-009 — Đội `[v2]`

**Định nghĩa.** Nhóm thí sinh chia sẻ **một đơn vị điểm**; buzz theo cá nhân, điểm về đội.

- **Tên khác**: team
- **Đừng nhầm với**: **Ghế** — ở v2 nhiều ghế thuộc một đội

---

# B. Đơn vị tổ chức thi

### TERM-010 — Contest

**Định nghĩa.** Đơn vị tổ chức **bao trùm** — **bản thiết kế**, không phải một lần chạy. Gắn với một kho đề đã gán, và là **phạm vi của quy tắc no-repeat**: câu đã hỏi trong bất kỳ trận nào của contest thì không xuất hiện lại.

- **Thuộc contest**: mã phòng 6 số · kho đề · cấu hình gốc · mode trả lời · **phạm vi** no-repeat
- **Đừng nhầm với**: *"**cờ** no-repeat"* — **không tồn tại**. No-repeat là một **phạm vi**, không cấu hình được, không tắt được (`QĐ-044`). Gọi nó là *cờ* gợi ý một công tắc, và một công tắc như thế sẽ vô hiệu hoá cả hàng rào `everPublic`
- **Đừng nhầm với**: **Match** — một contest chứa **nhiều** trận, nhưng chỉ **một trận đang chạy** tại một thời điểm
- **Nguồn**: `QĐ-017`, `QĐ-039`, `QĐ-044`

### TERM-011 — Match · Trận

**Định nghĩa.** **Một lần chạy** của contest: chạy theo playlist các vòng, kết thúc bằng một kết quả đã đóng sổ.

- **Tên khác**: trận · trận đấu
- **Thuộc trận**: điểm · event log · ghế đã gán · biên bản · cấu hình **đã đóng băng** · `matchPurpose` · `revealAnswerAfterJudge`
- **Đừng nhầm với**: **Contest** (TERM-010). Và **đừng dùng từ *"game"*** — nó từng mang cả nghĩa *"trận"* lẫn nghĩa *"sản phẩm / thể loại gameshow"*. Trong đặc tả dùng **trận** hoặc **match**; *"game"* chỉ còn hợp lệ khi nói về sản phẩm (*"game engine"*, *"luật chơi"*)
- **Nguồn**: `QĐ-039`

### TERM-012 — Vòng · Round

**Định nghĩa.** Một phần thi của trận, có luật riêng. Năm vòng của O26: **Khởi động · Vượt chướng ngại vật · Tăng tốc · Về đích · Câu hỏi phụ**. Admin chọn vòng nào bắt đầu; được **bỏ hẳn**, **chạy lại**, hoặc **kết thúc khẩn cấp** một vòng.

- **Tên khác**: round · phần thi (nguyên văn luật gốc)
- **Giá trị**: `KHOI_DONG` · `VCNV` · `TANG_TOC` · `VE_DICH` · `TIE_BREAK`
- **Đừng nhầm với**: **Lượt** (TERM-013) — *"lượt riêng"* và *"lượt chung"* là hai phần **bên trong** vòng Khởi động, không phải hai vòng. Và **đừng dùng từ *"phase"* cho bước trong một vòng** — trong repo này `Phase 1-12` chỉ có nghĩa **mốc phát hành sản phẩm**; bước bên trong một câu gọi là **mốc**
- **Nguồn**: luật gốc (5 heading vòng) · `QĐ-002`, `QĐ-034`

### TERM-013 — Lượt

**Định nghĩa.** Từ này ứng với **bốn khái niệm khác nhau**; mỗi cái có một tên đầy đủ riêng, và **luôn dùng tên đầy đủ** khi có thể lẫn:

| Tên đầy đủ | Nghĩa |
|---|---|
| **Phân đoạn Khởi động** | *"lượt riêng"* (mỗi thí sinh 6 câu) và *"lượt chung"* (12 câu bấm chuông) |
| **Lượt riêng của một thí sinh** | Một thí sinh lần lượt trả lời 6 câu của mình |
| **Lượt chọn** | VCNV — *"mỗi thí sinh có tối đa 1 lượt lựa chọn"* hàng ngang |
| **Lượt thi** | Về đích — mỗi thí sinh một gói 3 câu; thứ tự tính lại sau mỗi lượt |

- **Tên khác**: turn
- **Đừng nhầm với**: ràng buộc *"tối đa 1 lượt"* của **lượt chọn** và *"thứ tự lượt"* của **lượt thi** là hai luật khác nhau, **không suy ra được nhau**
- **Nguồn**: luật gốc §Khởi động, §VCNV, §Về đích

### TERM-014 — Playlist

**Định nghĩa.** Danh sách vòng theo thứ tự của một trận. Là **gợi ý** — admin chọn vòng nào mở, playlist không cưỡng chế.

- **Đừng nhầm với**: **Bộ đề** — playlist xếp vòng, bộ đề gom câu hỏi
- **Nguồn**: `QĐ-002`

### TERM-015 — `contestPurpose` · `matchPurpose`

**Định nghĩa.** Hai cờ ở **hai tầng**, không gộp được:

| Tầng | Cờ | Quyết định |
|---|---|---|
| **Contest** | `contestPurpose: official \| practice` | Phân biệt contest thật với practice contest |
| **Match** | `matchPurpose: official \| practice` | Phép lọc kho đề · `revealAnswerAfterJudge` mặc định · retention |

Trong một **contest thật**, trận `practice` **chỉ được gán câu đã hiển thị** ở các trận thật trước đó — nên nó không thể nhìn thấy đề chưa thi.

- **Đừng nhầm với**: **`revealAnswerAfterJudge`** là cờ **riêng**, nay mặc định **BẬT cho cả hai mục đích trận** (`QĐ-080`) và vẫn đổi được từng trận — nó **không còn** phân biệt official với practice
- **Nguồn**: `QĐ-040`, `QĐ-051`, `QĐ-080`

### TERM-016 — Mode trả lời

**Định nghĩa.** Cách thí sinh đưa đáp án. **Sân khấu** (mặc định, và là mode **luật được đặc tả theo**): thí sinh **đọc** đáp án, máy chỉ dùng để giành quyền. **Nhập liệu**: thí sinh **gõ** đáp án. Đặt ở **cấp contest**, một giá trị chung cho toàn bộ vòng.

- **Tên khác**: mode sân khấu / mode nhập liệu · stage mode / input mode
- **Đừng nhầm với**: **VCNV và Tăng tốc LUÔN gõ máy bất kể mode.** Ở VCNV mode chỉ đổi **ai chọn hàng ngang**; đáp án Chướng ngại vật thì đi theo mode. Hệ quả ngược trực giác: mode sân khấu **không** làm nhẹ yêu cầu phần cứng
- **Nguồn**: `QĐ-016`, `QĐ-017`, `QĐ-018`, `QĐ-019`

---

# C. Trạng thái và sự kiện

### TERM-017 — Trạng thái trận

**Định nghĩa.** Enum **bốn** giá trị: **`LOBBY` · vòng đang chạy · `TIE_BREAK` · `FINISHED`**.

- **Đừng nhầm với**: **lớp phủ** (công bố kết quả, banner tạm dừng, dialog) **không** thuộc enum này — chúng chồng lên trạng thái đang chạy mà không huỷ nó
- **Nguồn**: `QĐ-032` · `game-state-machine.md` §A

### TERM-018 — State

**Định nghĩa.** Từ *"state"* trong repo này **luôn phải kèm thang bậc**. Có **bảy** thang bậc, tách hẳn nhau và có **vòng đời khác nhau**: cấp **trận** · cấp **giai đoạn** · cấp **câu** · cấp **ghế** · cấp **tín hiệu** · cấp **lớp phủ** · cấp **ô chữ**.

- **Đừng nhầm với**: chỉ cấp **trận**, **giai đoạn** và **câu** là loại trừ lẫn nhau. Cấp **ghế**, **tín hiệu** và **lớp phủ** là **cờ song song** — nhiều cái cùng đúng một lúc. Viết *"state"* trần trụi trong spec là lỗi
- **Nguồn**: `game-state-machine.md` §Quy ước đọc

### TERM-019 — Cửa vào vòng (`LOBBY`)

**Định nghĩa.** **Trạng thái nghỉ của trận** — không vòng nào đang chạy. Dùng **cả trước vòng đầu tiên lẫn giữa hai vòng**. Là **cửa vào của mọi vòng** và là **cửa ra duy nhất** của mọi vòng.

- **Tên khác**: `LOBBY` · nghỉ giữa vòng
- **Đừng nhầm với**: **nội dung trình diễn giữa hai vòng** (giao lưu, giải lao, video hình hiệu) là **lớp phủ** admin bật/tắt, **không** phải một trạng thái · đây cũng **không** phải cơ chế xử lý sự cố — van thoát khi sự cố rơi vào giữa một cửa sổ thời gian là **kết thúc khẩn cấp / bỏ / chạy lại vòng**
- **Nguồn**: `QĐ-032`, `QĐ-034`, `QĐ-036`

### TERM-020 — `TIE_BREAK` · Câu hỏi phụ

**Định nghĩa.** Vòng phân định thí sinh hoà điểm. **3 câu × 15 giây**, giành quyền bằng chuông, **không cộng điểm** — chỉ đổi thứ hạng. Vào khi admin bấm **chốt trận** và server tính ra có hoà ở vị trí thuộc `tieBreakPositions`.

- **Tên khác**: vòng phụ · tie-break
- **Giá trị**: `tieBreakPositions` **v1 khoá cứng `[1]`** — chỉ vị trí NHẤT; cửa tạo contest không cho chọn khác, cấu hình vẫn nhận mảng nhiều vị trí nhưng không có đường xử lý (`QĐ-085`)
- **Nguồn đề**: **không có kho riêng.** Rút từ ba kho — **Về đích → Khởi động → VCNV** theo thứ tự ưu tiên đó; kho **Tăng tốc không phải nguồn**; câu `isPractical` bị loại. Câu mượn bị bỏ qua `timeSeconds` và `value`. Admin **chỉ định trước** một số câu **còn available** được, **tại `LOBBY`**, hạn chót là mốc bấm Chốt trận — chỉ định là **đặt chỗ**, thiếu thì bù theo thứ tự ưu tiên (`QĐ-081`)
- **Đừng nhầm với**: **câu hỏi phụ ≠ câu hỏi dự phòng** (câu thay thế khi media hỏng) · **"không có kho riêng" ≠ "không cần kiểm kho"** — cửa vào vòng vẫn kiểm đủ 3 câu khả dụng, chỉ là phép kiểm đọc **ba kho nguồn** thay vì một kho khai riêng
- **Nguồn**: luật gốc §Câu hỏi phụ · `QĐ-036`, `QĐ-055`, `QĐ-081`, `QĐ-085`

### TERM-021 — Event · MatchEvent

**Định nghĩa.** Bản ghi **append-only** của mọi việc xảy ra trong trận. **Điểm là hàm của event log**, không phải một con số bị sửa trực tiếp. Lịch sử **linear, không bao giờ xoá**.

- **Tên khác**: sự kiện trận · event log
- **Giá trị**: các loại đã đặt tên — `QUESTIONS_DRAWN` · `QUESTION_USED` · `SCORE_ADJUST` · `MEDIA_KEY` · `TIE_BREAK_RESOLVED`
- **Không phải mọi event đều là event điểm.** `TIE_BREAK_RESOLVED` là **event thứ hạng**: nó nằm trong cùng nhật ký nhưng **không tham gia** `reduce` ra bảng điểm (`QĐ-083`)
- **Đừng nhầm với**: **Nhật ký thao tác** (`TERM-063`) — bảng chung ghi mọi thao tác của mọi vai, khác event trận ở **chủ**, **vòng đời** và **người đọc**; nó **tham chiếu** tới nhật ký này chứ không nhân bản (`QĐ-132`) · **thao tác của người dùng** — thao tác là cái được bấm và **có thể bị từ chối**; event là cái **đã xảy ra và được ghi**
- **Nguồn**: `QĐ-011`, `QĐ-083`, `QĐ-132`

### TERM-063 — Nhật ký thao tác · AuditLog

**Định nghĩa.** Bảng **chỉ thêm**, phạm vi **bản cài**, ghi dấu vết **mọi thao tác của mọi vai** — đăng nhập thành công và thất bại, đăng xuất, thao tác kho đề và bộ đề, quản lý contest, nhập và xuất, khán giả **bị đuổi**, và **mỗi lần xem đáp án**. Mỗi dòng mang **năm trường**: người thực hiện · hành động · đối tượng · thời điểm *(server time)* · địa chỉ nguồn.

- **Tên khác**: audit log · nhật ký kiểm toán
- **Mang thêm trường *kết quả*** *(thành công · bị từ chối · thất bại)* ở các thao tác mà `NFR-14` đòi, và ở **mọi** yêu cầu xem đáp án (`QĐ-133`)
- **Không tham gia phép tính nào.** Nó **không** là nguồn để tính điểm, khác hẳn `TERM-021`
- **Với sự kiện trong trận, nó THAM CHIẾU chứ không nhân bản** (`QĐ-132`) — nội dung sự kiện có đúng một nguồn sự thật
- **Có vòng đời RIÊNG**: hạn lưu trữ của nó là **một** giá trị, mặc định **24 tháng**, **không** theo `matchPurpose` (`QĐ-131`, `QĐ-136`). Dòng thuộc một trận **không** bị dọn theo hạn của trận đó. Quan hệ giữa hai trục **không bị cưỡng chế** — đặt thấp hơn hạn trận thì hệ thống cảnh báo, không chặn
- **Tham chiếu treo có nhãn riêng**: khi nguồn mà một dòng trỏ tới đã bị dọn, dòng vẫn hiện đủ năm trường nhưng mang nhãn ***"nguồn đã hết hạn"*** và đường đi tiếp bị vô hiệu hoá; `NFR-17` khi đó **không còn thoả** cho mốc ấy (`QĐ-137`). Ở cấu hình mặc định đây là **trạng thái thường trực** cho mọi trận quá 12 tháng
- ***"Chỉ thêm"* nói về BỀ MẶT SẢN PHẨM.** Không đường sửa, không đường xoá một dòng nào cho bất kỳ vai nào; bước dọn theo hạn riêng là đường **duy nhất** một dòng rời hệ thống, và nó không đi qua bề mặt nào (`QĐ-131`)
- **Đọc bằng hai cửa tách bạch** (`QĐ-135`): `PERM-015` `audit.read` ở phạm vi **bản cài**, và `PERM-062` `audit.readContest` ở phạm vi **`CONTEST`** — phạm vi sau tính **theo ĐỐI TƯỢNG của dòng**, không theo người thực hiện (`QĐ-138`). Vai dựng sẵn **Quản trị** giữ **cả hai** (`QĐ-142`). Một lần tra chịu **biên cứng về độ dài khoảng thời gian**, mặc định **24 tháng**, một giá trị cấp bản cài dùng chung cho cả hai cửa (`QĐ-139`, `QĐ-141`)
- **Chỉ ghi đường KÉO, không ghi đường ĐẨY.** Một **yêu cầu** xem đáp án của một phiên để lại một dòng ở bảng này; cú **đẩy đáp án hàng loạt** của engine tại mốc câu khép thì **không** — nó là một **sự kiện trong trận**, tra qua tham chiếu (`QĐ-145`)
- **Đừng nhầm với**: **Event · MatchEvent** (`TERM-021`) — thứ đó là nhật ký **của một trận** và là **nguồn để tính điểm**; hai bảng khác chủ, khác vòng đời, khác người đọc
- **Nguồn**: `QĐ-130` → `QĐ-145` · `PRD-REQ-082`, `PRD-REQ-116`, `NFR-14`, `NFR-17`

### TERM-022 — Event điểm

**Định nghĩa.** Đơn vị event sinh ra điểm. Có **hai hình dạng**, dùng ở hai chỗ khác nhau — **không phải hai định nghĩa tranh nhau**:

| | Hình dạng | Dùng ở |
|---|---|---|
| **E1** | Một event cho **một** người; chống trùng theo `(câu, thí sinh, loại phán quyết)` | Mọi vòng **trừ** Tăng tốc |
| **E2** | Một event cho **cả bảng** | **Tăng tốc** — điểm ở đó là hàm của **thứ hạng**, một quan hệ giữa những người được chấm Đúng |

- **Đừng nhầm với**: E1 có chiều **thí sinh**, E2 **không có** ⇒ **không tồn tại** khoá chống trùng dùng chung. Điều đó **không** gây vấn đề, vì **không có thao tác sửa từng phần một event E2**: câu đã chốt thì bảng điểm của câu là chung cuộc
- **Nguồn**: `QĐ-013`, `QĐ-014`

### TERM-023 — Hoàn nguyên

**Định nghĩa.** Đảo điểm bằng cách **thêm event đảo ngược** — như `git revert`, **không phải** `git reset --hard`. Event cũ không bị xoá.

- **Tên khác**: revert
- **Đừng nhầm với**: **`SCORE_ADJUST`** (TERM-024) — hoàn nguyên là đảo máy móc một event đã có; `SCORE_ADJUST` là **phán quyết mới của người** và **không tự đảo** theo vòng · **undo** — hoàn nguyên **không** giới hạn ở event gần nhất, và không có thao tác undo nào trong hệ thống
- **Nguồn**: `QĐ-011`, `QĐ-035`

### TERM-024 — `SCORE_ADJUST`

**Định nghĩa.** Event điều chỉnh điểm thủ công `{ghế, delta, lý do}` — **lý do bắt buộc**, vào audit log. Là **phán quyết của người** nên **không tự đảo** khi vòng bị bỏ, và **không thuộc vòng nào**.

- **Tên khác**: chỉnh điểm tay · điều chỉnh thủ công
- **Giá trị**: `delta` số nguyên âm, dương, **hoặc 0** — `delta = 0` vẫn sinh event: đó là một ghi chú chính thức vào biên bản
- **Đừng nhầm với**: đây là **van thoát duy nhất cho mọi sai lầm trong trận** — chấm nhầm ở Tăng tốc, ghế bị vô hiệu hoá giữa câu, mặc định SAI oan. Dùng được tới **mốc chốt trận**, sau đó trận niêm phong
- **Nguồn**: `QĐ-014`, `QĐ-035`, `QĐ-037`

---

# D. Tín hiệu và thao tác

### TERM-025 — Chuông

**Định nghĩa.** Tín hiệu thí sinh phát để **giành quyền trả lời**. **Chỉ nhận click chuột — không gán hotkey**, tránh bấm nhầm khi đang gõ đáp án. Nút **tự khoá ngay khi bấm**, ở frontend, trước khi gửi.

- **Tên khác**: buzz · buzzer
- **Giá trị**: bốn vòng có chuông, **bốn mốc mở cửa sổ khác nhau** — Khởi động lượt chung (từ mốc hiển thị câu) · Câu hỏi phụ (từ mốc start timer) · VCNV (bất cứ lúc nào trong vòng) · Về đích (5 giây từ mốc chấm Sai)
- **Đừng nhầm với**: **nút *"Mở chướng ngại vật"* ĐƯỢC XẾP LÀ CHUÔNG** ⇒ cũng chỉ nhận click, cũng tự khoá · **nút gửi đáp án** không phải chuông, có hotkey Enter và **không** khoá sau khi gửi
- **Nguồn**: `QĐ-023`, `QĐ-054`

### TERM-026 — Hàng đợi tín hiệu

**Định nghĩa.** Mọi tín hiệu của thí sinh vào hàng đợi **theo server timestamp**. **Không có cơ chế drop.** Hàng đợi **đang hoạt động** reset sau mỗi **vòng**; **lịch sử tín hiệu không bao giờ xoá**.

- **Giá trị**: **chặn** ở VCNV — admin duyệt Yes mới có hiệu lực; **không chặn** ở Khởi động lượt chung, cướp quyền Về đích, Câu hỏi phụ
- **Đừng nhầm với**: ***"không chặn" ≠ "không ghi nhận"*** — hàng đợi vẫn ghi thứ tự để admin can thiệp khi có sự cố · **dialog xác nhận của thí sinh** (chỉ có ở chọn hàng ngang mode nhập liệu) nằm **TRƯỚC** hàng đợi và **không thay thế** bước admin duyệt
- **Nguồn**: `QĐ-020`, `QĐ-021`

### TERM-027 — Từ chối tín hiệu

**Định nghĩa.** Admin bấm **No** cho một tín hiệu trong hàng đợi ⇒ tín hiệu kế tiếp lên. **Từ chối KHÔNG làm thí sinh mất lượt**, và **không tác dụng phụ nào đã phát sinh**: chưa đánh dấu đã hỏi, câu chưa tiêu, đồng hồ chưa chạy.

- **Tên khác**: bấm No · reject
- **Đừng nhầm với**: **Bị loại** (TERM-030) — từ chối là từ chối một tín hiệu, không đụng tư cách dự thi. Đây là **chỗ sửa lỗi bấm nhầm của thí sinh**, nên nó cố ý **không** phải hình phạt
- **Nguồn**: `QĐ-022`

### TERM-028 — Mốc admin

**Định nghĩa.** Thời điểm do admin bấm, thay cho một mốc mà luật gốc mô tả bằng hành vi của MC. Mốc admin là **TUYỆT ĐỐI**: không cửa sổ ân hạn, không trừ bù độ trễ tay người.

| Mốc trong luật gốc | Cú bấm |
|---|---|
| *"MC đọc xong câu hỏi"* | start timer |
| *"hiệu lệnh của người dẫn chương trình"* | start timer (Câu hỏi phụ) |
| *"câu hỏi được đọc lên hoặc hiện lên màn hình"* | hiển thị câu hỏi |
| *"MC công bố đáp án"* | công bố đáp án |

- **Đừng nhầm với**: ***"hiển thị câu hỏi"* và *"start timer"* là HAI thao tác**, thứ tự cố định. Khoảng giữa hai mốc chính là lúc MC đọc — gộp lại là phá luật
- **Nguồn**: `QĐ-006`, `QĐ-027`, `QĐ-028`

### TERM-029 — Phán quyết của admin

**Định nghĩa.** Hành vi **admin bấm** để chốt kết quả một câu. **Máy không tự chấm**: hệ thống chỉ (1) hiển thị bài làm cạnh đáp án đúng, (2) **highlight ký tự khác** như gợi ý.

- **Giá trị**: `Đúng` · `Sai` · `Huỷ kết quả`. **Không phải lúc nào cũng nhị phân** — chỉ hai lựa chọn khi *Sai* trừ **0 điểm**; ở vòng mà *Sai* có hình phạt, và ở câu chỉ có bản gửi quá hạn, có thêm `Huỷ kết quả`
- **Đừng nhầm với**: **kết quả so khớp** là **đầu vào của gợi ý**, không bao giờ là phán quyết. Không có `autoJudge` trong trận chính thức
- **Nguồn**: `QĐ-010`, `QĐ-061`

### TERM-030 — Bị loại

**Định nghĩa.** Trạng thái của một thí sinh **trong vòng VCNV** sau khi trả lời **sai Chướng ngại vật**. Chỉ mất quyền trong **vòng đó**, không rời trận.

- **Tên khác**: eliminated
- **Đừng nhầm với**: **không dịch sang *"loser"*** — đây là nhầm lẫn dễ đưa vào code nhất khi đặt tên cờ. Người bị loại khỏi VCNV **vẫn thi tiếp** Tăng tốc và Về đích, vẫn có thể thắng trận, và **giữ nguyên điểm hàng ngang đã kiếm**. Miền này **không có khái niệm *"người thua"***
- **Nguồn**: luật gốc §Vượt chướng ngại vật · `QĐ-057`

### TERM-031 — Bỏ vòng · Chạy lại vòng · Kết thúc khẩn cấp · Huỷ kết quả

**Định nghĩa.** Bốn thao tác khác nhau, **không thay thế cho nhau**. Từ *"cancelled"* / *"huỷ"* trần trụi **không được dùng** — luôn gọi tên đầy đủ:

| Tên | Điểm | Ghi chú |
|---|---|---|
| **Bỏ vòng** | **revert** | Biên bản nhãn *"đã bỏ"*; câu đã dùng **không** trả lại kho |
| **Chạy lại vòng** | **revert** rồi chạy mới | Cần kho đề còn đủ câu |
| **Kết thúc vòng khẩn cấp** | **GIỮ NGUYÊN** | Bấm được cả khi chưa đủ câu; biên bản nhãn *"kết thúc sớm"* |
| **Huỷ kết quả** | không sinh điểm cho ai | Là một **hạng phán quyết**, phạm vi **một câu** |

- **Đừng nhầm với**: ***"reset" không có nghĩa xoá*** — reset điểm = hoàn nguyên (TERM-023) · **Từ chối tín hiệu** (TERM-027) không đụng vòng lẫn điểm · bỏ vòng hoàn được **điểm** nhưng không hoàn được **đề đã lộ**
- **Nguồn**: `QĐ-034`, `QĐ-035`, `QĐ-061`

> **Không tồn tại thao tác *"huỷ cửa sổ cướp quyền"*** `[SUY RA]`. Chuyển câu thủ công chỉ hợp lệ khi câu đang ở giai đoạn hiển thị / đếm giờ / chờ chấm — **không** hợp lệ khi cửa sổ cướp đang mở. Cửa sổ cướp thuộc về câu và khép theo câu.

### TERM-032 — Vô hiệu hoá ghế

**Định nghĩa.** Admin tắt quyền thao tác của một ghế. **Đảo ngược được**, dùng **mọi lúc** trong một trận chưa đóng sổ. Ghế **vẫn ở trong trận**: giữ nguyên điểm, giữ nguyên vị trí, vẫn trên mọi bảng — chỉ mất quyền thao tác và không được đề xuất lượt.

- **Tên khác**: disable / enable ghế
- **Đừng nhầm với**: **kick** — v1 **không có kick**; nhu cầu *"rời hẳn một ghế khỏi trận"* hoãn sang phiên bản sau · **kick viewer** là kiểm duyệt khán giả công khai, việc hoàn toàn khác · **mất kết nối** — hai chuyện độc lập, quá grace **không** tự vô hiệu hoá ghế nào
- **Nguồn**: `QĐ-047`

---

# E. Điểm và kết quả

### TERM-033 — Điểm

**Định nghĩa.** Đại lượng tích luỹ quyết định thứ hạng, tính bằng `reduce(event log)`. **Được phép ÂM, không có sàn.** Điểm âm tham gia bình thường vào xếp lượt Về đích và điều kiện hoà.

- **Tên khác**: score
- **Giá trị**: số nguyên, âm hoặc dương, không chặn dưới
- **Đừng nhầm với**: **Giá trị câu** (TERM-034) — một câu có `value`, khác với điểm tích luỹ của người · **Thứ hạng** — dẫn xuất từ điểm
- **Nguồn**: `QĐ-011`, `QĐ-012`

### TERM-034 — Giá trị câu · Mức điểm

**Định nghĩa.** Số điểm gắn với một câu hỏi (`value`). Ở Về đích, thí sinh **chọn gói 3 câu từ hai mức {20, 30}**.

- **Giá trị**: Khởi động **+10** đúng, **−5** sai ở lượt chung · VCNV hàng ngang **10**, Chướng ngại vật **60/50/40/30** và **20** sau gợi ý cuối, ô trung tâm **10** · Tăng tốc **40/30/20/10** theo thứ hạng tốc độ · Về đích **{20, 30}**
- **Đừng nhầm với**: **thời gian KHÔNG suy ra từ mức điểm** — `timeSeconds` là metadata **từng câu** (TERM-045) · giá trị **lẻ** không phát sinh dưới luật 2026; nếu admin cấu hình mức lẻ thì `phạt = value / 2` bằng **phép chia số nguyên**, làm tròn xuống theo **độ lớn**
- **Phạm vi**: thang điểm cho số ghế **trên** 4 là `[v1.5]`; dưới 4 thì thuộc v1 qua **ghế bỏ thi** (`QĐ-105`)
- **Nguồn**: luật gốc · `QĐ-007`, `QĐ-058`, `QĐ-105`

### TERM-035 — Cướp quyền

**Định nghĩa.** Khi người thi chính ở Về đích trả lời **sai**, các thí sinh **khác** giành quyền bằng cách bấm chuông **trong 5 giây**. Cướp đúng ⇒ **transfer**: người sai **−value**, người cướp **+value**. Cướp sai ⇒ người cướp **−½ value**, người thi chính **không** được hoàn lại.

- **Tên khác**: steal · transfer
- **Đừng nhầm với**: **5 giây là cửa sổ bấm chuông**, không phải thời gian trả lời hay thời gian thực hành (20/40 giây) · **ghi nhận đáp án giống nhau**: người thi chính và người cướp **đều** tính **bản CUỐI CÙNG**, theo nguyên tắc hai trục (`QĐ-113`) — vế *"người cướp tính bản đầu tiên"* đã bị gỡ · `stealMode: 'add'` là option hợp lệ của hệ thống nhưng **không dùng** cho preset O26
- **Nguồn**: luật gốc §Về đích · `QĐ-058`

### TERM-036 — Ngôi sao hy vọng

**Định nghĩa.** Quyền đặt cược ở Về đích, **1 lần / thí sinh** trong một lần chạy vòng. Đúng ⇒ **gấp đôi** giá trị câu. Sai ⇒ **trừ giá trị câu**, *"kể cả các thí sinh còn lại có giành quyền trả lời hay không"*. Đặt **trước mốc admin bấm hiển thị câu hỏi**.

- **Tên khác**: NSHV
- **Đừng nhầm với**: hình phạt NSHV **THAY THẾ** phần nợ của transfer, **không cộng dồn** — câu 30đ ⇒ **A −30 · B +30**, không phải −60 · người **cướp** ăn **giá trị gốc**, không phải giá trị đã nhân đôi · **chủ thể bấm đi theo mode**: sân khấu ⇒ admin bấm, nhập liệu ⇒ thí sinh bấm
- **Phạm vi cờ**: một **lần chạy vòng**, không phải một trận — chạy lại vòng thì ngôi sao hồi sinh
- **Nguồn**: luật gốc §Về đích · `QĐ-019`, `QĐ-033`, `QĐ-058`

### TERM-037 — Thứ hạng tốc độ

**Định nghĩa.** Cơ chế tính điểm của **Tăng tốc**: điểm theo **thứ hạng nhanh trong số người được admin chấm ĐÚNG**, đo bằng **server-received timestamp của bản cuối**. Đồng thời gian ⇒ **cùng mức điểm**, bậc kế **nhảy qua** số người hoà.

- **Giá trị**: 40/30/20/10 cho 4 đơn vị điểm; độ phân giải đồng thời gian = **millisecond**
- **Đừng nhầm với**: thứ hạng chỉ tính trên **tập người được chấm ĐÚNG** — người sai **không giữ chỗ** trong thang · **bản nội dung y hệt bản trước không cập nhật mốc**: cập nhật mốc cho một bản không đổi nội dung cho phép thí sinh **tự làm xấu** thứ hạng của mình
- **Phạm vi**: thang cho số ghế **trên** 4 là `[v1.5]`; dưới 4 dùng chính thang này vì ghế bỏ thi **không giữ chỗ** *(chúng không bao giờ được chấm Đúng — `QĐ-105`)*
- **Nguồn**: luật gốc §Tăng tốc · `QĐ-059`, `QĐ-105`

### TERM-038 — Kết quả

**Định nghĩa.** Ba thứ khác nhau; **luôn gọi tên đầy đủ**:

| Tên đầy đủ | Nghĩa |
|---|---|
| **Phán quyết** | Kết quả chấm **một câu** — Đúng / Sai / Huỷ kết quả (TERM-029) |
| **Kết quả trận** | Bảng điểm cuối + thứ hạng, chốt tại mốc **chốt trận**; xuất biên bản/PDF |
| **Kết quả tie-break** | Người thắng Câu hỏi phụ — **không cộng điểm**, chỉ đổi thứ hạng. Ghi bằng event `TIE_BREAK_RESOLVED`; hoàn nguyên bằng **bỏ vòng `TIE_BREAK`** trước cú Chốt trận cuối, và **tự mất đối tượng** khi nhóm không còn bằng điểm (`QĐ-083`) |

- **Đừng nhầm với**: hai cái sau **không thể mâu thuẫn nhau**, nhưng **không phải vì không có đường sửa điểm** — tie-break xong thì trận về `LOBBY` và admin **vẫn sửa điểm được**. Chúng không mâu thuẫn được là vì kết quả tie-break **chỉ sắp thứ tự trong nhóm bằng điểm**: sửa điểm làm nhóm hết bằng nhau ⇒ kết quả tie-break **mất đối tượng** và phép phân định chạy lại (`GR-022` C8). Tình huống *"người thắng tie-break lại thua điểm"* vì thế **không tồn tại được**, chứ không phải bị chặn
- **Nguồn**: `QĐ-036`, `QĐ-037`, `QĐ-055`, `QĐ-083`

### TERM-039 — Người thắng

**Định nghĩa.** **Hạng nhất của bảng xếp hạng chốt tại mốc chốt trận.** Luật gốc chỉ dùng cụm *"thí sinh thắng cuộc"* trong ngữ cảnh Câu hỏi phụ; ở cấp trận, người thắng là hệ quả của thứ hạng chứ không phải một danh hiệu riêng.

- **Tên khác**: winner · hạng nhất
- **Đừng nhầm với**: trận đóng sổ với nhãn **`bỏ dở`** thì **không có người thắng** — không phân định thứ hạng
- **Nguồn**: luật gốc §Câu hỏi phụ · `QĐ-036`, `QĐ-038`

### TERM-040 — Hoà

**Định nghĩa.** **Bằng điểm.** Là điều kiện kích hoạt Câu hỏi phụ, thu hẹp còn **vị trí thuộc `tieBreakPositions`** — ở v1 là **chỉ vị trí NHẤT**, khoá cứng (`QĐ-085`). Hoà ở vị trí khác ghi **đồng hạng**, hạng kế nhảy qua.

- **Tên khác**: tie · đồng hạng
- **Đừng nhầm với**: **hoà ĐIỂM** (kích hoạt tie-break) và **hoà THỜI GIAN** ở Tăng tốc (cùng server timestamp ⇒ cùng mức điểm) là **hai thứ khác nhau** · hoà **ngoài** phạm vi phân định thì ghi **ĐỒNG HẠNG** vào biên bản, hạng kế **nhảy qua** số người đồng hạng · điểm âm tham gia điều kiện hoà bình thường
- **Nguồn**: luật gốc §Câu hỏi phụ, §Tăng tốc · `QĐ-049`, `QĐ-055`

### TERM-041 — Bốc thăm

**Định nghĩa.** Hết 3 câu tie-break vẫn chưa phân định ⇒ *"các thí sinh sẽ phải bốc thăm để chọn ra thí sinh thắng cuộc"*. Server random, **admin xác nhận**. **Bốc lại** là một thao tác riêng sinh event mới; cả hai lần bốc đều nằm trong event log, **lần cuối cùng có hiệu lực**.

- **Tên khác**: `exhaustedFallback: 'random-draw'`
- **Đừng nhầm với**: **Rút đề** (TERM-047) — tiếng Anh cả hai đều là *"draw"*, nhưng đây là **phân định người thắng**, kia là **chọn câu hỏi**. Đặt tên chung `draw*` cho cả hai sẽ che mất khác biệt. Tiếng Anh *"draw"* còn nghĩa **hoà** (TERM-040) — ba khái niệm, đừng dùng từ này trần trụi
- **Nguồn**: luật gốc §Câu hỏi phụ · `QĐ-055`

### TERM-042 — Hết giờ

**Định nghĩa.** Thời điểm server đóng cửa sổ nhận đáp án hoặc nhận tín hiệu. **Server time là source of truth duy nhất.** Máy **không tự sinh kết quả** khi hết giờ.

- **Tên khác**: timeout · deadline · `endsAt`
- **Giá trị**: Khởi động 3s · VCNV hàng ngang 15s · VCNV sau gợi ý cuối 15s · Tăng tốc 20/20/30/30s · Về đích 15s (câu 20đ) / 20s (câu 30đ) · cửa sổ cướp 5s · Câu hỏi phụ 15s
- **Đừng nhầm với**: **biên là biên ĐÓNG** — tới **đúng** mốc vẫn hợp lệ · **grace 120 giây** cũng là mốc thời gian nhưng **không phải** timeout của câu · hết giờ **khoá thí sinh, không khoá admin** · **Câu khép** (TERM-057) là mốc **khác hẳn**, đến sau
- **Nguồn**: luật gốc (mọi heading vòng) · `QĐ-029`, `QĐ-030`

### TERM-057 — Câu khép

**Định nghĩa.** Thời điểm **không còn ai được trả lời một câu nữa**. Đây là mốc **công bố đáp án** cho thí sinh, viewer và overlay (`QĐ-080`).

| Vòng · pha | Câu khép khi |
|---|---|
| Khởi động — lượt riêng · lượt chung | admin chấm xong; hoặc hết cửa sổ chuông không ai bấm ⇒ **câu bị bỏ qua** |
| VCNV — hàng ngang · ô trung tâm | admin chấm xong |
| Tăng tốc | admin chấm xong **cả bảng** |
| **Về đích** | cửa sổ cướp quyền **đã đóng** VÀ người cướp **đã được chấm**; hoặc hết 5 giây không ai bấm; hoặc người thi chính được chấm **Đúng** |

- **Tên khác**: mốc công bố
- **Đừng nhầm với**: ***"đã chấm"*** — hai mốc **trùng nhau ở mọi vòng TRỪ Về đích**, nơi cú bấm *chấm Sai* vừa là phán quyết vừa là cú **mở cửa sổ cướp quyền**. Lẫn hai mốc này là **xoá sổ cơ chế cướp quyền** · **Hết giờ** (TERM-042) — hết giờ chỉ khoá ô nhập của thí sinh, câu vẫn chưa khép vì chưa ai chấm · **Kết thúc vòng** — phạm vi vòng, không phải phạm vi câu
- **Ba ca đứng ngoài**: câu bị bỏ qua **vẫn công bố** · phán quyết **Huỷ kết quả** **không** tự công bố · đáp án **Chướng ngại vật** không theo cơ chế này, mà theo `GR-012`
- **Nguồn**: `QĐ-080` · `GR-037`, `GR-020` · `INV-017`

### TERM-043 — Grace kết nối

**Định nghĩa.** Cửa sổ **120 giây** giữ ghế cho thí sinh mất kết nối: giữ ghế + state-sync, banner *"đang kết nối lại"*. Quá grace, hệ thống **chỉ tô nổi bật** ghế trên màn admin — **không tự loại, không tự xoá**.

- **Đừng nhầm với**: **không có chính sách dropout tự động** — `dropoutPolicy` **không tồn tại** · grace là **khuyến nghị**, admin can thiệp sớm cũng được · mỗi lần mất kết nối mở cửa sổ **mới**, tính lại từ đầu, **không cộng dồn**, không giới hạn số lần · grace gắn với **GHẾ**, không gắn với một *"phiên"* — **không có entity `Session`** trong miền này; phiên xác thực là chuyện hạ tầng
- **Nguồn**: `QĐ-045`, `QĐ-046`

---

# F. Đề và kho đề

### TERM-044 — Câu hỏi

**Định nghĩa.** Đơn vị đề thi. Người tạo contest **phải chọn danh sách câu hỏi trước khi start**; hệ thống **không tự lấy đề** — rút chỉ random **trong danh sách đã gán**.

- **Giá trị**: `displayId` · `fieldId` · `wordCount` · `explanation` · `note` · `timeSeconds?` · `value?` · `clues[]?` · `everPublic` · `isPractical`
- **Nguồn**: luật gốc §Khởi động, §Tăng tốc, §Về đích · `QĐ-066`

**Một kiểu NHẬP đáp án.** Mọi câu hỏi có ô nhập dùng **một** ô nhập chữ duy nhất. **Câu hỏi lựa chọn** và **câu hỏi sắp xếp** của luật gốc soạn như câu hỏi thường, và **phần đề tự nêu format** thí sinh phải theo — ví dụ *"chọn A, B, C hay D"* hoặc *"viết thứ tự, cách nhau bởi dấu phẩy"*. Không có trường danh sách phương án (`QĐ-162`).

- **Đừng nhầm với**: **đáp án và bài làm luôn là CHUỖI.** Máy không chấm, nên nó không cần đánh giá một thứ tự hay một lựa chọn — nó chỉ **hiển thị bài làm cạnh đáp án** và tô khác biệt ký tự để admin phán quyết. Không có nhánh chấm riêng cho kiểu nào
- **Đừng nhầm với**: luật gốc liệt kê *3 loại câu* ở Khởi động và *4 loại* ở Tăng tốc, nhưng **nhìn nhanh · suy luận · đoạn băng · hình ảnh · đoạn nhạc** khác nhau ở **nội dung và media**, không ở cơ chế trả lời. Chúng là **phân loại cho người soạn đề**, không phải nhánh của engine
- **Đừng nhầm với**: **`isPractical`** là **kênh trả lời thứ tư**, nằm ngoài trục này — không có ô nhập nào, admin chấm *"đạt / không đạt"*

### TERM-045 — `timeSeconds`

**Định nghĩa.** Thời gian suy nghĩ, là **metadata của TỪNG CÂU HỎI** — cùng mức 20đ có thể câu 15s và câu 40s. Hệ thống chọn câu theo **mức điểm**, thời gian lấy **theo câu**; preset chỉ đặt default.

- **Đừng nhầm với**: nguồn ngoài **gắn chặt thời gian với mức điểm**; repo **cố ý** tổng quát hoá vượt nguồn. Per-question override **thắng** default của preset
- **Ngoại lệ DUY NHẤT của quy tắc "per-question thắng preset"**: ở **Câu hỏi phụ**, `timeSeconds` của câu **bị bỏ qua** — vòng đó luôn **15 giây**, con số **cố định, không cấu hình** (`GR-023`). Ngoại lệ chỉ phát sinh vì vòng này **mượn câu từ ba kho khác** và câu mượn mang theo `timeSeconds` của kho gốc (`QĐ-081`)
- **Nguồn**: `QĐ-055` *(giá trị mặc định)* · `QĐ-081` *(ngoại lệ)* · `CLAUDE.md` §Luật chơi & đề thi

### TERM-046 — Kho đề · Pool

**Định nghĩa.** Tập câu hỏi khả dụng. **Pool đã gán cho contest** là snapshot admin chọn trước khi start. Pre-flight kiểm **tại cửa vào từng vòng**; thiếu thì **không mở được vòng đó**.

- **Tên khác**: pool · ngân hàng đề · bộ đề · danh sách đã gán
- **Đừng nhầm với**: ba thứ khác nhau — **kho đề toàn hệ thống** ≠ **pool đã gán cho contest** ≠ **pool còn lại sau no-repeat**
- **Năm vòng nhưng chỉ BỐN kho**: Khởi động · VCNV · Tăng tốc · Về đích. **Câu hỏi phụ không có kho riêng** — kho của nó là **dẫn xuất** từ ba kho đầu trừ Tăng tốc (`QĐ-081`)
- **Nguồn**: `QĐ-042`, `QĐ-043`, `QĐ-081`

### TERM-047 — Rút đề

**Định nghĩa.** Server rút ngẫu nhiên **trong danh sách đã gán**, loại câu đã dùng trong contest. Phát event `QUESTIONS_DRAWN` để **replay được**.

- **Đừng nhầm với**: **Bốc thăm** (TERM-041) — tiếng Anh cả hai là *"draw"* nhưng khác hẳn nhau · **rút ≠ tiêu**: câu đã rút mà **chưa hiển thị** thì **chưa tiêu**, trả lại kho
- **Nguồn**: `QĐ-044`

### TERM-048 — `usedInContest` · No-repeat

**Định nghĩa.** Cờ đánh dấu câu **đã hiển thị cho thí sinh** trong bất kỳ trận nào của contest ⇒ câu **không xuất hiện lại** trong contest đó. **Câu đã dùng không trả lại pool**, kể cả khi vòng bị bỏ — *"điểm hoàn được, đề đã lộ thì không"*.

- **Giá trị**: mốc set cờ là **hiển thị**, không phải *"đã chấm"*. Câu bị bỏ qua, câu của vòng bị bỏ, câu của hàng ngang chưa hỏi khi cả sân bị loại — **đều tiêu**
- **Đừng nhầm với**: cờ này được dùng theo **hai chiều ngược nhau**: trận `official` lấy pool = câu `false`; trận `practice` **trong một contest thật** lấy pool = **CHỈ** câu `true` — nó chỉ được luyện trên đề đã lộ, nên **không bao giờ set thêm cờ nào** · **gỡ một câu khỏi danh sách gán không phải là trả nó về kho**
- **Nguồn**: `QĐ-040`, `QĐ-043`, `QĐ-044`

### TERM-049 — `everPublic`

**Định nghĩa.** Cờ **một chiều** đánh dấu câu đã từng nằm trong bộ đề public. Câu `everPublic = true` bị **chặn cứng** khỏi trận official; ép được với **xác nhận hai bước + audit**. Kiểm ở đơn vị **CÂU**.

Hàng rào gắn với **THAO TÁC, không gắn với mốc thời gian**: nó chạy ở **mọi** cửa đưa câu vào danh sách gán của một trận official — pre-flight, sửa danh sách ở `LOBBY`, hay import. **Không** áp cho trận `practice`.

- **Giá trị**: `true` / `false`, **một chiều vĩnh viễn** — đánh dấu nhầm thì phải tạo câu mới
- **Đi theo CÂU qua import/export** — khác `usedInContest`, vốn thuộc **contest** nên đặt lại khi nhập vào contest khác
- **Đừng nhầm với**: **`Question.visibility`** (TERM-050) là cờ **khác** — nó là **trạng thái hiện tại**, còn cờ này là **dấu vết lịch sử**. Chặn theo **`everPublic`**, không theo `visibility`, vì thứ nguy hiểm là *"đã từng lộ"* chứ không phải *"đang lộ"*
- **Nguồn**: `QĐ-040`, `QĐ-063`, `QĐ-071`

### TERM-050 — `Question.visibility`

**Định nghĩa.** **Giá trị DẪN XUẤT, read-only**: `PUBLIC` khi và chỉ khi câu **đang thuộc ≥1 bộ đề public**, ngược lại `PRIVATE`. **Không ai đặt được trực tiếp** — setter làm một câu thành public bằng cách **đưa nó vào một bộ đề public**.

- **Đừng nhầm với**: nếu cờ này set tay được thì một người **gỡ được nhãn public khỏi câu đã lộ**, vô hiệu hoá chính hàng rào chống rò đề bằng một thao tác trông hợp lệ. Đó là lý do nó **phải** là dẫn xuất · cặp đúng: **`visibility`** = hiện tại, **`everPublic`** (TERM-049) = lịch sử một chiều
- **Nguồn**: `QĐ-063`

### TERM-051 — RuleConfig · Preset `O26_DEFAULT@1`

**Định nghĩa.** Nơi khai **mọi** timer và điểm. **Không hard-code luật trong code hay UI.** Preset `O26_DEFAULT@1` là bộ giá trị mặc định theo luật O26, áp bằng nút *"Áp dụng luật 2026"*; mọi giá trị vẫn cấu hình được per-contest.

- **Đừng nhầm với**: hai option **hợp lệ về mặt hệ thống nhưng KHÔNG dùng cho preset O26** — `stealMode: 'add'` và `exhaustedFallback: 'admin-decides'`
- **Nguồn**: `CLAUDE.md` §Quy ước khác

---

# G. Thuật ngữ riêng của VCNV

### TERM-052 — Hàng ngang

**Định nghĩa.** Một trong **4 từ** cần đoán, đồng thời là **4 gợi ý** liên quan đến Chướng ngại vật. Thời gian suy nghĩ **15 giây**; trả lời đúng **+10**. **Mọi thí sinh chưa bị loại cùng trả lời bằng máy tính** khi một hàng ngang được chọn.

- **Giá trị**: O26 = **4 hàng**, mang số thứ tự **cố định** `1`→`4` trong một **Bộ VCNV** (TERM-058)
- **Đừng nhầm với**: ***"hàng ngang được MỞ" ≠ "miếng ghép được MỞ"*** — luật gốc dùng hai chủ ngữ khác nhau; đây là chìa khoá đọc điều kiện tới ô trung tâm · **lượt CHỌN hàng ngang** (theo vị trí, tối đa 1 lượt mỗi người) khác **việc TRẢ LỜI hàng ngang** (cả sân cùng trả lời) · **chủ thể CHỌN đổi theo mode**, còn **việc TRẢ LỜI thì luôn gõ máy**
- **Nguồn**: luật gốc §Vượt chướng ngại vật · `QĐ-018`, `QĐ-019`, `QĐ-052`

### TERM-053 — Trạng thái ô chữ

**Định nghĩa.** Mỗi ô của bàn cờ VCNV — 4 hàng ngang + ô trung tâm — mang **đúng một** trong ba giá trị: **chờ** → **đã hỏi** → **mở**. Năm ô độc lập với nhau.

- **Đừng nhầm với**: đây là trục **song song** với vòng đời câu hỏi, không lồng vào nhau. Một ô sang *đã hỏi* mà **chưa câu nào được hiển thị** là hợp lệ (admin đặt tay); một câu **đã chấm xong** mà ô vẫn ở *đã hỏi* cũng hợp lệ (không ai đúng)
- **Nguồn**: `QĐ-052`

### TERM-054 — Miếng ghép

**Định nghĩa.** Mảnh của hình ảnh Chướng ngại vật. **5 miếng**: 4 miếng ở 4 góc đánh số cố định tương ứng 4 hàng ngang, 1 miếng ở **ô trung tâm**. Có **≥1 người** trả lời đúng hàng ngang ⇒ miếng tương ứng **mở**.

- **Đừng nhầm với**: miếng ghép **không mở** vẫn không cản hàng ngang tiếp theo được hỏi — cả 4 hàng **luôn được hỏi hết**, nên ô trung tâm **luôn tới được**
- **Nguồn**: luật gốc §Vượt chướng ngại vật · `QĐ-057`

### TERM-058 — Bộ VCNV

**Định nghĩa.** **Đơn vị chọn tay và đơn vị rút của vòng Vượt chướng ngại vật.** Một bộ gồm **sáu thành phần**: **1 Chướng ngại vật** *(từ khoá ẩn + hình ảnh 5 miếng ghép)* · **4 hàng ngang** · **1 câu ô trung tâm**.

- **Tên khác**: bộ chướng ngại vật
- **Số thứ tự cố định**: bốn hàng ngang mang số `1`→`4`, mỗi số ứng với **một miếng ghép ở một góc cố định**. Hàng ngang **không hoán đổi được** — không giữa các bộ, cũng không cho nhau trong cùng bộ
- **Khả dụng**: một bộ chỉ mở được vòng VCNV khi **cả sáu thành phần đều chưa dùng**. Mất một thành phần là **vỡ bộ**
- **Đừng nhầm với**: ***hàng ngang KHÔNG phải câu hỏi độc lập*** — nó là **gợi ý của một Chướng ngại vật cụ thể** (*"4 từ hàng ngang, **cũng chính là 4 gợi ý liên quan đến** Chướng ngại vật"*). Ghép 4 hàng ngang bất kỳ với một Chướng ngại vật bất kỳ thì vòng **vẫn chạy trót lọt về kỹ thuật** nhưng **trò chơi mất nghĩa**, và không phép kiểm nào bắt được · **kho VCNV là kho các BỘ**, không phải kho câu · mượn một hàng ngang làm **Câu hỏi phụ** (`QĐ-081`) là **vỡ nguyên một bộ** — mất 6 thành phần để lấy 1, nên kho VCNV xếp **cuối** thứ tự ưu tiên rút
- **Nguồn**: luật gốc §Vượt chướng ngại vật, hai câu đầu · `QĐ-082` · `GR-031`

### TERM-055 — Chướng ngại vật

**Định nghĩa.** Đáp án ẩn mà cả vòng đi tìm. Thí sinh bấm chuông trả lời **bất cứ lúc nào** trong vòng. **Trả lời sai ⇒ bị loại khỏi vòng.**

- **Tên khác**: CNV · obstacle · từ khoá
- **Giá trị**: băng điểm **60 / 50 / 40 / 30** theo số hàng ngang **không còn ở trạng thái chờ**; sau gợi ý cuối là **20**, và 20 là **sàn**
- **Đừng nhầm với**: băng điểm đếm **số hàng ngang đã HỎI**, không phải số miếng ghép đã mở · băng chốt tại **mốc admin xác nhận tín hiệu**, không phải mốc thí sinh bấm · **ô trung tâm không vào phép đếm này** · nút *"Mở chướng ngại vật"* được xếp là **chuông**
- **Nguồn**: luật gốc §Vượt chướng ngại vật · `QĐ-052`, `QĐ-057`

### TERM-056 — Ô trung tâm · Gợi ý cuối

**Định nghĩa.** Miếng ghép thứ 5 kèm một câu hỏi, đưa ra **sau khi cả 4 hàng ngang đã được hỏi mà chưa ai giải đúng Chướng ngại vật**. **Mọi thí sinh chưa bị loại** cùng trả lời, luôn gõ máy, **không theo lượt**. Đúng ⇒ **+10** và ô mở; sai ⇒ ô không mở. Sau đó còn **15 giây** để giải Chướng ngại vật, đúng chỉ được **20 điểm**.

- **Đừng nhầm với**: mốc **20 điểm** gắn với việc **gợi ý cuối đã được đưa ra**, **không** phụ thuộc câu ô trung tâm đúng hay sai
- **Nguồn**: luật gốc §Vượt chướng ngại vật · `QĐ-052`, `QĐ-057`

---

# Tra cứu tên tiếng Anh

> Dùng khi gặp một định danh tiếng Anh trong code và cần biết nó là khái niệm nào.

| Tên tiếng Anh | Thuật ngữ chuẩn | Mã |
|---|---|---|
| `player`, `contestant` | Thí sinh | TERM-001 |
| `seat` | Ghế · Vị trí | TERM-002 |
| `scoringUnit` | Đơn vị điểm | TERM-003 |
| `admin`, `operator` | Admin | TERM-004 |
| `host` | **MC**, không phải admin | TERM-005 |
| `viewer` | Viewer | TERM-007 |
| `team` | Đội `[v2]` | TERM-009 |
| `contest` | Contest | TERM-010 |
| `match`, `game` | Match · Trận | TERM-011 |
| `user`, `account` | User — **một tài khoản đúng một vai** | TERM-008 |
| `round` | Vòng | TERM-012 |
| `turn` | Lượt — **4 nghĩa, luôn gọi tên đầy đủ** | TERM-013 |
| `state` | **Luôn kèm thang bậc** — có bảy thang | TERM-018 |
| `event` | Event · MatchEvent | TERM-021 |
| `audit log` | Nhật ký thao tác — **không phải** nhật ký sự kiện trận | TERM-063 |
| `revert` | Hoàn nguyên | TERM-023 |
| `buzz`, `buzzer` | Chuông | TERM-025 |
| `reject` | Từ chối tín hiệu | TERM-027 |
| `judge` | Phán quyết của admin | TERM-029 |
| `eliminated` | Bị loại — **không phải *"loser"*** | TERM-030 |
| `score` | Điểm | TERM-033 |
| `steal`, `transfer` | Cướp quyền | TERM-035 |
| `result` | Kết quả — **3 nghĩa, luôn gọi tên đầy đủ** | TERM-038 |
| `winner` | Người thắng | TERM-039 |
| `tie` | Hoà | TERM-040 |
| `draw` | **3 nghĩa** — rút đề · bốc thăm · hoà | TERM-047 · TERM-041 · TERM-040 |
| `timeout`, `deadline` | Hết giờ | TERM-042 |
| `questionClosed` | Câu khép — **mốc công bố đáp án**, khác *"đã chấm"* | TERM-057 |
| `question` | Câu hỏi | TERM-044 |
| `pool` | Kho đề | TERM-046 |
| `visibility` | **Dẫn xuất, read-only** — không set tay được | TERM-050 |
| `everPublic` | Dấu vết lịch sử một chiều | TERM-049 |
| `row` | Hàng ngang — **không phải câu độc lập** | TERM-052 |
| `obstacle` | Chướng ngại vật | TERM-055 |
| `obstacleSet` | Bộ VCNV — **đơn vị chọn và rút** của vòng | TERM-058 |

### TERM-064 — Hồ sơ triển khai

**Định nghĩa.** Một **bản cài đang chạy** của sản phẩm. Khái niệm này có **đúng hai** giá trị: **máy chủ** *(bản cài dựng bằng container)* và **portable** *(bản cài chạy LAN offline trên Windows)*. Một bản cài mang **đúng một** hồ sơ, và không hồ sơ nào phục vụ nhiều bản cài.

- **Tên thay thế**: *bản cài* · *bản triển khai* — ba cụm chỉ **cùng một** khái niệm, dùng lẫn nhau được (`QĐ-160`).
- **Miền giá trị**: `máy chủ` · `portable`.
- **Đừng nhầm với**: **contest** hay **trận** — hồ sơ triển khai là tính chất của một **bản cài**, không phải của một cuộc thi. **0** state và **0** transition nào của máy trạng thái trận đọc nó.
- **Ghi chú**: mọi **ngưỡng vận hành** *(kích thước media, giới hạn tần suất cổng khán giả, quy mô viewer)* đặt ở phạm vi **bản cài**; cụm *"theo hồ sơ"* đọc là *"theo bản cài, vì mỗi bản cài mang đúng một hồ sơ"*.
