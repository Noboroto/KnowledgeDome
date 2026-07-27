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
> **Ngày chốt**: 2026-07-24 và 2026-07-25 (đợt sau: Đ-16 → Đ-35, xem §11).

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

> **Cập nhật 2026-07-25**: `Đ-31` thêm **một ngưỡng chặn cứng thứ ba** — thiếu đề thì không mở được vòng. Đây là chỗ **ghi đè tiền lệ Athena**, xem §11.16.

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
| Timer | **Chạy hết**, không dừng giữa chừng `[Đ-6.4c]` |
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
| **Admin** | **KHÔNG khoá**. **Hai ngoại lệ**: nút **start timer** tự khoá sau lần bấm đầu; và ở vòng thí sinh **gõ đáp án** thì nút **chấm** khoá tới khi hết giờ (`Đ-35`) |

⇒ Mốc thời gian của luật là **ràng buộc với thí sinh**, **mốc tham chiếu** với người điều khiển. "Trong hạn hay quá hạn" ở phía phán quyết là đánh giá của MC và admin.

⇒ Đóng `game-rules-review.md` GRR-137. Đây cũng là **lý do** đứng sau Đ-16: giải thích vì sao phía thí sinh bị khoá mà phía admin thì không.

### 11.6 Đồng hồ chạy liên tục; trận dừng bằng cách admin ngừng thao tác `[Đ-21]`

Mọi thao tác đều do admin thực hiện ⇒ khi trận cần dừng thì **admin ngừng thao tác**, hệ thống không cần biết điều đó. **Bãi bỏ `R-GEN-08`.**

Ba điều hệ thống **không** làm:

- **Không đóng băng đồng hồ.** Một cửa sổ thời gian đã mở thì chạy hết theo server time.
- **Không tự dừng** khi admin hay thí sinh mất kết nối, ở bất kỳ ngưỡng nào.
- **Không có cấu hình ngưỡng** nào cho hai việc trên.

Máy trạng thái trận có **bốn** giá trị: `LOBBY` · `rounds[i]` · `TIE_BREAK` · `FINISHED`.

**Van thoát khi sự cố rơi vào giữa một cửa sổ có ràng buộc thời gian** (Tăng tốc, cướp quyền Về đích): dùng cơ chế đã có — admin **bỏ hoặc chạy lại vòng** (`Đ-5.1`); lưu ý câu đã dùng **không** trả lại kho đề (`Đ-5.2f`).

⇒ Đóng `game-rules-review.md` GRR-165, GRR-166, GRR-167 và bãi bỏ U-12.

### 11.7 Tín hiệu thí sinh không làm gián đoạn đồng hồ `[Đ-22]`

Khi có người bấm **"Mở chướng ngại vật"** giữa lúc đồng hồ hàng ngang đang chạy:

1. **Ghi nhận ngay** tín hiệu.
2. **Đồng hồ vẫn chạy bình thường** — không đóng băng, không kéo dài.
3. **Không lộ gì thêm cho tới khi admin bấm hiển thị** — ẩn **cả hai**: (a) **đáp án chuẩn của chương trình** cho hàng ngang đang chạy, (b) **bài làm của các thí sinh khác**.

Vế 3 là điều kiện để hai vế đầu an toàn, và phải ẩn **cả hai** nguồn dữ kiện mới đủ:

| Nếu lộ | Người bấm CNV được lợi gì |
|---|---|
| Đáp án chuẩn của chương trình | Biết luôn hàng ngang đó là gì — một gợi ý trọn vẹn mà luật không cho |
| Bài làm của thí sinh khác | Suy ra hàng ngang qua phỏng đoán của đối thủ, kể cả khi những phỏng đoán đó sai |

Ẩn một cái mà lộ cái kia thì cơ chế vẫn hỏng.

⇒ Đóng `game-rules-inventory.md` U-16 (đề xuất *"dừng đồng hồ hàng ngang"* — **bác bỏ**).

**Tiền lệ `Athena-Intelligent-Olympia`** — cùng cách xử lý, đã chạy thật:

| Nơi | Hành vi |
|---|---|
| `AICtrlLib/ObstacleUI.cs` `Obstacle_Click` | Nút CNV **không kiểm tra state**, bấm lúc nào cũng gửi ngay |
| `AICtrlLib/ObstacleUI.cs` `RowAnswerTextBox_PreviewKeyDown` | Ô nhập hàng ngang **chỉ nhận phím khi đang đếm giờ**; ngoài giờ chặn và xoá trắng — đúng tiền lệ của `Đ-20` và `Đ-16` |
| `AIServer/Obstacle.cs` `Timer_Tick` | Nhận lệnh `obstacle` ở **mọi** state, **không đụng tới đồng hồ**; pha đếm giờ vẫn chạy đủ 15 giây |
| `AIServer/Obstacle.cs` `ShowAnswer_Click` | Bài làm của thí sinh **chỉ hiện khi admin bấm**, không tự hiện lúc hết giờ |
| `AICtrlLib/ObstacleUI.cs` dòng 104 | Đáp án chuẩn bị **ẩn trên máy thí sinh**: `AnswerTextBlock` chỉ hiện với client số 4 (màn MC), các ghế 0-3 thì `Collapsed` — khớp `Đ-15.2` / `R-GEN-10` (đáp án chỉ tới admin và MC) |

**Khác biệt cần biết**: Athena còn **tự khoá ô nhập hàng ngang của chính người vừa bấm CNV**. Quyết định này **chưa nói** tới điểm đó — xem `product-discovery.md` §C-17 S-19.

### 11.8 Hai tín hiệu cùng mốc thời gian: hàng đợi tự quyết định `[Đ-23]`

Khi hai ghế khác nhau có **cùng server timestamp** ở vòng mà quyền trả lời **không chia được** (Khởi động lượt chung, cướp quyền Về đích, Câu hỏi phụ):

> **Cứ ngẫu nhiên — hàng đợi tự quyết định thứ tự.** Không ưu tiên theo số ghế, vị trí, hay bất kỳ tiêu chí nào khác.

**Điều kiện giữ cho quyết định này an toàn**: thứ tự mà hàng đợi chọn **phải được ghi lại**. Điều kiện đó đã có sẵn ở `Đ-7` (hàng đợi ghi theo thứ tự tới, lịch sử không bao giờ xoá).

⇒ Ngẫu nhiên **lúc nhận**, nhưng **tất định khi dựng lại**: replay event log đọc đúng thứ tự đã lưu và ra lại kết quả cũ. `Đ-5.3` (*"điểm là hàm của event log"*) **không bị phá** — đây chính là mối lo mà `game-rules-review.md` GRR-138 nêu, nay đã được khoá lại.

**Không áp cho vòng xếp hạng.** Ở Tăng tốc, **điểm chia được** nên cùng thời gian thì **cùng mức điểm** (K-8) — không cần và không được phân thứ tự ngẫu nhiên ở đó.

⇒ Đóng `game-rules-review.md` GRR-138.

### 11.9 Nút chuông tự khoá ngay khi bấm `[Đ-24]`

**Frontend khoá nút chuông ngay trong lần bấm đầu — trong cùng thao tác đó, TRƯỚC khi gửi tín hiệu.**

⇒ Một ghế chỉ phát được **một** tín hiệu chuông cho một câu. Hàng đợi không bao giờ nhận tín hiệu chuông trùng. Khoá gắn với **một CÂU**; sang câu mới thì mở lại.

**Không mâu thuẫn `CLAUDE.md` §UX** (*"KHÔNG chặn gửi lại, nút action không disable"*): quy tắc đó nhắm chống double-submit, còn đây là **khoá-theo-LUẬT-CHƠI** — chính `CLAUDE.md` nêu ngoại lệ tường minh *"chuông bị khoá khi sai, NSHV đã dùng, không tới lượt"*. Đã phát tín hiệu thì quyền đã được định đoạt, đúng là trạng thái game.

**Phạm vi**: mọi nút được xếp là **chuông**, gồm cả **"Mở chướng ngại vật"** (`Đ-4.3`). **KHÔNG** áp cho thao tác **chọn hàng ngang** — đó không phải chuông, và vẫn còn để mở.

**Tiền lệ `Athena-Intelligent-Olympia`** — cùng cách làm ở cả ba vòng có chuông, khoá đồng bộ ngay trong handler trước lệnh gửi:

| Nơi | Dòng |
|---|---|
| `AICtrlLib/ExtraUI.cs` `Answer_Click` (Câu hỏi phụ) | 238 |
| `AICtrlLib/ObstacleUI.cs` `Obstacle_Click` (VCNV) | 515 |
| `AICtrlLib/FinishUI.cs` (Về đích) | 395, 551 |

⇒ Đóng `game-rules-review.md` GRR-140.

### 11.10 Tín hiệu đến sau khi đã có người giành quyền: ghi nhận nhưng TRƠ `[Đ-25]`

Ở vòng hàng đợi **không chặn** (Khởi động lượt chung, cướp quyền Về đích), khi đã có người giành quyền mà người khác vẫn bấm:

> **Chỉ đưa vào hàng đợi, để admin có căn cứ xử lý.** Không tự động trao quyền, không mở lại chuông, không sinh hệ quả điểm.

Tín hiệu là **bằng chứng**, không phải hành động. Người bấm đầu giữ nguyên quyền trả lời.

Đúng vai mà `Đ-7.2` đã đặt cho hàng đợi ở các vòng không chặn: *"queue chỉ là lưới an toàn cho sự cố"* — nay nói rõ lưới đó dùng để làm gì.

**Không tự phân xử tranh cãi cũ**: quy tắc *"câu không mở lại chuông cho người khác"* chỉ có trong tài liệu repo, nguồn gốc `F26` im lặng (`game-rules-inventory.md` §R-KD-03). Quyết định này **không** biến nó thành ràng buộc cưỡng chế — hệ thống chỉ không **tự động** mở lại; còn admin có trao quyền cho người kế tiếp hay không vẫn là phán quyết của người, đúng nguyên tắc advisory (`Đ-5`).

**Hệ quả phụ**: mục *Evaluation order* của GR-003 khép lại — ba điều kiện (cửa sổ đã đóng · ghế không còn quyền · đã có người giành quyền) nay cho outcome độc lập, không xung đột, nên kiểm theo thứ tự nào cũng ra cùng kết quả.

### 11.11 "Hiển thị câu hỏi" và "start timer" là hai thao tác riêng `[Đ-26]`

**Hai nút tách rời, thứ tự cố định**: hiển thị câu hỏi **trước**, start timer **sau**. Không gộp, không đảo.

| Mốc | Mở / đóng cái gì |
|---|---|
| **Admin bấm hiển thị câu hỏi** | Đưa câu lên màn thí sinh và viewer · **mở cửa sổ chuông** · **đóng** cửa sổ đặt Ngôi sao hy vọng (`game-rules-review-old.md` GRR-048) |
| **Admin bấm start timer** | Mốc *"MC đọc xong"* (`Đ-6`) · bắt đầu đếm thời gian suy nghĩ |

Khoảng **giữa hai mốc** chính là lúc MC đọc — đúng quãng mà `F26` cho phép bấm chuông ở Khởi động lượt chung, và là cách hiện thực `game-rules-review-old.md` GRR-005 (*"cửa sổ chuông = thời gian MC đọc + 3 giây"*).

**Hệ quả**: *"bấm chuông trước khi câu được đưa ra"* **không tồn tại** — trước mốc thứ nhất, màn thí sinh chưa có gì và chuông chưa sống (`Đ-16`). Biên dưới của cửa sổ chuông nay xác định: **mốc admin bấm hiển thị**.

**Tiền lệ `Athena-Intelligent-Olympia`**: hai thao tác luôn tách rời —

| Vòng | Hiển thị câu | Bắt đầu đếm giờ |
|---|---|---|
| VCNV | `AIServer/Obstacle.cs` `Row_Click` (hiện số ký tự rồi nội dung câu) | `StartTime_Click` |
| Câu hỏi phụ | `AICtrlLib/ExtraUI.cs` lệnh `question` (bật nút chuông, State = Pausing) | lệnh `start` (State = OnTime) |

*Lưu ý khi đọc Athena*: vòng Khởi động của bản cũ là format 60 giây mỗi thí sinh, **không có chuông** (đã bãi bỏ theo K-15) — nên tiền lệ lấy từ hai vòng trên, không lấy từ `StartUI.cs`.

⇒ Đóng `game-rules-review.md` GRR-139.

### 11.12 Chấm xong là kết thúc câu; xoá hàng đợi và gỡ khoá chuông `[Đ-27]`

Sau khi người giành quyền bị chấm **Sai** ở Khởi động lượt chung:

> **KHÔNG mở lại chuông cho người khác.** Câu kết thúc, chuyển sang câu mới. Khi câu đóng: **xoá hàng đợi đang hoạt động** và **gỡ khoá chuông** cho mọi ghế.

**Phân xử một tranh cãi cũ**: quy tắc *"câu không mở lại chuông cho người khác"* trước nay **chỉ có trong tài liệu repo** (`game-rules-inventory.md` §R-KD-03), `F26` im lặng. Nay được xác nhận là hành vi hệ thống.

**Ba điều cần phân biệt cho đúng:**

| Thứ | Có bị xoá không |
|---|---|
| **Hàng đợi đang hoạt động** của câu vừa xong | **CÓ** — xoá khi câu đóng |
| **Khoá chuông** của từng ghế (`Đ-24`) | **Gỡ** — khoá gắn với một câu, sang câu mới thì mở lại |
| **Lịch sử tín hiệu**, gồm cả tín hiệu trơ theo `Đ-25` | **KHÔNG BAO GIỜ** (`Đ-7`) — admin vẫn xem lại được sau khi câu đã đóng |

**Làm mịn `Đ-7.b`**: quyết định cũ nói hàng đợi đang hoạt động reset **sau mỗi VÒNG**. Ở vòng có chuông, reset diễn ra **sau mỗi CÂU** — mịn hơn một bậc, không mâu thuẫn với `Đ-7.b` mà là trường hợp riêng chặt hơn.

### 11.13 Biên thời gian là biên ĐÓNG `[Đ-28]`

> Tín hiệu hoặc submission có server timestamp **đúng bằng** mốc hết giờ ⇒ **vẫn hợp lệ**. Chỉ **vượt quá** mốc mới bị loại.

Áp **thống nhất cho cả hai loại**, không có ngoại lệ theo vòng:

| Loại | Đúng mốc | Quá mốc |
|---|---|---|
| **Tín hiệu giành quyền trả lời** (chuông) | Tính là giành quyền | Không giành được |
| **Submission đáp án** | Được chấm | **Mặc định** không tính, không tham gia xếp hạng |

**Quá hạn KHÔNG phải là phán quyết của máy.** Bản quá hạn vẫn vào **lịch sử trên màn admin**, đánh dấu **ĐỎ** để phân biệt với bản hợp lệ. Hệ thống **chỉ đánh dấu, không chặn cứng** — hai việc sau đều là **thao tác bấm của admin**:

1. **Hiển thị** bản đó ra màn thí sinh và viewer hay không (nhất quán `Đ-22`).
2. **Chấm điểm** bản đó hay không — admin có thể quyết định vẫn công nhận.

⇒ Nhất quán với `Đ-1`: máy không tự chấm, **kể cả chấm loại**. "Quá hạn" là một **nhãn**, không phải một quyết định.

> **Lưu ý ở vòng xếp hạng**: nếu admin công nhận một bản quá hạn ở Tăng tốc thì **cả bảng xếp hạng của câu đó tính lại** — theo `Đ-5.3.1`, một câu là một event điểm cho toàn bộ người chơi.

**Tiền lệ `Athena-Intelligent-Olympia`**: hiển thị bài làm luôn là một nút riêng của admin — `AIServer/Acceleration.cs` `Answer_Click` (gửi lệnh `show`) ở Tăng tốc, `AIServer/Obstacle.cs` `ShowAnswer_Click` ở VCNV.

⇒ Đóng `game-rules-review-old.md` GRR-002 và GRR-035. Hai mục này nằm cùng nhóm "Quy ước biên thời gian" ở `game-rules-open-questions.md` Tầng 2, vốn được ghi chú *"nên quyết cùng lúc để khỏi sinh ba quy ước khác nhau"* — nay đã thống nhất.

**Còn lại của nhóm đó**: `GRR-031` — độ phân giải so sánh ở Tăng tốc (ms / 10ms / µs). `K-8` đã chốt **ms**, nhưng giá trị mặc định của `tieRule` trong SPEC thì chưa.

### 11.14 Nút thao tác một chiều tự tắt sau khi bấm `[Đ-29]`

Mẫu chung đã được chốt lặp lại cho bốn nút, nay phát biểu thành một quy tắc:

> Nút thực hiện một **bước không quay lại** sẽ **tự tắt ngay khi bấm**, và bước kế tiếp **bật lên** thay thế.

| Nút | Bấm xong thì | Mã |
|---|---|---|
| **Đúng / Sai** | Tắt; nút "Câu kế tiếp" bật | `Đ-17` |
| **Start timer** | Tắt (chống bấm trùng tạo mốc) | `Đ-20` |
| **Chuông** (gồm "Mở chướng ngại vật") | Tắt cho câu đó; sang câu mới mở lại | `Đ-24` |
| **Chuyển câu** | Tắt; nút "Hiển thị câu hỏi" của câu mới bật | *(quyết định 2026-07-25)* |

⇒ Không tồn tại "bấm nhầm hai lần" ở bất kỳ nút nào trong bảng.

**CHƯA áp cho** các nút chưa được chốt — mẫu này **không tự lan sang chúng**: nút **kết thúc vòng** (`game-rules-review.md` GRR-163 vẫn mở), mở miếng ghép, công bố Chướng ngại vật, xác nhận tín hiệu trong hàng đợi, bỏ vòng / chạy lại vòng (GRR-160).

> Phân biệt kỹ: nút **"chuyển câu"** đã chốt ở bảng trên; nút **"kết thúc vòng"** thì chưa. Hai thao tác khác nhau, dù cùng nằm trên màn điều khiển.

### 11.15 Luật cho bao nhiêu câu thì đúng bấy nhiêu `[Đ-30]`

> **Hệ thống không bao giờ tự sinh câu thứ N+1.** Khi đã hỏi đủ số câu quy định, nút "Câu kế tiếp" **chuyển thành nút kết thúc** — kết thúc lượt, kết thúc lượt chung, hoặc kết thúc vòng tuỳ ngữ cảnh.

| Vòng | Số câu theo luật |
|---|---|
| Khởi động — lượt riêng | **6** câu mỗi thí sinh |
| Khởi động — lượt chung | **12** câu |
| VCNV | **4** hàng ngang + **1** ô trung tâm |
| Tăng tốc | **4** câu |
| Về đích | **3** câu mỗi gói |
| Câu hỏi phụ | **3** câu |

**Hệ quả cho đặc tả**: biên *"lớn hơn max"* của mọi vòng là **KHÔNG TỒN TẠI**. Trước đây tài liệu để ngỏ *"chặn cứng hay chỉ cảnh báo"* — cả hai đều thừa, vì không có đường vào nào tạo ra được tình huống đó.

**Không lẫn với `U-3`**: cấu hình `rowCount` 5-8 ở VCNV là **đổi chính con số của luật** (ngoài luật O26, thang điểm cho cấu hình đó vẫn chưa định nghĩa — U-3 còn mở). Quyết định này chỉ nói: **đã ấn định bao nhiêu thì chạy đúng bấy nhiêu**, không tự vượt.

**Quan hệ với `Đ-15.3`** (ngưỡng chặn cứng): không thêm ngưỡng chặn nào. Đây là **thiếu đường vào**, không phải **chặn đường vào** — nên nguyên tắc *"chỉ chặn cứng ở ngưỡng bất khả thi vật lý"* vẫn nguyên vẹn.

### 11.16 Kho đề kiểm tại cửa vào từng vòng `[Đ-31]`

> **Không đủ số câu ⇒ KHÔNG BẮT ĐẦU ĐƯỢC VÒNG ĐÓ.** Các vòng khác vẫn bắt đầu bình thường.

⇒ **"Kho đề cạn giữa vòng" KHÔNG TỒN TẠI.** Nhu cầu của một vòng là **con số cố định** (`Đ-30`) và được kiểm đủ ngay tại cửa vào; câu bị bỏ qua vẫn nằm trong con số đó. **Chạy lại** một vòng cũng phải qua đúng cửa đó.

⇒ Đóng `game-rules-inventory.md` **U-9** (pool cạn giữa trận do skip nhiều) và **U-10** (pool câu phụ cạn giữa tie-break).

**Trả lời luôn `Đ-5.g` / `S-12`** (*"pre-flight chạy theo vòng hay theo cả playlist"*): **theo VÒNG**, tại thời điểm mở vòng — không phải một lần trước trận.

**Hai điều cần ghi rõ vì chúng đổi quyết định cũ:**

1. **Đây là chỗ chặn cứng THỨ BA.** `Đ-15.3` từng kết luận *"chỉ hai ngưỡng chặn cứng thật"* (cửa sổ cướp ≥2 và Câu hỏi phụ ≥2, đều về số người). Nay thêm một ngưỡng về **tài nguyên đề**. Tiêu chí cũ vẫn được tôn trọng: một vòng không đủ câu thì **không chạy trọn được**, tức là vòng đó **không còn nghĩa** — đúng định nghĩa "bất khả thi vật lý" của `§1.3`.

2. **Ghi đè tiền lệ Athena.** `§1.3` ghi nhận bản cũ *"thiếu đề chỉ CẢNH BÁO, không chặn"* — 5 chỗ cảnh báo về kho đề, không chỗ nào chặn. Nay thiếu đề **chặn ở cấp vòng**. Đây là khác biệt có chủ đích, không phải sao chép thiếu.

**Phạm vi chặn là VÒNG, không phải TRẬN**: trận vẫn chạy các vòng còn đủ đề. Admin thấy được thiếu bao nhiêu câu để bổ sung rồi mở lại.

### 11.17 Luôn ghi nhận đáp án cuối cùng `[Đ-32]`

> **Nút gửi KHÔNG bị khoá sau khi gửi.** Thí sinh sửa và gửi lại bao nhiêu lần cũng được; **bản cuối cùng** là bản được ghi nhận. Cửa duy nhất đóng lại là **hết giờ**.

**Hai loại nút, hai chế độ ngược nhau** — đây là chỗ dễ áp nhầm nhất:

| Nút | Sau khi bấm | Vì sao |
|---|---|---|
| **Chuông** | **Tự khoá** (`Đ-24`) | Giành quyền là hành vi **một lần, không rút lại** |
| **Gửi đáp án** | **KHÔNG khoá** | Đáp án là thứ **sửa được**; chỉ đồng hồ đóng cửa |

**Không mâu thuẫn luật gốc.** `F26` viết *"đáp án cuối cùng sẽ được ghi nhận. Nếu không thay đổi, chương trình sẽ ghi nhận đáp án đầu tiên."* Vế sau **không phải ngoại lệ** — gửi một lần thì bản đầu chính là bản cuối. Quyết định này chỉ phát biểu tổng quát điều luật gốc đã nói.

**Giữ nguyên `Đ-20.1`**: bản rỗng (chỉ khoảng trắng sau khi cắt) **không phải một đáp án** ⇒ bỏ qua, giữ bản hợp lệ trước đó. Gửi rỗng **không xoá được** bài đã làm.

**Bản gửi QUÁ HẠN: giữ cả hai, admin phán quyết**

*"Ghi nhận bản cuối"* chỉ áp trong **các bản hợp lệ**. Bản quá hạn **không tự ghi đè** bản hợp lệ:

| Tình huống | Hệ thống làm gì | Lựa chọn của admin |
|---|---|---|
| Có bản **hợp lệ**, còn gửi thêm bản **quá hạn** | Giữ **CẢ HAI**; bản quá hạn tô **ĐỎ** | **Đúng / Sai** |
| **Chỉ có** bản quá hạn (không gửi gì trong hạn) | Giữ bản quá hạn, tô **ĐỎ** | **Đúng / Sai / HUỶ KẾT QUẢ** |

> **`Huỷ kết quả` là outcome THỨ BA của thao tác chấm**, và chỉ tồn tại ở tình huống thứ hai. Nó **khác `Sai`**: ở Khởi động lượt chung `Sai` kéo theo **−5 điểm**, còn huỷ thì câu **không sinh điểm nào**. Đây là lần đầu tài liệu có một phán quyết không thuộc cặp Đúng/Sai — mọi chỗ mô tả thao tác chấm như nhị phân cần đọc lại theo mục này.

⇒ Làm rõ `Đ-28`: *"mặc định không tính, đánh dấu đỏ, admin quyết"* nay nói cụ thể **admin thấy gì** (cả hai bản, cạnh nhau) và **chọn được gì** (hai hay ba lựa chọn tuỳ tình huống).

**Quy tắc *"nội dung y hệt thì không cập nhật mốc thời gian"*** chỉ có ý nghĩa ở vòng **xếp hạng theo tốc độ** (Tăng tốc), nơi timestamp quyết định thứ hạng. Vòng không xếp theo thời gian thì không cần áp.

⇒ Nhất quán với `CLAUDE.md` §UX (*"KHÔNG chặn gửi lại; server nhận bản cuối cùng trước timeout"*) và với `Đ-20` (đồng hồ là thứ duy nhất khoá thao tác của thí sinh).

### 11.18 Mốc "MC công bố đáp án" là một thao tác bấm của admin `[Đ-33]`

> Hệ thống **không cần quan sát sân khấu**. Mốc cắt của việc ghi nhận đáp án được xác định bằng **thời điểm admin bấm**, ghi bằng server timestamp.

Đây là mảnh cuối của `Đ-6` (*"mọi mốc thời gian mà luật gốc mô tả bằng hành vi của MC đều ánh xạ thành MỘT thao tác bấm của admin"*). `Đ-26` đã ánh xạ hai mốc đầu; mốc thứ ba nay khép lại:

| Mốc trong luật gốc | Thao tác hệ thống |
|---|---|
| *"MC đọc xong câu hỏi"* | Admin bấm **start timer** (`Đ-6`) |
| *"câu hỏi được đọc lên hoặc hiện lên màn hình"* | Admin bấm **hiển thị câu hỏi** (`Đ-26`) |
| *"MC công bố đáp án"* | Admin bấm **công bố đáp án** |

⇒ Đóng `game-rules-inventory.md` **U-1** — cả ba mốc của MC nay đều có thao tác bấm tương ứng.

### 11.19 Phán quyết chỉ nhị phân khi "Sai" trừ 0 điểm `[Đ-34]`

> **Tiêu chí**: chỗ nào `Sai` kéo theo **hình phạt điểm** thì thao tác chấm phải có **ba** lựa chọn — **Đúng / Sai / Huỷ kết quả**.

| Vòng / pha | `Sai` trừ | Lựa chọn |
|---|---|---|
| Khởi động lượt riêng · VCNV hàng ngang · Tăng tốc · Về đích (người thi chính) · Câu hỏi phụ | **0** | Đúng / Sai |
| **Khởi động lượt chung** | **−5** | Đúng / Sai / **Huỷ** |
| **Về đích — người cướp quyền** | **−½ giá trị câu** | Đúng / Sai / **Huỷ** |
| **Về đích — câu có Ngôi sao hy vọng** | **−giá trị câu** | Đúng / Sai / **Huỷ** |

**Vì sao cần lựa chọn thứ ba**: ở những chỗ có hình phạt, cặp Đúng/Sai ép admin chọn giữa **cho điểm** và **phạt** — trong khi tình huống thực tế có thể không đáng cả hai (sự cố thiết bị, MC đọc nhầm, tranh cãi chưa ngã ngũ). `Huỷ kết quả` cho câu **không sinh điểm nào**, khác `Sai` ở chỗ không áp hình phạt.

**Ngoài ra** `Huỷ kết quả` **luôn có mặt** khi câu chỉ có bản gửi quá hạn (`Đ-32`), kể cả ở vòng mà `Sai` trừ 0.

⇒ Mở rộng `Đ-17`: một câu vẫn chỉ đi qua **một** phán quyết, nhưng tập lựa chọn là **hai hoặc ba** tuỳ ngữ cảnh. Mọi mô tả *"hai nút Đúng/Sai"* trong tài liệu cần đọc lại theo mục này.

### 11.20 Phán quyết của admin là quyết định cuối cùng `[Đ-35]`

> **Chấm xong thì KHOÁ NÚT GỬI.** Không còn bản nào tới sau để lật kết quả.

Để phán quyết không bao giờ rơi vào giữa lúc thí sinh còn đang sửa, luồng **tách theo kênh trả lời của vòng**:

| Kênh trả lời | Nút gửi của thí sinh | Nút chấm của admin |
|---|---|---|
| **Nói** — mode sân khấu | **Không tồn tại** (thí sinh đọc đáp án) | Bấm được **bất cứ lúc nào** |
| **Gõ** — mode nhập liệu, và các vòng **luôn gõ máy** | Sống tới khi hết giờ | **KHOÁ tới khi hết giờ** |

Hai nhánh đều triệt tiêu tranh chấp: nhánh trên **không có bản gửi nào** để đổi; nhánh dưới thì admin **chỉ chấm sau khi cửa nhận đáp án đã đóng**.

**Áp cho vòng nào**: `Đ-4.2` (VCNV hàng ngang) và `Đ-4.b` (Tăng tốc) chốt hai vòng này **luôn gõ máy bất kể mode contest** ⇒ chúng **luôn** theo nhánh dưới. Khởi động, Về đích và Câu hỏi phụ đi theo mode của contest (`Đ-4.a2`).

**⚠ Sửa `Đ-20`**: quyết định đó nói *"đồng hồ KHÔNG khoá gì cả với admin — ngoại lệ duy nhất là nút start timer"*. Nay có **ngoại lệ thứ hai**: ở vòng thí sinh gõ đáp án, **nút chấm khoá tới khi hết giờ**. Câu "ngoại lệ duy nhất" trong `Đ-20` đã lỗi thời.

⇒ Đóng nhánh cuối của GR-006 và đóng `game-rules-review.md` **GRR-148** (*"thao tác chấm ở Tăng tốc có bị khoá tới khi server-timeout không?"* — **có**).

---

### 11.21 Chọn hàng ngang: một đường vào mỗi mode, dedup bằng dialog phía thí sinh `[Đ-36]`

> **Thí sinh click chọn hàng ngang nhiều lần** — xử lý **tách theo mode**, không phải một quy tắc chung.

| Mode | Ai chọn hàng ngang | Thí sinh click nhiều lần |
|---|---|---|
| **Sân khấu** | **Chỉ ADMIN.** Máy thí sinh **không có** nút chọn | **KHÔNG TỒN TẠI** — không có nút thì không có tín hiệu |
| **Nhập liệu** | **Chỉ THÍ SINH.** **Admin KHÔNG chọn thay** | **Dialog xác nhận** ở phía thí sinh, xác nhận xong thì **khoá nút chọn** |

**Vì sao chỗ này được phép có dialog ở phía thí sinh**: thao tác chọn hàng ngang là thao tác **một chiều, hậu quả nặng, KHÔNG bị ép thời gian** — đúng tiêu chí "queue chặn" đã ghi ở `CLAUDE.md` §UX, và ngược hẳn với chuông (đua tốc độ, cửa sổ chặt). Không có gì để đua thì có chỗ cho một lớp xác nhận.

**⚠ Sửa `CLAUDE.md` §UX — hai câu tuyệt đối cũ đã lỗi thời:**

| Câu cũ | Nay thành |
|---|---|
| *"Dialog xác nhận **CHỈ** đặt ở phía ADMIN, **KHÔNG BAO GIỜ** ở phía thí sinh"* | Vẫn đúng cho **mọi thao tác đua tốc độ**; **ngoại lệ duy nhất** là **chọn hàng ngang ở mode nhập liệu** |
| *"Chọn hàng ngang: **hai đường vào** (thí sinh click **hoặc** admin click)"* | **Một đường vào cho mỗi mode** — không mode nào có hai. Tình huống "hai đường vào tranh nhau" biến mất |

**⚠ Làm mịn `Đ-4.2`**: quyết định đó chốt "hai đường vào"; nay hai đường **không cùng tồn tại**, mà phân theo mode.

**Khoá nút là TẠM, không vĩnh viễn** — hệ quả cưỡng bức bởi `Đ-7` (*"admin từ chối ⇒ thí sinh KHÔNG mất lượt"*): nếu khoá vĩnh viễn thì thí sinh bị từ chối không chọn lại được, tức là mất lượt. Vậy:

| Mốc | Nút chọn của thí sinh |
|---|---|
| Thí sinh xác nhận dialog | **Khoá** — chống click lặp trong lúc chờ admin duyệt |
| Admin bấm **Yes** | Khoá **giữ nguyên** — lượt chọn đã dùng |
| Admin bấm **No** | **MỞ LẠI** — thí sinh chọn lại được, không mất lượt |

**Hai lớp xác nhận không thừa nhau**: dialog phía thí sinh chống **bấm nhầm**; admin duyệt Yes/No là **phán quyết** (`Đ-7`). Queue chặn ở VCNV giữ nguyên ở cả hai mode.

⇒ Đóng nhánh idempotency của GR-007 (thao tác chọn hàng ngang trước nay nằm ngoài phạm vi `Đ-24` vì không phải chuông).

**Còn treo**: ở mode nhập liệu, nút chọn có hiện trên máy thí sinh **chưa tới lượt** không? `Đ-16` (invalid state → máy thí sinh không hiển thị gì) đẩy về "không hiện"; còn nhánh "tín hiệu sai lượt vào hàng đợi, cảnh báo không chặn cứng" (`Đ-5`) giả định tín hiệu tới được server. Chưa quyết ⇒ GR-007 C7 giữ nguyên.

---

### 11.22 Danh sách câu đã gán sửa được tại CỬA VÀO VÒNG `[Đ-37]`

> **Admin thêm / bớt câu hỏi trong danh sách đã gán ở `LOBBY`** (cửa vào vòng) — tức **trước khi mở bất kỳ vòng nào**. Trong lúc một vòng đang chạy thì **không**.

✅ **Chủ dự án chốt 2026-07-27.**

**Đây là PHÂN XỬ, không phải luật mới.** Ba phát biểu đang có, hai cái đã giả định sẵn việc này và chỉ một cái nói ngược:

| Nguồn | Phát biểu | Hàm ý |
|---|---|---|
| `Đ-31` (§11.16, câu cuối) | *"Admin thấy được thiếu bao nhiêu câu để **bổ sung rồi mở lại**."* | Sửa được **giữa trận** |
| `game-rules.md` §GR-025 Error outcomes | *"vòng không mở được; admin chuyển sang phương án ngoài hệ thống hoặc **bổ sung đề rồi mở lại**"* | Sửa được **giữa trận** |
| `game-rules.md` §GR-031 No-change guarantees | *"**Danh sách gán (snapshot) KHÔNG bị sửa** giữa trận"* | **Không** sửa được |

⇒ Phát biểu thứ ba là **chỗ lệch**, và nó lệch vì được viết khi pre-flight còn chạy **một lần trước trận**. `Đ-31` đã chuyển pre-flight sang **cửa vào từng vòng** nhưng câu đó không được rà lại. **Sửa GR-031 cho khớp `Đ-31`**, không phải ngược lại.

**Vì sao mốc là CỬA VÀO VÒNG, không phải "bất cứ lúc nào":**

- `Đ-31` đặt điểm kiểm kho đề ở cửa vào vòng. Cho sửa **đúng ở nơi kiểm** thì việc kiểm mới có lối thoát; cho sửa ở giữa vòng thì phép kiểm mất nghĩa vì tài nguyên đổi sau khi đã kiểm.
- `Đ-30` (*luật cho bao nhiêu câu thì đúng bấy nhiêu*) khiến nhu cầu của một vòng là **con số cố định, biết trước**. Đã qua được cửa vào thì trong vòng không cần thêm câu — nên **không có lý do nghiệp vụ** để sửa giữa vòng.
- Giữ được tính **tái dựng**: trong suốt một vòng, tập câu khả dụng là bất biến, nên replay event log ra đúng kết quả cũ (`Đ-5.3`).

**Bớt một câu ĐÃ ĐƯỢC HIỂN THỊ: KHÔNG cho phép** (chốt cùng ngày).

- Câu đã hiển thị là câu **đã tiêu** (`GRR-085`) và **không bao giờ trả lại kho** (`Đ-5.2f`). Gỡ nó khỏi danh sách gán sẽ tạo ra một đường lách: gỡ ra rồi thêm lại ⇒ hỏi lại một câu đã lộ, phá quy tắc no-repeat toàn contest (`R-GEN-06`).
- **Phân loại: INVALID STATE, không phải chặn cứng thứ tư.** Theo `Đ-16`, invalid state là *"thao tác không tồn tại ở trạng thái hiện tại"* — một câu đã tiêu **không còn là mục tiêu** của thao tác gỡ. Máy admin hiện **toast**, không ép được. ⇒ Số ngưỡng **chặn cứng vẫn là BA** (`Đ-15.3` hai ngưỡng số người + `Đ-31` cửa vào vòng); `NT-D` của `game-rules-resolutions.md` **không phải sửa**.
- Câu **đã rút nhưng CHƯA hiển thị** thì **gỡ được** — theo `GRR-118` nó là câu **chưa tiêu, trả lại kho**.

**Ranh giới với `Đ-5.2f`**: gỡ một câu khỏi danh sách gán chỉ có nghĩa **"không rút nữa"**. Nó **không** xoá cờ đã-dùng, **không** hoàn tác việc câu đã lộ, và **không** đụng no-repeat ở cấp contest.

⇒ **Hệ quả cấp sản phẩm** (giao diện, phân quyền, audit) ghi ở `docs/product-discovery.md` **C-18**.

---

### 11.23 `LOBBY` là CỬA VÀO VÒNG — trạng thái nghỉ duy nhất của trận `[Đ-38]`

> **Máy trạng thái trận có bốn giá trị**: `LOBBY` · `rounds[i]` · `TIE_BREAK` · `FINISHED`.
> **`LOBBY` = cửa vào vòng**: trạng thái nghỉ của trận, dùng **cả trước vòng đầu tiên lẫn giữa hai vòng**.

✅ **Chủ dự án chốt 2026-07-27.**

**Điều kiện VÀO `LOBBY`:**

| Đường vào | Ghi chú |
|---|---|
| Trận được tạo | Lần đầu, chưa vòng nào chạy |
| **Mọi nút "Kết thúc vòng"** | **Mọi** vòng kết thúc đều về đây, không vòng nào là ngoại lệ |
| Bỏ vòng · chạy lại vòng | `Đ-5.1` |

**Điều kiện RA — ba nhánh:**

| Đường ra | Điều kiện |
|---|---|
| Mở **bất kỳ vòng nào** | Cửa vào vòng đủ câu (`Đ-31`). Admin chọn vòng; playlist chỉ là gợi ý (`Đ-5`) |
| → `TIE_BREAK` | Hết playlist **và** hoà ở `tieBreakPositions` |
| → `FINISHED` | Hết playlist, không hoà |

**Thao tác cho phép tại `LOBBY`:** sửa điểm qua event log (`Đ-11.B`) · sửa **danh sách câu hỏi và thông tin câu hỏi** (`Đ-37`) · mở bất kỳ vòng nào · mở công bố kết quả (`C-19`) · gán ghế và vị trí *(chỉ khi chưa vòng nào từng chạy)*.

**Mốc đóng băng cấu hình là một GUARD TRÊN CẠNH RA, không phải một trạng thái riêng.**

`NT-C` quy định RuleConfig, mode trả lời và danh sách câu được **snapshot vào trận lúc start**, một chiều. Cụ thể:

> Khi admin mở một vòng từ `LOBBY`: **nếu chưa vòng nào từng chạy** thì cạnh đó **kèm việc đóng băng cấu hình**; ngược lại thì không.

Điều kiện *"chưa vòng nào từng chạy"* **không phải một cờ lưu trữ** — nó suy ra từ event log, đúng `Đ-5.3`. Hệ quả: chỉ **một** chỗ trong toàn hệ thống cần hỏi câu đó.

Hai ràng buộc phái sinh dùng **cùng** điều kiện ấy:

- **Gán ghế / vị trí**: chỉ khi chưa start — `R-VD-02` và `TERM-002` quy định vị trí *"gán thủ công trước trận"*, *"không thay đổi trong trận"*.
- **Bỏ vòng · chạy lại · hoàn nguyên**: không bị cấm khi chưa start, chỉ đơn giản là **rỗng** — chưa có event nào để hoàn nguyên. Không cần guard.

**Nội dung trình diễn giữa hai vòng** (giao lưu, giải lao, video hình hiệu) **không cần** trạng thái riêng: nó là **lớp phủ** do admin bật/tắt (`C-19`), không phải một bước của luồng thi đấu.

⇒ Đóng **vế (1)** của `GRR-077` — *"điều kiện vào và ra của trạng thái nghỉ giữa các vòng"*. **Vế (2) — *"trận đã bắt đầu nhưng không thể hoàn thành kết thúc bằng trạng thái nào"* — VẪN TREO**, theo dõi ở `game-state-machine.md` mục Unresolved (đề xuất `ABANDONED`, chưa duyệt). Hệ quả cấp sản phẩm ở `docs/product-discovery.md` **C-18**, **S-23**.

---

### 11.24 Pipeline vòng ĐỘC LẬP — khôi phục bằng sửa điểm + mở lại vòng `[Đ-39]`

> **Không vòng nào phụ thuộc trạng thái nội bộ của vòng khác.** Toàn bộ trạng thái của một trận nằm ở **hai** thứ: **event log** (ra điểm) và **vòng nào đang mở**. Khôi phục sau bất kỳ sự cố nào = **sửa điểm** + **mở lại vòng cần chạy**. Không có bước thứ ba.

✅ **Chủ dự án chốt 2026-07-27.** Mô hình lấy theo tiền lệ vận hành của Athena: một màn điều khiển, mọi vòng mở được từ đó, không ràng buộc thứ tự.

**Ba hệ quả bắt buộc:**

1. **Mở một vòng ⇒ vòng đó bắt đầu SẠCH.** Mọi trạng thái phạm vi vòng đều đặt lại tại mốc mở: cờ bị loại · lệnh cấm trả lời · lượt chọn đã dùng · khoá chuông · miếng ghép và hàng ngang · hàng đợi đang hoạt động · cửa sổ cướp quyền · gói câu đã chọn. **Không có trạng thái nào sống sót qua mốc mở vòng.**
   > Đây là điều kiện để lời hứa *"khôi phục bằng hai thao tác"* đúng. Nếu một cờ nào đó sống sót thì phải có thao tác thứ ba để dọn nó, và pipeline không còn độc lập.
   > **Ngoại lệ đúng một cái**: **Ngôi sao hy vọng** — phạm vi của nó là **một trận**, không phải một vòng (`R-VD-06`), nên nó **không** đặt lại. Xem `Đ-5.d` về phạm vi chính xác.

2. **Mọi ràng buộc "sau vòng X" đọc thành "tại thời điểm MỞ vòng Y".** Luật gốc mô tả thứ tự bằng cụm *"sau phần thi Tăng tốc"*, *"sau phần thi Về đích"* — hiểu là **mốc mở vòng kế**, không phải một vòng cụ thể phải đã chạy:
   - **Thứ tự lượt Về đích** = bảng điểm **tại thời điểm admin mở vòng Về đích** (tổng quát hoá `Đ-5.1d`).
   - **Điều kiện kích hoạt Câu hỏi phụ** = bảng điểm **tại thời điểm admin mở Câu hỏi phụ**.
   > ⇒ Bỏ vòng Tăng tốc hay bỏ vòng Về đích **không làm mất tiền đề** của vòng sau. Đóng nhánh *"bỏ Về đích thì tie-break mất tiền đề"*.

3. **Cấm mọi phụ thuộc ẩn giữa các vòng.** Một vòng chỉ được đọc: RuleConfig đã snapshot · danh sách câu đã gán · **bảng điểm hiện tại**. Không được đọc trạng thái nội bộ của vòng khác (ai bị loại ở VCNV, ai đã bấm chuông ở Khởi động…).

**Vì sao đây là quyết định về ĐỘ TIN CẬY, không phải về tiện dụng:** hồ sơ triển khai là **portable LAN, không có kỹ sư trực**. Khi sự cố xảy ra giữa buổi thi thật, thứ cứu được buổi thi là một mô hình mà người vận hành **giữ trọn trong đầu** — *"sửa điểm, mở lại vòng"*. Mọi trạng thái ẩn cần dọn thêm đều là một cách để buổi thi hỏng.

> **Tiền lệ phản diện, có bằng chứng thực địa**: `bug.txt` của Athena ghi đúng một lỗi, và nó chính là lỗi vi phạm hệ quả (1) — chạy lại vòng Về đích mà trạng thái cửa sổ cướp quyền của lần chạy trước không được dọn ⇒ **treo toàn bộ nút giành quyền trả lời**. Athena đúng ở mô hình điều khiển nhưng sai ở chỗ này; hệ quả (1) tồn tại để không lặp lại.

⇒ Đóng `Đ-7.3` (*hoàn nguyên trạng thái phi-điểm* — trước là đề xuất chưa duyệt), `GRR-115`, `GRR-116`, `GRR-117`, và tổng quát hoá `Đ-5.1d` / `Đ-5.1e`.

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
| **Đ-21** | Đồng hồ chạy liên tục; trận dừng bằng cách admin ngừng thao tác | §11.6 |
| **Đ-22** | Tín hiệu thí sinh không làm gián đoạn đồng hồ; hoãn HIỂN THỊ chứ không hoãn thời gian | §11.7 |
| **Đ-23** | Hai tín hiệu cùng mốc thời gian: hàng đợi tự quyết định, ngẫu nhiên | §11.8 |
| **Đ-24** | Nút chuông tự khoá ngay khi bấm (frontend, trước khi gửi) | §11.9 |
| **Đ-25** | Tín hiệu đến sau khi đã có người giành quyền: ghi nhận nhưng trơ | §11.10 |
| **Đ-26** | "Hiển thị câu hỏi" và "start timer" là hai thao tác riêng, thứ tự cố định | §11.11 |
| **Đ-27** | Chấm xong là kết thúc câu; xoá hàng đợi đang hoạt động, gỡ khoá chuông | §11.12 |
| **Đ-28** | Biên thời gian là biên ĐÓNG; bản quá hạn vào lịch sử, đánh dấu đỏ | §11.13 |
| **Đ-29** | Nút thao tác một chiều tự tắt sau khi bấm | §11.14 |
| **Đ-30** | Luật cho bao nhiêu câu thì đúng bấy nhiêu; không có câu thứ N+1 | §11.15 |
| **Đ-31** | Kho đề kiểm tại cửa vào từng vòng; thiếu thì không mở vòng đó | §11.16 |
| **Đ-32** | Luôn ghi nhận đáp án cuối cùng; nút gửi không khoá sau khi gửi | §11.17 |
| **Đ-33** | Mốc "MC công bố đáp án" là một thao tác bấm của admin | §11.18 |
| **Đ-34** | Phán quyết chỉ nhị phân khi "Sai" trừ 0 điểm; có phạt thì thêm "Huỷ kết quả" | §11.19 |
| **Đ-35** | Phán quyết của admin là quyết định cuối cùng; nút chấm khoá tới hết giờ ở vòng gõ máy | §11.20 |
| **Đ-36** | Chọn hàng ngang: một đường vào mỗi mode; mode nhập liệu dedup bằng dialog phía thí sinh + khoá tạm | §11.21 |
| **Đ-37** | Danh sách câu đã gán **sửa được tại cửa vào vòng**; **không** gỡ được câu đã hiển thị | §11.22 |
| **Đ-38** | **`LOBBY` = cửa vào vòng** — trạng thái nghỉ duy nhất; enum 4 giá trị; snapshot cấu hình là guard trên cạnh ra. Đóng **vế (1)** của `GRR-077`; vế (2) (*trận bỏ dở*) vẫn treo | §11.23 |
| **Đ-39** | **Pipeline vòng độc lập** — khôi phục = sửa điểm + mở lại vòng; mở vòng ⇒ vòng bắt đầu sạch; *"sau vòng X"* đọc thành *"tại mốc mở vòng Y"* | §11.24 |
