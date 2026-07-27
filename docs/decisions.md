# Sổ quyết định

> **Đây là nơi DUY NHẤT ghi lý do.** Các tài liệu đặc tả (`game-rules.md`, `game-state-machine.md`, `glossary.md`, `traceability.md`) chỉ nói **cái đang là**; khi cần biết **vì sao lại thế**, tra mã `QĐ-*` ở đây.
>
> **Phụ thuộc một chiều**: đặc tả trỏ vào sổ này; sổ này **không** trỏ ngược vào số hiệu của đặc tả. Nhờ vậy đánh số lại đặc tả không làm hỏng sổ, và ngược lại.

## Cách đọc

| Trường | Nghĩa |
|---|---|
| **Quyết định** | Phát biểu chuẩn tắc. Đây là thứ đặc tả phải phản ánh. |
| **Vì sao** | Lý do chọn, và **phương án bị loại** khi có. Phần này tồn tại để lần sau không ai mở lại một cuộc tranh luận đã xong. |
| **Hệ quả** | Những chỗ khác buộc phải theo. |
| **Nguồn** | Luật gốc, phát biểu của chủ dự án, hoặc suy luận từ quyết định khác. |
| **Thay cho** | Mã cũ mà mục này gộp vào. Dùng để truy nguyên tài liệu cũ và `reviews/`. |

**Ba hạng nguồn, không trộn lẫn:**

- **`[LUẬT GỐC]`** — đọc thẳng từ `source/fandom-olympia-26-luat-choi.md`. Không sửa được, chỉ diễn giải.
- **`[CHỦ DỰ ÁN]`** — phát biểu trực tiếp. Sửa được, nhưng phải qua chủ dự án.
- **`[SUY RA]`** — hệ quả bắt buộc của các mục trên. Sửa được bằng lập luận, nếu chỉ ra được chỗ suy luận sai.

**Tiền tố `QĐ-` cố ý khác `Đ-` cũ** — thấy `Đ-` là biết đang đọc tài liệu chưa dọn.

---

# A. Nguyên tắc nền

Sáu mục dưới đây chi phối mọi mục còn lại. Mâu thuẫn với chúng là dấu hiệu mục kia sai, không phải chúng sai.

### QĐ-001 — Máy độc quyền SỰ KIỆN, người độc quyền PHÁN QUYẾT

**Quyết định.** Ba tầng, không chồng vai:

| Tầng | Vai trò | Phương tiện |
|---|---|---|
| **MC** | Thẩm quyền phán quyết **trên sân khấu** | **Nói** |
| **Admin** | **Cảm biến và cơ cấu chấp hành DUY NHẤT** của hệ thống | **Bấm** |
| **Server** | Sự kiện và thời gian — server time, thứ tự chuông, đồng hồ | Không ai sửa được |

Áp cho **mọi** câu hỏi dạng *"ai làm X"*: nếu X là một phán quyết game thì **MC nói, admin bấm**.

**Vì sao.** Máy không quan sát được sân khấu. Mọi cố gắng cho máy suy đoán ý định của con người đều tạo ra một nhánh sai mà không ai gỡ được giữa buổi thi.

**Hệ quả.** Màn `/mc` là **READ-ONLY** — không tạo bề mặt quyền ghi cho MC. Mọi mốc thời gian mà luật gốc mô tả bằng hành vi của MC đều thành một cú bấm của admin (`QĐ-027`).

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `Đ-6.4a`

### QĐ-002 — Hệ thống ADVISORY: máy khuyến nghị, admin quyết

**Quyết định.** Máy **không** tự chấm Đúng/Sai và **không** cưỡng chế thứ tự vòng hay lượt — nó **khuyến nghị** và **cảnh báo**. Admin **luôn ép được** qua dialog Yes/No.

**Vì sao.** Thẩm quyền thuộc ban tổ chức. Một hệ thống cưỡng chế sẽ chặn đúng lúc cần linh hoạt nhất, và người vận hành không có đường vòng.

**Hệ quả.** Vị trí thí sinh và thứ tự lượt riêng Khởi động là **đầu vào của khuyến nghị**, không phải ràng buộc. Conflict luật ⇒ dialog cảnh báo, admin bấm Yes là thực hiện.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `Đ-5`, `Đ-7.1`, `C-15`

### QĐ-003 — Chỉ có BA chỗ chặn cứng

**Quyết định.** Hệ thống chỉ chặn cứng ở **ngưỡng bất khả thi vật lý** — nơi vòng hoặc pha đó không còn nghĩa:

1. Cửa sổ cướp quyền Về đích cần **≥2** thí sinh.
2. Câu hỏi phụ cần **≥2** thí sinh.
3. **Cửa vào vòng thiếu câu** thì không mở được vòng đó (`QĐ-042`).

Mọi lệch luật khác **chỉ cảnh báo**.

**Vì sao.** Ba ngưỡng này không phải lựa chọn thẩm mỹ: dưới ngưỡng thì thao tác không có đối tượng. Ngưỡng thứ ba là chỗ **ghi đè tiền lệ Athena** — bản cũ chỉ cảnh báo khi thiếu đề, và hậu quả là một vòng chạy dở rồi tắc.

**Hệ quả.** Ngưỡng thứ ba **không ép được**, nên phải có lối thoát: `QĐ-043` cho admin bổ sung câu tại cửa vào vòng.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `Đ-15.3`, `Đ-31`

### QĐ-004 — Thao tác ở INVALID STATE là nhánh KHÔNG TỒN TẠI

**Quyết định.** Một thao tác không hợp lệ **cho đối tượng ở trạng thái đó** thì nút **không bật**; nếu tín hiệu vẫn tới được thì server từ chối và admin thấy **toast**. Đây **không phải** chặn cứng và **không ép được**.

**Vì sao.** Hai thứ trông giống nhau nhưng khác hẳn về quyền: *chặn cứng* là **ngưỡng tài nguyên** (số người, số câu) — có thể tưởng tượng việc nới ra; *invalid state* là **thao tác không có đối tượng** — nới ra thì không có nghĩa gì. Trộn hai hạng sẽ khiến con số ba của `QĐ-003` trôi dần.

**Hệ quả.** Ba lớp phản hồi tách bạch: **toast** (không ép được) · **dialog cảnh báo** (ép được) · **dialog xác nhận** (chống bấm nhầm).

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `Đ-16`

### QĐ-005 — Chống bấm nhầm chỉ ở phía ADMIN

**Quyết định.**

- **Phía thí sinh**: tức thời, **không dialog**, không rút lại — *"thí sinh cần tốc độ, và tự chịu trách nhiệm sai lầm của mình"*.
- **Phía admin**: mọi thao tác **không hoàn tác được** đều qua **dialog Yes/No**.
- **Ngoại lệ duy nhất phía thí sinh**: chọn hàng ngang ở mode nhập liệu (`QĐ-019`) — thao tác một chiều, hậu quả nặng, **không bị ép thời gian**.

**Vì sao.** Dialog tốn giây. Ở chỗ đua tốc độ, một giây là kết quả trận. Ở chỗ không đua tốc độ, một cú bấm nhầm không thu hồi được thì đắt hơn nhiều.

**Hệ quả.** Lỗi bấm nhầm của thí sinh được sửa bằng **admin bấm No** ở hàng đợi (`QĐ-021`), không phải bằng nút "huỷ" phía thí sinh.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `Đ-4.3`, `Đ-7` (vế dialog), `Đ-36` (vế ngoại lệ)

### QĐ-006 — Mốc do admin bấm là TUYỆT ĐỐI

**Quyết định.** **Không** cửa sổ ân hạn, **không** trừ bù độ trễ tay người. Thời gian do server quyết định.

**Vì sao.** Grace kéo theo ba khoản nợ: một con số ma không căn cứ, một **biên mới** (bấm đúng mép ân hạn là trong hay ngoài), và nó làm mờ nguyên tắc *"server time là quyết định cuối cùng"*.

**Hệ quả.** Van thoát **không phải** grace mà là **sửa được sau**: lịch sử giữ đầy đủ để admin xem lại và can thiệp.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `Đ-6.3`

---

# B. Phạm vi phiên bản

### QĐ-007 — v1 đặc tả LUẬT cho đúng 4 thí sinh; schema và UI làm cho 1-12

**Quyết định.** Luật v1 chỉ viết cho **đúng 4 thí sinh** — luật gốc O26 viết cho 4 người, và hệ thống **không phát minh luật** cho số ghế khác. Nhưng **schema, seat model, `scoringUnit`, RuleConfig dạng mảng và giao diện** làm cho **1-12 người ngay từ v1**. v1.5 chỉ ship **luật + sửa controller**, không migrate schema, không dựng lại UI.

**Vì sao.** Phát minh luật cho số ghế khác là bịa requirement. Nhưng để dành schema cho phiên bản sau thì phải migrate — đắt hơn nhiều so với làm đủ ngay.

**Hệ quả.** Cấu hình 4 ghế mà **runtime tụt còn 3** (rớt quá grace, bị loại, bỏ cuộc) là tình huống của **v1**, không hoãn được: hệ thống cảnh báo, **admin quyết** (`QĐ-002`).

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `C-7`, `GRR-129`

### QĐ-008 — Mỗi contest có đúng MỘT admin

**Quyết định.** Không có mô hình nhiều admin đồng thời trên một contest.

**Vì sao.** Bỏ hẳn một lớp vấn đề: tranh chấp phán quyết, khoá đồng thời, thứ tự ghi. Hồ sơ triển khai là một buổi thi có một bàn điều khiển.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `Đ-18`

### QĐ-009 — v1 luôn có người điều khiển

**Quyết định.** Không có luồng v1 nào chạy mà không có admin.

**Hệ quả.** Rủi ro *"không ai chấm"* dồn hết sang tính năng luyện tập của v1.5, nơi nó được giải bằng phạm vi khác (`QĐ-040`).

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `C-14`

---

# C. Điểm và event log

### QĐ-010 — Máy KHÔNG phán xử đáp án

**Quyết định.** Với câu gõ, máy **chỉ highlight ký tự khác** giữa bài làm và đáp án. Admin tự đánh giá chính tả và ý nghĩa tương đồng. **Không viết đường code nào tự cộng/trừ điểm từ so khớp.**

**Vì sao.** Ban tổ chức quyết định đúng/sai.

**Hệ quả.** Mâu thuẫn *"chính tả nghiêm ngặt vs normalize"* **tan** — không có nhánh máy nào tự quyết, nên normalize và highlight chỉ là **trợ giúp hiển thị**. Bỏ dấu khi so khớp nghĩa là *"đừng tô đỏ chỗ khác dấu"*, **không** nghĩa là *"công nhận đúng"*.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `Đ-1`, `C-12`, `K-6`

### QĐ-011 — Điểm là hàm của event log; hoàn nguyên là THÊM event

**Quyết định.** Điểm không phải một con số bị sửa trực tiếp — nó là `reduce(event log)`. Hoàn nguyên = **thêm event đảo ngược**, như `git revert`, **không phải** `git reset --hard`. Lịch sử **linear, append-only, không bao giờ xoá**.

**Vì sao.** Với event log, hoàn nguyên một event ở giữa là well-defined. Ràng buộc cũ *"undo chỉ event chấm gần nhất"* tồn tại vì điểm từng là số bị sửa trực tiếp — nay không cần nữa.

**Hệ quả.** Bỏ vòng VCNV sinh event đảo ngược **chỉ cho event điểm của VCNV**; điểm Tăng tốc giữ nguyên. Không có khái niệm *"quay về snapshot mốc vòng"*.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `Đ-5.3`, `Đ-5.3.X`, `R-GEN-07` (bị thay thế), `GRR-073` (mất đối tượng)

### QĐ-012 — Điểm được phép ÂM

**Quyết định.** Không có sàn điểm. Điểm âm tham gia bình thường vào xếp lượt Về đích, điều kiện hoà, và hiển thị.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `Đ-2`

### QĐ-013 — Độ hạt của một event điểm: hai hình dạng, không tranh nhau

**Quyết định.** Hai hình dạng, dùng ở hai chỗ:

| | Hình dạng | Dùng ở |
|---|---|---|
| **E1** | Một event cho **một** người; chống trùng theo `(câu, thí sinh, loại phán quyết)` | Mọi vòng **trừ** Tăng tốc |
| **E2** | Một event cho **cả bảng** | **Tăng tốc** |

**Vì sao.** Điểm Tăng tốc là hàm của **thứ hạng**, mà thứ hạng là quan hệ **giữa những người được chấm Đúng** — điểm của A phụ thuộc phán quyết dành cho B. Tách thành bốn event độc lập thì mỗi event không tự đứng được, và replay từng phần cho ra bảng sai.

**Hệ quả.** Không cần khoá chống trùng dùng chung cho E1 và E2 — vì `QĐ-014` bỏ hẳn thao tác sửa từng phần một event E2. Hai hình dạng **không bao giờ phải nói chuyện với nhau**.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `Đ-5.3.1`, `GRR-071`, `TERM-026` (hết `CONFLICT`)

### QĐ-014 — Một câu đi qua ĐÚNG MỘT phán quyết; sai thì admin cộng tay

**Quyết định.** Sau khi admin chấm, nút chấm **khoá** và nút *"Câu kế tiếp"* hiện lên. **Không có** bấm lại, **không có** đổi phán quyết tại chỗ, **không có** cơ chế tính lại thứ hạng. Sửa một phán quyết đã chốt đi qua **điều chỉnh điểm thủ công**, mỗi ghế một event kèm lý do bắt buộc.

**Vì sao.** Hệ thống không quan tâm tới sai lầm — nó chỉ cần **một** cơ chế sửa sai đơn giản. Một cơ chế tính lại tự động sẽ là đường code duy nhất tự sinh điểm mà không qua phán quyết của người.

**Hệ quả.** Ở Tăng tốc, admin **tự tính delta** kể cả phần dây chuyền — một ghế từ Sai thành Đúng thì những người xếp sau tụt một bậc, có thể phải sửa tới bốn ghế cho một lỗi. Đây là **cái giá được chấp nhận có chủ ý**: đổi lấy một mô hình mà người vận hành giữ trọn trong đầu.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `Đ-17`, `Đ-56`

### QĐ-015 — Điểm số là thông tin CÔNG KHAI với mọi vai

**Quyết định.** Bảng điểm hiển thị cho viewer, overlay, thí sinh, MC, admin. Chỉ **đáp án** mới bị giới hạn (`QĐ-051`).

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `C-20`

---

# D. Hai mode trả lời

### QĐ-016 — Hai mode; SÂN KHẤU là mặc định và là mode luật được đặc tả theo

**Quyết định.**

| Mode | Nội dung |
|---|---|
| **Sân khấu** *(mặc định)* | Thí sinh **đọc** đáp án; máy chỉ dùng để **giành quyền trả lời** |
| **Nhập liệu** | Thí sinh **gõ** đáp án |

Luật được viết theo **mode sân khấu**; mode nhập liệu là **biến thể**.

**Hệ quả ngược trực giác.** Mode sân khấu **không** làm nhẹ yêu cầu phần cứng — VCNV và Tăng tốc vẫn buộc gõ máy (`QĐ-018`), nên mỗi thí sinh vẫn cần thiết bị nhập liệu đầy đủ.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `Đ-4`, `Đ-4.1`, `C-13`

### QĐ-017 — Mode đặt ở cấp CONTEST, một giá trị chung cho mọi vòng

**Quyết định.** Một contest **không trộn** hai mode.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `Đ-4.a2`

### QĐ-018 — VCNV và Tăng tốc LUÔN gõ máy

**Quyết định.** Ở **Tăng tốc**, không có mode. Ở **VCNV**, đáp án hàng ngang và câu ô trung tâm **luôn gõ máy**; mode chỉ đổi **cách chọn hàng ngang**, còn đáp án Chướng ngại vật thì **theo mode**.

**Vì sao.** Luật gốc nói thẳng *"các thí sinh trả lời bằng máy tính"* ở hai vòng này. Phát biểu đó **được giữ nguyên, không bị ghi đè** — không có mâu thuẫn với nguồn.

*Nguồn*: `[LUẬT GỐC]` · *Thay cho*: `Đ-4.b`, `Đ-4.X2`, `Đ-14`

### QĐ-019 — Chủ thể thao tác đi theo MODE, một đường vào cho mỗi mode

**Quyết định.** Áp cho **chọn hàng ngang**, **chọn gói Về đích**, và **đặt Ngôi sao hy vọng**:

| Mode | Ai thao tác |
|---|---|
| **Sân khấu** | Thí sinh **nói miệng**, **admin bấm**. Máy thí sinh **không render** nút đó |
| **Nhập liệu** | **Thí sinh** tự bấm; admin **không** chọn thay |

**Vì sao.** Hai đường vào song song cho cùng một thao tác tạo ra tranh chấp không có trọng tài. Một mode, một đường.

**Hai ngoại lệ có chủ ý.**
1. **Chọn gói Về đích, mode nhập liệu** vẫn giữ fallback admin chọn hộ `20/20/20` khi thí sinh không chọn kịp — luật cần một đường thoát để lượt thi không tắc, còn chọn hàng ngang thì không có mốc "quá hạn" tương đương.
2. **Chọn hàng ngang, mode nhập liệu** có **dialog xác nhận trên máy thí sinh** — ngoại lệ duy nhất của `QĐ-005`. Xác nhận xong thì khoá nút chọn; khoá là **tạm**, admin bấm No thì mở lại. Dialog này **không thay thế** bước admin duyệt: dialog chống bấm nhầm, admin duyệt là phán quyết.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `Đ-36`, `Đ-40`, `Đ-41`

---

# E. Tín hiệu và hàng đợi

### QĐ-020 — Mọi tín hiệu vào hàng đợi; KHÔNG có cơ chế drop

**Quyết định.** Mọi tín hiệu của thí sinh vào hàng đợi theo **server timestamp**. Khái niệm *"drop tín hiệu"* đã bị loại khỏi hệ thống. **Hàng đợi đang hoạt động** reset sau mỗi **vòng** rồi tái sử dụng; **lịch sử tín hiệu KHÔNG BAO GIỜ xoá**.

**Vì sao.** Lịch sử đầy đủ là van thoát thay cho grace (`QĐ-006`) — admin xem lại được và can thiệp được khi có sự cố.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `Đ-7`, `Đ-7.a`, `Đ-7.b`

### QĐ-021 — Hàng đợi chỉ CHẶN ở VCNV

**Quyết định.**

| Vòng · tín hiệu | Chặn |
|---|---|
| **VCNV** — chọn hàng ngang, *"Mở chướng ngại vật"* | **CÓ** — admin duyệt Yes mới có hiệu lực |
| Khởi động lượt chung — chuông | Không |
| Về đích — cướp quyền 5 giây | Không |
| Câu hỏi phụ — chuông | Không |

**Tiêu chí.** Tín hiệu **một chiều, hậu quả nặng, không bị ép thời gian** ⇒ chặn. Tín hiệu **đua tốc độ, cửa sổ chặt** ⇒ không chặn, server phân xử ngay; hàng đợi chỉ là lưới an toàn.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `Đ-7.2`

### QĐ-022 — Từ chối một tín hiệu KHÔNG làm thí sinh mất lượt

**Quyết định.** Admin bấm No ⇒ tín hiệu kế tiếp lên, và ghế bị từ chối **giữ nguyên lượt** của mình. Ở mode nhập liệu, nút chọn của ghế đó **mở lại**.

**Vì sao.** Đây là cơ chế sửa lỗi bấm nhầm của thí sinh (`QĐ-005`). Nếu reject làm mất lượt thì nó thành hình phạt, và admin sẽ ngại dùng.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `Đ-7` (vế reject), `GRR-143`

### QĐ-023 — Chuông chỉ nhận click chuột và tự khoá ngay khi bấm

**Quyết định.** **Không gán hotkey** cho chuông — tránh bấm nhầm khi đang gõ đáp án. Nút tự khoá **ở frontend, trước khi gửi**, gỡ khoá khi sang câu mới. Nút *"Mở chướng ngại vật"* của VCNV **được xếp là chuông** ⇒ cũng chỉ nhận click, cũng tự khoá, nên mỗi ghế chỉ phát **một** tín hiệu cho cả vòng.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `Đ-24`, `Đ-4.3` (vế chuông)

### QĐ-024 — Tín hiệu đến sau khi đã có người giành quyền: ghi nhận nhưng TRƠ

**Quyết định.** Tín hiệu vẫn vào lịch sử kèm timestamp, nhưng **không** đổi người giữ quyền và **không** sinh hệ quả nào.

**Vì sao.** *Trơ* khác *drop*: drop là mất dấu, trơ là có dấu mà không có hiệu lực. Lịch sử trơ chính là căn cứ để admin can thiệp khi có tranh chấp.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `Đ-25`

### QĐ-025 — Hai tín hiệu cùng mốc thời gian: hàng đợi tự quyết

**Quyết định.** Bằng nhau tới **millisecond** thì thứ tự do hàng đợi quyết định, không có quy tắc phá hoà nào thêm.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `Đ-23`

### QĐ-026 — Đặt Ngôi sao hy vọng ở mode nhập liệu KHÔNG cần admin duyệt

**Quyết định.** Tín hiệu có hiệu lực **ngay** khi tới — Về đích là vòng hàng đợi không chặn (`QĐ-021`).

**Vì sao.** Cụm *"admin duyệt Yes/No"* trong tài liệu cũ là **lỗi diễn đạt** mô tả **mode sân khấu**, nơi admin **là người bấm** nên khái niệm *"duyệt tín hiệu"* không tồn tại. Về sau bị đọc thành *"admin duyệt tín hiệu của thí sinh"*, tạo ra một mâu thuẫn không có thật.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `Đ-54`

---

# F. Đồng hồ và mốc thời gian

### QĐ-027 — Mọi mốc của MC ánh xạ thành MỘT cú bấm của admin

**Quyết định.**

| Mốc trong luật gốc | Sự kiện hệ thống |
|---|---|
| *"MC đọc xong câu hỏi"* | Admin bấm **start timer** |
| *"hiệu lệnh của người dẫn chương trình"* (Câu hỏi phụ) | Admin **bấm** — admin là người **nghe** hiệu lệnh |
| *"câu hỏi được đọc lên **hoặc** hiện lên màn hình"* (đóng cửa sổ NSHV) | Admin bấm **hiển thị câu hỏi** — mốc đến trước |
| *"MC công bố đáp án"* (mốc cắt Khởi động) | Admin **bấm** |

**Vì sao.** Hệ quả trực tiếp của `QĐ-001`: máy không quan sát được sân khấu, admin là cảm biến.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `Đ-6`, `Đ-6.1`, `Đ-33`, `GRR-048`

### QĐ-028 — "Hiển thị câu hỏi" và "start timer" là HAI thao tác, thứ tự cố định

**Quyết định.** Không gộp được. Hiển thị trước, start timer sau.

**Vì sao.** Gộp lại là **phá luật**: khoảng giữa hai mốc chính là lúc MC đọc, và hai thứ sống trong khoảng đó — cửa sổ chuông của Khởi động lượt chung, và mốc đóng cửa sổ Ngôi sao hy vọng. Gộp thì cửa sổ chuông co lại đúng bằng thời gian suy nghĩ, còn cửa sổ NSHV mất mốc đóng.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `Đ-26`

### QĐ-029 — Biên thời gian là biên ĐÓNG

**Quyết định.** Tới **đúng** mốc hạn là **hợp lệ**. Bản quá hạn **không bị máy loại thẳng** — nó vào lịch sử màn admin, **tô đỏ**, admin quyết.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `Đ-28`

### QĐ-030 — Đồng hồ khoá THÍ SINH, không khoá ADMIN; và không bao giờ đóng băng

**Quyết định.** Hết giờ thì ô nhập của thí sinh khoá, nút chấm của admin **không** khoá. Một cửa sổ thời gian đã mở thì **chạy hết theo server time** — hệ thống không tự dừng ở bất kỳ ngưỡng nào, kể cả khi có người mất kết nối, và **không thao tác nào đóng băng đồng hồ**.

**Hai ngoại lệ tường minh của vế thứ nhất.** Nút **start timer** tự khoá sau lần bấm đầu; và ở vòng thí sinh **gõ đáp án**, nút **chấm** khoá tới khi hết giờ — tránh chấm khi thí sinh còn đang sửa.

**Phân biệt KHÉP với ĐÓNG BĂNG.** Bất biến này cấm **đóng băng** — giữ đồng hồ lại rồi thả ra, vì thời gian đã trôi thì không lấy lại được. Nó **không** cấm một cửa sổ **kết thúc sớm** khi lý do tồn tại của nó đã hết (`QĐ-031`, `QĐ-034`).

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `Đ-20`, `Đ-21`, `Đ-35` (vế nút chấm)

### QĐ-031 — Tín hiệu KHÔNG GIÀNH QUYỀN không đụng đồng hồ; tín hiệu GIÀNH QUYỀN thì khép cửa sổ

**Quyết định.**

| Loại tín hiệu | Tác động lên đồng hồ |
|---|---|
| **Không giành quyền** — bài làm VCNV, *"Mở chướng ngại vật"* | **Không đụng gì.** Thứ bị hoãn là **việc HIỂN THỊ**, không phải thời gian |
| **Giành quyền, Khởi động lượt chung** | **Thay** bằng đồng hồ mới: 3 giây tính từ mốc giành được quyền |
| **Giành quyền, Câu hỏi phụ** | **Khép** cửa sổ 15 giây ngay tại timestamp của tín hiệu; **không** chạy tiếp, **không** có đồng hồ thay thế |

**Vì sao vế thứ ba.** Luật đã chốt *trả lời sai ⇒ cả nhóm sang câu kế*, nên sau khi một người giành quyền thì **không còn ai được bấm nữa** — đếm tiếp là đếm cho một cửa sổ đã hết đối tượng.

**Phương án bị loại.** Tiền lệ Athena *dừng rồi chạy tiếp từ chỗ dừng*. Nó **đúng với luật của Athena** — ở đó câu **vẫn mở** cho những người còn lại thử tiếp. KnowledgeDome chọn luật ngược theo đúng luật gốc, nên hành vi đồng hồ buộc phải khác.

**Không có đồng hồ trả lời riêng ở Câu hỏi phụ.** Luật gốc **im lặng có chủ ý**: nó nói rõ *"tính từ lúc giành được quyền"* ở Khởi động lượt chung và *"suy nghĩ **và trả lời**"* ở Về đích, nhưng ở Câu hỏi phụ chỉ ghi *"Thời gian suy nghĩ cho mỗi câu hỏi là 15 giây"* — và **không** có chế tài cho việc bấm chuông rồi im lặng. Ba chỗ vắng cùng lúc không phải sót. Bấm rồi im ⇒ admin chấm Sai ⇒ sang câu kế.

*Nguồn*: `[LUẬT GỐC]` + `[CHỦ DỰ ÁN]` · *Thay cho*: `Đ-22`, `Đ-53`

---

# G. Điều khiển trận

### QĐ-032 — `LOBBY` là CỬA VÀO VÒNG; enum cấp trận có 4 giá trị

**Quyết định.** Trạng thái nghỉ của trận có **một** giá trị duy nhất, dùng **cả trước vòng đầu tiên lẫn giữa hai vòng**. Enum cấp trận: `LOBBY` · *vòng đang chạy* · `TIE_BREAK` · `FINISHED`.

**Vì sao.** Hai trạng thái nghỉ cũ khác nhau đúng **một** điều: lần mở vòng đầu tiên trong đời một trận kèm **đóng băng cấu hình**. Khác biệt nằm trên **một cạnh**, không nằm ở **bản thân trạng thái** — và điều kiện *"chưa vòng nào từng chạy"* suy ra được từ event log, không cần cờ lưu trữ.

**Hệ quả.** Nội dung trình diễn giữa hai vòng (giao lưu, giải lao, video hình hiệu) **không cần** trạng thái riêng — nó là **lớp phủ** admin bật/tắt.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `Đ-38`

### QĐ-033 — Pipeline vòng ĐỘC LẬP

**Quyết định.** Mở một vòng ⇒ vòng đó bắt đầu **sạch**: mọi cờ phạm vi vòng được đặt lại. Một vòng chỉ được đọc **ba** thứ: RuleConfig đã đóng băng · danh sách câu đã gán · **bảng điểm hiện tại**. **Cấm mọi phụ thuộc ẩn** giữa các vòng. Mọi ràng buộc *"sau vòng X"* đọc thành *"tại thời điểm MỞ vòng Y"*.

**Vì sao — đây là quyết định về ĐỘ TIN CẬY, không phải tiện dụng.** Hồ sơ triển khai là **portable LAN, không có kỹ sư trực**. Thứ cứu được một buổi thi đang hỏng là mô hình mà người vận hành **giữ trọn trong đầu**: *"sửa điểm, mở lại vòng"*. Mọi trạng thái ẩn cần dọn thêm đều là một cách để buổi thi hỏng.

**Bằng chứng thực địa.** Lỗi duy nhất trong `bug.txt` của Athena chính là vi phạm nguyên tắc này — chạy lại vòng Về đích mà trạng thái cửa sổ cướp quyền của lần chạy trước không được dọn ⇒ treo toàn bộ nút giành quyền.

**Hệ quả.** Bỏ Tăng tốc hay bỏ Về đích **không làm mất tiền đề** của vòng sau. Cờ Ngôi sao hy vọng cũng theo phạm vi vòng — chạy lại Về đích thì ngôi sao **hồi sinh**, vì nếu không thì chạy lại vòng không khôi phục được tình trạng ban đầu và thí sinh mất quyền vì lỗi hệ thống chứ không phải vì lựa chọn của mình.

**Ngoại lệ duy nhất** là cờ **hành chính** của admin — `QĐ-047` — vì nó không do vòng nào sinh ra.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `Đ-39`, `Đ-42`, `Đ-7.3`, `Đ-5.1d`, `Đ-5.1e`, `GRR-115` → `GRR-117`

### QĐ-034 — Mọi vòng vào và ra qua `LOBBY`; có cửa ra khẩn cấp giữ điểm

**Quyết định.** Không có đường vòng → vòng. Một vòng có **bốn** cửa ra, tất cả đều về `LOBBY`:

| Cửa ra | Điểm |
|---|---|
| Kết thúc vòng (đã đủ câu) | giữ |
| **Kết thúc vòng khẩn cấp** — bấm được cả khi **chưa đủ câu** | **giữ nguyên** |
| Bỏ vòng | revert |
| Chạy lại vòng | revert rồi chạy mới |

**Vì sao cần cửa ra khẩn cấp.** Nếu bắt mọi vòng ra qua `LOBBY` mà chỉ có ba cửa cũ thì admin đang ở giữa một vòng hỏng bị khoá lại: *kết thúc vòng* đòi đủ câu, còn *bỏ* và *chạy lại* đều **mất điểm**. Cửa thứ tư là chỗ giữ được điểm đã ghi.

**Hệ quả.** Câu đang mở khép bằng **Huỷ kết quả** — không sinh điểm cho ai, và **không** ép mọi ghế thành SAI: cú bấm *"kết thúc khẩn cấp"* phát biểu *"vòng này hỏng"*, ép thành SAI ở đó là tự chấm trá hình. Biên bản gắn nhãn **"kết thúc sớm"**.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `Đ-46`

### QĐ-035 — Bỏ vòng và chạy lại vòng

**Quyết định.** Admin được bỏ hẳn hoặc chạy lại một vòng, với điều kiện kho đề còn câu.

| Khía cạnh | Quyết định |
|---|---|
| Điểm | **Revert** các event điểm của vòng đó |
| Biên bản | **Giữ đầy đủ**; vòng bị bỏ hiện kèm nhãn *"đã bỏ"* |
| Viewer / overlay | Thấy số **đột ngột thay đổi** — không hiệu ứng, không thông báo. Có chủ đích |
| Câu đã dùng | **KHÔNG** trả lại kho (`QĐ-044`) |
| Điều chỉnh điểm thủ công của admin | **KHÔNG** tự revert — đó là phán quyết của người, không thuộc vòng nào |
| Vòng đang chạy vs đã kết thúc | **Cùng quy tắc**, chỉ khác kích thước tập event |

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `Đ-5.1`, `Đ-5.2`, `Đ-11.B`

### QĐ-036 — Trận KHÔNG tự đóng sổ: `LOBBY` giữ trận cho tới khi admin bấm

**Quyết định.** Hết playlist chỉ làm hệ thống **gợi ý** chốt. Trận nằm ở `LOBBY` bao lâu tuỳ admin. Phép tính điều kiện hoà chạy **tại cú bấm "Chốt trận"**, không phải lúc vòng cuối kết thúc.

**Vì sao.** Trước quyết định này, hai cạnh rời `LOBBY` bật **tự động** ⇒ khoảnh khắc vòng cuối khép lại là thứ hạng đã chốt. Điều đó **mâu thuẫn với chính quyền sửa điểm ở `LOBBY`**: cửa sổ để sửa không tồn tại trên thực tế. Nay cửa sổ đó là cả khoảng trận đứng ở `LOBBY`, và điểm sửa **được tính vào** phép phân định hoà — sửa điểm có thể **tạo ra** hoặc **gỡ** một nhóm hoà.

**Hệ quả.** Hộp thoại chốt trận **phải nói rõ hệ quả** — *"chốt xong sẽ không sửa được điểm nữa"* (`QĐ-037`). Mở công bố kết quả **trước** khi chốt vẫn hợp lệ, nhưng bảng đó là **tạm thời**.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `Đ-48`

### QĐ-037 — `FINISHED` là terminal và NIÊM PHONG

**Quyết định.** Vào tới `FINISHED` thì bảng điểm **không đổi được nữa**: không điều chỉnh điểm, không chạy lại vòng, không sửa phán quyết. Trạng thái này **chỉ đọc**.

**Vì sao.** Đây là **nửa còn lại** của `QĐ-036`, không phải một ràng buộc rời. `QĐ-036` mở cửa sổ sửa điểm; mục này đóng nó tại mốc chốt. Có cả hai thì mô hình gọn đúng một câu: **sửa trước khi chốt; chốt rồi thì hết.** Đó cũng là thứ làm cú bấm chốt trận có sức nặng thật.

**Hệ quả.** Van thoát sau khi chốt còn đúng **một** cái và nó không đụng vào trận cũ: **tạo trận mới** (`QĐ-039`). Biên bản trận cũ giữ nguyên từng dòng — muốn ghi nhận rằng nó sai thì ghi ở trận mới, không sửa ngược quá khứ.

**Phương án bị loại.** `GRR-120` đề xuất cho phép **chạy lại vòng sau `FINISHED`** — **bác**, vì chạy lại vòng làm đổi điểm. Nhu cầu đằng sau nó được đáp ứng sớm hơn ở `LOBBY` và muộn hơn ở trận mới.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `Đ-51`, `GRR-120` (bác)

### QĐ-038 — Trận bỏ dở đóng sổ bằng NHÃN, không phải bằng trạng thái thứ năm

**Quyết định.** Admin bấm *"Huỷ trận"* ⇒ trận về `FINISHED` với `matchClosedReason = "bỏ dở"`. **Điểm giữ nguyên**, **không phân định thứ hạng, không có người thắng**. Enum cấp trận **giữ 4 giá trị**.

**Vì sao.** Thứ cần phân biệt *hoàn thành* với *bỏ dở* **không phải engine** — không transition nào rẽ nhánh theo nó, không luật chơi nào đọc nó. Thứ cần nó là **biên bản, thống kê và retention**. Đó đúng là hạng của một **thuộc tính đóng sổ**, không phải của một trạng thái. Đề xuất thêm state `ABANDONED` bị **bác ở dạng state, giữ ở dạng nhãn**.

**Trường đi kèm.** `matchClosedReason` (`hoàn thành` | `bỏ dở`) · `closedBy` · `closedAt` · `reason` (**bắt buộc** khi bỏ dở).

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `Đ-47`, `GRR-077` vế 2

### QĐ-039 — Contest là BẢN THIẾT KẾ, Match là MỘT LẦN CHẠY

**Quyết định.** Một contest chứa nhiều trận. Nút *"Bắt đầu trận mới"* truy cập được **sau khi** trận trước đã đóng sổ, với điều kiện **kho đề còn đủ** cho pre-flight.

**Đây KHÔNG phải transition rời `FINISHED`.** Nó **khởi tạo một thể hiện máy trạng thái mới**; trận cũ ở nguyên `FINISHED` vĩnh viễn, biên bản không đổi một dòng. Vẽ nó thành cạnh `FINISHED → LOBBY` sẽ làm `FINISHED` mất tính terminal mà chẳng được gì — thứ đổi là **trận nào đang chạy**, thuộc cấp **contest**.

**Ranh giới.**

| Thuộc CONTEST | Thuộc TRẬN |
|---|---|
| Mã phòng 6 số · kho đề · cấu hình gốc · cờ **no-repeat** | Điểm · event log · ghế đã gán · biên bản · cấu hình **đã đóng băng** |

**Hệ quả.** Viewer/overlay **không phải join lại** giữa hai trận. Trận mới **chưa đóng băng cấu hình** — nó đọc cấu hình contest **hiện tại** rồi đóng băng của riêng nó, nên sửa RuleConfig **giữa hai trận** là hợp lệ và không đụng trận đã chạy.

**Ràng buộc suy ra.** Một contest chỉ có **một trận đang chạy** tại một thời điểm — mã phòng thuộc contest, nên hai trận song song sẽ đụng nhau ở cùng một phòng.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `Đ-49`, `C-9`

### QĐ-040 — Tách PRACTICE CONTEST khỏi CONTEST THẬT

**Quyết định.** Hai tầng cờ:

| Tầng | Cờ | Dùng để |
|---|---|---|
| **Contest** | `contestPurpose: official \| practice` | Phân biệt contest thật với practice contest |
| **Match** | `matchPurpose: official \| practice` | Chọn phép lọc kho đề, `revealAnswerAfterJudge` mặc định, retention |

Trong một **contest thật**, trận `practice` **chỉ được gán câu ĐÃ HIỂN THỊ** ở các trận thật trước đó.

**Vì sao — lý do chính là CHỐNG LỘ ĐỀ.** Hệ quả trực tiếp: **không thể dùng contest thật để tổng duyệt TRƯỚC trận**, vì pool luyện tập chỉ gồm câu đã lộ. Đây là **cơ chế**, không phải kỷ luật con người. Muốn tổng duyệt thật thì tạo practice contest riêng.

**Lợi ích thứ hai.** Câu hỏi cũ *"trận practice có tiêu kho đề không"* có hai đáp án đều xấu — **có tiêu** thì một buổi tổng duyệt đốt sạch kho đề trận thật; **không tiêu** thì `QĐ-044` phải có ngoại lệ. Quyết định này **né cả hai**: mọi câu trận practice chạm **đã tiêu rồi**, nên nó **không thể tiêu thêm** — không phải vì được miễn, mà vì không còn gì để tiêu.

**Hệ quả — phép lọc kho đề ĐẢO CHIỀU.** Cùng một nút tạo trận, hai phép kiểm đối nhau; đây là chỗ dễ hiện thực nhầm nhất:

| `matchPurpose` | Pool gán được |
|---|---|
| `official` | Câu **CHƯA** hiển thị |
| `practice` (trong contest thật) | **CHỈ** câu **ĐÃ** hiển thị |

**Vế *"contest thật phải chạy ít nhất một lần mới practice được"* là HỆ QUẢ, không phải guard riêng**: contest chưa chạy thì pool đã-dùng rỗng, pre-flight tự chặn. Ghi ở dạng hệ quả vì dạng đó xử đúng luôn ca trận **bỏ dở** — nó đã lộ vài câu nên vẫn cho ra pool hợp lệ.

**Practice contest không cần luật mới**: phạm vi no-repeat vốn theo từng contest, nên nó tự tách khỏi mọi contest thật.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `Đ-57`, `U-31`

---

# H. Kho đề

### QĐ-041 — Luật cho bao nhiêu câu thì đúng bấy nhiêu

**Quyết định.** Hệ thống **không bao giờ tự sinh câu thứ N+1**. Số câu của một vòng là con số cố định của luật.

**Hệ quả.** Biên *"lớn hơn max"* của mọi vòng **không tồn tại**, và **"kho đề cạn giữa vòng" không tồn tại** — nhu cầu của vòng đã được kiểm đủ tại cửa vào. Nút *"Câu kế tiếp"* ở câu cuối cùng đã chuyển dạng thành *"Kết thúc lượt"*.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `Đ-30`

### QĐ-042 — Kho đề kiểm tại CỬA VÀO TỪNG VÒNG

**Quyết định.** Thiếu câu cho một vòng thì **không mở được vòng đó** — chặn cứng thứ ba của `QĐ-003`, **không ép được**.

**Vì sao.** Kiểm ở cửa vào từng vòng thay vì một lần lúc bắt đầu trận: admin đổi thứ tự vòng được (`QĐ-002`), nên không có "danh sách vòng sẽ chạy" để kiểm trước.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `Đ-31`

### QĐ-043 — Danh sách câu đã gán sửa được tại CỬA VÀO VÒNG

**Quyết định.** Ở `LOBBY`, admin thêm/bớt câu và sửa thông tin câu trong danh sách đã gán. Pre-flight chạy lại sau khi sửa. **Không** gỡ được câu **đã hiển thị**.

**Vì sao.** Đây là **lối thoát duy nhất** của ngưỡng chặn cứng `QĐ-042` — không có nó thì một vòng thiếu câu là mất hẳn. Còn cấm gỡ câu đã hiển thị là để bịt đường lách: gỡ rồi thêm lại sẽ vô hiệu hoá `QĐ-044`.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `Đ-37`, `C-18`

### QĐ-044 — Câu ĐÃ HIỂN THỊ thì không bao giờ trả lại kho

**Quyết định.** *"Đã dùng"* = **đã hiển thị cho thí sinh**, không phải *"đã chấm"*. Câu bị bỏ qua, câu của vòng bị bỏ, câu của hàng ngang chưa hỏi khi cả sân bị loại — **đều tiêu**. Ngược lại, câu **đã rút nhưng chưa hiển thị** là **chưa tiêu, trả lại kho**.

**Vì sao.** *"Điểm hoàn được, đề đã lộ thì không."* Rút mà chưa hiển thị thì chưa ai thấy đề ⇒ chưa lộ — đây cũng là thứ giữ cho việc admin bấm No ở hàng đợi **không có tác dụng phụ**.

**Phạm vi.** No-repeat tính **theo từng contest**, đi xuyên qua mọi trận của contest đó.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `Đ-5.2f`, `GRR-085`, `GRR-118`, `U-30`

---

# I. Ghế, kết nối, và quyền thao tác

### QĐ-045 — Quá grace thì hệ thống CHỈ TÔ NỔI BẬT; admin là người quyết

**Quyết định.** Grace **120 giây** giữ ghế và state-sync. Quá grace, hệ thống **chỉ tô nổi bật** ghế trên màn admin kèm thời lượng mất kết nối — **không tự loại, không tự xoá**. Không có chính sách dropout tự động nào; `dropoutPolicy` **không tồn tại**.

**Hệ quả.** Grace là **khuyến nghị**, không phải ràng buộc: admin can thiệp sớm cũng được. Ghế mất kết nối giữa vòng **không cản trận chạy tiếp**.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `U-13`

### QĐ-046 — Ghế kết nối lại được khôi phục kể cả GIỮA CÂU

**Quyết định.** Server đẩy đủ dữ kiện để client **dựng lại đúng màn đang thi**, kể cả khi câu đang mở và đồng hồ đang chạy.

**Ba ràng buộc.**
1. **Đồng hồ không reset** — client dựng lại từ **hạn chót theo server time**, không phải từ *"còn N giây"*. Mất kết nối **không mua thêm thời gian**.
2. **Không có đáp án trong gói** — payload đi qua đúng bộ lọc theo vai như mọi kênh khác. Khôi phục **không phải** đường vòng để lộ dữ liệu.
3. **Không sinh event** — khôi phục là thao tác **đọc**, không phải ghi.

**Thứ không khôi phục được**: ký tự đang gõ dở mà **chưa gửi** — nó chưa bao giờ tới server.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `Đ-45b`

### QĐ-047 — v1 KHÔNG có kick; dùng VÔ HIỆU HOÁ / KÍCH HOẠT LẠI

**Quyết định.** Admin bật/tắt quyền thao tác của một ghế, **đảo ngược được**, dùng **mọi lúc** trong một trận chưa đóng sổ — kể cả giữa một câu đang mở. Ghế bị vô hiệu hoá **vẫn ở trong trận**: giữ nguyên điểm, giữ nguyên vị trí, vẫn trên mọi bảng; chỉ **mất quyền thao tác** và không được đề xuất lượt.

**Vì sao bỏ ràng buộc "chỉ ở `LOBBY`" của phiên bản kick.** Ràng buộc cũ dựng trên tính **một chiều** của kick: rời một ghế khỏi trận giữa lúc câu đang mở làm hụt đầu vào của những phép tính đang dở. Vô hiệu hoá **không rời ai** — mọi phép tính vẫn đủ đầu vào, chỉ là ghế đó không nộp gì. Bắt đợi `LOBBY` sẽ làm mất đúng tình huống cần nó nhất: **máy một ghế chết giữa vòng**.

**Đây là cờ HÀNH CHÍNH, phạm vi TRẬN, nằm NGOÀI `QĐ-033`.** `QĐ-033` dọn cờ **do một vòng sinh ra**; cờ này do **admin** sinh ra và tồn tại tới khi admin gỡ. Nếu bị dọn mỗi lần mở vòng thì một ghế hỏng máy sẽ tự sống lại giữa trận.

**Không có luật riêng cho ghế bị vô hiệu hoá.** Giữa một câu, nó được xử **y như ghế không trả lời**. Thấy bất công ⇒ admin **cộng tay** (`QĐ-014`). Cố ý không thêm ngoại lệ — mỗi ngoại lệ là một nhánh phải kiểm thử.

**Vô hiệu hoá KHÔNG phải hệ quả của mất kết nối** — hai chuyện độc lập, hai nút khác nhau.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `Đ-52`, `Đ-45a` (bị thay thế)

---

# J. Hiển thị và bảo mật

### QĐ-048 — Admin TOÀN QUYỀN mở và đóng đáp án, ô chữ

**Quyết định.** Quyền không điều kiện. Thao tác **mở** đi qua **dialog Yes/No** để tránh bấm nhầm.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `Đ-11`, `C-11`

### QĐ-049 — Màn công bố kết quả là LỚP PHỦ, không phải một bước của luồng thi

**Quyết định.** Lớp hiển thị bảng xếp hạng, **chồng lên** trạng thái đang chạy mà **không huỷ** nó. Hệ thống **gợi ý** mở ở hai mốc — hết vòng và hết trận — nhưng admin mở/đóng **tuỳ ý, bất cứ lúc nào**. **Không có bộ đếm tự đóng.**

**Ba ràng buộc nội dung.**
1. Thứ hạng **do server tính và đẩy xuống**; client chỉ render. Client tự tính thì một máy lỡ mất một lệnh điểm sẽ hiện bảng khác server.
2. Hoà điểm ⇒ **ĐỒNG HẠNG**, hạng kế **nhảy qua** số người đồng hạng.
3. Điểm hiển thị = `reduce(event log)` tại thời điểm mở; **điểm âm hiển thị bình thường**.

**Nhịp lộ từng người là ANIMATION client-side**, không phải engine.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `C-19`, `GRR-032`, `GRR-057`

### QĐ-050 — Banner tạm dừng: chặn toàn cục, KHÔNG CHỮ

**Quyết định.** Lớp phủ chặn **toàn bộ** thao tác trên máy thí sinh và báo tạm dừng trên viewer/overlay. **Không chữ, không lý do.** Máy admin **không bị phủ**. **Chỉ bật được khi không đồng hồ nào đang chạy.**

**Vì sao có ràng buộc đồng hồ, và chiều ngược lại của nó.** Nếu bật được lúc đồng hồ chạy thì thí sinh bị chặn trong khi giờ vẫn trôi. Chiều ngược lại là **suy ra và bắt buộc**: **không start timer được khi banner đang bật** — thiếu nó thì admin cứ mở banner lúc rảnh rồi start timer, đúng cái tình huống muốn tránh.

**Hệ quả.** Không tồn tại thời điểm nào banner và một đồng hồ đang chạy cùng có mặt, nên câu hỏi *"banner có đóng băng đồng hồ không"* **không có chủ ngữ** — `QĐ-030` không cần ngoại lệ.

*Nguồn*: `[CHỦ DỰ ÁN]` + `[SUY RA]` (chiều thứ hai) · *Thay cho*: `C-21`

### QĐ-051 — Đáp án chỉ rời server tới ADMIN và MC

**Quyết định.** Thí sinh, viewer, overlay **không** nhận đáp án chuẩn. **Ngoại lệ có cấu hình**: `revealAnswerAfterJudge` — mặc định **TẮT** ở trận official, **BẬT** ở trận practice; khi bật thì đáp án chỉ được đẩy **sau khi đã chấm xong**.

**Hệ quả.** Mọi kênh đều đi qua cùng một bộ lọc theo vai — kể cả gói khôi phục kết nối (`QĐ-046`) và lớp công bố (`QĐ-049`).

*Nguồn*: `[CHỦ DỰ ÁN]`

---

# K. Quyết định theo vòng

### QĐ-052 — Ô chữ VCNV có BA trạng thái; ba nút của admin ĐỘC LẬP

**Quyết định.** Mỗi ô của bàn cờ VCNV — 4 hàng ngang + ô trung tâm — mang đúng một giá trị: **chờ** → **đã hỏi** → **mở**. Ba nút của admin **độc lập**, không nút nào kéo theo nút nào: *mở hàng ngang* · *hiển thị câu hỏi* · *start timer*.

**Vì sao tách trạng thái ô khỏi vòng đời câu hỏi.** Hai trục **song song**, không lồng nhau: một ô sang *đã hỏi* mà **chưa câu nào được hiển thị** là hợp lệ (admin đặt tay), và một câu **đã chấm xong** mà ô vẫn ở *đã hỏi* cũng hợp lệ (không ai đúng). Trộn hai trục thì cả hai tình huống đều thành trạng thái bất hợp lệ — mà chúng đúng là hai tình huống cần phục vụ.

**Băng điểm Chướng ngại vật là HÀM của trạng thái, không phải bộ đếm cộng dồn:**

```
băng = [60, 50, 40, 30, 20][ số hàng ngang KHÔNG ở trạng thái chờ ]
sau khi đã đưa ra gợi ý cuối ⇒ băng = 20 (sàn)
```

Cách viết này bảo đảm hai điều mà bộ đếm `+1` không: **không đếm trùng** (ba lối vào đều chỉ **đặt** trạng thái) và **lùi được** (admin đặt nhầm thì đặt lại, băng tự đúng theo).

**Ô trung tâm KHÔNG vào phép tính băng** — nó dùng chung ba trạng thái để **hiển thị**, nhưng băng chỉ đếm **4 hàng ngang**; thang có đúng 5 bậc cho 0→4 ô, thêm ô thứ năm là **tràn bậc**. Việc gợi ý cuối hạ băng xuống 20 là một **quy tắc riêng** với 20 là **sàn**.

**Đường mở tay của admin** đặt thẳng một ô sang giá trị bất kỳ, **cả hai chiều**. Nó **không sinh điểm cho ai** và **không tiêu câu** — mốc tiêu câu là *hiển thị câu hỏi*, mà nút đó đã tách rời. Đây là **đường duy nhất để dựng lại bàn cờ sau sự cố**, nên nó phải nuôi đúng biến mà băng điểm đọc: nếu không thì trạng thái khôi phục **trông đúng mà tính điểm sai**.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `Đ-44`, `Đ-50`, `U-32`

### QĐ-053 — Chốt câu ⇒ ghế chưa chấm mặc định SAI

**Quyết định.** Áp cho **Tăng tốc** và **câu hàng ngang VCNV**: admin chốt câu khi mới chấm một phần ⇒ mọi ghế chưa chấm **mặc định SAI**.

**Đây KHÔNG phải máy tự chấm.** Cú bấm *"chốt câu"* **chính là phán quyết** — nó phát biểu *"những ai tôi chưa chấm ⇒ SAI"*. **Máy không tự làm gì khi hết giờ**; không có bộ đếm nào tự chốt.

**Ngoại lệ tường minh — KHÔNG áp lên tín hiệu "Mở chướng ngại vật" đang chờ duyệt.** Ở VCNV, "sai" có **hai** hậu quả khác hẳn nhau:

| Loại phán quyết | Hậu quả khi SAI | Mặc định có áp |
|---|---|---|
| Câu hàng ngang | **0** điểm, ghế **vẫn thi tiếp** | **CÓ** |
| Trả lời Chướng ngại vật | Ghế **BỊ LOẠI** khỏi vòng | **KHÔNG** |

Rào này bắt buộc: nếu để mặc định quét cả tín hiệu CNV đang chờ, admin chốt câu vì lý do hoàn toàn khác sẽ **loại nhầm** một thí sinh — hậu quả nặng nhất của cả vòng.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `Đ-43`

### QĐ-054 — Câu hỏi phụ: chuông KHÔNG SỐNG trước mốc start timer

**Quyết định.** Máy **khử** tình huống bấm sớm thay vì **phạt** nó. Nút chuông không render trước mốc; server **cũng** từ chối tín hiệu tới sớm.

**Vì sao.** Luật gốc **phải** phạt vì trên trường quay nút chuông là **phần cứng, lúc nào cũng sống**. Hệ thống này **điều khiển được nút** nên khử luôn nguyên nhân — mục đích của luật được bảo đảm **tuyệt đối** thay vì bằng răn đe.

**Hệ quả.** Toàn bộ cơ chế *"lệnh cấm theo câu"* và *"gỡ lệnh cấm"* **không tồn tại trong hệ thống** — không có trạng thái, không có event, không có nút. Đặc tả máy trạng thái **không nhắc tới chúng**: nó mô tả những gì máy có, và máy không có chúng. Dấu truy nguyên về rule gốc sống ở **chính mục này** và ở `traceability.md`, không phải ở một trạng thái chết trong đặc tả.

**Thứ VẪN là yêu cầu sống**: server phải **từ chối** mọi tín hiệu chuông tới trước mốc start ở Câu hỏi phụ. Disable nút chỉ là UX; đây là ràng buộc zero-trust và nó thuộc về **event bấm chuông**, không thuộc về một trạng thái phạt đã biến mất.

**KHÔNG áp cho Khởi động lượt chung**: ở đó luật gốc nói thẳng *"Thí sinh **có thể** bấm chuông **trong khi** người dẫn chương trình đang đọc câu hỏi"*. Chặn ở đó là **phá luật**.

**Hệ quả thứ hai — "hiển thị câu hỏi mở cửa sổ chuông" thôi là quy tắc phổ quát.** Nó sinh ra từ nhu cầu của Khởi động lượt chung rồi bị áp cho mọi vòng. Sau quyết định này, mốc đó mở cửa sổ chuông ở **đúng một vòng**:

| Vòng | Cửa sổ chuông mở từ |
|---|---|
| **Khởi động lượt chung** | **Hiển thị câu hỏi** — sống suốt lúc MC đọc, thêm 3 giây sau start timer |
| **Câu hỏi phụ** | **Start timer** |
| **VCNV** (*"Mở chướng ngại vật"*) | Không gắn mốc nào — bấm được bất cứ lúc nào trong vòng |
| **Về đích** (cướp quyền) | Admin chấm Sai — cửa sổ 5 giây |

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `Đ-55`

### QĐ-055 — Câu hỏi phụ KHÔNG cộng điểm trận

**Quyết định.** Ba câu, 15 giây mỗi câu. Người thắng chỉ **đổi thứ hạng**, không cộng điểm. Trả lời **sai** ⇒ **cả nhóm sang câu tiếp**, người sai **không** bị loại khỏi các câu sau. Hết 3 câu chưa phân định ⇒ **bốc thăm**.

**Vì sao không cộng điểm.** Luật gốc mô tả kết quả là *"thí sinh có số điểm cao nhất **bằng với** số điểm của thí sinh còn lại"* — tức phân định thứ hạng, không phải ghi điểm.

**Không ai bấm trong 15 giây**, hoặc **cả nhóm bị cấm ở một câu**: vẫn **TÍNH** vào tổng ba câu.

*Nguồn*: `[LUẬT GỐC]` · *Thay cho*: `GRR-058`, `GRR-059`, `GRR-061`, `GRR-064`, `Đ-9.a`

### QĐ-056 — Khởi động: đọc nguồn cho bốn chỗ từng bị coi là thiếu

**Quyết định.**

| Mục | Quyết định |
|---|---|
| Cửa sổ chuông lượt chung | **Một khoảng liên tục** = (thời gian MC đọc) + 3 giây |
| Không trả lời ở lượt riêng | Xử **như trả lời sai**: 0 điểm, không trừ — cùng một thao tác của admin |
| Biên quỹ thời gian | Hết giờ còn câu, hoặc hết câu còn giờ ⇒ **lượt kết thúc** ở cả hai nhánh |
| Thứ tự lượt riêng | Luật **không quy định** ⇒ khuyến nghị theo contest settings |

*Nguồn*: `[LUẬT GỐC]` · *Thay cho*: `GRR-005`, `GRR-006`, `GRR-012`, `Đ-5.a`, `Đ-19`

### QĐ-057 — VCNV: sáu chỗ đọc nguồn đã giải quyết

**Quyết định.**

| Mục | Cách đọc nguồn |
|---|---|
| Điều kiện tới ô trung tâm | *"Từ hàng ngang được mở"* ≠ *"miếng ghép được mở"* — nguồn dùng **hai chủ ngữ khác nhau** ⇒ cả 4 hàng **luôn được hỏi hết** ⇒ ô trung tâm **luôn tới được** |
| Thang điểm CNV | *"Trong N từ hàng ngang"* đếm theo số hàng **ĐÃ HỎI**; bấm trước khi hàng 1 bắt đầu vẫn thuộc băng **60** |
| Mốc 20 điểm | Gắn với việc **gợi ý cuối đã được đưa ra**, **không** phụ thuộc ô trung tâm đúng hay sai |
| Luật quay vòng lượt chọn | *"Trong trường hợp này"* là **mệnh đề giới hạn phạm vi** ⇒ *"tối đa 1 lượt"* là luật chung, quay vòng là **ngoại lệ tường minh** khi đã có người bị loại. Không phải mâu thuẫn |
| Nhiều người bấm giải CNV | Thứ tự = thứ tự vào hàng đợi; người bị từ chối **không mất lượt** |
| Công bố Chướng ngại vật | Thao tác **thủ công** của admin, **tuỳ chọn** — vòng vẫn kết thúc được mà không công bố |

*Nguồn*: `[LUẬT GỐC]` · *Thay cho*: `GRR-013`, `GRR-016`, `GRR-019`, `GRR-021`, `GRR-025`, `GRR-027`

### QĐ-058 — Về đích: bốn chỗ đọc nguồn đã giải quyết

**Quyết định.**

| Mục | Quyết định |
|---|---|
| Ngôi sao hy vọng **sai** + có người cướp **đúng** | **Trừ MỘT lần.** Hình phạt NSHV **thay thế** phần nợ của transfer, không cộng dồn. Câu 30đ ⇒ **A −30 · B +30**, không phải −60 |
| Cửa sổ đặt NSHV | **Đóng tại mốc admin bấm hiển thị câu hỏi** |
| Mốc *"hoàn thành phần thi"* | **Sau khi** cửa sổ cướp của câu cuối đóng **và** đã chấm xong |
| Kết thúc câu sớm | **KHÔNG** — chạy hết giờ |
| Giá trị câu **lẻ** | Không phát sinh dưới luật 2026 (20/30 → 10/15). Nếu admin cấu hình mức lẻ: `phạt = value / 2` bằng **phép chia số nguyên**, làm tròn xuống theo **độ lớn** hình phạt |

**Hai con số hay bị lẫn.** **5 giây** là độ dài **cửa sổ bấm chuông** cướp quyền; **20/40 giây** là **thời gian thực hành của người cướp**, chỉ bắt đầu đếm **sau khi** đã có người giành được quyền.

*Nguồn*: `[LUẬT GỐC]` · *Thay cho*: `GRR-041`, `GRR-047`, `GRR-048`, `GRR-056`, `U-8`, `U-20`, `Đ-3`

### QĐ-059 — Tăng tốc: xếp hạng tính trên TẬP NGƯỜI ĐÚNG

**Quyết định.** Bốn câu, thời gian 20/20/30/30 giây, **luôn gõ máy**. Điểm theo **thứ hạng tốc độ trong số người được admin chấm ĐÚNG**: 40/30/20/10 — người sai **không giữ chỗ** trong thang. Đồng thời gian tới **millisecond** ⇒ **cùng mức điểm**, bậc kế **nhảy qua** số người hoà.

**Nhận mọi lần trả lời tới khi hết giờ, tính bản CUỐI CÙNG.** Không khoá ô nhập, không khoá nút gửi. Xếp hạng theo server-received timestamp của bản cuối. **Bản nội dung y hệt bản trước KHÔNG cập nhật mốc** — cập nhật mốc cho một bản không đổi nội dung cho phép thí sinh **tự làm xấu** thứ hạng của mình bằng thao tác vô nghĩa, mà ở vòng xếp hạng thì mốc **là** kết quả.

*Nguồn*: `[LUẬT GỐC]` + `[CHỦ DỰ ÁN]` · *Thay cho*: `GRR-033`, `GRR-039`, `Đ-32`, `K-8`

### QĐ-060 — Nút thao tác một chiều tự tắt sau khi bấm

**Quyết định.** Mọi nút thực hiện một thao tác một chiều **tự tắt** ngay sau lần bấm đầu. Server vẫn phải bỏ qua lệnh trùng — client không được tin.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `Đ-29`

### QĐ-061 — Phán quyết nhị phân khi "Sai" trừ 0 điểm; có phạt thì thêm "Huỷ kết quả"

**Quyết định.** Ở vòng mà *Sai* trừ 0 điểm (Khởi động lượt riêng), hai nút Đúng/Sai là đủ. Ở vòng mà *Sai* có **hình phạt** (Khởi động lượt chung, Về đích), cần nút thứ ba — **Huỷ kết quả** — cho tình huống câu không có kết quả mà không ai đáng bị phạt.

**Hệ quả.** *Huỷ kết quả* là một **hạng phán quyết có sẵn**, nên các đường khép câu bất thường (`QĐ-034`) dùng lại nó thay vì phát minh nhánh mới.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `Đ-34`

---

# L. Bảng tra mã CŨ → MỚI

> Dùng khi đọc tài liệu chưa dọn hoặc `reviews/`. Mã cũ **không còn xuất hiện** trong đặc tả.

| Mã cũ | Mã mới | Mã cũ | Mã mới |
|---|---|---|---|
| `Đ-1` | `QĐ-010` | `Đ-33` | `QĐ-027` |
| `Đ-2` | `QĐ-012` | `Đ-34` | `QĐ-061` |
| `Đ-3` | `QĐ-058` | `Đ-35` | `QĐ-030` |
| `Đ-4`, `Đ-4.1` | `QĐ-016` | `Đ-36` | `QĐ-019` |
| `Đ-4.2` | `QĐ-019` | `Đ-37` | `QĐ-043` |
| `Đ-4.3` | `QĐ-005`, `QĐ-023` | `Đ-38` | `QĐ-032` |
| `Đ-4.5`, `Đ-4.6` | `QĐ-002` | `Đ-39` | `QĐ-033` |
| `Đ-4.a2` | `QĐ-017` | `Đ-40`, `Đ-41` | `QĐ-019` |
| `Đ-4.b`, `Đ-4.X2` | `QĐ-018` | `Đ-42` | `QĐ-033` |
| `Đ-4.f` | `QĐ-056` | `Đ-43` | `QĐ-053` |
| `Đ-5` | `QĐ-002` | `Đ-44` | `QĐ-052` |
| `Đ-5.1`, `Đ-5.2` | `QĐ-035` | `Đ-45a` | `QĐ-047` *(thay thế)* |
| `Đ-5.2f` | `QĐ-044` | `Đ-45b` | `QĐ-046` |
| `Đ-5.3` | `QĐ-011` | `Đ-46` | `QĐ-034` |
| `Đ-5.3.1` | `QĐ-013` | `Đ-47` | `QĐ-038` |
| `Đ-5.3.X` | `QĐ-011` | `Đ-48` | `QĐ-036` |
| `Đ-5.a` | `QĐ-056` | `Đ-49` | `QĐ-039` |
| `Đ-6`, `Đ-6.1` | `QĐ-027` | `Đ-50` | `QĐ-052` |
| `Đ-6.2` → `Đ-6.4` | `QĐ-054` *(mất đường vào)* | `Đ-51` | `QĐ-037` |
| `Đ-6.3` | `QĐ-006` | `Đ-52` | `QĐ-047` |
| `Đ-6.4a` | `QĐ-001` | `Đ-53` | `QĐ-031` |
| `Đ-7`, `Đ-7.a`, `Đ-7.b` | `QĐ-020` | `Đ-54` | `QĐ-026` |
| `Đ-7.1` | `QĐ-002` | `Đ-55` | `QĐ-054` |
| `Đ-7.2` | `QĐ-021` | `Đ-56` | `QĐ-014` |
| `Đ-7.3` | `QĐ-033` | `Đ-57` | `QĐ-040` |
| `Đ-9`, `Đ-9.a` | `QĐ-055` | `C-7` | `QĐ-007` |
| `Đ-10.6`, `Đ-10.7` | `QĐ-055` | `C-9` | `QĐ-039` |
| `Đ-11` | `QĐ-048` | `C-11` | `QĐ-048` |
| `Đ-11.B` | `QĐ-035` | `C-12` | `QĐ-010` |
| `Đ-15.3` | `QĐ-003` | `C-13` | `QĐ-016` |
| `Đ-16` | `QĐ-004` | `C-14` | `QĐ-009` |
| `Đ-17` | `QĐ-014` | `C-15` | `QĐ-002` |
| `Đ-18` | `QĐ-008` | `C-18` | `QĐ-043` |
| `Đ-19` | `QĐ-056` | `C-19` | `QĐ-049` |
| `Đ-20`, `Đ-21` | `QĐ-030` | `C-20` | `QĐ-015` |
| `Đ-22` | `QĐ-031` | `C-21` | `QĐ-050` |
| `Đ-23` | `QĐ-025` | `U-8`, `U-20` | `QĐ-058` |
| `Đ-24` | `QĐ-023` | `U-13` | `QĐ-045` |
| `Đ-25` | `QĐ-024` | `U-30` | `QĐ-044` |
| `Đ-26` | `QĐ-028` | `K-6` | `QĐ-010` |
| `Đ-27` | `QĐ-020` | `K-8` | `QĐ-059` |
| `Đ-28` | `QĐ-029` | `NT-C` | `QĐ-032` |
| `Đ-29` | `QĐ-060` | `R-GEN-07` | `QĐ-011` *(bị thay thế)* |
| `Đ-30` | `QĐ-041` | `GRR-077` vế 1 | `QĐ-032` |
| `Đ-31` | `QĐ-003`, `QĐ-042` | `GRR-077` vế 2 | `QĐ-038` |
| `Đ-32` | `QĐ-059` | `GRR-120` | `QĐ-037` *(bác)* |

---

# M. Còn treo

*(Trống — mọi mục thuộc phạm vi luật chơi và máy trạng thái trận đã được chốt.)*

**Ngoài phạm vi sổ này, còn treo ở tài liệu khác:**

| Mã | Nội dung | Ở đâu |
|---|---|---|
| `C-1` | `revealAnswerAfterJudge` là per-contest hay per-match | `product-discovery.md` §6 |
| `C-2` · `C-5` | Quy mô viewer · ngưỡng độ trễ — bốn con số khác nhau giữa các nguồn | `product-discovery.md` §6 |
| `C-3` | Ai duyệt đề `DRAFT` → `ACTIVE` | `product-discovery.md` §6 |
| `C-10` | `Question.visibility` là cột set tay hay giá trị dẫn xuất | `product-discovery.md` §6 · `glossary.md` |

Bốn mục này thuộc **phạm vi sản phẩm**, không phải luật chơi — chúng không chặn đặc tả luật hay máy trạng thái.
