# Feature Specification: Contest builder và luật tuỳ biến

**Feature Branch**: `004-contest-builder-luat`

**Created**: 2026-07-30

**Status**: Draft

**Input**: EPIC-004 — Contest builder và luật tuỳ biến (`docs/PRD.md` §11, §12)

**Nguồn**: `docs/PRD.md` (EPIC-004 §11 dòng 430-441, PRD-REQ-020→026 và PRD-REQ-105 §12 dòng 758-814, JOURNEY-002 §10) · `docs/glossary.md` · `docs/game-rules.md` (`GR-001`→`GR-025`, `GR-031`) · `docs/game-state-machine.md` (`STATE-001`, guard cửa vào vòng) · `docs/traceability.md`. `docs/reviews/**` và `plans/**` không phải nguồn.

## Phạm vi

EPIC-004 phủ **giai đoạn dựng contest trước khi trận bắt đầu**: tạo contest, áp preset luật, cấu hình từng vòng, chọn mode trả lời cấp contest, gán ghế và vị trí, chọn danh sách câu hỏi, cấu hình chủ đề và âm thanh, và pre-flight kho đề tại cửa vào từng vòng. Đây là JOURNEY-002 trong PRD.

**Không thuộc phạm vi feature này** (thuộc epic khác, xem §Out of Scope): soạn câu hỏi và bộ đề (EPIC-002), nhập/xuất gói contest (EPIC-003), vòng đời một trận đang chạy và đóng băng cấu hình tại cú bấm bắt đầu (EPIC-005), thực thi luật trong lúc thi đấu — chấm điểm, hàng đợi tín hiệu, mốc thời gian (EPIC-006), can thiệp của admin giữa trận (EPIC-007).

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Tạo contest và áp preset luật 2026 (Priority: P1)

Admin tạo một contest mới rồi bấm nút **"Áp dụng luật 2026"** để toàn bộ cấu hình luật của contest — thời gian, số câu, mức điểm ở mọi vòng — được điền sẵn theo preset `O26_DEFAULT@1`, thay vì phải nhập tay từng giá trị.

**Actor**: ACTOR-001 admin · **Intent**: dựng khung luật của contest bằng một thao tác thay vì nhập tay · **User value**: xoá `PS-1`, không cần lập trình viên để có một contest chạy đúng luật O26 · **Related PRD requirements**: PRD-REQ-020, PRD-REQ-021 · **Related game rules**: `GR-001`…`GR-025` (toàn bộ) · **Related journey**: JOURNEY-002

**Why this priority**: Đây là lời giải trực tiếp cho `PS-1` (luật hard-code) và là bước đầu tiên bắt buộc của mọi contest. Không có bước này thì không có gì để cấu hình tiếp ở các story sau.

**Independent Test**: Tạo một contest trống, bấm "Áp dụng luật 2026", rồi đối chiếu từng giá trị luật hiện ra với bảng luật O26 ở `docs/game-rules.md` §Bốn bảng dùng chung — nếu khớp toàn bộ thì story này chạy được độc lập, không cần bước nào khác.

**Acceptance Scenarios**:

1. **Given** admin đã đăng nhập và chưa có contest nào đang mở, **When** admin bấm "Tạo contest" rồi đặt tên, **Then** hệ thống tạo một contest mới ở trạng thái chưa cấu hình luật, chưa có trận nào, và admin đó trở thành chủ contest.
2. **Given** một contest vừa tạo, chưa cấu hình luật, **When** admin bấm "Áp dụng luật 2026", **Then** mọi giá trị luật của cả năm vòng (Khởi động, VCNV, Tăng tốc, Về đích, Câu hỏi phụ) được đặt đúng theo preset `O26_DEFAULT@1`, khớp bảng luật O26.
3. **Given** một contest đã có cấu hình luật tuỳ chỉnh khác preset, **When** admin bấm lại "Áp dụng luật 2026", **Then** toàn bộ giá trị bị **ghi đè** về preset — thao tác là idempotent, bấm nhiều lần cho cùng một kết quả.
4. **Given** một contest đã áp preset, **When** admin không chỉnh sửa gì thêm, **Then** mọi giá trị luật MUST khớp bảng luật O26 (không giá trị nào rơi ra ngoài preset do lỗi mặc định).

---

### User Story 2 - Tuỳ chỉnh giá trị luật từng vòng, trong khuôn khổ ba khoá cứng của v1 (Priority: P1)

Admin sửa từng giá trị luật (thời gian suy nghĩ, số câu, mức điểm) cho từng vòng theo nhu cầu riêng của contest, khác với preset O26 nếu muốn — nhưng ba giá trị bị **khoá cứng ở v1** (số hàng ngang VCNV, playlist bốn vòng, phạm vi phân định hoà) không hiện lựa chọn nào khác trên giao diện.

**Actor**: ACTOR-001 admin · **Intent**: chỉnh luật của contest cho khớp nhu cầu riêng, trong khuôn khổ ba khoá cứng v1 · **User value**: một engine chạy được nhiều mùa luật, không riêng O26 · **Related PRD requirements**: PRD-REQ-020, PRD-REQ-022, PRD-REQ-024, PRD-REQ-025, PRD-REQ-105 · **Related game rules**: `GR-001`…`GR-025`, `GR-009` C11, `GR-020`, `GR-022`, `GR-023`, `GR-025`, `GR-030` · **Related journey**: JOURNEY-002

**Why this priority**: Đây là giá trị cốt lõi của EPIC-004 — "đổi luật không cần lập trình viên" (`PS-1`). Không tuỳ biến được thì contest builder chỉ là một màn hình đọc preset.

**Independent Test**: Trên một contest đã áp preset, đổi một giá trị đơn lẻ (vd thời gian suy nghĩ Khởi động lượt riêng) rồi lưu; xác nhận contest giữ giá trị mới mà không đụng các giá trị khác. Test độc lập với việc chạy trận thật.

**Acceptance Scenarios**:

1. **Given** một contest đã áp preset O26, **When** admin sửa một giá trị luật của một vòng (vd đổi thời gian suy nghĩ Tăng tốc), **Then** hệ thống lưu giá trị mới cho đúng vòng đó và không đổi giá trị của các vòng khác.
2. **Given** contest builder đang mở màn cấu hình VCNV, **When** admin tìm lựa chọn số hàng ngang, **Then** giao diện **không hiện** lựa chọn nào khác ngoài 4 — không có ô nhập cho phép gõ số khác.
3. **Given** contest builder đang mở màn cấu hình playlist, **When** admin tìm cách thêm/bớt/đổi thứ tự vòng, **Then** giao diện **không có** thao tác nào làm được việc đó — playlist cố định Khởi động → VCNV → Tăng tốc → Về đích.
4. **Given** contest builder đang mở màn cấu hình phân định hoà, **When** admin tìm lựa chọn vị trí phân định, **Then** giao diện **không hiện** lựa chọn nào khác ngoài "vị trí Nhất".
5. **Given** một request gửi thẳng tới server (bỏ qua UI) với `rowCount = 6` hoặc `tieBreakPositions = [1, 2]` cho một contest v1, **When** server nhận request đó, **Then** server **từ chối** ghi giá trị đó và cấu hình contest **giữ nguyên không đổi** — khoá cứng được **server thực thi (zero-trust)**, không chỉ là UX ẩn lựa chọn ở giao diện. Sức chứa "vẫn nhận 5-8 / nhiều phần tử không lỗi cấu trúc" (PRD-REQ-025, PRD-REQ-105) chỉ nói về **kiểu dữ liệu của schema** (chuẩn bị cho v1.5), không phải một đường ghi hợp lệ qua API cấu hình contest của v1.
6. **Given** một contest mang `rowCount` hoặc `tieBreakPositions` ngoài khoá cứng v1 — chỉ có thể tồn tại qua nguồn **ngoài** API v1 (seed/fixture/migration nội bộ, không qua contest builder) — **When** admin mở lại contest đó trong contest builder, **Then** contest **vẫn mở được**, giao diện **hiển thị giá trị thật** và **khoá (disable) trường đó khỏi chỉnh sửa** — vì server sẽ từ chối mọi lần ghi lại giá trị ≠ khoá cứng, cho sửa tiếp cũng vô nghĩa.
7. **Given** một contest đã áp preset O26, **When** admin sửa một mức điểm thành giá trị lẻ, **Then** hệ thống lưu giá trị lẻ đó nguyên trạng — cấu hình luật không tự làm tròn hay từ chối giá trị lẻ (`GR` không định nghĩa validation làm tròn ở bước cấu hình; làm tròn `value / 2` chỉ áp dụng khi tính phạt lúc thi đấu, thuộc EPIC-006).
8. **Given** contest builder đang mở màn cấu hình Về đích và Câu hỏi phụ, **When** admin tìm cách đổi cách tính cướp quyền sang **"cộng thêm"** (`stealMode: 'add'`, thay vì chuyển điểm) hoặc đổi cách xử lý hết câu Câu hỏi phụ sang **"admin tự quyết"** (thay vì bốc thăm), **Then** hệ thống **cho phép** — cả hai là option hợp lệ của engine dù không thuộc preset O26 — và giá trị được lưu là cấu hình riêng của contest đó, không đổi giá trị mặc định của preset.
9. **Given** một contest vừa áp preset O26, **When** admin không sửa gì thêm, **Then** cả hai option ở kịch bản 8 MUST ở đúng giá trị O26 — cướp quyền kiểu **chuyển điểm**, hết câu Câu hỏi phụ **bốc thăm** — và admin vẫn đổi thủ công được sau đó bất cứ lúc nào trước khi trận đóng băng cấu hình.
10. **Given** một contest đã có **ít nhất một trận `FINISHED`** và trận hiện tại (nếu có) đang ở `LOBBY`, **When** admin sửa một giá trị RuleConfig của contest, **Then** hệ thống **cho phép**, và thay đổi đó **không đụng** tới bản cấu hình đã đóng băng vào bất kỳ trận nào đã bắt đầu hay đã `FINISHED` — chỉ áp cho trận **kế tiếp** của contest đó.
11. **Given** một vòng của trận hiện tại **đang chạy** (không ở `LOBBY`), **When** admin thử sửa một giá trị RuleConfig cấp contest, **Then** thao tác **không thực hiện được** — cùng cửa khoá với việc sửa danh sách câu — và RuleConfig giữ nguyên không đổi.

---

### User Story 3 - Chọn mode trả lời cấp contest (Priority: P2)

Admin chọn đúng một mode trả lời — **sân khấu** (mặc định) hoặc **nhập liệu** — áp cho toàn bộ các vòng của contest. Mode này tách biệt khỏi preset luật.

**Actor**: ACTOR-001 admin · **Intent**: chọn điều kiện sân khấu (có micro, thí sinh có bàn phím không) tách khỏi giá trị luật · **User value**: một trận không trộn hai mô hình thao tác giữa chừng · **Related PRD requirements**: PRD-REQ-023 · **Related game rules**: `GR-007`, `GR-017`, `GR-021` · **Related journey**: JOURNEY-002

**Why this priority**: Mode ảnh hưởng trực tiếp tới cách thí sinh thao tác và tới việc chuẩn bị phần cứng ngày thi, nhưng contest vẫn tạo và cấu hình luật được mà chưa chọn mode (mặc định sân khấu) — nên độ ưu tiên thấp hơn hai story cấu hình lõi.

**Independent Test**: Đổi mode của một contest chưa có trận nào, xác nhận nút "Áp dụng luật 2026" không đổi mode, và xác nhận contest lưu đúng mode đã chọn.

**Acceptance Scenarios**:

1. **Given** một contest vừa tạo, **When** admin chưa chọn mode, **Then** contest mang mode mặc định **sân khấu**.
2. **Given** một contest đang ở mode sân khấu, **When** admin đổi sang mode nhập liệu, **Then** toàn bộ vòng của contest đó chuyển sang nhập liệu — không có vòng nào giữ mode cũ.
3. **Given** một contest đã đổi mode khác mặc định, **When** admin bấm "Áp dụng luật 2026", **Then** mode **không đổi** — nút preset chỉ chạm giá trị luật, không chạm mode.
4. **Given** một contest đã có **ít nhất một trận `FINISHED`**, **When** admin đổi mode của contest, **Then** thao tác **hợp lệ** — sửa mode không bị chặn bởi lịch sử trận đã đóng sổ (`QĐ-075`); trận `FINISHED` đó giữ nguyên mode đã dùng lúc thi.

---

### User Story 4 - Gán ghế và vị trí thí sinh (Priority: P1)

Admin gán mỗi thí sinh vào một ghế và một số vị trí (1..N) trước khi trận đầu tiên của contest chạy vòng nào. Sau khi vòng đầu tiên đã chạy, vị trí bất biến.

**Actor**: ACTOR-001 admin · **Intent**: cố định ai ngồi ghế nào và thứ tự vị trí trước khi luật cần đọc số vị trí · **User value**: hai luật vận hành (thứ tự chọn hàng ngang, phá hoà thứ tự lượt Về đích) có đầu vào ổn định · **Related PRD requirements**: PRD-REQ-026 · **Related game rules**: `GR-016`, `GR-007` · **Related journey**: JOURNEY-002, JOURNEY-004

**Why this priority**: Vị trí là đầu vào của hai luật vận hành (thứ tự chọn hàng ngang VCNV, phá hoà thứ tự lượt Về đích) — thiếu bước này thì trận không thể bắt đầu đúng luật.

**Independent Test**: Gán 4 ghế với vị trí 1-4 cho một contest, xác nhận lưu đúng; thử đổi vị trí sau khi giả lập một vòng đã chạy và xác nhận bị chặn.

**Acceptance Scenarios**:

1. **Given** một contest chưa có trận nào từng chạy vòng, **When** admin gán đúng 4 thí sinh vào 4 ghế với vị trí liên tục 1..4, **Then** hệ thống lưu đúng cặp (ghế, vị trí) cho từng thí sinh, và đây là cấu hình duy nhất **bắt đầu trận được** ở v1 (FR-030), áp như nhau cho contest official lẫn practice.
2. **Given** một contest v1 (official hoặc practice), **When** admin gán N thí sinh vào N ghế với 1 ≤ N ≤ 12, N ≠ 4 — qua giao diện hoặc qua request gửi thẳng tới server — **Then** hệ thống **lưu phép gán đó bình thường**: `PRD-REQ-026` và `NON-GOAL-012` đòi **cả mô hình dữ liệu lẫn giao diện** hỗ trợ 1-12 ngay ở v1, nên bước gán ghế **không** có khoá cứng nào.
2b. **Given** một contest v1 đã gán N ghế với **N > 4**, **When** admin bấm **bắt đầu trận**, **Then** server **chặn cứng, không ép được**, kèm thông điệp nêu rõ v1 chưa có thang điểm cho hơn 4 đơn vị điểm; cấu hình **chưa** bị đóng băng, phép gán ghế **giữ nguyên** không bị xoá, và admin bớt về ≤ 4 ghế rồi bắt đầu được ngay (FR-030). *(N < 4 thì **không** bị chặn — trận bắt đầu bình thường và các ghế thiếu chạy như **ghế bỏ thi**: vô hiệu hoá từ trước mốc đóng băng, điểm luôn 0, giữ nguyên vị trí — cơ chế thuộc `specs/005` FR-035, FR-036.)*
3. **Given** một trận của contest **đã chạy ít nhất một vòng**, **When** admin thử đổi vị trí của một ghế, **Then** thao tác **không thực hiện được** (invalid state) — không có nút hoặc đường API nào cho việc này, và vị trí của **mọi** ghế khác giữ nguyên không đổi.
4. **Given** một contest đang gán ghế, ghế X đã giữ vị trí 2, **When** admin gán vị trí 2 cho ghế Y (khác ghế X), **Then** hệ thống hiện **dialog xác nhận "thay thế?"**; admin bấm **Yes** ⇒ vị trí 2 chuyển sang ghế Y, ghế X trở về trạng thái **chưa gán vị trí**; admin bấm **No** ⇒ phép gán **giữ nguyên như trước khi thao tác**, không ghế nào đổi vị trí.
5. **Given** một contest **practice** đang gán ghế, **When** admin gán số ghế trên 4, **Then** hành vi **giống hệt** contest official — phép gán lưu bình thường ở bước gán ghế, và cú bấm bắt đầu trận mới là chỗ bị chặn (AC US4-2, US4-2b); `contestPurpose` **không** đổi hàng rào nào, vì luật đa ghế cho **mọi** mục đích trận là hạng mục v1.5.
6. **Given** một contest đang gán ghế, **When** admin bỏ trống vị trí ở giữa dãy (vd gán vị trí 1, 2, 4 nhưng bỏ trống 3), **Then** hệ thống **từ chối** — vị trí MUST là số nguyên dương **liên tục** (`TERM-002`) — và giữ nguyên phép gán hợp lệ gần nhất trước đó, không lưu dãy có lỗ hổng.

---

### User Story 5 - Chọn danh sách câu hỏi cho contest (Priority: P1)

Admin bắt buộc phải chọn một danh sách câu hỏi cụ thể (qua tìm kiếm toàn văn, lọc, sắp xếp trên kho đề) và gán vào contest trước khi trận có thể bắt đầu. Hệ thống không bao giờ tự lấy đề.

**Actor**: ACTOR-001 admin · **Intent**: chụp một tập câu hỏi cụ thể vào contest làm nguồn rút đề duy nhất của mọi trận · **User value**: đề không lộ ngoài kế hoạch, và không trận nào khởi động rồi thiếu câu · **Related PRD requirements**: không có `PRD-REQ` riêng dưới EPIC-004 trong `docs/PRD.md` §12 cho đúng bước "chọn danh sách câu" — yêu cầu này nằm ở `docs/PRD.md` §8 Core Game Loop bước 2 và JOURNEY-002, dẫn xuất từ `GOAL-002`, `GOAL-005` · **Related game rules**: `GR-031` (toàn bộ) · **Related journey**: JOURNEY-002, JOURNEY-001 (phụ thuộc câu `ACTIVE` từ EPIC-002)

**Why this priority**: Đây là điều kiện tiên quyết để bất kỳ vòng nào rút được câu hỏi (`GR-031`) — không có danh sách này thì không trận nào chạy được.

**Independent Test**: Trên một contest đã có kho đề `ACTIVE`, chạy tìm kiếm/lọc, chọn một tập câu, gán vào contest, rồi xác nhận danh sách đã gán hiển thị đúng và pre-flight của từng vòng phản ánh đúng số câu khả dụng.

**Acceptance Scenarios**:

1. **Given** kho đề có câu `ACTIVE` đủ cho các vòng, **When** admin tìm kiếm/lọc/sắp xếp rồi chọn một tập câu và gán vào contest, **Then** hệ thống lưu đúng danh sách đã chọn làm **snapshot** của contest — không tự thêm hay bớt câu nào ngoài lựa chọn của admin.
2. **Given** một contest chưa gán đủ câu cho một vòng, **When** admin thử mở vòng đó ở một trận, **Then** vòng đó **không mở được** — chặn cứng, không ép được — trong khi **các vòng khác vẫn mở bình thường** nếu đủ câu (`GR-031` C3).
3. **Given** vòng VCNV cần rút đề, **When** danh sách đã gán có đủ 4 hàng ngang và 1 Chướng ngại vật nhưng chúng **không thuộc cùng một bộ**, **Then** vòng VCNV **không mở được** — pre-flight đếm **bộ nguyên vẹn** (1 Chướng ngại vật + 4 hàng ngang + 1 ô trung tâm, cả sáu thành phần chưa dùng), không đếm số câu rời (`GR-031` C3b, `QĐ-082`).
4. **Given** một hàng ngang của một bộ VCNV đã bị admin chỉ định làm câu Câu hỏi phụ, **When** admin xác nhận chỉ định đó, **Then** hệ thống **cảnh báo trước**, nêu rõ bộ VCNV nào sẽ vỡ, trước khi admin xác nhận (`GR-031` C3c).
5. **Given** trận đang ở `LOBBY` (chưa vòng nào đang chạy), **When** admin thêm hoặc bớt câu trong danh sách đã gán, **Then** thao tác **được phép**, pre-flight chạy lại ngay sau khi sửa, và cờ `usedInContest` của các câu khác **không bị đụng tới** (`GR-031` C5).
6. **Given** một câu **đã hiển thị** cho thí sinh trong bất kỳ trận nào của contest, **When** admin thử gỡ câu đó khỏi danh sách đã gán, **Then** thao tác **không thực hiện được** — invalid state, không ép được (`GR-031` C6).
7. **Given** một câu **đã rút nhưng chưa hiển thị**, **When** admin gỡ câu đó khỏi danh sách đã gán, **Then** hệ thống **cho phép**, và câu đó coi như **chưa tiêu**, quay lại pool hiệu dụng của kho đề (`GR-031` C7).
8. **Given** một vòng đang chạy (không ở `LOBBY`), **When** admin thử sửa danh sách câu đã gán, **Then** thao tác **không thực hiện được** — cửa sửa chỉ mở ở `LOBBY` (`GR-031` C8).
9. **Given** một trận `practice` chạy trong một contest **official (thật)**, **When** hệ thống tính pool khả dụng cho trận đó, **Then** pool bị **lọc đảo chiều** — chỉ gồm câu đã có `usedInContest = true` (đã lộ ở trận thật trước đó); nếu contest chưa từng chạy trận nào, pool này **rỗng** và mọi vòng của trận practice tự chặn ở cửa vào (`GR-031` C9).
10. **Given** admin đưa vào danh sách một câu mang `everPublic = true` cho một contest **official**, **When** admin xác nhận thêm câu đó, **Then** hệ thống **chặn cứng**, ép được chỉ bằng xác nhận hai bước kèm audit (JOURNEY-002 alternative flow, `docs/PRD.md` §10).

---

### User Story 6 - Cấu hình chủ đề và âm thanh (Priority: P3)

Admin chọn chủ đề hiển thị và tải lên âm thanh cho từng slot hiệu ứng của contest.

**Actor**: ACTOR-001 admin · **Intent**: chuẩn bị phần lên hình của contest cùng lúc với luật, không phải nghĩ nhiều nếu không cần tuỳ biến sâu · **User value**: `GOAL-008` (trình diễn) · **Related PRD requirements**: không có `PRD-REQ` số hoá riêng trong `docs/PRD.md` §12 (chỉ liệt kê ở phạm vi EPIC-004 §11) — SHOULD, không MUST; xem `OQ-001` · **Related game rules**: không có `GR-*` — chủ đề/âm thanh là cấu hình trình diễn, đứng ngoài luật thi đấu · **Related journey**: JOURNEY-002

**Why this priority**: Ảnh hưởng tới trình diễn (`GOAL-008`) nhưng không chặn khả năng chạy trận đúng luật — mọi story trước có thể hoàn thành và trận vẫn thi đấu được dù chưa cấu hình chủ đề/âm thanh.

**Independent Test**: Chọn một chủ đề và upload một file âm thanh cho một slot, xác nhận lưu đúng; để trống các slot còn lại và xác nhận hệ thống không báo lỗi vì thiếu.

**Acceptance Scenarios**:

1. **Given** một contest đang cấu hình, **When** admin chọn một chủ đề hiển thị, **Then** hệ thống lưu lựa chọn đó cho contest.
2. **Given** một contest đang cấu hình âm thanh, **When** admin tải lên một file cho một slot hiệu ứng cụ thể trong ngưỡng kích thước env config, **Then** hệ thống lưu file đó gắn với đúng slot.
3. **Given** một slot âm thanh chưa được admin gán file, **When** hệ thống tới lúc phát slot đó trong trận, **Then** slot đó **im lặng** — không có bộ SFX mặc định nào tự động điền vào.
4. **Given** admin tải lên một file vượt ngưỡng kích thước cấu hình cho loại media đó, **When** hệ thống nhận file, **Then** hệ thống từ chối và thông điệp từ chối đọc đúng ngưỡng từ cấu hình hiện hành (không hard-code con số).
5. **Given** một contest chưa cấu hình chủ đề/âm thanh gì, **When** admin chọn một **gói dựng sẵn (preset)** từ danh sách gói có sẵn, **Then** hệ thống áp toàn bộ chủ đề và âm thanh của gói đó cho contest bằng một thao tác, và admin vẫn sửa lại từng phần sau đó nếu muốn.
6. **Given** một contest chưa cấu hình chủ đề/âm thanh và admin **không** chọn preset nào, **When** contest đó bắt đầu trận, **Then** hệ thống **không tự động điền** một chủ đề hay bộ âm thanh nào — chủ đề trống theo mặc định nền tảng (nếu có) và mọi slot âm thanh **im lặng**; đây không phải một ca lỗi.

---

### User Story 7 - Kiểm kho đề (pre-flight) trước khi bắt đầu (Priority: P1)

Trước khi admin bắt đầu trận và tại mỗi lần mở một vòng, hệ thống tự động kiểm danh sách câu đã gán có đủ cho vòng đó không, và chặn cứng nếu thiếu.

**Actor**: ACTOR-001 admin (người thấy kết quả kiểm) · ACTOR-007 server (thực hiện phép kiểm) · **Intent**: không để một vòng mở ra rồi kẹt giữa chừng vì thiếu đề · **User value**: `GOAL-005`, phần "kho đề tập trung" · **Related PRD requirements**: không có `PRD-REQ` riêng — cơ chế pre-flight bắt nguồn từ `QĐ-042` (chặn cứng thứ ba của `QĐ-003`) và dùng chung `GR-031` với US-005 · **Related game rules**: `GR-031` C3, C3b, C3c, C4, C8, §Đồng thời · **Related journey**: JOURNEY-002

**Why this priority**: Đây là rào chắn cuối cùng ngăn một trận khởi động rồi kẹt giữa chừng vì thiếu đề — mức độ nghiêm trọng ngang US-005 vì hai story chia sẻ cùng cơ chế `GR-031`.

**Independent Test**: Gán một danh sách câu cố ý thiếu cho một vòng, thử mở vòng đó, xác nhận bị chặn cứng và các vòng khác (đủ câu) vẫn mở bình thường.

**Acceptance Scenarios**:

1. **Given** danh sách câu đã gán đủ cho mọi vòng, **When** admin mở bất kỳ vòng nào, **Then** pre-flight qua, vòng mở bình thường.
2. **Given** danh sách câu đã gán thiếu đúng 1 câu so với nhu cầu của một vòng cụ thể, **When** admin thử mở vòng đó, **Then** hệ thống **chặn cứng**, hiển thị rõ **thiếu bao nhiêu câu**, và **không** cho ép qua.
3. **Given** một vòng đã từng mở được (đủ câu tại cửa vào), **When** vòng đó đang chạy, **Then** tập câu khả dụng của vòng **bất biến** trong suốt vòng — không có khái niệm "cạn kho giữa vòng" (`GR-031` C4).
4. **Given** admin bấm nút mở vòng nhiều lần liên tiếp trong lúc pre-flight đang chạy, **When** hệ thống nhận các lần bấm đó, **Then** hành vi trùng lặp không sinh hai lượt mở vòng — tương ứng với nguyên tắc "một admin cho mỗi contest, không có hai lượt vận hành đồng thời" (`GR-031` §Đồng thời).

---

### Edge Cases

- **Boundary — đúng bằng ngưỡng số câu cần**: danh sách đã gán có **đúng** số câu tối thiểu cho một vòng (không dư một câu nào) ⇒ vòng vẫn mở được (`GR-031` biên "tối thiểu 1 câu mỗi lượt" áp dụng theo hướng ≥, không phải >).
- **Invalid state — sửa cấu hình luật khi một vòng đang chạy**: RuleConfig và danh sách câu cấp contest sửa được ở `LOBBY` hoặc sau trận `FINISHED`, nhưng **không thực hiện được** trong lúc một vòng đang chạy (FR-028, `GR-031` C8) — sửa đổi đó không đụng bản đã đóng băng vào trận đang chạy hoặc đã đóng sổ.
- **Repeated action — bấm "Áp dụng luật 2026" nhiều lần liên tiếp**: idempotent, kết quả cuối luôn là preset (US-001 AC3).
- **Repeated action — bấm rút/gán lại danh sách câu nhiều lần**: mỗi lần gán là một snapshot mới ghi đè snapshot cũ khi contest ở trạng thái chưa có trận nào chạy vòng; không có event log riêng cho bước "gán danh sách" ở cấp contest builder (event `QUESTIONS_DRAWN` chỉ sinh lúc trận rút đề thật, thuộc EPIC-006).
- **Stale state — admin mở contest builder trên một contest đã có giá trị `rowCount` hoặc `tieBreakPositions` ngoài khoá cứng v1** (chỉ tồn tại qua nguồn ngoài API v1): hiển thị giá trị thật, khoá trường khỏi chỉnh sửa (FR-032, AC US2-6).
- **Duplicate event — gán một vị trí đã bị ghế khác giữ**: không có tranh chấp đồng thời thật (một admin cho mỗi contest); hệ thống hiện dialog xác nhận "thay thế?" (FR-029).
- **Partial failure — upload âm thanh thất bại giữa chừng (mất kết nối khi tải)**: US-006 AC4 chỉ phủ ca vượt ngưỡng kích thước; ca mất kết nối giữa chừng không có bảng quyết định nguồn nào mô tả (không phải phạm vi cấu hình luật của EPIC-004, cũng không nằm trong `GR-*`) — không ghi thành yêu cầu ở đây.
- **Conflicting rule — admin vừa khoá cứng playlist bốn vòng (US-002 AC3), vừa cần "chạy lại vòng" trong lúc vận hành**: đây **không phải xung đột** — `QĐ-069` phân biệt rõ khoá ở **thiết kế** contest (EPIC-004, phạm vi feature này) với quyền **chạy lại một vòng đã hỏng** lúc vận hành (EPIC-007, ngoài phạm vi).
- **Missing source behavior — chủ đề và âm thanh nằm trong phạm vi liệt kê của EPIC-004 nhưng chưa có `PRD-REQ` số hoá riêng**: cấu hình này là tuỳ chọn (SHOULD) và không chặn tạo hay bắt đầu contest (FR-023, FR-024); khoảng trống truy nguyên trong `docs/PRD.md` §12 vẫn còn ⇒ `OQ-001`.

## Requirements *(mandatory)*

### Functional Requirements

- **FR-033**: Với **mọi** quy tắc chặn/khoá/validate trong feature này — cả ba khoá cứng v1 (FR-004, FR-006, FR-007), pre-flight kho đề (FR-016), khoá sửa theo trạng thái vòng/trận (FR-014, FR-018, FR-028), khoá gỡ câu đã hiển thị (FR-019), chặn `everPublic` (FR-022), và chặn số ghế **lớn hơn 4** tại cửa bắt đầu trận (FR-030) — server MUST là nơi thực thi quyết định, và MUST từ chối mọi request vi phạm bất kể request đến từ giao diện contest builder hay gọi thẳng API/socket. Giao diện MUST chỉ đóng vai trò **lớp tăng cường (enhancement)** — ẩn lựa chọn không hợp lệ, disable nút, báo lỗi tức thời — để admin có phản hồi nhanh mà không cần chờ round-trip; giao diện **MUST NOT** là hàng rào duy nhất cho bất kỳ quy tắc nào trong danh sách trên. *(Toàn bộ US-002, US-004, US-005, US-007; `CLAUDE.md` §Quy ước code (zero-trust))*
- **FR-001**: Hệ thống MUST cho phép admin tạo một contest mới, ở trạng thái chưa cấu hình luật và chưa có trận nào. *(US-001; PRD-REQ-020; `GR-031` (contest là điều kiện tiên quyết của mọi rút đề); JOURNEY-002; AC US1-1)*
- **FR-002**: Hệ thống MUST cung cấp một thao tác duy nhất **"Áp dụng luật 2026"** đặt toàn bộ giá trị luật của mọi vòng trong contest về preset `O26_DEFAULT@1`. *(US-001; PRD-REQ-021; GOAL-003; `TERM-051`; AC US1-2, US1-3, US1-4)*
- **FR-003**: Hệ thống MUST cho phép sửa từng giá trị thời gian, số câu, và mức điểm của mọi vòng như dữ liệu cấu hình — không giá trị luật nào được viết cứng trong giao diện. *(US-002; PRD-REQ-020; `GR-001`…`GR-025`; AC US2-1, US2-7)*
- **FR-004**: Giao diện tạo contest MUST khoá số hàng ngang VCNV ở giá trị 4, không hiện lựa chọn nào khác — lớp tăng cường; hàng rào thật thi hành ở server theo FR-005 và nguyên tắc chung FR-033. *(US-002; PRD-REQ-025; `GR-009` C11; `QĐ-068`; AC US2-2)*
- **FR-005**: Schema/kiểu dữ liệu của `rowCount` MUST có sức chứa cho giá trị 5-8 (chuẩn bị cho v1.5), nhưng server MUST từ chối — theo nguyên tắc zero-trust, không chỉ ẩn ở UI — mọi request ghi `rowCount ≠ 4` cho một contest v1, bất kể qua UI hay gọi thẳng API; lần từ chối MUST để cấu hình contest nguyên trạng. *(US-002; PRD-REQ-025; FR-033; AC US2-5, US2-6)*
- **FR-006**: Giao diện tạo contest MUST khoá playlist ở đúng bốn vòng, đúng thứ tự Khởi động → Vượt chướng ngại vật → Tăng tốc → Về đích — không có thao tác thêm, bớt, lặp, hay đổi thứ tự vòng ở bước thiết kế contest; server MUST từ chối mọi request cấu hình playlist khác bốn vòng chuẩn, bất kể qua UI hay gọi thẳng API (FR-033). *(US-002; PRD-REQ-024; `GR-022`, `GR-030`; `QĐ-069`; AC US2-3)*
- **FR-007**: Giao diện tạo contest MUST khoá phạm vi phân định hoà ở đúng "vị trí Nhất" (`tieBreakPositions = [1]`), không hiện lựa chọn nào khác — lớp tăng cường; hàng rào thật thi hành ở server theo FR-008 và nguyên tắc chung FR-033. *(US-002; PRD-REQ-105; `GR-022`, `GR-023`, `GR-025`; `QĐ-085`; AC US2-4)*
- **FR-008**: Schema/kiểu dữ liệu của `tieBreakPositions` MUST có sức chứa cho mảng nhiều phần tử (chuẩn bị cho phiên bản sau), nhưng server MUST từ chối — theo nguyên tắc zero-trust — mọi request ghi `tieBreakPositions ≠ [1]` cho một contest v1, bất kể qua UI hay gọi thẳng API; lần từ chối MUST để cấu hình contest nguyên trạng. *(US-002; PRD-REQ-105; `GR-022`, `GR-025`; FR-033; AC US2-5, US2-6)*
- **FR-032**: Khi một contest mang `rowCount` hoặc `tieBreakPositions` ngoài khoá cứng v1 (chỉ có thể tồn tại qua nguồn ngoài API v1), contest builder MUST mở được contest đó, MUST hiển thị giá trị thật và MUST khoá (disable) trường đó khỏi chỉnh sửa thay vì từ chối mở toàn bộ contest. *(US-002; PRD-REQ-025, PRD-REQ-105; AC US2-6)*
- **FR-027**: Hệ thống MUST hỗ trợ hai option cấu hình hợp lệ nhưng **không** thuộc preset O26 — cướp quyền kiểu "cộng thêm" (`stealMode: 'add'`) thay cho "chuyển điểm", và xử lý hết câu Câu hỏi phụ kiểu "admin tự quyết" thay cho "bốc thăm" — và MUST NOT tự đặt hai option này làm giá trị mặc định của preset `O26_DEFAULT@1`. *(US-002; PRD-REQ-022; `GR-020`, `GR-025`; `traceability.md` §Biến thể bị loại; AC US2-8, US2-9)*
- **FR-009**: Hệ thống MUST cho phép admin đặt đúng một mode trả lời — **sân khấu** hoặc **nhập liệu** — cho toàn bộ các vòng của một contest; mode mặc định là **sân khấu**. *(US-003; PRD-REQ-023; `GR-007`, `GR-017`, `GR-021`; `TERM-016`; AC US3-1, US3-2)*
- **FR-010**: Nút "Áp dụng luật 2026" MUST NOT thay đổi mode trả lời hiện tại của contest. *(US-003; PRD-REQ-023; `QĐ-075`; AC US3-3)*
- **FR-011**: Hệ thống MUST cho phép sửa mode của một contest bất kể contest đó đã có trận `FINISHED` hay chưa; việc sửa MUST NOT tác động tới trận đã chạy. *(US-003; PRD-REQ-023; `QĐ-075`; AC US3-4)*
- **FR-012**: Hệ thống MUST cho phép admin gán mỗi thí sinh vào một ghế và một vị trí (số nguyên dương, liên tục 1..N) trước khi bất kỳ vòng nào của trận đầu tiên đã chạy. *(US-004; PRD-REQ-026; `TERM-002`; `GR-016`, `GR-007`; AC US4-1, US4-6)*
- **FR-029**: Khi admin gán một vị trí đã bị một ghế khác giữ, giao diện MUST hiện dialog xác nhận "thay thế?" như lớp tăng cường; server MUST tự kiểm tra lại xung đột vị trí tại thời điểm ghi (không tin dialog đã hiện phía client) và chỉ thực hiện chuyển vị trí khi request mang xác nhận hợp lệ — xác nhận Yes MUST chuyển vị trí sang ghế mới và đưa ghế cũ về trạng thái chưa gán vị trí; No hoặc đóng dialog MUST giữ nguyên phép gán trước đó, không ghế nào đổi vị trí. *(US-004; `TERM-002`; `QĐ-005`, `QĐ-072` (dialog xác nhận thao tác một chiều của admin); FR-033; AC US4-4)*
- **FR-030**: Bước **gán ghế** MUST nhận tổng số ghế từ 1 đến 12 cho **mọi** contest v1 — official lẫn practice: giao diện MUST cho hoàn tất phép gán ở mọi giá trị trong dải đó, và server MUST ghi phép gán đó. Số ghế MUST NOT là một khoá cứng của cửa tạo contest — nó khác `rowCount`, playlist và `tieBreakPositions` ở đúng điểm này. Hàng rào của v1 nằm ở **cửa bắt đầu trận** và nó chặn **số ghế LỚN HƠN 4**: server MUST chặn cứng, không ép được, cú bấm bắt đầu một trận có **trên 4 ghế** đã gán, kèm thông điệp nêu rõ v1 chưa có thang điểm cho số ghế đó; cấu hình MUST NOT bị đóng băng và phép gán ghế MUST giữ nguyên khi cú bắt đầu bị chặn. Trận có **1 đến 4** ghế MUST bắt đầu được — ghế thiếu chạy như **ghế bỏ thi**; cơ chế ghế bỏ thi thuộc `specs/005` FR-035, FR-036, không thuộc feature này. *(US-004; `PRD-REQ-026`, `PRD-REQ-114`, NON-GOAL-012, `QĐ-007`, `QĐ-105`, FR-033 (zero-trust); AC US4-1, US4-2, US4-2b, US4-5)*
- **FR-013**: Mô hình dữ liệu **và giao diện** MUST hỗ trợ 1 đến 12 thí sinh — không cần migrate schema **và không cần dựng lại giao diện** khi v1.5 mở khoá luật đa ghế; v1 chỉ chưa có **thang điểm** cho hơn 4 đơn vị điểm, và chỗ duy nhất khác biệt đó lộ ra là cửa bắt đầu trận (FR-030). *(US-004; PRD-REQ-026 (GOAL-002); NON-GOAL-012; `QĐ-007`; `GR-007`, `GR-016`; AC US4-2)*
- **FR-014**: Server MUST chặn mọi thao tác đổi vị trí của một ghế sau khi vòng đầu tiên của một trận trong contest đã chạy, bất kể qua UI hay gọi thẳng API (FR-033); giao diện không hiện nút cho thao tác này chỉ là lớp tăng cường. *(US-004; PRD-REQ-026; AC US4-3)*
- **FR-015**: Hệ thống MUST bắt buộc admin chọn một danh sách câu hỏi cụ thể — qua tìm kiếm toàn văn, lọc, sắp xếp trên kho đề — và gán vào contest trước khi bất kỳ trận nào có thể bắt đầu; hệ thống MUST NOT tự động lấy đề. *(US-005; không có `PRD-REQ` riêng — dẫn xuất từ `GOAL-002`, `GOAL-005`, `docs/PRD.md` §8 bước 2 (xem ghi chú "Related PRD requirements" của US-005); `GR-031`; JOURNEY-002; AC US5-1)*
- **FR-016**: Server MUST kiểm, tại cửa vào của từng vòng, danh sách câu đã gán có đủ số câu (hoặc đủ bộ, với VCNV) cho vòng đó hay không, và MUST chặn cứng việc mở vòng khi thiếu — không ép được, bất kể lệnh mở vòng đến từ UI hay gọi thẳng API (FR-033) — trong khi các vòng khác đủ câu vẫn mở bình thường. *(US-005, US-007; `GR-031` C3; `QĐ-042`; AC US5-2, US7-2)*
- **FR-017**: Đối với vòng Vượt chướng ngại vật, phép kiểm pre-flight MUST đếm theo **bộ nguyên vẹn** (1 Chướng ngại vật + 4 hàng ngang + 1 ô trung tâm, cả sáu thành phần chưa dùng) thay vì đếm số câu rời; hệ thống MUST cảnh báo trước khi một thao tác của admin (chỉ định hàng ngang làm câu Câu hỏi phụ) sẽ làm vỡ một bộ. *(US-005; `GR-031` C3b, C3c; `QĐ-082`; AC US5-3, US5-4)*
- **FR-018**: Server MUST cho phép admin thêm hoặc bớt câu trong danh sách đã gán **khi contest chưa có trận nào đang chạy** — tức ở bề mặt **dựng contest**, trước khi trận đầu tiên bắt đầu hoặc giữa hai trận — kiểm tại server, bất kể request đến từ UI hay gọi thẳng API (FR-033), và MUST chạy lại pre-flight ngay sau mỗi lần sửa. Việc sửa danh sách của một **trận đang tồn tại, tại `LOBBY` giữa các vòng** thuộc EPIC-005 (`specs/005` FR-025 → FR-028) — hai bề mặt khác nhau ở hai thời điểm không giao nhau (`QĐ-161`). *(US-005; `GR-031` C5, C8; AC US5-5, US5-8)*
- **FR-019**: Server MUST NOT cho phép gỡ khỏi danh sách đã gán một câu đã hiển thị cho thí sinh trong bất kỳ trận nào của contest, bất kể qua UI hay gọi thẳng API (FR-033). *(US-005; `GR-031` C6; `QĐ-044`; AC US5-6)*
- **FR-020**: Hệ thống MUST cho phép gỡ khỏi danh sách đã gán một câu đã rút nhưng chưa hiển thị, và MUST coi câu đó là chưa tiêu (trả lại pool hiệu dụng của kho đề). *(US-005; `GR-031` C7; AC US5-7)*
- **FR-021**: Khi tính pool khả dụng cho một trận `practice` chạy trong một contest `official`, hệ thống MUST đảo chiều phép lọc — chỉ gồm câu đã có `usedInContest = true`; nếu contest chưa từng chạy trận nào, pool đó MUST rỗng và mọi vòng của trận practice đó tự chặn ở cửa vào. *(US-005; `GR-031` C9; AC US5-9)*
- **FR-022**: Khi admin thêm vào danh sách đã gán một câu mang `everPublic = true` cho một contest `official`, server MUST chặn cứng thao tác đó — bất kể qua UI hay gọi thẳng API (FR-033) — chỉ ép được bằng xác nhận hai bước kèm audit, và server MUST xác thực đủ hai bước xác nhận trước khi ghi. *(US-005; JOURNEY-002 alternative flow; AC US5-10)*
- **FR-023**: Hệ thống MUST cho phép admin chọn một chủ đề hiển thị cho contest; cấu hình này là **tuỳ chọn (SHOULD)**, MUST NOT chặn tạo hoặc bắt đầu contest nếu admin chưa chọn. *(US-006; không có `PRD-REQ` số hoá riêng — xem `OQ-001`; GOAL-008; AC US6-1, US6-6)*
- **FR-024**: Hệ thống MUST cho phép admin tải lên âm thanh riêng cho từng slot hiệu ứng của contest, trong ngưỡng kích thước đọc từ cấu hình môi trường; cấu hình này là **tuỳ chọn (SHOULD)**, MUST NOT chặn tạo hoặc bắt đầu contest nếu admin chưa cấu hình. *(US-006; không có `PRD-REQ` số hoá riêng — xem `OQ-001`; `CLAUDE.md` §Quy ước code; AC US6-2, US6-4, US6-6)*
- **FR-025**: Hệ thống MUST để một slot âm thanh chưa được gán file phát ra im lặng — MUST NOT có bộ hiệu ứng âm thanh mặc định nào tự động điền vào slot trống khi admin không thao tác gì. *(US-006; `CLAUDE.md` §Sound; AC US6-3, US6-6)*
- **FR-031**: Hệ thống MUST cung cấp các gói chủ đề/âm thanh dựng sẵn (preset) mà admin chọn nguyên gói bằng một thao tác, thay cho việc phải tự cấu hình từng phần; admin MUST vẫn sửa lại được từng phần sau khi áp preset. Preset MUST chỉ có hiệu lực khi admin **chủ động chọn** — hệ thống MUST NOT tự điền một gói mặc định vào contest nào (FR-025). *(US-006; GOAL-008; AC US6-5, US6-6)*
- **FR-026**: Tập câu khả dụng của một vòng MUST bất biến trong suốt thời gian vòng đó đang chạy — không tồn tại tình huống "cạn kho giữa vòng". *(US-007; `GR-031` C4, C8; AC US7-3)*
- **FR-028**: Server MUST cho phép admin sửa RuleConfig cấp contest bất cứ khi nào trận hiện tại của contest đó ở `LOBBY` hoặc đã `FINISHED`; sửa đổi đó MUST NOT tác động tới bản cấu hình đã đóng băng vào một trận đã bắt đầu hoặc đã đóng sổ, và MUST chỉ áp cho trận kế tiếp. Server MUST chặn sửa RuleConfig cấp contest trong lúc một vòng của trận hiện tại đang chạy, bất kể request đến từ UI hay gọi thẳng API (FR-033); lần bị chặn MUST để RuleConfig nguyên trạng. *(US-002; `GR-031` C8 (cùng cửa khoá với danh sách câu); `QĐ-075`; AC US2-10, US2-11)*

### Key Entities

- **Contest**: Đơn vị tổ chức bao trùm, là bản thiết kế chứ không phải một lần chạy — gồm cấu hình luật gốc (RuleConfig theo từng vòng), mode trả lời, danh sách câu đã gán, mã phòng 6 số, phạm vi no-repeat, ghế và vị trí, chủ đề, và cấu hình âm thanh. Một contest chứa nhiều trận nhưng chỉ một trận đang chạy tại một thời điểm.
- **RuleConfig**: Tập giá trị luật cho một vòng — thời gian suy nghĩ, số câu, mức điểm — có thể là preset `O26_DEFAULT@1` hoặc tuỳ chỉnh; ba giá trị (`rowCount`, playlist, `tieBreakPositions`) bị khoá cứng ở v1 — server từ chối mọi ghi giá trị khác khoá cứng (zero-trust), giao diện chỉ ẩn lựa chọn làm lớp UX — trong khi schema/kiểu dữ liệu vẫn có sức chứa cho giá trị rộng hơn, chuẩn bị cho phiên bản sau.
- **Ghế · Vị trí (Seat · Position)**: Ghế là chỗ dự thi trong một trận; vị trí là số thứ tự 1..N gán thủ công trước trận, bất biến sau khi vòng đầu tiên đã chạy. Cả **mô hình dữ liệu lẫn giao diện** nhận 1-12 ghế ngay ở v1 (`PRD-REQ-026`, `NON-GOAL-012`, `QĐ-007`); riêng **cú bắt đầu trận** đòi **tối đa 4** vì v1 chưa có thang điểm cho hơn 4 đơn vị điểm; dưới 4 thì bù bằng **ghế bỏ thi** (FR-030, `PRD-REQ-114`).
- **Danh sách câu đã gán (Assigned question list)**: Snapshot câu hỏi admin chọn từ kho đề cho một contest, dùng làm nguồn rút đề của mọi trận trong contest đó; sửa được ở `LOBBY`, khoá dần theo câu đã hiển thị.
- **Mode trả lời (Answer mode)**: Thuộc tính cấp contest — sân khấu hoặc nhập liệu — áp cho toàn bộ vòng, tách biệt khỏi RuleConfig, được chụp vào trận lúc bắt đầu.
- **Chủ đề · Âm thanh (Theme · Sound)**: Cấu hình trình diễn của contest — một chủ đề hiển thị, và tập file âm thanh gán theo từng slot hiệu ứng, slot trống là hợp lệ (im lặng).

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: Admin tạo xong một contest có cấu hình luật đầy đủ, khớp 100% bảng luật O26, chỉ bằng đúng một thao tác bấm ("Áp dụng luật 2026") sau khi tạo contest.
- **SC-002**: 0% contest tạo qua giao diện v1 mang giá trị `rowCount` ≠ 4, playlist khác bốn vòng chuẩn, hoặc `tieBreakPositions` ≠ `[1]` — ba khoá cứng không có đường lách nào trên UI.
- **SC-003**: 100% lần admin thử mở một vòng thiếu câu (kể cả VCNV thiếu bộ nguyên vẹn) bị chặn kèm thông báo đúng số câu/bộ còn thiếu, đồng thời các vòng đủ câu khác của cùng trận vẫn mở được bình thường trong cùng phiên thao tác.
- **SC-004**: Admin hoàn thành toàn bộ luồng dựng contest (tạo → áp preset → tuỳ chỉnh luật → chọn mode → gán ghế → chọn danh sách câu → chủ đề/âm thanh) mà không phải rời khỏi luồng contest builder để thao tác ở màn hình khác cho bất kỳ bước nào thuộc PRD-REQ-020…026, PRD-REQ-105.
- **SC-005**: Sau khi vòng đầu tiên của một trận đã chạy, 0% thao tác đổi vị trí ghế thành công — kiểm tra bằng cách thử đổi vị trí ở mọi trạng thái trận đã qua `LOBBY` lần đầu.

## Assumptions

- Admin dựng contest là cùng một tài khoản với admin sẽ vận hành trận sau này, hoặc ít nhất mang cùng vai `Quản trị` — contest builder không phân biệt "admin thiết kế" với "admin vận hành" như hai vai khác nhau (khớp `TERM-004`, `TERM-062`: đúng một vai Quản trị).
- Kho đề đã có sẵn câu `ACTIVE` đủ dùng trước khi admin bắt đầu chọn danh sách câu — việc soạn và duyệt câu hỏi thuộc EPIC-002, là điều kiện tiên quyết chứ không phải một bước của feature này.
- "Đặt trước trận" và "trước khi vòng đầu tiên chạy" được coi là cùng một mốc thời gian cho mục đích khoá vị trí ghế (US-004), khớp cách `docs/PRD.md` §7 ACTOR-001 và `game-state-machine.md` STATE-001 mô tả ranh giới "chưa vòng nào từng chạy".
- Ngưỡng kích thước media (ảnh/video/audio) và giới hạn khác đọc từ cấu hình môi trường đã tồn tại sẵn ở tầng hạ tầng (theo `CLAUDE.md` §Quy ước khác) — feature này chỉ tiêu thụ ngưỡng đó, không định nghĩa lại.

## Out of Scope

- **EPIC-002 — Kho đề và bộ đề**: soạn câu hỏi, media, vòng duyệt `DRAFT`→`ACTIVE`, tìm kiếm/lọc/sắp xếp nội bộ kho đề (chỉ dùng lại ở đây, không định nghĩa lại).
- **EPIC-003 — Nhập/xuất và gói contest**: xuất/nhập contest sang bản portable, danh sách người tham gia mã hoá, phiếu tài khoản.
- **EPIC-005 — Phòng thi và vòng đời trận**: đóng băng cấu hình tại cú bấm bắt đầu trận, LOBBY như trạng thái vận hành, bốn cửa ra của vòng, chốt trận, Câu hỏi phụ vận hành thực tế, `FINISHED`.
- **EPIC-006 — Game engine và luật thi đấu**: thực thi luật trong lúc thi — chấm điểm, hàng đợi tín hiệu, mốc thời gian do admin bấm, rút đề thời gian thực, hoàn nguyên.
- **EPIC-007 — Điều khiển và can thiệp của admin**: chạy lại/bỏ vòng, điều chỉnh điểm thủ công, xử lý sự cố giữa trận.
- **NON-GOAL-011** (playlist tuỳ ý), **`QĐ-068`** (rowCount ≠ 4 vận hành), **NON-GOAL-017** (phân định hoà ở vị trí khác Nhất) — feature này chỉ có trách nhiệm **khoá cứng ba lựa chọn đó ở giao diện tạo contest**, không có trách nhiệm hiện thực hoá chúng.
- **NON-GOAL-012** (luật cho số ghế **trên** 4) — **không cùng khuôn với ba khoá cứng trên**: nó chỉ loại **đường xử lý luật**, và khai thẳng rằng v1 vẫn hỗ trợ **lưu trữ và giao diện** cho 1-12. Feature này vì thế **mở** bước gán ghế cho 1-12 (FR-013, FR-030) và chỉ dựng hàng rào ở cửa bắt đầu trận cho **số ghế > 4**. Cơ chế **ghế bỏ thi** cho trận dưới 4 ghế (`PRD-REQ-114`, `QĐ-105`) thuộc EPIC-005 và EPIC-006.
- **NON-GOAL-019** (trình hướng dẫn cài đặt lần đầu trên trình duyệt) và mọi cấu hình cấp hệ thống/triển khai (EPIC-012).

## Open Questions

Không có.

## Traceability Matrix

| User story | PRD requirement | Game rules | Functional requirements | Acceptance scenarios |
|---|---|---|---|---|
| US-001 — Tạo contest và áp preset | PRD-REQ-020, PRD-REQ-021 | `GR-001`…`GR-025` (toàn bộ) | FR-001, FR-002 | US1-1, US1-2, US1-3, US1-4 |
| US-002 — Tuỳ chỉnh luật trong khuôn khổ ba khoá cứng | PRD-REQ-020, PRD-REQ-022, PRD-REQ-024, PRD-REQ-025, PRD-REQ-105 | `GR-001`…`GR-025`, `GR-009` C11, `GR-020`, `GR-022`, `GR-023`, `GR-025`, `GR-030`, `GR-031` C8 | FR-003, FR-004, FR-005, FR-006, FR-007, FR-008, FR-027, FR-028, FR-032, FR-033 | US2-1…US2-11 |
| US-003 — Mode trả lời cấp contest | PRD-REQ-023 | `GR-007`, `GR-017`, `GR-021` | FR-009, FR-010, FR-011 | US3-1…US3-4 |
| US-004 — Gán ghế và vị trí | PRD-REQ-026 | `GR-016`, `GR-007` | FR-012, FR-013, FR-014, FR-029, FR-030, FR-033 | US4-1, US4-2, US4-3, US4-4, US4-5, US4-6 |
| US-005 — Chọn danh sách câu hỏi | (JOURNEY-002; `GR-031` không mang `PRD-REQ` riêng — xem `traceability.md` §Xuyên vòng, dòng `GR-031`) | `GR-031` (toàn bộ, C1–C9) | FR-015, FR-016, FR-017, FR-018, FR-019, FR-020, FR-021, FR-022, FR-033 | US5-1…US5-10 |
| US-006 — Chủ đề và âm thanh | (GOAL-008, phạm vi liệt kê trong EPIC-004 §11; không có `PRD-REQ` số hoá — xem `OQ-001`) | — | FR-023, FR-024, FR-025, FR-031 | US6-1…US6-6 |
| US-007 — Pre-flight kho đề | (`QĐ-042`; cùng nền `GR-031` với US-005) | `GR-031` C3, C3b, C3c, C4, C8, §Đồng thời | FR-016, FR-017, FR-026, FR-033 | US7-1…US7-4 |

**Ghi chú truy nguyên**: US-005 và US-007 dùng chung một quy tắc chuẩn tắc (`GR-031`) nhưng tách thành hai user story vì hai lát cắt giá trị khác nhau — US-005 là hành động của admin (chọn/sửa danh sách), US-007 là hành vi hệ thống (chặn cứng tại cửa vào vòng) — cả hai vẫn kiểm thử độc lập được theo đúng yêu cầu "vertical slice".
