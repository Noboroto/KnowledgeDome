# Red-team review — findings & trạng thái xử lý

## VÒNG 2 (12/07) — review phần mở rộng RuleConfig v2

User chốt **không cắt scope (1 version đủ tính năng — DEFERED D18)** nên mọi finding được vá bằng thiết kế trong `ruleconfig-v2-spec.md` v2.1:

| # | Finding | Xử lý |
|---|---|---|
| C-v2-1 | "Chặn public khi gắn contest" hở 4 đường vòng (multi-match, thứ tự ngược, reference chung, un-public) | ✅ cờ `everPublic` một chiều trên Set + Question; pre-flight MỌI match hard-block ở đơn vị CÂU; chặn cả chiều gán |
| C-v2-2 | 6 ngữ nghĩa Teams × round chưa định nghĩa | ✅ spec §2b (teamLockout, teamSubmission, loại cả đội VCNV, NSHV 1/đội, CẤM same-team steal, tie-break đại diện); 🟡 D17 chờ user xác nhận 4 default |
| C-v2-3 | Không có mô hình quyền đáp án của Set → export = máy in đáp án | ✅ ACL 3 mức view/viewAnswer/export; share-link mặc định KHÔNG đáp án + audit token/IP |
| H-v2-1..10 | CONFIG_PATCH bypass preflight; draw pool cạn; rankPoints active; VCNV data model; cluePoints; share-link bruteforce; search-by-answer oracle; custom-build worst-case; theme/sound asset pipeline; visibility 2 tầng | ✅ tất cả vá trong spec v2.1 (§3, §4b, §1.4, §5, §6, §9, §7, §10) |
| M-v2-1..9, L-v2-1..3 | rep scope, lệch quân số, playlist bounds, inline bypass duyệt, soundboard permission, CSS injection màu, disclaimer nhạc, milestone 6a/6b, KD kind params, SFX fallback, demo reference, rules-2026 §6 pointer | ✅ vá trong spec + phase files + rules-2026 deprecation note; demo đang rework theo UX rules mới |
| 14 mâu thuẫn tài liệu | "4 ghế/5 phần thi/5 miếng..." rải rác | ✅ dọn toàn bộ (phase-03/05/06/07/08/09/10, plan.md, PRD, user-stories, DEFERED D15) |
| Đề xuất cắt scope | Teams/clue-buzz/public-set/soundboard... → v1.1 | ❌ USER TỪ CHỐI — ghi nhận tại D18, hệ quả: Phase 6 dài hơn, milestone chia lại |

## VÒNG 1 — findings & trạng thái xử lý

> Reviewer thù địch chạy 12/07/2026 trên toàn bộ plan. Dưới đây là findings và cách plan đã được sửa (✅ = đã sửa vào plan, 🟡 = ghi DEFERED chờ user).

## CRITICAL

| # | Finding | Xử lý |
|---|---|---|
| C1 | Engine stateful in-process nhưng plan hứa HA 2-instance — không có match ownership. Race khi 2 instance cùng mutate state | ✅ phase-06: thêm kiến trúc **single-writer per match** (Redis lease + forward event tới owner, failover = lease hết hạn → instance khác restore); phase-10 SC sửa theo |
| C2 | Preload media câu N+1 cho THÍ SINH = rò đề (media chính là câu hỏi ở Tăng tốc/VCNV; DevTools xem trước được) | ✅ phase-06: preload phân tầng theo audience (viewer/overlay sớm, thí sinh chỉ lúc reveal); 🟡 DEFERED D12 |
| C3 | Redis là SPOF giết trận live — không có persistence/degraded mode/kill-test | ✅ phase-06 degraded mode auto-pause khi mất Redis; phase-10: Redis AOF everysec + kill-Redis test + runbook entry |

## HIGH

| # | Finding | Xử lý |
|---|---|---|
| H1 | 2 admin cùng điều khiển → double event, không có concurrency control | ✅ phase-06/08: control lock (1 host active) + `expectedStateVersion` trên AdminEvent |
| H2 | Admin rớt mạng — engine tự trôi, không ai chấm | ✅ RuleConfig `autoPauseOnHostDisconnect` (phase-06) |
| H3 | Luật VCNV thiếu: cả 4 bị loại, người bị loại có được trả lời hàng ngang?, chuông giữa timer hàng ngang | ✅ rules-2026.md thêm mục edge-cases; 🟡 DEFERED D13 |
| H4 | Tie-break chỉ viết cho 2 người hòa; hòa 3-4, hòa nhiều vị trí chưa định nghĩa | ✅ rules-2026.md edge-cases (function per nhóm hòa); 🟡 D13 |
| H5 | NSHV/cướp tương tác với skip/substitute/undo chưa định nghĩa; undo cần domain semantics | ✅ phase-06: giới hạn undo = event chấm gần nhất chưa build-upon, còn lại SCORE_ADJUST; rules edge-cases |
| H6 | Overlay token JWT dài hạn trong URL → lộ qua screenshot/scene collection | ✅ phase-05: token bootstrap ngắn hạn đổi lấy ticket + revoke/re-issue |
| H7 | Spike Phase 1 thiếu Better-auth trên Fastify | ✅ phase-01 step 4 mở rộng |
| H8 | Thứ tự persist event vs apply/broadcast chưa định nghĩa → crash mất event đã công bố | ✅ phase-06: append PG synchronous → apply → broadcast; kill-test assert |
| H9 | RuleConfig sửa được giữa trận LIVE → điểm không tái lập | ✅ phase-05/06: snapshot RuleConfig lúc start, sau đó chỉ qua event `CONFIG_PATCH` versioned |
| H10 | Submission của thí sinh khác có thể lộ trước TIME_UP | ✅ phase-06 invariant + phase-10 test payload |

## MEDIUM (tóm tắt xử lý)

- M1 rớt mạng đúng lượt riêng → rules edge-cases + 🟡 D13. M2 `buzzWindowSec` thêm vào rule-spec/RuleConfig. M3 tie timestamp Tăng tốc → rule-spec + 🟡 D13.
- M4 zip-slip + magic-bytes sniffing + cấm SVG → phase-03/04. M5 viewer rate-limit + khoá cổng lên P1 phase-05. M6 thêm model `Match` (Contest 1-n) → phase-05/06. M7 seed fixture bank → phase-03. M8 restore drill SC → phase-10. M9 phase-06 chia milestone 6a/6b. M10 task SFX royalty-free → phase-09.

## LOW (tóm tắt)

- L1 questions.html có trong demo ✅ (đã verify Playwright). L2 ghi chú sync demo theo D8 → plan.md. L3 ping config per namespace → phase-05. L4 session thí sinh 24h → phase-02. L5 sharded adapter cần spike → phase-01. L6 rounding điểm lẻ → rules §6 + Zod. L7 eslint no-literal-string → phase-01.
