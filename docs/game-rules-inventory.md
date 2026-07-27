# Game Rules Inventory — Olympia Contest System

> **Ngày lập**: 2026-07-23 · **Phạm vi**: mọi luật chơi hệ thống sẽ thực thi, thu thập từ repo + Internet.
>
> **Nguyên tắc**: KHÔNG bổ sung luật mới. Mọi dòng dẫn về file + heading nguồn; nguồn web dẫn URL trực tiếp. Chỗ nguồn im lặng ghi vào "Chưa định nghĩa", KHÔNG tự điền.
>
> **Trạng thái nguồn**: `plans/260711-2340-olympia-contest-system/**` là **bản nháp** (constitution v1.2.0). Nguồn web đã được lưu nguyên văn vào `docs/source/`.

## Bảng viết tắt nguồn

### Nguồn web ĐƯỢC CHỌN (đã lưu snapshot nguyên văn)

| Ký hiệu | URL trực tiếp | Bản lưu trữ local | Lấy lúc | Căn cứ |
|---|---|---|---|---|
| `F26` | <https://duong-len-dinh-olympia.fandom.com/vi/wiki/Lu%E1%BA%ADt_ch%C6%A1i/Olympia_26> | `docs/source/fandom-olympia-26-luat-choi.md` | 2026-07-23 | **`DEF` D8 ✅ chốt: source of truth DUY NHẤT** |

### Nguồn web BỊ LOẠI ❌ (không lưu snapshot)

| Ký hiệu | URL | Lý do loại |
|---|---|---|
| `W26` | <https://vi.wikipedia.org/wiki/%C4%90%C6%B0%E1%BB%9Dng_l%C3%AAn_%C4%91%E1%BB%89nh_Olympia_n%C4%83m_th%E1%BB%A9_26> | **`DEF` D8 chốt Fandom là source of truth duy nhất** → Wikipedia bị loại làm căn cứ luật. Snapshot đã tạo ngày 2026-07-23 và **đã xoá** theo chỉ đạo "bỏ lưu trữ rule bị loại". Các giá trị cụ thể bị loại: xem PHẦN 8A. |

Nguồn web khác đã tra, **không dùng làm căn cứ luật**:
<https://duong-len-dinh-olympia.fandom.com/vi/wiki/Olympia_26> ·
<https://duong-len-dinh-olympia.fandom.com/vi/wiki/Lu%E1%BA%ADt_ch%C6%A1i> ·
<https://vi.wikipedia.org/wiki/%C4%90%C6%B0%E1%BB%9Dng_l%C3%AAn_%C4%91%E1%BB%89nh_Olympia> ·
<https://tienphong.vn/luat-choi-moi-o-duong-len-dinh-olympia-nam-thu-24-post1578437.tpo> ·
<https://dantri.com.vn/nhip-song-tre/duong-len-dinh-olympia-dot-ngot-thay-doi-luat-choi-giua-mua-thu-22-20220103002309238.htm>

> **Cách đọc tài liệu này**: mọi tham chiếu `W26` bên dưới là **biến thể ĐÃ BỊ LOẠI**, giữ lại chỉ để truy vết vì sao loại. Không dùng `W26` làm căn cứ cho bất kỳ requirement nào.

### Nguồn repo

| Ký hiệu | File |
|---|---|
| `R26` | `plans/260711-2340-olympia-contest-system/research/rules-2026.md` |
| `SPEC` | `plans/260711-2340-olympia-contest-system/research/ruleconfig-v2-spec.md` |
| `DEF` | `plans/260711-2340-olympia-contest-system/DEFERED.md` |
| `PRD` | `plans/260711-2340-olympia-contest-system/PRD.md` |
| `US` | `plans/260711-2340-olympia-contest-system/user-stories.md` |
| `P06` | `plans/260711-2340-olympia-contest-system/phase-06-game-engine.md` |
| `DEMO` | `public/assets/engine.js` |
| `ATH` | `plans/260711-2340-olympia-contest-system/research/athena-scout.md` |

## Trạng thái game dùng trong tài liệu này

Nguồn: `game-rules-decisions.md` §11.23 `Đ-38` (2026-07-27) — **đang có hiệu lực**.

**`LOBBY` · `rounds[i]` (vòng theo playlist) · `TIE_BREAK` · `FINISHED`** — bốn giá trị.
`LOBBY` = **cửa vào vòng**: trạng thái nghỉ của trận, dùng cả **trước vòng đầu tiên** lẫn **giữa hai vòng**.

---

# PHẦN 0 — Trạng thái xác minh nguồn

## X-1. Source of truth đã lấy được nguyên văn ✅

`DEF` D8 chỉ định [Fandom — Luật chơi/Olympia 26](https://duong-len-dinh-olympia.fandom.com/vi/wiki/Lu%E1%BA%ADt_ch%C6%A1i/Olympia_26) là source of truth duy nhất.

- WebFetch trực tiếp trả **HTTP 402** (chặn ở tầng fetch, không phải Fandom).
- Lấy thành công qua **Exa web_fetch** → lưu nguyên văn vào `docs/source/fandom-olympia-26-luat-choi.md`.
- **Kết quả đối chiếu**: mọi giá trị gắn nhãn "✅ Fandom" trong `R26` đều **KHỚP** với nguyên văn. `R26` đáng tin ở phần giá trị số.

**Rủi ro còn lại**: Fandom là wiki cộng đồng, sửa được bất kỳ lúc nào, không versioning. `DEF` D8 không có cơ chế phát hiện thay đổi. Bản snapshot trong `docs/source/` là biện pháp cố định — nên dùng nó thay URL sống khi build preset.

## X-2. Wikipedia BỊ LOẠI làm nguồn luật — quyết định đã có sẵn trong `plans/`

Đợt kiểm kê phát hiện Wikipedia mô tả luật O26 khác Fandom ở nhiều điểm. **Không cần phân xử mới** — `DEF` **D8 đã chốt (12/07)**: *"source of truth = Fandom — Luật chơi/Olympia 26"*, và bảng giá trị trong D8 liệt kê đúng các số của Fandom.

→ **Follow D8. Mọi biến thể của Wikipedia bị REJECT.** Chi tiết từng giá trị bị loại: PHẦN 8A.

**Bằng chứng độc lập củng cố D8** (không phải căn cứ quyết định, chỉ là kiểm chứng):
- `W26` mục Khởi động mô tả 12 câu / 60 giây và ghi tối đa **180 điểm** — nhưng 12 × 10 = **120**. Sai số học nội tại.
- `W26` mục Về đích ghi mức **20 và 40**, nhưng bảng thời gian thực hành ngay dưới lại ghi **20 và 30**. Tự mâu thuẫn.
- `F26` chi tiết hơn và nhất quán nội tại ở mọi mục.

**Đối chiếu `R26` với `F26` (nguyên văn)**: mọi giá trị gắn nhãn "✅ Fandom" trong `R26` đều **KHỚP**. `R26` đáng tin ở phần giá trị số.

**Rủi ro còn lại**: Wikipedia là nguồn công khai dễ tra hơn Fandom → người ngoài dự án sẽ gặp số liệu khác và tưởng hệ thống sai. Nên ghi lý do loại vào tài liệu người dùng.

**Lưu ý toàn cục**: mọi giá trị dưới đây là **DEFAULT của preset `O26_DEFAULT@1`**, admin config được per-contest (`R26` §đầu; `SPEC` §1.1; `PRD` FR-3.1). Không giá trị nào hard-code trong engine.

---

# PHẦN 1 — KHỞI ĐỘNG

## R-KD-01. Lượt riêng

- **Nguồn**: [`F26` §Khởi động](https://duong-len-dinh-olympia.fandom.com/vi/wiki/Lu%E1%BA%ADt_ch%C6%A1i/Olympia_26) đoạn 2 · `R26` §1 · `DEMO` L14 · `SPEC` §4 kind `individual-count`
- **Nguyên văn nguồn**: *"Trong lượt riêng, mỗi thí sinh trả lời 6 câu hỏi. Thời gian suy nghĩ cho mỗi câu hỏi là 3 giây tính từ lúc người dẫn chương trình đọc xong câu hỏi."*
- **Điều kiện đầu vào**: lượt riêng của một thí sinh; **6 câu**; **3s**/câu tính từ khi MC đọc xong; trả lời **miệng**.
- **Kết quả khi đúng**: **+10**.
- **Kết quả khi sai**: **0**, không trừ.
- **Trạng thái game**: `rounds[i]` KHOI_DONG, turn kind `individual-count` (`DEMO`: `KHOI_DONG_RIENG`).
- **Actor**: Thí sinh, Admin/MC (chấm — `R26` §1).
- **Rule mâu thuẫn**: **K-1** — `W26` mô tả Khởi động hoàn toàn khác (12 câu/60s, một lượt).
- **Chưa định nghĩa**: **ai/cái gì kích hoạt timer 3s** — nguồn nói "từ lúc MC đọc xong", hệ thống không biết thời điểm đó. Không nguồn nào định nghĩa trigger. *(U-1)*
- **Chưa định nghĩa**: trả lời đúng sớm có sang câu tiếp ngay không. *(U-22)*

## R-KD-02. Lượt riêng kiểu quỹ thời gian (biến thể O21 — KHÔNG thuộc O26)

- **Nguồn**: `SPEC` §4 kind `individual-timed` (không có trong `F26`/`W26`)
- **Điều kiện đầu vào**: 1 thí sinh trả lời **liên tục** trong `perSeatSeconds` (30-300).
- **Kết quả khi đúng**: `+pointsCorrect` mỗi câu. **Khi sai**: 0, không penalty.
- **Trạng thái game**: `rounds[i]` KHOI_DONG.
- **Actor**: Thí sinh, Admin.
- **Rule mâu thuẫn**: `SPEC` §4 cho "trộn kind tuỳ ý" mà không ràng buộc công bằng — trong cùng một vòng, thí sinh A có thể thi kind khác thí sinh B.
- **Chưa định nghĩa**: `maxQuestions?` optional — hết giờ mà còn câu, hoặc hết câu mà còn giờ, xử lý ra sao.

## R-KD-03. Lượt chung — giành quyền bằng chuông

- **Nguồn**: [`F26` §Khởi động](https://duong-len-dinh-olympia.fandom.com/vi/wiki/Lu%E1%BA%ADt_ch%C6%A1i/Olympia_26) đoạn 3-5 · `R26` §1 · `DEMO` L15 · `SPEC` §4 kind `common-count`
- **Nguyên văn nguồn**: *"Trong lượt chung, các thí sinh trả lời 12 câu hỏi… Thời gian suy nghĩ cho mỗi câu hỏi là 3 giây **tính từ lúc thí sinh giành được quyền trả lời**. Mỗi câu trả lời đúng được 10 điểm. Trả lời sai hoặc bấm chuông mà không có câu trả lời sau 3 giây bị trừ 5 điểm. Thí sinh **có thể bấm chuông trong khi người dẫn chương trình đang đọc câu hỏi**."*
- **Điều kiện đầu vào**: lượt chung; 12 câu; bấm chuông giành quyền; **được bấm ngay khi MC đang đọc**.
- **Kết quả khi đúng**: **+10**.
- **Kết quả khi sai**: **−5** (kể cả "bấm chuông mà không có đáp án sau 3s"); `R26` §1 và §7 thêm: **câu đó KHÔNG mở lại chuông cho người khác**.
- **Trạng thái game**: `rounds[i]` KHOI_DONG kind `common-count` (`DEMO`: `KHOI_DONG_CHUNG`).
- **Actor**: Thí sinh, Admin.
- **Rule mâu thuẫn**: R-GEN-01 (chuông chỉ click chuột); R-TEAM-01 (thi đội khoá cá nhân → thành viên khác vẫn bấm được, trái "không mở lại chuông"); **R-TB-03** (tie-break cấm bấm trước hiệu lệnh, khởi động thì cho phép — xem K-3).
- **Chưa định nghĩa**: mốc bắt đầu đếm 3s = "thời điểm giành được quyền" — hệ thống biết chính xác (timestamp buzz), nhưng "không mở lại chuông cho người khác" **chỉ có trong `R26`, KHÔNG có trong `F26`**. `F26` im lặng về việc này.

## R-KD-04. Cửa sổ chuông rỗng

- **Nguồn**: [`F26` §Khởi động](https://duong-len-dinh-olympia.fandom.com/vi/wiki/Lu%E1%BA%ADt_ch%C6%A1i/Olympia_26) đoạn 5 · `R26` §1, §7
- **Nguyên văn nguồn**: *"Sau 3 giây tính từ thời điểm người dẫn chương trình đọc xong câu hỏi, nếu không có thí sinh nào giành quyền trả lời, câu hỏi đó sẽ bị bỏ qua."*
- **Điều kiện đầu vào**: MC đọc xong; hết `buzzWindowSec` (**3s**) không ai bấm.
- **Kết quả khi thoả**: bỏ qua câu → sang câu tiếp. `R26` §7 thêm: admin có nút next thủ công override.
- **Kết quả khi có người bấm**: chuyển R-KD-03.
- **Trạng thái game**: `rounds[i]` KHOI_DONG kind common.
- **Actor**: Thí sinh, Admin.
- **Rule mâu thuẫn**: —
- **Chưa định nghĩa**: chồng lấn giữa "được bấm khi MC đang đọc" và "cửa sổ 3s sau khi đọc xong" — có phải là **một** cửa sổ liên tục hay hai giai đoạn. Cả `F26` lẫn `R26` đều không nói. *(U-1)*

## R-KD-05. `[MỚI TỪ NGUỒN]` Ba loại câu hỏi Khởi động

- **Nguồn**: [`F26` §Khởi động](https://duong-len-dinh-olympia.fandom.com/vi/wiki/Lu%E1%BA%ADt_ch%C6%A1i/Olympia_26) danh sách cuối
- **Nguyên văn nguồn**: *"Có 3 loại câu hỏi được sử dụng trong phần thi này: (1) Câu hỏi yêu cầu tìm đáp án đúng, điền vào chỗ trống. (2) Câu hỏi lựa chọn: đúng sai, chọn các đáp án cho sẵn. (3) Câu hỏi có hình ảnh hoặc đoạn nhạc gợi ý."*
- **Điều kiện đầu vào**: câu thuộc vòng Khởi động.
- **Kết quả**: mô tả phân loại, không kèm hệ quả tính điểm.
- **Actor**: Setter (soạn), Thí sinh.
- **Rule mâu thuẫn**: —
- **Chưa định nghĩa**: **`R26` KHÔNG ghi nhận phân loại này**. `SPEC` §9 danh sách field của Question không có trường phân loại tương ứng. Loại (2) "lựa chọn đúng sai / chọn đáp án cho sẵn" hàm ý **câu trắc nghiệm** — hệ thống hiện chỉ có `acceptedAnswers` dạng text, không có mô hình lựa chọn. *(U-34)*

## R-KD-06. Ghi nhận đáp án Khởi động

- **Nguồn**: [`F26` §Khởi động](https://duong-len-dinh-olympia.fandom.com/vi/wiki/Lu%E1%BA%ADt_ch%C6%A1i/Olympia_26) đoạn cuối · `R26` §1 dòng "Ghi nhận đáp án"
- **Nguyên văn nguồn**: *"Thí sinh có thể thay đổi đáp án liên tục trước thời điểm người dẫn chương trình công bố đáp án và **đáp án cuối cùng sẽ được ghi nhận**. **Nếu không thay đổi, chương trình sẽ ghi nhận đáp án đầu tiên**."*
- **Điều kiện đầu vào**: thí sinh gửi ≥1 đáp án trước khi MC công bố.
- **Kết quả khi có đổi**: tính **bản CUỐI**. **Khi không đổi**: tính **bản ĐẦU**.
- **Trạng thái game**: `rounds[i]` KHOI_DONG.
- **Actor**: Thí sinh, Server.
- **Rule mâu thuẫn**: khớp nguyên tắc last-wins (R-TT-03) — nhưng mốc cắt ở đây là **"MC công bố đáp án"**, không phải server-timeout. Hai mốc khác nhau; không nguồn nào hợp nhất.
- **Chưa định nghĩa**: hệ thống biết "MC công bố đáp án" lúc nào — cùng vấn đề U-1.

## R-KD-07. Rút đề ngẫu nhiên (chỉ có trong repo)

- **Nguồn**: `SPEC` §4b · `DEF` D22.2 `[✅ user chốt]` · `PRD` FR-4.1 · `US` US-8.4 — **không có trong `F26`/`W26`**
- **Điều kiện đầu vào**: `Turn.drawConfig.mode = 'draw'`; slots `{field, count}`; `noRepeatInMatch: true`.
- **Kết quả khi đúng**: server rút câu **TRONG danh sách đã gán** (snapshot), loại câu `usedInContest`, emit `QUESTIONS_DRAWN` (replay được).
- **Kết quả khi sai (pool thiếu)**: pre-flight **chặn start** (thuật toán 2 bước + `reservePerField` default 2).
- **Trạng thái game**: `LOBBY` (pre-flight) + đầu mỗi turn.
- **Actor**: Admin (chọn đề trước), Server.
- **Rule mâu thuẫn**: R-GEN-06 (no-repeat toàn contest — skip cũng tiêu pool).
- **Chưa định nghĩa**: pool cạn **giữa trận** do skip nhiều lần (pre-flight chỉ chạy trước start). *(U-9)*

---

# PHẦN 2 — VƯỢT CHƯỚNG NGẠI VẬT (VCNV)

## R-VCNV-01. Hàng ngang — trả lời

- **Nguồn**: [`F26` §Vượt chướng ngại vật](https://duong-len-dinh-olympia.fandom.com/vi/wiki/Lu%E1%BA%ADt_ch%C6%A1i/Olympia_26) · `R26` §2 · `DEMO` L18 · `SPEC` §5
- **Nguyên văn nguồn**: *"Các thí sinh trả lời bằng máy tính. Thời gian suy nghĩ cho mỗi từ hàng ngang là 15 giây. Trả lời đúng được 10 điểm."*
- **Điều kiện đầu vào**: hàng ngang được chọn; **15s**; **tất cả thí sinh cùng trả lời bằng máy tính**.
- **Kết quả khi đúng**: **+10** cho mỗi người đúng; miếng ghép tương ứng **mở**.
- **Kết quả khi sai**: 0, không trừ. `F26`: *"**Nếu không trả lời được từ hàng ngang, miếng ghép tương ứng… sẽ không được mở ra**."*
- **Trạng thái game**: `rounds[i]` VCNV.
- **Actor**: Thí sinh, Admin, Server (auto-match).
- **Rule mâu thuẫn**: **K-5** — `W26` nói góc mở khi *"có **ít nhất 1 thí sinh** trả lời đúng"*, `F26` không nêu ngưỡng. Với 1-12 ghế, ngưỡng này quyết định.
- **Chưa định nghĩa**: `R26` không ghi điều kiện mở/không mở miếng ghép — chỉ có trong `F26`/`W26`.

## R-VCNV-02. `[MỚI TỪ NGUỒN]` Chính tả nghiêm ngặt

- **Nguồn**: [`F26` §Vượt chướng ngại vật](https://duong-len-dinh-olympia.fandom.com/vi/wiki/Lu%E1%BA%ADt_ch%C6%A1i/Olympia_26) đoạn "Câu trả lời…"
- **Nguyên văn nguồn**: *"Câu trả lời mà thí sinh đưa ra yêu cầu phải đúng chính tả. **Nếu có bất kỳ sai sót về kí tự, dấu câu, ngữ pháp, câu trả lời của thí sinh đó sẽ không được công nhận.** Đôi khi câu trả lời có ý nghĩa tương đồng **và có cùng tổng số chữ cái** với đáp án của chương trình cũng được chấp nhận."*
- **Điều kiện đầu vào**: chấm câu hàng ngang VCNV.
- **Kết quả khi đúng chính tả hoàn toàn**: công nhận.
- **Kết quả khi sai kí tự/dấu câu/ngữ pháp**: **không công nhận**.
- **Ngoại lệ**: ý nghĩa tương đồng **VÀ cùng tổng số chữ cái** → được chấp nhận.
- **Actor**: Server (auto-match), Admin (override).
- **Rule mâu thuẫn**: **K-6 — mâu thuẫn trực tiếp với R-GEN-04.** Hệ thống chốt normalize **bắt buộc** case-insensitive + trim + collapse space, và **tuỳ chọn bỏ dấu tiếng Việt**. Bỏ dấu tiếng Việt trực tiếp vi phạm *"sai sót về dấu câu → không công nhận"*.
- **Chưa định nghĩa**: quy tắc *"cùng tổng số chữ cái"* — repo **có** field `wordCount` (auto, `PRD` FR-2.2) nhưng đó là **số TỪ**, không phải **số chữ cái**; và không luật nào trong repo dùng nó để chấm. *(U-35)*

## R-VCNV-03. Lượt chọn hàng ngang

- **Nguồn**: [`F26` §Vượt chướng ngại vật](https://duong-len-dinh-olympia.fandom.com/vi/wiki/Lu%E1%BA%ADt_ch%C6%A1i/Olympia_26) · `R26` §2
- **Nguyên văn nguồn**: *"Mỗi thí sinh có tối đa 1 lượt lựa chọn… bắt đầu từ thí sinh ở vị trí số 1. Nếu 1 thí sinh **trước khi lựa chọn** từ hàng ngang mà bấm chuông trả lời Chướng ngại vật và bị loại, thí sinh ở **vị trí tiếp theo** sẽ được lựa chọn… nếu thí sinh ở vị trí cuối cùng hoàn thành lượt lựa chọn mà vẫn còn từ hàng ngang chưa được lựa chọn, lượt lựa chọn sẽ **quay trở lại với thí sinh ở vị trí số 1**."*
- **Điều kiện đầu vào**: đến lượt chọn; bắt đầu từ vị trí 1; mỗi TS tối đa 1 lượt.
- **Kết quả khi đúng**: TS chọn một hàng ngang chưa mở.
- **Kết quả khi TS đã bị loại**: lượt **dồn cho người tiếp theo**; quay vòng về vị trí 1 nếu còn hàng chưa chọn.
- **Trạng thái game**: `rounds[i]` VCNV.
- **Actor**: Thí sinh, Admin.
- **Rule mâu thuẫn**: **`SPEC` §5 cho `rowCount: 4..8` và `DEF` D1 cho 1-12 thí sinh** — "mỗi TS tối đa 1 lượt" không đủ lượt cho 12 người / 4 hàng. Không nguồn nào xử lý. *(U-4)*
- **Chưa định nghĩa**: "vị trí số 1" xác định thế nào khi số ghế ≠ 4.

## R-VCNV-04. Giải chướng ngại vật — điểm theo thời điểm bấm

- **Nguồn**: [`F26` §Vượt chướng ngại vật](https://duong-len-dinh-olympia.fandom.com/vi/wiki/Lu%E1%BA%ADt_ch%C6%A1i/Olympia_26) · [`W26` §Vượt chướng ngại vật](https://vi.wikipedia.org/wiki/%C4%90%C6%B0%E1%BB%9Dng_l%C3%AAn_%C4%91%E1%BB%89nh_Olympia_n%C4%83m_th%E1%BB%A9_26) (khớp nhau) · `R26` §2 · `DEMO` L20 · `SPEC` §5
- **Nguyên văn nguồn**: *"Thí sinh có thể bấm chuông trả lời Chướng ngại vật **bất cứ lúc nào**. Trả lời đúng… trong 1 từ hàng ngang đầu tiên được 60 điểm, trong 2 từ hàng ngang được 50, trong 3 được 40, trong 4 được 30."*
- **Điều kiện đầu vào**: bấm chuông bất kỳ lúc nào; index = số hàng ngang đã mở.
- **Kết quả khi đúng**: **60 / 50 / 40 / 30**; sau gợi ý cuối = **20**.
- **Kết quả khi sai**: **bị loại khỏi phần thi này**. Không nêu trừ điểm.
- **Trạng thái game**: `rounds[i]` VCNV.
- **Actor**: Thí sinh, Admin.
- **Rule mâu thuẫn**: `ATH` — Athena cũ **80/60/40/20** (giá trị cũ, đã bãi bỏ). `SPEC` §5 Zod: `cnvPointsByRowsOpened.length == rowCount (+1)` → `rowCount: 8` cần 9 giá trị; **không nguồn nào nói 9 giá trị đó là bao nhiêu**. *(U-3)*
- **Chưa định nghĩa**: người bị loại có bị trừ điểm hàng ngang đã kiếm không. *(U-24)*

## R-VCNV-05. Ô trung tâm (gợi ý cuối)

- **Nguồn**: [`F26`](https://duong-len-dinh-olympia.fandom.com/vi/wiki/Lu%E1%BA%ADt_ch%C6%A1i/Olympia_26) · [`W26`](https://vi.wikipedia.org/wiki/%C4%90%C6%B0%E1%BB%9Dng_l%C3%AAn_%C4%91%E1%BB%89nh_Olympia_n%C4%83m_th%E1%BB%A9_26) (khớp) · `R26` §2 · `DEMO` L22
- **Nguyên văn nguồn**: *"Sau khi cả 4 từ hàng ngang đã được mở ra mà không có thí sinh nào trả lời Chướng ngại vật, câu hỏi trong gợi ý cuối cùng sẽ được đưa ra ở ô trung tâm… Trả lời đúng câu hỏi ở ô trung tâm được 10 điểm, trả lời sai thì ô trung tâm sẽ không được mở ra. Các thí sinh sẽ có 15 giây suy nghĩ… Trả lời đúng Chướng ngại vật sau gợi ý cuối cùng chỉ được 20 điểm."*
- **Điều kiện đầu vào**: đã mở đủ 4 hàng ngang; chưa ai giải CNV.
- **Kết quả khi đúng**: câu ô trung tâm đúng → **+10** + mở ô; sau đó **15s** → giải CNV đúng = **20**.
- **Kết quả khi sai**: ô trung tâm **không mở**.
- **Trạng thái game**: `rounds[i]` VCNV, cuối vòng.
- **Actor**: Thí sinh, Admin.
- **Rule mâu thuẫn**: điều kiện `F26` là *"cả 4 hàng đã được **mở ra**"*, nhưng R-VCNV-01 nói hàng không ai trả lời đúng thì **không mở** → nếu có hàng không ai đúng thì điều kiện này **không bao giờ thoả**. `W26` dùng điều kiện khác: *"nếu **hết** 4 từ hàng ngang mà vẫn chưa tìm được CNV"* (hết lượt, không phải mở hết). **Mâu thuẫn thực sự, ảnh hưởng luồng vòng.** *(K-7)*
- **Chưa định nghĩa**: ai được trả lời câu ô trung tâm — cả sân hay theo lượt. *(U-25)*

## R-VCNV-06. Bấm chuông CNV giữa timer hàng ngang

- **Nguồn**: `R26` §7 — **nhãn "đề xuất của Claude"**, KHÔNG có trong `F26`/`W26`
- **Điều kiện đầu vào**: timer hàng ngang đang chạy; có người bấm chuông giải CNV.
- **Kết quả khi đúng**: **dừng đồng hồ** → xử lý CNV → đúng: kết thúc vòng.
- **Kết quả khi sai**: loại người bấm → **cho đồng hồ chạy tiếp** với người còn lại.
- **Trạng thái game**: `rounds[i]` VCNV ↔ dừng đồng hồ cục bộ.
- **Actor**: Thí sinh, Admin, Server.
- **Rule mâu thuẫn**: R-GEN-08 (dừng toàn trận) — nguồn không phân biệt rõ việc dừng một đồng hồ cục bộ với việc dừng cả trận.
- **Chưa định nghĩa**: **chưa được user chốt**. `F26` chỉ định nghĩa trường hợp bấm chuông **TRƯỚC KHI lựa chọn** hàng ngang (R-VCNV-03), không định nghĩa bấm **GIỮA** lúc timer chạy. *(U-16)*

## R-VCNV-07. Tất cả thí sinh bị loại khỏi VCNV

- **Nguồn**: `DEF` D13.1 `[✅ user chốt 12/07]` · `R26` §7 · `US` US-4.4 — **không có trong `F26`/`W26`**
- **Điều kiện đầu vào**: mọi thí sinh đều đã trả lời sai CNV.
- **Kết quả khi thoả**: kết thúc lượt/vòng; **không ai được điểm CNV**; hàng ngang chưa hỏi bị bỏ; **mở toàn bộ miếng ghép + công bố CNV = NÚT THỦ CÔNG của admin** (không auto).
- **Kết quả khi không thoả**: vòng chạy tiếp.
- **Trạng thái game**: `rounds[i]` VCNV → `LOBBY` (cửa vào vòng).
- **Actor**: Admin, Viewer.
- **Rule mâu thuẫn**: —
- **Chưa định nghĩa**: nếu admin không bấm nút mở, vòng có tự kết thúc không.

## R-VCNV-08. Gợi ý ký tự (biến thể O11-O12 — chỉ có trong repo)

- **Nguồn**: `SPEC` §5 `revealCharHints` + `hintMap` · `US` US-8.5 — **không có trong `F26`/`W26`**
- **Điều kiện đầu vào**: `revealCharHints: true`; `ObstacleSet.hintMap` khai vị trí ký tự thuộc CNV.
- **Kết quả khi đúng**: render gợi ý ký tự. **Khi hintMap không hợp lệ**: Zod/pre-flight chặn (`SPEC` §12).
- **Actor**: Setter, Thí sinh, Viewer.
- **Rule mâu thuẫn**: không thuộc luật O26.
- **Chưa định nghĩa**: gợi ý hiện lúc nào (khi mở hàng ngang hay sau khi hết giờ). *(U-32)*

---

# PHẦN 3 — TĂNG TỐC

## R-TT-01. Ranked-speed — điểm theo thứ hạng tốc độ

- **Nguồn**: [`F26` §Tăng tốc](https://duong-len-dinh-olympia.fandom.com/vi/wiki/Lu%E1%BA%ADt_ch%C6%A1i/Olympia_26) · [`W26` §Tăng tốc](https://vi.wikipedia.org/wiki/%C4%90%C6%B0%E1%BB%9Dng_l%C3%AAn_%C4%91%E1%BB%89nh_Olympia_n%C4%83m_th%E1%BB%A9_26) (khớp) · `R26` §3 · `DEMO` L25-27 · `SPEC` §6
- **Nguyên văn nguồn**: *"Có 4 câu hỏi với thời gian suy nghĩ cho mỗi câu hỏi lần lượt là 20, 20, 30 và 30 giây. Các thí sinh trả lời bằng máy tính. Trả lời đúng và nhanh nhất được 40 điểm… thứ 2 được 30… thứ 3 được 20… thứ 4 được 10 điểm."*
- **Điều kiện đầu vào**: **4 câu**; **20/20/30/30s**; tất cả cùng trả lời bằng máy tính, **KHÔNG bấm chuông**.
- **Kết quả khi đúng**: **40/30/20/10** theo thứ hạng tốc độ **trong số người đúng**.
- **Kết quả khi sai**: **0**, không trừ.
- **Trạng thái game**: `rounds[i]` TANG_TOC format `ranked-speed`.
- **Actor**: Thí sinh, Server (timestamp), Admin.
- **Rule mâu thuẫn**: `ATH` — Athena cũ 30s/câu đồng nhất (đã bãi bỏ). `SPEC` §1.4/§6: `rankPoints.length == số đơn vị điểm khai báo` + `DEF` D1 (1-12 ghế) → **12 ghế cần 12 giá trị, không nguồn nào định nghĩa**. *(U-2)*
- **Chưa định nghĩa**: thang điểm cho số thí sinh ≠ 4.

## R-TT-02. Đồng thời gian

- **Nguồn**: [`F26` §Tăng tốc](https://duong-len-dinh-olympia.fandom.com/vi/wiki/Lu%E1%BA%ADt_ch%C6%A1i/Olympia_26) đoạn cuối · [`W26`](https://vi.wikipedia.org/wiki/%C4%90%C6%B0%E1%BB%9Dng_l%C3%AAn_%C4%91%E1%BB%89nh_Olympia_n%C4%83m_th%E1%BB%A9_26) · `R26` §3, §7 · `DEF` D13.3 · `SPEC` §6 `tieRule`
- **Nguyên văn `F26`**: *"Trong trường hợp có nhiều thí sinh cùng trả lời đúng **trong cùng một khoảng thời gian**, những thí sinh đó cùng nhận được một mức điểm tùy theo mức độ trả lời nhanh câu hỏi đó."*
- **Nguyên văn `W26`**: *"Nếu có 2 thí sinh trở lên cùng trả lời đúng trong cùng một thời gian hệ thống ghi nhận (**tính đến 2 chữ số thập phân**), họ sẽ cùng giành được số điểm tương ứng."*
- **Điều kiện đầu vào**: ≥2 TS đúng với cùng timestamp.
- **Kết quả khi thoả**: **cùng nhận một mức điểm** (ví dụ 40/40/20/10).
- **Kết quả khi khác timestamp**: xếp hạng bình thường.
- **Actor**: Server.
- **`K-8` ĐÃ PHÂN XỬ 2026-07-26 — GIỮ `ms`**, theo quyết định của chủ dự án, lý do **máy tính dễ tính toán** (so hai số nguyên, không làm tròn, không số thực).
  - **Lập luận cũ của `K-8` sai, kết luận đúng vì lý do khác.** `K-8` cũ loại `W26` *"theo D8"*, nhưng **D8 chỉ cho phép loại `W26` ở chỗ `F26` nói KHÁC** — mà `F26` **im lặng** về độ phân giải nên không có gì để loại bằng. Con số `ms` đến từ `R26` §7 = **file nháp trong repo**, không phải nguồn; `DEF` D13.3 chỉ chốt **hành vi** (*"đồng thời gian → cùng mức điểm"*). ⇒ Căn cứ thật của `ms` là **quyết định kỹ thuật**, không phải nguồn luật. Ghi rõ để không ai trích `D8` như căn cứ cho con số này.
  - **Tiền lệ chương trình đi NGƯỢC lựa chọn này** — hai trang Wikipedia xác nhận độc lập: `W26` (*"cùng một thời gian hệ thống ghi nhận, **tính đến 2 chữ số thập phân**"*) và [`W` bài tổng](https://vi.wikipedia.org/wiki/%C4%90%C6%B0%E1%BB%9Dng_l%C3%AAn_%C4%91%E1%BB%89nh_Olympia) (*"thứ tự thời gian **được tính đến hàng phần trăm**"*).
  - **Cái giá đã biết và đã chấp nhận**: cửa sổ hoà **hẹp hơn 10 lần** thực tế chương trình. Vì thời gian do tay người bấm, ở độ phân giải ms hệ thống gần như **luôn** phân định được người thắng ⇒ điều khoản *"cùng nhận một mức điểm"* đúng về hành vi nhưng **rất ít khi kích hoạt**.
  - **Đường thoát nếu đổi ý**: đổi `tieRule` sang mức 10 ms — vẫn là số nguyên (chia nguyên cho 10 rồi so), **không** đánh đổi độ phức tạp tính toán, và **không** phải sửa luật hay decision table.
  - `SPEC` §6 option `tieRule: 'microsecond'` **giữ lại như cấu hình hợp lệ** nhưng **KHÔNG được dùng cho preset `O26_DEFAULT@1`** — cùng cách xử lý K-12/K-13.
  - **Bề rộng tiền lệ của điều khoản hoà**: có từ **Olympia 7** ([Fandom — Tăng tốc](https://duong-len-dinh-olympia.fandom.com/vi/wiki/T%C4%83ng_t%E1%BB%91c)), khoảng **19 mùa** liên tục — nó là điều khoản thật, không phải trường hợp giả định.
- **Nhảy bậc khi share-high — ĐÃ ĐÓNG**: sau nhóm k người đồng hạng ở bậc n, người kế nhận bậc **n+k** (`GRR-032`, standard competition ranking). Khớp đúng ví dụ 40/40/**20**/10 mà nguồn hàm ý.

## R-TT-03. Last-wins (chỉ có trong repo)

- **Nguồn**: `SPEC` §6 khối `LAST-WINS` · `DEF` D17.2, D20.1 `[✅ user chốt]` · `CLAUDE.md` §UX · `PRD` FR-5.1 — **`F26`/`W26` không mô tả cơ chế này cho Tăng tốc**
- **Điều kiện đầu vào**: vòng Tăng tốc; thí sinh gửi/gửi lại tự do; KHÔNG khoá input sau khi gửi.
- **Kết quả khi đúng**: tính **BẢN CUỐI CÙNG**; ranking theo **server-received timestamp của bản cuối**.
- **Kết quả các ngoại lệ**:
  - Nội dung **y hệt** bản trước (so sau trim, per seat) → **không cập nhật timestamp**.
  - Submission **RỖNG** (toàn whitespace sau trim) → **SKIP**, giữ bản trước (`DEF` D20.1).
  - Tới **sau server-timeout** → **loại**.
- **Actor**: Thí sinh, Server, Admin.
- **Rule mâu thuẫn**: R-KD-06 — Khởi động dùng mốc "MC công bố đáp án" + quy tắc "không đổi → tính bản ĐẦU"; Tăng tốc dùng mốc server-timeout + luôn bản cuối. `F26` chỉ nêu quy tắc đổi-đáp-án cho **Khởi động** và **Về đích**, không cho Tăng tốc.
- **Chưa định nghĩa**: admin có thấy lịch sử các bản trước để phân xử không (`SPEC` chỉ nói "collapse per-seat-latest"). *(U-31)*

## R-TT-04. `[MỚI TỪ NGUỒN]` Bốn loại câu hỏi Tăng tốc

- **Nguồn**: [`F26` §Tăng tốc](https://duong-len-dinh-olympia.fandom.com/vi/wiki/Lu%E1%BA%ADt_ch%C6%A1i/Olympia_26) · [`W26` §Tăng tốc](https://vi.wikipedia.org/wiki/%C4%90%C6%B0%E1%BB%9Dng_l%C3%AAn_%C4%91%E1%BB%89nh_Olympia_n%C4%83m_th%E1%BB%A9_26) (chi tiết hơn)
- **Nguyên văn `F26`**: *"Câu hỏi nhìn nhanh · Câu hỏi sắp xếp · Câu hỏi suy luận · Câu hỏi đoạn băng"*
- **`W26` chi tiết**: câu 1 Nhìn-Đáp (20s) · câu 2 Sắp xếp-Lựa chọn (20s, xếp ảnh theo thứ tự **hoặc** chọn ảnh ứng với đáp án A-F) · câu 3 IQ (30s) · câu 4 dữ kiện (30s, **video clip, dữ kiện đưa ra theo độ khó giảm dần**)
- **Điều kiện đầu vào**: câu thuộc vòng Tăng tốc, theo thứ tự 1→4.
- **Kết quả**: phân loại + gắn thời gian theo vị trí câu.
- **Actor**: Setter, Thí sinh.
- **Rule mâu thuẫn**: `R26` §3 chỉ liệt kê tên 4 loại trong ghi chú, **không mô hình hoá**. `SPEC` §9 danh sách field Question **không có trường loại câu Tăng tốc** → không khai được câu "sắp xếp" (cần thứ tự) hay "lựa chọn A-F" (cần danh sách đáp án).
- **Chưa định nghĩa**: **câu "sắp xếp" và "lựa chọn ảnh" cần mô hình dữ liệu hoàn toàn khác `acceptedAnswers` text** — không nguồn repo nào đề cập. *(U-36)*

## R-TT-05. Clue-buzz (biến thể — chỉ có trong repo)

- **Nguồn**: `SPEC` §6 khối `clue-buzz` · `DEMO` L29 · `US` US-8.6
- **Điều kiện đầu vào**: `format: 'clue-buzz'`; dữ kiện mở dần mỗi `clueIntervalSeconds`; bấm chuông bất kỳ lúc nào; câu phải có `clues[]` (2..maxClues).
- **Kết quả khi đúng**: điểm theo **mốc dữ kiện đang mở khi bấm** (`cluePoints`, length == `maxClues`; câu ít dữ kiện dùng **PREFIX**).
- **Kết quả khi sai**: `wrongLocksOut: true` → khoá người bấm sai.
- **Actor**: Thí sinh, Admin, Setter.
- **Rule mâu thuẫn**: không thuộc O26 — nhưng **gần với `W26` câu 4** (*"dữ kiện được đưa ra theo độ khó giảm dần"*), khác ở chỗ O26 không có cơ chế bấm chuông ở Tăng tốc. `SPEC` §6 Zod refine: `teamLockout` chỉ có nghĩa khi `wrongLocksOut=true`.
- **Chưa định nghĩa**: câu có 2 clues trong khi `maxClues=4` — xử lý giữa trận.

---

# PHẦN 4 — VỀ ĐÍCH

## R-VD-01. Chọn gói câu

- **Nguồn**: [`F26` §Về đích](https://duong-len-dinh-olympia.fandom.com/vi/wiki/Lu%E1%BA%ADt_ch%C6%A1i/Olympia_26) · `R26` §4 · `DEMO` L32 · `SPEC` §7
- **Nguyên văn `F26`**: *"Có 2 mức điểm 20, 30 điểm… Mỗi thí sinh có một lượt lựa chọn 3 câu hỏi 20, 30 điểm để tạo thành một gói điểm của mình."*
- **Điều kiện đầu vào**: đến lượt; chọn gói trước lượt thi.
- **Kết quả khi đúng**: gói **3 câu** từ 2 mức **{20, 30}**.
- **Kết quả khi chưa chọn lúc tới lượt**: `R26` §7 + `DEF` D13.4 — **admin chọn hộ**, default 20/20/20.
- **Actor**: Thí sinh, Admin.
- **Rule mâu thuẫn**: **K-2** — `W26` ghi mức **20 và 40**, thời gian 40đ = 30s (và tự mâu thuẫn với bảng thực hành ghi 30đ). `ATH` — Athena cũ 10/20/30 (đã bãi bỏ). `SPEC` §7 cho `packageMode: 'preset'` giá trị tuỳ ý (30/50/70/90...) hoặc `custom-build`.
- **Chưa định nghĩa**: thí sinh có được đổi gói sau khi chọn không. *(U-26)*

## R-VD-02. Thứ tự lượt thi

- **Nguồn**: [`F26` §Về đích](https://duong-len-dinh-olympia.fandom.com/vi/wiki/Lu%E1%BA%ADt_ch%C6%A1i/Olympia_26) danh sách "Thứ tự tham gia" · [`W26`](https://vi.wikipedia.org/wiki/%C4%90%C6%B0%E1%BB%9Dng_l%C3%AAn_%C4%91%E1%BB%89nh_Olympia_n%C4%83m_th%E1%BB%A9_26) · `R26` §4
- **Nguyên văn `F26`**: *"Lượt 1: Thí sinh có điểm số cao nhất sau phần thi Tăng tốc, và có **vị trí đứng thấp nhất** trong các thí sinh có cùng điểm số. Lượt 2: … **Điểm số được tính tại thời điểm sau khi thí sinh ở lượt 1 hoàn thành phần thi**…"*
- **Điều kiện đầu vào**: bắt đầu vòng, và sau **mỗi lượt hoàn thành**.
- **Kết quả khi đúng**: người điểm cao nhất **tại thời điểm xếp lượt** đi trước (tính lại sau mỗi lượt).
- **Kết quả khi hoà**: `F26` *"vị trí đứng thấp nhất"* / `W26` *"số thứ tự đứng nhỏ nhất"*.
- **Actor**: Server, Admin.
- **Rule mâu thuẫn**: `F26` "thấp nhất" vs `W26` "nhỏ nhất" — có thể cùng nghĩa (số thứ tự nhỏ) nhưng `F26` dùng từ mơ hồ.
- **Chưa định nghĩa**: "vị trí đứng" là ghế vật lý hay thứ hạng; với 1-12 ghế càng mơ hồ. *(U-27)*

## R-VD-03. Trả lời câu của mình

- **Nguồn**: [`F26` §Về đích](https://duong-len-dinh-olympia.fandom.com/vi/wiki/Lu%E1%BA%ADt_ch%C6%A1i/Olympia_26) · `R26` §4 · `DEMO` L33
- **Nguyên văn**: *"Thời gian suy nghĩ và trả lời cho câu hỏi 20 điểm là 15 giây, câu hỏi 30 điểm là 20 giây… Thí sinh nếu trả lời đúng ghi được điểm của câu hỏi đó, nếu trả lời sai thì 1 trong 3 thí sinh còn lại sẽ giành quyền trả lời…"*
- **Điều kiện đầu vào**: đến lượt; câu có `value`; thời gian default 20đ→15s, 30đ→20s (per-question `timeSeconds` override thắng).
- **Kết quả khi đúng**: **+ giá trị câu**.
- **Kết quả khi sai/hết giờ**: **0** + **mở quyền cướp**.
- **Ghi nhận đáp án**: *"trong khoảng thời gian quy định, thí sinh có thể thay đổi đáp án liên tục. Chương trình sẽ ghi nhận **đáp án cuối cùng** sau khi hết giờ."*
- **Actor**: Thí sinh, Admin.
- **Rule mâu thuẫn**: R-GEN-02.
- **Chưa định nghĩa**: —

## R-VD-04. Câu hỏi thực hành

- **Nguồn**: [`F26` §Về đích](https://duong-len-dinh-olympia.fandom.com/vi/wiki/Lu%E1%BA%ADt_ch%C6%A1i/Olympia_26) · [`W26`](https://vi.wikipedia.org/wiki/%C4%90%C6%B0%E1%BB%9Dng_l%C3%AAn_%C4%91%E1%BB%89nh_Olympia_n%C4%83m_th%E1%BB%A9_26) bảng · `R26` §4
- **Nguyên văn `F26`**: *"chương trình sẽ giới thiệu các dụng cụ liên quan… Đối với câu hỏi 20 điểm, thời gian suy nghĩ là 15 giây và thời gian thực hành là 30 giây. Đối với câu hỏi 30 điểm, … 20 giây và … 60 giây… [khi cướp] Đối với câu hỏi 20 điểm, thời gian thực hành là 20 giây. Đối với câu hỏi 30 điểm, … 40 giây."*
- **Điều kiện đầu vào**: câu được đánh dấu là câu thực hành.
- **Kết quả khi đạt yêu cầu**: ghi điểm câu. **Khi không đạt**: mở quyền cướp; người cướp làm lại với thời gian thực hành ngắn hơn.
- **Bảng thời gian**: 20đ → 15s nghĩ + 30s thực hành (cướp: 20s) · 30đ → 20s nghĩ + 60s thực hành (cướp: 40s).
- **Actor**: Thí sinh, MC/ban cố vấn/khách mời (giới thiệu dụng cụ — `W26`), Admin (chấm "đạt yêu cầu").
- **Rule mâu thuẫn**: `W26` thêm *"Một số tập **có thể** có 1 câu hỏi thực hành"* (không bắt buộc mỗi trận).
- **`U-6` ĐÃ ĐÓNG 2026-07-26** — ✅ chủ dự án chốt **bổ sung trường khai câu thực hành**. Năm trường, đặc tả đầy đủ ở `docs/game-rules.md` GR-019 §Mô hình dữ liệu:

| Trường | Mặc định | Ghi chú |
|---|---|---|
| `isPractical` | `false` | Chỉ hợp lệ với câu pool **Về đích** |
| `practiceSeconds` | **30** (20đ) · **60** (30đ) | Metadata **từng câu** như `timeSeconds` (G-4) |
| `stealPracticeSeconds` | **20** (20đ) · **40** (30đ) | Thời gian thực hành của **người cướp** — khác cửa sổ bấm chuông 5s |
| `equipmentNote` | rỗng | Dụng cụ BTC phải mang; máy chỉ nhắc, **không kiểm được** |
| `acceptanceCriteria` | rỗng | Thay `acceptedAnswers` ở câu thực hành; bảo mật ngang `acceptedAnswers` (admin + MC) |

  Chấm *"thực hành đạt yêu cầu"* vẫn là **đánh giá của người**, không so khớp text (nguyên tắc nền điểm 1). Quyết định này đóng luôn **`GRR-050/051`** (câu hỏi thực hành **thuộc v1**).

## R-VD-05. Cướp quyền (steal)

- **Nguồn**: [`F26` §Về đích](https://duong-len-dinh-olympia.fandom.com/vi/wiki/Lu%E1%BA%ADt_ch%C6%A1i/Olympia_26) · [`W26`](https://vi.wikipedia.org/wiki/%C4%90%C6%B0%E1%BB%9Dng_l%C3%AAn_%C4%91%E1%BB%89nh_Olympia_n%C4%83m_th%E1%BB%A9_26) (khớp) · `R26` §4 · `DEMO` L37, L284-293 · `SPEC` §7
- **Nguyên văn `F26`**: *"1 trong 3 thí sinh còn lại sẽ giành quyền trả lời bằng cách bấm chuông nhanh **trong 5 giây**. Thí sinh trả lời đúng sẽ **giành được điểm từ thí sinh trả lời sai**, thí sinh bấm chuông mà trả lời sai sẽ bị trừ **một nửa** số điểm của câu hỏi."*
- **Điều kiện đầu vào**: người thi chính sai/hết giờ; 3 TS còn lại bấm chuông trong **5s**.
- **Kết quả khi đúng**: **+giá trị câu, LẤY TỪ điểm người trả lời sai** (`stealMode: 'transfer'` — người sai −value, người cướp +value).
- **Kết quả khi sai**: **−½ giá trị câu**.
- **Ghi nhận đáp án**: *"Với thí sinh giành quyền trả lời, chương trình sẽ chỉ ghi nhận **đáp án đầu tiên**"* — ngược với người thi chính (bản cuối).
- **Actor**: Thí sinh (3 người còn lại), Admin.
- **Rule mâu thuẫn**:
  - `SPEC` §7 có option `stealMode: 'add'` (cộng không trừ người sai) — trái O26 default.
  - `ATH` — Athena cũ không transfer (đã bãi bỏ).
  - `SPEC` §2b: **CẤM cùng đội cướp** (hard-code) — chỉ áp khi thi đội.
  - `R26` §7: câu bị skip khi đang mở cửa sổ cướp → **huỷ cửa sổ**, không ai cộng/trừ (nhãn "đề xuất").
- **`U-20` ĐÃ ĐÓNG 2026-07-26 — phương án (b): `phạt = value / 2` bằng PHÉP CHIA SỐ NGUYÊN** (cắt về 0). Ví dụ `value = 25` ⇒ phạt **12** ⇒ điểm đổi **−12**. ✅ chủ dự án chốt.
  - ⚠️ **Cẩn thận dấu**: phải làm tròn trên **độ lớn** rồi mới gắn dấu âm. `floor(−12,5) = −13` là **sai** — nặng hơn, ngược quyết định.
  - Lý do: đúng bằng hành vi mặc định của chia số nguyên (không tốn code); chỗ nguồn im lặng thì chọn hướng **nhẹ hơn** cho thí sinh; **không** hard-code ràng buộc *"giá trị phải chẵn"* vào validation ⇒ giữ được lời hứa luật tuỳ biến. Phương án cũ của `R26` §7 (*Zod ép chẵn*) bị loại vì chính lý do thứ ba.
  - **Dưới `O26_DEFAULT@1` tình huống này KHÔNG TỒN TẠI**: 20/30 ⇒ nửa là 10/15, đều nguyên. Quy ước chỉ chạm contest cấu hình ngoài luật 2026.
- **Chưa định nghĩa**: "3 TS còn lại" với 1-12 ghế *(U-5)*; người bị cướp có xuống âm được không *(U-28)* — *lưu ý `U-28` đã được nguyên tắc nền điểm 7 trả lời: **điểm được phép âm, không có sàn***.

## R-VD-06. Ngôi sao hy vọng (NSHV)

- **Nguồn**: [`F26` §Về đích](https://duong-len-dinh-olympia.fandom.com/vi/wiki/Lu%E1%BA%ADt_ch%C6%A1i/Olympia_26) · [`W26`](https://vi.wikipedia.org/wiki/%C4%90%C6%B0%E1%BB%9Dng_l%C3%AAn_%C4%91%E1%BB%89nh_Olympia_n%C4%83m_th%E1%BB%A9_26) · `R26` §4 · `SPEC` §7 · `US` US-5.5
- **Nguyên văn `F26`**: *"Mỗi thí sinh được đặt ngôi sao hy vọng 1 lần. Trả lời đúng… được gấp đôi số điểm. Trả lời sai sẽ bị trừ đi số điểm của câu hỏi, **kể cả các thí sinh còn lại có giành quyền trả lời hay không**. Thí sinh phải đặt ngôi sao hy vọng **trước khi câu hỏi được đọc lên** bởi người dẫn chương trình hoặc hiện lên trên màn hình."*
- **Điều kiện đầu vào**: 1 lần/thí sinh/trận; đặt trước khi câu được đọc/hiện.
- **Kết quả khi đúng**: **×2** giá trị câu.
- **Kết quả khi sai**: **−giá trị câu**, kể cả có người cướp hay không.
- **Actor**: Thí sinh.
- **Rule mâu thuẫn**: `SPEC` §2b — thi đội: **1 lần/ĐƠN VỊ ĐIỂM/trận** (`DEF` D17.4). `R26` §7: NSHV áp sang câu thay thế khi câu bị skip (nhãn "đề xuất", chưa chốt *(U-18)*).
- **`U-8` ĐÃ ĐÓNG 2026-07-26 — mất ĐÚNG MỘT LẦN `value`.** Hình phạt NSHV **thay thế** phần nợ của transfer, không cộng dồn.
  - Căn cứ: `F26` dòng 90 *"Trả lời sai sẽ bị trừ đi số điểm của câu hỏi, **kể cả các thí sinh còn lại có giành quyền trả lời hay không**"*. Mệnh đề này tồn tại **đúng để** nói số học của người thi chính **độc lập** với hành động của người cướp ⇒ có cướp hay không, kết quả của họ **như nhau**. Đây là **quy định của nguồn**, không phải suy diễn.
  - Nó cũng giải thích vì sao mệnh đề đó phải có: ở câu **không** NSHV, người thi chính sai mà không ai cướp thì mất **0**; có NSHV thì mất `value` **dù không ai cướp**.

| Tình huống (A có NSHV, bị chấm Sai) | A | Người cướp B |
|---|---|---|
| Không ai cướp | **−value** | — |
| B cướp **đúng** | **−value** | **+value** |
| B cướp **sai** | **−value** | **−½ value** |

  - **Hai lỗi tài liệu từng làm `U-8` trông khó hơn thực tế**: (1) bảng cũ ở GR-021 C5 viết như thể *"TS NSHV"* và *"người thi chính"* là **hai người**, trong khi NSHV do chính người đang thi đặt lên câu của mình; (2) `GR-018` **đã ghi đúng đáp án** (*"−30 từ NSHV, không sinh sự kiện Sai riêng"*) mà GR-020/GR-021 chưa đồng bộ.
  - **Người cướp không dùng được NSHV trên câu đang cướp** — NSHV phải đặt trước khi câu được đọc; người cướp chỉ bấm chuông sau khi câu đã bị trả lời sai.

## R-VD-07. Pre-flight custom-build (chỉ có trong repo)

- **Nguồn**: `SPEC` §7 gạch đầu dòng cuối
- **Điều kiện đầu vào**: `packageMode: 'custom-build'`.
- **Kết quả khi đủ**: cần ≥ `số lượt × questionsPerTurn` câu cho **MỖI mốc** trong `valueChoices` (worst-case mọi người cùng chọn một mốc).
- **Kết quả khi thiếu**: pre-flight chặn — hoặc bật `allowValueExhaustion` (mốc hết câu → khoá lựa chọn, UI hiện mốc mờ).
- **Actor**: Admin, Thí sinh.
- **Rule mâu thuẫn**: — · **Chưa định nghĩa**: —

---

# PHẦN 5 — CÂU HỎI PHỤ / TIE-BREAK

## R-TB-01. Kích hoạt

- **Nguồn**: [`F26` §Câu hỏi phụ](https://duong-len-dinh-olympia.fandom.com/vi/wiki/Lu%E1%BA%ADt_ch%C6%A1i/Olympia_26) · [`W26` §Câu hỏi phụ](https://vi.wikipedia.org/wiki/%C4%90%C6%B0%E1%BB%9Dng_l%C3%AAn_%C4%91%E1%BB%89nh_Olympia_n%C4%83m_th%E1%BB%A9_26) · `R26` §5 · `SPEC` §8 · `DEF` D13.5 `[✅ user chốt]`
- **Nguyên văn `F26`**: *"Sau phần thi Về đích, các thí sinh có cùng số điểm sẽ bước vào phần thi Câu hỏi phụ."*
- **Điều kiện đầu vào**: hoà điểm; `DEF` D13.5 giới hạn **chỉ vị trí NHẤT** (`tieBreakPositions` default `[1]`).
- **Kết quả khi có hoà**: → `TIE_BREAK`. **Khi không hoà**: → `FINISHED`.
- **Actor**: Server, Admin.
- **Rule mâu thuẫn**: `W26` nêu **điều kiện thứ hai**: *"Sau 3 cuộc thi tuần của cùng một tháng hoặc sau 3 cuộc thi tháng của cùng một quý, có nhiều thí sinh có cùng số điểm nhì cao nhất"* — thuộc mô hình giải đấu nhiều trận, đã là **non-goal NG-2**. `SPEC` §3: TieBreakConfig chỉ hợp lệ ở **cuối playlist**.
- **Chưa định nghĩa**: `F26` nói "các thí sinh có cùng số điểm" không giới hạn vị trí; `DEF` D13.5 giới hạn vị trí nhất → **repo hẹp hơn nguồn**.

## R-TB-02. Thể thức

- **Nguồn**: [`F26` §Câu hỏi phụ](https://duong-len-dinh-olympia.fandom.com/vi/wiki/Lu%E1%BA%ADt_ch%C6%A1i/Olympia_26) · `R26` §5 · `DEMO` L39
- **Nguyên văn `F26`**: *"Các thí sinh trả lời 3 câu hỏi. Thời gian suy nghĩ cho mỗi câu hỏi là 15 giây. Thí sinh bấm chuông nhanh nhất trả lời đúng sẽ là thí sinh có số điểm cao nhất… **Nếu trả lời sai, các thí sinh sẽ bước sang câu hỏi tiếp theo.**"*
- **Điều kiện đầu vào**: **3 câu**, **15s**/câu, bấm chuông giành quyền.
- **Kết quả khi đúng**: bấm nhanh nhất + đúng → **thắng**.
- **Kết quả khi sai**: **cả nhóm sang câu TIẾP THEO** — không mở lại cùng câu.
- **Actor**: Thí sinh trong nhóm hoà, Admin.
- **Rule mâu thuẫn**: **K-9 — mâu thuẫn nội tại trong `R26`**: §5 nói "sang câu tiếp" (khớp `F26` ✅), §7 nói *"người bấm sai bị loại khỏi CÂU đó, những người còn lại **thi tiếp cùng câu**"* (nhãn "đề xuất"). **Nguồn ngoài đã phân xử: §5 đúng, §7 sai — cần sửa `R26` §7.**
- **Chưa định nghĩa**: `R26` §5 nói câu phụ **không cộng vào điểm trận**; `F26` diễn đạt lạ (*"sẽ là thí sinh có số điểm cao nhất bằng với số điểm của thí sinh còn lại"*) không nói rõ có cộng điểm không. `ATH` — Athena cũ +1đ/câu (đã bãi bỏ).

## R-TB-03. Bấm chuông trước hiệu lệnh

- **Nguồn**: [`F26` §Câu hỏi phụ](https://duong-len-dinh-olympia.fandom.com/vi/wiki/Lu%E1%BA%ADt_ch%C6%A1i/Olympia_26) đoạn cuối · `R26` §5
- **Nguyên văn**: *"Trong một câu hỏi, nếu có thí sinh bấm chuông trả lời **trước khi có hiệu lệnh của người dẫn chương trình**, thí sinh đó sẽ bị **mất quyền trả lời câu hỏi**."*
- **Điều kiện đầu vào**: bấm chuông trước hiệu lệnh MC.
- **Kết quả khi thoả**: **mất quyền trả lời câu đó**. **Khi bấm hợp lệ**: theo R-TB-02.
- **Actor**: Thí sinh, MC, Admin.
- **Rule mâu thuẫn**: **K-3** — R-KD-03 cho phép bấm **khi MC đang đọc** ở Khởi động; tie-break thì cấm. Hai luật ngược nhau về cùng hành vi vật lý. Cả hai đều từ `F26` nên **là chủ đích của chương trình**, không phải lỗi tài liệu — nhưng repo chưa mô hình hoá sự khác biệt.
- **Chưa định nghĩa**: hệ thống biết "hiệu lệnh MC" lúc nào. *(U-1)*

## R-TB-04. Hết câu chưa phân định

- **Nguồn**: [`F26`](https://duong-len-dinh-olympia.fandom.com/vi/wiki/Lu%E1%BA%ADt_ch%C6%A1i/Olympia_26) · [`W26`](https://vi.wikipedia.org/wiki/%C4%90%C6%B0%E1%BB%9Dng_l%C3%AAn_%C4%91%E1%BB%89nh_Olympia_n%C4%83m_th%E1%BB%A9_26) (khớp) · `R26` §5, §7 · `DEF` D13.6 · `SPEC` §8
- **Nguyên văn**: *"Sau 3 câu hỏi, nếu không tìm được thí sinh thắng cuộc, các thí sinh sẽ phải **bốc thăm** để chọn ra thí sinh thắng cuộc."*
- **Điều kiện đầu vào**: hết 3 câu (`maxQuestions`) vẫn hoà.
- **Kết quả khi thoả**: **bốc thăm** (`exhaustedFallback` default `'random-draw'`); `R26` thêm: hệ thống random, **admin xác nhận**.
- **Actor**: Server, Admin.
- **Rule mâu thuẫn**: `SPEC` §8 có option `'admin-decides'` — trao quyền cho người, trái nguồn.
- **Chưa định nghĩa**: pool câu phụ cạn **giữa** tie-break. *(U-10)*

## R-TB-05. Nhiều nhóm hoà (chỉ có trong repo, chưa chốt)

- **Nguồn**: `R26` §7 — **nhãn "đề xuất của Claude"** · `SPEC` §8
- **Điều kiện đầu vào**: hoà nhiều vị trí, hoặc nhóm hoà >2 người.
- **Kết quả**: chạy tie-break **per nhóm hoà, theo thứ tự vị trí** — `(tiedSeats[], targetPosition) → winner`.
- **Actor**: Thí sinh, Admin.
- **Rule mâu thuẫn**: `DEF` D13.5 chốt chỉ vị trí NHẤT → chỉ xảy ra khi admin đổi `tieBreakPositions`. `W26` nói tie-break có thể có **2 hoặc 3 thí sinh** cùng điểm cao nhất → nhóm >2 người là **có thật trong luật**.
- **Chưa định nghĩa**: **chưa được user chốt**. *(U-17)*

---

# PHẦN 6 — LUẬT XUYÊN VÒNG

## R-GEN-01. Chuông chỉ nhận click chuột (quy tắc sản phẩm, không phải luật chơi)

- **Nguồn**: `CLAUDE.md` §UX BẮT BUỘC · `PRD` FR-5.1 · `US` US-5.2 — **không có trong `F26`/`W26`**
- **Điều kiện đầu vào**: mọi vòng có chuông.
- **Kết quả**: nút chuông nhận **click chuột**; hotkey khác giữ nguyên (Enter gửi, 1-8 hàng, Esc xoá). **Không gán hotkey cho chuông.**
- **Actor**: Thí sinh.
- **Rule mâu thuẫn**: `red-team.md` VÒNG 3 **H-F3** — mâu thuẫn "keyboard-only trọn trận"; đã xử lý, trade-off accessibility **được ghi nhận là chấp nhận**.
- **Chưa định nghĩa**: thí sinh không dùng được chuột thi thế nào. *(U-33)*

## R-GEN-02. `timeSeconds` là metadata từng câu

- **Nguồn**: `DEF` D8 `[✅ user chốt]` · `R26` §đầu · `PRD` FR-2.2 · `CLAUDE.md`
- **Điều kiện đầu vào**: mọi câu hỏi.
- **Kết quả**: chọn câu theo **MỨC ĐIỂM**, thời gian **theo câu**; cùng 20đ có thể 15s và 40s. **Khi câu không khai**: fallback `defaultTimeByValue`.
- **Actor**: Setter, Server.
- **Rule mâu thuẫn**: `ATH` — Athena tính `time = value/2 + 5` (suy thời gian TỪ điểm), đã bãi bỏ. Nguồn ngoài (`F26`/`W26`) **gắn chặt thời gian với mức điểm** (20đ→15s, 30đ→20s) — repo tổng quát hoá vượt nguồn, đây là **chủ đích**, không phải sai.
- **Chưa định nghĩa**: —

## R-GEN-03. Chấm điểm — ĐÚNG/SAI DO ADMIN QUYẾT ĐỊNH (trận chính thức)

> ⚠️ **Quy tắc nền.** Chốt bởi chủ dự án 2026-07-23 — **ghi đè D4**: `autoJudge` **chỉ tồn tại ở bản tự luyện tập**, không tồn tại ở trận chính thức.

- **Nguồn**: **quyết định chủ dự án 2026-07-23** (supersede `DEF` D4) · nền cũ: `DEF` D4 `[✅ 12/07]` · `PRD` FR-4.2 · `P06` §Chấm · `US` US-4.2 · `SPEC` §13 (`matchPurpose`)
- **Điều kiện đầu vào**: câu được trả lời. **Phân nhánh theo `matchPurpose`, KHÔNG phải theo loại câu:**

| `matchPurpose` | Cơ chế chấm |
|---|---|
| **`official`** (trận chính thức) | Hệ thống **CHỈ** (1) hiển thị đáp án thí sinh + đáp án đúng cho admin, (2) **gợi ý** kết quả so khớp. Điểm **chỉ chốt khi ADMIN bấm Đúng/Sai**. **KHÔNG tồn tại `autoJudge`** — không có toggle, không có đường nào bật được |
| **`practice`** (tự luyện tập) | `autoJudge` **tồn tại** — cần thiết vì practice solo chạy **không có admin** (`US` US-10.2: *"không cần admin điều khiển"*, orchestrator auto-advance) |

- **Kết quả khi admin chưa bấm (official)**: câu **chưa được chấm**; không sinh điểm; gợi ý so khớp không có hiệu lực trong trận.
- **Trạng thái game**: mọi round.
- **Actor**: `official` → **Admin là người quyết định duy nhất** (hotkey C/X), Server chỉ hiển thị + gợi ý. `practice` → Server chấm được.
- **Rule mâu thuẫn**: ❌ **`DEF` D4 BỊ GHI ĐÈ** — D4 mô tả `autoJudge` là **toggle per-contest bật được ở mọi trận**. Nay `autoJudge` **gắn cứng với `matchPurpose: practice`**. Xem 8A K-16.
- **Hệ quả phạm vi phát hành**: practice là **mốc v1.5 (Phase 11)** → **trong v1 `autoJudge` KHÔNG tồn tại dưới bất kỳ hình thức nào**. Engine v1 chỉ có đường admin-chấm. Đơn giản hoá Phase 6.
- **Chưa định nghĩa**: hệ thống phân biệt câu miệng vs câu gõ bằng trường nào — `SPEC` §9 field list không có. *(U-7)* **Lưu ý**: ở `official`, U-7 **giảm mức chặn** (cả hai loại đều admin chấm; trường này chỉ quyết định có hiện ô nhập cho thí sinh không). Ở `practice`, U-7 **vẫn chặn** — máy không chấm được câu miệng. *(U-38)*
- **Chưa định nghĩa**: `practice` gặp **câu miệng** (Khởi động, Về đích) mà không có admin — máy không chấm được. Practice solo có bị giới hạn chỉ dùng câu gõ không? Không nguồn nào nói. *(U-38)*

### Hệ quả bắt buộc khi viết rule tiếp theo

Mọi rule khi mô tả "Kết quả khi đúng / khi sai" phải hiểu là **kết quả SAU KHI admin phán quyết** (ở trận chính thức). Cụ thể:

- `R-VCNV-01` (hàng ngang), `R-TT-01` (ranked-speed), `R-TT-03` (last-wins): hệ thống ghi nhận submission + timestamp và **xếp hạng**, nhưng "đúng" do admin xác nhận → **thứ hạng điểm chỉ tính trên tập người được admin chấm ĐÚNG**.
- `R-GEN-04` (normalize): kết quả normalize là **đầu vào của gợi ý**, không phải phán quyết.
- `R-VD-04` (câu thực hành): vốn đã là đánh giá của người — nay nhất quán với mọi loại câu khác.

### Hệ quả bắt buộc khi viết rule tiếp theo

Mọi rule trong tài liệu này khi mô tả "Kết quả khi đúng / khi sai" phải hiểu là **kết quả SAU KHI admin đã phán quyết**, không phải kết quả hệ thống tự suy ra. Cụ thể:

- `R-VCNV-01` (hàng ngang), `R-TT-01` (ranked-speed), `R-TT-03` (last-wins): hệ thống ghi nhận submission + timestamp và **xếp hạng**, nhưng "đúng" là do admin xác nhận → **thứ hạng điểm chỉ tính trên tập người được admin chấm ĐÚNG**.
- `R-GEN-04` (normalize): kết quả normalize là **đầu vào của gợi ý**, không phải phán quyết.
- `R-VD-04` (câu thực hành): vốn đã là đánh giá của người — nay nhất quán với mọi loại câu khác.

## R-GEN-04. Normalize đáp án — TRIM bắt buộc

- **Nguồn**: `DEF` D4, D20.1 `[✅ user chốt]` · `PRD` FR-4.2 · `CLAUDE.md` · `P06` §Chấm
- **Điều kiện đầu vào**: so khớp submission với `acceptedAnswers`.
- **Kết quả (bắt buộc, không tắt được)**: **case-insensitive** + **trim đầu/cuối** + **collapse khoảng trắng thừa về 1 space**. Áp cả submission lẫn acceptedAnswers.
- **Tuỳ chọn**: **bỏ dấu tiếng Việt là CONFIG**.
- **Actor**: Server, Setter.
- **Rule mâu thuẫn**: **K-6 — mâu thuẫn trực tiếp với R-VCNV-02.** `F26` VCNV: *"bất kỳ sai sót về kí tự, **dấu câu**, ngữ pháp → không được công nhận"*. Bật "bỏ dấu tiếng Việt" là vi phạm luật gốc. `F26` Tăng tốc cũng yêu cầu đúng chính tả, chỉ nới ở *"ý nghĩa tương đồng"*.
- **Chưa định nghĩa**: ngoại lệ *"ý nghĩa tương đồng"* (Tăng tốc) và *"ý nghĩa tương đồng + cùng tổng số chữ cái"* (VCNV) — **không cơ chế nào trong repo hiện thực được**; chỉ có `acceptedAnswers` liệt kê tay. *(U-35)*

## R-GEN-05. Server time là source of truth duy nhất

- **Nguồn**: `CLAUDE.md` §UX + §Quy ước · `PRD` FR-5.1, NFR-1 · `P06` §Timer service · `plan.md` §Nguyên tắc #1
- **Điều kiện đầu vào**: mọi timeout, thứ tự chuông, thứ hạng tốc độ.
- **Kết quả**: tính theo đồng hồ **server**; client chỉ hiển thị; buzzer timestamp gắn tại **instance OWNER** (`product-gaps.md` §5.1).
- **Actor**: Server, mọi client.
- **Rule mâu thuẫn**: `W26` định nghĩa độ phân giải ghi nhận là **2 chữ số thập phân** — repo dùng ms (xem K-8).
- **Chưa định nghĩa**: NTP/chrony yêu cầu ở compose; **profile portable Windows LAN** chạy 1 máy — không nguồn nào nói xử lý lệch clock ở đó. *(U-14)*

## R-GEN-06. No-repeat toàn CONTEST (chỉ có trong repo)

- **Nguồn**: `DEF` D22.3 `[✅ user chốt]` · `PRD` FR-4.1 · `US` US-3.10 · `P06`
- **Điều kiện đầu vào**: câu đã được **hỏi** trong bất kỳ match/vòng nào của contest → `usedInContest` (event `QUESTION_USED`).
- **Kết quả**: câu **không xuất hiện lại** trong contest; draw loại khỏi pool.
- **Kết quả khi pool còn lại thiếu**: pre-flight chặn (chỉ yêu cầu pool CÒN LẠI ≥ worst-case).
- **Actor**: Server, Admin.
- **Rule mâu thuẫn**: R-KD-07 (skip cũng tiêu pool).
- **Chưa định nghĩa**: "đã được hỏi" = đã hiện màn hình hay đã chấm; câu skip vì media hỏng có set cờ không. *(U-30)*

## R-GEN-07. Điểm = reduce(events), undo có giới hạn

- **Nguồn**: `P06` §Event sourcing · `PRD` FR-4.3, FR-4.4 · `US` US-4.3 · `red-team.md` H5
- **Điều kiện đầu vào**: admin muốn sửa/undo.
- **Kết quả khi hợp lệ**: undo **chỉ event chấm điểm GẦN NHẤT chưa có event khác build-upon**.
- **Kết quả khi đã build-upon**: dùng `SCORE_ADJUST {delta, reason}` — **bắt buộc nhập lý do**, vào audit log.
- **Actor**: Admin.
- **Rule mâu thuẫn**: R-VD-05 — undo một chấm điểm steal transfer phải hoàn nguyên **2 seat**; nguồn không nói reducer xử lý thế nào. *(U-15)*
- **Chưa định nghĩa**: undo có giới hạn số lần không.

## R-GEN-09. Reconnect grace

- **Nguồn**: `PRD` FR-3.5 · `US` US-5.4 · `research/ux-gaps.md`
- **Điều kiện đầu vào**: thí sinh mất kết nối.
- **Kết quả khi < 120s**: **giữ ghế** + state-sync; banner "đang kết nối lại".
- **Kết quả khi quá grace**: hệ thống **CHỈ TÔ NỔI BẬT** ghế trên màn admin kèm thời lượng mất kết nối; **admin quyết** giữ / gia hạn. Không có hệ quả tự động nào.
- **Actor**: Thí sinh, Server, Admin.
- **Rule mâu thuẫn**: `DEF` D13.4 — rớt đúng lượt riêng thì engine dừng lại chờ admin quyết, ghi đè grace.
- **`U-13` ĐÃ ĐÓNG 2026-07-26** — ✅ chủ dự án chốt: *"chỉ highlight, admin là người quyết"*. ⇒ **KHÔNG cần kê danh sách giá trị `dropoutPolicy`**, vì **không có chính sách tự động nào** để kê. Cấu hình duy nhất còn lại là **ngưỡng grace** (mặc định **120 giây**) — mốc để bắt đầu tô nổi bật.
  - Cùng **một mẫu** với `Đ-1` (tô khác biệt ký tự, admin chấm) và `Đ-28` (tô đỏ bản quá hạn, admin phán quyết). Nguyên tắc nền điểm 1 áp nguyên: máy đo và hiển thị **sự kiện**, người giữ **phán quyết**.
  - **`Đ-52` (27/07) — v1 KHÔNG ship kick.** Thay bằng **vô hiệu hoá / kích hoạt lại ghế** (`EVENT-048`): **đảo ngược được**, dùng **mọi lúc** trong trận chưa đóng sổ, ghế giữ nguyên điểm và vị trí. Vẫn qua dialog Yes/No + AuditLog kèm lý do (`CLAUDE.md` §UX, `S-2`). **Không** phải hệ quả của quá grace — hai chuyện độc lập. Chi tiết: `game-state-machine.md` `STATE-026`, `EVENT-048`.

## R-GEN-10. Phạm vi đáp án (bảo mật — quy tắc sản phẩm)

- **Nguồn**: `SPEC` §11 · `PRD` NFR-4, FR-2.4, FR-5.5 · `CLAUDE.md` · `DEF` D15.2
- **Kết quả mặc định**: đáp án rời server tới đúng **admin + MC** (authenticated + audit). Viewer/thí sinh/overlay **KHÔNG** nhận.
- **Ngoại lệ**: `revealAnswerAfterJudge` bật → đẩy xuống 3 kênh kia **SAU khi chấm**. Official default TẮT; practice default BẬT.
- **Actor**: Admin, MC, Reviewer, Server.
- **Rule mâu thuẫn**: **`PRD` NFR-4 nói "contest bật"; `SPEC` §11/§13 + `CLAUDE.md` nói "per-MATCH, không phải per-contest"** — chưa phân xử (xem `docs/product-discovery.md` C-1).
- **Chưa định nghĩa**: —

## R-GEN-11. Preload media mã hoá (quy tắc sản phẩm)

- **Nguồn**: `DEF` D12b `[✅ user chốt]` · `PRD` FR-4.6 · `P06` · `US` US-5.6 · `red-team.md` C2
- **Kết quả**: thí sinh preload **blob mã hoá** qua service worker; server phát `MEDIA_KEY` **đúng lúc reveal**; viewer/overlay preload URL thường.
- **Kết quả khi SW lỗi**: tự hạ cấp `reveal-only` + log; trận không đứng.
- **Actor**: Thí sinh, Server, BTC.
- **Chưa định nghĩa**: câu thay thế cũng hỏng media. *(U-11)*

## R-GEN-12. Chống rò đề từ bộ đề public

- **Nguồn**: `SPEC` §9 (vá C-v2-1) · `red-team.md` VÒNG 2
- **Kết quả khi có câu `everPublic=true`**: pre-flight **hard-block** mọi match; force cần confirm 2 bước + audit. Kiểm ở đơn vị **CÂU**, mỗi **match**.
- **Ngoại lệ**: match `practice` → cho phép, badge "đề public".
- **Actor**: Admin, Server.
- **Rule mâu thuẫn**: `DEF` D16 định nghĩa `Question.visibility` là **cột set tay**; `SPEC` §9 định nghĩa lại là **derived, read-only**.
- **Chưa định nghĩa**: `everPublic` một chiều vĩnh viễn — không có cách gỡ. *(U-29)*

---

# PHẦN 7 — LUẬT THI ĐỘI (v2, chưa hiện thực)

> Nguồn: `SPEC` §2b · `DEF` D17 `[✅ user chốt toàn bộ]` · `US` US-10.5. **Không có trong `F26`/`W26`** — đây là mở rộng của sản phẩm, không phải luật O26.

| ID | Rule | Điều kiện | Đúng → | Sai → | Actor | Mâu thuẫn / chưa định nghĩa |
|---|---|---|---|---|---|---|
| **R-TEAM-01** | Khoá cá nhân khi bấm sai | Khởi động chung / clue-buzz / cướp về đích | Khoá **CÁ NHÂN**; thành viên khác vẫn bấm được (`teamLockout: false`) | option `true` → khoá cả đội | Thí sinh, Server | Mâu thuẫn R-KD-03 (câu không mở lại chuông). Đội đông quân số có nhiều lượt bấm → chỉ **pre-flight cảnh báo** |
| **R-TEAM-02** | Tăng tốc last-wins theo đội | `scoringUnit: team` | Đáp án đội = **bản CUỐI của bất kỳ thành viên**; ranking theo ts bản cuối | — | Thí sinh, Server | Chưa định nghĩa: bản sau sai đè bản trước đúng |
| **R-TEAM-03** | VCNV sai CNV → loại CẢ ĐỘI | Thành viên trả lời sai CNV | Loại cả đội | — | Thí sinh | Lý do D17.3: tránh đội 4 người có 4 lần đoán |
| **R-TEAM-04** | NSHV 1 lần/đội | Vòng Về đích | 1 lần/đơn vị điểm/trận | — | Thí sinh | — |
| **R-TEAM-05** | Cấm cùng đội cướp | Cửa sổ steal | Chỉ đơn vị điểm khác được bấm | Server **reject** (hard-code) | Thí sinh, Server | Exploit: same-team steal = trả lời lại miễn phí |
| **R-TEAM-06** | Tie-break đội | `TIE_BREAK` với đội | Mỗi đội cử **1 người** bấm chuông | — | Thí sinh, Đội trưởng | Chưa định nghĩa: đội trưởng không chỉ định kịp |
| **R-TEAM-07** | Chế độ lượt cá nhân | `individualTurnMode` | `all-members`: mỗi TV 1 lượt · `representative`: 1 lượt/đội | — | Đội trưởng, Admin | Đổi đại diện chỉ tại **cửa vào vòng** (`LOBBY`) |

---

# PHẦN 8 — TRẠNG THÁI QUYẾT ĐỊNH

## 8A. ĐÃ CÓ QUYẾT ĐỊNH trong `plans/` → FOLLOW, biến thể còn lại ❌ REJECTED

> Không cần hỏi lại. Mỗi dòng ghi rõ quyết định nào chốt, theo cái gì, loại cái gì.

| # | Vấn đề | ✅ THEO | Căn cứ quyết định | ❌ REJECTED |
|---|---|---|---|---|
| **K-1** | Cấu trúc Khởi động | **2 lượt**: riêng 6 câu × 3s (sai 0) + chung 12 câu bấm chuông × 3s (sai −5) | `DEF` **D8** ✅ 12/07 — bảng giá trị liệt kê đúng 2 lượt; `R26` §1 | `W26`: **1 lượt duy nhất 12 câu / 60 giây, sai không trừ, tối đa 180đ**. Loại vì D8 chọn Fandom + sai số học nội tại |
| **K-2** | Về đích — mức điểm | **20 và 30**; 20đ→15s, 30đ→20s | `DEF` **D8** ✅ 12/07 — *"Gói 3 câu chọn từ 2 mức {20, 30} (không còn mức 10)"* | `W26`: **20 và 40**, 40đ→30s. Loại vì D8 + `W26` tự mâu thuẫn |
| **K-5** | VCNV — điều kiện mở góc ảnh | *"trả lời đúng từ hàng ngang → miếng ghép mở; không trả lời được → không mở"* | `DEF` **D8** (Fandom là nguồn duy nhất) | `W26`: *"**ít nhất 1 thí sinh** trả lời đúng → góc mở"*. Loại theo D8 → **ngưỡng số người trở thành U-37 chưa định nghĩa** |
| **K-7** | Điều kiện mở ô trung tâm | *"sau 4 hàng ngang, chưa ai giải CNV"* | `DEF` **D8** bảng VCNV — *"Ô trung tâm (sau 4 hàng, chưa ai giải CNV)"*; `R26` §2 | `W26`: *"hết 4 từ hàng ngang mà chưa tìm được CNV"* (diễn đạt khác). Loại theo D8 |
| **K-8** ✅ | Độ phân giải "đồng thời gian" Tăng tốc | **`ms`** (server-received). ✅ chủ dự án chốt 26/07 — lý do **máy tính dễ tính toán** | **Quyết định kỹ thuật**, KHÔNG phải nguồn luật (đừng trích `D8`): `F26` im lặng về độ phân giải; `ms` chỉ có ở `R26` §7 = nháp trong repo | `W26` + `W` bài tổng (hai trang, **2 chữ số thập phân**) — tiền lệ thật đi ngược lựa chọn này; cái giá là cửa sổ hoà hẹp hơn 10 lần, **đã chấp nhận**. `SPEC` §6 `'microsecond'`: giữ làm config, **KHÔNG** cho `O26_DEFAULT@1` |
| **K-9** | Tie-break — bấm sai | **Cả nhóm sang câu TIẾP THEO**, không mở lại cùng câu | `DEF` **D8** (Fandom) + `R26` §5 ✅; nguyên văn `F26` xác nhận | `R26` **§7** dòng *"người bấm sai bị loại khỏi CÂU đó, những người còn lại thi tiếp cùng câu"* — nhãn "đề xuất của Claude". **PHẢI XOÁ khỏi `R26` §7** |
| **K-12** | Steal mode | **`transfer`** (cướp đúng = LẤY điểm từ người sai) | `DEF` **D8** ✅ — *"cướp thành công = LẤY điểm từ người trả lời sai (`stealMode: 'transfer'`)"*; *"default cũ 'không steal' SAI"* | `SPEC` §7 option **`'add'`** — không phải luật O26. Giữ như config hợp lệ nhưng **KHÔNG được dùng cho preset `O26_DEFAULT@1`** |
| **K-13** | Tie-break hết câu | **Bốc thăm** (hệ thống random, admin xác nhận) | `DEF` **D13.6** ✅ 12/07 | `SPEC` §8 option **`'admin-decides'`** — không phải luật O26. Cùng xử lý như K-12 |
| **K-14** | Tie-break áp cho vị trí nào | **Chỉ vị trí NHẤT** (`tieBreakPositions` default `[1]`, config đổi được) | `DEF` **D13.5** ✅ 12/07 | Diện rộng hơn của `F26` (*"các thí sinh có cùng số điểm"*) — **thu hẹp có chủ đích**, không phải sai sót |
| **K-4** | `revealAnswerAfterJudge` phạm vi | **per-MATCH** | `SPEC` §11 + §13 (*"per-MATCH, **không phải per-contest**"*) · `plan.md` §Nguyên tắc #2 (*"per-match"*) · `CLAUDE.md` · `US` US-8.12 · phase-07 step 8 · phase-09 — **5 nguồn** | `PRD` **NFR-4** dòng *"**contest** bật revealAnswerAfterJudge"* — **wording lạc hậu duy nhất, PHẢI SỬA thành match** |
| **K-15** | Giá trị Athena cũ | Giá trị O26 theo `F26` | `DEF` **D8** | `ATH`: VCNV 80/60/40/20 · Về đích 10/20/30 · Tăng tốc 30s/câu · Khởi động 60s/TS. **Đã bãi bỏ hoàn toàn** — chỉ giữ để tránh nhầm khi đọc code Athena cũ |
| **K-3** | Bấm chuông trước hiệu lệnh | Khởi động: **được** bấm khi MC đang đọc · Tie-break: **mất quyền** | `DEF` **D8** — cả hai đều từ `F26`, là **chủ đích của chương trình** | Không có biến thể bị loại. Nhưng repo **chưa mô hình hoá** sự khác biệt → xem U-1 |
| **K-16** | **Ai quyết định Đúng/Sai** | **Trận chính thức (`official`): ADMIN quyết định, mọi câu, mọi vòng** — hệ thống chỉ hiển thị + gợi ý so khớp. **`autoJudge` CHỈ tồn tại ở bản tự luyện tập (`practice`)** | **Quyết định chủ dự án 2026-07-23** | ❌ `DEF` **D4** — mô tả `autoJudge` là **toggle per-contest bật được ở MỌI trận**, kể cả official. Nay `autoJudge` **gắn cứng với `matchPurpose: practice`**, không phải toggle tự do. Hệ quả: **v1 (chưa có practice) KHÔNG có `autoJudge`** |

### Hành động sửa tài liệu phát sinh từ 8A

| # | File cần sửa | Sửa gì |
|---|---|---|
| A-1 | `plans/…/research/rules-2026.md` §7 | **XOÁ** dòng tie-break *"những người còn lại thi tiếp cùng câu"* — mâu thuẫn §5 và trái `F26` (K-9) |
| A-2 | `plans/…/PRD.md` NFR-4 | Đổi *"contest bật `revealAnswerAfterJudge`"* → **"match bật"** (K-4) |
| A-3 | `plans/…/research/ruleconfig-v2-spec.md` §7, §8 | Ghi rõ `stealMode: 'add'` và `exhaustedFallback: 'admin-decides'` **không hợp lệ trong preset O26** (K-12, K-13) |
| A-4 | `plans/…/DEFERED.md` D4 · `PRD.md` FR-4.2 · `phase-06-game-engine.md` §Chấm · `user-stories.md` US-4.2 | **Bỏ `autoJudge` khỏi trận official** (K-16). Viết lại: official → hệ thống hiển thị + gợi ý so khớp, điểm chỉ chốt khi admin bấm Đúng/Sai. `autoJudge` **chỉ tồn tại ở `matchPurpose: practice`**, không phải toggle per-contest |
| A-6 | `plans/…/research/ruleconfig-v2-spec.md` §13 (bảng official vs practice) | **Thêm dòng `autoJudge`**: official = **không tồn tại** · practice = **có**. Đây là khác biệt policy giữa 2 purpose, bảng §13 hiện chưa có (K-16) |
| A-7 | `plans/…/phase-06-game-engine.md` · `phase-11-practice-mode.md` | Chuyển hạng mục `autoJudge` từ Phase 6 (v1) sang **Phase 11 (v1.5)** — v1 không có practice nên không có `autoJudge` (K-16) |
| A-5 | `public/assets/engine.js` | ✅ **ĐÃ KIỂM TRA 2026-07-23 — demo ĐÃ ĐÚNG, không cần sửa.** Mọi hàm tính điểm nhận `correct` như **tham số đầu vào**, không tự suy: `scoreKhoiDongRieng(cid, correct)` L187 · `scoreKhoiDongChung(cid, correct)` L195 · `scoreVcnvRow(results)` L204 · `scoreVcnvObstacle(cid, correct)` L218 · `scoreTangToc(answers)` L233 (xếp hạng **chỉ trên `a.correct`** — khớp đúng hệ quả K-16). Không có đường nào tự chấm |

## 8B. CHƯA CÓ QUYẾT ĐỊNH → còn mở

| # | Mâu thuẫn | Bên A | Bên B | Vì sao chưa quyết được |
|---|---|---|---|---|
| **K-6** | **Chính tả**: *"sai kí tự/**dấu câu**/ngữ pháp → không công nhận"* vs normalize *"bỏ dấu tiếng Việt = config"* | `F26` §VCNV + §Tăng tốc (qua D8) | `DEF` **D4** + `PRD` FR-4.2 | **Hai quyết định user chốt cùng ngày 12/07 mâu thuẫn nhau**: D8 nhận Fandom làm luật, D4 chốt cơ chế normalize. D4 không nói mình đang ghi đè luật gốc. **Nặng nhất trong danh sách** |
| **K-10** | `Question.visibility`: **cột set tay** vs **derived read-only** | `DEF` **D16** ✅ user chốt (*"thêm sẵn cột visibility"*) | `SPEC` §9 (vá H-v2-10 — *"derived, read-only"*) | D16 là quyết định user; `SPEC` §9 là bản vá red-team của Claude, **user chưa xác nhận việc sửa D16** |
| **K-11** | Luật cho **≠4 thí sinh**: thang điểm Tăng tốc, lượt chọn VCNV, số người cướp | `DEF` **D1** ✅ chốt 1-12 ghế | `F26` + `R26` viết cho **đúng 4** | D1 chốt số ghế nhưng **không chốt giá trị luật dẫn xuất**. `SPEC` §1.4 chỉ cho *cơ chế* (mảng theo số đơn vị điểm, runtime prefix), **không cho giá trị**. D1 "bối cảnh cũ" đã cảnh báo đúng vấn đề này và cảnh báo chưa được xử lý |

---

# PHẦN 9 — TỔNG HỢP TRƯỜNG HỢP CHƯA ĐỊNH NGHĨA

## Chặn viết acceptance criteria

| # | Chưa định nghĩa | Rule |
|---|---|---|
| U-1 | **Trigger cho mốc "MC đọc xong" / "hiệu lệnh MC" / "MC công bố đáp án"** — 3 mốc luật khác nhau, hệ thống không biết cái nào | R-KD-01, R-KD-04, R-KD-06, R-TB-03 |
| U-2 | Thang điểm Tăng tốc cho >4 thí sinh (12 giá trị là bao nhiêu) | R-TT-01 |
| U-3 | `cnvPointsByRowsOpened` cho `rowCount` 5-8 | R-VCNV-04 |
| U-4 | Lượt chọn hàng ngang khi số thí sinh > số hàng | R-VCNV-03 |
| U-5 | "3 TS còn lại" cướp quyền khi ghế ≠ 4 | R-VD-05 |
| ~~U-6~~ ✅ | **ĐÃ ĐÓNG 26/07** — bổ sung 5 trường: `isPractical` · `practiceSeconds` · `stealPracticeSeconds` · `equipmentNote` · `acceptanceCriteria`. Xem R-VD-04 và GR-019 | R-VD-04 |
| U-7 | **Trường phân biệt câu miệng vs câu gõ** không tồn tại | R-GEN-03 |
| U-8 | NSHV × steal transfer: số học khi cả hai cùng áp | R-VD-06, R-VD-05 |
| U-34 | **Câu trắc nghiệm Khởi động** ("đúng sai / chọn đáp án cho sẵn") không có mô hình dữ liệu | R-KD-05 |
| U-35 | **Ngoại lệ "ý nghĩa tương đồng" / "cùng tổng số chữ cái"** không hiện thực được bằng `acceptedAnswers` | R-VCNV-02, R-GEN-04 |
| U-36 | **Câu "sắp xếp" / "chọn ảnh A-F"** cần mô hình dữ liệu khác text | R-TT-04 |
| U-38 | **Practice gặp câu MIỆNG mà không có admin** — `autoJudge` chỉ so khớp text được; Khởi động và Về đích là trả lời miệng. Practice solo có bị giới hạn chỉ dùng câu gõ không? Không nguồn nào nói | R-GEN-03 |
| U-37 | **Ngưỡng số thí sinh đúng để mở miếng ghép VCNV** — `F26` (nguồn được chọn) chỉ nói "trả lời đúng → mở", không nêu ngưỡng. Biến thể "ít nhất 1 TS" của `W26` **đã bị loại** theo D8 (K-5) → câu hỏi trở thành chưa định nghĩa. Quyết định với 1-12 ghế | R-VCNV-01 |

## Chặn xử lý sự cố

| # | Chưa định nghĩa | Rule |
|---|---|---|
| U-9 | Pool cạn **giữa trận** do skip nhiều | R-KD-07, R-GEN-06 |
| U-10 | Pool câu phụ cạn **giữa** tie-break | R-TB-04 |
| U-11 | Câu thay thế cũng hỏng media | R-GEN-11 |
| ~~U-13~~ ✅ | **ĐÃ ĐÓNG 26/07** — quá grace thì **chỉ tô nổi bật, admin quyết**; không có `dropoutPolicy` tự động nên không có gì để kê. Chỉ còn cấu hình **ngưỡng grace** (120s) | R-GEN-09 |
| U-14 | Lệch clock ở profile portable (không NTP) | R-GEN-05 |
| U-15 | Undo một chấm điểm **steal transfer** (2 seat) | R-GEN-07, R-VD-05 |

## Chưa được chủ dự án chốt (còn nhãn "đề xuất")

| # | Mục | Nguồn |
|---|---|---|
| U-16 | Bấm chuông CNV giữa timer hàng ngang → có dừng đồng hồ không | `R26` §7 (R-VCNV-06) |
| U-17 | Tie-break nhiều nhóm hoà | `R26` §7 (R-TB-05) |
| U-18 | NSHV áp sang câu thay thế khi skip | `R26` §7 (R-VD-06) |
| U-19 | Câu skip khi đang mở cướp → huỷ cửa sổ | `R26` §7 (R-VD-05) |
| U-20 | Làm tròn điểm lẻ khi −½ giá trị lẻ | `R26` §7 (R-VD-05) |
| U-21 | Câu phụ rút từ pool `KV` | `DEF` D24 ("đề xuất của Claude, đổi được") |

## Chi tiết nhỏ, không chặn

| # | Chưa định nghĩa | Rule |
|---|---|---|
| U-22 | Trả lời đúng sớm có sang câu ngay không | R-KD-01 |
| U-24 | Người bị loại VCNV có bị trừ điểm hàng ngang đã kiếm không | R-VCNV-04 |
| U-25 | Ai được trả lời câu ô trung tâm | R-VCNV-05 |
| U-26 | Đổi gói về đích sau khi chọn | R-VD-01 |
| U-27 | "Vị trí đứng" = ghế vật lý hay thứ hạng | R-VD-02 |
| U-28 | Người bị cướp có xuống điểm âm không | R-VD-05 |
| U-29 | `everPublic` một chiều — có cách gỡ không | R-GEN-12 |
| U-30 | "Đã được hỏi" = đã hiện màn hình hay đã chấm | R-GEN-06 |
| U-31 | Admin có thấy lịch sử submission trước không | R-TT-03 |
| U-32 | Gợi ý ký tự hiện lúc nào | R-VCNV-08 |
| U-33 | Thí sinh không dùng được chuột | R-GEN-01 |

---

# Việc cần làm tiếp

**Không cần hỏi lại — chỉ cần thi hành:**

1. **Sửa 3 chỗ tài liệu theo 8A**: A-1 (`rules-2026.md` §7 xoá dòng tie-break sai), A-2 (`PRD.md` NFR-4 "contest"→"match"), A-3 (`spec` §7/§8 đánh dấu option ngoài luật O26).
2. **Dùng snapshot thay URL sống**: preset `O26_DEFAULT@1` trích từ `docs/source/fandom-olympia-26-luat-choi.md`, không từ URL — wiki sửa được bất kỳ lúc nào.

**Cần chủ dự án quyết:**

3. **Phân xử K-6, K-10, K-11** — 3 mâu thuẫn còn mở. **K-6** (chính tả gốc vs normalize D4) là nặng nhất: hai quyết định user chốt cùng ngày mâu thuẫn nhau. **K-11** (luật cho ≠4 thí sinh) chặn viết engine.
4. **Chốt U-16 → U-21** — 6 mục còn nhãn "đề xuất của Claude" trong `R26` §7 và `DEF` D24.
5. **Trả lời U-1** — một trigger duy nhất cho 3 mốc "MC đọc xong / hiệu lệnh MC / MC công bố đáp án"; ảnh hưởng Khởi động, tie-break, ghi nhận đáp án.
6. **Bổ sung mô hình dữ liệu cho U-6, U-7, U-34, U-35, U-36, U-37** — 6 loại câu hỏi/quy tắc chấm có trong luật gốc nhưng **không khai được** bằng field hiện có.
