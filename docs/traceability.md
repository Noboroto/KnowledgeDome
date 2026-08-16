# Truy nguyên

> **Mục đích**: trả lời **một** câu hỏi — *"điều này từ đâu ra?"* Mỗi rule trong `game-rules.md` phải chỉ được về **luật gốc O26**, về một **quyết định của chủ dự án**, hoặc về một **suy luận** ghi rõ.
>
> **Tài liệu này KHÔNG đặt ra luật.** Nó chỉ nối các mã lại với nhau. Có mâu thuẫn giữa nó và `game-rules.md` thì `game-rules.md` đúng.

## Nguồn

| Ký hiệu | Là gì | Vị thế |
|---|---|---|
| **Luật gốc** | `source/fandom-olympia-26-luat-choi.md` — bản lưu nguyên văn [Luật chơi/Olympia 26](https://duong-len-dinh-olympia.fandom.com/vi/wiki/Lu%E1%BA%ADt_ch%C6%A1i/Olympia_26), lấy 2026-07-23 | **Source of truth duy nhất về LUẬT** |
| **Âm thanh gốc** | `source/fandom-olympia-26-am-thanh.md` — bản lưu **trích lược cột `Tên`** của [Âm thanh](https://duong-len-dinh-olympia.fandom.com/vi/wiki/%C3%82m_thanh), lấy 2026-08-04 | Nguồn **danh sách khe âm thanh** cho `QĐ-122`. **Không** phải nguồn luật |
| **Quyết định** | `decisions.md` — `QĐ-001` → `QĐ-110` | Source of truth về **lựa chọn sản phẩm** |
| **Rule** | `game-rules.md` — `GR-001` → `GR-037` | Đặc tả nghiệp vụ **chuẩn tắc** |
| **Máy trạng thái** | `game-state-machine.md` — `STATE-*` · `EVENT-*` · `T-*` · `INV-*` | Đặc tả **chuẩn tắc** phần vận hành |
| **Thuật ngữ** | `glossary.md` — `TERM-001` → `TERM-058` | Tên gọi chuẩn |
| `plans/**` | Kế hoạch kỹ thuật do ClaudeKit tạo | **KHÔNG phải nguồn.** Tầng dưới của spec; không trích như requirement |
| `reviews/` | Kho lưu thảo luận, đề xuất `GRR-*` | **KHÔNG phải nguồn** |
| Athena | `D:\Github\Athena-Intelligent-Olympia` | **Tiền lệ**, không phải thẩm quyền. Dùng để phát hiện nhánh thiếu và xếp thứ tự việc kiểm thử |

**Wikipedia bị loại làm căn cứ luật** — bản Wikipedia mô tả Khởi động, mức điểm Về đích và điều kiện mở góc ảnh **khác** bản Fandom, và tự mâu thuẫn ở vài chỗ. Chủ dự án chọn Fandom làm nguồn duy nhất. Mọi giá trị của bản Wikipedia đã bị loại **có chủ đích**, không phải bỏ sót — xem §Biến thể bị loại.

## Ba tầng nguồn

Mỗi phát biểu trong `game-rules.md` và `decisions.md` mang **đúng một** nhãn:

| Nhãn | Nghĩa | Ai đổi được |
|---|---|---|
| `[LUẬT GỐC]` | Nguồn nói thẳng | **Không ai** — đổi là không còn chơi luật O26 |
| `[CHỦ DỰ ÁN]` | Nguồn im lặng, chủ dự án chọn | Chủ dự án |
| `[SUY RA]` | Hệ quả bắt buộc của một quyết định khác | Đổi được, nhưng phải đổi cả tiền đề |

---

# Ma trận rule ↔ luật gốc

## Khởi động

| Rule | Luật gốc | Quyết định liên quan |
|---|---|---|
| `GR-001` chấm câu lượt riêng | *"mỗi thí sinh trả lời 6 câu hỏi. Thời gian suy nghĩ cho mỗi câu hỏi là 3 giây tính từ lúc người dẫn chương trình đọc xong câu hỏi"* · đúng **+10**, sai **0** | `QĐ-010` `QĐ-027` `QĐ-041` |
| `GR-002` hết thời gian suy nghĩ | Cùng đoạn — 3 giây | `QĐ-029` `QĐ-030` |
| `GR-003` giành quyền bằng chuông | *"Thí sinh có thể bấm chuông trong khi người dẫn chương trình đang đọc câu hỏi"* · *"3 giây tính từ lúc thí sinh giành được quyền trả lời"* | `QĐ-023` `QĐ-028` |
| `GR-004` chấm câu và hình phạt | *"Trả lời sai hoặc bấm chuông mà không có câu trả lời sau 3 giây bị trừ 5 điểm"* | `QĐ-061` |
| `GR-005` cửa sổ chuông rỗng | *"Sau 3 giây… nếu không có thí sinh nào giành quyền trả lời, câu hỏi đó sẽ bị bỏ qua"* | `QĐ-041` `QĐ-044` |
| `GR-006` ghi nhận đáp án | Nguồn im lặng về cơ chế ghi nhận — toàn bộ là quyết định sản phẩm | `QĐ-016` `QĐ-029` `QĐ-056` |

## Vượt chướng ngại vật

| Rule | Luật gốc | Quyết định liên quan |
|---|---|---|
| `GR-031` §Ngoại lệ — bộ VCNV | *"Có 4 từ hàng ngang, **cũng chính là 4 gợi ý liên quan đến Chướng ngại vật**"* · *"4 miếng ghép **tương ứng với** 4 từ hàng ngang ở 4 góc và **được đánh số cố định**"* | `QĐ-082` |
| `GR-007` lượt chọn hàng ngang | §VCNV — lượt chọn theo thứ tự | `QĐ-019` `QĐ-021` |
| `GR-008` trả lời hàng ngang và mở miếng ghép | *"trả lời đúng từ hàng ngang → miếng ghép mở"* · *"bất kỳ sai sót về kí tự, dấu câu, ngữ pháp → không được công nhận"* | `QĐ-010` `QĐ-018` `QĐ-052` `QĐ-057` |
| `GR-009` bấm chuông giải Chướng ngại vật | §VCNV — băng điểm theo số hàng đã mở. **Lựa chọn phán quyết thứ ba (*Huỷ kết quả*) KHÔNG có trong luật gốc** — xem §Bổ sung ngoài O26 | `QĐ-021` `QĐ-057` `QĐ-104` |
| `GR-010` trả lời sai Chướng ngại vật | §VCNV — sai thì bị loại khỏi vòng | `QĐ-057` |
| `GR-011` ô trung tâm và gợi ý cuối | *"sau 4 hàng ngang, chưa ai giải được Chướng ngại vật"* | `QĐ-048` `QĐ-057` |
| `GR-012` toàn bộ thí sinh bị loại | Nguồn im lặng — suy ra từ luật loại người | `QĐ-044` `QĐ-057` |

## Tăng tốc

| Rule | Luật gốc | Quyết định liên quan |
|---|---|---|
| `GR-013` xếp hạng tốc độ | §Tăng tốc — thang **40/30/20/10** theo thứ hạng tốc độ | `QĐ-013` `QĐ-059` |
| `GR-014` đồng thời gian | Nguồn im lặng về **độ phân giải** | `QĐ-025` `QĐ-059` |
| `GR-015` ghi nhận bản cuối | Nguồn im lặng — quyết định sản phẩm | `QĐ-018` `QĐ-029` |

## Về đích

| Rule | Luật gốc | Quyết định liên quan |
|---|---|---|
| `GR-016` thứ tự lượt thi | §Về đích — lần lượt từng thí sinh | `QĐ-002` `QĐ-058` |
| `GR-017` chọn gói câu | §Về đích — gói 3 câu từ hai mức **20** và **30** | `QĐ-058` |
| `GR-018` trả lời câu của mình | 20đ → **15 giây**, 30đ → **20 giây** | `QĐ-029` `QĐ-058` |
| `GR-019` câu hỏi thực hành | *"Trong câu hỏi thực hành… chương trình sẽ giới thiệu các dụng cụ liên quan"* · 20đ → **30 giây**, 30đ → **60 giây**; người cướp **20/40 giây** | `QĐ-051` `QĐ-058` `QĐ-066` |
| `GR-020` cướp quyền | §Về đích — cửa sổ **5 giây**; cướp đúng **lấy** điểm của người sai; cướp sai **−½** giá trị câu | `QĐ-012` `QĐ-021` `QĐ-058` `QĐ-061` |
| `GR-021` Ngôi sao hy vọng | *"Trả lời sai sẽ bị trừ đi số điểm của câu hỏi, kể cả các thí sinh còn lại có giành quyền trả lời hay không"* | `QĐ-019` `QĐ-026` `QĐ-058` |

## Câu hỏi phụ

| Rule | Luật gốc | Quyết định liên quan |
|---|---|---|
| `GR-022` điều kiện kích hoạt | *"Sau phần thi Về đích, các thí sinh có cùng số điểm sẽ bước vào phần thi Câu hỏi phụ"* | `QĐ-036` `QĐ-037` `QĐ-038` |
| `GR-023` thể thức ba câu | *"Các thí sinh trả lời 3 câu hỏi. Thời gian suy nghĩ cho mỗi câu hỏi là 15 giây… Nếu trả lời sai, các thí sinh sẽ bước sang câu hỏi tiếp theo"* | `QĐ-031` `QĐ-055` |
| `GR-024` chuông không sống trước hiệu lệnh | *"nếu có thí sinh bấm chuông trả lời trước khi có hiệu lệnh… thí sinh đó sẽ bị mất quyền trả lời câu hỏi"* | `QĐ-054` |
| `GR-025` hết câu chưa phân định | *"Sau 3 câu hỏi, nếu không tìm được thí sinh thắng cuộc, các thí sinh sẽ phải bốc thăm"* | `QĐ-011` `QĐ-055` |

## Xuyên vòng — không có trong luật gốc

Bảy rule dưới đây **không** đến từ luật gốc. Chúng tồn tại vì đây là một **hệ thống phần mềm** chứ không phải một buổi ghi hình: luật gốc không cần nói ai bấm nút, điểm lưu thế nào, hay ai được thấy đáp án.

| Rule | Vì sao tồn tại | Quyết định |
|---|---|---|
| `GR-026` phán quyết của admin | Luật gốc giả định có người chấm; phần mềm phải nói **rõ** rằng máy không chấm | `QĐ-001` `QĐ-010` `QĐ-014` `QĐ-053` |
| `GR-027` chuẩn hoá và tô nổi bật | Công cụ giúp admin nhìn ra chỗ khác; **không** phải cơ chế chấm | `QĐ-010` |
| `GR-028` điểm là hàm của event log | Mô hình dữ liệu, để sửa sai mà không mất lịch sử | `QĐ-011` `QĐ-012` `QĐ-013` |
| `GR-029` điều chỉnh điểm thủ công | Van thoát cho mọi sai sót; thay cho việc chấm lại | `QĐ-014` `QĐ-039` |
| `GR-030` bỏ vòng, chạy lại, kết thúc sớm | Xử lý sự cố buổi thi | `QĐ-034` `QĐ-035` |
| `GR-031` rút đề và không lặp câu | Kho đề là khái niệm của hệ thống, không của luật. **Ngoại lệ**: ràng buộc **bộ VCNV** thì CÓ gốc ở luật gốc — xem dòng dưới | `QĐ-041` `QĐ-042` `QĐ-043` `QĐ-044` `QĐ-082` |
| `GR-032` hàng đợi tín hiệu | Luật gốc có chuông vật lý; phần mềm phải định nghĩa thứ tự và quyền duyệt. §Kích hoạt tay là **bổ sung ngoài O26** — luật gốc chỉ có một người giành quyền mỗi câu | `QĐ-020` `QĐ-021` `QĐ-022` `QĐ-104` |
| `GR-033` mốc thời gian do admin bấm | Máy không quan sát được sân khấu ⇒ **admin là cảm biến** | `QĐ-006` `QĐ-027` `QĐ-028` |
| `GR-034` chuông chỉ nhận click chuột | Chống bấm nhầm khi đang gõ | `QĐ-023` |
| `GR-035` server time | Công bằng và dựng lại được | `QĐ-006` `QĐ-029` |
| `GR-036` mất kết nối và giữ ghế | Rủi ro của mạng, không có ở trường quay | `QĐ-045` `QĐ-046` `QĐ-047` |
| `GR-037` phạm vi hiển thị đáp án | Bảo mật đề — luật gốc không cần vì đáp án nằm trên giấy của MC. **Mốc công bố** thì có gốc ở luật: cửa sổ cướp quyền Về đích buộc mốc phải là *câu khép*, không phải *đã chấm*. Mốc còn **lùi thêm** khi có kích hoạt tay. **Bước (6) ghi audit** không có gốc ở luật chút nào — nó là yêu cầu **kiểm toán** của sản phẩm (`QĐ-133`), và vì thế nó không đổi kết cục của một ca nào. **§Kéo và đẩy** cũng thuần kiểm toán, không có gốc ở luật: nó chỉ nói dấu vết của C5 và C7 nằm bảng nào (`QĐ-145`) | `QĐ-048` `QĐ-051` `QĐ-062` `QĐ-080` `QĐ-104` `QĐ-133` `QĐ-145` |

---

# Biến thể bị loại

Giữ lại **chỉ để tránh cài nhầm**. Không dòng nào dưới đây là requirement.

| Chủ đề | Bản đã chọn | Bản bị loại | Vì sao loại |
|---|---|---|---|
| Cấu trúc Khởi động | **Hai lượt**: riêng 6 câu × 3 giây (sai 0) + chung 12 câu bấm chuông × 3 giây (sai −5) | Wikipedia: **một lượt 12 câu / 60 giây, sai không trừ** | Chủ dự án chọn Fandom; bản Wikipedia còn sai số học nội tại |
| Mức điểm Về đích | **20** và **30**; 20đ → 15 giây, 30đ → 20 giây | Wikipedia: **20** và **40**, 40đ → 30 giây | Cùng lý do; bản Wikipedia tự mâu thuẫn |
| Mở miếng ghép VCNV | *"trả lời đúng → mở"* | Wikipedia: *"**ít nhất 1 thí sinh** trả lời đúng → mở"* | Fandom là nguồn duy nhất |
| Câu hỏi phụ, trả lời sai | **Cả nhóm sang câu tiếp theo** | *"người bấm sai bị loại khỏi câu đó, những người còn lại thi tiếp cùng câu"* | Không có trong nguồn — là đề xuất trong bản nháp |
| Cướp quyền | **Chuyển điểm**: người cướp đúng **lấy** điểm của người sai | *"cộng thêm"* — người sai không mất gì | Không phải luật O26. Vẫn là **cấu hình hợp lệ**, nhưng **không** dùng cho preset O26 |
| Câu hỏi phụ hết câu | **Bốc thăm** | *"admin tự quyết"* | Cùng xử lý như trên |
| Độ phân giải *"đồng thời"* | **Mili-giây**, theo mốc server nhận | Hai chữ số thập phân — tiền lệ chương trình thật | Chủ dự án chọn mili-giây: máy tính dễ tính. Cái giá là cửa sổ hoà hẹp hơn 10 lần, **đã chấp nhận** |
| Giá trị của Athena | Giá trị O26 theo Fandom | Athena: VCNV 80/60/40/20 · Về đích 10/20/30 · Tăng tốc 30 giây/câu · Khởi động 60 giây/thí sinh | Athena chạy luật của mùa khác. Giữ dòng này **chỉ** để khỏi nhầm khi đọc code Athena |
| Máy tự chấm | **Không tồn tại** ở bất kỳ đâu | Bản nháp từng mô tả một cờ *"tự chấm"* bật được ở mọi trận | Ghi đè bởi `QĐ-010`. **Máy không bao giờ phán xử đáp án** |

---

# Biến thể ngoài luật O26 — cấu hình được, không bật mặc định

Engine hỗ trợ, preset O26 **không** dùng. Ghi ở đây để chúng không bị hiểu nhầm là luật.

| Biến thể | Thuộc mùa | Trạng thái |
|---|---|---|
| Khởi động kiểu **quỹ thời gian** — một thí sinh trả lời liên tục trong N giây | O21 | Cấu hình được; **không** trong preset O26 |
| VCNV **gợi ý ký tự** | O11-O12 | Cấu hình được; **không** trong preset O26 |
| Tăng tốc **clue-buzz** | — | Cấu hình được; **không** trong preset O26 |
| VCNV `rowCount` **5-8** hàng ngang | — | Cấu hình được. Đây là **đổi con số của luật**, khác hẳn việc hệ thống tự đẻ thêm câu |
| Câu hỏi phụ cho **nhiều nhóm hoà** | — | `tieBreakPositions` **v1 khoá cứng `[1]`** — chỉ phân định vị trí NHẤT (`QĐ-085`). Mở rộng được ở phiên bản sau |

> ⚠️ **v1 KHOÁ CỨNG ba thứ trong bảng này**: `rowCount` = **4** (`QĐ-068`), playlist = **bốn vòng chuẩn, đúng thứ tự** (`QĐ-069`), và `tieBreakPositions` = **`[1]`** (`QĐ-085`). Mô hình dữ liệu và RuleConfig vẫn nhận giá trị khác để phiên bản sau chỉ việc mở khoá — nhưng **engine-path chưa tồn tại** và cửa tạo contest **không cho chọn**. Cùng khuôn với `QĐ-007`: hạ tầng làm sẵn, luật khoá ở trường hợp v1.
>
> Riêng `tieBreakPositions`, thứ còn thiếu **không phải luật của một lượt phân định** — cơ chế Câu hỏi phụ vốn không phụ thuộc vị trí — mà là luật **điều phối nhiều lượt**: thứ tự giải nhiều nhóm hoà, ngân sách `3N` câu khi `N` chỉ biết được tại cú bấm chốt trận, và việc tái nhập vòng phân định vốn bị `GR-022` C7 chặn.

## Bổ sung ngoài O26 — LUÔN BẬT, không phải biến thể cấu hình

Khác bảng trên: mục dưới đây **không** tắt được và **không** phải tuỳ chọn. Nó là hạ tầng vận hành, không phải một cách chơi khác.

| Bổ sung | Vì sao không có trong luật gốc | Mã |
|---|---|---|
| **Admin kích hoạt tay một tín hiệu chuông khác** khi người giữ quyền bị chấm *Huỷ kết quả* — mọi vòng có giành quyền bằng chuông, gồm cả tín hiệu *"Mở chướng ngại vật"* | Luật gốc chỉ có **một** người giành quyền cho mỗi câu, và không có khái niệm *"huỷ một lượt giành quyền"* — trên trường quay, sự cố được xử bằng lời của MC chứ không bằng thao tác trên máy. Bản phần mềm cần một đường **hiện, có dấu vết** để làm đúng việc đó | `QĐ-104` |
| **Huỷ kết quả** thành lựa chọn phán quyết thứ ba cho tín hiệu Chướng ngại vật | Hệ quả trực tiếp của mục trên: cần một loại phán quyết mang nghĩa *"lượt này coi như không xảy ra"* mà **không** kích hoạt hình phạt bị-loại của `GR-010` | `QĐ-104` |

> **Vì sao vẫn trung thành với luật gốc.** Cả hai mục chỉ mở rộng **quyền can thiệp của người vận hành**, không đụng con số nào của luật: thang điểm, hình phạt, thời gian và điều kiện thắng thua giữ nguyên. Người được kích hoạt tay chịu **y hệt** số học của vòng.

---

# Không còn mục treo

**Quy mô viewer và ngưỡng độ trễ** — chủ dự án đã quyết là **chưa cần trả lời ở giai đoạn này** (`QĐ-067`). Đây là con số phi chức năng, chỉ đi vào hai chỗ và cả hai đều là **cấu hình**: ngưỡng rate-limit của cổng viewer, và mục tiêu kiểm thử tải. Không rule, không transition nào đọc nó. Từ `QĐ-088`, hai kênh public nhận đẩy **một chiều** và nằm ngoài kênh của lõi thi đấu ⇒ con số này chắc chắn **không chạm tới công bằng trận**.

**Luật cho số ghế TRÊN 4** không nằm ở đây vì nó **không phải câu hỏi còn treo** mà là **phạm vi phiên bản**: v1 đặc tả luật cho **tối đa 4 thí sinh**; schema, mô hình ghế và giao diện làm cho **1-12** ngay từ đầu; thang điểm cho hơn 4 đơn vị điểm thuộc **v1.5**. Trận **dưới** 4 thí sinh thì **thuộc v1** — chạy bằng **ghế bỏ thi**, vẫn áp nguyên luật 4 ghế. Xem `QĐ-007`, `QĐ-105`.
