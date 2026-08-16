# Feature Specification: Trải nghiệm thí sinh

**Feature Branch**: `008-trai-nghiem-thi-sinh`

**Created**: 2026-08-03

**Status**: Draft

**Input**: EPIC-008 — Trải nghiệm thí sinh (`docs/PRD.md` §11 dòng 483-494, §12 mục EPIC-008)

**Nguồn**: `docs/PRD.md` (EPIC-008 §11; PRD-REQ-064 → PRD-REQ-072 §12 mục EPIC-008; PRD-REQ-015, PRD-REQ-046, PRD-REQ-049, PRD-REQ-050 — các requirement khai **Related epic** gồm EPIC-008; §9.1 → §9.5; JOURNEY-004, JOURNEY-005, JOURNEY-006 §10; §14 FS-20, FS-28, FS-29, FS-30; §15.8 NFR-36 → NFR-39; ma trận truy nguyên §22) · `docs/game-rules.md` (`GR-034`, `GR-036`, `GR-006`, `GR-015` — dải EPIC-008 khai; và các rule mà PRD-REQ của epic này tham chiếu: `GR-003`, `GR-007`, `GR-008`, `GR-009`, `GR-013`, `GR-017`, `GR-020`, `GR-021`, `GR-027`, `GR-028`, `GR-032`, `GR-037`; bốn bảng dùng chung; 23 nguyên tắc nền) · `docs/game-state-machine.md` (`STATE-017` → `STATE-028`, `STATE-029` → `STATE-032`, `STATE-034`, `STATE-038`, `STATE-040`; `EVENT-037` → `EVENT-042`, `EVENT-045`, `EVENT-046`; §Invalid transitions; `INV-001` → `INV-022`) · `docs/decisions.md` (`QĐ-004`, `QĐ-005`, `QĐ-012`, `QĐ-015`, `QĐ-019`, `QĐ-023`, `QĐ-029`, `QĐ-045`, `QĐ-046`, `QĐ-059`, `QĐ-060`, `QĐ-072`, `QĐ-080`, `QĐ-105`, `QĐ-109`, `QĐ-110`) · `docs/glossary.md` (`TERM-001`, `TERM-025`, `TERM-027`, `TERM-037`, `TERM-043`, `TERM-053`) · `docs/traceability.md` · `.specify/memory/constitution.md` bản 1.4.0 · `CLAUDE.md` §UX, §Quy ước code.

**Lưu ý về đầu vào**: lệnh gọi yêu cầu đọc `docs/rule-traceability.md` và `docs/reviews/prd-review.md` — **cả hai đường dẫn không tồn tại** trong repo. Tương ứng gần nhất là `docs/traceability.md` (đã dùng); không có tài liệu nào thay thế `docs/reviews/prd-review.md`, và theo `.specify/memory/constitution.md` §*Nguồn đã migrate xong* thì `docs/reviews/**` **không thoả cổng truy nguyên** nên dù có cũng không dùng làm nguồn requirement. Đây là chênh **đường dẫn đầu vào**, không phải `CONFLICT` nghiệp vụ, nên không ghi vào §Open Questions. Cùng cách xử lý đã dùng ở `specs/005`, `specs/006` và `specs/007`.

**Quy ước truy nguyên của feature này** — bốn điểm cần biết trước khi đọc bảng truy nguyên:

1. **Requirement khai nhiều epic vẫn thuộc epic này.** PRD-REQ-015, 046, 049, 050, 066, 069, 070, 072 đều khai `EPIC-008` trong trường **Related epic**. Spec này chỉ đặc tả **mặt máy thí sinh** của những requirement đó; **luật số học, mô hình điểm và hệ quả phía engine** thuộc `specs/006`, còn **bề mặt bấm của admin** thuộc `specs/007` — cả hai **không** đặc tả lại ở đây (xem §Out of Scope).
2. **`docs/decisions.md` là nguồn hợp lệ và được dùng.** `CLAUDE.md` §1 khai nó là *"nơi duy nhất ghi vì sao"* và nó nằm trong `docs/`, nên nó thoả Cổng 1 của constitution.
3. **Một câu có ĐÚNG MỘT mốc nhận bài: `hạn chót`.** Nút gửi phía thí sinh tắt **đúng tại `hạn chót`** ở **mọi** vòng, và **không** vòng nào có thêm một mốc cắt thứ hai (`QĐ-109`, `QĐ-110`). Spec này dùng chữ **`hạn chót`** ở mọi FR để khỏi tái tạo nhiều tên gọi cho một mốc. Ngoại lệ **duy nhất** là **người cướp quyền Về đích** — ca này không có `hạn chót` riêng, mốc đóng của nó là **cú bấm chấm của admin** (FR-021a).
4. **Mode trả lời là cấu hình cấp CONTEST, không phải requirement của spec này.** Ở chỗ một hành vi rẽ nhánh theo *mode sân khấu* / *mode nhập liệu*, spec này phát biểu **hành vi của máy thí sinh trong từng nhánh**; việc **đặt** giá trị đó thuộc EPIC-004.

## Phạm vi

EPIC-008 phủ **toàn bộ bề mặt mà một thí sinh chạm vào trong một trận đang chạy**: nút chuông chỉ nhận click chuột và tự khoá trong lần bấm đầu, nút gửi đáp án sống tới hạn chót với ngữ nghĩa bản-cuối-thắng, ô nhập Tăng tốc không khoá sau lần trả lời đầu, bảng điểm realtime của **tất cả** các ghế gồm cả điểm âm, ba trạng thái của nút chọn hàng ngang ở mode nhập liệu, một-đường-vào-cho-mỗi-mode của chọn hàng ngang · Ngôi sao hy vọng · chọn gói câu Về đích, ranh giới đáp án không bao giờ tới máy thí sinh trước mốc câu khép, khôi phục sau mất kết nối giữa câu, và hai quy ước giao diện xuyên suốt — không render control ở trạng thái không hợp lệ, và không chặn gửi lại trừ khoá theo luật chơi.

Actor chính: **ACTOR-003 thí sinh**. Journey: **JOURNEY-004 — Vào phòng**, **JOURNEY-005 — Thi đấu**, **JOURNEY-006 — Xử lý sự cố** *(nhánh mất kết nối)*.

**Mục tiêu của epic** (`docs/PRD.md` §11): *"Thí sinh thi được nhanh, công bằng, và luôn biết mình đang ở đâu"*, với **user value** là *"phản hồi tức thì và đủ bối cảnh — bảng điểm của tất cả các ghế, không chỉ điểm của mình"*.

**Không thuộc phạm vi feature này** (xem §Out of Scope): luật số học của từng vòng, thang xếp hạng, cửa sổ chuông và mô hình điểm (EPIC-006); màn điều khiển của admin, màn chấm, hàng đợi và mọi cú bấm phán quyết (EPIC-007); vòng đời cấp trận (EPIC-005); màn khán giả, lớp phủ dựng stream và màn MC (EPIC-009); xác thực, mã phòng 6 số và catalog permission (EPIC-001); kho đề (EPIC-002); contest builder và việc đặt mode trả lời (EPIC-004); biên bản sau trận (EPIC-010).

## User Scenarios & Testing *(mandatory)*

### US-001 — Chuông chỉ nhận click chuột và tự khoá trong lần bấm đầu (Priority: P1)

- **Title**: Không tồn tại phím nào phát được tín hiệu chuông
- **Actor**: ACTOR-003 thí sinh
- **Intent**: Giành quyền trả lời bằng đúng một cú click chuột, và không bao giờ phát nhầm tín hiệu vì một phím lỡ tay trong lúc đang gõ đáp án.
- **User value**: Thí sinh gõ đáp án bằng cả hai tay mà không phải sợ chạm nhầm; và một cú click run tay không biến thành hai tín hiệu.
- **Priority**: P1
- **Priority rationale**: Chuông là **đường giành quyền duy nhất** ở ba vòng và là đường **duy nhất** mở Chướng ngại vật. Gán một phím tắt cho nó — kể cả rồi bỏ qua — biến mọi ô nhập đáp án thành một cái bẫy; và khoá muộn *(sau khi gửi thay vì trước khi gửi)* cho phép một ghế phát hai tín hiệu cho cùng một câu.
- **Independent test**: Ở một câu Khởi động lượt chung đang mở cửa sổ chuông, thử lần lượt mọi phím và tổ hợp phím thông dụng và xác nhận **không tín hiệu nào** được tạo; rồi click chuột một lần và xác nhận nút chuyển sang khoá **trước** khi yêu cầu rời máy; click lần thứ hai và xác nhận không có tín hiệu thứ hai. Sang câu kế và xác nhận nút đã mở lại.
- **Related PRD requirements**: PRD-REQ-064
- **Related game rules**: `GR-034` *(toàn bộ bảng)*, `GR-003` C5, C6, `GR-009` §Điều kiện, C9, C10, `GR-032` §Bấm trùng · `INV-005`, `INV-006`
- **Related journey**: JOURNEY-005

---

### US-002 — Nút gửi đáp án sống tới hạn chót, bản cuối thắng (Priority: P1)

- **Title**: Nút chuông và nút gửi có vòng đời NGƯỢC nhau
- **Actor**: ACTOR-003 thí sinh
- **Intent**: Sửa và gửi lại đáp án bao nhiêu lần cũng được cho tới hạn chót của câu, và biết chắc bản cuối cùng là bản được tính.
- **User value**: Một lỗi gõ phát hiện ở giây thứ hai không còn là một câu mất điểm; và thí sinh không phải cân nhắc *"gửi bây giờ hay chờ thêm"* như một canh bạc.
- **Priority**: P1
- **Priority rationale**: Hai nút này dễ bị cài chung một mẫu *"bấm xong thì tắt"*, và cài chung là **sai ở cả hai đầu** — hoặc chuông không khoá *(một ghế phát nhiều tín hiệu)*, hoặc nút gửi khoá *(thí sinh mất quyền sửa mà luật cho phép)*.
- **Independent test**: Ở một câu Khởi động mode nhập liệu, gửi ba bản khác nhau trong cửa sổ thời gian; xác nhận nút gửi **không** khoá sau lần nào, bản **thứ ba** là bản được ghi nhận, và cả ba bản vẫn xem lại được trong lịch sử. Sau đó gửi một bản chỉ gồm khoảng trắng và xác nhận bản được ghi nhận **không đổi**.
- **Related PRD requirements**: PRD-REQ-065, PRD-REQ-015
- **Related game rules**: `GR-006` *(toàn bộ bảng)*, `GR-034` §Bấm trùng, `GR-027` §Điều kiện · `QĐ-109`, `QĐ-110` · `INV-001`
- **Related journey**: JOURNEY-005

---

### US-003 — Tăng tốc: gửi lại không bao giờ tự làm xấu thứ hạng (Priority: P1)

- **Title**: Ô nhập không khoá sau lần trả lời đầu; bản y hệt không dời mốc
- **Actor**: ACTOR-003 thí sinh
- **Intent**: Ở vòng xếp hạng theo tốc độ, tiếp tục sửa đáp án tới khi hết giờ mà không sợ mỗi lần bấm gửi là một lần tự đẩy lùi thứ hạng của mình.
- **User value**: Thí sinh dùng trọn thời gian được cho mà không bị phạt vì đã dùng nó; và một cú bấm gửi lặp lại vì lo lắng không đổi kết quả gì.
- **Priority**: P1
- **Priority rationale**: Ở vòng này **mốc chính là kết quả**. Nếu một bản trùng nội dung cũng cập nhật mốc thì thí sinh **tự làm xấu thứ hạng của mình bằng một thao tác vô nghĩa** — một hình phạt mà nguồn không có.
- **Independent test**: Ở một câu Tăng tốc, gửi `"Hà Nội"`, rồi `"Hanoi"`, rồi `"Hà Nội"` lần nữa; xác nhận bản được ghi nhận là `"Hà Nội"` và **mốc xếp hạng vẫn là mốc của lần gửi ĐẦU** khai nội dung đó. Lặp lại với bản cuối là một nội dung mới và xác nhận mốc **có** dời sang lần gửi cuối.
- **Related PRD requirements**: PRD-REQ-066
- **Related game rules**: `GR-015` *(toàn bộ bảng)*, `GR-013` §Điều kiện, `GR-014` §Thứ tự đánh giá · `TERM-037`
- **Related journey**: JOURNEY-005

---

### US-004 — Bảng điểm realtime của TẤT CẢ các ghế trên máy thí sinh (Priority: P1)

- **Title**: Điểm là thông tin công khai — thí sinh thấy đúng thứ khán giả thấy
- **Actor**: ACTOR-003 thí sinh
- **Intent**: Nhìn thấy điểm của mọi ghế, cập nhật ngay khi nó đổi, để biết mình đang đứng đâu trước khi quyết định có bấm chuông hay có đặt Ngôi sao hy vọng.
- **User value**: Những quyết định chiến thuật mà luật cố ý trao cho thí sinh — cướp quyền, đặt cược, chọn gói — chỉ có nghĩa khi thí sinh biết khoảng cách điểm.
- **Priority**: P1
- **Priority rationale**: `product-discovery.md` §6 xếp đây là *"chỗ dễ cài thiếu nhất"*. Ba loại thông tin có **ba** chế độ khác nhau (`GR-037` §bảng) và rất dễ bị gộp thành một: đáp án **mật tới mốc câu khép**, bài làm của ghế khác **ẩn tạm thời**, còn điểm **công khai luôn luôn**. Siết nhầm điểm theo chế độ của đáp án là làm hỏng vòng chơi mà không ai báo lỗi.
- **Independent test**: Mở một màn thí sinh và một màn khán giả cạnh nhau; cho admin chấm một câu và xác nhận **cả bốn ghế** đổi điểm trên máy thí sinh cùng lúc với màn khán giả. Sau đó cho admin bỏ một vòng và xác nhận máy thí sinh hiện điểm **tụt** đúng theo, gồm cả trường hợp điểm xuống dưới 0.
- **Related PRD requirements**: PRD-REQ-067
- **Related game rules**: `GR-028` §Điều kiện, §Biên, `GR-037` §bảng ba loại thông tin, `GR-008` §Điều kiện *(bài làm của ghế khác)* · `INV-002`, `INV-017`, `INV-018` · `QĐ-012`, `QĐ-015`
- **Related journey**: JOURNEY-005

---

### US-005 — Thao tác đua tốc độ: tức thời, không dialog, không rút lại (Priority: P1)

- **Title**: Thí sinh cần tốc độ, và tự chịu trách nhiệm sai lầm của mình
- **Actor**: ACTOR-003 thí sinh
- **Intent**: Bấm chuông, bấm *"Mở chướng ngại vật"* và gửi đáp án mà không có một hộp thoại nào chen giữa ý định và tín hiệu.
- **User value**: Không mất phần nghìn giây nào cho một cú xác nhận ở đúng những chỗ mà phần nghìn giây quyết định ai giành được quyền.
- **Priority**: P1
- **Priority rationale**: Thêm một dialog vào đường chuông là **đổi luật chơi** mà không ai khai — nó dời mốc server timestamp của mọi tín hiệu đi một lượng phụ thuộc tốc độ tay người. Đường sửa lỗi bấm nhầm của thí sinh đã được nguồn chỉ định rõ ràng ở chỗ khác: **admin bấm No** trong hàng đợi, không phải một nút huỷ.
- **Independent test**: Rà toàn bộ màn thi đấu của thí sinh ở cả hai mode và xác nhận **không** đường nào trong ba đường trên có dialog, và **không** nút huỷ hay rút lại nào tồn tại. Xác nhận ngoại lệ duy nhất — chọn hàng ngang ở mode nhập liệu — **có** dialog.
- **Related PRD requirements**: PRD-REQ-068
- **Related game rules**: `GR-034` §Điều kiện, `GR-009` §Điều kiện, `GR-007` §Điều kiện, C7, `GR-032` §Điều kiện · `TERM-001` · `QĐ-005`, `QĐ-019`
- **Related journey**: JOURNEY-005

---

### US-006 — Chọn hàng ngang: một đường vào cho mỗi mode, nút có BA trạng thái (Priority: P1)

- **Title**: Trạng thái thứ ba — mở lại khi admin bấm No — là chỗ dễ bỏ sót nhất
- **Actor**: ACTOR-003 thí sinh
- **Intent**: Ở mode nhập liệu, chọn hàng ngang của mình qua một dialog xác nhận, chờ admin duyệt, và nếu bị từ chối thì chọn lại được ngay mà **không** mất lượt.
- **User value**: Một cú click nhầm không lấy mất lượt chọn duy nhất của thí sinh; và không bao giờ có hai người cùng tranh một thao tác mà không có trọng tài.
- **Priority**: P1
- **Priority rationale**: Hai đường vào song song cho cùng một thao tác tạo tranh chấp không có trọng tài. Trạng thái thứ ba dễ bị bỏ sót, và bỏ sót là **thí sinh mất lượt trái luật** — hậu quả nặng nhất mà một lỗi giao diện gây ra được ở vòng này.
- **Independent test**: Ở một trận mode nhập liệu đang ở Vượt chướng ngại vật, cho ghế tới lượt click một hàng ngang; xác nhận dialog hiện, xác nhận xong thì nút **khoá**, và đúng **một** tín hiệu rời máy. Cho admin bấm **No** và xác nhận nút **mở lại**, ghế chọn được hàng khác, ô chữ **không** đổi trạng thái và đồng hồ **chưa** chạy. Chuyển sang một trận mode sân khấu và xác nhận máy thí sinh **không render** nút chọn nào.
- **Related PRD requirements**: PRD-REQ-069, PRD-REQ-046
- **Related game rules**: `GR-007` C2, C5, C6, C7, C8, §Thứ tự đánh giá, `GR-032` C1, C3, `GR-009` C8, C14, `GR-010` §Cấm · `STATE-027`, `STATE-029`, `STATE-031`, `STATE-038`, `EVENT-039` · `INV-008`
- **Related journey**: JOURNEY-005

---

### US-007 — Đặt Ngôi sao hy vọng theo đúng một đường vào của mode (Priority: P1)

- **Title**: Cửa sổ đóng tại mốc hiển thị câu; ngoài cửa sổ thì ngôi sao vẫn chưa dùng
- **Actor**: ACTOR-003 thí sinh
- **Intent**: Ở mode nhập liệu, đặt cược lên câu của chính mình trước khi câu được mở, và biết chắc thao tác đó có hiệu lực ngay chứ không chờ ai duyệt.
- **User value**: Quyết định đặt cược — thứ đắt nhất mà luật trao cho thí sinh — không bị trượt mất vì một khâu duyệt hay một cửa sổ mơ hồ.
- **Priority**: P1
- **Priority rationale**: Đây là **cờ một chiều** ảnh hưởng trực tiếp tới điểm. Nếu nút vẫn render sau mốc đóng cửa sổ thì thí sinh tưởng đã đặt được mà thực ra chưa — và ngược lại, nếu tín hiệu phải chờ duyệt thì Về đích *(vòng hàng đợi **không** chặn)* bị áp nhầm ngữ nghĩa của Vượt chướng ngại vật.
- **Independent test**: Ở một trận mode nhập liệu đang ở Về đích, khi câu đã rút mà **chưa** hiển thị, cho ghế đang thi bấm Ngôi sao hy vọng; xác nhận cờ có hiệu lực **ngay** và nút chuyển sang disabled. Cho admin bấm hiển thị câu, rồi thử bấm lại và xác nhận nút **không còn render**. Chuyển sang trận mode sân khấu và xác nhận máy thí sinh **không có** nút này, và một tín hiệu gửi thẳng tới server bị **từ chối** kèm dòng nhật ký.
- **Related PRD requirements**: PRD-REQ-069 *(vế Ngôi sao hy vọng)*
- **Related game rules**: `GR-021` C3, C6, C7, C8, §Thứ tự đánh giá, §Bấm trùng · `STATE-026`, `EVENT-042` · `INV-005`
- **Related journey**: JOURNEY-005

---

### US-008 — Chọn gói câu Về đích: một đường vào theo mode, có fallback của admin (Priority: P1)

- **Title**: Ngoại lệ có chủ ý — chọn gói giữ fallback, chọn hàng ngang thì cấm tuyệt đối
- **Actor**: ACTOR-003 thí sinh
- **Intent**: Ở mode nhập liệu, dựng gói ba câu của mình từ hai mức điểm, đổi ý bao nhiêu lần cũng được cho tới khi câu đầu tiên được mở.
- **User value**: Thí sinh cân nhắc mức mạo hiểm theo khoảng cách điểm ngay trước lượt của mình, thay vì phải chốt sớm rồi tiếc.
- **Priority**: P1
- **Priority rationale**: Đây là ngoại lệ **có chủ ý** so với chọn hàng ngang, và nguồn nói thẳng lý do: lượt thi cần một đường thoát để **không tắc**, còn chọn hàng ngang thì không có mốc *"quá hạn"* tương đương. Cài nhầm theo mẫu của chọn hàng ngang sẽ làm một lượt Về đích kẹt vô hạn khi thí sinh im lặng.
- **Independent test**: Ở một trận mode nhập liệu tới lượt một ghế, chọn `20/20/20` rồi đổi sang `20/30/30`; xác nhận **last-wins**. Cho admin bấm hiển thị câu đầu tiên và xác nhận nút chọn đã **khoá**. Chuyển sang trận mode sân khấu và xác nhận máy thí sinh **không render** nút chọn gói, và server **từ chối** một tín hiệu chọn gói gửi thẳng tới.
- **Related PRD requirements**: PRD-REQ-070
- **Related game rules**: `GR-017` C1, C2, C3, C5, C6, C7, C8, §Bấm trùng · `STATE-012`, `EVENT-041`
- **Related journey**: JOURNEY-005

---

### US-009 — Đáp án chỉ tới máy thí sinh từ mốc CÂU KHÉP (Priority: P1)

- **Title**: Hàng rào chống rò đề, nhìn từ phía nhận
- **Actor**: ACTOR-003 thí sinh
- **Intent**: Thấy đáp án đúng ngay sau khi câu khép — nếu trận bật cờ công bố — và **không** bao giờ thấy nó sớm hơn một mili-giây nào.
- **User value**: Thí sinh học được ngay tại chỗ mình sai gì; và không ai thắng một câu bằng cách đọc thứ lẽ ra chưa được gửi xuống.
- **Priority**: P1
- **Priority rationale**: Đây là **hàng rào chống rò đề**, và sai ở đây là **hỏng sản phẩm** chứ không phải một lỗi giao diện. Nguồn nêu đích danh lỗi phải tránh: tiền lệ Athena **đẩy đáp án xuống mọi máy client rồi ẩn bằng một cờ hiển thị**. Ở Về đích, công bố sớm còn xoá sổ trọn cơ chế cướp quyền.
- **Independent test**: Bắt gói tin trên kênh của một máy thí sinh trong trọn một câu Về đích có chấm Sai người thi chính; xác nhận **không byte nào** mang đáp án rời server trong lúc cửa sổ cướp quyền còn mở, và đáp án chỉ tới **sau khi** người cướp đã được chấm. Lặp với trận tắt cờ công bố và xác nhận đáp án **không bao giờ** tới.
- **Related PRD requirements**: PRD-REQ-049
- **Related game rules**: `GR-037` C3, C4, C5, C6, C7, C8, C8b, C9, §Cấm, `GR-008` §Điều kiện, C9, `GR-020` §Cấm · `INV-017` · `QĐ-062`, `QĐ-080`
- **Related journey**: JOURNEY-005

---

### US-010 — Khôi phục sau mất kết nối giữa một câu (Priority: P1)

- **Title**: Quay lại thấy đúng màn và đúng thời gian còn lại — không mua thêm giây nào
- **Actor**: ACTOR-003 thí sinh
- **Intent**: Rớt mạng giữa câu, kết nối lại, và thấy ngay câu đang mở với thời gian còn lại đúng theo đồng hồ server, cùng bản gửi gần nhất của chính mình.
- **User value**: Một sự cố mạng không biến thành một câu mất trắng; và thí sinh biết chuyện gì đang xảy ra nhờ banner thay vì nhìn một màn hình đóng băng.
- **Priority**: P1
- **Priority rationale**: Đây là lời giải trực tiếp cho `PS-5`. Hai chi tiết dễ cài sai và cả hai đều đổi kết quả trận: dựng lại đồng hồ từ *"còn N giây"* thay vì từ **hạn chót server** biến mất kết nối thành cách **mua thêm thời gian**; và nhét đáp án hay bài làm của ghế khác vào gói khôi phục biến nó thành một đường vòng lộ dữ liệu.
- **Independent test**: Mở một câu Vượt chướng ngại vật hạn `15` giây; ngắt kết nối một ghế ở giây thứ `4`, gửi lại ở giây thứ `9`; xác nhận ghế thấy còn **6** giây, thấy đúng câu đang mở, thấy bản gửi lúc giây `3` của chính mình, **không** thấy đáp án và **không** thấy bài làm của ghế khác; xác nhận banner *"đang kết nối lại"* đã hiện trong lúc mất và tắt khi nối lại; và xác nhận **không sự kiện nào** được sinh bởi thao tác khôi phục.
- **Related PRD requirements**: PRD-REQ-050
- **Related game rules**: `GR-036` C1, C8, C10, §Biên, §Nối lại nhiều lần, §Không đổi gì · `STATE-023`, `STATE-024`, `STATE-034`, `EVENT-045`, `EVENT-046` · `INV-016` · `QĐ-045`, `QĐ-046` · `TERM-043`
- **Related journey**: JOURNEY-006, JOURNEY-005

---

### US-011 — Máy thí sinh không render control ở trạng thái không hợp lệ; `Esc` không quay lại (Priority: P2)

- **Title**: Nhánh không tồn tại, và `Esc` thuộc về trình duyệt
- **Actor**: ACTOR-003 thí sinh
- **Intent**: Chỉ nhìn thấy những nút thật sự bấm được ở thời điểm hiện tại, và không bao giờ bị văng khỏi chế độ toàn màn hình giữa trận vì một phím quen tay.
- **User value**: Màn thi đấu không bày ra thứ gây hiểu nhầm, và không có đường nào rời trận bằng một cú gõ nhầm.
- **Priority**: P2
- **Priority rationale**: Đây là **quy ước giao diện xuyên suốt**, không phải một cơ chế luật — trận vẫn chạy đúng kết quả nếu cài sai, chỉ là thí sinh bị đánh lừa. Nhưng vế `Esc` có một hậu quả cụ thể: trình duyệt dùng `Esc` để thoát toàn màn hình, nên gán nó làm nút quay lại sẽ làm văng fullscreen giữa trận.
- **Independent test**: Ở màn thi đấu mode sân khấu, nhấn `Esc` và xác nhận **không** đổi màn, **không** rời trận, **không** thoát khỏi trận đấu. Chuyển sang mode nhập liệu, gõ vào ô nhập rồi nhấn `Esc` và xác nhận ô nhập **bị xoá** mà màn hình **không** đổi. Đặt trận vào một trạng thái mà ghế chưa tới lượt và xác nhận control tương ứng **không được render** thay vì được render rồi làm mờ.
- **Related PRD requirements**: PRD-REQ-071
- **Related game rules**: `GR-034` §Điều kiện *(các hotkey khác)*, C6, `GR-032` §Điều kiện, `GR-007` §Thứ tự đánh giá · §Invalid transitions *(hạng **KHÔNG CÓ TÍN HIỆU**)* · NFR-39 · `QĐ-004`, `QĐ-072`
- **Related journey**: JOURNEY-005

---

### US-012 — Không chặn gửi lại; nhưng khoá theo LUẬT CHƠI thì vẫn khoá (Priority: P2)

- **Title**: Hai loại nút mờ, và chỉ một loại là hợp lệ
- **Actor**: ACTOR-003 thí sinh
- **Intent**: Bấm lại một thao tác khi mạng có vẻ chậm, mà không bị giao diện chặn; đồng thời hiểu ngay vì sao một nút đang mờ khi luật chơi là thứ đang khoá nó.
- **User value**: Không đứng nhìn một nút mờ mà không biết là hệ thống hỏng hay là luật đang cấm.
- **Priority**: P2
- **Priority rationale**: Ghi rõ ranh giới để lần rà quy tắc giao diện sau **không ai gỡ nhầm khoá theo luật chơi**. Chống gửi trùng là việc của server *(nhật ký sự kiện)*; disable nút để chống gửi trùng là đẩy trách nhiệm sai chỗ và làm hỏng trải nghiệm khi mạng chậm.
- **Independent test**: Với một thao tác không phải chuông, bấm liên tiếp 10 lần trong 1 giây; xác nhận nút **chỉ** hiện trạng thái đang xử lý, **không** bị disable, và server chỉ ghi nhận một kết quả. Sau đó đặt ghế vào trạng thái *chuông đã khoá* và xác nhận nút **có** disable kèm dấu hiệu nêu rõ đó là khoá **theo luật chơi**.
- **Related PRD requirements**: PRD-REQ-072
- **Related game rules**: `GR-034` C5, `GR-006` §Điều kiện, `GR-032` §Bấm trùng · `STATE-026`, `STATE-027`, `STATE-028` · `CLAUDE.md` §UX · `QĐ-060`
- **Related journey**: JOURNEY-005

---

### Edge Cases

Liệt kê theo tám nhóm mà lệnh gọi yêu cầu. Mỗi mục ghi **hành vi đã có nguồn**; mục nào nguồn im lặng thì trỏ sang §Open Questions.

**Boundary cases**

- Cửa sổ chuông **đúng mốc** — `3` giây Khởi động lượt chung · `5` giây cướp quyền Về đích · `15` giây Câu hỏi phụ: biên **ĐÓNG**, click đúng mốc **vẫn tạo tín hiệu**; một mili-giây sau thì **không còn nút để bấm** (`GR-034` §Biên, `GR-032` §Biên, `INV-005`).
- Bản gửi đến **đúng `hạn chót`**: được ghi nhận như bản hợp lệ — biên **đóng** (`GR-006` C3, `GR-008` C8).
- Nút gửi phía thí sinh tắt **đúng tại `hạn chót`** ở **mọi** vòng — không đâu có thêm một giây thao tác nào (`QĐ-109`).
- Ngưỡng chờ kết nối **đúng `120,000` giây** vẫn **trong** ngưỡng; quá mốc mới hết (`GR-036` §Biên).
- Gửi lại **trong cùng một mili-giây** ở Tăng tốc: nội dung khác ⇒ cập nhật mốc; y hệt ⇒ **không** cập nhật (`GR-015` §Biên).
- **0** lần gửi ở Tăng tốc: không có bản nào để chấm — *"không trả lời"* và *"trả lời sai"* là **cùng một thao tác** phía admin (`GR-015` §Biên).
- Hotkey `1`–`8` phủ tới 8 hàng ngang, nhưng v1 khoá cứng `rowCount = 4` nên chỉ `1`–`4` có mục tiêu; `5`–`8` **không** trỏ vào đâu và **không** sinh tín hiệu (`GR-034` §Điều kiện, `GR-009` C11).
- **Người cướp quyền Về đích không có `hạn chót` riêng**: `GR-018` §Thứ tự đánh giá đi thẳng từ *giành quyền* sang *admin chấm*, và `GR-032` §Kích hoạt tay nói rõ *"Về đích… **không có** đồng hồ trả lời riêng"*. Mốc đóng của ghế đó là **cú bấm chấm của admin**, theo quy tắc chung `GR-006` §Đồng thời — **không** đẻ thêm giá trị cấu hình nào (FR-021a).
- **Cửa sổ cướp quyền mở tại cú bấm Sai của admin, không tại `hạn chót`**: `GR-002` §Kích hoạt nói *"**Admin bấm Sai.** Đồng hồ hết giờ **không** tự sinh kết quả"*, và `GR-018` C3 xử *"không trả lời"* **y như** chấm Sai. Nên nút chuông của ba ghế còn lại sống lại tại một mốc **admin quyết định**, có thể cách `hạn chót` một khoảng bất kỳ (`QĐ-006`).
- Trận **dưới 4 thí sinh**: ghế bỏ thi được vô hiệu hoá **từ trước cú bấm bắt đầu trận**, máy ghế đó bị chặn toàn bộ và điểm **luôn 0**; nó vẫn hiện trên bảng điểm của mọi máy thí sinh (`GR-036` C5b, `QĐ-105`).

**Invalid state**

- Bấm chuông ở `STATE-018` của **Câu hỏi phụ** *(trước mốc start timer)*: nút **không hiển thị**, bấm **không phản hồi**, **và server cũng từ chối** — zero-trust (§Invalid transitions, `GR-024`, `GR-034`).
- Ghế **bị loại khỏi Vượt chướng ngại vật** thao tác bất kỳ trong vòng đó: máy thí sinh **không hiển thị gì** ⇒ không tín hiệu nào được tạo (`STATE-022`, `GR-010`, `GR-009` C9).
- Ghế **bị vô hiệu hoá** thao tác bất kỳ: máy bị chặn toàn bộ và **server enforce**; tín hiệu lỡ tới vào lịch sử với kết cục **TRƠ** — **không drop** (`STATE-025`, `GR-036` C5).
- **Banner tạm dừng đang bật**: banner **che toàn bộ màn thí sinh** *(không thấy bảng điểm, không thấy câu hỏi)* **và** mọi sự kiện của thí sinh bị chặn, **server enforce**, không tin client (`STATE-040`, `GR-035`, `QĐ-115`). Dữ liệu bên dưới vẫn chảy — **che ≠ đổi**; tắt banner là thấy trạng thái **hiện tại**.
- **Banner và đồng hồ LOẠI TRỪ LẪN NHAU**: `STATE-040` §Vào khai *"không có cửa sổ thời gian nào đang đếm"* là **điều kiện cứng**, và §Invalid transitions chặn cú mở banner khi đang có đồng hồ chạy. Nên tổ hợp *(banner đang bật × đồng hồ đang đếm)* **không dựng được**, và câu hỏi *"banner có đóng băng đồng hồ không"* **không có chủ ngữ** (`QĐ-050`).
- Trỏ vào một **hàng ngang đã mở**: nó không còn là mục tiêu chọn được ⇒ nút không render (`GR-007` C5).
- Máy thí sinh gửi tín hiệu **chọn hàng ngang** ở **mode sân khấu** · **chọn gói** ở mode sân khấu · **Ngôi sao hy vọng** ở mode sân khấu: cả ba đều là hạng **KHÔNG CÓ TÍN HIỆU**, và server **từ chối** nếu vẫn nhận được (§Invalid transitions, `GR-007`, `GR-017` C5, `GR-021` C8).
- Đặt Ngôi sao hy vọng từ `STATE-018` trở đi: cửa sổ **đã đóng**, nút không render, và ngôi sao **vẫn chưa dùng** (`GR-021` C6, `STATE-026`).
- Gửi đáp án **sau khi admin đã chấm**: nút gửi đã khoá lại (`EVENT-040` §Không hợp lệ ở, `GR-006` §Đồng thời).
- Bấm chuông khi trận đã ở `STATE-008` FINISHED: ghi lịch sử, **không có hiệu lực** (§Invalid transitions).

**Repeated action**

- Bấm chuông lần thứ hai trong **cùng một câu**: nút tự khoá **trước khi gửi** ⇒ không có cú bấm thứ hai; server vẫn phải bỏ qua nếu yêu cầu lọt tới (`GR-034` §Bấm trùng, `STATE-028`).
- Bấm *"Mở chướng ngại vật"* lần thứ hai trong **cùng một vòng**: được **nếu và chỉ nếu** tín hiệu trước đó **chưa** đi qua phán quyết Đúng/Sai — tức nó bị admin **từ chối** hoặc bị chấm **Huỷ kết quả**. Đã đi qua Đúng/Sai thì nút biến mất vì cờ *bị loại* hoặc vì vòng đã kết thúc, **không** vì một khoá riêng (`GR-009` C6, C6b, C8, C9, C10 · FR-004, FR-004a).
- Gửi đáp án nhiều lần: **không** phải bấm trùng mà là hành vi **được luật cho phép**; bản cuối thắng, lịch sử giữ nguyên (`GR-006` C2, `GR-015`).
- Gửi lại **nội dung y hệt**: vẫn là bản cuối; ở Khởi động không có hệ quả nào, ở Tăng tốc **không** dời mốc (`GR-006` C6, `GR-015` C1).
- Đặt Ngôi sao hy vọng lần thứ hai: nút đã disabled ngay tại lần đầu; server vẫn phải bỏ qua tín hiệu trùng (`GR-021` C3, §Bấm trùng).
- Chọn gói nhiều lần trước mốc khoá: **last-wins** — cùng ngữ nghĩa với nút gửi đáp án, **không** áp cơ chế *"bấm xong thì tắt"* (`GR-017` §Bấm trùng).
- Thí sinh click nhiều lần vào nút chọn hàng ngang ở mode nhập liệu: click đầu mở **dialog**; xác nhận xong **khoá nút** ⇒ **đúng một** tín hiệu rời máy (`GR-007` C7).

**Stale state**

- Ghế quay lại sau mất kết nối: đồng hồ **không đặt lại** — client dựng lại từ **hạn chót theo server time**, không phải từ *"còn N giây"* (`GR-036` C8, `QĐ-046`).
- **Ký tự đang gõ dở mà chưa gửi** không khôi phục được — nó chưa bao giờ tới server (`STATE-023` §Thứ không khôi phục được).
- Tín hiệu **vô hiệu khi đích của nó đóng**: chọn hàng ngang gắn với **lượt chọn**, trả lời gắn với **câu**, *"Mở chướng ngại vật"* gắn với **vòng** (`GR-032` §Vòng đời tín hiệu).
- Hàng đợi **đang hoạt động** đặt lại theo đích; **lịch sử tín hiệu không bao giờ xoá** (`GR-032` C5, `INV-001`).
- Bảng điểm trên máy thí sinh sau khi admin **bỏ vòng**: điểm tụt đột ngột là hành vi **đúng** — điểm là `reduce` của nhật ký, không phải một con số bị sửa tại chỗ (`GR-028`, `INV-002`).

**Duplicate event**

- Server MUST bỏ qua **mọi** tín hiệu chuông trùng, kể cả khi giao diện đã khoá — khoá phía giao diện chỉ là lớp thứ nhất (`GR-032` §Bấm trùng, `GR-034`).
- Server MUST bỏ qua tín hiệu Ngôi sao hy vọng trùng (`GR-021` §Bấm trùng).
- Thao tác **khôi phục kết nối** là phép **đọc**: nó MUST NOT sinh sự kiện nào, nên gọi lại bao nhiêu lần cũng không đổi trạng thái trận (`QĐ-046`, `STATE-023`).
- Gửi lại đúng chuỗi cũ ở Tăng tốc là **idempotent về mốc xếp hạng** — đây chính là điều `GR-015` bảo vệ.

**Partial failure**

- Ghế mất kết nối **giữa một vòng đang chạy**: vòng **chạy tiếp bình thường**; đồng hồ **không dừng** vì một ghế (`GR-036` C4, `INV-016`).
- Hai ghế cùng mất kết nối: **hai cửa sổ chờ độc lập** (`GR-036` C10).
- Ghế bị **vô hiệu hoá giữa một câu đang mở**: xử **y như ghế không trả lời** ⇒ mặc định Sai khi admin chốt câu; bù đắp đi qua điều chỉnh điểm thủ công của admin (`GR-036` C7).
- Bản gửi phát ra kịp nhưng **tới muộn do mạng**: server còn **giữ** nó trong `(hạn chót, hạn chót + padding]` và tô **đỏ** để admin còn phán quyết; ở **Khởi động** không có biên trên. Đây là quy tắc **server**, thuộc `specs/006`; phía thí sinh **không** được thêm giây thao tác nào (`QĐ-109`).
- Tín hiệu tới trong lúc ghế đang bị chặn *(vô hiệu hoá, banner tạm dừng)*: vào lịch sử với kết cục **TRƠ**, **không drop** (`INV-006`).

**Ranh giới dễ đọc nhầm thành mâu thuẫn**

- **Ngôi sao hy vọng KHÔNG có "ba trạng thái".** Quy tắc **một-đường-vào** áp cho cả chọn hàng ngang lẫn Ngôi sao hy vọng, nhưng **ba trạng thái nút** *(sống · khoá chờ duyệt · mở lại)* là hệ quả riêng của **hàng đợi CHẶN**. Về đích là vòng hàng đợi **không chặn**, nên tín hiệu Ngôi sao hy vọng có hiệu lực **ngay**, và nút chỉ có hai trạng thái: sống, rồi disabled *(`GR-021`, `GR-032` §Điều kiện · FR-049, FR-051)*.
- **`Esc` có hai kết cục, phân theo *ô nhập có tồn tại không*, không phân theo mode.** Ở Khởi động / Về đích / Câu hỏi phụ **mode sân khấu** không tồn tại ô nhập ⇒ `Esc` **không làm gì**. Ở mọi chỗ có ô nhập — ba vòng đó ở **mode nhập liệu**, cộng **Vượt chướng ngại vật** và **Tăng tốc** ở **cả hai mode** — `Esc` **xoá ô nhập đang có tiêu điểm**. Ở mọi ca, `Esc` **không** rời trận và **không** đổi màn *(FR-081, FR-082)*.

**Missing source behavior**

- **Băng điểm Chướng ngại vật cho `rowCount` 5-8**: `GR-009` C11 nói thẳng nguồn **không có thang nào**; v1 khoá cứng `rowCount = 4` nên **không chặn** feature này. Ghi lại để phiên bản sau không lấp bằng suy diễn.
- **Luật cho số ghế TRÊN 4**: chặn cứng ở cú bấm bắt đầu trận (`QĐ-105`, EPIC-005) nên **không chặn** feature này.
- **Kích thước khung nhìn tham chiếu**: `NFR-37` nói thẳng đây là **quyết định thiết kế, không phải yêu cầu chuẩn tắc** — nên spec này phát biểu ràng buộc *"nội dung chính nằm trong một khung nhìn"* mà **không** khai một con số pixel.

## Requirements *(mandatory)*

### Functional Requirements

#### Nhóm A — Chuông chỉ nhận click chuột và tự khoá (US-001)

- **FR-001**: Nút chuông MUST chỉ nhận **click chuột**. Hệ thống MUST NOT gán **bất kỳ** phím hay tổ hợp phím nào cho chuông — yêu cầu là *"không tồn tại phím nào được gán"*, MUST NOT hiện thực bằng cách gán rồi bỏ qua sự kiện. *(US-001 · PRD-REQ-064 · GR-034 §Điều kiện, C1, C3 · AC-001, AC-002)*
- **FR-002**: Nút *"Mở chướng ngại vật"* MUST được xếp là **chuông**, nên MUST chỉ nhận click chuột, MUST tự khoá khi bấm, và MUST NOT có dialog phía thí sinh. *(US-001, US-005 · PRD-REQ-064, PRD-REQ-068 · GR-034 §Điều kiện, C2, GR-009 §Điều kiện · AC-009)*
- **FR-003**: Nút chuông MUST tự khoá **ngay trong lần bấm đầu, ở giao diện, TRƯỚC khi tín hiệu rời máy**, để một ghế MUST NOT phát được quá một tín hiệu chuông đang **chờ xử lý** tại một thời điểm. Khoá này MUST chỉ phục vụ việc chống một cú bấm thành hai tín hiệu, MUST NOT bị dùng làm cơ chế đánh dấu *"ghế đã tiêu lượt"* — hai mục đích khác nhau. *(US-001 · PRD-REQ-064 · GR-034 §Bấm trùng, STATE-028 · AC-004)*
- **FR-004**: Điều kiện **gỡ khoá** MUST theo loại nút. Với **nút chuông thường** *(Khởi động lượt chung · cướp quyền Về đích · Câu hỏi phụ)*: khoá gắn với **một câu**, và khi câu kết thúc hệ thống MUST gỡ khoá cho mọi ghế. Với **nút *"Mở chướng ngại vật"***: khoá MUST chỉ trở thành **vĩnh viễn trong lần chạy vòng** khi tín hiệu của ghế đó đã đi qua một phán quyết **Đúng** hoặc **Sai**; khi tín hiệu bị admin **từ chối** hoặc bị chấm **Huỷ kết quả**, hệ thống MUST **mở lại** nút cho ghế đó và ghế MUST bấm lại được trong cùng vòng. *(US-001 · PRD-REQ-064 · GR-034 §Bấm trùng, GR-009 C6b, C8, GR-032 §Kích hoạt tay, STATE-028 §Ra · QĐ-061 · AC-010, AC-009a, AC-009b)*
- **FR-004a**: Số lần một ghế được **chấm** cho tín hiệu *"Mở chướng ngại vật"* trong một lần chạy vòng MUST là **tối đa một** phán quyết Đúng/Sai — cơ chế *"một lần đoán, sai thì loại"* của luật gốc MUST đếm theo **phán quyết**, MUST NOT đếm theo **cú bấm**. Hệ thống MUST NOT cần một khoá riêng để cưỡng chế điều này: chấm **Đúng** kết thúc vòng và chấm **Sai** đặt cờ *bị loại*, và cả hai đều đã làm nút biến mất. *(US-001 · PRD-REQ-064 · GR-009 C6, C9, C10, GR-010, STATE-022 · AC-009a, AC-011, AC-012)*
- **FR-004b**: Một phán quyết **Huỷ kết quả** MUST tiêu **tín hiệu** *(tín hiệu chuyển sang đã xử lý và ở lại lịch sử)* nhưng MUST NOT tiêu **quyền của ghế**; hệ thống MUST NOT đặt cờ *bị loại* và MUST NOT sinh điểm cho ai. *(US-001, US-012 · PRD-REQ-064 · GR-009 C6b, GR-032 §Kích hoạt tay · INV-006, INV-019 · QĐ-061, QĐ-104 · AC-009b)*
- **FR-005**: Ngoài cửa sổ hợp lệ của sự kiện, nút chuông MUST NOT được render và thao tác MUST không phản hồi, nên **không tín hiệu nào được tạo**. Đây MUST là hạng *không có tín hiệu*, MUST NOT hiện thực thành một cơ chế **drop** ở server. *(US-001, US-011 · PRD-REQ-064, PRD-REQ-071 · GR-034 C6, GR-003 C6, GR-032 §Điều kiện · INV-006 · AC-006)*
- **FR-005a**: Khi một tín hiệu của ghế đã tới server và có **outcome**, hệ thống MUST báo outcome đó về **chính máy đã phát tín hiệu**, bằng **nhãn trên chính nút vừa bấm** theo nhóm *mờ kèm nhãn* của FR-086 — MUST NOT dùng một lớp phủ hay toast riêng. Nhãn MUST phân biệt được ít nhất ba kết cục: **đã giành quyền** · **thua tốc độ, tín hiệu trơ** · **tín hiệu đang chờ admin duyệt**. Việc báo này MUST NOT đổi trạng thái nào của luật — tín hiệu trơ vẫn MUST NOT đổi người giữ quyền và MUST NOT mở lại chuông. *(US-001, US-005, US-012 · PRD-REQ-064, PRD-REQ-068, PRD-REQ-072 · GR-003 C3, GR-032 §Điều kiện, STATE-029, STATE-032 · INV-006, INV-013 · AC-005a)*
- **FR-006**: Biên của mọi cửa sổ chuông MUST là biên **đóng**: một click đúng mốc kết thúc MUST vẫn tạo tín hiệu. *(US-001 · PRD-REQ-064 · GR-034 §Biên, GR-032 §Biên · INV-005 · AC-007)*
- **FR-007**: Ở **Câu hỏi phụ**, nút chuông MUST chưa sống trước mốc **start timer** của admin; server MUST cũng từ chối một tín hiệu chuông tới trước mốc đó. *(US-001, US-011 · PRD-REQ-064 · GR-034, GR-024, EVENT-037 §Không hợp lệ ở · AC-008)*
- **FR-008**: Các phím tắt **khác** MUST giữ nguyên và MUST NOT bị gỡ theo: `Enter` gửi đáp án · `1`–`8` chọn hàng ngang · `Esc` xoá ô nhập. Gõ `Enter` trong lúc đang nhập đáp án MUST **gửi đáp án** và MUST NOT phát tín hiệu chuông. *(US-001, US-011 · PRD-REQ-064, PRD-REQ-071 · GR-034 §Điều kiện, C4 · AC-003)*
- **FR-009**: Server MUST bỏ qua một tín hiệu chuông **trùng** kể cả khi giao diện đã khoá — khoá phía giao diện MUST chỉ là lớp thứ nhất, MUST NOT là lớp duy nhất. *(US-001 · PRD-REQ-064 · GR-034 §Bấm trùng, GR-032 §Bấm trùng · CLAUDE.md §Zero-trust · AC-005)*
- **FR-010**: Ghế **đã bị loại khỏi Vượt chướng ngại vật** và ghế thuộc một vòng **đã có người giải đúng Chướng ngại vật** MUST NOT thấy nút *"Mở chướng ngại vật"*, và thao tác MUST không phản hồi. *(US-001, US-011 · PRD-REQ-064 · GR-009 C9, C10, GR-010, STATE-022 · AC-011, AC-012)*

#### Nhóm B — Nút gửi đáp án sống tới hạn chót (US-002)

- **FR-011**: Nút gửi đáp án MUST NOT khoá sau khi gửi và MUST sống tới **`hạn chót`** của câu; thí sinh MUST sửa và gửi lại được không giới hạn số lần trong khoảng đó. *(US-002 · PRD-REQ-065 · GR-006 §Điều kiện, C2 · QĐ-029, QĐ-109 · AC-013, AC-014)*
- **FR-012**: Nút chuông và nút gửi MUST có **vòng đời ngược nhau**; hệ thống MUST NOT dùng chung một mẫu *"bấm xong thì tắt"* cho cả hai. *(US-002 · PRD-REQ-065 · GR-034 §Bấm trùng, GR-006 §Điều kiện · AC-015)*
- **FR-013**: Bản được ghi nhận MUST là bản **hợp lệ cuối cùng trước mốc đóng** của câu. Gửi đúng một lần MUST cho kết quả y hệt: bản đầu **chính là** bản cuối. *(US-002 · PRD-REQ-065 · GR-006 C1, C2 · QĐ-110 · AC-013, AC-016)*
- **FR-013a**: Toàn hệ thống MUST theo **nguyên tắc hai trục** khi phân xử nhiều tín hiệu của cùng một ghế:
  - **Trục GIÀNH QUYỀN** — *ai* được làm — MUST lấy tín hiệu **ĐẦU TIÊN** theo server timestamp. Áp cho: bấm chuông giành quyền · bấm *"Mở chướng ngại vật"* · thứ tự hàng đợi chọn hàng ngang.
  - **Trục NỘI DUNG** — làm ra *cái gì* — MUST lấy bản **CUỐI CÙNG** hợp lệ trước mốc đóng. Áp cho: đáp án ở **mọi** vòng và **mọi** vai, gồm **người cướp quyền Về đích**; và chọn gói câu.

  Hệ thống MUST NOT áp quy tắc của một trục sang trục kia. *(US-002, US-003, US-008 · PRD-REQ-065, PRD-REQ-066, PRD-REQ-070 · GR-003, GR-006, GR-007, GR-015, GR-017, GR-018, GR-020 · AC-024a)*
- **FR-014**: Bản gửi đến **đúng `hạn chót`** MUST được ghi nhận — biên **đóng**. *(US-002 · PRD-REQ-065 · GR-006 C3, GR-008 C8 · INV-005 · AC-017)*
- **FR-015**: Bản gửi **rỗng sau khi cắt khoảng trắng** MUST bị bỏ qua và MUST NOT ghi đè bản hợp lệ đã có. *(US-002 · PRD-REQ-065, PRD-REQ-015 · GR-006 C7 · AC-018)*
- **FR-016**: Gửi lại **nội dung y hệt** ở Khởi động MUST vẫn được coi là bản cuối và MUST NOT đổi kết quả nào. *(US-002 · PRD-REQ-065 · GR-006 C6 · AC-019)*
- **FR-017**: **Lịch sử các bản đã gửi MUST NOT bị xoá**; chỉ *bản được ghi nhận* thay đổi. Thí sinh MUST NOT có đường xoá một bản đã gửi. *(US-002 · PRD-REQ-065 · GR-006 §Không đổi gì, GR-015 §Không đổi gì · INV-001 · AC-020)*
- **FR-018**: Mọi đầu vào văn bản của thí sinh MUST được **cắt khoảng trắng hai đầu ở CẢ giao diện lẫn server**; validate ở giao diện MUST chỉ là trải nghiệm, MUST NOT là lớp cưỡng chế duy nhất. *(US-002 · PRD-REQ-015 · GR-027 §Điều kiện, GR-006 §Điều kiện · CLAUDE.md §Quy ước code · AC-021)*
- **FR-019**: Ở **mode sân khấu**, nút gửi đáp án MUST NOT tồn tại ở Khởi động, Về đích và Câu hỏi phụ — thí sinh **nói** đáp án. *(US-002, US-011 · PRD-REQ-065 · EVENT-040 §Không hợp lệ ở, bảng §Kênh trả lời · AC-022)*
- **FR-020**: **Vượt chướng ngại vật** *(câu hàng ngang và câu ô trung tâm)* và **Tăng tốc** MUST luôn có ô nhập và nút gửi **bất kể mode contest**. *(US-002, US-003 · PRD-REQ-065, PRD-REQ-066 · bảng §Kênh trả lời, GR-008 §Điều kiện · AC-023)*
- **FR-021**: Sau khi admin đã chấm đối tượng tương ứng, nút gửi của ghế đó MUST khoá lại. *(US-002 · PRD-REQ-065 · GR-006 §Đồng thời, EVENT-040 §Không hợp lệ ở · AC-024)*
- **FR-021a**: **Người cướp quyền Về đích** MUST theo đúng quy tắc chung của trục nội dung: nút gửi MUST **không khoá** sau lần gửi đầu, và bản được ghi nhận MUST là bản **cuối cùng**. Vòng này MUST NOT có `hạn chót` riêng cho người cướp; mốc đóng MUST là **cú bấm chấm của admin** theo quy tắc chung, và hệ thống MUST NOT đẻ thêm một giá trị cấu hình thời gian nào cho ca này. *(US-002 · PRD-REQ-065 · GR-006 §Đồng thời, GR-018 §Thứ tự đánh giá, GR-020 §Thứ tự đánh giá, GR-032 §Kích hoạt tay · AC-024a)*

#### Nhóm C — Tăng tốc: nhận mọi lần trả lời, tính bản cuối (US-003)

- **FR-022**: Ở **Tăng tốc**, hệ thống MUST NOT khoá ô nhập hay nút gửi sau lần trả lời đầu tiên. *(US-003 · PRD-REQ-066 · GR-015 §Mục đích · AC-025)*
- **FR-023**: Mốc dùng để xếp hạng MUST là **server-received timestamp của bản cuối hợp lệ**, MUST NOT là mốc của bản gửi đầu. *(US-003 · PRD-REQ-066 · GR-015 §Thứ tự đánh giá, GR-013 §Điều kiện · TERM-037 · AC-026)*
- **FR-024**: Một bản có nội dung **khác** bản trước MUST cập nhật **cả** nội dung lẫn mốc xếp hạng. *(US-003 · PRD-REQ-066 · GR-015 C2 · AC-027)*
- **FR-025**: Một bản có nội dung **y hệt** bản trước sau khi cắt khoảng trắng MUST NOT cập nhật mốc xếp hạng; mốc MUST giữ ở lần gửi **đầu tiên** khai nội dung đó. *(US-003 · PRD-REQ-066 · GR-015 C1, §Vì sao không cập nhật mốc · AC-028)*
- **FR-026**: Ở Tăng tốc, một bản **rỗng sau khi trim** MUST bị bỏ qua, giữ nguyên **cả** bản được ghi nhận **lẫn** mốc của nó. *(US-003 · PRD-REQ-066 · GR-015 C3, C4 · AC-029, AC-030)*
- **FR-027**: Máy thí sinh MUST hiển thị rõ rằng bản vừa gửi đã được server nhận, để thí sinh biết mình còn cần gửi lại hay không. *(US-003, US-002 · PRD-REQ-066 · NFR-36 · CLAUDE.md §UX · AC-031)*

#### Nhóm D — Bảng điểm realtime của tất cả các ghế (US-004)

- **FR-028**: Màn thí sinh MUST hiện điểm của **tất cả** các ghế trong trận, MUST NOT chỉ hiện điểm của ghế đang xem. *(US-004 · PRD-REQ-067 · GR-037 §bảng ba loại thông tin · QĐ-015 · AC-032)*
- **FR-029**: Bảng điểm trên máy thí sinh MUST cập nhật **theo thời gian thực, cùng lúc và cùng nội dung với màn khán giả**. **Ngoại lệ tường minh và duy nhất**: trong lúc **banner tạm dừng** đang bật, banner MUST **che toàn bộ** màn thí sinh nên bảng điểm MUST NOT hiện — xem FR-076a. *(US-004 · PRD-REQ-067 · GR-028 §Thứ tự đánh giá, STATE-040 · QĐ-115 · AC-033, AC-090a)*
- **FR-030**: Bảng điểm MUST phản ánh cả các thao tác làm **điểm tụt đột ngột** — hoàn nguyên, bỏ vòng, điều chỉnh điểm âm — vì điểm là **hàm của nhật ký sự kiện**, MUST NOT là một con số được sửa tại chỗ. *(US-004 · PRD-REQ-067 · GR-028 §Điều kiện, C4 · INV-002 · AC-034)*
- **FR-031**: **Điểm âm MUST hiện bình thường**; hệ thống MUST NOT kẹp giá trị hiển thị về 0 và MUST NOT có sàn. *(US-004 · PRD-REQ-067 · GR-028 §Biên · INV-018 · QĐ-012 · AC-035)*
- **FR-032**: Ràng buộc phạm vi hiển thị MUST chỉ áp cho **đáp án chuẩn** và **bài làm của ghế khác**; nó MUST NOT áp cho **điểm**, vốn công khai với mọi vai ở mọi thời điểm. *(US-004, US-009 · PRD-REQ-067, PRD-REQ-049 · GR-037 §bảng ba loại thông tin, GR-008 §Điều kiện · INV-017 · AC-036)*

#### Nhóm E — Thao tác đua tốc độ: tức thời, không dialog, không rút lại (US-005)

- **FR-033**: Bấm chuông, bấm *"Mở chướng ngại vật"* và gửi đáp án MUST có hiệu lực **tức thời** ở phía thí sinh — tín hiệu MUST rời máy ngay tại cú bấm. *(US-005 · PRD-REQ-068 · TERM-001, GR-034 C1 · AC-037)*
- **FR-034**: Ba đường trên MUST NOT có dialog xác nhận nào trên máy thí sinh. *(US-005 · PRD-REQ-068 · QĐ-005 · CLAUDE.md §UX · AC-038)*
- **FR-035**: Máy thí sinh MUST NOT có nút huỷ, rút lại hay thu hồi cho bất kỳ tín hiệu nào đã phát. *(US-005 · PRD-REQ-068 · PRD §11 EPIC-008 Out of scope · AC-039)*
- **FR-036**: **Ngoại lệ duy nhất** có dialog phía thí sinh MUST là **chọn hàng ngang ở mode nhập liệu**; hệ thống MUST NOT mở rộng ngoại lệ này sang thao tác nào khác. *(US-005, US-006 · PRD-REQ-068, PRD-REQ-069 · GR-007 §Điều kiện, C7, STATE-038 · QĐ-005, QĐ-019 · AC-040)*
- **FR-037**: Đường sửa lỗi bấm nhầm của thí sinh MUST là **admin bấm No** trong hàng đợi; hệ thống MUST NOT drop tín hiệu và MUST cho mọi tín hiệu đã tới server một kết cục — thực thi, bị từ chối, hoặc **trơ**. *(US-005, US-006 · PRD-REQ-068, PRD-REQ-046 · GR-032 §Mục đích, §Điều kiện · INV-006 · AC-041)*

#### Nhóm F — Chọn hàng ngang: một đường vào theo mode, nút ba trạng thái (US-006)

- **FR-038**: Ở **mode sân khấu**, máy thí sinh MUST NOT render nút chọn hàng ngang; đường vào duy nhất MUST là admin click. *(US-006 · PRD-REQ-069 · GR-007 §Kích hoạt, C6, §Invalid transitions · AC-042)*
- **FR-039**: Ở **mode nhập liệu**, **chỉ thí sinh** MUST chọn được hàng ngang; admin MUST NOT có nút chọn thay. *(US-006 · PRD-REQ-069 · GR-007 §Kích hoạt, §Invalid transitions · AC-043)*
- **FR-040**: Cả hai đường vào MUST đi qua **hàng đợi chặn** và MUST chỉ có hiệu lực sau khi admin xác nhận. *(US-006 · PRD-REQ-069 · GR-007 §Điều kiện, GR-032 C1, C2 · AC-044)*
- **FR-041**: Nút chọn hàng ngang của thí sinh MUST có **đúng ba trạng thái**: **sống** · **khoá chờ duyệt** · **mở lại khi admin từ chối**. Hệ thống MUST NOT bỏ trạng thái thứ ba. *(US-006 · PRD-REQ-069, PRD-REQ-046 · GR-007 C2, C7, §Khoá là tạm, STATE-029, STATE-031 · AC-045, AC-046)*
- **FR-042**: Sau khi thí sinh xác nhận dialog, **đúng một** tín hiệu MUST rời máy và nút MUST chuyển sang khoá. *(US-006 · PRD-REQ-069 · GR-007 C7 · AC-047)*
- **FR-043**: Dialog xác nhận phía thí sinh MUST NOT thay thế bước admin duyệt — hai lớp MUST tồn tại đồng thời và phục vụ hai mục đích khác nhau *(chống bấm nhầm vs phán quyết)*. *(US-006 · PRD-REQ-069 · GR-007 §Ví dụ *không hợp lệ*, STATE-038 · AC-048)*
- **FR-044**: Khi admin bấm **No**, nút chọn của ghế đó MUST **mở lại**, ghế MUST giữ nguyên lượt, và hệ thống MUST NOT để lại tác dụng phụ nào: ô chữ chưa đánh dấu · câu chưa tiêu · đồng hồ chưa chạy. *(US-006 · PRD-REQ-046, PRD-REQ-069 · GR-007 C2, GR-032 C3, TERM-027 · INV-008 · AC-046, AC-049)*
- **FR-045**: Một hàng ngang **đã mở** MUST NOT còn là mục tiêu chọn được: nút MUST NOT render và thao tác MUST không phản hồi. *(US-006, US-011 · PRD-REQ-069 · GR-007 C5 · AC-050)*
- **FR-046**: Bốn phép kiểm — đúng lượt · chưa bị loại · hàng ngang còn chưa mở · lượt chưa dùng — MUST là **điều kiện render** đánh giá tại thời điểm dựng màn thí sinh, MUST NOT là các bước validate xếp thứ tự sau khi tín hiệu đã phát. *(US-006, US-011 · PRD-REQ-069, PRD-REQ-071 · GR-007 §Thứ tự đánh giá, STATE-027 · AC-051)*
- **FR-047**: Một ghế vừa bấm *"Mở chướng ngại vật"* và đang **chờ duyệt** MUST vẫn trả lời được câu hàng ngang bình thường — hệ thống MUST NOT khoá ghế đó sớm. *(US-006, US-005 · PRD-REQ-069 · GR-009 C14 · AC-052)*

#### Nhóm G — Đặt Ngôi sao hy vọng theo mode (US-007)

- **FR-048**: Ở **mode sân khấu**, máy thí sinh MUST NOT render nút Ngôi sao hy vọng; server MUST **từ chối** một tín hiệu Ngôi sao hy vọng đến từ máy thí sinh và MUST ghi một dòng nhật ký thao tác. *(US-007 · PRD-REQ-069 · GR-021 C8, EVENT-042 · AC-053)*
- **FR-049**: Ở **mode nhập liệu**, thí sinh MUST tự bấm được, và tín hiệu MUST có hiệu lực **ngay**, MUST NOT chờ admin duyệt — Về đích là vòng hàng đợi **không** chặn. *(US-007 · PRD-REQ-069 · GR-021 §bảng theo mode, GR-032 §Điều kiện · AC-054)*
- **FR-050**: Cửa sổ đặt Ngôi sao hy vọng MUST đóng tại mốc **admin bấm hiển thị câu**. Ngoài cửa sổ, nút MUST NOT render, thao tác MUST không phản hồi, và cờ *đã dùng* MUST **vẫn chưa** được đặt. *(US-007, US-011 · PRD-REQ-069 · GR-021 §Điều kiện, C6, STATE-026 · AC-055)*
- **FR-051**: Một cú bấm Ngôi sao hy vọng thứ hai trong cùng một lần chạy vòng MUST NOT được chấp nhận: nút MUST đã disabled tại lần đầu, và server MUST bỏ qua tín hiệu trùng. *(US-007, US-012 · PRD-REQ-069, PRD-REQ-072 · GR-021 C3, §Bấm trùng · AC-056)*
- **FR-052**: Người **cướp quyền** MUST NOT đặt được Ngôi sao hy vọng trên câu đang cướp; ngôi sao của họ MUST còn nguyên cho lượt thi của chính họ. *(US-007 · PRD-REQ-069 · GR-021 §Người cướp KHÔNG thể dùng NSHV, GR-020 §Điều kiện · AC-057)*
- **FR-053**: Khi admin **bỏ vòng** hoặc **chạy lại vòng** Về đích, cờ Ngôi sao hy vọng MUST được đặt lại và nút MUST sống lại trên máy thí sinh. *(US-007 · PRD-REQ-069 · GR-021 §Điều kiện, STATE-026 §Ra · AC-058)*

#### Nhóm H — Chọn gói câu Về đích (US-008)

- **FR-054**: Gói câu MUST gồm **đúng ba mục**, mỗi mục thuộc **hai mức điểm cấu hình được** *(preset `O26_DEFAULT@1` đặt {20, 30})*. Hệ thống MUST NOT hard-code hai con số đó vào giao diện. *(US-008 · PRD-REQ-070 · GR-017 §Điều kiện · CLAUDE.md §Quy ước code · AC-059)*
- **FR-055**: Ở **mode sân khấu**, máy thí sinh MUST NOT render nút chọn gói; **admin bấm theo lời thí sinh** MUST là đường vào duy nhất, MUST NOT được mô tả như một fallback. Server MUST **từ chối** một tín hiệu chọn gói tới từ máy thí sinh. *(US-008 · PRD-REQ-070 · GR-017 §bảng theo mode, C4, C5 · AC-060, AC-061)*
- **FR-056**: Ở **mode nhập liệu**, thí sinh MUST tự chọn được gói của mình. *(US-008 · PRD-REQ-070 · GR-017 §bảng theo mode · AC-062)*
- **FR-057**: Ở **mode nhập liệu**, admin MUST có **fallback chọn hộ** một gói mặc định khi thí sinh chưa chọn lúc tới lượt, và MUST override được giá trị mặc định đó. Cú chọn hộ MUST là một **giá trị khởi tạo**, MUST NOT là một cú chốt: nút chọn trên máy thí sinh MUST **vẫn sống** sau đó, và thí sinh MUST đổi lại được theo bản-cuối-thắng. Hệ thống MUST NOT tạo ra một mốc khoá thứ hai cho ca này. *(US-008 · PRD-REQ-070 · GR-017 C3, C6, §Điều kiện · AC-063)*
- **FR-058**: Đổi gói MUST theo nguyên tắc **bản-cuối-thắng** cho tới mốc **admin bấm hiển thị câu đầu tiên** của gói; từ mốc đó nút MUST khoá. Đây MUST là **mốc khoá duy nhất** của việc chọn gói — cú chọn hộ của admin MUST NOT là một mốc thứ hai. *(US-008 · PRD-REQ-070 · GR-017 §Điều kiện, C6, C7 · AC-063, AC-064, AC-065)*
- **FR-059**: Một gói có **số mục khác ba** hoặc mang **mức ngoài tập cấu hình** MUST bị từ chối ở **cả** giao diện lẫn server. *(US-008 · PRD-REQ-070 · GR-017 C8, §Điều kiện · CLAUDE.md §Zero-trust · AC-066)*
- **FR-060**: Chọn gói MUST NOT áp cơ chế *"bấm xong thì tắt"* của chuông — nó MUST mang ngữ nghĩa của nút gửi đáp án. *(US-008, US-012 · PRD-REQ-070, PRD-REQ-072 · GR-017 §Bấm trùng · AC-064)*

#### Nhóm I — Đáp án chỉ tới máy thí sinh từ mốc câu khép (US-009)

- **FR-061**: **Trước mốc CÂU KHÉP**, đáp án chuẩn và tiêu chí đạt của câu thực hành MUST NOT rời server tới máy thí sinh, ở **mọi** kênh và **mọi** thời điểm. *(US-009 · PRD-REQ-049 · GR-037 §Điều kiện, C4 · INV-017 · AC-067)*
- **FR-062**: Hệ thống MUST NOT đẩy đáp án xuống máy thí sinh trước mốc rồi ẩn bằng một **cờ hiển thị**; server MUST chỉ đẩy **tại đúng mốc**. *(US-009 · PRD-REQ-049 · GR-037 §Cấm · CLAUDE.md §Phạm vi hiển thị đáp án · AC-068)*
- **FR-063**: Từ **mốc câu khép** trở đi, nếu cờ *hiện đáp án sau khi chấm* của trận **bật**, server MUST đẩy đáp án tới máy thí sinh. Cờ này MUST ở **cấp trận** và MUST mặc định **bật**. *(US-009 · PRD-REQ-049 · GR-037 §Điều kiện, C5 · QĐ-062, QĐ-080 · AC-069)*
- **FR-064**: Khi cờ **tắt**, máy thí sinh MUST NOT nhận đáp án ở bất kỳ thời điểm nào của câu. *(US-009 · PRD-REQ-049 · GR-037 C3 · AC-070)*
- **FR-065**: Ở **Về đích**, trong lúc **cửa sổ cướp quyền còn mở**, server MUST NOT đẩy đáp án tới máy thí sinh; câu MUST chỉ khép khi cửa sổ đã đóng **và** người cướp đã được chấm, hoặc hết cửa sổ không ai bấm, hoặc người thi chính được chấm Đúng. *(US-009 · PRD-REQ-049 · GR-037 §bảng mốc câu khép, C6, GR-020 §Cấm · AC-071, AC-072)*
- **FR-066**: Ba ca biên MUST giữ đúng:
  - Câu **bị bỏ qua** — hết cửa sổ chuông không ai bấm — MUST **vẫn** được công bố theo cờ: câu đó đã hỏi, đã tiêu, đã khép.
  - Câu khép bằng phán quyết **Huỷ kết quả** MUST NOT **tự** công bố; đường công bố **bằng tay** của admin vẫn là một đường khác và vẫn dùng được.
  - Đáp án **Chướng ngại vật** MUST NOT đi theo cơ chế FR-061 → FR-065: nó lộ **khi có người giải đúng**, hoặc **khi admin bấm công bố**, và cờ *hiện đáp án sau khi chấm* MUST NOT mở đường nào cho nó.

  *(US-009 · PRD-REQ-049 · GR-037 C7, C8, C9, GR-012 C2 · AC-073, AC-074, AC-075)*
- **FR-067**: Khi admin đang chờ **kích hoạt tay** một tín hiệu khác sau một phán quyết *Huỷ kết quả*, mốc câu khép MUST **lùi** và server MUST NOT đẩy đáp án tới máy thí sinh trước mốc mới. *(US-009 · PRD-REQ-049 · GR-037 C8b, GR-032 §Kích hoạt tay · AC-076)*
- **FR-068**: **Bài làm của ghế khác** MUST ẩn trên máy thí sinh khi câu còn mở, và MUST chỉ lộ khi admin bấm hiển thị. Một tín hiệu *"Mở chướng ngại vật"* tới giữa chừng MUST NOT làm lộ thêm thứ gì. *(US-009, US-004 · PRD-REQ-049 · GR-008 §Điều kiện, C9, §Đồng thời, GR-037 §bảng ba loại thông tin · AC-077)*

#### Nhóm J — Khôi phục sau mất kết nối (US-010)

- **FR-069**: Khi server phát hiện một ghế mất kết nối, ghế đó MUST được giữ trong một **ngưỡng chờ mặc định 120 giây** đo bằng đồng hồ server; ngưỡng MUST là giá trị cấu hình được. *(US-010 · PRD-REQ-050 · GR-036 §Điều kiện, STATE-023 · TERM-043 · AC-078)*
- **FR-070**: Trong lúc mất kết nối, máy thí sinh MUST hiện banner *"đang kết nối lại"*; khi nối lại được, banner MUST tắt và hệ thống MUST báo đã kết nối lại. *(US-010 · PRD-REQ-050 · GR-036 C1, STATE-034 · NFR-36 · AC-079)*
- **FR-071**: Gói khôi phục MUST gồm: vòng và giai đoạn hiện tại · câu đang mở *(nội dung, media, mức điểm)* · **`hạn chót` theo server time** · **bản gửi gần nhất của chính ghế đó** · các cờ ghế còn hiệu lực · trạng thái chuông theo luật · bàn cờ Vượt chướng ngại vật · **điểm công khai** · các lớp phủ đang bật. *(US-010 · PRD-REQ-050 · GR-036 C8, STATE-023 §Gói khôi phục · AC-080)*
- **FR-072**: Client MUST dựng lại đồng hồ từ **`hạn chót` theo server time**, MUST NOT dựng từ một giá trị *"còn N giây"*; đồng hồ MUST NOT được đặt lại, vì mất kết nối MUST NOT mua thêm thời gian. *(US-010 · PRD-REQ-050 · GR-036 C8, §Ví dụ · INV-004 · QĐ-046 · AC-081)*
- **FR-073**: Gói khôi phục MUST NOT chứa **đáp án** và MUST NOT chứa **bài làm của ghế khác**. *(US-010, US-009 · PRD-REQ-050, PRD-REQ-049 · GR-036 C8, STATE-023 §Ba ràng buộc · INV-017 · AC-082)*
- **FR-074**: Thao tác khôi phục MUST là một phép **đọc**: nó MUST NOT sinh sự kiện nào và MUST NOT đổi trạng thái trận. *(US-010 · PRD-REQ-050 · QĐ-046, STATE-023 · AC-083)*
- **FR-075**: **Mỗi lần mất kết nối MUST mở một cửa sổ chờ mới tính lại từ đầu**; hệ thống MUST NOT cộng dồn thời gian các lần trước và MUST NOT giới hạn số lần. *(US-010 · PRD-REQ-050 · GR-036 §Nối lại nhiều lần · AC-084)*
- **FR-076**: Đồng hồ của câu MUST NOT dừng vì một ghế mất kết nối; vòng MUST chạy tiếp bình thường và hai ghế cùng mất kết nối MUST có hai cửa sổ chờ độc lập. *(US-010 · PRD-REQ-050 · GR-036 C4, C10 · INV-016 · AC-085, AC-086)*
- **FR-076a**: Trong lúc **banner tạm dừng đang bật**, banner MUST **che toàn bộ màn thí sinh** — thí sinh MUST NOT thấy bảng điểm và MUST NOT thấy câu hỏi — **và** mọi sự kiện do thí sinh phát MUST bị server từ chối. Dữ liệu bên dưới MUST tiếp tục chảy: khi banner tắt, máy thí sinh MUST hiện trạng thái **hiện tại**, MUST NOT hiện trạng thái đã lưu lúc banner bật, và hệ thống MUST NOT cần một bước đồng bộ lại nào. Đặc tả này MUST NOT nhắc tới đồng hồ: banner và đồng hồ **loại trừ lẫn nhau** *(`STATE-040` §Vào, `QĐ-050`)* nên dưới banner **không có đồng hồ nào** để che. `INV-021` MUST giữ nguyên hiệu lực — **che không phải đổi**. *(US-010, US-011, US-004 · PRD-REQ-050, PRD-REQ-067, PRD-REQ-071 · STATE-040, §Invalid transitions, GR-035 · QĐ-050, QĐ-115 · INV-021 · AC-090a)*
- **FR-077**: Quá ngưỡng chờ, hệ thống MUST NOT tự loại, tự xoá hay tự vô hiệu hoá ghế nào; ghế MUST ở lại trận với nguyên điểm và nguyên vị trí. *(US-010 · PRD-REQ-050 · GR-036 C2, STATE-024 · AC-087)*
- **FR-078**: **Ký tự đang gõ dở mà chưa gửi** MUST NOT được khôi phục — nó chưa từng tới server; hệ thống MUST NOT tạo ra ảo giác rằng nó đã được lưu. *(US-010 · PRD-REQ-050 · STATE-023 §Thứ không khôi phục được · AC-088)*

#### Nhóm K — Invalid state và hành vi `Esc` (US-011)

- **FR-079**: Máy thí sinh MUST phân biệt **hai** kết cục hiển thị cho một control không bấm được, và tiêu chí phân định MUST là **control đó có nghĩa ở pha hiện tại hay không**:
  - **Không có nghĩa ở pha này ⇒ MUST NOT render.** Thao tác MUST không phản hồi và **không sự kiện nào được tạo**. Áp cho: ngoài cửa sổ chuông · Câu hỏi phụ trước mốc start timer · ghế **bị loại** khỏi vòng · ghế **bị vô hiệu hoá** · mode sân khấu với nút chọn hàng ngang, chọn gói và Ngôi sao hy vọng · hàng ngang **đã mở** · nút Ngôi sao hy vọng sau mốc hiển thị câu.
  - **Có nghĩa nhưng ghế này bị luật cấm ⇒ MUST render ở trạng thái vô hiệu hoá kèm nhãn** — xem FR-086.

  Cả hai kết cục MUST cho ra **không tín hiệu nào được tạo**, và cả hai MUST NOT bị hiện thực thành một cơ chế **drop** ở server. *(US-011, US-012 · PRD-REQ-071, PRD-REQ-072 · §Invalid transitions hạng **KHÔNG CÓ TÍN HIỆU**, PRD §9.4, GR-034 C5, C6, GR-003 C5, QĐ-004 · INV-006 · AC-089, AC-089a, AC-096)*
- **FR-080**: Server MUST kiểm lại mọi tín hiệu của thí sinh và MUST từ chối tín hiệu ở trạng thái không hợp lệ, bất kể giao diện đã ẩn control hay chưa. *(US-011 · PRD-REQ-071 · §Invalid transitions, CLAUDE.md §Zero-trust · AC-090)*
- **FR-081**: Ở **màn thi đấu của thí sinh**, `Esc` MUST NOT là nút quay lại và MUST NOT rời trận hay đổi màn. *(US-011 · PRD-REQ-071 · NFR-39, QĐ-072 · CLAUDE.md §Điều hướng · AC-091)*
- **FR-082**: Ở **mode nhập liệu**, `Esc` MUST chỉ **xoá ô nhập** đang có tiêu điểm. Ở **mode sân khấu** — nơi không có ô nhập ở Khởi động, Về đích và Câu hỏi phụ — `Esc` MUST không làm gì. *(US-011 · PRD-REQ-071 · GR-034 §Điều kiện, QĐ-004 · AC-092, AC-093)*
- **FR-083**: Màn thi đấu của thí sinh MUST là **ngoại lệ** của quy tắc *"mọi màn có nút quay lại"*; hệ thống MUST NOT đặt một đường rời trận bằng phím tắt ở màn này. *(US-011 · PRD-REQ-071 · NFR-39 · CLAUDE.md §Điều hướng · AC-091)*

#### Nhóm L — Không chặn gửi lại; khoá theo luật chơi thì vẫn khoá (US-012)

- **FR-084**: Nút hành động MUST chỉ hiện **trạng thái đang xử lý** và MUST NOT bị vô hiệu hoá để chống gửi trùng; người dùng MUST gửi lại được và server MUST nhận bản cuối cùng trước hạn. *(US-012 · PRD-REQ-072 · CLAUDE.md §UX · QĐ-060 · AC-094)*
- **FR-085**: Chống trùng và bảo đảm tính idempotent MUST là việc của **server** *(nhật ký sự kiện)*, MUST NOT là việc của giao diện. *(US-012 · PRD-REQ-072 · GR-028, GR-032 §Bấm trùng · INV-002 · AC-095)*
- **FR-086**: Khoá **theo LUẬT CHƠI** MUST render nút ở trạng thái vô hiệu hoá **kèm nhãn nêu lý do**, MUST NOT ẩn nút, và MUST NOT để trạng thái đó lẫn với **trạng thái đang xử lý** của FR-084. Tập ca MUST gồm đúng sáu mục, mỗi mục một nhãn **khác nhau**: ghế **đã bấm chuông và bị chấm Sai** ở câu này · **Ngôi sao hy vọng đã dùng** · **lượt chọn hàng ngang đã dùng** · **chưa tới lượt** · tín hiệu **đang chờ admin duyệt** · tín hiệu **đã trơ vì thua tốc độ** *(FR-005a)*. *(US-012, US-011 · PRD-REQ-072 · GR-034 C5, PRD §9.4, STATE-026, STATE-027, STATE-028, STATE-029 · AC-096, AC-097)*
- **FR-087**: Ranh giới giữa hai loại khoá MUST được ghi lại như một quy ước dùng chung của các nút một chiều phía thí sinh, để một lần rà quy tắc giao diện về sau MUST NOT gỡ nhầm khoá theo luật chơi. *(US-012 · PRD-REQ-072 · PRD-REQ-072 §Acceptance intent · AC-098)*

#### Nhóm M — Ràng buộc xuyên suốt của màn thí sinh

- **FR-088**: Mọi thao tác bất đồng bộ trên máy thí sinh MUST hiển thị trạng thái đang xử lý, và MUST có phản hồi thành công hoặc thất bại sau mỗi hành động. *(US-002, US-003, US-012 · NFR-36 · CLAUDE.md §UX · AC-099)*
- **FR-089**: Nội dung chính của màn thi đấu MUST nằm gọn trong **một khung nhìn tham chiếu** — không ép cuộn để thấy phần quan trọng, cũng không nhồi nhét. Con số kích thước MUST là quyết định thiết kế, MUST NOT được khai như một requirement chuẩn tắc. *(US-004, US-011 · NFR-37 · CLAUDE.md §UX · AC-100)*
- **FR-090**: Mọi chuỗi giao diện trên máy thí sinh MUST chỉ dùng tiếng Việt, và mọi mốc thời gian hiển thị MUST theo UTC+7. *(US-004, US-010 · NFR-38 · CLAUDE.md §Quy ước code · AC-101)*
- **FR-091**: Media của thí sinh MUST được preload ở dạng **mã hoá**, với khoá phát đúng lúc reveal theo server time, và MUST có đường lui reveal-only khi cơ chế preload không khả dụng. *(US-009 · PRD-REQ-049 · CLAUDE.md §Quy ước code · INV-017 · AC-102)*

### Key Entities

- **Chuông**: Tín hiệu thí sinh phát để **giành quyền trả lời**. Chỉ nhận click chuột, không phím tắt, tự khoá ngay khi bấm ở giao diện trước khi gửi. Nút *"Mở chướng ngại vật"* thuộc loại này. *(TERM-025, `GR-034`, `STATE-028`)*
- **Nguyên tắc hai trục**: Quy tắc phái sinh dùng cho **mọi** cơ chế có nhiều tín hiệu từ cùng một ghế. Trục **giành quyền** *(ai được làm)* lấy tín hiệu **đầu tiên**; trục **nội dung** *(làm ra cái gì)* lấy bản **cuối cùng**. Gặp một cơ chế mới thì hỏi nó thuộc trục nào rồi suy ra. *(`GR-003`, `GR-006`, `GR-007`, `GR-015`, `GR-017`, `GR-018`, `GR-020`)*
- **Bản được ghi nhận**: Bản gửi hợp lệ **cuối cùng trước mốc đóng** của một ghế cho một câu, ở **mọi vai** kể cả người cướp quyền. Mốc đóng là `hạn chót` ở mọi ca, **trừ người cướp quyền Về đích** — ca này không có `hạn chót` riêng nên mốc đóng là **cú bấm chấm của admin**. Khác hẳn **lịch sử các bản đã gửi** — thứ không bao giờ bị xoá và chỉ dùng để admin xem lại. *(`GR-006`, `GR-015`, `GR-018`, `GR-020`, `INV-001`)*
- **Mốc xếp hạng**: Server-received timestamp của **bản cuối hợp lệ**, dùng riêng ở Tăng tốc. Nó **không** dời khi bản mới trùng nội dung bản trước sau khi trim. *(TERM-037, `GR-015`)*
- **Ba trạng thái của nút chọn hàng ngang**: **sống** · **khoá chờ duyệt** · **mở lại khi admin từ chối**. Trạng thái thứ ba tồn tại vì từ chối **không** làm thí sinh mất lượt. *(`GR-007`, `STATE-029`, `STATE-031`, `INV-008`)*
- **Dialog xác nhận phía thí sinh**: Lớp phủ duy nhất phía thí sinh, chỉ có ở **chọn hàng ngang mode nhập liệu**. Nó chống **bấm nhầm**, và **không** thay thế bước admin duyệt. *(`STATE-038`, `GR-007`)*
- **Cờ Ngôi sao hy vọng đã dùng**: Cờ một chiều trong phạm vi **một lần chạy vòng** Về đích; đặt lại khi admin bỏ hoặc chạy lại vòng. *(`STATE-026`, `GR-021`)*
- **Gói câu Về đích**: Đúng ba mục, mỗi mục thuộc hai mức điểm cấu hình được. Đổi được theo bản-cuối-thắng tới mốc admin bấm hiển thị câu đầu tiên. *(`GR-017`, `STATE-012`)*
- **Mốc câu khép**: Thời điểm không còn ai được trả lời câu đó nữa — mốc **duy nhất** mà đáp án được phép rời server tới máy thí sinh. Trùng cú bấm chấm ở mọi vòng **trừ** Về đích và trừ các ca có kích hoạt tay. *(`GR-037`, `INV-017`)*
- **Gói khôi phục**: Ảnh chụp **chỉ đọc** mà server gửi cho một ghế vừa nối lại. Ba ràng buộc: dựng đồng hồ từ `hạn chót` server · không chứa đáp án · không sinh sự kiện. *(`STATE-023`, `GR-036`, `QĐ-046`)*
- **Ngưỡng chờ kết nối**: Cửa sổ mặc định **120 giây** giữ ghế, đo bằng đồng hồ server, tính lại từ đầu ở mỗi lần mất kết nối. *(TERM-043, `GR-036`)*
- **Khoá theo luật chơi**: Trạng thái vô hiệu hoá **hợp lệ** của một nút **vẫn được render**, kèm nhãn nêu lý do, do trạng thái game gây ra. Đúng **sáu** ca, mỗi ca một nhãn khác nhau: đã bấm & bị chấm Sai · Ngôi sao hy vọng đã dùng · lượt chọn đã dùng · chưa tới lượt · đang chờ admin duyệt · tín hiệu đã trơ vì thua tốc độ. Khác hẳn **hai** thứ: disable nút để **chống gửi trùng** *(bị cấm)*, và **control mất nghĩa ở pha hiện tại** *(không render)*. *(`GR-034` C5, `PRD` §9.4, `QĐ-060`)*

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: **0** tổ hợp phím phát được tín hiệu chuông — thử **toàn bộ** phím đơn, phím kèm modifier và các phím media trên một máy thí sinh trong cửa sổ chuông đang mở.
- **SC-002**: **0** trường hợp một ghế phát quá **một** tín hiệu chuông cho cùng một đích — thử với 10 cú click liên tiếp trong 1 giây, ở cả ba vòng có chuông và ở nút *"Mở chướng ngại vật"*.
- **SC-003**: **100%** số lần gửi lại đáp án trước `hạn chót` đều được server ghi nhận, và bản **cuối cùng** là bản được tính ở **100%** số lượt thử — kiểm với chuỗi 1, 3 và 20 lần gửi.
- **SC-004**: **0** lần mốc xếp hạng Tăng tốc bị dời bởi một bản trùng nội dung sau khi trim — thử với 50 chuỗi gửi sinh ngẫu nhiên có lẫn bản trùng, bản rỗng và bản mới.
- **SC-005**: **0** byte mang đáp án rời server tới kênh của máy thí sinh trước mốc câu khép, trong một trận đầy đủ có dùng cả *Huỷ kết quả*, cả **kích hoạt tay** và cả một câu Về đích bị cướp quyền — xác minh bằng bắt gói tin, không bằng quan sát giao diện.
- **SC-006**: **100%** số lần bảng điểm trên máy thí sinh đổi giá trị **cùng lúc** với màn khán giả, đo bằng chênh lệch hiển thị **dưới một khung hình** trên cùng một mạng LAN; và **100%** số ghế đều có mặt trên bảng, kể cả ghế bỏ thi và ghế đang âm điểm.
- **SC-007**: **0** lần điểm âm bị hiển thị thành `0` hay bị ẩn — thử với các giá trị `-5`, `-30` và `-100`.
- **SC-008**: **100%** số lần admin bấm **No** cho một tín hiệu chọn hàng ngang đều làm nút chọn của ghế đó **mở lại**, và **0** tác dụng phụ phát sinh — ô chữ không đổi, câu không bị tiêu, đồng hồ không chạy.
- **SC-009**: **0** dialog xác nhận tồn tại trên đường bấm chuông, đường *"Mở chướng ngại vật"* và đường gửi đáp án — rà toàn bộ màn thí sinh ở **cả hai** mode.
- **SC-010**: **100%** control không bấm được đều rơi đúng một trong hai kết cục — **không render** khi control mất nghĩa ở pha hiện tại, **render vô hiệu hoá kèm nhãn** khi ghế bị luật cấm — rà **100%** các tổ hợp *(vòng × giai đoạn × cờ ghế × mode)* dựng được; **0** control ở nhóm mất nghĩa bị render, **0** control ở nhóm bị luật cấm bị ẩn, và với mỗi tổ hợp xác minh server cũng từ chối tín hiệu tương ứng.
- **SC-011**: Một ghế nối lại giữa câu thấy đúng thời gian còn lại với sai số **dưới 200 ms** so với đồng hồ server, ở **100%** số lần thử; và **0** gói khôi phục nào chứa đáp án hoặc bài làm của ghế khác.
- **SC-019**: **0** thời điểm nào máy thí sinh để lộ bảng điểm hoặc câu hỏi trong lúc banner tạm dừng đang bật — thử ở mọi trạng thái mà banner bật được; và **100%** số lần tắt banner đều cho ra **trạng thái hiện tại**, đo bằng một cú điều chỉnh điểm thực hiện trong lúc banner che.
- **SC-012**: **0** sự kiện được sinh bởi thao tác khôi phục kết nối — đối chiếu nhật ký sự kiện trước và sau 20 lần ngắt-nối liên tiếp của cùng một ghế.
- **SC-013**: **0** lần `Esc` ở màn thi đấu làm rời trận, đổi màn, hay thoát khỏi chế độ toàn màn hình — thử ở cả hai mode và ở cả năm vòng.
- **SC-014**: **0** nút hành động phía thí sinh bị vô hiệu hoá vì lý do **chống gửi trùng**; **100%** số nút đang bị khoá **theo luật chơi** đều được render kèm nhãn phân biệt được với trạng thái đang xử lý; và **6/6** lý do khoá cho ra **sáu nhãn khác nhau**, phân biệt được bằng mắt — gồm hai nhãn phân định *đã giành quyền* với *thua tốc độ*.
- **SC-018**: **100%** tín hiệu do một ghế phát ra đều có outcome **hiển thị trên chính máy ghế đó** trong vòng một nhịp cập nhật; **0** trường hợp thí sinh không phân biệt được ba kết cục *đã giành quyền* · *thua tốc độ* · *đang chờ duyệt* — đo bằng cách hỏi người thử nghiệm đọc trạng thái từ màn hình mà không xem nhật ký.
- **SC-015**: **100%** số lần chọn gói câu không hợp lệ *(số mục ≠ 3, mức ngoài tập cấu hình)* đều bị từ chối ở **cả** giao diện lẫn server — xác minh cả qua giao diện lẫn qua yêu cầu gửi thẳng tới server.
- **SC-016**: **0** tín hiệu tới từ máy thí sinh ở mode sân khấu cho ba thao tác chỉ-admin *(chọn hàng ngang, chọn gói, Ngôi sao hy vọng)* được server chấp nhận; **100%** số lần từ chối đều để lại một dòng nhật ký.
- **SC-017**: Nội dung chính của màn thi đấu nằm trọn trong một khung nhìn tham chiếu, đo bằng **0** phần tử thao tác bắt buộc nằm dưới mép dưới khi trang vừa tải, ở cả hai mode và ở cả năm vòng.

## Out of Scope

- **Luật số học và mô hình engine**: mức điểm và hình phạt của từng vòng, thang xếp hạng `40/30/20/10` và quy tắc nhảy bậc khi đồng thời gian, băng điểm Chướng ngại vật, cơ chế transfer của cướp quyền, số học của Ngôi sao hy vọng, độ dài các cửa sổ chuông, việc **server quyết ai giành quyền**, điểm là hàm của nhật ký sự kiện và cơ chế hoàn nguyên, server time, rút đề và không lặp câu, **cửa sổ giữ bản tới muộn** *(server giữ hay từ chối một bản, biên trên, ngoại lệ Khởi động)* — thuộc EPIC-006 (`specs/006-game-engine-luat-thi-dau`). Spec này chỉ đặc tả **những gì máy thí sinh làm và nhìn thấy**.
- **Bề mặt điều khiển của admin**: bốn mốc bấm của một câu, màn chấm và đối chiếu đáp án, mốc mở nút chấm, cú bấm *chốt câu*, màn hàng đợi và thao tác duyệt / từ chối / **kích hoạt tay**, điều chỉnh điểm thủ công, ba cửa ra của một vòng, mở và đóng đáp án, bàn cờ Vượt chướng ngại vật, vô hiệu hoá và kích hoạt lại ghế, **phán quyết ghế mất kết nối** *(giữ / gia hạn)*, ba hạng dialog và cảnh báo lệch luật — thuộc EPIC-007 (`specs/007-dieu-khien-can-thiep-admin`). Riêng **PRD-REQ-046**, **PRD-REQ-069**, **PRD-REQ-070** và **PRD-REQ-072** khai cả EPIC-008, nên **hệ quả trên máy thí sinh** của chúng nằm trong spec này; mặt admin vẫn thuộc spec 007.
- **Vòng đời cấp trận**: cú bấm bắt đầu trận và việc đóng băng cấu hình, trạng thái nghỉ, chốt trận, huỷ trận, niêm phong, hàng rào số ghế và cơ chế **ghế bỏ thi** — thuộc EPIC-005. Spec này chỉ nhận **hệ quả hiển thị** của ghế bỏ thi trên bảng điểm.
- **Trình diễn**: màn khán giả, lớp phủ 1920×1080 cho phần mềm dựng hình, màn MC chữ lớn, **nội dung và cách render** lớp công bố kết quả và banner tạm dừng, chủ đề, các khe âm thanh — thuộc EPIC-009. Spec này chỉ phát biểu rằng bảng điểm trên máy thí sinh **khớp** màn khán giả, và rằng lớp phủ đang bật nằm trong gói khôi phục.
- **Xác thực và vào phòng**: đăng nhập bằng username và mật khẩu, mã phòng 6 số, catalog permission và `PERM-045`, mô hình vai — thuộc EPIC-001. Spec này chỉ phát biểu ranh giới *"máy thí sinh không giữ `PERM-045`"* như một **cổng**, không đặc tả catalog.
- **Kho đề và bộ đề** (EPIC-002) · **nhập/xuất gói contest** (EPIC-003) · **contest builder và luật tuỳ biến**, gồm việc **đặt mode trả lời cấp contest**, đặt cờ *hiện đáp án sau khi chấm*, đặt ngưỡng chờ kết nối và đặt hai mức điểm của gói Về đích (EPIC-004) · **sau trận: bảng điểm cuối, biên bản, thống kê, phát lại** (EPIC-010) · **nhật ký thao tác chung và hạn lưu trữ** (EPIC-011) · **hai hồ sơ triển khai** (EPIC-012).
- **Giao diện luyện tập solo, vai trainer, bộ đề public và share-link** — thuộc v1.5, không thuộc bất kỳ epic nào của v1.
- **Mọi cơ chế tự chấm hoặc tự cộng trừ điểm** (`NON-GOAL-001`) · **luật cho số ghế trên 4** (`NON-GOAL-012`) · **thi đội** (`NON-GOAL-013`) · **dialog xác nhận cho thao tác đua tốc độ** và **nút huỷ phía thí sinh** (`docs/PRD.md` §11 EPIC-008 *Out of scope*, `QĐ-005`).

## Open Questions

**Không có.** Vế EPIC-003 của `PRD-REQ-098` — ràng buộc *xuất gói contest không phụ thuộc dịch vụ ngoài hay kết nối Internet* — nay do `specs/003` nhận (FR-032).

## Traceability Matrix

| User story | PRD requirement | Game rules | Functional requirements | Acceptance scenarios |
|---|---|---|---|---|
| US-001 | PRD-REQ-064 | `GR-034` *(toàn bộ bảng)*, `GR-003` C5, C6, `GR-009` §Điều kiện, C6, C6b, C8, C9, C10, `GR-032` §Bấm trùng, §Biên, §Kích hoạt tay, `QĐ-023`, `QĐ-061`, `INV-005`, `INV-006`, `INV-019` | FR-001 → FR-010 **+ FR-004a, FR-004b** | AC-001 → AC-012 **+ AC-009a, AC-009b** |
| US-002 | PRD-REQ-065, PRD-REQ-015 | `GR-006` *(toàn bộ bảng)*, `GR-034` §Bấm trùng, `GR-027` §Điều kiện, `GR-008` C8, `GR-018` §Điều kiện, `GR-020` §Điều kiện, C4, bảng §Kênh trả lời, `QĐ-029`, `QĐ-058`, `QĐ-109`, `QĐ-110`, `INV-001`, `INV-005` | FR-011 → FR-021 **+ FR-013a, FR-021a** | AC-013 → AC-024 **+ AC-024a** |
| US-003 | PRD-REQ-066 | `GR-015` *(toàn bộ bảng)*, `GR-013` §Điều kiện, `GR-014` §Thứ tự đánh giá, `TERM-037` | FR-022 → FR-027 | AC-025 → AC-031 |
| US-004 | PRD-REQ-067 | `GR-028` §Điều kiện, C4, §Biên, `GR-037` §bảng ba loại thông tin, `GR-008` §Điều kiện, `INV-002`, `INV-017`, `INV-018` | FR-028 → FR-032 | AC-032 → AC-036 |
| US-005 | PRD-REQ-068 | `GR-034` §Điều kiện, C1, `GR-009` §Điều kiện, `GR-007` §Điều kiện, C7, `GR-032` §Mục đích, `TERM-001`, `INV-006` | FR-033 → FR-037 | AC-037 → AC-041 |
| US-006 | PRD-REQ-069, PRD-REQ-046 | `GR-007` C2, C5, C6, C7, C8, §Thứ tự đánh giá, `GR-032` C1, C2, C3, `GR-009` C8, C14, `STATE-027`, `STATE-029`, `STATE-031`, `STATE-038`, `INV-008` | FR-038 → FR-047 | AC-042 → AC-052 |
| US-007 | PRD-REQ-069 *(vế Ngôi sao hy vọng)* | `GR-021` C3, C6, C7, C8, §Thứ tự đánh giá, §Bấm trùng, `GR-020` §Điều kiện, `STATE-026`, `EVENT-042`, `INV-005` | FR-048 → FR-053 | AC-053 → AC-058 |
| US-008 | PRD-REQ-070 | `GR-017` C1, C2, C3, C4, C5, C6, C7, C8, §Bấm trùng, §Khác `QĐ-019`, `STATE-012`, `EVENT-041` | FR-054 → FR-060 **+ FR-013a** | AC-059 → AC-066 |
| US-009 | PRD-REQ-049 | `GR-037` C3 → C9, §Cấm, §bảng mốc câu khép, `GR-008` §Điều kiện, C9, `GR-020` §Cấm, `GR-032` §Kích hoạt tay, `INV-017` | FR-061 → FR-068 **+ FR-091** | AC-067 → AC-077, AC-102 |
| US-010 | PRD-REQ-050 | `GR-036` C1, C2, C4, C8, C10, §Biên, §Nối lại nhiều lần, `STATE-023`, `STATE-024`, `STATE-034`, `INV-004`, `INV-016`, `QĐ-046` | FR-069 → FR-078 | AC-078 → AC-088 |
| US-011 | PRD-REQ-071, PRD-REQ-072 | `GR-034` §Điều kiện, C5, C6, `GR-003` C5, `GR-007` §Thứ tự đánh giá, `GR-032` §Điều kiện, §Invalid transitions, `PRD` §9.4, `NFR-39`, `QĐ-004`, `QĐ-072` | FR-079 → FR-083 **+ FR-005, FR-046, FR-086** | AC-089 → AC-093 **+ AC-089a** |
| US-012 | PRD-REQ-072 | `GR-034` C5, `GR-006` §Điều kiện, `GR-032` §Bấm trùng, `GR-028`, `PRD` §9.4, `STATE-026` → `STATE-029`, `QĐ-060` | FR-084 → FR-087 **+ FR-051, FR-060, FR-079** | AC-094 → AC-098 **+ AC-089a** |
| *(xuyên suốt)* | PRD-REQ-067, 071 · `NFR-36` → `NFR-39` · `CLAUDE.md` §UX, §Quy ước code | — | FR-088 → FR-091 | AC-099 → AC-102 |

### Bản đồ phủ bảng quyết định

> Mỗi outcome trong bảng quyết định của từng `GR-NNN` **thuộc phạm vi feature này** đều có ít nhất một acceptance scenario. Ca nào thuộc epic khác thì ghi rõ spec sở hữu, theo đúng cách `specs/006` và `specs/007` đã dùng — chúng **không** được đặc tả lại ở đây.

**Bốn rule mà EPIC-008 khai trực tiếp**

| Game rule | Ca trong bảng quyết định | Acceptance scenarios |
|---|---|---|
| `GR-034` | C1 · C2 · C3 · C4 · C5 *(khoá theo luật chơi ⇒ **mờ kèm nhãn**)* · C6 *(ngoài cửa sổ ⇒ **ẩn**)* | AC-001 · AC-009 · AC-002 · AC-003 · AC-096, AC-089a · AC-006, AC-089 |
| `GR-036` | C1 · C2 · C3 · C4 · C5 · C5b · C6 · C7 · C8 · C9 · C10 · C11 | AC-079 · AC-087 · *(EPIC-007)* · AC-085 · AC-090 · AC-032 · *(EPIC-007)* · *(EPIC-007)* · AC-080, AC-081, AC-082, AC-088 · *(EPIC-007)* · AC-086 · *(EPIC-005)* |
| `GR-006` | C1 · C2 · C3 · C4 · C5 · C6 · C7 | AC-013 · AC-014 · AC-017 · *(EPIC-007 — hiển thị và chấm bản quá hạn)* · *(EPIC-007)* · AC-019 · AC-018 |
| `GR-015` | C1 · C2 · C3 · C4 · C5 · C6 | AC-028 · AC-027 · AC-029 · AC-030 · *(EPIC-007)* · *(EPIC-007)* |

**Các rule mà PRD-REQ của EPIC-008 tham chiếu — chỉ phủ phần thuộc epic này**

| Game rule | Ca thuộc phạm vi EPIC-008 | Acceptance scenarios |
|---|---|---|
| `GR-003` | C1, C2 *(click trong cửa sổ tạo tín hiệu)* · C3 *(tín hiệu trơ ⇒ **nhãn báo outcome về chính máy phát**)* · C5 *(đã chấm Sai ở câu này ⇒ **mờ kèm nhãn**)* · C6 *(ngoài cửa sổ ⇒ **ẩn**)* · §Biên | AC-001 · AC-005 · AC-096 · AC-006, AC-089 · AC-007 |
| `GR-007` | C2 *(bấm No ⇒ mở lại, không tác dụng phụ)* · C5 *(hàng đã mở)* · C6 *(mode sân khấu không có nút)* · C7 *(dialog rồi khoá, đúng một tín hiệu)* · C8 *(sai lượt vẫn vào hàng đợi)* · §Thứ tự đánh giá | AC-046, AC-049 · AC-050 · AC-042 · AC-045, AC-047 · AC-051 · AC-089 |
| `GR-008` | C6 *(ghế bị loại không có ô nhập)* · C8 *(đúng mốc vẫn được ghi nhận)* · C9 *(không lộ thêm gì khi có tín hiệu CNV)* · §Điều kiện *(hàng ngang luôn gõ máy)* | AC-011 · AC-017 · AC-077 · AC-023 |
| `GR-009` | C6 *(chấm Sai ⇒ ghế bị loại, máy hết nút)* · **C6b** *(Huỷ kết quả ⇒ không bị loại, nút mở lại)* · C8 *(bấm No ⇒ không mất lượt, nút mở lại)* · C9, C10 *(không hiển thị)* · C14 *(vẫn trả lời hàng ngang khi đang chờ duyệt)* · §Điều kiện *(nút là chuông; mỗi ghế đi qua tối đa **một PHÁN QUYẾT Đúng/Sai** cho cả vòng)* | AC-009a · AC-009b · AC-009b · AC-011, AC-012 · AC-052 · AC-009, AC-009a |
| `GR-013` | §Điều kiện *(mốc xếp hạng là bản cuối)* — thang điểm và bảng C1→C7 thuộc EPIC-006 | AC-026 · *(EPIC-006 — `specs/006`)* |
| `GR-017` | C1 · C2 · C3 · C4 · C5 · C6 · C7 · C8 · §Bấm trùng | AC-059 · AC-059 · AC-063 · AC-060 · AC-061 · AC-064 · AC-065 · AC-066 · AC-064 |
| `GR-020` | §Điều kiện *(người cướp tính **bản cuối**)* · §Cấm *(không công bố khi cửa sổ cướp mở)* · C4 *(chuỗi ba bản ⇒ chấm trên **bản cuối**)* · §Thứ tự đánh giá *(không có đồng hồ trả lời riêng)* | AC-024a · AC-071 · AC-024a · AC-024a |
| `GR-021` | C3 *(bấm lần hai)* · C6 *(bấm sau mốc hiển thị)* · C7 *(mode sân khấu admin bấm)* · C8 *(máy thí sinh gửi ⇒ từ chối)* · §Thứ tự đánh giá · §Bấm trùng | AC-056 · AC-055 · AC-053 · AC-053 · AC-054 · AC-056 |
| `GR-027` | §Điều kiện *(trim hai đầu là bước bắt buộc)* — việc **tô khác biệt** thuộc EPIC-007 | AC-021 · *(EPIC-007 — `specs/007` AC-028 → AC-032)* |
| `GR-028` | §Điều kiện, C4 *(bỏ vòng ⇒ tính lại)* · §Biên *(không sàn, không trần)* — bảng C1→C5 về sinh event thuộc EPIC-006 | AC-034 · AC-035 · *(EPIC-006)* |
| `GR-032` | C1 *(vào hàng đợi chờ duyệt)* · C3 *(bấm No ⇒ không mất lượt)* · C5 *(hết vòng ⇒ hàng đợi đặt lại, lịch sử giữ)* · C6 *(ghế bị loại không tạo tín hiệu)* · §Bấm trùng · §Biên · §Vòng đời tín hiệu | AC-044 · AC-046 · AC-041 · AC-011 · AC-004, AC-005 · AC-007 · AC-041 |
| `GR-037` | C3 · C4 · C5 · C6 · C7 · C8 · C8b · C9 · §Cấm · §bảng ba loại thông tin | AC-070 · AC-067 · AC-069 · AC-071 · AC-073 · AC-074 · AC-076 · AC-075 · AC-068 · AC-036 |

**Bảng dùng chung, trạng thái và transition**

| Nguồn | Nội dung | Acceptance scenarios |
|---|---|---|
| bảng §*Kênh trả lời quyết định lúc nào admin chấm được* | kênh **nói** ⇒ máy thí sinh không có nút gửi · kênh **gõ** ⇒ nút gửi sống tới `hạn chót` · hai vòng luôn gõ máy | AC-022 · AC-013 · AC-023 |
| bảng §*Số câu theo luật* | gói Về đích **3** mục | AC-059 |
| `STATE-022` | ghế bị loại khỏi Vượt chướng ngại vật ⇒ máy không hiển thị gì | AC-011 |
| `STATE-023`, `STATE-024`, `STATE-034` | grace 120 giây · gói khôi phục · quá grace không hệ quả tự động · banner kết nối | AC-078, AC-079, AC-080, AC-087 |
| `STATE-025` | ghế bị vô hiệu hoá ⇒ mọi tín hiệu **TRƠ**, không drop | AC-090 |
| `STATE-026`, `STATE-027`, `STATE-028` | ba cờ ghế sinh ra **khoá theo luật chơi**; `STATE-028` §Ra tách **hai** nhánh gỡ khoá theo loại nút *(chuông thường gắn với một câu · *"Mở chướng ngại vật"* gắn với phán quyết)* | AC-055, AC-056 · AC-045 · AC-004, AC-010, AC-009b, AC-096 |
| `STATE-038` | dialog xác nhận phía thí sinh — ngoại lệ duy nhất | AC-040, AC-045, AC-048 |
| `STATE-040` | banner tạm dừng **che toàn bộ màn thí sinh** và chặn mọi sự kiện; dữ liệu bên dưới vẫn chảy *(`QĐ-115`)*; banner và đồng hồ **loại trừ lẫn nhau** *(`QĐ-050`)* | AC-090, AC-090a |
| `EVENT-037` → `EVENT-042` | sáu sự kiện do thí sinh phát, gồm §Hợp lệ ở và §Không hợp lệ ở của từng cái | AC-001, AC-009, AC-045, AC-013, AC-062, AC-054 |
| `EVENT-040` | người cướp quyền Về đích theo quy tắc **bản cuối** như mọi vai; **không** có ngoại lệ bản-đầu-tiên nào | AC-024a |
| §*Invalid transitions* | các dòng thuộc sự kiện của thí sinh — hạng **KHÔNG CÓ TÍN HIỆU** | AC-006, AC-008, AC-011, AC-012, AC-042, AC-050, AC-053, AC-055, AC-061, AC-089, AC-090 |
| `QĐ-109`, `QĐ-110` | nút gửi tắt đúng tại `hạn chót` · Khởi động không còn mốc cắt riêng | AC-014, AC-017 · AC-016 |
| `NFR-36` → `NFR-39` | trạng thái đang xử lý · một khung nhìn · tiếng Việt và UTC+7 · ngoại lệ nút quay lại | AC-099 · AC-100 · AC-101 · AC-091 |

### Acceptance Scenarios (chi tiết)

> Mọi kịch bản đều giả định **một** phiên admin đang giữ quyền điều khiển (`GR-026` §Đồng thời) và mọi mốc thời gian đều là **server time** (`INV-004`). Phần *concurrency outcome* chỉ nói về tín hiệu của **thí sinh**.

#### Nhóm A — Chuông chỉ nhận click chuột và tự khoá (US-001)

**AC-001** — *US*: US-001 · *FR*: FR-001, FR-003, FR-033 · *GR*: `GR-034` C1, `GR-003` C1, C2
**Given** một trận official mode sân khấu đang ở Khởi động lượt chung, admin đã bấm hiển thị câu nên **cửa sổ chuông đang mở**, chưa ghế nào bấm, nút chuông của ghế 3 đang ở trạng thái sống, điểm của bốn ghế lần lượt là `10 · 0 · 20 · −5`,
**When** thí sinh ghế 3 **click chuột** vào nút chuông,
**Then** một tín hiệu chuông rời máy kèm server timestamp; nút chuông của ghế 3 chuyển sang **khoá ngay tại cú bấm, trước khi tín hiệu rời máy**; tín hiệu vào hàng đợi **không chặn** và được tính ngay; điểm của cả bốn ghế **không đổi** — giành quyền chưa phải phán quyết; và lịch sử tín hiệu ghi thêm đúng một dòng.

**AC-002** — *US*: US-001 · *FR*: FR-001 · *GR*: `GR-034` C3
**Given** cùng trạng thái AC-001, tiêu điểm bàn phím đang ở màn thi đấu của ghế 3,
**When** thí sinh gõ lần lượt phím cách, `B`, `Space`, `Ctrl+Enter` và mọi phím đơn khác,
**Then** **không tín hiệu chuông nào được tạo** ở bất kỳ phím nào; hàng đợi không nhận thêm dòng nào; nút chuông vẫn ở trạng thái sống; và trạng thái câu, đồng hồ, điểm đều **không đổi**.

**AC-003** — *US*: US-001 · *FR*: FR-008 · *GR*: `GR-034` C4
**Given** một trận mode nhập liệu đang ở Khởi động, câu đang đếm giờ, ghế 2 đã gõ `"Huế"` vào ô nhập nhưng chưa gửi,
**When** thí sinh ghế 2 nhấn `Enter`,
**Then** hệ thống **gửi đáp án** `"Huế"`; **không** tín hiệu chuông nào được tạo; nút chuông vẫn nguyên trạng thái trước đó; và ô nhập vẫn cho gõ tiếp.

**AC-004** — *US*: US-001, US-012 · *FR*: FR-003, FR-086 · *GR*: `GR-034` §Bấm trùng, `STATE-028`
**Given** thí sinh ghế 3 vừa click chuông ở AC-001 và nút đã chuyển sang khoá,
**When** thí sinh ghế 3 click vào nút chuông lần thứ hai trong cùng câu đó,
**Then** **không tín hiệu thứ hai nào được tạo**; hàng đợi vẫn chỉ có một dòng của ghế 3; nút mang dấu hiệu nêu rõ đây là **khoá theo luật chơi**, không phải trạng thái đang xử lý; và không sự kiện nào được sinh.

**AC-005** — *US*: US-001 · *FR*: FR-009 · *GR*: `GR-034` §Bấm trùng, `GR-032` §Bấm trùng
**Given** cùng trạng thái AC-004, một công cụ ngoài giao diện gửi thẳng một yêu cầu chuông thứ hai của ghế 3 tới server,
**When** server nhận yêu cầu đó,
**Then** server **bỏ qua** nó; **0** tín hiệu mới được ghi vào hàng đợi đang hoạt động; người giữ quyền **không đổi**; và điểm **không đổi**.

**AC-005a** — *US*: US-001, US-005, US-012 · *FR*: FR-005a, FR-086 · *GR*: `GR-003` C1, C3, `STATE-032` · `INV-006`, `INV-013`
**Given** một câu Khởi động lượt chung đang mở cửa sổ chuông, chưa ai bấm,
**When** ghế 1 click chuông tại `t=1,20s` và ghế 3 click tại `t=1,35s`,
**Then** ghế 1 giành quyền và nút chuông của ghế 1 mang nhãn **"đã giành quyền"**; tín hiệu của ghế 3 thành **trơ** và nút chuông của ghế 3 mang nhãn **"đã có người giành quyền trước"** — hai nhãn **khác nhau**, đọc được ngay trên máy từng ghế; **không** lớp phủ hay toast nào được bật cho việc này; và về mặt luật **không gì đổi** — ghế 3 **không** thành người giữ quyền, chuông **không** mở lại, đồng hồ **không** bị gián đoạn, tín hiệu trơ **vẫn ở lại lịch sử**. Lặp lại ở Vượt chướng ngại vật với một tín hiệu **đang chờ admin duyệt** và **Then** nhãn là **"đang chờ duyệt"** — khác cả hai nhãn trên.

**AC-006** — *US*: US-001, US-011 · *FR*: FR-005, FR-079 · *GR*: `GR-034` C6, `GR-003` C6
**Given** một câu Khởi động lượt chung mà cửa sổ chuông **đã đóng** vì đã quá `3` giây kể từ mốc start timer và không ai bấm,
**When** thí sinh ghế 1 đưa chuột tới vị trí nút chuông và click,
**Then** nút **không được render** ở vị trí đó nên thao tác **không phản hồi**; **không tín hiệu nào được tạo** — đây là hạng *không có tín hiệu*, **không** phải một tín hiệu bị server drop; và hàng đợi, đồng hồ, điểm đều không đổi.

**AC-007** — *US*: US-001 · *FR*: FR-006 · *GR*: `GR-034` §Biên, `GR-032` §Biên · `INV-005`
**Given** một câu Khởi động lượt chung với cửa sổ chuông `3` giây tính từ mốc start timer, chưa ai bấm,
**When** thí sinh ghế 4 click chuông tại **đúng mốc `3,000` giây** theo server time,
**Then** tín hiệu **được tạo và hợp lệ** — biên là biên **đóng**; câu **không** bị bỏ qua. Lặp lại kịch bản với một click tại `3,001` giây và **Then** nút đã không còn render, **không tín hiệu nào được tạo**.

**AC-008** — *US*: US-001, US-011 · *FR*: FR-007 · *GR*: `GR-024`, `EVENT-037` §Không hợp lệ ở
**Given** một trận đang ở Câu hỏi phụ, admin đã bấm **hiển thị câu** nhưng **chưa** bấm start timer,
**When** thí sinh ghế 2 click vào vị trí nút chuông và, song song, một yêu cầu chuông được gửi thẳng tới server,
**Then** máy thí sinh **không hiển thị** nút nên click không phản hồi; **và server cũng từ chối** yêu cầu gửi thẳng — zero-trust; không tín hiệu nào có hiệu lực; và trạng thái câu không đổi.

**AC-009** — *US*: US-001, US-005 · *FR*: FR-002 · *GR*: `GR-034` C2, `GR-009` §Điều kiện
**Given** một trận đang ở Vượt chướng ngại vật, chưa hàng ngang nào được hỏi, ghế 1 chưa bị loại và chưa ai giải đúng Chướng ngại vật,
**When** thí sinh ghế 1 click nút *"Mở chướng ngại vật"*,
**Then** tín hiệu rời máy **ngay, không qua dialog nào**; nút tự khoá tại cú bấm; tín hiệu vào **hàng đợi chặn** và ở trạng thái **chờ duyệt**; đồng hồ của câu hàng ngang đang chạy **vẫn chạy bình thường**; và **không** thứ gì thêm được hiển thị cho ghế 1 — không đáp án chuẩn, không bài làm của ghế khác. Xác nhận thêm rằng **không phím nào** phát được tín hiệu này.

**AC-009a** — *US*: US-001 · *FR*: FR-004, FR-004a · *GR*: `GR-009` C6, C9, C10, `STATE-022`
**Given** một trận đang ở Vượt chướng ngại vật, ghế 1 đã bấm *"Mở chướng ngại vật"* nên nút đang khoá, admin đã bấm **Yes** cho tín hiệu đó,
**When** admin chấm **Sai**,
**Then** ghế 1 mang cờ **bị loại khỏi vòng** và máy ghế 1 **không hiển thị** nút nào của vòng nữa — nút biến mất vì cờ *bị loại*, **không** vì một khoá riêng; ghế 1 **không** bấm lại được cho tới hết vòng; và điểm hàng ngang ghế 1 đã kiếm được **giữ nguyên, không bị trừ**. Lặp lại kịch bản với phán quyết **Đúng** và **Then** vòng kết thúc nên nút *"Mở chướng ngại vật"* biến mất trên **mọi** máy thí sinh.

**AC-009b** — *US*: US-001, US-012 · *FR*: FR-004, FR-004b · *GR*: `GR-009` C6b, C8, `GR-032` §Kích hoạt tay · `INV-006`, `INV-019`
**Given** cùng trạng thái mở đầu AC-009a — ghế 1 đã bấm, nút đang khoá, admin đã bấm Yes, chưa hàng ngang nào được hỏi nên băng đang ở `60`,
**When** admin chấm **Huỷ kết quả**,
**Then** ghế 1 **KHÔNG** mang cờ *bị loại*; **không ai** được điểm; nút *"Mở chướng ngại vật"* của ghế 1 **mở lại** và ghế 1 bấm lại được **trong cùng vòng**; **tín hiệu cũ vẫn bị tiêu** — nó chuyển sang *đã xử lý* và ở lại lịch sử, **không** bị xoá; và vòng chạy tiếp bình thường. Lặp lại kịch bản với admin bấm **No** *(chưa chấm gì)* và **Then** nút cũng **mở lại**, ghế 1 **không mất lượt**, và **0 tác dụng phụ** phát sinh.

**AC-010** — *US*: US-001 · *FR*: FR-004 · *GR*: `GR-034` §Bấm trùng, `STATE-028` §Ra
**Given** ghế 3 đã bấm chuông ở câu số 5 của Khởi động lượt chung và nút đang khoá, admin đã chấm xong câu đó,
**When** admin chuyển sang câu số 6 và bấm hiển thị câu,
**Then** khoá chuông của **mọi ghế** được gỡ; nút chuông của ghế 3 trở lại trạng thái **sống**; và ghế 3 bấm được bình thường ở câu mới.

**AC-011** — *US*: US-001, US-006, US-011 · *FR*: FR-010, FR-079 · *GR*: `GR-009` C9, `GR-010`, `GR-008` C6, `STATE-022`
**Given** một trận đang ở Vượt chướng ngại vật, ghế 2 đã bị chấm **Sai** cho tín hiệu Chướng ngại vật nên mang cờ **bị loại khỏi vòng**, một câu hàng ngang đang mở và đồng hồ đang chạy,
**When** thí sinh ghế 2 thử click nút *"Mở chướng ngại vật"*, thử click chọn một hàng ngang, và thử gõ vào ô nhập đáp án,
**Then** máy ghế 2 **không hiển thị** control nào trong ba thứ đó và mọi thao tác **không phản hồi**; **0** tín hiệu được tạo; **điểm hàng ngang ghế 2 đã kiếm được giữ nguyên, không bị trừ**; và ghế 2 vẫn thấy bảng điểm của cả bốn ghế.

**AC-012** — *US*: US-001, US-011 · *FR*: FR-010 · *GR*: `GR-009` C10
**Given** một trận ở Vượt chướng ngại vật mà ghế 4 vừa được chấm **Đúng** cho tín hiệu Chướng ngại vật nên vòng đã kết thúc,
**When** thí sinh ghế 1 thử click nút *"Mở chướng ngại vật"*,
**Then** nút **không hiển thị**, thao tác **không phản hồi**, **0** tín hiệu được tạo, và trạng thái vòng không đổi.

#### Nhóm B — Nút gửi đáp án sống tới hạn chót (US-002)

**AC-013** — *US*: US-002 · *FR*: FR-011, FR-013 · *GR*: `GR-006` C1, C2
**Given** một trận mode nhập liệu đang ở Khởi động lượt riêng, câu của ghế 1 đang đếm giờ với `hạn chót` là `t0 + 3s`, ghế 1 chưa gửi bản nào,
**When** thí sinh ghế 1 gửi `"Hà Nội"` tại `t0+1s`, sửa và gửi `"Huế"` tại `t0+2s`, rồi sửa và gửi `"Đà Nẵng"` tại `t0+2,5s`,
**Then** nút gửi **không khoá** sau lần nào; **bản được ghi nhận là `"Đà Nẵng"`**; cả ba bản vẫn nằm trong lịch sử bản đã gửi của ghế 1; điểm **không đổi** — ghi nhận đáp án chưa phải phán quyết; và ô nhập vẫn cho sửa tiếp cho tới `hạn chót`.

**AC-014** — *US*: US-002 · *FR*: FR-011 · *GR*: `QĐ-109`
**Given** cùng câu như AC-013 với `hạn chót` là `t0 + 3s`,
**When** đồng hồ đi qua mốc `t0 + 3s`,
**Then** nút gửi và ô nhập trên máy thí sinh **tắt đúng tại mốc đó**, không có thêm giây thao tác nào; thí sinh không gửi thêm được bản nào từ giao diện; và bản được ghi nhận vẫn là bản hợp lệ cuối cùng trước mốc.

**AC-015** — *US*: US-002 · *FR*: FR-012 · *GR*: `GR-034` §Bấm trùng, `GR-006` §Điều kiện
**Given** một câu Khởi động lượt chung mode nhập liệu mà ghế 2 vừa giành quyền bằng chuông và đang trong thời gian suy nghĩ,
**When** thí sinh ghế 2 gửi đáp án rồi gửi lại lần nữa,
**Then** **nút chuông vẫn ở trạng thái khoá** *(vòng đời một chiều)* trong khi **nút gửi vẫn sống** và nhận được lần gửi thứ hai *(vòng đời ngược lại)*; hai nút **không** dùng chung một hành vi; và bản thứ hai trở thành bản được ghi nhận.

**AC-016** — *US*: US-002 · *FR*: FR-013 · *GR*: `GR-006` C1 · `QĐ-110`
**Given** một câu Khởi động mode nhập liệu mà ghế 3 gửi **đúng một bản** `"Hà Nội"` tại `t0+1s` rồi không đụng nữa, `hạn chót` là `t0 + 3s`,
**When** đồng hồ qua `hạn chót` và admin bấm chấm,
**Then** bản được ghi nhận là `"Hà Nội"` — nó **vừa là bản đầu vừa là bản cuối**; hệ thống **không** cần một mốc *"công bố đáp án"* riêng để chốt điều đó; và **`hạn chót` là hạn nhận bài duy nhất** của câu.

**AC-017** — *US*: US-002 · *FR*: FR-014 · *GR*: `GR-006` C3, `GR-008` C8 · `INV-005`
**Given** một câu Vượt chướng ngại vật hàng ngang với `hạn chót` là `t0 + 15s`, ghế 4 đã gửi `"Sông Hồng"` tại `t0+10s`,
**When** thí sinh ghế 4 gửi `"Sông Đà"` tại **đúng mốc `t0 + 15,000s`** theo server time,
**Then** bản đó **được ghi nhận** — biên **đóng**; bản được ghi nhận đổi thành `"Sông Đà"`; và một bản gửi tại `t0 + 15,001s` **không** còn nút để phát đi từ giao diện.

**AC-018** — *US*: US-002 · *FR*: FR-015, FR-018 · *GR*: `GR-006` C7
**Given** ghế 1 đã có bản được ghi nhận là `"Huế"`, câu vẫn đang đếm giờ,
**When** thí sinh ghế 1 xoá ô nhập, gõ ba dấu cách rồi gửi,
**Then** bản đó **rỗng sau khi trim** nên bị **bỏ qua**; **bản được ghi nhận vẫn là `"Huế"`, không bị ghi đè**; lịch sử vẫn ghi lại lần gửi rỗng đó; và điểm không đổi.

**AC-019** — *US*: US-002 · *FR*: FR-016 · *GR*: `GR-006` C6
**Given** ghế 2 đã gửi `"Huế"` ở một câu Khởi động và câu vẫn đang đếm giờ,
**When** thí sinh ghế 2 gửi lại đúng chuỗi `"Huế"` một lần nữa,
**Then** bản được ghi nhận **vẫn là `"Huế"`**; kết quả chấm **không đổi**; ở Khởi động **không có** hệ quả nào về thời gian vì vòng này không xếp theo tốc độ; và lịch sử ghi thêm một dòng.

**AC-020** — *US*: US-002 · *FR*: FR-017 · *GR*: `GR-006` §Không đổi gì · `INV-001`
**Given** ghế 1 đã gửi năm bản khác nhau trong một câu,
**When** admin mở lịch sử bản đã gửi của ghế 1 cho câu đó,
**Then** **cả năm** bản còn nguyên kèm mốc thời gian; **0** bản bị xoá; máy thí sinh **không** có đường xoá một bản đã gửi; và chỉ *bản được ghi nhận* là thứ đã thay đổi qua năm lần.

**AC-021** — *US*: US-002 · *FR*: FR-018 · *GR*: `GR-027` §Điều kiện
**Given** một câu Tăng tốc đang đếm giờ,
**When** thí sinh gửi chuỗi `"  Huế  "` — có khoảng trắng ở cả hai đầu,
**Then** giao diện cắt khoảng trắng hai đầu **trước khi gửi**; server **cắt lại một lần nữa** khi nhận; bản được lưu là `"Huế"`; và một yêu cầu gửi thẳng tới server mang `"  Huế  "` cũng cho kết quả `"Huế"`.

**AC-022** — *US*: US-002, US-011 · *FR*: FR-019 · *GR*: bảng §Kênh trả lời, `EVENT-040` §Không hợp lệ ở
**Given** một trận **mode sân khấu** đang lần lượt ở Khởi động, Về đích và Câu hỏi phụ,
**When** thí sinh quan sát màn thi đấu của mình ở cả ba vòng,
**Then** **không tồn tại** ô nhập và nút gửi đáp án ở cả ba — thí sinh **nói** đáp án; **0** tín hiệu gửi đáp án có thể phát ra từ máy thí sinh; và server từ chối một yêu cầu gửi đáp án nếu vẫn nhận được.

**AC-023** — *US*: US-002, US-003 · *FR*: FR-020 · *GR*: bảng §Kênh trả lời, `GR-008` §Điều kiện
**Given** cùng trận **mode sân khấu** như AC-022, nay đang ở Vượt chướng ngại vật rồi ở Tăng tốc,
**When** thí sinh quan sát màn thi đấu của mình ở hai vòng đó,
**Then** ô nhập và nút gửi **có tồn tại** ở cả hai — hai vòng này **luôn gõ máy bất kể mode contest**; và mọi quy tắc bản-cuối-thắng vẫn áp bình thường.

**AC-024** — *US*: US-002 · *FR*: FR-021 · *GR*: `GR-006` §Đồng thời
**Given** một câu Khởi động mode nhập liệu đã qua `hạn chót` và admin vừa bấm chấm cho ghế 1,
**When** thí sinh ghế 1 thử gửi thêm một bản,
**Then** nút gửi của ghế 1 **đã khoá lại**; **0** bản mới được ghi nhận; phán quyết **không bị lật**; và nếu một yêu cầu vẫn lọt tới thì server ghi nó vào lịch sử mà **không** đổi bản được ghi nhận.

**AC-024a** — *US*: US-002 · *FR*: FR-013a, FR-021a · *GR*: `GR-020` §Điều kiện, C4, `GR-018` §Điều kiện, `GR-006` §Đồng thời
**Given** một trận **mode nhập liệu** đang ở Về đích, câu `30` điểm của ghế A vừa bị admin chấm **Sai** nên cửa sổ cướp `5` giây mở, ghế B bấm chuông sớm nhất và giành quyền cướp, admin **chưa** bấm chấm B,
**When** thí sinh ghế B gửi `"Paris"`, rồi `"London"`, rồi `"Berlin"`,
**Then** nút gửi của B **không khoá** sau lần nào — người cướp theo **đúng quy tắc chung** của trục nội dung; bản được ghi nhận là **`"Berlin"`**; cả ba bản vẫn trong lịch sử; **không** tồn tại một `hạn chót` riêng cho người cướp; và nút gửi của B chỉ khoá lại tại **cú bấm chấm của admin**. Đối chiếu trục còn lại trong cùng kịch bản: nếu ghế C bấm chuông **sau** B thì tín hiệu của C là **trơ** — trục **giành quyền** vẫn lấy **người đầu tiên**, không đổi.

#### Nhóm C — Tăng tốc (US-003)

**AC-025** — *US*: US-003 · *FR*: FR-022 · *GR*: `GR-015` §Mục đích
**Given** một câu Tăng tốc `20` giây đang đếm giờ, ghế 1 đã gửi bản đầu tại `t0+3s`,
**When** thí sinh ghế 1 tiếp tục gõ và gửi thêm bản nữa tại `t0+8s`,
**Then** ô nhập và nút gửi **vẫn sống** sau lần gửi đầu — hệ thống **không** khoá chúng; bản thứ hai được server nhận; và thí sinh gửi tiếp được cho tới `hạn chót`.

**AC-026** — *US*: US-003 · *FR*: FR-023 · *GR*: `GR-015` §Thứ tự đánh giá, `GR-013` §Điều kiện
**Given** một câu Tăng tốc mà ghế 1 gửi `"Paris"` tại `100 ms` rồi `"Berlin"` tại `200 ms`, ghế 2 gửi một bản duy nhất tại `150 ms`, cả hai đều sẽ được admin chấm Đúng,
**When** admin chốt câu,
**Then** mốc xếp hạng của ghế 1 là **`200 ms`** — mốc của **bản cuối**, không phải `100 ms`; ghế 2 xếp **trước** ghế 1; và **không** phép xếp hạng nào dùng mốc của bản gửi đầu.

**AC-027** — *US*: US-003 · *FR*: FR-024 · *GR*: `GR-015` C2
**Given** một câu Tăng tốc mà ghế 3 đã gửi `"Hà Nội"` tại `100 ms` rồi `"Hanoi"` tại `150 ms`,
**When** thí sinh ghế 3 gửi `"Hà Nội khác"` tại `200 ms`,
**Then** bản được ghi nhận là `"Hà Nội khác"` **và** mốc xếp hạng **cập nhật thành `200 ms`** — nội dung mới thì mốc dời theo; lịch sử giữ cả ba bản.

**AC-028** — *US*: US-003 · *FR*: FR-025 · *GR*: `GR-015` C1
**Given** cùng chuỗi mở đầu như AC-027 — `"Hà Nội"` tại `100 ms`, `"Hanoi"` tại `150 ms`,
**When** thí sinh ghế 3 gửi lại **`"Hà Nội"`** tại `200 ms`,
**Then** bản được ghi nhận là `"Hà Nội"` nhưng mốc xếp hạng **vẫn là `100 ms`** — bản trùng nội dung sau trim **không** dời mốc; thứ hạng của ghế 3 **không tụt** vì thao tác này; và lịch sử vẫn ghi đủ ba lần gửi.

**AC-029** — *US*: US-003 · *FR*: FR-026 · *GR*: `GR-015` C3
**Given** một câu Tăng tốc mà ghế 4 đã gửi `"Hà Nội"` tại `100 ms`,
**When** thí sinh ghế 4 gửi `"   "` tại `150 ms`,
**Then** bản rỗng sau trim **bị bỏ qua**; bản được ghi nhận **vẫn là `"Hà Nội"`**; và mốc xếp hạng **vẫn là `100 ms`**.

**AC-030** — *US*: US-003 · *FR*: FR-026 · *GR*: `GR-015` C4
**Given** một câu Tăng tốc mà ghế 2 gửi `"   "` tại `100 ms` — bản đầu tiên của ghế đó,
**When** thí sinh ghế 2 gửi `"Hà Nội"` tại `150 ms`,
**Then** bản rỗng đầu **bị bỏ qua** nên **không** trở thành mốc; bản được ghi nhận là `"Hà Nội"` và mốc xếp hạng là **`150 ms`**.

**AC-031** — *US*: US-002, US-003 · *FR*: FR-027, FR-088 · *GR*: `NFR-36`
**Given** một câu Tăng tốc đang đếm giờ và ghế 1 vừa bấm gửi,
**When** server ghi nhận bản đó,
**Then** máy thí sinh hiện **trạng thái đang xử lý** trong lúc chờ và **xác nhận đã nhận** khi server trả lời; thí sinh phân biệt được *"đã gửi"* với *"chưa gửi"*; và nút gửi **không** bị vô hiệu hoá trong lúc chờ.

#### Nhóm D — Bảng điểm realtime (US-004)

**AC-032** — *US*: US-004 · *FR*: FR-028 · *GR*: `GR-037` §bảng ba loại thông tin, `GR-036` C5b
**Given** một trận bốn ghế trong đó ghế 4 là **ghế bỏ thi** *(vô hiệu hoá từ trước cú bấm bắt đầu trận, điểm luôn 0)*, điểm hiện tại là `10 · 0 · 20 · 0`,
**When** thí sinh ghế 1 nhìn màn thi đấu của mình,
**Then** bảng điểm hiện **đủ bốn ghế** kèm điểm của từng ghế, gồm cả ghế bỏ thi; **không** ghế nào bị ẩn; và ghế 1 thấy đúng con số mà màn khán giả đang hiện.

**AC-033** — *US*: US-004 · *FR*: FR-029 · *GR*: `GR-028` §Thứ tự đánh giá
**Given** một màn thí sinh và một màn khán giả cùng mở trên cùng mạng LAN, điểm đang là `10 · 0 · 20 · 0`,
**When** admin chấm **Đúng** một câu `10` điểm cho ghế 2,
**Then** cả hai màn đổi ghế 2 từ `0` thành `10`; chênh lệch thời điểm hiển thị giữa hai màn nằm **dưới một khung hình**; và **không** thao tác nào của thí sinh cần thiết để bảng cập nhật.

**AC-034** — *US*: US-004 · *FR*: FR-030 · *GR*: `GR-028` C4 · `INV-002`
**Given** một trận đã chạy xong Vượt chướng ngại vật với điểm `40 · 10 · 20 · 0`,
**When** admin **bỏ vòng** Vượt chướng ngại vật,
**Then** bảng điểm trên **máy thí sinh** hiện điểm **tụt** theo đúng các sự kiện đảo ngược vừa được thêm; **không** sự kiện cũ nào biến mất khỏi nhật ký; và giá trị mới bằng đúng `reduce` của nhật ký sau khi thêm các sự kiện đảo ngược.

**AC-035** — *US*: US-004 · *FR*: FR-031 · *GR*: `GR-028` §Biên · `INV-018`
**Given** ghế 3 đang có `0` điểm ở Khởi động lượt chung,
**When** admin chấm **Sai** cho ghế 3 ở một câu mang hình phạt `−5`, rồi chấm Sai thêm một câu nữa,
**Then** máy thí sinh hiện điểm ghế 3 là **`−5`** rồi **`−10`**; giá trị **không** bị kẹp về `0`; dấu âm hiện rõ; và bảng xếp hạng vẫn xếp ghế 3 bình thường.

**AC-036** — *US*: US-004, US-009 · *FR*: FR-032, FR-068 · *GR*: `GR-037` §bảng ba loại thông tin, `GR-008` §Điều kiện
**Given** một câu Vượt chướng ngại vật hàng ngang đang mở, bốn ghế đều đã gửi bài, admin **chưa** bấm hiển thị bài làm và **chưa** chấm,
**When** thí sinh ghế 1 nhìn màn thi đấu của mình,
**Then** ghế 1 thấy **điểm của cả bốn ghế** *(công khai)*; **không** thấy **đáp án chuẩn** *(mật tới mốc câu khép)*; **không** thấy **bài làm của ba ghế kia** *(ẩn tạm thời)*; và ba chế độ này **không** bị gộp thành một.

#### Nhóm E — Tức thời, không dialog, không rút lại (US-005)

**AC-037** — *US*: US-005 · *FR*: FR-033 · *GR*: `GR-034` C1, `TERM-001`
**Given** một câu Về đích của ghế 2 đã qua `hạn chót` mà ghế 2 **không trả lời**, admin **chưa** bấm gì nên nút chuông của ba ghế còn lại **chưa sống**,
**When** admin bấm **Sai** cho ghế 2 — mốc mở cửa sổ cướp `5` giây, ở **cả hai** mode — rồi thí sinh ghế 4 click chuông,
**Then** nút chuông của ba ghế còn lại sống **đúng tại cú bấm của admin**, không tại `hạn chót` — *"không trả lời"* và *"trả lời sai"* đi qua **cùng một** thao tác; tín hiệu của ghế 4 rời máy **ngay tại cú bấm**, không qua bước trung gian nào; server gắn timestamp và phân xử **ngay** — hàng đợi ở vòng này **không chặn**; và ghế 4 nhận được phản hồi về việc mình có giành được quyền hay không. Lặp lại ở **mode sân khấu** và **Then** luồng cướp quyền chạy y hệt — chỉ khác là ghế 4 **nói** đáp án thay vì gõ.

**AC-038** — *US*: US-005 · *FR*: FR-034 · *GR*: `QĐ-005`
**Given** một máy thí sinh ở mode nhập liệu, rà toàn bộ năm vòng,
**When** kiểm ba đường bấm chuông, *"Mở chướng ngại vật"* và gửi đáp án,
**Then** **0** dialog xác nhận tồn tại trên cả ba đường; mỗi cú bấm phát tín hiệu ngay; và kết quả rà là như nhau ở mode sân khấu.

**AC-039** — *US*: US-005 · *FR*: FR-035 · *GR*: `PRD` §11 EPIC-008 Out of scope
**Given** ghế 1 vừa bấm chuông và tín hiệu đã tới server,
**When** thí sinh ghế 1 tìm cách thu hồi tín hiệu đó,
**Then** máy thí sinh **không có** nút huỷ, rút lại hay thu hồi nào; tín hiệu vẫn nguyên trong hàng đợi và lịch sử; và đường sửa lỗi duy nhất là admin can thiệp.

**AC-040** — *US*: US-005, US-006 · *FR*: FR-036 · *GR*: `GR-007` C7, `STATE-038`
**Given** một trận **mode nhập liệu** đang ở Vượt chướng ngại vật, ghế 1 đang tới lượt chọn hàng ngang,
**When** thí sinh ghế 1 click hàng ngang số 3,
**Then** một **dialog xác nhận hiện trên máy thí sinh** — ngoại lệ duy nhất của quy tắc *không dialog phía thí sinh*; chưa tín hiệu nào rời máy trước khi thí sinh xác nhận; và rà lại toàn bộ màn xác nhận **không** thao tác nào khác của thí sinh có dialog.

**AC-041** — *US*: US-005, US-006 · *FR*: FR-037 · *GR*: `GR-032` §Mục đích, C5 · `INV-006`
**Given** một câu Khởi động lượt chung mà ghế 1 giành quyền và ghế 3 bấm chậm hơn nên tín hiệu của ghế 3 thành **trơ**,
**When** vòng kết thúc,
**Then** **mọi** tín hiệu đã tới server đều có một kết cục — ghế 1 *thực thi*, ghế 3 *trơ*; **0** tín hiệu bị drop; **hàng đợi đang hoạt động** được đặt lại khi đích đóng nhưng **lịch sử tín hiệu giữ nguyên**; admin xem lại được cả hai; và **ghế 3 được báo kết cục đó ngay trên nút chuông của mình** theo FR-005a.

#### Nhóm F — Chọn hàng ngang (US-006)

**AC-042** — *US*: US-006, US-011 · *FR*: FR-038 · *GR*: `GR-007` C6, §Invalid transitions
**Given** một trận **mode sân khấu** đang ở Vượt chướng ngại vật, ghế 1 đang tới lượt chọn,
**When** thí sinh ghế 1 nhìn màn thi đấu của mình và một yêu cầu chọn hàng ngang được gửi thẳng tới server từ phiên của ghế 1,
**Then** máy ghế 1 **không render** nút chọn hàng ngang nào; **0** tín hiệu phát được từ giao diện; **server từ chối** yêu cầu gửi thẳng; và đường vào duy nhất là admin click.

**AC-043** — *US*: US-006 · *FR*: FR-039 · *GR*: `GR-007` §Kích hoạt, §Invalid transitions
**Given** một trận **mode nhập liệu** đang ở Vượt chướng ngại vật, ghế 1 đang tới lượt chọn,
**When** admin tìm nút chọn hàng ngang trên màn điều khiển,
**Then** nút đó **không tồn tại** ở mode này — admin **không** chọn thay; đường vào duy nhất là thí sinh click; và admin chỉ có nút **Yes/No** để duyệt tín hiệu.

**AC-044** — *US*: US-006 · *FR*: FR-040 · *GR*: `GR-032` C1, `GR-007` §Điều kiện
**Given** cùng trạng thái AC-043, ghế 1 vừa xác nhận dialog chọn hàng ngang số 3,
**When** tín hiệu tới server,
**Then** tín hiệu vào **hàng đợi chặn** ở trạng thái **chờ duyệt**; hàng ngang số 3 **chưa** được đưa ra; ô chữ **chưa** chuyển sang *đã hỏi*; câu **chưa** bị tiêu; đồng hồ **chưa** chạy; và mọi hệ quả chỉ xảy ra sau khi admin bấm **Yes**.

**AC-045** — *US*: US-006, US-012 · *FR*: FR-041, FR-042 · *GR*: `GR-007` C7, `STATE-029`
**Given** cùng trạng thái AC-040 — dialog đang hiện trên máy ghế 1,
**When** thí sinh ghế 1 xác nhận dialog rồi click thêm ba lần nữa vào các hàng ngang khác,
**Then** **đúng một** tín hiệu rời máy; nút chọn của ghế 1 chuyển sang trạng thái **khoá chờ duyệt** và mang dấu hiệu nêu rõ đây là khoá **theo luật chơi**; ba lần click sau **không** tạo tín hiệu nào; và hàng đợi chỉ có một dòng của ghế 1.

**AC-046** — *US*: US-006 · *FR*: FR-041, FR-044 · *GR*: `GR-007` C2, `GR-032` C3, `TERM-027` · `INV-008`
**Given** cùng trạng thái AC-045 — tín hiệu của ghế 1 đang chờ duyệt, nút chọn của ghế 1 đang khoá, ô chữ hàng ngang 3 ở trạng thái **chờ**, đồng hồ chưa chạy, kho đề chưa tiêu câu nào,
**When** admin bấm **No**,
**Then** nút chọn của ghế 1 **mở lại** — trạng thái thứ ba; ghế 1 **giữ nguyên lượt chọn** và chọn được hàng khác ngay; **0 tác dụng phụ**: ô chữ hàng ngang 3 **vẫn ở trạng thái chờ**, câu **vẫn chưa tiêu và được trả lại kho**, đồng hồ **vẫn chưa chạy**, cờ *lượt đã dùng* của ghế 1 **chưa** được đặt; tín hiệu bị từ chối **vẫn nằm trong lịch sử**; và tín hiệu kế tiếp trong hàng đợi lên.

**AC-047** — *US*: US-006 · *FR*: FR-042 · *GR*: `GR-007` C7
**Given** cùng trạng thái AC-040 — dialog đang hiện trên máy ghế 1,
**When** thí sinh ghế 1 **huỷ** dialog thay vì xác nhận,
**Then** **không tín hiệu nào rời máy**; nút chọn của ghế 1 vẫn ở trạng thái **sống**; ghế 1 chọn lại được ngay; và hàng đợi không nhận dòng nào.

**AC-048** — *US*: US-006 · *FR*: FR-043 · *GR*: `GR-007` §Ví dụ *không hợp lệ*
**Given** cùng trạng thái AC-044 — ghế 1 đã xác nhận dialog và tín hiệu đang chờ duyệt,
**When** hệ thống chạy tiếp mà admin **chưa** bấm gì,
**Then** hàng ngang số 3 **vẫn chưa được đưa ra** — dialog của thí sinh **không** thay thế phán quyết của admin; hai lớp phục vụ hai mục đích khác nhau; và trạng thái vòng đứng yên cho tới khi admin bấm Yes hoặc No.

**AC-049** — *US*: US-006 · *FR*: FR-044 · *GR*: `GR-007` C2
**Given** cùng trạng thái AC-046 sau khi admin đã bấm **No** và nút của ghế 1 đã mở lại,
**When** thí sinh ghế 1 chọn hàng ngang số 2 và admin bấm **Yes**,
**Then** hàng ngang số 2 được đưa ra; cờ *lượt chọn đã dùng* của ghế 1 **bây giờ** mới được đặt; ô số 2 chuyển sang *đã hỏi*; và lần bị từ chối trước đó **không** làm ghế 1 mất lượt.

**AC-050** — *US*: US-006, US-011 · *FR*: FR-045 · *GR*: `GR-007` C5
**Given** một trận mode nhập liệu ở Vượt chướng ngại vật, hàng ngang số 1 **đã mở**, ghế 2 đang tới lượt chọn,
**When** thí sinh ghế 2 đưa chuột tới hàng ngang số 1 và click,
**Then** hàng ngang số 1 **không được render** như một mục tiêu chọn được; thao tác **không phản hồi**; **0** tín hiệu được tạo; và các hàng chưa mở vẫn click được bình thường.

**AC-051** — *US*: US-006, US-011 · *FR*: FR-046 · *GR*: `GR-007` §Thứ tự đánh giá, C8
**Given** một trận mode nhập liệu ở Vượt chướng ngại vật, ghế 3 **chưa tới lượt** theo khuyến nghị của hệ thống,
**When** máy ghế 3 được dựng,
**Then** bốn phép kiểm — đúng lượt, chưa bị loại, hàng ngang còn chưa mở, lượt chưa dùng — được đánh giá **một lần tại thời điểm dựng màn** như **điều kiện render**; nút chọn của ghế 3 **không render** nên **không tín hiệu nào sinh ra để phải sắp thứ tự**; và nếu admin đã **ép lượt trái luật** ở khâu gán lượt trước đó thì phép kiểm chỉ đổi **giá trị**, không đổi cách đánh giá.

**AC-052** — *US*: US-006, US-005 · *FR*: FR-047 · *GR*: `GR-009` C14, C8
**Given** một trận mode nhập liệu ở Vượt chướng ngại vật, ghế 2 vừa bấm *"Mở chướng ngại vật"* và tín hiệu đang **chờ duyệt**, một câu hàng ngang đang mở và đồng hồ đang chạy,
**When** thí sinh ghế 2 gõ và gửi đáp án cho câu hàng ngang đó,
**Then** thao tác đó **được chấp nhận bình thường** — hệ thống **không** khoá ghế 2 sớm; bản gửi được ghi nhận; và nếu sau đó admin bấm **No** cho tín hiệu Chướng ngại vật thì ghế 2 **không mất gì**.

#### Nhóm G — Ngôi sao hy vọng (US-007)

**AC-053** — *US*: US-007, US-011 · *FR*: FR-048 · *GR*: `GR-021` C7, C8, `EVENT-042`
**Given** một trận **mode sân khấu** đang ở Về đích, câu tiếp theo của ghế 2 **đã rút nhưng chưa hiển thị**, ghế 2 chưa dùng Ngôi sao hy vọng,
**When** thí sinh ghế 2 nhìn màn thi đấu của mình, và một yêu cầu đặt Ngôi sao hy vọng được gửi thẳng tới server từ phiên của ghế 2,
**Then** máy ghế 2 **không render** nút Ngôi sao hy vọng; **server từ chối** yêu cầu gửi thẳng và **ghi một dòng nhật ký thao tác**; cờ *đã dùng* **không** được đặt; và đường vào duy nhất là **admin bấm** sau khi nghe thí sinh nói trên sân khấu.

**AC-054** — *US*: US-007 · *FR*: FR-049 · *GR*: `GR-021` §bảng theo mode, §Thứ tự đánh giá, `GR-032` §Điều kiện
**Given** một trận **mode nhập liệu** đang ở Về đích, câu tiếp theo của ghế 2 đã rút nhưng **chưa hiển thị**, ghế 2 chưa dùng Ngôi sao hy vọng,
**When** thí sinh ghế 2 bấm nút Ngôi sao hy vọng,
**Then** cờ có hiệu lực **ngay**, **không** vào hàng đợi chờ duyệt — Về đích là vòng hàng đợi **không chặn**; nút chuyển sang **disabled**; điểm **không đổi** — đặt cược chưa phải phán quyết; và admin thấy được cờ đã đặt trên màn điều khiển.

**AC-055** — *US*: US-007, US-011 · *FR*: FR-050 · *GR*: `GR-021` C6, `STATE-026`
**Given** cùng trận mode nhập liệu, ghế 3 **chưa** bấm Ngôi sao hy vọng và câu của ghế 3 đã rút,
**When** admin bấm **hiển thị câu**, rồi thí sinh ghế 3 tìm nút Ngôi sao hy vọng,
**Then** cửa sổ đã **đóng** tại mốc hiển thị câu; nút **không còn render** và thao tác **không phản hồi**; **0** tín hiệu được tạo; và **cờ *đã dùng* của ghế 3 vẫn CHƯA được đặt** — ngôi sao còn nguyên cho một lần chạy vòng khác.

**AC-056** — *US*: US-007, US-012 · *FR*: FR-051, FR-086 · *GR*: `GR-021` C3, §Bấm trùng
**Given** ghế 2 vừa đặt Ngôi sao hy vọng thành công ở AC-054 và nút đang disabled,
**When** thí sinh ghế 2 bấm lại, và một yêu cầu trùng được gửi thẳng tới server,
**Then** nút đã disabled nên **không tín hiệu nào rời máy**; nút mang dấu hiệu nêu rõ đây là **khoá theo luật chơi** *(ngôi sao đã dùng)*; **server bỏ qua** yêu cầu trùng; và **0** sự kiện mới được sinh.

**AC-057** — *US*: US-007 · *FR*: FR-052 · *GR*: `GR-021` §Người cướp KHÔNG thể dùng NSHV, `GR-020` §Điều kiện
**Given** một câu Về đích của ghế 1 vừa bị chấm **Sai** nên cửa sổ cướp quyền đang mở, ghế 4 chưa dùng Ngôi sao hy vọng của mình,
**When** thí sinh ghế 4 bấm chuông để cướp quyền rồi tìm nút Ngôi sao hy vọng cho câu đó,
**Then** nút **không tồn tại** cho câu đang cướp — Ngôi sao hy vọng phải đặt **trước khi câu được đọc**; **cờ của ghế 4 vẫn chưa dùng** và còn nguyên cho lượt thi của chính ghế 4; và số học của câu đang cướp theo `GR-020` không đổi.

**AC-058** — *US*: US-007 · *FR*: FR-053 · *GR*: `GR-021` §Điều kiện, `STATE-026` §Ra
**Given** ghế 2 đã dùng Ngôi sao hy vọng trong lần chạy vòng Về đích hiện tại và nút đang disabled,
**When** admin **chạy lại vòng** Về đích,
**Then** cờ *Ngôi sao hy vọng đã dùng* của ghế 2 được **đặt lại**; nút **sống lại** trên máy ghế 2 trong cửa sổ hợp lệ của lần chạy mới; và điểm của các lần chạy trước đã được hoàn nguyên bằng sự kiện đảo ngược, không bị xoá.

#### Nhóm H — Chọn gói câu Về đích (US-008)

**AC-059** — *US*: US-008 · *FR*: FR-054 · *GR*: `GR-017` §Điều kiện, C1, C2
**Given** một trận mode nhập liệu đang ở Về đích, ghế 1 tới lượt, hai mức điểm cấu hình của contest là `{20, 30}`,
**When** thí sinh ghế 1 mở màn chọn gói,
**Then** giao diện đòi **đúng ba mục**, mỗi mục chọn từ **hai mức đã cấu hình**; bốn phối hợp `20/20/20 · 20/20/30 · 20/30/30 · 30/30/30` đều chọn được; và hai con số mức điểm được **đọc từ cấu hình**, không hard-code trong giao diện.

**AC-060** — *US*: US-008, US-011 · *FR*: FR-055 · *GR*: `GR-017` C4
**Given** một trận **mode sân khấu** đang ở Về đích, ghế 1 tới lượt,
**When** thí sinh ghế 1 nhìn màn thi đấu của mình,
**Then** máy ghế 1 **không render** nút chọn gói nào; đường vào duy nhất là **admin bấm theo lời thí sinh nói trên sân khấu**; và **không** tồn tại mốc *"quá hạn"* nào ở nhánh này vì admin vốn là người bấm.

**AC-061** — *US*: US-008, US-011 · *FR*: FR-055 · *GR*: `GR-017` C5, §Invalid transitions
**Given** cùng trạng thái AC-060,
**When** một yêu cầu chọn gói `20/30/30` được gửi thẳng tới server từ phiên của ghế 1,
**Then** **server từ chối** yêu cầu đó; gói của ghế 1 **không đổi**; và một dòng nhật ký được ghi.

**AC-062** — *US*: US-008 · *FR*: FR-056 · *GR*: `GR-017` §bảng theo mode, `EVENT-041`
**Given** cùng trạng thái AC-059,
**When** thí sinh ghế 1 chọn `20/30/30`,
**Then** gói được ghi nhận; server **validate lại** số mục và tập mức bất kể client là ai; và ba câu sẽ được rút theo bộ lọc mức — thứ tự chọn **không** ảnh hưởng thứ tự rút.

**AC-063** — *US*: US-008 · *FR*: FR-057 · *GR*: `GR-017` C3
**Given** một trận **mode nhập liệu** ở Về đích, ghế 2 tới lượt nhưng **chưa chọn gói**,
**When** admin dùng fallback chọn hộ,
**Then** hệ thống đề xuất gói mặc định `20/20/20`; admin **override được** giá trị đó trong dialog; gói được đặt cho ghế 2 và lượt thi **không bị tắc**; **nút chọn trên máy ghế 2 vẫn sống**; và khi ghế 2 sau đó chọn `20/30/30`, giá trị đó **thắng** theo bản-cuối-thắng. Chỉ tại mốc **admin bấm hiển thị câu đầu tiên** nút mới khoá — **không** tồn tại mốc khoá thứ hai nào sinh ra từ cú chọn hộ.

**AC-064** — *US*: US-008, US-012 · *FR*: FR-058, FR-060 · *GR*: `GR-017` C6, §Bấm trùng
**Given** ghế 1 đã chọn `20/20/20` và admin **chưa** bấm hiển thị câu đầu tiên của gói,
**When** thí sinh ghế 1 đổi sang `20/30/30` rồi đổi tiếp sang `30/30/30`,
**Then** cả hai lần đổi **được chấp nhận** — **last-wins**; gói cuối cùng là `30/30/30`; nút chọn **không** bị tắt sau lần chọn đầu — nó **không** phải chuông; và không sự kiện điểm nào được sinh.

**AC-065** — *US*: US-008, US-011 · *FR*: FR-058 · *GR*: `GR-017` C7
**Given** ghế 1 đã chốt gói `30/30/30` và admin vừa bấm **hiển thị câu đầu tiên** của gói,
**When** thí sinh ghế 1 thử đổi gói,
**Then** nút chọn **đã khoá** — mốc khoá là cú bấm hiển thị câu đầu, vì từ đó **đề đã rời server**; **0** tín hiệu được tạo; và gói vẫn là `30/30/30`.

**AC-066** — *US*: US-008 · *FR*: FR-059 · *GR*: `GR-017` C8
**Given** một trận mode nhập liệu ở Về đích với hai mức cấu hình `{20, 30}`, ghế 3 đang chọn gói,
**When** thí sinh ghế 3 thử gửi gói `20/20` *(thiếu một mục)*, rồi thử gửi gói `20/40/20` *(mức ngoài tập cấu hình)* — cả hai qua giao diện và qua yêu cầu gửi thẳng tới server,
**Then** giao diện **từ chối** cả hai trước khi gửi; server **cũng từ chối** cả hai khi nhận yêu cầu gửi thẳng; gói của ghế 3 **không đổi**; và **0** sự kiện được sinh.

#### Nhóm I — Đáp án chỉ tới máy thí sinh từ mốc câu khép (US-009)

**AC-067** — *US*: US-009 · *FR*: FR-061 · *GR*: `GR-037` C4 · `INV-017`
**Given** một trận có cờ *hiện đáp án sau khi chấm* **BẬT**, một câu Khởi động lượt riêng đang đếm giờ và **chưa** được chấm,
**When** bắt gói tin trên kênh của máy thí sinh trong suốt thời gian đó,
**Then** **0 byte** mang đáp án chuẩn rời server tới máy thí sinh; máy thí sinh **không** giữ `PERM-045` nên không có đường yêu cầu đáp án; và một yêu cầu đọc đáp án gửi thẳng từ phiên của thí sinh bị server **im lặng từ chối**.

**AC-068** — *US*: US-009 · *FR*: FR-062 · *GR*: `GR-037` §Cấm
**Given** cùng trạng thái AC-067,
**When** kiểm toàn bộ dữ liệu mà máy thí sinh đã nhận từ đầu trận tới thời điểm đó,
**Then** đáp án của **bất kỳ** câu nào — kể cả câu chưa hỏi — **không** có mặt trong bộ nhớ máy thí sinh; hệ thống **không** dùng cách đẩy trước rồi ẩn bằng một cờ hiển thị; và server chỉ đẩy **tại đúng mốc câu khép**.

**AC-069** — *US*: US-009 · *FR*: FR-063 · *GR*: `GR-037` C5
**Given** cùng câu như AC-067 với cờ **BẬT**,
**When** admin bấm chấm và câu **khép**,
**Then** server đẩy đáp án tới máy thí sinh; đáp án hiện trên màn của cả bốn ghế; đây là mốc **đầu tiên** đáp án rời server tới nhóm này; và cùng lúc đó khán giả và lớp phủ dựng stream cũng nhận.

**AC-070** — *US*: US-009 · *FR*: FR-064 · *GR*: `GR-037` C3
**Given** một trận có cờ *hiện đáp án sau khi chấm* **TẮT**, một câu Khởi động đã được chấm và đã khép,
**When** bắt gói tin trên kênh của máy thí sinh sau mốc khép,
**Then** đáp án **không** tới máy thí sinh ở bất kỳ thời điểm nào; chỉ phiên giữ `PERM-045` xem được; và hành vi này **đổi được cho từng trận** bằng một thao tác.

**AC-071** — *US*: US-009 · *FR*: FR-065 · *GR*: `GR-037` C6, `GR-020` §Cấm
**Given** một trận Về đích có cờ **BẬT**, câu `30` điểm của ghế 1 vừa bị admin chấm **Sai** nên **cửa sổ cướp quyền `5` giây đang mở**,
**When** bắt gói tin trên kênh máy thí sinh trong suốt `5` giây đó,
**Then** **0 byte** mang đáp án rời server — câu **chưa khép**; ghế 4 bấm chuông và cướp quyền mà **không** đọc được đáp án; và chỉ **sau khi** admin chấm xong ghế 4 thì đáp án mới tới.

**AC-072** — *US*: US-009 · *FR*: FR-065 · *GR*: `GR-037` §bảng mốc câu khép
**Given** cùng trận Về đích với cờ **BẬT**, câu `20` điểm của ghế 2 đang mở,
**When** admin chấm **Đúng** cho ghế 2,
**Then** câu **khép ngay** — cửa sổ cướp quyền **không** mở khi người thi chính đúng; và đáp án tới máy thí sinh tại mốc đó. Lặp lại với ca *"chấm Sai rồi hết `5` giây không ai bấm"* và **Then** câu khép tại mốc cửa sổ đóng và đáp án tới lúc đó.

**AC-073** — *US*: US-009 · *FR*: FR-066 · *GR*: `GR-037` C7
**Given** một câu Khởi động lượt chung có cờ **BẬT**, hết cửa sổ chuông `3` giây mà **không ai bấm** nên câu **bị bỏ qua**,
**When** cửa sổ chuông đóng,
**Then** câu vẫn **khép** và đáp án **vẫn được công bố** tới máy thí sinh — câu đã hỏi, đã tiêu, giữ kín không bảo vệ được gì; và câu **không** quay lại kho.

**AC-074** — *US*: US-009 · *FR*: FR-066 · *GR*: `GR-037` C8
**Given** một câu Khởi động lượt chung có cờ **BẬT** mà admin vừa chấm **Huỷ kết quả** cho người giữ quyền,
**When** phán quyết được ghi nhận,
**Then** hệ thống **không tự công bố** đáp án tới máy thí sinh; admin vẫn mở tay được nếu muốn; và quy tắc này phát biểu theo **loại phán quyết**, áp cả khi *Huỷ kết quả* là phán quyết khép câu cho người cướp quyền Về đích.

**AC-075** — *US*: US-009 · *FR*: FR-066 · *GR*: `GR-037` C9
**Given** một trận đang ở Vượt chướng ngại vật với cờ **BẬT**, chưa ai giải đúng Chướng ngại vật,
**When** admin chấm xong một câu hàng ngang,
**Then** **đáp án của hàng ngang đó** được công bố theo cơ chế này; nhưng **đáp án Chướng ngại vật KHÔNG** đi theo nó — nó chỉ lộ khi có người giải đúng hoặc khi admin bấm công bố.

**AC-076** — *US*: US-009 · *FR*: FR-067 · *GR*: `GR-037` C8b, `GR-032` §Kích hoạt tay
**Given** một câu Khởi động lượt chung có cờ **BẬT**, người giữ quyền vừa bị chấm **Huỷ kết quả**, hàng đợi còn một tín hiệu trơ và admin **đang chờ kích hoạt tay**,
**When** bắt gói tin trên kênh máy thí sinh trong khoảng chờ đó,
**Then** **0 byte** mang đáp án rời server — mốc câu khép đã **lùi** tới sau khi người được kích hoạt được chấm; công bố ở đây sẽ xoá cơ hội của người sắp được kích hoạt; và đáp án chỉ tới sau khi ghế được kích hoạt đã được chấm.

**AC-077** — *US*: US-009, US-004 · *FR*: FR-068 · *GR*: `GR-008` C9, §Đồng thời
**Given** một câu Vượt chướng ngại vật hàng ngang đang đếm giờ, ghế 1, 3 và 4 đã gửi bài,
**When** thí sinh ghế 2 bấm *"Mở chướng ngại vật"* giữa chừng,
**Then** tín hiệu được ghi nhận ngay và đồng hồ **vẫn chạy**; ghế 2 **không** thấy đáp án chuẩn của hàng ngang; ghế 2 **không** thấy bài làm của ba ghế kia; và **cả hai** nguồn dữ kiện đó phải cùng ẩn — ẩn một cái mà lộ cái kia thì cơ chế vẫn hỏng.

#### Nhóm J — Khôi phục sau mất kết nối (US-010)

**AC-078** — *US*: US-010 · *FR*: FR-069 · *GR*: `GR-036` §Điều kiện, §Biên
**Given** một trận đang ở Tăng tốc, ghế 3 đang kết nối bình thường, ngưỡng chờ cấu hình ở mặc định `120` giây,
**When** server phát hiện ghế 3 mất kết nối tại `T`,
**Then** một cửa sổ chờ mở từ `T`, đo bằng **đồng hồ server**; ghế 3 **được giữ** trong trận; nối lại tại **đúng `T + 120,000` giây** vẫn **trong** ngưỡng — biên đóng; và nối lại sau mốc đó thì ghế chuyển sang trạng thái *quá ngưỡng, chờ admin*.

**AC-079** — *US*: US-010 · *FR*: FR-070 · *GR*: `GR-036` C1, `STATE-034`
**Given** cùng trạng thái AC-078 ngay sau khi mất kết nối,
**When** máy ghế 3 phát hiện mất kết nối rồi nối lại được ở giây thứ `40`,
**Then** máy ghế 3 hiện banner **"đang kết nối lại"** trong suốt `40` giây đó; khi nối lại, banner tắt và hệ thống báo **đã kết nối lại**; và trạng thái ghế được khôi phục.

**AC-080** — *US*: US-010 · *FR*: FR-071 · *GR*: `GR-036` C8, `STATE-023` §Gói khôi phục
**Given** một câu Vượt chướng ngại vật mở lúc `0s` với `hạn chót` `15s`; ghế 3 đã gửi `"Sông Hồng"` lúc `3s`, mất kết nối lúc `4s`, ghế 1 đã bị loại khỏi vòng, một miếng ghép đã mở, điểm là `10 · 0 · 20 · 0`, lớp công bố kết quả đang tắt,
**When** ghế 3 nối lại lúc `9s`,
**Then** gói khôi phục mang: vòng và giai đoạn hiện tại · câu đang mở kèm media và mức điểm · **`hạn chót` `15s` theo server time** · **bản `"Sông Hồng"` của chính ghế 3** · các cờ ghế còn hiệu lực của ghế 3 · trạng thái chuông theo luật · bàn cờ với miếng ghép đã mở · **điểm công khai của cả bốn ghế** · các lớp phủ đang bật.

**AC-081** — *US*: US-010 · *FR*: FR-072 · *GR*: `GR-036` C8, §Ví dụ · `INV-004`
**Given** cùng kịch bản AC-080,
**When** ghế 3 nối lại lúc `9s`,
**Then** máy ghế 3 hiện **còn `6` giây**, dựng từ **`hạn chót` theo server time** chứ không phải từ một giá trị *"còn 15 giây"*; đồng hồ **không** được đặt lại; mất kết nối **không mua thêm thời gian**; và sai số so với đồng hồ server dưới `200 ms`.

**AC-082** — *US*: US-010, US-009 · *FR*: FR-073 · *GR*: `GR-036` C8, `STATE-023` §Ba ràng buộc · `INV-017`
**Given** cùng kịch bản AC-080, ba ghế còn lại đã gửi bài cho câu đang mở,
**When** ghế 3 nối lại và nhận gói khôi phục,
**Then** gói **không chứa đáp án chuẩn** của câu đang mở hay của bất kỳ câu nào; gói **không chứa bài làm của ba ghế kia**; và khôi phục **không** trở thành một đường vòng để lộ dữ liệu.

**AC-083** — *US*: US-010 · *FR*: FR-074 · *GR*: `QĐ-046`, `STATE-023`
**Given** nhật ký sự kiện của trận có `N` dòng ngay trước khi ghế 3 nối lại,
**When** ghế 3 nối lại và nhận gói khôi phục, rồi lặp lại chu trình ngắt-nối `20` lần,
**Then** nhật ký sự kiện **vẫn có đúng `N` dòng** — khôi phục là phép **đọc**; **0** sự kiện được sinh; trạng thái trận không đổi; và lịch sử mất-và-nối-lại vẫn được ghi riêng để admin xem.

**AC-084** — *US*: US-010 · *FR*: FR-075 · *GR*: `GR-036` §Nối lại nhiều lần
**Given** ghế 3 đã mất kết nối `100` giây rồi nối lại thành công,
**When** ghế 3 mất kết nối lần thứ hai,
**Then** một cửa sổ chờ **mới** mở, tính lại **từ đầu** `120` giây; **không** cộng dồn `100` giây của lần trước; **không** giới hạn số lần mất kết nối; và mọi lần mất và nối lại đều vào lịch sử, **không bao giờ xoá**.

**AC-085** — *US*: US-010 · *FR*: FR-076 · *GR*: `GR-036` C4 · `INV-016`
**Given** một câu Tăng tốc `20` giây đang đếm giờ với bốn ghế,
**When** ghế 3 mất kết nối ở giây thứ `5` và vẫn chưa nối lại ở giây thứ `20`,
**Then** đồng hồ của câu **không dừng**; vòng **chạy tiếp bình thường**; ba ghế còn lại tiếp tục gửi bài và được xếp hạng bình thường; và ghế 3 được xử **y như ghế không trả lời** khi admin chốt câu.

**AC-086** — *US*: US-010 · *FR*: FR-076 · *GR*: `GR-036` C10
**Given** một câu đang mở,
**When** ghế 2 mất kết nối tại `T` và ghế 4 mất kết nối tại `T + 30s`,
**Then** hai cửa sổ chờ chạy **độc lập** — của ghế 2 hết tại `T + 120s`, của ghế 4 hết tại `T + 150s`; **không** cửa sổ nào ảnh hưởng cửa sổ kia; và banner hiện riêng trên từng máy.

**AC-087** — *US*: US-010 · *FR*: FR-077 · *GR*: `GR-036` C2, `STATE-024`
**Given** ghế 3 đã quá ngưỡng `120` giây mà chưa nối lại và admin chưa làm gì,
**When** hệ thống chạy tiếp,
**Then** hệ thống **chỉ tô nổi bật** ghế 3 trên màn admin kèm thời lượng; **không tự loại, không tự xoá, không tự vô hiệu hoá**; ghế 3 **giữ nguyên điểm và nguyên vị trí** trong trận và vẫn hiện trên bảng điểm của mọi máy thí sinh; và ghế 3 ở lại trạng thái này cho tới khi nối lại được hoặc admin can thiệp.

**AC-088** — *US*: US-010 · *FR*: FR-078 · *GR*: `STATE-023` §Thứ không khôi phục được
**Given** cùng kịch bản AC-080; ngoài bản đã gửi lúc `3s`, ghế 3 còn đang gõ dở chuỗi `"Sông Đ"` trong ô nhập lúc `4s` mà **chưa bấm gửi**,
**When** ghế 3 nối lại lúc `9s`,
**Then** ô nhập hiện **rỗng** — chuỗi gõ dở **không** được khôi phục vì nó chưa từng tới server; bản `"Sông Hồng"` gửi lúc `3s` **còn nguyên**; và giao diện **không** tạo ảo giác rằng chuỗi gõ dở đã được lưu.

#### Nhóm K — Invalid state và `Esc` (US-011)

**AC-089** — *US*: US-011 · *FR*: FR-079 · *GR*: §Invalid transitions, `GR-034` C6, `QĐ-004` · `INV-006`
**Given** một trận mode **sân khấu** ở Vượt chướng ngại vật, ghế 3 đang tới lượt chọn hàng ngang; và ở một kịch bản thứ hai, một câu Khởi động lượt chung mà **cửa sổ chuông đã đóng**,
**When** thí sinh ghế 3 tìm nút chọn hàng ngang, và ở kịch bản hai thí sinh tìm nút chuông,
**Then** cả hai control **không được render** — chúng **không có nghĩa ở pha hiện tại**; thao tác **không phản hồi**; **0** sự kiện được tạo; và đây là hạng *không có tín hiệu*, **không** phải một tín hiệu bị server drop.

**AC-089a** — *US*: US-011, US-012 · *FR*: FR-079, FR-086 · *GR*: `GR-034` C5, C6, `PRD` §9.4
**Given** một trận mode nhập liệu ở Vượt chướng ngại vật với bốn ghế; ghế 3 **chưa tới lượt** chọn hàng ngang, ghế 2 **đã dùng** lượt chọn của mình, ghế 1 có tín hiệu **đang chờ admin duyệt**, và ghế 4 **bị loại khỏi vòng**,
**When** dựng màn thi đấu của cả bốn ghế,
**Then** ghế 3, ghế 2 và ghế 1 đều **thấy** nút chọn ở trạng thái vô hiệu hoá **kèm nhãn nêu lý do**, và ba nhãn **khác nhau** — *chưa tới lượt* · *đã dùng lượt chọn* · *đang chờ duyệt*; ghế 4 **không thấy** nút nào vì cờ *bị loại* làm control mất nghĩa ở pha này; **0** tín hiệu được tạo ở cả bốn máy; và **0** nhãn nào bị nhầm với trạng thái **đang xử lý**.

**AC-090** — *US*: US-011 · *FR*: FR-080 · *GR*: §Invalid transitions, `STATE-025`, `STATE-040`
**Given** ghế 2 đang bị **vô hiệu hoá**, và ở một kịch bản thứ hai **banner tạm dừng đang bật** cho cả trận,
**When** một yêu cầu bấm chuông của ghế 2 được gửi **thẳng tới server**, bỏ qua giao diện, ở cả hai kịch bản,
**Then** **server từ chối** ở cả hai — server enforce, **không tin client**; tín hiệu vào **lịch sử** với kết cục **TRƠ**, **không bị drop**; điểm và trạng thái câu **không đổi**; và giao diện của ghế 2 vốn đã bị chặn toàn bộ.

**AC-090a** — *US*: US-004, US-010, US-011 · *FR*: FR-029, FR-076a · *GR*: `STATE-040` §Vào, §Cho phép, §Ra · `QĐ-050`, `QĐ-115` · `INV-021`
**Given** một trận đang ở giữa hai câu của vòng Tăng tốc — câu trước đã chấm xong, câu sau **chưa hiển thị**, nên **không cửa sổ thời gian nào đang đếm**; điểm bốn ghế là `10 · 0 · 20 · −5`; máy thí sinh đang hiện bảng điểm bình thường,
**When** admin bật **banner tạm dừng**, rồi trong lúc banner bật điều chỉnh điểm ghế 2 thêm `+5`, rồi tắt banner,
**Then** ngay khi banner bật, màn thí sinh **bị che toàn bộ** — **không** thấy bảng điểm, **không** thấy câu hỏi; mọi thao tác của thí sinh **bị server từ chối** và tín hiệu lỡ tới vào lịch sử với kết cục **TRƠ**; màn **admin không bị phủ** và admin điều chỉnh điểm được bình thường. Khi banner tắt: máy thí sinh hiện ngay điểm ghế 2 là **`5`** — **trạng thái hiện tại**, không phải `0` như lúc banner bật — và **0** bước đồng bộ lại nào được yêu cầu. Xác nhận thêm rằng nút mở banner **không bật** khi thử ở một trạng thái có đồng hồ đang đếm *(`STATE-040` §Vào)*, nên tổ hợp *banner × đồng hồ đang chạy* **không dựng được**.

**AC-091** — *US*: US-011 · *FR*: FR-081, FR-083 · *GR*: `NFR-39`, `QĐ-072`
**Given** một thí sinh đang ở **màn thi đấu** trong chế độ toàn màn hình, mode sân khấu, một câu đang đếm giờ,
**When** thí sinh nhấn `Esc`,
**Then** hệ thống **không** quay lại màn trước, **không** về hub, **không** rời trận; màn thi đấu **không** có nút quay lại — đây là **ngoại lệ tường minh** của quy tắc *mọi màn có nút Back*; và **không** phím tắt nào khác tạo ra một đường rời trận.

**AC-092** — *US*: US-011 · *FR*: FR-082 · *GR*: `GR-034` §Điều kiện
**Given** một trận **mode nhập liệu** ở Khởi động, ô nhập của ghế 1 đang có tiêu điểm và chứa chuỗi `"Hà Nộ"` chưa gửi,
**When** thí sinh ghế 1 nhấn `Esc`,
**Then** ô nhập **bị xoá** thành rỗng; màn hình **không đổi**; chế độ toàn màn hình **không** bị thoát; **0** bản gửi nào được phát đi; và bản được ghi nhận trước đó **không đổi**.

**AC-093** — *US*: US-011 · *FR*: FR-082 · *GR*: `PRD-REQ-071`
**Given** một trận **mode sân khấu** ở Khởi động — không tồn tại ô nhập ở vòng này,
**When** thí sinh nhấn `Esc`,
**Then** hệ thống **không làm gì**; màn hình không đổi; không rời trận. Chuyển sang Vượt chướng ngại vật của cùng trận đó — vòng **luôn gõ máy** nên ô nhập **có** tồn tại — và **Then** `Esc` xoá ô nhập bình thường.

#### Nhóm L — Không chặn gửi lại; khoá theo luật chơi (US-012)

**AC-094** — *US*: US-012 · *FR*: FR-084 · *GR*: `CLAUDE.md` §UX, `QĐ-060`
**Given** một trận mode nhập liệu ở Khởi động, câu đang đếm giờ, mạng có độ trễ đáng kể,
**When** thí sinh bấm nút **gửi đáp án** 10 lần liên tiếp trong 1 giây,
**Then** nút **chỉ hiện trạng thái đang xử lý** và **không** bị vô hiệu hoá; cả 10 lần bấm đều gửi được; server nhận **bản cuối cùng** trước hạn; và **không** lần bấm nào bị giao diện chặn.

**AC-095** — *US*: US-012 · *FR*: FR-085 · *GR*: `GR-028`, `GR-032` §Bấm trùng · `INV-002`
**Given** cùng kịch bản AC-094 với 10 yêu cầu đã tới server,
**When** server xử lý cả 10,
**Then** kết quả trận **không** bị nhân lên — bản được ghi nhận là **một**; chống trùng và tính idempotent do **server** đảm nhiệm qua nhật ký sự kiện; và giao diện **không** phải chịu trách nhiệm đó.

**AC-096** — *US*: US-012 · *FR*: FR-086 · *GR*: `GR-034` C5, `STATE-028`
**Given** ghế 1 đã bấm chuông ở một câu Khởi động lượt chung và đã bị chấm **Sai** cho câu đó, câu vẫn chưa chuyển,
**When** thí sinh ghế 1 nhìn nút chuông,
**Then** nút **vẫn được render**, ở trạng thái vô hiệu hoá **kèm nhãn nêu lý do** — đây là **khoá theo luật chơi**, không phải chống gửi trùng và không phải ẩn control; nhãn phân biệt được với trạng thái **đang xử lý**; và **0** tín hiệu được tạo nếu vẫn bấm. Đối chiếu với ca **ngoài cửa sổ chuông** ở AC-006: ở đó nút **không** được render — hai kết cục khác nhau cho hai lý do khác nhau.

**AC-097** — *US*: US-012 · *FR*: FR-086 · *GR*: `STATE-026`, `STATE-027`
**Given** ghế 2 đã dùng Ngôi sao hy vọng, và ghế 3 đã dùng lượt chọn hàng ngang của mình,
**When** hai thí sinh nhìn hai nút tương ứng,
**Then** cả hai nút ở trạng thái vô hiệu hoá kèm dấu hiệu **khoá theo luật chơi**; hai lý do khoá phân biệt được với nhau qua nội dung chữ; và không nút nào bị nhầm thành trạng thái đang xử lý.

**AC-098** — *US*: US-012 · *FR*: FR-087 · *GR*: `PRD-REQ-072` §Acceptance intent
**Given** toàn bộ các nút một chiều phía thí sinh — chuông, *"Mở chướng ngại vật"*, xác nhận chọn hàng ngang, Ngôi sao hy vọng,
**When** rà quy ước giao diện của chúng,
**Then** cả bốn dùng **một thành phần dùng chung** kèm ghi chú phân loại rõ hai loại khoá; **0** nút nào bị disable vì lý do chống gửi trùng; và ghi chú đủ để một lần rà về sau **không gỡ nhầm** khoá theo luật chơi.

#### Nhóm M — Ràng buộc xuyên suốt

**AC-099** — *US*: US-002, US-003, US-012 · *FR*: FR-088 · *GR*: `NFR-36`
**Given** một thí sinh thực hiện lần lượt mọi thao tác bất đồng bộ có trên màn thi đấu,
**When** mỗi thao tác được phát đi,
**Then** giao diện hiện trạng thái đang xử lý rõ ràng trong lúc chờ; có phản hồi thành công hoặc thất bại sau mỗi thao tác; **0** thao tác nào để giao diện im lặng; và độ trễ cảm nhận được của phản hồi là không đáng kể.

**AC-100** — *US*: US-004, US-011 · *FR*: FR-089 · *GR*: `NFR-37`
**Given** màn thi đấu của thí sinh trên khung nhìn tham chiếu của dự án, ở cả hai mode và ở cả năm vòng,
**When** trang vừa tải xong,
**Then** **0** phần tử thao tác bắt buộc nằm dưới mép dưới — không phải cuộn để bấm chuông, gửi đáp án hay xem bảng điểm; bố cục **không nhồi nhét** và giữ được khoảng thở; và con số kích thước khung nhìn là một quyết định thiết kế, **không** được khai như một requirement chuẩn tắc.

**AC-101** — *US*: US-004, US-010 · *FR*: FR-090 · *GR*: `NFR-38`
**Given** màn thi đấu của thí sinh,
**When** rà toàn bộ chuỗi giao diện và mọi mốc thời gian hiển thị,
**Then** **100%** chuỗi là tiếng Việt và **0** chuỗi hard-code trong mã giao diện — chúng nằm ở file hằng số dùng chung; **100%** mốc thời gian hiển thị theo **UTC+7**; và không có thiết lập múi giờ theo người dùng.

**AC-102** — *US*: US-009 · *FR*: FR-091 · *GR*: `CLAUDE.md` §Quy ước code · `INV-017`
**Given** một câu có media, chưa tới mốc reveal,
**When** máy thí sinh preload media của câu đó,
**Then** media nằm trên máy ở dạng **mã hoá** và **không** giải mã được trước mốc; khoá được phát đúng lúc reveal theo **server time**; khi cơ chế preload không khả dụng, hệ thống lui về đường **reveal-only** mà **không** đẩy media trước; và không nhánh nào để media giải mã được nằm sẵn trên máy trước mốc.

## Assumptions

Những giả định dưới đây là **giá trị mặc định hợp lý cho chi tiết KHÔNG quan trọng** theo Nguyên tắc III của constitution. Chi tiết quan trọng mà nguồn im lặng đã được đánh dấu ở §Open Questions thay vì đặt vào đây.

- **Máy thí sinh là một trình duyệt hiện đại có chuột hoặc thiết bị trỏ tương đương.** Yêu cầu *"chuông chỉ nhận click chuột"* giả định tồn tại một thiết bị trỏ; nguồn không nói tới cảm ứng, và spec này không mở rộng sang đó.
- **Máy thí sinh chạy ở chế độ toàn màn hình trong trận.** Đây là tiền đề của lập luận `QĐ-072` về `Esc`, tuy nguồn không phát biểu nó thành một requirement riêng.
- **Một thí sinh ngồi đúng một ghế trong một trận, và một phiên đăng nhập tương ứng đúng một ghế.** Nguồn mô tả *"ghế"* như đơn vị của mọi cờ và mọi tín hiệu; ca một người điều khiển hai ghế không được nguồn nhắc tới và nằm ngoài spec này.
- **Cấu hình mode trả lời và cờ *hiện đáp án sau khi chấm* đã được chốt trước cú bấm bắt đầu trận** và **không đổi giữa trận** — mode ở cấp contest, cờ được chụp vào trận lúc start. Spec này không đặc tả hành vi khi hai giá trị đó đổi giữa chừng.
- **Ngưỡng chờ kết nối `120` giây, mức điểm gói Về đích `{20, 30}`, độ dài các cửa sổ chuông và mọi mốc thời gian khác đều là RuleConfig cấu hình được**, và các con số trong spec này là **giá trị của preset `O26_DEFAULT@1`** chứ không phải hằng số của hệ thống.
- **Bảng điểm trên máy thí sinh và trên màn khán giả đọc cùng một nguồn dữ liệu** — cùng kết quả `reduce` của nhật ký sự kiện. Yêu cầu *"cập nhật cùng lúc"* được hiểu là cùng nguồn và cùng nhịp phát, không phải một cơ chế đồng bộ hoá riêng giữa hai màn.
- **Trận chạy trên mạng LAN của phòng thi** trong hồ sơ triển khai portable, nên ngưỡng *"dưới một khung hình"* của SC-006 được đo trên mạng đó, không phải qua Internet công cộng.
