# Business-Rule Review — Khiếm khuyết đặc tả luật chơi

> **Ngày review**: lượt 1 — 2026-07-24 · lượt 2 — 2026-07-24 · suy luận + duyệt — 2026-07-25
> **Loại**: business-rule review (KHÔNG phải technical design review)
>
> **Nguyên tắc thi hành**: chỉ NÊU vấn đề, KHÔNG tự phân xử mâu thuẫn, KHÔNG phát minh luật bổ sung, KHÔNG đề xuất schema/kiến trúc/code. Mọi vấn đề đều dẫn file + section + trích nguyên văn ngắn.

## Bộ ba tài liệu — đọc file nào khi nào

| File | Nội dung | Dùng khi |
|---|---|---|
| **[`game-rules-decisions.md`](./game-rules-decisions.md)** | **Quyết định ĐÃ CHỐT**, sắp theo chủ đề và theo vòng | Viết PRD / spec / code |
| **[`game-rules-open-questions.md`](./game-rules-open-questions.md)** | **Còn chờ quyết**, xếp theo mức chặn | Chủ dự án cần trả lời |
| **`game-rules-review-old.md`** (file này) | **Danh sách 136 vấn đề** GRR-001 → GRR-136 kèm trạng thái, và phụ lục thuật ngữ | Tra một mã GR cụ thể, xem lập luận gốc |

> Các khối quyết định **Đ-1 → Đ-15** trước đây nằm ở `game-rules-decisions.md` của file này đã được **chuyển sang hai file trên** và sắp xếp lại. Nội dung không đổi; chỉ đổi nơi lưu và thứ tự.

---

## 0. Phạm vi phiên bản (tóm tắt)

> Chi tiết: `game-rules-decisions.md` §2.

**v1 = ĐÚNG 4 thí sinh** (về LUẬT). Luật cho 1-12 thí sinh chuyển sang **v1.5**. **v1 vẫn hỗ trợ lưu trữ dữ liệu và UI cho 1-12 người.**

### 0.1 Nhãn phiên bản trong danh sách vấn đề

| Nhãn | Nghĩa | Mã |
|---|---|---|
| `[v1.5]` | Hoãn — chỉ do số ghế ≠ 4 | GRR-017 · GRR-029 · GRR-030 · GRR-045 · GRR-083 · GRR-105 · GRR-127 |
| `[v2]` | Hoãn — thi đội | GRR-093 → GRR-098 |

> ⚠️ Nhãn `[v2]` gắn ở **6 mục số ghế** trong §2 đọc là **`[v1.5]`** (sửa 24/07). Nhãn `[v2]` chỉ còn đúng cho nhóm thi đội.

**KHÔNG hoãn được** — xảy ra ngay trong v1: **GRR-129** (cấu hình 4 ghế nhưng runtime tụt còn 3). Đã chốt: admin quyết.

### 0.2 Trục biến thiên KHÁC — chưa được quyết định "4 thí sinh" cover

| Mã | Câu hỏi |
|---|---|
| **GRR-020** | v1 có khoá cứng `rowCount = 4` không? *(trục **số hàng ngang**)* |
| **GRR-078** | v1 có khoá playlist chuẩn 4 vòng không? |
| **GRR-104** | Miền giá trị hợp lệ khi admin tự sửa preset |

### 0.3 Ranh giới tài liệu

Tài liệu này chỉ chứa **luật game**. Mọi vấn đề về **điều khiển hệ thống** (quyền admin, thao tác vận hành, UI, audit, phân quyền) thuộc **`docs/product-discovery.md`** §6 (C-11 → C-16, S-1 → S-15).

### 0.4 Ký hiệu trạng thái trong danh sách

| Ký hiệu | Nghĩa |
|---|---|
| `[ĐÓNG — …]` | Đã được chốt hoặc đã được giải; xem mã quyết định kèm theo |
| `[ĐÓNG PHẦN — …]` | Đã giải một phần, phần còn lại nêu ngay trong mục |
| `[THU HẸP — …]` | Vấn đề vẫn còn nhưng phạm vi đã hẹp lại đáng kể |
| `[NẶNG HƠN — …]` | Một quyết định sau đó làm vấn đề này khó hơn |
| `[ĐỔI TÍNH CHẤT — …]` | Bản chất vấn đề đã đổi |
| `[v1.5]` `[v2]` | Hoãn sang phiên bản sau |

---

## 1. Tổng quan

### 1.1 Phạm vi

Review đặc tả **luật chơi** (business rules): điều kiện, nhánh kết quả, biên giá trị, trạng thái, thứ tự, đồng thời, idempotency, thuật ngữ. Không đánh giá kiến trúc, hiệu năng, bảo mật kỹ thuật, UI.

### 1.2 File đã đọc (đọc đầy đủ, không skim)

| File | Kích thước | Ghi chú |
|---|---|---|
| `docs/game-rules-inventory.md` | 74 KB / 740 dòng | Đọc toàn bộ PHẦN 0 đến PHẦN 9 |
| `docs/source/fandom-olympia-26-luat-choi.md` | 11.7 KB / 106 dòng | Đọc toàn bộ — snapshot nguồn luật `F26`, file duy nhất trong `docs/source/` |

Tài liệu trong `plans/**` KHÔNG được dùng làm căn cứ requirement (theo `CLAUDE.md`: plans là BẢN NHÁP, không được trích như requirement đã chốt); chỉ trích lại qua phần đã được `game-rules-inventory.md` dẫn.

### 1.3 Bảng đếm theo nhãn

| Nhãn | Số lượng |
|---|---|
| MISSING | 59 |
| AMBIGUOUS | 13 |
| CONFLICT | 17 |
| UNREACHABLE | 2 |
| ORDER_DEPENDENT | 4 |
| BOUNDARY_UNDEFINED | 11 |
| **Tổng** | **106** |

### 1.4 Bảng đếm theo mức độ

| Mức độ | Số lượng |
|---|---|
| Cao (khả năng gây bế tắc khi vận hành trận thật) | 40 |
| Trung bình | 51 |
| Thấp | 15 |

### 1.5 Bảng đếm theo module

| Module | Số vấn đề |
|---|---|
| A. Khởi động | 12 (GRR-001 đến GRR-012) |
| B. VCNV | 16 (GRR-013 đến GRR-028) |
| C. Tăng tốc | 11 (GRR-029 đến GRR-039) |
| D. Về đích và NSHV | 17 (GRR-040 đến GRR-056) |
| E. Tie-break / Câu hỏi phụ | 9 (GRR-057 đến GRR-065) |
| F. Chấm điểm và so khớp đáp án | 9 (GRR-066 đến GRR-074) |
| G. Timer / trạng thái / kết nối | 9 (GRR-075 đến GRR-083) |
| H. Kho đề / rút đề / no-repeat | 9 (GRR-084 đến GRR-092) |
| I. Thi đội (v2) | 6 (GRR-093 đến GRR-098) |
| J. Meta tài liệu và nguồn | 8 (GRR-099 đến GRR-106) |

### 1.6 Lượt 2 và tổng hợp

Lượt 2 (rà soát sau khi có các quyết định) bổ sung **30 mục** GRR-107 → GRR-136 — xem §2B. Tổng toàn tài liệu: **136 mục**.

| Nhóm lượt 2 | Mô tả | ID | SL |
|---|---|---|---|
| **A** | Bị bỏ sót ở lượt 1 | GRR-107 → GRR-113 | 7 |
| **B** | **Mới sinh ra từ chính các quyết định** | GRR-114 → GRR-131 | 18 |
| **C** | Mâu thuẫn quyết định ↔ quyết định, hoặc ↔ `CLAUDE.md` | GRR-132 → GRR-136 | 5 |

### 1.7 Trạng thái xử lý

| Trạng thái | Nơi tra |
|---|---|
| **Đã chốt** | Nhãn `[ĐÓNG]` ngay trên tiêu đề mục; nội dung quyết định ở [`game-rules-decisions.md`](./game-rules-decisions.md) |
| **Có đề xuất, chờ duyệt** | [`game-rules-open-questions.md`](./game-rules-open-questions.md) §Tầng 4 |
| **Chưa có phương án** | [`game-rules-open-questions.md`](./game-rules-open-questions.md) §Tầng 1, 2, 3, 5, 6 |
| **Hoãn v1.5 / v2** | §0.1 của tài liệu này |

> **31 mục** đã được chủ dự án **duyệt lô** ngày 2026-07-25 (nhóm độ tin cậy Cao) — danh sách đầy đủ ở `game-rules-decisions.md` §10 và §11.

---

## 2. Danh sách vấn đề

Ký hiệu nguồn giữ nguyên như `game-rules-inventory.md`: `F26` = Fandom snapshot, `W26` = Wikipedia (đã bị loại), `R26`/`SPEC`/`DEF`/`PRD`/`US`/`P06`/`DEMO`/`ATH` = tài liệu trong `plans/`.

---

## A. KHỞI ĐỘNG

### GRR-001 · MISSING · Cao · `[ĐÓNG — Đ-6: admin start timer]`
- **Vị trí**: `docs/source/fandom-olympia-26-luat-choi.md` §Khởi động; `docs/game-rules-inventory.md` §R-KD-01, §U-1
- **Trích**: *"Thời gian suy nghĩ cho mỗi câu hỏi là 3 giây tính từ lúc người dẫn chương trình đọc xong câu hỏi."* — inventory: *"ai/cái gì kích hoạt timer 3s — nguồn nói từ lúc MC đọc xong, hệ thống không biết thời điểm đó. Không nguồn nào định nghĩa trigger."*
- **Vấn đề**: mốc bắt đầu timer là sự kiện ngoài hệ thống. Không rule nào định nghĩa sự kiện tương ứng trong hệ thống, cũng không định nghĩa điều gì xảy ra nếu mốc đó không bao giờ được phát. Trạng thái "câu đã hiện nhưng timer chưa chạy" không có đường thoát được đặc tả.
- **Câu hỏi**: Sự kiện nào trong hệ thống được coi là "MC đọc xong câu hỏi", ai phát nó, và nếu không được phát thì hệ thống xử lý ra sao?

### GRR-002 · BOUNDARY_UNDEFINED · Trung bình
- **Vị trí**: `fandom-olympia-26-luat-choi.md` §Khởi động; `game-rules-inventory.md` §R-KD-01, §R-KD-03
- **Trích**: *"Thời gian suy nghĩ cho mỗi câu hỏi là 3 giây"* · *"bấm chuông mà không có câu trả lời sau 3 giây bị trừ 5 điểm"*
- **Vấn đề**: không nguồn nào nói đáp án hoặc lần bấm chuông đến **đúng** mốc 3.000s là hợp lệ hay quá hạn (nhỏ hơn, hay nhỏ hơn bằng). Với độ phân giải ms mà `R26` §7 chốt cho xếp hạng, biên này quyết định thắng thua.
- **Câu hỏi**: Đáp án hoặc lần bấm chuông đến đúng mốc hết giờ (chênh 0 ms) được tính là kịp hay quá hạn?

### GRR-003 · UNREACHABLE · Trung bình
- **Vị trí**: `fandom-olympia-26-luat-choi.md` §Khởi động đoạn cuối; `game-rules-inventory.md` §R-KD-06
- **Trích**: *"đáp án cuối cùng sẽ được ghi nhận. Nếu không thay đổi, chương trình sẽ ghi nhận đáp án đầu tiên."*
- **Vấn đề**: khi thí sinh không thay đổi đáp án, đáp án đầu tiên và đáp án cuối cùng là cùng một bản ghi, nên nhánh thứ hai không bao giờ tạo ra kết quả khác nhánh thứ nhất. Nhánh này hoặc thừa, hoặc đang ám chỉ một phân biệt mà nguồn không phát biểu.
- **Câu hỏi**: Nhánh "nếu không thay đổi thì ghi nhận đáp án đầu tiên" nhằm phân biệt tình huống nào so với "ghi nhận đáp án cuối cùng"?

### GRR-004 · CONFLICT · Cao
- **Vị trí**: `game-rules-inventory.md` §R-KD-06 vs §R-TT-03 vs `CLAUDE.md` §UX
- **Trích**: R-KD-06 mốc cắt là *"trước thời điểm người dẫn chương trình công bố đáp án"*; R-TT-03: *"tính BẢN CUỐI CÙNG; ranking theo server-received timestamp của bản cuối"* với mốc server-timeout. Inventory tự ghi: *"Hai mốc khác nhau; không nguồn nào hợp nhất."*
- **Vấn đề**: hai rule cùng mô tả việc ghi nhận đáp án nhưng dùng hai mốc cắt và hai quy tắc chọn bản khác nhau. Cùng một hành vi (gửi lại đáp án) cho kết quả khác nhau tuỳ vòng, không có rule cấp trên phân xử.
- **Câu hỏi**: Mốc cắt ghi nhận đáp án ở Khởi động là "MC công bố đáp án" hay "server-timeout", và quy tắc chọn bản (đầu hay cuối) có được thống nhất giữa các vòng không?

### GRR-005 · AMBIGUOUS · Cao
- **Vị trí**: `fandom-olympia-26-luat-choi.md` §Khởi động; `game-rules-inventory.md` §R-KD-04
- **Trích**: *"Thí sinh có thể bấm chuông trong khi người dẫn chương trình đang đọc câu hỏi. Sau 3 giây tính từ thời điểm người dẫn chương trình đọc xong câu hỏi, nếu không có thí sinh nào giành quyền trả lời, câu hỏi đó sẽ bị bỏ qua."*
- **Vấn đề**: inventory ghi *"chồng lấn giữa được bấm khi MC đang đọc và cửa sổ 3s sau khi đọc xong — có phải là một cửa sổ liên tục hay hai giai đoạn. Cả F26 lẫn R26 đều không nói."* Thêm nữa: bấm chuông giữa lúc đang đọc thì timer 3 giây trả lời chạy ngay (*"3 giây tính từ lúc thí sinh giành được quyền trả lời"*) trong khi câu hỏi chưa đọc xong; nguồn không nói MC có dừng đọc không.
- **Câu hỏi**: Cửa sổ bấm chuông là một khoảng liên tục hay hai giai đoạn tách biệt, và khi bấm giữa lúc đang đọc thì đồng hồ 3 giây trả lời bắt đầu từ lúc nào?

### GRR-006 · MISSING · Trung bình
- **Vị trí**: `fandom-olympia-26-luat-choi.md` §Khởi động (lượt riêng); `game-rules-inventory.md` §R-KD-01
- **Trích**: *"Trả lời đúng được 10 điểm, trả lời sai không bị trừ điểm."*
- **Vấn đề**: chỉ có hai nhánh đúng và sai. Trường hợp thí sinh **không trả lời gì** trong 3 giây không được xếp vào nhánh nào; ở lượt chung nguồn có nêu riêng (*"bấm chuông mà không có câu trả lời sau 3 giây"*) nhưng lượt riêng thì không.
- **Câu hỏi**: Ở lượt riêng, không trả lời gì trong 3 giây được xử lý như trả lời sai hay là trạng thái riêng, và ai xác nhận nó?

### GRR-007 · MISSING · Cao · `[ĐỔI TÍNH CHẤT — Đ-7.2: nay quyết định cả cơ chế queue, không chỉ điểm]`
- **Vị trí**: `game-rules-inventory.md` §R-KD-03
- **Trích**: *"R26 §1 và §7 thêm: câu đó KHÔNG mở lại chuông cho người khác"* · *"không mở lại chuông cho người khác chỉ có trong R26, KHÔNG có trong F26. F26 im lặng về việc này."*
- **Vấn đề**: nguồn luật được chọn im lặng về nhánh sau khi một thí sinh bấm chuông và bị chấm sai. Hai xử lý khả dĩ (bỏ câu, hoặc mở lại chuông trong thời gian còn lại) cho kết quả điểm khác nhau và đều có tiền lệ trong tài liệu repo.
- **Câu hỏi**: Sau khi một thí sinh bấm chuông và bị chấm sai ở lượt chung, câu hỏi bị bỏ qua hay mở lại chuông cho các thí sinh còn lại?

### GRR-008 · MISSING · Cao
- **Vị trí**: `fandom-olympia-26-luat-choi.md` §Khởi động; `game-rules-inventory.md` §R-GEN-03
- **Trích**: *"Trả lời sai hoặc bấm chuông mà không có câu trả lời sau 3 giây bị trừ 5 điểm."* vs R-GEN-03: *"Điểm chỉ chốt khi ADMIN bấm Đúng/Sai"*, *"Kết quả khi admin chưa bấm (official): câu chưa được chấm; không sinh điểm"*.
- **Vấn đề**: trường hợp bấm chuông rồi im lặng không có đáp án nào để admin phán quyết, nhưng luật vẫn quy định trừ 5 điểm. Không rule nào nói hình phạt này tự phát sinh theo timer hay phải chờ admin, và admin thao tác gì.
- **Câu hỏi**: Khi thí sinh bấm chuông nhưng không gửi đáp án nào cho tới hết 3 giây, hình phạt trừ 5 phát sinh tự động hay chỉ khi admin thao tác, và admin thao tác gì?

### GRR-009 · BOUNDARY_UNDEFINED · Cao · `[ĐÓNG — Đ-2]`
- **Vị trí**: `fandom-olympia-26-luat-choi.md` §Khởi động, §Về đích; `game-rules-inventory.md` §R-KD-03, §R-VD-05 (U-28)
- **Trích**: *"bị trừ 5 điểm"* · *"bị trừ một nửa số điểm của câu hỏi"* · inventory: *"người bị cướp có xuống âm được không (U-28)"*
- **Vấn đề**: không nguồn nào định nghĩa sàn điểm. Điểm âm có hợp lệ không, tính vào xếp hạng Về đích và điều kiện hoà thế nào — đều không nêu. Biên 0 và âm áp cho toàn hệ thống điểm.
- **Câu hỏi**: Điểm của thí sinh có được phép âm không; nếu có sàn thì sàn là bao nhiêu và phần bị cắt có ảnh hưởng tới số điểm chuyển sang người cướp không?

### GRR-010 · MISSING · Trung bình
- **Vị trí**: `fandom-olympia-26-luat-choi.md` §Khởi động; `game-rules-inventory.md` §R-KD-05, §U-34
- **Trích**: *"Câu hỏi lựa chọn: đúng sai, chọn các đáp án cho sẵn."* · inventory: *"hệ thống hiện chỉ có acceptedAnswers dạng text, không có mô hình lựa chọn."*
- **Vấn đề**: luật gốc công nhận một loại câu có cơ chế trả lời khác hẳn, nhưng không rule nào nói cách trả lời, cách chấm, xử lý khi chọn nhiều đáp án hay không chọn gì.
- **Câu hỏi**: Câu hỏi lựa chọn ở Khởi động có nằm trong phạm vi v1 không, và luật trả lời cùng cách chấm của loại câu này là gì?

### GRR-011 · CONFLICT · Trung bình
- **Vị trí**: `game-rules-inventory.md` §R-KD-02
- **Trích**: *"SPEC §4 cho trộn kind tuỳ ý mà không ràng buộc công bằng — trong cùng một vòng, thí sinh A có thể thi kind khác thí sinh B."*
- **Vấn đề**: hai thể thức lượt riêng (6 câu 3 giây, và quỹ thời gian) được phép cùng tồn tại trong một vòng, cho hai cách sinh điểm khác nhau. Không rule nào cấm, cũng không rule nào nói kết quả được so sánh thế nào.
- **Câu hỏi**: Trong một vòng Khởi động, có được phép cho các thí sinh thi bằng hai thể thức lượt riêng khác nhau không; nếu có thì điểm của họ có được coi là so sánh được không?

### GRR-012 · MISSING · Thấp
- **Vị trí**: `game-rules-inventory.md` §R-KD-02
- **Trích**: *"maxQuestions optional — hết giờ mà còn câu, hoặc hết câu mà còn giờ, xử lý ra sao."*
- **Vấn đề**: hai nhánh biên của thể thức quỹ thời gian không có kết quả được định nghĩa.
- **Câu hỏi**: Ở thể thức quỹ thời gian, hết giờ khi còn câu chưa hỏi và hết câu khi còn giờ thì lượt kết thúc thế nào?

---

## B. VƯỢT CHƯỚNG NGẠI VẬT (VCNV)

### GRR-013 · UNREACHABLE · Cao · `[ĐÓNG — Đ-8.1, duyệt Đ-12]`
- **Vị trí**: `fandom-olympia-26-luat-choi.md` §Vượt chướng ngại vật; `game-rules-inventory.md` §R-VCNV-05 (K-7)
- **Trích**: *"Sau khi cả 4 từ hàng ngang đã được mở ra mà không có thí sinh nào trả lời Chướng ngại vật, câu hỏi trong gợi ý cuối cùng sẽ được đưa ra ở ô trung tâm"* — cùng nguồn: *"Nếu không trả lời được từ hàng ngang, miếng ghép tương ứng… sẽ không được mở ra."*
- **Vấn đề**: nếu bất kỳ hàng ngang nào không ai trả lời đúng thì miếng ghép đó không mở, nên điều kiện "cả 4 từ hàng ngang đã được mở ra" **không bao giờ thoả**; nhánh ô trung tâm trở thành nhánh chết và vòng không có đường kết thúc được đặc tả cho tình huống này. Inventory ghi: *"Mâu thuẫn thực sự, ảnh hưởng luồng vòng."*
- **Câu hỏi**: Điều kiện mở ô trung tâm là cả 4 miếng ghép đã mở, hay đã hỏi hết 4 hàng ngang bất kể mở được bao nhiêu, và nếu chỉ mở được 2 miếng thì vòng kết thúc thế nào?

### GRR-014 · MISSING · Cao
- **Vị trí**: `fandom-olympia-26-luat-choi.md` §Vượt chướng ngại vật; `game-rules-inventory.md` §U-37, §8A K-5
- **Trích**: *"Sau khi trả lời đúng từ hàng ngang, miếng ghép tương ứng… sẽ được mở ra."* · inventory: *"F26 (nguồn được chọn) chỉ nói trả lời đúng thì mở, không nêu ngưỡng. Biến thể ít nhất 1 TS của W26 đã bị loại"*
- **Vấn đề**: với nhiều thí sinh cùng trả lời một hàng ngang, không rule nào nói cần bao nhiêu người đúng để mở miếng ghép (1 người, người được chọn hàng, đa số, hay tất cả).
- **Câu hỏi**: Cần bao nhiêu thí sinh trả lời đúng một từ hàng ngang để miếng ghép tương ứng được mở, và người chọn hàng có vai trò đặc biệt trong điều kiện này không?

### GRR-015 · AMBIGUOUS · Trung bình
- **Vị trí**: `game-rules-inventory.md` §R-VCNV-01 vs `fandom-olympia-26-luat-choi.md` §VCNV
- **Trích**: inventory ghi *"Kết quả khi đúng: +10 cho mỗi người đúng"*, trong khi nguồn chỉ nói *"Trả lời đúng được 10 điểm."*
- **Vấn đề**: mệnh đề "cho mỗi người đúng" là diễn giải, không có trong nguồn. Nguồn không nói điểm dành cho tất cả người đúng hay chỉ người có lượt chọn hàng đó.
- **Câu hỏi**: 10 điểm của từ hàng ngang dành cho mọi thí sinh trả lời đúng hay chỉ cho thí sinh đã chọn hàng ngang đó?

### GRR-016 · CONFLICT · Cao · `[ĐÓNG — Đ-8.3, duyệt Đ-12]`
- **Vị trí**: `fandom-olympia-26-luat-choi.md` §Vượt chướng ngại vật (mâu thuẫn nội tại trong nguồn)
- **Trích**: *"Mỗi thí sinh có tối đa 1 lượt lựa chọn để chọn trả lời một trong các từ hàng ngang này"* vs *"nếu thí sinh ở vị trí cuối cùng hoàn thành lượt lựa chọn từ hàng ngang mà vẫn còn từ hàng ngang chưa được lựa chọn, lượt lựa chọn sẽ quay trở lại với thí sinh ở vị trí số 1."*
- **Vấn đề**: hai câu trong cùng nguồn cho kết quả trái ngược với thí sinh vị trí 1 khi vòng quay lại (tối đa 1 lượt, hay được lượt thứ hai). Ngoài ra mệnh đề mở đầu *"Trong trường hợp này"* khiến không rõ luật quay vòng chỉ áp khi đã có người bị loại hay áp luôn.
- **Câu hỏi**: Khi lượt chọn quay lại vị trí số 1, thí sinh đó có được lượt chọn thứ hai không, và luật quay vòng chỉ áp dụng khi đã có thí sinh bị loại hay trong mọi trường hợp?

### GRR-017 · MISSING · Cao · `[v2]`
- **Vị trí**: `game-rules-inventory.md` §R-VCNV-03, §U-4, §8B K-11
- **Trích**: *"SPEC §5 cho rowCount 4..8 và DEF D1 cho 1-12 thí sinh — mỗi TS tối đa 1 lượt không đủ lượt cho 12 người / 4 hàng. Không nguồn nào xử lý."*
- **Vấn đề**: khi số thí sinh nhiều hơn số hàng, một phần thí sinh không bao giờ có lượt chọn; khi ít hơn, luật quay vòng phải chạy nhưng va vào GRR-016. Cả hai biên đều không có luật.
- **Câu hỏi**: Với 1-12 ghế và 4-8 hàng ngang, luật phân bổ lượt chọn hàng ngang là gì khi số thí sinh nhiều hơn hoặc ít hơn số hàng?

### GRR-018 · AMBIGUOUS · Trung bình · `[ĐÓNG PHẦN — Đ-4.4; còn Đ-4.4a]`
- **Vị trí**: `fandom-olympia-26-luat-choi.md` §Vượt chướng ngại vật; `game-rules-inventory.md` §R-VCNV-03
- **Trích**: *"bắt đầu từ thí sinh ở vị trí số 1"* · inventory: *"vị trí số 1 xác định thế nào khi số ghế khác 4."*
- **Vấn đề**: vị trí số 1 có thể là ghế vật lý, thứ tự đăng ký, hoặc người dẫn đầu điểm. Không rule nào định nghĩa.
- **Câu hỏi**: Vị trí số 1 được xác định bằng tiêu chí nào và có thay đổi giữa các vòng không?

### GRR-019 · BOUNDARY_UNDEFINED · Cao
- **Vị trí**: `fandom-olympia-26-luat-choi.md` §Vượt chướng ngại vật; `game-rules-inventory.md` §R-VCNV-04
- **Trích**: *"Thí sinh có thể bấm chuông trả lời Chướng ngại vật bất cứ lúc nào. Trả lời đúng Chướng ngại vật trong 1 từ hàng ngang đầu tiên được 60 điểm…"*
- **Vấn đề**: thang điểm bắt đầu ở mốc 1 từ hàng ngang nhưng luật cho bấm bất cứ lúc nào, tức cả khi **chưa mở hàng nào** (mốc 0) — giá trị điểm ở mốc 0 không tồn tại. Thêm nữa, cụm "trong 1 từ hàng ngang" mơ hồ giữa "sau khi 1 hàng đã mở" và "trong lúc đang hỏi hàng thứ nhất".
- **Câu hỏi**: Trả lời đúng Chướng ngại vật khi chưa có hàng ngang nào được mở thì được bao nhiêu điểm, và mốc "trong N từ hàng ngang" đếm theo số hàng đã mở hay số hàng đã hỏi?

### GRR-020 · MISSING · Cao
- **Vị trí**: `game-rules-inventory.md` §R-VCNV-04, §U-3
- **Trích**: *"SPEC §5 Zod: cnvPointsByRowsOpened.length == rowCount (+1), nên rowCount 8 cần 9 giá trị; không nguồn nào nói 9 giá trị đó là bao nhiêu."*
- **Vấn đề**: cấu hình cho phép 4-8 hàng ngang nhưng thang điểm CNV chỉ được luật gốc định nghĩa cho đúng 4 hàng (60/50/40/30 và 20).
- **Câu hỏi**: Với 5-8 hàng ngang, thang điểm Chướng ngại vật theo số hàng đã mở là bao nhiêu?

### GRR-021 · MISSING · Cao · `[ĐÓNG PHẦN — Đ-7: queue theo thứ tự tới, reject không mất lượt]`
- **Vị trí**: `fandom-olympia-26-luat-choi.md` §Vượt chướng ngại vật; `game-rules-inventory.md` §R-VCNV-04
- **Trích**: *"Thí sinh có thể bấm chuông trả lời Chướng ngại vật bất cứ lúc nào."* · *"Nếu trả lời sai Chướng ngại vật, thí sinh sẽ bị loại khỏi phần thi này."*
- **Vấn đề**: không rule nào xử lý đồng thời: hai thí sinh bấm chuông giải CNV chênh nhau vài ms thì ai được trả lời, người kia có bị coi là đã dùng lượt không, và sau khi người thứ nhất bị chấm sai thì lần bấm của người thứ hai tính ở mốc điểm nào.
- **Câu hỏi**: Khi nhiều thí sinh bấm chuông giải Chướng ngại vật gần như cùng lúc, thứ tự xử lý là gì, và lần bấm kế tiếp sau một phán quyết sai được tính ở mốc điểm nào?

### GRR-022 · MISSING · Cao
- **Vị trí**: `fandom-olympia-26-luat-choi.md` §Vượt chướng ngại vật; `game-rules-inventory.md` §R-VCNV-04
- **Trích**: *"Nếu trả lời sai Chướng ngại vật, thí sinh sẽ bị loại khỏi phần thi này."*
- **Vấn đề**: cụm bị loại khỏi phần thi này không được định nghĩa về phạm vi. Nguồn chỉ nêu hệ quả với **lượt chọn** (*"thí sinh ở vị trí tiếp theo sẽ được lựa chọn"*), không nói người bị loại còn được trả lời hàng ngang để lấy 10 điểm không, còn được trả lời câu ô trung tâm không.
- **Câu hỏi**: Thí sinh bị loại khỏi VCNV còn được quyền gì trong phần còn lại của vòng: trả lời hàng ngang, trả lời câu ô trung tâm, hay không còn quyền nào?

### GRR-023 · MISSING · Thấp
- **Vị trí**: `game-rules-inventory.md` §R-VCNV-04, §U-24
- **Trích**: *"người bị loại có bị trừ điểm hàng ngang đã kiếm không (U-24)"*
- **Vấn đề**: nhánh xử lý điểm đã tích luỹ của người bị loại không được định nghĩa.
- **Câu hỏi**: Điểm hàng ngang mà thí sinh đã ghi trước khi bị loại có được giữ không?

### GRR-024 · MISSING · Trung bình
- **Vị trí**: `fandom-olympia-26-luat-choi.md` §Vượt chướng ngại vật; `game-rules-inventory.md` §R-VCNV-05, §U-25
- **Trích**: *"Trả lời đúng câu hỏi ở ô trung tâm được 10 điểm, trả lời sai thì ô trung tâm sẽ không được mở ra."*
- **Vấn đề**: (a) không nói ai được trả lời câu ô trung tâm — cả sân, người chưa bị loại, hay theo lượt; (b) với nhiều thí sinh, trả lời sai là sai của ai — ngưỡng mở ô trung tâm không được định nghĩa, cùng dạng thiếu như GRR-014.
- **Câu hỏi**: Ai được trả lời câu hỏi ô trung tâm, và cần bao nhiêu người đúng để ô trung tâm được mở?

### GRR-025 · BOUNDARY_UNDEFINED · Trung bình
- **Vị trí**: `fandom-olympia-26-luat-choi.md` §Vượt chướng ngại vật; `game-rules-inventory.md` §R-VCNV-05
- **Trích**: *"Các thí sinh sẽ có 15 giây suy nghĩ để đưa ra Chướng ngại vật. Trả lời đúng Chướng ngại vật sau gợi ý cuối cùng ở ô trung tâm chỉ được 20 điểm."*
- **Vấn đề**: không rule nào nói mốc điểm khi câu ô trung tâm **bị trả lời sai** (ô không mở) nhưng vẫn có 15 giây giải CNV — vẫn 20 điểm hay quay lại mốc theo số hàng đã mở; cũng không nói hết 15 giây không ai đúng thì vòng kết thúc ra sao.
- **Câu hỏi**: Khi câu ô trung tâm bị trả lời sai, thí sinh giải đúng Chướng ngại vật trong 15 giây sau đó được bao nhiêu điểm, và nếu hết 15 giây không ai đúng thì vòng kết thúc thế nào?

### GRR-026 · ORDER_DEPENDENT · Cao
- **Vị trí**: `game-rules-inventory.md` §R-VCNV-06 (U-16)
- **Trích**: *"R26 §7 — nhãn đề xuất của Claude, KHÔNG có trong F26/W26"* · *"timer hàng ngang đang chạy; có người bấm chuông giải CNV… dừng đồng hồ… loại người bấm rồi cho đồng hồ chạy tiếp"* · *"chưa được user chốt"*
- **Vấn đề**: luật gốc chỉ định nghĩa trường hợp bấm chuông **trước khi lựa chọn** hàng ngang. Trường hợp bấm **giữa lúc timer hàng ngang chạy** phụ thuộc hoàn toàn vào thứ tự xử lý (xử CNV trước hay chấm hàng ngang trước; đáp án hàng ngang đã gửi trong lúc đồng hồ ngừng có tính không) và chưa được chốt.
- **Câu hỏi**: Khi có người bấm chuông giải Chướng ngại vật trong lúc đồng hồ hàng ngang đang chạy, sự kiện nào được xử lý trước, và các đáp án hàng ngang đã gửi trước đó xử lý ra sao?

### GRR-027 · MISSING · Trung bình
- **Vị trí**: `game-rules-inventory.md` §R-VCNV-07
- **Trích**: *"mở toàn bộ miếng ghép + công bố CNV = NÚT THỦ CÔNG của admin (không auto)"* · *"Chưa định nghĩa: nếu admin không bấm nút mở, vòng có tự kết thúc không."*
- **Vấn đề**: trạng thái tất cả thí sinh đã bị loại không có đường thoát nếu admin không thao tác.
- **Câu hỏi**: Nếu admin không bấm nút mở Chướng ngại vật, vòng VCNV kết thúc bằng cách nào?

### GRR-028 · MISSING · Thấp
- **Vị trí**: `game-rules-inventory.md` §R-VCNV-08, §U-32
- **Trích**: *"gợi ý hiện lúc nào (khi mở hàng ngang hay sau khi hết giờ) (U-32)"*
- **Vấn đề**: điều kiện kích hoạt gợi ý ký tự không được định nghĩa.
- **Câu hỏi**: Gợi ý ký tự được hiển thị tại thời điểm nào trong chu trình một hàng ngang?

---

## C. TĂNG TỐC

### GRR-029 · MISSING · Cao · `[v2]`
- **Vị trí**: `fandom-olympia-26-luat-choi.md` §Tăng tốc; `game-rules-inventory.md` §R-TT-01, §U-2, §8B K-11
- **Trích**: *"Trả lời đúng và nhanh nhất được 40 điểm… Trả lời đúng và nhanh thứ 4 được 10 điểm."* · inventory: *"12 ghế cần 12 giá trị, không nguồn nào định nghĩa."*
- **Vấn đề**: thang điểm chỉ có 4 bậc, hệ thống hỗ trợ 1-12 ghế. Giá trị cho bậc 5 đến 12 không tồn tại trong bất kỳ nguồn nào.
- **Câu hỏi**: Với số thí sinh khác 4, thang điểm theo thứ hạng tốc độ của Tăng tốc là bao nhiêu ở từng bậc?

### GRR-030 · MISSING · Cao · `[v2]`
- **Vị trí**: `fandom-olympia-26-luat-choi.md` §Tăng tốc; `game-rules-inventory.md` §R-TT-01
- **Trích**: *"Trả lời đúng và nhanh thứ 4 được 10 điểm."*
- **Vấn đề**: nếu số người trả lời đúng **nhiều hơn** số bậc điểm (ví dụ 6 người đúng, 4 bậc), luật không nói người từ bậc 5 trở đi được bao nhiêu (0 điểm, hay lặp bậc cuối). Ngược lại nếu ít hơn số bậc thì các bậc thấp bỏ trống, cũng không phát biểu.
- **Câu hỏi**: Khi số thí sinh trả lời đúng vượt quá số bậc điểm được khai báo, những người ngoài bậc cuối được bao nhiêu điểm?

### GRR-031 · CONFLICT · Cao
- **Vị trí**: `game-rules-inventory.md` §R-TT-02 (K-8); `fandom-olympia-26-luat-choi.md` §Tăng tốc
- **Trích**: `F26`: *"cùng trả lời đúng trong cùng một khoảng thời gian"* · inventory: *"W26 = 2 chữ số thập phân (khoảng 10ms); F26 không nêu; R26 §7 chốt ms… SPEC §6 còn có option tieRule microsecond phá vỡ hoàn toàn quy tắc này; không nguồn nào nói default."*
- **Vấn đề**: ba độ phân giải khác nhau cho cùng một điều kiện đồng thời gian, chênh nhau tới 4 bậc độ lớn, và không có giá trị mặc định được chốt. Cùng một cặp submission có thể vừa là hoà vừa không tuỳ cấu hình.
- **Câu hỏi**: "Cùng một khoảng thời gian" ở Tăng tốc được đo ở độ phân giải nào, và giá trị mặc định của tuỳ chọn tieRule là gì?

### GRR-032 · BOUNDARY_UNDEFINED · Cao
- **Vị trí**: `game-rules-inventory.md` §R-TT-02
- **Trích**: *"Kết quả khi thoả: cùng nhận một mức điểm (ví dụ 40/40/20/10)"* · *"khi share-high, hạng tiếp theo nhảy mấy bậc (40/40/20/10 hàm ý nhảy 1 bậc, không phát biểu thành luật)."*
- **Vấn đề**: quy tắc xếp hạng sau khi có đồng hạng (nhảy bậc hay không) chỉ tồn tại dưới dạng ví dụ, không phải luật. Với đồng hạng 3 người hoặc đồng hạng ở bậc giữa, kết quả không suy ra được.
- **Câu hỏi**: Sau một nhóm đồng hạng gồm k người ở bậc thứ n, người kế tiếp nhận bậc điểm thứ mấy?

### GRR-033 · AMBIGUOUS · Trung bình
- **Vị trí**: `game-rules-inventory.md` §R-TT-03
- **Trích**: *"Nội dung y hệt bản trước (so sau trim, per seat) thì không cập nhật timestamp."*
- **Vấn đề**: "bản trước" không rõ là bản gửi liền trước hay bản đang được ghi nhận. Chuỗi A rồi B rồi lại A cho hai cách hiểu khác nhau về timestamp cuối cùng, ảnh hưởng trực tiếp xếp hạng tốc độ.
- **Câu hỏi**: Khi thí sinh gửi A, rồi B, rồi lại A, timestamp dùng để xếp hạng là lần gửi A đầu tiên hay lần gửi A thứ hai?

### GRR-034 · MISSING · Trung bình
- **Vị trí**: `game-rules-inventory.md` §R-TT-03
- **Trích**: *"Submission RỖNG (toàn whitespace sau trim) thì SKIP, giữ bản trước (DEF D20.1)."*
- **Vấn đề**: rule này khiến thí sinh không thể **rút lại** đáp án đã gửi. Không rule nào nói đây là chủ đích, cũng không có cơ chế nào khác cho phép rút. Nhánh thí sinh muốn bỏ trống không có kết quả.
- **Câu hỏi**: Thí sinh có được phép rút lại đáp án đã gửi để nộp trống không; nếu không thì đây là quyết định luật hay hệ quả phụ?

### GRR-035 · BOUNDARY_UNDEFINED · Trung bình
- **Vị trí**: `game-rules-inventory.md` §R-TT-03
- **Trích**: *"Tới sau server-timeout thì loại."*
- **Vấn đề**: không định nghĩa biên "sau" (lớn hơn, hay lớn hơn bằng) so với mốc timeout tính bằng ms, trong khi cùng tài liệu chốt độ phân giải ms cho xếp hạng.
- **Câu hỏi**: Submission có server-received timestamp bằng đúng mốc timeout được nhận hay bị loại?

### GRR-036 · ORDER_DEPENDENT · Trung bình
- **Vị trí**: `game-rules-inventory.md` §R-TT-03, §R-GEN-05
- **Trích**: *"ranking theo server-received timestamp của bản cuối"* · *"buzzer timestamp gắn tại instance OWNER"*
- **Vấn đề**: khi hai submission của **cùng một ghế** đến với cùng timestamp (hai tab, hai thiết bị), không rule nào nói bản nào là bản cuối. Tương tự cho hai lần bấm chuông cùng ghế. Không có tie-break nội bộ theo ghế.
- **Câu hỏi**: Khi cùng một ghế có hai submission đến ở cùng một mốc thời gian server, bản nào được coi là bản cuối cùng?

### GRR-037 · MISSING · Cao
- **Vị trí**: `fandom-olympia-26-luat-choi.md` §Tăng tốc; `game-rules-inventory.md` §R-TT-04, §U-36
- **Trích**: *"Câu hỏi nhìn nhanh · Câu hỏi sắp xếp · Câu hỏi suy luận · Câu hỏi đoạn băng"* · inventory: *"câu sắp xếp và lựa chọn ảnh cần mô hình dữ liệu hoàn toàn khác acceptedAnswers text — không nguồn repo nào đề cập."*
- **Vấn đề**: luật gốc công nhận loại câu sắp xếp nhưng không rule nào định nghĩa cách trả lời, cách xác định đúng sai (đúng hoàn toàn hay đúng một phần), và cách so sánh tốc độ với các loại câu khác.
- **Câu hỏi**: Câu hỏi sắp xếp ở Tăng tốc có thuộc phạm vi hệ thống không; nếu có thì đúng sai được xác định theo tiêu chí nào (toàn phần hay từng vị trí)?

### GRR-038 · MISSING · Thấp
- **Vị trí**: `game-rules-inventory.md` §R-TT-05
- **Trích**: *"điểm theo mốc dữ kiện đang mở khi bấm (cluePoints, length == maxClues; câu ít dữ kiện dùng PREFIX)"* · *"câu có 2 clues trong khi maxClues=4 — xử lý giữa trận."*
- **Vấn đề**: khi câu có ít dữ kiện hơn maxClues, luật điểm dùng prefix nhưng không nói điều gì xảy ra sau khi dữ kiện cuối đã mở mà chưa ai bấm (chờ hết giờ, hay bỏ câu).
- **Câu hỏi**: Ở thể thức clue-buzz, sau khi dữ kiện cuối cùng đã mở mà chưa ai bấm chuông thì câu hỏi kết thúc thế nào?

### GRR-039 · ORDER_DEPENDENT · Cao · `[ĐÓNG PHẦN — Đ-5.3.1: một câu = một event cho toàn bộ người chơi]`
- **Vị trí**: `game-rules-inventory.md` §R-GEN-03 (Hệ quả), §R-TT-01, §R-GEN-07
- **Trích**: *"hệ thống ghi nhận submission + timestamp và xếp hạng, nhưng đúng do admin xác nhận, nên thứ hạng điểm chỉ tính trên tập người được admin chấm ĐÚNG."* · R-GEN-07: *"undo chỉ event chấm điểm GẦN NHẤT chưa có event khác build-upon"*
- **Vấn đề**: thứ hạng của mọi thí sinh phụ thuộc vào tập người được chấm đúng, nên **thứ tự admin chấm** và **việc admin đổi phán quyết** đều làm thay đổi điểm của những người đã được chấm trước. Không rule nào nói điểm được tính lại toàn bộ hay giữ nguyên, và quy tắc undo lại cấm sửa event không phải mới nhất.
- **Câu hỏi**: Khi admin đổi phán quyết của một thí sinh ở Tăng tốc sau khi đã chấm những người khác, thứ hạng và điểm của toàn bộ câu được tính lại hay giữ nguyên?

---

## D. VỀ ĐÍCH VÀ NGÔI SAO HY VỌNG

### GRR-040 · AMBIGUOUS · Cao · `[CHẶN — câu duy nhất còn lại của cụm "vị trí", xem Đ-4.5]`
- **Vị trí**: `fandom-olympia-26-luat-choi.md` §Về đích; `game-rules-inventory.md` §R-VD-02, §U-27
- **Trích**: *"Nếu các thí sinh có cùng điểm số, thí sinh sẽ bắt đầu từ vị trí đứng thấp nhất."* · inventory: *"vị trí đứng là ghế vật lý hay thứ hạng; với 1-12 ghế càng mơ hồ."*
- **Vấn đề**: cụm vị trí đứng thấp nhất có ít nhất ba cách hiểu: số ghế nhỏ nhất, số ghế lớn nhất (thấp theo vị trí sân khấu), hoặc thứ hạng thấp nhất. Đây là tie-break quyết định thứ tự thi của cả vòng.
- **Câu hỏi**: Vị trí đứng thấp nhất được hiểu là số ghế nhỏ nhất, số ghế lớn nhất, hay thứ hạng điểm thấp nhất?

### GRR-041 · AMBIGUOUS · Trung bình
- **Vị trí**: `fandom-olympia-26-luat-choi.md` §Về đích (Thứ tự tham gia); `game-rules-inventory.md` §R-VD-02
- **Trích**: *"Điểm số được tính tại thời điểm sau khi thí sinh ở lượt 1 hoàn thành phần thi."*
- **Vấn đề**: không nói "hoàn thành phần thi" là mốc nào (sau câu cuối, sau khi admin chấm xong câu cuối, sau khi cửa sổ cướp của câu cuối đóng). Điểm cướp và điểm NSHV được chốt ở các mốc khác nhau, nên thứ tự lượt kế tiếp phụ thuộc mốc chọn.
- **Câu hỏi**: Thời điểm thí sinh ở lượt trước hoàn thành phần thi được tính là lúc nào, và điểm thay đổi do cướp có được tính vào lúc xếp lượt kế tiếp không?

### GRR-042 · MISSING · Cao
- **Vị trí**: `fandom-olympia-26-luat-choi.md` §Về đích
- **Trích**: *"1 trong 3 thí sinh còn lại sẽ giành quyền trả lời bằng cách bấm chuông nhanh trong 5 giây."*
- **Vấn đề**: nguồn chỉ định nghĩa cửa sổ **bấm chuông** 5 giây, không định nghĩa thí sinh giành quyền có **bao nhiêu thời gian để trả lời** sau khi bấm (chỉ câu thực hành mới có thời gian riêng 20s/40s). Không rule nào trong repo bù khoảng trống này.
- **Câu hỏi**: Sau khi giành quyền cướp, thí sinh có bao nhiêu giây để đưa ra đáp án?

### GRR-043 · MISSING · Cao
- **Vị trí**: `fandom-olympia-26-luat-choi.md` §Về đích; `game-rules-inventory.md` §R-VD-05
- **Trích**: *"thí sinh bấm chuông mà trả lời sai sẽ bị trừ một nửa số điểm của câu hỏi."*
- **Vấn đề**: không rule nào nói sau khi người cướp đầu tiên trả lời sai thì cửa sổ 5 giây có mở lại cho các thí sinh còn lại hay câu hỏi kết thúc. Cũng không nói xử lý khi nhiều người bấm trong 5 giây (chỉ người nhanh nhất, hay xếp hàng đợi).
- **Câu hỏi**: Sau khi người giành quyền cướp trả lời sai, các thí sinh còn lại có được bấm chuông tiếp trong phần thời gian còn lại không?

### GRR-044 · CONFLICT · Cao
- **Vị trí**: `fandom-olympia-26-luat-choi.md` §Về đích; `CLAUDE.md` §UX BẮT BUỘC; `game-rules-inventory.md` §R-VD-05
- **Trích**: `F26`: *"Với thí sinh giành quyền trả lời, chương trình sẽ chỉ ghi nhận đáp án đầu tiên của thí sinh đó."* vs `CLAUDE.md`: *"KHÔNG chặn gửi lại… người dùng gửi lại được — server nhận bản cuối cùng trước timeout."*
- **Vấn đề**: hai rule cùng match trên một hành vi (người cướp gửi đáp án nhiều lần) nhưng cho kết quả ngược nhau: bản đầu và bản cuối. Không có rule phân xử; `CLAUDE.md` chỉ trừ trường hợp khoá theo luật chơi (disable nút), không trừ trường hợp ghi nhận bản đầu.
- **Câu hỏi**: Với thí sinh cướp quyền ở Về đích, hệ thống ghi nhận đáp án đầu tiên theo luật gốc hay bản cuối cùng theo nguyên tắc UX toàn repo?

### GRR-045 · MISSING · Cao · `[v2]`
- **Vị trí**: `fandom-olympia-26-luat-choi.md` §Về đích; `game-rules-inventory.md` §R-VD-05, §U-5, §8B K-11
- **Trích**: *"1 trong 3 thí sinh còn lại"* · inventory: *"3 TS còn lại với 1-12 ghế (U-5)"*
- **Vấn đề**: luật viết cho đúng 4 ghế. Với 1 ghế thì không có ai cướp (nhánh cướp trở thành không bao giờ xảy ra và không có xử lý thay thế); với 12 ghế thì "3 thí sinh còn lại" không xác định là ai.
- **Câu hỏi**: Với số ghế khác 4, những ai được quyền cướp, và khi chỉ có 1 thí sinh thì câu trả lời sai được xử lý thế nào?

### GRR-046 · BOUNDARY_UNDEFINED · Trung bình · `[THU HẸP — Đ-3]`
- **Vị trí**: `game-rules-inventory.md` §R-VD-05, §U-20
- **Trích**: *"điểm lẻ khi trừ một nửa giá trị lẻ (R26 §7 đề xuất Zod ép chẵn hoặc floor về 0, chưa chốt (U-20))"*
- **Vấn đề**: quy tắc làm tròn cho một nửa số điểm khi giá trị câu lẻ chưa được chốt; cấu hình cho phép mốc điểm tuỳ ý (preset 30/50/70/90).
- **Câu hỏi**: Khi giá trị câu hỏi là số lẻ, một nửa số điểm được làm tròn theo quy tắc nào?

### GRR-047 · MISSING · Cao · `[ĐÓNG — Đ-11.A, duyệt Đ-12: trừ MỘT lần]`
- **Vị trí**: `fandom-olympia-26-luat-choi.md` §Về đích; `game-rules-inventory.md` §R-VD-06, §U-8
- **Trích**: *"Trả lời sai sẽ bị trừ đi số điểm của câu hỏi, kể cả các thí sinh còn lại có giành quyền trả lời hay không."* · inventory: *"Người dùng NSHV mất 1 hay 2 lần value?"*
- **Vấn đề**: khi thí sinh đặt NSHV trả lời sai và có người cướp đúng, hai rule cùng áp lên cùng một thí sinh: trừ value theo NSHV và trừ value theo transfer. Tổng hình phạt không được định nghĩa.
- **Câu hỏi**: Khi thí sinh đặt Ngôi sao hy vọng trả lời sai và có người cướp đúng, thí sinh đó bị trừ một lần hay hai lần giá trị câu hỏi?

### GRR-048 · MISSING · Cao · `[ĐÓNG — Đ-11.A, duyệt Đ-12: đóng khi admin bấm hiển thị câu hỏi]`
- **Vị trí**: `fandom-olympia-26-luat-choi.md` §Về đích; `game-rules-inventory.md` §R-VD-06, §U-1
- **Trích**: *"Thí sinh phải đặt ngôi sao hy vọng trước khi câu hỏi được đọc lên bởi người dẫn chương trình hoặc hiện lên trên màn hình."*
- **Vấn đề**: cùng dạng thiếu như GRR-001 nhưng ở mốc khác: hệ thống không có định nghĩa cho mốc trước khi câu hỏi được đọc hoặc hiện, nên không xác định được thời điểm đóng cửa sổ đặt NSHV, và không nói yêu cầu đặt muộn bị xử lý thế nào.
- **Câu hỏi**: Cửa sổ đặt Ngôi sao hy vọng mở và đóng tại sự kiện nào, và yêu cầu đặt sau thời hạn được xử lý ra sao (từ chối im lặng, báo lỗi, hay tiêu mất lượt)?

### GRR-049 · MISSING · Trung bình
- **Vị trí**: `fandom-olympia-26-luat-choi.md` §Về đích; `game-rules-inventory.md` §R-VD-06
- **Trích**: *"Mỗi thí sinh được đặt ngôi sao hy vọng 1 lần."*
- **Vấn đề**: nhiều nhánh không có kết quả: (a) đặt NSHV rồi **hết giờ không trả lời** — có tính là trả lời sai để trừ value không; (b) đặt rồi **rút lại** trước khi câu được đọc — có tiêu lượt không; (c) gửi lệnh đặt **hai lần** (idempotency) — lần thứ hai bị từ chối hay ghi đè; (d) NSHV có đặt được ở **câu cướp** của người khác không.
- **Câu hỏi**: Đặt NSHV rồi không trả lời, rút lại, hoặc gửi lệnh đặt hai lần được xử lý ra sao, và người cướp quyền có được dùng NSHV trên câu đang cướp không?

### GRR-050 · MISSING · Cao
- **Vị trí**: `fandom-olympia-26-luat-choi.md` §Về đích; `game-rules-inventory.md` §R-VD-04, §U-6
- **Trích**: *"Thí sinh nếu trả lời đúng hoặc thực hành đạt yêu cầu sẽ ghi được điểm"* · inventory: *"không có trường dữ liệu nào cho câu thực hành… Chấm thực hành đạt yêu cầu là đánh giá của người, không phải so khớp text."*
- **Vấn đề**: một loại câu có bộ thời gian và quy trình riêng tồn tại trong luật nhưng không có cách nào để đánh dấu và không có quy tắc chuyển trạng thái (khi nào chuyển từ pha suy nghĩ sang pha thực hành, ai bấm chuyển).
- **Câu hỏi**: Câu hỏi thực hành có thuộc phạm vi v1 không; nếu có thì chuyển từ pha suy nghĩ sang pha thực hành xảy ra tại sự kiện nào và ai xác nhận đạt yêu cầu?

### GRR-051 · MISSING · Trung bình
- **Vị trí**: `fandom-olympia-26-luat-choi.md` §Về đích; `game-rules-inventory.md` §R-VD-04
- **Trích**: *"Đối với câu hỏi 20 điểm, thời gian thực hành là 20 giây. Đối với câu hỏi 30 điểm, thời gian thực hành là 40 giây."* (áp cho người cướp)
- **Vấn đề**: nguồn nêu thời gian **thực hành** khi cướp nhưng không nêu thời gian **suy nghĩ** khi cướp cho câu thực hành, và không nói người cướp có được xem lại phần giới thiệu dụng cụ không. Cùng khoảng trống với GRR-042 nhưng ở nhánh câu thực hành.
- **Câu hỏi**: Với câu thực hành bị cướp, người cướp có thời gian suy nghĩ bao lâu trước khi bước vào thời gian thực hành?

### GRR-052 · MISSING · Trung bình
- **Vị trí**: `fandom-olympia-26-luat-choi.md` §Về đích; `game-rules-inventory.md` §R-VD-01
- **Trích**: *"Mỗi thí sinh có một lượt lựa chọn 3 câu hỏi 20, 30 điểm để tạo thành một gói điểm của mình."*
- **Vấn đề**: không nói ràng buộc thành phần gói: có bắt buộc dùng đủ cả hai mức không, có được chọn 30/30/30 hay 20/20/20 không, thứ tự hỏi trong gói do ai quyết định.
- **Câu hỏi**: Gói 3 câu có ràng buộc gì về số câu mỗi mức, và thứ tự hỏi 3 câu trong gói do ai quyết định?

### GRR-053 · CONFLICT · Trung bình
- **Vị trí**: `game-rules-inventory.md` §R-VD-01 vs §R-VD-07
- **Trích**: R-VD-01: *"R26 §7 + DEF D13.4 — admin chọn hộ, default 20/20/20."* vs R-VD-07: *"bật allowValueExhaustion (mốc hết câu thì khoá lựa chọn, UI hiện mốc mờ)"*
- **Vấn đề**: hai rule cùng match khi thí sinh không chọn kịp và mốc 20 điểm đã cạn câu: một rule ép default 20/20/20, rule kia nói mốc 20 bị khoá. Kết quả mâu thuẫn, không có rule ưu tiên.
- **Câu hỏi**: Khi mốc điểm mặc định đã hết câu mà thí sinh không chọn kịp, admin chọn hộ theo quy tắc nào?

### GRR-054 · MISSING · Thấp
- **Vị trí**: `game-rules-inventory.md` §R-VD-01, §U-26
- **Trích**: *"thí sinh có được đổi gói sau khi chọn không (U-26)"*
- **Vấn đề**: nhánh sửa lựa chọn không có kết quả (cho phép đến mốc nào, có audit không).
- **Câu hỏi**: Thí sinh có được đổi gói sau khi đã xác nhận không, và nếu có thì hạn chót là mốc nào?

### GRR-055 · MISSING · Trung bình
- **Vị trí**: `game-rules-inventory.md` §R-VD-05, §U-19
- **Trích**: *"R26 §7: câu bị skip khi đang mở cửa sổ cướp thì huỷ cửa sổ, không ai cộng/trừ (nhãn đề xuất)."*
- **Vấn đề**: chưa chốt; và không nói điểm đã trừ của người trả lời sai (nếu đã chốt trước đó) có được hoàn không, tức là thao tác bị huỷ thì dữ liệu có thay đổi hay không chưa xác định.
- **Câu hỏi**: Khi một câu bị bỏ giữa lúc cửa sổ cướp đang mở, các thay đổi điểm đã ghi nhận trước đó của câu ấy có được hoàn lại không?

### GRR-056 · MISSING · Trung bình
- **Vị trí**: `fandom-olympia-26-luat-choi.md` §Về đích; `game-rules-inventory.md` §R-VD-03, §U-22
- **Trích**: *"Chương trình sẽ ghi nhận đáp án cuối cùng của thí sinh sau khi hết giờ."*
- **Vấn đề**: hàm ý câu hỏi luôn chạy hết thời gian ngay cả khi thí sinh đã trả lời. Không rule nào nói có cơ chế kết thúc sớm, cũng không nói admin có được rút ngắn không; cùng câu hỏi này còn để ngỏ ở Khởi động (U-22).
- **Câu hỏi**: Có tồn tại cơ chế kết thúc câu hỏi sớm khi thí sinh đã gửi đáp án không, hay mọi câu đều chạy hết thời gian?

---

## E. TIE-BREAK / CÂU HỎI PHỤ

### GRR-057 · MISSING · Cao · `[ĐÓNG PHẦN — Đ-10.7: recommend mọi nhóm hoà, admin chọn]`
- **Vị trí**: `fandom-olympia-26-luat-choi.md` §Câu hỏi phụ; `game-rules-inventory.md` §R-TB-01 (K-14)
- **Trích**: `F26`: *"Sau phần thi Về đích, các thí sinh có cùng số điểm sẽ bước vào phần thi Câu hỏi phụ."* · inventory: *"DEF D13.5 giới hạn chỉ vị trí NHẤT… repo hẹp hơn nguồn."*
- **Vấn đề**: quyết định thu hẹp tie-break về vị trí nhất, nhưng không rule nào nói kết quả xếp hạng cho các vị trí hoà còn lại (đồng hạng, hay phân định bằng tiêu chí khác). Trận có thể kết thúc mà không có thứ hạng xác định cho vị trí 2 và 3.
- **Câu hỏi**: Khi hai thí sinh hoà điểm ở vị trí không phải vị trí nhất, thứ hạng cuối cùng của họ được xác định thế nào?

### GRR-058 · CONFLICT · Trung bình
- **Vị trí**: `fandom-olympia-26-luat-choi.md` §Câu hỏi phụ; `game-rules-inventory.md` §R-TB-02
- **Trích**: `F26`: *"Thí sinh bấm chuông nhanh nhất trả lời đúng sẽ là thí sinh có số điểm cao nhất bằng với số điểm của thí sinh còn lại."* · inventory: *"R26 §5 nói câu phụ không cộng vào điểm trận; F26 diễn đạt lạ… không nói rõ có cộng điểm không."*
- **Vấn đề**: hai nguồn cho hai kết quả khác nhau về việc điểm cuối trận của người thắng tie-break có thay đổi không; câu trong `F26` tự nó mơ hồ.
- **Câu hỏi**: Người thắng câu hỏi phụ có được cộng điểm vào điểm trận không, hay chỉ thay đổi thứ hạng?

### GRR-059 · MISSING · Cao
- **Vị trí**: `fandom-olympia-26-luat-choi.md` §Câu hỏi phụ; `game-rules-inventory.md` §R-TB-02 (K-9)
- **Trích**: *"Nếu trả lời sai, các thí sinh sẽ bước sang câu hỏi tiếp theo."*
- **Vấn đề**: nguồn không nói thí sinh đã bấm sai có bị loại khỏi phần thi Câu hỏi phụ không, hay vẫn được bấm ở các câu sau. Hai cách hiểu cho hai kết quả thắng thua khác nhau; việc K-9 đã phân xử sang câu tiếp không trả lời câu hỏi này.
- **Câu hỏi**: Thí sinh bấm chuông và trả lời sai ở câu hỏi phụ có còn quyền bấm chuông ở các câu tiếp theo không?

### GRR-060 · MISSING · Cao · `[ĐÓNG — Đ-6.1: hiệu lệnh MC = thao tác bấm của admin]`
- **Vị trí**: `fandom-olympia-26-luat-choi.md` §Câu hỏi phụ; `game-rules-inventory.md` §R-TB-03, §U-1
- **Trích**: *"nếu có thí sinh bấm chuông trả lời trước khi có hiệu lệnh của người dẫn chương trình, thí sinh đó sẽ bị mất quyền trả lời câu hỏi."*
- **Vấn đề**: mốc hiệu lệnh của MC không có sự kiện tương ứng trong hệ thống (cùng nhóm với GRR-001 và GRR-048 nhưng là mốc thứ ba, khác nghĩa). Không có mốc này thì không phân biệt được lần bấm hợp lệ và không hợp lệ.
- **Câu hỏi**: Hiệu lệnh của người dẫn chương trình ở phần Câu hỏi phụ tương ứng với sự kiện nào trong hệ thống và do ai phát?

### GRR-061 · MISSING · Trung bình
- **Vị trí**: `fandom-olympia-26-luat-choi.md` §Câu hỏi phụ; `game-rules-inventory.md` §R-TB-03
- **Trích**: *"thí sinh đó sẽ bị mất quyền trả lời câu hỏi"*
- **Vấn đề**: nhánh khi **tất cả** thí sinh trong nhóm hoà đều mất quyền ở một câu không được định nghĩa: câu đó có tính vào 3 câu không, có sang câu tiếp ngay không.
- **Câu hỏi**: Nếu tất cả thí sinh trong nhóm hoà đều mất quyền ở cùng một câu, câu đó có được tính vào tổng 3 câu không?

### GRR-062 · MISSING · Trung bình
- **Vị trí**: `game-rules-inventory.md` §R-TB-04
- **Trích**: *"bốc thăm (exhaustedFallback default random-draw); R26 thêm: hệ thống random, admin xác nhận."*
- **Vấn đề**: trạng thái `TIE_BREAK` không có đường thoát nếu admin không xác nhận kết quả bốc thăm; cũng không nói admin có được bốc lại không (idempotency của thao tác bốc thăm).
- **Câu hỏi**: Nếu admin không xác nhận kết quả bốc thăm, trận kết thúc bằng cách nào; và admin có được yêu cầu bốc lại không?

### GRR-063 · ORDER_DEPENDENT · Trung bình
- **Vị trí**: `game-rules-inventory.md` §R-TB-05, §U-17
- **Trích**: *"chạy tie-break per nhóm hoà, theo thứ tự vị trí"* · *"chưa được user chốt"*
- **Vấn đề**: khi có nhiều nhóm hoà, thứ tự chạy quyết định pool câu còn lại và (nếu câu phụ có cộng điểm, xem GRR-058) cả kết quả các nhóm sau. Chưa chốt.
- **Câu hỏi**: Khi có nhiều nhóm hoà, các nhóm được phân định theo thứ tự nào và có dùng chung pool câu hỏi phụ không?

### GRR-064 · MISSING · Trung bình
- **Vị trí**: `game-rules-inventory.md` §R-TB-04, §U-10
- **Trích**: *"pool câu phụ cạn giữa tie-break (U-10)"*
- **Vấn đề**: nhánh hết câu trước khi đủ 3 câu không có kết quả được định nghĩa.
- **Câu hỏi**: Nếu pool câu hỏi phụ cạn trước khi hỏi đủ 3 câu, phần thi phân định bằng cách nào?

### GRR-065 · MISSING · Trung bình
- **Vị trí**: `fandom-olympia-26-luat-choi.md` §Câu hỏi phụ; `game-rules-inventory.md` §R-TB-05
- **Trích**: *"Sau 3 câu hỏi, nếu không tìm được thí sinh thắng cuộc, các thí sinh sẽ phải bốc thăm để chọn ra thí sinh thắng cuộc."*
- **Vấn đề**: với nhóm hoà từ 3 người trở lên (inventory xác nhận nhóm trên 2 người là có thật trong luật), luật chỉ chọn ra **một** người thắng, không nói thứ hạng của những người thua trong nhóm được phân định thế nào.
- **Câu hỏi**: Trong nhóm hoà từ 3 người trở lên, sau khi xác định được người thắng thì thứ hạng của những người còn lại được xác định thế nào?

---

## F. CHẤM ĐIỂM VÀ SO KHỚP ĐÁP ÁN

### GRR-066 · CONFLICT · Cao · `[ĐÓNG — Đ-1]`
- **Vị trí**: `fandom-olympia-26-luat-choi.md` §VCNV và §Tăng tốc; `game-rules-inventory.md` §R-VCNV-02, §R-GEN-04, §8B K-6
- **Trích**: `F26`: *"Nếu có bất kỳ sai sót về kí tự, dấu câu, ngữ pháp, câu trả lời của thí sinh đó sẽ không được công nhận."* vs R-GEN-04: *"case-insensitive + trim + collapse khoảng trắng"* bắt buộc, *"bỏ dấu tiếng Việt là CONFIG"*
- **Vấn đề**: hai rule cùng match trên cùng thao tác (so khớp đáp án) và cho kết quả ngược nhau. Inventory tự đánh giá: *"Hai quyết định user chốt cùng ngày 12/07 mâu thuẫn nhau… Nặng nhất trong danh sách."* Chưa phân xử.
- **Câu hỏi**: Quy tắc chính tả nghiêm ngặt của luật gốc hay quy tắc normalize của hệ thống được ưu tiên, và tuỳ chọn bỏ dấu tiếng Việt có được phép bật trong trận chính thức không?

### GRR-067 · AMBIGUOUS · Cao · `[ĐÓNG PHẦN — Đ-1; còn Đ-1.a/b/c]`
- **Vị trí**: `fandom-olympia-26-luat-choi.md` §VCNV; `game-rules-inventory.md` §R-VCNV-02, §U-35
- **Trích**: *"Đôi khi câu trả lời có ý nghĩa tương đồng và có cùng tổng số chữ cái với đáp án của chương trình cũng được chấp nhận."*
- **Vấn đề**: ba tầng mơ hồ trong một câu: (a) "đôi khi" không phải điều kiện xác định; (b) "ý nghĩa tương đồng" không có tiêu chí; (c) "tổng số chữ cái" không rõ đếm ký tự, đếm chữ cái không tính dấu và khoảng trắng, hay đếm từ — inventory ghi repo chỉ có `wordCount` là **số TỪ**. Ở §Tăng tốc, cùng ngoại lệ này lại **không có** điều kiện cùng tổng số chữ cái, tạo hai chuẩn khác nhau giữa hai vòng.
- **Câu hỏi**: Cùng tổng số chữ cái đếm đơn vị nào, ý nghĩa tương đồng do ai quyết định, và vì sao ngoại lệ này khác nhau giữa VCNV và Tăng tốc?

### GRR-068 · CONFLICT · Cao
- **Vị trí**: `game-rules-inventory.md` §R-GEN-03 vs các rule phạt theo timeout
- **Trích**: R-GEN-03: *"Điểm chỉ chốt khi ADMIN bấm Đúng/Sai… KHÔNG tồn tại autoJudge"* vs `F26`: *"bấm chuông mà không có câu trả lời sau 3 giây bị trừ 5 điểm"* và R-VD-06: *"Trả lời sai sẽ bị trừ đi số điểm của câu hỏi, kể cả các thí sinh còn lại có giành quyền trả lời hay không."*
- **Vấn đề**: có những hình phạt phát sinh từ **sự kiện thời gian** chứ không từ nội dung đáp án. Rule nền nói mọi điểm chỉ phát sinh từ phán quyết admin, nhưng không nói các hình phạt theo timeout có thuộc ngoại lệ không, và nếu thuộc thì admin thao tác gì trên một ô đáp án trống.
- **Câu hỏi**: Các hình phạt phát sinh do hết giờ (không có đáp án để chấm) có thuộc phạm vi chỉ admin quyết định không, và nếu có thì admin xác nhận bằng thao tác nào?

### GRR-069 · MISSING · Cao
- **Vị trí**: `game-rules-inventory.md` §R-GEN-03, §U-7
- **Trích**: *"hệ thống phân biệt câu miệng vs câu gõ bằng trường nào — SPEC §9 field list không có (U-7)"*
- **Vấn đề**: luật gốc dùng hai kênh trả lời khác nhau (*"Các thí sinh trả lời bằng máy tính"* ở VCNV và Tăng tốc; Khởi động cùng Về đích trả lời miệng), nhưng không rule nào xác định câu nào thuộc kênh nào. Điều này quyết định thí sinh có ô nhập hay không và luật ghi nhận đáp án nào áp dụng.
- **Câu hỏi**: Kênh trả lời (miệng hay gõ máy) được xác định theo vòng thi, theo từng câu hỏi, hay theo cấu hình trận?

### GRR-070 · MISSING · Trung bình · `[HOÃN v1.5 — C-14: v1 luôn có người điều khiển]`
- **Vị trí**: `game-rules-inventory.md` §R-GEN-03, §U-38
- **Trích**: *"practice gặp câu miệng (Khởi động, Về đích) mà không có admin — máy không chấm được. Practice solo có bị giới hạn chỉ dùng câu gõ không? Không nguồn nào nói."*
- **Vấn đề**: ở chế độ practice không có admin, các vòng dùng đáp án miệng không có cơ chế chấm nào được định nghĩa.
- **Câu hỏi**: Ở chế độ practice, các vòng có câu trả lời miệng được xử lý ra sao?

### GRR-071 · MISSING · Cao · `[NẶNG HƠN — Đ-5.3: điểm = event log ⇒ bấm Đúng 2 lần có nguy cơ cộng dồn]`
- **Vị trí**: `game-rules-inventory.md` §R-GEN-03, §R-GEN-07
- **Trích**: *"Điểm chỉ chốt khi ADMIN bấm Đúng/Sai (hotkey C/X)"* · *"undo chỉ event chấm điểm GẦN NHẤT chưa có event khác build-upon"*
- **Vấn đề**: không rule nào nói kết quả khi admin bấm **lặp lại** cùng phán quyết (Đúng hai lần) hay bấm phán quyết **ngược lại** (Đúng rồi Sai) trên cùng một submission: lần sau bị bỏ qua, ghi đè, hay cộng dồn. Đây là idempotency của thao tác chấm, thao tác quan trọng nhất của trận.
- **Câu hỏi**: Bấm Đúng hai lần liên tiếp trên cùng một đáp án, và bấm Sai sau khi đã bấm Đúng, cho kết quả điểm như thế nào?

### GRR-072 · MISSING · Trung bình · `[ĐÓNG — Đ-5.3: revert một event transfer đảo cả hai vế]`
- **Vị trí**: `game-rules-inventory.md` §R-GEN-07, §U-15
- **Trích**: *"undo một chấm điểm steal transfer phải hoàn nguyên 2 seat; nguồn không nói reducer xử lý thế nào (U-15)"*
- **Vấn đề**: thao tác hoàn tác trên một event ảnh hưởng hai thí sinh chưa được định nghĩa về mặt luật (hoàn cả hai hay chỉ một, và nếu người kia đã có event sau đó thì sao).
- **Câu hỏi**: Hoàn tác một lần chấm cướp quyền có hoàn nguyên điểm cho cả hai thí sinh không, kể cả khi một trong hai đã có thay đổi điểm sau đó?

### GRR-073 · AMBIGUOUS · Trung bình · `[PHỤ THUỘC Đ-5.3.X — có thể biến mất nếu revert thay thế undo]`
- **Vị trí**: `game-rules-inventory.md` §R-GEN-07
- **Trích**: *"undo chỉ event chấm điểm GẦN NHẤT chưa có event khác build-upon"* · *"Chưa định nghĩa: undo có giới hạn số lần không."*
- **Vấn đề**: cụm build-upon không có định nghĩa nghiệp vụ (event nào được coi là dựa trên event nào: cùng thí sinh, cùng câu, hay cùng vòng). Số lần undo cũng không giới hạn.
- **Câu hỏi**: Một event được coi là build-upon event khác theo tiêu chí nghiệp vụ nào, và có giới hạn số lần hoàn tác trong một trận không?

### GRR-074 · AMBIGUOUS · Trung bình · `[ĐÓNG — Đ-1]`
- **Vị trí**: `fandom-olympia-26-luat-choi.md` §Tăng tốc
- **Trích**: *"Đôi khi câu trả lời có ý nghĩa tương đồng với đáp án của chương trình cũng được chấp nhận."*
- **Vấn đề**: ngoại lệ có tính tuỳ nghi (đôi khi) nhưng đứng ngay cạnh quy tắc tuyệt đối (*"bất kỳ sai sót… sẽ không được công nhận"*) trong cùng đoạn — hai rule cùng match một đáp án gần đúng và cho kết quả trái ngược, không có thứ tự ưu tiên.
- **Câu hỏi**: Khi một đáp án vừa sai chính tả vừa có ý nghĩa tương đồng, rule nào được áp trước?

---

## G. TIMER / TRẠNG THÁI / KẾT NỐI

### GRR-075 · CONFLICT · Cao
- **Vị trí**: `game-rules-inventory.md` §R-GEN-02 vs §R-KD-01, §R-VCNV-01, §R-TT-01, §R-VD-03
- **Trích**: R-GEN-02: *"timeSeconds là metadata TỪNG CÂU HỎI… cùng 20đ có thể 15s và 40s"*; R-VD-03 ghi *"per-question timeSeconds override thắng"* — nhưng các vòng khác đều nêu thời gian cố định: *"Thời gian suy nghĩ cho mỗi câu hỏi là 3 giây"*, *"Thời gian suy nghĩ cho mỗi từ hàng ngang là 15 giây"*, *"lần lượt là 20, 20, 30 và 30 giây"*.
- **Vấn đề**: quy tắc thời gian theo câu thắng chỉ được phát biểu rõ ở Về đích. Không rule nào nói nó có áp cho Khởi động (3s), VCNV (15s), Tăng tốc (20/20/30/30) hay không. Ở Tăng tốc, thời gian còn gắn với **vị trí câu trong vòng** chứ không với câu, nên hai cơ chế xác định thời gian cùng match.
- **Câu hỏi**: Thời gian khai trên từng câu hỏi có ghi đè thời gian cố định của Khởi động, VCNV và Tăng tốc không; ở Tăng tốc thì thời gian theo vị trí câu hay theo câu?

### GRR-076 · MISSING · Cao
- **Vị trí**: `game-rules-inventory.md` §R-GEN-08, §Trạng thái game
- **Trích**: §Trạng thái game (bản 2026-07-24) — danh sách trạng thái trận không kèm điều kiện chuyển.
- **Vấn đề**: không rule nào nói các sự kiện của thí sinh (bấm chuông, gửi đáp án) **đến sau khi trận đã `FINISHED`** được xử lý thế nào: loại bỏ, hay ghi nhận với timestamp lúc gửi. Với luật xếp hạng theo ms, hai cách cho kết quả khác nhau.
- **Câu hỏi**: Sự kiện bấm chuông hoặc gửi đáp án đến sau khi trận đã `FINISHED` được loại bỏ hay được ghi nhận, và timestamp nào được dùng?

### GRR-077 · MISSING · Trung bình
- **Vị trí**: `game-rules-inventory.md` §Trạng thái game dùng trong tài liệu này
- **Trích**: §Trạng thái game (bản 2026-07-24) — enum trạng thái trận.
- **Vấn đề**: danh sách trạng thái không kèm điều kiện chuyển. **Trạng thái nghỉ giữa các vòng** không có rule nào nói khi nào vào, khi nào ra, ai kích hoạt. Không có trạng thái nào cho trận bị **huỷ hoặc bỏ dở**, nên một trận đã bắt đầu mà không thể kết thúc không có đường thoát được đặc tả.
- **Câu hỏi**: Điều kiện vào và ra của **trạng thái nghỉ giữa các vòng** là gì, và một trận đã bắt đầu nhưng không thể hoàn thành được kết thúc bằng trạng thái nào?

### GRR-078 · CONFLICT · Thấp
- **Vị trí**: `game-rules-inventory.md` §Trạng thái game dùng trong tài liệu này
- **Trích**: *"Demo dùng chuỗi phẳng: CHO / KHOI_DONG_RIENG / KHOI_DONG_CHUNG / VCNV / TANG_TOC / VE_DICH / TIE_BREAK / KET_THUC"* so với mô hình `rounds[i]` theo playlist
- **Vấn đề**: hai mô hình trạng thái cùng tồn tại: một cố định theo tên vòng, một tổng quát theo playlist. Chuỗi phẳng không biểu diễn được playlist có vòng lặp lại hoặc thứ tự khác.
- **Câu hỏi**: Playlist có được phép chứa cùng một loại vòng nhiều lần hoặc theo thứ tự khác thứ tự chuẩn không?

### GRR-079 · MISSING · Trung bình
- **Vị trí**: `game-rules-inventory.md` §R-GEN-08
- **Trích**: §R-GEN-08 (bản 2026-07-24) — đóng băng deadline rồi đặt `endsAt` mới khi chạy tiếp.
- **Vấn đề**: rule chỉ nói tới deadline của câu hỏi. Các cửa sổ thời gian khác (5 giây bấm chuông cướp, 3 giây cửa sổ chuông Khởi động, 15 giây ô trung tâm) có được đóng băng và khôi phục theo cùng cách không thì không nói.
- **Câu hỏi**: Nếu một cửa sổ bấm chuông (5 giây cướp hoặc 3 giây Khởi động) bị gián đoạn, nó được khôi phục phần thời gian còn lại hay bắt đầu lại?

### GRR-080 · MISSING · Trung bình
- **Vị trí**: `game-rules-inventory.md` §R-GEN-08, §U-12
- **Trích**: §R-GEN-08 (bản 2026-07-24) — ngưỡng N giây admin mất kết nối, `U-12`.
- **Vấn đề**: ngưỡng phản ứng khi admin mất kết nối không có giá trị.
- **Câu hỏi**: Admin mất kết nối bao nhiêu giây thì hệ thống phản ứng, và phản ứng đó là gì?

### GRR-081 · CONFLICT · Trung bình
- **Vị trí**: `game-rules-inventory.md` §R-GEN-09, §U-13
- **Trích**: *"Kết quả khi dưới 120s: giữ ghế + state-sync"* · *"DEF D13.4 — rớt đúng lượt riêng thì engine dừng lại chờ admin quyết, ghi đè grace"* · *"danh sách đầy đủ giá trị dropoutPolicy (U-13)"*
- **Vấn đề**: hai rule cùng match tình huống thí sinh mất kết nối đúng lượt riêng (grace 120s, hay dừng lại chờ admin) và cho hai luồng khác nhau; đồng thời nhánh quá grace trỏ tới một tập giá trị chưa được liệt kê.
- **Câu hỏi**: Khi thí sinh mất kết nối đúng lượt riêng, hệ thống áp grace 120 giây hay dừng lại chờ admin, và các lựa chọn xử lý sau khi hết grace gồm những gì?

### GRR-082 · MISSING · Trung bình
- **Vị trí**: `game-rules-inventory.md` §R-GEN-05, §U-14
- **Trích**: *"NTP/chrony yêu cầu ở compose; profile portable Windows LAN chạy 1 máy — không nguồn nào nói xử lý lệch clock ở đó (U-14)"*
- **Vấn đề**: rule nền server time là source of truth duy nhất không có nhánh xử lý khi đồng hồ server bị chỉnh hoặc lệch giữa trận (timestamp lùi về quá khứ), trong khi mọi xếp hạng đều dựa vào nó.
- **Câu hỏi**: Nếu đồng hồ server thay đổi giữa trận khiến timestamp không đơn điệu tăng, kết quả xếp hạng đã ghi nhận được xử lý ra sao?

### GRR-083 · BOUNDARY_UNDEFINED · Trung bình · `[v2]`
- **Vị trí**: `game-rules-inventory.md` §R-TT-01, §R-VD-05, §8B K-11
- **Trích**: *"DEF D1 chốt 1-12 ghế"* vs *"F26 + R26 viết cho đúng 4"*
- **Vấn đề**: biên nhỏ nhất (1 ghế) làm nhiều rule mất nghĩa: lượt chung không có ai để giành quyền, xếp hạng tốc độ chỉ có một bậc, cướp quyền không có người cướp, tie-break không thể xảy ra. Không rule nào nói các vòng này bị chặn ở cấu hình 1 ghế hay vẫn chạy với nhánh rỗng.
- **Câu hỏi**: Với cấu hình 1 hoặc 2 ghế, các vòng phụ thuộc nhiều thí sinh (lượt chung, cướp quyền, xếp hạng tốc độ) có bị chặn ở pre-flight không hay vẫn chạy?

---

## H. KHO ĐỀ / RÚT ĐỀ / NO-REPEAT

### GRR-084 · MISSING · Cao · `[NẶNG HƠN — Đ-5.1: chạy lại vòng làm tiêu pool nhanh hơn nhiều]`
- **Vị trí**: `game-rules-inventory.md` §R-KD-07, §R-GEN-06, §U-9
- **Trích**: *"pool cạn giữa trận do skip nhiều lần (pre-flight chỉ chạy trước start) (U-9)"*
- **Vấn đề**: rule chỉ có nhánh chặn ở pre-flight. Nhánh hết câu giữa trận không có kết quả, nên trận có thể không tiếp tục được và không có đường thoát được đặc tả.
- **Câu hỏi**: Khi pool câu hỏi của một mức hoặc một lĩnh vực cạn giữa trận, vòng đang chạy được xử lý thế nào?

### GRR-085 · AMBIGUOUS · Trung bình
- **Vị trí**: `game-rules-inventory.md` §R-GEN-06, §U-30
- **Trích**: *"câu đã được hỏi trong bất kỳ match/vòng nào của contest thì usedInContest"* · *"đã được hỏi = đã hiện màn hình hay đã chấm; câu skip vì media hỏng có set cờ không (U-30)"*
- **Vấn đề**: điều kiện đánh dấu câu đã dùng có nhiều cách hiểu, ảnh hưởng trực tiếp tới việc câu có thể xuất hiện lại hay không.
- **Câu hỏi**: Một câu hỏi được coi là đã được hỏi tại thời điểm nào, và câu bị bỏ do sự cố có bị đánh dấu đã dùng không?

### GRR-086 · AMBIGUOUS · Trung bình · `[ĐÓNG PHẦN — Đ-5.1: phạm vi là CONTEST]`
- **Vị trí**: `game-rules-inventory.md` §R-KD-07 vs §R-GEN-06
- **Trích**: R-KD-07: *"noRepeatInMatch: true"* · R-GEN-06: *"No-repeat toàn CONTEST… câu không xuất hiện lại trong contest"*
- **Vấn đề**: hai phạm vi không lặp cùng tồn tại (match và contest) mà không nói quan hệ: cờ per-match có ý nghĩa gì khi luật contest đã nghiêm ngặt hơn, và có được tắt luật contest không.
- **Câu hỏi**: Phạm vi không lặp là match hay contest, và hai cấu hình này quan hệ với nhau thế nào?

### GRR-087 · BOUNDARY_UNDEFINED · Thấp
- **Vị trí**: `game-rules-inventory.md` §R-KD-07
- **Trích**: *"thuật toán 2 bước + reservePerField default 2"*
- **Vấn đề**: giá trị dự trữ 2 câu mỗi lĩnh vực không có căn cứ nghiệp vụ được nêu và không nói nó có đủ cho các nhánh tiêu pool ngoài dự kiến (skip, câu hỏng media, câu thay thế) hay không.
- **Câu hỏi**: Con số dự trữ 2 câu mỗi lĩnh vực dựa trên tình huống tiêu hao nào?

### GRR-088 · MISSING · Thấp
- **Vị trí**: `game-rules-inventory.md` §R-GEN-12, §U-29
- **Trích**: *"everPublic một chiều vĩnh viễn — không có cách gỡ (U-29)"*
- **Vấn đề**: không có nhánh hoàn tác cho một cờ ảnh hưởng vĩnh viễn tới khả năng dùng câu hỏi; trường hợp gán nhầm không có đường xử lý.
- **Câu hỏi**: Câu hỏi bị đánh dấu everPublic do nhầm lẫn có được khôi phục không, và ai có quyền?

### GRR-089 · CONFLICT · Trung bình
- **Vị trí**: `game-rules-inventory.md` §R-GEN-12, §8B K-10
- **Trích**: *"DEF D16 định nghĩa Question.visibility là cột set tay; SPEC §9 định nghĩa lại là derived, read-only."*
- **Vấn đề**: cùng một thuộc tính có hai định nghĩa nghiệp vụ loại trừ nhau (do người đặt, hay suy ra từ lịch sử), chưa phân xử.
- **Câu hỏi**: visibility của câu hỏi do người quản lý đặt hay được suy ra từ lịch sử sử dụng?

### GRR-090 · MISSING · Trung bình
- **Vị trí**: `game-rules-inventory.md` §R-GEN-11, §U-11
- **Trích**: *"Kết quả khi SW lỗi: tự hạ cấp reveal-only + log; trận không đứng."* · *"câu thay thế cũng hỏng media (U-11)"*
- **Vấn đề**: nhánh đệ quy (câu thay thế cũng hỏng) không có điểm dừng được định nghĩa.
- **Câu hỏi**: Nếu câu thay thế cũng không phát được media, vòng thi tiếp tục thế nào và điểm của câu đó xử lý ra sao?

### GRR-091 · MISSING · Thấp
- **Vị trí**: `game-rules-inventory.md` §U-21
- **Trích**: *"Câu phụ rút từ pool KV | DEF D24 (đề xuất của Claude, đổi được)"*
- **Vấn đề**: nguồn câu hỏi cho phần tie-break chưa được chốt, trong khi tie-break là phần quyết định người thắng.
- **Câu hỏi**: Câu hỏi phụ được lấy từ pool nào và có phải gán trước khi start như các vòng khác không?

### GRR-092 · CONFLICT · Trung bình
- **Vị trí**: `game-rules-inventory.md` §R-GEN-10 vs §8A K-4
- **Trích**: §R-GEN-10: *"PRD NFR-4 nói contest bật; SPEC §11/§13 + CLAUDE.md nói per-MATCH, không phải per-contest — chưa phân xử"* vs §8A K-4 ghi đã chốt per-MATCH và liệt PRD là *"wording lạc hậu duy nhất, PHẢI SỬA"*.
- **Vấn đề**: cùng một tài liệu vừa ghi chưa phân xử vừa ghi đã chốt cho cùng một vấn đề — hai phát biểu về trạng thái quyết định mâu thuẫn nhau.
- **Câu hỏi**: Phạm vi của revealAnswerAfterJudge đã được chốt là per-match chưa, và mục nào trong tài liệu là bản có hiệu lực?

---

## I. THI ĐỘI (v2)

### GRR-093 · CONFLICT · Trung bình
- **Vị trí**: `game-rules-inventory.md` §PHẦN 7 R-TEAM-01 vs §R-KD-03
- **Trích**: R-TEAM-01: *"Khoá CÁ NHÂN; thành viên khác vẫn bấm được (teamLockout: false)"* vs R-KD-03: *"câu đó KHÔNG mở lại chuông cho người khác"*
- **Vấn đề**: hai rule cùng match tình huống bấm sai ở lượt chung khi thi đội và cho kết quả ngược nhau (câu đóng, hay đồng đội vẫn bấm được).
- **Câu hỏi**: Ở thi đội, sau khi một thành viên bấm sai ở lượt chung, câu hỏi có còn mở cho thành viên khác không?

### GRR-094 · MISSING · Trung bình
- **Vị trí**: `game-rules-inventory.md` §PHẦN 7 R-TEAM-02
- **Trích**: *"Đáp án đội = bản CUỐI của bất kỳ thành viên; ranking theo ts bản cuối"* · *"Chưa định nghĩa: bản sau sai đè bản trước đúng"*
- **Vấn đề**: nhánh thành viên gửi sau ghi đè đáp án đúng của đồng đội không có kết quả được định nghĩa.
- **Câu hỏi**: Ở Tăng tốc thi đội, đáp án gửi sau của một thành viên có ghi đè đáp án trước của đồng đội trong mọi trường hợp không?

### GRR-095 · MISSING · Trung bình
- **Vị trí**: `game-rules-inventory.md` §PHẦN 7 R-TEAM-05
- **Trích**: *"Chỉ đơn vị điểm khác được bấm | Server reject (hard-code)"*
- **Vấn đề**: rule nói thao tác bị từ chối nhưng không nói dữ liệu có thay đổi không: lần bấm bị reject có được ghi log, có tiêu cửa sổ 5 giây, có bị phạt không.
- **Câu hỏi**: Lần bấm chuông bị từ chối vì cùng đội có ghi nhận vào lịch sử trận và có ảnh hưởng tới cửa sổ cướp đang chạy không?

### GRR-096 · MISSING · Thấp
- **Vị trí**: `game-rules-inventory.md` §PHẦN 7 R-TEAM-06
- **Trích**: *"Mỗi đội cử 1 người bấm chuông"* · *"Chưa định nghĩa: đội trưởng không chỉ định kịp"*
- **Vấn đề**: nhánh không có người đại diện khi vào tie-break không có kết quả.
- **Câu hỏi**: Nếu đội không cử được người bấm chuông trước khi tie-break bắt đầu thì xử lý thế nào?

### GRR-097 · CONFLICT · Thấp
- **Vị trí**: `game-rules-inventory.md` §PHẦN 7 R-TEAM-03 vs §R-VCNV-03
- **Trích**: R-TEAM-03: *"Loại cả đội"* vs R-VCNV-03: *"thí sinh ở vị trí tiếp theo sẽ được lựa chọn"*
- **Vấn đề**: khi cả đội bị loại, luật dồn lượt chọn hàng ngang (viết theo cá nhân) không nói lượt được dồn theo cá nhân hay theo đội.
- **Câu hỏi**: Khi một đội bị loại khỏi VCNV, các lượt chọn hàng ngang còn lại của đội đó được xử lý thế nào?

### GRR-098 · MISSING · Thấp
- **Vị trí**: `game-rules-inventory.md` §PHẦN 7 R-TEAM-07
- **Trích**: *"Đổi đại diện chỉ tại cửa vào vòng"*
- **Vấn đề**: rule phụ thuộc vào sự tồn tại của một **trạng thái nghỉ giữa các vòng**, nhưng điều kiện vào trạng thái đó chưa được định nghĩa (GRR-077). Nếu playlist không có giai đoạn nghỉ thì rule không có đường thực thi.
- **Câu hỏi**: Nếu playlist không có giai đoạn nghỉ giữa hai vòng, đội có được đổi người đại diện không?

---

## J. META — TÀI LIỆU VÀ NGUỒN

### GRR-099 · CONFLICT · Trung bình
- **Vị trí**: `game-rules-inventory.md` §Bảng viết tắt nguồn vs §R-TT-04, §R-VD-04
- **Trích**: *"mọi tham chiếu W26 bên dưới là biến thể ĐÃ BỊ LOẠI… Không dùng W26 làm căn cứ cho bất kỳ requirement nào."* nhưng R-TT-04 vẫn ghi *"W26 chi tiết: câu 1 Nhìn-Đáp (20s)… câu 4 dữ kiện (30s, video clip, dữ kiện đưa ra theo độ khó giảm dần)"* và R-VD-04 ghi actor *"MC/ban cố vấn/khách mời (giới thiệu dụng cụ — W26)"*.
- **Vấn đề**: nguồn đã bị loại vẫn đang cung cấp nội dung mô tả cho một số rule, khiến không rõ phần nào của rule là có hiệu lực.
- **Câu hỏi**: Các chi tiết chỉ có ở W26 (mô tả loại câu Tăng tốc, vai trò giới thiệu dụng cụ) có hiệu lực hay bị loại cùng với nguồn?

### GRR-100 · AMBIGUOUS · Thấp
- **Vị trí**: `game-rules-inventory.md` §R-GEN-03 — hai khối "Hệ quả bắt buộc khi viết rule tiếp theo" liên tiếp
- **Trích**: hai khối gần như trùng nhau, cùng bắt đầu *"Mọi rule khi mô tả Kết quả khi đúng / khi sai phải hiểu là kết quả SAU KHI admin phán quyết"*
- **Vấn đề**: cùng một quy tắc nền được phát biểu hai lần với chữ khác nhau chút ít trong cùng một section; không rõ có phải hai quy tắc khác nhau hay là lặp do biên tập.
- **Câu hỏi**: Hai khối Hệ quả bắt buộc là một quy tắc bị lặp hay hai quy tắc khác nhau?

### GRR-101 · AMBIGUOUS · Thấp
- **Vị trí**: `docs/source/fandom-olympia-26-luat-choi.md` header
- **Trích**: *"Nguồn này mâu thuẫn với Wikipedia ở nhiều điểm — xem docs/source/wikipedia-olympia-26-luat-choi.md"*
- **Vấn đề**: file được trỏ tới đã bị xoá theo `game-rules-inventory.md` (*"Snapshot đã tạo ngày 2026-07-23 và đã xoá"*), nên đây là tham chiếu treo ngay trong tài liệu nguồn.
- **Câu hỏi**: Tham chiếu tới snapshot Wikipedia đã xoá có cần giữ trong header nguồn không?

### GRR-102 · MISSING · Thấp
- **Vị trí**: `game-rules-inventory.md` §R-GEN-01, §U-33
- **Trích**: *"nút chuông nhận click chuột… Không gán hotkey cho chuông."* · *"thí sinh không dùng được chuột thi thế nào (U-33)"*
- **Vấn đề**: không có nhánh thay thế cho thí sinh không thao tác được chuột; rule tuyệt đối, không có ngoại lệ được định nghĩa.
- **Câu hỏi**: Có cơ chế nào cho thí sinh không sử dụng được chuột tham gia các vòng có chuông không?

### GRR-103 · MISSING · Trung bình
- **Vị trí**: `game-rules-inventory.md` §X-1
- **Trích**: *"Fandom là wiki cộng đồng, sửa được bất kỳ lúc nào, không versioning. DEF D8 không có cơ chế phát hiện thay đổi."*
- **Vấn đề**: nguồn luật duy nhất có thể thay đổi mà không ai biết; không rule nào định nghĩa quy trình khi nguồn đổi (giữ snapshot, cập nhật preset, hiệu lực với contest đang chạy).
- **Câu hỏi**: Khi nguồn luật gốc thay đổi sau ngày snapshot, quy trình cập nhật preset và hiệu lực đối với contest đang diễn ra là gì?

### GRR-104 · MISSING · Trung bình
- **Vị trí**: `game-rules-inventory.md` §Lưu ý toàn cục
- **Trích**: *"mọi giá trị dưới đây là DEFAULT của preset O26_DEFAULT@1, admin config được per-contest… Không giá trị nào hard-code trong engine."*
- **Vấn đề**: không rule nào giới hạn miền giá trị hợp lệ khi admin sửa preset (số câu bằng 0, thời gian bằng 0 giây, điểm âm, thang điểm rỗng), cũng không nói sửa preset giữa contest có hiệu lực với các trận đã chạy không.
- **Câu hỏi**: Miền giá trị hợp lệ của các tham số luật khi admin tự cấu hình là gì, và thay đổi cấu hình giữa contest có áp cho các trận đã diễn ra không?

### GRR-105 · BOUNDARY_UNDEFINED · Trung bình · `[v2]`
- **Vị trí**: `fandom-olympia-26-luat-choi.md` §Về đích (Thứ tự tham gia); `game-rules-inventory.md` §R-VD-02
- **Trích**: *"Lượt 3: Thí sinh có điểm số cao hơn trong 2 thí sinh còn lại… Lượt 4: Thí sinh cuối cùng chưa bắt đầu phần thi."*
- **Vấn đề**: danh sách thứ tự viết cứng cho đúng 4 lượt, trong đó lượt 3 dùng cách diễn đạt chỉ hợp lệ khi còn đúng 2 người. Với số ghế khác 4 không có quy tắc tổng quát nào được phát biểu.
- **Câu hỏi**: Với số thí sinh khác 4, quy tắc xác định thứ tự lượt Về đích được phát biểu tổng quát thế nào?

### GRR-106 · BOUNDARY_UNDEFINED · Trung bình
- **Vị trí**: `game-rules-inventory.md` §R-TB-01, §R-GEN-07
- **Trích**: *"Kết quả khi có hoà: chuyển TIE_BREAK. Khi không hoà: chuyển FINISHED."*
- **Vấn đề**: điều kiện hoà không nói rõ so sánh trên giá trị điểm sau khi đã áp mọi điều chỉnh (bao gồm `SCORE_ADJUST` của admin sau vòng cuối), và không nói mốc thời gian nào chốt điểm để xét hoà; nếu admin sửa điểm sau khi đã vào `TIE_BREAK` thì trạng thái có quay lại được không.
- **Câu hỏi**: Điểm được chốt tại mốc nào để xét điều kiện hoà, và nếu admin điều chỉnh điểm sau khi đã vào `TIE_BREAK` thì trạng thái được xử lý ra sao?

---

## 2B. Vấn đề phát hiện ở LƯỢT 2 (rà soát 2026-07-24, sau các quyết định trong `game-rules-decisions.md`)

> **Phạm vi lượt 2**: chỉ những khiếm khuyết **chưa có** trong GRR-001 → GRR-106. Nguồn đọc lại đầy đủ: `docs/game-rules-inventory.md` (PHẦN 0 → PHẦN 9), `docs/source/fandom-olympia-26-luat-choi.md`, `CLAUDE.md`, và toàn bộ toàn bộ khối quyết định (nay ở `game-rules-decisions.md`).
>
> **Nguyên tắc giữ nguyên**: chỉ NÊU vấn đề, KHÔNG phân xử, KHÔNG phát minh luật, KHÔNG đề xuất schema/kiến trúc. Mỗi mục kết thúc bằng câu hỏi cho chủ dự án.
>
> Vấn đề thuộc **điều khiển hệ thống** (quyền admin, UI, audit, phân quyền, quy trình vận hành) **không** ghi ở đây — chuyển sang `docs/product-discovery.md`.

### 2B.0 Bảng đếm

**Theo nhãn**

| Nhãn | Số lượng |
|---|---|
| MISSING | 16 |
| CONFLICT | 10 |
| ORDER_DEPENDENT | 2 |
| AMBIGUOUS | 1 |
| BOUNDARY_UNDEFINED | 1 |
| UNREACHABLE | 0 |
| **Tổng** | **30** |

**Theo mức độ**

| Mức độ | Số lượng |
|---|---|
| Cao | 14 |
| Trung bình | 13 |
| Thấp | 3 |

**Theo nhóm nguồn**

| Nhóm | Mô tả | ID | Số lượng |
|---|---|---|---|
| **A** | Bị bỏ sót ở lượt 1 (phần inventory chưa soi kỹ) | GRR-107 → GRR-113 | 7 |
| **B** | **MỚI SINH RA từ chính các quyết định trong `game-rules-decisions.md`** | GRR-114 → GRR-131 | 18 |
| **C** | Mâu thuẫn quyết định ↔ quyết định, hoặc quyết định ↔ `CLAUDE.md` | GRR-132 → GRR-136 | 5 |

---

### Nhóm A — Bỏ sót ở lượt 1

### GRR-107 · MISSING · Cao
- **Vị trí**: `docs/source/fandom-olympia-26-luat-choi.md` §Vượt chướng ngại vật; `docs/game-rules-inventory.md` §R-VCNV-05
- **Trích**: *"Các thí sinh sẽ có 15 giây suy nghĩ để đưa ra Chướng ngại vật. Trả lời đúng Chướng ngại vật sau gợi ý cuối cùng ở ô trung tâm chỉ được 20 điểm."*
- **Vấn đề**: **cơ chế trả lời** trong cửa sổ 15 giây cuối không được định nghĩa. Toàn vòng trước đó, giải Chướng ngại vật là **bấm chuông giành quyền** (*"bấm chuông trả lời Chướng ngại vật bất cứ lúc nào"* — một người, sai thì bị loại); còn hàng ngang là **mọi người cùng trả lời**. Cụm *"Các thí sinh"* (số nhiều) khớp với thể thức đồng loạt, nhưng mốc 20 điểm lại thuộc thang giải-CNV-bằng-chuông. Hai thể thức cho kết quả khác nhau: một người được 20 điểm, hay mọi người đúng đều được 20 điểm; và hình phạt "sai thì bị loại" có còn áp không. GRR-019/GRR-021/GRR-024/GRR-025 không hỏi về **cơ chế** này — GRR-025 chỉ hỏi mốc điểm khi câu ô trung tâm sai và cách kết thúc vòng.
- **Câu hỏi**: Trong 15 giây sau gợi ý cuối cùng, thí sinh giải Chướng ngại vật bằng cách bấm chuông giành quyền (một người) hay tất cả cùng trả lời, và luật "sai thì bị loại" có còn áp trong cửa sổ này không?

### GRR-108 · MISSING · Cao
- **Vị trí**: `fandom-olympia-26-luat-choi.md` §Câu hỏi phụ; `game-rules-inventory.md` §R-TB-02, §R-TB-03
- **Trích**: *"nếu có thí sinh bấm chuông trả lời trước khi có hiệu lệnh của người dẫn chương trình, thí sinh đó sẽ bị mất quyền trả lời câu hỏi."* · R-TB-02: *"Kết quả khi sai: cả nhóm sang câu TIẾP THEO — không mở lại cùng câu."*
- **Vấn đề**: "mất quyền" **không phải** "trả lời sai", nên nhánh chuyển sang câu tiếp của R-TB-02 không match. Khi một thí sinh mất quyền do bấm sớm, không rule nào nói **các thí sinh còn lại có tiếp tục thi câu đó không**, hay câu bị bỏ. GRR-059 hỏi về người **trả lời sai** ở các câu sau; GRR-061 hỏi trường hợp **tất cả** cùng mất quyền. Nhánh "một người mất quyền, những người khác vẫn còn quyền, trong CÙNG một câu" chưa có mục nào.
- **Câu hỏi**: Khi một thí sinh mất quyền vì bấm trước hiệu lệnh, câu hỏi đó vẫn tiếp tục cho các thí sinh còn lại hay bị bỏ sang câu tiếp?

### GRR-109 · MISSING · Trung bình · *(biến thể của GRR-012)*
- **Vị trí**: `game-rules-inventory.md` §R-KD-02
- **Trích**: *"1 thí sinh trả lời **liên tục** trong `perSeatSeconds` (30-300)."* · *"Kết quả khi đúng: `+pointsCorrect` mỗi câu."*
- **Vấn đề**: GRR-012 hỏi về **kết thúc lượt** (hết giờ mà còn câu / hết câu mà còn giờ). Chưa có mục nào hỏi về **câu đang dở tại mốc hết quỹ giờ**: câu đã hiện, thí sinh đã trả lời nhưng admin chưa chấm khi đồng hồ về 0 — câu đó có được chấm và tính điểm không. Ngoài ra thể thức này không nêu giới hạn thời gian **từng câu**, nên không có mốc nào xác định một câu đã kết thúc; khác hẳn thể thức 6 câu × 3 giây.
- **Câu hỏi**: Ở thể thức quỹ thời gian, câu đang trả lời dở tại mốc hết giờ có được chấm không, và mỗi câu trong quỹ thời gian có giới hạn riêng không?

### GRR-110 · MISSING · Trung bình · *(biến thể của GRR-081)*
- **Vị trí**: `game-rules-inventory.md` §R-GEN-09, §R-VD-05, §R-TB-02
- **Trích**: R-GEN-09: *"Kết quả khi < 120s: giữ ghế + state-sync"* · `DEF` D13.4: *"rớt đúng lượt riêng thì engine dừng lại chờ admin quyết"*
- **Vấn đề**: GRR-081 chỉ xét mất kết nối **đúng lượt riêng**. Chưa có mục nào xét mất kết nối trong các **cửa sổ ngắn có tính tranh chấp**: cửa sổ cướp 5 giây ở Về đích, cửa sổ chuông 3 giây ở Khởi động lượt chung, câu hỏi phụ 15 giây. Grace 120 giây dài hơn toàn bộ các cửa sổ này, nên nhánh "giữ ghế + state-sync" không cho kết quả nào về việc cửa sổ đó được đóng băng, chạy tiếp, hay chạy tiếp mà bỏ qua người mất kết nối.
- **Câu hỏi**: Khi một thí sinh mất kết nối trong lúc cửa sổ cướp 5 giây hoặc câu hỏi phụ đang chạy, cửa sổ đó được đóng băng, chạy tiếp, hay chạy tiếp mà loại người đó?

### GRR-111 · MISSING · Trung bình
- **Vị trí**: `CLAUDE.md` §Luật chơi & đề thi (D23); `game-rules-inventory.md` §R-GEN-06, §R-GEN-12
- **Trích**: `CLAUDE.md`: *"Contest config import/export trọn gói (D23): ZIP = Excel câu hỏi… use-case soạn trên bản Internet → import vào portable."* · R-GEN-06: *"câu đã được hỏi trong bất kỳ match/vòng nào của contest thì `usedInContest`"* · R-GEN-12: *"Kết quả khi có câu `everPublic=true`: pre-flight hard-block mọi match"*
- **Vấn đề**: hai luật quan trọng gắn với **trạng thái lịch sử của câu hỏi** (`usedInContest`, `everPublic`), nhưng không rule nào nói trạng thái đó có đi theo câu qua đường **export → import** hay không. Nếu không đi theo, mọi câu import đều là "chưa dùng" và "chưa public" — luật no-repeat toàn contest và hàng rào chống rò đề bị vô hiệu bằng một thao tác hợp lệ. Nếu có đi theo, chưa nói trạng thái đó tính theo contest nguồn hay contest đích. GRR-085/GRR-086 chỉ bàn phạm vi và mốc đánh dấu **trong** một contest, không bàn đường import.
- **Câu hỏi**: Câu hỏi import từ gói ZIP có mang theo trạng thái `usedInContest` và `everPublic` của contest nguồn không, và luật no-repeat áp cho contest đích tính từ mốc nào?

### GRR-112 · MISSING · Trung bình
- **Vị trí**: `game-rules-inventory.md` §R-GEN-10
- **Trích**: *"Ngoại lệ: `revealAnswerAfterJudge` bật → đẩy xuống 3 kênh kia **SAU khi chấm**."*
- **Vấn đề**: rule chỉ có nhánh "sau khi chấm". Với các câu **không bao giờ được chấm** — câu bị bỏ qua vì không ai bấm chuông (`F26`: *"câu hỏi đó sẽ bị bỏ qua"*), câu bị admin skip vì sự cố, hàng ngang không ai trả lời đúng, câu thuộc vòng bị bỏ theo Đ-5.1 — không có sự kiện "chấm" nào để kích hoạt, nên đáp án không bao giờ được đẩy. Không rule nào nói đây là chủ đích hay là nhánh thiếu; trong khi đó câu vẫn bị tính là đã tiêu theo Đ-5.2f.
- **Câu hỏi**: Với câu không bao giờ được admin chấm (bỏ qua, skip, vòng bị bỏ), đáp án có được đẩy xuống thí sinh/viewer khi `revealAnswerAfterJudge` bật không, và tại sự kiện nào?

### GRR-113 · MISSING · Thấp
- **Vị trí**: `game-rules-inventory.md` §R-TT-05
- **Trích**: *"Kết quả khi sai: `wrongLocksOut: true` → khoá người bấm sai."*
- **Vấn đề**: phạm vi và thời hạn của "khoá" không được định nghĩa: khoá trong phạm vi **câu đang hỏi**, phạm vi **vòng**, hay tới hết trận; và có sự kiện nào giải khoá không. Cùng tài liệu đang dùng ba cụm khác nhau cho ba hình phạt khác nhau ("bị loại khỏi phần thi này" ở VCNV, "mất quyền trả lời câu hỏi" ở tie-break, "khoá" ở clue-buzz) mà không quy về một thang phạm vi chung.
- **Câu hỏi**: `wrongLocksOut` khoá thí sinh trong phạm vi câu hỏi, vòng, hay cả trận, và có sự kiện nào giải khoá không?

---

### Nhóm B — Vấn đề MỚI SINH RA từ các quyết định trong `game-rules-decisions.md`

### GRR-114 · CONFLICT · Cao
- **Vị trí**: `game-rules-decisions.md` Đ-5.3 (mô hình revert) vs `game-rules-inventory.md` §R-TT-01, §R-TT-02
- **Trích**: Đ-5.3: *"Điểm là các event log… Revert = thêm event đảo ngược, không phải xoá event cũ."* và *"Revert được **event ở giữa** lịch sử; các event sau **giữ nguyên**."* vs R-TT-01: *"40/30/20/10 theo thứ hạng tốc độ **trong số người đúng**"*
- **Vấn đề**: mô hình Đ-5.3 giả định mỗi event điểm là một **delta độc lập** đảo ngược được bằng một event đối xứng. Ở Tăng tốc điều đó không đúng: điểm mỗi thí sinh là **hàm của TẬP người được chấm đúng**. Revert một event chấm Đúng của thí sinh A không chỉ trả lại điểm của A — nó **đẩy thứ hạng của B, C, D lên một bậc**, tức phải sinh thêm event điểm cho những người **không liên quan tới thao tác revert**, trái với vế "các event sau giữ nguyên". Khác GRR-039 (hỏi về **thứ tự admin chấm**), mục này hỏi về **ngữ nghĩa của revert** vừa được chốt.
- **Câu hỏi**: Khi revert một event chấm Đúng ở Tăng tốc, các thí sinh khác được lên bậc điểm có được sinh event điều chỉnh kèm theo không, hay điểm của họ giữ nguyên như đã ghi?

### GRR-115 · ORDER_DEPENDENT · Cao
- **Vị trí**: `game-rules-decisions.md` Đ-5.2, Đ-5.3 vs `fandom-olympia-26-luat-choi.md` §VCNV; `game-rules-inventory.md` §R-VCNV-04
- **Trích**: `F26`: *"Trả lời đúng Chướng ngại vật trong 1 từ hàng ngang đầu tiên được 60 điểm, trong 2 từ hàng ngang được 50 điểm…"* · Đ-5.3: *"Revert được event ở giữa lịch sử; các event sau giữ nguyên."*
- **Vấn đề**: điểm Chướng ngại vật **phụ thuộc số hàng ngang đã mở tại thời điểm bấm**, tức phụ thuộc các event **trước đó**. Nếu admin revert event mở hàng ngang thứ 2 (do chấm nhầm), event giải CNV xảy ra **sau** đó lẽ ra phải ở mốc 60 thay vì 50 — nhưng Đ-5.3 nói event sau giữ nguyên. Hai phát biểu cùng match một chuỗi thao tác và cho hai giá trị điểm. Cùng họ với GRR-114 nhưng cơ chế phụ thuộc khác: ở đây là **trạng thái bàn cờ**, không phải bảng xếp hạng.
- **Câu hỏi**: Khi revert một event làm thay đổi số hàng ngang đã mở, mốc điểm của các event giải Chướng ngại vật xảy ra sau đó có được tính lại không?

### GRR-116 · ORDER_DEPENDENT · Cao
- **Vị trí**: `game-rules-decisions.md` Đ-5, Đ-5.3 vs `fandom-olympia-26-luat-choi.md` §Về đích
- **Trích**: `F26`: *"Điểm số được tính tại thời điểm sau khi thí sinh ở lượt 1 hoàn thành phần thi."* · Đ-5.3: *"Điểm là hàm của event log."*
- **Vấn đề**: thứ tự lượt Về đích được tính lại **sau mỗi lượt** dựa trên điểm tại mốc đó. Nếu admin revert một event điểm thuộc **Khởi động hoặc Tăng tốc** trong lúc Về đích đã chạy 2 lượt, điểm tại mốc xếp lượt 1 và lượt 2 thay đổi ⇒ **thứ tự lượt đã chạy trở thành sai theo luật**. Không quyết định nào nói kết quả: giữ nguyên thứ tự đã chạy, sinh cảnh báo, hay tính lại recommendation trên lịch sử đã sửa. Đ-5 chỉ nói hệ thống cảnh báo, mà danh sách conflict được cảnh báo còn treo (Đ-5.b).
- **Câu hỏi**: Khi một event điểm ở vòng trước bị revert giữa vòng Về đích, các lượt đã chạy có bị coi là sai thứ tự không, và recommendation cho lượt kế tiếp tính trên điểm trước hay sau revert?

### GRR-117 · MISSING · Cao
- **Vị trí**: `game-rules-decisions.md` Đ-5.1, Đ-5.2, Đ-5.3
- **Trích**: Đ-5.2: *"Bỏ vòng thì **điểm** vòng đó sẽ reset về trước đó."* · Đ-5.3: *"**Điểm** là các event log. Reset thực hiện REVERT **điểm**."*
- **Vấn đề**: cả ba quyết định chỉ định nghĩa hoàn nguyên cho **ĐIỂM**. Một vòng bị bỏ hoặc chạy lại còn để lại các **trạng thái luật phi-điểm** chưa có quy tắc nào: **lượt Ngôi sao hy vọng đã tiêu** (1 lần/thí sinh/trận, không đặt lại được); **trạng thái "bị loại khỏi phần thi này"** ở VCNV; **miếng ghép đã mở / Chướng ngại vật đã công bố**; **gói câu 3 mức đã chọn** ở Về đích; **lượt chọn hàng ngang đã dùng**. Riêng với đề, Đ-5.2f đã chốt **không** hoàn nguyên — tiền lệ tồn tại cho một loại trạng thái phi-điểm nhưng không được phát biểu tổng quát. Khác GRR-055.b (hỏi ở cấp **một câu bị bỏ**), mục này ở cấp **vòng bị bỏ / chạy lại**.
- **Câu hỏi**: Khi bỏ hoặc chạy lại một vòng, các trạng thái phi-điểm (NSHV đã dùng, thí sinh bị loại khỏi VCNV, miếng ghép đã mở, gói câu đã chọn, lượt chọn đã dùng) có được hoàn nguyên không?

### GRR-118 · MISSING · Trung bình
- **Vị trí**: `game-rules-decisions.md` Đ-5.1, Đ-5.2f vs `game-rules-inventory.md` §R-KD-07, §R-GEN-06
- **Trích**: Đ-5.2f: *"câu hỏi đã dùng KHÔNG được trả lại pool… đề đã lộ thì không tái sử dụng được."* · R-KD-07: *"server rút câu TRONG danh sách đã gán (snapshot)… emit `QUESTIONS_DRAWN`"*, trạng thái *"`LOBBY` (pre-flight) + **đầu mỗi turn**"*
- **Vấn đề**: việc **rút đề** xảy ra ở đầu mỗi turn, trước khi câu được hỏi. Khi một vòng bị bỏ ngay sau khi rút mà **chưa hỏi câu nào**, các câu đó ở trạng thái **đã rút nhưng chưa lộ**. Lý lẽ của Đ-5.2f (*"đề đã lộ"*) không áp cho chúng, nhưng phát biểu (*"câu đã dùng"*) thì phụ thuộc định nghĩa "đã dùng" vốn còn treo ở GRR-085. Hệ quả trực tiếp: điều kiện *"ngân hàng đề còn câu hỏi"* của Đ-5.1 cho hai kết quả khác nhau.
- **Câu hỏi**: Câu đã được rút cho một vòng nhưng chưa được hỏi khi vòng đó bị bỏ có bị coi là đã tiêu không?

### GRR-119 · MISSING · Cao
- **Vị trí**: `game-rules-decisions.md` Đ-5; `fandom-olympia-26-luat-choi.md` §Khởi động, §Về đích; `game-rules-inventory.md` §R-KD-01
- **Trích**: Đ-5: *"Admin là người chọn đây là lượt của ai — hệ thống chỉ hiển thị recommendation dựa trên luật."* · `F26`: *"mỗi thí sinh trả lời 6 câu hỏi"*
- **Vấn đề**: quyền chọn lượt tự do của admin sinh ra một trạng thái mới chưa có luật: **lượt đang dở dang**. Nếu admin chuyển sang lượt của thí sinh B khi thí sinh A mới trả lời 3/6 câu, không rule nào nói lượt của A đã kết thúc hay còn treo, A có được quay lại trả lời 3 câu còn lại không, và 3 câu chưa hỏi xử lý ra sao. Cùng dạng áp cho gói 3 câu của Về đích: `F26` xếp lượt kế tiếp theo mốc *"sau khi thí sinh ở lượt 1 **hoàn thành** phần thi"* — mốc "hoàn thành" không tồn tại khi lượt bị cắt giữa chừng, làm nặng thêm GRR-041.
- **Câu hỏi**: Khi admin chuyển lượt trong lúc lượt của một thí sinh chưa hỏi hết số câu quy định, lượt đó được coi là đã kết thúc hay còn treo, và thí sinh có được quay lại hoàn tất không?

### GRR-120 · MISSING · Cao
- **Vị trí**: `game-rules-decisions.md` Đ-5, Đ-5.1 vs `game-rules-inventory.md` §Trạng thái game, §R-TB-01
- **Trích**: Đ-5.1: *"Admin được bỏ hẳn một vòng, và được chạy lại một vòng."* · R-TB-01: *"Kết quả khi có hoà: → `TIE_BREAK`. Khi không hoà: → `FINISHED`."*
- **Vấn đề**: không có ràng buộc thời điểm cho quyền chạy lại vòng. Nếu trận đã ở `FINISHED` (kết quả đã công bố, có thể đã xuất biên bản/PDF) mà admin chạy lại một vòng, không rule nào nói trạng thái trận quay về `rounds[i]`, kết quả đã công bố bị thu hồi thế nào, hay thao tác này bị chặn. GRR-077 hỏi về trận **không hoàn thành được**; đây là chiều ngược lại — trận **đã hoàn thành rồi quay lại**.
- **Câu hỏi**: Quyền bỏ vòng / chạy lại vòng có còn hiệu lực sau khi trận đã ở trạng thái `FINISHED` không, và nếu có thì trận chuyển về trạng thái nào?

### GRR-121 · CONFLICT · Cao
- **Vị trí**: `game-rules-decisions.md` Đ-5, Đ-5.3 vs `game-rules-inventory.md` §R-TB-01 (`SPEC` §3)
- **Trích**: R-TB-01: *"`SPEC` §3: TieBreakConfig chỉ hợp lệ ở **cuối playlist**."* · Đ-5: *"Admin là người chọn vòng nào sẽ bắt đầu."*
- **Vấn đề**: hai phát biểu cùng match tình huống "chạy một vòng sau khi tie-break đã diễn ra" và loại trừ nhau: ràng buộc `SPEC` §3 nói tie-break luôn nằm cuối, Đ-5 cho admin chạy bất kỳ vòng nào ở bất kỳ thời điểm nào. Nặng hơn: nếu điểm bị revert **sau khi** tie-break đã kết thúc, điều kiện hoà từng kích hoạt tie-break có thể không còn đúng, hoặc xuất hiện cặp hoà mới — không rule nào nói kết quả tie-break đã có bị huỷ, được giữ, hay phải chạy lại. Bổ sung cho GRR-106 ở chiều **sau** khi đã vào và đã kết thúc tie-break.
- **Câu hỏi**: Ràng buộc "tie-break chỉ ở cuối playlist" còn hiệu lực sau Đ-5 không, và nếu điểm thay đổi sau khi tie-break đã kết thúc thì kết quả tie-break được xử lý thế nào?

### GRR-122 · BOUNDARY_UNDEFINED · Trung bình
- **Vị trí**: `game-rules-decisions.md` Đ-5.1, Đ-5.2, Đ-2 vs `game-rules-inventory.md` §R-TB-01; `fandom-olympia-26-luat-choi.md` §Câu hỏi phụ
- **Trích**: Đ-5.2: *"Bỏ vòng thì điểm vòng đó sẽ reset về trước đó."* · `F26`: *"các thí sinh có cùng số điểm sẽ bước vào phần thi Câu hỏi phụ."*
- **Vấn đề**: quyền bỏ vòng tạo ra một biên trước đây không thể xảy ra: nếu admin bỏ **mọi** vòng có sinh điểm, toàn bộ thí sinh về **0 điểm** ⇒ cả sân cùng điểm ⇒ điều kiện hoà ở vị trí nhất thoả với **toàn bộ thí sinh**. Kết hợp Đ-2 (cho phép điểm âm), trạng thái "cả sân cùng điểm" còn xảy ra được ở giá trị âm. Luật câu hỏi phụ viết cho việc phân định giữa hai thí sinh (*"bằng với số điểm của thí sinh còn lại"*) và chỉ chọn ra **một** người thắng. GRR-065 đã hỏi thứ hạng những người thua trong nhóm ≥3, nhưng chưa mục nào hỏi biên **toàn sân cùng điểm**.
- **Câu hỏi**: Nếu toàn bộ thí sinh cùng điểm (kể cả cùng 0 hoặc cùng một giá trị âm) sau khi bỏ vòng, tie-break vẫn được kích hoạt bình thường hay có ngưỡng chặn?

### GRR-123 · MISSING · Cao
- **Vị trí**: `game-rules-decisions.md` Đ-4.2 vs Đ-4.4, Đ-5
- **Trích**: Đ-4.2: *"Mode nhập liệu: thí sinh click → admin xác nhận Yes/No. Hoặc admin click → admin xác nhận Yes/No. **Tín hiệu nào tới trước sẽ được xử lý, tín hiệu còn lại bị DROP**."* · Đ-4.4: *"Hệ thống **có gán** mỗi lượt chọn cho một thí sinh cụ thể."*
- **Vấn đề**: quy tắc first-signal-wins được phát biểu **không kèm điều kiện về lượt**. Nếu thí sinh **không có lượt** click chọn hàng ngang trước thí sinh **đang có lượt**, tín hiệu sai lượt tới trước ⇒ theo Đ-4.2 nó được xử lý còn tín hiệu đúng lượt bị drop. Kết quả này mâu thuẫn với Đ-4.4 (lượt gán cho một thí sinh cụ thể) và với Đ-5 (admin quyết định lượt của ai). Không quyết định nào nói tín hiệu sai lượt bị lọc ở tầng nào, hay cũng vào dialog Yes/No của admin như một tín hiệu hợp lệ.
- **Câu hỏi**: Tín hiệu chọn hàng ngang đến từ thí sinh không được gán lượt có bị loại trước khi vào quy tắc first-signal-wins không, hay vẫn cạnh tranh bình thường và để admin phân xử bằng dialog?

### GRR-124 · CONFLICT · Trung bình
- **Vị trí**: `game-rules-decisions.md` Đ-4.3 vs Đ-4.2
- **Trích**: Đ-4.3: *"**Đối với thí sinh: KHÔNG chống bấm nhầm**… Không dialog xác nhận, không bước rút lại."* và *"bước xác nhận chống-bấm-nhầm chỉ dành cho **ADMIN**, **không** áp cho thí sinh. Thao tác của thí sinh luôn tức thời và không hoàn tác."* vs Đ-4.2: *"thí sinh click → **admin xác nhận Yes/No**"*
- **Vấn đề**: hai quyết định cùng ngày cùng match một thao tác của thí sinh (click chọn hàng ngang) và cho kết quả trái ngược. Theo Đ-4.3, thao tác của thí sinh tức thời và không hoàn tác; theo Đ-4.2, nó **không** có hiệu lực cho tới khi admin bấm Yes, và admin bấm No thì bị huỷ — tức có lưới an toàn, chỉ do người khác cầm. Nguyên tắc "áp toàn hệ thống" của Đ-4.3 vì vậy không đúng với chính Đ-4.2. Việc Đ-4.2a còn treo (bấm No thì quay về trạng thái nào) làm hệ quả càng không xác định.
- **Câu hỏi**: Nguyên tắc "thao tác của thí sinh luôn tức thời và không hoàn tác" có ngoại lệ nào ngoài chọn hàng ngang không, và trong danh sách thao tác của thí sinh, thao tác nào có dialog admin, thao tác nào không?

### GRR-125 · CONFLICT · Trung bình
- **Vị trí**: `game-rules-decisions.md` Đ-4.3 vs `CLAUDE.md` §UX; `game-rules-inventory.md` §R-GEN-01
- **Trích**: Đ-4.3: *"Nút 'Mở chướng ngại vật' được xếp là CHUÔNG ⇒ áp quy tắc `CLAUDE.md` §UX: chỉ nhận click chuột, KHÔNG gán hotkey."* vs R-GEN-01: *"nút chuông nhận click chuột; **hotkey khác giữ nguyên (Enter gửi, 1-8 hàng, Esc xoá)**"*
- **Vấn đề**: Đ-4.3 phân loại lại một nút **giành quyền** thành chuông vì hệ quả bấm nhầm là nghiêm trọng. Nút **chọn hàng ngang** cũng là thao tác giành quyền và cũng không hoàn tác được (tiêu lượt chọn duy nhất theo `F26`), nhưng `CLAUDE.md`/R-GEN-01 đang cho phép hotkey `1-8`. Sau Đ-4.2 (thí sinh click chọn hàng ngang) hai rule cùng match một nút của thí sinh và cho hai kết luận ngược nhau về hotkey.
- **Câu hỏi**: Tiêu chí để một nút được xếp là "chuông" (cấm hotkey) là gì, và nút chọn hàng ngang của thí sinh có thuộc nhóm đó không?

### GRR-126 · AMBIGUOUS · Trung bình · *(biến thể của Đ-4.X2)*
- **Vị trí**: `game-rules-decisions.md` Đ-4 (đính chính VCNV), Đ-4.X2 vs `fandom-olympia-26-luat-choi.md` §VCNV
- **Trích**: Đ-4: *"Vượt chướng ngại vật: thí sinh trả lời HOÀN TOÀN BẰNG MÁY TÍNH ở cả hai mode. Mode ở VCNV **chỉ đổi cách chọn hàng ngang**."* · `F26`: *"Trả lời đúng câu hỏi ở ô trung tâm được 10 điểm"*
- **Vấn đề**: Đ-4.X2 đã nêu tranh chấp về kênh trả lời của **đáp án Chướng ngại vật**. Nhưng VCNV còn một loại đáp án thứ ba chưa được nhắc ở bất kỳ đâu: **câu hỏi ô trung tâm** — một câu hỏi độc lập trị giá 10 điểm, không phải hàng ngang cũng không phải Chướng ngại vật. Danh sách của Đ-4 (*"chỉ đổi cách chọn hàng ngang"*) không nói gì về nó, nên không xác định được nó luôn gõ máy hay đi theo mode. Kết hợp GRR-024 (chưa biết **ai** được trả lời câu này), nhánh này thiếu điều kiện để thực thi ở cả hai mode.
- **Câu hỏi**: Câu hỏi ở ô trung tâm được trả lời bằng máy tính trong mọi trường hợp, hay đi theo mode như Khởi động và Về đích?

### GRR-127 · MISSING · Thấp · `[v1.5]`
- **Vị trí**: `game-rules-decisions.md` Đ-4, Đ-4.a2, Đ-4.h vs `game-rules-inventory.md` §R-GEN-03 (`matchPurpose: practice`)
- **Trích**: Đ-4.a2: *"mode ở cấp contest là MỘT giá trị chung cho toàn bộ vòng."* · Đ-4.h: *"toàn bộ v1 đều phải có người điều khiển"* · R-GEN-03: practice *"chạy **không có admin**"*
- **Vấn đề**: mode sân khấu được định nghĩa bằng việc **thí sinh đọc câu trả lời** và có người nghe (*"máy tính chỉ dùng để giành quyền trả lời"*). Ở `matchPurpose: practice` không có admin và không có người nghe, nên mode sân khấu không có đường thực thi. Không quyết định nào nói practice bị khoá cứng ở mode nhập liệu hay vẫn nhận cả hai giá trị mode ở cấp contest. Khác GRR-070 (đã hoãn v1.5, hỏi về **cách chấm**), mục này hỏi **giá trị cấu hình mode** có bị ràng buộc theo `matchPurpose` không.
- **Câu hỏi**: Ở `matchPurpose: practice`, mode ở cấp contest có bị khoá về mode nhập liệu không?

### GRR-128 · CONFLICT · Thấp
- **Vị trí**: `game-rules-decisions.md` Đ-1 vs `game-rules-inventory.md` §R-VCNV-01, §R-VCNV-02, §R-GEN-04
- **Trích**: R-VCNV-01: *"Actor: Thí sinh, Admin, **Server (auto-match)**"* · R-VCNV-02: *"**Kết quả khi đúng chính tả hoàn toàn**: công nhận. **Kết quả khi sai kí tự/dấu câu/ngữ pháp**: không công nhận."*
- **Vấn đề**: Đ-1 chốt hệ thống **không có nhánh nào tự công nhận hay bác bỏ** đáp án, nhưng inventory vẫn phát biểu các nhánh kết quả này như **rule của máy** và vẫn liệt Server là actor phán quyết. R-GEN-03 chỉ thêm một khối "hệ quả bắt buộc" chung, không sửa từng rule cụ thể — người đọc vẫn thấy hai phát biểu cùng match một thao tác chấm. Khác GRR-100 (hai khối "hệ quả bắt buộc" trùng nhau — lỗi biên tập), mục này là **nội dung rule chưa đồng bộ với quyết định**.
- **Câu hỏi**: Các nhánh "công nhận / không công nhận" và actor "Server (auto-match)" trong inventory nay được hiểu là gợi ý hiển thị, hay tài liệu cần phát biểu lại?

### GRR-129 · MISSING · Cao · `[ĐÓNG — Đ-14.G: admin quyết, hệ thống chỉ cảnh báo]`
- **Vị trí**: `game-rules-decisions.md` §2 (*"v1 = ĐÚNG 4 thí sinh"*) vs `game-rules-inventory.md` §R-GEN-09 (`dropoutPolicy`), §U-13
- **Trích**: `game-rules-decisions.md`: *"**v1 = ĐÚNG 4 thí sinh.** Luật phải được làm rõ và đầy đủ cho cấu hình 4 thí sinh trước."* · R-GEN-09: *"Kết quả khi quá grace: theo `dropoutPolicy`"* · U-13: *"danh sách đầy đủ giá trị `dropoutPolicy`"* chưa có
- **Vấn đề**: "đúng 4 thí sinh" là ràng buộc **cấu hình**, nhưng số thí sinh **đang thi** có thể tụt xuống 3 **giữa trận** (mất kết nối quá grace, admin loại, thí sinh bỏ cuộc). Khi đó chính các luật được giữ lại cho v1 vì "chỉ đúng ở cấu hình 4" mất căn cứ: thang 40/30/20/10 (4 bậc cho 3 người), *"1 trong 3 thí sinh còn lại"* cướp quyền (còn 2), 4 lượt chọn / 4 hàng ngang, danh sách Lượt 1→4 của Về đích. GRR-029/GRR-030/GRR-045/GRR-105 đã hoãn v2 **với lý do là số ghế cấu hình**, nên không mục nào đang phủ nhánh "số ghế cấu hình = 4 nhưng số người thi thực tế < 4".
- **Câu hỏi**: Nếu một thí sinh rời trận giữa chừng ở v1, trận tiếp tục với 3 người theo luật nào, hay dừng lại?

### GRR-130 · MISSING · Trung bình
- **Vị trí**: `game-rules-decisions.md` Đ-5.2e, Đ-5.3 vs `CLAUDE.md` §Quy ước khác (D22, sound)
- **Trích**: Đ-5.2e: *"viewer/overlay thấy **số đột ngột thay đổi** — không hiệu ứng, không thông báo."* · `CLAUDE.md`: *"Animation là module ĐỘC LẬP với engine/rule (D22): engine chỉ emit semantic event"* · *"engine emit `sound-cue {slot}`"*
- **Vấn đề**: Đ-5.2e chỉ nói về **hiển thị số điểm**. Theo D22, mọi thay đổi trạng thái đi qua **semantic event**, và mỗi event map được sang animation và `sound-cue`. Các event mới do Đ-5.1/5.2/5.3 sinh ra (revert điểm, bỏ vòng, chạy lại vòng, override qua cảnh báo) chưa được nói là có phát semantic event hay không: nếu có, client sẽ chạy animation/âm thanh cho đúng thao tác mà Đ-5.2e chủ ý muốn giấu; nếu không, tồn tại một loại thay đổi trạng thái không đi qua event, phá nguyên tắc D22 và mô hình "điểm là hàm của event log" của Đ-5.3.
- **Câu hỏi**: Các thao tác revert / bỏ vòng / chạy lại vòng có phát semantic event ra client không, và nếu có thì mapping sang animation và `sound-cue` được quy định thế nào?

### GRR-131 · MISSING · Cao
- **Vị trí**: `game-rules-decisions.md` Đ-4.1, Đ-4.d
- **Trích**: Đ-4.1: *"Mọi rule trong tài liệu này và trong `docs/game-rules-inventory.md` được hiểu là **luật của mode sân khấu**. Mode nhập liệu là **biến thể**, không phải bản gốc."*
- **Vấn đề**: quyết định xác lập mode sân khấu là bản gốc nhưng **không kèm danh sách rule khác biệt** của biến thể. Đ-4.d mới nêu 5 mục (GRR-002, GRR-003, GRR-004, GRR-035, GRR-044) dưới dạng câu hỏi **chưa được trả lời**. Hệ quả: mode nhập liệu — mode **duy nhất** có ô nhập, có timestamp đáp án, có highlight của Đ-1 — hiện **không có đặc tả nào của riêng nó**, trong khi Đ-4.a2 cho phép một contest chạy hoàn toàn ở mode này. Ngược lại, ở mode sân khấu chưa nói rõ các rule ghi nhận đáp án bị **loại bỏ hẳn** hay chỉ **không được kích hoạt**.
- **Câu hỏi**: Danh sách đầy đủ các rule bị thay đổi hoặc mất hiệu lực khi contest chạy ở mode nhập liệu gồm những rule nào, và ở mode sân khấu các rule ghi nhận đáp án bị loại bỏ hay chỉ không được kích hoạt?

---

### Nhóm C — Mâu thuẫn quyết định ↔ quyết định hoặc quyết định ↔ `CLAUDE.md`

### GRR-132 · CONFLICT · Cao
- **Vị trí**: `game-rules-decisions.md` §2 vs `CLAUDE.md` §Lộ trình version (D18)
- **Trích**: `game-rules-decisions.md`: *"**v1 = ĐÚNG 4 thí sinh.** … Luật cho số thí sinh khác 4 để **v2**."* vs `CLAUDE.md`: *"**v1 — Solo contest** (Phase 1-10): contest chính thức, thí sinh CÁ NHÂN **1-12 ghế**, **đủ loại vòng + biến thể**"*
- **Vấn đề**: hai phát biểu về **phạm vi v1** loại trừ nhau trên cùng một trục. `CLAUDE.md` là tài liệu nền của repo và đang khai 1-12 ghế cùng "đủ loại vòng + biến thể" cho v1; `game-rules-decisions.md` thu hẹp v1 về đúng 4 ghế và đẩy phần còn lại sang v2. Mâu thuẫn này quyết định trực tiếp việc 6 mục hoãn ở `game-rules-decisions.md` §2 (GRR-017, GRR-029, GRR-030, GRR-045, GRR-083, GRR-105) có thật sự được hoãn không, và pre-flight có chặn cấu hình ≠ 4 ghế ở v1 không.
- **Câu hỏi**: Phát biểu nào có hiệu lực cho v1 — "1-12 ghế" của `CLAUDE.md` hay "đúng 4 thí sinh" của `game-rules-decisions.md` — và văn bản còn lại có cần được phát biểu lại không?

### GRR-133 · CONFLICT · Cao
- **Vị trí**: `game-rules-decisions.md` Đ-5.1 vs `CLAUDE.md` §Luật chơi & đề thi; `game-rules-inventory.md` §R-KD-07
- **Trích**: Đ-5.1: *"Admin được bỏ hẳn một vòng, và được chạy lại một vòng — với điều kiện trong **ngân hàng đề của contest** còn câu hỏi."* vs `CLAUDE.md`: *"Người tạo contest **PHẢI chọn danh sách câu hỏi trước khi start**… hệ thống KHÔNG tự lấy đề — draw chỉ RANDOM TRONG **danh sách đã gán** (snapshot)."*
- **Vấn đề**: hai phát biểu dùng hai tập câu hỏi khác nhau làm điều kiện. "Ngân hàng đề của contest" là kho rộng; "danh sách đã gán (snapshot)" là tập hẹp đã cố định trước khi start. Khi snapshot cạn nhưng kho contest còn câu, hai rule cho kết quả ngược nhau: được chạy lại vòng (Đ-5.1) hay không có câu để rút (`CLAUDE.md`, R-KD-07). Nếu theo Đ-5.1 thì hệ thống phải lấy câu **ngoài** snapshot — trái nguyên tắc "hệ thống KHÔNG tự lấy đề". Đây là tiền đề của Đ-5.1g/h/i, cả ba đều không xác định được khi chưa chốt tập nào là căn cứ.
- **Câu hỏi**: Điều kiện "còn câu hỏi" của Đ-5.1 đo trên danh sách đã gán cho trận hay trên toàn kho đề của contest, và nếu là kho contest thì việc bổ sung câu vào snapshot giữa trận có được phép không?

### GRR-134 · CONFLICT · Trung bình
- **Vị trí**: `game-rules-decisions.md` Đ-4.3 vs `CLAUDE.md` §UX BẮT BUỘC
- **Trích**: Đ-4.3: *"Thao tác của thí sinh **luôn tức thời và không hoàn tác**."* vs `CLAUDE.md`: *"**KHÔNG chặn gửi lại**… người dùng gửi lại được — server nhận **bản cuối cùng** trước timeout"* và *"Tăng tốc: nhận MỌI lần trả lời đến khi hết giờ, tính BẢN CUỐI CÙNG"*
- **Vấn đề**: nguyên tắc của Đ-4.3 được phát biểu **áp toàn hệ thống** cho mọi thao tác của thí sinh, trong khi nguyên tắc nền của `CLAUDE.md` cho phép thí sinh **ghi đè liên tục** đáp án đã gửi — tức thao tác của thí sinh vừa không tức-thời-chung-cuộc, vừa hoàn tác được bằng cách gửi bản mới. Hai phát biểu cùng match hành vi "thí sinh gửi lại đáp án" và cho hai mô hình khác nhau. Không văn bản nào nói phạm vi Đ-4.3 chỉ giới hạn ở **thao tác dạng nút giành quyền**.
- **Câu hỏi**: Nguyên tắc "không hoàn tác" của Đ-4.3 áp cho mọi thao tác của thí sinh, hay chỉ cho nhóm thao tác giành quyền, còn việc gửi đáp án vẫn theo last-wins của `CLAUDE.md`?

### GRR-135 · CONFLICT · Trung bình
- **Vị trí**: `game-rules-decisions.md` Đ-5.2e vs `CLAUDE.md` §UX BẮT BUỘC
- **Trích**: Đ-5.2e: *"viewer/overlay thấy số đột ngột thay đổi — **không hiệu ứng, không thông báo**. Quyết định có chủ đích."* vs `CLAUDE.md`: *"**Visibility of System Status**: Mọi thao tác async… PHẢI hiển thị trạng thái rõ ràng. Không để UI im lặng khi đang xử lý."*
- **Vấn đề**: hai quy tắc cùng match một sự kiện (điểm trên màn viewer/overlay thay đổi do thao tác của admin) và cho kết quả ngược nhau: im lặng hoàn toàn, hay bắt buộc có phản hồi trạng thái. `CLAUDE.md` phát biểu quy tắc cho **MỌI UI trong repo** và không nêu ngoại lệ cho viewer/overlay. Cần biết đây là ngoại lệ cấp cho một trường hợp, hay là thu hẹp phạm vi của quy tắc nền (chỉ áp cho người thực hiện thao tác, không áp cho người xem).
- **Câu hỏi**: Đ-5.2e là ngoại lệ riêng của thao tác bỏ vòng, hay quy tắc "Visibility of System Status" chỉ áp cho người thực hiện thao tác chứ không áp cho viewer/overlay?

### GRR-136 · CONFLICT · Trung bình
- **Vị trí**: `game-rules-decisions.md` Đ-5.2, Đ-5.3 vs `CLAUDE.md` §Lộ trình version (retention), §Quy ước khác (AuditLog)
- **Trích**: Đ-5.2: *"**Biên bản trận đấu vẫn phải đầy đủ thông tin.**"* · Đ-5.3: *"Lịch sử luôn linear, **mọi thao tác không bị xoá**."* vs `CLAUDE.md`: *"retention riêng (**practice 3 tháng / official 12 tháng**)"*
- **Vấn đề**: hai nguyên tắc cùng match dữ liệu trận đấu và loại trừ nhau theo thời gian: append-only không xoá, và xoá/ẩn danh theo hạn lưu trữ. Vì Đ-5.3 định nghĩa **điểm là hàm của event log**, việc dọn retention không chỉ xoá lịch sử mà còn làm **mất khả năng tái tính điểm** của trận. Không văn bản nào nói biên bản/PDF đã xuất có thay thế được event log sau khi retention chạy, hay official 12 tháng là ngoại lệ được giữ.
- **Câu hỏi**: Sau khi hạn retention của một trận official kết thúc, event log điểm có bị xoá không, và nếu có thì "biên bản đầy đủ" của Đ-5.2 tồn tại dưới dạng nào?

---

## 3. Phụ lục — Bảng thuật ngữ mơ hồ

| Thuật ngữ | Các cách hiểu có thể | Nơi xuất hiện |
|---|---|---|
| "MC đọc xong câu hỏi" | sự kiện MC bấm nút · hết audio · admin bấm start timer · không có sự kiện nào tương ứng | `F26` §Khởi động; inventory §R-KD-01, §U-1 (GRR-001) |
| "hiệu lệnh của người dẫn chương trình" | cùng nghĩa đọc xong · một tín hiệu riêng sau khi đọc xong · lệnh mở chuông | `F26` §Câu hỏi phụ; inventory §R-TB-03 (GRR-060) |
| "MC công bố đáp án" | mốc thứ ba sau khi hết giờ · cùng lúc admin chấm | `F26` §Khởi động; inventory §R-KD-06 (GRR-004) |
| "trước khi câu hỏi được đọc lên hoặc hiện lên trên màn hình" | mốc hiển thị cho thí sinh · mốc hiển thị cho viewer · mốc admin mở câu | `F26` §Về đích; inventory §R-VD-06 (GRR-048) |
| "vị trí đứng thấp nhất" / "vị trí số 1" | số ghế nhỏ nhất · số ghế lớn nhất · thứ hạng điểm thấp nhất · vị trí sân khấu | `F26` §Về đích, §VCNV; inventory §R-VD-02 (U-27), §R-VCNV-03 (GRR-040, GRR-018) |
| "bị loại khỏi phần thi này" | mất lượt chọn hàng ngang · mất luôn quyền trả lời hàng ngang · mất luôn quyền trả lời ô trung tâm · khoá toàn bộ input | `F26` §VCNV; inventory §R-VCNV-04 (GRR-022) |
| "trong 1 / 2 / 3 / 4 từ hàng ngang" | số hàng đã mở · số hàng đã hỏi · thứ tự hàng đang hỏi | `F26` §VCNV; inventory §R-VCNV-04 (GRR-019) |
| "cả 4 từ hàng ngang đã được mở ra" | 4 miếng ghép đã mở · đã hỏi hết 4 hàng bất kể mở được bao nhiêu | `F26` §VCNV; inventory §R-VCNV-05 (GRR-013) |
| "cùng một khoảng thời gian" (Tăng tốc) | cùng ms · cùng 2 chữ số thập phân · cùng microsecond · cùng một cửa sổ do cấu hình | `F26` §Tăng tốc; inventory §R-TT-02 (GRR-031) |
| "cùng tổng số chữ cái" | số ký tự kể cả dấu · số chữ cái không dấu · số ký tự không tính khoảng trắng · số từ (wordCount) | `F26` §VCNV; inventory §R-VCNV-02 (GRR-067) |
| "ý nghĩa tương đồng" | đồng nghĩa do admin công nhận · nằm trong acceptedAnswers · so khớp mờ | `F26` §VCNV, §Tăng tốc; inventory §R-GEN-04 (GRR-067, GRR-074) |
| "đúng chính tả" | khớp chính xác từng ký tự kể cả dấu · khớp sau normalize · khớp sau bỏ dấu | `F26` §VCNV, §Tăng tốc; inventory §R-VCNV-02 vs §R-GEN-04 (GRR-066) |
| "đáp án đầu tiên" / "đáp án cuối cùng" | bản gửi sớm nhất · bản đang hiển thị khi hết giờ · bản khác rỗng gần nhất | `F26` §Khởi động, §Về đích; inventory §R-KD-06, §R-TT-03, §R-VD-05 (GRR-003, GRR-033, GRR-044) |
| "thay đổi đáp án" | gửi nội dung khác bản trước · mọi lần gửi lại kể cả trùng nội dung | `F26` §Khởi động; inventory §R-TT-03 (GRR-033) |
| "1 trong 3 thí sinh còn lại" | đúng 3 người ở cấu hình 4 ghế · mọi thí sinh trừ người đang thi · mọi thí sinh chưa bị loại | `F26` §Về đích; inventory §R-VD-05 (GRR-045) |
| "hoàn thành phần thi" (lượt Về đích) | trả lời xong câu cuối · admin chấm xong câu cuối · đóng cửa sổ cướp câu cuối | `F26` §Về đích; inventory §R-VD-02 (GRR-041) |
| "một nửa số điểm của câu hỏi" | chia đôi làm tròn xuống · làm tròn lên · chỉ áp cho giá trị chẵn | `F26` §Về đích; inventory §R-VD-05 (GRR-046) |
| "đã được hỏi" (no-repeat) | đã hiển thị cho thí sinh · đã chấm · đã rút khỏi pool | inventory §R-GEN-06 (GRR-085) |
| "build-upon" (undo) | event sau của cùng thí sinh · cùng câu · cùng vòng · bất kỳ event chấm nào sau đó | inventory §R-GEN-07 (GRR-073) |
| "đơn vị điểm" (scoringUnit) | thí sinh cá nhân · đội · ghế | inventory §R-TT-01, §PHẦN 7 |
| "trả lời sai" | đáp án sai nội dung · hết giờ không trả lời · bấm chuông rồi im lặng · thực hành không đạt | `F26` mọi phần; inventory §R-KD-01, §R-GEN-03 (GRR-006, GRR-008, GRR-068) |
| "dừng" | dừng toàn trận · dừng cục bộ đồng hồ một hàng ngang | inventory §R-GEN-08 vs §R-VCNV-06 (GRR-026, GRR-079) |
| "câu thực hành đạt yêu cầu" | đánh giá của admin · của ban cố vấn · tiêu chí ghi trong câu hỏi | `F26` §Về đích; inventory §R-VD-04 (GRR-050) |

---

## 4. Ghi chú phạm vi

- Tài liệu này KHÔNG phân xử bất kỳ mâu thuẫn nào ở trên, KHÔNG đề xuất giá trị thay thế, KHÔNG bổ sung rule mới. Mọi mục kết thúc bằng câu hỏi cần chủ dự án trả lời.
- Các mã `U-*` và `K-*` được trích lại từ `docs/game-rules-inventory.md` để truy vết. Review này bổ sung thêm những khiếm khuyết chưa được liệt kê trong PHẦN 8 và PHẦN 9 của tài liệu đó, đáng chú ý: GRR-003, GRR-005, GRR-006, GRR-008, GRR-016, GRR-019, GRR-021, GRR-030, GRR-032, GRR-033, GRR-034, GRR-035, GRR-036, GRR-039, GRR-042, GRR-043, GRR-044, GRR-049, GRR-051, GRR-052, GRR-053, GRR-056, GRR-059, GRR-061, GRR-062, GRR-065, GRR-068, GRR-071, GRR-075, GRR-076, GRR-077, GRR-079, GRR-083, GRR-092, GRR-095, GRR-099, GRR-100, GRR-101, GRR-103, GRR-104, GRR-105, GRR-106.
