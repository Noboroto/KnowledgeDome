# Hợp nhất requirements corpus — báo cáo

**Ngày**: 2026-08-06, rà lại 2026-08-16 · **Nhánh**: `remaster`

Corpus được viết lại tại chỗ thành **current-state requirements**: mỗi tài liệu active chỉ còn behavior đang có hiệu lực, mỗi section đọc độc lập được, và reference chỉ còn vai trò truy nguồn.

**Quy mô**: 20.330 → 16.340 dòng trên bốn tài liệu canonical và 12 feature spec, cộng 12 file checklist. `docs/decisions.md` **không** bị đụng tới — nó là sổ provenance và được phép giữ lịch sử.

---

## Files changed

| File | Trước | Sau |
|---|---:|---:|
| `docs/PRD.md` | 2.041 | 1.983 |
| `docs/game-rules.md` | 1.953 | 1.951 |
| `docs/game-state-machine.md` | 1.722 | 1.713 |
| `docs/glossary.md` | 679 | 679 *(không đổi — đã sạch)* |
| `docs/permissions.md` | 230 | 230 *(không đổi — đã sạch)* |
| `docs/traceability.md` | 160 | 160 *(không đổi — đã sạch)* |
| `specs/001-xac-thuc-phan-quyen` | 823 | 820 |
| `specs/002-kho-de-bo-de` | 612 | 593 |
| `specs/003-nhap-xuat-goi-contest` | 556 | 537 |
| `specs/004-contest-builder-luat` | 294 | 279 |
| `specs/005-phong-thi-vong-doi-tran` | 466 | 453 |
| `specs/006-game-engine-luat-thi-dau` | 1.787 | 1.730 |
| `specs/007-dieu-khien-can-thiep-admin` | 1.538 | 1.470 |
| `specs/008-trai-nghiem-thi-sinh` | 1.272 | 1.145 |
| `specs/009-trinh-dien-khan-gia-mc-overlay` | 1.092 | 958 |
| `specs/010-sau-tran-ket-qua-thong-ke` | 905 | 765 |
| `specs/011-du-lieu-ca-nhan-quyen-rieng-tu` | 835 | 630 |
| `specs/012-van-hanh-hai-ho-so-trien-khai` | 691 | 634 |
| 12 × `specs/*/checklists/requirements.md` | 891 | 456 |

---

## Superseded decisions removed

### Quyết định đã bị phủ định, nay đã bị xoá khỏi tài liệu active

| Quyết định cũ bị xoá | Behavior hiện hành thay thế | Nơi từng nằm |
|---|---|---|
| Vòng VCNV có **một** giá trị cấu hình thời gian dùng chung cho ba cửa sổ | **Hai** knob độc lập: *(a)* thời gian suy nghĩ mỗi câu *(chung cho hàng ngang và ô trung tâm)* · *(b)* cửa sổ giải Chướng ngại vật sau gợi ý cuối | `specs/006` `OQ-001` + `AC-131a` |
| Người cướp quyền Về đích tính **bản đầu tiên** | Tính **bản cuối cùng** — nguyên tắc hai trục: giành lượt lấy người đầu, đáp án lấy bản cuối | `specs/006` `XCONF-1`, `specs/008` |
| Mỗi ghế phát **một** tín hiệu *"Mở chướng ngại vật"* cho cả vòng | Hạn mức gắn với **phán quyết**, không với vòng | `specs/006` `XCONF-2`, `specs/008` |
| Ghế đã chấm Sai ⇒ **ẩn** nút chuông | Nút **vẫn render**, ở trạng thái khoá **kèm nhãn** nêu lý do — sáu ca, sáu nhãn | `specs/008` |
| Mốc cắt *"công bố đáp án"* là hạn nhận bài ở Khởi động | `hạn chót` của câu là hạn nhận bài **duy nhất**; mốc cắt riêng **không tồn tại** | `docs/game-state-machine.md` `EVENT-016`, `specs/007` `OQ-004` |
| Nút chấm ở kênh gõ khoá **tới hết giờ** | Khoá tới **`hạn chót + padding`**; riêng Khởi động mở ngay tại `hạn chót` | `docs/PRD.md` §16 bảng ba kênh |
| Ba hạng phản hồi; cảnh báo lệch luật là hạng riêng | **Bốn** loại, ba tầng dialog phân biệt bằng **cách thoát**; cảnh báo lệch luật **dùng lại** tầng Yes/No thường | `specs/007` Key Entities + `SC-008` |
| Banner tạm dừng phủ **cả màn MC** | Banner **không** phủ màn MC | `specs/009` |
| Đáp án Chướng ngại vật chỉ lộ cho **viewer** | Lộ tới **cả** màn khán giả lẫn lớp phủ, cùng lúc cùng điều kiện | `specs/009` |
| Engine đẩy đáp án **đè lên** cú đóng hiển thị bằng tay | **Thao tác tay thắng** cờ tự động ở cả hai chiều | `specs/009` |
| `PRD-REQ-084` — hệ thống **hiện** disclaimer bản quyền | **Không** hiện ở bất cứ đâu *(`NON-GOAL-020`)*; viết thành một FR nghịch bình thường | `specs/011` `US-007`, `FR-047`→`FR-050` |
| Chuỗi kích hoạt tay **dừng khi hàng đợi cạn** | **Không có trần tổng** — chuỗi bị chặn bởi **ý chí của người**, không bởi ngưỡng máy | `docs/game-state-machine.md` `T-098`, `specs/006` `XCONF-3` |
| `specs/004` `FR-033` chặn số ghế **≠ 4** | Chặn số ghế **> 4**; dưới 4 chạy bằng **ghế bỏ thi** | `specs/004` |
| Nhật ký chỉ ghi lần xem đáp án **có trả** | Ghi **mọi** yêu cầu kèm kết quả, gồm cả lần **bị từ chối** | `docs/game-rules.md` `GR-037` |

### Khối lịch sử đã cắt khỏi tài liệu active

- **`docs/PRD.md` ô Version** — changelog ~9.000 ký tự mô tả mọi thay đổi behavior từ bản 2.2.1 → 3.4.0. Nay chỉ còn số phiên bản.
- **`docs/PRD.md` §21** — bảng tám câu hỏi **đã đóng** của bản 2.1.0 kèm ngày phân xử. Bốn `CONFLICT` phát hiện trong lúc hợp nhất từng nằm ở đây, nay đã phân xử *(`QĐ-149`→`QĐ-152`)*; mục còn lại một câu trạng thái.
- **`docs/PRD.md` §22.3** — bảng 45 hàng *"Quyết định mới → Requirement chịu ảnh hưởng"*, mọi hàng mang động từ delta *(mới · rút · viết lại · bị thay · bị tách)*. Provenance thật đã nằm ở cột `Source` của §22 và của từng requirement.
- **`docs/PRD.md` §Quy ước bảo trì** — vế *"dãy số được đánh lại cho liền"* bị `QĐ-146` phủ nhận; nay khai mã là **định danh vĩnh viễn**.
- **`docs/game-state-machine.md` `EVENT-016`** — một event tồn tại **chỉ để** kể lịch sử của chính nó *("KHÔNG còn là sự kiện riêng", "Vai trò cũ đã bị GỠ", "Vì sao gỡ")*. Xoá trọn; hệ quả còn hiệu lực *(một câu có tối đa bốn mốc bấm)* chuyển sang §C. **0** `T-*` hay `STATE-*` nào tham chiếu nó.
- **11 phần `## Clarifications`** trong 11 spec — tổng ~257 dòng Q/A. Mọi quyết định đã được xác minh là đã nằm trong FR/AC/US trước khi xoá; chỗ nào chưa đủ thì viết vào FR/AC **trước**.
- **9 mục `## Open Questions`** chứa `✅ ĐÃ PHÂN XỬ` / `❌ BỎ HẲN` / `ĐÃ ĐÓNG` / bảng *"Tiến độ phiên làm rõ"* / khối `<details>` *"nội dung mâu thuẫn trước khi phân xử"*.
- **12 file checklist** — 891 → 456 dòng; cắt toàn bộ nhật ký phiên, giữ các cổng chất lượng.
- **Nhãn roadmap trong tài liệu luật** — `[v1.5]` và `[v2]` ở `GR-010` C6 *(thi đội)*, `GR-013` C5, `GR-014`, `GR-016`, `GR-020`. `game-rules.md` chỉ mô tả luật **đang có**.
- **Ghi chú thiết kế tương lai** — *"Đường thoát nếu đổi ý: đặt `tieRule` ở mức 10 ms"* mô tả một trường cấu hình chưa tồn tại.

---

## Bug thật phát hiện trong lúc hợp nhất

Bảy chỗ **không phải** dấu vết văn bản mà là lỗi nội dung, phát hiện nhờ đối chiếu chéo:

| # | Lỗi | Bằng chứng | Đã sửa thành |
|---|---|---|---|
| 1 | **`T-005` đi thẳng vòng → vòng** (`STATE-002` → `STATE-003`) | **Năm** phát biểu ngược lại: Sơ đồ 1 `S002 --> S001` · `EVENT-002` §Hợp lệ ở *("chỉ ở `STATE-001`")* · `STATE-001` §Vào · `GR-030` C4 · `STATE-003` §Vào | `STATE-002` → **`STATE-001`** |
| 2 | **`STATE-004` §Ra trỏ vào `STATE-007`** (TIE_BREAK) | `T-008` khai `STATE-004` → `STATE-001`; Sơ đồ 1 `S004 --> S001`; `GR-012` C1 | Cả ba nhánh về **`STATE-001`** |
| 3 | **`AC-131a` kiểm thử behavior đã bị phủ định** — *"cả ba cửa sổ đều 20 giây"* | `FR-083a` cùng file khai **hai knob độc lập** | Viết lại: (a) đổi, (b) giữ nguyên |
| 4 | **`T-098` mâu thuẫn `GR-032`** — *"chuỗi dừng khi hàng đợi cạn"* | `GR-032` §Kích hoạt tay và `PRD-REQ-113` đều khai *"không có trần tổng"* | Không có trần tổng |
| 5 | **`specs/004` `FR-033` vs `FR-030`** — chặn *"≠ 4"* vs chặn *"> 4"* | `QĐ-105`, `PRD-REQ-114` | Chặn **> 4** |
| 6 | **`AC-069d` ghi 51 khe** âm thanh | `FR-074d`, `SC-015c`, Key Entities đều ghi **52**; `11+14+8+15+4 = 52` | **52** |
| 7 | **Key Entities ghi 5 ca khoá** theo luật chơi | `FR-086` và `SC-014` khai **sáu** ca | **Sáu**, bổ sung ca *tín hiệu đã trơ* |

Cộng bốn lỗi đếm trong `docs/PRD.md`: số quyết định *(145 → 148)*, số `T-*` *(100 → 102)*, số `TERM-*` *(60 → 63)*, số requirement *(110 → 109 sau khi `PRD-REQ-084` rút, rồi → 110 khi `PRD-REQ-117` ra đời)*. Và một bảng ở §9.4 bị một blockquote chèn vào giữa, cắt bảng làm hai khiến ba hàng cuối mất header.

---

## Duplicate statements removed

- **`specs/007` `SC-008`** — trùng `SC-014` về cùng phép đo, nhưng khai *"ba hạng"* trong khi `SC-014` khai *"bốn loại"*. Giữ `SC-014`.
- **`docs/PRD.md` `PRD-REQ-056`** — bảng ba tầng dialog trùng nguyên vẹn `STATE-035`. PRD chỉ được mô tả outcome cấp sản phẩm ⇒ thay bằng văn xuôi, ma trận đầy đủ ở `STATE-035`.
- **`specs/008`, `specs/009` Edge Cases** — `CONFLICT-001/002/003` và `OQ-001`→`OQ-007` là bản sao thứ hai của hành vi đã có trong FR.
- **`specs/005` `FR-034`** — gộp vế *"kiểm số ghế ở cú bấm bắt đầu trận"* thay vì để `FR-035` lặp lại câu zero-trust.
- **`specs/002` `AC-004b`, `AC-014d`** — viết lại theo mô hình **phiên bản theo cú Save**; bản cũ dựa trên *"nhật ký gắn với lần hiển thị"* đã bị `PRD-REQ-115` phủ.

---

## Obsolete scenarios removed

- `specs/011` — `US-007`, `FR-047`→`FR-050`, `AC-039`→`AC-041`, `SC-016` *(cụm disclaimer bản quyền)*. Không mã nào được tái sử dụng, và **không** giữ ghi chú *"số hiệu để trống"*.
- `specs/006` — `OQ-001` *(phát biểu ngược `FR-083a`)*, `OQ-002`, `OQ-003`, `OQ-006`, `XCONF-1`→`XCONF-3`.
- `specs/007` — `OQ-001`→`OQ-004`, toàn bộ mục Open Questions.
- `specs/008` — `CONFLICT-001/002/003`, `OQ-001`→`OQ-003`, `XCONF-1`→`XCONF-3`.
- `specs/009` — `CONFLICT-001/002`, `OQ-001`→`OQ-007`.
- `specs/010` — `CONFLICT-001/002`, `OQ-001`→`OQ-009`.
- `specs/004` — `Q1`→`Q6`.

**0** cặp acceptance scenario nào còn cho hai outcome trái ngược.

---

## Effective behaviors expanded

Nơi một section trước đây chỉ trỏ sang chỗ khác, nay nêu đủ điều kiện · thứ tự đánh giá · outcome · no-change guarantee:

| Vị trí | Trước | Sau | Nguồn |
|---|---|---|---|
| `GR-001` C3, `GR-002` C2, `GR-003` C2 | *"Chuyển `GR-00x`"* | Nêu đủ outcome và state change tại chỗ | `GR-001`→`GR-003` |
| `GR-009` §Biên, `GR-010` C5, `GR-011` §Biên | *"chuyển `GR-011`/`GR-012`"* | Nêu gợi ý cuối, băng điểm hạ 20, câu chưa hiển thị trả lại kho | `GR-011`, `GR-012` |
| `GR-020` §Mốc đóng | *"theo quy tắc chung `GR-006`"* | Nêu **no-change guarantee**: chấm xong thì nút gửi khoá, bản tới sau không lật được phán quyết | `GR-006`, `INV-009` |
| `GR-020` C6 | *"— xem `GR-021`"* | Nêu rõ hình phạt NSHV **thay thế** phần nợ transfer | `GR-021` C5 |
| `GR-023` C5 | *"Xem `GR-030`"* ở cột state change | Nêu đủ: vòng về `LOBBY`, *cách rời* `đã bỏ`, câu đã hiển thị không trả lại kho, dialog phá huỷ + bắt lý do | `GR-030` |
| `STATE-002` §Ra | Không nêu đích | Nêu đích `STATE-001` + *"không có đường vòng → vòng"* | `T-005`, `GR-030` C4 |
| `specs/001` `FR-002`, `FR-003` | Chỉ trỏ `PERM-*` | **6 bước thứ tự đánh giá** + no-change guarantee | `PRD-REQ-002`, `GR-037` |
| `specs/001` `FR-008` | Chuỗi nhãn `C1…C9` | **7 bước đánh giá + bảng quyết định 10 dòng + bảng mốc CÂU KHÉP theo vòng** | `GR-037` |
| `specs/001` `FR-024`, `FR-025` | *"bảy thao tác phá huỷ"* | Nêu đích danh 7 `PERM-*` + thứ tự invalid-state → permission → quyền điều khiển | `docs/permissions.md` |
| `specs/002` `FR-010`→`FR-013d` | Vòng đời `DRAFT`/`ACTIVE` rút gọn | Điều kiện khởi tạo, outcome + no-change từng nhánh, no-op khi duyệt lại, phân xử đồng thời, xoá mềm | `GR-031` |
| `specs/003` `FR-011`, `FR-012` | *"ngưỡng cấu hình được"* | **Biên đóng** *(`≤` chấp nhận, chỉ `>` từ chối)*, thông điệp đọc ngưỡng từ cấu hình, no-change khi từ chối | `PRD-REQ-019` |
| `specs/003` `FR-006`, `FR-022` | Luồng nhập gói | Thứ tự đánh giá đầy đủ; tách *thiếu media* khỏi *gói hỏng / sai cụm* | `NFR-24b` |
| `specs/006` `FR-121` | Không có AC nào | Thêm **`AC-052a`** — phiên bản câu bất biến, sửa kho đề không đổi nhật ký trận | `INV-001`, `INV-022` |
| `specs/008` `FR-066` | Ba ca biên gộp | Tách ba gạch đầu dòng, mỗi ca một outcome | `GR-037` C7/C8/C9, `GR-012` C2 |
| `specs/012` `FR-034` | *"tập chặn cứng giữ nguyên như `INV-014`"* | Liệt kê đủ ba chỗ chặn cứng | `INV-014` |
| `specs/001` | Bảng `GR-037` thiếu ca C10 | Thêm dòng C10 + **`AC-017b`** — cú đóng hiển thị bằng tay thắng cờ reveal | `GR-037` C10, `PERM-044` |

---

## Controlled duplications introduced

Lặp lại có chủ đích để feature spec đọc độc lập, **khớp nguyên nghĩa** nguồn, không thêm/đổi behavior:

- Bảng quyết định `GR-037` và bảng mốc CÂU KHÉP theo vòng → `specs/001` `FR-008`.
- Danh sách 7 permission phá huỷ từ `docs/permissions.md` → `specs/001` `FR-024`.
- Kết cục `GR-037` C7/C8/C9 và `GR-012` C2 → `specs/008` `FR-066`.
- Điều kiện/outcome từ `GR-021`, `GR-032`, `GR-034`, `STATE-040`, `QĐ-076`, `QĐ-096` → mục *Ranh giới dễ đọc nhầm thành mâu thuẫn* của `specs/008` và `specs/009`.
- Ba chỗ chặn cứng của `INV-014` → `specs/012` `FR-034`.
- Giá trị mặc định `padding` = **5 giây**, RuleConfig cấp contest, biên đóng → `specs/007` Key Entities *(trước đó chỉ tồn tại trong Clarifications)*.

---

## References simplified

- **~120 con trỏ `§Clarifications`** gỡ khỏi FR/AC/Edge Cases của 11 spec.
- **`docs/PRD.md`**: `NON-GOAL-019` giữ cho *trình hướng dẫn cài đặt*; disclaimer → `NON-GOAL-020`.
- **`specs/007`**: 4 hàng bảng truy nguyên bỏ hậu tố `*(OQ-00x)*`; 2 hàng bỏ *"mốc cắt đã bỏ"* / *"nay có biên trên"*.
- **`specs/006`**: 2 ô bảng chỉ ghi `*(OQ-005)*` → nêu lý do thật.
- **`specs/009`** `AC-069`: `OQ-005` → `FR-074d`.
- **`specs/012`**: §Quy ước truy nguyên 6 điểm gộp vào `## Out of Scope`.
- **`docs/decisions.md`**: `QĐ-012b` không tồn tại → `NFR-24`.

---

## IDs remapped

- `NON-GOAL-019` *(disclaimer)* → **`NON-GOAL-020`**. Mã `NON-GOAL-019` giữ nguyên cho *trình hướng dẫn cài đặt lần đầu*, mục cũ hơn. 16 chỗ trỏ đã cập nhật.
- `PRD-REQ-084` — đã rút; 3 reference chết còn lại đã gỡ khỏi `docs/PRD.md` và `specs/011`.
- `EVENT-016` — xoá; mã để trống, không cấp lại.
- `specs/008`: mục truy nguyên còn mở được cấp **`OQ-004`** *(mã chưa dùng)*.
- `specs/004`: mục còn mở duy nhất được cấp **`OQ-001`**; 5 chỗ trỏ đã cập nhật.
- `specs/011`: thêm **`FR-055`** *(ràng buộc nghịch về disclaimer)*.
- `specs/006`: thêm **`AC-052a`**. `specs/001`: thêm **`AC-017b`**.
- **0** `FR-*` / `AC-*` / `US-*` / `SC-*` nào bị đánh số lại hàng loạt. **0** ID trùng trong bất kỳ file nào.

---

## Nghiệm thu

| Phép kiểm | Kết quả |
|---|---|
| Dấu vết supersession trong active documents | **0** |
| `CONFLICT-*` còn mở trong `docs/` | **0** *(bốn mục đã phân xử → `QĐ-149`→`QĐ-152`)* |
| Mục `## Clarifications` còn lại | **0** |
| ID trùng trong cùng file | **0** |
| Reference chết *(`GR`, `STATE`, `EVENT`, `INV`, `TERM`, `PERM`, `PRD-REQ`, `NFR`, `NON-GOAL`, `FS`, `RISK`, `ASSUMPTION`, `GOAL`, `ACTOR`, `JOURNEY`, `QĐ`)* | **0** |
| Cặp acceptance scenario có outcome trái ngược | **0** |

---

## Unresolved conflicts

**Không còn.** Bốn mâu thuẫn phát hiện trong lúc hợp nhất đã được chủ dự án phân xử ngày 2026-08-06 và ghi thành **`QĐ-149` → `QĐ-152`**:

| Mã cũ | Phán quyết | Ghi thành | Đã sửa theo |
|---|---|---|---|
| `CONFLICT-001` | `ASSUMPTION-002` đọc **theo trục hồ sơ**; portable không mang giả định này — mục tiêu 1 trận của nó là hình dạng ca dùng, không phải kỳ vọng chờ kiểm chứng | `QĐ-149` | `PRD.md` §18 `ASSUMPTION-002` |
| `CONFLICT-002` | **v1 CÓ bề mặt dọn dữ liệu thủ công**, gác bằng `PERM-063` `retention.purge`. Thứ ngoài phạm vi là **bộ hẹn giờ**, không phải bước dọn | `QĐ-150` | `PRD-REQ-117` *(mới)* · `PERM-063` *(mới)* · `PRD.md` §11 EPIC-011 Scope + Out of scope, §14 `FS-35`, §20.2, §22 · `specs/011` US-006, `FR-041a`→`FR-041e`, `AC-033a`→`AC-033c`, `SC-015a`→`SC-015c`, Key Entities |
| `CONFLICT-003` | Bảng tóm tắt phạm vi hiển thị **phải nêu ngoại lệ C10**; bảng quyết định thắng bảng tóm tắt | `QĐ-151` | `game-rules.md` §Phạm vi hiển thị, dòng *Đáp án chuẩn* |
| `CONFLICT-004` | Dấu Đúng/Sai trước cú bấm **chốt câu** là **lựa chọn tạm, sửa tự do**; **không** đặt giới hạn số lần | `QĐ-152` | `game-state-machine.md` `INV-009` *(gắn nguồn, thôi là suy luận)* |

**Ranh giới đã chấp nhận của `QĐ-150`**: `PERM-063` **không** thuộc bảy thao tác phá huỷ của `QĐ-078` — bảy thao tác đó đổi kết quả một trận, còn bước dọn là vòng đời dữ liệu cấp bản cài. Hệ quả trực tiếp: `NFR-15` *(lý do bắt buộc)* **không** áp cho bước dọn; hàng rào của nó là dialog liệt kê phạm vi của `NFR-35`.

## Unresolved clarifications

Mười ba mục đã được chủ dự án phân xử ngày 2026-08-06, ghi thành **`QĐ-153` → `QĐ-161`**:

| Spec | Mã | Phán quyết | Ghi thành |
|---|---|---|---|
| `002` | `OQ-005` | Xoá mềm là **một chiều** và **gỡ tư cách thành viên**; bộ Chướng ngại vật mất thành phần thì **vỡ** | `QĐ-154` |
| `002` | `OQ-007` | Lưu câu **thiếu media được**; chỉ cảnh báo, **không** thử lại | `QĐ-155` |
| `002` | `OQ-003` vế `clues[]` | Câu Chướng ngại vật mang **đúng một** gợi ý | `QĐ-153` |
| `002` | `OQ-003` vế `options[]` | **Tự tan** — bỏ hẳn `answerInputKind` và `options[]`; **một** ô nhập chữ duy nhất, format yêu cầu nằm trong **phần đề**. Lật `QĐ-066` | `QĐ-162` |
| `003` | `OQ-006` | Cụm mật khẩu **không bao giờ được lưu** ⇒ mỗi lần xuất là một lần khai lại; câu hỏi *dùng lại hay bắt mới* **tự tan** | `QĐ-156` |
| `003` | `OQ-007` | Chặn xuất/nhập **chỉ khi kho đề rỗng**; từ 1 câu trở lên đều nhận | `QĐ-157` |
| `004` | `OQ-001` | **Mở rộng** `PRD-REQ-078` để `Related epic` gồm EPIC-004, thay vì cấp mã mới | — *(sửa PRD)* |
| `006` | `OQ-004` | Băng điểm cho `rowCount` 5-8: **preset mặc định** `băng(k) = max(30, 70 − 10k)`, suy từ luật gốc; mảng vẫn cấu hình được | `QĐ-158` |
| `008` | `OQ-004` | `specs/003` nhận vế EPIC-003 của `PRD-REQ-098` *(FR-032 mới)* | — |
| `012` | `OQ-002` | Nút khoá cổng là thao tác của **admin đang điều khiển trận**; hành vi ở `specs/001` FR-035 | — *(sửa `specs/009`)* |
| `012` | `OQ-003` | *Hồ sơ triển khai*, *bản cài*, *bản triển khai* là **một** khái niệm, hai giá trị | `QĐ-160`, `TERM-064` |
| `012` | `OQ-004` | Ngưỡng media **dùng chung** cho cả hai hồ sơ | `QĐ-159` |
| `012` | `OQ-006` | Giữ nguyên: quy mô viewer là **mặc định cấu hình được**, chốt số khi định cỡ máy thật | — |
| *(mục C)* | Trùng lặp `004` ↔ `005` | **Không phải trùng lặp** — `004` sở hữu lúc **chưa có trận chạy**, `005` sở hữu **`LOBBY` của trận đang tồn tại** | `QĐ-161` |

**Còn lại hai mục, cả hai ngoài phạm vi v1:**

| Spec | Mã | Nội dung |
|---|---|---|
| `006` | `OQ-004` | Băng điểm cho `rowCount` 5-8 — **đã có mặc định** (`QĐ-158`), vẫn ngoài phạm vi v1 vì `rowCount` khoá cứng ở 4 |
| `006` | `OQ-005` | Luật cho số ghế **trên** 4 — nguồn chỉ viết cho đúng 4 người; v1 chặn cứng ở cú bấm bắt đầu trận |

## Lượt rà 2026-08-16

Quét lại toàn corpus bằng script *(dead reference trên 17 họ mã · ID trùng trong cùng file · độ lệch cột bảng · đối chiếu số đếm tự khai)*. Sáu nhóm còn sót, đã sửa.

### Khối lịch sử đã cắt

- **6 đoạn `**Lưu ý về đầu vào**`** ở `specs/006` → `specs/011` — ghi chép về một lệnh gọi trỏ vào hai đường dẫn không tồn tại. Không mang requirement nào; `**Nguồn**` ngay trên nó đã khai đủ nguồn đã dùng.
- **8 mục `## Open Questions` kể lại phán quyết cũ** — `specs/001`, `003`, `004`, `005`, `007`, `008`, `009`, `012`. Mọi nội dung đã xác minh là có mặt trong FR/AC trước khi cắt: chủ sở hữu nút khoá cổng ở `specs/001` FR-035 · ngưỡng media dùng chung ở `specs/012` FR-023 và AC-011x · quy mô viewer cấu hình được ở `specs/012` FR-023 và AC-022 · vế EPIC-003 của `PRD-REQ-098` ở `specs/003` FR-032. Nay mỗi mục còn đúng *"Không có."*; `specs/006` giữ hai mục thật sự còn mở.
- **Một câu lặp nguyên văn hai lần** trong `docs/game-state-machine.md` §Ngoại lệ lớp phủ.

### Effective behaviors expanded

| Vị trí | Trước | Sau |
|---|---|---|
| `EVENT-006` §Hợp lệ ở | *"như `EVENT-005`"* | Nêu đủ điều kiện vào, hạng dialog, và cả hai nhánh không hợp lệ |
| `EVENT-013` §Mô tả và §Hợp lệ ở | *"như `EVENT-012`"* cho cả ba trường | Nêu đủ phán quyết, hai trạng thái hợp lệ, ba ca không hợp lệ kèm mốc khoá nút chấm |
| `EVENT-023` §Không hợp lệ ở | *"như `EVENT-022`"* | Nêu đủ: tín hiệu đã duyệt hoặc đã từ chối, nút một chiều tự tắt, server bỏ qua lệnh trùng |
| `GR-037` C9 | *"theo `GR-012`"* | Nêu mốc lộ thật: có người giải đúng, hoặc admin bấm công bố; cờ hiện đáp án không mở đường nào |
| `docs/game-state-machine.md` §Ba hạng phản hồi | *"Xem `INV-014`"* | Kể tên **ba** chỗ chặn cứng ngay trong ô bảng |
| `specs/011` bảng C9 | *"theo `GR-012`"* | Nêu mốc lộ thật |
| `specs/012` `AC-032` §Given | *"như `INV-014` khai"* | Kể tên ba chỗ chặn cứng trong chính Given |
| `specs/009` `AC-023` §Then | *"đi theo `GR-037`"* | Nêu outcome: đáp án hàng ngang đẩy tới cả hai kênh public ngay khi câu khép vì cờ đang BẬT |

### Số đếm tự khai đã lệch với corpus

| Chỗ khai | Khai | Thực tế |
|---|---|---|
| `docs/PRD.md` §2, `game-rules.md`, `game-state-machine.md` | 148 quyết định `QĐ-001`→`QĐ-148` | **163**, `QĐ-001`→`QĐ-163` |
| `docs/README.md` | 145 quyết định | **163** |
| `docs/PRD.md` §2, `docs/README.md` | 63 thuật ngữ `TERM-001`→`TERM-063` | **64**, tới `TERM-064` |
| `docs/README.md` | 62 permission `PERM-001`→`PERM-062` | **63** — `PERM-063` `retention.purge` ra đời cùng `QĐ-150` |
| `CLAUDE.md` §1 | `QĐ-001`→`QĐ-145` | **`QĐ-163`** |

### Nghiệm thu lượt rà

| Phép kiểm | Kết quả |
|---|---|
| Dead reference trên 17 họ mã, trong `docs/` và 12 spec | **0** *(bốn mã `EPIC-013`/`EPIC-014`/`JOURNEY-008`/`PRD-REQ-099` ở `README` là mục lục của `roadmap-post-v1.md`, có định nghĩa ở đó)* |
| ID trùng trong cùng file | **0** |
| Bảng markdown lệch cột | **0** |
| `FR-*` chỉ trỏ đi mà không nêu behavior | **0** |
| `Then` chỉ trỏ đi mà không nêu outcome | **0** |
| Guard của `EVENT-*` mượn từ event khác | **0** |
| Số đếm tự khai lệch corpus | **0** |
| Giá trị biên trùng lặp xuyên tài liệu *(cửa sổ chuông 3/5/15 giây · `padding` 5 giây · 52 + 6 = 58 khe âm thanh · trần 4 ghế)* | Nhất quán |

---

## Việc còn lại cần quyết định ranh giới, không phải consolidate

**Trùng lặp xuyên spec giữa `specs/004` và `specs/005`.** `specs/004` `FR-018`/`FR-019`/`FR-020` và `specs/005` `FR-025`→`FR-028` đặc tả **cùng một** quy tắc sửa danh sách câu tại `LOBBY` (`GR-031` C5-C8), dưới hai `PRD-REQ` khác nhau. Gộp chúng là một quyết định **ranh giới epic** *(EPIC-004 contest builder vs EPIC-005 phòng thi)*, không phải một phép khử trùng lặp — nên cả hai được giữ nguyên.
