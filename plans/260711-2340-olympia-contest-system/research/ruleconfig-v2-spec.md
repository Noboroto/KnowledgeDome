# RuleConfig v2 — Spec tổng quát hoá (round playlist)

> **v2.1 — 12/07, sau red-team v2.** User đã chốt: KHÔNG cắt scope, toàn bộ tính năng trong 1 version. Vì vậy mọi lỗ hổng red-team chỉ ra được vá bằng thiết kế trong bản này. Các điểm cần user xác nhận đánh dấu 🟡 (chi tiết ở `plans/DEFERED.md` D17-D19).
> File này là **spec engine chính**; `rules-2026.md` giữ vai trò luật gốc O26 + edge-cases (§6 của file đó đã bị thay bởi file này).
> Quyết định nền: teams config per-contest; bộ đề private = owner+ACL, password chỉ cho share-link; public set kèm đáp án theo setting per-set (default có); UI tối ưu 4 ghế, adaptive 1-12.

## 1. Nguyên tắc thiết kế

1. **Contest = Seats + Teams? + Round Playlist + Theme + Sound.** Không có số vòng/số thí sinh/số câu cứng trong engine.
2. Giá trị dạng "tùy chọn 30/45/60/90s" = **số nguyên tự do trong bounds Zod** + preset chips trên UI.
3. **Preset-driven để kìm test matrix**: engine hỗ trợ mọi tổ hợp Zod-hợp-lệ; test/UAT/demo chạy trên preset chuẩn (`O26_DEFAULT@1`, `O21_CLASSIC@1`, `TEAM_12@1`). Tổ hợp lạ được Zod + pre-flight chặn.
4. Phổ điểm theo thứ hạng là **mảng độ dài = số đơn vị điểm ĐÃ KHAI BÁO** (seats hoặc teams); runtime dùng **prefix** theo số đơn vị đang active (ghế trống/dropout không làm config invalid) — thống nhất với rules-2026 §7 "thứ hạng thực tế".

## 2. Cơ cấu thí sinh & đội

```ts
seats: SeatConfig[]                       // 1..12 ghế
teams?: { id, name, seatIds[] }[]         // optional; mỗi seat ∈ tối đa 1 team; team ≥1 người
scoringUnit: 'seat' | 'team'
individualTurnMode: 'all-members' | 'representative'
```

- Buzz/nhập đáp án LUÔN là cá nhân (mỗi thành viên một thiết bị); điểm reduce về `scoringUnit`.
- `representative`: đội trưởng chọn đại diện **per-round** trong lobby; đổi đại diện chỉ tại INTERMISSION (admin xác nhận). Về đích khi representative: **1 lượt/đội** do đại diện thi; all-members: mỗi thành viên 1 lượt.
- Pre-flight **warning** (không chặn) khi các đội lệch quân số mà playlist có turn individual + all-members (lệch cấu trúc điểm).

### 2b. Teams × từng vòng — ngữ nghĩa bắt buộc (vá C-v2-2)

| Tình huống | Luật (default, config được) |
|---|---|
| Khởi động chung / clue-buzz: 1 thành viên bấm sai | **Khoá chuông CẢ ĐỘI** cho câu đó (`teamLockout: true` default) — chặn lợi thế quân số 🟡 D17.1 |
| Tăng tốc ranked-speed khi team | **`last-wins` (user chốt 12/07)**: mọi thành viên gửi/gửi lại tự do đến server-timeout; đáp án của đội = **bản CUỐI CÙNG** (bất kỳ thành viên) theo server-received timestamp; ranking theo timestamp bản cuối |
| VCNV trả lời sai CNV | Loại **CẢ ĐỘI** khỏi phần thi khi scoringUnit=team (`wrongCnvEliminates` áp theo đơn vị điểm) 🟡 D17.3 |
| NSHV | **1 lần / đơn vị điểm / trận** (đội = 1 lần dù all-members) 🟡 D17.4 |
| Cướp điểm về đích | **CẤM cùng đội cướp** (same-team steal = lượt trả lời lại miễn phí — exploit); chỉ đơn vị điểm khác được bấm |
| Tie-break giữa các đội | Mỗi đội cử 1 người bấm chuông (representative của round đó; nếu all-members thì đội trưởng chỉ định trước khi tie-break bắt đầu) |
| Hàng ngang VCNV / câu chung khác | Mọi thành viên cùng trả lời; đội tính theo `teamSubmission` như trên |

## 3. Round playlist

```ts
rounds: RoundConfig[]     // Zod: min(1); thứ tự/số lượng/lặp loại tuỳ ý
type RoundConfig = KhoiDongConfig | VcnvConfig | TangTocConfig | VeDichConfig | TieBreakConfig
// chung: id, label hiển thị tuỳ ý (phục vụ cả D7 tránh thương hiệu), intermission trước/sau
```

- TieBreakConfig chỉ hợp lệ ở **cuối playlist** hoặc auto-trigger theo `tieBreakPositions` sau vòng cuối (Zod chặn đứng giữa).
- Orchestrator chạy tuần tự playlist; state `INTERMISSION` giữa các vòng (giữ từ gap-analysis 4.2).
- **`CONFIG_PATCH` giữa trận phải chạy lại pre-flight incremental trước khi apply; fail → reject** (vá H-v2-1).

## 4. Khởi động — 4 kiểu lượt

```ts
KhoiDongConfig = { type: 'KHOI_DONG', turns: Turn[] }
```

| kind | Cơ chế | Tham số | Điểm |
|---|---|---|---|
| `individual-timed` (O21 cũ) | 1 thí sinh (hoặc đại diện) trả lời liên tục trong quỹ thời gian | `perSeatSeconds` (30-300), `maxQuestions?` | `pointsCorrect` (+10); sai/bỏ qua: 0 (không penalty) |
| `individual-count` (O24+ riêng) | N câu, mỗi câu quỹ thời gian riêng | `questionCount`, `perQuestionSeconds` | +`pointsCorrect`; sai 0 |
| `common-timed` (O22) | Cả sân bấm chuông trong quỹ thời gian chung, hết câu này sang câu khác | `totalSeconds` (30-120), `answerSecondsAfterBuzz` | đúng +`pointsCorrect`; **sai `pointsWrong` (default −5)**, câu đó không mở lại chuông |
| `common-count` (O23/O24+ chung) | N câu bấm chuông | `questionCount`, `answerSecondsAfterBuzz`, `buzzWindowSec` | như common-timed |

Số lượt tuỳ ý, trộn kind tuỳ ý. Lượt individual chạy cho từng seat/đại diện theo §2.

### 4b. Rút bộ câu hỏi ngẫu nhiên (optional)

```ts
Turn.drawConfig?: {
  mode: 'manual' | 'draw',
  slots: { field: FieldId | 'RANDOM', count: number }[],
  noRepeatInMatch: true,
  shuffle: boolean,
}
```

- `Field` là taxonomy quản lý được (phase-03); `RANDOM` rút từ pool còn lại mọi lĩnh vực.
- Rút ở SERVER khi lượt bắt đầu → event `QUESTIONS_DRAWN` (replay được).
- **Pre-flight check (vá H-v2-2)** — thuật toán 2 bước: (1) trừ pool cho mọi slot có field cụ thể trên toàn playlist (greedy theo field); (2) phần dư phải ≥ tổng slot RANDOM; và pool mỗi lĩnh vực phải ≥ nhu cầu + `reservePerField` (default 2) để chừa **câu dự phòng** cho skip/substitute (skip cũng tiêu pool khi noRepeatInMatch). Fail → chặn start, báo thiếu cụ thể.

## 5. VCNV

```ts
VcnvConfig = {
  rowCount: 4..8, hasCenterPiece: boolean,
  rowPoints: number, rowThinkSeconds: number,
  cnvPointsByRowsOpened: number[],   // Zod: length == rowCount (+1 nếu hasCenterPiece), giảm dần
  revealCharHints: boolean,          // kiểu O11-O12
  wrongCnvEliminates: boolean,       // áp theo đơn vị điểm (§2b)
}
```

**Data model (vá H-v2-4):** `ObstacleSet` có `rowCount` (4-8), `rows[]` (đúng rowCount hàng), ảnh chia miếng theo rowCount (+center) — grid mask sinh động theo rowCount, KHÔNG cứng 5 miếng; `hintMap` per-row: vị trí ký tự trong đáp án hàng ngang thuộc CNV (render gợi ý khi revealCharHints). Pre-flight so `rowCount` của set với config.

## 6. Tăng tốc — 2 format

```ts
TangTocConfig = {
  questionCount: number,
  format: 'ranked-speed' | 'clue-buzz',
  // ranked-speed: cùng trả lời trên máy, xếp hạng tốc độ trong số đơn vị trả lời đúng.
  // LAST-WINS (user chốt 12/07): nhận MỌI submission đến server-timeout, KHÔNG khoá sau khi gửi;
  // đáp án tính điểm = BẢN CUỐI CÙNG per đơn vị điểm; ranking theo server-received timestamp của bản cuối.
  // Server time là source of truth duy nhất — submission tới sau server-timeout bị loại bất kể client hiển thị gì.
  // Chi tiết last-wins (gap-sweep H-F4):
  //  - Mọi submission TRIM trước khi xử lý (user chốt 12/07 — đề/đáp án trong kho cũng trim khi lưu).
  //  - Submission NỘI DUNG Y HỆT bản trước (so sánh SAU trim, per seat) → KHÔNG cập nhật timestamp.
  //  - Submission RỖNG (kể cả toàn whitespace sau trim) → SKIP, giữ bản trước (✅ user chốt D20.1).
  //  - Sửa đúng→sai sát giờ: đúng tinh thần last-wins — UI luôn hiện rõ "bản sẽ được tính" của mình.
  //  - Chấm (auto-match + admin confirm) chỉ chạy trên BẢN CUỐI sau TIME_UP; admin channel realtime collapse per-seat-latest (không spam mỗi lần gửi lại).
  rankPoints?: number[],       // length == số đơn vị điểm ĐÃ KHAI BÁO; runtime prefix (§1.4)
  tieRule?: 'share-high' | 'microsecond',
  // clue-buzz ("3-4 dữ kiện"): dữ kiện mở dần, bấm chuông bất kỳ lúc nào
  maxClues?: 3 | 4,
  cluePoints?: number[],       // Zod: length == maxClues; câu ít dữ kiện hơn dùng PREFIX (vá H-v2-5)
  clueIntervalSeconds?: number,
  wrongLocksOut: boolean,      // khoá khi sai; teamLockout (§2b) CHỈ có nghĩa khi wrongLocksOut=true (Zod refine chặn tổ hợp vô nghĩa — gap-sweep M-F8)
  defaultTimeSeconds: number,  // fallback khi câu không có timeSeconds
}
```

Câu clue-buzz trong kho có `clues[]` (2..maxClues, text/media) — pre-flight validate đủ.

## 7. Về đích

```ts
VeDichConfig = {
  packageMode: 'preset' | 'custom-build',
  packages?: { name, questionValues: number[] }[],   // preset: giá trị tuỳ ý (30/50/70/90...)
  questionsPerTurn: number, valueChoices?: number[], // custom-build từ các mốc
  defaultTimeByValue: Record<number, number>,        // Zod: valueChoices ⊆ keys; packages values ⊆ keys (vá H-v2-8)
  hopeStar: { enabled, multiplier, wrongPenaltyFactor },   // 1 lần/đơn vị điểm/trận (§2b)
  steal: { enabled, gainFactor, lossFactor, stealMode: 'add'|'transfer', windowSeconds }, // cấm same-team (§2b)
}
```

- `timeSeconds` là **thuộc tính từng câu** (câu "20 điểm loại 15s" và "loại 40s" cùng tồn tại); fallback `defaultTimeByValue`.
- **Pre-flight custom-build = worst-case per value** (vá H-v2-8): cần ≥ `số lượt × questionsPerTurn` câu cho MỖI mốc trong valueChoices (mọi người có thể cùng chọn một mốc) — hoặc admin bật `allowValueExhaustion` (mốc hết câu thì khoá lựa chọn đó, UI thí sinh thấy mốc bị mờ).

## 8. Tie-break (định nghĩa đầy đủ — vá thiếu sót spec cũ)

```ts
TieBreakConfig = {
  maxQuestions: number (default 3), thinkSeconds: number (default 15),
  tieBreakPositions: number[] (default [1]),
  exhaustedFallback: 'random-draw' | 'admin-decides' (default random-draw, admin xác nhận),
}
```

Chạy per nhóm hòa theo thứ tự vị trí (rules-2026 §7); teams: đại diện bấm chuông (§2b). Pre-flight: pool câu phụ ≥ `maxQuestions × số nhóm hòa tối đa có thể` (ước lượng = số đơn vị điểm − 1) — kế thừa D13.6.

## 9. Kho đề & Bộ đề (chi tiết phase-03/04) — vá C-v2-1, C-v2-3

- Question thêm: `displayId`, `fieldId`, `wordCount` (auto), `explanation`, `note`, `timeSeconds?`, `value?`, `clues[]?`, và **`everPublic: boolean` (cờ vĩnh viễn, một chiều)**.
- **QuestionSet**: `displayId`, visibility `PRIVATE|PUBLIC`, `everPublic` (một chiều).
  - PRIVATE: owner + **ACL 3 mức: `view` / `viewAnswer` / `export`** (export-kèm-đáp-án yêu cầu viewAnswer + export; export câu reference của setter khác yêu cầu quyền trên CÂU theo CASL conditions).
  - **Share-link**: token ≥128-bit random + password (rate-limit 5 lần/phút/IP+link, lockout) + TTL + revoke; share-link **mặc định KHÔNG kèm đáp án** (bật kèm đáp án là hành động riêng, cảnh báo); mọi truy cập share-link audit theo token+IP (không có user vẫn audit được).
  - PUBLIC: xem/tải tự do, kèm đáp án theo setting per-set (default có, cảnh báo trước khi public). **Public là một chiều về mặt bảo mật**: set + mọi câu trong set bị đóng dấu `everPublic=true` vĩnh viễn (un-public không gỡ dấu — đáp án đã có thể bị tải).
- **Chống rò đề sang thi đấu (vá C-v2-1, cả 4 đường vòng):** pre-flight của MỌI match **chặn** (hard-block, force cần confirm 2 bước + audit) khi snapshot chứa bất kỳ câu nào `everPublic=true` HOẶC đang xuất hiện trong set public/share-link-kèm-đáp-án còn hiệu lực. Kiểm ở đơn vị CÂU, không phải set; kiểm mỗi match (không phải mỗi contest); chiều gán-set-public-vào-contest cũng bị chặn ngay lúc gán.
- SetItem: reference câu kho theo displayId HOẶC nhập tay inline; inline "lưu vào kho" → vào **DRAFT** (đi qua duyệt như thường); inline dùng thẳng trong contest là chủ đích được phép (owner tự chịu trách nhiệm) — ghi rõ trong UI.
- `Question.visibility` (cột đặt sẵn từ gap 4.1) định nghĩa lại: **derived, read-only** = PUBLIC nếu câu đang thuộc ≥1 set public, else PRIVATE — không phải cột set tay (vá H-v2-10).
- **Tra cứu theo đáp án**: chỉ chạy trên tập câu user có `viewAnswer` theo CASL conditions (setter → câu của mình; admin → tất cả) — không phải permission toàn cục (vá H-v2-7).
- Excel: template quy ước per-round-type, roundtrip; export tuân ACL 3 mức ở trên.

## 10. Theming & Âm thanh

```ts
theme: { colors: TokenOverrides, contestLogo?, contestBanner?, seatPhotos, introVideo?, roundIntroVideos? }
sound: { cues: Record<CueSlot, AudioAssetRef|null>, backgroundTracks: AudioAssetRef[], volumes: {cues, background} }
```

- **Mọi asset theme/sound đi qua CHUNG media pipeline phase-03** (presign + magic-bytes sniff + cấm SVG + giới hạn: ảnh ≤10MB, video hình hiệu ≤200MB, audio ≤20MB) — vá H-v2-9.
- `colors`: Zod chỉ nhận **hex/rgb()/hsl() hợp lệ** (regex) — chặn CSS injection qua CSS variables (vá M-v2-6).
- CueSlot matrix: mỗi round type × (intro | question-appear | countdown-loop | last-5s | buzz | correct | wrong | timeup | reveal | round-end) + global (match-intro, intermission, podium, hope-star, cnv-solved). **Fallback chain: slot → global default → silent** (vá L-v2-1); bộ SFX mặc định royalty-free phủ nhóm global + cue cơ bản, slot không gán rơi về fallback.
- Soundboard: phát/dừng/next/volume/duck nhạc nền — **permission riêng `match.soundboard`** gán theo contest (ngoài control lock, co-host phụ trách được) — vá M-v2-5. Upload backgroundTrack hiện **disclaimer bản quyền** (nhất quán D10; ghi vào guide-admin) — vá M-v2-7.
- Nhạc nền phát ở viewer/overlay; contestant mặc định tắt.

## 11. MC view

Route `/mc` (permission `match.viewAnswer` theo contest): câu hỏi chữ rất to + đáp án + tóm tắt bảng điểm/kết quả vòng. Read-only. Đáp án rời server tới đúng 2 kênh: admin + MC (authenticated + audit).

## 12. Zod validation & pre-flight (tổng hợp)

- `rounds.min(1)`; TieBreak chỉ cuối playlist. `rankPoints/cluePoints` đúng độ dài như từng mục; mảng điểm giảm dần, số nguyên.
- `cnvPointsByRowsOpened.length == rowCount(+1)`; ObstacleSet.rowCount == config.rowCount; hintMap hợp lệ khi revealCharHints.
- `valueChoices ⊆ keys(defaultTimeByValue)`; packages values ⊆ keys.
- Draw: thuật toán 2 bước + reservePerField (§4b). Custom-build worst-case (§7). Tie-break pool (§8).
- Teams: mỗi team ≥1 seat; representative đã chỉ định cho mọi round cần nó; warning lệch quân số.
- everPublic hard-block (§9). CONFIG_PATCH re-preflight (§3).
- Preset immutable versioned (`O26_DEFAULT@1`); match snapshot config lúc start (H9 giữ nguyên).
