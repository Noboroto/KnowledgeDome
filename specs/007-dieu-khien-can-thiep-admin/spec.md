# Feature Specification: Điều khiển và can thiệp của admin

**Feature Branch**: `007-dieu-khien-can-thiep-admin`

**Created**: 2026-08-03

**Status**: Draft

**Input**: EPIC-007 — Điều khiển và can thiệp của admin (`docs/PRD.md` §11 dòng 470-481, §12)

**Nguồn**: `docs/PRD.md` (EPIC-007 §11; PRD-REQ-051→063 §12 mục EPIC-007; PRD-REQ-030, PRD-REQ-031, PRD-REQ-036, PRD-REQ-038, PRD-REQ-043, PRD-REQ-072, PRD-REQ-093, PRD-REQ-113 — các requirement khai **Related epic** gồm EPIC-007; §9.3, §9.4 ba hạng phản hồi; §9.5 bất biến; JOURNEY-005, JOURNEY-006 §10; ma trận truy nguyên §18) · `docs/game-rules.md` (`GR-026` → `GR-034` — dải EPIC-007 khai; và các rule được PRD-REQ của epic này tham chiếu: `GR-006`, `GR-009`, `GR-011`, `GR-012`, `GR-013`, `GR-015`, `GR-019`, `GR-035`, `GR-036`, `GR-037`; bốn bảng dùng chung) · `docs/game-state-machine.md` (`STATE-033` → `STATE-043`, `STATE-021`, `STATE-025`, `STATE-029` → `STATE-032`; `EVENT-004` → `EVENT-023`, `EVENT-028` → `EVENT-035`, `EVENT-052`; `T-084` → `T-090`, `T-097` → `T-100`; §Invalid transitions; `INV-001` → `INV-022`) · `docs/permissions.md` (`PERM-031` → `PERM-048` — chỉ dùng làm **cổng** của thao tác, catalog thuộc EPIC-001) · `docs/glossary.md` · `docs/traceability.md` · `.specify/memory/constitution.md` bản 1.4.0 · `CLAUDE.md` §UX, §Quy ước code.

**Quy ước truy nguyên của feature này** — ba điểm cần biết trước khi đọc bảng truy nguyên:

1. **Requirement khai nhiều epic vẫn thuộc epic này.** PRD-REQ-030, 031, 036, 038, 043, 072, 093, 113 đều khai `EPIC-007` trong trường **Related epic**. Spec này chỉ đặc tả **mặt điều khiển của admin** ở những requirement đó; **luật số học và hệ quả phía engine** của chúng thuộc `specs/005` và `specs/006` và **không** đặc tả lại ở đây (xem §Out of Scope).
2. **Hai thao tác lớp phủ có neo PRD nằm ở EPIC-009.** EPIC-007 §11 khai trong **Scope** cụm *"mở và đóng đáp án, ô chữ, **màn công bố, banner tạm dừng**"*, nhưng requirement mô tả **nội dung** hai lớp phủ đó là PRD-REQ-075 và PRD-REQ-076, khai `EPIC-009`. Spec này vì thế chỉ nhận **bề mặt bấm của admin** — nút mở, nút đóng, điều kiện bật nút, dấu vết nhật ký — và trỏ neo tới **EPIC-007 §11 Scope + các mệnh đề nói về admin trong PRD-REQ-075/076**. Việc **render** hai lớp phủ trên máy thí sinh, màn khán giả và lớp phủ dựng stream thuộc EPIC-009. Đây **không** phải `CONFLICT`: hai epic khai hai mặt khác nhau của cùng một cơ chế, và `docs/PRD.md` §11 EPIC-009 Scope không khai nút bấm.
3. **Permission là CỔNG, không phải requirement của spec này.** Ở chỗ một thao tác bị giới hạn cho *phiên đang giữ quyền điều khiển* hoặc cho một mã `PERM-NNN`, spec này phát biểu **ràng buộc**, không đặc tả catalog hay mô hình vai — hai thứ đó thuộc EPIC-001.

## Phạm vi

EPIC-007 phủ **bề mặt điều khiển và can thiệp của người vận hành trong một trận đang chạy**: bốn mốc bấm của một câu, phán quyết Đúng / Sai / Huỷ kết quả và cú bấm *chốt câu*, màn chấm với đối chiếu đáp án và lịch sử bài gửi, màn hàng đợi tín hiệu cùng thao tác duyệt / từ chối / kích hoạt tay, điều chỉnh điểm thủ công có lý do bắt buộc, ba cửa ra chủ động của một vòng, mở và đóng đáp án, bàn cờ Vượt chướng ngại vật, lớp công bố kết quả và banner tạm dừng, vô hiệu hoá và kích hoạt lại ghế, và ba hạng cảnh báo giao diện phân biệt bằng thứ chúng bảo vệ.

Actor chính: **ACTOR-001 admin**. Journey: **JOURNEY-005 — Thi đấu** và **JOURNEY-006 — Xử lý sự cố**.

**Mục tiêu của epic** (`docs/PRD.md` §11): *"Người vận hành luôn có đường sửa mọi sai lầm mà không mất lịch sử"* — giải `PS-7`, và bảo đảm **trận không bao giờ kẹt**.

**Không thuộc phạm vi feature này** (xem §Out of Scope): luật số học của từng vòng và mô hình điểm (EPIC-006); vòng đời cấp trận, cú bấm bắt đầu trận, chốt trận, huỷ trận, tạo trận mới (EPIC-005); giao diện máy thí sinh (EPIC-008); màn khán giả, lớp phủ dựng stream, màn MC, chủ đề, âm thanh (EPIC-009); xác thực, catalog permission, mô hình vai, giành quyền điều khiển (EPIC-001); kho đề và bộ đề (EPIC-002); contest builder (EPIC-004); biên bản và thống kê sau trận (EPIC-010); nhật ký thao tác chung và hạn lưu trữ (EPIC-011).

## User Scenarios & Testing *(mandatory)*

### US-001 — Bốn mốc bấm của một câu (Priority: P1)

- **Title**: Admin là cảm biến của sân khấu — mỗi mốc của luật là một cú bấm
- **Actor**: ACTOR-001 admin
- **Intent**: Đưa một câu qua trọn vòng đời của nó bằng những cú bấm tuần tự — hiển thị câu, start timer, bắt đầu thực hành *(chỉ câu thực hành)*, phán quyết — sao cho mọi mốc mà luật gốc mô tả bằng hành vi của MC đều có một thời điểm chính xác trên đồng hồ server.
- **User value**: Cửa sổ chuông, cửa sổ Ngôi sao hy vọng và thời gian suy nghĩ đều mở và đóng đúng lúc, nên kết quả trận không phụ thuộc vào việc máy có "đoán" được sân khấu hay không.
- **Priority**: P1
- **Priority rationale**: Đây là bề mặt mà **mọi** vòng đi qua; không có nó thì không câu nào chạy được. Gộp *hiển thị câu* với *start timer* là **phá luật** — cửa sổ chuông của Khởi động lượt chung co lại còn bằng thời gian suy nghĩ, và cửa sổ Ngôi sao hy vọng mất mốc đóng.
- **Independent test**: Ở một câu Khởi động lượt chung, xác nhận nút *start timer* **không bật** trước khi bấm *hiển thị câu*; bấm hiển thị rồi kiểm cửa sổ chuông đã mở và nút Ngôi sao hy vọng đã tắt trong khi đồng hồ **chưa** chạy; bấm start timer và kiểm đồng hồ chạy từ `timeSeconds` của câu; bấm start timer lần hai và kiểm nút đã tự khoá.
- **Related PRD requirements**: PRD-REQ-043, PRD-REQ-054
- **Related game rules**: `GR-033` *(toàn bộ bảng)*, `GR-019` §Thứ tự đánh giá *(mốc thứ ba)*, `GR-006` §Điều kiện *(mốc cắt)*, `GR-003`, `GR-021` · `INV-004`, `INV-015`
- **Related journey**: JOURNEY-005

---

### US-002 — Màn phán quyết: hai hay ba nút tuỳ hình phạt của vòng (Priority: P1)

- **Title**: Số nút chấm đổi theo vòng; một đối tượng đi qua đúng một phán quyết
- **Actor**: ACTOR-001 admin
- **Intent**: Chốt kết quả của một đối tượng được chấm — một thí sinh ở một câu — bằng đúng một cú bấm, với bộ nút đúng bằng bộ phán quyết mà vòng đó cho phép.
- **User value**: Admin không phải nhớ vòng nào có nút thứ ba giữa một trận trực tiếp, và không bao giờ bị ép chọn giữa cho điểm và phạt khi tình huống không đáng cả hai.
- **Priority**: P1
- **Priority rationale**: Phán quyết là **đường sinh điểm duy nhất** (`INV-003`) và là **điều kiện chuyển câu** (`INV-010`). Bố cục nút cố định cho mọi vòng đặt gánh nặng ghi nhớ vào đúng lúc admin ít chịu được nó nhất.
- **Independent test**: Mở lần lượt một câu Khởi động lượt riêng và một câu Khởi động lượt chung; xác nhận màn chấm hiện **hai** nút ở câu đầu và **ba** nút ở câu sau, không cần thao tác cấu hình nào ở giữa. Bấm chấm lần thứ hai cho cùng một đối tượng và xác nhận không sự kiện nào được sinh.
- **Related PRD requirements**: PRD-REQ-038, PRD-REQ-063, PRD-REQ-036
- **Related game rules**: `GR-026` *(toàn bộ bảng)*, bảng §*Phán quyết có hai hay ba lựa chọn*, bảng §*Bản gửi quá hạn*, `GR-004` C3, `GR-009` C6b, `GR-020` C2, `GR-021` · `INV-003`, `INV-009`, `INV-010`
- **Related journey**: JOURNEY-005

---

### US-003 — Màn chấm: đối chiếu, tô khác biệt, và lịch sử bài gửi (Priority: P1)

- **Title**: Máy bày đủ bằng chứng ra trước mắt; người đọc rồi quyết
- **Actor**: ACTOR-001 admin
- **Intent**: Nhìn bài làm của một ghế cạnh danh sách đáp án được chấp nhận, thấy ngay chỗ khác nhau, xem lại các bản đã gửi trước đó, và phân biệt được bản hợp lệ với bản quá hạn — tất cả trên một màn, trước khi bấm.
- **User value**: Không còn phải đọc chuỗi ký tự bằng mắt trần giữa trận trực tiếp; và một bài khớp đáp án **thứ ba** trong danh sách không bị tô đỏ toàn bộ thành báo động giả.
- **Priority**: P1
- **Priority rationale**: Đây là lời giải trực tiếp cho `PS-7` và là công cụ làm cho mô hình *"máy hiển thị, người phán quyết"* dùng được trên thực tế. Tô theo đáp án **đầu danh sách** sinh báo động giả đủ nhiều để admin bỏ qua cả những cảnh báo thật.
- **Independent test**: Với một câu có ba đáp án được chấp nhận, gửi một bài khớp tuyệt đối đáp án thứ ba; xác nhận màn chấm **không tô đỏ** gì và vẫn cho xem cả ba đáp án. Sau đó gửi thêm một bản sau hạn chót và xác nhận cả hai bản cùng hiện, bản sau tô đỏ, bản trước không bị thay.
- **Related PRD requirements**: PRD-REQ-051, PRD-REQ-052
- **Related game rules**: `GR-027` *(toàn bộ bảng)*, bảng §*Bản gửi quá hạn*, `GR-006` C4, C5, `GR-015` C5, C6, `GR-035` C6, `GR-019` §*Không có gì để highlight*
- **Related journey**: JOURNEY-005

---

### US-004 — Chấm Tăng tốc trên một màn, các ghế cạnh nhau (Priority: P2)

- **Title**: Thấy quan hệ thứ hạng trước khi chốt bảng
- **Actor**: ACTOR-001 admin
- **Intent**: Chấm toàn bộ các ghế của một câu Tăng tốc mà không rời màn, để nhìn thấy thứ tự thời gian giữa những người sắp được chấm Đúng.
- **User value**: Điểm Tăng tốc là hàm của **thứ hạng**; nhìn thấy cả bảng cùng lúc là điều kiện để phán quyết đúng cái quan hệ mà luật thưởng.
- **Priority**: P2
- **Priority rationale**: Đây là ràng buộc **bố cục** của một màn cụ thể, không phải một cơ chế luật; vòng vẫn chấm được nếu bố cục sai, chỉ là admin dễ chấm nhầm. Phụ thuộc US-002 về mặt hành vi nút nhưng kiểm thử được độc lập bằng chính bố cục.
- **Independent test**: Mở một câu Tăng tốc có bốn ghế đều đã gửi bài; xác nhận cả bốn bài làm, bốn mốc thời gian và bốn bộ nút chấm cùng nằm trong một khung nhìn, và không thao tác nào yêu cầu chuyển màn.
- **Related PRD requirements**: PRD-REQ-053
- **Related game rules**: `GR-013` *(thang theo thứ hạng)*, `GR-015` §Thứ tự đánh giá
- **Related journey**: JOURNEY-005

---

### US-005 — Nút chấm mở hay khoá theo KÊNH trả lời, và nói rõ lý do (Priority: P1)

- **Title**: Đồng hồ khoá thí sinh, không khoá admin — trừ đúng một ngoại lệ
- **Actor**: ACTOR-001 admin
- **Intent**: Biết chắc lúc nào bấm chấm được và vì sao nút đang mờ, ở cả ba kênh trả lời: nói trên sân khấu, gõ máy, và thao tác thực hành.
- **User value**: Ở vòng gõ máy, admin không chấm nhầm trên một bản mà thí sinh còn đang sửa; ở vòng nói và vòng thực hành, admin không bị nút mờ chặn oan.
- **Priority**: P1
- **Priority rationale**: Chấm sớm ở vòng gõ máy là **chấm trên một bản không phải bản luật công nhận** — sai kết quả mà không ai thấy. Nút mờ **không giải thích** thì admin sẽ tưởng hệ thống hỏng và tìm đường lách.
- **Independent test**: Ở một câu Tăng tốc, xác nhận nút chấm không bấm được trước mốc hết giờ và giao diện nêu rõ lý do; sang một câu Khởi động ở mode sân khấu, xác nhận nút chấm bấm được ngay khi câu vừa hiển thị.
- **Related PRD requirements**: PRD-REQ-054
- **Related game rules**: bảng §*Kênh trả lời quyết định lúc nào admin chấm được*, `GR-019` §Điều kiện, `GR-026` §Biên, `GR-006` §Đồng thời, `GR-015` §Đồng thời · `INV-015`
- **Related journey**: JOURNEY-005

---

### US-006 — Chốt câu khi mới chấm một phần (Priority: P1)

- **Title**: Cú bấm *chốt câu* chính là phán quyết cho những ai chưa được chấm
- **Actor**: ACTOR-001 admin
- **Intent**: Khép một câu chấm-theo-lô — Tăng tốc và câu hàng ngang Vượt chướng ngại vật — khi chỉ mới đánh dấu một phần các ghế, và biết trước chính xác ghế nào sắp bị tính là Sai.
- **User value**: Vòng không kẹt vì một ghế không trả lời, mà cũng không có bộ đếm nào tự chốt sau lưng admin.
- **Priority**: P1
- **Priority rationale**: Không có cửa này thì `INV-010` *("không tồn tại trạng thái vòng đã đóng mà còn câu chưa chấm")* không giữ được ở hai vòng chấm theo lô. Ngoại lệ cho tín hiệu *"Mở chướng ngại vật"* là **bắt buộc**: sai ở đó **loại thí sinh**, nên một cú bấm chốt câu vì lý do khác sẽ loại nhầm người.
- **Independent test**: Ở một câu Tăng tốc bốn ghế, đánh dấu Đúng cho hai ghế rồi bấm *chốt câu*; xác nhận dialog liệt kê **tên** hai ghế còn lại, và sau khi xác nhận thì hai ghế đó nhận 0 và không giữ chỗ trong thang. Lặp lại ở một câu hàng ngang VCNV đang có một tín hiệu *"Mở chướng ngại vật"* chờ duyệt và xác nhận tín hiệu đó **không** bị chấm Sai.
- **Related PRD requirements**: PRD-REQ-062
- **Related game rules**: `GR-026` C4, C5, `GR-013` C6, `GR-008` §Thứ tự đánh giá, `GR-009` C6b · `INV-009`, `INV-010`
- **Related journey**: JOURNEY-005

---

### US-007 — Điều chỉnh điểm thủ công với lý do bắt buộc (Priority: P1)

- **Title**: Van thoát duy nhất cho mọi sai lầm trong trận
- **Actor**: ACTOR-001 admin
- **Intent**: Cộng hoặc trừ một lượng bất kỳ cho một ghế, kèm lý do, ở bất cứ lúc nào trận chưa đóng sổ — kể cả giữa một vòng đang chạy.
- **User value**: Mọi sai sót còn lại đều có đường sửa mà **không** phải chấm lại, không phải hoàn nguyên cả vòng, và không mất dấu vết ai đã quyết điều gì.
- **Priority**: P1
- **Priority rationale**: `PRD-REQ-037` cấm chấm lại và cấm đổi phán quyết tại chỗ; nếu không có cửa này thì mọi sai sót sau khi chấm đều **không sửa được**. Ô lý do là kênh duy nhất ghi xuất xứ một quyết định vào biên bản.
- **Independent test**: Điều chỉnh `+5` cho một ghế kèm lý do và xác nhận điểm đổi đúng và biên bản có một dòng. Sau đó điều chỉnh **lượng 0** kèm lý do và xác nhận **vẫn** có một dòng mới trong biên bản dù điểm không đổi. Cuối cùng thử điều chỉnh với ô lý do trống và xác nhận không sự kiện nào được sinh.
- **Related PRD requirements**: PRD-REQ-055
- **Related game rules**: `GR-029` *(toàn bộ bảng)*, `GR-028` C3, `GR-030` §*Không đổi gì* · `INV-002`, `INV-018`
- **Related journey**: JOURNEY-005, JOURNEY-006

---

### US-008 — Ba cửa ra chủ động của một vòng (Priority: P1)

- **Title**: Bỏ vòng · chạy lại vòng · kết thúc khẩn cấp — và chỉ một trong ba giữ nguyên điểm
- **Actor**: ACTOR-001 admin
- **Intent**: Rời khỏi một vòng hỏng theo đúng ý định của mình: xoá kết quả của nó, chạy lại nó từ đầu, hoặc đi tiếp mà **giữ** điểm đã ghi.
- **User value**: Một vòng hỏng giữa buổi thi trực tiếp không khoá được admin ở lại trong đó; và biên bản vẫn nói đúng chuyện gì đã xảy ra.
- **Priority**: P1
- **Priority rationale**: Đây là lời giải trực tiếp cho vế *"trận không bao giờ kẹt"* của mục tiêu epic. Cửa **giữ nguyên điểm** tồn tại **chỉ vì** khác biệt đó — thiếu nó thì admin buộc phải xoá điểm hợp lệ để thoát khỏi một vòng hỏng.
- **Independent test**: Chạy một vòng tới khi có điểm, rồi lần lượt thử ba cửa trên ba lần chạy khác nhau; xác nhận **kết thúc khẩn cấp** để bảng điểm không đổi, **bỏ vòng** làm điểm của vòng đó bị đảo bằng sự kiện mới *(không bị xoá)*, và **chạy lại** đưa vòng về đầu sau khi qua cửa kiểm kho đề. Cả ba lần xác nhận dialog không tắt được bằng `Esc` và bắt nhập lý do.
- **Related PRD requirements**: PRD-REQ-031, PRD-REQ-056
- **Related game rules**: `GR-030` *(toàn bộ bảng)*, `GR-029` C4, C5, `GR-031` C2 · `INV-001`, `INV-022`
- **Related journey**: JOURNEY-006

---

### US-009 — Admin chọn vòng nào mở; lệch khuyến nghị chỉ cảnh báo (Priority: P1)

- **Title**: Playlist là gợi ý, không phải hàng rào
- **Actor**: ACTOR-001 admin
- **Intent**: Mở vòng nào mình muốn từ trạng thái nghỉ, kể cả khi thứ tự đó lệch với khuyến nghị của hệ thống.
- **User value**: Buổi thi thật đổi thứ tự vì lý do sân khấu — thiết bị hỏng, khách mời tới muộn — và hệ thống không được là thứ chặn đường.
- **Priority**: P1
- **Priority rationale**: `INV-020` khai thẳng *"thứ tự vòng và thứ tự lượt là KHUYẾN NGHỊ"*. Cưỡng chế thứ tự sẽ chặn đúng lúc cần nhất, và hạ khuyến nghị xuống thành im lặng thì mất luôn giá trị cảnh báo.
- **Independent test**: Từ trạng thái nghỉ, chọn một vòng **khác** vòng được khuyến nghị; xác nhận dialog nêu tên luật bị lệch, bấm Yes thì vòng mở bình thường, bấm No thì không gì xảy ra. Chọn đúng vòng khuyến nghị và xác nhận **không** dialog nào hiện.
- **Related PRD requirements**: PRD-REQ-030
- **Related game rules**: `GR-030` C4 · `INV-014`, `INV-020`, `INV-022`
- **Related journey**: JOURNEY-005

---

### US-010 — Ba hạng cảnh báo giao diện, phân biệt bằng thứ chúng bảo vệ (Priority: P1)

- **Title**: *"Không làm được"* khác hẳn *"làm được nhưng nguy hiểm"*
- **Actor**: ACTOR-001 admin
- **Intent**: Nhìn một phản hồi của hệ thống và biết ngay mình đang ở tình huống nào: thao tác không tồn tại, thao tác hợp lệ nhưng không hoàn tác được, hay thao tác phá huỷ cần ghi lý do.
- **User value**: Admin học đúng mô hình của hệ thống ngay từ buổi thi đầu tiên, nên không bấm bừa qua một dialog quan trọng vì tưởng nó giống một dialog vô hại.
- **Priority**: P1
- **Priority rationale**: Trộn ba hạng lại làm hỏng **cả ba**: toast bị bấm qua như dialog, dialog phá huỷ bị đóng như toast. Đây cũng là lớp bảo vệ chung của mọi story khác trong epic này.
- **Independent test**: Kích hoạt lần lượt ba tình huống — bấm một nút ở trạng thái không hợp lệ, mở đáp án, và bỏ vòng — rồi xác nhận ba phản hồi khác nhau về hình thức, và chỉ hạng thứ ba bắt nhập lý do và không đóng được bằng `Esc` hay click ra ngoài.
- **Related PRD requirements**: PRD-REQ-056
- **Related game rules**: `GR-029` §Điều kiện, `GR-030` §Điều kiện, `GR-026`, `GR-032` C9 · `PRD §9.4` · `INV-014`
- **Related journey**: JOURNEY-005, JOURNEY-006

---

### US-011 — Mở và đóng đáp án: thao tác tay thắng mọi cờ tự động (Priority: P1)

- **Title**: Không có khái niệm engine tự mở
- **Actor**: ACTOR-001 admin
- **Intent**: Đưa đáp án chuẩn của câu đang chạy ra **mọi vai đang xem** bằng một cú bấm, và thu lại bằng một cú bấm khác, không phụ thuộc cấu hình nào của trận.
- **User value**: Khi cờ công bố tự động đang tắt mà MC vẫn muốn khán giả thấy đáp án, admin không phải sửa cấu hình giữa trận.
- **Priority**: P1
- **Priority rationale**: Đây là van thoát hiển thị duy nhất, và nó nằm cạnh **hàng rào chống rò đề** — sai ở đây là hỏng sản phẩm. Quyền này phải độc quyền của phiên đang giữ quyền điều khiển; MC đọc được đáp án nhưng **không** công bố được cho ai.
- **Independent test**: Với một trận đang tắt cờ công bố tự động, bấm mở đáp án ở một câu; xác nhận nội dung tới máy thí sinh, màn khán giả và lớp phủ, có một dòng nhật ký, và **điểm không đổi**. Mở màn MC song song và xác nhận ở đó không tồn tại nút mở.
- **Related PRD requirements**: PRD-REQ-057
- **Related game rules**: `GR-037` *(toàn bộ bảng, gồm C8 và C8b)*, `GR-012` C2 · `INV-017`, `INV-021` · `PERM-044`, `PERM-045`
- **Related journey**: JOURNEY-005

---

### US-012 — Bàn cờ Vượt chướng ngại vật: ba trạng thái, đặt được cả hai chiều (Priority: P1)

- **Title**: Đường duy nhất dựng lại bàn cờ sau sự cố
- **Actor**: ACTOR-001 admin
- **Intent**: Đặt thẳng từng ô của bàn cờ sang giá trị mong muốn — chờ, đã hỏi, mở — theo **cả hai chiều**, để khôi phục đúng trạng thái sau một sự cố mà không phải chạy lại cả vòng.
- **User value**: Một ô bị đánh dấu nhầm giữa vòng sửa được tại chỗ, và băng điểm Chướng ngại vật tự đúng theo mà không ai phải tính tay.
- **Priority**: P1
- **Priority rationale**: Băng điểm là **hàm** của trạng thái bàn cờ, không phải bộ đếm cộng dồn — chính vì thế mà lùi được. Một bộ đếm `+1` thì đếm trùng và không có đường về, biến một cú bấm nhầm thành hỏng cả vòng.
- **Independent test**: Ở vòng VCNV, đặt tay một ô sang *đã hỏi* và xác nhận băng tụt đúng một bậc mà **không ai** được cộng điểm; đặt ô đó về *chờ* và xác nhận băng **lên lại**; đặt lại đúng giá trị nó đang có và xác nhận không có gì thay đổi.
- **Related PRD requirements**: PRD-REQ-058
- **Related game rules**: `GR-009` C12, C13, `GR-011` C1, C3, C8, `GR-012` C2, C3, C5, `GR-008` · `STATE-041` → `STATE-043`, `T-084` → `T-086` · `INV-021`
- **Related journey**: JOURNEY-005, JOURNEY-006

---

### US-013 — Màn hàng đợi: duyệt, từ chối, và tách bạch lịch sử (Priority: P1)

- **Title**: Hàng đợi đang hoạt động ở một chỗ, lịch sử tín hiệu ở chỗ khác
- **Actor**: ACTOR-001 admin
- **Intent**: Nhìn thấy mọi tín hiệu của thí sinh kèm mốc thời gian và kết cục của nó, duyệt hoặc từ chối những tín hiệu đang chờ ở Vượt chướng ngại vật, và xem lại lịch sử đầy đủ để gỡ một lệnh cấm khi cần.
- **User value**: Lỗi bấm nhầm của thí sinh sửa được bằng một cú bấm No mà thí sinh **không mất lượt**; và khi có khiếu nại thì admin có nguyên chuỗi bằng chứng để phân xử.
- **Priority**: P1
- **Priority rationale**: Lịch sử đầy đủ là **van thoát thay cho cửa sổ ân hạn** — hệ thống chọn không có grace chính vì có cái này. Trộn hàng đợi đang hoạt động với lịch sử làm admin duyệt nhầm một tín hiệu đã hết hiệu lực.
- **Independent test**: Ở VCNV, cho hai ghế cùng chọn hàng ngang; xác nhận cả hai xuất hiện trong hàng đợi đang hoạt động theo đúng thứ tự mốc thời gian. Bấm No cho tín hiệu đầu và xác nhận ghế đó **không mất lượt**, ô chữ chưa đổi, câu chưa tiêu, đồng hồ chưa chạy, và tín hiệu bị từ chối vẫn còn nguyên trong lịch sử.
- **Related PRD requirements**: PRD-REQ-059
- **Related game rules**: `GR-032` C1 → C7, `GR-009` C7, C8, `GR-007` · `INV-001`, `INV-006`, `INV-007`, `INV-008`
- **Related journey**: JOURNEY-005, JOURNEY-006

---

### US-014 — Bề mặt kích hoạt tay một tín hiệu sau phán quyết Huỷ kết quả (Priority: P2)

- **Title**: Trao lại lượt cho một ghế còn tín hiệu trong hàng đợi
- **Actor**: ACTOR-001 admin
- **Intent**: Sau khi chấm **Huỷ kết quả** cho người đang giữ quyền trả lời, chọn một ứng viên còn hiệu lực trong hàng đợi và trao quyền cho họ.
- **User value**: Một cú chuông hỏng, một sự cố thiết bị, một tình huống ngoài luật — tất cả đều có đường xử lý mà không phải bỏ cả câu hay cả vòng.
- **Priority**: P2
- **Priority rationale**: `PRD-REQ-044` hứa rằng hàng đợi là *"lưới an toàn để admin can thiệp"* nhưng lời hứa đó **không có cơ chế**; đây là cơ chế duy nhất của nó. Xếp P2 vì nó là **biến thể ngoài luật gốc O26** và chỉ mở sau đúng một loại phán quyết — trận vẫn chạy trọn vẹn khi chưa có nó.
- **Independent test**: Ở một câu Khởi động lượt chung có hai ghế đã bấm chuông, chấm **Huỷ kết quả** cho người giành quyền; xác nhận nút kích hoạt tay bật, hệ thống khuyến nghị tín hiệu sớm nhất chưa xử lý, và sau khi kích hoạt thì ghế được chọn nhận **trọn 3 giây** mới. Lặp lại với phán quyết **Sai** và xác nhận nút **không** bật.
- **Related PRD requirements**: PRD-REQ-113
- **Related game rules**: `GR-032` C8, C9 và §*Kích hoạt tay*, `GR-009` C6b, `GR-037` C8b, `GR-003`, `GR-020`, `GR-023` · `EVENT-052`, `T-097` → `T-100` · `INV-006`, `INV-008`, `INV-009`, `INV-014`, `INV-020`
- **Related journey**: JOURNEY-005, JOURNEY-006

---

### US-015 — Ba trạng thái câu phân biệt được trên màn admin (Priority: P2)

- **Title**: Ranh giới gỡ được / không gỡ được chính là ranh giới tiêu câu
- **Actor**: ACTOR-001 admin
- **Intent**: Nhìn danh sách câu của một trận và biết ngay câu nào chưa rút, câu nào đã rút mà chưa hiển thị *(còn gỡ được)*, và câu nào đã hiển thị *(không gỡ được nữa)*.
- **User value**: Không mất câu oan vì gỡ nhầm, và không lộ đề vì tưởng một câu đã hiển thị là chưa.
- **Priority**: P2
- **Priority rationale**: Đây là ràng buộc **hiển thị** trên một màn; quy tắc tiêu câu và lệnh cấm gỡ đã được cưỡng chế ở tầng server bởi EPIC-005 và EPIC-006. Thiếu nó thì admin thao tác chậm và dễ nhầm, nhưng dữ liệu vẫn đúng.
- **Independent test**: Ở trạng thái nghỉ, mở màn danh sách câu của một trận đã chạy một phần; xác nhận ba nhóm câu phân biệt được **bằng mắt** và thao tác gỡ chỉ khả dụng ở đúng nhóm giữa.
- **Related PRD requirements**: PRD-REQ-060
- **Related game rules**: `GR-031` C5, C6, C7, C8 và §*Ranh giới "đã dùng"* · `INV-011`
- **Related journey**: JOURNEY-005

---

### US-016 — Vô hiệu hoá và kích hoạt lại ghế; phán quyết ghế mất kết nối (Priority: P2)

- **Title**: Máy độc quyền đo, người độc quyền quyết — cả ở chuyện kết nối
- **Actor**: ACTOR-001 admin
- **Intent**: Tắt và bật lại quyền thao tác của một ghế ở bất cứ lúc nào trận chưa đóng sổ, và quyết định giữ hay gia hạn cho một ghế đã quá ngưỡng chờ kết nối.
- **User value**: Một máy thí sinh chết giữa vòng không làm kẹt trận, và không có chính sách tự động nào loại một ghế sau lưng admin.
- **Priority**: P2
- **Priority rationale**: Ràng buộc *"chỉ ở trạng thái nghỉ"* sẽ làm mất đúng tình huống cần nhất. Xếp P2 vì trận vẫn chạy được khi mọi thiết bị bình thường; nó là công cụ của JOURNEY-006 chứ không phải của luồng chính.
- **Independent test**: Giữa một câu đang mở, vô hiệu hoá một ghế và xác nhận ghế đó mất quyền thao tác nhưng **giữ nguyên điểm và vị trí** trên mọi bảng, tín hiệu lỡ tới của nó vào lịch sử ở trạng thái trơ. Kích hoạt lại và xác nhận **không có gì bị hoàn nguyên**.
- **Related PRD requirements**: PRD-REQ-061
- **Related game rules**: `GR-036` C2, C3, C5, C6, C7, C9 · `STATE-023` → `STATE-025`, `EVENT-029`, `EVENT-030` · `INV-016`
- **Related journey**: JOURNEY-006

---

### US-017 — Lớp phủ do admin điều khiển: công bố kết quả và banner tạm dừng (Priority: P2)

- **Title**: Bật một lớp lên, tắt nó đi, và trạng thái bên dưới không suy suyển
- **Actor**: ACTOR-001 admin
- **Intent**: Mở màn công bố bảng xếp hạng bất cứ lúc nào, và bật một banner tạm dừng chặn toàn bộ thao tác của thí sinh khi buổi thi cần dừng — rồi tắt cả hai bằng chính tay mình.
- **User value**: Sân khấu điều khiển được từ một chỗ, và không lớp phủ nào ăn mất trạng thái của vòng đang chạy bên dưới.
- **Priority**: P2
- **Priority rationale**: Không cơ chế luật nào phụ thuộc hai lớp này — chúng là công cụ trình diễn và công cụ dừng. Ràng buộc quan trọng nhất là **loại trừ hai chiều** giữa banner và đồng hồ, và nó kiểm thử được độc lập.
- **Independent test**: Giữa một vòng đang chạy nhưng **không có đồng hồ nào đếm**, mở rồi đóng màn công bố và xác nhận vòng tiếp tục đúng chỗ cũ. Bật banner tạm dừng rồi thử bấm start timer và xác nhận nút không bật; đóng banner và xác nhận nút sống lại.
- **Related PRD requirements**: EPIC-007 §11 Scope *(bề mặt bấm của admin)* · PRD-REQ-075, PRD-REQ-076 *(chỉ các mệnh đề nói về **thao tác của admin**: nút mở, nút đóng, điều kiện bật nút, dấu vết nhật ký — việc **render** hai lớp phủ trên máy thí sinh, màn khán giả và lớp phủ dựng stream thuộc EPIC-009)*
- **Related game rules**: `GR-035`, `GR-028` · `STATE-033`, `STATE-040`, `EVENT-032` → `EVENT-035`, `T-087` → `T-090` · `INV-016`, `INV-021`
- **Related journey**: JOURNEY-005, JOURNEY-006

---

### US-018 — Nút một chiều: không chặn gửi lại, nhưng khoá theo luật chơi thì vẫn khoá (Priority: P2)

- **Title**: Hai loại nút mờ, hai lý do khác hẳn nhau
- **Actor**: ACTOR-001 admin
- **Intent**: Bấm lại một thao tác chậm mạng mà không bị giao diện chặn, trong khi những nút bị khoá **theo luật chơi** vẫn khoá và nói rõ rằng chúng khoá vì luật.
- **User value**: Không mất thao tác vì mạng chậm giữa trận trực tiếp, mà cũng không vô tình gỡ mất một khoá mà luật đang cần.
- **Priority**: P2
- **Priority rationale**: Đây là ghi chú ranh giới để lần rà quy tắc giao diện sau không ai gỡ nhầm khoá theo luật chơi. Nó là quy ước dùng chung, kiểm thử được bằng chính thành phần dùng chung đó.
- **Independent test**: Bấm một nút hành động hai lần liên tiếp trong lúc mạng chậm; xác nhận nút chỉ hiện trạng thái đang xử lý chứ **không** bị vô hiệu hoá, và server nhận bản cuối. Đối chiếu với một nút bị khoá theo luật chơi và xác nhận nó vô hiệu hoá thật và mang nhãn phân loại rõ.
- **Related PRD requirements**: PRD-REQ-072
- **Related game rules**: `GR-034` C5, `GR-006`, `GR-029` §Bấm trùng, `GR-033` C2 · `CLAUDE.md` §UX
- **Related journey**: JOURNEY-005

---

### US-019 — Cảnh báo khi một thao tác làm vỡ bộ Vượt chướng ngại vật (Priority: P3)

- **Title**: Mượn một hàng ngang là mất sáu thành phần để lấy một
- **Actor**: ACTOR-001 admin
- **Intent**: Được cảnh báo — kèm **tên bộ** sẽ vỡ — trước khi chỉ định một hàng ngang, một câu ô trung tâm, hay một Chướng ngại vật làm Câu hỏi phụ.
- **User value**: Một trận hoà không âm thầm ăn mất bộ Vượt chướng ngại vật của trận kế.
- **Priority**: P3
- **Priority rationale**: Chỉ chạm tới đúng một thao tác hiếm — chỉ định câu Câu hỏi phụ — và chỉ ở những contest chạy nhiều trận. Trận đơn lẻ không bao giờ gặp.
- **Independent test**: Chỉ định một hàng ngang thuộc một bộ nguyên vẹn làm Câu hỏi phụ; xác nhận dialog nêu **tên bộ** sẽ vỡ. Sau đó chạy phép rút tự động của Câu hỏi phụ khi hai kho kia còn câu và xác nhận nó **không** chạm kho Vượt chướng ngại vật.
- **Related PRD requirements**: PRD-REQ-093
- **Related game rules**: `GR-031` C3b, C3c, `GR-023` §Nguồn đề · `TERM-058`
- **Related journey**: JOURNEY-006

---

### Edge Cases

Liệt kê theo tám nhóm mà lệnh gọi yêu cầu. Mỗi mục ghi **hành vi đã có nguồn**; mục nào nguồn im lặng thì trỏ sang §Open Questions.

**Boundary cases**

- Cửa sổ giữ bản tới muộn: nút chấm mở tại **đúng** mốc `hạn chót + padding`; ở Khởi động mở tại `hạn chót` vì vòng đó không có biên trên. *(Biên của chính cửa sổ — `specs/006` FR-070a.)*
- Cửa sổ chuông **đúng mốc** — 3 giây Khởi động lượt chung · 5 giây cướp quyền Về đích · 15 giây Câu hỏi phụ: biên **đóng**, tín hiệu đúng mốc vẫn hợp lệ (`GR-033` §Biên, `INV-005`). Một mili-giây sau thì **không có nút để bấm**.
- Ngưỡng chờ kết nối **đúng `120,000` giây** vẫn nằm trong ngưỡng (`GR-036` §Biên).
- Điều chỉnh điểm **lượng 0**: vẫn sinh sự kiện (`GR-029` C3). Lý do **không giới hạn độ dài** (`GR-029` §Biên).
- Băng điểm Chướng ngại vật: **0** hàng ngang đã hỏi ⇒ băng 60; sau gợi ý cuối ⇒ **20, là sàn**, kể cả khi câu ô trung tâm sai (`GR-009` §Biên, `GR-011` C3).
- Kết thúc khẩn cấp **không** có ràng buộc về số câu đã hỏi; bỏ được **bất kỳ** vòng nào; chạy lại **không giới hạn số lần** miễn kho còn câu (`GR-030` §Biên).
- Số lần điều chỉnh điểm trong một trận: **không giới hạn**, mỗi lần một sự kiện (`GR-029` §Biên).

**Invalid state**

- Bấm *start timer* khi câu **chưa hiển thị**: nút chưa bật — thứ tự cố định (`GR-033`, §Invalid transitions).
- Bấm *start timer* khi **banner tạm dừng đang bật**: toast, không ép được. Bấm *mở banner* khi **đồng hồ đang chạy**: toast (`GR-035`, `T-089`).
- Chấm ở vòng **gõ máy** khi đồng hồ còn chạy: nút mờ (`GR-006`, bảng §Kênh trả lời).
- Chấm một câu thuộc vòng **đã bị bỏ**: toast (`GR-013` C7, `GR-030`).
- Điều chỉnh điểm ở trận **đã đóng sổ**: toast, không ép được (`GR-029` C6).
- Vô hiệu hoá ghế ở trận **đã đóng sổ**: nút không bật (`EVENT-030`).
- Bỏ một vòng **đã ở nhãn đã bỏ**: server từ chối — đây là invalid state, **không phải** dedup (`GR-030` C5).
- Đặt trạng thái ô chữ **ngoài vòng Vượt chướng ngại vật**: nút không tồn tại (`EVENT-021`).
- Mở vòng khác khi một vòng **đang chạy**: trong màn vòng **không có nút** — invalid state, không phải chặn cứng (`GR-030` C4).
- Sửa danh sách câu **giữa một vòng đang chạy**: toast (`GR-031` C8). Gỡ một câu **đã hiển thị**: toast, không ép được (`GR-031` C6).
- Kích hoạt tay sau phán quyết **Đúng** hoặc **Sai**: nhánh **không tồn tại**, nút không bật (`EVENT-052`).

**Repeated action**

- Bấm *start timer* lần thứ hai: nút tự khoá sau lần đầu ⇒ cửa sổ **không thể** bị kéo dài (`GR-033` C2).
- Bấm *hiển thị câu* lần thứ hai: chỉ lần đầu có hiệu lực (`GR-033` §Bấm trùng).
- Bấm chấm lần thứ hai cho **cùng bộ ba** *(câu, thí sinh, loại phán quyết)*: nút đã khoá; server vẫn phải bỏ qua nếu yêu cầu lọt tới (`GR-026` §Bấm trùng, `INV-009`).
- Bấm *bỏ vòng* lần thứ hai do trễ mạng: ba lớp độc lập chặn — nút một chiều tự tắt, dialog phá huỷ bắt nhập lý do, và server từ chối lệnh bỏ cho một vòng đã ở trạng thái đã bỏ (`GR-030` C5).
- Bấm *công bố Chướng ngại vật* hoặc *mở ô trung tâm* lần thứ hai: nút một chiều, tự tắt (`GR-012` C5, `GR-011` C8).
- **Điều chỉnh điểm KHÔNG dedup, và đó là chủ đích**: hai lần `+5` liên tiếp là một yêu cầu hợp lệ, tổng `+10` (`GR-029` §Bấm trùng).
- Vô hiệu hoá một ghế **đã bị vô hiệu hoá**: no-op (`EVENT-030`).
- Đặt một ô chữ về **đúng giá trị nó đang có**: no-op — phép gán, không phải phép cộng (`T-085`).

**Stale state**

- Duyệt một tín hiệu thuộc **câu đã khép** hoặc **vòng đã đóng**: tín hiệu vô hiệu theo **đích** của nó, không còn là ứng viên, dù vẫn nguyên trong lịch sử (`GR-032` §Vòng đời tín hiệu, `GR-012` §Đồng thời).
- Hàng đợi đang hoạt động **đặt lại** khi đích đóng; **lịch sử tín hiệu không bao giờ xoá** (`GR-032` C5, `INV-001`).
- Một cú giành quyền điều khiển **mất đối tượng** khi phiên đang giữ kết nối lại (`T-095`) — cơ chế thuộc EPIC-001, nêu ở đây vì nó đổi ai là người bấm.

**Duplicate event**

- Server phải bỏ qua **mọi** phán quyết trùng bộ ba, kể cả khi giao diện đã khoá — khoá ở giao diện chỉ là lớp tăng cường (`INV-009`).
- Từ chối một tín hiệu là **idempotent**: tín hiệu đã bị từ chối không từ chối lần nữa (`GR-032` §Bấm trùng).
- Tính lại điểm từ nhật ký là **idempotent**: gọi bao nhiêu lần cũng cùng kết quả (`GR-028` §Bấm trùng).

**Partial failure**

- Chốt một câu chấm-theo-lô khi **mới chấm một phần**: ghế chưa chấm tính là Sai — Tăng tốc nhận 0 và **không giữ chỗ** trong thang; hàng ngang VCNV nhận 0 và **không bị loại** (`GR-026` C4).
- **Ngoại lệ**: tín hiệu *"Mở chướng ngại vật"* đang chờ duyệt **không** bị mặc định Sai (`GR-026` C5, `INV-010`).
- **Kết thúc khẩn cấp không dùng mặc định Sai**: câu đang mở khép bằng **Huỷ kết quả** — không sinh điểm cho ai (`EVENT-004`, `GR-030` C3).
- Ghế bị vô hiệu hoá **giữa một câu đang mở**: xử y như ghế không trả lời ⇒ mặc định Sai khi admin chốt câu; thấy bất công thì admin cộng tay (`GR-036` C7).
- Ghế quá ngưỡng chờ kết nối **giữa một vòng đang chạy**: vòng chạy tiếp bình thường, hệ thống chỉ tô nổi bật (`GR-036` C2, C4).

**Conflicting rule**

- **Hạng dialog của điều chỉnh điểm thủ công**: đây là **tầng riêng ở giữa** — `Esc` **đóng** dialog và khi đó **không lưu** thay đổi; click ra ngoài **không** đóng; ô lý do **bắt buộc**; nhận phím tắt `Y`/`N`. Nó **không** thuộc hạng phá huỷ và **không** phải Yes/No thường (FR-045, FR-061a).
- **Dialog cảnh báo lệch luật**: dùng lại **y hệt** tầng Yes/No thường về cách thoát — `Esc` và click ra ngoài đều đóng và đều tương đương **No**, có phím tắt `Y`/`N`, **không** bắt nhập lý do. Khác dialog xác nhận **chỉ ở nội dung chữ**, và khác toast invalid state ở chỗ **ép được** (FR-063a).
- **`GR-037` C8 vs C6 ở Về đích** — phán quyết *Huỷ kết quả* cho người cướp quyền: đã được `docs/game-rules.md` giải tường minh là **C8 thắng C6**, quy tắc phát biểu theo **loại phán quyết** chứ không theo vai bị chấm. **Không** phải conflict.
- **`GR-009` chỉ có Đúng/Sai vs `PRD-REQ-113` đòi ba lựa chọn cho tín hiệu Chướng ngại vật**: đã hoà giải tại nguồn — `GR-009` C6b và dòng cuối bảng §*Phán quyết có hai hay ba lựa chọn* nay khai tường minh ngoại lệ này (`QĐ-104`). **Không** phải conflict.
- **`GR-029` C4 *"điều chỉnh mồ côi"* vs cơ chế hoàn nguyên của `GR-030`**: nguồn nói thẳng đây là hành vi **đúng** — `SCORE_ADJUST` là sự kiện **của admin**, không thuộc vòng nào. **Không** phải conflict.

**Missing source behavior**

- **Bản gửi quá hạn tới SAU cú bấm *chốt câu* ở Tăng tốc**: tình huống **không dựng được**. Nút chấm và nút *chốt câu* ở kênh gõ chỉ mở **từ `hạn chót + padding`**, tức khi cửa sổ giữ bản tới muộn đã đóng và server đã từ chối mọi bản tới sau mốc đó. Vì vậy mọi bản **chấm được** luôn tới **trước** cú bấm *chốt câu*, và hệ thống MUST NOT có cơ chế tính lại bảng sau khi câu đã chốt (FR-033, FR-035).
- **Vòng đời câu ở Khởi động mode nhập liệu**: **không tồn tại** mốc bấm *"công bố đáp án"* riêng. `hạn chót` của câu là hạn nhận bài **duy nhất**; bản được ghi nhận là bản hợp lệ **cuối cùng trước `hạn chót`**; nút chấm mở **ngay tại `hạn chót`**. Đưa đáp án ra màn hình đi qua cú bấm **mở đáp án** chung của FR-065 (FR-009).
- **Băng điểm Chướng ngại vật cho `rowCount` 5-8**: `GR-009` C11 nói thẳng nguồn **không có thang nào**; v1 khoá cứng `rowCount = 4` nên **không chặn** feature này. Ghi lại để phiên bản sau không lấp bằng suy diễn.
- **Luật cho số ghế TRÊN 4**: chặn cứng ở cú bấm bắt đầu trận (`QĐ-105`, EPIC-005) nên **không chặn** feature này.

## Requirements *(mandatory)*

### Functional Requirements

#### Nhóm A — Bốn mốc bấm của một câu (US-001)

- **FR-001**: Mọi thời điểm mà luật gốc mô tả bằng **hành vi của MC** MUST ánh xạ thành **một cú bấm của admin** trên màn điều khiển; hệ thống MUST NOT suy ra các mốc đó từ bộ đếm hay từ bất kỳ tín hiệu tự động nào. *(US-001 · PRD-REQ-043 · GR-033 §Mục đích, §Kích hoạt · AC-001, AC-002)*
- **FR-002**: *Hiển thị câu hỏi* và *start timer* MUST là **hai nút riêng, thứ tự cố định** — hiển thị trước, start timer sau. Hệ thống MUST NOT cho đảo thứ tự, MUST NOT gộp hai nút, và MUST NOT có cấu hình nào gộp chúng lại. Trước khi *hiển thị câu* được bấm, nút *start timer* MUST chưa bật. *(US-001 · PRD-REQ-043 · GR-033 §Điều kiện, C4 · AC-001, AC-002, AC-007)*
- **FR-003**: Cú bấm *hiển thị câu hỏi* MUST tạo đúng bốn hệ quả tại mốc đó: đưa câu lên màn thí sinh và màn khán giả · **mở cửa sổ chuông** *(chỉ ở Khởi động lượt chung)* · **đóng cửa sổ đặt Ngôi sao hy vọng** *(mọi vòng)* · đánh dấu câu **đã dùng**. *(US-001 · PRD-REQ-043 · GR-033 C3, GR-003, GR-021, GR-031 §Ranh giới "đã dùng" · AC-001)*
- **FR-004**: Sau *hiển thị câu* và trước *start timer*, câu MUST đang hiện trên các màn trong khi đồng hồ **chưa chạy** — hệ thống MUST coi đây là trạng thái hợp lệ, không phải lỗi. *(US-001 · PRD-REQ-043 · GR-033 C4 · AC-004)*
- **FR-005**: Nút *start timer* MUST tự khoá ngay sau lần bấm đầu, để một cú bấm lặp MUST NOT kéo dài được cửa sổ thời gian. *(US-001 · PRD-REQ-043 · GR-033 C2, §Bấm trùng · AC-003)*
- **FR-006**: Mốc do admin bấm MUST tuyệt đối: hệ thống MUST NOT có cửa sổ ân hạn và MUST NOT trừ bù độ trễ tay người hay độ trễ mạng vào mốc đã ghi. *(US-001 · PRD-REQ-043 · GR-033 C6, §Điều kiện · AC-006)*
- **FR-007**: Ở **câu thực hành**, hệ thống MUST có một nút **mốc thứ ba** — *bắt đầu thực hành* — đóng pha suy nghĩ và mở pha thực hành; nút này MUST NOT tồn tại ở câu không được khai là thực hành. *(US-001, US-005 · PRD-REQ-043, PRD-REQ-054 · GR-019 §Điều kiện, §Thứ tự đánh giá · AC-008, AC-009)*
- **FR-008**: Ở **Câu hỏi phụ**, cú bấm *start timer* MUST đồng thời là mốc **hiệu lệnh** mở cửa sổ chuông; trước mốc đó nút chuông của thí sinh MUST chưa sống. *(US-001 · PRD-REQ-043 · GR-033 C5 · AC-005)*
- **FR-009**: Ở **Khởi động**, hệ thống MUST NOT có một mốc bấm *"công bố đáp án"* riêng: `hạn chót` của câu MUST là hạn nhận bài **duy nhất**, và bản được ghi nhận MUST là bản hợp lệ **cuối cùng trước `hạn chót`**. Việc đưa đáp án ra màn hình MUST đi qua cú bấm **mở đáp án** chung của FR-065 chứ MUST NOT là một mốc thứ ba của vòng đời câu. Vì vậy một câu MUST có tối đa **bốn** mốc bấm: hiển thị câu · start timer · bắt đầu thực hành *(chỉ câu thực hành)* · phán quyết. Việc neo bản được ghi nhận vào `hạn chót` là quy tắc **engine**, đặc tả ở `specs/006` FR-070. *(US-001, US-011 · PRD-REQ-043, PRD-REQ-057 · GR-006 §Điều kiện, C3, GR-037 · AC-011, AC-012)*
- **FR-010**: Trong lúc **banner tạm dừng đang bật**, nút *start timer* MUST NOT bấm được và hệ thống MUST phản hồi bằng toast invalid state. *(US-001, US-017 · PRD-REQ-043, PRD-REQ-056 · GR-035, GR-033 · AC-010)*

#### Nhóm B — Màn phán quyết và cú bấm chốt câu (US-002, US-006)

- **FR-011**: Số nút phán quyết MUST đổi theo vòng và pha: **hai** nút *Đúng / Sai* ở nơi mà *Sai* trừ **0** điểm — Khởi động lượt riêng, câu hàng ngang Vượt chướng ngại vật, Tăng tốc, Câu hỏi phụ, Về đích người thi chính; **ba** nút *Đúng / Sai / Huỷ kết quả* ở nơi mà *Sai* kéo theo hình phạt — Khởi động lượt chung, Về đích người cướp quyền, Về đích câu có Ngôi sao hy vọng. Hệ thống MUST NOT dùng chung một bố cục hai nút cho mọi vòng. *(US-002 · PRD-REQ-038, PRD-REQ-063 · GR-026, bảng §Phán quyết có hai hay ba lựa chọn, GR-004, GR-020, GR-021 · AC-013, AC-014, AC-021)*
- **FR-012**: Tín hiệu *"Mở chướng ngại vật"* MUST có **ba** nút phán quyết dù chấm *Sai* ở đó không trừ điểm — hình phạt là **bị loại khỏi vòng**. *(US-002, US-014 · PRD-REQ-038, PRD-REQ-113 · GR-009 C6b, bảng §Phán quyết dòng cuối, GR-010 · AC-023)*
- **FR-013**: Lựa chọn **Huỷ kết quả** MUST luôn có mặt khi một câu **chỉ có bản gửi quá hạn**, kể cả ở vòng mà *Sai* trừ 0 điểm. *(US-002, US-003 · PRD-REQ-038, PRD-REQ-052 · bảng §Bản gửi quá hạn C2, GR-006 C5, GR-015 C6 · AC-022)*
- **FR-014**: Phán quyết **Huỷ kết quả** MUST khép đối tượng được chấm mà **không sinh điểm cho ai** và MUST NOT áp hình phạt của nhánh *Sai*. *(US-002 · PRD-REQ-038 · GR-004 C3, GR-009 C6b, GR-020 C2 · AC-015)*
- **FR-015**: Khi admin **chưa bấm** nút phán quyết nào, hệ thống MUST NOT sinh điểm, MUST giữ câu ở trạng thái chưa chấm, MUST NOT hiện nút chuyển câu, và kết quả đối chiếu MUST chỉ tồn tại dưới dạng gợi ý hiển thị. *(US-002 · PRD-REQ-036 · GR-026 C3 · INV-003, INV-010 · AC-016)*
- **FR-016**: Sau khi một đối tượng đã được chấm, nút phán quyết của **đúng đối tượng đó** MUST tự khoá và server MUST từ chối một phán quyết trùng bộ ba *(câu, thí sinh, loại phán quyết)* kể cả khi yêu cầu lọt tới, MUST NOT sinh sự kiện cho lần bấm đó. *(US-002 · PRD-REQ-036, PRD-REQ-038 · GR-026 §Bấm trùng, INV-009 · AC-019)*
- **FR-017**: Nút phán quyết MUST bấm được **sau khi** cửa sổ thời gian của câu đã đóng — đồng hồ khoá thí sinh, MUST NOT khoá admin. *(US-002, US-005 · PRD-REQ-054 · GR-026 §Biên, INV-015 · AC-020)*
- **FR-018**: Ở **Tăng tốc** và **câu hàng ngang Vượt chướng ngại vật**, dấu Đúng/Sai đặt cho từng ghế MUST là **lựa chọn tạm**: admin MUST sửa lại được không giới hạn số lần cho tới mốc bấm *chốt câu*, và hệ thống MUST NOT sinh sự kiện điểm trước mốc đó. *(US-002, US-004, US-006 · PRD-REQ-036, PRD-REQ-062 · GR-013 §Bấm trùng, GR-008 §Thứ tự đánh giá, INV-009 · AC-024, AC-026)*
- **FR-019**: Ở **Tăng tốc** và **câu hàng ngang Vượt chướng ngại vật**, cú bấm *chốt câu* MUST chính là phán quyết cho mọi ghế chưa được chấm, và những ghế đó MUST tính là **Sai**: ở Tăng tốc nhận 0 và **không giữ chỗ** trong thang xếp hạng; ở hàng ngang VCNV nhận 0 và **không bị loại** khỏi vòng. Hệ thống MUST NOT có bộ đếm tự chốt khi hết giờ. *(US-006 · PRD-REQ-062 · GR-026 C4, GR-013 C6 · AC-017)*
- **FR-020**: Dialog xác nhận của cú bấm *chốt câu* MUST liệt kê **tên các ghế** sắp bị mặc định Sai. *(US-006 · PRD-REQ-062 · GR-026 C4 · AC-017)*
- **FR-021**: Quy tắc mặc định Sai của *chốt câu* MUST NOT áp lên tín hiệu *"Mở chướng ngại vật"* đang chờ duyệt — MUST NOT tồn tại đường nào để một cú bấm chốt câu loại một thí sinh khỏi vòng. *(US-006 · PRD-REQ-062 · GR-026 C5, GR-010, INV-010 §Ngoại lệ tường minh · AC-018)*
- **FR-022**: Sau cú bấm *chốt câu* ở hai vòng chấm theo lô, hệ thống MUST NOT còn đường đổi phán quyết của bất kỳ ghế nào trong câu đó, và MUST NOT tính lại thứ hạng — sửa sai MUST đi qua điều chỉnh điểm thủ công. *(US-002, US-004, US-006 · PRD-REQ-036, PRD-REQ-062 · GR-013 §Bấm trùng, INV-009 · AC-026)*
- **FR-023**: Nút phán quyết MUST NOT bấm được cho một câu thuộc **vòng đã bị bỏ**; hệ thống MUST phản hồi bằng toast invalid state. *(US-002, US-008 · PRD-REQ-063 · GR-013 C7, GR-030 · AC-025)*
- **FR-024**: Bộ nút phán quyết MUST đổi ngay khi admin chuyển giữa hai pha có hình phạt khác nhau, MUST NOT đòi một thao tác cấu hình trung gian nào. *(US-002 · PRD-REQ-063 · GR-026, bảng §Phán quyết · AC-021)*

#### Nhóm C — Màn chấm, đối chiếu và bản quá hạn (US-003, US-004, US-005)

- **FR-025**: Màn chấm MUST hiển thị bài làm của một ghế **cạnh** đáp án và MUST tô khác biệt **theo ký tự** so với đáp án **gần nhất** trong danh sách đáp án được chấp nhận, đồng thời MUST cho xem **cả danh sách**. *(US-003 · PRD-REQ-051 · GR-027 §Thứ tự đánh giá · AC-027)*
- **FR-026**: Phép chuẩn hoá dùng cho việc tô MUST gồm ba bước **bắt buộc** — không phân biệt hoa thường, cắt khoảng trắng hai đầu, gộp khoảng trắng thừa — và một bước **tuỳ chọn** là bỏ dấu tiếng Việt, mặc định **TẮT**. Bước tuỳ chọn MUST chỉ đổi cách tô màu và MUST NOT đổi phán quyết. *(US-003 · PRD-REQ-051 · GR-027 §Điều kiện, C2, C3 · AC-028, AC-029, AC-030)*
- **FR-027**: Phép chuẩn hoá MUST là hàm thuần, tất định. Bài làm **gốc** MUST được lưu; kết quả chuẩn hoá MUST chỉ là dữ liệu tạm để hiển thị. Danh sách đáp án được chấp nhận MUST cố định từ lúc soạn câu và MUST NOT sửa được giữa trận. *(US-003 · PRD-REQ-051 · GR-027 §Điều kiện, §Không đổi gì · AC-033)*
- **FR-028**: Với bài làm **rỗng hoặc không gửi**, hệ thống MUST NOT so và MUST NOT tô — trạng thái hiển thị MUST là *"không có đáp án"*. *(US-003 · PRD-REQ-051 · GR-027 C5 · AC-032)*
- **FR-029**: Màn chấm MUST giữ **số chữ của đáp án** như một gợi ý phụ. *(US-003 · PRD-REQ-051 · GR-027 · AC-027)*
- **FR-030**: Admin MUST xem được **lịch sử các bản đã gửi** của một ghế trong một câu; hệ thống MUST NOT xoá bản nào khỏi lịch sử đó. *(US-003 · PRD-REQ-051 · GR-006 §Không đổi gì, GR-015 §Không đổi gì, INV-001 · AC-034)*
- **FR-031**: Khi một ghế có bản gửi **hợp lệ** và còn gửi thêm bản **quá hạn**, hệ thống MUST giữ **cả hai**, MUST đánh dấu bản quá hạn bằng màu **đỏ**, và MUST bật nút chấm cho cả hai. Bản quá hạn MUST NOT tự ghi đè bản hợp lệ và hệ thống MUST NOT tự loại nó. *(US-003 · PRD-REQ-052 · bảng §Bản gửi quá hạn C1, GR-006 C4, GR-015 C5, GR-035 C6 · AC-035, AC-037)*
- **FR-032**: Khi một câu **chỉ có** bản quá hạn, hệ thống MUST giữ nó, tô **đỏ**, và MUST cho thêm lựa chọn **Huỷ kết quả**. *(US-003 · PRD-REQ-052 · bảng §Bản gửi quá hạn C2, GR-006 C5, GR-015 C6 · AC-036)*
- **FR-033**: Cả việc **hiển thị** lẫn việc **chấm** một bản quá hạn MUST là thao tác bấm của admin; hệ thống MUST chỉ đánh dấu và MUST NOT chặn cứng. Khi admin công nhận một bản quá hạn ở **Tăng tốc**, bảng xếp hạng của câu đó MUST tính trên dấu đã sửa tại **cú bấm *chốt câu***; vì FR-035 và **cửa sổ giữ bản tới muộn** *(đặc tả ở `specs/006` FR-070a)* bảo đảm mọi bản chấm được **luôn** tới trước cú bấm đó, hệ thống MUST NOT cần và MUST NOT có cơ chế tính lại bảng sau khi câu đã chốt. *(US-003 · PRD-REQ-052 · GR-035 C6, GR-015 C6, INV-009 · AC-038, AC-038a)*
- **FR-034**: Ở **Tăng tốc**, admin MUST chấm toàn bộ các ghế của một câu trên **một** màn hiển thị cạnh nhau — bài làm, mốc thời gian và bộ nút của mọi ghế MUST cùng nằm trong một khung nhìn; hệ thống MUST NOT tách việc chấm từng người ra các màn rời. *(US-004 · PRD-REQ-053 · GR-013 §Mục đích, §Thứ tự đánh giá · AC-039)*
- **FR-035**: Thời điểm admin chấm được MUST phụ thuộc **kênh trả lời**, không phụ thuộc vòng: kênh **nói** *(mode sân khấu)* MUST cho bấm chấm bất cứ lúc nào; kênh **gõ** *(mode nhập liệu, và Vượt chướng ngại vật cùng Tăng tốc vốn luôn gõ máy)* MUST khoá nút chấm và nút *chốt câu* tới mốc **`hạn chót + padding`** — tức tới khi **cửa sổ giữ bản tới muộn** đã đóng *(quy tắc cửa sổ đặc tả ở `specs/006` FR-070a, không thuộc spec này)*, **không** chỉ tới `hạn chót`; riêng **Khởi động**, vốn không có biên trên, MUST mở nút chấm từ **`hạn chót`**; kênh **thực hành** MUST để nút chấm sống suốt. *(US-005, US-006 · PRD-REQ-054 · bảng §Kênh trả lời, GR-019 §Điều kiện, GR-035 C6 · AC-040, AC-041, AC-042, AC-038a, AC-011a)*
- **FR-036**: Khi nút chấm đang mờ, giao diện MUST nêu rõ **lý do** nút đang mờ. *(US-005 · PRD-REQ-054 · GR-019, bảng §Kênh trả lời · AC-041)*
- **FR-037**: Ở **mode sân khấu**, hệ thống MUST NOT có bài làm dạng chữ để tô; các quy tắc ghi nhận bản đầu/bản cuối, mốc thời gian và tô nổi bật MUST vẫn tồn tại nhưng MUST chỉ là không được kích hoạt. Ở **câu thực hành**, cơ chế chuẩn hoá và tô khác biệt MUST NOT áp. *(US-003, US-005 · PRD-REQ-051, PRD-REQ-054 · GR-027 §Ở mode sân khấu, GR-019 §Điều kiện · AC-042a)*

#### Nhóm D — Điều chỉnh điểm thủ công (US-007)

- **FR-038**: Admin MUST cộng hoặc trừ một lượng bất kỳ cho một ghế kèm **lý do**, ở **mọi lúc trận chưa đóng sổ** — cả ở trạng thái nghỉ lẫn giữa một vòng đang chạy. *(US-007 · PRD-REQ-055 · GR-029 §Điều kiện, C1 · AC-043, AC-051)*
- **FR-039**: **Lý do là bắt buộc**: khi ô lý do trống, hệ thống MUST NOT sinh sự kiện và server MUST từ chối yêu cầu kể cả khi nó lọt tới. Lý do MUST được cắt khoảng trắng hai đầu ở cả hai đầu hệ thống, nên một lý do chỉ gồm khoảng trắng MUST bị coi là trống. *(US-007 · PRD-REQ-055 · GR-029 §Điều kiện · CLAUDE.md §Quy ước code · AC-049)*
- **FR-040**: Lượng điều chỉnh **bằng 0** MUST vẫn sinh sự kiện — đó là một ghi chú chính thức vào biên bản trận. *(US-007 · PRD-REQ-055 · GR-029 C3 · AC-045)*
- **FR-041**: Lượng điều chỉnh **âm** MUST hợp lệ và MUST NOT bị kẹp về 0; điểm sau điều chỉnh MUST được phép âm. *(US-007 · PRD-REQ-055 · GR-029 C2, INV-018 · AC-044)*
- **FR-042**: Sự kiện điều chỉnh điểm MUST NOT bị hoàn nguyên khi một vòng bị bỏ hay chạy lại — nó là phán quyết **của người** và không thuộc vòng nào. Thứ tự giữa điều chỉnh và cửa ra của vòng MUST NOT đổi kết quả này. *(US-007, US-008 · PRD-REQ-055 · GR-029 C4, C5, §ghi chú, GR-030 §Không đổi gì · AC-046, AC-047)*
- **FR-043**: Hệ thống MUST NOT chống trùng cho thao tác điều chỉnh điểm: hai lần cùng lượng liên tiếp MUST cộng dồn theo thứ tự bấm. *(US-007 · PRD-REQ-055 · GR-029 §Bấm trùng · AC-050)*
- **FR-044**: Ở trận **đã đóng sổ** — với **cả hai** nhãn *hoàn thành* và *bỏ dở* — nút điều chỉnh điểm MUST NOT bật, hệ thống MUST phản hồi bằng toast invalid state, và MUST NOT cho ép qua. *(US-007 · PRD-REQ-055 · GR-029 C6 · AC-048)*
- **FR-045**: Thao tác điều chỉnh điểm MUST đi qua dialog của **tầng riêng ở giữa**: `Esc` MUST đóng dialog và khi đóng bằng `Esc` hệ thống MUST NOT lưu thay đổi nào; click ra ngoài MUST NOT đóng dialog; ô lý do MUST bắt buộc; nút xác nhận MUST tự tắt sau khi bấm; dialog MUST nhận phím tắt `Y`/`N`; và thao tác MUST ghi vào nhật ký kèm người bấm, lý do và mốc thời gian. *(US-007, US-010 · PRD-REQ-055, PRD-REQ-056 · GR-029 §Điều kiện, C1, STATE-035 · AC-052)*

#### Nhóm E — Ba cửa ra chủ động của vòng, và chọn vòng (US-008, US-009)

- **FR-046**: Một vòng đang chạy MUST rời về trạng thái nghỉ qua đúng bốn cửa: **kết thúc vòng** *(đủ câu, giữ điểm)* · **kết thúc khẩn cấp** *(bấm được cả khi chưa đủ câu, giữ nguyên điểm, nhãn "kết thúc sớm")* · **bỏ vòng** *(hoàn nguyên điểm, nhãn "đã bỏ")* · **chạy lại vòng** *(hoàn nguyên rồi chạy mới)*. *(US-008 · PRD-REQ-031 · GR-030 §Mục đích, C1, C2, C3 · AC-053, AC-054, AC-055, AC-064)*
- **FR-047**: **Ba cửa sau** MUST đi qua dialog **hạng phá huỷ**: dialog MUST NOT tắt được bằng `Esc` hay click ra ngoài, và MUST bắt nhập lý do. Cửa **kết thúc vòng** thường MUST NOT đòi lý do. *(US-008, US-010 · PRD-REQ-031, PRD-REQ-056 · GR-030 §Điều kiện, STATE-035 · AC-058, AC-064)*
- **FR-048**: **Bỏ vòng** MUST hoàn nguyên điểm của vòng bằng **sự kiện đảo ngược thêm vào**, MUST NOT xoá sự kiện cũ, và biên bản MUST giữ vòng đó kèm nhãn *"đã bỏ"*. *(US-008 · PRD-REQ-031 · GR-030 C1, GR-028 C4, INV-001 · AC-053)*
- **FR-049**: **Chạy lại vòng** MUST hoàn nguyên rồi mở lại vòng từ đầu, và MUST đi qua **cửa kiểm kho đề** như một vòng mới; kho không đủ câu MUST chặn thao tác. Chạy lại MUST NOT bị chống trùng — mỗi lần là một lần chạy mới có chủ đích. *(US-008 · PRD-REQ-031 · GR-030 C2, §ghi chú, GR-031 · AC-054, AC-060)*
- **FR-050**: **Kết thúc khẩn cấp** MUST khép vòng ngay tại chỗ mà **giữ nguyên điểm**: hệ thống MUST NOT sinh sự kiện đảo ngược nào, và biên bản MUST mang nhãn *"kết thúc sớm"*. *(US-008 · PRD-REQ-031 · GR-030 C3, EVENT-004 · AC-055)*
- **FR-051**: Ở **kết thúc khẩn cấp**, câu đang mở MUST khép bằng **Huỷ kết quả** — không sinh điểm cho ai — và hệ thống MUST NOT ép mọi ghế thành Sai. *(US-006, US-008 · PRD-REQ-031, PRD-REQ-062 · GR-030 C3, EVENT-004 §side effect 2, INV-010 · AC-059)*
- **FR-052**: **Cả ba cửa** MUST NOT trả câu **đã hiển thị** về kho. *(US-008, US-015 · PRD-REQ-031 · GR-030 §Điều kiện, §Không đổi gì, INV-011 · AC-061)*
- **FR-053**: Ra lệnh **bỏ vòng** cho một vòng **đã ở nhãn đã bỏ** MUST bị server từ chối và MUST NOT làm điểm tụt lần thứ hai; hệ thống MUST phản hồi bằng toast invalid state, MUST NOT xử lý như một cú bấm trùng. *(US-008, US-010 · PRD-REQ-031, PRD-REQ-056 · GR-030 C5, §ghi chú · AC-057)*
- **FR-054**: Trong màn của một vòng đang chạy, hệ thống MUST NOT có nút mở một vòng khác; admin MUST rời vòng hiện tại qua một trong bốn cửa rồi mới mở vòng kế. *(US-008, US-009 · PRD-REQ-031 · GR-030 C4 · AC-056)*
- **FR-055**: Admin MUST chọn được vòng nào mở tiếp từ trạng thái nghỉ. Hệ thống MUST **khuyến nghị** theo luật và cấu hình contest, MUST NOT cưỡng chế thứ tự đó. *(US-009 · PRD-REQ-030 · GR-030 C4, INV-020 · AC-062, AC-063)*
- **FR-056**: Khi admin chọn một vòng **khác** khuyến nghị, hệ thống MUST hiện dialog cảnh báo nêu **tên luật bị lệch**, MUST cho ép qua bằng Yes, và MUST NOT chặn cứng. Bấm No MUST không để lại thay đổi nào. *(US-009, US-010 · PRD-REQ-030, PRD-REQ-056 · GR-030 C4, INV-014, INV-020 · AC-062)*
- **FR-057**: Khi admin chọn đúng vòng được khuyến nghị, hệ thống MUST NOT hiện dialog cảnh báo nào. *(US-009 · PRD-REQ-030 · INV-020 · AC-063)*

#### Nhóm F — Ba hạng cảnh báo giao diện (US-010)

- **FR-058**: Thao tác ở **invalid state** MUST phản hồi bằng **toast**: hệ thống MUST nêu rõ vì sao, MUST NOT bắt nhập lý do, và MUST NOT cho ép qua. *(US-010 · PRD-REQ-056 · GR-029 C6, GR-030 C5, PRD §9.4 · AC-065, AC-071)*
- **FR-059**: **Mỗi loại invalid state** MUST có thông điệp riêng; hệ thống MUST NOT dùng chung một thông điệp cho nhiều nguyên nhân khác nhau. *(US-010 · PRD-REQ-056 · PRD §9.4 · AC-065)*
- **FR-060**: Thao tác **không hoàn tác được nhưng hợp lệ** MUST đi qua dialog **Yes/No thường**: mở đáp án, mở ô chữ, xác nhận một tín hiệu trong hàng đợi chặn. Ở tầng này, `Esc` **và** click ra ngoài đều MUST đóng dialog và đều MUST tương đương **No**; dialog MUST nhận phím tắt `Y`/`N`; và hệ thống MUST NOT bắt nhập lý do. *(US-010, US-011, US-012, US-013 · PRD-REQ-056, PRD-REQ-057 · STATE-035, GR-032 · AC-066)*
- **FR-061**: Thao tác **hạng phá huỷ** — bỏ vòng, chạy lại vòng, kết thúc sớm, huỷ trận — MUST đi qua dialog **không tắt được bằng `Esc` hay click ra ngoài**, MUST bắt nhập lý do, và MUST NOT nhận phím tắt `Y`/`N` — tầng này chỉ nhận click chuột. *(US-008, US-010 · PRD-REQ-056 · GR-030 §Điều kiện, STATE-035, GR-034 · AC-067)*
- **FR-061a**: Hệ thống MUST phân biệt **ba tầng dialog** bằng **cách thoát**, và *"bắt nhập lý do"* MUST là một trục **độc lập** với tầng chứ không phải thuộc tính suy ra từ tầng: hạng phá huỷ *(không `Esc`, không click ngoài, có lý do, không phím tắt)* · điều chỉnh điểm *(có `Esc` và không lưu, không click ngoài, có lý do, có phím tắt)* · Yes/No thường *(có `Esc` và có click ngoài, cả hai = No, không lý do, có phím tắt)*. Hệ thống MUST NOT gán một tầng cho một thao tác chỉ vì thao tác đó bắt nhập lý do. *(US-007, US-008, US-010 · PRD-REQ-056, PRD-REQ-055 · GR-029 §Điều kiện, GR-030 §Điều kiện, STATE-035 · AC-052, AC-066, AC-067, AC-069)*
- **FR-062**: Thao tác **đóng** một hiển thị MUST NOT có dialog xác nhận. *(US-010, US-011, US-017 · PRD-REQ-056 · GR-037, STATE-040 · AC-068)*
- **FR-063**: Các hạng phản hồi MUST phân biệt được **bằng mắt** — toast invalid state và **ba tầng dialog** của FR-061a; hệ thống MUST NOT trình bày một toast giống một dialog, và MUST NOT trình bày hai tầng dialog giống hệt nhau. *(US-009, US-010, US-014 · PRD-REQ-056 · PRD §9.4, STATE-035 → STATE-037 · AC-069, AC-070)*
- **FR-063a**: **Dialog cảnh báo lệch luật** MUST dùng lại **tầng Yes/No thường** của FR-061a, MUST NOT là một tầng riêng, và MUST NOT đòi khác biệt thị giác với dialog xác nhận: `Esc` và click ra ngoài đều MUST đóng và đều MUST tương đương **No**, MUST nhận phím tắt `Y`/`N`, và MUST NOT bắt nhập lý do. Khác biệt duy nhất so với dialog xác nhận MUST là **nội dung chữ** — cảnh báo nêu **điểm lệch luật**, xác nhận nêu **hậu quả không hoàn tác được**. Hệ thống MUST vẫn giữ khác biệt về **quyền** giữa cảnh báo *(ép được)* và toast invalid state *(không ép được)*. *(US-009, US-010, US-012, US-014 · PRD-REQ-056 · PRD §9.4, STATE-036, STATE-037 · AC-070, AC-087, AC-101)*
- **FR-064**: Phía **thí sinh** MUST NOT có toast invalid state — ở đó nút **không render** và bấm **không phản hồi**, nên không có gì để báo. *(US-010 · PRD-REQ-056 · STATE-037, PRD §9.4 · AC-072)*

#### Nhóm G — Mở và đóng đáp án (US-011)

- **FR-065**: Admin MUST mở và đóng đáp án của câu đang chạy **không điều kiện**; hệ thống MUST NOT có khái niệm engine tự mở. *(US-011 · PRD-REQ-057 · GR-037 §Không đổi gì, PERM-044 · AC-073, AC-077)*
- **FR-066**: Thao tác **mở** đáp án MUST đi qua dialog Yes/No. *(US-011, US-010 · PRD-REQ-057, PRD-REQ-056 · STATE-035 · AC-075)*
- **FR-067**: Khi admin bấm mở đáp án, nội dung MUST tới **mọi vai đang xem** — máy thí sinh, màn khán giả và lớp phủ dựng stream — kể cả khi cờ *hiện đáp án sau khi chấm* của trận đang **tắt**. *(US-011 · PRD-REQ-057 · GR-037, PERM-044 · AC-073)*
- **FR-068**: Mở và đóng hiển thị đáp án MUST NOT đụng tới điểm, MUST NOT sinh sự kiện điểm, và MUST NOT đổi trạng thái trận, vòng, câu, ghế hay tín hiệu. *(US-011 · PRD-REQ-057 · GR-037 §Không đổi gì, INV-021 · AC-074)*
- **FR-069**: Mỗi lần mở đáp án MUST vào nhật ký thao tác. *(US-011 · PRD-REQ-057 · GR-037 §Điều kiện · AC-080)*
- **FR-070**: Quyền mở và đóng đáp án MUST độc quyền của **phiên đang giữ quyền điều khiển**; màn MC MUST NOT có nút nào cho thao tác này. *(US-011 · PRD-REQ-057 · GR-037 C2, PERM-044 vs PERM-045 · AC-076)*
- **FR-071**: Câu khép bằng phán quyết **Huỷ kết quả** MUST NOT tự công bố đáp án; đường duy nhất để đáp án hiện ra ở đó MUST là admin mở tay. Quy tắc này MUST áp cho **mọi** vị trí trong chuỗi, kể cả khi *Huỷ kết quả* là phán quyết khép câu cho người cướp quyền Về đích. *(US-011 · PRD-REQ-057 · GR-037 C8 · AC-078)*
- **FR-072**: Trong lúc hệ thống đang chờ admin **kích hoạt tay** một tín hiệu khác sau một cú *Huỷ kết quả*, đáp án MUST NOT được công bố tới thí sinh, khán giả hay lớp phủ — mốc câu khép đã lùi. *(US-011, US-014 · PRD-REQ-057, PRD-REQ-113 · GR-037 C8b, GR-032 §Kích hoạt tay · AC-079)*
- **FR-073**: Ở vòng Vượt chướng ngại vật, admin MUST bấm được nút **công bố Chướng ngại vật** để mở toàn bộ miếng ghép; thao tác này MUST là **thủ công và tuỳ chọn** — vòng MUST kết thúc được mà không cần nó. *(US-011, US-012 · PRD-REQ-057, PRD-REQ-058 · GR-012 C2, C3, EVENT-020 · AC-090, AC-090a)*

#### Nhóm H — Bàn cờ Vượt chướng ngại vật (US-012)

- **FR-074**: Mỗi ô của bàn cờ — 4 hàng ngang và ô trung tâm — MUST mang đúng **một** trong ba giá trị **chờ · đã hỏi · mở**, và **năm ô MUST độc lập** với nhau. *(US-012 · PRD-REQ-058 · STATE-041 → STATE-043 · AC-081)*
- **FR-075**: Admin MUST đặt thẳng một ô sang giá trị bất kỳ, **cả hai chiều**, ở **mọi giai đoạn** của vòng — kể cả khi một câu khác đang mở và đồng hồ đang chạy. Thao tác MUST đi qua dialog Yes/No và MUST vào nhật ký thao tác. *(US-012 · PRD-REQ-058 · EVENT-021, T-084 · AC-082, AC-084, AC-086)*
- **FR-076**: Băng điểm Chướng ngại vật MUST là **hàm** đọc theo **số hàng ngang không còn ở trạng thái chờ**, MUST NOT là bộ đếm cộng dồn. Ô trung tâm MUST NOT vào phép tính này. Sau khi gợi ý cuối đã được đưa ra, băng MUST là **20** và 20 MUST là sàn, không phụ thuộc câu ô trung tâm đúng hay sai. *(US-012 · PRD-REQ-058 · GR-009 §Điều kiện, GR-011 C1, C3, STATE-042 · AC-089)*
- **FR-077**: Đặt tay một ô sang **đã hỏi** MUST làm băng tụt đúng một bậc y như một lượt hỏi thật, và MUST NOT cộng điểm cho ai. *(US-012 · PRD-REQ-058 · GR-009 C12 · AC-082)*
- **FR-078**: Đặt tay một ô sang **mở** MUST NOT đổi băng điểm — biến mà băng đọc là *đã hỏi*, không phải *đã lộ*. *(US-012 · PRD-REQ-058 · GR-009 C13, STATE-043 · AC-083)*
- **FR-079**: Đặt một ô **lùi** về giá trị trước đó MUST làm băng điểm **lên lại** theo đúng hàm ở FR-076. *(US-012 · PRD-REQ-058 · T-084 · AC-084)*
- **FR-080**: Đặt một ô về **đúng giá trị nó đang có** MUST là thao tác rỗng — phép gán, không phải phép cộng. *(US-012 · PRD-REQ-058 · T-085 · AC-085)*
- **FR-081**: Đặt trạng thái ô chữ MUST NOT tiêu câu, MUST NOT đụng đồng hồ, và MUST NOT đụng hàng đợi tín hiệu. *(US-012 · PRD-REQ-058 · EVENT-021 §Ba ranh giới · AC-086)*
- **FR-082**: Đặt trạng thái của ô thuộc **câu đang được hỏi dở** MUST hiện dialog cảnh báo lệch luật và MUST cho ép qua. *(US-012, US-010 · PRD-REQ-058, PRD-REQ-056 · §Invalid transitions, STATE-036 · AC-087)*
- **FR-083**: Ngoài vòng Vượt chướng ngại vật, nút đặt trạng thái ô chữ MUST NOT tồn tại. *(US-012 · PRD-REQ-058 · EVENT-021 · AC-088)*
- **FR-084**: Nút **mở miếng ghép**, nút **đưa ra gợi ý cuối / mở ô trung tâm** và nút **công bố Chướng ngại vật** MUST là nút một chiều tự tắt sau lần bấm đầu; server MUST bỏ qua lệnh trùng. *(US-012, US-018 · PRD-REQ-058 · GR-011 C8, GR-012 C5, EVENT-018, EVENT-019 · AC-090, AC-090b, AC-090c)*

#### Nhóm I — Màn hàng đợi và kích hoạt tay (US-013, US-014)

- **FR-085**: Màn admin MUST hiện hàng đợi tín hiệu **đầy đủ** kèm server timestamp của từng tín hiệu và trạng thái *"không có hiệu lực"* khi có. *(US-013 · PRD-REQ-059 · GR-032, INV-006 · AC-098)*
- **FR-086**: Màn admin MUST **tách bạch** hàng đợi **đang hoạt động** với **lịch sử tín hiệu**; sau khi hàng đợi đang hoạt động được đặt lại, lịch sử MUST vẫn xem được nguyên vẹn. *(US-013 · PRD-REQ-059 · GR-032 C5, §Vòng đời tín hiệu, INV-001 · AC-095, AC-097, AC-098)*
- **FR-087**: Ở **Vượt chướng ngại vật**, admin MUST duyệt hoặc từ chối từng tín hiệu đang chờ; hệ thống MUST xử lý **FIFO thuần theo server timestamp** và MUST NOT ưu tiên theo loại tín hiệu, số ghế hay vị trí. *(US-013 · PRD-REQ-059 · GR-032 C1, C2, §Đồng thời, EVENT-022, INV-007 · AC-091, AC-092, AC-099)*
- **FR-088**: Admin bấm **No** cho một tín hiệu MUST đưa tín hiệu kế tiếp lên, MUST giữ nguyên lượt của ghế bị từ chối, và MUST NOT để lại tác dụng phụ nào — ô chữ chưa đánh dấu đã hỏi, câu chưa tiêu, đồng hồ chưa chạy. *(US-013 · PRD-REQ-059 · GR-032 C3, GR-009 C8, INV-008 · AC-093)*
- **FR-089**: Nút duyệt và nút từ chối của một tín hiệu MUST tự tắt sau lần bấm đầu; server MUST bỏ qua lệnh trùng, và một tín hiệu đã bị từ chối MUST NOT bị từ chối lần nữa. *(US-013, US-018 · PRD-REQ-059 · GR-032 §Bấm trùng, EVENT-022, EVENT-023 · AC-099)*
- **FR-090**: Ở các vòng **không chặn** — Khởi động lượt chung, cướp quyền Về đích, Câu hỏi phụ — màn hàng đợi MUST hiện thứ tự tín hiệu làm lưới an toàn nhưng MUST NOT có nút duyệt, vì tín hiệu ở đó có hiệu lực ngay theo server timestamp. *(US-013 · PRD-REQ-059 · GR-032 C4 · AC-094)*
- **FR-091**: Tín hiệu từ một ghế **đã bị loại** khỏi vòng Vượt chướng ngại vật MUST NOT xuất hiện trong hàng đợi đang hoạt động, vì máy thí sinh không hiển thị gì và không tín hiệu nào được tạo. *(US-013 · PRD-REQ-059 · GR-032 C6, GR-010 C4 · AC-096)*
- **FR-092**: Tín hiệu **bị từ chối** MUST vẫn nằm trong lịch sử để admin xem lại và **gỡ lệnh cấm** khi cần. *(US-013 · PRD-REQ-059 · GR-032 C7, INV-001 · AC-097)*
- **FR-093**: Khi người đang giữ quyền trả lời bị chấm **Huỷ kết quả**, admin MUST **kích hoạt tay** được **một tín hiệu còn hiệu lực** trong hàng đợi để người phát tín hiệu đó thành người giữ quyền mới. Phạm vi MUST là **mọi vòng có giành quyền bằng chuông** — Khởi động lượt chung, cướp quyền Về đích, Câu hỏi phụ, và tín hiệu *"Mở chướng ngại vật"* của Vượt chướng ngại vật. *(US-014 · PRD-REQ-113 · GR-032 C8, §Kích hoạt tay, EVENT-052 · AC-100)*
- **FR-094**: Trigger của kích hoạt tay MUST **chỉ** là phán quyết *Huỷ kết quả*; sau phán quyết **Đúng** hoặc **Sai**, nhánh này MUST NOT tồn tại và nút MUST NOT bật. *(US-014 · PRD-REQ-113 · GR-032 §Kích hoạt tay, EVENT-052 · AC-106)*
- **FR-095**: Tập ứng viên MUST gồm đúng hai nhóm — tín hiệu **trơ chưa từng được xử lý** *(kể cả tín hiệu đang chờ duyệt trong hàng đợi chặn của Vượt chướng ngại vật)*, và ghế **vừa bị chấm Huỷ kết quả** ở chính câu đó *(chính vòng đó ở Vượt chướng ngại vật)*. Hệ thống MUST loại khỏi tập ứng viên: ghế **bị loại** khỏi vòng Vượt chướng ngại vật, và ghế đang bị **vô hiệu hoá**. Ứng viên MUST còn hiệu lực với **đích** của tín hiệu. *(US-014 · PRD-REQ-113 · GR-032 §Kích hoạt tay, T-099, T-100, GR-010, INV-019 · AC-105)*
- **FR-096**: Hệ thống MUST **khuyến nghị** tín hiệu sớm nhất chưa xử lý theo server timestamp và MUST cho admin chọn ứng viên khác. Chọn lệch khuyến nghị MUST hiện **dialog cảnh báo lệch luật** Yes/No nêu **ghế bị vượt** và **thứ tự gốc**, MUST cho ép qua, MUST NOT bắt nhập lý do, và MUST NOT trở thành một chỗ chặn cứng thứ tư. *(US-014, US-010 · PRD-REQ-113 · GR-032 C9, INV-014, INV-020 · AC-101)*
- **FR-097**: Khi kích hoạt tay, server MUST cấp lại **trọn** cửa sổ suy nghĩ của vòng tính từ **mốc admin bấm kích hoạt** — Khởi động lượt chung **3 giây**, Câu hỏi phụ **15 giây**; Về đích và tín hiệu Chướng ngại vật MUST NOT mở cửa sổ nào vì hai chỗ đó không có cửa sổ trả lời riêng. *(US-014 · PRD-REQ-113 · GR-032 §Kích hoạt tay, T-097 · AC-102)*
- **FR-098**: Hạn mức kích hoạt tay MUST theo **đích** của tín hiệu: ở ba vòng chuông *(đích là **câu**)* MUST là **đúng một lần cho mỗi câu**; ở Vượt chướng ngại vật *(đích là **vòng**)* MUST là **một lần cho mỗi phán quyết Huỷ kết quả**, chuỗi dừng khi hàng đợi tín hiệu Chướng ngại vật của vòng cạn, và hệ thống MUST NOT áp trần tổng số lần trong một vòng. Vượt hạn mức MUST phản hồi bằng **toast** invalid state, MUST NOT cho ép qua. *(US-014 · PRD-REQ-113 · GR-032 §Kích hoạt tay, T-098 · AC-103, AC-104)*
- **FR-099**: Người được kích hoạt MUST chịu **y hệt** số học của vòng như một người giành quyền bình thường; hệ thống MUST NOT tạo một bảng điểm riêng cho họ. *(US-014 · PRD-REQ-113 · GR-032 §Kích hoạt tay, GR-003, GR-004, GR-020, GR-023 · AC-106a)*
- **FR-100**: Mọi cú kích hoạt tay MUST vào nhật ký thao tác kèm **ghế được chọn**, **ghế bị vượt**, và **thứ tự gốc**. Tín hiệu được kích hoạt MUST chuyển từ *không có hiệu lực* sang *có hiệu lực* và MUST giữ **cả hai** trạng thái trong lịch sử. Thao tác này MUST NOT sinh sự kiện điểm, MUST NOT đụng cờ đã-dùng của câu, và MUST NOT đổi trạng thái nào của bàn cờ Vượt chướng ngại vật. *(US-014 · PRD-REQ-113 · EVENT-052 §Side effects, INV-001 · AC-106b)*

#### Nhóm J — Ba trạng thái câu trên màn admin (US-015)

- **FR-101**: Màn admin MUST phân biệt **bằng mắt** ba trạng thái của một câu trong danh sách đã gán: **chưa rút** · **đã rút chưa hiển thị** *(gỡ được)* · **đã hiển thị** *(không gỡ được)*. *(US-015 · PRD-REQ-060 · GR-031 §Ranh giới "đã dùng", STATE-017, STATE-018 · AC-107)*
- **FR-102**: Ở trạng thái nghỉ, admin MUST gỡ được một câu **đã rút mà chưa hiển thị** khỏi danh sách gán, và câu đó MUST quay về kho. *(US-015 · PRD-REQ-060 · GR-031 C7, INV-011 · AC-108)*
- **FR-103**: Gỡ một câu **đã hiển thị** MUST NOT thực hiện được: hệ thống MUST phản hồi bằng toast invalid state và MUST NOT cho ép qua. *(US-015 · PRD-REQ-060 · GR-031 C6, INV-011 · AC-109)*
- **FR-104**: Cửa sửa danh sách câu MUST chỉ mở ở **trạng thái nghỉ**; giữa một vòng đang chạy hệ thống MUST phản hồi bằng toast. Sau khi sửa, phép kiểm kho đề MUST chạy lại, và cờ đã-dùng của câu MUST NOT bị đụng tới. *(US-015 · PRD-REQ-060 · GR-031 C5, C8, EVENT-031 · AC-109a, AC-109b)*

#### Nhóm K — Ghế: vô hiệu hoá, kích hoạt lại, mất kết nối (US-016)

- **FR-105**: Admin MUST tắt và bật lại quyền thao tác của một ghế, **đảo ngược được**, ở **mọi lúc** trong một trận chưa đóng sổ — kể cả giữa một câu đang mở và đồng hồ đang chạy, và với ghế đang kết nối lẫn ghế đang mất kết nối. Thao tác MUST đi qua dialog Yes/No, MUST đòi **lý do**, và MUST vào nhật ký thao tác. *(US-016 · PRD-REQ-061 · GR-036 C5, EVENT-030 · AC-110)*
- **FR-106**: Ghế bị vô hiệu hoá MUST **ở lại trong trận**: giữ nguyên điểm, giữ nguyên vị trí, vẫn hiện trên mọi bảng; ghế đó MUST chỉ mất quyền thao tác và MUST NOT được đề xuất lượt. Tín hiệu lỡ tới từ ghế đó MUST vào lịch sử ở trạng thái **trơ**, MUST NOT bị drop. *(US-016 · PRD-REQ-061 · GR-036 C5, STATE-025, INV-006 · AC-110)*
- **FR-107**: **Kích hoạt lại** một ghế MUST NOT hoàn nguyên gì: điểm ghi trong lúc ghế bị vô hiệu hoá MUST giữ nguyên, và bài đã chấm MUST không đổi. *(US-016 · PRD-REQ-061 · GR-036 C6, §Không đổi gì · AC-111)*
- **FR-108**: Vô hiệu hoá MUST là một **quyết định riêng** của admin, MUST NOT là hệ quả tự động của mất kết nối; hai việc MUST đi qua hai nút khác nhau. *(US-016 · PRD-REQ-061 · GR-036 §Điều kiện · AC-119)*
- **FR-109**: Ghế bị vô hiệu hoá **giữa một câu đang mở** MUST được xử y như ghế không trả lời — mặc định **Sai** khi admin chốt câu; đường bù duy nhất MUST là điều chỉnh điểm thủ công. *(US-016, US-006, US-007 · PRD-REQ-061, PRD-REQ-062 · GR-036 C7, GR-026 C4 · AC-112)*
- **FR-110**: Ghế **quá ngưỡng chờ kết nối** MUST được **tô nổi bật** trên màn admin kèm thời lượng đã mất kết nối; hệ thống MUST NOT tự loại, tự xoá hay tự vô hiệu hoá ghế nào, và vòng đang chạy MUST tiếp tục bình thường. *(US-016 · PRD-REQ-061 · GR-036 C2, C4 · AC-113)*
- **FR-111**: Với một ghế quá ngưỡng, admin MUST có đúng **hai** lựa chọn — **giữ** và **gia hạn** — và cả hai MUST NOT phá gì. Admin MUST can thiệp được **trước** khi hết ngưỡng. *(US-016 · PRD-REQ-061 · GR-036 C3, C9, EVENT-029 · AC-114, AC-115)*
- **FR-112**: Ở trận **đã đóng sổ**, nút vô hiệu hoá và kích hoạt lại ghế MUST NOT bật. Bấm cùng chiều hai lần MUST là thao tác rỗng. *(US-016 · PRD-REQ-061 · EVENT-030 · AC-116, AC-117)*
- **FR-113**: Hệ thống MUST NOT có thao tác **kick** một ghế thí sinh ở v1. *(US-016 · PRD-REQ-061 · GR-036 §Điều kiện · AC-118)*

#### Nhóm L — Lớp phủ do admin điều khiển (US-017)

- **FR-114**: Admin MUST mở và đóng **lớp công bố kết quả** tuỳ ý, ở **mọi trạng thái cấp trận**. Hệ thống MUST **gợi ý** mở ở hai mốc — vừa kết thúc một vòng, và trận kết thúc — nhưng MUST NOT tự mở. *(US-017 · EPIC-007 §11 Scope · PRD-REQ-075 · STATE-033, EVENT-032, T-087 · AC-120, AC-121)*
- **FR-115**: Hệ thống MUST NOT có bộ đếm tự đóng cho lớp công bố kết quả hay cho banner tạm dừng; mốc đóng MUST là một cú bấm của admin. *(US-017 · EPIC-007 §11 Scope · PRD-REQ-075, PRD-REQ-076 · EVENT-033, EVENT-035 · AC-122)*
- **FR-116**: Mở hay đóng bất kỳ lớp phủ nào MUST NOT sinh sự kiện điểm, MUST NOT đổi trạng thái trận, vòng, câu, ghế hay tín hiệu, MUST NOT đụng đồng hồ và MUST NOT đụng hàng đợi; đóng lớp phủ MUST để trạng thái bên dưới lộ lại **nguyên vẹn**. *(US-017 · EPIC-007 §11 Scope · PRD-REQ-075 · INV-021, T-087 → T-090 · AC-120, AC-126)*
- **FR-117**: Nút mở **banner tạm dừng** MUST chỉ bật khi **không có cửa sổ thời gian nào đang đếm**; khi có đồng hồ chạy, hệ thống MUST phản hồi bằng toast. *(US-017 · EPIC-007 §11 Scope · PRD-REQ-076 · GR-035, T-089 · AC-123)*
- **FR-118**: Trong lúc banner tạm dừng đang bật, banner MUST NOT phủ màn admin — admin MUST làm được mọi thứ như thường, trừ **start timer**. *(US-017, US-001 · EPIC-007 §11 Scope · PRD-REQ-076 · STATE-040 · AC-124, AC-125)*
- **FR-119**: Banner tạm dừng MUST **không chữ**: hệ thống MUST NOT hiển thị lý do, tên người bấm hay đồng hồ trên banner. Thao tác mở và đóng banner MUST NOT đòi dialog xác nhận vì nó đảo ngược được, nhưng MUST vào nhật ký thao tác. *(US-017 · EPIC-007 §11 Scope · PRD-REQ-076 · STATE-040 §Bốn ràng buộc suy ra · AC-125)*

#### Nhóm M — Nút một chiều và khoá theo luật chơi (US-018)

- **FR-120**: Nút hành động MUST chỉ hiện trạng thái **đang xử lý** và MUST NOT bị vô hiệu hoá để chống gửi trùng; người dùng MUST gửi lại được, và server MUST nhận bản cuối cùng trước hạn. Chống trùng MUST là việc của server. *(US-018 · PRD-REQ-072 · CLAUDE.md §UX · AC-127)*
- **FR-121**: Khoá **theo luật chơi** — chuông đã bấm, Ngôi sao hy vọng đã dùng, chưa tới lượt, ghế bị loại — MUST vẫn vô hiệu hoá nút bình thường, và hệ thống MUST đánh dấu rõ đó là khoá theo luật chơi chứ không phải chống gửi trùng. *(US-018 · PRD-REQ-072 · GR-034 C5, GR-006 · AC-128, AC-130)*
- **FR-122**: Hệ thống MUST có **một thành phần dùng chung** cho các nút một chiều — chấm, start timer, chuyển câu, duyệt tín hiệu — kèm ghi chú phân loại giữa *khoá chống bấm trùng* và *khoá theo luật chơi*. *(US-018 · PRD-REQ-072 · CLAUDE.md §UX · AC-129)*
- **FR-123**: Dialog xác nhận MUST là **ngoại lệ có chủ đích** của quy tắc không chặn gửi lại: nó chống bấm nhầm một hành động không thu hồi được, MUST NOT được dùng như một cơ chế chống gửi trùng. *(US-018, US-010 · PRD-REQ-072, PRD-REQ-056 · CLAUDE.md §UX, STATE-035 · AC-127, AC-129)*

#### Nhóm N — Cảnh báo vỡ bộ Vượt chướng ngại vật (US-019)

- **FR-124**: Khi admin chỉ định một hàng ngang, một câu ô trung tâm, hay một Chướng ngại vật làm **Câu hỏi phụ**, hệ thống MUST cảnh báo rằng thao tác đó **làm vỡ một bộ** và MUST nêu rõ **bộ nào**. *(US-019 · PRD-REQ-093 · GR-031 C3c, TERM-058 · AC-131)*
- **FR-125**: Thứ tự ưu tiên rút tự động của Câu hỏi phụ MUST xếp kho Vượt chướng ngại vật **cuối cùng**; phép rút tự động MUST NOT chạm kho đó khi hai kho kia còn câu. *(US-019 · PRD-REQ-093 · GR-023 §Nguồn đề, GR-031 C3c · AC-132)*

#### Nhóm O — Ràng buộc xuyên suốt

- **FR-126**: **Mọi** thao tác của admin trong phạm vi feature này MUST vào nhật ký thao tác kèm người bấm, hành động, đối tượng, mốc thời gian. Hệ thống MUST NOT có thao tác nào miễn trừ audit, kể cả những thao tác không hiển thị lý do ra ngoài. *(US-001 → US-019 · PRD-REQ-055, PRD-REQ-057, PRD-REQ-061, PRD-REQ-113 · GR-029 C1, GR-037, STATE-040 §Bốn ràng buộc suy ra, EVENT-052 · AC-133)*
- **FR-127**: Nội dung chính của mỗi màn điều khiển MUST nằm trong **một khung nhìn** trên khung nhìn tham chiếu của dự án; bảng dài MUST ưu tiên phân trang với số dòng theo khung nhìn. *(US-004, US-013, US-015 · PRD-REQ-053, PRD-REQ-059, PRD-REQ-060 · CLAUDE.md §UX Bố cục theo viewport · AC-134)*
- **FR-128**: Mọi ô nhập văn bản của admin — lý do điều chỉnh điểm, lý do cửa ra phá huỷ — MUST được cắt khoảng trắng hai đầu ở **cả** giao diện lẫn server. *(US-007, US-008 · PRD-REQ-055, PRD-REQ-031 · CLAUDE.md §Quy ước code · AC-135)*
- **FR-129**: Mọi chuỗi giao diện MUST là tiếng Việt và mọi mốc thời gian hiển thị MUST theo UTC+7. *(US-001 → US-019 · CLAUDE.md §Quy ước code · AC-136)*
- **FR-130**: Mọi thao tác async trên màn điều khiển MUST có chỉ báo trạng thái đang xử lý, và MUST có phản hồi thành công hoặc thất bại sau mỗi hành động. *(US-018 · PRD-REQ-072 · CLAUDE.md §UX Trạng thái và phản hồi · AC-127, AC-130)*

### Key Entities

- **Mốc bấm của một câu**: Một thời điểm trên đồng hồ server do admin tạo ra bằng một cú bấm, ánh xạ từ một hành vi của MC trên sân khấu. Bốn mốc chuẩn: hiển thị câu · start timer · bắt đầu thực hành *(chỉ câu thực hành)* · phán quyết. Mốc là **tuyệt đối** — không ân hạn, không bù trễ. *(TERM-028, `GR-033`)*
- **Cửa sổ giữ bản tới muộn (`padding`)**: Khoảng **`(hạn chót, hạn chót + padding]`** mà server còn **giữ** một bản gửi phát ra kịp nhưng tới muộn do mạng. `padding` là **RuleConfig cấu hình được ở cấp contest**, mặc định **5 giây**, MUST NOT hard-code; biên là **biên đóng** — bản tới đúng mốc `hạn chót + padding` vẫn được giữ. Bản đó **vẫn là bản quá hạn** — nó chỉ được giữ để còn đi qua **phán quyết Đúng/Sai** của admin, **không** được khoan nhượng thành hợp lệ. Nút gửi phía thí sinh tắt đúng tại `hạn chót` ở mọi vòng. Ở **Khởi động** không có biên trên. **Quy tắc cửa sổ thuộc EPIC-006**; spec này chỉ dùng nó làm mốc mở nút chấm. *(`QĐ-109` · `specs/006` FR-070a, FR-070b)*
- **Phán quyết**: Một quyết định của admin về một **đối tượng được chấm** — bộ ba *(câu, thí sinh, loại phán quyết)*. Có hai hoặc ba giá trị tuỳ hình phạt của vòng: Đúng · Sai · Huỷ kết quả. *(TERM-029, `GR-026`)*
- **Cú bấm chốt câu**: Ở hai vòng chấm theo lô, đây **chính là** phán quyết cho mọi ghế chưa được chấm, và là mốc duy nhất sinh sự kiện điểm của câu đó. *(`GR-026` C4)*
- **Điều chỉnh điểm thủ công**: Một sự kiện `{ghế, lượng, lý do}` do admin tạo, độc lập với mọi phán quyết về đáp án, không thuộc vòng nào, không bị hoàn nguyên theo vòng, và không bị chống trùng. *(TERM-024, `GR-029`)*
- **Cửa ra của một vòng**: Một trong bốn đường rời khỏi một vòng đang chạy về trạng thái nghỉ, khác nhau ở chỗ điểm được **giữ** hay bị **hoàn nguyên**, và ở nhãn ghi vào biên bản. *(TERM-031, `GR-030`)*
- **Hàng đợi đang hoạt động**: Tập tín hiệu còn hiệu lực với **đích** chưa đóng, xử lý FIFO thuần theo server timestamp. Khác hẳn **lịch sử tín hiệu** — thứ không bao giờ bị xoá và là van thoát thay cho cửa sổ ân hạn. *(TERM-026, `GR-032`)*
- **Ứng viên kích hoạt tay**: Một tín hiệu trơ chưa xử lý, hoặc chính ghế vừa bị Huỷ kết quả, còn hiệu lực với đích của nó và không mang cờ *bị loại* hay *bị vô hiệu hoá*. *(`GR-032` §Kích hoạt tay, `T-099`, `T-100`)*
- **Trạng thái một ô của bàn cờ**: Đúng một trong ba giá trị **chờ · đã hỏi · mở**, đặt được cả hai chiều bởi admin, năm ô độc lập. *(TERM-053, `STATE-041` → `STATE-043`)*
- **Băng điểm Chướng ngại vật**: Giá trị hiện tại của Chướng ngại vật — **hàm** đọc theo số hàng ngang không còn ở trạng thái chờ, không phải bộ đếm cộng dồn, nên lùi được. *(TERM-053, `GR-009`)*
- **Cờ hành chính của ghế**: Trạng thái *bị vô hiệu hoá* — phạm vi **trận**, đảo ngược được, do admin đặt, và là cờ **duy nhất** không bị đặt lại khi mở một vòng mới. *(TERM-032, `STATE-025`, `INV-022`)*
- **Hạng phản hồi**: Nhóm phân loại của một thông báo từ hệ thống tới admin. Có **bốn** loại phân biệt được bằng mắt, trong đó ba tầng dialog khác nhau ở **cách thoát**: **toast invalid state** *(không nút, không ép được, tự tắt)* · **dialog Yes/No thường** *(`Esc` và click ra ngoài đều đóng và đều tương đương **No**, có phím tắt `Y`/`N`, không bắt lý do)* · **dialog điều chỉnh điểm** *(`Esc` đóng và **không lưu** thay đổi, click ra ngoài **không** đóng, bắt lý do, có phím tắt)* · **dialog hạng phá huỷ** *(không đóng bằng `Esc`, không đóng khi click ra ngoài, bắt lý do, không phím tắt)*. **Dialog cảnh báo lệch luật** dùng lại **tầng Yes/No thường**, khác dialog xác nhận chỉ ở **nội dung chữ**, và khác toast ở chỗ **ép được**. *(`PRD §9.4`, `STATE-035` → `STATE-037`)*
- **Lớp phủ do admin điều khiển**: Một lớp hiển thị chồng lên trạng thái đang chạy mà không huỷ nó — lớp công bố kết quả và banner tạm dừng. Nhiều lớp bật cùng lúc là hợp lệ. *(`STATE-033`, `STATE-040`, `INV-021`)*

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: **0** trận rơi vào trạng thái không có đường đi tiếp — mọi vòng đang chạy đều rời được về trạng thái nghỉ qua ít nhất một trong bốn cửa ra, ở **100%** số trạng thái mà một vòng có thể đang ở.
- **SC-002**: **0** sự kiện điểm được sinh mà không có một cú bấm của admin đứng ngay trước nó trong nhật ký thao tác — kiểm bằng cách đối chiếu từng sự kiện điểm của một trận đầy đủ với nhật ký.
- **SC-003**: **100%** thao tác của admin trong feature này để lại đúng một dòng nhật ký kèm người bấm, hành động, đối tượng và mốc thời gian; **0** thao tác không dấu vết, kể cả những thao tác không hiển thị lý do ra ngoài.
- **SC-004**: **100%** số lần bấm phán quyết lần thứ hai cho cùng bộ ba *(câu, thí sinh, loại phán quyết)* đều bị từ chối và sinh **0** sự kiện — xác minh cả qua giao diện lẫn qua yêu cầu gửi thẳng tới server.
- **SC-005**: **0** lần đáp án rời server tới thí sinh, khán giả hay lớp phủ trước mốc câu khép trong một trận đầy đủ có dùng cả *Huỷ kết quả* lẫn *kích hoạt tay* — xác minh bằng bắt gói tin trên ba kênh đó.
- **SC-006**: Băng điểm Chướng ngại vật tính đúng ở **100%** số chuỗi thao tác bàn cờ được thử, gồm các chuỗi có ít nhất một bước **lùi** và một bước **đặt lại đúng giá trị đang có**.
- **SC-007**: **100%** tín hiệu bị admin từ chối đều để lại ghế phát ở nguyên lượt của nó và **0** tác dụng phụ nào phát sinh — ô chữ không đổi, cờ đã-dùng của câu không đổi, đồng hồ không chạy.
- **SC-009**: **0** trường hợp điểm tụt gấp đôi hay cộng gấp đôi do một cú bấm lặp trên các nút một chiều — thử với mỗi nút một chiều, mỗi nút ít nhất 10 lần bấm liên tiếp trong 1 giây.
- **SC-010**: **100%** số lần điều chỉnh điểm với ô lý do trống hoặc chỉ gồm khoảng trắng đều bị từ chối và sinh **0** sự kiện.
- **SC-011**: Nội dung chính của **mọi** màn điều khiển nằm trọn trong một khung nhìn tham chiếu, đo bằng **0** phần tử thao tác bắt buộc nằm dưới mép dưới khi trang vừa tải.
- **SC-013**: **0** lần nút chấm hoặc nút *chốt câu* mở trước mốc `hạn chót + padding` ở vòng có cửa sổ giữ bản tới muộn — thử với `padding` cấu hình ở **0**, **5** và **30** giây; và **100%** số lần ở Khởi động nút chấm mở ngay tại `hạn chót`. Tại mốc mở, **100%** bản mà server đã giữ đều đã hiện trên màn chấm kèm dấu quá hạn. *(Hành vi giữ/từ chối của server đo ở `specs/006` SC tương ứng.)*
- **SC-014**: Bốn loại phản hồi — toast và ba tầng dialog — phân biệt được bằng mắt ở **100%** số tình huống thử; **0** lần hạng phá huỷ đóng được bằng `Esc`, click ra ngoài, hay phím tắt; **0** lần dialog điều chỉnh điểm đóng được bằng click ra ngoài; và **100%** số lần đóng dialog điều chỉnh điểm bằng `Esc` đều **không** lưu thay đổi.
- **SC-012**: **0** thời điểm nào banner tạm dừng và một đồng hồ đang đếm cùng có mặt — xác minh bằng cách thử bật banner ở mọi trạng thái có đồng hồ và thử start timer khi banner đang bật.

## Out of Scope

- **Luật số học của từng vòng và mô hình điểm**: mức điểm, hình phạt, thang xếp hạng Tăng tốc, số câu theo luật, cửa sổ chuông, cơ chế cướp quyền, Ngôi sao hy vọng, điểm là hàm của nhật ký sự kiện, hoàn nguyên bằng sự kiện đảo ngược, server time, rút đề và quy tắc không lặp câu, phạm vi hiển thị đáp án ở tầng engine, mất kết nối ở tầng engine, **cửa sổ giữ bản tới muộn** *(mốc tắt nút gửi · server giữ hay từ chối một bản · biên trên và ngoại lệ Khởi động · việc bỏ mốc cắt của `GR-006`)* — `specs/006` FR-070, FR-070a, FR-070b — thuộc EPIC-006 (`specs/006-game-engine-luat-thi-dau`). Spec này chỉ đặc tả **bề mặt bấm** và **những gì admin nhìn thấy**.
- **Vòng đời cấp trận**: cú bấm bắt đầu trận và việc đóng băng cấu hình, trạng thái nghỉ là cửa vào/ra duy nhất của mọi vòng, cú bấm **Chốt trận** và phép tìm nhóm hoà, **Huỷ trận** và nhãn *bỏ dở*, niêm phong trận đã đóng sổ, tạo trận mới trong cùng contest, đặt chỗ câu Câu hỏi phụ ở trạng thái nghỉ, hàng rào số ghế tại cú bấm bắt đầu trận và cơ chế ghế bỏ thi — thuộc EPIC-005 (`specs/005-phong-thi-vong-doi-tran`). Riêng **PRD-REQ-030** và **PRD-REQ-031** khai cả EPIC-007 nên **mặt bấm của admin** ở hai requirement đó nằm trong spec này (US-008, US-009); phần vòng đời trạng thái vẫn thuộc spec 005.
- **Giao diện và trải nghiệm máy thí sinh**: nút chuông chỉ nhận click chuột và tự khoá, vòng đời ngược của nút gửi, hai bố cục Vượt chướng ngại vật theo mode, ba trạng thái nút chọn hàng ngang, chọn gói câu và đặt Ngôi sao hy vọng ở mode nhập liệu, bảng điểm realtime trên máy thí sinh, dialog xác nhận phía thí sinh, phím tắt và hành vi `Esc`, khôi phục sau mất kết nối — thuộc EPIC-008. Riêng **PRD-REQ-072** khai cả EPIC-007 nên quy ước **nút một chiều phía admin** nằm trong spec này (US-018); mặt thí sinh của cùng requirement thuộc EPIC-008.
- **Trình diễn**: **nội dung và cách render** màn khán giả, lớp phủ 1920×1080, màn MC chữ lớn, lớp công bố kết quả *(bảng xếp hạng, quy tắc đồng hạng, hiệu ứng)*, banner tạm dừng *(biểu hiện trên máy thí sinh và viewer)*, chủ đề, các khe âm thanh, và quy tắc **không báo cho khán giả về can thiệp của admin** — thuộc EPIC-009. Spec này chỉ nhận **nút mở và nút đóng** của hai lớp phủ, cùng điều kiện bật nút — xem §Quy ước truy nguyên điểm 2.
- **Xác thực và phân quyền**: catalog permission, mô hình vai dựng sẵn và vai tuỳ biến, cơ chế một phiên giữ quyền điều khiển, chuyển quyền, **giành quyền điều khiển khi phiên admin mất kết nối** và prompt duyệt của MC — thuộc EPIC-001. Spec này chỉ phát biểu permission như một **cổng** của thao tác.
- **Kho đề và bộ đề** (EPIC-002) · **nhập/xuất gói contest** (EPIC-003) · **contest builder và luật tuỳ biến**, gồm cả việc khoá cứng `rowCount = 4` (EPIC-004) · **sau trận: bảng điểm cuối, biên bản in, thống kê ghi ngược kho đề, phát lại** (EPIC-010) · **nhật ký thao tác chung, hạn lưu trữ, mã hoá dữ liệu cá nhân** (EPIC-011). Spec này chỉ đặc tả rằng thao tác của admin **phải** vào nhật ký (FR-126), không đặc tả bảng nhật ký hay chính sách lưu trữ.
- **Mọi cơ chế tự cộng trừ điểm** (`NON-GOAL-001`) · **tính lại thứ hạng tự động sau khi đổi phán quyết** (`QĐ-014`) · **luật cho số ghế trên 4** (`NON-GOAL-012`) · **thi đội** (`NON-GOAL-013`) — không thuộc v1 dưới bất kỳ epic nào.

## Open Questions

Không có. Mọi requirement của feature này đều có nguồn chuẩn tắc trong `docs/`.

## Traceability Matrix

| User story | PRD requirement | Game rules | Functional requirements | Acceptance scenarios |
|---|---|---|---|---|
| US-001 | PRD-REQ-043, PRD-REQ-054 | `GR-033`, `GR-019` §Thứ tự đánh giá, `GR-006` §Điều kiện, `GR-003`, `GR-021`, `GR-035`, `INV-004`, `INV-015` | FR-001 → FR-010 | AC-001 → AC-012 **+ AC-011a** |
| US-002 | PRD-REQ-038, PRD-REQ-063, PRD-REQ-036 | `GR-026`, bảng §Phán quyết, bảng §Bản gửi quá hạn, `GR-004` C3, `GR-009` C6b, `GR-020` C2, `GR-021`, `INV-003`, `INV-009`, `INV-010` | FR-011 → FR-018, FR-022 → FR-024 | AC-013 → AC-016, AC-019 → AC-026 |
| US-003 | PRD-REQ-051, PRD-REQ-052 | `GR-027`, bảng §Bản gửi quá hạn, `GR-006` C4, C5, `GR-015` C5, C6, `GR-035` C6, `GR-019` §Điều kiện | FR-025 → FR-033, FR-037 | AC-027 → AC-038 **+ AC-038a**, AC-042a |
| US-004 | PRD-REQ-053 | `GR-013` §Mục đích, §Thứ tự đánh giá, `GR-015` §Thứ tự đánh giá | FR-018, FR-022, FR-034, FR-127 | AC-024, AC-026, AC-039, AC-134 |
| US-005 | PRD-REQ-054 | bảng §Kênh trả lời, `GR-019` §Điều kiện, `GR-026` §Biên, `GR-006` §Đồng thời, `GR-015` §Đồng thời, `INV-015` | FR-007, FR-017, FR-035 → FR-037 | AC-008, AC-009, AC-011a, AC-020, AC-038a, AC-040 → AC-042a |
| US-006 | PRD-REQ-062 | `GR-026` C4, C5, `GR-013` C6, `GR-008` §Thứ tự đánh giá, `GR-009` C6b, `INV-009`, `INV-010` | FR-018 → FR-022 **+ FR-035**, FR-051, FR-109 | AC-017, AC-018, AC-024, AC-026, AC-038a, AC-059, AC-112 |
| US-007 | PRD-REQ-055 | `GR-029` *(toàn bộ)*, `GR-028` C3, `GR-030` §Không đổi gì, `INV-002`, `INV-018` | FR-038 → FR-045 **+ FR-061a**, FR-128 | AC-043 → AC-052, AC-135 |
| US-008 | PRD-REQ-031, PRD-REQ-056 | `GR-030` *(toàn bộ)*, `GR-029` C4, C5, `GR-031` C2, `INV-001`, `INV-022` | FR-042, FR-046 → FR-054, FR-061 | AC-046, AC-047, AC-053 → AC-061, AC-064, AC-067 |
| US-009 | PRD-REQ-030 | `GR-030` C4, `INV-014`, `INV-020` | FR-054 → FR-057 **+ FR-063a** | AC-056, AC-062, AC-063, AC-070 |
| US-010 | PRD-REQ-056 | `GR-029` §Điều kiện, `GR-030` §Điều kiện, `GR-026`, `GR-032` C9, `PRD §9.4`, `INV-014` | FR-045, FR-058 → FR-064 **+ FR-061a, FR-063a**, FR-123 | AC-052, AC-065 → AC-072, AC-127 |
| US-011 | PRD-REQ-057 | `GR-037` *(toàn bộ, gồm C8, C8b)*, `GR-012` C2, `INV-017`, `INV-021`, `PERM-044`, `PERM-045` | **FR-009**, FR-060, FR-062, FR-065 → FR-073 | AC-012, AC-066, AC-068, AC-073 → AC-080, AC-090, AC-090a |
| US-012 | PRD-REQ-058 | `GR-009` C12, C13, `GR-011` C1, C3, C8, `GR-012` C2, C3, C5, `GR-008`, `STATE-041` → `STATE-043`, `T-084` → `T-086`, `INV-021` | FR-073 → FR-084 **+ FR-063a** | AC-081 → AC-090c |
| US-013 | PRD-REQ-059 | `GR-032` C1 → C7, `GR-009` C7, C8, `GR-007`, `GR-010` C4, `INV-001`, `INV-006`, `INV-007`, `INV-008` | FR-085 → FR-092, FR-127 | AC-091 → AC-099, AC-134 |
| US-014 | **PRD-REQ-113** | `GR-032` C8, C9, §Kích hoạt tay, `GR-009` C6b, `GR-037` C8b, `GR-003`, `GR-020`, `GR-023`, `EVENT-052`, `T-097` → `T-100`, `INV-006`, `INV-008`, `INV-009`, `INV-014`, `INV-020` | FR-012, FR-072, FR-093 → FR-100 **+ FR-063a** | AC-023, AC-079, AC-100 → AC-106b |
| US-015 | PRD-REQ-060 | `GR-031` C5, C6, C7, C8, §Ranh giới "đã dùng", `INV-011` | FR-052, FR-101 → FR-104 | AC-061, AC-107 → AC-109b |
| US-016 | PRD-REQ-061 | `GR-036` C2, C3, C5, C6, C7, C9, `STATE-023` → `STATE-025`, `EVENT-029`, `EVENT-030`, `INV-016` | FR-105 → FR-113 | AC-110 → AC-119 |
| US-017 | EPIC-007 §11 Scope · PRD-REQ-075, PRD-REQ-076 *(mệnh đề về admin)* | `GR-035`, `GR-028`, `STATE-033`, `STATE-040`, `EVENT-032` → `EVENT-035`, `T-087` → `T-090`, `INV-016`, `INV-021` | FR-010, FR-114 → FR-119 | AC-010, AC-120 → AC-126 |
| US-018 | PRD-REQ-072 | `GR-034` C5, `GR-006`, `GR-029` §Bấm trùng, `GR-033` C2, `CLAUDE.md` §UX | FR-084, FR-089, FR-120 → FR-123, FR-130 | AC-090b, AC-099, AC-127 → AC-130 |
| US-019 | PRD-REQ-093 | `GR-031` C3b, C3c, `GR-023` §Nguồn đề, `TERM-058` | FR-124, FR-125 | AC-131, AC-132 |
| *(xuyên suốt)* | PRD-REQ-055, 057, 061, 072, 113 · `CLAUDE.md` §UX, §Quy ước code | — | FR-126 → FR-130 | AC-133 → AC-136 |

### Bản đồ phủ bảng quyết định

> Mỗi outcome trong bảng quyết định của từng `GR-NNN` **thuộc phạm vi feature này** đều có ít nhất một acceptance scenario. Ca nào thuộc epic khác thì ghi rõ spec sở hữu, theo đúng cách `specs/006` đã dùng — chúng **không** được đặc tả lại ở đây.

**Chín rule mà EPIC-007 khai trực tiếp (`GR-026` → `GR-034`)**

| Game rule | Ca trong bảng quyết định | Acceptance scenarios |
|---|---|---|
| `GR-026` | C1 · C2 · C3 · C4 · C5 | AC-013 · AC-014 · AC-016 · AC-017 · AC-018 |
| `GR-027` | C1 · C2 · C3 · C4 · C5 | AC-028 · AC-029 · AC-030 · AC-031 · AC-032 |
| `GR-028` | C1 · C2 · C3 · C4 · C5 | AC-013 · AC-014 · AC-043 · AC-053 · *(EPIC-006 — `specs/006` AC-019)* |
| `GR-029` | C1 · C2 · C3 · C4 · C5 · C6 | AC-043 · AC-044 · AC-045 · AC-046 · AC-047 · AC-048 |
| `GR-030` | C1 · C2 · C3 · C4 · C5 | AC-053 · AC-054 · AC-055 · AC-056 · AC-057 |
| `GR-031` | C1 · C2 · C3 · C3b · C3c · C4 · C5 · C6 · C7 · C8 · C9 | *(EPIC-006)* · *(EPIC-006)* · AC-060 · *(EPIC-006 — kiểm bộ nguyên vẹn)* · AC-131 · *(EPIC-006)* · AC-109b · AC-109 · AC-108 · AC-109a · *(EPIC-006 — phép lọc đảo chiều)* |
| `GR-032` | C1 · C2 · C3 · C4 · C5 · C6 · C7 · **C8** · **C9** | AC-091 · AC-092 · AC-093 · AC-094 · AC-095 · AC-096 · AC-097 · AC-100 · AC-101 |
| `GR-033` | C1 · C2 · C3 · C4 · C5 · C6 | AC-002 · AC-003 · AC-001 · AC-004 · AC-005 · AC-006 |
| `GR-034` | C1 · C2 · C3 · C4 · C5 · C6 | *(EPIC-008)* · *(EPIC-008)* · *(EPIC-008)* · *(EPIC-008)* · AC-130 · *(EPIC-008)* |

**Các rule mà PRD-REQ của EPIC-007 tham chiếu — chỉ phủ phần thuộc epic này**

| Game rule | Ca thuộc phạm vi EPIC-007 | Acceptance scenarios |
|---|---|---|
| `GR-006` | C2, C3 *(bản được ghi nhận = bản hợp lệ cuối cùng trước `hạn chót`)* · C4 · C5 · §Đồng thời *(nút chấm khoá tới khi cửa sổ giữ bản tới muộn đóng)* | AC-011 · AC-011a, AC-035 · AC-011a, AC-036, AC-022 · AC-041 |
| `GR-009` | C6b *(ba lựa chọn + kích hoạt tay)* · C7 *(băng chốt tại mốc xác nhận)* · C8 *(bấm No)* · C12 *(đặt tay đã hỏi)* · C13 *(đặt tay mở)* | AC-023, AC-104 · AC-092 · AC-093 · AC-082 · AC-083 |
| `GR-010` | C4 *(ghế bị loại không tạo tín hiệu)* | AC-096 |
| `GR-011` | C1 *(gợi ý cuối hạ băng)* · C3 *(sai vẫn giữ băng 20)* · C8 *(mở ô trung tâm hai lần)* | AC-089 · AC-089 · AC-090c |
| `GR-012` | C2 *(admin bấm công bố)* · C3 *(không bấm vẫn kết thúc)* · C5 *(bấm hai lần)* | AC-090 · AC-090a · AC-090 |
| `GR-013` | C6 *(chấm Sai không giữ chỗ)* · C7 *(vòng đã bỏ ⇒ toast)* · §Bấm trùng | AC-017 · AC-025 · AC-024, AC-026 |
| `GR-015` | C5 *(bản hợp lệ + bản quá hạn)* · C6 *(chỉ bản quá hạn)* | AC-035, AC-038a · AC-036, AC-038, AC-038a |
| `GR-019` | §Điều kiện *(mốc thứ ba, nút chấm sống suốt, không có gì để tô)* | AC-008, AC-009, AC-042, AC-042a |
| `GR-020` | C2 *(admin chọn Huỷ kết quả thay vì Sai cho người cướp)* | AC-014, AC-015 |
| `GR-023` | §Nguồn đề *(thứ tự ưu tiên ba kho)* | AC-132 |
| `GR-035` | C6 *(bản quá hạn giữ lại, tô đỏ, không chặn cứng; ở vòng có `padding` cửa sổ giữ có **biên trên** `hạn chót + padding`, Khởi động không có biên trên)* | AC-035, AC-037, AC-038a, AC-011a |
| `GR-036` | C2 · C3 · C5 · C6 · C7 · C9 | AC-113 · AC-114 · AC-110 · AC-111 · AC-112 · AC-115 |
| `GR-037` | C1 · C2 · C3 · C4 · C5 · C6 · C7 · C8 · C8b · C9 | AC-080 · AC-076 · AC-073 · AC-073 · AC-077 · *(EPIC-006 — `specs/006` AC-069)* · *(EPIC-006)* · AC-078 · AC-079 · AC-090 |

**Bảng dùng chung và transition**

| Nguồn | Nội dung | Acceptance scenarios |
|---|---|---|
| bảng §*Phán quyết có hai hay ba lựa chọn* | sáu dòng vòng · pha, gồm dòng cuối *(tín hiệu Chướng ngại vật)* | AC-013 · AC-014 · AC-021 · AC-023 |
| bảng §*Bản gửi quá hạn* | C1 *(có cả hai)* · C2 *(chỉ bản quá hạn)* | AC-035 · AC-036, AC-022 |
| bảng §*Kênh trả lời* | kênh nói · kênh gõ · kênh thực hành | AC-040 · AC-041 · AC-042 |
| `T-084` · `T-085` · `T-086` | đặt ô · no-op · công bố toàn bàn | AC-082, AC-084 · AC-085 · AC-090 |
| `T-087` → `T-090` | mở/đóng công bố · mở/đóng banner | AC-120, AC-122 · AC-123, AC-124 |
| `T-097` → `T-100` | cấp lại cửa sổ · hạn mức · tín hiệu trơ thành hiệu lực · cấp lại quyền cho chính ghế bị huỷ | AC-102 · AC-103, AC-104 · AC-100 · AC-105 |
| `EVENT-004` | bốn side effect của kết thúc khẩn cấp | AC-055, AC-059, AC-061 |
| `EVENT-021` | ba ranh giới của đặt trạng thái ô chữ | AC-086 |
| **`QĐ-108`** | ba tầng dialog phân biệt bằng cách thoát × trục *bắt lý do* độc lập; phím tắt `Y`/`N` trừ hạng phá huỷ | AC-052 · AC-066 · AC-067 · AC-069 |
| **`QĐ-072`** | cảnh báo lệch luật dùng lại tầng Yes/No thường | AC-070 |
| **`QĐ-109`** | **mốc mở nút chấm** = `hạn chót + padding` *(quy tắc cửa sổ giữ bản tới muộn thuộc `specs/006`)* | AC-038a · AC-011a |
| **`QĐ-110`** | bỏ mốc bấm *công bố đáp án* khỏi vòng đời câu — một câu còn tối đa bốn mốc | AC-011 · AC-012 |
| §*Invalid transitions* | các dòng thuộc thao tác của admin trong epic này | AC-007, AC-010, AC-025, AC-041, AC-048, AC-056, AC-057, AC-065, AC-087, AC-088, AC-103, AC-109, AC-109a, AC-116, AC-123, AC-124 |

### Acceptance Scenarios (chi tiết)

> Mọi kịch bản đều giả định **một** phiên admin đang giữ quyền điều khiển — `GR-026` §Đồng thời khai *"mỗi contest chỉ có MỘT admin, nên không có hai luồng thao tác admin song song"*. Vì vậy phần *concurrency outcome* của các kịch bản dưới chỉ nói về **tín hiệu của thí sinh**, không nói về hai admin.

#### Nhóm A — Bốn mốc bấm của một câu (US-001)

**AC-001** — *US*: US-001 · *FR*: FR-001, FR-002, FR-003 · *GR*: `GR-033` C3
**Given** một trận đang ở vòng Khởi động lượt chung, câu tiếp theo đã được rút nhưng **chưa hiển thị**, đồng hồ chưa chạy, một ghế còn quyền đặt Ngôi sao hy vọng, và câu đó chưa mang cờ đã-dùng,
**When** admin bấm **hiển thị câu hỏi**,
**Then** câu hiện trên máy thí sinh và màn khán giả; cửa sổ chuông **mở**; cửa sổ đặt Ngôi sao hy vọng **đóng** và nút đó không còn render trên máy thí sinh; câu được đánh dấu **đã dùng**; đồng hồ **vẫn chưa chạy**; điểm của mọi ghế không đổi; và một dòng nhật ký thao tác được ghi.

**AC-002** — *US*: US-001 · *FR*: FR-001, FR-002 · *GR*: `GR-033` C1
**Given** cùng câu như AC-001 sau khi đã hiển thị, đồng hồ chưa chạy, `timeSeconds` của câu là một giá trị đã cấu hình,
**When** admin bấm **start timer**,
**Then** đồng hồ bắt đầu chạy từ `timeSeconds` của câu; mốc được ghi bằng server timestamp; cửa sổ chuông đã mở từ mốc hiển thị **vẫn đang mở** và không bị đặt lại; điểm không đổi.

**AC-003** — *US*: US-001 · *FR*: FR-005 · *GR*: `GR-033` C2, §Bấm trùng
**Given** admin vừa bấm start timer cho một câu và đồng hồ đang chạy,
**When** admin bấm **start timer** lần thứ hai,
**Then** nút đã ở trạng thái khoá nên không có thao tác nào được phát; hạn chót của câu **không** bị đẩy lùi; không sự kiện nào được sinh; và nếu một yêu cầu vẫn lọt tới thì server bỏ qua.

**AC-004** — *US*: US-001 · *FR*: FR-004 · *GR*: `GR-033` C4
**Given** một câu vừa được hiển thị và admin chưa bấm start timer,
**When** một khoảng thời gian bất kỳ trôi qua trong lúc MC đọc câu,
**Then** câu vẫn hiện trên các màn, đồng hồ **chưa chạy**, hệ thống không coi đây là trạng thái lỗi và không hiện cảnh báo nào; hạn chót của câu chưa tồn tại.

**AC-005** — *US*: US-001 · *FR*: FR-008 · *GR*: `GR-033` C5
**Given** một trận đang ở vòng Câu hỏi phụ, câu đã hiển thị, admin chưa bấm start timer, và nút chuông trên máy mọi thí sinh **chưa sống**,
**When** admin bấm **start timer** — mốc *hiệu lệnh*,
**Then** từ mốc đó nút chuông sống và cửa sổ **15 giây** bắt đầu tính; tín hiệu chuông tới **trước** mốc này không tồn tại vì máy thí sinh không render nút.

**AC-006** — *US*: US-001 · *FR*: FR-006 · *GR*: `GR-033` C6
**Given** một câu đã hiển thị và mạng giữa máy admin với server đang trễ,
**When** admin bấm **start timer**,
**Then** mốc được ghi bằng **server timestamp tại lúc server nhận**; hệ thống không cộng bù độ trễ, không mở cửa sổ ân hạn, và không cho phép sửa mốc đã ghi về sau.

**AC-007** — *US*: US-001 · *FR*: FR-002 · *GR*: `GR-033` §Điều kiện · §Invalid transitions
**Given** một câu đã được rút nhưng **chưa hiển thị**,
**When** admin tìm cách bấm **start timer**,
**Then** nút start timer chưa bật nên không thao tác nào được phát; câu vẫn ở trạng thái chưa hiển thị; cờ đã-dùng chưa được đặt; và không sự kiện nào được sinh.

**AC-008** — *US*: US-001, US-005 · *FR*: FR-007 · *GR*: `GR-019` §Thứ tự đánh giá
**Given** một câu Về đích được khai là **câu thực hành**, đã hiển thị và đã start timer, pha suy nghĩ đang chạy,
**When** admin bấm **bắt đầu thực hành**,
**Then** pha suy nghĩ đóng, pha thực hành mở và đếm theo `practiceSeconds` của câu; nút phán quyết vẫn bấm được vì đây là kênh thực hành; điểm không đổi.

**AC-009** — *US*: US-001, US-005 · *FR*: FR-007 · *GR*: `GR-019` §Điều kiện
**Given** một câu **không** được khai là câu thực hành, đã hiển thị và đã start timer,
**When** admin xem màn điều khiển của câu đó,
**Then** nút *bắt đầu thực hành* **không tồn tại** trên màn; vòng đời câu chỉ có ba mốc còn lại.

**AC-010** — *US*: US-001, US-017 · *FR*: FR-010, FR-118 · *GR*: `GR-035` · `T-089`
**Given** banner tạm dừng **đang bật**, một câu đã hiển thị và đồng hồ chưa chạy,
**When** admin bấm **start timer**,
**Then** nút không bật và hệ thống hiện **toast** invalid state; đồng hồ vẫn chưa chạy; admin **vẫn** làm được mọi thao tác khác trên màn của mình vì banner không phủ máy admin.

**AC-011** — *US*: US-001 · *FR*: FR-009 · *GR*: `GR-006` C2, C3
**Given** một câu Khởi động ở **mode nhập liệu** với `hạn chót` ở `13,000` giây; ghế A gửi `"Hà Nội"` lúc `11,000` giây, sửa thành `"Huế"` lúc `12,500` giây, và gửi lại `"Huế"` lúc **đúng** `13,000` giây,
**When** đồng hồ chạm `hạn chót`,
**Then** bản được ghi nhận cho ghế A là `"Huế"`; bản tới **đúng** `hạn chót` vẫn hợp lệ vì biên là biên **đóng**; cả ba bản còn nguyên trong lịch sử; và **không** tồn tại một mốc bấm *"công bố đáp án"* riêng nào trong vòng đời câu này.

**AC-011a** — *US*: US-001, US-005 · *FR*: FR-035 · *GR*: `GR-006` C4, C5, §Đồng thời · `INV-009` *(hành vi cửa sổ: `specs/006` AC-099a)*
**Given** một câu Khởi động mode nhập liệu với `hạn chót` ở `13,000` giây; ghế B phát một bản lúc `12,990` giây mà mạng trễ nên tới server lúc `13,040` giây; ghế E phát một bản lúc `12,995` giây trên một đường truyền rất tệ nên tới server lúc `25,000` giây,
**When** đồng hồ chạm `hạn chót`, admin chấm ghế B, rồi bản của ghế E mới tới,
**Then** nút chấm trên màn điều khiển mở **ngay từ `13,000` giây** chứ không chờ thêm — Khởi động không có biên trên nên không có gì để chờ; màn chấm hiện **cả hai** bản quá hạn kèm dấu đỏ, và bản của ghế chỉ có bản quá hạn hiện thêm lựa chọn **Huỷ kết quả**; bản của ghế E tới sau khi admin đã chấm thì vào lịch sử nhưng **không** lật được phán quyết đã chốt.

**AC-012** — *US*: US-001, US-011 · *FR*: FR-009, FR-065 · *GR*: `GR-006` §Mục đích · `GR-037`
**Given** hai câu Khởi động song song — một ở **mode nhập liệu**, một ở **mode sân khấu**,
**When** admin xem màn điều khiển của từng câu,
**Then** ở **cả hai** màn không tồn tại nút *công bố đáp án* như một mốc riêng của vòng đời câu; việc đưa đáp án ra màn hình đi qua đúng **một** đường là nút **mở đáp án** chung, thứ hoạt động y hệt nhau ở cả hai mode và đi qua dialog Yes/No.

#### Nhóm B — Màn phán quyết và cú bấm chốt câu (US-002, US-006)

**AC-013** — *US*: US-002 · *FR*: FR-011 · *GR*: `GR-026` C1, bảng §Phán quyết dòng 1
**Given** một câu Khởi động **lượt riêng** giá trị 10 điểm, đã hết giờ chưa chấm, ghế A đang có 0 điểm,
**When** admin bấm **Đúng** cho ghế A,
**Then** màn chấm chỉ hiện **hai** nút *Đúng / Sai*; hệ thống sinh đúng một sự kiện điểm `+10`; điểm ghế A thành **10**; câu chuyển sang trạng thái đã chấm; điểm mọi ghế khác không đổi.

**AC-014** — *US*: US-002 · *FR*: FR-011 · *GR*: `GR-026` C2, `GR-004` C2, bảng §Phán quyết dòng 3
**Given** một câu Khởi động **lượt chung** mà ghế B đã giành quyền, đã hết cửa sổ trả lời, ghế B đang có 30 điểm,
**When** admin bấm **Sai** cho ghế B,
**Then** màn chấm hiện **ba** nút *Đúng / Sai / Huỷ kết quả*; hệ thống sinh một sự kiện điểm `−5`; điểm ghế B thành **25**; điểm mọi ghế khác không đổi — lượt chung không có cơ chế chuyển điểm giữa người với người.

**AC-015** — *US*: US-002 · *FR*: FR-014 · *GR*: `GR-004` C3, `GR-020` C2
**Given** cùng câu như AC-014, ghế B đang có 30 điểm và đã giành quyền,
**When** admin bấm **Huỷ kết quả** cho ghế B,
**Then** hệ thống sinh một sự kiện phán quyết giá trị **0**; điểm ghế B **vẫn 30** và hình phạt `−5` **không** được áp; câu chuyển sang trạng thái đã chấm; điểm mọi ghế khác không đổi.

**AC-016** — *US*: US-002 · *FR*: FR-015 · *GR*: `GR-026` C3
**Given** ghế A đã gửi một bài làm **khớp tuyệt đối** một phần tử trong danh sách đáp án, đồng hồ câu đã hết giờ, và admin chưa bấm nút phán quyết nào,
**When** một khoảng thời gian bất kỳ trôi qua,
**Then** không sự kiện điểm nào được sinh; điểm mọi ghế không đổi; câu treo ở trạng thái chưa chấm; nút chuyển câu **không hiện**; và kết quả đối chiếu chỉ tồn tại dưới dạng gợi ý hiển thị cho admin.

**AC-017** — *US*: US-002, US-006 · *FR*: FR-019, FR-020 · *GR*: `GR-026` C4, `GR-013` C6
**Given** một câu Tăng tốc có bốn ghế, admin đã đánh dấu Đúng cho hai ghế và chưa đánh dấu hai ghế còn lại, đồng hồ đã hết,
**When** admin bấm **chốt câu**,
**Then** dialog xác nhận liệt kê **tên** hai ghế chưa chấm; sau khi xác nhận, hai ghế đó tính là **Sai**, nhận 0 và **không giữ chỗ** trong thang xếp hạng; hệ thống sinh đúng **một** sự kiện điểm cho toàn bảng; và không bộ đếm nào đã tự chốt trước cú bấm này.

**AC-018** — *US*: US-006 · *FR*: FR-021 · *GR*: `GR-026` C5
**Given** một câu hàng ngang Vượt chướng ngại vật đang mở, ba ghế đã được chấm, và một tín hiệu *"Mở chướng ngại vật"* của ghế D **đang chờ duyệt** trong hàng đợi chặn,
**When** admin bấm **chốt câu** cho câu hàng ngang đó,
**Then** các ghế chưa chấm của **câu hàng ngang** tính là Sai, nhận 0 và **không bị loại**; tín hiệu *"Mở chướng ngại vật"* của ghế D **không** bị chấm Sai, vẫn nguyên ở trạng thái chờ duyệt; ghế D **không** bị đặt cờ bị loại.

**AC-019** — *US*: US-002 · *FR*: FR-016 · *GR*: `GR-026` §Bấm trùng · `INV-009`
**Given** admin vừa bấm **Đúng** cho ghế A ở một câu Khởi động lượt riêng và điểm đã cộng,
**When** admin bấm **Đúng** cho ghế A ở chính câu đó lần thứ hai, và một yêu cầu trùng bộ ba cũng được gửi thẳng tới server,
**Then** nút đã khoá nên giao diện không phát thao tác; server từ chối yêu cầu trùng; điểm ghế A **không** đổi lần thứ hai; và không sự kiện nào được sinh cho lần bấm đó.

**AC-020** — *US*: US-002, US-005 · *FR*: FR-017 · *GR*: `GR-026` §Biên · `INV-015`
**Given** một câu ở mode nhập liệu đã **hết giờ** từ 30 giây trước, ghế A có một bản gửi hợp lệ chưa được chấm,
**When** admin bấm **Đúng** cho ghế A,
**Then** phán quyết được chấp nhận bình thường — đồng hồ khoá thí sinh chứ không khoá admin; điểm cộng đúng; câu chuyển sang trạng thái đã chấm.

**AC-021** — *US*: US-002 · *FR*: FR-011, FR-024 · *GR*: bảng §Phán quyết · `PRD-REQ-063`
**Given** admin vừa chấm xong câu cuối của Khởi động **lượt riêng** — nơi màn chấm có hai nút,
**When** admin chuyển sang một câu Khởi động **lượt chung**,
**Then** màn chấm hiện **ba** nút mà không đòi thao tác cấu hình trung gian nào; số nút đổi ngay tại lần render đầu của câu mới.

**AC-022** — *US*: US-002, US-003 · *FR*: FR-013 · *GR*: bảng §Bản gửi quá hạn C2, `GR-006` C5
**Given** một câu Khởi động **lượt riêng** — vòng mà *Sai* trừ **0** điểm — mà ghế A **chỉ** gửi một bản **sau** hạn chót,
**When** admin mở màn chấm của câu đó,
**Then** màn chấm hiện **ba** nút *Đúng / Sai / Huỷ kết quả* dù vòng này bình thường chỉ có hai; bản quá hạn được tô đỏ và không bị máy loại.

**AC-023** — *US*: US-002, US-014 · *FR*: FR-012 · *GR*: `GR-009` C6b, bảng §Phán quyết dòng cuối
**Given** một tín hiệu *"Mở chướng ngại vật"* của ghế C **đã được admin xác nhận** trong vòng Vượt chướng ngại vật, ghế C chưa mang cờ bị loại,
**When** admin mở màn chấm của tín hiệu đó,
**Then** màn chấm hiện **ba** nút *Đúng / Sai / Huỷ kết quả* dù chấm Sai ở đây không trừ điểm; và chọn **Huỷ kết quả** khép tín hiệu mà ghế C **không** bị loại và không ai được điểm.

**AC-024** — *US*: US-002, US-004, US-006 · *FR*: FR-018 · *GR*: `GR-013` §Bấm trùng, `GR-008` §Thứ tự đánh giá
**Given** một câu Tăng tốc bốn ghế đã hết giờ, admin đã đánh dấu **Đúng** cho ghế A và **Sai** cho ghế B, chưa bấm *chốt câu*,
**When** admin đổi dấu ghế A thành Sai, đổi lại thành Đúng, rồi đổi dấu ghế B thành Đúng,
**Then** mọi lần đổi đều được chấp nhận; **không** sự kiện điểm nào được sinh trong suốt giai đoạn đó; điểm hiển thị của cả bốn ghế không đổi.

**AC-025** — *US*: US-002 · *FR*: FR-023 · *GR*: `GR-013` C7, `GR-030`
**Given** vòng Tăng tốc **đã bị bỏ** và điểm của nó đã được hoàn nguyên; một câu thuộc vòng đó còn ở trạng thái chưa chấm trong nhật ký,
**When** admin tìm cách bấm **Đúng** cho một ghế ở câu đó,
**Then** hệ thống hiện **toast** invalid state và không cho ép qua; không sự kiện điểm nào được sinh; bảng điểm không đổi.

**AC-026** — *US*: US-002, US-004, US-006 · *FR*: FR-018, FR-022 · *GR*: `GR-013` §Bấm trùng · `INV-009`
**Given** cùng câu Tăng tốc như AC-024, và admin **đã bấm** *chốt câu* nên bảng điểm của câu đã sinh,
**When** admin tìm cách đổi dấu của ghế B một lần nữa,
**Then** thao tác đó không tồn tại trên giao diện và server từ chối nếu yêu cầu lọt tới; bảng điểm của câu **không** được tính lại; đường sửa duy nhất còn lại là điều chỉnh điểm thủ công.

#### Nhóm C — Màn chấm, đối chiếu và bản quá hạn (US-003, US-004, US-005)

**AC-027** — *US*: US-003 · *FR*: FR-025, FR-029 · *GR*: `GR-027` §Thứ tự đánh giá · `PRD-REQ-051`
**Given** một câu có danh sách ba đáp án được chấp nhận và ghế A gửi một bài làm khớp tuyệt đối **đáp án thứ ba**,
**When** admin mở màn chấm của ghế A,
**Then** bài làm hiện cạnh đáp án và **không ký tự nào bị tô** — hệ thống so với đáp án **gần nhất**, không phải đáp án đầu danh sách; cả ba đáp án đều xem được; số chữ của đáp án hiện như một gợi ý phụ.

**AC-028** — *US*: US-003 · *FR*: FR-026 · *GR*: `GR-027` C1
**Given** đáp án chuẩn là `"Huế"` và ghế A gửi `"  HUẾ  "`,
**When** admin mở màn chấm,
**Then** sau chuẩn hoá không có khác biệt nào và hệ thống hiển thị trạng thái *"khớp đúng chính tả"*; **không** điểm nào được sinh từ kết quả này.

**AC-029** — *US*: US-003 · *FR*: FR-026 · *GR*: `GR-027` C2
**Given** đáp án chuẩn là `"Huế"`, ghế A gửi `"Hué"`, và tuỳ chọn bỏ dấu đang **TẮT**,
**When** admin mở màn chấm,
**Then** ký tự khác ở vị trí dấu được tô nổi bật; phán quyết vẫn để trống chờ admin.

**AC-030** — *US*: US-003 · *FR*: FR-026 · *GR*: `GR-027` C3
**Given** đáp án chuẩn là `"Huế"`, ghế A gửi `"Hue"`, và tuỳ chọn bỏ dấu đang **BẬT**,
**When** admin mở màn chấm,
**Then** **không** ký tự nào bị tô vì dấu bị bỏ qua khi so; bài làm gốc `"Hue"` vẫn hiển thị nguyên văn; và admin vẫn chấm Sai được nếu muốn — cách tô không ràng buộc phán quyết.

**AC-031** — *US*: US-003 · *FR*: FR-025, FR-026 · *GR*: `GR-027` C4
**Given** đáp án chuẩn là `"Huế"` và ghế A gửi `"Thành phố Huế"`,
**When** admin mở màn chấm,
**Then** hai từ thừa được tô nổi bật; phần khớp không bị tô; phán quyết vẫn để trống.

**AC-032** — *US*: US-003 · *FR*: FR-028 · *GR*: `GR-027` C5
**Given** ghế A **không gửi gì**, hoặc chỉ gửi một chuỗi rỗng sau khi cắt khoảng trắng,
**When** admin mở màn chấm của ghế A,
**Then** hệ thống **không so và không tô**; trạng thái hiển thị là *"không có đáp án"*; nút phán quyết vẫn bấm được theo quy tắc kênh trả lời.

**AC-033** — *US*: US-003 · *FR*: FR-027 · *GR*: `GR-027` §Điều kiện, §Không đổi gì
**Given** ghế A gửi `"  hUế  "` và hệ thống đã chuẩn hoá để hiển thị,
**When** admin xem lại bài làm đó và mở màn sửa câu hỏi trong lúc trận đang chạy,
**Then** bài làm **gốc** `"  hUế  "` còn nguyên trong lịch sử; kết quả chuẩn hoá chỉ là dữ liệu hiển thị; và danh sách đáp án được chấp nhận **không** sửa được giữa trận.

**AC-034** — *US*: US-003 · *FR*: FR-030 · *GR*: `GR-006` §Không đổi gì, `GR-015` §Không đổi gì · `INV-001`
**Given** ghế A đã gửi ba bản khác nhau trong cửa sổ thời gian của một câu,
**When** admin mở lịch sử bài gửi của ghế A ở câu đó,
**Then** cả ba bản hiện đủ kèm server timestamp của từng bản; bản được ghi nhận được đánh dấu rõ; và không bản nào bị xoá khỏi lịch sử.

**AC-035** — *US*: US-003 · *FR*: FR-031 · *GR*: bảng §Bản gửi quá hạn C1, `GR-006` C4, `GR-015` C5, `GR-035` C6
**Given** một câu có hạn chót ở `13,000` giây; ghế A gửi `"Huế"` lúc `12,998` giây và gửi thêm `"Hà Nội"` lúc `13,002` giây,
**When** admin mở màn chấm của ghế A,
**Then** **cả hai** bản hiện cạnh nhau; bản `"Hà Nội"` được tô **đỏ** là quá hạn; bản `"Huế"` **không** bị thay; nút chấm bật được cho cả hai; và hệ thống không tự loại bản nào.

**AC-036** — *US*: US-003 · *FR*: FR-032, FR-013 · *GR*: bảng §Bản gửi quá hạn C2, `GR-015` C6
**Given** cùng câu như AC-035 nhưng ghế B **chỉ** gửi một bản, lúc `13,050` giây,
**When** admin mở màn chấm của ghế B,
**Then** bản đó được giữ và tô **đỏ**; màn chấm hiện thêm lựa chọn **Huỷ kết quả**; và hệ thống không tự loại bản đó.

**AC-037** — *US*: US-003 · *FR*: FR-031, FR-033 · *GR*: `GR-035` C6 · `INV-013`
**Given** cùng trạng thái như AC-035, và admin chưa bấm gì,
**When** một khoảng thời gian bất kỳ trôi qua,
**Then** bản quá hạn **không tự lộ** ra bất kỳ màn nào ngoài màn admin; **không** bị máy loại; và **không** sinh sự kiện nào — cả việc hiển thị lẫn việc chấm đều chờ một cú bấm của admin.

**AC-038** — *US*: US-003 · *FR*: FR-033 · *GR*: `GR-015` C6 · `INV-009`
**Given** một câu Tăng tốc, ghế A có bản hợp lệ lúc `100 ms` và ghế B **chỉ** có một bản quá hạn nằm trong cửa sổ padding; cửa sổ giữ bản tới muộn đã đóng hẳn và admin **chưa** bấm *chốt câu*,
**When** admin đánh dấu Đúng cho cả A và B rồi bấm *chốt câu*,
**Then** bảng xếp hạng của câu tính trên **cả hai** người được chấm Đúng theo mốc bản cuối của mỗi người, và sinh đúng một sự kiện điểm cho toàn bảng tại cú bấm đó; sau mốc chốt câu **không** tồn tại cơ chế tính lại bảng nào.

**AC-038a** — *US*: US-003, US-005, US-006 · *FR*: FR-033, FR-035 · *GR*: `GR-035` C6, `GR-015` C5, C6 · `INV-009` *(hành vi cửa sổ: `specs/006` AC-099a)*
**Given** một câu Tăng tốc có `hạn chót` ở `13,000` giây và `padding` cấu hình ở **5 giây**, tức cửa sổ giữ bản tới muộn đóng ở `18,000` giây; ghế B phát một bản lúc `12,990` giây mà mạng trễ nên tới server lúc `15,000` giây; ghế D phát một bản mà tới server lúc `18,001` giây,
**When** ghế C tìm cách bấm gửi lúc `13,500` giây, và admin tìm cách bấm *chốt câu* lúc `14,000` giây rồi thử lại lúc `18,500` giây,
**Then** nút *chốt câu* trên màn điều khiển **chưa mở** ở `14,000` giây và **mở** ở `18,500` giây; tại mốc mở, màn chấm đã hiện đủ bản của ghế B kèm dấu đỏ và **không** hiện bản của ghế D *(server đã từ chối — `specs/006` FR-070a)*; và vì vậy **không** tồn tại đường nào để một bản chấm được tới sau cú bấm *chốt câu*.

**AC-039** — *US*: US-004 · *FR*: FR-034 · *GR*: `GR-013` §Mục đích
**Given** một câu Tăng tốc bốn ghế, cả bốn đã gửi bài và đồng hồ đã hết,
**When** admin mở màn chấm của câu đó,
**Then** bài làm, server timestamp và bộ nút của **cả bốn** ghế cùng nằm trong một khung nhìn; không thao tác chấm nào yêu cầu chuyển màn; và cú bấm *chốt câu* nằm cùng màn đó.

**AC-040** — *US*: US-005 · *FR*: FR-035 · *GR*: bảng §Kênh trả lời dòng *nói*
**Given** một contest ở **mode sân khấu**, một câu Khởi động vừa được hiển thị, đồng hồ đang chạy, và máy thí sinh không có ô nhập nào,
**When** admin mở màn chấm,
**Then** nút phán quyết **bấm được ngay**, không chờ hết giờ; không có bài làm dạng chữ nào để tô.

**AC-041** — *US*: US-005 · *FR*: FR-035, FR-036 · *GR*: bảng §Kênh trả lời dòng *gõ*, `GR-006` §Đồng thời
**Given** một câu Tăng tốc — vòng **luôn gõ máy** bất kể mode contest — đồng hồ **đang chạy** và ghế A đã gửi một bản,
**When** admin tìm cách bấm phán quyết cho ghế A ở ba thời điểm: trong lúc đồng hồ chạy, ngay sau `hạn chót` nhưng cửa sổ giữ bản tới muộn chưa đóng, và sau `hạn chót + padding`,
**Then** hai lần đầu nút chấm mờ và không phát được thao tác, giao diện nêu rõ **lý do** nút mờ là *đang chờ cửa sổ giữ bản tới muộn đóng*; lần thứ ba nút **bật**; và không sự kiện nào được sinh ở hai lần đầu.

**AC-042** — *US*: US-005 · *FR*: FR-035 · *GR*: `GR-019` §Điều kiện
**Given** một **câu thực hành** Về đích đang ở pha thực hành, đồng hồ pha thực hành còn chạy,
**When** admin bấm phán quyết *đạt* cho người thi chính,
**Then** phán quyết được chấp nhận ngay — kênh thực hành không có ô nhập nào để chờ nên nút chấm sống suốt; điểm cộng theo giá trị câu.

**AC-042a** — *US*: US-003, US-005 · *FR*: FR-037 · *GR*: `GR-027` §Ở mode sân khấu, `GR-019` §Điều kiện
**Given** một câu Khởi động ở **mode sân khấu**, và song song một câu thực hành Về đích,
**When** admin mở màn chấm của từng câu,
**Then** ở cả hai màn **không có gì để tô** vì không có bài làm dạng chữ; các quy tắc ghi nhận bản cuối, mốc thời gian và tô nổi bật vẫn tồn tại trong hệ thống nhưng không được kích hoạt; và nút phán quyết bấm được bình thường ở cả hai.

#### Nhóm D — Điều chỉnh điểm thủ công (US-007)

**AC-043** — *US*: US-007 · *FR*: FR-038 · *GR*: `GR-029` C1, `GR-028` C3
**Given** một trận chưa đóng sổ đang ở trạng thái nghỉ, ghế A đang có 5 điểm,
**When** admin điều chỉnh `+5` cho ghế A kèm lý do *"sửa nhầm câu 2"* và xác nhận,
**Then** hệ thống sinh một sự kiện điều chỉnh; điểm ghế A thành **10**; nhật ký thao tác ghi người bấm, lý do và mốc thời gian; điểm mọi ghế khác không đổi; và không phán quyết nào của câu nào bị đụng tới.

**AC-044** — *US*: US-007 · *FR*: FR-041 · *GR*: `GR-029` C2 · `INV-018`
**Given** ghế C đang có **0** điểm trong một trận chưa đóng sổ,
**When** admin điều chỉnh `−10` cho ghế C kèm lý do và xác nhận,
**Then** điểm ghế C thành **−10**; hệ thống **không** kẹp về 0; số âm hiển thị bình thường trên mọi bảng.

**AC-045** — *US*: US-007 · *FR*: FR-040 · *GR*: `GR-029` C3
**Given** một trận chưa đóng sổ, ghế A đang có 10 điểm,
**When** admin điều chỉnh lượng **0** cho ghế A kèm lý do *"ghi nhận khiếu nại của đội A, MC đã giải thích tại chỗ"*,
**Then** hệ thống **vẫn sinh** một sự kiện; điểm ghế A **vẫn 10**; và biên bản trận có thêm một dòng mang đúng lý do đó.

**AC-046** — *US*: US-007, US-008 · *FR*: FR-042 · *GR*: `GR-029` C4, `GR-030` §Không đổi gì
**Given** vòng Khởi động đã cộng tổng `+30` cho ghế A, và admin **đã** điều chỉnh tay `+5` cho ghế A sau đó; điểm ghế A đang là 35,
**When** admin **bỏ** vòng Khởi động kèm lý do,
**Then** chỉ các sự kiện điểm **của vòng đó** bị đảo; điều chỉnh tay `+5` **vẫn giữ**; điểm ghế A thành **5**; và mọi sự kiện cũ còn nguyên trong nhật ký.

**AC-047** — *US*: US-007, US-008 · *FR*: FR-042 · *GR*: `GR-029` C5
**Given** vòng Vượt chướng ngại vật **đã bị bỏ** và điểm của nó đã hoàn nguyên; ghế A đang có `−50` điểm,
**When** admin điều chỉnh `+10` cho ghế A kèm lý do,
**Then** điều chỉnh có hiệu lực bình thường; điểm ghế A thành **−40**; việc vòng đã bị bỏ trước đó không ảnh hưởng gì tới thao tác này.

**AC-048** — *US*: US-007 · *FR*: FR-044 · *GR*: `GR-029` C6 · §Invalid transitions
**Given** một trận **đã đóng sổ** — thử lần lượt với nhãn *hoàn thành* và nhãn *bỏ dở*,
**When** admin tìm cách điều chỉnh điểm cho một ghế, và một yêu cầu cũng được gửi thẳng tới server,
**Then** nút không bật ở cả hai nhãn; hệ thống hiện **toast** invalid state và không cho ép qua; server từ chối yêu cầu; bảng điểm và biên bản đã xuất **không** đổi.

**AC-049** — *US*: US-007 · *FR*: FR-039 · *GR*: `GR-029` §Điều kiện · §Invalid transitions
**Given** một trận chưa đóng sổ và admin đã nhập lượng `+5` cho ghế A,
**When** admin xác nhận với ô lý do **trống**, rồi thử lại với ô lý do chỉ gồm khoảng trắng,
**Then** cả hai lần đều bị từ chối — chuỗi chỉ gồm khoảng trắng bị coi là trống sau khi cắt hai đầu; không sự kiện nào được sinh; điểm ghế A không đổi.

**AC-050** — *US*: US-007 · *FR*: FR-043 · *GR*: `GR-029` §Bấm trùng
**Given** ghế A đang có 10 điểm và admin thực sự muốn cộng tổng `+10`,
**When** admin điều chỉnh `+5` kèm lý do, xác nhận, rồi lặp lại đúng thao tác đó lần thứ hai,
**Then** hệ thống sinh **hai** sự kiện riêng; điểm ghế A thành **20**; hệ thống **không** chống trùng và không gộp hai lần thành một.

**AC-051** — *US*: US-007 · *FR*: FR-038 · *GR*: `GR-029` §Điều kiện
**Given** một vòng đang chạy, một câu đang mở và đồng hồ đang đếm,
**When** admin điều chỉnh `+3` cho ghế B kèm lý do và xác nhận,
**Then** thao tác được chấp nhận ngay giữa vòng; điểm ghế B đổi; đồng hồ của câu đang mở **không** dừng và **không** bị đặt lại; trạng thái câu không đổi.

**AC-052** — *US*: US-007, US-010 · *FR*: FR-045, FR-061a · *GR*: `GR-029` §Điều kiện, C1 · `STATE-035`
**Given** admin đã nhập lượng `+5` và lý do cho một điều chỉnh điểm cho ghế A, dialog xác nhận đang hiện, và ghế A đang có 10 điểm,
**When** admin lần lượt thử: click ra ngoài dialog · nhấn `Esc` · mở lại và gõ phím tắt `Y`,
**Then** click ra ngoài **không** đóng dialog và không mất dữ liệu đã nhập; `Esc` **đóng** dialog và **không lưu thay đổi** — điểm ghế A vẫn 10 và không sự kiện nào được sinh; phím tắt `Y` xác nhận được thao tác; nút xác nhận **tự tắt** sau lần bấm đầu nên thao tác thực thi đúng một lần; và một dòng nhật ký được ghi kèm lý do.

#### Nhóm E — Ba cửa ra chủ động của vòng, và chọn vòng (US-008, US-009)

**AC-053** — *US*: US-008 · *FR*: FR-046, FR-048 · *GR*: `GR-030` C1, `GR-028` C4
**Given** vòng Khởi động đang chạy và đã sinh các sự kiện điểm tổng `+30` cho ghế A; điểm ghế A đang là 40,
**When** admin chọn **Bỏ vòng**, nhập lý do và xác nhận,
**Then** hệ thống sinh một sự kiện đảo ngược cho **mỗi** sự kiện điểm của vòng đó; điểm ghế A thành **10**; **không** sự kiện cũ nào bị xoá; vòng về trạng thái nghỉ; và biên bản giữ vòng đó kèm nhãn *"đã bỏ"*.

**AC-054** — *US*: US-008 · *FR*: FR-046, FR-049 · *GR*: `GR-030` C2, `GR-031`
**Given** vòng Vượt chướng ngại vật đang chạy, đã tiêu 4 câu, và kho đề còn đủ câu cho một lần chạy mới,
**When** admin chọn **Chạy lại vòng**, nhập lý do và xác nhận,
**Then** điểm của vòng được hoàn nguyên bằng sự kiện đảo ngược; hàng đợi đang hoạt động được đặt lại; vòng đi qua **cửa kiểm kho đề** rồi mở lại từ đầu với câu **khác**; 4 câu đã tiêu **không** trả lại kho; và mọi cờ phạm vi vòng của các ghế được đặt lại.

**AC-055** — *US*: US-008 · *FR*: FR-046, FR-050 · *GR*: `GR-030` C3 · `EVENT-004`
**Given** vòng Tăng tốc đang chạy, mới hỏi 2 trong 4 câu, và bảng điểm hiện tại đã có điểm từ hai câu đó,
**When** admin chọn **Kết thúc khẩn cấp**, nhập lý do và xác nhận,
**Then** vòng khép ngay tại chỗ và về trạng thái nghỉ; bảng điểm **không đổi** — không sự kiện đảo ngược nào được sinh; cờ phạm vi vòng được dọn; và biên bản mang nhãn *"kết thúc sớm"*.

**AC-056** — *US*: US-008, US-009 · *FR*: FR-054 · *GR*: `GR-030` C4
**Given** vòng Khởi động đang chạy và chưa rời về trạng thái nghỉ,
**When** admin tìm cách mở vòng Tăng tốc từ trong màn của vòng Khởi động,
**Then** màn của một vòng đang chạy **không có nút** mở vòng khác — đây là invalid state chứ không phải chặn cứng; vòng Khởi động vẫn chạy; và admin phải rời qua một trong bốn cửa trước.

**AC-057** — *US*: US-008, US-010 · *FR*: FR-053 · *GR*: `GR-030` C5, §ghi chú
**Given** vòng Khởi động **đã ở nhãn đã bỏ** và điểm đã hoàn nguyên `−30`,
**When** một lệnh **Bỏ vòng** thứ hai cho chính vòng đó tới server do trễ mạng,
**Then** server từ chối lệnh; điểm **không** tụt gấp đôi; hệ thống phản hồi bằng **toast** invalid state chứ không xử lý như một cú bấm trùng; và không sự kiện nào được sinh.

**AC-058** — *US*: US-008, US-010 · *FR*: FR-047, FR-061 · *GR*: `GR-030` §Điều kiện · `STATE-035`
**Given** một vòng đang chạy,
**When** admin kích hoạt lần lượt **Bỏ vòng**, **Chạy lại vòng** và **Kết thúc khẩn cấp**, và ở mỗi dialog thử nhấn `Esc` rồi thử click ra ngoài,
**Then** cả ba dialog **không đóng** bằng `Esc` hay click ra ngoài; cả ba **bắt nhập lý do**; và bỏ trống lý do thì nút xác nhận không thực thi được.

**AC-059** — *US*: US-006, US-008 · *FR*: FR-051 · *GR*: `GR-030` C3 · `EVENT-004` §side effect 2 · `INV-010`
**Given** vòng Tăng tốc đang chạy với **một câu đang mở** mà mới hai trong bốn ghế được đánh dấu,
**When** admin chọn **Kết thúc khẩn cấp** và xác nhận,
**Then** câu đang mở khép bằng **Huỷ kết quả** — không sinh điểm cho ai; hai ghế chưa đánh dấu **không** bị ép thành Sai; bảng điểm không đổi; và không tồn tại trạng thái *"vòng đã đóng mà còn câu chưa phán quyết"*.

**AC-060** — *US*: US-008 · *FR*: FR-049 · *GR*: `GR-030` C2, `GR-031` C3 · `INV-014`
**Given** vòng Vượt chướng ngại vật đang chạy, và kho đề của contest chỉ còn **3** câu khả dụng trong khi vòng cần nhiều hơn,
**When** admin chọn **Chạy lại vòng** và xác nhận,
**Then** cửa kiểm kho đề **chặn cứng** thao tác và nêu số câu còn thiếu; vòng **không** được hoàn nguyên và **không** mở lại; bảng điểm không đổi; và lối thoát được nêu là sửa danh sách câu ở trạng thái nghỉ.

**AC-061** — *US*: US-008, US-015 · *FR*: FR-052 · *GR*: `GR-030` §Điều kiện · `INV-011` · `EVENT-004` §side effect 4
**Given** một vòng đã hiển thị 3 câu và đã rút nhưng chưa hiển thị 1 câu,
**When** admin lần lượt thử cả ba cửa **bỏ vòng**, **chạy lại vòng** và **kết thúc khẩn cấp** trên ba lần chạy khác nhau,
**Then** ở cả ba lần, 3 câu **đã hiển thị** vẫn mang cờ đã-dùng và **không** trả về kho; câu **đã rút chưa hiển thị** chưa tiêu và trả về kho; và cờ đã-dùng không bao giờ bị gỡ.

**AC-062** — *US*: US-009, US-010 · *FR*: FR-055, FR-056 · *GR*: `GR-030` C4 · `INV-020`
**Given** trận đang ở trạng thái nghỉ, hệ thống khuyến nghị mở vòng Vượt chướng ngại vật theo cấu hình contest,
**When** admin chọn mở vòng **Về đích** thay vì vòng được khuyến nghị,
**Then** hệ thống hiện dialog cảnh báo nêu **tên luật bị lệch**; bấm **Yes** thì vòng Về đích mở bình thường; bấm **No** thì không có gì xảy ra — trận vẫn ở trạng thái nghỉ, không sự kiện nào được sinh, không cờ nào bị đặt lại.

**AC-063** — *US*: US-009 · *FR*: FR-055, FR-057 · *GR*: `INV-020`
**Given** cùng trạng thái như AC-062,
**When** admin chọn đúng vòng **được khuyến nghị**,
**Then** vòng mở ngay mà **không** dialog cảnh báo nào hiện; mọi cờ phạm vi vòng được đặt lại theo quy tắc mở vòng.

**AC-064** — *US*: US-008 · *FR*: FR-046, FR-047 · *GR*: `GR-030` §Mục đích
**Given** một vòng đã hỏi **đủ** số câu theo luật và câu cuối đã được chấm,
**When** admin bấm **Kết thúc vòng** — cửa thường,
**Then** vòng về trạng thái nghỉ; điểm giữ nguyên; hệ thống **không** đòi nhập lý do; và biên bản ghi vòng đó ở nhãn *hiệu lực*.

#### Nhóm F — Ba hạng cảnh báo giao diện (US-010)

**AC-065** — *US*: US-010 · *FR*: FR-058, FR-059 · *GR*: `PRD §9.4` · `STATE-037`
**Given** một trận **đã đóng sổ**, và song song một câu thuộc **vòng đã bị bỏ**,
**When** admin lần lượt tìm cách điều chỉnh điểm ở trận đã đóng sổ và chấm câu thuộc vòng đã bỏ,
**Then** cả hai lần hệ thống hiện **toast** invalid state; hai toast mang **thông điệp khác nhau** ứng với hai nguyên nhân khác nhau; không lần nào cho ép qua; và không dữ liệu nào đổi.

**AC-066** — *US*: US-010, US-011 · *FR*: FR-060, FR-061a · *GR*: `STATE-035`
**Given** một câu đang chạy và một ô chữ chưa mở trong vòng Vượt chướng ngại vật,
**When** admin lần lượt bấm **mở đáp án** và **mở miếng ghép**, và ở mỗi dialog thử `Esc`, click ra ngoài, và phím tắt `Y`,
**Then** cả hai đi qua dialog **Yes/No thường**; bấm Yes hoặc gõ `Y` thì thao tác thực thi và sinh một dòng nhật ký; bấm No, nhấn `Esc`, hoặc click ra ngoài đều tương đương **No** và **không có gì xảy ra**; và cả hai dialog **không** đòi nhập lý do.

**AC-067** — *US*: US-008, US-010 · *FR*: FR-061, FR-061a · *GR*: `GR-030` §Điều kiện · `STATE-035` · `GR-034`
**Given** một vòng đang chạy,
**When** admin kích hoạt **Bỏ vòng** rồi thử nhấn `Esc`, click ra ngoài, gõ phím tắt `Y`, và bấm xác nhận với ô lý do trống,
**Then** dialog **không đóng** ở hai thử nghiệm đầu; phím tắt `Y` **không** xác nhận được vì hạng phá huỷ chỉ nhận click chuột; nút xác nhận không thực thi khi lý do trống; vòng vẫn đang chạy; và không sự kiện nào được sinh qua cả bốn thử nghiệm.

**AC-068** — *US*: US-010, US-011, US-017 · *FR*: FR-062 · *GR*: `PRD-REQ-056`
**Given** đáp án đang được hiển thị cho mọi vai, và lớp công bố kết quả đang bật,
**When** admin bấm **đóng** đáp án, rồi bấm **đóng** lớp công bố,
**Then** cả hai thao tác thực thi **ngay**, không dialog xác nhận nào hiện; trạng thái bên dưới lộ lại nguyên vẹn; và cả hai vào nhật ký thao tác.

**AC-069** — *US*: US-007, US-010 · *FR*: FR-063, FR-061a · *GR*: `PRD §9.4` · `STATE-035` → `STATE-037`
**Given** một trận đang chạy,
**When** admin lần lượt gặp một toast invalid state, một dialog Yes/No thường, một dialog điều chỉnh điểm, và một dialog hạng phá huỷ,
**Then** bốn phản hồi **phân biệt được bằng mắt**; ba tầng dialog khác nhau đúng ở **cách thoát** — Yes/No thường thoát bằng cả `Esc` lẫn click ngoài, điều chỉnh điểm chỉ thoát bằng `Esc`, hạng phá huỷ không thoát được bằng đường nào; hai tầng sau bắt nhập lý do còn tầng đầu thì không; và toast không có nút nào để bấm.

**AC-070** — *US*: US-009, US-010, US-014 · *FR*: FR-063a, FR-056, FR-096 · *GR*: `STATE-036`, `STATE-037`
**Given** một trận đang chạy ở trạng thái nghỉ, hệ thống khuyến nghị một vòng cụ thể,
**When** admin chọn một vòng khác khuyến nghị, rồi ở dialog hiện ra lần lượt thử `Esc`, click ra ngoài, và phím tắt `Y`,
**Then** dialog dùng **y hệt** tầng Yes/No thường về cách thoát — `Esc` và click ra ngoài đều đóng và đều tương đương **No** nên vòng không mở, phím tắt `Y` ép qua được; dialog **không** bắt nhập lý do; nội dung chữ nêu rõ **điểm lệch luật** chứ không nêu hậu quả không hoàn tác được; và dialog này vẫn khác toast invalid state ở chỗ **ép được**, dù hai thứ không đòi phân biệt thị giác với dialog xác nhận.

**AC-071** — *US*: US-010 · *FR*: FR-058 · *GR*: `PRD §9.4` · `STATE-037`
**Given** một thao tác của admin ở invalid state bất kỳ,
**When** toast hiện ra,
**Then** toast **không** có ô nhập lý do, **không** có nút ép qua, và tự tắt sau khi hiện xong — nó là thông báo chứ không phải một mốc thời gian của luật.

**AC-072** — *US*: US-010 · *FR*: FR-064 · *GR*: `STATE-037` · `PRD §9.4`
**Given** một ghế thí sinh đang ở trạng thái mà một thao tác không hợp lệ — ví dụ chuông đã khoá sau khi bấm,
**When** thí sinh tìm cách thực hiện thao tác đó,
**Then** trên máy thí sinh **không** có toast nào — nút không render và bấm không phản hồi; không tín hiệu nào được tạo; và không có gì để báo vì không có sự kiện nào xảy ra.

#### Nhóm G — Mở và đóng đáp án (US-011)

**AC-073** — *US*: US-011 · *FR*: FR-065, FR-067 · *GR*: `GR-037` C3, C4 · `PERM-044`
**Given** một trận có cờ *hiện đáp án sau khi chấm* đang **TẮT**, một câu đang mở và **chưa khép**, và thí sinh, khán giả, lớp phủ đang **không** nhận đáp án,
**When** admin bấm **mở đáp án** và xác nhận dialog,
**Then** đáp án tới **mọi vai đang xem** — máy thí sinh, màn khán giả và lớp phủ dựng stream — bất chấp cờ đang tắt; một dòng nhật ký được ghi; và bảng điểm không đổi.

**AC-074** — *US*: US-011 · *FR*: FR-068 · *GR*: `GR-037` §Không đổi gì · `INV-021`
**Given** cùng trạng thái như AC-073 ngay sau khi admin mở đáp án,
**When** admin quan sát trạng thái trận,
**Then** **không** sự kiện điểm nào được sinh; trạng thái trận, vòng, câu, ghế và tín hiệu **không** đổi; đồng hồ không bị đụng; hàng đợi không bị đụng.

**AC-075** — *US*: US-010, US-011 · *FR*: FR-066 · *GR*: `STATE-035`
**Given** một câu đang chạy và đáp án đang kín với thí sinh, khán giả, lớp phủ,
**When** admin bấm **mở đáp án**,
**Then** một dialog Yes/No hiện trước; bấm No thì đáp án **vẫn kín** và không dòng nhật ký nào được ghi; chỉ bấm Yes mới đẩy đáp án đi.

**AC-076** — *US*: US-011 · *FR*: FR-070 · *GR*: `GR-037` C2 · `PERM-044` vs `PERM-045`
**Given** một contest có gán MC, và một phiên MC đang mở màn MC trong lúc phiên admin đang giữ quyền điều khiển,
**When** MC xem màn của mình ở một câu chưa khép,
**Then** MC **thấy** đáp án trên màn của chính mình và mỗi lần xem vào nhật ký; nhưng màn MC **không có nút** mở hay đóng đáp án cho các vai khác; và một yêu cầu mở đáp án phát từ phiên MC bị server từ chối.

**AC-077** — *US*: US-011 · *FR*: FR-065 · *GR*: `GR-037` C5, §Bấm trùng
**Given** một câu **đã khép** trong một trận có cờ công bố đang **BẬT**, nên đáp án đã tự tới thí sinh, khán giả và lớp phủ,
**When** admin bấm **đóng** đáp án,
**Then** đáp án thu lại khỏi ba kênh đó; thao tác không đòi dialog; một dòng nhật ký được ghi; và điểm cùng trạng thái trận không đổi.

**AC-078** — *US*: US-011 · *FR*: FR-071 · *GR*: `GR-037` C8
**Given** một câu Về đích mà **người cướp quyền** vừa bị chấm **Huỷ kết quả**, cửa sổ cướp đã đóng, và cờ công bố của trận đang **BẬT**,
**When** phán quyết được chốt,
**Then** server **không** tự đẩy đáp án tới thí sinh, khán giả hay lớp phủ — quy tắc phát biểu theo **loại phán quyết** chứ không theo vai bị chấm; đáp án chỉ hiện nếu admin bấm mở tay.

**AC-079** — *US*: US-011, US-014 · *FR*: FR-072 · *GR*: `GR-037` C8b · `GR-032` §Kích hoạt tay
**Given** một câu Khởi động lượt chung mà người giữ quyền vừa bị chấm **Huỷ kết quả**, hàng đợi còn tín hiệu trơ, admin **chưa** bấm kích hoạt tay, và cờ công bố đang **BẬT**,
**When** một khoảng thời gian bất kỳ trôi qua,
**Then** đáp án **không** rời server tới thí sinh, khán giả hay lớp phủ — mốc câu khép đã lùi tới sau khi người được kích hoạt được chấm; và bộ kiểm không-rò-đáp-án pass trên cả ba kênh.

**AC-080** — *US*: US-011 · *FR*: FR-069 · *GR*: `GR-037` C1, §Điều kiện, §Bấm trùng
**Given** admin đang giữ quyền điều khiển và một câu đang chạy,
**When** admin xem đáp án trên màn của mình ba lần và bấm mở đáp án cho mọi vai một lần,
**Then** nhật ký có ba dòng *xem đáp án* và một dòng *mở đáp án*; trạng thái trận không đổi qua bốn thao tác đó.

#### Nhóm H — Bàn cờ Vượt chướng ngại vật (US-012)

**AC-081** — *US*: US-012 · *FR*: FR-074 · *GR*: `STATE-041` → `STATE-043`
**Given** vòng Vượt chướng ngại vật vừa mở, bàn cờ có 4 hàng ngang và 1 ô trung tâm,
**When** admin xem bàn cờ trên màn điều khiển,
**Then** cả năm ô đang ở giá trị **chờ**; mỗi ô mang đúng một trong ba giá trị; và đặt một ô sang giá trị khác **không** làm đổi giá trị của bốn ô còn lại.

**AC-082** — *US*: US-012 · *FR*: FR-075, FR-077 · *GR*: `GR-009` C12 · `T-084`
**Given** vòng Vượt chướng ngại vật đang chạy, cả 4 hàng ngang đang ở **chờ**, băng điểm Chướng ngại vật đang là **60**, và điểm mọi ghế đã biết,
**When** admin đặt tay một hàng ngang sang **đã hỏi** và xác nhận dialog,
**Then** băng tụt xuống **50** — đúng một bậc, y như một lượt hỏi thật; **không ghế nào** được cộng điểm; câu của ô đó **không** tiêu; đồng hồ và hàng đợi không bị đụng; và một dòng nhật ký được ghi.

**AC-083** — *US*: US-012 · *FR*: FR-078 · *GR*: `GR-009` C13 · `STATE-043`
**Given** cùng trạng thái như AC-082 sau khi một hàng ngang đã ở **đã hỏi** và băng đang là **50**,
**When** admin đặt tay chính ô đó sang **mở**,
**Then** băng **vẫn 50** — biến mà băng đọc là *đã hỏi*, không phải *đã lộ*; nên mở tay trọn một ô chỉ tính **một** lần; không ai được cộng điểm.

**AC-084** — *US*: US-012 · *FR*: FR-075, FR-079 · *GR*: `T-084`
**Given** hai hàng ngang đang ở **đã hỏi** và băng đang là **40**,
**When** admin đặt tay một trong hai ô đó **lùi** về **chờ**,
**Then** băng **lên lại 50** — băng được tính lại từ trạng thái bốn hàng ngang chứ không phải từ một bộ đếm; không sự kiện điểm nào được sinh.

**AC-085** — *US*: US-012 · *FR*: FR-080 · *GR*: `T-085`
**Given** một ô đang ở **đã hỏi** và băng đang là 50,
**When** admin đặt chính ô đó sang **đã hỏi** một lần nữa,
**Then** thao tác là phép gán rỗng — trạng thái ô không đổi, băng **không** tụt thêm bậc nào, và không sự kiện điểm nào được sinh.

**AC-086** — *US*: US-012 · *FR*: FR-075, FR-081 · *GR*: `EVENT-021` §Ba ranh giới
**Given** vòng Vượt chướng ngại vật đang chạy với **một câu hàng ngang khác đang mở và đồng hồ đang đếm**, hàng đợi có một tín hiệu đang chờ duyệt,
**When** admin đặt tay một ô **không** liên quan tới câu đang mở sang **đã hỏi**,
**Then** thao tác được chấp nhận; **không** câu nào bị tiêu; đồng hồ của câu đang mở **không** dừng và không bị đặt lại; hàng đợi và tín hiệu đang chờ **không** đổi; và một dòng nhật ký được ghi.

**AC-087** — *US*: US-012, US-010 · *FR*: FR-082 · *GR*: §Invalid transitions · `STATE-036`
**Given** một câu hàng ngang **đang được hỏi dở** — đã hiển thị, đồng hồ đang chạy, chưa chấm,
**When** admin đặt tay trạng thái của **chính ô đó**,
**Then** hệ thống hiện **dialog cảnh báo** lệch luật; bấm Yes thì thao tác thực thi bình thường và băng tính lại; bấm No thì trạng thái ô không đổi.

**AC-088** — *US*: US-012 · *FR*: FR-083 · *GR*: `EVENT-021`
**Given** trận đang ở vòng Tăng tốc — ngoài vòng Vượt chướng ngại vật,
**When** admin xem màn điều khiển,
**Then** nút đặt trạng thái ô chữ **không tồn tại** — không có bàn cờ nào để đặt.

**AC-089** — *US*: US-012 · *FR*: FR-076 · *GR*: `GR-011` C1, C3 · `STATE-042`
**Given** cả **4** hàng ngang đã được hỏi, chưa ai giải đúng Chướng ngại vật, băng đang là **30**, và ô trung tâm đang ở **chờ**,
**When** admin bấm **đưa ra gợi ý cuối**, sau đó chấm **Sai** cho câu ô trung tâm,
**Then** băng hạ xuống **20** ngay tại mốc gợi ý cuối; ô trung tâm **không** mở vì câu bị chấm Sai; băng **vẫn 20** — 20 là sàn và không phụ thuộc câu ô trung tâm đúng hay sai; và ô trung tâm không tham gia phép tính băng.

**AC-090** — *US*: US-011, US-012 · *FR*: FR-073, FR-084 · *GR*: `GR-012` C2, C5 · `T-086` · `GR-037` C9
**Given** vòng Vượt chướng ngại vật đã khép vì toàn bộ thí sinh bị loại, một số ô còn ở **chờ** và một số ở **đã hỏi**,
**When** admin bấm **công bố Chướng ngại vật**, rồi bấm lần thứ hai,
**Then** lần đầu đưa **mọi** ô sang **mở** và hiện Chướng ngại vật cho khán giả; lần thứ hai không tồn tại vì nút một chiều đã tự tắt và server bỏ qua lệnh trùng; không ai được cộng điểm; và đáp án Chướng ngại vật đi theo đường này chứ không theo cơ chế công bố của `GR-037`.

**AC-090a** — *US*: US-011, US-012 · *FR*: FR-073 · *GR*: `GR-012` C3
**Given** cùng trạng thái như AC-090, và admin **không** bấm công bố,
**When** admin bấm kết thúc vòng,
**Then** vòng vẫn khép được bình thường — công bố là **tuỳ chọn**, không phải điều kiện; không trạng thái tắc nào phát sinh.

**AC-090b** — *US*: US-012, US-018 · *FR*: FR-084 · *GR*: `EVENT-018`
**Given** một câu hàng ngang đã chấm xong với ít nhất một ghế được chấm Đúng, miếng ghép tương ứng chưa mở,
**When** admin bấm **mở miếng ghép** rồi bấm lần thứ hai,
**Then** lần đầu đi qua dialog xác nhận và đưa ô sang **mở**; nút tự tắt nên lần thứ hai không phát được thao tác; băng điểm **không** đổi qua thao tác này.

**AC-090c** — *US*: US-012, US-018 · *FR*: FR-084 · *GR*: `GR-011` C8
**Given** câu ô trung tâm đã được chấm Đúng và ô trung tâm vừa được mở,
**When** admin bấm **mở ô trung tâm** lần thứ hai,
**Then** thao tác không tồn tại — nút một chiều đã tự tắt; server bỏ qua lệnh trùng; trạng thái ô không đổi.

#### Nhóm I — Màn hàng đợi và kích hoạt tay (US-013, US-014)

**AC-091** — *US*: US-013 · *FR*: FR-085, FR-087 · *GR*: `GR-032` C1
**Given** vòng Vượt chướng ngại vật đang ở pha chờ chọn hàng ngang, hàng đợi đang trống,
**When** ghế A chọn một hàng ngang trong cửa sổ hợp lệ,
**Then** tín hiệu vào hàng đợi ở trạng thái **chờ duyệt** kèm server timestamp; ô chữ **chưa** đổi trạng thái; câu chưa tiêu; đồng hồ chưa chạy; và tín hiệu hiện trên màn admin trong phần *hàng đợi đang hoạt động*.

**AC-092** — *US*: US-013 · *FR*: FR-087 · *GR*: `GR-032` C2, `GR-009` C7 · `EVENT-022`
**Given** hàng đợi chặn có hai tín hiệu đang chờ, tín hiệu của ghế A ở **đầu** hàng theo server timestamp; một hàng ngang đang ở **chờ**,
**When** admin bấm **Yes** cho tín hiệu của ghế A,
**Then** tín hiệu thực thi; ô được chọn chuyển sang **đã hỏi** và băng điểm chốt tại **mốc xác nhận này**; tín hiệu của ghế B lên đầu hàng; nút duyệt của tín hiệu A tự tắt.

**AC-093** — *US*: US-013 · *FR*: FR-088 · *GR*: `GR-032` C3, `GR-009` C8 · `INV-008`
**Given** cùng trạng thái như AC-092, ghế A chưa dùng lượt chọn của mình, ô chữ đang ở **chờ**, câu chưa tiêu, đồng hồ chưa chạy,
**When** admin bấm **No** cho tín hiệu của ghế A,
**Then** tín hiệu bị bỏ và tín hiệu của ghế B lên đầu hàng; ghế A **không mất lượt**; ô chữ **vẫn ở chờ**; câu **chưa tiêu**; đồng hồ **chưa chạy**; ở mode nhập liệu nút chọn của ghế A **mở lại**; và tín hiệu bị từ chối vẫn nằm nguyên trong lịch sử.

**AC-094** — *US*: US-013 · *FR*: FR-090 · *GR*: `GR-032` C4
**Given** một câu Khởi động lượt chung với cửa sổ chuông đang mở,
**When** ghế A bấm chuông ở `3,2` giây và ghế B bấm ở `3,5` giây,
**Then** ghế A giành quyền **ngay** theo server timestamp mà không chờ admin duyệt; tín hiệu của ghế B vào lịch sử ở trạng thái **trơ**; màn admin hiện thứ tự hai tín hiệu nhưng **không** có nút duyệt cho vòng này.

**AC-095** — *US*: US-013 · *FR*: FR-086 · *GR*: `GR-032` C5 · `INV-001`
**Given** vòng Vượt chướng ngại vật vừa kết thúc, trong vòng đó đã có ít nhất năm tín hiệu với đủ ba kết cục — thực thi, bị từ chối, trơ,
**When** admin mở màn hàng đợi,
**Then** phần **hàng đợi đang hoạt động** trống; phần **lịch sử tín hiệu** vẫn hiện đủ năm tín hiệu kèm timestamp và kết cục của từng cái; và không tín hiệu nào bị xoá.

**AC-096** — *US*: US-013 · *FR*: FR-091 · *GR*: `GR-032` C6, `GR-010` C4
**Given** ghế D **đã bị loại** khỏi vòng Vượt chướng ngại vật sau khi giải sai Chướng ngại vật,
**When** ghế D tìm cách chọn một hàng ngang hoặc bấm *"Mở chướng ngại vật"*,
**Then** máy ghế D **không hiển thị** nút nào và bấm không phản hồi ⇒ **không tín hiệu nào được tạo**; hàng đợi đang hoạt động không có mục mới; và đây không phải drop vì không có gì để drop.

**AC-097** — *US*: US-013 · *FR*: FR-086, FR-092 · *GR*: `GR-032` C7 · `INV-001`
**Given** admin đã bấm No cho một tín hiệu của ghế A ở một pha đã kết thúc, và ghế A khiếu nại,
**When** admin mở lịch sử tín hiệu và tra lại,
**Then** tín hiệu bị từ chối hiện đủ kèm server timestamp, kết cục *bị từ chối*, và ghế phát; admin dùng bản ghi đó làm căn cứ để **gỡ lệnh cấm** hoặc điều chỉnh điểm; và bản ghi **không** bị sửa hay xoá bởi thao tác đó.

**AC-098** — *US*: US-013 · *FR*: FR-085, FR-086 · *GR*: `GR-032` · `INV-006`
**Given** một vòng đang chạy đã phát sinh cả tín hiệu có hiệu lực lẫn tín hiệu trơ,
**When** admin mở màn hàng đợi,
**Then** mọi tín hiệu hiện đủ kèm server timestamp; tín hiệu trơ mang nhãn *"không có hiệu lực"*; phần **đang hoạt động** và phần **lịch sử** tách bạch rõ trên màn; và không tín hiệu nào đã tới server mà thiếu kết cục.

**AC-099** — *US*: US-013, US-018 · *FR*: FR-087, FR-089 · *GR*: `GR-032` §Bấm trùng · `INV-007`
**Given** hàng đợi chặn có ba tín hiệu với ba server timestamp khác nhau,
**When** admin duyệt tín hiệu đầu, rồi bấm nút duyệt của chính tín hiệu đó lần thứ hai, và bấm nút từ chối của một tín hiệu đã bị từ chối trước đó,
**Then** hàng đợi xử lý **thuần FIFO theo timestamp**, không ưu tiên theo loại tín hiệu hay số ghế; cả hai cú bấm lặp đều không phát được thao tác vì nút một chiều đã tự tắt; server bỏ qua lệnh trùng; và từ chối là **idempotent**.

**AC-100** — *US*: US-014 · *FR*: FR-093 · *GR*: `GR-032` C8, §Kích hoạt tay · `EVENT-052`, `T-099`
**Given** một câu Khởi động lượt chung mà ghế A đã giành quyền và ghế B có một tín hiệu **trơ chưa từng được xử lý**; admin vừa chấm **Huỷ kết quả** cho ghế A,
**When** admin bấm **kích hoạt tay** và chọn tín hiệu của ghế B,
**Then** ghế B thành người giữ quyền mới; tín hiệu của ghế B chuyển từ *không có hiệu lực* sang *có hiệu lực*; **cả hai** trạng thái giữ trong lịch sử; không sự kiện điểm nào được sinh bởi chính thao tác này; và cờ đã-dùng của câu không đổi.

**AC-101** — *US*: US-010, US-014 · *FR*: FR-096 · *GR*: `GR-032` C9 · `INV-014`, `INV-020`
**Given** cùng trạng thái như AC-100 nhưng hàng đợi có **hai** tín hiệu trơ — ghế B lúc `3,2` giây và ghế C lúc `3,6` giây; hệ thống khuyến nghị ghế B,
**When** admin chọn **ghế C**,
**Then** hệ thống hiện **dialog cảnh báo lệch luật** Yes/No nêu rõ **ghế B bị vượt** và thứ tự gốc theo timestamp; dialog **không** bắt nhập lý do; bấm Yes thì ghế C thành người giữ quyền; bấm No thì không có gì đổi; và đây **không** trở thành một chỗ chặn cứng thứ tư.

**AC-102** — *US*: US-014 · *FR*: FR-097 · *GR*: `GR-032` §Kích hoạt tay · `T-097`
**Given** ba tình huống song song — một câu Khởi động lượt chung, một câu Câu hỏi phụ, và một câu Về đích ở pha cướp quyền — mỗi tình huống vừa có một phán quyết **Huỷ kết quả** và còn ứng viên trong hàng đợi,
**When** admin kích hoạt tay ở từng tình huống,
**Then** ở Khởi động lượt chung ghế được kích hoạt nhận **trọn 3 giây** tính từ mốc bấm kích hoạt; ở Câu hỏi phụ nhận **trọn 15 giây**; ở Về đích **không** cửa sổ nào được mở vì vòng đó không có đồng hồ trả lời riêng; và không cửa sổ nào kế thừa thời gian còn lại của lượt trước.

**AC-103** — *US*: US-014 · *FR*: FR-098 · *GR*: `GR-032` §Kích hoạt tay · `T-098`
**Given** một câu Khởi động lượt chung đã dùng **một** lần kích hoạt tay, và ghế được kích hoạt cũng vừa bị chấm **Huỷ kết quả**; hàng đợi vẫn còn một tín hiệu trơ,
**When** admin tìm cách kích hoạt tay lần thứ hai cho **cùng câu** đó,
**Then** hệ thống hiện **toast** invalid state và **không** cho ép qua — hạn mức ở ba vòng chuông là đúng một lần cho mỗi câu; không người giữ quyền mới nào được đánh dấu; và câu đi tiếp theo luật bình thường.

**AC-104** — *US*: US-014 · *FR*: FR-098, FR-012 · *GR*: `GR-032` §Kích hoạt tay, `GR-009` C6b · `T-098`
**Given** vòng Vượt chướng ngại vật có **ba** tín hiệu *"Mở chướng ngại vật"* trong hàng đợi chặn; admin đã xác nhận tín hiệu thứ nhất rồi chấm **Huỷ kết quả** cho nó,
**When** admin kích hoạt tay tín hiệu thứ hai, chấm Huỷ kết quả cho nó, rồi kích hoạt tay tín hiệu thứ ba,
**Then** cả hai lần kích hoạt đều được chấp nhận — ở Vượt chướng ngại vật hạn mức là **một lần cho mỗi phán quyết Huỷ kết quả**, không có trần tổng trong một vòng; chuỗi chỉ dừng khi hàng đợi tín hiệu Chướng ngại vật cạn; và không ghế nào bị đặt cờ bị loại bởi các phán quyết Huỷ kết quả đó.

**AC-105** — *US*: US-014 · *FR*: FR-095 · *GR*: `GR-032` §Kích hoạt tay · `T-099`, `T-100` · `GR-010`, `INV-019`
**Given** một vòng Vượt chướng ngại vật vừa có một phán quyết **Huỷ kết quả**; trong hàng đợi có tín hiệu của ghế B *(trơ, chưa xử lý)*, ghế D *(đã bị loại khỏi vòng)*, ghế E *(đang bị vô hiệu hoá)*, và ghế A là ghế **vừa bị Huỷ kết quả**,
**When** admin mở danh sách ứng viên kích hoạt tay,
**Then** danh sách gồm đúng **ghế B** và **ghế A** — admin đưa lại được chính người vừa bị huỷ cho một lượt thứ hai; **ghế D** và **ghế E** không xuất hiện; và chọn ghế A thì tín hiệu cũ của ghế A ở nguyên trạng thái đã duyệt, không sinh trạng thái tín hiệu mới.

**AC-106** — *US*: US-014 · *FR*: FR-094 · *GR*: `GR-032` §Kích hoạt tay · `EVENT-052`
**Given** một câu Khởi động lượt chung với hàng đợi còn tín hiệu trơ,
**When** admin chấm **Sai** cho người giữ quyền, rồi ở một câu khác chấm **Đúng** cho người giữ quyền,
**Then** ở cả hai trường hợp nút **kích hoạt tay không bật** — nhánh này không tồn tại sau *Đúng* hay *Sai*; câu khép theo luật bình thường; và mốc công bố đáp án **không** bị lùi.

**AC-106a** — *US*: US-014 · *FR*: FR-099 · *GR*: `GR-032` §Kích hoạt tay, `GR-004`, `GR-020`
**Given** ghế B vừa được kích hoạt tay ở một câu Khởi động lượt chung, ghế B đang có 20 điểm,
**When** admin chấm **Sai** cho ghế B sau khi cửa sổ 3 giây mới đóng,
**Then** ghế B nhận `−5` — **y hệt** một người giành quyền bình thường của vòng đó; hệ thống **không** dùng một bảng điểm riêng cho người được kích hoạt; điểm ghế B thành **15**.

**AC-106b** — *US*: US-014 · *FR*: FR-100, FR-126 · *GR*: `EVENT-052` §Side effects
**Given** admin vừa kích hoạt tay ghế C trong khi hệ thống khuyến nghị ghế B,
**When** admin mở nhật ký thao tác,
**Then** có một dòng ghi rõ **ghế được chọn** là C, **ghế bị vượt** là B, và **thứ tự gốc** theo server timestamp; dòng đó không sửa được; và trạng thái bàn cờ Vượt chướng ngại vật cùng cờ đã-dùng của câu không đổi bởi thao tác này.

#### Nhóm J — Ba trạng thái câu trên màn admin (US-015)

**AC-107** — *US*: US-015 · *FR*: FR-101 · *GR*: `GR-031` §Ranh giới "đã dùng"
**Given** một trận đã chạy một phần: trong danh sách câu đã gán có câu chưa rút, câu đã rút mà chưa hiển thị, và câu đã hiển thị,
**When** admin mở màn danh sách câu ở trạng thái nghỉ,
**Then** ba nhóm phân biệt được **bằng mắt**; thao tác gỡ chỉ khả dụng ở nhóm *đã rút chưa hiển thị*; và nhóm *đã hiển thị* mang dấu không gỡ được.

**AC-108** — *US*: US-015 · *FR*: FR-102 · *GR*: `GR-031` C7 · `INV-011`
**Given** một câu đã được rút nhưng **chưa hiển thị** cho ai, trận đang ở trạng thái nghỉ,
**When** admin gỡ câu đó khỏi danh sách gán,
**Then** thao tác được chấp nhận; câu quay về kho vì chưa tiêu; cờ đã-dùng của nó chưa từng được đặt; và phép kiểm kho đề chạy lại sau khi sửa.

**AC-109** — *US*: US-015 · *FR*: FR-103 · *GR*: `GR-031` C6 · `INV-011`
**Given** một câu **đã hiển thị** cho thí sinh trong một vòng đã chạy, trận đang ở trạng thái nghỉ,
**When** admin tìm cách gỡ câu đó khỏi danh sách gán, và một yêu cầu cũng được gửi thẳng tới server,
**Then** hệ thống hiện **toast** invalid state và không cho ép qua; server từ chối yêu cầu; cờ đã-dùng của câu **không** bị gỡ; và danh sách gán không đổi.

**AC-109a** — *US*: US-015 · *FR*: FR-104 · *GR*: `GR-031` C8 · `EVENT-031`
**Given** một vòng **đang chạy**,
**When** admin tìm cách mở cửa sửa danh sách câu đã gán,
**Then** hệ thống hiện **toast** invalid state; danh sách gán không đổi; và tập câu khả dụng giữ **bất biến suốt vòng** đó.

**AC-109b** — *US*: US-015 · *FR*: FR-104 · *GR*: `GR-031` C5
**Given** trận đang ở trạng thái nghỉ và một vòng sắp mở đang thiếu câu,
**When** admin thêm câu từ kho vào danh sách gán và lưu,
**Then** phép kiểm kho đề **chạy lại** ngay sau khi sửa; vòng mở được nếu đã đủ; thao tác vào nhật ký; và cờ đã-dùng của mọi câu **không** bị đụng tới.

#### Nhóm K — Ghế: vô hiệu hoá, kích hoạt lại, mất kết nối (US-016)

**AC-110** — *US*: US-016 · *FR*: FR-105, FR-106 · *GR*: `GR-036` C5 · `STATE-025` · `INV-006`
**Given** một câu đang mở với đồng hồ đang chạy; ghế E đang online, có 25 điểm và đứng ở vị trí 3 trên bảng xếp hạng,
**When** admin vô hiệu hoá ghế E qua dialog Yes/No kèm lý do và xác nhận,
**Then** ghế E mất quyền thao tác nhưng **ở lại trong trận**: giữ nguyên 25 điểm, giữ nguyên vị trí, vẫn hiện trên mọi bảng; ghế E không được đề xuất lượt; một tín hiệu lỡ tới từ ghế E vào lịch sử ở trạng thái **trơ**, không bị drop; đồng hồ **không** dừng; và thao tác vào nhật ký kèm lý do.

**AC-111** — *US*: US-016 · *FR*: FR-107 · *GR*: `GR-036` C6, §Không đổi gì
**Given** ghế E đang bị vô hiệu hoá, và trong lúc đó một câu đã bị chốt khiến ghế E nhận mặc định Sai,
**When** admin kích hoạt lại ghế E,
**Then** ghế E thi tiếp bình thường; **không có gì bị hoàn nguyên** — điểm ghi trong lúc bị vô hiệu hoá giữ nguyên, bài đã chấm không đổi, lịch sử tín hiệu không đổi.

**AC-112** — *US*: US-006, US-016 · *FR*: FR-109 · *GR*: `GR-036` C7, `GR-026` C4
**Given** một câu Tăng tốc đang mở; ghế E bị admin vô hiệu hoá **giữa** câu đó và chưa gửi bài nào,
**When** admin bấm **chốt câu**,
**Then** ghế E được xử **y như ghế không trả lời** — tính là Sai, nhận 0, không giữ chỗ trong thang; dialog xác nhận liệt kê ghế E trong danh sách sắp bị mặc định Sai; và nếu admin thấy bất công thì đường bù duy nhất là điều chỉnh điểm thủ công.

**AC-113** — *US*: US-016 · *FR*: FR-110 · *GR*: `GR-036` C2, C4
**Given** ghế F mất kết nối lúc `t=0` và ngưỡng chờ mặc định là 120 giây; một vòng đang chạy với một câu đang mở,
**When** đồng hồ vượt quá `120,000` giây kể từ mốc mất kết nối và admin chưa làm gì,
**Then** ghế F được **tô nổi bật** trên màn admin kèm **thời lượng** đã mất kết nối; hệ thống **không** tự loại, tự xoá hay tự vô hiệu hoá ghế F; ghế F giữ nguyên điểm và vị trí; và vòng đang chạy tiếp tục bình thường với đồng hồ không dừng.

**AC-114** — *US*: US-016 · *FR*: FR-111 · *GR*: `GR-036` C3 · `EVENT-029`
**Given** ghế F đã quá ngưỡng chờ và đang được tô nổi bật,
**When** admin mở phán quyết cho ghế F,
**Then** có đúng **hai** lựa chọn — **giữ** và **gia hạn**; cả hai đều không phá gì và không sinh sự kiện điểm; sau khi chọn, ghế F vẫn ở nguyên trong trận với nguyên điểm.

**AC-115** — *US*: US-016 · *FR*: FR-111 · *GR*: `GR-036` C9
**Given** ghế F mất kết nối được 40 giây, tức **chưa** hết ngưỡng 120 giây,
**When** admin gia hạn sớm cho ghế F, và ở một lần thử khác vô hiệu hoá sớm ghế F,
**Then** cả hai thao tác được chấp nhận — 120 giây là **khuyến nghị**, không phải ràng buộc cưỡng chế; không thao tác nào bị chặn vì chưa hết ngưỡng.

**AC-116** — *US*: US-016 · *FR*: FR-112 · *GR*: `EVENT-030` · §Invalid transitions
**Given** một trận **đã đóng sổ**,
**When** admin tìm cách vô hiệu hoá hoặc kích hoạt lại một ghế,
**Then** nút không bật; hệ thống phản hồi bằng toast invalid state; trạng thái ghế và bảng điểm không đổi.

**AC-117** — *US*: US-016 · *FR*: FR-112 · *GR*: `EVENT-030`
**Given** ghế E **đang bị vô hiệu hoá**,
**When** admin bấm vô hiệu hoá ghế E một lần nữa,
**Then** thao tác là **no-op** — trạng thái ghế không đổi và không sự kiện nào được sinh; hành vi tương tự khi kích hoạt lại một ghế đang hoạt động bình thường.

**AC-118** — *US*: US-016 · *FR*: FR-113 · *GR*: `GR-036` §Điều kiện
**Given** một trận đang chạy với một ghế thí sinh đã xác thực,
**When** admin duyệt toàn bộ các thao tác khả dụng trên ghế đó,
**Then** **không tồn tại** thao tác kick ghế thí sinh ở v1; công cụ duy nhất là vô hiệu hoá / kích hoạt lại, và nó đảo ngược được.

**AC-119** — *US*: US-016 · *FR*: FR-108 · *GR*: `GR-036` §Điều kiện
**Given** ghế F đang mất kết nối và đã quá ngưỡng chờ,
**When** một khoảng thời gian bất kỳ trôi qua mà admin không bấm gì,
**Then** ghế F **không** tự bị vô hiệu hoá — vô hiệu hoá là một quyết định riêng đi qua **nút khác**; hai việc không dây dưa vào nhau; và trạng thái quyền thao tác của ghế F không đổi.

#### Nhóm L — Lớp phủ do admin điều khiển (US-017)

**AC-120** — *US*: US-017 · *FR*: FR-114, FR-116 · *GR*: `T-087`, `T-088` · `INV-021`
**Given** một vòng đang chạy với một câu đã chấm xong, đang chờ chuyển câu, và **không** đồng hồ nào đếm,
**When** admin mở lớp công bố kết quả rồi đóng nó,
**Then** lớp công bố bật lên chồng trên trạng thái đang chạy mà **không huỷ nó**; sau khi đóng, vòng tiếp tục **đúng chỗ cũ** với đúng câu và đúng trạng thái; **không** sự kiện điểm nào được sinh; và trạng thái trận, vòng, câu, ghế, tín hiệu không đổi.

**AC-121** — *US*: US-017 · *FR*: FR-114 · *GR*: `EVENT-032` · `STATE-033`
**Given** một vòng vừa kết thúc và trận về trạng thái nghỉ,
**When** hệ thống nhận diện mốc đó,
**Then** hệ thống **gợi ý** mở lớp công bố nhưng **không tự mở**; admin vẫn mở được ở bất kỳ trạng thái cấp trận nào khác, kể cả khi không có gợi ý.

**AC-122** — *US*: US-017 · *FR*: FR-115 · *GR*: `EVENT-033`, `EVENT-035` · `STATE-033`, `STATE-040`
**Given** lớp công bố kết quả đang bật, và ở một lần thử khác banner tạm dừng đang bật,
**When** một khoảng thời gian dài trôi qua mà admin không bấm gì,
**Then** **không** lớp nào tự đóng — không có bộ đếm tự đóng; mốc đóng duy nhất là một cú bấm của admin.

**AC-123** — *US*: US-017 · *FR*: FR-117 · *GR*: `GR-035` · `T-089`
**Given** một câu đang mở với **đồng hồ đang đếm**,
**When** admin tìm cách mở banner tạm dừng,
**Then** nút mở banner **không bật** và hệ thống hiện toast; banner không bật lên; đồng hồ tiếp tục chạy bình thường.

**AC-124** — *US*: US-001, US-017 · *FR*: FR-010, FR-118 · *GR*: `GR-035` · §Invalid transitions
**Given** banner tạm dừng **đang bật** và một câu đã hiển thị,
**When** admin bấm start timer, rồi đóng banner và bấm lại,
**Then** lần đầu nút không bật và hệ thống hiện toast; sau khi đóng banner, nút **sống lại** và start timer thực thi bình thường — hai chiều của ràng buộc loại trừ đều được cưỡng chế.

**AC-125** — *US*: US-017 · *FR*: FR-118, FR-119 · *GR*: `STATE-040` §Bốn ràng buộc suy ra
**Given** trận đang ở trạng thái nghỉ, không đồng hồ nào đếm,
**When** admin bấm mở banner tạm dừng,
**Then** banner bật **không qua dialog xác nhận** vì thao tác đảo ngược được; banner **không chữ** — không lý do, không tên người bấm, không đồng hồ; banner **không phủ** màn admin; máy thí sinh bị chặn toàn bộ và server cưỡng chế việc chặn đó chứ không chỉ giao diện; và thao tác vào nhật ký.

**AC-126** — *US*: US-017 · *FR*: FR-116 · *GR*: `INV-021`
**Given** một vòng đang chạy, lớp công bố kết quả **và** banner tạm dừng cùng bật,
**When** admin đóng lần lượt từng lớp,
**Then** hai lớp bật cùng lúc là hợp lệ; đóng từng lớp để trạng thái bên dưới lộ lại **nguyên vẹn**; và không lớp nào đã đổi trạng thái trận, vòng, câu, ghế, tín hiệu, đồng hồ, hàng đợi hay quyền thao tác.

#### Nhóm M — Nút một chiều và khoá theo luật chơi (US-018)

**AC-127** — *US*: US-010, US-018 · *FR*: FR-120, FR-123, FR-130 · *GR*: `CLAUDE.md` §UX
**Given** một nút hành động trên màn admin và mạng đang chậm,
**When** admin bấm nút đó hai lần liên tiếp trước khi phản hồi đầu tiên về,
**Then** nút chỉ hiện trạng thái **đang xử lý** chứ **không** bị vô hiệu hoá để chống trùng; người dùng gửi lại được; server nhận bản cuối và tự chống trùng; và một phản hồi thành công hoặc thất bại hiện sau mỗi lần.

**AC-128** — *US*: US-018 · *FR*: FR-121 · *GR*: `GR-034` C5
**Given** một ghế đã bấm chuông và bị chấm Sai ở câu hiện tại, nên chuông của ghế đó bị khoá theo luật chơi,
**When** admin xem trạng thái nút của ghế đó trên màn điều khiển,
**Then** nút hiện ở trạng thái **vô hiệu hoá thật**, khác với trạng thái đang xử lý; và hệ thống đánh dấu rõ đây là khoá **theo luật chơi**, không phải chống gửi trùng.

**AC-129** — *US*: US-018 · *FR*: FR-122, FR-123 · *GR*: `CLAUDE.md` §UX
**Given** bốn nút một chiều của admin — chấm, start timer, chuyển câu, duyệt tín hiệu,
**When** kiểm tra cách bốn nút này được dựng,
**Then** cả bốn dùng **một thành phần dùng chung**; thành phần đó mang ghi chú phân loại giữa *khoá chống bấm trùng* và *khoá theo luật chơi*; và dialog xác nhận được dùng đúng cho các thao tác không thu hồi được chứ không phải như một cơ chế chống gửi trùng.

**AC-130** — *US*: US-018 · *FR*: FR-121, FR-130 · *GR*: `GR-034` C5, `GR-006`
**Given** ba tình huống khoá theo luật chơi cùng tồn tại — một ghế đã dùng Ngôi sao hy vọng, một ghế chưa tới lượt, và một ghế đã bị loại khỏi vòng Vượt chướng ngại vật,
**When** admin xem màn điều khiển,
**Then** cả ba trạng thái khoá hiện rõ trên màn admin kèm lý do luật tương ứng; không trạng thái nào bị trình bày như trạng thái đang xử lý; và mọi thao tác async khác vẫn có chỉ báo trạng thái riêng.

#### Nhóm N — Cảnh báo vỡ bộ Vượt chướng ngại vật (US-019)

**AC-131** — *US*: US-019 · *FR*: FR-124 · *GR*: `GR-031` C3c · `TERM-058`
**Given** kho đề của contest có đúng một bộ Vượt chướng ngại vật nguyên vẹn — 1 Chướng ngại vật, 4 hàng ngang, 1 câu ô trung tâm — và vòng Vượt chướng ngại vật chưa chạy,
**When** admin chỉ định **một hàng ngang** của bộ đó làm Câu hỏi phụ,
**Then** hệ thống hiện dialog cảnh báo nêu rõ thao tác **làm vỡ một bộ** và nêu **tên bộ** sẽ vỡ; nếu admin xác nhận thì bộ đó không còn dùng được cho vòng Vượt chướng ngại vật dù 5 thành phần kia còn nguyên, và cửa vào vòng đó sẽ chặn.

**AC-132** — *US*: US-019 · *FR*: FR-125 · *GR*: `GR-023` §Nguồn đề, `GR-031` C3c
**Given** một trận cần Câu hỏi phụ; kho Về đích và kho Khởi động **còn** câu khả dụng, kho Vượt chướng ngại vật cũng còn bộ nguyên vẹn,
**When** hệ thống chạy phép rút tự động cho Câu hỏi phụ,
**Then** phép rút lấy câu từ kho Về đích trước, rồi kho Khởi động, và **không chạm** kho Vượt chướng ngại vật khi hai kho kia còn câu; không bộ nào bị vỡ.

#### Nhóm O — Ràng buộc xuyên suốt

**AC-133** — *US*: US-001 → US-019 · *FR*: FR-126 · *GR*: `GR-029` C1, `GR-037` §Điều kiện, `STATE-040` §Bốn ràng buộc suy ra, `EVENT-052` §Side effects
**Given** một trận đầy đủ trong đó admin đã dùng ít nhất một lần mỗi thao tác của feature này — bốn mốc bấm, phán quyết, chốt câu, điều chỉnh điểm, ba cửa ra, mở/đóng đáp án, đặt trạng thái ô chữ, duyệt và từ chối tín hiệu, kích hoạt tay, vô hiệu hoá ghế, mở/đóng hai lớp phủ,
**When** admin xuất nhật ký thao tác của trận đó,
**Then** **mỗi** thao tác có đúng một dòng kèm người bấm, hành động, đối tượng và mốc thời gian; **0** thao tác thiếu dấu vết — kể cả banner tạm dừng, thứ không hiển thị lý do ra ngoài; và không dòng nào sửa được.

**AC-134** — *US*: US-004, US-013, US-015 · *FR*: FR-127 · *GR*: `CLAUDE.md` §UX Bố cục theo viewport
**Given** ba màn điều khiển có nhiều dữ liệu nhất — màn chấm Tăng tốc bốn ghế, màn hàng đợi có lịch sử dài, màn danh sách câu của một contest lớn,
**When** mở từng màn ở khung nhìn tham chiếu của dự án,
**Then** nội dung chính của mỗi màn nằm trọn trong một khung nhìn; **0** phần tử thao tác bắt buộc nằm dưới mép dưới khi trang vừa tải; và bảng dài dùng phân trang với số dòng theo khung nhìn.

**AC-135** — *US*: US-007, US-008 · *FR*: FR-128 · *GR*: `CLAUDE.md` §Quy ước code
**Given** admin nhập lý do `"  sửa nhầm câu 2  "` cho một điều chỉnh điểm, và ở lần khác nhập một lý do chỉ gồm khoảng trắng cho một cửa ra hạng phá huỷ,
**When** admin xác nhận từng thao tác,
**Then** lý do thứ nhất được lưu là `"sửa nhầm câu 2"` — cắt hai đầu ở cả giao diện lẫn server; lý do thứ hai bị coi là **trống** và thao tác bị từ chối ở cả hai đầu.

**AC-136** — *US*: US-001 → US-019 · *FR*: FR-129 · *GR*: `CLAUDE.md` §Quy ước code
**Given** một trận chạy vào buổi tối theo giờ UTC+7,
**When** admin xem mọi màn điều khiển và xuất nhật ký thao tác,
**Then** mọi chuỗi giao diện là tiếng Việt; mọi mốc thời gian hiển thị và mọi dòng nhật ký format theo **UTC+7**; và không có thiết lập múi giờ theo người dùng nào tồn tại.

## Assumptions

Ba mục dưới đây là **mặc định hợp lý** cho chi tiết mà `docs/` không quy định và việc đoán sai **không** đổi kết quả trận — ghi lại theo Nguyên tắc III của `.specify/memory/constitution.md`. Mọi thứ ảnh hưởng tới luật chơi, quyền hạn, ngưỡng thời gian hay quy tắc phân xử đều **không** nằm ở đây mà ở §Open Questions.

- **Trình bày của gợi ý so khớp**: `GR-027` quy định *"tô ký tự khác"* nhưng không quy định màu, kiểu chữ hay cách đánh dấu cụ thể — trừ **màu đỏ** cho bản quá hạn, thứ được nêu thẳng. Spec này giả định cách đánh dấu còn lại là quyết định thiết kế, miễn nó phân biệt được với dấu đỏ của bản quá hạn.
- **Vị trí đặt các nút trên màn điều khiển**: `docs/` chỉ ràng buộc **hai** điều — Tăng tốc chấm trên một màn (`PRD-REQ-053`), và nút **Huỷ trận** không đặt cạnh nút bỏ vòng (`EVENT-008`). Bố cục còn lại là quyết định thiết kế trong khuôn khổ `CLAUDE.md` §UX.
- **Ngưỡng hiển thị của danh sách lịch sử tín hiệu**: `GR-032` đòi lịch sử **không bao giờ xoá** nhưng không nêu bao nhiêu mục hiện cùng lúc. Spec này giả định phân trang theo khung nhìn như mọi bảng dài khác (FR-127), miễn không mục nào bị mất khỏi lịch sử.
