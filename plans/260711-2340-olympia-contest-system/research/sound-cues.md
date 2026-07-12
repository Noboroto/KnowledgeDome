# Sound Cue Map — event-driven (research 12/07, theo yêu cầu D10)

> Nguồn: source Athena (`AICtrlLib/Sound.cs` + *UI.cs — bản mô phỏng cũ là bản đồ event thực chiến, có file:line), Fandom wiki (402 — chỉ xác nhận tồn tại trang "Âm thanh"/"Danh sách đoạn nhạc"), Wikipedia/VTV. Admin sẽ tự upload file — hệ thống chỉ cần đúng CUE SLOTS.

## Cue map chính (rút gọn — đầy đủ 36 cue trong bảng dưới)

### Global (match-level)
| Slot | Thời điểm | Loại | Nguồn |
|---|---|---|---|
| `match.intro` | Mở đầu chương trình, giới thiệu thí sinh | jingle/opening | Wikipedia (nhạc hiệu Hoàng Vân/Lưu Hà An) |
| `match.result-display` | Màn hiển thị kết quả tổng | loop nền | Athena `ResultUI.cs:59` ShowingResult.mp3 |
| `match.ending` | Kết thúc chương trình | ending theme | Athena `MainUI.cs:129` Ending.mp3 |
| `intermission` | Giữa các vòng/giải lao | loop nền | không có trong Athena — bổ sung theo plan |
| `podium` | Trao giải/vinh danh | jingle lớn | không rõ trong Athena — giữ slot |

### Per-round (mỗi loại vòng một bộ, admin upload riêng)
| Slot | Thời điểm | Loại | Nguồn Athena tiêu biểu |
|---|---|---|---|
| `intro` | Mở vòng | jingle | Start.cs:205, Obstacle.cs:102, AccelerationUI.cs:147, FinishUI.cs:243 |
| **`player-select`** (MỚI) | Gọi thí sinh vào lượt (bản thường + bản thí sinh CUỐI khác nhau) | jingle | StartUI.cs:222 PStart / :315 FStart; FinishUI.cs:293 Player |
| `question-appear` | Câu hỏi hiện | sting | AccelerationUI.cs:259 Show |
| **`row-select`** (MỚI — VCNV) | Chọn hàng ngang | sting | ObstacleUI.cs:244 RowChose |
| **`row-show`** (MỚI — VCNV) | Lưới ô chữ hiện | sting | ObstacleUI.cs:233 RowShow |
| `countdown-loop` | Nhạc nền đếm giờ suy nghĩ | loop | Start.cs:326, ObstacleUI.cs:261, AccelerationUI.cs:181 OnTime |
| `last-5s` | 5s cuối | loop gấp | không có file riêng trong Athena (dùng OnTime) — giữ slot, admin tuỳ |
| `buzz` | Bấm chuông giành quyền | SFX | không có trong Athena (chuông vật lý) — giữ slot |
| `correct` / `wrong` | Chấm đúng/sai | jingle | *UI.cs Right/Wrong.mp3 (mỗi vòng bộ riêng) |
| `reveal` | Công bố đáp án (chỉ khi revealAnswerAfterJudge) | sting | ObstacleUI.cs:255 ShowAnswer, AccelerationUI.cs:188 Answer |
| **`packet-select`** (MỚI — Về đích) | Chọn gói điểm | sting | FinishUI.cs:297 Show |
| **`value-announce`** (MỚI — Về đích) | Công bố giá trị câu (file riêng THEO MỨC ĐIỂM) | jingle + loop | FinishUI.cs:371 `Finish/{value}.mp3` — slot nhận map value→file |
| **`hope-star`** (chuyển từ global về per-round Về đích) | Kích hoạt NSHV | sting | FinishUI.cs:346 Star; ObstacleUI.cs:375 Activate |
| **`steal-chance`** (MỚI — Về đích) | Mở quyền cướp / chuyển lượt người khác | sting | FinishUI.cs:392 Other, :396 Chance |
| `cnv-solved` / `cnv-failed` (VCNV) | Giải đúng/trượt CNV | jingle lớn | ObstacleUI.cs:381 RightObstacle / :385 WrongObstacle |
| `round-end` | Kết vòng | sting | StartUI.cs:276 Finish, FinishUI.cs:426 |

### Slot điều chỉnh so với matrix cũ
- **Thêm 7 slot**: `player-select`, `row-select`, `row-show`, `packet-select`, `value-announce` (nhận map theo mức điểm), `steal-chance`, `match.result-display`/`match.ending` (tách khỏi podium).
- **`timeup`**: Athena không có âm riêng khi hết giờ — GIỮ slot (chương trình thật có thể có), fallback silent.
- **4 kênh phát của Athena đáng học**: BackPlay (nền, loop, volume thấp), ForePlay (one-shot ưu tiên cao), TFPlay (đúng/sai), ContentPlay — map sang thuộc tính slot: `{ channel: 'background'|'foreground', loop, duckBackground }`.

## Ánh xạ vào engine

- Engine emit `sound-cue { slot, roundType, meta: { value?, isCorrect?, isFinalPlayer? } }` cùng lúc state đổi; client resolve file theo sound config + fallback chain slot → global default → **silent** (admin chưa upload = im lặng, không lỗi).
- Cấu trúc lưu file đề xuất: `sound/{contestId}/{roundType}/{slot}/file.mp3` (value-announce: `{slot}/{value}.mp3`).

## Câu hỏi mở (không chặn)
- Tiếng chuông buzz: Athena dùng chuông vật lý — hệ thống mới cần SFX (admin upload).
- Nhạc 5s cuối có file riêng ở chương trình thật? Slot đã có, admin quyết.
