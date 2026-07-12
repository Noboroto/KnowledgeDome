# Rule-spec Đường lên đỉnh Olympia — Luật O26 ("luật 2026")

> ✅ **XÁC NHẬN 12/07 (D8 đã chốt): SOURCE OF TRUTH = [Fandom — Luật chơi/Olympia 26](https://duong-len-dinh-olympia.fandom.com/vi/wiki/Lu%E1%BA%ADt_ch%C6%A1i/Olympia_26)** (đã truy cập + đối chiếu toàn văn 12/07). Mọi giá trị dưới đây đã sửa theo Fandom, hết mâu thuẫn ⚠️.
> Mọi giá trị vẫn là DEFAULT của hệ thống (RuleConfig) — admin config được per-contest; contest builder có nút **"Áp dụng luật 2026"** áp nguyên preset này.
> **Điểm độc lập với thời gian**: `timeSeconds` là metadata TỪNG CÂU HỎI (cùng mức 20đ có thể có câu 15s và câu 40s); hệ thống chọn câu theo MỨC ĐIỂM, thời gian lấy theo câu; bảng dưới chỉ là default khi câu không tự khai thời gian.

4 thí sinh, 4 vòng + câu hỏi phụ.

## 1. Khởi động (KHOI_DONG)

Tách 2 lượt — riêng + chung.

| Tham số | Default (Fandom O26) | Ghi chú |
|---|---|---|
| Lượt riêng: số câu/thí sinh | 6 | thi lần lượt từng thí sinh |
| Lượt riêng: thời gian mỗi câu | **3s** (từ lúc MC đọc xong) | ✅ Fandom (default cũ 5s SAI) |
| Lượt riêng: điểm đúng / sai | +10 / 0 | |
| Lượt chung: số câu | 12 | cả 4 thí sinh bấm chuông giành quyền; **được bấm khi MC đang đọc** |
| Lượt chung: thời gian trả lời sau khi giành quyền | **3s** | ✅ Fandom (default cũ 5s SAI) |
| Lượt chung: cửa sổ chuông sau khi MC đọc xong | 3s — không ai bấm → bỏ qua câu | `buzzWindowSec` default 3 |
| Lượt chung: điểm đúng / sai | +10 / **−5** | "sai hoặc bấm chuông mà không có đáp án sau 3s" đều −5; sai thì KHÔNG mở lại chuông cho người khác |
| Ghi nhận đáp án | đổi liên tục trước khi MC công bố — **tính bản CUỐI**; không đổi → tính bản đầu | khớp nguyên tắc last-wins |
| Chấm | MC/admin bấm Đúng/Sai (trả lời miệng) | |

## 2. Vượt chướng ngại vật (VCNV)

| Tham số | Default (Fandom O26) | Ghi chú |
|---|---|---|
| Số hàng ngang | 4 | + 1 ô trung tâm (miếng ghép thứ 5, gợi ý cuối) |
| Ảnh chướng ngại vật | 5 miếng ghép (4 góc đánh số cố định + trung tâm) | hàng ngang đúng → mở miếng tương ứng |
| Lượt chọn hàng ngang | mỗi TS tối đa 1 lượt, từ vị trí số 1; có người bị loại → lượt dồn, quay vòng về vị trí 1 nếu còn hàng chưa chọn | ✅ Fandom |
| Thời gian suy nghĩ mỗi hàng ngang | 15s | cả 4 cùng trả lời bằng bàn phím; yêu cầu đúng chính tả |
| Điểm hàng ngang đúng | +10 | |
| Bấm chuông trả lời CNV | bất kỳ lúc nào | trả lời sai → **bị loại khỏi phần thi này** |
| Điểm giải đúng CNV | **60 / 50 / 40 / 30** khi bấm trong hàng ngang thứ 1/2/3/4 | ✅ Fandom (default cũ 80/60/40/20 SAI) |
| Ô trung tâm (sau 4 hàng, chưa ai giải CNV) | câu hỏi gợi ý cuối: đúng +10 (mở ô), sai không mở; sau đó **15s** suy nghĩ CNV; giải đúng lúc này = **20 điểm** | ✅ Fandom |

## 3. Tăng tốc (TANG_TOC)

| Tham số | Default (Fandom O26) | Ghi chú |
|---|---|---|
| Số câu | 4 | 4 loại: nhìn nhanh / sắp xếp / suy luận / đoạn băng |
| Thời gian mỗi câu | **20 / 20 / 30 / 30s** | ✅ Fandom (default cũ 10/20/30/40 SAI) |
| Cách trả lời | cả 4 cùng trả lời trên máy (KHÔNG bấm chuông) | hệ thống ghi timestamp; đúng chính tả |
| Điểm theo thứ hạng tốc độ (trong số người đúng) | 40 / 30 / 20 / 10 | sai = 0, không trừ; **đồng thời gian → cùng mức điểm** (✅ Fandom xác nhận đề xuất D13.3) |
| Chấm | auto so khớp đáp án + admin override | |

## 4. Về đích (VE_DICH)

Gói câu chỉ còn 2 mức 20/30 (bỏ mức 10 của luật cũ).

| Tham số | Default (Fandom O26) | Ghi chú |
|---|---|---|
| Số câu mỗi thí sinh | 3 (chọn từ 2 mức 20/30 thành gói) | chọn trước lượt thi |
| Thứ tự lượt thi | điểm cao nhất TẠI THỜI ĐIỂM xếp lượt (tính lại sau mỗi lượt hoàn thành); hoà → vị trí đứng thấp hơn đi trước | ✅ Fandom |
| Thời gian suy nghĩ (default theo mức điểm) | 20đ → 15s, 30đ → 20s | ✅ Fandom; per-question `timeSeconds` override |
| Câu thực hành | 20đ: 15s nghĩ + 30s thực hành; 30đ: 20s nghĩ + 60s thực hành | ✅ Fandom |
| Trả lời đúng | +giá trị câu | |
| Trả lời sai/hết giờ | 0, mở quyền cướp | |
| Cướp quyền (3 TS còn lại bấm chuông, **5s**) | đúng: **+giá trị câu LẤY TỪ điểm người trả lời sai (`stealMode: 'transfer'`)** ✅ Fandom; sai: **−½ giá trị câu**; câu thực hành khi cướp: 20đ→20s, 30đ→40s thực hành | default cũ "không steal" SAI |
| Ghi nhận đáp án | người thi chính: **bản CUỐI** trước hết giờ; người cướp: **bản ĐẦU TIÊN** | ✅ Fandom — engine phân biệt 2 chế độ |
| Ngôi sao hy vọng | 1 lần/thí sinh, đặt TRƯỚC khi câu được đọc/hiện; đúng ×2 giá trị câu, sai −giá trị câu (kể cả có người cướp hay không) | ✅ Fandom |

## 5. Câu hỏi phụ (TIE_BREAK)

- Kích hoạt khi hòa điểm ở vị trí cần phân định.
- 3 câu, 15s/câu, bấm chuông giành quyền; **bấm nhanh nhất + trả lời đúng → thắng; sai → cả nhóm sang câu TIẾP THEO** (✅ Fandom — không mở lại cùng câu cho người còn lại).
- **Bấm chuông trước hiệu lệnh MC → mất quyền trả lời câu đó** (✅ Fandom).
- Không cộng vào điểm trận. Hết 3 câu chưa phân định → **bốc thăm** (✅ Fandom xác nhận đề xuất D13.6; hệ thống random, admin xác nhận).

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
| ✅ VCNV: cả 4 thí sinh bị loại (đều sai CNV) | **User chốt 12/07:** kết thúc lượt/vòng, không ai được điểm CNV; **mở toàn bộ miếng ghép + công bố CNV là NÚT THỦ CÔNG của admin** (không auto — giữ nhịp dẫn); các hàng ngang chưa hỏi bị bỏ |
| ✅ VCNV: người bị loại có được trả lời hàng ngang còn lại? | **Không** — Fandom xác nhận (lượt chọn dồn cho người chưa bị loại, quay vòng về vị trí 1) |
| VCNV: bấm chuông giải CNV giữa lúc timer hàng ngang đang chạy | Timer hàng ngang PAUSE, xử lý CNV xong (đúng → kết thúc vòng; sai → loại người bấm) rồi RESUME cho người còn lại |
| Khởi động lượt chung: không ai bấm chuông | Hết `buzzWindowSec` (default **3s** sau khi đọc xong — Fandom) → công bố đáp án, sang câu tiếp (admin có nút next thủ công override) |
| ✅ Khởi động lượt chung: bấm sai | −5, câu đó KHÔNG mở lại chuông cho người khác (Fandom; admin override được) |
| ✅ Tăng tốc: 2 người đúng cùng timestamp (ms) | Cùng nhận mức điểm cao (40/40/20/10) — **Fandom xác nhận** ("cùng khoảng thời gian → cùng mức điểm"); so sánh ở độ phân giải ms server-received |
| Tăng tốc với ghế trống/2 người chơi (D1b) | Thang điểm vẫn 40/30/... theo thứ hạng thực tế trong số người đúng |
| ✅ Thí sinh rớt mạng đúng lượt riêng của mình (khởi động riêng / về đích) | **User chốt 12/07:** engine tự PAUSE + prompt admin: chờ reconnect (trong grace) / skip lượt / thay thế; chưa chọn gói về đích khi tới lượt → admin chọn hộ (default 20/20/20) |
| Về đích: quyền cướp đang mở mà câu bị skip (media hỏng) | Huỷ cửa sổ cướp, không ai cộng/trừ; câu thay thế chạy lại từ đầu |
| NSHV đã chọn, câu bị thay bằng câu dự phòng | NSHV áp sang câu thay thế (không mất lượt chọn) |
| Tie-break hòa 3-4 người / hòa nhiều vị trí | Chạy tie-break **per nhóm hòa, theo thứ tự vị trí cần phân định** (function `(tiedSeats[], targetPosition) → winner`); người bấm sai bị loại khỏi CÂU đó, những người còn lại thi tiếp cùng câu; hết 3 câu → bốc thăm |
| ✅ Vị trí nào cần tie-break? | **User chốt 12/07:** chỉ vị trí NHẤT; config `tieBreakPositions` đổi được |
| ✅ Hết câu hỏi phụ khi hoà dai dẳng | Preflight yêu cầu tối thiểu N câu phụ (config, default 3); vẫn hết → bốc thăm — **Fandom xác nhận** |
| Điểm lẻ khi admin config mức điểm tuỳ ý (cướp sai −½ của 25) | RuleConfig ép giá trị câu là số chẵn (Zod); nếu vẫn lẻ → làm tròn về 0 (floor với âm là ceil) |

## Nguồn

- **SOURCE OF TRUTH (D8 chốt 12/07):** https://duong-len-dinh-olympia.fandom.com/vi/wiki/Lu%E1%BA%ADt_ch%C6%A1i/Olympia_26
- https://vi.wikipedia.org/wiki/Đường_lên_đỉnh_Olympia
- https://vi.wikipedia.org/wiki/Đường_lên_đỉnh_Olympia_năm_thứ_25
- https://vi.wikipedia.org/wiki/Đường_lên_đỉnh_Olympia_năm_thứ_26
- https://dantri.com.vn/giao-duc/duong-len-dinh-olympia-24-mo-man-doi-luat-choi-o-hai-vong-thi-20231016152318649.htm
- https://vietnamnet.vn/chuong-trinh-duong-len-dinh-olympia-nam-thu-24-doi-luat-choi-2202569.html
- https://tienphong.vn/luat-choi-moi-o-duong-len-dinh-olympia-nam-thu-24-post1578437.tpo
