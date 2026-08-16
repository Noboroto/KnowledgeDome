# Specification Quality Checklist: EPIC-001 — Xác thực và phân quyền

**Purpose**: Kiểm tính đầy đủ và chất lượng của specification trước khi chuyển sang bước lập kế hoạch
**Created**: 2026-07-29
**Feature**: [spec.md](../spec.md)

## Content Quality

- [x] Không có chi tiết triển khai (ngôn ngữ, framework, API)
- [x] Tập trung vào giá trị người dùng và nhu cầu nghiệp vụ
- [x] Viết được cho người đọc không phải lập trình viên
- [x] Đã hoàn thành mọi mục bắt buộc

## Requirement Completeness

- [x] **Không còn marker `[NEEDS CLARIFICATION]`** — ✅ **0 marker.** Ba marker gốc đóng bằng `QĐ-093`, `QĐ-095`, `QĐ-065`; ngày **2026-07-30** phân xử thêm **7** ca (§1b). Marker cuối — tranh chấp quyền điều khiển **TRỐNG** ở FR-020b — đóng bằng **`QĐ-096`**, đã ghi vào `docs/decisions.md` §A.
- [x] Requirement kiểm thử được và không mơ hồ
- [x] Success criteria đo được
- [x] Success criteria không phụ thuộc công nghệ
- [x] Đã định nghĩa đủ acceptance scenario *(62 AC)*
- [x] Đã nhận diện edge case *(§7)*
- [x] Phạm vi có ranh giới rõ *(§8)*
- [x] Đã nêu dependency và assumption *(§8, §11)*

## Feature Readiness

- [x] Mọi functional requirement có acceptance criteria rõ ràng *(43/43 FR đều trỏ tới ít nhất một AC)*
- [x] User scenario phủ các luồng chính
- [x] Feature đáp ứng được các outcome đo được ở Success Criteria
- [x] Không có chi tiết triển khai rò vào specification

## Sáu cổng của constitution 1.3.0

| Cổng | Kết quả |
|---|---|
| **1. Truy nguyên** | ✅ **43/43 FR** truy về `docs/` *(PRD-REQ, GR, NFR, QĐ, TERM)*. Bảy phán quyết ngày **2026-07-30** đã vào sổ thành `QĐ-097`→`QĐ-103`, nên bốn FR mới *(FR-020b, FR-021a, FR-021b, FR-030b)* và các vế mới của FR-015/FR-021/FR-030/FR-035 đều truy về `docs/`, **không** về spec này. Không trích nguồn ngoài `docs/`. |
| **2. Mâu thuẫn** | ✅ **0 CONFLICT.** Không FR nào phụ thuộc một mâu thuẫn chưa phân xử. |
| **3. Kiểm thử được** | ✅ Mọi FR có ≥1 AC `Given/When/Then`. Không còn tính từ không đo được. |
| **4. Góc nhìn** | ✅ 7/7 user story có chủ ngữ là vai người dùng thật; không tiêu đề nào chứa tên component, endpoint hay bảng dữ liệu. |
| **5. Độc lập** | ✅ 7/7 story có "Independent Test" và độ ưu tiên. US-001 *(P1)* tự nó là MVP dùng được. |
| **6. Đo được** | ✅ 8 Success Criteria đều có số + đơn vị + điều kiện đo, không phụ thuộc công nghệ. |


## Notes

Không còn mục nào chờ phân xử. Mọi requirement của feature này truy nguyên được về `docs/`.
