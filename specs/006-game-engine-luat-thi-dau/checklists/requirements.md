# Specification Quality Checklist: Game engine và luật thi đấu

**Purpose**: Validate specification completeness and quality before proceeding to planning
**Created**: 2026-07-30
**Feature**: [spec.md](../spec.md)

## Content Quality

- [x] No implementation details (languages, frameworks, APIs)
- [x] Focused on user value and business needs
- [x] Written for non-technical stakeholders
- [x] All mandatory sections completed

## Requirement Completeness

- [x] No [NEEDS CLARIFICATION] markers remain *(cả 3 marker đã được chủ dự án phân xử ở phiên làm rõ 2026-07-30 — xem Notes)*
- [x] Requirements are testable and unambiguous
- [x] Success criteria are measurable
- [x] Success criteria are technology-agnostic (no implementation details)
- [x] All acceptance scenarios are defined
- [x] Edge cases are identified
- [x] Scope is clearly bounded
- [x] Dependencies and assumptions identified

## Feature Readiness

- [x] All functional requirements have clear acceptance criteria
- [x] User scenarios cover primary flows
- [x] Feature meets measurable outcomes defined in Success Criteria
- [x] No implementation details leak into specification

## Notes

### Về mục "No [NEEDS CLARIFICATION] markers remain" *(cập nhật 2026-07-30, sau `/speckit-clarify`)*

Ba marker sinh ra ở bước `/speckit-specify` **đã được chủ dự án phân xử** trong phiên làm rõ và tích hợp vào spec:

| Marker | Nội dung | Phân xử | Tích hợp tại |
|---|---|---|---|
| `OQ-001` | Thời lượng suy nghĩ câu ô trung tâm VCNV | Dùng **chung một giá trị cấu hình cấp vòng VCNV** với câu hàng ngang và cửa sổ giải Chướng ngại vật sau gợi ý cuối; preset mặc định 15 giây, **cấu hình được** | **FR-083a**, FR-073, FR-083 · **AC-131a** |
| `OQ-002` | Mốc câu khép khi người cướp quyền Về đích bị chấm *Huỷ kết quả* | `GR-037` **C8 thắng C6** — *Huỷ kết quả* không bao giờ tự công bố, phát biểu theo **loại phán quyết** chứ không theo vai bị chấm | **FR-057a** · **AC-071a** |
| `OQ-003` | Nguồn đề Câu hỏi phụ trong trận `practice` của contest thật | Phép **đảo chiều** của `GR-031` C9 chạy **trước**, rồi mới áp thứ tự ưu tiên ba kho; pre-flight đếm trên tập đã lộ, không bù bằng câu chưa lộ | **FR-105a** · **AC-190a** |

Hai mục còn lại — **OQ-004** *(băng điểm cho `rowCount` 5-8)* và **OQ-005** *(luật cho số ghế ≠ 4)* — là *missing source behavior* nằm **ngoài phạm vi v1**, giữ lại để phiên bản sau không lấp bằng suy diễn. Chúng chưa bao giờ mang nhãn `NEEDS CLARIFICATION`.

### ✅ Cổng 1 (truy nguyên) — `OQ-006` ĐÃ ĐÓNG (2026-07-30)

Quyết định đã được ghi thành **`QĐ-104`** trong `docs/decisions.md` §E *(Tín hiệu và hàng đợi)*, và bốn tài liệu đặc tả đã cập nhật theo:

| Tài liệu | Sửa gì |
|---|---|
| `docs/game-rules.md` — bảng §*Phán quyết có hai hay ba lựa chọn* | Thêm dòng **VCNV — tín hiệu *"Mở chướng ngại vật"*** *(Sai ⇒ bị loại, nên có lựa chọn thứ ba)*, kèm ghi chú vì sao đây là ngoại lệ của tiêu chí *"chỉ nhị phân khi Sai trừ 0"* |
| `docs/game-rules.md` — `GR-009` | Thêm ca **C6b** *(Huỷ kết quả ⇒ không bị loại, không sinh điểm, mở đường kích hoạt tay)* và một gạch đầu dòng ở §Điều kiện |
| `docs/game-rules.md` — `GR-032` | Thêm ca **C8**, **C9** và mục **§Kích hoạt tay** đầy đủ sáu vế, kèm ghi chú *"không đụng FIFO"* |
| `docs/game-rules.md` — `GR-037` | Thêm dòng bảng *Mốc câu khép* cho ca có kích hoạt tay · thêm ca **C8b** · **C8** nay nói rõ **C8 thắng C6** ở ca khép-câu của người cướp quyền |
| `docs/game-state-machine.md` | **`EVENT-052`** *(kích hoạt tay — tập ứng viên, chọn lệch thứ tự, side effects)* · mục **§I** mới với **`T-097`** → **`T-100`** *(mở lại cửa sổ · hạn mức theo đích · tín hiệu trơ/chờ duyệt → đã duyệt · cấp lại quyền cho ghế vừa bị huỷ)* · **`INV-009`** viết lại: *"một câu **KHÉP** một lần"*, đơn vị là bộ ba `(câu, thí sinh, loại phán quyết)`, kèm hai ca mốc-khép-đến-muộn và vế dấu-tạm của hai vòng chấm theo lô |
| `docs/traceability.md` | Mục mới **§Bổ sung ngoài O26 — LUÔN BẬT** *(tách khỏi bảng "biến thể cấu hình được" vì cơ chế này không tắt được)*; cập nhật ba dòng ma trận `GR-009`, `GR-032`, `GR-037` |

Dải mã `QĐ-001 → QĐ-104` đã được cập nhật ở `game-rules.md`, `game-state-machine.md`, `PRD.md`, `README.md`, `traceability.md`, `product-discovery.md`.

**Kết quả**: `FR-034a` → `FR-034e` và `FR-055a` nay truy nguyên về `docs/`; **không còn FR nào bị chặn ở khâu implement**.

<details><summary>Bản ghi rủi ro trước khi đóng</summary>

*(mở rộng sau phiên làm rõ thứ hai, 2026-07-30)*

Hai phiên làm rõ chốt một cụm hành vi **mới so với luật gốc O26**: admin **kích hoạt tay** một tín hiệu chuông **bất kỳ còn hiệu lực** trong hàng đợi khi người đang giữ quyền bị chấm *Huỷ kết quả*, áp cho **mọi vòng có giành quyền bằng chuông** — gồm cả tín hiệu *"Mở chướng ngại vật"* của VCNV.

Nội dung đã đầy đủ và nằm ở **FR-034a → FR-034e** cùng **FR-055a**, với AC-044a→AC-044g. Nhưng `docs/game-rules.md` chỉ cấp **giấy phép chung** (`GR-032` C4 · `PRD-REQ-045`: *"hàng đợi vẫn ghi thứ tự để admin can thiệp khi có sự cố"*), **không** mô tả cơ chế — nên cụm FR này truy nguyên về quyết định **phát biểu trong chat**, thứ mà `.specify/memory/constitution.md` §Cổng 1 nói thẳng là **không thoả**.

**Hai điểm trong cụm này sửa thẳng vào tài liệu luật**, không chỉ là bổ sung:

- **Huỷ kết quả trở thành lựa chọn thứ ba** cho tín hiệu Chướng ngại vật ⇒ sửa `GR-009` và **bảng §2.2** của `docs/game-rules.md`.
- **Mốc câu khép lùi** tới sau khi người được kích hoạt đã được chấm ⇒ mở rộng bảng *Mốc câu khép theo vòng* của `GR-037`.

**Việc cần làm**: ghi `QĐ` mới vào `docs/decisions.md` phủ đủ **5 điểm** liệt kê ở `OQ-006` của spec; cập nhật `GR-009`, bảng §2.2 và `GR-037` trong `docs/game-rules.md`; bổ sung `docs/traceability.md` §Biến thể ngoài luật O26. Cho tới lúc đó, **FR-034a → FR-034e** và **FR-055a** MUST NOT chuyển sang task implementation. Phần còn lại của spec **không** bị chặn.

**Không** thuộc diện này: **FR-009a** *(dấu từng ghế sửa được tới mốc chốt câu)* — đây là **cách đọc** `GR-013` §Bấm trùng và `GR-008` §Thứ tự đánh giá, không thêm hành vi mới; và **FR-083a** *(giá trị thời gian VCNV cấu hình được)* — bám thẳng `CLAUDE.md` §Quy ước khác.

</details>

### Kết quả phiên làm rõ thứ hai (2026-07-30)

| Quyết định | Tích hợp tại |
|---|---|
| Kích hoạt tay **có** áp cho tín hiệu *"Mở chướng ngại vật"*, và tín hiệu đó có thêm **Huỷ kết quả** | **FR-034c** · AC-044d |
| Giới hạn số lần bám theo **đích** của tín hiệu: một lần mỗi **câu** ở ba vòng chuông, một lần mỗi ***Huỷ kết quả*** ở VCNV *(không trần tổng)* | **FR-034d** · AC-044e |
| Dấu Đúng/Sai từng ghế ở Tăng tốc và hàng ngang VCNV là **lựa chọn tạm**, sửa được tới mốc *chốt câu* | **FR-009a** · AC-004a |
| Kích hoạt **tín hiệu bất kỳ** còn hiệu lực, không bắt buộc theo thứ tự — máy khuyến nghị, admin quyết | **FR-034a** *(sửa)* · AC-044a |
| Chọn lệch khuyến nghị ⇒ **cảnh báo hạng 2**, Yes/No, **không** bắt lý do | **FR-034e** · AC-044g |
| Tập ứng viên: tín hiệu **trơ chưa xử lý** + ghế **vừa bị Huỷ kết quả**; **loại** ghế bị loại VCNV và ghế vô hiệu hoá | **FR-034e** · AC-044f |

### Sửa lỗi phát hiện trong phiên làm rõ

Bốn khiếm khuyết được phơi ra và đã sửa:

1. **FR-003** viết đơn vị chống trùng của phán quyết theo **câu**, mâu thuẫn **FR-015** vốn dùng bộ ba *(câu, thí sinh, loại phán quyết)*. Bản đọc theo-câu sẽ **cấm chấm người cướp quyền** sau khi đã chấm người thi chính — cấm đúng cơ chế trung tâm của Về đích — và chỏi `GR-008` C2. Đã sửa: `INV-009` đọc là *"một câu **KHÉP** một lần"*.
2. **FR-034a** đặt ra một **chặn cứng tự thêm** (*"MUST NOT cho admin chọn tuỳ ý một ghế ngoài thứ tự"*), trong khi `INV-014` chỉ có **ba** chỗ chặn cứng và đây không phải một trong ba. Đã gỡ, thay bằng mẫu *khuyến nghị + cảnh báo ép được* của `GR-016` C5 / `INV-020`.
3. **FR-034b** viết thời lượng promote ở Câu hỏi phụ là *"giá trị cấu hình của vòng"*, mâu thuẫn **FR-101** vốn khai 3 câu × 15 giây là **cố định, không cấu hình**. Đã sửa thành *"15 giây cố định"*.
4. **SC-003** có lỗi dấu nháy và phát biểu chưa nêu ngưỡng đo. Đã viết lại: *pass 100% cho cả ba kênh · **0** lần xuất hiện đáp án của câu đang mở*.

### Về truy nguyên PRD của luật từng vòng

`FR-062` → `FR-100` *(luật số học của Khởi động, VCNV, Tăng tốc, Về đích)* tham chiếu **neo epic + neo §PRD** thay vì một mã `PRD-REQ-NNN` riêng, vì PRD **không sinh** requirement riêng cho từng con số mà khai chúng qua EPIC-006 §11 *(`Related game rules: GR-001 → GR-037` toàn bộ, Scope: *"luật từng vòng"*)*, `PRD §8.2` *(bảng Số câu theo luật)* và `PRD §9.2` *(mức điểm và thời gian từng vòng)*. Quy ước này ghi rõ ở đầu `spec.md` §Quy ước truy nguyên và ở §Clarifications. Hành vi vẫn có nguồn chuẩn tắc đầy đủ ở `docs/game-rules.md`, nên đây **không** phải lỗ hổng truy nguyên.

### Về đường dẫn đầu vào không tồn tại

Lệnh gọi yêu cầu đọc `docs/rule-traceability.md` và `docs/reviews/prd-review.md`; cả hai **không tồn tại**. Đã dùng `docs/traceability.md` thay thế và ghi chú ở đầu `spec.md`. `docs/reviews/**` không thoả cổng truy nguyên của `.specify/memory/constitution.md` nên không có mất mát nguồn requirement.

### Phủ bảng quyết định

`spec.md` §Traceability Matrix có một bảng **Bản đồ phủ bảng quyết định** ánh xạ từng ca (`C1`, `C2`, …) của 30 `GR-NNN` thuộc phạm vi sang ít nhất một `AC-NNN`, để kiểm được yêu cầu *"mỗi outcome trong decision table phải có ít nhất một acceptance scenario"*. Các ca gắn nhãn `[v1.5]` / `[v2]` được đánh dấu là ngoài phạm vi v1 và trỏ tới `OQ-005` thay vì một AC.

- Items marked incomplete require spec updates before `/speckit-clarify` or `/speckit-plan`
