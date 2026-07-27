# Luật chơi

> **Phạm vi**: 37 rule `GR-001` → `GR-037`, phủ 5 vòng thi và luật xuyên vòng.
>
> **Loại tài liệu**: đặc tả **nghiệp vụ**. Không chứa code, pseudocode, tên framework, tên bảng dữ liệu hay tên lớp.
>
> **Tài liệu này nói CÁI ĐANG LÀ.** Lý do đằng sau mỗi lựa chọn nằm ở `decisions.md`, tra theo mã `QĐ-*`.

## Quan hệ với tài liệu khác

| File | Vai trò |
|---|---|
| `source/fandom-olympia-26-luat-choi.md` | **Luật gốc O26 nguyên văn** — source of truth duy nhất |
| `decisions.md` | **Vì sao** — 66 quyết định `QĐ-001` → `QĐ-066`, kèm bảng tra mã cũ |
| `glossary.md` | Thuật ngữ chuẩn `TERM-*`; file này dùng đúng tên ở đó |
| `game-state-machine.md` | Máy trạng thái: `STATE-*` · `EVENT-*` · `T-*` · `INV-*`. Mọi rule ở đây phải khớp với một hoặc nhiều transition ở đó |
| `traceability.md` | Ma trận truy nguyên requirement ↔ luật gốc ↔ `QĐ-*` |
| `reviews/` | **Kho lưu** — biên bản thảo luận và đề xuất `GRR-*`. **Không phải nguồn**, không trích vào đây |

## Cách đọc một rule

Mỗi rule dùng chung một khuôn. Không phải trường nào cũng có mặt — trường nào không có nội dung thì **không xuất hiện**, thay vì để dấu gạch ngang.

| Trường | Nội dung |
|---|---|
| **Mục đích** | Rule này quyết định điều gì |
| **Kích hoạt** | Sự kiện nào làm rule chạy |
| **Điều kiện** | Tiền đề phải đúng, và các ràng buộc chi phối |
| **Bảng quyết định** | Ca → kết quả mong đợi → thay đổi trạng thái. Đây là phần **chuẩn tắc**; các trường khác giải thích cho nó |
| **Không đổi gì** | Những thứ rule này **bảo đảm không đụng tới** — dùng để viết acceptance criteria phủ định |
| **Thứ tự đánh giá** | Khi nhiều điều kiện cùng áp, thứ tự nào thắng |
| **Biên** | Giá trị ở mép: nhỏ nhất, lớn nhất, đúng bằng mốc |
| **Bấm trùng** | Điều gì xảy ra khi cùng một thao tác đến hai lần |
| **Đồng thời** | Điều gì xảy ra khi hai thao tác đến cùng lúc |
| **Ví dụ** | Ca cụ thể, có số |
| **Nguồn** | Luật gốc và `QĐ-*` |

**Đánh dấu suy luận**: `[SUY RA]` = hệ quả bắt buộc của một quyết định, không phải phát biểu trực tiếp của nguồn.

**Tài liệu này chỉ mô tả những gì hệ thống CÓ.** Nhánh không tồn tại thì không có mục cho nó. Riêng dòng **Không đổi gì** là ngoại lệ có chủ ý — nó là **yêu cầu phủ định**, chặn một hiện thực sai dễ xảy ra.

---

# Nguyên tắc nền

> Hai mươi ba mệnh đề chi phối **mọi** rule. Chúng không lặp lại ở từng rule; rule nào phụ thuộc thì trích mã `QĐ-*` tương ứng. Đầy đủ bối cảnh và phương án bị loại: `decisions.md`.

| # | Nguyên tắc | Mã |
|---|---|---|
| 1 | **Máy độc quyền SỰ KIỆN, người độc quyền PHÁN QUYẾT.** Mọi outcome *"đúng/sai"* trong tài liệu này là kết quả **SAU KHI admin bấm** | `QĐ-001` · `QĐ-010` |
| 2 | **Admin là cảm biến.** Mọi mốc mà luật gốc mô tả bằng hành vi của MC đều thành một cú bấm của admin, và mốc đó **tuyệt đối** — không ân hạn | `QĐ-006` · `QĐ-027` |
| 3 | **Không có cơ chế drop tín hiệu.** Mọi tín hiệu vào hàng đợi theo server timestamp; lịch sử không bao giờ xoá | `QĐ-020` |
| 4 | **Hàng đợi chỉ CHẶN ở VCNV.** Các vòng khác: có chuông là tính ngay | `QĐ-021` |
| 5 | **Server time là source of truth duy nhất**, trên **đồng hồ đơn điệu** | `QĐ-006` · `INV-004` |
| 6 | **Điểm là hàm của event log**; hoàn nguyên bằng cách **thêm event đảo ngược** | `QĐ-011` |
| 7 | **Điểm được phép âm, không có sàn** | `QĐ-012` |
| 8 | **Chỉ CHẶN CỨNG ở ba chỗ**; mọi lệch luật khác chỉ **cảnh báo**, admin ép được | `QĐ-003` |
| 9 | **Thao tác ở INVALID STATE không thực hiện được** — nhánh **không tồn tại**, khác hẳn chặn cứng | `QĐ-004` |
| 10 | **Đồng hồ khoá THÍ SINH, không khoá ADMIN** — hai ngoại lệ ở điểm 22 | `QĐ-030` |
| 11 | **Đồng hồ chạy liên tục, không bao giờ đóng băng**; trận dừng bằng cách admin ngừng thao tác | `QĐ-030` |
| 12 | **Một câu chỉ đi qua ĐÚNG MỘT phán quyết** — chấm xong thì khoá, muốn sửa thì điều chỉnh điểm thủ công | `QĐ-014` |
| 13 | **Phán quyết là điều kiện để chuyển câu** — nút *"Câu kế tiếp"* chỉ hiện sau khi đã chấm | `QĐ-014` · `INV-010` |
| 14 | **Tín hiệu KHÔNG giành quyền không làm gián đoạn đồng hồ.** Cái bị hoãn là **việc HIỂN THỊ**, không phải thời gian | `QĐ-031` |
| 15 | **Hai tín hiệu cùng mốc: hàng đợi tự quyết**, ngẫu nhiên lúc nhận, **tất định khi dựng lại** | `QĐ-025` |
| 16 | **Nút chuông tự khoá ngay khi bấm**, ở frontend, trước khi gửi | `QĐ-023` |
| 17 | ***"Hiển thị câu hỏi"* và *"start timer"* là HAI thao tác**, thứ tự cố định | `QĐ-028` |
| 18 | **Chấm xong là KẾT THÚC CÂU** — xoá hàng đợi đang hoạt động, gỡ khoá chuông | `QĐ-020` |
| 19 | **Luật cho bao nhiêu câu thì đúng bấy nhiêu** — không có câu thứ N+1 | `QĐ-041` |
| 20 | **Kho đề kiểm tại CỬA VÀO TỪNG VÒNG** — thiếu thì không mở được vòng đó | `QĐ-042` |
| 21 | **LUÔN ghi nhận đáp án CUỐI CÙNG**; nút gửi **không** khoá sau khi gửi | `QĐ-029` · `QĐ-059` |
| 22 | **Phán quyết của admin là quyết định cuối cùng**; ở vòng gõ máy, nút chấm **khoá tới khi hết giờ** | `QĐ-030` |
| 23 | **Chọn hàng ngang có MỘT đường vào cho mỗi mode** — và là chỗ **duy nhất** có dialog phía thí sinh | `QĐ-005` · `QĐ-019` |

## Bốn bảng dùng chung

Bốn bảng dưới đây nhiều rule cùng đọc, nên đặt ở đây thay vì lặp lại.

### Số câu theo luật

| Vòng | Số câu |
|---|---|
| Khởi động — lượt riêng | **6** mỗi thí sinh |
| Khởi động — lượt chung | **12** |
| VCNV | **4** hàng ngang + **1** ô trung tâm |
| Tăng tốc | **4** |
| Về đích | **3** mỗi gói |
| Câu hỏi phụ | **3** |

> Biên *"lớn hơn max"* của mọi vòng **không tồn tại** — không phải *"chặn cứng"*, cũng không phải *"cảnh báo"*. Không có đường vào thì không cần cả hai. **Đừng lẫn với `rowCount` 5-8** ở VCNV: đó là **đổi con số của luật**, khác hẳn việc hệ thống tự đẻ thêm câu ngoài con số đã cấu hình.

### Phán quyết có hai hay ba lựa chọn

**Tiêu chí: chỉ nhị phân khi *Sai* trừ 0 điểm.** Chỗ nào *Sai* kéo theo hình phạt thì phải có lựa chọn thứ ba — nếu không, admin bị ép chọn giữa cho điểm và phạt, trong khi tình huống có thể không đáng cả hai.

| Vòng · pha | *Sai* trừ bao nhiêu | Lựa chọn của admin |
|---|---|---|
| Khởi động lượt riêng · VCNV hàng ngang · Tăng tốc · Câu hỏi phụ | **0** | Đúng / Sai |
| Về đích — người thi chính | **0** (chỉ mở cửa sổ cướp) | Đúng / Sai |
| **Khởi động — lượt chung** | **−5** | Đúng / Sai / **Huỷ kết quả** |
| **Về đích — người cướp quyền** | **−½ giá trị câu** | Đúng / Sai / **Huỷ kết quả** |
| **Về đích — câu có Ngôi sao hy vọng** | **−giá trị câu** | Đúng / Sai / **Huỷ kết quả** |

> ***Huỷ kết quả* luôn có mặt** khi câu **chỉ có bản gửi quá hạn**, kể cả ở vòng mà *Sai* trừ 0. Dù hai hay ba lựa chọn, vẫn là **một** phán quyết cho một câu.

### Bản gửi quá hạn

| Tình huống | Hệ thống làm gì | Admin chọn |
|---|---|---|
| Có bản **hợp lệ**, còn gửi thêm bản **quá hạn** | Giữ **cả hai**, bản quá hạn tô **đỏ** | Đúng / Sai |
| **Chỉ có** bản quá hạn | Giữ, tô **đỏ** | Đúng / Sai / **Huỷ kết quả** |

> Bản quá hạn **không tự ghi đè** bản hợp lệ — *"ghi nhận bản cuối"* chỉ áp **trong các bản hợp lệ**. Máy **không tự loại** bản quá hạn; cả việc **hiển thị** lẫn việc **chấm** đều là cú bấm của admin.

### Kênh trả lời quyết định lúc nào admin chấm được

| Kênh | Nút gửi của thí sinh | Nút chấm của admin |
|---|---|---|
| **Nói** — mode sân khấu | **Không tồn tại** | Bấm được **bất cứ lúc nào** |
| **Gõ** — mode nhập liệu, và các vòng **luôn gõ máy** | Sống tới khi hết giờ | **Khoá tới khi hết giờ** |

> **VCNV hàng ngang** và **Tăng tốc** luôn gõ máy **bất kể mode contest**, nên hai vòng này luôn theo nhánh dưới. Đây là **ngoại lệ thứ hai** của nguyên tắc 10; ngoại lệ thứ nhất là nút start timer.

---

# Mục lục

| Nhóm | Rule |
|---|---|
| **Khởi động** | `GR-001` chấm câu lượt riêng · `GR-002` hết thời gian suy nghĩ · `GR-003` giành quyền bằng chuông · `GR-004` chấm câu và hình phạt · `GR-005` cửa sổ chuông rỗng · `GR-006` ghi nhận đáp án |
| **Vượt chướng ngại vật** | `GR-007` lượt chọn hàng ngang · `GR-008` trả lời hàng ngang và mở miếng ghép · `GR-009` bấm chuông giải Chướng ngại vật · `GR-010` trả lời sai Chướng ngại vật · `GR-011` ô trung tâm và gợi ý cuối · `GR-012` toàn bộ thí sinh bị loại |
| **Tăng tốc** | `GR-013` xếp hạng tốc độ · `GR-014` đồng thời gian · `GR-015` ghi nhận bản cuối |
| **Về đích** | `GR-016` thứ tự lượt thi · `GR-017` chọn gói câu · `GR-018` trả lời câu của mình · `GR-019` câu hỏi thực hành · `GR-020` cướp quyền · `GR-021` Ngôi sao hy vọng |
| **Câu hỏi phụ** | `GR-022` điều kiện kích hoạt · `GR-023` thể thức ba câu · `GR-024` bấm chuông trước hiệu lệnh · `GR-025` hết câu chưa phân định |
| **Chấm điểm và mô hình điểm** | `GR-026` phán quyết của admin · `GR-027` chuẩn hoá và highlight · `GR-028` điểm là hàm của event log · `GR-029` điều chỉnh điểm thủ công · `GR-030` bỏ vòng và chạy lại vòng · `GR-031` rút đề và no-repeat |
| **Điều khiển và hạ tầng** | `GR-032` hàng đợi tín hiệu · `GR-033` mốc thời gian do admin bấm · `GR-034` chuông chỉ nhận click chuột · `GR-035` server time · `GR-036` mất kết nối và giữ ghế · `GR-037` phạm vi hiển thị đáp án |

---

## GR-001 — Khởi động lượt riêng: chấm câu

**Mục đích.** Xác định điểm một thí sinh nhận được cho một câu trong **lượt riêng**, sau khi admin phán quyết.

**Kích hoạt.** Admin bấm Đúng hoặc Sai cho câu đang mở của thí sinh đang tới lượt.

**Điều kiện.**

- Trận đang ở Khởi động lượt riêng; lượt đã khoá vào **một** thí sinh trước câu đầu tiên.
- Câu đã hiển thị và admin đã bấm start timer. Thời gian suy nghĩ **3 giây** tính từ mốc đó.
- Ở mode sân khấu (mặc định) thí sinh **đọc** đáp án — hệ thống không nhận nội dung, admin là người nghe và phán quyết.
- Đúng **+10** · Sai **0**, không phạt. Lượt riêng **không có** hình phạt.

**Bảng quyết định**

| Ca | Điều kiện | Kết quả | Thay đổi trạng thái |
|---|---|---|---|
| C1 | Admin bấm **Đúng** | **+10** | Sinh event điểm; câu sang *đã chấm* |
| C2 | Admin bấm **Sai** | **0**, không trừ | Sinh event điểm giá trị 0; câu sang *đã chấm* |
| C3 | Hết 3 giây, admin **chưa** bấm | Chuyển `GR-002` | Câu vẫn chưa chấm cho tới khi admin bấm |
| C4 | Câu thuộc **vòng đã bị bỏ**, admin bấm Đúng | **Toast**, không thực hiện được — không sinh event cho vòng đã bỏ | Không đổi |

**Không đổi gì.** Điểm mọi thí sinh khác · lịch sử event (mọi hiệu chỉnh là event **thêm vào**) · câu đã dùng **không** trả lại kho.

**Biên.** Câu **1** → **6** áp dụng bình thường. **Câu thứ 7 không tồn tại** — sau khi chấm câu 6, nút *"Câu kế tiếp"* đã chuyển thành *"Kết thúc lượt"*. Mốc **3.000 giây**: hệ thống **không phân xử** — đồng hồ không khoá nút chấm, trong hạn hay quá hạn là đánh giá của MC và admin.

**Ví dụ.**

- *Hợp lệ*: câu thứ 3 của thí sinh vị trí 2; admin bấm start timer, thí sinh đọc đáp án, MC xác nhận đúng, admin bấm Đúng ⇒ **+10**.
- *Không hợp lệ*: hệ thống tự cộng 10 vì đáp án khớp mà admin chưa bấm ⇒ trái `QĐ-010`.
- *Biên*: thí sinh đọc đáp án đúng khoảnh khắc đồng hồ chỉ 3.000 giây ⇒ admin vẫn bấm Đúng được.

**Nguồn**: luật gốc §Khởi động đoạn 2 · `QĐ-010`, `QĐ-014`, `QĐ-027`, `QĐ-041`

---

## GR-002 — Khởi động lượt riêng: hết thời gian suy nghĩ

**Mục đích.** Xác định kết quả khi thí sinh trong lượt riêng **không đưa ra đáp án** trong thời gian suy nghĩ.

**Kích hoạt.** **Admin bấm Sai.** Đồng hồ hết giờ **không** tự sinh kết quả.

**Điều kiện.**

- ***"Không trả lời"* và *"trả lời sai"* là CÙNG một thao tác** — admin bấm Sai. Hệ thống không phân biệt hai tình huống.
- Kết quả: **0 điểm, không trừ**.

**Bảng quyết định**

| Ca | Điều kiện | Kết quả | Thay đổi trạng thái |
|---|---|---|---|
| C1 | Thí sinh im lặng; admin bấm Sai | **0**, không trừ | Sinh event điểm; câu chờ admin bấm *"Câu kế tiếp"* |
| C2 | Thí sinh đọc đáp án trong hạn | Chuyển `GR-001` | — |
| C3 | Đáp án đến sát hoặc quá mốc 3 giây | **MC và admin đánh giá** — nút chấm không bị đồng hồ khoá | Theo phán quyết của admin |
| C4 | Trả lời đúng ở giây thứ 1, admin bấm Đúng | Điểm chốt **ngay**; đồng hồ còn lại mất ý nghĩa | Câu **không tự chuyển** — sang câu tiếp là thao tác riêng của admin |

**Không đổi gì.** Điểm của thí sinh và mọi người khác · **không sinh hình phạt nào** — hình phạt **−5** chỉ thuộc lượt chung.

**Biên.** `t = 0` là mốc admin bấm start timer. `t = 3.000` giây: hệ thống **không phân xử**. `t > 3` giây: nút chấm **vẫn bấm được**.

**Ví dụ.**

- *Hợp lệ*: thí sinh trả lời đúng ở giây thứ 1 ⇒ **+10 ngay**; hai giây còn lại không còn ý nghĩa, câu vẫn đứng đó tới khi admin chuyển.
- *Không hợp lệ*: hệ thống tự trừ 5 vì không trả lời ⇒ sai luật **hai lần** — máy không tự chấm, và **−5** không thuộc lượt riêng.
- *Biên*: thí sinh bắt đầu nói ở giây 2.9 nhưng MC xác nhận sau giây 3 ⇒ admin vẫn bấm Đúng được.

**Nguồn**: luật gốc §Khởi động đoạn 2 · `QĐ-010`, `QĐ-030`, `QĐ-056`

---

## GR-003 — Khởi động lượt chung: giành quyền bằng chuông

**Mục đích.** Xác định thí sinh nào **giành được quyền trả lời** một câu trong lượt chung.

**Kích hoạt.** Một thí sinh bấm chuông.

**Điều kiện.**

- **Cửa sổ chuông là MỘT khoảng liên tục**: mở từ **mốc admin bấm hiển thị câu hỏi**, kéo qua thời gian MC đọc, và thêm **3 giây** sau mốc start timer.
- Luật gốc **cho phép** bấm chuông **trong khi MC đang đọc** — đây là lý do cửa sổ mở sớm như vậy.
- Hàng đợi ở vòng này **KHÔNG chặn**: có chuông là tính ngay theo server timestamp; hàng đợi vẫn ghi thứ tự để admin can thiệp khi có sự cố.
- Thời gian suy nghĩ **3 giây** tính **từ thời điểm giành được quyền**, không phải từ mốc admin bấm.

**Bảng quyết định**

| Ca | Điều kiện | Kết quả | Thay đổi trạng thái |
|---|---|---|---|
| C1 | Cửa sổ mở, một thí sinh bấm đầu tiên | Giành quyền; bắt đầu đếm **3 giây** | Ghi tín hiệu; đánh dấu người giành quyền |
| C2 | Bấm khi MC đang đọc, trước mốc start timer | **Hợp lệ** — luật cho phép | Như C1 |
| C3 | Đã có người giành quyền, người khác bấm | Tín hiệu **TRƠ** — ghi làm căn cứ cho admin, **không** đổi người giữ quyền, **không** mở lại chuông | Ghi tín hiệu kèm timestamp |
| C4 | Hai ghế **cùng** server timestamp | **Hàng đợi tự quyết**, ngẫu nhiên — không ưu tiên theo ghế hay vị trí | Cả hai tín hiệu được ghi kèm thứ tự đã chọn |
| C5 | Ghế đã bấm và bị chấm Sai ở câu này, bấm tiếp | Nút **không hiển thị**, bấm **không phản hồi** ⇒ **không tín hiệu nào được tạo** | Không đổi |
| C6 | Bấm sau khi cửa sổ đã đóng | Như C5 | Không đổi |

**Không đổi gì.** Điểm — giành quyền **chưa phải** phán quyết · **lịch sử tín hiệu không bao giờ xoá** · cửa sổ chuông **không** được kéo dài bởi thao tác của thí sinh.

**Biên.** *"Bấm trước khi câu được đưa ra"* **không tồn tại** — trước mốc hiển thị, màn thí sinh chưa có gì và chuông chưa sống. Mốc **3.000 giây** sau start timer: biên **ĐÓNG**, tín hiệu đúng mốc **vẫn giành quyền**. Sau đó: câu bị bỏ qua theo `GR-005`.

**Đồng thời.** Hai ghế cùng mốc ⇒ hàng đợi tự chọn, **ngẫu nhiên lúc nhận** nhưng **tất định khi dựng lại** (thứ tự đã chọn được ghi và không xoá). Quy tắc *"cùng thời gian thì cùng mức điểm"* của Tăng tốc **không dùng lại được ở đây**: điểm chia được, quyền trả lời thì không.

**Ví dụ.**

- *Hợp lệ*: MC mới đọc nửa câu, thí sinh vị trí 3 bấm ⇒ giành quyền; đồng hồ 3 giây bắt đầu từ thời điểm bấm.
- *Không hợp lệ*: dùng phím tắt để bấm chuông ⇒ không có đường phát tín hiệu.
- *Biên*: hai thí sinh bấm, server ghi cùng một mốc mili-giây ⇒ hàng đợi tự chọn; thứ tự đã chọn được ghi lại để phân xử về sau.

**Nguồn**: luật gốc §Khởi động đoạn 3-5 · `QĐ-020`, `QĐ-021`, `QĐ-023`, `QĐ-024`, `QĐ-025`, `QĐ-028`

---

## GR-004 — Khởi động lượt chung: chấm câu và hình phạt

**Mục đích.** Xác định điểm cộng và điểm trừ cho thí sinh **đã giành quyền** trong lượt chung.

**Kích hoạt.** Admin bấm Đúng, Sai, hoặc Huỷ kết quả cho người đã giành quyền.

**Điều kiện.**

- Đúng **+10** · Sai **−5**.
- **Bấm chuông rồi im lặng hết 3 giây** cũng **−5** — nhánh phạt riêng, khác hẳn *"không ai bấm"* (`GR-005`).
- Đây là vòng có **ba** lựa chọn phán quyết, vì *Sai* kéo theo hình phạt.

**Bảng quyết định**

| Ca | Điều kiện | Kết quả | Thay đổi trạng thái |
|---|---|---|---|
| C1 | Admin bấm **Đúng** | **+10** | Sinh event điểm |
| C2 | Admin bấm **Sai** | **−5** | Sinh event điểm âm |
| C3 | Admin bấm **Huỷ kết quả** | **0** — câu không sinh điểm cho ai; **không** áp **−5** | Sinh event giá trị 0; câu *đã chấm* |
| C4 | Giành quyền rồi không trả lời; admin bấm Sai | **−5** | Sinh event điểm âm |
| C5 | Thí sinh đang có **0** điểm và bị **−5** | Điểm thành **−5** — **không có sàn** | Sinh event điểm âm |
| C6 | Câu đã chấm Sai | **KHÔNG mở lại chuông** — câu kết thúc | **Xoá hàng đợi đang hoạt động**; **gỡ khoá chuông** mọi ghế cho câu mới |

**Không đổi gì.** Điểm của các thí sinh **khác** — lượt chung **không có** cơ chế chuyển điểm giữa người với người · **xoá hàng đợi chỉ đụng hàng đợi ĐANG HOẠT ĐỘNG**; lịch sử tín hiệu, kể cả tín hiệu trơ, **không bao giờ xoá**.

**Biên.** Điểm **không có sàn**. Câu **12** là câu cuối; **câu thứ 13 không tồn tại**. Mốc **3.000 giây** kể từ khi giành quyền: hệ thống **không phân xử**.

**Ví dụ.**

- *Hợp lệ*: giành quyền, trả lời sai ⇒ **−5**; đang có 30 còn **25**.
- *Không hợp lệ*: hệ thống tự trừ **−5** dựa trên so khớp văn bản mà admin chưa bấm.
- *Biên*: đang có **0** điểm, bấm chuông rồi im lặng ⇒ **−5**; điểm âm hợp lệ.

**Nguồn**: luật gốc §Khởi động đoạn 3 · `QĐ-010`, `QĐ-012`, `QĐ-020`, `QĐ-061`

---

## GR-005 — Khởi động lượt chung: cửa sổ chuông rỗng

**Mục đích.** Xác định điều gì xảy ra với một câu lượt chung mà **không ai** giành quyền.

**Kích hoạt.** Hết **3 giây** kể từ mốc start timer mà không có tín hiệu chuông nào.

**Điều kiện.** Hết cửa sổ mà không ai bấm ⇒ **câu bị bỏ qua**. Admin có thao tác **chuyển câu thủ công** để đóng sớm.

**Bảng quyết định**

| Ca | Điều kiện | Kết quả | Thay đổi trạng thái |
|---|---|---|---|
| C1 | Hết 3 giây, **0** tín hiệu | Câu **bị bỏ qua** | Câu khép lại, đánh dấu **đã dùng** |
| C2 | ≥1 tín hiệu trong cửa sổ | Chuyển `GR-003` | — |
| C3 | Tín hiệu đến **đúng mốc** hết cửa sổ | **Hợp lệ** — biên **đóng** ⇒ chuyển `GR-003`, câu **không** bị bỏ qua | Đánh dấu người giành quyền |
| C4 | Admin chuyển câu thủ công trước khi hết 3 giây | Câu bị bỏ qua ngay | Câu khép lại, **đã dùng** |

**Không đổi gì.** Điểm của **mọi** thí sinh · câu bị bỏ qua **không** trả lại kho đề.

**Biên.** Tín hiệu đúng mốc **3.000 giây** vẫn hợp lệ (biên đóng) ⇒ câu không bị bỏ qua.

> ***"Kho đề cạn giữa vòng" không tồn tại***: pre-flight chạy tại **cửa vào từng vòng**; qua được cửa thì đủ câu cho trọn vòng, và câu bị bỏ qua vẫn nằm trong con số cố định đó.

**Ví dụ.**

- *Hợp lệ*: hết 3 giây không ai bấm ⇒ câu bỏ qua, không ai đổi điểm.
- *Không hợp lệ*: câu bị bỏ qua được đưa lại vào kho để hỏi ở trận sau trong cùng contest.
- *Biên*: tín hiệu duy nhất đến đúng mốc 3.000 giây ⇒ **vẫn hợp lệ**, thí sinh đó giành quyền.

**Nguồn**: luật gốc §Khởi động đoạn 5 · `QĐ-029`, `QĐ-041`, `QĐ-042`, `QĐ-044`

---

## GR-006 — Khởi động: ghi nhận đáp án

**Mục đích.** Xác định **bản đáp án nào được ghi nhận** khi thí sinh gửi nhiều lần. Chỉ áp ở **mode nhập liệu** — ở mode sân khấu không có bản gửi nào.

**Kích hoạt.** Thí sinh gửi hoặc gửi lại đáp án trước **mốc cắt**.

**Điều kiện.**

- **Mốc cắt = admin bấm *"công bố đáp án"***, ánh xạ của mốc *"MC công bố đáp án"*.
- Nút gửi **KHÔNG khoá sau khi gửi**; thí sinh sửa và gửi lại bao nhiêu lần cũng được.
- **Luôn ghi nhận bản CUỐI CÙNG.** Phát biểu *"nếu không thay đổi thì ghi nhận đáp án đầu tiên"* của luật gốc **không phải ngoại lệ** — gửi một lần thì bản đầu **chính là** bản cuối.
- **Bản rỗng không phải một đáp án**: bỏ qua, giữ bản hợp lệ trước đó. Mọi đáp án được **trim** hai đầu.

**Bảng quyết định**

| Ca | Điều kiện | Kết quả | Thay đổi trạng thái |
|---|---|---|---|
| C1 | Gửi đúng một bản trong hạn | Ghi nhận bản đó — vừa là bản đầu vừa là bản cuối | Lưu bản được ghi nhận |
| C2 | Gửi nhiều bản khác nhau trước mốc cắt | Ghi nhận bản **cuối cùng** | Bản trước bị thay trong kết quả; **lịch sử giữ nguyên** |
| C3 | Bản cuối đến **đúng mốc cắt** | **Được ghi nhận** — biên **đóng** | Cập nhật bản được ghi nhận |
| C4 | Có bản hợp lệ, bản sau **quá hạn** | **GIỮ CẢ HAI**; bản quá hạn tô **đỏ**. Admin quyết Đúng / Sai | Bản quá hạn **không tự thay** bản hợp lệ |
| C5 | **Chỉ có** bản quá hạn | Tô **đỏ**; admin quyết Đúng / Sai / **Huỷ kết quả** | Kết quả câu theo phán quyết |
| C6 | Gửi lại nội dung **y hệt** | Vẫn là bản cuối; kết quả chấm không đổi | Cập nhật bản được ghi nhận |
| C7 | Bản gửi **rỗng** sau khi trim | **Bỏ qua** — không ghi đè bản đã có | Bản được ghi nhận không đổi |

**Không đổi gì.** **Lịch sử các bản đã gửi không bị xoá** — chỉ *bản được ghi nhận* thay đổi · điểm — ghi nhận đáp án **không phải** phán quyết.

**Thứ tự đánh giá.** Hai bước, cố định: (a) bản đến **trong hạn** không → (b) nếu có và **khác rỗng**, nó thành bản được ghi nhận. Không cần so nội dung với bản trước.

**Đồng thời.** Không có tranh chấp giữa *gửi* và *chấm*: ở mode gõ, nút chấm **khoá tới khi hết giờ**; và **chấm xong thì nút gửi khoá lại** — không còn bản nào tới sau để lật kết quả.

> Quy tắc *"nội dung y hệt thì không cập nhật mốc thời gian"* chỉ có nghĩa ở **vòng xếp hạng theo tốc độ**. Khởi động không xếp theo thời gian nên **không áp**.

**Ví dụ.**

- *Hợp lệ*: gửi `"Hà Nội"`, sửa thành `"Huế"` trước khi công bố ⇒ ghi nhận **`"Huế"`**.
- *Hợp lệ*: gửi `"Hà Nội"` một lần rồi không đụng nữa ⇒ ghi nhận **`"Hà Nội"`**.
- *Không hợp lệ*: bản gửi sau mốc cắt **tự động** thay bản hợp lệ.
- *Biên*: bản sửa cuối đến đúng khoảnh khắc admin bấm công bố ⇒ **vẫn được ghi nhận**.

**Nguồn**: luật gốc §Khởi động đoạn cuối · `QĐ-027`, `QĐ-029`, `QĐ-030`, `QĐ-059`

---

## GR-007 — VCNV: lượt chọn hàng ngang

**Mục đích.** Xác định thí sinh nào được chọn hàng ngang tiếp theo và thứ tự các lượt chọn.

**Kích hoạt.** Một tín hiệu chọn hàng ngang được phát. **Chủ thể phát tín hiệu phụ thuộc mode**: sân khấu ⇒ chỉ **admin** click; nhập liệu ⇒ chỉ **thí sinh** click, admin **không** chọn thay.

**Điều kiện.**

- **Mỗi thí sinh có tối đa 1 lượt lựa chọn**, bắt đầu từ **vị trí số 1**.
- Thí sinh bị loại **trước khi** dùng lượt ⇒ lượt **dồn sang vị trí tiếp theo**.
- Trong trường hợp đó, nếu vị trí cuối đã chọn xong mà **vẫn còn hàng ngang chưa chọn** ⇒ lượt **quay lại vị trí số 1**. Đây là **ngoại lệ tường minh** của *"tối đa 1 lượt"*, chỉ áp khi đã có người bị loại — không mâu thuẫn.
- Hàng đợi ở VCNV **CHẶN**: tín hiệu chỉ có hiệu lực khi admin xác nhận. **Từ chối không làm thí sinh mất lượt.**
- Ở mode **nhập liệu**, thao tác chọn đi qua **dialog xác nhận trên máy thí sinh**; xác nhận xong thì **khoá nút chọn**, và khoá **mở lại** nếu admin bấm No.

**Bảng quyết định**

| Ca | Điều kiện | Kết quả | Thay đổi trạng thái |
|---|---|---|---|
| C1 | Đúng lượt, hàng ngang chưa mở; admin bấm **Yes** | Hàng ngang đó được đưa ra | Đánh dấu **lượt chọn đã dùng**; ô chuyển sang *đã hỏi* |
| C2 | Admin bấm **No** | Tín hiệu kế tiếp lên; **thí sinh không mất lượt**; ở mode nhập liệu nút chọn **mở lại** | **Không tác dụng phụ nào**: ô chưa đánh dấu · câu **trả lại kho** (chưa hiển thị) · đồng hồ chưa chạy |
| C3 | Người đang tới lượt **đã bị loại** | Lượt dồn sang **vị trí tiếp theo** | Cập nhật người tới lượt |
| C4 | Vị trí cuối đã chọn xong, còn hàng ngang chưa chọn | Lượt **quay lại vị trí số 1** | Cập nhật người tới lượt |
| C5 | Trỏ vào hàng ngang **đã mở** | **Không tồn tại** — nó không còn là mục tiêu chọn được; nút không render, bấm không phản hồi | Không đổi |
| C6 | Thí sinh click nhiều lần *(mode sân khấu)* | **Không tồn tại** — máy thí sinh **không có nút chọn** | Không đổi |
| C7 | Thí sinh click nhiều lần *(mode nhập liệu)* | Click đầu mở **dialog**; xác nhận xong **khoá nút** ⇒ **đúng một** tín hiệu rời máy | Nút chọn của ghế chuyển sang khoá |
| C8 | Tín hiệu từ thí sinh **sai lượt** | Vào hàng đợi; hệ thống **cảnh báo**, admin quyết — không chặn cứng | Ghi tín hiệu |
| C9 | Một tín hiệu chọn và một tín hiệu *"Mở chướng ngại vật"* cùng chờ | **Thuần theo thứ tự tới, KHÔNG ưu tiên theo loại**. Cùng mốc ⇒ hàng đợi tự quyết | Hàng đợi giữ đúng thứ tự tiếp nhận |

**Không đổi gì.** Lượt của thí sinh bị từ chối · **lịch sử tín hiệu không bao giờ xoá** · điểm — chọn hàng ngang chưa phải phán quyết.

**Thứ tự đánh giá.** **Không còn ý nghĩa.** Bốn phép kiểm — đúng lượt · đã bị loại · hàng ngang còn chưa mở · đã dùng lượt — đều là **điều kiện render**, đánh giá **một lần tại thời điểm dựng màn thí sinh**. Trạng thái không hợp lệ thì nút **không render** ⇒ **không tín hiệu nào được sinh ra** ⇒ không có gì để sắp thứ tự. Việc admin **ép lượt trái luật** diễn ra ở **khâu gán lượt**, tức **trước** khi cửa sổ chọn mở, nên nó đổi giá trị của phép kiểm chứ không tạo ra tín hiệu cần phân xử.

**Biên.** Còn **4** hàng chưa chọn: lượt bắt đầu từ vị trí 1. Còn **0**: chuyển `GR-011`. Số hàng ngang khác 4 nằm ngoài luật O26 — xem `GR-009` C11.

**Đồng thời.** Hàng đợi chặn xử lý **thuần theo server timestamp**, không ưu tiên theo loại tín hiệu. Hai tín hiệu cùng mốc ⇒ hàng đợi tự quyết, ngẫu nhiên.

> Điều này **cố định băng điểm** ở `GR-009`: băng chốt theo trạng thái tại **mốc admin xác nhận**, mà thứ tự xác nhận đã tất định theo thứ tự hàng đợi.

**Ví dụ.**

- *Hợp lệ*: mode nhập liệu — thí sinh vị trí 1 click hàng ngang 3, dialog hiện, xác nhận ⇒ nút khoá, tín hiệu vào hàng đợi; admin bấm Yes ⇒ hàng ngang 3 được đưa ra.
- *Không hợp lệ*: hệ thống tự mở hàng ngang khi thí sinh xác nhận dialog mà không cần admin duyệt. **Dialog của thí sinh không thay thế phán quyết của admin** — hai lớp khác mục đích.
- *Biên*: vị trí 4 vừa dùng xong lượt, còn 1 hàng chưa chọn vì vị trí 2 đã bị loại ⇒ lượt **quay lại vị trí 1**.
- *Khoá là tạm*: thí sinh xác nhận ⇒ nút khoá; admin bấm **No** ⇒ nút **mở lại**, thí sinh chọn hàng khác, **không mất lượt**.

**Nguồn**: luật gốc §VCNV đoạn 3 · `QĐ-002`, `QĐ-019`, `QĐ-021`, `QĐ-022`, `QĐ-025`

---

## GR-008 — VCNV: trả lời hàng ngang và mở miếng ghép

**Mục đích.** Xác định điểm cho đáp án hàng ngang và điều kiện mở miếng ghép tương ứng.

**Kích hoạt.** Admin **bấm hiển thị đáp án** sau khi hết giờ, rồi phán quyết đáp án của từng thí sinh.

**Điều kiện.**

- Hàng ngang **luôn trả lời bằng máy**, bất kể mode contest. Thời gian suy nghĩ **15 giây**.
- Đúng **+10** cho **mỗi** người được chấm đúng · Sai **0**, không trừ.
- **≥1 người đúng ⇒ miếng ghép mở.** Không ai đúng ⇒ miếng ghép **không** mở.
- **Không thứ gì tự lộ khi hết giờ** — hết giờ chỉ khoá ô nhập. Hai thứ chỉ hiện khi **admin bấm hiển thị**: **đáp án chuẩn** của hàng ngang, và **bài làm của từng thí sinh**.

**Bảng quyết định**

| Ca | Điều kiện | Kết quả | Thay đổi trạng thái |
|---|---|---|---|
| C1 | Một thí sinh được chấm **Đúng** | **+10**; miếng ghép mở | Sinh event điểm; ô sang *mở* |
| C2 | **Nhiều** người cùng đúng | **+10 cho MỖI người**; miếng ghép mở | Sinh event điểm cho từng người |
| C3 | **Không ai** đúng | **0** cho tất cả; miếng ghép **không** mở | Ô ở nguyên *đã hỏi* |
| C4 | Đúng 1 trong 4 người | Ngưỡng mở là **≥1**, và **mọi người đúng đều được 10** — nguồn không đặt ngưỡng nào, cũng không giới hạn số người hưởng điểm | Miếng ghép mở · +10 từng người |
| C5 | Admin bấm mở miếng ghép **hai lần** | **Không tồn tại** — nút một chiều, tự tắt | Server bỏ qua lệnh trùng |
| C6 | Đáp án từ ghế **đã bị loại** | Ô nhập **không hiển thị**, thao tác **không phản hồi** | Không đổi |
| C7 | Bản gửi đến **sau** mốc 15 giây | **Giữ cả bản hợp lệ lẫn bản quá hạn**, bản quá hạn tô **đỏ**. Admin quyết Đúng / Sai; nếu **chỉ có** bản quá hạn thì thêm **Huỷ kết quả** | Hiển thị ra ngoài và chấm đều **do admin bấm** |
| C8 | Đáp án đến **đúng mốc** 15 giây | **Được chấm** — biên **đóng** | Ghi nhận như bản hợp lệ |
| C9 | Có người bấm *"Mở chướng ngại vật"* giữa chừng | **Ghi nhận ngay**; đồng hồ **vẫn chạy**; **đáp án chuẩn** và **bài làm của người khác** đều **chưa hiển thị** | Ghi tín hiệu; đồng hồ không đổi |
| C10 | Chính tả sai lệch nhỏ | Máy **chỉ** highlight ký tự khác; **admin tự đánh giá** | Sinh event theo phán quyết |

**Không đổi gì.** Điểm của người bị chấm sai — hàng ngang **không có** hình phạt · miếng ghép **không** mở khi không ai đúng · trạng thái *đã hỏi* của ô **không** phụ thuộc miếng ghép có mở hay không.

**Thứ tự đánh giá.** Tất định, ba bước: **(a)** chấm **toàn bộ** thí sinh — điều kiện *"≥1 đúng"* chỉ đánh giá được khi đã biết phán quyết của mọi người → **(b)** xác định miếng ghép có mở không, là phép OR trên tập kết quả bước (a) → **(c)** đóng câu.

> Thứ tự này **không nhạy cảm với số người đúng**, đúng vì ngưỡng là *"≥1"* chứ không phải một con số cụ thể.

**Biên.** **0** người đúng: miếng ghép không mở, không ai được điểm. **Tất cả** đúng: mỗi người +10. Mốc **15.000 giây**: submission đúng mốc **được chấm** (biên đóng).

**Đồng thời.** Tín hiệu *"Mở chướng ngại vật"* đến giữa lúc đồng hồ chạy: **ghi nhận ngay**, **đồng hồ vẫn chạy bình thường**, và **không lộ gì thêm** cho tới khi admin bấm hiển thị.

> Vế thứ ba là **điều kiện để hai vế đầu an toàn**. Phải ẩn **cả hai** nguồn dữ kiện: đáp án chuẩn cho người bấm biết luôn hàng ngang là gì, còn bài làm của người khác cho họ suy ra qua phỏng đoán của đối thủ. Ẩn một cái mà lộ cái kia thì cơ chế vẫn hỏng.

**Ví dụ.**

- *Hợp lệ*: hàng ngang 2 được đưa ra, 3 thí sinh gõ đúng ⇒ mỗi người **+10**, miếng ghép số 2 mở.
- *Không hợp lệ*: hệ thống tự cộng 10 vì đáp án khớp chuỗi.
- *Biên*: cả 4 đều sai ⇒ **không ai** được điểm, miếng ghép **không** mở, nhưng hàng ngang 2 **vẫn tính là đã hỏi**.

**Nguồn**: luật gốc §VCNV đoạn 3 và đoạn *"Sau khi trả lời đúng…"* · `QĐ-010`, `QĐ-018`, `QĐ-029`, `QĐ-031`, `QĐ-052`

---

## GR-009 — VCNV: bấm chuông giải Chướng ngại vật

**Mục đích.** Xác định điểm khi giải đúng Chướng ngại vật, theo **số hàng ngang đã hỏi** tại thời điểm xét.

**Kích hoạt.** Thí sinh bấm nút *"Mở chướng ngại vật"* — bấm được **bất cứ lúc nào** trong vòng.

**Điều kiện.**

- **Biến quyết định băng điểm là *số hàng ngang KHÔNG còn ở trạng thái chờ***, không phải số miếng ghép đã mở.
- Băng: **1 hàng → 60** · **2 → 50** · **3 → 40** · **4 → 30**. Sau khi **gợi ý cuối đã đưa ra**: **20**, không phụ thuộc câu ô trung tâm đúng hay sai. **20 là sàn.**
- Nút này được xếp là **chuông** ⇒ chỉ nhận click chuột, **tự khoá khi bấm**, phía thí sinh không có dialog. Mỗi ghế phát **một** tín hiệu cho cả vòng — cơ chế *"một lần đoán, sai thì loại"* của luật gốc được giữ nguyên.
- Hàng đợi **CHẶN**: admin xác nhận rồi tín hiệu mới có hiệu lực.

**Bảng quyết định**

| Ca | Điều kiện | Kết quả | Thay đổi trạng thái |
|---|---|---|---|
| C1 | Chưa hàng ngang nào được hỏi; admin xác nhận rồi chấm **Đúng** | **+60** | Sinh event điểm; vòng kết thúc |
| C2 · C3 · C4 | Như C1, đã hỏi **2** · **3** · **4** hàng | **+50** · **+40** · **+30** | Như C1 |
| C5 | Sau khi gợi ý cuối đã đưa ra | **+20** | Như C1 |
| C6 | Admin chấm **Sai** | Chuyển `GR-010` — thí sinh **bị loại** khỏi vòng | Đặt cờ bị loại |
| C7 | Bấm khi đã hỏi 1 hàng, admin xác nhận muộn hơn | **Băng chốt theo trạng thái tại mốc ADMIN XÁC NHẬN.** Tình huống *"số hàng đổi giữa hai mốc"* **không dựng được**: hàng đợi **chặn**, nên chừng nào tín hiệu còn chờ duyệt thì **không hàng ngang nào mở thêm được** | Băng chốt tại mốc xác nhận |
| C8 | Admin bấm **No** | Tín hiệu kế tiếp lên; **thí sinh không mất lượt** | **Chưa tác dụng phụ nào** — cùng lập luận `GR-007` C2 |
| C9 · C10 | Ghế **đã bị loại** bấm tiếp · **đã có người giải đúng** | Nút **không hiển thị**, bấm **không phản hồi** | Không đổi |
| C11 | Cấu hình 5-8 hàng ngang | **Băng điểm lấy từ cấu hình, không suy ra từ luật.** `rowCount` **không khoá cứng ở 4**; đổi lại thang điểm là **mảng cấu hình BẮT BUỘC** dài bằng `rowCount`. Thiếu mảng đó là **thiếu cấu hình** ⇒ chặn tại contest builder, **không** phải tình huống lúc chạy | Băng đọc từ mảng cấu hình |
| C12 | Admin **đánh dấu đã hỏi** một ô bằng tay | **Băng tụt một bậc y như một lượt hỏi thật.** **Không ai được cộng điểm** từ thao tác này | Ô sang *đã hỏi*; `AuditLog` ghi **do-admin** để phân biệt với do-luồng |
| C13 | Admin **lộ đáp án** một ô | **Băng KHÔNG đổi** — biến đếm là *đã hỏi*, không phải *đã lộ*. Nhờ vậy mở tay trọn một ô chỉ tính **một lần** | Ô sang *mở* |
| C14 | Cần cộng điểm cho tình huống ngoài luật | **Admin tự cộng tay** qua điều chỉnh điểm — có event, có tên người bấm, **hoàn nguyên được** | Sinh event điều chỉnh điểm |

**Không đổi gì.**

- Điểm hàng ngang đã kiếm được của người giải **sai**: **GIỮ NGUYÊN, không bị trừ.** *"Bị loại khỏi phần thi này"* = mất mọi **quyền** trong vòng; nguồn **không có** mệnh đề trừ điểm nào.
- Lượt của thí sinh bị từ chối · lịch sử tín hiệu.
- **Thao tác mở/đóng bằng tay của admin KHÔNG tự sinh điểm cho ai** — nó đổi **giá** của Chướng ngại vật, không phải **điểm** của thí sinh. Điểm ở VCNV chỉ đến từ **ba** đường: câu hàng ngang chấm Đúng · chuông *"Mở chướng ngại vật"* chấm Đúng · điều chỉnh điểm thủ công.

**Thứ tự đánh giá.** Tất định: **(1) admin xác nhận tín hiệu → (2) chốt số hàng ngang đã hỏi → (3) admin phán quyết.**

- (1) trước (2) vì mốc tính điểm là **thời điểm tín hiệu có hiệu lực**, nên cũng là lúc đọc trạng thái bàn cờ.
- (2) trước (3) để băng cố định **trước** khi phán quyết — kết quả không phụ thuộc admin chấm nhanh hay chậm.

**Biên.** Chưa hàng nào bắt đầu: vẫn thuộc băng **60**. Sau gợi ý cuối: **20**, là **sàn**.

**Đồng thời.** Hàng đợi xử lý thuần theo thứ tự tới, không ưu tiên theo loại tín hiệu.

> **Băng điểm không bị đe doạ** bởi thứ tự này: hàng đợi **chặn**, nên tín hiệu chọn hàng ngang đang chờ duyệt **chưa** làm tăng số hàng đã hỏi. Dù tín hiệu CNV được duyệt trước hay sau, số hàng tại mốc xác nhận nó là **như nhau**.

**Ví dụ.**

- *Hợp lệ*: chưa hàng nào được hỏi, thí sinh vị trí 4 bấm, admin xác nhận, đáp án đúng ⇒ **+60**, vòng kết thúc.
- *Không hợp lệ*: gán phím tắt cho nút này ⇒ không có đường phát tín hiệu; nó là chuông.
- *Biên*: bấm khi vừa hỏi xong hàng thứ 4 nhưng gợi ý cuối **chưa** đưa ra ⇒ **30**, không phải 20.

**Nguồn**: luật gốc §VCNV đoạn *"Thí sinh có thể bấm chuông…"* · `QĐ-021`, `QĐ-023`, `QĐ-052`, `QĐ-057`

---

## GR-010 — VCNV: trả lời sai Chướng ngại vật

**Mục đích.** Xác định hệ quả khi thí sinh giải **sai** Chướng ngại vật.

**Kích hoạt.** Admin bấm **Sai** cho một tín hiệu giải Chướng ngại vật đã được xác nhận.

**Điều kiện.**

- Luật gốc: *"Nếu trả lời sai Chướng ngại vật, thí sinh sẽ **bị loại khỏi phần thi này**."*
- Nguồn **không** nêu hình phạt trừ điểm. Không được tự thêm.
- *"Bị loại"* có phạm vi **một vòng** — thí sinh vẫn thi các vòng sau và vẫn có thể thắng trận.

**Bảng quyết định**

| Ca | Điều kiện | Kết quả | Thay đổi trạng thái |
|---|---|---|---|
| C1 | Admin bấm **Sai** | **Bị loại khỏi VCNV**; **không** trừ điểm | Đặt cờ bị loại |
| C2 | Người bị loại **chưa dùng** lượt chọn | Lượt dồn sang vị trí tiếp theo | Cập nhật người tới lượt |
| C3 | Người bị loại **đã có +10** từ hàng ngang trước đó | **GIỮ NGUYÊN, không bị trừ lại.** *"Bị loại khỏi phần thi này"* tước **quyền tham gia**, không tước **điểm đã ghi** | Điểm không đổi; chỉ đặt cờ trong phạm vi vòng |
| C4 | Ghế đã bị loại tiếp tục thao tác | Máy thí sinh **không hiển thị gì**, bấm **không phản hồi** | Không đổi |
| C5 | **Người cuối cùng** bị loại | Chuyển `GR-012` | Vòng kết thúc |
| C6 `[v2]` | Một thành viên đội trả lời sai | Loại **cả đội** | Đặt cờ cho cả đội |

**Không đổi gì.** Thí sinh **không** bị loại khỏi trận — các vòng sau vẫn tham gia bình thường · điểm của người bị loại và của mọi người khác · **nguồn không quy định hình phạt điểm ⇒ không được tự thêm**.

**Thứ tự đánh giá.** Không còn thứ tự nào phải quy định. *"Người đó còn tín hiệu khác trong hàng đợi"* **không tồn tại** — nút chuông tự khoá sau lần bấm đầu. Hai hệ quả còn lại (đặt cờ bị loại, dồn lượt chọn) là hai hệ quả **độc lập** của cùng một phán quyết.

**Biên.** **1** người bị loại: lượt dồn sang vị trí tiếp theo. **Toàn bộ** bị loại: chuyển `GR-012`. Số lần đoán mỗi người: luật hàm ý **1** — sai là loại, và nút chuông tự khoá giữ đúng con số đó.

**Đồng thời.** Hai tình huống, cả hai đều đã có quy tắc:

| Tình huống | Xử lý |
|---|---|
| Phán quyết **Sai** được chốt **trong lúc đồng hồ hàng ngang đang chạy** | Đồng hồ **chạy tiếp bình thường**, không dừng, không kéo dài. Câu hàng ngang **tiếp tục** cho những người còn quyền |
| Người vừa bị loại **đang có một đáp án hàng ngang chờ chấm** | Bản đó **vẫn được chấm bình thường** nếu gửi **trước** mốc phán quyết Sai. Máy **không** tự huỷ nó — máy không phán quyết; admin là người quyết |

> Mốc bị loại là **mốc admin bấm**, tuyệt đối, không ân hạn. Mọi thứ trước mốc đó hợp lệ; sau mốc đó nút của người này không còn render.

**Ví dụ.**

- *Hợp lệ*: thí sinh vị trí 1 giải sai ⇒ bị loại khỏi VCNV, vẫn thi Tăng tốc và Về đích bình thường.
- *Không hợp lệ*: hệ thống trừ 30 điểm vì giải sai ⇒ nguồn không quy định hình phạt điểm.
- *Biên*: người thứ tư giải sai ⇒ toàn bộ đã bị loại, chuyển `GR-012`.

**Nguồn**: luật gốc §VCNV câu cuối · `QĐ-010`, `QĐ-023`, `QĐ-031`, `QĐ-057`

---

## GR-011 — VCNV: ô trung tâm và gợi ý cuối

**Mục đích.** Xác định điều kiện đưa ra **gợi ý cuối** ở ô trung tâm và điểm cho giai đoạn cuối vòng.

**Kích hoạt.** Cả **4 hàng ngang đã được hỏi** mà chưa ai giải đúng Chướng ngại vật.

**Điều kiện.**

- Điều kiện kích hoạt phải đọc theo **hai chủ ngữ khác nhau** của nguồn: *"từ hàng ngang được mở"* **≠** *"miếng ghép được mở"*. Cả 4 hàng ngang **luôn được hỏi hết**, nên giai đoạn này **luôn tới được**.
- Câu ô trung tâm: đúng **+10** và ô mở; sai thì ô **không** mở.
- Sau gợi ý cuối, giải đúng Chướng ngại vật chỉ được **20 điểm** — mốc này gắn với việc **gợi ý cuối đã được đưa ra**, **không** phụ thuộc câu ô trung tâm đúng hay sai.
- Cửa sổ giải Chướng ngại vật sau gợi ý cuối: **15 giây**.

**Bảng quyết định**

| Ca | Điều kiện | Kết quả | Thay đổi trạng thái |
|---|---|---|---|
| C1 | 4 hàng ngang đã hỏi, chưa ai giải | Gợi ý cuối được đưa ra ở ô trung tâm | Chuyển giai đoạn; băng điểm hạ xuống **20** |
| C2 | Admin chấm **Đúng** câu ô trung tâm | **+10**; ô trung tâm mở | Sinh event điểm; ô sang *mở* |
| C3 | Admin chấm **Sai** | Ô **không** mở; băng điểm **vẫn là 20** | Không sinh điểm |
| C4 | Giải đúng Chướng ngại vật trong 15 giây | **+20** | Sinh event điểm; vòng kết thúc |
| C5 | Ai được trả lời câu ô trung tâm | **Mọi thí sinh CHƯA BỊ LOẠI**, **không theo lượt**. Câu ô trung tâm **luôn gõ máy**, bất kể mode | Ghi nhận đáp án của từng người còn quyền |
| C6 | Có hàng ngang không ai trả lời đúng | Giai đoạn này **vẫn** tới được — điều kiện là *hàng ngang đã được hỏi*, không phải *miếng ghép đã mở* | Chuyển giai đoạn bình thường |
| C7 | Hết 15 giây không ai giải | Vòng khép lại | — |
| C8 | Admin bấm mở ô trung tâm **hai lần** | **Không tồn tại** — nút một chiều, tự tắt | Server bỏ qua lệnh trùng |

**Không đổi gì.** Miếng ghép của các hàng ngang · **việc đã đưa ra gợi ý cuối không bị hoàn tác** bởi kết quả sai của câu ô trung tâm · điểm của thí sinh đã bị loại.

**Thứ tự đánh giá.** Tất định vì **cả ba đều là mốc admin bấm**, và nút sau chỉ bật khi nút trước đã bấm: **(1) chấm câu ô trung tâm → (2) mở ô trung tâm → (3) mở cửa sổ 15 giây.**

> *"Ai kịp bấm chuông trong 15 giây"* vì vậy **không phụ thuộc tốc độ thao tác của admin**: cửa sổ mở tại mốc (3) và đóng đúng 15 giây sau mốc đó, chung cho mọi thí sinh còn quyền.

**Biên.** Đúng **4** hàng ngang đã hỏi: kích hoạt. Mốc **15.000 giây**: tín hiệu đúng mốc **vẫn hợp lệ** (biên đóng). Còn **0** người chưa bị loại: chuyển `GR-012`.

**Đồng thời.** Tín hiệu giải Chướng ngại vật đến **đúng lúc admin đang đưa ra gợi ý cuối**: băng chốt theo trạng thái tại **mốc admin xác nhận tín hiệu**, không theo mốc thí sinh bấm.

- Mốc *"đã đưa gợi ý cuối"* là **một cú bấm của admin**, nên có server timestamp rõ ràng — biên giữa băng **30** và băng **20** là một so sánh **tất định**, không phải vùng xám.
- Hàng đợi **chặn** ở VCNV ⇒ tín hiệu chờ duyệt **không** tự chuyển băng trong lúc chờ.

**Ví dụ.**

- *Hợp lệ*: 4 hàng ngang đã hỏi, 2 miếng ghép không mở vì không ai đúng; gợi ý cuối **vẫn** được đưa ra; thí sinh giải đúng ⇒ **+20**.
- *Không hợp lệ*: hệ thống chờ đủ 4 miếng ghép mở mới cho ra gợi ý cuối ⇒ đọc sai nguồn.
- *Biên*: bấm chuông ngay **trước** khi gợi ý cuối được đưa ra ⇒ băng **30**; ngay **sau** ⇒ **20**.

**Nguồn**: luật gốc §VCNV đoạn *"Sau khi cả 4 từ hàng ngang…"* · `QĐ-018`, `QĐ-052`, `QĐ-057`

---

## GR-012 — VCNV: toàn bộ thí sinh bị loại

**Mục đích.** Xác định cách kết thúc vòng khi **không còn thí sinh nào** đủ điều kiện tiếp tục.

**Kích hoạt.** Thí sinh cuối cùng chưa bị loại giải **sai** Chướng ngại vật.

**Điều kiện.**

- Vòng kết thúc; **không ai được điểm Chướng ngại vật**.
- Hàng ngang **chưa được hỏi** thì bị bỏ.
- **Mở toàn bộ miếng ghép và công bố Chướng ngại vật là thao tác THỦ CÔNG của admin**, không tự động, và **tuỳ chọn**.

**Bảng quyết định**

| Ca | Điều kiện | Kết quả | Thay đổi trạng thái |
|---|---|---|---|
| C1 | Mọi thí sinh đã bị loại | Vòng kết thúc; không ai được điểm Chướng ngại vật | Vòng về `LOBBY`; hàng ngang chưa hỏi bị bỏ |
| C2 | Admin bấm **công bố** | Mở mọi miếng ghép và hiện Chướng ngại vật cho viewer | Mọi ô sang *mở* |
| C3 | Admin **không** bấm công bố | **Không** phải trạng thái tắc — admin luôn kết thúc vòng được | Vòng vẫn kết thúc |
| C4 | Còn hàng ngang **chưa được hỏi** | Bị bỏ. Câu của nó **chưa hiển thị cho ai ⇒ CHƯA TIÊU, trả lại kho** | Cờ đã-dùng **không** đặt cho câu đó |
| C5 | Bấm công bố **hai lần** | **Không tồn tại** — nút một chiều, tự tắt | Server bỏ qua lệnh trùng |
| C6 | Tín hiệu đến sau khi vòng đã kết thúc | Nút **không hiển thị**, bấm **không phản hồi** | Không đổi |
| C7 | Còn đúng **1** người chưa bị loại | Rule chưa áp dụng; vòng chạy tiếp với người đó | Không đổi |
| C8 | Các thí sinh đã có điểm hàng ngang trước khi bị loại | **GIỮ NGUYÊN cho tất cả** — bị loại tước **quyền**, không tước **điểm**. Kể cả khi **cả sân** bị loại, điểm đã ghi vẫn mang sang vòng sau | Điểm không đổi |

**Không đổi gì.** Điểm hàng ngang đã kiếm được của **mọi** thí sinh · lịch sử tín hiệu · **câu của hàng ngang chưa hỏi không bị đánh dấu đã dùng**.

> **Câu chưa hiển thị thì CHƯA TIÊU.** Mốc tiêu câu là **hiển thị cho thí sinh**; một hàng ngang không bao giờ được chọn thì câu của nó chưa ai thấy, nên **trả lại kho**. Điều này khác hẳn câu **bị bỏ qua sau khi đã hiển thị** — cái đó tiêu.

**Thứ tự đánh giá.** Tất định: **(1) công bố — nếu admin chọn làm → (2) trả lại kho các câu chưa hiển thị → (3) đánh dấu vòng kết thúc.** Bước (1) là **tuỳ chọn** và không phải điều kiện để (3) xảy ra.

**Biên.** Số người bị loại = số thí sinh − 1: vòng **vẫn chạy tiếp**. = số thí sinh: áp dụng C1. Hàng ngang chưa hỏi từ 0 đến 4 — bao nhiêu cũng bị bỏ.

**Đồng thời.** Tín hiệu còn trong hàng đợi khi vòng kết thúc: **vô hiệu, nhưng không bị xoá**. Tín hiệu **gắn với ĐÍCH của nó** và vô hiệu khi đích đóng:

| Loại tín hiệu | Đích | Vô hiệu khi |
|---|---|---|
| Chọn hàng ngang | **lượt chọn** | Lượt chọn đó kết thúc |
| Trả lời hàng ngang | **câu** | Câu được chấm xong |
| *"Mở chướng ngại vật"* | **vòng** | Vòng VCNV kết thúc |

> ⇒ Việc *"hàng đợi đang hoạt động đặt lại sau mỗi vòng"* là **hệ quả tự nhiên** của quy tắc trên, không phải một quy tắc riêng. **Lịch sử tín hiệu giữ vĩnh viễn** — admin vẫn xem lại được để phân xử khiếu nại sau khi vòng đã đóng.

**Ví dụ.**

- *Hợp lệ*: cả 4 thí sinh đều giải sai ⇒ vòng kết thúc, không ai được điểm Chướng ngại vật; admin bấm công bố để khán giả thấy đáp án.
- *Không hợp lệ*: hệ thống **tự động** mở toàn bộ miếng ghép ngay khi người cuối bị loại.
- *Biên*: còn đúng 1 thí sinh chưa bị loại ⇒ rule chưa áp dụng, vòng tiếp tục với người đó.

**Nguồn**: luật gốc §VCNV · `QĐ-020`, `QĐ-044`, `QĐ-057`

---

## GR-013 — Tăng tốc: xếp hạng tốc độ

### Status

CONFIRMED

### Purpose
Định nghĩa cách tính điểm ở vòng Tăng tốc dựa trên thứ tự trả lời đúng (tốc độ), không phải bấm chuông hay tích luỹ.

### Actors
- Thí sinh: trả lời câu
- Server: ghi nhận timestamp, xếp hạng người đúng
- Admin: (không phán xử — O26 không xác định vai trò admin trong xếp hạng)

### Related states
- `rounds[i]` với `format: 'ranked-speed'` (TANG_TOC)
- Submission đã được ghi nhận từ R-TT-03 (last-wins)
- Trạng thái admin: **chưa chấm / chấm Đúng / chấm Sai** (R-GEN-03)

### Trigger
Vòng Tăng tốc bắt đầu, hoặc admin chấm điểm một câu ở vòng này.

### Preconditions
- Vòng Tăng tốc đang chạy (`TANG_TOC`)
- ≥1 thí sinh trả lời ≥1 câu
- Admin đã bấm Đúng cho một hoặc nhiều thí sinh (R-GEN-03)

### Inputs
- Tập thí sinh chấm Đúng câu đó
- Server-received timestamp của submission từng người (R-TT-03, R-GEN-05)
- Thứ tự thí sinh từ `tieRule` cấu hình (mặc định: `tieRule: 'millisecond'`, xem R-TT-02)

### Conditions
1. Điểm khi người đúng ≤ số người chơi (4 mặc định, 1-12 tổng quát)
2. Điểm khi hoà tốc độ → tính theo R-TT-02 (cùng mức điểm)
3. Điểm khi người sai → 0 (R-TT-01)

### Decision table

| Case | Conditions | Expected outcome | State change | Error |
|---|---|---|---|---|
| C1 | 4 TS chơi, 3 người trả lời đúng, server timestamp 1st→2nd→3rd | +40 (1st) / +30 (2nd) / +20 (3rd) · 4th không cộng (sai hoặc chưa đúng) | EventLog: SCORE_ADJUST ×3 cho 3 người | — |
| C2 | 4 TS chơi, 2 TS cùng timestamp (đứng đầu), 1 TS thứ 2 | +40 (cả 2 người 1st) / +20 (người 2nd) — không có người đứng 30 điểm | EventLog: SCORE_ADJUST ×2 → 40, ×1 → 20 | — |
| C3 | 4 TS chơi, 1 TS đúng | +40 (chỉ người đó) | EventLog: SCORE_ADJUST ×1 → 40 | — |
| C4 | 4 TS chơi, 0 người đúng | Không ai cộng điểm | Không sinh EventLog điểm | — |
| C5 | Ghế ≠ 4 (ví dụ 6 thí sinh), 5 người trả lời đúng | **NGOÀI PHẠM VI v1** `[v1.5]` — v1 đặc tả cho **đúng 4 thí sinh** (§2); thang điểm cho dải 1-12 người thuộc v1.5 (`game-rules-inventory.md` U-2) | — | — | 
| C6 | Admin bấm Sai (không phải Đúng) | Người đó nhận 0 điểm, không tính trong xếp hạng | Không sinh EventLog điểm (hoặc SCORE_ADJUST delta=0) | — |

### Outcomes
- **Khi chấm Đúng**: thí sinh nhận điểm 40/30/20/10 theo thứ hạng tốc độ (xem R-TT-02 điều kiện hoà)
- **Khi chấm Sai**: thí sinh không nhận điểm (0)
- **Khi chưa chấm**: không sinh sự kiện điểm (trạng thái lơ lửng, không tính vào bảng xếp hạng trận)

### State changes
- EventLog: thêm event loại `SCORE_ADJUST` (hoặc tương tự) ghi nhận điểm cộng cho ≤4 thí sinh (phụ thuộc thứ hạng)
- Bảng xếp hạng trận (match scoreboard): cập nhật điểm tích luỹ của thí sinh được chấm
- **Chú ý Đ-5.3.1**: một câu = MỘT event điểm cho TOÀN BỘ người chơi (xem điểu kiện C2: tính lại xếp hạng, cộng điểm để "share-high")

### No-change guarantees
- Timestamp của submission người khác không thay đổi (server time là source of truth, R-GEN-05)
- Các câu trước không bị ảnh hưởng (tính điểm độc lập per-câu)
- Người thi chính của Về đích không bị ảnh hưởng (vòng khác)

### Error outcomes
- **Người chấm không phải admin**: server reject (permission)
- **Thí sinh chưa trả lời**: không thể chấm → UI khóa nút, hoặc server skip
- **Admin chấm câu thuộc vòng đã bị bỏ**: máy admin hiện toast, không thực hiện được (đóng `game-rules-review.md` GRR-161)

### Evaluation order
1. Ghi nhận tập người chấm Đúng từ admin
2. Sắp xếp theo timestamp (R-GEN-05) với độ phân giải **millisecond** (R-TT-02, K-8)
3. Xử lý hoà tốc độ (R-TT-02): gán cùng mức điểm, nhảy bậc
4. Sinh event điểm cho từng thí sinh theo hạng cuối cùng
5. Cập nhật bảng xếp hạng trận

### Boundaries
- **Số TS < 1**: không chơi, không tính luật
- **Số TS = 1**: người đó bắt buộc 40 điểm nếu chấm Đúng
- **Số TS = 4**: thang 40/30/20/10 (luật O26 gốc)
- **Số TS = 5-12**: `[v1.5]` — ngoài phạm vi luật v1 (§2: v1 = đúng 4 thí sinh); thang điểm cho dải này thuộc v1.5 (U-2)
- **Số TS ≥ 13**: không hỗ trợ (UI max 12 ghế)

### Idempotency
Bấm Đúng hai lần câu và thí sinh → **một event** (R-GEN-03 idempotency). Bấm Sai sau Đúng → **revert + event mới** (Đ-5.3, R-GEN-03).

### Concurrency
Không tồn tại phán quyết đồng thời: **mỗi contest chỉ có MỘT admin duy nhất** (`Đ-18`), nên mọi thao tác chấm luôn tuần tự. Quy tắc *"một câu = MỘT event cho toàn bộ"* (`Đ-5.3.1`) áp trên một dòng thao tác duy nhất.

### Examples
**Hợp lệ:**
- 4 TS (A-B-C-D): A trả lời 100ms, B trả lời 150ms, C trả lời 200ms, D không trả lời. Admin chấm Đúng A/B/C → A +40, B +30, C +20.
- 4 TS (A-B-C-D): A trả lời 100ms, B trả lời 100ms, C trả lời 150ms, D không trả lời. Admin chấm Đúng A/B/C → A +40, B +40 (cùng mốc 100ms), C +10 (nhảy từ 2nd → 3rd).
- 1 TS solo: trả lời đúng → +40.

**Không hợp lệ:**
- 6 TS, thang điểm 40/30/20/10 chỉ có 4 giá trị → `[v1.5]`, ngoài phạm vi luật v1.

### Source traceability
- **Định nghĩa thứ hạng tốc độ**: `game-rules-inventory.md` R-TT-01 (ranked-speed)
- **Công thức điểm 40/30/20/10**: `game-rules-inventory.md` R-TT-01 · `game-rules-decisions.md` §9.3
- **Hoà tốc độ**: `game-rules-inventory.md` R-TT-02 · `game-rules-decisions.md` §3.4 `Đ-5.3.1`
- **Server time**: `game-rules-inventory.md` R-GEN-05 · `CLAUDE.md` §Quy ước
- **Idempotency & Revert**: `game-rules-decisions.md` §3.3, §7.1 `Đ-5.3`

---

## GR-014 — Tăng tốc: đồng thời gian

### Status

CONFIRMED

### Purpose
Quy định xử lý khi hai hay nhiều thí sinh trả lời đúng trong cùng một khoảng thời gian (hoà tốc độ).

### Actors
- Thí sinh: trả lời câu cùng thời điểm
- Server: so sánh timestamp, xếp hạng
- Admin: chấm Đúng/Sai

### Related states
- Vòng Tăng tốc, đã ghi nhận submission từ R-TT-03
- Trạng thái timestamp của ≥2 người: `server-received` bằng nhau ở **MILLISECOND** (số nguyên ms)

### Trigger
Admin chấm Đúng ≥2 thí sinh của cùng một câu, có server-received timestamp **bằng nhau ở mức millisecond**.

### Preconditions
- Vòng Tăng tốc đang chạy
- ≥2 thí sinh nộp submission có **server-received timestamp (ms) bằng nhau**
- Nội dung submission (sau trim) khác nhau hoặc giống nhau (điều này không ảnh hưởng quyết định hoà)

### Inputs
- Tập người hoà: `{TS1, TS2, … TSn}` có timestamp ms bằng nhau
- Vị trí thứ hạng hiện tại của tập hoà (trước khi chấm)
- Thang điểm câu: [40, 30, 20, 10]

### Conditions
1. **Độ phân giải so sánh: MILLISECOND** (số nguyên ms, server-received) — ✅ **chủ dự án chốt 2026-07-26**, lý do: **máy tính dễ tính toán**. So sánh là phép so hai số nguyên, không có làm tròn, không có số thực
2. Tất cả người hoà nhận **cùng mức điểm** (đó là điểm thứ hạng của nhóm) — `tieRule: 'share-high'`
3. Thứ hạng tiếp theo **nhảy qua** số người hoà: sau nhóm k người đồng hạng ở bậc n, người kế nhận bậc **n+k** (`GRR-032` — standard competition ranking)

> **Chênh với tiền lệ chương trình — đã biết và đã chấp nhận.** Chương trình thật xếp thứ tự ở **hàng phần trăm giây** (xem **Source traceability**), tức cửa sổ hoà của hệ thống này **hẹp hơn 10 lần**: hai bản cách nhau 3 ms sẽ **được phân định** ở đây trong khi chương trình thật coi là **hoà**. Hệ quả thực tế: luật *"cùng nhận một mức điểm"* vẫn đúng về hành vi nhưng **rất ít khi được kích hoạt**.
>
> Đây là **lựa chọn có chủ đích**, không phải sai sót. Nếu sau này muốn khớp chương trình thật thì **không phải sửa luật** — chỉ đổi `tieRule` sang mức 10 ms, và phép so vẫn là số nguyên (`timestamp / 10`), không phát sinh số thực. Ghi ở đây để lần sau không ai phải điều tra lại.

### Decision table

| Case | Conditions | Expected outcome | State change | Error |
|---|---|---|---|---|
| C1 — **ca kinh điển 40/40/20/10** | 4 TS đều đúng: A và B cùng **5420 ms**, C **7100 ms**, D **9800 ms** | A +40, B +40 (cùng bậc 1) · C **+20** (bậc 1+2 = **3**, bỏ qua bậc 2) · D **+10** (bậc 4) | EventLog: **một** event điểm cho cả bảng | — |
| C1b | Cùng C1 nhưng D **sai** | A +40, B +40 · C +20 · D +0 | EventLog: một event cho cả bảng | — |
| C2 | 4 TS, A, B, C cùng **5420 ms** đúng; D **7100 ms** **đúng** | A +40, B +40, C +40 · D **+10** (bậc 1+3 = **4**) | EventLog: một event cho cả bảng | — |
| C2b | Cùng C2 nhưng D **SAI** | A +40, B +40, C +40 · D +0 (sai = 0, không trừ) | EventLog: một event cho cả bảng | — |
| C3 | 4 TS, A **3200 ms**; B và C cùng **5420 ms**; D **7100 ms** — cả 4 đúng | A +40 · B +30, C +30 (cùng bậc 2) · D **+10** (bậc 2+2 = **4**) | EventLog: một event cho cả bảng | — |
| C4 | 4 TS, A **3200 ms**; B và C cùng **5420 ms** đúng; D sai | A +40 · B +30, C +30 · D +0 | EventLog: một event cho cả bảng | — |
| C5 — biên của lựa chọn ms | A **5423 ms** vs B **5429 ms** | **KHÔNG hoà** — A +40, B +30. Chênh 6 ms là **phân định được** ở độ phân giải ms. *(Chương trình thật coi đây là **hoà** vì cả hai là 5,42 s — chênh đã biết, xem Conditions)* | EventLog: một event cho cả bảng | — |
| C6 | A **5420 ms** vs B **5421 ms** — chênh đúng **1 ms** | **KHÔNG hoà** — A +40, B +30. 1 ms là đơn vị nhỏ nhất, vẫn phân định | EventLog: một event cho cả bảng | — |

### Outcomes
- **Tất cả người hoà**: nhận điểm của thứ hạng soonest của nhóm (ví dụ hoà ở hạng 2 → tất cả +30)
- **Người còn lại** (chấm Đúng nhưng không hoà): nhận điểm theo hạng không bị chiếm bởi nhóm hoà
- **Điểm không bao giờ âm**: nhảy bậc không sinh điểm "trả lại" (chỉ sinh event +X, không −Y)

### State changes
- EventLog: cập nhật điểm cho tất cả người hoà với giá trị **cùng nhau**
- Bảng xếp hạng: thay đổi thứ hạng trận của người hoà (nâng lên vị trí cao hơn nếu trước đó chưa được chấm)

### No-change guarantees
- Submission nội dung của mỗi người không thay đổi (hoà là về thời gian, không phải nội dung)
- Timestamp của người không hoà không ảnh hưởng (họ được chấm Đúng hay Sai là quyết định riêng)
- Các câu khác không bị ảnh hưởng

### Error outcomes
- **Cấu hình `tieRule` khác mặc định**: `tieRule` là **RuleConfig**, admin đổi được. Mặc định `O26_DEFAULT@1` = **`share-high` ở millisecond**. Hai option còn lại vẫn cấu hình được: **10 ms** (khớp chương trình thật) và `'microsecond'` của SPEC §6 — riêng `'microsecond'` thì điều khoản *"cùng nhận một mức điểm"* trên thực tế **không bao giờ chạy**, nên **không dùng cho preset `O26_DEFAULT@1`**
- **Chỉ 1 người đúng**: không phải hoà, không tính luật này
- **Nhiều bản gửi của cùng một người**: mốc dùng để xếp hạng là bản **CUỐI CÙNG** hợp lệ, theo GR-015 (last-wins) — không liên quan luật hoà ở đây

### Evaluation order
1. Admin chấm Đúng/Sai cho **từng** thí sinh; chỉ người được chấm **Đúng** vào tập xếp hạng
2. Lấy **bản cuối cùng hợp lệ** của mỗi người (GR-015, last-wins) và thời gian trả lời của bản đó
3. So sánh **trực tiếp trên số nguyên millisecond** — không làm tròn, không số thực
4. Nhóm những người có giá trị bằng nhau = **một bậc**
5. Gán **cùng mức điểm** cho cả nhóm (`share-high`)
6. Bậc kế tiếp = bậc hiện tại **+ số người trong nhóm** (`GRR-032`)
7. Admin bấm **"chốt câu"** ⇒ phát **một** event điểm cho toàn bộ bảng (`Đ-5.3.1a`, `Đ-5.3.1b`)

### Boundaries
- **Số người hoà = 0**: không áp luật (0 người có cùng timestamp → không hoà)
- **Số người hoà = 1**: không áp luật (1 người không thể "hoà" với ai)
- **Số người hoà = 2-4**: luật bình thường
- **Số người hoà ≥ 5** (chỉ xảy ra khi >4 ghế): `[v1.5]` — ngoài phạm vi luật v1.

### Idempotency
Chấm Đúng nhóm hoà 2 lần → **một event** (dedup). Chấm Sai sau Đúng → revert + event mới.

### Concurrency
Nếu 2 admin chấm người khác nhau cùng câu: xử lý theo mô hình v1 (không tới). V2 cần chốt **khoá câu** hoặc **atomic update**.

### Examples
**Hợp lệ:**
- 4 TS đều đúng: A **5420 ms**, B **5420 ms**, C **7100 ms**, D **9800 ms** ⇒ **40 / 40 / 20 / 10** — đúng ví dụ kinh điển mà nguồn hàm ý.
- A, B, C cùng **5420 ms** đúng, D **7100 ms** đúng ⇒ **40 / 40 / 40 / 10** (nhóm 3 người ở bậc 1 ⇒ bậc kế = 1+3 = 4).

**Không hợp lệ:**
- Dùng `tieRule: 'microsecond'` cho preset `O26_DEFAULT@1` ⇒ luật hoà thành mã chết.
- Xếp hạng trên timestamp của **bản gửi đầu** thay vì **bản cuối** ⇒ trái GR-015 (last-wins).

### Source traceability

**`K-8` ĐÃ PHÂN XỬ 2026-07-26 — giữ MILLISECOND theo quyết định của chủ dự án, biết rõ nó lệch tiền lệ chương trình.**

Điều tra tiền lệ ngày 26/07 cho ra kết quả **ngược** với lựa chọn `ms`; chủ dự án vẫn chọn `ms` với lý do **máy tính dễ tính toán**. Ghi cả hai phía để quyết định này về sau không bị đọc là sai sót:

| Nguồn | Nói gì về độ phân giải | Hạng nguồn |
|---|---|---|
| `docs/source/fandom-olympia-26-luat-choi.md` §Tăng tốc | *"trong cùng một **khoảng thời gian**"* — **KHÔNG nêu độ phân giải** | **Source of truth** (D8) |
| [`W26` vi.wikipedia — Olympia 26](https://vi.wikipedia.org/wiki/%C4%90%C6%B0%E1%BB%9Dng_l%C3%AAn_%C4%91%E1%BB%89nh_Olympia_n%C4%83m_th%E1%BB%A9_26) | *"Nếu có 2 thí sinh trở lên cùng trả lời đúng trong cùng một thời gian hệ thống ghi nhận (**tính đến 2 chữ số thập phân**), họ sẽ cùng giành được số điểm tương ứng."* | Nguồn ngoài, **cụ thể** |
| [`W` vi.wikipedia — Đường lên đỉnh Olympia](https://vi.wikipedia.org/wiki/%C4%90%C6%B0%E1%BB%9Dng_l%C3%AAn_%C4%91%E1%BB%89nh_Olympia) | *"Tùy theo thứ tự thời gian (**được tính đến hàng phần trăm**) trả lời đúng, thí sinh sẽ ghi được 40, 30, 20 và 10 điểm."* | Nguồn ngoài, **xác nhận độc lập** |
| `plans/.../research/rules-2026.md` §7 | *"độ phân giải ms server-received"* | **BẢN NHÁP tự viết trong repo** |

**Lập luận cũ của `K-8` KHÔNG đứng được — nhưng kết luận vẫn được giữ vì lý do khác.** `K-8` cũ loại `W26` *"theo D8"*; thực ra **D8 chỉ cho phép loại `W26` ở chỗ Fandom nói KHÁC**, mà ở đây Fandom **im lặng** hoàn toàn về độ phân giải nên không có gì để loại bằng. Con số `ms` đến từ `R26` §7 = **file nháp trong repo**, không phải nguồn; `D13.3` chỉ chốt **hành vi**, không chốt độ phân giải.

⇒ Căn cứ thật của `ms` **không phải nguồn luật**, mà là **quyết định kỹ thuật của chủ dự án**: so hai số nguyên ms là phép so đơn giản nhất, không làm tròn, không số thực. `game-rules-inventory.md` R-TT-02 ghi đúng cái giá phải trả: *"chọn ms làm cửa sổ hoà **hẹp hơn 10 lần** so với `W26` → ít trường hợp hoà hơn thực tế chương trình."*

> **Đường thoát nếu đổi ý**: đổi `tieRule` sang mức 10 ms. Phép so vẫn là số nguyên — chia nguyên cho 10 rồi so — nên **không** đánh đổi gì về độ phức tạp tính toán. Luật, decision table và thang nhảy bậc **không phải sửa**.

**Bề rộng tiền lệ**: điều khoản chia điểm khi đồng thời gian tồn tại **từ Olympia 7** ([Fandom — Tăng tốc](https://duong-len-dinh-olympia.fandom.com/vi/wiki/T%C4%83ng_t%E1%BB%91c): *"Từ Olympia 7, điểm số đã được tính theo tiêu chí thí sinh trả lời đúng và nhanh nhất. Trong trường hợp có nhiều thí sinh cùng trả lời đúng trong cùng một khoảng thời gian, những thí sinh đó sẽ cùng ghi được một mức điểm…"*) ⇒ khoảng **19 mùa** liên tục, không phải điều khoản mới của O26.

- **Định nghĩa hoà tốc độ**: `game-rules-inventory.md` R-TT-02 · `docs/source/fandom-olympia-26-luat-choi.md` §Tăng tốc
- **Nhảy bậc khi hoà**: `GRR-032` — standard competition ranking (đóng mục *"chưa định nghĩa"* của R-TT-02)
- **Một câu = một event điểm**: `game-rules-decisions.md` §3.4 · `Đ-5.3.1a`, `Đ-5.3.1b`
- **Server time**: `game-rules-inventory.md` R-GEN-05 · GR-035 (đồng hồ đơn điệu)

---

## GR-015 — Tăng tốc: ghi nhận bản cuối

### Status

CONFIRMED

### Purpose
Quy định Tăng tốc không khoá input sau khi trả lời lần đầu, và hệ thống tính điểm dựa trên **bản submission cuối cùng** (last-wins), không phải bản đầu.

### Actors
- Thí sinh: gửi/gửi lại submission tự do
- Server: ghi nhận tất cả lần gửi, chỉ lấy bản cuối (per-seat)
- Admin: chấm Đúng/Sai dựa trên bản cuối

### Related states
- Vòng Tăng tốc đang chạy
- Input chưa được khoá (R-GEN-03: Tăng tốc không có `autoJudge`, admin chấm; nhưng UI không disable input)
- Submission history: ghi nhận tất cả lần gửi (server-side)

### Trigger
Thí sinh gửi lần 1 → sửa (gửi lần 2) → … → gửi lần N. Admin chấm Đúng/Sai câu đó.

### Preconditions
- Vòng Tăng tốc, timer câu vẫn chạy hoặc đã hết
- Thí sinh chưa hết timeout toàn câu (timeout là deadline chung, không per-lần gửi)
- Submission (sau trim) là **khác rỗng** hoặc **y hệt bản trước** (xem C3 dưới)

### Inputs
- Tất cả submission của thí sinh ấy trong câu: `[submit_1, submit_2, … submit_n]`
- Server-received timestamp của mỗi lần gửi
- Nội dung (sau trim) của lần gửi cuối: `submit_n`

### Conditions
1. Nội dung cuối **có thay đổi** so với trước → timestamp cập nhật = timestamp lần gửi mới
2. Nội dung cuối **y hệt** bản trước → **không cập nhật timestamp** (giữ timestamp lần gửi đầu tiên khai nội dung đó)
3. Nội dung cuối **rỗng** (toàn whitespace sau trim) → **bỏ qua, giữ bản trước** (R-TT-03 define)

### Decision table

| Case | Conditions | Expected outcome | State change | Error |
|---|---|---|---|---|
| C1 | TS gửi "Hà Nội" lúc 100ms, rồi "Hanoi" lúc 150ms, cuối cùng "Hà Nội" lúc 200ms | **Bản cuối = "Hà Nội"**. Timestamp = 100ms (lần đầu khai "Hà Nội", bản 200ms là **y hệt** nên không cập nhật) | Server lưu `lastSubmission = "Hà Nội" @ 100ms` (timestamp đầu) | — |
| C2 | TS gửi "Hà Nội" lúc 100ms, rồi "Hanoi" lúc 150ms, sau cùng "Hà Nội khác" lúc 200ms | **Bản cuối = "Hà Nội khác"** (khác bản 100ms). Timestamp = 200ms (cập nhật vì nội dung mới) | Server lưu `lastSubmission = "Hà Nội khác" @ 200ms` | — |
| C3 | TS gửi "Hà Nội" lúc 100ms, sau "   " (3 space, rỗng sau trim) lúc 150ms | **Bản cuối = "Hà Nội"** (bản 150ms bị SKIP vì rỗng). Timestamp = 100ms | Server lưu `lastSubmission = "Hà Nội" @ 100ms` · event lần 150ms **không được ghi nhận** | — |
| C4 | TS gửi rỗng lúc 100ms, sau "Hà Nội" lúc 150ms | **Bản cuối = "Hà Nội"** (bản 100ms bị SKIP, bản 150ms là đầu tiên hợp lệ). Timestamp = 150ms | Server lưu `lastSubmission = "Hà Nội" @ 150ms` · bỏ qua bản rỗng 100ms | — |
| C5 | Có bản hợp lệ trước deadline, còn gửi thêm sau deadline | **GIỮ CẢ HAI**: bản hợp lệ cuối dùng cho xếp hạng, bản quá hạn tô **đỏ** để admin đối chiếu và quyết **Đúng / Sai** | Cả hai hiển thị trên màn admin; hiển thị ra ngoài và chấm đều **do admin bấm** | — |
| C6 | Chỉ gửi **sau** deadline, không có bản hợp lệ nào | Bản quá hạn tô **đỏ**; admin quyết **Đúng / Sai / HUỶ KẾT QUẢ**. Nếu công nhận thì **cả bảng xếp hạng của câu tính lại** (`Đ-5.3.1`) | Ghi vào lịch sử; kết quả câu theo phán quyết của admin | — |

### Outcomes
- **Bản cuối được chấm**: nếu admin chấm Đúng → người nhận điểm (xem GR-013 GR-014)
- **Bản cuối được so khớp** (nếu admin cần gợi ý): highlight so với đáp án đúng (R-GEN-04)
- **Nội dung bản trước không ảnh hưởng**: hoàn toàn bỏ qua, không tính điểm, không tính trạng thái

### State changes
- Server history: **giữ lịch sử đầy đủ** các submission (append-only) để admin xem lại nếu cần (`game-rules-review-old.md` GRR-115 - chưa chốt nhưng U-31 hỏi)
- Submission field của seat: cập nhật thành bản cuối
- Timestamp của seat: cập nhật theo quy tắc C1, C2, C3 trên

### No-change guarantees
- Đáp án đúng không thay đổi
- Submission của seat khác không ảnh hưởng
- Timestamp deadline không thay đổi (deadline = server-timeout, không liên quan)
- Điểm từ câu trước không ảnh hưởng

### Error outcomes
- **Submission quá deadline**: không tự tính, nhưng **vẫn giữ và hiển thị** để admin quyết (C5, C6)
- **Nút chấm khoá tới khi hết giờ** (Tăng tốc luôn gõ máy) ⇒ admin không thể chấm khi thí sinh còn đang sửa; chấm xong thì **nút gửi khoá** (`Đ-35`)
- **Toàn bộ submission rỗng trong câu**: không có bản nào để chấm ⇒ admin bấm **Sai** — *không trả lời* và *trả lời sai* là cùng một thao tác (`Đ-19`).

### Evaluation order
1. Thí sinh gửi submission (lần 1, 2, …, n)
2. Server ghi nhận tất cả (per-seat history)
3. Deadline câu đó
4. Admin chấm → server lấy bản cuối hợp lệ (skip rỗng, tuân C1-C4 ở trên)
5. Tính điểm dựa trên bản cuối (GR-013)
6. Tính timestamp xếp hạng dựa trên timestamp bản cuối (hoặc timestamp đầu nếu nội dung y hệt, C1)

### Boundaries
- **Số lần gửi = 0**: không gửi → submission rỗng (không chấm được, U-31)
- **Số lần gửi = 1**: bản duy nhất, timestamp = lần gửi duy nhất
- **Số lần gửi ≥ 2**: áp quy tắc last-wins + skip-empty
- **Khoảng thời gian gửi = 0** (gửi lại ngay tức thì cùng ms): nội dung khác → cập nhật, nội dung y hệt → không cập nhật (C1)

### Idempotency
Gửi lần 2 nội dung y hệt → **không cập nhật timestamp** (để tính xếp hạng dùng lần gửi đầu tiên nội dung đó). Gửi lần 3 nội dung khác → **cập nhật timestamp** thành lần 3.

### Concurrency
Nếu thí sinh gửi lại giữa admin chấm câu khác: server phải hỗ trợ concurrent update submission. V1 áp dụng, cần đảm bảo server-received timestamp được ghi chính xác.

### Examples
**Hợp lệ:**
- Thí sinh gửi "Paris" @ 100ms, rồi "London" @ 150ms, cuối cùng "Paris" @ 200ms → bản cuối = "Paris" @ 100ms (không cập nhật vì nội dung y hệt); admin chấm dựa trên "Paris".
- Thí sinh gửi "Paris" @ 100ms, "London" @ 150ms, "Berlin" @ 200ms → bản cuối = "Berlin" @ 200ms; xếp hạng tốc độ dùng timestamp 200ms.
- Thí sinh gửi "Paris" @ 100ms, "   " @ 150ms (rỗng), "Paris" @ 200ms → bản cuối = "Paris" @ 100ms (200ms y hệt, giữ 100ms); bản 150ms bị skip.

**Không hợp lệ:**
- Thí sinh gửi "Paris" @ 100ms, rồi gửi @ 40s khi deadline 30s → bản 40s bị loại, dùng "Paris" @ 100ms.
- Thí sinh không gửi gì (rỗng từ đầu) → admin bấm **Sai**; không tham gia xếp hạng (`Đ-19`).

### Source traceability
- **Last-wins nguyên tắc**: `game-rules-decisions.md` §9.3 ("tính BẢN CUỐI CÙNG")
- **Last-wins chi tiết**: `game-rules-inventory.md` R-TT-03 (last-wins, skip-empty, no-update-timestamp)
- **Không khoá input**: `CLAUDE.md` §UX BẮT BUỘC (Tăng tốc nhận MỌI lần trả lời đến khi hết giờ)
- **Server time**: `game-rules-inventory.md` R-GEN-05 · `CLAUDE.md` §Quy ước

---

## GR-016 — Về đích: thứ tự lượt thi

### Status

CONFIRMED

### Purpose
Định nghĩa thứ tự nào thí sinh được thi lượt đầu tiên, tiếp theo, ở vòng Về đích. Thứ tự tính lại sau mỗi lượt hoàn thành.

### Actors
- Thí sinh: xếp thứ tự
- Server: tính toán điểm tích luỹ, recommend thứ tự
- Admin: chọn người thi (override recommendation nếu cần, per Đ-5)

### Related states
- Vòng Về đích (VE_DICH)
- Trạng thái điểm tích luỹ sau khi hoàn tất Tăng tốc (được tính lại sau mỗi lượt)
- Vị trí ghế (TERM-002 — số thứ tự gán trước trận, dùng tie-break)

### Trigger
Bắt đầu vòng Về đích, hoặc sau khi một lượt Về đích hoàn thành.

### Preconditions
- Vòng Tăng tốc đã kết thúc
- Điểm tích luỹ của tất cả thí sinh đã được tính (sau chấm điểm Tăng tốc)
- Admin bấm "chọn lượt" để xác định ai sẽ thi

### Inputs
- Điểm tích luỹ của từng thí sinh: `score[i]` (có thể âm per Đ-2)
- Vị trí ghế của từng thí sinh: `position[i]` (1-4 chuẩn)
- Tập thí sinh **chưa thi lượt này** của vòng Về đích

### Conditions
1. Người điểm cao nhất tại thời điểm xếp lượt → đi trước
2. Nếu hoà điểm → người **vị trí thấp nhất** (nhỏ nhất, ví dụ pos 1 < pos 2) → đi trước
3. Lặp lại sau mỗi lượt hoàn thành (tính lại bảng xếp)

### Decision table

| Case | Conditions | Expected outcome | State change | Error |
|---|---|---|---|---|
| C1 | Sau TT: A=110đ/pos1, B=90đ/pos2, C=80đ/pos3, D=70đ/pos4 | Lượt 1: A (điểm cao nhất 110) | UI recommend A, admin xác nhận | — |
| C2 | Sau TT: A=110đ/pos2, B=110đ/pos1, C=90đ/pos3, D=80đ/pos4 | Lượt 1: B (110 điểm, hoà A nhưng pos 1 < pos 2) | Recommend B (tie-break = position) | — |
| C3 | Sau lượt 1 (A thi), A có −20 (sai cướp): A=90đ/pos1, B=110đ/pos2, C=80đ/pos3, D=70đ/pos4 | Lượt 2: B (110đ, cao nhất giây) | Tính lại ranking sau lượt 1 → recommend B | — |
| C4 | 4 TS hoà điểm: A/B/C/D =100đ với pos 1/2/3/4 | **Tie-break theo "vị trí thấp nhất" = A (pos 1)** | **`"vị trí"` = SỐ THỨ TỰ 1..N do BỐC THĂM** — không phải số ghế, không phải thứ hạng điểm (`GRR-040`). *"Vị trí đứng thấp nhất"* = **số vị trí NHỎ NHẤT**. Đóng `U-27` và `U-17` | — |
| C5 | Admin override recommendation: B có 90đ nhưng A có 110đ → admin chọn B thi lượt 1 | Chạy lượt B (không đúng recommendation) | **Dialog cảnh báo** (Đ-5): "thứ tự sai, vẫn thực hiện?" → admin Yes | Conflict luật (cảnh báo, không chặn) |

### Outcomes
- **Recommendation**: hệ thống hiển thị người nên đi (điểm cao nhất, tie-break vị trí)
- **Quyết định cuối**: admin chọn (có thể override recommendation, Đ-5)
- **Cảnh báo khi conflict**: hiện dialog Yes/No, không chặn cứng
- **Tính lại sau lượt**: điểm có thay đổi → recommendation thay đổi

### State changes
- UI: cập nhật hiển thị ranking (bảng điểm) sau mỗi lượt
- System: ghi nhận ai đã thi lượt nào (để tính "chưa thi")
- Điểm (nếu lượt hoàn thành): cập nhật score[i] sau chấm

### No-change guarantees
- Vị trí ghế không thay đổi trong trận
- Điểm từ Tăng tốc không bị ảnh hưởng (chỉ cộng thêm Về đích)
- Quyết định lượt trước không ảnh hưởng lượt sau (hoàn toàn độc lập)

### Error outcomes
- **Tất cả người đã thi (chưa lấy từ "chưa thi")**: không còn lượt → chuyển tie-break hoặc kết thúc (hệ thống không chặn cứng, admin quyết, Đ-5.1)
- **Tie-break toàn bộ** (4 người hoà): giải được — `"vị trí"` là **số bốc thăm 1..N**, người có số nhỏ nhất đi trước (`GRR-040`). Đóng `U-27`, `U-17`.

> `"Vị trí"` là một **giá trị dữ liệu của ghế trong contest**, đặt trước trận và không đổi giữa trận. **Quy trình bốc thăm ngoài đời** diễn ra khi nào so với lúc dựng contest là câu hỏi vận hành, ghi ở `Đ-4.5a` — nó **không** ảnh hưởng luật tie-break ở đây.

### Evaluation order
1. Tính điểm tích luỹ hiện tại của mỗi TS: `score[i] = sum(events từ Khởi động → Tăng tốc)`
2. Sắp xếp giảm theo `score[i]`
3. Nếu hoà: sắp xếp theo `position[i]` (tăng, tức nhỏ nhất lên trước)
4. Recommend TS đầu tiên trong danh sách sau sắp
5. Admin chọn (accept recommendation hoặc override) → xác nhận thi lượt
6. Lượt hoàn thành → quay lại bước 1

### Boundaries
- **Điểm cao nhất âm** (ví dụ tất cả −100): vẫn xếp theo luật (cao nhất = −100 > −110)
- **Vị trí = 0 hoặc > 4**: không tồn tại trong v1 — đúng 4 ghế, vị trí 1→4 (§2). Dải 1-12 thuộc `[v1.5]`.
- **Chưa thi ai cả** (lượt 1): recommend người điểm cao nhất + vị trí thấp nhất
- **Chưa thi 1 người**: lượt cuối cùng, tự động

### Idempotency
Không áp dụng (mỗi lượt là sự kiện độc lập). Tính lại recommendation sau mỗi lượt (có thể khác).

### Concurrency
Server tính lại ranking giữa các lượt: phải lấy snapshot điểm **sau khi lượt trước hoàn toàn kết thúc** (chấm xong, rút lệnh cấm nếu có). Không tính trong lúc câu chạy.

### Examples
**Hợp lệ:**
- 4 TS sau TT: A 110, B 90, C 80, D 70 → recommend A.
- A thi, sai cướp −20 → A 90. Bây giờ B 90 cùng A → recommend B (pos nhỏ hơn).
- A và B hoà 110 (pos 1 và 2) → recommend A (pos 1).

**Không hợp lệ:**
- Admin chọn D thi (nhỏ nhất) khi A là cao nhất → cảnh báo, nhưng cho admin bấm Yes/No.

### Source traceability
- **Thứ tự lượt Về đích**: `game-rules-inventory.md` R-VD-02 · `game-rules-decisions.md` §6.2, §9.4
- **Tính lại sau mỗi lượt**: `game-rules-inventory.md` R-VD-02 (*"tại thời điểm sau khi thí sinh ở lượt 1 hoàn thành phần thi"*)
- **Vị trí tie-break**: `game-rules-inventory.md` R-VD-02 (F26 "vị trị đứng thấp nhất"; W26 "số thứ tự nhỏ nhất")
- **Admin override + cảnh báo**: `game-rules-decisions.md` §6.1, §6.2 (Đ-5: advisory, không chặn)

---

## GR-017 — Về đích: chọn gói câu

### Status

CONFIRMED

### Purpose
Quy định thí sinh chọn 3 câu từ 2 mức điểm {20, 30} để tạo thành gói cá nhân trước khi bắt đầu thi lượt đó.

### Actors

**Chủ thể chọn phụ thuộc MODE TRẢ LỜI** (`Đ-40`, chốt 27/07 — cùng khuôn `Đ-36` của chọn hàng ngang):

| Mode | Ai chọn | Cơ chế |
|---|---|---|
| **Sân khấu** (mặc định) | **Admin bấm** | TS **nói gói của mình trên sân khấu**; admin nghe và bấm. Máy TS **không render** nút chọn gói — đây là **đường vào duy nhất**, không phải fallback |
| **Nhập liệu** | **Thí sinh** | TS tự chọn trên máy; admin **chọn hộ** default 20/20/20 nếu TS chưa chọn khi tới lượt (C3) |

- Server: xác nhận hợp lệ, lưu lựa chọn (validate lại bất kể client là ai — Zero-trust)

### Related states
- Vòng Về đích, trước khi câu hỏi được rút (draw)
- Trạng thái TS: **chưa chọn** / **đã chọn** gói
- Pool câu: danh sách câu 20đ và 30đ sẵn có

### Trigger
TS bước vào màn chọn gói, hoặc tới lượt mà TS chưa chọn.

### Preconditions
- Vòng Về đích đang chạy
- TS là người được xếp lượt hiện tại (GR-016)
- Pool câu còn đủ (pre-flight từng câu hỏi, chưa draw)
- Chế độ Về đích: `packageMode: 'preset'` (mặc định O26 — chỉ chọn mức 20/30 được)

### Inputs
- Tập mức điểm có sẵn: {20, 30} (preset O26; config khác = custom, chưa chốt v1)
- Số câu cần chọn: 3 (cố định O26)
- Thứ tự mức được chọn: [X, Y, Z] ∈ {20, 30}³

### Conditions
1. Lựa chọn **phải chứa 3 mức** từ tập {20, 30}
2. Các phối hợp hợp lệ: {20/20/20, 20/20/30, 20/30/30, 30/30/30}
3. Mỗi mục = **1 câu cần draw sau đó** (draw không liên hệ lựa chọn — chỉ draw từ pool mức tương ứng)

### Decision table

| Case | Conditions | Expected outcome | State change | Error |
|---|---|---|---|---|
| C1 | TS chọn 20/20/20 | Hệ thống chấp nhận, chuẩn bị draw 3 câu từ pool 20đ | `MatchState.packageChoice[seat] = [20, 20, 20]` · pool lock để reserve 3 câu 20đ | — |
| C2 | TS chọn 20/30/30 | Hệ thống chấp nhận (phối hợp hợp lệ) | `packageChoice[seat] = [20, 30, 30]` · lock reserve 1 câu 20đ + 2 câu 30đ | — |
| C3 | **Mode nhập liệu**, TS chưa chọn khi tới lượt (lượt bắt đầu) | **Admin chọn hộ: default 20/20/20** | `packageChoice[seat] = [20, 20, 20]` · admin có thể override trong dialog | — |
| C5 | **Mode sân khấu** — TS nói gói trên sân khấu | **Admin bấm chốt gói theo lời TS** (`Đ-40`). Không có mốc "quá hạn": admin vốn là người bấm | `packageChoice[seat] = [a, b, c]` · **last-wins** tới mốc khoá như C4 | — |
| C6 | **Mode sân khấu** — máy TS gửi lựa chọn gói | **Không tồn tại đường này**: nút không render; server **từ chối** nếu vẫn nhận được (Zero-trust) | Không đổi | Drop + AuditLog |
| C4 | TS có thể đổi gói sau khi chọn không? | **ĐỔI ĐƯỢC, cho tới mốc admin bấm "hiển thị câu đầu tiên của gói"** (`GRR-054`) | `packageChoice` **last-wins** tới mốc đó, rồi khoá. Mốc này đã tồn tại sẵn (`Đ-26`) và có đúng ý nghĩa cần thiết: **đề đã rời server**, không thể đổi ngược | — |

### Outcomes
- **Chấp nhận**: ghi nhận lựa chọn, chuẩn bị draw 3 câu
- **Từ chối**: hiện UI error, yêu cầu chọn lại (client-side validate)
- **Chưa chọn** (chỉ mode nhập liệu): admin chọn hộ default 20/20/20 (server-side fallback)
- **Mode sân khấu**: không có nhánh "chưa chọn" — admin là người bấm, gói chỉ tồn tại khi admin đã bấm

### State changes
- Bảng chọn gói: thêm record `(seat, packageChoice=[a, b, c])`
- Pool câu: **reserve** N câu mỗi mức (nếu config yêu cầu pre-flight; chưa chốt v1)
- Giao diện: khóa nút chọn sau khi confirm (không đổi được)

### No-change guarantees
- Lựa chọn của TS khác không ảnh hưởng (mỗi TS 3 câu riêng)
- Pool toàn câu không bị cạn chỉ vì 1 TS chọn (pre-flight kiểm worst-case: tất cả chọn cùng mức)
- Thứ tự chọn không ảnh hưởng thứ tự draw (draw ngẫu nhiên, chỉ dùng mức làm filter)

### Error outcomes
- **Pool mức nào đó không đủ**: không bắt đầu được vòng Về đích; kiểm tại cửa vào vòng theo worst-case (mọi thí sinh cùng chọn một mốc)
- **Số mục > 3 hoặc < 3**: UI reject (client + server)
- **Mức không nằm {20, 30}**: UI reject (client + server Zod)

### Evaluation order
1. **Sân khấu**: TS nói gói trên sân khấu ⇒ admin vào màn chốt gói · **Nhập liệu**: TS vào màn chọn gói (hoặc admin chọn hộ nếu TS không chọn kịp)
2. Nhập lựa chọn: click 3 nút / chọn dropdown (UI tuỳ design) — trên máy **admin** hay máy **thí sinh** tuỳ mode
3. Client validate (3 item, mỗi item ∈ {20, 30})
4. Confirm → server lưu lựa chọn
5. Lệnh draw 3 câu (câu 1 từ mức `choice[0]`, câu 2 từ `choice[1]`, câu 3 từ `choice[2]`)

### Boundaries
- **Số câu = 0**: không chọn được (UI require ≥1, nhưng luật bắt buộc 3)
- **Số câu = 1-2**: UI yêu cầu chọn 3
- **Số câu = 3**: hợp lệ
- **Số câu ≥ 4**: UI reject (maximum 3)

### Idempotency
Chọn nhiều lần trước mốc khoá: **last-wins** — bản chọn cuối cùng có hiệu lực, cùng ngữ nghĩa với nút gửi đáp án (nguyên tắc nền điểm 21: *"đáp án là thứ sửa được; chỉ đồng hồ/mốc mới đóng cửa"*). Chọn **đổi** gói: **được, cho tới mốc admin bấm hiển thị câu đầu tiên** (`GRR-054`). Đóng `U-26`.

> Chọn gói **không phải chuông** — nó không đua tốc độ, nên không áp `Đ-29`/điểm 16 (*"bấm xong thì tắt"*).

### Concurrency
Hai TS chọn cùng lúc: không có ảnh hưởng (mỗi TS riêng pool, chỉ pre-flight worst-case check toàn trận).

### Examples
**Hợp lệ:**
- TS chọn 20/20/20 → lưu, chuẩn bị draw 3 câu từ pool 20đ.
- TS chọn 20/30/30 → lưu, chuẩn bị draw 1 câu 20đ + 2 câu 30đ.
- TS quá hạn chưa chọn → admin bấm "chọn 20/20/20 cho TS này" → lưu default.

**Không hợp lệ:**
- TS chọn 20/20 (thiếu 1 câu) → UI: "hãy chọn 3 câu".
- TS chọn 20/40/20 (40 không hợp lệ) → UI: "chỉ chọn mức 20 hoặc 30".

### Source traceability
- **2 mức điểm**: `game-rules-inventory.md` R-VD-01 (20 và 30) · `game-rules-decisions.md` §9.4, K-2
- **3 câu/gói**: `game-rules-inventory.md` R-VD-01 ("gói 3 câu")
- **Admin chọn hộ default**: `game-rules-inventory.md` R-VD-01 + `game-rules-decisions.md` §9.4 · R-VD-01 nói *"admin chọn hộ"*, default 20/20/20 từ `R26` §7
- **Cấu hình**: `SPEC` §7 (packageMode), không phát minh thêm

---

## GR-018 — Về đích: trả lời câu của mình

### Status

CONFIRMED

### Purpose
Quy định khi người thi chính trả lời câu của họ (không phải cướp), áp dụng quy tắc điểm, thời gian và ghi nhận đáp án chuẩn riêng cho vị trí này.

### Actors
- Thí sinh (người thi chính): trả lời câu
- Admin: chấm Đúng/Sai
- Server: ghi nhận bản cuối, tính điểm

### Related states
- Vòng Về đích, lượt của TS (GR-016 recommneded)
- Câu được draw từ gói đã chọn (GR-017)
- Trạng thái câu: **chưa chấm** / **chấm Đúng** / **chấm Sai** (mở cửa sổ cướp nếu sai)

### Trigger
TS chịu trách nhiệm trả lời câu được draw; timer bắt đầu (thời gian suy nghĩ + thực hành).

### Preconditions
- Vòng Về đích, TS là người thi chính lượt này
- Câu được draw (gắn với mức 20 hoặc 30 từ gói TS chọn)
- Timer chưa hết
- Giá trị câu (`value`) xác định (20 hoặc 30)
- Thời gian câu (`timeSeconds`) được lấy từ metadata câu (R-GEN-02)

### Inputs
- `value` của câu: 20 hoặc 30
- `timeSeconds` của câu: ví dụ 15s (20đ) hoặc 20s (30đ) — fallback `defaultTimeByValue` nếu câu không khai
- Submission của TS: text (miệng hoặc gõ tuỳ mode, R-GEN-03)
- Phán quyết admin: Đúng hoặc Sai (không tự chấm ở v1 official)

### Conditions
1. **Tiền điều kiện chấm**: admin phán quyết (không tự chấm, K-16)
2. **Ghi nhận đáp án**: bản **cuối cùng** trước khi admin chấm (khác GR-015 last-wins — Khởi động dùng bản ĐẦU nếu không đổi, R-KD-06)
3. **Thời gian suy nghĩ + thực hành**: 20đ→15s suy + 30s thực, 30đ→20s suy + 60s thực (bảng R-VD-04, **chỉ khi kết quả Đúng mới tính**; nếu Sai → mở cướp)

### Decision table

| Case | Conditions | Expected outcome | State change | Error |
|---|---|---|---|---|
| C1 | Câu 20đ, TS trả lời "Hà Nội" (Đúng) | +20 điểm · không mở cửa sổ cướp | `score[seat] += 20` · event SCORE_ADJUST | — |
| C2 | Câu 30đ, TS trả lời "Paris" (Sai) | 0 điểm · **mở cửa sổ cướp 5 giây** · 3 TS còn lại được bấm | `score[seat] += 0` · queue chợp cửa sổ cướp (per Đ-7.2 = không chặn) | — |
| C3 | Câu 20đ, TS không trả lời (timeout) | **Coi như sai** (R-VD-03) → 0 điểm · mở cửa sổ cướp | `score[seat] += 0` | — |
| C4 | Câu 30đ, TS trả lời "München" (Đúng, nhưng nhập sai) | Admin chấm Đúng (override gợi ý so khớp) → +30 | `score[seat] += 30` | — |
| C5 | Câu 30đ, TS trả lời "Đ" lúc 15s (sớm), admin chấm Đúng lúc 20s | **+30** (khác Tăng tốc last-wins — Về đích không dedup timestamp; time deadline là timer câu, không server-timeout) | `score[seat] += 30` | — |
| C6 | TS + NSHV sai (GR-021) | **−30 TỪ NSHV, không sinh thêm từ Sai** — hình phạt NSHV thay thế transfer (Đ-9.4, `game-rules-review-old.md` GRR-047) | `score[seat] −= 30` (1 lần, không dồn) | — |

### Outcomes
- **Đúng**: + giá trị câu (20 hoặc 30)
- **Sai hoặc timeout**: 0 điểm (không trừ, khác Khởi động) + **mở quyền cướp**
- **Nếu NSHV sai**: −value (một nửa nó được cộng lại nếu có người cướp Đúng — xem Đ-9.4 `game-rules-review-old.md` GRR-047)

### State changes
- Score: `score[seat] += {+value, 0, -value}` tuỳ kết quả
- Câu: chuyển sang trạng thái "đã chấm"
- Nếu sai: mở cửa sổ cướp (5 giây, per Đ-7.2 không chặn queue)
- EventLog: thêm sự kiện SCORE_ADJUST (hoặc tương tự) ghi nhận delta điểm

### No-change guarantees
- Submission của 3 TS còn lại không ảnh hưởng (họ chưa trả lời câu này)
- Câu trước không bị ảnh hưởng
- NSHV của TS khác không ảnh hưởng (NSHV độc lập per-TS)

### Error outcomes
- **Admin chấm người không phải TS thi chính**: server reject (permission)
- **Chấm lại lần 2**: idempotency per Đ-5.3 (bấm Đúng 2 lần = 1 event; bấm Sai sau Đúng = revert + event mới)
- **Chấm sau hết cửa sổ cướp**: vẫn chấm được (cửa sổ chỉ cho bấm chuông, không chặn chấm)

### Evaluation order
1. TS chọn gói (GR-017) → draw 3 câu
2. Câu 1: TS được nhìn thấy, có timer hiển thị
3. TS gửi submission (theo mode)
4. Hết timer hoặc TS gửi trong timer
5. Admin bấm Đúng/Sai (hoặc hệ thống chấm nếu practice, K-16)
6. Nếu Đúng: cộng value, kết thúc câu
7. Nếu Sai: cộng 0, mở cửa sổ cướp 5s
8. Sau khi cửa sổ cướp đóng hoặc ai cướp được: câu kết thúc

### Boundaries
- **Value = 0**: không hợp lệ (config chỉ 20/30)
- **Value ∈ {20, 30}**: hợp lệ (chỉ O26)
- **Value ∉ {20, 30}** (ví dụ 50): ngoài preset `O26_DEFAULT@1`; admin cấu hình được nhưng không thuộc luật 2026 (K-2)
- **timeSeconds = 0**: không hợp lệ (UI chặn, pre-flight chặn)

### Idempotency
Chấm Đúng 2 lần → 1 event (dedup). Chấm Sai → Đúng → revert + event mới.

### Concurrency
Không applicable (1 TS thi 1 lượt; vòng Khởi động chung mới concurrent).

### Examples
**Hợp lệ:**
- Câu 20đ (15s timer), TS trả lời "Hà Nội" @ 10s, admin chấm Đúng @ 12s → +20 điểm.
- Câu 30đ (20s timer), TS hết timer (chưa gửi), admin chấm Sai → 0 điểm + mở cửa sổ cướp 5s.
- Câu 30đ, TS + NSHV sai (−30 từ NSHV, không sinh sự kiện Sai riêng) → −30 tổng cộng.

**Không hợp lệ:**
- Câu 20đ, TS trả lời, admin chấm 2 lần Đúng → ghi nhận 1 lần (idempotency).

### Source traceability
- **Giá trị câu 20/30**: `game-rules-inventory.md` R-VD-01, R-VD-03 · `game-rules-decisions.md` §9.4, K-2
- **Thời gian suy nghĩ + thực hành**: `game-rules-inventory.md` R-VD-04 (bảng thời gian)
- **Bản ghi nhận**: `game-rules-inventory.md` R-VD-03 (bản cuối) — khác Khởi động
- **Mở cửa sổ cướp nếu sai**: `game-rules-inventory.md` R-VD-03 ("mở quyền cướp")
- **NSHV sai lấn**: `game-rules-decisions.md` §9.4 `game-rules-review-old.md` GRR-047

---

## GR-019 — Về đích: câu hỏi thực hành

### Status

CONFIRMED

### Purpose
Định nghĩa khi câu được đánh dấu là "thực hành", thí sinh được cung cấp dụng cụ thực hành, và chấm điểm là "thực hành đạt yêu cầu" (không phải so khớp text).

### Actors
- Thí sinh (người thi chính): thực hành với dụng cụ
- MC / Ban cố vấn / Khách mời: giới thiệu dụng cụ
- Admin: bấm mốc chuyển pha, chấm "đạt yêu cầu / không đạt"
- Setter: **khai câu là thực hành qua các trường ở mục dưới**

### Mô hình dữ liệu — 5 trường bổ sung *(✅ chủ dự án chốt 2026-07-26, đóng `U-6`)*

> Trước 26/07 không có cách nào khai một câu là "thực hành" ⇒ toàn bộ GR-019 không có đường vào. Năm trường dưới đây là **tập tối thiểu** để rule chạy được; mỗi trường đều truy nguyên về một câu trong nguồn gốc.

| Trường | Kiểu | Mặc định | Nguồn / lý do |
|---|---|---|---|
| **`isPractical`** | boolean | `false` | Cờ khai câu thực hành. **Chỉ hợp lệ với câu pool Về đích** — luật gốc chỉ có câu thực hành ở vòng này |
| **`practiceSeconds`** | số giây | **30** (câu 20đ) · **60** (câu 30đ) | *"Đối với câu hỏi 20 điểm… thời gian thực hành là 30 giây. Đối với câu hỏi 30 điểm… thời gian thực hành là 60 giây."* Là **metadata TỪNG CÂU** như `timeSeconds` (G-4), preset chỉ đặt default |
| **`stealPracticeSeconds`** | số giây | **20** (câu 20đ) · **40** (câu 30đ) | *"Đối với câu hỏi 20 điểm, thời gian thực hành là 20 giây. Đối với câu hỏi 30 điểm, thời gian thực hành là 40 giây."* — câu này của nguồn nói về **người cướp quyền**. Đây là trường đóng `C4` |
| **`equipmentNote`** | text | rỗng | *"chương trình sẽ giới thiệu các **dụng cụ liên quan** đến câu hỏi thực hành"* — BTC phải mang dụng cụ tới trường quay, nên phải khai được từ lúc soạn đề |
| **`acceptanceCriteria`** | text | rỗng | Tiêu chí để admin phán quyết *"đạt yêu cầu"*. Đây là **thứ thay `acceptedAnswers`** ở câu thực hành: không có đáp án chữ để so khớp |

**Ràng buộc và hệ quả:**

- **Bảo mật**: `acceptanceCriteria` cùng mức với `acceptedAnswers` — **chỉ admin + MC** (GR-037). `equipmentNote` cũng **không** ra viewer/thí sinh trong trận (nó tiết lộ bản chất câu hỏi), nhưng **phải** xem được trong kho đề để BTC chuẩn bị.
- **Không có gì để highlight**: GR-027 (chuẩn hoá + tô màu khác biệt) **không áp** cho câu thực hành — không tồn tại đáp án text.
- **Kênh trả lời thứ BA**: thực hành không phải *nói* cũng không phải *gõ*. Theo nguyên tắc nền điểm 22, nó đi **nhánh trên** (như *nói*): **không có ô nhập nào để chờ**, nên nút chấm của admin **sống suốt**, không khoá tới hết giờ.
- **Hai mốc bấm, không phải một** (nguyên tắc nền điểm 17 + NT-B): `hiển thị câu` → `start timer suy nghĩ` → **`bắt đầu thực hành`** → `chấm`. Mốc thứ ba là **nút mới của admin**, và chính nó đóng câu hỏi *"ai bấm, lúc nào"* của `GRR-050/051`.
- **Dụng cụ là vật lý, máy không kiểm được**: cửa vào vòng (`Đ-31`) chỉ **hiển thị `equipmentNote` như checklist** và **cảnh báo**, **không chặn cứng** (NT-D). Không có chỗ chặn thứ tư.
- **`Đ-19` áp nguyên**: *"thực hành không đạt"* và *"không thực hành gì"* là **cùng một thao tác** (Sai) — không cần nút thứ ba.

### Related states
- Vòng Về đích
- Câu được draw từ gói đã chọn
- **Thuộc tính câu**: `isPractical = true`

### Trigger
Câu được draw và có `isPractical = true`.

### Preconditions
- Câu có `isPractical = true`
- TS là người thi chính lượt này
- Dụng cụ thực hành đã được BTC chuẩn bị theo `equipmentNote` (**ngoài hệ thống** — máy chỉ nhắc, không kiểm được)
- MC / Ban cố vấn có mặt để giới thiệu dụng cụ

### Inputs
- Câu 20đ vs 30đ (xác định thời gian)
- Thời gian suy nghĩ + thực hành: bảng R-VD-04
  - 20đ: 15s suy + **30s thực hành** (cướp: 20s)
  - 30đ: 20s suy + **60s thực hành** (cướp: 40s)
- `acceptanceCriteria` — tiêu chí để admin đánh giá (máy **không** phán quyết, nguyên tắc nền điểm 1)
- `equipmentNote` — dụng cụ cần chuẩn bị

### Conditions
1. Khi câu là thực hành → timer chia **2 pha**: suy nghĩ (`timeSeconds`) rồi thực hành (`practiceSeconds`), ranh giới là **một nút bấm của admin**
2. **Admin chấm**: "đạt yêu cầu" / "không đạt" — hai lựa chọn, theo `Đ-19` *"không thực hành gì"* cũng là "không đạt"
3. Nếu **đạt**: +value (20 hoặc 30)
4. Nếu **không đạt**: 0 điểm → mở **cửa sổ bấm chuông 5 giây** để cướp quyền
5. Người cướp được quyền thì thực hành trong `stealPracticeSeconds` (**20s** cho câu 20đ · **40s** cho câu 30đ)

### Decision table

| Case | Conditions | Expected outcome | State change | Error |
|---|---|---|---|---|
| C1 | Câu 20đ thực hành: TS suy 15s, thực hành 30s, admin chấm "đạt" | **+20 điểm** (không mở cướp) | EventLog: SCORE_ADJUST +20 | — |
| C2 | Câu 20đ thực hành: TS suy 15s, hết 30s thực hành mà chưa xong, admin chấm "không đạt" | **0 điểm** + **mở cửa sổ bấm chuông 5 giây** | EventLog: 0 · queue cửa sổ cướp (Đ-7.2) | — |
| C3 | Câu 30đ thực hành: TS suy 20s, thực hành 60s, admin chấm "đạt" | **+30 điểm** | EventLog: SCORE_ADJUST +30 | — |
| C4 — **đóng `U-6`** | Câu 30đ thực hành, người thi chính không đạt, có người bấm chuông trong 5s và cướp được | Người cướp **cũng thực hành**, trong **`stealPracticeSeconds` = 40 giây** (câu 20đ thì 20 giây). Đạt ⇒ **lấy điểm từ** người thi chính (`transfer`, K-12); không đạt ⇒ **−½ giá trị câu** | EventLog: transfer hoặc −½ value | — |

> **Phân biệt hai con số hay bị lẫn ở C2/C4** — nguồn có **hai** mốc thời gian khác nhau cho pha cướp, và bản trước của rule này gộp sai thành một:
>
> | Mốc | Giá trị | Nguồn |
> |---|---|---|
> | **Cửa sổ bấm chuông** để giành quyền | **5 giây**, không đổi theo mức điểm | *"giành quyền trả lời bằng cách bấm chuông nhanh trong **5 giây**"* |
> | **Thời gian thực hành của người cướp** | **20s** (câu 20đ) · **40s** (câu 30đ) | *"Đối với câu hỏi 20 điểm, thời gian thực hành là 20 giây. Đối với câu hỏi 30 điểm, thời gian thực hành là 40 giây."* |
>
> Con số 20/40 **chỉ bắt đầu đếm SAU KHI** đã có người cướp được quyền — nó không phải độ dài cửa sổ chuông.
| C5 | Câu 20đ thực hành, TS + NSHV đạt | **+20 × 2 = +40 điểm** (nhân đôi) | EventLog: SCORE_ADJUST +40 | — |
| C6 | Câu 20đ thực hành, TS + NSHV không đạt | **−20** (hình phạt NSHV, coi như sai) + mở cửa sổ cướp | EventLog: SCORE_ADJUST −20 | — |

### Outcomes
- **Đạt yêu cầu**: + value (20 hoặc 30)
- **Không đạt**: 0 điểm (hoặc −value nếu NSHV) → mở cựơ sổ cướp với thời gian tương ứng
- Không có **tính điểm tự động** (admin đánh giá, không so khớp)

### State changes
- EventLog: ghi SCORE_ADJUST (delta +20/30/−20/−30)
- Câu: chuyển trạng thái "đã chấm"
- Timer: hết thời gian thực hành → mở cửa sổ cướp (nếu chưa chấm)

### No-change guarantees
- Dụng cụ không bị ảnh hưởng (vật lý, ngoài hệ thống)
- Câu khác không ảnh hưởng
- Phán quyết của admin là cuối cùng (không tự chấm)

### Error outcomes
- **`isPractical = true` trên câu KHÔNG thuộc pool Về đích**: cấu hình không hợp lệ ⇒ chặn ở **contest builder / kho đề**, không phải lỗi lúc chạy
- **`isPractical = true` nhưng `equipmentNote` rỗng**: **cảnh báo** ở cửa vào vòng (BTC có thể không biết cần mang gì) — không chặn cứng (NT-D)
- **`acceptanceCriteria` rỗng**: admin vẫn phán quyết được bằng đánh giá của mình (mô hình advisory, nguyên tắc nền điểm 1) ⇒ **cảnh báo**, không chặn

### Evaluation order
1. Draw câu (GR-017)
2. Đọc `isPractical` của câu
3. Admin bấm **hiển thị câu** ⇒ đề lên màn (mốc `Đ-26`, cũng là mốc `usedInContest`)
4. Admin bấm **start timer** ⇒ đếm pha suy nghĩ `timeSeconds`
5. Admin bấm **bắt đầu thực hành** ⇒ đếm pha thực hành `practiceSeconds` *(mốc mới, đóng `GRR-050/051`)*
6. TS thực hành trong thời gian quy định
5. Admin chấm "đạt" (Yes) hay "không đạt" (No)
6. Tính điểm tuỳ kết quả

### Boundaries
- **Câu 20đ thực hành**: 15s suy + 30s thực hành (cứng per bảng R-VD-04)
- **Câu 30đ thực hành**: 20s suy + 60s thực hành (cứng)
- **Câu không thực hành**: không áp GR-019 (xem GR-018 thay)
- **Thời gian cướp**: 20s (câu 20đ) / 40s (câu 30đ) — không tuỳ chỉnh

### Idempotency
Chấm "đạt" 2 lần → 1 event. Chấm "không đạt" → "đạt" → revert + event mới.

### Concurrency
Không applicable (1 TS thi).

### Examples
**Hợp lệ:**
- Câu 20đ thực hành (rubik cube): TS suy 15s, sau đó xoay rubik 30s, hoàn thành trước hết hạn. Admin chấm "đạt" → +20 điểm.
- Câu 30đ thực hành, TS + NSHV "đạt" → +30 × 2 = +60 điểm.

**Không hợp lệ:**
- Đặt `isPractical = true` cho câu Khởi động / VCNV / Tăng tốc ⇒ ngoài luật, chặn ở kho đề.
- Cho máy tự kết luận *"đạt yêu cầu"* từ `acceptanceCriteria` ⇒ trái nguyên tắc nền điểm 1; đó là văn bản để **người** đối chiếu.
- Gộp *"start timer"* và *"bắt đầu thực hành"* thành một nút ⇒ mất mốc đóng pha suy nghĩ.

### Source traceability
- **Khai câu thực hành (5 trường)**: ✅ chủ dự án chốt **2026-07-26** — đóng `U-6` và `GRR-050/051`. Xem mục **Mô hình dữ liệu** đầu rule
- **Thời gian suy nghĩ + thực hành + thời gian của người cướp**: `docs/source/fandom-olympia-26-luat-choi.md` §Về đích đoạn *"Trong câu hỏi thực hành…"* (nguyên văn cả ba cặp số) · `game-rules-inventory.md` R-VD-04
- **Cửa sổ bấm chuông 5 giây** (khác thời gian thực hành của người cướp): cùng đoạn nguồn
- **Chấm "đạt yêu cầu"**: `game-rules-inventory.md` R-VD-04 · nguyên tắc nền điểm 1 (`Đ-1` — người phán quyết)
- **Mốc chuyển pha là nút của admin**: `Đ-26`, `Đ-33` (mẫu chung), đóng `GRR-050/051`

---

## GR-020 — Về đích: cướp quyền

### Status

CONFIRMED

### Purpose
Khi người thi chính trả lời sai (hoặc hết giờ) câu Về đích, 3 TS còn lại được bấm chuông để giành quyền trả lời trong cửa sổ 5 giây. Người cướp đúng lấy điểm từ người sai (transfer); cướp sai bị trừ nửa giá trị.

### Actors
- 3 TS còn lại: bấm chuông trong 5s (RO không áp — không nhịp chơi này)
- Server: ghi nhận timestamp, xếp hạng (first-come / first-buzz)
- Admin: chấm Đúng/Sai của người cướp. **Không có nhánh tự chấm nào** — trong trận chính thức máy không bao giờ tự công nhận hay bác bỏ đáp án (nguyên tắc nền điểm 1, `Đ-1`)

### Related states
- Vòng Về đích, người thi chính vừa **chấm Sai** (hoặc timeout)
- Cửa sổ cướp: 5 giây, **không chặn queue** (Đ-7.2 — không chặn, server phân xử ngay)
- NSHV của 3 TS còn lại: sẵn sàng (nếu chưa dùng lần này)

### Trigger
Admin bấm "Sai" cho người thi chính (hoặc timeout tự động) → **mở cửa sổ cướp 5 giây**.

### Preconditions
- Vòng Về đích, câu được draw
- Người thi chính **chấm Sai hoặc timeout**
- Câu chưa hết giờ (timer đã bắt đầu cửa sổ cướp)
- 3 TS còn lại chuẩn bị bấm chuông (không khoá input)

### Inputs
- Người thi chính: TS[i] (có điểm −value tại đây)
- 3 TS còn lại: TS[j], TS[k], TS[l] (chưa trả lời câu này)
- Giá trị câu: value ∈ {20, 30}
- Thành phố: "Hà Nội" → transfer = −20 từ TS[i] tới người cướp đúng
- Cửa sổ chuông: **5 giây** (cứng per O26, R-VD-05)

### Conditions
1. Khi có **1+ TS bấm** trong 5s → **ai bấm trước (fastest)** được quyền trả lời
2. Nếu **nhiều thí sinh cùng timestamp**: **hàng đợi tự quyết định, ngẫu nhiên** (`Đ-23`) — cướp quyền là vòng hàng đợi **không chặn**, và quyền trả lời không chia được nên không dùng quy tắc chia điểm của Tăng tốc.
3. Người cướp **ghi nhận bản ĐẦU TIÊN** (khác TS thi chính dùng bản cuối, R-VD-05)
4. **Transfer khi cướp Đúng**: −value từ TS[i], +value tới TS[cướp]
5. **Trừ nửa khi cướp Sai**: −(value/2) từ TS[cướp]

### Decision table

| Case | Conditions | Expected outcome | State change | Error |
|---|---|---|---|---|
| C1 | Câu 20đ, TS[i] sai "London", 3 người bấm @ 100/150/200ms → TS[j] @ 100ms đúng "Paris" | TS[i] −20, TS[j] +20 (transfer) | EventLog: SCORE_ADJUST TS[i]−20, TS[j]+20 | — |
| C2 | Câu 30đ, TS[i] sai, TS[j] @ 100ms cướp **sai** "Moscow" | TS[j] −15 (nửa 30); admin có thể chọn **Huỷ kết quả** thay vì Sai để không áp hình phạt | EventLog: SCORE_ADJUST TS[j]−15 · TS[i] vẫn −30 (không hoàn lại) | — |
| C3 | Câu 20đ, TS[i] sai, **không ai bấm trong 5s** | TS[i] −20 (vẫn trừ), không ai +20 | EventLog: SCORE_ADJUST TS[i]−20 | — |
| C4 | Câu 20đ, TS[i] sai, TS[j] gửi 3 bản: "Paris" @ 100ms, "London" @ 150ms, "Berlin" @ 200ms | TS[j] chấm dựa trên **bản ĐẦU "Paris"** (khác GR-015) | Admin so khớp "Paris" để chấm | — |
| C5 — **đóng `U-8`** | Câu 20đ, người thi chính A **có NSHV** và bị chấm Sai; B cướp đúng | **A −20 · B +20.** A mất giá trị câu **ĐÚNG MỘT LẦN**: hình phạt NSHV **thay thế** phần nợ của transfer, không cộng dồn | EventLog: A −20 (nhãn NSHV), B +20 | — |
| C6 | Số người còn lại ≠ 3 (ghế ≠ 4) | `[v1.5]` — v1 đặc tả cho đúng 4 thí sinh nên *"3 thí sinh còn lại"* luôn đúng nghĩa; dải 1-12 thuộc v1.5 (U-5). Riêng trường hợp **runtime tụt dưới 4 giữa trận** đã chốt: **admin quyết**, hệ thống chỉ cảnh báo (§6.4) | — | — |

### Outcomes
- **Cướp Đúng**: người cướp +value (lấy từ TS[i]), TS[i] −value (transfer)
- **Cướp Sai**: người cướp −(value/2), TS[i] vẫn −value (không hoàn lại)
- **Không ai cướp**: TS[i] −value, không ai nhận được
- **Nếu người thi chính có NSHV và sai** (`U-8` — ĐÓNG): người đó mất **−value một lần**, **bất kể** có ai cướp hay không; người cướp đúng vẫn **+value**. Nguồn quy định thẳng ở dòng 90: *"Trả lời sai sẽ bị trừ đi số điểm của câu hỏi, **kể cả các thí sinh còn lại có giành quyền trả lời hay không**"* ⇒ số học của người thi chính **độc lập** với hành động của người cướp. Xem C5 và GR-021

### State changes
- EventLog: SCORE_ADJUST cho TS[i] (−value) và TS[cướp] (±delta)
- Câu: chuyển sang trạng thái "kết thúc" (đã chấm cấp Đúng/Sai)
- Cửa sổ cướp: đóng lại

### No-change guarantees
- TS[j], TS[k], TS[l] không bấm → điểm không thay đổi (chỉ TS[i] −value, chưa ai +)
- Câu trước không ảnh hưởng
- Submission của TS[i] không thay đổi (dùng để so khớp gợi ý, không chấm lại)

### Error outcomes
- **Admin không chấm "Sai" cho TS[i]**: cửa sổ cướp không mở (logic tùy implement)
- **Bấm ngoài cửa sổ 5s**: không tính (server reject ngoài deadline)
- **Giá trị câu LẺ, hình phạt `−½`** (`U-20` — ✅ chủ dự án chốt **2026-07-26**, phương án **(b)**): **làm tròn xuống theo ĐỘ LỚN của hình phạt**, tức `phạt = value / 2` bằng **phép chia số nguyên** (cắt về 0). Ví dụ `value = 25` ⇒ phạt **12** ⇒ điểm đổi **−12**.

> ⚠️ **Cẩn thận với DẤU** — đây là chỗ dễ cài sai: `−½ × 25 = −12,5`. Nếu hiểu *"làm tròn xuống"* theo nghĩa toán học thì `floor(−12,5) = −13`, tức **nặng hơn**, **ngược** quyết định. Phải làm tròn trên **độ lớn** rồi mới gắn dấu âm: `−(25 / 2) = −12`.
>
> Ba lý do chọn (b): **(1)** đúng bằng hành vi mặc định của phép chia số nguyên ⇒ không tốn dòng code nào, cùng tinh thần với `K-8`; **(2)** chỗ nguồn im lặng thì chọn hướng **nhẹ hơn** cho thí sinh — hình phạt nặng quá mức sinh khiếu nại công bằng, nhẹ quá mức thì không; **(3)** **không** hard-code ràng buộc "giá trị phải chẵn" vào validation, giữ được lời hứa luật tuỳ biến.
>
> **Dưới preset `O26_DEFAULT@1` tình huống này KHÔNG TỒN TẠI**: giá trị chỉ 20 và 30 ⇒ nửa là 10 và 15, đều nguyên. Quy ước này chỉ chạm tới contest cấu hình ngoài luật 2026.
- **Điểm người cướp xuống âm**: hợp lệ — **điểm được phép âm, KHÔNG có sàn** (`Đ-2`).

### Evaluation order
1. TS[i] thi chính → admin chấm Sai (hoặc timeout tự động)
2. Mở cửa sổ cướp: 5 giây
3. TS[j], TS[k], TS[l] bấm chuông (mỗi lần bấm = tín hiệu vào queue, Đ-7.2 không chặn)
4. Server xếp theo server timestamp, người sớm nhất giành quyền; **cùng mốc thì hàng đợi tự quyết định, ngẫu nhiên** (`Đ-23`)
5. Admin chấm người cướp (Đúng/Sai)
6. Tính điểm transfer / trừ nửa

### Boundaries
- **Cửa sổ = 5 giây**: cứng per O26 (R-VD-05)
- **Người cướp ∈ {3 TS khác, không team cùng}**: cứng (R-TEAM-05 chỉ thi đội)
- **Giá trị ∈ {20, 30}**: chỉ Về đích preset O26
- **Nửa giá trị = 10 (20/2) / 15 (30/2)**: đều là số nguyên. Giá trị lẻ **không phát sinh dưới luật 2026** (`Đ-3`); nó chỉ xuất hiện nếu admin cấu hình mức điểm lẻ, và trường hợp đó **đã chốt** — `U-20` đóng **2026-07-26**: `phạt = value / 2` bằng **phép chia số nguyên**, làm tròn xuống theo **độ lớn** hình phạt (xem Error outcomes). Ví dụ `value = 25` ⇒ phạt **12**.

### Idempotency
Chấm người cướp Đúng 2 lần → 1 event transfer. Chấm Sai → Đúng → revert + event mới.

### Concurrency
Không tồn tại: **mỗi contest chỉ có MỘT admin duy nhất** (`Đ-18`).

### Examples
**Hợp lệ:**
- Câu 20đ, TS[A] sai "London", TS[B] bấm @ 100ms đúng "Paris" → A −20, B +20.
- Câu 20đ, TS[A] sai, TS[B] @ 100ms sai "Moscow" → B −10 (nửa 20).
- Câu 20đ, TS[A] sai, không ai bấm trong 5s → A −20, không ai +.

**Không hợp lệ:**
- 6 TS chơi → `[v1.5]`, ngoài phạm vi luật v1.

### Source traceability
- **Cửa sổ 5 giây**: `game-rules-inventory.md` R-VD-05 · `game-rules-decisions.md` §9.4, K-12
- **Transfer điểm**: `game-rules-inventory.md` R-VD-05 (K-12 chốt "giành được điểm TỪ thí sinh trả lời sai")
- **Trừ nửa sai**: `game-rules-inventory.md` R-VD-05 ("bị trừ một nửa số điểm")
- **Bản ghi nhận người cướp**: `game-rules-inventory.md` R-VD-05 (bản ĐẦU TIÊN)
- **Không chặn queue**: `game-rules-decisions.md` §5.2 Đ-7.2 (Về đích cướp = không chặn)

---

## GR-021 — Về đích: Ngôi sao hy vọng

### Status

CONFIRMED

### Purpose
Quyền đặt cược lên câu hỏi Về đích của TS: đúng → ×2 điểm câu, sai → −value, bất kể có người cướp hay không. Mỗi TS **1 lần / lần chạy vòng Về đích** (`Đ-42`) — trong luồng thường là **1 lần/trận** như luật gốc; chạy lại vòng thì đặt lại.

### Actors

**Chủ thể ĐẶT phụ thuộc MODE TRẢ LỜI** (`Đ-41`, chốt 27/07 — cùng khuôn `Đ-36` / `Đ-40`):

| Mode | Ai bấm | Cơ chế |
|---|---|---|
| **Sân khấu** (mặc định) | **Admin** | Người thi chính **nói miệng trên sân khấu** *"đặt Ngôi sao hy vọng"* **trước khi câu được mở**; admin nghe và bấm, qua **dialog Yes/No** (thao tác không hoàn tác được). Máy TS **không render** nút NSHV |
| **Nhập liệu** | **Thí sinh** (người thi chính) | TS tự bấm nút NSHV trước mốc đóng cửa sổ |

- Server: xác nhận NSHV còn / đã tiêu, tính số học, validate lại chủ thể theo mode (Zero-trust)

### Related states
- Vòng Về đích, trước khi câu được đọc hoặc hiện màn hình (mốc admin bấm "hiển thị câu", `game-rules-review-old.md` GRR-048)
- Trạng thái NSHV: **chưa dùng** / **đã dùng**
- Hàng đợi tín hiệu: NSHV vào queue, admin duyệt (Đ-7.2 VCNV chặn, nhưng NSHV không trong VCNV)

### Trigger
Nút NSHV được bấm trước mốc đóng cửa sổ — **sân khấu**: admin bấm sau khi nghe TS nói · **nhập liệu**: TS tự bấm (`Đ-41`).

### Preconditions
- Vòng Về đích, lượt của TS
- TS **chưa dùng NSHV** lần nào trong trận
- Nút NSHV chưa bị khoá (TS chưa bấm)
- **Mốc đóng cửa sổ NSHV**: **khi admin bấm "hiển thị câu hỏi"** (R-VD-06 + `game-rules-review-old.md` GRR-048)

### Inputs
- Giá trị câu: value ∈ {20, 30}
- Trạng thái NSHV của TS: **chưa tiêu** (true)
- Quyết định admin: **Yes** (chấp nhận NSHV) hay **No** (từ chối)

### Conditions
1. **Chỉ 1 lần / TS / LẦN CHẠY VÒNG** (`Đ-42`, 27/07): sau khi bấm lần 1 → nút NSHV disabled. **Chạy lại vòng Về đích (`EVENT-004`) hoặc bỏ vòng (`EVENT-003`) ⇒ cờ ĐẶT LẠI**, ngôi sao dùng được lại. Luồng thường vẫn đúng *"1 lần / TS / trận"* của luật gốc, vì Về đích chạy một lần trong một trận và NSHV chỉ tồn tại ở vòng này. Đề xuất `Đ-5.d` (phạm vi contest, không hồi sinh) **bị bác**
2. **Mốc đóng**: admin bấm "hiển thị câu" (`game-rules-review-old.md` GRR-048 — mốc "đọc lên hoặc hiện lên")
3. **Bấm trước mốc**: là hợp lệ; bấm sau → không hợp lệ (queue reject ngoài cửa sổ)
4. **Queue**: Về đích là vòng hàng đợi **KHÔNG chặn** (`Đ-7.2`) ⇒ tín hiệu đặt Ngôi sao hy vọng **có hiệu lực ngay**, không chờ admin xác nhận. **Chỉ áp cho mode nhập liệu** — ở mode sân khấu admin là người bấm nên không có tín hiệu để duyệt (`Đ-41`). **`Đ-54` (27/07) chốt dứt điểm vế này**: mode nhập liệu **cũng không cần admin duyệt**, tín hiệu có hiệu lực ngay khi tới.
5. **Mode sân khấu**: máy TS **không có** nút NSHV ⇒ không tồn tại đường phát tín hiệu từ phía TS; server **từ chối** nếu vẫn nhận được (Zero-trust).

### Decision table

| Case | Conditions | Expected outcome | State change | Error |
|---|---|---|---|---|
| C1 | Câu 20đ, TS bấm NSHV @ 100ms (trước admin bấm hiển thị), admin chấm Đúng | **+20 × 2 = +40** | `score[seat] += 40` · `nshvUsed[seat] = true` | — |
| C2 | Câu 30đ, TS bấm NSHV @ 100ms, admin chấm Sai | **−30** (hình phạt đầy đủ, không cộng dồn lên cướp transfer) | `score[seat] −= 30` · `nshvUsed[seat] = true` | — |
| C3 | Câu 20đ, TS bấm NSHV lần 1 @ 100ms (admin chấp nhận Yes), rồi bấm lần 2 @ 200ms | Lần 2 **không được chấp nhận** — nút disabled | Lần 2 bị **reject** (hoặc UI không cho bấm) | — |
| C4 | Câu 30đ, TS bấm NSHV @ 100ms (trước mốc đóng cửa sổ), admin chấm "Đúng" + có người cướp đúng | **TS +60 (30×2)** · **người cướp +30 (transfer từ sai TS[i], không phải từ TS ấy)** | `score[seat_nshv] += 60` · `score[seat_steal] += 30` | — |
| C5 — **đóng `U-8`** | Câu 30đ, người thi chính A **có NSHV** và bị chấm **Sai**; B cướp đúng | **A −30 · B +30.** A mất giá trị câu **đúng một lần** — hình phạt NSHV **thay thế** phần nợ của transfer, **không cộng dồn** thành −60 | `score[A] −= 30` · `score[B] += 30` · `nshvUsed[A] = true` | — |
| C5b | Cùng C5 nhưng **B cướp sai** | **A −30** (không đổi) · **B −15** (`−½` giá trị câu, hình phạt riêng của B) | `score[A] −= 30` · `score[B] −= 15` | — |
| C5c | Cùng C5 nhưng **không ai cướp** | **A −30** (không đổi) | `score[A] −= 30` | — |

> **`U-8` được nguồn quy định thẳng, không phải suy diễn.** Dòng 90 của `docs/source/fandom-olympia-26-luat-choi.md`: *"Trả lời sai sẽ bị trừ đi số điểm của câu hỏi, **kể cả các thí sinh còn lại có giành quyền trả lời hay không**."* Mệnh đề in đậm tồn tại **đúng để** nói rằng số học của người thi chính **độc lập** với hành động của người cướp ⇒ C5, C5b, C5c cho A **cùng một** kết quả `−value`.
>
> Nó cũng giải thích **vì sao mệnh đề đó cần có mặt**: ở câu **không** NSHV, A sai mà không ai cướp thì A mất **0**; có NSHV thì A mất `value` **dù không ai cướp**. Đó chính là điều nguồn muốn nhấn.
>
> **Hai lỗi tài liệu từng làm `U-8` trông khó hơn thực tế** — nay đã sửa:
> 1. Bảng cũ viết như thể *"TS NSHV"* và *"người thi chính TS[i]"* là **hai người khác nhau**. Không phải: Ngôi sao hy vọng do **chính người đang thi** đặt lên câu **của mình**, trước khi câu được đọc (Conditions điểm 2). Cùng một người — bảng dựng sai tiền đề nên không ra được đáp số.
> 2. `GR-018` **đã ghi đúng đáp án từ trước** (*"−30 từ NSHV, không sinh sự kiện Sai riêng"*), chỉ GR-020/GR-021 chưa đồng bộ.
>
> **Người cướp KHÔNG thể dùng Ngôi sao hy vọng trên câu đang cướp**: NSHV phải đặt **trước khi câu được đọc**, còn người cướp chỉ quyết định bấm chuông **sau khi** câu đã đọc và đã bị trả lời sai. Ngôi sao của họ vẫn còn nguyên cho **lượt thi của chính họ**.
| C7 — **mode sân khấu** (`Đ-41`) | TS nói *"đặt Ngôi sao hy vọng"* trên sân khấu trước khi câu được mở; admin bấm | **Hợp lệ** — admin là chủ thể bấm ở mode này; qua **dialog Yes/No** vì thao tác không hoàn tác được. Cùng một người bấm NSHV và bấm hiển thị câu ⇒ **không có cuộc đua ở biên cửa sổ** | `nshvUsed[seat] = true` | — |
| C8 — **mode sân khấu**, máy TS gửi tín hiệu NSHV | Nút không render nhưng vẫn có request tới server | **Không tồn tại đường này** — server từ chối (Zero-trust) | Không đổi | Drop + AuditLog |
| C6 — bấm sau khi cửa sổ đã đóng | Admin đã bấm hiển thị câu lúc 50ms, thí sinh bấm NSHV lúc 100ms | **KHÔNG TỒN TẠI** — cửa sổ đặt Ngôi sao hy vọng đóng tại mốc admin bấm hiển thị (`game-rules-review-old.md` GRR-048); ngoài cửa sổ thì nút **không hiển thị và không phản hồi** (`Đ-16`) ⇒ không có tín hiệu nào được tạo | Không đổi; ngôi sao **vẫn chưa dùng** | — |

### Outcomes
- **NSHV Đúng**: ×2 giá trị câu (20 → +40, 30 → +60)
- **NSHV Sai**: −value (20 → −20, 30 → −30), **bất kể có người cướp hay không** (R-VD-06)
- **NSHV lần 2+**: **không được chấp nhận** (NSHV đã tiêu)

### State changes
- Flag: `nshvUsed[seat] = true` (khi admin chấp nhận, sau bấm Yes)
- Score: +40/60 (đúng) hoặc −20/−30 (sai)
- Nút NSHV: disable sau khi dùng lần 1
- EventLog: SCORE_ADJUST ghi nhận delta điểm
- Tín hiệu vào hàng đợi **không chặn** của vòng Về đích (`Đ-7.2`) — có hiệu lực ngay khi tới, trong cửa sổ hợp lệ.

### No-change guarantees
- NSHV của TS khác không ảnh hưởng (độc lập per-TS)
- Câu trước không ảnh hưởng (NSHV gắn câu hiện tại)
- Điểm từ câu thi chính (nếu không NSHV) không ảnh hưởng (cộng độc lập)

### Error outcomes
- **Bấm lần 2**: không được chấp nhận (nút disabled hoặc server reject)
- **Bấm ngoài cửa sổ**: không tồn tại — nút không hiển thị và không phản hồi sau khi cửa sổ đóng (`Đ-16`); ngôi sao vẫn chưa dùng.
- **NSHV × cướp transfer Đúng**: **đã chốt** — `U-8` đóng **2026-07-26** bằng **đọc nguồn** (dòng 90 luật gốc), xem C5 / C5b / C5c. Người thi chính mất giá trị câu **ĐÚNG MỘT LẦN**: hình phạt NSHV **thay thế** phần nợ của transfer, **không cộng dồn**. Câu 30đ ⇒ **A −30 · B +30**, không phải −60

### Evaluation order
1. TS nhìn thấy câu sắp được hỏi (chuẩn bị trước)
2. TS bấm NSHV nút
3. Tín hiệu vào hàng đợi **không chặn** của vòng Về đích (`Đ-7.2`)
4. Tín hiệu **có hiệu lực NGAY, KHÔNG chờ admin duyệt** (`Đ-54`, chốt 27/07) — hàng đợi Về đích **không chặn**, queue chỉ ghi nhận thứ tự làm lưới an toàn
5. Nút NSHV disable ngay tại mốc đó

> **Câu chữ cũ của hai bước này (*"Admin duyệt Yes = chấp nhận, No = từ chối"* · *"Nút disable nếu admin Yes"*) là LỖI DIỄN ĐẠT, đã sửa ngày 27/07.** Nó mô tả **mode sân khấu** — nơi **admin là người bấm** (`Đ-41`), nên dialog Yes/No ở đó là **chống bấm nhầm**, không phải duyệt tín hiệu của thí sinh. Về sau bị đọc thành *"admin duyệt tín hiệu"*, tạo ra một mâu thuẫn với Conditions điểm 4 vốn không có thật. `Đ-54` chốt vế *"có hiệu lực ngay"*; xem `game-state-machine.md` `T-055`.
6. Admin bấm "hiển thị câu" → mốc đóng cửa sổ NSHV
7. TS trả lời → admin chấm → tính × 2 (đúng) hoặc −value (sai)

### Boundaries
- **Số lần NSHV = 0**: chưa dùng (hợp lệ)
- **Số lần NSHV = 1**: dùng được (1 lần/trận)
- **Số lần NSHV ≥ 2**: bị reject (nút disable sau lần 1)
- **Giá trị ×2 = 40 (20×2) / 60 (30×2)**: cứng, không tùy chỉnh
- **Hình phạt sai = −20 / −30**: cứng

### Idempotency
Bấm NSHV lần 1 → admin Yes → flag set. Bấm lần 2 → reject (nút disable). Chấm Đúng 2 lần → 1 event (×2 vẫn tính 1 lần).

### Concurrency
Nếu 2 TS cùng bấm NSHV trong cùng câu: mỗi TS độc lập, không ảnh hưởng nhau.

### Examples
**Hợp lệ:**
- Câu 20đ, TS bấm NSHV (**có hiệu lực ngay**, `Đ-54`), TS trả lời Đúng → +40.
- Câu 30đ, TS bấm NSHV, TS trả lời Sai → −30 (kể cả có người cướp Đúng).
- TS bấm NSHV lần 1, lần 2 (nút disable) → lần 2 không được bấm.
- **TS đặt NSHV rồi trả lời Sai + có người cướp Đúng** (`U-8` đóng 26/07, C5b): câu 30đ ⇒ **A −30 · B +30**. A mất giá trị câu **đúng một lần** — **không** cộng dồn thành −60.

**Không hợp lệ:**
- TS thao tác đặt ngôi sao sau khi admin đã bấm "hiển thị câu" → nút đã tắt, không có tín hiệu nào được tạo; ngôi sao vẫn chưa dùng.
- **[mode sân khấu]** máy thí sinh render nút NSHV → không tồn tại; thí sinh **nói miệng**, admin bấm (`Đ-41`).

### Source traceability
- **Quyền đặt cược**: `game-rules-inventory.md` R-VD-06 · `game-rules-decisions.md` §9.4
- **1 lần/TS/trận**: `game-rules-inventory.md` R-VD-06 ("mỗi thí sinh được đặt ngôi sao hy vọng 1 lần")
- **Đúng ×2**: `game-rules-inventory.md` R-VD-06 ("được gấp đôi số điểm")
- **Sai −value, bất kể cướp**: `game-rules-inventory.md` R-VD-06 ("kể cả các thí sinh còn lại có giành quyền trả lời hay không")
- **Mốc đóng cửa sổ**: `game-rules-inventory.md` R-VD-06 (`game-rules-review-old.md` GRR-048 — "trước khi câu được đọc lên hoặc hiện lên")
- **Hàng đợi**: không rõ NSHV có chặn không, chỉ có VCNV rõ chặn (Đ-7.2)

## GR-022 — Câu hỏi phụ: điều kiện kích hoạt

### Status

CONFIRMED

### Purpose
Xác định thời điểm và điều kiện bắt đầu vòng Câu hỏi phụ (Tie-break) trong một trận.

### Actors
- Server
- Admin

### Related states
- Match state: **`LOBBY`** (đã xong Về đích, **chưa chốt**) → `FINISHED` (không hoà) | `TIE_BREAK` (có hoà) — rẽ nhánh **tại cú bấm *"Chốt trận"*** (`EVENT-045`, `Đ-48`), không tự động
- Round: nằm ở cuối playlist

### Trigger
Hoàn thành vòng Về đích (About-Finish, All-Seas)

### Preconditions
- Vòng Về đích đã kết thúc (cửa sổ cướp quyền của câu cuối đóng và chấm xong)
- Tối thiểu **hai** thí sinh có **cùng số điểm cao nhất** tại mốc đó
- TieBreakConfig đã được cấu hình và hợp lệ ở cuối playlist

### Inputs
- Bảng điểm tích luỹ sau vòng Về đích
- Danh sách `tieBreakPositions` (vị trí cần phân định; default: `[1]` = chỉ vị trí NHẤT)
- Danh sách thí sinh còn trong trận

### Conditions
- **Hoà điểm**: Nhóm thí sinh có điểm = điểm cao nhất
- **Trong phạm vi vị trí**: Thí sinh hoà phải ở vị trí nằm trong `tieBreakPositions`
- **Nhóm ≥2 người**: Chỉ kích hoạt khi nhóm hoà có tối thiểu 2 người

### Decision table

| Case | Conditions | Expected outcome | State change | Error |
|---|---|---|---|---|
| C1 — Hoà ở vị trí NHẤT (default) | Admin bấm **Chốt trận** ở `LOBBY`; 2+ TS cùng điểm cao nhất, vị trí 1 ∈ `tieBreakPositions` | Chuyển `TIE_BREAK` | Match state: **`LOBBY` → `TIE_BREAK`** (`Đ-48` — **không** đi qua `FINISHED`) · Gán `tiedSeats` = [2+ ghế hoà] | — |
| C2 — Hoà ở vị trí khác | Như C1; 2+ TS cùng điểm, vị trí ≠ 1 và ∈ config | Chuyển `TIE_BREAK` | Cùng C1 | — |
| C3 — Không hoà | Admin bấm **Chốt trận**; 1 TS điểm cao nhất | Kết thúc trận | Match state: **`LOBBY` → `FINISHED`**, `matchClosedReason = "hoàn thành"`. Đóng sổ là **cú bấm của admin**, không tự động (`Đ-48`) | — |
| C4 — Hoà ngoài phạm vi | Như C3; 2+ TS cùng điểm, vị trí ∉ `tieBreakPositions` | Kết thúc trận, đồng hạng | Match state: **`LOBBY` → `FINISHED`** · Ghi nhận cả nhóm cùng hạng trong biên bản | — |
| C6 — **Chưa bấm Chốt trận** (`Đ-48`, chốt 27/07) | Vòng Về đích đã xong, admin **chưa** bấm gì | **Trận đứng ở `LOBBY`** — chưa tính hoà, chưa đóng sổ, chưa có thứ hạng | Không đổi. Admin **vẫn sửa điểm được** (`GR-029`), và điểm sửa **được tính vào** phép phân định hoà khi bấm chốt | — |
| C5 — **Trận không thể hoàn thành** (`Đ-47`, chủ dự án chốt 27/07) | Mất điện, hỏng thiết bị, huỷ buổi thi — admin bấm **"Huỷ trận"** (`EVENT-044`) từ **bất kỳ** trạng thái nào, kể cả giữa một vòng | Trận **đóng sổ** với nhãn **`matchClosedReason = "bỏ dở"`**. **Điểm giữ nguyên, không revert**; **KHÔNG phân định thứ hạng, không người thắng** | Match state: `FINISHED` — **enum vẫn 4 giá trị**, `Đ-38` không mở lại; đề xuất state `ABANDONED` của `GRR-077` **bị bác ở dạng state, giữ ở dạng nhãn**. Ghi `closedBy` / `closedAt` / `reason` · biên bản in nhãn *"trận bỏ dở"* kèm đủ các vòng đã chạy · thống kê (`SM-11`) không đếm vào trận hoàn thành | Dialog **hạng phá huỷ** + **lý do bắt buộc** |

### Outcomes
- **Kích hoạt tie-break**: Chuyển trạng thái trận sang `TIE_BREAK`, gọi admin chọn nhóm người tham gia (hoặc auto-select theo recommendation)
- **Không kích hoạt**: Trận chuyển `FINISHED`, đơn theo điểm (thắng ở vị trí NHẤT đã xác định)
- **Đồng hạng lưu biên bản**: Ghi rõ vị trí hoà và các thí sinh cùng hạng

### State changes

> **Sửa 27/07 theo `Đ-48` / `Đ-51`.** Trước đó mục này ghi *"`FINISHED` ← hoàn thành vòng Về đích nhưng chưa phân định"* rồi C1 ghi *"`FINISHED` → `TIE_BREAK`"* — đọc như thể **rời được** `FINISHED`. Trạng thái *"đã hết vòng Về đích nhưng chưa phân định"* **có tên riêng và tên đó là `LOBBY`**: mọi vòng ra qua `LOBBY` (`Đ-46a`), trận **đợi ở đó** cho tới khi admin bấm *"Chốt trận"* (`EVENT-045`), và phép tính hoà (`EVENT-037`) chạy **tại cú bấm đó** — nên điểm admin sửa ở `LOBBY` **được tính vào**. `FINISHED` là **terminal và niêm phong** (`Đ-51`).

- Match: `LOBBY` ← hoàn thành vòng Về đích (`Đ-46a`) — **chưa đóng sổ, chưa tính hoà**
- Match: `LOBBY` → `TIE_BREAK` ← admin bấm **Chốt trận** (`EVENT-045`) và `EVENT-037` cho ra **có hoà** trong `tieBreakPositions`
- Match: `LOBBY` → `FINISHED` ← admin bấm **Chốt trận** và **không hoà** (hoặc hoà ngoài phạm vi); `matchClosedReason = "hoàn thành"`
- Match: `FINISHED` + `matchClosedReason = "bỏ dở"` ← admin huỷ trận (`Đ-47`, `EVENT-044`)
- Queue (Đ-7.b): Reset sau vòng Về đích, sẵn sàng cho vòng Câu hỏi phụ

### No-change guarantees
- **Điểm của thí sinh không đổi** khi kích hoạt tie-break (chỉ ghi nhận trạng thái hoà)
- **Danh sách câu chưa dùng** của contest giữ nguyên
- **Cài đặt playlist** không thay đổi

### Error outcomes
- Pool tie-break không đủ 3 câu: **không bắt đầu được vòng Câu hỏi phụ** (kiểm tại cửa vào vòng)
- TieBreakConfig không hợp lệ (k lệnh pre-flight): Pre-flight chặn trước khi start

### Evaluation order
1. Server tính điểm tích luỹ sau vòng Về đích
2. So sánh điểm, tìm nhóm cao nhất
3. Kiểm tra `tieBreakPositions`
4. Kiểm tra số người (≥2)
5. Nếu thỏa: gọi Phase 1 của Đ-9 (admin chọn người)

### Boundaries
- **Nhóm hoà = 2 người**: Kích hoạt (boundary thấp)
- **Nhóm hoà = 4 người** (toàn sân): Kích hoạt nếu trong config
- **Không hoà** (1 TS): Không kích hoạt
- **Hoà ở vị trí 2, 3, 4**: Kích hoạt nếu config mở rộng

### Idempotency
Phép tính điều kiện hoà chạy **tại cú bấm *"Chốt trận"*** (`EVENT-045`, `Đ-48`), trên bảng điểm **tại đúng mốc đó** — không phải lúc vòng Về đích kết thúc. Kết quả quyết định trạng thái kế (`TIE_BREAK` hoặc `FINISHED`) và **không phụ thuộc vào bấm lặp**: nút chốt một chiều, tự tắt.

### Concurrency
Nếu server đang tính điểm và admin bấm "kết thúc vòng" đồng thời, server hoàn tất tính toán trước, sau đó gọi admin chọn tie-break.

### Examples
- **V1 (4 thí sinh)**: A=100, B=100, C=90, D=85 → Kích hoạt TIE_BREAK cho A, B (vị trí 1)
- **V1 (4 thí sinh), config wider**: A=100, B=100, C=100, D=85; config `tieBreakPositions=[1,2]` → Kích hoạt cho A, B, C
- **Không hoà**: A=110, B=100, C=90 → Không kích hoạt, trận `FINISHED`
- **Hoà ngoài phạm vi**: Điểm cuối: A=100, B=100, C=100 nhưng config `[1]` → Không kích hoạt, ghi 3 người đồng hạng

### Source traceability
- `docs/source/fandom-olympia-26-luat-choi.md` §Câu hỏi phụ *"Sau phần thi Về đích, các thí sinh có cùng số điểm sẽ bước vào phần thi Câu hỏi phụ"*
- `docs/game-rules-inventory.md` §R-TB-01 (điều kiện kích hoạt)
- `docs/game-rules-decisions.md` §9.5 (Đ-9) — luồng 8 phase
- `plans/260711-2340-olympia-contest-system/DEFERED.md` D13.5 (scope: chỉ vị trí NHẤT)

---

## GR-023 — Câu hỏi phụ: thể thức ba câu

### Status

CONFIRMED

### Purpose
Định nghĩa cơ chế trả lời, thời gian, và kết quả của ba câu hỏi trong vòng Câu hỏi phụ.

### Actors
- Thí sinh trong nhóm hoà
- Admin (chọn người, xác nhận phán quyết)
- MC (phán quyết trên sân khấu)

### Related states
- Match state: `TIE_BREAK`
- Pha trong Đ-9: phase 2→8, lặp 3 lần

### Trigger
Bắt đầu vòng Câu hỏi phụ (admin bấm start TIE_BREAK)

### Preconditions
- Vòng Câu hỏi phụ đã được kích hoạt (GR-022)
- Tối thiểu 2 thí sinh trong nhóm hoà
- Bộ 3 câu hỏi được chuẩn bị (rút từ pool, pass pre-flight check)

### Inputs
- Danh sách 3 câu hỏi phụ
- Mức điểm mỗi câu: 0 (không cộng vào điểm trận)
- Thời gian suy nghĩ: **15 giây** / câu (cố định, không config)
- Danh sách thí sinh tham gia (gán ở phase 1, Đ-9)

### Conditions
- **Mỗi câu**: 15 giây suy nghĩ, bấm chuông giành quyền
- **Trả lời miệng** (mode sân khấu) hoặc **gõ máy** (mode nhập liệu — Đ-10.6)
- **Chuông chỉ nhận click chuột** (R-GEN-01)
- **Queue không chặn**: Tín hiệu chuông **tính ngay** theo server timestamp
- **Có người giành quyền ⇒ đồng hồ 15 giây DỪNG NGAY và không chạy tiếp** (`Đ-53`, chốt 27/07). Cửa sổ 15 giây là cửa sổ **suy nghĩ + giành quyền**; sai thì **cả nhóm sang câu kế** (C2) nên không còn ai để đếm giờ cho
- **KHÔNG có đồng hồ trả lời riêng sau khi giành quyền** — luật gốc im lặng **có chủ ý**: nó nói rõ *"tính từ lúc giành được quyền"* ở Khởi động lượt chung và *"suy nghĩ **và trả lời**"* ở Về đích, nhưng ở Câu hỏi phụ **chỉ ghi** *"Thời gian suy nghĩ cho mỗi câu hỏi là 15 giây"*, và **không** có chế tài cho việc bấm chuông rồi im lặng (khác −5 của Khởi động). Bấm rồi im ⇒ admin chấm **Sai** ⇒ sang câu kế. **Không phát minh cửa sổ trả lời cho vòng này**

### Decision table

| Case | Conditions | Expected outcome | State change | Error |
|---|---|---|---|---|
| C1 — Người A bấm nhanh nhất, trả lời đúng | A chuông ← server ts, admin chấm Đúng | A thắng tie-break | Match: TIE_BREAK → FINISHED · Thứ hạng: A = NHẤT (vị trí 1 vẫn là A) | — |
| C2 — Người A bấm, trả lời sai | Admin chấm Sai | Cả nhóm sang câu 2 | Câu hiện 1 → (queue reset) → câu 2 mở | — |
| C3 — Hết 15s, không ai bấm | Timeout, không có chuông | Sang câu 2 | Câu 1 đóng (không cộng điểm nên = không ai đúng) | — |
| C4 — Hết 3 câu, chưa có người đúng | Cả 3 câu không ai trả lời đúng | Bốc thăm (GR-025) | TIE_BREAK chuyển phase bốc thăm (Đ-9.b) | — |
| C5 — Cắt ngang (admin bấm chuyển vòng sớm) | Admin/MC yêu cầu bỏ vòng (GR-030) | Vòng Câu hỏi phụ đóng | Match: TIE_BREAK → FINISHED · Theo kết quả hiện có (nếu chưa ai đúng) | Xem GR-030 |

### Outcomes
- **Có người trả lời đúng**: Người đó thắng tie-break, trở thành nhân tố quyết định hạng vị trí
- **Sai**: cả nhóm sang câu tiếp (K-9). Không có tình huống *"nhiều người sai cùng một câu"* — mỗi câu chỉ **một** người giành được quyền trả lời, và chấm xong là kết thúc câu (`Đ-27`).
- **Không ai đúng sau 3 câu**: Gọi GR-025 (bốc thăm)
- **Không cộng điểm**: Kết quả chỉ thay đổi thứ hạng, không thay đổi điểm tích luỹ

### State changes
- Queue: Reset sau mỗi câu (Đ-7.b)
- Pha Đ-9: Tăng từ 1→2→3→(4)
- Điểm: Không đổi (K-9: không cộng vào điểm trận, chỉ đổi thứ hạng)

### No-change guarantees
- **Không cộng / trừ điểm** cho bất kỳ thí sinh nào
- **Thứ tự chuông không ảnh hưởng kết quả khác** (chỉ quyết định ai thắng trong 3 câu)
- **Câu đã dùng** không trả lại pool (Đ-5.2f)

### Error outcomes
- **Pool tie-break cạn giữa vòng**: không tồn tại — đủ 3 câu mới mở được vòng
- **Một thí sinh offline trong lúc vòng** (`game-rules-review-old.md` GRR-129 + R-GEN-09 grace): Server xử lý dropout, cả nhóm được thông báo

### Evaluation order
1. Admin/server mở câu 1, bấm hiển thị, MC đọc
2. Admin start timer (15 giây)
3. Thí sinh bấm, server ghi timestamp
4. Hết 15s: Admin chấm (nếu có tín hiệu) hoặc bỏ qua
5. Loop cho câu 2, 3 (cùng quy tắc)
6. Nếu sau 3 câu vẫn chưa có người đúng: → GR-025

### Boundaries
- **Thời gian = 15 giây** (cố định, không thay đổi per-question)
- **Số câu = 3** (cố định, chỉ có trong luật O26)
- **Thí sinh tối thiểu = 2** (nếu chỉ 1 người → không cần tie-break, phải lùi GR-022)

### Idempotency
- Mỗi chuông đúng = 1 event đúng cho người đó (nếu trùng admin chấm Đúng lần 2, dedup theo Đ-3.3: `(câu, thí sinh, Đúng)` = 1 event)
- Nếu admin bấm Sai lần 2 ← cùng câu, cùng người: sinh revert + event mới (Đ-3.3)

### Concurrency
- Queue không chặn (Đ-7.2): hai người bấm cùng ms ⇒ hàng đợi tự quyết định thứ tự, ngẫu nhiên; thứ tự đó được ghi lại

### Examples
- **Happy path**: Câu 1 hết 15s, không ai bấm → Câu 2, C bấm ← 7 giây, admin chấm Đúng → C thắng, vị trí 1 = C
- **Sai rồi đúng**: Câu 1 A bấm sai, câu 2 B bấm đúng → B thắng
- **Hết 3 câu không ai đúng**: Câu 3 hết 15s → bốc thăm (GR-025)
- **Admin bấm Sai nhầm**: không bấm lại được — nút chấm **tự khoá sau lần bấm đầu** (`Đ-17`). Muốn sửa thì đi qua hoàn nguyên event log (GR-028) hoặc điều chỉnh thủ công (GR-029).

### Source traceability
- `docs/source/fandom-olympia-26-luat-choi.md` §Câu hỏi phụ *"Các thí sinh trả lời 3 câu hỏi. Thời gian suy nghĩ cho mỗi câu hỏi là 15 giây. Thí sinh bấm chuông nhanh nhất trả lời đúng sẽ là thí sinh có số điểm cao nhất bằng với số điểm của thí sinh còn lại. Nếu trả lời sai, các thí sinh sẽ bước sang câu hỏi tiếp theo."*
- `docs/game-rules-inventory.md` §R-TB-02 (thể thức)
- `docs/game-rules-decisions.md` §9.5 §9.4 (không cộng điểm — Đ-9, `game-rules-review-old.md` GRR-058)
- `docs/reviews/game-rules-review-old.md` GRR-021 (reject không mất lượt), GRR-059 (sai không bị loại câu sau)

---

## GR-024 — Câu hỏi phụ: bấm chuông trước hiệu lệnh

### Status

**CONFIRMED — nhưng `Đ-55` (27/07) làm rule này KHÔNG CÒN ĐƯỜNG PHÁT SINH trong v1.**

> **Máy KHỬ tình huống thay vì PHẠT nó.** Nút chuông ở Câu hỏi phụ **không sống trước mốc admin bấm start timer**, nên **không tồn tại** tín hiệu bấm sớm để mà phạt. Toàn bộ C1 → C3 và cơ chế gỡ cấm vì thế **không đạt tới được**; `STATE-023` và `EVENT-019` của `game-state-machine.md` là **trạng thái/event không có đường vào**.
>
> **Rule KHÔNG sai và KHÔNG bị bác.** Mục đích của nó — *không ai được lợi thế do bấm trước hiệu lệnh* — **vẫn được bảo đảm**, và bảo đảm **mạnh hơn**: chặn thì tuyệt đối, phạt thì chỉ răn đe. Luật gốc buộc phải phạt vì trên trường quay nút chuông là **phần cứng, lúc nào cũng sống**; ở bản phần mềm ràng buộc đó **không tồn tại**.
>
> **Giữ nguyên văn rule** để truy nguyên về luật gốc, và để nếu sau này có cấu hình cho phép chuông sống sớm thì đã có sẵn thể thức xử lý.
>
> **Zero-trust**: disable nút chỉ là UX — **server vẫn phải từ chối** mọi tín hiệu chuông tới trước mốc start ở vòng này (cùng khuôn `GR-034` C6).
>
> **KHÔNG áp cho Khởi động lượt chung**: ở đó luật gốc cho phép *"bấm chuông trong khi người dẫn chương trình đang đọc câu hỏi"* — chuông vẫn sống từ mốc **hiển thị câu**. Xem `game-state-machine.md` `EVENT-005`.

### Purpose
Định nghĩa hình phạt và cơ chế khi thí sinh bấm chuông trước khi MC phát hiệu lệnh bắt đầu.

### Actors
- Thí sinh (phát tín hiệu chuông)
- MC (quyết định, phát hiệu lệnh)
- Admin (ghi nhận sự kiện, gỡ cấm nếu cần)

### Related states
- Match state: `TIE_BREAK`
- Pha Đ-9: phase 2 (mở câu) → phase 3 (MC hô bắt đầu → admin start timer)
- Cửa sổ lệnh cấm: **giữa phase 2 và phase 3** (Đ-9, Đ-6.2a)

### Trigger
Thí sinh bấm chuông trong khoảng thời gian MC đang đọc câu hỏi (trước khi "hiệu lệnh" phát.

### Preconditions
- Vòng Câu hỏi phụ đang chạy
- Admin đã bấm "hiển thị câu hỏi" (phase 2)
- Admin chưa bấm "start timer" (phase 3) hoặc MC chưa phát hiệu lệnh (Đ-6.1)

### Inputs
- Timestamp tín hiệu chuông từ client thí sinh
- Timestamp "admin start timer" (hoặc "hiệu lệnh MC" nếu có sensor)
- Thí sinh ID

### Conditions
- **Tín hiệu chuông đến trước admin start timer** (server timestamp)
- **Chỉ áp ở vòng Câu hỏi phụ** (K-3: Khởi động lượt chung **cho phép** bấm khi MC đang đọc)
- **Lệnh cấm gắn với CÂU**, không phải với người (Đ-6.2)

### Decision table

| Case | Conditions | Expected outcome | State change | Error |
|---|---|---|---|---|
| C1 — Bấm trước hiệu lệnh | Timestamp buzz < timestamp start-timer | **Mất quyền trả lời câu đó** (TRẠNG THÁI CẤM, gỡ được) | Seat: nhãn "cấm câu X" (Đ-6.2a) · Queue ghi nhận tín hiệu · Audit log | — |
| C2 — MC gỡ cấm trước khi trả lời | MC nói gỡ, admin bấm Yes | Cấm được gỡ, **giữ timestamp gốc** (thứ tự vẫn theo lúc bấm lần đầu) | Seat: gỡ nhãn cấm · Lịch sử tín hiệu giữ nguyên | — |
| C3 — MC gỡ cấm nhưng quá hạn | Admin gỡ cấm sau khi thí sinh khác đã trả lời xong | **Kết quả đã chốt**, gỡ cấm vô hiệu lực (chỉ ghi lịch sử) | Thứ hạng không đổi | — |
| C4 — Bấm đúng lúc (≥ start-timer) | Timestamp buzz ≥ timestamp start-timer | **Hợp lệ**, theo R-TB-02 | Tín hiệu vào queue, admin chấm bình thường | — |
| C5 — Cầu sau khi câu đóng | Bấm sau khi timer 15s hết | **Chuông không có hiệu lực** (vẫn ghi lịch sử, không vào queue duyệt) | Lịch sử tín hiệu append-only, không kích hoạt outcome | — |

### Outcomes
- **Mất quyền trả lời**: Thí sinh bị **cấm trả lời câu này dù bấm chuông (nếu admin gỡ được)**
- **Gỡ cấm**: MC quyết bằng lời, admin bấm, cấm được gỡ, **không mất lượt, timestamp gốc được giữ**
- **Không gỡ**: Thí sinh mất quyền trả lời câu này, phía sau mở cho người khác
- **Lịch sử bất biến**: Tất cả tín hiệu (cả hợp lệ lẫn bị cấm) được ghi trong queue history append-only (Đ-7.1)

### State changes
- Seat: thêm trạng thái cấm ({câu: 2, reason: 'early-buzz'}) — trạng thái phi-điểm (Đ-7.3)
- Queue: tín hiệu vẫn ghi, nhưng không sinh outcome (nếu trước khi start-timer)
- Cửa sổ lệnh cấm: đóng lại sau khi admin bấm start-timer

### No-change guarantees
- **Điểm không đổi** khi bấm sớm (không trừ điểm, không cộng)
- **Lượt không bị mất** nếu admin / MC gỡ cấm
- **Lịch sử tín hiệu không bị xoá** (append-only, Đ-7.1)
- **Kết quả câu khác không ảnh hưởng** — mỗi câu độc lập

### Error outcomes
- "Hiệu lệnh MC" = **admin bấm start timer** (`Đ-6.1`) — hệ thống không quan sát sân khấu
- Admin bấm start-timer nhầm lúc thí sinh vẫn đọc: không outcome (timestamp quyết định)
- Cơ chế gỡ cấm: chỉ hoạt động **trước khi trả lời** (Đ-6.4)

### Evaluation order
1. Admin bấm "hiển thị câu hỏi" (phase 2, Đ-9, mốc admin)
2. Thí sinh có thể bấm trong khoảng (cửa sổ cấm mở)
3. MC đọc, đối thoại với audience
4. MC phát hiệu lệnh (hoặc admin bấm start-timer, Đ-6 / Đ-6.1)
5. Server so sánh timestamp: buzz < start-timer? → cấm
6. Nếu MC gỡ cấm: lệnh cấm hết hiệu lực (nhưng lịch sử giữ)
7. Thí sinh khác bấm: tính bình thường (hoặc gỡ cấm nếu tính lại)

### Boundaries
- **Trước start-timer bao nhiêu** = cấm (không giới hạn, chỉ < mốc admin)
- **Sau start-timer bao nhiêu** = hợp lệ (ngay ms sau cũng được)
- **Cửa sổ gỡ cấm = khoảng còn lại của câu** (Đ-6.4: phải trước trả lời, trong lúc câu mở)

### Idempotency
- Mỗi tín hiệu chuông = 1 entry trong lịch sử (append-only)
- Admin bấm **gỡ lệnh cấm**: nút một chiều, **tự tắt sau khi bấm** (`Đ-29`) ⇒ không có lần bấm thứ hai ở giao diện; server bỏ qua lệnh gỡ trùng (`CLAUDE.md` §Zero-trust). *(Còn treo, KHÔNG chặn rule này: thao tác gỡ có sinh **event hoàn nguyên** và có cần **dialog xác nhận** không — `Đ-6.3c`, đề xuất là **có event, không dialog**. Đây là chi tiết ghi log/giao diện, không đổi outcome của GR-024)*

### Concurrency
- Hai thí sinh bấm sớm cùng lúc: cả hai bị cấm (timestamp < start-timer cho cả hai)
- Admin gỡ cấm của A, B bấm sớm câu khác: độc lập (cấm là per-câu)

### Examples
- **Bấm sớm 2 giây**: Câu 2, timer chưa start, A bấm → "A cấm câu 2"
- **MC gỡ cấm**: A bấm sớm, MC nói "gỡ cho A", admin bấm Yes → A được bấm lại, timestamp gốc = 2 giây trước start (thứ tự so với C bấm đúng lúc = theo timestamp gốc)
- **Gỡ cấm quá hạn**: A bấm sớm, MC không gỡ ngay, B bấm đúng lúc + admin chấm B đúng + B thắng → 2 phút sau admin gỡ cấm cho A = vô ích (câu đã kết thúc, kết quả chốt)
- **Không gỡ cấm**: A bấm sớm, MC im lặng, B bấm đúng lúc = B được nhìn nhận

### Source traceability
- `docs/source/fandom-olympia-26-luat-choi.md` §Câu hỏi phụ *"Trong một câu hỏi, nếu có thí sinh bấm chuông trả lời trước khi có hiệu lệnh của người dẫn chương trình, thí sinh đó sẽ bị mất quyền trả lời câu hỏi."*
- `docs/game-rules-inventory.md` §R-TB-03 (bấm chuông trước hiệu lệnh)
- `docs/game-rules-decisions.md` §8.3 §8.2 (Đ-6.2 → Đ-6.4: phạm vi hình phạt, cơ chế gỡ cấm)
- `docs/reviews/game-rules-review-old.md` GRR-024 (nếu có) hoặc GRR-033 (K-3: Khởi động vs Tie-break khác)

---

## GR-025 — Câu hỏi phụ: hết câu chưa phân định

### Status

CONFIRMED

### Purpose
Định nghĩa cơ chế khi hoàn tất 3 câu hỏi phụ nhưng vẫn chưa xác định được người thắng.

### Actors
- Server (random draw)
- Admin (xác nhận kết quả bốc thăm)

### Related states
- Match state: `TIE_BREAK`
- Pha Đ-9: phase 8 → phase bốc thăm (Đ-9.b)

### Trigger
Kết thúc câu hỏi thứ 3 của tie-break, không có thí sinh nào trả lời đúng

### Preconditions
- Đã hỏi hết 3 câu hỏi (GR-023)
- Kết quả: 0 thí sinh trả lời đúng (hoặc toàn bộ sai, hoặc timeout)
- Danh sách thí sinh trong nhóm hoà ≥ 2 người

### Inputs
- Danh sách thí sinh trong nhóm tie-break
- `exhaustedFallback` config (default: `'random-draw'`)
- RNG seed (nếu có — không khai ở SPEC)

### Conditions
- **Tất cả 3 câu đã closed** (15 giây mỗi câu hết)
- **Không ai trả lời đúng** (tất cả timeout hoặc sai, hoặc bị cấm cả 3 câu)
- **Nhóm hoà ≥ 2 người**
- **exhaustedFallback = 'random-draw'** (mặc định, chỉ config hợp lệ cho v1)

### Decision table

| Case | Conditions | Expected outcome | State change | Error |
|---|---|---|---|---|
| C1 — 3 câu chưa ai đúng, 2+ người | Kết quả 3 câu = [sai/sai/sai] hoặc [timeout/timeout/timeout] | **Bốc thăm** random (admin xác nhận) | Server sinh random ID | — |
| C2 — Admin xác nhận bốc thăm | Admin bấm Yes trên kết quả random | Người bốc trúng = **NHẤT** (vị trí tie-break) | Match: TIE_BREAK → FINISHED · Thứ hạng final: người bốc trúng = 1 | — |
| C3 — Admin bấm "bốc lại" | Admin muốn random lại (mạng lag, server hỏng) | **Bốc thăm lần 2** (random mới) | **Cả hai lần đều là event thật trong log** (append-only, nguyên tắc nền điểm 6 — lịch sử linear, không bao giờ xoá). **Lần cuối cùng có hiệu lực**; lần 1 **không bị đánh dấu vô hiệu**, nó chỉ bị một event sau ghi đè kết quả. Đóng `game-rules-review.md` GRR-157 | — |
| C4 — Pool câu phụ cạn | Không đủ 3 câu cho vòng Câu hỏi phụ | **KHÔNG BẮT ĐẦU ĐƯỢC vòng Câu hỏi phụ** — pre-flight chặn tại cửa vào vòng. Không tồn tại tình huống cạn giữa vòng | Vòng không mở; các vòng khác vẫn bắt đầu được | — |

### Outcomes
- **Bốc thăm**: Server / admin xác nhận 1 người từ nhóm = người thắng vị trí NHẤT
- **Kết quả chốt**: Kết quả bốc thăm là **tối sau cùng**, không được undo (event append-only, Đ-5.3)
- **Lịch sử bất biến**: nếu bốc lại, **cả lần 1 và lần 2 đều nằm trong event log**; **lần cuối cùng có hiệu lực** (điểm 6). Không có khái niệm *"huỷ lần 1"* — sửa kết quả chỉ bằng cách thêm event sau
- **Nhóm hoà không được phân định**: nếu admin **không** chọn phân định một nhóm, các thành viên nhóm đó ghi **ĐỒNG HẠNG** (`GRR-057`/`Đ-10.7a` — theo standard competition ranking đã duyệt ở `GRR-032`)

### State changes
- MatchEvent: thêm event "TIEBREAK_DRAW" hoặc tương tự (ghi kết quả bốc)
- Match: TIE_BREAK → FINISHED
- Thứ hạng: Cập nhật người bốc trúng = vị trí 1 (của nhóm hoà)

### No-change guarantees
- **Điểm không đổi** (tie-break không cộng điểm, chỉ thay đổi thứ hạng)
- **Lịch sử 3 câu không xoá** (append-only)
- **Những người khác thứ hạng giữ nguyên** (chỉ nhóm tie-break được xếp lại)

### Error outcomes
- Pool câu phụ không đủ: vòng không mở được; admin chuyển sang phương án ngoài hệ thống hoặc bổ sung đề rồi mở lại
- RNG hỏng: Fallback sang admin quyết định (advisory)
- Bấm bốc lại khi đã confirm (`game-rules-review.md` GRR-160): **nút bốc thăm là nút một chiều, tự tắt sau khi bấm** (`Đ-29`). *"Bốc lại"* là **một thao tác riêng, có dialog Yes/No** (thao tác không hoàn tác được ⇒ `CLAUDE.md` §UX), sinh **event mới**, không phải bấm trùng nút cũ

### Evaluation order
1. Câu 3 đóng, server kiểm tra: có ai trả lời đúng không?
2. Nếu không: gọi bốc thăm
3. Server random 1 người từ nhóm
4. Hiện kết quả cho admin (đề xuất)
5. Admin bấm Yes (hoặc bấm Bốc lại)
6. Kết quả chốt, update thứ hạng
7. Hệ thống chuyển FINISHED

### Boundaries
- **Nhóm hoà = 2 người**: Bốc thăm (boundary thấp)
- **Nhóm hoà = 4 người**: Bốc thăm (boundary cao, chỉ nếu config mở rộng)
- **Pool tie-break = 0 câu còn lại**: không mở được vòng Câu hỏi phụ

### Idempotency
- Mỗi lần bấm "Bốc thăm" sinh **1 random event mới** — thao tác này **KHÔNG idempotent theo thiết kế**, và đó là chủ đích: mục đích của "bốc lại" chính là ra một kết quả khác. Chống bấm trùng ngoài ý muốn nằm ở `Đ-29` (nút tự tắt) + dialog Yes/No, **không** nằm ở dedup phía server
- Nếu admin bấm "Yes" trên kết quả bốc lần 1 rồi bấm lại "Yes" (lag) → lần 2 là no-op (đã xác nhận rồi)

### Concurrency
- Không có — vòng TIE_BREAK chỉ admin điều khiển, không có thí sinh action nào cạnh tranh

### Examples
- **2 người hoà**: A=100, B=100 → 3 câu chưa ai đúng → random [A|B] → server chọn B → B NHẤT
- **Bốc lại**: Random chọn A, admin click "Bốc lại" → random lại, lần 2 = B
- **3 người hoà** (nếu config `[1,2,3]`): A, B, C cùng 100 → bốc → server chọn C → C = vị trí 1 · A, B vẫn đồng hạng dưới C
- **Pool cạn**: Câu 1/2/3 chỉ có 2 câu hợp lệ → admin gặp "câu 3 = N/A" → advisory

### Source traceability
- `docs/source/fandom-olympia-26-luat-choi.md` §Câu hỏi phụ *"Sau 3 câu hỏi, nếu không tìm được thí sinh thắng cuộc, các thí sinh sẽ phải bốc thăm để chọn ra thí sinh thắng cuộc."*
- `docs/game-rules-inventory.md` §R-TB-04 (hết câu chưa phân định)
- `docs/game-rules-decisions.md` §9.5 (Đ-9.b: vòng lặp)
- `docs/reviews/game-rules-review.md` GRR-155 (điều kiện kích hoạt bốc), GRR-156 (kết quả ghi bằng vật gì), `game-rules-review.md` GRR-157 (idempotency bốc)

---

## GR-026 — Phán quyết của admin

### Status

CONFIRMED

### Purpose
Định nghĩa quyền và cơ chế phán quyết Đúng/Sai của admin, phân biệt giữa trận chính thức (official) và tự luyện tập (practice).

### Actors
- Admin (phán quyết duy nhất ở official; co-occur với autoJudge ở practice)
- Server (hiển thị gợi ý, tính điểm theo phán quyết)
- Reviewer / MC (kiến nghị, không phán quyết)

### Related states
- Match config: `matchPurpose` (official | practice)
- Match state: mọi vòng (khi câu được trả lời)
- Trạng thái câu: submitted → scored (nếu admin bấm) | unscored (nếu admin chưa bấm)

### Trigger
Một thí sinh gửi đáp án (submission)

### Preconditions
- Câu hỏi được trả lời (thí sinh gửi submission hoặc hết giờ)
- Match config: `matchPurpose: 'official'` OR `'practice'`
- Server nhận & ghi nhận submission + timestamp

### Inputs
- Submission text (thí sinh gửi hoặc trống = timeout / không trả lời)
- Expected answer(s) từ Question config
- `matchPurpose` (quyết định ai chấm)
- Kết quả normalize & highlight (đầu vào để gợi ý, không phải phán quyết)

### Conditions

**Trận chính thức (official):**
- **`autoJudge` KHÔNG tồn tại** dưới bất kỳ hình thức nào
- Điểm **chỉ chốt khi ADMIN bấm Đúng/Sai** (hotkey C/X)
- Server chỉ **(1) hiển thị đáp án thí sinh + đáp án đúng**, **(2) gợi ý kết quả so khớp** (highlight ký tự khác)
- Admin quyết định dựa trên gợi ý, không bị ràng buộc

**Tự luyện tập (practice) [v1.5+]:**
- `autoJudge` **tồn tại** (v1 không có practice nên không có autoJudge)
- Máy so khớp & tự cộng/trừ điểm (nếu autoJudge bật)
- Admin vẫn có thể override

### Decision table

| Case | Conditions | Expected outcome | State change | Error |
|---|---|---|---|---|
| C1 — Official, admin chấm Đúng | Match official, submission hợp lệ, admin bấm C | **+điểm** (theo giá trị câu) | MatchEvent: `{action: JUDGE, result: CORRECT}` · Điểm tích luỹ += value | — |
| C2 — Official, admin chấm Sai | Match official, submission, admin bấm X | **−điểm** (hoặc 0, tùy vòng) | MatchEvent: `{action: JUDGE, result: INCORRECT}` · Điểm tích luỹ += (−penalty hoặc 0) | — |
| C3 — Official, admin chưa bấm | Match official, submission vẫn treo | **Không sinh điểm** (gợi ý so khớp không có hiệu lực trong trận) | Câu = unscored · Viewer/overlay thấy "chưa chấm" hoặc trống | — |
| C4 — Practice, autoJudge ON (v1.5+) | Match practice, autoJudge bật, submission gửi | **Máy so khớp, tự cộng/trừ điểm** | MatchEvent: auto-generated · Điểm tích luỹ cập nhật ngay (không chờ admin) | — |
| C5 — Practice, admin override autoJudge | Match practice, autoJudge result ≠ MC phán, admin bấm | **Admin kết quả thắng** (revert autoJudge, event mới) | MatchEvent: `{action: JUDGE, override: true}` · Điểm được tính lại | — |

### Outcomes

**Official:**
- Admin bấm Đúng: điểm sinh, event ghi, submit quay lại (không chỉnh lại được trừ undo)
- Admin bấm Sai: hình phạt / 0 điểm, event ghi
- Admin chưa bấm: không sinh điểm, có thể tiếp tục chấm câu khác hoặc quay lại câu này
- **Không có cơ chế máy tự chấm** — ở official, máy không bao giờ quyết Đúng/Sai

**Practice:**
- autoJudge chạy: máy so khớp, cộng/trừ tự động (v1.5+, không phải v1)
- Admin có thể xem lại & override
- Audit log ghi cả autoJudge result lẫn admin override

### State changes
- MatchEvent: thêm event JUDGE hoặc AUTO_JUDGE
- Điểm tích luỹ: cập nhật theo phán quyết
- Trạng thái câu: submitted → scored
- Lịch sử submission: giữ nguyên (append-only)

### No-change guarantees
- **Submission của thí sinh KHÔNG bị xoá** (append-only event log, Đ-5.3)
- **Kết quả so khớp (normalize & highlight) KHÔNG ảnh hưởng phán quyết** — chỉ là gợi ý (Đ-1)
- **Ở official: máy không bao giờ thay phán quyết admin**
- **Ở practice: autoJudge là default, admin có thể override** (ngoài v1)

### Error outcomes
- **Câu miệng vs câu gõ** (`U-7` — ĐÓNG): kênh trả lời **không phải thuộc tính của câu hỏi**, nó là **mode của contest** (`Đ-4`) cộng với hai vòng luôn gõ máy (VCNV, Tăng tốc). Ở trận chính thức **cả hai kênh đều do admin chấm** (nguyên tắc nền điểm 1) ⇒ phân biệt này **không đổi outcome của GR-026**, nó chỉ đổi việc có gì để highlight hay không (`GR-027`). Các rule *"ghi nhận đáp án đầu/cuối"*, *"timestamp đáp án"*, *"highlight"* **không bị xoá ở mode sân khấu, chỉ không được kích hoạt** (`GRR-131`)
- **Practice gặp câu MIỆNG** (`U-38` — ĐÓNG): **không tồn tại tình huống này.** Practice **khoá về mode nhập liệu** (`GRR-127`), vì mode sân khấu cần người nghe. Practice thuộc **v1.5**; v1 luôn có người điều khiển nên mọi câu đều có admin chấm
- **Câu chưa chấm không thể bị bỏ lại**: phán quyết là **điều kiện để chuyển sang câu tiếp theo**, nên không tồn tại trạng thái "vòng đã đóng mà còn câu chưa chấm" — **ở mọi vòng, không còn ngoại lệ**.
- **Chốt câu khi mới chấm một phần** (`Đ-43`, chủ dự án chốt 27/07 — đóng `GRR-162`): áp cho **hai vòng chấm theo lô** — **Tăng tốc** và **câu hàng ngang VCNV**. Admin bấm **chốt câu** lúc nào cũng được; **mọi ghế chưa chấm ⇒ SAI**.
  - **Tăng tốc**: ghế mặc định SAI nhận **0** điểm và **không giữ chỗ** trong thang 40/30/20/10. Vẫn là **MỘT** event điểm cho toàn bộ bảng ⇒ huỷ kết quả revert cả bảng.
  - **Câu hàng ngang VCNV**: ghế mặc định SAI nhận **0** điểm và **KHÔNG bị loại** — bị loại chỉ đến từ trả lời **sai Chướng ngại vật** (GR-010).
  - **NGOẠI LỆ — tín hiệu "Mở chướng ngại vật" đang chờ duyệt KHÔNG bị mặc định**: đó là phán quyết **riêng lẻ, có chủ đích**, và SAI ở đó **loại thí sinh**. Không tồn tại đường nào để một cú bấm chốt câu loại một thí sinh.
  - **Không phải máy tự chấm**, không phải ngoại lệ của nguyên tắc nền điểm 1: **cú bấm chốt câu CHÍNH LÀ phán quyết** (*"những ai tôi chưa chấm ⇒ SAI"*). **Không** có bộ đếm nào tự chốt khi hết giờ.

### Evaluation order
1. Thí sinh gửi submission (hoặc timeout)
2. Server ghi nhận: timestamp, nội dung, seat ID
3. Server normalize & highlight (đầu vào gợi ý)
4. **Nếu official**: chờ admin bấm C/X
5. **Nếu practice**: (v1.5+) autoJudge chạy, cộng/trừ điểm; admin có thể xem & override
6. MatchEvent sinh, điểm cập nhật (hoặc không nếu official chưa bấm)

### Boundaries
- **Official**: Admin quyết định 100% (không máy can thiệp)
- **Practice**: autoJudge mặc định (admin override được)
- **v1 (chưa có practice)**: Chỉ có official → không có autoJudge

### Idempotency
Per Đ-3.3:
- Admin bấm **Đúng hai lần** = 1 event (dedup `(câu, thí sinh, Đúng)`)
- Admin bấm **Sai sau Đúng** = revert + event mới

### Concurrency
Không xảy ra — **mỗi contest chỉ có MỘT admin duy nhất**, nên không tồn tại hai luồng thao tác admin đồng thời

### Examples
- **Official, C đúng**: A gửi "Huế", admin bấm C → +10
- **Official, B sai, lượt chung**: A bấm chuông, admin chấm X → −5
- **Official, chưa chấm**: A gửi, hạn 3s hết, admin bận → câu vẫn unscored, viewer thấy trống
- **Practice (v1.5+)**: A gửi "Huế", autoJudge so khớp đúng → +10 ngay · Nếu accept thêm "Hue" thì autoJudge cũng cộng; nếu MC nói sai thì admin override → −5 thay thế

### Source traceability
- `docs/game-rules-inventory.md` §R-GEN-03 (chấm điểm — đúng/sai do admin quyết)
- `docs/game-rules-decisions.md` §3.1 (Đ-1: máy chỉ highlight, admin chấm)
- `docs/reviews/game-rules-review.md` GRR-162 (nhánh else: admin không bao giờ bấm)
- `plans/260711-2340-olympia-contest-system/DEFERED.md` D4 (autoJudge gắn `matchPurpose: practice`, chỉ v1.5+) · D8 (quyết định user 2026-07-23: ghi đè D4)

---

## GR-027 — Chuẩn hoá và highlight đáp án

### Status

CONFIRMED

### Purpose
Định nghĩa cơ chế xử lý và hiển thị đáp án để giúp admin phán quyết, đặc biệt trong trận chính thức (official) nơi máy không tự chấm.

### Actors
- Server (normalize, highlight)
- Admin (quyết định dựa trên kết quả gợi ý)
- Setter (cấu hình `acceptedAnswers`, `normalizeOptions`)

### Related states
- Match state: mọi vòng (khi có submission)
- Match config: `normalizeOptions` (cấu hình normalize)

### Trigger
Thí sinh gửi đáp án (submission)

### Preconditions
- Submission nhận được từ thí sinh
- Question config có `acceptedAnswers` list
- `normalizeOptions` được cấu hình (mặc định: case-insensitive + trim + collapse space)

### Inputs
- Submission text từ client thí sinh
- `acceptedAnswers[]` từ Question
- `normalizeOptions`: {caseInsensitive: bool, trim: bool, collapseSpace: bool, stripDiacritics: bool}

### Conditions
- **Bắt buộc**: case-insensitive, **trim đầu/cuối**, **collapse khoảng trắng thừa** (Đ-4 mặc định)
- **Tuỳ chọn**: bỏ dấu tiếng Việt (stripDiacritics = config)
- **Không bao giờ tự chấm** ở official (Đ-1, R-GEN-03): normalize chỉ là **đầu vào của gợi ý**

### Decision table

| Case | Conditions | Expected outcome | State change | Error |
|---|---|---|---|---|
| C1 — Khớp chính xác sau normalize | Submission "  HUẾ  " vs ans "Huế", case-insensitive + trim ON | **Highlight = KHÔNG CÓ khác biệt** (100% match) | UI: "Khớp đúng chính tả" (gợi ý) · Admin có thể bấm C | — |
| C2 — Khác 1 ký tự | Submission "Hué" vs ans "Huế", stripDiacritics OFF | **Highlight = ký tự khác** (dấu ') | UI: "Khác chỗ: ký tự 2" · Admin bấm C hoặc X tùy MC | — |
| C3 — Khác 1 ký tự, bỏ dấu ON | Submission "Hue" vs ans "Huế", stripDiacritics ON | **Highlight = không khác** (dấu bỏ qua) | UI: "Khớp nếu bỏ dấu" (gợi ý tuỳ chọn) | **`K-6` ĐÃ TAN — không còn là conflict.** F26 (*"dấu sai → không công nhận"*) nói về **PHÁN QUYẾT**, còn `stripDiacritics` chỉ đổi **CÁCH TÔ MÀU**. Vì máy không phán quyết (nguyên tắc nền điểm 1), hai phát biểu không cùng đối tượng nên không thể mâu thuẫn: bỏ dấu ON nghĩa là *"đừng tô đỏ chỗ khác dấu"*, **không** nghĩa là *"công nhận đúng"*. Admin vẫn chấm Sai theo F26 nếu muốn |
| C4 — Khác từ ngữ | Submission "Thành phố Huế" vs ans "Huế" | **Highlight = từ thêm/thiếu** (mỗi từ khác) | UI: "Có từ thêm: Thành phố" · Admin quyết | — |
| C5 — Trống / timeout | Submission = "" hoặc không gửi | **Không highlight** (không so khớp) | UI: "Không có đáp án" · Gợi ý = N/A | — |

### Outcomes
- **Khớp chính xác**: Gợi ý "đúng chính tả" → admin nhìn thấy nên bấm C ★ nhưng không bắt buộc
- **Khác 1-2 ký tự**: Highlight rõ chỗ khác → admin xem MC phán & bấm C/X
- **Khác nhiều**: Highlight các phần khác → admin phán quyết (dùng ngoại lệ *"ý nghĩa tương đồng"*)
- **Rỗng**: Không highlight, gợi ý = "không có đáp án"
- **Gợi ý không ảnh hưởng phán quyết**: ở official, gợi ý chỉ là reference (Đ-1)

### State changes
- Submission state: nhận → normalize → highlight → hiển thị
- Event log: ghi submission gốc, không lưu kết quả normalize (normalize là tạm thời)

### No-change guarantees
- **Submission gốc KHÔNG thay đổi** (append-only, ghi cả trước/sau normalize)
- **Kết quả normalize KHÔNG tự chấm** (chỉ gợi ý)
- **acceptedAnswers KHÔNG bị sửa** động (cấu hình lúc tạo câu, không đổi giữa trận)

### Error outcomes
- **stripDiacritics = ON vs F26** (`K-6`): **KHÔNG còn là mâu thuẫn — `K-6` ĐÃ TAN**, xem C3. F26 (*"dấu sai → không công nhận"*) nói về **PHÁN QUYẾT**; `stripDiacritics` chỉ đổi **CÁCH TÔ MÀU**. Máy không phán quyết (nguyên tắc nền điểm 1) nên hai phát biểu **không cùng đối tượng**: bỏ dấu ON nghĩa là *"đừng tô đỏ chỗ khác dấu"*, **không** nghĩa là *"công nhận đúng"* — admin vẫn chấm Sai theo F26 nếu muốn
- **Ngoại lệ "ý nghĩa tương đồng"** (U-35): Không có cơ chế mã hoá → admin tự đánh giá (chỉ gợi ý text-match được)
- **Trường dữ liệu "câu miệng"** (U-7): Không có → normalize text-based chỉ áp câu gõ được

### Evaluation order
1. Thí sinh gửi submission
2. Server normalize per config: 
   - Lowercase (case-insensitive) 
   - Trim đầu/cuối 
   - Collapse khoảng trắng 
   - Optnl: bỏ dấu (if stripDiacritics)
3. So sánh submission normalize vs acceptedAnswers[] normalize
4. Tính **character-level diff** (highlight ký tự khác)
5. Hiển thị cho admin: submission vs expected + highlight
6. Admin bấm C/X (admin quyết định, không buộc theo gợi ý)

### Boundaries
- **Case-insensitive bắt buộc**: "Huế" = "huế" = "HUẾ"
- **Trim bắt buộc**: "  Huế  " = "Huế"
- **Collapse space bắt buộc**: "Huế  " (3 space) = "Huế " (1 space)
- **stripDiacritics tuỳ chọn**: Có config quyết, default OFF (Đ-4)

### Idempotency
Mỗi lần gọi normalize trên cùng submission → **cùng kết quả** (deterministic, hàm pure)

### Concurrency
Không có — normalize là server-side, chỉ đọc config (không write)

### Examples
- **Submission "  Huế  "** vs ans "Huế": 
  - Normalize: "huế" vs "huế" → **100% match** 
  - Highlight: KHÔNG CÓ khác → gợi ý "đúng chính tả"
- **Submission "Hue"** (không dấu) vs ans "Huế" (có dấu):
  - Normalize (stripDiacritics OFF): "hue" vs "huế" → **khác ký tự 2** (è vs e)
  - Highlight: ký tự 2 khác → gợi ý "khác dấu"
  - Admin xem MC: nếu MC bảo "đúng" vì "ý nghĩa tương đồng" → admin bấm C
- **Submission "Thành phố Huế"** vs ans "Huế":
  - Normalize: "thành phố huế" vs "huế" → **khác từ đầu**
  - Highlight: từ "thành" "phố" khác → gợi ý "có từ thêm"
  - Admin quyết (tùy quy tắc câu)
- **Submission ""** (rỗng) vs ans "Huế":
  - Normalize: "" vs "huế" → **rỗng**
  - Highlight: KHÔNG → gợi ý "không có đáp án"
  - Admin bấm X (hoặc cảnh báo "timeout không trả lời")

### Source traceability
- `docs/game-rules-inventory.md` §R-GEN-04 (normalize — TRIM, case-insensitive, collapse space, tuỳ chọn bỏ dấu)
- `docs/game-rules-decisions.md` §3.1 (Đ-1: máy hiển thị + gợi ý, không tự chấm)
- `docs/reviews/game-rules-review-old.md` GRR-027 · `K-6` (**đã tan**, xem C3 và Error outcomes)
- `docs/source/fandom-olympia-26-luat-choi.md` §VCNV *"bất kỳ sai sót về kí tự, dấu câu, ngữ pháp → không được công nhận"* · §Tăng tốc *"đúng chính tả; ý nghĩa tương đồng được chấp nhận"*

---

## GR-028 — Điểm là hàm của event log

### Status

CONFIRMED

### Purpose
Định nghĩa mô hình dữ liệu điểm: điểm là kết quả tính toán từ event log (append-only), không phải một con số được sửa trực tiếp.

### Actors
- Server (tính toán reduce event log)
- Admin (phát sinh event qua chấm điểm / điều chỉnh thủ công)

### Related states
- Match state: mọi giai đoạn
- MatchEvent: các loại có ảnh hưởng điểm (JUDGE, SCORE_ADJUST, REVERT)

### Trigger
Bất kỳ hành động phát sinh điểm (admin chấm, skip vòng, điều chỉnh tay)

### Preconditions
- Event log không bao giờ xoá (append-only)
- Điểm được tính bằng **reduce(MatchEvent[])** với một reducer function

### Inputs
- MatchEvent[] (toàn bộ sự kiện trong trận, theo thứ tự thời gian)
- Reducer function: `(accumulator, event) => newScore`

### Conditions
- **Lịch sử linear** (append-only): mỗi thao tác thêm event mới, không xoá/sửa event cũ
- **Reset = REVERT** (kiểu `git revert`): thêm event đảo ngược, không quay snapshot
- **Điểm = reduce từ đầu** hoặc **incremental từ mốc lần trước** (tùy implementation, kết quả phải bằng nhau)

### Decision table

| Case | Conditions | Expected outcome | State change | Error |
|---|---|---|---|---|
| C1 — Chấm đúng câu 1 | Admin bấm C cho câu 1 value=10 | Event: `{type: JUDGE, result: CORRECT, value: 10}` · Điểm được tính lại từ event log = 10 | MatchEvent += event | — |
| C2 — Chấm sai câu 2 | Admin bấm X cho câu 2 value=20, penalty=−5 | Event: `{type: JUDGE, result: INCORRECT, penalty: −5}` · Điểm được tính lại từ event log = 10 + (−5) = 5 | MatchEvent += event | — |
| C3 — Điều chỉnh tay (SCORE_ADJUST) | Admin chỉnh +3 (lý do: sửa nhầm) | Event: `{type: SCORE_ADJUST, delta: +3, reason: '…'}` · Điểm được tính lại từ event log = 5 + 3 = 8 | MatchEvent += event · Audit log ghi reason | — |
| C4 — Bỏ vòng (REVERT vòng X) | Admin bỏ vòng Khởi động | Event đảo ngược cho vòng Khởi động | Thêm N event đảo ngược vào log | — |
| C5 — Xem lại lịch sử điểm | Query: score(ts=T) khi đã có 5 events | reduce([events từ đầu tới T]) = điểm tại thời điểm T | Không sinh event (read-only) | — |

### Outcomes
- **Mọi thao tác phát sinh điểm đều vào event log**
- **Điểm luôn = reduce(MatchEvent)** tính từ đầu (replay-able, audit-able)
- **Không có "điểm = number update"** — nó là hàm (computed property)
- **Lịch sử bất biến** để xem lại & phân xử tranh chấp

### State changes
- MatchEvent: append (thêm event mới, không xoá/sửa cũ)
- Điểm tích luỹ: recalculate từ event log
- Audit log: ghi `SCORE_ADJUST` với reason (nếu có)

### No-change guarantees
- **Event log không bị xoá**: Thí sinh A chấm đúng 10 điểm lần 1, sửa lại −5 điểm lần 2 → cả 2 event đều ở log, điểm = 10 + (−5) = 5 (từ recalc)
- **Thứ tự event bất biến**: Không shuffle event log để đạo lý toán
- **Điểm không bao giờ bị edit trực tiếp**: Chỉ edit event log, điểm là kết quả

### Error outcomes
- **Reducer crash** (logic sai, event invalid): Server báo → admin can thiệp (advisory)
- **Event cũ data type lạc hậu** (schema migration): Pre-flight chặn hoặc fallback transformer


### Evaluation order
1. Admin phát sinh hành động (chấm, điều chỉnh, bỏ vòng)
2. Server sinh event, thêm vào MatchEvent[]
3. Trigger recalculate score: `score = reduce(MatchEvent)` từ đầu
4. Cập nhật UI & lưu điểm final
5. Lịch sử giữ lại để xem lại / replay

### Boundaries
- **Điểm tối thiểu**: Không có (cho phép âm, Đ-2)
- **Điểm tối đa**: Không có limit (config tuỳ ý)
- **Lịch sử tối dài**: Toàn bộ vòng × câu × chấm lại (có thể GB log nếu trận dài)

### Idempotency
Mỗi lần `reduce(MatchEvent)` trên cùng event array → **cùng kết quả** (deterministic)

### Concurrency
Không xảy ra — **mỗi contest chỉ có MỘT admin duy nhất**, nên không tồn tại hai luồng thao tác admin đồng thời

### Examples
- **Happy path**: 
  - Event 1: JUDGE CORRECT +10 → score = 10
  - Event 2: JUDGE INCORRECT −5 → score = 5
  - Event 3: SCORE_ADJUST +3 (lý do: MC nói sửa) → score = 8
- **Revert nửa đường**: 
  - Events 1→5: score = 50
  - Event 6: event đảo ngược cho vòng VCNV (bỏ vòng) → events 2-4 đảo ngược
  - New score = reduce([e1, −e2, −e3, −e4, e5]) = calculate lại
- **Audit lịch sử**: Admin xem "điểm tại 14:05:30?" → reduce([e1 lúc 14:00, e2 lúc 14:03, e3 lúc 14:05]) = điểm khi đó

### Source traceability
- `docs/game-rules-decisions.md` §7.1 (Đ-5.3: điểm = event log, revert = thêm event đảo ngược)
- `docs/game-rules-inventory.md` §R-GEN-07 (điểm = reduce, undo có giới hạn)
- `docs/reviews/game-rules-review.md` GRR-147 (độ hạt event điểm: theo người hay theo câu / toàn bảng)
- `plans/260711-2340-olympia-contest-system/phase-06-game-engine.md` §Event sourcing

---

## GR-029 — Điều chỉnh điểm thủ công

### Status

CONFIRMED

### Purpose
Định nghĩa cơ chế điều chỉnh điểm (SCORE_ADJUST) do admin quyết định, độc lập với việc revert event chấm điểm, không tự revert khi bỏ vòng.

### Actors
- Admin (phát sinh điều chỉnh)
- Server (ghi event, tính điểm lại)
- Audit log (ghi reason bắt buộc)

### Related states
- MatchEvent: event SCORE_ADJUST là loại độc lập
- Match state: mọi lúc **khi trận chưa đóng sổ** (không bị ràng buộc ở vòng nào) — **`Đ-51` (27/07) loại `FINISHED`**: trận đã chốt thì **niêm phong**, không điều chỉnh được nữa. Vế *"không ràng buộc ở vòng nào"* giữ nguyên: điều chỉnh vẫn làm được ở `LOBBY` lẫn giữa một vòng đang chạy.

### Trigger
Admin chọn "Chỉnh điểm tay" hoặc tương tự (nút thủ công)

### Preconditions
- Admin có quyền điều chỉnh (role admin)
- Nhập được giá trị delta (+ hoặc −) và lý do
- Lý do **bắt buộc** nhập (không để trống)

### Inputs
- Delta: số nguyên (dương, âm, hoặc 0)
- Reason: text (bắt buộc, audit trail)
- Target seat ID (ghế nào bị chỉnh)

### Conditions
- **Bắt buộc nhập lý do** (không để trống)
- **Độc lập với event chấm điểm**: không ràng buộc vòng, câu, hoặc loại phán quyết
- **Không tự revert** khi bỏ vòng (Đ-11.B): `SCORE_ADJUST` là phán quyết của người, không phải kết quả máy
- **Vào audit log** với timestamp và admin ID

### Decision table

| Case | Conditions | Expected outcome | State change | Error |
|---|---|---|---|---|
| C1 — Chỉnh +5 | Admin: delta=+5, reason="sửa nhầm câu 2", seat=A | Event: `{type: SCORE_ADJUST, delta: +5, reason: '…', seat: A}` · Score A += 5 | MatchEvent += event · Audit: ghi admin + reason + ts | — |
| C2 — Chỉnh −10 | Admin: delta=−10, reason="câu thực hành admin chấm sai", seat=B | Event: `{type: SCORE_ADJUST, delta: −10, reason: '…', seat: B}` · Score B += (−10) = −10 nếu Ban đầu 0 | MatchEvent += event · Score cho phép âm (Đ-2) | — |
| C3 — Chỉnh delta=0 | Admin nhập delta=0, reason="..." | **SINH EVENT** | Điểm không đổi, nhưng event **vẫn được ghi**: lịch sử là append-only và không bao giờ xoá (nguyên tắc nền điểm 6), còn ô **lý do** là **kênh duy nhất** ghi được xuất xứ một quyết định (`S-14` chốt AuditLog không có trường *"người yêu cầu"*). Một điều chỉnh delta=0 kèm lý do chính là **một ghi chú chính thức vào biên bản trận** — vứt nó đi là vứt mất dữ liệu phân xử | — |
| C4 — Chỉnh ngay sau bỏ vòng | Admin chỉnh +3 cho A sau khi bỏ VCNV | **Chỉnh vẫn có hiệu lực** — không tự revert theo bỏ vòng (Đ-11.B) | MatchEvent: vòng bị revert + event chỉnh riêng · Điểm được tính lại từ event log | — |
| C5 — Bỏ vòng **sau** chỉnh | Admin chỉnh +5 cho A, sau đó bỏ vòng Khởi động | **Chỉnh vẫn giữ**: bỏ vòng revert sự kiện Khởi động nhưng **không revert SCORE_ADJUST** (Đ-11.B) | MatchEvent: revert KĐ events, SCORE_ADJUST vẫn ở | **Điều chỉnh "mồ côi" là hành vi ĐÚNG, không phải lỗi** (`GRR-159`). `GRR-117` chốt: bỏ/chạy lại vòng hoàn nguyên **các trạng thái sinh bởi event CỦA VÒNG ĐÓ**; `SCORE_ADJUST` là event **của admin**, không thuộc vòng nào ⇒ nằm ngoài phạm vi hoàn nguyên. Muốn bỏ nó thì hoàn nguyên **chính nó**, như mọi event khác |
| C6 — Chỉnh **sau khi trận đã chốt** | Trận ở `FINISHED` — **cả hai nhãn**, `hoàn thành` lẫn `bỏ dở` | **KHÔNG thực hiện được** — nút không bật; admin thấy toast *invalid state*, **không ép được** | Không sinh event nào | `Đ-51` (27/07) — trận **niêm phong** tại mốc chốt. Cửa sổ điều chỉnh là `LOBBY` **trước** khi bấm *"Chốt trận"* (`Đ-48`); phát hiện sai sót **sau** khi chốt thì ghi ở **trận mới** (`Đ-49`), **không** sửa ngược biên bản đã đóng |

### Outcomes
- **Event vào MatchEvent** với delta + reason + admin ID
- **Điểm được tính lại** từ reduce(MatchEvent)
- **Audit trail bất biến**: ghi reason để hiểu tại sao chỉnh
- **Không revert theo bỏ vòng** (phân biệt rõ với REVERT event)

### State changes
- MatchEvent: thêm event SCORE_ADJUST
- Điểm tích luỹ: cập nhật từ reduce
- Audit log: ghi delta + reason + admin + ts

### No-change guarantees
- **Chỉnh không bị xoá** (append-only)
- **Chỉnh không tự hết hạn theo vòng**: (Đ-11.B) nó là phán quyết của người, độc lập với luật máy
- **Nếu bỏ vòng trước chỉnh**: Chỉnh vẫn sinh event mới, độc lập bỏ vòng

### Error outcomes
- **Lý do trống**: Server reject, báo "bắt buộc nhập lý do"
- **Delta không phải số**: Server reject
- **Seat không tồn tại**: Server reject

### Evaluation order
1. Admin bấm "Chỉnh điểm tay"
2. Nhập delta (tuỳ chọn: +/−) + reason (bắt buộc)
3. Chọn ghế (hoặc auto = ghế hiện tại)
4. Xác nhận (dialog Yes/No, Đ-1.4 — chỉnh điểm = admin thao tác, nên có dialog)
5. Server sinh event, thêm vào MatchEvent
6. Recalculate score từ reduce(MatchEvent)
7. Audit log ghi delta + reason + admin + ts

### Boundaries
- **Delta có thể âm**: không có sàn (Đ-2)
- **Lý do độ dài**: không limit (text field)
- **Tần suất**: không limit (admin chỉnh mấy lần cũng được, mỗi lần 1 event)

### Idempotency
Nếu admin bấm chỉnh lần 2 (lag), **lần 2 = event mới** — thao tác này **KHÔNG dedup**, khác với JUDGE (dedup per `Đ-3.3`), và đó là **chủ đích**: hai lần chỉnh +5 liên tiếp là một yêu cầu hợp lệ (tổng +10), máy không có cách nào phân biệt nó với một cú bấm trùng do lag.

Chống bấm trùng ngoài ý muốn nằm ở **giao diện**, không ở dedup phía server:

- Nút xác nhận điều chỉnh là **nút một chiều, tự tắt sau khi bấm** (`Đ-29`).
- Điều chỉnh điểm là thao tác **không hoàn tác được** ⇒ đi qua **dialog Yes/No** (`CLAUDE.md` §UX).
- **Bắt nhập lý do** (`R-GEN-07`) — một bước gõ tay nữa khiến bấm trùng do lag gần như không xảy ra.

### Concurrency
Không có hai admin nên các lần điều chỉnh luôn tuần tự; nhiều lần điều chỉnh liên tiếp cộng dồn theo thứ tự bấm

### Examples
- **Sửa nhầm**: Admin chấm sai câu 2 (−5), sau đó nhân ra MC nói đúng → chỉnh +5, reason="sửa nhầm câu 2" → score += 5
- **Chỉnh về âm**: Lượt chung C bấm sai (−5), MC yêu cầu chỉnh thêm (do audio nghe không rõ) → chỉnh −5 nữa, score C = −10 (âm được, Đ-2)
- **Chỉnh sau bỏ vòng** (`game-rules-review.md` GRR-159 case): Admin bỏ VCNV (revert +60), rồi chỉnh +10 vì "câu 4 admin bấm nhầm" → score = −60 + 10 = −50 (không tự revert chỉnh)
- **Chỉnh trước bỏ vòng**: Admin chỉnh +5, sau đó admin bỏ vòng Khởi động → chỉnh +5 vẫn ở, KĐ event bị revert

### Source traceability
- `docs/game-rules-inventory.md` §R-GEN-07 (điều chỉnh = SCORE_ADJUST, bắt buộc lý do)
- `docs/game-rules-decisions.md` §6.3 (Đ-11.B: SCORE_ADJUST không tự revert)
- `docs/reviews/game-rules-review.md` GRR-159 (SCORE_ADJUST mồ côi sau bỏ vòng)
- `docs/source/game-rules-decisions.md` §3 (mô hình điểm event log)

---

## GR-030 — Bỏ vòng và chạy lại vòng

### Status

CONFIRMED

### Purpose
Định nghĩa cơ chế bỏ hẳn một vòng hoặc chạy lại vòng, bao gồm revert điểm, quản lý pool đề, và lịch sử audit.

### Actors
- Admin (chọn bỏ hoặc chạy lại)
- Server (revert event, tính điểm lại, quản lý pool)
- Event log (append-only, không xoá cũ)

### Related states
- Match state: `rounds[i]` (bất kỳ vòng nào)
- MatchEvent: tất cả event của vòng bị revert (thêm event đảo ngược)
- Question pool: `usedInContest` cờ **KHÔNG reset** khi bỏ vòng (Đ-5.2f)

### Trigger
Admin chọn "Bỏ vòng" hoặc "Chạy lại vòng" (nút thủ công)

### Preconditions
- Admin có quyền (role)
- Dialog xác nhận (Đ-1.4 — hành động không hoàn tác, cần confirm)
- Pool đề còn lại đủ câu (nếu chạy lại)

### Inputs
- Vòng ID (KHOI_DONG, VCNV, TANG_TOC, VE_DICH, TIE_BREAK)
- Hành động: **"bỏ"** · **"chạy lại"** · **"kết thúc sớm"** (`Đ-46b`) — ba cửa ra chủ động của một vòng đang chạy, bên cạnh nút *"Kết thúc vòng"* thường (chỉ hiện khi đã đủ câu)

### Conditions
- **Vòng phải tồn tại** trong playlist
- **Danh sách event của vòng** phải được xác định rõ (theo vòng)
- **Nếu chạy lại**: pool đề phải còn đủ câu (pre-flight check)
- **Nếu bỏ hẳn**: điểm revert, biên bản vẫn giữ với nhãn "đã bỏ" (Đ-5.2d)
- **Nếu kết thúc sớm** (`Đ-46b`): **điểm KHÔNG revert**; biên bản nhãn **"kết thúc sớm"**; **không** có guard về số câu đã hỏi

### Decision table

| Case | Conditions | Expected outcome | State change | Error |
|---|---|---|---|---|
| C1 — Bỏ vòng Khởi động | Admin bấm "Bỏ vòng KĐ" → dialog Yes | Điểm KĐ revert (REVERT event cho tất cả event KĐ) | MatchEvent: +N event đảo ngược · Điểm recalc · Biên bản: vòng KĐ = "đã bỏ" | — |
| C2 — Chạy lại vòng VCNV | Admin bấm "Chạy lại VCNV" → dialog Yes | Pool check OK → reset queue, mở lại VCNV, admin chọn lại từ đầu | MatchEvent: REVERT VCNV events + reset queue · Playlist: vòng VCNV "đã chạy lại" | Nếu pool không đủ → pre-flight chặn |
| C3 — Bỏ vòng nhưng SCORE_ADJUST ở vòng đó | Admin bỏ KĐ (KĐ events revert), nhưng có +5 SCORE_ADJUST gắn KĐ | **SCORE_ADJUST KHÔNG tự revert** (Đ-11.B) | MatchEvent: revert KĐ events, SCORE_ADJUST vẫn ở | Ví dụ `game-rules-review.md` GRR-159 |
| C4 — Muốn sang vòng B khi vòng A chưa xong (`game-rules-review.md` GRR-164) | Admin đang ở giữa vòng A, còn tín hiệu chờ duyệt | **Không có đường vòng → vòng** (`Đ-46a`): admin **rời vòng A trước** — kết thúc sớm (C6, giữ điểm) · bỏ (C1) · chạy lại (C2) — về `LOBBY`, rồi mở B. **Thứ tự vòng vẫn do admin quyết**, không hệ thống nào ép (`INV-20`) | Match state: A → `LOBBY` → B | Trong màn vòng **không có nút mở vòng khác** (INVALID STATE, không phải hard-block) |
| C6 — **Kết thúc vòng khẩn cấp** (`Đ-46b`, chủ dự án chốt 27/07) | Vòng hỏng giữa chừng, **chưa hỏi đủ câu**, admin muốn đi tiếp mà **giữ điểm** | Vòng khép **ngay tại chỗ** → `LOBBY`. **Điểm GIỮ NGUYÊN, KHÔNG revert** — đây là khác biệt duy nhất và cũng là toàn bộ lý do tồn tại của cửa này. Câu đang mở khép bằng **Huỷ kết quả**, không sinh điểm cho ai (`Đ-43` **không** áp: kết thúc khẩn cấp không phải phán quyết về đáp án) | Không sinh event đảo ngược nào · dọn cờ phạm vi vòng (`Đ-39`) · biên bản: vòng = **"kết thúc sớm"** · câu **đã hiển thị** không trả lại kho, câu **chưa hiển thị** vẫn còn trong danh sách gán | Dialog **hạng phá huỷ** (không tắt được) + **bắt nhập lý do**, cùng khuôn C1 |
| C5 — Bỏ vòng hai lần (lag) | Admin bấm "Bỏ VCNV" lần 1, bấm lại lần 2 | **NO-OP — điểm KHÔNG tụt gấp đôi** (`GRR-160`) | Ba lớp chặn độc lập: **(1)** nút "Bỏ vòng" là nút một chiều, **tự tắt sau khi bấm** (`Đ-29`); **(2)** bỏ vòng thuộc **hạng phá huỷ** ⇒ dialog Yes/No **không tắt được** và **bắt nhập lý do**, nên không thể là cú bấm phản xạ; **(3)** server từ chối lệnh bỏ vòng cho một vòng **đã ở trạng thái đã bỏ** — đây là **INVALID STATE**, không phải dedup (`CLAUDE.md` §Zero-trust) | — |

### Outcomes
- **Bỏ vòng**: Điểm bị revert (event đảo ngược), biên bản nhãn "đã bỏ", câu đã dùng KHÔNG trả lại (Đ-5.2f)
- **Chạy lại**: Vòng reset, queue reset (Đ-7.b), mở lại từ đầu với câu mới (từ pool)
- **Kết thúc sớm** (`Đ-46b`): vòng khép về `LOBBY`, **điểm giữ nguyên**, biên bản nhãn "kết thúc sớm", câu đã dùng KHÔNG trả lại
- **Lịch sử**: Vòng bị bỏ vẫn ghi trong biên bản (dấu vết)
- **Viewer/overlay**: Số điểm thay đổi đột ngột, không hiệu ứng (Đ-5.2e)

### State changes
- MatchEvent: +N event đảo ngược (vòng bị bỏ)
- Điểm tích luỹ: recalculate từ reduce(event còn lại)
- Queue: reset (nếu chạy lại) hoặc giữ nguyên (nếu chỉ bỏ)
- Biên bản vòng: thêm nhãn "đã bỏ" · "đã chạy lại" · **"kết thúc sớm"** (`Đ-46b`)
- Pool đề: `usedInContest` cờ **KHÔNG reset** (Đ-5.2f)

### No-change guarantees
- **Câu đã dùng không trả lại pool** (Đ-5.2f): mỗi câu rút = 1 lần, không hoàn lại dù vòng bỏ
- **Event cũ không xoá**: REVERT event thêm vào (append-only), không xoá event gốc
- **Audit trail bất biến**: cả bỏ lẫn chạy lại đều ghi
- **Nếu bỏ vòng X rồi chạy lại Y**: X giữ nhãn "đã bỏ", Y được track riêng

### Error outcomes
- **Pool cạn**: chạy lại vòng cũng phải qua **cửa vào vòng** — thiếu câu thì không chạy lại được vòng đó
- **Vòng chưa kết thúc**: `game-rules-review.md` GRR-164 cảnh báo (không hard-block)
- **Bỏ vòng bấm 2 lần** (`game-rules-review.md` GRR-160): **no-op** — vòng đã ở trạng thái *"đã bỏ"* thì lệnh bỏ lần nữa là thao tác ở **invalid state**, server từ chối. Xem C5

### Evaluation order
1. Admin chọn vòng
2. Admin bấm "Bỏ vòng" hoặc "Chạy lại"
3. Dialog xác nhận: "Sẽ bỏ hẳn vòng X, revert điểm. Tiếp tục?"
4. Nếu "Bỏ": 
   - Server sinh REVERT event cho mọi event của vòng X
   - Recalculate score
   - Biên bản ghi nhãn "đã bỏ"
5. Nếu "Chạy lại":
   - Pre-flight check pool
   - Sinh REVERT event
   - Reset queue (Đ-7.b)
   - Mở lại vòng, admin chọn câu từ đầu
   - Biên bản ghi nhãn "đã chạy lại"

### Boundaries
- **Vòng bỏ**: có thể bất kỳ vòng nào (KĐ, VCNV, TT, VĐ, TIE_BREAK)
- **Chạy lại**: cần pool còn câu, **ngân hàng chỉ giảm, không tăng** (Đ-5.2f)
- **Số lần chạy lại**: không limit (admin chạy mấy lần cũng được, miễn pool còn)

### Idempotency
Bấm "Bỏ vòng" lần 2 (lag): **no-op, không sinh revert event thứ hai, điểm không tụt gấp đôi.**

Cần phân biệt rõ hai cơ chế — `Đ-3.3` chỉ dedup `JUDGE` và **không** cần mở rộng sang `REVERT`:

| Cơ chế | Áp cho | Vì sao đủ ở đây |
|---|---|---|
| **Dedup** (hai lệnh giống nhau, giữ một) | `JUDGE` | Không dùng cho bỏ vòng |
| **INVALID STATE** (lệnh không hợp lệ ở trạng thái hiện tại) | Bỏ vòng, chạy lại vòng | Vòng **đã bỏ** không còn là mục tiêu của lệnh bỏ ⇒ lệnh bị từ chối bất kể nó là bấm trùng hay bấm mới (nguyên tắc nền điểm 9) |

> Cùng cách này áp cho **chạy lại vòng**: mỗi lần chạy lại là một **lần chạy mới có chủ đích**, không phải bấm trùng — nên nó **không** bị dedup, mà bị chặn bởi dialog hạng phá huỷ và cửa kiểm kho đề (`Đ-31`).

### Concurrency
Không xảy ra — **mỗi contest chỉ có MỘT admin duy nhất**, nên không tồn tại hai luồng thao tác admin đồng thời

### Examples
- **Bỏ Khởi động**: Score trước = 40 (10+10+20 từ KĐ + Tăng tốc), bỏ KĐ → revert −30 (KĐ) = 10 còn lại
- **Chạy lại VCNV**: VCNV đã chạy, pool lần 1 dùng 4 câu, bấm "Chạy lại" → pool chỉ còn N−4 câu, cần ≥4 câu tiếp theo, chạy lại với câu khác
- **Bỏ vòng + SCORE_ADJUST**: Admin bỏ KĐ (revert −30), nhưng trước đó chỉnh +5 → score = −30 + 5 = −25 (chỉnh giữ)
- **Chạy lại vòng bị bỏ**: Bỏ TT (revert điểm), sau đó chạy lại TT (pool phải còn 4 câu) → vòng TT được track 2 lần trong biên bản

### Source traceability
- `docs/game-rules-decisions.md` §6.3 (Đ-5.1 bỏ vòng / chạy lại vòng; Đ-5.2 reset = revert)
- §6.3 (Đ-5.2d — biên bản giữ, vòng nhãn "đã bỏ")
- §6.3 (Đ-5.2f — câu đã dùng không trả lại)
- §6.3 (Đ-5.2e — viewer thấy số đột ngột thay đổi, có chủ đích)
- `docs/reviews/game-rules-review.md` GRR-155 (bỏ Về đích → tie-break mất tiền đề), GRR-160 (bỏ vòng bấm 2 lần), `game-rules-review.md` GRR-161 (chấm câu của vòng đã bỏ)

---

## GR-031 — Rút đề và no-repeat toàn contest

### Status

CONFIRMED

### Purpose
Định nghĩa cơ chế rút ngẫu nhiên câu hỏi từ pool, và quy tắc không lặp lại câu trong toàn bộ contest (qua nhiều match).

### Actors
- Admin (chọn đề trước start, phát động draw)
- Server (rút câu, kiểm tra no-repeat, emit event)
- Setter (cấu hình câu hỏi, đáp án)

### Related states
- Match state: `LOBBY` (pre-flight) → `rounds[i]` (draw tại đầu mỗi turn)
- MatchEvent: `QUESTIONS_DRAWN` (ghi câu rút để replay)
- Question cờ: `usedInContest` (không xoá, chỉ set true)

### Trigger
Admin bấm "start trận" (LOBBY → vòng đầu) hoặc đầu mỗi turn khi cần rút câu

### Preconditions
- Contest đã gán kho đề (`questionSetId`)
- Admin đã chọn danh sách câu trước start (full-text search + filter + sort trên kho, snapshot vào match config)
- **Người tạo contest PHẢI chọn danh sách câu hỏi trước khi start**, hệ thống KHÔNG tự lấy
- Pre-flight check: pool còn lại ≥ worst-case cần (số lượt × số câu/lượt × `reservePerField` default 2)

### Inputs
- DrawConfig: `{mode: 'draw', slots: [{field, count}, ...], noRepeatInMatch: true}`
- `reservePerField`: mặc định 2 (worst-case reserve)
- Danh sách câu đã gán (snapshot)
- Cờ `usedInContest` từ các match trước (global scope)

### Conditions
- **No-repeat toàn contest**: Câu đã hỏi (qua bất kỳ match nào) → cờ `usedInContest = true` → loại khỏi pool
- **No-repeat chỉ trong danh sách gán**: Server rút **TRONG danh sách đã gán** (snapshot), loại câu `usedInContest`
- **⚠ Trận `practice` trong một contest THẬT thì phép lọc ĐẢO CHIỀU** (`Đ-57`, chốt 27/07): pool của nó = **CHỈ những câu `usedInContest = true`** — tức chỉ câu **đã lộ** ở các trận thật trước đó. Hệ quả: trận practice **không tiêu thêm câu nào** (mọi câu nó chạm đã tiêu rồi) và **không thể nhìn thấy đề chưa thi** ⇒ **không dùng contest thật để tổng duyệt trước trận được**. Contest chưa chạy trận nào thì pool này **rỗng** và pre-flight tự chặn — đó là dạng chính xác của *"contest thật phải chạy ≥1 lần mới practice được"*. Trong một **practice contest** thì không có phép đảo nào: mọi trận ở đó lọc bình thường, trong phạm vi `usedInContest` của riêng nó
- **Bỏ qua vẫn tiêu câu**, nhưng không gây cạn giữa vòng: câu bị bỏ qua nằm trong đúng con số cố định của vòng (`Đ-30`), và con số đó đã được kiểm đủ tại cửa vào vòng
- **Cấu hình 2 bước** (Đ-5 advisory): Algorithm pre-flight 2 bước + `reservePerField`

### Decision table

| Case | Conditions | Expected outcome | State change | Error |
|---|---|---|---|---|
| C1 — Start match, pool đủ | Lần 1 rút 4 câu từ pool 200, no-repeat ON | Server select random 4 từ 200 | Event: `QUESTIONS_DRAWN{qids: […]}` · Set `usedInContest = true` cho 4 câu · Điểm snapshot vào match config | — |
| C2 — Match 2, no-repeat | Pool 200, câu từ match 1 = 4 câu = `usedInContest`, rút lại | Pool effective = 196 (loại 4 câu cũ), rút 4 từ 196 | Event ghi câu mới · Set cờ mới | — |
| C3 — Pool cạn (pre-flight theo vòng) | Vòng cần 4 câu, kho còn 3 | **Không bắt đầu được vòng đó**; **các vòng khác vẫn bắt đầu bình thường** | Vòng không mở; admin thấy báo thiếu bao nhiêu câu | — |
| C4 — Pool cạn giữa vòng | Đã bắt đầu vòng rồi mới thiếu câu | **KHÔNG TỒN TẠI** — nhu cầu của vòng là con số cố định (`Đ-30`) và được kiểm đủ tại cửa vào vòng; câu bị bỏ qua vẫn nằm trong con số đó | Không đổi | — |
| C5 — Replay event log | Query: câu nào được rút ở lượt 1 match X? | Tra event `QUESTIONS_DRAWN` → danh sách qid | Read-only, không sinh event mới | — |
| C6 — Admin sửa danh sách gán tại cửa vào vòng | Ở `LOBBY` (cửa vào vòng), admin thêm / bớt câu trong danh sách đã gán | **ĐƯỢC** (`Đ-37`) — đây là lối thoát cho ngưỡng chặn cứng của `Đ-31`. Pre-flight chạy lại sau khi sửa | Danh sách gán cập nhật · vào AuditLog · **cờ `usedInContest` KHÔNG đụng tới** | — |
| C7 — Bớt một câu **đã hiển thị** | Câu đã lên màn thí sinh, admin gỡ khỏi danh sách gán | **KHÔNG TỒN TẠI** — câu đã tiêu (`GRR-085`) không còn là mục tiêu của thao tác gỡ; máy admin hiện **toast**, không ép được (`Đ-16`). Nếu cho gỡ thì gỡ-rồi-thêm-lại thành đường lách no-repeat | Không đổi | — |
| C8 — Bớt một câu **đã rút, chưa hiển thị** | Câu đã rút ở đầu lượt nhưng admin chưa bấm hiển thị | **ĐƯỢC** — theo `GRR-118` câu này **chưa tiêu, trả lại kho** | Câu rời danh sách gán và quay về kho | — |
| C9 — Sửa danh sách **giữa lúc một vòng đang chạy** | Vòng đã mở, đang hỏi câu | **KHÔNG** (`Đ-37`) — cửa sửa chỉ mở ở `LOBBY` — cửa vào vòng. Trong vòng không có nhu cầu: số câu của vòng là con số cố định (`Đ-30`) đã kiểm đủ tại cửa vào | Không đổi | — |

### Outcomes
- **Câu rút được ghi dấu** `usedInContest = true` (mềm, không phải xoá)
- **Event `QUESTIONS_DRAWN`** ghi rõ qid + timestamp (để replay)
- **Pool effective giảm** (loại câu `usedInContest`)
- **Câu skip cũng tiêu pool** (Đ-5.2f): **`"đã dùng"` = ĐÃ HIỂN THỊ cho thí sinh**, không phải *"đã chấm"* (`GRR-085`) ⇒ câu bị bỏ qua sau khi đã hiện vẫn set cờ `usedInContest`. Ngược lại, câu **đã rút nhưng CHƯA hiển thị** là **chưa tiêu, trả lại kho** (`GRR-118`). Đóng `U-30`

### State changes
- MatchEvent: +1 `QUESTIONS_DRAWN` event (per turn rút)
- Question.usedInContest: += (bộ câu rút lần này)
- Match config: snapshot câu gán
- Pool: effective giảm (câu bị loại không cần xoá DB)

### No-change guarantees
- **Cờ `usedInContest` KHÔNG reset** khi bỏ vòng / chạy lại (Đ-5.2f): mỗi câu rút = 1 lần, vĩnh viễn tiêu khỏi pool
- **Event `QUESTIONS_DRAWN` không thay đổi** (append-only): dùng để replay trận
- **Danh sách gán KHÔNG bị sửa TRONG LÚC MỘT VÒNG ĐANG CHẠY.** Trong suốt một vòng, tập câu khả dụng là **bất biến** ⇒ replay event log ra đúng kết quả cũ (`Đ-5.3`).

> ⚠️ **Sửa 2026-07-27 (`Đ-37`)** — bản trước ghi *"Danh sách gán (snapshot) KHÔNG bị sửa **giữa trận**"*. Phát biểu đó là **tàn dư** từ thời pre-flight chạy **một lần trước trận**, và nó mâu thuẫn với hai chỗ khác đã có: `Đ-31` (*"admin thấy được thiếu bao nhiêu câu để **bổ sung rồi mở lại**"*) và **GR-025 Error outcomes** (*"**bổ sung đề rồi mở lại**"*).
>
> Phạm vi đúng là **VÒNG, không phải TRẬN**: sửa được ở `LOBBY` — cửa vào vòng (xem C6), không sửa được khi một vòng đang chạy (C9). Đây cũng là **lối thoát bắt buộc** cho `Đ-31` — một ngưỡng **chặn cứng không ép được** mà không có đường xử lý thì vòng thiếu đề sẽ mất hẳn.

### Error outcomes
- **Pool cạn trước start** (pre-flight chặn): Người tạo contest chưa gán đủ câu hoặc contest dùng pool chia sẻ quá tải
- **Pool cạn giữa vòng**: không tồn tại — pre-flight chạy tại cửa vào **từng vòng**
- **Câu skip**: `"đã dùng"` = **đã hiển thị** (`GRR-085`); rút mà chưa hiện thì trả lại kho (`GRR-118`). Ranh giới trùng đúng mốc `Đ-26` (*admin bấm hiển thị câu hỏi*) — cùng một mốc phục vụ cả ba rule. Đóng `U-30`

### Evaluation order
1. Admin chọn câu hỏi trước start (kho đề → full-text search → filter → sort → snapshot vào match config) ← **PHẢI làm trước khi start** (R-KD-07)
2. Pre-flight (1 bước): kiểm tra worst-case pool ≥ ngưỡng (num_turns × questions_per_turn × reservePerField)
3. Admin bấm "start trận"
4. Đầu mỗi turn / vòng khi cần câu:
   - Query pool: câu trong snapshot, loại `usedInContest = true`
   - Random select N câu
   - Emit event `QUESTIONS_DRAWN`
   - Set cờ `usedInContest = true`
   - Hiển thị câu cho thí sinh

### Boundaries
- **Câu tối thiểu / turn**: ≥ 1
- **Pool worst-case**: `num_turns × max_questions_per_turn × reservePerField` (default reserve = 2)
- **Phạm vi no-repeat**: Toàn contest (global `usedInContest` flag)

### Idempotency
- Mỗi call "rút câu" (draw) → **sinh 1 `QUESTIONS_DRAWN` event**; nếu admin bấm draw lại (lag), lần 2 = **event riêng**, **không dedup**. Điều này **an toàn** nhờ `GRR-118`: câu của lần rút thứ nhất **chưa hiển thị** ⇒ **chưa tiêu, trả lại kho** ⇒ bấm trùng không làm hao pool. Chống bấm trùng ngoài ý muốn vẫn nằm ở `Đ-29` (nút một chiều tự tắt)
- Event log ghi hết → replay được thứ tự câu

### Concurrency
Không có hai luồng điều khiển đồng thời (**một admin duy nhất mỗi contest**), nên hai lượt không thể cùng rút đề một lúc

### Examples
- **Contest 1 vòng Khởi động**: Pool 100 câu, admin chọn 60 câu (snapshot), start → pre-flight: 1 vòng × 1 lượt × 6 câu/TS × 4 TS = 24 cần, còn 60 ≥ 24 OK → start
  - Lượt 1: draw 6 câu cho TS A → event ghi → set cờ 6 câu
  - Pool effective sau = 54 (100 − 6)
- **Contest 2 dùng pool chung**: Pool 100, match 1 dùng 60 gán, match 2 dùng 60 gán (snapshot khác), match 1 rút 4 → `usedInContest` = 4, match 2 rút 4 từ 56 (loại 4 cũ)
- **Không cạn được giữa vòng**: vòng Khởi động lượt riêng cần đúng 6 câu × 4 thí sinh = 24 câu, kiểm đủ tại cửa vào vòng; câu bị bỏ qua nằm trong 24 câu đó, nên đã mở được vòng thì chạy trọn vòng

### Source traceability
- `docs/source/fandom-olympia-26-luat-choi.md` — không đề cập draw (repo mở rộng)
- **Sửa danh sách gán tại cửa vào vòng**: ✅ chủ dự án chốt **2026-07-27** — `docs/reviews/game-rules-decisions.md` §11.22 `Đ-37`; hệ quả cấp sản phẩm ở `docs/product-discovery.md` **C-18**
- `docs/game-rules-inventory.md` §R-KD-07 (rút đề, emit `QUESTIONS_DRAWN`) · §R-GEN-06 (no-repeat toàn contest)
- `docs/game-rules-decisions.md` §6.3 (Đ-5.2f — câu đã dùng không trả lại)
- `plans/260711-2340-olympia-contest-system/DEFERED.md` D22 (draw, no-repeat, snapshot danh sách)

## GR-032 — Hàng đợi tín hiệu và xác nhận của admin

### Status

CONFIRMED

### Purpose
Tất cả tín hiệu từ thí sinh phải vào hàng đợi theo thứ tự thời gian server, không có cơ chế bỏ tín hiệu. Admin lựa chọn duyệt hoặc từ chối từng tín hiệu tùy vòng. Việc reject một tín hiệu không làm thí sinh mất lượt của mình.

### Actors
- **Thí sinh** — phát tín hiệu (bấm chuông, chọn hàng ngang, gởi đáp án)
- **Admin** — xác nhận hoặc reject tín hiệu
- **Server** — ghi nhận tín hiệu, quản lý hàng đợi

### Related states
- Vòng đang chạy (`rounds[i]`)
- Trạng thái hàng đợi: hoạt động (reset sau mỗi vòng) hoặc bị xoá
- Tín hiệu: chờ duyệt, được duyệt (Yes), bị từ chối (No)

### Trigger
- Thí sinh bấm chuông ở vòng Khởi động lượt chung, VCNV, Về đích, hoặc Câu hỏi phụ
- Thí sinh chọn hàng ngang ở VCNV
- Thí sinh bấm nút "Mở chướng ngại vật" ở VCNV
- Thí sinh gửi đáp án ở bất kỳ vòng nào

### Preconditions
- Vòng phải đang chạy (trạng thái trận là `rounds[i]`)
- Thí sinh là thành viên tham gia trận hoặc thi đội (cùng match)
- Ghế của thí sinh còn hoạt động (chưa bị dropout hoặc loại)

### Inputs
- `signal`: loại tín hiệu (buzz, select_row, open_obstacle, submit_answer)
- ghế phát tín hiệu
- `timestamp`: server timestamp khi nhận (millisecond precision)
- `payload`: chi tiết tín hiệu (câu hỏi ID, hàng ngang, dữ liệu đáp án)

### Conditions
**Điều kiện để tín hiệu vào hàng đợi:**
- Server timestamp phải nằm trong cửa sổ hợp lệ của sự kiện (vd: trong 3 giây cửa sổ chuông)
- Không kiểm tra quyền hợp lệ ở hàng đợi — tất cả tín hiệu nhận vào cùng một cơ chế

**Điều kiện để chặn ở VCNV (queue chỉ chặn ở vòng này):**
- Tín hiệu chọn hàng ngang và "Mở chướng ngại vật" phải chờ admin Yes/No
- Tín hiệu khác (submit hàng ngang) không chặn — xử lý ngay

**Điều kiện để không chặn ở vòng khác:**
- Khởi động lượt chung, Về đích cướp quyền: có chuông là tính ngay theo timestamp
- Câu hỏi phụ: xử lý thứ tự sau khi hết 15 giây

### Decision table

| Case | Conditions | Expected outcome | State change | Error |
|---|---|---|---|---|
| C1 — Tín hiệu hợp lệ ở VCNV (chọn hàng) | Trong cửa sổ; queue chặn | Vào queue chờ duyệt | Trạng thái: `pending_review` | — |
| C2 — Admin bấm Yes | Tín hiệu ở queue, admin xác nhận | Tín hiệu được thực thi; tín hiệu kế lên | Trạng thái: `approved`, tín hiệu này thực thi | — |
| C3 — Admin bấm No | Tín hiệu ở queue, admin reject | Tín hiệu bị bỏ qua; thí sinh không mất lượt; tín hiệu kế lên | Trạng thái: `rejected` | — |
| C4 — Tín hiệu ở vòng không chặn (Khởi động chung) | Cùng cửa sổ, queue không chặn | Tính ngay theo timestamp; không chờ duyệt | Trạng thái: `executed` | — |
| C5 — Kết thúc vòng (reset queue) | Vòng chuyển sang vòng kế | Queue được làm trống; lịch sử giữ nguyên | Queue state: `reset`, history: `append-only` | — |
| C6 — Reject không làm mất lượt | Admin bấm No; thí sinh chưa dùng lượt riêng | Thí sinh vẫn có lượt riêng; tín hiệu kế từ thí sinh khác được duyệt | Thí sinh state: không đổi | — |
| C7 — Tín hiệu từ ghế đã bị loại VCNV | Thí sinh bị loại; vẫn thao tác trên máy | Máy thí sinh **không hiển thị gì**, bấm **không phản hồi** ⇒ không có tín hiệu nào được tạo, **không vào hàng đợi** | Không đổi | — |
| C8 — Lịch sử tín hiệu không xoá | Sau khi reject, vòng tiếp theo | Tín hiệu đã reject vẫn trong log; admin xem lại được | History: `append-only, never deleted` | — |

### Outcomes
- **Thực thi**: tín hiệu được chấp nhận và xử lý theo luật vòng tương ứng
- **Reject**: admin bấm No; tín hiệu bị loại bỏ; thí sinh không mất lượt
- **Reset**: khi kết thúc vòng, hàng đợi trống nhưng lịch sử lưu giữ

### State changes
- `queue.status`: `empty` → `pending_review` (khi có tín hiệu vào queue chặn)
- `queue.status`: `pending_review` → `empty` (khi vòng kết thúc)
- `signal.status`: `received` → `pending_review` → `approved` hoặc `rejected`
- Vòng chuyển → queue reset; lịch sử tín hiệu giữ nguyên (append-only)

### No-change guarantees
- **LỊCH SỬ tín hiệu KHÔNG BAO GIỜ xoá**: append-only log, để admin xem lại và gỡ lệnh cấm nếu cần (từ `game-rules-decisions.md` Đ-7)
- **Reject KHÔNG làm thí sinh mất lượt**: thí sinh vẫn giữ quyền lượt riêng hoặc lượt chọn hàng ngang (từ `game-rules-decisions.md` Đ-7)
- **Không có drop**: tất cả tín hiệu phải có outcome (loại bỏ khái niệm drop hoàn toàn)

### Error outcomes
- Tín hiệu từ ghế không còn quyền (đã loại, đã rớt quá grace): máy thí sinh **không hiển thị gì**, bấm **không phản hồi** ⇒ không có tín hiệu nào được tạo và không vào hàng đợi (đóng `game-rules-review.md` GRR-141, GRR-146)
- Tín hiệu chuông trùng của cùng một ghế: KHÔNG XẢY RA — nút chuông tự khoá ngay khi bấm ở frontend

### Evaluation order
1. Server nhận tín hiệu → gắn timestamp
2. Kiểm tra tín hiệu hợp lệ (trong cửa sổ thời gian)
3. Vào hàng đợi
4. Nếu queue chặn (VCNV): chờ admin Yes/No
5. Nếu queue không chặn (Khởi động chung, cướp quyền): tính ngay theo timestamp
6. Kết thúc vòng: queue reset; lịch sử giữ

### Boundaries
- **Khoa thời gian 3 giây** — Khởi động lượt chung sau khi admin start timer
- **Khoa thời gian 5 giây** — Về đích cướp quyền sau khi admin bấm Sai
- **Khoa thời gian 15 giây** — Câu hỏi phụ; xử lý tín hiệu sau khoa đóng
- **Hàng đợi chặn** — Chỉ áp ở VCNV (chọn hàng ngang + "Mở chướng ngại vật"); 2 vòng khác không chặn
- **Queue reset** — Sau mỗi vòng kết thúc, trước vòng kế tiếp

### Idempotency
**Tín hiệu chuông trùng của cùng một ghế**: KHÔNG XẢY RA — nút chuông **tự khoá ngay khi bấm** ở frontend, trước khi gửi. Hàng đợi không bao giờ nhận hai tín hiệu chuông của cùng một ghế cho cùng một câu.

> Server vẫn phải bỏ qua tín hiệu trùng nếu nhận được (`CLAUDE.md` §Zero-trust) — khoá giao diện không thay thế kiểm tra ở server.

**Reject idempotent**: Admin bấm No cho tín hiệu X → tín hiệu X được đánh dấu rejected, không phát sinh reject lần nữa

### Concurrency
**Tín hiệu cùng timestamp**: quy tắc riêng từng vòng —
- VCNV (queue chặn): không có xung đột vì admin duyệt tuần tự
- Khởi động chung, cướp quyền: hai tín hiệu cùng ms → **hàng đợi tự quyết định, ngẫu nhiên**; thứ tự được ghi lại nên dựng lại được

**Hai loại tín hiệu trong queue chặn (VCNV)**: chọn hàng ngang và "Mở chướng ngại vật" dùng **chung một hàng đợi**, xử lý **FIFO thuần theo server timestamp** — **KHÔNG** ưu tiên theo loại (`game-rules-review.md` GRR-144).

- Căn cứ: nguyên tắc nền điểm 3 — *"mọi tín hiệu của thí sinh đều vào hàng đợi theo server timestamp"*. Không có mệnh đề nào cho phép sắp xếp lại theo loại, và thêm quy tắc ưu tiên sẽ tạo một tiêu chí mà thí sinh không quan sát được.
- Hai tín hiệu **cùng mốc**: hàng đợi tự quyết **ngẫu nhiên** (điểm 15), như mọi chỗ khác.
- **Băng điểm Chướng ngại vật không bị ảnh hưởng** bởi lựa chọn này: queue **chặn** ở VCNV nên tín hiệu chọn hàng ngang đang chờ duyệt chưa làm tăng số hàng đã mở; băng chốt tại **mốc admin xác nhận** (`Đ-7.c`). Xem GR-009 C7.

**Vòng đời tín hiệu** (`Đ-7.b2`): *tín hiệu gắn với **ĐÍCH** của nó và vô hiệu khi đích đóng* — chọn hàng ngang gắn với **lượt chọn**, trả lời gắn với **câu**, "Mở chướng ngại vật" gắn với **vòng**. Việc hàng đợi đang hoạt động được đặt lại theo vòng/câu là **hệ quả** của quy tắc này, không phải quy tắc riêng. **Lịch sử tín hiệu không bao giờ bị xoá** (điểm 3).

### Examples

**Ví dụ 1 — Happy path (VCNV chọn hàng)**:
- t=0s: Thí sinh A bấm chọn hàng ngang 1 → queue chặn
- t=0.2s: Admin thấy tín hiệu → hiện dialog Yes/No
- t=0.5s: Admin bấm Yes → hàng 1 được hỏi, tín hiệu kế lên (nếu có)

**Ví dụ 2 — Reject (thí sinh bấm nhầm)**:
- t=0s: Thí sinh B bấm "Mở chướng ngại vật" → queue chặn
- t=0.3s: Admin thấy tín hiệu nhưng thấy nhầm → bấm No
- Kết quả: Tín hiệu reject; B không mất lượt; tín hiệu tiếp (C hoặc D) lên

**Ví dụ 3 — Không chặn (Khởi động lượt chung)**:
- t=3.2s (từ start timer): Thí sinh A bấm chuông
- t=3.5s: Thí sinh B bấm chuông
- Kết quả: A được quyền (timestamp 3.2s); B vào queue nhưng không chặn, xử lý ngoài điều khiển admin

**Ví dụ 4 — Boundary (hết khoa chuông)**:
- Khoa chuông Khởi động chung = 3 giây từ lúc admin start timer
- t=3.0s: Thí sinh C bấm chuông → hợp lệ (đúng biên)
- t=3.01s: Thí sinh D bấm chuông → cửa sổ đã đóng, máy thí sinh **không hiển thị gì và không phản hồi** ⇒ không có tín hiệu nào được tạo

**Ví dụ 5 — Append-only (xem lại lịch sử sau reject)**:
- A bấm, admin reject
- Vòng kết thúc, queue reset
- Admin nhìn lịch sử: tín hiệu của A vẫn thấy (status: rejected)
- → Admin quyết có gỡ lệnh cấm không

### Source traceability
- `game-rules-decisions.md` §5.1 `Đ-7` — Hàng đợi tín hiệu
- `game-rules-decisions.md` §5.2 `Đ-7.2` — Queue chỉ chặn ở VCNV
- `game-rules-decisions.md` §5.1 `Đ-7` — Reject không mất lượt
- `CLAUDE.md` §UX BẮT BUỘC — Hàng đợi theo thứ tự tới, KHÔNG drop
- `CLAUDE.md` §UX BẮT BUỘC — LỊCH SỬ tín hiệu không xoá

---

## GR-033 — Mốc thời gian do admin bấm

### Status

CONFIRMED

### Purpose
Mọi thời điểm quan trọng trong trận mà luật gốc mô tả bằng hành vi của MC đều ánh xạ thành một thao tác bấm của admin. Admin là cảm biến của hệ thống; mốc admin là tuyệt đối không có ân hạn.

### Actors
- **Admin** — bấm start timer, bấm hiển thị câu, bấm mốc khác
- **MC** — nói trên sân khấu; admin nghe và bấm
- **Server** — ghi nhận timestamp; không ai sửa được

### Related states
- Timer: `running` / `stopped` — đồng hồ **không bao giờ đóng băng** (`Đ-21`)
- Câu hỏi: `hidden` / `displayed`
- NSHV: `open` / `closed` (mốc đóng = admin bấm hiển thị)

### Trigger
- MC đọc xong câu hỏi → **Admin bấm start timer**
- MC đưa ra "hiệu lệnh" ở Câu hỏi phụ → **Admin bấm** (admin là người nghe)
- Câu hỏi được đọc lên hoặc hiện lên màn hình → **Admin bấm hiển thị câu** (đóng cửa sổ NSHV)

### Preconditions
- Trận đang chạy vòng tương ứng
- Admin phải có quyền thao tác

### Inputs
- `action`: loại mốc (start_timer, display_question, other_milestone)
- `timestamp`: server timestamp khi bấm

### Conditions
**Mốc start timer**:
- Thường sau khi MC đọc xong câu hỏi
- Kích hoạt countdown từ `timeSeconds` của câu

**Mốc hiển thị câu**:
- Đóng cửa sổ NSHV (không cho phép đặt NSHV sau mốc này)
- Khởi động cửa sổ trả lời (ở mode sân khấu: hiển thị câu; ở mode nhập liệu: bật ô nhập)

**Mốc MC hiệu lệnh** (Câu hỏi phụ):
- Admin bấm khi MC nói "bắt đầu" hoặc "giải đáp"

### Decision table

| Case | Conditions | Expected outcome | State change | Error |
|---|---|---|---|---|
| C1 — Start timer lần đầu | MC đọc xong; admin bấm | Timer khởi động từ `timeSeconds` của câu | Timer state: `running`; mốc ghi nhận | — |
| C2 — Start timer lần hai (bấm nhầm) | Admin bấm lại trong 100ms | **KHÔNG XẢY RA** — nút start timer tự khoá ngay sau lần bấm đầu | Không đổi | — |
| C3 — Hiển thị câu hỏi | Trước lượt trả lời; admin bấm | Câu hiện; cửa sổ NSHV đóng | Question state: `displayed`; NSHV state: `closed` | — |
| C4 — Hiển thị câu trước start timer | Admin bấm hiển thị trước start timer | Câu hiện; cửa sổ NSHV đóng | Question state: `displayed`; Timer vẫn chưa chạy | — |
| C5 — Mốc MC hiệu lệnh (Câu hỏi phụ) | Sau khi MC nói "bắt đầu"; admin bấm | Tín hiệu từ thí sinh bắt đầu có hiệu lực (chưa bấm là bị cấm) | Timer state: `running` (hoặc cấm lệnh đóng) | — |
| C6 — Mốc tuyệt đối (không ân hạn) | Admin bấm; có lag mạng | Server timestamp là thời điểm **tuyệt đối**, không có grace thêm | Timestamp: exactly_when_clicked | — |

### Outcomes
- Timer khởi động hoặc dừng lại ở timestamp server (không bao giờ sửa sau)
- Cửa sổ NSHV đóng (nếu là mốc hiển thị)
- Tín hiệu thí sinh có hiệu lực hoặc bị cấm tùy mốc

### State changes
- `timer.state`: từ `stopped` → `running` (start timer)
- `question.state`: từ `hidden` → `displayed` (display question)
- `nshv.state`: từ `open` → `closed` (không cho phép đặt NSHV sau mốc này)

### No-change guarantees
- **Mốc admin là TUYỆT ĐỐI**: không có cửa sổ ân hạn, không trừ bù độ trễ tay người (từ `game-rules-decisions.md` Đ-6.3)
- **Lịch sử giữ đầy đủ để gỡ cấm**: admin xem lại và quyết gỡ bằng cơ chế riêng, không phải sửa mốc

### Error outcomes
- Bấm trùng nút start timer: không xảy ra — nút tự khoá sau lần bấm đầu
- Hai mốc là **hai thao tác tách rời, theo thứ tự cố định**: hiển thị câu hỏi **trước**, start timer **sau**. Cửa sổ chuông mở tại mốc thứ nhất

### Evaluation order
1. Admin bấm → server ghi nhận timestamp
2. Kiểm tra loại mốc (start timer, display, hiệu lệnh)
3. Áp dụng hệ quả của mốc (timer chạy, câu hiện, lệnh cấm đóng mở)
4. Không có cảnh báo hay grace; mốc được chốt ngay

### Boundaries
- **3 giây** — Cửa sổ chuông Khởi động lượt chung từ mốc "MC đọc xong" đến hết 3 giây
- **5 giây** — Cửa sổ cướp quyền Về đích từ mốc "admin bấm Sai" đến 5 giây
- **15 giây** — Câu Câu hỏi phụ từ mốc "hiệu lệnh" đến hết 15 giây
- **Không có grace**: timestamp server là quyết định cuối cùng; không cộng thêm độ trễ tay người

### Idempotency
**Nút start timer tự khoá ngay sau lần bấm đầu** để chống bấm trùng ⇒ không tồn tại lần bấm thứ hai, độ dài cửa sổ không thể bị kéo dài do thao tác lặp.

> Đây là **ngoại lệ duy nhất** của quy tắc "đồng hồ không khoá thao tác của admin": khoá này nhằm chống bấm trùng, không phải chống lệch luật.

**Mốc hiển thị câu hỏi bấm hai lần**: giữ nguyên; chỉ lần đầu có hiệu lực.

### Concurrency
**Hai mốc khác nhau (hiển thị + start timer)**: là **hai thao tác riêng, thứ tự cố định** — hiển thị trước, start timer sau. Không đảo được, không gộp được.

- **Hiển thị câu hỏi**: đưa câu lên màn thí sinh và viewer; **mở cửa sổ chuông**; đóng cửa sổ đặt Ngôi sao hy vọng (`game-rules-review-old.md` GRR-048).
- **Start timer**: mốc *"MC đọc xong"*; bắt đầu đếm thời gian suy nghĩ.
- Khoảng giữa hai mốc chính là lúc MC đọc — quãng mà luật cho phép bấm chuông ở Khởi động lượt chung.

**Hai admin bấm cùng mốc**: KHÔNG XẢY RA — mỗi contest chỉ có một admin duy nhất

### Examples

**Ví dụ 1 — Happy path (start timer)**:
- MC đọc xong câu "Ai là tác giả Truyện Kiều?"
- Admin bấm "Start timer" ở t=5.234s (server time)
- Timer Khởi động chạy từ `timeSeconds` của câu (vd 3 giây ở Khởi động riêng)
- Hết giây thứ 3 → câu đóng, không ai bấm được

**Ví dụ 2 — Hiển thị câu trước start**:
- Admin bấm "Hiển thị" ở t=2.1s
- Câu hiện trên màn hình; cửa sổ NSHV đóng
- Lúc t=2.9s: Thí sinh bấm NSHV → bị chặn (cửa sổ đã đóng)
- Lúc t=3.0s: Admin bấm "Start timer"
- Cửa sổ chuông Khởi động chung = (khoảng MC đang đọc từ 2.1 đến 3.0) + 3s = 4 giây → sai luật

**Ví dụ 3 — Câu hỏi phụ hiệu lệnh**:
- MC nói "Bắt đầu! Thí sinh bấm chuông"
- Admin bấm "Hiệu lệnh" ở t=10.5s
- Thí sinh A bấm ở t=10.2s → so với mốc bằng server timestamp; **đúng mốc vẫn tính** (biên đóng, `Đ-28`).

**Ví dụ 4 — Tuyệt đối không ân hạn**:
- t=0: Admin bấm Start timer nhưng bấm nhầm
- t=0.1s: Admin thấy lag, bấm lại
- Kết quả: đồng hồ chạy từ lần bấm đầu; **không có lần bấm thứ hai** — nút start timer tự khoá ngay sau lần bấm đầu (`Đ-20`).

**Ví dụ 5 — Boundary 3 giây Khởi động**:
- Admin bấm Start timer ở t=10.0s (MC đọc xong)
- Cửa sổ chuông Khởi động chung = t=10.0 đến t=13.0s (3 giây)
- t=13.0s: thí sinh bấm chuông đúng mốc hết giờ → **vẫn hợp lệ** (biên đóng, `Đ-28`).

### Source traceability
- `game-rules-decisions.md` §8.1 `Đ-6` — Admin bấm start timer = "MC đọc xong"
- `game-rules-decisions.md` §8.1 `Đ-6.1` — "Hiệu lệnh MC" = admin bấm
- `game-rules-decisions.md` §8.2 `Đ-6.3` — Mốc admin tuyệt đối; không ân hạn
- `game-rules-decisions.md` §9.4 `[`game-rules-review-old.md` GRR-048]` — Cửa sổ NSHV đóng khi admin bấm hiển thị
- `CLAUDE.md` — Admin là cảm biến; server time là quyết định cuối cùng

---

## GR-034 — Chuông chỉ nhận click chuột

### Status

CONFIRMED

### Purpose
Nút bấm chuông (để giành quyền trả lời) chỉ nhận tín hiệu từ click chuột. Không gán phím bàn phím (hotkey) cho chuông. Mục đích: tránh bấm nhầm khi thí sinh đang gõ đáp án ở các vòng khác.

### Actors
- **Thí sinh** — dùng chuột click nút chuông
- **UI/Client** — xử lý sự kiện click; không xử lý hotkey cho chuông
- **Server** — nhận timestamp click, xác nhận

### Related states
- Vòng: Khởi động lượt chung, VCNV, Về đích, Câu hỏi phụ (các vòng có chuông)
- Nút chuông: `enabled` / `disabled` (nếu bị khoá theo luật chơi)

### Trigger
- Thí sinh click chuột lên nút "Bấm chuông" (vd nút hình chuông, text "BUZZ", hoặc tương tự)

### Preconditions
- Vòng đang chạy (cửa sổ chuông mở)
- Nút chuông không bị disable theo luật chơi (vd: không bị trừ điểm trước, chưa tới lượt ở VCNV)

### Inputs
- `eventType`: "click" (click chuột; không phải keydown/keyup)
- `timestamp`: server timestamp

### Conditions
**Điều kiện nhận tín hiệu chuông**:
- Sự kiện phải từ click chuột (không từ bàn phím)
- Trong cửa sổ hợp lệ của vòng (vd 3 giây Khởi động chung)

**Điều kiện bị bỏ qua**:
- Hotkey của chuông (nếu có) → không xử lý
- Event từ bàn phím (vd spacebar, Enter, số phím) → loại bỏ

### Decision table

| Case | Conditions | Expected outcome | State change | Error |
|---|---|---|---|---|
| C1 — Click chuột vào nút | Vòng Khởi động chung; thí sinh click → | Gửi tín hiệu chuông; vào hàng đợi | Queue: tín hiệu nhận vào | — |
| C2 — Click chuột VCNV "Mở chướng ngại vật" | Vòng VCNV; thí sinh click nút → | Gửi tín hiệu; queue chặn đợi admin duyệt | Queue: `pending_review` | — |
| C3 — Hotkey (vd spacebar) cho chuông | Phím bấm → | **BỊ BỎ QUA** — không gửi tín hiệu | Không thay đổi | — |
| C4 — Hotkey khác (Enter gửi đáp án) | Thí sinh gõ đáp án, bấm Enter → | Gửi đáp án, **KHÔNG** gửi tín hiệu chuông | Đáp án: submitted; chuông: không bấm | — |
| C5 — Nút bị disable theo luật chơi | Thí sinh bấm sai; bị trừ điểm; nút khoá → | Nút bị khoá theo luật chơi nên không phát tín hiệu | Nút ở trạng thái khoá; không có tín hiệu nào được tạo | — |
| C6 — Click nút nhưng hết cửa sổ chuông | Bấm sau khi cửa sổ chuông đã đóng | Máy thí sinh **không hiển thị gì**, bấm **không phản hồi** ⇒ không có tín hiệu nào được tạo | Không đổi | — |

### Outcomes
- **Click hợp lệ**: tín hiệu chuông gửi tới server với timestamp
- **Hotkey**: bỏ qua; không xử lý
- **Nút disable**: click không có hiệu lực; UI hiển thị state disable (vd xám ra, không phản hồi)

### State changes
- Tín hiệu chuông: từ `sent` → `queued` → `approved`/`rejected` (tuỳ vòng)

### No-change guarantees
- **Nút "Mở chướng ngại vật" được xếp là CHUÔNG**: cũng chỉ nhận click, không hotkey (từ `game-rules-decisions.md` Đ-4.3)
- **Hotkey khác giữ nguyên**: Enter gửi đáp án, 1-8 chọn hàng ngang, Esc xoá ô (CLAUDE.md)
- **Khoá theo luật chơi vẫn áp dụng**: nút chuông bị disable nếu trừ sai, NSHV dùng hết, không tới lượt VCNV

### Error outcomes
- Click nhưng hết cửa sổ chuông: nút **không hiển thị và không phản hồi** ⇒ không có tín hiệu nào được tạo (nguyên tắc nền #9)
- UI không hiển thị nút: lỗi UX; không có workaround

### Evaluation order
1. Thí sinh click chuột (mouse event)
2. Check event type = "click" (loại bỏ keydown/keyup)
3. Check nút chuông disabled hay không (theo luật chơi)
4. Nếu enabled: gửi tín hiệu
5. Nếu disabled: bỏ qua; UI hiển thị disable state

### Boundaries
- **Hoàn toàn không support hotkey cho chuông**: 0 phím được gán
- **Các khoa chuông**: 3 giây (Khởi động chung), 5 giây (cướp quyền Về đích), 15 giây (Câu hỏi phụ)

### Idempotency
**Click chuông nhiều lần**: KHÔNG XẢY RA — nút **tự khoá ngay trong lần bấm đầu**, trước khi gửi tín hiệu. Cú click thứ hai không gặp nút đang bật.

**Nút được mở lại** (sang câu mới): click lại được tính bình thường — khoá gắn với **một câu**, không phải cả vòng.

### Concurrency
**Hai thí sinh click chuông cùng ms**: Xử lý theo server timestamp, không có vấn đề đặc biệt ở phía UI

### Examples

**Ví dụ 1 — Happy path (click nút)**:
- Khoa chuông Khởi động chung mở
- Thí sinh A đặt chuột vào nút chuông
- Bấm chuột (click) lúc t=1.5s → tín hiệu gửi, vào queue

**Ví dụ 2 — Hotkey spacebar (bị bỏ qua)**:
- Thí sinh B gõ đáp án, vô tình bấm spacebar
- Spacebar → **KHÔNG xử lý** (không gán hotkey cho chuông)
- Chuông không được bấm; đáp án vẫn gõ bình thường

**Ví dụ 3 — Hotkey Enter (gửi đáp án, không phải chuông)**:
- Thí sinh C gõ xong đáp án
- Bấm Enter → gửi đáp án, **KHÔNG** gửi tín hiệu chuông
- Nếu vòng có chuông (vd Tăng tốc), C vẫn phải click chuột riêng

**Ví dụ 4 — Nút bị disable (bấm sai)**:
- VCNV: Thí sinh D giải CNV sai → loại khỏi vòng
- Nút chuông của D bị disable (UI xám ra)
- D click nút → bỏ qua; không có hiệu ứng

**Ví dụ 5 — Boundary cửa sổ 3 giây**:
- Khoa 3s từ t=0 đến t=3
- Thí sinh E click ở t=3.000s → **tính** (biên đóng, `Đ-28`).
- Thí sinh F click ở t=3.001s → quá khoa

### Source traceability
- `game-rules-inventory.md` §R-GEN-01 — Chuông chỉ nhận click chuột
- `CLAUDE.md` §UX BẮT BUỘC — Nút chuông chỉ click chuột; không hotkey
- `CLAUDE.md` §UX BẮT BUỘC — Nút "Mở chướng ngại vật" cũng là chuông
- `game-rules-decisions.md` §5.3 `Đ-4.3` — "Mở chướng ngại vật" là chuông

---

## GR-035 — Server time là source of truth duy nhất

### Status

CONFIRMED

### Purpose
Tất cả timeout, thứ tự chuông, thứ hạng tốc độ, và mốc thời gian quyết định trong trận đều tính theo đồng hồ server. Client chỉ hiển thị; không ai sửa được server time. Đây là nguyên tắc nền để đảm bảo công bằng và tái lập được lịch sử trận.

### Actors
- **Server** — quản lý thời gian; emit mốc thời gian cho client
- **Client** — hiển thị countdown, nhưng server là quyết định cuối cùng
- **Admin** — không thể sửa server time

### Related states
- Timer: đếm ngược từ `endsAt` (tính từ server time)
- Tín hiệu thí sinh: gắn timestamp server nhận
- Ranking tốc độ: xếp theo `mốc thời gian server nhận được` của server

### Trigger
- Mỗi sự kiện liên quan đến thời gian (timeout, thứ tự chuông, xếp hạng Tăng tốc)

### Preconditions
- Server phải đang chạy đồng bộ thời gian
- Không có sự cố lệch clock quá mức chấp nhận được

### Inputs
- Server timestamp (millisecond precision): khi nhận tín hiệu, khi timeout, khi tính xếp hạng

### Conditions
**Timer timeout**:
- Tính từ `endsAt` (được set bằng `receivedTime + timeSeconds`)
- Khi `thời gian server đã tới mốc hết giờ` → timeout

**Thứ tự chuông**:
- So sánh `receivedTime` của tín hiệu (không phải UI timestamp của client)
- Ai có `receivedTime` sớm nhất → được quyền

**Xếp hạng Tăng tốc**:
- Xếp hạng theo `receivedTime` của bản cuối (last-wins)
- Phạm vi "đồng thời gian" = cùng ms (K-8 theo `DEF` D13.3 và `R26` §7)

### Decision table

| Case | Conditions | Expected outcome | State change | Error |
|---|---|---|---|---|
| C1 — Timer hết giờ | `thời gian server đã tới mốc hết giờ` → | Câu đóng; không nhận submission mới | Đồng hồ: đã hết giờ | — |
| C2 — Hai chuông cùng ms (Khởi động chung) | Thí sinh A và B bấm cùng timestamp → | **Hàng đợi tự quyết định**, ngẫu nhiên; người vào trước giành quyền | Thứ tự hàng đợi được ghi lại (append-only) | — |
| C3 — Tăng tốc: người A gửi t=5.123s, người B gửi t=5.124s | Xếp hạng theo mốc thời gian server nhận được → | A hạng 1, B hạng 2 (nếu cùng đúng) | Xếp hạng: A trên B | — |
| C4 — Tăng tốc: Cùng ms t=5.125s (≥2 người) | Xếp hạng theo mốc thời gian server nhận được → | Cùng hạng (chia cao hơn); nếu 2 người đúng cùng ms → cùng nhận 40 điểm (K-8) | Xếp hạng: chia cùng mức cao hơn | — |
| C5 — Client lag (UI chậm) | Client hiển thị t=4.5s nhưng server đã timeout t=4.0s → | Server quyết; client hiển thị chỉ là gợi ý | Server timestamp: `4.0s`; Client: `4.5s` (chênh lệch, Server thắng) | — |
| C6 — Tín hiệu nhận sau timeout nhưng client chưa thấy timeout | Submission tới t=3.05s; server timeout t=3.0s → | Bỏ qua submission; hệ thống không xử lý | Submission: `rejected (after timeout)` | — |

### Outcomes
- **Timer**: chỉ server time quyết định hết giờ
- **Thứ tự chuông**: timestamp server nhận
- **Ranking**: sorted by `receivedTime` (server side)
- **Client hiển thị**: chỉ reference; không ảnh hưởng logic

### State changes
- Đồng hồ: đang chạy → hết giờ (khi thời gian server tới mốc hết giờ)
- Submission: `received` → `accepted` hoặc `rejected (late)` (phụ thuộc server time)

### No-change guarantees
- **Server time là quyết định cuối cùng và KHÔNG thay đổi**: không sửa lại sau, không ân hạn (từ `game-rules-decisions.md` Đ-6.3 và `CLAUDE.md`)
- **Client không có quyền lực**: client hiển thị là informational; server là executive

### Error outcomes
- Lệch clock (portable Windows LAN, máy không NTP) — `U-14` ĐÓNG: **không ảnh hưởng luật.** Client **chỉ hiển thị**, mọi mốc đều lấy theo server. Riêng phía server, thứ tự và xếp hạng trong một trận phải tính trên **ĐỒNG HỒ ĐƠN ĐIỆU** (monotonic) của tiến trình, **đồng hồ tường chỉ dùng để hiển thị và ghi log** ⇒ đồng hồ hệ thống bị chỉnh giữa trận **không thể** đảo thứ hạng đã ghi. Đóng luôn `GRR-082`
- Submission sau timeout: bỏ qua; không cộng điểm

### Evaluation order
1. Server nhận tín hiệu → ghi nhận `receivedTime`
2. So sánh `receivedTime` với `endsAt`
3. Nếu `receivedTime > endsAt` → reject (after timeout)
4. Nếu hợp lệ → xử lý theo logic vòng; xếp hạng dùng `receivedTime`
5. Client hiển thị countdown (tính `thời gian còn lại tới mốc hết giờ`)

### Boundaries
- **Millisecond precision**: server timestamp được ghi đến ms
- **Thứ tự "đồng thời gian" = cùng ms** (K-8): nếu A gửi 5.123s, B gửi 5.123s → cùng hạng
- **Không có grace**: timeout là tuyệt đối; submission sau `endsAt` = quá hạn

### Idempotency
**Gửi lại submission ở Tăng tốc** (last-wins):
- Submission A gửi ở t=5.100s (đúng)
- Submission A gửi lại ở t=5.500s (sai)
- → Dùng timestamp bản cuối t=5.500s; ranking được tính lại

### Concurrency
**Hai client gửi tín hiệu cùng lúc**: Server so sánh `receivedTime` (không phải "cùng lúc" trong UI); thứ tự xác định bằng timestamp server

**Timeout và tín hiệu tới cùng ms** — **biên ĐÓNG**, tín hiệu thắng:
- Tín hiệu hoặc submission ở t = 3.000s, timeout cũng ở t = 3.000s ⇒ **tính là hợp lệ**.
- Áp cho **cả hai loại**: tín hiệu giành quyền trả lời và submission đáp án.
- Chỉ khi timestamp **vượt quá** mốc mới **mặc định không tính** — bản đó vẫn được **giữ và hiển thị** trên màn admin, tô **đỏ**, **cạnh bản hợp lệ** (nếu có). **Hiển thị ra ngoài** và **chấm điểm** đều là thao tác bấm của admin; hệ thống chỉ đánh dấu, không chặn cứng.

### Examples

**Ví dụ 1 — Timer timeout**:
- Câu Khởi động lượt riêng: `timeSeconds = 3s`
- Admin start timer lúc server t=10.000s → `endsAt = 10.003s`
- t=10.002s: Thí sinh A gửi đáp án → hợp lệ
- t=10.004s: Thí sinh B gửi đáp án → **quá hạn**, giữ lại và tô đỏ cho admin quyết

**Ví dụ 2 — Tăng tốc: Cùng ms**:
- Câu 1 (20s): A gửi t=8.500s (đúng), B gửi t=8.500s (đúng)
- → Cùng rank cao; cả hai +40 điểm

**Ví dụ 3 — Tăng tốc: Last-wins**:
- A gửi t=8.123s (đúng), rồi gửi lại t=8.456s (sai)
- → Dùng bản cuối t=8.456s (sai) → A +0 điểm (không trong top 4)

**Ví dụ 4 — Client lag (UI chậm)**:
- Server hết giờ t=10.003s; client hiển thị t=10.001s (chậm 2ms)
- Thí sinh click gửi khi UI hiển thị t=9.998s → nhưng server đã timeout
- → Bỏ qua; không cộng điểm

**Ví dụ 5 — Boundary ms**:
- Khoa timeout = t=10.000s đến t=10.003s (3s = 3000ms)
- t=10.003s trong khi mốc là 10.000s → **quá mốc**: mặc định không tính, giữ lại và tô đỏ cạnh bản hợp lệ, admin quyết (`Đ-28`, `Đ-32`).

### Source traceability
- `CLAUDE.md` §UX BẮT BUỘC — Server time là source of truth duy nhất
- `CLAUDE.md` §Quy ước khác — Server-authoritative tuyệt đối
- `game-rules-inventory.md` §R-GEN-05 — Server time source of truth
- `game-rules-decisions.md` §8.2 `Đ-6.3` — Mốc admin tuyệt đối; server time quyết định

---

## GR-036 — Mất kết nối và giữ ghế

### Status

CONFIRMED

### Purpose
Thí sinh bị mất kết nối có thời gian grace **120 giây** để kết nối lại và giữ ghế. Trong khoảng grace, trạng thái của ghế được giữ nguyên (state-sync khi quay lại).

**Quá grace, hệ thống CHỈ HIGHLIGHT — ADMIN là người quyết** *(✅ chủ dự án chốt 2026-07-26, đóng `U-13`)*: không có chính sách tự động nào, không tự loại, không tự xoá ghế. Ghế được **tô nổi bật trên màn admin** kèm thời lượng đã mất kết nối; admin chọn **giữ · gia hạn**.

**v1 KHÔNG có kick — thay bằng VÔ HIỆU HOÁ / KÍCH HOẠT LẠI** *(✅ `Đ-52`, chốt 2026-07-27, thay cho `Đ-45a`)*. Trước đó `Đ-45a` cho kick nhưng **chỉ ở `LOBBY`**, vì rời một ghế khỏi trận giữa lúc câu đang mở làm hỏng thứ đang tính dở — xếp hạng Tăng tốc, băng điểm Chướng ngại vật, cửa sổ chấm của `Đ-43`.

Vô hiệu hoá **không rời ai khỏi trận**: ghế vẫn có mặt trong mọi bảng, điểm nguyên vẹn, chỉ mất quyền thao tác — nên **không phép tính nào bị hụt đầu vào**, và ràng buộc `LOBBY` **không còn lý do tồn tại**. Thao tác dùng được **mọi lúc**, kể cả giữa một câu đang mở, và **đảo ngược được**. Xem C2d.

**Vô hiệu hoá là quyết định RIÊNG, không phải hệ quả của mất kết nối**: quá grace **không** tự vô hiệu hoá ghế nào. Hai chuyện độc lập, hai nút khác nhau.

**Ghế quay lại được khôi phục kể cả giữa câu** *(✅ `Đ-45b`, cùng ngày)*: server đẩy đủ dữ kiện để client dựng lại đúng màn đang thi — xem C4b.

> Đây **cùng một mẫu** với `Đ-1` (máy tô khác biệt ký tự, admin chấm) và với `Đ-28` (máy tô đỏ bản quá hạn, admin phán quyết). Nguyên tắc nền điểm 1 áp nguyên: **máy độc quyền SỰ KIỆN** (đo và hiển thị thời gian mất kết nối), **người độc quyền PHÁN QUYẾT**.
>
> ⇒ **`dropoutPolicy` KHÔNG còn là một enum chính sách tự động.** Không cần kê tập giá trị `mất-quyền / loại-khỏi-vòng / xoá-khỏi-trận` nữa — không giá trị nào trong số đó được máy tự áp. Thứ duy nhất còn là **cấu hình** ở đây là **ngưỡng grace** (mốc bắt đầu tô nổi bật), mặc định **120 giây**.

### Actors
- **Thí sinh** — mất kết nối, kết nối lại
- **Server** — theo dõi kết nối, quản lý grace period
- **Admin** — người **duy nhất** đổi được trạng thái ghế; can thiệp được cả trước lẫn sau khi hết grace

### Related states
- Ghế: `connected` / `disconnected` / `reconnected` / `dropped_out`
- Grace timer: 120 giây kể từ khi mất kết nối

### Trigger
- Thí sinh mất kết nối tới server (socket timeout, network down, browser close)
- Server phát hiện timeout connection của thí sinh ≥ **1000ms** (vd)

### Preconditions
- Thí sinh đã join trận (authenticated)
- Trận đang chạy

### Inputs
- ghế mất kết nối
- `disconnectTime`: server timestamp khi phát hiện disconnect
- **Ngưỡng grace** (mặc định **120 giây**) — mốc để hệ thống bắt đầu **tô nổi bật** ghế. Không có `dropoutPolicy` tự động (`U-13` đóng)

### Conditions
**Mất kết nối**:
- Connection socket ngắt quá 1000ms (timeout)

**Trong grace (< 120s)**:
- Thí sinh kết nối lại trước hết 120 giây
- Ghế được restore state

**Quá grace (>= 120s)**:
- Ghế được **tô nổi bật** trên màn admin; trạng thái ghế **không tự đổi**
- Thứ duy nhất đổi được trạng thái ghế là **một cú bấm của admin**: **giữ / gia hạn** (mọi lúc). Không có chính sách tự động nào — `dropoutPolicy` **không tồn tại** (`U-13` đóng 26/07)
- **`Đ-52` (27/07) — v1 KHÔNG có kick.** Nhánh *kick* của `Đ-45a` bị gỡ khỏi rule này. Thay thế là **vô hiệu hoá / kích hoạt lại ghế** (`EVENT-048`): **đảo ngược được**, dùng **mọi lúc** (không ràng buộc `LOBBY`), ghế **giữ nguyên điểm và vị trí**, chỉ mất quyền thao tác. **Vô hiệu hoá KHÔNG phải hệ quả của mất kết nối** — hai chuyện độc lập, admin phải bấm riêng

### Decision table

| Case | Conditions | Expected outcome | State change | Error |
|---|---|---|---|---|
| C1 — Mất kết nối; kết nối lại < 120s | Thí sinh quay lại trong grace | Ghế restore state; banner "đã kết nối lại" | Seat state: quay về cũ | — |
| C2 — Quá 120s grace period | Admin không can thiệp; thời gian quá 120s | **Ghế được TÔ NỔI BẬT trên màn admin** kèm thời lượng mất kết nối. **KHÔNG tự loại, KHÔNG tự xoá.** Ghế **giữ nguyên** trong trận cho tới khi admin bấm | Trạng thái ghế **không đổi**; chỉ thêm chỉ báo hiển thị | — |
| C2b — Admin phán quyết sau khi quá grace (`Đ-52`, chốt 27/07) | Admin bấm **giữ** / **gia hạn** — **hai** lựa chọn, không còn *kick* | Theo đúng lựa chọn của admin. Cả hai đều **không phá gì**, chỉ nói *"chờ tiếp"* | Chỉ đổi khi admin bấm | — |
| C2c — **Ghế quá grace giữa một vòng đang chạy** | Vòng chưa kết thúc | Ghế **ở lại trạng thái tô nổi bật**; admin **giữ / gia hạn**. Vòng **chạy tiếp bình thường** — hệ thống không dừng vì một ghế mất kết nối (`Đ-21`) | Không đổi | — |
| C2d — **Vô hiệu hoá / kích hoạt lại một ghế** (`Đ-52`, chốt 27/07) | Admin bấm `EVENT-048` + dialog Yes/No + lý do. **Mọi lúc** trong một trận chưa đóng sổ; ghế đang online hay mất kết nối đều được | Ghế **mất quyền thao tác** nhưng **ở lại trong trận**: giữ nguyên điểm, giữ nguyên vị trí, vẫn trên bảng điểm và bảng xếp hạng. Tín hiệu lỡ tới vào lịch sử nhưng **TRƠ**, không drop. **Kích hoạt lại KHÔNG hoàn nguyên gì** — điểm ghi trong lúc bị vô hiệu hoá giữ nguyên | Seat: `disabled` ⇄ bình thường; vào AuditLog | Giữa một câu đang mở, ghế bị vô hiệu hoá được xử **y như ghế không trả lời** (`Đ-43` mặc định SAI khi admin chốt câu). Thấy bất công ⇒ admin **cộng tay** (`GR-029`) |
| C3 — Rớt mạng đúng lượt riêng Khởi động | Disconnect xảy ra vòng Khởi động riêng | **Admin quyết** — hệ thống chạy tiếp bình thường; admin ngừng thao tác và xử lý ngoài hệ thống | Không đổi | — |
| C4 — Kết nối lại; trận đã chuyển vòng | Thí sinh kết nối lại ở vòng tiếp theo | Sync state toàn bộ vòng; restore tất cả UI | UI: full sync | — |
| C4b — **Kết nối lại khi câu đang mở, đồng hồ đang chạy** (`Đ-45b`, chốt 27/07) | Thí sinh quay lại giữa một câu | **Client dựng lại đúng màn đang thi**: câu đang mở, **hạn chót theo server time**, bản gửi gần nhất của chính ghế đó, các cờ ghế còn hiệu lực, bàn cờ VCNV, điểm công khai, lớp phủ đang bật. **Đồng hồ KHÔNG reset** — mất kết nối không mua thêm thời gian. Gói khôi phục **không chứa đáp án** và **không chứa bài của ghế khác** | UI: full sync giữa câu; **không sinh event nào** | Ký tự đang gõ dở **chưa gửi** thì mất — chưa từng tới server |
| C5 — Admin can thiệp trước hết grace | Admin **gia hạn** cho giữ lâu hơn | **ĐƯỢC.** Grace 120 giây là **khuyến nghị của hệ thống**, không phải ràng buộc cưỡng chế: đây không phải ngưỡng bất khả thi vật lý nên nó chỉ **cảnh báo**, admin ép được (nguyên tắc nền điểm 8). **Vô hiệu hoá sớm cũng được** và không phụ thuộc grace — nó là thao tác độc lập (`Đ-52`, C2d) | Gia hạn ⇒ ghế giữ tiếp. Thao tác của admin đi qua dialog Yes/No + ghi AuditLog kèm lý do | — |
| C6 — Hai thí sinh cùng mất kết nối | Cả A, B disconnect trong grace | Cả hai đều có 120s để quay lại độc lập | Seat A, B: both in grace | — |
| C7 — Mất kết nối ở cuối câu hỏi phụ | Disconnect t=0; hết grace t=120; trận đã xong | **Ghế được GIỮ** | Grace chỉ chi phối **quyền thao tác trong trận**; khi trận đã kết thúc thì không còn gì để thao tác nên hết grace **không có hệ quả nào**. Bản ghi ghế thuộc **contest** và sống theo **retention của match** (`official` 12 tháng · `practice` 3 tháng), không bị grace xoá | — |
| C8 — Grace deadline vs timeout cùng ms | t=119.999s: submission tới, grace hết ở t=120 | Submission được tính vì tới trong grace | Submission: `accepted` | — |

### Outcomes
- **Trong grace**: ghế giữ vị trí; state sync; banner thông báo reconnecting
- **Quá grace**: **chỉ tô nổi bật, không có hệ quả tự động nào.** Mọi thay đổi trạng thái ghế đều đến từ **một thao tác bấm của admin**
- **Quay lại**: UI restore toàn bộ trạng thái

### State changes
- `seat.connection`: `connected` → `disconnected` → `reconnected` (hoặc `dropped_out`)
- `seat.state`: được sync lại nếu quay lại
- Grace timer: 120 giây từ disconnect

### No-change guarantees
- **Grace period là 120 giây**: không thay đổi (từ `game-rules-inventory.md` R-GEN-09)
- **Lịch sử không xoá**: event log giữ nguyên; kết nối lại không xoá event của vòng hiện tại
- **Ghế không bị xoá ngay khi disconnect**: giữ 120s để quay lại

### Error outcomes
- Lệch clock portable Windows LAN (`U-14` — ĐÓNG): grace đo bằng **đồng hồ server**, client không tham gia tính toán; máy thí sinh không NTP **không ảnh hưởng** mốc grace. Xem GR-035
- Grace quá hạn nhưng admin chưa thao tác (ĐÓNG): **CHỜ ADMIN, không auto-drop.** v1 luôn có người điều khiển, và hệ thống không tự thực hiện thao tác có hệ quả (nguyên tắc nền điểm 1). Hết grace chỉ **hiện khuyến nghị** cho admin; ghế vẫn giữ tới khi admin bấm
- **`U-13` ĐÓNG 2026-07-26**: **không cần** kê tập giá trị `dropoutPolicy` — vì **không có chính sách tự động nào** để kê. Máy chỉ **tô nổi bật**, admin quyết. Cấu hình duy nhất còn lại là **ngưỡng grace** (mặc định 120 giây)

### Evaluation order
1. Server phát hiện disconnect (socket timeout)
2. Bắt đầu grace 120s countdown
3. Nếu kết nối lại trước 120s: state-sync, restore
4. Nếu quá 120s: **tô nổi bật ghế trên màn admin**, chờ admin phán quyết — không áp hệ quả nào

### Boundaries
- **Grace 120 giây**: cứng; không thay đổi
- **Deadline xác định**: disconnect_time + 120000ms
- **Boundary t=120.000s**: **vẫn trong grace** — biên đóng (`Đ-28`).

### Idempotency
**Reconnect lần thứ hai**: kết nối lại, mất lại, kết nối lại ⇒ **grace tính LẠI TỪ ĐẦU kể từ lần mất kết nối GẦN NHẤT**, và **không giới hạn số lần** phục hồi.

- Grace là *"cửa sổ chờ một ghế đang mất kết nối quay lại"*; mỗi lần mất kết nối mở một cửa sổ mới. Cộng dồn thời gian của các lần trước sẽ biến grace thành **hình phạt cho mạng yếu**, mà nguồn không có hình phạt nào như vậy.
- Giới hạn số lần cũng là một hình phạt không có trong nguồn. Trường hợp lạm dụng/thiết bị lỗi xử lý bằng **can thiệp của admin** (C5), không bằng luật cứng.
- Mọi lần mất và nối lại đều vào **lịch sử, không bao giờ xoá** (nguyên tắc nền điểm 3) để admin phân xử.

### Concurrency
**Disconnect ở biên grace**: submission tới ở 119.999s, grace hết ở 120.000s ⇒ **trong grace, được nhận** (biên đóng, `Đ-28`).

**Số phận của các submission đã nhận khi ghế bị kick** — `GRR-169` từng để treo, **`Đ-45a` đóng bằng cách loại bỏ tình huống**: kick chỉ hợp lệ ở `LOBBY`, mà ở `LOBBY` **không câu nào đang mở, không đồng hồ nào chạy** ⇒ tại mốc kick **không tồn tại submission đang treo**. Mọi bài đã nhận đều thuộc một vòng **đã khép và đã chấm** ⇒ **GIỮ NGUYÊN, không revert**; chúng là lịch sử append-only (`INV-01`), in đủ trong biên bản. **Kick chỉ chặn tương lai.** Muốn gỡ điểm của một vòng thì đường đúng là **bỏ / chạy lại vòng**, hoặc điều chỉnh điểm thủ công — không phải kick.

### Examples

**Ví dụ 1 — Happy path (quay lại trong grace)**:
- Thí sinh A kết nối vào lúc t=0
- Mất kết nối ở t=10s
- Kết nối lại ở t=60s (còn 60s grace)
- Ghế A restore; tiếp tục thi

**Ví dụ 2 — Quá grace giữa một vòng đang chạy**:
- Thí sinh B mất kết nối t=0
- t=120s: quá grace → ghế **tô nổi bật** trên màn admin, vẫn ở trong trận; vòng **chạy tiếp bình thường**
- Admin có **giữ / gia hạn** — và nếu muốn chặn hẳn B thao tác thì bấm **vô hiệu hoá** (`EVENT-048`), **được phép ngay giữa vòng** (`Đ-52`)
- Mọi bài B đã gửi trong vòng vừa rồi **giữ nguyên**, đã chấm thì đã chấm
- B quay lại và admin đổi ý ⇒ **kích hoạt lại**, ghế thi tiếp bình thường, điểm không đổi

**Ví dụ 3 — Rớt giữa lượt riêng Khởi động**:
- Vòng Khởi động lượt riêng; thí sinh C mất kết nối
- **Engine KHÔNG tự dừng** — đồng hồ chạy tiếp theo server time (`Đ-21`, `INV-16`). Ghế C được **tô nổi bật** trên màn admin
- Admin là người quyết và là người bấm: ngừng thao tác chờ C quay lại, hoặc chạy tiếp, hoặc **bỏ / chạy lại vòng** nếu lượt đã hỏng
- Cần chặn C thao tác thì **vô hiệu hoá** ngay tại chỗ (`Đ-52`) — không phải đợi về `LOBBY`, và bật lại được bất cứ lúc nào

**Ví dụ 4 — State-sync (quay lại vòng tiếp theo)**:
- Thí sinh D quay lại lúc t=50s (trong grace)
- Nhưng trận đã chuyển từ Khởi động sang VCNV
- Server sync trạng thái VCNV cho D; D tiếp tục từ VCNV

**Ví dụ 4b — State-sync giữa một câu đang chạy** (`Đ-45b`):
- Câu hàng ngang VCNV mở lúc t=0, hạn chót server t=15s
- Thí sinh E mất kết nối t=4s, quay lại t=9s
- Server đẩy: câu đang mở · **hạn chót t=15s** (không phải *"còn 15 giây"*) · bản E đã gửi lúc t=3s · cờ E chưa bị loại · bàn cờ VCNV hiện tại
- E thấy đồng hồ **còn 6 giây**, không phải 15 — mất kết nối **không mua thêm thời gian**
- Ký tự E gõ dở lúc t=4s mà chưa bấm gửi thì **mất**; bản gửi lúc t=3s thì còn

**Ví dụ 5 — Boundary grace 120s**:
- Disconnect ở t=0
- t=119.999s: submission tới → tính được
- t=120.000s: **vẫn trong grace** (biên đóng, `Đ-28`); quá 120.000s mới hết.

### Source traceability
- `game-rules-inventory.md` §R-GEN-09 — Reconnect grace 120 giây
- `game-rules-decisions.md` §11.6 `Đ-21` — đồng hồ chạy liên tục, admin xử lý sự cố ngoài hệ thống
- `CLAUDE.md` — State-sync khi quay lại

---

## GR-037 — Phạm vi hiển thị đáp án

### Status

CONFIRMED

### Purpose
Đáp án của các câu hỏi chỉ được phép hiển thị cho những kênh cụ thể tùy theo cấu hình `revealAnswerAfterJudge`. Mục đích: bảo vệ bí mật đề; admin + MC luôn được xem; viewer chỉ xem sau khi chấm xong (nếu bật).

> ⚠️ **Phạm vi của rule này CHỈ là ĐÁP ÁN.** Đừng tổng quát hoá sang các loại thông tin khác — ba loại có ba chế độ khác nhau:
>
> | Loại | Chế độ | Quy định ở |
> |---|---|---|
> | **Đáp án chuẩn** | **MẬT** — chỉ admin + MC | **Rule này** |
> | **Bài làm của thí sinh khác** | **ẨN TẠM THỜI** trong lúc câu còn mở; lộ khi admin bấm hiển thị | GR-008 C9 · nguyên tắc nền điểm 14 |
> | **ĐIỂM SỐ** | **CÔNG KHAI, LUÔN LUÔN** — mọi vai, mọi lúc, gồm cả máy thí sinh | `product-discovery.md` **C-20** (chốt 2026-07-27) |
>
> Ba thứ độc lập nhau: điểm công khai **không** nới lỏng hai dòng trên.

### Actors
- **Admin** — luôn được xem đáp án (authenticated, audit)
- **MC** — luôn được xem đáp án (authenticated, audit)
- **Thí sinh** — không được xem đáp án (trừ practice bật reveal)
- **Viewer** — không được xem (trừ practice sau chấm xong)
- **Overlay** — không được xem (read-only)

### Related states
- `revealAnswerAfterJudge`: `true` / `false` (config per-match)
- `matchPurpose`: `official` / `practice`
- Câu hỏi state: `not_yet_judged` / `judged`

### Trigger
- Khi có yêu cầu hiển thị đáp án từ client (admin UI, viewer, overlay)
- Khi câu vừa được admin chấm xong (`official`)

### Preconditions
- Câu phải đã được admin chấm xong (nếu là `official`)
- Client phải là kênh được phép xem (admin, MC, viewer nếu practice + bật reveal)

### Inputs
- `clientRole`: admin / mc / contestant / viewer / overlay
- `revealAnswerAfterJudge`: `true` / `false`
- `matchPurpose`: `official` / `practice`
- `judged`: `true` / `false` (câu đã chấm xong chưa)

### Conditions
**Admin + MC luôn được xem**:
- Không phụ thuộc `revealAnswerAfterJudge` hoặc `matchPurpose`

**Thí sinh + Viewer + Overlay**:
- **`official` (trận chính thức)**:
  - `revealAnswerAfterJudge = false` (default) → KHÔNG được xem
  - `revealAnswerAfterJudge = true` → được xem SAU khi `judged = true`
- **`practice` (tự luyện tập)**:
  - `revealAnswerAfterJudge = false` (tuỳ config) → KHÔNG được xem
  - `revealAnswerAfterJudge = true` (default) → được xem SAU khi `judged = true`

### Decision table

| Case | Conditions | Expected outcome | State change | Error |
|---|---|---|---|---|
| C1 — Admin xem đáp án | Kênh: admin; bất kỳ config → | Trả về đáp án; ghi audit log | No state change | — |
| C2 — MC xem đáp án | Kênh: MC; bất kỳ config → | Trả về đáp án; ghi audit log | No state change | — |
| C3 — Viewer ở trận official, reveal OFF | `matchPurpose=official`; `revealAnswerAfterJudge=false` → | **KHÔNG trả về đáp án** | Server không trả đáp án | — |
| C4 — Viewer ở trận official, reveal ON, chưa chấm | `matchPurpose=official`; `revealAnswerAfterJudge=true`; chưa chấm → | **KHÔNG trả về đáp án** | Server không trả đáp án | — |
| C5 — Viewer ở trận official, reveal ON, đã chấm | `matchPurpose=official`; `revealAnswerAfterJudge=true`; đã chấm → | Trả về đáp án | No state change | — |
| C6 — Viewer ở trận practice, reveal OFF | `matchPurpose=practice`; `revealAnswerAfterJudge=false` → | **KHÔNG trả về đáp án** | Server không trả đáp án | — |
| C7 — Viewer ở trận practice, reveal ON, đã chấm | `matchPurpose=practice`; `revealAnswerAfterJudge=true`; đã chấm → | Trả về đáp án | No state change | — |
| C8 — Overlay (OBS frame) | Kênh: overlay (read-only) → | **LUÔN KHÔNG trả đáp án**; read-only tuyệt đối | Server không trả đáp án | — |
| C9 — phạm vi cấu hình `revealAnswerAfterJudge` | Một contest chứa **cả** match `official` lẫn match rehearsal | **PER-MATCH** — mỗi match mang giá trị riêng, lấy mặc định theo `matchPurpose` (`official` ⇒ TẮT · `practice` ⇒ BẬT). Đóng `K-4` | Giá trị được **snapshot vào match lúc start**; sửa cấu hình contest sau đó không đụng match đã chạy | — |

> **Vì sao per-match, không phải per-contest** — ba nguồn thuận, một nguồn nghịch, và nguồn nghịch là **bản nháp**:
>
> | Nguồn | Phát biểu | Hiệu lực |
> |---|---|---|
> | `research/ruleconfig-v2-spec.md` §11 và §13 | **per-MATCH**, §13 còn ghi thẳng *"không phải per-contest"* | Spec engine |
> | `CLAUDE.md` §Nguyên tắc code | *"config **per-match** theo matchPurpose"* | **Đang có hiệu lực** |
> | `plans/**/PRD.md` NFR-4 | *"**contest** bật"* | **BẢN NHÁP** — theo `.specify/memory/constitution.md`, nháp không phải nguồn sự thật |
>
> Lập luận độc lập với thứ bậc nguồn: per-contest **phá use-case rehearsal của `D21`** — một contest không thể vừa có trận chính thức giấu đáp án vừa có trận tổng duyệt hiện đáp án. Việc cần làm: sửa câu chữ `NFR-4` khi migrate vào `docs/source/`.
>
> **Không lẫn với mode trả lời**, vốn là **per-CONTEST** (`Q-C1c`): mode là thuộc tính của **sân khấu và phần cứng** (có micro không, thí sinh có bàn phím không) nên không đổi giữa hai trận cùng phòng; `revealAnswerAfterJudge` là thuộc tính **sư phạm** của từng trận. Hai trục khác nhau, không phải thiếu nhất quán.

### Outcomes
- **Trả về đáp án**: server gửi `question.acceptedAnswers`; audit log ghi lại
- **Bỏ qua**: server không gửi; client nhận `null` hoặc `forbidden`

### State changes
- No state change (đáp án hiển thị không thay đổi trạng thái trận)
- Audit log: thêm entry (ai xem đáp án, lúc nào)

### No-change guarantees
- **Admin + MC luôn được xem**: không có ngoại lệ
- **Viewer không bao giờ được xem** trừ khi (practice + reveal ON + judged)
- **Overlay luôn read-only**: không bao giờ được xem đáp án
- **Lịch sử audit**: đáp án view được ghi nhận

### Error outcomes
- Client yêu cầu đáp án không được phép: server bỏ qua, không trả lỗi (im lặng)

### Evaluation order
1. Nhận request xem đáp án từ client
2. Check `clientRole` (admin/mc/contestant/viewer/overlay)
3. Nếu admin hoặc MC → trả đáp án
4. Nếu kênh khác:
   - Check `matchPurpose`, `revealAnswerAfterJudge`, `judged`
   - Quyết định trả hay bỏ
5. Ghi audit log (nếu trả)

### Boundaries
- **Phạm vi admin + MC**: cứng (luôn được)
- **Phạm vi viewer**: tuỳ config
- **Phạm vi overlay**: không được (cứng)
- **Mốc reveal**: SAU khi `judged = true` (SAU khi admin bấm Đúng/Sai)

### Idempotency
**Xem đáp án nhiều lần**: Audit log ghi mỗi lần; không ảnh hưởng trạng thái trận

### Concurrency
**Hai viewer xem cùng lúc**: Độc lập; cả hai được phép hay không phép tuỳ config

### Examples

**Ví dụ 1 — Admin xem (luôn được)**:
- Admin UI hiển thị câu hỏi + ô nhập đáp án (side-by-side)
- Admin thấy đáp án dù chưa chấm, chưa xong trận

**Ví dụ 2 — Viewer official, reveal OFF**:
- Trận `official`; `revealAnswerAfterJudge = false` (default)
- Câu chấm xong; admin + MC xem đáp án
- Viewer không xem được; client nhận empty (hoặc `null`)

**Ví dụ 3 — Viewer practice, reveal ON**:
- Trận `practice`; `revealAnswerAfterJudge = true` (default)
- Câu chưa chấm → viewer không xem
- Câu chấm xong (`judged = true`) → viewer xem được

**Ví dụ 4 — Overlay (OBS)**:
- OBS livestream lấy frame từ `/overlay` (public, 6-digit room code)
- OBS **KHÔNG bao giờ** nhận đáp án; read-only tuyệt đối

**Ví dụ 5 — hai match trong cùng một contest** (chính là use-case đóng `K-4`):
- Contest "Chung kết khối 12" chứa **match tổng duyệt** (`practice`, `revealAnswerAfterJudge = true`) và **match chính thức** (`official`, `revealAnswerAfterJudge = false`)
- Cùng một contest, cùng một kho đề, cùng một mode trả lời — **nhưng hai giá trị reveal khác nhau**
- Với per-contest thì kịch bản này **không dựng được**; đây là lý do quyết định cuối cùng là **per-match**

### Source traceability
- **`K-4` ĐÃ PHÂN XỬ 2026-07-26 — per-MATCH**: `SPEC` §11/§13 + `CLAUDE.md` (đang có hiệu lực) thắng `PRD` NFR-4 (**bản nháp**). Xem C9
- `game-rules-inventory.md` §R-GEN-10 — Phạm vi đáp án
- `CLAUDE.md` §Zero-trust security — Đáp án chỉ admin + MC
- `CLAUDE.md` §Mô hình truy cập — Viewer public, read-only
- `game-rules-inventory.md` §R-GEN-03, §TERM-019 — `matchPurpose` + `revealAnswerAfterJudge`

