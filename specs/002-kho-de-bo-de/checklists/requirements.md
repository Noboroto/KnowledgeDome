# Specification Quality Checklist: EPIC-002 — Kho đề và bộ đề

**Purpose**: Kiểm tính đầy đủ và chất lượng của specification trước khi chuyển sang bước lập kế hoạch
**Created**: 2026-07-30
**Feature**: [spec.md](../spec.md)

## Content Quality

- [x] Không có chi tiết triển khai (ngôn ngữ, framework, API)
- [x] Tập trung vào giá trị người dùng và nhu cầu nghiệp vụ
- [x] Viết được cho người đọc không phải lập trình viên
- [x] Đã hoàn thành mọi mục bắt buộc

## Requirement Completeness

- [ ] **Không còn marker `[NEEDS CLARIFICATION]`** — ⚠️ **2 mục mở còn lại** (`OQ-003` giới hạn số lượng trường mảng, `OQ-007` hỏng media giữa chừng — cả hai là MISSING/NEEDS CLARIFICATION, **không phải** CONFLICT). Đã chạy `/speckit-clarify` **2026-07-30** (5 câu, xem §Clarifications của spec) và đóng **5/7** mục mở ban đầu, gồm cả CONFLICT duy nhất (`OQ-001`).
- [x] Requirement kiểm thử được và không mơ hồ
- [x] Success criteria đo được
- [x] Success criteria không phụ thuộc công nghệ
- [x] Đã định nghĩa đủ acceptance scenario *(34 AC)*
- [x] Đã nhận diện edge case *(§7)*
- [x] Phạm vi có ranh giới rõ *(§8 — đặc biệt tách rõ phần "dữ liệu/cấu trúc" thuộc EPIC-002 khỏi phần "thực thi lúc chạy trận" thuộc EPIC-006 cho `PRD-REQ-047` và `PRD-REQ-092`)*
- [x] Đã nêu dependency và assumption *(§8, §11)*

## Feature Readiness

- [x] Mọi functional requirement có acceptance criteria rõ ràng *(33/33 FR đều trỏ tới ít nhất một AC, trừ FR-019 và FR-026 vốn kiểm ở EPIC-003 — đã ghi rõ)*
- [x] User scenario phủ các luồng chính *(8 user story)*
- [x] Feature đáp ứng được các outcome đo được ở Success Criteria
- [x] Không có chi tiết triển khai rò vào specification

## Sáu cổng của constitution 1.3.0

| Cổng | Kết quả |
|---|---|
| **1. Truy nguyên** | ✅ **33/33 FR** truy về `docs/` hoặc về phiên `/speckit-clarify` 2026-07-30 *(bảy FR mới — FR-001b, FR-002b, FR-004b, FR-013b→d, FR-015b — chưa vào `docs/decisions.md`, xem ghi chú dưới)*. Không trích nguồn ngoài `docs/` cho phần đã có nguồn. |
| **2. Mâu thuẫn** | ✅ **0 CONFLICT chưa phân xử.** `OQ-001` (CONFLICT duy nhất) đã đóng — "bộ đề" là entity riêng, N-N với câu hỏi (FR-014, FR-015). |
| **3. Kiểm thử được** | ✅ Mọi FR có ≥1 AC `Given/When/Then` (trừ FR-019, FR-026 — kiểm ở EPIC-003). |
| **4. Góc nhìn** | ✅ 8/8 user story có chủ ngữ là vai người dùng thật (setter, admin); không tiêu đề nào chứa tên component, endpoint hay bảng dữ liệu. |
| **5. Độc lập** | ✅ 8/8 story có "Independent Test" và độ ưu tiên. US-001 *(P1)* tự nó kiểm thử được không cần contest hay trận nào. |
| **6. Đo được** | ✅ 5 Success Criteria đều có số/tỉ lệ + điều kiện đo, không phụ thuộc công nghệ. |

## Notes

- **Đã chạy `/speckit-clarify` 2026-07-30** (5 câu, xem `## Clarifications` của spec) — đóng `OQ-001` (CONFLICT), `OQ-002`, `OQ-004`, `OQ-005`, `OQ-006`.
- **Còn 2 mục mở**: `OQ-003` (giới hạn `clues[]`/`options[]`), `OQ-007` (hỏng media giữa chừng) — cả hai thấp tác động hơn so với năm mục đã đóng, không chặn `/speckit-plan` theo đánh giá tại thời điểm chạy `/speckit-clarify`, nhưng khuyến nghị chủ dự án xác nhận trước khi thiết kế chi tiết luồng soạn câu.
- **Bảy FR mới sinh ra từ phiên clarify chưa được ghi vào `docs/decisions.md`** — chủ dự án cần chuyển các quyết định ở `## Clarifications` của spec thành `QĐ-*` chính thức, tương tự cách EPIC-001 đã làm (`QĐ-097`→`QĐ-103`), để cổng 1 (Truy nguyên) hết phụ thuộc chính spec này.
- **Hai file nguồn được yêu cầu đọc không tồn tại**: `docs/rule-traceability.md` *(file thật là `docs/traceability.md`)* và `docs/reviews/prd-review.md` *(không có trong `docs/reviews/`, vốn chỉ chứa `game-rules-*.md` và `README.md`)*. Chi tiết ở §0 của spec. Việc thiếu file thứ hai không tạo lỗ hổng truy nguyên vì `docs/reviews/**` là kho lưu, không phải nguồn — cùng ghi chú với `specs/001-xac-thuc-phan-quyen`.
