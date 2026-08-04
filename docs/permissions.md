# Catalog phân quyền

> **Vai trò tài liệu.** Đây là **catalog tra cứu** — danh sách permission, phạm vi của chúng, và các vai seed. Cùng loại với `glossary.md`: nó **không đặt ra luật**. Mô hình và các điều cấm nằm ở `QĐ-094`; yêu cầu cấp sản phẩm ở `PRD.md` `PRD-REQ-109`→`111`.
>
> **Mã**: `PERM-001` → `PERM-061`.

---

## 1. Đọc bảng này thế nào

**Server kiểm PERMISSION, không bao giờ kiểm VAI.** Vai chỉ là một túi có tên chứa permission. Viết `nếu vai == "admin"` ở bất kỳ đâu là **vi phạm `QĐ-078`** — xem §5.

**Phép gán mang phạm vi**: `(tài khoản, vai, phạm vi)`, phạm vi ∈ {`HỆ THỐNG`, một contest cụ thể}.

**Mỗi tài khoản mang ĐÚNG MỘT vai** (`QĐ-065`). Ràng buộc đặt lên **cột *vai***: mọi phép gán của một tài khoản mang cùng một vai, nhưng tài khoản gán được ở **nhiều phạm vi**. Hệ quả: tổ hợp *Thí sinh* + *Quản trị / MC / Người ra đề* **không dựng nổi** — loại trừ là tính chất cấu trúc, không phải một luật phải kiểm.

| Cột | Nghĩa |
|---|---|
| **Phạm vi** | `HỆ THỐNG` — kho đề, tài khoản, vận hành bản cài · `CONTEST` — gán theo từng contest |
| **⚠️** | Thuộc **bảy thao tác phá huỷ** của `QĐ-078` |
| **Cổng thêm** | Cổng **ngoài** RBAC còn phải qua — xem §4 |

---

## 2. Permission theo phạm vi `HỆ THỐNG`

### 2.1 Kho đề

| Mã | Permission | Cho phép | Nguồn |
|---|---|---|---|
| `PERM-001` | `question.create` | Soạn câu hỏi mới, lưu ở `DRAFT` | `QĐ-063` · `JOURNEY-001` |
| `PERM-002` | `question.edit.own` | Sửa câu **của mình** khi còn `DRAFT` | `QĐ-063` |
| `PERM-003` | `question.edit.any` | Sửa câu của người khác | `[SUY RA]` từ `PERM-002` |
| `PERM-004` | `question.approve` | Duyệt `DRAFT` → `ACTIVE` | `QĐ-064` |
| `PERM-005` | `question.reject` | Trả câu về `DRAFT` kèm ghi chú | `QĐ-064` · `JOURNEY-001` |
| `PERM-006` | `question.read` | Xem câu hỏi trong kho **không kèm đáp án** | `GR-037` |
| `PERM-007` | `question.readAnswer` | Xem **đáp án chuẩn** trong kho. Mỗi lần xem vào nhật ký | `GR-037` C1, C2 |
| `PERM-008` | `question.search` | Tìm kiếm toàn văn, lọc, sắp xếp kho đề | `PRD-REQ-014` |
| `PERM-009` | `questionSet.manage` | Tạo và sửa bộ đề | `QĐ-063` |
| `PERM-010` | `questionSet.publish` | Đặt một bộ đề thành public. **Kéo theo `everPublic` một chiều** cho mọi câu trong đó | `QĐ-063`, `QĐ-071` |
| `PERM-011` | `question.markUsed.bulk` | Đánh dấu "đã dùng" hàng loạt bằng tay | `QĐ-084` |

### 2.2 Tài khoản, vai, nhật ký

| Mã | Permission | Cho phép | Nguồn |
|---|---|---|---|
| `PERM-012` | `user.manage` | Tạo, sửa, vô hiệu hoá tài khoản | `QĐ-087` |
| `PERM-013` | `role.manage` | **Định nghĩa vai tuỳ biến** và sửa túi permission của nó | `QĐ-086` vế 7 |
| `PERM-014` | `role.assign.system` | Gán vai ở phạm vi `HỆ THỐNG` | `QĐ-065` |
| `PERM-015` | `audit.read` | Đọc nhật ký thao tác | `PRD-REQ-081` |

### 2.3 Nhập, xuất, vận hành bản cài

| Mã | Permission | Cho phép | Nguồn |
|---|---|---|---|
| `PERM-016` | `package.export` | Xuất gói contest *(câu hỏi, metadata media, media)* | `QĐ-071`, `QĐ-084` |
| `PERM-017` | `package.import` | Nhập gói contest | `QĐ-071` |
| `PERM-018` | `participants.export` | Xuất **danh sách người tham gia**. Tách riêng khỏi `PERM-016`: đây là một lần **dữ liệu cá nhân rời hệ thống** | `QĐ-086` |
| `PERM-019` | `participants.import` | Nhập danh sách người tham gia, dựng lại tài khoản | `QĐ-086`, `QĐ-087` |
| `PERM-020` | `results.export` | Xuất biên bản, nhật ký sự kiện, bản kê câu đã dùng, kết quả rút gọn | `QĐ-084`, `QĐ-077` |
| `PERM-021` | `retention.manage` | Đặt hạn lưu trữ theo mục đích trận | `QĐ-091` |

---

## 3. Permission theo phạm vi `CONTEST`

### 3.1 Dựng contest

| Mã | Permission | Cho phép | ⚠️ | Nguồn |
|---|---|---|---|---|
| `PERM-022` | `contest.create` | Tạo contest. Người tạo thành **chủ contest** *(thuộc tính cố định, không gán lại)* | | `TERM-059` |
| `PERM-023` | `contest.edit` | Cấu hình luật, preset, mode trả lời, chủ đề, âm thanh | | `QĐ-075`, `TERM-051` |
| `PERM-024` | `contest.assignSeat` | Gán ghế và vị trí | | `GR-016` |
| `PERM-025` | `role.assign.contest` | Gán **vai vận hành** trong contest này | | `QĐ-065`, `PRD-REQ-006` |
| `PERM-026` | `contest.questionList.edit` | Chọn và sửa danh sách câu đã gán | ⚠️ | `QĐ-078`, `GR-031` |
| `PERM-027` | `contest.everPublic.override` | Ép một câu mang `everPublic` vào trận official | ⚠️ | `QĐ-078`, `QĐ-071` |
| `PERM-028` | `preflight.run` | Chạy kiểm kho đề | | `GR-031` |

### 3.2 Vòng đời trận

| Mã | Permission | Cho phép | ⚠️ | Nguồn |
|---|---|---|---|---|
| `PERM-029` | `match.create` | Tạo trận mới trong contest | | `QĐ-039` |
| `PERM-030` | `match.start` | Bắt đầu trận ⇒ **đóng băng cấu hình** | | `GR-031`, `GR-037` |
| `PERM-031` | `round.open` | Mở một vòng | | `GR-030`, `INV-020` |
| `PERM-032` | `round.end` | Kết thúc vòng theo cửa thường | | `GR-030` |
| `PERM-033` | `round.skip` | **Bỏ vòng** — hoàn nguyên bằng event đảo ngược | ⚠️ | `QĐ-078`, `GR-030` |
| `PERM-034` | `round.rerun` | **Chạy lại vòng** | ⚠️ | `QĐ-078`, `GR-030` |
| `PERM-035` | `round.emergencyEnd` | **Kết thúc vòng khẩn cấp** — giữ nguyên điểm | ⚠️ | `QĐ-078`, `GR-030` |
| `PERM-036` | `match.finalize` | Chốt trận | | `GR-022` |
| `PERM-037` | `match.cancel` | **Huỷ trận** — đóng sổ với nhãn `bỏ dở` | ⚠️ | `QĐ-078`, `GR-022` |

### 3.3 Điều khiển một câu

| Mã | Permission | Cho phép | Cổng thêm | Nguồn |
|---|---|---|---|---|
| `PERM-038` | `question.display` | **Mốc 1** — hiển thị câu hỏi | phiên giữ quyền | `GR-033` |
| `PERM-039` | `timer.start` | **Mốc 2** — start timer *(ánh xạ "MC đọc xong")* | phiên giữ quyền | `GR-033` |
| `PERM-040` | `practice.start` | **Mốc 3** — bắt đầu pha thực hành | phiên giữ quyền | `GR-019` |
| `PERM-041` | `judge` | **Mốc 4** — phán quyết Đúng / Sai / Huỷ kết quả, và chốt câu | phiên giữ quyền | `GR-026` |
| `PERM-042` | `signal.approve` | Duyệt / từ chối tín hiệu trong hàng đợi chặn *(VCNV)* | phiên giữ quyền | `GR-032` |
| `PERM-043` | `score.adjust` | **Điều chỉnh điểm thủ công** — lý do bắt buộc | phiên giữ quyền | `QĐ-078`, `GR-029` |

*`PERM-043` mang ⚠️ — thuộc bảy thao tác phá huỷ.*

### 3.4 Hiển thị và bàn cờ

| Mã | Permission | Cho phép | Cổng thêm | Nguồn |
|---|---|---|---|---|
| `PERM-044` | `answer.reveal` | Mở và đóng đáp án bằng tay **cho mọi vai đang xem**. **Thắng cờ `revealAnswerAfterJudge` ở CẢ HAI CHIỀU** — cú mở thắng cờ TẮT, cú đóng **chặn** cú đẩy của engine tại mốc câu khép kể cả khi cờ BẬT; hiệu lực của cú đóng ở **phạm vi câu** | phiên giữ quyền | `QĐ-048`, `QĐ-074`, `QĐ-119` |
| `PERM-045` | `match.readAnswer` | **Nhận đáp án chuẩn của câu đang chạy trên màn của CHÍNH MÌNH** — mọi lúc, **không** phụ thuộc cờ `revealAnswerAfterJudge` và **không** phụ thuộc mốc câu khép. Mỗi lần nhận vào nhật ký | — | `GR-037` C1, C2 |
| `PERM-046` | `crossword.setState` | Đặt trạng thái ô chữ theo cả hai chiều | phiên giữ quyền | `QĐ-052` |
| `PERM-047` | `overlay.announce` | Mở và đóng lớp công bố kết quả. **Loại trừ với `PERM-048`**: không mở được khi banner tạm dừng đang bật | phiên giữ quyền | `QĐ-049`, `QĐ-117` |
| `PERM-048` | `overlay.pause` | Mở và đóng banner tạm dừng. **Loại trừ với `PERM-047`**: không mở được khi lớp công bố đang bật. Banner **không phủ màn admin và màn MC** | phiên giữ quyền | `QĐ-050`, `QĐ-116`, `QĐ-117` |
| `PERM-049` | `viewer.gate.lock` | **Khoá cổng** phòng khán giả | | `QĐ-067`, `PRD-REQ-086` |

> **Đừng lẫn `PERM-044` với `PERM-045`** — đây là cặp dễ lẫn nhất trong catalog. `PERM-044` là **hành vi GHI**: một cú bấm đẩy đáp án ra **mọi màn đang xem**. `PERM-045` là **hành vi ĐỌC**: đáp án hiện trên màn của **chính người giữ permission**, không ai khác thấy gì. Vai *Quản trị* giữ cả hai; vai **MC chỉ giữ `PERM-045`** — MC đọc được đáp án nhưng **không** công bố được cho ai.
>
> `PERM-045` cũng **khác `PERM-007`**: `PERM-007` là đáp án **trong kho đề**, phạm vi `HỆ THỐNG`; `PERM-045` là đáp án của **câu đang chạy trong một trận**, phạm vi `CONTEST`. Hai phạm vi khác nhau nên không gộp được.

### 3.5 Ghế và quyền điều khiển

| Mã | Permission | Cho phép | Cổng thêm | Nguồn |
|---|---|---|---|---|
| `PERM-050` | `seat.disable` | Vô hiệu hoá và kích hoạt lại một ghế | phiên giữ quyền | `GR-036`, `QĐ-047` |
| `PERM-051` | `seat.disconnect.judge` | Phán quyết ghế mất kết nối — giữ hoặc gia hạn | phiên giữ quyền | `GR-036` |
| `PERM-052` | `match.control.transfer` | **Chuyển** quyền điều khiển cho phiên khác | **phải đang giữ quyền** | `QĐ-008` |
| `PERM-053` | `match.control.seize` | **Giành** quyền điều khiển | **holder phải mất kết nối** | `QĐ-093` |

### 3.6 Quyền của MC

| Mã | Permission | Cho phép | Nguồn |
|---|---|---|---|
| `PERM-054` | `match.control.seize.approve` | Duyệt / từ chối một cú giành quyền. **Đây là permission GHI DUY NHẤT mà vai MC được giữ** | `QĐ-093` |

> **Rào chắn chống lan.** `PERM-054` MUST NOT xuất hiện trong bất kỳ túi nào cùng với một permission ghi khác của trận. Đặc biệt: MC **không** giữ `PERM-042` `signal.approve` — hàng đợi tín hiệu của **thí sinh** vẫn thuộc admin (`QĐ-093`, `QĐ-001`).

### 3.7 Thao tác của thí sinh

> Bảy permission dưới đây **luôn** đi kèm hai cổng ngoài RBAC: tài khoản phải **đang ngồi đúng ghế** của trận, và **trạng thái game** phải cho phép (`INV-014`, `GR-034`). Có permission mà chưa tới lượt hoặc chuông đang khoá thì vẫn không đi được.

| Mã | Permission | Cho phép | Nguồn |
|---|---|---|---|
| `PERM-055` | `seat.buzz` | Bấm chuông. **Chỉ nhận click chuột** | `GR-034`, `EVENT-037` |
| `PERM-056` | `seat.openObstacle` | Bấm *"Mở chướng ngại vật"*. Xếp hạng **chuông** | `GR-034`, `EVENT-038` |
| `PERM-057` | `seat.submitAnswer` | Gửi đáp án | `GR-015`, `EVENT-040` |
| `PERM-058` | `seat.pickRow` | Chọn hàng ngang — **chỉ ở mode nhập liệu** | `QĐ-019`, `EVENT-039` |
| `PERM-059` | `seat.pickPackage` | Chọn gói câu — **chỉ ở mode nhập liệu** | `QĐ-019`, `EVENT-041` |
| `PERM-060` | `seat.setHopeStar` | Đặt Ngôi sao hy vọng — **chỉ ở mode nhập liệu** | `QĐ-019`, `EVENT-042` |
| `PERM-061` | `scoreboard.read` | Xem bảng điểm của **mọi** ghế, gồm điểm âm | `QĐ-015` |

---

## 4. Bốn cổng ĐỨNG NGOÀI RBAC

Có permission là **điều kiện cần, không phải điều kiện đủ**. Bốn cổng sau độc lập, và gộp bất kỳ cái nào vào RBAC là sai:

| Cổng | Nội dung | Nguồn |
|---|---|---|
| **Phiên giữ quyền điều khiển** | Ghi vào một trận đang chạy còn cần phiên **đang giữ quyền**. Hai phiên admin đủ quyền như nhau, chỉ một phiên bấm được | `QĐ-008` |
| **Một tài khoản một vai** | Ràng buộc trên **cột *vai*** của phép gán, không phải một permission. Nó làm tổ hợp *Thí sinh* + *Quản trị / MC / Người ra đề* **không dựng nổi** — loại trừ là tính chất cấu trúc, không còn cửa nào phải kiểm | `QĐ-065` |
| **Chủ contest** | Thuộc tính cố định của contest, dùng đúng một việc: **ưu tiên khi nhiều admin cùng giành quyền**. **Không phải vai, không phải permission** | `TERM-059`, `QĐ-093` |
| **Trạng thái game** | Chuông đang khoá · chưa tới lượt · ghế bị vô hiệu hoá · trận `FINISHED` niêm phong · ba chỗ chặn cứng. Đủ quyền vẫn không đi được | `INV-014`, `GR-029` C6 |

---

## 5. Bốn điều CẤM

1. **CẤM kiểm vai thay cho kiểm permission.** Không đường code nào được hỏi *"tài khoản này có phải admin không"* để quyết cho đi hay không. Chỉ hỏi *"tài khoản này có permission X trong phạm vi Y không"*. Đây là phát biểu thi hành được của `QĐ-078`.
2. **CẤM cấp permission thẳng cho một tài khoản.** Mọi permission tới người dùng **qua một vai**. Muốn một người có bộ quyền khác thì tạo **vai tuỳ biến** (`PERM-013`), không vá ngoại lệ lên tài khoản.
3. **CẤM suy permission này ra permission kia.** Có `PERM-043` `score.adjust` không kéo theo `PERM-037` `match.cancel`. Bảy thao tác ⚠️ mỗi thứ đứng riêng.
4. **CẤM để `PERM-054` đi cùng permission ghi khác của trận** trong một túi vai — xem §3.6.

---

## 6. Bốn vai seed

Bản cài trống có đúng bốn vai dựng sẵn, khớp bốn vai cần xác thực của `TERM-008`. Mọi cấu hình quyền khác dựng bằng **vai tuỳ biến** (`PERM-013`), không bằng cách sửa bốn vai này.

| Vai | Phạm vi gán được | Túi permission |
|---|---|---|
| **Quản trị** *(admin)* | `HỆ THỐNG` và `CONTEST` | **Toàn bộ** `PERM-001`→`PERM-053`, gồm cả bảy thao tác ⚠️. **Trừ** `PERM-054`→`PERM-061` |
| **Người ra đề** *(setter)* | `HỆ THỐNG` | `PERM-001`, `002`, `006`, `007`, `008`, `009`, `010`, `016`, `017` |
| **MC** | `CONTEST` | `PERM-045`, `PERM-054`, `PERM-061` — **đúng một permission ghi** (`PERM-054`), dùng được **kể cả khi banner tạm dừng đang bật** (`QĐ-116`) |
| **Thí sinh** | `CONTEST` | `PERM-055` → `PERM-061` |

> **Túi của mỗi vai chỉ chứa permission ĐÚNG PHẠM VI mà vai đó gán được.** Vai MC gán ở phạm vi `CONTEST`, nên nó **không** giữ được `PERM-006`/`PERM-007` — hai permission của **kho đề**, phạm vi `HỆ THỐNG`. MC đọc đáp án của **câu đang chạy** qua `PERM-045`, không qua kho đề: MC không có việc gì trong kho đề.

> **Vì sao vai Quản trị seed giữ đủ bảy thao tác phá huỷ.** `QĐ-087` khai *"tài khoản admin là **toàn quyền trên trận**"*. Ca dùng mà `QĐ-078` muốn giải — *"cấp một tài khoản chạy trận mà không cho nó xoá vòng"* — dựng bằng một **vai tuỳ biến** bỏ bảy mục ⚠️, **không** bằng cách bóp vai seed. `QĐ-078` đòi việc đó **làm được**; nó không đòi mặc định phải hẹp.

**Vai tuỳ biến** do đơn vị tự định nghĩa **phải xuất kèm định nghĩa** trong gói contest — nếu không, bên nhận dựng lại được tài khoản mà không dựng lại được quyền (`QĐ-086` vế 7).

---

## 7. Vòng đời của một phép cấp

**Trong lúc một trận chưa đóng sổ, quyền chỉ NỞ RA, không bao giờ CO LẠI** (`QĐ-095`).

| Thao tác | Trận liên quan **chưa đóng sổ** | Không trận nào chạy |
|---|---|---|
| **Thêm** permission vào túi một vai | ✅ Được, **hiệu lực ngay** | ✅ Được, hiệu lực ngay |
| **Gán thêm** một **phạm vi** cho tài khoản *(vẫn đúng vai đó — `QĐ-065`)* | ✅ Được, **hiệu lực ngay** | ✅ Được, hiệu lực ngay |
| **Gỡ** permission khỏi túi một vai | ❌ **Invalid state** | ✅ Được, hiệu lực ngay |
| **Thu hồi** phép gán vai | ❌ **Invalid state** | ✅ Được, hiệu lực ngay |
| **Vô hiệu hoá** tài khoản | ❌ **Invalid state** | ✅ Được, hiệu lực ngay |

**Ba đường bị chặn cùng lúc** vì luật phát biểu theo **hiệu ứng** — *làm giảm quyền hiệu dụng* — chứ không theo tên thao tác. Chặn một đường mà hở hai đường kia thì không chặn gì cả.

**"Trận liên quan" xác định bằng PHẠM VI của phép gán** (`QĐ-094`):

- Phép gán ở **một contest** ⇒ chỉ trận của contest đó khoá.
- Phép gán ở **`HỆ THỐNG`** ⇒ **bất kỳ** trận nào đang chạy cũng khoá.
- Sửa **túi của một vai** ⇒ khoá nếu **bất kỳ** phép gán nào của vai đó đang dính một trận chưa đóng sổ.

**Mốc mở khoá là `STATE-008` `FINISHED`.** `STATE-001` LOBBY **vẫn khoá** — ở đó vẫn kẹt được: mất `PERM-031` thì không mở vòng kế, mất `PERM-052` thì không chuyển quyền, mà còn kết nối thì `PERM-053` không mở.

**Hạng phản hồi: invalid state** — nút không bật, server từ chối, **không ép được**. Không phải chặn cứng: `INV-014` vẫn đúng ba chỗ, và cả ba đều là ngưỡng của **luật chơi**.

**Cần gỡ người giữa buổi thì đóng sổ trận trước** — chốt trận, hoặc huỷ trận với nhãn `bỏ dở` (`QĐ-038`) — rồi thu hồi. Đường này luôn có và để lại biên bản.

---

## 8. Ai KHÔNG có mục trong catalog này

**Khán giả** và **máy dựng stream** không có permission nào, **không có tài khoản**, và **không có vai**. Chúng vào phòng bằng **đúng một URL** — mã phòng nằm trong URL, không bước nhập mã, không chờ duyệt (`QĐ-096`).

Hai kênh này là **một chiều, không có đường ghi** — read-only là tính chất **cấu trúc**, không phải một permission bằng rỗng phải kiểm (`QĐ-088`, `PRD-REQ-107`). Đừng tạo vai *"viewer"* trong catalog: một vai rỗng gợi ý rằng có thứ để cấp thêm.

**URL là thứ duy nhất giữ quyền vào**, nên nó rò dễ hơn mã gõ tay. Hàng rào còn lại là **rate-limit** và nút **khoá cổng** (`PERM-049`, `PRD-REQ-086`) — không có yếu tố thứ hai.
