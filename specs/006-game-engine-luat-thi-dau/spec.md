# Feature Specification: Game engine và luật thi đấu

**Feature Branch**: `006-game-engine-luat-thi-dau`

**Created**: 2026-07-30

**Status**: Draft

**Input**: EPIC-006 — Game engine và luật thi đấu (`docs/PRD.md` §11 dòng 456-467, §12)

**Nguồn**: `docs/PRD.md` (EPIC-006 §11; PRD-REQ-036→050, PRD-REQ-088→093 §12; Core Game Loop §8.1, §8.2; Game States §9; JOURNEY-005 §10; ma trận truy nguyên §18) · `docs/game-rules.md` (`GR-001`→`GR-037` toàn bộ, 23 nguyên tắc nền, 4 bảng dùng chung) · `docs/game-state-machine.md` (`STATE-017`→`STATE-021` cấp câu, `STATE-022`→`STATE-028` cấp ghế, `STATE-029`→`STATE-032` cấp tín hiệu, `STATE-041`→`STATE-043` ô chữ, `EVENT-009`→`EVENT-015`, `EVENT-037`→`EVENT-048`, `INV-001`→`INV-022`) · `docs/glossary.md` (`TERM-020`, `TERM-022`, `TERM-023`, `TERM-025`→`TERM-029`, `TERM-037`, `TERM-043`, `TERM-045`→`TERM-048`, `TERM-053`, `TERM-057`, `TERM-058`) · `docs/traceability.md`. `docs/reviews/**` và `plans/**` **không phải nguồn**.

**Lưu ý về đầu vào**: lệnh gọi yêu cầu đọc `docs/rule-traceability.md` và `docs/reviews/prd-review.md` — **cả hai đường dẫn không tồn tại** trong repo. Tương ứng gần nhất là `docs/traceability.md` (đã dùng); không có tài liệu review nào thay thế `docs/reviews/prd-review.md`, và theo `.specify/memory/constitution.md` thì `docs/reviews/**` **không thoả cổng truy nguyên** nên dù có cũng không dùng làm nguồn requirement. Đây là chênh **đường dẫn đầu vào**, không phải `CONFLICT` nghiệp vụ, nên không ghi vào §Open Questions.

**Quy ước truy nguyên của feature này**: EPIC-006 khai *"Related game rules: `GR-001` → `GR-037` (toàn bộ)"* và *"Scope: Năm vòng và **luật từng vòng**"*. Luật số học của từng vòng (`GR-001`→`GR-025`) vì thế **thuộc phạm vi EPIC-006 qua chính khai báo epic**, dù PRD không sinh một `PRD-REQ-NNN` riêng cho mỗi con số. Ở những FR đó, cột PRD trỏ tới **neo PRD gần nhất còn hiệu lực** — `PRD §8.2` (Core Game Loop cấp câu, bảng *Số câu theo luật*), `PRD §9.2` (bảng trạng thái cấp trận, khai đủ mức điểm và thời gian từng vòng) và mục **EPIC-006 §11** — thay vì bịa một mã `PRD-REQ` không tồn tại. Đây **không phải** `NEEDS CLARIFICATION`: hành vi có nguồn chuẩn tắc đầy đủ ở `docs/game-rules.md`, chỉ là PRD chọn tham chiếu thay vì sao chép.

## Phạm vi

EPIC-006 phủ **engine thi hành luật bên trong một trận đang chạy**: năm vòng thi và luật số học của từng vòng (Khởi động lượt riêng và lượt chung, Vượt chướng ngại vật, Tăng tốc, Về đích, Câu hỏi phụ), mô hình điểm dựa trên nhật ký sự kiện append-only và cơ chế hoàn nguyên bằng sự kiện đảo ngược, hàng đợi tín hiệu cùng quy tắc chặn/không chặn, server time là nguồn sự thật duy nhất, các mốc thời gian ánh xạ từ hành vi MC sang cú bấm của admin, cơ chế rút đề và quy tắc không lặp câu trong toàn contest, phạm vi hiển thị đáp án theo mốc **câu khép**, và xử lý mất kết nối kèm giữ ghế.

Actor chính: **ACTOR-007 server** *(thi hành)* và **ACTOR-001 admin** *(phán quyết)*. Journey chính: **JOURNEY-005 — Thi đấu**.

**Không thuộc phạm vi feature này** (xem §Out of Scope): vòng đời cấp trận và bốn cửa ra của một vòng (EPIC-005); giao diện điều khiển của admin, màn chấm, điều chỉnh điểm thủ công, ba hạng cảnh báo (EPIC-007); giao diện và trải nghiệm máy thí sinh (EPIC-008); màn khán giả, lớp phủ, màn MC (EPIC-009); xác thực và phân quyền (EPIC-001); kho đề và bộ đề (EPIC-002); contest builder (EPIC-004).

## Clarifications

### Session 2026-07-30

- Q: `docs/rule-traceability.md` và `docs/reviews/prd-review.md` trong lệnh gọi không tồn tại — dừng lại hay tiếp tục? → A: **Tiếp tục** với `docs/traceability.md` và ghi chú chênh đường dẫn ở đầu tài liệu (cùng cách xử lý đã dùng ở `specs/005-phong-thi-vong-doi-tran`). `docs/reviews/**` không thoả cổng truy nguyên nên không có mất mát nguồn.
- Q: Luật số học từng vòng (`GR-001`→`GR-025`) không có `PRD-REQ-NNN` riêng — có ghi `NEEDS CLARIFICATION` không? → A: **Không**. EPIC-006 khai `GR-001`→`GR-037` toàn bộ và *"luật từng vòng"* trong Scope; `PRD §8.2` và `PRD §9.2` khai đủ số câu, thời gian và mức điểm. Truy nguyên đi qua **neo epic + neo §PRD**, ghi rõ ở §Quy ước truy nguyên.
- Q: `GR-022` (điều kiện kích hoạt Câu hỏi phụ) đã thuộc `specs/005-phong-thi-vong-doi-tran` FR-018→FR-020 — spec này có đặc tả lại không? → A: **Không đặc tả lại**. Spec 006 chỉ phủ **thể thức bên trong vòng** `TIE_BREAK` (`GR-023`, `GR-024`, `GR-025`) và **nguồn đề** của vòng (`PRD-REQ-090`); cú bấm Chốt trận, phép tìm nhóm hoà và hai lần chốt vẫn thuộc spec 005. `GR-022` chỉ xuất hiện ở đây như **tiền đề** của US-012.
- Q: `PRD-REQ-091` (đặt chỗ câu Câu hỏi phụ tại LOBBY) khai `EPIC-005, EPIC-006` — spec nào sở hữu? → A: **Spec 005 sở hữu** (đã có FR-030→FR-033 ở đó). Spec 006 chỉ đặc tả **hệ quả phía engine**: câu đã đặt chỗ bị loại khỏi phép rút của các vòng mở sau đó, và cảnh báo vỡ bộ VCNV (`PRD-REQ-093`, thuộc EPIC-006).
- Q (giải `OQ-001`): Câu **ô trung tâm** VCNV có thời lượng suy nghĩ bao nhiêu và lấy từ đâu? → A: **Dùng CÙNG một giá trị với câu hàng ngang** — không phải metadata riêng từng câu, và người soạn đề **không** khai thời lượng riêng cho câu ô trung tâm. **Nhưng con số `15` giây KHÔNG cố định**: nó là **giá trị cấu hình luật cấp vòng VCNV**, preset `O26_DEFAULT@1` đặt mặc định `15`. Cùng giá trị cấu hình đó chi phối **ba** cửa sổ của vòng: thời gian suy nghĩ câu hàng ngang · thời gian suy nghĩ câu ô trung tâm · cửa sổ giải Chướng ngại vật sau gợi ý cuối. Đổi giá trị ở contest builder đổi **cả ba** cùng lúc. Điều này khớp `CLAUDE.md` §Quy ước khác *("Mọi timer/điểm là RuleConfig — KHÔNG hard-code luật")*; ngoại lệ *cố định, không cấu hình* vẫn **chỉ** là 3 câu × 15 giây của Câu hỏi phụ (`GR-023` §Biên) và hệ số Ngôi sao hy vọng (`GR-021` §Biên).
- Q (giải `OQ-003`): Trận `practice` chạy trong contest **thật** có hoà ở vị trí Nhất — vòng Câu hỏi phụ rút đề từ tập nào, khi `GR-031` C9 đảo chiều phép lọc còn `GR-023` §Nguồn đề dùng phép lọc thuận? → A: **Phép đảo của `GR-031` C9 THẮNG.** Thứ tự áp dụng là **hai bước, cố định**: **(1)** dựng kho khả dụng theo phép đảo — **chỉ** câu đã lộ ở các trận thật trước đó của contest; **(2)** trên kho đó mới áp phép rút của `GR-023` — ba kho nguồn theo thứ tự ưu tiên Về đích → Khởi động → VCNV, loại kho Tăng tốc, loại câu thực hành. Cửa vào vòng đếm *"đủ 3 câu khả dụng"* trên **tập đã lộ**. Mọi vòng của một trận practice vì thế đọc **cùng một** kho đảo chiều — không có vòng nào là ngoại lệ. Tích hợp vào **FR-105a**.
- Q: Ở Tăng tốc và câu hàng ngang VCNV, dấu Đúng/Sai đã đặt cho một ghế có sửa được **trước** khi bấm *chốt câu* không? → A: **Có — sửa thoải mái tới mốc chốt câu.** Dấu từng ghế là **lựa chọn tạm**, không sinh sự kiện điểm; cú bấm *chốt câu* mới là phán quyết thật và là chỗ duy nhất sinh sự kiện. Từ mốc đó không còn đường đổi phán quyết nào. Tích hợp vào **FR-009a**, và **FR-003** đã được sửa để không còn khoá dấu ngay ở hai vòng này.
- Q: Admin kích hoạt tay tín hiệu **kế tiếp** hay tín hiệu **bất kỳ** trong hàng đợi? → A: **Bất kỳ tín hiệu còn hiệu lực với đích của nó.** Hệ thống **khuyến nghị** tín hiệu sớm nhất chưa xử lý nhưng MUST NOT chặn cứng lựa chọn khác — `INV-014` chỉ có ba chỗ chặn cứng và đây không phải một trong ba; mẫu áp dụng là `GR-016` C5 / `INV-020` *(máy khuyến nghị, admin quyết)*. Ràng buộc *"đúng thứ tự server timestamp"* mà bản trước của **FR-034a** đặt ra là một **chặn cứng tự thêm** và đã bị gỡ. `INV-007` **không** bị đụng: nó chi phối **thứ tự xử lý bình thường** của hàng đợi, còn đây là **can thiệp ngoại lệ của người**, hiện rõ và có ghi nhật ký; lệnh cấm của `GR-032` §Đồng thời nhắm vào **quy tắc máy ẩn**, không nhắm vào quyết định của admin. Tập ứng viên giới hạn ở tín hiệu còn hiệu lực với **đích** — cùng câu ở ba vòng chuông, cùng vòng ở VCNV.
- Q: Khi admin kích hoạt một tín hiệu **lệch** khuyến nghị, hệ thống phản hồi ở hạng nào trong ba hạng của `PRD §9.4`? → A: **Hạng 2 — dialog cảnh báo lệch luật, Yes/No, KHÔNG bắt nhập lý do.** Cùng khuôn với `GR-016` C5 *(chọn người thi Về đích khác khuyến nghị)*; **không** nâng lên hạng phá huỷ như bỏ vòng hay huỷ trận. Dialog MUST nêu rõ ghế bị vượt và thứ tự gốc. Tích hợp vào **FR-034e**.
- Q: Tập ứng viên hợp lệ của kích hoạt tay gồm những ai? → A: **Đúng hai nhóm** — ghế có tín hiệu **trơ chưa từng được xử lý**, và ghế **vừa bị chấm Huỷ kết quả** ở chính câu/vòng đó *(tức admin đưa lại được chính người vừa bị huỷ, cho họ lượt thứ hai)*. **Loại khỏi** tập ứng viên: ghế **bị loại** khỏi vòng VCNV — bị loại là **tuyệt đối** trong phạm vi vòng (`GR-010`, `INV-019`), kích hoạt tay **không** phải đường gỡ lệnh cấm đó; và ghế đang bị **vô hiệu hoá** — admin phải **kích hoạt lại ghế** trước đã (`GR-036` C5). Tích hợp vào **FR-034e**.
- Q: Cơ chế kích hoạt tay (**FR-034a**) có áp cho tín hiệu *"Mở chướng ngại vật"* ở VCNV không, và tín hiệu đó có lựa chọn **Huỷ kết quả** không? → A: **Có áp, và có thêm Huỷ kết quả.** Tín hiệu Chướng ngại vật MUST có **ba** lựa chọn *(Đúng / Sai / Huỷ kết quả)* thay vì hai như `GR-009` mô tả; *Huỷ kết quả* là đường khép một tín hiệu **đã xác nhận** mà **không loại** thí sinh, và khi dùng nó admin kích hoạt được tín hiệu Chướng ngại vật kế tiếp trong hàng đợi chặn. Tích hợp vào **FR-034c**. *Lưu ý phạm vi*: đây là **bổ sung ngoài bảng §2.2 và ngoài `GR-009`**, nên đi cùng `QĐ` của `OQ-006`.
- Q: Giới hạn *"đúng MỘT lần"* của kích hoạt tay ánh xạ sang đơn vị nào ở VCNV, khi tín hiệu Chướng ngại vật gắn với **vòng** chứ không phải một câu? → A: **Một lần cho mỗi phán quyết *Huỷ kết quả***, chuỗi chỉ dừng khi hàng đợi tín hiệu Chướng ngại vật của vòng đã cạn — **không** có trần tổng số lần trong một vòng. Đây là **khác biệt có chủ đích** so với ba vòng kia *(gắn với một câu ⇒ trần đúng một lần)*, và nó bám theo **đích** của tín hiệu. Tích hợp vào **FR-034d**.
- Q: Khi lượt cướp bị **Huỷ kết quả**, hàng đợi đã giữ sẵn thứ tự các cú bấm chuông — admin có **kích hoạt tay** được lượt của người bấm kế tiếp không, và ở những vòng nào? → A: **Có, và áp cho MỌI vòng có giành quyền bằng chuông** — Khởi động lượt chung, cướp quyền Về đích, Câu hỏi phụ. Trigger là phán quyết **Huỷ kết quả** cho người đang giữ quyền. *(Ràng buộc **"đúng theo thứ tự server timestamp"** đặt ra ở phiên này **đã bị gỡ** ở phiên làm rõ thứ hai — xem bullet *"tín hiệu kế tiếp hay tín hiệu bất kỳ"* bên dưới; bản có hiệu lực là **khuyến nghị + cảnh báo ép được**.)* Tích hợp vào **FR-034a**. *Ba tham số đi kèm* (chốt cùng phiên, tích hợp vào **FR-034b**): **đồng hồ** — cấp lại **trọn** cửa sổ suy nghĩ của vòng, tính từ mốc admin bấm kích hoạt; **số lần** — **đúng MỘT lần cho mỗi câu**, yêu cầu thứ hai bị từ chối; **hình phạt** — **y hệt** người giành quyền bình thường của vòng đó, không có bảng điểm riêng. *Hệ quả về mốc công bố*: câu khép lùi tới sau khi người được kích hoạt đã được chấm (**FR-055a**). *Hệ quả kéo theo*: `INV-009` phải đọc là *"một câu **KHÉP** một lần"*, không phải *"mỗi câu một cú bấm chấm"* — vì Về đích vốn đã chấm người thi chính rồi chấm người cướp, và `GR-008` C2 chấm từng thí sinh cho một câu hàng ngang; **FR-003** đã được sửa theo cách đọc này *(đơn vị chống trùng là bộ ba `(câu, thí sinh, loại phán quyết)`, khớp FR-015)*. **Cần một `QĐ` mới trong `docs/decisions.md`** để thoả cổng truy nguyên — xem `OQ-006`.
- Q (giải `OQ-002`): Ở Về đích, người cướp quyền bị chấm **Huỷ kết quả** — câu đã khép theo `GR-037` C6 nhưng loại phán quyết lại thuộc C8 *(không tự công bố)*. Server có tự đẩy đáp án tới thí sinh, khán giả và lớp phủ không? → A: **KHÔNG — `GR-037` C8 thắng `GR-037` C6.** Phán quyết **Huỷ kết quả** MUST NOT tự công bố đáp án ở **bất kỳ** vị trí nào của chuỗi Về đích, kể cả khi nó là phán quyết **khép câu** cho người cướp quyền. Đáp án của câu đó chỉ hiện khi **admin mở tay** (`QĐ-048`). Quy tắc phát biểu theo **loại phán quyết**, không theo vai bị chấm — không có nhánh riêng cho *người thi chính* so với *người cướp quyền*.

## User Scenarios & Testing *(mandatory)*

### US-001 — Phán quyết của admin là đường sinh điểm duy nhất (Priority: P1)

- **Title**: Máy hiển thị và tô nổi bật; người quyết Đúng/Sai
- **Actor**: ACTOR-001 admin *(quyết)*; ACTOR-007 server *(ghi)*
- **Intent**: Chấm một câu đã có bài làm — gõ máy, nói trên sân khấu, hoặc thao tác thực hành — và để hệ thống chốt điểm đúng theo phán quyết đó, không sớm hơn và không khác đi.
- **User value**: Kết quả trận luôn giải thích được bằng một quyết định của con người; máy không bao giờ trừ oan ai vì một phép so khớp chuỗi.
- **Priority**: P1
- **Priority rationale**: Đây là nguyên tắc nền số 1 (`QĐ-001`, `QĐ-010`) và là `NON-GOAL-001` bao trùm mọi non-goal khác — mọi rule tính điểm khác đều đọc kết quả của story này.
- **Independent test**: Ở một vòng bất kỳ, gửi một bài làm **khớp tuyệt đối** với đáp án chuẩn rồi không bấm gì; xác nhận điểm không đổi và không sự kiện điểm nào được sinh. Sau đó bấm Đúng và xác nhận điểm chỉ đổi tại đúng cú bấm đó.
- **Related PRD requirements**: PRD-REQ-036, PRD-REQ-037, PRD-REQ-038
- **Related game rules**: `GR-026`, `GR-027`, bảng *Phán quyết có hai hay ba lựa chọn*, bảng *Bản gửi quá hạn*, bảng *Kênh trả lời* · `INV-003`, `INV-009`, `INV-010`
- **Related journey**: JOURNEY-005

---

### US-002 — Điểm là hàm của nhật ký sự kiện, hoàn nguyên bằng sự kiện đảo ngược (Priority: P1)

- **Title**: Mô hình điểm append-only, dựng lại được ở bất kỳ mốc nào
- **Actor**: ACTOR-007 server
- **Intent**: Tính điểm của mọi ghế bằng cách rút gọn toàn bộ nhật ký sự kiện, và hoàn nguyên một vòng hay một quyết định bằng cách **thêm** sự kiện đảo ngược thay vì xoá lịch sử.
- **User value**: Mọi con số trên bảng điểm truy nguyên được về một chuỗi sự kiện; khiếu nại phân xử được bằng cách soi lại điểm tại đúng mốc thời gian tranh chấp.
- **Priority**: P1
- **Priority rationale**: Đây là điều kiện tiên quyết của phát lại, soi lại, và phân xử; nếu điểm là một cột số bị ghi đè thì mọi cơ chế hoàn nguyên của EPIC-005 và EPIC-007 mất nền.
- **Independent test**: Chạy chuỗi `+10` → `−5` → điều chỉnh tay `+3`, xác nhận điểm bằng `8` **và** cả ba sự kiện còn nguyên trong nhật ký. Sau đó bỏ vòng và xác nhận điểm tính lại đúng trong khi không sự kiện cũ nào biến mất.
- **Related PRD requirements**: PRD-REQ-039, PRD-REQ-040, PRD-REQ-041
- **Related game rules**: `GR-028`, `GR-013` *(hình dạng sự kiện điểm của Tăng tốc)* · `INV-001`, `INV-002`, `INV-018`
- **Related journey**: JOURNEY-005, JOURNEY-006

---

### US-003 — Server time là nguồn sự thật duy nhất cho mọi mốc (Priority: P1)

- **Title**: Hạn chót, thứ tự chuông và thứ hạng tốc độ tính trên đồng hồ server
- **Actor**: ACTOR-007 server
- **Intent**: Phân xử mọi tranh chấp thời gian — ai bấm trước, bài nào trong hạn, ai nhanh hơn — bằng một đồng hồ duy nhất mà không ai sửa được.
- **User value**: Công bằng đo được: máy thí sinh lệch giờ, mạng trễ, hay admin chỉnh giờ hệ thống giữa trận đều không đảo được kết quả đã ghi.
- **Priority**: P1
- **Priority rationale**: Ba vòng có cơ chế đua tốc độ (Khởi động lượt chung, Tăng tốc, cướp quyền Về đích) đều đọc trực tiếp story này; sai ở đây là sai kết quả trận.
- **Independent test**: Gửi hai tín hiệu lệch nhau đúng **1 ms** và xác nhận hệ thống phân định được; gửi một tín hiệu tới **đúng** mốc hạn và xác nhận nó hợp lệ (biên đóng). Chỉnh giờ hệ thống lùi lại giữa trận và xác nhận thứ hạng đã ghi không đổi.
- **Related PRD requirements**: PRD-REQ-042
- **Related game rules**: `GR-035` · `INV-004`, `INV-005`, `INV-016`
- **Related journey**: JOURNEY-005

---

### US-004 — Mốc thời gian do admin bấm: hai thao tác tuần tự, tuyệt đối (Priority: P1)

- **Title**: *Hiển thị câu hỏi* rồi *start timer* — không đảo, không gộp, không ân hạn
- **Actor**: ACTOR-001 admin *(cảm biến)*; ACTOR-007 server *(ghi mốc)*
- **Intent**: Đánh dấu hai thời điểm mà máy không quan sát được — *câu đã lên sân khấu* và *MC đã đọc xong* — bằng hai cú bấm riêng, để mọi cửa sổ thời gian của luật neo đúng chỗ.
- **User value**: Cửa sổ chuông của Khởi động lượt chung giữ đúng độ dài mà luật gốc cho phép (bấm được trong lúc MC đọc), và cửa sổ đặt Ngôi sao hy vọng có một mốc đóng rõ ràng.
- **Priority**: P1
- **Priority rationale**: Gộp hai mốc là **phá luật** chứ không phải lệch trải nghiệm — cửa sổ chuông co lại còn bằng thời gian suy nghĩ và cửa sổ Ngôi sao hy vọng mất mốc đóng (`PRD-REQ-043`, `QĐ-028`).
- **Independent test**: Ở Khởi động lượt chung, bấm *hiển thị câu hỏi*, chờ vài giây rồi mới bấm *start timer*; xác nhận một tín hiệu chuông phát ra **giữa hai mốc** là hợp lệ, và một tín hiệu đặt Ngôi sao hy vọng phát ra sau mốc thứ nhất thì không tồn tại.
- **Related PRD requirements**: PRD-REQ-043
- **Related game rules**: `GR-033`, `GR-003` *(cửa sổ chuông)*, `GR-021` *(cửa sổ NSHV)*, `GR-031` *(mốc tiêu câu)* · `INV-015`
- **Related journey**: JOURNEY-005

---

### US-005 — Hàng đợi tín hiệu: FIFO thuần, không drop, chặn đúng một chỗ (Priority: P1)

- **Title**: Mọi tín hiệu có outcome; chặn chỉ ở Vượt chướng ngại vật; từ chối không làm mất lượt
- **Actor**: ACTOR-007 server *(xếp hàng)*; ACTOR-001 admin *(duyệt ở VCNV)*; ACTOR-003 thí sinh *(phát tín hiệu)*
- **Intent**: Tiếp nhận mọi tín hiệu của thí sinh theo đúng thứ tự tới, cho tín hiệu ở vòng đua tốc độ có hiệu lực ngay, và bắt tín hiệu một-chiều-hậu-quả-nặng ở VCNV chờ admin xác nhận.
- **User value**: Không tín hiệu nào biến mất không dấu vết, nên admin luôn có căn cứ để can thiệp và **gỡ lệnh cấm**; thí sinh bấm nhầm ở VCNV được admin sửa hộ mà không mất lượt.
- **Priority**: P1
- **Priority rationale**: `INV-006` (mọi tín hiệu có outcome), `INV-007` (FIFO thuần) và `INV-008` (từ chối không mất lượt) là ba bất biến chi phối toàn bộ đường vào của thí sinh; `NON-GOAL-014` cấm hẳn cơ chế drop.
- **Independent test**: Ở VCNV, cho hai thí sinh phát một tín hiệu *chọn hàng ngang* và một tín hiệu *Mở chướng ngại vật* cách nhau vài chục ms; xác nhận admin duyệt đúng theo thứ tự tới không ưu tiên theo loại, và sau khi admin bấm No cho tín hiệu đầu thì ghế đó vẫn chọn lại được, bàn cờ và kho đề không đổi.
- **Related PRD requirements**: PRD-REQ-044, PRD-REQ-045, PRD-REQ-046
- **Related game rules**: `GR-032`, `GR-007` C2/C9, `GR-009` C8, `GR-003` C3, `GR-012` §Đồng thời · `INV-006`, `INV-007`, `INV-008`, `INV-013`
- **Related journey**: JOURNEY-005

---

### US-006 — Rút đề trong danh sách đã gán, không lặp câu trong toàn contest (Priority: P1)

- **Title**: Rút ngẫu nhiên có nhật ký · ranh giới *đã hiển thị* · kiểm kho tại cửa vào từng vòng · bộ VCNV
- **Actor**: ACTOR-007 server *(rút và kiểm)*; ACTOR-001 admin *(chọn danh sách)*
- **Intent**: Lấy câu cho từng lượt bằng cách rút ngẫu nhiên **trong danh sách đã gán**, loại câu đã hiển thị ở bất kỳ trận nào của contest, và chặn một vòng không đủ câu ngay tại cửa vào thay vì để nó chạy dở rồi tắc.
- **User value**: *"Điểm hoàn được, đề đã lộ thì không"* — không câu nào bị hỏi hai lần trong một contest, và không trận nào chết giữa vòng vì cạn đề.
- **Priority**: P1
- **Priority rationale**: `INV-011` và `INV-012` là hai bất biến neo toàn bộ cơ chế bỏ vòng / chạy lại vòng của EPIC-005; vòng VCNV còn thêm ràng buộc **bộ** mà không phép kiểm nào khác bắt được (`PRD-REQ-092`).
- **Independent test**: Gán một danh sách đủ 4 hàng ngang và 1 Chướng ngại vật nhưng **không cùng bộ**; xác nhận vòng VCNV không mở được dù đếm theo số câu là đủ. Ở một vòng khác, rút một câu rồi gỡ nó khỏi danh sách trước khi hiển thị và xác nhận câu quay lại kho; hiển thị một câu rồi bỏ vòng và xác nhận câu đó **không** quay lại.
- **Related PRD requirements**: PRD-REQ-047, PRD-REQ-048, PRD-REQ-092, PRD-REQ-093
- **Related game rules**: `GR-031`, `GR-005` §ghi chú kho đề, `GR-012` C4, `GR-023` §Nguồn đề · `INV-011`, `INV-012`, `INV-014`
- **Related journey**: JOURNEY-002, JOURNEY-005

---

### US-007 — Đáp án kín tới mốc CÂU KHÉP, rồi công bố đúng lúc (Priority: P1)

- **Title**: Phạm vi hiển thị đáp án theo permission và theo mốc câu khép
- **Actor**: ACTOR-007 server *(cổng)*; ACTOR-001 admin và ACTOR-004 MC *(giữ `PERM-045`)*; ACTOR-003 thí sinh, ACTOR-005 khán giả, ACTOR-006 lớp phủ *(nhận từ mốc câu khép)*
- **Intent**: Giữ đáp án chuẩn chỉ ở phía server và phiên giữ permission đọc đáp án cho tới khi câu **khép**, rồi đẩy nó ra ba kênh còn lại nếu cờ *hiện đáp án sau khi chấm* đang bật.
- **User value**: Giải `PS-6` triệt để — đáp án không nằm sẵn trong bộ nhớ máy thí sinh; đồng thời khán giả và stream vẫn có trải nghiệm gameshow chuẩn là biết đáp án ngay sau khi câu xong.
- **Priority**: P1
- **Priority rationale**: `INV-017` là hàng rào chống rò đề, và mốc **câu khép** ở Về đích đến **sau** cú bấm chấm Sai — công bố sớm ở đó xoá sổ cơ chế cướp quyền, tức **sai luật gốc** chứ không phải lệch trải nghiệm (`PRD-REQ-088`).
- **Independent test**: Ở Về đích, chấm Sai người thi chính rồi bắt gói tin và soi bộ nhớ của máy thí sinh, kênh khán giả và lớp phủ trong suốt cửa sổ cướp quyền 5 giây; xác nhận không kênh nào có đáp án. Sau khi người cướp được chấm, xác nhận đáp án xuất hiện đồng thời trên cả ba kênh.
- **Related PRD requirements**: PRD-REQ-049, PRD-REQ-088, PRD-REQ-089
- **Related game rules**: `GR-037`, `GR-020` §Cấm, `GR-008`, `GR-012`, `GR-019` §Bảo mật · `INV-017`
- **Related journey**: JOURNEY-005

---

### US-008 — Vòng Khởi động: lượt riêng và lượt chung (Priority: P1)

- **Title**: 6 câu mỗi thí sinh không phạt, rồi 12 câu tranh chuông có phạt −5
- **Actor**: ACTOR-001 admin *(phán quyết và bấm mốc)*; ACTOR-003 thí sinh *(trả lời)*
- **Intent**: Chạy trọn vòng mở màn theo đúng hai pha của luật gốc, với cửa sổ chuông mở sớm từ mốc hiển thị câu và hình phạt chỉ tồn tại ở pha lượt chung.
- **User value**: Vòng đầu tiên của mọi trận cho ra đúng con số mà khán giả quen thuộc từ chương trình gốc.
- **Priority**: P1
- **Priority rationale**: Đây là vòng đầu của playlist khoá cứng v1 (`PRD-REQ-024`); không có nó thì không trận nào chạy được bước nào.
- **Independent test**: Chạy trọn 6 câu lượt riêng cho một ghế và xác nhận sau câu thứ 6 nút chuyển câu đổi thành *kết thúc lượt* — câu thứ 7 không tồn tại. Chạy một câu lượt chung, cho một ghế bấm chuông rồi im lặng, và xác nhận admin bấm Sai cho ra **−5** trong khi cùng tình huống ở lượt riêng cho ra **0**.
- **Related PRD requirements**: PRD §8.2 *(bảng Số câu theo luật)*, PRD §9.2 *(`STATE-002`, `STATE-003`)*, EPIC-006 §11 · PRD-REQ-036, PRD-REQ-038, PRD-REQ-041
- **Related game rules**: `GR-001`, `GR-002`, `GR-003`, `GR-004`, `GR-005`, `GR-006`
- **Related journey**: JOURNEY-005

---

### US-009 — Vòng Vượt chướng ngại vật: bàn cờ, băng điểm, và cơ chế bị loại (Priority: P1)

- **Title**: 4 hàng ngang + ô trung tâm · băng 60/50/40/30/20 · sai Chướng ngại vật là bị loại
- **Actor**: ACTOR-001 admin *(duyệt tín hiệu và phán quyết)*; ACTOR-003 thí sinh *(chọn hàng ngang, gõ đáp án, bấm Mở chướng ngại vật)*
- **Intent**: Chạy vòng suy luận với bàn cờ năm ô độc lập, băng điểm đọc theo số hàng ngang **đã hỏi**, và cơ chế một-lần-đoán mà sai thì mất quyền trong vòng nhưng giữ nguyên điểm.
- **User value**: Vòng thưởng cho suy luận sớm giữ đúng ý nghĩa: bấm càng sớm giá càng cao, và mở tay một ô không âm thầm cho ai điểm.
- **Priority**: P1
- **Priority rationale**: Đây là vòng duy nhất có hàng đợi **chặn**, cơ chế **bị loại**, và một biến trạng thái (băng điểm) là **hàm** chứ không phải bộ đếm — ba đặc thù không tái sử dụng được từ vòng khác.
- **Independent test**: Mở đúng một hàng ngang rồi cho một ghế bấm *Mở chướng ngại vật*; giữ tín hiệu chờ duyệt và xác nhận trong lúc chờ không hàng ngang nào mở thêm được, băng chốt ở **50** tại mốc admin xác nhận. Cho ghế đó trả lời sai và xác nhận nó bị loại khỏi vòng mà điểm hàng ngang đã kiếm được vẫn nguyên.
- **Related PRD requirements**: PRD §8.2, PRD §9.2 *(`STATE-004`)*, EPIC-006 §11 · PRD-REQ-045, PRD-REQ-046, PRD-REQ-058, PRD-REQ-069, PRD-REQ-092
- **Related game rules**: `GR-007`, `GR-008`, `GR-009`, `GR-010`, `GR-011`, `GR-012` · `INV-019`
- **Related journey**: JOURNEY-005

---

### US-010 — Vòng Tăng tốc: xếp hạng theo tốc độ trên tập người được chấm Đúng (Priority: P1)

- **Title**: 4 câu luôn gõ máy · thang 40/30/20/10 · một câu là một sự kiện điểm cho toàn bảng
- **Actor**: ACTOR-003 thí sinh *(gõ và gửi lại tới hết giờ)*; ACTOR-007 server *(xếp hạng)*; ACTOR-001 admin *(chấm)*
- **Intent**: Chấm cả sân cho một câu rồi xếp hạng những người được chấm Đúng theo mốc server nhận của **bản cuối**, và phát đúng một sự kiện điểm cho cả bảng.
- **User value**: Thí sinh sửa đáp án tự do tới hết giờ mà không tự làm xấu thứ hạng bằng một thao tác vô nghĩa; hoàn nguyên một câu đảo đúng cả bảng chứ không lệch từng ghế.
- **Priority**: P1
- **Priority rationale**: Đây là vòng duy nhất có sự kiện điểm **hình dạng khác** (`PRD-REQ-040`) và là chỗ duy nhất thang điểm phụ thuộc một quan hệ giữa nhiều người — tách sai thì phát lại từng phần cho ra bảng sai.
- **Independent test**: Cho ba ghế gửi đáp án ở `100 ms`, `150 ms`, `200 ms`, chấm Đúng cả ba và xác nhận `+40 / +30 / +20` trong **một** sự kiện. Cho một ghế gửi lại **đúng chuỗi cũ** ở `500 ms` và xác nhận mốc xếp hạng của ghế đó không đổi.
- **Related PRD requirements**: PRD §9.2 *(`STATE-005`)*, EPIC-006 §11 · PRD-REQ-040, PRD-REQ-042, PRD-REQ-066
- **Related game rules**: `GR-013`, `GR-014`, `GR-015` · `TERM-037`
- **Related journey**: JOURNEY-005

---

### US-011 — Vòng Về đích: gói câu, câu thực hành, cướp quyền và Ngôi sao hy vọng (Priority: P1)

- **Title**: Mỗi thí sinh một lượt · gói 3 câu mức {20, 30} · cướp 5 giây · đặt cược gấp đôi
- **Actor**: ACTOR-003 thí sinh *(chọn gói, trả lời, cướp quyền)*; ACTOR-001 admin *(chốt lượt, bấm mốc, phán quyết)*
- **Intent**: Chạy vòng quyết định với thứ tự lượt tính lại sau mỗi lượt, gói câu do thí sinh quyết theo mode, cửa sổ cướp quyền mở ngay tại cú bấm chấm Sai, và cơ chế đặt cược có hình phạt đúng một lần.
- **User value**: Cơ chế chuyển điểm lớn nhất trận hoạt động đúng như luật gốc — người cướp phải thật sự biết đáp án chứ không đọc lại thứ vừa hiện trên màn hình.
- **Priority**: P1
- **Priority rationale**: Vòng này chứa nhiều tương tác chéo nhất (thứ tự lượt × gói câu × thực hành × cướp quyền × Ngôi sao hy vọng) và là chỗ duy nhất mốc **câu khép** không trùng cú bấm chấm.
- **Independent test**: Cho một ghế đặt Ngôi sao hy vọng lên câu 30 điểm, bị chấm Sai, rồi lần lượt dựng ba nhánh — người cướp đúng, người cướp sai, không ai cướp — và xác nhận ghế đó mất **đúng −30** ở cả ba nhánh. Xác nhận thêm rằng nút đặt Ngôi sao hy vọng không tồn tại sau mốc admin bấm hiển thị câu.
- **Related PRD requirements**: PRD §9.2 *(`STATE-006`)*, EPIC-006 §11 · PRD-REQ-038, PRD-REQ-041, PRD-REQ-069, PRD-REQ-070, PRD-REQ-088
- **Related game rules**: `GR-016`, `GR-017`, `GR-018`, `GR-019`, `GR-020`, `GR-021` · `INV-018`
- **Related journey**: JOURNEY-005

---

### US-012 — Vòng Câu hỏi phụ: thể thức ba câu và nguồn đề mượn (Priority: P2)

- **Title**: 3 câu × 15 giây, không cộng trừ điểm, rút từ ba kho nguồn, hết câu thì bốc thăm
- **Actor**: ACTOR-001 admin *(bấm mốc và phán quyết)*; ACTOR-003 thí sinh trong nhóm hoà *(tranh chuông)*; ACTOR-007 server *(rút đề, bốc thăm)*
- **Intent**: Phân định nhóm hoà ở vị trí Nhất bằng ba câu tranh chuông không sinh điểm, lấy đề mượn từ ba kho nguồn theo thứ tự ưu tiên, và bốc thăm khi hết ba câu vẫn chưa ai đúng.
- **User value**: Nhánh phân định chạy được mà không bắt admin chuẩn bị một kho riêng cho một vòng **có thể không bao giờ chạy**, và không tốn thêm câu nào nhờ số dư tất định 12 câu của kho Về đích.
- **Priority**: P2
- **Priority rationale**: Giữ đúng mức đã chốt ở PRD cho `PRD-REQ-090` (P2) — nhánh này chỉ chạy khi có hoà ở vị trí Nhất, nên nó không nằm trên đường đi bắt buộc của một trận, khác với năm vòng còn lại.
- **Independent test**: Nạp một contest đúng 69 câu cộng một bộ Chướng ngại vật, chạy trọn bốn vòng, tạo một nhóm hoà ở vị trí Nhất rồi mở vòng Câu hỏi phụ; xác nhận vòng mở được, ba câu đều rút từ kho Về đích, mỗi câu đúng 15 giây bất kể `timeSeconds` khai ở câu gốc, và không sự kiện điểm nào được sinh.
- **Related PRD requirements**: PRD-REQ-090, PRD-REQ-093 · PRD §8.1 bước 7a/7b
- **Related game rules**: `GR-023`, `GR-024`, `GR-025`, `GR-017` §Biên *(số dư 12 câu)*, `GR-031` *(vỡ bộ)* · `GR-022` *(tiền đề, đặc tả ở spec 005)* · `TERM-020`
- **Related journey**: JOURNEY-005, JOURNEY-007

---

### US-013 — Mất kết nối: giữ ghế, khôi phục đúng trạng thái, không chính sách tự động (Priority: P1)

- **Title**: Ngưỡng chờ 120 giây · quá hạn chỉ tô nổi bật · gói khôi phục không rò đề và không mua thêm thời gian
- **Actor**: ACTOR-003 thí sinh *(mất và nối lại)*; ACTOR-007 server *(phát hiện, dựng gói khôi phục)*; ACTOR-001 admin *(quyết sau ngưỡng)*
- **Intent**: Cho một ghế rớt mạng quay lại đúng màn đang thi với đúng thời gian còn lại, và khi quá ngưỡng thì đưa quyết định cho admin thay vì tự loại hay tự xoá.
- **User value**: Giải `PS-5` — mất kết nối không còn làm hỏng trận, và mạng yếu không bị biến thành hình phạt.
- **Priority**: P1
- **Priority rationale**: `PRD-REQ-050` P1; hệ thống tiền lệ không có cơ chế nối lại nào, nên đây là một trong tám hạn chế cụ thể mà sản phẩm phải giải.
- **Independent test**: Mở một câu VCNV 15 giây tại `t=0`, ngắt kết nối một ghế ở `t=4`, cho nó quay lại ở `t=9`; xác nhận nó thấy còn **6 giây** chứ không phải 15, thấy lại bản gửi của **chính nó** lúc `t=3`, và gói khôi phục không chứa đáp án chuẩn lẫn bài làm của ghế khác.
- **Related PRD requirements**: PRD-REQ-050 · PRD-REQ-049 *(gói khôi phục không rò đáp án)*
- **Related game rules**: `GR-036` · `INV-016`, `INV-017`
- **Related journey**: JOURNEY-006

---

### Edge Cases

**Boundary cases**

- Tín hiệu chuông tới **đúng** mốc `3.000` giây của cửa sổ Khởi động lượt chung, `5.000` giây của cửa sổ cướp quyền, `15.000` giây của Câu hỏi phụ hoặc của câu hàng ngang VCNV — biên **đóng**, tín hiệu **thắng** (`GR-003`, `GR-005` C3, `GR-008` C8, `GR-032` §Biên, `INV-005`).
- Bài gửi Tăng tốc của hai ghế lệch **đúng 1 ms** — vẫn phân định được, **không** hoà (`GR-014` C6).
- Băng điểm Chướng ngại vật khi **chưa hàng nào** được hỏi là **60**; sau khi gợi ý cuối đã đưa ra là **20** và đó là **sàn** — bấm khi vừa hỏi xong hàng thứ 4 mà gợi ý cuối **chưa** đưa ra vẫn là **30** (`GR-009` C1/C5, §Biên).
- Ngưỡng chờ mất kết nối đúng `120.000` giây **vẫn trong ngưỡng**; quá mốc mới hết (`GR-036` §Biên).
- Câu thứ **7** của lượt riêng, câu thứ **13** của lượt chung, câu thứ **5** của Tăng tốc: **không tồn tại** — không phải chặn cứng, không phải cảnh báo, mà là **không có đường vào** (`GR-001` §Biên, `GR-004` §Biên, bảng *Số câu theo luật*, `INV-012`).
- Nhóm hoà Tăng tốc **0** hoặc **1** người: không phải hoà, luật nhảy bậc không áp (`GR-014` §Biên).
- Một mình thi ở Tăng tốc và được chấm Đúng ⇒ **+40** (`GR-013` C3, §Biên).

**Invalid state**

- Bấm chấm cho một câu thuộc **vòng đã bị bỏ** ⇒ toast *invalid state*, không sinh sự kiện cho vòng đã bỏ; **không ép được** (`GR-001` C4, `GR-013` C7).
- Trỏ vào một hàng ngang **đã mở**, bấm chuông từ một ghế **đã bị loại**, bấm *Mở chướng ngại vật* khi **đã có người giải đúng**, thao tác sau khi **vòng đã kết thúc** ⇒ nút không render và bấm không phản hồi ⇒ **không tín hiệu nào được sinh** (`GR-007` C5, `GR-009` C9/C10, `GR-010` C4, `GR-012` C6, `GR-032` C6).
- Mode **sân khấu**: máy thí sinh gửi tín hiệu chọn hàng ngang, chọn gói câu, hoặc đặt Ngôi sao hy vọng ⇒ đường này **không tồn tại**; server từ chối và ghi nhật ký thao tác (`GR-007` C6, `GR-017` C5, `GR-021` C8, `QĐ-019`).
- Đặt Ngôi sao hy vọng **sau** mốc admin bấm hiển thị câu ⇒ nút không render; ngôi sao **vẫn chưa dùng**, không bị tiêu oan (`GR-021` C6).
- Bấm chuông ở Câu hỏi phụ **trước** mốc admin bấm start timer ⇒ không có nút; nếu một client tự chế vẫn gửi được thì server **từ chối** và ghi nhật ký (`GR-024` C1).
- Cấu hình `rowCount` ≠ 4 ⇒ không dựng được ở v1; băng điểm cho 5-8 hàng **không tồn tại trong bất kỳ nguồn nào** (`GR-009` C11, `PRD-REQ-025`).

**Repeated action · idempotency**

- Bấm chấm lần thứ hai cho cùng một câu ⇒ nút đã tự khoá; nếu yêu cầu vẫn lọt tới server thì bị **từ chối**, điểm không đổi và **không sinh sự kiện** (`GR-026` §Bấm trùng, `PRD-REQ-037`, `INV-009`).
- Bấm *start timer* lần thứ hai ⇒ **không xảy ra**; đây là ngoại lệ duy nhất của nguyên tắc *"đồng hồ khoá thí sinh, không khoá admin"*, và nó tồn tại để cửa sổ không bị kéo dài do thao tác lặp (`GR-033` C2).
- Bấm *mở miếng ghép*, *mở ô trung tâm*, *công bố Chướng ngại vật* lần thứ hai ⇒ nút một chiều đã tự tắt; server bỏ qua lệnh trùng (`GR-008` C5, `GR-011` C8, `GR-012` C5).
- Từ chối một tín hiệu **đã bị từ chối** ⇒ **idempotent**, không đổi gì (`GR-032` §Bấm trùng).
- Rút đề lần nữa khi câu của lần rút trước **chưa hiển thị** ⇒ sinh sự kiện rút đề riêng, **không dedup**, và an toàn vì câu cũ chưa tiêu nên trả lại kho (`GR-031` §Bấm trùng).
- Gửi lại đáp án **y hệt** ở Tăng tốc ⇒ vẫn là bản được ghi nhận nhưng mốc xếp hạng **không cập nhật** (`GR-015` C1, `PRD-REQ-066`).
- Gửi lại đáp án **y hệt** ở Khởi động ⇒ cập nhật bản được ghi nhận; quy tắc *y-hệt-thì-không-cập-nhật-mốc* **không áp** vì vòng này không xếp theo thời gian (`GR-006` C6 và ghi chú kèm).
- *"Bốc lại"* ở Câu hỏi phụ **không idempotent theo thiết kế** — mục đích của nó là ra kết quả khác; chống bấm nhầm nằm ở nút một chiều và dialog, không ở dedup phía server (`GR-025` §Bấm trùng).

**Stale state**

- Băng điểm Chướng ngại vật chốt theo trạng thái bàn cờ tại **mốc admin xác nhận tín hiệu**, không phải mốc thí sinh bấm; tình huống *"số hàng đổi giữa hai mốc"* **không dựng được** vì hàng đợi ở VCNV **chặn** (`GR-009` C7, §Đồng thời).
- Thứ tự lượt Về đích lấy bảng điểm **sau khi lượt trước kết thúc hoàn toàn** — cửa sổ cướp đã đóng **và** đã chấm xong; không tính giữa lúc một câu đang chạy (`GR-016` §Đồng thời).
- Người vừa bấm *Mở chướng ngại vật* vẫn **được** trả lời một câu hàng ngang trong lúc tín hiệu chờ duyệt; khoá sớm là **nghiêm hơn luật** và sẽ vi phạm quy tắc *"admin bấm No ⇒ không mất gì"* (`GR-009` C14).
- Người vừa bị loại đang có một đáp án hàng ngang **chờ chấm**: bản đó **vẫn được chấm bình thường** nếu gửi **trước** mốc phán quyết Sai (`GR-010` §Đồng thời).

**Duplicate event · đồng thời**

- Hai ghế bấm chuông có **cùng** server timestamp ở một vòng không chặn ⇒ hàng đợi **tự quyết, ngẫu nhiên lúc nhận** nhưng **tất định khi dựng lại**, vì thứ tự đã chọn được ghi và không xoá (`GR-003` C4, `GR-032` §Đồng thời, `GR-035` C2, `QĐ-025`).
- Một tín hiệu *chọn hàng ngang* và một tín hiệu *Mở chướng ngại vật* cùng chờ trong hàng đợi chặn ⇒ xử lý **thuần theo thứ tự tới, KHÔNG ưu tiên theo loại**; thêm quy tắc ưu tiên sẽ tạo một tiêu chí mà thí sinh không quan sát được (`GR-007` C9, `GR-032` §Đồng thời).
- Hai thí sinh Tăng tốc cùng mili-giây và cùng được chấm Đúng ⇒ **cùng bậc, cùng mức điểm**, bậc kế nhảy qua số người hoà (`GR-014`, `GR-035` C4).
- Tín hiệu *Mở chướng ngại vật* tới **giữa lúc đồng hồ câu hàng ngang đang chạy** ⇒ ghi nhận ngay, đồng hồ **vẫn chạy**, và **cả** đáp án chuẩn **lẫn** bài làm của người khác đều chưa hiển thị (`GR-008` C9, `INV-013`).
- Phán quyết Sai cho Chướng ngại vật chốt **trong lúc đồng hồ hàng ngang đang chạy** ⇒ đồng hồ chạy tiếp, không dừng, không kéo dài; câu hàng ngang tiếp tục cho những người còn quyền (`GR-010` §Đồng thời).

**Partial failure**

- Admin bấm *chốt câu* ở Tăng tốc hoặc câu hàng ngang VCNV khi mới chấm một phần ⇒ mọi ghế chưa chấm tính là **Sai**; ở Tăng tốc ghế đó nhận 0 và **không giữ chỗ** trong thang, ở hàng ngang VCNV ghế đó nhận 0 và **KHÔNG bị loại** (`GR-026` C4).
- Cùng cú bấm đó **không** chạm tới tín hiệu *Mở chướng ngại vật* đang chờ duyệt — không tồn tại đường nào để một cú chốt câu **loại** một thí sinh (`GR-026` C5, `PRD-REQ-062`).
- Ghế bị **vô hiệu hoá** giữa một câu đang mở ⇒ xử **y như ghế không trả lời**, mặc định Sai khi admin chốt câu; thấy bất công thì admin cộng tay (`GR-036` C7).
- Một hàng ngang **chưa bao giờ được chọn** khi vòng VCNV kết thúc vì toàn bộ bị loại ⇒ câu của nó **chưa hiển thị cho ai ⇒ chưa tiêu, trả lại kho** (`GR-012` C4).
- Ký tự thí sinh gõ dở tại thời điểm rớt mạng, chưa bấm gửi ⇒ **mất**, vì nó chưa từng tới server; bản đã gửi trước đó còn nguyên (`GR-036` §Ví dụ).

**Conflicting rule**

- Luật gốc nói *"nếu không thay đổi thì ghi nhận đáp án đầu tiên"*, còn hệ thống **luôn ghi nhận bản cuối** — hai phát biểu **không mâu thuẫn**: gửi một lần thì bản đầu **chính là** bản cuối (`GR-006` §Điều kiện).
- `GR-017` giữ fallback *admin chọn hộ gói mặc định* ở mode nhập liệu, trong khi `GR-007` **cấm tuyệt đối** admin chọn thay hàng ngang ở cùng mode. Đây là **khác biệt có chủ ý**, đã ghi rõ ở `GR-017` §Nguồn: lượt thi cần một đường thoát để không tắc, còn chọn hàng ngang không có mốc *"quá hạn"* tương đương.
- `GR-007` cho *"tối đa 1 lượt chọn mỗi thí sinh"* nhưng C4 cho lượt **quay lại vị trí số 1** khi còn hàng ngang chưa chọn — là **ngoại lệ tường minh**, chỉ áp khi đã có người bị loại (`GR-007` §Điều kiện).
- `stripDiacritics` bật trông như chỏi với luật gốc *"bất kỳ sai sót về kí tự, dấu câu → không được công nhận"* — **không cùng đối tượng**: nguồn nói về **phán quyết**, cờ này chỉ đổi **cách tô màu**, và máy không phán quyết (`GR-027` §ghi chú).
- Ở Tăng tốc, quy tắc *"cùng thời gian thì cùng mức điểm"* **không dùng lại được** ở Khởi động lượt chung: điểm chia được, quyền trả lời thì không (`GR-003` §Đồng thời).

**Missing source behavior**

- Băng điểm Chướng ngại vật cho `rowCount` **5-8**: nguồn **không có thang nào**; khi mở khoá ở phiên bản sau, băng phải là **mảng cấu hình bắt buộc** dài bằng `rowCount`, không suy ra từ luật (`GR-009` C11). Ngoài phạm vi v1 — xem §Open Questions.
- Thang điểm Tăng tốc và thứ tự lượt Về đích cho **số ghế ≠ 4**: nguồn chỉ viết cho đúng 4 người (`GR-013` C5, `GR-014` §Biên, `GR-016` §Biên, `GR-020` C6, `NON-GOAL-012`). Ngoài phạm vi v1 — xem §Open Questions.
- **Thời lượng suy nghĩ của câu ô trung tâm VCNV**: `GR-011` khai `+10` khi đúng và cửa sổ **15 giây** để giải Chướng ngại vật **sau** gợi ý cuối, nhưng không khai thời lượng của chính câu ô trung tâm — xem §Open Questions.
- **Mốc câu khép khi người cướp quyền Về đích bị chấm *Huỷ kết quả***: `GR-037` C6 nói câu chỉ khép sau khi người cướp được chấm, còn C8 nói phán quyết *Huỷ kết quả* **không tự công bố**; tổ hợp hai điều này không được nguồn nào phân xử — xem §Open Questions.
- **Nguồn đề của Câu hỏi phụ trong một trận `practice` chạy trong contest thật**: `GR-031` C9 đảo chiều phép lọc *(chỉ câu đã dùng)*, còn `GR-023` §Nguồn đề mặc định phép lọc thuận *(loại câu đã dùng)* — giao của hai quy tắc không được nêu, xem §Open Questions.

## Requirements *(mandatory)*

### Functional Requirements

#### Nhóm A — Phán quyết là đường sinh điểm duy nhất (US-001)

- **FR-001**: Hệ thống MUST NOT có bất kỳ đường xử lý nào tự cộng hoặc trừ điểm từ kết quả đối chiếu văn bản giữa bài làm và đáp án; điểm MUST chỉ chốt sau khi admin bấm một trong các nút phán quyết. *(US-001 · PRD-REQ-036 · GR-026, INV-003 · AC-001, AC-003)*
- **FR-002**: Hệ thống MUST chỉ làm đúng hai việc với bài làm dạng chữ: hiển thị nó cạnh đáp án, và tô nổi bật khác biệt ký tự. Kết quả đối chiếu MUST NOT xuất hiện ở bất kỳ vị trí nào trong đường sinh sự kiện điểm, và MUST NOT tồn tại cấu hình nào bật chấm tự động. *(US-001 · PRD-REQ-036 · GR-026, GR-027, NON-GOAL-001 · AC-003, AC-015)*
- **FR-003**: Một câu MUST chỉ được **KHÉP một lần**; sau khi khép, hệ thống MUST NOT cho chấm lại, đổi phán quyết tại chỗ, hay tự tính lại thứ hạng — sửa sai MUST đi qua điều chỉnh điểm thủ công. Đơn vị chống trùng của một phán quyết MUST là bộ ba *(câu, thí sinh, loại phán quyết)* — **không** phải *(câu)* — vì một câu hợp lệ mang **nhiều** phán quyết ở các vòng nhiều người: `GR-008` chấm **từng** thí sinh cho một câu hàng ngang, và `GR-018` → `GR-020` chấm người thi chính rồi chấm **người cướp quyền** cho cùng một câu Về đích. Ở các vòng chấm **từng ghế rồi mới chốt câu** — Tăng tốc và câu hàng ngang VCNV — dấu Đúng/Sai đặt cho một ghế MUST là **lựa chọn tạm**, MUST NOT sinh sự kiện điểm ngay, và admin MUST sửa lại được **tuỳ ý cho tới mốc bấm *chốt câu*** (xem FR-009a). Ở các vòng còn lại, cú bấm phán quyết **chính là** mốc khép của đối tượng đó và hệ thống MUST khoá nút chấm của đúng đối tượng vừa chấm. Trong cả hai trường hợp, sau khi câu đã khép, server MUST từ chối một phán quyết trùng bộ ba đó kể cả khi yêu cầu lọt tới, và MUST NOT sinh sự kiện cho lần bấm đó. *(US-001 · PRD-REQ-037 · GR-026 §Bấm trùng, GR-008 C2, GR-020, INV-009 — đọc `INV-009` là *"một câu khép một lần"*, xem §Clarifications 2026-07-30 · AC-006)*
- **FR-004**: Phán quyết MUST là điều kiện để chuyển sang câu kế; hệ thống MUST NOT tồn tại trạng thái *"vòng đã đóng mà còn câu chưa chấm"*. *(US-001 · PRD-REQ-037 · GR-026, INV-010 · AC-003)*
- **FR-005**: Số lựa chọn phán quyết MUST đổi theo vòng: hai lựa chọn **Đúng / Sai** ở vòng mà *Sai* trừ **0** điểm (Khởi động lượt riêng, câu hàng ngang VCNV, Tăng tốc, Câu hỏi phụ, Về đích người thi chính); ba lựa chọn **Đúng / Sai / Huỷ kết quả** ở vòng mà *Sai* kéo theo hình phạt (Khởi động lượt chung, Về đích người cướp quyền, Về đích câu có Ngôi sao hy vọng). *(US-001 · PRD-REQ-038 · GR-026, bảng §Phán quyết có hai hay ba lựa chọn · AC-007)*
- **FR-006**: Lựa chọn **Huỷ kết quả** MUST luôn có mặt khi một câu **chỉ có bản gửi quá hạn**, kể cả ở vòng mà *Sai* trừ 0 điểm. Dù hai hay ba lựa chọn, vẫn MUST là **một** phán quyết cho một câu. *(US-001 · PRD-REQ-038 · bảng §Bản gửi quá hạn, bảng §Phán quyết · AC-008)*
- **FR-007**: Khi một ghế có bản gửi **hợp lệ** và còn gửi thêm bản **quá hạn**, hệ thống MUST giữ **cả hai** và MUST đánh dấu bản quá hạn bằng màu đỏ; bản quá hạn MUST NOT tự ghi đè bản hợp lệ, và hệ thống MUST NOT tự loại bản quá hạn. Cả việc **hiển thị** lẫn việc **chấm** bản quá hạn MUST là thao tác bấm của admin. *(US-001 · PRD-REQ-036, PRD-REQ-038 · bảng §Bản gửi quá hạn, GR-035 C6 · AC-009, AC-030)*
- **FR-008**: Phép chuẩn hoá bài làm MUST gồm ba bước **bắt buộc** — không phân biệt hoa thường, cắt khoảng trắng hai đầu, gộp khoảng trắng thừa — và một bước **tuỳ chọn** là bỏ dấu tiếng Việt, mặc định **TẮT**. Phép chuẩn hoá MUST là hàm thuần, tất định. Bài làm **gốc** MUST được lưu; kết quả chuẩn hoá MUST chỉ là dữ liệu tạm để hiển thị. Danh sách đáp án được chấp nhận MUST cố định từ lúc soạn câu và MUST NOT sửa được giữa trận. *(US-001 · PRD-REQ-036 · GR-027 · AC-010 → AC-014)*
- **FR-009**: Ở **Tăng tốc** và **câu hàng ngang Vượt chướng ngại vật**, cú bấm *chốt câu* của admin MUST chính là phán quyết cho mọi ghế chưa được chấm, và những ghế đó MUST tính là **Sai**. Ở Tăng tốc, ghế đó MUST nhận 0 và MUST NOT giữ chỗ trong thang xếp hạng. Ở câu hàng ngang VCNV, ghế đó MUST nhận 0 và MUST NOT bị loại khỏi vòng. Hệ thống MUST NOT có bộ đếm tự chốt khi hết giờ. Quy tắc này MUST NOT áp lên tín hiệu *"Mở chướng ngại vật"* đang chờ duyệt. *(US-001, US-009, US-010 · PRD-REQ-036 · GR-026 C4, C5 · AC-004, AC-005)*
- **FR-009a**: Ở **Tăng tốc** và **câu hàng ngang VCNV**, dấu Đúng/Sai của từng ghế MUST sửa lại được **không giới hạn số lần** cho tới mốc admin bấm *chốt câu*; hệ thống MUST NOT khoá dấu của một ghế ngay khi đặt. Sự kiện điểm MUST chỉ sinh tại **cú bấm *chốt câu***, và từ mốc đó MUST NOT còn đường đổi phán quyết của bất kỳ ghế nào trong câu — sửa sai MUST đi qua điều chỉnh điểm thủ công. Điều này khớp `GR-013` §Bấm trùng *("không có đường đổi phán quyết của một người **sau khi đã chốt câu**")* và `GR-008` §Thứ tự đánh giá *("chấm **toàn bộ** thí sinh → rồi mới đóng câu")* (§Clarifications 2026-07-30). *(US-001, US-009, US-010 · PRD-REQ-036, PRD-REQ-037 · GR-008 §Thứ tự đánh giá, GR-013 §Bấm trùng, GR-026 C4, INV-009 · AC-004a)*
- **FR-010**: Thời điểm admin chấm được MUST phụ thuộc **kênh trả lời**, không phụ thuộc vòng: kênh **nói** (mode sân khấu) MUST không có nút gửi phía thí sinh và nút chấm MUST bấm được bất cứ lúc nào; kênh **gõ** (mode nhập liệu và các vòng luôn gõ máy) MUST giữ nút gửi sống tới khi hết giờ và MUST khoá nút chấm tới khi hết giờ; kênh **thực hành** MUST để nút chấm sống suốt vì không có ô nhập nào để chờ. *(US-001, US-011 · PRD-REQ-036, PRD-REQ-038 · bảng §Kênh trả lời, GR-019 · AC-016)*

#### Nhóm B — Mô hình điểm (US-002)

- **FR-011**: Điểm của một ghế MUST là kết quả tính lại từ toàn bộ nhật ký sự kiện của trận, MUST NOT là một giá trị được ghi đè trực tiếp. *(US-002 · PRD-REQ-039 · GR-028, INV-002 · AC-017)*
- **FR-012**: Nhật ký sự kiện MUST linear và append-only: hệ thống MUST NOT xoá, MUST NOT sửa, và MUST NOT đảo thứ tự bất kỳ sự kiện đã ghi nào. *(US-002 · PRD-REQ-039 · GR-028, INV-001 · AC-017, AC-018)*
- **FR-013**: Hoàn nguyên MUST thực hiện bằng cách **thêm** sự kiện đảo ngược; bỏ một vòng MUST sinh một sự kiện đảo ngược cho **mỗi** sự kiện điểm thuộc vòng đó. *(US-002 · PRD-REQ-039 · GR-028 C4, TERM-023 · AC-018)*
- **FR-014**: Phép tính điểm MUST tất định — cùng một chuỗi sự kiện MUST luôn cho cùng kết quả — và hệ thống MUST trả lời được câu hỏi *"điểm tại mốc thời gian T là bao nhiêu"* mà MUST NOT sinh sự kiện nào cho phép truy vấn đó. *(US-002 · PRD-REQ-039 · GR-028 C5 · AC-019, AC-020)*
- **FR-015**: Ở mọi vòng **trừ Tăng tốc**, một sự kiện điểm MUST ứng với đúng **một** thí sinh và MUST chống trùng theo bộ ba *(câu, thí sinh, loại phán quyết)*. *(US-002 · PRD-REQ-040 · GR-028, TERM-022 · AC-021)*
- **FR-016**: Ở **Tăng tốc**, một câu MUST sinh đúng **một** sự kiện điểm cho **toàn bộ** bảng; hoàn nguyên một câu Tăng tốc MUST đảo cả bảng điểm của câu đó chứ MUST NOT đảo từng ghế riêng lẻ. *(US-002, US-010 · PRD-REQ-040 · GR-013, GR-028 · AC-022)*
- **FR-017**: Hệ thống MUST NOT kẹp điểm về 0; điểm âm MUST hợp lệ và MUST tham gia bình thường vào xếp thứ tự lượt Về đích, điều kiện hoà, và mọi bảng hiển thị. *(US-002, US-011 · PRD-REQ-041 · GR-004 C5, GR-016, GR-020, INV-018 · AC-023)*
- **FR-018**: Sự kiện phân định Câu hỏi phụ MUST là **sự kiện thứ hạng**, MUST NOT tham gia phép tính điểm; hoàn nguyên nó MUST NOT đi qua sự kiện đảo ngược mà đi qua cửa **bỏ vòng** của vòng phân định. *(US-002, US-012 · PRD-REQ-039 · GR-028 §ghi chú, GR-023, QĐ-083 · AC-024)*

#### Nhóm C — Server time (US-003)

- **FR-019**: Hạn chót của một câu, thứ tự chuông và thứ hạng tốc độ MUST tính trên đồng hồ **server**, ở độ phân giải **mili-giây**; *"cùng lúc"* MUST nghĩa là **cùng mili-giây**, so trên số nguyên, không làm tròn và không dùng số thực. *(US-003 · PRD-REQ-042 · GR-035, GR-014, INV-004 · AC-027, AC-028)*
- **FR-020**: Mọi biên thời gian MUST là **biên đóng**: một tín hiệu hoặc một bản gửi tới **đúng** mốc MUST được coi là hợp lệ; chỉ khi **vượt quá** mốc mới mặc định không tính. *(US-003 · PRD-REQ-042 · GR-035 §Biên, INV-005 · AC-031)*
- **FR-021**: Thứ tự và xếp hạng trong một trận MUST xác định bằng **đồng hồ đơn điệu** của tiến trình server; đồng hồ tường MUST chỉ dùng để hiển thị và ghi nhật ký. Chỉnh giờ hệ thống giữa trận MUST NOT đảo được thứ hạng đã ghi, và máy thí sinh không đồng bộ giờ MUST NOT ảnh hưởng tới kết quả. *(US-003 · PRD-REQ-042 · GR-035 · AC-029, AC-032)*
- **FR-022**: Client MUST chỉ hiển thị thời gian; hệ thống MUST đẩy hạn chót xuống client dưới dạng **mốc tuyệt đối trên đồng hồ server**, MUST NOT dưới dạng *"còn N giây"*. *(US-003, US-013 · PRD-REQ-042 · GR-035, GR-036 C8 · AC-029, AC-200)*
- **FR-023**: Khi hai tín hiệu ở một vòng **không chặn** có cùng server timestamp, hàng đợi MUST tự quyết thứ tự — ngẫu nhiên tại thời điểm nhận, nhưng **tất định khi dựng lại** nhờ thứ tự đã chọn được ghi và không xoá. Hệ thống MUST NOT ưu tiên theo số ghế hay số vị trí. *(US-003, US-005 · PRD-REQ-042, PRD-REQ-044 · GR-003 C4, GR-032 §Đồng thời, GR-035 C2, QĐ-025 · AC-026)*
- **FR-024**: Đồng hồ MUST NOT đóng băng vì bất kỳ sự cố nào ngoài sân khấu; một cửa sổ đã mở MUST chạy hết theo server time. *(US-003, US-013 · PRD-REQ-042 · GR-033, GR-036 C4, INV-016, NON-GOAL-015 · AC-199)*

#### Nhóm D — Mốc do admin bấm (US-004)

- **FR-025**: Mọi thời điểm mà luật gốc mô tả bằng **hành vi của MC** MUST ánh xạ thành **một cú bấm của admin**; hệ thống MUST NOT suy ra các mốc đó từ bất kỳ tín hiệu tự động nào. *(US-004 · PRD-REQ-043 · GR-033, TERM-028 · AC-034, AC-035)*
- **FR-026**: *Hiển thị câu hỏi* và *start timer* MUST là **hai thao tác riêng, thứ tự cố định** — hiển thị trước, start timer sau. Hệ thống MUST NOT cho đảo thứ tự, MUST NOT cho gộp, và MUST NOT có cấu hình nào gộp chúng lại. *(US-004 · PRD-REQ-043 · GR-033, QĐ-028 · AC-037, AC-040)*
- **FR-027**: Mốc *hiển thị câu hỏi* MUST đồng thời tạo đúng bốn hệ quả: đưa câu lên màn thí sinh và khán giả · **mở cửa sổ chuông** *(chỉ ở Khởi động lượt chung)* · **đóng cửa sổ đặt Ngôi sao hy vọng** *(mọi vòng)* · đánh dấu câu **đã dùng**. *(US-004, US-006, US-008, US-011 · PRD-REQ-043, PRD-REQ-047 · GR-033 C3, GR-003, GR-021, GR-031 · AC-034)*
- **FR-028**: Mốc do admin bấm MUST tuyệt đối: hệ thống MUST NOT có cửa sổ ân hạn và MUST NOT trừ bù độ trễ tay người hay độ trễ mạng. *(US-004 · PRD-REQ-043 · GR-033 C6, QĐ-027 · AC-039)*
- **FR-029**: Nút *start timer* MUST tự khoá ngay sau lần bấm đầu, để cửa sổ thời gian MUST NOT bị kéo dài bởi thao tác lặp. *(US-004 · PRD-REQ-043 · GR-033 C2 · AC-036)*

#### Nhóm E — Hàng đợi tín hiệu (US-005)

- **FR-030**: Mọi tín hiệu của thí sinh MUST vào một hàng đợi xử lý **FIFO thuần theo server timestamp**; hệ thống MUST NOT ưu tiên theo loại tín hiệu, số ghế, hay số vị trí. *(US-005 · PRD-REQ-044 · GR-032, GR-007 C9, INV-007 · AC-048)*
- **FR-031**: Mọi tín hiệu đã tới server MUST có đúng một outcome — **thực thi**, **bị từ chối**, hoặc **trơ** *(được ghi nhận, không sinh hệ quả)* — và MUST được ghi kèm server timestamp. Hệ thống MUST NOT có cơ chế drop tín hiệu. *(US-005 · PRD-REQ-044 · GR-032, GR-003 C3, INV-006, NON-GOAL-014 · AC-049)*
- **FR-032**: **Lịch sử tín hiệu MUST NOT bị xoá** trong bất kỳ hoàn cảnh nào. Chỉ **hàng đợi đang hoạt động** được đặt lại, và việc đặt lại MUST theo **đích** của tín hiệu: tín hiệu chọn hàng ngang vô hiệu khi lượt chọn đó kết thúc · tín hiệu trả lời vô hiệu khi câu được chấm xong · tín hiệu *Mở chướng ngại vật* vô hiệu khi vòng VCNV kết thúc. *(US-005 · PRD-REQ-044 · GR-032 §Vòng đời tín hiệu, GR-012 §Đồng thời, INV-001 · AC-045, AC-047, AC-051)*
- **FR-033**: Ở **Vượt chướng ngại vật** — chọn hàng ngang và *Mở chướng ngại vật* — tín hiệu MUST chờ admin xác nhận mới có hiệu lực; chừng nào một tín hiệu còn chờ duyệt thì hệ thống MUST NOT để trạng thái bàn cờ đổi. *(US-005, US-009 · PRD-REQ-045 · GR-032 C1, GR-009 C7 · AC-041, AC-042)*
- **FR-034**: Ở **Khởi động lượt chung**, **cướp quyền Về đích** và **Câu hỏi phụ**, tín hiệu MUST có hiệu lực **ngay** theo server timestamp, MUST NOT chờ duyệt; hàng đợi ở đó MUST vẫn ghi thứ tự làm lưới an toàn cho admin can thiệp. *(US-005 · PRD-REQ-045 · GR-032 C4, GR-003, GR-020, GR-023 · AC-044)*
- **FR-034a**: Ở **mọi vòng có giành quyền trả lời bằng chuông** — Khởi động lượt chung, cướp quyền Về đích, Câu hỏi phụ, **và tín hiệu *"Mở chướng ngại vật"* của VCNV** *(được `GR-034` xếp là chuông — xem FR-077)* — khi người đang giữ quyền bị chấm **Huỷ kết quả**, admin MUST **kích hoạt tay** được **một tín hiệu bất kỳ còn hiệu lực** trong hàng đợi để người phát tín hiệu đó thành người giữ quyền mới. Hệ thống MUST **khuyến nghị** tín hiệu **sớm nhất chưa được xử lý** theo server timestamp, nhưng MUST cho admin chọn một tín hiệu khác — đây là **khuyến nghị, không phải ràng buộc cưỡng chế**, cùng mẫu với `GR-016` C5 và `INV-020`. Hệ thống MUST NOT chặn cứng lựa chọn lệch thứ tự: `INV-014` chỉ có **ba** chỗ chặn cứng và đây không phải một trong ba. Tập ứng viên MUST giới hạn ở những tín hiệu còn hiệu lực với **đích** của chúng — cùng **một câu** ở Khởi động lượt chung / cướp quyền Về đích / Câu hỏi phụ, cùng **một vòng** ở VCNV (`GR-032` §Vòng đời tín hiệu); tín hiệu thuộc câu đã khép hoặc vòng đã đóng MUST NOT là ứng viên dù vẫn còn trong lịch sử. Việc kích hoạt MUST là **thao tác của admin**, MUST NOT tự động. Tín hiệu được kích hoạt MUST chuyển từ trạng thái **trơ** sang **có hiệu lực** và MUST giữ nguyên trong lịch sử ở cả hai trạng thái. Đây là hiện thực cụ thể của mệnh đề *"hàng đợi là lưới an toàn để admin can thiệp khi có sự cố"* (`GR-032` §Kích hoạt tay, `PRD-REQ-045`); máy trạng thái đặc tả ở `EVENT-052` và `T-097` → `T-100` (`QĐ-104`). *(US-005, US-008, US-011, US-012 · PRD-REQ-044, PRD-REQ-045 · GR-032 C4, GR-003 C3, GR-020, GR-023 · AC-044a)*
- **FR-034b**: Ba tham số của thao tác kích hoạt tay MUST là: **(a) Đồng hồ** — server MUST cấp lại **trọn** cửa sổ suy nghĩ của vòng cho người được kích hoạt, tính từ **mốc admin bấm kích hoạt** *(Khởi động lượt chung: **3 giây** · Câu hỏi phụ: **15 giây cố định** — con số này MUST NOT cấu hình được, khớp FR-101 · Về đích: vòng này không có đồng hồ trả lời riêng nên **không** cửa sổ nào được mở)*. **(b) Số lần** — admin MUST kích hoạt tay tối đa **ĐÚNG MỘT LẦN cho mỗi câu**; sau lần đó câu MUST đi tiếp theo luật bình thường, và một yêu cầu kích hoạt thứ hai cho cùng câu MUST bị server từ chối. *(VCNV theo một giới hạn khác vì tín hiệu ở đó gắn với **vòng** — xem FR-034d.)* **(c) Hình phạt** — người được kích hoạt MUST chịu **y hệt** số học của vòng như một người giành quyền bình thường: Khởi động lượt chung `+10 / −5 / Huỷ kết quả`, cướp quyền Về đích `transfer / −½ giá trị câu / Huỷ kết quả`, Câu hỏi phụ không sinh điểm. Hệ thống MUST NOT tạo một bảng điểm riêng cho người được kích hoạt (§Clarifications 2026-07-30). *(US-005, US-008, US-011, US-012 · PRD-REQ-044, PRD-REQ-045 · GR-003, GR-004, GR-020, GR-023 · AC-044b, AC-044c)*
- **FR-034c**: Tín hiệu *"Mở chướng ngại vật"* của VCNV MUST có **ba** lựa chọn phán quyết — **Đúng / Sai / Huỷ kết quả** — thay vì hai như `GR-009` mô tả. *Huỷ kết quả* ở đây MUST là đường để admin khép một tín hiệu **đã xác nhận** mà **không loại** thí sinh, và MUST NOT sinh điểm cho ai. Khi dùng nó, admin MUST kích hoạt tay được **một tín hiệu *"Mở chướng ngại vật"* bất kỳ còn hiệu lực** trong hàng đợi chặn của vòng, theo FR-034a và FR-034e. Hai tham số của FR-034b ánh xạ sang vòng này như sau: **đồng hồ** — vòng VCNV **không có** cửa sổ trả lời riêng cho tín hiệu Chướng ngại vật, nên **không** cửa sổ nào được mở *(suy ra cùng cách với nhánh Về đích của FR-034b)*; **hình phạt** — người được kích hoạt chịu y hệt luật của `GR-009`/`GR-010`, tức chấm Sai vẫn **bị loại khỏi vòng** và **không** trừ điểm. Băng điểm MUST vẫn chốt tại **mốc admin xác nhận** tín hiệu được kích hoạt, theo FR-078. **Đây là bổ sung ngoài bảng §2.2 và ngoài `GR-009` — xem `OQ-006`** (§Clarifications 2026-07-30). *(US-005, US-009 · PRD-REQ-045, PRD-REQ-038 · GR-009 C6, C8, GR-010, GR-034, bảng §2.2 · AC-044d)*
- **FR-034d**: Giới hạn số lần kích hoạt tay MUST khác nhau theo **đích** của tín hiệu, vì đích quyết định phạm vi sống của nó: ở **Khởi động lượt chung, cướp quyền Về đích và Câu hỏi phụ** — tín hiệu gắn với **một câu** — giới hạn MUST là **đúng một lần cho mỗi câu** (FR-034b b). Ở **VCNV** — tín hiệu *"Mở chướng ngại vật"* gắn với **cả vòng** (`GR-012` §Đồng thời) — giới hạn MUST là **một lần cho mỗi phán quyết *Huỷ kết quả***: mỗi lần một tín hiệu đã xác nhận bị Huỷ kết quả, admin MUST kích hoạt được một tín hiệu còn hiệu lực khác, và chuỗi này MUST chỉ dừng khi hàng đợi tín hiệu Chướng ngại vật của vòng đã cạn. Hệ thống MUST NOT áp trần tổng số lần trong một vòng VCNV (§Clarifications 2026-07-30). *(US-005, US-009 · PRD-REQ-044, PRD-REQ-045 · GR-012 §Đồng thời, GR-032 §Vòng đời tín hiệu, GR-009 · AC-044e)*
- **FR-034e**: **Tập ứng viên** của thao tác kích hoạt tay MUST gồm đúng hai nhóm: **(a)** ghế có tín hiệu **trơ chưa từng được xử lý** *(bấm chuông nhưng thua tốc độ, hoặc đang chờ duyệt trong hàng đợi chặn của VCNV)*; **(b)** ghế **vừa bị chấm Huỷ kết quả** ở chính câu đó *(hoặc chính vòng đó ở VCNV)* — nghĩa là admin MUST đưa lại được chính người vừa bị huỷ, cho họ lượt thứ hai trên cùng một câu. Hai nhóm sau MUST NOT là ứng viên: ghế **bị loại** khỏi vòng VCNV *(bị loại là tuyệt đối trong phạm vi vòng — `GR-010`, `INV-019`)* và ghế đang bị **vô hiệu hoá** *(admin MUST kích hoạt lại ghế trước đã — `GR-036` C5)*. Khi admin chọn một ứng viên **lệch** khuyến nghị, hệ thống MUST hiện **dialog cảnh báo lệch luật** hạng Yes/No nêu rõ ghế bị vượt và thứ tự gốc, MUST cho ép qua, và MUST NOT bắt nhập lý do — đây là **hạng 2** của `PRD §9.4`, cùng khuôn với `GR-016` C5, **không** phải hạng phá huỷ (§Clarifications 2026-07-30). *(US-005, US-009 · PRD-REQ-045, PRD-REQ-046 · GR-010, GR-016 C5, GR-036 C5, INV-019, INV-020, PRD §9.4 · AC-044f, AC-044g)*
- **FR-035**: Admin bấm **No** cho một tín hiệu MUST đưa tín hiệu kế tiếp lên, MUST giữ nguyên lượt của ghế bị từ chối, và MUST NOT để lại tác dụng phụ nào: ô chữ chưa đánh dấu, câu **trả lại kho** vì chưa hiển thị, đồng hồ chưa chạy. Ở mode nhập liệu, nút chọn của ghế đó MUST mở lại. *(US-005, US-009 · PRD-REQ-046 · GR-032 C3, GR-007 C2, GR-009 C8, INV-008 · AC-043)*
- **FR-036**: Ngoài cửa sổ hợp lệ của một sự kiện, máy thí sinh MUST NOT render nút tương ứng và thao tác MUST không phản hồi ⇒ **không tín hiệu nào được tạo**. Server MUST vẫn từ chối một tín hiệu như vậy nếu nó lọt tới, và MUST ghi nhật ký thao tác. *(US-005 · PRD-REQ-044 · GR-032 §Điều kiện, GR-024 C1, GR-034 C6 · AC-046, AC-050)*
- **FR-037**: Thao tác từ chối MUST idempotent: một tín hiệu đã bị từ chối MUST NOT bị từ chối lần nữa và MUST NOT đổi gì. *(US-005 · PRD-REQ-044 · GR-032 §Bấm trùng · AC-050)*
- **FR-038**: Một tín hiệu **không giành được quyền** MUST NOT làm gián đoạn đồng hồ; thứ bị hoãn MUST chỉ là việc **hiển thị**, MUST NOT là thời gian. *(US-005, US-009 · PRD-REQ-044 · GR-008 C9, INV-013 · AC-114)*

#### Nhóm F — Rút đề và không lặp câu (US-006)

- **FR-039**: Server MUST rút câu **ngẫu nhiên trong danh sách đã gán** của contest, MUST loại những câu đã dùng, và MUST phát một sự kiện rút đề để phát lại được đúng thứ tự câu cũ. Hệ thống MUST NOT tự lấy đề ngoài danh sách đã gán. *(US-006 · PRD-REQ-047 · GR-031 C1, TERM-047 · AC-052)*
- **FR-040**: Ranh giới *"đã dùng"* MUST là **đã hiển thị cho thí sinh** — MUST NOT là *"đã chấm"* và MUST NOT là *"đã rút"*. Câu **bị bỏ qua sau khi đã hiển thị** MUST vẫn tính là đã dùng. *(US-006 · PRD-REQ-047 · GR-031 §Ranh giới, GR-005 C1, INV-011 · AC-057, AC-060)*
- **FR-041**: Câu **đã rút nhưng chưa hiển thị** MUST được trả lại kho khi bị gỡ, khi tín hiệu tương ứng bị từ chối, hoặc khi một hàng ngang không bao giờ được chọn tới lúc vòng kết thúc. *(US-006, US-005, US-009 · PRD-REQ-047 · GR-031 C7, GR-007 C2, GR-012 C4 · AC-059, AC-137)*
- **FR-042**: Phạm vi quy tắc không-lặp-câu MUST là **toàn contest, xuyên mọi trận**; cờ đã-dùng MUST NOT bị đặt lại kể cả khi bỏ vòng hay chạy lại vòng. *(US-006 · PRD-REQ-047 · GR-031 §Điều kiện, GR-030 · AC-053, AC-060)*
- **FR-043**: Hệ thống MUST kiểm đủ câu tại cửa vào **từng vòng**, MUST NOT kiểm một lần lúc bắt đầu trận. Vòng thiếu câu MUST không mở được — đây là một trong đúng **ba** chỗ chặn cứng — trong khi các vòng khác MUST vẫn mở bình thường. *(US-006 · PRD-REQ-048 · GR-031 C3, INV-014 · AC-054)*
- **FR-044**: Số câu của một vòng MUST là con số cố định của luật (lượt riêng **6** mỗi thí sinh · lượt chung **12** · VCNV **4** hàng ngang + **1** ô trung tâm · Tăng tốc **4** · Về đích **3** mỗi gói · Câu hỏi phụ **3**). Hệ thống MUST NOT tự sinh câu thứ N+1, và tình huống *"kho đề cạn giữa vòng"* MUST NOT tồn tại. *(US-006 · PRD-REQ-048 · bảng §Số câu theo luật, GR-031 C4, INV-012 · AC-057)*
- **FR-045**: Đơn vị chọn tay và đơn vị rút của vòng Vượt chướng ngại vật MUST là một **bộ** gồm **1 Chướng ngại vật** *(từ khoá + hình ảnh 5 miếng ghép)*, **4 hàng ngang** và **1 câu ô trung tâm**. Bốn hàng ngang MUST mang số thứ tự cố định ứng với một miếng ghép ở một góc cố định, và MUST NOT hoán đổi được giữa các bộ. Một bộ MUST chỉ khả dụng khi **cả sáu thành phần đều chưa dùng**. *(US-006, US-009 · PRD-REQ-092 · GR-031 §Ngoại lệ, TERM-058 · AC-056)*
- **FR-046**: Phép kiểm kho của vòng Vượt chướng ngại vật MUST đếm **bộ nguyên vẹn**, MUST NOT đếm số câu; đủ số câu MUST NOT là điều kiện đủ để mở vòng. *(US-006, US-009 · PRD-REQ-092 · GR-031 C3b · AC-055)*
- **FR-047**: Khi admin chỉ định một hàng ngang, câu ô trung tâm, hoặc Chướng ngại vật làm câu Câu hỏi phụ, hệ thống MUST cảnh báo rằng thao tác đó **làm vỡ một bộ** và MUST nêu rõ bộ nào. Thứ tự ưu tiên rút tự động của Câu hỏi phụ MUST xếp kho Vượt chướng ngại vật **cuối cùng**. *(US-006, US-012 · PRD-REQ-093 · GR-031 C3c, GR-023 §Nguồn đề · AC-058)*
- **FR-048**: Trong một trận `practice` chạy bên trong một contest **thật**, phép lọc kho đề MUST đảo chiều: kho khả dụng MUST chỉ gồm những câu **đã dùng** ở các trận thật trước đó. Trận `practice` MUST NOT tiêu thêm câu nào và MUST NOT thấy được đề chưa thi. *(US-006 · PRD-REQ-047 · GR-031 C9 · AC-062)*
- **FR-049**: Thao tác rút đề MUST NOT dedup — mỗi lần rút MUST sinh một sự kiện riêng — và điều đó MUST an toàn vì câu của lần rút trước chưa hiển thị nên chưa tiêu và được trả lại kho. *(US-006 · PRD-REQ-047 · GR-031 §Bấm trùng · AC-063)*
- **FR-050**: Câu đã được đặt chỗ làm Câu hỏi phụ MUST bị loại khỏi phép rút của các vòng mở **sau thời điểm đặt chỗ**, và phép kiểm kho của những vòng đó MUST phản ánh chi phí đó. *(US-006, US-012 · PRD-REQ-093 · GR-023 §Nguồn đề, GR-031 · AC-058)*

#### Nhóm G — Phạm vi hiển thị đáp án (US-007)

- **FR-051**: **Trước** mốc **câu khép**, đáp án chuẩn và tiêu chí đạt của câu thực hành MUST chỉ rời server tới phiên đang giữ permission **đọc đáp án của câu đang chạy**; mỗi lần xem MUST ghi vào nhật ký thao tác. *(US-007 · PRD-REQ-049 · GR-037 C1, C2, INV-017 · AC-064, AC-065, AC-074)*
- **FR-052**: Cửa kiểm quyền xem đáp án MUST hỏi **permission**, MUST NOT hỏi tên vai; hệ thống MUST NOT hard-code danh sách vai ở hàng rào này. *(US-007 · PRD-REQ-049 · GR-037 §ghi chú bước (2), QĐ-094 · AC-073)*
- **FR-053**: **Từ** mốc câu khép trở đi, server MUST đẩy đáp án tới **thí sinh, khán giả và lớp phủ dựng stream** nếu cờ *hiện đáp án sau khi chấm* đang **bật**; nếu cờ **tắt**, server MUST NOT đẩy tới ba kênh đó ở bất kỳ thời điểm nào. *(US-007 · PRD-REQ-049 · GR-037 C3, C5 · AC-066, AC-068)*
- **FR-054**: Cờ *hiện đáp án sau khi chấm* MUST ở **cấp TRẬN**, MUST mặc định **BẬT** cho cả trận chính thức lẫn trận luyện tập, MUST đổi được cho từng trận, và MUST được chụp vào trận tại cú bấm bắt đầu trận. *(US-007 · PRD-REQ-049 · GR-037 §Điều kiện, QĐ-062, QĐ-080 · AC-066, AC-068)*
- **FR-055**: Mốc **câu khép** MUST xác định theo vòng: Khởi động lượt riêng, câu hàng ngang VCNV, câu ô trung tâm VCNV và Tăng tốc MUST khép tại cú bấm chấm; Khởi động lượt chung MUST khép tại cú bấm chấm **hoặc** khi hết cửa sổ chuông 3 giây không ai bấm; **Về đích** MUST khép khi cửa sổ cướp quyền đã đóng **và** người cướp đã được chấm, hoặc khi hết 5 giây không ai bấm, hoặc khi người thi chính được chấm **Đúng**. *(US-007, US-011 · PRD-REQ-088 · GR-037 §bảng Mốc câu khép, TERM-057 · AC-069, AC-070)*
- **FR-055a**: Khi admin **kích hoạt tay** một tín hiệu kế tiếp theo FR-034a, mốc **câu khép** MUST lùi tới sau khi **người được kích hoạt đã được chấm**, hoặc cửa sổ vừa cấp lại cho người đó đã đóng mà admin đã chốt câu. Trong khoảng từ cú bấm *Huỷ kết quả* tới mốc mới đó, server MUST NOT đẩy đáp án tới thí sinh, khán giả hay lớp phủ — cùng lập luận với `FR-056`: công bố sớm sẽ xoá cơ hội của người được kích hoạt. *(US-007, US-005 · PRD-REQ-088, PRD-REQ-045 · GR-037 C6, C8, GR-032 C4 · AC-044c)*
- **FR-056**: Ở **Về đích**, hệ thống MUST NOT công bố đáp án trong lúc cửa sổ cướp quyền còn mở, trên bất kỳ kênh nào của thí sinh, khán giả hay lớp phủ. *(US-007, US-011 · PRD-REQ-088 · GR-037 C6, GR-020 §Cấm · AC-069)*
- **FR-057**: Ba ca biên của mốc công bố MUST xử lý như sau: câu **bị bỏ qua** MUST vẫn công bố · phán quyết **Huỷ kết quả** MUST NOT tự công bố *(admin vẫn mở tay được)* · đáp án **Chướng ngại vật** MUST NOT theo cơ chế này mà theo quy tắc riêng của vòng VCNV. *(US-007 · PRD-REQ-088 · GR-037 C7, C8, C9, GR-012 · AC-067, AC-071, AC-075)*
- **FR-057a**: Quy tắc *"**Huỷ kết quả** không tự công bố"* MUST phát biểu theo **loại phán quyết**, MUST NOT theo vai bị chấm hay vị trí trong chuỗi câu. Ở **Về đích**, khi phán quyết **khép câu** là *Huỷ kết quả* cho **người cướp quyền**, server MUST NOT tự đẩy đáp án tới thí sinh, khán giả hay lớp phủ — dù câu đã khép theo `GR-037` C6 và cờ *hiện đáp án sau khi chấm* đang bật. Trong ca này `GR-037` **C8 thắng C6**, và đáp án MUST chỉ hiện qua thao tác **mở tay của admin** (§Clarifications 2026-07-30). *(US-007, US-011 · PRD-REQ-088 · GR-037 C6, C8, GR-020 · AC-071a)*
- **FR-058**: Server MUST chỉ gửi đáp án tại **đúng mốc câu khép**; hệ thống MUST NOT gửi đáp án xuống client sớm hơn rồi dựa vào một cờ hiển thị phía client để giấu nó. *(US-007 · PRD-REQ-089 · GR-037 §Cấm, INV-017 · AC-072)*
- **FR-059**: Yêu cầu xem đáp án từ một phiên không có quyền MUST bị server **im lặng** từ chối — MUST NOT trả đáp án và MUST NOT tiết lộ qua thông điệp lỗi. *(US-007 · PRD-REQ-049 · GR-037 §Điều kiện · AC-076)*
- **FR-060**: Tiêu chí đạt của câu thực hành MUST cùng mức bảo mật với đáp án chuẩn. Ghi chú dụng cụ MUST NOT ra tới khán giả hay thí sinh trong trận, nhưng MUST xem được trong kho đề để ban tổ chức chuẩn bị. *(US-007, US-011 · PRD-REQ-049 · GR-019 §Bảo mật · AC-164)*
- **FR-061**: Việc công bố đáp án MUST là **một chiều ở phía engine** — đã công bố thì engine MUST NOT tự thu lại — trong khi quyền đóng hiển thị **thủ công** của admin MUST giữ nguyên. Xem đáp án MUST NOT sinh sự kiện điểm nào. *(US-007 · PRD-REQ-049 · GR-037 §Bấm trùng, §Không đổi gì, QĐ-048 · AC-074)*

#### Nhóm H — Vòng Khởi động (US-008)

- **FR-062**: Ở **Khởi động lượt riêng**, lượt MUST khoá vào **một** thí sinh trước câu đầu tiên, số câu MUST là **6** cho mỗi thí sinh, thời gian suy nghĩ MUST là **3 giây** tính từ mốc start timer, và kết quả MUST là **+10** khi Đúng, **0** khi Sai. Pha này MUST NOT có hình phạt nào. *(US-008 · PRD §9.2 `STATE-002`, EPIC-006 §11 · GR-001 C1, C2 · AC-077, AC-078)*
- **FR-063**: *"Không trả lời"* và *"trả lời sai"* MUST là **cùng một thao tác** của admin ở mọi vòng; hệ thống MUST NOT phân biệt hai tình huống này bằng hai đường xử lý khác nhau. Hết giờ MUST NOT tự sinh kết quả. *(US-008, US-010, US-011 · PRD-REQ-036 · GR-002, GR-015 §Biên, GR-018 C3 · AC-081, AC-082)*
- **FR-064**: Ở **Khởi động lượt chung**, cửa sổ chuông MUST là **một khoảng liên tục** mở từ mốc admin bấm *hiển thị câu hỏi*, kéo qua thời gian MC đọc, và thêm **3 giây** sau mốc *start timer*. Bấm chuông trong lúc MC đang đọc MUST hợp lệ. *(US-008, US-004 · PRD §9.2 `STATE-003`, EPIC-006 §11 · GR-003 C1, C2 · AC-084)*
- **FR-065**: Thời gian suy nghĩ của người **giành được quyền** ở lượt chung MUST là **3 giây** tính **từ thời điểm giành quyền**, MUST NOT tính từ mốc admin bấm. *(US-008 · PRD §9.2, EPIC-006 §11 · GR-003 §Điều kiện · AC-084)*
- **FR-066**: Khi đã có người giành quyền, tín hiệu chuông tiếp theo trong cùng câu MUST thành **trơ**: được ghi kèm timestamp làm căn cứ cho admin, MUST NOT đổi người giữ quyền, và MUST NOT mở lại chuông. *(US-008, US-005 · PRD-REQ-044 · GR-003 C3 · AC-085)*
- **FR-067**: Ở **Khởi động lượt chung**, số câu MUST là **12**, và kết quả MUST là **+10** khi Đúng, **−5** khi Sai, **0** khi Huỷ kết quả. Giành quyền rồi im lặng hết 3 giây MUST cũng dẫn tới **−5** khi admin bấm Sai. Phán quyết **Huỷ kết quả** MUST NOT áp hình phạt cho ai. *(US-008 · PRD §9.2 `STATE-003`, EPIC-006 §11 · GR-004 C1 → C4 · AC-088 → AC-091)*
- **FR-068**: Khi một câu đã được chấm, hệ thống MUST khép câu: MUST xoá **hàng đợi đang hoạt động**, MUST gỡ khoá chuông của **mọi** ghế cho câu mới, và MUST NOT mở lại chuông cho câu vừa chấm. *(US-008, US-005 · PRD-REQ-044 · GR-004 C6, GR-032 · AC-092)*
- **FR-069**: Ở lượt chung, hết cửa sổ chuông **3 giây** mà **không** tín hiệu nào tới ⇒ câu MUST **bị bỏ qua**, MUST được đánh dấu **đã dùng**, và MUST NOT đổi điểm của bất kỳ ai. Admin MUST có thao tác **chuyển câu thủ công** để đóng cửa sổ sớm, với cùng hệ quả. *(US-008, US-006 · PRD-REQ-048 · GR-005 C1, C4 · AC-093, AC-095)*
- **FR-070**: Ở **mode nhập liệu**, hệ thống MUST ghi nhận **bản cuối cùng** trong các bản hợp lệ; nút gửi MUST NOT khoá sau khi gửi. Bản **rỗng sau khi cắt khoảng trắng** MUST bị bỏ qua và MUST NOT ghi đè bản hợp lệ đã có. Mọi bản gửi MUST được trim hai đầu. Lịch sử các bản đã gửi MUST NOT bị xoá — chỉ *bản được ghi nhận* thay đổi. *(US-008 · PRD-REQ-036, PRD-REQ-065 *(giao diện thuộc EPIC-008)* · GR-006 C1 → C7 · AC-096 → AC-100)*

#### Nhóm I — Vòng Vượt chướng ngại vật (US-009)

- **FR-071**: Mỗi thí sinh MUST có **tối đa một** lượt chọn hàng ngang, bắt đầu từ **vị trí số 1**. Thí sinh bị loại **trước khi** dùng lượt ⇒ lượt MUST dồn sang vị trí tiếp theo. Nếu vị trí cuối đã chọn xong mà **vẫn còn** hàng ngang chưa chọn ⇒ lượt MUST quay lại vị trí số 1. *(US-009 · PRD §9.2 `STATE-004`, EPIC-006 §11 · GR-007 C3, C4, GR-010 C2 · AC-103, AC-104)*
- **FR-072**: Chủ thể phát tín hiệu chọn hàng ngang MUST phụ thuộc mode và MUST có đúng **một** đường vào cho mỗi mode: mode **sân khấu** ⇒ chỉ **admin** click, máy thí sinh MUST NOT render nút chọn; mode **nhập liệu** ⇒ chỉ **thí sinh** click, admin MUST NOT chọn thay. Cả hai đường MUST đi qua hàng đợi chặn và admin xác nhận. *(US-009 · PRD-REQ-069 · GR-007 §Kích hoạt, C6, C7 · AC-105, AC-106)*
- **FR-073**: Câu **hàng ngang** MUST luôn trả lời bằng máy bất kể mode contest, với thời gian suy nghĩ lấy từ **giá trị cấu hình thời gian của vòng VCNV** *(preset `O26_DEFAULT@1` đặt mặc định **15 giây**; xem FR-083a)*. Kết quả MUST là **+10** cho **mỗi** người được chấm Đúng và **0** khi Sai, không trừ. Miếng ghép tương ứng MUST mở khi có **≥1** người đúng và MUST NOT mở khi không ai đúng; trạng thái *đã hỏi* của ô MUST NOT phụ thuộc việc miếng ghép có mở hay không. *(US-009 · PRD §9.2 `STATE-004`, EPIC-006 §11 · GR-008 C1 → C4 · AC-109 → AC-112)*
- **FR-074**: Hệ thống MUST NOT tự lộ bất kỳ thứ gì khi **hết giờ** câu hàng ngang — hết giờ MUST chỉ khoá ô nhập. Bài làm của từng thí sinh MUST chỉ hiện khi admin bấm hiển thị, ở mọi thời điểm. *(US-009, US-007 · PRD-REQ-049 · GR-008 §Điều kiện · AC-113)*
- **FR-075**: Thứ tự đánh giá của một câu hàng ngang MUST tất định theo ba bước: **(a)** chấm **toàn bộ** thí sinh → **(b)** xác định miếng ghép có mở không, là phép OR trên tập kết quả bước (a) → **(c)** đóng câu. *(US-009 · PRD §9.2, EPIC-006 §11 · GR-008 §Thứ tự đánh giá · AC-117)*
- **FR-076**: Băng điểm Chướng ngại vật MUST là **hàm** đọc theo **số hàng ngang không còn ở trạng thái chờ**, MUST NOT là bộ đếm cộng dồn: **1 hàng → 60**, **2 → 50**, **3 → 40**, **4 → 30**, và **20** sau khi gợi ý cuối đã đưa ra — **20 là sàn**. Ô trung tâm MUST NOT vào phép tính này. *(US-009 · PRD-REQ-058 · GR-009 C1 → C5, GR-011, TERM-053 · AC-118)*
- **FR-077**: Nút *"Mở chướng ngại vật"* MUST được xếp là **chuông**: MUST chỉ nhận click chuột, MUST tự khoá khi bấm, MUST NOT có dialog phía thí sinh, và mỗi ghế MUST phát tối đa **một** tín hiệu cho cả vòng. Hàng đợi cho tín hiệu này MUST **chặn**. *(US-009, US-005 · PRD-REQ-045, PRD-REQ-064 *(giao diện thuộc EPIC-008)* · GR-009 §Điều kiện, GR-034 · AC-118)*
- **FR-078**: Băng điểm MUST chốt theo trạng thái bàn cờ tại **mốc admin xác nhận tín hiệu**, và thứ tự đánh giá MUST tất định: **(1)** admin xác nhận tín hiệu → **(2)** chốt số hàng ngang đã hỏi → **(3)** admin phán quyết. *(US-009 · PRD-REQ-058 · GR-009 C7, §Thứ tự đánh giá · AC-119)*
- **FR-079**: Ba trạng thái của mỗi ô bàn cờ MUST là **chờ → đã hỏi → mở**, năm ô MUST độc lập, và admin MUST đặt thẳng một ô sang giá trị bất kỳ **cả hai chiều**. Thao tác đặt tay MUST NOT sinh điểm và MUST NOT tiêu câu. Đánh dấu một ô là **đã hỏi** bằng tay MUST làm băng tụt một bậc y như một lượt hỏi thật, và nhật ký thao tác MUST ghi rõ đây là thao tác **do admin**. Đặt một ô sang **mở** MUST NOT đổi băng, vì biến đếm là *đã hỏi* chứ không phải *đã lộ*. *(US-009 · PRD-REQ-058 · GR-009 C12, C13, TERM-053 · AC-121, AC-122)*
- **FR-080**: Ở vòng VCNV, điểm MUST chỉ đến từ đúng **ba** đường: câu hàng ngang được chấm Đúng · tín hiệu *Mở chướng ngại vật* được chấm Đúng · điều chỉnh điểm thủ công của admin. Thao tác mở/đóng bằng tay của admin MUST NOT tự sinh điểm cho ai. *(US-009 · PRD-REQ-058 · GR-009 §Không đổi gì, C15 · AC-124)*
- **FR-081**: Chấm **Sai** một tín hiệu giải Chướng ngại vật MUST đặt cờ **bị loại khỏi vòng** cho ghế đó và MUST NOT trừ điểm. Phạm vi *"bị loại"* MUST là **một vòng** — ghế đó MUST vẫn thi các vòng sau và vẫn có thể thắng trận. Điểm hàng ngang đã kiếm được của ghế bị loại MUST giữ nguyên. *(US-009 · PRD §9.2, EPIC-006 §11 · GR-010 C1, C3, INV-019 · AC-125, AC-127)*
- **FR-082**: Số lần đoán Chướng ngại vật của mỗi ghế MUST là **một**; hệ thống MUST NOT khoá quyền trả lời câu hàng ngang của ghế đó trong lúc tín hiệu còn **chờ duyệt**, vì khoá sớm nghiêm hơn luật và mâu thuẫn với quy tắc *"admin bấm No ⇒ không mất gì"*. *(US-009 · PRD-REQ-046 · GR-009 C14, GR-010 §Biên · AC-123)*
- **FR-083**: Gợi ý cuối ở ô trung tâm MUST kích hoạt khi **cả 4 hàng ngang đã được HỎI** — điều kiện MUST đọc theo *hàng ngang đã hỏi*, MUST NOT đọc theo *miếng ghép đã mở*, nên giai đoạn này MUST luôn tới được. Câu ô trung tâm MUST cho **+10** và mở ô khi Đúng, và MUST NOT mở ô khi Sai; băng điểm MUST là **20** trong cả hai trường hợp, không phụ thuộc kết quả câu ô trung tâm. Cửa sổ giải Chướng ngại vật sau gợi ý cuối MUST dùng cùng giá trị cấu hình ở FR-083a *(mặc định **15 giây**)*, cho **+20** khi đúng. *(US-009 · PRD §9.2, EPIC-006 §11 · GR-011 C1 → C4, C6 · AC-128 → AC-131)*
- **FR-083a**: Vòng Vượt chướng ngại vật MUST có **một** giá trị cấu hình thời gian dùng chung, và giá trị đó MUST chi phối **cả ba** cửa sổ của vòng: thời gian suy nghĩ **câu hàng ngang** · thời gian suy nghĩ **câu ô trung tâm** · **cửa sổ giải Chướng ngại vật sau gợi ý cuối**. Preset `O26_DEFAULT@1` MUST đặt mặc định **15 giây**; đổi giá trị ở contest builder MUST đổi cả ba cùng lúc. Người soạn đề MUST NOT khai thời lượng riêng cho câu ô trung tâm, và hệ thống MUST NOT hard-code con số 15 ở đường xử lý nào (§Clarifications 2026-07-30). *(US-009 · PRD §9.2 `STATE-004`, EPIC-006 §11, GOAL-002 · GR-008, GR-011 · `CLAUDE.md` §Quy ước khác · AC-131a)*
- **FR-084**: Câu ô trung tâm MUST mở cho **mọi thí sinh chưa bị loại**, MUST NOT theo lượt, và MUST luôn trả lời bằng máy bất kể mode contest. *(US-009 · PRD §9.2, EPIC-006 §11 · GR-011 C5 · AC-132)*
- **FR-085**: Thứ tự ba mốc cuối vòng VCNV MUST tất định vì cả ba đều là cú bấm của admin và nút sau chỉ bật khi nút trước đã bấm: **(1)** chấm câu ô trung tâm → **(2)** mở ô trung tâm → **(3)** mở cửa sổ 15 giây. Cửa sổ MUST đóng đúng 15 giây sau mốc (3), chung cho mọi thí sinh còn quyền. *(US-009 · PRD §9.2, EPIC-006 §11 · GR-011 §Thứ tự đánh giá · AC-131)*
- **FR-086**: Khi **thí sinh cuối cùng** chưa bị loại giải sai Chướng ngại vật, vòng MUST kết thúc và MUST NOT ai được điểm Chướng ngại vật. Việc mở toàn bộ miếng ghép và công bố Chướng ngại vật MUST là thao tác **thủ công và tuỳ chọn** của admin; không bấm công bố MUST NOT là trạng thái tắc. Hàng ngang **chưa được hỏi** MUST bị bỏ và câu của nó MUST **trả lại kho** vì chưa hiển thị cho ai. Điểm hàng ngang đã ghi của **mọi** thí sinh MUST giữ nguyên. *(US-009, US-006 · PRD §9.2, EPIC-006 §11 · GR-012 C1 → C4, C7, C8, INV-019 · AC-135 → AC-138)*

#### Nhóm J — Vòng Tăng tốc (US-010)

- **FR-087**: Ở **Tăng tốc**, số câu MUST là **4**, mọi câu MUST luôn gõ máy bất kể mode contest, và thang điểm MUST là **40 / 30 / 20 / 10** theo thứ hạng, tính **chỉ trên tập người được chấm ĐÚNG**. Người được chấm Sai MUST nhận **0** và MUST NOT giữ chỗ trong thang. *(US-010 · PRD §9.2 `STATE-005`, EPIC-006 §11 · GR-013 C1, C3, C4, C6 · AC-140, AC-141, AC-145)*
- **FR-088**: Mốc xếp hạng Tăng tốc MUST là **server-received timestamp của bản cuối hợp lệ**. Thứ tự đánh giá MUST tất định: **(1)** ghi nhận tập người được chấm Đúng → **(2)** sắp theo mốc, độ phân giải mili-giây → **(3)** gộp người cùng mốc thành **một bậc** → **(4)** gán điểm theo bậc, bậc kế **nhảy qua** đúng số người trong nhóm hoà → **(5)** phát **một** sự kiện điểm cho cả bảng tại cú bấm chốt câu. *(US-010 · PRD-REQ-040, PRD-REQ-042 · GR-013 §Thứ tự đánh giá, GR-014 · AC-142, AC-143)*
- **FR-089**: Ở Tăng tốc, hệ thống MUST NOT khoá ô nhập hay nút gửi sau lần trả lời đầu. Bản có nội dung **khác** bản trước MUST cập nhật cả nội dung lẫn mốc xếp hạng; bản có nội dung **y hệt** sau khi trim MUST NOT cập nhật mốc; bản **rỗng** sau khi trim MUST bị bỏ qua và MUST giữ bản hợp lệ trước đó. *(US-010 · PRD-REQ-066 · GR-015 C1 → C4 · AC-147 → AC-149)*
- **FR-090**: Khi một ghế có bản hợp lệ **và** bản quá hạn, bản hợp lệ cuối MUST dùng cho xếp hạng và bản quá hạn MUST tô đỏ cho admin quyết. Khi một ghế **chỉ** có bản quá hạn và admin công nhận nó, **cả bảng xếp hạng của câu** MUST được tính lại. *(US-010 · PRD-REQ-066, PRD-REQ-038 · GR-015 C5, C6 · AC-150)*

#### Nhóm K — Vòng Về đích (US-011)

- **FR-091**: Thứ tự lượt Về đích MUST tính lại **sau mỗi lượt hoàn thành**, theo hai tiêu chí: điểm cao nhất tại thời điểm xếp lượt đi trước; hoà điểm ⇒ **số vị trí nhỏ nhất** đi trước. Hệ thống MUST chỉ **khuyến nghị**; admin MUST ép được qua dialog cảnh báo và hệ thống MUST NOT chặn cứng. Bảng xếp MUST lấy **sau khi lượt trước kết thúc hoàn toàn**. *(US-011 · PRD §9.2 `STATE-006`, PRD-REQ-041 · GR-016 C1 → C5, INV-020 · AC-151, AC-152)*
- **FR-092**: Gói câu Về đích MUST gồm **đúng ba mục**, mỗi mục thuộc hai mức điểm cấu hình được *(preset O26: 20 và 30)*. Chủ thể chọn MUST phụ thuộc mode: **sân khấu** ⇒ **admin** bấm theo lời thí sinh, máy thí sinh MUST NOT render nút chọn gói; **nhập liệu** ⇒ **thí sinh** chọn, và admin MUST có fallback chọn hộ gói mặc định **20/20/20** khi thí sinh chưa chọn lúc tới lượt. Đổi gói MUST theo nguyên tắc bản-cuối-thắng cho tới **mốc admin bấm hiển thị câu đầu tiên**, sau đó MUST khoá. Server MUST validate lại số mục và mức điểm bất kể client là ai. *(US-011 · PRD-REQ-070 · GR-017 C1 → C8 · AC-153 → AC-157)*
- **FR-093**: Phép kiểm kho của vòng Về đích MUST tính theo **trường hợp xấu nhất**: cần `3 × số ghế` câu ở mức 20 **VÀ** `3 × số ghế` câu ở mức 30. Thiếu ⇒ vòng MUST không mở được. *(US-011, US-006 · PRD-REQ-048 · GR-017 §Biên · AC-158)*
- **FR-094**: **Người thi chính** MUST được tính theo **bản cuối cùng**. Chấm Đúng ⇒ **+giá trị câu** và MUST NOT mở cửa sổ cướp. Chấm Sai hoặc không trả lời ⇒ **0 điểm**, không trừ, và MUST mở **cửa sổ cướp 5 giây**. Thời gian suy nghĩ MUST lấy từ metadata **từng câu**, MUST NOT suy ra từ mức điểm. Chấm **sau khi** cửa sổ cướp đã đóng MUST vẫn thực hiện được. *(US-011 · PRD §9.2, EPIC-006 §11, PRD-REQ-042 · GR-018 C1 → C5, §Biên, TERM-045 · AC-159 → AC-162)*
- **FR-095**: Câu **thực hành** MUST chia timer thành **hai pha** — suy nghĩ rồi thực hành — ngăn cách bởi **một cú bấm riêng của admin**, là mốc thứ ba sau *hiển thị câu* và *start timer*; hệ thống MUST NOT gộp mốc này vào mốc khác. Admin MUST chấm **đạt / không đạt**; *"không đạt"* và *"không thực hành gì"* MUST là cùng một thao tác. Đạt ⇒ **+giá trị câu**; không đạt ⇒ **0** và mở cửa sổ cướp 5 giây. Người cướp quyền MUST cũng thực hành, trong thời lượng thực hành riêng của người cướp. Cơ chế chuẩn hoá và tô khác biệt ký tự MUST NOT áp ở câu thực hành, và hệ thống MUST NOT có nhánh nào tự kết luận *"đạt yêu cầu"* từ tiêu chí đạt. *(US-011 · PRD §9.2, EPIC-006 §11, PRD-REQ-036, PRD-REQ-043 · GR-019 C1 → C6, §Thứ tự đánh giá · AC-163 → AC-165)*
- **FR-096**: Cờ *câu thực hành* MUST chỉ hợp lệ với câu thuộc kho Về đích, và cấu hình sai MUST bị chặn ở **kho đề**, MUST NOT là lỗi lúc chạy. Ghi chú dụng cụ rỗng hoặc tiêu chí đạt rỗng MUST chỉ sinh **cảnh báo** ở cửa vào vòng, MUST NOT chặn cứng. *(US-011 · PRD §9.2, EPIC-006 §11 · GR-019 §Ngoại lệ và cảnh báo · AC-166)*
- **FR-097**: Cửa sổ **cướp quyền** MUST là **5 giây**, hàng đợi MUST **không chặn**, và server MUST phân xử ngay theo timestamp. Người cướp MUST được tính theo **bản ĐẦU TIÊN**. Cướp **đúng** ⇒ **transfer**: người thi chính **−giá trị câu**, người cướp **+giá trị câu**. Cướp **sai** ⇒ người cướp **−½ giá trị câu**, và người thi chính MUST NOT được hoàn lại. Người thi chính MUST NOT cướp câu của chính mình. Bấm ngoài cửa sổ MUST NOT tạo tín hiệu nào. *(US-011 · PRD §9.2, EPIC-006 §11, PRD-REQ-045 · GR-020 C1 → C4, §Biên · AC-167 → AC-170)*
- **FR-098**: Khi giá trị câu **lẻ**, hình phạt cướp sai MUST tính bằng **phép chia số nguyên làm tròn xuống theo ĐỘ LỚN** rồi mới gắn dấu âm — ví dụ giá trị 25 cho hình phạt **−12**, MUST NOT là −13. Hệ thống MUST NOT hard-code ràng buộc *"giá trị phải chẵn"* vào validation. *(US-011 · PRD §9.2, EPIC-006 §11 · GR-020 §Giá trị câu LẺ · AC-171)*
- **FR-099**: **Ngôi sao hy vọng** MUST dùng được **một lần cho mỗi thí sinh trong một LẦN CHẠY vòng Về đích**; bỏ hoặc chạy lại vòng MUST đặt lại cờ này. Cửa sổ đặt MUST đóng tại **mốc admin bấm hiển thị câu hỏi**; ngoài cửa sổ nút MUST NOT render và ngôi sao MUST vẫn tính là **chưa dùng**. Chủ thể bấm MUST phụ thuộc mode: **sân khấu** ⇒ admin bấm theo lời thí sinh qua dialog Yes/No, máy thí sinh MUST NOT render nút; **nhập liệu** ⇒ thí sinh tự bấm và tín hiệu MUST có hiệu lực ngay, không chờ duyệt. *(US-011, US-004 · PRD-REQ-069 · GR-021 §Điều kiện, C3, C6, C7, C8 · AC-172 → AC-175, AC-177)*
- **FR-100**: Câu có Ngôi sao hy vọng MUST cho **gấp đôi giá trị câu** khi Đúng và **−giá trị câu** khi Sai. Phần mất của người thi chính MUST là **đúng một lần** và MUST độc lập với hành động của người cướp — ba nhánh *(cướp đúng, cướp sai, không ai cướp)* MUST cho người thi chính **cùng một** kết quả. Người cướp quyền MUST NOT dùng được Ngôi sao hy vọng trên câu đang cướp. *(US-011 · PRD §9.2, EPIC-006 §11 · GR-021 C1, C2, C5, C5b, C5c, GR-018 C6, GR-020 C5 · AC-176, AC-177)*

#### Nhóm L — Vòng Câu hỏi phụ (US-012)

- **FR-101**: Vòng Câu hỏi phụ MUST gồm **3 câu**, mỗi câu **15 giây** suy nghĩ; cả hai con số MUST cố định và MUST NOT cấu hình được. Vòng này MUST NOT cộng hay trừ điểm của bất kỳ ai; kết quả MUST chỉ đổi **thứ hạng** bên trong nhóm bằng điểm, và MUST NOT đảo được thứ tự hai người khác điểm. *(US-012 · PRD-REQ-090 · GR-023 §Điều kiện, TERM-020 · AC-178, AC-184)*
- **FR-102**: Khi một thí sinh **giành được quyền** ở Câu hỏi phụ, đồng hồ 15 giây MUST **dừng ngay**. Hệ thống MUST NOT có một đồng hồ trả lời riêng sau khi giành quyền, và MUST NOT có chế tài cho việc bấm chuông rồi im lặng — admin chấm **Sai** và cả nhóm sang câu kế. *(US-012 · PRD-REQ-090 · GR-023 §Điều kiện, C2 · AC-179, AC-183)*
- **FR-103**: Kết quả một câu Câu hỏi phụ MUST rẽ theo ba nhánh: chấm **Đúng** ⇒ người đó thắng phân định, vòng đóng và trận về trạng thái nghỉ kèm một sự kiện phân định ghi phương thức *trả lời*; chấm **Sai** ⇒ hàng đợi reset và **cả nhóm** sang câu kế; hết 15 giây **không ai bấm** ⇒ sang câu kế, không ai đúng. *(US-012 · PRD-REQ-090 · GR-023 C1, C2, C3 · AC-180 → AC-182)*
- **FR-104**: Nút chuông ở Câu hỏi phụ MUST NOT sống trước mốc admin bấm *start timer* ⇒ tín hiệu bấm sớm MUST NOT tồn tại. Server MUST vẫn từ chối mọi tín hiệu chuông tới trước mốc đó ở vòng này và MUST ghi nhật ký thao tác. Quy tắc này MUST NOT áp cho Khởi động lượt chung, nơi chuông sống từ mốc **hiển thị câu**. *(US-012, US-005 · PRD-REQ-090 · GR-024 C1 → C3 · AC-185, AC-186)*
- **FR-105**: Vòng Câu hỏi phụ MUST NOT yêu cầu một kho khai riêng. Đề MUST rút từ **ba kho nguồn** — Về đích, Khởi động, Vượt chướng ngại vật — theo đúng thứ tự ưu tiên đó. Kho **Tăng tốc** MUST NOT là nguồn. Câu **thực hành** MUST bị loại khỏi phép rút. *(US-012 · PRD-REQ-090 · GR-023 §Nguồn đề · AC-187, AC-188)*
- **FR-105a**: Trong một trận `practice` chạy bên trong một contest **thật**, thứ tự áp phép lọc cho vòng Câu hỏi phụ MUST là **hai bước cố định**: **(1)** dựng kho khả dụng theo **phép đảo** của FR-048 — **chỉ** những câu đã lộ ở các trận thật trước đó; **(2)** trên kho đó mới áp phép rút của FR-105 — ba kho nguồn theo thứ tự ưu tiên, loại kho Tăng tốc, loại câu thực hành. Phép kiểm *"đủ 3 câu khả dụng"* của FR-107 MUST đếm trên **tập đã lộ**. Hệ thống MUST NOT bù bằng câu chưa lộ trong bất kỳ hoàn cảnh nào — trận `practice` MUST NOT thấy được đề chưa thi ở **mọi** vòng, không có ngoại lệ cho vòng phân định (§Clarifications 2026-07-30). *(US-012, US-006 · PRD-REQ-047, PRD-REQ-090 · GR-031 C9, GR-023 §Nguồn đề · AC-190a)*
- **FR-106**: Metadata của kho gốc MUST bị vô hiệu hoá với câu mượn: thời lượng suy nghĩ MUST luôn là **15 giây** bất kể giá trị khai ở câu gốc, và mức điểm MUST bị bỏ qua vì vòng này không sinh điểm. *(US-012 · PRD-REQ-090 · GR-023 §Nguồn đề · AC-189)*
- **FR-107**: Cửa vào vòng Câu hỏi phụ MUST kiểm đủ **3 câu khả dụng** từ ba kho nguồn; thiếu ⇒ vòng MUST không mở được. Tình huống cạn đề giữa vòng MUST NOT tồn tại. *(US-012, US-006 · PRD-REQ-090, PRD-REQ-048 · GR-025 C4, GR-022 §Điều kiện · AC-190)*
- **FR-108**: Khi hết **3 câu** mà chưa ai đúng, server MUST đề xuất **một** người bằng bốc thăm ngẫu nhiên và MUST trình kết quả cho admin dưới dạng **đề xuất**. Admin bấm **Yes** ⇒ người đó nhận vị trí đang tranh và hệ thống MUST sinh sự kiện phân định ghi phương thức *bốc thăm*. Admin bấm **Bốc lại** ⇒ bốc lần nữa; **cả hai lần MUST là sự kiện thật** trong nhật ký append-only, **lần cuối cùng có hiệu lực**, và lần trước MUST NOT bị đánh dấu vô hiệu. *(US-012 · PRD-REQ-090 · GR-025 C1 → C3 · AC-191 → AC-193)*
- **FR-109**: Khi admin **không** phân định một nhóm hoà, các thành viên nhóm đó MUST được ghi **đồng hạng** theo quy tắc xếp hạng cạnh tranh chuẩn, và hạng kế tiếp MUST nhảy qua đúng số người đồng hạng. *(US-012 · PRD-REQ-090, PRD-REQ-105 · GR-025 C5, GR-022 C4 · AC-194)*

#### Nhóm M — Mất kết nối và giữ ghế (US-013)

- **FR-110**: Ghế mất kết nối MUST được giữ trong một ngưỡng chờ mặc định **120 giây**, đo bằng **đồng hồ server**, với biên **đóng** — đúng `120.000` giây vẫn trong ngưỡng. Ghế quay lại trong ngưỡng MUST được khôi phục trạng thái kèm thông báo đã kết nối lại. *(US-013 · PRD-REQ-050 · GR-036 C1, §Biên, TERM-043 · AC-196, AC-204)*
- **FR-111**: Quá ngưỡng, hệ thống MUST **chỉ tô nổi bật** ghế đó trên màn admin kèm thời lượng đã mất kết nối. Hệ thống MUST NOT tự loại, MUST NOT tự xoá, và MUST NOT tự vô hiệu hoá ghế nào. Admin MUST có hai lựa chọn: **giữ** hoặc **gia hạn**. Admin MUST can thiệp được cả **trước** khi hết ngưỡng. *(US-013 · PRD-REQ-050 · GR-036 C2, C3, C9, NON-GOAL-010 · AC-197, AC-198, AC-204)*
- **FR-112**: Mỗi lần mất kết nối MUST mở một cửa sổ chờ mới tính lại từ đầu — hệ thống MUST NOT cộng dồn thời gian các lần trước và MUST NOT giới hạn số lần. Hai ghế cùng mất kết nối MUST có hai cửa sổ **độc lập**. Mọi lần mất và nối lại MUST vào lịch sử và MUST NOT bị xoá. *(US-013 · PRD-REQ-050 · GR-036 C10, §Nối lại nhiều lần · AC-205)*
- **FR-113**: Đồng hồ MUST NOT dừng vì một ghế mất kết nối; vòng MUST chạy tiếp bình thường. *(US-013 · PRD-REQ-050 · GR-036 C4, INV-016 · AC-199)*
- **FR-114**: Gói khôi phục cho một ghế quay lại giữa câu MUST chứa: vòng và giai đoạn hiện tại · câu đang mở · **hạn chót theo server time** · bản gửi gần nhất **của chính ghế đó** · các cờ ghế còn hiệu lực · trạng thái chuông theo luật · bàn cờ VCNV · điểm công khai · lớp phủ đang bật. Gói khôi phục MUST NOT chứa đáp án chuẩn, MUST NOT chứa bài làm của ghế khác, và MUST NOT sinh sự kiện nào. Đồng hồ MUST NOT được đặt lại — mất kết nối MUST NOT mua thêm thời gian. *(US-013, US-007 · PRD-REQ-050, PRD-REQ-049 · GR-036 C8, INV-017 · AC-200, AC-207)*
- **FR-115**: **Vô hiệu hoá** một ghế MUST là quyết định **riêng** của admin, MUST NOT là hệ quả tự động của mất kết nối. Thao tác này MUST đảo ngược được và MUST làm được ở **mọi lúc** trong một trận chưa đóng sổ, kể cả giữa một câu đang mở. Ghế bị vô hiệu hoá MUST ở lại trong trận với nguyên điểm và nguyên vị trí, MUST vẫn trên mọi bảng, và MUST chỉ mất quyền thao tác; tín hiệu lỡ tới MUST vào lịch sử ở trạng thái **trơ**. **Kích hoạt lại** MUST NOT hoàn nguyên gì. *(US-013 · PRD-REQ-050, PRD-REQ-061 *(giao diện thuộc EPIC-007)* · GR-036 C5, C6 · AC-201, AC-202, AC-203)*
- **FR-116**: v1 MUST NOT có thao tác kick thí sinh khỏi trận. Ngưỡng chờ MUST chỉ chi phối quyền thao tác **trong trận**; hết ngưỡng sau khi trận đã kết thúc MUST vẫn giữ ghế. *(US-013 · PRD-REQ-050 · GR-036 C11, NON-GOAL-009 · AC-206, AC-208)*

#### Nhóm N — Xuyên suốt

- **FR-117**: Mọi luật, cửa kiểm, khoá và phép validate mô tả ở các FR trên MUST được **server thực thi**, bất kể yêu cầu đến từ giao diện nào hay gọi thẳng API/socket. Giao diện MUST chỉ là lớp tăng cường trải nghiệm (ẩn nút, khoá nút, báo lỗi sớm) và MUST NOT là hàng rào duy nhất. *(US-001 → US-013 · `CLAUDE.md` §Nguyên tắc code — zero-trust · AC-006, AC-050, AC-072, AC-155)*
- **FR-118**: Mọi con số của luật — thời gian, số câu, mức điểm, băng điểm, ngưỡng chờ — MUST đọc từ cấu hình luật của trận, MUST NOT hard-code trong đường xử lý. Ngoại lệ là những con số mà nguồn khai rõ là **cố định, không cấu hình**: 3 câu × 15 giây của Câu hỏi phụ, và hệ số gấp đôi cùng hình phạt của Ngôi sao hy vọng. *(US-008 → US-013 · `CLAUDE.md` §Quy ước khác · GR-023 §Biên, GR-021 §Biên · AC-178, AC-172)*
- **FR-119**: Mọi bài làm dạng chữ MUST được cắt khoảng trắng hai đầu ở **cả** phía giao diện lẫn phía server trước khi lưu và trước khi đối chiếu. *(US-001, US-008, US-010 · `CLAUDE.md` §Quy ước khác · GR-006, GR-015, GR-027 · AC-010, AC-100)*

### Key Entities

- **Sự kiện điểm**: Một bản ghi append-only mô tả một phán quyết hoặc một điều chỉnh làm đổi điểm. Ở mọi vòng trừ Tăng tốc, nó ứng với **một** thí sinh và chống trùng theo *(câu, thí sinh, loại phán quyết)*; ở Tăng tốc, nó ứng với **cả bảng** của một câu. *(TERM-022)*
- **Nhật ký sự kiện của trận**: Chuỗi linear, chỉ thêm, không bao giờ xoá — nguồn duy nhất để tính điểm và để dựng lại trạng thái trận tại bất kỳ mốc thời gian nào. *(INV-001, INV-002, INV-022)*
- **Tín hiệu**: Một hành động của thí sinh đã tới server — bấm chuông, chọn hàng ngang, bấm *Mở chướng ngại vật*, gửi đáp án. Mang server timestamp, có đúng một outcome *(thực thi / bị từ chối / trơ)*, gắn với một **đích** *(lượt chọn · câu · vòng)*, và vào lịch sử vĩnh viễn. *(TERM-026, TERM-027, STATE-029 → STATE-032)*
- **Hàng đợi đang hoạt động**: Tập tín hiệu còn hiệu lực với đích chưa đóng, xử lý FIFO thuần theo server timestamp; đặt lại khi đích đóng. Khác hẳn **lịch sử tín hiệu**, thứ không bao giờ bị xoá. *(TERM-026)*
- **Câu khép**: Mốc mà không còn ai được trả lời câu đó nữa — mốc công bố đáp án. Trùng cú bấm chấm ở mọi vòng **trừ Về đích**. *(TERM-057)*
- **Bộ Vượt chướng ngại vật**: Đơn vị chọn và rút của vòng VCNV — 1 Chướng ngại vật + 4 hàng ngang mang số thứ tự cố định + 1 câu ô trung tâm; chỉ khả dụng khi cả sáu thành phần chưa dùng. *(TERM-058)*
- **Băng điểm Chướng ngại vật**: Giá trị hiện tại của Chướng ngại vật, là **hàm** đọc theo số hàng ngang không còn ở trạng thái chờ, không phải bộ đếm cộng dồn. *(TERM-053)*
- **Cờ đã-dùng của câu**: Dấu vết phạm vi **contest**, đặt tại mốc câu **hiển thị cho thí sinh**, không bao giờ đặt lại. *(TERM-048)*
- **Thứ hạng tốc độ**: Quan hệ thứ tự giữa những người được chấm Đúng của một câu Tăng tốc, tính trên mốc server nhận của bản cuối hợp lệ, với quy tắc gộp bậc và nhảy bậc. *(TERM-037)*
- **Ngưỡng chờ kết nối**: Cửa sổ 120 giây mặc định mở tại thời điểm phát hiện một ghế mất kết nối; quá ngưỡng chỉ tô nổi bật, không sinh hệ quả nào. *(TERM-043)*

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: **0** sự kiện điểm trong toàn bộ nhật ký của một trận được sinh mà không có một cú bấm phán quyết của người đứng trước nó — kiểm bằng cách đối chiếu từng sự kiện điểm với một thao tác của admin trong nhật ký thao tác.
- **SC-002**: 100% giá trị điểm hiển thị trên mọi bảng bằng đúng kết quả tính lại từ nhật ký sự kiện, tại mọi thời điểm trong trận và sau mọi thao tác hoàn nguyên.
- **SC-003**: Bộ kiểm *"không rò đáp án"* **pass 100%** cho **cả ba** kênh thí sinh, khán giả và lớp phủ, ở **mọi** thời điểm trước mốc câu khép — bao gồm gói khôi phục kết nối và lớp công bố kết quả. Xác minh bằng bắt gói tin và soi bộ nhớ máy client: **0** lần xuất hiện đáp án của câu đang mở.
- **SC-004**: 100% tín hiệu đã tới server xuất hiện trong lịch sử tín hiệu kèm server timestamp và một outcome xác định; **0** tín hiệu biến mất không dấu vết.
- **SC-005**: **0** câu bị hỏi lần thứ hai trong phạm vi một contest, xuyên mọi trận — kiểm bằng cách đối chiếu tập câu đã hiển thị của mọi trận trong contest.
- **SC-006**: **0** trận rơi vào trạng thái tắc vì cạn đề giữa một vòng đang chạy — mọi trường hợp thiếu đề đều bị bắt tại cửa vào vòng.
- **SC-007**: Hai tín hiệu lệch nhau **1 mili-giây** phân định được ở 100% số lần thử; tín hiệu tới **đúng** mốc hạn được tính là hợp lệ ở 100% số lần thử.
- **SC-008**: 100% số lần bấm phán quyết lần thứ hai cho cùng một câu đều bị từ chối và **0** sự kiện được sinh từ chúng, xác minh cả qua giao diện lẫn gọi thẳng API.
- **SC-009**: 100% ghế quay lại trong ngưỡng chờ dựng lại đúng màn đang thi với thời gian còn lại khớp hạn chót theo server time — sai lệch cho phép là **0 giây** so với hạn chót ghi trong nhật ký.

## Edge Cases

*(Xem §User Scenarios & Testing → Edge Cases ở trên.)*

## Out of Scope

- **Vòng đời cấp trận**: cú bấm bắt đầu trận và việc đóng băng cấu hình, `LOBBY` là cửa vào/ra duy nhất, bốn cửa ra của một vòng, cú bấm Chốt trận và phép tìm nhóm hoà của `GR-022`, đóng sổ và huỷ trận, tạo trận mới, sửa danh sách câu ở `LOBBY`, **đặt chỗ câu Câu hỏi phụ** (`PRD-REQ-091`) — thuộc EPIC-005 (`specs/005-phong-thi-vong-doi-tran`).
- **Giao diện và thao tác điều khiển của admin**: màn chấm với tô khác biệt ký tự và lịch sử bài gửi (`PRD-REQ-051`), điều chỉnh điểm thủ công (`GR-029`), bỏ/chạy lại/kết thúc sớm vòng (`GR-030`), mở và đóng đáp án và ô chữ thủ công, màn hàng đợi, ba hạng cảnh báo giao diện, vô hiệu hoá/kích hoạt lại ghế ở tầng giao diện — thuộc EPIC-007. Spec này chỉ đặc tả **hệ quả phía engine** của những thao tác đó.
- **Giao diện máy thí sinh**: nút chuông chỉ nhận click chuột và tự khoá (`GR-034`, `PRD-REQ-064`), vòng đời ngược của nút gửi (`PRD-REQ-065`), hai bố cục VCNV theo mode, ba trạng thái nút chọn hàng ngang, bảng điểm realtime trên máy thí sinh, dialog xác nhận phía thí sinh, phím tắt và hành vi `Esc` — thuộc EPIC-008.
- **Trình diễn**: màn khán giả, lớp phủ 1920×1080, màn MC chữ lớn, lớp phủ công bố kết quả, chủ đề và âm thanh — thuộc EPIC-009.
- **Xác thực và phân quyền**: catalog permission, mô hình vai, một phiên giữ quyền điều khiển, giành quyền điều khiển khi phiên admin mất kết nối (`QĐ-093`) — thuộc EPIC-001. Spec này chỉ đặc tả rằng cửa kiểm đáp án **hỏi permission** (FR-052), không đặc tả permission được cấp ra sao.
- **Kho đề và bộ đề**: soạn câu, metadata, media, vòng duyệt `DRAFT` → `ACTIVE`, tìm kiếm và lọc, cờ hiển thị dẫn xuất — thuộc EPIC-002. Spec này chỉ đặc tả **cách engine rút** từ danh sách đã gán.
- **Contest builder**: nút *"Áp dụng luật 2026"*, cấu hình từng vòng, chọn mode trả lời cấp contest, gán ghế và vị trí, chọn danh sách câu **ban đầu**, khoá cứng `rowCount = 4` và `tieBreakPositions = [1]` ở đường ghi cấu hình — thuộc EPIC-004.
- **Nhập/xuất gói contest** (EPIC-003) · **sau trận: biên bản, thống kê** (EPIC-010) · **nhật ký thao tác chung và hạn lưu trữ** (EPIC-011).
- **Luật cho số ghế ≠ 4** (`NON-GOAL-012`), **thi đội** (`NON-GOAL-013`), **playlist tuỳ ý** (`NON-GOAL-011`), **mọi đường máy tự chấm** (`NON-GOAL-001`) — không thuộc v1 dưới bất kỳ epic nào.

## Open Questions

> **Cập nhật sau phiên làm rõ 2026-07-30**: ba marker `NEEDS CLARIFICATION` ban đầu — `OQ-001`, `OQ-002`, `OQ-003` — **đã được chủ dự án phân xử** và tích hợp vào các FR/AC tương ứng; chúng giữ lại ở đây dưới dạng **bản ghi đã đóng** để truy vết, không còn là câu hỏi treo. **Không còn marker `NEEDS CLARIFICATION` nào tồn đọng.**
>
> `OQ-006` — lỗ hổng truy nguyên của cụm **kích hoạt tay** — cũng **đã đóng**: quyết định được ghi thành **`QĐ-104`** và bốn tài liệu đặc tả đã cập nhật theo. **Không còn FR nào bị chặn ở khâu implement.**
>
> Còn lại đúng hai mục, cả hai **không chặn** gì: `OQ-004` và `OQ-005` là **missing source behavior nằm ngoài phạm vi v1**, nêu để phiên bản sau không lấp bằng suy diễn.

- **OQ-001 — ✅ ĐÃ PHÂN XỬ (§Clarifications 2026-07-30)**: **Thời lượng suy nghĩ của câu ô trung tâm Vượt chướng ngại vật**. Câu ô trung tâm dùng **cùng một giá trị cấu hình** với câu hàng ngang và với cửa sổ giải Chướng ngại vật sau gợi ý cuối — một giá trị duy nhất cấp vòng VCNV, preset đặt mặc định **15 giây**, **cấu hình được**. Người soạn đề không khai thời lượng riêng cho câu ô trung tâm. Đã tích hợp vào **FR-083a** *(và FR-073, FR-083 đọc theo nó)* cùng **AC-131a**. *(GR-011 · `CLAUDE.md` §Quy ước khác)*

- **OQ-006 — ✅ ĐÃ ĐÓNG (2026-07-30)**: Cơ chế **admin kích hoạt tay** đã được ghi thành **`QĐ-104`** trong `docs/decisions.md` §E, và **năm** tài liệu đặc tả đã cập nhật theo: `GR-009` *(ca C6b + điều kiện ba lựa chọn)* · bảng §*Phán quyết có hai hay ba lựa chọn* *(dòng tín hiệu Chướng ngại vật)* · `GR-032` *(ca C8, C9 + mục §Kích hoạt tay)* · `GR-037` *(dòng bảng Mốc câu khép + ca C8b, và C8 nay nói rõ **C8 thắng C6**)* · `docs/game-state-machine.md` *(**`EVENT-052`** kích hoạt tay · **`T-097`** → **`T-100`** ở mục §I mới · **`INV-009`** viết lại thành *"một câu KHÉP một lần"* với đơn vị là bộ ba `(câu, thí sinh, loại phán quyết)`)* · `docs/traceability.md` §*Bổ sung ngoài O26 — LUÔN BẬT*. **Cổng 1 đã thoả** — `FR-034a` → `FR-034e`, `FR-055a` và `FR-009a` nay truy nguyên về `docs/`, không còn bị chặn ở khâu implement. Giữ lại mục này làm bản ghi đã đóng.

  <details><summary>Nội dung lỗ hổng trước khi đóng</summary>

  Cơ chế được chủ dự án chốt trong hai phiên làm rõ 2026-07-30 nhưng **chưa có mã `QĐ`**. `docs/game-rules.md` khi đó chỉ cấp **giấy phép chung** — *"hàng đợi vẫn ghi thứ tự để admin can thiệp khi có sự cố"* (`GR-032` C4, `PRD-REQ-045`) — chứ không mô tả cơ chế. Theo Nguyên tắc I và Cổng 1 của `.specify/memory/constitution.md`, một quyết định phát biểu trong chat **chưa thoả cổng truy nguyên**. Nội dung quyết định **đã đầy đủ để ghi** sau hai phiên làm rõ; thứ còn thiếu chỉ là **mã `QĐ` và dòng ghi vào sổ**. `QĐ` này phải phủ **năm** điểm, trong đó **hai điểm sửa thẳng vào tài liệu luật**:

| # | Nội dung | Vị thế so với `docs/` |
|---|---|---|
| 1 | Cơ chế **kích hoạt tay** một tín hiệu bất kỳ còn hiệu lực khi người giữ quyền bị *Huỷ kết quả* | **Mới** — `GR-032` C4 chỉ cấp giấy phép chung |
| 2 | Ba tham số: cấp lại trọn cửa sổ suy nghĩ · giới hạn theo **đích** *(một lần mỗi câu ở ba vòng chuông; một lần mỗi *Huỷ kết quả* ở VCNV)* · hình phạt y hệt người giành quyền bình thường | **Mới** |
| 3 | **Huỷ kết quả trở thành lựa chọn thứ ba** cho tín hiệu *"Mở chướng ngại vật"* | **Sửa `GR-009` và bảng §2.2** — hai nguồn này hiện chỉ có Đúng/Sai ở đó |
| 4 | Tập ứng viên *(tín hiệu trơ chưa xử lý + ghế vừa bị Huỷ kết quả; loại ghế bị loại và ghế vô hiệu hoá)*, và chọn lệch khuyến nghị ⇒ **cảnh báo hạng 2**, không bắt lý do | **Mới** |
| 5 | Mốc **câu khép lùi** tới sau khi người được kích hoạt đã được chấm | **Mở rộng `GR-037`** §bảng Mốc câu khép |

  Riêng **FR-009a** *(sửa dấu từng ghế tới mốc chốt câu)* chưa bao giờ thuộc diện này — nó là **cách đọc** `GR-013` §Bấm trùng và `GR-008` §Thứ tự đánh giá, không thêm hành vi mới.

  </details> Đề nghị ghi kèm bảng đối chiếu ba vòng chuông và ghi rõ đây là **bổ sung ngoài luật gốc O26** *(luật gốc chỉ có một người giành quyền cho mỗi câu)* để `docs/traceability.md` §Biến thể ngoài luật O26 phản ánh được. *(GR-032 C4 · PRD-REQ-045 · constitution §Cổng 1, Nguyên tắc I)*

- **OQ-002 — ✅ ĐÃ PHÂN XỬ (§Clarifications 2026-07-30)**: **Mốc câu khép ở Về đích khi người cướp quyền bị chấm *Huỷ kết quả***. `GR-037` **C8 thắng C6** — phán quyết *Huỷ kết quả* MUST NOT tự công bố ở bất kỳ vị trí nào của chuỗi Về đích, kể cả khi nó là phán quyết khép câu; đáp án chỉ hiện khi admin mở tay. Quy tắc phát biểu theo **loại phán quyết**, không theo vai bị chấm. Đã tích hợp vào **FR-057a** và **AC-071a**. *(GR-037 C6, C8 · GR-020 · PRD-REQ-088)*

- **OQ-003 — ✅ ĐÃ PHÂN XỬ (§Clarifications 2026-07-30)**: **Nguồn đề của Câu hỏi phụ trong một trận `practice` chạy bên trong một contest THẬT**. Phép đảo của `GR-031` C9 **thắng** và chạy **trước**: dựng kho chỉ-câu-đã-lộ, rồi mới áp thứ tự ưu tiên ba kho của `GR-023` lên kho đó; cửa vào vòng đếm 3 câu trên tập đã lộ, không bù bằng câu chưa lộ. Đã tích hợp vào **FR-105a** và **AC-190a**. *(GR-023 §Nguồn đề · GR-031 C9 · PRD-REQ-090)*

- **OQ-004 — missing source behavior, ngoài phạm vi v1**: **Băng điểm Chướng ngại vật cho `rowCount` 5-8**. `GR-009` C11 nói thẳng rằng nguồn **không có thang nào** cho số hàng ngang khác 4, và khi mở khoá ở phiên bản sau thì băng phải là **mảng cấu hình bắt buộc** dài bằng `rowCount`, không suy ra từ luật. v1 khoá cứng `rowCount = 4` (`PRD-REQ-025`) nên câu hỏi này **không chặn v1**; ghi lại để phiên bản sau không lấp bằng suy diễn. *(GR-009 C11 · PRD-REQ-025 · QĐ-068)*

- **OQ-005 — missing source behavior, ngoài phạm vi v1**: **Luật cho số ghế ≠ 4**. Thang điểm Tăng tốc (`GR-013` C5, `GR-014` §Biên), thứ tự lượt Về đích (`GR-016` §Biên), và mệnh đề *"3 thí sinh còn lại"* của cướp quyền (`GR-020` C6) đều chỉ được nguồn viết cho **đúng 4 người**. v1 chỉ hỗ trợ **lưu trữ và giao diện** cho 1-12, không có đường xử lý luật (`NON-GOAL-012`, `QĐ-007`), nên câu hỏi này **không chặn v1**. Riêng ca **số ghế hoạt động tụt dưới 4 giữa trận** đã có phân xử một phần — *admin quyết, hệ thống chỉ cảnh báo* (`GR-020` C6) — nhưng chưa nêu hệ thống cảnh báo **cái gì** và ở mốc nào. *(GR-013 C5, GR-014, GR-016, GR-020 C6 · NON-GOAL-012)*

**Không có `CONFLICT` nào.** Bốn chỗ trông như mâu thuẫn — *"ghi nhận bản đầu"* vs *"ghi nhận bản cuối"*, fallback của `GR-017` vs lệnh cấm của `GR-007`, *"tối đa 1 lượt chọn"* vs lượt quay lại vị trí 1, và `stripDiacritics` vs mệnh đề chính tả của luật gốc — đều đã được chính `docs/game-rules.md` giải thích tường minh là **không cùng đối tượng** hoặc **ngoại lệ có chủ ý**; xem §Edge Cases → *Conflicting rule*.

## Traceability Matrix

| User story | PRD requirement | Game rules | Functional requirements | Acceptance scenarios |
|---|---|---|---|---|
| US-001 | PRD-REQ-036, PRD-REQ-037, PRD-REQ-038 | GR-026, GR-027, GR-008 §Thứ tự đánh giá, GR-013 §Bấm trùng, bảng §Phán quyết, bảng §Bản gửi quá hạn, bảng §Kênh trả lời, INV-003, INV-009, INV-010 | FR-001 → FR-010 **+ FR-009a** | AC-001 → AC-016 **+ AC-004a** |
| US-002 | PRD-REQ-039, PRD-REQ-040, PRD-REQ-041 | GR-028, GR-013, INV-001, INV-002, INV-018 | FR-011 → FR-018 | AC-017 → AC-024 |
| US-003 | PRD-REQ-042 | GR-035, GR-014, INV-004, INV-005, INV-016 | FR-019 → FR-024 | AC-025 → AC-033 |
| US-004 | PRD-REQ-043 | GR-033, GR-003, GR-021, GR-031, INV-015 | FR-025 → FR-029 | AC-034 → AC-040 |
| US-005 | PRD-REQ-044, PRD-REQ-045, PRD-REQ-046 | GR-032, GR-007 C2/C9, GR-009 C8, GR-003 C3, GR-012 §Đồng thời, INV-006, INV-007, INV-008, INV-013 | FR-030 → FR-038 **+ FR-034a → FR-034e** | AC-041 → AC-051 **+ AC-044a → AC-044g** |
| US-006 | PRD-REQ-047, PRD-REQ-048, PRD-REQ-092, PRD-REQ-093 | GR-031, GR-005, GR-012 C4, GR-023 §Nguồn đề, INV-011, INV-012, INV-014 | FR-039 → FR-050 | AC-052 → AC-063 |
| US-007 | PRD-REQ-049, PRD-REQ-088, PRD-REQ-089 | GR-037, GR-020 §Cấm, GR-008, GR-012, GR-019 §Bảo mật, INV-017 | FR-051 → FR-061 **+ FR-055a, FR-057a** | AC-064 → AC-076 **+ AC-071a** |
| US-008 | PRD §8.2, PRD §9.2 (`STATE-002`, `STATE-003`), EPIC-006 §11 · PRD-REQ-036, PRD-REQ-038, PRD-REQ-041 | GR-001, GR-002, GR-003, GR-004, GR-005, GR-006 | FR-062 → FR-070 | AC-077 → AC-101 |
| US-009 | PRD §8.2, PRD §9.2 (`STATE-004`), EPIC-006 §11 · PRD-REQ-045, PRD-REQ-046, PRD-REQ-058, PRD-REQ-069, PRD-REQ-092 | GR-007, GR-008, GR-009, GR-010, GR-011, GR-012, INV-019 | FR-071 → FR-086 **+ FR-083a** | AC-102 → AC-139 **+ AC-131a** |
| US-010 | PRD §9.2 (`STATE-005`), EPIC-006 §11 · PRD-REQ-040, PRD-REQ-042, PRD-REQ-066 | GR-013, GR-014, GR-015, TERM-037 | FR-087 → FR-090 | AC-140 → AC-150 |
| US-011 | PRD §9.2 (`STATE-006`), EPIC-006 §11 · PRD-REQ-038, PRD-REQ-041, PRD-REQ-069, PRD-REQ-070, PRD-REQ-088 | GR-016, GR-017, GR-018, GR-019, GR-020, GR-021, INV-018 | FR-091 → FR-100 | AC-151 → AC-177 |
| US-012 | PRD-REQ-090, PRD-REQ-093, PRD §8.1 bước 7a/7b | GR-023, GR-024, GR-025, GR-017 §Biên, GR-031 C9, GR-022 *(tiền đề)*, TERM-020 | FR-101 → FR-109 **+ FR-105a** | AC-178 → AC-195 **+ AC-190a** |
| US-013 | PRD-REQ-050, PRD-REQ-049 | GR-036, INV-016, INV-017, TERM-043 | FR-110 → FR-116 | AC-196 → AC-208 |
| *(xuyên suốt)* | `CLAUDE.md` §Nguyên tắc code, §Quy ước khác | — | FR-117, FR-118, FR-119 | AC-006, AC-010, AC-050, AC-072, AC-100, AC-155, AC-172, AC-178 |

### Bản đồ phủ bảng quyết định

> Mỗi outcome trong bảng quyết định của từng `GR-NNN` thuộc phạm vi feature này đều có ít nhất một acceptance scenario. Bảng dưới là bản đồ tra ngược để kiểm.

| Game rule | Ca trong bảng quyết định | Acceptance scenarios |
|---|---|---|
| `GR-001` | C1 · C2 · C3 · C4 | AC-077 · AC-078 · AC-079 · AC-080 |
| `GR-002` | C1 · C2 · C3 · C4 | AC-081 · AC-082 · AC-083 · AC-083 |
| `GR-003` | C1 · C2 · C3 · C4 · C5 · C6 | AC-084 · AC-084 · AC-085 · AC-086 · AC-087 · AC-087 |
| `GR-004` | C1 · C2 · C3 · C4 · C5 · C6 | AC-088 · AC-089 · AC-090 · AC-091 · AC-091 · AC-092 |
| `GR-005` | C1 · C2 · C3 · C4 | AC-093 · AC-084 · AC-094 · AC-095 |
| `GR-006` | C1 · C2 · C3 · C4 · C5 · C6 · C7 | AC-096 · AC-096 · AC-097 · AC-098 · AC-099 · AC-096 · AC-100 |
| `GR-007` | C1 · C2 · C3 · C4 · C5 · C6 · C7 · C8 · C9 | AC-102 · AC-043 · AC-103 · AC-104 · AC-107 · AC-105 · AC-106 · AC-108 · AC-048 |
| `GR-008` | C1 · C2 · C3 · C4 · C5 · C6 · C7 · C8 · C9 · C10 | AC-109 · AC-110 · AC-111 · AC-112 · AC-134 · AC-113 · AC-115 · AC-116 · AC-114 · AC-117 |
| `GR-009` | C1-C5 · C6 · C7 · C8 · C9-C10 · C11 · C12 · C13 · C14 · C15 | AC-118 · AC-125 · AC-119 · AC-043 · AC-120 · AC-126 · AC-121 · AC-122 · AC-123 · AC-124 |
| `GR-010` | C1 · C2 · C3 · C4 · C5 · C6 `[v2]` | AC-125 · AC-103 · AC-127 · AC-127 · AC-135 · *(ngoài phạm vi v1)* |
| `GR-011` | C1 · C2 · C3 · C4 · C5 · C6 · C7 · C8 | AC-128 · AC-129 · AC-130 · AC-131 · AC-132 · AC-133 · AC-131 · AC-134 |
| `GR-012` | C1 · C2 · C3 · C4 · C5 · C6 · C7 · C8 | AC-135 · AC-136 · AC-136 · AC-137 · AC-134 · AC-138 · AC-138 · AC-138 |
| `GR-013` | C1 · C2 · C3 · C4 · C5 `[v1.5]` · C6 · C7 | AC-140 · AC-142 · AC-141 · AC-141 · *(OQ-005)* · AC-145 · AC-145 |
| `GR-014` | C1 · C2 · C3 · C4 · C5 · C6 | AC-142 · AC-142 · AC-143 · AC-143 · AC-144 · AC-144 |
| `GR-015` | C1 · C2 · C3 · C4 · C5 · C6 | AC-147 · AC-148 · AC-149 · AC-149 · AC-150 · AC-150 |
| `GR-016` | C1 · C2 · C3 · C4 · C5 | AC-151 · AC-151 · AC-151 · AC-151 · AC-152 |
| `GR-017` | C1 · C2 · C3 · C4 · C5 · C6 · C7 · C8 | AC-153 · AC-153 · AC-154 · AC-155 · AC-155 · AC-156 · AC-156 · AC-157 |
| `GR-018` | C1 · C2 · C3 · C4 · C5 · C6 | AC-159 · AC-160 · AC-160 · AC-161 · AC-161 · AC-162 |
| `GR-019` | C1 · C2 · C3 · C4 · C5 · C6 | AC-163 · AC-163 · AC-163 · AC-164 · AC-165 · AC-165 |
| `GR-020` | C1 · C2 · C3 · C4 · C5 · C6 `[v1.5]` | AC-167 · AC-168 · AC-169 · AC-170 · AC-176 · *(OQ-005)* |
| `GR-021` | C1 · C2 · C3 · C4 · C5 · C5b · C5c · C6 · C7 · C8 | AC-172 · AC-172 · AC-173 · AC-177 · AC-176 · AC-176 · AC-176 · AC-174 · AC-175 · AC-175 |
| `GR-023` | C1 · C2 · C3 · C4 · C5 | AC-180 · AC-181 · AC-182 · AC-195 · AC-195 |
| `GR-024` | C1 · C2 · C3 | AC-185 · AC-186 · AC-185 |
| `GR-025` | C1 · C2 · C3 · C4 · C5 | AC-191 · AC-192 · AC-193 · AC-190 · AC-194 |
| `GR-026` | C1 · C2 · C3 · C4 · C5 | AC-001 · AC-002 · AC-003 · AC-004 · AC-005 |
| `GR-027` | C1 · C2 · C3 · C4 · C5 | AC-010 · AC-011 · AC-012 · AC-013 · AC-014 |
| `GR-028` | C1 · C2 · C3 · C4 · C5 | AC-017 · AC-017 · AC-017 · AC-018 · AC-019 |
| `GR-031` | C1 · C2 · C3 · C3b · C3c · C4 · C5 · C6 · C7 · C8 · C9 | AC-052 · AC-053 · AC-054 · AC-055 · AC-058 · AC-057 · AC-061 · AC-060 · AC-059 · AC-061 · AC-062 |
| `GR-032` | C1 · C2 · C3 · C4 · C5 · C6 · C7 | AC-041 · AC-042 · AC-043 · AC-044 · AC-045 · AC-046 · AC-047 |
| `GR-033` | C1 · C2 · C3 · C4 · C5 · C6 | AC-035 · AC-036 · AC-034 · AC-037 · AC-038 · AC-039 |
| `GR-034` | C1 · C2 · C3 · C4 · C5 · C6 | AC-118 *(là chuông)* · AC-118 · *(EPIC-008)* · *(EPIC-008)* · AC-087 · AC-050 |
| `GR-035` | C1 · C2 · C3 · C4 · C5 · C6 | AC-025 · AC-026 · AC-027 · AC-028 · AC-029 · AC-030 |
| `GR-036` | C1 · C2 · C3 · C4 · C5 · C6 · C7 · C8 · C9 · C10 · C11 | AC-196 · AC-197 · AC-198 · AC-199 · AC-201 · AC-202 · AC-203 · AC-200 · AC-204 · AC-205 · AC-206 |
| `GR-037` | C1 · C2 · C3 · C4 · C5 · C6 · C7 · C8 · C9 | AC-064 · AC-065 · AC-066 · AC-068 · AC-068 · AC-069 · AC-067 · AC-071 **+ AC-071a** *(ca khép-câu ở Về đích)* · AC-075 |
| `GR-032` C4 *(mở rộng)* | kích hoạt tay một tín hiệu bất kỳ còn hiệu lực — mọi vòng chuông, gồm cả tín hiệu Chướng ngại vật của VCNV | AC-044a · AC-044b · AC-044c · AC-044d · AC-044e · AC-044f · AC-044g |
| `GR-008`, `GR-013` *(sửa dấu)* | dấu từng ghế là lựa chọn tạm tới mốc chốt câu | AC-004a |
| `GR-031` C9 × `GR-023` | giao của phép lọc đảo chiều và thứ tự ưu tiên ba kho | AC-190a |
| `GR-008`, `GR-011` *(thời lượng)* | giá trị cấu hình dùng chung cho ba cửa sổ VCNV | AC-131a |
| `GR-022` | *(tiền đề — đặc tả ở `specs/005-phong-thi-vong-doi-tran` FR-018 → FR-020)* | — |
| `GR-029`, `GR-030` | *(thao tác của admin — đặc tả ở EPIC-005 / EPIC-007)* | AC-017, AC-018 *(chỉ hệ quả phía engine)* |

### Acceptance Scenarios (chi tiết)

#### Nhóm A — Phán quyết (US-001)

**AC-001** — *US*: US-001 · *FR*: FR-001 · *GR*: GR-026 C1
**Given** một câu Khởi động lượt riêng giá trị 10 điểm đang ở trạng thái đã hết giờ chưa chấm, ghế A có điểm hiện tại 0, admin là phiên đang giữ quyền điều khiển,
**When** admin bấm **Đúng** cho ghế A,
**Then** hệ thống sinh đúng một sự kiện điểm `+10` gắn với câu đó và ghế A, điểm ghế A thành **10**, câu chuyển sang trạng thái *đã chấm*, và điểm của mọi ghế khác không đổi.

**AC-002** — *US*: US-001 · *FR*: FR-001, FR-005 · *GR*: GR-026 C2
**Given** cùng câu như AC-001, và một câu Khởi động lượt chung mà ghế B đã giành quyền,
**When** admin bấm **Sai** ở từng câu,
**Then** ở lượt riêng hệ thống sinh sự kiện điểm giá trị **0** và ghế A không bị trừ; ở lượt chung hệ thống sinh sự kiện điểm **−5** cho ghế B — hình phạt phụ thuộc vòng, không phụ thuộc nút.

**AC-003** — *US*: US-001 · *FR*: FR-001, FR-002, FR-004 · *GR*: GR-026 C3
**Given** ghế A đã gửi một bài làm **khớp tuyệt đối** với một phần tử trong danh sách đáp án được chấp nhận, đồng hồ câu đã hết giờ, admin chưa bấm nút phán quyết nào,
**When** một khoảng thời gian bất kỳ trôi qua,
**Then** không sự kiện điểm nào được sinh, điểm của mọi ghế không đổi, nút *"Câu kế tiếp"* không hiện, và kết quả đối chiếu chỉ tồn tại dưới dạng gợi ý hiển thị cho admin.

**AC-004** — *US*: US-001, US-009, US-010 · *FR*: FR-009 · *GR*: GR-026 C4
**Given** một câu Tăng tốc có 4 ghế, admin mới chấm Đúng cho 2 ghế và chưa chấm 2 ghế còn lại; và song song, một câu hàng ngang VCNV mà admin mới chấm 1 trong 4 ghế,
**When** admin bấm **chốt câu** ở từng trường hợp,
**Then** mọi ghế chưa chấm tính là **Sai**: ở Tăng tốc chúng nhận 0 và không giữ chỗ trong thang 40/30/20/10 *(vẫn là một sự kiện điểm cho cả bảng)*; ở hàng ngang VCNV chúng nhận 0 và **không** bị đặt cờ bị loại.

**AC-004a** — *US*: US-001, US-010 · *FR*: FR-009a, FR-003 · *GR*: GR-013 §Bấm trùng, GR-008 §Thứ tự đánh giá
**Given** một câu Tăng tốc với 4 ghế, admin đã đặt dấu **Đúng** cho ghế A và **Sai** cho ghế B, chưa bấm *chốt câu*,
**When** admin đổi dấu của ghế A thành **Sai** rồi đổi lại thành **Đúng**, đổi dấu ghế B thành **Đúng**, sau đó mới bấm *chốt câu*; và cuối cùng thử đổi dấu ghế B một lần nữa,
**Then** mọi lần đổi **trước** cú *chốt câu* đều được chấp nhận và **không** sự kiện điểm nào được sinh trong suốt giai đoạn đó; tại cú *chốt câu* hệ thống sinh đúng **một** sự kiện điểm cho cả bảng theo dấu **cuối cùng** của từng ghế; và lần đổi **sau** *chốt câu* bị từ chối — sửa sai từ đây phải đi qua điều chỉnh điểm thủ công.

**AC-005** — *US*: US-001, US-009 · *FR*: FR-009 · *GR*: GR-026 C5
**Given** một câu hàng ngang VCNV đang chờ chấm, và trong hàng đợi chặn có một tín hiệu *"Mở chướng ngại vật"* của ghế C **chưa được admin duyệt**,
**When** admin bấm **chốt câu** cho câu hàng ngang đó,
**Then** tín hiệu *"Mở chướng ngại vật"* của ghế C **không** bị chấm Sai và **không** bị ảnh hưởng, ghế C **không** bị loại, và tín hiệu đó vẫn nằm nguyên trong hàng đợi chờ duyệt.

**AC-006** — *US*: US-001 · *FR*: FR-003, FR-117 · *GR*: GR-026 §Bấm trùng · INV-009
**Given** một câu vừa được chấm Đúng, nút chấm đã tự khoá ở giao diện,
**When** một yêu cầu phán quyết thứ hai cho cùng câu đó được gửi thẳng tới server, bỏ qua giao diện,
**Then** server từ chối yêu cầu, không sinh sự kiện nào, điểm không đổi, và không có đường nào đổi phán quyết tại chỗ hay tự tính lại thứ hạng.

**AC-007** — *US*: US-001 · *FR*: FR-005 · *GR*: bảng §Phán quyết có hai hay ba lựa chọn
**Given** trận lần lượt ở Khởi động lượt riêng, Khởi động lượt chung, câu hàng ngang VCNV, Tăng tốc, Về đích người thi chính, và Về đích người cướp quyền, mỗi lần một câu đang chờ chấm,
**When** màn chấm được dựng cho từng trường hợp,
**Then** ba trường hợp Khởi động lượt chung, Về đích người cướp quyền và Về đích câu có Ngôi sao hy vọng có **ba** lựa chọn *(Đúng / Sai / Huỷ kết quả)*, các trường hợp còn lại có **hai** lựa chọn *(Đúng / Sai)*.

**AC-008** — *US*: US-001 · *FR*: FR-006 · *GR*: bảng §Bản gửi quá hạn
**Given** một câu Tăng tốc *(vòng mà Sai trừ 0 điểm)* mà ghế A **chỉ** gửi một bản sau hạn chót, không có bản hợp lệ nào,
**When** admin mở màn chấm cho ghế A,
**Then** lựa chọn **Huỷ kết quả** có mặt bên cạnh Đúng và Sai, dù đây là vòng mà *Sai* trừ 0; và dù chọn nút nào thì vẫn là **một** phán quyết cho câu đó.

**AC-009** — *US*: US-001 · *FR*: FR-007 · *GR*: bảng §Bản gửi quá hạn, GR-035 C6
**Given** ghế A gửi bản `"Hà Nội"` **trước** hạn chót và bản `"Huế"` **sau** hạn chót,
**When** admin mở màn chấm cho ghế A,
**Then** cả hai bản đều hiển thị, bản `"Huế"` được tô **đỏ**, bản quá hạn **không** tự ghi đè bản hợp lệ, hệ thống **không** tự loại bản nào, và cả việc đưa bản quá hạn ra ngoài lẫn việc chấm đều chờ admin bấm.

**AC-010** — *US*: US-001 · *FR*: FR-008, FR-119 · *GR*: GR-027 C1
**Given** đáp án được chấp nhận là `"Huế"`, ghế A gửi `"  HUẾ  "`, tuỳ chọn bỏ dấu đang **TẮT**,
**When** hệ thống chuẩn hoá và đối chiếu để hiển thị,
**Then** màn chấm hiện **không có khác biệt** nào được tô, bài làm **gốc** `"  HUẾ  "` vẫn được lưu nguyên, và không sự kiện điểm nào được sinh từ việc đối chiếu này.

**AC-011** — *US*: US-001 · *FR*: FR-008 · *GR*: GR-027 C2
**Given** đáp án `"Huế"`, ghế A gửi `"Hué"`, tuỳ chọn bỏ dấu **TẮT**,
**When** hệ thống đối chiếu để hiển thị,
**Then** ký tự khác ở vị trí dấu được tô nổi bật, và phán quyết vẫn hoàn toàn thuộc về admin.

**AC-012** — *US*: US-001 · *FR*: FR-008 · *GR*: GR-027 C3
**Given** đáp án `"Huế"`, ghế A gửi `"Hue"`, tuỳ chọn bỏ dấu **BẬT**,
**When** hệ thống đối chiếu để hiển thị,
**Then** **không** ký tự nào được tô — dấu bị bỏ qua khi so — nhưng hệ thống **không** vì thế mà kết luận Đúng; admin vẫn chấm Sai được nếu muốn.

**AC-013** — *US*: US-001 · *FR*: FR-008 · *GR*: GR-027 C4
**Given** đáp án `"Huế"`, ghế A gửi `"Thành phố Huế"`,
**When** hệ thống đối chiếu để hiển thị,
**Then** hai từ thừa được tô nổi bật, và admin quyết theo yêu cầu của câu.

**AC-014** — *US*: US-001 · *FR*: FR-008 · *GR*: GR-027 C5
**Given** ghế A không gửi gì, hoặc chỉ gửi một chuỗi rỗng sau khi trim,
**When** admin mở màn chấm cho ghế A,
**Then** hệ thống **không so và không tô** gì cả, và trình bày trạng thái *"không có đáp án"*; admin vẫn bấm được nút phán quyết theo luật của vòng.

**AC-015** — *US*: US-001 · *FR*: FR-002 · *GR*: GR-027 §Không đổi gì
**Given** một loạt bài làm đã được chuẩn hoá và tô nổi bật ở nhiều câu khác nhau,
**When** kiểm tra nhật ký sự kiện và kho dữ liệu bài làm,
**Then** **không** sự kiện điểm nào truy nguyên về kết quả tô nổi bật, bài làm **gốc** của mọi ghế còn nguyên, và danh sách đáp án được chấp nhận không bị thay đổi bởi bất kỳ thao tác nào trong trận.

**AC-016** — *US*: US-001, US-011 · *FR*: FR-010 · *GR*: bảng §Kênh trả lời, GR-019
**Given** ba câu ở ba kênh khác nhau — một câu Khởi động ở mode sân khấu *(nói)*, một câu Tăng tốc *(gõ, luôn gõ máy)*, và một câu thực hành Về đích, tất cả đều đang trong thời gian còn lại,
**When** admin thử bấm nút chấm ở từng câu trước khi hết giờ,
**Then** câu **nói** chấm được ngay và máy thí sinh không có nút gửi; câu **gõ** có nút chấm **khoá** tới khi hết giờ trong khi nút gửi của thí sinh vẫn sống; câu **thực hành** có nút chấm **sống suốt** vì không có ô nhập nào để chờ.

#### Nhóm B — Mô hình điểm (US-002)

**AC-017** — *US*: US-002 · *FR*: FR-011, FR-012 · *GR*: GR-028 C1, C2, C3
**Given** ghế A ở điểm 0 trong một trận đang chạy,
**When** lần lượt xảy ra: chấm Đúng một câu 10 điểm, chấm Sai một câu có hình phạt −5, và một điều chỉnh điểm thủ công `+3` kèm lý do,
**Then** điểm hiển thị của ghế A bằng **8** và bằng đúng kết quả tính lại từ nhật ký; cả **ba** sự kiện còn nguyên trong nhật ký, không sự kiện nào bị xoá, sửa, hay đảo thứ tự.

**AC-018** — *US*: US-002 · *FR*: FR-012, FR-013 · *GR*: GR-028 C4
**Given** vòng Khởi động đã sinh 5 sự kiện điểm với tổng `+30` cho các ghế,
**When** vòng đó bị bỏ,
**Then** hệ thống **thêm** vào nhật ký một sự kiện đảo ngược cho **mỗi** sự kiện điểm thuộc vòng đó, điểm tính lại về đúng mức trước khi vòng chạy, và **không** sự kiện gốc nào biến mất khỏi nhật ký.

**AC-019** — *US*: US-002 · *FR*: FR-014 · *GR*: GR-028 C5
**Given** một trận đã chạy vài vòng với nhật ký sự kiện đầy đủ,
**When** một yêu cầu tra *"điểm của mọi ghế tại mốc 14:05:30"* được thực hiện,
**Then** hệ thống trả về kết quả rút gọn các sự kiện có mốc thời gian ≤ 14:05:30, và **không** sinh sự kiện nào cho chính phép truy vấn đó.

**AC-020** — *US*: US-002 · *FR*: FR-014 · *GR*: GR-028 §Bấm trùng
**Given** một nhật ký sự kiện cố định của một trận,
**When** phép tính điểm được gọi lại nhiều lần trên cùng nhật ký đó,
**Then** kết quả giống hệt nhau ở mọi lần gọi, và trạng thái trận không đổi.

**AC-021** — *US*: US-002 · *FR*: FR-015 · *GR*: GR-028, TERM-022
**Given** một câu Khởi động lượt chung mà ghế B đã được chấm Sai *(−5)*,
**When** một yêu cầu sinh sự kiện điểm trùng bộ ba *(cùng câu, cùng ghế B, cùng loại phán quyết)* tới server,
**Then** server nhận diện là trùng và **không** sinh sự kiện thứ hai; điểm ghế B vẫn là kết quả của đúng một lần `−5`.

**AC-022** — *US*: US-002, US-010 · *FR*: FR-016 · *GR*: GR-013, GR-028
**Given** một câu Tăng tốc đã chốt với kết quả `A +40 · B +30 · C +20 · D 0`,
**When** kiểm tra nhật ký, rồi sau đó bỏ vòng Tăng tốc,
**Then** câu đó ứng với đúng **một** sự kiện điểm mang kết quả của cả bốn ghế; và khi bỏ vòng, một sự kiện đảo ngược **cho cả bảng** của câu đó được thêm vào, không phải bốn sự kiện đảo ngược riêng lẻ.

**AC-023** — *US*: US-002, US-011 · *FR*: FR-017 · *GR*: GR-004 C5, GR-016, INV-018
**Given** ghế A đang có **0** điểm ở Khởi động lượt chung, và một bảng điểm cuối vòng Tăng tốc có `A = −100`, `B = −110`,
**When** ghế A bị chấm Sai *(−5)*, rồi vòng Về đích mở và hệ thống tính thứ tự lượt,
**Then** điểm ghế A thành **−5** *(không bị kẹp về 0)*, và ở bảng `{−100, −110}` hệ thống khuyến nghị ghế có **−100** đi trước vì đó là điểm cao nhất; điểm âm hiển thị bình thường trên mọi bảng.

**AC-024** — *US*: US-002, US-012 · *FR*: FR-018 · *GR*: GR-028 §ghi chú, GR-023
**Given** một trận đã có một sự kiện phân định Câu hỏi phụ ghi trong nhật ký,
**When** hệ thống tính điểm của mọi ghế,
**Then** sự kiện phân định đó **không** tham gia phép tính — không cộng, không trừ điểm cho ai — và đường hoàn nguyên duy nhất của nó là **bỏ vòng** phân định, không phải một sự kiện đảo ngược điểm.

#### Nhóm C — Server time (US-003)

**AC-025** — *US*: US-003 · *FR*: FR-019 · *GR*: GR-035 C1
**Given** một câu có hạn chót tính theo đồng hồ server, đang còn thời gian, các ghế đang gửi bài,
**When** đồng hồ server chạm đúng hạn chót,
**Then** câu chuyển sang trạng thái hết giờ chưa chấm, hệ thống không nhận thêm bản gửi **hợp lệ** mới, và không sự kiện điểm nào được sinh tự động.

**AC-026** — *US*: US-003, US-005 · *FR*: FR-023 · *GR*: GR-035 C2, GR-003 C4
**Given** Khởi động lượt chung, cửa sổ chuông đang mở, chưa ai giành quyền,
**When** hai ghế bấm chuông và server ghi **cùng một** mốc mili-giây cho cả hai,
**Then** hàng đợi tự chọn một trong hai, ngẫu nhiên tại thời điểm nhận, **không** ưu tiên theo số ghế hay số vị trí; thứ tự đã chọn được ghi vào lịch sử để dựng lại tất định về sau.

**AC-027** — *US*: US-003, US-010 · *FR*: FR-019 · *GR*: GR-035 C3, GR-014 C6
**Given** một câu Tăng tốc, ghế A có bản cuối được server nhận ở `5.123` giây và ghế B ở `5.124` giây, cả hai đều được chấm Đúng,
**When** admin chốt câu,
**Then** A xếp trên B — chênh **1 ms** vẫn phân định được — và hai ghế nhận hai mức điểm khác nhau theo thang.

**AC-028** — *US*: US-003, US-010 · *FR*: FR-019 · *GR*: GR-035 C4
**Given** một câu Tăng tốc, ghế A và ghế B có bản cuối được server nhận ở **cùng** mốc `8.500` giây, cả hai được chấm Đúng,
**When** admin chốt câu,
**Then** A và B **cùng bậc** và cùng nhận mức điểm của bậc đó; bậc kế nhảy qua đúng số người trong nhóm hoà.

**AC-029** — *US*: US-003 · *FR*: FR-021, FR-022 · *GR*: GR-035 C5
**Given** một máy thí sinh hiển thị đồng hồ chậm hơn server vài trăm mili-giây,
**When** ghế đó gửi một bản mà đồng hồ client còn báo *"còn thời gian"* nhưng server đã qua hạn chót,
**Then** server phân xử theo **hạn chót của mình** và đánh dấu bản đó là quá hạn; hiển thị ở client chỉ là tham khảo.

**AC-030** — *US*: US-003, US-001 · *FR*: FR-007 · *GR*: GR-035 C6
**Given** một câu đã qua hạn chót và ghế A gửi thêm một bản,
**When** server nhận bản đó,
**Then** bản được **giữ lại** và tô **đỏ** trên màn admin, đặt cạnh bản hợp lệ nếu có; hệ thống **không** chặn cứng và **không** tự loại nó.

**AC-031** — *US*: US-003 · *FR*: FR-020 · *GR*: GR-035 §Biên, INV-005
**Given** một cửa sổ chuông 3 giây bắt đầu ở `10.000` giây theo đồng hồ server,
**When** một tín hiệu chuông được server nhận ở **đúng** `13.000` giây, và một tín hiệu khác ở `13.001` giây,
**Then** tín hiệu ở `13.000` giây **hợp lệ** và giành được quyền; tín hiệu ở `13.001` giây không tồn tại vì nút đã không còn.

**AC-032** — *US*: US-003 · *FR*: FR-021 · *GR*: GR-035 §Điều kiện
**Given** một trận đang chạy đã ghi thứ tự chuông và thứ hạng tốc độ của vài câu,
**When** giờ hệ thống của máy chủ bị chỉnh lùi lại giữa trận,
**Then** thứ tự và thứ hạng đã ghi **không** đổi, và các cửa sổ thời gian đang chạy vẫn đếm đúng vì chúng dựa trên đồng hồ đơn điệu chứ không phải đồng hồ tường.

**AC-033** — *US*: US-003 · *FR*: FR-021 · *GR*: GR-035 §Không đổi gì
**Given** một hàng đợi đã ghi thứ tự của nhiều tín hiệu trong một vòng đã kết thúc,
**When** bất kỳ thao tác nào của admin diễn ra sau đó,
**Then** các mốc thời gian đã ghi và thứ tự đã ghi trong hàng đợi **không** bị sửa lại.

#### Nhóm D — Mốc do admin bấm (US-004)

**AC-034** — *US*: US-004, US-006, US-008, US-011 · *FR*: FR-025, FR-027 · *GR*: GR-033 C3
**Given** một câu Khởi động lượt chung đã được rút, chưa hiển thị; cửa sổ đặt Ngôi sao hy vọng của vòng Về đích tương ứng đang mở ở một trận khác cùng cấu hình,
**When** admin bấm **hiển thị câu hỏi**,
**Then** câu lên màn thí sinh và khán giả, **cửa sổ chuông mở** *(vì đây là lượt chung)*, **cửa sổ đặt Ngôi sao hy vọng đóng**, và câu được đánh dấu **đã dùng** — bốn hệ quả xảy ra tại đúng một mốc.

**AC-035** — *US*: US-004 · *FR*: FR-025 · *GR*: GR-033 C1
**Given** một câu đã hiển thị, đồng hồ chưa chạy, thời lượng suy nghĩ khai ở câu là 15 giây,
**When** admin bấm **start timer** ở mốc `5.234` giây theo đồng hồ server,
**Then** đồng hồ bắt đầu đếm 15 giây từ mốc đó, hạn chót được ghi là một **mốc tuyệt đối** trên đồng hồ server, và mốc bấm được lưu vào nhật ký.

**AC-036** — *US*: US-004 · *FR*: FR-029 · *GR*: GR-033 C2
**Given** admin vừa bấm **start timer** cho một câu,
**When** admin bấm nút đó lần thứ hai, hoặc một yêu cầu trùng tới server do trễ mạng,
**Then** nút đã tự khoá nên không có lần bấm thứ hai ở giao diện, server bỏ qua yêu cầu trùng, và cửa sổ thời gian **không** bị kéo dài.

**AC-037** — *US*: US-004 · *FR*: FR-026 · *GR*: GR-033 C4
**Given** admin đã bấm **hiển thị câu hỏi** nhưng chưa bấm **start timer**, MC đang đọc câu trên sân khấu,
**When** vài giây trôi qua,
**Then** câu đang hiện trên mọi màn, **đồng hồ chưa chạy** — đây là trạng thái hợp lệ theo thiết kế, không phải lỗi và không có cơ chế tự start.

**AC-038** — *US*: US-004, US-012 · *FR*: FR-025 · *GR*: GR-033 C5, GR-024
**Given** vòng Câu hỏi phụ, một câu đã hiển thị, admin chưa bấm start timer,
**When** admin bấm **start timer** — đây chính là mốc *hiệu lệnh của người dẫn chương trình*,
**Then** từ mốc đó nút chuông của các ghế trong nhóm hoà mới sống, và đồng hồ 15 giây bắt đầu đếm.

**AC-039** — *US*: US-004 · *FR*: FR-028 · *GR*: GR-033 C6
**Given** đường truyền giữa máy admin và server đang trễ vài trăm mili-giây,
**When** admin bấm một mốc bất kỳ,
**Then** server ghi mốc theo **thời điểm nó nhận được**, không cộng bù độ trễ, không có cửa sổ ân hạn, và mốc đó là tuyệt đối cho mọi phép tính sau đó.

**AC-040** — *US*: US-004 · *FR*: FR-026 · *GR*: GR-033 §Điều kiện, QĐ-028
**Given** giao diện điều khiển của admin cho một câu bất kỳ ở bất kỳ vòng nào,
**When** rà soát mọi đường thao tác và mọi cấu hình luật khả dụng,
**Then** *hiển thị câu hỏi* và *start timer* luôn là **hai** nút tuần tự, không tồn tại đường nào đảo thứ tự và không tồn tại cấu hình nào gộp chúng thành một thao tác.

#### Nhóm E — Hàng đợi tín hiệu (US-005)

**AC-041** — *US*: US-005 · *FR*: FR-033 · *GR*: GR-032 C1
**Given** vòng VCNV, ghế A đang tới lượt chọn, hàng ngang 3 chưa mở, mode nhập liệu,
**When** ghế A xác nhận chọn hàng ngang 3 và tín hiệu tới server,
**Then** tín hiệu vào hàng đợi ở trạng thái **chờ duyệt**, chưa sinh hệ quả nào: ô chưa đổi trạng thái, câu chưa hiển thị, đồng hồ chưa chạy.

**AC-042** — *US*: US-005 · *FR*: FR-033 · *GR*: GR-032 C2, GR-007 C1
**Given** tín hiệu của AC-041 đang ở đầu hàng đợi,
**When** admin bấm **Yes**,
**Then** hàng ngang 3 được đưa ra, lượt chọn của ghế A đánh dấu **đã dùng**, ô chuyển sang trạng thái *đã hỏi*, và tín hiệu kế tiếp trong hàng đợi lên đầu.

**AC-043** — *US*: US-005, US-009 · *FR*: FR-035 · *GR*: GR-032 C3, GR-007 C2, GR-009 C8 · INV-008
**Given** tín hiệu chọn hàng ngang của ghế A đang chờ duyệt, mode nhập liệu nên nút chọn của ghế A đang khoá tạm; câu của hàng ngang đó đã được rút nhưng **chưa hiển thị**,
**When** admin bấm **No**,
**Then** ghế A **không mất lượt**, nút chọn của ghế A **mở lại**, ô chữ chưa được đánh dấu, câu **quay lại kho** vì chưa tiêu, đồng hồ chưa chạy, tín hiệu kế tiếp lên đầu hàng đợi, và tín hiệu bị từ chối vẫn nằm trong lịch sử.

**AC-044** — *US*: US-005 · *FR*: FR-034 · *GR*: GR-032 C4
**Given** Khởi động lượt chung với cửa sổ chuông đang mở, và một cửa sổ cướp quyền Về đích đang mở ở một câu khác,
**When** một ghế bấm chuông ở mỗi trường hợp,
**Then** tín hiệu có hiệu lực **ngay** theo server timestamp, không chờ admin duyệt; hàng đợi vẫn ghi lại thứ tự làm lưới an toàn cho admin can thiệp khi có sự cố.

**AC-044a** — *US*: US-005, US-008 · *FR*: FR-034a · *GR*: GR-032 C4, GR-003 C3
**Given** một câu Khởi động lượt chung; ghế A bấm chuông ở `1.200` giây và giành quyền, ghế C bấm ở `1.450` giây và ghế B bấm ở `1.800` giây — hai tín hiệu sau ở trạng thái **trơ** trong lịch sử,
**When** admin chấm **Huỷ kết quả** cho A rồi mở danh sách ứng viên để **kích hoạt tay**,
**Then** hệ thống **khuyến nghị** ghế C — tín hiệu sớm nhất chưa xử lý — nhưng admin chọn được **ghế B** thay vào đó và hệ thống **không chặn cứng**; tín hiệu của ghế được chọn chuyển từ **trơ** sang **có hiệu lực**, và cả hai trạng thái đều còn trong lịch sử. Một tín hiệu thuộc **câu khác** đã khép MUST NOT xuất hiện trong danh sách ứng viên.

**AC-044b** — *US*: US-005, US-008 · *FR*: FR-034b · *GR*: GR-003, GR-004
**Given** tình huống AC-044a, ghế C vừa được kích hoạt ở mốc `t`,
**When** C trả lời và admin chấm **Sai**; và sau đó admin thử **kích hoạt tay** một lần nữa cho ghế B trong cùng câu,
**Then** C có **trọn 3 giây** suy nghĩ tính từ mốc `t` *(cửa sổ mới, không phải phần còn dư của cửa sổ cũ)*; C nhận **−5** đúng như một người giành quyền bình thường của lượt chung; và yêu cầu kích hoạt **lần thứ hai** trong cùng câu bị server **từ chối** — tối đa đúng một lần cho mỗi câu.

**AC-044c** — *US*: US-005, US-007, US-011 · *FR*: FR-034b, FR-055a · *GR*: GR-020, GR-037 C6, C8
**Given** vòng Về đích, cờ reveal **BẬT**; người thi chính A bị chấm Sai, ghế B giành quyền cướp và bị admin chấm **Huỷ kết quả**; trong hàng đợi còn tín hiệu của ghế C phát trong cửa sổ 5 giây,
**When** admin kích hoạt tay tín hiệu của C, C trả lời, và admin chấm C **Đúng**,
**Then** **không** cửa sổ đồng hồ mới nào được mở cho C *(Về đích không có đồng hồ trả lời riêng)*; C hưởng **transfer** đúng như một người cướp quyền bình thường — `A −giá trị câu · C +giá trị câu`; đáp án **không** rời server tới thí sinh, khán giả hay lớp phủ trong suốt khoảng từ cú bấm Huỷ kết quả tới khi C được chấm; và câu chỉ **khép** tại mốc chấm C.

**AC-044d** — *US*: US-005, US-009 · *FR*: FR-034c · *GR*: GR-009 C6/C8, GR-010, GR-034, bảng §2.2
**Given** vòng VCNV, đúng 2 hàng ngang đã hỏi nên băng điểm đang là **50**; ghế B bấm *"Mở chướng ngại vật"* và admin đã bấm **Yes** xác nhận tín hiệu; trong hàng đợi chặn còn tín hiệu Chướng ngại vật của ghế D,
**When** admin chấm **Huỷ kết quả** cho B — lựa chọn thứ ba của tín hiệu Chướng ngại vật — rồi kích hoạt tay tín hiệu của D,
**Then** ghế B **KHÔNG bị loại** khỏi vòng và **không** ai được cộng điểm từ cú bấm đó; ghế D lên làm người giải Chướng ngại vật; **không** cửa sổ đồng hồ nào được mở cho D; băng điểm chốt lại theo trạng thái bàn cờ tại **mốc admin xác nhận tín hiệu của D**; và nếu D bị chấm **Sai** thì D **bị loại khỏi vòng** và **không** bị trừ điểm, đúng `GR-010`.

**AC-044e** — *US*: US-005, US-009 · *FR*: FR-034d · *GR*: GR-012 §Đồng thời, GR-032 §Vòng đời tín hiệu
**Given** vòng VCNV với bốn ghế đều đã bấm *"Mở chướng ngại vật"*, hàng đợi chặn giữ đủ bốn tín hiệu theo thứ tự server timestamp; và song song, một câu Khởi động lượt chung mà admin vừa dùng kích hoạt tay đúng một lần,
**When** ở VCNV admin lần lượt xác nhận rồi chấm **Huỷ kết quả** cho ghế thứ nhất, kích hoạt ghế thứ hai, lại Huỷ kết quả, kích hoạt ghế thứ ba; và ở Khởi động lượt chung admin thử kích hoạt lần thứ hai trong cùng câu,
**Then** chuỗi ở VCNV **được phép** tiếp diễn tới khi hàng đợi Chướng ngại vật của vòng cạn — không trần tổng số lần; còn ở Khởi động lượt chung yêu cầu kích hoạt **lần thứ hai** trong cùng câu bị server **từ chối**. Hai giới hạn khác nhau vì **đích** của tín hiệu khác nhau *(vòng so với câu)*.

**AC-044f** — *US*: US-005, US-009 · *FR*: FR-034e · *GR*: GR-010, GR-036 C5, INV-019
**Given** một vòng VCNV: ghế A có tín hiệu Chướng ngại vật **trơ** chưa xử lý · ghế B **vừa bị chấm Huỷ kết quả** cho tín hiệu của mình · ghế C **đã bị loại** khỏi vòng vì giải sai Chướng ngại vật · ghế D đang bị admin **vô hiệu hoá**,
**When** admin mở danh sách ứng viên để kích hoạt tay,
**Then** danh sách chứa **ghế A và ghế B** — B được đưa lại dù vừa bị huỷ, tức nhận lượt thứ hai; danh sách **không** chứa ghế C *(bị loại là tuyệt đối trong vòng)* và **không** chứa ghế D *(admin phải kích hoạt lại ghế D trước)*; và một yêu cầu kích hoạt C hoặc D gửi thẳng tới server bị **từ chối**.

**AC-044g** — *US*: US-005 · *FR*: FR-034e · *GR*: GR-016 C5, INV-020, PRD §9.4
**Given** một câu Khởi động lượt chung có ba tín hiệu trơ chưa xử lý: ghế C ở `1.450` giây, ghế B ở `1.800` giây, ghế E ở `2.100` giây; hệ thống đang khuyến nghị **ghế C**,
**When** admin chọn kích hoạt **ghế E**,
**Then** hệ thống hiện **dialog cảnh báo lệch luật** Yes/No nêu rõ ghế E vượt qua ghế C và ghế B cùng thứ tự timestamp gốc; dialog **không** bắt nhập lý do và **không** thuộc hạng phá huỷ; admin bấm Yes thì ghế E thành người giữ quyền, bấm No thì không gì thay đổi và tập ứng viên giữ nguyên.

**AC-045** — *US*: US-005 · *FR*: FR-032 · *GR*: GR-032 C5
**Given** một vòng VCNV có nhiều tín hiệu đã được ghi trong hàng đợi và trong lịch sử,
**When** vòng kết thúc,
**Then** hàng đợi **đang hoạt động** trống, trong khi **lịch sử tín hiệu** giữ nguyên toàn bộ và vẫn xem lại được.

**AC-046** — *US*: US-005 · *FR*: FR-036 · *GR*: GR-032 C6
**Given** ghế D đã bị loại khỏi vòng VCNV do giải sai Chướng ngại vật,
**When** ghế D thử thao tác bất kỳ trên màn thi đấu,
**Then** máy ghế D không hiển thị nút nào cho các thao tác đó, thao tác không phản hồi, **không tín hiệu nào được tạo**, và hàng đợi không nhận thêm gì từ ghế D.

**AC-047** — *US*: US-005 · *FR*: FR-032 · *GR*: GR-032 C7
**Given** một tín hiệu của ghế B đã bị admin từ chối ở một vòng đã kết thúc,
**When** admin mở lại lịch sử tín hiệu để phân xử một khiếu nại,
**Then** tín hiệu đó vẫn hiển thị đầy đủ kèm server timestamp và trạng thái *bị từ chối*, và admin dùng được nó làm căn cứ để can thiệp.

**AC-048** — *US*: US-005 · *FR*: FR-030 · *GR*: GR-007 C9, GR-032 §Đồng thời · INV-007
**Given** vòng VCNV, hàng đợi chặn đang trống,
**When** một tín hiệu *chọn hàng ngang* tới ở `1.000` giây và một tín hiệu *"Mở chướng ngại vật"* tới ở `1.040` giây,
**Then** admin duyệt tín hiệu *chọn hàng ngang* trước — **thuần theo thứ tự tới, không ưu tiên theo loại** — và thứ tự đó được ghi lại.

**AC-049** — *US*: US-005 · *FR*: FR-031 · *GR*: GR-003 C3 · INV-006
**Given** Khởi động lượt chung, ghế A đã giành quyền,
**When** ghế B bấm chuông sau đó, trong cửa sổ vẫn còn,
**Then** tín hiệu của ghế B được ghi vào lịch sử kèm server timestamp với outcome **trơ**; nó **không** đổi người giữ quyền, **không** mở lại chuông, và **không** bị drop.

**AC-050** — *US*: US-005 · *FR*: FR-036, FR-037, FR-117 · *GR*: GR-032 §Bấm trùng, GR-034 C6
**Given** một tín hiệu đã bị admin từ chối, và một cửa sổ chuông đã đóng,
**When** một yêu cầu từ chối lần thứ hai cho cùng tín hiệu đó tới server, và một tín hiệu chuông do client tự chế gửi sau khi cửa sổ đã đóng,
**Then** yêu cầu từ chối lần hai **không đổi gì** *(idempotent)*, và tín hiệu chuông muộn bị server **từ chối** kèm một dòng nhật ký thao tác.

**AC-051** — *US*: US-005 · *FR*: FR-032 · *GR*: GR-032 §Vòng đời tín hiệu, GR-012 §Đồng thời
**Given** trong một vòng VCNV có ba loại tín hiệu cùng tồn tại: chọn hàng ngang, trả lời hàng ngang, và *"Mở chướng ngại vật"*,
**When** lần lượt: lượt chọn kết thúc, câu được chấm xong, và vòng kết thúc,
**Then** mỗi loại tín hiệu vô hiệu đúng tại thời điểm **đích của nó** đóng, và trong mọi trường hợp lịch sử tín hiệu vẫn được giữ nguyên vẹn.

#### Nhóm F — Rút đề và không lặp câu (US-006)

**AC-052** — *US*: US-006 · *FR*: FR-039 · *GR*: GR-031 C1
**Given** một contest có danh sách đã gán gồm 200 câu, chưa câu nào được dùng, một vòng cần 4 câu,
**When** server rút đề cho lượt đó,
**Then** 4 câu được chọn ngẫu nhiên **trong danh sách đã gán**, một sự kiện rút đề được phát để phát lại được, và cờ **đã dùng** chỉ được đặt khi từng câu **hiển thị**, không phải khi rút.

**AC-053** — *US*: US-006 · *FR*: FR-042 · *GR*: GR-031 C2
**Given** trận thứ nhất của một contest đã hiển thị 4 câu,
**When** trận thứ hai của cùng contest mở một vòng và server rút đề,
**Then** kho hiệu dụng là **196** câu — 4 câu đã dùng bị loại xuyên trận, dù trận thứ hai có dùng một danh sách gán khác.

**AC-054** — *US*: US-006 · *FR*: FR-043 · *GR*: GR-031 C3 · INV-014
**Given** một vòng cần 4 câu nhưng kho hiệu dụng cho vòng đó chỉ còn 3, trong khi các vòng khác vẫn đủ câu,
**When** admin thử mở vòng đó,
**Then** vòng **không mở được** *(chặn cứng)*, thông điệp nêu rõ còn thiếu bao nhiêu câu, và các vòng khác **vẫn mở bình thường**.

**AC-055** — *US*: US-006, US-009 · *FR*: FR-046 · *GR*: GR-031 C3b
**Given** danh sách đã gán có đủ 4 hàng ngang và 1 Chướng ngại vật và 1 câu ô trung tâm nhưng chúng **không thuộc cùng một bộ**,
**When** admin thử mở vòng VCNV,
**Then** vòng **không mở được** — phép kiểm đếm **bộ nguyên vẹn**, và đủ số câu không phải điều kiện đủ.

**AC-056** — *US*: US-006, US-009 · *FR*: FR-045 · *GR*: GR-031 §Ngoại lệ · TERM-058
**Given** một bộ VCNV gồm 1 Chướng ngại vật, 4 hàng ngang mang số thứ tự 1→4, và 1 câu ô trung tâm,
**When** thử ghép một hàng ngang của bộ này với một Chướng ngại vật của bộ khác, và thử mở vòng khi một trong sáu thành phần đã bị dùng,
**Then** thao tác hoán đổi không tồn tại — số thứ tự hàng ngang cố định ứng với một miếng ghép ở một góc cố định — và bộ có thành phần đã dùng **không** khả dụng.

**AC-057** — *US*: US-006 · *FR*: FR-040, FR-044 · *GR*: GR-031 C4, GR-005 C1 · INV-011, INV-012
**Given** một vòng Khởi động lượt chung đã mở được qua cửa kiểm kho đề, đang chạy, và một câu trong vòng **bị bỏ qua** vì không ai bấm chuông,
**When** vòng chạy tiếp tới hết 12 câu,
**Then** câu bị bỏ qua vẫn tính là **đã dùng** và nằm trong con số 12 cố định; hệ thống **không** rút thêm một câu thứ 13 để bù, và tình huống cạn đề giữa vòng không xảy ra.

**AC-058** — *US*: US-006, US-012 · *FR*: FR-047, FR-050 · *GR*: GR-031 C3c, GR-023 §Nguồn đề
**Given** một bộ VCNV còn nguyên vẹn trong danh sách đã gán, và admin đang ở trạng thái nghỉ giữa hai vòng,
**When** admin chỉ định một **hàng ngang** của bộ đó làm câu Câu hỏi phụ,
**Then** hệ thống cảnh báo rằng thao tác này **làm vỡ một bộ** và nêu rõ bộ nào; nếu admin xác nhận, bộ đó không còn dùng được cho vòng VCNV dù 5 thành phần kia còn nguyên, và câu đã đặt chỗ bị loại khỏi phép rút của các vòng mở sau đó. Phép rút tự động của Câu hỏi phụ **không bao giờ** chạm kho VCNV khi hai kho ưu tiên trên còn câu.

**AC-059** — *US*: US-006 · *FR*: FR-041 · *GR*: GR-031 C7
**Given** một câu đã được rút cho lượt kế nhưng **chưa hiển thị** cho ai,
**When** admin gỡ câu đó khỏi danh sách đã gán ở trạng thái nghỉ,
**Then** thao tác thành công và câu **quay lại** kho hiệu dụng — nó chưa tiêu.

**AC-060** — *US*: US-006 · *FR*: FR-040, FR-042 · *GR*: GR-031 C6
**Given** một câu **đã hiển thị** ở một vòng trước của trận này hoặc của một trận trước trong cùng contest,
**When** admin thử gỡ câu đó khỏi danh sách đã gán, và sau đó bỏ vòng chứa nó,
**Then** thao tác gỡ **không thực hiện được** *(invalid state, không ép được)*, và việc bỏ vòng đảo điểm nhưng **không** trả câu đó về kho.

**AC-061** — *US*: US-006 · *FR*: FR-043 · *GR*: GR-031 C5, C8
**Given** một vòng đang chạy dở, và một thời điểm khác khi trận đang ở trạng thái nghỉ,
**When** admin thử sửa danh sách câu đã gán ở từng thời điểm,
**Then** trong lúc vòng đang chạy **không** có cửa sửa nào mở; ở trạng thái nghỉ thì sửa được, phép kiểm kho đề chạy lại ngay, thao tác vào nhật ký, và cờ đã-dùng của các câu **không** bị đụng tới.

**AC-062** — *US*: US-006 · *FR*: FR-048 · *GR*: GR-031 C9
**Given** một contest **thật** đã chạy xong một trận và đã hiển thị 40 câu; và một contest thật khác **chưa chạy trận nào**,
**When** một trận `practice` được mở trong từng contest,
**Then** ở contest thứ nhất, kho khả dụng của trận practice **chỉ** gồm 40 câu đã lộ, và trận practice **không** tiêu thêm câu nào; ở contest thứ hai, kho đó **rỗng** nên cửa vào vòng tự chặn.

**AC-063** — *US*: US-006 · *FR*: FR-049 · *GR*: GR-031 §Bấm trùng
**Given** server vừa rút một câu cho lượt kế, câu đó **chưa hiển thị**,
**When** thao tác rút được thực hiện lần nữa,
**Then** một sự kiện rút đề **mới** được sinh *(không dedup)*, câu của lần rút trước quay lại kho vì chưa tiêu, và không câu nào bị mất.

#### Nhóm G — Phạm vi hiển thị đáp án (US-007)

**AC-064** — *US*: US-007 · *FR*: FR-051 · *GR*: GR-037 C1 · INV-017
**Given** một câu đang mở, **chưa khép**, và một phiên đang giữ quyền điều khiển có permission đọc đáp án của câu đang chạy,
**When** phiên đó yêu cầu xem đáp án,
**Then** server trả đáp án và ghi một dòng nhật ký thao tác cho lần xem đó; trạng thái trận và điểm không đổi.

**AC-065** — *US*: US-007 · *FR*: FR-051 · *GR*: GR-037 C2
**Given** cùng câu như AC-064, và một phiên **không** giữ quyền điều khiển nhưng **có** permission đọc đáp án,
**When** phiên đó yêu cầu xem đáp án,
**Then** server trả đáp án và ghi nhật ký; việc trả đáp án **không** kèm theo bất kỳ quyền công bố nào cho phiên đó.

**AC-066** — *US*: US-007 · *FR*: FR-053, FR-054 · *GR*: GR-037 C3
**Given** một trận có cờ *hiện đáp án sau khi chấm* đang **TẮT** *(admin đã đổi khỏi mặc định trước khi bắt đầu trận)*, một câu Khởi động đã được chấm xong,
**When** kênh thí sinh, kênh khán giả và kênh lớp phủ nhận dữ liệu của câu đó,
**Then** **không** kênh nào nhận được đáp án chuẩn, ở mọi thời điểm — kể cả sau mốc câu khép.

**AC-067** — *US*: US-007 · *FR*: FR-057 · *GR*: GR-037 C7
**Given** một trận có cờ *hiện đáp án sau khi chấm* **BẬT**, một câu Khởi động lượt chung **bị bỏ qua** vì hết 3 giây không ai bấm chuông,
**When** câu khép lại,
**Then** đáp án **vẫn được công bố** tới thí sinh, khán giả và lớp phủ — câu đã hỏi, đã tiêu, nên giữ kín không bảo vệ được gì.

**AC-068** — *US*: US-007 · *FR*: FR-053, FR-054 · *GR*: GR-037 C4, C5
**Given** một trận có cờ *hiện đáp án sau khi chấm* **BẬT** *(mặc định)*, một câu Khởi động lượt riêng đang mở và chưa được chấm,
**When** trước tiên các kênh thí sinh, khán giả và lớp phủ yêu cầu dữ liệu của câu; rồi admin chấm xong và các kênh đó nhận dữ liệu lần nữa,
**Then** ở lần đầu **không** kênh nào nhận đáp án *(câu chưa khép)*; ở lần sau cả ba kênh nhận đáp án *(câu đã khép)*.

**AC-069** — *US*: US-007, US-011 · *FR*: FR-055, FR-056 · *GR*: GR-037 C6, GR-020 §Cấm
**Given** vòng Về đích, một câu 30 điểm, cờ reveal **BẬT**, người thi chính A vừa bị chấm **Sai** nên cửa sổ cướp quyền 5 giây đang mở,
**When** trong suốt 5 giây đó, các kênh thí sinh, khán giả và lớp phủ nhận dữ liệu của câu,
**Then** **không** kênh nào nhận đáp án; sau khi cửa sổ đóng và người cướp B được chấm, đáp án mới xuất hiện đồng thời trên cả ba kênh.

**AC-070** — *US*: US-007, US-011 · *FR*: FR-055 · *GR*: GR-037 §bảng Mốc câu khép
**Given** lần lượt một câu ở mỗi vòng — Khởi động lượt riêng, Khởi động lượt chung, hàng ngang VCNV, ô trung tâm VCNV, Tăng tốc, Về đích — với cờ reveal **BẬT**,
**When** từng câu đi tới mốc kết thúc tự nhiên của nó,
**Then** năm trường hợp đầu khép **tại cú bấm chấm** *(lượt chung thêm nhánh hết 3 giây không ai bấm)*, còn Về đích khép muộn hơn: khi cửa sổ cướp đã đóng **và** người cướp đã được chấm, hoặc hết 5 giây không ai bấm, hoặc người thi chính được chấm **Đúng**.

**AC-071** — *US*: US-007 · *FR*: FR-057 · *GR*: GR-037 C8
**Given** một câu được khép bằng phán quyết **Huỷ kết quả**, cờ reveal **BẬT**,
**When** câu khép lại,
**Then** hệ thống **không tự** công bố đáp án tới thí sinh, khán giả hay lớp phủ; admin vẫn mở tay được bằng thao tác riêng của mình.

**AC-071a** — *US*: US-007, US-011 · *FR*: FR-057a · *GR*: GR-037 C6, C8, GR-020
**Given** vòng Về đích, một câu 30 điểm, cờ reveal **BẬT**; người thi chính A bị chấm **Sai** nên cửa sổ cướp mở; ghế B giành được quyền và trả lời,
**When** admin chấm **Huỷ kết quả** cho B — đây là phán quyết **khép câu**,
**Then** câu **đã khép** *(không còn ai được trả lời, không mở lại cửa sổ cướp)* nhưng server **không tự đẩy** đáp án tới thí sinh, khán giả hay lớp phủ; đáp án chỉ xuất hiện nếu admin **mở tay**; và điểm của A cùng của B không đổi so với trước cú bấm ngoài đúng hệ quả của phán quyết Huỷ kết quả.

**AC-072** — *US*: US-007 · *FR*: FR-058, FR-117 · *GR*: GR-037 §Cấm · INV-017
**Given** một câu đang mở, chưa khép, một máy thí sinh đã kết nối và một kênh lớp phủ đang chạy,
**When** bắt toàn bộ gói tin gửi tới hai kênh đó và soi bộ nhớ của trang thí sinh,
**Then** **không** tìm thấy đáp án của câu đang mở ở bất kỳ đâu — hệ thống không đẩy đáp án sớm rồi ẩn bằng một cờ hiển thị phía client.

**AC-073** — *US*: US-007 · *FR*: FR-052 · *GR*: GR-037 §ghi chú bước (2)
**Given** một vai tuỳ biến **không** mang tên *Quản trị* hay *MC* nhưng **có** permission đọc đáp án của câu đang chạy, và một vai tuỳ biến mang tên gợi nhớ tới quản trị nhưng **không** có permission đó,
**When** cả hai phiên yêu cầu xem đáp án của một câu chưa khép,
**Then** phiên thứ nhất **được** trả đáp án và phiên thứ hai **không** — cửa kiểm hỏi permission, không hỏi tên vai.

**AC-074** — *US*: US-007 · *FR*: FR-051, FR-061 · *GR*: GR-037 §Bấm trùng, §Không đổi gì
**Given** một phiên giữ permission đọc đáp án,
**When** phiên đó xem đáp án của cùng một câu ba lần liên tiếp, và sau đó câu được công bố ra khán giả,
**Then** ba dòng nhật ký thao tác được ghi, trạng thái trận và điểm **không** đổi, và sau khi đã công bố thì engine **không tự** thu đáp án lại — chỉ admin đóng hiển thị thủ công được.

**AC-075** — *US*: US-007, US-009 · *FR*: FR-057 · *GR*: GR-037 C9, GR-012
**Given** một vòng VCNV đang chạy với cờ reveal **BẬT**, nhiều câu hàng ngang đã được chấm xong,
**When** kiểm xem đáp án **Chướng ngại vật** *(từ khoá của bộ)* có được công bố theo cùng cơ chế không,
**Then** đáp án Chướng ngại vật **không** đi theo cơ chế câu khép; nó chỉ lộ khi có người giải đúng hoặc khi admin bấm công bố.

**AC-076** — *US*: US-007 · *FR*: FR-059 · *GR*: GR-037 §Điều kiện
**Given** một phiên thí sinh và một kênh khán giả, một câu chưa khép,
**When** một yêu cầu xem đáp án được gửi thẳng tới server từ hai kênh đó,
**Then** server **im lặng** từ chối — không trả đáp án và không tiết lộ qua thông điệp lỗi rằng đáp án tồn tại hay câu đang ở trạng thái nào.

#### Nhóm H — Vòng Khởi động (US-008)

**AC-077** — *US*: US-008 · *FR*: FR-062 · *GR*: GR-001 C1
**Given** Khởi động lượt riêng, lượt đã khoá vào ghế A ở vị trí 2 từ trước câu đầu tiên, đây là câu thứ 3 của A, câu đã hiển thị và admin đã bấm start timer, mode sân khấu nên A đọc đáp án,
**When** MC xác nhận đúng và admin bấm **Đúng**,
**Then** ghế A nhận **+10**, một sự kiện điểm được sinh, câu chuyển sang *đã chấm*, điểm ba ghế còn lại không đổi, và câu đã dùng **không** trả lại kho.

**AC-078** — *US*: US-008 · *FR*: FR-062 · *GR*: GR-001 C2
**Given** cùng tình huống AC-077,
**When** admin bấm **Sai**,
**Then** ghế A nhận **0** và **không bị trừ** — lượt riêng không có hình phạt — một sự kiện điểm giá trị 0 vẫn được sinh, và câu chuyển sang *đã chấm*.

**AC-079** — *US*: US-008 · *FR*: FR-062, FR-063 · *GR*: GR-001 C3, GR-002 §Biên
**Given** cùng tình huống AC-077, đồng hồ 3 giây đã chạy hết,
**When** admin vẫn chưa bấm nút phán quyết nào,
**Then** câu ở nguyên trạng thái **chưa chấm**, không sự kiện điểm nào được sinh, nút chấm **không** bị đồng hồ khoá, và admin vẫn bấm được bất cứ lúc nào sau đó.

**AC-080** — *US*: US-008 · *FR*: FR-062 · *GR*: GR-001 C4
**Given** vòng Khởi động lượt riêng đã bị **bỏ**, một câu của vòng đó nằm trong lịch sử,
**When** admin thử bấm **Đúng** cho câu đó,
**Then** hệ thống hiện toast *invalid state*, thao tác **không thực hiện được** và **không ép được**, và **không** sự kiện nào được sinh cho vòng đã bỏ.

**AC-081** — *US*: US-008 · *FR*: FR-063 · *GR*: GR-002 C1
**Given** Khởi động lượt riêng, ghế A im lặng suốt 3 giây,
**When** admin bấm **Sai**,
**Then** ghế A nhận **0**, không bị trừ, một sự kiện điểm được sinh, và câu chờ admin bấm *"Câu kế tiếp"*; hình phạt −5 **không** xuất hiện vì nó chỉ thuộc lượt chung.

**AC-082** — *US*: US-008 · *FR*: FR-063 · *GR*: GR-002 §Điều kiện, C2
**Given** hai ca ở lượt riêng — một ghế im lặng hết giờ, và một ghế đọc một đáp án sai trong hạn,
**When** admin xử lý cả hai,
**Then** cả hai đi qua **cùng một** thao tác *(bấm Sai)* và cho **cùng một** kết quả 0 điểm; hệ thống **không** có hai đường xử lý phân biệt *"không trả lời"* với *"trả lời sai"*.

**AC-083** — *US*: US-008 · *FR*: FR-062, FR-063 · *GR*: GR-002 C3, C4
**Given** một câu lượt riêng, ghế A trả lời **đúng ở giây thứ 1**; và một ca khác nơi ghế B bắt đầu nói ở giây `2.9` nhưng MC chỉ xác nhận sau mốc 3 giây,
**When** admin bấm **Đúng** ở từng ca,
**Then** ở ca đầu điểm chốt **ngay** và hai giây còn lại mất ý nghĩa, nhưng câu **không tự chuyển** — sang câu tiếp là thao tác riêng của admin; ở ca sau admin **vẫn bấm Đúng được** vì hệ thống không phân xử mốc 3 giây.

**AC-084** — *US*: US-008, US-004 · *FR*: FR-064, FR-065 · *GR*: GR-003 C1, C2, GR-005 C2
**Given** Khởi động lượt chung, admin đã bấm **hiển thị câu hỏi** ở `2.1` giây và MC đang đọc, chưa bấm start timer,
**When** ghế C bấm chuông ở `2.6` giây, rồi admin bấm start timer ở `3.0` giây,
**Then** tín hiệu của C ở `2.6` giây **hợp lệ** — cửa sổ chuông đã mở từ mốc hiển thị — C giành quyền, và đồng hồ **3 giây** suy nghĩ của C tính **từ thời điểm C giành quyền**, không phải từ mốc start timer.

**AC-085** — *US*: US-008, US-005 · *FR*: FR-066 · *GR*: GR-003 C3
**Given** cùng câu như AC-084, ghế C đã giành quyền, cửa sổ vẫn còn,
**When** ghế D bấm chuông,
**Then** tín hiệu của D thành **trơ**: được ghi kèm timestamp làm căn cứ cho admin, **không** đổi người giữ quyền, và **không** mở lại chuông; điểm không đổi vì giành quyền chưa phải phán quyết.

**AC-086** — *US*: US-008 · *FR*: FR-064 · *GR*: GR-003 C4
**Given** Khởi động lượt chung, cửa sổ chuông mở, chưa ai giành quyền,
**When** ghế A và ghế B bấm chuông và server ghi **cùng** một mốc mili-giây,
**Then** hàng đợi tự chọn một người, ngẫu nhiên, **không** ưu tiên theo ghế hay vị trí; cả hai tín hiệu đều được ghi kèm thứ tự đã chọn.

**AC-087** — *US*: US-008 · *FR*: FR-064 · *GR*: GR-003 C5, C6, GR-034 C5
**Given** ghế A đã bấm chuông và bị chấm Sai ở câu hiện tại; và một ca khác nơi cửa sổ chuông đã đóng,
**When** ghế A bấm chuông lần nữa ở ca đầu, và một ghế bất kỳ bấm ở ca sau,
**Then** cả hai trường hợp nút **không hiển thị** và bấm **không phản hồi** ⇒ **không tín hiệu nào được tạo**; trạng thái không đổi.

**AC-088** — *US*: US-008 · *FR*: FR-067 · *GR*: GR-004 C1
**Given** Khởi động lượt chung, ghế C đã giành quyền và trả lời,
**When** admin bấm **Đúng**,
**Then** ghế C nhận **+10** và một sự kiện điểm được sinh; điểm các ghế khác không đổi — lượt chung không có cơ chế chuyển điểm giữa người với người.

**AC-089** — *US*: US-008 · *FR*: FR-067 · *GR*: GR-004 C2
**Given** cùng tình huống AC-088, ghế C đang có 30 điểm,
**When** admin bấm **Sai**,
**Then** ghế C nhận **−5** và còn **25**; một sự kiện điểm âm được sinh.

**AC-090** — *US*: US-008 · *FR*: FR-067 · *GR*: GR-004 C3
**Given** cùng tình huống AC-088,
**When** admin bấm **Huỷ kết quả**,
**Then** câu **không** sinh điểm cho ai, hình phạt **−5 không** được áp, một sự kiện giá trị 0 được sinh, và câu chuyển sang *đã chấm*.

**AC-091** — *US*: US-008 · *FR*: FR-067 · *GR*: GR-004 C4, C5
**Given** ghế C giành quyền rồi **im lặng** hết 3 giây, và ghế C đang có **0** điểm,
**When** admin bấm **Sai**,
**Then** ghế C nhận **−5** và điểm thành **−5** — không có sàn; đây là nhánh phạt riêng, khác hẳn tình huống *không ai bấm chuông*.

**AC-092** — *US*: US-008, US-005 · *FR*: FR-068 · *GR*: GR-004 C6
**Given** một câu lượt chung vừa được chấm **Sai**, trong hàng đợi đang hoạt động còn vài tín hiệu trơ của câu đó,
**When** phán quyết được ghi,
**Then** câu **kết thúc** và chuông **không** mở lại cho câu đó; hàng đợi **đang hoạt động** được xoá; khoá chuông của **mọi** ghế được gỡ để dùng cho câu mới; và **lịch sử tín hiệu, kể cả tín hiệu trơ, không bị xoá**.

**AC-093** — *US*: US-008, US-006 · *FR*: FR-069 · *GR*: GR-005 C1
**Given** Khởi động lượt chung, một câu đã hiển thị và đã start timer, không ghế nào bấm chuông,
**When** hết **3 giây** cửa sổ,
**Then** câu **bị bỏ qua**, khép lại và được đánh dấu **đã dùng**; điểm của **mọi** thí sinh không đổi; và câu bị bỏ qua **không** trả lại kho đề.

**AC-094** — *US*: US-008 · *FR*: FR-069 · *GR*: GR-005 C3
**Given** cùng tình huống AC-093, cửa sổ 3 giây sắp hết,
**When** tín hiệu chuông **duy nhất** được server nhận ở **đúng** mốc `3.000` giây,
**Then** tín hiệu **hợp lệ** *(biên đóng)*, câu **không** bị bỏ qua, và ghế đó được đánh dấu là người giành quyền.

**AC-095** — *US*: US-008, US-006 · *FR*: FR-069 · *GR*: GR-005 C4
**Given** cùng tình huống AC-093, cửa sổ 3 giây mới trôi được 1 giây,
**When** admin bấm **chuyển câu thủ công**,
**Then** câu **bị bỏ qua ngay**, khép lại và đánh dấu **đã dùng**; điểm không đổi.

**AC-096** — *US*: US-008 · *FR*: FR-070 · *GR*: GR-006 C1, C2, C6
**Given** mode nhập liệu, một câu Khởi động đang mở, ghế A chưa gửi gì,
**When** ghế A gửi `"Hà Nội"`, rồi sửa thành `"Huế"`, rồi gửi lại `"Huế"` y hệt — tất cả trước mốc cắt,
**Then** bản được ghi nhận là **`"Huế"`**; nút gửi **không** khoá sau mỗi lần gửi; và **cả ba** lần gửi còn nguyên trong lịch sử.

**AC-097** — *US*: US-008 · *FR*: FR-070 · *GR*: GR-006 C3
**Given** cùng tình huống AC-096, ghế A đang sửa đáp án,
**When** bản sửa cuối được server nhận **đúng** khoảnh khắc admin bấm công bố đáp án,
**Then** bản đó **được ghi nhận** — biên **đóng**.

**AC-098** — *US*: US-008 · *FR*: FR-070 · *GR*: GR-006 C4
**Given** ghế A có một bản hợp lệ trong hạn và gửi thêm một bản **sau** mốc cắt,
**When** admin mở màn chấm,
**Then** **cả hai** bản được giữ, bản quá hạn tô **đỏ**, bản quá hạn **không tự thay** bản hợp lệ, và admin có hai lựa chọn Đúng / Sai.

**AC-099** — *US*: US-008 · *FR*: FR-070 · *GR*: GR-006 C5
**Given** ghế A **chỉ** gửi một bản, và bản đó sau mốc cắt,
**When** admin mở màn chấm,
**Then** bản được giữ và tô **đỏ**, và admin có **ba** lựa chọn Đúng / Sai / **Huỷ kết quả**.

**AC-100** — *US*: US-008 · *FR*: FR-070, FR-119 · *GR*: GR-006 C7, §Không đổi gì
**Given** ghế A đã có bản hợp lệ `"Hà Nội"`,
**When** ghế A gửi tiếp một chuỗi chỉ gồm khoảng trắng,
**Then** bản đó **bị bỏ qua** sau khi trim và **không** ghi đè bản đã có; bản được ghi nhận vẫn là `"Hà Nội"`; và bản rỗng vẫn xuất hiện trong **lịch sử** các bản đã gửi.

**AC-101** — *US*: US-008, US-006 · *FR*: FR-044 · *GR*: GR-001 §Biên, GR-004 §Biên · INV-012
**Given** một ghế vừa được chấm xong câu thứ **6** của lượt riêng; và một câu thứ **12** của lượt chung vừa được chấm xong,
**When** admin xem các nút điều khiển tiếp theo,
**Then** ở cả hai trường hợp nút *"Câu kế tiếp"* đã chuyển thành nút kết thúc lượt hoặc kết thúc vòng — **không** có đường nào rút câu thứ 7 của lượt riêng hay câu thứ 13 của lượt chung.

#### Nhóm I — Vòng Vượt chướng ngại vật (US-009)

**AC-102** — *US*: US-009 · *FR*: FR-071, FR-072 · *GR*: GR-007 C1
**Given** vòng VCNV vừa mở, cả 5 ô ở trạng thái *chờ*, người tới lượt là vị trí số 1, mode nhập liệu,
**When** ghế ở vị trí 1 chọn hàng ngang 3, xác nhận dialog trên máy mình, và admin bấm **Yes**,
**Then** hàng ngang 3 được đưa ra, lượt chọn của ghế đó đánh dấu **đã dùng**, và ô tương ứng chuyển sang trạng thái *đã hỏi*.

**AC-103** — *US*: US-009 · *FR*: FR-071 · *GR*: GR-007 C3, GR-010 C2
**Given** người tới lượt chọn tiếp theo là ghế ở vị trí 2, nhưng ghế đó **đã bị loại** khỏi vòng vì giải sai Chướng ngại vật và **chưa** dùng lượt chọn,
**When** hệ thống xác định người tới lượt,
**Then** lượt **dồn sang vị trí tiếp theo** *(vị trí 3)*, và ghế bị loại không được đề xuất lượt.

**AC-104** — *US*: US-009 · *FR*: FR-071 · *GR*: GR-007 C4
**Given** vị trí 4 vừa dùng xong lượt chọn của mình, nhưng vẫn còn **1** hàng ngang chưa được chọn vì vị trí 2 đã bị loại trước khi dùng lượt,
**When** hệ thống xác định người tới lượt,
**Then** lượt **quay lại vị trí số 1** — đây là ngoại lệ tường minh của quy tắc *"tối đa 1 lượt"*, chỉ áp khi đã có người bị loại.

**AC-105** — *US*: US-009 · *FR*: FR-072 · *GR*: GR-007 C6
**Given** vòng VCNV ở **mode sân khấu**, một hàng ngang chưa mở, một ghế đang tới lượt,
**When** ghế đó thử chọn hàng ngang trên máy mình, và một client tự chế gửi thẳng tín hiệu chọn tới server,
**Then** máy thí sinh **không có nút chọn** nào để bấm, và server **từ chối** tín hiệu tự chế kèm một dòng nhật ký thao tác; chỉ **admin** click chọn được.

**AC-106** — *US*: US-009 · *FR*: FR-072 · *GR*: GR-007 C7
**Given** vòng VCNV ở **mode nhập liệu**, ghế A đang tới lượt,
**When** ghế A click hàng ngang 2 nhiều lần liên tiếp,
**Then** click đầu mở **dialog xác nhận** trên máy A; sau khi A xác nhận, nút chọn của A **khoá** ⇒ **đúng một** tín hiệu rời máy A; admin **không** chọn thay được ở mode này.

**AC-107** — *US*: US-009 · *FR*: FR-072 · *GR*: GR-007 C5
**Given** hàng ngang 1 đã được mở ở lượt trước,
**When** ghế đang tới lượt thử trỏ vào hàng ngang 1,
**Then** hàng ngang 1 **không còn là mục tiêu chọn được** — nút không render, bấm không phản hồi — và trạng thái không đổi.

**AC-108** — *US*: US-009 · *FR*: FR-072 · *GR*: GR-007 C8
**Given** vòng VCNV mode nhập liệu, người tới lượt là ghế A, nhưng ghế B phát một tín hiệu chọn hàng ngang,
**When** tín hiệu của B tới server,
**Then** tín hiệu vào hàng đợi, hệ thống **cảnh báo** cho admin rằng đây là tín hiệu sai lượt, và admin quyết Yes/No — hệ thống **không chặn cứng**.

**AC-109** — *US*: US-009 · *FR*: FR-073 · *GR*: GR-008 C1
**Given** hàng ngang 2 đã được đưa ra, thời gian suy nghĩ 15 giây đã hết, admin đã bấm hiển thị bài làm,
**When** admin chấm **Đúng** cho đúng một thí sinh,
**Then** thí sinh đó nhận **+10**, miếng ghép số 2 **mở**, và một sự kiện điểm được sinh cho ghế đó.

**AC-110** — *US*: US-009 · *FR*: FR-073 · *GR*: GR-008 C2
**Given** cùng tình huống AC-109 với 3 thí sinh gõ cùng một đáp án đúng,
**When** admin chấm **Đúng** cho cả ba,
**Then** **mỗi** người nhận **+10** — không có giới hạn số người hưởng điểm — và miếng ghép mở; ba sự kiện điểm riêng được sinh, mỗi sự kiện cho một ghế.

**AC-111** — *US*: US-009 · *FR*: FR-073 · *GR*: GR-008 C3
**Given** cùng tình huống AC-109 nhưng **không ai** trong 4 ghế trả lời đúng,
**When** admin chấm Sai cho cả bốn,
**Then** cả bốn nhận **0** và **không bị trừ**, miếng ghép **không** mở, nhưng ô hàng ngang 2 **vẫn ở trạng thái *đã hỏi*** — trạng thái đó không phụ thuộc việc miếng ghép có mở hay không.

**AC-112** — *US*: US-009 · *FR*: FR-073 · *GR*: GR-008 C4
**Given** cùng tình huống AC-109 với đúng **1** trong 4 người được chấm Đúng,
**When** admin chốt câu,
**Then** miếng ghép **mở** — ngưỡng là **≥1**, không phải một con số cụ thể — và người đúng nhận +10 trong khi ba người còn lại nhận 0.

**AC-113** — *US*: US-009, US-007 · *FR*: FR-074 · *GR*: GR-008 §Điều kiện, C6
**Given** một câu hàng ngang đang chạy; đồng hồ 15 giây chạm hạn; và ghế D **đã bị loại** khỏi vòng,
**When** đồng hồ hết giờ, và ghế D thử gõ đáp án,
**Then** hết giờ **chỉ khoá ô nhập** và **không** tự lộ thứ gì — bài làm của từng thí sinh chỉ hiện khi admin bấm hiển thị; và ô nhập của ghế D **không hiển thị**, thao tác của D không phản hồi.

**AC-114** — *US*: US-009, US-005 · *FR*: FR-038 · *GR*: GR-008 C9 · INV-013
**Given** một câu hàng ngang đang chạy, đồng hồ 15 giây còn 8 giây, chưa ai được xem bài của ai,
**When** ghế B bấm *"Mở chướng ngại vật"*,
**Then** tín hiệu được **ghi nhận ngay** vào hàng đợi chặn, **đồng hồ vẫn chạy** bình thường, và **cả** đáp án chuẩn **lẫn** bài làm của những người khác đều **chưa** hiển thị.

**AC-115** — *US*: US-009 · *FR*: FR-073 · *GR*: GR-008 C7
**Given** một câu hàng ngang, ghế A gửi một bản trong hạn và một bản **sau** mốc 15 giây; ghế B **chỉ** gửi sau mốc,
**When** admin mở màn chấm,
**Then** với ghế A cả hai bản được giữ, bản quá hạn tô **đỏ**, admin có hai lựa chọn Đúng / Sai; với ghế B admin có thêm lựa chọn thứ ba **Huỷ kết quả**.

**AC-116** — *US*: US-009 · *FR*: FR-073 · *GR*: GR-008 C8
**Given** một câu hàng ngang với hạn chót 15 giây,
**When** một bản gửi được server nhận ở **đúng** mốc `15.000` giây,
**Then** bản đó **được ghi nhận như bản hợp lệ** — biên **đóng** — và được chấm bình thường.

**AC-117** — *US*: US-009 · *FR*: FR-075 · *GR*: GR-008 C10, §Thứ tự đánh giá
**Given** một câu hàng ngang mà một thí sinh gõ đáp án lệch chính tả nhỏ so với đáp án chuẩn,
**When** admin xử lý câu,
**Then** máy **chỉ** tô ký tự khác và admin tự đánh giá; và thứ tự xử lý của câu là tất định ba bước — chấm **toàn bộ** thí sinh trước, rồi xác định miếng ghép có mở không bằng phép OR trên tập kết quả, rồi mới đóng câu.

**AC-118** — *US*: US-009 · *FR*: FR-076, FR-077 · *GR*: GR-009 C1 → C5, GR-034
**Given** năm ca độc lập của một vòng VCNV — chưa hàng nào được hỏi, đã hỏi 2 hàng, đã hỏi 3 hàng, đã hỏi 4 hàng nhưng **chưa** đưa gợi ý cuối, và sau khi gợi ý cuối đã đưa ra,
**When** ở mỗi ca một ghế click nút *"Mở chướng ngại vật"*, admin xác nhận và chấm **Đúng**,
**Then** ghế đó nhận lần lượt **+60 · +50 · +40 · +30 · +20** và vòng kết thúc; nút này chỉ nhận **click chuột** *(không phím tắt nào)*, tự khoá khi bấm, và mỗi ghế chỉ phát được **một** tín hiệu cho cả vòng.

**AC-119** — *US*: US-009 · *FR*: FR-078 · *GR*: GR-009 C7, §Thứ tự đánh giá
**Given** đúng **1** hàng ngang đã được hỏi, ghế B bấm *"Mở chướng ngại vật"* và tín hiệu vào hàng đợi chặn,
**When** admin để tín hiệu chờ một lúc rồi mới bấm Yes, sau đó mới chấm,
**Then** trong lúc tín hiệu chờ duyệt **không hàng ngang nào mở thêm được**; băng chốt ở **50** theo trạng thái tại **mốc admin xác nhận**; và thứ tự là tất định: xác nhận tín hiệu → chốt số hàng đã hỏi → phán quyết.

**AC-120** — *US*: US-009 · *FR*: FR-077 · *GR*: GR-009 C9, C10
**Given** ghế D **đã bị loại** khỏi vòng; và một ca khác nơi ghế A **đã giải đúng** Chướng ngại vật,
**When** ghế D thử bấm *"Mở chướng ngại vật"* ở ca đầu, và một ghế bất kỳ thử bấm ở ca sau,
**Then** cả hai trường hợp nút **không hiển thị** và bấm **không phản hồi**; không tín hiệu nào được tạo và trạng thái không đổi.

**AC-121** — *US*: US-009 · *FR*: FR-079 · *GR*: GR-009 C12
**Given** chưa hàng ngang nào được hỏi, băng điểm đang là **60**,
**When** admin **đánh dấu đã hỏi** một ô bằng tay,
**Then** băng tụt xuống **50** y như một lượt hỏi thật, **không ai** được cộng điểm từ thao tác này, ô chuyển sang *đã hỏi*, và nhật ký thao tác ghi rõ đây là thao tác **do admin** để phân biệt với do luồng.

**AC-122** — *US*: US-009 · *FR*: FR-079 · *GR*: GR-009 C13
**Given** một ô đang ở trạng thái *đã hỏi*, băng điểm đang là **50**,
**When** admin **lộ đáp án** ô đó *(đặt ô sang trạng thái mở)*,
**Then** băng điểm **không đổi**, vẫn là **50** — biến đếm là *đã hỏi*, không phải *đã lộ* — nhờ vậy mở tay trọn một ô chỉ tính **một lần**.

**AC-123** — *US*: US-009 · *FR*: FR-082 · *GR*: GR-009 C14
**Given** ghế B vừa bấm *"Mở chướng ngại vật"* và tín hiệu đang **chờ duyệt**, đồng thời một câu hàng ngang đang chạy,
**When** ghế B gõ và gửi đáp án cho câu hàng ngang đó,
**Then** thao tác **được phép bình thường** — hệ thống **không** khoá quyền trả lời hàng ngang của B trong lúc chờ duyệt; nếu admin sau đó bấm **No** cho tín hiệu của B thì B **không mất gì**.

**AC-124** — *US*: US-009 · *FR*: FR-080 · *GR*: GR-009 C15, §Không đổi gì
**Given** một tình huống ngoài luật cần bù điểm cho một ghế ở vòng VCNV,
**When** admin xử lý bằng điều chỉnh điểm thủ công, và song song rà soát mọi thao tác mở/đóng bằng tay đã thực hiện trong vòng,
**Then** điều chỉnh sinh một sự kiện có tên người bấm và hoàn nguyên được; và **không** thao tác mở/đóng bằng tay nào sinh điểm cho ai — điểm ở VCNV chỉ đến từ ba đường: câu hàng ngang chấm Đúng, tín hiệu Chướng ngại vật chấm Đúng, và điều chỉnh thủ công.

**AC-125** — *US*: US-009 · *FR*: FR-081 · *GR*: GR-009 C6, GR-010 C1 · INV-019
**Given** ghế A đã kiếm được **+10** từ một câu hàng ngang trước đó và vừa bấm *"Mở chướng ngại vật"*, admin đã xác nhận tín hiệu,
**When** admin chấm **Sai**,
**Then** ghế A bị đặt cờ **bị loại khỏi vòng VCNV**, **không** bị trừ điểm, và **+10** đã kiếm được **giữ nguyên**; ghế A vẫn thi các vòng sau bình thường và vẫn có thể thắng trận.

**AC-126** — *US*: US-009 · *FR*: FR-076 · *GR*: GR-009 C11
**Given** một yêu cầu tạo hoặc sửa contest v1 đặt số hàng ngang bằng 6, gửi qua giao diện và gửi thẳng tới server,
**When** hệ thống xử lý yêu cầu,
**Then** giao diện **không hiện** lựa chọn nào khác 4 và server **từ chối** yêu cầu ghi; không có băng điểm nào cho 6 hàng ngang được suy diễn ra.

**AC-127** — *US*: US-009 · *FR*: FR-081 · *GR*: GR-010 C3, C4
**Given** ghế A vừa bị loại theo AC-125,
**When** ghế A tiếp tục thao tác trên màn thi đấu của mình,
**Then** máy ghế A **không hiển thị gì** cho các thao tác của vòng và bấm **không phản hồi**; điểm của ghế A và của mọi ghế khác **không đổi**.

**AC-128** — *US*: US-009 · *FR*: FR-083 · *GR*: GR-011 C1
**Given** cả **4** hàng ngang đã được **hỏi** và chưa ai giải đúng Chướng ngại vật, băng điểm đang là **30**,
**When** admin đưa ra gợi ý cuối ở ô trung tâm,
**Then** giai đoạn chuyển sang pha cuối và băng điểm hạ xuống **20**.

**AC-129** — *US*: US-009 · *FR*: FR-083 · *GR*: GR-011 C2
**Given** gợi ý cuối đã đưa ra, các ghế chưa bị loại đã gõ đáp án cho câu ô trung tâm,
**When** admin chấm **Đúng** cho một ghế,
**Then** ghế đó nhận **+10** và ô trung tâm chuyển sang *mở*; một sự kiện điểm được sinh.

**AC-130** — *US*: US-009 · *FR*: FR-083 · *GR*: GR-011 C3
**Given** cùng tình huống AC-129,
**When** admin chấm **Sai**,
**Then** ô trung tâm **không** mở, không sinh điểm, và băng điểm **vẫn là 20** — mốc 20 gắn với việc **gợi ý cuối đã đưa ra**, không phụ thuộc kết quả câu ô trung tâm.

**AC-131** — *US*: US-009 · *FR*: FR-083, FR-085 · *GR*: GR-011 C4, C7, §Thứ tự đánh giá
**Given** admin đã chấm câu ô trung tâm và đã mở ô trung tâm,
**When** admin mở cửa sổ **15 giây** để giải Chướng ngại vật, rồi một ghế bấm và được chấm Đúng trong cửa sổ; và ở một ca khác hết 15 giây không ai giải,
**Then** ca đầu ghế đó nhận **+20** và vòng kết thúc; ca sau vòng khép lại; và cửa sổ mở đúng tại mốc thứ ba của chuỗi *(chấm → mở ô → mở cửa sổ)*, đóng đúng 15 giây sau mốc đó, chung cho mọi thí sinh còn quyền nên **không** phụ thuộc tốc độ thao tác của admin.

**AC-131a** — *US*: US-009 · *FR*: FR-083a, FR-073, FR-083 · *GR*: GR-008, GR-011
**Given** một contest vừa bấm *"Áp dụng luật 2026"* nên giá trị cấu hình thời gian của vòng VCNV đang là **15 giây**; sau đó admin đổi giá trị đó thành **20 giây** rồi bắt đầu một trận mới,
**When** trận chạy trọn vòng VCNV — một câu hàng ngang, câu ô trung tâm, và cửa sổ giải Chướng ngại vật sau gợi ý cuối,
**Then** **cả ba** cửa sổ đều dài **20 giây**; người soạn đề không phải khai thời lượng riêng cho câu ô trung tâm ở bất kỳ đâu trong kho đề; và không màn hình hay đường xử lý nào hiện con số 15 như một hằng số cố định.

**AC-132** — *US*: US-009 · *FR*: FR-084 · *GR*: GR-011 C5
**Given** vòng VCNV ở **mode sân khấu**, gợi ý cuối đã đưa ra, hai ghế đã bị loại và hai ghế còn quyền,
**When** câu ô trung tâm được mở,
**Then** **cả hai** ghế chưa bị loại đều trả lời được, **không** theo lượt, và câu ô trung tâm **luôn gõ máy** dù contest đang ở mode sân khấu.

**AC-133** — *US*: US-009 · *FR*: FR-083 · *GR*: GR-011 C6
**Given** cả 4 hàng ngang đã được **hỏi** nhưng chỉ **2** miếng ghép mở vì hai hàng ngang không ai trả lời đúng,
**When** hệ thống xét điều kiện đưa ra gợi ý cuối,
**Then** giai đoạn gợi ý cuối **vẫn** tới được — điều kiện là *hàng ngang đã được hỏi*, **không** phải *miếng ghép đã mở*.

**AC-134** — *US*: US-009 · *FR*: FR-079 · *GR*: GR-008 C5, GR-011 C8, GR-012 C5
**Given** ba nút một chiều đã được bấm một lần: mở miếng ghép, mở ô trung tâm, và công bố Chướng ngại vật,
**When** mỗi nút được bấm lần thứ hai, hoặc một yêu cầu trùng tới server do trễ mạng,
**Then** nút đã tự tắt nên không có lần bấm thứ hai ở giao diện, và server **bỏ qua** lệnh trùng; trạng thái không đổi.

**AC-135** — *US*: US-009 · *FR*: FR-086 · *GR*: GR-012 C1, GR-010 C5
**Given** ba ghế đã bị loại khỏi vòng VCNV, còn đúng một ghế chưa bị loại,
**When** ghế cuối cùng đó giải **sai** Chướng ngại vật,
**Then** vòng **kết thúc** và **không ai** được điểm Chướng ngại vật; trận về trạng thái nghỉ.

**AC-136** — *US*: US-009 · *FR*: FR-086 · *GR*: GR-012 C2, C3
**Given** vòng VCNV vừa kết thúc theo AC-135, vẫn còn miếng ghép chưa mở,
**When** ở một ca admin bấm **công bố** và ở ca khác admin **không** bấm gì,
**Then** ca đầu mọi miếng ghép mở và Chướng ngại vật hiện cho khán giả; ca sau **không** phải trạng thái tắc — admin vẫn kết thúc vòng được bình thường.

**AC-137** — *US*: US-009, US-006 · *FR*: FR-041, FR-086 · *GR*: GR-012 C4
**Given** vòng VCNV kết thúc theo AC-135 khi vẫn còn **2** hàng ngang chưa bao giờ được chọn,
**When** vòng khép lại,
**Then** hai hàng ngang đó bị bỏ, và câu của chúng **chưa hiển thị cho ai ⇒ chưa tiêu ⇒ trả lại kho**; cờ đã-dùng **không** được đặt cho hai câu đó.

**AC-138** — *US*: US-009 · *FR*: FR-086 · *GR*: GR-012 C6, C7, C8
**Given** ba tình huống: một tín hiệu tới **sau** khi vòng đã kết thúc; một thời điểm còn đúng **1** người chưa bị loại; và một vòng kết thúc khi mọi ghế đã kiếm được điểm hàng ngang từ trước,
**When** từng tình huống diễn ra,
**Then** tín hiệu muộn không có nút để bấm và không phản hồi; với **1** người còn lại thì quy tắc kết thúc vòng **chưa áp dụng** và vòng chạy tiếp với người đó; và điểm hàng ngang đã ghi của **mọi** thí sinh **giữ nguyên**, mang sang vòng sau kể cả khi cả sân bị loại.

**AC-139** — *US*: US-009 · *FR*: FR-081 · *GR*: GR-010 §Đồng thời
**Given** đồng hồ của một câu hàng ngang đang chạy, ghế B đã gửi một đáp án cho câu đó ở giây thứ 4, và một tín hiệu Chướng ngại vật của B đang chờ duyệt,
**When** admin xác nhận tín hiệu rồi chấm **Sai** cho B ở giây thứ 9,
**Then** đồng hồ hàng ngang **chạy tiếp bình thường** — không dừng, không kéo dài — câu hàng ngang tiếp tục cho những người còn quyền, và bản B đã gửi **trước** mốc phán quyết Sai **vẫn được chấm bình thường**; máy **không** tự huỷ nó.

#### Nhóm J — Vòng Tăng tốc (US-010)

**AC-140** — *US*: US-010 · *FR*: FR-087, FR-088 · *GR*: GR-013 C1
**Given** một câu Tăng tốc với 4 ghế A·B·C·D; bản cuối hợp lệ của A được server nhận ở `100` ms, B ở `150` ms, C ở `200` ms, D không gửi gì,
**When** admin chấm Đúng cho A, B, C và chốt câu,
**Then** kết quả là **A +40 · B +30 · C +20 · D 0**, và toàn bộ nằm trong **một** sự kiện điểm duy nhất cho cả bảng.

**AC-141** — *US*: US-010 · *FR*: FR-087 · *GR*: GR-013 C3, C4
**Given** hai câu Tăng tốc: câu thứ nhất chỉ có **1** người được chấm Đúng, câu thứ hai **0** người được chấm Đúng,
**When** admin chốt từng câu,
**Then** ở câu thứ nhất người đó nhận **+40**; ở câu thứ hai **không ai** được cộng điểm và **không** sự kiện điểm nào được sinh cho câu đó.

**AC-142** — *US*: US-010 · *FR*: FR-088 · *GR*: GR-013 C2, GR-014 C1, C2
**Given** một câu Tăng tốc với A và B cùng mốc `5420` ms, C ở `7100` ms, D ở `9800` ms,
**When** admin chấm Đúng cả bốn; và ở một ca khác chấm Đúng ba người đầu còn D **Sai**,
**Then** ca đầu cho **40 · 40 · 20 · 10** và ca sau cho **40 · 40 · 20 · 0** — hai người ở bậc 1 nên bậc 2 bị **nhảy qua** và C vào bậc 3.

**AC-143** — *US*: US-010 · *FR*: FR-088 · *GR*: GR-014 C3, C4
**Given** hai cấu hình mốc: **(i)** A, B, C cùng `5420` ms và D ở `7100` ms; **(ii)** A ở `3200` ms, B và C cùng `5420` ms, D ở `7100` ms,
**When** admin chấm Đúng cả bốn ở từng cấu hình,
**Then** cấu hình (i) cho **40 · 40 · 40 · 10** *(ba người bậc 1 ⇒ D vào bậc 4)*; cấu hình (ii) cho **40 · 30 · 30 · 10** *(hai người bậc 2 ⇒ D vào bậc 4)*.

**AC-144** — *US*: US-010 · *FR*: FR-088 · *GR*: GR-014 C5, C6
**Given** hai cặp mốc: A `5423` ms vs B `5429` ms, và A `5420` ms vs B `5421` ms,
**When** admin chấm Đúng cả hai người ở từng cặp,
**Then** **không** cặp nào là hoà — chênh 6 ms và chênh **1 ms** đều phân định được ở độ phân giải mili-giây — nên hai người nhận `40` và `30`.

**AC-145** — *US*: US-010 · *FR*: FR-087 · *GR*: GR-013 C6, C7
**Given** một câu Tăng tốc mà ghế D trả lời nhưng bị chấm **Sai**; và một câu Tăng tốc khác thuộc một vòng **đã bị bỏ**,
**When** admin chốt câu thứ nhất, và thử bấm chấm ở câu thứ hai,
**Then** ghế D nhận **0** và **không giữ chỗ** trong thang — thang tụt bậc khi có người **đúng**, không phải khi có người **trả lời**; và thao tác ở câu thuộc vòng đã bỏ hiện toast *invalid state*, không thực hiện được.

**AC-146** — *US*: US-010 · *FR*: FR-016 · *GR*: GR-013 §Bấm trùng
**Given** một câu Tăng tốc, admin đã bấm Đúng cho ghế A,
**When** admin bấm Đúng lần nữa cho cùng ghế A và cùng câu,
**Then** chỉ **một** sự kiện điểm tồn tại cho câu đó; và **không** có đường nào đổi phán quyết của một người sau khi đã chốt câu — sửa sai đi qua điều chỉnh điểm thủ công.

**AC-147** — *US*: US-010 · *FR*: FR-089 · *GR*: GR-015 C1
**Given** một câu Tăng tốc, ghế A gửi `"Hà Nội"` ở `100` ms, `"Hanoi"` ở `150` ms, rồi `"Hà Nội"` ở `200` ms,
**When** hết giờ và admin chấm,
**Then** bản được ghi nhận là `"Hà Nội"` và mốc xếp hạng là **100** — bản ở `200` ms y hệt bản ở `100` ms nên **không** cập nhật mốc.

**AC-148** — *US*: US-010 · *FR*: FR-089 · *GR*: GR-015 C2
**Given** cùng ghế A gửi `"Hà Nội"` ở `100` ms, `"Hanoi"` ở `150` ms, rồi `"Hà Nội khác"` ở `200` ms,
**When** hết giờ và admin chấm,
**Then** bản được ghi nhận là `"Hà Nội khác"` và mốc xếp hạng là **200** — nội dung mới nên cập nhật cả nội dung lẫn mốc.

**AC-149** — *US*: US-010 · *FR*: FR-089 · *GR*: GR-015 C3, C4
**Given** hai chuỗi gửi: **(i)** `"Hà Nội"` ở `100` ms rồi `"   "` ở `150` ms; **(ii)** `"   "` ở `100` ms rồi `"Hà Nội"` ở `150` ms,
**When** hết giờ và admin chấm,
**Then** chuỗi (i) ghi nhận `"Hà Nội"` với mốc **100**, chuỗi (ii) ghi nhận `"Hà Nội"` với mốc **150** — bản rỗng sau trim bị bỏ qua ở cả hai vị trí và **không** ghi đè bản hợp lệ.

**AC-150** — *US*: US-010 · *FR*: FR-090, FR-063 · *GR*: GR-015 C5, C6, §Biên
**Given** ba ghế: A có bản hợp lệ trước hạn và một bản sau hạn; B **chỉ** gửi sau hạn; C **không gửi gì**,
**When** admin xử lý câu,
**Then** A dùng bản hợp lệ cuối cho xếp hạng và bản quá hạn tô **đỏ** để admin quyết Đúng / Sai; B được tô đỏ và admin có thêm **Huỷ kết quả**, và nếu admin công nhận bản của B thì **cả bảng xếp hạng của câu tính lại**; C không có bản nào để chấm nên admin bấm **Sai** — *không trả lời* và *trả lời sai* là cùng một thao tác.

#### Nhóm K — Vòng Về đích (US-011)

**AC-151** — *US*: US-011 · *FR*: FR-091 · *GR*: GR-016 C1, C2, C3, C4
**Given** bốn cấu hình bảng điểm ở đầu một lượt Về đích: **(i)** A 110 / B 90 / C 80 / D 70; **(ii)** A 110 ở vị trí 2 và B 110 ở vị trí 1; **(iii)** sau lượt 1, A còn 90 và B có 110; **(iv)** cả bốn cùng 100 với vị trí 1·2·3·4,
**When** hệ thống tính khuyến nghị người thi lượt kế,
**Then** lần lượt khuyến nghị **A · B · B · A** — điểm cao nhất đi trước, hoà điểm thì **số vị trí nhỏ nhất** đi trước, và bảng được **tính lại sau mỗi lượt hoàn thành**.

**AC-152** — *US*: US-011 · *FR*: FR-091 · *GR*: GR-016 C5 · INV-020
**Given** bảng điểm có A ở 110 là cao nhất, hệ thống đang khuyến nghị A,
**When** admin chọn cho B *(90 điểm)* thi trước,
**Then** hệ thống hiện **dialog cảnh báo** nêu rõ luật bị lệch, admin bấm Yes thì lượt của B **vẫn thực hiện** — hệ thống **không chặn cứng**.

**AC-153** — *US*: US-011 · *FR*: FR-092 · *GR*: GR-017 C1, C2
**Given** một lượt Về đích vừa chốt người thi là ghế A, chưa chọn gói, mức điểm cấu hình là {20, 30},
**When** gói **20/20/20** được chốt ở một ca và gói **20/30/30** ở ca khác,
**Then** cả hai được chấp nhận; hệ thống chuẩn bị rút lần lượt 3 câu mức 20, và 1 câu mức 20 cùng 2 câu mức 30.

**AC-154** — *US*: US-011 · *FR*: FR-092 · *GR*: GR-017 C3
**Given** vòng Về đích ở **mode nhập liệu**, tới lượt ghế A nhưng A **chưa chọn** gói,
**When** admin dùng thao tác chọn hộ,
**Then** gói mặc định **20/20/20** được áp và admin **override** được trong dialog trước khi chốt.

**AC-155** — *US*: US-011 · *FR*: FR-092, FR-117 · *GR*: GR-017 C4, C5
**Given** vòng Về đích ở **mode sân khấu**, tới lượt ghế A và A nói gói của mình trên sân khấu,
**When** admin bấm chốt gói theo lời A; và song song một client tự chế gửi thẳng một lựa chọn gói từ máy thí sinh tới server,
**Then** gói của A được chốt theo cú bấm của admin và **không** có mốc *"quá hạn"* nào ở ca này; máy thí sinh **không render** nút chọn gói, và server **từ chối** tín hiệu tự chế.

**AC-156** — *US*: US-011 · *FR*: FR-092 · *GR*: GR-017 C6, C7, §Bấm trùng
**Given** ghế A đã chốt gói 20/20/20, câu đầu tiên của gói đã được rút nhưng **chưa hiển thị**,
**When** A đổi gói sang 20/30/30 **trước** mốc admin bấm hiển thị câu đầu tiên; rồi thử đổi lần nữa **sau** mốc đó,
**Then** lần đổi trước mốc được chấp nhận theo nguyên tắc bản-cuối-thắng *(chọn gói không phải chuông nên không áp cơ chế bấm-xong-thì-tắt)*; lần đổi sau mốc **không** được chấp nhận vì đề đã rời server.

**AC-157** — *US*: US-011 · *FR*: FR-092 · *GR*: GR-017 C8
**Given** vòng Về đích với hai mức điểm cấu hình {20, 30},
**When** một lựa chọn gói gồm **2** mục được gửi, và một lựa chọn gồm 3 mục nhưng có một mức **40**,
**Then** cả hai bị **từ chối ở cả giao diện lẫn server** — số mục phải đúng 3 và mọi mức phải nằm trong tập cấu hình.

**AC-158** — *US*: US-011, US-006 · *FR*: FR-093 · *GR*: GR-017 §Biên
**Given** một trận 4 ghế sắp mở vòng Về đích, kho hiệu dụng có **12** câu mức 20 và **9** câu mức 30,
**When** hệ thống chạy phép kiểm kho tại cửa vào vòng,
**Then** vòng **không mở được** — phép kiểm đòi `3 × 4 = 12` câu ở **cả** mức 20 **và** mức 30 vì nó chạy **trước khi ai chọn gói**; thông điệp nêu rõ thiếu bao nhiêu câu ở mức nào.

**AC-159** — *US*: US-011 · *FR*: FR-094 · *GR*: GR-018 C1
**Given** ghế A đang thi lượt của mình với một câu **20 điểm** đã hiển thị và đã start timer,
**When** admin chấm **Đúng**,
**Then** A nhận **+20**, cửa sổ cướp quyền **không** mở, và câu kết thúc.

**AC-160** — *US*: US-011 · *FR*: FR-094, FR-063 · *GR*: GR-018 C2, C3
**Given** ghế A đang thi một câu **30 điểm**; ở một ca A trả lời sai, ở ca khác A không trả lời gì,
**When** admin chấm **Sai** ở cả hai ca,
**Then** cả hai cho **0 điểm** *(không trừ)* và **mở cửa sổ cướp quyền 5 giây**; hàng đợi cướp quyền **không chặn**.

**AC-161** — *US*: US-011 · *FR*: FR-094 · *GR*: GR-018 C4, C5, §Điều kiện
**Given** ghế A gửi ba bản đáp án cho câu của mình ở `2`, `6` và `11` giây, bản cuối lệch chính tả nhỏ so với đáp án chuẩn, và admin chấm ở giây thứ 18,
**When** admin xử lý câu,
**Then** bản được chấm là **bản CUỐI CÙNG** của A; máy **chỉ** tô ký tự khác và admin tự quyết; kết quả theo **phán quyết**, không theo thời điểm trả lời — Về đích **không** xếp hạng theo tốc độ.

**AC-162** — *US*: US-011 · *FR*: FR-094, FR-100 · *GR*: GR-018 C6, GR-021 C2
**Given** ghế A đã đặt Ngôi sao hy vọng lên một câu **30 điểm** của chính mình,
**When** admin chấm **Sai**,
**Then** A nhận **−30** *(đúng một lần)* và cửa sổ cướp quyền mở; một sự kiện điểm âm được sinh.

**AC-163** — *US*: US-011 · *FR*: FR-095 · *GR*: GR-019 C1, C2, C3
**Given** ba câu thực hành: 20 điểm được chấm **đạt**; 20 điểm hết pha thực hành và bị chấm **không đạt**; 30 điểm được chấm **đạt**,
**When** admin chấm từng câu,
**Then** kết quả lần lượt là **+20** không mở cửa sổ cướp · **0** và mở **cửa sổ bấm chuông 5 giây** · **+30**; *"không đạt"* và *"không thực hành gì"* là cùng một thao tác.

**AC-164** — *US*: US-011, US-007 · *FR*: FR-095, FR-060 · *GR*: GR-019 C4, §Bảo mật
**Given** một câu thực hành **30 điểm**, người thi chính không đạt, ghế B giành được quyền cướp trong cửa sổ 5 giây,
**When** B thực hành và admin chấm,
**Then** B thực hành trong **thời lượng thực hành của người cướp** *(khác thời lượng của người thi chính)*, đạt ⇒ **transfer**, không đạt ⇒ **−½ giá trị câu**; và trong suốt câu, **tiêu chí đạt** chỉ rời server tới phiên giữ permission đọc đáp án, còn **ghi chú dụng cụ** không ra tới khán giả hay thí sinh dù xem được trong kho đề.

**AC-165** — *US*: US-011 · *FR*: FR-095, FR-100 · *GR*: GR-019 C5, C6
**Given** một câu thực hành **20 điểm** mà người thi chính đã đặt Ngôi sao hy vọng,
**When** admin chấm **đạt** ở một ca và **không đạt** ở ca khác,
**Then** ca đầu cho **+40** *(gấp đôi)*; ca sau cho **−20** và mở cửa sổ cướp.

**AC-166** — *US*: US-011 · *FR*: FR-096 · *GR*: GR-019 §Ngoại lệ và cảnh báo
**Given** ba tình huống cấu hình: một câu **ngoài** kho Về đích được đánh cờ *câu thực hành*; một câu thực hành có ghi chú dụng cụ **rỗng**; một câu thực hành có tiêu chí đạt **rỗng**,
**When** hệ thống xử lý từng tình huống,
**Then** tình huống đầu bị chặn **ở kho đề**, không phải lỗi lúc chạy; hai tình huống sau chỉ sinh **cảnh báo** ở cửa vào vòng và **không** chặn cứng — admin vẫn phán quyết được bằng đánh giá của mình.

**AC-167** — *US*: US-011 · *FR*: FR-097 · *GR*: GR-020 C1
**Given** câu **20 điểm** của ghế A, A bị chấm Sai nên cửa sổ cướp 5 giây đang mở,
**When** ghế B bấm chuông sớm nhất và được chấm **Đúng**,
**Then** **transfer** diễn ra: **A −20 · B +20**; điểm của những người không bấm không đổi.

**AC-168** — *US*: US-011 · *FR*: FR-097 · *GR*: GR-020 C2
**Given** câu **30 điểm** của ghế A, A bị chấm Sai, cửa sổ cướp đang mở,
**When** ghế B giành quyền và bị chấm **Sai**,
**Then** B nhận **−15**; A **không** được hoàn lại — A giữ nguyên `0` nếu câu không có Ngôi sao hy vọng, hoặc `−30` nếu có; admin còn lựa chọn **Huỷ kết quả** thay cho Sai nếu không muốn áp hình phạt cho B.

**AC-169** — *US*: US-011 · *FR*: FR-097 · *GR*: GR-020 C3, §Biên
**Given** câu **20 điểm** của ghế A, A bị chấm Sai, cửa sổ cướp mở,
**When** hết **5 giây** mà không ai bấm chuông; và sau đó một ghế thử bấm,
**Then** A giữ nguyên kết quả của mình và **không ai** được cộng; tín hiệu bấm sau cửa sổ **không được tạo**; admin vẫn chấm được sau khi cửa sổ đã đóng vì cửa sổ chỉ chi phối việc **bấm chuông**.

**AC-170** — *US*: US-011 · *FR*: FR-097 · *GR*: GR-020 C4
**Given** ghế B giành được quyền cướp và gửi ba bản: `"Paris"` ở `100` ms, `"London"` ở `150` ms, `"Berlin"` ở `200` ms,
**When** admin chấm B,
**Then** phán quyết áp trên **bản ĐẦU TIÊN** `"Paris"` — ngược với người thi chính vốn tính bản cuối.

**AC-171** — *US*: US-011 · *FR*: FR-098 · *GR*: GR-020 §Giá trị câu LẺ
**Given** một contest cấu hình một mức điểm **lẻ** là 25, người thi chính bị chấm Sai, ghế B cướp và bị chấm Sai,
**When** hệ thống tính hình phạt cho B,
**Then** hình phạt là **−12**, không phải −13 — làm tròn xuống theo **độ lớn** rồi mới gắn dấu âm; hệ thống **không** chặn cấu hình mức lẻ bằng một ràng buộc *"giá trị phải chẵn"*.

**AC-172** — *US*: US-011 · *FR*: FR-099, FR-118 · *GR*: GR-021 C1, C2
**Given** ghế A đã đặt Ngôi sao hy vọng trước mốc hiển thị câu, câu là **20 điểm** ở một ca và **30 điểm** ở ca khác,
**When** admin chấm **Đúng** ở ca đầu và **Sai** ở ca sau,
**Then** ca đầu cho **+40** *(gấp đôi)* và ca sau cho **−30**; các con số này **cứng, không tuỳ chỉnh**.

**AC-173** — *US*: US-011 · *FR*: FR-099 · *GR*: GR-021 C3
**Given** ghế A đã đặt Ngôi sao hy vọng cho lượt này,
**When** một tín hiệu đặt Ngôi sao hy vọng thứ hai của A tới server,
**Then** nút đã disabled ở giao diện nên không có lần bấm thứ hai; server **bỏ qua** tín hiệu trùng và không đổi gì.

**AC-174** — *US*: US-011, US-004 · *FR*: FR-099 · *GR*: GR-021 C6
**Given** admin đã bấm **hiển thị câu hỏi** cho câu đầu tiên trong gói của ghế A, A chưa dùng Ngôi sao hy vọng,
**When** A thử đặt Ngôi sao hy vọng cho câu đó,
**Then** nút **không render** và thao tác **không phản hồi** — cửa sổ đã đóng tại mốc hiển thị — và Ngôi sao hy vọng của A **vẫn tính là chưa dùng**, còn nguyên cho các câu sau.

**AC-175** — *US*: US-011 · *FR*: FR-099 · *GR*: GR-021 C7, C8
**Given** vòng Về đích ở **mode sân khấu**, ghế A nói trên sân khấu rằng mình đặt Ngôi sao hy vọng trước khi câu được mở,
**When** admin bấm đặt Ngôi sao hy vọng cho A qua dialog Yes/No; và song song một client tự chế gửi thẳng tín hiệu đặt Ngôi sao hy vọng từ máy thí sinh,
**Then** cú bấm của admin hợp lệ và **không có cuộc đua ở biên cửa sổ** vì cùng một người bấm cả hai mốc; máy thí sinh **không render** nút, và server **từ chối** tín hiệu tự chế kèm một dòng nhật ký thao tác.

**AC-176** — *US*: US-011 · *FR*: FR-100 · *GR*: GR-021 C5, C5b, C5c, GR-020 C5
**Given** ghế A đặt Ngôi sao hy vọng lên câu **30 điểm** của mình và bị chấm **Sai**, cửa sổ cướp mở,
**When** lần lượt dựng ba nhánh — ghế B cướp **đúng**, ghế B cướp **sai**, và **không ai** cướp,
**Then** ghế A mất **đúng −30** ở **cả ba** nhánh — số học của người thi chính độc lập với hành động của người cướp; B nhận **+30** ở nhánh đầu, **−15** ở nhánh hai, và không đổi ở nhánh ba.

**AC-177** — *US*: US-011 · *FR*: FR-099, FR-100 · *GR*: GR-021 §Điều kiện, C4
**Given** ghế A đã dùng Ngôi sao hy vọng trong một lần chạy vòng Về đích; và ghế B đang là người cướp quyền một câu của A,
**When** vòng Về đích bị **chạy lại**, rồi A thử đặt Ngôi sao hy vọng lần nữa; và B thử đặt Ngôi sao hy vọng lên câu đang cướp,
**Then** sau khi chạy lại vòng, cờ Ngôi sao hy vọng của A **được đặt lại** và A dùng lại được; còn B **không** đặt được lên câu đang cướp — Ngôi sao hy vọng phải đặt **trước** khi câu được đọc, nên ngôi sao của B vẫn còn nguyên cho lượt thi của chính B. Ca *"đặt Ngôi sao hy vọng đúng và đồng thời có người cướp"* **không tồn tại** vì cướp quyền chỉ mở khi người thi chính bị chấm **Sai**.

#### Nhóm L — Vòng Câu hỏi phụ (US-012)

**AC-178** — *US*: US-012 · *FR*: FR-101, FR-118 · *GR*: GR-023 §Điều kiện, §Biên
**Given** một trận vừa vào vòng Câu hỏi phụ với một nhóm hoà gồm 2 ghế; các câu mượn từ kho Về đích khai `timeSeconds` là 15 và 20 giây,
**When** vòng chạy,
**Then** vòng có đúng **3 câu**, mỗi câu đúng **15 giây** suy nghĩ bất kể giá trị khai ở câu gốc; hai con số này **cố định** và không xuất hiện ở bất kỳ cửa cấu hình nào.

**AC-179** — *US*: US-012 · *FR*: FR-102 · *GR*: GR-023 §Điều kiện
**Given** một câu Câu hỏi phụ, đồng hồ 15 giây đang chạy và còn 9 giây,
**When** ghế A bấm chuông và giành được quyền,
**Then** đồng hồ 15 giây **dừng ngay**, không chạy tiếp; và **không** có một đồng hồ trả lời riêng nào được mở cho A sau đó.

**AC-180** — *US*: US-012 · *FR*: FR-103 · *GR*: GR-023 C1
**Given** câu thứ hai của vòng Câu hỏi phụ, ghế C bấm chuông sớm nhất ở giây thứ 7 và trả lời,
**When** admin chấm **Đúng**,
**Then** C **thắng** phân định và giữ vị trí đang tranh; vòng đóng và trận về trạng thái nghỉ; một sự kiện phân định ghi phương thức *trả lời* được sinh; và trận **chưa** đóng sổ — admin phải bấm Chốt trận một lần nữa.

**AC-181** — *US*: US-012 · *FR*: FR-103 · *GR*: GR-023 C2
**Given** câu thứ nhất của vòng, ghế A bấm chuông và trả lời,
**When** admin chấm **Sai**,
**Then** **cả nhóm** sang câu kế; hàng đợi đang hoạt động được reset và câu thứ hai mở; **không** sự kiện điểm nào được sinh.

**AC-182** — *US*: US-012 · *FR*: FR-103 · *GR*: GR-023 C3
**Given** câu thứ nhất của vòng, đồng hồ 15 giây đang chạy,
**When** hết 15 giây mà **không ai** bấm chuông,
**Then** câu đóng, không ai đúng, và vòng sang câu kế.

**AC-183** — *US*: US-012 · *FR*: FR-102 · *GR*: GR-023 §Điều kiện
**Given** ghế A đã bấm chuông và giành quyền nhưng **im lặng** không nói gì,
**When** admin chấm **Sai**,
**Then** cả nhóm sang câu kế và **không** hình phạt nào được áp cho A — vòng này không có chế tài cho việc bấm rồi im, khác hẳn −5 của Khởi động lượt chung.

**AC-184** — *US*: US-012 · *FR*: FR-101 · *GR*: GR-023 §Điều kiện, §Không đổi gì
**Given** một trận có nhóm hoà {A, B} cùng 100 điểm ở vị trí Nhất và ghế C có 90 điểm,
**When** vòng Câu hỏi phụ chạy trọn và A thắng phân định,
**Then** điểm của **mọi** thí sinh **không đổi** — vòng này không cộng cũng không trừ — thứ hạng của C nằm ngoài nhóm hoà **không** bị đụng tới, và kết quả chỉ sắp thứ tự **bên trong** nhóm bằng điểm.

**AC-185** — *US*: US-012, US-005 · *FR*: FR-104 · *GR*: GR-024 C1, C3
**Given** một câu Câu hỏi phụ đã hiển thị nhưng admin **chưa** bấm start timer; và một ca khác nơi 15 giây đã trôi hết,
**When** một ghế thử bấm chuông ở từng ca, và một client tự chế gửi thẳng tín hiệu chuông tới server ở ca đầu,
**Then** cả hai ca nút chuông **chưa/không còn render** nên **không có tín hiệu** nào được tạo; server **từ chối** tín hiệu tự chế và ghi một dòng nhật ký thao tác.

**AC-186** — *US*: US-012, US-004 · *FR*: FR-104 · *GR*: GR-024 C2, §Biên
**Given** một câu Câu hỏi phụ đã hiển thị, admin sắp bấm start timer,
**When** ghế A bấm chuông đúng một mili-giây **sau** mốc start timer,
**Then** tín hiệu **hợp lệ**, vào hàng đợi và được xử lý bình thường; biên đóng ở phía trước — đúng mốc là hợp lệ, một mili-giây trước thì không có nút. Quy tắc này **không** áp cho Khởi động lượt chung, nơi chuông sống từ mốc hiển thị câu.

**AC-187** — *US*: US-012 · *FR*: FR-105 · *GR*: GR-023 §Nguồn đề
**Given** một contest đã chạy trọn bốn vòng; kho Về đích còn dư đúng 12 câu chưa dùng, kho Khởi động còn dư một ít, kho VCNV còn một bộ nguyên vẹn; admin **không** chỉ định câu nào,
**When** vòng Câu hỏi phụ mở và server rút 3 câu,
**Then** cả 3 câu đều rút từ kho **Về đích** — thứ tự ưu tiên là Về đích → Khởi động → VCNV — và phép rút **không** chạm kho VCNV khi hai kho trên còn câu; hệ thống **không** đòi một kho Câu hỏi phụ khai riêng.

**AC-188** — *US*: US-012 · *FR*: FR-105 · *GR*: GR-023 §Nguồn đề
**Given** kho Về đích và kho Khởi động đều đã cạn, kho Tăng tốc còn nhiều câu, kho VCNV còn một bộ nguyên vẹn trong đó có một câu được đánh cờ *câu thực hành*,
**When** server rút đề cho vòng Câu hỏi phụ,
**Then** kho **Tăng tốc không** được dùng làm nguồn ở bất kỳ tình huống nào, và câu **thực hành** bị **loại** khỏi phép rút.

**AC-189** — *US*: US-012 · *FR*: FR-106 · *GR*: GR-023 §Nguồn đề
**Given** một câu mượn từ kho Về đích khai `timeSeconds` là 20 giây và mức điểm 30,
**When** câu đó được dùng làm một câu Câu hỏi phụ,
**Then** thời gian suy nghĩ là **15 giây** *(bỏ qua giá trị khai)*, mức điểm **bị bỏ qua** hoàn toàn vì vòng này không sinh điểm, và phân loại theo vòng gốc không mang ý nghĩa nào.

**AC-190** — *US*: US-012, US-006 · *FR*: FR-107 · *GR*: GR-025 C4
**Given** ba kho nguồn cộng lại chỉ còn **2** câu khả dụng sau khi đã bù cho danh sách đã đặt chỗ,
**When** hệ thống chạy phép kiểm tại cửa vào vòng Câu hỏi phụ,
**Then** vòng **không mở được** — chặn ở cửa vào — và tình huống cạn đề **giữa** vòng không tồn tại.

**AC-190a** — *US*: US-012, US-006 · *FR*: FR-105a, FR-048, FR-107 · *GR*: GR-031 C9, GR-023 §Nguồn đề
**Given** một contest **thật** đã chạy xong một trận official và đã lộ 69 câu, trong đó kho Về đích đã lộ 20 câu và kho Khởi động đã lộ 30 câu; kho chưa lộ vẫn còn hàng trăm câu; nay một trận `practice` chạy trong chính contest đó và có hoà ở vị trí Nhất,
**When** vòng Câu hỏi phụ mở và server rút 3 câu,
**Then** kho khả dụng được dựng **trước** bằng phép đảo — chỉ 69 câu **đã lộ** — rồi thứ tự ưu tiên ba kho mới áp lên tập đó, nên cả 3 câu rút ra đều nằm trong 20 câu Về đích **đã lộ**; phép kiểm cửa vào vòng đếm 3 câu trên tập đã lộ; và **không** câu chưa lộ nào được dùng để bù, kể cả khi tập đã lộ sát ngưỡng.

**AC-191** — *US*: US-012 · *FR*: FR-108 · *GR*: GR-025 C1
**Given** nhóm hoà {A, B}, cả **3** câu đã hỏi xong mà không ai được chấm Đúng,
**When** câu thứ ba đóng,
**Then** server bốc thăm ngẫu nhiên và trình **một** người dưới dạng **đề xuất** cho admin; kết quả chưa có hiệu lực cho tới khi admin xác nhận.

**AC-192** — *US*: US-012 · *FR*: FR-108 · *GR*: GR-025 C2
**Given** kết quả bốc thăm đề xuất ghế B,
**When** admin bấm **Yes**,
**Then** B nhận vị trí đang tranh, một sự kiện phân định ghi phương thức *bốc thăm* được sinh, vòng đóng và trận về trạng thái nghỉ — **chưa** đóng sổ.

**AC-193** — *US*: US-012 · *FR*: FR-108 · *GR*: GR-025 C3, §Bấm trùng
**Given** lần bốc thăm thứ nhất ra ghế A,
**When** admin bấm **Bốc lại** qua dialog Yes/No và lần thứ hai ra ghế B,
**Then** **cả hai** lần là sự kiện thật trong nhật ký append-only; **lần cuối cùng có hiệu lực** và lần trước **không** bị đánh dấu vô hiệu — nó chỉ bị một sự kiện sau ghi đè; thao tác này **không idempotent theo thiết kế**.

**AC-194** — *US*: US-012 · *FR*: FR-109 · *GR*: GR-025 C5, GR-022 C4
**Given** một nhóm hoà mà admin **không** phân định, ví dụ hoà ở một vị trí ngoài phạm vi phân định,
**When** trận đóng sổ,
**Then** các thành viên nhóm đó được ghi **đồng hạng** theo quy tắc xếp hạng cạnh tranh chuẩn, và hạng kế tiếp **nhảy qua** đúng số người đồng hạng.

**AC-195** — *US*: US-012 · *FR*: FR-108, FR-018 · *GR*: GR-023 C4, C5
**Given** vòng Câu hỏi phụ đang chạy; ở một ca cả 3 câu đã hỏi mà chưa ai đúng, ở ca khác admin muốn gỡ một phán quyết Sai đã bấm nhầm,
**When** ca đầu câu thứ ba đóng, và ca sau admin xử lý sai sót,
**Then** ca đầu chuyển sang pha **bốc thăm**; ca sau đường đúng là **bỏ vòng** phân định *(làm được cả trong lúc vòng chạy lẫn ở trạng thái nghỉ trước cú Chốt trận cuối)* — điều chỉnh điểm thủ công **không** dùng được ở đây vì vòng này không sinh điểm.

#### Nhóm M — Mất kết nối và giữ ghế (US-013)

**AC-196** — *US*: US-013 · *FR*: FR-110 · *GR*: GR-036 C1
**Given** ghế A mất kết nối ở giây thứ 10 của một vòng đang chạy, ngưỡng chờ là 120 giây,
**When** A quay lại ở giây thứ 60,
**Then** ghế A được khôi phục trạng thái, banner *đã kết nối lại* hiện, và A thi tiếp bình thường; không hệ quả nào được áp.

**AC-197** — *US*: US-013 · *FR*: FR-111 · *GR*: GR-036 C2
**Given** ghế B mất kết nối và đã quá **120 giây**, admin chưa làm gì,
**When** thời gian tiếp tục trôi,
**Then** ghế B được **tô nổi bật** trên màn admin kèm thời lượng đã mất kết nối; hệ thống **không** tự loại, **không** tự xoá, **không** tự vô hiệu hoá; ghế B giữ nguyên trong trận với nguyên điểm và nguyên vị trí.

**AC-198** — *US*: US-013 · *FR*: FR-111 · *GR*: GR-036 C3
**Given** ghế B đang ở trạng thái quá ngưỡng và được tô nổi bật,
**When** admin xử lý,
**Then** admin có đúng **hai** lựa chọn — **giữ** hoặc **gia hạn** — và cả hai đều không phá gì, chỉ nói *"chờ tiếp"*.

**AC-199** — *US*: US-013, US-003 · *FR*: FR-113, FR-024 · *GR*: GR-036 C4 · INV-016
**Given** một vòng đang chạy với một câu đang đếm giờ, ghế B quá ngưỡng chờ,
**When** thời gian trôi qua mốc ngưỡng,
**Then** đồng hồ của câu **không dừng** và vòng **chạy tiếp bình thường** — hệ thống không dừng vì một ghế mất kết nối.

**AC-200** — *US*: US-013, US-003 · *FR*: FR-114, FR-022 · *GR*: GR-036 C8
**Given** một câu VCNV mở lúc `0` giây với hạn chót `15` giây; ghế E đã gửi một bản lúc `3` giây rồi mất kết nối lúc `4` giây, và đang gõ dở vài ký tự chưa gửi tại thời điểm rớt,
**When** E quay lại ở giây thứ `9`,
**Then** client của E dựng lại đúng màn đang thi và thấy còn **6 giây** — server đẩy **hạn chót `15` giây** chứ không phải *"còn 15 giây"*; bản gửi lúc `3` giây còn nguyên; ký tự gõ dở **mất** vì chưa từng tới server; và đồng hồ **không** được đặt lại.

**AC-201** — *US*: US-013 · *FR*: FR-115 · *GR*: GR-036 C5
**Given** một trận đang chạy, ghế C đang hoạt động bình thường,
**When** admin **vô hiệu hoá** ghế C qua dialog Yes/No kèm lý do,
**Then** ghế C **mất quyền thao tác** nhưng **ở lại trong trận**: nguyên điểm, nguyên vị trí, vẫn trên bảng điểm và bảng xếp hạng; tín hiệu lỡ tới của C vào lịch sử ở trạng thái **trơ**; thao tác vào nhật ký.

**AC-202** — *US*: US-013 · *FR*: FR-115 · *GR*: GR-036 C6
**Given** ghế C đang bị vô hiệu hoá và trong lúc đó vài câu đã được chấm,
**When** admin **kích hoạt lại** ghế C,
**Then** C thi tiếp bình thường và **không** thứ gì được hoàn nguyên — điểm ghi trong lúc C bị vô hiệu hoá giữ nguyên.

**AC-203** — *US*: US-013 · *FR*: FR-115 · *GR*: GR-036 C7
**Given** một câu Tăng tốc đang mở và ghế C bị vô hiệu hoá giữa chừng,
**When** admin bấm chốt câu,
**Then** ghế C được xử **y như ghế không trả lời** — mặc định **Sai** — và nếu admin thấy bất công thì đường xử lý là **cộng tay** qua điều chỉnh điểm thủ công.

**AC-204** — *US*: US-013 · *FR*: FR-110, FR-111 · *GR*: GR-036 C9, §Biên
**Given** ghế B mất kết nối, đồng hồ chờ đang chạy,
**When** admin gia hạn ở giây thứ 40 *(trước ngưỡng)*; và ở một ca khác ghế B quay lại đúng mốc `120.000` giây,
**Then** can thiệp sớm của admin **được phép** — 120 giây là khuyến nghị, không phải ràng buộc cưỡng chế; và ghế quay lại đúng mốc `120.000` giây **vẫn trong ngưỡng** vì biên là biên đóng.

**AC-205** — *US*: US-013 · *FR*: FR-112 · *GR*: GR-036 C10, §Nối lại nhiều lần
**Given** ghế A đã mất kết nối và nối lại **ba** lần trong một vòng, và ghế B cũng đang mất kết nối cùng lúc với lần thứ ba của A,
**When** A mất kết nối lần thứ tư,
**Then** ngưỡng của A tính **lại từ đầu** kể từ lần mất gần nhất — **không** cộng dồn thời gian các lần trước và **không** giới hạn số lần; cửa sổ chờ của A và của B **độc lập**; và mọi lần mất và nối lại đều nằm trong lịch sử, không bị xoá.

**AC-206** — *US*: US-013 · *FR*: FR-116 · *GR*: GR-036 C11
**Given** ghế A mất kết nối ngay trước khi trận đóng sổ, và ngưỡng chờ hết **sau** khi trận đã kết thúc,
**When** ngưỡng hết,
**Then** **ghế được giữ** — ngưỡng chỉ chi phối quyền thao tác **trong trận**, và trận đã kết thúc thì không còn gì để thao tác; bản ghi ghế sống theo hạn lưu trữ của trận.

**AC-207** — *US*: US-013, US-007 · *FR*: FR-114 · *GR*: GR-036 C8 · INV-017
**Given** ghế E quay lại giữa một câu đang mở, các ghế khác đã gửi bài, đáp án chuẩn của câu chưa được công bố,
**When** server dựng gói khôi phục cho E,
**Then** gói chứa vòng và giai đoạn hiện tại, câu đang mở, hạn chót theo server time, bản gửi gần nhất **của chính E**, các cờ ghế còn hiệu lực, trạng thái chuông theo luật, bàn cờ VCNV, điểm công khai và lớp phủ đang bật; gói **không** chứa đáp án chuẩn, **không** chứa bài làm của ghế khác, và việc dựng gói **không** sinh sự kiện nào.

**AC-208** — *US*: US-013 · *FR*: FR-116 · *GR*: GR-036 §Điều kiện
**Given** một trận đang chạy và một ghế mà admin muốn loại hẳn khỏi trận,
**When** admin rà soát mọi thao tác khả dụng,
**Then** **không** tồn tại thao tác kick nào ở v1; đường xử lý duy nhất là **vô hiệu hoá**, vốn đảo ngược được và giữ ghế lại trong trận.
