# Game Rules — Đặc tả luật chơi Olympia Contest System

> **Ngày lập**: 2026-07-25 · **Phạm vi**: 37 rule GR-001 → GR-037, phủ 5 vòng thi + luật xuyên vòng.
>
> **Loại tài liệu**: đặc tả **nghiệp vụ**. Không chứa code, pseudocode, tên framework, tên bảng dữ liệu hay tên lớp.

## Quy ước mã

Hai tiền tố, hai loại đối tượng, **không còn trùng nhau**:

| Tiền tố | Nghĩa | Dải | Nằm ở |
|---|---|---|---|
| **`GR-`** | **Rule** — một quyết định nghiệp vụ | GR-001 → GR-037 | **File này** |
| **`GRR-`** | **Vấn đề** — một khiếm khuyết đặc tả được phát hiện khi review | GRR-001 → GRR-171 | `docs/reviews/game-rules-review-old.md` (GRR-001→136) · `docs/reviews/game-rules-review.md` (GRR-137→171) |

Tài liệu này vẫn giữ thói quen **kèm tên file khi trích một mã `GRR-`** để biết tra ở file review nào — ví dụ `game-rules-review.md` GRR-147.

## Quan hệ với các tài liệu khác

| File | Vai trò |
|---|---|
| `docs/source/fandom-olympia-26-luat-choi.md` | **Luật gốc O26 nguyên văn** — source of truth duy nhất |
| `docs/glossary.md` | **Thuật ngữ chuẩn** TERM-001 → TERM-060; file này dùng đúng tên ở đó |
| `docs/game-rules-inventory.md` | Kiểm kê R-*, các mục chưa định nghĩa U-*, các mâu thuẫn K-* |
| `docs/reviews/game-rules-decisions.md` | Quyết định **đã chốt** Đ-1 → Đ-15 |
| `docs/reviews/game-rules-open-questions.md` | Câu hỏi **còn chờ** chủ dự án quyết |
| `docs/reviews/game-rules-review.md` | Vấn đề lượt 3 (GRR-137 → GRR-171) |
| `docs/reviews/game-rules-review-old.md` | Vấn đề lượt 1-2 (GRR-001 → GRR-136) |

## Cách đọc Status

| Status | Nghĩa | Số rule |
|---|---|---|
| `CONFIRMED` | Mọi nhánh của rule đều có câu trả lời từ nguồn | **15** |
| `NEEDS CLARIFICATION` | Định nghĩa cốt lõi đã rõ, nhưng còn ≥1 nhánh chưa được nguồn quy định | **21** |
| `CONFLICT` | Hai nguồn cùng áp dụng nhưng cho outcome khác nhau; **tài liệu này không phân xử** | **1** |

> **Vì sao phần lớn rule vẫn `NEEDS CLARIFICATION`**: bốn loại khiếm khuyết `IDEMPOTENCY` · `CONCURRENCY` · `INVALID_TRANSITION` · `ORDER_DEPENDENT` chưa từng được rà cho tới lượt review thứ 3, nên hầu hết rule còn ít nhất một nhánh treo ở các mục **Evaluation order**, **Idempotency** hoặc **Concurrency**. Phần **Conditions**, **Decision table** hàng happy path và **Boundaries** của các rule đó vẫn dùng được để viết spec.
>
> Status là phép **AND**: chỉ cần một nhánh treo là cả rule treo. Đợt quyết định `Đ-16` → `Đ-31` (2026-07-25), cộng lượt rà suy luận cuối, đã đưa **14 rule** về `CONFIRMED`.

Các rule không mang trạng thái treo nào là điểm khởi đầu an toàn nhất:

| Mã | Tên | Status |
|---|---|---|
| GR-001 | Khởi động lượt riêng: chấm câu | `CONFIRMED` |
| GR-002 | Khởi động lượt riêng: hết thời gian suy nghĩ | `CONFIRMED` |
| GR-003 | Khởi động lượt chung: giành quyền bằng chuông | `CONFIRMED` |
| GR-004 | Khởi động lượt chung: chấm câu và hình phạt | `CONFIRMED` |
| GR-005 | Khởi động lượt chung: cửa sổ chuông rỗng | `CONFIRMED` |
| GR-006 | Khởi động: ghi nhận đáp án | `CONFIRMED` |
| GR-013 | Tăng tốc: xếp hạng tốc độ | `CONFIRMED` |
| GR-015 | Tăng tốc: ghi nhận bản cuối | `CONFIRMED` |
| GR-018 | Về đích: trả lời câu của mình | `CONFIRMED` |
| GR-022 | Câu hỏi phụ: điều kiện kích hoạt | `CONFIRMED` |
| GR-023 | Câu hỏi phụ: thể thức ba câu | `CONFIRMED` |
| GR-027 | Chuẩn hoá và highlight đáp án | `CONFIRMED` |
| GR-028 | Điểm là hàm của event log | `CONFIRMED` |
| GR-033 | Mốc thời gian do admin bấm | `CONFIRMED` |
| GR-034 | Chuông chỉ nhận click chuột | `CONFIRMED` |
| GR-037 | Phạm vi hiển thị đáp án | `CONFLICT` |

## Nguyên tắc nền áp cho MỌI rule

1. **Máy độc quyền SỰ KIỆN, người độc quyền PHÁN QUYẾT.** Mọi outcome "đúng/sai" trong tài liệu này là kết quả **SAU KHI admin bấm**, không phải kết quả hệ thống tự suy ra. Trong trận chính thức không tồn tại cơ chế tự chấm.
2. **Admin là cảm biến.** Mọi mốc thời gian mà luật gốc mô tả bằng hành vi của MC đều ánh xạ thành **một thao tác bấm của admin**. Mốc đó là **tuyệt đối** — không có ân hạn, không trừ bù độ trễ tay người.
3. **Không có cơ chế drop tín hiệu.** Mọi tín hiệu của thí sinh đều vào hàng đợi theo server timestamp. Hệ quả trực tiếp: **mọi sự kiện không hợp lệ vẫn bắt buộc phải có một outcome** — chỗ nào nguồn chưa quy định outcome đó, tài liệu này ghi `NEEDS CLARIFICATION` thay vì tự đặt ra.
4. **Hàng đợi chỉ CHẶN ở VCNV.** Khởi động lượt chung và cướp quyền Về đích: không chặn, có chuông là tính ngay.
5. **Server time là source of truth duy nhất.**
6. **Điểm là hàm của event log**; hoàn nguyên bằng cách **thêm event đảo ngược**, lịch sử linear và không bao giờ bị xoá.
7. **Điểm được phép âm, không có sàn.**
8. **Hệ thống chỉ CHẶN CỨNG ở ngưỡng bất khả thi vật lý**; mọi lệch luật khác chỉ **cảnh báo**, admin ép được qua dialog.
9. **Thao tác ở INVALID STATE không thực hiện được** — phân biệt rõ với điểm 8:

| Loại | Máy thí sinh | Máy admin | Admin ép được? |
|---|---|---|---|
| **Conflict LUẬT** (thi hai lần, đổi lượt, chạy lại vòng…) | — | **Dialog cảnh báo Yes/No** | **Có** |
| **INVALID STATE** (thao tác không tồn tại ở trạng thái hiện tại) | **Không hiển thị gì**, bấm **không phản hồi** ⇒ không có tín hiệu nào được tạo | **Toast** báo thao tác không hợp lệ, **không thực hiện được** | **Không** |

> Điểm 9 **không** mâu thuẫn điểm 3 (*"không có cơ chế drop"*): drop là **loại bỏ tín hiệu đã nhận**, còn ở đây **không tín hiệu nào được sinh ra**. Hàng đợi vẫn giữ nguyên nguyên tắc không drop đối với mọi tín hiệu thực sự tới server.

10. **Đồng hồ có hai vai trò bất đối xứng** — đây là lý do đứng sau điểm 9:

| Đối tượng | Đồng hồ làm gì |
|---|---|
| **Thí sinh** | **Khoá / mở khoá thao tác**: ngoài cửa sổ thì nút không hiển thị và không phản hồi |
| **Admin** | **KHÔNG khoá** — hết giờ vẫn bấm chấm được. **Hai ngoại lệ**: nút **start timer** tự khoá sau lần bấm đầu (chống bấm trùng), và ở vòng thí sinh **gõ đáp án** thì nút **chấm** khoá tới khi hết giờ (điểm 22) |

> Hệ quả: mốc thời gian của luật (3s, 15s, 20s…) là **ràng buộc đối với thí sinh** và là **mốc tham chiếu đối với người điều khiển**. "Trong hạn hay quá hạn" ở phía phán quyết là đánh giá của MC và admin, hệ thống không phân xử.

11. **KHÔNG tồn tại trạng thái "trận tạm dừng"** — mọi thao tác đều do admin thực hiện, nên khi trận cần dừng thì **admin ngừng thao tác**, hệ thống không cần biết. Không có đóng băng đồng hồ, không có tự động tạm dừng, không có trạng thái `PAUSED` trong máy trạng thái trận.

> Nếu sự cố xảy ra **giữa lúc một cửa sổ có ràng buộc thời gian đang mở** (Tăng tốc, cướp quyền Về đích) thì thí sinh mất thời gian thật. Van thoát là cơ chế đã có sẵn: admin **bỏ hoặc chạy lại vòng** (`Đ-5.1`) — lưu ý câu đã dùng **không** trả lại kho đề.

12. **Một câu chỉ đi qua ĐÚNG MỘT phán quyết** — sau khi admin bấm, **nút chấm bị khoá** và **nút "Câu kế tiếp" hiện lên**. Không có bấm nhầm, không có bấm lại, không có đổi phán quyết tại chỗ. Muốn sửa một phán quyết đã chốt thì đi qua hoàn nguyên event log (GR-028) hoặc điều chỉnh thủ công (GR-029).

> **Phán quyết KHÔNG phải lúc nào cũng nhị phân.** Tiêu chí: **chỉ nhị phân khi "Sai" trừ 0 điểm.** Chỗ nào "Sai" kéo theo hình phạt thì phải có lựa chọn thứ ba **"Huỷ kết quả"** — nếu không, admin bị ép chọn giữa cho điểm và phạt, trong khi tình huống có thể không đáng cả hai.

| Vòng / pha | "Sai" trừ bao nhiêu | Lựa chọn của admin |
|---|---|---|
| Khởi động — lượt riêng | **0** | Đúng / Sai |
| VCNV — hàng ngang | **0** | Đúng / Sai |
| Tăng tốc | **0** | Đúng / Sai |
| Về đích — người thi chính | **0** (chỉ mở cửa sổ cướp) | Đúng / Sai |
| Câu hỏi phụ | **0** | Đúng / Sai |
| **Khởi động — lượt chung** | **−5** | Đúng / Sai / **Huỷ kết quả** |
| **Về đích — người cướp quyền** | **−½ giá trị câu** | Đúng / Sai / **Huỷ kết quả** |
| **Về đích — câu có Ngôi sao hy vọng** | **−giá trị câu** | Đúng / Sai / **Huỷ kết quả** |

> Ngoài ra, **"Huỷ kết quả" luôn có mặt** khi câu **chỉ có bản gửi quá hạn** (điểm 21), kể cả ở vòng mà Sai trừ 0.
> Dù hai hay ba lựa chọn, vẫn là **một** phán quyết cho một câu — điểm 12 không đổi.

13. **Phán quyết là điều kiện để chuyển câu** — nút "Câu kế tiếp" **chỉ xuất hiện sau khi đã chấm** (chính là cơ chế của điểm 12) ⇒ không tồn tại trạng thái "vòng đã đóng mà còn câu chưa chấm" ở các vòng hỏi tuần tự.

> Khoá ở điểm 12 nằm tại **giao diện**. Theo `CLAUDE.md` §Zero-trust, server vẫn phải từ chối phán quyết lặp lại cho một câu đã chốt — khoá UI không thay thế kiểm tra ở server.

14. **Tín hiệu của thí sinh KHÔNG làm gián đoạn đồng hồ đang chạy** — tín hiệu được **ghi nhận ngay**, đồng hồ **chạy tiếp bình thường**. Cái bị hoãn là **việc HIỂN THỊ**, không phải thời gian: mọi đáp án chỉ lộ ra khi admin bấm hiển thị.

> Hệ quả thứ hai: **bản quá hạn cũng không tự lộ và cũng không bị máy loại thẳng**. Nó vào lịch sử trên màn admin kèm dấu **đỏ**; cả **việc hiển thị** lẫn **việc chấm điểm** đều là thao tác bấm của admin — máy chỉ đánh dấu, không phán quyết.
> Áp rõ nhất ở VCNV: có người bấm "Mở chướng ngại vật" giữa lúc đồng hồ hàng ngang chạy ⇒ ghi nhận, đồng hồ không dừng, và **cả hai** thứ sau **vẫn chưa hiện**: **đáp án chuẩn của chương trình** cho hàng ngang đó, và **bài làm của các thí sinh khác**. Ẩn cả hai mới đủ — hiện bất kỳ cái nào cũng cho người bấm CNV thêm dữ kiện mà luật không cho.

15. **Hai tín hiệu cùng mốc thời gian: hàng đợi tự quyết định, ngẫu nhiên** — không ưu tiên theo số ghế, vị trí hay bất kỳ tiêu chí nào. Áp ở nơi **quyền không chia được** (giành quyền trả lời).

> Ngẫu nhiên **lúc nhận**, **tất định khi dựng lại**: thứ tự hàng đợi chọn được ghi lại và không bao giờ xoá (điểm 3), nên replay event log vẫn ra kết quả cũ — `Đ-5.3` không bị phá.
> Khác với vòng xếp hạng (Tăng tốc): ở đó **điểm chia được**, nên cùng thời gian thì **cùng mức điểm** (K-8), không cần phân thứ tự.

16. **Nút chuông tự khoá ngay khi bấm** — frontend khoá **trong cùng thao tác đó, trước khi gửi tín hiệu** ⇒ một ghế chỉ phát được **một** tín hiệu chuông cho một câu. Khoá gắn với **một câu**; sang câu mới thì mở lại.

> Đây là **khoá-theo-luật-chơi**, không phải chống double-submit — `CLAUDE.md` §UX cho phép tường minh (*"chuông bị khoá khi sai, NSHV đã dùng, không tới lượt"*).
> Áp cho **mọi nút được xếp là chuông**, gồm cả nút **"Mở chướng ngại vật"** (`Đ-4.3`). **Không** áp cho thao tác **chọn hàng ngang** — đó không phải chuông; chọn hàng ngang có cơ chế riêng ở **điểm 23**.
> Server vẫn phải bỏ qua tín hiệu trùng nếu nhận được (`CLAUDE.md` §Zero-trust).

17. **"Hiển thị câu hỏi" và "start timer" là HAI thao tác riêng, thứ tự cố định** — hiển thị trước, start timer sau. Không gộp, không đảo.

| Mốc | Mở/đóng cái gì |
|---|---|
| **Admin bấm hiển thị câu hỏi** | Đưa câu lên màn thí sinh và viewer · **mở cửa sổ chuông** · **đóng** cửa sổ đặt Ngôi sao hy vọng |
| **Admin bấm start timer** | Mốc *"MC đọc xong"* · bắt đầu đếm thời gian suy nghĩ |

> Khoảng giữa hai mốc **chính là lúc MC đọc** — quãng mà luật cho phép bấm chuông ở Khởi động lượt chung.
> Hệ quả: **"bấm trước khi câu được đưa ra" không tồn tại** — trước mốc thứ nhất, màn thí sinh chưa có gì và chuông chưa sống.

18. **Chấm xong là KẾT THÚC CÂU** — không mở lại chuông cho người khác trên cùng một câu. Khi câu đóng: **xoá hàng đợi đang hoạt động** và **gỡ khoá chuông** cho mọi ghế, sẵn sàng cho câu mới.

> **Làm mịn `Đ-7.b`**: quyết định cũ nói hàng đợi đang hoạt động reset **sau mỗi VÒNG**; ở vòng có chuông thì reset **sau mỗi CÂU** — mịn hơn một bậc, không mâu thuẫn.
> "Xoá hàng đợi" chỉ đụng **hàng đợi đang hoạt động**. **Lịch sử tín hiệu không bao giờ bị xoá** (điểm 3) — gồm cả tín hiệu trơ của điểm 15; admin vẫn xem lại được sau khi câu đã đóng.

19. **LUẬT CHO BAO NHIÊU CÂU THÌ ĐÚNG BẤY NHIÊU** — hệ thống **không bao giờ tự sinh câu thứ N+1**. Khi đã hỏi đủ số câu quy định, nút "Câu kế tiếp" **chuyển thành nút kết thúc** (kết thúc lượt · kết thúc lượt chung · kết thúc vòng, tuỳ ngữ cảnh).

| Vòng | Số câu theo luật |
|---|---|
| Khởi động — lượt riêng | **6** câu mỗi thí sinh |
| Khởi động — lượt chung | **12** câu |
| VCNV | **4** hàng ngang + **1** ô trung tâm |
| Tăng tốc | **4** câu |
| Về đích | **3** câu mỗi gói |
| Câu hỏi phụ | **3** câu |

> ⇒ Biên *"lớn hơn max"* của mọi vòng là **KHÔNG TỒN TẠI** — không phải "chặn cứng", cũng không phải "cảnh báo". Không có đường vào thì không cần cả hai.
> **Không lẫn với `U-3`**: cấu hình `rowCount` 5-8 ở VCNV là **đổi con số của luật** (ngoài luật O26, thang điểm cho cấu hình đó chưa định nghĩa) — khác hẳn việc hệ thống tự đẻ thêm câu ngoài con số đã cấu hình.

20. **Kho đề được kiểm tại CỬA VÀO TỪNG VÒNG** — không đủ số câu thì **không bắt đầu được vòng đó**, nhưng **các vòng khác vẫn bắt đầu bình thường**.

> ⇒ **"Kho đề cạn giữa vòng" KHÔNG TỒN TẠI.** Nhu cầu của một vòng là con số cố định (điểm 19) và đã được kiểm đủ tại cửa vào; câu bị bỏ qua vẫn nằm trong con số đó. Chạy lại một vòng cũng phải qua đúng cửa đó.
> **Đây là chỗ chặn cứng thứ ba**, ngoài hai ngưỡng số người của `Đ-15.3` — và nó **ghi đè tiền lệ Athena** (bản cũ chỉ *cảnh báo* khi thiếu đề, không chặn). Lý do vẫn nằm trong tiêu chí cũ: một vòng không đủ câu thì **không chạy trọn được**, tức là vòng đó không còn nghĩa.

21. **LUÔN ghi nhận đáp án CUỐI CÙNG** — nút gửi **không bị khoá sau khi gửi**; thí sinh sửa và gửi lại bao nhiêu lần cũng được. Cửa duy nhất đóng lại là **hết giờ**.

| Nút | Sau khi bấm | Vì sao khác nhau |
|---|---|---|
| **Chuông** | **Tự khoá** (điểm 16) | Giành quyền là hành vi **một lần, không rút lại** |
| **Gửi đáp án** | **KHÔNG khoá** | Đáp án là thứ **sửa được**; chỉ đồng hồ mới đóng cửa |
| **Chọn hàng ngang** | **Dialog rồi khoá tạm** (điểm 23) | Một lần, không rút lại — **nhưng không đua tốc độ** |

> Đừng áp nhầm điểm 16 sang nút gửi — hai nút có ngữ nghĩa ngược nhau.
> **Bản rỗng không phải một đáp án**: bỏ qua, giữ bản hợp lệ trước đó (`Đ-20.1`) — nên gửi rỗng không xoá được bài đã làm.
> Phát biểu *"nếu không thay đổi thì ghi nhận đáp án đầu tiên"* của luật gốc **không phải ngoại lệ**: gửi một lần thì bản đầu chính là bản cuối.

**Khi có bản gửi quá hạn, hệ thống GIỮ CẢ HAI và để admin phán quyết:**

| Tình huống | Hệ thống làm gì | Admin chọn |
|---|---|---|
| Có bản **hợp lệ**, còn gửi thêm bản **quá hạn** | Giữ **cả hai**, bản quá hạn tô **đỏ** | **Đúng / Sai** |
| **Chỉ có** bản quá hạn (không gửi gì trong hạn) | Giữ bản quá hạn, tô **đỏ** | **Đúng / Sai / HUỶ KẾT QUẢ** |

> **"Huỷ kết quả" là outcome THỨ BA**, chỉ có ở tình huống thứ hai. Nó khác *Sai*: ở lượt chung *Sai* kéo theo **−5**, còn huỷ thì câu **không sinh điểm gì**.
> Bản quá hạn **không tự ghi đè** bản hợp lệ — "ghi nhận bản cuối" chỉ áp trong **các bản hợp lệ**.

22. **Phán quyết của admin là QUYẾT ĐỊNH CUỐI CÙNG — chấm xong thì khoá nút gửi.** Không còn bản nào tới sau để lật kết quả.

Để phán quyết không bao giờ rơi vào giữa lúc thí sinh còn đang sửa, luồng tách theo **kênh trả lời của vòng**:

| Kênh trả lời | Nút gửi của thí sinh | Nút chấm của admin |
|---|---|---|
| **Nói** — mode sân khấu | **Không tồn tại** | Bấm được **bất cứ lúc nào** |
| **Gõ** — mode nhập liệu, và các vòng **luôn gõ máy** | Sống tới khi hết giờ | **Khoá tới khi hết giờ** |

> **VCNV hàng ngang** (`Đ-4.2`) và **Tăng tốc** (`Đ-4.b`) luôn gõ máy **bất kể mode contest**, nên hai vòng này luôn theo nhánh dưới.
> Đây là **ngoại lệ thứ hai** của điểm 10 (*"đồng hồ không khoá thao tác của admin"*) — ngoại lệ thứ nhất là nút start timer.

23. **Chọn hàng ngang có MỘT đường vào cho mỗi mode** — không mode nào có hai. Đây là thao tác duy nhất trong game **không bị ép thời gian**, nên cũng là chỗ duy nhất có dialog ở phía thí sinh.

| Mode | Ai chọn hàng ngang | Thí sinh click nhiều lần |
|---|---|---|
| **Sân khấu** | **Chỉ ADMIN** — máy thí sinh không có nút chọn | **KHÔNG TỒN TẠI** (không có nút thì không có tín hiệu) |
| **Nhập liệu** | **Chỉ THÍ SINH** — admin **không** chọn thay | **Dialog xác nhận**, xác nhận xong **khoá nút chọn** |

> **Ngoại lệ tường minh của `CLAUDE.md` §UX** (*"dialog xác nhận không bao giờ ở phía thí sinh"*). Tiêu chí gốc của rule đó là **tốc độ**; chọn hàng ngang không đua tốc độ nên tiêu chí không áp. Mọi thao tác đua tốc độ (chuông, "Mở chướng ngại vật", gửi đáp án) vẫn **tuyệt đối không có dialog**.
> **Khoá là TẠM**: admin bấm **No** thì nút **mở lại** — bắt buộc bởi `Đ-7` (*"từ chối ⇒ thí sinh không mất lượt"*). Khoá chỉ trở thành vĩnh viễn cho câu đó khi admin bấm **Yes**.
> **Hai lớp xác nhận không thừa nhau**: dialog của thí sinh chống **bấm nhầm**; admin duyệt Yes/No là **phán quyết**. Hàng đợi vẫn **chặn** ở VCNV trong cả hai mode (điểm 4).

## Mục lục

### Vòng Khởi động

| Mã | Tên | Status |
|---|---|---|
| GR-001 | Khởi động lượt riêng: chấm câu | **CONFIRMED** |
| GR-002 | Khởi động lượt riêng: hết thời gian suy nghĩ | **CONFIRMED** |
| GR-003 | Khởi động lượt chung: giành quyền bằng chuông | **CONFIRMED** |
| GR-004 | Khởi động lượt chung: chấm câu và hình phạt | **CONFIRMED** |
| GR-005 | Khởi động lượt chung: cửa sổ chuông rỗng | **CONFIRMED** |
| GR-006 | Khởi động: ghi nhận đáp án | **CONFIRMED** |

### Vòng Vượt chướng ngại vật

| Mã | Tên | Status |
|---|---|---|
| GR-007 | VCNV: lượt chọn hàng ngang | NEEDS CLARIFICATION |
| GR-008 | VCNV: trả lời hàng ngang và mở miếng ghép | NEEDS CLARIFICATION |
| GR-009 | VCNV: bấm chuông giải Chướng ngại vật | NEEDS CLARIFICATION |
| GR-010 | VCNV: trả lời sai Chướng ngại vật | NEEDS CLARIFICATION |
| GR-011 | VCNV: ô trung tâm và gợi ý cuối | NEEDS CLARIFICATION |
| GR-012 | VCNV: toàn bộ thí sinh bị loại | NEEDS CLARIFICATION |

### Vòng Tăng tốc

| Mã | Tên | Status |
|---|---|---|
| GR-013 | Tăng tốc: xếp hạng tốc độ | **CONFIRMED** |
| GR-014 | Tăng tốc: đồng thời gian | NEEDS CLARIFICATION |
| GR-015 | Tăng tốc: ghi nhận bản cuối | **CONFIRMED** |

### Vòng Về đích

| Mã | Tên | Status |
|---|---|---|
| GR-016 | Về đích: thứ tự lượt thi | NEEDS CLARIFICATION |
| GR-017 | Về đích: chọn gói câu | NEEDS CLARIFICATION |
| GR-018 | Về đích: trả lời câu của mình | **CONFIRMED** |
| GR-019 | Về đích: câu hỏi thực hành | NEEDS CLARIFICATION |
| GR-020 | Về đích: cướp quyền | NEEDS CLARIFICATION |
| GR-021 | Về đích: Ngôi sao hy vọng | NEEDS CLARIFICATION |

### Vòng Câu hỏi phụ

| Mã | Tên | Status |
|---|---|---|
| GR-022 | Câu hỏi phụ: điều kiện kích hoạt | **CONFIRMED** |
| GR-023 | Câu hỏi phụ: thể thức ba câu | **CONFIRMED** |
| GR-024 | Câu hỏi phụ: bấm chuông trước hiệu lệnh | NEEDS CLARIFICATION |
| GR-025 | Câu hỏi phụ: hết câu chưa phân định | NEEDS CLARIFICATION |

### Chấm điểm và mô hình điểm

| Mã | Tên | Status |
|---|---|---|
| GR-026 | Phán quyết của admin | NEEDS CLARIFICATION |
| GR-027 | Chuẩn hoá và highlight đáp án | **CONFIRMED** |
| GR-028 | Điểm là hàm của event log | **CONFIRMED** |
| GR-029 | Điều chỉnh điểm thủ công | NEEDS CLARIFICATION |
| GR-030 | Bỏ vòng và chạy lại vòng | NEEDS CLARIFICATION |
| GR-031 | Rút đề và no-repeat toàn contest | NEEDS CLARIFICATION |

### Điều khiển và hạ tầng trận

| Mã | Tên | Status |
|---|---|---|
| GR-032 | Hàng đợi tín hiệu và xác nhận của admin | NEEDS CLARIFICATION |
| GR-033 | Mốc thời gian do admin bấm | **CONFIRMED** |
| GR-034 | Chuông chỉ nhận click chuột | **CONFIRMED** |
| GR-035 | Server time là source of truth duy nhất | NEEDS CLARIFICATION |
| GR-036 | Mất kết nối và giữ ghế | NEEDS CLARIFICATION |
| GR-037 | Phạm vi hiển thị đáp án | **CONFLICT** |

---

## GR-001 — Khởi động lượt riêng: chấm câu

### Status

CONFIRMED

### Purpose

Xác định điểm mà một thí sinh nhận được cho một câu hỏi trong lượt riêng của mình ở vòng Khởi động, sau khi admin phán quyết.

### Actors

Thí sinh (trả lời) · Admin (bấm start timer, bấm Đúng/Sai) · MC (đọc câu hỏi, phán quyết bằng lời) · Server (ghi nhận thời gian và event).

### Related states

`rounds[i]` = Khởi động, lượt riêng của một thí sinh.

### Trigger

Admin bấm Đúng hoặc Sai cho câu hỏi đang mở của thí sinh đang tới lượt.

### Preconditions

- Trận đang ở vòng Khởi động, lượt riêng, và thí sinh này là người đang tới lượt.
- Câu hỏi đã được đưa ra và admin đã bấm start timer (mốc *"MC đọc xong câu hỏi"*).
- Câu hỏi này chưa có phán quyết nào được chốt.

### Inputs

- Thí sinh đang tới lượt.
- Câu hỏi đang mở và giá trị điểm của nó.
- Thời gian suy nghĩ: **3 giây** tính từ mốc admin bấm start timer.
- Phán quyết của admin: Đúng hoặc Sai.

### Conditions

- Mode mặc định là mode sân khấu ⇒ thí sinh **đọc** đáp án; hệ thống không nhận được nội dung đáp án, admin là người nghe và phán quyết.
- Máy **không** tự chấm. Trong trận chính thức không tồn tại đường nào tự cộng/trừ điểm từ so khớp.
- Trả lời đúng: **+10**. Trả lời sai: **0**, không trừ điểm.

### Decision table

| Case | Conditions | Expected outcome | State change | Error |
|---|---|---|---|---|
| C1 — happy path | Thí sinh đúng lượt, câu đang mở, admin bấm Đúng trong hạn | **+10** cho thí sinh | Sinh event điểm; câu chuyển sang đã chấm | — |
| C2 — trả lời sai | Như C1, admin bấm Sai | **0**, không trừ | Sinh event điểm giá trị 0; câu đã chấm | — |
| C3 — boundary: hết 3 giây, chưa có phán quyết | Hết thời gian suy nghĩ mà admin chưa bấm | Chuyển GR-002 | Câu vẫn chưa chấm cho tới khi admin bấm | — |
| C4 — invalid state: vòng đã bị bỏ | Vòng Khởi động đã bị bỏ, câu này chưa chấm, admin bấm Đúng | Máy admin hiện **toast báo thao tác không hợp lệ**; **không thực hiện được** ⇒ không sinh event điểm cho vòng đã bỏ | Không đổi | — |

### Outcomes

- **Đúng**: thí sinh được cộng 10 điểm.
- **Sai**: không cộng, không trừ.
- **Chưa phán quyết**: câu chưa được chấm, không sinh điểm; gợi ý so khớp (nếu có) không có hiệu lực trong trận.

### State changes

- Sinh event điểm cho thí sinh của câu này.
- Câu chuyển sang trạng thái đã chấm.
- Câu được đánh dấu đã dùng trong contest.

### No-change guarantees

Khi rule không áp dụng được (sai lượt, câu đã chấm cùng loại phán quyết, admin chưa bấm):

- Điểm của mọi thí sinh **không đổi**.
- Lịch sử event **không bị xoá** — mọi hiệu chỉnh đều là event thêm vào.
- Câu đã dùng **không** được trả lại kho đề.

### Error outcomes

Nguồn không định nghĩa lỗi nghiệp vụ nào cho rule này. Các nhánh bất thường được xử bằng **cảnh báo cho admin**, không phải chặn cứng, trừ ngưỡng bất khả thi vật lý.

### Evaluation order

Không còn điều kiện nào phải kiểm: sau khi admin bấm Đúng hoặc Sai, **nút chấm bị khoá và nút "Câu kế tiếp" hiện lên** ⇒ một câu chỉ đi qua đúng **một** phán quyết, không có nhánh rẽ.

**Hết 3 giây KHÔNG khoá nút Đúng/Sai** — admin vẫn bấm được sau mốc đó. Mốc 3 giây là hướng dẫn cho MC và admin, **không phải ràng buộc của hệ thống**, nên không có xung đột thứ tự nào giữa sự kiện hết giờ và thao tác phán quyết.

> Hai vế từng được liệt kê ở đây **không còn là điều kiện phải kiểm**: *"thí sinh có đúng lượt không"* (lượt riêng khoá vào một thí sinh trước câu đầu tiên) và *"còn trong thời gian suy nghĩ không"* (đồng hồ không chặn thao tác chấm).

### Boundaries

| Biên | Giá trị | Quy định |
|---|---|---|
| Nhỏ hơn min | Số câu đã hỏi < 1 | Lượt chưa bắt đầu, rule không áp dụng |
| Bằng min | Câu thứ **1** trong 6 câu | Áp dụng bình thường |
| Trong khoảng | Câu 2-5 | Áp dụng bình thường |
| Bằng max | Câu thứ **6** | Áp dụng bình thường; sau đó lượt riêng của thí sinh này kết thúc |
| Lớn hơn max | Câu thứ 7 trở đi | **KHÔNG TỒN TẠI** — sau khi chấm xong câu thứ 6, nút "Câu kế tiếp" **chuyển thành nút "Kết thúc lượt"** ⇒ không có đường nào hỏi thêm câu |
| Biên thời gian | t = đúng **3.000 giây** | **Hệ thống không phân xử biên này** — đồng hồ không khoá nút phán quyết; trong hạn hay quá hạn là đánh giá của MC và admin |
| Biên điểm | — | Điểm **không có sàn**, được phép âm; rule này không tạo giá trị âm |

### Idempotency

- **Bấm lại nút chấm**: KHÔNG XẢY RA — sau lần bấm đầu, nút Đúng/Sai bị khoá và nút "Câu kế tiếp" hiện lên.
- **Bấm start timer lần thứ hai trên cùng một câu**: KHÔNG XẢY RA — nút start timer **tự khoá sau lần bấm đầu** để chống bấm trùng.

### Concurrency

- **Hai admin cùng phán quyết một câu**: KHÔNG XẢY RA — mỗi contest chỉ có **một admin duy nhất**.
- **Phán quyết đến đúng lúc hết thời gian suy nghĩ**: không có xung đột — hết giờ không khoá nút phán quyết, nên hai sự kiện này độc lập với nhau.

### Examples

- **Hợp lệ**: thí sinh ở vị trí 2 đang trong lượt riêng, câu thứ 3; admin bấm start timer; thí sinh đọc đáp án; MC xác nhận đúng; admin bấm Đúng ⇒ **+10**.
- **Không hợp lệ**: hệ thống tự cộng 10 điểm vì đáp án khớp đáp án chuẩn mà admin chưa bấm ⇒ trái nguyên tắc máy không tự chấm; ở trận chính thức không tồn tại đường nào sinh điểm mà không qua phán quyết của admin.
- **Boundary**: thí sinh đọc đáp án đúng khoảnh khắc đồng hồ chỉ 3.000 giây ⇒ hệ thống không phân xử; admin vẫn bấm được Đúng, MC và admin đánh giá.

### Source traceability

| Nguồn | Heading | Discovered rule ID |
|---|---|---|
| `docs/source/fandom-olympia-26-luat-choi.md` | §Khởi động, đoạn 2 | — |
| `docs/game-rules-inventory.md` | §R-KD-01 | R-KD-01 |
| `docs/reviews/game-rules-decisions.md` | §3.1 Đ-1 · §3.3 · §8.1 Đ-6 · §9.1 | Đ-1, Đ-6 |
| `docs/reviews/game-rules-review.md` | — | — |
| `docs/reviews/game-rules-review-old.md` | GRR-002 | — |
| `docs/glossary.md` | TERM-035, TERM-039, TERM-049 | — |

---

## GR-002 — Khởi động lượt riêng: hết thời gian suy nghĩ

### Status

CONFIRMED

### Purpose

Xác định kết quả khi thí sinh trong lượt riêng không đưa ra đáp án trong thời gian suy nghĩ.

### Actors

Thí sinh · Admin · Server (đồng hồ).

### Related states

`rounds[i]` = Khởi động, lượt riêng.

### Trigger

**Admin bấm Sai** cho một thí sinh không đưa ra đáp án trong thời gian suy nghĩ. Đồng hồ hết giờ **không** tự sinh kết quả — máy không tự chấm.

### Preconditions

- Câu hỏi đang mở cho thí sinh đang tới lượt.
- Admin đã bấm start timer.

### Inputs

- Thời gian suy nghĩ **3 giây** — mốc tham chiếu cho MC và admin, không khoá thao tác phán quyết.
- Mốc bắt đầu: thời điểm admin bấm start timer.

### Conditions

- Không trả lời được xử **như trả lời sai** ở lượt riêng: **0 điểm, không trừ**. Lượt riêng không có hình phạt.
- Cả hai tình huống — **trả lời sai** và **không trả lời** — đều được thể hiện bằng **cùng một thao tác: admin bấm Sai**. Hệ thống không phân biệt hai tình huống này.

### Decision table

| Case | Conditions | Expected outcome | State change | Error |
|---|---|---|---|---|
| C1 — happy path | Thí sinh không đưa ra đáp án; admin bấm Sai | **0 điểm**, không trừ | Sinh event điểm; câu chờ admin bấm "Câu kế tiếp" | — |
| C2 — có đáp án trước khi hết giờ | Thí sinh đọc đáp án trong hạn | Chuyển GR-001 | — | — |
| C3 — đáp án đến sát hoặc quá mốc 3 giây | Thí sinh trả lời quanh khoảnh khắc hết giờ | **MC và admin đánh giá**, hệ thống không phân xử — nút phán quyết không bị đồng hồ khoá | Theo phán quyết của admin | — |
| C4 — trả lời đúng sớm | Thí sinh trả lời đúng ở giây thứ 1, admin bấm Đúng | Điểm chốt **ngay**; câu **không tự chuyển** — sang câu tiếp là thao tác riêng, admin bấm "Câu kế tiếp" khi muốn. Đồng hồ còn lại không còn ý nghĩa | Sinh event điểm; câu chờ admin chuyển | — |

### Outcomes

- **Hết giờ không đáp án**: admin bấm Sai ⇒ 0 điểm, không trừ.
- **Có đáp án trong hạn**: rule này không áp dụng, chuyển GR-001.
- **Chuyển sang câu tiếp theo là thao tác RIÊNG** của admin, không phải hệ quả tự động của việc chấm.

### State changes

- Sinh event điểm giá trị 0; câu chuyển sang đã chấm và được đánh dấu đã dùng.
- Câu chỉ khép lại khi admin bấm "Câu kế tiếp".

### No-change guarantees

- Điểm của thí sinh và của mọi người khác **không đổi**.
- Không sinh hình phạt nào — lượt riêng **không** có trừ điểm.

### Error outcomes

Không có lỗi nghiệp vụ. Đây là một nhánh kết quả bình thường của luật.

### Evaluation order

Không có thứ tự nào phải quy định: hết giờ **không** khoá nút phán quyết, nên sự kiện hết giờ và thao tác bấm Sai của admin độc lập với nhau. Chỉ có **một** đường sinh kết quả — admin bấm.

### Boundaries

| Biên | Giá trị | Quy định |
|---|---|---|
| Nhỏ hơn min | t < 0 | Không tồn tại |
| Bằng min | t = 0 (mốc admin bấm start timer) | Đồng hồ bắt đầu; chưa có kết quả nào |
| Trong khoảng | 0 < t < 3 giây | Trong thời gian suy nghĩ theo luật |
| Bằng max | t = **3.000 giây** | **Hệ thống không phân xử** — đồng hồ chỉ hiển thị, MC và admin đánh giá |
| Lớn hơn max | t > 3 giây | Quá thời gian suy nghĩ theo luật; nút phán quyết **vẫn bấm được** |

### Idempotency

- **Bấm lại nút chấm**: KHÔNG XẢY RA — sau lần bấm đầu, nút Đúng/Sai bị khoá và nút "Câu kế tiếp" hiện lên.
- **Bấm start timer lần thứ hai**: KHÔNG XẢY RA — nút tự khoá sau lần bấm đầu.

> Khoá nằm ở **giao diện**; theo `CLAUDE.md` §Zero-trust, server vẫn phải từ chối phán quyết lặp lại cho một câu đã chốt.

### Concurrency

Không có xung đột: đồng hồ hết giờ không sinh kết quả và **không khoá thao tác nào của admin**, nên nó không tranh chấp với phán quyết. Trạng thái tạm dừng không tồn tại nên cũng không có nhánh xung đột với nó.

### Examples

- **Hợp lệ**: thí sinh im lặng hết 3 giây, admin bấm Sai ⇒ 0 điểm; nút "Câu kế tiếp" hiện lên, admin bấm khi MC đã sẵn sàng.
- **Hợp lệ**: thí sinh trả lời đúng ở giây thứ 1, admin bấm Đúng ⇒ **+10 ngay**; hai giây còn lại của đồng hồ không còn ý nghĩa, câu vẫn đứng đó tới khi admin chuyển.
- **Không hợp lệ**: hệ thống tự trừ 5 điểm vì không trả lời ⇒ sai luật hai lần; máy không tự chấm, và hình phạt −5 **chỉ** thuộc lượt chung.
- **Boundary**: thí sinh bắt đầu nói ở giây 2.9 nhưng MC xác nhận sau giây 3 ⇒ admin vẫn bấm được Đúng; hệ thống không chặn theo đồng hồ.

### Source traceability

| Nguồn | Heading | Discovered rule ID |
|---|---|---|
| `docs/source/fandom-olympia-26-luat-choi.md` | §Khởi động, đoạn 2 | — |
| `docs/game-rules-inventory.md` | §R-KD-01 | R-KD-01 |
| `docs/reviews/game-rules-decisions.md` | §9.1 (không trả lời ở lượt riêng xử như trả lời sai) | Đ-6 |
| `docs/reviews/game-rules-review-old.md` | GRR-002, GRR-006 | — |
| `docs/reviews/game-rules-review.md` | — | — |

---

## GR-003 — Khởi động lượt chung: giành quyền bằng chuông

### Status

CONFIRMED

### Purpose

Xác định thí sinh nào giành được quyền trả lời một câu hỏi trong lượt chung của vòng Khởi động.

### Actors

Thí sinh (bấm chuông) · Admin (bấm start timer) · Server (thứ tự và thời gian) · MC (đọc câu hỏi).

### Related states

`rounds[i]` = Khởi động, lượt chung.

### Trigger

Một thí sinh bấm chuông.

### Preconditions

- Trận đang ở lượt chung của vòng Khởi động.
- Câu hỏi đang được đưa ra; cửa sổ chuông của câu chưa đóng.

### Inputs

- Tín hiệu chuông kèm **server timestamp**.
- Ghế phát tín hiệu.
- Cửa sổ chuông: **một khoảng liên tục**, mở từ **mốc admin bấm hiển thị câu hỏi**, kéo dài qua thời gian MC đọc và thêm **3 giây** sau mốc admin bấm start timer.

### Conditions

- Thí sinh **được phép** bấm chuông **trong khi MC đang đọc câu hỏi**.
- Nút chuông **chỉ nhận click chuột**, không gán phím tắt.
- Ở lượt chung, hàng đợi tín hiệu **KHÔNG chặn**: có chuông là tính ngay theo thứ tự tới; hàng đợi vẫn ghi nhận thứ tự để admin can thiệp khi có sự cố.
- Thời gian suy nghĩ **3 giây** được tính **từ thời điểm giành được quyền**, không phải từ mốc admin bấm.

### Decision table

| Case | Conditions | Expected outcome | State change | Error |
|---|---|---|---|---|
| C1 — happy path | Cửa sổ chuông đang mở, một thí sinh bấm đầu tiên | Thí sinh đó giành quyền; bắt đầu đếm 3 giây suy nghĩ | Ghi tín hiệu vào hàng đợi; đánh dấu người giành quyền | — |
| C2 — bấm khi MC đang đọc | Bấm trước mốc admin bấm start timer nhưng câu đã được đưa ra | **Hợp lệ** — luật cho phép | Như C1 | — |
| C3 — người thứ hai bấm sau | Đã có người giành quyền, người khác bấm | **Ghi vào hàng đợi làm căn cứ cho admin**; **không** tự động trao quyền, **không** mở lại chuông. Người bấm đầu giữ quyền | Ghi tín hiệu kèm server timestamp; quyền trả lời không đổi | — |
| C4 — boundary: hai chuông cùng mốc thời gian | Hai ghế khác nhau có **cùng server timestamp** | **Hàng đợi tự quyết định** — thứ tự là ngẫu nhiên, không có tiêu chí ưu tiên nào (không theo ghế, không theo vị trí). Người vào hàng đợi trước giành quyền | Cả hai tín hiệu đều được ghi kèm thứ tự đã chọn | — |
| C5 — invalid state: ghế không còn quyền | Thí sinh đã bấm và bị chấm Sai ở câu này, tiếp tục bấm | Máy thí sinh **không hiển thị gì**, bấm **không phản hồi** ⇒ không có tín hiệu nào được tạo | Không đổi | — |
| C6 — reject: cửa sổ đã đóng | Bấm sau khi cửa sổ chuông của câu đã đóng | Máy thí sinh **không hiển thị gì**, bấm **không phản hồi** ⇒ không có tín hiệu nào được tạo | Không đổi | — |

### Outcomes

- **Giành quyền**: thí sinh bấm sớm nhất theo server timestamp được quyền trả lời; đồng hồ 3 giây bắt đầu từ thời điểm đó.
- **Không giành được**: không có hệ quả điểm và **không có hệ quả nào khác**; tín hiệu nằm trong hàng đợi làm **căn cứ để admin can thiệp** khi có sự cố hoặc khiếu nại.

### State changes

- Ghi tín hiệu vào hàng đợi kèm server timestamp.
- Đánh dấu người giành quyền cho câu hiện tại.
- Bắt đầu đồng hồ 3 giây suy nghĩ.

### No-change guarantees

- Điểm **không đổi** ở bước này — giành quyền chưa phải phán quyết.
- **Lịch sử tín hiệu không bao giờ bị xoá**; không có cơ chế drop.
- Cửa sổ chuông của câu **không** được kéo dài do thao tác của thí sinh.
- Tín hiệu đến sau khi đã có người giành quyền **không** làm đổi người giữ quyền, **không** mở lại chuông.

### Error outcomes

Không có lỗi nghiệp vụ được định nghĩa. Mọi tín hiệu đều có outcome xác định:

- **Ngoài cửa sổ hoặc từ ghế không còn quyền** (C5, C6): frontend không cho phát ⇒ không có tín hiệu nào tồn tại.
- **Đến sau khi đã có người giành quyền** (C3): được ghi vào hàng đợi nhưng **trơ** — không sinh hệ quả nào, chỉ là căn cứ để admin can thiệp.

### Evaluation order

Không còn thứ tự nào phải quy định. Ba điều kiện từng phải cân nhắc nay cho outcome **độc lập và không xung đột**, nên kiểm theo thứ tự nào cũng ra cùng kết quả:

| Điều kiện | Outcome |
|---|---|
| Cửa sổ chuông đã đóng | Không có tín hiệu (frontend không cho phát) |
| Ghế không còn quyền | Không có tín hiệu (frontend không cho phát) |
| Đã có người giành quyền | Tín hiệu được ghi nhưng **trơ** — chỉ là căn cứ cho admin |

### Boundaries

| Biên | Giá trị | Quy định |
|---|---|---|
| Nhỏ hơn min | Bấm trước khi câu được đưa ra | **KHÔNG TỒN TẠI** — nút hiển thị câu hỏi tách riêng khỏi nút bắt đầu; trước khi admin bấm hiển thị, màn thí sinh chưa có gì và chuông chưa sống ⇒ không có tín hiệu nào được tạo |
| Bằng min | **Mốc admin bấm hiển thị câu hỏi** — chuông sống từ đây | Hợp lệ |
| Trong khoảng | Trong lúc MC đọc, và trong 3 giây sau mốc admin bấm | Hợp lệ |
| Bằng max | t = đúng **3.000 giây** sau mốc admin bấm | **Vẫn hợp lệ** — biên **đóng**, tín hiệu đúng mốc được tính là giành quyền |
| Lớn hơn max | Sau 3 giây | Không giành được quyền; câu bị bỏ qua theo GR-005 |
| Số người bấm | 0 → GR-005 · 1 → C1 · ≥2 cùng mốc → C4 | — |

### Idempotency

Nút chuông **tự khoá ngay khi bấm** ở phía frontend, trong cùng thao tác đó, **trước khi gửi tín hiệu** ⇒ không tồn tại cú bấm thứ hai của cùng một ghế trong cùng một câu.

### Concurrency

Ở vòng này hàng đợi **không chặn**, server phân xử ngay theo thứ tự tới. Khi hai ghế có **cùng** server timestamp: **hàng đợi tự quyết định thứ tự, và thứ tự đó là ngẫu nhiên** — không ưu tiên theo số ghế, vị trí hay bất kỳ tiêu chí nào khác.

> Ngẫu nhiên **lúc nhận**, nhưng **tất định khi dựng lại**: thứ tự hàng đợi chọn được **ghi lại và không bao giờ xoá** (`Đ-7`), nên replay event log vẫn ra đúng kết quả cũ. Đây là điều kiện giữ cho `Đ-5.3` (*"điểm là hàm của event log"*) không bị phá.

Quy tắc *"cùng thời gian thì cùng mức điểm"* của Tăng tốc **không** dùng lại được ở đây: điểm chia được, còn quyền trả lời thì không.

### Examples

- **Hợp lệ**: MC mới đọc nửa câu hỏi, thí sinh vị trí 3 bấm chuông ⇒ giành quyền; đồng hồ 3 giây bắt đầu từ thời điểm bấm.
- **Không hợp lệ**: thí sinh dùng phím tắt để bấm chuông ⇒ không có đường nào phát tín hiệu; chuông chỉ nhận click chuột.
- **Boundary**: hai thí sinh bấm và server ghi cùng một mốc mili-giây ⇒ hàng đợi tự chọn một người, ngẫu nhiên; thứ tự đã chọn được ghi lại để phân xử về sau.

### Source traceability

| Nguồn | Heading | Discovered rule ID |
|---|---|---|
| `docs/source/fandom-olympia-26-luat-choi.md` | §Khởi động, đoạn 3-5 | — |
| `docs/game-rules-inventory.md` | §R-KD-03, §R-GEN-01 | R-KD-03, K-3 |
| `docs/reviews/game-rules-decisions.md` | §5.1 Đ-7 · §5.2 Đ-7.2 · §8.1 Đ-6 · §9.1 | Đ-7, Đ-7.2 |
| `docs/reviews/game-rules-review-old.md` | GRR-002, GRR-005, GRR-007 | — |
| `docs/reviews/game-rules-review.md` | — | — |
| `docs/glossary.md` | TERM-031, TERM-032 | — |

---

## GR-004 — Khởi động lượt chung: chấm câu và hình phạt

### Status

CONFIRMED

### Purpose

Xác định điểm cộng và điểm trừ cho thí sinh đã giành quyền trả lời trong lượt chung.

### Actors

Thí sinh (đã giành quyền) · Admin (phán quyết) · MC · Server.

### Related states

`rounds[i]` = Khởi động, lượt chung.

### Trigger

Admin bấm Đúng hoặc Sai cho thí sinh đã giành quyền; hoặc hết 3 giây suy nghĩ mà thí sinh không đưa ra đáp án.

### Preconditions

- Một thí sinh đã giành quyền trả lời theo GR-003.
- Câu hỏi chưa có phán quyết chốt.

### Inputs

- Thí sinh đã giành quyền.
- Thời gian suy nghĩ **3 giây** tính từ thời điểm giành quyền.
- Phán quyết của admin.

### Conditions

- Trả lời đúng: **+10**.
- Trả lời sai: **−5**.
- **Bấm chuông mà không có câu trả lời sau 3 giây**: cũng **−5**. Đây là nhánh phạt riêng, khác với nhánh "hết giờ không ai bấm" (GR-005).

### Decision table

| Case | Conditions | Expected outcome | State change | Error |
|---|---|---|---|---|
| C1 — happy path | Người giành quyền trả lời, admin bấm Đúng | **+10** | Sinh event điểm | — |
| C2 — trả lời sai | Admin bấm Sai | **−5** | Sinh event điểm âm | — |
| C3 — huỷ kết quả | Admin bấm **Huỷ kết quả** | **0** — câu không sinh điểm cho ai; **không** áp hình phạt −5 | Sinh event điểm giá trị 0; câu đã chấm | — |
| C4 — bấm chuông rồi im lặng | Giành quyền nhưng không đưa ra đáp án; admin bấm Sai | **−5** | Sinh event điểm âm | — |
| C5 — boundary: điểm xuống âm | Thí sinh đang có 0 điểm và bị −5 | Điểm thành **−5**; **không có sàn** | Sinh event điểm âm | — |
| C6 — sau khi người giành quyền trả lời sai | Câu đã chấm Sai | **KHÔNG mở lại chuông** — câu kết thúc, chuyển sang câu mới | **Xoá hàng đợi đang hoạt động** của câu vừa xong; **gỡ khoá chuông** cho mọi ghế để sẵn sàng câu mới | — |

### Outcomes

- **Đúng**: +10.
- **Sai**: −5.
- **Bấm chuông rồi không trả lời trong 3 giây**: −5.
- **Mọi nhánh đều KẾT THÚC CÂU** — không mở lại chuông cho người khác. Chấm xong là sang câu mới.

### State changes

- Sinh event điểm cho thí sinh đã giành quyền.
- Câu chuyển sang đã chấm và được đánh dấu đã dùng.
- **Xoá hàng đợi đang hoạt động** của câu vừa xong.
- **Gỡ khoá chuông** cho mọi ghế, sẵn sàng cho câu mới (`Đ-24`: khoá gắn với một câu).

### No-change guarantees

- Điểm của các thí sinh **khác** không đổi — lượt chung không có cơ chế chuyển điểm giữa người với người.
- Lịch sử event không bị xoá.
- **Xoá hàng đợi chỉ xoá hàng đợi ĐANG HOẠT ĐỘNG.** Lịch sử tín hiệu — kể cả các tín hiệu trơ theo `Đ-25` — **không bao giờ bị xoá**; admin vẫn xem lại được sau khi câu đã đóng.

### Error outcomes

Không có lỗi nghiệp vụ được định nghĩa; các nhánh bất thường đi qua cảnh báo cho admin.

### Evaluation order

Chỉ còn **một** điều kiện phải kiểm: câu đã có phán quyết chốt chưa. **Hết 3 giây không khoá nút Đúng/Sai**, và cả hai tình huống — trả lời sai, hoặc bấm chuông rồi không trả lời — đều được thể hiện bằng **cùng một thao tác: admin bấm Sai**. Không có sự kiện tự động nào tranh chấp với phán quyết.

### Boundaries

| Biên | Giá trị | Quy định |
|---|---|---|
| Nhỏ hơn min | — | Điểm **không có sàn**; không tồn tại "nhỏ hơn min" |
| Bằng min | Điểm hiện tại bất kỳ | Trừ 5 vẫn áp dụng |
| Trong khoảng | — | — |
| Bằng max | Câu thứ **12** của lượt chung | Áp dụng bình thường; hết lượt chung |
| Lớn hơn max | Câu thứ 13 trở đi | **KHÔNG TỒN TẠI** — sau khi chấm xong câu thứ 12, nút "Câu kế tiếp" **chuyển thành nút "Kết thúc lượt chung"** ⇒ không có đường nào hỏi thêm câu |
| Biên thời gian | t = đúng **3.000 giây** kể từ khi giành quyền | **Hệ thống không phân xử** — đồng hồ không khoá nút phán quyết; MC và admin đánh giá |

### Idempotency

- **Bấm lại nút chấm**: KHÔNG XẢY RA — nút Đúng/Sai khoá ngay sau lần bấm đầu, nút "Câu kế tiếp" hiện lên.

### Concurrency

Không tồn tại phán quyết đồng thời từ hai người: **mỗi contest chỉ có MỘT admin duy nhất**.

### Examples

- **Hợp lệ**: thí sinh giành quyền, trả lời sai ⇒ **−5**; thí sinh đang có 30 điểm còn **25**.
- **Không hợp lệ**: hệ thống tự trừ −5 dựa trên so khớp văn bản mà admin chưa bấm ⇒ trái nguyên tắc máy không tự chấm.
- **Boundary**: thí sinh đang có **0** điểm bấm chuông rồi im lặng ⇒ **−5**, điểm âm hợp lệ, không có sàn.

### Source traceability

| Nguồn | Heading | Discovered rule ID |
|---|---|---|
| `docs/source/fandom-olympia-26-luat-choi.md` | §Khởi động, đoạn 3 | — |
| `docs/game-rules-inventory.md` | §R-KD-03 | R-KD-03, K-1 |
| `docs/reviews/game-rules-decisions.md` | §3.1 Đ-1 · §3.2 Đ-2 · §3.3 | Đ-1, Đ-2 |
| `docs/reviews/game-rules-review-old.md` | GRR-002, GRR-007, GRR-068 | — |
| `docs/reviews/game-rules-review.md` | — | — |

---

## GR-005 — Khởi động lượt chung: cửa sổ chuông rỗng

### Status

CONFIRMED

### Purpose

Xác định điều gì xảy ra với một câu hỏi lượt chung mà không thí sinh nào giành quyền trả lời.

### Actors

Thí sinh · Admin · Server.

### Related states

`rounds[i]` = Khởi động, lượt chung.

### Trigger

Hết **3 giây** kể từ mốc admin bấm start timer mà không có tín hiệu chuông nào.

### Preconditions

- Câu hỏi đã được đưa ra và admin đã bấm start timer.
- Không có thí sinh nào giành quyền.

### Inputs

- Cửa sổ chuông **3 giây** sau mốc admin bấm start timer.
- Số tín hiệu chuông nhận được trong cửa sổ: **0**.

### Conditions

Hết cửa sổ mà không ai bấm ⇒ **câu bị bỏ qua**, chuyển câu tiếp theo. Admin có thao tác chuyển câu thủ công để ghi đè.

### Decision table

| Case | Conditions | Expected outcome | State change | Error |
|---|---|---|---|---|
| C1 — happy path | Hết 3 giây, không có tín hiệu nào | Câu **bị bỏ qua**, chuyển câu tiếp theo | Câu khép lại và được đánh dấu **đã dùng** | — |
| C2 — có tín hiệu trong cửa sổ | ≥1 tín hiệu | Chuyển GR-003 | — | — |
| C3 — boundary: tín hiệu đến đúng mốc | Tín hiệu có server timestamp đúng bằng mốc hết cửa sổ | **Hợp lệ** — biên **đóng**: tín hiệu đúng mốc vẫn được tính là giành quyền ⇒ chuyển GR-003, câu **không** bị bỏ qua | Đánh dấu người giành quyền | — |
| C4 — admin chuyển câu thủ công sớm | Admin bấm chuyển câu trước khi hết 3 giây | Câu bị bỏ qua ngay | Câu khép lại và đã dùng | — |
| C5 — kho đề cạn giữa vòng | Câu bị bỏ qua vẫn tiêu khỏi kho | **KHÔNG TỒN TẠI** — pre-flight chạy **tại thời điểm bắt đầu mỗi vòng**; thiếu câu thì **không bắt đầu được vòng đó**. Qua được cửa vào thì đủ câu cho trọn vòng | Không đổi | — |

### Outcomes

- **Không ai bấm**: câu bị bỏ qua, không ai được cộng hay trừ điểm.
- **Có người bấm**: rule này không áp dụng.

### State changes

- Câu khép lại, chuyển sang câu tiếp theo.
- Câu được đánh dấu **đã dùng trong contest**, kể cả khi bị bỏ qua.

### No-change guarantees

- Điểm của **mọi** thí sinh không đổi.
- Câu bị bỏ qua **không** được trả lại kho đề.

### Error outcomes

Không có lỗi nghiệp vụ.

### Evaluation order

Không còn thứ tự nào phải quy định. Tín hiệu đến **đúng mốc vẫn hợp lệ** (biên đóng, `Đ-28`) nên không có tranh chấp với sự kiện hết cửa sổ. Thao tác chuyển câu của admin đóng câu ngay tại thời điểm bấm, và nút đó **tự tắt sau lần bấm đầu** (`Đ-29`).

### Boundaries

| Biên | Giá trị | Quy định |
|---|---|---|
| Nhỏ hơn min | Số tín hiệu < 0 | Không tồn tại |
| Bằng min | Số tín hiệu = **0** | Áp dụng C1 — câu bị bỏ qua |
| Trong khoảng | Số tín hiệu ≥ 1 | Chuyển GR-003 |
| Bằng max | Mọi thí sinh đều bấm | Chuyển GR-003; thứ tự theo server timestamp |
| Lớn hơn max | — | Không tồn tại |
| Biên thời gian | t = đúng **3.000 giây** | Tín hiệu đúng mốc **vẫn hợp lệ** (biên đóng) ⇒ câu không bị bỏ qua |

### Idempotency

Sự kiện hết cửa sổ chỉ có nghĩa một lần cho một câu. **Bấm chuyển câu lặp lại: KHÔNG XẢY RA** — bấm xong thì nút "Chuyển câu" **tắt** và nút "Hiển thị câu hỏi" của câu mới **bật**.

### Concurrency

Tín hiệu và sự kiện hết cửa sổ **cùng mốc** ⇒ tín hiệu thắng (biên đóng, `Đ-28`). Tín hiệu đến sau khi admin đã bấm chuyển câu ⇒ cửa sổ đã đóng, máy thí sinh không phản hồi (`Đ-16`).

### Examples

- **Hợp lệ**: MC đọc xong, admin bấm start timer, hết 3 giây không ai bấm ⇒ câu bị bỏ qua, không ai đổi điểm.
- **Không hợp lệ**: câu bị bỏ qua được đưa lại vào kho để hỏi ở trận sau trong cùng contest ⇒ trái quy tắc câu đã dùng không trả lại kho.
- **Boundary**: tín hiệu duy nhất đến đúng mốc 3.000 giây ⇒ **vẫn hợp lệ** (biên đóng), thí sinh đó giành quyền, câu không bị bỏ qua.

### Source traceability

| Nguồn | Heading | Discovered rule ID |
|---|---|---|
| `docs/source/fandom-olympia-26-luat-choi.md` | §Khởi động, đoạn 5 | — |
| `docs/game-rules-inventory.md` | §R-KD-04, §R-GEN-06 | R-KD-04, U-30 |
| `docs/reviews/game-rules-decisions.md` | §6.3 Đ-5.2f · §8.1 Đ-6 | Đ-5.2f |
| `docs/reviews/game-rules-review-old.md` | GRR-002, GRR-005 | — |
| `docs/reviews/game-rules-review.md` | GRR-163 | — |

---

## GR-006 — Khởi động: ghi nhận đáp án

### Status

CONFIRMED

### Purpose

Xác định bản đáp án nào của thí sinh được ghi nhận khi thí sinh gửi nhiều lần trong vòng Khởi động.

### Actors

Thí sinh · Server (ghi nhận) · Admin (bấm mốc công bố đáp án) · MC (công bố bằng lời trên sân khấu).

### Related states

`rounds[i]` = Khởi động.

### Trigger

Thí sinh gửi hoặc gửi lại đáp án trước mốc cắt (admin bấm công bố đáp án).

### Preconditions

- Trận đang ở vòng Khởi động và contest chạy ở **mode nhập liệu** (ở mode sân khấu thí sinh đọc đáp án, không có bản gửi nào để ghi nhận).
- Câu hỏi đang mở.

### Inputs

- Các bản đáp án đã gửi của thí sinh, kèm server timestamp.
- Mốc cắt: **thời điểm admin bấm** — ánh xạ của mốc *"MC công bố đáp án"* (`Đ-6`).

### Conditions

- Thí sinh **được** thay đổi đáp án liên tục; **nút gửi KHÔNG bị khoá sau khi gửi**, chỉ đóng khi hết giờ.
- **Luôn ghi nhận bản CUỐI CÙNG.** Phát biểu *"nếu không thay đổi thì ghi nhận đáp án đầu tiên"* của luật gốc không phải ngoại lệ — gửi một lần thì bản đầu **chính là** bản cuối.
- **Bản rỗng** (chỉ khoảng trắng sau khi cắt) **không phải một đáp án**: bỏ qua, giữ bản hợp lệ trước đó (`Đ-20.1`).
- Mọi đáp án đều được cắt khoảng trắng đầu cuối trước khi so sánh.

### Decision table

| Case | Conditions | Expected outcome | State change | Error |
|---|---|---|---|---|
| C1 — happy path: gửi một lần | Thí sinh gửi đúng một bản trong hạn | Ghi nhận bản đó — nó vừa là bản đầu vừa là bản cuối | Lưu bản được ghi nhận | — |
| C2 — happy path: gửi nhiều lần | Gửi nhiều bản khác nhau trước mốc cắt | Ghi nhận bản **cuối cùng** | Bản trước bị thay thế trong kết quả; lịch sử giữ nguyên | — |
| C3 — boundary: bản cuối đến đúng mốc cắt | Server timestamp trùng mốc công bố | **Được ghi nhận** — biên **đóng**, submission đúng mốc vẫn tính | Cập nhật bản được ghi nhận | — |
| C4 — có bản hợp lệ, bản sau quá hạn | Bản đầu trong hạn, bản sửa đến sau mốc cắt | **GIỮ CẢ HAI**; bản quá hạn tô **đỏ**. Admin nhìn cả hai rồi quyết **Đúng / Sai** | Cả hai bản hiển thị trên màn admin; bản quá hạn không tự thay bản hợp lệ | — |
| C5 — chỉ có bản quá hạn | Thí sinh không gửi gì trong hạn, chỉ gửi sau mốc cắt | Bản quá hạn tô **đỏ**; admin quyết **Đúng / Sai / HUỶ KẾT QUẢ** | Ghi bản quá hạn vào lịch sử; kết quả câu theo phán quyết của admin | — |
| C6 — repeated action: gửi lại nội dung y hệt | Bản mới trùng bản trước sau khi cắt khoảng trắng | Vẫn là **bản cuối** được ghi nhận; nội dung không đổi nên kết quả chấm không đổi. Quy tắc *"nội dung y hệt thì không cập nhật mốc thời gian"* chỉ có ý nghĩa ở vòng **xếp hạng theo tốc độ**; Khởi động không xếp theo thời gian nên không áp | Cập nhật bản được ghi nhận | — |
| C7 — bản gửi rỗng | Nội dung chỉ gồm khoảng trắng sau khi cắt | **Bỏ qua** — giữ bản hợp lệ trước đó (`Đ-20.1`). Bản rỗng không phải một đáp án nên không ghi đè bản đã có | Bản được ghi nhận không đổi | — |
| C8 — mốc cắt được xác định thế nào | MC công bố đáp án trên sân khấu | **Admin bấm** — hệ thống không cần quan sát sân khấu. Mốc *"MC công bố đáp án"* ánh xạ thành **một thao tác bấm của admin**, đúng nguyên tắc `Đ-6` | Mốc cắt được ghi bằng server timestamp của lần bấm đó | — |

### Outcomes

- **Có thay đổi**: bản cuối cùng là bản được chấm.
- **Không thay đổi**: bản đầu tiên là bản được chấm.
- **Đến sau mốc cắt**: không tự thay bản hợp lệ, nhưng **vẫn giữ và hiển thị** cho admin quyết.

### State changes

- Cập nhật bản đáp án được ghi nhận của thí sinh cho câu hiện tại.

### No-change guarantees

- **Lịch sử các bản đã gửi không bị xoá** — chỉ bản *được ghi nhận* thay đổi.
- Điểm **không đổi** ở bước này; ghi nhận đáp án không phải phán quyết.

### Error outcomes

Không có lỗi nghiệp vụ được định nghĩa.

### Evaluation order

Chỉ còn **hai** bước, thứ tự cố định: (a) bản đến **trong hạn** không (biên đóng, `Đ-28`) → (b) nếu có và **khác rỗng**, nó thành bản được ghi nhận. Không cần so nội dung với bản trước: luôn lấy bản cuối.

### Boundaries

| Biên | Giá trị | Quy định |
|---|---|---|
| Nhỏ hơn min | 0 bản gửi | Không có gì để ghi nhận |
| Bằng min | **1** bản | Ghi nhận bản đầu tiên |
| Trong khoảng | 2 bản trở lên | Ghi nhận bản cuối cùng |
| Bằng max | — | Nguồn **không** đặt giới hạn số lần gửi |
| Lớn hơn max | — | Không tồn tại giới hạn |
| Biên thời gian | Bản đến đúng mốc cắt | **Được ghi nhận** (biên đóng, `Đ-28`). Mốc cắt là **thời điểm admin bấm**, ghi bằng server timestamp |

### Idempotency

Gửi lại là **hành vi bình thường, không bị chặn** — nút gửi không khoá sau khi gửi. Mỗi lần gửi hợp lệ đều thay bản được ghi nhận; **bản cuối thắng**, gửi bao nhiêu lần cũng vậy. Nội dung y hệt thì kết quả chấm không đổi.

### Concurrency

Bản gửi **đúng mốc cắt** vẫn được ghi nhận (biên đóng, `Đ-28`).

**Không xảy ra** — cách xử lý tách theo **kênh trả lời của vòng**:

| Kênh trả lời | Nút gửi của thí sinh | Nút chấm của admin |
|---|---|---|
| **Nói** (mode sân khấu) | **Không tồn tại** — thí sinh đọc đáp án | Bấm được **bất cứ lúc nào** |
| **Gõ** (mode nhập liệu, và mọi vòng luôn gõ máy) | Sống tới khi hết giờ | **Khoá tới khi hết giờ** |

Hai nhánh đều triệt tiêu tranh chấp: nhánh trên không có bản gửi nào để đổi, nhánh dưới thì admin chỉ chấm sau khi cửa nhận đáp án đã đóng.

**Và phán quyết của admin là quyết định cuối cùng**: bấm chấm xong thì **nút gửi khoá lại** — không còn bản nào tới sau để lật kết quả.

### Examples

- **Hợp lệ**: thí sinh gửi "Hà Nội", rồi sửa thành "Huế" trước khi công bố ⇒ ghi nhận **"Huế"**.
- **Hợp lệ**: thí sinh gửi "Hà Nội" một lần rồi không đụng nữa ⇒ ghi nhận **"Hà Nội"** (bản đầu, cũng là bản cuối).
- **Không hợp lệ**: bản gửi sau mốc cắt **tự động** thay bản hợp lệ ⇒ sai; nó chỉ được giữ lại tô đỏ để admin quyết.
- **Boundary**: bản sửa cuối cùng đến đúng khoảnh khắc admin bấm công bố ⇒ **vẫn được ghi nhận** (biên đóng, `Đ-28`).

### Source traceability

| Nguồn | Heading | Discovered rule ID |
|---|---|---|
| `docs/source/fandom-olympia-26-luat-choi.md` | §Khởi động, đoạn cuối | — |
| `docs/game-rules-inventory.md` | §R-KD-06, §R-TT-03 | R-KD-06 |
| `docs/reviews/game-rules-decisions.md` | §4 (hai mode trả lời) · §8.1 | Đ-4, Đ-6 |
| `CLAUDE.md` | §Quy ước khác (TRIM mọi input text) | — |
| `docs/glossary.md` | TERM-020 | — |

---

## GR-007 — VCNV: lượt chọn hàng ngang

### Status

NEEDS CLARIFICATION

### Purpose

Xác định thí sinh nào được chọn hàng ngang tiếp theo và thứ tự các lượt chọn trong vòng Vượt chướng ngại vật.

### Actors

Thí sinh (phát tín hiệu chọn) · Admin (xác nhận hoặc từ chối) · Server (hàng đợi).

### Related states

`rounds[i]` = VCNV.

### Trigger

Một tín hiệu chọn hàng ngang được phát. **Chủ thể phát tín hiệu phụ thuộc mode** (`Đ-36`): mode **sân khấu** thì chỉ **admin** click; mode **nhập liệu** thì chỉ **thí sinh** click, admin **không** chọn thay.

### Preconditions

- Trận đang ở vòng VCNV.
- Còn ít nhất một hàng ngang chưa được chọn.
- Chưa có thí sinh nào giải đúng Chướng ngại vật.

### Inputs

- Vị trí của các thí sinh (1 → N), gán thủ công trước trận.
- Danh sách hàng ngang đã chọn và chưa chọn.
- Danh sách thí sinh đã bị loại khỏi vòng này.
- Tín hiệu chọn kèm server timestamp.

### Conditions

- **Mỗi thí sinh có tối đa 1 lượt lựa chọn**, bắt đầu từ thí sinh ở **vị trí số 1**.
- Nếu một thí sinh **trước khi lựa chọn** mà bấm chuông giải Chướng ngại vật và bị loại, lượt chọn **dồn cho thí sinh ở vị trí tiếp theo**.
- Trong trường hợp đó, nếu thí sinh ở vị trí cuối cùng đã hoàn thành lượt chọn mà **vẫn còn hàng ngang chưa được chọn**, lượt chọn **quay trở lại thí sinh ở vị trí số 1**. Đây là ngoại lệ tường minh, chỉ áp khi đã có người bị loại — không mâu thuẫn với *"tối đa 1 lượt"*.
- Ở VCNV, hàng đợi tín hiệu **CHẶN**: tín hiệu chỉ có hiệu lực khi admin xác nhận.
- Admin **từ chối** một tín hiệu thì tín hiệu kế tiếp lên, và **thí sinh không mất lượt**.
- **Một đường vào cho mỗi mode** (`Đ-36`): sân khấu → chỉ admin click; nhập liệu → chỉ thí sinh click.
- Ở mode **nhập liệu**, thao tác chọn của thí sinh đi qua **dialog xác nhận trên máy thí sinh**; xác nhận xong thì **nút chọn khoá**. Khoá **mở lại** nếu admin bấm No.

### Decision table

| Case | Conditions | Expected outcome | State change | Error |
|---|---|---|---|---|
| C1 — happy path | Thí sinh đúng lượt chọn một hàng ngang chưa mở; admin xác nhận | Hàng ngang đó được đưa ra | Đánh dấu lượt chọn của thí sinh đã dùng; hàng ngang chuyển sang đang hỏi | — |
| C2 — admin từ chối | Admin bấm No cho tín hiệu | Tín hiệu kế tiếp trong hàng đợi lên; **thí sinh không mất lượt** · ở mode nhập liệu, **nút chọn của thí sinh mở lại** (`Đ-36`) | NEEDS CLARIFICATION — `game-rules-review.md` GRR-143: chưa nói hàng ngang đã bị đánh dấu đã hỏi chưa, câu đã bị rút khỏi kho chưa, đồng hồ đã chạy chưa | — |
| C3 — người đang tới lượt đã bị loại | Thí sinh bị loại trước khi dùng lượt chọn | Lượt dồn cho **vị trí tiếp theo** | Cập nhật người đang tới lượt | — |
| C4 — boundary: hết vòng vị trí mà còn hàng chưa chọn | Vị trí cuối đã chọn xong, vẫn còn hàng ngang | Lượt **quay lại vị trí số 1** | Cập nhật người đang tới lượt | — |
| C5 — chọn hàng ngang đã mở | Thao tác trỏ vào hàng ngang đã được chọn | **KHÔNG TỒN TẠI** — hàng ngang đã mở không còn là mục tiêu chọn được; máy thí sinh **không hiển thị nó như lựa chọn** và bấm **không phản hồi** (`Đ-16`) | Không đổi | — |
| C6a — repeated action, mode **sân khấu** | Thí sinh click chọn hàng ngang nhiều lần | **KHÔNG TỒN TẠI** — máy thí sinh **không có nút chọn** ở mode này; chọn hàng ngang do admin (`Đ-36`) | Không đổi | — |
| C6b — repeated action, mode **nhập liệu** | Thí sinh click chọn hàng ngang nhiều lần | Click đầu mở **dialog xác nhận**; xác nhận xong **khoá nút chọn** ⇒ **đúng một tín hiệu** rời máy thí sinh cho một lượt chọn (`Đ-36`) | Nút chọn của ghế đó chuyển sang khoá; khoá **mở lại** nếu admin bấm No | — |
| C7 — invalid state: tín hiệu từ thí sinh sai lượt | Thí sinh chưa tới lượt phát tín hiệu chọn | Tín hiệu vào hàng đợi; admin quyết định — hệ thống **cảnh báo**, không chặn cứng | Ghi tín hiệu | — |
| C8 — số thí sinh nhiều hơn số hàng ngang | Ví dụ 12 thí sinh, 4 hàng ngang | **NGOÀI PHẠM VI v1** `[v1.5]` — v1 đặc tả cho **đúng 4 thí sinh** (§2), mà 4 thí sinh / 4 hàng ngang thì *"tối đa 1 lượt"* vừa đủ. Luật cho 1-12 người thuộc v1.5 (`game-rules-inventory.md` U-4) | — | — |
| C9 — hai loại tín hiệu chờ cùng lúc | Một tín hiệu chọn hàng ngang và một tín hiệu "Mở chướng ngại vật" cùng nằm trong hàng đợi | NEEDS CLARIFICATION — `game-rules-review.md` GRR-144: chưa nói hàng đợi xử lý thuần theo thứ tự tới hay ưu tiên theo loại | Không xác định | — |

### Outcomes

- **Được xác nhận**: hàng ngang được đưa ra, lượt chọn của thí sinh đã dùng.
- **Bị từ chối**: tín hiệu kế tiếp lên, thí sinh giữ nguyên lượt.
- **Người tới lượt đã bị loại**: lượt dồn sang vị trí tiếp theo, có thể quay vòng về vị trí 1.

### State changes

- Đánh dấu lượt chọn của thí sinh đã dùng.
- Hàng ngang chuyển sang trạng thái đang được hỏi.
- Cập nhật người đang tới lượt chọn.

### No-change guarantees

- **Từ chối không làm thí sinh mất lượt.**
- **Lịch sử tín hiệu không bao giờ bị xoá.**
- Điểm **không đổi** ở bước này.

### Error outcomes

Không có lỗi nghiệp vụ. Tín hiệu sai lượt đi qua **cảnh báo**, admin vẫn ép được.

### Evaluation order

NEEDS CLARIFICATION — nguồn không quy định thứ tự kiểm tra giữa: (a) thí sinh có đúng lượt không, (b) thí sinh đã bị loại chưa, (c) hàng ngang còn chưa mở không, (d) thí sinh đã dùng lượt chọn chưa. Thứ tự này quyết định outcome của C5 và C7.

### Boundaries

| Biên | Giá trị | Quy định |
|---|---|---|
| Nhỏ hơn min | 0 hàng ngang chưa chọn | Vòng chuyển sang giai đoạn gợi ý cuối (GR-011) |
| Bằng min | **1** hàng ngang còn lại | Áp dụng bình thường |
| Trong khoảng | 2-3 hàng ngang còn lại | Áp dụng bình thường |
| Bằng max | **4** hàng ngang (chưa hỏi gì) | Lượt chọn bắt đầu từ vị trí số 1 |
| Lớn hơn max | Số hàng ngang > 4 | Ngoài luật O26; `game-rules-inventory.md` U-3 ghi nhận thang điểm cho cấu hình này chưa định nghĩa |
| Số thí sinh | 4 | Luật v1 đặc tả cho đúng 4; khác 4 xem U-4 |

### Idempotency

Nút **"Mở chướng ngại vật"** là chuông ⇒ **tự khoá ngay khi bấm**, không có tín hiệu trùng.

Thao tác **chọn hàng ngang** không phải chuông nên đi theo cơ chế riêng (`Đ-36`), và cơ chế đó khiến tín hiệu trùng **không sinh ra được** ở cả hai mode:

| Mode | Vì sao không có tín hiệu trùng |
|---|---|
| **Sân khấu** | Máy thí sinh **không có nút chọn** |
| **Nhập liệu** | Dialog xác nhận + **khoá nút** sau khi xác nhận ⇒ đúng một tín hiệu cho một lượt chọn |

> Khoá ở đây nằm tại **giao diện**. Theo `CLAUDE.md` §Zero-trust, server vẫn phải bỏ qua tín hiệu chọn trùng nếu nhận được.

NEEDS CLARIFICATION — **thao tác xác nhận của admin** bấm hai lần vẫn chưa được quy định.

### Concurrency

NEEDS CLARIFICATION — `game-rules-review.md` GRR-144: thứ tự xử lý giữa tín hiệu chọn hàng ngang và tín hiệu "Mở chướng ngại vật" trong cùng một hàng đợi chặn chưa được quy định, trong khi thứ tự đó ảnh hưởng trực tiếp tới băng điểm Chướng ngại vật (GR-009).

### Examples

- **Hợp lệ**: mode nhập liệu — thí sinh vị trí 1 click hàng ngang số 3, dialog hiện, thí sinh xác nhận ⇒ nút chọn khoá, tín hiệu vào hàng đợi; admin bấm Yes ⇒ hàng ngang 3 được đưa ra.
- **Không hợp lệ**: hệ thống tự động mở hàng ngang khi thí sinh xác nhận dialog mà không cần admin duyệt ⇒ trái quy tắc hàng đợi chặn ở VCNV. Dialog của thí sinh **không thay thế** phán quyết của admin.
- **Boundary**: thí sinh vị trí 4 vừa dùng xong lượt, còn 1 hàng ngang chưa chọn vì thí sinh vị trí 2 đã bị loại trước đó ⇒ lượt quay lại **vị trí số 1**.
- **Ngoại lệ khoá**: thí sinh xác nhận dialog ⇒ nút khoá; admin bấm **No** ⇒ nút **mở lại**, thí sinh chọn hàng khác, **không mất lượt**.

### Source traceability

| Nguồn | Heading | Discovered rule ID |
|---|---|---|
| `docs/source/fandom-olympia-26-luat-choi.md` | §Vượt chướng ngại vật, đoạn 3 | — |
| `docs/game-rules-inventory.md` | §R-VCNV-03 | R-VCNV-03, U-4 |
| `docs/reviews/game-rules-decisions.md` | §5.1 Đ-7 · §5.2 Đ-7.2 · §5.4 Đ-4.2 · §6.2 · §11.21 Đ-36 | Đ-7, Đ-7.2, Đ-4.2, Đ-36 |
| `docs/reviews/game-rules-review-old.md` | GRR-016 | — |
| `docs/reviews/game-rules-review.md` | GRR-143, GRR-144 | — |
| `docs/glossary.md` | TERM-002, TERM-032, TERM-057 | — |

---

## GR-008 — VCNV: trả lời hàng ngang và mở miếng ghép

### Status

NEEDS CLARIFICATION

### Purpose

Xác định điểm cho đáp án hàng ngang và điều kiện mở miếng ghép tương ứng.

### Actors

Thí sinh (gõ đáp án) · Admin (phán quyết, mở miếng ghép) · Server.

### Related states

`rounds[i]` = VCNV.

### Trigger

Admin **bấm hiển thị đáp án** sau khi hết thời gian suy nghĩ, rồi phán quyết đáp án hàng ngang của từng thí sinh.

### Preconditions

- Một hàng ngang đã được chọn và đưa ra.
- Thời gian suy nghĩ **15 giây** đã được khởi động.

### Inputs

- Đáp án của **tất cả** thí sinh chưa bị loại — hàng ngang **luôn trả lời bằng máy**, bất kể mode của contest.
- Thời gian suy nghĩ **15 giây**.
- Phán quyết của admin cho từng thí sinh.

### Conditions

- Trả lời đúng: **+10** cho **mỗi** thí sinh được chấm đúng.
- Trả lời sai: **0**, không trừ.
- Trả lời đúng hàng ngang ⇒ **miếng ghép tương ứng được mở**. Không trả lời được ⇒ **miếng ghép không được mở**.
- Thao tác mở miếng ghép là thao tác **của admin**, đi qua dialog xác nhận.
- **Không thứ gì tự lộ khi hết giờ** — hết giờ chỉ khoá ô nhập của thí sinh. Hai thứ sau chỉ hiện khi **admin bấm hiển thị**: (a) **đáp án chuẩn của chương trình** cho hàng ngang, (b) **bài làm của từng thí sinh**.

### Decision table

| Case | Conditions | Expected outcome | State change | Error |
|---|---|---|---|---|
| C1 — happy path | Một thí sinh được admin chấm Đúng | **+10** cho thí sinh đó; miếng ghép tương ứng được mở | Sinh event điểm; miếng ghép chuyển sang đã mở | — |
| C2 — nhiều thí sinh cùng đúng | Nhiều người được chấm Đúng cho cùng hàng ngang | **+10 cho mỗi người**; miếng ghép được mở | Sinh event điểm cho từng người | — |
| C3 — không ai đúng | Mọi thí sinh đều bị chấm Sai hoặc không trả lời | **0** cho tất cả; miếng ghép **không** được mở | Hàng ngang khép lại, miếng ghép giữ nguyên | — |
| C4 — boundary: cần bao nhiêu người đúng để mở | Đúng 1 trong 4 người trả lời đúng | NEEDS CLARIFICATION — `game-rules-inventory.md` U-37: nguồn chỉ nói *"trả lời đúng → mở"*, không nêu ngưỡng số người; biến thể *"ít nhất 1 thí sinh"* của nguồn khác đã bị loại theo K-5 | Không xác định | — |
| C5 — repeated action: admin bấm mở miếng ghép hai lần | Miếng ghép đã mở, admin bấm mở lần nữa | NEEDS CLARIFICATION — nguồn không nói thao tác mở là vô hiệu khi đã mở | Không xác định | — |
| C6 — invalid state: đáp án từ thí sinh đã bị loại | Người đã bị loại vẫn thao tác trên máy | Ô nhập **không hiển thị**, thao tác **không phản hồi** ⇒ không gửi được đáp án nào | Không đổi | — |
| C7 — đáp án đến sau khi hết 15 giây | Bản gửi đến sau mốc hết giờ | **GIỮ CẢ BẢN HỢP LỆ LẪN BẢN QUÁ HẠN**, bản quá hạn tô **đỏ**. Admin quyết **Đúng / Sai**; nếu **chỉ có** bản quá hạn thì có thêm lựa chọn **HUỶ KẾT QUẢ** | Cả hai bản hiển thị trên màn admin; hiển thị ra ngoài và chấm điểm đều **do admin bấm** | — |
| C8 — boundary: đáp án đến đúng mốc 15 giây | Server timestamp trùng mốc | **Được chấm** — biên **đóng** | Bản được ghi nhận như bản hợp lệ | — |
| C9 — có người bấm "Mở chướng ngại vật" giữa chừng | Tín hiệu CNV đến trong lúc đồng hồ 15 giây đang chạy | **Ghi nhận ngay**; đồng hồ **vẫn chạy bình thường**; **đáp án chuẩn của chương trình** và **bài làm của các thí sinh khác** đều **chưa hiển thị** cho tới khi admin bấm hiển thị | Ghi tín hiệu vào hàng đợi; đồng hồ không đổi | — |
| C10 — chính tả sai lệch nhỏ | Đáp án khác đáp án chuẩn ở dấu câu | Máy **chỉ** highlight ký tự khác; **admin tự đánh giá** và bấm Đúng/Sai | Sinh event theo phán quyết của admin | — |

### Outcomes

- **Được chấm đúng**: +10 cho từng người đúng, miếng ghép mở.
- **Không ai đúng**: không ai được điểm, miếng ghép không mở.
- Hàng ngang vẫn **được coi là đã hỏi** dù miếng ghép có mở hay không — đây là căn cứ đếm băng điểm ở GR-009.

### State changes

- Sinh event điểm cho từng thí sinh được chấm đúng.
- Miếng ghép tương ứng chuyển sang đã mở (khi có người đúng và admin thực hiện thao tác mở).
- Hàng ngang chuyển sang đã hỏi.

### No-change guarantees

- Điểm của thí sinh bị chấm sai **không đổi** — hàng ngang **không có** hình phạt.
- Miếng ghép **không** được mở khi không ai trả lời đúng.
- Trạng thái *"đã hỏi"* của hàng ngang **không** phụ thuộc việc miếng ghép có mở hay không.

### Error outcomes

Không có lỗi nghiệp vụ được định nghĩa.

### Evaluation order

NEEDS CLARIFICATION — nguồn không quy định thứ tự giữa: (a) chấm từng thí sinh, (b) xác định miếng ghép có được mở không, (c) đánh dấu hàng ngang đã hỏi. Thứ tự (b) so với (a) trở nên quan trọng khi số người đúng khác nhau — xem U-37.

### Boundaries

| Biên | Giá trị | Quy định |
|---|---|---|
| Nhỏ hơn min | Số người đúng < 0 | Không tồn tại |
| Bằng min | **0** người đúng | Miếng ghép không mở, không ai được điểm |
| Trong khoảng | 1 → 3 người đúng | Mỗi người đúng +10; ngưỡng mở miếng ghép — xem U-37 |
| Bằng max | **Tất cả** thí sinh đều đúng | Mỗi người +10; miếng ghép mở |
| Lớn hơn max | — | Không tồn tại |
| Biên thời gian | t = đúng **15.000 giây** | Submission đúng mốc **được chấm** (biên đóng) |

### Idempotency

- Bấm cùng loại phán quyết nhiều lần cho **cùng một thí sinh**: một event duy nhất.
- Bấm mở miếng ghép nhiều lần: NEEDS CLARIFICATION — nguồn không quy định.

### Concurrency

Tín hiệu **"Mở chướng ngại vật" đến giữa lúc đồng hồ hàng ngang đang chạy**: hệ thống **ghi nhận ngay**, **đồng hồ vẫn chạy bình thường** (không tạm dừng, không kéo dài), và **không lộ gì thêm cho tới khi admin bấm hiển thị** — cả **đáp án chuẩn của chương trình** lẫn **bài làm của các thí sinh khác**.

> Vế thứ ba là điều kiện để hai vế đầu an toàn. Phải ẩn **cả hai** nguồn dữ kiện: đáp án chuẩn cho người bấm CNV biết luôn hàng ngang đó là gì, còn bài làm của người khác cho họ suy ra hàng ngang qua phỏng đoán của đồng đối thủ. Ẩn một cái mà lộ cái kia thì cơ chế vẫn hỏng.

### Examples

- **Hợp lệ**: hàng ngang 2 được đưa ra, 3 thí sinh gõ đúng ⇒ mỗi người **+10**, miếng ghép số 2 được mở.
- **Không hợp lệ**: hệ thống tự cộng 10 điểm cho thí sinh vì đáp án khớp chuỗi ⇒ trái nguyên tắc máy không tự chấm.
- **Boundary**: cả 4 thí sinh đều sai ⇒ **không ai** được điểm, miếng ghép số 2 **không** mở, nhưng hàng ngang 2 vẫn tính là **đã hỏi**.

### Source traceability

| Nguồn | Heading | Discovered rule ID |
|---|---|---|
| `docs/source/fandom-olympia-26-luat-choi.md` | §Vượt chướng ngại vật, đoạn 3 và đoạn "Sau khi trả lời đúng…" | — |
| `docs/game-rules-inventory.md` | §R-VCNV-01, §R-VCNV-02 | R-VCNV-01, U-37, K-5, K-6 |
| `docs/reviews/game-rules-decisions.md` | §3.1 Đ-1 · §4.2 · §9.2 | Đ-1, Đ-4.X2 |
| `docs/reviews/game-rules-review-old.md` | GRR-002, GRR-013, GRR-019 | — |
| `docs/reviews/game-rules-review.md` | GRR-146 | — |
| `docs/glossary.md` | TERM-057, TERM-058 | — |

---

## GR-009 — VCNV: bấm chuông giải Chướng ngại vật

### Status

NEEDS CLARIFICATION

### Purpose

Xác định điểm mà thí sinh nhận được khi giải đúng Chướng ngại vật, theo thời điểm bấm chuông.

### Actors

Thí sinh (bấm chuông) · Admin (xác nhận tín hiệu, phán quyết) · Server.

### Related states

`rounds[i]` = VCNV.

### Trigger

Thí sinh bấm nút "Mở chướng ngại vật".

### Preconditions

- Trận đang ở vòng VCNV.
- Thí sinh chưa bị loại khỏi vòng này.
- Chưa có ai giải đúng Chướng ngại vật.

### Inputs

- Tín hiệu kèm server timestamp.
- **Số hàng ngang đã hỏi** tại thời điểm xét — đây là biến quyết định băng điểm, **không phải** số miếng ghép đã mở.
- Trạng thái: gợi ý cuối đã được đưa ra hay chưa.

### Conditions

- Thí sinh được bấm chuông giải Chướng ngại vật **bất cứ lúc nào**.
- Băng điểm khi trả lời đúng: **trong 1 hàng ngang → 60** · **2 hàng → 50** · **3 hàng → 40** · **4 hàng → 30**.
- Sau khi **gợi ý cuối đã được đưa ra**: chỉ còn **20**, không phụ thuộc câu ô trung tâm đúng hay sai.
- Nút "Mở chướng ngại vật" được xếp là **chuông** ⇒ chỉ nhận click chuột, phía thí sinh không có dialog.
- Hàng đợi ở VCNV **CHẶN**: admin xác nhận rồi tín hiệu mới có hiệu lực.

### Decision table

| Case | Conditions | Expected outcome | State change | Error |
|---|---|---|---|---|
| C1 — happy path | Bấm khi chưa hàng ngang nào được hỏi; admin xác nhận; admin chấm Đúng | **+60** | Sinh event điểm; vòng kết thúc | — |
| C2 — bấm sau 2 hàng ngang đã hỏi | Như C1, đã hỏi 2 hàng | **+50** | Như C1 | — |
| C3 — bấm sau 3 hàng | Như C1 | **+40** | Như C1 | — |
| C4 — bấm sau 4 hàng | Như C1 | **+30** | Như C1 | — |
| C5 — bấm sau khi gợi ý cuối đã đưa ra | Gợi ý cuối đã hiện | **+20** | Như C1 | — |
| C6 — trả lời sai | Admin chấm Sai | Chuyển GR-010 — thí sinh **bị loại** khỏi vòng | Đánh dấu thí sinh bị loại | — |
| C7 — boundary: mốc chốt băng điểm | Bấm khi đã hỏi 1 hàng, admin xác nhận sau khi hàng 2 đã bắt đầu | NEEDS CLARIFICATION — `game-rules-review.md` GRR-142: chưa nói băng điểm chốt theo server timestamp lúc bấm hay theo trạng thái lúc admin xác nhận; chênh lệch **10 điểm** | Không xác định | — |
| C8 — admin từ chối tín hiệu | Admin bấm No | Tín hiệu kế tiếp lên; **thí sinh không mất lượt** | NEEDS CLARIFICATION — `game-rules-review.md` GRR-143 (tác dụng phụ đã phát sinh hay chưa) | — |
| C9 — invalid state: bấm sau khi đã bị loại | Thí sinh đã bị loại tiếp tục bấm | Máy thí sinh **không hiển thị gì**, bấm **không phản hồi** ⇒ không có tín hiệu nào được tạo | Không đổi | — |
| C10 — invalid state: đã có người giải đúng | Vòng đã kết thúc vì có người giải đúng | Máy thí sinh **không hiển thị gì**, bấm **không phản hồi** ⇒ không có tín hiệu nào được tạo | Không đổi | — |
| C11 — số hàng ngang khác 4 | Cấu hình 5-8 hàng ngang | NEEDS CLARIFICATION — `game-rules-inventory.md` U-3: băng điểm cho cấu hình này chưa được định nghĩa | Không xác định | — |

### Outcomes

- **Giải đúng**: cộng điểm theo băng tương ứng số hàng ngang đã hỏi; vòng kết thúc.
- **Giải sai**: chuyển GR-010, thí sinh bị loại khỏi vòng.
- **Bị admin từ chối**: thí sinh không mất lượt.

### State changes

- Sinh event điểm cho người giải đúng.
- Vòng VCNV kết thúc khi có người giải đúng.

### No-change guarantees

- Điểm hàng ngang đã kiếm được trước đó của người giải sai: NEEDS CLARIFICATION — `game-rules-inventory.md` U-24 chưa nói có bị trừ không.
- **Từ chối không làm thí sinh mất lượt.**
- **Lịch sử tín hiệu không bao giờ bị xoá.**

### Error outcomes

Không có lỗi nghiệp vụ được định nghĩa.

### Evaluation order

NEEDS CLARIFICATION — nguồn không quy định thứ tự giữa: (a) chốt số hàng ngang đã hỏi để tính băng điểm, (b) admin xác nhận tín hiệu, (c) admin phán quyết đúng/sai. Thứ tự (a) so với (b) chính là nội dung `game-rules-review.md` GRR-142.

### Boundaries

| Biên | Giá trị | Quy định |
|---|---|---|
| Nhỏ hơn min | Số hàng ngang đã hỏi < 1 (chưa hàng nào bắt đầu) | Vẫn thuộc băng **60** |
| Bằng min | Đã hỏi **1** hàng | **60** |
| Trong khoảng | Đã hỏi 2-3 hàng | **50** · **40** |
| Bằng max | Đã hỏi **4** hàng | **30** |
| Lớn hơn max | Sau khi gợi ý cuối đã đưa ra | **20** |
| Ngoài luật | Cấu hình 5-8 hàng ngang | NEEDS CLARIFICATION — U-3 |

### Idempotency

Nút "Mở chướng ngại vật" là **chuông** ⇒ **tự khoá ngay khi bấm** (`Đ-24`), nên một thí sinh chỉ phát được **một** tín hiệu cho một vòng. Cơ chế *"một lần đoán, sai thì loại"* của luật gốc được giữ nguyên.

### Concurrency

NEEDS CLARIFICATION — `game-rules-review.md` GRR-144: khi một tín hiệu chọn hàng ngang đang chờ admin duyệt và một tín hiệu "Mở chướng ngại vật" đến, thứ tự xử lý chưa được quy định, mà thứ tự đó làm đổi băng điểm.

### Examples

- **Hợp lệ**: chưa hàng ngang nào được hỏi, thí sinh vị trí 4 bấm "Mở chướng ngại vật", admin xác nhận, đáp án đúng ⇒ **+60**, vòng kết thúc.
- **Không hợp lệ**: thí sinh gán phím tắt cho nút "Mở chướng ngại vật" ⇒ không có đường phát tín hiệu; nút này là chuông, chỉ nhận click chuột.
- **Boundary**: thí sinh bấm khi vừa hỏi xong hàng ngang thứ 4 nhưng gợi ý cuối chưa được đưa ra ⇒ **30**, không phải 20.

### Source traceability

| Nguồn | Heading | Discovered rule ID |
|---|---|---|
| `docs/source/fandom-olympia-26-luat-choi.md` | §Vượt chướng ngại vật, đoạn "Thí sinh có thể bấm chuông…" | — |
| `docs/game-rules-inventory.md` | §R-VCNV-04, §R-VCNV-06 | R-VCNV-04, U-3, U-16, U-24 |
| `docs/reviews/game-rules-decisions.md` | §5.2 Đ-7.2 · §5.3 Đ-4.3 · §9.2 | Đ-4.3, Đ-7.2 |
| `docs/reviews/game-rules-review-old.md` | GRR-019, GRR-021, GRR-025 | — |
| `docs/reviews/game-rules-review.md` | GRR-142, GRR-143, GRR-144, GRR-145, GRR-146 | — |
| `docs/glossary.md` | TERM-059 | — |

---

## GR-010 — VCNV: trả lời sai Chướng ngại vật

### Status

NEEDS CLARIFICATION

### Purpose

Xác định hệ quả khi thí sinh giải sai Chướng ngại vật.

### Actors

Thí sinh · Admin (phán quyết) · Server.

### Related states

`rounds[i]` = VCNV.

### Trigger

Admin bấm Sai cho một tín hiệu giải Chướng ngại vật đã được xác nhận.

### Preconditions

- Thí sinh đã bấm "Mở chướng ngại vật" và admin đã xác nhận tín hiệu.
- Thí sinh chưa bị loại trước đó.

### Inputs

- Thí sinh phát tín hiệu.
- Phán quyết Sai của admin.

### Conditions

- *"Nếu trả lời sai Chướng ngại vật, thí sinh sẽ **bị loại khỏi phần thi này**."*
- Nguồn **không** nêu hình phạt trừ điểm cho việc giải sai.
- "Bị loại" có phạm vi **một vòng** — thí sinh vẫn thi các vòng sau và vẫn có thể thắng trận.

### Decision table

| Case | Conditions | Expected outcome | State change | Error |
|---|---|---|---|---|
| C1 — happy path | Admin bấm Sai | Thí sinh **bị loại khỏi vòng VCNV**; **không** trừ điểm | Đánh dấu thí sinh bị loại | — |
| C2 — lượt chọn của người bị loại | Người bị loại chưa dùng lượt chọn hàng ngang | Lượt dồn sang vị trí tiếp theo (GR-007) | Cập nhật người tới lượt | — |
| C3 — điểm hàng ngang đã kiếm | Người bị loại đã được +10 từ hàng ngang trước đó | NEEDS CLARIFICATION — `game-rules-inventory.md` U-24: chưa nói có bị trừ lại không | Không xác định | — |
| C4 — invalid state: người đã bị loại tiếp tục thao tác | Sự kiện đến từ ghế đã bị loại | Máy thí sinh **không hiển thị gì**, bấm **không phản hồi** ⇒ không có tín hiệu nào được tạo | Không đổi | — |
| C5 — boundary: người cuối cùng bị loại | Mọi thí sinh đều đã bị loại | Chuyển GR-012 | Vòng kết thúc | — |
| C6 — thi đội | Một thành viên trả lời sai | Loại **cả đội** — luật v2, chưa hiện thực | Đánh dấu cả đội bị loại | — |

### Outcomes

- **Bị loại khỏi vòng VCNV**: mất quyền trả lời hàng ngang, mất quyền giải Chướng ngại vật, mất lượt chọn chưa dùng.
- **Không** mất điểm đã có (mức độ chắc chắn: xem U-24).
- **Không** rời khỏi trận.

### State changes

- Đánh dấu thí sinh ở trạng thái bị loại khỏi vòng VCNV.
- Cập nhật thứ tự lượt chọn hàng ngang nếu cần.

### No-change guarantees

- Thí sinh **không** bị loại khỏi trận; các vòng sau vẫn tham gia bình thường.
- Nguồn **không** quy định hình phạt trừ điểm ⇒ không được tự thêm.
- Điểm của các thí sinh khác không đổi.

### Error outcomes

Không có lỗi nghiệp vụ được định nghĩa.

### Evaluation order

Không còn thứ tự nào phải quy định: (c) **không tồn tại** — nút chuông tự khoá sau lần bấm đầu (`Đ-24`) nên người đó không có tín hiệu nào khác trong hàng đợi. Còn (a) và (b) là hai hệ quả độc lập của cùng một phán quyết, thứ tự nào cũng ra cùng kết quả.

### Boundaries

| Biên | Giá trị | Quy định |
|---|---|---|
| Nhỏ hơn min | 0 người bị loại | Vòng chạy bình thường |
| Bằng min | **1** người bị loại | Lượt chọn dồn sang vị trí tiếp theo |
| Trong khoảng | 2-3 người bị loại | Vòng vẫn chạy với người còn lại |
| Bằng max | **Tất cả** thí sinh bị loại | Chuyển GR-012 |
| Lớn hơn max | — | Không tồn tại |
| Số lần đoán mỗi người | Luật hàm ý **1** — sai là loại | Xem `game-rules-review.md` GRR-145 về nguy cơ phá giới hạn này |

### Idempotency

Bấm lại nút chấm: KHÔNG XẢY RA — nút khoá sau lần bấm đầu (`Đ-17`). Cũng không có **nhiều tín hiệu** của cùng người trong hàng đợi: nút chuông tự khoá ngay khi bấm (`Đ-24`).

### Concurrency

NEEDS CLARIFICATION — nguồn không quy định điều gì xảy ra khi phán quyết Sai được chốt trong lúc đồng hồ hàng ngang đang chạy, hoặc khi người này đồng thời có một đáp án hàng ngang đang chờ chấm.

### Examples

- **Hợp lệ**: thí sinh vị trí 1 giải Chướng ngại vật sai ⇒ bị loại khỏi VCNV, vẫn thi Tăng tốc và Về đích bình thường.
- **Không hợp lệ**: hệ thống trừ 30 điểm của thí sinh vì giải sai ⇒ nguồn không quy định hình phạt điểm.
- **Boundary**: người thứ tư giải sai ⇒ toàn bộ thí sinh đã bị loại, chuyển GR-012.

### Source traceability

| Nguồn | Heading | Discovered rule ID |
|---|---|---|
| `docs/source/fandom-olympia-26-luat-choi.md` | §Vượt chướng ngại vật, câu cuối | — |
| `docs/game-rules-inventory.md` | §R-VCNV-04, §R-VCNV-07, §PHẦN 7 R-TEAM-03 | R-VCNV-04, U-24 |
| `docs/reviews/game-rules-decisions.md` | §7.3 (hoàn nguyên trạng thái phi-điểm — **đề xuất chưa duyệt**) | — |
| `docs/reviews/game-rules-review.md` | GRR-145, GRR-146 | — |
| `docs/glossary.md` | TERM-036, TERM-046 | — |

---

## GR-011 — VCNV: ô trung tâm và gợi ý cuối

### Status

NEEDS CLARIFICATION

### Purpose

Xác định điều kiện đưa ra gợi ý cuối ở ô trung tâm và điểm cho giai đoạn cuối của vòng VCNV.

### Actors

Thí sinh · Admin · Server.

### Related states

`rounds[i]` = VCNV, giai đoạn cuối vòng.

### Trigger

Cả 4 hàng ngang đã được hỏi xong mà chưa có thí sinh nào giải đúng Chướng ngại vật.

### Preconditions

- Cả 4 hàng ngang đã được đưa ra.
- Chưa ai giải đúng Chướng ngại vật.
- Còn ít nhất một thí sinh chưa bị loại.

### Inputs

- Câu hỏi ở ô trung tâm.
- Thời gian suy nghĩ để đưa ra Chướng ngại vật sau gợi ý cuối: **15 giây**.

### Conditions

- Câu hỏi ô trung tâm: trả lời đúng **+10** và ô trung tâm được mở; trả lời sai thì **ô trung tâm không được mở**.
- Sau gợi ý cuối, giải đúng Chướng ngại vật chỉ được **20 điểm**.
- Mốc 20 điểm gắn với việc **gợi ý cuối đã được đưa ra**, **không** phụ thuộc câu ô trung tâm đúng hay sai.
- Điều kiện kích hoạt phải đọc theo hai chủ ngữ khác nhau của nguồn: *"từ hàng ngang được mở"* ≠ *"miếng ghép được mở"* ⇒ cả 4 hàng ngang **luôn được hỏi hết**, nên giai đoạn này **luôn tới được**.

### Decision table

| Case | Conditions | Expected outcome | State change | Error |
|---|---|---|---|---|
| C1 — happy path | 4 hàng ngang đã hỏi, chưa ai giải Chướng ngại vật | Gợi ý cuối được đưa ra ở ô trung tâm | Chuyển giai đoạn; băng điểm Chướng ngại vật hạ xuống **20** | — |
| C2 — trả lời đúng câu ô trung tâm | Admin chấm Đúng | **+10**; ô trung tâm được mở | Sinh event điểm; ô trung tâm mở | — |
| C3 — trả lời sai câu ô trung tâm | Admin chấm Sai | Ô trung tâm **không** được mở; băng điểm Chướng ngại vật **vẫn là 20** | Không sinh điểm | — |
| C4 — giải đúng Chướng ngại vật sau gợi ý cuối | Trong 15 giây | **+20** | Sinh event điểm; vòng kết thúc | — |
| C5 — ai được trả lời câu ô trung tâm | Cả sân hay theo lượt | NEEDS CLARIFICATION — `game-rules-inventory.md` U-25 | Không xác định | — |
| C6 — có hàng ngang không ai trả lời đúng | Miếng ghép của hàng đó không mở | Giai đoạn này **vẫn** tới được — điều kiện là *hàng ngang đã được hỏi*, không phải *miếng ghép đã mở* | Chuyển giai đoạn bình thường | — |
| C7 — boundary: hết 15 giây không ai giải | Không có tín hiệu giải Chướng ngại vật | Chuyển kết thúc vòng | Vòng khép lại | — |
| C8 — repeated action: admin bấm mở ô trung tâm hai lần | Ô đã mở, bấm lại | NEEDS CLARIFICATION — nguồn không quy định | Không xác định | — |

### Outcomes

- **Gợi ý cuối được đưa ra**: băng điểm Chướng ngại vật hạ còn 20.
- **Câu ô trung tâm đúng**: +10 và ô mở.
- **Câu ô trung tâm sai**: ô không mở, nhưng gợi ý cuối vẫn đã được đưa ra.
- **Giải đúng Chướng ngại vật sau đó**: +20.

### State changes

- Chuyển vòng sang giai đoạn gợi ý cuối.
- Mở ô trung tâm khi câu ô trung tâm được chấm đúng.
- Sinh event điểm tương ứng.

### No-change guarantees

- Miếng ghép của các hàng ngang **không** bị thay đổi bởi rule này.
- Việc đã đưa ra gợi ý cuối **không** bị hoàn tác bởi kết quả sai của câu ô trung tâm.
- Điểm của thí sinh đã bị loại không đổi.

### Error outcomes

Không có lỗi nghiệp vụ được định nghĩa.

### Evaluation order

NEEDS CLARIFICATION — nguồn không quy định thứ tự giữa: (a) chấm câu ô trung tâm, (b) mở ô trung tâm, (c) mở cửa sổ 15 giây để giải Chướng ngại vật. Trình tự này ảnh hưởng ai kịp bấm chuông trong 15 giây đó.

### Boundaries

| Biên | Giá trị | Quy định |
|---|---|---|
| Nhỏ hơn min | < 4 hàng ngang đã hỏi | Giai đoạn chưa kích hoạt |
| Bằng min | Đúng **4** hàng ngang đã hỏi | Kích hoạt gợi ý cuối |
| Trong khoảng | Trong 15 giây sau gợi ý cuối | Còn cơ hội giải Chướng ngại vật với **20** điểm |
| Bằng max | t = đúng **15.000 giây** | Tín hiệu giải Chướng ngại vật đúng mốc **vẫn hợp lệ** (biên đóng) |
| Lớn hơn max | Sau 15 giây | Vòng khép lại |
| Số người còn lại | ≥1 người chưa bị loại | 0 người → chuyển GR-012 |

### Idempotency

NEEDS CLARIFICATION — thao tác mở ô trung tâm và thao tác đưa ra gợi ý cuối bấm lặp lại chưa được quy định. Riêng nút start timer thì đã có quy tắc: **tự khoá sau lần bấm đầu**.

### Concurrency

NEEDS CLARIFICATION — nguồn không quy định điều gì xảy ra khi một tín hiệu giải Chướng ngại vật đến đúng lúc admin đang đưa ra gợi ý cuối. Đây chính là biên giữa băng **30** và băng **20** — chênh lệch 10 điểm, cùng họ với `game-rules-review.md` GRR-142.

### Examples

- **Hợp lệ**: 4 hàng ngang đã hỏi, 2 miếng ghép không mở vì không ai đúng; gợi ý cuối vẫn được đưa ra; thí sinh giải đúng Chướng ngại vật ⇒ **+20**.
- **Không hợp lệ**: hệ thống chờ đủ 4 miếng ghép mở mới cho ra gợi ý cuối ⇒ đọc sai nguồn; điều kiện là hàng ngang **đã được hỏi**.
- **Boundary**: thí sinh bấm chuông giải Chướng ngại vật ngay trước khi gợi ý cuối được đưa ra ⇒ thuộc băng **30**; bấm ngay sau ⇒ **20**.

### Source traceability

| Nguồn | Heading | Discovered rule ID |
|---|---|---|
| `docs/source/fandom-olympia-26-luat-choi.md` | §Vượt chướng ngại vật, đoạn "Sau khi cả 4 từ hàng ngang…" | — |
| `docs/game-rules-inventory.md` | §R-VCNV-05 | R-VCNV-05, U-25, K-7 |
| `docs/reviews/game-rules-decisions.md` | §9.2 · §10 (cách đọc hai chủ ngữ) | Đ-12 |
| `docs/reviews/game-rules-review-old.md` | GRR-002, GRR-013, GRR-025 | — |
| `docs/reviews/game-rules-review.md` | GRR-142 | — |
| `docs/glossary.md` | TERM-060 | — |

---

## GR-012 — VCNV: toàn bộ thí sinh bị loại

### Status

NEEDS CLARIFICATION

### Purpose

Xác định cách kết thúc vòng VCNV khi không còn thí sinh nào đủ điều kiện tiếp tục.

### Actors

Admin (thao tác thủ công) · Viewer (theo dõi) · Server.

### Related states

`rounds[i]` = VCNV → `INTERMISSION`.

### Trigger

Thí sinh cuối cùng chưa bị loại giải sai Chướng ngại vật.

### Preconditions

Mọi thí sinh đều đã ở trạng thái bị loại khỏi vòng VCNV.

### Inputs

- Trạng thái bị loại của từng thí sinh.
- Danh sách hàng ngang chưa được hỏi.

### Conditions

- Vòng kết thúc; **không ai được điểm Chướng ngại vật**.
- Hàng ngang chưa hỏi **bị bỏ**.
- Việc **mở toàn bộ miếng ghép và công bố Chướng ngại vật** là **thao tác thủ công của admin**, không tự động.

### Decision table

| Case | Conditions | Expected outcome | State change | Error |
|---|---|---|---|---|
| C1 — happy path | Mọi thí sinh đã bị loại | Vòng kết thúc; không ai được điểm Chướng ngại vật | Vòng chuyển sang kết thúc; hàng ngang chưa hỏi bị bỏ | — |
| C2 — admin công bố Chướng ngại vật | Admin bấm thao tác mở toàn bộ | Mở mọi miếng ghép và hiện Chướng ngại vật cho viewer | Trạng thái hiển thị thay đổi | — |
| C3 — admin không bấm công bố | Admin bỏ qua thao tác này | **Không** phải trạng thái tắc — admin luôn chuyển vòng được | Vòng vẫn kết thúc được | — |
| C4 — hàng ngang chưa hỏi | Còn 2 hàng ngang chưa được chọn | Bị bỏ; câu tương ứng vẫn tính là **đã dùng** | Câu không trả lại kho đề | — |
| C5 — repeated action: bấm công bố hai lần | Đã mở toàn bộ, bấm lại | NEEDS CLARIFICATION — nguồn không quy định | Không xác định | — |
| C6 — invalid state: tín hiệu đến sau khi vòng đã kết thúc | Thí sinh vẫn bấm chuông | Máy thí sinh **không hiển thị gì**, bấm **không phản hồi** ⇒ không có tín hiệu nào được tạo | Không đổi | — |
| C7 — boundary: chỉ còn 1 người chưa bị loại | Chưa thoả điều kiện | Vòng chạy tiếp với người đó | Không đổi | — |
| C8 — điểm hàng ngang đã kiếm | Các thí sinh đã có điểm hàng ngang trước khi bị loại | NEEDS CLARIFICATION — `game-rules-inventory.md` U-24 | Không xác định | — |

### Outcomes

- Vòng VCNV kết thúc mà không ai được điểm Chướng ngại vật.
- Hàng ngang chưa hỏi bị bỏ, câu tương ứng vẫn tiêu khỏi kho đề.
- Việc công bố Chướng ngại vật cho khán giả do admin chủ động thực hiện.

### State changes

- Vòng VCNV chuyển sang kết thúc.
- Khi admin bấm công bố: mọi miếng ghép và Chướng ngại vật chuyển sang hiển thị.

### No-change guarantees

- Điểm hàng ngang đã kiếm được **không** bị thay đổi bởi rule này (mức độ chắc chắn: xem U-24).
- Câu hỏi của các hàng ngang bị bỏ **không** được trả lại kho đề.
- Lịch sử tín hiệu không bị xoá.

### Error outcomes

Không có lỗi nghiệp vụ được định nghĩa.

### Evaluation order

NEEDS CLARIFICATION — nguồn không quy định thứ tự giữa: (a) đánh dấu vòng kết thúc, (b) bỏ các hàng ngang chưa hỏi, (c) thao tác công bố của admin. Riêng (c) đã được xác nhận là **không bắt buộc** để (a) xảy ra.

### Boundaries

| Biên | Giá trị | Quy định |
|---|---|---|
| Nhỏ hơn min | 0 người bị loại | Rule không áp dụng |
| Bằng min | Số người bị loại = số thí sinh − 1 | Rule chưa áp dụng; vòng chạy tiếp |
| Trong khoảng | — | — |
| Bằng max | **Toàn bộ** thí sinh bị loại | Áp dụng C1 |
| Lớn hơn max | — | Không tồn tại |
| Hàng ngang chưa hỏi | 0 → 4 | Bao nhiêu cũng bị bỏ |

### Idempotency

NEEDS CLARIFICATION — thao tác công bố Chướng ngại vật của admin bấm lặp lại chưa được quy định; thao tác kết thúc vòng bấm lặp lại cùng dạng `game-rules-review.md` GRR-163.

### Concurrency

NEEDS CLARIFICATION — nguồn không quy định điều gì xảy ra với các tín hiệu còn nằm trong hàng đợi tại thời điểm vòng kết thúc. Hàng đợi **đang hoạt động** được đặt lại sau mỗi vòng, nhưng lịch sử tín hiệu thì giữ vĩnh viễn.

### Examples

- **Hợp lệ**: cả 4 thí sinh đều giải sai Chướng ngại vật ⇒ vòng kết thúc, không ai được điểm Chướng ngại vật; admin bấm công bố để khán giả thấy đáp án.
- **Không hợp lệ**: hệ thống tự động mở toàn bộ miếng ghép ngay khi người cuối bị loại ⇒ đây phải là thao tác thủ công của admin.
- **Boundary**: còn đúng 1 thí sinh chưa bị loại ⇒ rule chưa áp dụng, vòng tiếp tục với người đó.

### Source traceability

| Nguồn | Heading | Discovered rule ID |
|---|---|---|
| `docs/game-rules-inventory.md` | §R-VCNV-07 | R-VCNV-07, U-24 |
| `docs/reviews/game-rules-decisions.md` | §5.1 Đ-7.b · §6.3 Đ-5.2f · §9.2 | Đ-7.b, Đ-5.2f |
| `docs/reviews/game-rules-review-old.md` | GRR-027 | — |
| `docs/reviews/game-rules-review.md` | GRR-163 | — |
| `docs/glossary.md` | TERM-024, TERM-036, TERM-054 | — |

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

NEEDS CLARIFICATION

### Purpose
Quy định xử lý khi hai hay nhiều thí sinh trả lời đúng trong cùng một khoảng thời gian (hoà tốc độ).

### Actors
- Thí sinh: trả lời câu cùng thời điểm
- Server: so sánh timestamp, xếp hạng
- Admin: chấm Đúng/Sai

### Related states
- Vòng Tăng tốc, đã ghi nhận submission từ R-TT-03
- Trạng thái timestamp của ≥2 người: `server-received` bằng nhau (**millisecond**, không phải chữ số thập phân, K-8)

### Trigger
Admin chấm Đúng ≥2 thí sinh của cùng một câu, có server-received timestamp bằng nhau (đến mức millisecond).

### Preconditions
- Vòng Tăng tốc đang chạy
- ≥2 thí sinh nộp submission với **server-received timestamp hoàn toàn giống nhau** (R-GEN-05)
- Nội dung submission (sau trim) khác nhau hoặc giống nhau (điều này không ảnh hưởng quyết định hoà)

### Inputs
- Tập người hoà: `{TS1, TS2, … TSn}` có timestamp bằng nhau
- Vị trí thứ hạng hiện tại của tập hoà (trước khi chấm)
- Thang điểm câu: [40, 30, 20, 10]

### Conditions
1. Độ phân giải so sánh: **millisecond** (R-GEN-05 điểm, K-8)
2. Tất cả người hoà nhận **cùng mức điểm** (đó là điểm thứ hạng của nhóm)
3. Thứ hạng tiếp theo nhảy qua số người hoà (xem C2, C4 dưới)

### Decision table

| Case | Conditions | Expected outcome | State change | Error |
|---|---|---|---|---|
| C1 | 4 TS, 2 người (A, B) cùng timestamp 100ms trả lời đúng, 1 người (C) 150ms | A +40, B +40 (cùng 1st) · C +10 (skip 2nd và 3rd, nhảy to 4th) | EventLog: SCORE_ADJUST ×2 → 40, ×1 → 10 | — |
| C2 | 4 TS, 3 người (A, B, C) cùng timestamp 100ms đúng, 1 người (D) 150ms | A +40, B +40, C +40 · D +0 (4 người nhưng chỉ 3 đúng → không có hạng 4) | EventLog: SCORE_ADJUST ×3 → 40, D không sinh event | — |
| C3 | 4 TS, 1 người (A) 100ms, 2 người (B, C) cùng 150ms, 1 người (D) 200ms | A +40 · B +30, C +30 (cùng 2nd) · D +10 | EventLog: SCORE_ADJUST ×4 (A→40, B→30, C→30, D→10) | — |
| C4 | 4 TS, 1 người (A) 100ms, 2 người (B, C) cùng 150ms, không có người 200ms | A +40 · B +30, C +30 · không tính hạng 4 | EventLog: SCORE_ADJUST ×3 | — |
| C5 | 4 TS, 2 người (A, B) timestamp 100.001ms vs 100.002ms (khác millisecond) | **Xem GR-015**: đây là vấn đề last-wins ghi nhận, không phải hoà tốc độ → A bấm trước → A +40, B +30 | EventLog: SCORE_ADJUST A→40, B→30 | — |

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
- **Độ phân giải khác millisecond** (ví dụ so sánh đến microsecond): **NEEDS CLARIFICATION** — SPEC §6 có option `tieRule: 'microsecond'`, nhưng K-8 chốt millisecond cho O26 (R-TT-02); chưa nói default là gì
- **Chỉ 1 người đúng**: không phải hoà, không tính luật này
- **Khác millisecond nhưng gần nhất**: theo GR-015 (last-wins, không phải hoà)

### Evaluation order
1. Sau khi admin chấm Đúng cho ≥2 TS
2. Server so sánh server-received timestamp của tất cả người đúng, độ phân giải **millisecond**
3. Nhóm người có timestamp bằng nhau = "hoà"
4. Gán cùng mức điểm cho nhóm
5. Tính hạng tiếp theo dựa trên số người hoà

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
- 4 TS (A, B, C, D): A trả lời 100ms, B trả lời 100ms (hoà A), C trả lời 150ms, D không trả lời. Admin chấm Đúng A/B/C → A +40, B +40 (hoà A ở hạng 1), C +10 (nhảy to 4th vì 2 người hoà hạng 1).
- 4 TS: A 100ms, B 100ms, C 100ms (hoà A/B), D 150ms. Admin chấm Đúng A/B/C/D → A/B/C +40 (hoà), D +0 (không đủ hạng).

**Không hợp lệ:**
- A 100.001ms, B 100.002ms → không hoà (khác millisecond) → không áp GR-014, theo GR-015 (last-wins).

### Source traceability
- **Định nghĩa hoà tốc độ**: `game-rules-inventory.md` R-TT-02 · `game-rules-decisions.md` §3.4 (một câu = một event)
- **Độ phân giải**: `game-rules-inventory.md` R-TT-02 (K-8 chốt millisecond thay vì 2 chữ số thập phân của W26)
- **Server time**: `game-rules-inventory.md` R-GEN-05
- **Quy tắc xếp hạng**: `docs/source/fandom-olympia-26-luat-choi.md` §Tăng tốc (nguyên văn "trong cùng một khoảng thời gian")

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

NEEDS CLARIFICATION

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
| C4 | **NEEDS CLARIFICATION** — 4 TS hoà điểm: A/B/C/D =100đ với pos 1/2/3/4 | **Tie-break theo "vị trí thấp nhất" = A (pos 1)** | Recommend A, nhưng tie-break luật (R-VD-02) chưa định nghĩa "vị trí" = ghế hay thứ hạng (U-27) | Chưa chốt "vị trí" là gì |
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
- **Tie-break toàn bộ** (4 người hoà): **NEEDS CLARIFICATION** (U-27, U-17)

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

NEEDS CLARIFICATION

### Purpose
Quy định thí sinh chọn 3 câu từ 2 mức điểm {20, 30} để tạo thành gói cá nhân trước khi bắt đầu thi lượt đó.

### Actors
- Thí sinh: chọn gói câu
- Server: xác nhận hợp lệ, lưu lựa chọn
- Admin: chọn hộ nếu TS chưa chọn khi tới lượt (default 20/20/20)

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
| C3 | TS chưa chọn khi tới lượt (lượt bắt đầu) | **Admin chọn hộ: default 20/20/20** | `packageChoice[seat] = [20, 20, 20]` · admin có thể override trong dialog | — |
| C4 | **NEEDS CLARIFICATION** — TS có thể đổi gói sau khi chọn không? | Hiện không có định nghĩa → assume không đổi được (chốt khi lựa chọn) | `packageChoice` lock, chỉ nở lại ở lượt tiếp theo (nếu có) | — |

### Outcomes
- **Chấp nhận**: ghi nhận lựa chọn, chuẩn bị draw 3 câu
- **Từ chối**: hiện UI error, yêu cầu chọn lại (client-side validate)
- **Chưa chọn**: admin chọn hộ default 20/20/20 (server-side fallback)

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
1. TS vào màn chọn gói (hoặc admin chọn hộ nếu TS không chọn kịp)
2. Nhập lựa chọn: click 3 nút / chọn dropdown (UI tuỳ design)
3. Client validate (3 item, mỗi item ∈ {20, 30})
4. Confirm → server lưu lựa chọn
5. Lệnh draw 3 câu (câu 1 từ mức `choice[0]`, câu 2 từ `choice[1]`, câu 3 từ `choice[2]`)

### Boundaries
- **Số câu = 0**: không chọn được (UI require ≥1, nhưng luật bắt buộc 3)
- **Số câu = 1-2**: UI yêu cầu chọn 3
- **Số câu = 3**: hợp lệ
- **Số câu ≥ 4**: UI reject (maximum 3)

### Idempotency
Chọn 2 lần gói {20/30/30} → giữ lựa chọn đầu tiên (không cập nhật). Chọn **đổi** lựa chọn: **NEEDS CLARIFICATION** (U-26).

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

NEEDS CLARIFICATION

### Purpose
Định nghĩa khi câu được đánh dấu là "thực hành", thí sinh được cung cấp dụng cụ thực hành, và chấm điểm là "thực hành đạt yêu cầu" (không phải so khớp text).

### Actors
- Thí sinh (người thi chính): thực hành với dụng cụ
- MC / Ban cố vấn / Khách mời: giới thiệu dụng cụ (per W26, không spec v1)
- Admin: chấm "thực hành đạt yêu cầu"
- Setter: đánh dấu câu là "thực hành" (chưa có field — U-6)

### Related states
- Vòng Về đích
- Câu được draw từ gói đã chọn
- **Thuộc tính câu**: **NEEDS CLARIFICATION** — không có trường dữ liệu trong SPEC §9 để khai "câu thực hành" (U-6)

### Trigger
Câu được draw, nó có thuộc tính "thực hành" (chưa định nghĩa cách khai).

### Preconditions
- Câu được đánh dấu là "thực hành" (chưa có cơ chế)
- TS là người thi chính lượt này
- Dụng cụ thực hành có sẵn (ví dụ rubik cube, bàn cờ — không định nghĩa v1)
- MC / Ban cố vấn có mặt để giới thiệu (per W26)

### Inputs
- Câu 20đ vs 30đ (xác định thời gian)
- Thời gian suy nghĩ + thực hành: bảng R-VD-04
  - 20đ: 15s suy + **30s thực hành** (cướp: 20s)
  - 30đ: 20s suy + **60s thực hành** (cướp: 40s)
- Tiêu chí "đạt yêu cầu" (không định nghĩa, để admin đánh giá)

### Conditions
1. Khi câu là thực hành → timer **riêng** thành 2 phase: suy nghĩ + thực hành
2. **Admin chấm**: "đạt yêu cầu" (Yes/No) hoặc **miễn chấm = Sai** (nếu bấy lâu chưa làm xong)
3. Nếu **đạt**: +value (20 hoặc 30)
4. Nếu **không đạt**: 0 điểm (khác câu suy giải) → mở cửa sổ cướp

### Decision table

| Case | Conditions | Expected outcome | State change | Error |
|---|---|---|---|---|
| C1 | Câu 20đ thực hành: TS suy 15s, thực hành 30s, admin chấm "đạt" | **+20 điểm** (không mở cướp) | EventLog: SCORE_ADJUST +20 | — |
| C2 | Câu 20đ thực hành: TS suy 15s, thực hành 20s (còn 10s mà chưa làm xong), admin chấm "không đạt" | **0 điểm** + **mở cửa sổ cướp 20s** (20s = thời gian cướp 20đ) | EventLog: 0 · queue cửa sổ cướp (Đ-7.2) | — |
| C3 | Câu 30đ thực hành: TS suy 20s, thực hành 60s, admin chấm "đạt" | **+30 điểm** | EventLog: SCORE_ADJUST +30 | — |
| C4 | **NEEDS CLARIFICATION** — Câu 30đ thực hành, cướp đúng: người cướp thực hành 40s hay làm gì? | Bảng R-VD-04 nói người cướp "thực hành 20s (20đ) hoặc 40s (30đ)", nhưng không nói cách xác định câu **cướp đó** là hay không thực hành (U-6 — không khai được) | — | Chưa định nghĩa hành vi |
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
- **Tiêu chí "đạt" chưa định nghĩa** (U-6): admin tự quyết (ADVISORY MODEL, Đ-5)
- **Không khai được câu thực hành** (U-6): NEEDS CLARIFICATION — không field trong schema để khai (xem SPEC §9)

### Evaluation order
1. Draw câu (GR-017)
2. Check câu là "thực hành" (chưa có cơ chế)
3. Timer 2 phase: suy nghĩ, sau đó thực hành
4. TS thực hành trong thời gian quy định
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
- Không có field "thực hành" để khai ở setter → **NEEDS CLARIFICATION** (U-6).
- Câu 20đ "thực hành", người cướp lấy 20s → không biết người cướp có cần thực hành không (U-6).

### Source traceability
- **Thời gian thực hành**: `game-rules-inventory.md` R-VD-04 (bảng 20đ vs 30đ)
- **Chấm "đạt yêu cầu"**: `game-rules-inventory.md` R-VD-04 ("chấm 'đạt yêu cầu'")
- **Cướp thực hành**: `game-rules-inventory.md` R-VD-04 (bảng "khi cướp")
- **Chưa có field dữ liệu**: `SPEC` §9 (Question fields: `displayId, fieldId, wordCount, explanation, note, timeSeconds?, value?, clues[]?, everPublic` — chưa khai "thực hành")

---

## GR-020 — Về đích: cướp quyền

### Status

NEEDS CLARIFICATION

### Purpose
Khi người thi chính trả lời sai (hoặc hết giờ) câu Về đích, 3 TS còn lại được bấm chuông để giành quyền trả lời trong cửa sổ 5 giây. Người cướp đúng lấy điểm từ người sai (transfer); cướp sai bị trừ nửa giá trị.

### Actors
- 3 TS còn lại: bấm chuông trong 5s (RO không áp — không nhịp chơi này)
- Server: ghi nhận timestamp, xếp hạng (first-come / first-buzz)
- Admin: chấm Đúng/Sai của người cướp (hoặc **NEEDS CLARIFICATION**: chấm cũng chưa chốt xử lý cần thiều để auto)

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
| C5 | Câu 20đ, TS[i] sai + NSHV sai (−20 từ NSHV, hình phạt thay thế transfer) → TS[j] cướp đúng | TS[i] −20 (từ NSHV, thay thế −20 transfer + người cướp lấy được) → TS[j] +0 hoặc +20? | **NEEDS CLARIFICATION** (U-8) — số học khi NSHV × transfer chưa định | Conflict GR-021 × GR-020 |
| C6 | Số người còn lại ≠ 3 (ghế ≠ 4) | `[v1.5]` — v1 đặc tả cho đúng 4 thí sinh nên *"3 thí sinh còn lại"* luôn đúng nghĩa; dải 1-12 thuộc v1.5 (U-5). Riêng trường hợp **runtime tụt dưới 4 giữa trận** đã chốt: **admin quyết**, hệ thống chỉ cảnh báo (§6.4) | — | — |

### Outcomes
- **Cướp Đúng**: người cướp +value (lấy từ TS[i]), TS[i] −value (transfer)
- **Cướp Sai**: người cướp −(value/2), TS[i] vẫn −value (không hoàn lại)
- **Không ai cướp**: TS[i] −value, không ai nhận được
- **Nếu NSHV TS[i] sai**: **NEEDS CLARIFICATION** (U-8) — kết hợp GR-021 + GR-020 chưa định

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
- **Giá trị lẻ −½ value lẻ**: **NEEDS CLARIFICATION** (U-20 — làm tròn thế nào?)
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
- **Nửa giá trị = 10 (20/2) / 15 (30/2)**: đều là số nguyên. Giá trị lẻ **không phát sinh dưới luật 2026** (`Đ-3`); chỉ xuất hiện nếu admin cấu hình mức điểm lẻ, và quy tắc làm tròn cho trường hợp đó vẫn chưa chốt (U-20).

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

NEEDS CLARIFICATION

### Purpose
Quyền đặt cược lên câu hỏi Về đích của TS: đúng → ×2 điểm câu, sai → −value, bất kể có người cướp hay không. Mỗi TS 1 lần/trận.

### Actors
- Thí sinh (người thi chính): bấm nút NSHV trước khi câu được đọc/hiện
- Admin: duyệt tín hiệu NSHV (queue, chấm Yes/No, per Đ-7.2)
- Server: xác nhận NSHV còn / đã tiêu, tính số học

### Related states
- Vòng Về đích, trước khi câu được đọc hoặc hiện màn hình (mốc admin bấm "hiển thị câu", `game-rules-review-old.md` GRR-048)
- Trạng thái NSHV: **chưa dùng** / **đã dùng**
- Hàng đợi tín hiệu: NSHV vào queue, admin duyệt (Đ-7.2 VCNV chặn, nhưng NSHV không trong VCNV)

### Trigger
Thí sinh bấm nút NSHV trước mốc đóng cửa sổ.

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
1. **Chỉ 1 lần/TS/trận**: sau khi bấm lần 1 được admin chấp nhận (Yes) → nút NSHV disabled
2. **Mốc đóng**: admin bấm "hiển thị câu" (`game-rules-review-old.md` GRR-048 — mốc "đọc lên hoặc hiện lên")
3. **Bấm trước mốc**: là hợp lệ; bấm sau → không hợp lệ (queue reject ngoài cửa sổ)
4. **Queue**: Về đích là vòng hàng đợi **KHÔNG chặn** (`Đ-7.2`) ⇒ tín hiệu đặt Ngôi sao hy vọng **có hiệu lực ngay**, không chờ admin xác nhận.

### Decision table

| Case | Conditions | Expected outcome | State change | Error |
|---|---|---|---|---|
| C1 | Câu 20đ, TS bấm NSHV @ 100ms (trước admin bấm hiển thị), admin chấm Đúng | **+20 × 2 = +40** | `score[seat] += 40` · `nshvUsed[seat] = true` | — |
| C2 | Câu 30đ, TS bấm NSHV @ 100ms, admin chấm Sai | **−30** (hình phạt đầy đủ, không cộng dồn lên cướp transfer) | `score[seat] −= 30` · `nshvUsed[seat] = true` | — |
| C3 | Câu 20đ, TS bấm NSHV lần 1 @ 100ms (admin chấp nhận Yes), rồi bấm lần 2 @ 200ms | Lần 2 **không được chấp nhận** — nút disabled | Lần 2 bị **reject** (hoặc UI không cho bấm) | — |
| C4 | Câu 30đ, TS bấm NSHV @ 100ms (trước mốc đóng cửa sổ), admin chấm "Đúng" + có người cướp đúng | **TS +60 (30×2)** · **người cướp +30 (transfer từ sai TS[i], không phải từ TS ấy)** | `score[seat_nshv] += 60` · `score[seat_steal] += 30` | — |
| C5 | **NEEDS CLARIFICATION** — Câu 30đ, TS NSHV sai (−30) + TS[i] sai → có người cướp đúng | TS NSHV −30 · người thi chính TS[i] −30 (transfer) · người cướp +30? Hay +0? (U-8) | — | Số học không chốt |
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
- **NSHV × cướp transfer Đúng**: số học khi cả hai áp dụng chưa chốt (U-8)

### Evaluation order
1. TS nhìn thấy câu sắp được hỏi (chuẩn bị trước)
2. TS bấm NSHV nút
3. Tín hiệu vào hàng đợi **không chặn** của vòng Về đích (`Đ-7.2`)
4. Admin duyệt (Yes = chấp nhận, No = từ chối) — hoặc **auto** nếu không trong queue
5. Nút disable nếu admin Yes
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
- Câu 20đ, TS bấm NSHV, admin Yes, TS trả lời Đúng → +40.
- Câu 30đ, TS bấm NSHV, admin Yes, TS trả lời Sai → −30 (kể cả có người cướp Đúng).
- TS bấm NSHV lần 1 (admin Yes), lần 2 (nút disable) → lần 2 không được bấm.

**Không hợp lệ:**
- TS thao tác đặt ngôi sao sau khi admin đã bấm "hiển thị câu" → nút đã tắt, không có tín hiệu nào được tạo; ngôi sao vẫn chưa dùng.
- TS NSHV sai + có người cướp Đúng → số học chưa chốt (U-8).

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
- Match state: `FINISHED` (không hoà) | `TIE_BREAK` (có hoà)
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
| C1 — Hoà ở vị trí NHẤT (default) | 2+ TS cùng điểm cao nhất, vị trí 1 ∈ `tieBreakPositions` | Chuyển `TIE_BREAK` | Match state: `FINISHED` → `TIE_BREAK` · Gán `tiedSeats` = [2+ ghế hoà] | — |
| C2 — Hoà ở vị trí khác | 2+ TS cùng điểm, vị trí ≠ 1 và ∈ config | Chuyển `TIE_BREAK` | Cùng C1 | — |
| C3 — Không hoà | 1 TS điểm cao nhất | Kết thúc trận | Match state: `FINISHED` · Admin xác nhận hoặc di chuyển (nếu có nút "Xem kết quả") | — |
| C4 — Hoà ngoài phạm vi | 2+ TS cùng điểm, vị trí ∉ `tieBreakPositions` | Kết thúc trận, đồng hạng | Match state: `FINISHED` · Ghi nhận cả nhóm cùng hạng trong biên bản | — |

### Outcomes
- **Kích hoạt tie-break**: Chuyển trạng thái trận sang `TIE_BREAK`, gọi admin chọn nhóm người tham gia (hoặc auto-select theo recommendation)
- **Không kích hoạt**: Trận chuyển `FINISHED`, đơn theo điểm (thắng ở vị trí NHẤT đã xác định)
- **Đồng hạng lưu biên bản**: Ghi rõ vị trí hoà và các thí sinh cùng hạng

### State changes
- Match: `FINISHED` ← hoàn thành vòng Về đích nhưng chưa phân định
- Match: `TIE_BREAK` ← kích hoạt tie-break
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
Mỗi khi hoàn thành vòng Về đích, server **tính toán lại** điều kiện hoà; kết quả quyết định **trạng thái chuyên biệt** (TIE_BREAK hoặc FINISHED), không phụ thuộc vào bấm lặp.

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

NEEDS CLARIFICATION

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
- Admin bấm gỡ lệnh cấm: 1 lần gỡ = gỡ được (không re-gỡ); nếu bấm gỡ lần 2 = no-op (NEEDS CLARIFICATION — không có rule ghi idempotency của thao tác gỡ cấm)

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

NEEDS CLARIFICATION

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
| C3 — Admin bấm "bốc lại" | Admin muốn random lại (mạng lag, server hỏng) | **Bốc thăm lần 2** (random mới) | Event lịch sử ghi **cả lần 1 và lần 2** (append-only) · Chỉ lần cuối cùng = người NHẤT | NEEDS CLARIFICATION (`game-rules-review.md` GRR-157): kết quả bốc lần 2 có vào event không, hay coi lần 1 vô hiệu |
| C4 — Pool câu phụ cạn | Không đủ 3 câu cho vòng Câu hỏi phụ | **KHÔNG BẮT ĐẦU ĐƯỢC vòng Câu hỏi phụ** — pre-flight chặn tại cửa vào vòng. Không tồn tại tình huống cạn giữa vòng | Vòng không mở; các vòng khác vẫn bắt đầu được | — |

### Outcomes
- **Bốc thăm**: Server / admin xác nhận 1 người từ nhóm = người thắng vị trí NHẤT
- **Kết quả chốt**: Kết quả bốc thăm là **tối sau cùng**, không được undo (event append-only, Đ-5.3)
- **Lịch sử bất biến**: Nếu bốc lại, cả lần 1 và lần 2 được ghi trong event log (NEEDS CLARIFICATION nếu là cái nào có hiệu lực)

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
- Bấm bốc lại khi đã confirm (`game-rules-review.md` GRR-160): NEEDS CLARIFICATION — làm lại random hay no-op

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
- Mỗi lần bấm "Bốc thăm" sinh 1 random event mới (NEEDS CLARIFICATION — không rõ nó có idempotency không)
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

NEEDS CLARIFICATION

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
- **Không rõ câu miệng vs câu gõ** (NEEDS CLARIFICATION — U-7): ở official cả hai đều admin chấm; ở practice, máy chỉ so khớp text
- **Practice gặp câu MIỆNG** (NEEDS CLARIFICATION — U-38): autoJudge không chấm được câu miệng; có bị skip hay admin chấm manually?
- **Câu chưa chấm không thể bị bỏ lại**: phán quyết là **điều kiện để chuyển sang câu tiếp theo**, nên không tồn tại trạng thái "vòng đã đóng mà còn câu chưa chấm" ở các vòng hỏi tuần tự. Riêng vòng xếp hạng (Tăng tốc), việc chuyển câu khi **mới chấm một phần** số thí sinh vẫn chưa được quy định — xem `game-rules-review.md` GRR-162.

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
| C3 — Khác 1 ký tự, bỏ dấu ON | Submission "Hue" vs ans "Huế", stripDiacritics ON | **Highlight = không khác** (dấu bỏ qua) | UI: "Khớp nếu bỏ dấu" (gợi ý tuỳ chọn) | CONFLICT (K-6): F26 yêu cầu "dấu sai → không công nhận", nhưng bỏ dấu ON → coi như đúng |
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
- **stripDiacritics = ON mâu thuẫn với F26** (K-6): Không phải lỗi logic, mà là **mâu thuẫn luật** (chưa phân xử)
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
- `docs/reviews/game-rules-review-old.md` GRR-027 (nếu có — hoặc K-6 mâu thuẫn chính tả)
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

NEEDS CLARIFICATION

### Purpose
Định nghĩa cơ chế điều chỉnh điểm (SCORE_ADJUST) do admin quyết định, độc lập với việc revert event chấm điểm, không tự revert khi bỏ vòng.

### Actors
- Admin (phát sinh điều chỉnh)
- Server (ghi event, tính điểm lại)
- Audit log (ghi reason bắt buộc)

### Related states
- MatchEvent: event SCORE_ADJUST là loại độc lập
- Match state: mọi lúc (không bị ràng buộc ở vòng nào)

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
| C3 — Chỉnh delta=0 | Admin nhập delta=0, reason="..." | **No-op**: không sinh event (delta=0) hoặc **sinh event để ghi lịch sử** (phụ thuộc design) | NEEDS CLARIFICATION — không rõ delta=0 có event không | — |
| C4 — Chỉnh ngay sau bỏ vòng | Admin chỉnh +3 cho A sau khi bỏ VCNV | **Chỉnh vẫn có hiệu lực** — không tự revert theo bỏ vòng (Đ-11.B) | MatchEvent: vòng bị revert + event chỉnh riêng · Điểm được tính lại từ event log | — |
| C5 — Bỏ vòng **sau** chỉnh | Admin chỉnh +5 cho A, sau đó bỏ vòng Khởi động | **Chỉnh vẫn giữ**: bỏ vòng revert sự kiện Khởi động nhưng **không revert SCORE_ADJUST** (Đ-11.B) | MatchEvent: revert KĐ events, SCORE_ADJUST vẫn ở | NEEDS CLARIFICATION (`game-rules-review.md` GRR-159): chỉnh "mồ côi" khi bỏ vòng sau đó |

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
Nếu admin bấm chỉnh lần 2 (lag), **lần 2 = event mới** (NEEDS CLARIFICATION — không rõ có dedup không). Khác với JUDGE (dedup per Đ-3.3).

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

NEEDS CLARIFICATION

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
- Hành động: "bỏ" hoặc "chạy lại"

### Conditions
- **Vòng phải tồn tại** trong playlist
- **Danh sách event của vòng** phải được xác định rõ (theo vòng)
- **Nếu chạy lại**: pool đề phải còn đủ câu (pre-flight check)
- **Nếu bỏ hẳn**: điểm revert, biên bản vẫn giữ với nhãn "đã bỏ" (Đ-5.2d)

### Decision table

| Case | Conditions | Expected outcome | State change | Error |
|---|---|---|---|---|
| C1 — Bỏ vòng Khởi động | Admin bấm "Bỏ vòng KĐ" → dialog Yes | Điểm KĐ revert (REVERT event cho tất cả event KĐ) | MatchEvent: +N event đảo ngược · Điểm recalc · Biên bản: vòng KĐ = "đã bỏ" | — |
| C2 — Chạy lại vòng VCNV | Admin bấm "Chạy lại VCNV" → dialog Yes | Pool check OK → reset queue, mở lại VCNV, admin chọn lại từ đầu | MatchEvent: REVERT VCNV events + reset queue · Playlist: vòng VCNV "đã chạy lại" | Nếu pool không đủ → pre-flight chặn |
| C3 — Bỏ vòng nhưng SCORE_ADJUST ở vòng đó | Admin bỏ KĐ (KĐ events revert), nhưng có +5 SCORE_ADJUST gắn KĐ | **SCORE_ADJUST KHÔNG tự revert** (Đ-11.B) | MatchEvent: revert KĐ events, SCORE_ADJUST vẫn ở | Ví dụ `game-rules-review.md` GRR-159 |
| C4 — Bỏ vòng B khi vòng A chưa xong (`game-rules-review.md` GRR-164) | Admin mở vòng B khi vòng A còn tín hiệu chờ duyệt | **Advisory**: cảnh báo "vòng A chưa kết thúc", admin bấm Yes/No ⇒ vẫn thực hiện | Match state: A → B (admin quyết định thứ tự) | Không hard-block (advisory mode, Đ-1.2) |
| C5 — Bỏ vòng hai lần (lag) | Admin bấm "Bỏ VCNV" lần 1, bấm lại lần 2 | **Lần 2 là lặp lại revert?** NEEDS CLARIFICATION (`game-rules-review.md` GRR-160) | Nếu no-op: điểm không đổi · Nếu revert lần 2: điểm tụt gấp đôi | NEEDS CLARIFICATION — Đ-3.3 chỉ dedup JUDGE, không cover REVERT |

### Outcomes
- **Bỏ vòng**: Điểm bị revert (event đảo ngược), biên bản nhãn "đã bỏ", câu đã dùng KHÔNG trả lại (Đ-5.2f)
- **Chạy lại**: Vòng reset, queue reset (Đ-7.b), mở lại từ đầu với câu mới (từ pool)
- **Lịch sử**: Vòng bị bỏ vẫn ghi trong biên bản (dấu vết)
- **Viewer/overlay**: Số điểm thay đổi đột ngột, không hiệu ứng (Đ-5.2e)

### State changes
- MatchEvent: +N event đảo ngược (vòng bị bỏ)
- Điểm tích luỹ: recalculate từ reduce(event còn lại)
- Queue: reset (nếu chạy lại) hoặc giữ nguyên (nếu chỉ bỏ)
- Biên bản vòng: thêm nhãn "đã bỏ" hoặc "đã chạy lại"
- Pool đề: `usedInContest` cờ **KHÔNG reset** (Đ-5.2f)

### No-change guarantees
- **Câu đã dùng không trả lại pool** (Đ-5.2f): mỗi câu rút = 1 lần, không hoàn lại dù vòng bỏ
- **Event cũ không xoá**: REVERT event thêm vào (append-only), không xoá event gốc
- **Audit trail bất biến**: cả bỏ lẫn chạy lại đều ghi
- **Nếu bỏ vòng X rồi chạy lại Y**: X giữ nhãn "đã bỏ", Y được track riêng

### Error outcomes
- **Pool cạn**: chạy lại vòng cũng phải qua **cửa vào vòng** — thiếu câu thì không chạy lại được vòng đó
- **Vòng chưa kết thúc**: `game-rules-review.md` GRR-164 cảnh báo (không hard-block)
- **Bỏ vòng bấm 2 lần** (`game-rules-review.md` GRR-160): NEEDS CLARIFICATION — có no-op hay revert lại

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
Bấm "Bỏ vòng" lần 2 (lag): NEEDS CLARIFICATION (`game-rules-review.md` GRR-160) — nó có dedup không, hay sinh revert event thứ 2 (điểm tụt gấp đôi)

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

NEEDS CLARIFICATION

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

### Outcomes
- **Câu rút được ghi dấu** `usedInContest = true` (mềm, không phải xoá)
- **Event `QUESTIONS_DRAWN`** ghi rõ qid + timestamp (để replay)
- **Pool effective giảm** (loại câu `usedInContest`)
- **Câu skip cũng tiêu pool** (Đ-5.2f): nếu bỏ qua lượt → câu đã hiển thị set cờ `usedInContest` (NEEDS CLARIFICATION — chưa rõ "đã dùng" = đã hiện hay đã chấm)

### State changes
- MatchEvent: +1 `QUESTIONS_DRAWN` event (per turn rút)
- Question.usedInContest: += (bộ câu rút lần này)
- Match config: snapshot câu gán
- Pool: effective giảm (câu bị loại không cần xoá DB)

### No-change guarantees
- **Cờ `usedInContest` KHÔNG reset** khi bỏ vòng / chạy lại (Đ-5.2f): mỗi câu rút = 1 lần, vĩnh viễn tiêu khỏi pool
- **Event `QUESTIONS_DRAWN` không thay đổi** (append-only): dùng để replay trận
- **Danh sách gán** (snapshot) **KHÔNG bị sửa** giữa trận

### Error outcomes
- **Pool cạn trước start** (pre-flight chặn): Người tạo contest chưa gán đủ câu hoặc contest dùng pool chia sẻ quá tải
- **Pool cạn giữa vòng**: không tồn tại — pre-flight chạy tại cửa vào **từng vòng**
- **Câu skip**: NEEDS CLARIFICATION (U-30 — "đã dùng" = hiện hay chấm)

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
- Mỗi call "rút câu" (draw) → **sinh 1 `QUESTIONS_DRAWN` event**; nếu admin bấm draw lại (lag), lần 2 = **event riêng** (NEEDS CLARIFICATION — không rõ có dedup không)
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
- `docs/game-rules-inventory.md` §R-KD-07 (rút đề, emit `QUESTIONS_DRAWN`) · §R-GEN-06 (no-repeat toàn contest)
- `docs/game-rules-decisions.md` §6.3 (Đ-5.2f — câu đã dùng không trả lại)
- `plans/260711-2340-olympia-contest-system/DEFERED.md` D22 (draw, no-repeat, snapshot danh sách)

## GR-032 — Hàng đợi tín hiệu và xác nhận của admin

### Status

NEEDS CLARIFICATION

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

**Hai loại tín hiệu trong queue chặn (VCNV)**: Chọn hàng ngang và "Mở chướng ngại vật" cùng queue (`game-rules-review.md` GRR-144 — NEEDS CLARIFICATION):
- FIFO hay ưu tiên loại nào? Chưa định nghĩa

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
- Timer: `running` / `paused` / `stopped`
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

NEEDS CLARIFICATION

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
- Lệch clock quá lớn (portable Windows LAN): NEEDS CLARIFICATION (U-14) — chưa có quy tắc xử lý
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

NEEDS CLARIFICATION

### Purpose
Thí sinh bị mất kết nối có thời gian grace **120 giây** để kết nối lại và giữ ghế. Quá thời gian grace, ghế bị xử lý theo `dropoutPolicy` (mất quyền, bị loại từ vòng, hoặc xoá khỏi trận tùy vòng). Trong khoảng grace, trạng thái của ghế được giữ nguyên (state-sync khi quay lại).

### Actors
- **Thí sinh** — mất kết nối, kết nối lại
- **Server** — theo dõi kết nối, quản lý grace period
- **Admin** — có thể can thiệp trước khi hết grace (hoặc tự động xử lý)

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
- `dropoutPolicy`: config của trận quy định xử lý dropout (chưa liệt kê đầy đủ — U-13)

### Conditions
**Mất kết nối**:
- Connection socket ngắt quá 1000ms (timeout)

**Trong grace (< 120s)**:
- Thí sinh kết nối lại trước hết 120 giây
- Ghế được restore state

**Quá grace (>= 120s)**:
- Ghế bị xử lý theo `dropoutPolicy`
- Ghế có thể bị loại khỏi vòng, mất quyền, hoặc xoá từ trận

### Decision table

| Case | Conditions | Expected outcome | State change | Error |
|---|---|---|---|---|
| C1 — Mất kết nối; kết nối lại < 120s | Thí sinh quay lại trong grace | Ghế restore state; banner "đã kết nối lại" | Seat state: quay về cũ | — |
| C2 — Quá 120s grace period | Admin không can thiệp; thời gian quá 120s | Xử lý theo `dropoutPolicy` | Seat state: `dropped_out` hoặc loại khỏi vòng | — |
| C3 — Rớt mạng đúng lượt riêng Khởi động | Disconnect xảy ra vòng Khởi động riêng | **Admin quyết** — không có trạng thái tạm dừng của hệ thống; admin ngừng thao tác và xử lý ngoài hệ thống | Không đổi | — |
| C4 — Kết nối lại; trận đã chuyển vòng | Thí sinh kết nối lại ở vòng tiếp theo | Sync state toàn bộ vòng; restore tất cả UI | UI: full sync | — |
| C5 — Admin can thiệp trước hết grace | Admin kick thí sinh hoặc cho phép giữ lâu hơn | **NEEDS CLARIFICATION**: Quyền admin mở rộng grace? | Tuỳ quyết định | Không định nghĩa admin override |
| C6 — Hai thí sinh cùng mất kết nối | Cả A, B disconnect trong grace | Cả hai đều có 120s để quay lại độc lập | Seat A, B: both in grace | — |
| C7 — Mất kết nối ở cuối câu hỏi phụ | Disconnect t=0; hết grace t=120; trận đã xong | Quá grace nhưng trận xong; ghế xoá hay giữ? | **NEEDS CLARIFICATION**: retention policy | `game-rules-review-old.md` GRR-129 chưa định rõ |
| C8 — Grace deadline vs timeout cùng ms | t=119.999s: submission tới, grace hết ở t=120 | Submission được tính vì tới trong grace | Submission: `accepted` | — |

### Outcomes
- **Trong grace**: ghế giữ vị trí; state sync; banner thông báo reconnecting
- **Quá grace**: xử lý theo `dropoutPolicy` (mất quyền vòng hiện tại, loại khỏi trận, hoặc giữ điểm cũ không cộng thêm)
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
- Lệch clock portable Windows LAN: NEEDS CLARIFICATION (U-14) — không có quy tắc xử lý grace trên thiết bị không NTP
- `dropoutPolicy` không định nghĩa: U-13 — NEEDS CLARIFICATION (danh sách đầy đủ giá trị)
- Grace quá hạn nhưng admin chưa định nghĩa hành động: NEEDS CLARIFICATION (auto-drop hay chờ admin)

### Evaluation order
1. Server phát hiện disconnect (socket timeout)
2. Bắt đầu grace 120s countdown
3. Nếu kết nối lại trước 120s: state-sync, restore
4. Nếu quá 120s: áp `dropoutPolicy`

### Boundaries
- **Grace 120 giây**: cứng; không thay đổi
- **Deadline xác định**: disconnect_time + 120000ms
- **Boundary t=120.000s**: **vẫn trong grace** — biên đóng (`Đ-28`).

### Idempotency
**Reconnect lần thứ hai**: Nếu kết nối lại, mất lại, kết nối lại (trong 120s từ disconnect đầu):
- Được phục hồi lần 2? Hay 120s tính từ disconnect lần cuối?
- NEEDS CLARIFICATION

### Concurrency
**Disconnect ở biên grace**: submission tới ở 119.999s, grace hết ở 120.000s ⇒ **trong grace, được nhận** (biên đóng, `Đ-28`). Câu còn mở là **số phận của các submission đã nhận sau khi ghế bị xử dropout** (`game-rules-review.md` GRR-169):
- Submission được tính hay bỏ?
- Ghế vẫn ở trận hay đã dropped?

### Examples

**Ví dụ 1 — Happy path (quay lại trong grace)**:
- Thí sinh A kết nối vào lúc t=0
- Mất kết nối ở t=10s
- Kết nối lại ở t=60s (còn 60s grace)
- Ghế A restore; tiếp tục thi

**Ví dụ 2 — Quá grace (tự động dropout)**:
- Thí sinh B mất kết nối t=0
- t=120s: quá grace → xử lý theo `dropoutPolicy`
- Nếu policy = `remove_from_round` → B loại khỏi vòn hiện tại
- B không thể quay lại vòng này

**Ví dụ 3 — Rớt vòng riêng Khởi động**:
- Vòng Khởi động riêng; thí sinh C mất kết nối
- Engine tự pause (D13.4); admin thấy cảnh báo
- Admin quyết: resume (C giữ lượt) hay dropout (C mất lượt)

**Ví dụ 4 — State-sync (quay lại vòng tiếp theo)**:
- Thí sinh D quay lại lúc t=50s (trong grace)
- Nhưng trận đã chuyển từ Khởi động sang VCNV
- Server sync trạng thái VCNV cho D; D tiếp tục từ VCNV

**Ví dụ 5 — Boundary grace 120s**:
- Disconnect ở t=0
- t=119.999s: submission tới → tính được
- t=120.000s: **vẫn trong grace** (biên đóng, `Đ-28`); quá 120.000s mới hết.

### Source traceability
- `game-rules-inventory.md` §R-GEN-09 — Reconnect grace 120 giây
- `game-rules-decisions.md` D13.4 — Auto-pause khi thí sinh rớt mạng lượt riêng
- `CLAUDE.md` — State-sync khi quay lại

---

## GR-037 — Phạm vi hiển thị đáp án

### Status

CONFLICT

### Purpose
Đáp án của các câu hỏi chỉ được phép hiển thị cho những kênh cụ thể tùy theo cấu hình `revealAnswerAfterJudge`. Mục đích: bảo vệ bí mật đề; admin + MC luôn được xem; viewer chỉ xem sau khi chấm xong (nếu bật).

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
| C9 — CONFLICT: PRD vs SPEC | `PRD` NFR-4: "contest bật" · `SPEC` + `CLAUDE.md`: "per-MATCH" → | **NEEDS CLARIFICATION — K-4**: per-match hay per-contest? | Config scope chưa chốt | K-4: CONFLICT |

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

**Ví dụ 5 — CONFLICT (K-4) — chưa quyết định**:
- `PRD` NFR-4: *"contest bật revealAnswerAfterJudge"* → per-contest
- `SPEC` §11, §13: *"per-MATCH"* → per-match (mỗi trận khác nhau)
- **NEEDS CLARIFICATION**: Dùng cái nào? K-4 chưa chốt

### Source traceability
- **CONFLICT — K-4**: `PRD` NFR-4 vs `SPEC` §11/§13 + `CLAUDE.md`
- `game-rules-inventory.md` §R-GEN-10 — Phạm vi đáp án
- `CLAUDE.md` §Zero-trust security — Đáp án chỉ admin + MC
- `CLAUDE.md` §Mô hình truy cập — Viewer public, read-only
- `game-rules-inventory.md` §R-GEN-03, §TERM-019 — `matchPurpose` + `revealAnswerAfterJudge`

