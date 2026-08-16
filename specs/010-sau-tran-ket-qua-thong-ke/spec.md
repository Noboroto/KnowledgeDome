# Feature Specification: Sau trận — kết quả, thống kê, phát lại

**Feature Branch**: `010-sau-tran-ket-qua-thong-ke`

**Created**: 2026-08-04

**Status**: Draft

**Input**: EPIC-010 — Sau trận: kết quả, thống kê, phát lại (`docs/PRD.md` §11 dòng 512-523, §12 mục EPIC-010)

**Nguồn**: `docs/PRD.md` (EPIC-010 §11; PRD-REQ-079, PRD-REQ-080, PRD-REQ-081, PRD-REQ-096, PRD-REQ-097 §12 mục EPIC-010; PRD-REQ-115 và PRD-REQ-098 — hai requirement khai **Related epic** gồm EPIC-010; JOURNEY-007 §10; §14 FS-34, FS-34b; §15 NFR-12, NFR-14, NFR-16, NFR-17, NFR-35, NFR-35b; ma trận truy nguyên §22) · `docs/game-rules.md` (`GR-022`, `GR-028` — hai rule mà epic khai trực tiếp; và các rule mà PRD-REQ của epic này tham chiếu: `GR-030`, `GR-031`, `GR-037`; `GR-025` C5 ở đúng vế **quy tắc đồng hạng**; `GR-029` ở đúng vế **dòng chỉnh điểm tay trong biên bản**) · `docs/game-state-machine.md` (`STATE-008` FINISHED và §Trường đi kèm, §Khi `bỏ dở`; `EVENT-005`, `EVENT-006`, `EVENT-004`, `EVENT-007`, `EVENT-008`, `EVENT-028`, `EVENT-036`, `EVENT-048`; `T-012`, `T-013`, `T-014`, `T-016`, `T-016b`, `T-016c`, `T-020`, `T-021`, `T-022`; §Invalid transitions cho `STATE-008`; `INV-001`, `INV-022`) · `docs/decisions.md` (`QĐ-036`, `QĐ-037`, `QĐ-038`, `QĐ-039`, `QĐ-040`, `QĐ-077`, `QĐ-083`, `QĐ-084`, `QĐ-086`, `QĐ-091`, `QĐ-105`, `QĐ-107`, `QĐ-124` → `QĐ-130`) · `docs/permissions.md` (`PERM-015`, `PERM-020`, `PERM-021`, và các permission đọc kho đề ở §2.1) · `docs/glossary.md` (`TERM-011`, `TERM-021`, `TERM-022`, `TERM-023`, `TERM-024`, `TERM-031`, `TERM-038`, `TERM-039`, `TERM-040`, `TERM-041`, `TERM-047`, `TERM-048`) · `docs/traceability.md` · `.specify/memory/constitution.md` bản 1.4.0 · `CLAUDE.md` §Luật chơi, §Kiến trúc, §UX, §Quy ước code.

**Lưu ý về đầu vào**: lệnh gọi yêu cầu đọc `docs/rule-traceability.md` và `docs/reviews/prd-review.md` — **cả hai đường dẫn không tồn tại** trong repo. Tương ứng gần nhất của cái thứ nhất là `docs/traceability.md` (đã dùng). Không có tài liệu nào thay thế cái thứ hai; `docs/reviews/` chỉ chứa hồ sơ rà **luật chơi**, và theo `.specify/memory/constitution.md` §*Nguồn đã migrate xong* thì `docs/reviews/**` **không thoả cổng truy nguyên** nên dù có cũng không dùng làm nguồn requirement. Đây là chênh **đường dẫn đầu vào**, không phải một mâu thuẫn nghiệp vụ. Cùng cách xử lý đã dùng ở `specs/005` → `specs/009`.

**Quy ước truy nguyên của feature này** — năm điểm cần biết trước khi đọc bảng truy nguyên:

1. **Epic này đứng SAU mốc niêm phong, không đứng trên nó.** Cú bấm **Chốt trận** (`EVENT-007`), phép phân định hoà, và mọi transition vào `STATE-008` thuộc `specs/005` *(vòng đời trận)* và `specs/006` *(engine)*. EPIC-010 nhận `STATE-008` như **đầu vào đã có** và đặc tả những gì đọc ra được từ đó. Ranh giới này là lý do §Out of Scope dài hơn thường lệ.
2. **Bốn hiện vật, một nguồn.** `QĐ-084` khai **bốn loại gói xuất** — X1 *(gói contest)* · X2 *(toàn bộ kết quả + nhật ký sự kiện)* · X3 *(bản kê câu đã dùng)* · X4 *(kết quả rút gọn)* — cộng **biên bản trận** (`QĐ-077`), là hiện vật thứ năm và là hiện vật **cho người đọc**. EPIC-010 sở hữu **X2, X4 và biên bản**. X1 và X3 thuộc EPIC-003 *(`specs/003`)*. Ràng buộc **"X2, X3, X4 đều sinh từ nhật ký sự kiện, và biên bản sinh từ cùng nguồn đó"** là ràng buộc của epic này và được viết thành FR chuẩn tắc, vì nếu vỡ thì bốn hiện vật nói bốn con số khác nhau về cùng một trận.
3. **Hai hạng người đọc, hai mức chi tiết.** Biên bản là **một trận, cho người đọc**, in theo **lần chạy** kèm nhãn. X2 và X4 là **cấp contest, cho máy đọc**, gộp mọi trận. Không cái nào thay cái nào (`QĐ-084` §Ranh giới với biên bản PDF).
4. **`docs/decisions.md` là nguồn hợp lệ và được dùng.** `CLAUDE.md` §1 khai nó là *"nơi duy nhất ghi vì sao"* và nó nằm trong `docs/`, nên nó thoả Cổng 1 của constitution. Ba quyết định chi phối trực tiếp epic này: **`QĐ-077`** *(biên bản theo lần chạy; job dọn không đụng hiện vật đã xuất)*, **`QĐ-084`** *(bốn gói, một nguồn, ba biện pháp gác đáp án)*, **`QĐ-107`** *(trận ghi phiên bản câu đã dùng, đủ để in lại biên bản đúng nội dung đã lên sóng)*.
5. **Chuẩn tắc từ ngữ.** *"Bốc thăm"* (`TERM-041`) là **phân định người thắng**; *"rút đề"* (`TERM-047`) là **chọn câu hỏi** — tiếng Anh cả hai đều là *draw*, spec này không bao giờ dùng chữ đó trần trụi. *"Hoàn nguyên"* (`TERM-023`) là **thêm event đảo ngược**, không phải quay về ảnh chụp và không phải *undo*. *"Đóng sổ"* phủ **cả hai** nhãn `hoàn thành` và `bỏ dở`; khi cần nói riêng thì gọi đích danh nhãn.

## Phạm vi

EPIC-010 phủ **mọi thứ đọc ra được từ một trận sau khi nó rời tay người điều khiển**: bảng điểm cuối và thứ hạng của một trận đã niêm phong, **biên bản trận in theo lần chạy** kèm nhãn *đã bỏ · đã chạy lại · kết thúc sớm · hoàn thành*, **gói kết quả và nhật ký sự kiện cấp contest** cho máy đọc, **gói kết quả rút gọn** cho công bố và báo cáo, **thống kê ghi ngược kho đề**, và ràng buộc **hiện vật đã xuất nằm ngoài tầm với của hạn lưu trữ**.

Actor chính: **ACTOR-001 admin**; ACTOR-002 người ra đề là người **nhận** đầu ra của thống kê ghi ngược. Journey: **JOURNEY-007 — Sau trận**.

**Mục tiêu của epic** (`docs/PRD.md` §11): *"Kết quả có thể in ra, phân xử được, và nuôi ngược lại kho đề"*, với **user value** là *"biên bản đủ để trả lời khiếu nại, và số liệu để cải thiện kho đề"*.

**Phát lại một trận NẰM NGOÀI phạm vi** — `docs/PRD.md` §11 EPIC-010 khai thẳng ở trường **Out of scope**, và §20 xếp EPIC-010 là *"thuộc phạm vi, **trừ phát lại**"*. Tên epic vẫn mang chữ *"phát lại"* vì đó là tên do PRD đặt; spec này **không** đặc tả bất kỳ requirement phát lại nào. Tính chất **tất định** của `reduce(event log)` (`GR-028`) vẫn được đặc tả, nhưng ở đúng vai trò của nó trong epic này — **điều kiện để hai hiện vật khác nhau nói cùng một con số**, không phải một tính năng phát lại.

**Không thuộc phạm vi feature này** (xem §Out of Scope): cú bấm Chốt trận và phép phân định hoà (EPIC-005, EPIC-006); mọi cú bấm bỏ vòng / chạy lại / kết thúc sớm / huỷ trận sinh ra các nhãn mà biên bản in (EPIC-007); job dọn dữ liệu tự động và bảng `AuditLog` chung (EPIC-011); gói contest X1 và bản kê câu đã dùng X3 (EPIC-003); lớp phủ công bố kết quả trên hai kênh public (EPIC-009); hai hồ sơ triển khai (EPIC-012).

## User Scenarios & Testing *(mandatory)*

### US-001 — Đọc kết quả đã niêm phong của một trận (Priority: P1)

- **Title**: `FINISHED` là **chỉ đọc**, và hai nhãn đóng sổ cho hai loại kết quả khác nhau
- **Actor**: ACTOR-001 admin
- **Intent**: Mở lại một trận đã đóng sổ và thấy bảng điểm cuối, thứ hạng, người thắng, cùng lý do đóng sổ — mà không thao tác nào của mình đổi được một con số nào.
- **User value**: Đây là câu trả lời đầu tiên cho mọi khiếu nại, và là thứ mà bốn hiện vật còn lại của epic này đều chiếu ra. Không đọc được kết quả thì không có gì để in và không có gì để xuất.
- **Priority**: P1
- **Priority rationale**: Ba slice còn lại của epic *(biên bản, X2, X4)* đều là **phép chiếu** của cùng bảng kết quả này. Ngoài ra `STATE-008` là trạng thái **terminal** — mọi lỗi ở tầng đọc đều là lỗi vĩnh viễn vì không có đường sửa ngược, chỉ có đường *"ghi ở trận mới"*. Đây là slice duy nhất mà toàn bộ epic dừng lại nếu nó sai.
- **Independent test**: Chạy trọn một trận tới `FINISHED` nhãn `hoàn thành`, mở lại nó, và xác nhận đọc được bảng điểm cuối, thứ hạng, người thắng, `closedBy` và `closedAt`. Sau đó rà **toàn bộ** bề mặt của màn đó và xác nhận không tồn tại thao tác nào sinh event; gửi thẳng tới server một lệnh chỉnh điểm, một lệnh bỏ vòng và một lệnh chạy lại vòng cho trận này, xác nhận cả ba bị từ chối và điểm không đổi. Lặp lại với một trận nhãn `bỏ dở` và xác nhận không có thứ hạng, không có người thắng.
- **Related PRD requirements**: PRD-REQ-079, PRD-REQ-097
- **Related game rules**: `GR-022` C3, C4, C5, C7 · `GR-025` C5 · `GR-028` §Điều kiện, §Biên · `GR-029` C6 · `STATE-008` · `INV-001`, `INV-018` · `NFR-12`
- **Related journey**: JOURNEY-007

---

### US-002 — Biên bản trận in theo LẦN CHẠY (Priority: P1)

- **Title**: Gộp các lần chạy là xoá đúng thông tin mà người phân xử cần
- **Actor**: ACTOR-001 admin
- **Intent**: Xuất một biên bản của **một** trận, cho người đọc, trong đó một vòng chạy nhiều lần hiện thành **nhiều khối** theo thứ tự thời gian, mỗi khối kèm số lần chạy và một nhãn.
- **User value**: Đây là hiện vật mà đơn vị tổ chức đưa ra khi có khiếu nại. Một biên bản giấu lần hỏng không trả lời được câu hỏi *"vì sao điểm tụt"*, mà đó lại là câu hỏi duy nhất người khiếu nại đặt ra.
- **Priority**: P1
- **Priority rationale**: `docs/PRD.md` §11 khai **user value** của cả epic bằng đúng cụm *"biên bản đủ để trả lời khiếu nại"*, và `NFR-16` nâng ràng buộc *in theo lần chạy* thành yêu cầu phi chức năng chuẩn tắc. Đây là slice mà bỏ đi thì epic mất lý do tồn tại. *(Ghi chú: `PRD-REQ-079` mang priority **P2** ở tầng **requirement**; P1 ở đây là priority của **slice**, tính theo giá trị giao được và theo thứ tự phụ thuộc, không phải chép lại con số của requirement.)*
- **Independent test**: Chạy một trận trong đó một vòng bị **bỏ** rồi **chạy lại**, một vòng khác **kết thúc sớm**, và có ít nhất một lần **chỉnh điểm tay** kèm lý do. Xuất biên bản và xác nhận: vòng thứ nhất hiện **hai khối riêng với hai nhãn khác nhau**; vòng thứ hai mang nhãn *kết thúc sớm*; mọi event hoàn nguyên hiện thành **dòng riêng** kèm tên người bấm và lý do; không khối nào bị gộp và không lần hỏng nào bị giấu.
- **Related PRD requirements**: PRD-REQ-079, PRD-REQ-115 *(đúng vế **in lại đúng nội dung đã lên sóng**)*
- **Related game rules**: `GR-028` C1 → C4, §Không đổi gì · `GR-030` C1, C2, C3, C5, §Không đổi gì · `GR-029` C3, C4 · `GR-022` C5 · `INV-001`, `INV-022` · `NFR-16`, `NFR-17`
- **Related journey**: JOURNEY-007

---

### US-003 — Xuất toàn bộ kết quả và nhật ký sự kiện của contest (Priority: P2)

- **Title**: Bằng chứng phân xử ở dạng máy đọc được — và một đường rò đề nếu không gác
- **Actor**: ACTOR-001 admin
- **Intent**: Xuất **một** gói cấp contest gồm **mọi trận**: nhật ký sự kiện đầy đủ, điểm, thứ hạng, sự kiện hoàn nguyên kèm người bấm và lý do — xuất được ở mọi lúc, kể cả giữa trận.
- **User value**: Biên bản trả lời khiếu nại cho **một** trận và cho **người** đọc; gói này trả lời cho **cả contest** và cho **máy** đọc. Nó cũng là van thoát của hạn lưu trữ: gói đã xuất nằm ngoài hệ thống nên job dọn không chạm tới.
- **Priority**: P2
- **Priority rationale**: Đây là hiện vật **cấp contest** duy nhất mang đủ dữ liệu để dựng lại mọi con số, nên nó là nguồn mà X4 chiếu ra — làm nó trước thì X4 gần như miễn phí, làm ngược lại thì phải viết hai đường sinh dữ liệu. Nó xếp sau biên bản vì tình huống *"có khiếu nại ngay tại chỗ"* dùng biên bản, còn gói này phục vụ lưu trữ và phân xử nguội. `PRD-REQ-096` cũng mang **P2**.
- **Independent test**: Chạy hai trận trong cùng một contest, trận đầu có một vòng bị bỏ rồi chạy lại. Xuất gói và xác nhận nó chứa **cả hai** trận, chứa **đủ các lần chạy** của vòng đó, và mọi câu hỏi được tham chiếu bằng **định danh**. Xuất lần nữa giữa lúc trận thứ ba đang chạy và xác nhận gói mang dấu **"trận chưa đóng sổ"**. Xuất bằng một tài khoản **không** có quyền đọc kho đề và xác nhận gói **không** chứa nội dung câu lẫn đáp án. Xác nhận **mỗi** lần xuất để lại một dòng nhật ký thao tác, và xác nhận **không** tồn tại đường nhập gói này trở lại.
- **Related PRD requirements**: PRD-REQ-096, PRD-REQ-098 *(đúng vế **X2 có ở cả hai hồ sơ triển khai**)*
- **Related game rules**: `GR-028` C1 → C5 · `GR-030` C1, C2, C3 · `GR-037` §Cấm, và ba biện pháp gác của `QĐ-084` §Ranh giới đáp án · `INV-001` · `NFR-12`, `NFR-14`, `NFR-17`
- **Related journey**: JOURNEY-007

---

### US-004 — Xuất kết quả rút gọn (Priority: P3)

- **Title**: Công bố chỉ cần kết quả — đưa cả nhật ký ra ngoài cho việc đó là mở bề mặt rò đề mà không được gì
- **Actor**: ACTOR-001 admin
- **Intent**: Xuất một gói gọn gồm bảng điểm cuối, thứ hạng và người thắng theo từng trận, **không** chứa đáp án, **không** chứa nội dung câu hỏi, và **không** chứa cả định danh câu hỏi.
- **User value**: Gửi cho ban tổ chức, cho báo cáo, cho bảng tin — những nơi mà một gói mang nhật ký sự kiện đầy đủ vừa thừa vừa nguy hiểm.
- **Priority**: P3
- **Priority rationale**: Đây là **phép chiếu** của gói X2 với một phép lọc trừ, nên nó không mở khả năng mới nào mà chỉ thu hẹp bề mặt của một khả năng đã có. Bỏ nó thì admin vẫn công bố được kết quả bằng cách đọc từ US-001. `PRD-REQ-097` cũng mang **P3**.
- **Independent test**: Chốt một trận, xuất gói rút gọn và xác nhận nó chứa bảng điểm cuối, thứ hạng và người thắng, đồng thời rà **toàn bộ** nội dung gói và xác nhận **0** định danh câu hỏi, **0** nội dung câu và **0** đáp án. Thử xuất từ một trận **chưa** đóng sổ và xác nhận bị từ chối **kèm lý do**, và xác nhận không dữ liệu nào của trận đó đổi. Xác nhận **không** tồn tại đường nhập gói này trở lại.
- **Related PRD requirements**: PRD-REQ-097, PRD-REQ-098 *(đúng vế **X4 có ở cả hai hồ sơ triển khai**)*
- **Related game rules**: `GR-022` C3, C4, C5, C7 · `GR-025` C5 · `GR-028` §Biên · `STATE-008` §Trường đi kèm
- **Related journey**: JOURNEY-007

---

### US-005 — Thống kê ghi ngược kho đề (Priority: P3)

- **Title**: Vòng phản hồi để người ra đề biết câu nào quá dễ hay quá khó
- **Actor**: ACTOR-001 admin *(người kích hoạt)* · ACTOR-002 người ra đề *(người đọc kết quả)*
- **Intent**: Sau khi trận đóng sổ, số liệu sử dụng của các câu đã dùng chảy ngược về kho đề và hiện lên cạnh câu đó.
- **User value**: Không có vòng này thì kho đề là một chiều — soạn ra, dùng đi, và không ai biết câu nào hỏng. Đây là toàn bộ lý do `GOAL-005` tồn tại ở phía người ra đề.
- **Priority**: P3
- **Priority rationale**: Đây là giá trị **tích luỹ qua nhiều mùa**, không phải giá trị của ngày thi. Trận vẫn chạy trọn, vẫn phân xử được và vẫn công bố được nếu slice này chưa có. `PRD-REQ-081` cũng mang **P3**. Slice này cũng là slice có **nhiều ràng buộc phái sinh nhất** — hai bộ số theo `matchPurpose`, ranh giới *đã hiển thị*, giới hạn cùng bản cài — nên xếp sau là đúng thứ tự rủi ro.
- **Independent test**: Chạy trọn một trận trên **cùng bản cài** với kho đề, chốt trận, rồi mở kho đề và xác nhận các câu đã dùng trong trận đó hiện số liệu sử dụng, còn các câu chưa dùng thì không. Xác nhận bước ghi ngược **không** đụng tới cờ `usedInContest` *(cờ đó đã được đặt tại mốc hiển thị, thuộc `GR-031`)* và **không** sinh event nào trong nhật ký sự kiện của trận đã niêm phong.
- **Related PRD requirements**: PRD-REQ-081
- **Related game rules**: `GR-031` C1, C9, §Ranh giới *"đã dùng"*, §Không đổi gì · `GR-022` C5 · `STATE-008` §Khi `bỏ dở`
- **Related journey**: JOURNEY-007

---

### US-006 — Hiện vật đã xuất nằm ngoài tầm với của hạn lưu trữ (Priority: P3)

- **Title**: Hạn lưu trữ phục vụ quyền riêng tư, nhưng nó không được xoá mất bằng chứng mà đơn vị tổ chức đã chủ động giữ
- **Actor**: ACTOR-001 admin
- **Intent**: Biết chắc rằng một hiện vật đã xuất — biên bản, gói kết quả và nhật ký sự kiện, gói rút gọn — sẽ **không** bị job dọn dữ liệu chạm tới, và được **cảnh báo trước** khi job sắp chạy.
- **User value**: Không có bảo đảm này thì đơn vị tổ chức phải chọn giữa *tuân thủ hạn lưu trữ* và *giữ bằng chứng phân xử* — hai thứ mà `QĐ-040` và `QĐ-077` đều đòi cùng lúc.
- **Priority**: P3
- **Priority rationale**: Ràng buộc này chỉ có đối tượng khi **job dọn dữ liệu tồn tại**, mà bản thân job **nằm ngoài phạm vi v1** (`docs/PRD.md` §11 EPIC-011 §Out of scope). Nó vào v1 vì nó là **điều kiện đặt lên job**, không phải job — nhưng nó không chặn gì nếu chưa làm. `PRD-REQ-080` cũng mang **P3**.
- **Independent test**: Xuất cả ba hiện vật của một trận, đặt hạn lưu trữ về một giá trị đã quá hạn với trận đó, chạy bước dọn dữ liệu, và xác nhận: có **cảnh báo trước** khi bước dọn bắt đầu, và cả ba hiện vật đã xuất còn nguyên sau khi bước dọn kết thúc.
- **Related PRD requirements**: PRD-REQ-080
- **Related game rules**: — *(requirement khai **Related game rules: —**)*; `NFR-35`, `NFR-35b` · `QĐ-077`, `QĐ-040`, `QĐ-091`
- **Related journey**: JOURNEY-007

---

### Edge Cases

**Giá trị biên**

- Trận có **đúng một** người điểm cao nhất ⇒ một người thắng, không nhóm hoà (`GR-022` C3).
- Nhóm hoà **2** người và nhóm hoà **4** người *(toàn sân)* — cả hai đều là nhóm hợp lệ; kết quả in ra khác nhau ở số dòng, không khác nhau ở quy tắc.
- Hoà ở vị trí **ngoài** `tieBreakPositions` ⇒ ghi **đồng hạng**, hạng kế **nhảy qua** đúng số người đồng hạng (`GR-022` C4, `GR-025` C5). Ca cực đoan: **cả bốn** ghế cùng điểm ở một vị trí ngoài phạm vi ⇒ bốn dòng đồng hạng, không có hạng kế.
- Điểm **âm** trên bảng kết quả cuối — không có sàn, không kẹp về `0`, tham gia xếp hạng bình thường (`GR-028` §Biên, `INV-018`).
- **Ghế bỏ thi** *(0đ, vô hiệu hoá từ đầu trận — `QĐ-105`)* vẫn nằm trên bảng xếp hạng của kết quả cuối, nhưng **không** là ứng viên phân định hoà. Trận có ghế bỏ thi vẫn in đủ số ghế.
- Một vòng chạy lại **nhiều lần** ⇒ biên bản có **N+1** khối cho vòng đó; số lần chạy **không giới hạn** miễn kho còn câu (`GR-030` §Biên).
- Contest có **đúng một** trận và trận đó nhãn `bỏ dở` ⇒ gói cấp contest vẫn xuất được và vẫn có nội dung; nó chỉ không có thứ hạng.
- `SCORE_ADJUST` với `delta = 0` kèm lý do ⇒ **vẫn là một dòng** trong biên bản; đó là kênh duy nhất ghi xuất xứ một quyết định (`GR-029` C3, `TERM-024`).

**Trạng thái không hợp lệ**

- Chỉnh điểm, bỏ vòng, chạy lại vòng, vô hiệu hoá ghế — **mọi** thao tác đổi kết quả — trên một trận `STATE-008`: nút không bật, lệnh lọt tới server thì bị từ chối, **không ép được**, và **không** dữ liệu nào đổi (`GR-029` C6, §Invalid transitions).
- Xuất gói rút gọn từ một trận **chưa** đóng sổ ⇒ **từ chối kèm lý do** (`QĐ-084` §Thời điểm xuất).
- Xuất gói rút gọn từ một trận nhãn **`bỏ dở`** ⇒ **thành công**, gói mang bảng điểm tại mốc đóng, hai trường *thứ hạng* và *người thắng* **để trống**, cộng nhãn `bỏ dở` và lý do.
- Yêu cầu xuất gói kết quả và nhật ký sự kiện **không có** `results.export` ⇒ một thông điệp từ chối **duy nhất**, không phân biệt được theo trạng thái trận hay theo quyền đọc kho đề.
- Yêu cầu xuất gói **rút gọn** không có `results.export`, trên một trận **đang chạy** ⇒ nhận **cùng** thông điệp từ chối của nhánh thiếu permission; phản hồi **không** để lộ trạng thái trận. Cùng yêu cầu đó với phiên **có** permission ⇒ từ chối **kèm lý do** nêu rõ trận chưa đóng sổ.
- Yêu cầu thống kê ghi ngược cho một trận chạy trên **bản portable** ⇒ không có số liệu để ghi, **bất kể `matchPurpose`**; trận đó chỉ đóng góp đúng cờ *đã dùng* qua gói X3 (`PRD-REQ-081`, `QĐ-084` §Hệ quả).
- Một câu được hỏi ở **cả** trận `official` lẫn trận `practice` của cùng contest ⇒ hai bộ số **riêng**, không cộng vào nhau.
- Một vòng đi qua **bốn** lần chạy với bốn cửa ra khác nhau ⇒ bốn khối, trường *lần chạy* nhận `1 · 2 · 3 · 4` và trường *cách rời* nhận đủ bốn giá trị; khối hỏng thứ nhất phân biệt được với khối hỏng thứ ba.

**Thao tác lặp**

- Xuất **cùng một** gói hai lần trên cùng một trạng thái ⇒ nội dung **giống hệt** *(hệ quả của `reduce` tất định — `GR-028` §Bấm trùng)*, nhưng **hai** dòng nhật ký thao tác. Đây là idempotent về **nội dung**, không idempotent về **dấu vết**, và cả hai vế đều đúng có chủ đích.
- In biên bản hai lần ⇒ nội dung giống hệt; trạng thái trận không đổi.
- Tính lại số liệu thống kê nhiều lần cho cùng một trận ⇒ **cùng một kết quả**; số liệu là đại lượng dẫn xuất nên không cộng dồn và không cần dấu chống trùng.
- Gọi xuất **lại** sau một lần thất bại ⇒ **thành công bình thường**, không cần bước dọn dẹp nào trước; hệ thống không giữ trạng thái cho lần gọi hỏng.

**Trạng thái cũ và sự kiện trùng**

- Gói X2 xuất **giữa trận** mang dấu *"trận chưa đóng sổ"*; trận chạy tiếp thì gói cũ **không** tự cập nhật và **không** tự vô hiệu — nó là một ảnh chụp có dấu, đúng như thiết kế.
- Câu hỏi bị **sửa và Save** sau trận A ⇒ biên bản trận A vẫn in **nội dung đã lên sóng**, vì trận ghi lại phiên bản nào đã dùng (`PRD-REQ-115`, `QĐ-107`). Câu bị **xoá mềm** ⇒ vẫn không mất tham chiếu từ nhật ký của trận cũ.
- Một event hoàn nguyên và event gốc của nó **cùng có mặt** trong biên bản — đó là hành vi đúng, không phải trùng lặp (`GR-028` §Không đổi gì, `INV-001`).
- Một `TIE_BREAK_RESOLVED` **mất đối tượng** *(nhóm không còn bằng điểm — `GR-022` §Còn hiệu lực)* vẫn nằm nguyên trong nhật ký và vẫn hiện trong biên bản; nó chỉ không tham gia phép sắp thứ hạng.

**Thất bại một phần**

- Xuất gói bị gián đoạn giữa chừng ⇒ **không có hiện vật**, và cách xử lý là **gọi lại**. Xuất không phải một trạng thái hệ thống theo dõi, nên **không tồn tại** khái niệm *hiện vật dở dang* để phân loại hay dọn dẹp. Lần gọi hỏng để lại **một** dòng nhật ký mang kết quả *thất bại*.
- Tệp cụt nằm trên máy người vận hành ⇒ **ngoài phạm vi hệ thống**. `NFR-35b` nói về tệp mà **người vận hành đang giữ**, không khai một thực thể hệ thống quản lý vòng đời.

## Requirements *(mandatory)*

### Functional Requirements

#### Nhóm A — Kết quả đã niêm phong của một trận (US-001)

- **FR-001**: Hệ thống MUST cho đọc lại **bảng điểm cuối** của một trận đã đóng sổ, gồm **đủ số ghế** của trận đó — kể cả **ghế bỏ thi** đang mang điểm `0`. *(US-001 · PRD-REQ-079 · `STATE-008`, `QĐ-105` · AC-001)*
- **FR-002**: Điểm trên bảng kết quả cuối MUST hiển thị **nguyên giá trị**, gồm cả **giá trị âm**; hệ thống MUST NOT kẹp về `0` và MUST NOT ẩn. *(US-001 · PRD-REQ-079 · `GR-028` §Biên, `INV-018` · AC-002)*
- **FR-003**: Với trận nhãn **`hoàn thành`**, hệ thống MUST hiển thị **thứ hạng đã chốt** và **người thắng**. *(US-001 · PRD-REQ-097 · `GR-022` C3, C7, `STATE-008` · AC-001, AC-003)*
- **FR-004**: Nhóm hoà **ngoài** phạm vi phân định MUST ghi **đồng hạng**, và hạng kế MUST **nhảy qua** đúng số người đồng hạng. *(US-001 · PRD-REQ-097 · `GR-022` C4, `GR-025` C5, `TERM-040` · AC-004)*
- **FR-005**: Với trận nhãn **`bỏ dở`**, hệ thống MUST NOT hiển thị thứ hạng và MUST NOT hiển thị người thắng; điểm MUST hiển thị **tại mốc đóng**, không hoàn nguyên. *(US-001 · PRD-REQ-079 · `GR-022` C5, `STATE-008` §Khi `bỏ dở` · AC-005)*
- **FR-006**: Hệ thống MUST hiển thị **nhãn đóng sổ** *(`hoàn thành` | `bỏ dở`)*, **người đóng sổ**, **thời điểm đóng sổ**, và **lý do** khi nhãn là `bỏ dở`. *(US-001 · PRD-REQ-079 · `STATE-008` §Trường đi kèm · AC-005, AC-006)*
- **FR-007**: Một trận ở trạng thái đã đóng sổ MUST là **chỉ đọc**: hệ thống MUST từ chối **mọi** thao tác sinh hoặc đổi event điểm của trận đó — chỉnh điểm tay, bỏ vòng, chạy lại vòng, kết thúc sớm, vô hiệu hoá ghế — và MUST NOT có đường ép qua. *(US-001 · PRD-REQ-079 · `GR-029` C6, `GR-030`, §Invalid transitions, `INV-001` · AC-007, AC-008)*
- **FR-008**: Việc từ chối ở FR-007 MUST được kiểm ở **server**, không chỉ ở giao diện, và MUST NOT làm đổi bất kỳ dữ liệu nào của trận. *(US-001 · PRD-REQ-079 · `CLAUDE.md` §Zero-trust, §Invalid transitions · AC-007, AC-008)*
- **FR-009**: Đọc lại kết quả MUST NOT sinh event nào trong nhật ký sự kiện của trận. *(US-001 · PRD-REQ-079 · `GR-028` C5 · AC-009)*
- **FR-010**: Một `TIE_BREAK_RESOLVED` **mất đối tượng** MUST vẫn còn trong nhật ký và MUST NOT bị đánh dấu vô hiệu; nó MUST NOT tham gia phép sắp thứ hạng của kết quả cuối. *(US-001 · PRD-REQ-079 · `GR-022` §Còn hiệu lực, `GR-028` §Không phải mọi event, `QĐ-083`, `INV-001` · AC-010)*

#### Nhóm B — Biên bản trận theo lần chạy (US-002)

- **FR-011**: Hệ thống MUST xuất được một **biên bản của MỘT trận**, dành cho **người đọc**, chứa mọi vòng đã chạy. *(US-002 · PRD-REQ-079 · `QĐ-077`, `NFR-16` · AC-011)*
- **FR-011a**: Việc xuất biên bản MUST khả dụng ở **cả hai** hồ sơ triển khai và MUST NOT phụ thuộc dịch vụ ngoài hay kết nối Internet — cùng ràng buộc với bốn loại gói. *(US-002 · PRD-REQ-098, PRD-REQ-085 · AC-038a)*
- **FR-012**: Một vòng chạy **nhiều lần** MUST in thành **nhiều khối**, xếp theo **thứ tự thời gian**. Hệ thống MUST NOT gộp các lần chạy và MUST NOT giấu lần hỏng. *(US-002 · PRD-REQ-079 · `QĐ-077`, `GR-030` C2, `NFR-16` · AC-012)*
- **FR-013**: Mỗi khối MUST mang **hai trường tách bạch**: **(1) cách rời** — một giá trị thuộc tập *đã bỏ* · *đã chạy lại* · *kết thúc sớm* · *hoàn thành*, mô tả **cửa ra đã kết thúc lần chạy đó**; và **(2) lần chạy thứ mấy** — một số nguyên đếm từ `1` trong phạm vi vòng đó. Hệ thống MUST NOT trộn hai trục vào một trường. *(US-002 · PRD-REQ-079 · `QĐ-077`, `GR-030` C1, C2, C3, `T-012`, `T-013`, `T-014`, `TERM-031` · AC-012, AC-012a, AC-013, AC-014)*
- **FR-013a**: Giá trị *hoàn thành* của trường **cách rời** MUST áp cho **bất kỳ** lần chạy nào kết thúc bình thường, kể cả lần chạy sinh ra bởi một cú chạy lại. Trường **lần chạy thứ mấy** MUST tăng đơn điệu theo thời gian trong phạm vi một vòng và MUST NOT lặp lại giá trị. *(US-002 · PRD-REQ-079 · `QĐ-077`, `GR-030` §Biên · AC-012a)*
- **FR-014**: Vòng bị **bỏ** MUST hiện với nhãn *đã bỏ* và MUST hiện các **event đảo ngược** của nó. *(US-002 · PRD-REQ-079 · `GR-030` C1, `GR-028` C4 · AC-013)*
- **FR-015**: Vòng **kết thúc sớm** MUST hiện với nhãn *kết thúc sớm*, và biên bản MUST NOT chứa event đảo ngược nào sinh bởi cửa ra đó. *(US-002 · PRD-REQ-079 · `GR-030` C3 · AC-014)*
- **FR-016**: **Mọi sự kiện hoàn nguyên** MUST hiện trong biên bản **như mọi sự kiện khác** — một dòng riêng kèm **tên người bấm** và **lý do**. Hệ thống MUST NOT thể hiện hoàn nguyên bằng một phép trừ thầm lặng. *(US-002 · PRD-REQ-079 · `QĐ-077`, `GR-028` §Điều kiện, `TERM-023` · AC-015)*
- **FR-017**: Mỗi **điều chỉnh điểm tay** MUST hiện thành một dòng kèm **ghế**, **delta**, **lý do**, **người bấm** và **thời điểm** — gồm cả trường hợp `delta = 0`. *(US-002 · PRD-REQ-079 · `GR-029` C1, C3, `TERM-024` · AC-016)*
- **FR-018**: Điều chỉnh điểm tay MUST **giữ nguyên** trong biên bản kể cả khi vòng liên quan bị bỏ hoặc chạy lại; hệ thống MUST NOT cuốn nó theo phạm vi hoàn nguyên của vòng. *(US-002 · PRD-REQ-079 · `GR-029` C4, C5, `GR-030` §Không đổi gì · AC-017)*
- **FR-019**: Biên bản của trận nhãn **`bỏ dở`** MUST in **điểm tại mốc đóng** kèm nhãn *trận bỏ dở* và **đủ các vòng đã chạy**; nó MUST NOT in thứ hạng và MUST NOT in người thắng. *(US-002 · PRD-REQ-079 · `GR-022` C5, `STATE-008` §Khi `bỏ dở` · AC-018)*
- **FR-020**: Biên bản MUST in **nội dung câu hỏi đúng như đã lên sóng** — tức theo **phiên bản** mà trận đã ghi lại tại mốc hiển thị — kể cả khi câu đó đã bị sửa hoặc xoá mềm sau trận. *(US-002 · PRD-REQ-115, PRD-REQ-079 · `QĐ-107`, `INV-022` · AC-019, AC-020)*
- **FR-021**: Biên bản MUST sinh từ **cùng một nguồn** với gói kết quả và nhật ký sự kiện — nhật ký sự kiện của trận. Hệ thống MUST NOT hiện thực biên bản như một đường tính điểm độc lập. *(US-002, US-003 · PRD-REQ-079, PRD-REQ-096 · `QĐ-084` §Một nguồn, §Ranh giới với biên bản PDF · AC-021)*
- **FR-022**: In biên bản MUST NOT sinh event nào và MUST NOT đổi trạng thái trận; in lại nhiều lần trên cùng một trạng thái MUST cho **nội dung giống hệt**. *(US-002 · PRD-REQ-079 · `GR-028` §Bấm trùng, C5 · AC-022)*
- **FR-023**: Mỗi lần xuất biên bản MUST ghi một dòng vào **nhật ký thao tác**, gồm người thực hiện, hành động, đối tượng, thời điểm và địa chỉ nguồn. *(US-002 · PRD-REQ-079 · `NFR-14`, `PERM-020` · AC-023)*
- **FR-024**: Mọi mốc thời gian trong biên bản MUST hiển thị theo **UTC+7**, và toàn bộ chuỗi MUST là **tiếng Việt**. *(US-002 · PRD-REQ-079 · `NFR-38`, `CLAUDE.md` §Quy ước code · AC-024)*

#### Nhóm C — Gói toàn bộ kết quả và nhật ký sự kiện (US-003)

- **FR-025**: Hệ thống MUST xuất được một gói **cấp contest** chứa **mọi trận** của contest đó. *(US-003 · PRD-REQ-096 · `QĐ-084` X2 · AC-025)*
- **FR-026**: Gói này MUST chứa, cho từng trận: **nhật ký sự kiện đầy đủ**, **điểm**, **thứ hạng**, và **sự kiện hoàn nguyên kèm người bấm và lý do**. *(US-003 · PRD-REQ-096 · `QĐ-084` X2, `GR-028` C1 → C4 · AC-025, AC-026)*
- **FR-027**: Gói này MUST chứa **đủ các lần chạy** của một vòng đã bị bỏ rồi chạy lại — cùng ranh giới với biên bản ở FR-012. *(US-003 · PRD-REQ-096 · `GR-030` C1, C2, `QĐ-077` · AC-027)*
- **FR-028**: Gói này MUST tham chiếu câu hỏi bằng **định danh**. *(US-003 · PRD-REQ-096 · `QĐ-084` §Ranh giới đáp án · AC-028)*
- **FR-029**: Gói này MUST NOT nhúng nội dung câu hỏi và MUST NOT nhúng đáp án **trừ khi người xuất có quyền đọc kho đề**. *(US-003 · PRD-REQ-096 · `QĐ-084` §Ranh giới đáp án, `GR-037` §Cấm · AC-029, AC-030)*
- **FR-030**: **Mỗi** lần xuất gói này MUST ghi một dòng vào nhật ký thao tác — nó là một lần **xem đáp án ở quy mô lớn**. *(US-003 · PRD-REQ-096 · `QĐ-084` §Ranh giới đáp án, `NFR-14` · AC-031)*
- **FR-031**: Gói xuất từ một trận **chưa đóng sổ** MUST mang dấu **"trận chưa đóng sổ"**. *(US-003 · PRD-REQ-096 · `QĐ-084` §Thời điểm xuất · AC-032)*
- **FR-032**: Hệ thống MUST cho xuất gói này ở **mọi lúc**, kể cả giữa một trận đang chạy; việc xuất MUST NOT đổi trạng thái trận và MUST NOT sinh event nào của trận. *(US-003 · PRD-REQ-096 · `QĐ-084` §Thời điểm xuất, `GR-028` C5 · AC-032, AC-033)*
- **FR-033**: Gói này MUST **chỉ có chiều ra**. Hệ thống MUST NOT có đường nhập nó trở lại. *(US-003 · PRD-REQ-096 · `QĐ-084` §Chỉ X1 và X3 nhập lại được · AC-034)*
- **FR-034**: Gói này MUST sinh từ **nhật ký sự kiện**, và gói kết quả rút gọn MUST là **phép chiếu** của nó. Hệ thống MUST NOT hiện thực hai gói thành hai đường sinh dữ liệu độc lập. *(US-003, US-004 · PRD-REQ-096, PRD-REQ-097 · `QĐ-084` §Một nguồn · AC-035)*
- **FR-035**: Xuất gói này hai lần trên cùng một trạng thái MUST cho **nội dung giống hệt**, và MUST ghi **hai** dòng nhật ký thao tác. *(US-003 · PRD-REQ-096 · `GR-028` §Bấm trùng, `NFR-14` · AC-036)*
- **FR-036**: Nội dung gói MUST đủ để tính lại điểm của **bất kỳ mốc thời gian nào** trong trận từ chuỗi event của nó. *(US-003 · PRD-REQ-096 · `GR-028` §Điều kiện, C5, `NFR-12` · AC-037)*
- **FR-037**: Việc xuất gói này MUST NOT phụ thuộc dịch vụ ngoài hay kết nối Internet, và MUST khả dụng ở **cả hai** hồ sơ triển khai. *(US-003 · PRD-REQ-098 · `QĐ-084` §Quyết định · AC-038)*
- **FR-038**: Xuất gói này MUST đòi permission `results.export`. *(US-003 · PRD-REQ-096 · `PERM-020`, `CLAUDE.md` §Zero-trust · AC-039)*
- **FR-038a**: Phép kiểm `results.export` MUST chạy **trước** phép kiểm trạng thái trận và **trước** phép kiểm quyền đọc kho đề. Yêu cầu không có permission này MUST bị từ chối bằng **một** thông điệp duy nhất, và thông điệp đó MUST NOT khác nhau theo trạng thái trận hay theo việc phiên gọi có quyền đọc kho đề hay không. **0** byte nội dung gói MUST rời server ở nhánh từ chối này. *(US-003 · PRD-REQ-096 · `PERM-020`, `GR-037` §Thứ tự đánh giá, `CLAUDE.md` §Zero-trust · AC-029a, AC-039)*
- **FR-038b**: Gói kết quả và nhật ký sự kiện có **đúng MỘT** cửa từ chối — `results.export` ở FR-038a. Khi phiên gọi **có** permission đó, hệ thống MUST hoàn tất lần xuất: trạng thái trận **chỉ** quyết việc gói có mang dấu *"trận chưa đóng sổ"* hay không, và quyền đọc kho đề **chỉ** quyết gói có nhúng nội dung câu cùng đáp án hay không. Hệ thống MUST NOT từ chối một yêu cầu xuất **gói này** vì trận đang chạy, và MUST NOT từ chối vì phiên gọi thiếu quyền đọc kho đề. *(US-003 · PRD-REQ-096 · `QĐ-084` §Thời điểm xuất, §Ranh giới đáp án · AC-029, AC-029a, AC-032)*

#### Nhóm D — Gói kết quả rút gọn (US-004)

- **FR-039**: Hệ thống MUST xuất được một gói **kết quả rút gọn** gồm **bảng điểm cuối**, **thứ hạng** và **người thắng**, theo từng trận. *(US-004 · PRD-REQ-097 · `QĐ-084` X4 · AC-040)*
- **FR-040**: Gói này MUST NOT chứa đáp án, MUST NOT chứa nội dung câu hỏi, và MUST NOT chứa cả **định danh câu hỏi**. *(US-004 · PRD-REQ-097 · `QĐ-084` §Ranh giới đáp án · AC-041)*
- **FR-041**: Gói này MUST chỉ xuất được từ trận **đã đóng sổ** — **cả hai nhãn** `hoàn thành` lẫn `bỏ dở` đều thoả; yêu cầu xuất từ trận **chưa** đóng sổ MUST bị **từ chối kèm lý do**, và MUST NOT đổi dữ liệu nào của trận. *(US-004 · PRD-REQ-097 · `QĐ-084` §Thời điểm xuất, `GR-022`, `STATE-008` §Mô tả · AC-042, AC-042a)*
- **FR-041a**: Với trận nhãn **`bỏ dở`**, gói MUST chứa bảng điểm **tại mốc đóng**, MUST để **trống** hai trường *thứ hạng* và *người thắng*, và MUST mang **nhãn `bỏ dở`** cùng **lý do đóng sổ**. Hệ thống MUST NOT suy ra một thứ hạng cho trận này và MUST NOT bỏ nhãn khiến gói trông như một kết quả có thứ hạng. *(US-004 · PRD-REQ-097 · `GR-022` C5, `STATE-008` §Khi `bỏ dở`, §Trường đi kèm · AC-042a)*
- **FR-041b**: Gói kết quả rút gọn có **HAI** cửa từ chối, và hệ thống MUST đánh giá theo đúng thứ tự sau:
  1. **`results.export`** — thiếu ⇒ từ chối bằng **một** thông điệp duy nhất. Thông điệp đó MUST NOT khác nhau theo trạng thái trận, theo nhãn đóng sổ hay theo quyền đọc kho đề; **0** byte nội dung gói rời server.
  2. **Trạng thái trận** — trận chưa đóng sổ ⇒ từ chối **kèm lý do** nêu rõ trận chưa đóng sổ nên chưa có thứ hạng.
  3. Cả hai đạt ⇒ xuất gói theo FR-039, FR-040, FR-041a.

  Một phiên **thiếu** `results.export` MUST NOT suy ra được trạng thái trận từ phản hồi. *(US-004 · PRD-REQ-097, PRD-REQ-096 · `PERM-020`, `QĐ-084` §Thời điểm xuất, `GR-037` §Thứ tự đánh giá, `CLAUDE.md` §Zero-trust · AC-042, AC-042b, AC-045)*
- **FR-042**: Gói này MUST **chỉ có chiều ra**. Hệ thống MUST NOT có đường nhập nó trở lại. *(US-004 · PRD-REQ-097 · `QĐ-084` §Chỉ X1 và X3 nhập lại được · AC-043)*
- **FR-043**: Thứ hạng trong gói này MUST theo **cùng quy tắc đồng hạng** với FR-004 — đồng hạng ghi đủ, hạng kế nhảy qua. *(US-004 · PRD-REQ-097 · `GR-022` C4, `GR-025` C5 · AC-044)*
- **FR-044**: Việc xuất gói này MUST NOT phụ thuộc dịch vụ ngoài hay kết nối Internet, và MUST khả dụng ở **cả hai** hồ sơ triển khai. *(US-004 · PRD-REQ-098 · `QĐ-084` §Quyết định · AC-038)*
- **FR-045**: Xuất gói này MUST đòi permission `results.export` và MUST ghi một dòng vào nhật ký thao tác. *(US-004 · PRD-REQ-097 · `PERM-020`, `NFR-14` · AC-045)*

#### Nhóm E — Thống kê ghi ngược kho đề (US-005)

- **FR-046**: Từ mốc một trận **đóng sổ** — **cả hai nhãn** `hoàn thành` lẫn `bỏ dở` — **số liệu sử dụng** của các câu đã dùng trong trận đó MUST phản ánh trận đó khi người ra đề xem chúng trong kho đề. *(US-005 · PRD-REQ-081 · `GR-031` C1, `GR-022` C5 · AC-046, AC-046a)*
- **FR-046d**: Số liệu sử dụng MUST là **đại lượng dẫn xuất** — tính từ nhật ký sự kiện của mọi trận đã đóng sổ tại mốc đọc. Hệ thống MUST NOT giữ nó như một giá trị được cộng dồn, MUST NOT cần dấu chống trùng, và MUST NOT có thao tác nào làm số liệu lệch khỏi nguồn. Tính lại nhiều lần trên cùng một tập trận MUST cho **cùng một** kết quả. *(US-005 · PRD-REQ-081 · `GR-028` §Điều kiện, §Bấm trùng, `INV-001` · AC-046c)*
- **FR-046a**: Số liệu sử dụng của một câu MUST gồm ba nhóm trường: **(1) số lần câu đã hiển thị**; **(2) lần dùng gần nhất** — thời điểm và trận; **(3) kết quả trả lời** — số lượt được chấm **Đúng**, **Sai** và **Huỷ kết quả**, trên **tổng số lượt**. Mọi trường MUST dẫn xuất từ nhật ký sự kiện đã có; hệ thống MUST NOT đòi thu thập dữ liệu mới trong trận để phục vụ yêu cầu này. *(US-005 · PRD-REQ-081 · `GR-026`, `GR-028` C1, C2, `GR-031` C1 · AC-046)*
- **FR-046b**: Số liệu MUST **tách thành hai bộ theo `matchPurpose`** — một bộ cho trận `official`, một bộ cho trận `practice`. Hai bộ MUST NOT cộng vào nhau ở bất kỳ đâu, và bề mặt hiển thị ở kho đề MUST cho phân biệt được bộ nào là bộ nào. *(US-005 · PRD-REQ-081 · `GR-031` C9, `TERM-015` · AC-046b)*
- **FR-046e**: Bề mặt hiển thị **cả hai** bộ số MUST có mặt ở **v1**. Ở v1 — nơi `matchPurpose: practice` chưa được bật — bộ `practice` MUST hiển thị **giá trị rỗng** *(số lần đã hiển thị `0`, không có lần dùng gần nhất, `0` lượt trên tổng `0`)*, và hệ thống MUST NOT ẩn bộ đó, MUST NOT gộp nó vào bộ `official`, và MUST NOT đòi một thay đổi bề mặt nào ở v1.5 để nó bắt đầu có số. *(US-005 · PRD-REQ-081 · `CLAUDE.md` §Phạm vi phiên bản, §Quy ước code · AC-046b)*
- **FR-046c**: Số liệu sử dụng MUST KHÔNG chứa **thời gian trả lời** của thí sinh. *(US-005 · PRD-REQ-081 · `GR-033`, `TERM-028` · AC-046)*
- **FR-047**: Câu **đã dùng** MUST hiện số liệu sử dụng khi người ra đề xem nó trong kho đề. *(US-005 · PRD-REQ-081 · `GR-031` §Ranh giới *"đã dùng"* · AC-046, AC-047)*
- **FR-048**: Tập câu được ghi số liệu MUST là tập câu **đã hiển thị cho thí sinh** — cùng ranh giới với cờ `usedInContest`. Câu **đã rút nhưng chưa hiển thị** MUST NOT nhận số liệu. *(US-005 · PRD-REQ-081 · `GR-031` §Ranh giới *"đã dùng"*, C1, `TERM-048` · AC-047, AC-048)*
- **FR-049**: Bước ghi ngược MUST NOT đổi cờ `usedInContest` và MUST NOT sinh event nào trong nhật ký sự kiện của trận đã niêm phong. *(US-005 · PRD-REQ-081 · `GR-031` §Không đổi gì, `INV-001` · AC-049)*
- **FR-050**: Yêu cầu này MUST chỉ áp cho trận chạy **trên cùng bản cài** với kho đề. Hệ thống MUST NOT kỳ vọng số liệu thống kê từ trận chạy trên hồ sơ portable — **bất kể `matchPurpose`**; trận đó đóng góp đúng cờ *đã dùng* qua bản kê câu đã dùng. Giới hạn này **độc lập** với phép tách hai bộ số ở FR-046b. *(US-005 · PRD-REQ-081 · `QĐ-084` §Hệ quả · AC-050)*
- **FR-051**: Số liệu sử dụng MUST NOT là một đường đọc đáp án — nó MUST NOT làm lộ nội dung câu hay đáp án cho phiên không có quyền đọc kho đề. *(US-005 · PRD-REQ-081 · `GR-037` §Cấm, `CLAUDE.md` §Phạm vi hiển thị đáp án · AC-051)*

#### Nhóm F — Hiện vật đã xuất và hạn lưu trữ (US-006)

- **FR-052**: Job dọn dữ liệu theo hạn lưu trữ MUST **cảnh báo trước** khi chạy. *(US-006 · PRD-REQ-080 · `QĐ-077`, `NFR-35` · AC-052)*
- **FR-053**: Job dọn dữ liệu MUST NOT đụng vào **biên bản đã xuất**. *(US-006 · PRD-REQ-080 · `QĐ-077`, `NFR-35` · AC-053)*
- **FR-054**: **Mọi gói đã xuất** MUST được hiểu là nằm **ngoài** tầm với của hạn lưu trữ. *(US-006 · PRD-REQ-080 · `NFR-35b`, `QĐ-084` §X2 là van thoát · AC-053, AC-054)*
- **FR-055**: Hệ thống MUST cho phép xuất hiện vật **trước** khi hạn lưu trữ tới, để bằng chứng phân xử còn nguyên sau khi dữ liệu thô bị dọn. *(US-006 · PRD-REQ-080 · `QĐ-077`, `QĐ-091` · AC-054)*
- **FR-055a**: Xuất MUST NOT là một **trạng thái** mà hệ thống theo dõi. Một lần gọi hoặc cho ra hiện vật hoàn chỉnh, hoặc không cho ra gì; hệ thống MUST NOT giữ khái niệm *"hiện vật dở dang"*, MUST NOT có bản ghi vòng đời cho một lần xuất, và MUST cho **gọi lại bao nhiêu lần cũng được** mà không cần dọn dẹp gì trước. *(US-006, US-002, US-003, US-004 · PRD-REQ-080 · `NFR-35b` · AC-054a)*
- **FR-055b**: **Mỗi** lần gọi xuất MUST để lại **đúng một** dòng nhật ký thao tác mang **kết quả** — *thành công* hoặc *thất bại* — cùng người thực hiện, hành động, đối tượng, thời điểm và địa chỉ nguồn. Áp cho **cả ba** loại hiện vật, và áp cho cả lần **bị từ chối** lẫn lần **thất bại**. *(US-002, US-003, US-004, US-006 · PRD-REQ-080 · `NFR-14` · AC-054a)*

#### Nhóm G — Ràng buộc xuyên suốt (mọi US)

- **FR-056**: Mọi thao tác bất đồng bộ của feature này — in biên bản, xuất gói, chạy thống kê ghi ngược — MUST hiển thị trạng thái đang xử lý rõ ràng: spinner, progress hoặc skeleton. *(mọi US · `NFR-36`, `CLAUDE.md` §UX · AC-055)*
- **FR-057**: Mỗi thao tác xuất MUST có phản hồi **thành công hoặc thất bại** ngay sau khi kết thúc. *(mọi US · `NFR-36`, `CLAUDE.md` §UX · AC-055)*
- **FR-058**: Nội dung chính của mỗi màn thuộc feature này MUST nằm gọn trong **một** khung nhìn tham chiếu, không nhồi nhét, không ép cuộn để thấy phần quan trọng. Danh sách dài MUST ưu tiên phân trang. *(mọi US · `NFR-37`, `CLAUDE.md` §UX · AC-056)*
- **FR-059**: Mọi chuỗi giao diện MUST là **tiếng Việt** và MUST đến từ file hằng số, không hard-code trong markup; mọi mốc thời gian hiển thị MUST theo **UTC+7**, không có thiết lập múi giờ theo người dùng. *(mọi US · `NFR-38`, `CLAUDE.md` §Quy ước code · AC-057)*
- **FR-060**: Mỗi màn thuộc feature này MUST có nút quay lại rõ ràng về màn trước hoặc menu. *(mọi US · `NFR-39`, `CLAUDE.md` §Điều hướng · AC-058)*

### Key Entities

- **Kết quả trận**: bảng điểm cuối + thứ hạng của một trận, chốt tại mốc đóng sổ. Thuộc tính: nhãn đóng sổ *(`hoàn thành` | `bỏ dở`)*, người đóng sổ, thời điểm đóng sổ, lý do *(bắt buộc khi `bỏ dở`)*. **Dẫn xuất** từ nhật ký sự kiện, không phải một bản ghi được sửa tại chỗ (`TERM-038`, `GR-028`).
- **Khối biên bản**: một **lần chạy** của một vòng trong một trận. Thuộc tính: vòng · **lần chạy thứ mấy** *(số nguyên từ `1`, không lặp trong phạm vi vòng)* · **cách rời** *(đã bỏ · đã chạy lại · kết thúc sớm · hoàn thành)* · tập event thuộc lần chạy đó. **Hai trường đầu là hai trục độc lập**, không suy ra được từ nhau. Một vòng có từ một tới N khối.
- **Hiện vật đã xuất**: một tệp đã rời hệ thống — biên bản, gói kết quả và nhật ký sự kiện, gói rút gọn. Thuộc tính mang **trong tệp**: loại · trận hoặc contest nguồn · thời điểm xuất · người xuất · dấu *"trận chưa đóng sổ"* nếu có · nhãn `bỏ dở` và lý do nếu có. **Không phải một thực thể hệ thống theo dõi** — nó nằm ngoài hệ thống, do người vận hành giữ, nên nằm **ngoài** vòng đời của dữ liệu thô. Hệ thống không có bản ghi trạng thái nào cho một lần xuất.
- **Số liệu sử dụng của câu hỏi**: **đại lượng dẫn xuất** từ nhật ký sự kiện của mọi trận đã đóng sổ, gắn với **câu** chứ không gắn với trận — không phải một giá trị được ghi và cộng dồn. Ba nhóm trường: **số lần đã hiển thị** · **lần dùng gần nhất** *(thời điểm + trận)* · **kết quả trả lời** *(số lượt Đúng / Sai / Huỷ kết quả trên tổng số lượt)*. Tồn tại thành **hai bộ độc lập** theo `matchPurpose` — `official` và `practice` — không bao giờ cộng vào nhau. **Không** chứa số liệu thời gian trả lời.

## Success Criteria *(mandatory)*

- **SC-001**: **0** thao tác trên một trận đã đóng sổ đổi được một con số nào — thử **100%** danh sách thao tác đổi kết quả *(chỉnh điểm, bỏ vòng, chạy lại vòng, kết thúc sớm, vô hiệu hoá ghế)*, mỗi thao tác gửi cả qua giao diện lẫn thẳng tới server, trên trận nhãn `hoàn thành` và trận nhãn `bỏ dở`.
- **SC-002**: **100%** số vòng chạy nhiều lần đều in ra đúng **số khối bằng số lần chạy** — thử với vòng chạy lại 1, 2 và 5 lần, và với cả năm vòng của luật. Trong mỗi trường hợp, trường *lần chạy* nhận đúng dãy `1..N` **không lặp**, và trường *cách rời* nhận đúng cửa ra thực tế của từng lần chạy; **0** khối nào phải suy trường này ra từ trường kia.
- **SC-003**: **0** sự kiện hoàn nguyên bị in thành một phép trừ không có dòng riêng — rà **100%** event đảo ngược của một trận có ít nhất một vòng bị bỏ, và mỗi dòng phải có đủ tên người bấm và lý do.
- **SC-004**: **100%** số nhóm hoà ngoài phạm vi phân định đều in **đồng hạng** với hạng kế **nhảy qua** đúng số người — thử với nhóm 2, 3 và 4 người, trên cả bảng kết quả cuối, biên bản và gói rút gọn, và **ba** hiện vật cho **cùng một** thứ hạng.
- **SC-005**: **0** byte mang đáp án hoặc nội dung câu hỏi có trong gói kết quả và nhật ký sự kiện xuất bởi tài khoản **không** có quyền đọc kho đề — rà bằng khám nội dung tệp, trên một contest có đủ năm vòng và ít nhất 60 câu đã dùng.
- **SC-006**: **0** định danh câu hỏi có trong gói kết quả rút gọn — rà **100%** nội dung tệp, trên cùng contest của SC-005.
- **SC-007**: **100%** số lần xuất gói và in biên bản đều để lại đúng **một** dòng nhật ký thao tác — đối chiếu số lần xuất với số dòng nhật ký sau **50** lần xuất liên tiếp gồm cả ba loại hiện vật.
- **SC-008**: **100%** số cặp *xuất hai lần trên cùng một trạng thái* cho ra **nội dung giống hệt** — so khớp từng byte, thử **20** cặp trên cả ba loại hiện vật.
- **SC-009**: **100%** số gói xuất từ trận chưa đóng sổ mang dấu *"trận chưa đóng sổ"*; và **100%** số yêu cầu xuất gói rút gọn từ trận chưa đóng sổ bị từ chối **kèm lý do**, với **0** thay đổi dữ liệu ở trận đó.
- **SC-010**: Biên bản, gói kết quả và nhật ký sự kiện, và gói rút gọn của cùng một trận cho ra **cùng một** bảng điểm cuối và **cùng một** thứ hạng ở **100%** số trận thử — thử ít nhất **10** trận gồm cả trận có vòng bị bỏ, trận có vòng chạy lại, trận có tie-break và trận nhãn `bỏ dở`.
- **SC-011**: **100%** số câu bị sửa nội dung sau trận vẫn in ra **nội dung đã lên sóng** trong biên bản của trận cũ — thử **20** câu, gồm cả câu bị **xoá mềm** sau trận.
- **SC-012**: **100%** số hiện vật đã xuất còn nguyên sau khi bước dọn dữ liệu chạy trên một trận đã quá hạn lưu trữ; và bước dọn phát cảnh báo **trước** khi bắt đầu ở **100%** số lần chạy.
- **SC-013**: **0** câu **đã rút nhưng chưa hiển thị** nhận số liệu sử dụng — thử một trận trong đó admin bấm rút nhiều lần trước khi hiển thị, ở cả năm vòng.
- **SC-014**: Cả **hai** loại gói của epic này xuất được trên hồ sơ portable **đã ngắt Internet**, ở **100%** số lần thử.
- **SC-015**: **0** phiên thiếu `results.export` phân biệt được nhau qua phản hồi từ chối — thử **cả bốn** tổ hợp *(có / không quyền đọc kho đề)* × *(có / không trận đang chạy)*, đối chiếu nội dung, mã lỗi và thời gian phản hồi; và **0** byte nội dung gói rời server ở cả bốn.
- **SC-016**: **100%** số câu đã hiển thị trong một trận nhãn **`bỏ dở`** đều nhận đủ **ba nhóm trường** số liệu — thử **20** câu trải trên ít nhất hai vòng đã chạy trước lúc huỷ trận.
- **SC-024**: Trên một bản cài **v1**, **100%** số câu đã dùng đều hiện **đủ hai bộ** số ở kho đề với bộ `practice` mang giá trị rỗng — thử **20** câu; và **0** bộ nào bị ẩn, bị gộp, hay đòi một thay đổi bề mặt để bắt đầu có số ở v1.5.
- **SC-017**: **0** lần số liệu của trận `official` và trận `practice` bị cộng vào nhau — thử **20** câu được hỏi ở **cả hai** loại trận trong cùng contest, và với mỗi câu xác minh hai bộ số đọc riêng được và tổng của chúng **không** xuất hiện ở bất kỳ đâu trên bề mặt kho đề.
- **SC-019**: **0** lần tính lại số liệu thống kê cho ra kết quả khác lần trước trên cùng một tập trận — chạy lại **10** lần trên một kho `60` câu đã dùng qua ba contest; và **0** dấu chống trùng nào cần tồn tại để đạt kết quả đó.
- **SC-020**: **100%** số lần gọi xuất — thành công, bị từ chối, hay thất bại — đều để lại **đúng một** dòng nhật ký mang kết quả; thử **30** lần gọi trộn cả ba outcome trên cả ba loại hiện vật.
- **SC-021**: **100%** số lần gọi xuất **lại** ngay sau một lần thất bại đều thành công mà **không** cần thao tác dọn dẹp nào; thử **10** cặp, ngắt ở các mốc khác nhau trong quá trình xuất.
- **SC-022**: Biên bản xuất được trên hồ sơ portable **đã ngắt Internet** ở **100%** số lần thử, với nội dung đầy đủ như trên hồ sơ máy chủ.
- **SC-023**: **0** phiên thiếu `results.export` phân biệt được trạng thái trận qua phản hồi từ chối của **gói kết quả rút gọn** — thử **cả bốn** tổ hợp *(có / không quyền)* × *(trận đã đóng sổ / đang chạy)*, đối chiếu nội dung, mã lỗi và thời gian phản hồi; và **100%** số phiên **có** quyền bị từ chối vì trận chưa đóng sổ đều nhận **lý do nêu rõ** điều đó.
- **SC-018**: **100%** số gói kết quả rút gọn xuất từ trận nhãn `bỏ dở` đều mang **nhãn `bỏ dở`**, **lý do đóng sổ**, và **hai trường trống** ở thứ hạng và người thắng — thử **10** trận huỷ ở các vòng khác nhau; và **0** gói nào chứa một thứ hạng hay một người thắng được suy ra.

## Out of Scope

Danh sách này ghi rõ những gì **không** thuộc feature này, và spec nào sở hữu chúng. Requirement nào nằm ở đây thì spec này **không** đặc tả lại, kể cả khi nó xuất hiện trong một acceptance scenario với vai trò tiền đề.

**Ghi rõ trong PRD**

- **Phát lại một trận** — `docs/PRD.md` §11 EPIC-010 §Out of scope, §20 *"trừ phát lại"*. Không thuộc v1, không thuộc bất kỳ spec nào hiện có.

**Thuộc epic khác**

- **Cú bấm Chốt trận, phép phân định hoà, vòng Câu hỏi phụ, mốc niêm phong** — EPIC-005, EPIC-006 (`specs/005`, `specs/006`). `GR-022` C1, C2, C6, C8 và `GR-023`, `GR-025` C1 → C4 thuộc về đó; feature này chỉ đọc **kết quả** của chúng.
- **Cú bấm bỏ vòng, chạy lại vòng, kết thúc sớm, huỷ trận, chỉnh điểm tay** — EPIC-007 (`specs/007`). Feature này đặc tả **những gì các cú bấm đó để lại trong biên bản**, không đặc tả chính các cú bấm, dialog phá huỷ hay ô nhập lý do của chúng.
- **Mô hình `điểm = reduce(event log)` và cơ chế hoàn nguyên** — EPIC-006 (`specs/006`). Feature này dựa vào tính **tất định** của phép tính đó nhưng không hiện thực lại nó.
- **Gói contest X1 và bản kê câu đã dùng X3**, gồm cả nhập X3 và đánh dấu *đã dùng* hàng loạt bằng tay — EPIC-003 (`specs/003`). `PRD-REQ-094`, `PRD-REQ-095` không thuộc feature này.
- **Bảng `AuditLog` chung, MÀN ĐỌC nhật ký thao tác, và job dọn dữ liệu tự động** — EPIC-011 (`specs/011`). `PRD-REQ-082`, `PRD-REQ-083` không thuộc feature này; feature này chỉ đặt **hai ràng buộc lên bước dọn** (`PRD-REQ-080`) và **ghi vào** nhật ký đã có. **Màn đọc nhật ký thao tác vào v1 và do EPIC-011 sở hữu** (`QĐ-130`) — cùng chủ với bảng, để mọi thay đổi lược đồ không phải qua hai spec. `NFR-17` là yêu cầu về **nội dung** nhật ký; nó không đòi EPIC-010 có màn đọc.
- **Vòng đời dữ liệu câu hỏi** — mã hiển thị duy nhất, xoá mềm, đánh phiên bản theo cú bấm Save — EPIC-002 (`specs/002`); vế *"trận ghi phiên bản nào đã dùng"* thuộc EPIC-006 (`specs/006`). Feature này chỉ đặc tả **hệ quả ở biên bản** của `PRD-REQ-115`.
- **Lớp phủ công bố kết quả trên hai kênh public** — EPIC-009 (`specs/009`). `STATE-008` §Cho phép nhắc *"hệ thống gợi ý mở công bố tại đây"*; bản thân lớp công bố, thứ tự chồng lớp và banner tạm dừng đều thuộc spec đó.
- **Tạo trận mới trong cùng contest** (`EVENT-036`, `T-021`, `T-022`) — EPIC-005 (`specs/005`). Nó là thao tác **cấp contest** và không đụng trận đã đóng sổ.
- **Hai hồ sơ triển khai và giới hạn media** — EPIC-012. Feature này chỉ đặc tả ràng buộc *"hai gói của epic này có ở cả hai hồ sơ"* (`PRD-REQ-098`).
- **Catalog permission và cửa kiểm quyền** — EPIC-001 (`specs/001`). Feature này **dùng** `PERM-020` và `PERM-015` nhưng không định nghĩa chúng.

**Không thuộc v1**

- Màn **so sánh diff** và thao tác **quay về bản cũ** của phiên bản câu hỏi (`PRD-REQ-115` vế cuối).
- **Đồng bộ kết quả** từ hồ sơ portable về máy chủ trung tâm — `QĐ-084` khai đây là **ranh giới chấp nhận có chủ đích**, không phải chỗ thiếu đặc tả.

## Traceability Matrix

| User story | PRD requirement | Game rules | Functional requirements | Acceptance scenarios |
|---|---|---|---|---|
| US-001 | PRD-REQ-079, PRD-REQ-097 | `GR-022` C3, C4, C5, C7, §Còn hiệu lực · `GR-025` C5 · `GR-028` §Điều kiện, C5, §Biên, §Không phải mọi event · `GR-029` C6 · `STATE-008` · `INV-001`, `INV-018` · `NFR-12` | FR-001 → FR-010 | AC-001 → AC-010 |
| US-002 | PRD-REQ-079, PRD-REQ-115, PRD-REQ-098 | `GR-028` C1 → C4, §Không đổi gì, §Bấm trùng · `GR-030` C1, C2, C3, C5, §Không đổi gì, §Biên · `GR-029` C1, C3, C4, C5 · `GR-022` C5 · `T-012`, `T-013`, `T-014` · `INV-001`, `INV-022` · `NFR-14`, `NFR-16`, `NFR-17`, `NFR-38` | FR-011 → FR-024 **+ FR-011a, FR-013a, FR-021** | AC-011 → AC-024 **+ AC-012a, AC-038a** |
| US-003 | PRD-REQ-096, PRD-REQ-098 | `GR-028` C1 → C5, §Điều kiện, §Bấm trùng · `GR-030` C1, C2, C3 · `GR-037` §Cấm, §Thứ tự đánh giá · `INV-001` · `NFR-12`, `NFR-14`, `NFR-17` · `PERM-020` | FR-025 → FR-038 **+ FR-038a, FR-038b, FR-021, FR-034** | AC-025 → AC-039 **+ AC-029a** |
| US-004 | PRD-REQ-097, PRD-REQ-098 | `GR-022` C3, C4, C5, C7 · `GR-025` C5 · `GR-028` §Biên · `GR-037` §Thứ tự đánh giá · `STATE-008` §Mô tả, §Trường đi kèm, §Khi `bỏ dở` · `PERM-020` | FR-039 → FR-045 **+ FR-041a, FR-041b, FR-034** | AC-040 → AC-045 **+ AC-042a, AC-042b, AC-035, AC-038** |
| US-005 | PRD-REQ-081 | `GR-031` C1, C9, §Ranh giới *"đã dùng"*, §Không đổi gì · `GR-022` C5 · `GR-026` · `GR-033` · `GR-037` §Cấm · `STATE-008` §Khi `bỏ dở` · `TERM-015`, `TERM-048` | FR-046 → FR-051 **+ FR-046a, FR-046b, FR-046c, FR-046d, FR-046e** | AC-046 → AC-051 **+ AC-046a, AC-046b, AC-046c** |
| US-006 | PRD-REQ-080 | — *(requirement khai **Related game rules: —**)* · `NFR-14`, `NFR-35`, `NFR-35b` · `QĐ-077`, `QĐ-040`, `QĐ-091` | FR-052 → FR-055 **+ FR-055a, FR-055b** | AC-052 → AC-054 **+ AC-054a** |
| *(xuyên suốt)* | `NFR-36` → `NFR-39` · `CLAUDE.md` §UX, §Quy ước code | — | FR-056 → FR-060 | AC-055 → AC-058 |

### Bản đồ phủ bảng quyết định

> Mỗi outcome trong bảng quyết định của từng `GR-NNN` **thuộc phạm vi feature này** đều có ít nhất một acceptance scenario. Ca nào thuộc epic khác thì ghi rõ spec sở hữu, theo đúng cách `specs/006` → `specs/009` đã dùng — chúng **không** được đặc tả lại ở đây.

**Rule mà EPIC-010 khai trực tiếp**

| Game rule | Ca trong bảng quyết định | Acceptance scenarios |
|---|---|---|
| `GR-022` | C1 *(vào `TIE_BREAK`, hoà vị trí nhất)* | *(EPIC-005, EPIC-006 — `specs/005`, `specs/006`)*; mặt sau trận: AC-032 *(gói mang dấu chưa đóng sổ)* |
| `GR-022` | C2 *(hoà vị trí khác, vẫn ∈ config)* | *(EPIC-005, EPIC-006)* — v1 khoá cứng `tieBreakPositions = [1]` nên ca này không dựng được ở v1 |
| `GR-022` | C3 *(một người điểm cao nhất ⇒ `hoàn thành`)* | AC-001, AC-003, AC-040 |
| `GR-022` | C4 *(hoà ngoài phạm vi ⇒ đồng hạng, hạng kế nhảy qua)* | AC-004, AC-044 |
| `GR-022` | C5 *(huỷ trận ⇒ nhãn `bỏ dở`, không thứ hạng)* | AC-005, AC-018, AC-042a, AC-046a |
| `GR-022` | C6 *(đứng ở `LOBBY`, chưa bấm gì)* | *(EPIC-005)*; mặt sau trận: AC-032 |
| `GR-022` | C7 *(chốt lần hai, tie-break còn hiệu lực ⇒ đóng sổ)* | AC-003 |
| `GR-022` | C8 *(chốt lần hai, nhóm hoà khác đi ⇒ event mất đối tượng)* | AC-010 |
| `GR-028` | C1 *(chấm Đúng ⇒ `+10`)* | AC-011, AC-025 |
| `GR-028` | C2 *(chấm Sai ⇒ `−5`)* | AC-011, AC-025 |
| `GR-028` | C3 *(`SCORE_ADJUST` `+3` kèm lý do)* | AC-016 |
| `GR-028` | C4 *(bỏ vòng ⇒ N event đảo ngược)* | AC-013, AC-015, AC-026 |
| `GR-028` | C5 *(hỏi điểm ở một mốc ⇒ không sinh event)* | AC-009, AC-033, AC-037 |
| `GR-028` | §Bấm trùng *(`reduce` cùng mảng cho cùng kết quả)* | AC-022, AC-036 |

**Rule mà requirement của epic này tham chiếu**

| Game rule | Ca trong bảng quyết định | Acceptance scenarios |
|---|---|---|
| `GR-030` | C1 *(bỏ vòng ⇒ cách rời "đã bỏ")* | AC-013, AC-012a |
| `GR-030` | C2 *(chạy lại vòng ⇒ nhiều khối, hai trường tách bạch)* | AC-012, AC-012a |
| `GR-030` | C3 *(kết thúc sớm ⇒ điểm giữ nguyên, cách rời "kết thúc sớm")* | AC-014, AC-012a |
| `GR-030` | C4 *(không có đường vòng → vòng)* | *(EPIC-007 — `specs/007`)*; không sinh khối biên bản mới nên không có mặt sau trận |
| `GR-030` | C5 *(bỏ vòng lần hai bị từ chối)* | AC-021 *(biên bản không sinh khối thứ hai)* |
| `GR-029` | C1, C2 *(delta dương, delta âm)* | AC-016 |
| `GR-029` | C3 *(`delta = 0` vẫn sinh event)* | AC-016 |
| `GR-029` | C4, C5 *(điều chỉnh không bị cuốn theo bỏ vòng)* | AC-017 |
| `GR-029` | C6 *(trận `FINISHED` ⇒ không thực hiện được)* | AC-007, AC-008 |
| `GR-031` | C1 *(rút, sinh `QUESTIONS_DRAWN`, đặt cờ khi hiển thị)* | AC-047, AC-048 |
| `GR-031` | C2, C3, C3b, C3c, C4, C5, C6, C7, C8 | *(EPIC-002, EPIC-005, EPIC-006 — `specs/002`, `specs/005`, `specs/006`)* |
| `GR-031` | C9 *(practice đảo chiều phép lọc)* | AC-046b |
| `GR-025` | C5 *(không phân định ⇒ đồng hạng)* | AC-004, AC-044 |
| `GR-025` | C1 → C4 *(bốc thăm, bốc lại, thiếu câu)* | *(EPIC-006 — `specs/006`)* |
| `GR-037` | C1 → C10 *(phạm vi hiển thị đáp án trong trận)* | *(EPIC-006, EPIC-007, EPIC-009 — `specs/006`, `specs/007`, `specs/009`)* |
| `GR-037` | §Cấm *(cấm đẩy đáp án tới nơi không được phép)* | AC-029, AC-030, AC-041, AC-051 |

**Trạng thái, event và bất biến**

| Nguồn | Nội dung được phủ | Acceptance scenarios |
|---|---|---|
| `STATE-008` §Cho phép, §Cấm, §Ra | chỉ đọc · xem lại biên bản và toàn bộ lịch sử · terminal, không đường ra | AC-001, AC-007, AC-008 |
| `STATE-008` §Trường đi kèm | `matchClosedReason` · `closedBy` · `closedAt` · `reason` bắt buộc khi `bỏ dở` | AC-005, AC-006 |
| `STATE-008` §Khi `bỏ dở` | điểm giữ nguyên · không thứ hạng, không người thắng · thống kê trận không đếm, nhưng **số liệu của câu VẪN ghi** | AC-005, AC-018, AC-042a, AC-046a |
| §*Invalid transitions* cho `STATE-008` | `EVENT-028` chỉnh điểm ⇒ toast · `EVENT-005` / `EVENT-006` ⇒ nút không tồn tại · `EVENT-030` ⇒ nút không bật · `EVENT-037` tín hiệu ⇒ ghi lịch sử, không có hiệu lực | AC-007, AC-008 |
| `INV-001`, `INV-018`, `INV-022` | lịch sử chỉ thêm, không bao giờ mất · điểm được phép âm · trạng thái trận nằm ở hai thứ | AC-010, AC-015 · AC-002 · AC-020, AC-037 |
| `PERM-020`, `PERM-015` | `results.export` gác cả ba hiện vật, là **cửa từ chối duy nhất** của gói kết quả và nhật ký sự kiện, và là **cửa kiểm ĐẦU TIÊN** của gói rút gọn *(gói rút gọn có cửa thứ hai — trạng thái trận)* · `audit.read` gác **màn đọc thuộc EPIC-011**, ngoài phạm vi feature này | AC-029a, AC-039, AC-042b, AC-045, AC-054a · *(EPIC-011)* |
| `QĐ-077`, `QĐ-084`, `QĐ-107` | biên bản theo lần chạy, **hai trường tách bạch** · bốn gói một nguồn, ba biện pháp gác · phiên bản câu đã dùng | AC-012, AC-012a · AC-021, AC-035 · AC-019, AC-020 |
| `TERM-015`, `TERM-048` | `matchPurpose` tách hai bộ số thống kê · cờ đã dùng một chiều, nên câu ở trận `bỏ dở` vẫn cần phản hồi | AC-046b · AC-046a, AC-050 |
| `NFR-36` → `NFR-39` | trạng thái đang xử lý · một khung nhìn · tiếng Việt và UTC+7 · nút quay lại | AC-055 · AC-056 · AC-057 · AC-058 |

### Acceptance Scenarios (chi tiết)

> Mọi kịch bản đều giả định **một** phiên admin đang giữ quyền điều khiển (`GR-026` §Đồng thời) và mọi mốc thời gian đều là **server time** (`INV-004`). Trừ khi nói khác, trận là trận **chính thức**, mode **sân khấu**, **bốn ghế** đều hoạt động, và trận chạy **trên cùng bản cài** với kho đề.

#### Nhóm A — Kết quả đã niêm phong (US-001)

**AC-001** — *US*: US-001 · *FR*: FR-001, FR-003 · *GR*: `GR-022` C3 · `STATE-008`
**Given** một trận bốn ghế A, B, C, D đã chạy trọn năm vòng và đã được admin bấm Chốt trận với điểm cuối `A=110 · B=100 · C=90 · D=85`, trận đang ở `STATE-008` nhãn `hoàn thành`, và admin đang mở màn kết quả của trận đó,
**When** admin đọc bảng kết quả cuối,
**Then** bảng hiện **đủ bốn** ghế với đúng bốn con số trên; thứ hạng hiện `A` hạng 1, `B` hạng 2, `C` hạng 3, `D` hạng 4; người thắng hiện là `A`; và **0** event được sinh trong nhật ký sự kiện của trận.

**AC-002** — *US*: US-001 · *FR*: FR-002 · *GR*: `GR-028` §Biên · `INV-018`
**Given** một trận đã đóng sổ nhãn `hoàn thành` với điểm cuối `A=40 · B=0 · C=−5 · D=−100`,
**When** admin đọc bảng kết quả cuối,
**Then** ba giá trị âm hiện **nguyên giá trị** `−5` và `−100`; hệ thống **không** kẹp về `0`, **không** ẩn, và **không** đổi dấu; và thứ hạng xếp `A` hạng 1, `B` hạng 2, `C` hạng 3, `D` hạng 4 — điểm âm tham gia xếp hạng bình thường.

**AC-003** — *US*: US-001 · *FR*: FR-003 · *GR*: `GR-022` C7 · `EVENT-048`
**Given** một trận trong đó `A=100` và `B=100` đã vào vòng Câu hỏi phụ, `A` thắng phân định, trận đã về `LOBBY` với một `TIE_BREAK_RESOLVED` còn hiệu lực, điểm **không** đổi sau đó, và admin đã bấm Chốt trận **lần hai** đưa trận về `STATE-008` nhãn `hoàn thành`,
**When** admin đọc bảng kết quả cuối,
**Then** `A` hiện hạng 1 và là người thắng, `B` hiện hạng 2; điểm của cả hai vẫn là `100`; và bảng **không** hiện hai người đồng hạng 1 — thứ hạng đã được phân định bởi vòng Câu hỏi phụ, không bởi điểm.

**AC-004** — *US*: US-001 · *FR*: FR-004 · *GR*: `GR-022` C4, `GR-025` C5
**Given** một trận đã đóng sổ nhãn `hoàn thành` với điểm cuối `A=110 · B=100 · C=100 · D=85`, `tieBreakPositions = [1]`, và nhóm hoà nằm ở **vị trí nhì** tức ngoài phạm vi phân định,
**When** admin đọc bảng kết quả cuối,
**Then** `A` hiện hạng 1; `B` và `C` hiện **đồng hạng nhì**; `D` hiện **hạng tư**; **không** có dòng nào mang hạng ba; và người thắng hiện là `A`.

**AC-005** — *US*: US-001 · *FR*: FR-005, FR-006 · *GR*: `GR-022` C5 · `STATE-008` §Khi `bỏ dở`
**Given** một trận đang chạy giữa vòng Tăng tốc với điểm `A=60 · B=45 · C=45 · D=20` thì mất điện hội trường, và admin đã bấm **Huỷ trận** với lý do *"mất điện hội trường"*, đưa trận về `STATE-008` nhãn `bỏ dở`,
**When** admin mở màn kết quả của trận đó,
**Then** bảng hiện điểm **tại mốc đóng** đúng bốn con số trên, **không** hoàn nguyên gì; hệ thống **không** hiện thứ hạng và **không** hiện người thắng; nhãn đóng sổ hiện `bỏ dở`; lý do *"mất điện hội trường"* hiện đầy đủ; và **0** event điểm được sinh bởi thao tác đọc này.

**AC-006** — *US*: US-001 · *FR*: FR-006 · `STATE-008` §Trường đi kèm
**Given** một trận đã đóng sổ nhãn `hoàn thành` bởi tài khoản admin `bt-01` lúc `20:47:12` giờ UTC+7,
**When** admin đọc màn kết quả,
**Then** màn hiện nhãn đóng sổ `hoàn thành`, người đóng sổ `bt-01`, và thời điểm `20:47:12` theo **UTC+7**; trường lý do **không** bắt buộc ở nhãn này nên được phép trống; và mọi mốc thời gian khác trên màn cũng hiển thị theo UTC+7.

**AC-007** — *US*: US-001 · *FR*: FR-007, FR-008 · *GR*: `GR-029` C6 · §Invalid transitions
**Given** một trận đã ở `STATE-008` nhãn `hoàn thành` với điểm cuối `A=110 · B=100 · C=90 · D=85`, và admin đang mở màn kết quả,
**When** admin tìm thao tác **chỉnh điểm tay** cho ghế `A`,
**Then** nút chỉnh điểm **không bật**; nếu lệnh vẫn tới được server thì server **từ chối** và admin thấy phản hồi *invalid state*; **không ép được**; và điểm bốn ghế **không đổi** — vẫn đúng bốn con số ban đầu, **0** event mới trong nhật ký.

**AC-008** — *US*: US-001 · *FR*: FR-007, FR-008 · *GR*: `GR-030` · §Invalid transitions
**Given** cùng trận của AC-007, trong đó vòng VCNV đã chạy và đã sinh sáu event điểm,
**When** admin gửi thẳng tới server một lệnh **bỏ vòng** VCNV, rồi một lệnh **chạy lại vòng** VCNV, rồi một lệnh **vô hiệu hoá ghế** `C`,
**Then** **cả ba** lệnh bị từ chối ở server; **0** event đảo ngược được sinh; điểm bốn ghế **không đổi**; trạng thái trận vẫn là `STATE-008` nhãn `hoàn thành`; và mỗi lần từ chối để lại một dòng nhật ký thao tác.

**AC-009** — *US*: US-001 · *FR*: FR-009 · *GR*: `GR-028` C5
**Given** một trận đã đóng sổ với nhật ký sự kiện gồm đúng `47` event,
**When** admin mở màn kết quả **năm lần liên tiếp** và mỗi lần đọc bảng điểm cuối cùng thứ hạng,
**Then** nhật ký sự kiện của trận vẫn có đúng `47` event sau cả năm lần; **0** event mới được sinh; và cả năm lần đọc cho ra **cùng một** bảng điểm và **cùng một** thứ hạng.

**AC-010** — *US*: US-001 · *FR*: FR-010 · *GR*: `GR-022` §Còn hiệu lực, C8, `GR-028` §Không phải mọi event
**Given** một trận trong đó `A=100` và `B=100` đã vào vòng Câu hỏi phụ, `A` thắng và sinh một `TIE_BREAK_RESOLVED` cho nhóm `{A, B}`; sau đó ở `LOBBY` admin chỉnh `A` từ `100` lên `110`, làm nhóm `{A, B}` tan; admin bấm Chốt trận lần hai và trận về `STATE-008` nhãn `hoàn thành`,
**When** admin đọc bảng kết quả cuối và mở nhật ký sự kiện của trận,
**Then** `A` hiện hạng 1 **theo điểm** `110` và `B` hạng 2 với `100`; `TIE_BREAK_RESOLVED` vẫn **còn nguyên** trong nhật ký, **không** bị xoá và **không** bị đánh dấu vô hiệu; nó **không** tham gia phép sắp thứ hạng; và điểm của `B` **không đổi**.

#### Nhóm B — Biên bản theo lần chạy (US-002)

**AC-011** — *US*: US-002 · *FR*: FR-011 · *GR*: `GR-028` C1, C2 · `NFR-16`
**Given** một trận đã đóng sổ nhãn `hoàn thành`, đã chạy đủ năm vòng, nhật ký sự kiện chứa cả event chấm **Đúng** `+10` lẫn event chấm **Sai** `−5`,
**When** admin xuất biên bản của trận đó,
**Then** biên bản chứa **mọi vòng đã chạy**; mỗi event điểm hiện thành một dòng đọc được với ghế, loại phán quyết và delta; bảng điểm cuối trong biên bản trùng khớp bảng ở màn kết quả; và trạng thái trận **không đổi**.

**AC-012** — *US*: US-002 · *FR*: FR-012, FR-013 · *GR*: `GR-030` C2 · `QĐ-077`, `NFR-16`
**Given** một trận trong đó vòng **Tăng tốc** đã chạy tới câu thứ hai thì admin bấm **Bỏ vòng** với lý do *"lỗi máy chiếu"*, rồi bấm **Chạy lại vòng** và chạy trọn vòng đó bằng bốn câu khác; trận sau đó đã đóng sổ,
**When** admin xuất biên bản,
**Then** vòng Tăng tốc hiện thành **hai khối riêng**, xếp theo **thứ tự thời gian**; khối thứ nhất mang *lần chạy* `1` và *cách rời* **`đã bỏ`**; khối thứ hai mang *lần chạy* `2` và *cách rời* **`hoàn thành`**; hệ thống **không** gộp hai khối và **không** giấu lần hỏng; và hai câu đã hiển thị ở lần chạy thứ nhất vẫn xuất hiện trong khối thứ nhất.

**AC-012a** — *US*: US-002 · *FR*: FR-013, FR-013a · *GR*: `GR-030` §Biên · `QĐ-077`
**Given** một trận trong đó vòng **VCNV** đi qua chuỗi: lần chạy 1 bị **bỏ**, lần chạy 2 bị admin bấm **chạy lại** giữa chừng, lần chạy 3 **kết thúc sớm**, lần chạy 4 chạy trọn; trận sau đó đã đóng sổ,
**When** admin xuất biên bản,
**Then** vòng VCNV hiện thành **bốn khối**; trường *lần chạy* nhận đúng các giá trị `1 · 2 · 3 · 4`, **không** giá trị nào lặp; trường *cách rời* nhận đúng `đã bỏ · đã chạy lại · kết thúc sớm · hoàn thành`; hai trường hiện **tách bạch**, không trường nào phải suy ra từ trường kia; và khối 1 phân biệt được với khối 3 dù cả hai đều là lần chạy hỏng.

**AC-013** — *US*: US-002 · *FR*: FR-013, FR-014 · *GR*: `GR-030` C1, `GR-028` C4
**Given** một trận trong đó vòng **Khởi động** đã cộng `30` điểm cho ghế `A` rồi admin bấm **Bỏ vòng** với lý do *"đọc nhầm bộ câu"*, và trận sau đó đã đóng sổ,
**When** admin xuất biên bản,
**Then** khối của vòng Khởi động mang nhãn ***đã bỏ***; khối đó chứa **cả** các event điểm gốc **và** các event đảo ngược tương ứng; điểm cuối của `A` trong biên bản phản ánh phần đã hoàn nguyên; và **không** event nào biến mất khỏi biên bản.

**AC-014** — *US*: US-002 · *FR*: FR-013, FR-015 · *GR*: `GR-030` C3
**Given** một trận trong đó vòng **Về đích** đang chạy được hai lượt thì admin bấm **Kết thúc sớm** với lý do *"quá giờ phát sóng"*, điểm của hai lượt đã chạy được **giữ nguyên**, và trận sau đó đã đóng sổ,
**When** admin xuất biên bản,
**Then** khối của vòng Về đích mang nhãn ***kết thúc sớm***; biên bản **không** chứa event đảo ngược nào sinh bởi cửa ra này; điểm hai lượt đã chạy hiện nguyên giá trị; và câu đang mở lúc đó hiện với phán quyết **Huỷ kết quả**, không sinh điểm cho ai.

**AC-015** — *US*: US-002 · *FR*: FR-016 · *GR*: `GR-028` §Điều kiện · `QĐ-077`, `INV-001`
**Given** cùng trận của AC-013, trong đó cú bỏ vòng do admin `bt-02` bấm lúc `19:12:40` UTC+7 với lý do *"đọc nhầm bộ câu"*,
**When** admin xuất biên bản và rà các event đảo ngược,
**Then** **mỗi** event đảo ngược hiện thành một **dòng riêng** kèm tên người bấm `bt-02`, lý do *"đọc nhầm bộ câu"* và thời điểm `19:12:40` UTC+7; hệ thống **không** thể hiện hoàn nguyên bằng một phép trừ thầm lặng; và event gốc tương ứng vẫn còn nguyên dòng của nó.

**AC-016** — *US*: US-002 · *FR*: FR-017 · *GR*: `GR-029` C1, C2, C3 · `TERM-024`
**Given** một trận đã đóng sổ trong đó admin đã ba lần chỉnh điểm tay: `+5` cho `A` lý do *"sửa nhầm câu 2"*, `−5` cho `C` lý do *"MC yêu cầu trừ thêm"*, và `0` cho `B` lý do *"ghi nhận khiếu nại của thí sinh B, MC đã phán quyết giữ nguyên"*,
**When** admin xuất biên bản,
**Then** biên bản có **đúng ba** dòng chỉnh điểm tay; mỗi dòng mang ghế, delta, lý do, người bấm và thời điểm; dòng `delta = 0` **vẫn có mặt** với đầy đủ lý do; và **không** dòng nào bị gộp vào một dòng khác.

**AC-017** — *US*: US-002 · *FR*: FR-018 · *GR*: `GR-029` C4, `GR-030` §Không đổi gì
**Given** một trận trong đó admin chỉnh tay `+5` cho ghế `A`, **sau đó** bỏ vòng Khởi động vốn đã cộng `30` cho `A`; trận sau đó đã đóng sổ,
**When** admin xuất biên bản,
**Then** dòng chỉnh tay `+5` **vẫn còn** trong biên bản và **không** bị đảo ngược; khối vòng Khởi động mang nhãn *đã bỏ* và chứa event đảo ngược cho `−30`; và điểm cuối của `A` trong biên bản phản ánh đúng **cả hai** — phần chỉnh tay giữ, phần vòng hoàn nguyên.

**AC-018** — *US*: US-002 · *FR*: FR-019 · *GR*: `GR-022` C5 · `STATE-008` §Khi `bỏ dở`
**Given** cùng trận của AC-005 — huỷ giữa vòng Tăng tốc, nhãn `bỏ dở`, lý do *"mất điện hội trường"*, ba vòng trước đó đã chạy xong,
**When** admin xuất biên bản,
**Then** biên bản in **đủ ba vòng đã chạy** cộng phần Tăng tốc dở dang; nó mang nhãn *trận bỏ dở* và in lý do; nó in điểm **tại mốc đóng**; và nó **không** in thứ hạng, **không** in người thắng.

**AC-019** — *US*: US-002 · *FR*: FR-020 · PRD-REQ-115 · `QĐ-107`
**Given** một câu hỏi `Q-138` đã hiển thị trong trận A với nội dung *"Thủ đô của Australia là thành phố nào?"*; sau khi trận A đóng sổ, người ra đề sửa câu đó thành *"Thành phố nào là thủ đô của Australia?"* và bấm Save, sinh một phiên bản mới,
**When** admin xuất biên bản của trận A,
**Then** biên bản in **nội dung đã lên sóng** — bản trước khi sửa; kho đề vẫn hiện **bản mới**; và điểm cùng thứ hạng của trận A **không đổi**.

**AC-020** — *US*: US-002 · *FR*: FR-020 · PRD-REQ-115 · `INV-022`
**Given** cùng câu `Q-138` của AC-019, nay bị **xoá mềm** khỏi kho đề,
**When** admin xuất biên bản của trận A,
**Then** biên bản vẫn in đủ nội dung câu đó theo phiên bản đã lên sóng; tham chiếu từ nhật ký sự kiện của trận A **không** mất; và biên bản **không** chứa chỗ trống hay lỗi tham chiếu nào.

**AC-021** — *US*: US-002 · *FR*: FR-021 · *GR*: `GR-030` C5 · `QĐ-084` §Một nguồn
**Given** một trận trong đó admin bấm **Bỏ vòng** cho vòng VCNV, dialog xác nhận đã qua, và ngay sau đó một lệnh bỏ vòng **thứ hai** cho cùng vòng đó tới server do trễ mạng; trận sau đó đã đóng sổ,
**When** admin xuất biên bản và xuất gói kết quả cùng nhật ký sự kiện của contest,
**Then** biên bản có **đúng một** khối *đã bỏ* cho vòng VCNV, **không** khối thứ hai; điểm **không** tụt gấp đôi; và bảng điểm cuối trong biên bản **trùng khớp từng con số** với bảng trong gói — hai hiện vật sinh từ cùng một nguồn.

**AC-022** — *US*: US-002 · *FR*: FR-022 · *GR*: `GR-028` §Bấm trùng, C5
**Given** một trận đã đóng sổ với nhật ký sự kiện gồm đúng `52` event,
**When** admin xuất biên bản **ba lần liên tiếp** mà không thao tác gì khác giữa các lần,
**Then** ba biên bản có **nội dung giống hệt nhau**; nhật ký sự kiện vẫn có đúng `52` event; **0** event mới được sinh; và trạng thái trận vẫn là `STATE-008`.

**AC-023** — *US*: US-002 · *FR*: FR-023 · `NFR-14`, `PERM-020`
**Given** một trận đã đóng sổ và tài khoản `bt-03` mang permission `results.export`,
**When** `bt-03` xuất biên bản của trận đó,
**Then** nhật ký thao tác nhận **đúng một** dòng mới, gồm người thực hiện `bt-03`, hành động *xuất biên bản*, đối tượng là trận đó, thời điểm theo UTC+7, và địa chỉ nguồn; và nhật ký sự kiện của **trận** không nhận dòng nào.

**AC-024** — *US*: US-002 · *FR*: FR-024 · `NFR-38`
**Given** một trận đã đóng sổ với các mốc thời gian được ghi ở database theo UTC,
**When** admin xuất biên bản và rà toàn bộ nội dung,
**Then** **100%** mốc thời gian trong biên bản hiển thị theo **UTC+7**; **100%** chuỗi trong biên bản là **tiếng Việt**; và **không** tồn tại thiết lập múi giờ theo người dùng ảnh hưởng tới kết quả.

#### Nhóm C — Gói kết quả và nhật ký sự kiện (US-003)

**AC-025** — *US*: US-003 · *FR*: FR-025, FR-026 · *GR*: `GR-028` C1, C2 · `QĐ-084` X2
**Given** một contest đã chạy **hai** trận, cả hai đã đóng sổ, nhật ký sự kiện của hai trận gồm tổng cộng `95` event trong đó có cả chấm Đúng, chấm Sai và chỉnh điểm tay,
**When** admin xuất gói kết quả và nhật ký sự kiện của contest,
**Then** gói chứa **cả hai** trận; với từng trận nó chứa nhật ký sự kiện đầy đủ, điểm, thứ hạng, và các sự kiện hoàn nguyên kèm người bấm và lý do; tổng số event trong gói bằng `95`; và **0** event bị lược.

**AC-026** — *US*: US-003 · *FR*: FR-026 · *GR*: `GR-028` C4
**Given** cùng contest của AC-025, trong đó trận thứ nhất có một vòng bị bỏ, sinh bốn event đảo ngược do admin `bt-02` bấm kèm lý do,
**When** admin xuất gói,
**Then** gói chứa **cả bốn** event đảo ngược; mỗi event mang **người bấm** và **lý do**; các event điểm gốc cũng còn nguyên; và gói **không** thể hiện hoàn nguyên bằng một con số điểm đã bị sửa tại chỗ.

**AC-027** — *US*: US-003 · *FR*: FR-027 · *GR*: `GR-030` C1, C2
**Given** cùng contest của AC-025, trong đó trận thứ nhất có vòng Tăng tốc bị **bỏ** rồi **chạy lại** như AC-012,
**When** admin xuất gói,
**Then** gói chứa **đủ hai lần chạy** của vòng đó, phân biệt được với nhau; và tập lần chạy trong gói **trùng khớp** với tập khối trong biên bản của cùng trận.

**AC-028** — *US*: US-003 · *FR*: FR-028 · `QĐ-084` §Ranh giới đáp án
**Given** một contest đã dùng `60` câu hỏi qua hai trận, và admin thực hiện xuất bằng một tài khoản **có** quyền đọc kho đề,
**When** admin xuất gói và rà cách gói tham chiếu câu hỏi,
**Then** **100%** tham chiếu câu hỏi trong gói đi qua **định danh** của câu; và định danh đó là thứ sống sót qua một vòng xuất rồi nhập gói contest.

**AC-029** — *US*: US-003 · *FR*: FR-029 · *GR*: `GR-037` §Cấm · `QĐ-084` §Ranh giới đáp án
**Given** cùng contest của AC-028, và một tài khoản `bt-04` mang permission `results.export` nhưng **không** có quyền đọc kho đề,
**When** `bt-04` xuất gói,
**Then** gói xuất ra chứa đủ nhật ký sự kiện, điểm và thứ hạng; nó chứa **định danh** của cả `60` câu; nhưng nó chứa **0** nội dung câu hỏi và **0** đáp án; và thao tác **không** bị từ chối — nó chỉ ra một gói hẹp hơn.

**AC-030** — *US*: US-003 · *FR*: FR-029 · `QĐ-084` §Ranh giới đáp án
**Given** cùng contest của AC-029, và một tài khoản `bt-05` mang **cả** `results.export` **lẫn** quyền đọc kho đề,
**When** `bt-05` xuất gói,
**Then** gói chứa nội dung câu hỏi và đáp án cho `60` câu đó; và mọi phần còn lại của gói **giống hệt** gói mà `bt-04` xuất ra ở AC-029 — chênh lệch duy nhất là phần nội dung câu.

**AC-029a** — *US*: US-003 · *FR*: FR-038a, FR-038b · `PERM-020`, `GR-037` §Thứ tự đánh giá
**Given** bốn phiên đều **không** mang `results.export`, khác nhau ở hai trục: có / không quyền đọc kho đề, nhân với contest có trận đang chạy / mọi trận đã đóng sổ,
**When** cả bốn phiên yêu cầu xuất gói kết quả và nhật ký sự kiện,
**Then** cả bốn nhận **cùng một** thông điệp từ chối, **không** phân biệt được nhau qua nội dung, mã lỗi hay thời gian phản hồi; **0** byte nội dung gói rời server ở cả bốn lần; **0** phiên nào suy ra được mình có quyền đọc kho đề hay không; và **0** dữ liệu nào của contest bị đổi. Ngược lại, một phiên **có** `results.export` nhưng thiếu quyền đọc kho đề, gọi trên contest có trận đang chạy, **không** bị từ chối — nó nhận một gói hẹp mang dấu *"trận chưa đóng sổ"*.

**AC-031** — *US*: US-003 · *FR*: FR-030 · `NFR-14`, `QĐ-084` §Ranh giới đáp án
**Given** cùng contest của AC-030,
**When** `bt-05` xuất gói **ba lần** liên tiếp,
**Then** nhật ký thao tác nhận **đúng ba** dòng mới, mỗi dòng đủ người thực hiện, hành động, đối tượng, thời điểm và địa chỉ nguồn; và **không** lần xuất nào bị gộp hay bị bỏ qua trong nhật ký — mỗi lần xuất là một lần xem đáp án ở quy mô lớn.

**AC-032** — *US*: US-003 · *FR*: FR-031, FR-032 · `QĐ-084` §Thời điểm xuất · *GR*: `GR-022` C1, C6
**Given** một contest có một trận **đã đóng sổ** và một trận **đang chạy** ở giữa vòng VCNV, chưa bấm Chốt trận,
**When** admin xuất gói kết quả và nhật ký sự kiện,
**Then** thao tác **thành công**; gói mang dấu **"trận chưa đóng sổ"** cho trận đang chạy; trận đã đóng sổ trong cùng gói **không** mang dấu đó; trạng thái trận đang chạy **không đổi** — vẫn ở đúng vòng, đúng câu, đúng hàng đợi; và **0** event của trận được sinh.

**AC-033** — *US*: US-003 · *FR*: FR-032 · *GR*: `GR-028` C5
**Given** cùng contest của AC-032, với trận đang chạy có nhật ký gồm đúng `31` event tại mốc xuất,
**When** admin xuất gói giữa lúc một câu đang mở và một đồng hồ đang đếm,
**Then** nhật ký của trận vẫn có đúng `31` event sau khi xuất; đồng hồ **không** bị dừng, **không** bị đặt lại; hàng đợi tín hiệu **không** đổi; và điểm bốn ghế **không** đổi.

**AC-034** — *US*: US-003 · *FR*: FR-033 · `QĐ-084` §Chỉ X1 và X3 nhập lại được
**Given** một gói kết quả và nhật ký sự kiện đã xuất ra tệp,
**When** rà **toàn bộ** bề mặt nhập của hệ thống — mọi màn, mọi thao tác nhập, mọi định dạng nhận vào,
**Then** **0** đường nhập nhận gói này; hệ thống **không** có thao tác nào biến gói này trở lại thành dữ liệu trong máy; và nếu tệp được đưa vào một đường nhập khác thì đường đó **từ chối** mà không thay đổi dữ liệu nào.

**AC-035** — *US*: US-003, US-004 · *FR*: FR-034 · `QĐ-084` §Một nguồn
**Given** một contest có hai trận đã đóng sổ, trong đó trận thứ hai có một nhóm đồng hạng nhì và một điểm âm,
**When** admin xuất **cả ba** hiện vật — biên bản của từng trận, gói kết quả và nhật ký sự kiện, và gói kết quả rút gọn,
**Then** ba hiện vật cho ra **cùng một** bảng điểm cuối và **cùng một** thứ hạng cho cả hai trận, tới từng con số và từng dòng đồng hạng; và không hiện vật nào cần một phép tính điểm riêng của nó.

**AC-036** — *US*: US-003 · *FR*: FR-035 · *GR*: `GR-028` §Bấm trùng
**Given** một contest đã đóng sổ hoàn toàn, không trận nào đang chạy,
**When** admin xuất gói kết quả và nhật ký sự kiện **hai lần** liên tiếp mà không thao tác gì khác giữa hai lần,
**Then** hai gói có **nội dung giống hệt nhau**; nhật ký thao tác nhận **hai** dòng; và **0** dữ liệu nào của contest hay của các trận bị đổi.

**AC-037** — *US*: US-003 · *FR*: FR-036 · *GR*: `GR-028` §Điều kiện, C5 · `NFR-12`, `INV-022`
**Given** một trận đã đóng sổ trong đó điểm ghế `A` đi qua chuỗi `+10 → −5 → +3` và đạt `8` ở mốc `14:05:30` UTC+7, sau đó còn nhận thêm `+20`,
**When** admin xuất gói và dùng nội dung gói để tính lại điểm của `A` tại mốc `14:05:30`,
**Then** phép tính cho ra `8`; nó dùng **chỉ** các event có thời điểm không sau mốc đó; kết quả **tất định** — tính lại nhiều lần cho cùng con số; và phép tính này **không** cần dữ liệu nào ngoài gói.

**AC-038** — *US*: US-003, US-004 · *FR*: FR-037, FR-044 · PRD-REQ-098
**Given** một bản cài theo **hồ sơ portable** trên Windows, đã **ngắt Internet**, chứa một contest có hai trận đã đóng sổ,
**When** admin xuất gói kết quả và nhật ký sự kiện, rồi xuất gói kết quả rút gọn,
**Then** **cả hai** thao tác thành công; nội dung hai gói đầy đủ như trên hồ sơ máy chủ; **0** lời gọi ra dịch vụ ngoài được thực hiện; và cả hai gói mở đọc được mà không cần kết nối nào.

**AC-038a** — *US*: US-002 · *FR*: FR-011a · PRD-REQ-098, PRD-REQ-085
**Given** cùng bản cài portable của AC-038, đã ngắt Internet, chứa một trận đã đóng sổ có một vòng bị bỏ rồi chạy lại,
**When** admin xuất **biên bản** của trận đó,
**Then** thao tác thành công; biên bản chứa đủ mọi vòng và đủ các khối của vòng chạy nhiều lần, với đầy đủ hai trường *cách rời* và *lần chạy thứ mấy*; **0** lời gọi ra dịch vụ ngoài được thực hiện; và biên bản mở đọc được mà không cần kết nối nào — trận chạy trên portable **không** phải mang dữ liệu về máy chủ trung tâm mới có biên bản.

**AC-039** — *US*: US-003 · *FR*: FR-038 · `PERM-020`
**Given** một tài khoản `bt-06` **không** mang permission `results.export`, đang đăng nhập và có quyền xem contest,
**When** `bt-06` gửi thẳng tới server một yêu cầu xuất gói kết quả và nhật ký sự kiện,
**Then** server **từ chối**; **0** byte dữ liệu gói rời server; nhật ký thao tác nhận một dòng ghi lần từ chối; và **0** dữ liệu nào của contest bị đổi.

#### Nhóm D — Gói kết quả rút gọn (US-004)

**AC-040** — *US*: US-004 · *FR*: FR-039 · *GR*: `GR-022` C3 · `QĐ-084` X4
**Given** một contest có hai trận đã đóng sổ nhãn `hoàn thành`, trận thứ nhất người thắng là `A`, trận thứ hai người thắng là `C`,
**When** admin xuất gói kết quả rút gọn,
**Then** gói chứa **cả hai** trận; với từng trận nó chứa bảng điểm cuối, thứ hạng và người thắng; người thắng ghi đúng `A` và `C`; và các con số điểm trùng khớp với màn kết quả của từng trận.

**AC-041** — *US*: US-004 · *FR*: FR-040 · `QĐ-084` §Ranh giới đáp án
**Given** cùng contest của AC-040, trong đó hai trận đã dùng tổng cộng `60` câu hỏi,
**When** admin xuất gói kết quả rút gọn và rà **toàn bộ** nội dung tệp,
**Then** gói chứa **0** đáp án, **0** nội dung câu hỏi, và **0** định danh câu hỏi; nó chứa **0** dòng nhật ký sự kiện; và nó vẫn đủ để công bố kết quả của cả hai trận.

**AC-042** — *US*: US-004 · *FR*: FR-041 · `QĐ-084` §Thời điểm xuất
**Given** một contest có một trận **đang chạy** ở giữa vòng Về đích, chưa bấm Chốt trận, điểm hiện tại `A=60 · B=45 · C=45 · D=20`,
**When** admin yêu cầu xuất gói kết quả rút gọn cho contest đó,
**Then** yêu cầu bị **từ chối kèm lý do** nêu rõ trận chưa đóng sổ nên chưa có thứ hạng; **0** tệp được sinh; trạng thái trận đang chạy **không đổi** — vẫn đúng vòng, đúng câu, đúng đồng hồ; và điểm bốn ghế **không đổi**.

**AC-042a** — *US*: US-004 · *FR*: FR-041, FR-041a · *GR*: `GR-022` C5 · `STATE-008` §Khi `bỏ dở`
**Given** cùng trận của AC-005 — huỷ giữa vòng Tăng tốc, nhãn `bỏ dở`, lý do *"mất điện hội trường"*, điểm tại mốc đóng `A=60 · B=45 · C=45 · D=20`, không có thứ hạng và không có người thắng,
**When** admin xuất gói kết quả rút gọn cho contest chứa trận đó,
**Then** thao tác **thành công** — `bỏ dở` là một cách đóng sổ nên thoả điều kiện xuất; gói chứa bảng điểm **tại mốc đóng** đúng bốn con số trên; hai trường *thứ hạng* và *người thắng* để **trống**; gói mang **nhãn `bỏ dở`** và **lý do** *"mất điện hội trường"*; hệ thống **không** suy ra một thứ hạng nào và **không** bịa một người thắng; gói vẫn **không** chứa định danh câu hỏi nào; và trạng thái trận **không đổi**.

**AC-042b** — *US*: US-004 · *FR*: FR-041b · `PERM-020`, `GR-037` §Thứ tự đánh giá
**Given** bốn phiên gọi xuất gói kết quả rút gọn, trải đủ **cả bốn** tổ hợp *(có / không `results.export`)* × *(trận đã đóng sổ / trận đang chạy)*,
**When** mỗi phiên gửi yêu cầu xuất,
**Then** hai phiên **không** có `results.export` nhận **cùng một** thông điệp từ chối — giống hệt nhau về nội dung, mã lỗi và độ trễ, **không** nêu trạng thái trận và **không** nêu nhãn đóng sổ — với **0** byte nội dung gói rời server; phiên **có** quyền trên trận đang chạy nhận từ chối **kèm lý do** nêu rõ trận chưa đóng sổ; phiên **có** quyền trên trận đã đóng sổ **thành công**; và đối chiếu phản hồi của hai phiên đầu **không** phân biệt được trạng thái trận.

**AC-043** — *US*: US-004 · *FR*: FR-042 · `QĐ-084` §Chỉ X1 và X3 nhập lại được
**Given** một gói kết quả rút gọn đã xuất ra tệp,
**When** rà **toàn bộ** bề mặt nhập của hệ thống,
**Then** **0** đường nhập nhận gói này; và đưa tệp vào một đường nhập khác cho ra một lần **từ chối** mà không thay đổi dữ liệu nào.

**AC-044** — *US*: US-004 · *FR*: FR-043 · *GR*: `GR-022` C4, `GR-025` C5
**Given** cùng trận của AC-004 — điểm cuối `A=110 · B=100 · C=100 · D=85`, hoà ở vị trí nhì, ngoài phạm vi phân định,
**When** admin xuất gói kết quả rút gọn,
**Then** gói ghi `A` hạng 1, `B` và `C` **đồng hạng nhì**, `D` **hạng tư**; nó **không** chứa hạng ba; người thắng ghi `A`; và thứ hạng trong gói **trùng khớp** thứ hạng ở màn kết quả và ở biên bản.

**AC-045** — *US*: US-004 · *FR*: FR-045 · `PERM-020`, `NFR-14`
**Given** một tài khoản `bt-06` **không** mang `results.export` và một tài khoản `bt-03` **có** mang nó,
**When** `bt-06` yêu cầu xuất gói kết quả rút gọn, rồi `bt-03` thực hiện cùng yêu cầu,
**Then** yêu cầu của `bt-06` bị **từ chối** và **0** byte gói rời server; yêu cầu của `bt-03` thành công; nhật ký thao tác nhận **hai** dòng — một cho lần từ chối, một cho lần xuất thành công; và **0** dữ liệu nào của contest bị đổi ở cả hai lần.

#### Nhóm E — Thống kê ghi ngược kho đề (US-005)

**AC-046** — *US*: US-005 · *FR*: FR-046, FR-046a, FR-046c, FR-047 · *GR*: `GR-031` C1, `GR-026`
**Given** một trận `official` chạy **trên cùng bản cài** với kho đề, đã hiển thị `24` câu cho thí sinh, trong đó câu `Q-301` được hỏi **một** lượt và bị chấm **Sai**, câu `Q-302` được hỏi **một** lượt và bị chấm **Đúng**, câu `Q-303` được hỏi **một** lượt và nhận phán quyết **Huỷ kết quả**; trận vừa được admin bấm Chốt trận đưa về `STATE-008` nhãn `hoàn thành` lúc `20:47:12` UTC+7,
**When** bước ghi ngược thống kê chạy cho trận đó,
**Then** cả `24` câu đã hiển thị đều mang số liệu sử dụng khi người ra đề mở chúng trong kho đề; mỗi câu hiện **số lần đã hiển thị**, **thời điểm và trận của lần gần nhất** — đúng trận này và đúng mốc `20:47:12` UTC+7 — và **số lượt Đúng / Sai / Huỷ kết quả trên tổng số lượt**; `Q-301` hiện `0/1/0` trên tổng `1`, `Q-302` hiện `1/0/0`, `Q-303` hiện `0/0/1`; số liệu **không** chứa trường thời gian trả lời nào; các câu **chưa** dùng **không** mang số liệu nào; và điểm cùng thứ hạng của trận **không đổi**.

**AC-046a** — *US*: US-005 · *FR*: FR-046 · *GR*: `GR-022` C5, `GR-030` §Không đổi gì · `TERM-048`
**Given** một trận `official` chạy trên cùng bản cài, đã chạy trọn vòng Khởi động và VCNV với `18` câu đã hiển thị, rồi bị admin bấm **Huỷ trận** giữa vòng Tăng tốc, về `STATE-008` nhãn `bỏ dở`,
**When** bước ghi ngược thống kê chạy cho trận đó,
**Then** cả `18` câu đã hiển thị đều nhận số liệu sử dụng với đủ ba nhóm trường; số liệu vào **bộ số `official`**; các câu đó vẫn mang cờ `usedInContest = true` và **không** trả lại kho; và trận vẫn **không** có thứ hạng, **không** có người thắng — bước ghi ngược **không** đụng tới kết quả trận.

**AC-046b** — *US*: US-005 · *FR*: FR-046b, FR-046e · *GR*: `GR-031` C9 · `TERM-015` · `CLAUDE.md` §Phạm vi phiên bản
**Given** một contest thật đã chạy một trận `official` trong đó câu `Q-401` được hỏi một lượt và bị chấm **Sai**; sau đó cùng contest chạy một trận `practice`, và `Q-401` — vốn đã lộ nên nằm trong kho của trận practice — được hỏi lại **hai** lượt, cả hai đều chấm **Đúng**; cả hai trận đã đóng sổ,
**When** người ra đề mở `Q-401` trong kho đề,
**Then** kho đề hiện **hai bộ số riêng**: bộ `official` ghi `0/1/0` trên tổng `1`, bộ `practice` ghi `2/0/0` trên tổng `2`; hai bộ **không** cộng vào nhau ở bất kỳ đâu — **không** tồn tại một con số `2/1/0` trên tổng `3`; bề mặt hiển thị cho phân biệt được bộ nào là bộ nào; và trận practice **không** tiêu thêm câu nào khỏi kho.
**And** trên một bản cài **v1** — nơi `matchPurpose: practice` chưa bật nên chỉ có trận `official` — mở một câu bất kỳ đã dùng: kho đề vẫn hiện **đủ hai bộ**, bộ `practice` hiện `0` lần đã hiển thị, **không** có lần dùng gần nhất và `0/0/0` trên tổng `0`; bộ đó **không** bị ẩn và **không** bị gộp vào bộ `official`.

**AC-046c** — *US*: US-005 · *FR*: FR-046d · *GR*: `GR-028` §Điều kiện, §Bấm trùng · `INV-001`
**Given** cùng trận của AC-046, đã đóng sổ, với số liệu của `Q-301` đang là `0/1/0` trên tổng `1`,
**When** bước tính số liệu chạy lại **năm lần** liên tiếp cho cùng trận đó, không có trận nào mới đóng sổ ở giữa,
**Then** `Q-301` vẫn hiện `0/1/0` trên tổng `1` sau cả năm lần — **không** cộng dồn thành `0/5/0` và **không** mất dữ liệu của trận nào; **0** dấu chống trùng nào cần tồn tại để đạt kết quả này; nhật ký sự kiện của trận vẫn nguyên số event; và **0** event mới được sinh.

**AC-047** — *US*: US-005 · *FR*: FR-047, FR-048 · *GR*: `GR-031` §Ranh giới *"đã dùng"*
**Given** cùng trận của AC-046, trong đó một câu `Q-207` **đã được rút** cho vòng Khởi động nhưng admin **chưa bấm hiển thị** nó trước khi vòng kết thúc,
**When** người ra đề mở `Q-207` trong kho đề sau khi trận đóng sổ,
**Then** `Q-207` **không** mang số liệu sử dụng; nó vẫn nằm trong kho khả dụng của contest; và cờ `usedInContest` của nó vẫn là **chưa dùng**.

**AC-048** — *US*: US-005 · *FR*: FR-048 · *GR*: `GR-031` C1, §Ranh giới *"đã dùng"*
**Given** cùng trận của AC-046, trong đó một câu `Q-208` **đã hiển thị** cho thí sinh rồi bị **bỏ qua** vì không ai bấm chuông,
**When** người ra đề mở `Q-208` trong kho đề sau khi trận đóng sổ,
**Then** `Q-208` **mang** số liệu sử dụng — ranh giới là *đã hiển thị*, không phải *đã chấm*; và cờ `usedInContest` của nó là **đã dùng**.

**AC-049** — *US*: US-005 · *FR*: FR-049 · *GR*: `GR-031` §Không đổi gì · `INV-001`
**Given** cùng trận của AC-046, với nhật ký sự kiện gồm đúng `88` event tại mốc đóng sổ và `24` câu mang cờ `usedInContest = true`,
**When** bước ghi ngược thống kê chạy,
**Then** nhật ký sự kiện của trận vẫn có đúng `88` event; **0** event mới được sinh trong trận đã niêm phong; cờ `usedInContest` của cả `24` câu **không đổi**; và **0** câu nào bị đổi cờ từ đã dùng về chưa dùng.

**AC-050** — *US*: US-005 · *FR*: FR-050 · `QĐ-084` §Hệ quả
**Given** một trận đã chạy trọn và đóng sổ trên một bản cài **portable**, tách khỏi máy chủ trung tâm, và admin đã mang **bản kê câu đã dùng** về nhập vào contest tương ứng ở máy chủ trung tâm,
**When** người ra đề mở các câu đó trong kho đề ở máy chủ trung tâm,
**Then** các câu mang cờ **đã dùng**; chúng **không** mang số liệu thống kê từ trận portable; hệ thống **không** báo lỗi và **không** hiện chỗ trống bất thường vì thiếu số liệu; và cờ đã dùng chỉ đi một chiều — nhập lại cùng bản kê lần nữa không đổi gì.

**AC-051** — *US*: US-005 · *FR*: FR-051 · *GR*: `GR-037` §Cấm
**Given** cùng trận của AC-046, và một phiên **không** có quyền đọc kho đề,
**When** phiên đó truy cập bề mặt hiển thị số liệu sử dụng,
**Then** **0** byte nội dung câu hỏi và **0** byte đáp án rời server tới phiên đó qua đường này; và số liệu sử dụng **không** trở thành một kênh suy ra đáp án.

#### Nhóm F — Hiện vật đã xuất và hạn lưu trữ (US-006)

**AC-052** — *US*: US-006 · *FR*: FR-052 · `QĐ-077`, `NFR-35`
**Given** một bản cài có hạn lưu trữ đặt ở `12` tháng cho trận chính thức, và một trận chính thức đã đóng sổ quá `12` tháng,
**When** bước dọn dữ liệu theo hạn lưu trữ chuẩn bị chạy,
**Then** hệ thống phát **cảnh báo trước** khi bước dọn bắt đầu, nêu rõ phạm vi sắp bị dọn; và bước dọn **không** bắt đầu trước khi cảnh báo được phát.

**AC-053** — *US*: US-006 · *FR*: FR-053, FR-054 · `QĐ-077`, `NFR-35`, `NFR-35b`
**Given** cùng trận của AC-052, trong đó admin **đã xuất** biên bản, gói kết quả và nhật ký sự kiện, và gói kết quả rút gọn trước khi hạn tới,
**When** bước dọn dữ liệu chạy trên trận đó,
**Then** cả **ba** hiện vật đã xuất còn nguyên sau khi bước dọn kết thúc; hệ thống **không** xoá, **không** sửa và **không** đánh dấu vô hiệu bất kỳ hiện vật nào; và điều này đúng bất kể hiện vật nằm ở đâu.

**AC-054** — *US*: US-006 · *FR*: FR-054, FR-055 · `QĐ-084` §X2 là van thoát · `QĐ-091`
**Given** một contest có ba trận, hạn lưu trữ `3` tháng cho trận luyện tập và `12` tháng cho trận chính thức, và một trận luyện tập sắp tới hạn,
**When** admin xuất gói kết quả và nhật ký sự kiện **trước** khi hạn tới, rồi bước dọn chạy,
**Then** gói đã xuất còn nguyên; dữ liệu thô của trận luyện tập được dọn đúng hạn của nó; hai trận chính thức **không** bị chạm tới vì hạn của chúng khác; và bằng chứng phân xử của trận đã dọn vẫn đọc được từ gói.

#### Nhóm G — Ràng buộc xuyên suốt

**AC-054a** — *US*: US-006, US-003 · *FR*: FR-055a, FR-055b · `NFR-14`, `NFR-35b`
**Given** một contest có hai trận đã đóng sổ, và admin gọi xuất gói kết quả và nhật ký sự kiện nhưng lần gọi **thất bại** giữa chừng vì hết dung lượng đĩa,
**When** admin xử lý chỗ thiếu dung lượng rồi gọi xuất **lại**, rồi gọi thêm **ba lần** nữa,
**Then** cả bốn lần gọi sau đều thành công, mỗi lần cho ra một hiện vật **hoàn chỉnh**; hệ thống **không** đòi bước dọn dẹp nào trước khi gọi lại, **không** giữ bản ghi trạng thái nào cho lần gọi hỏng, và **không** có thực thể *"hiện vật dở dang"* nào tồn tại trong hệ thống để phải phân loại; nhật ký thao tác nhận **năm** dòng — một mang kết quả *thất bại*, bốn mang kết quả *thành công*; và **0** dữ liệu nào của contest hay của các trận bị đổi qua cả năm lần.

**AC-055** — *US*: mọi US · *FR*: FR-056, FR-057 · `NFR-36`
**Given** admin đang ở màn sau trận của một contest có hai trận và `60` câu đã dùng,
**When** admin lần lượt chạy in biên bản, xuất gói kết quả và nhật ký sự kiện, và xuất gói kết quả rút gọn,
**Then** **mỗi** thao tác hiển thị trạng thái đang xử lý rõ ràng — spinner, progress hoặc skeleton — trong suốt thời gian chạy; **không** thao tác nào để giao diện im lặng; và **mỗi** thao tác kết thúc bằng một phản hồi thành công hoặc thất bại rõ ràng.

**AC-056** — *US*: mọi US · *FR*: FR-058 · `NFR-37`
**Given** màn kết quả của một trận bốn ghế và màn danh sách hiện vật đã xuất của một contest có `30` mục, vừa tải xong trên khung nhìn tham chiếu của dự án,
**When** kiểm vị trí các phần tử quan trọng,
**Then** **0** phần tử quan trọng nằm dưới mép dưới; nội dung chính của mỗi màn nằm gọn trong một khung nhìn; danh sách `30` mục dùng **phân trang** với số dòng vừa một màn thay vì ép cuộn cả trang; và bố cục **không** nhồi nhét — vẫn giữ khoảng thở.

**AC-057** — *US*: mọi US · *FR*: FR-059 · `NFR-38`
**Given** mọi màn của feature này đang mở,
**When** rà toàn bộ chuỗi hiển thị và mọi mốc thời gian,
**Then** mọi chuỗi đều là tiếng Việt và đến từ file hằng số, **không** hard-code trong markup; mọi mốc thời gian hiển thị theo **UTC+7**; và **không** có thiết lập múi giờ theo người dùng.

**AC-058** — *US*: mọi US · *FR*: FR-060 · `NFR-39`
**Given** admin đang ở màn kết quả của một trận, mở từ danh sách trận của contest,
**When** admin tìm đường quay lại,
**Then** màn có nút **Back** rõ ràng về màn trước hoặc menu; `Esc` đóng modal đang mở trước, rồi mới quay lại; và thao tác quay lại **không** sinh event nào và **không** đổi dữ liệu nào.

## Assumptions

- **Trận mặc định là trận official, bốn ghế đều hoạt động, mode sân khấu**, trừ khi kịch bản nói khác. Đây là mặc định của nguồn (`QĐ-062`, `QĐ-105`, `CLAUDE.md` §Hai mode trả lời), không phải một lựa chọn của spec này.
- **Trận mặc định chạy trên CÙNG bản cài với kho đề.** Ca hồ sơ portable được gọi tên tường minh ở AC-038 và AC-050; ngoài hai chỗ đó, mọi kịch bản đứng trên giả định cùng bản cài — đây là điều kiện mà `PRD-REQ-081` đặt ra cho chính nó.
- **Một phiên admin giữ quyền điều khiển cho mỗi contest** tại một thời điểm (`NFR-07`, `GR-026` §Đồng thời). Không kịch bản nào của feature này dựng ca hai phiên cùng xuất, vì mọi thao tác ở đây là **đọc** và `GR-028` §Đồng thời khai *"không xảy ra"*.
- **`v1` khoá cứng `tieBreakPositions = [1]`** (`PRD-REQ-105`, `GR-022`). Mọi kịch bản đồng hạng trong spec này đứng ở vị trí **ngoài** vị trí nhất, vì đó là hình dạng duy nhất dựng được ở v1.
- **Định dạng cụ thể của mỗi hiện vật là quyết định thiết kế**, không phải yêu cầu chuẩn tắc. Nguồn nói *"xuất PDF"* cho biên bản (`STATE-008` §Cho phép) và nói *"máy đọc được"* cho hai gói (`QĐ-084`); spec này phát biểu **nội dung** và **ràng buộc** mà không khai định dạng tệp, ngoài đúng vế mà nguồn đã khai.
- **Feature này không giữ trạng thái nghiệp vụ nào của riêng nó.** Toàn bộ trạng thái trận nằm ở **hai** thứ — nhật ký sự kiện và vòng nào đang mở (`INV-022`) — nên mọi thứ feature này in ra và xuất ra đều là **hàm** của hai thứ đó. Số liệu sử dụng ở kho đề là ngoại lệ duy nhất: nó là dữ liệu **mới sinh ra**, ở phạm vi kho đề chứ không phải phạm vi trận.
- **Bước ghi ngược thống kê nằm ở phạm vi kho đề, không phải một event của trận.** Giả định này suy ra từ `INV-001` *(trận đã niêm phong thì nhật ký không nhận thêm gì)* và từ `QĐ-084` §Hệ quả cuối *(hai đường ghi cờ đã dùng đều **không** sinh `MatchEvent`)*. Số liệu là **đại lượng dẫn xuất** (FR-046d), nên cụm *"bước ghi ngược"* trong spec này chỉ **thời điểm số liệu trở nên đúng**, không chỉ một thao tác được kích hoạt.
