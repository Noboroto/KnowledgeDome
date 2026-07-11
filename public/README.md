# Olympia Contest System — Demo tĩnh

Bộ demo TĨNH (vanilla HTML/CSS/JS, không build step, không CDN) mô phỏng gameshow
"Đường lên đỉnh Olympia" **luật 2026** với mock data — dùng để duyệt/sửa design & animation
trước khi code hệ thống thật.

Rule-spec tham chiếu: `plans/260711-2340-olympia-contest-system/research/rules-2026.md`

## Chạy local

Bất kỳ static server nào đều chạy được:

```bash
npx serve public        # http://localhost:3000
# hoặc
python -m http.server 8000 --directory public
```

Mở file trực tiếp (`file://`) cũng chạy được vì không dùng ES modules/fetch.

## Deploy Vercel

```bash
cd public
npx vercel --prod       # framework preset: Other (static)
```

Hoặc trên dashboard: import repo → Root Directory = `public` → không cần build command,
Output Directory = `.` (toàn bộ là static asset).

## Bản đồ file

| File | Vai trò |
|---|---|
| `index.html` | Hub: giới thiệu, link 5 màn, bảng hotkey tổng hợp |
| `contestant.html` | Màn thí sinh — tối giản, phản hồi tức thì, keyboard-only |
| `viewer.html` | Màn sân khấu/khán giả — animation đầy đủ mọi vòng + kịch bản auto-play |
| `overlay.html` | OBS Browser Source 1920×1080 nền trong suốt (`?bg=checker` để xem ngoài OBS) |
| `admin.html` | Bàn điều khiển trận: stepper vòng, chấm điểm, chỉnh timer/điểm tay, duyệt viewer, log |
| `questions.html` | Kho đề: bảng + filter/search, modal editor, modal Import/Export ZIP (mock) |
| `assets/tokens.css` | **Design tokens** — màu, font, spacing, radius, shadow, duration/easing |
| `assets/base.css` | Reset + component chung (button, card, badge, modal, table, demo-panel) |
| `assets/mock-data.js` | `window.MOCK`: 4 thí sinh, kho câu hỏi đủ 4 vòng + câu phụ, viewer chờ duyệt |
| `assets/engine.js` | `window.Engine`: state machine, timer, **tính điểm đúng luật 2026**, pub/sub |

## Sửa design qua tokens.css

Mọi style đều tham chiếu CSS variables trong `assets/tokens.css` — sửa một chỗ, đổi toàn demo:

- **Màu sân khấu**: `--navy-900` (nền), `--grad-stage`, accent `--gold`, sai/trừ điểm `--red`.
- **Tốc độ animation**: `--dur-instant/fast/med/slow/epic` + easing `--ease-out/spring/in-out`.
  Ví dụ muốn mọi hiệu ứng sân khấu chậm hơn: tăng `--dur-epic`.
- **Chữ**: scale `--fs-*`; font mặc định là system-ui stack (hỗ trợ tiếng Việt sẵn, không tải font ngoài).
- **Spacing/bo góc**: `--sp-*`, `--r-*`.

`prefers-reduced-motion: reduce` tự tắt animation trang trí (khai báo trong `base.css`).

## Luật tính điểm trong engine (default 2026)

| Vòng | Điểm |
|---|---|
| Khởi động riêng (6 câu, 5s) | đúng +10, sai 0 |
| Khởi động chung (12 câu, chuông, 5s) | đúng +10, sai **−5**, câu đó không mở lại chuông |
| VCNV hàng ngang (15s) | +10/người đúng, mỗi hàng đúng mở 1 miếng ghép |
| VCNV giải CNV | đúng: 80/60/40/20 theo số hàng đã mở (0-1/2/3/4); sai: **bị loại** |
| Tăng tốc (4 câu, 10/20/30/40s) | 40/30/20/10 theo tốc độ trong số người đúng |
| Về đích (3 câu, gói 20/30) | đúng +giá trị; NSHV đúng ×2, sai −giá trị; cướp đúng +giá trị, sai −½ |
| Câu hỏi phụ | không cộng điểm trận, tối đa 3 câu, 15s |

Các giá trị nằm trong `Engine.RULES` (`assets/engine.js`) — hệ thống thật sẽ cho admin config per-contest.

## Hotkey nhanh

- **Thí sinh**: `Space` chuông · `Enter` gửi · `1–4` hàng ngang VCNV · `Esc` xoá input
- **Viewer**: `P` auto-play kịch bản · `→` từng bước
- **Overlay**: `1–9` bật/tắt phần tử
- **Admin**: `C` đúng · `X` sai · `S` start/pause timer · `←/→` chuyển câu
- **Kho đề**: `/` tìm kiếm · `N` câu hỏi mới

## Lưu ý

- Đây là demo mock: không network, không lưu trữ; mọi thao tác reset khi reload.
- Overlay chỉ animate `transform`/`opacity` để mượt trong OBS Browser Source.
- Các chỗ luật đánh dấu ⚠️ trong rules-2026.md (thời gian lượt riêng, mảng điểm CNV,
  cướp điểm có steal hay không…) đang dùng giá trị default — chờ chủ dự án chốt.
