# Rule-spec Đường lên đỉnh Olympia — Luật O24-O26 ("luật 2026")

> Bản tổng hợp từ research online (Wikipedia VN O25/O26, Dân Trí, VietnamNet, Tiền Phong, VTV).
> Đây là **draft chờ user duyệt**. Mọi giá trị đều là DEFAULT của hệ thống — admin config được per-contest.
> Các chỗ đánh dấu ⚠️ là nguồn mâu thuẫn / chưa chắc chắn (xem DEFERED.md D8).

Tổng điểm lý thuyết toàn trận: ~800 (giảm từ ~980 của luật cũ). 4 thí sinh, 4 vòng + câu hỏi phụ.

## 1. Khởi động (KHOI_DONG)

Từ O24: tách 2 lượt — riêng + chung.

| Tham số | Default | Ghi chú |
|---|---|---|
| Lượt riêng: số câu/thí sinh | 6 | thi lần lượt từng thí sinh |
| Lượt riêng: thời gian mỗi câu | 5s ⚠️ | nguồn khác nói 3s; admin config |
| Lượt riêng: điểm đúng / sai | +10 / 0 | |
| Lượt chung: số câu | 12 | cả 4 thí sinh bấm chuông giành quyền |
| Lượt chung: thời gian trả lời sau khi bấm chuông | 5s | |
| Lượt chung: điểm đúng / sai | +10 / **-5** | sai thì các thí sinh khác KHÔNG được bấm lại câu đó (MC công bố đáp án) ⚠️ |
| Chấm | MC/admin bấm Đúng/Sai (trả lời miệng) | |

## 2. Vượt chướng ngại vật (VCNV)

| Tham số | Default | Ghi chú |
|---|---|---|
| Số hàng ngang | 4 | + 1 ô trung tâm (miếng ghép thứ 5) |
| Ảnh chướng ngại vật | 5 miếng ghép (4 góc + trung tâm) | mỗi hàng ngang đúng → mở miếng tương ứng |
| Thời gian suy nghĩ mỗi hàng ngang | 15s | cả 4 thí sinh cùng trả lời bằng bàn phím |
| Điểm hàng ngang đúng | +10 | |
| Bấm chuông trả lời CNV | bất kỳ lúc nào | trả lời sai → **bị loại khỏi phần thi này** |
| Điểm trả lời đúng CNV | 80 / 60 / 40 / 20 ⚠️ | theo số hàng ngang đã mở (0-1 / 2 / 3 / 4+trung tâm). Một nguồn khác nói 60/50/40/30/20 — cần user xác nhận; admin config mảng điểm |

## 3. Tăng tốc (TANG_TOC)

| Tham số | Default | Ghi chú |
|---|---|---|
| Số câu | 4 | độ khó tăng dần, thường có câu video/hình ảnh |
| Thời gian mỗi câu | 10 / 20 / 30 / 40s ⚠️ | có nguồn nói O26 đổi thành 20/20/30/30; admin config mảng |
| Cách trả lời | cả 4 cùng trả lời trên máy (KHÔNG bấm chuông) | hệ thống ghi timestamp |
| Điểm theo thứ hạng tốc độ (trong số người đúng) | 40 / 30 / 20 / 10 | sai = 0, không trừ |
| Chấm | auto so khớp đáp án + admin override | |

## 4. Về đích (VE_DICH)

Từ O24: gói câu chỉ còn 2 mức 20/30 (bỏ mức 10 của luật cũ).

| Tham số | Default | Ghi chú |
|---|---|---|
| Số câu mỗi thí sinh | 3 | thí sinh tự chọn mức điểm từng câu trước lượt thi |
| Mức điểm mỗi câu | 20 hoặc 30 | |
| Thời gian suy nghĩ | 20đ → 15s, 30đ → 20s ⚠️ | nguồn khác nói 20s/40s; admin config map |
| Trả lời đúng | +giá trị câu | |
| Trả lời sai/hết giờ | 0, mở quyền cướp | |
| Cướp quyền (thí sinh khác bấm chuông, 5s) | đúng: **+giá trị câu** (điểm lấy từ thí sinh về đích ⚠️ — cần xác nhận có steal hay chỉ cộng); sai: **-½ giá trị câu** | |
| Ngôi sao hy vọng | 1 lần/thí sinh, chọn trước khi đọc câu hỏi; đúng ×2 giá trị câu, sai −giá trị câu | |

## 5. Câu hỏi phụ (TIE_BREAK)

- Kích hoạt khi hòa điểm ở vị trí cần phân định.
- Tối đa 3 câu, 15s/câu, bấm chuông giành quyền; đúng → thắng; sai → thí sinh còn lại có cơ hội.
- Không cộng vào điểm trận. Hết 3 câu chưa phân định → bốc thăm (hệ thống random, admin xác nhận).

## 6. Nguyên tắc thiết kế engine — ⚠️ ĐÃ THAY THẾ

> **Mục này đã bị thay bởi `ruleconfig-v2-spec.md`** (12/07 — round playlist, 1-12 ghế + đội, mọi biến thể vòng). Nội dung dưới giữ lại làm ngữ cảnh lịch sử; engine code theo spec v2.

1. **Mọi con số ở trên là `RuleConfig` (JSON, Zod-validated)** — admin sửa per-contest, có preset "O26 chuẩn".
2. Server-authoritative: timer, timestamp bấm chuông, điểm đều tính ở server.
3. Câu trả lời miệng → admin chấm Đúng/Sai; câu nhập text → auto-match (normalize dấu/hoa thường) + admin override.
4. Mọi thay đổi điểm có audit log (ai, lúc nào, lý do).

## 7. Edge-cases luật — "luật của tình huống xấu" (bổ sung sau red-team)

> Nguồn công khai chỉ mô tả happy path. Các hành vi dưới đây là **đề xuất của Claude** để engine encode được; các mục 🟡 cần user xác nhận theo luật thật (DEFERED D13).

| Tình huống | Hành vi đề xuất |
|---|---|
| 🟡 VCNV: cả 4 thí sinh bị loại (đều sai CNV) | Kết thúc vòng ngay: mở toàn bộ miếng ghép + công bố CNV, không ai được điểm CNV; các hàng ngang chưa hỏi bị bỏ |
| 🟡 VCNV: người bị loại có được trả lời hàng ngang còn lại? | **Không** (theo luật thật) — chỉ mất quyền CNV lẫn hàng ngang |
| VCNV: bấm chuông giải CNV giữa lúc timer hàng ngang đang chạy | Timer hàng ngang PAUSE, xử lý CNV xong (đúng → kết thúc vòng; sai → loại người bấm) rồi RESUME cho người còn lại |
| Khởi động lượt chung: không ai bấm chuông | Hết `buzzWindowSec` (default 5s sau khi đọc xong) → công bố đáp án, sang câu tiếp (admin có nút next thủ công override) |
| 🡪 Khởi động lượt chung: bấm sai | −5, câu đó KHÔNG mở lại chuông cho người khác (theo research; admin override được) |
| 🟡 Tăng tốc: 2 người đúng cùng timestamp (ms) | Cùng nhận mức điểm cao (kiểu thể thao: 40/40/20/10); so sánh ở độ phân giải microsecond trước khi coi là hòa |
| Tăng tốc với ghế trống/2 người chơi (D1b) | Thang điểm vẫn 40/30/... theo thứ hạng thực tế trong số người đúng |
| 🟡 Thí sinh rớt mạng đúng lượt riêng của mình (khởi động riêng / về đích) | Engine tự PAUSE + prompt admin: chờ reconnect (trong grace) / skip lượt / thay thế; chưa chọn gói về đích khi tới lượt → admin chọn hộ (default 20/20/20) |
| Về đích: quyền cướp đang mở mà câu bị skip (media hỏng) | Huỷ cửa sổ cướp, không ai cộng/trừ; câu thay thế chạy lại từ đầu |
| NSHV đã chọn, câu bị thay bằng câu dự phòng | NSHV áp sang câu thay thế (không mất lượt chọn) |
| Tie-break hòa 3-4 người / hòa nhiều vị trí | Chạy tie-break **per nhóm hòa, theo thứ tự vị trí cần phân định** (function `(tiedSeats[], targetPosition) → winner`); người bấm sai bị loại khỏi CÂU đó, những người còn lại thi tiếp cùng câu; hết 3 câu → bốc thăm |
| 🟡 Vị trí nào cần tie-break? | Default: chỉ vị trí NHẤT (giải thưởng); config `tieBreakPositions` |
| 🟡 Hết câu hỏi phụ khi hoà dai dẳng | Preflight yêu cầu tối thiểu N câu phụ (config, default 3); vẫn hết → bốc thăm (khớp luật gốc), admin xác nhận |
| Điểm lẻ khi admin config mức điểm tuỳ ý (cướp sai −½ của 25) | RuleConfig ép giá trị câu là số chẵn (Zod); nếu vẫn lẻ → làm tròn về 0 (floor với âm là ceil) |

## Nguồn

- https://vi.wikipedia.org/wiki/Đường_lên_đỉnh_Olympia
- https://vi.wikipedia.org/wiki/Đường_lên_đỉnh_Olympia_năm_thứ_25
- https://vi.wikipedia.org/wiki/Đường_lên_đỉnh_Olympia_năm_thứ_26
- https://dantri.com.vn/giao-duc/duong-len-dinh-olympia-24-mo-man-doi-luat-choi-o-hai-vong-thi-20231016152318649.htm
- https://vietnamnet.vn/chuong-trinh-duong-len-dinh-olympia-nam-thu-24-doi-luat-choi-2202569.html
- https://tienphong.vn/luat-choi-moi-o-duong-len-dinh-olympia-nam-thu-24-post1578437.tpo
