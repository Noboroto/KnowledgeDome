# Luật chơi — CÂU HỎI CÒN CHỜ QUYẾT

> **Mục đích**: tập hợp mọi thứ **chưa được chốt**, xếp theo mức chặn. Đây là file chủ dự án cần trả lời.
>
> **Ba file liên quan**:
> - `game-rules-decisions.md` — quyết định đã chốt
> - `game-rules-review-old.md` — danh sách 136 vấn đề (GRR-001 → GRR-136) kèm trạng thái
> - **`game-rules-open-questions.md`** (file này) — còn chờ quyết
>
> **Quy ước**:
> - **`[CHẶN]`** — không trả lời thì không viết được acceptance criteria
> - **`[ĐỀ XUẤT]`** — đã có phương án kèm lập luận, chỉ cần duyệt hoặc bác
> - **`[MỞ]`** — chưa có phương án, cần chủ dự án phát biểu

---

## Mục lục

- [Tầng 1 — Ba danh sách đang chặn](#tầng-1--ba-danh-sách-đang-chặn)
- [Tầng 2 — Quy ước biên thời gian](#tầng-2--quy-ước-biên-thời-gian)
- [Tầng 3 — Phạm vi v1](#tầng-3--phạm-vi-v1)
- [Tầng 4 — Đề xuất chờ duyệt (nhóm tin cậy TB)](#tầng-4--đề-xuất-chờ-duyệt-nhóm-tin-cậy-tb)
- [Tầng 5 — Con số và chính sách](#tầng-5--con-số-và-chính-sách)
- [Tầng 6 — Dữ kiện chỉ chủ dự án biết](#tầng-6--dữ-kiện-chỉ-chủ-dự-án-biết)
- [Tầng 7 — Câu hỏi nhỏ đi kèm quyết định đã chốt](#tầng-7--câu-hỏi-nhỏ-đi-kèm-quyết-định-đã-chốt)

---

## Tầng 1 — Ba danh sách đang chặn

> Ba mục này là **dữ liệu phải kê**, không suy ra được, và mỗi cái đang chặn một nhóm tính năng.
> Dưới đây là **BẢN NHÁP để sửa**, soạn từ các quyết định đã chốt + đối chiếu bản cũ `Athena-Intelligent-Olympia`.

### 1.0 Đối chiếu: Athena thực sự chặn và cảnh báo những gì

| Loại | Nội dung | Số chỗ |
|---|---|---|
| **CHẶN CỨNG** | `ExtraPlayerPicker`: `CountPlayer < 2` → *"Không đủ người tham gia"* | **1 chỗ duy nhất** |
| **CẢNH BÁO** (OK, không chặn) | *"Không đủ câu hỏi khởi động trong kho"* · *"không đủ câu hỏi tăng tốc trong kho"* · *"Khômg đủ câu hỏi về đích trong kho"* · *"Đã hết chướng ngại vật trong kho"* · *"Đã hết câu hỏi trong kho"* (5 chỗ) · *"Hết phần tăng tốc"* | Toàn bộ về **ĐỀ** và **ranh giới pha** |
| **DIALOG Yes/No** | *"Bạn có muốn mở hàng ngang?"* · *"Bạn có muốn mở toàn bộ CNV không?"* | Khớp C-11 |
| **SỰ CỐ** | *"Máy {X} đã bị ngắt kết nối"* + `SaveLog` | **1 loại duy nhất** |

**Ba kết luận:**
1. **Athena không có cảnh báo LUẬT nào** — mọi cảnh báo đều về **tài nguyên (đề)**. Cảnh báo conflict luật là **năng lực MỚI**, không có tiền lệ để sao chép.
2. **Thiếu đề chỉ cảnh báo, không chặn.** `CLAUDE.md` nghiêm hơn ở **pre-flight**; cho **cạn giữa trận** thì cách Athena khớp mô hình advisory.
3. Athena có **state machine trong từng vòng**, mọi chuyển pha do admin click, kèm `SaveLog` — tiền lệ cho luồng 8 phase của Đ-9.

### 1.1 `[CHẶN]` Danh sách conflict được CẢNH BÁO — *nháp Đ-15.1*

Phân loại theo **đối tượng bị vi phạm** để kiểm được đã đủ hay chưa. Mọi mục là **cảnh báo Yes/No, KHÔNG chặn**.

| # | Nhóm | Tình huống | Nguồn ràng buộc |
|---|---|---|---|
| **L1** | Lượt | Chọn lượt cho thí sinh **đã hoàn thành lượt** ở vòng này (thi 2 lần) | ví dụ của chủ dự án |
| **L2** | Lượt | Chọn lượt **khác recommendation** (bỏ qua người đang tới lượt) | ví dụ của chủ dự án |
| **L3** | Lượt | Chọn lượt cho thí sinh **đang bị loại / bị cấm / đã rời trận** | GRR-022, Đ-6.2 |
| **L4** | Lượt | **Chuyển lượt khi lượt hiện tại chưa hỏi hết** số câu quy định | GRR-119 |
| **L5** | Lượt | VCNV: gán lượt chọn hàng ngang cho người **đã dùng lượt** (trừ nhánh quay vòng hợp lệ) | `F26` + GRR-016 |
| **V1** | Vòng | **Chạy lại một vòng đã kết thúc** | Đ-5.1 |
| **V2** | Vòng | **Bỏ hẳn một vòng** | Đ-5.1, Đ-5.2 |
| **V3** | Vòng | Bắt đầu vòng mới khi **vòng hiện tại chưa kết thúc** | Đ-5 |
| **V4** | Vòng | Chạy **tie-break khi không có ai hoà**, hoặc **chưa phải cuối playlist** | GRR-121 |
| **V5** | Vòng | Chạy lại vòng **sau khi trận đã `FINISHED`** | GRR-120 |
| **N1** | Người tham gia | Câu hỏi phụ: **thêm người KHÔNG hoà điểm** | Đ-10.7b |
| **N2** | Người tham gia | **Số thí sinh đang thi ≠ 4** | GRR-129 |
| **Đ1** | Đề | Snapshot **còn ít hơn** số câu vòng sắp chạy cần | Athena |
| **Đ2** | Đề | **Hết câu giữa vòng** | GRR-084, Athena |
| **E1** | Event/điểm | **Revert event thuộc vòng đã kết thúc** khi các vòng sau đã chạy | GRR-115, GRR-116 |
| **E2** | Event/điểm | **Đổi phán quyết** (Đúng↔Sai) sau khi vòng chứa câu đó đã kết thúc | GRR-071, Đ-5.3 |

**Câu hỏi**: danh sách có **thiếu** loại nào không? Mục nào muốn **chặn cứng** thay vì cảnh báo?

### 1.2 `[CHẶN]` Danh sách "SỰ CỐ PHÁT SINH" — *nháp Đ-15.2*

Cho phép admin dùng queue chuyển quyền. Athena chỉ có **một** loại (S1).

| # | Sự cố | Can thiệp được phép |
|---|---|---|
| **S1** | Thí sinh **mất kết nối** tại thời điểm bấm hoặc ngay sau | Chuyển quyền sang tín hiệu kế tiếp |
| **S2** | **Thiết bị thí sinh lỗi** (chuông kẹt, double-fire) | Bỏ tín hiệu đó, lấy tín hiệu kế tiếp |
| **S3** | Tín hiệu đến trong lúc trận **`PAUSED`** hoặc giữa hai pha | Xác định lại tín hiệu nào hợp lệ |
| **S4** | **Sự cố sân khấu**: MC đọc nhầm/thiếu, đề hiển thị sai, media hỏng | Huỷ kết quả câu, dùng lại queue sau khi khắc phục |
| **S5** | **Bấm chuông trước hiệu lệnh** ở Câu hỏi phụ | **Gỡ lệnh cấm** *(đã chốt: Đ-6.3, Đ-6.4)* |
| **S6** | **Khiếu nại tại chỗ** của BTC/BGK về thứ tự bấm | Xem lại lịch sử, phân xử *(Đ-6.4b: hệ thống giữ timestamp gốc, người phân xử)* |
| **S7** | **Admin bấm nhầm** (mở nhầm, xác nhận nhầm) | Revert theo Đ-5.3 |

> Theo **S-14** (không có trường "người yêu cầu"), **lý do can thiệp phải nhập dạng văn bản tự do** — đó là nơi duy nhất ghi được xuất xứ. Liên quan **S-2** ở `product-discovery`.

### 1.3 `[CHẶN]` Ngưỡng TỐI THIỂU VẬT LÝ (chặn cứng) — *nháp Đ-15.3*

Tiêu chí: **chặn khi vòng/pha đó về mặt vật lý không còn NGHĨA**, không chặn vì lệch luật.

| Vòng / pha | Ngưỡng | Lý do |
|---|---|---|
| Khởi động — lượt riêng | **≥ 1** | Một người vẫn thi được |
| Khởi động — lượt chung | **≥ 1** | Một người vẫn bấm chuông và trả lời được |
| VCNV | **≥ 1** | Một người vẫn chọn hàng và giải CNV được |
| Tăng tốc | **≥ 1** | Một người vẫn xếp hạng bậc 1 |
| Về đích — thi gói câu | **≥ 1** | Một người vẫn thi gói của mình |
| **Về đích — cửa sổ cướp quyền** | **≥ 2** | Phải có **người khác** mới có ai cướp |
| **Câu hỏi phụ** | **≥ 2** | Không có gì để phân định — khớp tiền lệ Athena |
| Toàn trận | **≥ 1** | 0 người ⇒ không vòng nào chạy được |

⇒ **Chỉ HAI ngưỡng chặn cứng thật**. Mọi thứ khác ≥1, tức gần như không bao giờ chặn.

**Ba câu kèm theo:**

| ID | Câu hỏi |
|---|---|
| **Đ-15.a** | Danh sách conflict thiếu loại nào? Mục nào muốn chặn cứng? |
| **Đ-15.b** | Cảnh báo có **tắt được** không (admin đã biết, không muốn bị hỏi lại trong cùng vòng)? Athena không có cơ chế này, nhưng 16 loại conflict sẽ hỏi nhiều hơn Athena rất nhiều |
| **Đ-15.c** | Ngưỡng vật lý bị vi phạm ⇒ chặn cứng, **không cho admin ép** — đây có phải **ngoại lệ duy nhất** của nguyên tắc *"admin luôn ép được"*? |

---

## Tầng 2 — Quy ước biên thời gian

> **Nên quyết CÙNG LÚC.** Quyết lẻ sẽ sinh ba quy ước khác nhau cho cùng một khái niệm — vi phạm DRY.

| Mã | Câu hỏi | Trạng thái |
|---|---|---|
| **GRR-002** | Đáp án hoặc lần bấm chuông đến **đúng mốc hết giờ** (chênh 0 ms) là **kịp hay trượt**? | `[MỞ]` |
| **GRR-031** | *"Cùng một khoảng thời gian"* ở Tăng tốc đo ở **độ phân giải nào** — ms / 10ms / µs? Giá trị mặc định của `tieRule`? | `[MỞ]` |
| **GRR-035** | Submission có timestamp **bằng đúng mốc timeout** được nhận hay bị loại? | `[MỞ]` |

*Không có căn cứ nào trong nguồn — đây là quy ước phải chọn.*

---

## Tầng 3 — Phạm vi v1

| Mã | Câu hỏi | Trạng thái |
|---|---|---|
| **GRR-010** | **Câu hỏi lựa chọn** (Khởi động) có thuộc v1 không? | `[MỞ]` |
| **GRR-037** | **Câu hỏi sắp xếp** (Tăng tốc) có thuộc v1 không? | `[MỞ]` |
| **GRR-050/051** | **Câu hỏi thực hành** (Về đích) có thuộc v1 không? | `[MỞ]` |
| **Đ-0.1a** | v1 có UI cho 1-12 ghế nhưng luật chỉ cho 4: cấu hình **5-12 ghế** thì trận **start được không**? | `[MỞ]` |
| **§0.2 S-1** | v1 có **khoá cứng `rowCount = 4`** không? *(trục "số hàng ngang", khác trục "số thí sinh")* | `[MỞ]` |
| **§0.2 S-2** | v1 có **khoá playlist chuẩn 4 vòng** không? | `[MỞ]` |

### Dữ kiện cho ba loại câu hỏi *(Đ-13)*

| Mã | Loại | Vòng | Mức khó |
|---|---|---|---|
| GRR-010 | Câu hỏi lựa chọn | **Khởi động** | **RẺ NHẤT** — mặc định mode sân khấu, thí sinh **đọc** đáp án ⇒ chỉ cần phương án **hiển thị được**, **không cần mô hình dữ liệu mới**. Chỉ mode nhập liệu mới cần UI |
| GRR-037 | Câu hỏi sắp xếp | **Tăng tốc** | **KHÓ NHẤT** — Tăng tốc **luôn nhập liệu** (không có đường thoát bằng đọc miệng), **và** cần quy tắc đúng/sai chưa tồn tại (**đúng hoàn toàn** hay **đúng từng vị trí**), ảnh hưởng trực tiếp xếp hạng tốc độ |
| GRR-050/051 | Câu hỏi thực hành | **Về đích** | **TRUNG BÌNH** — phần **chấm** không phải vấn đề (*"đạt yêu cầu"* là đánh giá của người, khớp mô hình admin-chấm); cái thiếu là **chuyển pha suy nghĩ → thực hành**: ai bấm, lúc nào |

*Ba loại còn lại của Tăng tốc (nhìn nhanh, suy luận, đoạn băng) chỉ khác ở **media của đề bài**, đáp án vẫn là text ⇒ **không cần gì mới**.*

---

## Tầng 4 — Đề xuất chờ duyệt (nhóm tin cậy TB)

> Đã có đề xuất kèm lập luận. Khác nhóm đã duyệt ở chỗ **có phương án khác cũng hợp lý**.

### 4.1 Luật thi đấu

| Mã | Đề xuất | Căn cứ |
|---|---|---|
| **GRR-007** | Bấm sai ở lượt chung ⇒ **KHÔNG mở lại chuông** | Nguồn: *"3 giây tính từ lúc **thí sinh giành được quyền**"* ⇒ mở lại thì mỗi người kế tiếp có 3 giây mới ⇒ thời gian một câu **không bị chặn trên**; với 12 câu là rủi ro thời lượng thật |
| **GRR-043** | Cướp sai ở Về đích ⇒ **KHÔNG mở lại** cửa sổ | Cùng lập luận; nguồn dùng **số ít**: *"**1** trong 3 thí sinh còn lại"* |
| **GRR-014 / GRR-015** | Miếng ghép mở khi **≥1 người đúng**; **mọi người đúng đều được 10 điểm** | Nguồn không giới hạn người |
| **GRR-022 / GRR-023** | *"Bị loại khỏi phần thi này"* = **mất mọi quyền trong vòng VCNV**. **Điểm đã ghi được GIỮ** | Nguồn không nói trừ điểm |
| **GRR-024** | Câu ô trung tâm: **người chưa bị loại** được trả lời | — |
| **GRR-032** | Sau nhóm đồng hạng k người ở bậc n, người kế nhận bậc **n+k** | Standard competition ranking — khớp ví dụ 40/40/20/10 |
| **GRR-036** | Cùng ghế, cùng timestamp ⇒ dùng **số thứ tự tiếp nhận** của server làm tie-break | — |
| **GRR-038** | Clue-buzz hết dữ kiện chưa ai bấm ⇒ **chạy hết giờ câu** | — |
| **GRR-040** | *"Vị trí đứng thấp nhất"* = **số vị trí NHỎ NHẤT** (vị trí 1) | Vị trí là số 1..N do bốc thăm |
| **GRR-042** | Người cướp có **15/20 giây** (theo mức điểm câu) | Dòng 82 gắn với **câu hỏi**, không gắn với **người thi**: *"Thời gian suy nghĩ **và trả lời** cho câu hỏi 20 điểm là 15 giây…"* |
| **GRR-049** | NSHV: đặt rồi **không trả lời** = sai ⇒ −value · **không rút lại được** · gửi lệnh 2 lần = **idempotent** | — |
| **GRR-052** | Gói 3 câu **không có ràng buộc thành phần** — được chọn 30/30/30 hoặc 20/20/20 | Nguồn không ràng buộc |
| **GRR-053** | Admin chọn hộ ⇒ **chọn mốc còn câu**, không ép mốc đã cạn | — |
| **GRR-055** | Bỏ câu giữa cửa sổ cướp ⇒ **revert các event điểm của câu đó** | Tiền lệ Đ-5.2/5.3 |
| **GRR-063** | Nhiều nhóm hoà ⇒ phân định **từ vị trí cao xuống thấp** | — |
| **GRR-065** | Nhóm hoà ≥3: nguồn chỉ chọn **một** người thắng ⇒ những người còn lại **đồng hạng** | — |
| **GRR-075** | `timeSeconds` per-question **ghi đè** thời gian cố định ở **mọi** vòng | Nguyên tắc "mọi timer là RuleConfig" |
| **GRR-107** | 15 giây sau gợi ý cuối: **mọi thí sinh còn lại cùng trả lời**, ai đúng được **20 điểm**. *"Sai thì bị loại"* **vô nghĩa** ở cửa sổ này vì vòng kết thúc ngay sau | Nguồn dùng số nhiều *"Các thí sinh sẽ có 15 giây"* |
| **GRR-108** | Một người mất quyền ⇒ **câu vẫn tiếp tục** cho người còn lại | *"Mất quyền"* là trạng thái theo người, theo câu |
| **GRR-109** | Câu dở khi hết quỹ giờ: admin **vẫn chấm được sau khi hết giờ**. **Không có giới hạn thời gian riêng cho từng câu** | Nguồn: *"trả lời **liên tục**"* |
| **GRR-113** | `wrongLocksOut` khoá trong phạm vi **CÂU**. **Kèm khuyến nghị**: ba hình phạt đang dùng ba cụm khác nhau (*"bị loại khỏi phần thi"* · *"mất quyền trả lời câu hỏi"* · *"khoá"*) — nên quy về **một thang phạm vi chung: câu / vòng / trận** | Cùng lập luận GRR-007 |
| **GRR-126** | Câu **ô trung tâm** **luôn gõ máy** | Cùng loại với hàng ngang, không phải đáp án CNV |

### 4.2 Trạng thái, kết nối, dữ liệu

| Mã | Đề xuất | Căn cứ |
|---|---|---|
| **GRR-076** | `PAUSED`/`FINISHED`: tín hiệu **ghi vào lịch sử, không có hiệu lực** (không drop) | Đ-7 |
| **GRR-079** | Pause toàn trận ⇒ lưu `remainingMs` cho **mọi** cửa sổ, không chỉ deadline câu | — |
| **GRR-081 / GRR-084 / GRR-090** | **Cảnh báo cho admin**, không chặn cứng; điểm dừng của đệ quy *"câu thay thế cũng hỏng"* = **admin bỏ câu** | Mô hình advisory |
| **GRR-085** | *"Đã được hỏi"* = **đã hiển thị cho thí sinh** | Khớp Đ-5.2f |
| **GRR-089** | `Question.visibility` là **derived, read-only** | Tránh hai nguồn sự thật |
| **GRR-110** | Mất kết nối trong cửa sổ ngắn ⇒ **cửa sổ chạy tiếp, không pause** | Tiền lệ Đ-6.4c |
| **GRR-111** | `usedInContest` **reset** khi import (trạng thái theo contest); `everPublic` **PHẢI đi theo** (thuộc tính của câu) | Nếu không, hàng rào chống rò đề bị vô hiệu bằng một thao tác hợp lệ |
| **GRR-112** | Câu không được chấm ⇒ **không tự đẩy đáp án**, nhưng **admin mở tay được** | C-11 |
| **GRR-115** | Revert làm đổi số hàng đã mở ⇒ **event sau giữ nguyên**; admin dùng `SCORE_ADJUST` nếu muốn | Đ-5.3; máy không tự tính lại |
| **GRR-116** | Revert điểm vòng trước ⇒ **lượt đã chạy giữ nguyên**; recommendation lượt kế tính trên **điểm hiện tại** | Thứ tự vốn đã là quyết định của admin |
| **GRR-117** | Bỏ/chạy lại vòng ⇒ **hoàn nguyên TẤT CẢ trạng thái sinh bởi event của vòng đó**, TRỪ *"câu đã dùng"*. **Một quy tắc, một ngoại lệ** | Các trạng thái phi-điểm cũng sinh từ event |
| **GRR-118** | Câu **đã rút nhưng chưa hiển thị** = **chưa tiêu, trả lại pool** | Khớp GRR-085 |
| **GRR-120** | Chạy lại vòng sau `FINISHED`: **cho phép**, trạng thái về `rounds[i]`, PDF in lại, biên bản giữ cả hai. ⚠️ Đối chiếu Đ-6.4d (*"đã kết thúc thì kết quả đã chốt"*) — tiền lệ đó ở cấp **câu**, mở rộng lên cấp **trận** là suy rộng | `[cần xác nhận]` |
| **GRR-121** | *"Tie-break chỉ ở cuối playlist"* ⇒ thành **recommendation**, không chặn. Điểm đổi sau tie-break ⇒ kết quả **giữ nguyên**, admin quyết chạy lại | Đ-5 đã biến playlist thành gợi ý |
| **GRR-122** | Toàn sân cùng điểm (kể cả 0 hoặc âm) ⇒ tie-break **kích hoạt bình thường**, không có ngưỡng chặn | Đ-10.7 |
| **GRR-127** `[v1.5]` | Practice ⇒ **khoá về mode nhập liệu** | Mode sân khấu cần người nghe |
| **GRR-130** | Revert/bỏ vòng/chạy lại **CÓ phát semantic event**; viewer thấy số đổi đột ngột là **mapping "không animation"** ở client | D22 cho phép; **không mâu thuẫn** |
| **GRR-131** | Rule **chỉ sống ở mode nhập liệu**: ghi nhận đáp án đầu/cuối · timestamp đáp án · highlight · bấm chuông = nộp. Ở mode sân khấu chúng **không bị xoá, chỉ không được kích hoạt** | Cùng một engine |
| **GRR-134** | Đ-4.3 (*"không hoàn tác"*) nói về **nút GIÀNH QUYỀN**, không phải **gửi đáp án**; gửi đáp án vẫn **last-wins**. ⇒ **thu hẹp phát biểu Đ-4.3** | Ngoại lệ tường minh: Đ-10.6 |
| **GRR-135** | *"Visibility of System Status"* là phản hồi cho **người THỰC HIỆN thao tác**; viewer không thực hiện thao tác ⇒ **thu hẹp phạm vi**, không phải cấp ngoại lệ | — |
| **GRR-136** | Append-only áp **trong vòng đời dữ liệu**; retention xoá **cả trận** sau hạn — **không cùng trục**. ⚠️ Hệ quả: sau retention **điểm không tái tính được**, biên bản/PDF thành hồ sơ duy nhất | — |
| **GRR-128** | Inventory phải **phát biểu lại** các nhánh *"công nhận / không công nhận"* và actor *"Server (auto-match)"* thành gợi ý hiển thị | **Việc sửa tài liệu**, không phải câu hỏi luật |

---

## Tầng 5 — Con số và chính sách

### 5.1 Con số không có trong luật `[MỞ]`

| Mã | Câu hỏi |
|---|---|
| **GRR-080** | Admin mất kết nối **bao nhiêu giây** thì trận tự động tạm dừng? |
| **GRR-087** | `reservePerField` = 2 dựa trên tình huống tiêu hao nào? |
| **Đ-5.1g** | *"Còn câu hỏi"* đo ở mức nào — **đủ trọn vòng** hay **≥1 câu**? Theo **từng mức điểm / lĩnh vực** hay tổng số? |
| **GRR-046 / Đ-3** | Admin nhập **giá trị câu lẻ** cho Về đích: **validation chặn** không cho nhập, hay cho nhập rồi định nghĩa **quy tắc làm tròn**? *(Dưới luật 2026 điểm lẻ **không tồn tại**: 20/30 → 10/15)* |

### 5.2 Chính sách `[MỞ]`

| Mã | Câu hỏi |
|---|---|
| **GRR-011** | Trong một vòng Khởi động, có được phép cho thí sinh thi **hai thể thức lượt riêng khác nhau** không? |
| **GRR-028** | **Gợi ý ký tự** hiển thị tại thời điểm nào trong chu trình một hàng ngang? |
| **GRR-034** | Thí sinh có được **rút lại đáp án** đã gửi để nộp trống không? Rule *"submission rỗng thì SKIP"* khiến không rút được — đây là **chủ đích** hay **hệ quả phụ**? |
| **GRR-054** | Thí sinh có được **đổi gói** sau khi đã xác nhận không? Hạn chót là mốc nào? |
| **GRR-057 / Đ-10.7a** | Nếu admin **KHÔNG** chọn phân định một nhóm hoà, thứ hạng cuối của nhóm đó ghi thế nào — **đồng hạng**, hay tiêu chí khác? |
| **GRR-077** | Một trận đã bắt đầu nhưng **không thể hoàn thành** được kết thúc bằng **trạng thái nào**? (chưa có trạng thái cho trận huỷ) |
| **GRR-088** | Câu bị đánh dấu `everPublic` **do nhầm lẫn** có được khôi phục không? Ai có quyền? |
| **GRR-091** | **Câu hỏi phụ lấy từ pool nào**, và có phải gán trước khi start như các vòng khác không? |
| **GRR-104** | **Miền giá trị hợp lệ** của các tham số luật khi admin tự cấu hình là gì? Đổi cấu hình giữa contest có áp cho trận đã diễn ra không? |

---

## Tầng 6 — Dữ kiện chỉ chủ dự án biết

| Mã | Câu hỏi |
|---|---|
| **Đ-4.5a** | *"Vị trí"* do **ai đặt** và đặt **lúc nào**? Có đổi được giữa contest không? |
| **Đ-9.e** | **BGK (ban giám khảo)** có trùng A-9 (BTC/trọng tài) hay là stakeholder riêng? *(Suy được: BGK **không cần role/permission** vì không thao tác hệ thống)* |
| **S-15** | **Biên bản viết tay** là **yêu cầu quy trình bắt buộc** hay **giả định vận hành**? PDF kết quả có cần in kèm phần trống ghi tay / chỗ ký? Có cần mẫu biên bản chuẩn không? |
| **GRR-082** | Nếu **đồng hồ server thay đổi** giữa trận khiến timestamp không đơn điệu tăng, kết quả xếp hạng đã ghi xử lý ra sao? |
| **GRR-103** | Khi **nguồn luật gốc (wiki) thay đổi** sau ngày snapshot, quy trình cập nhật preset và hiệu lực với contest đang chạy là gì? |
| **AS-4** | v1 giả định **1 người** hay **≥2 người** vận hành? *(`product-gaps` khuyến nghị crew ≥2; admin nay gánh: chọn vòng, chọn lượt, hiện câu, start timer, chấm, mở/đóng hiển thị, phán quyết mọi dialog)* |

---

## Tầng 7 — Câu hỏi nhỏ đi kèm quyết định đã chốt

### 7.1 Từ Đ-1 (highlight)

| ID | Câu hỏi |
|---|---|
| **Đ-1.a** | Khi câu có **nhiều `acceptedAnswers`**, highlight so sánh với đáp án nào? |
| **Đ-1.b** | So sánh trên chuỗi **nguyên văn** hay **đã normalize**? Tuỳ chọn "bỏ dấu tiếng Việt" còn tồn tại không (chỉ đổi tô màu, không đổi phán quyết)? |
| **Đ-1.c** | `wordCount` / *"cùng tổng số chữ cái"* còn hiển thị như gợi ý cho admin không? |

### 7.2 Từ Đ-4 (mode)

| ID | Câu hỏi |
|---|---|
| **Đ-4.c** | Mode sân khấu có **ghi lại nội dung đáp án** (admin gõ hộ) cho log/PDF không, hay chỉ lưu Đúng/Sai? |
| **Đ-4.d** | Rule *"đáp án đầu/cuối"* và biên timestamp chỉ áp cho **mode nhập liệu và VCNV/Tăng tốc**, đúng không? |
| **Đ-4.e2** | Mode nhập liệu cho **cả** thí sinh click **và** admin click — ai quyết dùng đường nào? |
| **Đ-4.e3** | Mode sân khấu: luật *"mỗi thí sinh tối đa 1 lượt"* được **hệ thống ràng buộc** hay chỉ là quy ước? *(Đã hạ mức chặn nhờ Đ-7.1)* |
| **Q-C1b** | Sửa mode **sau khi contest đã có match `FINISHED`** thì sao? |
| **Q-C1c** | Contest chứa cả match official lẫn rehearsal ⇒ **dùng chung mode**, đúng không? *(Đối chiếu C-1: `revealAnswerAfterJudge` là per-match)* |
| **Q-C2** | Mode có nằm trong preset `O26_DEFAULT@1` không? Nút *"Áp dụng luật 2026"* đặt mode nào? |
| **Q-C3** | Mode có được ghi vào **contest bundle** export/import (D23) không? |

### 7.3 Từ Đ-5 (điều khiển trận)

| ID | Câu hỏi |
|---|---|
| **Đ-5.d** | Admin **override** rồi thì luật phái sinh tính sao — vd thi Về đích **2 lần** thì có **2 gói câu**? NSHV có dùng lại được? |
| **Đ-5.e** | Override có ghi **AuditLog riêng kèm lý do** không? |
| **Đ-5.f** | Recommendation hiển thị cho **ai**? *(Đề xuất: admin + MC, KHÔNG viewer — lộ thứ tự sắp tới)* |
| **Đ-5.g** | **Pre-flight** chạy **một lần cho cả playlist** hay **theo từng vòng**? |
| **Đ-5.1d** | ⚠️ **Bỏ vòng Tăng tốc phá mốc của Về đích** — luật xếp thứ tự Về đích theo *"điểm số cao nhất **sau phần thi Tăng tốc**"*. Nếu Tăng tốc bị bỏ, mốc này không tồn tại ⇒ recommendation dựa trên gì? |
| **Đ-5.1e** | Bỏ **Khởi động** thì điểm vào Tăng tốc/Về đích đều bằng 0 — có cảnh báo gì không? |
| **Đ-5.1f** | Bỏ / chạy lại vòng có thuộc **danh sách conflict được cảnh báo** không? *(Đề xuất: có — V1, V2 ở §1.1)* |
| **Đ-5.1h** | Kiểm tra điều kiện *"còn câu hỏi"* **lúc nào** — khi bấm chạy lại (chặn trước), hay để cạn giữa chừng? |
| **Đ-5.1i** | Nếu **không đủ câu**, hệ thống **chặn cứng** hay chỉ **cảnh báo cho admin ép**? *(Đây là ngoại lệ đầu tiên của "admin luôn ép được")* |
| **GRR-133** | ⚠️ Điều kiện *"còn câu hỏi"* đo trên **danh sách đã gán (snapshot)** hay trên **toàn kho đề contest**? Nếu là kho contest thì việc **bổ sung câu vào snapshot giữa trận** có được phép không — trái nguyên tắc *"hệ thống KHÔNG tự lấy đề"*? *(Đề xuất: đo trên **snapshot**, admin gán thêm câu là thao tác có chủ đích)* |

### 7.4 Từ Đ-5.3 (event log)

| ID | Câu hỏi |
|---|---|
| **Đ-5.3.1a** | Event điểm của câu Tăng tốc phát **tại thời điểm nào** — tự động khi chấm xong người cuối, hay admin bấm *"chốt câu"*? Trước mốc đó bảng điểm hiển thị gì? |
| **Đ-5.3.1b** | Quy tắc *"một câu = một event"* có áp cho **các vòng khác** không (Khởi động lượt chung, VCNV hàng ngang)? |
| **GRR-106** | **Điểm được chốt tại mốc nào** để xét điều kiện hoà? Nếu admin sửa điểm **sau khi đã vào `TIE_BREAK`** thì trạng thái xử lý ra sao? |

### 7.5 Từ Đ-6 (mốc thời gian, lệnh cấm)

| ID | Câu hỏi |
|---|---|
| **Đ-6.3b** | Trạng thái cấm khác — *"bị loại khỏi phần thi này"* ở VCNV — có **gỡ được** không? |
| **Đ-6.3c** | Gỡ cấm có sinh **event revert** không, và có cần **dialog xác nhận** không? *(Đề xuất: có event; **không** cần dialog vì đảo ngược được và đang bị ép thời gian)* |
| **Đ-6.2c** | ⚠️ **Về đích chưa có quy tắc**: bấm chuông **trước khi cửa sổ cướp 5 giây mở** thì sao? *(Đề xuất: **vô hiệu**, phải bấm lại; Về đích **không có** hình phạt bấm sớm như Câu hỏi phụ)* |

### 7.6 Từ Đ-7 (hàng đợi)

| ID | Câu hỏi |
|---|---|
| **Đ-7.b2** | ⚠️ **Vòng đời tín hiệu**: *"reset theo vòng"* một mình **KHÔNG đủ**. Ba phạm vi cùng tồn tại — VÒNG (*"Mở chướng ngại vật"*), LƯỢT CHỌN (chọn hàng ngang), CÂU (mọi chuông còn lại). **Đề xuất**: *tín hiệu gắn với ĐÍCH của nó và vô hiệu khi đích đóng* — khi đó reset-theo-vòng là **hệ quả tự nhiên**, không phải quy tắc riêng, và tránh bảng ngoại lệ theo từng vòng |
| **Đ-7.c** | **Mốc tính điểm** dùng thời điểm **thí sinh bấm** hay **admin xác nhận**? Ở VCNV thang điểm CNV phụ thuộc số hàng đã mở ⇒ hai mốc cho hai mức điểm |
| **Đ-7.d** | Thí sinh bị reject **bấm lại ngay** được không, **bao nhiêu lần**? Có rate-limit theo LUẬT không? |
| **Đ-7.e** | Thí sinh có **thấy trạng thái queue** của mình không? *(`CLAUDE.md` buộc "visibility of system status", nhưng hiện vị trí trong hàng đợi là **lộ thông tin** cho người khác)* |
| **Đ-7.g** | Trong lúc admin duyệt queue, **timer câu có chạy tiếp** không? |
| **Đ-7.2b** | Khi admin chuyển quyền vì sự cố, **điểm và hình phạt đã ghi** của người đầu tiên xử lý ra sao? |
| **Đ-7.2c** | **Cửa sổ đã đóng** rồi mới phát hiện sự cố: tín hiệu kế tiếp trong queue **còn dùng được** không? |
| **GRR-125** | **Tiêu chí để một nút được xếp là "chuông"** (cấm hotkey) là gì? Nút **chọn hàng ngang** có thuộc nhóm đó không? *(Hiện `CLAUDE.md` cho hotkey `1-8` chọn hàng)* |

### 7.7 Từ Đ-9, Đ-10 (Câu hỏi phụ)

| ID | Câu hỏi |
|---|---|
| **Đ-9.c** | Đã chốt cơ chế (bấm chuông = nộp). Còn: sau khi bấm còn **sửa được** không? *(Đề xuất: **khoá**)* · gõ mà **không bấm chuông** ⇒ **không nộp, không tính** · bấm khi **ô rỗng** ⇒ nộp rỗng, admin chấm Sai, **không có hình phạt trừ điểm** |
| **Đ-10.7b** | Admin chọn **khác recommendation** có thuộc danh sách conflict cảnh báo không? *(Đề xuất: chọn trong tập hoà = bình thường; **thêm người KHÔNG hoà = cảnh báo**)* |

---

## Phụ lục — Mục thuộc `product-discovery.md`

Không thuộc luật chơi, ghi ở `docs/product-discovery.md` §6:

| Mã | Nội dung |
|---|---|
| **C-1** | `revealAnswerAfterJudge` per-contest hay per-match *(đề xuất: **per-match**)* |
| **C-4** | D26/D27 (roster bundle, result bundle) đã chốt hay còn treo |
| **C-9** | Ranh giới Contest vs Match trong UI |
| **C-2, C-3, C-6, C-10** | Tài liệu cũ chưa sync |
| **S-1** | **Ai** có quyền bỏ vòng / chạy lại vòng / override cảnh báo — chưa gắn permission CASL |
| **S-2** | Revert có **bắt nhập lý do** không *(quan trọng hơn hẳn sau S-14)* |
| **S-3 → S-12** | Dialog cảnh báo, recommendation hiển thị cho ai, sound-cue slot mới, trình bày biên bản/PDF, Esc ở mode sân khấu, lịch sử submission, highlight nhiều đáp án, retention vs biên bản, viewer thấy trận quay ngược, pre-flight theo vòng |
| **S-15** | **Biên bản viết tay** là phần chính thức của hồ sơ trận |
| **Bản quyền** | Format Olympia + license repo *(D7 đã xoá, câu hỏi pháp lý chưa được trả lời)* |
