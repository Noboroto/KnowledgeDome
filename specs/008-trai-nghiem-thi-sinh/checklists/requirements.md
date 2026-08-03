# Specification Quality Checklist: Trải nghiệm thí sinh (EPIC-008)

**Purpose**: Kiểm tính đầy đủ và chất lượng của specification trước khi chuyển sang giai đoạn plan
**Created**: 2026-08-03
**Last validated**: 2026-08-03 *(sau phiên `/speckit-clarify`)*
**Feature**: [spec.md](../spec.md)

## Content Quality

- [x] Không có chi tiết hiện thực (ngôn ngữ, framework, API)
- [x] Tập trung vào giá trị người dùng và nhu cầu nghiệp vụ
- [x] Viết cho người đọc không chuyên kỹ thuật
- [x] Mọi mục bắt buộc đã hoàn thành

## Requirement Completeness

- [x] Không còn marker `[NEEDS CLARIFICATION]` — **0 marker**; cả sáu mục đã được chủ dự án phân xử
- [x] Requirement kiểm thử được và không mơ hồ
- [x] Success criteria đo được
- [x] Success criteria không phụ thuộc công nghệ
- [x] Mọi acceptance scenario đã được định nghĩa (108 AC)
- [x] Edge case đã được nhận diện (8 nhóm theo lệnh gọi)
- [x] Phạm vi được khoanh rõ (§Phạm vi, §Out of Scope)
- [x] Dependency và assumption đã nêu (§Assumptions)

## Feature Readiness

- [x] Mọi functional requirement có tiêu chí nghiệm thu rõ ràng — 97/97 FR đều trỏ tới ít nhất một AC
- [x] User scenario phủ các luồng chính — 12 user story, mỗi story một lát cắt dọc
- [x] Feature đạt được các kết quả đo được ở Success Criteria — **0 CONFLICT còn tồn đọng**
- [x] Không có chi tiết hiện thực rò vào specification

## Sáu cổng chất lượng của constitution 1.4.0

| # | Cổng | Kết quả |
|---|---|---|
| 1 | **Truy nguyên** | ✅ 97/97 FR truy về `docs/` hoặc nằm ở §Assumptions. **Cả 6 phán quyết đã vào `docs/` ngày 2026-08-03** — `QĐ-111` → `QĐ-114` cùng các sửa đổi kéo theo |
| 2 | **Mâu thuẫn** | ✅ **0 CONFLICT tồn đọng** — ba `CONFLICT` trong `docs/` và hai `XCONF` giữa các spec đều đã được chủ dự án phân xử; `specs/006` đã hợp nhất |
| 3 | **Kiểm thử được** | ✅ Mọi FR có ít nhất một AC `Given/When/Then`; không còn tính từ không đo được |
| 4 | **Góc nhìn** | ✅ 12/12 story có chủ ngữ là vai người dùng *(ACTOR-003 thí sinh)* |
| 5 | **Độc lập** | ✅ 12/12 story có §Independent Test và độ ưu tiên; 10 story P1, 2 story P2 |
| 6 | **Đo được** | ✅ 18 SC đều có số và đơn vị, không phụ thuộc công nghệ |

## Kết quả phiên `/speckit-clarify` 2026-08-03

**5 câu hỏi, 5 phán quyết.** Cả ba `CONFLICT` và hai trong ba `NEEDS CLARIFICATION` đã được giải.

| Mã | Phán quyết | FR / AC chịu ảnh hưởng |
|---|---|---|
| `CONFLICT-001` | Khoá nút chuông gắn với **phán quyết**, không gắn với câu cũng không gắn với vòng. *Huỷ kết quả* hoặc *bấm No* ⇒ nút **mở lại** | FR-003, FR-004, FR-004a, FR-004b · AC-009a, AC-009b |
| `CONFLICT-002` | Ẩn vs mờ phân định bằng **control có nghĩa ở pha này không**. Mất nghĩa ⇒ ẩn; bị luật cấm ⇒ **mờ kèm nhãn** | FR-079, FR-086 · AC-089, AC-089a, AC-096 |
| `CONFLICT-003` | **Nguyên tắc hai trục**: giành lượt lấy **người đầu tiên**, đáp án lấy **bản cuối cùng**. `GR-020` áp nhầm trục | FR-011, FR-013, FR-013a, FR-021, FR-021a · AC-024a |
| `OQ-001` | Cú **chọn hộ** gói câu của admin là **giá trị khởi tạo**, không phải cú chốt | FR-057, FR-058 · AC-063 |
| `OQ-002` | Outcome của tín hiệu **báo về máy phát** bằng **nhãn trên nút**, không dùng lớp phủ riêng | FR-005a, FR-086 · AC-005a |
| `OQ-003` | Banner tạm dừng **CHE TOÀN BỘ màn thí sinh** — ngoại lệ tường minh và duy nhất của `PRD-REQ-067`; dữ liệu bên dưới vẫn chảy *(che ≠ đổi)* | FR-029, FR-076a · AC-090a |

### Audit chéo 8 spec × PRD — phiên 2026-08-03 *(lượt hai)*

Hai mâu thuẫn **giữa các spec** phát hiện và đã hợp nhất; `specs/006` đã sửa.

| Mã | Nội dung | Trạng thái |
|---|---|---|
| `XCONF-1` | `specs/006` FR-097 *(bản đầu tiên)* vs `CONFLICT-003` *(bản cuối)* | ✅ Hợp nhất — `specs/006` FR-097, AC-170 đã sửa |
| `XCONF-2` | `specs/006` FR-077 *(một tín hiệu cho cả vòng)* vs `CONFLICT-001` *(hạn mức theo phán quyết)* | ✅ Hợp nhất — `specs/006` FR-077, AC-118 đã sửa; AC-118a mới |
| `XCONF-3` | `GR-032` *"chuỗi dừng khi hàng đợi cạn"* mất bảo đảm | ✅ Không phải mâu thuẫn — `PRD-REQ-113` đã khai *"không có trần tổng"*; chỉ sửa câu chữ `docs/` |

Hai lỗ **truy nguyên** *(không phải mâu thuẫn hành vi)*: `PRD-REQ-074` khai `EPIC-001` mà không spec nào trích *(nội dung có phủ ở `specs/001` FR-033, chỉ sai neo)* · `PRD-REQ-098` khai `EPIC-003` mà `specs/003` không nhận *(vế còn lại thuộc EPIC-012, chưa có spec)*.

Đã kiểm và **sạch**: `PRD-REQ-025`, `105`, `113`, `114` — chia theo facet, không chồng lấn. Sáu orphan còn lại thuộc EPIC-009/010/011, chưa có spec.

**Giới hạn**: chưa làm diff ngữ nghĩa từng cặp trên toàn bộ ~7.300 dòng của 8 spec.

## `docs/` đã cập nhật — 2026-08-03 ✅

Cả sáu phán quyết đã vào `docs/`, nên **Cổng 1 thoả**. Bộ thay đổi:

| Nguồn | Đã làm |
|---|---|
| `docs/decisions.md` | **5 `QĐ` mới**: `QĐ-111` *(khoá chuông gắn với phán quyết)* · `QĐ-112` *(ẩn khi mất nghĩa, mờ kèm nhãn khi bị luật cấm)* · `QĐ-113` *(nguyên tắc hai trục)* · `QĐ-114` *(báo outcome về máy phát)* · `QĐ-115` *(banner che toàn bộ màn thí sinh)*. `QĐ-023` mang ghi chú bị `QĐ-111` thay một vế |
| `docs/PRD.md` | `PRD-REQ-064` · `PRD-REQ-067` *(ngoại lệ banner)* · `PRD-REQ-072` *(6 nhãn)* · §9.4 *(ghi chú hai trục: hạng phản hồi ≠ kết cục hiển thị)* · §13.3 bảng Về đích · `PRD-REQ-065` bổ sung `GR-018`, `GR-020` vào Related game rules · version **2.4.0 → 2.5.0** |
| `docs/game-rules.md` | Nguyên tắc nền **16** và **21** · `GR-003` C3, C5, C6 · `GR-009` §Điều kiện, C6b, C8, C9-C10 · `GR-017` C3 · `GR-018` §Điều kiện · `GR-020` §Điều kiện, C4, §Nguồn · `GR-032` §Điều kiện, §Kích hoạt tay · `GR-034` C5, C6, §Bấm trùng, §Nguồn |
| `docs/game-state-machine.md` | Sơ đồ 3b `S013` · `STATE-013` §Cấm · `STATE-028` *(§Ra tách hai nhánh, thêm §Hiển thị)* · `STATE-040` *(§Mô tả, §Ra, §Nguồn + ghi chú "che ≠ đổi")* · **`EVENT-040` §Ngoại lệ đã GỠ** · §Invalid transitions *(header + 2 dòng)* · `T-063` |
| `docs/glossary.md` | `TERM` cướp quyền — bỏ vế *"người cướp tính bản đầu tiên"* |
| `CLAUDE.md`, `docs/README.md` | Dải quyết định `QĐ-001` → **`QĐ-114`** |
| `specs/006` | ✅ Hợp nhất — FR-077, FR-097, AC-118, AC-170 sửa; AC-118a mới; ba `XCONF` vào §Open Questions |

**Còn nợ, ngoài phạm vi phiên này** — `specs/001` sửa neo FR-033 để trích thêm `PRD-REQ-074` *(lỗ truy nguyên, không đổi hành vi)*; và bộ sửa `docs/` của `specs/007` `OQ-003`/`OQ-004` *(`QĐ-108`→`QĐ-110` đã ghi nhưng `PRD-REQ-052`, `054`, `056`, `GR-006`, `GR-015`, `GR-035`, `STATE-035`, `EVENT-016` chưa sửa theo)*.

## Notes

### Độ trễ tài liệu đã biết — không phải phát hiện của spec này

`GR-006` §Điều kiện vẫn khai *"mốc cắt = admin bấm công bố đáp án"* trong khi `QĐ-110` đã **bỏ hẳn** mốc đó; bảng §*Kênh trả lời* vẫn nói nút chấm khoá *"tới khi hết giờ"* trong khi `QĐ-109` đã đặt mốc là `hạn chót + padding`. Cả hai đã được `specs/007` ghi nhận ở `OQ-003`, `OQ-004`. Spec này dùng **quyết định sau** làm chuẩn.

### Kết luận

Spec **qua đủ 6/6 cổng**. **0 `NEEDS CLARIFICATION`, 0 `CONFLICT`, 0 `XCONF` tồn đọng**, và `docs/` đã bắt kịp toàn bộ phán quyết.

**Sẵn sàng cho `/speckit-plan`** — không còn FR nào bị chặn.
