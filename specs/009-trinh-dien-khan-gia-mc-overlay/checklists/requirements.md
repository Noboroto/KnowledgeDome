# Specification Quality Checklist: Trình diễn — khán giả · lớp phủ · MC · chủ đề · âm thanh

**Purpose**: Kiểm tính đầy đủ và chất lượng của specification trước khi sang bước lập kế hoạch
**Created**: 2026-08-04
**Feature**: [spec.md](../spec.md)

## Content Quality

- [x] Không có chi tiết hiện thực (ngôn ngữ, framework, API)
- [x] Tập trung vào giá trị người dùng và nhu cầu nghiệp vụ
- [x] Viết cho người đọc phi kỹ thuật
- [x] Đã hoàn tất mọi mục bắt buộc

**Ghi chú kiểm chứng.** Hai con số kỹ thuật xuất hiện trong spec — `1920×1080` và *"kênh một chiều server → client"* — **không** phải chi tiết hiện thực do spec tự chọn: cái thứ nhất là con số chuẩn tắc của `PRD-REQ-073`, cái thứ hai là phát biểu **chiều truyền** của `PRD-REQ-107` và `QĐ-088`, không nêu giao thức nào. Spec cố ý **không** viết tên giao thức, tên thư viện hay tên endpoint ở bất kỳ FR nào.

## Requirement Completeness

- [x] Không còn marker [NEEDS CLARIFICATION] — **9/9 mục đã phân xử sau phiên hai**, xem §Ghi chú
- [x] Requirement kiểm thử được và không nhập nhằng
- [x] Success criteria đo được
- [x] Success criteria không phụ thuộc công nghệ
- [x] Đã định nghĩa đủ acceptance scenario
- [x] Đã nhận diện edge case
- [x] Phạm vi được khoanh rõ
- [x] Đã nhận diện dependency và assumption

## Feature Readiness

- [x] Mọi functional requirement có acceptance criteria rõ ràng
- [x] User scenario phủ các luồng chính
- [x] Feature đáp ứng được các outcome đo được ở Success Criteria
- [x] Không có chi tiết hiện thực rò vào specification

## Kiểm bổ sung theo yêu cầu của lệnh gọi

- [x] Mỗi user story có đủ 10 trường: ID · Title · Actor · Intent · User value · Priority · Priority rationale · Independent test · Related PRD requirements · Related game rules · Related journey
- [x] Mỗi acceptance scenario có ID · US liên quan · FR liên quan · GR liên quan · Given/When/Then
- [x] `Given` nêu đủ actor, game state, dữ liệu ban đầu và precondition
- [x] `When` chỉ chứa một action hoặc event chính
- [x] `Then` kiểm business outcome, state transition, side effect, error outcome, và **no-change guarantee**
- [x] Mỗi outcome trong bảng quyết định của `GR-NNN` thuộc phạm vi feature có ít nhất một AC — xem §*Bản đồ phủ bảng quyết định*; ca thuộc epic khác được ghi rõ spec sở hữu
- [x] Mỗi FR có ID · phát biểu kiểm thử được · tham chiếu US · PRD-REQ · GR · AC
- [x] FR không chứa framework, database, API route, class, function hay thuật toán
- [x] Edge case liệt kê đủ tám nhóm: boundary · invalid state · repeated action · stale state · duplicate event · partial failure · conflicting rule · missing source behavior
- [x] Out of scope ghi rõ requirement và epic không thuộc feature này
- [x] Có bảng traceability đúng năm cột yêu cầu
- [x] Không tự thêm game behavior · không tự giải CONFLICT · không tự chọn thứ tự validation · không tự tạo error code · không đổi nghĩa `GR-NNN`

## Ghi chú

### Phiên làm rõ 2026-08-04 — 5 câu hỏi, 6/9 mục đã đóng

`/speckit-clarify` chạy 5 câu hỏi *(mức trần)* cộng một lần làm rõ lại không tính vào trần. Kết quả:

| Mã | Loại | Trạng thái sau phiên | FR / AC liên quan |
|---|---|---|---|
| `CONFLICT-001` | CONFLICT | ✅ **Đóng** — banner tạm dừng **KHÔNG** phủ màn MC; §F bảng `F.3` bỏ MC | FR-055, **FR-055a** · AC-050, **AC-050a** |
| `CONFLICT-002` | CONFLICT | ✅ **Đóng** — tách theo **khe**: *mở màn công bố* hướng khán giả, năm khe sự cố chỉ-admin; nguồn phát `admin \| overlay` mặc định `overlay`, không lui tự động | FR-074, **FR-074a/b/c**, **FR-065a** · AC-069, **AC-069a/b/c** |
| `OQ-001` | NEEDS CLARIFICATION | ✅ **Mất chủ ngữ** theo `CONFLICT-001` | FR-055a · AC-050a |
| `OQ-002` | NEEDS CLARIFICATION | ✅ **Đóng** — banner × lớp công bố **loại trừ lẫn nhau**, hạng invalid state | FR-044, **FR-058a** · AC-041, **AC-058a** |
| `OQ-003` | NEEDS CLARIFICATION | ✅ **Đóng** — lớp phủ **CÓ** nhận đáp án Chướng ngại vật; đứng ngoài `GR-037` chỉ đổi **thời điểm**, không đổi **người nhận** | FR-026, **FR-026a** · AC-023 |
| `OQ-006` | Missing evaluation order | ✅ **Đóng** — **cú đóng tay thắng** cú đẩy tại mốc câu khép; phạm vi **câu**, không dính sang câu sau | FR-027, **FR-027a** · AC-024, **AC-024a** |
| `OQ-004` | NEEDS CLARIFICATION | ⏳ **Còn mở** — *"chủ đề hiển thị"* gồm gì, áp cho màn nào | ⚠️ **FR-072** *(chặn)* |
| `OQ-005` | NEEDS CLARIFICATION | ⏳ **Còn mở một vế** — danh sách **đầy đủ** khe nhóm *mốc thi đấu*. Vế **kênh phát đã đóng** cùng `CONFLICT-002` | FR-074 *(ghi chú, **không** chặn)* |
| `OQ-007` | Missing invalid-transition outcome | ⏳ **Còn mở** — kênh public hiện gì khi lớp phủ bật mà trạng thái bên dưới không cho nội dung có nghĩa | **Không chặn FR nào** |

**Sau phiên một còn đúng một FR mang cảnh báo ⚠️: `FR-072`.** Trước phiên một là năm — FR-026, FR-027, FR-055, FR-072, FR-074. Phiên hai đóng nốt; xem mục kế tiếp.

### Phiên làm rõ 2026-08-04 phiên hai — 4 câu hỏi, 3 mục cuối đã đóng

| Mã | Loại | Trạng thái sau phiên hai | FR / AC liên quan |
|---|---|---|---|
| `OQ-004` | NEEDS CLARIFICATION | ✅ **Đóng** — chủ đề = **ba trục tĩnh** *(bảng màu · logo · ảnh nền)*, **không** tài sản động, **không** phông; áp cho đúng **hai kênh public** | FR-072, **FR-072a**, **FR-072b** · AC-067, **AC-067a** |
| `OQ-005` | NEEDS CLARIFICATION | ✅ **Đóng** — **52 khe** mốc thi đấu dẫn xuất từ trang `Âm thanh` wiki Fandom, lọc biến thể ngoài O26 + khe truyền hình; biến thể độ dài gộp thành một khe *đếm giờ* mỗi vòng | **FR-074d**, **FR-074e** · **AC-069d** |
| `OQ-007` | Missing invalid-transition outcome | ✅ **Đóng** — lớp phủ rỗng **dựng bình thường với dữ liệu hiện có**; không nhánh trạng thái rỗng, không từ chối cú mở | **FR-085a** · **AC-079a** |

**9/9 mục đã phân xử. Không còn mục nghiệp vụ nào mở.** Spec từ 1.031 → ~1.075 dòng; FR 93 → 100; AC 85 → 89; SC 21 → 23.

### Cổng truy nguyên phiên hai: ✅ ĐÃ ĐÓNG (2026-08-04)

`docs/decisions.md` nhận **ba `QĐ` mới**:

| Mã | Nội dung | Hạng nguồn |
|---|---|---|
| `QĐ-121` | Chủ đề hiển thị = **ba trục tĩnh**, áp cho **đúng hai kênh public** | `[CHỦ DỰ ÁN]` |
| `QĐ-122` | **52 khe** *mốc thi đấu* dẫn xuất từ wiki Fandom, lọc hai tầng + gộp khe đếm giờ | `[CHỦ DỰ ÁN]` · *Lấp chỗ trống của* `QĐ-079` |
| `QĐ-123` | Lớp phủ thiếu dữ liệu vẫn dựng **bình thường**, không nhánh trạng thái rỗng | `[CHỦ DỰ ÁN]` |

**Một nguồn mới**: `docs/source/fandom-olympia-26-am-thanh.md` — snapshot trang [`Âm thanh`](https://duong-len-dinh-olympia.fandom.com/vi/wiki/%C3%82m_thanh) lấy 2026-08-04, **trích lược cột `Tên`** *(bỏ cột file và cột khoảng ngày phát sóng; lý do ghi ở §Phạm vi trích lược của chính file)*. Đây là file thứ hai trong `docs/source/`, sau bản luật.

**Năm file đã sửa theo**: `docs/PRD.md` *(`PRD-REQ-075`, `PRD-REQ-078` · §22 ma trận · §22.3 · bảng nguồn · bump **2.8.0**)* · `docs/traceability.md` *(bảng nguồn gốc nhận dòng thứ hai)* · `docs/decisions.md` *(`QĐ-079` nhận con trỏ ⚠️ ngược cho cụm "ngoài các mốc thi đấu")* · `docs/README.md` và `CLAUDE.md` *(đếm quyết định 120 → 123)*.

**Kết quả**: **0 FR** còn mang cảnh báo chặn implementation. Nợ truy nguyên còn lại: **không**.

**Ghi chú phương pháp cho `OQ-005`**: đây là mục duy nhất trong chín mục được đóng bằng **bằng chứng ngoài**, không phải bằng suy luận từ `docs/`. Chủ dự án chỉ định wiki Fandom — cùng nguồn đã được `QĐ` cũ chọn làm source of truth luật O26 — và trang `Âm thanh` của wiki có cột *Tên* đúng là danh sách **khe ngữ nghĩa**, không phải danh sách file nhạc. Vì vậy nợ truy nguyên của mục này nặng hơn hai mục kia: nó cần một **snapshot bất biến**, không chỉ một `QĐ`.

### Vì sao ba mục từng để nguyên sau phiên một — và đó đã là hành vi ĐÚNG

Lệnh gọi `/speckit-specify` ghi rõ ở §Open questions: *"Giữ nguyên NEEDS CLARIFICATION, CONFLICT, missing boundary, missing evaluation order, missing invalid-transition outcome. **Không tự trả lời.**"* Yêu cầu này **thắng** mục checklist mặc định *"Không còn marker [NEEDS CLARIFICATION]"*, khớp Nguyên tắc II và III của `.specify/memory/constitution.md` bản 1.4.0, và khớp cách `specs/005` → `specs/008` đã xử lý. `/speckit-clarify` có trần **5 câu hỏi**; ba mục còn lại là ba mục **tác động thấp nhất** trong chín mục và không mục nào chặn quá một FR.

### Cổng truy nguyên: ✅ ĐÃ ĐÓNG cho sáu mục đã phân xử (2026-08-04)

`docs/decisions.md` nhận **năm `QĐ` mới**:

| Mã | Nội dung | Hạng nguồn |
|---|---|---|
| `QĐ-116` | Banner tạm dừng **KHÔNG** phủ màn MC | `[CHỦ DỰ ÁN]` |
| `QĐ-117` | Banner × lớp công bố **loại trừ lẫn nhau**, hạng invalid state | `[CHỦ DỰ ÁN]` |
| `QĐ-118` | Đáp án Chướng ngại vật tới **cả overlay** — đứng ngoài `GR-037` chỉ đổi **thời điểm** | `[CHỦ DỰ ÁN]` · *Thay cho* chữ *"cho viewer"* ở `GR-012` C2 |
| `QĐ-119` | Thao tác tay thắng ở **cả hai chiều** — cú đóng chặn cú đẩy tại mốc câu khép | `[CHỦ DỰ ÁN]` · *Thay cho* vế *"engine không bao giờ tự mở gì"* của `QĐ-074` |
| `QĐ-120` | Khe âm thanh **hai nhóm** + **nguồn phát** `admin \| overlay` | `[CHỦ DỰ ÁN]` · *Thay cho* danh sách phẳng của `QĐ-079` |

**Bảy file đã sửa theo**: `docs/PRD.md` *(`PRD-REQ-049`, `073`, `075`, `076`, `077`, `078` · §22.3 · bump **2.7.0** · đếm quyết định)* · `docs/game-rules.md` *(`GR-012` §Điều kiện + C2 + §Nguồn; `GR-037` **C10** mới + §Thứ tự đánh giá bước (5b) + §Bấm trùng + §Không đổi gì + §Nguồn)* · `docs/game-state-machine.md` *(§F bảng nhóm · `STATE-021` · `STATE-033` · `STATE-039` · `STATE-040` + bảng ràng buộc hai chiều thứ hai · `EVENT-032` · `EVENT-034` · `T-087` · `T-089` · §Invalid transitions · `INV-017` · `INV-021` · sơ đồ mermaid)* · `docs/permissions.md` *(`PERM-044`, `PERM-047`, `PERM-048`, dòng vai MC)* · `docs/glossary.md` *(`TERM-007`)* · `docs/README.md` và `CLAUDE.md` *(đếm quyết định 115 → 120)*.

**Hai con trỏ ⚠️ ngược** được đặt vào `QĐ-074` và `QĐ-079` để người đọc mục cũ biết vế nào đã bị thay — cùng khuôn với con trỏ mà `QĐ-111` để lại ở `QĐ-023`.

**Kết quả**: mọi FR neo vào sáu mục đã phân xử nay **truy nguyên được về `docs/`** và vào task implementation được. Nợ truy nguyên còn lại: **không**.

### Chênh đường dẫn đầu vào — đã xử lý, không phải một mục mở

Lệnh gọi yêu cầu đọc `docs/rule-traceability.md` và `docs/reviews/prd-review.md`; **cả hai không tồn tại**. Đã dùng `docs/traceability.md` thay cho cái thứ nhất; không có tài liệu thay thế cho cái thứ hai, và `docs/reviews/**` vốn **không thoả cổng truy nguyên** theo constitution nên dù có cũng không dùng làm nguồn requirement. Đây là chênh **đầu vào**, không phải mâu thuẫn nghiệp vụ, nên nó nằm ở §Lưu ý về đầu vào của spec chứ không ở §Open Questions. Cùng cách xử lý đã dùng ở `specs/005` → `specs/008`.

### Hai mục `Out of scope` của EPIC-009 nằm TRONG spec

`docs/PRD.md` §11 EPIC-009 liệt kê *"báo cho khán giả về can thiệp của admin"* và *"đẩy khuyến nghị lượt xuống khán giả"* ở mục **Out of scope**. Hai thứ đó bị **cấm**, không phải chưa làm — nên spec giữ chúng dưới dạng `MUST NOT` ở FR-065 → FR-071 và kiểm bằng SC-013, SC-014. Chỉ **truyền video** (`NON-GOAL-003`) mới thật sự nằm ngoài.

### Kết quả rà

**Vòng soạn thảo (2026-08-04, `/speckit-specify`)** — chạy 3 vòng rà. Vòng 1 phát hiện `CONFLICT-002` *(khe âm thanh vs lệnh cấm báo khán giả)* — ban đầu bị bỏ sót vì `PRD-REQ-078` khai **Related game rules: —** nên dễ đọc thành một mục không giao với ai. Vòng 2 bổ sung `OQ-006` và `OQ-007` sau khi rà lại §Thứ tự đánh giá và §Invalid transitions cho từng lớp phủ. Vòng 3 đối chiếu bảng phủ quyết định với `GR-004`, `GR-012`, `GR-016`, `GR-025`, `GR-028`, `GR-030`, `GR-035`, `GR-037` — mọi ca đều có AC hoặc có ghi chú spec sở hữu.

**Vòng làm rõ (2026-08-04, `/speckit-clarify`)** — 5 câu hỏi, 6 mục đóng. Spec từ 928 → 1.031 dòng; FR 85 → 93; AC 79 → 85; SC 18 → 21. Rà lại ID: **0** trùng, **0** đứt số. Ba phán quyết đáng ghi vì chúng **sửa lại chính cách đọc nguồn**, không chỉ lấp chỗ trống:

- `CONFLICT-001` — cột *"Ai thấy"* của bảng §F trộn **hai** câu hỏi *(ai nhìn thấy lớp phủ / lớp phủ chặn thao tác của ai)*; nhóm `F.3` chỉ có nghĩa với vế thứ hai, mà màn MC read-only nên không có thao tác để chặn.
- `CONFLICT-002` — cả hai phía tranh trên **trục sai**. Câu hỏi đúng là *"**từng khe** có hướng khán giả không"*, không phải *"nhóm sự kiện điều khiển có phát cho khán giả không"*. `QĐ-076` cấm bốn thao tác **sự cố**; *mở màn công bố* là cú bấm **trình diễn**.
- `OQ-003` — `GR-012` C2 **thiếu chữ do trình tự ban hành**: nó viết trước `QĐ-080`, thời điểm overlay **vẫn** mang lệnh cấm tuyệt đối, nên chữ *"viewer"* đúng lúc viết và thành thiếu sót sau khi vế 2 gỡ lệnh cấm.

**Vòng làm rõ phiên hai (2026-08-04)** — 4 câu hỏi, 3 mục cuối đóng. Mục `OQ-005` đổi cách giải quyết giữa chừng: ba phương án đầu *(dẫn xuất từ `EVENT-001` → `EVENT-052` · tuyển chọn tay · dẫn xuất có loại trừ)* đều là **suy diễn nội bộ**; chủ dự án chỉ ra rằng nguồn gốc O26 đã có sẵn danh sách, và kiểm chứng cho thấy đúng — trang `Âm thanh` của wiki Fandom có cột *Tên* là khe ngữ nghĩa. Phương án dẫn xuất-từ-nguồn thắng cả ba.

**Toàn bộ mục checklist nay PASS.** Mục *"không còn marker"* — vốn cố ý để trống sau `/speckit-specify` và sau phiên làm rõ một — đã thoả sau phiên hai: 9/9 mục phân xử xong. Điều kiện còn lại **không** thuộc chất lượng spec mà thuộc **truy nguyên**: ba `QĐ` và một snapshot phải vào `docs/` trước khi sáu FR tương ứng vào task.
