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

**Số hiệu là ĐỊNH DANH, không phải chỉ mục.** Mục mới nhận số kế tiếp còn trống và được đặt vào **đúng chương chủ đề** của nó — nên thứ tự đọc không nhất thiết tăng dần. Đây là chủ đích: đánh số lại mỗi lần thêm một quyết định sẽ làm mọi trích dẫn cũ trôi nghĩa, kể cả trích dẫn nằm ngoài repo.

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

### QĐ-068 — v1 khoá cứng BỐN hàng ngang VCNV; cấu hình vẫn nhận 5-8

**Quyết định.** v1 **khoá cứng `rowCount = 4`**. Mô hình dữ liệu, RuleConfig và giao diện vẫn nhận **5-8** để phiên bản sau chỉ việc mở khoá, nhưng **engine-path cho ≠ 4 chưa tồn tại** và cửa tạo contest **không cho chọn** giá trị khác.

**Vì sao.** Băng điểm Chướng ngại vật cho 5-8 hàng **không tồn tại trong bất kỳ nguồn nào** — bịa ra là bịa requirement. Khoá cứng ở tầng **cấu hình** thì rẻ và gỡ được; để mở mà không có luật thì hệ thống chạy vào một nhánh không ai đặc tả.

**Hệ quả.** Cùng khuôn với `QĐ-007`: **hạ tầng làm sẵn, luật để sau**. Không migrate schema khi mở khoá.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `U-3`, `C-7` *(vế `rowCount`)*

### QĐ-069 — v1 khoá cứng playlist BỐN vòng chuẩn

**Quyết định.** Playlist v1 **luôn là bốn vòng**, đúng thứ tự luật gốc: **Khởi động → Vượt chướng ngại vật → Tăng tốc → Về đích**. **Câu hỏi phụ không phải vòng thứ năm** — nó là nhánh phân định, chỉ mở khi có hoà trong `tieBreakPositions` (`GR-022`).

Không lặp vòng, không đổi thứ tự, không bớt vòng ở **thiết kế** contest.

**Vì sao.** Playlist tuỳ ý là mục tiêu dài hạn, nhưng luật gốc **không có** tình huống *"chạy Khởi động hai lần trong một trận"*: điểm cộng dồn thế nào, cờ Ngôi sao hy vọng có đặt lại không, thứ tự lượt tính theo lần chạy nào — không câu nào trong nguồn trả lời được.

**Đừng nhầm với `QĐ-035`.** Bỏ vòng và **chạy lại** vòng vẫn được phép — đó là **sửa sự cố** một vòng đã hỏng, khác hẳn việc **thiết kế** một contest có hai vòng Khởi động. Thứ tự **chạy** vẫn do admin quyết (`QĐ-002`); thứ bị khoá là **cấu hình** playlist.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `C-7` *(vế playlist)*, `G-2` *(vế "số vòng không cứng")*

### QĐ-085 — v1 khoá cứng phạm vi phân định hoà: CHỈ vị trí NHẤT

**Quyết định.** `tieBreakPositions` ở v1 **luôn là `[1]`**. Cửa tạo contest **không cho chọn** giá trị khác. Mô hình dữ liệu và cấu hình luật vẫn **nhận** mảng nhiều vị trí mà không lỗi cấu trúc — nhưng **đường xử lý không tồn tại**.

Hoà ở mọi vị trí khác vị trí nhất ⇒ ghi **đồng hạng**, hạng kế nhảy qua. Đây không phải ca lỗi, đó là `GR-022` C4 đã có sẵn.

**Vì sao — đây là khoá cứng THỨ BA, cùng khuôn với hai cái đã có.** `QĐ-068` khoá `rowCount = 4` và `QĐ-069` khoá playlist bốn vòng, cả hai vì cùng một lý do: **luật cho giá trị khác không tồn tại trong nguồn, mở ra là bịa luật.** `tieBreakPositions` rơi đúng vào khuôn đó mà chưa bị khoá. Luật gốc chỉ mô tả chọn ra **một** người thắng.

**Chỗ thiếu KHÔNG phải luật của một lượt phân định, mà là luật ĐIỀU PHỐI nhiều lượt.** Cơ chế Câu hỏi phụ tự nó không phụ thuộc vị trí — 3 câu, chuông, không cộng điểm, chạy cho hạng 2 cũng hoạt động y hệt. Thứ không có nguồn là bốn điều dưới đây, và cả bốn chỉ xuất hiện khi có **từ hai nhóm hoà trở lên**:

- **Thứ tự phân định.** Hoà ở hạng 1 và hoà ở hạng 3 cùng lúc thì giải nhóm nào trước, và giải xong nhóm đầu có phải tính lại nhóm sau không.
- **Ngân sách câu.** 3 câu cho mỗi nhóm hay 3 câu dùng chung. Pre-flight (`GR-031`) hiện kiểm **một** con số cố định; với N nhóm nó phải kiểm `3N`, mà N chỉ biết được **tại cú bấm chốt trận** — sau khi cửa vào vòng đã đóng.
- **Tái nhập `STATE-007`.** `GR-022` C7 chốt *"chốt trận lần hai **không** vào lại Câu hỏi phụ"*. Với nhiều nhóm, cú bấm thứ hai **phải** vào lại cho nhóm chưa giải — tức phải viết lại chính cái guard chống vòng lặp mà `QĐ-083` dựng lên.
- **Va chạm với `QĐ-083` vế 5.** Event tie-break mất hiệu lực khi nhóm không còn bằng điểm **và không còn đúng `position`**. Với nhiều nhóm, một lần admin sửa điểm ở `LOBBY` có thể làm mất hiệu lực nhóm này mà giữ nguyên nhóm kia — trạng thái hợp lệ nhưng chưa ai đặc tả cách đọc.

Khoá `[1]` xoá cả bốn khỏi v1 mà **không mất tính năng nào đang có nguồn**.

**Hệ quả.**

- **`GR-022` C1 và C4 giữ nguyên văn.** Chúng đã viết theo `tieBreakPositions`, nên khoá giá trị không đụng câu chữ — chỉ thu hẹp tập giá trị đầu vào.
- **`traceability.md` chuyển dòng *"Câu hỏi phụ cho nhiều nhóm hoà"*** từ nhóm *cấu hình được* sang nhóm **v1 khoá cứng**, cạnh `rowCount` và playlist. Ghi chú *"v1 khoá cứng **hai** thứ"* thành **ba**.
- **`QĐ-083` §Ba ca biên vẫn đúng nguyên văn.** Ca *"sửa điểm làm hoà nhóm khác"* nay chỉ có nghĩa: sửa điểm tạo ra một nhóm hoà **mới ở vị trí nhất**. Vẫn là `TIE_BREAK` lần hai, vẫn tiêu 3 câu, vẫn tối đa 4 lần với kho dư 12 câu.
- **Ví dụ `tieBreakPositions = [1,2]` trong `QĐ-083`** là minh hoạ cho phương án *"+1đ"* đã bị loại, **không phải** cấu hình v1 hỗ trợ. Giữ nguyên vì nó đang chứng minh một lập luận, không đang mô tả sản phẩm.

*Nguồn*: `[CHỦ DỰ ÁN]` · `[SUY RA]` từ `QĐ-068`, `QĐ-069` *(cùng khuôn)* · *Thay cho*: câu hỏi mở **phạm vi phân định hoà** *(nay đã đóng)*, `GRR-158`

### QĐ-089 — Số trận song song: mục tiêu định cỡ là SÁU, không phải chặn cứng

**Quyết định.** Một bản cài được định cỡ và kiểm thử tải cho **tối đa 6 trận chạy đồng thời**; trường hợp thường trực là **1-2 trận**.

Con số này là **mục tiêu vận hành**, **không** phải ràng buộc chức năng: hệ thống **không đếm** số trận đang chạy và **không từ chối** trận thứ bảy. Không rule, không transition, không guard nào đọc nó.

**Vì sao không biến nó thành chặn cứng.** `PS-8` đã được giải ở tầng kiến trúc — trạng thái thuộc về **trận**, không phải biến toàn cục (`QĐ-039`) — và lời giải đó đúng với mọi N. Thêm một hạn ngạch nghĩa là thêm một ca hỏng mới *(từ chối mở trận vì lý do không liên quan tới luật chơi)* để đổi lấy đúng con số không.

Cái con số dùng để làm là **định cỡ máy** và **dựng bài kiểm thử tải**. `INV-014` chốt chỉ có **ba** chỗ chặn cứng; đây không phải chỗ thứ tư.

**Hệ quả.** `ASSUMPTION-002` chuyển từ *"con số chốt mà không dẫn nguồn nhu cầu"* sang **có con số từ chủ dự án**, nhưng vế *"nhiều trận song song là nhu cầu thật"* **vẫn là giả định chưa kiểm chứng** — 6 là kỳ vọng, không phải quan sát.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: câu hỏi mở **số trận song song** *(nay đã đóng)*

### QĐ-008 — Một contest có nhiều TÀI KHOẢN admin, nhưng đúng MỘT PHIÊN điều khiển

**Quyết định.** Ràng buộc *"một admin"* đặt ở tầng **phiên**, không ở tầng tài khoản. Một contest được gán **nhiều tài khoản** quyền admin; tại một thời điểm chỉ **một phiên** giữ quyền điều khiển. Các phiên admin khác **xem được, không bấm được** — cùng bề mặt hiển thị, khác quyền ghi.

**Vì sao.** Khoá theo **tài khoản** thì mất người là mất trận: admin ốm, máy hỏng, đổi ca giữa buổi đều thành sự cố không lối thoát. Khoá theo **phiên** giữ nguyên toàn bộ lợi ích của một-người-bấm — không tranh chấp phán quyết, không khoá đồng thời, thứ tự ghi tất định — mà vẫn có **người thay thế**.

**Hệ quả.** Phải có cơ chế **chuyển quyền điều khiển** giữa hai phiên, và mọi lần chuyển đều vào `AuditLog`. Mỗi event vẫn mang **đúng một** `actor` = phiên đang giữ quyền lúc bấm ⇒ mô hình event log không đổi gì.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `Đ-18`, `S-16`

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

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `Đ-5.3.1`, `GRR-071`, `TERM-026`

### QĐ-014 — Một câu đi qua ĐÚNG MỘT phán quyết; sai thì admin cộng tay

**Quyết định.** Sau khi admin chấm, nút chấm **khoá** và nút *"Câu kế tiếp"* hiện lên. **Không có** bấm lại, **không có** đổi phán quyết tại chỗ, **không có** cơ chế tính lại thứ hạng. Sửa một phán quyết đã chốt đi qua **điều chỉnh điểm thủ công**, mỗi ghế một event kèm lý do bắt buộc.

**Vì sao.** Hệ thống không quan tâm tới sai lầm — nó chỉ cần **một** cơ chế sửa sai đơn giản. Một cơ chế tính lại tự động sẽ là đường code duy nhất tự sinh điểm mà không qua phán quyết của người.

**Hệ quả.** Ở Tăng tốc, admin **tự tính delta** kể cả phần dây chuyền — một ghế từ Sai thành Đúng thì những người xếp sau tụt một bậc, có thể phải sửa tới bốn ghế cho một lỗi. Đây là **cái giá được chấp nhận có chủ ý**: đổi lấy một mô hình mà người vận hành giữ trọn trong đầu.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `Đ-17`, `Đ-56`

### QĐ-015 — Điểm số là thông tin CÔNG KHAI với mọi vai

**Quyết định.** Bảng điểm hiển thị cho viewer, overlay, thí sinh, MC, admin. Chỉ **đáp án** mới bị giới hạn (`QĐ-051`).

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `C-20`

### QĐ-083 — Kết quả Câu hỏi phụ là EVENT THỨ HẠNG, không phải event điểm

**Quyết định.** Bốn vế:

1. **Vật ghi là một event cấp trận, loại riêng — `TIE_BREAK_RESOLVED`**, nằm trong cùng nhật ký append-only với mọi event khác. **Một loại event cho cả hai đường thắng**: được chấm Đúng ở một trong ba câu, hoặc thắng bốc thăm. Trường `method` phân biệt hai đường.
2. **Nó KHÔNG phải event điểm.** Không cộng, không trừ, không xuất hiện trong `reduce(event log)` ra bảng điểm. Bảng điểm sau tie-break **y hệt** bảng điểm trước tie-break.
3. **Độ ưu tiên: chỉ sắp trong nhóm BẰNG ĐIỂM.** Thứ hạng là `f(điểm, các event tie-break)`, trong đó vế sau chỉ được phép sắp thứ tự **bên trong** một nhóm cùng điểm. Nó **không bao giờ** đảo được thứ tự hai người khác điểm.
4. **Hoàn nguyên được, bằng đúng cơ chế chung** — vòng `TIE_BREAK` xong thì trận về **`LOBBY`**, chưa niêm phong. Muốn gỡ kết quả thì **bỏ vòng `TIE_BREAK`** như bỏ bất kỳ vòng nào khác (`QĐ-034`). Không có cơ chế hoàn nguyên riêng cho vòng này.
5. **Một event tie-break chỉ CÒN HIỆU LỰC khi nhóm của nó vẫn đang bằng điểm** và vẫn ở đúng `position`, đo tại **mốc đọc**. Không còn thoả ⇒ event **mất đối tượng**, tự động không áp dụng — không xoá, không đánh dấu vô hiệu, không cảnh báo. Vì thế event mang thêm `scoreAtResolution`.

**Vì sao không cộng 1 điểm cho người thắng.** Phương án *"+1đ"* từng được cân nhắc và bị loại. Nó có ba ưu điểm thật — nằm gọn trong mô hình `QĐ-011`, tự khử mọi tình huống hoà, và giữ thứ hạng là hàm của **một** nguồn duy nhất. Nhưng nó hỏng ở bốn chỗ:

- **Nó sửa LUẬT chứ không sửa mô hình dữ liệu.** `QĐ-055` chốt *"không cộng điểm"* với lý do đọc thẳng từ luật gốc.
- **Biên bản nói dối.** Bảng điểm cuối in `101 – 100`, một con số **chưa từng xảy ra trên sân khấu**. Cả ý nghĩa của vòng này là *"hai người bằng điểm"*.
- **Đẻ ra hoà mới.** Với `tieBreakPositions = [1,2]`, nhóm vị trí 2 đang ở 99 thắng tie-break rồi lên 100 — bằng nhóm vị trí 1 vừa phân định xong.
- **`+1` là số ma** trong một hệ mà mọi giá trị điểm đều là bội của mức và `scoringUnit` cấu hình được; và ở nhánh bốc thăm nó thành *"trúng thăm được 1 điểm"*.

**Vì sao chốt `TIE_BREAK` → `LOBBY`, không thẳng `FINISHED`.** Hai tài liệu từng nói ngược nhau: `GR-023` C1 và `GR-025` C2 ghi *về `LOBBY`*, còn `STATE-007`, `STATE-016`, `T-017`, `T-019` ghi *thẳng `FINISHED`*. Chọn **`LOBBY`** vì nó giữ được **cửa sửa sai cho đúng cái vòng quyết định người vô địch**: `FINISHED` niêm phong ngay, nên một cú chấm nhầm ở câu tie-break là chung cục, không đường gỡ. Giá phải trả là **hai** cú bấm Chốt trận thay vì một, cộng một mệnh đề guard — và guard đó **không phải luật mới**, nó suy ra thẳng từ vế 3.

**Hai hệ quả của `LOBBY`, và cách vế 5 giải cả hai.**

- **Vòng lặp tái nhập.** Tie-break không đổi điểm ⇒ cú Chốt trận thứ hai lại thấy nhóm hoà cũ và lại vào `TIE_BREAK`. **Giải:** `GR-022` **bỏ qua** nhóm đã có `TIE_BREAK_RESOLVED` **còn hiệu lực** ⇒ đóng sổ theo thứ hạng đã phân định.
- **Admin sửa điểm sau tie-break** (`QĐ-035` mở ở `LOBBY`) ⇒ nguy cơ *"người thắng tie-break lại thua điểm"* mà `TERM-038` khẳng định không thể có. **Giải:** sửa điểm làm nhóm không còn bằng điểm ⇒ event **mất đối tượng** ⇒ phép phân định chạy lại trên bảng điểm mới. Đây là hành vi **đúng**, không phải ca lỗi: điểm mới là điểm đúng, thứ hạng phải theo nó.

**Ba ca biên đã kiểm.** Sửa điểm làm **hết hoà** ⇒ đóng sổ theo điểm · sửa điểm làm **hoà nhóm khác** ⇒ vào `TIE_BREAK` lần hai, tiêu thêm 3 câu *(kho dư 12 câu ⇒ tối đa 4 lần, cửa vào vòng vẫn kiểm như thường)* · sửa điểm rồi **sửa ngược lại** ⇒ event **sống lại**, vì tiêu chí là *trạng thái hiện tại* chứ không phải *đã từng bị đụng* — nhất quán với `QĐ-011`.

**Hệ quả.**

- **`GR-023` C1 và `GR-025` C2 giữ nguyên đích `LOBBY`**; ngược lại `STATE-007`, `STATE-016`, `T-017`, `T-019` phải sửa từ `FINISHED` sang `LOBBY`.
- **`GR-022` phải thêm guard** và hai ca cho cú bấm Chốt trận thứ hai.
- **`GR-022` §Bấm trùng phải viết lại.** Câu *"nút Chốt trận một chiều, tự tắt ⇒ không có lần bấm thứ hai"* **mâu thuẫn trực tiếp** với hướng này. Đúng phải là: nút **tắt trong lúc một vòng đang chạy** *(gồm `TIE_BREAK`)* và **sống lại khi trận về `LOBBY`**. *"Một chiều"* nghĩa là **mỗi cú bấm chỉ được phân giải một lần**, không phải *"đúng một lần trong đời một trận"*.
- **Ví dụ cuối của `GR-023` trỏ sai và phải sửa**: *"admin chấm Sai nhầm ⇒ đi qua hoàn nguyên (`GR-028`) hoặc điều chỉnh thủ công (`GR-029`)"* — cả hai đều là đường **điểm**, không dùng được cho một vòng không sinh điểm. Đường đúng là **bỏ vòng `TIE_BREAK`**, làm được cả trong lúc vòng chạy lẫn ở `LOBBY` **trước** cú Chốt trận cuối.
- **Sau khi trận `FINISHED` thì hết cửa** — niêm phong áp cho kết quả tie-break y như mọi thứ khác (`QĐ-037`).
- **`TERM-021` bổ sung `TIE_BREAK_RESOLVED`** vào danh sách loại event đã đặt tên. Đây chính là chỗ trống mà `GRR-156` chỉ ra.
- **Nhánh bốc thăm không phải làm lại gì**: `EVENT-027` *(xác nhận kết quả bốc thăm)* là cái sinh ra `TIE_BREAK_RESOLVED` với `method = 'random-draw'`. Các lần bốc trước vẫn nằm nguyên trong log theo `QĐ-011`.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `GRR-156`, và mâu thuẫn `LOBBY` / `FINISHED` giữa `game-rules.md` và `game-state-machine.md`

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
| Mã phòng 6 số · kho đề · cấu hình gốc · **phạm vi no-repeat** | Điểm · event log · ghế đã gán · biên bản · cấu hình **đã đóng băng** |

> **Không phải *"cờ"* no-repeat.** No-repeat **không cấu hình được, không tắt được** — xem `QĐ-044`. Nó là **phạm vi** *(ranh giới tính không-lặp là một contest)*, không phải một công tắc thuộc contest.

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

**No-repeat KHÔNG cấu hình được.** Nó là một **phạm vi**, không phải một **cờ** — không tồn tại giá trị tắt, không cửa nào trong giao diện bật/tắt nó, và không rule nào đọc một tham số điều khiển nó.

*Vì sao không cho tắt.* Tắt no-repeat là mở lại đúng ba thứ mà cả cụm quyết định kho đề dựng lên để chặn: hỏi lại câu đã lộ trên sóng · vô hiệu hoá hàng rào `everPublic` *(`QĐ-071`)* bằng một công tắc trông vô hại · và làm phép suy số trận từ kho đề mất nghĩa. Nếu về sau thật sự cần *"cho phép lặp"*, đó phải là một quyết định riêng có lý do riêng, **không phải một cờ nằm sẵn chờ ai đó bật**.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `Đ-5.2f`, `GRR-085`, `GRR-118`, `U-30`, và vế *"cờ no-repeat"* của `QĐ-039`

### QĐ-071 — Hàng rào `everPublic` gắn với THAO TÁC, không gắn với MỐC THỜI GIAN

**Quyết định.** Bất cứ khi nào một câu được **đưa vào danh sách gán của một trận official** — ở pre-flight trước trận, ở `LOBBY` qua `QĐ-043`, hay bằng bất kỳ đường nào về sau — hàng rào `everPublic` **chạy y hệt**: chặn cứng, **ép được** với **xác nhận hai bước**, vào `AuditLog`.

**`everPublic` là thuộc tính của CÂU, đi theo câu qua import/export.** `usedInContest` thuộc về **contest** nên đặt lại khi nhập vào contest khác; `everPublic` **không** — nó là dấu vết *"câu này đã từng lộ"*, và sự lộ đó không mất đi vì đổi contest.

**Không áp cho trận `practice`**: kho của practice contest **cố ý** là đề public.

**Vì sao.** Hàng rào cũ được viết khi chỉ có **một** cửa vào — pre-flight. `QĐ-043` mở cửa thứ hai, import mở cửa thứ ba. Cả ba quyết định đều đúng trong phạm vi của mình, nhưng chỗ hở giữa chúng cho một chuỗi **ba bước hợp lệ** vô hiệu hoá toàn bộ cơ chế chống rò đề: tạo trận với danh sách sạch → bấm start → ở `LOBBY` thêm câu đã lộ vào. Không ép, không audit, vì hàng rào không được gọi ở đó.

Gắn hàng rào vào **thao tác** thì mọi cửa mở về sau **tự được bảo vệ**, không phải nhớ vá từng cái.

**Đây là bài học của `QĐ-042` lặp lại**: ở đó, kiểm kho đề **một lần trước trận** là sai vì vòng mở ở nhiều thời điểm; lời giải là kiểm **tại từng cửa vào vòng**. Cùng dạng lỗi, cùng cách sửa.

Và là **cùng lỗ hổng với `QĐ-063`** nhìn từ hướng khác: ở đó là *gỡ nhãn public khỏi câu đã lộ*, ở đây là *đưa câu đã lộ vào bằng cửa sau*. Một kết cục, hai đường.

**Hệ quả.** Không viết nhánh mới — cửa của `QĐ-043` và luồng import **gọi lại đúng** hàng rào đã có. Không thêm chỗ chặn cứng nào: `QĐ-003` giữ nguyên **ba** chỗ, vì đây vẫn là **cùng một** hàng rào, chỉ được gọi ở nhiều điểm hơn. Vẫn ép được — chỉ là ép có dấu vết.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `S-21`, `GRR-111`

### QĐ-082 — VCNV chọn theo BỘ; hàng ngang không phải câu độc lập

**Quyết định.** Bốn vế:

1. **Đơn vị chọn tay và đơn vị rút của vòng VCNV là một BỘ**, không phải sáu mục rời. Một bộ gồm: **1 Chướng ngại vật** *(từ khoá ẩn + hình ảnh 5 miếng ghép)* · **4 hàng ngang** · **1 câu ô trung tâm**.
2. **Bốn hàng ngang mang số thứ tự CỐ ĐỊNH trong bộ** — `1` đến `4` — và số đó ứng với **một miếng ghép ở một góc cố định** của hình. Hàng ngang **không hoán đổi được** giữa các bộ, cũng **không hoán đổi được cho nhau** trong cùng một bộ.
3. **Một bộ chỉ khả dụng cho vòng VCNV khi CẢ SÁU thành phần đều chưa dùng.** Mất một thành phần là **vỡ bộ**: bộ đó không mở được vòng VCNV nữa, dù năm thành phần kia còn nguyên.
4. **Pre-flight vòng VCNV kiểm theo bộ nguyên vẹn**, không kiểm theo số câu: cần **≥1 bộ**.

**Vì sao.** Luật gốc nói thẳng, và nói ở hai chỗ: *"Có 4 từ hàng ngang, **cũng chính là 4 gợi ý liên quan đến Chướng ngại vật** mà các thí sinh phải đi tìm"*, và *"4 miếng ghép **tương ứng với** 4 từ hàng ngang ở 4 góc và **được đánh số cố định**"*.

Nghĩa là hàng ngang **không phải câu hỏi độc lập** — nó là **gợi ý của một Chướng ngại vật cụ thể**. Ghép 4 hàng ngang bất kỳ với một Chướng ngại vật bất kỳ thì vòng vẫn **chạy trót lọt về mặt kỹ thuật** — đủ câu, đủ điểm, đủ miếng ghép — nhưng **trò chơi mất nghĩa**: không còn gì để suy ra, và băng điểm 60/50/40/30 thưởng cho việc *suy luận sớm* trở thành thưởng cho việc *đoán mò*.

**Đây là chỗ đặc tả từng thiếu.** `GR-031` chỉ mô tả **một** cơ chế duy nhất — *"server rút ngẫu nhiên trong danh sách đã gán"* — và cụm *"gợi ý liên quan"* của luật gốc **không xuất hiện ở bất kỳ tài liệu đặc tả nào**. Một hiện thực đọc đúng `GR-031` mà không đọc luật gốc sẽ rút 4 hàng ngang rời nhau, và **không phép kiểm nào bắt được lỗi đó**.

**Hệ quả.**

- **`GR-031` có đúng một ngoại lệ**: ở vòng VCNV, phép rút chọn **một bộ**, rồi dùng trọn sáu thành phần của bộ đó. Bốn vòng kia rút **từng câu** như cũ.
- **Số câu không đổi.** Một bộ = 5 câu *(4 hàng ngang + 1 ô trung tâm)* + 1 Chướng ngại vật. Sàn một trận vẫn là **69 câu + 1 bộ**; chỉ **đơn vị chọn** đổi, không phải con số.
- **Nối thẳng với `QĐ-081` vế 3.** Đây chính là lý do kho VCNV xếp **cuối** trong thứ tự ưu tiên rút của Câu hỏi phụ: mượn một hàng ngang ra làm câu tie-break là **vỡ nguyên một bộ** — mất 6 thành phần để lấy 1. Giao diện phải **cảnh báo** khi admin chỉ định một hàng ngang làm Câu hỏi phụ, và nói rõ bộ nào sẽ vỡ.
- **Người ra đề soạn theo bộ**, không soạn hàng ngang rời. Kho VCNV là **kho các bộ**, không phải kho câu.
- **Import/export đi theo bộ** — tách một hàng ngang khỏi bộ khi xuất là tạo ra dữ liệu không dùng được ở nơi nhận.

*Nguồn*: luật gốc §Vượt chướng ngại vật, hai câu đầu · `[SUY RA]` từ chính hai câu đó · *Thay cho*: chỗ trống trong `GR-031`

### QĐ-084 — BỐN loại gói xuất, có đủ ở CẢ HAI hồ sơ triển khai

**Quyết định.** Sản phẩm xuất được **bốn** loại gói, và **cả hồ sơ máy chủ lẫn hồ sơ portable đều có đủ bốn** — đây là chức năng của lõi, không phải tính năng riêng của bản có Internet.

| | Gói | Nội dung | Chiều |
|---|---|---|---|
| **X1** | **Gói contest** | Câu hỏi · metadata media · media theo vòng · cấu hình luật · **danh sách người tham gia đã mã hoá** (`QĐ-086`, tuỳ chọn) | **Ra và vào** — nhập thành contest **nháp** |
| **X2** | **Toàn bộ kết quả + nhật ký sự kiện** | Mọi trận của contest: nhật ký sự kiện đầy đủ, điểm, thứ hạng, hoàn nguyên, người bấm, lý do | **Chỉ ra** |
| **X3** | **Bản kê câu đã dùng** | `questionId` · `usedAt` · `matchId` | **Ra và vào** — hợp vào cờ đã dùng |
| **X4** | **Kết quả rút gọn** | Bảng điểm cuối · thứ hạng · người thắng, theo từng trận | **Chỉ ra** |

**Chỉ X1 và X3 nhập lại được.** Không có đường mang **kết quả** từ portable về máy chủ trung tâm — đó là **ranh giới chấp nhận có chủ đích**, không phải chỗ thiếu đặc tả. Thứ duy nhất cần quay ngược là *"câu nào đã lộ"*, và X3 gánh trọn việc đó.

**Một nguồn, ba phép chiếu.** X2, X3, X4 **đều sinh từ nhật ký sự kiện**; X3 và X4 là **phép chiếu của X2**, không phải ba đường sinh dữ liệu độc lập. Chỉ X1 đọc nguồn khác — cấu hình contest và kho đề — vì contest là **bản thiết kế**, không phải lần chạy (`QĐ-039`).

**Vì sao ràng buộc "một nguồn".** Ba đường tính riêng nghĩa là **ba** chỗ tính điểm và **hai** chỗ định nghĩa *"đã dùng"*. Đủ để bốn gói nói ba con số khác nhau về cùng một trận, mà không phép kiểm nào bắt được.

**X3 là phép HỢP, không phải ghi đè.** Nhập bản kê chỉ đặt cờ `usedInContest` từ `false` sang `true`, **không bao giờ** ngược lại — vì `TERM-048` khai cờ này **một chiều vĩnh viễn**. Chiều `true → false` chính là lỗ hổng chống rò đề mà `GRR-111` đã chỉ ra với `everPublic`. Đổi lại được ba tính chất miễn phí: **idempotent** *(nhập lại cùng file là no-op)*, **trộn được nhiều nguồn** *(hai máy portable, nhập theo thứ tự nào cũng ra một kết quả)*, và **không tồn tại ca "nhập nhầm file làm mất cờ"**.

**Đánh dấu "đã dùng" hàng loạt bằng tay là ĐƯỜNG VÀO THỦ CÔNG của cùng cơ chế đó** — không phải một đường ghi thứ hai. Admin lọc trong danh sách gán, chọn nhiều, xác nhận; nhập X3 chỉ là *cùng thao tác ấy nhưng danh sách do máy điền sẵn*. Nhờ vậy khi file hỏng vẫn còn đường tay, và cờ `usedInContest` chỉ có **một** cửa ghi.

**Ranh giới đáp án — X2 là một đường rò đề nếu không gác.** Ba biện pháp **bắt buộc**:

- **X2 xuất theo `questionId`**, chỉ nhúng nội dung câu khi người xuất có quyền đọc kho đề;
- **mỗi lần xuất X2 vào `AuditLog`** — nó là một lần *"xem đáp án"* ở quy mô lớn;
- **X4 không chứa đáp án, và không chứa cả `questionId`** — chỉ điểm, thứ hạng, người thắng.

**Thời điểm xuất.** X4 **chỉ từ trận đã `FINISHED`** — chưa chốt thì chưa có thứ hạng (`GR-022`), gọi là *"kết quả"* là sai. X2 xuất được **mọi lúc**, kể cả giữa trận, nhưng gói phải đóng dấu **"trận chưa đóng sổ"**.

**Ranh giới với biên bản PDF (`QĐ-077`).** Biên bản là **một trận, cho người đọc**, in theo lần chạy kèm nhãn *đã bỏ / đã chạy lại / kết thúc sớm*. X2 và X4 là **cấp contest, cho máy đọc**, gộp mọi trận. Không cái nào thay cái nào — nhưng **sinh từ cùng nguồn**, nếu không biên bản và gói sẽ trôi khỏi nhau.

**Điều kiện cần: định danh câu ổn định.** X3 ghép được và X2 đọc lại được **chỉ khi** `questionId` sống sót qua xuất → nhập của X1. Gói contest **mang định danh ổn định của câu** và nhập **giữ nguyên**. Điều này vốn đã ngầm cần cho `QĐ-071` *(`everPublic` đi theo câu qua import/export)*, nhưng chưa từng được viết ra.

**Hệ quả.**

- **Rủi ro *"không có đường mang kết quả về"* đổi hạng**: từ *"chưa quyết"* thành **ranh giới chấp nhận có chủ đích**, cùng loại với `RISK-009` — nên nó **không còn là một mục rủi ro mở** và đã rời bảng `PRD.md` §18. *(Mã `RISK-010` trong bảng hiện hành là một rủi ro khác — dữ liệu cá nhân lộ qua vật mang, `QĐ-086`.)*
- **Thống kê ghi ngược kho đề (`PRD-REQ-081`) chỉ áp cho trận chạy trên CÙNG bản cài.** Trận chạy trên portable đóng góp đúng **một bit** *"đã dùng"*, không đóng góp số liệu. Đây là cái giá đã biết của việc bỏ đồng bộ kết quả.
- **X2 là van thoát của hạn lưu trữ.** `QĐ-077` yêu cầu job dọn không đụng biên bản đã xuất; với X2 câu đó có nghĩa vật lý — gói đã xuất nằm **ngoài** hệ thống. Xuất trước khi hết hạn thì bằng chứng phân xử còn nguyên mà dữ liệu trong máy vẫn dọn đúng hạn.
- **Nhập X3 phải có bản xem trước ba nhóm** trước khi cho bấm: *sẽ chuyển sang đã dùng* · *đã ở trạng thái đó, bỏ qua* · **không thuộc danh sách gán của contest đích** — nhóm thứ ba **báo rõ và không tự áp**, vì cờ này gắn với contest chứ không gắn với câu.
- **Cả hai đường ghi đều qua dialog Yes/No** (`QĐ-072`, không hoàn tác được) và ghi `AuditLog` kèm nguồn là *tay* hay *bản kê nào*. **Không** sinh `MatchEvent` — đây là thao tác cấp contest, không thuộc trận nào.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: câu hỏi mở **gói kết quả chiều ngược** *(nay đã đóng)*, `RISK-010`

### QĐ-086 — Gói contest MANG danh sách người tham gia, và danh sách đó LUÔN mã hoá

**Quyết định.** Gói contest (`QĐ-084` X1) có thêm một phần: **danh sách người tham gia**. Bảy vế:

1. **Bật/tắt được khi xuất.** Gói không kèm danh sách vẫn là gói hợp lệ.
2. **Danh sách LUÔN ở dạng đã mã hoá**, kể cả khi nó không mang mật khẩu nào. Không có biến thể *"để nguyên chữ cho dễ đọc"*.
3. **Chìa khoá KHÔNG nằm trong gói.** Người xuất đặt một cụm mật khẩu và truyền qua kênh khác. Người nhập **bắt buộc** nhập đúng cụm đó. Sai cụm mật khẩu, hoặc gói bị sửa một byte ⇒ **từ chối toàn bộ**, không nhập nửa vời. Gói không kèm danh sách thì không hỏi gì.
4. **Cách xử lý mật khẩu là THIẾT LẬP của người xuất**, hai lựa chọn, ghi vào phần mô tả gói để bên nhập biết cách dựng:
   - **(a) Tạo mật khẩu mới lúc nhập, kèm phiếu tài khoản in được** — mặc định. Mật khẩu cũ **không** rời hệ thống nguồn.
   - **(b) Giữ mật khẩu hiện tại** — tiện hơn cho thí sinh, đổi lại là đưa dấu vết mật khẩu ra khỏi hệ thống.
5. **Chỉ người ĐƯỢC GÁN vào contest** — thí sinh, MC, và **tài khoản admin của contest** (`QĐ-087`). Không bao giờ xuất toàn bộ danh bạ của bản cài.
6. **Trùng tên đăng nhập ở bên nhận thì HỎI, không tự nối** — ba lựa chọn: nối vào tài khoản sẵn có · tạo tài khoản mới có hậu tố · huỷ nhập. Tự nối là trao quyền của một người cho một người khác chỉ vì trùng tên.
7. **Vai tuỳ biến đi kèm.** Vai do đơn vị tự định nghĩa ở bản nguồn phải xuất kèm định nghĩa, nếu không bên nhận dựng lại được tài khoản mà không dựng lại được quyền.

**Vì sao phải có.** Bản có Internet và bản portable là **hai hệ tài khoản độc lập** — người tạo ở bên này không tồn tại ở bên kia, còn ghế thì trỏ tới người. Gói không mang danh sách thì nhập xong **không ai đăng nhập được**, và admin phải gõ lại toàn bộ tài khoản cùng phép gán ghế ngay tại hội trường. Mất gần hết giá trị của chữ *"trọn gói"* trong `GOAL-009`.

**Vì sao mã hoá là bắt buộc chứ không phải tuỳ chọn.** Nội dung này là **dữ liệu cá nhân của học sinh vị thành niên** — tên, trường, lớp — đi trên một chiếc USB. Ở phương án (b) nó còn mang thêm dấu vết mật khẩu, thứ mà `CLAUDE.md` §Zero-trust xếp cao nhất. Để tuỳ chọn nghĩa là sẽ có người tắt nó đúng vào lần cần nhất. Đây cũng là **`RISK-007` ở dạng thứ hai**: không phải lộ qua mã phòng, mà lộ qua vật mang.

**Vì sao (a) là mặc định mà không phải là ép buộc.** (a) an toàn hơn thật — mật khẩu cũ không rời hệ nguồn, nên kể cả lộ cụm mật khẩu gói thì cũng không lộ mật khẩu nào đang dùng ở bản trung tâm. Nhưng (b) là thứ duy nhất cho thí sinh đăng nhập bằng đúng mật khẩu quen, và người xuất là người biết ngày thi của mình phát phiếu được hay không. Nên: mặc định (a), đổi được, và giao diện nói rõ cái giá của từng lựa chọn ngay cạnh chỗ chọn.

**Điểm yếu nhất của sơ đồ là chính cụm mật khẩu do người đặt** — giao diện phải kiểm độ mạnh lúc xuất. Không có cách nào bù chỗ này bằng kỹ thuật.

**Hệ quả.**

- **`EPIC-003` hết mục *out of scope*** về danh sách thí sinh.
- **Xuất và nhập danh sách đều vào `AuditLog`**, tách riêng khỏi lần xuất gói — đây là một lần **dữ liệu cá nhân rời hệ thống**, không cùng hạng với xuất câu hỏi.
- **Tài liệu vận hành phải nói: xoá gói sau ngày thi.** Gói đã ra khỏi hệ thống thì hạn lưu trữ (`QĐ-091`) không với tới được nó.
- **Phiếu tài khoản là hiện vật in ra** — nó thừa hưởng đúng vấn đề của mọi thứ in ra, và tài liệu vận hành phải nhắc thu lại hoặc huỷ.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: câu hỏi mở **danh sách thí sinh trong gói contest** *(nay đã đóng)*

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

### QĐ-070 — Admin mất kết nối là ca của CLIENT, không có cơ chế riêng

**Quyết định.** Admin là **một client như mọi client**. Rớt mạng thì quay lại và **khôi phục từ state của server** — cùng cơ chế với ghế thí sinh (`QĐ-046`). **Không** đóng băng đồng hồ, **không** tự tạm dừng trận, **không** có vai dự phòng tự động.

**Server sập là mất khả năng cứu** — và đó là ranh giới đã chấp nhận, không phải chỗ thiếu đặc tả.

**Vì sao.** Mọi phương án *"máy tự xử khi vắng admin"* đều đụng `QĐ-001`: máy không được tự phán quyết. Đóng băng đồng hồ thì đụng `QĐ-030`. Đường duy nhất còn lại — cũng là đường rẻ nhất — là **coi admin như client** và dựa vào chính cơ chế khôi phục đã có.

**Hệ quả.** Trong lúc admin vắng, **đồng hồ vẫn chạy** và thí sinh vẫn bị khoá theo giờ. Đây là **hệ quả được chấp nhận**, không phải lỗi. Van thoát sau sự cố là **điều chỉnh điểm thủ công** (`GR-029`) hoặc **bỏ / chạy lại vòng** (`GR-030`) — admin xem lại lịch sử rồi quyết.

Vì quyền điều khiển gắn với **phiên** (`QĐ-008`), một tài khoản admin khác **tiếp quản được** khi phiên cũ mất kết nối.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `S-17`

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

**Mở ở `TIE_BREAK`**: nhóm chưa phân định hiện **ĐỒNG HẠNG** — đúng như trạng thái thật lúc đó. Không ẩn bảng, không bịa thứ tự tạm: bảng xếp hạng là **ảnh chụp** của `reduce(event log)`, và ở mốc đó hai người **đang** bằng điểm nhau thật.

**Lớp phủ áp cho MỌI vai, gồm cả máy thí sinh** — điểm vốn đã công khai với mọi vai (`QĐ-015`) nên không lộ thêm gì.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `C-19`, `S-24`, `S-25`, `GRR-032`, `GRR-057`

### QĐ-050 — Banner tạm dừng: chặn toàn cục, KHÔNG CHỮ

**Quyết định.** Lớp phủ chặn **toàn bộ** thao tác trên máy thí sinh và báo tạm dừng trên viewer/overlay. **Không chữ, không lý do.** Máy admin **không bị phủ**. **Chỉ bật được khi không đồng hồ nào đang chạy.**

**Vì sao có ràng buộc đồng hồ, và chiều ngược lại của nó.** Nếu bật được lúc đồng hồ chạy thì thí sinh bị chặn trong khi giờ vẫn trôi. Chiều ngược lại là **suy ra và bắt buộc**: **không start timer được khi banner đang bật** — thiếu nó thì admin cứ mở banner lúc rảnh rồi start timer, đúng cái tình huống muốn tránh.

**Hệ quả.** Không tồn tại thời điểm nào banner và một đồng hồ đang chạy cùng có mặt, nên câu hỏi *"banner có đóng băng đồng hồ không"* **không có chủ ngữ** — `QĐ-030` không cần ngoại lệ.

*Nguồn*: `[CHỦ DỰ ÁN]` + `[SUY RA]` (chiều thứ hai) · *Thay cho*: `C-21`

### QĐ-051 — Đáp án chỉ rời server tới ADMIN và MC **trước mốc công bố**

**Quyết định.** Trước mốc công bố, thí sinh, viewer và overlay **không** nhận đáp án chuẩn. Admin và MC được xem **mọi lúc**, không phụ thuộc cấu hình, mỗi lần xem vào audit.

Từ mốc công bố trở đi thì khác — xem `QĐ-080`, quyết định **mốc công bố là CÂU KHÉP** và mở phạm vi người nhận sang **cả bốn** vai còn lại kể cả overlay.

**Hệ quả.** Mọi kênh đều đi qua cùng một bộ lọc theo vai — kể cả gói khôi phục kết nối (`QĐ-046`) và lớp công bố (`QĐ-049`).

*Nguồn*: `[CHỦ DỰ ÁN]`

### QĐ-080 — Công bố đáp án tại mốc CÂU KHÉP, cho cả overlay

**Quyết định.** Bốn vế, không tách rời được:

1. **Mốc công bố là CÂU KHÉP**, không phải *"đã chấm"*. Câu khép = **không còn ai được trả lời câu đó nữa**. Ở Khởi động, VCNV hàng ngang, VCNV ô trung tâm và Tăng tốc, hai mốc **trùng nhau**. Ở **Về đích** thì không: cú bấm *chấm Sai* của người thi chính **chính là** cú mở cửa sổ cướp quyền (`GR-018`), nên câu chỉ khép **sau khi cửa sổ cướp đóng và người cướp đã được chấm** — hoặc hết 5 giây không ai bấm.
2. **Phạm vi người nhận là cả bốn vai còn lại**: thí sinh · viewer · **overlay**. Lệnh cấm tuyệt đối với overlay **bị gỡ**.
3. **`revealAnswerAfterJudge` giữ nguyên là cờ cấp trận, nhưng mặc định đổi thành BẬT cho cả `official` lẫn `practice`.** Admin tắt được cho từng trận. **Không đường nào vòng qua cờ** — kể cả câu bị bỏ qua ở vế 4.
4. **Ba ca biên:**
   - **Câu bị bỏ qua** *(Khởi động lượt chung, hết 3 giây không ai bấm chuông — `GR-005`)*: câu **chưa từng được chấm** nhưng **đã tiêu**. **VẪN công bố.**
   - **Phán quyết *Huỷ kết quả*** (`QĐ-061`): **KHÔNG** tự công bố.
   - **Chướng ngại vật**: **KHÔNG** theo cơ chế này. Đáp án Chướng ngại vật giữ nguyên `GR-012` — lộ khi có người giải đúng, hoặc khi admin bấm công bố.

**Vì sao.**

*Vì sao mốc là **câu khép** chứ không phải **đã chấm**.* Ở Về đích hai mốc lệch nhau, và lệch đúng vào chỗ đắt nhất trận. Công bố tại *chấm Sai* nghĩa là: A sai ⇒ đáp án hiện lên mọi màn hình ⇒ B bấm chuông trong 5 giây, đọc lại thứ vừa thấy ⇒ **B +30, A −30**. Cướp quyền là cơ chế chuyển điểm lớn nhất của vòng quyết định; công bố sớm **xoá sổ nó** và biến vòng thành cuộc thi bấm chuột. Đây là **sai luật gốc**, không phải lệch trải nghiệm.

Chọn *"câu khép"* thay vì *"đã chấm, trừ Về đích"* vì nó **không đẻ nhánh đặc biệt**: mốc này đã có sẵn trong đặc tả — `TERM-031` ghi *"cửa sổ cướp **thuộc về câu và khép theo câu**"*, và `STATE-021 → EVENT-003` chính là nó.

*Vì sao mở cho overlay.* Overlay là kênh đẩy dữ liệu cho phần mềm dựng hình. Công bố đáp án trên sóng sau khi đã chấm là chuẩn của gameshow truyền hình; giữ lệnh cấm tuyệt đối chỉ khiến người dựng hình phải chèn tay đáp án từ một nguồn khác.

*Vì sao `GOAL-005` không bị vi phạm.* Mục tiêu là *"đáp án không bao giờ tới client **trước lúc công bố**"*. Tại mốc câu khép, câu đã hỏi xong và đề đã lộ — không còn gì để giữ. Ràng buộc chỉ **dịch chỗ**: không phải *ai được thấy* mà **đáp án được phép rời server vào lúc nào**.

*Vì sao Huỷ kết quả không tự công bố, trong khi câu bị bỏ qua thì có.* Câu bị bỏ qua là một kết cục **bình thường** của luật — MC đọc đáp án lên là việc thường ngày. *Huỷ kết quả* thì gần như luôn gắn với **tình huống sự cố đang được xử lý**: câu chỉ có bản gửi quá hạn, hoặc câu đang mở bị khép vì kết thúc vòng khẩn cấp (`QĐ-034`). Tự đẩy đáp án lên sân khấu giữa lúc admin đang chữa sự cố là gây nhiễu. Admin vẫn **mở tay được** theo `QĐ-048`.

*Vì sao Chướng ngại vật đứng ngoài.* Đáp án Chướng ngại vật không thuộc một câu hỏi nào — nó là đích của cả vòng. Cuốn nó theo quy tắc công bố tự động sẽ **kết thúc vòng sớm** ngay khi hàng ngang đầu tiên khép.

**Hệ quả.**

- **Ràng buộc kỹ thuật bắt buộc**: server chỉ đẩy đáp án **tại đúng mốc công bố**. **Cấm** đẩy đáp án xuống client sớm rồi ẩn bằng một cờ hiển thị. Đây là **lỗi của tiền lệ Athena** — nó nạp toàn bộ ngân hàng đề xuống mọi máy client, mã hoá bằng phép dịch ký tự, và chỉ dựa vào một cờ `Visibility` của giao diện; đáp án nằm sẵn trong bộ nhớ máy thí sinh suốt trận. Đó đúng là `PS-6`.
- **Công bố là một chiều ở phía engine**: đã công bố thì engine không tự thu lại. Quyền **đóng thủ công** của admin giữ nguyên theo `QĐ-048`.
- `INV-017` mất vế *"overlay không bao giờ"*; `GR-008` mất vế *"không thứ gì tự lộ"* đối với **đáp án chuẩn** — vế đó vẫn đúng cho **bài làm của thí sinh khác**.
- Ba loại thông tin nay có ba chế độ mới: **đáp án chuẩn** = mật tới mốc câu khép, sau đó công khai theo cờ · **bài làm của thí sinh khác** = ẩn tạm thời, lộ khi admin bấm · **điểm số** = công khai luôn luôn.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: vế *"overlay không bao giờ nhận đáp án"* của `QĐ-051`, và mặc định `official ⇒ TẮT` của `QĐ-062`

### QĐ-072 — Ba hạng cảnh báo giao diện, phân biệt bằng thứ chúng bảo vệ

**Quyết định.** Đúng **ba** hạng, không hơn:

| Hạng | Dùng khi | Hình thức | Bắt lý do |
|---|---|---|---|
| **Toast** | Thao tác ở **invalid state** — không tồn tại đường thực hiện | Thông báo trôi, **nói rõ vì sao** | Không |
| **Dialog Yes/No** | Thao tác **không hoàn tác được** nhưng hợp lệ — mở đáp án, xác nhận tín hiệu, điều chỉnh điểm | Hộp thoại hai nút | Chỉ điều chỉnh điểm |
| **Dialog phá huỷ** | Bỏ vòng · chạy lại vòng · kết thúc sớm · huỷ trận | **Không tắt được** bằng `Esc` hay click ra ngoài | **Có** |

**Thao tác ĐÓNG hiển thị không có dialog.** Dialog chống thứ **không thu hồi được**; đóng lại thì mở lại được và không lộ thêm gì.

**Cảnh báo lệch luật là dialog Yes/No, không bắt lý do.** Nó **nêu tên luật đang bị lệch** và **không tự đóng** — admin có thể đang nhìn chỗ khác.

**Vì sao.** Toast và dialog trông giống nhau với người dùng nhưng khác nhau ở chỗ **ép được hay không**: toast là *"không có đường này"*, dialog là *"có đường, bạn chắc chưa"*. Trộn hai thứ thì admin học sai mô hình và sẽ hoảng khi gặp toast giữa trận.

Mỗi loại invalid state phải có **thông điệp riêng**. Một câu chung chung giữa lúc đang chạy chương trình thì vô dụng.

**`Esc` ở màn thí sinh mode sân khấu**: **không làm gì** — ba vòng đó không có ô nhập để xoá, và `Esc` không bao giờ là nút quay lại ở màn thi đấu.

*Nguồn*: `[SUY RA]` từ `QĐ-004`, `QĐ-005` · *Thay cho*: `Q-A1`, `S-3`, `S-7`, `S-18`

### QĐ-073 — Màn chấm của admin

**Quyết định.**

- **Tô so với đáp án GẦN NHẤT** trong `acceptedAnswers` — ít khác biệt nhất — và cho xem **cả danh sách**. Tô theo đáp án đầu sẽ báo động giả khi thí sinh dùng một biến thể hợp lệ khác.
- **Giữ `wordCount`** như gợi ý phụ: nguồn có ngoại lệ *"cùng tổng số chữ cái"*, admin cần con số đó để áp.
- **Tăng tốc chấm trên MỘT màn, cả bốn ghế cạnh nhau** — xếp hạng là phép tính trên toàn bảng (`QĐ-013`), chấm từng người một thì admin không thấy được thứ mình đang xếp.
- **Admin xem được lịch sử bài gửi** của một ghế, không chỉ bản đang tính. Cần cho phân xử khiếu nại; và `QĐ-029` đã giao cho admin quyền sửa, nên phải cho admin dữ liệu để sửa đúng.
- **Bản hợp lệ và bản quá hạn hiện CẠNH NHAU**, bản quá hạn tô đỏ, và **nút chấm bật được cho cả hai**. Khoá cứng bản quá hạn là lấy mất quyền phán quyết mà `QĐ-010` đã giao.

*Nguồn*: `[SUY RA]` từ `QĐ-010`, `QĐ-013`, `QĐ-029` · *Thay cho*: `Q-B2`, `Q-B4`, `Q-B5`, `S-8`, `S-9`

### QĐ-074 — Thao tác tay của admin THẮNG mọi cờ tự động

**Quyết định.** Admin bấm mở đáp án ⇒ đáp án ra tới **mọi vai đang xem**, kể cả khi `revealAnswerAfterJudge` **TẮT**. Cờ đó là **mặc định tự động**; cú bấm là **quyết định thủ công**, và thủ công thắng. Mỗi lần mở vào `AuditLog`.

**Mở/đóng hiển thị KHÔNG đụng tới điểm.** Hai trục khác nhau: điểm chỉ đổi qua event (`QĐ-011`). Đóng lại một ô đã mở là đổi hiển thị, không hoàn nguyên gì.

**Độc quyền admin** — chính xác hơn: **phiên đang giữ quyền điều khiển** (`QĐ-008`). MC không có nút nào; màn MC là read-only.

**Không có khái niệm "engine tự mở".** Engine không bao giờ tự mở gì, nên `AuditLog` **không cần** phân biệt *do-admin* với *do-engine* — mọi lần mở đều do admin. Chỗ **cần** phân biệt là khác: ô VCNV chuyển sang *đã hỏi* do **luồng** hay do **admin đánh dấu tay** (`GR-009` C12), vì hai đường đó cùng đổi băng điểm.

*Nguồn*: `[SUY RA]` từ `QĐ-048`, `QĐ-051`, `QĐ-062` · *Thay cho*: `Q-A3`, `Q-A5`, `Q-A6`, `Q-A7`

### QĐ-075 — Mode trả lời: chụp vào trận, không thuộc preset luật

**Quyết định.**

- **Mode được CHỤP vào trận lúc start.** Sửa mode ở contest về sau **không** đụng trận đã chạy — cùng khuôn `QĐ-062`. Nhờ vậy sửa mode khi contest đã có trận `FINISHED` là **hợp lệ**, không cần chặn.
- **Mode KHÔNG nằm trong preset `O26_DEFAULT@1`.** Preset chứa **giá trị luật**; mode là **điều kiện sân khấu** — có micro không, thí sinh có bàn phím không. Nút *"Áp dụng luật 2026"* **không đụng** mode.
- **Mode CÓ đi theo contest bundle** export/import, nhưng **sửa được sau khi nhập**: nơi nhận có thể có sân khấu khác.
- **Một mode chung cho cả trận official lẫn trận practice** của cùng contest — mode là thuộc tính của **phòng**, không của **mục đích trận**. Đây là chỗ nó khác `revealAnswerAfterJudge` (`QĐ-062`), và khác **có lý do**, không phải thiếu nhất quán.
- **Biên bản ở mode sân khấu ghi Đúng/Sai, không ghi nội dung** — không có bài làm dạng chữ để ghi. Mode nhập liệu thì ghi cả nội dung.

*Nguồn*: `[SUY RA]` từ `QĐ-016`, `QĐ-017`, `QĐ-062` · *Thay cho*: `Q-C1b`, `Q-C1c`, `Q-C2`, `Q-C3`, `Q-C4`

### QĐ-076 — Viewer KHÔNG được báo về can thiệp của admin

**Quyết định.** Bỏ vòng, chạy lại vòng, sửa danh sách đề, gỡ lệnh cấm — viewer **không thấy thông báo nào**. Điểm và bàn cờ đổi **đột ngột, không hiệu ứng, không giải thích**. Không khoá cổng phòng, không màn chờ.

**Khuyến nghị lượt cũng chỉ tới admin và MC.** Đẩy xuống viewer là lộ **thứ tự sắp tới** — hỏng trình diễn, và trao một lợi thế mà luật không định trao.

**Vì sao.** Viewer xem một **buổi thi**, không xem một **bảng điều khiển**. Mọi thông báo kiểu *"admin vừa bỏ vòng 2"* đều biến sự cố hậu trường thành sự kiện trên sân khấu. Người giải thích chuyện đang xảy ra là **MC**, không phải giao diện.

*Nguồn*: `[SUY RA]` từ `QĐ-035`, `QĐ-051` · *Thay cho*: `S-4`, `S-11`, `S-22`

### QĐ-077 — Biên bản trận ghi theo LẦN CHẠY

**Quyết định.** Một vòng chạy nhiều lần thì biên bản in **nhiều khối**, theo thứ tự thời gian, mỗi khối một **số lần chạy** và một **nhãn**: *đã bỏ* · *đã chạy lại* · *kết thúc sớm* · *hoàn thành*. Không gộp, không giấu lần hỏng.

**Event hoàn nguyên hiện trong biên bản** như mọi event khác — nó là một dòng có tên người bấm và lý do, không phải một phép trừ thầm lặng.

**Xuất biên bản trước khi dọn dữ liệu.** Job dọn theo retention **cảnh báo trước**, và **không** đụng biên bản đã xuất — hiện vật đã xuất nằm ngoài vòng đời của dữ liệu thô.

*Nguồn*: `[SUY RA]` từ `QĐ-011`, `QĐ-035` · *Thay cho*: `S-6`, `S-10`

### QĐ-078 — Thao tác phá huỷ có permission RIÊNG, không mặc định theo vai

**Quyết định.** Bỏ vòng · chạy lại vòng · kết thúc sớm · huỷ trận · điều chỉnh điểm · sửa danh sách đề · ép qua hàng rào `everPublic` — mỗi thứ là **một permission riêng** trong catalog, không suy ra từ *"là admin"*.

**Vì sao.** Đây là các thao tác **đổi được kết quả trận**. Zero-trust bắt server kiểm quyền cho **mọi** request; nếu quyền chỉ là *"vai admin"* thì không có cách nào cấp một tài khoản chạy trận mà không đồng thời cho nó xoá vòng. Tách permission cũng là thứ làm cho `QĐ-008` — nhiều tài khoản, một phiên — dùng được thật.

*Nguồn*: `[SUY RA]` từ `QĐ-008` · *Thay cho*: `S-1`

### QĐ-079 — Danh sách `sound-cue` phủ cả sự kiện điều khiển

**Quyết định.** Ngoài các mốc thi đấu, danh sách slot có thêm: **hoàn nguyên · bỏ vòng · chạy lại vòng · kết thúc sớm · ép qua cảnh báo · mở màn công bố**. Slot trống = **im lặng**; không có bộ âm mặc định.

**Vì sao.** Thiếu slot thì không thêm được về sau mà không sửa engine. Có slot mà để trống thì **không tốn gì**.

*Nguồn*: `[SUY RA]` từ `QĐ-049` · *Thay cho*: `S-5`

### QĐ-088 — Hai kênh public nhận đẩy MỘT CHIỀU trên HTTP; kênh hai chiều chỉ cho vai đã xác thực

**Quyết định.** Hai kênh public — **màn khán giả** và **lớp phủ dựng stream** — nhận cập nhật bằng **luồng sự kiện một chiều server → client trên HTTP**, cộng một lời gọi đọc thông thường để lấy ảnh chụp trạng thái lúc vào phòng. Chúng **không** dùng kênh hai chiều.

Kênh hai chiều **chỉ dành cho vai đã xác thực**: admin, thí sinh, MC.

**Vì sao — lý do mạnh nhất là bảo mật, không phải hiệu năng.** Quy tắc hiện hành là *"server drop mọi sự kiện ghi từ kênh viewer"*. Đó là một **luật phải cưỡng chế**, và mọi luật phải cưỡng chế đều hỏng được: thêm một handler, quên một guard, một namespace mới sao chép nhầm cấu hình. Với kênh một chiều, **không tồn tại đường ghi để mà chặn** — tính chất read-only chuyển từ *thứ phải kiểm* thành *thứ không thể vi phạm*. Đây đúng là dạng hàng rào mà `INV-017` và `GR-037` cần: không dựa vào việc nhớ kiểm.

**Vì sao nó cũng tách được câu hỏi quy mô ra khỏi công bằng trận.** Khán giả không còn nằm chung ngân sách kết nối với lõi thi đấu, nên **số khán giả tăng không đụng tới** thứ tự chuông, đồng hồ hay thứ hạng tốc độ — những thứ `GR-035` và `INV-004` bảo đảm. Trước đây một phòng đông là một rủi ro công bằng; nay nó chỉ còn là một câu hỏi định cỡ.

**Nói cho đúng: việc này KHÔNG làm số khán giả thành miễn phí.** Mỗi người xem vẫn giữ **một kết nối mở** tới server. Nó rẻ hơn đáng kể và đặt sau proxy được, nhưng nó không phải bằng không. Điều đổi được là **hạng** của con số: từ một **cam kết sản phẩm** xuống một **giá trị định cỡ triển khai**. `QĐ-067` vì thế vẫn đứng nguyên — con số vẫn là mặc định cấu hình được, chỉ khác là nay đã biết rõ nó không chạm vào luật chơi.

**Hệ quả.**

- **`CLAUDE.md` §Stack phải nói rõ phạm vi**: kênh socket hai chiều dành cho vai đã xác thực; hai kênh public đi đường HTTP một chiều.
- **Lớp phủ vẫn nhận đáp án từ mốc câu khép** (`QĐ-080`) — chiều truyền không đổi cái gì được truyền. Kênh một chiều **đẩy được** đáp án đúng lúc; nó chỉ không nhận vào.
- **Nút *"khoá cổng"*** (`PRD-REQ-086`) áp ở tầng vào của kênh public, không đổi.
- **Không đụng tới việc nạp trước media mã hoá** (`QĐ-012b`) — đó là đường tải nội dung, không phải đường sự kiện.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: vế *"quy mô viewer có ảnh hưởng trận không"* của câu hỏi mở **quy mô viewer** *(nay đã đóng)*

### QĐ-090 — BỎ HẲN tuỳ chọn biệt danh

**Quyết định.** v1 **không có** tuỳ chọn hiển thị biệt danh thay tên thật. Các kênh public hiện đúng **tên hiển thị của ghế** như người dựng contest đã nhập.

**Vì sao.** Nó không mua được thứ nó hứa. Khán giả tại chỗ **nhìn thấy mặt và biết tên** thí sinh; che tên trên màn hình không giấu được ai với người đang ngồi trong hội trường, và cũng không giấu được với người xem stream vì MC đọc tên bằng lời. Đổi lại, giữ nó là thêm một trạng thái hiển thị phải kiểm ở **cả bốn** kênh (khán giả, lớp phủ, MC, thí sinh) và thêm một chỗ để lệch nhau.

**Đường giảm thiểu thật thì vẫn còn, và nó không phải một tính năng.** Tên hiển thị của ghế là **trường tự do**. Người tổ chức muốn để *"Minh A."* thay tên đầy đủ thì gõ đúng như thế lúc dựng contest. Đó là quyết định của người nhập liệu — chỗ duy nhất biết được bối cảnh — chứ không phải một công tắc của hệ thống.

**Hệ quả.**

- **Yêu cầu *"tuỳ chọn dùng biệt danh"* bị xoá khỏi `PRD.md`**, và `EPIC-011` bỏ vế đó khỏi phạm vi. Mã `PRD-REQ-083` được **dùng lại** cho hạn lưu trữ (`QĐ-091`), theo quy ước đánh số lại cho liền của `PRD.md`.
- **`RISK-007` mất một hướng giảm thiểu.** Còn lại: giới hạn tần suất, nút khoá cổng phòng, **và** khuyến nghị đặt tên hiển thị rút gọn trong tài liệu vận hành. Mức tác động của rủi ro **không đổi** — hướng giảm thiểu cũ vốn đã yếu, việc bỏ nó chỉ làm hồ sơ rủi ro **nói thật hơn**.
- **`ASSUMPTION-005` phải viết lại**: căn cứ cũ dẫn chính tuỳ chọn này.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: câu hỏi mở **biệt danh có bật mặc định không** *(nay đã đóng)*

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

*Nguồn đề của vòng này*: xem `QĐ-081` — **không có kho Câu hỏi phụ riêng**.

### QĐ-081 — Câu hỏi phụ KHÔNG có kho riêng; rút từ ba kho nguồn

**Quyết định.** Năm vế:

1. **Không tồn tại kho Câu hỏi phụ.** Đề của vòng này rút từ **ba kho nguồn**: **Về đích · Khởi động · VCNV**, trong danh sách đã gán của trận, loại câu đã dùng như mọi vòng khác.
2. **Kho Tăng tốc KHÔNG phải nguồn.**
3. **Thứ tự ưu tiên rút: Về đích → Khởi động → VCNV.**
4. **Câu mượn bị vô hiệu hoá bốn thứ metadata:** `timeSeconds` **bỏ qua**, luôn dùng **15 giây** · `value` **bỏ qua**, vòng này không sinh điểm · câu `isPractical` **bị loại khỏi phép rút** · phân loại theo vòng của kho gốc **không mang theo ý nghĩa nào**.
5. **Admin chỉ định một số câu làm Câu hỏi phụ được — TẠI `LOBBY`.** Bốn ràng buộc: chỉ nhận câu **còn available** *(chưa hiển thị)* · làm được ở **bất kỳ `LOBBY` nào**, hạn chót là **mốc bấm Chốt trận** · **gỡ chỉ định được**, đối xứng · chỉ định là **ĐẶT CHỖ có hiệu lực TỪ THỜI ĐIỂM CHỈ ĐỊNH**: câu đó bị loại khỏi phép rút của **các vòng mở sau đó**. Chỉ định **ít hơn 3** câu ⇒ phần thiếu **bù theo thứ tự ưu tiên** ở vế 3. Không chỉ định gì ⇒ lui hẳn về vế 1.

**Vì sao bỏ kho riêng.** Một kho khai riêng cho một vòng **có thể không bao giờ chạy** là chi phí đặt sai chỗ: nó bắt admin chuẩn bị đề cho một nhánh điều kiện, và tạo ra một cửa chặn cứng thứ tư trên thực tế. Ba kho nguồn đã có sẵn câu đúng khuôn cần dùng.

**Vì sao ba kho đó, và vì sao KHÔNG Tăng tốc.** Câu hỏi phụ là **một người giành quyền bằng chuông**, cửa sổ **15 giây**, và ở mode sân khấu thì **trả lời bằng miệng**. Ba kho nguồn chứa câu trả lời được bằng một câu nói. Kho Tăng tốc thì không: câu ở đó là *nhìn nhanh · sắp xếp · suy luận · đoạn băng*, **cả sân cùng gõ máy**, tính điểm theo **thứ hạng tốc độ**. Câu *sắp xếp* đặt vào cửa sổ tranh chuông là hỏng — kéo thả không xong kịp, và ở mode sân khấu thì **không có gì để nói**.

**Vì sao thứ tự ưu tiên là Về đích trước.** Kho Về đích **luôn dư đúng 12 câu** sau khi vòng chạy xong: pre-flight đòi 24 *(12 mức 20 **và** 12 mức 30, vì bốn thí sinh có thể cùng chọn một mức)* nhưng vòng chỉ tiêu **12**. Số dư này là **cấu trúc, tất định, không phụ thuộc lựa chọn của ai** — nên nó là nguồn đúng cho một nhánh điều kiện. Kho VCNV thì ngược lại, **khan nhất**: mỗi trận cần **một bộ** gồm Chướng ngại vật, 4 hàng ngang và câu ô trung tâm, và **rút một hàng ngang ra làm câu hỏi phụ sẽ LÀM VỠ cả bộ** — bộ thiếu gợi ý không dùng cho vòng VCNV của trận sau được nữa. Không có thứ tự này thì một trận hoà âm thầm ăn mất bộ VCNV của trận kế.

**Vì sao chỉ định là ĐẶT CHỖ chứ không phải danh sách ưu tiên.** Nếu câu được chỉ định vẫn nằm trong phép rút của vòng gốc, nó có thể bị vòng gốc tiêu mất và hệ thống **âm thầm bỏ qua chỉ định của admin** — đúng thứ `QĐ-074` cấm.

**Vì sao chỉ định đặt ở `LOBBY` chứ không ở cấu hình trận.** Cấu hình trận **đóng băng tại cú bấm bắt đầu trận** (`QĐ-075`). Đặt tính năng ở đó buộc admin chọn câu tie-break **trước khi trận chạy** — trước khi biết có hoà không, và trước khi biết câu nào còn lại. Đặt ở `LOBBY` thì nó **đi nhờ ngoại lệ sẵn có của `QĐ-043`**, không tạo ngoại lệ mới cho quy tắc đóng băng.

**Vì sao chi phí là hàm của THỜI ĐIỂM, không phải của tính năng.** Đặt chỗ chỉ tốn khi còn vòng nào **mở sau đó** muốn rút vào cùng kho:

| Chỉ định ở | Vòng còn lại sau đó | Chi phí |
|---|---|---|
| `LOBBY` trước Về đích | Về đích | **+k** vào kho Về đích |
| **`LOBBY` cuối**, sau khi Về đích đã chạy | **không còn vòng nào** | **0** |

Ở `LOBBY` cuối, kho Về đích đang có **đúng 12 câu dư** mà không vòng nào sẽ đụng tới. Chỉ định 3 trong 12 ⇒ **không lấy mất của ai**. Đây cũng là thời điểm **tự nhiên nhất**: admin nhìn bảng điểm, thấy có hoà, rồi chọn ba câu hợp khuôn tranh chuông. ⇒ Chi phí `+k` **tránh được hoàn toàn**, không phải cái giá bắt buộc.

**Vì sao chỉ nhận câu còn available.** Không có ràng buộc này, admin chỉ định một câu **đã hiển thị** ⇒ vòng Câu hỏi phụ hỏi lại **một câu đã lộ**, phá quy tắc không lặp câu. Ranh giới đúng là **đã hiển thị**, cùng ranh giới `GR-031` C6 dùng để cấm gỡ câu đã tiêu khỏi danh sách gán.

**Vì sao thiếu thì bù chứ không chặn.** Chỉ định 1 câu rồi chặn cứng vì chưa đủ 3 là quá tay — hệ thống chỉ có **ba** chỗ chặn cứng (`QĐ-003`) và đây không thuộc ba chỗ đó. Bù theo thứ tự ưu tiên giữ đúng khuôn advisory.

**Hệ quả.**

- **Sàn kho đề của một trận KHÔNG tăng.** Trước quyết định này, một trận chuẩn cần **69 câu + 3 câu phụ = 72**. Nay là **69**, và nhánh Câu hỏi phụ được phục vụ bởi 12 câu Về đích vốn đã bắt buộc phải có. Với N trận: dư Về đích tại mọi thời điểm là `24N − 12j ≥ 12`, nên **mọi trận đều có đủ đề cho tie-break**. Vế 5 **không** làm sàn này tăng, vì chỉ định ở `LOBBY` cuối có chi phí **0**.
- **Chặn cứng ở cửa vào vòng vẫn giữ** — nó vẫn là *"cửa vào vòng thiếu câu"*, nên `QĐ-003` giữ nguyên con số **ba**. Nhưng nó **gần như không còn chạm tới**: chỉ xảy ra khi admin chủ động gỡ hết câu dư ở `LOBBY`, hoặc chỉ định một danh sách rồi để nó cạn.
- **Ngoại lệ duy nhất của quy tắc "per-question override thắng preset"** (`TERM-045`) nằm ở vế 4: 15 giây của Câu hỏi phụ là **cố định, không cấu hình** (`GR-023`), nên nó thắng ngược lại `timeSeconds` của câu. Hai quy tắc này đá nhau ở đúng ca mượn kho, và vế 4 phân xử.
- **Câu mượn vẫn tiêu như mọi câu khác** — cờ đã-dùng bật tại mốc hiển thị, không lặp lại trong contest.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: vế *"kho câu phụ riêng"* của `QĐ-055` và `GR-022`

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

# L. Mô hình dữ liệu và quyền

### QĐ-062 — `revealAnswerAfterJudge` là cờ CẤP TRẬN

**Quyết định.** Cờ này thuộc **match**, không thuộc contest. Admin đổi được cho từng trận.

> **Mặc định đã đổi** (`QĐ-080`): nay **BẬT cho cả `official` lẫn `practice`**. Trước đây `official` mặc định tắt. Vế *"cờ ở cấp trận"* dưới đây **không đổi** — nó vẫn là lý do duy nhất khiến cờ này không đặt được ở cấp contest.

**Vì sao.** Ba chỗ đã chốt đều chỉ cùng một hướng, và một trong ba khiến phương án per-contest **không thể đúng**: `QĐ-040` cho một **contest thật chứa cả trận official lẫn trận practice**. Nếu cờ đặt ở cấp contest thì trận practice trong contest thật **không bật được** — mất đúng công dụng của nó. Thêm nữa `QĐ-051` khai mặc định **theo `matchPurpose`** (vốn per-match), và `QĐ-032` liệt cờ này vào gói **đóng băng vào TRẬN**.

**Hệ quả.** Phát biểu *"contest bật"* trong `PRD` là **câu chữ lạc hậu**, phải sửa — không phải một cách đọc thay thế.

*Nguồn*: `[SUY RA]` từ `QĐ-032`, `QĐ-040`, `QĐ-051` · *Thay cho*: `C-1`, `K-4`

### QĐ-063 — `Question.visibility` là giá trị DẪN XUẤT, không phải cột set tay

**Quyết định.** `visibility` = `PUBLIC` **khi và chỉ khi** câu đang thuộc ≥1 bộ đề public. **Read-only**, không ai đặt được trực tiếp. Setter làm một câu thành public bằng cách **đưa nó vào một bộ đề public**, không bằng cách bật một cờ.

**Vì sao.** Nếu `visibility` set tay được thì một người **gỡ được nhãn public khỏi câu đã lộ** — vô hiệu hoá chính hàng rào chống rò đề mà `everPublic` tồn tại để dựng, bằng một thao tác trông hoàn toàn hợp lệ. Zero-trust cấm để con người tắt một dấu vết bảo mật.

**Hai cờ, hai vai, đừng gộp:**

| Cờ | Loại | Dùng để |
|---|---|---|
| `visibility` | **Trạng thái hiện tại**, dẫn xuất | Hiển thị và lọc trong kho đề |
| `everPublic` | **Dấu vết lịch sử**, một chiều | Pre-flight **hard-block** trận official |

Pre-flight chặn theo **`everPublic`**, không theo `visibility` — vì thứ nguy hiểm là *"đã từng lộ"*, không phải *"đang lộ"*.

*Nguồn*: `[SUY RA]` từ `QĐ-040` + zero-trust · *Thay cho*: `C-10`, `K-10`, `D16` (vế *"cột set tay"* bị thay thế)

### QĐ-064 — Duyệt đề `DRAFT` → `ACTIVE` là việc của ADMIN

**Quyết định.** Vai **admin** duyệt; hàng chờ duyệt nằm trên dashboard admin. **Không có vai reviewer riêng.**

**Vì sao.** `CLAUDE.md` §UX đã khai thẳng hàng chờ này thuộc dashboard admin, và danh sách vai của hệ thống không có vai nào khác đảm nhiệm được.

**Phân biệt với `QĐ-008`.** `QĐ-008` nói *"mỗi contest một admin"* — đó là **admin của một trận đang chạy**. Duyệt đề diễn ra ở **kho đề**, vốn nằm **ngoài** phạm vi một contest, nên ràng buộc một-người không áp ở đây.

*Nguồn*: `CLAUDE.md` §UX · *Thay cho*: `C-3`

### QĐ-065 — `User` là TÀI KHOẢN; một tài khoản giữ được nhiều vai, trừ một ràng buộc loại trừ

**Quyết định.** `User` = tài khoản xác thực. **Một tài khoản giữ được nhiều vai** — *admin* và *setter* là cặp thường gặp nhất, và ở buổi thi nhỏ một người có thể vừa nói vừa bấm.

**Ràng buộc loại trừ, bắt buộc kiểm ở server:** tài khoản đang ngồi **ghế thí sinh** của một trận **không được** đồng thời giữ vai **admin**, **MC**, hoặc **setter** trong contest đó.

**Vì sao.** `QĐ-051` cho **admin và MC thấy đáp án**. Một thí sinh kiêm một trong hai vai đó là **gian lận có cấu trúc** — không phải rủi ro vận hành mà là một lỗ hổng do mô hình quyền để hở. Đây là ràng buộc **kiểm được bằng dữ liệu**, nên phải kiểm.

**Hệ quả.** Ràng buộc gắn với **contest**, không với hệ thống: cùng một người có thể là thí sinh ở contest này và admin ở contest khác.

**Setter kiêm MC là ĐƯỢC PHÉP.** Cả hai vai đều vốn đã thấy đáp án, nên ghép chúng **không lộ thêm gì** — khác hẳn ca thí sinh ở trên. Xung đột lợi ích *"người ra đề dẫn trận dùng đề của mình"* là **rủi ro quy trình**, và ở quy mô một trường thì cấm nó thường đồng nghĩa với không tổ chức được. Hệ thống **cảnh báo** ở cửa gán vai và ghi `AuditLog`, **không chặn** — đúng mô hình advisory (`QĐ-002`).

**Vai hệ thống ≠ vai vận hành.** `User` mang **vai hệ thống** (tài khoản có gì trong catalog); *MC*, *trainer*, *host* là **quyền gán theo contest**, không phải vai seed. Trộn hai khái niệm là cách nhanh nhất để đếm sai số role và cấp thừa quyền.

*Nguồn*: `[SUY RA]` từ `QĐ-051` + `CLAUDE.md` §Mô hình truy cập · *Thay cho*: `A-12`

### QĐ-066 — Ba kiểu NHẬP đáp án; đáp án luôn là CHUỖI

**Quyết định.** Thêm hai trường vào `Question`:

| Trường | Giá trị | Dùng để |
|---|---|---|
| `answerInputKind` | `text` · `choice` · `ordering` | Quyết định **widget nhập** trên máy thí sinh |
| `options[]` | danh sách phương án | Nội dung để render, cho `choice` và `ordering` |

**Đáp án và bài làm vẫn là CHUỖI ở cả ba kiểu**: câu lựa chọn lưu `"B"`, câu sắp xếp lưu `"B, D, A, C"`.

**Vì sao — `QĐ-010` làm bài toán nhỏ đi rất nhiều.** Máy **không chấm**, nên nó **không cần đánh giá** một thứ tự hay một lựa chọn; nó chỉ cần **hiển thị bài làm cạnh đáp án** để admin phán quyết. Serialise về chuỗi thì cơ chế **tô khác biệt ký tự** chạy nguyên và **không đẻ ra nhánh chấm mới** — câu sắp xếp `BDCA` so với đáp án `BDAC` cho ra highlight đúng hai vị trí bị hoán.

**Vì sao chỉ hai kiểu mới, không phải bảy.** Luật gốc liệt kê 3 loại ở Khởi động và 4 loại ở Tăng tốc, nhưng *nhìn nhanh · suy luận · đoạn băng · hình ảnh · đoạn nhạc* khác nhau ở **nội dung và media**, không ở **cơ chế trả lời** — tất cả đều gõ một chuỗi. Chúng là **phân loại cho người soạn đề**, không phải nhánh của engine, nên thuộc về trường mô tả chứ không cần trường điều khiển.

**Hệ quả.** `isPractical` là kênh trả lời **thứ tư**, nằm ngoài trục này — nó không có ô nhập nào, admin chấm *"đạt / không đạt"*.

*Nguồn*: luật gốc §Khởi động (3 loại), §Tăng tốc (4 loại) + `[SUY RA]` từ `QĐ-010` · *Thay cho*: `U-34`, `U-36`, `U-7`

### QĐ-087 — Cài đặt lần đầu: dòng lệnh khi dựng máy, tài khoản admin đi theo gói khi ra hội trường

**Quyết định.** Hai đường, cùng tồn tại, phục vụ hai thời điểm khác nhau:

- **Dòng lệnh — đường nền, luôn có.** Tạo tài khoản admin đầu tiên trên một bản cài trống. Đây là thao tác của người **dựng máy**, làm **trước** ngày thi.
- **Tài khoản admin đi theo gói contest** (`QĐ-086` vế 5) — đường của **ngày thi**. Nhập gói vào bản portable là có sẵn tài khoản điều khiển; không phải mở dòng lệnh ở hội trường.

**v1 KHÔNG làm trình hướng dẫn cài đặt trên trình duyệt.**

**Vì sao hai đường mà không phải một.** Hai đường phục vụ hai người khác nhau ở hai lúc khác nhau. Người dựng máy portable là người có khả năng chạy dòng lệnh và có thời gian làm việc đó ở nhà; người vận hành ngày thi là giáo viên trong hội trường, trước giờ phát sóng. Bắt người thứ hai làm việc của người thứ nhất là chỗ hỏng thật của `PRD-REQ-085` — chứ không phải chuyện dòng lệnh xấu.

**Vì sao chưa làm trình hướng dẫn.** Nó là đường thứ ba cho cùng một việc, và là đường **nhạy cảm nhất**: một bề mặt web mở, không xác thực, tạo được tài khoản toàn quyền. Muốn làm cho đúng thì phải có mã dùng-một-lần sinh lúc cài, tức lại quay về một thao tác dòng lệnh. Hai đường trên đã phủ hết ca dùng đã biết.

**Ràng buộc kèm theo — tài khoản admin trong gói LUÔN theo phương án (a).** `QĐ-086` vế 4 cho người xuất chọn giữ mật khẩu cũ; **ngoại lệ: tài khoản admin không được chọn (b)**. Nó luôn được cấp mật khẩu mới lúc nhập, in ra phiếu. Lý do: tài khoản admin là **toàn quyền trên trận**, nên cái giá của việc dấu vết mật khẩu của nó rời hệ thống khác hẳn cái giá với một ghế thí sinh. Đây là vế `[SUY RA]`, không phải phát biểu trực tiếp của chủ dự án.

**Hệ quả.**

- **Bản cài trống + gói không kèm danh sách người tham gia ⇒ vẫn phải dùng dòng lệnh.** Tài liệu vận hành phải nói thẳng ca này, vì nó là ca duy nhất còn lại có thể xảy ra ở hội trường.
- **`EPIC-012` hết mục *out of scope*** về trải nghiệm cài đặt lần đầu.
- **Phép kiểm nghiệm thu của `PRD-REQ-085` mạnh lên**: chạy trọn một trận trên bản portable **đã ngắt Internet**, bắt đầu từ thao tác nhập gói — không có bước dòng lệnh nào ở giữa.

*Nguồn*: `[CHỦ DỰ ÁN]` · vế ràng buộc tài khoản admin: `[SUY RA]` từ `QĐ-078` · *Thay cho*: câu hỏi mở **trải nghiệm cài đặt lần đầu** *(nay đã đóng)*

### QĐ-091 — Hạn lưu trữ: admin đặt được; mặc định 12 tháng chính thức, 3 tháng luyện tập

**Quyết định.** Hạn lưu trữ là **giá trị cấu hình do admin đặt**, riêng cho từng mục đích trận. Giá trị **mặc định**:

| Mục đích trận | Mặc định |
|---|---|
| `official` | **12 tháng** |
| `practice` | **3 tháng** |

Hai con số là **mặc định**, không phải luật: không rule, không transition nào đọc chúng.

**Vì sao đặt mặc định thay vì để trống.** `QĐ-040` đã chốt phần chuẩn tắc — hạn **khác nhau theo mục đích trận**. Cái còn thiếu chỉ là con số, và con số không có thì job dọn không dựng được, còn `PRD-REQ-080` không có gì để kiểm. Khác với quy mô viewer (`QĐ-067`), ở đây **không cần đo gì cả** để chọn: đây là lựa chọn về quyền riêng tư, không phải về phần cứng.

**Vì sao 3 tháng cho luyện tập.** Trận luyện tập không có giá trị phân xử — không ai khiếu nại một buổi tập. Giữ lâu chỉ tích thêm dữ liệu cá nhân của học sinh mà không đổi được gì.

**Rủi ro của việc để admin đặt.** Admin đặt được nghĩa là admin đặt được **rất dài**. Đó là chấp nhận có chủ đích: đơn vị tự host là bên chịu trách nhiệm pháp lý, nên bên đó phải là bên quyết. Hệ thống làm đúng một việc — **mặc định về phía giữ ít hơn**.

**Hệ quả.**

- **Hai con số nay thuộc `docs/`.** `CLAUDE.md` bỏ chúng khỏi §Lộ trình version; chỗ đó chỉ còn nói *"retention riêng theo mục đích trận"*.
- **`game-state-machine.md` `T-022`** ghi *"retention 3 tháng"* — đọc là **mặc định**, không phải hằng số.
- **Job dọn thuộc v1.5** (`roadmap-post-v1.md` §8.1), nhưng **ràng buộc đặt lên nó** (`PRD-REQ-080`: cảnh báo trước, không đụng biên bản đã xuất) thuộc v1 và không đổi.
- **Gói đã xuất nằm ngoài tầm với của hạn lưu trữ** — gồm cả gói mang danh sách người tham gia (`QĐ-086`). Đây là lý do tài liệu vận hành phải nhắc xoá gói sau ngày thi.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: câu hỏi mở **giá trị hạn lưu trữ** *(nay đã đóng)*

---

# M. Bảng tra mã CŨ → MỚI

> Dùng khi đọc tài liệu chưa dọn hoặc `reviews/`. Mã cũ **không còn xuất hiện** trong đặc tả.

> Bảng này liệt kê các họ mã chính. Nguồn chuẩn là dòng ***Thay cho*** của từng mục — có mã cũ nào không thấy ở đây thì tìm trong đó.

### Mã cấp sản phẩm đã ngừng dùng

| Mã cũ | Mã mới |
|---|---|
| `A-12` | `QĐ-065` |
| `Q-A1` | `QĐ-051` |
| `Q-A3` | `QĐ-074` |
| `Q-A5` | `QĐ-074` |
| `Q-A6` | `QĐ-074` |
| `Q-A7` | `QĐ-074` |
| `Q-B2` | `QĐ-073` |
| `Q-B4` | `QĐ-073` |
| `Q-B5` | `QĐ-073` |
| `Q-C2` | `QĐ-075` |
| `Q-C3` | `QĐ-075` |
| `Q-C4` | `QĐ-075` |
| `Q-C1b` | `QĐ-075` |
| `Q-C1c` | `QĐ-075` |
| `S-1` | `QĐ-078` |
| `S-3` | `QĐ-051` |
| `S-4` | `QĐ-076` |
| `S-5` | `QĐ-079` |
| `S-6` | `QĐ-077` |
| `S-7` | `QĐ-051` |
| `S-8` | `QĐ-073` |
| `S-9` | `QĐ-073` |
| `S-10` | `QĐ-077` |
| `S-11` | `QĐ-076` |
| `S-16` | `QĐ-008` |
| `S-17` | `QĐ-070` |
| `S-18` | `QĐ-051` |
| `S-21` | `QĐ-071` |
| `S-22` | `QĐ-076` |
| `S-24` | `QĐ-049` |
| `S-25` | `QĐ-049` |

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
| `C-1`, `K-4` | `QĐ-062` | `U-7`, `U-34`, `U-36` | `QĐ-066` |
| `C-3` | `QĐ-064` | `C-2`, `C-5` | `QĐ-067` |
| `C-10`, `K-10` | `QĐ-063` | `D16` *(vế "cột set tay")* | `QĐ-063` *(bị thay thế)* |

---

# N. Đã hoãn có chủ đích

**Không còn mục treo nào.** Hai mục dưới đây **không phải câu hỏi chưa trả lời** — chúng là câu hỏi chủ dự án đã quyết là **chưa cần trả lời lúc này**.

### QĐ-067 — Quy mô viewer và ngưỡng độ trễ: HOÃN, không phải treo

**Quyết định.** Không chốt con số viewer đồng thời và ngưỡng độ trễ ở giai đoạn này. Mọi chỗ cần một con số dùng **mặc định cấu hình được**; không rule, không transition, không đặc tả nào được viết dựa trên một con số cụ thể.

**Vì sao.** Đây là **mục tiêu phi chức năng**, phụ thuộc sức chứa hội trường và phần cứng máy chủ thật. Không luật chơi nào và không quyết định nào trong sổ này hàm ý được một con số — mọi cách *"suy ra"* ở đây đều là bịa. Bốn tài liệu cũ đưa **bốn con số khác nhau**, và sự khác nhau đó không phản ánh tranh luận nào, chỉ phản ánh việc chưa ai đo.

Chốt bừa một con số **đắt hơn** là để trống: nó biến một giá trị cấu hình thành một cam kết, và cam kết sai thì phải viết lại cả mục tiêu kiểm thử tải.

**Hệ quả.** Con số này chỉ đi vào **hai** chỗ, cả hai đều là **cấu hình**: ngưỡng **rate-limit** của cổng viewer, và **mục tiêu load-test**. Cả hai để mặc định, sửa bằng biến môi trường.

Khi nào cần trả lời, chỉ phải chốt hai điều: **(a)** số viewer đồng thời tối đa ở hồ sơ **portable LAN** — suy từ hội trường lớn nhất dự kiến; **(b)** hồ sơ **compose** có cần con số cao hơn không, và cao bao nhiêu.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: `C-2`, `C-5`

**Bổ sung sau `QĐ-088`.** Việc hai kênh public chuyển sang đẩy một chiều **không** trả lời câu hỏi này, nhưng nó hạ **hạng** của câu hỏi: con số viewer nay chắc chắn không chạm tới công bằng trận, chỉ còn là giá trị định cỡ. Hai điều cần chốt ở đoạn cuối vẫn nguyên như trên.

### QĐ-092 — Bộ chỉ số: chốt BẢY, hoãn BỐN cho tới khi có người dùng đầu tiên

**Quyết định.** Nhóm chỉ số đề xuất được tách làm hai, và chỉ nhóm đầu là **tiêu chí nghiệm thu v1**:

| | Chỉ số | Vì sao chốt được ngay |
|---|---|---|
| **Chốt** | thời gian dựng một contest khi kho đề đã có · số lần **ép qua cảnh báo** trong một trận · số lần trận gián đoạn vì lỗi hệ thống · tỉ lệ thí sinh nối lại trong ngưỡng chờ · sự kiện bị khiếu nại có đủ dấu vết · số đáp án rò ra kênh không có quyền · thời điểm sớm nhất thí sinh thấy được nội dung đề | Đo được bằng **bộ kiểm tự động và một trận thử**, không cần ai ngoài đội |
| **Hoãn** | tỉ lệ admin dựng được contest lần đầu không cần hỏi ai · tỉ lệ tái sử dụng câu hỏi sau 5 trận · số trận thật trong ba tháng đầu · nhân sự tối thiểu vận hành một trận | Cần **người ngoài đội** hoặc **thời gian sau khi ra mắt** |

Bốn chỉ số hoãn **chỉ được đặt ngưỡng sau khi có người dùng đầu tiên đã cam kết** (`ASSUMPTION-001`).

**Vì sao hoãn thay vì bỏ.** Chúng là bốn chỉ số **duy nhất** đo được thứ mà `product-discovery.md` §9 tự nhận là đang thiếu: *"không tiêu chí nào đo giá trị với người dùng."* Bỏ chúng là chấp nhận vĩnh viễn cái thiếu đó. Nhưng đặt ngưỡng bây giờ là đặt cho **một dân số chưa tồn tại** — `≥ 4/5 người` khi chưa biết năm người đó là ai thì không phải một mục tiêu, nó là một con số trang trí.

**Hai chỉ số chốt nằm TRONG bộ kiểm đã có, không phải bộ kiểm mới.** *Số đáp án rò* và *thời điểm sớm nhất thí sinh thấy đề* là **hai phép đo cụ thể** của chính bộ kiểm *"không rò đáp án"* đã chốt từ đầu. Ghi riêng để chúng có ngưỡng rõ, không phải để dựng thêm bộ kiểm thứ hai.

**Hệ quả.** `PRD.md` §19.2 tách làm hai bảng thay vì một bảng mang `NEEDS CLARIFICATION`; bốn chỉ số hoãn chuyển sang `roadmap-post-v1.md`, cùng chỗ với các hạng mục chưa gắn mốc.

*Nguồn*: `[CHỦ DỰ ÁN]` · *Thay cho*: câu hỏi mở **bộ chỉ số thành công chưa chốt** *(nay đã đóng)*
