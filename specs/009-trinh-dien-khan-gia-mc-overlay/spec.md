# Feature Specification: Trình diễn — khán giả · lớp phủ · MC · chủ đề · âm thanh

**Feature Branch**: `009-trinh-dien-khan-gia-mc-overlay`

**Created**: 2026-08-04

**Status**: Draft

**Input**: EPIC-009 — Trình diễn: khán giả · lớp phủ · MC · chủ đề · âm thanh (`docs/PRD.md` §11 dòng 498-509, §12 mục EPIC-009)

**Nguồn**: `docs/PRD.md` (EPIC-009 §11; PRD-REQ-073 → PRD-REQ-078 §12 mục EPIC-009; PRD-REQ-041, PRD-REQ-049, PRD-REQ-107 — ba requirement khai **Related epic** gồm EPIC-009; §9.1 → §9.5; JOURNEY-004, JOURNEY-005 §10; §14 FS-04, FS-19, FS-31, FS-32, FS-33; §15.1 NFR-06, §15.4 NFR-20, NFR-21, NFR-21b, §15.8 NFR-36 → NFR-39; §18 RISK-005; ma trận truy nguyên §22) · `docs/game-rules.md` (`GR-037` — rule mà epic khai trực tiếp; và các rule mà PRD-REQ của epic này tham chiếu: `GR-004`, `GR-016`, `GR-025`, `GR-028`, `GR-030`, `GR-035`; `GR-012` ở đúng vế **hiện Chướng ngại vật cho khán giả**) · `docs/game-state-machine.md` (§F Cấp LỚP PHỦ; `STATE-033`, `STATE-034`, `STATE-039`, `STATE-040`; `EVENT-032` → `EVENT-035`, `EVENT-051`; `T-087` → `T-090`, `T-092` → `T-095`; §Invalid transitions; `INV-016` → `INV-018`, `INV-021`) · `docs/decisions.md` (`QĐ-049`, `QĐ-050`, `QĐ-051`, `QĐ-062`, `QĐ-067`, `QĐ-076`, `QĐ-079`, `QĐ-080`, `QĐ-088`, `QĐ-093`, `QĐ-096`, `QĐ-102`, `QĐ-115`) · `docs/permissions.md` (`PERM-023`, `PERM-044`, `PERM-045`, `PERM-047`, `PERM-048`, `PERM-049`, `PERM-054`; §8 kênh public) · `docs/glossary.md` (`TERM-005`, `TERM-007`, `TERM-017`, `TERM-018`, `TERM-057`) · `docs/traceability.md` · `.specify/memory/constitution.md` bản 1.4.0 · `CLAUDE.md` §UX, §Quy ước code.

**Lưu ý về đầu vào**: lệnh gọi yêu cầu đọc `docs/rule-traceability.md` và `docs/reviews/prd-review.md` — **cả hai đường dẫn không tồn tại** trong repo. Tương ứng gần nhất của cái thứ nhất là `docs/traceability.md` (đã dùng). Không có tài liệu nào thay thế cái thứ hai; `docs/reviews/` chỉ chứa hồ sơ rà **luật chơi**, và theo `.specify/memory/constitution.md` §*Nguồn đã migrate xong* thì `docs/reviews/**` **không thoả cổng truy nguyên** nên dù có cũng không dùng làm nguồn requirement. Đây là chênh **đường dẫn đầu vào**, không phải `CONFLICT` nghiệp vụ, nên không ghi vào §Open Questions. Cùng cách xử lý đã dùng ở `specs/005` → `specs/008`.

**Quy ước truy nguyên của feature này** — bốn điểm cần biết trước khi đọc bảng truy nguyên:

1. **Requirement khai nhiều epic vẫn thuộc epic này.** PRD-REQ-041, PRD-REQ-049 và PRD-REQ-107 đều khai `EPIC-009` trong trường **Related epic**. Spec này chỉ đặc tả **mặt trình diễn** của chúng — cái gì hiện trên màn khán giả, lớp phủ dựng stream và màn MC. **Số học điểm, mô hình sự kiện và hàng rào server** thuộc `specs/006`; **cú bấm của admin** thuộc `specs/007`; **mã phòng, URL vào phòng và catalog permission** thuộc `specs/001`. Không thứ nào trong ba nhóm đó được đặc tả lại ở đây (xem §Out of Scope).
2. **Ba kênh, ba mô hình truy cập khác nhau.** *Màn khán giả* và *lớp phủ dựng stream* là **public, một chiều, không tài khoản** (`TERM-007`, `QĐ-088`, `QĐ-096`). *Màn MC* là **kênh có xác thực**, hai chiều, mang vai *MC* với đúng một permission ghi (`PERM-054`). Trộn ba kênh này thành *"màn hiển thị"* là lỗi — chúng khác nhau ở cả chiều truyền lẫn phạm vi đáp án.
3. **`docs/decisions.md` là nguồn hợp lệ và được dùng.** `CLAUDE.md` §1 khai nó là *"nơi duy nhất ghi vì sao"* và nó nằm trong `docs/`, nên nó thoả Cổng 1 của constitution. Hai quyết định gần đây chi phối trực tiếp epic này: **`QĐ-096`** *(hai kênh public vào bằng **đúng một URL**, mã phòng nằm trong URL, không có bước nhập mã riêng)* và **`QĐ-102`** *(nút khoá cổng áp **chỉ** cho màn khán giả; lớp phủ luôn vào được)*.
4. **Chuẩn tắc từ ngữ.** *"Overlay"* và *"lớp phủ dựng stream"* là **một thứ** — frame stream cho phần mềm dựng hình (`TERM-007` §Đừng nhầm với). Chữ *"lớp phủ"* trần trụi trong `game-state-machine.md` §F lại là **thang bậc trạng thái** `STATE-033` → `STATE-040`. Spec này dùng **`lớp phủ dựng stream`** cho kênh, và **`lớp hiển thị`** cho thang bậc trạng thái, để hai nghĩa không đè lên nhau.

## Phạm vi

EPIC-009 phủ **toàn bộ bề mặt mà một người KHÔNG thi đấu nhìn thấy trong một trận đang chạy**: màn khán giả public vào bằng một URL, lớp phủ 1920×1080 nền trong suốt cho phần mềm dựng hình, màn MC chữ rất to có đáp án và read-only trừ đúng một prompt, lớp hiển thị công bố kết quả với thứ hạng do server tính, banner tạm dừng không chữ chặn toàn cục, chính sách *"khán giả không được báo về can thiệp của admin"*, và chủ đề cùng các khe âm thanh phủ cả sự kiện điều khiển.

Actor chính: **ACTOR-005 khán giả** · **ACTOR-006 máy dựng stream** · **ACTOR-004 MC**. Journey: **JOURNEY-004 — Vào phòng**, **JOURNEY-005 — Thi đấu**.

**Mục tiêu của epic** (`docs/PRD.md` §11): *"Trận lên hình được và khán giả theo dõi được"*, với **user value** là *"giải `PS-4` — tích hợp livestream mà không cần dựng thủ công"*.

**Không thuộc phạm vi feature này** (xem §Out of Scope): số học điểm và mô hình sự kiện (EPIC-006); mọi cú bấm của admin sinh ra các lớp hiển thị này (EPIC-007); màn thi đấu của thí sinh (EPIC-008); xác thực, mã phòng 6 số, URL vào phòng và cơ chế giành quyền điều khiển (EPIC-001); việc **đặt** giá trị chủ đề và tải file âm thanh trong contest builder (EPIC-004); rate-limit và nút khoá cổng (EPIC-012); biên bản và bảng điểm cuối sau trận (EPIC-010).

## User Scenarios & Testing *(mandatory)*

### US-001 — Màn khán giả public: vào bằng URL, không có đường ghi (Priority: P1)

- **Title**: Read-only là tính chất CẤU TRÚC, không phải một luật server phải nhớ kiểm
- **Actor**: ACTOR-005 khán giả
- **Intent**: Mở một đường dẫn và theo dõi được trận đang chạy — sân khấu, bảng điểm, bàn cờ — mà không phải có tài khoản, không phải chờ ai duyệt.
- **User value**: Khán giả là số đông và ẩn danh; mọi bước đăng nhập hay gõ mã trước giờ phát sóng đều là ma sát mà không đổi lấy hàng rào nào.
- **Priority**: P1
- **Priority rationale**: Đây là kênh **đông người nhất** và là kênh **duy nhất mở với Internet**. Nếu nó có một đường ghi thì mọi hàng rào của `INV-017` và `GR-037` phải dựa vào việc **nhớ kiểm** ở từng handler — đúng dạng luật mà `QĐ-088` viết ra để xoá bỏ.
- **Independent test**: Mở đường dẫn phòng trên một máy chưa từng đăng nhập; xác nhận thấy được trạng thái trận hiện tại và điểm của mọi ghế mà không qua bước nào. Sau đó rà toàn bộ bề mặt của kênh và xác nhận **không tồn tại** đường gửi một sự kiện nào lên server; thử gửi thẳng một sự kiện ghi tới server bằng tay và xác nhận nó không có đích đến.
- **Related PRD requirements**: PRD-REQ-073, PRD-REQ-107, PRD-REQ-041
- **Related game rules**: `GR-037` §bảng ba loại thông tin, `GR-028` §Biên, `GR-004` C5, `GR-016` §Điều kiện · `INV-018` · `NFR-20`
- **Related journey**: JOURNEY-004, JOURNEY-005

---

### US-002 — Lớp phủ 1920×1080 nền trong suốt cho phần mềm dựng hình (Priority: P1)

- **Title**: Chồng lên video mà không có nền — lời giải cho `PS-4`
- **Actor**: ACTOR-006 máy dựng stream
- **Intent**: Nạp **một URL** vào phần mềm dựng hình và có ngay một lớp đồ hoạ khớp trận, chồng lên nguồn video mà không che nó.
- **User value**: Đơn vị tổ chức không phải dựng thủ công từng lớp đồ hoạ cho mỗi trận — đây là toàn bộ lý do `GOAL-008` tồn tại.
- **Priority**: P1
- **Priority rationale**: Lớp phủ **chính là buổi phát sóng** (`QĐ-102`), không phải khán giả của nó. Hỏng lớp phủ là hỏng sản phẩm ở đúng chỗ khách hàng nhìn thấy, và nó cũng là kênh **dễ bị bỏ sót** nhất trong bộ kiểm không-rò-đáp-án vì không ai ngồi trước nó.
- **Independent test**: Nạp đường dẫn lớp phủ vào một phần mềm dựng hình, đặt lên trên một nguồn video; xác nhận vùng không có đồ hoạ **trong suốt hoàn toàn** và khung là **1920×1080**. Sau đó khoá cổng phòng khán giả và xác nhận lớp phủ **vẫn kết nối lại được**. Cuối cùng chạy bộ kiểm không-rò-đáp-án trên chính kênh này ở mọi thời điểm trước mốc câu khép.
- **Related PRD requirements**: PRD-REQ-073, PRD-REQ-107, PRD-REQ-049
- **Related game rules**: `GR-037` C4, C5, C9 · `INV-017` · `NFR-20`, `NFR-21`
- **Related journey**: JOURNEY-004, JOURNEY-005

---

### US-003 — Đáp án không rời server tới khán giả và lớp phủ trước mốc CÂU KHÉP (Priority: P1)

- **Title**: Hàng rào chống rò đề trên hai kênh không có ai ngồi canh
- **Actor**: ACTOR-005 khán giả · ACTOR-006 máy dựng stream
- **Intent**: Thấy đáp án đúng lúc gameshow công bố nó — không sớm hơn một giây nào, kể cả khi cờ hiện đáp án đang bật.
- **User value**: Trận giữ được kịch tính, và cơ chế cướp quyền Về đích — cơ chế chuyển điểm lớn nhất của vòng quyết định — không bị xoá sổ bởi một cú công bố sớm.
- **Priority**: P1
- **Priority rationale**: `QĐ-080` gọi thẳng việc công bố tại mốc *chấm Sai* của Về đích là **sai luật gốc, không phải lệch trải nghiệm**. Đây cũng là chỗ tiền lệ Athena hỏng: nạp đáp án xuống client rồi ẩn bằng một cờ hiển thị. Với hai kênh public thì hậu quả nặng hơn màn thí sinh — bất kỳ ai có đường dẫn đều đọc được.
- **Independent test**: Chạy một trận đủ năm vòng có dùng cả *Huỷ kết quả*, cả **kích hoạt tay**, và một câu Về đích bị cướp quyền; bắt gói tin trên **cả hai** kênh public và xác nhận không byte nào mang đáp án chuẩn trước mốc câu khép tương ứng. Lặp lại với cờ *hiện đáp án sau khi chấm* **tắt** và xác nhận đáp án **không bao giờ** tới hai kênh này.
- **Related PRD requirements**: PRD-REQ-049, PRD-REQ-073
- **Related game rules**: `GR-037` *(toàn bộ bảng và bảng §Mốc câu khép theo vòng)*, `GR-012` C2 · `INV-017` · `TERM-057`
- **Related journey**: JOURNEY-005

---

### US-004 — Màn MC chữ rất to có đáp án, read-only trừ đúng một prompt (Priority: P1)

- **Title**: MC phán quyết bằng LỜI; màn `/mc` có đúng một nút trong toàn hệ thống
- **Actor**: ACTOR-004 MC
- **Intent**: Đọc câu hỏi và đáp án từ một màn riêng, chữ đủ to để đọc trên sân khấu, mà không phải nhìn ké màn admin.
- **User value**: `product-discovery.md` §8 nói thẳng: *"nếu chỉ giữ được một thứ ngoài màn khán giả, giữ màn MC"* — không có nó thì MC phá luồng thao tác của admin suốt trận.
- **Priority**: P1
- **Priority rationale**: Hai mặt cùng P1. Mặt **có**: MC là người đọc câu hỏi, nên đây là màn không thể thiếu. Mặt **không có**: mỗi nút thừa trên màn này là một lỗ thủng của `QĐ-001` — và lỗ nguy hiểm nhất là để MC duyệt tín hiệu của **thí sinh**, việc mà `STATE-039` §Cấm ghi thành một điều cấm riêng.
- **Independent test**: Mở màn MC bằng một tài khoản mang vai *MC*; xác nhận thấy câu hỏi và đáp án ngay cả **trước khi admin chấm**, và trên toàn màn **không có** nút điều khiển trận nào. Sau đó gửi thẳng tới server từng loại sự kiện ghi của admin và của thí sinh dưới danh nghĩa phiên MC này, và xác nhận **mọi** cái bị từ chối. Cuối cùng ngắt mạng phiên admin đang giữ quyền, cho một admin khác bấm giành, và xác nhận prompt duyệt hiện lên đúng màn này và **không đóng được**.
- **Related PRD requirements**: PRD-REQ-074, PRD-REQ-049, PRD-REQ-077
- **Related game rules**: `GR-037` C2 · `STATE-039`, `EVENT-051`, `T-092` → `T-095` · `PERM-045`, `PERM-054`
- **Related journey**: JOURNEY-005, JOURNEY-006

---

### US-005 — Lớp hiển thị công bố kết quả: server tính thứ hạng, admin mở và đóng tuỳ ý (Priority: P2)

- **Title**: Một LỚP PHỦ, nên nó không huỷ trạng thái đang chạy bên dưới
- **Actor**: ACTOR-001 admin *(người bấm)* · ACTOR-005 khán giả *(người xem)*
- **Intent**: Đưa bảng xếp hạng lên mọi màn ở bất cứ lúc nào, rồi tắt đi và trận tiếp tục đúng chỗ cũ.
- **User value**: Người vận hành có một công cụ trình diễn dùng được giữa trận mà không phải trả giá bằng việc rời vòng đang chạy — thứ mà một trạng thái cấp trận sẽ bắt họ làm.
- **Priority**: P2
- **Priority rationale**: P2 chứ không P1 vì trận **chạy được** mà không có nó — bảng điểm realtime đã có trên mọi kênh. Giá trị của nó là trình diễn và là chỗ duy nhất thể hiện quy tắc **đồng hạng**. Cài sai theo hướng *"một bước của luồng thi"* thì mỗi lần công bố sẽ phải rời vòng rồi quay lại — hai transition giả và một cửa cho lỗi mất trạng thái (`INV-021`).
- **Independent test**: Mở lớp công bố **giữa** một vòng đang chạy, đóng lại, và xác nhận vòng tiếp tục **đúng chỗ cũ** — cùng câu, cùng cờ ghế, cùng hàng đợi. Dựng một trận có hai ghế bằng điểm và xác nhận bảng ghi **đồng hạng** với hạng kế **nhảy qua**. Dựng một ghế điểm âm và xác nhận nó hiển thị bình thường. Cuối cùng để lớp mở trong vài phút và xác nhận **không có** bộ đếm tự đóng.
- **Related PRD requirements**: PRD-REQ-075, PRD-REQ-041
- **Related game rules**: `GR-028` §Điều kiện, C4, C5, §Biên, `GR-025` C5, `GR-016` §Điều kiện *(điểm âm tham gia bình thường)* · `STATE-033`, `EVENT-032`, `EVENT-033`, `T-087`, `T-088` · `INV-018`, `INV-021`
- **Related journey**: JOURNEY-005, JOURNEY-007

---

### US-006 — Banner tạm dừng: không chữ, chặn toàn cục, loại trừ lẫn nhau với đồng hồ (Priority: P2)

- **Title**: Ràng buộc HAI CHIỀU làm câu hỏi *"banner có đóng băng đồng hồ không"* mất chủ ngữ
- **Actor**: ACTOR-001 admin *(người bấm)* · ACTOR-005 khán giả và ACTOR-006 máy dựng stream *(người thấy)*
- **Intent**: Dừng trận thật khi có sự cố ngoài sân khấu, mà không phải giải thích gì trên màn hình.
- **User value**: Người giải thích chuyện đang xảy ra là **MC**, không phải giao diện; một banner có chữ biến sự cố hậu trường thành sự kiện trên sân khấu.
- **Priority**: P2
- **Priority rationale**: P2 vì trận vẫn có ba cửa ra khác cho một vòng hỏng. Nhưng vế **loại trừ với đồng hồ** phải cài đúng ngay lần đầu: thiếu chiều thứ hai *(không start timer khi banner đang bật)* thì admin cứ mở banner lúc rảnh rồi start timer — đúng cái tình huống `QĐ-050` viết ra để tránh, và khi đó `INV-016` **cần** một ngoại lệ mà nguồn không có.
- **Independent test**: Ở một trạng thái **không có đồng hồ nào đang đếm**, bật banner; xác nhận màn khán giả và lớp phủ hiện báo hiệu **không chữ**, màn admin **không bị phủ**, và nút start timer **không bật**. Sau đó thử bật banner trong lúc một câu đang đếm giờ và xác nhận nút mở banner **không bật**. Cuối cùng đóng banner và xác nhận trạng thái bên dưới lộ lại **nguyên vẹn**.
- **Related PRD requirements**: PRD-REQ-076
- **Related game rules**: `GR-035` *(vế đồng hồ)* · `STATE-040`, `EVENT-034`, `EVENT-035`, `T-089`, `T-090`, §Invalid transitions · `INV-006`, `INV-016`, `INV-021`
- **Related journey**: JOURNEY-006

---

### US-007 — Khán giả KHÔNG được báo về can thiệp của admin (Priority: P2)

- **Title**: Điểm đổi **đột ngột, không hiệu ứng, không giải thích** — và đó là hành vi ĐÚNG
- **Actor**: ACTOR-005 khán giả · ACTOR-006 máy dựng stream
- **Intent**: Xem một **buổi thi**, không xem một **bảng điều khiển**.
- **User value**: Trình diễn không bị vỡ bởi những dòng kiểu *"admin vừa bỏ vòng 2"*; và thứ tự lượt sắp tới không bị lộ ra ngoài.
- **Priority**: P2
- **Priority rationale**: Hai vế, hai mức hỏng khác nhau. Vế **không báo can thiệp** hỏng thì mất trình diễn. Vế **khuyến nghị lượt chỉ tới admin và MC** hỏng thì **trao một lợi thế mà luật không định trao** — thí sinh hoặc người ngoài đọc được thứ tự sắp tới. Vế thứ hai mới là lý do mục này không phải P3.
- **Independent test**: Cho admin lần lượt bỏ một vòng, chạy lại một vòng, sửa danh sách đề và gỡ một lệnh cấm; sau mỗi thao tác, kiểm toàn bộ dữ liệu tới hai kênh public và xác nhận **không có** bản tin nào ngoài số điểm mới, và số điểm đổi **không kèm hiệu ứng chuyển tiếp**. Sau đó mở vòng Về đích và xác nhận khuyến nghị lượt có mặt trên màn admin và màn MC nhưng **không** có trên hai kênh public.
- **Related PRD requirements**: PRD-REQ-077
- **Related game rules**: `GR-030` C1, C2, C3, §Ở phía viewer, `GR-016` C1 → C5 *(khuyến nghị lượt)*, `GR-028` C4 · `INV-020`
- **Related journey**: JOURNEY-005

---

### US-008 — Chủ đề và các khe âm thanh; khe trống là im lặng (Priority: P3)

- **Title**: Engine phát tín hiệu ngữ nghĩa; ánh xạ sang âm thanh là cấu hình phía client
- **Actor**: ACTOR-001 admin
- **Intent**: Gán file âm thanh cho từng khe sự kiện và đặt chủ đề hiển thị cho một contest, mà không phải nhờ lập trình viên.
- **User value**: Đơn vị tổ chức dựng được nhận diện riêng cho giải của mình; và sửa luật về sau không làm hỏng âm thanh, cũng như ngược lại.
- **Priority**: P3
- **Priority rationale**: Không có nó thì trận **vẫn chạy đủ luật** — khe trống là im lặng theo thiết kế. Nhưng danh sách khe phải phủ **cả sự kiện điều khiển** ngay từ đầu: thiếu khe thì không thêm được về sau mà không sửa engine, còn có khe mà để trống thì **không tốn gì**.
- **Independent test**: Gán file cho khe *bỏ vòng*, để trống khe *chạy lại vòng*, rồi lần lượt bỏ một vòng và chạy lại một vòng; xác nhận cái thứ nhất phát đúng file, cái thứ hai **im lặng và không báo lỗi**. Sau đó đổi chủ đề của contest và xác nhận màn khán giả và lớp phủ đổi theo.
- **Related PRD requirements**: PRD-REQ-078
- **Related game rules**: — *(`PRD-REQ-078` khai **Related game rules: —**)*
- **Related journey**: JOURNEY-002

---

### Edge Cases

Liệt kê theo tám nhóm mà lệnh gọi yêu cầu. Mỗi mục ghi **hành vi đã có nguồn**; mục nào nguồn im lặng thì trỏ sang §Open Questions.

**Boundary cases**

- **Mốc câu khép ở Về đích** đến **sau** cú bấm chấm của người thi chính, cách nhau **ít nhất** bằng độ dài cửa sổ cướp quyền (`GR-037` §Biên). Đáp án tới hai kênh public **đúng tại** mốc đó, không sớm hơn một mili-giây nào.
- **Câu bị bỏ qua** ở Khởi động lượt chung — hết `3` giây không ai bấm: câu **chưa từng được chấm** nhưng **đã tiêu** ⇒ **vẫn công bố** cho khán giả và lớp phủ nếu cờ bật (`GR-037` C7, `QĐ-080` vế 4).
- **Nhóm hoà 2 người** là biên dưới của quy tắc đồng hạng; **4 người cùng điểm** là biên trên ở v1 (`GR-025` §Biên, `QĐ-049` ràng buộc 2).
- **Điểm âm** trên bảng xếp hạng: `−5`, `−30`, `−100` đều hiển thị nguyên giá trị; **không có sàn** và **không có trần** (`GR-028` §Biên, `GR-004` C5, `INV-018`).
- **Banner tạm dừng bật đúng lúc không đồng hồ nào đang đếm** là **điều kiện cứng của trạng thái**, không phải một khuyến nghị (`STATE-040` §Vào, `T-089`).
- **Khoá cổng đang bật**: khán giả **mới** không vào được; khán giả **đang xem** không bị ngắt; **lớp phủ vẫn vào được**, kể cả một lần kết nối lại giữa buổi phát sóng (`QĐ-102`, `PRD-REQ-086` §Note). Nút khoá cổng là **thao tác của admin đang điều khiển trận**; hành vi của nó thuộc `specs/001` FR-035.
- **Số khán giả** MUST NOT chạm tới thứ tự chuông, đồng hồ hay thứ hạng tốc độ — hai kênh public nằm **ngoài** kênh của lõi thi đấu (`QĐ-088`, `NFR-06`). Nhưng mỗi người xem **vẫn giữ một kết nối mở**; con số là **giá trị định cỡ triển khai**, không phải cam kết sản phẩm (`PRD-REQ-087` §Note).

**Invalid state**

- **Mở lớp công bố khi nó đã đang mở**: nút là nút **một chiều, tự đổi thành nút đóng** ⇒ nhánh này không tồn tại (`EVENT-032` §Không hợp lệ ở).
- **Mở lớp công bố khi banner tạm dừng đang bật** — và chiều ngược lại: nút **không bật**, toast invalid state, **không ép được**; tổ hợp *(banner × lớp công bố)* **không dựng được** (`FR-058a`).
- **Đóng lớp công bố khi nó chưa mở**: không hợp lệ (`EVENT-033` §Không hợp lệ ở).
- **Mở banner khi đang có đồng hồ chạy** — `STATE-019` · `STATE-010` · `STATE-013`: nút **không bật**, toast invalid state, **không ép được** (`EVENT-034` §Không hợp lệ ở, §Invalid transitions).
- **Start timer khi banner đang bật**: chiều còn lại của ràng buộc loại trừ ⇒ toast; phải đóng banner trước (§Invalid transitions dòng `STATE-040` × `EVENT-010`).
- **Đóng banner khi banner chưa bật**: không hợp lệ (`EVENT-035` §Không hợp lệ ở).
- **MC bấm bất cứ thứ gì ngoài prompt duyệt giành quyền**: màn `/mc` **không có nút nào khác**, và server MUST từ chối mọi sự kiện ghi khác đến từ vai này (`PRD-REQ-074`, `STATE-039` §Cấm).
- **Kênh khán giả hoặc lớp phủ gửi một sự kiện lên server**: nhánh này **không tồn tại về cấu trúc** — kênh một chiều, không có đường ghi. Đây **không** phải một luật server phải cưỡng chế (`QĐ-088`, `NFR-20`, `PRD-REQ-107`).
- **Prompt duyệt giành quyền khi phiên đang giữ VẪN còn kết nối**: nhánh này không tồn tại (`PRD-REQ-108`, `T-092` §Điều kiện). Cơ chế thuộc EPIC-001.

**Repeated action**

- **Xem đáp án nhiều lần trên màn MC**: mỗi lần **một dòng audit**; trạng thái trận **không đổi** (`GR-037` §Bấm trùng).
- **Mở rồi đóng rồi mở lại lớp công bố**: mỗi lần server tính lại thứ hạng từ nhật ký sự kiện tại thời điểm mở; **không** sinh event điểm nào ở bất kỳ lần nào (`T-087`, `INV-021`).
- **Bật rồi tắt rồi bật lại banner**: hợp lệ không giới hạn số lần; mỗi lần vào `AuditLog`; **không cần dialog xác nhận** vì thao tác đảo ngược được bằng một cú bấm (`STATE-040` §Bốn ràng buộc suy ra, `QĐ-005`).
- **Admin bấm công bố Chướng ngại vật hai lần** (`GR-012` C5): **không tồn tại** — nút một chiều, tự tắt; server bỏ qua lệnh trùng. Cú bấm thuộc EPIC-007; spec này chỉ nhận **hệ quả hiển thị** trên kênh khán giả.

**Stale state**

- **Đóng lớp công bố ⇒ trạng thái bên dưới lộ lại**: đó là trạng thái **hiện tại**, không phải ảnh chụp lúc mở (`STATE-033` §Ra, `T-088`).
- **Đóng banner ⇒ trạng thái bên dưới lộ lại nguyên vẹn**: cũng là trạng thái **hiện tại** — dữ liệu bên dưới **vẫn chảy** trong lúc banner bật; **che ≠ đổi** (`STATE-040` §Ra, `QĐ-115`).
- **Lớp phủ dựng stream kết nối lại giữa buổi phát sóng**: lấy ảnh chụp trạng thái hiện tại qua một lời gọi đọc, rồi tiếp tục nhận đẩy một chiều (`QĐ-088`). Ảnh chụp đó MUST đi qua **cùng** bộ lọc đáp án của `GR-037` — không có đường tắt nào cho gói khôi phục (`QĐ-051` §Hệ quả).
- **Cú giành quyền mất đối tượng** khi phiên đang giữ kết nối lại: prompt trên màn MC **đóng**, quyền ở nguyên chỗ cũ, **không xoá, không cảnh báo** (`T-095`).

**Duplicate event**

- **Công bố là MỘT CHIỀU ở phía engine** — đã công bố thì engine **không tự thu lại**. Quyền đóng hiển thị thủ công của người giữ `PERM-044` giữ nguyên và là một đường **khác** (`GR-037` §Bấm trùng, §Không đổi gì).
- **Server đẩy một sự kiện thứ hạng đã tính** khi mở lớp công bố — **một** sự kiện, không phải mỗi ghế một cái; client **không được tự tính** thứ hạng từ bản sao điểm của mình (`EVENT-032`, `STATE-033` §Cấm).
- **Một câu Tăng tốc là MỘT event điểm cho toàn bộ bảng**; hoàn nguyên đảo cả bảng ⇒ trên kênh khán giả, bốn ô điểm đổi **cùng một nhịp**, không phải bốn nhịp (`PRD-REQ-040`, `QĐ-013`). Mô hình event thuộc EPIC-006.

**Partial failure**

- **Một ghế mất kết nối**: kênh khán giả và lớp phủ **không** dừng, đồng hồ **không** dừng vì một ghế (`GR-036` C4, `INV-016`). Banner *"đang kết nối lại"* (`STATE-034`) hiện trên **máy ghế đó**, không phải trên kênh public.
- **Phiên admin đang giữ quyền mất kết nối**: hệ thống MUST NOT dừng đồng hồ và MUST NOT tạm dừng trận; hai kênh public **không được báo gì**, và ở ca contest **không có MC** thì cú giành có hiệu lực **ngay và âm thầm** (`PRD-REQ-108`, `T-092`).
- **Khe âm thanh chưa gán file**: **im lặng**, và MUST NOT báo lỗi (`QĐ-079`, `PRD-REQ-078` §Acceptance intent).
- **Admin không chuẩn bị âm thanh nào**: sản phẩm chạy **hoàn toàn im lặng**. Đây là **rủi ro đã ghi nhận và chấp nhận**, không phải lỗi — `RISK-005`.

**Ranh giới dễ đọc nhầm thành mâu thuẫn**

- **Banner tạm dừng tác động lên đúng HAI loại màn**: **máy thí sinh** *(che toàn bộ + chặn toàn bộ thao tác — mặt đó thuộc `specs/008`)* và **hai kênh public** *(báo hiệu thuần thị giác)*. **Màn MC và màn admin không bị phủ**; trên màn MC, câu hỏi và đáp án vẫn đọc được và prompt duyệt giành quyền `STATE-039` vẫn bấm được trong lúc banner bật *(FR-055, FR-055a)*.
- **Lệnh cấm báo khán giả áp theo KHE, không theo nhóm sự kiện.** Khe *mở màn công bố* là một **mốc trình diễn** nên nó **hướng khán giả**; năm khe *hoàn nguyên · bỏ vòng · chạy lại vòng · kết thúc sớm · ép qua cảnh báo* là **thao tác sự cố** nên chỉ phát trên **máy admin** *(FR-065a, FR-074a)*.
- **Vào phòng bằng một URL chứa mã phòng 6 số** — hai cách nói của cùng một thứ. Mã phòng nằm **trong** URL; **không** có bước nhập mã riêng, **không** tài khoản, **không** chờ duyệt *(FR-001, `QĐ-096`)*.
- **Lớp công bố áp cho cả máy thí sinh mà KHÔNG lộ đáp án**: nó chỉ chứa **tên · điểm · thứ hạng**, và **điểm là thông tin công khai với mọi vai ở mọi thời điểm**. Chỉ **đáp án chuẩn** mới bị siết theo mốc câu khép *(FR-043, FR-054, FR-031)*.

**Missing source behavior**

- **Khe mà nguồn không liệt kê** — ví dụ phán quyết **sai** ở vòng Câu hỏi phụ: giữ nguyên tình trạng **thiếu**; bổ sung là quyết định mới của chủ dự án, không suy diễn từ luật (`FR-074e`).
- **Khe âm thanh của một thao tác can thiệp phát ra bản trộn phát sóng**: đây là ca mà một lệnh cấm **thị giác** bị lách qua đường **thính giác**. Năm khe sự cố thuộc nhóm **chỉ-admin** và **không** đi qua cấu hình nguồn phát, nên nhánh này không tồn tại (`FR-065a`, `FR-074a`).
- **Nguồn phát đang là lớp phủ mà không lớp phủ nào kết nối**: khán giả **không nghe gì**; hệ thống **không** tự lui và **không** báo lỗi (`FR-074c`).
- **Cú đóng hiển thị bằng tay còn hiệu lực khi sang câu mới**: **không dính** — hành vi hiển thị của câu mới quay về mặc định do cờ cấp trận quyết định (`FR-027a`, `NFR-25`).
- **Kích thước khung nhìn tham chiếu** cho màn khán giả và màn MC: `NFR-37` nói thẳng đây là **quyết định thiết kế, không phải yêu cầu chuẩn tắc** — nên spec này phát biểu ràng buộc *"nội dung chính nằm trong một khung nhìn"* mà **không** khai một con số pixel. Riêng lớp phủ dựng stream **có** con số chuẩn tắc `1920×1080` vì `PRD-REQ-073` khai thẳng.
- **Cỡ chữ cụ thể của màn MC**: nguồn chỉ nói *"chữ rất to"* (`PRD-REQ-074`). Không có con số nào; spec này phát biểu ràng buộc **đọc được ở khoảng cách sân khấu** và để con số cho tầng thiết kế, cùng cách xử lý với `NFR-37`.

## Requirements *(mandatory)*

### Functional Requirements

#### Nhóm A — Màn khán giả public (US-001)

- **FR-001**: Hệ thống MUST có một **màn khán giả** hiển thị trạng thái trận đang chạy, vào được bằng **một đường dẫn phòng** mà MUST NOT đòi tài khoản, MUST NOT có bước nhập mã riêng, và MUST NOT chờ ai duyệt. *(US-001 · PRD-REQ-073 · `QĐ-096`, `TERM-007` · AC-001)*
- **FR-002**: Kênh của màn khán giả MUST là kênh **một chiều server → client**, cộng **một** lời gọi đọc để lấy ảnh chụp trạng thái lúc vào phòng. Kênh này MUST NOT có đường gửi sự kiện lên server. *(US-001 · PRD-REQ-107 · `QĐ-088`, `NFR-20` · AC-002)*
- **FR-003**: Tính chất read-only của kênh khán giả MUST là **cấu trúc** — hệ thống MUST NOT hiện thực nó bằng một guard ở server bỏ các sự kiện ghi đến từ kênh này. *(US-001 · PRD-REQ-107 · `QĐ-088` · AC-002, AC-003)*
- **FR-004**: Kênh hai chiều MUST chỉ dành cho **vai đã xác thực** — admin, thí sinh, MC. *(US-001, US-004 · PRD-REQ-107 · `QĐ-088` · AC-003)*
- **FR-005**: Màn khán giả MUST hiển thị **điểm của mọi ghế**, cập nhật khi điểm đổi. Điểm MUST là thông tin **công khai** — kênh này MUST nhận điểm ở mọi thời điểm, không phụ thuộc mốc nào. *(US-001 · PRD-REQ-041, PRD-REQ-073 · `GR-037` §bảng ba loại thông tin, `QĐ-015` · AC-004)*
- **FR-006**: Bảng điểm và bảng xếp hạng trên kênh khán giả MUST hiển thị **điểm âm** nguyên giá trị; hệ thống MUST NOT kẹp về `0` và MUST NOT ẩn. *(US-001, US-005 · PRD-REQ-041 · `GR-028` §Biên, `GR-004` C5, `INV-018` · AC-005)*
- **FR-007**: Màn khán giả MUST NOT hard-code số ghế; nó MUST dựng đúng số ghế của trận, gồm cả **ghế bỏ thi** đang mang điểm `0`. *(US-001 · PRD-REQ-041, PRD-REQ-075 · `QĐ-105` · AC-006)*
- **FR-008**: Số khán giả đang xem MUST NOT chạm tới thứ tự chuông, đồng hồ hay thứ hạng tốc độ. *(US-001 · PRD-REQ-107 · `QĐ-088`, `NFR-06` · AC-007)*
- **FR-009**: Hệ thống MUST NOT tạo một **vai** *"khán giả"* hay *"lớp phủ"* trong catalog phân quyền — hai kênh này nằm **ngoài** hệ phân quyền. *(US-001, US-002 · PRD-REQ-107 · `QĐ-096` vế 2, `permissions.md` §8 · AC-003)*
- **FR-010**: Màn khán giả MUST hiển thị được bàn cờ Vượt chướng ngại vật, gồm cả trạng thái **mở toàn bộ miếng ghép** khi admin bấm công bố Chướng ngại vật. Cú bấm đó thuộc EPIC-007; kênh này MUST chỉ **render** hệ quả. *(US-001 · PRD-REQ-073 · `GR-012` C2, C3 · AC-008)*

#### Nhóm B — Lớp phủ dựng stream (US-002)

- **FR-011**: Hệ thống MUST có một **lớp phủ dựng stream** khung **1920×1080**, **nền trong suốt**, vào được bằng một đường dẫn phòng theo cùng mô hình truy cập của màn khán giả. *(US-002 · PRD-REQ-073 · `QĐ-096` · AC-009)*
- **FR-012**: Vùng không có đồ hoạ của lớp phủ MUST trong suốt hoàn toàn, để nó chồng lên nguồn video mà MUST NOT che nguồn đó. *(US-002 · PRD-REQ-073 · AC-009)*
- **FR-013**: Kênh của lớp phủ MUST tuân đúng FR-002, FR-003 và FR-004 — một chiều, không có đường ghi, không thuộc catalog phân quyền. *(US-002 · PRD-REQ-107 · `QĐ-088` · AC-010)*
- **FR-014**: Lớp phủ MUST **vào được kể cả khi cổng phòng khán giả đang khoá**, gồm cả một lần **kết nối lại** giữa buổi phát sóng, và MUST NOT đòi thao tác mở cổng nào. *(US-002 · PRD-REQ-073 · `QĐ-102` · AC-011)*
- **FR-015**: Khi lớp phủ kết nối lại, ảnh chụp trạng thái mà nó nhận MUST đi qua **cùng** bộ lọc đáp án của `GR-037`; hệ thống MUST NOT có đường tắt nào cho gói khôi phục của kênh này. *(US-002, US-003 · PRD-REQ-049 · `QĐ-051` §Hệ quả, `INV-017` · AC-012)*
- **FR-016**: Lớp phủ MUST nhận **cùng lúc và cùng điều kiện với màn khán giả** mọi thứ mà kênh khán giả nhận, gồm cả đáp án từ mốc câu khép. Hệ thống MUST NOT có lệnh cấm riêng nào cho kênh này. *(US-002, US-003 · PRD-REQ-073, PRD-REQ-049 · `QĐ-080` vế 2, `TERM-007` · AC-013)*
- **FR-017**: Chiều truyền một chiều MUST NOT đổi **cái gì** được truyền — kênh một chiều MUST đẩy được đáp án đúng lúc, nó chỉ MUST NOT nhận vào. *(US-002, US-003 · PRD-REQ-107 · `PRD-REQ-107` §Note, `QĐ-088` §Hệ quả · AC-013)*
- **FR-018**: Bộ kiểm **không rò đáp án** MUST pass trên kênh lớp phủ ở **mọi thời điểm trước mốc câu khép**, kể cả khi cờ *hiện đáp án sau khi chấm* đang **bật**. *(US-002, US-003 · PRD-REQ-073 · `INV-017`, `NFR-21` · AC-014)*

#### Nhóm C — Phạm vi hiển thị đáp án trên hai kênh public (US-003)

- **FR-019**: **Trước mốc CÂU KHÉP**, đáp án chuẩn MUST NOT rời server tới màn khán giả và lớp phủ dựng stream, **bất kể** giá trị của cờ *hiện đáp án sau khi chấm*. *(US-003 · PRD-REQ-049, PRD-REQ-073 · `GR-037` C3, C4, `INV-017` · AC-015, AC-016)*
- **FR-020**: **Từ mốc câu khép trở đi**, server MUST đẩy đáp án tới hai kênh này **nếu và chỉ nếu** cờ *hiện đáp án sau khi chấm* của **trận đó** đang bật. *(US-003 · PRD-REQ-049 · `GR-037` C5, `QĐ-062`, `QĐ-080` vế 3 · AC-017, AC-016)*
- **FR-021**: Hệ thống MUST NOT đẩy đáp án xuống hai kênh này trước mốc rồi ẩn bằng một cờ hiển thị phía client. Server MUST chỉ đẩy **tại đúng mốc**. *(US-003 · PRD-REQ-049 · `GR-037` §Cấm, `NFR-21b`, `QĐ-080` · AC-018)*
- **FR-022**: Ở **Về đích**, khi người thi chính bị chấm **Sai**, hệ thống MUST NOT công bố đáp án trong lúc **cửa sổ cướp quyền còn mở**; câu chỉ khép khi cửa sổ đóng **và** người cướp đã được chấm, hoặc hết cửa sổ không ai bấm, hoặc người thi chính được chấm **Đúng**. *(US-003 · PRD-REQ-049 · `GR-037` C6, §bảng Mốc câu khép, `QĐ-080` vế 1 · AC-019)*
- **FR-023**: Câu **bị bỏ qua** — hết cửa sổ chuông không ai bấm — MUST **vẫn** được công bố theo cờ; câu đó **đã tiêu, đã khép**. *(US-003 · PRD-REQ-049 · `GR-037` C7, `QĐ-080` vế 4 · AC-020)*
- **FR-024**: Câu khép bằng phán quyết **Huỷ kết quả** MUST NOT **tự** công bố tới hai kênh public. Đường công bố tay của người giữ `PERM-044` giữ nguyên và là một đường **khác**. *(US-003 · PRD-REQ-049 · `GR-037` C8, `QĐ-048`, `QĐ-080` vế 4 · AC-021)*
- **FR-025**: Khi hệ thống **đang chờ admin kích hoạt tay** một tín hiệu khác sau một cú *Huỷ kết quả*, mốc câu khép MUST **lùi** tới sau khi người được kích hoạt đã được chấm; trước mốc mới đó server MUST NOT công bố đáp án tới hai kênh public. *(US-003 · PRD-REQ-049, PRD-REQ-113 (vế trình diễn) · `GR-037` C8b, `GR-032` §Kích hoạt tay, `QĐ-104` · AC-022)*
- **FR-026**: Đáp án **Chướng ngại vật** MUST NOT đi theo cơ chế của FR-019 → FR-025; nó theo `GR-012` — lộ khi có người giải đúng, hoặc khi admin bấm công bố. *(US-003 · PRD-REQ-049 · `GR-037` C9, `GR-012` C2, `QĐ-080` vế 4 · AC-023)*
- **FR-026a**: Việc đứng ngoài cơ chế của FR-019 → FR-025 MUST chỉ áp cho **trục thời điểm**, MUST NOT áp cho **trục người nhận**: khi đáp án Chướng ngại vật lộ theo `GR-012`, nó MUST tới **cả màn khán giả lẫn lớp phủ dựng stream**, **cùng lúc và cùng điều kiện**. Hệ thống MUST NOT giữ một lệnh cấm riêng nào cho lớp phủ ở đường này. *(US-003, US-002 · PRD-REQ-049, PRD-REQ-073 · `GR-012` C2, `QĐ-080` vế 2, `TERM-007`, `QĐ-102` · AC-023)*
- **FR-027**: Công bố MUST là **một chiều ở phía engine**: đã công bố thì engine MUST NOT tự thu lại. Mệnh đề này MUST chỉ nói về **engine**; nó MUST NOT được đọc thành *"cú đẩy của engine đè được lên phán quyết của người"*. *(US-003 · PRD-REQ-049 · `GR-037` §Bấm trùng · AC-024)*
- **FR-027a**: Khi một cú **đóng hiển thị bằng tay** của người giữ `PERM-044` còn hiệu lực cho câu đang chạy, engine MUST NOT đẩy đáp án tới hai kênh public tại mốc câu khép — **cú đóng tay thắng**. Hiệu lực của nó MUST ở **phạm vi câu** và MUST chấm dứt bằng đúng **một** cách: người giữ `PERM-044` **tự mở lại**. Nó MUST NOT dính sang câu sau; sang câu mới, hành vi hiển thị MUST quay về mặc định do cờ cấp trận quyết định. *(US-003 · PRD-REQ-049 · `GR-037` §Không đổi gì, §Bấm trùng, `PERM-044`, `QĐ-048`, `QĐ-001` · `NFR-25` · AC-024a)*
- **FR-028**: Cửa kiểm phạm vi đáp án MUST hỏi **permission**, MUST NOT hỏi **tên vai**. Hai kênh public không mang tài khoản nên không giữ permission nào; điều này MUST là hệ quả của cửa kiểm, MUST NOT là một danh sách chặn theo tên kênh. *(US-003, US-004 · PRD-REQ-049 · `GR-037` §Thứ tự đánh giá bước (2), `QĐ-094`, `INV-017` · AC-025)*
- **FR-029**: Cờ *hiện đáp án sau khi chấm* MUST ở **cấp TRẬN**, MUST mặc định **BẬT** cho cả trận chính thức lẫn trận luyện tập, và MUST đổi được cho từng trận. Giá trị MUST được chụp vào trận lúc bắt đầu; sửa cấu hình contest sau đó MUST NOT đụng trận đã chạy. *(US-003 · PRD-REQ-049 · `GR-037` §Điều kiện, `QĐ-062`, `QĐ-080` vế 3 · AC-026)*
- **FR-030**: Yêu cầu đọc đáp án **không được phép** MUST bị server **im lặng** từ chối — không trả đáp án, và MUST NOT trả một thông điệp tiết lộ sự tồn tại của nó. *(US-003 · PRD-REQ-049 · `GR-037` §Điều kiện · AC-027)*
- **FR-031**: Ngoài đáp án chuẩn, hai loại thông tin còn lại MUST giữ **chế độ riêng** của chúng và MUST NOT bị siết theo chế độ của đáp án: **điểm số** công khai với mọi kênh **luôn luôn**; **bài làm của thí sinh** ẩn tạm thời khi câu còn mở và lộ khi admin bấm hiển thị. *(US-003, US-001 · PRD-REQ-049, PRD-REQ-041 · `GR-037` §bảng ba loại thông tin, `GR-008` §Điều kiện, `QĐ-015` · AC-028)*
- **FR-032**: Xem đáp án MUST NOT sinh event điểm nào và MUST NOT đổi trạng thái trận. *(US-003, US-004 · PRD-REQ-049 · `GR-037` §Không đổi gì · AC-029)*

#### Nhóm D — Màn MC (US-004)

- **FR-033**: MC MUST có một màn riêng hiển thị **câu hỏi và đáp án**, chữ đủ to để đọc ở khoảng cách sân khấu. *(US-004 · PRD-REQ-074 · `product-discovery.md` §8 · AC-030)*
- **FR-034**: Màn MC MUST nhận đáp án **mọi lúc**, MUST NOT phụ thuộc cờ *hiện đáp án sau khi chấm* và MUST NOT phụ thuộc mốc câu khép — vì phiên đó giữ `PERM-045`. *(US-004 · PRD-REQ-049, PRD-REQ-074 · `GR-037` C2, `PERM-045` · AC-031)*
- **FR-035**: **Mỗi lần** đáp án tới màn MC MUST vào nhật ký thao tác. *(US-004 · PRD-REQ-049 · `GR-037` C2, §Thứ tự đánh giá bước (6), `NFR-14` · AC-032)*
- **FR-036**: Màn MC MUST NOT có **bất kỳ nút điều khiển trận nào**. *(US-004 · PRD-REQ-074 · `STATE-039` §Mô tả · AC-033)*
- **FR-037**: Bề mặt quyền ghi **duy nhất** được phép trên màn MC MUST là prompt **duyệt / từ chối cú giành quyền điều khiển**. Server MUST từ chối **mọi** sự kiện ghi khác đến từ vai này, **đặc biệt là duyệt tín hiệu của thí sinh**. *(US-004 · PRD-REQ-074 · `STATE-039` §Cấm, `PERM-054`, `TERM-005` · AC-034, AC-035)*
- **FR-038**: Việc giữ `PERM-045` MUST NOT kèm quyền **công bố** đáp án cho ai khác — đó là `PERM-044`, một permission riêng. *(US-004 · PRD-REQ-049 · `GR-037` C2, `permissions.md` §chú thích `PERM-044` vs `PERM-045` · AC-036)*
- **FR-039**: Khi prompt duyệt giành quyền hiện trên màn MC, nó MUST NOT có nút đóng hay bỏ qua — MC phải quyết. *(US-004 · PRD-REQ-074, PRD-REQ-108 (vế màn MC) · `STATE-039` §Cấm, `QĐ-093` · AC-034)*
- **FR-040**: Trong lúc prompt duyệt đang hiện, mọi thao tác của trạng thái bên dưới MUST vẫn chạy — hệ thống MUST NOT dừng đồng hồ và MUST NOT tạm dừng trận. *(US-004 · PRD-REQ-074, PRD-REQ-108 · `STATE-039` §Cho phép, `T-092`, `INV-016`, `INV-021` · AC-037)*
- **FR-041**: **Khuyến nghị lượt** MUST hiện trên màn MC và màn admin. *(US-004, US-007 · PRD-REQ-077 · `QĐ-076`, `GR-016` §Điều kiện · AC-038)*

#### Nhóm E — Lớp hiển thị công bố kết quả (US-005)

- **FR-042**: Màn công bố kết quả MUST là một **lớp phủ** chồng lên trạng thái đang chạy mà MUST NOT huỷ nó. *(US-005 · PRD-REQ-075 · `STATE-033`, `T-087`, `INV-021`, `QĐ-049` · AC-039)*
- **FR-043**: Lớp công bố MUST áp cho **mọi vai**, gồm **cả máy thí sinh**, màn khán giả, lớp phủ dựng stream và màn MC. *(US-005 · PRD-REQ-075 · `QĐ-049` · AC-040)*
- **FR-044**: Hệ thống MUST **gợi ý** mở lớp công bố ở **hai mốc** — vừa kết thúc một vòng, và trận kết thúc. Gợi ý MUST NOT là điều kiện: admin MUST mở và đóng được **tuỳ ý, ở mọi trạng thái cấp trận**. Điều kiện **duy nhất** MUST là ràng buộc loại trừ với banner tạm dừng ở FR-058a — đây là một ràng buộc giữa **hai lớp phủ**, MUST NOT hiểu thành một điều kiện về **trạng thái cấp trận**. *(US-005 · PRD-REQ-075 · `EVENT-032` §Hợp lệ ở, `STATE-033` §Vào · AC-041)*
- **FR-045**: Hệ thống MUST NOT có **bộ đếm tự đóng** cho lớp công bố; mốc đóng MUST là một thao tác bấm. *(US-005 · PRD-REQ-075 · `STATE-033` §Ra, `EVENT-033`, `T-088` · AC-042)*
- **FR-046**: Thứ hạng MUST **do server tính và đẩy xuống**; client MUST chỉ hiển thị và MUST NOT tự tính thứ hạng từ bản sao điểm của mình. *(US-005 · PRD-REQ-075 · `STATE-033` §Cấm, `EVENT-032`, `QĐ-049` ràng buộc 1 · AC-043)*
- **FR-047**: Server MUST đẩy bảng đã tính bằng **một** sự kiện. *(US-005 · PRD-REQ-075 · `EVENT-032`, `T-087` · AC-043)*
- **FR-048**: Hoà điểm MUST ghi **đồng hạng**, và hạng kế MUST **nhảy qua** số người đồng hạng. *(US-005 · PRD-REQ-075 · `QĐ-049` ràng buộc 2, `GR-025` C5 · AC-044)*
- **FR-049**: Mở lớp công bố ở `TIE_BREAK` khi một nhóm **chưa được phân định** MUST hiện nhóm đó **đồng hạng**; hệ thống MUST NOT ẩn bảng và MUST NOT bịa một thứ tự tạm. *(US-005 · PRD-REQ-075 · `QĐ-049`, `GR-025` C5 · AC-045)*
- **FR-050**: Điểm hiển thị trên lớp công bố MUST là kết quả tính từ **nhật ký sự kiện tại thời điểm mở**. *(US-005 · PRD-REQ-075 · `QĐ-049` ràng buộc 3, `GR-028` §Điều kiện, C5 · AC-046)*
- **FR-051**: Lớp công bố MUST chịu được **điểm âm** và MUST NOT hard-code số ghế. *(US-005 · PRD-REQ-075, PRD-REQ-041 · `INV-018`, `GR-028` §Biên · AC-005, AC-006)*
- **FR-052**: Mở hoặc đóng lớp công bố MUST NOT sinh event điểm, MUST NOT đổi trạng thái trận / vòng / câu / ghế / tín hiệu, MUST NOT đụng đồng hồ, MUST NOT đụng hàng đợi và MUST NOT đổi quyền thao tác. *(US-005, US-006 · PRD-REQ-075 · `INV-021`, `STATE-033` §Cấm, `T-087` · AC-047)*
- **FR-053**: Đóng lớp công bố MUST làm lộ lại trạng thái bên dưới **nguyên vẹn** — tức trạng thái **hiện tại**, MUST NOT là ảnh chụp lúc mở. *(US-005 · PRD-REQ-075 · `STATE-033` §Ra, `T-088` · AC-048)*
- **FR-054**: Lớp công bố MUST NOT lộ đáp án. Nhịp lộ từng người MUST là **animation phía client**, MUST NOT là một cơ chế của engine. *(US-005, US-003 · PRD-REQ-075, PRD-REQ-049 · `STATE-033` §Cấm, `EVENT-032`, `QĐ-049`, `CLAUDE.md` §Quy ước code · AC-049)*

#### Nhóm F — Banner tạm dừng (US-006)

- **FR-055**: Banner tạm dừng MUST chặn **toàn bộ** thao tác trên máy thí sinh, MUST báo hiệu **trận đang tạm dừng** trên màn khán giả và lớp phủ dựng stream, và MUST NOT phủ màn admin. *(US-006 · PRD-REQ-076 · `STATE-040` §Mô tả, `QĐ-050` · AC-050)*
- **FR-055a**: Banner tạm dừng MUST tác động lên đúng **hai** loại màn — máy thí sinh và hai kênh public. Nó MUST NOT phủ **màn MC** và MUST NOT phủ màn admin; trên màn MC, câu hỏi và đáp án MUST vẫn đọc được và prompt duyệt giành quyền (`STATE-039`) MUST vẫn bấm được trong lúc banner đang bật. *(US-006, US-004 · PRD-REQ-076, PRD-REQ-074 · `STATE-040` §Mô tả, `STATE-039` §Cho phép, `QĐ-093` · AC-050a)*
- **FR-056**: Banner MUST **không có chữ nào** — MUST NOT hiện lý do, tên người bấm, hay đồng hồ. *(US-006 · PRD-REQ-076 · `STATE-040` §Cấm, `QĐ-050` · AC-051)*
- **FR-057**: Banner MUST chỉ bật được khi **không có cửa sổ thời gian nào đang đếm**; khi có đồng hồ chạy, nút mở banner MUST NOT bật. *(US-006 · PRD-REQ-076 · `STATE-040` §Vào, `T-089`, `EVENT-034` §Không hợp lệ ở · AC-052)*
- **FR-058**: Chiều còn lại của ràng buộc MUST được cưỡng chế: hệ thống MUST NOT cho start timer trong lúc banner đang bật; phải đóng banner trước. *(US-006 · PRD-REQ-076 · `STATE-040` §Cấm, §Invalid transitions, `QĐ-050` · AC-053)*
- **FR-058a**: Banner tạm dừng và lớp hiển thị công bố kết quả MUST **loại trừ lẫn nhau**, ràng buộc **hai chiều**: hệ thống MUST NOT cho mở lớp công bố khi banner đang bật, và MUST NOT cho mở banner khi lớp công bố đang bật. Ở cả hai chiều, nút MUST NOT bật; lệnh lọt tới server MUST bị từ chối kèm phản hồi hạng **invalid state**, và MUST NOT ép được. Ràng buộc này MUST NOT là một chỗ **chặn cứng** thứ tư của luật chơi, và MUST NOT mở rộng thành một quy tắc thứ tự chồng lớp cho các cặp lớp phủ khác. *(US-006, US-005 · PRD-REQ-076, PRD-REQ-075 · `STATE-033` §Vào, `STATE-040` §Vào, §Invalid transitions · `INV-014`, `INV-021` · AC-058a)*
- **FR-059**: Hệ thống MUST NOT có bộ đếm tự đóng cho banner; mốc đóng MUST là một thao tác bấm. *(US-006 · PRD-REQ-076 · `STATE-040` §Cấm, §Ra, `EVENT-035` · AC-054)*
- **FR-060**: Việc chặn thao tác MUST được **server cưỡng chế**, MUST NOT chỉ dựa vào client. *(US-006 · PRD-REQ-076 · `STATE-040` §Bốn ràng buộc suy ra, `CLAUDE.md` §Zero-trust · AC-055)*
- **FR-061**: Tín hiệu của thí sinh tới server trong lúc banner đang bật MUST NOT bị **drop**; nó MUST vào lịch sử với kết cục **trơ**. *(US-006 · PRD-REQ-076 · `STATE-040` §Bốn ràng buộc suy ra, `INV-006` · AC-056)*
- **FR-062**: Mở và đóng banner MUST vào nhật ký thao tác. *"Không ghi lý do"* MUST là một ràng buộc **hiển thị**, MUST NOT là một miễn trừ audit. *(US-006 · PRD-REQ-076 · `STATE-040` §Bốn ràng buộc suy ra, `NFR-14` · AC-057)*
- **FR-063**: Banner MUST NOT đòi dialog xác nhận — thao tác đảo ngược được bằng một cú bấm. *(US-006 · PRD-REQ-076 · `STATE-040` §Bốn ràng buộc suy ra, `QĐ-005` · AC-058)*
- **FR-064**: Trong lúc banner bật, dữ liệu bên dưới MUST **vẫn chảy** — server vẫn đẩy, client vẫn nhận; banner MUST chỉ phủ lên trên. Đóng banner MUST cho ra **trạng thái hiện tại**. *(US-006 · PRD-REQ-076 · `STATE-040` §Ra, `QĐ-115`, `INV-021` · AC-059)*

#### Nhóm G — Khán giả không được báo về can thiệp của admin (US-007)

- **FR-065**: **Bỏ vòng · chạy lại vòng · sửa danh sách đề · gỡ lệnh cấm** MUST NOT sinh thông báo nào tới màn khán giả và lớp phủ dựng stream. *(US-007 · PRD-REQ-077 · `QĐ-076`, `GR-030` §Ở phía viewer · AC-060)*
- **FR-065a**: Lệnh cấm của FR-065 MUST áp cho **mọi phương tiện**, không riêng phương tiện nhìn: hệ thống MUST NOT phát **âm thanh** của một thao tác can thiệp tới màn khán giả hay lớp phủ dựng stream. Một khe âm thanh ra bản trộn phát sóng **là** một thông báo, chỉ đổi phương tiện từ mắt sang tai. *(US-007, US-008 · PRD-REQ-077, PRD-REQ-078 · `QĐ-076`, `QĐ-079` · AC-069a)*
- **FR-066**: Điểm và bàn cờ trên hai kênh public MUST đổi **đột ngột, không hiệu ứng chuyển tiếp, không giải thích**. *(US-007 · PRD-REQ-077 · `GR-030` §Ở phía viewer, `QĐ-076` · AC-061)*
- **FR-067**: Sau khi admin **bỏ** một vòng, kênh khán giả MUST NOT nhận bản tin nào ngoài **số điểm mới**. *(US-007 · PRD-REQ-077 · `GR-030` C1, `QĐ-076` · AC-062)*
- **FR-068**: Sau khi admin **chạy lại** một vòng, kênh khán giả MUST NOT nhận thông báo nào; nó MUST chỉ thấy điểm đã hoàn nguyên và vòng mở lại. *(US-007 · PRD-REQ-077 · `GR-030` C2 · AC-063)*
- **FR-069**: Sau khi admin **kết thúc sớm** một vòng, kênh khán giả MUST NOT nhận thông báo nào, và điểm MUST **giữ nguyên** — không có event đảo ngược nào để hiển thị. *(US-007 · PRD-REQ-077 · `GR-030` C3 · AC-064)*
- **FR-070**: **Khuyến nghị lượt** MUST NOT được đẩy xuống màn khán giả và lớp phủ dựng stream. *(US-007 · PRD-REQ-077 · `QĐ-076`, `GR-016`, `INV-020` · AC-065)*
- **FR-071**: Nhãn *"đã bỏ"* / *"đã chạy lại"* / *"kết thúc sớm"* của một lần chạy vòng MUST NOT xuất hiện trên hai kênh public. Chúng thuộc **biên bản trận** (EPIC-010). *(US-007 · PRD-REQ-077 · `GR-030` §Điều kiện, `QĐ-077` · AC-066)*

#### Nhóm H — Chủ đề và các khe âm thanh (US-008)

- **FR-072**: Contest MUST cấu hình được một **chủ đề hiển thị** gồm đúng **ba trục tĩnh**: **bảng màu** · **logo giải** · **ảnh nền**. *(US-008 · PRD-REQ-078 · `PERM-023` · AC-067)*
- **FR-072a**: Chủ đề MUST NOT chứa **tài sản động** — hình hiệu, clip mở màn hay chuỗi animation. Hiệu ứng động thuộc **module animation** (`FR-085`) và âm thanh thuộc **khe âm thanh** (`FR-073` → `FR-078`); chủ đề MUST NOT lấn sang hai trục đó. Chủ đề cũng MUST NOT chứa **phông chữ** — phông khoá cứng ở `CLAUDE.md` §Quy ước code. *(US-008 · PRD-REQ-078 · `QĐ-079`, `CLAUDE.md` §Quy ước code · AC-067)*
- **FR-072b**: Chủ đề MUST áp cho đúng **hai kênh public** — màn khán giả và lớp phủ dựng stream. Hệ thống MUST NOT áp chủ đề lên **màn thí sinh, màn MC và màn admin**; ba màn này là bề mặt **vận hành** và MUST giữ nguyên bảng màu hệ thống để một chủ đề đặt sai MUST NOT làm hỏng khả năng đọc của người đang thao tác. *(US-008, US-004 · PRD-REQ-078, PRD-REQ-074 · `FR-082`, `NFR-37` · AC-067a)*
- **FR-073**: Hệ thống MUST cho admin **tải lên** file âm thanh cho từng **khe sự kiện**. *(US-008 · PRD-REQ-078 · `QĐ-079`, `PERM-023` · AC-068)*
- **FR-074**: Danh sách khe MUST phủ **cả sự kiện điều khiển**: hoàn nguyên · bỏ vòng · chạy lại vòng · kết thúc sớm · ép qua cảnh báo · mở màn công bố. *(US-008 · PRD-REQ-078 · `QĐ-079` · AC-069)* Danh sách **đầy đủ** của nhóm *mốc thi đấu* ở **FR-074d**.
- **FR-074a**: Mỗi khe MUST thuộc đúng **một** trong hai nhóm, và ranh giới MUST là *khán giả có được nghe khe này không*:
  - **Nhóm hướng khán giả** — các **mốc thi đấu** và khe **mở màn công bố**: MUST phát tới khán giả qua nguồn phát của FR-074b.
  - **Nhóm chỉ-admin** — *hoàn nguyên · bỏ vòng · chạy lại vòng · kết thúc sớm · ép qua cảnh báo*: MUST chỉ phát trên **máy admin**, và MUST NOT tới màn khán giả hay lớp phủ dựng stream ở bất kỳ cấu hình nào.

  *(US-008, US-007 · PRD-REQ-078, PRD-REQ-077 · `QĐ-079`, `QĐ-076` · AC-069, AC-069a)*
- **FR-074b**: **Nguồn phát** cho nhóm hướng khán giả MUST là một giá trị cấu hình được với đúng hai lựa chọn — **máy admin** hoặc **lớp phủ dựng stream** — mặc định **lớp phủ dựng stream**; admin MUST bật, tắt và đổi được giá trị này. Cấu hình này MUST chỉ áp cho **nhóm hướng khán giả**; nhóm chỉ-admin MUST NOT đi qua nó. **Màn khán giả MUST NOT bao giờ là nguồn phát.** *(US-008 · PRD-REQ-078 · `QĐ-079`, `QĐ-102` · AC-069b)*
- **FR-074c**: Khi nguồn phát đang là **lớp phủ dựng stream** mà **không có lớp phủ nào kết nối**, hệ thống MUST NOT tự đổi nguồn phát sang máy admin và MUST NOT báo lỗi — trận chạy **im lặng** phía khán giả cho tới khi admin tự đổi cấu hình. *(US-008 · PRD-REQ-078 · `QĐ-079`, `QĐ-001` · AC-069c)*
- **FR-074d**: Nhóm **mốc thi đấu** MUST gồm đúng **52 khe** dưới đây, chia theo vòng. Danh sách này dẫn xuất từ cột *Tên* của trang `Âm thanh` trên wiki Fandom — cùng nguồn với `docs/source/fandom-olympia-26-luat-choi.md` — sau khi loại **biến thể ngoài O26** và **khe truyền hình không ứng với sự kiện engine**. Mọi khe ở đây MUST thuộc nhóm **hướng khán giả** theo `FR-074a`. Hệ thống MUST NOT tách khe theo **độ dài đồng hồ**: mỗi vòng có đúng **một** khe *đếm giờ*, vì `timeSeconds` là metadata của **từng câu** và mọi timer là RuleConfig. *(US-008 · PRD-REQ-078 · `QĐ-079` · AC-069d)*

  - **Khởi động (11)**: bắt đầu vòng · chuẩn bị sẵn sàng · mở câu hỏi · nhạc nền câu hỏi · đếm giờ · báo hiệu hết giờ · đếm ngược chờ tín hiệu · tín hiệu trả lời · đúng · sai · hoàn thành lượt
  - **Vượt chướng ngại vật (14)**: bắt đầu vòng · công bố có Chướng ngại vật · bày bàn cờ hàng ngang · mở một hàng ngang · mở câu hỏi hàng ngang · đếm giờ hàng ngang · hiện đáp án hàng ngang · đúng hàng ngang · sai hàng ngang · mở miếng ghép hình ảnh · tín hiệu trả lời Chướng ngại vật · đếm ngược chờ tín hiệu · mở toàn bộ hàng ngang · đúng Chướng ngại vật
  - **Tăng tốc (8)**: bắt đầu vòng · mở câu hỏi · đếm giờ · nhận bài làm · hiện đáp án · mở từng bài làm theo thứ hạng tốc độ · đúng · sai
  - **Về đích (15)**: bắt đầu vòng · bước lên sân khấu · mở gói điểm · chọn mức điểm · mở câu hỏi · giới thiệu khách mời câu hỏi thực hành · mở câu hỏi thực hành · đếm giờ suy nghĩ · đếm giờ thực hành · đặt Ngôi sao hy vọng · đúng · sai · mở cửa sổ cướp quyền · tín hiệu cướp quyền · về vị trí
  - **Câu hỏi phụ (4)**: mở câu hỏi · đếm giờ · tín hiệu trả lời · đúng

- **FR-074e**: Danh sách `FR-074d` MUST là **tập đóng ở v1** — hệ thống MUST NOT tự sinh thêm khe từ sự kiện engine nào khác, và MUST NOT bỏ khe nào trong danh sách kể cả khi chưa gán file. Khe **thiếu trên nguồn** *(ví dụ: vòng Câu hỏi phụ không có khe cho phán quyết **sai** vì wiki không liệt kê)* MUST giữ nguyên tình trạng thiếu; bổ sung là một quyết định mới của chủ dự án, MUST NOT suy diễn từ luật. *(US-008 · PRD-REQ-078 · `QĐ-079` · AC-069d)*
- **FR-075**: Khe **chưa gán file** MUST là **im lặng**, và MUST NOT sinh lỗi hay cảnh báo. *(US-008 · PRD-REQ-078 · `QĐ-079` · AC-070)*
- **FR-076**: Hệ thống MUST NOT có **bộ âm thanh mặc định**. *(US-008 · PRD-REQ-078 · `QĐ-079`, `RISK-005` · AC-071)*
- **FR-077**: Engine MUST chỉ phát **tín hiệu ngữ nghĩa**; ánh xạ tín hiệu sang âm thanh MUST là cấu hình **phía client**. Sửa luật MUST NOT đụng âm thanh, và ngược lại. *(US-008 · PRD-REQ-078 · `QĐ-079`, `CLAUDE.md` §Quy ước code · AC-072)*
- **FR-078**: Client MUST tải trước toàn bộ file âm thanh của contest khi vào phòng. *(US-008 · PRD-REQ-078 · `CLAUDE.md` §Quy ước code · AC-073)*
- **FR-079**: Giới hạn kích thước file âm thanh MUST đọc từ cấu hình môi trường; hệ thống MUST NOT hard-code con số ở code hay ở thông điệp từ chối. *(US-008 · PRD-REQ-078, PRD-REQ-085 (vế giới hạn media) · `CLAUDE.md` §Quy ước code · AC-074)*
- **FR-080**: Đổi chủ đề hoặc đổi file âm thanh của một contest MUST NOT đụng một trận **đã bắt đầu** — cấu hình đã đóng băng tại cú bấm bắt đầu trận. *(US-008 · PRD-REQ-078 · `PRD-REQ-028`, `GR-037` §Điều kiện (khuôn chụp-vào-trận) · AC-075)*

#### Nhóm I — Ràng buộc xuyên suốt (mọi US)

- **FR-081**: Mọi thao tác bất đồng bộ trên màn khán giả, lớp phủ và màn MC MUST hiển thị trạng thái đang xử lý rõ ràng. *(mọi US · `NFR-36` · `CLAUDE.md` §UX · AC-076)*
- **FR-082**: Nội dung chính của màn khán giả và màn MC MUST nằm gọn trong **một** khung nhìn tham chiếu, không nhồi nhét và không ép cuộn để thấy phần quan trọng. *(US-001, US-004 · `NFR-37` · AC-077)*
- **FR-083**: Mọi chuỗi giao diện MUST chỉ dùng tiếng Việt, tách ra file hằng số, và mọi mốc thời gian hiển thị MUST theo UTC+7. *(mọi US · `NFR-38` · `CLAUDE.md` §Quy ước code · AC-078)*
- **FR-084**: Bố cục màn khán giả MUST đưa **sân khấu và bảng điểm** lên trước — đó là thông tin quan trọng nhất với vai đang xem. *(US-001 · `CLAUDE.md` §UX · AC-079)*
- **FR-085**: Ánh xạ sự kiện ngữ nghĩa → animation MUST là một **module độc lập** với engine và với rule. *(mọi US · `CLAUDE.md` §Quy ước code, `EVENT-032` · AC-072)*
- **FR-085a**: Khi một lớp phủ đang bật mà trạng thái bên dưới **chưa cho nội dung có nghĩa**, hai kênh public MUST dựng lớp phủ đó **bình thường với dữ liệu hiện có**. Hệ thống MUST NOT hiện một **trạng thái rỗng** riêng, MUST NOT để trống phần dữ liệu của lớp phủ, và MUST NOT từ chối cú mở vì lý do *"chưa đáng hiển thị"* — việc mở là **phán quyết của admin** (`QĐ-001`), và các quy tắc nội dung đã có *(đồng hạng, điểm âm, đủ số ghế)* đã định nghĩa trọn vẹn kết cục cho mọi giá trị dữ liệu, gồm cả giá trị toàn `0`. *(mọi US · PRD-REQ-075 · `GR-025` C5, `EVENT-032` §Hợp lệ ở, `QĐ-001` · AC-079a)*

### Key Entities

- **Màn khán giả**: Kênh public, **một chiều server → client**, vào bằng **một URL** chứa mã phòng. Không tài khoản, không vai, không permission. Đây là kênh **duy nhất** bị nút *khoá cổng* chặn. *(`TERM-007`, `QĐ-088`, `QĐ-096`, `QĐ-102`)*
- **Lớp phủ dựng stream**: Cùng mô hình truy cập với màn khán giả, nhưng là **frame stream 1920×1080 nền trong suốt** cho phần mềm dựng hình. Nhận **mọi thứ** cùng lúc và cùng điều kiện với màn khán giả — gồm đáp án theo `GR-037` **và** đáp án Chướng ngại vật theo `GR-012`; **không** có lệnh cấm riêng nào cho kênh này. **Không** bị khoá cổng chặn, và là **nguồn phát mặc định** của nhóm khe hướng khán giả. *(`TERM-007`, `QĐ-080`, `QĐ-102`)*
- **Màn MC**: Kênh **có xác thực**, chữ rất to, hiển thị câu hỏi **và đáp án** mọi lúc qua `PERM-045`. Có **đúng một** bề mặt quyền ghi trong toàn hệ thống — prompt duyệt giành quyền điều khiển (`PERM-054`). *(`TERM-005`, `STATE-039`)*
- **Mốc câu khép**: Thời điểm **không còn ai được trả lời câu đó nữa** — mốc **duy nhất** mà đáp án được phép rời server tới hai kênh public. Trùng cú bấm chấm ở mọi vòng **trừ** Về đích và trừ các ca có kích hoạt tay. *(`TERM-057`, `GR-037`, `INV-017`)*
- **Cờ hiện đáp án sau khi chấm**: Cờ **cấp TRẬN**, mặc định **BẬT**, chụp vào trận lúc bắt đầu. Nó chỉ điều khiển nhánh **sau** mốc câu khép; nó **không** mở được đường nào trước mốc. *(`QĐ-062`, `QĐ-080`, `GR-037` C3, C4, C5)*
- **Lớp hiển thị công bố kết quả**: Lớp phủ `STATE-033`, chứa tên · điểm · thứ hạng của mọi ghế. Ba ràng buộc nội dung: thứ hạng do **server** tính · hoà ⇒ **đồng hạng, hạng kế nhảy qua** · điểm là kết quả tính từ nhật ký sự kiện tại thời điểm mở, **âm hiển thị bình thường**. *(`STATE-033`, `QĐ-049`, `GR-028`)*
- **Banner tạm dừng**: Lớp phủ `STATE-040`, **không chữ**, là lớp phủ **duy nhất** chặn thao tác mà không gắn với một thao tác cụ thể nào. **Loại trừ lẫn nhau với đồng hồ** theo cả hai chiều. Trên máy thí sinh nó **che toàn bộ màn**; trên hai kênh public nó là báo hiệu **thuần thị giác**. *(`STATE-040`, `QĐ-050`, `QĐ-115`)*
- **Khe âm thanh**: Một điểm neo ngữ nghĩa mà admin gán một file vào. Khe **trống là im lặng**; **không** có bộ mặc định. Danh sách khe phủ **cả sự kiện điều khiển**, không chỉ mốc thi đấu: **58 khe** — `52` mốc thi đấu *(dẫn xuất từ trang `Âm thanh` của wiki Fandom, lọc biến thể ngoài O26 và khe truyền hình; xem `FR-074d`)* cộng `6` khe điều khiển. Mỗi vòng có đúng **một** khe *đếm giờ*; **không** khe nào mang con số giây trong tên. Mỗi khe thuộc đúng **một** trong hai nhóm — **hướng khán giả** *(mốc thi đấu + mở màn công bố)* hoặc **chỉ-admin** *(năm thao tác sự cố)* — và tiêu chí phân định là *khán giả có được nghe khe này không*. *(`QĐ-079`, `QĐ-076`, `RISK-005`)*
- **Nguồn phát**: Giá trị cấu hình với đúng hai lựa chọn — **máy admin** hoặc **lớp phủ dựng stream** — mặc định **lớp phủ**, bật/tắt và đổi được bởi admin. Chỉ áp cho **nhóm hướng khán giả**; nhóm chỉ-admin không đi qua nó. **Màn khán giả không bao giờ là nguồn phát.** Thiếu lớp phủ ⇒ im lặng, **không** lui tự động. *(`QĐ-102`, `QĐ-001` · FR-074b, FR-074c)*
- **Chủ đề hiển thị**: Cấu hình cấp contest, đặt qua `PERM-023`, gồm đúng **ba trục tĩnh** — **bảng màu · logo giải · ảnh nền**. **Không** chứa tài sản động *(hình hiệu, clip, animation)* và **không** chứa phông chữ. Áp cho đúng **hai kênh public**; màn thí sinh, màn MC và màn admin nằm **ngoài** phạm vi chủ đề. *(`PRD-REQ-078`, `PERM-023`, `CLAUDE.md` §Quy ước code)*

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: **0** đường ghi tồn tại trên kênh khán giả và kênh lớp phủ — rà **100%** bề mặt của hai kênh, và với mỗi loại sự kiện ghi trong hệ thống, xác minh không có đích đến nào nhận được nó từ hai kênh này.
- **SC-002**: **0** byte mang đáp án chuẩn rời server tới màn khán giả hoặc lớp phủ trước mốc câu khép, trong một trận đầy đủ năm vòng có dùng cả *Huỷ kết quả*, cả **kích hoạt tay** và cả một câu Về đích bị cướp quyền — xác minh bằng **bắt gói tin**, không bằng quan sát giao diện, và lặp lại với cờ hiện đáp án ở **cả hai** giá trị.
- **SC-003**: **100%** số câu khép đều đẩy đáp án tới **cả hai** kênh public **cùng một mốc**, với chênh lệch dưới một khung hình trên cùng một mạng LAN — đo trên toàn bộ bảng §Mốc câu khép theo vòng, gồm cả ca *câu bị bỏ qua*.
- **SC-004**: **0** trường hợp đáp án xuất hiện trên hai kênh public trong khoảng từ cú bấm chấm Sai của người thi chính Về đích tới khi người cướp được chấm — thử với **20** câu Về đích, gồm cả ca hết cửa sổ không ai bấm.
- **SC-004b**: **100%** số lần một cú **đóng hiển thị bằng tay** còn hiệu lực đều chặn được cú đẩy của engine tại mốc câu khép — thử **20** câu ở cả năm vòng với cờ hiện đáp án **BẬT**; và **0** lần cú đóng đó dính sang câu kế tiếp.
- **SC-005**: **0** nút điều khiển trận tồn tại trên màn MC ngoài prompt duyệt giành quyền; và **100%** số loại sự kiện ghi gửi thẳng tới server dưới danh nghĩa phiên MC đều bị từ chối — thử với **toàn bộ** danh sách sự kiện của admin và của thí sinh, và mỗi lần từ chối để lại một dòng nhật ký.
- **SC-006**: **100%** số lần đáp án tới màn MC đều để lại đúng một dòng nhật ký — đối chiếu số lần xem với số dòng audit sau **50** lần xem liên tiếp.
- **SC-007**: **100%** số lần mở rồi đóng lớp công bố **giữa một vòng đang chạy** đều cho ra trạng thái bên dưới **không đổi** — cùng câu, cùng cờ ghế, cùng hàng đợi, cùng giá trị đồng hồ theo server time; **0** event điểm được sinh, đo trên **20** lần mở-đóng ở cả năm vòng.
- **SC-008**: **100%** số nhóm hoà điểm đều hiển thị **đồng hạng** với hạng kế **nhảy qua** đúng số người đồng hạng — thử với nhóm 2, 3 và 4 người, và với ca mở lớp công bố ở `TIE_BREAK` khi nhóm chưa được phân định.
- **SC-009**: **0** lần điểm âm bị hiển thị thành `0` hay bị ẩn trên màn khán giả, lớp phủ và lớp công bố — thử với các giá trị `−5`, `−30` và `−100`.
- **SC-010**: **0** thời điểm nào banner tạm dừng và một đồng hồ đang đếm cùng có mặt — thử mở banner ở **100%** các trạng thái có đồng hồ chạy, và thử start timer ở **100%** các trạng thái có banner bật; cả hai chiều đều phải cho ra một control **không bật**.
- **SC-010b**: **0** thời điểm nào banner tạm dừng và lớp công bố kết quả cùng bật — thử **cả hai chiều** ở mọi trạng thái cấp trận mà cả hai lớp đều mở được; cả hai chiều cho ra một control **không bật** cộng một phản hồi **ép không được**; và **0** tổ hợp lớp phủ **khác** bị chặn nhầm theo.
- **SC-011**: **0** ký tự văn bản hiển thị trên banner tạm dừng ở màn khán giả và lớp phủ — rà bằng ảnh chụp màn hình ở **mọi** trạng thái mà banner bật được.
- **SC-012**: **100%** số lần mở và đóng banner đều để lại một dòng nhật ký, và **100%** tín hiệu của thí sinh tới server trong lúc banner bật đều xuất hiện trong lịch sử với kết cục **trơ** — **0** tín hiệu bị drop.
- **SC-013**: **0** bản tin nào ngoài số điểm mới tới kênh khán giả sau mỗi thao tác can thiệp của admin — thử **bỏ vòng**, **chạy lại vòng**, **kết thúc sớm**, **sửa danh sách đề** và **gỡ lệnh cấm**, mỗi loại ít nhất một lần, đối chiếu bằng bắt gói tin.
- **SC-014**: **0** lần khuyến nghị lượt xuất hiện trên kênh khán giả hoặc lớp phủ, trong khi **100%** số lần nó xuất hiện trên màn admin và màn MC — thử trọn một vòng Về đích bốn lượt.
- **SC-015**: **100%** khe âm thanh đã gán đều phát đúng file khi sự kiện tương ứng xảy ra, và **100%** khe chưa gán đều **im lặng** với **0** lỗi và **0** cảnh báo — rà toàn bộ danh sách khe.
- **SC-015b**: **0** lần một khe thuộc nhóm **chỉ-admin** phát ra tiếng trên màn khán giả hoặc lớp phủ dựng stream — đo bằng ghi luồng âm thanh của hai kênh trong một trận có dùng **cả năm** thao tác sự cố, ở **cả hai** giá trị của nguồn phát; và **0** lần màn khán giả phát ra tiếng ở bất kỳ cấu hình nào.
- **SC-015c**: Danh sách khe đếm được đúng **58** — `52` mốc thi đấu chia `11 · 14 · 8 · 15 · 4` theo năm vòng, cộng `6` khe điều khiển; **0** khe mang một con số giây trong tên; **0** khe của `Ô mạo hiểm`, của cấu trúc *Bộ 8/12/16 câu hỏi*, hay của nhóm truyền hình ngoài trận; và danh sách **không đổi** sau khi sửa `timeSeconds` của **10** câu hỏi bất kỳ ở cả năm vòng.
- **SC-015d**: **100%** số màn có người thao tác — thí sinh, MC, admin — giữ nguyên bảng màu hệ thống khi contest mang một chủ đề đặt màu tương phản thấp nhất dựng được; và đúng **2** màn *(khán giả, lớp phủ)* đổi theo chủ đề.
- **SC-016**: Lớp phủ dựng stream giữ đúng khung `1920×1080` và **0** pixel nền không trong suốt ngoài vùng có đồ hoạ — đo bằng phân tích kênh alpha của một khung hình đã xuất.
- **SC-017**: **100%** số lần lớp phủ kết nối lại trong lúc cổng phòng khán giả **đang khoá** đều thành công mà không cần thao tác mở cổng nào — thử **10** lần ngắt-nối liên tiếp.
- **SC-018**: Nội dung chính của màn khán giả và màn MC nằm trọn trong một khung nhìn tham chiếu, đo bằng **0** phần tử quan trọng nằm dưới mép dưới khi trang vừa tải — thử ở cả năm vòng và ở `LOBBY`.

## Out of Scope

- **Luật số học và mô hình engine**: mức điểm và hình phạt của từng vòng, thang xếp hạng, cơ chế cướp quyền, **điểm là hàm của nhật ký sự kiện** và cơ chế hoàn nguyên bằng event đảo ngược, server time và độ phân giải mili-giây, hàng đợi tín hiệu, **kích hoạt tay** một tín hiệu sau *Huỷ kết quả*, rút đề và không lặp câu, điều kiện kích hoạt và thể thức Câu hỏi phụ — thuộc EPIC-006 (`specs/006-game-engine-luat-thi-dau`). Spec này chỉ đặc tả **những gì ba kênh trình diễn nhìn thấy**.
- **Bề mặt điều khiển của admin**: cú bấm mở và đóng lớp công bố, cú bấm mở và đóng banner tạm dừng, cú bấm công bố Chướng ngại vật, cú bấm mở và đóng đáp án bằng tay (`PERM-044`), bỏ / chạy lại / kết thúc sớm một vòng, sửa danh sách đề, gỡ lệnh cấm, điều chỉnh điểm thủ công, ba hạng dialog và cảnh báo lệch luật — thuộc EPIC-007 (`specs/007-dieu-khien-can-thiep-admin`). Riêng `PRD-REQ-049` và `PRD-REQ-041` khai cả EPIC-009, nên **hệ quả trên ba kênh trình diễn** của chúng nằm trong spec này.
- **Màn thi đấu của thí sinh**: chuông, gửi đáp án, bảng điểm trên máy thí sinh, chọn hàng ngang, Ngôi sao hy vọng, khôi phục sau mất kết nối, và **mặt máy thí sinh của banner tạm dừng** *(`QĐ-115` — banner che toàn bộ màn)* — thuộc EPIC-008 (`specs/008-trai-nghiem-thi-sinh`).
- **Xác thực và vào phòng**: đăng nhập, mã phòng 6 số và URL vào phòng, catalog permission gồm `PERM-044`, `PERM-045`, `PERM-047`, `PERM-048`, `PERM-054`, mô hình vai, **cơ chế giành quyền điều khiển** `EVENT-050` và luật phân xử khi nhiều người cùng giành — thuộc EPIC-001 (`specs/001-xac-thuc-phan-quyen`). Spec này chỉ đặc tả **mặt màn MC** của `PRD-REQ-074`: prompt hiện ở đâu, không đóng được, và không có bề mặt ghi nào khác.
- **Vòng đời cấp trận**: cú bấm bắt đầu trận và việc đóng băng cấu hình, chốt trận, huỷ trận, niêm phong — thuộc EPIC-005.
- **Contest builder**: việc **đặt** giá trị chủ đề, **tải file** âm thanh lên trong luồng dựng contest, đặt cờ *hiện đáp án sau khi chấm* — thuộc EPIC-004. Spec này đặc tả **hành vi** của chủ đề và âm thanh lúc trận chạy, không đặc tả màn cấu hình.
- **Kho đề và bộ đề** (EPIC-002) · **nhập/xuất gói contest** (EPIC-003) · **sau trận: bảng điểm cuối, biên bản theo lần chạy, thống kê** (EPIC-010) · **nhật ký thao tác chung và hạn lưu trữ** (EPIC-011) · **hai hồ sơ triển khai, giới hạn media, rate-limit và nút khoá cổng** (EPIC-012 — spec này chỉ nhận ràng buộc `QĐ-102` rằng khoá cổng **không** chặn lớp phủ).
- **Truyền video** (`NON-GOAL-003`) — hệ thống sinh **lớp đồ hoạ**, không sinh hay truyền hình ảnh video.
- **Phát lại một trận** (`product-discovery.md` §5 E-10) · **giao diện luyện tập solo và vai trainer** (v1.5) · **luật cho số ghế trên 4** (`NON-GOAL-012`) · **thi đội** (`NON-GOAL-013`).

> **Hai mục `Out of scope` của EPIC-009 trong `docs/PRD.md` §11 nằm TRONG spec này, không nằm ngoài.** *"Báo cho khán giả về can thiệp của admin"* và *"đẩy khuyến nghị lượt xuống khán giả"* là hai thứ bị **cấm**, không phải hai thứ chưa làm — nên chúng xuất hiện dưới dạng `MUST NOT` ở FR-065 → FR-071 và được kiểm bằng SC-013, SC-014.

## Open Questions

> Mục này chỉ chứa câu hỏi **CÒN MỞ**. Mọi quyết định đã chốt nằm ở §Requirements và §Acceptance Scenarios; spec này không giữ bản ghi phân xử song song.

**Không còn câu hỏi mở nào trong phạm vi EPIC-009.**

## Traceability Matrix

| User story | PRD requirement | Game rules | Functional requirements | Acceptance scenarios |
|---|---|---|---|---|
| US-001 | PRD-REQ-073, PRD-REQ-107, PRD-REQ-041 | `GR-037` §bảng ba loại thông tin, `GR-028` §Biên, `GR-004` C5, `GR-016` §Điều kiện, `GR-012` C2, C3 · `INV-018` · `NFR-06`, `NFR-20` | FR-001 → FR-010 | AC-001 → AC-008 |
| US-002 | PRD-REQ-073, PRD-REQ-107, PRD-REQ-049 | `GR-037` C4, C5, C9 · `INV-017` · `NFR-20`, `NFR-21` | FR-011 → FR-018 | AC-009 → AC-014 |
| US-003 | PRD-REQ-049, PRD-REQ-073 | `GR-037` *(toàn bộ bảng + bảng §Mốc câu khép theo vòng)*, `GR-012` C2, `GR-008` §Điều kiện · `INV-017` · `TERM-057` | FR-019 → FR-032 **+ FR-015, FR-016, FR-054** | AC-015 → AC-029 **+ AC-012, AC-013, AC-049** |
| US-004 | PRD-REQ-074, PRD-REQ-049, PRD-REQ-077 | `GR-037` C2, §Bấm trùng, §Không đổi gì · `STATE-039`, `EVENT-051`, `T-092` → `T-095` · `PERM-045`, `PERM-054` | FR-033 → FR-041 **+ FR-004, FR-028, FR-032** | AC-030 → AC-038 |
| US-005 | PRD-REQ-075, PRD-REQ-041 | `GR-028` §Điều kiện, C4, C5, §Biên, `GR-025` C5, `GR-016` §Điều kiện · `STATE-033`, `EVENT-032`, `EVENT-033`, `T-087`, `T-088` · `INV-014`, `INV-018`, `INV-021` | FR-042 → FR-054 **+ FR-058a, FR-006, FR-007** | AC-039 → AC-049 **+ AC-058a, AC-005, AC-006** |
| US-006 | PRD-REQ-076 | `GR-035` *(vế đồng hồ)* · `STATE-040`, `STATE-039` §Cho phép, `STATE-033` §Vào, `EVENT-034`, `EVENT-035`, `T-089`, `T-090`, §Invalid transitions · `INV-006`, `INV-014`, `INV-016`, `INV-021` | FR-055 → FR-064 **+ FR-055a, FR-058a, FR-052** | AC-050 → AC-059 **+ AC-050a, AC-058a** |
| US-007 | PRD-REQ-077 | `GR-030` C1, C2, C3, §Ở phía viewer, `GR-016` C1 → C5, `GR-028` C4 · `INV-020` | FR-065 → FR-071 **+ FR-065a, FR-041** | AC-060 → AC-066 **+ AC-038, AC-069a** |
| US-008 | PRD-REQ-078 | — *(requirement khai **Related game rules: —**)*; vế **không báo khán giả** kéo `GR-030` §Ở phía viewer vào qua FR-074a | FR-072 → FR-080 **+ FR-072a, FR-072b, FR-074a → FR-074e, FR-065a** | AC-067 → AC-075 **+ AC-067a, AC-069a → AC-069d** |
| *(xuyên suốt)* | `NFR-36` → `NFR-39` · `CLAUDE.md` §UX, §Quy ước code | — | FR-081 → FR-085 **+ FR-085a** | AC-076 → AC-079 **+ AC-079a** |

### Bản đồ phủ bảng quyết định

> Mỗi outcome trong bảng quyết định của từng `GR-NNN` **thuộc phạm vi feature này** đều có ít nhất một acceptance scenario. Ca nào thuộc epic khác thì ghi rõ spec sở hữu, theo đúng cách `specs/006` → `specs/008` đã dùng — chúng **không** được đặc tả lại ở đây.

**Rule mà EPIC-009 khai trực tiếp**

| Game rule | Ca trong bảng quyết định | Acceptance scenarios |
|---|---|---|
| `GR-037` | C1 *(giữ `PERM-045`, phiên đang giữ quyền điều khiển)* | *(EPIC-007 — màn chấm của admin, `specs/007`)* |
| `GR-037` | C2 *(giữ `PERM-045`, phiên MC)* | AC-031, AC-032, AC-036 |
| `GR-037` | C3 *(không giữ `PERM-045`, reveal TẮT)* | AC-016 |
| `GR-037` | C4 *(không giữ, reveal BẬT, câu **chưa** khép)* | AC-015, AC-012 |
| `GR-037` | C5 *(không giữ, reveal BẬT, câu **đã** khép)* | AC-017, AC-013 |
| `GR-037` | C6 *(Về đích, cửa sổ cướp đang mở)* | AC-019 |
| `GR-037` | C7 *(câu bị bỏ qua)* | AC-020 |
| `GR-037` | C8 *(khép bằng Huỷ kết quả)* | AC-021 |
| `GR-037` | C8b *(đang chờ kích hoạt tay)* | AC-022 |
| `GR-037` | C9 *(đáp án Chướng ngại vật — ngoài phạm vi về **thời điểm**, trong phạm vi về **người nhận**)* | AC-023 |
| `GR-037` | §Cấm · §Thứ tự đánh giá · §Bấm trùng · §Không đổi gì · §Biên · §bảng ba loại thông tin | AC-018 · AC-025, AC-027 · AC-024 · AC-029 · AC-019 · AC-028 |
| `GR-037` §bảng **Mốc câu khép theo vòng** | Khởi động lượt riêng · Khởi động lượt chung *(hai nhánh)* · VCNV hàng ngang · VCNV ô trung tâm · Tăng tốc · **Về đích** *(ba nhánh)* · **mọi vòng có kích hoạt tay** | AC-017 · AC-017, AC-020 · AC-017 · AC-017 · AC-017 *(vế "cả bảng")* · AC-019 · AC-022 |

**Các rule mà PRD-REQ của EPIC-009 tham chiếu — chỉ phủ phần thuộc epic này**

| Game rule | Ca thuộc phạm vi EPIC-009 | Acceptance scenarios |
|---|---|---|
| `GR-028` | §Điều kiện *(điểm là kết quả tính từ nhật ký)* · C1, C2, C3 *(sinh event — **EPIC-006**)* · C4 *(bỏ vòng ⇒ tính lại ⇒ điểm trên kênh public đổi đột ngột)* · C5 *(hỏi điểm tại một mốc ⇒ không sinh event)* · §Biên *(không sàn, không trần)* | AC-046 · *(EPIC-006 — `specs/006`)* · AC-062 · AC-046 · AC-005 |
| `GR-030` | C1 *(bỏ vòng)* · C2 *(chạy lại vòng)* · C3 *(kết thúc sớm)* · C4 *(không có đường vòng → vòng — **EPIC-007**)* · C5 *(bấm bỏ lần hai — **EPIC-007**)* · §Ở phía viewer *(điểm đổi đột ngột, không hiệu ứng)* | AC-062 · AC-063 · AC-064 · *(EPIC-007 — `specs/007`)* · *(EPIC-007)* · AC-061 |
| `GR-035` | C1 → C5, C6, C6b, C6c *(mọi ca đều là phân xử **server-side** về thời gian — **EPIC-006**)* · §Điều kiện *(client chỉ hiển thị)* · vế **đồng hồ** mà `PRD-REQ-076` trích *(banner và đồng hồ loại trừ lẫn nhau)* | *(EPIC-006 — `specs/006`)* · AC-004 · AC-052, AC-053 |
| `GR-025` | C1, C2, C3 *(bốc thăm, xác nhận, bốc lại — **EPIC-006/007**)* · C4 *(không đủ 3 câu ⇒ vòng không mở — **EPIC-006**)* · **C5** *(admin không phân định ⇒ **đồng hạng**, standard competition ranking)* | *(EPIC-006, EPIC-007)* · *(EPIC-006)* · AC-044, AC-045 |
| `GR-016` | C1, C2, C3, C4 *(khuyến nghị lượt — mặt **hiển thị** trên màn MC và mặt **cấm hiển thị** trên kênh public)* · C5 *(dialog cảnh báo — **EPIC-007**)* · §Điều kiện *(điểm âm tham gia bình thường)* | AC-038, AC-065 · *(EPIC-007)* · AC-005 |
| `GR-004` | C1, C2, C3, C4 *(số học phán quyết — **EPIC-006**)* · **C5** *(0 điểm bị `−5` ⇒ `−5`, không có sàn ⇒ hiển thị nguyên giá trị)* · C6 *(không mở lại chuông — **EPIC-006**)* | *(EPIC-006)* · AC-005 · *(EPIC-006)* |
| `GR-012` | C1 *(vòng kết thúc — **EPIC-006**)* · **C2** *(admin bấm công bố ⇒ mở mọi miếng ghép và hiện Chướng ngại vật cho **cả hai kênh public**)* · **C3** *(không bấm ⇒ không tắc)* · C4 *(hàng ngang chưa hỏi ⇒ trả lại kho — **EPIC-006**)* · C5 *(bấm hai lần — **EPIC-007**)* · C6, C7, C8 *(**EPIC-006**)* | *(EPIC-006)* · AC-008, AC-023 · AC-008 · *(EPIC-006)* · *(EPIC-007)* · *(EPIC-006)* |

**Trạng thái, sự kiện và transition**

| Nguồn | Nội dung | Acceptance scenarios |
|---|---|---|
| §F bảng nhóm lớp phủ | ba nhóm `F.1` / `F.2` / `F.3` khác nhau ở *ai thấy* và *có chặn thao tác bên dưới không*; dòng `F.3` **không gồm MC** — banner chặn-toàn-cục tác động lên máy thí sinh và hai kênh public | AC-039, AC-050, AC-050a |
| `STATE-033` | lớp công bố: §Vào · §Cho phép · §Cấm · §Ra · ba ràng buộc nội dung | AC-041 · AC-039 · AC-043, AC-047, AC-049 · AC-042, AC-048 · AC-043, AC-044, AC-046 |
| `STATE-034` | banner *"đang kết nối lại"* nằm trên **máy ghế mất kết nối**, **không** trên kênh public | AC-060 *(vế no-change)* |
| `STATE-039` | prompt duyệt giành quyền: §Vào · §Cho phép · §Cấm · §Ra | AC-034 · AC-037 · AC-035, AC-034 · AC-034 |
| `STATE-040` | banner tạm dừng: §Mô tả · §Vào · §Cho phép · §Cấm · §Ra · bốn ràng buộc suy ra · ràng buộc hai chiều | AC-050 · AC-052 · AC-050 · AC-051, AC-053, AC-054 · AC-059 · AC-055, AC-056, AC-057, AC-058 · AC-052, AC-053 |
| `EVENT-032`, `EVENT-033`, `T-087`, `T-088` | mở và đóng lớp công bố; server đẩy **một** sự kiện đã tính; không có bộ đếm tự đóng | AC-041, AC-043 · AC-042 · AC-047 |
| `EVENT-034`, `EVENT-035`, `T-089`, `T-090` | mở và đóng banner; guard đồng hồ; máy admin không bị phủ | AC-052 · AC-054 · AC-050 · AC-059 |
| `EVENT-051`, `T-093`, `T-094`, `T-095` | MC duyệt / từ chối; cú giành **mất đối tượng** khi holder nối lại | AC-034 *(gồm cả §biến thể)* |
| §*Invalid transitions* | mở banner khi đồng hồ chạy ⇒ toast · start timer khi banner bật ⇒ toast · **mở lớp công bố khi banner bật và ngược lại ⇒ toast** · sự kiện ghi từ kênh public ⇒ **không có đích đến** | AC-052 · AC-053 · AC-058a · AC-002, AC-003 |
| `INV-006`, `INV-014`, `INV-016`, `INV-017`, `INV-018`, `INV-021` | tín hiệu luôn có outcome · **ba** chỗ chặn cứng, cặp loại trừ lớp phủ **không** là chỗ thứ tư · đồng hồ không đóng băng · đáp án chỉ rời server tới `PERM-045` · điểm được phép âm · lớp phủ không đổi trạng thái bên dưới; cặp *banner × lớp công bố* là **ngoại lệ duy nhất** được liệt kê, mọi tổ hợp lớp phủ khác vẫn bật cùng lúc được | AC-056 · AC-058a · AC-037 · AC-015, AC-025 · AC-005 · AC-047, AC-048, AC-058a, AC-059 |
| `permissions.md` `PERM-044` vs `PERM-045` | đọc trên màn của chính mình ≠ công bố cho mọi màn; `PERM-044` **thắng cờ**, nên cú đóng tay **chặn** cú đẩy tại mốc câu khép | AC-036 · AC-024a |
| `QĐ-088`, `QĐ-096`, `QĐ-102` | kênh một chiều · vào bằng một URL · khoá cổng không chặn lớp phủ | AC-002 · AC-001 · AC-011 |
| `NFR-36` → `NFR-39` | trạng thái đang xử lý · một khung nhìn · tiếng Việt và UTC+7 · bố cục theo vai | AC-076 · AC-077 · AC-078 · AC-079 |

### Acceptance Scenarios (chi tiết)

> Mọi kịch bản đều giả định **một** phiên admin đang giữ quyền điều khiển (`GR-026` §Đồng thời) và mọi mốc thời gian đều là **server time** (`INV-004`). Trừ khi nói khác, trận là trận **chính thức**, mode **sân khấu**, bốn ghế, và cờ *hiện đáp án sau khi chấm* đang **BẬT** (mặc định — `QĐ-062`).

#### Nhóm A — Màn khán giả public (US-001)

**AC-001** — *US*: US-001 · *FR*: FR-001 · *GR*: `GR-037` §bảng ba loại thông tin
**Given** một trận đang chạy ở Khởi động lượt riêng với mã phòng `482913`, điểm bốn ghế là `10 · 0 · 20 · −5`, và một máy tính **chưa từng đăng nhập** vào hệ thống, không có phiên nào và không có cookie nào,
**When** người xem mở đường dẫn phòng chứa mã `482913`,
**Then** màn khán giả hiện ngay trạng thái trận hiện tại và điểm của **cả bốn** ghế; **không** có màn đăng nhập, **không** có ô nhập mã riêng, **không** có bước chờ duyệt; hệ thống **không** tạo tài khoản nào và **không** gán vai nào cho phiên xem này; và trạng thái trận **không đổi** — một người xem vào phòng không sinh event nào của trận.

**AC-002** — *US*: US-001 · *FR*: FR-002, FR-003 · *GR*: `GR-037` §Cấm
**Given** cùng trạng thái AC-001, màn khán giả đã vào phòng và đang nhận cập nhật,
**When** rà toàn bộ bề mặt của kênh khán giả và thử phát mỗi loại sự kiện ghi của hệ thống từ phía kênh này,
**Then** kênh khán giả nhận cập nhật qua đúng **một chiều server → client** cộng **một** lời gọi đọc lúc vào phòng; **không tồn tại** đường gửi sự kiện lên server; **không** sự kiện ghi nào có đích đến; và điểm cùng trạng thái trận **không đổi** sau mọi lần thử.

**AC-003** — *US*: US-001 · *FR*: FR-003, FR-004, FR-009 · *GR*: `GR-037` §Thứ tự đánh giá
**Given** cùng trạng thái AC-001, và catalog phân quyền của hệ thống đã nạp đầy đủ,
**When** rà catalog tìm một vai hoặc một permission dành cho khán giả hay lớp phủ, rồi thử mở kênh **hai chiều** từ một phiên không xác thực,
**Then** catalog **không** chứa vai *"khán giả"* hay *"lớp phủ"*, và **không** chứa permission nào cấp cho hai kênh này; kênh hai chiều **chỉ** chấp nhận phiên đã xác thực mang vai admin, thí sinh hoặc MC; và việc read-only của hai kênh public **không** được hiện thực bằng một guard bỏ sự kiện ghi — nhánh đó không tồn tại về cấu trúc.

**AC-004** — *US*: US-001 · *FR*: FR-005 · *GR*: `GR-037` §bảng ba loại thông tin, `GR-035` §Điều kiện
**Given** một trận đang ở Khởi động lượt chung, câu 5 đang mở, chưa ai bị chấm, điểm bốn ghế là `20 · 20 · 10 · 0`, màn khán giả đang xem,
**When** admin chấm **Đúng** cho ghế 2,
**Then** màn khán giả hiện điểm mới `20 · 30 · 10 · 0`; điểm tới kênh này **không** phụ thuộc mốc câu khép và **không** phụ thuộc cờ hiện đáp án — điểm là thông tin công khai; đồng hồ hiển thị trên kênh này dựng từ **mốc server**, và client **chỉ hiển thị**, không tự tính.

**AC-005** — *US*: US-001, US-005 · *FR*: FR-006, FR-051 · *GR*: `GR-028` §Biên, `GR-004` **C5**, `GR-016` §Điều kiện · `INV-018`
**Given** một trận trong đó ghế 4 đang có `0` điểm và vừa bị chấm **Sai** ở lượt chung với hình phạt `−5`, ghế 1 đang có `−100`, màn khán giả và lớp phủ đang xem, lớp công bố đang mở,
**When** kênh public nhận bảng điểm và bảng xếp hạng mới,
**Then** ghế 4 hiện **`−5`** và ghế 1 hiện **`−100`** nguyên giá trị trên **cả ba** bề mặt — bảng điểm màn khán giả, bảng điểm lớp phủ và lớp công bố; hệ thống **không** kẹp về `0`, **không** ẩn, và **không** đổi dấu; thứ tự trên bảng xếp hạng tính với giá trị âm tham gia bình thường; và **không** event nào được sinh bởi việc hiển thị.

**AC-006** — *US*: US-001, US-005 · *FR*: FR-007, FR-051
**Given** một trận **ba** thí sinh — ghế 4 là **ghế bỏ thi**, vô hiệu hoá từ trước cú bấm bắt đầu trận và điểm luôn `0`, ba ghế còn lại có `30 · 20 · 10`,
**When** màn khán giả và lớp công bố dựng bảng,
**Then** cả bốn ghế đều có mặt, ghế 4 hiện `0`; hệ thống **không** hard-code con số `4` ở bất kỳ chỗ dựng bảng nào; xếp hạng dựng bình thường trên ba ghế còn lại; và điểm của ghế bỏ thi **không đổi** ở mọi thời điểm của trận.

**AC-007** — *US*: US-001 · *FR*: FR-008 · *GR*: `GR-035` §Điều kiện
**Given** một trận đang ở Tăng tốc với một câu đang đếm giờ, và số khán giả đang xem tăng từ `5` lên `500` trong 30 giây,
**When** bốn ghế gửi bài và server xếp hạng theo mốc nhận,
**Then** thứ tự chuông, giá trị đồng hồ và thứ hạng tốc độ **không đổi** so với cùng kịch bản chạy với `5` khán giả; hai kênh public nằm ngoài kênh của lõi thi đấu; và số khán giả **không** đi vào phép tính nào của luật.

**AC-008** — *US*: US-001 · *FR*: FR-010 · *GR*: `GR-012` **C2**, **C3**
**Given** một vòng Vượt chướng ngại vật trong đó **cả bốn** thí sinh đã bị loại, bàn cờ còn hai hàng ngang ở trạng thái *chờ*, màn khán giả đang xem,
**When** admin bấm **công bố** Chướng ngại vật,
**Then** màn khán giả hiện **mọi** miếng ghép đã mở và hiện Chướng ngại vật; ở nhánh admin **không** bấm thì màn khán giả giữ nguyên bàn cờ và vòng **vẫn** kết thúc được — đây không phải trạng thái tắc; và điểm hàng ngang đã kiếm được của mọi ghế **không đổi** ở cả hai nhánh.

#### Nhóm B — Lớp phủ dựng stream (US-002)

**AC-009** — *US*: US-002 · *FR*: FR-011, FR-012
**Given** một trận đang chạy, và một phần mềm dựng hình đã nạp đường dẫn lớp phủ của phòng đó, đặt lớp này **lên trên** một nguồn video,
**When** phần mềm dựng hình render một khung hình,
**Then** khung lớp phủ đúng **1920×1080**; mọi vùng không có đồ hoạ **trong suốt hoàn toàn** ở kênh alpha; nguồn video bên dưới nhìn thấy được nguyên vẹn qua các vùng đó; và trạng thái trận **không đổi** — nạp lớp phủ không sinh event nào.

**AC-010** — *US*: US-002 · *FR*: FR-013 · *GR*: `GR-037` §Cấm
**Given** lớp phủ đã vào phòng như AC-009,
**When** rà bề mặt của kênh lớp phủ và thử phát mỗi loại sự kiện ghi từ phía nó,
**Then** kết cục **y hệt** AC-002 và AC-003 — một chiều, không có đường ghi, không có vai và không có permission nào trong catalog; và trạng thái trận **không đổi**.

**AC-011** — *US*: US-002 · *FR*: FR-014
**Given** một trận đang chạy, admin đã bấm **khoá cổng** phòng khán giả, và lớp phủ dựng stream **đang** kết nối,
**When** lớp phủ mất mạng 5 giây rồi kết nối lại,
**Then** lớp phủ vào lại được **ngay**, không cần thao tác mở cổng nào; trong khi đó một khán giả **mới** thử vào bằng đường dẫn khán giả thì **không** vào được; khán giả **đang xem** không bị ngắt; và trạng thái trận **không đổi** ở cả ba nhánh.

**AC-012** — *US*: US-002, US-003 · *FR*: FR-015 · *GR*: `GR-037` **C4** · `INV-017`
**Given** một câu Về đích 30đ, người thi chính vừa bị chấm **Sai** nên cửa sổ cướp quyền đang mở — **câu chưa khép** — và cờ hiện đáp án đang **BẬT**,
**When** lớp phủ kết nối lại và nhận ảnh chụp trạng thái,
**Then** ảnh chụp **không** chứa đáp án chuẩn; nó đi qua **cùng** bộ lọc của `GR-037` như đường đẩy thường; **không** có đường tắt riêng nào cho gói khôi phục của kênh này; và trạng thái trận **không đổi** — việc kết nối lại là một phép đọc.

**AC-013** — *US*: US-002, US-003 · *FR*: FR-016, FR-017 · *GR*: `GR-037` **C5**
**Given** một câu Khởi động lượt chung vừa được admin chấm **Sai** nên **câu đã khép**, cờ hiện đáp án **BẬT**, cả màn khán giả và lớp phủ đang kết nối,
**When** server đẩy đáp án,
**Then** **cả hai** kênh nhận đáp án **cùng một mốc**, cùng nội dung và cùng điều kiện; **không** có lệnh cấm riêng nào áp cho lớp phủ; chiều truyền một chiều **không** cản việc đẩy; và điểm cùng trạng thái trận **không đổi** — công bố đáp án không sinh event điểm.

**AC-014** — *US*: US-002, US-003 · *FR*: FR-018 · *GR*: `GR-037` §Cấm · `NFR-21`
**Given** một trận đầy đủ năm vòng, cờ hiện đáp án **BẬT** suốt trận, và một công cụ bắt gói tin đặt trên kênh lớp phủ,
**When** trận chạy hết từ đầu tới lúc chốt,
**Then** **0** byte mang đáp án chuẩn xuất hiện trên kênh lớp phủ ở **bất kỳ** thời điểm nào trước mốc câu khép tương ứng của từng câu; điều này đúng **kể cả** khi cờ đang bật; và bộ kiểm không-rò-đáp-án pass mà **không** cần đọc trạng thái giao diện.

#### Nhóm C — Phạm vi hiển thị đáp án trên hai kênh public (US-003)

**AC-015** — *US*: US-003 · *FR*: FR-019 · *GR*: `GR-037` **C4** · `INV-017`
**Given** một câu Khởi động lượt riêng đang đếm giờ, chưa ai được chấm nên **câu chưa khép**, cờ hiện đáp án **BẬT**, màn khán giả và lớp phủ đang xem,
**When** hai kênh public nhận đợt cập nhật kế tiếp,
**Then** **không** kênh nào nhận đáp án chuẩn; server **không trả**; điểm và trạng thái câu vẫn tới bình thường; và trạng thái trận **không đổi**.

**AC-016** — *US*: US-003 · *FR*: FR-019, FR-020 · *GR*: `GR-037` **C3**
**Given** một trận có cờ hiện đáp án **TẮT**, một câu Khởi động vừa được admin chấm nên **câu đã khép**,
**When** hai kênh public nhận đợt cập nhật kế tiếp,
**Then** **không** kênh nào nhận đáp án chuẩn — cờ tắt thì mốc câu khép không mở đường nào; server **không trả**; và trong cùng trận đó, màn MC **vẫn** thấy đáp án vì phiên đó giữ `PERM-045`.

**AC-017** — *US*: US-003 · *FR*: FR-020 · *GR*: `GR-037` **C5**, §bảng Mốc câu khép
**Given** một câu **Khởi động lượt riêng** vừa được admin chấm **Đúng** nên hai mốc *đã chấm* và *câu khép* **trùng nhau**, cờ hiện đáp án **BẬT**,
**When** admin bấm chấm,
**Then** server đẩy đáp án tới thí sinh, màn khán giả và lớp phủ **tại đúng mốc đó**; và cùng kết cục xảy ra ở **VCNV hàng ngang**, **VCNV ô trung tâm** *(chấm xong)* và **Tăng tốc** *(chấm xong **cả bảng** — một câu là một event điểm cho toàn bộ bảng, nên đáp án tới hai kênh public **một lần**, không phải bốn lần)*.

**AC-018** — *US*: US-003 · *FR*: FR-021 · *GR*: `GR-037` §Cấm · `NFR-21b`
**Given** một câu bất kỳ chưa khép, cờ hiện đáp án **BẬT**,
**When** kiểm nội dung thực tế đã tới bộ nhớ của client màn khán giả và client lớp phủ bằng công cụ bắt gói tin,
**Then** đáp án **chưa** có mặt ở đó dưới bất kỳ dạng nào; hệ thống **không** đẩy sớm rồi ẩn bằng một cờ hiển thị phía client; server chỉ đẩy **tại đúng mốc**; và ở mốc đó thì nó mới xuất hiện.

**AC-019** — *US*: US-003 · *FR*: FR-022 · *GR*: `GR-037` **C6**, §bảng Mốc câu khép, §Biên · `GR-020` §Cấm
**Given** một câu **Về đích** 30đ, người thi chính vừa bị admin chấm **Sai** — chính cú bấm đó mở **cửa sổ cướp quyền 5 giây** — cờ hiện đáp án **BẬT**,
**When** cửa sổ cướp quyền đang mở và một ghế khác bấm chuông, trả lời, rồi được admin chấm,
**Then** trong **toàn bộ** khoảng từ cú bấm chấm Sai tới khi người cướp được chấm, đáp án **không** xuất hiện trên màn khán giả và lớp phủ; đáp án chỉ tới hai kênh **sau** khi người cướp được chấm; ở nhánh **hết 5 giây không ai bấm**, câu khép tại mốc đóng cửa sổ và đáp án tới tại đúng mốc đó; và ở nhánh **người thi chính được chấm Đúng**, cửa sổ cướp **không mở** và câu khép ngay.

**AC-020** — *US*: US-003 · *FR*: FR-023 · *GR*: `GR-037` **C7**
**Given** một câu **Khởi động lượt chung** đã hiển thị, cửa sổ chuông `3` giây đã mở và **không ghế nào bấm**, cờ hiện đáp án **BẬT**,
**When** cửa sổ chuông đóng và câu bị **bỏ qua**,
**Then** đáp án **vẫn** được đẩy tới màn khán giả và lớp phủ — câu đã tiêu, đã khép; **không** event điểm nào được sinh cho ai; và câu **không** quay lại kho.

**AC-021** — *US*: US-003 · *FR*: FR-024 · *GR*: `GR-037` **C8**
**Given** một câu bất kỳ mà admin vừa chấm **Huỷ kết quả** cho người đang giữ quyền, cờ hiện đáp án **BẬT**, và **không** có tín hiệu nào đang chờ kích hoạt tay,
**When** phán quyết được ghi nhận,
**Then** hệ thống **không tự** đẩy đáp án tới hai kênh public; đường công bố **bằng tay** của người giữ `PERM-044` vẫn dùng được và là một đường **khác**; quy tắc này phát biểu theo **loại phán quyết**, nên nó áp cho **mọi** vị trí trong chuỗi, gồm cả khi *Huỷ kết quả* là phán quyết khép câu cho người cướp quyền Về đích; và điểm **không đổi**.

**AC-022** — *US*: US-003 · *FR*: FR-025 · *GR*: `GR-037` **C8b**
**Given** một câu Khởi động lượt chung mà người giữ quyền vừa bị chấm **Huỷ kết quả**, hàng đợi còn một tín hiệu **trơ chưa từng được xử lý**, và admin **chưa** bấm kích hoạt tay, cờ hiện đáp án **BẬT**,
**When** hai kênh public nhận đợt cập nhật kế tiếp,
**Then** **không** kênh nào nhận đáp án — mốc câu khép đã **lùi** tới sau khi người được kích hoạt được chấm; đáp án chỉ tới **sau** phán quyết cho ghế đó; và lịch sử tín hiệu **không đổi** trong lúc chờ.

**AC-023** — *US*: US-003 · *FR*: FR-026 · *GR*: `GR-037` **C9**, `GR-012` C2
**Given** một vòng Vượt chướng ngại vật, cờ hiện đáp án **BẬT**, và một câu hàng ngang vừa được chấm nên **câu đó** đã khép,
**When** server đẩy cập nhật,
**Then** đáp án của **câu hàng ngang** đi theo `GR-037` và tới hai kênh public; nhưng đáp án **Chướng ngại vật** **không** đi theo cơ chế đó — nó chỉ lộ khi có người giải đúng, hoặc khi admin bấm công bố theo `GR-012`; cờ hiện đáp án **không** mở đường nào cho đáp án Chướng ngại vật; và khi nó **có** lộ theo `GR-012` thì nó tới **cả màn khán giả lẫn lớp phủ**, **cùng lúc và cùng điều kiện** — việc đứng ngoài `GR-037` chỉ đổi **thời điểm**, **không** đổi **người nhận**.

**AC-024** — *US*: US-003 · *FR*: FR-027 · *GR*: `GR-037` §Bấm trùng
**Given** một câu đã khép và đáp án đã được đẩy tới hai kênh public,
**When** trạng thái trận tiếp tục thay đổi — chuyển câu, mở vòng khác, hoàn nguyên điểm,
**Then** engine **không tự thu lại** đáp án đã công bố; công bố là **một chiều ở phía engine**; và quyền đóng hiển thị **bằng tay** của người giữ `PERM-044` giữ nguyên như một đường riêng — engine đi **một chiều**, admin đi được **cả hai**.

**AC-024a** — *US*: US-003 · *FR*: FR-027a · *GR*: `GR-037` §Không đổi gì, §Bấm trùng · `PERM-044`, `QĐ-048`
**Given** một câu Khởi động lượt riêng đang mở, cờ hiện đáp án **BẬT**, admin đã dùng `PERM-044` **mở** đáp án sớm cho mọi vai đang xem rồi **đóng** lại — cú đóng còn hiệu lực, màn khán giả và lớp phủ hiện **không** có đáp án,
**When** admin bấm chấm và **câu khép**,
**Then** engine **không** đẩy đáp án tới màn khán giả và lớp phủ — cú đóng tay **thắng**; hai kênh public vẫn kín cho tới khi người giữ `PERM-044` **tự mở lại**; màn MC **vẫn** thấy đáp án suốt thời gian đó vì nó đi đường `PERM-045`, một đường khác; điểm và trạng thái trận **không đổi**; và khi admin chuyển sang **câu mới**, cú đóng **không** dính theo — hành vi hiển thị của câu mới quay về mặc định do cờ cấp trận quyết định.

**AC-025** — *US*: US-003, US-004 · *FR*: FR-028 · *GR*: `GR-037` §Thứ tự đánh giá bước (2) · `INV-017`
**Given** một đơn vị đã tạo một **vai tuỳ biến** mang `PERM-045`, và một tài khoản mang vai đó đang mở màn của mình trong một trận có câu **chưa khép**,
**When** phiên đó yêu cầu đáp án,
**Then** server trả đáp án và ghi audit — cửa kiểm hỏi **permission**, **không** hỏi tên vai; song song, hai kênh public **không** nhận gì vì chúng không mang tài khoản nên không giữ permission nào; và việc hai kênh public không nhận là **hệ quả của cửa kiểm**, không phải một danh sách chặn theo tên kênh.

**AC-026** — *US*: US-003 · *FR*: FR-029 · *GR*: `GR-037` §Điều kiện
**Given** một contest chứa **hai** trận — một trận **chính thức** và một trận **tổng duyệt** — cả hai dùng chung phòng; trận tổng duyệt đã bắt đầu với cờ hiện đáp án **BẬT**,
**When** admin đặt cờ của trận chính thức thành **TẮT** rồi bắt đầu trận đó,
**Then** hai trận chạy với hai giá trị cờ khác nhau; giá trị được **chụp vào trận** lúc bắt đầu; sửa cấu hình contest sau đó **không đụng** trận đã chạy; và mặc định của cờ là **BẬT** cho cả hai loại trận.

**AC-027** — *US*: US-003 · *FR*: FR-030 · *GR*: `GR-037` §Điều kiện
**Given** một phiên **không** giữ `PERM-045` — ví dụ một phiên thí sinh — trong một trận có câu chưa khép,
**When** phiên đó gửi thẳng tới server một yêu cầu đọc đáp án,
**Then** server **im lặng** từ chối: không trả đáp án và không trả một thông điệp tiết lộ sự tồn tại của nó; **không** event nào được sinh; và trạng thái trận **không đổi**.

**AC-028** — *US*: US-003, US-001 · *FR*: FR-031 · *GR*: `GR-037` §bảng ba loại thông tin, `GR-008` §Điều kiện
**Given** một câu VCNV hàng ngang đang mở, bốn ghế đã gửi bài, câu **chưa** được chấm, cờ hiện đáp án **BẬT**, màn khán giả đang xem,
**When** kênh khán giả nhận cập nhật,
**Then** **điểm** của cả bốn ghế có mặt — công khai, luôn luôn; **bài làm** của các ghế **chưa** lộ vì câu còn mở, và chỉ lộ khi admin bấm hiển thị; **đáp án chuẩn** **không** có mặt vì câu chưa khép; ba chế độ này **độc lập** và việc điểm công khai **không** nới lỏng hai cái còn lại.

**AC-029** — *US*: US-003, US-004 · *FR*: FR-032 · *GR*: `GR-037` §Không đổi gì
**Given** một trận đang chạy, màn MC mở, đáp án hiện trên màn đó,
**When** MC đọc đáp án và màn nhận lại dữ liệu nhiều lần,
**Then** **không** event điểm nào được sinh; trạng thái trận, túi permission của các vai, và cửa sổ cướp quyền cùng mọi con số của luật đều **không đổi**; chỉ nhật ký audit có thêm dòng.

#### Nhóm D — Màn MC (US-004)

**AC-030** — *US*: US-004 · *FR*: FR-033 · *GR*: `GR-037` C2
**Given** một tài khoản mang vai **MC**, được gán ở phạm vi contest của trận đang chạy, một câu Về đích 30đ vừa được admin bấm hiển thị,
**When** MC mở màn `/mc`,
**Then** màn hiện **câu hỏi và đáp án**, chữ đủ to để đọc ở khoảng cách sân khấu; và trạng thái trận **không đổi** — mở màn MC là một phép đọc.

**AC-031** — *US*: US-004 · *FR*: FR-034 · *GR*: `GR-037` **C2**
**Given** cùng trạng thái AC-030, câu **chưa** được chấm nên **chưa khép**, và trận có cờ hiện đáp án **TẮT**,
**When** màn MC dựng nội dung,
**Then** đáp án **vẫn** hiện — phiên MC giữ `PERM-045` nên nó xem được **mọi lúc**, không phụ thuộc cờ và không phụ thuộc mốc; trong khi ở cùng thời điểm đó màn khán giả, lớp phủ và máy thí sinh **không** có đáp án.

**AC-032** — *US*: US-004 · *FR*: FR-035 · *GR*: `GR-037` C2, §Thứ tự đánh giá bước (6) · `NFR-14`
**Given** cùng trạng thái AC-031, nhật ký audit đang có `N` dòng,
**When** đáp án tới màn MC **50** lần liên tiếp qua các câu khác nhau,
**Then** nhật ký có đúng `N + 50` dòng, mỗi dòng ghi người xem, đối tượng và mốc thời gian; và trạng thái trận **không đổi** sau cả 50 lần.

**AC-033** — *US*: US-004 · *FR*: FR-036
**Given** một phiên MC đang mở màn `/mc` trong một trận đang chạy giữa một vòng,
**When** rà toàn bộ bề mặt của màn này,
**Then** **không** có nút điều khiển trận nào — không chấm, không start timer, không mở đáp án, không mở ô chữ, không duyệt tín hiệu, không mở lớp công bố, không mở banner.

**AC-034** — *US*: US-004 · *FR*: FR-037, FR-039 · *GR*: `STATE-039` §Vào, §Cấm, §Ra · `T-093`, `T-094`, `T-095`
**Given** một trận đang chạy giữa một câu đang đếm giờ, contest **có** MC được gán, phiên admin đang giữ quyền vừa **mất kết nối**, và một phiên admin khác của cùng contest bấm **giành quyền**,
**When** cú giành được ghi nhận,
**Then** một prompt duyệt / từ chối hiện trên màn `/mc`; prompt **không có nút đóng hay bỏ qua**; quyền **chưa** đổi cho tới khi MC quyết; MC bấm **Duyệt** ⇒ quyền chuyển sang người giành, MC bấm **Từ chối** ⇒ quyền ở nguyên chỗ cũ và cú giành ở lại lịch sử vĩnh viễn ở trạng thái bị từ chối; cả hai nhánh vào `AuditLog`; **không** event điểm nào được sinh; và ở **biến thể** phiên đang giữ **kết nối lại** trước khi MC quyết thì prompt đóng, quyền ở nguyên chỗ cũ, cú giành **mất đối tượng** — không xoá, không cảnh báo.

**AC-035** — *US*: US-004 · *FR*: FR-037 · *GR*: `STATE-039` §Cấm
**Given** một phiên MC đã xác thực trong một trận có một tín hiệu thí sinh **đang chờ duyệt** ở hàng đợi VCNV,
**When** phiên MC gửi thẳng tới server lần lượt: một sự kiện duyệt tín hiệu của thí sinh, một sự kiện chấm, một sự kiện start timer, một sự kiện mở lớp công bố, một sự kiện điều chỉnh điểm,
**Then** server **từ chối tất cả**; tín hiệu vẫn ở trạng thái chờ duyệt và **chỉ** admin duyệt được nó; điểm, đồng hồ, hàng đợi và trạng thái trận đều **không đổi**; và mỗi lần từ chối để lại một dòng nhật ký.

**AC-036** — *US*: US-004 · *FR*: FR-038 · *GR*: `GR-037` C2 · `permissions.md` §chú thích
**Given** một câu **chưa khép**, phiên MC đang thấy đáp án trên màn của mình,
**When** MC tìm cách công bố đáp án đó cho thí sinh, khán giả hoặc lớp phủ,
**Then** **không** có bề mặt nào để làm việc đó — `PERM-045` là hành vi **đọc trên màn của chính mình**, còn công bố là `PERM-044` mà vai MC **không** giữ; và đáp án **không** rời server tới kênh nào khác.

**AC-037** — *US*: US-004 · *FR*: FR-040 · *GR*: `STATE-039` §Cho phép · `INV-016`, `INV-021`
**Given** cùng trạng thái AC-034 — prompt duyệt đang hiện trên màn MC, một câu Về đích đang đếm giờ với hạn chót đã đặt,
**When** MC chưa bấm gì trong 10 giây,
**Then** đồng hồ của câu **vẫn chạy** theo server time và **không** bị đặt lại; vòng **vẫn tiếp**; thí sinh vẫn thao tác được bình thường; trận **không** tự tạm dừng; và hai kênh public **không** nhận thông báo nào về việc đang có một cú giành chờ duyệt.

**AC-038** — *US*: US-004, US-007 · *FR*: FR-041, FR-070 · *GR*: `GR-016` **C1**, **C2**, **C3**, **C4** · `QĐ-076`
**Given** một vòng **Về đích** vừa mở, điểm bốn ghế là `110 · 90 · 80 · 70`, hệ thống khuyến nghị ghế 1 đi trước,
**When** màn admin, màn MC, màn khán giả và lớp phủ cùng dựng nội dung,
**Then** khuyến nghị lượt hiện trên màn admin **và** màn MC; nó **không** xuất hiện trên màn khán giả và lớp phủ; và cùng kết cục ở ca **hoà điểm** *(vị trí nhỏ hơn đi trước)*, ca **bảng tính lại sau mỗi lượt**, và ca **cả bốn cùng điểm** *(số vị trí nhỏ nhất đi trước)*.

#### Nhóm E — Lớp hiển thị công bố kết quả (US-005)

**AC-039** — *US*: US-005 · *FR*: FR-042, FR-043 · *GR*: `STATE-033` §Cho phép · `INV-021`
**Given** một vòng Vượt chướng ngại vật đang chạy, câu hàng ngang thứ hai đang mở và đang đếm giờ, một tín hiệu đang chờ duyệt trong hàng đợi, điểm bốn ghế là `40 · 30 · 20 · −10`,
**When** admin bấm **mở lớp công bố**,
**Then** lớp công bố hiện trên **mọi** vai — máy thí sinh, màn khán giả, lớp phủ dựng stream và màn MC; trạng thái bên dưới **không đổi**: cùng câu, cùng cờ ghế, cùng hàng đợi, cùng giá trị đồng hồ theo server time; và **không** event điểm nào được sinh.

**AC-040** — *US*: US-005 · *FR*: FR-043 · *GR*: `QĐ-049`
**Given** cùng trạng thái AC-039 với lớp công bố đang mở,
**When** so sánh nội dung lớp công bố trên bốn loại màn,
**Then** cả bốn hiện **cùng** bảng — tên, điểm, thứ hạng của mọi ghế; việc áp cho máy thí sinh **không** lộ thêm gì vì điểm vốn đã công khai với mọi vai; và lớp này **không** chứa đáp án.

**AC-041** — *US*: US-005 · *FR*: FR-044 · *GR*: `EVENT-032` §Hợp lệ ở, `STATE-033` §Vào
**Given** một trận vừa kết thúc vòng Khởi động và đang ở `LOBBY`,
**When** kiểm bề mặt của admin, rồi thử mở lớp công bố ở `LOBBY`, giữa một vòng đang chạy, ở `TIE_BREAK` và ở `FINISHED`,
**Then** hệ thống **gợi ý** mở ở hai mốc — vừa hết vòng và hết trận; gợi ý **không** là điều kiện: cả bốn trạng thái đều mở được; **không** trạng thái cấp trận nào chặn thao tác này; và điều kiện **duy nhất** gặp phải là ràng buộc loại trừ với banner tạm dừng ở AC-058a, vốn là ràng buộc giữa **hai lớp phủ** chứ không phải một điều kiện về trạng thái cấp trận.

**AC-042** — *US*: US-005 · *FR*: FR-045 · *GR*: `STATE-033` §Ra, `EVENT-033`, `T-088`
**Given** lớp công bố đang mở,
**When** để nguyên trong 10 phút mà không ai bấm gì,
**Then** lớp **vẫn mở** — **không** có bộ đếm tự đóng; nó chỉ đóng khi admin bấm; và bấm đóng khi lớp **chưa** mở là thao tác không hợp lệ.

**AC-043** — *US*: US-005 · *FR*: FR-046, FR-047 · *GR*: `STATE-033` §Cấm, `EVENT-032`, `T-087`
**Given** một trận có điểm `40 · 30 · 20 · −10`, và một client màn khán giả đã **lỡ mất** một đợt cập nhật điểm nên bản sao điểm của nó lệch,
**When** admin mở lớp công bố,
**Then** server tính thứ hạng từ nhật ký sự kiện và đẩy xuống **một** sự kiện kèm bảng đã tính; client lệch kia hiện đúng bảng của server, **không** hiện bảng tính từ bản sao của mình; và client **không** có đường tự tính thứ hạng.

**AC-044** — *US*: US-005 · *FR*: FR-048 · *GR*: `GR-025` **C5** · `QĐ-049` ràng buộc 2
**Given** một trận kết thúc với điểm `100 · 100 · 80 · 70` — ghế 1 và ghế 2 bằng điểm và admin **không** phân định nhóm đó,
**When** admin mở lớp công bố,
**Then** ghế 1 và ghế 2 ghi **đồng hạng 1**; ghế 3 ghi hạng **3** — hạng kế **nhảy qua** đúng số người đồng hạng; ghế 4 ghi hạng **4**; và cùng quy tắc áp cho nhóm 3 người *(hai người hạng 1, người kế hạng 3)* và nhóm 4 người *(cả bốn hạng 1)*.

**AC-045** — *US*: US-005 · *FR*: FR-049 · *GR*: `GR-025` C5 · `QĐ-049`
**Given** một trận đang ở `TIE_BREAK` với hai ghế bằng `100` điểm và vòng **chưa** phân định xong,
**When** admin mở lớp công bố,
**Then** hai ghế đó hiện **đồng hạng** — đúng trạng thái thật tại mốc đó; hệ thống **không** ẩn bảng và **không** bịa một thứ tự tạm; và trạng thái vòng `TIE_BREAK` **không đổi**.

**AC-046** — *US*: US-005 · *FR*: FR-050 · *GR*: `GR-028` §Điều kiện, **C5** · `QĐ-049` ràng buộc 3
**Given** một trận có nhật ký gồm `+10`, `−5`, `+3` cho ghế 1 nên điểm hiện tại là `8`,
**When** admin mở lớp công bố,
**Then** ghế 1 hiện **`8`** — kết quả tính từ nhật ký tại thời điểm mở, **không** phải một cột số bị sửa tại chỗ; **cả ba** event vẫn nguyên trong nhật ký; và việc tính này **không** sinh event nào.

**AC-047** — *US*: US-005, US-006 · *FR*: FR-052 · *GR*: `INV-021`, `T-087`
**Given** một vòng Về đích đang chạy: ghế 2 đang thi, một câu đang đếm giờ, ghế 3 đã dùng Ngôi sao hy vọng, một tín hiệu đang trơ trong hàng đợi,
**When** admin mở lớp công bố rồi đóng lại **20** lần liên tiếp,
**Then** sau cả 20 lần: **0** event điểm được sinh; trạng thái trận, vòng, câu, ghế và tín hiệu đều **không đổi**; đồng hồ **không** bị đụng; hàng đợi **không** bị đụng; và quyền thao tác **không** đổi.

**AC-048** — *US*: US-005 · *FR*: FR-053 · *GR*: `STATE-033` §Ra, `T-088`
**Given** lớp công bố đang mở giữa một vòng, điểm ghế 1 là `40` tại thời điểm mở,
**When** admin điều chỉnh điểm ghế 1 thành `50` **trong lúc** lớp đang mở, rồi đóng lớp,
**Then** trạng thái bên dưới lộ lại là trạng thái **hiện tại** — ghế 1 hiện `50`, không phải `40`; vòng tiếp tục **đúng chỗ cũ**; và không phát sinh bài toán đồng bộ lại nào.

**AC-049** — *US*: US-005, US-003 · *FR*: FR-054 · *GR*: `STATE-033` §Cấm
**Given** một câu **chưa khép** đang mở, và admin mở lớp công bố chồng lên,
**When** kiểm nội dung lớp công bố trên bốn loại màn và kiểm gói tin tới hai kênh public,
**Then** lớp công bố **không** chứa đáp án dưới bất kỳ dạng nào; nhịp lộ từng người trên bảng là **animation phía client**, dựng từ bảng server đã đẩy, **không** phải một chuỗi sự kiện của engine; và đáp án vẫn **không** rời server tới hai kênh public.

#### Nhóm F — Banner tạm dừng (US-006)

**AC-050** — *US*: US-006 · *FR*: FR-055 · *GR*: `STATE-040` §Mô tả, §Cho phép, `T-089`
**Given** một trận đang ở `LOBBY` giữa hai vòng nên **không có cửa sổ thời gian nào đang đếm**, bốn máy thí sinh đang kết nối, màn khán giả và lớp phủ đang xem,
**When** admin bấm **mở banner tạm dừng**,
**Then** máy thí sinh bị chặn **toàn bộ** thao tác; màn khán giả và lớp phủ hiện báo hiệu **trận đang tạm dừng** thuần thị giác; **máy admin không bị phủ** và admin làm mọi thứ như thường; và **không** event điểm nào được sinh.

**AC-050a** — *US*: US-006, US-004 · *FR*: FR-055a · *GR*: `STATE-040` §Mô tả, `STATE-039` §Cho phép · `QĐ-093`
**Given** banner tạm dừng **đang bật** ở `LOBBY`, contest **có** MC được gán, màn `/mc` đang mở với một câu đã hiển thị, phiên admin đang giữ quyền vừa **mất kết nối**, và một phiên admin khác bấm **giành quyền**,
**When** prompt duyệt giành quyền hiện trên màn `/mc`,
**Then** màn MC **không bị banner phủ** — câu hỏi và đáp án vẫn đọc được; prompt duyệt **bấm được** ngay trong lúc banner đang bật; MC bấm **Duyệt** ⇒ quyền chuyển sang người giành và banner **vẫn bật** không đổi; máy thí sinh vẫn bị chặn toàn bộ; hai kênh public vẫn hiện báo hiệu không chữ và **không** nhận thông báo nào về cú giành; và **không** event điểm nào được sinh.

**AC-051** — *US*: US-006 · *FR*: FR-056 · *GR*: `STATE-040` §Cấm
**Given** banner tạm dừng đang bật như AC-050,
**When** chụp màn hình màn khán giả và lớp phủ,
**Then** banner **không** chứa ký tự văn bản nào — không lý do, không tên người bấm, không đồng hồ; khán giả tự hiểu từ bối cảnh sân khấu.

**AC-052** — *US*: US-006 · *FR*: FR-057 · *GR*: `STATE-040` §Vào, `EVENT-034` §Không hợp lệ ở, `T-089`
**Given** một câu Tăng tốc đang **đếm giờ** — `STATE-019`, đồng hồ đang chạy,
**When** admin tìm nút mở banner tạm dừng,
**Then** nút **không bật**; nếu lệnh vẫn tới server thì server từ chối và admin thấy **toast invalid state**; thao tác **không ép được**; và đồng hồ, điểm cùng trạng thái câu đều **không đổi**.

**AC-053** — *US*: US-006 · *FR*: FR-058 · *GR*: `STATE-040` §Cấm, §Invalid transitions
**Given** banner tạm dừng **đang bật** ở `LOBBY`, admin vừa mở một vòng mới và bấm hiển thị câu đầu tiên,
**When** admin tìm nút **start timer**,
**Then** nút **không bật**; server từ chối nếu lệnh vẫn tới, kèm toast; admin phải **đóng banner trước**; và vì hai chiều đều bị chặn nên **không tồn tại** thời điểm nào banner và một đồng hồ đang chạy cùng có mặt.

**AC-054** — *US*: US-006 · *FR*: FR-059 · *GR*: `STATE-040` §Cấm, `EVENT-035`
**Given** banner tạm dừng đang bật,
**When** để nguyên trong 10 phút,
**Then** banner **vẫn bật** — **không** có bộ đếm tự đóng; nó chỉ tắt khi admin bấm; và bấm đóng khi banner **chưa** bật là thao tác không hợp lệ.

**AC-055** — *US*: US-006 · *FR*: FR-060 · *GR*: `STATE-040` §Bốn ràng buộc suy ra
**Given** banner tạm dừng đang bật, và một máy thí sinh đã bị sửa để bỏ qua lớp chặn phía client,
**When** máy đó gửi thẳng tới server một sự kiện bấm chuông và một sự kiện gửi đáp án,
**Then** server **từ chối cả hai** — việc chặn được **server cưỡng chế**, không chỉ client; điểm, hàng đợi đang hoạt động và trạng thái câu đều **không đổi**.

**AC-056** — *US*: US-006 · *FR*: FR-061 · *GR*: `INV-006`
**Given** cùng trạng thái AC-055,
**When** kiểm lịch sử tín hiệu sau hai lần gửi bị từ chối,
**Then** **cả hai** tín hiệu có mặt trong lịch sử kèm server timestamp và kết cục **trơ**; **0** tín hiệu bị drop; và lịch sử tín hiệu **không bao giờ bị xoá**.

**AC-057** — *US*: US-006 · *FR*: FR-062 · *GR*: `STATE-040` §Bốn ràng buộc suy ra · `NFR-14`
**Given** nhật ký thao tác đang có `N` dòng,
**When** admin bật rồi tắt banner **5** lần liên tiếp,
**Then** nhật ký có đúng `N + 10` dòng, mỗi dòng ghi người bấm, thao tác và mốc thời gian; *"không ghi lý do"* chỉ là ràng buộc **hiển thị trên banner**, **không** là miễn trừ audit.

**AC-058** — *US*: US-006 · *FR*: FR-063 · *GR*: `QĐ-005`
**Given** một trạng thái mà banner bật được,
**When** admin bấm nút mở banner,
**Then** banner bật **ngay**, **không** qua dialog xác nhận và **không** đòi nhập lý do — thao tác đảo ngược được bằng một cú bấm.

**AC-058a** — *US*: US-006, US-005 · *FR*: FR-058a · *GR*: `STATE-033` §Vào, `STATE-040` §Vào · `INV-014`, `INV-021`
**Given** một trận đang ở `LOBBY` vừa kết thúc một vòng nên **không có cửa sổ thời gian nào đang đếm**, điểm bốn ghế là `40 · 30 · 20 · −10`, màn khán giả và lớp phủ đang xem, và admin đã mở **lớp công bố kết quả**,
**When** admin tìm nút **mở banner tạm dừng**,
**Then** nút **không bật**; lệnh lọt tới server thì bị từ chối kèm phản hồi hạng **invalid state**, và **không ép được**; muốn bật banner thì admin phải **đóng lớp công bố trước**; ở **chiều ngược lại** — banner đang bật, admin tìm nút mở lớp công bố — kết cục **y hệt**; nên tổ hợp *(banner đang bật × lớp công bố đang bật)* **không dựng được** và câu hỏi lớp nào trên cùng **không có chủ ngữ**; ràng buộc này **không** là chỗ chặn cứng thứ tư của `INV-014`; các tổ hợp lớp phủ **khác** vẫn bật cùng lúc được bình thường; và điểm cùng trạng thái trận **không đổi** ở mọi nhánh.

**AC-059** — *US*: US-006 · *FR*: FR-064 · *GR*: `STATE-040` §Ra · `QĐ-115`, `INV-021`
**Given** banner tạm dừng đang bật ở `LOBBY`, điểm ghế 1 là `40` tại thời điểm bật, màn khán giả và lớp phủ đang hiện báo hiệu tạm dừng,
**When** admin điều chỉnh điểm ghế 1 thành `50` **trong lúc** banner bật, rồi tắt banner,
**Then** dữ liệu bên dưới **vẫn chảy** suốt thời gian banner bật — server vẫn đẩy, client vẫn nhận; tắt banner cho ra **trạng thái hiện tại**, ghế 1 hiện `50`; **không** phát sinh bước đồng bộ lại nào; và trạng thái trận, vòng, câu, ghế đều **không đổi** bởi chính việc bật hay tắt banner.

#### Nhóm G — Khán giả không được báo về can thiệp của admin (US-007)

**AC-060** — *US*: US-007 · *FR*: FR-065 · *GR*: `GR-030` §Ở phía viewer · `QĐ-076`
**Given** một trận đang chạy, màn khán giả và lớp phủ đang xem, một công cụ bắt gói tin đặt trên cả hai kênh, một ghế đang mất kết nối trong cửa sổ giữ ghế,
**When** admin lần lượt **sửa danh sách đề** và **gỡ một lệnh cấm**,
**Then** hai kênh public **không** nhận bản tin nào về hai thao tác đó; banner *"đang kết nối lại"* của ghế mất kết nối chỉ hiện trên **máy ghế đó**, **không** trên hai kênh public; và bảng điểm trên hai kênh **không đổi** vì hai thao tác này không sinh event điểm.

**AC-061** — *US*: US-007 · *FR*: FR-066 · *GR*: `GR-030` §Ở phía viewer
**Given** điểm bốn ghế trên màn khán giả là `40 · 30 · 20 · 10`,
**When** một thao tác của admin làm điểm ghế 1 tụt xuống `10`,
**Then** ô điểm của ghế 1 đổi từ `40` sang `10` **đột ngột** — **không** hiệu ứng đếm ngược, **không** hiệu ứng chuyển tiếp, **không** dòng chữ giải thích; và cùng quy tắc áp cho bàn cờ Vượt chướng ngại vật.

**AC-062** — *US*: US-007 · *FR*: FR-067 · *GR*: `GR-030` **C1**, `GR-028` **C4**
**Given** một trận trong đó vòng Khởi động đã cộng `30` điểm cho ghế 1, điểm hiện tại của ghế 1 là `40`, màn khán giả đang xem,
**When** admin **bỏ** vòng Khởi động kèm lý do,
**Then** màn khán giả hiện ghế 1 còn **`10`**; **không** bản tin nào khác tới kênh này — không tên vòng bị bỏ, không lý do, không tên người bấm; nhãn *"đã bỏ"* **không** xuất hiện trên kênh public; và các event cũ trong nhật ký **không** bị xoá.

**AC-063** — *US*: US-007 · *FR*: FR-068 · *GR*: `GR-030` **C2**
**Given** một vòng Vượt chướng ngại vật đã tiêu 4 câu và đã cộng điểm, màn khán giả đang xem,
**When** admin **chạy lại** vòng đó kèm lý do,
**Then** màn khán giả thấy điểm đã hoàn nguyên và bàn cờ mở lại từ đầu; **không** thông báo nào về việc vòng vừa được chạy lại; và câu đã hiển thị **không** quay lại kho — nhưng việc đó cũng không hiện gì trên kênh public.

**AC-064** — *US*: US-007 · *FR*: FR-069 · *GR*: `GR-030` **C3**
**Given** một vòng Về đích hỏng giữa chừng, chưa hỏi đủ câu, điểm bốn ghế là `60 · 40 · 30 · 20`, màn khán giả đang xem,
**When** admin **kết thúc sớm** vòng đó kèm lý do,
**Then** điểm trên màn khán giả **giữ nguyên** `60 · 40 · 30 · 20` — không có event đảo ngược nào; vòng khép về `LOBBY`; **không** thông báo nào tới kênh public; và nhãn *"kết thúc sớm"* chỉ tồn tại trong biên bản, không trên kênh public.

**AC-065** — *US*: US-007 · *FR*: FR-070 · *GR*: `GR-016` · `INV-020` · `QĐ-076`
**Given** một vòng Về đích với ba lượt còn chưa thi và một khuyến nghị lượt đang hiện trên màn admin,
**When** kiểm gói tin tới màn khán giả và lớp phủ,
**Then** **0** byte nào mang khuyến nghị lượt; thứ tự sắp tới **không** lộ ra hai kênh public; và điều này giữ nguyên qua cả bốn lượt của vòng.

**AC-066** — *US*: US-007 · *FR*: FR-071 · *GR*: `GR-030` §Điều kiện
**Given** một trận có một vòng bị **bỏ** rồi **chạy lại**, nên biên bản sẽ có hai khối với hai nhãn khác nhau,
**When** kiểm nội dung tới màn khán giả và lớp phủ trong suốt trận,
**Then** **không** nhãn *"đã bỏ"*, *"đã chạy lại"*, *"kết thúc sớm"* hay *"hoàn thành"* nào xuất hiện trên hai kênh này; chúng chỉ tồn tại trong biên bản trận sau trận.

#### Nhóm H — Chủ đề và các khe âm thanh (US-008)

**AC-067** — *US*: US-008 · *FR*: FR-072
**Given** một contest đã dựng xong với chủ đề mặc định, và một trận **chưa** bắt đầu,
**When** admin đổi chủ đề hiển thị của contest rồi bắt đầu trận,
**Then** màn khán giả và lớp phủ dựng theo **bảng màu, logo và ảnh nền** mới; chủ đề **không** mang tài sản động nào và **không** mang phông chữ; giá trị chủ đề thuộc cấu hình **cấp contest**; và trạng thái trận **không đổi** bởi việc đổi chủ đề.

**AC-067a** — *US*: US-008, US-004 · *FR*: FR-072b · `NFR-37`
**Given** một contest mang một chủ đề đặt màu tương phản rất thấp, một trận đang chạy, và cả năm loại màn cùng mở — khán giả · lớp phủ dựng stream · thí sinh · MC · admin,
**When** rà từng màn,
**Then** đúng **hai** màn public đổi theo chủ đề; **màn thí sinh, màn MC và màn admin giữ nguyên bảng màu hệ thống** và không màn nào trong ba màn đó bị giảm khả năng đọc; và trạng thái trận **không đổi** bởi việc đổi chủ đề.

**AC-068** — *US*: US-008 · *FR*: FR-073
**Given** một contest đang ở màn cấu hình, khe *bỏ vòng* chưa gán file,
**When** admin tải lên một file âm thanh cho khe đó,
**Then** khe *bỏ vòng* mang file đó; việc gán là cấu hình **cấp contest**, không phải cấp trận; và trạng thái của các khe khác **không đổi**.

**AC-069** — *US*: US-008 · *FR*: FR-074 · `QĐ-079`
**Given** danh sách khe âm thanh của một contest đang mở,
**When** rà danh sách,
**Then** nó chứa khe cho **cả sự kiện điều khiển**: hoàn nguyên · bỏ vòng · chạy lại vòng · kết thúc sớm · ép qua cảnh báo · mở màn công bố; sáu khe này có mặt **kể cả khi** chưa gán file nào; và mỗi khe mang đúng **một** nhãn nhóm — *hướng khán giả* hoặc *chỉ-admin*. *(Danh sách đầy đủ của nhóm* mốc thi đấu *ở `FR-074d`; AC-069d kiểm nó.)*

**AC-069a** — *US*: US-008, US-007 · *FR*: FR-074a, FR-065a · *GR*: `GR-030` §Ở phía viewer · `QĐ-076`, `QĐ-079`
**Given** một contest đã gán file cho **cả sáu** khe sự kiện điều khiển, nguồn phát đang ở giá trị mặc định **lớp phủ dựng stream**, một trận đang chạy, và một công cụ bắt cả gói tin lẫn luồng âm thanh đặt trên màn khán giả và lớp phủ,
**When** admin lần lượt **bỏ** một vòng, **chạy lại** một vòng, **kết thúc sớm** một vòng, **ép qua** một cảnh báo lệch luật, và thực hiện một cú **hoàn nguyên**,
**Then** **0** trong năm thao tác đó phát ra tiếng trên màn khán giả hoặc lớp phủ — năm khe này thuộc nhóm **chỉ-admin** và MUST NOT đi qua cấu hình nguồn phát; cả năm **có** phát trên máy admin; riêng khe **mở màn công bố** thì **có** phát tới khán giả vì nó thuộc nhóm hướng khán giả; điểm trên hai kênh public vẫn đổi **đột ngột, không hiệu ứng, không giải thích**; và trạng thái trận **không đổi** bởi việc phát hay không phát tiếng.

**AC-069b** — *US*: US-008 · *FR*: FR-074b · `QĐ-102`
**Given** một contest mới dựng, chưa ai đụng vào cấu hình nguồn phát, một lớp phủ dựng stream và một màn khán giả đang kết nối,
**When** một mốc thi đấu thuộc nhóm hướng khán giả xảy ra,
**Then** nguồn phát đang là **lớp phủ dựng stream** — giá trị mặc định; tiếng phát ra từ **lớp phủ**; **màn khán giả im lặng** và nó MUST NOT là nguồn phát ở bất kỳ cấu hình nào; sau đó admin đổi nguồn sang **máy admin** ⇒ tiếng chuyển sang máy admin và lớp phủ im lặng; admin **tắt** nguồn phát ⇒ **không** máy nào phát nhóm hướng khán giả, còn nhóm chỉ-admin **vẫn** phát bình thường trên máy admin.

**AC-069c** — *US*: US-008 · *FR*: FR-074c · `QĐ-001`, `QĐ-079`
**Given** một trận chỉ chạy trong hội trường, **không** có lớp phủ dựng stream nào kết nối, nguồn phát vẫn ở giá trị mặc định **lớp phủ dựng stream**, các khe đều đã gán file,
**When** một mốc thi đấu thuộc nhóm hướng khán giả xảy ra,
**Then** **không** có tiếng nào phát tới khán giả; hệ thống **không tự** đổi nguồn phát sang máy admin; **không** lỗi và **không** cảnh báo nào phát sinh; nhóm chỉ-admin **vẫn** phát trên máy admin; và tình trạng này chỉ chấm dứt khi **admin tự đổi** cấu hình — mọi luật và mọi con số của trận **không đổi** suốt thời gian đó.

**AC-069d** — *US*: US-008 · *FR*: FR-074d, FR-074e · `QĐ-079`
**Given** một contest mới dựng, chưa gán file cho khe nào, danh sách khe âm thanh đang mở,
**When** rà toàn bộ danh sách,
**Then** nhóm **mốc thi đấu** có đúng **52** khe chia theo năm vòng — Khởi động `11` · Vượt chướng ngại vật `14` · Tăng tốc `8` · Về đích `15` · Câu hỏi phụ `4`; mỗi vòng có đúng **một** khe *đếm giờ* và **0** khe mang một con số giây trong tên; **0** khe của `Ô mạo hiểm` và **0** khe của cấu trúc *Bộ 8/12/16 câu hỏi* có mặt; **0** khe truyền hình ngoài trận *(nhạc hiệu mở đầu, giới thiệu cuộc thi, hình hiệu phần thi, trao giải…)* có mặt; toàn bộ `52` khe mang nhãn nhóm **hướng khán giả**; cộng `6` khe điều khiển ra tổng **58** khe; và danh sách **không đổi** khi đổi `timeSeconds` của bất kỳ câu hỏi nào.

**AC-070** — *US*: US-008 · *FR*: FR-075
**Given** một contest có khe *bỏ vòng* **đã gán** file và khe *chạy lại vòng* **để trống**, một trận đang chạy,
**When** admin lần lượt **bỏ** một vòng rồi **chạy lại** một vòng,
**Then** thao tác thứ nhất phát đúng file đã gán; thao tác thứ hai **im lặng**, **không** báo lỗi và **không** hiện cảnh báo; và cả hai thao tác vẫn thực hiện đầy đủ hệ quả về điểm.

**AC-071** — *US*: US-008 · *FR*: FR-076 · `RISK-005`
**Given** một bản cài mới, một contest mới dựng, và admin **chưa** tải lên file âm thanh nào,
**When** chạy trọn một trận,
**Then** trận chạy **hoàn toàn im lặng**; hệ thống **không** phát bất kỳ âm thanh mặc định nào; **không** lỗi và **không** cảnh báo nào phát sinh; và mọi luật cùng mọi con số của trận **không đổi** vì thiếu âm thanh.

**AC-072** — *US*: US-008 · *FR*: FR-077, FR-085
**Given** một contest có đầy đủ khe âm thanh đã gán, và một thay đổi luật được áp — ví dụ đổi thời gian một vòng trong RuleConfig,
**When** chạy lại trận với luật mới,
**Then** ánh xạ sự kiện → âm thanh **không đổi** và không cần sửa; engine chỉ phát **tín hiệu ngữ nghĩa**; ánh xạ tín hiệu → âm thanh và tín hiệu → animation đều là **module phía client, độc lập với engine và rule**; và sửa một trong hai chiều **không** đụng chiều kia.

**AC-073** — *US*: US-008 · *FR*: FR-078
**Given** một contest có 12 khe đã gán file, một client vừa vào phòng,
**When** client hoàn tất bước vào phòng,
**Then** toàn bộ file âm thanh của contest đã được tải trước; không có lần tải nào xảy ra tại **thời điểm phát**; và trạng thái trận **không đổi** bởi việc tải trước.

**AC-074** — *US*: US-008 · *FR*: FR-079
**Given** ngưỡng kích thước file âm thanh được đặt bằng cấu hình môi trường, và admin tải lên một file **vượt** ngưỡng,
**When** hệ thống từ chối file,
**Then** thông điệp từ chối nêu ngưỡng **đọc từ cấu hình**; **không** con số nào bị hard-code ở code hay ở thông điệp; và đổi ngưỡng bằng cấu hình rồi tải lại thì file được nhận mà **không** phải sửa code.

**AC-075** — *US*: US-008 · *FR*: FR-080
**Given** một trận **đã bắt đầu** với chủ đề A và một bộ khe âm thanh A,
**When** admin đổi chủ đề của contest sang B và thay file của một khe,
**Then** trận **đang chạy** vẫn dùng chủ đề A và bộ khe A — cấu hình đã đóng băng tại cú bấm bắt đầu trận; trận **mới tiếp theo** trong cùng contest dùng chủ đề B và bộ khe mới; và điểm cùng trạng thái của trận đang chạy **không đổi**.

#### Nhóm I — Ràng buộc xuyên suốt

**AC-076** — *US*: mọi US · *FR*: FR-081 · `NFR-36`
**Given** màn khán giả đang tải ảnh chụp trạng thái lúc vào phòng, và màn MC đang chờ một đợt cập nhật,
**When** thao tác bất đồng bộ đang chạy,
**Then** mỗi màn hiển thị trạng thái đang xử lý rõ ràng — spinner, progress hoặc skeleton; **không** màn nào im lặng trong lúc chờ.

**AC-077** — *US*: US-001, US-004 · *FR*: FR-082 · `NFR-37`
**Given** màn khán giả và màn MC vừa tải xong trên khung nhìn tham chiếu của dự án,
**When** kiểm vị trí các phần tử quan trọng,
**Then** **0** phần tử quan trọng nằm dưới mép dưới; nội dung chính nằm gọn trong một khung nhìn; và bố cục **không** nhồi nhét — vẫn giữ khoảng thở.

**AC-078** — *US*: mọi US · *FR*: FR-083 · `NFR-38`
**Given** ba màn của epic này đang mở,
**When** rà toàn bộ chuỗi hiển thị và mọi mốc thời gian,
**Then** mọi chuỗi đều là tiếng Việt và đến từ file hằng số, **không** hard-code trong markup; mọi mốc thời gian hiển thị theo **UTC+7**; và **không** có thiết lập múi giờ theo người dùng.

**AC-079** — *US*: US-001 · *FR*: FR-084 · `CLAUDE.md` §UX
**Given** màn khán giả đang mở giữa một vòng,
**When** kiểm thứ tự đọc của bố cục,
**Then** **sân khấu** và **bảng điểm** nằm ở vị trí được đọc trước; thông tin giá trị thấp **không** chiếm chỗ đẹp; và bố cục ưu tiên đúng thông tin quan trọng với vai đang xem.

**AC-079a** — *US*: US-005, US-001, US-002 · *FR*: FR-085a · *GR*: `GR-025` C5 · `EVENT-032`, `QĐ-001`
**Given** một trận vừa bắt đầu và đang ở `LOBBY`, chưa vòng nào chạy, mọi ghế mang điểm `0` và chưa có thứ hạng nào được tính,
**When** admin mở **lớp công bố kết quả**,
**Then** hai kênh public dựng lớp công bố **bình thường** với dữ liệu hiện có — đủ số ghế của trận, mọi ghế điểm `0`, và **cả bảng đồng hạng 1** theo quy tắc đồng hạng; hệ thống MUST NOT hiện một trạng thái rỗng riêng, MUST NOT bỏ trống phần dữ liệu, và MUST NOT từ chối cú mở; **0** event điểm được sinh; và trạng thái trận **không đổi** — đóng lớp lại thì trận vẫn ở `LOBBY`.

## Assumptions

- **Trận là trận official, bốn ghế, mode sân khấu, cờ hiện đáp án BẬT** trừ khi kịch bản nói khác. Đây là mặc định của nguồn (`QĐ-062`, `QĐ-080`, `CLAUDE.md` §Hai mode trả lời), không phải một lựa chọn của spec này.
- **Một phiên admin giữ quyền điều khiển cho mỗi contest** tại một thời điểm (`NFR-07`, `GR-026` §Đồng thời). Mọi kịch bản trong spec này đứng trên giả định đó; ca nhiều người cùng giành quyền thuộc EPIC-001.
- **Kích thước khung nhìn tham chiếu** cho màn khán giả và màn MC là một quyết định thiết kế, không phải yêu cầu chuẩn tắc (`NFR-37`). Spec này phát biểu ràng buộc mà không khai con số pixel. Riêng lớp phủ dựng stream **có** con số chuẩn tắc `1920×1080` vì `PRD-REQ-073` khai thẳng.
- **Cỡ chữ của màn MC** cũng thuộc tầng thiết kế; nguồn chỉ nói *"chữ rất to"* (`PRD-REQ-074`), nên spec này phát biểu tiêu chí **đọc được ở khoảng cách sân khấu**.
- **Ba kênh trình diễn không giữ trạng thái nghiệp vụ nào của riêng chúng.** Toàn bộ trạng thái trận nằm ở **hai** thứ — nhật ký sự kiện và vòng nào đang mở (`INV-022`) — nên mọi thứ ba kênh này hiển thị đều là hàm của hai thứ đó cộng tập lớp phủ đang bật.
- **Hai kênh public nằm ngoài hệ phân quyền**, nên mọi phát biểu dạng *"kênh public không nhận X"* trong spec này là **hệ quả của cửa kiểm permission**, không phải một danh sách chặn theo tên kênh (`QĐ-094`, `QĐ-096`, `permissions.md` §8).
