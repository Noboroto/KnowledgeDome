# Olympia Contest System — Demo tĩnh

Bộ demo TĨNH (vanilla HTML/CSS/JS, không build step, không CDN) mô phỏng gameshow
"Đường lên đỉnh Olympia" theo **spec v2 (RuleConfig tổng quát hoá)** với mock data —
dùng để duyệt/sửa design & animation trước khi code hệ thống thật.

Spec tham chiếu:
- `plans/260711-2340-olympia-contest-system/research/ruleconfig-v2-spec.md` (engine chính)
- `plans/260711-2340-olympia-contest-system/research/rules-2026.md` (luật gốc O26)

## Chạy local

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
| `index.html` | Hub 2 chiều: theo **ROLE** (6 màn) và theo **VÒNG THI** (deep-link `?round=…` mở đúng trạng thái) |
| `contestant.html` | Màn thí sinh — tối giản, phản hồi tức thì; có state clue-buzz, VCNV 4/8 hàng, câu phụ |
| `viewer.html` | Màn sân khấu — kịch bản 22 bước + switcher **4/8/12 thí sinh · chế độ đội**, tăng tốc 2 format, VCNV 4/8 hàng + gợi ý ký tự |
| `overlay.html` | OBS Browser Source 1920×1080 nền trong suốt (`?bg=checker` xem ngoài OBS, `?round=…` đổi banner) |
| `admin.html` | Bàn điều khiển: stepper render từ **playlist** (mock-data), chấm điểm chống double-submit, **soundboard** mock, bảng điểm adaptive |
| `mc.html` | **MỚI** — màn MC read-only: câu hỏi chữ RẤT to + khung đáp án "chỉ MC/admin thấy" + tóm tắt điểm |
| `questions.html` | Kho đề: 2 tab **Câu hỏi** (pagination 10 dòng, cột displayId/lĩnh vực/số từ) và **Bộ đề** (PRIVATE/PUBLIC, share-link, cảnh báo everPublic) |
| `assets/tokens.css` | **Design tokens** — màu, font, spacing, radius, shadow, duration/easing, z-index, toast, pagination |
| `assets/base.css` | Reset + component chung (button, card, badge, modal, table, **toast, spinner, nav-back, pager**) |
| `assets/ui.js` | **MỚI** — `window.UI`: toast (loading → success/error), `mockAsync` (delay 300-600ms + khoá nút), hotkey Esc/H, query param |
| `assets/mock-data.js` | `window.MOCK`: 12 thí sinh, **teams**, **playlist**, soundboard tracks, clues tăng tốc, VCNV 4/8 hàng + hintMap, fields, sets bộ đề |
| `assets/engine.js` | `window.Engine`: state machine, timer, tính điểm đúng luật, **clue-buzz**, **teamScores** (reduce điểm về đội), pub/sub |

## 4 UX rules áp toàn demo

1. **Visibility of System Status** — mọi thao tác async-mock (gửi đáp án, chấm điểm, lưu câu hỏi,
   kick viewer, share-link, public bộ đề…) giả lập delay 300-600ms và hiện spinner/toast loading.
2. **Immediate Feedback** — UI phản hồi ≤100ms (`--dur-instant`); toast success/error sau MỖI action
   (pattern loading → success/error trong `assets/ui.js`).
3. **Không chặn gửi lại (rule chống double-submit ĐÃ BỎ theo yêu cầu chủ dự án)** — nút chỉ hiện
   spinner báo trạng thái, không disable; tăng tốc gửi lại thoải mái, hệ thống tính **BẢN CUỐI CÙNG**
   trước server-timeout. **Server time là source of truth duy nhất.** Nút chuông CHỈ nhận click chuột.
4. **Viewport-Conscious Layout** — reference laptop **1440×900** (~844px usable): nội dung chính
   mỗi màn gọn 1 viewport; bảng kho đề **phân trang 10 dòng**; hub gập bảng hotkey vào `<details>`.

## Điều hướng

- Mọi trang có nút **← Hub** ở top-left (overlay: chỉ hiện với `?bg=checker`).
- **Esc** = quay lại trang trước/hub. Nếu đang mở modal → Esc đóng modal trước. Trong ô nhập → Esc xoá ô nhập (thí sinh).
- **H** = về hub (bỏ qua khi đang gõ trong input).

### Deep-link theo vòng (`?round=`)

Giá trị: `khoidong` · `vcnv` · `tangtoc` · `vedich` · `cauphu`

| Trang | Hành vi khi có `?round=` |
|---|---|
| `contestant.html` | Nhảy thẳng state vòng đó (`&format=clue` cho tăng tốc clue-buzz) |
| `viewer.html` | Nhảy tới bước setup vòng đó trong kịch bản (câu phụ dựng view riêng) |
| `mc.html` / `admin.html` | Chọn đúng vòng trong playlist |
| `overlay.html` | Đổi banner vòng thi |
| `questions.html` | Preset filter vòng thi (`?tab=sets` mở tab Bộ đề) |

## Demo tính năng spec v2

- **Adaptive 4/8/12 + đội** — viewer (panel demo) & admin (trên bảng điểm): nút `4 / 8 / 12 / 👥 Đội`.
  ≤4 giữ layout sân khấu; 5-12 grid gọn 2 cột; chế độ đội = 12 người → 4 đội × 3, điểm đội TO + thành viên nhỏ.
- **Tăng tốc clue-buzz** — viewer panel demo: `⇄ Ranked-speed ↔ Clue-buzz`, `＋ Mở dữ kiện tiếp`,
  mock chuông đúng/sai (điểm mốc 40/30/20/10, sai khoá chuông). Contestant: state "Tăng tốc clue-buzz"
  tự mở dữ kiện mỗi 4s, chuông MỞ suốt.
- **VCNV 4/8 hàng** — viewer panel demo: `⇄ 4 hàng ↔ 8 hàng` (miếng ghép sinh động theo số hàng,
  không cứng 5 miếng) + `💡 Gợi ý ký tự CNV` (ô ký tự thuộc CNV lật ra màu tím).
- **Soundboard** (admin, cột trái): play/pause, next, mute, volume slider — mock, có progress bar.
- **Playlist stepper** (admin): render từ `MOCK.playlist`, thêm/bớt/lặp vòng chỉ cần sửa mock-data.
- **Bộ đề** (questions → tab Bộ đề): badge PRIVATE/PUBLIC/`⚠ everPublic`/`🔗 link`;
  share-link modal (link + password + cảnh báo "không kèm đáp án");
  nút Public hiện cảnh báo đỏ "public = công khai đáp án vĩnh viễn".
- **MC view** (`mc.html`): route read-only theo spec §11 — đáp án chỉ tới admin + MC.

## Sửa design qua tokens.css

Mọi style đều tham chiếu CSS variables trong `assets/tokens.css` — sửa một chỗ, đổi toàn demo:

- **Màu sân khấu**: `--navy-900` (nền), `--grad-stage`, accent `--gold`, sai/trừ điểm `--red`.
- **Tốc độ animation**: `--dur-instant/fast/med/slow/epic` + easing `--ease-out/spring/in-out`.
- **Chữ**: scale `--fs-*`; font mặc định system-ui stack (hỗ trợ tiếng Việt sẵn, không tải font ngoài).
- **Toast/pagination**: `--toast-w`, `--toast-bg`, `--z-toast`, `--page-btn`.

`prefers-reduced-motion: reduce` tự tắt animation trang trí (khai báo trong `base.css`).
Overlay chỉ animate `transform`/`opacity` để mượt trong OBS Browser Source.

## Luật tính điểm trong engine (default 2026 + v2)

| Vòng | Điểm |
|---|---|
| Khởi động riêng (6 câu, 5s) | đúng +10, sai 0 |
| Khởi động chung (12 câu, chuông, 5s) | đúng +10, sai **−5**, câu đó không mở lại chuông |
| VCNV hàng ngang (15s) | +10/người đúng, mỗi hàng đúng mở 1 miếng ghép |
| VCNV giải CNV | đúng: 80/60/40/20 theo số hàng đã mở; sai: **bị loại** (theo đơn vị điểm) |
| Tăng tốc ranked-speed | 40/30/20/10 theo tốc độ trong số người đúng |
| Tăng tốc clue-buzz | 40/30/20/10 theo **mốc dữ kiện** đang mở khi bấm chuông; sai khoá chuông |
| Về đích (gói 20/30) | đúng +giá trị; NSHV đúng ×2, sai −giá trị; cướp đúng +giá trị, sai −½ |
| Câu hỏi phụ | không cộng điểm trận, tối đa 3 câu, 15s |
| Chế độ đội | buzz/nhập đáp án cá nhân → `Engine.teamScores()` reduce về đội |

Các giá trị nằm trong `Engine.RULES` (`assets/engine.js`) — hệ thống thật cho admin config per-contest (Zod bounds).

## Hotkey nhanh

- **Mọi trang**: `Esc` quay lại (đóng modal trước) · `H` về hub
- **Thí sinh**: chuông = **click chuột (không hotkey)** · `Enter` gửi · `1–8` hàng ngang VCNV · `Esc` (trong ô nhập) xoá
- **Viewer**: `P` auto-play kịch bản · `→` từng bước
- **Overlay**: `1–9` bật/tắt phần tử
- **Admin**: `C` đúng · `X` sai · `S` start/pause timer · `←/→` chuyển câu
- **MC**: `←/→` chuyển câu
- **Kho đề**: `/` tìm kiếm · `N` câu hỏi mới

## Lưu ý

- Demo mock: không network, không lưu trữ; mọi thao tác reset khi reload.
  Delay 300-600ms là GIẢ LẬP để thấy loading state — bản thật là network thật.
- Các điểm spec đánh dấu 🟡 trong ruleconfig-v2-spec.md (teamLockout, teamSubmission…)
  đang dùng giá trị default — chờ chủ dự án chốt (D17.x trong DEFERED.md).
