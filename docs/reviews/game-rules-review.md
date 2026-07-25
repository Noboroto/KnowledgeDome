# Business-Rule Review — Lượt 3 (GRR-137 → GRR-171)

> **Ngày review**: 2026-07-25 · **Loại**: business-rule review (KHÔNG phải technical design review)
>
> **Nguồn rà soát**: `docs/source/fandom-olympia-26-luat-choi.md` (nguyên văn F26) · `docs/game-rules-inventory.md` (R-*, U-1→U-38, K-1→K-16)
>
> **Đối chiếu để không nêu lại**: `game-rules-decisions.md` (Đ-1→Đ-15) · `game-rules-open-questions.md` · `game-rules-review-old.md` (GRR-001→GRR-136)
>
> **Nguyên tắc thi hành**: chỉ NÊU vấn đề. KHÔNG tự sửa rule, KHÔNG tự quyết định behavior, KHÔNG thêm business requirement, KHÔNG đề xuất schema/kiến trúc/code.

## Vì sao có lượt 3

Lượt 1-2 (GRR-001→GRR-136) rà soát **nội dung luật**: giá trị, điều kiện, mâu thuẫn giữa các nguồn. Lượt này rà soát **hành vi hệ thống quanh luật** — bốn loại khiếm khuyết mà 136 mục cũ **chưa dùng lần nào**:

| Loại | Số mục | Vì sao lượt trước bỏ sót |
|---|---|---|
| `IDEMPOTENCY_UNDEFINED` | 8 | `CLAUDE.md` §UX **cấm disable nút gửi** ⇒ bấm lặp là hành vi thường xuyên, nhưng `Đ-3.3` chỉ định nghĩa dedup cho **một** thao tác (chấm điểm) |
| `CONCURRENCY_UNDEFINED` | 6 | `Đ-7.2` chốt queue **không chặn** ở 3 vòng ⇒ server phân xử ngay, nhưng quy tắc phân xử chỉ có cho Tăng tốc (K-8) |
| `INVALID_TRANSITION_UNDEFINED` | 6 | `Đ-7.a` **bỏ hẳn khái niệm drop tín hiệu** ⇒ mọi sự kiện không hợp lệ **bắt buộc** phải có outcome, chưa rule nào nói outcome đó là gì |
| `ORDER_DEPENDENT` | 5 | `Đ-5.3` chuyển điểm sang event log + `Đ-11.B` cho `SCORE_ADJUST` không tự revert ⇒ sinh ra các cặp thao tác đổi thứ tự cho kết quả khác nhau |
| `MISSING` | 4 | — |
| `CONFLICT` | 3 | — |
| `SIDE_EFFECT_UNDEFINED` · `AMBIGUOUS` · `UNREACHABLE` | 1 mỗi loại | — |

**Tổng: 35 mục.** Không mục nào trùng nội dung với GRR-001→GRR-136; chỗ nào gần một mục cũ đều ghi rõ ranh giới ở dòng *Ranh giới*.

## Mức chặn

| Mức | Nghĩa | Số mục |
|---|---|---|
| **Cao** | Làm SAI ĐIỂM hoặc KẸT LUỒNG trận trong kịch bản thực tế | 21 |
| **Trung bình** | Gây bất nhất dữ liệu / trải nghiệm sai, không đổi kết quả trận | 12 |
| **Thấp `[v2]`** | Chỉ phát sinh ở thi đội | 2 |

---

# PHẦN 1 — KHỞI ĐỘNG

## GRR-137 · `IDEMPOTENCY_UNDEFINED` · Cao — admin bấm start timer lần thứ hai

- **Mô tả**: `Đ-6` chốt *"MC đọc xong câu hỏi" = admin bấm start timer*, không nói kết quả khi bấm **lần thứ hai** trên cùng một câu. Admin bấm ở t=0, thấy đồng hồ chưa nhảy (lag), bấm lại ở t=1.2s → nếu lệnh 2 đặt `endsAt` mới, thí sinh có **4.2 giây thay vì 3**; nếu bị bỏ qua, admin không có cách sửa khi lần bấm đầu là nhầm.
- **Rule**: R-KD-01, R-KD-03, R-KD-04 (và mọi vòng dùng mốc admin).
- **File nguồn**: `docs/source/fandom-olympia-26-luat-choi.md` §Khởi động · `game-rules-decisions.md` §8.1 `Đ-6`, §8.2 `Đ-6.3`.
- **Tác động**: sai độ dài cửa sổ 3s ⇒ sai kết quả "bấm chuông trong hạn / quá hạn". `Đ-6.3` tuyên bố mốc admin là **tuyệt đối**, nhưng chính thao tác tạo mốc lại không idempotent.
- **Ranh giới**: `§3.3 [GRR-071]` chỉ dedup thao tác **chấm điểm**, không đụng thao tác **tạo mốc thời gian**.
- **❓ Câu hỏi**: Lần bấm start timer thứ hai trên cùng một câu bị bỏ qua, hay đặt lại đồng hồ từ đầu?

## GRR-138 · `CONCURRENCY_UNDEFINED` · Cao — hai chuông cùng ms ở vòng queue KHÔNG chặn

- **Mô tả**: Tăng tốc có luật đồng-thời-gian (*"cùng nhận một mức điểm"*, K-8 chốt độ phân giải **ms**), nhưng **quyền trả lời thì không chia được** — một trong hai người phải được gọi. `Đ-7.2` nói Khởi động lượt chung và cướp quyền Về đích *"có chuông là tính ngay"* theo thứ tự tới, im lặng khi **thứ tự tới bằng nhau**.
- **Rule**: R-KD-03, R-VD-05.
- **File nguồn**: F26 §Khởi động, §Về đích · `game-rules-inventory.md` §R-KD-03, §R-TT-02 (K-8) · `game-rules-decisions.md` §5.2 `Đ-7.2`.
- **Tác động**: quyết định trực tiếp ai được **+10** và ai bị **−5**. Không có quy tắc ⇒ kết quả phụ thuộc thứ tự xử lý nội bộ, **không tái lập được khi replay event log** — phá `Đ-5.3` ("điểm là hàm của event log").
- **Ranh giới**: GRR-021 chỉ hỏi cho VCNV (nơi queue CHẶN, admin phân xử được); GRR-036 chỉ hỏi cùng-một-ghế ở Tăng tốc.
- **❓ Câu hỏi**: Khi hai chuông có cùng server timestamp ở vòng queue không chặn, hệ thống chọn theo tiêu chí nào (vị trí ghế, hay đẩy sang admin phân xử)?

## GRR-139 · `ORDER_DEPENDENT` · Cao — hai thao tác admin tạo mốc cho cùng một câu, chưa quy định thứ tự

- **Mô tả**: *bấm hiển thị câu hỏi* (mốc đóng NSHV — `GRR-048`) và *bấm start timer* (mốc "MC đọc xong" — `Đ-6`) là hai thao tác riêng. Không nguồn nào quy định thứ tự bắt buộc, cũng không nói outcome khi admin bấm start timer mà **chưa** bấm hiển thị. `[GRR-005]` định nghĩa cửa sổ chuông lượt chung = **(thời gian MC đọc) + 3 giây** — khoảng "MC đọc" **chỉ tồn tại nếu hai thao tác tách rời**; admin bấm gộp ⇒ cửa sổ co lại còn đúng 3s và luật *"được bấm khi MC đang đọc"* mất hiệu lực trên thực tế.
- **Rule**: R-KD-03, R-KD-04 (kéo theo R-VD-06 ở Về đích).
- **File nguồn**: F26 §Khởi động · `game-rules-decisions.md` §8.1, §9.1 `[GRR-005]`, §9.4 `[GRR-048]`.
- **Tác động**: độ dài cửa sổ chuông biến thiên theo thói quen bấm của admin, không theo luật. Ở Về đích còn làm **cửa sổ NSHV không bao giờ đóng** nếu admin bỏ qua thao tác hiển thị.
- **❓ Câu hỏi**: "Bấm hiển thị câu hỏi" và "bấm start timer" là hai thao tác bắt buộc tách rời theo thứ tự cố định, hay admin được bấm gộp / đảo thứ tự?

## GRR-140 · `IDEMPOTENCY_UNDEFINED` · Trung bình — cùng một ghế bấm chuông nhiều lần trong một câu

- **Mô tả**: nút chuông không bị disable (`CLAUDE.md` §UX chỉ cho disable khi khoá-theo-LUẬT), `Đ-7` đưa mọi tín hiệu vào queue theo thứ tự tới và `Đ-7.a` **không có cơ chế drop**. Không rule nào nói các lần bấm thứ 2, 3, 4 của **cùng một ghế trong cùng một câu** được gộp hay tồn tại độc lập. A bấm 3 lần, giành quyền, trả lời sai (−5); hai entry còn lại của A vẫn nằm trong queue.
- **Rule**: R-KD-03, R-VD-05.
- **File nguồn**: `game-rules-decisions.md` §5.1 `Đ-7`, §5.2 `Đ-7.2` · `CLAUDE.md` §UX BẮT BUỘC.
- **Tác động**: queue phình rác; khả năng một thí sinh bị xử lý hai lần trên cùng câu (**trừ −5 hai lần**).
- **❓ Câu hỏi**: Nhiều lần bấm chuông của cùng một ghế trong cùng một câu được gộp thành một tín hiệu duy nhất hay giữ nguyên tất cả trong hàng đợi?

## GRR-141 · `INVALID_TRANSITION_UNDEFINED` · Trung bình — tín hiệu chuông từ ghế không còn quyền

- **Mô tả**: chưa rule nào định nghĩa outcome của tín hiệu chuông đến từ ghế **đã bấm và bị chấm Sai** trên câu đó, hoặc khi câu đã đóng chuông. Vì `Đ-7.a` bỏ hẳn drop, tín hiệu này **bắt buộc** phải có outcome — bị lọc trước khi vào queue, hay vào queue để admin reject từng cái. Người vừa bị −5 tiếp tục bấm ⇒ mỗi lần bấm sinh một mục chờ duyệt giữa lúc câu tiếp theo đang chạy.
- **Rule**: R-KD-03.
- **File nguồn**: F26 §Khởi động · `game-rules-inventory.md` §R-KD-03 · `game-rules-decisions.md` §5.1 `Đ-7.a`.
- **Tác động**: mỗi tầng tự quyết; admin bị ngập tín hiệu rác đúng lúc cần thao tác nhanh.
- **Ranh giới**: GRR-007 hỏi *câu có mở lại chuông không*; đây là *outcome của tín hiệu không hợp lệ*.
- **❓ Câu hỏi**: Tín hiệu chuông từ ghế không còn quyền trên câu hiện tại bị lọc trước hàng đợi, hay vẫn vào hàng đợi và cần admin reject?

---

# PHẦN 2 — VƯỢT CHƯỚNG NGẠI VẬT

## GRR-142 · `ORDER_DEPENDENT` · Cao — băng điểm CNV chốt tại lúc bấm hay lúc admin xác nhận

- **Mô tả**: ở VCNV queue **CHẶN** (`Đ-7.2`) — tín hiệu chỉ có hiệu lực khi admin bấm Yes. Thang điểm CNV đếm theo **số hàng ngang ĐÃ HỎI** (`[GRR-019]`). Không nguồn nào nói mốc điểm chốt tại **timestamp thí sinh bấm** hay **thời điểm admin xác nhận**. B bấm "Mở chướng ngại vật" khi mới hỏi 1 hàng (băng **60**), admin bận duyệt việc khác và xác nhận sau khi hàng 2 đã bắt đầu ⇒ **60 hay 50**.
- **Rule**: R-VCNV-04, R-VCNV-06.
- **File nguồn**: F26 §VCNV · `game-rules-decisions.md` §5.2 `Đ-7.2`, §9.2 `[GRR-019]`, `[GRR-025]`.
- **Tác động**: sai điểm ở vòng có biên độ lớn nhất trận (60/50/40/30/20); giá trị phụ thuộc **độ trễ tay người**, trái tinh thần "server time là quyết định cuối cùng".
- **❓ Câu hỏi**: Mốc điểm Chướng ngại vật chốt theo server timestamp lúc thí sinh bấm, hay theo trạng thái bàn cờ lúc admin xác nhận?

## GRR-143 · `SIDE_EFFECT_UNDEFINED` · Cao — admin bấm No: những gì đã xảy ra có được hoàn nguyên không

- **Mô tả**: `Đ-7` chỉ nói reject **không làm thí sinh mất lượt** và tín hiệu kế tiếp lên. Không nói các tác dụng phụ khác tại thời điểm admin bấm No cho tín hiệu chọn hàng ngang: hàng ngang đó đã bị đánh dấu "đã hỏi" chưa (biến số của thang điểm CNV — GRR-142), câu hỏi tương ứng đã bị rút và đánh cờ `usedInContest` chưa (`Đ-5.2f`: câu đã dùng **KHÔNG trả lại pool**), đồng hồ 15 giây đã khởi động chưa. Admin bấm No nhưng câu của hàng ngang đó đã tiêu khỏi pool ⇒ **hàng ngang không còn câu để hỏi lại**.
- **Rule**: R-VCNV-03, R-VCNV-01, R-GEN-06.
- **File nguồn**: `game-rules-decisions.md` §5.4 `Đ-4.2`, §5.1 `Đ-7`, §6.3 `Đ-5.2f` · `game-rules-inventory.md` §R-KD-07, §R-GEN-06 (U-30).
- **Tác động**: tiêu đề ngoài ý muốn, có thể làm vòng không hoàn thành được — trong khi mục đích của **No** chính là "chưa có gì xảy ra cả".
- **Ranh giới**: GRR-085/GRR-118 bàn định nghĩa "đã dùng" nói chung, không bàn nhánh reject.
- **❓ Câu hỏi**: Khi admin bấm No, hệ thống coi như chưa có gì xảy ra (câu chưa rút, hàng chưa hỏi, timer chưa chạy), hay các tác dụng phụ đã phát sinh và được giữ nguyên?

## GRR-144 · `CONCURRENCY_UNDEFINED` · Cao — hai loại tín hiệu khác bản chất trong cùng một hàng đợi chặn

- **Mô tả**: VCNV có *chọn hàng ngang* và *"Mở chướng ngại vật"* cùng đi vào một queue chặn. Không rule nào nói queue xử lý thuần FIFO trộn lẫn hay tách luồng theo loại. Admin đang mở dialog duyệt tín hiệu chọn hàng ngang của A; B bấm "Mở chướng ngại vật". Nếu FIFO thuần, B phải chờ; admin bấm Yes cho A ⇒ hàng ngang mới được hỏi ⇒ theo `[GRR-019]` số hàng đã hỏi tăng ⇒ **B tụt băng 60→50** dù đã bấm trước khi hàng đó bắt đầu.
- **Rule**: R-VCNV-03, R-VCNV-04, R-VCNV-06.
- **File nguồn**: F26 §VCNV (*"bấm chuông trả lời Chướng ngại vật bất cứ lúc nào"*) · `game-rules-decisions.md` §5.2 `Đ-7.2`, §9.2.
- **Tác động**: sai băng điểm CNV; một tín hiệu **kết thúc-vòng** có thể nằm chờ sau một tín hiệu ít quan trọng hơn.
- **Ranh giới**: GRR-026 / U-16 hỏi pause timer khi có người bấm CNV giữa timer hàng ngang, không hỏi trật tự giữa hai loại tín hiệu.
- **❓ Câu hỏi**: Hàng đợi VCNV xử lý thuần theo thứ tự tới cho cả hai loại tín hiệu, hay "Mở chướng ngại vật" được ưu tiên trước tín hiệu chọn hàng ngang đang chờ?

## GRR-145 · `IDEMPOTENCY_UNDEFINED` · Cao — bấm "Mở chướng ngại vật" nhiều lần

- **Mô tả**: nút này là chuông, **không dialog phía thí sinh**, bấm là gửi ngay (`Đ-4.3`), queue không drop (`Đ-7.a`). Không rule nào nói nhiều lần bấm của cùng một thí sinh được gộp. A bấm 3 lần; admin duyệt entry 1, A trả lời sai và **bị loại khỏi phần thi này**; hai entry còn lại của A vẫn trong hàng đợi.
- **Rule**: R-VCNV-04, R-VCNV-06.
- **File nguồn**: F26 §VCNV (*"Nếu trả lời sai Chướng ngại vật, thí sinh sẽ bị loại"*) · `game-rules-decisions.md` §5.1 `Đ-7`, §5.3 `Đ-4.3`.
- **Tác động**: nguy cơ một thí sinh được đoán CNV **nhiều lần** — phá đúng cơ chế mà luật gốc dùng để giới hạn rủi ro (một lần đoán, sai thì loại).
- **❓ Câu hỏi**: Nhiều lần bấm "Mở chướng ngại vật" của cùng một thí sinh trong cùng một vòng được gộp thành một tín hiệu duy nhất hay giữ tất cả trong hàng đợi?

## GRR-146 · `INVALID_TRANSITION_UNDEFINED` · Trung bình — sự kiện đến từ ghế đã bị loại khỏi VCNV

- **Mô tả**: nguồn định nghĩa **trạng thái "bị loại khỏi phần thi này"** nhưng không định nghĩa outcome của các sự kiện đến từ ghế đang ở trạng thái đó: đáp án hàng ngang, tín hiệu chọn hàng ngang, tín hiệu "Mở chướng ngại vật". Vì `Đ-7.a` bỏ drop, các sự kiện này bắt buộc phải có outcome. Người đã bị loại tiếp tục gửi đáp án hàng ngang; admin thấy đáp án đó trên màn chấm và bấm Đúng ⇒ **+10 cho người đã bị loại**.
- **Rule**: R-VCNV-01, R-VCNV-03, R-VCNV-04.
- **File nguồn**: F26 §VCNV · `game-rules-inventory.md` §R-VCNV-04 · `game-rules-decisions.md` §5.1 `Đ-7.a`.
- **Tác động**: cộng điểm cho người không còn quyền; hoặc ngược lại, tín hiệu bị bỏ im lặng — trái nguyên tắc "không drop".
- **Ranh giới**: GRR-022 hỏi *phạm vi quyền* của người bị loại; GRR-123 hỏi tín hiệu sai lượt. Đây là *outcome của sự kiện đến từ trạng thái đã bị vô hiệu*.
- **❓ Câu hỏi**: Sự kiện đến từ ghế đã bị loại khỏi VCNV bị từ chối ở server, hay vẫn được ghi nhận và hiển thị cho admin phán quyết?

---

# PHẦN 3 — TĂNG TỐC

## GRR-147 · `CONFLICT` · Cao — hai quyết định đã chốt mô tả hai đối tượng dedup/revert khác nhau

> Mục này gộp hai phát hiện độc lập của hai người review — cùng một gốc mâu thuẫn, hai kịch bản.

- **Mô tả**: `§3.3 [GRR-071]` dedup ở server theo `(câu, thí sinh, loại phán quyết)` — **có chiều thí sinh**; `§3.4 [Đ-5.3.1]` chốt *"một câu = MỘT event điểm cho TOÀN BỘ người chơi"* — event **không có chiều thí sinh**. Hai quyết định cùng match một thao tác.
  - **Kịch bản 1 — chấm lần đầu**: admin bấm Đúng cho A (sinh event bảng xếp hạng #1), rồi bấm Đúng cho B (bảng phải tính lại). Event #2 là **bản thay thế** của #1 hay là **event thứ hai cộng dồn**; và bấm Đúng cho A lần nữa thì dedup theo khoá nào.
  - **Kịch bản 2 — đổi phán quyết sau khi chốt câu**: câu 2 đã chốt (A 40, B 30, C 20), MC phán lại B thực ra sai. `§3.3` nói *"bấm Sai sau Đúng = revert + event mới"*, nhưng đối tượng revert theo `Đ-5.3.1` là **cả bảng của câu**. Revert theo B thì C không lên bậc (sai F26 *"nhanh thứ 3"*); revert cả bảng thì phải sinh lại điểm cho A và C.
- **Rule**: R-TT-01, R-TT-02, R-GEN-03, R-GEN-07.
- **File nguồn**: `game-rules-decisions.md` §3.3 `[GRR-071]`, §3.4 `[Đ-5.3.1]`, §7.1 `Đ-5.3` · `game-rules-inventory.md` §R-TT-01.
- **Tác động**: cộng dồn hoặc mất điểm ở vòng có giá trị 40/30/20/10 cho cả bảng. `Đ-5.3.1` được chốt để đóng GRR-039/GRR-114, nhưng chỉ đóng cho **thứ tự chấm lần đầu**, không cho **đổi phán quyết sau khi chốt câu** — ca thường gặp nhất khi có khiếu nại.
- **❓ Câu hỏi**: Ở vòng xếp hạng, mỗi lần admin bấm Đúng/Sai sinh một event **thay thế toàn bộ bảng** của câu đó, hay dedup và revert vẫn tính theo **từng thí sinh**?

## GRR-148 · `CONCURRENCY_UNDEFINED` · Cao — chấm chồng lên cửa sổ nhận đáp án

- **Mô tả**: không rule nào cấm admin **bắt đầu chấm trước khi hết giờ**, trong khi R-TT-03 cho thí sinh gửi lại tự do đến hết giờ và tính **bản cuối**. Đến giây 15/20 admin đã thấy đáp án của A và bấm Đúng; giây 18 A gửi bản mới (sai) — bản cuối là bản sai, nhưng event điểm đã sinh trên bản cũ. Tập "người trả lời đúng" — biến số quyết định **toàn bảng** theo `Đ-5.3.1` — được chốt tại một thời điểm không được định nghĩa.
- **Rule**: R-TT-01, R-TT-03, R-GEN-03.
- **File nguồn**: F26 §Tăng tốc · `game-rules-inventory.md` §R-TT-03 · `CLAUDE.md` §UX (*"nhận MỌI lần trả lời đến khi hết giờ, tính BẢN CUỐI CÙNG"*) · `game-rules-decisions.md` §3.4.
- **Tác động**: điểm chấm trên một đáp án **không phải bản được luật công nhận**.
- **Ranh giới**: `[GRR-056]` ("không kết thúc câu sớm") đã chốt cho **Về đích**, không nói về thao tác chấm ở Tăng tốc.
- **❓ Câu hỏi**: Thao tác chấm ở Tăng tốc có bị khoá cho tới khi server-timeout của câu đóng không?

## GRR-149 · `ORDER_DEPENDENT` · Trung bình — chạy lại vòng: chỉ số vị trí câu và bảng 20/20/30/30

- **Mô tả**: thời gian Tăng tốc gắn với **vị trí câu trong vòng** (20/20/30/30), không gắn với câu. `Đ-5.1` cho admin **chạy lại vòng**, `Đ-5.2f` giữ "câu đã dùng không trả lại pool" ⇒ lần chạy thứ hai dùng bộ câu khác. Không rule nào nói chỉ số vị trí được **reset về 1** hay **tiếp tục từ 5** — không có giá trị thời gian nào định nghĩa cho vị trí 5, hoặc câu dạng "nhìn nhanh" bị cấp 30 giây.
- **Rule**: R-TT-01, R-GEN-02.
- **File nguồn**: F26 §Tăng tốc · `game-rules-decisions.md` §6.3 `Đ-5.1`, `Đ-5.2f`.
- **Tác động**: thời gian suy nghĩ sai luật ở vòng có chênh lệch thời gian lớn nhất (20 vs 30 giây).
- **Ranh giới**: GRR-075 hỏi "thời gian theo vị trí hay theo câu" ở trạng thái bình thường, không ở nhánh chạy lại vòng.
- **❓ Câu hỏi**: Khi chạy lại vòng Tăng tốc, chỉ số vị trí câu (và bảng thời gian gắn với nó) được đặt lại từ đầu hay tiếp tục đếm?

---

# PHẦN 4 — VỀ ĐÍCH

## GRR-150 · `INVALID_TRANSITION_UNDEFINED` · Cao — chuông đến TRƯỚC khi cửa sổ cướp mở

- **Mô tả**: cửa sổ cướp 5 giây chỉ mở khi người thi chính **sai** — mà "sai" chỉ tồn tại khi **admin bấm Sai** (`Đ-1`). Giữa lúc hết giờ và lúc admin bấm có thể có nhiều giây (MC hội ý). Queue ở Về đích **KHÔNG chặn**, *"có chuông là tính ngay"* (`Đ-7.2`), và Về đích **không có** hình phạt bấm trước hiệu lệnh (`Đ-6.2` chốt hình phạt đó **chỉ ở Câu hỏi phụ**). ⇒ thí sinh giữ chuột bấm liên tục từ trước khi admin phán quyết **luôn là tín hiệu đầu tiên**, **luôn thắng cướp quyền**.
- **Rule**: R-VD-05, R-VD-03.
- **File nguồn**: F26 §Về đích · `game-rules-decisions.md` §5.2 `Đ-7.2`, §8.3 `Đ-6.2`, §3.1 `Đ-1`.
- **Tác động**: **vô hiệu hoá cơ chế tranh chuông** của vòng quyết định điểm cao nhất trận; thắng thua do thói quen bấm sớm.
- **Ranh giới**: GRR-042/GRR-043 hỏi thời gian trả lời sau khi cướp và việc mở lại cửa sổ, không hỏi tín hiệu đến **trước** khi cửa sổ mở.
- **❓ Câu hỏi**: Tín hiệu chuông đến trước thời điểm admin bấm Sai bị vô hiệu, hay vẫn được xếp vào hàng đợi cướp quyền theo thứ tự tới?

## GRR-151 · `IDEMPOTENCY_UNDEFINED` · Cao — bấm Sai lần thứ hai có khởi động lại cửa sổ 5 giây không

- **Mô tả**: `§3.3` chỉ định nghĩa idempotency cho **event điểm** (*"bấm Đúng hai lần = một event"*). Ở Về đích, thao tác chấm còn có **tác dụng phụ phi-điểm**: bấm Sai **mở cửa sổ cướp 5 giây**. Admin bấm Sai lúc t=0, bấm lại lúc t=2s do UI chưa cập nhật ⇒ cửa sổ kéo dài thành **7 giây**, tín hiệu đến ở giây thứ 6 trở nên hợp lệ.
- **Rule**: R-VD-05, R-GEN-03.
- **File nguồn**: F26 §Về đích (*"trong 5 giây"*) · `game-rules-decisions.md` §3.3.
- **Tác động**: cửa sổ tranh chuông dài hơn luật; đổi kết quả cướp quyền.
- **❓ Câu hỏi**: Lần bấm Sai thứ hai trên cùng một câu có khởi động lại cửa sổ cướp 5 giây không?

## GRR-152 · `AMBIGUOUS` · Cao — "số điểm của câu hỏi" ở câu có NSHV, vế NGƯỜI CƯỚP

- **Mô tả**: ở câu có Ngôi sao hy vọng, cụm *"số điểm của câu hỏi"* xuất hiện trong **ba mệnh đề với ba chủ thể**: người đặt NSHV đúng được **gấp đôi**; người cướp đúng *"giành được điểm từ thí sinh trả lời sai"*; người cướp sai bị trừ *"một nửa số điểm của câu hỏi"*. `[GRR-047]` đã chốt **phía người đặt** (trừ MỘT lần), **không** chốt phía người cướp. Câu 30 điểm có NSHV, người thi chính sai, B cướp đúng — B được **+30 hay +60**; B cướp sai — **−15 hay −30**.
- **Rule**: R-VD-05, R-VD-06.
- **File nguồn**: F26 §Về đích · `game-rules-inventory.md` §R-VD-05, §R-VD-06 (U-8) · `game-rules-decisions.md` §9.4 `[GRR-047]`.
- **Tác động**: sai tới **30 điểm** trong một thao tác, ở vòng cuối quyết định thứ hạng.
- **❓ Câu hỏi**: Với câu có NSHV, điểm cộng cho người cướp đúng và điểm trừ người cướp sai tính trên **giá trị gốc** của câu hay **giá trị đã nhân đôi**?

## GRR-153 · `ORDER_DEPENDENT` · Cao — `SCORE_ADJUST` rơi vào giữa hai lượt Về đích

- **Mô tả**: thứ tự lượt Về đích tính lại sau mỗi lượt theo điểm tại mốc đó, mốc *"hoàn thành phần thi"* đã chốt là **sau khi cửa sổ cướp của câu cuối đóng VÀ chấm xong** (`[GRR-041]`). Nhưng `SCORE_ADJUST` **không tự revert** và không bị ràng buộc thời điểm (`Đ-11.B`). Không rule nào nói kết quả khi `SCORE_ADJUST` rơi vào **sau khi recommendation đã được admin tiêu thụ** (đã gọi người cho lượt kế) nhưng **trước khi** lượt đó bắt đầu: admin sửa −10 cho A vì chấm nhầm ở Khởi động ⇒ theo luật, lượt 3 phải là B chứ không phải A đã được gọi lên.
- **Rule**: R-VD-02, R-GEN-07.
- **File nguồn**: F26 §Về đích (*"Điểm số được tính tại thời điểm sau khi thí sinh ở lượt 1 hoàn thành phần thi"*) · `game-rules-decisions.md` §6.3 `Đ-11.B`, §9.4 `[GRR-041]`.
- **Tác động**: thứ tự lượt sai luật, ảnh hưởng dây chuyền tới các lượt sau và tới việc ai được chọn gói câu còn lại.
- **Ranh giới**: GRR-116 hỏi nhánh REVERT của event vòng trước, không hỏi `SCORE_ADJUST` thủ công xen giữa hai lượt.
- **❓ Câu hỏi**: `SCORE_ADJUST` phát sinh sau khi lượt kế tiếp đã được chọn có làm hệ thống cảnh báo và tính lại recommendation không, hay lượt đã chọn được giữ nguyên?

## GRR-154 · `INVALID_TRANSITION_UNDEFINED` · Trung bình — người thi chính bấm chuông cướp câu của chính mình

- **Mô tả**: không rule nào định nghĩa outcome của đáp án / tín hiệu chuông đến từ ghế **không đang có quyền** ở Về đích: thí sinh chưa tới lượt gửi đáp án, hoặc **chính người thi chính** bấm chuông trong cửa sổ cướp câu của mình (luật ghi rõ *"1 trong 3 thí sinh còn lại"*). Vì không có drop, các sự kiện này phải đi đâu đó ⇒ tín hiệu của người thi chính là **đầu tiên** trong hàng đợi, admin xử lý theo thứ tự tới và trao quyền cho **chính người vừa trả lời sai**.
- **Rule**: R-VD-03, R-VD-05.
- **File nguồn**: F26 §Về đích · `game-rules-decisions.md` §5.1 `Đ-7`, §5.2 `Đ-7.2`.
- **Tác động**: trao quyền cướp cho người không đủ điều kiện ⇒ sai điểm và sai luật gốc.
- **Ranh giới**: GRR-123 hỏi tín hiệu sai lượt ở **VCNV** — nơi queue CHẶN và admin duyệt; ở Về đích queue **KHÔNG chặn** nên tín hiệu có hiệu lực ngay, hệ quả khác hẳn.
- **❓ Câu hỏi**: Tín hiệu chuông từ chính người thi chính trong cửa sổ cướp câu của mình bị server từ chối, hay vẫn vào hàng đợi để admin reject?

---

# PHẦN 5 — CÂU HỎI PHỤ / TIE-BREAK

## GRR-155 · `UNREACHABLE` · Cao — bỏ vòng Về đích làm tiền đề kích hoạt tie-break biến mất

- **Mô tả**: F26 §Câu hỏi phụ: *"**Sau phần thi Về đích**, các thí sinh có cùng số điểm sẽ bước vào phần thi Câu hỏi phụ."* `Đ-5.1` cho admin bỏ hẳn một vòng. Nếu Về đích bị bỏ, mốc *"sau phần thi Về đích"* **không bao giờ xảy ra** ⇒ điều kiện vào `TIE_BREAK` không có tiền đề, dù cả sân đang hoà. Danh sách hệ quả của `Đ-5.1` mới liệt **bỏ Tăng tốc** (`Đ-5.1d`) và **bỏ Khởi động** (`Đ-5.1e`) — thiếu đúng vòng cuối.
- **Rule**: R-TB-01, `Đ-5.1`.
- **File nguồn**: F26 §Câu hỏi phụ · `game-rules-open-questions.md` §7.3 (`Đ-5.1d`, `Đ-5.1e`).
- **Tác động**: trận hoà không có đường phân định, hoặc phải phát minh mốc mới.
- **Ranh giới**: GRR-121/GRR-122 chỉ bàn tie-break **sau khi đã kích hoạt**.
- **❓ Câu hỏi**: Khi vòng Về đích bị bỏ hoặc không nằm trong playlist, điều kiện kích hoạt Câu hỏi phụ đo tại mốc nào?

## GRR-156 · `MISSING` · Cao — kết quả tie-break không nằm trong mô hình "điểm = event log"

- **Mô tả**: `[GRR-058]` chốt người thắng Câu hỏi phụ **không được cộng điểm**, chỉ đổi thứ hạng. Vậy thứ hạng chung cuộc = hàm của (điểm, kết quả tie-break), nhưng `Đ-5.3` chỉ định nghĩa **điểm** là hàm của event log. Không rule nào nói kết quả tie-break được **ghi bằng vật gì**, có phải event không, có revert được không. Hệ quả: admin revert một event Về đích sau khi tie-break xong (GRR-121 đề xuất giữ nguyên kết quả) ⇒ biên bản có "người thắng tie-break" là người **thua điểm**, không có quy tắc nào nói cái nào thắng khi xuất PDF.
- **Rule**: R-TB-01, R-TB-02, R-TB-04, `Đ-5.3`.
- **File nguồn**: `game-rules-decisions.md` §9.5 `[GRR-058]`, §7.1 · `game-rules-inventory.md` §R-TB-02.
- **Tác động**: không xác định được **kết quả trận** — đầu ra cuối cùng của cả hệ thống.
- **Ranh giới**: GRR-057/GRR-065 bàn thứ hạng của nhóm **không được phân định**; đây là vật ghi và độ ưu tiên của nhóm **đã được phân định**.
- **❓ Câu hỏi**: Kết quả tie-break được ghi thành event của trận và có bị revert theo điểm không, hay là một phán quyết cấp trận độc lập luôn thắng so sánh điểm?

## GRR-157 · `IDEMPOTENCY_UNDEFINED` · Trung bình — bốc thăm không tất định, không tái dựng được

- **Mô tả**: hết 3 câu vẫn hoà ⇒ hệ thống random, admin xác nhận (R-TB-04). Admin bấm bốc lại ⇒ lần 2 ra người khác. Không rule nào nói kết quả nào có hiệu lực, cả hai lần có vào lịch sử append-only không. Nặng hơn: R-KD-07 nói draw đề phát `QUESTIONS_DRAWN` để **replay được**, bốc thăm tie-break **không có phát biểu tương tự** ⇒ một nguồn ngẫu nhiên không ghi lại làm trận **không tái dựng được**, trái `Đ-5.3`.
- **Rule**: R-TB-04, R-KD-07, `Đ-5.3`.
- **File nguồn**: `game-rules-inventory.md` §R-TB-04, §R-KD-07.
- **Tác động**: tranh chấp không phân xử được ở đúng thao tác quyết định người vô địch.
- **Ranh giới**: GRR-062 hỏi *có được bốc lại không*; đây là *ghi nhận và tính tất định của kết quả bốc*.
- **❓ Câu hỏi**: Mỗi lần bốc thăm có ghi thành một event bất biến (kèm kết quả) không, và nếu bốc lại thì lần nào có hiệu lực?

## GRR-158 · `CONFLICT` (dạng 17 — scope creep) · Trung bình — tie-break đa vị trí / đa nhóm vượt xa luật gốc

- **Mô tả**: F26 chỉ mô tả Câu hỏi phụ để chọn ra **một** người thắng. Repo đang đặc tả song song: `tieBreakPositions` config (D13.5), R-TB-05 chạy tie-break **per nhóm hoà theo thứ tự vị trí**, `Đ-10.7` recommend **mọi** nhóm hoà, `Đ-10.7b` cho admin thêm cả người **không hoà**. Mỗi lớp mở rộng lại đẻ ra một câu hỏi luật không có nguồn: thứ tự nhóm (GRR-063), pool dùng chung (GRR-064), thứ hạng người thua (GRR-065), ngưỡng ≥2 người (`Đ-15.3` còn nháp), toàn sân cùng điểm (GRR-122). **Không mục nào hỏi liệu bản thân việc mở rộng có cần cho v1 không.**
- **Rule**: R-TB-01, R-TB-05, K-14, `Đ-10.7`.
- **File nguồn**: F26 §Câu hỏi phụ · `game-rules-inventory.md` §R-TB-01, §R-TB-05, §8A K-14.
- **Tác động**: 5+ câu hỏi luật đang mở đều là **hệ quả của một tính năng không có trong nguồn**. Đây là mục duy nhất trong lượt 3 **giảm** phạm vi thay vì thêm câu hỏi.
- **❓ Câu hỏi**: v1 có cần tie-break cho vị trí khác vị trí nhất không, hay `tieBreakPositions` cố định `[1]` và các vị trí hoà còn lại ghi đồng hạng?

---

# PHẦN 6 — ĐIỀU KHIỂN TRẬN VÀ EVENT LOG

## GRR-159 · `ORDER_DEPENDENT` · Cao — `SCORE_ADJUST` sống sót qua "bỏ vòng", để lại điểm mồ côi

- **Mô tả**: admin chấm nhầm câu 4 của B ở Khởi động rồi sửa bằng `SCORE_ADJUST {+10}`. Sau đó bỏ hẳn vòng Khởi động. Theo `Đ-11.B`, event điểm của vòng bị revert nhưng `SCORE_ADJUST` **KHÔNG tự revert** (*"đó là phán quyết của người"*) ⇒ B giữ **+10 thuộc về một vòng không còn tồn tại**. Làm ngược thứ tự (bỏ vòng trước, chỉnh sau) thì +10 là một điều chỉnh độc lập hợp lệ. **Cùng hai thao tác, hai kết quả điểm, chỉ khác thứ tự.**
- **Rule**: R-GEN-07, `Đ-5.2`, `Đ-5.3`, `Đ-11.B`.
- **File nguồn**: `game-rules-inventory.md` §R-GEN-07 · `game-rules-decisions.md` §6.3 (bảng *"`SCORE_ADJUST` của admin — KHÔNG tự revert"*), §7.1.
- **Tác động**: điểm cuối trận sai **mà không có dấu vết mâu thuẫn nào trong log** — cả hai event đều hợp lệ.
- **Ranh giới**: GRR-072 (undo steal 2 ghế) và GRR-117 (trạng thái phi-điểm) không đụng `SCORE_ADJUST` xuyên vòng.
- **❓ Câu hỏi**: `SCORE_ADJUST` gắn với một vòng cụ thể (⇒ bị revert cùng vòng) hay là điều chỉnh cấp trận độc lập với mọi thao tác bỏ vòng?

## GRR-160 · `IDEMPOTENCY_UNDEFINED` · Cao — bỏ vòng / chạy lại vòng bấm hai lần

- **Mô tả**: mạng lag, admin bấm "Bỏ vòng VCNV" hai lần (hoặc hai tab). `Đ-5.3` định nghĩa revert = **thêm event đảo ngược**; hai lần bấm ⇒ **hai bộ event đảo ngược** ⇒ điểm VCNV bị trừ hai lần, thí sinh xuống âm (`Đ-2` cho phép âm nên **không có sàn nào chặn lại**). Không rule nào nói lần bấm thứ hai là no-op.
- **Rule**: `Đ-5.1`, `Đ-5.2`, `Đ-5.3`, `§3.3`.
- **File nguồn**: `game-rules-decisions.md` §3.3 (dedup **chỉ** cho thao tác chấm), §6.3, §7.1.
- **Tác động**: sai điểm im lặng ở **thao tác phá huỷ nhất của hệ thống**; `CLAUDE.md` §UX còn cấm disable nút ⇒ bấm lại là hành vi được khuyến khích.
- **❓ Câu hỏi**: Dedup của §3.3 có mở rộng cho revert / bỏ vòng / chạy lại (khoá theo `(vòng, loại thao tác)`) hay mỗi lần bấm sinh một revert mới?

## GRR-161 · `INVALID_TRANSITION_UNDEFINED` · Cao — chấm một câu thuộc vòng đã bị bỏ

- **Mô tả**: admin bỏ vòng Khởi động (điểm đã revert), nhưng màn chấm còn treo 3 submission chưa phán quyết của vòng đó. Theo `§1.2` hệ thống advisory **không chặn cứng**; admin bấm Đúng ⇒ sinh **event điểm mới** cho một vòng đang mang nhãn "đã bỏ" ⇒ **điểm sống lại** trong khi biên bản vẫn ghi vòng đã bỏ. Không rule nào nói thao tác chấm bị vô hiệu sau khi vòng bị bỏ, cũng không nói nó bị chặn hay chỉ cảnh báo.
- **Rule**: R-GEN-03, `Đ-5.1`, `Đ-5.2d`, `Đ-11.B`.
- **File nguồn**: `game-rules-decisions.md` §6.3, §1.2-1.3 · `game-rules-inventory.md` §R-GEN-03.
- **Tác động**: mâu thuẫn trực tiếp giữa bảng điểm và biên bản; **cách đơn giản nhất để phá bất biến** "điểm = reduce(event của các vòng còn hiệu lực)".
- **❓ Câu hỏi**: Sau khi một vòng bị bỏ, các submission chưa chấm của vòng đó còn chấm được không, và nếu admin vẫn chấm thì event sinh ra có hiệu lực với điểm không?

## GRR-162 · `MISSING` · Cao — nhánh else của nguyên tắc advisory: admin KHÔNG BAO GIỜ bấm

- **Mô tả**: hết vòng Tăng tốc, 4 submission của câu 4 chưa được bấm Đúng/Sai. R-GEN-03 nói *"chưa chấm ⇒ không sinh điểm"*. Không rule nào nói **vòng có được kết thúc khi còn câu chưa chấm không**, các submission đó xử lý ra sao (bỏ, treo, cảnh báo), và điểm câu đó có vĩnh viễn = 0 cho mọi người không. Cùng vấn đề ở Khởi động lượt chung khi có người bấm chuông rồi im lặng — hình phạt **−5** của F26 không có ai kích hoạt.
- **Rule**: R-GEN-03, R-KD-03, `Đ-1`, `Đ-5`.
- **File nguồn**: `game-rules-inventory.md` §R-GEN-03 (*"Kết quả khi admin chưa bấm: câu chưa được chấm"*) · `game-rules-decisions.md` §1.2.
- **Tác động**: kết quả trận phụ thuộc việc admin có quên hay không, mà hệ thống không có phát biểu nào về trạng thái "câu chưa phán quyết khi vòng đóng" — **chặn viết acceptance criteria cho mọi vòng**.
- **Ranh giới**: GRR-068 hỏi *ai kích hoạt* hình phạt −5; chưa mục nào hỏi *điều gì xảy ra khi không ai kích hoạt cho tới hết vòng*.
- **❓ Câu hỏi**: Khi kết thúc một vòng còn câu chưa được chấm, các câu đó được coi là bỏ (0 điểm cho mọi người), bị chặn không cho kết thúc vòng, hay chỉ cảnh báo cho admin?

## GRR-163 · `IDEMPOTENCY_UNDEFINED` · Trung bình — "kết thúc vòng" bấm hai lần

- **Mô tả**: `Đ-7.b`: hàng đợi đang hoạt động **reset sau mỗi VÒNG**. Admin bấm "kết thúc vòng" hai lần do lag: lần 2 hoặc là no-op, hoặc **kết thúc luôn vòng kế vừa được mở** (nuốt cả một vòng), hoặc reset lần nữa hàng đợi đã có tín hiệu mới của vòng kế. Không rule nào nói hành vi lặp lại của thao tác chuyển vòng.
- **Rule**: `Đ-7.b`, `Đ-5`, `Đ-11.B`.
- **File nguồn**: `game-rules-decisions.md` §5.1, §6.1.
- **Tác động**: mất tín hiệu hợp lệ của vòng kế tiếp, hoặc bỏ qua một vòng mà không ai chủ ý.
- **❓ Câu hỏi**: Thao tác chuyển/kết thúc vòng có gắn định danh vòng để lần bấm lặp lại thành no-op không?

## GRR-164 · `INVALID_TRANSITION_UNDEFINED` · Trung bình — bắt đầu vòng B khi vòng A chưa kết thúc

- **Mô tả**: `Đ-5` cho admin *"chọn vòng nào bắt đầu"*, không kèm điều kiện về vòng đang chạy. Admin bấm bắt đầu Tăng tốc trong lúc VCNV còn 2 hàng ngang chưa hỏi và một tín hiệu "Mở chướng ngại vật" đang chờ duyệt. Mô hình `rounds[i]` **hàm ý** một vòng hoạt động nhưng **không phát biểu**; không rule nào nói VCNV được coi là đã kết thúc, bị treo, hay quay lại được, và tín hiệu đang chờ duyệt còn hiệu lực không.
- **Rule**: §Trạng thái game, `Đ-5`, `Đ-7.b`.
- **File nguồn**: `game-rules-inventory.md` §Trạng thái game · `game-rules-decisions.md` §6.1.
- **Tác động**: quyết định vòng bị cắt có được "chạy lại" (`Đ-5.1`) hay là một vòng dở dang không nhãn; ảnh hưởng biên bản và pool đề.
- **Ranh giới**: GRR-119 hỏi cùng dạng ở cấp **lượt trong một vòng**, không ở cấp **vòng**.
- **❓ Câu hỏi**: Bắt đầu một vòng mới có tự động kết thúc vòng đang chạy không, và một trận có được có nhiều vòng ở trạng thái đang chạy không?

---

# PHẦN 7 — HẠ TẦNG TRẬN (PAUSE · RECONNECT · ADMIN)

## GRR-165 · `MISSING` · Cao — `PAUSED` không có đường thoát được đặc tả

- **Mô tả**: admin mất kết nối > N giây ⇒ auto-pause. Nếu admin không quay lại (máy hỏng) và phòng không có admin thứ hai, không rule nào nói **ai resume được**, có ai khác được cấp quyền không, hay trận kẹt vĩnh viễn ở `PAUSED`. Thêm: `PAUSED` được liệt kê **ngang hàng** với `rounds[i]` / `INTERMISSION` / `TIE_BREAK`, nên khi pause xảy ra giữa `TIE_BREAK`, không có phát biểu nào nói resume quay về `TIE_BREAK` (state cũ lưu ở đâu, có lưu cả pha nào trong luồng 8 phase của `Đ-9` không).
- **Rule**: R-GEN-08, §Trạng thái game, `Đ-9`.
- **File nguồn**: `game-rules-inventory.md` §Trạng thái game, §R-GEN-08.
- **Tác động**: "state không có đường thoát" ở đúng cơ chế được thiết kế để **cứu sự cố**.
- **Ranh giới**: GRR-077 hỏi `INTERMISSION`; GRR-076/GRR-079 hỏi event và timer **trong lúc** PAUSED. Chưa mục nào hỏi **cấu trúc lồng và điều kiện thoát**.
- **❓ Câu hỏi**: `PAUSED` là một trạng thái riêng hay một cờ phủ lên trạng thái đang chạy, và ai được resume khi admin gây ra auto-pause không quay lại?

## GRR-166 · `CONCURRENCY_UNDEFINED` · Cao — auto-pause kích hoạt đúng lúc timer hết giờ

- **Mô tả**: cửa sổ cướp 5 giây còn **20 ms**; admin đã mất kết nối và ngưỡng N giây chạm đúng khoảnh khắc đó. Auto-pause *"đóng băng deadline, lưu `remainingMs`"*; timeout của server đáng lẽ đóng cửa sổ. Hai sự kiện server cùng mốc, không có thứ tự quy định ⇒ hoặc cửa sổ đóng (không ai cướp được), hoặc cửa sổ **được nối lại 20 ms sau khi resume vài phút sau** — lúc đó thí sinh đã biết đáp án.
- **Rule**: R-GEN-08, R-VD-05, R-GEN-05.
- **File nguồn**: `game-rules-inventory.md` §R-GEN-08, §R-GEN-05.
- **Tác động**: thay đổi trực tiếp điểm. Tiền lệ `Đ-6.4c` (*"timer chạy hết, không pause"*) mâu thuẫn ngầm với việc auto-pause đóng băng mọi timer.
- **❓ Câu hỏi**: Khi auto-pause và một timeout cùng đến, sự kiện nào được áp trước, và cửa sổ đã hết giờ trước khi pause có được mở lại khi resume không?

## GRR-167 · `IDEMPOTENCY_UNDEFINED` · Trung bình — hai nguyên nhân pause chồng nhau

- **Mô tả**: admin pause thủ công để xử lý khiếu nại, rồi rớt mạng ⇒ auto-pause kích hoạt trên một trận **đã** pause. Admin quay lại, hệ thống tự resume (vì nguyên nhân auto đã hết) ⇒ **trận chạy tiếp trong khi khiếu nại chưa xử xong**. Không rule nào nói pause là boolean hay bộ đếm nguyên nhân, resume có cần khớp nguyên nhân không.
- **Rule**: R-GEN-08 (4 nguyên nhân a-d).
- **File nguồn**: `game-rules-inventory.md` §R-GEN-08.
- **Tác động**: trận chạy tiếp ngoài ý muốn khi thí sinh chưa sẵn sàng; input mở lại giữa lúc không ai điều khiển.
- **❓ Câu hỏi**: Khi nhiều nguyên nhân pause cùng tồn tại, resume yêu cầu tất cả nguyên nhân đã hết hay một thao tác resume của admin là đủ?

## GRR-168 · `CONCURRENCY_UNDEFINED` · Cao — không rule nào nói chỉ có MỘT admin đang điều khiển

- **Mô tả**: mô hình ba tầng gọi admin là *"cơ cấu chấp hành **DUY NHẤT**"* — nhưng đó là phát biểu về **vai trò**, không phải về **số lượng phiên**; `CLAUDE.md` §Mô hình truy cập cho nhiều tài khoản admin. Admin 1 bấm Đúng cho seat B, admin 2 bấm Sai cho seat B trong cùng 300 ms. `§3.3` dedup theo `(câu, thí sinh, **loại phán quyết**)` ⇒ hai loại khác nhau, **cả hai đều được ghi**, kết quả cuối phụ thuộc thứ tự tới. Cùng dạng cho hai admin cùng bấm start timer / cùng bấm bỏ vòng.
- **Rule**: R-GEN-03, `Đ-1`, `§3.3`, mô hình ba tầng §1.1.
- **File nguồn**: `game-rules-decisions.md` §1.1, §3.3 · `CLAUDE.md` §Mô hình truy cập.
- **Tác động**: **mọi bảo đảm idempotency của §3.3 giả định một người bấm**; không có khoá điều khiển nào được đặc tả.
- **Ranh giới**: S-1 (`product-discovery.md`) chỉ hỏi **ai có quyền**, không hỏi **bao nhiêu người cùng lúc**.
- **❓ Câu hỏi**: Một trận có tối đa một admin đang điều khiển tại một thời điểm không, và nếu có nhiều thì phán quyết mâu thuẫn giữa hai admin được phân xử thế nào?

## GRR-169 · `CONCURRENCY_UNDEFINED` · Trung bình — grace 120s hết đúng lúc đáp án đang tới

- **Mô tả**: TS C rớt mạng ở giây **119.9** của grace; gói tin đáp án Tăng tốc của C (gửi trước khi rớt) tới server ở **120.05s**, sau khi ghế đã bị xử theo `dropoutPolicy`. Không rule nào nói đáp án đã nhận của một ghế bị xử lý dropout còn hiệu lực không, và ghế đó có còn trong **tập xếp hạng** của câu đang chạy không — ảnh hưởng bậc điểm của **những người khác** theo R-TT-01.
- **Rule**: R-GEN-09, R-TT-01, R-TT-03.
- **File nguồn**: `game-rules-inventory.md` §R-GEN-09, §R-TT-03.
- **Tác động**: thay đổi bậc điểm của **cả bảng** ở một câu, không chỉ của người rớt.
- **Ranh giới**: GRR-110 đề xuất "cửa sổ chạy tiếp, không pause", không nói dữ liệu đã ghi của người rớt.
- **❓ Câu hỏi**: Khi hết grace, các submission đã nhận của thí sinh đó có bị vô hiệu không, và ghế đó còn tham gia xếp hạng của câu đang chạy dở không?

---

# PHẦN 8 — THI ĐỘI `[v2]`

## GRR-170 · `MISSING` · Thấp `[v2]` — NSHV 1 lần/đội nhưng mỗi thành viên có một lượt Về đích

- **Mô tả**: R-TEAM-04 chốt NSHV **1 lần/đơn vị điểm/trận**; R-TEAM-07 `individualTurnMode: 'all-members'` cho **mỗi thành viên một lượt** Về đích. Đội 3 người ⇒ 3 lượt, 9 câu, nhưng chỉ **1 ngôi sao**. Không rule nào nói ai trong đội được quyền đặt, ai quyết khi hai thành viên cùng muốn đặt ở lượt của mình, và cửa sổ đặt (đóng khi admin bấm hiển thị câu — `[GRR-048]`) áp cho lượt của ai.
- **Rule**: R-TEAM-04, R-TEAM-07, R-VD-06.
- **File nguồn**: `game-rules-inventory.md` §PHẦN 7.
- **Tác động**: tranh chấp trong đội không có quy tắc. GRR-093→GRR-098 không phủ giao của hai rule này.
- **❓ Câu hỏi**: Ở chế độ `all-members`, quyền dùng ngôi sao hy vọng duy nhất của đội thuộc về ai và được khoá tại thời điểm nào?

## GRR-171 · `CONFLICT` · Thấp `[v2]` — ai chọn người thi Câu hỏi phụ khi thi đội

- **Mô tả**: R-TEAM-06 nói **đội trưởng cử 1 người** bấm chuông ở `TIE_BREAK`; `Đ-9` phase 1 nói **admin chọn người tham gia**, `Đ-10.7` nói hệ thống recommend, admin chọn. Hai rule cùng match một thao tác và trao quyền cho **hai chủ thể khác nhau**; đội trưởng cử A còn admin đưa B vào thì không có quy tắc phân xử. R-TEAM-07 lại buộc *"đổi đại diện chỉ tại `INTERMISSION`"* — mà `TIE_BREAK` không đi qua `INTERMISSION` (điều kiện vào/ra của `INTERMISSION` còn treo ở GRR-077).
- **Rule**: R-TEAM-06, R-TEAM-07, `Đ-9`, `Đ-10.7`.
- **File nguồn**: `game-rules-inventory.md` §PHẦN 7 · `game-rules-decisions.md` §9.5.
- **Tác động**: hai chủ thể **cùng có quyền** trên một thao tác.
- **Ranh giới**: GRR-096 hỏi đội trưởng **không kịp** cử người; đây là **xung đột thẩm quyền**.
- **❓ Câu hỏi**: Ở tie-break thi đội, người bấm chuông của mỗi đội do đội trưởng cử hay do admin chọn ở phase 1?

---

# PHỤ LỤC A — Đã tự kiểm và LOẠI khỏi danh sách

Những mục dưới đây từng bị nghi là khiếm khuyết, nhưng **nguồn hoặc quyết định đã chốt có nói rõ** ⇒ không đưa vào báo cáo:

| Nghi vấn | Đã được trả lời ở |
|---|---|
| Biên t = 3.000s (bấm đúng mốc là trong hay ngoài) | GRR-002 |
| Điểm được phép âm, không sàn | `Đ-2` |
| NSHV sai + có người cướp đúng — số học phía người ĐẶT | `[GRR-047]` (phía người CƯỚP vẫn mở → **GRR-152**) |
| Thứ tự admin chấm Tăng tốc **lần đầu** | `Đ-5.3.1`, `[GRR-039]` (đổi phán quyết sau khi chốt vẫn mở → **GRR-147**) |
| Chuỗi gửi A→B→A ở Tăng tốc | `[GRR-033]` |
| Reject có làm thí sinh mất lượt không | `Đ-7` |
| Điều kiện mở ô trung tâm | `[GRR-013]` |
| Thang điểm CNV đếm theo hàng ĐÃ HỎI | `[GRR-019]` |
| Điểm lẻ khi −½ giá trị lẻ | `Đ-3` (nguồn phát sinh đã xác định; cách xử lý vẫn treo) |
| Ràng buộc undo "gần nhất / build-upon" | `Đ-5.3.X` (đã bỏ, mất đối tượng) |
| Thời gian câu thực hành 30s vs 20s | §10 (hai chủ thể: người thi chính vs người cướp) |

# PHỤ LỤC B — Bảng tra nhanh

| GR | Loại | Mức | Một dòng |
|---|---|---|---|
| 137 | IDEMPOTENCY | Cao | Start timer bấm hai lần |
| 138 | CONCURRENCY | Cao | Hai chuông cùng ms, queue không chặn |
| 139 | ORDER_DEPENDENT | Cao | "Hiển thị câu hỏi" vs "start timer" |
| 140 | IDEMPOTENCY | TB | Cùng ghế bấm chuông nhiều lần |
| 141 | INVALID_TRANSITION | TB | Chuông từ ghế đã mất quyền |
| 142 | ORDER_DEPENDENT | Cao | Băng điểm CNV: lúc bấm hay lúc duyệt |
| 143 | SIDE_EFFECT | Cao | Admin bấm No — cái gì đã xảy ra |
| 144 | CONCURRENCY | Cao | Hai loại tín hiệu một queue chặn |
| 145 | IDEMPOTENCY | Cao | "Mở CNV" bấm nhiều lần |
| 146 | INVALID_TRANSITION | TB | Sự kiện từ ghế đã bị loại VCNV |
| 147 | CONFLICT | Cao | Khoá dedup vs event toàn bảng |
| 148 | CONCURRENCY | Cao | Chấm trước khi hết giờ Tăng tốc |
| 149 | ORDER_DEPENDENT | TB | Chạy lại vòng: vị trí câu 20/20/30/30 |
| 150 | INVALID_TRANSITION | Cao | Chuông trước khi cửa sổ cướp mở |
| 151 | IDEMPOTENCY | Cao | Bấm Sai lần 2 restart cửa sổ 5s |
| 152 | AMBIGUOUS | Cao | Câu NSHV: người cướp ăn 30 hay 60 |
| 153 | ORDER_DEPENDENT | Cao | `SCORE_ADJUST` giữa hai lượt Về đích |
| 154 | INVALID_TRANSITION | TB | Người thi chính tự cướp câu mình |
| 155 | UNREACHABLE | Cao | Bỏ Về đích ⇒ tie-break mất tiền đề |
| 156 | MISSING | Cao | Kết quả tie-break ghi bằng vật gì |
| 157 | IDEMPOTENCY | TB | Bốc thăm không tất định |
| 158 | CONFLICT | TB | Tie-break đa vị trí — scope creep |
| 159 | ORDER_DEPENDENT | Cao | `SCORE_ADJUST` mồ côi sau bỏ vòng |
| 160 | IDEMPOTENCY | Cao | Bỏ vòng bấm hai lần ⇒ trừ hai lần |
| 161 | INVALID_TRANSITION | Cao | Chấm câu của vòng đã bỏ |
| 162 | MISSING | Cao | Admin không bao giờ bấm |
| 163 | IDEMPOTENCY | TB | Kết thúc vòng bấm hai lần |
| 164 | INVALID_TRANSITION | TB | Vòng B bắt đầu khi A chưa xong |
| 165 | MISSING | Cao | `PAUSED` không có đường thoát |
| 166 | CONCURRENCY | Cao | Auto-pause đụng timeout |
| 167 | IDEMPOTENCY | TB | Hai nguyên nhân pause chồng |
| 168 | CONCURRENCY | Cao | Nhiều admin cùng điều khiển |
| 169 | CONCURRENCY | TB | Grace 120s vs đáp án đang tới |
| 170 | MISSING | Thấp `[v2]` | NSHV 1 lần/đội vs all-members |
| 171 | CONFLICT | Thấp `[v2]` | Ai chọn người thi tie-break đội |
