# Specification Quality Checklist: Điều khiển và can thiệp của admin

**Purpose**: Validate specification completeness and quality before proceeding to planning
**Created**: 2026-08-03
**Feature**: [spec.md](../spec.md)

## Content Quality

- [x] No implementation details (languages, frameworks, APIs)
- [x] Focused on user value and business needs
- [x] Written for non-technical stakeholders
- [x] All mandatory sections completed

## Requirement Completeness

- [x] No [NEEDS CLARIFICATION] markers remain
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

## Sáu cổng của `.specify/memory/constitution.md` 1.4.0

- [x] **Cổng 1 — truy nguyên**: ✅ **ĐÃ ĐÓNG (2026-08-03)**. 124/132 FR truy thẳng về `docs/` từ đầu; 8 FR còn lại nay đã có cả `QĐ` **lẫn** tài liệu đặc tả khớp theo — `QĐ-108`, `QĐ-109`, `QĐ-110` trong `docs/decisions.md`, và `OQ-002` vốn đã được `QĐ-072` khai từ trước. Ba tài liệu đã sửa xong: `docs/PRD.md` *(`PRD-REQ-020`, `052`, `054`, `056`)* · `docs/game-rules.md` *(`GR-006`, `GR-015`, `GR-035`, hai bảng dùng chung, nguyên tắc nền 22)* · `docs/game-state-machine.md` *(`STATE-035`, `EVENT-016`, `T-036`, §Invalid transitions, `INV-015`)*. `FR-032a`/`FR-032b` đã **chuyển sang `specs/006`** *(FR-070a, FR-070b)* theo phân vai engine-vs-màn-điều-khiển. Không FR nào trích nguồn ngoài `docs/`.
- [x] **Cổng 2 — mâu thuẫn**: `CONFLICT` duy nhất (`OQ-001`) đã ghi đúng định dạng và **đã được chủ dự án phân xử** bằng mô hình hai trục. Không FR nào còn phụ thuộc một mâu thuẫn chưa phân xử.
- [x] **Cổng 3 — kiểm thử được**: 132/132 FR có ít nhất một acceptance scenario `Given/When/Then`; không còn tính từ không đo được.
- [x] **Cổng 4 — góc nhìn**: 19/19 user story có chủ ngữ là vai người dùng (ACTOR-001 admin); không tiêu đề nào chứa tên component, endpoint hay bảng dữ liệu.
- [x] **Cổng 5 — độc lập**: 19/19 story có "Independent Test" và độ ưu tiên. Các story P1 phủ trọn vòng đời một câu — bốn mốc bấm, phán quyết, màn chấm, chốt câu, ba cửa ra, ba hạng cảnh báo, mở/đóng đáp án, bàn cờ, hàng đợi — nên tự chúng đã là một MVP điều khiển dùng được.
- [x] **Cổng 6 — đo được**: 14 mục Success Criteria đều có số và đơn vị, và không mục nào phụ thuộc công nghệ.

## Kiểm riêng theo yêu cầu của lệnh gọi

- [x] Mỗi user story có đủ 10 trường: ID · Title · Actor · Intent · User value · Priority · Priority rationale · Independent test · Related PRD requirements · Related game rules · Related journey.
- [x] Mỗi acceptance scenario có ID, Related user story, Related functional requirements, Related game rules, và Given/When/Then đầy đủ.
- [x] Mỗi outcome trong bảng quyết định của `GR-026` → `GR-034` có ít nhất một acceptance scenario; các ca thuộc epic khác được ghi rõ spec sở hữu trong §Bản đồ phủ bảng quyết định.
- [x] Edge cases liệt kê đủ tám nhóm được yêu cầu: boundary · invalid state · repeated action · stale state · duplicate event · partial failure · conflicting rule · missing source behavior.
- [x] Không tự thêm game behavior; không tự giải quyết `CONFLICT`; không tự chọn validation order chưa được quy định; không tự tạo error code; không đổi nghĩa `GR-NNN` nào.
- [x] §Out of Scope ghi rõ requirement và epic không thuộc feature này, kèm lý do cho ba requirement khai nhiều epic (PRD-REQ-030/031, PRD-REQ-072, PRD-REQ-113).

## Notes

**Cập nhật sau phiên làm rõ 2026-08-03** — 26/28 → **27/28** mục đạt.

**Hai mục đổi sang ĐẠT**:

1. *"No [NEEDS CLARIFICATION] markers remain"* — cả bốn `OQ` đã được chủ dự án phân xử; **0** marker còn trong spec.
2. **Cổng 2 — mâu thuẫn** — `CONFLICT` duy nhất (`OQ-001`) đã phân xử bằng mô hình hai trục; không FR nào còn phụ thuộc một mâu thuẫn treo.

**Cổng 1 — truy nguyên: ✅ ĐÃ ĐÓNG (2026-08-03).** Mục này từng là một REGRESSION **được dự đoán trước** của việc phân xử: bốn quyết định phát biểu trong phiên làm rõ, mà theo Nguyên tắc I thì chỉ nội dung trong `docs/` mới là nguồn. Nay `docs/` đã bắt kịp toàn bộ.

| `OQ` | Quyết định | Tài liệu đã sửa |
|---|---|---|
| `OQ-001` | `QĐ-108` — tầng dialog xác định bằng **cách thoát**; *bắt lý do* là trục riêng | `PRD-REQ-056` *(viết lại theo hai trục, đổi tiêu đề thành "bốn loại phản hồi")* · `STATE-035` *(bảng ba tầng; điều chỉnh điểm **gỡ khỏi** hạng phá huỷ, khai tầng riêng; §Cấm thêm quy tắc phím tắt)* |
| `OQ-002` | `QĐ-072` *(đã có từ trước)* — cảnh báo lệch luật **dùng lại** tầng Yes/No thường | `PRD-REQ-056` *(bổ sung cảnh báo lệch luật vào bảng tầng và một mệnh đề nói rõ nó ép được)* |
| `OQ-003` | `QĐ-109` — **cửa sổ giữ bản tới muộn** `(hạn chót, hạn chót + padding]` | `GR-035` **C6b, C6c mới** · `GR-015` **C7 mới**, C5, C6, §Đồng thời · bảng §*Kênh trả lời* · bảng §*Bản gửi quá hạn* · nguyên tắc nền **22** · `PRD-REQ-020` *(`padding` vào cấu hình luật, mặc định 5 giây)* · `PRD-REQ-052`, `PRD-REQ-054` · `T-036`, `EVENT-012`/`013`, §Invalid transitions, `INV-015`, ghi chú Sơ đồ 2 · `QĐ-030` mang ghi chú bị `QĐ-109` làm chính xác hơn |
| `OQ-004` | `QĐ-110` — Khởi động **không có mốc cắt riêng** | `GR-006` §Điều kiện, C2, C3, §Thứ tự đánh giá, §Đồng thời, §Ví dụ, §Nguồn · **`EVENT-016` viết lại thành bí danh**, vai trò mốc cắt đã gỡ · `QĐ-027` mang ghi chú bị `QĐ-110` thay một vế |

**Mức chặn thực tế: 0.** Cả 132 FR đều qua Cổng 1. Hai cụm từng treo — **dialog** *(FR-045, FR-060, FR-061, FR-061a, FR-063a)* và **mốc mở nút chấm** *(FR-009, FR-033, FR-035)* — nay đã có nguồn đầy đủ. Quy tắc **cửa sổ giữ bản tới muộn** thuộc `specs/006` *(FR-070a, FR-070b)* và cũng đã có nguồn.

**Sẵn sàng cho `/speckit-plan`** — không còn FR nào bị chặn.

**Ghi chú quy trình đáng giữ.** `docs/decisions.md` **không** nằm trong danh sách đọc của lệnh `/speckit-specify` đã dùng, và đó là lý do `OQ-002` bị nêu như một câu hỏi mở dù đã có `QĐ-072`. Lần sinh spec sau nên đọc `decisions.md` **trước khi** kết luận một chỗ là chưa quy định — nó là nơi duy nhất ghi *vì sao*, nên cũng là nơi dễ chứa sẵn câu trả lời nhất.
