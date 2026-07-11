---
phase: 6
title: "Game Engine (luật 2026)"
status: pending
priority: P1
dependencies: [3, 5]
---

# Phase 6: Game Engine (luật 2026)

## Overview
Trái tim hệ thống: state machine server-authoritative cho 4 vòng + câu phụ theo `research/rules-2026.md`, RuleConfig admin chỉnh được, timer server, buzzer công bằng, chấm điểm (auto + manual), event-sourced match log cho undo/audit/replay, pause/resume, backup state.

## Requirements
- Functional: chạy đủ 5 phần thi đúng luật default O26; admin đổi timer/điểm per-contest (RuleConfig) và can thiệp giữa trận (cộng/trừ điểm tay, skip câu, thay câu dự phòng, pause/resume, undo); sound-cue events phát cho client; preload media câu kế tiếp (không kèm đáp án).
- Non-functional: buzzer xếp hạng theo server timestamp (đơn vị ms); state phục hồi được sau server restart; mọi biến động điểm truy vết được.

## Architecture

### Match state machine

```mermaid
stateDiagram-v2
  [*] --> LOBBY
  LOBBY --> KHOI_DONG: admin start (pre-flight OK)
  KHOI_DONG --> VCNV
  VCNV --> TANG_TOC
  TANG_TOC --> VE_DICH
  VE_DICH --> TIE_BREAK: hòa điểm cần phân định
  VE_DICH --> FINISHED
  TIE_BREAK --> FINISHED
  note right of KHOI_DONG
    Sub-machine: 4 lượt riêng
    (mỗi lượt 6 câu × 5s)
    rồi lượt chung 12 câu (chuông)
  end note
  note right of VE_DICH
    Sub-machine per thí sinh:
    chọn gói 3 câu (20/30)
    → hỏi → NSHV? → cướp?
  end note
```

Mỗi vòng là một module engine riêng implement interface chung:

```ts
interface RoundEngine {
  start(ctx: MatchContext): void;
  handleEvent(e: ContestantEvent | AdminEvent): EngineResult; // validate + emit
  snapshot(): RoundStateSnapshot;   // để state-sync & backup
  restore(s: RoundStateSnapshot): void;
}
```

### Event sourcing & điểm số

```mermaid
flowchart LR
  E[MatchEvent log - append only<br/>Postgres, id tăng dần<br/>buzz, answer, judge, adjust,<br/>skip, pause, undo] --> R[Reducer tính điểm]
  R --> S[ScoreState hiện tại<br/>cache Redis]
  E --> A[Audit/khiếu nại/replay]
```

- **Điểm = reduce(events)**: không có cột `score` sửa tay; "sửa điểm" = append `SCORE_ADJUST {delta, reason}`; "undo" = append `UNDO {targetEventId}` (reducer bỏ qua event bị undo). Immutable log = audit + replay + phân xử "em bấm trước" (giữ cả server-received ts lẫn socket packet ts).
- **Undo có domain semantics** (red-team H5): chỉ cho undo event chấm điểm GẦN NHẤT chưa có event khác build-upon (chưa sang câu/lượt mới); trường hợp khác dùng `SCORE_ADJUST` kèm lý do. Tương tác admin-can-thiệp × luật (NSHV đã chọn rồi câu bị thay → NSHV áp sang câu thay thế; câu bị skip khi đang mở quyền cướp → huỷ cửa sổ cướp, không ai mất điểm) định nghĩa trong `rules-2026.md` §7.
- **Thứ tự persist** (red-team H8): mọi event = append Postgres **synchronous** (1 insert) → apply vào state → broadcast. Không bao giờ broadcast trước khi persist — crash không được làm "tụt" điểm client đã thấy. Kill-test phải assert invariant này.
- **Single-writer per match** (red-team C1): mỗi match được **own bởi đúng 1 instance** qua Redis lease (`match:{id}:owner`, TTL renew); instance khác nhận socket event thì forward tới owner qua Redis pub/sub. Failover: lease hết hạn → instance khác giành lease, restore từ snapshot + replay event log, tự động PAUSE chờ admin resume. Socket.IO Redis adapter chỉ lo broadcast — KHÔNG lo xử lý event; đây là 2 tầng tách biệt.
- **Degraded mode khi mất Redis** (red-team C3): health-check Redis; mất kết nối → engine tự PAUSE trận + thông báo "tạm dừng kỹ thuật" trên mọi màn; Redis quay lại → khôi phục từ Postgres snapshot/event log rồi admin resume.
- **RuleConfig freeze** (red-team H9): RuleConfig snapshot vào match lúc start; sau start chỉ đổi được qua event `CONFIG_PATCH` (versioned) để reducer áp đúng config theo thời điểm — điểm luôn tái lập được từ log.
- **Concurrency control admin** (red-team H1): tại một thời điểm chỉ 1 host giữ **control lock** (người khác read-only, có nút "take control"); mọi AdminEvent kèm `expectedStateVersion`, lệch → reject để chặn double-click/2-người-cùng-bấm.
- **Auto-pause khi mất admin** (red-team H2): RuleConfig `autoPauseOnHostDisconnect: true` — admin channel trống quá N giây trong phần thi tự trôi (khởi động riêng) → engine tự PAUSE.
- **Buzzer**: mở cửa sổ chuông → client bấm → server ghi `BUZZ {seat, tsServer}` (Redis `INCR` thứ tự nguyên tử) → khoá chuông người thắng, thông báo tất cả. Sai ở lượt chung khởi động: `-5` theo RuleConfig.
- **Timer service**: server giữ deadline tuyệt đối (`endsAt`), tick broadcast 250ms `{remainingMs, serverNow}`; client interpolate để render mượt; hết giờ server tự chuyển state (không chờ client).
- **RuleConfig** (Zod, `packages/shared`): toàn bộ tham số trong `rules-2026.md` §6 — preset `O26_DEFAULT`, admin clone + sửa per contest; validate ràng buộc (vd mảng điểm VCNV giảm dần).
- **Chấm**: câu nhập text → normalize (bỏ dấu cách thừa, không phân biệt hoa thường, tuỳ chọn bỏ dấu tiếng Việt) so `acceptedAnswers` → kết quả tạm + admin confirm/override; câu miệng → admin bấm Đúng/Sai (DEFERED D4).
- **Media preload phân tầng theo audience** (red-team C2 — media CHÍNH LÀ đề ở Tăng tốc/VCNV, preload sớm cho thí sinh = rò đề qua DevTools): viewer/overlay được preload câu N+1 sớm; **thí sinh chỉ nhận URL media đúng lúc reveal** (chấp nhận phụ thuộc băng thông — LAN khi thi thật thì không vấn đề). Policy trong RuleConfig `preloadPolicy` (DEFERED D12). Media 404/lỗi → admin nhận cảnh báo + nút skip/thay câu dự phòng.
- **Submission không lộ chéo** (red-team H10): đáp án đang gõ/đã gửi của thí sinh chỉ tới admin channel realtime; thí sinh khác + viewer chỉ thấy sau `TIME_UP`/reveal — là invariant có test riêng.
- **Pause/resume**: PAUSE đóng băng deadline (lưu `remainingMs`), khoá input; RESUME đặt `endsAt` mới. **Backup**: snapshot state (Redis) ghi Postgres mỗi 10s + mỗi event quan trọng; server restart → restore từ snapshot + replay event sau đó.
- **Sound cues**: engine emit `sound-cue {type: buzz|correct|wrong|timeup|round-intro}` — client tự phát file (Phase 7-9).

## Related Code Files
- Create: `apps/api/src/engine/` (match-orchestrator, rounds/khoi-dong.engine.ts, vcnv.engine.ts, tang-toc.engine.ts, ve-dich.engine.ts, tie-break.engine.ts, timer.service, buzzer.service, scoring.reducer, answer-matcher, snapshot.service)
- Prisma: `MatchEvent`, `MatchSnapshot`
- Modify: `packages/shared` (RuleConfigSchema + preset O26_DEFAULT, toàn bộ socket event contracts, ScoreState)
- Test: `apps/api/test/engine/` — unit test reducer + từng round engine là TRỌNG TÂM test của cả dự án

## Milestones nội bộ (red-team M9: phase này = critical path, to bằng 3-4 phase khác)

- **6a** (mở khoá Phase 7/8 integrate sớm): MatchEvent + reducer + timer + buzzer + single-writer/lease + engine KHOI_DONG.
- **6b**: VCNV, TANG_TOC, VE_DICH, TIE_BREAK engines + orchestrator + preload + snapshot/restore + degraded mode.

## Implementation Steps
1. `RuleConfigSchema` + preset `O26_DEFAULT` từ `rules-2026.md` (các giá trị ⚠️ D8 là default có comment; kèm `buzzWindowSec`, `dropoutPolicy`, `autoPauseOnHostDisconnect`, `preloadPolicy`, `stealMode`, quy tắc làm tròn điểm lẻ — Zod ép số nguyên).
2. MatchEvent schema — **mọi event có `schemaVersion` ngay từ event đầu tiên** (reducer đọc được version cũ; thiếu cái này thì release sau làm event log trận cũ không replay được — gap 3.1) + scoring reducer thuần (pure function) + property-based tests (điểm không âm ngoài luật cho phép, undo idempotent...).
3. Timer service + buzzer service (Redis) + test công bằng: 50 buzz đồng thời xếp đúng thứ tự server-received. **Buzzer timestamp gắn tại instance OWNER của match** (không phải instance nhận socket — 2 máy lệch clock là bất công hệ thống, gap 5.1); deployment yêu cầu NTP/chrony, vào checklist ngày thi.
4. Round engines theo thứ tự: KHOI_DONG → VCNV → TANG_TOC → VE_DICH → TIE_BREAK, mỗi engine kèm bộ test kịch bản — BẮT BUỘC gồm các edge-case ở `rules-2026.md` §7: cả 4 bị loại khỏi VCNV, chuông CNV giữa timer hàng ngang, hòa timestamp Tăng tốc, tie-break 3-4 người/nhiều vị trí, thí sinh rớt mạng đúng lượt riêng, NSHV × skip/substitute, không ai bấm chuông hết `buzzWindowSec`.
5. Match orchestrator nối các engine + state **`INTERMISSION`** giữa các vòng (admin điều khiển; viewer/overlay hiện bảng điểm tổng + vòng kế tiếp + intro thí sinh đầu trận — 30-40% thời lượng buổi thi thật là "giữa các vòng", gap 4.2) + admin intervention (adjust/skip/substitute/pause/undo) + state-sync cho mọi client mới/reconnect. Chức năng **anonymize contestant** (thay displayName bằng mã trong dữ liệu trận cũ, không phá event log — privacy gap 1.1, retention config D14).
6. Media preload pipeline + sound-cue emitter.
7. Snapshot/restore + kill-test: giết process giữa vòng, restart, trận tiếp tục đúng.

## Success Criteria
- [ ] Chạy trọn 1 trận mô phỏng automated (script client giả 4 thí sinh) ra điểm đúng với bảng tính tay cho ≥3 kịch bản.
- [ ] Buzzer: 50 kết nối bấm trong cùng 100ms → thứ tự đúng theo server timestamp, không trùng hạng.
- [ ] Undo chấm sai → điểm tự tính lại đúng; log giữ nguyên cả 2 event.
- [ ] Kill server giữa VCNV → restart → trận resume đúng state, client tự sync.
- [ ] Admin đổi RuleConfig (vd tăng tốc 20/20/30/30) → engine chạy theo giá trị mới không sửa code.

## Risk Assessment
- Phức tạp nhất dự án — chia nhỏ theo round engine, mỗi engine PR riêng + test riêng.
- Reducer chậm khi log dài → snapshot điểm định kỳ, reduce từ snapshot gần nhất.
- Đồng hồ client lệch → mọi hiển thị đếm ngược dựa `serverNow` trong tick, không dùng `Date.now()` client trực tiếp.
