# Scout report — Athena-Intelligent-Olympia (bản C#/WPF cũ)

> Nguồn: quét repo D:\Github\Athena-Intelligent-Olympia. Mục đích: rút bài học cho bản viết lại TypeScript.

## Kiến trúc cũ

- Desktop WPF, LAN-only, TCP socket thô port 2644, protocol tag tự chế `[<key>payload]<key>`, ASCIIEncoding + bảng thay thế dấu tiếng Việt (hack).
- 3 vai máy: **AIServer** (bàn điều khiển MC + toàn bộ game logic), **AIClient** (màn hiển thị cho thí sinh 0-3 / MC 4 / viewer 5-6), **AIDataManager** (soạn đề).
- State machine thủ công: mỗi vòng một WPF Page với `enum States` + `DispatcherTimer` 10ms poll, trộn lẫn animation + logic + net + nhạc trong một `switch` khổng lồ.

## Giá trị luật CŨ hard-code trong Athena (để đối chiếu)

| Vòng | Timer | Điểm |
|---|---|---|
| Khởi động | 60s/thí sinh (1 lượt riêng liên tục) | +10/câu, cần ≥20 câu kho |
| VCNV | — | hàng ngang +10; CNV 80/60/40/20 theo số hàng đã mở |
| Tăng tốc | 30s/câu | 40/30/20/10 theo tốc độ |
| Về đích | time = value/2 + 5 (10đ→10s, 20đ→15s, 30đ→20s) | gói 10/20/30; NSHV ×2/−value; cướp đúng +value, sai −value/2 |
| Câu phụ | 15s | +1đ/câu |

## Data & "bảo mật" đề

- Không có database: file `.etai` = text serialize tag tự chế rồi **char-shift +1372** (Caesar, không phải mã hoá thật). Media rời trên đĩa theo hashcode 6 số (`Tests\Images\<hash>.png`, `Clips\<hash>.mp4`, `Sounds\<hash>.mp3`).
- Import Excel bằng EPPlus, vùng ô tuyệt đối cứng `A1:H136`, layout hàng cố định — cực kỳ mong manh.
- "Bảo mật" chỉ chống mở bằng Notepad. Bài học: bản mới giữ đáp án ở server, không bao giờ gửi client trước công bố; media qua presigned URL.

## Pain points chính cần tránh

1. Buffer socket cố định 1024 byte, không framing → message dài là hỏng; không reconnect (static TcpClient duy nhất).
2. Global mutable static state → không chạy 2 trận song song, không test được.
3. Điểm/timer hard-code khắp nơi; đổi luật = sửa code + build lại.
4. State lưu trong Text của UI control (parse TextBlock lấy điểm!).
5. Side-effect trong property setter (set ảnh = ghi file; set câu hỏi = gửi net + phát nhạc + save).
6. Chấm điểm hoàn toàn thủ công bằng chuột, không auto-match đáp án.

## Điểm mạnh đáng kế thừa

- Tách rõ 3 vai: control (admin/MC) / display (thí sinh, viewer) / editor (soạn đề) — map thẳng sang web.
- Mô hình 5 vòng đúng chuẩn Olympia; media theo id.
- Ý tưởng "gói đề theo vòng" (Start/Obstacle/Acceleration/Finish/Extra) với số câu tối thiểu mỗi kho.
