# Roadmap sau v1 — phạm vi v1.5 và v2

> **Phạm vi tài liệu.** Đây là nơi giữ **mọi hạng mục ngoài phiên bản 1.0** đã được tách khỏi `docs/PRD.md`. `PRD.md` chỉ đặc tả **v1**; tài liệu này giữ actor, hành trình, epic, yêu cầu và câu hỏi mở thuộc **v1.5** và **v2**, cùng danh sách hạng mục **chưa gắn mốc**.
>
> **Hiệu lực.** Tài liệu này **không phải requirement đang thi hành**. Không mục nào ở đây được đưa vào `/speckit.specify` cho tới khi phạm vi phiên bản tương ứng được mở. Khi mở, mục tương ứng **chuyển ngược về `PRD.md`** và rời khỏi đây.
>
> **Nguồn.** Giữ nguyên nguồn của từng mục khi còn ở `PRD.md`: `docs/source/`, `docs/decisions.md`, `docs/game-rules.md`, `docs/game-state-machine.md`, `docs/glossary.md`, `docs/traceability.md`, `docs/product-discovery.md`. `plans/**` và `docs/reviews/**` **không phải nguồn**.
>
> **Đánh số — chia chung một dãy với `PRD.md`, không đè lên nhau.** Tài liệu này giữ `PRD-REQ-099`→`103`, `QUESTION-009`→`011`, `ACTOR-007`, `JOURNEY-008`, `EPIC-013`→`014`, `FS-37`→`38`. Khi một hạng mục được mở phạm vi, nó **chuyển thẳng sang `PRD.md`** giữ nguyên mã, và không lúc nào hai tài liệu mang trùng mã.
>
> **Cập nhật 2026-07-29.** `PRD.md` nay dùng tới `PRD-REQ-107` — dãy `099`→`103` vẫn thuộc tài liệu này, nên `PRD.md` có một khoảng trống cố ý ở đó. `PRD.md` **không còn câu hỏi mở nào**; ba `QUESTION-*` dưới đây giữ nguyên số hiệu để mọi trích dẫn cũ không trôi nghĩa.
>
> Số hiệu ở đây là **chỉ mục của bản hiện hành**, không phải định danh vĩnh viễn: `PRD.md` đánh số lại mỗi khi có mục đóng hoặc chuyển đi, và tài liệu này chạy theo. Trích dẫn `PRD-REQ-*` hay `QUESTION-*` từ nơi khác phải kèm **tên** hạng mục.

---

## 1. Document Status

| Trường | Giá trị |
|---|---|
| **Version** | 1.1.0 — tách khỏi `PRD.md`, rồi đánh số nối tiếp `PRD.md` 2.0.0 |
| **Status** | Kho phạm vi hoãn — chờ chủ dự án mở phiên bản |
| **Last updated** | 2026-07-29 |
| **Open clarification count** | **3** *(`QUESTION-009`, `QUESTION-010`, `QUESTION-011`)* |

### Căn cứ lộ trình

`CLAUDE.md` §Lộ trình version · `QĐ-007` — ba phiên bản, mô hình dữ liệu chuẩn bị **đầy đủ ngay từ v1** để phiên bản sau không phải chuyển đổi dữ liệu:

| Phiên bản | Nội dung | Vị trí đặc tả |
|---|---|---|
| **v1** | Contest chính thức, thí sinh cá nhân, luật cho **đúng 4 thí sinh** | `docs/PRD.md` |
| **v1.5** | Luyện tập + **luật cho 1-12 thí sinh** | Tài liệu này §3, §4, §5, §6 |
| **v2** | Thi đội — bấm chuông cá nhân, điểm về đội | Tài liệu này §3, §5, §6 |

---

## 2. Target Users — bổ sung ngoài v1

| Nhóm | Mô tả | Nguồn | Phiên bản |
|---|---|---|---|
| **Người phụ trách luyện tập** | Chạy trận luyện tập và lặp lại nhanh | `product-discovery.md` §2 A-7 | `[v1.5]` |

---

## 3. Actors

### ACTOR-007 — Người phụ trách luyện tập (trainer) `[v1.5]`

- **Role**: Tạo và chạy trận luyện tập.
- **Goals**: Tạo trận `practice`, luyện tập, chạy lại nhanh giữ nguyên ghế và mã phòng.
- **Permissions**: Như admin trong phạm vi trận `practice`. Là **quyền gán theo contest**, không phải vai seed (`QĐ-065`).
- **Constraints**: Trong một contest thật, trận `practice` **chỉ được gán câu đã hiển thị** ở các trận thật trước đó — phép lọc kho đề đảo chiều (`QĐ-040`, `TERM-048`).
- **Related journeys**: JOURNEY-008.
- **Source**: `product-discovery.md` §2 A-7 · `QĐ-040`, `QĐ-065`.

> **Thi đội `[v2]` không thêm actor mới** — đơn vị điểm đổi từ người sang đội, nhưng người thao tác vẫn là ACTOR-003 thí sinh.

---

## 4. Journeys

### JOURNEY-008 — Luyện tập `[v1.5]`

- **Actor**: ACTOR-007 người phụ trách luyện tập · ACTOR-003 thí sinh
- **Starting state**: Có contest practice, hoặc contest thật đã chạy ít nhất một trận
- **Goal**: Luyện tập lặp lại nhanh
- **Main flow**: Tạo trận `practice` → luyện tập → chạy lại nhanh, giữ ghế và mã phòng
- **Alternative flows**: Trong contest thật, kho đề của trận practice **chỉ gồm câu đã hiển thị** ⇒ không thể dùng contest thật để tổng duyệt trước trận · `revealAnswerAfterJudge` mặc định **bật** ở practice
- **End state**: Trận practice đóng sổ; không câu nào bị tiêu thêm
- **Related game rules**: `GR-031` C9, `GR-037`
- **Source**: `product-discovery.md` §4 J-8 · `QĐ-040`, `QĐ-062`

> **Thi đội `[v2]` chưa có hành trình** — `PRD-REQ-102` và `PRD-REQ-103` ghi *"(chưa đặc tả)"* ở trường Related journey.

---

## 5. Epics

### EPIC-013 — Luyện tập `[v1.5]`

- **Goal**: Thí sinh và người phụ trách luyện tập được mà không đốt kho đề.
- **Primary actor**: ACTOR-007 người phụ trách luyện tập
- **User value**: Chuẩn bị trước trận thật, và dùng lại đề đã lộ một cách an toàn.
- **Scope**: `matchPurpose: practice` · bộ đề public và liên kết chia sẻ · giao diện luyện tập một mình · vai trainer · hạn lưu trữ riêng · `revealAnswerAfterJudge` mặc định bật · **luật cho 1-12 thí sinh** *(chỉ ship luật và sửa bộ điều khiển, không đổi mô hình dữ liệu)*.
- **Out of scope**: Thi đội (NON-GOAL-013).
- **Related journeys**: JOURNEY-008
- **Related game rules**: `GR-031` C9, `GR-037`
- **Dependencies**: EPIC-006, EPIC-011 *(đều thuộc v1)*
- **MVP status**: **v1.5 — sau MVP**
- **Source**: `product-discovery.md` §5 E-13 · `CLAUDE.md` §Lộ trình version · `QĐ-007`, `QĐ-040`, `QĐ-062`

### EPIC-014 — Thi đội `[v2]`

- **Goal**: Nhiều thí sinh chia sẻ một đơn vị điểm.
- **Primary actor**: ACTOR-003 thí sinh *(theo đội)*
- **User value**: Mở format thi mới cho cùng một engine.
- **Scope**: Đội và đơn vị điểm cấp đội · bấm chuông theo cá nhân, điểm về đội · luật riêng cho Ngôi sao hy vọng và Câu hỏi phụ khi thi đội.
- **Out of scope**: Toàn bộ v1 và v1.5.
- **Related journeys**: *(chưa đặc tả)*
- **Related game rules**: `GR-010` C6 `[v2]`
- **Dependencies**: EPIC-013
- **MVP status**: **v2 — sau MVP**. **Hai câu hỏi luật còn mở**: `QUESTION-010`, `QUESTION-011`
- **Source**: `product-discovery.md` §5 E-14 · `TERM-003`, `TERM-009` · `CLAUDE.md` §Lộ trình version

---

## 6. Requirements

> Giữ nguyên định dạng của `PRD.md` §12. Dãy bắt đầu từ `PRD-REQ-099` vì `PRD.md` dùng tới `098`.

### EPIC-013 — Luyện tập `[v1.5]`

**PRD-REQ-099 — Trận luyện tập trong contest thật chỉ dùng câu ĐÃ HIỂN THỊ**
- **Description**: Trong một contest thật, trận luyện tập MUST chỉ được gán câu **đã hiển thị** ở các trận thật trước đó — phép lọc kho đề **đảo chiều** so với trận official — và MUST NOT đánh dấu thêm câu nào là đã dùng.
- **Actor**: ACTOR-007 · **Related epic**: EPIC-013 · **Related journey**: JOURNEY-008 · **Related game rules**: `GR-031` C9
- **Priority**: P3 · **Rationale**: Đây là **cơ chế**, không phải kỷ luật con người: hệ quả là **không thể dùng contest thật để tổng duyệt trước trận**.
- **Acceptance intent**: Tạo trận luyện tập trong contest thật chưa chạy trận nào ⇒ kho đề rỗng, không mở vòng được.
- **Source**: `QĐ-040` · `GR-031` · `TERM-015`, `TERM-048` · **Status**: CONFIRMED

**PRD-REQ-100 — Bộ đề public và liên kết chia sẻ cho luyện tập**
- **Description**: Hệ thống MUST hỗ trợ bộ đề public kèm liên kết chia sẻ, và giao diện luyện tập một mình.
- **Actor**: ACTOR-007, ACTOR-003 · **Related epic**: EPIC-013 · **Related journey**: JOURNEY-008 · **Related game rules**: `GR-031`
- **Priority**: P3 · **Rationale**: Đây là chỗ cờ *đã từng public* sinh ra, và cũng là lý do hàng rào chặn trận official phải tồn tại.
- **Acceptance intent**: Câu vào bộ đề public mang dấu vết một chiều ngay lập tức.
- **Source**: `CLAUDE.md` §Lộ trình version · `QĐ-063`, `QĐ-071` · **Status**: CONFIRMED

**PRD-REQ-101 — Luật cho 1-12 thí sinh `[v1.5]`**
- **Description**: v1.5 MUST bổ sung luật cho 1-12 thí sinh — thang điểm Tăng tốc và các quy tắc phụ thuộc số ghế — và MUST chỉ đổi luật cùng lớp điều khiển, MUST NOT đổi mô hình dữ liệu.
- **Actor**: ACTOR-003 · **Related epic**: EPIC-013 · **Related journey**: JOURNEY-005 · **Related game rules**: `GR-013` C5, `GR-014`, `GR-016`, `GR-020` C6
- **Priority**: P3 · **Rationale**: Mô hình dữ liệu và giao diện đã làm cho 1-12 từ v1 chính là để bước này không phải chuyển đổi dữ liệu.
- **Acceptance intent**: Ship luật đa ghế không kèm bước chuyển đổi dữ liệu nào.
- **Source**: `QĐ-007` · `CLAUDE.md` §Lộ trình version · **Status**: NEEDS CLARIFICATION — *thang điểm Tăng tốc cho số ghế ≠ 4 chưa có trong bất kỳ nguồn nào; ai quyết giá trị và theo căn cứ gì?* (`QUESTION-009`)

### EPIC-014 — Thi đội `[v2]`

**PRD-REQ-102 — Đơn vị điểm cấp đội**
- **Description**: v2 MUST hỗ trợ đơn vị điểm là **đội**: bấm chuông theo cá nhân, điểm về đội. Mô hình dữ liệu MUST đã sẵn sàng từ v1.
- **Actor**: ACTOR-003 · **Related epic**: EPIC-014 · **Related journey**: *(chưa đặc tả)* · **Related game rules**: `GR-010` C6 `[v2]`
- **Priority**: P3 · **Rationale**: Chuẩn bị mô hình dữ liệu từ v1 để không phải chuyển đổi về sau.
- **Acceptance intent**: Mọi mảng cấu hình luật đánh chỉ số theo **đơn vị điểm**, không theo người.
- **Source**: `TERM-003`, `TERM-009` · `QĐ-007` · **Status**: CONFIRMED

**PRD-REQ-103 — Luật thi đội cho Ngôi sao hy vọng và Câu hỏi phụ**
- **Description**: v2 MUST đặc tả ai trong đội được đặt Ngôi sao hy vọng và khoá lúc nào, và ai chọn người thi Câu hỏi phụ.
- **Actor**: ACTOR-003 · **Related epic**: EPIC-014 · **Related journey**: *(chưa đặc tả)* · **Related game rules**: `GR-021`, `GR-023`
- **Priority**: P3 · **Rationale**: Đội ba người có ba lượt và chín câu nhưng **một** ngôi sao — giao của hai luật này chưa có nguồn nào phủ.
- **Acceptance intent**: *(chưa viết được — xem trạng thái)*
- **Source**: `docs/reviews/game-rules-review.md` GRR-170, GRR-171 · **Status**: NEEDS CLARIFICATION *(`QUESTION-010`, `QUESTION-011`)*

---

## 7. Feature Summary — hạng mục ngoài v1

| Mã | Hạng mục | Epic | Requirement |
|---|---|---|---|
| FS-37 | Luyện tập: phép lọc kho đề đảo chiều, bộ đề public, luật đa ghế | EPIC-013 | PRD-REQ-099, 100, 101 |
| FS-38 | Thi đội | EPIC-014 | PRD-REQ-102, 103 |

---

## 8. Post-MVP Scope

### 8.1 v1.5

| Hạng mục | Epic | Requirement |
|---|---|---|
| Trận luyện tập, bộ đề public, liên kết chia sẻ, giao diện luyện tập một mình, vai trainer, hạn lưu trữ riêng | EPIC-013 | PRD-REQ-099, 100 |
| **Luật cho 1-12 thí sinh** — chỉ ship luật và lớp điều khiển, không đổi mô hình dữ liệu | EPIC-013 | PRD-REQ-101 |
| **Job dọn dữ liệu theo hạn lưu trữ** | EPIC-011 | PRD-REQ-080 *(yêu cầu vẫn ở `PRD.md`; chỉ **mốc ship** là v1.5)* |

### 8.2 v2

| Hạng mục | Epic | Requirement |
|---|---|---|
| Thi đội — đơn vị điểm cấp đội, bấm chuông cá nhân điểm về đội | EPIC-014 | PRD-REQ-102 |
| Luật thi đội cho Ngôi sao hy vọng và Câu hỏi phụ | EPIC-014 | PRD-REQ-103 |

### 8.3 Chưa gắn mốc

| Hạng mục | Ghi chú |
|---|---|
| **Phát lại một trận** | `product-discovery.md` §5 E-10 ghi *"phát lại: sau"*, không gắn phiên bản |
| **Đánh phiên bản khi sửa câu đã duyệt** | §8 xếp là mục cắt được của kho đề |
| **Kick thí sinh khỏi trận** | Nhu cầu *"rời hẳn một ghế khỏi trận"* **hoãn sang phiên bản sau** (`TERM-032`) |
| **Mở khoá số hàng ngang 5-8 và playlist tuỳ ý** | Mô hình dữ liệu đã sẵn sàng; đường xử lý và cửa giao diện chưa có (`QĐ-068`, `QĐ-069`) |
| **Mở khoá phạm vi phân định hoà (`tieBreakPositions` ngoài `[1]`)** | Khoá cứng thứ ba của v1 (`QĐ-085`). Mở ra **phải viết mới bốn thứ chưa có nguồn**: thứ tự giải nhiều nhóm hoà · ngân sách `3N` câu khi `N` chỉ biết tại cú bấm chốt trận · điều kiện tái nhập `STATE-007` *(hiện bị `GR-022` C7 chặn)* · cách đọc `QĐ-083` vế 5 khi một nhóm mất hiệu lực còn nhóm kia thì không |
| **Bốn chỉ số thành công còn hoãn** | Tỉ lệ admin dựng được contest lần đầu không cần hỏi ai · tỉ lệ tái sử dụng câu hỏi sau 5 trận · số trận thật trong ba tháng đầu · nhân sự tối thiểu vận hành một trận. Cả bốn cần **người ngoài đội** hoặc **thời gian sau khi ra mắt**; chỉ đặt ngưỡng sau khi `ASSUMPTION-001` có lời giải (`QĐ-092`). Chúng là bốn chỉ số **duy nhất** đo được giá trị với người dùng — hoãn, không bỏ |
| **Đồng bộ TỰ ĐỘNG kết quả portable → trung tâm** | `QĐ-084` chốt là **không làm**. Bốn gói xuất có đủ ở cả hai hồ sơ; chỉ cờ *đã dùng* có đường nhập ngược (`PRD-REQ-095`) |

---

## 9. Open Questions

> Ba câu hỏi này **không chặn v1**. Chúng chặn đúng phiên bản mang nhãn tương ứng.

**QUESTION-009 — Thang điểm cho số ghế ≠ 4 `[v1.5]`**
- **Context**: **MISSING**. Thang điểm Tăng tốc **40/30/20/10** và các quy tắc phụ thuộc số ghế chỉ định nghĩa cho **đúng 4 đơn vị điểm** (`GR-013` C5, `GR-014`, `GR-016`, `GR-020` C6 đều gắn nhãn `[v1.5]`). Không nguồn nào cho giá trị với số ghế khác. Đây **không phải mục treo** mà là **phạm vi phiên bản** (`QĐ-007`) — nhưng v1.5 không ship được cho tới khi có câu trả lời.
- **Affected requirements**: PRD-REQ-101 · **Affected rules**: `GR-013`, `GR-014`, `GR-016`, `GR-020` · **Required decision owner**: Chủ dự án

**QUESTION-010 — Ngôi sao hy vọng khi thi đội `[v2]`**
- **Context**: **MISSING**. Đội ba người có ba lượt và chín câu nhưng **một** ngôi sao. Không nguồn chuẩn tắc nào nói ai trong đội được đặt và khoá lúc nào. `game-rules.md` chỉ có một mục `[v2]` duy nhất — `GR-010` C6, về việc một thành viên trả lời sai thì loại cả đội.
- **Affected requirements**: PRD-REQ-103 · **Affected rules**: `GR-021` · **Required decision owner**: Chủ dự án *(khi mở phạm vi v2)*
- *Xuất xứ phát hiện*: `docs/reviews/game-rules-review.md` GRR-170

**QUESTION-011 — Ai chọn người thi Câu hỏi phụ khi thi đội `[v2]`**
- **Context**: **MISSING**. Không tài liệu chuẩn tắc nào đặc tả điểm này — trong `docs/` hiện không có rule thi đội nào.
- **Affected requirements**: PRD-REQ-103 · **Affected rules**: `GR-023`, `GR-025` · **Required decision owner**: Chủ dự án *(khi mở phạm vi v2)*
- *Xuất xứ phát hiện*: `docs/reviews/game-rules-review.md` GRR-171

---

## 10. Traceability

> Viết tắt cột **Actor**: `TS` thí sinh · `TR` trainer. Cột **Journey**: `J5`, `J8` ứng với `JOURNEY-005`, `JOURNEY-008`.

| PRD requirement | Goal | Epic | Journey | Actor | Game rules | Source | Status |
|---|---|---|---|---|---|---|---|
| PRD-REQ-099 | GOAL-001 | EPIC-013 | J8 | TR | `GR-031` | `QĐ-040`, `TERM-015` | CONFIRMED |
| PRD-REQ-100 | GOAL-001 | EPIC-013 | J8 | TR, TS | `GR-031` | `QĐ-063`, `QĐ-071` | CONFIRMED |
| PRD-REQ-101 | GOAL-002 | EPIC-013 | J5 | TS | `GR-013`, `GR-014`, `GR-016`, `GR-020` | `QĐ-007` | **NEEDS CLARIFICATION** |
| PRD-REQ-102 | GOAL-002 | EPIC-014 | — | TS | `GR-010` C6 `[v2]` | `TERM-003`, `TERM-009` | CONFIRMED |
| PRD-REQ-103 | GOAL-002 | EPIC-014 | — | TS | `GR-021`, `GR-023` | `reviews/` GRR-170, GRR-171 | **NEEDS CLARIFICATION** |

### Đối chiếu ngược: Open question → Requirement bị ảnh hưởng

| Question | Loại | Requirement bị ảnh hưởng |
|---|---|---|
| QUESTION-009 | MISSING `[v1.5]` | PRD-REQ-101 |
| QUESTION-010 | MISSING `[v2]` | PRD-REQ-103 |
| QUESTION-011 | MISSING `[v2]` | PRD-REQ-103 |

---

*Hết tài liệu. Khi chủ dự án mở phạm vi v1.5 hoặc v2, chuyển mục tương ứng ngược về `docs/PRD.md` rồi mới chạy `/speckit.specify`.*
