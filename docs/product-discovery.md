# Sản phẩm

> **Phạm vi**: vấn đề, người dùng, mục tiêu, hành trình, epic, và **những gì giao diện phải làm** để các quyết định trong `decisions.md` thành hình.
>
> **Không chứa luật chơi** — luật ở `game-rules.md`. **Không chứa lý do** — lý do ở `decisions.md`, tra theo `QĐ-*`.
>
> **Không phải PRD.** Đây là đầu vào cho PRD, và là nơi giữ những phát biểu cấp sản phẩm chưa có FR tương ứng.

## Quan hệ với tài liệu khác

| File | Vai trò |
|---|---|
| `decisions.md` | **Vì sao** — `QĐ-001` → `QĐ-082` |
| `game-rules.md` · `game-state-machine.md` | Luật và vận hành |
| `traceability.md` | Truy nguyên rule ↔ luật gốc ↔ `QĐ-*` |
| `plans/**` | **Bản nháp.** Mọi trích dẫn dưới đây là **bối cảnh**, không phải requirement |

---

# 1. Vấn đề

Trường học và CLB muốn tổ chức thi đấu theo format Olympia hiện phải chọn giữa **PowerPoint thủ công** và một **phần mềm desktop LAN cũ** (bản Athena C#/WPF). Tám hạn chế cụ thể:

| # | Hạn chế |
|---|---|
| PS-1 | Luật hard-code — đổi luật là sửa code rồi build lại |
| PS-2 | Không chạy online: LAN-only, socket TCP thô |
| PS-3 | Không có kho đề dùng lại — file rời, media theo mã băm trên đĩa |
| PS-4 | Không tích hợp livestream |
| PS-5 | Mất kết nối là hỏng trận — không có cơ chế nối lại |
| PS-6 | "Bảo mật" đề chỉ là dịch ký tự — chống được mỗi Notepad |
| PS-7 | Chấm điểm hoàn toàn thủ công, không có công cụ đối chiếu đáp án |
| PS-8 | Trạng thái toàn cục dùng chung ⇒ không chạy được hai trận song song |

**Phát biểu cô đọng.** Người tổ chức không có công cụ nào vừa **(a)** cho tuỳ biến luật, **(b)** giữ kín đề tới đúng lúc công bố, **(c)** chạy realtime công bằng, và **(d)** lên hình livestream được.

**Chỗ chưa được chứng minh.** Không có nghiên cứu người dùng nào; toàn bộ phát biểu vấn đề rút từ suy luận và đối chiếu Athena. Chưa biết **có bao nhiêu đơn vị thực sự chờ sản phẩm này** và **ai là người dùng đầu tiên đã cam kết**. Xem `AS-1`.

---

# 2. Người dùng

## Vai trực tiếp dùng hệ thống

| ID | Vai | Nhu cầu cốt lõi |
|---|---|---|
| A-1 | **Admin** | Dựng contest, điều khiển trận, chấm điểm, can thiệp sự cố |
| A-2 | **Người ra đề** | Soạn câu hỏi của mình, gắn media, nhập/xuất phần đề phụ trách |
| A-3 | **Thí sinh** | Đăng nhập nhanh, bấm chuông, gõ đáp án, thấy phản hồi tức thì |
| A-4 | **MC** | Màn riêng chữ rất to: câu hỏi **và đáp án**. Read-only tuyệt đối |
| A-5 | **Khán giả** | Nhập mã 6 số là xem được — không tài khoản, không chờ duyệt |
| A-6 | **Máy dựng stream** | Overlay 1920×1080 nền trong suốt |
| A-7 | **Người phụ trách luyện tập** `[v1.5]` | Tạo trận practice, chạy lại nhanh |

**Không có vai "người duyệt đề" riêng** — admin duyệt `DRAFT` → `ACTIVE` (`QĐ-064`).

## Vai gián tiếp

**Ban tổ chức / trọng tài** quan tâm công bằng và phân xử khiếu nại. **Đơn vị tự host** chịu trách nhiệm pháp lý về dữ liệu. **Phụ huynh** quan tâm việc con lên hình.

*Ban giám khảo* không phải một vai riêng — họ **không thao tác hệ thống** (mô hình ba tầng: chỉ admin bấm), nên trùng với ban tổ chức về mặt hệ thống.

## Mô hình vai

**Vai hệ thống ≠ vai vận hành** (`QĐ-065`). Tài khoản mang **vai hệ thống**; *MC*, *người phụ trách luyện tập*, *host* là **quyền gán theo contest**, không phải vai seed. Trộn hai khái niệm là cách nhanh nhất để đếm sai số role và cấp thừa quyền.

Một tài khoản giữ được **nhiều vai**. **Ràng buộc loại trừ duy nhất**: tài khoản đang ngồi ghế thí sinh không được đồng thời là admin, MC hay người ra đề **trong contest đó** — admin và MC thấy đáp án. Setter kiêm MC thì **được**, chỉ cảnh báo.

**Quyền điều khiển gắn với PHIÊN, không với tài khoản** (`QĐ-008`): nhiều tài khoản admin, một phiên bấm được, các phiên khác xem.

---

# 3. Mục tiêu

| ID | Mục tiêu |
|---|---|
| G-1 | **Một engine, hai mục đích**: contest chính thức và luyện tập, phân biệt bằng mục đích trận |
| G-2 | **Nền tảng tuỳ biến**: mọi thời gian, số câu, mức điểm đều là cấu hình. **v1 khoá cứng** playlist 4 vòng, 4 hàng ngang, 4 thí sinh (`QĐ-007`, `QĐ-068`, `QĐ-069`); dữ liệu và schema làm cho **1-12** ngay từ đầu |
| G-3 | **Nút "Áp dụng luật 2026"** áp preset `O26_DEFAULT@1` |
| G-4 | **Điểm độc lập thời gian** — thời lượng là metadata từng câu, không suy ra từ mức điểm |
| G-5 | **Kho đề tập trung, bảo mật cao** — đáp án không bao giờ tới client trước lúc công bố |
| G-6 | **Realtime công bằng** — server time là nguồn sự thật duy nhất |
| G-7 | **Vận hành tin cậy** — hoàn nguyên được, phục hồi sau sự cố, audit đủ để phân xử |
| G-8 | **Trình diễn** — viewer, overlay OBS, màn MC, theming, âm thanh tuỳ chỉnh |
| G-9 | **Chuyển trọn gói** — soạn trên bản Internet, nhập vào bản portable ngày thi |
| G-10 | **Hai hình thức triển khai** — Docker compose và portable Windows LAN |

## Không làm

Không stream video (chỉ đẩy dữ liệu cho OBS) · không giải đấu nhiều trận, không bảng xếp hạng mùa · **không nhận dạng giọng nói để tự chấm** · không đa ngôn ngữ · không nhiều tổ chức trên một bản cài · không chặn sao chép đề · không tự chuyển mã media ở v1.

**Và bao trùm tất cả: không có bất kỳ đường nào máy tự chấm Đúng/Sai** (`QĐ-010`) — kể cả với câu gõ máy.

## Pháp lý

**Bản quyền format Olympia**: đã được chấp thuận. Sản phẩm dùng tên và cấu trúc vòng bình thường. Thiết kế trung lập vẫn giữ — tên vòng là dữ liệu trong RuleConfig — nhưng vì nó phục vụ **tính tuỳ biến**, không phải để né bản quyền.

**License repo**: không ghi ở giai đoạn này, đây là **sản phẩm nội bộ**. Xét lại nếu về sau mở mã hoặc bán.

**Nhạc nền do người dùng upload**: hệ thống không kiểm được nguồn gốc ⇒ giữ **disclaimer khi upload**, và nâng nó thành yêu cầu chính thức thay vì để ở mục rủi ro.

---

# 4. Hành trình

| # | Ai | Đi qua những gì |
|---|---|---|
| **J-1** Chuẩn bị kho đề | Người ra đề → Admin | Soạn câu theo ba kho, gắn metadata và media → `DRAFT` → **admin** duyệt → `ACTIVE` |
| **J-2** Dựng contest | Admin | Tạo contest → *"Áp dụng luật 2026"* → cấu hình từng vòng → gán ghế → **chọn danh sách câu** (bắt buộc; hệ thống không tự lấy đề) → theme và âm thanh → kiểm kho đề |
| **J-3** Chuyển sang portable | Admin | Xuất gói contest → mang sang máy portable → nhập → contest nháp → kiểm lại |
| **J-4** Vào phòng | Thí sinh · khán giả · OBS | Admin phát mã 6 số → thí sinh đăng nhập rồi nhập mã → thử chuông, thử âm thanh, báo sẵn sàng → khán giả và overlay nhập cùng mã |
| **J-5** Thi đấu | Admin chính; thí sinh và MC phụ | Mở vòng → hiển thị câu → start timer → thí sinh bấm chuông hoặc gõ → admin chấm → câu kế. Xen kẽ: chỉnh điểm, hoàn nguyên, mở màn công bố |
| **J-6** Xử lý sự cố | Admin | Thí sinh rớt mạng: giữ ghế 120 giây rồi tô nổi bật, admin quyết · **admin rớt mạng: quay lại và khôi phục từ server** (`QĐ-070`) · vòng hỏng: bỏ, chạy lại, hoặc kết thúc sớm · thiếu đề: sửa danh sách ở `LOBBY` |
| **J-7** Sau trận | Admin | Chốt trận → xuất biên bản PDF → thống kê ghi ngược kho đề → job dọn dữ liệu theo hạn |
| **J-8** Luyện tập `[v1.5]` | Người phụ trách · thí sinh | Tạo trận practice → luyện tập → chạy lại nhanh, giữ ghế và mã phòng |

**Hai hành trình chưa có**: **kết quả từ portable quay về máy chủ trung tâm** — trận chạy trên portable sinh ra kết quả và thống kê, hiện không có đường mang về; và **cài đặt lần đầu** — mới có giải pháp kỹ thuật tạo admin bằng dòng lệnh, chưa có trải nghiệm.

---

# 5. Epic

| Epic | Tên | Mốc |
|---|---|---|
| E-1 | Xác thực và phân quyền | v1 |
| E-2 | Kho đề và bộ đề | v1 |
| E-3 | Nhập/xuất và gói contest | v1 |
| E-4 | Contest builder và luật tuỳ biến | v1 |
| E-5 | Phòng thi và vòng đời trận | v1 |
| E-6 | Game engine và luật thi đấu | v1 |
| E-7 | Điều khiển và can thiệp của admin | v1 |
| E-8 | Trải nghiệm thí sinh | v1 |
| E-9 | Trình diễn: viewer · overlay · MC · theming · âm thanh | v1 |
| E-10 | Sau trận: kết quả, thống kê, phát lại | v1 *(phát lại: sau)* |
| E-11 | Dữ liệu cá nhân và quyền riêng tư | v1 *(job dọn: v1.5)* |
| E-12 | Vận hành và hai hồ sơ triển khai | v1 |
| E-13 | Luyện tập | **v1.5** |
| E-14 | Thi đội | **v2** |

**Hai epic chưa có yêu cầu nguồn**: **danh sách thí sinh trong gói contest** (mã hoá, hợp nhất tài khoản trùng, phiếu tài khoản) và **gói kết quả chiều ngược**. Cả hai chỉ tồn tại trong bản nháp.

---

# 6. Giao diện phải làm gì

Các quyết định ở `decisions.md` ràng buộc giao diện. Bảng dưới là **danh sách kiểm** khi viết yêu cầu chức năng — nó không đặt ra quyết định nào mới.

## Màn admin

| Yêu cầu | Vì sao | Epic |
|---|---|---|
| **Hai nút tuần tự cho mỗi câu** — *hiển thị câu* rồi *start timer*. Gộp lại là **phá luật**: cửa sổ chuông co lại bằng thời gian suy nghĩ và cửa sổ Ngôi sao hy vọng mất mốc đóng | `QĐ-028` | E-7 |
| **Ba hạng cảnh báo** — toast cho invalid state, dialog Yes/No cho việc không hoàn tác được, dialog phá huỷ cho bỏ/chạy lại/huỷ. Mỗi invalid state một thông điệp riêng | `QĐ-072` | E-7 |
| **Bản hợp lệ và bản quá hạn hiện cạnh nhau**, quá hạn tô đỏ, **nút chấm bật cho cả hai** | `QĐ-073` | E-7 |
| **Tăng tốc chấm trên một màn, bốn ghế cạnh nhau** | `QĐ-073` | E-7 |
| **Số nút chấm thay đổi theo vòng** — thêm *Huỷ kết quả* ở vòng có hình phạt. Không dùng chung một layout hai nút | `QĐ-061` | E-7 |
| **Nút chấm khoá tới hết giờ ở vòng gõ máy**, sống suốt ở vòng nói. Phải chỉ báo **vì sao** đang mờ | `QĐ-030` | E-7 |
| **Hàng đợi hiện đầy đủ** kèm timestamp và trạng thái *"không có hiệu lực"*; tách bạch **hàng đợi đang hoạt động** với **lịch sử tín hiệu** | `QĐ-024` `QĐ-025` | E-7 |
| **Lối vào sửa danh sách đề ở `LOBBY`**, và thông điệp thiếu đề dẫn thẳng sang đó | `QĐ-042` `QĐ-043` | E-5 |
| **Ba trạng thái câu phân biệt được**: chưa rút · đã rút chưa hiển thị *(gỡ được)* · đã hiển thị *(không gỡ được)* | `QĐ-044` | E-5 |
| **Nút mở/đóng màn công bố**, kèm gợi ý ở hai mốc: hết vòng, hết trận | `QĐ-049` | E-7 |
| **Chuyển quyền điều khiển** giữa hai phiên admin, ghi audit | `QĐ-008` | E-1 |

## Màn thí sinh

| Yêu cầu | Vì sao | Epic |
|---|---|---|
| **Bảng điểm của TẤT CẢ các ghế**, không chỉ điểm của mình — đây là chỗ dễ cài thiếu nhất | `QĐ-015` | E-8 |
| **Điểm cập nhật realtime** như viewer, gồm cả lúc hoàn nguyên làm điểm tụt đột ngột. Điểm âm hiện bình thường | `QĐ-012` `QĐ-015` | E-8 |
| **Không render** control ở invalid state; bấm **không phản hồi** | `QĐ-004` | E-8 |
| **Nút chuông và nút gửi có vòng đời NGƯỢC nhau** — chuông tự khoá khi bấm, nút gửi sống tới hết giờ. Đừng dùng chung một mẫu | `QĐ-023` `QĐ-029` | E-8 |
| **Màn VCNV có hai layout theo mode**, không phải một layout bật/tắt một nút | `QĐ-019` | E-8 |
| **Nút chọn hàng ngang có BA trạng thái**: sống · khoá chờ duyệt · **mở lại khi admin từ chối**. Trạng thái thứ ba dễ bị bỏ sót và bỏ sót là thí sinh mất lượt trái luật | `QĐ-019` `QĐ-022` | E-8 |
| **`Esc` không làm gì** ở mode sân khấu — không có ô nhập để xoá, và `Esc` không bao giờ là nút quay lại ở màn thi đấu | `QĐ-072` | E-8 |

## Viewer và overlay

| Yêu cầu | Vì sao | Epic |
|---|---|---|
| **Không báo gì về can thiệp của admin** — điểm và bàn cờ đổi đột ngột, không hiệu ứng, không giải thích. Người giải thích là MC | `QĐ-076` | E-9 |
| **Không thấy khuyến nghị lượt** — đó là lộ thứ tự sắp tới | `QĐ-076` | E-9 |
| **Lớp phủ công bố chồng lên** màn đang chạy mà không huỷ nó; áp cho **cả máy thí sinh** | `QĐ-049` | E-8 · E-9 |
| **Bảng xếp hạng chịu được điểm âm và đồng hạng**, không hard-code số ghế | `QĐ-012` `QĐ-049` | E-9 |
| **Overlay nhận đáp án cùng lúc và cùng điều kiện với viewer** — từ mốc **câu khép**, theo cờ reveal. Lệnh cấm tuyệt đối trước đây **đã gỡ** | `QĐ-080` | E-9 |

## Xuyên suốt

| Yêu cầu | Vì sao | Epic |
|---|---|---|
| **Một component chung cho nút một chiều** — chấm · start timer · chuông · chuyển câu. Ghi rõ đây là **khoá theo LUẬT CHƠI**, không phải chống double-submit, để lần rà quy tắc UX sau không ai gỡ nhầm | `QĐ-060` | E-7 · E-8 |
| **Permission riêng cho từng thao tác phá huỷ**, không suy ra từ *"là admin"* | `QĐ-078` | E-1 |
| **Danh sách `sound-cue` phủ cả sự kiện điều khiển**; slot trống là im lặng | `QĐ-079` | E-9 |
| **Biên bản in theo LẦN CHẠY** kèm nhãn; event hoàn nguyên hiện như mọi event khác | `QĐ-077` | E-10 |
| **Audit mọi thao tác, mọi vai** | `CLAUDE.md` | E-11 |

---

# 7. Giả định chưa được kiểm chứng

| ID | Giả định | Vì sao đáng ngờ |
|---|---|---|
| AS-1 | **Có nhu cầu thị trường thực** | Không có nghiên cứu người dùng, không có người dùng đầu tiên cam kết |
| AS-2 | **Năm trận song song là nhu cầu thật** | Con số được chốt mà không dẫn nguồn nhu cầu. Với mô hình một bản cài cho một đơn vị, năm trận cùng lúc là bất thường |
| AS-3 | **Admin chấp nhận chọn tay toàn bộ đề trước mỗi trận** | Hệ thống cố ý không tự lấy đề. Chưa ai ước lượng số câu phải chọn cho một trận chuẩn |
| AS-5 | **Preload mã hoá qua service worker chạy ổn định** | Chính nguồn xếp nó là hạng mục phức tạp nhất phía client, và đã phải chuẩn bị phương án lui |
| AS-6 | **Mã 6 số công khai là chấp nhận được về quyền riêng tư** | Ai có mã đều thấy tên và trường lớp của học sinh vị thành niên. Chỉ có tuỳ chọn dùng biệt danh, không bật mặc định |
| AS-7 | **Người tổ chức có sẵn nhạc và hiệu ứng âm thanh** | Slot trống là im lặng, không có bộ mặc định ⇒ sản phẩm ra mắt sẽ **hoàn toàn im lặng** nếu admin không chuẩn bị |
| AS-9 | **Wiki nguồn chính xác và ổn định** | Wiki cộng đồng, sửa được bất kỳ lúc nào, không có phiên bản. Đối phó: dùng **bản lưu trong `source/`**, không dùng URL sống |
| AS-10 | **Thí sinh có thiết bị riêng để gõ** | VCNV và Tăng tốc **luôn** gõ máy ⇒ mỗi ghế cần một thiết bị nhập liệu đầy đủ cho cả trận, kể cả ở mode sân khấu. Chưa nguồn nào nêu yêu cầu phần cứng tối thiểu |

**Hai giả định đã được trả lời**: *"một admin đủ vận hành một trận"* — nay quyền gắn với **phiên** nên có người thay thế (`QĐ-008`), số người vận hành là khuyến nghị quy trình, không phải ràng buộc hệ thống. Và *"demo tĩnh `public/` là tham chiếu thiết kế tin cậy"* — **đã bác**: demo lệch tài liệu thì demo sai.

---

# 8. Phạm vi MVP — đề xuất

> **Đây là ý kiến, không phải requirement.** Chủ dự án đã chốt v1 gồm 10 phase và đã từ chối cắt scope một lần.

**Nhận xét thẳng**: v1 hiện tại không phải MVP mà là một sản phẩm hoàn chỉnh. Đã cắt thi đội và luyện tập, nhưng vẫn ôm **cả bốn biến thể Khởi động, hai format Tăng tốc, preload mã hoá, hai hồ sơ triển khai, theming và soundboard đầy đủ**. Phần lớn là **biến thể** của tính năng, không phải tính năng.

Ba khoá cứng của v1 (`QĐ-007`, `QĐ-068`, `QĐ-069`) **đã cắt sẵn một phần** việc này — bốn thí sinh, bốn hàng ngang, bốn vòng.

**Nguyên tắc nếu cắt tiếp**: một đường đi hoàn chỉnh từ soạn đề tới xuất kết quả; **cắt biến thể, không cắt giai đoạn**.

| Epic | Cắt được gì |
|---|---|
| Kho đề | Đánh phiên bản khi sửa câu đã duyệt |
| Nhập/xuất | Gói contest trọn gói, danh sách thí sinh, gói kết quả |
| Contest builder | Playlist tuỳ ý *(đã khoá bởi `QĐ-069`)*; biến thể vòng |
| Trình diễn | Theming và soundboard |
| Sau trận | Thống kê ghi ngược, phát lại |
| Triển khai | **Chọn một hồ sơ** |

**Hai cảnh báo về chính đề xuất này.** Cắt **màn MC** là cắt vào chỗ đau — MC là người đọc câu hỏi, không có `/mc` thì MC phải nhìn màn admin và hỏng luồng J-5; nếu chỉ giữ được một thứ ngoài viewer, **giữ `/mc`**. Cắt **âm thanh** thì hợp lệ về thiết kế nhưng sản phẩm sẽ demo rất tệ.

---

# 9. Đo thế nào

## Đã có trong nguồn — đều là tiêu chí kỹ thuật

Một trận thử với người thật, trọn bốn vòng kèm livestream, không sự cố chặn trận · điểm cuối trận khớp 100% với tính tay ở ba kịch bản tự động · admin đổi luật qua giao diện và trận chạy theo giá trị mới · bộ kiểm *"không rò đáp án"* pass.

**Không tiêu chí nào đo giá trị với người dùng.**

## Đề xuất bổ sung

| ID | Đo gì | Ngưỡng gợi ý |
|---|---|---|
| SM-1 | Thời gian dựng một contest hoàn chỉnh khi kho đề đã có | ≤ 30 phút |
| SM-2 | Tỉ lệ admin dựng được contest lần đầu **không cần hỏi ai** | ≥ 4/5 người |
| SM-3 | Số câu phải chọn tay cho một trận chuẩn | Đo trước, đặt ngưỡng sau — kiểm chứng `AS-3` |
| SM-4 | Tỉ lệ tái sử dụng câu hỏi sau 5 trận | ≥ 30% — chứng minh `PS-3` đã giải |
| SM-5 | Số lần admin phải **ép qua cảnh báo** trong một trận | Đếm riêng loại này, không gộp với điều chỉnh điểm |
| SM-6 | Số lần trận gián đoạn vì **lỗi hệ thống** | 0 |
| SM-7 | Tỉ lệ thí sinh nối lại thành công trong ngưỡng chờ | ≥ 95% |
| SM-8 | Sự kiện bị khiếu nại có **đủ timestamp và chuỗi thao tác** trong log | 100% |
| SM-9 | Số đáp án rò ra kênh không có quyền | 0 |
| SM-10 | Thời điểm sớm nhất thí sinh có thể thấy nội dung đề trước lúc công bố | Không bao giờ — kiểm chứng `AS-5` |
| SM-11 | Số trận thật trong ba tháng đầu | ≥ 3 — kiểm chứng `AS-1` |
| SM-12 | Nhân sự tối thiểu vận hành được một trận | Ghi nhận thực tế |

> **`SM-5` đã đổi nghĩa.** Trước đây nó đếm *"số lần admin can thiệp ngoài luật"* và ngầm coi can thiệp là bất thường. Nhưng can thiệp **chính là mô hình vận hành** (`QĐ-002`) — điều chỉnh điểm và chọn lượt là việc bình thường, không phải dấu hiệu hỏng. Thứ đáng đếm là **số lần ép qua một cảnh báo**: đó mới là lúc hệ thống và người vận hành bất đồng.

**Không đặt metric** về adoption hay tăng trưởng khi chưa có người dùng đầu tiên cam kết.

---

# 10. Còn phải làm

| # | Việc | Chặn cái gì |
|---|---|---|
| 1 | **Migrate các phát biểu ở đây vào `docs/PRD.md`** — nhiều yêu cầu ở §6 chưa có yêu cầu chức năng tương ứng, đặc biệt nhóm điều khiển của admin | Viết PRD |
| 2 | **Chốt hoặc đóng** danh sách thí sinh trong gói contest, và gói kết quả chiều ngược | E-3, và có hay không hành trình *portable → trung tâm* |
| 3 | **Chạy gate người thật** — một giáo viên và hai học sinh thử 30 phút | Kiểm chứng `AS-1`, `AS-3`, `AS-10` |
| 4 | **Cơ chế phát hiện wiki nguồn thay đổi**, hoặc chấp nhận bản lưu là chốt chặn cuối | `AS-9` |

---

# 11. Nguồn đã đọc

Toàn bộ `plans/260711-2340-olympia-contest-system/` — yêu cầu sản phẩm, user story, sổ hoãn `D1`→`D27`, lộ trình, và thư mục `research/` gồm luật O26, spec engine, khoảng trống sản phẩm, khoảng trống UX, ba vòng red-team, khảo sát Athena, kiến trúc kỹ thuật, hiệu ứng âm thanh, và các bản bàn về hàng đợi. Cộng với `CLAUDE.md`.

**Toàn bộ là bản nháp.** Không dòng nào ở trên được trích như requirement đã chốt.
