# Luật chơi — CÁC QUYẾT ĐỊNH ĐÃ CHỐT

> **Mục đích**: đây là tập quyết định **đã được chủ dự án chốt** về luật chơi. Dùng làm đầu vào để viết `docs/PRD.md` và `specs/<feature>/spec.md`.
>
> **Ba file liên quan**:
> - **`game-rules-decisions.md`** (file này) — quyết định đã chốt, sắp theo chủ đề
> - `game-rules-review-old.md` — danh sách 136 vấn đề phát hiện (GRR-001 → GRR-136) kèm trạng thái
> - `game-rules-open-questions.md` — những gì còn chờ chủ dự án quyết
>
> **Nguồn luật gốc**: `docs/source/fandom-olympia-26-luat-choi.md` (snapshot 2026-07-23).
> **Điều khiển hệ thống** (quyền, UI, audit, quy trình) KHÔNG nằm ở đây — xem `docs/product-discovery.md` §6 C-11 → C-16 và S-1 → S-15.
>
> **Ngày chốt**: 2026-07-24 và 2026-07-25 (đợt sau: Đ-16 → Đ-21, xem §11).

---

## Mục lục

1. [Nguyên tắc nền](#1-nguyên-tắc-nền)
2. [Phạm vi phiên bản](#2-phạm-vi-phiên-bản)
3. [Chấm điểm và so khớp đáp án](#3-chấm-điểm-và-so-khớp-đáp-án)
4. [Hai mode trả lời](#4-hai-mode-trả-lời)
5. [Tín hiệu của thí sinh và hàng đợi](#5-tín-hiệu-của-thí-sinh-và-hàng-đợi)
6. [Điều khiển trận: vòng, lượt, bỏ và chạy lại](#6-điều-khiển-trận-vòng-lượt-bỏ-và-chạy-lại)
7. [Mô hình dữ liệu điểm: event log và revert](#7-mô-hình-dữ-liệu-điểm-event-log-và-revert)
8. [Mốc thời gian và vai trò của admin](#8-mốc-thời-gian-và-vai-trò-của-admin)
9. [Quyết định theo từng vòng](#9-quyết-định-theo-từng-vòng)
10. [Cách đọc nguồn đã được xác nhận](#10-cách-đọc-nguồn-đã-được-xác-nhận)
11. [Quyết định 2026-07-25](#11-quyết-định-2026-07-25--phát-sinh-khi-rà-decision-table)
12. [Bảng tra mã quyết định](#12-bảng-tra-mã-quyết-định)

---

## 1. Nguyên tắc nền

### 1.1 Mô hình BA TẦNG — máy độc quyền SỰ KIỆN, người độc quyền PHÁN QUYẾT

| Tầng | Vai trò | Phương tiện |
|---|---|---|
| **MC** | Thẩm quyền phán quyết **trên sân khấu** | **Nói** |
| **Admin** | **Cảm biến + cơ cấu chấp hành DUY NHẤT** của hệ thống | **Bấm** |
| **Server** | Sự kiện và thời gian (server time, thứ tự chuông, timer) | Không ai sửa được |

Áp cho **mọi** câu hỏi *"ai làm X"*: nếu X là phán quyết game thì **MC nói, admin bấm**.

⇒ Màn `/mc` là **READ-ONLY**. Không tạo bề mặt quyền ghi mới cho MC. *(Đ-6.4a — nhánh A)*

### 1.2 Hệ thống ADVISORY

- Máy **không** tự chấm Đúng/Sai.
- Máy **không** cưỡng chế thứ tự vòng/lượt — chỉ **recommend** và **cảnh báo**.
- Admin **luôn ép được** qua dialog Yes/No.
- **Ngoại lệ duy nhất**: ngưỡng bất khả thi vật lý (xem §6.5).

### 1.3 Ranh giới CHẶN CỨNG vs CẢNH BÁO

> Hệ thống **chỉ chặn cứng** ở ngưỡng **bất khả thi vật lý** — khi vòng/pha đó không còn nghĩa. **Mọi lệch luật khác chỉ CẢNH BÁO.**

*Đối chiếu bản cũ `Athena-Intelligent-Olympia`: toàn bộ server chỉ có **một** chỗ chặn cứng (`ExtraPlayerPicker`: `CountPlayer < 2`); thiếu đề chỉ là cảnh báo.*

### 1.4 Chống bấm nhầm: chỉ ở phía ADMIN

- **Phía thí sinh**: tức thời, **không dialog**, không rút lại — *"thí sinh cần tốc độ, và tự chịu trách nhiệm sai lầm của mình"*.
- **Phía admin**: mọi thao tác không hoàn tác được đều qua **dialog Yes/No**.
- **Pattern chung**: *thí sinh phát tín hiệu (tức thời) → hàng đợi → admin xác nhận → mới có hiệu lực.* Lỗi bấm nhầm của thí sinh được sửa bằng **admin bấm No**.

*(Đ-4.3, Đ-7)*

---

## 2. Phạm vi phiên bản

> **v1 = ĐÚNG 4 thí sinh** (về LUẬT). Luật cho 1-12 thí sinh chuyển sang **v1.5**.
> **v1 vẫn hỗ trợ LƯU TRỮ DỮ LIỆU và UI cho 1-12 người** — schema, seat model, `scoringUnit`, RuleConfig dạng mảng, và giao diện. v1.5 chỉ ship **luật + sửa controller**, KHÔNG migrate schema, KHÔNG dựng lại UI.

**Hoãn sang v1.5** (chỉ do số ghế ≠ 4): GRR-017 · GRR-029 · GRR-030 · GRR-045 · GRR-083 · GRR-105 · GRR-127
**Hoãn sang v2** (thi đội): GRR-093 → GRR-098

**KHÔNG hoãn được** — xảy ra ngay trong v1: **GRR-129** — cấu hình 4 ghế nhưng **runtime tụt còn 3** (rớt quá grace, bị loại, bỏ cuộc). Đã chốt: **admin quyết**, hệ thống chỉ cảnh báo (§6.4).

---

## 3. Chấm điểm và so khớp đáp án

### 3.1 Máy KHÔNG phán xử đáp án `[Đ-1]`

> Vì chương trình do ban tổ chức quyết định đúng/sai, repo **chỉ HIỂN THỊ highlight các ký tự khác** với đáp án; phần còn lại do admin tự đánh giá.

Hệ quả: mâu thuẫn *"chính tả nghiêm ngặt vs normalize"* **không còn là mâu thuẫn luật**, vì không có nhánh máy nào tự quyết. Normalize và highlight chỉ là **trợ giúp hiển thị**.

**Không viết đường code nào tự cộng/trừ điểm từ so khớp.**

### 3.2 Điểm được phép ÂM `[Đ-2]`

Không có sàn điểm. Điểm âm tham gia bình thường vào xếp lượt Về đích và điều kiện hoà.

### 3.3 Idempotency của thao tác chấm `[GRR-071]`

Dedup ở **server** theo `(câu, thí sinh, loại phán quyết)`:
- Bấm **Đúng hai lần** = **một** event
- Bấm **Sai sau Đúng** = **revert + event mới**

### 3.4 Vòng xếp hạng: một câu = MỘT event `[Đ-5.3.1]`

Điểm Tăng tốc là hàm của **tập người trả lời đúng** ⇒ **một câu = MỘT event điểm cho TOÀN BỘ người chơi**, không phải mỗi người một event. Revert = revert **cả bảng xếp hạng** của câu đó.

⇒ Thứ tự admin chấm **không** làm sai điểm người đã chấm trước.

---

## 4. Hai mode trả lời

### 4.1 Định nghĩa `[Đ-4]`

| Mode | Nội dung |
|---|---|
| **Sân khấu** *(MẶC ĐỊNH)* | Thí sinh **đọc** đáp án; máy tính **chỉ để giành quyền trả lời** |
| **Nhập liệu** | Thí sinh **gõ** đáp án |

**Luật được đặc tả theo mode SÂN KHẤU** `[Đ-4.1]`. Mode nhập liệu là **biến thể**.

### 4.2 Áp cho vòng nào

| Vòng | Kênh trả lời |
|---|---|
| Khởi động · Về đích · Câu hỏi phụ | **Theo mode** |
| **VCNV** | **LUÔN gõ máy** (đáp án hàng ngang). Mode chỉ đổi **cách chọn hàng ngang** |
| **Tăng tốc** | **LUÔN gõ máy**, không có mode `[Đ-4.b]` |

⇒ Phát biểu *"các thí sinh trả lời bằng máy tính"* của `F26` (VCNV, Tăng tốc) **được giữ nguyên, KHÔNG bị ghi đè**. Không có mâu thuẫn với nguồn.

### 4.3 Cấu hình `[Đ-4.a2]`

Mode đặt ở **cấp CONTEST**, là **MỘT giá trị chung cho toàn bộ vòng**. Một contest không trộn hai mode.

### 4.4 Chi tiết theo mode

- **VCNV mode sân khấu**: chọn hàng ngang **do admin điều khiển**.
- **VCNV mode nhập liệu**: **thí sinh click chuột**, hoặc admin điều khiển.
- **VCNV mọi mode**: thí sinh dùng máy để chọn **"Mở chướng ngại vật"**.
- **Đáp án Chướng ngại vật** đi theo **mode**; *"trả lời hoàn toàn bằng máy tính"* chỉ áp cho **đáp án hàng ngang** `[Đ-4.X2]`.
- **Câu ô trung tâm**: cùng loại với hàng ngang ⇒ **luôn gõ máy** *(đề xuất Đ-14, chưa duyệt)*.
- **Khởi động lượt riêng mode sân khấu**: máy tính **hiển thị câu hỏi** `[Đ-4.f]`.
- **Câu hỏi phụ mode nhập liệu**: cho phép **gõ ngay từ khi mở đề**; **nhấn chuông == NỘP đáp án** `[Đ-10.6]`.

**Lưu ý ngược trực giác**: mode sân khấu **KHÔNG** làm nhẹ yêu cầu phần cứng — VCNV và Tăng tốc vẫn buộc gõ máy, nên mỗi thí sinh vẫn cần thiết bị nhập liệu đầy đủ.

---

## 5. Tín hiệu của thí sinh và hàng đợi

### 5.1 Hàng đợi `[Đ-7]`

- **Mọi** tín hiệu của thí sinh vào **HÀNG ĐỢI** theo thứ tự tới (server timestamp).
- **KHÔNG có cơ chế drop** — khái niệm "drop tín hiệu" đã bị loại khỏi hệ thống `[Đ-7.a]`.
- Admin **reject** tín hiệu 1 ⇒ tín hiệu 2 lên; **reject KHÔNG làm thí sinh mất lượt**.
- **Hàng đợi đang hoạt động** reset sau mỗi **VÒNG** rồi tái sử dụng `[Đ-7.b]`.
- **LỊCH SỬ tín hiệu KHÔNG BAO GIỜ XOÁ** (append-only) — để admin xem lại và **gỡ lệnh cấm** `[Đ-6.3]`.

### 5.2 Queue chỉ CHẶN ở VCNV `[Đ-7.2]`

| Vòng | Queue ghi nhận | Queue chặn |
|---|---|---|
| **VCNV** — chọn hàng ngang, "Mở chướng ngại vật" | Có | **CÓ** — admin xác nhận mới có hiệu lực |
| **Khởi động lượt chung** — chuông | Có | **KHÔNG** — có chuông là tính ngay |
| **Về đích** — cướp quyền 5 giây | Có | **KHÔNG** — như trên |
| **Câu hỏi phụ** — chuông | Có | **KHÔNG chặn**, nhưng **xử lý thứ tự SAU khi hết 15 giây** `[Đ-9]` |

**Tiêu chí phân biệt**:
> Tín hiệu **một chiều, hậu quả nặng, KHÔNG bị ép thời gian** ⇒ queue **chặn**.
> Tín hiệu **đua tốc độ, cửa sổ chặt** ⇒ **không chặn**, server phân xử ngay; queue chỉ là lưới an toàn cho sự cố.

### 5.3 Nút "Mở chướng ngại vật" `[Đ-4.3]`

- Được xếp là **CHUÔNG** ⇒ **chỉ nhận click chuột, KHÔNG gán hotkey**.
- Phía thí sinh **không có dialog** — bấm là gửi tín hiệu ngay.
- **ADMIN xác nhận rồi mới được mở** — đây là chỗ sửa lỗi bấm nhầm của thí sinh.

### 5.4 Chọn hàng ngang `[Đ-4.2]`

Hai đường vào (thí sinh click **hoặc** admin click), cả hai đều qua **hàng đợi** + **admin xác nhận Yes/No**. Không drop.

---

## 6. Điều khiển trận: vòng, lượt, bỏ và chạy lại

### 6.1 Admin điều khiển `[Đ-5]`

1. **Admin chọn vòng nào bắt đầu.**
2. **Admin chọn đây là lượt của ai** — hệ thống chỉ hiển thị **recommendation dựa trên luật**.
3. Conflict luật ⇒ **dialog cảnh báo**, admin bấm Yes/No để vẫn thực hiện. **KHÔNG chặn cứng.**

### 6.2 Thứ tự do ADMIN quyết định `[Đ-7.1]`

Contest settings (vị trí thí sinh, thứ tự lượt riêng Khởi động) là **đầu vào của RECOMMENDATION**, không phải ràng buộc cưỡng chế.

**Luật quy định thứ tự ở đâu** *(rà soát nguồn — `[Đ-4.5]`)*:

| Vòng / pha | Có lượt? | Luật quy định thứ tự |
|---|---|---|
| Khởi động — **lượt riêng** | **Có** | **KHÔNG có luật** ⇒ recommend theo **contest settings** `[Đ-5.a]` |
| Khởi động — lượt chung | Không | Bấm chuông nhanh |
| VCNV — **chọn hàng ngang** | **Có** | Theo **vị trí**: *"bắt đầu từ thí sinh ở vị trí số 1"* |
| VCNV — trả lời hàng ngang | Không | Mọi thí sinh trả lời trong 15 giây |
| VCNV — giải Chướng ngại vật | Không | *"bấm chuông… bất cứ lúc nào"* |
| Tăng tốc | Không | Tất cả trả lời cùng lúc |
| Về đích — **lượt gói câu** | **Có** | Theo **điểm số**, tính lại sau mỗi lượt; tie-break theo **vị trí** |
| Về đích — cướp quyền | Không | Bấm chuông nhanh trong 5 giây |
| Câu hỏi phụ | Không | Bấm chuông nhanh nhất |

⇒ **Contest settings chỉ cần hai trường**: **vị trí** (dùng cho VCNV *và* tie-break Về đích) và **thứ tự lượt riêng Khởi động**. KHÔNG thêm trường "thứ tự lượt VCNV" — luật đã buộc theo vị trí.

*"Vị trí" là giá trị gán thủ công trước trận (thực tế chương trình: BTC **bốc thăm** thứ tự xuất phát 1→4) `[Đ-4.6]`.*

### 6.3 Bỏ vòng và chạy lại vòng `[Đ-5.1]`

> Admin được **bỏ hẳn một vòng** và **chạy lại một vòng**, với điều kiện **ngân hàng đề của contest còn câu hỏi**.

| Khía cạnh | Quyết định |
|---|---|
| **Điểm** | **Reset về trước đó** = **REVERT** các event điểm của vòng đó `[Đ-5.2]` |
| **Chạy lại** | **Cùng quy tắc**: reset về trước rồi chạy mới `[Đ-5.1a]` |
| **Biên bản** | **Giữ đầy đủ**; vòng bị bỏ hiện **kèm nhãn "đã bỏ"**, như một vòng đã chạy `[Đ-5.2d]` |
| **Viewer/overlay** | Thấy **số đột ngột thay đổi** — không hiệu ứng, không thông báo. Có chủ đích `[Đ-5.2e]` |
| **Câu hỏi đã dùng** | **KHÔNG trả lại pool** — điểm hoàn được, đề đã lộ thì không `[Đ-5.2f]` |
| **`SCORE_ADJUST` của admin** | **KHÔNG tự revert** — đó là phán quyết của người `[Đ-11.B]` |
| **Vòng đang chạy vs đã kết thúc** | **Cùng quy tắc**, chỉ khác kích thước tập event `[Đ-11.B]` |
| **Queue** | Reset như kết thúc vòng bình thường `[Đ-11.B]` |

### 6.4 Số thí sinh tụt dưới 4 giữa trận `[GRR-129]`

> **Do admin quyết định**, vì admin là người điều khiển trận đấu: điều khiển vòng nào, lượt ai.

Trận **không tự dừng**, hệ thống **không tự phát minh luật**; recommend theo luật và **cảnh báo** khi lựa chọn không khớp cấu hình 4 người.

### 6.5 Ngưỡng chặn cứng — *bản nháp, chưa duyệt*

Chỉ hai ngưỡng thật: **cửa sổ cướp Về đích ≥2** và **Câu hỏi phụ ≥2**. Mọi vòng khác **≥1**. Chi tiết: `game-rules-open-questions.md` §Đ-15.3.

---

## 7. Mô hình dữ liệu điểm: event log và revert

### 7.1 Ngữ nghĩa `[Đ-5.3]`

> **Điểm là các event log. Reset thực hiện REVERT điểm** (tương tự `git revert`, **KHÔNG phải** `git reset --hard`). **Lịch sử luôn linear, mọi thao tác không bị xoá.**

Ba tính chất:
1. **Điểm là hàm của event log**, không phải một con số được sửa trực tiếp.
2. **Revert = thêm event đảo ngược**, không xoá event cũ.
3. **Lịch sử linear, append-only.**

**Ví dụ**: chạy KĐ → VCNV → TT rồi bỏ **VCNV** ⇒ sinh event đảo ngược **chỉ cho event điểm của VCNV**; điểm Tăng tốc **giữ nguyên**. Không có khái niệm "quay về snapshot mốc vòng".

### 7.2 Revert thay thế ràng buộc undo cũ `[Đ-5.3.X]`

Ràng buộc `R-GEN-07` (*"undo chỉ event chấm điểm GẦN NHẤT"*) **bị Đ-5.3 thay thế**. Ràng buộc cũ tồn tại để tránh bất nhất khi điểm là **số bị sửa trực tiếp**; với event log, revert event ở giữa là well-defined.

⇒ **GRR-073** (*"build-upon" nghĩa là gì*) **bị bỏ khỏi danh sách** — mất đối tượng.

### 7.3 Hoàn nguyên trạng thái PHI-ĐIỂM — *đề xuất, chưa duyệt*

Các trạng thái khác (NSHV đã tiêu, người bị loại VCNV, miếng ghép đã mở, gói câu đã chọn, lượt chọn đã dùng) **cũng sinh từ event** ⇒ revert đảo **tất cả** đồng nhất. **Một quy tắc, một ngoại lệ** (ngoại lệ = "câu đã dùng", Đ-5.2f). Chi tiết: `game-rules-open-questions.md`.

---

## 8. Mốc thời gian và vai trò của admin

### 8.1 Nguyên tắc `[Đ-6, Đ-6.1]`

> **Mọi mốc thời gian mà luật gốc mô tả bằng hành vi của MC đều ánh xạ thành MỘT THAO TÁC BẤM CỦA ADMIN** — máy không quan sát được sân khấu, **admin là cảm biến**.

| Mốc trong luật gốc | Sự kiện hệ thống |
|---|---|
| *"MC đọc xong câu hỏi"* | **Admin bấm start timer** `[Đ-6]` |
| *"hiệu lệnh của người dẫn chương trình"* (Câu hỏi phụ) | **Admin bấm** — admin là người **nghe** hiệu lệnh `[Đ-6.1]` |
| *"câu hỏi được đọc lên **hoặc hiện lên** trên màn hình"* (đóng cửa sổ NSHV) | **Admin bấm hiển thị câu hỏi** — mốc đến trước `[GRR-048]` |

### 8.2 Mốc admin là TUYỆT ĐỐI `[Đ-6.3]`

> **KHÔNG có cửa sổ ân hạn, KHÔNG trừ bù độ trễ tay người** — thời gian do server quyết định.

Van thoát **không phải grace** mà là: lịch sử giữ đầy đủ để admin **xem lại và gỡ lệnh cấm**.

*Lý do chọn "tuyệt đối + sửa được sau" thay vì grace: grace kéo theo ba khoản nợ — một con số ma không căn cứ, một biên mới (bấm đúng mép ân hạn là trong hay ngoài), và làm mờ nguyên tắc "server time là quyết định cuối cùng".*

### 8.3 Lệnh cấm và gỡ cấm `[Đ-6.2 → Đ-6.4]`

**Phạm vi hình phạt** *"mất quyền trả lời câu hỏi"*: chỉ ở **Câu hỏi phụ**, **theo CÂU**, khi bấm chuông **trước hiệu lệnh**.

*Đối chiếu: hai vòng khác quy định NGƯỢC LẠI — Khởi động cho bấm "trong khi MC đang đọc"; VCNV cho bấm "bất cứ lúc nào". Đây là ngoại lệ riêng của tie-break.*

**Cơ chế gỡ cấm**:

| | Quyết định |
|---|---|
| Bản chất | Một **TRẠNG THÁI CẤM** (không phải bỏ qua một lần bấm), **gỡ được** `[Đ-6.2a]` |
| Thời điểm | Diễn ra **TRƯỚC khi trả lời**, trong lúc câu còn mở `[Đ-6.4]` |
| Ai quyết | **MC quyết bằng lời, admin bấm** `[Đ-6.4a]` |
| Thứ tự sau khi gỡ | **Giữ timestamp gốc**; ai được quyền trả lời là **chuyện của BTC, không phải của hệ thống** `[Đ-6.4b]` |
| Timer | **Chạy hết**, không pause `[Đ-6.4c]` |
| Sau khi câu kết thúc | **Kết quả đã chốt**, không quan tâm cấm hay không `[Đ-6.4d]` |

**Phát biểu gọn**: lệnh cấm là **trạng thái theo CÂU**, **chết cùng câu**; cửa sổ gỡ cấm thực tế = **thời gian còn lại của câu**.

---

## 9. Quyết định theo từng vòng

### 9.1 Khởi động

| Mục | Quyết định |
|---|---|
| Cửa sổ chuông lượt chung | **MỘT khoảng liên tục** = (thời gian MC đọc) + 3 giây `[GRR-005]` |
| Mốc bắt đầu 3 giây | **Admin bấm start timer** `[Đ-6]` |
| Không trả lời ở lượt riêng | Xử như **trả lời sai**: 0 điểm, không trừ `[GRR-006]` |
| Biên quỹ thời gian | Hết giờ còn câu / hết câu còn giờ ⇒ **lượt kết thúc** ở cả hai nhánh `[GRR-012]` |
| Mode sân khấu | Máy tính **hiển thị câu hỏi** `[Đ-4.f]` |
| Thứ tự lượt riêng | Luật **không quy định** ⇒ recommend theo **contest settings** `[Đ-5.a]` |

### 9.2 Vượt chướng ngại vật

| Mục | Quyết định |
|---|---|
| Điều kiện mở ô trung tâm | *"Từ hàng ngang được mở"* ≠ *"miếng ghép được mở"* ⇒ cả 4 hàng **luôn được hỏi hết** ⇒ ô trung tâm **luôn tới được** `[GRR-013]` |
| Thang điểm CNV | *"Trong N từ hàng ngang"* đếm theo **số hàng ĐÃ HỎI**; bấm trước khi hàng 1 bắt đầu vẫn thuộc băng **60 điểm** `[GRR-019]` |
| Mốc 20 điểm | Gắn với việc **gợi ý cuối đã được đưa ra**, KHÔNG phụ thuộc ô trung tâm đúng/sai `[GRR-025]` |
| Luật quay vòng lượt chọn | *"Trong trường hợp này"* **giới hạn phạm vi** ⇒ *"tối đa 1 lượt"* là luật chung, quay vòng là **ngoại lệ tường minh** khi đã có người bị loại. **Không phải mâu thuẫn** `[GRR-016]` |
| Kênh trả lời | Hàng ngang **luôn gõ máy**; đáp án CNV **theo mode** `[Đ-4.X2]` |
| Chọn hàng ngang | Queue + admin xác nhận `[Đ-4.2]` |
| Nút "Mở chướng ngại vật" | Là **chuông** (cấm hotkey); admin xác nhận mới mở `[Đ-4.3]` |
| Nhiều người bấm giải CNV | Thứ tự = thứ tự vào queue; người bị reject **không mất lượt** `[GRR-021]` |
| Admin không bấm mở CNV | Không còn là trạng thái tắc — admin luôn chuyển vòng được `[GRR-027]` |

### 9.3 Tăng tốc

| Mục | Quyết định |
|---|---|
| Kênh trả lời | **LUÔN nhập liệu**, không có mode `[Đ-4.b]` |
| Độ hạt event điểm | **Một câu = MỘT event cho toàn bộ người chơi** `[Đ-5.3.1]` |
| Thứ tự admin chấm | Không còn làm sai điểm người đã chấm trước `[GRR-039]` |
| Chuỗi gửi A→B→A | Timestamp = **lần gửi A thứ hai** (khác bản trước nên cập nhật) `[GRR-033]` |

### 9.4 Về đích

| Mục | Quyết định |
|---|---|
| NSHV sai + có người cướp đúng | **Trừ MỘT lần.** Hình phạt NSHV **thay thế** transfer, không cộng dồn `[GRR-047]` |
| Cửa sổ đặt NSHV | **Đóng khi admin bấm hiển thị câu hỏi** `[GRR-048]` |
| Mốc *"hoàn thành phần thi"* | **Sau khi cửa sổ cướp của câu cuối đóng VÀ chấm xong** `[GRR-041]` |
| Kết thúc câu sớm | **KHÔNG** — chạy hết giờ `[GRR-056]` |
| Điểm lẻ | Chỉ phát sinh khi admin cấu hình giá trị **lẻ**; dưới luật 2026 **không tồn tại** (20/30 → 10/15) `[Đ-3]` — quy tắc xử lý **chưa chốt** |

### 9.5 Câu hỏi phụ

**Luồng 8 phase** `[Đ-9]`:

1. **Admin chọn người tham gia**
2. **Admin bấm hiển thị câu hỏi** — MC đọc câu hỏi trên sân khấu
3. **MC hô bắt đầu — admin bấm start timer**
4. **Timer chạy, thí sinh bấm** — *ai biết thì chắc chắn bấm*, nên **timer cứ việc chạy**
5. **Hết thời gian, admin và MC xử lý thứ tự bấm**
6. **Thí sinh trả lời miệng** *(mode sân khấu)*
7. **Admin bấm Đúng/Sai** (theo hướng dẫn của BTC, BGK, MC)
8. **Hệ thống tính điểm theo luật**

**Vòng lặp** `[Đ-9.b]`:
```
với i = 1..3:
    phase 2 → 8
    nếu có người trả lời ĐÚNG  →  người đó thắng, KẾT THÚC
hết 3 câu chưa có người đúng  →  BỐC THĂM
```

**Cửa sổ lệnh cấm**: khoảng **giữa phase 2 và phase 3** — trong lúc MC đang đọc câu hỏi.

| Mục | Quyết định |
|---|---|
| Có cộng điểm vào điểm trận không | **KHÔNG** — chỉ đổi thứ hạng. Nguồn: *"có số điểm cao nhất **bằng với số điểm của thí sinh còn lại**"* `[GRR-058]` |
| Trả lời sai có bị loại khỏi câu sau | **KHÔNG** `[GRR-059]` |
| Không ai bấm trong 15 giây | **TÍNH** vào tổng 3 câu, sang câu tiếp `[Đ-9.a]` |
| Cả nhóm bị cấm ở một câu | **TÍNH** vào tổng 3 câu `[GRR-061]` |
| Pool câu phụ cạn | **Bốc thăm** `[GRR-064]` |
| Ai được đưa vào | Hệ thống recommend **MỌI nhóm hoà**; admin chọn `[Đ-10.7]` |
| Mode nhập liệu | Gõ **ngay từ khi mở đề**; **nhấn chuông == NỘP đáp án** `[Đ-10.6]` |
| Bốc thăm không xác nhận | Không còn là trạng thái tắc — admin luôn kết thúc được `[GRR-062]` |

---

## 10. Cách đọc nguồn đã được xác nhận

Sáu chỗ mà lượt review đầu xếp là mâu thuẫn/thiếu, nhưng **nguồn đã trả lời** — chỉ cần đọc kỹ hơn. Đã được chủ dự án duyệt `[Đ-12]`.

| Mã | Cách đọc |
|---|---|
| **GRR-013** | Nguồn dùng **hai chủ ngữ khác nhau**: *"miếng ghép không được mở"* (dòng 53) vs *"cả 4 **từ hàng ngang** đã được mở"* (dòng 57) |
| **GRR-019** | *"Trong N từ hàng ngang"* = số hàng **đã hỏi**, không phải số miếng ghép đã mở |
| **GRR-016** | *"Trong trường hợp này"* là **mệnh đề giới hạn phạm vi** |
| **GRR-047** | *"kể cả các thí sinh còn lại có giành quyền trả lời hay không"* tồn tại để nói **kết quả giống nhau** ở cả hai trường hợp |
| **GRR-048** | Nguồn dùng **"hoặc"** ⇒ lấy mốc **đến trước** |
| **GRR-058** | *"bằng với số điểm của thí sinh còn lại"* khẳng định **điểm không đổi** |
| **GRR-005** | Ghép *"có thể bấm trong khi MC đang đọc"* với mốc admin start timer |

**Ghi chú về thời gian câu thực hành** — không mâu thuẫn, mà là **hai chủ thể**:

| | Câu 20 điểm | Câu 30 điểm |
|---|---|---|
| Người thi chính | suy nghĩ 15s · thực hành **30s** | suy nghĩ 20s · thực hành **60s** |
| Người cướp quyền | thực hành **20s** | thực hành **40s** |

---

## 11. Quyết định 2026-07-25 — phát sinh khi rà decision table

> Sáu quyết định dưới đây chốt trong lượt rà `docs/game-rules.md`. Mỗi cái **xoá bỏ** một hoặc nhiều nhánh mà tài liệu trước đó để treo.
> Góc **điều khiển hệ thống** của cùng bộ quyết định này ghi ở `docs/product-discovery.md` §C-17.

### 11.1 Thao tác ở INVALID STATE không thực hiện được `[Đ-16]`

| Phía | Hành vi |
|---|---|
| **Thí sinh** | **Không hiển thị gì**, bấm **không phản hồi** ⇒ không tín hiệu nào được tạo |
| **Admin** | Hiện **toast** báo thao tác không hợp lệ, **không thực hiện được** |

Phân biệt với `Đ-5` (conflict LUẬT → dialog cảnh báo, admin **ép được**): invalid state là *state machine không nhận*, không phải *lệch luật*.

**Không** mâu thuẫn `Đ-7.a` (*"không có cơ chế drop"*): drop là loại bỏ tín hiệu **đã nhận**; ở đây **không tín hiệu nào được sinh ra**.

⇒ Đóng `game-rules-review.md` GRR-141, GRR-146, GRR-161.

### 11.2 Một câu đi qua ĐÚNG MỘT phán quyết; phán quyết mở đường sang câu kế `[Đ-17]`

**Cơ chế**: admin bấm Đúng hoặc Sai ⇒ **nút chấm bị khoá**, **nút "Câu kế tiếp" hiện lên**.

Hai hệ quả:
1. **Không có bấm nhầm, bấm lại, hay đổi phán quyết tại chỗ.** Muốn sửa một phán quyết đã chốt thì đi qua hoàn nguyên event log (`Đ-5.3`) hoặc `SCORE_ADJUST` (`Đ-11.B`).
2. Không chuyển sang câu tiếp theo được nếu câu hiện tại chưa được chấm ⇒ **không tồn tại trạng thái "vòng đã đóng mà còn câu chưa chấm"** ở các vòng hỏi tuần tự.

> **Quan hệ với §3.3**: dedup theo `(câu, thí sinh, loại phán quyết)` **vẫn giữ nguyên ở SERVER** theo `CLAUDE.md` §Zero-trust. Khác biệt là ở cấp nghiệp vụ thì tình huống "admin bấm hai lần" **không còn xảy ra** — dedup nay là lưới an toàn, không phải luồng thường gặp.

**Còn mở**: vòng xếp hạng (Tăng tốc) — chuyển câu khi mới chấm **một phần** số thí sinh thì thế nào (`game-rules-review.md` GRR-162).

### 11.3 Mỗi contest chỉ có MỘT admin duy nhất `[Đ-18]`

Không tồn tại hai luồng thao tác admin đồng thời ⇒ mọi event của admin luôn tuần tự theo thứ tự bấm. Mô hình ba tầng (§1.1) nay đúng cả về **vai trò** lẫn **số người đang điều khiển**.

⇒ Đóng `game-rules-review.md` GRR-168.

### 11.4 "Trả lời sai" và "không trả lời" là CÙNG một thao tác `[Đ-19]`

Cả hai đều thể hiện bằng **admin bấm Sai**. Hệ thống **không phân biệt** hai tình huống, và đồng hồ hết giờ **không** tự sinh kết quả — nhất quán với `Đ-1` (máy không tự chấm).

Áp cho cả lượt riêng (0 điểm, không trừ) và lượt chung (−5).

### 11.5 Đồng hồ có hai vai trò bất đối xứng `[Đ-20]`

| Đối tượng | Đồng hồ làm gì |
|---|---|
| **Thí sinh** | **Khoá / mở khoá thao tác** — ngoài cửa sổ thì nút không hiển thị, bấm không phản hồi |
| **Admin** | **KHÔNG khoá gì**. Ngoại lệ duy nhất: **nút start timer tự khoá sau lần bấm đầu** để chống bấm trùng |

⇒ Mốc thời gian của luật là **ràng buộc với thí sinh**, **mốc tham chiếu** với người điều khiển. "Trong hạn hay quá hạn" ở phía phán quyết là đánh giá của MC và admin.

⇒ Đóng `game-rules-review.md` GRR-137. Đây cũng là **lý do** đứng sau Đ-16: giải thích vì sao phía thí sinh bị khoá mà phía admin thì không.

### 11.6 KHÔNG tồn tại trạng thái "trận tạm dừng" `[Đ-21]`

Mọi thao tác đều do admin thực hiện ⇒ trận cần dừng thì **admin ngừng thao tác**, hệ thống không cần biết. **Bãi bỏ** `R-GEN-08`: không đóng băng đồng hồ, không tự động tạm dừng, không có giá trị `PAUSED` trong máy trạng thái.

Máy trạng thái trận còn **5 giá trị**: `LOBBY` · `rounds[i]` · `INTERMISSION` · `TIE_BREAK` · `FINISHED`.

**Van thoát khi sự cố rơi vào giữa một cửa sổ có ràng buộc thời gian** (Tăng tốc, cướp quyền Về đích): dùng cơ chế đã có — admin **bỏ hoặc chạy lại vòng** (`Đ-5.1`); lưu ý câu đã dùng **không** trả lại kho đề (`Đ-5.2f`).

⇒ Đóng `game-rules-review.md` GRR-165, GRR-166, GRR-167 và bãi bỏ U-12.

### 11.7 Tín hiệu thí sinh không làm gián đoạn đồng hồ `[Đ-22]`

Khi có người bấm **"Mở chướng ngại vật"** giữa lúc đồng hồ hàng ngang đang chạy:

1. **Ghi nhận ngay** tín hiệu.
2. **Đồng hồ vẫn chạy bình thường** — không tạm dừng, không kéo dài.
3. **Không lộ gì thêm cho tới khi admin bấm hiển thị** — ẩn **cả hai**: (a) **đáp án chuẩn của chương trình** cho hàng ngang đang chạy, (b) **bài làm của các thí sinh khác**.

Vế 3 là điều kiện để hai vế đầu an toàn, và phải ẩn **cả hai** nguồn dữ kiện mới đủ:

| Nếu lộ | Người bấm CNV được lợi gì |
|---|---|
| Đáp án chuẩn của chương trình | Biết luôn hàng ngang đó là gì — một gợi ý trọn vẹn mà luật không cho |
| Bài làm của thí sinh khác | Suy ra hàng ngang qua phỏng đoán của đối thủ, kể cả khi những phỏng đoán đó sai |

Ẩn một cái mà lộ cái kia thì cơ chế vẫn hỏng.

⇒ Đóng `game-rules-inventory.md` U-16 (đề xuất *"pause timer hàng ngang"* — **bác bỏ**).

**Tiền lệ `Athena-Intelligent-Olympia`** — cùng cách xử lý, đã chạy thật:

| Nơi | Hành vi |
|---|---|
| `AICtrlLib/ObstacleUI.cs` `Obstacle_Click` | Nút CNV **không kiểm tra state**, bấm lúc nào cũng gửi ngay |
| `AICtrlLib/ObstacleUI.cs` `RowAnswerTextBox_PreviewKeyDown` | Ô nhập hàng ngang **chỉ nhận phím khi đang đếm giờ**; ngoài giờ chặn và xoá trắng — đúng tiền lệ của `Đ-20` và `Đ-16` |
| `AIServer/Obstacle.cs` `Timer_Tick` | Nhận lệnh `obstacle` ở **mọi** state, **không đụng tới đồng hồ**; pha đếm giờ vẫn chạy đủ 15 giây |
| `AIServer/Obstacle.cs` `ShowAnswer_Click` | Bài làm của thí sinh **chỉ hiện khi admin bấm**, không tự hiện lúc hết giờ |
| `AICtrlLib/ObstacleUI.cs` dòng 104 | Đáp án chuẩn bị **ẩn trên máy thí sinh**: `AnswerTextBlock` chỉ hiện với client số 4 (màn MC), các ghế 0-3 thì `Collapsed` — khớp `Đ-15.2` / `R-GEN-10` (đáp án chỉ tới admin và MC) |

**Khác biệt cần biết**: Athena còn **tự khoá ô nhập hàng ngang của chính người vừa bấm CNV**. Quyết định này **chưa nói** tới điểm đó — xem `product-discovery.md` §C-17 S-19.

---

## 12. Bảng tra mã quyết định

| Mã | Nội dung | Mục |
|---|---|---|
| **Đ-1** | Máy chỉ highlight, admin chấm | §3.1 |
| **Đ-2** | Điểm được phép âm | §3.2 |
| **Đ-3** | Điểm lẻ — đã xác định nguồn phát sinh, **chưa chốt** cách xử lý | §9.4 |
| **Đ-4** | Hai mode trả lời | §4 |
| **Đ-4.1** | Luật đặc tả theo mode sân khấu | §4.1 |
| **Đ-4.2** | Chọn hàng ngang: hai đường vào, hàng đợi | §5.4 |
| **Đ-4.3** | "Mở chướng ngại vật" là chuông; admin xác nhận | §5.3 |
| **Đ-4.5** | Rà soát nguồn: luật quy định thứ tự ở đâu | §6.2 |
| **Đ-4.6** | Tra cứu nguồn ngoài: "vị trí" = bốc thăm | §6.2 |
| **Đ-4.a2** | Mode ở cấp contest, một giá trị chung | §4.3 |
| **Đ-4.b** | Tăng tốc luôn nhập liệu | §4.2 |
| **Đ-4.f** | Khởi động lượt riêng: máy hiển thị câu hỏi | §9.1 |
| **Đ-4.X2** | "Hoàn toàn bằng máy tính" áp cho đáp án hàng ngang | §4.4 |
| **Đ-5** | Admin điều khiển vòng và lượt; hệ thống advisory | §6.1 |
| **Đ-5.1** | Bỏ vòng / chạy lại vòng | §6.3 |
| **Đ-5.2** | Reset = revert điểm; biên bản đầy đủ | §6.3 |
| **Đ-5.3** | Điểm = event log; revert linear | §7.1 |
| **Đ-5.3.1** | Vòng xếp hạng: một câu = một event | §3.4 |
| **Đ-5.3.X** | Revert thay thế ràng buộc undo cũ | §7.2 |
| **Đ-6** | Admin là người start timer | §8.1 |
| **Đ-6.1** | "Hiệu lệnh MC" = thao tác bấm của admin | §8.1 |
| **Đ-6.2** | Phạm vi hình phạt "mất quyền trả lời câu hỏi" | §8.3 |
| **Đ-6.3** | Mốc admin tuyệt đối; lịch sử giữ để gỡ cấm | §8.2 |
| **Đ-6.4** | Gỡ cấm trước khi trả lời, do MC điều khiển | §8.3 |
| **Đ-6.4a** | Nhánh A: MC quyết bằng lời, admin bấm | §1.1 |
| **Đ-7** | Hàng đợi tín hiệu; reject không mất lượt | §5.1 |
| **Đ-7.1** | Thứ tự do admin quyết định | §6.2 |
| **Đ-7.2** | Queue chỉ chặn ở VCNV | §5.2 |
| **Đ-8** | Rà soát nguồn lần 2 — cách đọc | §10 |
| **Đ-9** | Luồng 8 phase của Câu hỏi phụ | §9.5 |
| **Đ-10** | Suy luận cho Câu hỏi phụ | §9.5 |
| **Đ-10.6** | Mode nhập liệu: bấm chuông == nộp | §9.5 |
| **Đ-10.7** | Recommend mọi nhóm hoà, admin chọn | §9.5 |
| **Đ-11** | Suy luận toàn bộ danh sách treo | *(đề xuất — xem open-questions)* |
| **Đ-12** | Duyệt lô 31 mục tin cậy Cao | §10 |
| **Đ-13** | Ba loại câu hỏi treo phạm vi | *(xem open-questions)* |
| **Đ-14** | Suy luận cho §2B | *(đề xuất — xem open-questions)* |
| **Đ-15** | Bản nháp ba danh sách | *(xem open-questions)* |
| **Đ-16** | Thao tác ở invalid state không thực hiện được | §11.1 |
| **Đ-17** | Một câu = một phán quyết; phán quyết mở đường sang câu kế | §11.2 |
| **Đ-18** | Mỗi contest chỉ có một admin duy nhất | §11.3 |
| **Đ-19** | "Trả lời sai" và "không trả lời" là cùng một thao tác | §11.4 |
| **Đ-20** | Đồng hồ khoá thí sinh, không khoá admin | §11.5 |
| **Đ-21** | Không tồn tại trạng thái "trận tạm dừng" | §11.6 |
| **Đ-22** | Tín hiệu thí sinh không làm gián đoạn đồng hồ; hoãn HIỂN THỊ chứ không hoãn thời gian | §11.7 |
