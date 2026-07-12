---
phase: 12
title: "Teams — thi đội (release v2)"
status: pending
priority: P1
dependencies: [10]
---

# Phase 12: Teams (thi đội) — release v2

## Overview
Kích hoạt chế độ thi ĐỘI (user chốt D1: bấm chuông/trả lời CÁ NHÂN, điểm về ĐỘI — kiểu Pop Culture Jeopardy). SCHEMA đã có sẵn từ v1 (`Team`, `seat.teamId nullable`, `scoringUnit`, reducer team-aware — v1 mỗi seat là trường hợp suy biến team size 1); phase này bật semantics đội trong engine theo **spec §2b**, UI đội mọi màn, preset `TEAM_12@1`.

**✅ D17 ĐÃ CHỐT TOÀN BỘ (12/07)** — không còn blocker: D17.1 khoá CÁ NHÂN khi bấm sai (`teamLockout: false` default, option true); D17.2 last-wins; D17.3 VCNV sai CNV loại CẢ ĐỘI; D17.4 NSHV 1/đội.

## Requirements
- Functional (semantics §2b — engine áp theo `scoringUnit: team`):
  - **Buzz/khoá**: thành viên bấm sai ở vòng chuông (khởi động chung, clue-buzz, cướp về đích) → **khoá CÁ NHÂN, thành viên khác vẫn bấm được** (`teamLockout: false` default — ✅ D17.1 12/07; option `true` cho giải công bằng quân số nghiêm ngặt); vì khoá cá nhân nên CẤM same-team-steal càng bắt buộc.
  - **Submission đội (VCNV/Tăng tốc)**: mọi thành viên gõ được; tính **bản CUỐI của bất kỳ thành viên** trước server-timeout (✅ D17.2 last-wins); ranking theo server-received ts của bản cuối.
  - **VCNV sai CNV**: loại CẢ ĐỘI (D17.3 default) — tránh đội 4 người có 4 lần đoán.
  - **NSHV**: 1 lần/ĐỘI/trận (D17.4 default).
  - **Về đích**: CẤM cùng đội cướp điểm của nhau (hard-code — exploit, không config); lượt cá nhân khi có đội theo `individualTurnMode: all-members | representative` (config per-contest, D1).
  - **Tie-break**: đội cử 1 người bấm (hard-code).
  - Điểm reduce về đội; lệch quân số giữa các đội đã ghi nhận ở red-team M-v2 (cân bằng là trách nhiệm BTC, hệ thống chỉ cảnh báo pre-flight).
- UI:
  - **Contest builder** (phase-08 mở rộng): setup teams kéo-thả thành viên vào đội, đặt tên/màu đội, chọn scoringUnit + individualTurnMode + teamLockout.
  - **Contestant**: hiển thị tên đội + điểm đội; trạng thái khoá đội; thành viên nào vừa submit.
  - **Viewer/Overlay/MC**: adaptive layout theo ĐỘI (điểm đội to, thành viên nhỏ — thiết kế đã chốt trong phase-09); scoreboard/podium theo đội.
  - **Pre-flight**: teams hợp lệ (đủ thành viên, không ghế mồ côi), cảnh báo lệch quân số.
- Preset & test: `TEAM_12@1` (12 người/4 đội) thành preset chính thức; test matrix + UAT bổ sung kịch bản đội.

## Related Code Files
- Modify: `apps/api/src/engine/` (mọi round engine + reducer: nhánh scoringUnit=team theo §2b), `apps/web/src/pages/contestant|viewer|overlay|mc` (team display), `apps/web/src/pages/admin` (contest builder teams, judging theo đội), `packages/shared` (kích hoạt validate teams trong RuleConfigSchema — schema đã có từ v1)
- Test: `apps/api/test/engine/teams/` — kịch bản §2b từng vòng; same-team-steal bị reject; NSHV/đội; teamLockout cả 2 giá trị

## Implementation Steps
1. Reducer + round engines: nhánh team cho từng vòng theo §2b đã chốt (khởi động chung/riêng theo individualTurnMode + khoá cá nhân, VCNV loại cả đội, tăng tốc last-wins any-member, về đích + cấm same-team steal, NSHV 1/đội, tie-break đại diện).
3. Contest builder: teams setup UI + pre-flight teams.
4. Team display: contestant → viewer/overlay → MC (adaptive layout đã thiết kế sẵn phase-09).
5. Preset `TEAM_12@1` + bộ test kịch bản đội đầy đủ (bao gồm edge: thành viên rớt mạng giữa lượt cá nhân của đội, đội bị khoá toàn bộ, lệch quân số 1v4).
6. Load test: 12 thiết bị thí sinh + buzzer storm theo đội (bổ sung kịch bản phase-10).
7. UAT trận đội thật end-to-end.

## Success Criteria
- [ ] Kịch bản automated `TEAM_12@1` (12 người/4 đội) ra điểm đúng bảng tính tay ≥1 kịch bản đủ 4 vòng.
- [ ] Same-team steal bị server reject (test); NSHV dùng 2 lần/đội bị reject.
- [ ] teamLockout=false (default): 1 thành viên sai → chỉ cá nhân bị khoá, đồng đội vẫn bấm được; =true: cả đội bị khoá câu đó.
- [ ] Tăng tốc đội: bản cuối của BẤT KỲ thành viên được tính; ranking đúng theo server-ts.
- [ ] Viewer/overlay hiển thị đúng theo đội ở cả 4 vòng + podium; 60fps giữ nguyên.

## Risk Assessment
- Test matrix phình (đây là lý do teams được tách khỏi v1) — giữ nguyên tắc preset-driven: engine hỗ trợ mọi config Zod-hợp-lệ, test/UAT chạy trên preset chuẩn.
- Đội đông lợi thế cấu trúc nếu BTC config sai (teamLockout=false + đội lệch quân số) — pre-flight cảnh báo + docs guide-admin.
