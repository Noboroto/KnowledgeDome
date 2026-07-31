# Specification Quality Checklist: EPIC-003 — Nhập/xuất và gói contest

**Purpose**: Kiểm tính đầy đủ và chất lượng của specification trước khi chuyển sang bước lập kế hoạch
**Created**: 2026-07-30
**Feature**: [spec.md](../spec.md)

## Content Quality

- [x] Không có chi tiết triển khai (ngôn ngữ, framework, API)
- [x] Tập trung vào giá trị người dùng và nhu cầu nghiệp vụ
- [x] Viết được cho người đọc không phải lập trình viên
- [x] Đã hoàn thành mọi mục bắt buộc

## Requirement Completeness

- [ ] **Không còn marker `[NEEDS CLARIFICATION]`** — ⚠️ **2 mục mở còn lại** (`OQ-006` cụm mật khẩu mới hay dùng lại khi xuất lại danh sách, `OQ-007` gói với kho đề rỗng — cả hai MISSING, không phải CONFLICT). Đã chạy `/speckit-clarify` **2026-07-30** (5 câu, xem `## Clarifications` của spec) và đóng **5/7** mục mở ban đầu.
- [x] Requirement kiểm thử được và không mơ hồ
- [x] Success criteria đo được
- [x] Success criteria không phụ thuộc công nghệ
- [x] Đã định nghĩa đủ acceptance scenario *(28 AC)*
- [x] Đã nhận diện edge case *(§7)*
- [x] Phạm vi có ranh giới rõ *(§8 — đặc biệt tách rõ `X1`/`X3` thuộc EPIC-003 khỏi `X2`/`X4` thuộc EPIC-010, và tách vế "xuất kèm định nghĩa vai" khỏi vế "định nghĩa vai" thuộc EPIC-001)*
- [x] Đã nêu dependency và assumption *(§8, §11)*

## Feature Readiness

- [x] Mọi functional requirement có acceptance criteria rõ ràng *(31/31 FR đều trỏ tới ít nhất một AC)*
- [x] User scenario phủ các luồng chính *(7 user story)*
- [x] Feature đáp ứng được các outcome đo được ở Success Criteria
- [x] Không có chi tiết triển khai rò vào specification

## Sáu cổng của constitution 1.3.0

| Cổng | Kết quả |
|---|---|
| **1. Truy nguyên** | ✅ **31/31 FR** truy về `docs/` hoặc về phiên `/speckit-clarify` 2026-07-30 *(bốn FR mới — FR-006b, FR-019b, FR-029, và phần bổ sung của FR-011/FR-022 — chưa vào `docs/decisions.md`, xem ghi chú dưới)*. Không trích nguồn ngoài `docs/` cho phần đã có nguồn. |
| **2. Mâu thuẫn** | ✅ **0 CONFLICT.** Không phát hiện mâu thuẫn nào giữa PRD, game rules và decisions trong phạm vi đã đọc; phiên clarify cũng không phát sinh CONFLICT mới. |
| **3. Kiểm thử được** | ✅ Mọi FR có ≥1 AC `Given/When/Then`. |
| **4. Góc nhìn** | ✅ 7/7 user story có chủ ngữ là vai người dùng thật (admin, setter); không tiêu đề nào chứa tên component, endpoint hay bảng dữ liệu. |
| **5. Độc lập** | ✅ 7/7 story có "Independent Test" và độ ưu tiên. US-001 *(P1)* tự nó là MVP dùng được — xuất/nhập một gói trọn vẹn. |
| **6. Đo được** | ✅ 5 Success Criteria đều có số/tỉ lệ + điều kiện đo, không phụ thuộc công nghệ. |

## Notes

- **Đã chạy `/speckit-clarify` 2026-07-30** (5 câu, xem `## Clarifications` của spec) — đóng `OQ-001` (thiếu media → nhập một phần, đánh dấu), `OQ-002` (biên đóng cho ngưỡng media), `OQ-003` (ngắt giữa chừng → tất cả-hoặc-không, thao tác server-side đơn), `OQ-004` (không giới hạn số lần thử cụm mật khẩu ở tầng nghiệp vụ), `OQ-005` (đồng thời hai admin nhập bản kê → không cần phân xử, phép hợp một chiều an toàn).
- **Còn 2 mục mở**: `OQ-006` (cụm mật khẩu mới hay dùng lại khi xuất lại danh sách người tham gia), `OQ-007` (gói với kho đề rỗng) — cả hai thấp tác động, không chặn `/speckit-plan` theo đánh giá tại thời điểm chạy `/speckit-clarify`, nhưng khuyến nghị chủ dự án xác nhận trước khi thiết kế chi tiết.
- **Các FR mới/mở rộng từ phiên clarify chưa được ghi vào `docs/decisions.md`** — chủ dự án cần chuyển các quyết định ở `## Clarifications` của spec thành `QĐ-*` chính thức, tương tự cách EPIC-001 và EPIC-002 đã làm, để cổng 1 (Truy nguyên) hết phụ thuộc chính spec này.
- **Hai file nguồn được yêu cầu đọc không tồn tại**: `docs/rule-traceability.md` *(file thật là `docs/traceability.md`)* và `docs/reviews/prd-review.md` *(không có trong `docs/reviews/`, vốn chỉ chứa `game-rules-*.md` và `README.md`)*. Chi tiết ở §0 của spec. Việc thiếu file thứ hai không tạo lỗ hổng truy nguyên vì `docs/reviews/**` là kho lưu, không phải nguồn — cùng ghi chú với `specs/001-xac-thuc-phan-quyen` và `specs/002-kho-de-bo-de`.
