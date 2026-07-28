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
| `decisions.md` | **Vì sao** — 82 quyết định `QĐ-001` → `QĐ-082`, kèm bảng tra mã cũ |
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
| **Câu hỏi phụ** | `GR-022` điều kiện kích hoạt · `GR-023` thể thức ba câu · `GR-024` chuông không sống trước hiệu lệnh · `GR-025` hết câu chưa phân định |
| **Chấm điểm và mô hình điểm** | `GR-026` phán quyết của admin · `GR-027` chuẩn hoá và tô nổi bật · `GR-028` điểm là hàm của event log · `GR-029` điều chỉnh điểm thủ công · `GR-030` bỏ vòng, chạy lại, kết thúc sớm · `GR-031` rút đề và không lặp câu |
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
- **Không thứ gì tự lộ khi HẾT GIỜ** — hết giờ chỉ khoá ô nhập, không mở gì cả.
- **Bài làm của từng thí sinh** chỉ hiện khi **admin bấm hiển thị**, ở mọi thời điểm.
- **Đáp án chuẩn của hàng ngang** tự công bố tại mốc **CÂU KHÉP** *(tức khi admin chấm xong)* nếu `revealAnswerAfterJudge` bật — `GR-037`, `QĐ-080`. Đây là **thay đổi so với bản trước**, vốn buộc admin bấm; hai thứ trên **không** đi cùng nhau nữa. Admin vẫn mở và đóng tay được (`QĐ-048`).

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
| C11 | Cấu hình 5-8 hàng ngang | **Không dựng được ở v1** — `rowCount` **khoá cứng ở 4** (`QĐ-068`), cửa tạo contest không cho chọn giá trị khác. Mô hình dữ liệu vẫn nhận 5-8 để phiên bản sau chỉ việc mở khoá; khi mở, băng điểm là **mảng cấu hình BẮT BUỘC** dài bằng `rowCount` — không suy ra từ luật, vì nguồn không có thang cho ≠ 4 | Không đổi ở v1 |
| C12 | Admin **đánh dấu đã hỏi** một ô bằng tay | **Băng tụt một bậc y như một lượt hỏi thật.** **Không ai được cộng điểm** từ thao tác này | Ô sang *đã hỏi*; `AuditLog` ghi **do-admin** để phân biệt với do-luồng |
| C13 | Admin **lộ đáp án** một ô | **Băng KHÔNG đổi** — biến đếm là *đã hỏi*, không phải *đã lộ*. Nhờ vậy mở tay trọn một ô chỉ tính **một lần** | Ô sang *mở* |
| C14 | Người vừa bấm *"Mở chướng ngại vật"* trả lời một hàng ngang trong lúc chờ duyệt | **ĐƯỢC, bình thường.** Nguồn chỉ loại thí sinh khi trả lời **SAI Chướng ngại vật**; khoá ngay lúc bấm là **nghiêm hơn luật**. Và nếu admin bấm **No**, người đó phải **không mất gì** (`QĐ-022`) — khoá sớm sẽ vi phạm chính điều đó | Không đổi |
| C15 | Cần cộng điểm cho tình huống ngoài luật | **Admin tự cộng tay** qua điều chỉnh điểm — có event, có tên người bấm, **hoàn nguyên được** | Sinh event điều chỉnh điểm |

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

**Mục đích.** Định nghĩa cách tính điểm ở Tăng tốc: theo **thứ hạng tốc độ trong số người được chấm ĐÚNG**, không theo chuông và không tích luỹ.

**Kích hoạt.** Admin chấm một câu của vòng này.

**Điều kiện.**

- Thang điểm **40 / 30 / 20 / 10** theo thứ hạng, **chỉ tính trên tập người được chấm ĐÚNG**.
- **Người sai không giữ chỗ trong thang** — thang tụt bậc khi có người **đúng**, không phải khi có người trả lời.
- Mốc xếp hạng là **server-received timestamp của bản cuối hợp lệ** (`GR-015`).
- Một câu = **MỘT** event điểm cho **toàn bộ** bảng.

**Bảng quyết định**

| Ca | Điều kiện | Kết quả | Thay đổi trạng thái |
|---|---|---|---|
| C1 | 4 ghế, 3 người đúng theo thứ tự 1→2→3 | **+40 · +30 · +20**; người thứ tư **0** | Một event điểm cho cả bảng |
| C2 | 2 người **cùng mốc** đứng đầu, 1 người sau | **+40 · +40** cho hai người đầu; người sau **+20** — bậc kế **nhảy qua** số người hoà | Một event điểm cho cả bảng |
| C3 | Đúng **1** người | **+40** cho người đó | Một event điểm |
| C4 | **0** người đúng | Không ai cộng điểm | Không sinh event điểm |
| C5 `[v1.5]` | Số ghế ≠ 4 | **Ngoài phạm vi luật v1** — thang cho dải 1-12 ghế thuộc v1.5 | — |
| C6 | Admin bấm **Sai** | **0** điểm; **không tính** trong xếp hạng, và **không giữ chỗ** trong thang | Nằm trong event điểm chung của câu |
| C7 | Câu thuộc **vòng đã bị bỏ** | **Toast**, không thực hiện được | Không đổi |

**Không đổi gì.** Timestamp bản gửi của người khác · các câu trước — tính điểm **độc lập theo từng câu** · các vòng khác.

**Thứ tự đánh giá.** **(1)** ghi nhận tập người được chấm Đúng → **(2)** sắp theo timestamp bản cuối, độ phân giải **millisecond** → **(3)** gộp người cùng mốc thành **một bậc** → **(4)** gán điểm theo bậc, bậc kế **nhảy qua** số người hoà → **(5)** phát **một** event điểm cho cả bảng tại cú bấm **chốt câu**.

**Biên.** **1** ghế: người đó nhận **40** nếu được chấm Đúng. **4** ghế: thang O26 gốc. **5-12** ghế: `[v1.5]`.

**Bấm trùng.** Bấm Đúng hai lần cho cùng người và cùng câu ⇒ **một** event. **Không có** đường đổi phán quyết của một người sau khi đã chốt câu — sửa sai đi qua điều chỉnh điểm thủ công.

**Ví dụ.**

- 4 ghế A·B·C·D — A **100 ms**, B **150 ms**, C **200 ms**, D không trả lời; admin chấm Đúng A, B, C ⇒ **A +40 · B +30 · C +20**.
- A và B **cùng 100 ms**, C **150 ms**, cả ba đúng ⇒ **A +40 · B +40 · C +20**. Hai người ở bậc 1 nên bậc kế là **bậc 3**, tức **20 điểm** — không phải 10.
- Một mình thi, trả lời đúng ⇒ **+40**.

**Nguồn**: luật gốc §Tăng tốc · `QĐ-013`, `QĐ-014`, `QĐ-059`

---

## GR-014 — Tăng tốc: đồng thời gian

**Mục đích.** Quy định xử lý khi nhiều thí sinh trả lời đúng **trong cùng một khoảng thời gian**.

**Kích hoạt.** Admin chấm Đúng ≥2 thí sinh của cùng một câu, có server-received timestamp **bằng nhau ở mức millisecond**.

**Điều kiện.**

- **Độ phân giải so sánh: MILLISECOND** — hai số nguyên, không làm tròn, không số thực.
- Mọi người trong nhóm hoà nhận **cùng mức điểm** — mức của bậc mà nhóm đứng.
- Bậc kế **nhảy qua** số người hoà: sau nhóm `k` người ở bậc `n`, người kế nhận bậc `n+k`.

**Bảng quyết định**

| Ca | Mốc thời gian | Kết quả |
|---|---|---|
| C1 | A và B cùng **5420**, C **7100**, D **9800** — cả bốn đúng | **40 · 40 · 20 · 10.** Hai người ở bậc 1 ⇒ bỏ qua bậc 2, C vào **bậc 3** |
| C2 | Như C1 nhưng D **sai** | **40 · 40 · 20 · 0** |
| C3 | A, B, C cùng **5420** đúng; D **7100** đúng | **40 · 40 · 40 · 10.** Ba người ở bậc 1 ⇒ D vào **bậc 4** |
| C4 | A **3200**; B và C cùng **5420**; D **7100** — cả bốn đúng | **40 · 30 · 30 · 10.** Hai người ở bậc 2 ⇒ D vào **bậc 4** |
| C5 | A **5423** vs B **5429** | **KHÔNG hoà** — 40 · 30. Chênh 6 ms là phân định được ở độ phân giải ms |
| C6 | A **5420** vs B **5421** — chênh đúng **1 ms** | **KHÔNG hoà** — 1 ms là đơn vị nhỏ nhất, vẫn phân định |

**Không đổi gì.** Nội dung bản gửi — hoà là về **thời gian**, không phải nội dung · các câu khác · **nhảy bậc không sinh điểm âm** cho ai.

**Thứ tự đánh giá.** **(1)** chấm từng người; chỉ người **Đúng** vào tập xếp hạng → **(2)** lấy **bản cuối hợp lệ** của mỗi người và mốc của bản đó → **(3)** so **trực tiếp trên số nguyên ms** → **(4)** gộp người bằng nhau thành một bậc → **(5)** gán cùng mức điểm cho cả nhóm → **(6)** bậc kế = bậc hiện tại **+ số người trong nhóm** → **(7)** chốt câu ⇒ **một** event điểm cho cả bảng.

**Biên.** Nhóm hoà **0** hoặc **1** người: không phải hoà, luật không áp. **2-4**: bình thường. **≥5**: chỉ xảy ra khi >4 ghế, `[v1.5]`.

> **Chênh với tiền lệ chương trình — đã biết và đã chấp nhận.** Chương trình thật xếp thứ tự ở **hàng phần trăm giây**, tức cửa sổ hoà ở đây **hẹp hơn 10 lần**: hai bản cách nhau 3 ms sẽ **được phân định** trong khi chương trình thật coi là **hoà**. Hệ quả: điều khoản *"cùng nhận một mức điểm"* vẫn đúng về hành vi nhưng **rất ít khi được kích hoạt**.
>
> Đây là **lựa chọn có chủ đích** vì lý do kỹ thuật — so hai số nguyên là phép so đơn giản nhất. **Đường thoát nếu đổi ý**: đặt `tieRule` ở mức **10 ms**; phép so vẫn là số nguyên (chia nguyên cho 10 rồi so), nên **không** đánh đổi gì về độ phức tạp. **Luật, bảng quyết định và thang nhảy bậc không phải sửa.**
>
> **Bề rộng tiền lệ**: điều khoản chia điểm khi đồng thời gian tồn tại **từ Olympia 7** — khoảng 19 mùa liên tục, không phải điều khoản mới của O26.

**Ví dụ.**

- *Hợp lệ*: A **5420**, B **5420**, C **7100**, D **9800**, cả bốn đúng ⇒ **40 / 40 / 20 / 10**.
- *Hợp lệ*: A, B, C cùng **5420** đúng, D **7100** đúng ⇒ **40 / 40 / 40 / 10**.
- *Không hợp lệ*: xếp hạng trên timestamp của **bản gửi đầu** thay vì **bản cuối** ⇒ trái `GR-015`.

**Nguồn**: luật gốc §Tăng tốc · `QĐ-013`, `QĐ-049`, `QĐ-059`

---

## GR-015 — Tăng tốc: ghi nhận bản cuối

**Mục đích.** Quy định Tăng tốc **không khoá ô nhập** sau lần trả lời đầu, và điểm tính trên **bản cuối cùng**.

**Kích hoạt.** Thí sinh gửi rồi gửi lại; admin chấm câu.

**Điều kiện.**

- Nội dung cuối **có thay đổi** ⇒ mốc xếp hạng **cập nhật** theo lần gửi mới.
- Nội dung cuối **y hệt** bản trước sau khi trim ⇒ **KHÔNG cập nhật mốc**, giữ lần đầu khai nội dung đó.
- Bản **rỗng** sau khi trim ⇒ **bỏ qua**, giữ bản hợp lệ trước đó.

> **Vì sao không cập nhật mốc khi nội dung y hệt**: cập nhật mốc cho một bản **không đổi nội dung** cho phép thí sinh **tự làm xấu** thứ hạng của mình bằng một thao tác vô nghĩa — mà ở vòng xếp hạng thì mốc **chính là** kết quả.

**Bảng quyết định**

| Ca | Chuỗi gửi | Bản được ghi nhận | Mốc xếp hạng |
|---|---|---|---|
| C1 | `"Hà Nội"` @100 → `"Hanoi"` @150 → `"Hà Nội"` @200 | `"Hà Nội"` | **100** — bản @200 y hệt bản @100 nên không cập nhật |
| C2 | `"Hà Nội"` @100 → `"Hanoi"` @150 → `"Hà Nội khác"` @200 | `"Hà Nội khác"` | **200** — nội dung mới |
| C3 | `"Hà Nội"` @100 → `"   "` @150 | `"Hà Nội"` | **100** — bản rỗng bị bỏ qua |
| C4 | `"   "` @100 → `"Hà Nội"` @150 | `"Hà Nội"` | **150** — bản rỗng đầu bị bỏ qua |
| C5 | Có bản hợp lệ trước hạn, còn gửi thêm **sau** hạn | Bản hợp lệ cuối dùng cho xếp hạng; bản quá hạn tô **đỏ** | Admin đối chiếu rồi quyết **Đúng / Sai** |
| C6 | **Chỉ** gửi sau hạn | Bản quá hạn tô **đỏ**; admin quyết **Đúng / Sai / Huỷ kết quả** | Nếu công nhận thì **cả bảng xếp hạng của câu tính lại** |

**Không đổi gì.** **Lịch sử đầy đủ các bản đã gửi** — chỉ *bản được ghi nhận* đổi · bản gửi của ghế khác · hạn chót của câu.

**Thứ tự đánh giá.** **(1)** server ghi nhận mọi lần gửi → **(2)** hết giờ → **(3)** admin chấm; server lấy **bản cuối hợp lệ**, bỏ qua bản rỗng → **(4)** tính điểm theo `GR-013` → **(5)** mốc xếp hạng theo quy tắc *y hệt thì không cập nhật* ở trên.

**Biên.** **0** lần gửi: không có bản nào để chấm ⇒ admin bấm **Sai** — *không trả lời* và *trả lời sai* là **cùng một thao tác**. Gửi lại **trong cùng một mili-giây**: nội dung khác ⇒ cập nhật; y hệt ⇒ không.

**Đồng thời.** Không có tranh chấp giữa *gửi* và *chấm*: Tăng tốc **luôn gõ máy**, nên **nút chấm khoá tới khi hết giờ**; và chấm xong thì **nút gửi khoá lại**.

**Ví dụ.**

- `"Paris"` @100 → `"London"` @150 → `"Paris"` @200 ⇒ bản cuối `"Paris"`, mốc **100**.
- `"Paris"` @100 → `"London"` @150 → `"Berlin"` @200 ⇒ bản cuối `"Berlin"`, mốc **200**.
- Không gửi gì ⇒ admin bấm **Sai**; không tham gia xếp hạng và **không giữ chỗ** trong thang.

**Nguồn**: luật gốc §Tăng tốc · `QĐ-029`, `QĐ-030`, `QĐ-056`, `QĐ-059`

---

## GR-016 — Về đích: thứ tự lượt thi

**Mục đích.** Xác định thí sinh nào thi lượt kế tiếp. Thứ tự **tính lại sau mỗi lượt hoàn thành**.

**Kích hoạt.** Mở vòng Về đích, hoặc một lượt vừa hoàn thành và còn người chưa thi.

**Điều kiện.**

- **Điểm cao nhất tại thời điểm xếp lượt** đi trước.
- Hoà điểm ⇒ **số vị trí NHỎ NHẤT** đi trước. *"Vị trí"* là **số thứ tự `1..N` gán cho ghế trước trận**, không phải thứ hạng điểm.
- Hệ thống chỉ **khuyến nghị**; admin chốt và **ép được** qua dialog cảnh báo.
- Điểm **âm** tham gia bình thường: cao nhất trong `{−100, −110}` là **−100**.

**Bảng quyết định**

| Ca | Điều kiện | Kết quả |
|---|---|---|
| C1 | A **110**/vị trí 1 · B **90** · C **80** · D **70** | Khuyến nghị **A** |
| C2 | A **110**/vị trí 2 · B **110**/vị trí 1 | Khuyến nghị **B** — hoà điểm, vị trí nhỏ hơn |
| C3 | Sau lượt 1, A còn **90**; B **110** | Khuyến nghị **B** — bảng tính lại sau mỗi lượt |
| C4 | Cả bốn cùng **100**, vị trí 1·2·3·4 | Khuyến nghị **A** — số vị trí nhỏ nhất |
| C5 | Admin chọn B (**90**) trong khi A có **110** | **Dialog cảnh báo** Yes/No; admin bấm Yes ⇒ vẫn thực hiện. **Không chặn cứng** |

**Không đổi gì.** Vị trí ghế — **không đổi trong suốt trận** · điểm từ các vòng trước · quyết định của lượt trước không ràng buộc lượt sau.

**Thứ tự đánh giá.** **(1)** tính điểm tích luỹ hiện tại → **(2)** sắp giảm dần theo điểm → **(3)** hoà thì sắp tăng dần theo **số vị trí** → **(4)** khuyến nghị người đầu danh sách → **(5)** admin chốt hoặc ép → **(6)** lượt hoàn thành, quay lại bước 1.

**Biên.** Chưa ai thi: khuyến nghị người điểm cao nhất. Còn **1** người chưa thi: đó là lượt cuối. Số ghế ≠ 4: `[v1.5]`.

**Đồng thời.** Bảng xếp phải lấy **sau khi lượt trước kết thúc hoàn toàn** — nghĩa là cửa sổ cướp đã đóng **và** đã chấm xong. Không tính giữa lúc một câu đang chạy.

**Ví dụ.**

- A **110** · B **90** · C **80** · D **70** ⇒ khuyến nghị **A**.
- A thi, cướp sai **−20** còn **90**; B cũng **90** nhưng vị trí nhỏ hơn ⇒ khuyến nghị **B**.
- Admin chọn D thi trước trong khi A cao nhất ⇒ **cảnh báo**, admin bấm Yes thì vẫn chạy.

**Nguồn**: luật gốc §Về đích đoạn *"Thứ tự tham gia…"* · `QĐ-002`, `QĐ-012`

---

## GR-017 — Về đích: chọn gói câu

**Mục đích.** Thí sinh chọn **3 câu** từ hai mức **{20, 30}** để tạo gói của mình, trước khi câu đầu tiên được rút.

**Kích hoạt.** Thí sinh được chốt là người thi lượt hiện tại.

**Điều kiện.**

- Gói gồm **đúng 3 mục**, mỗi mục ∈ **{20, 30}**. Bốn phối hợp hợp lệ: **20/20/20 · 20/20/30 · 20/30/30 · 30/30/30**.
- **Chủ thể chọn phụ thuộc MODE:**

| Mode | Ai chọn | Cơ chế |
|---|---|---|
| **Sân khấu** *(mặc định)* | **Admin bấm** | Thí sinh **nói gói của mình trên sân khấu**, admin nghe và bấm. Máy thí sinh **không render** nút chọn gói — đây là **đường vào duy nhất**, không phải fallback |
| **Nhập liệu** | **Thí sinh** | Tự chọn trên máy; admin **chọn hộ** mặc định **20/20/20** nếu chưa chọn khi tới lượt |

- **Đổi gói được, cho tới mốc admin bấm hiển thị câu ĐẦU TIÊN của gói** — last-wins tới mốc đó rồi khoá. Mốc này có sẵn và có đúng ý nghĩa cần thiết: **đề đã rời server**.
- Server **validate lại** bất kể client là ai.

**Bảng quyết định**

| Ca | Điều kiện | Kết quả |
|---|---|---|
| C1 | Chọn **20/20/20** | Chấp nhận; chuẩn bị rút 3 câu mức 20 |
| C2 | Chọn **20/30/30** | Chấp nhận; chuẩn bị rút 1 câu mức 20 và 2 câu mức 30 |
| C3 | **Mode nhập liệu**, chưa chọn khi tới lượt | **Admin chọn hộ mặc định 20/20/20**, override được trong dialog |
| C4 | **Mode sân khấu**, thí sinh nói gói | **Admin bấm chốt gói theo lời thí sinh.** Không có mốc *"quá hạn"* — admin vốn là người bấm |
| C5 | **Mode sân khấu**, máy thí sinh gửi lựa chọn gói | **Không tồn tại đường này**: nút không render; server **từ chối** nếu vẫn nhận được |
| C6 | Đổi gói **trước** mốc khoá | **Được** — last-wins |
| C7 | Đổi gói **sau** mốc khoá | **Không** — đề đã rời server |
| C8 | Số mục ≠ 3, hoặc mức ngoài {20, 30} | Từ chối ở cả client và server |

**Không đổi gì.** Lựa chọn của thí sinh khác — mỗi người một gói riêng · thứ tự chọn **không** ảnh hưởng thứ tự rút; mức chỉ là bộ lọc.

**Biên.** Pre-flight kiểm kho đề theo **trường hợp xấu nhất**: mọi thí sinh cùng chọn một mức. Vì phép kiểm chạy **trước khi ai chọn**, nó phải đúng cho **cả hai** mức:

> **Cần `3 × số ghế` câu ở mức 20 VÀ `3 × số ghế` câu ở mức 30.** Với 4 ghế: **12 + 12 = 24 câu**, trong khi vòng chỉ tiêu **12**. Số dư **đúng 12** là tất định, không phụ thuộc lựa chọn của ai — và nó là **nguồn đề của vòng Câu hỏi phụ** (`GR-023`, `QĐ-081`).

Thiếu ⇒ **không mở được vòng Về đích**.

**Bấm trùng.** Chọn nhiều lần trước mốc khoá là **last-wins** — cùng ngữ nghĩa với nút gửi đáp án. Chọn gói **không phải chuông**, nên **không** áp cơ chế *"bấm xong thì tắt"*.

**Ví dụ.**

- *Hợp lệ*: chọn **20/30/30** ⇒ rút 1 câu mức 20 và 2 câu mức 30.
- *Hợp lệ*: mode nhập liệu, thí sinh chưa chọn khi tới lượt ⇒ admin bấm chọn hộ **20/20/20**.
- *Không hợp lệ*: chọn **20/20** (thiếu một mục) hoặc **20/40/20** (mức 40 không có trong preset).

**Nguồn**: luật gốc §Về đích đoạn 4 · `QĐ-019`, `QĐ-029`, `QĐ-042`

> **Khác `QĐ-019` một điểm, có chủ ý**: ở **chọn hàng ngang**, mode nhập liệu **cấm tuyệt đối** admin chọn thay. Ở **chọn gói**, mode nhập liệu **vẫn giữ** fallback admin chọn hộ — vì lượt thi cần một đường thoát để không tắc, còn chọn hàng ngang thì không có mốc *"quá hạn"* tương đương.

---

## GR-018 — Về đích: trả lời câu của mình

**Mục đích.** Quy định điểm, thời gian và cách ghi nhận đáp án cho **người thi chính**.

**Kích hoạt.** Câu trong gói được hiển thị và admin bấm start timer.

**Điều kiện.**

- Giá trị câu **20** hoặc **30**; thời gian suy nghĩ lấy từ **metadata từng câu** — mặc định **15 giây** (câu 20đ) và **20 giây** (câu 30đ).
- **Người thi chính tính BẢN CUỐI CÙNG.** *(Người cướp quyền thì ngược lại — chỉ tính bản đầu tiên, xem `GR-020`.)*
- Đúng ⇒ **+giá trị câu**. Sai hoặc không trả lời ⇒ **0 điểm** *(không trừ)* và **mở cửa sổ cướp 5 giây**.
- *"Không trả lời"* và *"trả lời sai"* là **cùng một thao tác** của admin.

**Bảng quyết định**

| Ca | Điều kiện | Kết quả | Thay đổi trạng thái |
|---|---|---|---|
| C1 | Câu 20đ, admin chấm **Đúng** | **+20**; **không** mở cửa sổ cướp | Sinh event điểm; câu kết thúc |
| C2 | Câu 30đ, admin chấm **Sai** | **0**; **mở cửa sổ cướp 5 giây** | Hàng đợi cướp quyền **không chặn** |
| C3 | Không trả lời | Xử **như sai** ⇒ 0 điểm, mở cửa sổ cướp | Như C2 |
| C4 | Đáp án lệch chính tả nhỏ | Máy **chỉ** highlight ký tự khác; **admin quyết** | Theo phán quyết |
| C5 | Trả lời sớm, admin chấm muộn hơn trong cùng câu | Kết quả theo **phán quyết**, không theo thời điểm trả lời — Về đích **không xếp hạng theo tốc độ** | Sinh event điểm |
| C6 | Câu **có Ngôi sao hy vọng**, chấm Sai | **−giá trị câu, ĐÚNG MỘT LẦN** — xem `GR-021` | Sinh event điểm âm; mở cửa sổ cướp |

**Không đổi gì.** Bản gửi của ba thí sinh còn lại · các câu trước · Ngôi sao hy vọng của người khác — cờ này **độc lập theo từng thí sinh**.

**Thứ tự đánh giá.** **(1)** gói được chốt, rút 3 câu → **(2)** hiển thị câu → **(3)** start timer → **(4)** thí sinh trả lời → **(5)** admin chấm → **(6a)** Đúng: cộng giá trị, **câu kết thúc**; **(6b)** Sai: 0 điểm, **mở cửa sổ cướp 5 giây** → **(7)** cửa sổ đóng hoặc có người cướp được ⇒ câu kết thúc.

**Biên.** Giá trị câu chỉ **{20, 30}** dưới preset O26. Chấm **sau khi cửa sổ cướp đã đóng** vẫn thực hiện được — cửa sổ chỉ chi phối **việc bấm chuông**, không khoá nút chấm.

**Ví dụ.**

- Câu 20đ, trả lời `"Hà Nội"` ở giây 10, admin chấm Đúng ⇒ **+20**.
- Câu 30đ, hết giờ chưa trả lời, admin chấm Sai ⇒ **0** và mở cửa sổ cướp **5 giây**.
- Câu 30đ **có NSHV**, chấm Sai ⇒ **−30**, và đó là **toàn bộ** phần mất của người thi chính, kể cả khi sau đó có người cướp đúng.

**Nguồn**: luật gốc §Về đích đoạn 5 và đoạn cuối · `QĐ-029`, `QĐ-056`, `QĐ-058`

---

## GR-019 — Về đích: câu hỏi thực hành

**Mục đích.** Định nghĩa câu được khai là **thực hành**: thí sinh thao tác với dụng cụ, và admin chấm *"đạt yêu cầu"* thay vì so khớp chữ.

**Kích hoạt.** Câu được rút có `isPractical = true`.

**Mô hình dữ liệu — năm trường**

| Trường | Kiểu | Mặc định | Vì sao cần |
|---|---|---|---|
| `isPractical` | boolean | `false` | Cờ khai câu thực hành. **Chỉ hợp lệ với câu thuộc kho Về đích** — luật gốc chỉ có câu thực hành ở vòng này |
| `practiceSeconds` | giây | **30** (câu 20đ) · **60** (câu 30đ) | *"Đối với câu hỏi 20 điểm… thời gian thực hành là 30 giây. Đối với câu hỏi 30 điểm… là 60 giây."* Là **metadata TỪNG CÂU**; preset chỉ đặt mặc định |
| `stealPracticeSeconds` | giây | **20** (câu 20đ) · **40** (câu 30đ) | Cùng đoạn nguồn, nhưng nói về **người cướp quyền** |
| `equipmentNote` | text | rỗng | *"chương trình sẽ giới thiệu các **dụng cụ liên quan**"* — ban tổ chức phải mang dụng cụ tới, nên phải khai được từ lúc soạn đề |
| `acceptanceCriteria` | text | rỗng | Tiêu chí để admin phán quyết *"đạt yêu cầu"*. Đây là **thứ thay cho đáp án chữ** ở câu thực hành |

**Điều kiện.**

- Timer chia **hai pha**: suy nghĩ (`timeSeconds`) rồi thực hành (`practiceSeconds`). Ranh giới là **một cú bấm riêng của admin** — mốc thứ ba, sau *hiển thị câu* và *start timer*.
- Admin chấm **đạt** / **không đạt**. *"Không đạt"* và *"không thực hành gì"* là **cùng một thao tác**.
- Đạt ⇒ **+giá trị câu**. Không đạt ⇒ **0** và mở **cửa sổ cướp 5 giây**.
- **Kênh trả lời thứ BA** — không phải nói, không phải gõ. Vì **không có ô nhập nào để chờ**, nút chấm của admin **sống suốt**, không khoá tới hết giờ.
- **Không có gì để highlight**: cơ chế chuẩn hoá và tô khác biệt ký tự **không áp** ở đây.

**Bảng quyết định**

| Ca | Điều kiện | Kết quả |
|---|---|---|
| C1 | Câu 20đ, admin chấm **đạt** | **+20**; không mở cửa sổ cướp |
| C2 | Câu 20đ, hết 30 giây thực hành, admin chấm **không đạt** | **0** và mở **cửa sổ bấm chuông 5 giây** |
| C3 | Câu 30đ, admin chấm **đạt** | **+30** |
| C4 | Câu 30đ, người thi chính không đạt, có người cướp được quyền | Người cướp **cũng thực hành**, trong **`stealPracticeSeconds`** — **40 giây** cho câu 30đ, **20 giây** cho câu 20đ. Đạt ⇒ **transfer**; không đạt ⇒ **−½ giá trị câu** |
| C5 | Câu 20đ **có Ngôi sao hy vọng**, đạt | **+40** — gấp đôi |
| C6 | Câu 20đ **có Ngôi sao hy vọng**, không đạt | **−20**, và mở cửa sổ cướp |

> **Hai con số hay bị lẫn ở pha cướp** — nguồn có **hai** mốc khác nhau: **cửa sổ bấm chuông** để giành quyền là **5 giây**, không đổi theo mức điểm; **thời gian thực hành của người cướp** là **20/40 giây** và chỉ bắt đầu đếm **sau khi** đã có người giành được quyền.

**Không đổi gì.** Dụng cụ — vật lý, ngoài hệ thống · các câu khác · phán quyết của admin là cuối cùng, **không có nhánh máy tự kết luận** *"đạt yêu cầu"* từ `acceptanceCriteria`.

**Thứ tự đánh giá.** **(1)** rút câu → **(2)** admin bấm **hiển thị câu** *(đây cũng là mốc tiêu câu)* → **(3)** admin bấm **start timer**, đếm pha suy nghĩ → **(4)** admin bấm **bắt đầu thực hành**, đếm pha thực hành → **(5)** admin chấm *đạt* / *không đạt* → **(6)** tính điểm.

**Biên.** Câu 20đ: 15 giây suy nghĩ + **30** giây thực hành. Câu 30đ: 20 + **60**. Người cướp: **20** / **40**.

**Ngoại lệ và cảnh báo.**

- `isPractical` trên câu **không thuộc kho Về đích** ⇒ cấu hình không hợp lệ, chặn ở **kho đề**, không phải lỗi lúc chạy.
- `equipmentNote` rỗng ⇒ **cảnh báo** ở cửa vào vòng — ban tổ chức có thể không biết cần mang gì. **Không chặn cứng**: dụng cụ là vật lý, máy không kiểm được.
- `acceptanceCriteria` rỗng ⇒ admin vẫn phán quyết được bằng đánh giá của mình. **Cảnh báo**, không chặn.

**Bảo mật.** `acceptanceCriteria` cùng mức với đáp án — **chỉ admin và MC**. `equipmentNote` cũng **không** ra viewer hay thí sinh trong trận vì nó tiết lộ bản chất câu hỏi, nhưng **phải** xem được trong kho đề để ban tổ chức chuẩn bị.

**Ví dụ.**

- *Hợp lệ*: câu 20đ, thí sinh suy nghĩ 15 giây rồi thao tác 30 giây, admin chấm **đạt** ⇒ **+20**.
- *Hợp lệ*: câu 30đ có NSHV, chấm **đạt** ⇒ **+60**.
- *Không hợp lệ*: gộp *"start timer"* và *"bắt đầu thực hành"* thành một nút ⇒ mất mốc đóng pha suy nghĩ.
- *Không hợp lệ*: cho máy tự kết luận *"đạt yêu cầu"* từ `acceptanceCriteria` — đó là văn bản để **người** đối chiếu.

**Nguồn**: luật gốc §Về đích đoạn *"Trong câu hỏi thực hành…"* · `QĐ-010`, `QĐ-027`, `QĐ-028`, `QĐ-051`, `QĐ-058`, `QĐ-066`

---

## GR-020 — Về đích: cướp quyền

**Mục đích.** Khi người thi chính trả lời **sai**, các thí sinh còn lại giành quyền bằng cách bấm chuông **trong 5 giây**.

**Kích hoạt.** Admin chấm **Sai** cho người thi chính ⇒ mở cửa sổ cướp.

**Điều kiện.**

- Cửa sổ **5 giây**, hàng đợi **KHÔNG chặn** — server phân xử ngay theo timestamp. Cùng mốc ⇒ hàng đợi tự quyết, ngẫu nhiên.
- **Người cướp chỉ tính BẢN ĐẦU TIÊN** — ngược với người thi chính, vốn tính bản cuối.
- Cướp **đúng** ⇒ **transfer**: người sai **−giá trị câu**, người cướp **+giá trị câu**.
- Cướp **sai** ⇒ người cướp **−½ giá trị câu**; người thi chính **không** được hoàn lại.
- Người thi chính **không** cướp câu của chính mình. Người cướp **không** dùng Ngôi sao hy vọng trên câu đang cướp.

**Bảng quyết định**

| Ca | Điều kiện | Kết quả |
|---|---|---|
| C1 | Câu 20đ, A sai; B bấm sớm nhất và **đúng** | **A −20 · B +20** |
| C2 | Câu 30đ, A sai; B cướp **sai** | **B −15**; A vẫn **−30 nếu có NSHV**, hoặc **0** nếu không. Admin có thể chọn **Huỷ kết quả** thay vì Sai để không áp hình phạt cho B |
| C3 | Câu 20đ, A sai, **không ai bấm** trong 5 giây | A giữ nguyên kết quả của mình; không ai được cộng |
| C4 | B gửi ba bản: `"Paris"` @100 → `"London"` @150 → `"Berlin"` @200 | Chấm trên **bản ĐẦU `"Paris"`** |
| C5 | Câu 20đ, A **có NSHV** và bị chấm Sai; B cướp đúng | **A −20 · B +20.** A mất giá trị câu **ĐÚNG MỘT LẦN** — hình phạt NSHV **thay thế** phần nợ của transfer |
| C6 `[v1.5]` | Số ghế ≠ 4 | v1 đặc tả cho đúng 4 nên *"3 thí sinh còn lại"* luôn đúng nghĩa. Riêng ca **runtime tụt dưới 4 giữa trận** đã chốt: **admin quyết**, hệ thống chỉ cảnh báo |

**Không đổi gì.** Điểm của những người **không bấm** · các câu trước · bản gửi của người thi chính.

> **Cấm: KHÔNG công bố đáp án khi cửa sổ cướp quyền đang mở.** Cú bấm *chấm Sai* của người thi chính vừa là phán quyết vừa là cú **mở cửa sổ này** — nên ở Về đích, *"đã chấm"* **không** phải mốc công bố. Câu chỉ khép sau khi cửa sổ đóng và người cướp đã được chấm (`GR-037`, `QĐ-080`). Công bố sớm biến cướp quyền thành cuộc thi bấm chuột: người cướp chỉ việc đọc lại đáp án vừa hiện trên màn hình.

**Thứ tự đánh giá.** **(1)** admin chấm Sai cho người thi chính → **(2)** mở cửa sổ **5 giây** → **(3)** các thí sinh khác bấm chuông → **(4)** người sớm nhất theo server timestamp giành quyền; cùng mốc thì hàng đợi tự quyết → **(5)** admin chấm người cướp → **(6)** tính transfer hoặc trừ nửa.

**Biên.** Cửa sổ **5 giây**, cứng. Bấm ngoài cửa sổ: **không có tín hiệu nào được tạo**. Chấm **sau khi** cửa sổ đã đóng vẫn thực hiện được — cửa sổ chỉ chi phối việc **bấm chuông**. Điểm người cướp **được phép âm, không có sàn**.

**Giá trị câu LẺ.** Dưới preset O26 **không tồn tại** — giá trị chỉ 20 và 30, nửa là 10 và 15, đều nguyên. Nếu admin cấu hình mức lẻ: `phạt = value / 2` bằng **phép chia số nguyên**, làm tròn xuống theo **độ lớn**.

> ⚠️ **Cẩn thận với DẤU** — đây là chỗ dễ cài sai. `−½ × 25 = −12,5`. Hiểu *"làm tròn xuống"* theo nghĩa toán học thì `floor(−12,5) = −13`, tức **nặng hơn** và **ngược** quyết định. Phải làm tròn trên **độ lớn** rồi mới gắn dấu âm: `−(25 / 2) = −12`.
>
> Ba lý do chọn hướng này: đúng bằng hành vi mặc định của phép chia số nguyên nên **không tốn dòng code nào**; chỗ nguồn im lặng thì chọn hướng **nhẹ hơn** cho thí sinh; và **không** phải hard-code ràng buộc *"giá trị phải chẵn"* vào validation, giữ được lời hứa luật tuỳ biến.

**Ví dụ.**

- Câu 20đ, A sai `"London"`, B bấm sớm nhất và đúng `"Paris"` ⇒ **A −20 · B +20**.
- Câu 20đ, A sai, B cướp sai ⇒ **B −10**.
- Câu 20đ, A sai, không ai bấm trong 5 giây ⇒ không ai được cộng.

**Nguồn**: luật gốc §Về đích đoạn 5 · `QĐ-012`, `QĐ-021`, `QĐ-025`, `QĐ-058`, `QĐ-061`

---

## GR-021 — Về đích: Ngôi sao hy vọng

**Mục đích.** Quyền đặt cược lên câu của chính mình: đúng ⇒ **gấp đôi**, sai ⇒ **−giá trị câu**, *"kể cả các thí sinh còn lại có giành quyền trả lời hay không"*.

**Kích hoạt.** Nút NSHV được bấm **trước mốc admin hiển thị câu**.

**Điều kiện.**

- **Một lần cho mỗi thí sinh trong một LẦN CHẠY vòng Về đích.** Luồng thường vẫn đúng *"1 lần/trận"* như luật gốc, vì Về đích chạy một lần trong một trận. **Bỏ hoặc chạy lại vòng ⇒ cờ đặt lại**, ngôi sao dùng được lại.
- **Cửa sổ đóng tại mốc admin bấm hiển thị câu hỏi.** Ngoài cửa sổ thì nút **không render** ⇒ không có tín hiệu nào, và ngôi sao **vẫn chưa dùng**.
- **Chủ thể bấm phụ thuộc MODE:**

| Mode | Ai bấm | Cơ chế |
|---|---|---|
| **Sân khấu** *(mặc định)* | **Admin** | Thí sinh **nói miệng trên sân khấu** trước khi câu được mở; admin nghe và bấm, qua **dialog Yes/No** vì thao tác không hoàn tác được. Máy thí sinh **không render** nút NSHV |
| **Nhập liệu** | **Thí sinh** | Tự bấm. Tín hiệu **có hiệu lực NGAY, không chờ admin duyệt** — Về đích là vòng hàng đợi **không chặn** |

**Bảng quyết định**

| Ca | Điều kiện | Kết quả |
|---|---|---|
| C1 | Câu 20đ, đặt NSHV, admin chấm **Đúng** | **+40** |
| C2 | Câu 30đ, đặt NSHV, admin chấm **Sai** | **−30** |
| C3 | Bấm NSHV lần thứ hai | **Không được chấp nhận** — nút đã disabled |
| C4 | Câu 30đ, NSHV **đúng**, đồng thời có người cướp | Ca này **không tồn tại**: cướp quyền chỉ mở khi người thi chính bị chấm **Sai** |
| C5 | Câu 30đ, A có NSHV bị chấm **Sai**; B cướp **đúng** | **A −30 · B +30** |
| C5b | Như C5 nhưng B cướp **sai** | **A −30** *(không đổi)* · **B −15** |
| C5c | Như C5 nhưng **không ai cướp** | **A −30** *(không đổi)* |
| C6 | Bấm sau khi admin đã hiển thị câu | **Không tồn tại** — nút không render, không phản hồi. Ngôi sao **vẫn chưa dùng** |
| C7 | **Mode sân khấu**: thí sinh nói, admin bấm | **Hợp lệ.** Cùng một người bấm NSHV và bấm hiển thị câu ⇒ **không có cuộc đua ở biên cửa sổ** |
| C8 | **Mode sân khấu**: máy thí sinh gửi tín hiệu NSHV | **Không tồn tại đường này** — server từ chối, ghi `AuditLog` |

> **Số học NSHV × cướp quyền được nguồn quy định THẲNG, không phải suy diễn.** Luật gốc: *"Trả lời sai sẽ bị trừ đi số điểm của câu hỏi, **kể cả các thí sinh còn lại có giành quyền trả lời hay không**."* Mệnh đề in đậm tồn tại **đúng để** nói rằng số học của người thi chính **độc lập** với hành động của người cướp ⇒ C5, C5b, C5c cho A **cùng một** kết quả.
>
> Nó cũng giải thích **vì sao mệnh đề đó cần có mặt**: ở câu **không** NSHV, A sai mà không ai cướp thì A mất **0**; có NSHV thì A mất `value` **dù không ai cướp**. Đó chính là điều nguồn muốn nhấn.
>
> **Người cướp KHÔNG thể dùng NSHV trên câu đang cướp**: NSHV phải đặt **trước khi câu được đọc**, còn người cướp chỉ quyết định bấm chuông **sau khi** câu đã đọc và đã bị trả lời sai. Ngôi sao của họ vẫn còn nguyên cho **lượt thi của chính họ**.

**Không đổi gì.** NSHV của thí sinh khác — cờ này **độc lập theo từng người** · các câu trước · điểm của câu không đặt NSHV.

**Thứ tự đánh giá.** **(1)** thí sinh thấy câu sắp được hỏi → **(2)** NSHV được đặt, **có hiệu lực ngay** → **(3)** nút NSHV disabled → **(4)** admin bấm **hiển thị câu**, đóng cửa sổ NSHV → **(5)** thí sinh trả lời → **(6)** admin chấm ⇒ **gấp đôi** hoặc **−giá trị câu**.

**Biên.** Gấp đôi: **40** (câu 20đ) · **60** (câu 30đ). Hình phạt: **−20** · **−30**. Đều cứng, không tuỳ chỉnh.

**Bấm trùng.** Bấm lần thứ hai không tồn tại ở giao diện — nút disabled ngay tại lần đầu. Server vẫn phải bỏ qua tín hiệu trùng.

**Ví dụ.**

- *Hợp lệ*: câu 20đ, đặt NSHV, trả lời Đúng ⇒ **+40**.
- *Hợp lệ*: câu 30đ, đặt NSHV, trả lời Sai ⇒ **−30**, kể cả khi sau đó có người cướp đúng.
- *Không hợp lệ*: đặt NSHV **sau** khi admin đã bấm hiển thị câu ⇒ nút đã tắt; ngôi sao vẫn chưa dùng.
- *Không hợp lệ*: **mode sân khấu** mà máy thí sinh render nút NSHV.

**Nguồn**: luật gốc §Về đích đoạn 6 · `QĐ-019`, `QĐ-026`, `QĐ-035`, `QĐ-058`

---

## GR-022 — Câu hỏi phụ: điều kiện kích hoạt

**Mục đích.** Xác định khi nào trận rẽ sang vòng Câu hỏi phụ, khi nào đóng sổ thẳng.

**Kích hoạt.** Admin bấm **Chốt trận** ở `LOBBY` sau khi vòng Về đích đã xong.

**Điều kiện.**

- Hết vòng Về đích, trận **về `LOBBY`** — **chưa** đóng sổ, **chưa** tính hoà, **chưa** có thứ hạng. Admin vẫn sửa điểm được ở đây.
- Phép phân định hoà chạy **tại cú bấm Chốt trận**, trên bảng điểm **ở đúng mốc đó** ⇒ điểm admin vừa sửa **được tính vào**.
- Có hoà **trong `tieBreakPositions`** ⇒ `TIE_BREAK`; không thì `FINISHED`. `tieBreakPositions` mặc định `[1]` — chỉ phân định vị trí **NHẤT**.
- Nhóm hoà phải có **≥ 2** người.
- Phải có đủ **3 câu khả dụng** cho vòng này, kiểm ở **cửa vào vòng**. **Không có kho Câu hỏi phụ riêng** — đề rút từ ba kho nguồn *(Về đích · Khởi động · VCNV)*, xem `GR-023` và `QĐ-081`.

**Bảng quyết định**

| Ca | Điều kiện | Kết quả | Thay đổi trạng thái |
|---|---|---|---|
| C1 | Bấm Chốt trận; 2+ người cùng điểm cao nhất, vị trí 1 ∈ `tieBreakPositions` | Vào vòng Câu hỏi phụ | `LOBBY` → `TIE_BREAK`; gán `tiedSeats` |
| C2 | Như C1 nhưng hoà ở vị trí khác, vẫn ∈ config | Như C1 | Như C1 |
| C3 | Bấm Chốt trận; **một** người điểm cao nhất | Đóng sổ | `LOBBY` → `FINISHED`, `matchClosedReason = "hoàn thành"` |
| C4 | Bấm Chốt trận; có hoà nhưng vị trí **∉** `tieBreakPositions` | Đóng sổ, ghi **đồng hạng** | `LOBBY` → `FINISHED`; biên bản ghi cả nhóm cùng hạng |
| C5 | Admin bấm **Huỷ trận** từ **bất kỳ** trạng thái nào — mất điện, hỏng thiết bị, huỷ buổi thi | Đóng sổ với nhãn **`matchClosedReason = "bỏ dở"`**. **Điểm giữ nguyên, không revert**; **không** phân định thứ hạng, **không** có người thắng | `FINISHED`. Ghi `closedBy` / `closedAt` / `reason`; biên bản in nhãn *"trận bỏ dở"* kèm đủ các vòng đã chạy; thống kê không đếm vào trận hoàn thành. Dialog hạng **phá huỷ** + **lý do bắt buộc** |
| C6 | Vòng Về đích xong, admin **chưa bấm gì** | Trận **đứng ở `LOBBY`** — chưa tính hoà, chưa đóng sổ | Không đổi. Admin vẫn sửa điểm được (`GR-029`) |

**Không đổi gì.** Điểm khi vào `TIE_BREAK` — chỉ ghi nhận trạng thái hoà · danh sách câu chưa dùng · cài đặt playlist.

**Thứ tự đánh giá.** **(1)** admin bấm Chốt trận → **(2)** server tính điểm tích luỹ tại mốc đó → **(3)** tìm nhóm cao nhất → **(4)** đối chiếu `tieBreakPositions` và số người ≥ 2 → **(5)** rẽ `TIE_BREAK` hoặc `FINISHED`.

**Biên.** Nhóm hoà **2** người: kích hoạt. Nhóm hoà **4** người (toàn sân): kích hoạt nếu config mở. Không hoà: không kích hoạt.

**Bấm trùng.** Nút Chốt trận **một chiều, tự tắt** ⇒ không có lần bấm thứ hai.

**Ví dụ.**

- A=100, B=100, C=90, D=85 ⇒ `TIE_BREAK` cho A, B.
- A=100, B=100, C=100, D=85 với `tieBreakPositions=[1,2]` ⇒ `TIE_BREAK` cho A, B, C.
- A=110, B=100, C=90 ⇒ `FINISHED`.
- A=B=C=100 nhưng config `[1]` ⇒ `FINISHED`, ghi 3 người đồng hạng.

**Nguồn**: luật gốc §Câu hỏi phụ *"các thí sinh có cùng số điểm sẽ bước vào phần thi Câu hỏi phụ"* · `QĐ-036`, `QĐ-037`, `QĐ-038`, `QĐ-042`

---

## GR-023 — Câu hỏi phụ: thể thức ba câu

**Mục đích.** Cơ chế trả lời, thời gian và kết quả của ba câu hỏi phân định.

**Kích hoạt.** Admin bắt đầu vòng `TIE_BREAK`.

**Điều kiện.**

- **Ba câu**, mỗi câu **15 giây** suy nghĩ — cả hai con số **cố định**, không cấu hình.
- Bấm chuông giành quyền; chuông **chỉ nhận click chuột**. Hàng đợi **không chặn** — tính ngay theo server timestamp.
- **Không cộng, không trừ điểm.** Kết quả chỉ đổi **thứ hạng**.
- **Có người giành được quyền ⇒ đồng hồ 15 giây DỪNG NGAY**, không chạy tiếp. Cửa sổ 15 giây là cửa sổ **suy nghĩ + giành quyền**; trả lời sai thì **cả nhóm sang câu kế**, không còn ai để đếm giờ cho.
- **Không có đồng hồ trả lời riêng sau khi giành quyền.** Nguồn im lặng **có chủ ý**: nó nói rõ *"tính từ lúc giành được quyền"* ở Khởi động lượt chung và *"suy nghĩ **và trả lời**"* ở Về đích, nhưng ở Câu hỏi phụ **chỉ ghi** *"Thời gian suy nghĩ cho mỗi câu hỏi là 15 giây"*, và **không** có chế tài cho việc bấm chuông rồi im lặng — khác hẳn −5 của Khởi động. Bấm rồi im ⇒ admin chấm **Sai** ⇒ sang câu kế. **Không phát minh cửa sổ trả lời cho vòng này.**

**Nguồn đề — vòng này KHÔNG có kho riêng**

- Đề rút từ **ba kho nguồn**: **Về đích · Khởi động · VCNV**, trong danh sách đã gán, loại câu đã dùng như mọi vòng.
- **Thứ tự ưu tiên rút: Về đích → Khởi động → VCNV.** Kho Về đích **luôn dư đúng 12 câu** sau khi vòng chạy xong *(pre-flight đòi 24, vòng tiêu 12 — `GR-017`)*, nên nó là nguồn đúng. Kho VCNV khan nhất và rút vào đó **làm vỡ một bộ**.
- **Kho Tăng tốc KHÔNG phải nguồn.** Câu ở đó là *nhìn nhanh · sắp xếp · suy luận · đoạn băng*, dựng cho **cả sân cùng gõ máy** và tính theo **thứ hạng tốc độ**; đặt vào một cửa sổ tranh chuông 15 giây là hỏng, và ở mode sân khấu thì **không có gì để nói**.
- **Câu `isPractical` bị LOẠI khỏi phép rút** — nó có hai pha thời gian và cần giới thiệu dụng cụ, không có đường chạy trong khuôn 15 giây.
- **Metadata của kho gốc bị vô hiệu hoá**: `timeSeconds` **bỏ qua**, luôn **15 giây** · `value` **bỏ qua**, vòng này không sinh điểm · phân loại theo vòng **không mang ý nghĩa nào**.
- **Admin chỉ định một số câu làm Câu hỏi phụ được — TẠI `LOBBY`**, không phải ở cấu hình trận *(cấu hình trận đã đóng băng từ cú bấm bắt đầu trận)*. Bốn ràng buộc:
  - Chỉ nhận câu **còn available** — chưa hiển thị. Câu **đã hiển thị** không chỉ định được, cùng ranh giới `GR-031` C6.
  - Làm được ở **bất kỳ `LOBBY` nào**; hạn chót là **mốc bấm Chốt trận**, tức trước khi vào vòng.
  - **Gỡ chỉ định được**, đối xứng; câu gỡ ra quay lại phép rút của vòng gốc.
  - Chỉ định là **ĐẶT CHỖ có hiệu lực TỪ THỜI ĐIỂM CHỈ ĐỊNH**: câu đó bị loại khỏi phép rút của **các vòng mở sau đó**. ⇒ Chỉ định ở `LOBBY` **trước** Về đích tốn **+k** vào kho Về đích; chỉ định ở `LOBBY` **cuối** tốn **0**, vì không còn vòng nào đụng tới 12 câu dư.
- **Chỉ định ít hơn 3 câu** ⇒ danh sách chỉ định tiêu trước, **phần thiếu bù theo thứ tự ưu tiên** ở trên. Không chặn cứng. Không chỉ định gì ⇒ rút hoàn toàn theo thứ tự ưu tiên.
- **Cấm** để `timeSeconds` hoặc `value` của câu mượn chi phối vòng này; **cấm** lấy câu từ kho Tăng tốc; **cấm** rút câu `isPractical`.

**Bảng quyết định**

| Ca | Điều kiện | Kết quả | Thay đổi trạng thái |
|---|---|---|---|
| C1 | A bấm sớm nhất, admin chấm **Đúng** | A **thắng** phân định | `TIE_BREAK` → `LOBBY`; A giữ vị trí đang tranh |
| C2 | A bấm, admin chấm **Sai** | **Cả nhóm sang câu kế** | Hàng đợi reset; câu kế mở |
| C3 | Hết 15 giây, không ai bấm | Sang câu kế | Câu đóng, không ai đúng |
| C4 | Hết cả **3 câu**, chưa ai đúng | **Bốc thăm** — `GR-025` | Chuyển pha bốc thăm |
| C5 | Admin bỏ vòng giữa chừng (`GR-030`) | Vòng đóng theo kết quả hiện có | Xem `GR-030` |

**Không đổi gì.** Điểm của **mọi** thí sinh · thứ hạng của người ngoài nhóm hoà · câu đã dùng **không** trả lại kho.

**Thứ tự đánh giá.** **(1)** admin bấm hiển thị câu, MC đọc → **(2)** admin **start timer**, 15 giây → **(3)** thí sinh bấm, server ghi timestamp → **(4)** có người giành quyền: **timer dừng** → **(5)** admin chấm → **(6)** đúng thì kết thúc vòng, sai hoặc hết giờ thì sang câu kế → **(7)** hết 3 câu chưa ai đúng: sang `GR-025`.

**Biên.** Thời gian **15 giây**, số câu **3**, nhóm tối thiểu **2** người — cả ba đều cứng. **15 giây thắng `timeSeconds` của câu mượn** — xem §Nguồn đề.

**Đồng thời.** Hai người bấm cùng mốc ⇒ hàng đợi tự quyết, ngẫu nhiên; thứ tự đó được ghi lại.

**Ví dụ.**

- Câu 1 hết giờ không ai bấm → câu 2, C bấm ở giây 7, chấm **Đúng** ⇒ C thắng.
- Câu 1 A bấm sai → câu 2 B bấm đúng ⇒ B thắng.
- Cả 3 câu không ai đúng ⇒ bốc thăm.
- Admin chấm **Sai** nhầm: nút chấm **tự khoá sau lần bấm đầu**; muốn sửa thì đi qua hoàn nguyên (`GR-028`) hoặc điều chỉnh thủ công (`GR-029`).

**Nguồn**: luật gốc §Câu hỏi phụ *"Các thí sinh trả lời 3 câu hỏi. Thời gian suy nghĩ cho mỗi câu hỏi là 15 giây… Nếu trả lời sai, các thí sinh sẽ bước sang câu hỏi tiếp theo."* · `QĐ-020`, `QĐ-025`, `QĐ-031`, `QĐ-055`, `QĐ-060`, `QĐ-081`

---

## GR-024 — Câu hỏi phụ: chuông không sống trước hiệu lệnh

**Mục đích.** Bảo đảm *không ai được lợi thế do bấm chuông trước hiệu lệnh* — mục tiêu mà luật gốc đạt bằng **hình phạt**, còn bản phần mềm đạt bằng **khử tình huống**.

**Kích hoạt.** Cửa vào mỗi câu của vòng Câu hỏi phụ.

**Điều kiện.**

- Nút chuông ở Câu hỏi phụ **không sống trước mốc admin bấm start timer** ⇒ **không tồn tại** tín hiệu bấm sớm.
- **Zero-trust**: tắt nút chỉ là UX — **server vẫn từ chối** mọi tín hiệu chuông tới trước mốc start ở vòng này, và ghi `AuditLog`.
- **KHÔNG áp cho Khởi động lượt chung**: ở đó luật gốc cho phép *"bấm chuông trong khi người dẫn chương trình đang đọc câu hỏi"*, nên chuông sống từ mốc **hiển thị câu**.

**Bảng quyết định**

| Ca | Điều kiện | Kết quả |
|---|---|---|
| C1 | Bấm chuông **trước** mốc start timer | Nút chưa render ⇒ **không có tín hiệu**. Server từ chối tín hiệu giả mạo, ghi `AuditLog` |
| C2 | Bấm chuông **từ mốc start timer trở đi** | **Hợp lệ** — vào hàng đợi, chấm bình thường (`GR-023`) |
| C3 | Bấm sau khi hết 15 giây | **Không có tín hiệu** — cùng cơ chế C1 |

**Không đổi gì.** Điểm · lượt · kết quả các câu khác — mỗi câu độc lập.

**Biên.** Ranh giới là **mốc admin bấm start timer**, đóng ở phía trước: đúng mốc là **hợp lệ**, một ms trước là không có nút.

**Vì sao không giữ cơ chế phạt của luật gốc.** Luật gốc buộc phải phạt vì trên trường quay chuông là **phần cứng, lúc nào cũng sống**; ở bản phần mềm ràng buộc đó không tồn tại. Chặn thì **tuyệt đối**, phạt thì chỉ răn đe — nên bản này bảo đảm mục tiêu **mạnh hơn** nguyên bản.

**Ví dụ.**

- *Hợp lệ*: admin start timer, A bấm ngay ms sau ⇒ A giành quyền.
- *Không tồn tại*: A bấm khi MC còn đang đọc ⇒ máy A không có nút để bấm.
- *Không hợp lệ*: client tự chế gửi tín hiệu chuông sớm ⇒ server từ chối, ghi log.

**Nguồn**: luật gốc §Câu hỏi phụ *"nếu có thí sinh bấm chuông trả lời trước khi có hiệu lệnh của người dẫn chương trình, thí sinh đó sẽ bị mất quyền trả lời câu hỏi"* · `QĐ-054`, `QĐ-028`

---

## GR-025 — Câu hỏi phụ: hết câu chưa phân định

**Mục đích.** Cơ chế khi hỏi hết ba câu mà vẫn chưa xác định được người thắng.

**Kích hoạt.** Câu thứ ba đóng, không ai trả lời đúng.

**Điều kiện.**

- Đủ **3 câu** đã hỏi; **0** người đúng — sai hết, hết giờ hết, hoặc pha trộn cả hai.
- Nhóm hoà **≥ 2** người.
- `exhaustedFallback = 'random-draw'` — giá trị hợp lệ duy nhất ở v1.
- Kết quả bốc thăm là **event trong log**, không có khái niệm *"huỷ"*: sửa chỉ bằng cách **thêm event sau**.

**Bảng quyết định**

| Ca | Điều kiện | Kết quả |
|---|---|---|
| C1 | 3 câu chưa ai đúng, nhóm ≥ 2 người | Server đề xuất **một** người bằng bốc thăm ngẫu nhiên |
| C2 | Admin bấm **Yes** trên kết quả | Người đó nhận vị trí đang tranh; vòng đóng, trận về `LOBBY` |
| C3 | Admin bấm **Bốc lại** | Bốc thăm lần nữa. **Cả hai lần đều là event thật** trong log append-only; **lần cuối cùng có hiệu lực**, lần trước **không** bị đánh dấu vô hiệu — nó chỉ bị một event sau ghi đè |
| C4 | **Không đủ 3 câu khả dụng** từ ba kho nguồn | **Vòng không mở được** — chặn ở cửa vào vòng. Không tồn tại ca cạn giữa vòng. Ca này **gần như không chạm tới**: kho Về đích luôn dư đúng 12 câu sau vòng Về đích (`GR-017`, `QĐ-081`) |
| C5 | Admin **không** phân định một nhóm | Các thành viên nhóm đó ghi **đồng hạng**, theo standard competition ranking |

**Không đổi gì.** Điểm — vòng này không cộng trừ · lịch sử 3 câu · thứ hạng của người ngoài nhóm.

**Thứ tự đánh giá.** **(1)** câu 3 đóng, kiểm tra có ai đúng không → **(2)** không: server bốc → **(3)** hiện kết quả cho admin dưới dạng **đề xuất** → **(4)** admin bấm Yes hoặc Bốc lại → **(5)** chốt, cập nhật thứ hạng → **(6)** vòng đóng.

**Biên.** Nhóm **2** người: bốc. Nhóm **4** người: bốc, nếu config mở tới đó. **Dưới 3 câu khả dụng** từ ba kho nguồn *(kể cả sau khi bù cho danh sách chỉ định)*: vòng không mở.

**Bấm trùng.** Nút bốc thăm **một chiều, tự tắt**. *"Bốc lại"* là **thao tác riêng, có dialog Yes/No**, sinh **event mới** — không phải bấm trùng nút cũ. Thao tác này **không idempotent theo thiết kế**: mục đích của nó chính là ra kết quả khác. Chống bấm nhầm nằm ở nút tự tắt + dialog, **không** ở dedup phía server.

**Ví dụ.**

- A=100, B=100, 3 câu chưa ai đúng ⇒ bốc ⇒ B nhận vị trí NHẤT.
- Bốc ra A, admin bấm **Bốc lại**, lần 2 ra B ⇒ B có hiệu lực; **cả hai lần** nằm trong log.
- Ba người hoà với config `[1,2,3]` ⇒ bốc ra C ⇒ C vị trí 1, A và B đồng hạng dưới C.

**Nguồn**: luật gốc §Câu hỏi phụ *"Sau 3 câu hỏi, nếu không tìm được thí sinh thắng cuộc, các thí sinh sẽ phải bốc thăm để chọn ra thí sinh thắng cuộc."* · `QĐ-011`, `QĐ-042`, `QĐ-055`, `QĐ-060`

---

## GR-026 — Phán quyết của admin

**Mục đích.** Ấn định ai quyết Đúng/Sai và điều gì làm điểm sinh ra.

**Kích hoạt.** Một câu có đáp án để chấm — thí sinh gửi bài, nói trên sân khấu, hoặc hết giờ mà không trả lời.

**Điều kiện.**

- **Điểm chỉ chốt khi ADMIN bấm Đúng/Sai.** Không có nhánh nào máy tự cộng trừ.
- Máy chỉ làm hai việc: **hiển thị bài làm cạnh đáp án**, và **tô nổi bật chỗ khác** (`GR-027`). Cả hai là **gợi ý**, không ràng buộc admin.
- Phán quyết **không phụ thuộc kênh trả lời**: nói miệng, gõ máy hay thực hành đều do admin chấm. Kênh chỉ quyết định **có gì để tô** hay không.
- Phán quyết là **điều kiện để sang câu kế** ⇒ không tồn tại trạng thái *"vòng đã đóng mà còn câu chưa chấm"*.
- **Một câu đi qua ĐÚNG MỘT phán quyết.** Chấm nhầm thì sửa bằng **điều chỉnh tay** (`GR-029`), không chấm lại.

**Bảng quyết định**

| Ca | Điều kiện | Kết quả |
|---|---|---|
| C1 | Admin bấm **Đúng** | **+giá trị câu**; sinh event phán quyết |
| C2 | Admin bấm **Sai** | **−hình phạt** hoặc **0**, tuỳ vòng; sinh event phán quyết |
| C3 | Admin **chưa bấm** | **Không sinh điểm.** Câu treo ở trạng thái chưa chấm; gợi ý so khớp **không có hiệu lực** |
| C4 | Admin bấm **chốt câu** khi mới chấm một phần — chỉ ở **Tăng tốc** và **câu hàng ngang VCNV** | **Mọi ghế chưa chấm ⇒ SAI.** Tăng tốc: ghế đó nhận **0** và **không giữ chỗ** trong thang 40/30/20/10, vẫn là **MỘT** event cho toàn bảng. Hàng ngang VCNV: nhận **0** và **KHÔNG bị loại** |
| C5 | Tín hiệu **"Mở chướng ngại vật"** đang chờ duyệt, admin bấm chốt câu | **KHÔNG bị mặc định SAI** — đó là phán quyết riêng lẻ, có chủ đích, và SAI ở đó **loại thí sinh**. Không tồn tại đường nào để một cú bấm chốt câu loại một thí sinh |

> **C4 không phải máy tự chấm.** Cú bấm *"chốt câu"* **chính là phán quyết**: nó phát biểu *"những ai tôi chưa chấm ⇒ SAI"*. **Không** có bộ đếm nào tự chốt khi hết giờ.

**Không đổi gì.** Bài làm của thí sinh — append-only, không bao giờ bị xoá · kết quả tô nổi bật **không** đụng tới phán quyết · điểm của người không liên quan tới câu.

**Thứ tự đánh giá.** **(1)** nhận bài làm, ghi timestamp và ghế → **(2)** chuẩn hoá và tô nổi bật → **(3)** admin đọc, cân nhắc, bấm → **(4)** sinh event, điểm cập nhật.

**Biên.** Chấm **sau khi** cửa sổ thời gian đã đóng vẫn thực hiện được — đồng hồ khoá thí sinh, không khoá admin.

**Bấm trùng.** Nút chấm **tự khoá sau lần bấm đầu** ⇒ không có lần thứ hai ở giao diện; server vẫn phải bỏ qua tín hiệu trùng.

**Đồng thời.** Không xảy ra — **mỗi contest chỉ có MỘT admin**, nên không có hai luồng thao tác admin song song.

**Ví dụ.**

- A gửi `"Huế"`, admin bấm **Đúng** ⇒ **+10**.
- A bấm chuông ở lượt chung rồi trả lời sai, admin bấm **Sai** ⇒ **−5**.
- A gửi bài, hết giờ, admin đang bận ⇒ câu **chưa chấm**; viewer không thấy điểm nào cho câu đó.

**Nguồn**: `QĐ-001`, `QĐ-008`, `QĐ-010`, `QĐ-014`, `QĐ-030`, `QĐ-053`, `QĐ-060`

---

## GR-027 — Chuẩn hoá và tô nổi bật đáp án

**Mục đích.** Xử lý bài làm để **admin dễ nhìn ra chỗ khác** — không phải để máy kết luận.

**Kích hoạt.** Nhận được một bài làm dạng chữ.

**Điều kiện.**

- **Bắt buộc**: không phân biệt hoa thường · **cắt khoảng trắng đầu cuối** · **gộp khoảng trắng thừa**.
- **Tuỳ chọn**: bỏ dấu tiếng Việt (`stripDiacritics`, mặc định **TẮT**).
- Chuẩn hoá là **hàm thuần, tất định** — cùng đầu vào cho cùng kết quả.
- Bài làm **gốc** được lưu; kết quả chuẩn hoá là **tạm thời**, chỉ để hiển thị.
- `acceptedAnswers` cố định từ lúc soạn câu, **không sửa giữa trận**.

**Bảng quyết định**

| Ca | Bài làm vs đáp án | Máy hiển thị |
|---|---|---|
| C1 | `"  HUẾ  "` vs `"Huế"` | **Không có khác biệt** sau chuẩn hoá — *"khớp đúng chính tả"* |
| C2 | `"Hué"` vs `"Huế"`, bỏ dấu **TẮT** | Tô **ký tự khác** ở vị trí dấu |
| C3 | `"Hue"` vs `"Huế"`, bỏ dấu **BẬT** | **Không tô** — dấu bị bỏ qua khi so |
| C4 | `"Thành phố Huế"` vs `"Huế"` | Tô **từ thừa** |
| C5 | Rỗng hoặc không gửi | **Không so, không tô** — *"không có đáp án"* |

> **`stripDiacritics` không hề chỏi với luật gốc.** Nguồn nói *"bất kỳ sai sót về kí tự, dấu câu, ngữ pháp → không được công nhận"* — đó là câu về **PHÁN QUYẾT**. `stripDiacritics` chỉ đổi **CÁCH TÔ MÀU**. Vì máy không phán quyết, hai phát biểu **không cùng đối tượng**: bật nó nghĩa là *"đừng tô đỏ chỗ khác dấu"*, **không** nghĩa là *"công nhận đúng"*. Admin vẫn chấm Sai nếu muốn.

**Không đổi gì.** Bài làm gốc · `acceptedAnswers` · phán quyết — tô màu **không bao giờ** sinh điểm.

**Thứ tự đánh giá.** **(1)** hạ hoa thường → **(2)** cắt đầu cuối → **(3)** gộp khoảng trắng → **(4)** bỏ dấu nếu bật → **(5)** so với từng phần tử `acceptedAnswers` đã chuẩn hoá → **(6)** tính khác biệt **theo ký tự** → **(7)** hiển thị bài làm cạnh đáp án kèm tô nổi bật.

**Biên.** `"Huế"` = `"huế"` = `"HUẾ"` = `"  Huế  "` = `"Huế   Huế"` gộp về một dấu cách. Ngoại lệ *"ý nghĩa tương đồng"* **không mã hoá được** — admin tự đánh giá.

**Ở mode sân khấu.** Không có bài làm dạng chữ ⇒ **không có gì để tô**. Các quy tắc *ghi nhận bài đầu/cuối*, *timestamp*, *tô nổi bật* **không bị xoá, chỉ không được kích hoạt**.

**Ví dụ.**

- `"  Huế  "` vs `"Huế"` ⇒ không tô; admin bấm Đúng.
- `"Hue"` vs `"Huế"`, bỏ dấu TẮT ⇒ tô ký tự khác; admin nghe MC rồi quyết.
- `"Thành phố Huế"` vs `"Huế"` ⇒ tô hai từ thừa; admin quyết theo yêu cầu của câu.

**Nguồn**: luật gốc §VCNV *"bất kỳ sai sót về kí tự, dấu câu, ngữ pháp → không được công nhận"* · §Tăng tốc *"đúng chính tả; ý nghĩa tương đồng được chấp nhận"* · `QĐ-010`, `QĐ-016`

---

## GR-028 — Điểm là hàm của event log

**Mục đích.** Ấn định mô hình dữ liệu điểm: điểm là **kết quả tính từ nhật ký sự kiện**, không phải một con số bị sửa tại chỗ.

**Kích hoạt.** Bất kỳ thao tác nào phát sinh điểm — chấm, điều chỉnh tay, bỏ vòng.

**Điều kiện.**

- Nhật ký **append-only**: mỗi thao tác **thêm** event, không bao giờ xoá hay sửa event cũ.
- **Điểm = reduce(event log)** — thuộc tính tính toán, không phải cột số.
- **Hoàn nguyên = THÊM event đảo ngược**, kiểu `git revert`, không phải quay về ảnh chụp.
- Phép tính **tất định**: cùng chuỗi event luôn cho cùng điểm, nên phát lại và soi lại được ở bất kỳ mốc thời gian nào.

**Bảng quyết định**

| Ca | Thao tác | Event sinh ra | Điểm sau đó |
|---|---|---|---|
| C1 | Chấm **Đúng** câu 10đ | `JUDGE / CORRECT / +10` | 0 → **10** |
| C2 | Chấm **Sai** câu có phạt −5 | `JUDGE / INCORRECT / −5` | 10 → **5** |
| C3 | Điều chỉnh tay **+3**, có lý do | `SCORE_ADJUST / +3 / reason` | 5 → **8** |
| C4 | **Bỏ vòng** | **N** event đảo ngược, một cho mỗi event điểm của vòng đó | Tính lại từ đầu |
| C5 | Hỏi *"điểm lúc 14:05 là bao nhiêu"* | **Không sinh event** | `reduce(các event tới 14:05)` |

**Không đổi gì.** Event cũ — không xoá, không sửa, không đảo thứ tự · bài làm của thí sinh · điểm **không bao giờ** bị ghi đè trực tiếp.

**Thứ tự đánh giá.** **(1)** admin thao tác → **(2)** sinh event, nối vào nhật ký → **(3)** tính lại điểm → **(4)** phát cho client hiển thị.

**Biên.** Không có **sàn** — điểm âm hợp lệ. Không có **trần**.

**Bấm trùng.** `reduce` trên cùng mảng event luôn cho cùng kết quả; gọi lại bao nhiêu lần cũng không đổi gì.

**Đồng thời.** Không xảy ra — **một admin cho mỗi contest**.

**Ví dụ.**

- `+10` → `−5` → `+3` ⇒ điểm **8**, và **cả ba** event còn nguyên trong nhật ký.
- Bỏ vòng VCNV sau khi đã có 5 event: thêm các event đảo ngược cho những event thuộc vòng đó; điểm tính lại; **không** event nào biến mất.
- Xem lại lúc `14:05:30`: `reduce` các event có timestamp ≤ mốc đó.

**Nguồn**: `QĐ-008`, `QĐ-011`, `QĐ-012`, `QĐ-013`, `QĐ-035`

---

## GR-029 — Điều chỉnh điểm thủ công

**Mục đích.** Cho admin cộng trừ một lượng điểm bất kỳ kèm **lý do**, độc lập với mọi phán quyết về đáp án.

**Kích hoạt.** Admin bấm **Chỉnh điểm tay**.

**Điều kiện.**

- **Lý do là bắt buộc.** Không lý do thì không sinh event.
- Làm được **ở mọi lúc trận chưa đóng sổ** — cả ở `LOBBY` lẫn giữa một vòng đang chạy. **Trừ `FINISHED`**: trận đã chốt thì **niêm phong**.
- **Không tự hoàn nguyên khi bỏ vòng.** Đây là phán quyết **của người**, không thuộc vòng nào.
- Là thao tác **không hoàn tác được** ⇒ **dialog Yes/No**, nút xác nhận **tự tắt sau khi bấm**.

**Bảng quyết định**

| Ca | Điều kiện | Kết quả |
|---|---|---|
| C1 | delta **+5**, lý do *"sửa nhầm câu 2"* | Sinh `SCORE_ADJUST`; điểm ghế đó **+5**; ghi audit kèm admin, lý do, mốc thời gian |
| C2 | delta **−10** | Điểm **−10**, **được phép âm**, không có sàn |
| C3 | delta **= 0**, có lý do | **VẪN SINH EVENT.** Điểm không đổi nhưng ô **lý do** là **kênh duy nhất** ghi được xuất xứ một quyết định — một điều chỉnh delta 0 kèm lý do chính là **ghi chú chính thức vào biên bản trận**. Vứt nó đi là vứt mất dữ liệu phân xử |
| C4 | Điều chỉnh **rồi** bỏ vòng | Điều chỉnh **vẫn giữ**; chỉ event của vòng bị đảo ngược |
| C5 | Bỏ vòng **rồi** điều chỉnh | Điều chỉnh có hiệu lực bình thường |
| C6 | Trận đã ở `FINISHED` — **cả hai nhãn**, *hoàn thành* lẫn *bỏ dở* | **Không thực hiện được**: nút không bật, admin thấy toast *invalid state*, **không ép được**. Phát hiện sai sót sau khi chốt thì ghi ở **trận mới**, không sửa ngược biên bản đã đóng |

> **Điều chỉnh "mồ côi" sau khi bỏ vòng là hành vi ĐÚNG.** Bỏ hoặc chạy lại vòng hoàn nguyên các trạng thái sinh bởi **event CỦA VÒNG ĐÓ**; `SCORE_ADJUST` là event **của admin**, không thuộc vòng nào ⇒ nằm ngoài phạm vi hoàn nguyên. Muốn bỏ nó thì hoàn nguyên **chính nó**, như mọi event khác.

**Không đổi gì.** Event cũ · phán quyết của các câu · điểm của ghế khác.

**Thứ tự đánh giá.** **(1)** bấm Chỉnh điểm tay → **(2)** nhập delta và **lý do** → **(3)** chọn ghế → **(4)** dialog Yes/No → **(5)** sinh event, tính lại điểm → **(6)** ghi audit.

**Biên.** Delta âm hợp lệ, không có sàn. Lý do không giới hạn độ dài. Số lần điều chỉnh **không giới hạn** — mỗi lần một event.

**Bấm trùng.** Thao tác này **KHÔNG dedup**, và đó là **chủ đích**: hai lần `+5` liên tiếp là một yêu cầu hợp lệ (tổng `+10`), máy không có cách nào phân biệt nó với cú bấm trùng do trễ mạng. Chống bấm nhầm nằm ở giao diện: nút một chiều tự tắt · dialog Yes/No · **bắt nhập lý do** — một bước gõ tay khiến bấm trùng do trễ gần như không xảy ra.

**Đồng thời.** Không xảy ra — một admin cho mỗi contest; nhiều lần điều chỉnh cộng dồn theo thứ tự bấm.

**Ví dụ.**

- Chấm Sai câu 2 (`−5`), sau đó MC nói đúng ⇒ chỉnh `+5`, lý do *"sửa nhầm câu 2"*.
- C bấm sai ở lượt chung (`−5`), MC yêu cầu trừ thêm ⇒ chỉnh `−5` nữa, điểm C = **−10**.
- Bỏ VCNV (hoàn nguyên `−60`) rồi chỉnh `+10` ⇒ điểm `−50`; điều chỉnh **không** bị cuốn theo.

**Nguồn**: `QĐ-011`, `QĐ-012`, `QĐ-037`, `QĐ-039`, `QĐ-060`

---

## GR-030 — Bỏ vòng, chạy lại vòng, kết thúc sớm

**Mục đích.** Ba cửa ra chủ động của một vòng đang chạy, bên cạnh nút *Kết thúc vòng* thường.

**Kích hoạt.** Admin chọn **Bỏ vòng** · **Chạy lại vòng** · **Kết thúc sớm**.

**Điều kiện.**

- Cả ba thuộc **hạng phá huỷ**: dialog **không tắt được** + **bắt nhập lý do**.
- **Bỏ** ⇒ điểm của vòng **hoàn nguyên** bằng event đảo ngược; biên bản giữ vòng đó với nhãn **"đã bỏ"**.
- **Chạy lại** ⇒ hoàn nguyên rồi mở lại vòng từ đầu; phải qua **cửa kiểm kho đề** như một vòng mới.
- **Kết thúc sớm** ⇒ **điểm GIỮ NGUYÊN, không hoàn nguyên** — đây là khác biệt duy nhất và cũng là toàn bộ lý do cửa này tồn tại.
- Cả ba **không** trả câu **đã hiển thị** về kho.
- `SCORE_ADJUST` **không** bị cuốn theo bất kỳ cửa nào (`GR-029`).

**Bảng quyết định**

| Ca | Điều kiện | Kết quả |
|---|---|---|
| C1 | **Bỏ** vòng Khởi động | Sinh event đảo ngược cho **mọi** event điểm của vòng; tính lại điểm; biên bản: vòng = **"đã bỏ"** |
| C2 | **Chạy lại** VCNV | Hoàn nguyên, đặt lại hàng đợi, mở lại vòng; admin chọn lại từ đầu bằng câu **khác** — kho chỉ giảm, không tăng. Thiếu câu thì cửa vào vòng chặn |
| C3 | **Kết thúc sớm** — vòng hỏng giữa chừng, **chưa hỏi đủ câu**, admin muốn đi tiếp mà **giữ điểm** | Vòng khép **ngay tại chỗ** về `LOBBY`. **Không** sinh event đảo ngược nào; dọn cờ phạm vi vòng; biên bản: vòng = **"kết thúc sớm"**. Câu đang mở khép bằng **Huỷ kết quả** — không sinh điểm cho ai, và **không** ép mọi ghế thành SAI |
| C4 | Muốn sang vòng B khi vòng A chưa xong | **Không có đường vòng → vòng.** Admin **rời A trước** — kết thúc sớm, bỏ, hoặc chạy lại — về `LOBBY` rồi mở B. Trong màn vòng **không có nút mở vòng khác**: đây là **invalid state**, không phải chặn cứng. **Thứ tự vòng vẫn do admin quyết** |
| C5 | Bấm **Bỏ vòng** lần thứ hai do trễ mạng | **Không có hiệu lực, điểm KHÔNG tụt gấp đôi.** Ba lớp độc lập: nút một chiều tự tắt · dialog phá huỷ + bắt nhập lý do nên không thể là cú bấm phản xạ · server từ chối lệnh bỏ cho một vòng **đã ở trạng thái đã bỏ** |

> **Đây là INVALID STATE, không phải dedup.** Vòng **đã bỏ** không còn là mục tiêu hợp lệ của lệnh bỏ, nên lệnh bị từ chối **bất kể** nó là bấm trùng hay bấm mới. Cùng cách này áp cho **chạy lại**: mỗi lần chạy lại là một **lần chạy mới có chủ đích**, không phải bấm trùng — nên nó không bị dedup, mà bị chặn bởi dialog phá huỷ và cửa kiểm kho đề.

**Không đổi gì.** Event cũ — event đảo ngược là **thêm vào**, không xoá · câu đã hiển thị vẫn tiêu · `SCORE_ADJUST` · biên bản vòng bị bỏ vẫn còn nguyên trong hồ sơ trận.

**Thứ tự đánh giá.** **(1)** admin chọn vòng và cửa ra → **(2)** dialog phá huỷ + nhập lý do → **(3)** *bỏ* / *chạy lại*: sinh event đảo ngược, tính lại điểm; *kết thúc sớm*: **bỏ qua bước này** → **(4)** *chạy lại*: qua cửa kiểm kho đề rồi mở lại vòng → **(5)** gắn nhãn vào biên bản.

**Biên.** Bỏ được **bất kỳ** vòng nào. Chạy lại **không giới hạn số lần**, miễn kho còn câu. Kết thúc sớm **không** có ràng buộc về số câu đã hỏi.

**Ở phía viewer.** Điểm đổi **đột ngột, không hiệu ứng** — có chủ đích.

**Đồng thời.** Không xảy ra — một admin cho mỗi contest.

**Ví dụ.**

- Điểm đang 40, bỏ Khởi động (đã cộng 30) ⇒ còn **10**.
- VCNV đã tiêu 4 câu, chạy lại ⇒ cần **4 câu khác**; kho chỉ còn 3 thì cửa vào vòng chặn.
- Bỏ Khởi động (`−30`) sau khi đã chỉnh tay `+5` ⇒ điểm `−25`; điều chỉnh giữ nguyên.
- Bỏ Tăng tốc rồi chạy lại ⇒ biên bản ghi vòng đó **hai lần**, một *"đã bỏ"* một *"đã chạy lại"*.

**Nguồn**: `QĐ-034`, `QĐ-035`, `QĐ-042`, `QĐ-044`, `QĐ-060`

---

## GR-031 — Rút đề và không lặp câu trong toàn contest

**Mục đích.** Cơ chế rút ngẫu nhiên câu hỏi và quy tắc **một câu chỉ hỏi một lần** trong phạm vi một contest.

**Kích hoạt.** Đầu mỗi lượt cần câu.

**Điều kiện.**

- **Người tạo contest PHẢI chọn danh sách câu trước khi start** — tìm kiếm toàn văn, lọc, sắp xếp trên kho rồi chụp vào cấu hình trận. Hệ thống **không** tự lấy đề.
- Server rút **ngẫu nhiên TRONG danh sách đã chụp**, loại những câu có `usedInContest = true`.
- **Ngoại lệ DUY NHẤT — vòng VCNV rút theo BỘ, không rút từng câu.** Một bộ gồm **1 Chướng ngại vật + 4 hàng ngang + 1 câu ô trung tâm**; hàng ngang là **gợi ý của chính Chướng ngại vật đó**, mang **số thứ tự cố định** ứng với một miếng ghép ở một góc cố định, nên **không hoán đổi được** giữa các bộ. Bộ chỉ khả dụng khi **cả sáu thành phần đều chưa dùng**; mất một thành phần là **vỡ bộ**. Xem `QĐ-082`.
- `usedInContest` là cờ **toàn contest**, xuyên nhiều trận, và **không bao giờ được đặt lại** — kể cả khi bỏ hoặc chạy lại vòng.
- **Kho đề kiểm tại cửa vào TỪNG VÒNG**, không phải một lần trước trận.
- Mỗi lần rút sinh event `QUESTIONS_DRAWN` ⇒ phát lại trận ra đúng thứ tự câu cũ.

**Ranh giới "đã dùng"**

> **"Đã dùng" = ĐÃ HIỂN THỊ cho thí sinh**, không phải *"đã chấm"* và không phải *"đã rút"*. Câu **bị bỏ qua sau khi đã hiện** vẫn tiêu. Câu **đã rút nhưng chưa hiện** là **chưa tiêu, trả lại kho**. Ranh giới trùng đúng mốc **admin bấm hiển thị câu hỏi** — cùng một mốc phục vụ cả `GR-026`, `GR-030` và rule này.

**Bảng quyết định**

| Ca | Điều kiện | Kết quả |
|---|---|---|
| C1 | Rút 4 câu từ kho 200, chưa câu nào dùng | Chọn ngẫu nhiên 4; sinh `QUESTIONS_DRAWN`; đặt cờ khi các câu đó **hiển thị** |
| C2 | Trận thứ hai của cùng contest, 4 câu trước đã dùng | Kho hiệu dụng **196** |
| C3 | Vòng cần 4 câu, kho còn **3** | **Vòng đó không mở được**; **các vòng khác vẫn mở bình thường**. Admin thấy thiếu bao nhiêu câu |
| C3b | Vòng **VCNV**, kho có 4 hàng ngang và 1 Chướng ngại vật nhưng **không thuộc cùng một bộ** | **Vòng không mở được** — phép kiểm đếm **bộ nguyên vẹn**, không đếm câu. Đủ số câu **không phải** điều kiện đủ |
| C3c | Một hàng ngang của bộ X đã bị dùng làm **Câu hỏi phụ** (`GR-023`) | **Bộ X vỡ**, không dùng cho vòng VCNV nữa dù 5 thành phần kia còn nguyên. Giao diện **cảnh báo trước** khi admin chỉ định một hàng ngang làm Câu hỏi phụ, nêu rõ bộ nào sẽ vỡ |
| C4 | Kho cạn **giữa** vòng | **KHÔNG TỒN TẠI** — nhu cầu của vòng là con số cố định, đã kiểm đủ tại cửa vào; câu bị bỏ qua nằm trong con số đó |
| C5 | Admin sửa danh sách gán **tại `LOBBY`** | **ĐƯỢC.** Kho đề kiểm lại sau khi sửa; vào audit; cờ `usedInContest` **không** bị đụng tới |
| C6 | Bớt một câu **đã hiển thị** | **KHÔNG TỒN TẠI** — câu đã tiêu không còn là mục tiêu của thao tác gỡ; admin thấy toast, **không ép được**. Cho gỡ thì gỡ-rồi-thêm-lại thành đường lách quy tắc không lặp |
| C7 | Bớt một câu **đã rút, chưa hiển thị** | **ĐƯỢC** — câu chưa tiêu, quay về kho |
| C8 | Sửa danh sách **giữa lúc một vòng đang chạy** | **KHÔNG.** Cửa sửa chỉ mở ở `LOBBY`. Trong vòng không có nhu cầu: số câu của vòng là con số cố định đã kiểm đủ tại cửa vào. Nhờ vậy tập câu khả dụng **bất biến suốt một vòng** ⇒ phát lại ra đúng kết quả cũ |
| C9 | Trận **practice trong một contest THẬT** | **Phép lọc ĐẢO CHIỀU**: kho của nó = **CHỈ những câu `usedInContest = true`**, tức chỉ câu **đã lộ** ở các trận thật trước. Hệ quả: trận practice **không tiêu thêm câu nào** và **không thể thấy đề chưa thi** ⇒ **không dùng contest thật để tổng duyệt trước trận được**. Contest chưa chạy trận nào thì kho này **rỗng** và cửa vào vòng tự chặn — đó là dạng chính xác của *"contest thật phải chạy ít nhất một lần mới practice được"*. Trong một **practice contest** thì không có phép đảo nào |

**Không đổi gì.** Cờ `usedInContest` khi bỏ hoặc chạy lại vòng · event `QUESTIONS_DRAWN` · danh sách gán trong suốt một vòng đang chạy.

**Thứ tự đánh giá.** **(1)** admin chọn câu vào danh sách gán → **(2)** cửa vào vòng kiểm đủ số câu vòng cần → **(3)** đầu lượt: truy kho trong danh sách gán, loại câu đã dùng → **(4)** rút ngẫu nhiên → **(5)** sinh `QUESTIONS_DRAWN` → **(6)** admin bấm hiển thị ⇒ **đặt cờ đã dùng**.

**Biên.** Tối thiểu **1** câu mỗi lượt. Phạm vi không-lặp là **toàn contest**.

**Bấm trùng.** Bấm rút lần nữa sinh **event riêng, không dedup** — và điều đó **an toàn**: câu của lần rút trước **chưa hiển thị** nên **chưa tiêu**, trả lại kho. Chống bấm nhầm vẫn ở nút một chiều tự tắt.

**Đồng thời.** Không xảy ra — một admin cho mỗi contest, nên hai lượt không thể cùng rút một lúc.

**Ví dụ.**

- Kho 100 câu, admin gán 60. Khởi động lượt riêng cần `6 câu × 4 thí sinh = 24` ⇒ cửa vào vòng cho qua. Rút 6 câu cho A, hiển thị hết ⇒ kho hiệu dụng còn 54.
- Trận 2 dùng danh sách gán khác; 4 câu của trận 1 vẫn bị loại.
- Vòng Khởi động lượt riêng đã mở được thì **chạy trọn vòng** — không có đường cạn giữa chừng.

**Nguồn**: `QĐ-039`, `QĐ-040`, `QĐ-041`, `QĐ-042`, `QĐ-043`, `QĐ-044`, `QĐ-082`

---

## GR-032 — Hàng đợi tín hiệu và xác nhận của admin

**Mục đích.** Mọi tín hiệu của thí sinh vào **một hàng đợi** theo server timestamp; **không có cơ chế drop**; từ chối một tín hiệu **không** làm thí sinh mất lượt.

**Kích hoạt.** Thí sinh bấm chuông, chọn hàng ngang, bấm *Mở chướng ngại vật*, hoặc gửi đáp án.

**Điều kiện.**

- Tín hiệu chỉ tồn tại **trong cửa sổ hợp lệ** của sự kiện. Ngoài cửa sổ, máy thí sinh **không hiển thị nút và không phản hồi** ⇒ **không có tín hiệu nào được tạo**.
- Hàng đợi **chỉ CHẶN ở VCNV**: chọn hàng ngang và *Mở chướng ngại vật* phải chờ admin **Yes/No**.
- Các vòng còn lại **không chặn**: có chuông là tính ngay theo timestamp; hàng đợi vẫn ghi thứ tự làm lưới an toàn.
- **Hàng đợi đang hoạt động** đặt lại theo **đích** của tín hiệu; **LỊCH SỬ tín hiệu KHÔNG BAO GIỜ xoá**.
- Tín hiệu bị **từ chối** ⇒ tín hiệu kế lên, **thí sinh không mất lượt**.

**Bảng quyết định**

| Ca | Điều kiện | Kết quả |
|---|---|---|
| C1 | VCNV, thí sinh chọn hàng ngang trong cửa sổ | Vào hàng đợi, **chờ duyệt** |
| C2 | Admin bấm **Yes** | Tín hiệu thực thi; tín hiệu kế lên |
| C3 | Admin bấm **No** | Tín hiệu bị bỏ; **thí sinh không mất lượt**; tín hiệu kế lên |
| C4 | Khởi động lượt chung, Về đích cướp quyền | **Không chờ duyệt** — tính ngay theo timestamp |
| C5 | Vòng kết thúc | Hàng đợi đang hoạt động **trống**; **lịch sử giữ nguyên** |
| C6 | Tín hiệu từ ghế **đã bị loại** ở VCNV | Máy thí sinh **không hiển thị gì**, bấm **không phản hồi** ⇒ không có tín hiệu, không vào hàng đợi |
| C7 | Xem lại sau khi đã từ chối | Tín hiệu bị từ chối **vẫn trong lịch sử**; admin xem lại và **gỡ lệnh cấm** nếu cần |

**Vòng đời tín hiệu.** Tín hiệu gắn với **ĐÍCH** của nó và **vô hiệu khi đích đóng** — chọn hàng ngang gắn với **lượt chọn**, trả lời gắn với **câu**, *Mở chướng ngại vật* gắn với **vòng**. Việc hàng đợi được đặt lại theo vòng hay theo câu là **hệ quả** của quy tắc này, không phải một quy tắc riêng.

**Không đổi gì.** Lịch sử tín hiệu · lượt của thí sinh bị từ chối · điểm.

**Thứ tự đánh giá.** **(1)** server nhận, gắn timestamp → **(2)** kiểm cửa sổ → **(3)** vào hàng đợi → **(4)** VCNV: chờ Yes/No; vòng khác: tính ngay → **(5)** hết vòng: hàng đợi đang hoạt động đặt lại, lịch sử giữ.

**Biên.** Cửa sổ chuông: **3 giây** Khởi động lượt chung · **5 giây** Về đích cướp quyền · **15 giây** Câu hỏi phụ. Đúng biên là **hợp lệ**; một ms sau là **không có nút để bấm**.

**Bấm trùng.** Không xảy ra — nút chuông **tự khoá ngay khi bấm**, trước khi gửi. Server vẫn phải bỏ qua tín hiệu trùng nếu nhận được. Từ chối là **idempotent**: tín hiệu đã bị từ chối không từ chối lần nữa.

**Đồng thời.**

- **VCNV** — admin duyệt tuần tự nên không có xung đột.
- **Vòng không chặn** — hai tín hiệu cùng mốc: hàng đợi **tự quyết, ngẫu nhiên**; thứ tự được ghi lại nên dựng lại được.
- **Hai loại tín hiệu trong hàng đợi chặn của VCNV** — chọn hàng ngang và *Mở chướng ngại vật* dùng **chung một hàng đợi**, xử lý **FIFO thuần theo timestamp**, **KHÔNG** ưu tiên theo loại. Thêm quy tắc ưu tiên sẽ tạo một tiêu chí mà **thí sinh không quan sát được**. Băng điểm Chướng ngại vật **không bị ảnh hưởng**: hàng đợi chặn nên tín hiệu đang chờ duyệt chưa làm tăng số hàng đã mở; băng chốt tại **mốc admin xác nhận** (`GR-009`).

**Ví dụ.**

- VCNV: A chọn hàng 1 ⇒ chờ duyệt ⇒ admin **Yes** ⇒ hàng 1 được hỏi.
- VCNV: B bấm *Mở chướng ngại vật* do bấm nhầm ⇒ admin **No** ⇒ B **không mất lượt**, tín hiệu kế lên.
- Khởi động lượt chung: A bấm ở 3,2 giây, B ở 3,5 giây ⇒ A giành quyền, không ai chờ duyệt.
- Biên: cửa sổ 3 giây — bấm đúng **3,00 s** hợp lệ; **3,01 s** thì không có nút.

**Nguồn**: `QĐ-020`, `QĐ-021`, `QĐ-022`, `QĐ-023`, `QĐ-024`, `QĐ-025`, `QĐ-029`

---

## GR-033 — Mốc thời gian do admin bấm

**Mục đích.** Mọi thời điểm mà luật gốc mô tả bằng **hành vi của MC** đều ánh xạ thành **một cú bấm của admin** — máy không quan sát được sân khấu, **admin là cảm biến**.

**Kích hoạt.** MC đọc xong câu · MC ra hiệu lệnh · câu cần lên màn hình.

**Điều kiện.**

- **"Hiển thị câu hỏi" và "start timer" là HAI thao tác, thứ tự cố định**: hiển thị **trước**, start timer **sau**. Không đảo được, không gộp được.
  - **Hiển thị câu hỏi** — đưa câu lên màn thí sinh và viewer; **mở cửa sổ chuông**; **đóng** cửa sổ đặt Ngôi sao hy vọng; **tiêu câu khỏi kho**.
  - **Start timer** — chính là mốc *"MC đọc xong"*; bắt đầu đếm thời gian suy nghĩ.
  - Khoảng **giữa** hai mốc chính là lúc MC đọc — quãng mà luật cho phép bấm chuông ở Khởi động lượt chung.
- **Mốc admin là TUYỆT ĐỐI**: **không** cửa sổ ân hạn, **không** trừ bù độ trễ tay người. Van thoát không phải grace mà là **lịch sử đầy đủ** để admin xem lại và can thiệp.
- Đồng hồ **không bao giờ đóng băng** vì sự cố ngoài sân khấu.

**Bảng quyết định**

| Ca | Điều kiện | Kết quả |
|---|---|---|
| C1 | Admin bấm **start timer** | Đồng hồ chạy từ `timeSeconds` của câu; mốc ghi bằng server timestamp |
| C2 | Bấm **start timer** lần thứ hai | **KHÔNG XẢY RA** — nút tự khoá ngay sau lần bấm đầu ⇒ cửa sổ không thể bị kéo dài do thao tác lặp |
| C3 | Admin bấm **hiển thị câu** | Câu hiện; **cửa sổ chuông mở**; **cửa sổ Ngôi sao hy vọng đóng**; câu tiêu khỏi kho |
| C4 | Đã hiển thị nhưng chưa start timer | Câu đang hiện, **đồng hồ chưa chạy** — đúng thiết kế, đây là quãng MC đọc |
| C5 | **Câu hỏi phụ**: admin bấm mốc *hiệu lệnh* | Từ mốc này nút chuông mới sống (`GR-024`) |
| C6 | Có trễ mạng khi bấm | Server timestamp là mốc **tuyệt đối**; không cộng bù gì |

> **C2 là ngoại lệ DUY NHẤT** của nguyên tắc *"đồng hồ khoá thí sinh, không khoá admin"*: khoá này nhằm chống **bấm trùng**, không phải chống lệch luật.

**Không đổi gì.** Điểm · thứ tự các mốc đã ghi · lịch sử.

**Thứ tự đánh giá.** **(1)** admin bấm → **(2)** server ghi timestamp → **(3)** áp hệ quả của mốc → **(4)** chốt ngay, không cảnh báo, không ân hạn.

**Biên.** Cửa sổ tính **từ** mốc: 3 giây Khởi động lượt chung · 5 giây Về đích cướp quyền · 15 giây Câu hỏi phụ. Biên là **biên đóng** — đúng mốc vẫn tính.

**Bấm trùng.** Start timer: nút tự khoá. Hiển thị câu: chỉ lần đầu có hiệu lực.

**Đồng thời.** Không xảy ra — một admin cho mỗi contest.

**Ví dụ.**

- MC đọc xong *"Ai là tác giả Truyện Kiều?"*, admin bấm start timer ở `t=5,234 s` ⇒ đồng hồ chạy từ `timeSeconds` của câu.
- Admin bấm hiển thị ở `t=2,1 s`; thí sinh bấm Ngôi sao hy vọng ở `t=2,9 s` ⇒ **không có nút**, cửa sổ đã đóng. Admin bấm start timer ở `t=3,0 s`; cửa sổ chuông của Khởi động lượt chung **đã mở từ `t=2,1 s`** — đúng luật gốc: chuông sống trong lúc MC đọc.
- Khởi động lượt chung, start timer ở `t=10,0 s` ⇒ cửa sổ tới `t=13,0 s`; bấm đúng `13,000 s` **vẫn hợp lệ**.

**Nguồn**: `QĐ-006`, `QĐ-027`, `QĐ-028`, `QĐ-029`, `QĐ-030`, `QĐ-060`

---

## GR-034 — Chuông chỉ nhận click chuột

**Mục đích.** Tránh bấm nhầm chuông khi thí sinh đang gõ đáp án.

**Kích hoạt.** Thí sinh tác động lên nút chuông.

**Điều kiện.**

- Chuông **chỉ nhận click chuột**. **Không** gán phím nào cho chuông — không phải "gán rồi bỏ qua", mà là **không tồn tại** phím nào.
- Nút **"Mở chướng ngại vật"** của VCNV **được xếp là CHUÔNG** ⇒ cũng chỉ nhận click chuột.
- **Các hotkey khác giữ nguyên**: `Enter` gửi đáp án, `1`–`8` chọn hàng ngang, `Esc` xoá ô nhập.
- Khoá **theo LUẬT CHƠI** vẫn disable nút bình thường — đó là trạng thái game, không phải chống bấm trùng.

**Bảng quyết định**

| Ca | Điều kiện | Kết quả |
|---|---|---|
| C1 | Click chuột trong cửa sổ hợp lệ | Tín hiệu gửi đi, vào hàng đợi |
| C2 | Click *Mở chướng ngại vật* ở VCNV | Tín hiệu gửi, hàng đợi **chặn**, chờ admin duyệt |
| C3 | Gõ phím bất kỳ | **Không có tín hiệu chuông** — không phím nào được gán |
| C4 | Gõ `Enter` khi đang nhập đáp án | **Gửi đáp án**, không phải bấm chuông |
| C5 | Nút bị khoá theo luật chơi — đã trả lời sai, đã dùng hết quyền, chưa tới lượt | Nút ở trạng thái khoá; **không có tín hiệu nào được tạo** |
| C6 | Click sau khi cửa sổ đã đóng | Nút **không hiển thị và không phản hồi** ⇒ không có tín hiệu |

**Không đổi gì.** Các hotkey khác · điểm · lượt.

**Thứ tự đánh giá.** **(1)** nhận sự kiện click → **(2)** nút có đang bật không → **(3)** bật thì gửi tín hiệu kèm server timestamp.

**Biên.** **Không** hotkey cho chuông — con số là **không phím nào**. Cửa sổ: 3 giây · 5 giây · 15 giây tuỳ vòng, biên **đóng**.

**Bấm trùng.** Không xảy ra — nút **tự khoá ngay trong lần bấm đầu**, trước khi gửi. Sang câu mới nút mở lại: khoá gắn với **một câu**, không phải cả vòng.

**Đồng thời.** Hai người click cùng mốc ⇒ server timestamp phân xử; phía giao diện không có gì đặc biệt.

**Ví dụ.**

- Cửa sổ mở, A click ở `t=1,5 s` ⇒ tín hiệu vào hàng đợi.
- B đang gõ đáp án và vô tình chạm phím cách ⇒ **chuông không kêu**; ô nhập vẫn gõ bình thường.
- C gõ xong rồi `Enter` ⇒ **gửi đáp án**; muốn bấm chuông thì vẫn phải click.
- VCNV: D giải sai Chướng ngại vật ⇒ bị loại ⇒ máy D **không còn nút nào**.
- Biên: click đúng `3,000 s` **tính**; `3,001 s` thì không còn nút.

**Nguồn**: `QĐ-018`, `QĐ-023`, `QĐ-029`

---

## GR-035 — Server time là nguồn sự thật duy nhất

**Mục đích.** Timeout, thứ tự chuông và thứ hạng tốc độ đều tính theo **đồng hồ server**. Client chỉ hiển thị.

**Kích hoạt.** Mọi sự kiện có yếu tố thời gian.

**Điều kiện.**

- Hạn chót của một câu = **mốc tuyệt đối trên đồng hồ server**, không phải *"còn N giây"* ở client.
- Thứ tự chuông và thứ hạng tốc độ so trên **mốc server NHẬN được**, không phải mốc client gửi.
- **Đồng hồ đơn điệu** (monotonic) của tiến trình quyết định thứ tự và xếp hạng trong một trận; đồng hồ tường **chỉ dùng để hiển thị và ghi log** ⇒ chỉnh giờ hệ thống giữa trận **không thể** đảo thứ hạng đã ghi. Máy thí sinh không đồng bộ giờ **không ảnh hưởng gì**.
- Độ phân giải **mili-giây**; *"cùng lúc"* nghĩa là **cùng mili-giây**.
- Biên là **biên ĐÓNG**: tín hiệu đúng mốc thì **tín hiệu thắng**.

**Bảng quyết định**

| Ca | Điều kiện | Kết quả |
|---|---|---|
| C1 | Đồng hồ chạm hạn chót | Câu đóng; không nhận bài mới |
| C2 | Hai chuông **cùng mili-giây** ở vòng không chặn | **Hàng đợi tự quyết, ngẫu nhiên**; thứ tự được ghi lại |
| C3 | Tăng tốc: A nhận `5,123 s`, B nhận `5,124 s`, cùng đúng | A trên B |
| C4 | Tăng tốc: hai người **cùng mili-giây**, cùng đúng | **Cùng hạng**, cùng nhận mức điểm cao hơn |
| C5 | Client hiển thị chậm hơn server | **Server quyết**; hiển thị ở client chỉ là tham khảo |
| C6 | Bản gửi tới **sau** hạn chót | **Mặc định không tính** — nhưng **giữ lại và tô ĐỎ** trên màn admin, **cạnh** bản hợp lệ nếu có. Hiển thị ra ngoài và chấm điểm đều là **thao tác bấm của admin**; hệ thống chỉ đánh dấu, **không chặn cứng** |

**Không đổi gì.** Mốc đã ghi — không sửa lại về sau · thứ tự đã ghi trong hàng đợi.

**Thứ tự đánh giá.** **(1)** server nhận, ghi mốc → **(2)** so với hạn chót → **(3)** trong hạn thì xử lý theo luật vòng; quá hạn thì đánh dấu đỏ → **(4)** xếp hạng dùng mốc server nhận.

**Biên.** Đúng mốc là **hợp lệ** — cho **cả** tín hiệu giành quyền lẫn bài gửi. Chỉ khi **vượt quá** mốc mới mặc định không tính.

**Bấm trùng.** Tăng tốc nhận mọi lần gửi tới hết giờ và tính **bản cuối cùng**; thứ hạng dùng mốc nhận của **bản cuối**.

**Ví dụ.**

- Câu Khởi động lượt riêng 3 giây, start ở `10,000 s` ⇒ hạn `13,000 s`. Bài tới `12,998 s`: hợp lệ. Bài tới `13,002 s`: **quá hạn**, giữ lại và tô đỏ cho admin quyết.
- Tăng tốc: A và B cùng đúng ở `8,500 s` ⇒ **cùng hạng**, cùng nhận **40**.
- Tăng tốc: A gửi đúng ở `8,123 s` rồi gửi lại sai ở `8,456 s` ⇒ tính **bản cuối** (sai).

**Nguồn**: `QĐ-006`, `QĐ-025`, `QĐ-029`, `QĐ-059`

---

## GR-036 — Mất kết nối và giữ ghế

**Mục đích.** Cho một ghế mất kết nối cơ hội quay lại mà không mất gì, và ấn định rằng **quá hạn chờ thì máy CHỈ TÔ NỔI BẬT — admin là người quyết**.

**Kích hoạt.** Server phát hiện một ghế mất kết nối.

**Điều kiện.**

- Ngưỡng chờ mặc định **120 giây**, đo bằng **đồng hồ server**. Đây là **cấu hình duy nhất** còn lại ở rule này.
- **Quá ngưỡng: KHÔNG có chính sách tự động nào** — không tự loại, không tự xoá ghế. Ghế được **tô nổi bật trên màn admin** kèm thời lượng đã mất kết nối; admin chọn **giữ** hoặc **gia hạn**.
- **v1 KHÔNG có kick.** Thay bằng **vô hiệu hoá / kích hoạt lại**: **đảo ngược được**, dùng **mọi lúc** kể cả giữa một câu đang mở, ghế **ở lại trong trận** với nguyên điểm và nguyên vị trí, chỉ mất quyền thao tác.
- **Vô hiệu hoá là quyết định RIÊNG**, không phải hệ quả của mất kết nối — hai chuyện độc lập, hai nút khác nhau.
- Đồng hồ **không dừng** vì một ghế mất kết nối; vòng chạy tiếp bình thường.

> Đây **cùng một mẫu** với *"máy tô khác biệt ký tự, admin chấm"* và *"máy tô đỏ bản quá hạn, admin phán quyết"*: **máy độc quyền SỰ KIỆN** (đo và hiển thị thời lượng mất kết nối), **người độc quyền PHÁN QUYẾT**.

**Bảng quyết định**

| Ca | Điều kiện | Kết quả |
|---|---|---|
| C1 | Quay lại **trong** ngưỡng | Ghế khôi phục trạng thái; banner *đã kết nối lại* |
| C2 | **Quá** ngưỡng, admin chưa làm gì | Ghế **tô nổi bật** kèm thời lượng. **Không tự loại, không tự xoá**; ghế giữ nguyên trong trận |
| C3 | Admin phán quyết sau khi quá ngưỡng | **Hai** lựa chọn: **giữ** · **gia hạn**. Cả hai đều không phá gì, chỉ nói *"chờ tiếp"* |
| C4 | Quá ngưỡng **giữa một vòng đang chạy** | Vòng **chạy tiếp bình thường** — hệ thống không dừng vì một ghế mất kết nối |
| C5 | Admin **vô hiệu hoá** một ghế | Ghế **mất quyền thao tác** nhưng **ở lại trong trận**: nguyên điểm, nguyên vị trí, vẫn trên bảng điểm và bảng xếp hạng. Tín hiệu lỡ tới vào lịch sử nhưng **TRƠ**. Qua dialog Yes/No + lý do, vào audit |
| C6 | Admin **kích hoạt lại** | Thi tiếp bình thường. **KHÔNG hoàn nguyên gì** — điểm ghi trong lúc bị vô hiệu hoá giữ nguyên |
| C7 | Ghế bị vô hiệu hoá **giữa một câu đang mở** | Xử **y như ghế không trả lời**: mặc định **SAI** khi admin chốt câu. Thấy bất công ⇒ admin **cộng tay** (`GR-029`) |
| C8 | Quay lại **giữa một câu, đồng hồ đang chạy** | Client dựng lại **đúng màn đang thi**: câu đang mở · **hạn chót theo server time** · bản gửi gần nhất **của chính ghế đó** · các cờ ghế còn hiệu lực · bàn cờ VCNV · điểm công khai · lớp phủ đang bật. **Đồng hồ KHÔNG đặt lại** — mất kết nối **không mua thêm thời gian**. Gói khôi phục **không chứa đáp án** và **không chứa bài của ghế khác** |
| C9 | Admin can thiệp **trước** khi hết ngưỡng | **ĐƯỢC.** 120 giây là **khuyến nghị**, không phải ràng buộc cưỡng chế — admin gia hạn sớm, hoặc vô hiệu hoá sớm, đều được |
| C10 | Hai ghế cùng mất kết nối | Hai cửa sổ chờ **độc lập** |
| C11 | Hết ngưỡng **sau khi trận đã xong** | **Ghế được giữ.** Ngưỡng chỉ chi phối **quyền thao tác trong trận**; trận đã kết thúc thì không còn gì để thao tác. Bản ghi ghế thuộc **contest** và sống theo **retention của trận** |

**Không đổi gì.** Điểm đã ghi · lịch sử mất và nối lại · bài đã gửi và đã chấm — vô hiệu hoá **chỉ chặn tương lai**.

**Thứ tự đánh giá.** **(1)** phát hiện mất kết nối → **(2)** mở cửa sổ chờ 120 giây → **(3)** quay lại kịp: đồng bộ lại trạng thái → **(4)** quá hạn: **tô nổi bật**, chờ admin, **không áp hệ quả nào**.

**Biên.** Đúng `120,000 s` **vẫn trong ngưỡng** — biên đóng; quá mốc mới hết.

**Nối lại nhiều lần.** Ngưỡng tính **LẠI TỪ ĐẦU** kể từ lần mất kết nối **gần nhất**, và **không giới hạn số lần**. Cộng dồn thời gian các lần trước sẽ biến ngưỡng thành **hình phạt cho mạng yếu** — nguồn không có hình phạt nào như vậy; giới hạn số lần cũng vậy. Lạm dụng hay thiết bị lỗi xử lý bằng **can thiệp của admin**, không bằng luật cứng. Mọi lần mất và nối lại đều vào **lịch sử, không bao giờ xoá**.

**Ví dụ.**

- A mất kết nối ở `10 s`, quay lại ở `60 s` ⇒ khôi phục, thi tiếp.
- B quá ngưỡng giữa vòng ⇒ ghế tô nổi bật, vòng chạy tiếp; admin **giữ** hoặc **gia hạn**, và nếu muốn chặn hẳn B thao tác thì **vô hiệu hoá** — được phép ngay giữa vòng. B quay lại, admin đổi ý ⇒ **kích hoạt lại**, điểm không đổi.
- Câu VCNV mở lúc `0 s`, hạn `15 s`. E mất kết nối ở `4 s`, quay lại ở `9 s` ⇒ server đẩy **hạn chót `15 s`**, không phải *"còn 15 giây"* ⇒ E thấy còn **6 giây**. Bản E gửi lúc `3 s` còn nguyên; ký tự gõ dở lúc `4 s` chưa gửi thì **mất** — nó chưa từng tới server.

**Nguồn**: `QĐ-001`, `QĐ-030`, `QĐ-045`, `QĐ-046`, `QĐ-047`, `QĐ-053`

---

## GR-037 — Phạm vi hiển thị đáp án

**Mục đích.** Ấn định ai được thấy **đáp án chuẩn**, và khi nào.

**Kích hoạt.** Bất kỳ yêu cầu hiển thị đáp án nào.

> ⚠️ **Phạm vi rule này CHỈ là ĐÁP ÁN.** Ba loại thông tin có ba chế độ khác nhau, đừng tổng quát hoá:
>
> | Loại | Chế độ | Quy định ở |
> |---|---|---|
> | **Đáp án chuẩn** | **MẬT tới mốc CÂU KHÉP**; từ mốc đó công bố cho mọi vai theo cờ | **Rule này** |
> | **Bài làm của thí sinh khác** | **ẨN TẠM THỜI** khi câu còn mở; lộ khi admin bấm hiển thị | `GR-008` |
> | **ĐIỂM SỐ** | **CÔNG KHAI, LUÔN LUÔN** — mọi vai, mọi lúc, gồm cả máy thí sinh | `QĐ-015` |
>
> Ba thứ độc lập; điểm công khai **không** nới lỏng hai dòng trên.

**Điều kiện.**

- **Admin và MC luôn được xem**, không phụ thuộc cấu hình nào và không phụ thuộc mốc nào. Mỗi lần xem đều **vào audit**.
- **Thí sinh, viewer và overlay được xem từ mốc CÂU KHÉP trở đi**, với điều kiện `revealAnswerAfterJudge` **BẬT**. Trước mốc đó: **không ai trong ba vai này** được xem.
- **Mốc công bố là CÂU KHÉP, không phải "đã chấm"** — xem bảng §Mốc câu khép theo vòng bên dưới. Hai mốc trùng nhau ở mọi vòng **trừ Về đích**.
- `revealAnswerAfterJudge` là cờ **CẤP TRẬN**, mặc định **BẬT** cho cả `official` lẫn `practice`, admin đổi được từng trận. Giá trị được **chụp vào trận lúc start**; sửa cấu hình contest sau đó **không đụng** trận đã chạy.
- **Đáp án Chướng ngại vật KHÔNG thuộc rule này** — nó theo `GR-012`: lộ khi có người giải đúng, hoặc khi admin bấm công bố.
- Yêu cầu không được phép: server **im lặng**, không trả đáp án.

**Mốc câu khép theo vòng**

| Vòng · pha | Câu khép khi |
|---|---|
| Khởi động — lượt riêng | admin chấm xong *(hai mốc trùng)* |
| Khởi động — lượt chung | admin chấm xong; **hoặc** hết cửa sổ chuông 3 giây không ai bấm ⇒ câu bị bỏ qua |
| VCNV — câu hàng ngang | admin chấm xong *(hoặc bấm chốt câu — `GR-026` C4)* |
| VCNV — câu ô trung tâm | admin chấm xong |
| Tăng tốc | admin chấm xong cả bảng *(một câu = một event điểm — `QĐ-013`)* |
| **Về đích** | **cửa sổ cướp quyền đã đóng VÀ người cướp đã được chấm**; hoặc hết 5 giây không ai bấm; hoặc người thi chính được chấm **Đúng** *(không mở cửa sổ cướp)* |

**Bảng quyết định**

| Ca | Vai · cấu hình · mốc | Kết quả |
|---|---|---|
| C1 | **Admin** | Trả đáp án; ghi audit |
| C2 | **MC** | Trả đáp án; ghi audit |
| C3 | Thí sinh · viewer · overlay — reveal **TẮT** | **Không trả** |
| C4 | Thí sinh · viewer · overlay — reveal **BẬT** · **câu chưa khép** | **Không trả** |
| C5 | Thí sinh · viewer · overlay — reveal **BẬT** · **câu đã khép** | Trả đáp án |
| C6 | **Về đích** — người thi chính vừa bị chấm Sai, **cửa sổ cướp đang mở** | **Không trả** — câu **chưa khép**. Trả ở đây là xoá sổ cướp quyền |
| C7 | Câu **bị bỏ qua** *(không ai bấm chuông)* · reveal **BẬT** | Trả đáp án — câu đã tiêu, đã khép |
| C8 | Câu khép bằng phán quyết **Huỷ kết quả** | **Không tự trả.** Admin mở tay được (`QĐ-048`) |
| C9 | Đáp án **Chướng ngại vật** | Ngoài phạm vi rule này — theo `GR-012` |

> **Vì sao cờ đặt ở cấp TRẬN, không phải cấp CONTEST.** Một contest thật được phép chứa **cả** trận chính thức lẫn trận tổng duyệt. Đặt ở cấp contest thì không thể đặt hai giá trị khác nhau cho hai trận cùng phòng — kịch bản đó **không dựng được**.
>
> **Không lẫn với mode trả lời**, vốn đặt ở **cấp CONTEST**: mode là thuộc tính của **sân khấu và phần cứng** (có micro không, thí sinh có bàn phím không) nên không đổi giữa hai trận cùng phòng; `revealAnswerAfterJudge` là thuộc tính **sư phạm** của từng trận. Hai trục khác nhau, không phải thiếu nhất quán.

**Cấm.**

- **Cấm đẩy đáp án xuống client trước mốc câu khép rồi ẩn bằng cờ hiển thị.** Server chỉ được đẩy **tại đúng mốc**. Đây là lỗi của tiền lệ Athena — nó nạp toàn bộ ngân hàng đề xuống mọi máy client và chỉ dựa vào một cờ hiển thị của giao diện, khiến đáp án nằm sẵn trong bộ nhớ máy thí sinh suốt trận.
- **Cấm công bố khi cửa sổ cướp quyền Về đích đang mở** — xem C6.
- **Cấm cuốn đáp án Chướng ngại vật theo cơ chế này** — xem C9.

**Không đổi gì.** Trạng thái trận — xem đáp án **không** sinh event điểm nào · quyền của admin và MC · quyền đóng hiển thị thủ công của admin (`QĐ-048`) · **cửa sổ cướp quyền và mọi con số của luật**.

**Thứ tự đánh giá.** **(1)** nhận yêu cầu → **(2)** xét vai: admin hoặc MC ⇒ trả ngay → **(3)** còn lại: xét cờ reveal → **(4)** xét **câu đã khép chưa** → **(5)** xét ca biên *(Huỷ kết quả · Chướng ngại vật)* → **(6)** ghi audit nếu có trả.

**Biên.** Mốc công bố là **CÂU KHÉP**, không sớm hơn. Ở Về đích, mốc này đến **sau** cú bấm Đúng/Sai của người thi chính, cách nhau ít nhất bằng độ dài cửa sổ cướp quyền.

**Bấm trùng.** Xem nhiều lần: mỗi lần một dòng audit; trạng thái trận không đổi. Công bố là **một chiều ở phía engine** — đã công bố thì engine không tự thu lại.

**Ví dụ.**

- Admin thấy đáp án cạnh bài làm **ngay cả khi chưa chấm** — đó là công cụ để chấm.
- Khởi động lượt chung, reveal BẬT: admin chấm Sai ⇒ câu kết thúc ⇒ đáp án hiện trên máy thí sinh, màn khán giả và overlay.
- **Về đích, câu 30đ, reveal BẬT**: A trả lời sai ⇒ admin chấm Sai ⇒ cửa sổ cướp 5 giây mở, **đáp án vẫn kín** ⇒ B bấm chuông, trả lời, admin chấm ⇒ **bây giờ** đáp án mới hiện. Nếu công bố ở bước chấm Sai thì B chỉ việc đọc lại thứ vừa thấy — cướp quyền mất nghĩa.
- Khởi động lượt chung, hết 3 giây không ai bấm chuông: câu bị bỏ qua, **vẫn công bố** — câu đã hỏi, đã tiêu, giữ kín không bảo vệ được gì.
- Trận chính thức tắt cờ: chấm xong, chỉ admin và MC thấy đáp án — hành vi cũ vẫn dựng lại được bằng một thao tác.

**Nguồn**: `QĐ-015`, `QĐ-017`, `QĐ-039`, `QĐ-048`, `QĐ-051`, `QĐ-062`, `QĐ-080`
