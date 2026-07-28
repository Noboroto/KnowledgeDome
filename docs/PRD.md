# Product Requirements Document

> **Phạm vi tài liệu.** PRD này đặc tả **phiên bản 1.0** của sản phẩm: mục tiêu, actor, hành trình, epic và yêu cầu cấp sản phẩm của đúng phiên bản đó. Nó KHÔNG chứa framework, mô hình dữ liệu, thiết kế API, thiết kế lớp hay code, và KHÔNG chứa user story chi tiết — user story thuộc bước `/speckit.specify`.
>
> Hạng mục nằm ngoài phạm vi phiên bản này được ghi ở `docs/roadmap-post-v1.md`; mọi mã định danh trong tài liệu này liền mạch và chỉ trỏ vào chính nó.
>
> **Nguồn.** Mọi yêu cầu ở đây truy nguyên về `docs/source/`, `docs/decisions.md`, `docs/game-rules.md`, `docs/game-state-machine.md`, `docs/glossary.md`, `docs/traceability.md`, `docs/product-discovery.md`. `plans/**` và `docs/reviews/**` **không phải nguồn**.

---

## 1. Document Status

| Trường | Giá trị |
|---|---|
| **Version** | 2.1.0 — *xoá mọi mục đã chốt và mọi ghi chép về mâu thuẫn đã sửa; đánh số lại `ASSUMPTION-*`, `METRIC-*`* |
| **Quy ước bảo trì** | Mục **đã chốt / đã đóng / đã sửa** bị **xoá khỏi tài liệu**, không giữ lại dưới dạng ghi chú lịch sử; dãy số liên quan được **đánh lại cho liền**. Lịch sử tra ở `git log` và `docs/reviews/` |
| **Đặc tả cho** | **Phiên bản sản phẩm 1.0** |
| **Status** | Bản đầu tiên — chờ chủ dự án phê duyệt |
| **Last updated** | 2026-07-29 |
| **Open conflict count** | **0** |
| **Open clarification count** | **8** |

### Source documents

| Nguồn | Vai trò | Trạng thái khi đọc |
|---|---|---|
| `docs/source/fandom-olympia-26-luat-choi.md` | Luật gốc O26 nguyên văn (snapshot 2026-07-23) | Không sửa; source of truth **duy nhất về LUẬT** |
| `docs/decisions.md` | 82 quyết định `QĐ-001`→`QĐ-082` | Source of truth về **lựa chọn sản phẩm**; §N khai *"không còn mục treo nào"* |
| `docs/game-rules.md` | 37 rule `GR-001`→`GR-037`, 23 nguyên tắc nền, 4 bảng dùng chung | Chuẩn tắc; **không còn marker treo** |
| `docs/game-state-machine.md` | 42 `STATE-*`, 47 `EVENT-*`, 90 `T-*`, 22 `INV-*`, 8 sơ đồ | Chuẩn tắc; **không còn marker treo** |
| `docs/glossary.md` | 58 thuật ngữ `TERM-001`→`TERM-058` | Chuẩn tắc về tên gọi |
| `docs/traceability.md` | Ma trận rule ↔ luật gốc ↔ `QĐ-*`; biến thể bị loại; biến thể ngoài O26 | Không đặt ra luật |
| `docs/product-discovery.md` | Vấn đề, người dùng, mục tiêu, hành trình, epic, giả định, cách đo | Đầu vào cho PRD; **tự khai không phải PRD** |
| `docs/reviews/game-rules-review.md` | Rà soát lượt 3, `GRR-137`→`GRR-171` (2026-07-25) | **Kho lưu, KHÔNG phải requirement** — dùng để phát hiện câu hỏi mở |
| `CLAUDE.md` · `.specify/memory/constitution.md` | Ràng buộc kỹ thuật và cổng chất lượng có hiệu lực ngay | Tham chiếu, không sao chép |

---

## 2. Product Overview

**KnowledgeDome** là **nền tảng web tổ chức thi đấu gameshow kiến thức tuỳ biến** theo mô hình *Đường lên đỉnh Olympia*, chạy luật mùa **O26** làm preset mặc định.

**Loại trò chơi.** Trận đấu kiến thức nhiều vòng, thời gian thực, có người dẫn chương trình và khán giả. Một trận `official` gồm bốn vòng theo thứ tự cố định — **Khởi động → Vượt chướng ngại vật → Tăng tốc → Về đích** (`QĐ-069`) — cộng nhánh phân định **Câu hỏi phụ** khi có hoà điểm ở vị trí cần phân định (`QĐ-055`, `TERM-020`). Bốn thí sinh cá nhân tranh điểm bằng cách bấm chuông giành quyền hoặc gõ đáp án; điểm được cộng, trừ, và chuyển giữa các thí sinh tuỳ vòng.

**Giá trị chính.**

1. **Luật là dữ liệu, không phải code.** Mọi thời gian, số câu và mức điểm khai trong cấu hình luật; một nút *"Áp dụng luật 2026"* áp preset `O26_DEFAULT@1`, và mọi giá trị vẫn sửa được cho từng contest (`TERM-051`, `product-discovery.md` §3 G-2, G-3).
2. **Người phán quyết, máy giữ sự kiện.** Máy **không bao giờ** tự chấm Đúng/Sai; nó chỉ hiển thị bài làm cạnh đáp án và tô khác biệt ký tự để admin quyết (`QĐ-001`, `QĐ-010`, `GR-026`, `INV-003`).
3. **Công bằng đo được.** Server time là nguồn sự thật duy nhất cho hạn chót, thứ tự chuông và thứ hạng tốc độ, ở độ phân giải mili-giây, biên đóng (`GR-035`, `INV-004`, `INV-005`).
4. **Đề kín tới đúng lúc, rồi công bố.** Trước mốc **câu khép**, đáp án chỉ rời server tới admin và MC; từ mốc đó nó được công bố cho thí sinh, khán giả và lớp phủ dựng stream (`GR-037`, `INV-017`, `QĐ-080`, `TERM-057`).
5. **Sửa được sai lầm mà không mất lịch sử.** Điểm là hàm của nhật ký sự kiện append-only; hoàn nguyên là **thêm** sự kiện đảo ngược (`GR-028`, `INV-001`, `INV-002`, `TERM-023`).
6. **Lên hình được.** Màn khán giả, lớp phủ cho phần mềm dựng stream, màn MC chữ lớn, chủ đề và âm thanh tuỳ chỉnh (`product-discovery.md` §3 G-8).

---

## 3. Problem Statement

Trường học và câu lạc bộ muốn tổ chức thi đấu theo format Olympia hiện chỉ có hai lựa chọn: **PowerPoint thủ công**, hoặc một **phần mềm desktop LAN cũ**. Tám hạn chế cụ thể (`product-discovery.md` §1):

| # | Hạn chế |
|---|---|
| PS-1 | Luật hard-code — đổi luật là sửa code rồi build lại |
| PS-2 | Không chạy online: chỉ LAN, giao tiếp thô |
| PS-3 | Không có kho đề dùng lại — file rời, media theo mã băm trên đĩa |
| PS-4 | Không tích hợp livestream |
| PS-5 | Mất kết nối là hỏng trận — không có cơ chế nối lại |
| PS-6 | "Bảo mật" đề chỉ là dịch ký tự |
| PS-7 | Chấm điểm hoàn toàn thủ công, không có công cụ đối chiếu đáp án |
| PS-8 | Trạng thái toàn cục dùng chung ⇒ không chạy được hai trận song song |

**Phát biểu cô đọng.** Người tổ chức không có công cụ nào vừa **(a)** cho tuỳ biến luật, **(b)** giữ kín đề tới đúng lúc công bố, **(c)** chạy realtime công bằng, và **(d)** lên hình livestream được (`product-discovery.md` §1).

**Chỗ chưa được chứng minh.** Không có nghiên cứu người dùng nào; toàn bộ phát biểu vấn đề rút từ suy luận và đối chiếu hệ thống tiền lệ. Chưa biết có bao nhiêu đơn vị thực sự chờ sản phẩm này, và ai là người dùng đầu tiên đã cam kết (`product-discovery.md` §1 · `ASSUMPTION-001`).

---

## 4. Goals

| ID | Mục tiêu | Nguồn |
|---|---|---|
| **GOAL-001** | **Một engine, hai mục đích** — contest chính thức và luyện tập, phân biệt bằng mục đích trận | `product-discovery.md` §3 G-1 · `QĐ-040` |
| **GOAL-002** | **Nền tảng tuỳ biến** — mọi thời gian, số câu, mức điểm là cấu hình; v1 khoá cứng playlist 4 vòng, 4 hàng ngang, luật cho đúng 4 thí sinh; dữ liệu và giao diện làm cho 1-12 ngay từ đầu | §3 G-2 · `QĐ-007`, `QĐ-068`, `QĐ-069` |
| **GOAL-003** | **Nút "Áp dụng luật 2026"** áp preset `O26_DEFAULT@1` | §3 G-3 · `TERM-051` |
| **GOAL-004** | **Điểm độc lập thời gian** — thời lượng là metadata từng câu, không suy ra từ mức điểm | §3 G-4 · `TERM-045` |
| **GOAL-005** | **Kho đề tập trung, bảo mật cao** — đáp án không bao giờ tới client trước lúc công bố | §3 G-5 · `GR-037`, `INV-017` |
| **GOAL-006** | **Realtime công bằng** — server time là nguồn sự thật duy nhất | §3 G-6 · `GR-035`, `INV-004` |
| **GOAL-007** | **Vận hành tin cậy** — hoàn nguyên được, phục hồi sau sự cố, audit đủ để phân xử | §3 G-7 · `GR-028`, `GR-036`, `QĐ-077` |
| **GOAL-008** | **Trình diễn** — màn khán giả, lớp phủ dựng stream, màn MC, chủ đề, âm thanh tuỳ chỉnh | §3 G-8 · `QĐ-049`, `QĐ-079` |
| **GOAL-009** | **Chuyển trọn gói** — soạn trên bản có Internet, nhập vào bản portable ngày thi | §3 G-9 |
| **GOAL-010** | **Hai hình thức triển khai** — máy chủ dựng bằng container, và bản portable chạy LAN trên Windows | §3 G-10 |

---

## 5. Non-Goals

| ID | Không thuộc phạm vi | Nguồn |
|---|---|---|
| **NON-GOAL-001** | **Máy tự chấm Đúng/Sai** — không tồn tại ở bất kỳ đâu, kể cả câu gõ máy. Bao trùm mọi non-goal khác | `QĐ-010` · `product-discovery.md` §3 · `traceability.md` §Biến thể bị loại |
| **NON-GOAL-002** | Nhận dạng giọng nói để tự chấm | `product-discovery.md` §3 |
| **NON-GOAL-003** | Truyền video — hệ thống chỉ đẩy dữ liệu cho phần mềm dựng stream | §3 |
| **NON-GOAL-004** | Giải đấu nhiều trận, bảng xếp hạng theo mùa | §3 |
| **NON-GOAL-005** | Đa ngôn ngữ — chỉ tiếng Việt | §3 · `CLAUDE.md` |
| **NON-GOAL-006** | Nhiều tổ chức trên một bản cài | §3 |
| **NON-GOAL-007** | Chặn sao chép đề | §3 |
| **NON-GOAL-008** | Tự chuyển mã media ở v1 | §3 |
| **NON-GOAL-009** | **Kick thí sinh khỏi trận ở v1** — thay bằng vô hiệu hoá / kích hoạt lại ghế | `QĐ-047`, `TERM-032` |
| **NON-GOAL-010** | **Chính sách dropout tự động** — quá ngưỡng chờ, hệ thống chỉ tô nổi bật; admin quyết | `QĐ-045`, `TERM-043` |
| **NON-GOAL-011** | **Playlist tuỳ ý ở v1** — bốn vòng chuẩn, đúng thứ tự; không lặp, không đổi, không bớt ở thiết kế contest | `QĐ-069` |
| **NON-GOAL-012** | **Luật cho số ghế ≠ 4** — v1 chỉ hỗ trợ **lưu trữ và giao diện** cho 1-12; đường xử lý luật chỉ có cho đúng 4 | `QĐ-007` |
| **NON-GOAL-013** | **Thi đội** — v1 chỉ thi cá nhân; đơn vị điểm cấp đội không thuộc phạm vi | `product-discovery.md` §5 E-14 |
| **NON-GOAL-014** | **Cơ chế "drop" tín hiệu** — mọi tín hiệu đã tới server đều có outcome | `QĐ-020`, `INV-006` |
| **NON-GOAL-015** | **Đóng băng đồng hồ** — cửa sổ đã mở thì chạy hết theo server time | `QĐ-030`, `INV-016` |
| **NON-GOAL-016** | **Sửa trận đã đóng sổ** — `FINISHED` là terminal và niêm phong | `QĐ-037`, `STATE-008` |

---

## 6. Target Users

| Nhóm | Mô tả | Nguồn |
|---|---|---|
| **Người tổ chức thi đấu ở trường học và câu lạc bộ** | Nhóm chính. Cần dựng contest, chuẩn bị đề, vận hành trận trước khán giả, và lên hình | `product-discovery.md` §1, §2 |
| **Người soạn đề của đơn vị tổ chức** | Xây và duy trì kho đề dùng lại được — trực tiếp giải `PS-3` | §1, §2 |
| **Học sinh dự thi** | Người dùng cuối trong trận; cần đăng nhập nhanh, phản hồi tức thì, và thấy đủ bối cảnh để chơi | §2 A-3 |
| **Khán giả tại chỗ và trực tuyến** | Truy cập public chỉ bằng mã phòng 6 số, không tài khoản, không chờ duyệt | §2 A-5 · `TERM-007` |
| **Ekip sản xuất livestream** | Cần lớp phủ nền trong suốt để dựng hình | §2 A-6 |

**Vai gián tiếp** (`product-discovery.md` §2): ban tổ chức / trọng tài quan tâm công bằng và phân xử khiếu nại; đơn vị tự host chịu trách nhiệm pháp lý về dữ liệu; phụ huynh quan tâm việc con lên hình. **Ban giám khảo không phải một vai riêng** — họ không thao tác hệ thống, nên trùng với ban tổ chức về mặt hệ thống.

---

## 7. Actors

> **Mô hình vai** (`QĐ-065`, `product-discovery.md` §2): **vai hệ thống ≠ vai vận hành**. Tài khoản mang vai hệ thống; *MC*, *người phụ trách luyện tập*, *host* là **quyền gán theo contest**. Một tài khoản giữ được nhiều vai. **Quyền điều khiển gắn với PHIÊN, không với tài khoản** (`QĐ-008`).

### ACTOR-001 — Admin (người vận hành)

- **Role**: Cảm biến và cơ cấu chấp hành **duy nhất** của hệ thống. Máy không quan sát được sân khấu, nên mọi mốc mà luật gốc mô tả bằng hành vi của MC đều ánh xạ thành một cú bấm của admin.
- **Goals**: Dựng contest · điều khiển trận · phán quyết Đúng/Sai · can thiệp khi có sự cố · duyệt đề `DRAFT` → `ACTIVE`.
- **Permissions**: Toàn quyền mở/đóng đáp án và ô chữ (`QĐ-048`); phán quyết một câu (`GR-026`); mốc thời gian (`GR-033`); duyệt/từ chối tín hiệu (`GR-032`); điều chỉnh điểm thủ công (`GR-029`); bỏ/chạy lại/kết thúc sớm vòng (`GR-030`); chốt và huỷ trận. **Mỗi thao tác phá huỷ là một permission RIÊNG, không suy ra từ "là admin"** (`QĐ-078`).
- **Constraints**: Đúng **một phiên** giữ quyền điều khiển tại một thời điểm; các phiên admin khác xem-không-bấm; chuyển quyền điều khiển ghi audit (`QĐ-008`). Không được đồng thời ngồi ghế thí sinh của contest đó (`QĐ-065`). Mọi thao tác không hoàn tác được đi qua dialog xác nhận (`QĐ-005`, `QĐ-072`).
- **Related journeys**: JOURNEY-001 → JOURNEY-007.
- **Source**: `QĐ-001`, `QĐ-004`, `QĐ-005`, `QĐ-008`, `QĐ-048`, `QĐ-078`, `TERM-004`.

### ACTOR-002 — Người ra đề (setter)

- **Role**: Soạn câu hỏi và bộ đề trong kho đề.
- **Goals**: Soạn câu của mình, gắn metadata và media, nhập/xuất phần đề phụ trách.
- **Permissions**: Tạo và sửa câu ở trạng thái `DRAFT`; đưa câu vào bộ đề. Làm một câu thành public **chỉ bằng cách đưa nó vào một bộ đề public** — không đặt trực tiếp được cờ hiển thị (`QĐ-063`).
- **Constraints**: Không điều khiển trận. Đề phải qua admin duyệt `DRAFT` → `ACTIVE` — **không có vai người duyệt riêng** (`QĐ-064`). Không được đồng thời ngồi ghế thí sinh của contest đó (`QĐ-065`).
- **Related journeys**: JOURNEY-001.
- **Source**: `TERM-006`, `QĐ-063`, `QĐ-064`, `QĐ-065`.

### ACTOR-003 — Thí sinh

- **Role**: Người dự thi, ngồi một ghế, tự trả lời và tự phát tín hiệu.
- **Goals**: Đăng nhập nhanh, bấm chuông, gõ đáp án, thấy phản hồi tức thì và thấy bảng điểm của tất cả các ghế.
- **Permissions**: Bấm chuông (`EVENT-037`); bấm *"Mở chướng ngại vật"* (`EVENT-038`); gửi đáp án (`EVENT-040`). Ở mode nhập liệu thêm: chọn hàng ngang (`EVENT-039`), chọn gói câu (`EVENT-041`), đặt Ngôi sao hy vọng (`EVENT-042`).
- **Constraints**: **Không có dialog xác nhận, không rút lại được** — ngoại lệ duy nhất là chọn hàng ngang ở mode nhập liệu (`QĐ-005`, `QĐ-019`). Không nhận đáp án chuẩn **trước mốc câu khép**; từ mốc đó thì nhận, nếu `revealAnswerAfterJudge` bật (`GR-037`, `QĐ-080`). Ở mode sân khấu, máy thí sinh **không render** nút chọn hàng ngang / chọn gói / Ngôi sao hy vọng, và server từ chối nếu tín hiệu lọt tới (`QĐ-019`).
- **Related journeys**: JOURNEY-004, JOURNEY-005.
- **Source**: `TERM-001`, `QĐ-005`, `QĐ-019`, `QĐ-023`.

### ACTOR-004 — MC (người dẫn chương trình)

- **Role**: **Thẩm quyền phán quyết trên sân khấu, thực hiện bằng NÓI.** Không thao tác hệ thống.
- **Goals**: Đọc câu hỏi và công bố kết quả trên sân khấu; cần màn riêng chữ rất to hiển thị câu hỏi **và đáp án**.
- **Permissions**: **Chỉ đọc.** Được xem đáp án không phụ thuộc cấu hình, mỗi lần xem vào audit (`GR-037`). Không có nút điều khiển nào (`QĐ-074`).
- **Constraints**: Màn `/mc` là **read-only tuyệt đối**; không tạo bề mặt quyền ghi mới cho MC (`QĐ-001`). Không được đồng thời ngồi ghế thí sinh của contest đó (`QĐ-065`). *Setter kiêm MC được phép* — hệ thống chỉ cảnh báo và ghi audit.
- **Related journeys**: JOURNEY-005.
- **Source**: `TERM-005`, `QĐ-001`, `QĐ-065`, `QĐ-074`, `GR-037`.

### ACTOR-005 — Khán giả (viewer)

- **Role**: Người xem, truy cập public chỉ bằng mã phòng 6 số — không tài khoản, không chờ duyệt.
- **Goals**: Xem sân khấu và bảng điểm theo thời gian thực.
- **Permissions**: **Read-only tuyệt đối** — server drop mọi sự kiện ghi từ kênh này. Thấy điểm số (điểm là thông tin công khai với mọi vai, `QĐ-015`).
- **Constraints**: Không nhận đáp án chuẩn trừ ngoại lệ `revealAnswerAfterJudge` (`GR-037`). **Không được báo gì về can thiệp của admin** — điểm và bàn cờ đổi đột ngột, không hiệu ứng, không giải thích; người giải thích là MC (`QĐ-076`). Không thấy khuyến nghị lượt. Có rate-limit và nút *"khoá cổng"* của admin.
- **Related journeys**: JOURNEY-004, JOURNEY-005.
- **Source**: `TERM-007`, `QĐ-015`, `QĐ-076`, `CLAUDE.md` §Mô hình truy cập.

### ACTOR-006 — Máy dựng stream (overlay)

- **Role**: Kênh public riêng cung cấp lớp phủ 1920×1080 nền trong suốt cho phần mềm dựng hình.
- **Goals**: Nhận dữ liệu trận để hiển thị chồng lên video.
- **Permissions**: Read-only, cùng mô hình truy cập với khán giả (mã phòng 6 số). **Nhận đáp án từ mốc câu khép**, cùng lúc và cùng điều kiện với khán giả (`QĐ-080`).
- **Constraints**: Không nhận đáp án **trước mốc câu khép** (`QĐ-080`).
- **Related journeys**: JOURNEY-004, JOURNEY-005.
- **Source**: `product-discovery.md` §2 A-6 · `QĐ-051`, `QĐ-080`, `GR-037`, `TERM-007`.

### ACTOR-007 — Server (tác nhân hệ thống)

- **Role**: Giữ **sự kiện và thời gian**. Không ai sửa được.
- **Goals**: Xác định hạn chót, thứ tự chuông, thứ hạng tốc độ; rút đề; phát hiện mất kết nối; tính điều kiện hoà.
- **Permissions**: Phát `EVENT-043` rút đề · `EVENT-044` hết giờ · `EVENT-045` phát hiện mất kết nối · `EVENT-046` hết grace · `EVENT-047` tính điều kiện hoà.
- **Constraints**: **Không transition nào sinh điểm mà không qua phán quyết của người** (`INV-003`). Hết giờ **không tự sinh kết quả** (`EVENT-044`). `EVENT-047` chỉ chạy trong lòng `EVENT-007`, không gọi độc lập được.
- **Related journeys**: mọi journey.
- **Source**: `QĐ-001`, `INV-003`, `INV-004`, `game-state-machine.md` §H.

---

## 8. Core Game Loop

> Nguồn: `game-state-machine.md` §7 *(core loop)*, Sơ đồ 1 và Sơ đồ 2 · `game-rules.md` §Bốn bảng dùng chung.

### 8.1 Vòng ngoài — cấp TRẬN

| # | Bước | State | Event | Game rule |
|---|---|---|---|---|
| 1 | Trận được tạo; chạy kiểm kho đề; gán ghế và vị trí; khán giả và lớp phủ vào phòng bằng mã 6 số | `STATE-001` LOBBY | — | `GR-031`, `GR-016` |
| 2 | Admin bắt đầu trận ⇒ **đóng băng cấu hình** (luật, mode trả lời, danh sách câu, cờ hiện đáp án), một lần cho cả đời trận, rồi mở vòng đầu | `STATE-001` → vòng đầu (`T-001`) | `EVENT-001` | `GR-031`, `GR-037` |
| 3 | Vòng chạy. **Admin chọn vòng nào bắt đầu**; playlist chỉ là gợi ý | một trong `STATE-002`…`STATE-006` | `EVENT-002` | `GR-030` · `INV-020` |
| 4 | Rời vòng về LOBBY qua đúng **bốn cửa ra**: kết thúc đủ câu · kết thúc khẩn cấp (giữ điểm) · bỏ vòng (revert) · chạy lại vòng (revert rồi chạy mới) | vòng → `STATE-001` (`T-010`…`T-014`) | `EVENT-003` / `004` / `005` / `006` | `GR-030` |
| 5 | Lặp bước 3-4 cho vòng kế. Giữa hai vòng admin điều chỉnh điểm, sửa danh sách câu, mở màn công bố | `STATE-001` | `EVENT-028`, `EVENT-031`, `EVENT-032` | `GR-029`, `GR-031` |
| 6 | Admin **chốt trận**. Server tính bảng điểm và tìm nhóm hoà **ngay tại cú bấm** | `STATE-001` → `STATE-007` hoặc `STATE-008` (`T-015`/`T-016`) | `EVENT-007` kéo theo `EVENT-047` | `GR-022` |
| 7a | Có hoà ở vị trí cần phân định ⇒ Câu hỏi phụ: 3 câu × 15 giây, giành quyền bằng chuông, **không cộng điểm** | `STATE-007` | `EVENT-037`, `EVENT-012` | `GR-023`, `GR-024` |
| 7b | Hết 3 câu chưa phân định ⇒ bốc thăm; server random, admin xác nhận | `STATE-007` → `STATE-016` → `STATE-001` (`T-018`, `T-019`) | `EVENT-025`, `EVENT-026`, `EVENT-027` | `GR-025` |
| 7c | Phân định xong ⇒ trận **về LOBBY, CHƯA đóng sổ**; ghi `TIE_BREAK_RESOLVED`. Còn cửa **bỏ vòng** để sửa phán quyết nhầm; admin sửa điểm làm nhóm hết bằng nhau thì kết quả tie-break **mất đối tượng** | `STATE-007`/`016` → `STATE-001` (`T-017`, `T-019`) | `EVENT-048` | `GR-022` C8 · `QĐ-083` |
| 7d | Admin bấm **chốt trận lần hai** ⇒ đóng sổ theo thứ hạng đã phân định; **không** vào lại Câu hỏi phụ | `STATE-001` → `STATE-008` (`T-016b`) | `EVENT-007`, `EVENT-047` | `GR-022` C7 |
| 8 | Trận đóng sổ, **niêm phong**. Van thoát duy nhất là tạo trận mới trong cùng contest | `STATE-008` FINISHED (terminal) | `EVENT-036` (`T-021`/`T-022`) | `GR-022` · `QĐ-037`, `QĐ-039` |
| — | **Bất cứ lúc nào**: huỷ trận ⇒ đóng sổ với nhãn *bỏ dở*, điểm giữ nguyên, **không có người thắng** | mọi state đang chạy → `STATE-008` (`T-020`) | `EVENT-008` | `GR-022` · `QĐ-038` |

### 8.2 Vòng trong — cấp CÂU (chạy bên trong mọi vòng)

| # | Bước | State | Event | Game rule |
|---|---|---|---|---|
| 1 | Server rút đề ngẫu nhiên **trong danh sách đã gán**, loại câu đã dùng. Câu **chưa tiêu**; cửa sổ đặt Ngôi sao hy vọng **mở** | `STATE-017` | `EVENT-043` | `GR-031`, `GR-021` |
| 2 | **Mốc 1 — admin hiển thị câu hỏi.** Đóng cửa sổ Ngôi sao hy vọng; đánh dấu câu **đã dùng**; mở cửa sổ chuông (chỉ ở Khởi động lượt chung). Khoảng sau mốc này là lúc **MC đọc** | `STATE-018` (`T-024`) | `EVENT-009` | `GR-033`, `GR-031`, `GR-003` |
| 3 | **Mốc 2 — admin start timer**, ánh xạ *"MC đọc xong câu hỏi"*. Ở Câu hỏi phụ đây cũng là mốc mở chuông | `STATE-019` (`T-025`) | `EVENT-010` | `GR-033`, `GR-024` |
| 3b | *(Chỉ câu thực hành)* **Mốc 3 — admin bắt đầu thực hành**: đóng pha suy nghĩ, mở pha thực hành | `STATE-014` (`T-066`) | `EVENT-011` | `GR-019` |
| 4 | Server đóng cửa nhận đáp án khi tới hạn. **Máy không tự chấm.** Bản quá hạn được giữ và tô đỏ | `STATE-020` (`T-026`) | `EVENT-044` | `GR-035` · bảng *Bản gửi quá hạn* |
| 5 | **Mốc 4 — admin phán quyết** Đúng / Sai / Huỷ kết quả. Sinh sự kiện điểm; xoá hàng đợi đang hoạt động; gỡ khoá chuông mọi ghế | `STATE-021` (`T-027`/`T-028`) | `EVENT-012` / `013` / `014` | `GR-026`, `GR-032` · `INV-009` |
| 6 | Còn câu ⇒ câu kế; đủ số câu theo luật ⇒ nút chuyển thành *kết thúc lượt / vòng* | `STATE-017` (`T-029`) hoặc `STATE-001` (`T-030`) | `EVENT-003` | `GR-026` · `INV-010`, `INV-012` |

**Ràng buộc bất khả xâm phạm của chuỗi này.** *Hiển thị câu hỏi* và *start timer* là **hai thao tác, thứ tự cố định, không gộp được** — khoảng giữa hai mốc chính là lúc MC đọc, và nó chứa cửa sổ chuông của Khởi động lượt chung cùng mốc đóng cửa sổ Ngôi sao hy vọng. Gộp lại là **phá luật** (`GR-033`, `QĐ-028`).

**Số câu theo luật** (`game-rules.md` §2.1): Khởi động lượt riêng **6**/thí sinh · Khởi động lượt chung **12** · VCNV **4 hàng ngang + 1 ô trung tâm** · Tăng tốc **4** · Về đích **3**/gói · Câu hỏi phụ **3**. Hệ thống **không bao giờ tự sinh câu thứ N+1** (`INV-012`).

**Ở VCNV có một nút thứ NĂM đứng TRƯỚC cả bốn mốc**: *mở hàng ngang* — thuộc trục trạng thái ô chữ, độc lập với vòng đời câu (`game-state-machine.md` Sơ đồ 7).

---

## 9. Game States

> Tóm tắt từ `docs/game-state-machine.md`. **Không sao chép toàn bộ máy trạng thái** — chi tiết transition, guard và bất biến ở tài liệu gốc.

### 9.1 Bảy thang bậc trạng thái

Từ *"state"* trong dự án này **luôn phải kèm thang bậc** (`TERM-018`). Có bảy thang, vòng đời khác nhau:

| Thang | Mã | Tính chất |
|---|---|---|
| **Trận** | `STATE-001`…`008` | Loại trừ lẫn nhau — đúng một giá trị |
| **Giai đoạn** | `STATE-009`…`016` | Nằm trong một trạng thái cấp trận |
| **Câu** | `STATE-017`…`021` | Vòng đời một câu hỏi |
| **Ghế** | `STATE-022`…`028` | **Cờ song song** — nhiều cái cùng đúng |
| **Tín hiệu** | `STATE-029`…`032` | Vòng đời một tín hiệu trong hàng đợi |
| **Lớp phủ** | `STATE-033`…`039` | Chồng lên trạng thái đang chạy, **không huỷ nó** |
| **Ô chữ** | `STATE-040`…`042` | Một giá trị cho mỗi ô bàn cờ VCNV (5 ô độc lập) |

### 9.2 Trạng thái cấp trận

| State | Nghĩa |
|---|---|
| `STATE-001` **LOBBY** | Trạng thái nghỉ; **cửa vào của mọi vòng và cửa ra duy nhất của mọi vòng**; nơi chạy kiểm kho đề và sửa danh sách câu |
| `STATE-002` Khởi động · lượt riêng | 6 câu/thí sinh, 3 giây, đúng +10 / sai 0 |
| `STATE-003` Khởi động · lượt chung | 12 câu, giành quyền bằng chuông, đúng +10 / sai hoặc bấm rồi im lặng −5 |
| `STATE-004` Vượt chướng ngại vật | 4 hàng ngang + 1 ô trung tâm; **vòng duy nhất hàng đợi CHẶN** |
| `STATE-005` Tăng tốc | 4 câu, 20/20/30/30 giây, luôn gõ máy, điểm theo thứ hạng tốc độ 40/30/20/10 |
| `STATE-006` Về đích | Mỗi thí sinh một lượt, gói 3 câu mức {20, 30}, cướp quyền, Ngôi sao hy vọng |
| `STATE-007` TIE_BREAK | 3 câu × 15 giây, giành quyền bằng chuông, **không cộng điểm** — chỉ đổi thứ hạng |
| `STATE-008` FINISHED | Đã đóng sổ, **niêm phong, chỉ đọc, terminal**; mang nhãn `hoàn thành` hoặc `bỏ dở` |

### 9.3 Trạng thái đáng chú ý ở các thang khác

- **Cấp câu**: `STATE-017` đã rút chưa hiển thị (**chưa tiêu**, gỡ ra thì trả lại kho; là chỗ **duy nhất** đặt được Ngôi sao hy vọng) → `018` đã hiển thị chưa chạy giờ (**mốc tiêu câu**) → `019` đang đếm giờ → `020` hết giờ chưa chấm → `021` đã chấm chờ chuyển câu.
- **Cấp ghế** (cờ song song): bị loại khỏi VCNV *(phạm vi vòng)* · mất kết nối trong grace *(120 giây)* · quá grace chờ admin · **bị vô hiệu hoá** *(phạm vi trận, cờ hành chính, đảo ngược được)* · Ngôi sao hy vọng đã dùng *(phạm vi lần chạy vòng)* · lượt chọn hàng ngang đã dùng · chuông đã khoá *(phạm vi câu)*.
- **Cấp tín hiệu**: chờ duyệt *(chỉ tồn tại ở VCNV)* → đã duyệt / bị từ chối; hoặc **trơ** *(được ghi nhận, không sinh hệ quả)*. Trạng thái cuối **không bao giờ bị xoá**.
- **Cấp lớp phủ**: công bố kết quả · banner kết nối · dialog xác nhận của admin · dialog cảnh báo lệch luật · toast invalid state · **dialog xác nhận phía thí sinh (ngoại lệ duy nhất)** · banner tạm dừng *(chặn toàn cục, không chữ, không phủ admin)*.
- **Cấp ô chữ**: **chờ → đã hỏi → mở**, đặt được cả hai chiều bởi admin. Băng điểm Chướng ngại vật là **hàm của trạng thái**, đọc theo **số hàng ngang không còn ở trạng thái chờ**, không phải bộ đếm cộng dồn; ô trung tâm **không** vào phép tính.

### 9.4 Ba hạng phản hồi khi một thao tác không đi được

| Hạng | Ép được? | Dùng khi |
|---|---|---|
| **Invalid state** (toast) | **Không** — nhánh **không tồn tại**, nút không bật | Thao tác không có nghĩa ở trạng thái hiện tại |
| **Cảnh báo lệch luật** (dialog Yes/No) | **Có** | Hệ thống khuyến nghị khác admin; admin luôn ép được |
| **Chặn cứng** | **Không** | **Đúng ba chỗ** (`INV-014`): cửa sổ cướp quyền cần ≥2 thí sinh · Câu hỏi phụ cần ≥2 thí sinh · cửa vào vòng thiếu câu |

### 9.5 Bất biến chi phối toàn hệ thống

`INV-001` lịch sử chỉ thêm không bao giờ mất · `INV-002` điểm luôn là hàm của nhật ký sự kiện · `INV-003` không sinh điểm mà không qua phán quyết của người · `INV-004` mọi mốc thời gian là server time · `INV-005` biên thời gian là **biên đóng** · `INV-006` mọi tín hiệu đã tới server đều có outcome · `INV-007` hàng đợi FIFO thuần theo server timestamp · `INV-008` từ chối không làm mất lượt · `INV-009` một câu đúng một phán quyết · `INV-010` phán quyết là điều kiện chuyển câu · `INV-011` câu đã hiển thị không bao giờ trả lại kho · `INV-012` số câu của một vòng là con số cố định · `INV-013` tín hiệu không giành quyền không gián đoạn đồng hồ · `INV-014` chỉ ba chỗ chặn cứng · `INV-015` đồng hồ khoá thí sinh không khoá admin · `INV-016` đồng hồ không bao giờ đóng băng · `INV-017` đáp án chỉ rời server tới admin và MC · `INV-018` điểm được phép âm · `INV-019` bị loại tước quyền không tước điểm · `INV-020` thứ tự vòng và lượt là khuyến nghị · `INV-021` lớp phủ không đổi trạng thái bên dưới · `INV-022` toàn bộ trạng thái trận nằm ở hai thứ — nhật ký sự kiện và vòng nào đang mở.

---

## 10. User Journeys

### JOURNEY-001 — Chuẩn bị kho đề

- **Actor**: ACTOR-002 người ra đề → ACTOR-001 admin
- **Starting state**: Kho đề trống hoặc chưa đủ cho contest sắp tới
- **Goal**: Có tập câu hỏi `ACTIVE` dùng được cho contest
- **Main flow**: Soạn câu theo ba kho (theo vòng) → gắn metadata *(mã hiển thị, lĩnh vực, số chữ, giải thích, ghi chú, thời lượng, mức điểm, kiểu nhập đáp án, phương án)* và media → lưu `DRAFT` → **admin duyệt** → `ACTIVE`
- **Alternative flows**: Admin trả về `DRAFT` kèm ghi chú · setter đưa câu vào **bộ đề public** ⇒ câu mang dấu vết `everPublic` một chiều và bị chặn khỏi trận official về sau
- **End state**: Câu ở `ACTIVE`, sẵn sàng để gán vào contest
- **Related game rules**: `GR-031`
- **Source**: `product-discovery.md` §4 J-1 · `QĐ-063`, `QĐ-064`, `QĐ-071`

### JOURNEY-002 — Dựng contest

- **Actor**: ACTOR-001 admin
- **Starting state**: Kho đề có câu `ACTIVE`
- **Goal**: Contest sẵn sàng bắt đầu trận
- **Main flow**: Tạo contest → bấm **"Áp dụng luật 2026"** → cấu hình từng vòng → chọn **mode trả lời** (cấp contest) → gán ghế và vị trí → **chọn danh sách câu hỏi** *(bắt buộc — hệ thống không tự lấy đề)* → chủ đề và âm thanh → chạy kiểm kho đề
- **Alternative flows**: Kiểm kho đề báo thiếu ⇒ quay lại chọn thêm câu · thêm câu mang `everPublic` ⇒ chặn cứng, ép được bằng xác nhận hai bước + audit
- **End state**: Contest ở trạng thái sẵn sàng; trận mới ở `STATE-001` LOBBY
- **Related game rules**: `GR-031`, `GR-017`
- **Source**: `product-discovery.md` §4 J-2 · `QĐ-017`, `QĐ-042`, `QĐ-071`, `TERM-051`

### JOURNEY-003 — Chuyển sang bản portable

- **Actor**: ACTOR-001 admin
- **Starting state**: Contest đã dựng xong trên bản có Internet
- **Goal**: Cùng contest chạy được trên máy portable ngày thi
- **Main flow**: Xuất gói contest → mang sang máy portable → nhập → contest ở dạng nháp → kiểm lại
- **Alternative flows**: Nhập thiếu media ⇒ báo lỗi và chỉ ra câu nào · cờ `everPublic` **đi theo câu** qua nhập/xuất, cờ đã-dùng thì **đặt lại** vì thuộc contest
- **End state**: Contest chạy được offline trên LAN
- **Related game rules**: `GR-031`
- **Source**: `product-discovery.md` §4 J-3 · `QĐ-071`, `TERM-048`, `TERM-049`

### JOURNEY-004 — Vào phòng

- **Actor**: ACTOR-003 thí sinh · ACTOR-005 khán giả · ACTOR-006 máy dựng stream
- **Starting state**: Trận ở `STATE-001` LOBBY
- **Goal**: Mọi vai kết nối đúng phòng và sẵn sàng
- **Main flow**: Admin phát **mã 6 số** → thí sinh đăng nhập rồi nhập mã → thử chuông, thử âm thanh, báo sẵn sàng → khán giả và lớp phủ nhập cùng mã, không cần tài khoản
- **Alternative flows**: Thí sinh vào nhầm ghế ⇒ admin gán lại *(chỉ khi chưa vòng nào từng chạy)* · admin khoá cổng phòng khán giả
- **End state**: Đủ ghế đã kết nối; trận bắt đầu được
- **Related game rules**: `GR-036`
- **Source**: `product-discovery.md` §4 J-4 · `CLAUDE.md` §Mô hình truy cập

### JOURNEY-005 — Thi đấu

- **Actor**: ACTOR-001 admin *(chính)*; ACTOR-003 thí sinh và ACTOR-004 MC *(phụ)*
- **Starting state**: `STATE-001` LOBBY, cấu hình đã đóng băng
- **Goal**: Chạy trọn các vòng và ra bảng điểm
- **Main flow**: Mở vòng → hiển thị câu → start timer → thí sinh bấm chuông hoặc gõ → admin phán quyết → câu kế. Lặp tới hết vòng, về LOBBY, mở vòng kế
- **Alternative flows**: Điều chỉnh điểm thủ công · hoàn nguyên · mở màn công bố kết quả · duyệt/từ chối tín hiệu ở VCNV · ép qua cảnh báo lệch luật khi đổi thứ tự lượt
- **End state**: Mọi vòng đã chạy; trận ở LOBBY chờ chốt
- **Related game rules**: `GR-001`…`GR-025`, `GR-026`, `GR-032`, `GR-033`
- **Source**: `product-discovery.md` §4 J-5

### JOURNEY-006 — Xử lý sự cố

- **Actor**: ACTOR-001 admin
- **Starting state**: Trận đang chạy, có sự cố
- **Goal**: Trận tiếp tục được, hoặc khép lại một cách có ghi chép
- **Main flow**: Nhận diện sự cố → chọn công cụ tương ứng → ghi lý do → tiếp tục
- **Alternative flows**: **Thí sinh rớt mạng** ⇒ giữ ghế 120 giây, quá ngưỡng thì tô nổi bật, admin quyết giữ hay gia hạn · **admin rớt mạng** ⇒ quay lại và khôi phục từ server; tài khoản admin khác tiếp quản được · **vòng hỏng** ⇒ bỏ vòng, chạy lại vòng, hoặc kết thúc khẩn cấp *(giữ nguyên điểm)* · **thiếu đề** ⇒ sửa danh sách câu ở LOBBY · **máy một ghế chết** ⇒ vô hiệu hoá ghế, sau đó kích hoạt lại
- **End state**: Trận chạy tiếp, hoặc đóng sổ với nhãn `bỏ dở`
- **Related game rules**: `GR-029`, `GR-030`, `GR-036`
- **Source**: `product-discovery.md` §4 J-6 · `QĐ-045`, `QĐ-047`, `QĐ-070`

### JOURNEY-007 — Sau trận

- **Actor**: ACTOR-001 admin
- **Starting state**: Mọi vòng đã chạy; trận ở LOBBY
- **Goal**: Kết quả được chốt, ghi lại và lưu trữ đúng hạn
- **Main flow**: Bấm **chốt trận** → server tính bảng điểm và điều kiện hoà → *(có hoà ⇒ Câu hỏi phụ ⇒ `TIE_BREAK_RESOLVED` ⇒ về `LOBBY` ⇒ bấm **chốt trận lần hai**)* → `FINISHED` → xuất biên bản → thống kê ghi ngược kho đề → **xuất gói** *(kết quả + nhật ký sự kiện · bản kê câu đã dùng · kết quả rút gọn)* → job dọn dữ liệu theo hạn
- **Alternative flows**: Huỷ trận ⇒ nhãn `bỏ dở`, không phân định thứ hạng · hoà ngoài phạm vi phân định ⇒ ghi **đồng hạng**, hạng kế nhảy qua · **trận chạy trên bản portable** ⇒ mang **bản kê câu đã dùng** về máy chủ trung tâm và nhập vào contest tương ứng; không có đường mang kết quả về
- **End state**: `STATE-008` FINISHED, niêm phong; biên bản đã xuất
- **Related game rules**: `GR-022`, `GR-023`, `GR-025`, `GR-028`, `GR-031`
- **Source**: `product-discovery.md` §4 J-7 · `QĐ-036`, `QĐ-037`, `QĐ-038`, `QĐ-077`, `QĐ-083`, `QĐ-084`

### Một hành trình chưa có

| Hành trình | Trạng thái |
|---|---|
| **Cài đặt lần đầu** | Mới có giải pháp kỹ thuật tạo admin bằng dòng lệnh, **chưa có trải nghiệm người dùng** — `QUESTION-002` |

*Nguồn: `product-discovery.md` §4.*

---

## 11. Epics

> Chia theo **user goal / game capability**, không theo tầng kỹ thuật. Mốc phát hành lấy từ `product-discovery.md` §5 và `CLAUDE.md` §Lộ trình version.

### EPIC-001 — Xác thực và phân quyền

- **Goal**: Đúng người thấy đúng thứ, và một trận luôn có đúng một người điều khiển.
- **Primary actor**: ACTOR-001 admin
- **User value**: Đáp án không rò; quyền điều khiển chuyển giao được khi admin đổi ca hoặc máy hỏng.
- **Scope**: Tài khoản và vai hệ thống · vai vận hành gán theo contest · ràng buộc loại trừ thí sinh ↔ admin/MC/setter · **một phiên giữ quyền điều khiển** + chuyển quyền có audit · permission riêng cho từng thao tác phá huỷ · truy cập public bằng mã phòng 6 số cho khán giả và lớp phủ.
- **Out of scope**: Nhiều tổ chức trên một bản cài (NON-GOAL-006); vai người duyệt đề riêng (`QĐ-064`).
- **Related journeys**: JOURNEY-001, JOURNEY-004
- **Related game rules**: `GR-037`
- **Dependencies**: —
- **MVP status**: **v1**
- **Source**: `product-discovery.md` §5 E-1 · `QĐ-008`, `QĐ-064`, `QĐ-065`, `QĐ-078`

### EPIC-002 — Kho đề và bộ đề

- **Goal**: Câu hỏi soạn một lần, dùng lại nhiều lần, không rò và không lặp.
- **Primary actor**: ACTOR-002 người ra đề
- **User value**: Giải `PS-3` và `PS-6` — kho tập trung, tìm kiếm được, và đề kín tới lúc công bố.
- **Scope**: Soạn câu với metadata và media · ba kiểu nhập đáp án + kênh thực hành · vòng duyệt `DRAFT` → `ACTIVE` · bộ đề · cờ hiển thị **dẫn xuất** và dấu vết `everPublic` một chiều · tìm kiếm toàn văn, lọc, sắp xếp · cờ đã-dùng theo contest.
- **Out of scope**: Đánh phiên bản khi sửa câu đã duyệt (`product-discovery.md` §8 nêu là mục cắt được); chặn sao chép đề (NON-GOAL-007).
- **Related journeys**: JOURNEY-001, JOURNEY-002
- **Related game rules**: `GR-031`, `GR-027`
- **Dependencies**: EPIC-001
- **MVP status**: **v1**
- **Source**: §5 E-2 · `QĐ-041`…`QĐ-044`, `QĐ-063`, `QĐ-066`, `QĐ-071`

### EPIC-003 — Nhập/xuất và gói contest

- **Goal**: Soạn ở nơi có Internet, thi ở nơi không có.
- **Primary actor**: ACTOR-001 admin
- **User value**: Giải `PS-2` cho ngày thi — mang trọn contest sang máy portable.
- **Scope**: Xuất và nhập gói contest gồm câu hỏi, metadata media, và media theo từng vòng · **định danh câu ổn định qua nhập/xuất** · giữ `everPublic` theo câu, đặt lại cờ đã-dùng theo contest · nhập/xuất phần đề của một setter · **đánh dấu "đã dùng" hàng loạt bằng tay** và **bản kê câu đã dùng xuất/nhập được** (`QĐ-084`).
- **Out of scope**: **Danh sách thí sinh trong gói contest** — **chưa có yêu cầu nguồn** (`QUESTION-001`). **Đồng bộ tự động kết quả portable → trung tâm** — `QĐ-084` chốt là **không làm**.
- **Related journeys**: JOURNEY-003
- **Related game rules**: `GR-031`
- **Dependencies**: EPIC-002
- **MVP status**: **v1**
- **Source**: §5 E-3 · `QĐ-071`

### EPIC-004 — Contest builder và luật tuỳ biến

- **Goal**: Đổi luật không cần lập trình viên.
- **Primary actor**: ACTOR-001 admin
- **User value**: Giải `PS-1` — mọi thời gian, số câu và mức điểm là cấu hình.
- **Scope**: Tạo contest · nút **"Áp dụng luật 2026"** áp preset · cấu hình từng vòng · chọn **mode trả lời** ở cấp contest · gán ghế và vị trí · chọn danh sách câu · chủ đề và âm thanh · kiểm kho đề trước khi bắt đầu.
- **Out of scope**: Playlist tuỳ ý (NON-GOAL-011); số hàng ngang ≠ 4 (`QĐ-068`); luật cho số ghế ≠ 4 (NON-GOAL-012).
- **Related journeys**: JOURNEY-002
- **Related game rules**: `GR-031`
- **Dependencies**: EPIC-002
- **MVP status**: **v1**
- **Source**: §5 E-4 · `QĐ-007`, `QĐ-017`, `QĐ-068`, `QĐ-069`, `QĐ-075`, `TERM-051`

### EPIC-005 — Phòng thi và vòng đời trận

- **Goal**: Một contest chạy được nhiều trận, mỗi trận có vòng đời rõ ràng và kết thúc có ghi chép.
- **Primary actor**: ACTOR-001 admin
- **User value**: Giải `PS-8` — trạng thái thuộc về trận, không phải biến toàn cục; và trận hỏng vẫn khép lại được.
- **Scope**: Contest là bản thiết kế, trận là một lần chạy · mã phòng 6 số · LOBBY là cửa vào và cửa ra của mọi vòng · đóng băng cấu hình tại cú bấm bắt đầu trận · bốn cửa ra của vòng · chốt trận và nhánh Câu hỏi phụ · huỷ trận với nhãn `bỏ dở` · `FINISHED` niêm phong · tạo trận mới trong cùng contest · sửa danh sách câu ở LOBBY.
- **Out of scope**: Sửa trận đã đóng sổ (NON-GOAL-016).
- **Related journeys**: JOURNEY-004, JOURNEY-005, JOURNEY-006, JOURNEY-007
- **Related game rules**: `GR-022`, `GR-030`, `GR-031`
- **Dependencies**: EPIC-004
- **MVP status**: **v1**
- **Source**: §5 E-5 · `QĐ-032`…`QĐ-039`, `QĐ-042`, `QĐ-043`

### EPIC-006 — Game engine và luật thi đấu

- **Goal**: Trận chạy đúng luật O26, và mọi con số truy nguyên được.
- **Primary actor**: ACTOR-007 server *(thi hành)*; ACTOR-001 admin *(phán quyết)*
- **User value**: Kết quả trận đúng và giải thích được — đây là lý do tồn tại của sản phẩm.
- **Scope**: Năm vòng và luật từng vòng · điểm là hàm của nhật ký sự kiện · hoàn nguyên bằng sự kiện đảo ngược · hàng đợi tín hiệu và quy tắc chặn/không chặn · server time là nguồn sự thật · rút đề và quy tắc không lặp câu · phạm vi hiển thị đáp án · mất kết nối và giữ ghế.
- **Out of scope**: Mọi đường máy tự chấm (NON-GOAL-001); luật đa ghế (NON-GOAL-012); luật thi đội (NON-GOAL-013).
- **Related journeys**: JOURNEY-005
- **Related game rules**: `GR-001` → `GR-037` *(toàn bộ)*
- **Dependencies**: EPIC-004, EPIC-005
- **MVP status**: **v1**
- **Source**: §5 E-6 · toàn bộ `game-rules.md` và `game-state-machine.md`

### EPIC-007 — Điều khiển và can thiệp của admin

- **Goal**: Người vận hành luôn có đường sửa mọi sai lầm mà không mất lịch sử.
- **Primary actor**: ACTOR-001 admin
- **User value**: Giải `PS-7` — có công cụ đối chiếu đáp án; và trận không bao giờ kẹt.
- **Scope**: Bốn mốc bấm của một câu · phán quyết Đúng / Sai / Huỷ kết quả · màn chấm với tô khác biệt ký tự và lịch sử bài gửi · duyệt và từ chối tín hiệu · điều chỉnh điểm thủ công có lý do bắt buộc · bỏ / chạy lại / kết thúc khẩn cấp vòng · mở và đóng đáp án, ô chữ, màn công bố, banner tạm dừng · vô hiệu hoá và kích hoạt lại ghế · ba hạng cảnh báo giao diện.
- **Out of scope**: Bất kỳ cơ chế nào tự cộng trừ điểm (NON-GOAL-001); tính lại thứ hạng tự động sau khi đổi phán quyết (`QĐ-014`).
- **Related journeys**: JOURNEY-005, JOURNEY-006
- **Related game rules**: `GR-026` → `GR-034`
- **Dependencies**: EPIC-006
- **MVP status**: **v1**
- **Source**: §5 E-7, §6 *(màn admin)* · `QĐ-048`, `QĐ-052`, `QĐ-072`, `QĐ-073`, `QĐ-074`

### EPIC-008 — Trải nghiệm thí sinh

- **Goal**: Thí sinh thi được nhanh, công bằng, và luôn biết mình đang ở đâu.
- **Primary actor**: ACTOR-003 thí sinh
- **User value**: Phản hồi tức thì và đủ bối cảnh — bảng điểm của **tất cả** các ghế, không chỉ điểm của mình.
- **Scope**: Bấm chuông chỉ nhận click chuột và tự khoá · nút gửi đáp án sống tới hết giờ · hai bố cục VCNV theo mode · ba trạng thái của nút chọn hàng ngang · chọn gói câu và Ngôi sao hy vọng ở mode nhập liệu · bảng điểm realtime gồm điểm âm · khôi phục sau mất kết nối giữa câu.
- **Out of scope**: Dialog xác nhận cho thao tác đua tốc độ (`QĐ-005`); nút "huỷ" phía thí sinh.
- **Related journeys**: JOURNEY-004, JOURNEY-005
- **Related game rules**: `GR-034`, `GR-036`, `GR-006`, `GR-015`
- **Dependencies**: EPIC-006
- **MVP status**: **v1**
- **Source**: §5 E-8, §6 *(màn thí sinh)* · `QĐ-015`, `QĐ-019`, `QĐ-023`, `QĐ-029`

### EPIC-009 — Trình diễn: khán giả · lớp phủ · MC · chủ đề · âm thanh

- **Goal**: Trận lên hình được và khán giả theo dõi được.
- **Primary actor**: ACTOR-005 khán giả · ACTOR-006 máy dựng stream · ACTOR-004 MC
- **User value**: Giải `PS-4` — tích hợp livestream mà không cần dựng thủ công.
- **Scope**: Màn khán giả public bằng mã 6 số · lớp phủ 1920×1080 nền trong suốt · màn MC chữ lớn có đáp án, read-only · lớp phủ công bố kết quả · banner tạm dừng · chủ đề · các khe âm thanh phủ cả sự kiện điều khiển.
- **Out of scope**: Truyền video (NON-GOAL-003); báo cho khán giả về can thiệp của admin (`QĐ-076`); đẩy khuyến nghị lượt xuống khán giả.
- **Related journeys**: JOURNEY-004, JOURNEY-005
- **Related game rules**: `GR-037`
- **Dependencies**: EPIC-006
- **MVP status**: **v1**
- **Source**: §5 E-9, §6 *(khán giả và lớp phủ)* · `QĐ-049`, `QĐ-050`, `QĐ-076`, `QĐ-079`

### EPIC-010 — Sau trận: kết quả, thống kê, phát lại

- **Goal**: Kết quả có thể in ra, phân xử được, và nuôi ngược lại kho đề.
- **Primary actor**: ACTOR-001 admin
- **User value**: Biên bản đủ để trả lời khiếu nại, và số liệu để cải thiện kho đề.
- **Scope**: Bảng điểm cuối và thứ hạng · biên bản in **theo lần chạy** kèm nhãn · thống kê ghi ngược kho đề · nhật ký thao tác đủ để phân xử.
- **Out of scope**: **Phát lại một trận** — ngoài phạm vi (`product-discovery.md` §5 E-10).
- **Related journeys**: JOURNEY-007
- **Related game rules**: `GR-022`, `GR-028`
- **Dependencies**: EPIC-006
- **MVP status**: **Thuộc phạm vi**, trừ phát lại
- **Source**: §5 E-10 · `QĐ-077`

### EPIC-011 — Dữ liệu cá nhân và quyền riêng tư

- **Goal**: Dữ liệu học sinh vị thành niên không tồn tại lâu hơn mức cần, và mọi thao tác đều có dấu vết.
- **Primary actor**: ACTOR-001 admin *(và đơn vị tự host)*
- **User value**: Đơn vị tổ chức chịu được trách nhiệm pháp lý về dữ liệu.
- **Scope**: Nhật ký thao tác chung, append-only, cho **mọi thao tác của mọi vai** · tuỳ chọn dùng biệt danh thay tên thật · disclaimer khi tải nhạc lên · hạn lưu trữ khác nhau theo mục đích trận · xuất biên bản trước khi dọn dữ liệu.
- **Out of scope**: **Job dọn dữ liệu tự động** — ngoài phạm vi (`product-discovery.md` §5 E-11). Ràng buộc *"dọn dữ liệu không được đụng biên bản đã xuất"* (`PRD-REQ-080`) vẫn thuộc phạm vi vì nó là điều kiện đặt lên job, không phải job.
- **Related journeys**: JOURNEY-007
- **Related game rules**: `GR-037`
- **Dependencies**: EPIC-001
- **MVP status**: **Thuộc phạm vi**, trừ job dọn tự động
- **Source**: §5 E-11, §3 *(pháp lý)*, §7 AS-5 · `QĐ-040`, `QĐ-077` · `CLAUDE.md` §Quy ước khác

### EPIC-012 — Vận hành và hai hồ sơ triển khai

- **Goal**: Cùng một sản phẩm chạy được ở phòng máy chủ và ở hội trường không có Internet.
- **Primary actor**: ACTOR-001 admin *(và người cài đặt)*
- **User value**: Giải `PS-2` — không bị khoá vào LAN, cũng không phụ thuộc Internet ngày thi.
- **Scope**: Hồ sơ máy chủ dựng bằng container · hồ sơ portable Windows chạy LAN offline · giới hạn kích thước media · rate-limit cổng khán giả và nút khoá cổng.
- **Out of scope**: Nhiều tổ chức trên một bản cài (NON-GOAL-006). **Trải nghiệm cài đặt lần đầu chưa có** (`QUESTION-002`).
- **Related journeys**: JOURNEY-003, JOURNEY-004
- **Related game rules**: —
- **Dependencies**: EPIC-003
- **MVP status**: **v1**
- **Source**: §5 E-12, §3 G-10 · `QĐ-067` · `CLAUDE.md` §Quy ước khác

### Epic chưa có yêu cầu nguồn

**Danh sách thí sinh trong gói contest** *(mã hoá, hợp nhất tài khoản trùng, phiếu tài khoản)* — chỉ tồn tại trong bản nháp `plans/**` ⇒ **không phải requirement**. Xem `QUESTION-001`. *(Nguồn: `product-discovery.md` §5.)*

---

## 12. Product Requirements

> **Quy ước.** Mỗi yêu cầu có đúng một `Status` ∈ {`CONFIRMED`, `NEEDS CLARIFICATION`, `CONFLICT`}. `Priority` ∈ {P1, P2, P3}: **P1** = không có thì không chạy được một trận · **P2** = cần nhưng trận vẫn chạy được nếu thiếu · **P3** = phần cắt được nếu phải cắt.
> **Acceptance intent** nói *nghiệm thu nhìn vào đâu*, không phải kịch bản kiểm thử — kịch bản `Given/When/Then` thuộc `specs/<feature>/spec.md`.

### EPIC-001 — Xác thực và phân quyền

**PRD-REQ-001 — Truy cập public bằng mã phòng 6 số cho khán giả và lớp phủ**
- **Description**: Hệ thống MUST cho khán giả và máy dựng stream vào phòng chỉ bằng mã phòng 6 số, không tài khoản, không chờ duyệt; và MUST thi hành read-only ở phía server bất kể client gửi gì.
- **Actor**: ACTOR-005, ACTOR-006 · **Related epic**: EPIC-001 · **Related journey**: JOURNEY-004 · **Related game rules**: `GR-037`
- **Priority**: P1 · **Rationale**: Khán giả là số đông và không thể phát tài khoản cho từng người; read-only phải ở server vì client public không đáng tin.
- **Acceptance intent**: Vào được bằng mã; mọi sự kiện ghi từ kênh này bị bỏ; không có đường nào nhận đáp án.
- **Source**: `CLAUDE.md` §Mô hình truy cập · `TERM-007` · `QĐ-051` · **Status**: CONFIRMED

**PRD-REQ-002 — Xác thực bắt buộc cho mọi vai có thể ghi hoặc thấy đáp án**
- **Description**: Thí sinh, MC, admin và người ra đề MUST đăng nhập bằng tài khoản; hệ thống MUST kiểm quyền ở server cho mọi yêu cầu, không phụ thuộc giao diện đã ẩn nút hay chưa.
- **Actor**: ACTOR-001…004 · **Related epic**: EPIC-001 · **Related journey**: JOURNEY-004 · **Related game rules**: `GR-037`
- **Priority**: P1 · **Rationale**: Mọi giao diện có thể gửi sự kiện hoặc thấy đáp án đều là bề mặt tấn công.
- **Acceptance intent**: Yêu cầu không có quyền bị từ chối ở server kể cả khi đến từ phiên đã vào phòng.
- **Source**: `CLAUDE.md` §Mô hình truy cập, §Nguyên tắc code · `QĐ-060` · **Status**: CONFIRMED

**PRD-REQ-003 — Một tài khoản giữ nhiều vai, trừ một ràng buộc loại trừ**
- **Description**: Một tài khoản MUST giữ được nhiều vai hệ thống. Tài khoản đang ngồi ghế thí sinh của một trận MUST NOT đồng thời là admin, MC hay người ra đề **của contest đó**; ràng buộc này MUST được kiểm ở server. Người ra đề kiêm MC được phép — hệ thống chỉ cảnh báo và ghi nhật ký.
- **Actor**: ACTOR-001…004 · **Related epic**: EPIC-001 · **Related journey**: JOURNEY-002 · **Related game rules**: `GR-037`
- **Priority**: P1 · **Rationale**: Admin và MC thấy đáp án ⇒ kiêm ghế thí sinh là gian lận có cấu trúc. Ràng buộc gắn với contest, không với hệ thống.
- **Acceptance intent**: Gán vai vi phạm bị từ chối; gán setter-kiêm-MC đi qua kèm cảnh báo và dòng nhật ký.
- **Source**: `QĐ-065` · `TERM-008` · **Status**: CONFIRMED

**PRD-REQ-004 — Đúng một phiên giữ quyền điều khiển, chuyển giao được**
- **Description**: Nhiều tài khoản MUST có được vai admin, nhưng tại một thời điểm chỉ **một phiên** giữ quyền ghi; các phiên admin khác xem-không-bấm. Hệ thống MUST có thao tác chuyển quyền điều khiển giữa hai phiên, và MUST ghi mọi lần chuyển vào nhật ký.
- **Actor**: ACTOR-001 · **Related epic**: EPIC-001 · **Related journey**: JOURNEY-006 · **Related game rules**: `GR-026`
- **Priority**: P1 · **Rationale**: Khoá theo tài khoản làm mất trận khi admin ốm, máy hỏng, hay đổi ca. Đặt ràng buộc ở tầng phiên giữ được cả tính đơn nhất lẫn khả năng thay người.
- **Acceptance intent**: Hai phiên admin cùng mở — chỉ một phiên bấm được; sau khi chuyển quyền, vai trò đảo lại và có dòng nhật ký.
- **Source**: `QĐ-008` · `QĐ-070` · **Status**: CONFIRMED

**PRD-REQ-005 — Permission riêng cho từng thao tác phá huỷ**
- **Description**: Bỏ vòng · chạy lại vòng · kết thúc vòng khẩn cấp · huỷ trận · điều chỉnh điểm · sửa danh sách đề · ép qua hàng rào đề đã từng public — mỗi thao tác MUST là một permission riêng, MUST NOT suy ra từ *"là admin"*.
- **Actor**: ACTOR-001 · **Related epic**: EPIC-001 · **Related journey**: JOURNEY-006 · **Related game rules**: `GR-029`, `GR-030`
- **Priority**: P2 · **Rationale**: Nếu suy từ vai thì không có cách nào cấp một tài khoản chạy trận mà không đồng thời cho nó xoá vòng.
- **Acceptance intent**: Cấp một tài khoản quyền vận hành trận nhưng không quyền bỏ vòng ⇒ nút bỏ vòng không dùng được và server từ chối.
- **Source**: `QĐ-078` · `product-discovery.md` §6 · **Status**: CONFIRMED

**PRD-REQ-006 — Vai vận hành gán theo contest, tách khỏi vai hệ thống**
- **Description**: *MC*, *người phụ trách luyện tập*, *host* MUST là quyền gán theo từng contest, MUST NOT là vai seed của hệ thống.
- **Actor**: ACTOR-001 · **Related epic**: EPIC-001 · **Related journey**: JOURNEY-002 · **Related game rules**: —
- **Priority**: P2 · **Rationale**: Trộn hai khái niệm là cách nhanh nhất để đếm sai số vai và cấp thừa quyền.
- **Acceptance intent**: Một tài khoản là MC ở contest A không có quyền MC ở contest B.
- **Source**: `QĐ-065` · `product-discovery.md` §2 · **Status**: CONFIRMED

### EPIC-002 — Kho đề và bộ đề

**PRD-REQ-007 — Soạn câu hỏi với metadata và media**
- **Description**: Người ra đề MUST soạn được câu hỏi kèm mã hiển thị, lĩnh vực, số chữ của đáp án, giải thích, ghi chú, thời lượng suy nghĩ tuỳ chọn, mức điểm tuỳ chọn, danh sách gợi ý tuỳ chọn, và media.
- **Actor**: ACTOR-002 · **Related epic**: EPIC-002 · **Related journey**: JOURNEY-001 · **Related game rules**: `GR-031`
- **Priority**: P1 · **Rationale**: Kho đề dùng lại được là cách giải `PS-3`.
- **Acceptance intent**: Câu lưu và mở lại giữ nguyên mọi trường; media phát được.
- **Source**: `TERM-044` · `QĐ-066` · **Status**: CONFIRMED

**PRD-REQ-008 — Thời lượng suy nghĩ là metadata của TỪNG CÂU**
- **Description**: Thời lượng suy nghĩ MUST là thuộc tính của từng câu hỏi, MUST NOT suy ra từ mức điểm. Hệ thống chọn câu theo **mức điểm**, lấy thời lượng **theo câu**; preset chỉ đặt giá trị mặc định và giá trị của câu **thắng** giá trị mặc định.
- **Actor**: ACTOR-002 · **Related epic**: EPIC-002 · **Related journey**: JOURNEY-001 · **Related game rules**: `GR-018`, `GR-013`
- **Priority**: P1 · **Rationale**: Nguồn ngoài gắn chặt thời gian với mức điểm; dự án cố ý tổng quát hoá vượt nguồn để cùng mức 20đ có câu 15 giây và câu 40 giây.
- **Acceptance intent**: Hai câu cùng mức điểm, khác thời lượng ⇒ đồng hồ chạy đúng theo từng câu.
- **Source**: `TERM-045` · `GOAL-004` · `CLAUDE.md` §Luật chơi & đề thi · **Status**: CONFIRMED

**PRD-REQ-009 — Ba kiểu nhập đáp án, đáp án luôn là chuỗi**
- **Description**: Câu hỏi MUST hỗ trợ ba kiểu nhập — nhập chữ, chọn một phương án, sắp thứ tự — và kiểu nhập chỉ quyết định **widget trên máy thí sinh**. Đáp án và bài làm MUST lưu dưới dạng chuỗi ở cả ba kiểu. Hệ thống MUST NOT có nhánh chấm riêng cho kiểu nào.
- **Actor**: ACTOR-002, ACTOR-003 · **Related epic**: EPIC-002 · **Related journey**: JOURNEY-001 · **Related game rules**: `GR-027`
- **Priority**: P1 · **Rationale**: Máy không chấm nên không cần đánh giá một thứ tự hay một lựa chọn; giữ chuỗi thì cơ chế tô khác biệt ký tự chạy nguyên cho cả ba.
- **Acceptance intent**: Bài làm kiểu sắp thứ tự hiện ra dưới dạng chuỗi cạnh đáp án và được tô khác biệt như câu nhập chữ.
- **Source**: `QĐ-066` · `TERM-044` · **Status**: CONFIRMED

**PRD-REQ-010 — Câu thực hành là kênh trả lời thứ tư**
- **Description**: Câu hỏi thực hành MUST có các trường riêng: cờ thực hành *(chỉ hợp lệ ở kho Về đích)*, thời gian thực hành của người thi chính, thời gian thực hành của người cướp quyền, ghi chú dụng cụ, và tiêu chí đạt. Không có ô nhập nào; admin chấm *đạt / không đạt*. Tiêu chí đạt MUST cùng mức bảo mật với đáp án.
- **Actor**: ACTOR-002, ACTOR-001 · **Related epic**: EPIC-002 · **Related journey**: JOURNEY-001, JOURNEY-005 · **Related game rules**: `GR-019`
- **Priority**: P2 · **Rationale**: Luật gốc mô tả câu thực hành với hai pha thời gian và hai bộ số khác nhau cho người thi chính và người cướp.
- **Acceptance intent**: Câu thực hành hiện đúng hai pha; tiêu chí đạt không rời server tới thí sinh hay khán giả.
- **Source**: `GR-019` · `QĐ-066` · `source/fandom-olympia-26-luat-choi.md` §Về đích · **Status**: CONFIRMED

**PRD-REQ-011 — Vòng duyệt `DRAFT` → `ACTIVE` do admin thực hiện**
- **Description**: Câu hỏi mới MUST vào trạng thái nháp và MUST được admin duyệt trước khi dùng được trong trận. Hàng chờ duyệt MUST hiện trên bảng điều khiển của admin. Hệ thống MUST NOT có vai người duyệt riêng.
- **Actor**: ACTOR-001, ACTOR-002 · **Related epic**: EPIC-002 · **Related journey**: JOURNEY-001 · **Related game rules**: `GR-031`
- **Priority**: P2 · **Rationale**: Thêm một vai chỉ để duyệt làm phình mô hình quyền mà không thêm bảo đảm nào.
- **Acceptance intent**: Câu ở nháp không xuất hiện trong danh sách chọn được cho contest.
- **Source**: `QĐ-064` · `CLAUDE.md` §UX *(dashboard admin)* · **Status**: CONFIRMED

**PRD-REQ-012 — Cờ hiển thị của câu là giá trị DẪN XUẤT, chỉ đọc**
- **Description**: Cờ hiển thị của một câu MUST bằng *public* khi và chỉ khi câu đang thuộc ít nhất một bộ đề public, ngược lại *private*. Không vai nào MUST được đặt trực tiếp cờ này; người ra đề làm một câu thành public bằng cách đưa nó vào một bộ đề public.
- **Actor**: ACTOR-002 · **Related epic**: EPIC-002 · **Related journey**: JOURNEY-001 · **Related game rules**: `GR-031`
- **Priority**: P2 · **Rationale**: Nếu đặt tay được thì một người gỡ được nhãn public khỏi câu đã lộ, vô hiệu hoá chính hàng rào chống rò đề bằng một thao tác trông hợp lệ.
- **Acceptance intent**: Không có đường nào trong giao diện hay ở server đặt trực tiếp cờ này.
- **Source**: `QĐ-063` · `TERM-050` · **Status**: CONFIRMED

**PRD-REQ-013 — Dấu vết `đã từng public` là một chiều và chặn trận official**
- **Description**: Câu đã từng nằm trong bộ đề public MUST mang một dấu vết **một chiều, vĩnh viễn**. Bất cứ khi nào một câu được đưa vào danh sách gán của một trận official — kiểm trước trận, sửa danh sách ở LOBBY, hay nhập gói — hàng rào MUST chạy y hệt: chặn, ép được bằng **xác nhận hai bước**, và ghi nhật ký. Hàng rào MUST NOT áp cho trận luyện tập. Dấu vết này MUST đi theo câu qua nhập/xuất.
- **Actor**: ACTOR-001 · **Related epic**: EPIC-002, EPIC-003 · **Related journey**: JOURNEY-002, JOURNEY-003 · **Related game rules**: `GR-031`
- **Priority**: P1 · **Rationale**: Gắn hàng rào vào **một** cửa vào để hở chuỗi ba bước hợp lệ: tạo trận với danh sách sạch → bắt đầu → ở LOBBY thêm câu đã lộ vào, không ép, không nhật ký.
- **Acceptance intent**: Thêm câu đã lộ qua **bất kỳ** cửa nào đều gặp cùng một hàng rào và cùng một dòng nhật ký.
- **Source**: `QĐ-071` · `TERM-049` · **Status**: CONFIRMED

**PRD-REQ-014 — Tìm kiếm toàn văn, lọc và sắp xếp trên kho đề**
- **Description**: Admin MUST tìm được câu bằng tìm kiếm toàn văn kèm bộ lọc và sắp xếp khi chọn danh sách câu cho contest.
- **Actor**: ACTOR-001 · **Related epic**: EPIC-002 · **Related journey**: JOURNEY-002 · **Related game rules**: `GR-031`
- **Priority**: P1 · **Rationale**: Hệ thống cố ý không tự lấy đề, nên toàn bộ gánh nặng chọn đề đổ lên công cụ tìm kiếm.
- **Acceptance intent**: Dựng được danh sách đủ cho một trận chuẩn mà không phải cuộn qua toàn bộ kho.
- **Source**: `CLAUDE.md` §Luật chơi & đề thi · `product-discovery.md` §4 J-2 · **Status**: CONFIRMED

**PRD-REQ-015 — Cắt khoảng trắng đầu cuối ở cả hai đầu**
- **Description**: Mọi đầu vào văn bản — câu hỏi, đáp án, bài làm — MUST được cắt khoảng trắng hai đầu ở **cả** giao diện lẫn server.
- **Actor**: ACTOR-002, ACTOR-003 · **Related epic**: EPIC-002, EPIC-008 · **Related journey**: JOURNEY-001, JOURNEY-005 · **Related game rules**: `GR-027`, `GR-006`
- **Priority**: P2 · **Rationale**: Khoảng trắng thừa phá việc đối chiếu và tạo báo động giả trên màn chấm.
- **Acceptance intent**: Bản gửi chỉ gồm khoảng trắng bị bỏ qua; bản có khoảng trắng thừa tô giống bản không có.
- **Source**: `CLAUDE.md` §Quy ước khác · `GR-006`, `GR-027` · **Status**: CONFIRMED

### EPIC-003 — Nhập/xuất và gói contest

**PRD-REQ-016 — Xuất và nhập gói contest trọn vẹn**
- **Description**: Hệ thống MUST xuất được một contest thành gói gồm câu hỏi, metadata media, và media xếp theo từng vòng; và MUST nhập lại gói đó trên một bản cài khác thành contest ở dạng nháp. Gói MUST mang **định danh ổn định của từng câu hỏi**, và bên nhập MUST giữ nguyên định danh đó.
- **Note**: Vế định danh ổn định là **điều kiện cần** của `PRD-REQ-095` *(bản kê câu đã dùng)* và `PRD-REQ-096` *(nhật ký sự kiện)* — không có nó thì hai gói kia không ghép được với dữ liệu bên nhận. Nó cũng vốn đã ngầm cần cho `QĐ-071` *(`everPublic` đi theo câu qua nhập/xuất)*.
- **Actor**: ACTOR-001 · **Related epic**: EPIC-003 · **Related journey**: JOURNEY-003 · **Related game rules**: `GR-031`
- **Priority**: P1 · **Rationale**: Trường hợp dùng đã xác định: soạn trên bản có Internet, nhập vào bản portable ngày thi.
- **Acceptance intent**: Nhập xong, contest kiểm kho đề đạt mà không cần thao tác sửa nào.
- **Source**: `product-discovery.md` §4 J-3, §5 E-3 · `GOAL-009` · **Status**: CONFIRMED

**PRD-REQ-017 — Cờ theo câu đi qua nhập/xuất, cờ theo contest thì đặt lại**
- **Description**: Dấu vết *đã từng public* MUST đi theo câu qua nhập/xuất. Cờ *đã dùng trong contest* MUST được đặt lại khi câu vào một contest khác, vì nó là thuộc tính của contest.
- **Actor**: ACTOR-001 · **Related epic**: EPIC-003 · **Related journey**: JOURNEY-003 · **Related game rules**: `GR-031`
- **Priority**: P1 · **Rationale**: Hai cờ có hai chủ sở hữu khác nhau; nhầm chủ sở hữu làm hoặc rò đề, hoặc khoá nhầm cả kho.
- **Acceptance intent**: Sau khi nhập vào contest mới, câu đã lộ vẫn bị chặn; câu đã dùng ở contest cũ vẫn rút được.
- **Source**: `QĐ-071` · `TERM-048`, `TERM-049` · **Status**: CONFIRMED

**PRD-REQ-018 — Nhập/xuất phần đề của một người ra đề**
- **Description**: Người ra đề MUST nhập và xuất được phần đề mình phụ trách.
- **Actor**: ACTOR-002 · **Related epic**: EPIC-003 · **Related journey**: JOURNEY-001 · **Related game rules**: —
- **Priority**: P3 · **Rationale**: Nhiều người soạn song song rồi gộp lại là cách làm việc thực tế của một ban đề.
- **Acceptance intent**: Xuất phần của một người rồi nhập vào bản cài khác không đụng phần của người khác.
- **Source**: `product-discovery.md` §2 A-2 · **Status**: CONFIRMED

**PRD-REQ-019 — Giới hạn kích thước media**
- **Description**: Hệ thống MUST áp giới hạn kích thước cho media tải lên và MUST cho phép cấu hình các giới hạn này.
- **Actor**: ACTOR-002 · **Related epic**: EPIC-003, EPIC-012 · **Related journey**: JOURNEY-001 · **Related game rules**: —
- **Priority**: P2 · **Rationale**: Bản portable chạy trên máy tính xách tay; media không giới hạn làm gói contest không mang đi được.
- **Acceptance intent**: File vượt ngưỡng bị từ chối kèm thông điệp nêu ngưỡng.
- **Note**: **Không có con số chuẩn tắc.** Ngưỡng là **giá trị vận hành**, đặt bằng biến môi trường theo từng bản triển khai. Yêu cầu này chỉ ràng buộc rằng ngưỡng **tồn tại**, **cấu hình được**, và **không hard-code**.
- **Source**: `CLAUDE.md` §Quy ước khác · **Status**: CONFIRMED

**PRD-REQ-094 — Đánh dấu "đã dùng" hàng loạt bằng tay**
- **Description**: Admin MUST đánh dấu được **nhiều câu cùng lúc** là đã dùng trong một contest, chọn từ danh sách câu đã gán. Thao tác MUST một chiều — chỉ đặt cờ từ chưa-dùng sang đã-dùng, MUST NOT có đường ngược. MUST qua dialog xác nhận và MUST vào nhật ký thao tác.
- **Actor**: ACTOR-001 · **Related epic**: EPIC-003 · **Related journey**: JOURNEY-007 · **Related game rules**: `GR-031`
- **Priority**: P2 · **Rationale**: Trận chạy trên bản portable không có đường mang kết quả về; nếu không đánh dấu được bằng tay thì kho đề ở máy trung tâm vẫn coi những câu đã lên sóng là chưa dùng.
- **Acceptance intent**: Chọn 12 câu, xác nhận ⇒ cả 12 chuyển sang đã dùng, không câu nào đảo ngược được, nhật ký ghi đủ người và thời điểm.
- **Source**: `QĐ-084` · `TERM-048` · **Status**: CONFIRMED

**PRD-REQ-095 — Xuất và nhập bản kê câu đã dùng**
- **Description**: Hệ thống MUST xuất được bản kê các câu đã dùng của một contest, và MUST nhập bản kê đó vào một contest trên bản cài khác. Phép nhập MUST là **phép hợp** — chỉ đặt cờ sang đã-dùng, MUST NOT xoá cờ nào. Trước khi áp, hệ thống MUST hiện bản xem trước tách làm ba nhóm: sẽ chuyển sang đã dùng · đã ở trạng thái đó · **không thuộc danh sách câu đã gán của contest đích**. Nhóm thứ ba MUST NOT được áp tự động.
- **Actor**: ACTOR-001 · **Related epic**: EPIC-003 · **Related journey**: JOURNEY-007 · **Related game rules**: `GR-031`
- **Priority**: P2 · **Rationale**: Đây là đường duy nhất mang thông tin *"câu nào đã lộ"* từ bản portable về máy chủ trung tâm; làm tay cho từng câu thì không dùng được ở quy mô nhiều trận.
- **Acceptance intent**: Nhập cùng một bản kê hai lần cho ra cùng kết quả và lần thứ hai không đổi gì.
- **Source**: `QĐ-084` · **Status**: CONFIRMED

### EPIC-004 — Contest builder và luật tuỳ biến

**PRD-REQ-020 — Mọi thời gian, số câu và mức điểm là cấu hình**
- **Description**: Không giá trị luật nào MUST được viết cứng trong code hay giao diện; tất cả MUST khai trong cấu hình luật của contest.
- **Actor**: ACTOR-001 · **Related epic**: EPIC-004 · **Related journey**: JOURNEY-002 · **Related game rules**: toàn bộ `GR-001`…`GR-025`
- **Priority**: P1 · **Rationale**: Đây là lời giải trực tiếp cho `PS-1` và là điều kiện để cùng một engine chạy nhiều mùa luật.
- **Acceptance intent**: Đổi một giá trị qua giao diện rồi chạy trận ⇒ trận chạy theo giá trị mới, không cần build lại.
- **Source**: `CLAUDE.md` §Quy ước khác · `TERM-051` · `GOAL-002` · **Status**: CONFIRMED

**PRD-REQ-021 — Nút "Áp dụng luật 2026" áp preset O26**
- **Description**: Contest builder MUST có một thao tác áp bộ giá trị mặc định theo luật O26 lên toàn bộ cấu hình luật của contest.
- **Actor**: ACTOR-001 · **Related epic**: EPIC-004 · **Related journey**: JOURNEY-002 · **Related game rules**: toàn bộ
- **Priority**: P1 · **Rationale**: Không có nút này thì tính tuỳ biến biến thành gánh nặng nhập liệu cho mọi contest.
- **Acceptance intent**: Sau khi áp, mọi giá trị khớp bảng luật O26 trong `traceability.md`.
- **Source**: `GOAL-003` · `TERM-051` · `traceability.md` · **Status**: CONFIRMED

**PRD-REQ-022 — Hai option hợp lệ nhưng KHÔNG thuộc preset O26**
- **Description**: Hệ thống MUST hỗ trợ hai option cấu hình mà preset O26 MUST NOT dùng: cướp quyền theo kiểu *cộng thêm* thay vì *chuyển điểm*, và cách xử lý khi hết câu Câu hỏi phụ theo kiểu *admin tự quyết* thay vì *bốc thăm*.
- **Actor**: ACTOR-001 · **Related epic**: EPIC-004 · **Related journey**: JOURNEY-002 · **Related game rules**: `GR-020`, `GR-025`
- **Priority**: P3 · **Rationale**: Hai giá trị này là biến thể của mùa khác; ghi rõ để chúng không bị hiểu nhầm là luật O26 và cũng không bị cài nhầm thành mặc định.
- **Acceptance intent**: Áp preset O26 ⇒ cả hai option ở giá trị O26; đổi được thủ công.
- **Source**: `TERM-051` · `traceability.md` §Biến thể bị loại · `GR-025` *(bốc thăm là giá trị hợp lệ **duy nhất ở v1**)* · **Status**: CONFIRMED

**PRD-REQ-023 — Mode trả lời đặt ở cấp CONTEST, một giá trị chung cho mọi vòng**
- **Description**: Contest MUST có đúng một mode trả lời — **sân khấu** *(mặc định)* hoặc **nhập liệu** — áp cho toàn bộ các vòng. Mode MUST được chụp vào trận tại cú bấm bắt đầu trận; sửa mode ở contest về sau MUST NOT đụng trận đã chạy. Mode MUST NOT nằm trong preset luật.
- **Actor**: ACTOR-001 · **Related epic**: EPIC-004 · **Related journey**: JOURNEY-002 · **Related game rules**: `GR-007`, `GR-017`, `GR-021`
- **Priority**: P1 · **Rationale**: Mode là **điều kiện sân khấu**, không phải giá trị luật; một contest trộn hai mode làm thí sinh phải học hai mô hình thao tác giữa trận.
- **Acceptance intent**: Nút *"Áp dụng luật 2026"* không đụng mode; trận đang chạy giữ nguyên mode dù contest bị sửa.
- **Source**: `QĐ-016`, `QĐ-017`, `QĐ-075` · `TERM-016` · **Status**: CONFIRMED

**PRD-REQ-024 — v1 khoá cứng bốn vòng, đúng thứ tự**
- **Description**: Ở v1, playlist của contest MUST luôn là **Khởi động → Vượt chướng ngại vật → Tăng tốc → Về đích**. Cửa tạo contest MUST NOT cho chọn playlist khác. Câu hỏi phụ MUST NOT là vòng thứ năm của playlist — nó là nhánh phân định.
- **Actor**: ACTOR-001 · **Related epic**: EPIC-004 · **Related journey**: JOURNEY-002 · **Related game rules**: `GR-022`, `GR-030`
- **Priority**: P1 · **Rationale**: Luật gốc không có tình huống *"chạy Khởi động hai lần trong một trận"*. Khoá ở thiết kế contest không đụng quyền **chạy lại** một vòng khi vận hành.
- **Acceptance intent**: Không có đường nào trong giao diện tạo ra playlist khác bốn vòng chuẩn.
- **Source**: `QĐ-069` · `traceability.md` §Biến thể ngoài luật O26 · **Status**: CONFIRMED

**PRD-REQ-025 — v1 khoá cứng bốn hàng ngang; dữ liệu vẫn nhận 5-8**
- **Description**: Số hàng ngang của Vượt chướng ngại vật MUST bằng 4 và cửa tạo contest MUST NOT cho chọn giá trị khác. Mô hình dữ liệu và cấu hình luật MUST vẫn **nhận** giá trị 5-8 mà không lỗi cấu trúc, dù không có đường xử lý nào cho các giá trị đó.
- **Actor**: ACTOR-001 · **Related epic**: EPIC-004 · **Related journey**: JOURNEY-002 · **Related game rules**: `GR-009` C11
- **Priority**: P1 · **Rationale**: Băng điểm Chướng ngại vật cho 5-8 hàng **không tồn tại trong bất kỳ nguồn nào** ⇒ mở ra là bịa luật.
- **Acceptance intent**: Cấu hình nhận giá trị 5-8 mà không lỗi cấu trúc; giao diện tạo contest không hiện lựa chọn.
- **Source**: `QĐ-068` · `traceability.md` · **Status**: CONFIRMED

**PRD-REQ-026 — Gán ghế và vị trí trước trận; vị trí bất biến trong trận**
- **Description**: Admin MUST gán thí sinh vào ghế và gán số vị trí trước khi trận bắt đầu. Vị trí MUST NOT đổi trong suốt một trận. Mô hình dữ liệu và giao diện MUST hỗ trợ 1-12 ghế, trong khi **luật của v1 chỉ đặc tả cho đúng 4**.
- **Actor**: ACTOR-001 · **Related epic**: EPIC-004 · **Related journey**: JOURNEY-002, JOURNEY-004 · **Related game rules**: `GR-016`, `GR-007`
- **Priority**: P1 · **Rationale**: Vị trí là đầu vào của hai luật — thứ tự chọn hàng ngang, và phá hoà thứ tự lượt Về đích; đổi giữa trận làm hai luật đó mất nghĩa.
- **Acceptance intent**: Sau khi vòng đầu tiên đã chạy, không có đường nào đổi vị trí.
- **Source**: `QĐ-007` · `TERM-002` · `GR-016` · **Status**: CONFIRMED

### EPIC-005 — Phòng thi và vòng đời trận

**PRD-REQ-027 — Contest là bản thiết kế, trận là một lần chạy**
- **Description**: Một contest MUST chứa được nhiều trận, và MUST chỉ có **một trận đang chạy** tại một thời điểm. Contest giữ mã phòng, kho đề đã gán, cấu hình gốc và **phạm vi** không-lặp-câu; trận giữ điểm, nhật ký sự kiện, ghế đã gán, biên bản và cấu hình đã đóng băng. Không-lặp-câu MUST NOT là một cờ cấu hình được.
- **Actor**: ACTOR-001 · **Related epic**: EPIC-005 · **Related journey**: JOURNEY-005, JOURNEY-007 · **Related game rules**: `GR-031`
- **Priority**: P1 · **Rationale**: Trạng thái thuộc về trận là lời giải trực tiếp cho `PS-8`.
- **Acceptance intent**: Khán giả và lớp phủ không phải vào lại phòng giữa hai trận; sửa cấu hình luật giữa hai trận là hợp lệ.
- **Source**: `QĐ-039` · `TERM-010`, `TERM-011` · **Status**: CONFIRMED

**PRD-REQ-028 — Bắt đầu trận đóng băng cấu hình, đúng một lần**
- **Description**: Cú bấm bắt đầu trận MUST đóng băng vào trận: cấu hình luật, mode trả lời, danh sách câu đã gán, và cờ hiện đáp án sau khi chấm. Việc đóng băng MUST xảy ra đúng một lần cho cả đời trận.
- **Actor**: ACTOR-001 · **Related epic**: EPIC-005 · **Related journey**: JOURNEY-005 · **Related game rules**: `GR-031`, `GR-037`
- **Priority**: P1 · **Rationale**: Nếu cấu hình đổi được giữa trận thì nhật ký sự kiện không dựng lại được kết quả.
- **Acceptance intent**: Sửa contest sau khi trận đã bắt đầu không đổi hành vi của trận đó.
- **Source**: `EVENT-001`, `T-001` · `QĐ-075`, `QĐ-062` · **Status**: CONFIRMED

**PRD-REQ-029 — LOBBY là cửa vào và cửa ra duy nhất của mọi vòng**
- **Description**: Trạng thái nghỉ của trận MUST có đúng **một** giá trị, dùng cả trước vòng đầu tiên lẫn giữa hai vòng. Mọi vòng MUST vào và ra qua trạng thái này. Hệ thống MUST NOT có đường đi thẳng từ vòng này sang vòng khác.
- **Actor**: ACTOR-001 · **Related epic**: EPIC-005 · **Related journey**: JOURNEY-005 · **Related game rules**: `GR-030` C4
- **Priority**: P1 · **Rationale**: Hai trạng thái nghỉ tách biệt chỉ khác nhau trên **một cạnh** — cạnh đóng băng cấu hình — và khác biệt đó suy ra được từ nhật ký sự kiện.
- **Acceptance intent**: Bấm mở vòng B khi vòng A chưa khép ⇒ toast invalid state, không phải cảnh báo ép được.
- **Source**: `QĐ-032`, `QĐ-034` · `STATE-001` · `TERM-019` · **Status**: CONFIRMED

**PRD-REQ-030 — Admin chọn vòng nào bắt đầu; playlist chỉ là gợi ý**
- **Description**: Admin MUST chọn được vòng nào mở tiếp. Hệ thống MUST khuyến nghị theo luật và cấu hình contest, và khi admin chọn khác khuyến nghị thì MUST cảnh báo bằng dialog cho ép qua — MUST NOT chặn cứng.
- **Actor**: ACTOR-001 · **Related epic**: EPIC-005, EPIC-007 · **Related journey**: JOURNEY-005 · **Related game rules**: `GR-030` · `INV-020`
- **Priority**: P1 · **Rationale**: Buổi thi thật đổi thứ tự vì lý do sân khấu; hệ thống cưỡng chế thứ tự sẽ chặn đúng lúc cần nhất.
- **Acceptance intent**: Chọn vòng ngoài khuyến nghị ⇒ dialog nêu tên luật bị lệch, bấm Yes thì đi tiếp.
- **Source**: `QĐ-002` · `INV-020` · **Status**: CONFIRMED

**PRD-REQ-031 — Bốn cửa ra của một vòng**
- **Description**: Một vòng MUST rời về LOBBY qua đúng bốn cửa: **kết thúc vòng** *(đủ câu, giữ điểm)* · **kết thúc khẩn cấp** *(bấm được cả khi chưa đủ câu, **giữ nguyên điểm**, nhãn "kết thúc sớm")* · **bỏ vòng** *(hoàn nguyên điểm, nhãn "đã bỏ")* · **chạy lại vòng** *(hoàn nguyên rồi chạy mới, phải qua cửa kiểm kho đề như vòng mới)*. Ba cửa sau MUST đi qua dialog hạng phá huỷ có bắt nhập lý do.
- **Actor**: ACTOR-001 · **Related epic**: EPIC-005, EPIC-007 · **Related journey**: JOURNEY-006 · **Related game rules**: `GR-030`
- **Priority**: P1 · **Rationale**: Chỉ ba cửa ra thì admin ở giữa một vòng hỏng bị khoá. Cửa **giữ nguyên điểm** tồn tại chính vì khác biệt đó.
- **Acceptance intent**: Kết thúc khẩn cấp ⇒ bảng điểm không đổi; bỏ vòng ⇒ điểm của vòng đó bị đảo bằng sự kiện mới, không bị xoá.
- **Source**: `QĐ-034`, `QĐ-035` · `GR-030` · `TERM-031` · **Status**: CONFIRMED

**PRD-REQ-032 — Trận không tự đóng sổ**
- **Description**: Hết playlist, hệ thống MUST chỉ **gợi ý** chốt trận. Phép tính bảng điểm và điều kiện hoà MUST chạy tại cú bấm **chốt trận**, trên bảng điểm ở đúng mốc đó. Không cạnh nào rời LOBBY MUST bật tự động.
- **Actor**: ACTOR-001 · **Related epic**: EPIC-005 · **Related journey**: JOURNEY-007 · **Related game rules**: `GR-022`
- **Priority**: P1 · **Rationale**: Đóng sổ tự động mâu thuẫn với chính quyền sửa điểm ở LOBBY — điểm sửa ở LOBBY có thể **tạo ra** hoặc **gỡ** một nhóm hoà.
- **Acceptance intent**: Điều chỉnh điểm ở LOBBY rồi bấm chốt ⇒ điều kiện hoà tính trên bảng điểm đã sửa.
- **Source**: `QĐ-036` · `GR-022` · `EVENT-047` · **Status**: CONFIRMED

**PRD-REQ-033 — Trận đóng sổ là terminal và niêm phong**
- **Description**: Sau khi đóng sổ, bảng điểm MUST NOT đổi được: không điều chỉnh điểm, không chạy lại vòng, không sửa phán quyết. Van thoát duy nhất MUST là tạo một trận mới trong cùng contest, và việc đó MUST NOT là một chuyển tiếp rời trạng thái đóng sổ.
- **Actor**: ACTOR-001 · **Related epic**: EPIC-005 · **Related journey**: JOURNEY-007 · **Related game rules**: `GR-029` C6
- **Priority**: P1 · **Rationale**: Nếu sửa được sau khi đóng sổ thì biên bản đã xuất mất giá trị phân xử.
- **Acceptance intent**: Mọi nút sửa điểm ở trận đã đóng sổ đều không tồn tại, và server từ chối nếu yêu cầu lọt tới.
- **Source**: `QĐ-037` · `STATE-008` · **Status**: CONFIRMED

**PRD-REQ-034 — Huỷ trận đóng sổ bằng NHÃN, không phải trạng thái riêng**
- **Description**: Huỷ trận MUST đưa trận về trạng thái đóng sổ với nhãn *bỏ dở*, giữ nguyên điểm, **không phân định thứ hạng và không có người thắng**. Hệ thống MUST ghi người bấm, thời điểm, và **lý do bắt buộc**.
- **Actor**: ACTOR-001 · **Related epic**: EPIC-005 · **Related journey**: JOURNEY-006 · **Related game rules**: `GR-022`
- **Priority**: P2 · **Rationale**: Thêm một trạng thái thứ năm không đáng, vì không chuyển tiếp nào rẽ nhánh theo nó — thứ cần nó là biên bản, thống kê và hạn lưu trữ.
- **Acceptance intent**: Trận bỏ dở xuất được biên bản, và biên bản không có mục người thắng.
- **Source**: `QĐ-038` · `GR-022` · `TERM-039` · **Status**: CONFIRMED

**PRD-REQ-035 — Sửa danh sách câu đã gán tại cửa vào vòng**
- **Description**: Ở LOBBY, admin MUST thêm và bớt câu trong danh sách đã gán và MUST chạy lại phép kiểm kho đề. Hệ thống MUST NOT cho gỡ câu **đã hiển thị**. Thông điệp báo thiếu đề MUST dẫn thẳng sang màn sửa danh sách.
- **Actor**: ACTOR-001 · **Related epic**: EPIC-005 · **Related journey**: JOURNEY-006 · **Related game rules**: `GR-031`
- **Priority**: P1 · **Rationale**: Đây là **lối thoát duy nhất** của chặn cứng *"cửa vào vòng thiếu câu"*; cấm gỡ câu đã hiển thị để bịt đường lách quy tắc không-lặp-câu.
- **Acceptance intent**: Vòng không mở được vì thiếu câu ⇒ bổ sung ở LOBBY ⇒ vòng mở được.
- **Source**: `QĐ-042`, `QĐ-043` · `T-003`, `T-004` · **Status**: CONFIRMED

### EPIC-006 — Game engine và luật thi đấu

**PRD-REQ-036 — Máy KHÔNG BAO GIỜ tự chấm Đúng/Sai**
- **Description**: Không đường xử lý nào MUST tự cộng hoặc trừ điểm từ kết quả đối chiếu văn bản. Hệ thống MUST chỉ (1) hiển thị bài làm cạnh đáp án và (2) tô khác biệt ký tự làm gợi ý. Điểm MUST chỉ chốt sau khi admin bấm.
- **Actor**: ACTOR-001, ACTOR-007 · **Related epic**: EPIC-006, EPIC-007 · **Related journey**: JOURNEY-005 · **Related game rules**: `GR-026`, `GR-027`, `GR-001`, `GR-004`, `GR-008` · `INV-003`
- **Priority**: P1 · **Rationale**: Đây là nguyên tắc nền chi phối toàn bộ sản phẩm — máy độc quyền sự kiện, người độc quyền phán quyết. Nó cũng làm tan mâu thuẫn *"chính tả nghiêm ngặt vs chuẩn hoá"*: chuẩn hoá chỉ là trợ giúp hiển thị.
- **Acceptance intent**: Không tồn tại cấu hình nào bật chấm tự động; kết quả đối chiếu không bao giờ xuất hiện trong đường sinh điểm.
- **Source**: `QĐ-001`, `QĐ-010` · `GR-026` · `INV-003` · `NON-GOAL-001` · **Status**: CONFIRMED

**PRD-REQ-037 — Một câu đi qua ĐÚNG MỘT phán quyết**
- **Description**: Sau khi chấm, nút chấm MUST khoá và nút chuyển câu MUST hiện. Hệ thống MUST NOT cho bấm lại, đổi phán quyết tại chỗ, hay tính lại thứ hạng. Sửa sai MUST đi qua điều chỉnh điểm thủ công. Server MUST từ chối phán quyết lặp kể cả khi yêu cầu lọt tới.
- **Actor**: ACTOR-001 · **Related epic**: EPIC-006 · **Related journey**: JOURNEY-005 · **Related game rules**: `GR-026` · `INV-009`, `INV-010`
- **Priority**: P1 · **Rationale**: Cơ chế tính lại tự động sẽ là đường code **duy nhất** tự sinh điểm không qua người, phá `PRD-REQ-036`.
- **Acceptance intent**: Bấm chấm lần hai không đổi điểm và không sinh sự kiện.
- **Source**: `QĐ-014` · `INV-009` · **Status**: CONFIRMED

**PRD-REQ-038 — Phán quyết có hai hay ba lựa chọn tuỳ hình phạt của vòng**
- **Description**: Ở vòng mà *Sai* trừ 0 điểm, admin MUST có hai lựa chọn Đúng / Sai. Ở vòng mà *Sai* có hình phạt, MUST có thêm lựa chọn thứ ba **Huỷ kết quả**. Lựa chọn thứ ba MUST **luôn** có mặt khi câu chỉ có bản gửi quá hạn, kể cả ở vòng *Sai* trừ 0.
- **Actor**: ACTOR-001 · **Related epic**: EPIC-006, EPIC-007 · **Related journey**: JOURNEY-005 · **Related game rules**: `GR-026`, `GR-004`, `GR-020`, `GR-021` · bảng §2.2
- **Priority**: P1 · **Rationale**: Không có lựa chọn thứ ba thì mọi cách khép một câu bất thường đều phải trừ điểm oan ai đó.
- **Acceptance intent**: Số nút chấm đổi theo vòng; không dùng chung một bố cục hai nút cho mọi vòng.
- **Source**: `QĐ-061` · `game-rules.md` §2.2 · `TERM-029` · **Status**: CONFIRMED

**PRD-REQ-039 — Điểm là hàm của nhật ký sự kiện; hoàn nguyên là THÊM sự kiện**
- **Description**: Điểm MUST được tính lại từ toàn bộ nhật ký sự kiện, MUST NOT là một con số bị ghi đè trực tiếp. Hoàn nguyên MUST thực hiện bằng cách **thêm** sự kiện đảo ngược. Nhật ký MUST linear, chỉ thêm, và **không bao giờ bị xoá**.
- **Actor**: ACTOR-007 · **Related epic**: EPIC-006 · **Related journey**: JOURNEY-005, JOURNEY-006 · **Related game rules**: `GR-028` · `INV-001`, `INV-002`
- **Priority**: P1 · **Rationale**: Đây là điều kiện để phát lại, soi lại ở bất kỳ mốc nào, và phân xử khiếu nại.
- **Acceptance intent**: Sau mọi thao tác, điểm hiển thị bằng kết quả tính lại từ nhật ký; sau khi bỏ vòng, sự kiện cũ vẫn còn trong nhật ký.
- **Source**: `QĐ-011` · `GR-028` · `TERM-023` · **Status**: CONFIRMED

**PRD-REQ-040 — Sự kiện điểm có hai hình dạng**
- **Description**: Ở mọi vòng **trừ Tăng tốc**, một sự kiện điểm MUST ứng với một thí sinh, chống trùng theo bộ ba *(câu, thí sinh, loại phán quyết)*. Ở **Tăng tốc**, một câu MUST sinh **một** sự kiện điểm cho **toàn bộ bảng**.
- **Actor**: ACTOR-007 · **Related epic**: EPIC-006 · **Related journey**: JOURNEY-005 · **Related game rules**: `GR-013`, `GR-028`
- **Priority**: P1 · **Rationale**: Điểm Tăng tốc là hàm của **thứ hạng** — một quan hệ giữa những người được chấm Đúng; tách thành bốn sự kiện độc lập thì phát lại từng phần cho ra bảng sai.
- **Acceptance intent**: Hoàn nguyên một câu Tăng tốc đảo cả bảng điểm của câu đó, không phải từng ghế.
- **Source**: `QĐ-013` · `TERM-022` · **Status**: CONFIRMED

**PRD-REQ-041 — Điểm được phép ÂM, không có sàn**
- **Description**: Hệ thống MUST NOT kẹp điểm về 0. Điểm âm MUST tham gia bình thường vào xếp lượt Về đích, điều kiện hoà, và mọi bảng hiển thị.
- **Actor**: ACTOR-007 · **Related epic**: EPIC-006, EPIC-009 · **Related journey**: JOURNEY-005 · **Related game rules**: `GR-004`, `GR-016` · `INV-018`
- **Priority**: P1 · **Rationale**: Hình phạt −5 ở Khởi động lượt chung và −½ giá trị câu ở cướp quyền đều có thể đưa một ghế xuống dưới 0 một cách hợp lệ.
- **Acceptance intent**: Bảng xếp hạng và bảng điểm thí sinh hiển thị số âm bình thường, không hard-code số ghế.
- **Source**: `QĐ-012` · `INV-018` · **Status**: CONFIRMED

**PRD-REQ-042 — Server time là nguồn sự thật duy nhất**
- **Description**: Hạn chót, thứ tự chuông và thứ hạng tốc độ MUST tính trên đồng hồ server, ở độ phân giải **mili-giây**. Biên thời gian MUST là **biên đóng** — timestamp đúng bằng mốc vẫn hợp lệ. Client MUST chỉ hiển thị.
- **Actor**: ACTOR-007 · **Related epic**: EPIC-006 · **Related journey**: JOURNEY-005 · **Related game rules**: `GR-035` · `INV-004`, `INV-005`
- **Priority**: P1 · **Rationale**: Công bằng và khả năng dựng lại. Hệ quả kèm theo: chỉnh giờ hệ thống giữa trận không đảo được thứ hạng đã ghi, và máy thí sinh không đồng bộ giờ không ảnh hưởng gì.
- **Acceptance intent**: Tín hiệu tới đúng mốc hạn được tính là hợp lệ; hai tín hiệu lệch 1 ms phân định được.
- **Source**: `QĐ-006`, `QĐ-029` · `GR-035` · `GOAL-006` · **Status**: CONFIRMED

**PRD-REQ-043 — Mốc do admin bấm là tuyệt đối, không ân hạn**
- **Description**: Mọi mốc mà luật gốc mô tả bằng hành vi của MC MUST ánh xạ thành **một cú bấm của admin**. Mốc đó MUST tuyệt đối: không cửa sổ ân hạn, không trừ bù độ trễ tay người. **"Hiển thị câu hỏi" và "start timer" MUST là hai thao tác, thứ tự cố định, không đảo, không gộp.**
- **Actor**: ACTOR-001 · **Related epic**: EPIC-006, EPIC-007 · **Related journey**: JOURNEY-005 · **Related game rules**: `GR-033`, `GR-028` *(mốc)*
- **Priority**: P1 · **Rationale**: Máy không quan sát được sân khấu ⇒ admin là cảm biến. Gộp hai mốc là **phá luật**: cửa sổ chuông co lại còn bằng thời gian suy nghĩ, và cửa sổ Ngôi sao hy vọng mất mốc đóng. Van thoát cho lỗi bấm không phải ân hạn mà là lịch sử đầy đủ để xem lại và gỡ.
- **Acceptance intent**: Giao diện admin có hai nút tuần tự cho mỗi câu; không có cấu hình nào gộp chúng lại.
- **Source**: `QĐ-006`, `QĐ-027`, `QĐ-028` · `GR-033` · `TERM-028` · **Status**: CONFIRMED

**PRD-REQ-044 — Mọi tín hiệu vào hàng đợi theo server timestamp; không có cơ chế drop**
- **Description**: Mọi tín hiệu của thí sinh MUST vào một hàng đợi xử lý **FIFO thuần theo server timestamp**, không ưu tiên theo loại, số ghế hay vị trí. Mọi tín hiệu đã tới server MUST có outcome — thực thi, bị từ chối, hoặc **trơ** — và MUST được ghi. **Lịch sử tín hiệu MUST NOT bị xoá**; chỉ hàng đợi *đang hoạt động* được đặt lại sau mỗi vòng.
- **Actor**: ACTOR-007 · **Related epic**: EPIC-006 · **Related journey**: JOURNEY-005 · **Related game rules**: `GR-032` · `INV-006`, `INV-007`
- **Priority**: P1 · **Rationale**: Drop làm mất dấu; trơ giữ được dấu mà không sinh hiệu lực — đó là căn cứ để admin can thiệp và **gỡ lệnh cấm** khi có sự cố.
- **Acceptance intent**: Tín hiệu đến sau khi đã có người giành quyền vẫn xuất hiện trong lịch sử kèm timestamp và trạng thái *không có hiệu lực*.
- **Source**: `QĐ-020`, `QĐ-024`, `QĐ-025` · `GR-032` · `TERM-026` · **Status**: CONFIRMED

**PRD-REQ-045 — Hàng đợi chỉ CHẶN ở Vượt chướng ngại vật**
- **Description**: Ở Vượt chướng ngại vật — chọn hàng ngang và bấm *"Mở chướng ngại vật"* — tín hiệu MUST chờ admin duyệt mới có hiệu lực. Ở Khởi động lượt chung, cướp quyền Về đích, và Câu hỏi phụ, tín hiệu MUST có hiệu lực ngay theo server timestamp; hàng đợi ở đó là lưới an toàn để admin can thiệp khi có sự cố.
- **Actor**: ACTOR-001, ACTOR-007 · **Related epic**: EPIC-006 · **Related journey**: JOURNEY-005 · **Related game rules**: `GR-032`, `GR-007`, `GR-009`
- **Priority**: P1 · **Rationale**: Tiêu chí phân biệt: tín hiệu *một chiều, hậu quả nặng, không bị ép thời gian* ⇒ chặn. Tín hiệu *đua tốc độ, cửa sổ chặt* ⇒ không chặn, server phân xử ngay.
- **Acceptance intent**: Ở VCNV, tín hiệu chưa duyệt không sinh hệ quả nào; ở Khởi động lượt chung, có chuông là tính ngay.
- **Source**: `QĐ-021` · `GR-032` · **Status**: CONFIRMED

**PRD-REQ-046 — Từ chối một tín hiệu KHÔNG làm thí sinh mất lượt**
- **Description**: Admin bấm No cho một tín hiệu MUST đưa tín hiệu kế tiếp lên, MUST giữ nguyên lượt của ghế bị từ chối, và MUST NOT để lại tác dụng phụ nào: chưa đánh dấu ô đã hỏi, câu chưa tiêu, đồng hồ chưa chạy. Ở mode nhập liệu, nút chọn của ghế đó MUST mở lại.
- **Actor**: ACTOR-001, ACTOR-003 · **Related epic**: EPIC-006, EPIC-008 · **Related journey**: JOURNEY-005 · **Related game rules**: `GR-032`, `GR-007` · `INV-008`
- **Priority**: P1 · **Rationale**: Đây là **chỗ sửa lỗi bấm nhầm của thí sinh**, nên nó cố ý không phải hình phạt; nếu reject làm mất lượt, admin sẽ ngại dùng cơ chế sửa lỗi.
- **Acceptance intent**: Sau khi bị từ chối, thí sinh chọn lại được; ô chữ và kho đề không đổi.
- **Source**: `QĐ-022` · `INV-008` · `TERM-027` · **Status**: CONFIRMED

**PRD-REQ-047 — Rút đề trong danh sách đã gán; câu đã HIỂN THỊ không bao giờ trả lại kho**
- **Description**: Người tạo contest MUST chọn danh sách câu trước khi bắt đầu trận; hệ thống MUST NOT tự lấy đề. Server MUST rút ngẫu nhiên **trong danh sách đã gán**, loại câu đã dùng, và MUST phát một sự kiện rút đề để phát lại được. Ranh giới *đã dùng* MUST là **đã hiển thị cho thí sinh** — không phải *đã chấm*, không phải *đã rút*. Câu đã rút mà chưa hiển thị MUST được trả lại kho. Phạm vi không-lặp-câu MUST là **toàn contest, xuyên mọi trận**.
- **Actor**: ACTOR-001, ACTOR-007 · **Related epic**: EPIC-006, EPIC-002 · **Related journey**: JOURNEY-002, JOURNEY-005 · **Related game rules**: `GR-031` · `INV-011`
- **Priority**: P1 · **Rationale**: *"Điểm hoàn được, đề đã lộ thì không."* Mốc **hiển thị** trùng đúng cú bấm đã dùng cho ba mục đích khác, nên không đẻ thêm mốc mới.
- **Acceptance intent**: Bỏ một vòng ⇒ điểm đảo, nhưng câu đã hiển thị không quay lại kho.
- **Source**: `QĐ-041`, `QĐ-044` · `GR-031` · `TERM-047`, `TERM-048` · **Status**: CONFIRMED

**PRD-REQ-048 — Kiểm kho đề tại cửa vào TỪNG VÒNG**
- **Description**: Hệ thống MUST kiểm đủ câu tại cửa vào **từng vòng**, không phải một lần lúc bắt đầu trận. Thiếu câu ⇒ **không mở được vòng đó** *(một trong ba chỗ chặn cứng)*. Số câu của một vòng MUST là con số cố định của luật; hệ thống MUST NOT tự sinh câu thứ N+1, và tình huống *"kho đề cạn giữa vòng"* MUST NOT tồn tại.
- **Actor**: ACTOR-007 · **Related epic**: EPIC-006 · **Related journey**: JOURNEY-005, JOURNEY-006 · **Related game rules**: `GR-031`, `GR-005` · `INV-012`, `INV-014`
- **Priority**: P1 · **Rationale**: Admin đổi thứ tự vòng được, nên không tồn tại một *"danh sách vòng sẽ chạy"* để kiểm một lần. Tiền lệ chỉ cảnh báo khi thiếu đề dẫn tới vòng chạy dở rồi tắc.
- **Acceptance intent**: Vòng thiếu câu không mở được; thông điệp dẫn thẳng sang màn sửa danh sách.
- **Source**: `QĐ-042`, `QĐ-003` · `GR-031` · **Status**: CONFIRMED

**PRD-REQ-049 — Trước mốc câu khép, đáp án chỉ rời server tới admin và MC**
- **Description**: Trước mốc **câu khép**, đáp án chuẩn và tiêu chí đạt của câu thực hành MUST chỉ tới admin và MC; mỗi lần xem MUST vào nhật ký. Từ mốc câu khép trở đi, server MUST đẩy đáp án tới **thí sinh, khán giả và lớp phủ dựng stream** nếu cờ *hiện đáp án sau khi chấm* **bật**. Cờ này MUST ở **cấp TRẬN**, mặc định **BẬT** cho cả trận official lẫn luyện tập, đổi được cho từng trận. Yêu cầu không có quyền MUST bị server im lặng từ chối.
- **Actor**: ACTOR-001, ACTOR-003, ACTOR-004, ACTOR-005, ACTOR-006 · **Related epic**: EPIC-006, EPIC-008, EPIC-009 · **Related journey**: JOURNEY-005 · **Related game rules**: `GR-037` · `INV-017`
- **Priority**: P1 · **Rationale**: Công bố đáp án sau khi câu khép là chuẩn của gameshow truyền hình và **không** đụng `GOAL-005` — tại mốc đó câu đã hỏi xong, đề đã lộ. Cờ vẫn phải ở cấp trận vì một contest thật chứa **cả** trận official lẫn trận luyện tập.
- **Acceptance intent**: Bộ kiểm *"không rò đáp án"* pass cho mọi kênh **ở mọi thời điểm trước mốc câu khép**, gồm cả gói khôi phục kết nối và lớp công bố kết quả.
- **Source**: `QĐ-051`, `QĐ-062`, `QĐ-080` · `GR-037` · `INV-017` · `TERM-057` · **Status**: CONFIRMED

**PRD-REQ-088 — Mốc công bố là CÂU KHÉP, không phải "đã chấm"**
- **Description**: Hệ thống MUST công bố đáp án tại mốc **câu khép** — thời điểm không còn ai được trả lời câu đó nữa. Ở Khởi động, Vượt chướng ngại vật và Tăng tốc, mốc này MUST trùng với cú bấm chấm. Ở **Về đích**, khi người thi chính bị chấm Sai, hệ thống MUST NOT công bố đáp án trong lúc cửa sổ cướp quyền còn mở; câu chỉ khép khi cửa sổ đóng **và** người cướp đã được chấm, hoặc hết cửa sổ không ai bấm. Ba ca biên: câu **bị bỏ qua** MUST vẫn công bố; phán quyết **Huỷ kết quả** MUST NOT tự công bố; đáp án **Chướng ngại vật** MUST NOT theo cơ chế này. Công bố MUST là một chiều ở phía engine; quyền đóng hiển thị thủ công của admin MUST giữ nguyên.
- **Actor**: ACTOR-007, ACTOR-001 · **Related epic**: EPIC-006 · **Related journey**: JOURNEY-005 · **Related game rules**: `GR-037`, `GR-020`, `GR-008`, `GR-012` · `INV-017`
- **Priority**: P1 · **Rationale**: Ở Về đích, cú bấm *chấm Sai* **chính là** cú mở cửa sổ cướp quyền. Công bố tại mốc đó thì người cướp chỉ việc đọc lại đáp án vừa hiện — cơ chế chuyển điểm lớn nhất trận bị xoá sổ và vòng quyết định thành cuộc thi bấm chuột. Đây là **sai luật gốc**, không phải lệch trải nghiệm.
- **Acceptance intent**: Ở Về đích, đáp án không xuất hiện trên bất kỳ kênh nào của thí sinh, khán giả hay lớp phủ trong khoảng từ cú bấm chấm Sai tới khi người cướp được chấm.
- **Source**: `QĐ-080` · `GR-037`, `GR-020` · `TERM-057` · **Status**: CONFIRMED

**PRD-REQ-089 — Đáp án chỉ được đẩy tại đúng mốc, không đẩy trước rồi ẩn**
- **Description**: Server MUST chỉ gửi đáp án tới thí sinh, khán giả và lớp phủ **tại đúng mốc câu khép**. Hệ thống MUST NOT gửi đáp án xuống client sớm hơn rồi dựa vào một cờ hiển thị phía client để giấu nó.
- **Actor**: ACTOR-007 · **Related epic**: EPIC-006 · **Related journey**: JOURNEY-005 · **Related game rules**: `GR-037` · `INV-017`
- **Priority**: P1 · **Rationale**: Đây chính là lỗi của hệ thống tiền lệ — nó nạp toàn bộ ngân hàng đề xuống mọi máy client dưới một phép mã hoá tầm thường, và thứ duy nhất giấu đáp án là một cờ hiển thị của giao diện; đáp án nằm sẵn trong bộ nhớ máy thí sinh suốt trận. Đó đúng là `PS-6`.
- **Acceptance intent**: Bắt gói tin và soi bộ nhớ máy thí sinh trước mốc câu khép ⇒ không có đáp án của câu đang mở.
- **Source**: `QĐ-080` · `CLAUDE.md` §Nguyên tắc code · **Status**: CONFIRMED

**PRD-REQ-090 — Câu hỏi phụ không có kho riêng; rút từ ba kho nguồn**
- **Description**: Hệ thống MUST NOT yêu cầu một kho Câu hỏi phụ khai riêng. Đề của vòng này MUST rút từ ba kho nguồn — **Về đích, Khởi động, Vượt chướng ngại vật** — theo thứ tự ưu tiên đó. Kho **Tăng tốc** MUST NOT là nguồn. Câu **thực hành** MUST bị loại khỏi phép rút. Câu mượn MUST bị bỏ qua thời lượng suy nghĩ của kho gốc *(luôn 15 giây)* và bỏ qua mức điểm *(vòng này không sinh điểm)*. Cửa vào vòng MUST vẫn kiểm đủ **3 câu khả dụng**.
- **Actor**: ACTOR-001, ACTOR-007 · **Related epic**: EPIC-004, EPIC-006 · **Related journey**: JOURNEY-002, JOURNEY-007 · **Related game rules**: `GR-022`, `GR-023`, `GR-025`, `GR-017`
- **Priority**: P2 · **Rationale**: Bắt admin chuẩn bị một kho riêng cho một vòng **có thể không bao giờ chạy** là chi phí đặt sai chỗ. Kho Về đích **luôn dư đúng 12 câu** sau khi vòng chạy xong — số dư tất định, không phụ thuộc lựa chọn của ai — nên nhánh phân định hoà được phục vụ **không tốn thêm câu nào**. Thứ tự ưu tiên đặt Về đích trước vì kho VCNV khan nhất và rút vào đó **làm vỡ một bộ**.
- **Acceptance intent**: Contest nạp đúng 69 câu + 1 bộ Chướng ngại vật ⇒ trận chạy trọn bốn vòng **và** mở được nhánh Câu hỏi phụ khi có hoà.
- **Source**: `QĐ-081` · `GR-023`, `GR-017` · `TERM-020`, `TERM-046` · **Status**: CONFIRMED

**PRD-REQ-091 — Admin chỉ định câu làm Câu hỏi phụ, tại LOBBY, dưới dạng ĐẶT CHỖ**
- **Description**: Ở **LOBBY**, admin MUST chỉ định được một số câu **còn available** *(chưa hiển thị)* làm Câu hỏi phụ, và MUST gỡ chỉ định được. Thao tác này MUST làm được ở **bất kỳ LOBBY nào** cho tới **mốc bấm Chốt trận**. Câu **đã hiển thị** MUST NOT chỉ định được. Chỉ định MUST là **đặt chỗ có hiệu lực từ thời điểm chỉ định**: câu đó bị loại khỏi phép rút của **các vòng mở sau đó**, và pre-flight của những vòng đó MUST phản ánh chi phí. Chỉ định **ít hơn 3 câu** ⇒ hệ thống MUST bù phần thiếu theo thứ tự ưu tiên, MUST NOT chặn cứng. Không chỉ định gì ⇒ rút hoàn toàn theo thứ tự ưu tiên.
- **Actor**: ACTOR-001 · **Related epic**: EPIC-005, EPIC-006 · **Related journey**: JOURNEY-006, JOURNEY-007 · **Related game rules**: `GR-023`, `GR-031`, `GR-022`
- **Priority**: P3 · **Rationale**: Chỉ định tồn tại vì **tính phù hợp**, không phải vì thiếu đề — một câu Về đích 30đ dài dòng là câu tie-break tệ. Phải là **đặt chỗ** chứ không phải danh sách ưu tiên, nếu không hệ thống sẽ **âm thầm bỏ qua chỉ định của admin** khi vòng gốc tiêu mất câu — đúng thứ `QĐ-074` cấm. Phải ở **LOBBY** chứ không ở cấu hình trận, vì cấu hình trận đóng băng từ cú bấm bắt đầu trận và admin khi đó **chưa biết có hoà hay không**. Ràng buộc *"còn available"* bịt đường hỏi lại một câu đã lộ.
- **Acceptance intent**: Chỉ định 3 câu ở **LOBBY cuối** *(sau khi Về đích đã chạy)* ⇒ pre-flight không đòi thêm câu nào. Chỉ định 3 câu Về đích ở **LOBBY trước Về đích** ⇒ pre-flight vòng Về đích đòi **27** thay vì 24, và ba câu đó không xuất hiện trong gói của thí sinh nào.
- **Source**: `QĐ-081`, `QĐ-074`, `QĐ-043` · **Status**: CONFIRMED

**PRD-REQ-092 — Vượt chướng ngại vật chọn và rút theo BỘ**
- **Description**: Đơn vị chọn tay và đơn vị rút của vòng Vượt chướng ngại vật MUST là một **bộ** gồm **1 Chướng ngại vật** *(từ khoá + hình ảnh 5 miếng ghép)*, **4 hàng ngang** và **1 câu ô trung tâm**. Bốn hàng ngang MUST mang số thứ tự **cố định** ứng với một miếng ghép ở một góc cố định, và MUST NOT hoán đổi được giữa các bộ hay cho nhau. Một bộ MUST chỉ khả dụng khi **cả sáu thành phần đều chưa dùng**. Kiểm kho vòng này MUST đếm **bộ nguyên vẹn**, MUST NOT đếm số câu. Người ra đề MUST soạn theo bộ, và nhập/xuất MUST đi theo bộ.
- **Actor**: ACTOR-002, ACTOR-001, ACTOR-007 · **Related epic**: EPIC-002, EPIC-004, EPIC-006 · **Related journey**: JOURNEY-001, JOURNEY-002 · **Related game rules**: `GR-031`, `GR-007`, `GR-008`, `GR-009`, `GR-011`
- **Priority**: P1 · **Rationale**: Luật gốc nói hàng ngang *"**cũng chính là 4 gợi ý liên quan đến** Chướng ngại vật"* và miếng ghép *"**được đánh số cố định**"*. Hàng ngang **không phải câu hỏi độc lập**. Ghép 4 hàng ngang bất kỳ với một Chướng ngại vật bất kỳ thì vòng **vẫn chạy trót lọt về mặt kỹ thuật** — đủ câu, đủ điểm, đủ miếng ghép — nhưng **không còn gì để suy ra**, và băng điểm 60/50/40/30 vốn thưởng cho suy luận sớm trở thành thưởng cho đoán mò. **Không phép kiểm nào bắt được lỗi này**, nên nó phải là ràng buộc dữ liệu.
- **Acceptance intent**: Kho có đủ 4 hàng ngang và 1 Chướng ngại vật nhưng **không cùng bộ** ⇒ vòng VCNV **không mở được**.
- **Source**: luật gốc §Vượt chướng ngại vật *(hai câu đầu)* · `QĐ-082` · `GR-031` · `TERM-058` · **Status**: CONFIRMED

**PRD-REQ-093 — Cảnh báo khi một thao tác làm vỡ bộ VCNV**
- **Description**: Khi admin chỉ định một hàng ngang, câu ô trung tâm, hoặc Chướng ngại vật làm Câu hỏi phụ, hệ thống MUST cảnh báo rằng thao tác đó **làm vỡ một bộ** và MUST nêu rõ bộ nào. Thứ tự ưu tiên rút tự động của Câu hỏi phụ MUST xếp kho VCNV **cuối cùng**.
- **Actor**: ACTOR-001 · **Related epic**: EPIC-006, EPIC-007 · **Related journey**: JOURNEY-006 · **Related game rules**: `GR-023`, `GR-031`
- **Priority**: P2 · **Rationale**: Mượn một hàng ngang làm câu tie-break là **mất 6 thành phần để lấy 1**. Kho VCNV là tài nguyên khan nhất — một bộ cho mỗi trận — nên một trận hoà có thể âm thầm ăn mất bộ của trận kế nếu không có cảnh báo và không có thứ tự ưu tiên.
- **Acceptance intent**: Chỉ định một hàng ngang làm Câu hỏi phụ ⇒ dialog nêu tên bộ sẽ vỡ; phép rút tự động không bao giờ chạm kho VCNV khi hai kho kia còn câu.
- **Source**: `QĐ-081`, `QĐ-082` · **Status**: CONFIRMED

**PRD-REQ-050 — Mất kết nối: giữ ghế, không có chính sách tự động**
- **Description**: Ghế mất kết nối MUST được giữ trong một ngưỡng chờ mặc định **120 giây** với đồng bộ lại trạng thái, và MUST hiện banner *"đang kết nối lại"*. Quá ngưỡng, hệ thống MUST **chỉ tô nổi bật** ghế trên màn admin kèm thời lượng; MUST NOT tự loại, tự xoá, hay tự vô hiệu hoá ghế nào. Mỗi lần mất kết nối MUST mở một cửa sổ mới tính lại từ đầu, **không cộng dồn, không giới hạn số lần**. Đồng hồ MUST NOT dừng. Gói khôi phục MUST NOT chứa đáp án hay bài làm của ghế khác, và MUST NOT sinh sự kiện.
- **Actor**: ACTOR-003, ACTOR-007 · **Related epic**: EPIC-006, EPIC-008 · **Related journey**: JOURNEY-006 · **Related game rules**: `GR-036`
- **Priority**: P1 · **Rationale**: Đây là lời giải cho `PS-5`. Cộng dồn ngưỡng chờ sẽ thành *"hình phạt cho mạng yếu"*. Mất kết nối MUST NOT mua thêm thời gian, nên client dựng lại từ **hạn chót theo server time**.
- **Acceptance intent**: Ghế quay lại giữa câu thấy đúng màn và đúng thời gian còn lại; ghế quá ngưỡng chỉ được tô nổi bật.
- **Source**: `QĐ-045`, `QĐ-046` · `GR-036` · `TERM-043` · **Status**: CONFIRMED

### EPIC-007 — Điều khiển và can thiệp của admin

**PRD-REQ-051 — Màn chấm: tô khác biệt ký tự so với đáp án gần nhất**
- **Description**: Màn chấm MUST hiển thị bài làm cạnh đáp án và tô khác biệt ký tự so với đáp án **gần nhất** trong danh sách đáp án được chấp nhận, đồng thời MUST cho xem cả danh sách. MUST giữ số chữ của đáp án như gợi ý phụ. Admin MUST xem được **lịch sử bài gửi** của một ghế. Chuẩn hoá MUST không phân biệt hoa thường, cắt khoảng trắng hai đầu, và gộp khoảng trắng thừa; bỏ dấu là **tuỳ chọn, mặc định tắt** và MUST chỉ đổi cách tô màu.
- **Actor**: ACTOR-001 · **Related epic**: EPIC-007 · **Related journey**: JOURNEY-005 · **Related game rules**: `GR-027`, `GR-026`
- **Priority**: P1 · **Rationale**: Tô theo đáp án đầu danh sách sinh báo động giả. Bài làm **gốc** phải được lưu; kết quả chuẩn hoá chỉ là tạm thời để hiển thị.
- **Acceptance intent**: Bài làm khớp đáp án thứ ba trong danh sách được tô sạch, không phải tô đỏ toàn bộ.
- **Source**: `QĐ-073`, `QĐ-010` · `GR-027` · **Status**: CONFIRMED

**PRD-REQ-052 — Bản hợp lệ và bản quá hạn hiện cạnh nhau, chấm được cả hai**
- **Description**: Khi một câu có cả bản hợp lệ và bản quá hạn, hệ thống MUST giữ **cả hai** và tô **đỏ** bản quá hạn. Khi chỉ có bản quá hạn, MUST giữ, tô đỏ, và cho thêm lựa chọn **Huỷ kết quả**. Bản quá hạn MUST NOT tự ghi đè bản hợp lệ và MUST NOT bị máy loại. Nút chấm MUST bật được cho cả hai.
- **Actor**: ACTOR-001 · **Related epic**: EPIC-007 · **Related journey**: JOURNEY-005 · **Related game rules**: `GR-006`, `GR-015`, `GR-035` · bảng §2.3
- **Priority**: P1 · **Rationale**: Khoá cứng bản quá hạn là lấy mất quyền phán quyết của admin — trái `PRD-REQ-036`.
- **Acceptance intent**: Công nhận một bản quá hạn ở Tăng tốc ⇒ cả bảng xếp hạng của câu đó tính lại.
- **Source**: `QĐ-029`, `QĐ-073` · `game-rules.md` §2.3 · **Status**: CONFIRMED

**PRD-REQ-053 — Tăng tốc chấm trên MỘT màn, bốn ghế cạnh nhau**
- **Description**: Ở Tăng tốc, admin MUST chấm toàn bộ các ghế trên **một** màn hiển thị cạnh nhau, MUST NOT chấm từng người ở các màn tách rời.
- **Actor**: ACTOR-001 · **Related epic**: EPIC-007 · **Related journey**: JOURNEY-005 · **Related game rules**: `GR-013`
- **Priority**: P2 · **Rationale**: Điểm ở đây là hàm của **thứ hạng** giữa những người được chấm Đúng; chấm rời từng người che mất chính quan hệ mà admin cần thấy.
- **Acceptance intent**: Chấm một câu Tăng tốc không phải chuyển màn.
- **Source**: `QĐ-073` · `product-discovery.md` §6 · **Status**: CONFIRMED

**PRD-REQ-054 — Nút chấm khoá tới hết giờ ở vòng gõ máy, sống suốt ở vòng nói**
- **Description**: Ở kênh **nói** *(mode sân khấu)*, nút chấm MUST bấm được bất cứ lúc nào. Ở kênh **gõ** *(mode nhập liệu, và Vượt chướng ngại vật + Tăng tốc luôn gõ máy)*, nút chấm MUST khoá tới khi hết giờ. Ở kênh **thực hành**, nút chấm MUST sống suốt. Giao diện MUST chỉ báo **vì sao** nút đang mờ.
- **Actor**: ACTOR-001 · **Related epic**: EPIC-007 · **Related journey**: JOURNEY-005 · **Related game rules**: `GR-019` · bảng §2.4
- **Priority**: P1 · **Rationale**: Thí sinh còn sửa đáp án tới khi hết giờ; chấm sớm ở vòng gõ máy là chấm trên một bản không phải bản luật công nhận.
- **Acceptance intent**: Ở Tăng tốc, nút chấm không bấm được trước mốc hết giờ, và giao diện nói rõ lý do.
- **Source**: `QĐ-030` · `game-rules.md` §2.4 · **Status**: CONFIRMED

**PRD-REQ-055 — Điều chỉnh điểm thủ công với lý do bắt buộc**
- **Description**: Admin MUST cộng hoặc trừ một lượng bất kỳ cho một ghế kèm **lý do bắt buộc**, ở **mọi lúc trận chưa đóng sổ**, kể cả giữa một vòng. Lượng bằng **0 vẫn MUST sinh sự kiện** — đó là một ghi chú chính thức vào biên bản. Sự kiện này MUST NOT tự hoàn nguyên khi vòng bị bỏ, vì nó là phán quyết của người và không thuộc vòng nào. Hệ thống MUST NOT chống trùng cho thao tác này.
- **Actor**: ACTOR-001 · **Related epic**: EPIC-007 · **Related journey**: JOURNEY-005, JOURNEY-006 · **Related game rules**: `GR-029`
- **Priority**: P1 · **Rationale**: Đây là **van thoát duy nhất cho mọi sai lầm trong trận** — chấm nhầm ở Tăng tốc, ghế bị vô hiệu hoá giữa câu, mặc định Sai oan. Ô lý do là kênh duy nhất ghi xuất xứ quyết định vào biên bản.
- **Acceptance intent**: Lượng 0 vẫn sinh một dòng trong biên bản; bỏ vòng không cuốn theo sự kiện này.
- **Source**: `QĐ-014`, `QĐ-035`, `QĐ-039` · `GR-029` · `TERM-024` · **Status**: CONFIRMED

**PRD-REQ-056 — Ba hạng cảnh báo giao diện, phân biệt bằng thứ chúng bảo vệ**
- **Description**: **Toast** cho invalid state — không ép được, MUST nêu rõ vì sao, MUST NOT bắt nhập lý do, và **mỗi loại invalid state MUST có thông điệp riêng**. **Dialog Yes/No** cho thao tác không hoàn tác được nhưng hợp lệ. **Dialog hạng phá huỷ** cho bỏ vòng, chạy lại vòng, kết thúc sớm, huỷ trận — MUST NOT tắt được bằng `Esc` hay click ra ngoài, và MUST bắt nhập lý do. Thao tác **đóng** hiển thị MUST NOT có dialog.
- **Actor**: ACTOR-001 · **Related epic**: EPIC-007 · **Related journey**: JOURNEY-005, JOURNEY-006 · **Related game rules**: `GR-030`
- **Priority**: P1 · **Rationale**: Trộn toast với dialog làm admin học sai mô hình — người vận hành phải phân biệt được *"không làm được"* với *"làm được nhưng nguy hiểm"*.
- **Acceptance intent**: Ba loại phản hồi phân biệt được bằng mắt; dialog phá huỷ không đóng được bằng `Esc`.
- **Source**: `QĐ-072`, `QĐ-004`, `QĐ-005` · `game-state-machine.md` §F · **Status**: CONFIRMED

**PRD-REQ-057 — Admin toàn quyền mở và đóng đáp án, ô chữ; thao tác tay thắng mọi cờ tự động**
- **Description**: Admin MUST mở và đóng đáp án cùng ô chữ không điều kiện; thao tác **mở** MUST đi qua dialog Yes/No. Khi admin bấm mở đáp án, nội dung MUST tới **mọi vai đang xem**, kể cả khi cờ *hiện đáp án sau khi chấm* đang tắt; mỗi lần mở MUST vào nhật ký. Mở và đóng hiển thị MUST NOT đụng tới điểm. Quyền này MUST độc quyền của phiên đang giữ quyền điều khiển; MC MUST NOT có nút nào.
- **Actor**: ACTOR-001 · **Related epic**: EPIC-007 · **Related journey**: JOURNEY-005 · **Related game rules**: `GR-037`
- **Priority**: P1 · **Rationale**: *"Không có khái niệm engine tự mở"* ⇒ nhật ký không cần phân biệt do-admin hay do-hệ-thống.
- **Acceptance intent**: Mở đáp án khi cờ đang tắt vẫn đẩy được tới khán giả và sinh một dòng nhật ký; điểm không đổi.
- **Source**: `QĐ-048`, `QĐ-074` · `GR-037` · **Status**: CONFIRMED

**PRD-REQ-058 — Ba trạng thái ô chữ, đặt được cả hai chiều**
- **Description**: Mỗi ô của bàn cờ Vượt chướng ngại vật — 4 hàng ngang và ô trung tâm — MUST mang đúng một trong ba giá trị **chờ → đã hỏi → mở**, và năm ô MUST độc lập với nhau. Admin MUST đặt thẳng một ô sang giá trị bất kỳ, **cả hai chiều**; thao tác đó MUST NOT sinh điểm và MUST NOT tiêu câu. Băng điểm Chướng ngại vật MUST là **hàm** đọc theo số hàng ngang không còn ở trạng thái chờ, MUST NOT là bộ đếm cộng dồn; ô trung tâm MUST NOT vào phép tính này.
- **Actor**: ACTOR-001 · **Related epic**: EPIC-007, EPIC-006 · **Related journey**: JOURNEY-005, JOURNEY-006 · **Related game rules**: `GR-009`, `GR-011`, `GR-012`
- **Priority**: P1 · **Rationale**: Đây là **đường duy nhất dựng lại bàn cờ sau sự cố**; bộ đếm cộng dồn thì đếm trùng và không lùi được.
- **Acceptance intent**: Đặt một ô về đúng giá trị nó đang có là thao tác rỗng; băng điểm tính lại đúng sau khi admin sửa tay.
- **Source**: `QĐ-052` · `TERM-053` · `EVENT-021` · **Status**: CONFIRMED

**PRD-REQ-059 — Hàng đợi hiện đầy đủ, tách bạch đang hoạt động với lịch sử**
- **Description**: Màn admin MUST hiện hàng đợi tín hiệu đầy đủ kèm timestamp và trạng thái *"không có hiệu lực"*, và MUST tách bạch **hàng đợi đang hoạt động** với **lịch sử tín hiệu**.
- **Actor**: ACTOR-001 · **Related epic**: EPIC-007 · **Related journey**: JOURNEY-005, JOURNEY-006 · **Related game rules**: `GR-032`
- **Priority**: P1 · **Rationale**: Lịch sử đầy đủ là van thoát thay cho ân hạn: admin xem lại và **gỡ lệnh cấm** khi cần.
- **Acceptance intent**: Sau khi hàng đợi đang hoạt động được đặt lại, lịch sử vẫn xem được nguyên vẹn.
- **Source**: `QĐ-020`, `QĐ-024`, `QĐ-025` · `product-discovery.md` §6 · **Status**: CONFIRMED

**PRD-REQ-060 — Ba trạng thái câu phân biệt được trên màn admin**
- **Description**: Màn admin MUST phân biệt rõ ba trạng thái: **chưa rút** · **đã rút chưa hiển thị** *(gỡ được)* · **đã hiển thị** *(không gỡ được)*.
- **Actor**: ACTOR-001 · **Related epic**: EPIC-007 · **Related journey**: JOURNEY-005 · **Related game rules**: `GR-031`
- **Priority**: P2 · **Rationale**: Ranh giới gỡ được / không gỡ được chính là ranh giới tiêu câu; nhìn nhầm là mất câu oan hoặc lộ đề.
- **Acceptance intent**: Ba trạng thái phân biệt được bằng mắt trên màn danh sách câu.
- **Source**: `QĐ-044` · `product-discovery.md` §6 · **Status**: CONFIRMED

**PRD-REQ-061 — Vô hiệu hoá và kích hoạt lại ghế**
- **Description**: Admin MUST tắt và bật lại quyền thao tác của một ghế, **đảo ngược được**, ở **mọi lúc** trong một trận chưa đóng sổ, kể cả giữa một câu đang mở. Ghế bị vô hiệu hoá MUST vẫn ở trong trận: giữ nguyên điểm, giữ nguyên vị trí, vẫn trên mọi bảng; chỉ mất quyền thao tác và không được đề xuất lượt. Thao tác này MUST là quyết định riêng, MUST NOT là hệ quả tự động của mất kết nối. Hệ thống MUST NOT có thao tác kick ở v1.
- **Actor**: ACTOR-001 · **Related epic**: EPIC-007 · **Related journey**: JOURNEY-006 · **Related game rules**: `GR-036`
- **Priority**: P2 · **Rationale**: Ràng buộc *"chỉ ở LOBBY"* sẽ làm mất đúng tình huống cần nhất — máy một ghế chết giữa vòng. Giữa một câu, ghế bị vô hiệu hoá xử **y như ghế không trả lời**; thấy bất công thì admin cộng tay.
- **Acceptance intent**: Vô hiệu hoá rồi kích hoạt lại không hoàn nguyên gì; điểm và bài đã chấm không đổi.
- **Source**: `QĐ-047` · `GR-036` · `TERM-032` · **Status**: CONFIRMED

**PRD-REQ-062 — Chốt câu khi mới chấm một phần: ghế chưa chấm mặc định SAI**
- **Description**: Ở **Tăng tốc** và **câu hàng ngang Vượt chướng ngại vật**, cú bấm *chốt câu* của admin MUST chính là phán quyết, và mọi ghế chưa được chấm MUST tính là Sai. Hệ thống MUST NOT có bộ đếm tự chốt. Dialog xác nhận MUST liệt kê tên các ghế sắp bị mặc định Sai. Quy tắc này MUST NOT áp lên tín hiệu *"Mở chướng ngại vật"* đang chờ duyệt.
- **Actor**: ACTOR-001 · **Related epic**: EPIC-007 · **Related journey**: JOURNEY-005 · **Related game rules**: `GR-026` C4, C5
- **Priority**: P1 · **Rationale**: Ngoại lệ cho tín hiệu *"Mở chướng ngại vật"* là bắt buộc: trả lời sai Chướng ngại vật ⇒ ghế **bị loại**, còn sai câu hàng ngang ⇒ 0 điểm và vẫn thi tiếp. Không có rào này thì admin chốt câu vì lý do khác sẽ **loại nhầm** một thí sinh.
- **Acceptance intent**: Chốt câu hàng ngang khi còn tín hiệu Chướng ngại vật chờ duyệt ⇒ tín hiệu đó không bị chấm Sai.
- **Source**: `QĐ-053` · `GR-026` · `T-045`, `T-053` · **Status**: CONFIRMED

**PRD-REQ-063 — Số nút chấm thay đổi theo vòng**
- **Description**: Giao diện chấm MUST đổi số nút theo vòng, thêm **Huỷ kết quả** ở vòng có hình phạt; MUST NOT dùng chung một bố cục hai nút cho mọi vòng.
- **Actor**: ACTOR-001 · **Related epic**: EPIC-007 · **Related journey**: JOURNEY-005 · **Related game rules**: `GR-026` · bảng §2.2
- **Priority**: P2 · **Rationale**: Bố cục cố định buộc admin nhớ vòng nào có nút thứ ba — một gánh nặng đặt sai chỗ giữa trận trực tiếp.
- **Acceptance intent**: Chuyển từ Khởi động lượt riêng sang lượt chung ⇒ số nút chấm đổi.
- **Source**: `QĐ-061` · `product-discovery.md` §6 · **Status**: CONFIRMED

### EPIC-008 — Trải nghiệm thí sinh

**PRD-REQ-064 — Chuông chỉ nhận click chuột và tự khoá ngay khi bấm**
- **Description**: Nút chuông MUST chỉ nhận click chuột; hệ thống MUST NOT gán phím tắt nào cho chuông. Nút *"Mở chướng ngại vật"* MUST được xếp là chuông ⇒ cũng chỉ nhận click chuột. Nút MUST tự khoá **ngay trong lần bấm đầu, ở giao diện, trước khi gửi**, và khoá MUST gắn với **một câu** — sang câu mới thì mở lại. Các phím tắt khác MUST giữ nguyên: gửi đáp án, chọn hàng ngang, xoá ô nhập.
- **Actor**: ACTOR-003 · **Related epic**: EPIC-008 · **Related journey**: JOURNEY-005 · **Related game rules**: `GR-034`, `GR-003`, `GR-009`
- **Priority**: P1 · **Rationale**: Tránh bấm nhầm khi đang gõ đáp án. *"Không tồn tại phím nào được gán"* — khác hẳn với *"gán rồi bỏ qua"*.
- **Acceptance intent**: Không tổ hợp phím nào phát được tín hiệu chuông; bấm chuông hai lần trong một câu chỉ sinh một tín hiệu ở phía client.
- **Source**: `QĐ-023` · `GR-034` · `TERM-025` · `CLAUDE.md` §UX · **Status**: CONFIRMED

**PRD-REQ-065 — Nút chuông và nút gửi có vòng đời NGƯỢC nhau**
- **Description**: Nút chuông MUST tự khoá khi bấm. Nút gửi đáp án MUST **không** khoá sau khi gửi và MUST sống tới khi hết giờ; hệ thống MUST ghi nhận **bản cuối cùng** trong các bản hợp lệ. Bản rỗng sau khi cắt khoảng trắng MUST bị bỏ qua. Lịch sử các bản đã gửi MUST NOT bị xoá.
- **Actor**: ACTOR-003 · **Related epic**: EPIC-008 · **Related journey**: JOURNEY-005 · **Related game rules**: `GR-006`, `GR-015`, `GR-034`
- **Priority**: P1 · **Rationale**: Hai nút này dễ bị cài chung một mẫu, và cài chung là sai ở cả hai đầu — hoặc chuông không khoá, hoặc thí sinh mất quyền sửa đáp án.
- **Acceptance intent**: Gửi đáp án ba lần trong cửa sổ thời gian ⇒ bản thứ ba được ghi nhận, cả ba vẫn trong lịch sử.
- **Source**: `QĐ-023`, `QĐ-029` · `product-discovery.md` §6 · `CLAUDE.md` §UX · **Status**: CONFIRMED

**PRD-REQ-066 — Tăng tốc: nhận mọi lần trả lời tới khi hết giờ, tính bản cuối**
- **Description**: Ở Tăng tốc, hệ thống MUST NOT khoá ô nhập hay nút gửi sau lần trả lời đầu. Thứ hạng MUST tính theo **server-received timestamp của bản cuối**. Bản có nội dung **khác** bản trước MUST cập nhật cả nội dung lẫn mốc; bản có nội dung **y hệt** sau khi cắt khoảng trắng MUST NOT cập nhật mốc.
- **Actor**: ACTOR-003, ACTOR-007 · **Related epic**: EPIC-008, EPIC-006 · **Related journey**: JOURNEY-005 · **Related game rules**: `GR-015`, `GR-013`
- **Priority**: P1 · **Rationale**: Nếu bản trùng nội dung cũng cập nhật mốc thì thí sinh **tự làm xấu** thứ hạng của mình bằng một thao tác vô nghĩa.
- **Acceptance intent**: Gửi lại đúng chuỗi cũ không làm tụt thứ hạng.
- **Source**: `QĐ-059`, `QĐ-029` · `GR-015` · `TERM-037` · **Status**: CONFIRMED

**PRD-REQ-067 — Máy thí sinh hiện bảng điểm của TẤT CẢ các ghế, realtime**
- **Description**: Màn thí sinh MUST hiện bảng điểm của **tất cả** các ghế, không chỉ điểm của mình, và MUST cập nhật theo thời gian thực như màn khán giả — gồm cả lúc hoàn nguyên làm điểm tụt đột ngột. Điểm âm MUST hiện bình thường.
- **Actor**: ACTOR-003 · **Related epic**: EPIC-008 · **Related journey**: JOURNEY-005 · **Related game rules**: `GR-028` · `INV-017` *(điểm là công khai)*
- **Priority**: P1 · **Rationale**: Điểm là thông tin công khai với mọi vai; chỉ **đáp án** bị giới hạn. `product-discovery.md` §6 xếp đây là *"chỗ dễ cài thiếu nhất"*.
- **Acceptance intent**: Thí sinh thấy điểm mọi ghế đổi cùng lúc với khán giả.
- **Source**: `QĐ-012`, `QĐ-015` · `product-discovery.md` §6 · **Status**: CONFIRMED

**PRD-REQ-068 — Thao tác của thí sinh: tức thời, không dialog, không rút lại**
- **Description**: Mọi thao tác đua tốc độ của thí sinh — bấm chuông, *"Mở chướng ngại vật"*, gửi đáp án — MUST có hiệu lực tức thời, MUST NOT có dialog xác nhận, và MUST NOT rút lại được. **Ngoại lệ duy nhất**: chọn hàng ngang ở mode nhập liệu MUST có dialog xác nhận trên máy thí sinh.
- **Actor**: ACTOR-003 · **Related epic**: EPIC-008 · **Related journey**: JOURNEY-005 · **Related game rules**: `GR-007`, `GR-034`
- **Priority**: P1 · **Rationale**: *"Thí sinh cần tốc độ, và tự chịu trách nhiệm sai lầm của mình."* Chỗ sửa lỗi bấm nhầm của thí sinh là admin bấm No ở hàng đợi, không phải một nút huỷ. Ngoại lệ tồn tại vì chọn hàng ngang là thao tác **một chiều, hậu quả nặng, không bị ép thời gian**.
- **Acceptance intent**: Không màn thí sinh nào có dialog trên đường bấm chuông hay gửi đáp án.
- **Source**: `QĐ-005`, `QĐ-019` · `TERM-001` · `CLAUDE.md` §UX · **Status**: CONFIRMED

**PRD-REQ-069 — Chọn hàng ngang: MỘT đường vào cho mỗi mode, nút có BA trạng thái**
- **Description**: Ở mode sân khấu, **chỉ admin** MUST chọn được hàng ngang và máy thí sinh MUST NOT render nút chọn. Ở mode nhập liệu, **chỉ thí sinh** MUST chọn được và admin MUST NOT chọn thay. Cả hai đường đều MUST đi qua hàng đợi chặn và admin xác nhận. Nút chọn của thí sinh MUST có **ba** trạng thái: sống · khoá chờ duyệt · **mở lại khi admin từ chối**. Cùng quy tắc một-đường-vào MUST áp cho đặt Ngôi sao hy vọng.
- **Actor**: ACTOR-001, ACTOR-003 · **Related epic**: EPIC-008, EPIC-006 · **Related journey**: JOURNEY-005 · **Related game rules**: `GR-007`, `GR-021`
- **Priority**: P1 · **Rationale**: Hai đường vào song song cho cùng một thao tác tạo tranh chấp không có trọng tài. Trạng thái thứ ba dễ bị bỏ sót, và bỏ sót là **thí sinh mất lượt trái luật**.
- **Acceptance intent**: Admin bấm No ⇒ nút chọn của thí sinh mở lại và thí sinh chọn lại được.
- **Source**: `QĐ-019`, `QĐ-022` · `GR-007` · `product-discovery.md` §6 · **Status**: CONFIRMED

**PRD-REQ-070 — Chọn gói câu Về đích: một đường vào theo mode, có fallback của admin**
- **Description**: Gói câu MUST gồm **đúng ba mục**, mỗi mục thuộc hai mức điểm cấu hình được. Ở mode sân khấu, **admin** MUST bấm theo lời thí sinh và máy thí sinh MUST NOT render nút. Ở mode nhập liệu, **thí sinh** MUST chọn, và admin MUST có fallback chọn hộ một gói mặc định khi thí sinh không kịp. Đổi gói MUST theo nguyên tắc bản-cuối-thắng tới mốc admin bấm hiển thị **câu đầu tiên**, sau đó khoá.
- **Actor**: ACTOR-001, ACTOR-003 · **Related epic**: EPIC-008, EPIC-006 · **Related journey**: JOURNEY-005 · **Related game rules**: `GR-017`
- **Priority**: P1 · **Rationale**: Đây là ngoại lệ **có chủ ý** so với chọn hàng ngang: chọn gói giữ fallback của admin, chọn hàng ngang thì cấm tuyệt đối. Khoá tại mốc hiển thị câu đầu vì từ đó đề đã rời server.
- **Acceptance intent**: Ở mode sân khấu, máy thí sinh không có nút chọn gói và server từ chối nếu tín hiệu lọt tới.
- **Source**: `QĐ-019` · `GR-017` · **Status**: CONFIRMED

**PRD-REQ-071 — Máy thí sinh không render control ở invalid state; `Esc` không quay lại**
- **Description**: Ở trạng thái không hợp lệ, máy thí sinh MUST NOT render control tương ứng và thao tác MUST không phản hồi. Ở màn thi đấu, `Esc` MUST NOT là nút quay lại: ở mode nhập liệu nó chỉ xoá ô nhập, ở mode sân khấu nó MUST không làm gì.
- **Actor**: ACTOR-003 · **Related epic**: EPIC-008 · **Related journey**: JOURNEY-005 · **Related game rules**: `GR-034`
- **Priority**: P2 · **Rationale**: Trình duyệt dùng `Esc` để thoát toàn màn hình — biến nó thành nút quay lại sẽ làm văng khỏi chế độ toàn màn hình giữa trận.
- **Acceptance intent**: Nhấn `Esc` ở mode sân khấu không đổi màn hình và không rời trận.
- **Source**: `QĐ-004`, `QĐ-072` · `CLAUDE.md` §Điều hướng · **Status**: CONFIRMED

**PRD-REQ-072 — Không chặn gửi lại; khoá theo LUẬT CHƠI thì vẫn khoá**
- **Description**: Nút hành động MUST chỉ hiện trạng thái đang xử lý, MUST NOT bị vô hiệu hoá để chống gửi trùng; người dùng MUST gửi lại được và server nhận **bản cuối cùng** trước hạn. Chống trùng là việc của server. Khoá **theo luật chơi** — chuông đã bấm, Ngôi sao hy vọng đã dùng, chưa tới lượt — MUST vẫn vô hiệu hoá bình thường, và hệ thống MUST đánh dấu rõ đó là khoá theo luật chơi.
- **Actor**: ACTOR-001, ACTOR-003 · **Related epic**: EPIC-007, EPIC-008 · **Related journey**: JOURNEY-005 · **Related game rules**: `GR-034`, `GR-006`
- **Priority**: P2 · **Rationale**: Ghi rõ ranh giới để lần rà quy tắc giao diện sau không ai gỡ nhầm khoá theo luật chơi.
- **Acceptance intent**: Có một thành phần dùng chung cho nút một chiều — chấm, start timer, chuông, chuyển câu — kèm ghi chú phân loại.
- **Source**: `QĐ-060` · `CLAUDE.md` §UX · `product-discovery.md` §6 · **Status**: CONFIRMED

### EPIC-009 — Trình diễn

**PRD-REQ-073 — Màn khán giả và lớp phủ dựng stream**
- **Description**: Hệ thống MUST có màn khán giả và một lớp phủ 1920×1080 nền trong suốt cho phần mềm dựng hình, cả hai vào bằng mã phòng 6 số. Lớp phủ MUST **không bao giờ** nhận đáp án.
- **Actor**: ACTOR-005, ACTOR-006 · **Related epic**: EPIC-009 · **Related journey**: JOURNEY-004, JOURNEY-005 · **Related game rules**: `GR-037`
- **Priority**: P1 · **Rationale**: Đây là lời giải cho `PS-4` — không cần dựng hình thủ công.
- **Acceptance intent**: Lớp phủ chồng lên video mà không có nền; bộ kiểm không-rò-đáp-án pass trên kênh này kể cả khi cờ hiện đáp án đang bật.
- **Source**: `product-discovery.md` §2 A-6, §3 G-8 · `QĐ-051` · **Status**: CONFIRMED

**PRD-REQ-074 — Màn MC chữ lớn, read-only tuyệt đối**
- **Description**: MC MUST có màn riêng chữ rất to hiển thị câu hỏi **và đáp án**. Màn này MUST là chỉ đọc tuyệt đối và MUST NOT có bất kỳ nút điều khiển nào.
- **Actor**: ACTOR-004 · **Related epic**: EPIC-009 · **Related journey**: JOURNEY-005 · **Related game rules**: `GR-037`
- **Priority**: P1 · **Rationale**: MC là người đọc câu hỏi; không có màn này thì MC phải nhìn màn admin và hỏng luồng thi đấu. `product-discovery.md` §8 ghi rõ: *"nếu chỉ giữ được một thứ ngoài màn khán giả, giữ màn MC."*
- **Acceptance intent**: Không có bề mặt quyền ghi nào trên màn MC; server từ chối mọi sự kiện ghi từ vai này.
- **Source**: `QĐ-001` · `product-discovery.md` §2 A-4, §8 · **Status**: CONFIRMED

**PRD-REQ-075 — Lớp phủ công bố kết quả**
- **Description**: Màn công bố kết quả MUST là **lớp phủ** chồng lên trạng thái đang chạy mà không huỷ nó, và MUST áp cho **cả máy thí sinh**. Hệ thống MUST gợi ý mở ở hai mốc — hết vòng và hết trận — nhưng admin MUST mở và đóng tuỳ ý; MUST NOT có bộ đếm tự đóng. Thứ hạng MUST do server tính và đẩy xuống; client chỉ hiển thị. Hoà điểm MUST ghi **đồng hạng**, hạng kế nhảy qua số người đồng hạng. Bảng xếp hạng MUST chịu được điểm âm và MUST NOT hard-code số ghế.
- **Actor**: ACTOR-001, ACTOR-005 · **Related epic**: EPIC-009 · **Related journey**: JOURNEY-005, JOURNEY-007 · **Related game rules**: `GR-028`, `GR-025`
- **Priority**: P2 · **Rationale**: Là lớp phủ nên nó không cần một trạng thái riêng và không làm mất trạng thái đang chạy bên dưới.
- **Acceptance intent**: Mở lớp công bố giữa một vòng rồi đóng lại ⇒ vòng tiếp tục đúng chỗ cũ.
- **Source**: `QĐ-049` · `STATE-033` · **Status**: CONFIRMED

**PRD-REQ-076 — Banner tạm dừng: chặn toàn cục, không chữ**
- **Description**: Banner tạm dừng MUST chặn toàn bộ thao tác trên máy thí sinh và báo tạm dừng trên màn khán giả cùng lớp phủ, **không chữ, không lý do**; MUST NOT phủ màn admin. Banner MUST chỉ bật được khi **không đồng hồ nào đang chạy**, và ngược lại hệ thống MUST NOT cho start timer khi banner đang bật.
- **Actor**: ACTOR-001 · **Related epic**: EPIC-009 · **Related journey**: JOURNEY-006 · **Related game rules**: `GR-035` · `INV-016`
- **Priority**: P2 · **Rationale**: Ràng buộc hai chiều làm câu hỏi *"banner có đóng băng đồng hồ không"* mất chủ ngữ — không tồn tại thời điểm nào banner và một đồng hồ đang chạy cùng có mặt, nên bất biến *đồng hồ không đóng băng* không cần ngoại lệ.
- **Acceptance intent**: Nút start timer không bật khi banner đang bật, và ngược lại.
- **Source**: `QĐ-050` · `STATE-039`, `T-089` · **Status**: CONFIRMED

**PRD-REQ-077 — Khán giả KHÔNG được báo về can thiệp của admin**
- **Description**: Bỏ vòng, chạy lại vòng, sửa danh sách đề, gỡ lệnh cấm MUST NOT sinh thông báo nào tới khán giả; điểm và bàn cờ MUST đổi **đột ngột, không hiệu ứng, không giải thích**. Khuyến nghị lượt MUST chỉ tới admin và MC.
- **Actor**: ACTOR-005, ACTOR-006 · **Related epic**: EPIC-009 · **Related journey**: JOURNEY-005 · **Related game rules**: `GR-030`
- **Priority**: P2 · **Rationale**: Người giải thích là **MC**, không phải giao diện. Đẩy khuyến nghị lượt xuống khán giả là lộ thứ tự sắp tới ⇒ trao một lợi thế mà luật không định trao.
- **Acceptance intent**: Sau khi admin bỏ một vòng, kênh khán giả không có bản tin nào ngoài số điểm mới.
- **Source**: `QĐ-076` · `product-discovery.md` §6 · **Status**: CONFIRMED

**PRD-REQ-078 — Chủ đề và âm thanh tuỳ chỉnh; khe trống là im lặng**
- **Description**: Hệ thống MUST cho admin tải lên file âm thanh cho từng khe sự kiện, và danh sách khe MUST phủ **cả sự kiện điều khiển** — hoàn nguyên, bỏ vòng, chạy lại vòng, kết thúc sớm, ép qua cảnh báo, mở màn công bố. Khe trống MUST là im lặng; hệ thống MUST NOT có bộ âm thanh mặc định. Contest MUST cấu hình được chủ đề hiển thị.
- **Actor**: ACTOR-001 · **Related epic**: EPIC-009 · **Related journey**: JOURNEY-002 · **Related game rules**: —
- **Priority**: P3 · **Rationale**: Engine chỉ phát tín hiệu ngữ nghĩa; ánh xạ tín hiệu sang âm thanh là cấu hình phía client, nên sửa luật không đụng âm thanh và ngược lại.
- **Acceptance intent**: Bỏ một vòng phát đúng khe âm thanh đã gán; khe chưa gán không phát gì và không báo lỗi.
- **Source**: `QĐ-079` · `CLAUDE.md` §Quy ước khác · **Status**: CONFIRMED
- **Ghi chú rủi ro**: xem `RISK-005` — không có bộ âm thanh mặc định nghĩa là sản phẩm ra mắt sẽ **hoàn toàn im lặng** nếu admin không chuẩn bị.

### EPIC-010 — Sau trận

**PRD-REQ-079 — Biên bản trận in theo LẦN CHẠY**
- **Description**: Một vòng chạy nhiều lần MUST in thành **nhiều khối** theo thứ tự thời gian, mỗi khối kèm số lần chạy và một nhãn: *đã bỏ* · *đã chạy lại* · *kết thúc sớm* · *hoàn thành*. Biên bản MUST NOT gộp hay giấu lần hỏng. Sự kiện hoàn nguyên MUST hiện như mọi sự kiện khác, kèm tên người bấm và lý do.
- **Actor**: ACTOR-001 · **Related epic**: EPIC-010 · **Related journey**: JOURNEY-007 · **Related game rules**: `GR-028`, `GR-030`
- **Priority**: P2 · **Rationale**: Biên bản là công cụ phân xử khiếu nại; gộp các lần chạy là xoá đúng thông tin mà người phân xử cần.
- **Acceptance intent**: Trận có một vòng bị bỏ và chạy lại ⇒ biên bản có hai khối riêng với hai nhãn khác nhau.
- **Source**: `QĐ-077` · `product-discovery.md` §6 · **Status**: CONFIRMED

**PRD-REQ-080 — Xuất biên bản trước khi dọn dữ liệu**
- **Description**: Job dọn dữ liệu theo hạn lưu trữ MUST cảnh báo trước và MUST NOT đụng vào biên bản đã xuất.
- **Actor**: ACTOR-001 · **Related epic**: EPIC-010, EPIC-011 · **Related journey**: JOURNEY-007 · **Related game rules**: —
- **Priority**: P3 · **Rationale**: Hạn lưu trữ phục vụ quyền riêng tư, nhưng nó không được xoá mất bằng chứng phân xử mà đơn vị tổ chức đã chủ động giữ.
- **Acceptance intent**: Sau khi job dọn chạy, biên bản đã xuất vẫn còn.
- **Source**: `QĐ-077` · `QĐ-040` · **Status**: CONFIRMED

**PRD-REQ-081 — Thống kê ghi ngược kho đề**
- **Description**: Sau khi trận đóng sổ, hệ thống MUST ghi số liệu sử dụng ngược lại kho đề. Yêu cầu này chỉ áp cho trận chạy **trên cùng bản cài** — trận chạy trên bản portable đóng góp đúng cờ *đã dùng* qua `PRD-REQ-095`, MUST NOT kỳ vọng số liệu thống kê.
- **Actor**: ACTOR-001, ACTOR-002 · **Related epic**: EPIC-010 · **Related journey**: JOURNEY-007 · **Related game rules**: `GR-031`
- **Priority**: P3 · **Rationale**: Không có vòng phản hồi này thì người ra đề không biết câu nào quá dễ hay quá khó.
- **Acceptance intent**: Câu đã dùng hiện số liệu sử dụng trong kho đề.
- **Source**: `product-discovery.md` §4 J-7, §5 E-10 · `QĐ-084` · **Status**: CONFIRMED

**PRD-REQ-096 — Xuất toàn bộ kết quả và nhật ký sự kiện của contest**
- **Description**: Hệ thống MUST xuất được một gói cấp contest gồm **mọi trận**: nhật ký sự kiện đầy đủ, điểm, thứ hạng, sự kiện hoàn nguyên kèm người bấm và lý do. Gói MUST tham chiếu câu hỏi bằng **định danh**, và MUST NOT nhúng nội dung câu hay đáp án trừ khi người xuất có quyền đọc kho đề. Mỗi lần xuất MUST vào nhật ký thao tác. Gói xuất từ trận chưa đóng sổ MUST mang dấu **"trận chưa đóng sổ"**. Gói này **chỉ có chiều ra** — hệ thống MUST NOT nhập nó trở lại.
- **Actor**: ACTOR-001 · **Related epic**: EPIC-010 · **Related journey**: JOURNEY-007 · **Related game rules**: `GR-028`, `GR-030`, `GR-037`
- **Priority**: P2 · **Rationale**: Đây là bằng chứng phân xử ở dạng máy đọc được, và là van thoát của hạn lưu trữ — gói đã xuất nằm ngoài hệ thống nên job dọn không chạm tới.
- **Acceptance intent**: Gói xuất ra chứa đủ các lần chạy của một vòng bị bỏ rồi chạy lại; xuất giữa trận cho ra gói có dấu chưa đóng sổ; xuất bởi tài khoản không có quyền đọc kho đề cho ra gói không có nội dung câu.
- **Source**: `QĐ-084` · `QĐ-077` · **Status**: CONFIRMED

**PRD-REQ-097 — Xuất kết quả rút gọn**
- **Description**: Hệ thống MUST xuất được gói kết quả rút gọn gồm bảng điểm cuối, thứ hạng và người thắng theo từng trận. Gói này MUST NOT chứa đáp án, nội dung câu hỏi, hay định danh câu hỏi. Chỉ xuất được từ trận đã đóng sổ. **Chỉ có chiều ra.**
- **Actor**: ACTOR-001 · **Related epic**: EPIC-010 · **Related journey**: JOURNEY-007 · **Related game rules**: `GR-022`, `GR-028`
- **Priority**: P3 · **Rationale**: Công bố và báo cáo chỉ cần kết quả; đưa cả nhật ký sự kiện ra ngoài cho việc đó là mở rộng bề mặt rò đề mà không được gì.
- **Acceptance intent**: Xuất từ trận chưa đóng sổ bị từ chối kèm lý do; gói xuất ra không chứa định danh câu nào.
- **Source**: `QĐ-084` · **Status**: CONFIRMED

> **Bốn gói xuất, một nguồn.** `PRD-REQ-016` *(gói contest)* đọc cấu hình contest và kho đề. `PRD-REQ-096` *(kết quả và nhật ký)* đọc nhật ký sự kiện, và **`PRD-REQ-095` cùng `PRD-REQ-097` là phép chiếu của nó** — MUST NOT hiện thực thành ba đường sinh dữ liệu độc lập. Biên bản PDF (`PRD-REQ-079`) là **một trận, cho người đọc**; hai gói kia là **cấp contest, cho máy đọc**; cả ba MUST sinh từ cùng một nguồn.

### EPIC-011 — Dữ liệu cá nhân và quyền riêng tư

**PRD-REQ-082 — Nhật ký mọi thao tác, mọi vai**
- **Description**: Hệ thống MUST ghi một nhật ký chung, chỉ thêm, cho **mọi thao tác của mọi vai**: đăng nhập thành công và thất bại, thao tác trên kho đề, quản lý contest, mọi sự kiện trong trận, khán giả vào và bị đuổi, nhập và xuất, và **mỗi lần xem đáp án**. Mỗi dòng MUST có người thực hiện, hành động, đối tượng, thời điểm và địa chỉ nguồn.
- **Actor**: mọi actor · **Related epic**: EPIC-011 · **Related journey**: mọi journey · **Related game rules**: `GR-037`
- **Priority**: P1 · **Rationale**: Đây là điều kiện để phân xử khiếu nại và để chứng minh đáp án không rò.
- **Acceptance intent**: Mỗi sự kiện bị khiếu nại có đủ thời điểm và chuỗi thao tác trong nhật ký.
- **Source**: `CLAUDE.md` §Quy ước khác · `QĐ-071`, `QĐ-074`, `QĐ-008`, `QĐ-065` · `TERM-021` · **Status**: CONFIRMED

**PRD-REQ-083 — Tuỳ chọn dùng biệt danh thay tên thật**
- **Description**: Contest MUST có tuỳ chọn hiển thị biệt danh thay cho tên thật của thí sinh trên các kênh public.
- **Actor**: ACTOR-001 · **Related epic**: EPIC-011 · **Related journey**: JOURNEY-002 · **Related game rules**: —
- **Priority**: P2 · **Rationale**: Ai có mã phòng đều thấy tên và thông tin trường lớp của học sinh vị thành niên; tuỳ chọn này là biện pháp giảm thiểu duy nhất hiện có.
- **Acceptance intent**: Bật tuỳ chọn ⇒ kênh khán giả và lớp phủ hiện biệt danh.
- **Source**: `product-discovery.md` §7 AS-5 · **Status**: NEEDS CLARIFICATION — *tuỳ chọn này có nên bật mặc định cho trận có thí sinh vị thành niên không?* (`QUESTION-004`)

**PRD-REQ-084 — Disclaimer khi tải nhạc lên**
- **Description**: Hệ thống MUST hiện disclaimer về bản quyền khi người dùng tải file âm thanh lên.
- **Actor**: ACTOR-001 · **Related epic**: EPIC-011 · **Related journey**: JOURNEY-002 · **Related game rules**: —
- **Priority**: P3 · **Rationale**: Hệ thống không kiểm được nguồn gốc nhạc nền do người dùng cung cấp; `product-discovery.md` §3 nâng disclaimer này thành **yêu cầu chính thức** thay vì để ở mục rủi ro.
- **Acceptance intent**: Không tải được file âm thanh mà không thấy disclaimer.
- **Source**: `product-discovery.md` §3 *(pháp lý)* · **Status**: CONFIRMED

### EPIC-012 — Vận hành và hai hồ sơ triển khai

**PRD-REQ-085 — Hai hồ sơ triển khai**
- **Description**: Sản phẩm MUST triển khai được theo hai hồ sơ: một hồ sơ máy chủ dựng bằng container, và một hồ sơ **portable chạy LAN offline trên Windows**. Hồ sơ portable MUST hoạt động đầy đủ không cần Internet.
- **Actor**: ACTOR-001 · **Related epic**: EPIC-012 · **Related journey**: JOURNEY-003, JOURNEY-004 · **Related game rules**: —
- **Priority**: P1 · **Rationale**: Ngày thi ở hội trường thường không có mạng ổn định; đây là điều kiện để giải `PS-2` mà không quay về mô hình chỉ-LAN.
- **Acceptance intent**: Chạy trọn một trận trên bản portable với máy chủ ngắt Internet.
- **Source**: `GOAL-010` · `product-discovery.md` §3 G-10, §5 E-12 · **Status**: CONFIRMED

**PRD-REQ-098 — Bốn loại gói xuất có đủ ở cả hai hồ sơ triển khai**
- **Description**: Cả hồ sơ máy chủ dựng bằng container lẫn hồ sơ portable MUST xuất được đủ bốn loại gói: gói contest (`PRD-REQ-016`), toàn bộ kết quả và nhật ký sự kiện (`PRD-REQ-096`), bản kê câu đã dùng (`PRD-REQ-095`), và kết quả rút gọn (`PRD-REQ-097`). Việc xuất MUST NOT phụ thuộc dịch vụ ngoài hay kết nối Internet.
- **Actor**: ACTOR-001 · **Related epic**: EPIC-012, EPIC-003, EPIC-010 · **Related journey**: JOURNEY-003, JOURNEY-007 · **Related game rules**: —
- **Priority**: P2 · **Rationale**: Bản portable là nơi trận thật chạy; nếu nó xuất được ít hơn thì dữ liệu của chính những trận quan trọng nhất bị mắc kẹt trong máy.
- **Acceptance intent**: Xuất đủ bốn gói trên bản portable đã ngắt Internet.
- **Source**: `QĐ-084` · **Status**: CONFIRMED

**PRD-REQ-086 — Rate-limit và khoá cổng phòng khán giả**
- **Description**: Cổng vào của khán giả MUST có giới hạn tần suất **cấu hình được**, và admin MUST có thao tác **khoá cổng** phòng.
- **Actor**: ACTOR-001 · **Related epic**: EPIC-012, EPIC-001 · **Related journey**: JOURNEY-004 · **Related game rules**: —
- **Priority**: P2 · **Rationale**: Mã 6 số là công khai, nên cổng khán giả là bề mặt duy nhất mở với Internet.
- **Acceptance intent**: Khoá cổng ⇒ khán giả mới không vào được, khán giả đang xem không bị ngắt.
- **Source**: `CLAUDE.md` §Mô hình truy cập · `QĐ-067` · **Status**: CONFIRMED *(ngưỡng cụ thể: xem `PRD-REQ-087`)*

**PRD-REQ-087 — Số viewer đồng thời và ngưỡng độ trễ dùng mặc định cấu hình được**
- **Description**: Không rule, không chuyển tiếp, không đặc tả nào MUST được viết dựa trên một con số viewer đồng thời hay một ngưỡng độ trễ cụ thể. Mọi chỗ cần con số MUST dùng **mặc định cấu hình được**.
- **Actor**: ACTOR-001 · **Related epic**: EPIC-012 · **Related journey**: JOURNEY-004 · **Related game rules**: —
- **Priority**: P2 · **Rationale**: Đây là mục tiêu phi chức năng phụ thuộc sức chứa hội trường và phần cứng thật; chốt bừa một con số biến một giá trị cấu hình thành một cam kết.
- **Acceptance intent**: Đổi ngưỡng bằng cấu hình môi trường mà không sửa code.
- **Source**: `QĐ-067` · **Status**: NEEDS CLARIFICATION — *chủ dự án đã **hoãn có chủ đích**; khi cần chốt phải trả lời (a) số viewer đồng thời tối đa ở hồ sơ portable LAN, (b) hồ sơ container có cần con số cao hơn không* (`QUESTION-003`)

---

## 13. Business Rules

> Tóm tắt **các nhóm** rule. Nội dung chuẩn tắc ở `docs/game-rules.md` — **không sao chép lại ở đây**.

### 13.1 Hai mươi ba nguyên tắc nền

`game-rules.md` mở đầu bằng 23 mệnh đề chi phối mọi rule. Sáu mệnh đề có sức chi phối rộng nhất:

| Nguyên tắc | Nội dung | Quyết định |
|---|---|---|
| **Máy giữ sự kiện, người phán quyết** | Mọi outcome đúng/sai là kết quả **sau khi** admin bấm | `QĐ-001`, `QĐ-010` |
| **Admin là cảm biến** | Mốc luật gốc mô tả bằng hành vi MC ⇒ một cú bấm admin, tuyệt đối, không ân hạn | `QĐ-006`, `QĐ-027` |
| **Không có cơ chế drop** | Mọi tín hiệu vào hàng đợi theo server timestamp; lịch sử không bao giờ xoá | `QĐ-020` |
| **Chỉ chặn cứng ở ba chỗ** | Mọi lệch luật khác chỉ cảnh báo, admin ép được | `QĐ-003` |
| **Invalid state là nhánh KHÔNG TỒN TẠI** | Khác hẳn chặn cứng — không có đường vào, không ép được | `QĐ-004` |
| **Điểm là hàm của nhật ký sự kiện** | Hoàn nguyên bằng cách thêm sự kiện đảo ngược | `QĐ-011` |

### 13.2 Bốn bảng dùng chung

| Bảng | Nội dung |
|---|---|
| **Số câu theo luật** | Khởi động riêng **6**/thí sinh · chung **12** · VCNV **4+1** · Tăng tốc **4** · Về đích **3**/gói · Câu hỏi phụ **3**. Biên *"lớn hơn max"* **không tồn tại** |
| **Phán quyết hai hay ba lựa chọn** | Chỉ nhị phân khi *Sai* trừ **0** điểm; có hình phạt ⇒ thêm **Huỷ kết quả** |
| **Bản gửi quá hạn** | Giữ, tô đỏ, không tự ghi đè, không tự loại; chỉ có bản quá hạn ⇒ thêm **Huỷ kết quả** |
| **Kênh trả lời quyết định lúc nào chấm được** | **Nói** ⇒ chấm bất cứ lúc nào · **Gõ** ⇒ nút chấm khoá tới hết giờ · **Thực hành** ⇒ nút chấm sống suốt |

### 13.3 Nhóm rule theo vòng

| Nhóm | Rule | Nội dung cốt lõi |
|---|---|---|
| **Khởi động** | `GR-001` → `GR-006` | Lượt riêng: 6 câu × 3 giây, đúng **+10**, sai **0**. Lượt chung: 12 câu, giành quyền bằng chuông, đúng **+10**, sai hoặc bấm rồi im lặng **−5**. Cửa sổ chuông là **một khoảng liên tục** từ mốc hiển thị câu qua thời gian MC đọc + 3 giây. Cửa sổ rỗng ⇒ **câu bị bỏ qua và vẫn tính là đã dùng**. Ghi nhận **bản cuối cùng** |
| **Vượt chướng ngại vật** | `GR-007` → `GR-012` | Mỗi thí sinh tối đa **1 lượt chọn** hàng ngang, bắt đầu từ vị trí 1, quay vòng khi đã có người bị loại. Hàng ngang **luôn gõ máy**, 15 giây, đúng **+10 cho mỗi người**; **≥1 người đúng ⇒ miếng ghép mở**. Băng điểm Chướng ngại vật **60/50/40/30**, sau gợi ý cuối **20** *(sàn)*, đếm theo số hàng ngang **đã hỏi**. Giải sai ⇒ **bị loại khỏi vòng, KHÔNG trừ điểm**. Ô trung tâm **luôn tới được** vì cả 4 hàng luôn được hỏi hết |
| **Tăng tốc** | `GR-013` → `GR-015` | 4 câu, 20/20/30/30 giây, **luôn gõ máy**. Thang **40/30/20/10** theo thứ hạng tốc độ, **chỉ tính trên tập người được chấm ĐÚNG**; người sai không giữ chỗ. Đồng thời gian tới **mili-giây** ⇒ cùng mức điểm, bậc kế nhảy qua. Ghi nhận **bản cuối**; bản trùng nội dung không cập nhật mốc |
| **Về đích** | `GR-016` → `GR-021` | Thứ tự lượt theo điểm cao nhất, hoà thì **số vị trí nhỏ nhất**, tính lại sau mỗi lượt. Gói **3 câu** từ hai mức **{20, 30}**. Cướp quyền: cửa sổ **5 giây**, cướp đúng ⇒ **chuyển điểm**, cướp sai ⇒ **−½ giá trị câu**; người cướp tính **bản ĐẦU TIÊN**. Ngôi sao hy vọng **1 lần/thí sinh/lần chạy vòng**: đúng ⇒ **×2**, sai ⇒ **−giá trị câu, đúng một lần**. Câu thực hành có **hai pha thời gian** |
| **Câu hỏi phụ** | `GR-022` → `GR-025` | Kích hoạt tại cú bấm **chốt trận** khi có hoà ở vị trí cần phân định. **3 câu × 15 giây**, cả hai con số **cố định, không cấu hình**. **Không cộng, không trừ điểm** — chỉ đổi thứ hạng. Chuông **không sống trước mốc start timer**. Hết 3 câu chưa phân định ⇒ **bốc thăm**, admin xác nhận |

### 13.4 Nhóm rule xuyên vòng

Mười hai rule không đến từ luật gốc — chúng tồn tại vì đây là **phần mềm** chứ không phải một buổi ghi hình:

| Rule | Nội dung |
|---|---|
| `GR-026` | **Phán quyết của admin** — máy không chấm; một câu đúng một phán quyết; phán quyết là điều kiện chuyển câu |
| `GR-027` | **Chuẩn hoá và tô nổi bật** — công cụ giúp admin nhìn ra chỗ khác, **không** phải cơ chế chấm |
| `GR-028` | **Điểm là hàm của nhật ký sự kiện** — chỉ thêm, tất định, phát lại được |
| `GR-029` | **Điều chỉnh điểm thủ công** — van thoát cho mọi sai sót, lý do bắt buộc, không tự hoàn nguyên |
| `GR-030` | **Bỏ vòng · chạy lại vòng · kết thúc sớm** — ba cửa ra chủ động, đều thuộc hạng phá huỷ |
| `GR-031` | **Rút đề và không lặp câu** — ranh giới *đã dùng* = **đã hiển thị**; kiểm kho tại cửa vào từng vòng |
| `GR-032` | **Hàng đợi tín hiệu** — FIFO thuần theo server timestamp, chỉ chặn ở VCNV, lịch sử không xoá |
| `GR-033` | **Mốc thời gian do admin bấm** — hai thao tác, thứ tự cố định, không gộp |
| `GR-034` | **Chuông chỉ nhận click chuột** — không phím tắt, tự khoá ngay |
| `GR-035` | **Server time là nguồn sự thật duy nhất** — mili-giây, biên đóng, đồng hồ đơn điệu |
| `GR-036` | **Mất kết nối và giữ ghế** — ngưỡng 120 giây, không chính sách tự động, v1 không có kick |
| `GR-037` | **Phạm vi hiển thị đáp án** — admin và MC luôn thấy; lớp phủ không bao giờ; ba loại thông tin ba chế độ |

### 13.5 Yêu cầu phủ định

`game-rules.md` dùng ba nhãn phủ định là **quyết định chuẩn tắc**, không phải lỗ hổng:

- **"Không tồn tại"** — nhánh không có đường vào *(vd: câu thứ 7 ở lượt riêng; kho đề cạn giữa vòng; bấm chuông trước khi câu được đưa ra ở Câu hỏi phụ)*.
- **"INVALID STATE"** — thao tác không hợp lệ ở trạng thái hiện tại *(vd: đi thẳng từ vòng này sang vòng khác; bỏ vòng lần thứ hai; điều chỉnh điểm sau khi trận đóng sổ)*.
- **"Nguồn không quy định ⇒ không được tự thêm"** — *(vd: không trừ điểm khi giải sai Chướng ngại vật; không phát minh cửa sổ trả lời riêng cho Câu hỏi phụ)*.

### 13.6 Biến thể bị loại — giữ để không cài nhầm

Wikipedia bị loại làm căn cứ luật; chủ dự án chọn Fandom làm nguồn duy nhất. Các giá trị bị loại **có chủ đích**, không phải bỏ sót: Khởi động một lượt 12 câu / 60 giây · mức điểm Về đích 20 và 40 · mở miếng ghép khi *"ít nhất 1 thí sinh"* đúng · người bấm sai bị loại khỏi câu ở Câu hỏi phụ · cướp quyền kiểu *cộng thêm* · hết câu tie-break do *admin tự quyết* · độ phân giải hai chữ số thập phân · các giá trị của hệ thống tiền lệ · **cờ tự chấm**. *(Nguồn: `traceability.md` §Biến thể bị loại.)*

---

## 14. Functional Scope

| # | Capability sản phẩm phải hỗ trợ | Epic | Product requirements |
|---|---|---|---|
| FS-01 | Xác thực, vai hệ thống và vai vận hành, ràng buộc loại trừ | EPIC-001 | PRD-REQ-002, 003, 006 |
| FS-02 | Một phiên giữ quyền điều khiển, chuyển giao được, có nhật ký | EPIC-001 | PRD-REQ-004 |
| FS-03 | Permission riêng cho từng thao tác phá huỷ | EPIC-001 | PRD-REQ-005 |
| FS-04 | Truy cập public bằng mã phòng 6 số, read-only thi hành ở server | EPIC-001, EPIC-009 | PRD-REQ-001, 073 |
| FS-05 | Soạn câu hỏi với metadata, media, ba kiểu nhập, và kênh thực hành | EPIC-002 | PRD-REQ-007, 008, 009, 010 |
| FS-06 | Vòng duyệt đề và tìm kiếm kho đề | EPIC-002 | PRD-REQ-011, 014 |
| FS-07 | Cờ hiển thị dẫn xuất và hàng rào đề đã từng public | EPIC-002 | PRD-REQ-012, 013 |
| FS-08 | Xuất và nhập gói contest; giữ đúng cờ theo câu và theo contest; định danh câu ổn định qua nhập/xuất | EPIC-003 | PRD-REQ-016, 017, 018 |
| **FS-08b** | **Đánh dấu "đã dùng" hàng loạt bằng tay, và bản kê câu đã dùng xuất/nhập được** | EPIC-003 | PRD-REQ-094, 095 |
| FS-09 | Cấu hình luật đầy đủ + preset O26 áp bằng một thao tác | EPIC-004 | PRD-REQ-020, 021, 022 |
| FS-10 | Mode trả lời cấp contest, chụp vào trận | EPIC-004 | PRD-REQ-023 |
| FS-11 | Khoá cứng của v1: bốn vòng, bốn hàng ngang, luật cho bốn thí sinh | EPIC-004 | PRD-REQ-024, 025, 026 |
| FS-12 | Vòng đời trận: LOBBY, đóng băng cấu hình, bốn cửa ra, chốt và huỷ trận | EPIC-005 | PRD-REQ-027…034 |
| FS-13 | Sửa danh sách câu tại cửa vào vòng | EPIC-005 | PRD-REQ-035 |
| FS-14 | Engine chấm điểm không tự phán quyết | EPIC-006 | PRD-REQ-036, 037, 038 |
| FS-15 | Nhật ký sự kiện và mô hình điểm hoàn nguyên được | EPIC-006 | PRD-REQ-039, 040, 041 |
| FS-16 | Đồng hồ server và mốc do admin bấm | EPIC-006 | PRD-REQ-042, 043 |
| FS-17 | Hàng đợi tín hiệu và quy tắc duyệt/từ chối | EPIC-006 | PRD-REQ-044, 045, 046 |
| FS-18 | Rút đề, không lặp câu, kiểm kho tại cửa vào từng vòng | EPIC-006 | PRD-REQ-047, 048 |
| FS-19 | Phạm vi hiển thị đáp án theo vai | EPIC-006, EPIC-009 | PRD-REQ-049 |
| FS-20 | Mất kết nối, giữ ghế, khôi phục giữa câu | EPIC-006, EPIC-008 | PRD-REQ-050 |
| FS-21 | Màn chấm với đối chiếu, lịch sử bài gửi, bản quá hạn | EPIC-007 | PRD-REQ-051, 052, 053, 054 |
| FS-22 | Điều chỉnh điểm thủ công có lý do | EPIC-007 | PRD-REQ-055 |
| FS-23 | Ba hạng cảnh báo giao diện | EPIC-007 | PRD-REQ-056, 063 |
| FS-24 | Mở/đóng hiển thị và điều khiển bàn cờ VCNV | EPIC-007 | PRD-REQ-057, 058 |
| FS-25 | Hiển thị hàng đợi và trạng thái câu cho admin | EPIC-007 | PRD-REQ-059, 060 |
| FS-26 | Vô hiệu hoá và kích hoạt lại ghế | EPIC-007 | PRD-REQ-061 |
| FS-27 | Chốt câu khi mới chấm một phần | EPIC-007 | PRD-REQ-062 |
| FS-28 | Thao tác của thí sinh: chuông, gửi đáp án, chọn hàng ngang, chọn gói | EPIC-008 | PRD-REQ-064…070 |
| FS-29 | Bảng điểm realtime trên máy thí sinh | EPIC-008 | PRD-REQ-067 |
| FS-30 | Quy tắc giao diện xuyên suốt: không chặn gửi lại, `Esc`, invalid state | EPIC-008 | PRD-REQ-071, 072 |
| FS-31 | Màn MC, lớp phủ công bố, banner tạm dừng | EPIC-009 | PRD-REQ-074, 075, 076 |
| FS-32 | Chính sách hiển thị cho khán giả khi admin can thiệp | EPIC-009 | PRD-REQ-077 |
| FS-33 | Chủ đề và các khe âm thanh | EPIC-009 | PRD-REQ-078 |
| FS-34 | Biên bản trận theo lần chạy, thống kê ghi ngược | EPIC-010 | PRD-REQ-079, 080, 081 |
| **FS-34b** | **Xuất toàn bộ kết quả và nhật ký sự kiện; xuất kết quả rút gọn — cả hai chỉ có chiều ra** | EPIC-010 | PRD-REQ-096, 097 |
| FS-35 | Nhật ký thao tác toàn hệ thống và biện pháp quyền riêng tư | EPIC-011 | PRD-REQ-082, 083, 084 |
| FS-36 | Hai hồ sơ triển khai, giới hạn media, rate-limit cổng khán giả, **bốn loại gói xuất có đủ ở cả hai hồ sơ** | EPIC-012 | PRD-REQ-019, 085, 086, 087, 098 |


---

## 15. Non-Functional Requirements

> Chỉ ghi yêu cầu **có nguồn**. Nơi nguồn chỉ nói định tính, hoặc con số chỉ tồn tại ngoài `docs/`, mục được đánh dấu `NEEDS CLARIFICATION`.

### 15.1 Công bằng và thời gian *(fairness, latency)*

| ID | Yêu cầu | Nguồn | Trạng thái |
|---|---|---|---|
| NFR-01 | Thứ tự tín hiệu và thứ hạng tốc độ MUST tính trên **đồng hồ đơn điệu của server**, độ phân giải **mili-giây**; đồng hồ tường chỉ dùng để hiển thị và ghi nhật ký | `GR-035`, `INV-004` | CONFIRMED |
| NFR-02 | Biên thời gian MUST là **biên đóng** — timestamp đúng bằng mốc là hợp lệ | `GR-035`, `INV-005`, `QĐ-029` | CONFIRMED |
| NFR-03 | Chỉnh giờ hệ thống giữa trận MUST NOT đảo được thứ hạng đã ghi; máy thí sinh không đồng bộ giờ MUST NOT ảnh hưởng kết quả | `GR-035` | CONFIRMED |
| NFR-04 | MUST NOT có cửa sổ ân hạn hay trừ bù độ trễ tay người cho mốc do admin bấm | `QĐ-006`, `GR-033` | CONFIRMED |
| NFR-05 | Giao diện MUST phản hồi thao tác **ngay lập tức, không có độ trễ cảm nhận được**; mọi luồng bất đồng bộ MUST đi theo `loading → success/error`. **Không có ngưỡng mili-giây chuẩn tắc** | `CLAUDE.md` §UX | CONFIRMED |
| NFR-06 | Ngưỡng độ trễ và số viewer đồng thời MUST dùng mặc định cấu hình được | `QĐ-067` | NEEDS CLARIFICATION *(`QUESTION-003`)* |

### 15.2 Đồng thời *(concurrency)*

| ID | Yêu cầu | Nguồn | Trạng thái |
|---|---|---|---|
| NFR-07 | MUST có đúng **một phiên** giữ quyền điều khiển cho mỗi contest tại một thời điểm | `QĐ-008` | CONFIRMED |
| NFR-08 | Một contest MUST chỉ có **một trận đang chạy** tại một thời điểm | `QĐ-039` | CONFIRMED |
| NFR-09 | Hàng đợi tín hiệu MUST xử lý **FIFO thuần theo server timestamp**, không ưu tiên theo loại, số ghế hay vị trí; hai tín hiệu cùng mốc do hàng đợi tự quyết, **tất định khi dựng lại** | `INV-007`, `QĐ-025` | CONFIRMED |
| NFR-10 | Mỗi thao tác MUST có đúng **một đường vào** cho mỗi mode trả lời | `QĐ-019` | CONFIRMED |
| NFR-11 | Nhiều trận song song trên một bản cài | `product-discovery.md` §7 AS-2 *(giả định chưa kiểm chứng, con số lấy từ bản nháp)* | NEEDS CLARIFICATION *(`QUESTION-007`)* |

### 15.3 Khả năng kiểm toán *(auditability)*

| ID | Yêu cầu | Nguồn | Trạng thái |
|---|---|---|---|
| NFR-12 | Nhật ký sự kiện trận MUST **linear, chỉ thêm, không bao giờ xoá**; hoàn nguyên bằng cách thêm sự kiện | `INV-001`, `QĐ-011` | CONFIRMED |
| NFR-13 | Lịch sử tín hiệu và lịch sử các bản đã gửi MUST NOT bị xoá | `GR-032`, `GR-006`, `GR-015` | CONFIRMED |
| NFR-14 | Hệ thống MUST ghi nhật ký **mọi thao tác của mọi vai**, gồm mỗi lần xem đáp án và mỗi lần chuyển quyền điều khiển | `CLAUDE.md` §Quy ước khác · `QĐ-074`, `QĐ-008` | CONFIRMED |
| NFR-15 | Mọi thao tác phá huỷ MUST kèm **lý do bắt buộc** | `QĐ-072`, `GR-029`, `GR-030` | CONFIRMED |
| NFR-16 | Biên bản trận MUST in theo **lần chạy** kèm nhãn, không gộp, không giấu lần hỏng | `QĐ-077` | CONFIRMED |
| NFR-17 | Sự kiện bị khiếu nại MUST có đủ thời điểm và chuỗi thao tác trong nhật ký | `product-discovery.md` §9 SM-8 | CONFIRMED |

### 15.4 Bảo mật *(security)*

| ID | Yêu cầu | Nguồn | Trạng thái |
|---|---|---|---|
| NFR-18 | Server MUST kiểm xác thực và quyền cho **mọi** yêu cầu, bất kể client là ai, đã vào phòng gì, hay giao diện có ẩn nút hay không | `CLAUDE.md` §Nguyên tắc code | CONFIRMED |
| NFR-19 | Mọi đầu vào MUST được kiểm lại ở server; kiểm ở giao diện chỉ là trải nghiệm | `CLAUDE.md` §Nguyên tắc code | CONFIRMED |
| NFR-20 | Kênh khán giả và lớp phủ MUST bị thi hành read-only ở server — mọi sự kiện ghi bị bỏ | `CLAUDE.md` §Nguyên tắc code, `TERM-007` | CONFIRMED |
| NFR-21 | **Trước mốc câu khép**, đáp án MUST NOT rời server tới bất kỳ vai nào ngoài admin và MC — kể cả lớp phủ dựng stream. Từ mốc đó, đáp án được đẩy tới thí sinh, khán giả và lớp phủ theo cờ reveal | `INV-017`, `GR-037`, `QĐ-080` | CONFIRMED |
| NFR-21b | Server MUST chỉ đẩy đáp án **tại đúng mốc**; MUST NOT đẩy sớm rồi dựa vào cờ hiển thị phía client để giấu | `QĐ-080` · `CLAUDE.md` §Nguyên tắc code | CONFIRMED |
| NFR-22 | Gói khôi phục sau mất kết nối MUST đi qua đúng bộ lọc theo vai — không chứa đáp án, không chứa bài làm của ghế khác | `QĐ-046`, `GR-036` | CONFIRMED |
| NFR-23 | Server MUST bỏ qua lệnh trùng của nút một chiều — **client không được tin** | `QĐ-060` | CONFIRMED |
| NFR-24 | Media của thí sinh MUST được nạp trước ở dạng mã hoá, khoá chỉ phát đúng lúc công bố theo server time, kèm phương án lui khi cơ chế không khả dụng | `CLAUDE.md` §Quy ước khác · `product-discovery.md` §7 AS-4 | CONFIRMED |

### 15.5 Khả năng phục hồi *(recoverability)*

| ID | Yêu cầu | Nguồn | Trạng thái |
|---|---|---|---|
| NFR-25 | Mỗi vòng MUST bắt đầu **sạch**: mọi cờ phạm vi vòng đặt lại; vòng chỉ đọc ba thứ — cấu hình đã đóng băng, danh sách câu đã gán, bảng điểm hiện tại. **Ngoại lệ duy nhất**: cờ hành chính vô hiệu hoá ghế | `QĐ-033`, `INV-022` | CONFIRMED |
| NFR-26 | Ghế mất kết nối MUST được giữ **120 giây** *(cấu hình được, là khuyến nghị)*; quá ngưỡng chỉ tô nổi bật | `GR-036`, `QĐ-045` | CONFIRMED |
| NFR-27 | Admin mất kết nối MUST khôi phục từ trạng thái server bằng cùng cơ chế của client thường; MUST NOT đóng băng đồng hồ, MUST NOT tự tạm dừng trận, MUST NOT có vai dự phòng tự động | `QĐ-070` | CONFIRMED |
| NFR-28 | Phép tính điểm MUST tất định để phát lại và soi lại ở bất kỳ mốc nào | `GR-028` | CONFIRMED |
| NFR-29 | Bàn cờ VCNV MUST dựng lại được sau sự cố qua đường đặt trạng thái ô của admin | `QĐ-052` | CONFIRMED |

### 15.6 Tính sẵn sàng *(availability)* — ranh giới đã chấp nhận

| ID | Yêu cầu | Nguồn | Trạng thái |
|---|---|---|---|
| NFR-30 | Hồ sơ portable MUST hoạt động đầy đủ **không cần Internet** | `GOAL-010` | CONFIRMED |
| NFR-31 | **Server sập là mất khả năng cứu** — đây là ranh giới **đã chấp nhận**, không phải chỗ thiếu đặc tả | `QĐ-070` | CONFIRMED |
| NFR-32 | v1 MUST NOT có luồng nào chạy mà không có admin | `QĐ-009` | CONFIRMED |

### 15.7 Lưu trữ và vòng đời dữ liệu *(retention)*

| ID | Yêu cầu | Nguồn | Trạng thái |
|---|---|---|---|
| NFR-33 | Hạn lưu trữ MUST khác nhau theo **mục đích trận** | `QĐ-040` | CONFIRMED |
| NFR-34 | Giá trị hạn lưu trữ cụ thể *(luyện tập 3 tháng · official 12 tháng)* | `CLAUDE.md` §Lộ trình version | NEEDS CLARIFICATION — *hai con số chỉ có ở `CLAUDE.md`; `docs/` không chốt số nào* (`QUESTION-006`) |
| NFR-35 | Job dọn dữ liệu MUST cảnh báo trước và MUST NOT đụng biên bản đã xuất | `QĐ-077` | CONFIRMED |

### 15.8 Trải nghiệm và bố cục *(usability)*

| ID | Yêu cầu | Nguồn | Trạng thái |
|---|---|---|---|
| NFR-36 | Mọi thao tác bất đồng bộ MUST hiển thị trạng thái đang xử lý rõ ràng | `CLAUDE.md` §UX | CONFIRMED |
| NFR-37 | Nội dung chính của mỗi màn MUST nằm gọn trong **một** khung nhìn tham chiếu, không nhồi nhét, không ép cuộn để thấy phần quan trọng. **Kích thước khung nhìn tham chiếu là quyết định thiết kế, không phải yêu cầu chuẩn tắc** | `CLAUDE.md` §UX | CONFIRMED |
| NFR-38 | Giao diện MUST chỉ dùng tiếng Việt; múi giờ hiển thị MUST thống nhất UTC+7 | `CLAUDE.md` §Quy ước khác | CONFIRMED |
| NFR-39 | Mỗi màn MUST có nút quay lại rõ ràng — **trừ màn thi đấu của thí sinh**, nơi `Esc` MUST NOT quay lại | `CLAUDE.md` §Điều hướng · `QĐ-072` | CONFIRMED |

---

## 16. Dependencies

### 16.1 Phụ thuộc nội bộ

| Phụ thuộc | Nội dung |
|---|---|
| **Thứ bậc nguồn** | `docs/source/` → `docs/PRD.md` → `specs/<feature>/spec.md` → `plan.md` → `tasks.md`. Tầng dưới truy nguyên về tầng trên |
| **Sổ quyết định** | Mọi yêu cầu liên quan lựa chọn sản phẩm phụ thuộc `decisions.md`; sửa một `QĐ-*` có thể làm lệch yêu cầu tương ứng |
| **Đặc tả luật và máy trạng thái** | `game-rules.md` đúng về **luật**; `game-state-machine.md` đúng về **vận hành**. Hai bên phải khớp |
| **Thuật ngữ** | `glossary.md` là nguồn tên gọi; đặc biệt các từ nhiều nghĩa: *lượt* (4 nghĩa) · *kết quả* (3 nghĩa) · *draw* (3 nghĩa) · *state* (7 thang bậc) |
| **Ràng buộc kỹ thuật toàn repo** | `CLAUDE.md` — trải nghiệm, bảo mật zero-trust, server-authoritative, không lặp logic, ngôn ngữ, múi giờ, phông chữ, quy ước commit |
| **Cổng chất lượng** | `.specify/memory/constitution.md` — sáu cổng phải qua trước khi một spec chuyển sang bước lập kế hoạch |
| **Thứ tự epic** | EPIC-002 phụ thuộc EPIC-001 · EPIC-003, EPIC-004 phụ thuộc EPIC-002 · EPIC-005 phụ thuộc EPIC-004 · EPIC-006…EPIC-010 phụ thuộc EPIC-005 |

### 16.2 Hệ thống ngoài

| Hệ thống | Vai trò | Ghi chú |
|---|---|---|
| **Phần mềm dựng livestream** | Tiêu thụ lớp phủ 1920×1080 nền trong suốt | Sản phẩm **không** truyền video (NON-GOAL-003) |
| **Trình duyệt của mọi vai** | Nền tảng chạy duy nhất | Cơ chế nạp trước media mã hoá phụ thuộc năng lực trình duyệt; phải có phương án lui |
| **Thiết bị nhập liệu của từng ghế** | VCNV và Tăng tốc **luôn gõ máy** ⇒ mỗi ghế cần thiết bị nhập đầy đủ cho **cả trận**, kể cả ở mode sân khấu | Chưa nguồn nào nêu yêu cầu phần cứng tối thiểu — `ASSUMPTION-008` |

### 16.3 Dữ liệu ngoài

| Nguồn | Vai trò | Rủi ro |
|---|---|---|
| **Luật chơi/Olympia 26 trên Fandom** | Source of truth về luật | Wiki cộng đồng, sửa được bất kỳ lúc nào, **không có phiên bản**. Đối phó: dùng **bản lưu trong `docs/source/`**, không dùng URL sống — `ASSUMPTION-007`, `RISK-006` |
| **Media do người dùng cung cấp** | Ảnh, video, âm thanh của câu hỏi và các khe âm thanh | Hệ thống không kiểm được nguồn gốc ⇒ disclaimer khi tải lên (PRD-REQ-084) |

### 16.4 Phụ thuộc vận hành

| Phụ thuộc | Nội dung |
|---|---|
| **Có người vận hành trong mọi trận** | v1 không có luồng nào chạy không admin (`QĐ-009`). Số người vận hành là **khuyến nghị quy trình**, không phải ràng buộc hệ thống, vì quyền gắn với phiên (`QĐ-008`) |
| **Admin chọn tay toàn bộ danh sách đề trước mỗi trận** | Hệ thống cố ý không tự lấy đề — `ASSUMPTION-003` |
| **Đơn vị tổ chức tự chuẩn bị âm thanh** | Khe trống là im lặng, không có bộ mặc định — `ASSUMPTION-006`, `RISK-005` |
| **Đơn vị tự host chịu trách nhiệm pháp lý về dữ liệu** | `product-discovery.md` §2 |

### 16.5 Phụ thuộc pháp lý

| Nội dung | Trạng thái |
|---|---|
| **Bản quyền format Olympia** | **Đã được chấp thuận.** Sản phẩm dùng tên và cấu trúc vòng bình thường. Thiết kế trung lập vẫn giữ vì nó phục vụ **tính tuỳ biến**, không phải để né bản quyền |
| **Giấy phép mã nguồn** | **Không ghi ở giai đoạn này** — đây là sản phẩm nội bộ. Xét lại nếu về sau mở mã hoặc bán |
| **Bản quyền nhạc nền do người dùng tải lên** | Giữ **disclaimer khi tải lên**, và nâng nó thành yêu cầu chính thức (PRD-REQ-084) |

*Nguồn §16.5: `product-discovery.md` §3 (pháp lý).*

---

## 17. Assumptions

> Chỉ ghi giả định **có căn cứ**, và chỉ những giả định **còn hiệu lực**. Cột Nguồn dẫn về mã `AS-*` của `product-discovery.md` §7 để tra chéo.

| ID | Giả định | Căn cứ / vì sao đáng ngờ | Nguồn |
|---|---|---|---|
| **ASSUMPTION-001** | **Có nhu cầu thị trường thực** cho sản phẩm này | Không có nghiên cứu người dùng, không có người dùng đầu tiên đã cam kết. Toàn bộ phát biểu vấn đề rút từ suy luận và đối chiếu hệ thống tiền lệ | `product-discovery.md` §1, §7 AS-1 |
| **ASSUMPTION-002** | **Nhiều trận song song là nhu cầu thật** | Con số được chốt mà không dẫn nguồn nhu cầu. Với mô hình một bản cài cho một đơn vị, nhiều trận cùng lúc là bất thường | §7 AS-2 |
| **ASSUMPTION-003** | **Admin chấp nhận bỏ thời gian chọn tay toàn bộ đề trước mỗi trận** | Hệ thống **cố ý** không tự lấy đề. Khối lượng là **69 câu + 1 bộ Chướng ngại vật mỗi trận** (§19.3); chưa ai kiểm chứng admin chấp nhận bỏ **bao nhiêu thời gian** cho từng ấy câu | §7 AS-3 · §19.3 |
| **ASSUMPTION-004** | **Cơ chế nạp trước media mã hoá chạy ổn định trên trình duyệt** | Chính nguồn xếp nó là hạng mục phức tạp nhất phía client, và đã phải chuẩn bị phương án lui | §7 AS-4 |
| **ASSUMPTION-005** | **Mã 6 số công khai là chấp nhận được về quyền riêng tư** | Ai có mã đều thấy tên và trường lớp của học sinh vị thành niên. Chỉ có tuỳ chọn dùng biệt danh, **không bật mặc định** | §7 AS-5 · `PRD-REQ-083` |
| **ASSUMPTION-006** | **Người tổ chức có sẵn nhạc và hiệu ứng âm thanh** | Khe trống là im lặng, không có bộ mặc định ⇒ sản phẩm ra mắt sẽ **hoàn toàn im lặng** nếu admin không chuẩn bị | §7 AS-6 · `PRD-REQ-078` |
| **ASSUMPTION-007** | **Wiki nguồn chính xác và ổn định** | Wiki cộng đồng, sửa được bất kỳ lúc nào, không có phiên bản. Đối phó: dùng **bản lưu trong `docs/source/`**, không dùng URL sống | §7 AS-7 |
| **ASSUMPTION-008** | **Thí sinh có thiết bị riêng để gõ** | VCNV và Tăng tốc **luôn** gõ máy ⇒ mỗi ghế cần một thiết bị nhập đầy đủ cho cả trận, kể cả ở mode sân khấu. Chưa nguồn nào nêu yêu cầu phần cứng tối thiểu | §7 AS-8 · `QĐ-018`, `TERM-016` |

---

## 18. Risks

> **Mitigation direction là HƯỚNG xử lý, không phải requirement mới.** Không mục nào dưới đây sinh ra một `PRD-REQ-*`.

| ID | Mô tả | Impact | Likelihood | Related requirements | Mitigation direction | Source |
|---|---|---|---|---|---|---|
| **RISK-001** | **Không có người dùng đầu tiên đã cam kết** ⇒ toàn bộ sản phẩm có thể giải một vấn đề không ai trả tiền để giải | Cao — ảnh hưởng chính đáng của cả dự án | *(không đủ căn cứ)* | Toàn bộ | Chạy gate người thật *(một giáo viên và hai học sinh thử 30 phút)* trước khi mở rộng phạm vi | `product-discovery.md` §1, §7 AS-1, §10 |
| **RISK-002** | **Số câu phải chọn tay quá lớn** ⇒ dựng contest thành việc nặng, admin bỏ cuộc. **Nay đã lượng hoá: 69 câu + 1 bộ Chướng ngại vật cho MỘT trận**, trong đó 12 câu chắc chắn không dùng. Contest 3 trận: **207 câu** | Cao — chặn hành trình dựng contest | Cao *(con số là tất định, không phải ước lượng)* | PRD-REQ-014, PRD-REQ-047, PRD-REQ-090 | Tìm kiếm, lọc và sắp xếp trên kho đề **là đường tới hạn** của hành trình dựng contest, không phải tiện ích. Cân nhắc thao tác chọn hàng loạt theo bộ lọc | §7 AS-3 · §19.3 · `GR-017`, `QĐ-081` |
| **RISK-003** | **Mỗi ghế cần một thiết bị nhập đầy đủ cho cả trận** ⇒ đơn vị tổ chức thiếu phần cứng và phát hiện muộn | Cao — chặn ngày thi | *(không đủ căn cứ)* | PRD-REQ-066, PRD-REQ-069 | Nêu yêu cầu phần cứng tối thiểu trong tài liệu vận hành; xác nhận ở gate người thật | §7 AS-8 |
| **RISK-004** | **Cơ chế nạp trước media mã hoá không chạy ổn định** ⇒ hoặc lộ đề sớm, hoặc media không kịp | Cao — chạm trực tiếp `GOAL-005` | *(nguồn xếp là hạng mục phức tạp nhất phía client)* | PRD-REQ-049, NFR-24 | Giữ phương án lui *(chỉ công bố, không nạp trước)*; đo mốc sớm nhất thí sinh có thể thấy nội dung đề | §7 AS-4, §9 SM-10 |
| **RISK-005** | **Sản phẩm ra mắt hoàn toàn im lặng** vì không có bộ âm thanh mặc định và admin chưa chuẩn bị | Trung bình — không chặn trận, nhưng demo rất tệ | Cao *(khe trống là im lặng theo thiết kế)* | PRD-REQ-078 | Hướng dẫn chuẩn bị âm thanh trong tài liệu dựng contest; cân nhắc gói mẫu tách rời sản phẩm | §7 AS-6, §8 · `QĐ-079` |
| **RISK-006** | **Wiki nguồn thay đổi** mà không ai phát hiện ⇒ luật trong sản phẩm lệch với luật người dùng tra được | Trung bình | *(wiki cộng đồng, không có phiên bản)* | PRD-REQ-021 | Đã có bản lưu bất biến trong `docs/source/`; cân nhắc cơ chế phát hiện thay đổi, hoặc chấp nhận bản lưu là chốt chặn cuối | §7 AS-7, §10 |
| **RISK-007** | **Dữ liệu học sinh vị thành niên lộ qua mã phòng công khai** | Cao — rủi ro pháp lý cho đơn vị tự host | Trung bình | PRD-REQ-083, PRD-REQ-086 | Tuỳ chọn biệt danh; rate-limit và khoá cổng phòng; nhắc trong tài liệu vận hành | §7 AS-5 |
| **RISK-008** | **Phạm vi v1 quá rộng** — v1 hiện không phải MVP mà là một sản phẩm hoàn chỉnh, vẫn ôm cả bốn biến thể Khởi động, hai format Tăng tốc, nạp trước mã hoá, hai hồ sơ triển khai, chủ đề và bộ âm thanh đầy đủ | Cao — kéo dài thời gian tới trận thật đầu tiên | Cao | Toàn bộ epic v1 | Nếu cắt: **cắt biến thể, không cắt giai đoạn**; giữ một đường đi hoàn chỉnh từ soạn đề tới xuất kết quả. Ba khoá cứng của v1 đã cắt sẵn một phần. **Nếu chỉ giữ được một thứ ngoài màn khán giả, giữ màn MC** | §8 |
| **RISK-009** | **Server sập giữa trận** ⇒ mất khả năng cứu | Cao | *(không đủ căn cứ)* | NFR-31 | **Ranh giới đã chấp nhận có chủ đích**, không phải chỗ thiếu đặc tả. Hướng giảm thiểu thuộc vận hành, không thuộc sản phẩm | `QĐ-070` |

---

## 19. Success Metrics

> Nguồn phân biệt rõ hai nhóm: nhóm **đã có trong nguồn** là **tiêu chí kỹ thuật**, và nhóm **đề xuất** chưa được chủ dự án chốt. `product-discovery.md` §9 tự nhận xét: *"Không tiêu chí nào đo giá trị với người dùng."* PRD **không tự tạo số liệu mục tiêu**.

### 19.1 Đã có trong nguồn — tiêu chí kỹ thuật

| ID | Đo gì | Ngưỡng | Goal | Nguồn | Trạng thái |
|---|---|---|---|---|---|
| **METRIC-001** | Một trận thử với người thật, trọn bốn vòng kèm livestream, **không sự cố chặn trận** | 0 sự cố chặn | GOAL-007, GOAL-008 | `product-discovery.md` §9 | CONFIRMED |
| **METRIC-002** | Điểm cuối trận khớp với tính tay | **100%** ở ba kịch bản tự động | GOAL-006 | §9 | CONFIRMED |
| **METRIC-003** | Admin đổi luật qua giao diện và trận chạy theo giá trị mới | Đạt / không đạt | GOAL-002 | §9 | CONFIRMED |
| **METRIC-004** | Bộ kiểm *"không rò đáp án"* | Pass | GOAL-005 | §9 | CONFIRMED |

### 19.2 Đề xuất — chưa được chốt

> `product-discovery.md` §9 ghi rõ đây là **đề xuất bổ sung**, không phải requirement. Toàn bộ nhóm này mang `NEEDS CLARIFICATION` (`QUESTION-008`).

| ID | Đo gì | Ngưỡng gợi ý | Goal | Nguồn |
|---|---|---|---|---|
| METRIC-005 | Thời gian dựng một contest hoàn chỉnh khi kho đề đã có | ≤ 30 phút | GOAL-002 | §9 SM-1 |
| METRIC-006 | Tỉ lệ admin dựng được contest lần đầu không cần hỏi ai | ≥ 4/5 người | GOAL-002 | §9 SM-2 |
| METRIC-007 | Tỉ lệ tái sử dụng câu hỏi sau 5 trận | ≥ 30% — chứng minh `PS-3` đã giải | GOAL-005 | §9 SM-4 |
| METRIC-008 | Số lần admin phải **ép qua cảnh báo** trong một trận | *Đếm riêng, không gộp với điều chỉnh điểm* | GOAL-007 | §9 SM-5 |
| METRIC-009 | Số lần trận gián đoạn vì **lỗi hệ thống** | 0 | GOAL-007 | §9 SM-6 |
| METRIC-010 | Tỉ lệ thí sinh nối lại thành công trong ngưỡng chờ | ≥ 95% | GOAL-007 | §9 SM-7 |
| METRIC-011 | Sự kiện bị khiếu nại có đủ thời điểm và chuỗi thao tác trong nhật ký | 100% | GOAL-007 | §9 SM-8 |
| METRIC-012 | Số đáp án rò ra kênh không có quyền | 0 | GOAL-005 | §9 SM-9 |
| METRIC-013 | Thời điểm sớm nhất thí sinh có thể thấy nội dung đề trước lúc công bố | *Không bao giờ* — kiểm chứng `ASSUMPTION-004` | GOAL-005 | §9 SM-10 |
| METRIC-014 | Số trận thật trong ba tháng đầu | ≥ 3 — kiểm chứng `ASSUMPTION-001` | GOAL-001 | §9 SM-11 |
| METRIC-015 | Nhân sự tối thiểu vận hành được một trận | *Ghi nhận thực tế* | GOAL-007 | §9 SM-12 |

**Vì sao đếm *"ép qua cảnh báo"* chứ không đếm *"can thiệp"*.** Can thiệp **chính là mô hình vận hành** (`QĐ-002`) — điều chỉnh điểm và chọn lượt là việc bình thường, đếm nó không nói lên điều gì. Thứ đáng đếm là **số lần ép qua một cảnh báo**: đó mới là lúc hệ thống và người vận hành bất đồng. *(Nguồn: `product-discovery.md` §9.)*

**Không đặt metric** về mức độ chấp nhận hay tăng trưởng khi chưa có người dùng đầu tiên đã cam kết. *(Nguồn: §9.)*

### 19.3 Giá trị SUY RA từ luật — không phải mục chờ đo

> `product-discovery.md` §9 SM-3 xếp *"số câu phải chọn tay cho một trận chuẩn"* vào nhóm **đo trước, đặt ngưỡng sau**. **Phân loại đó sai**: con số này **tất định**, suy ra được từ số câu của từng vòng cộng với quy tắc pre-flight, không cần một trận thật nào để biết.

**Sàn kho đề của một trận chuẩn** *(4 thí sinh, 4 vòng, luật O26)*:

| Kho | Tối thiểu | Thực hỏi | Dư | Căn cứ |
|---|---|---|---|---|
| Khởi động | **36** | 36 | 0 | 6 câu × 4 ghế *(lượt riêng)* + 12 *(lượt chung)* |
| VCNV | **5 câu + 1 bộ Chướng ngại vật** | 5 | 0 | 4 hàng ngang + 1 câu ô trung tâm |
| Tăng tốc | **4** | 4 | 0 | `GR-013` |
| Về đích | **24** | 12 | **12** | Pre-flight trường hợp xấu nhất: 12 mức 20 **và** 12 mức 30 (`GR-017`) |
| Câu hỏi phụ | **0** | 3 *(nếu hoà)* | — | Không có kho riêng; dùng 12 câu dư của Về đích (`QĐ-081`) |
| **TỔNG** | **69 câu + 1 bộ CNV** | **57**, hoặc 60 nếu hoà | **12** | |

**Số trận N mà một kho đã nạp hỗ trợ được** — chặn bởi **vòng khan nhất**, không phải tổng kho:

```
N = min( ⌊K_khởi_động / 36⌋,  số_bộ_VCNV,  ⌊K_tăng_tốc / 4⌋,
         ⌊K_về_đích@20 / 12⌋,  ⌊K_về_đích@30 / 12⌋ )
```

Cho **N trận**: `69N` câu + `N` bộ Chướng ngại vật. Không lặp câu là phạm vi **toàn contest** và cờ đã-dùng **không bao giờ đặt lại**, nên con số nhân thẳng theo N. Mỗi lần **chạy lại một vòng** cộng thêm nhu cầu của đúng vòng đó.

| ID | Đo gì | Ngưỡng | Trạng thái |
|---|---|---|---|
| **METRIC-016** | Số câu phải chọn tay cho một trận chuẩn | **69** *(giá trị suy ra, không phải mục tiêu)* | CONFIRMED |

Thời gian admin bỏ ra để chọn từng ấy câu **là** `METRIC-005`, không phải một chỉ số riêng.

---

## 20. MVP Scope

> **MVP = v1.** Chủ dự án đã chốt v1 gồm mười giai đoạn và **đã từ chối cắt phạm vi một lần**. Đề xuất cắt trong `product-discovery.md` §8 là **ý kiến, không phải requirement** — nó được ghi ở đây dưới dạng hướng xử lý rủi ro (`RISK-008`), không phải phạm vi.

### 20.1 Epic thuộc MVP

| Epic | Tên | Ghi chú phạm vi |
|---|---|---|
| EPIC-001 | Xác thực và phân quyền | Đầy đủ |
| EPIC-002 | Kho đề và bộ đề | Trừ đánh phiên bản khi sửa câu đã duyệt |
| EPIC-003 | Nhập/xuất và gói contest | Trừ danh sách thí sinh *(chưa có yêu cầu nguồn)* |
| EPIC-004 | Contest builder và luật tuỳ biến | Có ba khoá cứng của v1 |
| EPIC-005 | Phòng thi và vòng đời trận | Đầy đủ |
| EPIC-006 | Game engine và luật thi đấu | Luật cho **đúng 4 thí sinh** |
| EPIC-007 | Điều khiển và can thiệp của admin | Đầy đủ |
| EPIC-008 | Trải nghiệm thí sinh | Đầy đủ |
| EPIC-009 | Trình diễn | Đầy đủ |
| EPIC-010 | Sau trận | **Trừ phát lại** |
| EPIC-011 | Dữ liệu cá nhân và quyền riêng tư | **Trừ job dọn dữ liệu tự động** |
| EPIC-012 | Vận hành và hai hồ sơ triển khai | Đầy đủ |

### 20.2 Requirement thuộc MVP

Tổng **98 yêu cầu**, phân bố theo độ ưu tiên:

| Priority | Nghĩa | Số lượng | Danh sách `PRD-REQ-*` |
|---|---|---|---|
| **P1** | Không có thì không chạy được một trận | **62** | 001, 002, 003, 004, 007, 008, 009, 013, 014, 016, 017, 020, 021, 023, 024, 025, 026, 027, 028, 029, 030, 031, 032, 033, 035, 036, 037, 038, 039, 040, 041, 042, 043, 044, 045, 046, 047, 048, 049, 050, 051, 052, 054, 055, 056, 057, 058, 059, 062, 064, 065, 066, 067, 068, 069, 070, 073, 074, 082, 085, **088**, **089** |
| **P2** | Cần cho v1 nhưng trận vẫn chạy được nếu thiếu | **27** | 005, 006, 010, 011, 012, 015, 019, 034, 053, 060, 061, 063, 071, 072, 075, 076, 077, 079, 083, 086, 087, **090**, **093**, **094**, **095**, **096**, **098** |
| **P3** | Phần cắt được của v1 | **8** | 018, 022, 078, 080, 081, 084, **091**, **097** |

*(P1 nay **63** — thêm **092**.)*

Trong 98 yêu cầu, **96 ở trạng thái `CONFIRMED`** và **2 mang `NEEDS CLARIFICATION`** — PRD-REQ-083, 087. Không yêu cầu nào ở trạng thái `CONFLICT`.

### 20.3 Lý do ưu tiên

1. **Một đường đi hoàn chỉnh trước, biến thể sau.** Nguyên tắc nếu phải cắt tiếp: *"một đường đi hoàn chỉnh từ soạn đề tới xuất kết quả; **cắt biến thể, không cắt giai đoạn**"* (`product-discovery.md` §8).
2. **Ba khoá cứng của v1 đã cắt sẵn một phần công việc** — bốn thí sinh, bốn hàng ngang, bốn vòng (`QĐ-007`, `QĐ-068`, `QĐ-069`).
3. **Không cắt màn MC.** MC là người đọc câu hỏi; không có màn riêng thì MC phải nhìn màn admin và hỏng luồng thi đấu. Nếu chỉ giữ được một thứ ngoài màn khán giả, **giữ màn MC** (§8).
4. **Cắt âm thanh hợp lệ về thiết kế nhưng sản phẩm sẽ demo rất tệ** (§8, `RISK-005`).

### 20.4 Dependencies của MVP

Chuỗi chặn: EPIC-001 → EPIC-002 → EPIC-004 → EPIC-005 → EPIC-006 → {EPIC-007, EPIC-008, EPIC-009, EPIC-010}. EPIC-003 và EPIC-012 nhánh song song sau EPIC-002. EPIC-011 chạy song song từ EPIC-001.

**Chặn ngoài phạm vi kỹ thuật**: không còn.

---

## 21. Open Questions

> Tổng hợp mọi `CONFLICT`, `NEEDS CLARIFICATION`, `MISSING`, `BOUNDARY_UNDEFINED`, `ORDER_DEPENDENT`, `INVALID_TRANSITION_UNDEFINED` phát hiện được khi đọc toàn bộ nguồn.
>
>
> **Câu hỏi được chốt thì bị XOÁ khỏi đây**, và danh sách được **đánh số lại** — số hiệu là **chỉ mục của bản hiện hành**, không phải định danh vĩnh viễn. Trích dẫn `QUESTION-*` từ tài liệu khác phải kèm tên câu hỏi.

**QUESTION-001 — Danh sách thí sinh trong gói contest**
- **Context**: **MISSING**. `product-discovery.md` §5 ghi đây là **epic chưa có yêu cầu nguồn**; các chi tiết *(mã hoá, hợp nhất tài khoản trùng, phiếu tài khoản)* chỉ tồn tại trong bản nháp `plans/**` ⇒ không phải requirement.
- **Affected requirements**: PRD-REQ-016 · **Affected rules**: — · **Required decision owner**: Chủ dự án *(chốt hoặc đóng hẳn)*

**QUESTION-002 — Trải nghiệm cài đặt lần đầu**
- **Context**: **MISSING**. Mới có giải pháp kỹ thuật tạo tài khoản admin bằng dòng lệnh, **chưa có trải nghiệm người dùng**.
- **Affected requirements**: PRD-REQ-085 · **Affected rules**: — · **Required decision owner**: Chủ dự án

**QUESTION-003 — Quy mô viewer và ngưỡng độ trễ**
- **Context**: Chủ dự án đã quyết là **chưa cần trả lời ở giai đoạn này** (`QĐ-067`) — **hoãn có chủ đích, không phải mục treo**. Bốn tài liệu cũ đưa bốn con số khác nhau, và khác biệt đó chỉ phản ánh việc chưa ai đo. Khi cần trả lời phải chốt hai điều: **(a)** số viewer đồng thời tối đa ở hồ sơ portable LAN, **(b)** hồ sơ container có cần con số cao hơn không và cao bao nhiêu.
- **Affected requirements**: PRD-REQ-086, PRD-REQ-087, NFR-06 · **Affected rules**: *không rule nào đọc con số này* · **Required decision owner**: Chủ dự án

**QUESTION-004 — Tuỳ chọn biệt danh có bật mặc định cho thí sinh vị thành niên không?**
- **Context**: `product-discovery.md` §7 AS-5 ghi *"Chỉ có tuỳ chọn dùng biệt danh, **không bật mặc định**"* và xếp giả định *"mã 6 số công khai là chấp nhận được về quyền riêng tư"* vào nhóm **chưa được kiểm chứng**. Không nguồn nào nói mặc định này đã được cân nhắc hay chỉ là giá trị rơi vào.
- **Affected requirements**: PRD-REQ-083 · **Affected rules**: — · **Required decision owner**: Chủ dự án

**QUESTION-005 — v1 có khoá cứng phạm vi phân định hoà không?**
- **Context**: v1 đã khoá cứng **hai** giá trị bằng cùng một khuôn — số hàng ngang bằng 4 (`QĐ-068`) và playlist bốn vòng (`QĐ-069`) — nhưng **không** khoá `tieBreakPositions`, vốn mặc định `[1]` *(chỉ phân định vị trí nhất)* và `traceability.md` xếp vào nhóm *"cấu hình được, không bật mặc định"*. Luật gốc chỉ mô tả chọn ra **một** người thắng.
- **Câu hỏi**: v1 có khoá cứng `tieBreakPositions = [1]` như đã khoá hai giá trị kia không, hay để mở?
- **Affected requirements**: PRD-REQ-024, PRD-REQ-025, PRD-REQ-032 · **Affected rules**: `GR-022`, `GR-025` · **Required decision owner**: Chủ dự án
- *Xuất xứ phát hiện*: `docs/reviews/game-rules-review.md` GRR-158 — mục **duy nhất** trong đợt rà soát đề nghị **thu hẹp** phạm vi thay vì thêm câu hỏi *(kho lưu, không phải requirement)*

**QUESTION-006 — Giá trị hạn lưu trữ**
- **Context**: `QĐ-040` chốt rằng hạn lưu trữ **khác nhau theo mục đích trận**, nhưng không cho con số. Hai giá trị *(luyện tập 3 tháng · official 12 tháng)* chỉ xuất hiện ở `CLAUDE.md` §Lộ trình version.
- **Affected requirements**: NFR-33, NFR-34, PRD-REQ-080 · **Affected rules**: — · **Required decision owner**: Chủ dự án

**QUESTION-007 — Số trận song song**
- **Context**: `product-discovery.md` §7 AS-2 xếp *"nhiều trận song song là nhu cầu thật"* vào nhóm giả định **chưa được kiểm chứng**, và ghi rõ *"con số được chốt mà không dẫn nguồn nhu cầu"*. Con số cụ thể chỉ tồn tại trong bản nháp `plans/**` ⇒ không phải requirement.
- **Affected requirements**: NFR-11, PRD-REQ-027 · **Affected rules**: — · **Required decision owner**: Chủ dự án

**QUESTION-008 — Bộ chỉ số thành công đề xuất chưa được chốt**
- **Context**: `product-discovery.md` §9 ghi rõ nhóm `METRIC-005` → `METRIC-015` là **đề xuất bổ sung**, không phải requirement; và nhận xét rằng nhóm đã có trong nguồn *"không tiêu chí nào đo giá trị với người dùng"*.
- **Affected requirements**: METRIC-005 → METRIC-015 · **Affected rules**: — · **Required decision owner**: Chủ dự án

**Không mục nào trong `game-rules.md`, `game-state-machine.md`, `decisions.md`, `glossary.md` hay `traceability.md` còn mang marker treo.** Ba tài liệu đầu tự khai điều này; lần quét toàn văn xác nhận. Marker duy nhất được định nghĩa và dùng trong bộ đặc tả là `[SUY RA]` — nhãn nguồn, không phải câu hỏi mở.

---

## 22. Traceability Matrix

> Viết tắt cột **Actor**: `AD` admin · `ST` setter · `TS` thí sinh · `MC` MC · `KG` khán giả · `OV` lớp phủ · `TR` trainer · `SV` server.
> Viết tắt cột **Journey**: `J1`…`J7` ứng với `JOURNEY-001`…`JOURNEY-007`.
> Cột **Source** ghi nguồn chính; nguồn đầy đủ ở mục yêu cầu tương ứng trong §12.

| PRD requirement | Goal | Epic | Journey | Actor | Game rules | Source | Status |
|---|---|---|---|---|---|---|---|
| PRD-REQ-001 | GOAL-008 | EPIC-001 | J4 | KG, OV | `GR-037` | `CLAUDE.md` §Mô hình truy cập · `TERM-007` | CONFIRMED |
| PRD-REQ-002 | GOAL-005 | EPIC-001 | J4 | AD, ST, TS, MC | `GR-037` | `CLAUDE.md` §Nguyên tắc code | CONFIRMED |
| PRD-REQ-003 | GOAL-005 | EPIC-001 | J2 | AD, ST, TS, MC | `GR-037` | `QĐ-065` | CONFIRMED |
| PRD-REQ-004 | GOAL-007 | EPIC-001 | J6 | AD | `GR-026` | `QĐ-008`, `QĐ-070` | CONFIRMED |
| PRD-REQ-005 | GOAL-007 | EPIC-001 | J6 | AD | `GR-029`, `GR-030` | `QĐ-078` | CONFIRMED |
| PRD-REQ-006 | GOAL-007 | EPIC-001 | J2 | AD | — | `QĐ-065` | CONFIRMED |
| PRD-REQ-007 | GOAL-005 | EPIC-002 | J1 | ST | `GR-031` | `TERM-044`, `QĐ-066` | CONFIRMED |
| PRD-REQ-008 | GOAL-004 | EPIC-002 | J1 | ST | `GR-013`, `GR-018` | `TERM-045` | CONFIRMED |
| PRD-REQ-009 | GOAL-002 | EPIC-002 | J1 | ST, TS | `GR-027` | `QĐ-066` | CONFIRMED |
| PRD-REQ-010 | GOAL-002 | EPIC-002 | J1, J5 | ST, AD | `GR-019` | `GR-019`, `QĐ-066` | CONFIRMED |
| PRD-REQ-011 | GOAL-005 | EPIC-002 | J1 | AD, ST | `GR-031` | `QĐ-064` | CONFIRMED |
| PRD-REQ-012 | GOAL-005 | EPIC-002 | J1 | ST | `GR-031` | `QĐ-063`, `TERM-050` | CONFIRMED |
| PRD-REQ-013 | GOAL-005 | EPIC-002, EPIC-003 | J2, J3 | AD | `GR-031` | `QĐ-071`, `TERM-049` | CONFIRMED |
| PRD-REQ-014 | GOAL-005 | EPIC-002 | J2 | AD | `GR-031` | `CLAUDE.md` §Luật chơi & đề thi | CONFIRMED |
| PRD-REQ-015 | GOAL-007 | EPIC-002, EPIC-008 | J1, J5 | ST, TS | `GR-006`, `GR-027` | `CLAUDE.md` §Quy ước khác | CONFIRMED |
| PRD-REQ-016 | GOAL-009 | EPIC-003 | J3 | AD | `GR-031` | `product-discovery.md` §5 E-3 | CONFIRMED |
| PRD-REQ-017 | GOAL-009 | EPIC-003 | J3 | AD | `GR-031` | `QĐ-071`, `TERM-048` | CONFIRMED |
| PRD-REQ-018 | GOAL-009 | EPIC-003 | J1 | ST | — | `product-discovery.md` §2 A-2 | CONFIRMED |
| PRD-REQ-019 | GOAL-010 | EPIC-003, EPIC-012 | J1 | ST | — | `CLAUDE.md` §Quy ước khác | CONFIRMED |
| PRD-REQ-020 | GOAL-002 | EPIC-004 | J2 | AD | `GR-001`…`GR-025` | `CLAUDE.md` §Quy ước khác · `TERM-051` | CONFIRMED |
| PRD-REQ-021 | GOAL-003 | EPIC-004 | J2 | AD | toàn bộ | `TERM-051` · `traceability.md` | CONFIRMED |
| PRD-REQ-022 | GOAL-002 | EPIC-004 | J2 | AD | `GR-020`, `GR-025` | `traceability.md` §Biến thể bị loại | CONFIRMED |
| PRD-REQ-023 | GOAL-002 | EPIC-004 | J2 | AD | `GR-007`, `GR-017`, `GR-021` | `QĐ-016`, `QĐ-017`, `QĐ-075` | CONFIRMED |
| PRD-REQ-024 | GOAL-002 | EPIC-004 | J2 | AD | `GR-022`, `GR-030` | `QĐ-069` | CONFIRMED |
| PRD-REQ-025 | GOAL-002 | EPIC-004 | J2 | AD | `GR-009` | `QĐ-068` | CONFIRMED |
| PRD-REQ-026 | GOAL-002 | EPIC-004 | J2, J4 | AD | `GR-007`, `GR-016` | `QĐ-007`, `TERM-002` | CONFIRMED |
| PRD-REQ-027 | GOAL-001 | EPIC-005 | J5, J7 | AD | `GR-031` | `QĐ-039`, `TERM-010` | CONFIRMED |
| PRD-REQ-028 | GOAL-007 | EPIC-005 | J5 | AD | `GR-031`, `GR-037` | `EVENT-001`, `QĐ-075` | CONFIRMED |
| PRD-REQ-029 | GOAL-007 | EPIC-005 | J5 | AD | `GR-030` | `QĐ-032`, `QĐ-034` | CONFIRMED |
| PRD-REQ-030 | GOAL-007 | EPIC-005, EPIC-007 | J5 | AD | `GR-030` · `INV-020` | `QĐ-002` | CONFIRMED |
| PRD-REQ-031 | GOAL-007 | EPIC-005, EPIC-007 | J6 | AD | `GR-030` | `QĐ-034`, `QĐ-035` | CONFIRMED |
| PRD-REQ-032 | GOAL-007 | EPIC-005 | J7 | AD | `GR-022` | `QĐ-036`, `EVENT-047` | CONFIRMED |
| PRD-REQ-033 | GOAL-007 | EPIC-005 | J7 | AD | `GR-029` | `QĐ-037`, `STATE-008` | CONFIRMED |
| PRD-REQ-034 | GOAL-007 | EPIC-005 | J6 | AD | `GR-022` | `QĐ-038` | CONFIRMED |
| PRD-REQ-035 | GOAL-007 | EPIC-005 | J6 | AD | `GR-031` | `QĐ-042`, `QĐ-043` | CONFIRMED |
| PRD-REQ-036 | GOAL-007 | EPIC-006, EPIC-007 | J5 | AD, SV | `GR-026`, `GR-027` · `INV-003` | `QĐ-001`, `QĐ-010` | CONFIRMED |
| PRD-REQ-037 | GOAL-007 | EPIC-006 | J5 | AD | `GR-026` · `INV-009` | `QĐ-014` | CONFIRMED |
| PRD-REQ-038 | GOAL-007 | EPIC-006, EPIC-007 | J5 | AD | `GR-026`, `GR-004`, `GR-020` | `QĐ-061` · `game-rules.md` §2.2 | CONFIRMED |
| PRD-REQ-039 | GOAL-007 | EPIC-006 | J5, J6 | SV | `GR-028` · `INV-001` | `QĐ-011` | CONFIRMED |
| PRD-REQ-040 | GOAL-007 | EPIC-006 | J5 | SV | `GR-013`, `GR-028` | `QĐ-013`, `TERM-022` | CONFIRMED |
| PRD-REQ-041 | GOAL-006 | EPIC-006, EPIC-009 | J5 | SV | `GR-004`, `GR-016` · `INV-018` | `QĐ-012` | CONFIRMED |
| PRD-REQ-042 | GOAL-006 | EPIC-006 | J5 | SV | `GR-035` · `INV-004` | `QĐ-006`, `QĐ-029` | CONFIRMED |
| PRD-REQ-043 | GOAL-006 | EPIC-006, EPIC-007 | J5 | AD | `GR-033` | `QĐ-027`, `QĐ-028` | CONFIRMED |
| PRD-REQ-044 | GOAL-006 | EPIC-006 | J5 | SV | `GR-032` · `INV-006` | `QĐ-020`, `QĐ-024` | CONFIRMED |
| PRD-REQ-045 | GOAL-006 | EPIC-006 | J5 | AD, SV | `GR-032`, `GR-007`, `GR-009` | `QĐ-021` | CONFIRMED |
| PRD-REQ-046 | GOAL-006 | EPIC-006, EPIC-008 | J5 | AD, TS | `GR-032`, `GR-007` · `INV-008` | `QĐ-022`, `TERM-027` | CONFIRMED |
| PRD-REQ-047 | GOAL-005 | EPIC-006, EPIC-002 | J2, J5 | AD, SV | `GR-031` · `INV-011` | `QĐ-041`, `QĐ-044` | CONFIRMED |
| PRD-REQ-048 | GOAL-007 | EPIC-006 | J5, J6 | SV | `GR-031`, `GR-005` · `INV-012` | `QĐ-042`, `QĐ-003` | CONFIRMED |
| PRD-REQ-049 | GOAL-005 | EPIC-006, EPIC-009 | J5 | AD, MC, OV | `GR-037` · `INV-017` | `QĐ-051`, `QĐ-062`, `QĐ-080` | CONFIRMED |
| PRD-REQ-050 | GOAL-007 | EPIC-006, EPIC-008 | J6 | TS, SV | `GR-036` | `QĐ-045`, `QĐ-046` | CONFIRMED |
| PRD-REQ-051 | GOAL-007 | EPIC-007 | J5 | AD | `GR-027`, `GR-026` | `QĐ-073`, `QĐ-010` | CONFIRMED |
| PRD-REQ-052 | GOAL-007 | EPIC-007 | J5 | AD | `GR-006`, `GR-015`, `GR-035` | `QĐ-029`, `QĐ-073` | CONFIRMED |
| PRD-REQ-053 | GOAL-007 | EPIC-007 | J5 | AD | `GR-013` | `QĐ-073` | CONFIRMED |
| PRD-REQ-054 | GOAL-006 | EPIC-007 | J5 | AD | `GR-019` · bảng §2.4 | `QĐ-030` | CONFIRMED |
| PRD-REQ-055 | GOAL-007 | EPIC-007 | J5, J6 | AD | `GR-029` | `QĐ-014`, `QĐ-039` | CONFIRMED |
| PRD-REQ-056 | GOAL-007 | EPIC-007 | J5, J6 | AD | `GR-030` | `QĐ-072`, `QĐ-004` | CONFIRMED |
| PRD-REQ-057 | GOAL-007 | EPIC-007 | J5 | AD | `GR-037` | `QĐ-048`, `QĐ-074` | CONFIRMED |
| PRD-REQ-058 | GOAL-007 | EPIC-006, EPIC-007 | J5, J6 | AD | `GR-009`, `GR-011`, `GR-012` | `QĐ-052`, `TERM-053` | CONFIRMED |
| PRD-REQ-059 | GOAL-007 | EPIC-007 | J5, J6 | AD | `GR-032` | `QĐ-020`, `QĐ-024` | CONFIRMED |
| PRD-REQ-060 | GOAL-005 | EPIC-007 | J5 | AD | `GR-031` | `QĐ-044` | CONFIRMED |
| PRD-REQ-061 | GOAL-007 | EPIC-007 | J6 | AD | `GR-036` | `QĐ-047`, `TERM-032` | CONFIRMED |
| PRD-REQ-062 | GOAL-007 | EPIC-007 | J5 | AD | `GR-026` | `QĐ-053`, `T-045`, `T-053` | CONFIRMED |
| PRD-REQ-063 | GOAL-007 | EPIC-007 | J5 | AD | `GR-026` · bảng §2.2 | `QĐ-061` | CONFIRMED |
| PRD-REQ-064 | GOAL-006 | EPIC-008 | J5 | TS | `GR-034`, `GR-003`, `GR-009` | `QĐ-023`, `TERM-025` | CONFIRMED |
| PRD-REQ-065 | GOAL-006 | EPIC-008 | J5 | TS | `GR-006`, `GR-015`, `GR-034` | `QĐ-023`, `QĐ-029` | CONFIRMED |
| PRD-REQ-066 | GOAL-006 | EPIC-006, EPIC-008 | J5 | TS, SV | `GR-015`, `GR-013` | `QĐ-059`, `TERM-037` | CONFIRMED |
| PRD-REQ-067 | GOAL-008 | EPIC-008 | J5 | TS | `GR-028` | `QĐ-012`, `QĐ-015` | CONFIRMED |
| PRD-REQ-068 | GOAL-006 | EPIC-008 | J5 | TS | `GR-007`, `GR-034` | `QĐ-005`, `QĐ-019` | CONFIRMED |
| PRD-REQ-069 | GOAL-006 | EPIC-006, EPIC-008 | J5 | AD, TS | `GR-007`, `GR-021` | `QĐ-019`, `QĐ-022` | CONFIRMED |
| PRD-REQ-070 | GOAL-006 | EPIC-006, EPIC-008 | J5 | AD, TS | `GR-017` | `QĐ-019` | CONFIRMED |
| PRD-REQ-071 | GOAL-008 | EPIC-008 | J5 | TS | `GR-034` | `QĐ-004`, `QĐ-072` | CONFIRMED |
| PRD-REQ-072 | GOAL-008 | EPIC-007, EPIC-008 | J5 | AD, TS | `GR-034`, `GR-006` | `QĐ-060` · `CLAUDE.md` §UX | CONFIRMED |
| PRD-REQ-073 | GOAL-008 | EPIC-009 | J4, J5 | KG, OV | `GR-037` | `product-discovery.md` §3 G-8 · `QĐ-051` | CONFIRMED |
| PRD-REQ-074 | GOAL-008 | EPIC-009 | J5 | MC | `GR-037` | `QĐ-001` · `product-discovery.md` §8 | CONFIRMED |
| PRD-REQ-075 | GOAL-008 | EPIC-009 | J5, J7 | AD, KG | `GR-028`, `GR-025` | `QĐ-049`, `STATE-033` | CONFIRMED |
| PRD-REQ-076 | GOAL-008 | EPIC-009 | J6 | AD | `GR-035` · `INV-016` | `QĐ-050`, `STATE-039` | CONFIRMED |
| PRD-REQ-077 | GOAL-008 | EPIC-009 | J5 | KG, OV | `GR-030` | `QĐ-076` | CONFIRMED |
| PRD-REQ-078 | GOAL-008 | EPIC-009 | J2 | AD | — | `QĐ-079` | CONFIRMED |
| PRD-REQ-079 | GOAL-007 | EPIC-010 | J7 | AD | `GR-028`, `GR-030` | `QĐ-077` | CONFIRMED |
| PRD-REQ-080 | GOAL-007 | EPIC-010, EPIC-011 | J7 | AD | — | `QĐ-077`, `QĐ-040` | CONFIRMED |
| PRD-REQ-081 | GOAL-005 | EPIC-010 | J7 | AD, ST | `GR-031` | `product-discovery.md` §5 E-10 | CONFIRMED |
| PRD-REQ-082 | GOAL-007 | EPIC-011 | mọi J | mọi actor | `GR-037` | `CLAUDE.md` §Quy ước khác · `QĐ-074` | CONFIRMED |
| PRD-REQ-083 | GOAL-007 | EPIC-011 | J2 | AD | — | `product-discovery.md` §7 AS-5 | **NEEDS CLARIFICATION** |
| PRD-REQ-084 | GOAL-007 | EPIC-011 | J2 | AD | — | `product-discovery.md` §3 *(pháp lý)* | CONFIRMED |
| PRD-REQ-085 | GOAL-010 | EPIC-012 | J3, J4 | AD | — | `product-discovery.md` §3 G-10 | CONFIRMED |
| PRD-REQ-086 | GOAL-008 | EPIC-012, EPIC-001 | J4 | AD | — | `CLAUDE.md` §Mô hình truy cập | CONFIRMED |
| PRD-REQ-087 | GOAL-010 | EPIC-012 | J4 | AD | — | `QĐ-067` | **NEEDS CLARIFICATION** |
| PRD-REQ-088 | GOAL-008 | EPIC-006 | J5 | SV, AD | `GR-037`, `GR-020`, `GR-008`, `GR-012` | `QĐ-080`, `TERM-057` | CONFIRMED |
| PRD-REQ-089 | GOAL-005 | EPIC-006 | J5 | SV | `GR-037` · `INV-017` | `QĐ-080` · `CLAUDE.md` §Nguyên tắc code | CONFIRMED |
| PRD-REQ-090 | GOAL-005 | EPIC-004, EPIC-006 | J2, J7 | AD, SV | `GR-022`, `GR-023`, `GR-025`, `GR-017` | `QĐ-081`, `TERM-020` | CONFIRMED |
| PRD-REQ-091 | GOAL-002 | EPIC-005, EPIC-006 | J6, J7 | AD | `GR-023`, `GR-031`, `GR-022` | `QĐ-081`, `QĐ-074`, `QĐ-043` | CONFIRMED |
| PRD-REQ-092 | GOAL-005 | EPIC-002, EPIC-004, EPIC-006 | J1, J2 | ST, AD, SV | `GR-031`, `GR-007`, `GR-008`, `GR-009`, `GR-011` | luật gốc §VCNV · `QĐ-082`, `TERM-058` | CONFIRMED |
| PRD-REQ-093 | GOAL-007 | EPIC-006, EPIC-007 | J6 | AD | `GR-023`, `GR-031` | `QĐ-081`, `QĐ-082` | CONFIRMED |
| PRD-REQ-094 | GOAL-005 | EPIC-003 | J7 | AD | `GR-031` | `QĐ-084`, `TERM-048` | CONFIRMED |
| PRD-REQ-095 | GOAL-005 | EPIC-003 | J7 | AD | `GR-031` | `QĐ-084` | CONFIRMED |
| PRD-REQ-096 | GOAL-007 | EPIC-010 | J7 | AD | `GR-028`, `GR-030`, `GR-037` | `QĐ-084`, `QĐ-077` | CONFIRMED |
| PRD-REQ-097 | GOAL-007 | EPIC-010 | J7 | AD | `GR-022`, `GR-028` | `QĐ-084` | CONFIRMED |
| PRD-REQ-098 | GOAL-009 | EPIC-012, EPIC-003, EPIC-010 | J3, J7 | AD | — | `QĐ-084` | CONFIRMED |

### 22.1 Đối chiếu ngược: Goal → Requirement

| Goal | Requirement |
|---|---|
| GOAL-001 | 027, 088, 089 |
| GOAL-002 | 009, 010, 020, 022, 023, 024, 025, 026, 090, 091, 092 |
| GOAL-003 | 021 |
| GOAL-004 | 008 |
| GOAL-005 | 002, 003, 007, 011, 012, 013, 014, 047, 049, 060, 081 |
| GOAL-006 | 041, 042, 043, 044, 045, 046, 054, 064, 065, 066, 068, 069, 070 |
| GOAL-007 | 004, 005, 006, 015, 028, 029, 030, 031, 032, 033, 034, 035, 036, 037, 038, 039, 040, 048, 050, 051, 052, 053, 055, 056, 057, 058, 059, 061, 062, 063, 079, 080, 082, 083, 084 |
| GOAL-008 | 001, 067, 071, 072, 073, 074, 075, 076, 077, 078, 086 |
| GOAL-009 | 016, 017, 018 |
| GOAL-010 | 019, 085, 087 |

### 22.2 Đối chiếu ngược: Game rule → Requirement

| Game rule | Requirement |
|---|---|
| `GR-001` → `GR-006` *(Khởi động)* | 015, 036, 038, 041, 052, 065, 072 |
| `GR-007` → `GR-012` *(VCNV)* | 026, 045, 046, 058, 068, 069 |
| `GR-013` → `GR-015` *(Tăng tốc)* | 008, 040, 052, 053, 065, 066, 090 |
| `GR-016` → `GR-021` *(Về đích)* | 008, 010, 026, 038, 041, 054, 069, 070, 090, 092 |
| `GR-022` → `GR-025` *(Câu hỏi phụ)* | 024, 032, 034, 075, 092 |
| `GR-026` *(phán quyết)* | 004, 036, 037, 038, 051, 062, 063 |
| `GR-027` *(chuẩn hoá, tô nổi bật)* | 009, 015, 036, 051 |
| `GR-028` *(điểm là hàm event log)* | 039, 040, 067, 075, 079, 101, 102 |
| `GR-029` *(điều chỉnh điểm)* | 005, 033, 055 |
| `GR-030` *(bỏ / chạy lại / kết thúc sớm)* | 005, 029, 030, 031, 056, 077, 079 |
| `GR-031` *(rút đề, không lặp câu)* | 007, 011, 012, 013, 014, 016, 017, 027, 028, 035, 047, 048, 060, 081, 099, 100 |
| `GR-032` *(hàng đợi tín hiệu)* | 044, 045, 046, 059 |
| `GR-033` *(mốc admin bấm)* | 043 |
| `GR-034` *(chuông chỉ click chuột)* | 064, 065, 068, 071, 072 |
| `GR-035` *(server time)* | 042, 052, 076 |
| `GR-036` *(mất kết nối, giữ ghế)* | 050, 061 |
| `GR-037` *(phạm vi hiển thị đáp án)* | 001, 002, 003, 028, 049, 057, 073, 074, 082 |

### 22.3 Đối chiếu ngược: Open question → Requirement bị ảnh hưởng

| Question | Loại | Requirement bị ảnh hưởng |
|---|---|---|
| QUESTION-001 | MISSING | PRD-REQ-016 |
| QUESTION-002 | MISSING | PRD-REQ-085 |
| QUESTION-003 | NEEDS CLARIFICATION | PRD-REQ-086, 087 · NFR-06 |
| QUESTION-004 | NEEDS CLARIFICATION | PRD-REQ-083 |
| QUESTION-005 | NEEDS CLARIFICATION | PRD-REQ-024, 025, 032 |
| QUESTION-006 | NEEDS CLARIFICATION | PRD-REQ-080 · NFR-33, NFR-34 |
| QUESTION-007 | NEEDS CLARIFICATION | PRD-REQ-027 · NFR-11 |
| QUESTION-008 | NEEDS CLARIFICATION | METRIC-005 → METRIC-015 |

---

*Hết tài liệu. Bước tiếp theo trong quy trình: `/speckit.specify` cho từng feature, lấy PRD này làm nguồn cấp trên.*

*Câu hỏi còn ở mức chặn nghiệm thu: `QUESTION-005` — **v1 có khoá cứng phạm vi phân định hoà không** — nên được chủ dự án phân xử trước khi viết spec cho EPIC-005 và EPIC-006.*
