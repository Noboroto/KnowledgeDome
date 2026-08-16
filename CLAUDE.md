# KnowledgeDome — Hệ thống thi đấu Olympia

Nền tảng web tổ chức thi đấu gameshow kiến thức tuỳ biến, mô hình *Đường lên đỉnh Olympia*, chạy luật mùa **O26** làm preset mặc định.

**Toàn bộ tài liệu, spec, plan, task và ghi chú trong repo viết bằng tiếng Việt.**

---

## 1. Nguồn sự thật

**`docs/` là nguồn sự thật DUY NHẤT.** Bản đồ tài liệu: `docs/README.md`. Sổ quyết định: `docs/decisions.md` (`QĐ-001` → `QĐ-163`) — đây là nơi duy nhất ghi *vì sao*.

### Thứ bậc nguồn

| # | Đường dẫn | Vai trò |
|---|---|---|
| 1 | `docs/source/` | Tài liệu gốc về nghiệp vụ và sản phẩm. **Không sửa** trừ khi được yêu cầu rõ ràng |
| 2 | `docs/PRD.md` | Requirement cấp sản phẩm và định nghĩa epic. Phải truy nguyên về `docs/source/` |
| 3 | `specs/<feature>/spec.md` | Requirement và user story chuẩn tắc của feature *(Spec Kit)*. **Thắng** mọi mô tả không chính thức trong chat hay trong plan |
| 4 | `plans/<feature ID>/yyyyMMdd-HHmmss <feature/topic>/plan.md` | Kế hoạch kỹ thuật chuẩn tắc của feature đó *(ClaudeKit)* |
| 5 | `specs/<feature>/tasks.md` | Danh sách task thực thi chuẩn tắc *(ClaudeKit)* |
| 6 | `plans/yyyyMMdd-HHmmss <feature/topic>/plan.md` | Kế hoạch kỹ thuật cho việc nhỏ **nằm ngoài** spec *(ClaudeKit)* |

Tầng dưới phải truy nguyên được về tầng trên.

### Quy ước đặt tên tài liệu

**Mọi tài liệu trong repo mang prefix timestamp `yyyyMMdd-HHmmss <topic>`** *(24 giờ, giờ UTC+7 lúc tạo)* — plan, research, review, journal, ghi chú, báo cáo. Prefix đặt ở **thư mục** khi tài liệu có nhiều file, đặt ở **tên file** khi chỉ một file.

**Ngoại lệ duy nhất: `docs/` và `specs/`** — hai thư mục này giữ tên ổn định, không timestamp, vì chúng là nguồn sự thật và bị tham chiếu chéo bằng đường dẫn cố định.

### Quy tắc đặc tả

- **Không bao giờ tự bịa requirement nghiệp vụ.**
- Thiếu thông tin ⇒ đánh dấu `NEEDS CLARIFICATION`. Mâu thuẫn ⇒ đánh dấu `CONFLICT`. Không tự hoà giải.
- Mọi requirement phải trỏ được về nguồn của nó.
- **Phân vai công cụ**: **Spec Kit chỉ sở hữu ĐẶC TẢ** — `specs/<feature>/spec.md`. **ClaudeKit sở hữu mọi thứ từ đó trở xuống** — `plan.md`, `tasks.md`, scout, research, implement, review, test.
- Plan của mọi feature đều do ClaudeKit tạo, đặt trong `plans/` theo quy ước đặt tên bên dưới, và phải truy nguyên về `specs/<feature>/spec.md`. `tasks.md` vẫn đặt trong `specs/<feature>/` để đứng cạnh spec, nhưng do ClaudeKit sinh ra từ plan.
- **`plans/` không bao giờ là nguồn.** Plan là tầng dưới của `spec.md`; một requirement nghiệp vụ **không được phép ra đời ở đó**.
- **Không mở rộng phạm vi feature trong lúc implement.**

> Chi tiết cưỡng chế — 6 cổng chất lượng, định dạng marker, quy tắc sửa đổi — ở `.specify/memory/constitution.md` **bản 1.4.0**.
>
> **Không còn đường nào đưa nội dung ngoài `docs/` vào spec.** `docs/reviews/**`, `public/`, lịch sử git và ghi chú trong chat đều **không** thoả cổng truy nguyên.

---

## 2. Phạm vi phiên bản

| Phiên bản | Nội dung |
|---|---|
| **v1 — Solo contest** | Contest chính thức, thí sinh **cá nhân**, đủ loại vòng và biến thể, kho đề, màn viewer / overlay / MC / admin, hai hồ sơ triển khai *(compose và portable Windows)* |
| **v1.5 — Practice + luật đa ghế** | `matchPurpose: practice`, bộ đề public + share-link, giao diện luyện tập solo, vai trainer, retention riêng theo `matchPurpose` *(giá trị mặc định ở `QĐ-091`)* · **luật cho 1-12 thí sinh** — chỉ ship luật và sửa controller, **không** migrate schema |
| **v2 — Teams** | Thi đội: bấm chuông cá nhân, điểm về đội |

**Luật v1 đặc tả cho TỐI ĐA 4 THÍ SINH** (`QĐ-007`, `QĐ-105`). Luật gốc O26 viết cho đúng 4 người; **không phát minh luật cho số ghế lớn hơn**.

Trận **dưới 4 thí sinh vẫn chạy được ở v1**: ghế thiếu người thành **ghế bỏ thi** — vô hiệu hoá từ đầu trận, điểm luôn 0, xếp hạng bình thường nhưng không tham gia phép phân định hoà — và trận áp **nguyên luật 4 ghế** (`QĐ-105`). Trận **trên 4 ghế** bị **chặn cứng ở cú bấm bắt đầu trận**, vì thang điểm Tăng tốc chỉ định nghĩa cho 4 đơn vị điểm.

Nhưng **lưu trữ dữ liệu và giao diện vẫn làm cho 1-12 người ngay từ v1** — schema, seat model, `scoringUnit`, RuleConfig dạng mảng và toàn bộ UI. Chỉ **thang điểm** cho hơn 4 ghế là chưa có.

**Schema chuẩn bị ĐẦY ĐỦ ngay từ v1** — `Team`, `seat.teamId`, `scoringUnit`, `matchPurpose`, `visibility`/`everPublic`, ACL, retention. Không để dành schema cho phiên bản sau; v1 chỉ chưa bật engine-path tương ứng.

---

## 3. Luật chơi

### Nguồn luật

Source of truth về luật O26 là **Fandom wiki** ([Luật chơi/Olympia 26](https://duong-len-dinh-olympia.fandom.com/vi/wiki/Lu%E1%BA%ADt_ch%C6%A1i/Olympia_26)); bản lưu bất biến ở `docs/source/fandom-olympia-26-luat-choi.md`. Bảng đối chiếu giá trị và biến thể bị loại: `docs/traceability.md`.

Mọi giá trị vẫn là **RuleConfig tuỳ biến được**; contest builder có nút *"Áp dụng luật 2026"* áp preset `O26_DEFAULT@1`.

### Điểm độc lập thời gian

`timeSeconds` là metadata **của từng câu hỏi** — cùng mức 20 điểm có thể câu 15 giây và câu 40 giây. Hệ thống chọn câu theo **mức điểm**, thời gian lấy theo câu; preset chỉ đặt mặc định.

### Kho đề

- **Người tạo contest phải chọn danh sách câu trước khi bắt đầu trận** — tìm kiếm toàn văn, lọc, sắp xếp trên kho đề. Hệ thống **không** tự lấy đề: rút đề chỉ **ngẫu nhiên trong danh sách đã gán**. Pre-flight chặn bắt đầu khi thiếu.
- **Xuất/nhập contest trọn gói**: ZIP gồm Excel câu hỏi *(mặc định; nhận cả CSV và Google Sheet)*, JSON metadata media, và media theo thư mục con từng vòng. Dùng cho luồng soạn trên bản có Internet rồi nhập vào bản portable.

### Mô hình ADVISORY — người vận hành phán quyết

Nguyên tắc nền: **máy độc quyền SỰ KIỆN, người độc quyền PHÁN QUYẾT** (`QĐ-001`). Ba tầng, áp cho mọi câu hỏi dạng *"ai làm X"*:

| Tầng | Vai trò | Phương tiện |
|---|---|---|
| **MC** | Thẩm quyền phán quyết **trên sân khấu** | **Nói** |
| **Admin** | **Cảm biến và cơ cấu chấp hành DUY NHẤT** của hệ thống | **Bấm** |
| **Server** | Sự kiện và thời gian — server time, thứ tự chuông, đồng hồ | Không ai sửa được |

**Màn `/mc` là read-only, trừ đúng MỘT ngoại lệ**: MC duyệt cú **giành quyền điều khiển** khi phiên admin đang giữ mất kết nối (`QĐ-093`) — chỗ duy nhất mà tầng *"bấm"* trống nên không còn ai thi hành lời của MC. Ngoài đó **không tạo bề mặt quyền ghi nào cho MC**; đặc biệt MC **không** duyệt tín hiệu của thí sinh — hàng đợi VCNV vẫn thuộc admin.

Hệ quả:

- **Máy không tự chấm Đúng/Sai.** Với câu gõ, máy chỉ **tô ký tự khác** giữa bài làm và đáp án; admin tự đánh giá chính tả và ý nghĩa tương đồng. **Không viết đường code nào tự cộng trừ điểm từ kết quả so khớp.**
- **Admin chọn vòng nào bắt đầu và lượt của ai.** Hệ thống chỉ **khuyến nghị** theo luật và cấu hình contest — vị trí thí sinh và thứ tự lượt riêng Khởi động đều là **đầu vào của khuyến nghị**, không phải ràng buộc. Lệch luật ⇒ **dialog cảnh báo**, admin bấm Yes là thực hiện. **Không chặn cứng.**
- **Admin toàn quyền mở và đóng đáp án, ô chữ.**
- **Mọi mốc thời gian mà luật gốc mô tả bằng hành vi của MC đều ánh xạ thành một cú bấm của admin** — máy không quan sát được sân khấu. *"MC đọc xong câu hỏi"* ⇒ admin bấm start timer; *"hiệu lệnh của người dẫn chương trình"* ⇒ admin bấm.
- **Mốc do admin bấm là tuyệt đối**: không cửa sổ ân hạn, không trừ bù độ trễ tay người. Van thoát không phải grace mà là **lịch sử đầy đủ** để admin xem lại và gỡ lệnh cấm.
- **Bỏ vòng và chạy lại vòng** được phép, miễn còn câu. **Điểm là hàm của event log; hoàn nguyên là REVERT** — thêm event đảo ngược như `git revert`, không phải `reset --hard`. Lịch sử linear, append-only, **không xoá**; vòng bị bỏ hiện trong biên bản kèm nhãn *"đã bỏ"*. Câu đã dùng **không** trả lại kho.
- **Vòng tính điểm theo thứ hạng (Tăng tốc): một câu là MỘT event điểm cho toàn bộ bảng**, không phải mỗi người một event — hoàn nguyên là đảo cả bảng của câu đó.

### Hai mode trả lời

Cấu hình ở **cấp contest**, một giá trị chung cho mọi vòng.

- **Mode sân khấu** *(mặc định, và là mode mà luật được đặc tả theo)*: thí sinh **đọc** đáp án; máy chỉ dùng để giành quyền trả lời.
- **Mode nhập liệu**: thí sinh **gõ** đáp án.

Áp cho **Khởi động, Về đích, Câu hỏi phụ**. **VCNV và Tăng tốc luôn gõ máy** bất kể mode — ở VCNV, mode chỉ đổi cách **chọn hàng ngang**, và thí sinh vẫn dùng máy để bấm *"Mở chướng ngại vật"*.

**Chọn hàng ngang có đúng MỘT đường vào cho mỗi mode**: sân khấu ⇒ **chỉ admin** click, máy thí sinh không render nút chọn; nhập liệu ⇒ **chỉ thí sinh** click, **admin không chọn thay**. Cả hai đường đều qua hàng đợi và admin xác nhận Yes/No.

**Điểm được phép ÂM**, không có sàn.

### Hàng đợi tín hiệu

- **Mọi tín hiệu của thí sinh vào hàng đợi theo thứ tự tới** (server timestamp). **Không có cơ chế drop** — mọi tín hiệu đã tới server đều có outcome: thực thi, bị từ chối, hoặc **trơ**.
- **Hàng đợi đang hoạt động** đặt lại theo **đích** của tín hiệu; **lịch sử tín hiệu không bao giờ xoá**, để admin xem lại và gỡ lệnh cấm.
- **Chỉ CHẶN ở VCNV** — chọn hàng ngang và *"Mở chướng ngại vật"*: admin duyệt lần lượt, xác nhận mới có hiệu lực. Từ chối ⇒ tín hiệu kế lên, **thí sinh không mất lượt**. Đây là chỗ sửa lỗi bấm nhầm của thí sinh — admin bấm No, không phải bắt thí sinh xác nhận.
- **Khởi động lượt chung, cướp quyền Về đích, Câu hỏi phụ — không chặn**: có chuông là tính ngay theo server timestamp. Hàng đợi vẫn ghi thứ tự làm lưới an toàn.
- **Tiêu chí phân biệt**: tín hiệu *một chiều, hậu quả nặng, không bị ép thời gian* ⇒ chặn. Tín hiệu *đua tốc độ, cửa sổ chặt* ⇒ không chặn, server phân xử ngay.
- Khi người đang giữ quyền bị chấm **Huỷ kết quả**, admin **kích hoạt tay** được một tín hiệu khác còn hiệu lực trong hàng đợi (`QĐ-104`).

---

## 4. Kiến trúc và bảo mật

### Stack

**Backend**: NestJS + **Express adapter** *(không dùng Fastify)*, Zod, Prisma + Postgres, Redis, Better-auth, Socket.IO, `@casl/ability`.
**Frontend**: React + Vite, MUI, Motion for React, Zustand, TanStack Query.
**Lưu trữ media**: MinIO.

### Kênh và mô hình truy cập

| Kênh | Ai dùng | Cơ chế |
|---|---|---|
| **Có xác thực** | Admin, thí sinh, MC, setter | **Socket.IO** hai chiều. Đăng nhập bằng username + password, kiểm permission |
| **Public** | Màn khán giả, overlay OBS | **SSE + REST một chiều**, vào bằng **mã phòng 6 số** — không tài khoản, không chờ duyệt. Có rate-limit và nút *"khoá cổng"* của admin |

Kênh public **không có đường ghi**, nên read-only ở đó là tính chất **cấu trúc**, không phải một luật server phải cưỡng chế (`QĐ-088`).

### Zero-trust

**Không bao giờ tin client.** Mọi request và socket event đều verify auth + permission (CASL) ở server, bất kể client là ai, đã join room gì, hay giao diện có ẩn nút hay không. Mọi input validate lại ở server — validate ở frontend chỉ là UX.

### Phạm vi hiển thị đáp án

Đây là hàng rào chống rò đề; sai ở đây là hỏng sản phẩm.

- **Trước mốc CÂU KHÉP**, đáp án chỉ rời server tới phiên giữ permission `PERM-045` `match.readAnswer` *(có xác thực, có audit)* — ở bốn vai dựng sẵn là **Quản trị** và **MC**. Thí sinh, khán giả và overlay **không** nhận.
- **Cửa kiểm hỏi PERMISSION, không hỏi tên vai** (`QĐ-094`).
- **Từ mốc câu khép**, server đẩy đáp án tới thí sinh, khán giả và overlay nếu cờ `revealAnswerAfterJudge` bật — cờ **cấp TRẬN**, mặc định **bật** cho cả trận chính thức lẫn luyện tập, đổi được từng trận (`QĐ-062`, `QĐ-080`).
- **Câu khép ≠ "đã chấm".** Ở Về đích, cú bấm chấm Sai **mở** cửa sổ cướp quyền 5 giây; câu chỉ khép sau khi cửa sổ đóng **và** người cướp đã được chấm. Công bố sớm là xoá sổ cơ chế cướp quyền.
- Ba ca biên: câu **bị bỏ qua** vẫn công bố · phán quyết **Huỷ kết quả** không tự công bố · đáp án **Chướng ngại vật** theo `GR-012`, nằm ngoài cơ chế này.
- **CẤM đẩy đáp án xuống client trước mốc rồi ẩn bằng một cờ hiển thị.** Đây đúng là lỗi của hệ thống tiền lệ Athena.

### Server-authoritative

Timer, điểm và chuông **chỉ tính ở server**; client chỉ render. **Server time là source of truth duy nhất và là quyết định cuối cùng** cho timeout, thứ tự chuông và thứ hạng tốc độ.

### Nhật ký thao tác

**Nhật ký thao tác mọi thao tác, mọi vai** (`TERM-063`): đăng nhập / đăng xuất / đăng nhập hỏng, CRUD kho đề và bộ đề, quản lý contest, mọi event trong trận, khán giả **bị kick**, xuất/nhập, và **xem đáp án**. Bảng chung, append-only, gồm actor · action · target · timestamp · IP.

Bốn ranh giới, đừng đọc sai:

- **Event trong trận vào bằng THAM CHIẾU, không nhân bản** (`QĐ-132`). `MatchEvent` là bảng riêng và là **nguồn để tính điểm**; nhật ký thao tác trỏ tới nó, **không** chép nội dung sang. Nội dung một event có **đúng một** nguồn sự thật.
- **Khán giả VÀO phòng không ghi** (`QĐ-134`). Kênh public cố ý không có định danh, nên mọi lối giữ dòng đó đều phải bịa một chủ ngữ. Cú **kick** thì ghi — đó là phán quyết của admin, có người bấm.
- **Mỗi YÊU CẦU xem đáp án để lại một dòng mang KẾT QUẢ** *(đã trả · bị từ chối)*, gồm cả lần bị từ chối (`QĐ-133`). Chuỗi yêu cầu bị từ chối liên tiếp là dấu hiệu **dò đề**; không ghi thì nó vô hình.
- **Bảng này có hạn lưu trữ RIÊNG** (`QĐ-131`), không theo `matchPurpose`. *"Append-only"* là ràng buộc lên **bề mặt sản phẩm** — không đường sửa, không đường xoá cho bất kỳ vai nào; bước dọn theo hạn riêng là đường **duy nhất** một dòng rời hệ thống.

Đọc bằng **hai cửa tách bạch** (`QĐ-135`): `PERM-015` `audit.read` cấp **bản cài**, và `PERM-062` cấp **`CONTEST`**.

---

## 5. UX — bắt buộc

Áp cho **mọi** giao diện trong repo: app React lẫn demo tĩnh `public/`.

### Trạng thái và phản hồi

- **Hiển thị trạng thái hệ thống**: mọi thao tác async — submit, save, validate, load — phải có spinner, progress indicator hoặc skeleton. Không để giao diện im lặng khi đang xử lý.
- **Phản hồi tức thì**: toast thành công hoặc thất bại sau mỗi action, không có độ trễ cảm nhận được. Luồng async theo mẫu `loading → success/error` — MUI dùng `Snackbar`/`Alert`, demo tĩnh dùng toast component chung.

### Không chặn gửi lại

Nút action chỉ hiện trạng thái loading, **không disable**. Người dùng gửi lại được; server nhận **bản cuối cùng** trước timeout. Dedup và idempotency là việc của **server** (event log), không phải của giao diện.

> **Ngoại lệ**: khoá **theo luật chơi** — chuông bị khoá khi đã trả lời sai, Ngôi sao hy vọng đã dùng, chưa tới lượt — vẫn disable bình thường. Đó là trạng thái game, không phải chống double-submit.

### Chuông

**Nút chuông chỉ nhận click chuột**; **không** gán phím tắt nào cho chuông, để tránh bấm nhầm khi đang gõ đáp án. Các phím tắt khác giữ nguyên: `Enter` gửi, `1`-`8` chọn hàng ngang.

**Nút *"Mở chướng ngại vật"* được xếp là CHUÔNG** ⇒ cũng chỉ nhận click chuột.

### Dialog xác nhận

Đây là **ngoại lệ có chủ đích** của rule *không chặn gửi lại*: nó chống bấm nhầm một hành động không thu hồi được, không phải chống double-submit.

- **Phía admin**: mọi thao tác không hoàn tác được đều qua dialog Yes/No — mở đáp án, mở ô chữ, xác nhận chọn hàng ngang, xác nhận nút *"Mở chướng ngại vật"* của thí sinh.
- **Phía thí sinh: tức thời, không dialog, không rút lại.** *"Thí sinh cần tốc độ, và tự chịu trách nhiệm sai lầm của mình."* Áp cho mọi thao tác đua tốc độ: chuông, *"Mở chướng ngại vật"*, gửi đáp án.
- **Ngoại lệ duy nhất phía thí sinh — chọn hàng ngang ở mode nhập liệu**: thao tác một chiều, hậu quả nặng, **không** bị ép thời gian ⇒ có dialog xác nhận trên máy thí sinh, xác nhận xong thì khoá nút chọn. Khoá là **tạm**: admin bấm No thì **mở lại**. Dialog này **không thay thế** bước admin duyệt — hai lớp khác mục đích: dialog chống bấm nhầm, admin duyệt là phán quyết.

### Vòng Tăng tốc

Nhận **mọi** lần trả lời cho tới khi hết giờ và tính **bản cuối cùng** — không khoá ô nhập hay nút gửi sau lần trả lời đầu. Xếp hạng theo server-received timestamp của bản cuối.

### Bố cục theo viewport

Thiết kế chủ đích theo kích thước màn hình. Giữ **nội dung chính của mỗi trang trong một viewport** trên khung nhìn tham chiếu của dự án — không thứ gì quan trọng phải scroll mới thấy — nhưng **không nhồi nhét**: giữ breathing room và whitespace dễ đọc. Cân bằng cả hai chiều hỏng:

- **Không ép scroll.** Phần tử quan trọng nằm dưới fold thì sửa layout: bỏ page header trùng breadcrumb, nén hàng stat-card *hero* thành metric strip mỏng, dùng form 2 cột compact thay vì card xếp dọc từng field. **Bảng dài ưu tiên pagination** với page size theo viewport *(~10-12 dòng)* để header + toolbar + rows + pager vừa một màn — đây là cách sửa chính. Table body scroll nội bộ + sticky header + pager ghim là **fallback** khi ranh giới trang bất tiện *(ví dụ ma trận cố định)*. Chỉ giữ scroll cả trang khi không còn cách hợp lý, và phải giải thích được.
- **Không phí không gian, cũng không quá đặc.** Không để mảng trống lớn hoặc thông tin giá trị thấp chiếm chỗ đẹp; cũng không nén chặt đến rối. Ưu tiên thông tin quan trọng với **vai đang xem**.
- **Dashboard** bố cục theo **Z / F reading model**, đưa thông tin liên quan nhất của từng vai lên trước: admin ⇒ điều khiển trận + hàng chờ duyệt đề `DRAFT` → `ACTIVE`; setter ⇒ câu hỏi của mình + trạng thái duyệt; thí sinh ⇒ trạng thái thi + điểm; khán giả ⇒ sân khấu + bảng điểm.

### Điều hướng

Mọi màn có nút **Back** rõ ràng về màn trước hoặc menu. `Esc` = back *(đóng modal trước nếu đang mở)*, `H` = về hub.

> **Ngoại lệ — màn thi đấu của thí sinh**: `Esc` **chỉ xoá ô nhập**, không back. Trình duyệt dùng `Esc` để thoát fullscreen, nên gán back vào đó sẽ làm văng fullscreen giữa trận.

---

## 6. Quy ước code

- **DRY.** Không lặp logic, hằng số hay schema. Zod schema, RuleConfig, permission catalog và socket event contract đều đặt ở `packages/shared` dùng chung frontend lẫn backend; validation viết **một lần** bằng Zod, chạy cả hai đầu. Component, hook hoặc util lặp từ hai lần trở lên phải trích xuất.
- **Mọi timer và mức điểm là RuleConfig** — không hard-code luật trong code hay giao diện.
- **Trim mọi input text ở CẢ frontend lẫn backend** — đề, đáp án, bài làm — tránh khoảng trắng đầu cuối phá so khớp.
- **Chỉ tiếng Việt**, không i18n. **Múi giờ thống nhất UTC+7**: database lưu UTC, mọi hiển thị / log / PDF format theo UTC+7, không có thiết lập múi giờ theo người dùng.
- **String giao diện tách ra file constants** (`vi.ts`), không hard-code trong JSX.
- **Animation là module ĐỘC LẬP với engine và rule.** Engine chỉ emit semantic event; ánh xạ event → animation là config phía client. Sửa rule không đụng animation và ngược lại.
- **Media của thí sinh preload ở dạng MÃ HOÁ qua service worker**; key phát đúng lúc reveal theo server time, fallback reveal-only khi service worker không khả dụng. Màn khán giả và overlay preload URL thường.
- **Âm thanh**: engine emit `sound-cue { slot }`; admin tự upload file cho từng slot — slot trống là im lặng, **không** có bộ SFX mặc định. Client pre-download toàn bộ SFX khi vào phòng.
- **Giới hạn kích thước media là env config**, mỗi loại *(ảnh, video, audio)* một ngưỡng riêng. **Không hard-code con số** ở bất kỳ đâu trong code hay giao diện; thông điệp từ chối phải đọc ngưỡng từ config.
- **Font "Be Vietnam Pro"** + fallback font hệ thống hỗ trợ tiếng Việt *(stack chuẩn ở `public/assets/tokens.css`, biến `--font-sans`)*. App thật **self-host** woff2 trong bundle vì bản portable chạy LAN offline — không dùng CDN; demo tĩnh dùng Google Fonts.
- **Commit theo Conventional Commits, KHÔNG AI attribution.**
