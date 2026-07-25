# Glossary — Thuật ngữ nghiệp vụ Olympia Contest System

> **Ngày lập**: 2026-07-25
> **Nguồn rà soát**: `docs/source/fandom-olympia-26-luat-choi.md` (F26, nguyên văn luật gốc) · `docs/game-rules-inventory.md` · `docs/reviews/game-rules-review.md` (GRR-137→GRR-171) · `docs/reviews/game-rules-decisions.md` (Đ-1→Đ-15, tên chuẩn đã chốt)
>
> **Nguyên tắc**: KHÔNG tự sáng tạo định nghĩa. Mọi định nghĩa dẫn file + heading nguồn. Từ nào tài liệu chưa nói đủ ⇒ `NEEDS CLARIFICATION`; từ nào được dùng với nhiều nghĩa ⇒ `CONFLICT`. Từ nào **không tồn tại** trong miền này ⇒ ghi ở [Phụ lục A](#phụ-lục-a--thuật-ngữ-được-yêu-cầu-nhưng-không-tồn-tại-trong-miền), KHÔNG bịa định nghĩa để lấp chỗ.

## Trạng thái — thống kê

| Status | Số mục | Nghĩa |
|---|---|---|
| `CONFIRMED` | 28 | Tài liệu định nghĩa đủ và nhất quán |
| `NEEDS CLARIFICATION` | 22 | Từ có được dùng nhưng định nghĩa chưa đủ để viết spec |
| `CONFLICT` | 10 | Cùng một từ mang **nhiều nghĩa khác nhau** trong tài liệu |

**10 mục `CONFLICT` — đọc trước khi viết bất kỳ spec nào**: TERM-006 `host` · TERM-013 `game` · TERM-016 `lượt / turn` · TERM-017 `phase` · TERM-022 `state` · TERM-027 `event điểm` · TERM-038 `cancelled` · TERM-044 `result` · TERM-048 `draw` · TERM-055 `visibility / everPublic`.

> **Tỉ lệ `CONFIRMED` 28/60 không phải lỗi kiểm kê** — phần lớn mục `NEEDS CLARIFICATION` là thuật ngữ **có định nghĩa cốt lõi rõ ràng** nhưng còn một nhánh chưa chốt (thường là một mã U-x hoặc GR-x đã ghi nhận). Trường Definition của các mục đó vẫn dùng được; chỉ nhánh nêu ở Status là chưa.

---

# A. CON NGƯỜI VÀ VAI TRÒ

## TERM-001 — Thí sinh

- **Definition**: người dự thi, ngồi một ghế, tự trả lời và tự phát tín hiệu; *"thí sinh cần tốc độ, và tự chịu trách nhiệm sai lầm của mình"* — phía thí sinh không có dialog xác nhận, không rút lại được.
- **Alternative names**: player, contestant, TS (viết tắt trong inventory).
- **Actor / entity liên quan**: Ghế (TERM-002), Đơn vị điểm (TERM-003), Đội (TERM-010).
- **Allowed values**: v1 **LUẬT đặc tả cho ĐÚNG 4**; **lưu trữ + UI hỗ trợ 1-12**.
- **Unit**: người.
- **Terms dễ nhầm**: **Ghế** (chỗ ngồi, tồn tại cả khi trống) · **Đơn vị điểm** (v2: 1 đội = 1 đơn vị điểm nhưng nhiều thí sinh) · **User** (TERM-009).
- **Related rules**: R-KD-01, R-VCNV-03, R-VD-02, K-11.
- **Source**: `game-rules-inventory.md` §PHẦN 8B K-11 · `game-rules-decisions.md` §1.4, §2.
- **Status**: `CONFIRMED`

## TERM-002 — Ghế · Vị trí

- **Definition**: **Ghế** = chỗ dự thi trong một trận. **Vị trí** = số thứ tự 1→N gán cho ghế, **gán thủ công trước trận** (thực tế chương trình: BTC bốc thăm thứ tự xuất phát 1→4).
- **Alternative names**: seat · seat position · "vị trí đứng" (nguyên văn F26) · "số thứ tự".
- **Actor / entity liên quan**: Thí sinh (TERM-001), Contest settings.
- **Allowed values**: 1-12 (lưu trữ); vị trí là số nguyên dương liên tục.
- **Unit**: —
- **Terms dễ nhầm**: **Thứ hạng** (xếp theo điểm, đổi liên tục trong trận) — F26 dùng cụm *"vị trí đứng thấp nhất"* mà không nói là ghế hay hạng (U-27).
- **Related rules**: R-VCNV-03, R-VD-02, U-27.
- **Source**: `docs/source/fandom-olympia-26-luat-choi.md` §Về đích · `game-rules-inventory.md` §R-VD-02 (U-27) · `game-rules-decisions.md` §6.2 `[Đ-4.6]`.
- **Status**: `NEEDS CLARIFICATION` — U-27 chưa chốt "vị trí đứng" là ghế vật lý hay thứ hạng.

## TERM-003 — Đơn vị điểm

- **Definition**: chủ thể được cộng/trừ điểm. v1: **1 đơn vị điểm = 1 thí sinh**. v2 thi đội: **1 đơn vị điểm = 1 đội**, buzz vẫn theo cá nhân nhưng điểm về đội.
- **Alternative names**: `scoringUnit`.
- **Actor / entity liên quan**: Thí sinh, Đội, mọi rule tính điểm.
- **Allowed values**: `individual` · `team`.
- **Unit**: —
- **Terms dễ nhầm**: **Thí sinh** — hai khái niệm trùng nhau ở v1 và **tách nhau ở v2**; mọi mảng RuleConfig (thang điểm Tăng tốc) đánh theo **đơn vị điểm**, không theo người.
- **Related rules**: R-TT-01, R-TEAM-04, `SPEC` §1.4.
- **Source**: `game-rules-inventory.md` §R-TT-01, §PHẦN 7 · `CLAUDE.md` §Lộ trình version.
- **Status**: `CONFIRMED`

## TERM-004 — Admin

- **Definition**: **cảm biến + cơ cấu chấp hành DUY NHẤT của hệ thống**. Máy không quan sát được sân khấu ⇒ mọi mốc thời gian mà luật gốc mô tả bằng hành vi của MC đều ánh xạ thành **một thao tác bấm của admin**. Admin bấm Đúng/Sai, start timer, mở đáp án/ô chữ, chọn vòng và lượt, duyệt tín hiệu trong hàng đợi.
- **Alternative names**: người vận hành · operator · (trong tên config: `host` — xem TERM-006).
- **Actor / entity liên quan**: MC (TERM-005), Server, Hàng đợi tín hiệu (TERM-032).
- **Allowed values**: —
- **Unit**: —
- **Terms dễ nhầm**: **MC** — MC phán quyết bằng **lời**, admin thi hành bằng **bấm**; màn `/mc` là READ-ONLY.
- **Related rules**: R-GEN-03, Đ-1, Đ-5, Đ-6.
- **Source**: `game-rules-decisions.md` §1.1, §8.1 · `game-rules-inventory.md` §R-GEN-03.
- **Status**: `CONFIRMED` — **mỗi contest chỉ có MỘT admin duy nhất** (chốt 2026-07-25), nên "DUY NHẤT" đúng cả về vai trò lẫn số người đang điều khiển; không tồn tại hai luồng thao tác admin đồng thời.

## TERM-005 — MC

- **Definition**: người dẫn chương trình — **thẩm quyền phán quyết trên sân khấu**, thực hiện bằng **nói**. Không thao tác hệ thống.
- **Alternative names**: người dẫn chương trình (nguyên văn F26) · người dẫn.
- **Actor / entity liên quan**: Admin (thi hành), Thí sinh.
- **Allowed values**: —
- **Unit**: —
- **Terms dễ nhầm**: **Host** (TERM-006) — trong luật gốc "host" nghĩa là MC, trong tên config của repo lại là admin.
- **Related rules**: Đ-6, Đ-6.4a, R-TB-03.
- **Source**: `docs/source/fandom-olympia-26-luat-choi.md` §Khởi động, §Câu hỏi phụ · `game-rules-decisions.md` §1.1.
- **Status**: `CONFIRMED`

## TERM-006 — Host `⚠ CONFLICT`

- **Definition**: từ này mang **hai nghĩa loại trừ nhau** trong tài liệu:
  - **(H1) Người dẫn chương trình = MC** — nghĩa của luật gốc (*"hiệu lệnh của người dẫn chương trình"*).
  - **(H2) Admin** — nghĩa trong tên config `autoPauseOnHostDisconnect`: "host disconnect" ở đây là **admin mất kết nối**, không phải MC.
- **Alternative names**: (H1) MC, người dẫn chương trình · (H2) admin, người điều khiển trận.
- **Actor / entity liên quan**: MC, Admin, R-GEN-08.
- **Allowed values**: —
- **Unit**: —
- **Terms dễ nhầm**: chính hai nghĩa của nó. MC **không có quyền ghi**; admin là bên duy nhất bấm được ⇒ hiểu nhầm H1↔H2 sẽ gán quyền sai cho `/mc`.
- **Related rules**: R-GEN-08, Đ-6.1, GRR-165.
- **Source**: `docs/source/fandom-olympia-26-luat-choi.md` §Câu hỏi phụ (H1) · `game-rules-inventory.md` §R-GEN-08 (H2).
- **Status**: `CONFLICT`

## TERM-007 — Setter

- **Definition**: người soạn câu hỏi và bộ đề trong kho đề.
- **Alternative names**: người soạn đề.
- **Actor / entity liên quan**: Câu hỏi (TERM-051), Kho đề (TERM-053).
- **Allowed values**: —
- **Unit**: —
- **Terms dễ nhầm**: **Admin** — setter không điều khiển trận; đề của setter phải qua duyệt DRAFT→ACTIVE.
- **Related rules**: R-KD-05, R-GEN-04, R-VCNV-08.
- **Source**: `game-rules-inventory.md` §R-KD-05, §R-VCNV-08 · `CLAUDE.md` §UX (Dashboard).
- **Status**: `CONFIRMED`

## TERM-008 — Viewer

- **Definition**: người xem, truy cập **public chỉ bằng mã phòng 6 số** (không account, không duyệt); **read-only tuyệt đối** — server drop mọi event ghi từ namespace này. Không được thấy đáp án trừ khi `revealAnswerAfterJudge` bật.
- **Alternative names**: khán giả · người xem. Kênh liên quan: **overlay OBS**.
- **Actor / entity liên quan**: Server, `revealAnswerAfterJudge`.
- **Allowed values**: —
- **Unit**: —
- **Terms dễ nhầm**: **Overlay** — cùng mô hình truy cập nhưng là frame stream cho OBS, không phải màn người xem.
- **Related rules**: R-GEN-10, Đ-5.2e.
- **Source**: `game-rules-inventory.md` §R-GEN-10 · `CLAUDE.md` §Mô hình truy cập.
- **Status**: `CONFIRMED`

## TERM-009 — User

- **Definition**: **chưa có định nghĩa nghiệp vụ**. Tài liệu chỉ liệt kê các **vai trò** cần auth (thí sinh, MC, admin/setter; v1.5 thêm trainer) và nhóm public không cần auth (viewer, overlay). Không có phát biểu nào định nghĩa "user" như một entity, cũng không nói một người có thể giữ nhiều vai trò trong cùng một trận hay không.
- **Alternative names**: tài khoản · account.
- **Actor / entity liên quan**: mọi vai trò trên.
- **Allowed values**: chưa liệt kê được tập vai trò đóng.
- **Unit**: —
- **Terms dễ nhầm**: **Thí sinh** (vai trò trong trận) · **Đơn vị điểm** (chủ thể tính điểm).
- **Related rules**: R-GEN-10.
- **Source**: `CLAUDE.md` §Mô hình truy cập · `game-rules-inventory.md` §R-GEN-10.
- **Status**: `NEEDS CLARIFICATION`

## TERM-010 — Đội `[v2]`

- **Definition**: nhóm thí sinh chia sẻ **một đơn vị điểm**; buzz theo cá nhân, điểm về đội.
- **Alternative names**: team.
- **Actor / entity liên quan**: Đơn vị điểm (TERM-003), Đội trưởng.
- **Allowed values**: —
- **Unit**: —
- **Terms dễ nhầm**: **Ghế** — v2 nhiều ghế thuộc một đội.
- **Related rules**: R-TEAM-01 → R-TEAM-07, GRR-170, GRR-171.
- **Source**: `game-rules-inventory.md` §PHẦN 7.
- **Status**: `CONFIRMED` (luật v2 chưa hiện thực; hai điểm giao còn mở — GRR-170, GRR-171)

---

# B. ĐƠN VỊ TỔ CHỨC THI

## TERM-011 — Contest

- **Definition**: đơn vị tổ chức **bao trùm**, gắn với một kho đề đã gán; phạm vi của quy tắc **no-repeat**: câu đã hỏi trong bất kỳ match nào của contest thì không xuất hiện lại.
- **Alternative names**: cuộc thi.
- **Actor / entity liên quan**: Match (TERM-012), Kho đề (TERM-053), RuleConfig (TERM-056).
- **Allowed values**: —
- **Unit**: —
- **Terms dễ nhầm**: **Match** — no-repeat ở cấp **contest**; `revealAnswerAfterJudge` ở cấp **match** (K-4); mode trả lời ở cấp **contest** (Đ-4.a2).
- **Related rules**: R-GEN-06, K-4, Đ-4.a2.
- **Source**: `game-rules-inventory.md` §R-GEN-06, §8A K-4.
- **Status**: `CONFIRMED`

## TERM-012 — Match · Trận

- **Definition**: một lần thi đấu hoàn chỉnh, chạy theo playlist các vòng, kết thúc bằng một kết quả. Là đơn vị mang `matchPurpose` và `revealAnswerAfterJudge`.
- **Alternative names**: trận · trận đấu.
- **Actor / entity liên quan**: Contest, Vòng (TERM-015), Trạng thái trận (TERM-021).
- **Allowed values**: —
- **Unit**: —
- **Terms dễ nhầm**: **Contest** (TERM-011) · **Game** (TERM-013).
- **Related rules**: K-4, R-GEN-12, Đ-5.
- **Source**: `game-rules-inventory.md` §8A K-4, §R-GEN-12.
- **Status**: `CONFIRMED`

## TERM-013 — Game `⚠ CONFLICT`

- **Definition**: dùng với **hai nghĩa**:
  - **(G1) = Match** — heading *"Trạng thái game dùng trong tài liệu này"* rồi liệt kê `LOBBY / rounds[i] / … / FINISHED`, tức trạng thái của **một trận**.
  - **(G2) = sản phẩm / thể loại** — *"gameshow kiến thức tuỳ biến"*, *"luật chơi (game rules)"*, *"game engine"*: không phải một thực thể chạy được.
- **Alternative names**: (G1) trận, match · (G2) gameshow, luật chơi.
- **Actor / entity liên quan**: Match, RuleConfig.
- **Allowed values**: —
- **Unit**: —
- **Terms dễ nhầm**: mọi trường/biến đặt tên `game*` phải nói rõ thuộc nghĩa nào; `gameState` (G1) khác `gameRules` (G2) về vòng đời.
- **Related rules**: §Trạng thái game, R-GEN-*.
- **Source**: `game-rules-inventory.md` §Trạng thái game (G1) · `CLAUDE.md` §đầu, §Nguyên tắc code (G2).
- **Status**: `CONFLICT`

## TERM-014 — Session

- **Definition**: **không có định nghĩa** trong tài liệu luật. Khái niệm gần nhất được dùng nhưng chưa đặt tên là **phiên kết nối của thí sinh** (R-GEN-09 reconnect grace 120s giữ ghế). Phía admin thì câu hỏi về số phiên đã khép: **mỗi contest chỉ có một admin duy nhất**.
- **Alternative names**: phiên · phiên kết nối.
- **Actor / entity liên quan**: Admin, Thí sinh, R-GEN-09.
- **Allowed values**: —
- **Unit**: —
- **Terms dễ nhầm**: **Match** — một trận có thể trải nhiều phiên kết nối; một phiên không tương ứng một trận.
- **Related rules**: R-GEN-09.
- **Source**: `game-rules-inventory.md` §R-GEN-09.
- **Status**: `NEEDS CLARIFICATION`

## TERM-015 — Vòng · Round

- **Definition**: một phần thi của trận, có luật riêng. Năm vòng của O26: **Khởi động · Vượt chướng ngại vật · Tăng tốc · Về đích · Câu hỏi phụ**. Admin chọn vòng nào bắt đầu; được **bỏ hẳn** hoặc **chạy lại** một vòng.
- **Alternative names**: round · `rounds[i]` · phần thi (nguyên văn F26).
- **Actor / entity liên quan**: Match, Lượt (TERM-016), Playlist (TERM-018).
- **Allowed values**: `KHOI_DONG` · `VCNV` · `TANG_TOC` · `VE_DICH` · `TIE_BREAK` (chuỗi demo).
- **Unit**: —
- **Terms dễ nhầm**: **Lượt** (TERM-016) — F26 gọi *"lượt riêng / lượt chung"* là hai phần **bên trong** vòng Khởi động, không phải hai vòng.
- **Related rules**: Đ-5, Đ-5.1, GRR-164.
- **Source**: `docs/source/fandom-olympia-26-luat-choi.md` (5 heading vòng) · `game-rules-inventory.md` §Trạng thái game.
- **Status**: `CONFIRMED`

## TERM-016 — Lượt · Turn `⚠ CONFLICT`

- **Definition**: dùng với **bốn nghĩa khác nhau**:
  - **(T1) Phân đoạn của vòng Khởi động** — *"lượt riêng"* (mỗi TS 6 câu) và *"lượt chung"* (12 câu bấm chuông). Không phải lượt của một người.
  - **(T2) Lượt của một thí sinh trong lượt riêng** — mỗi TS lần lượt trả lời 6 câu của mình.
  - **(T3) Lượt lựa chọn ở VCNV** — *"mỗi thí sinh có tối đa 1 lượt lựa chọn"* hàng ngang.
  - **(T4) Lượt thi Về đích** — mỗi TS một gói 3 câu, thứ tự tính lại sau mỗi lượt.
- **Alternative names**: turn · `Turn` (entity trong SPEC, mang `drawConfig`, `kind`) · lượt thi · lượt chọn.
- **Actor / entity liên quan**: Vòng, Thí sinh, `Turn.drawConfig`.
- **Allowed values**: `kind`: `individual-count` · `individual-timed` · `common-count`.
- **Unit**: —
- **Terms dễ nhầm**: **Vòng** (TERM-015) · **Pha** (TERM-017). Ràng buộc *"tối đa 1 lượt"* của T3 và *"thứ tự lượt"* của T4 là hai luật khác nhau, không suy ra được nhau.
- **Related rules**: R-KD-01, R-KD-03, R-VCNV-03, R-VD-02, Đ-4.5.
- **Source**: `docs/source/fandom-olympia-26-luat-choi.md` §Khởi động, §VCNV, §Về đích · `game-rules-inventory.md` §R-KD-01, §R-VCNV-03 · `game-rules-decisions.md` §6.2.
- **Status**: `CONFLICT`

## TERM-017 — Phase · Pha `⚠ CONFLICT`

- **Definition**: dùng với **hai nghĩa ở hai thang bậc hoàn toàn khác nhau**:
  - **(P1) Bước trong luồng một câu hỏi** — luồng 8 phase của Câu hỏi phụ (admin chọn người → hiển thị câu → start timer → … → tính điểm).
  - **(P2) Giai đoạn phát triển sản phẩm** — "Phase 1-10" là v1, "Phase 11" là v1.5, "Phase 12" là v2; `phase-06-game-engine.md` là **file kế hoạch**.
- **Alternative names**: (P1) pha, bước · (P2) mốc phát hành, milestone.
- **Actor / entity liên quan**: (P1) Câu hỏi phụ, Admin · (P2) lộ trình version.
- **Allowed values**: (P1) 1→8 cho Câu hỏi phụ · (P2) 1→12.
- **Unit**: —
- **Terms dễ nhầm**: cả hai đều đánh số bắt đầu từ 1 ⇒ "phase 2" là hai thứ khác nhau tuỳ ngữ cảnh.
- **Related rules**: Đ-9, GRR-165.
- **Source**: `game-rules-decisions.md` §9.5 `[Đ-9]` (P1) · `CLAUDE.md` §Lộ trình version (P2).
- **Status**: `CONFLICT`

## TERM-018 — Playlist

- **Definition**: danh sách vòng theo thứ tự của một trận. `TieBreakConfig` **chỉ hợp lệ ở cuối playlist**.
- **Alternative names**: danh sách vòng.
- **Actor / entity liên quan**: Match, Vòng, TIE_BREAK.
- **Allowed values**: —
- **Unit**: —
- **Terms dễ nhầm**: **Bộ đề** — playlist xếp vòng, bộ đề gom câu hỏi.
- **Related rules**: R-TB-01, GRR-155.
- **Source**: `game-rules-inventory.md` §R-TB-01.
- **Status**: `CONFIRMED`

## TERM-019 — matchPurpose

- **Definition**: mục đích của trận, quyết định **chính sách chấm điểm** và mặc định bảo mật đáp án. `official`: **không tồn tại `autoJudge`** dưới bất kỳ hình thức nào, điểm chỉ chốt khi admin bấm. `practice`: `autoJudge` tồn tại vì practice solo chạy không có admin.
- **Alternative names**: mục đích trận · official / practice.
- **Actor / entity liên quan**: Match, Admin, `revealAnswerAfterJudge`.
- **Allowed values**: `official` · `practice`.
- **Unit**: —
- **Terms dễ nhầm**: **`revealAnswerAfterJudge`** — là cờ riêng, chỉ **mặc định** theo `matchPurpose` (official TẮT, practice BẬT).
- **Related rules**: R-GEN-03, R-GEN-10, K-16.
- **Source**: `game-rules-inventory.md` §R-GEN-03, §8A K-16.
- **Status**: `CONFIRMED`

## TERM-020 — Mode trả lời

- **Definition**: cách thí sinh đưa đáp án. **Sân khấu** (MẶC ĐỊNH, và là mode **luật được đặc tả theo**): thí sinh **đọc** đáp án, máy chỉ dùng để giành quyền. **Nhập liệu**: thí sinh **gõ** đáp án. Đặt ở **cấp contest**, một giá trị chung cho toàn bộ vòng.
- **Alternative names**: mode sân khấu / mode nhập liệu · stage mode / input mode.
- **Actor / entity liên quan**: Contest, Thí sinh, mọi vòng.
- **Allowed values**: `sân khấu` · `nhập liệu`.
- **Unit**: —
- **Terms dễ nhầm**: **VCNV và Tăng tốc LUÔN gõ máy bất kể mode** — ở VCNV mode chỉ đổi cách **chọn hàng ngang**; đáp án Chướng ngại vật thì đi theo mode.
- **Related rules**: Đ-4, Đ-4.1, Đ-4.b, Đ-4.X2.
- **Source**: `game-rules-decisions.md` §4 · `CLAUDE.md` §Hai mode trả lời.
- **Status**: `CONFIRMED`

---

# C. TRẠNG THÁI VÀ SỰ KIỆN

## TERM-021 — Trạng thái trận

- **Definition**: máy trạng thái của một trận: `LOBBY` · `rounds[i]` · `INTERMISSION` · `TIE_BREAK` · `FINISHED`. **Không có `PAUSED`** — xem TERM-023.
- **Alternative names**: match state · trạng thái game (nghĩa G1 của TERM-013).
- **Actor / entity liên quan**: Match, Admin, Server.
- **Allowed values**: 5 giá trị trên; demo dùng chuỗi phẳng `CHO / KHOI_DONG_RIENG / KHOI_DONG_CHUNG / VCNV / TANG_TOC / VE_DICH / TIE_BREAK / KET_THUC`.
- **Unit**: —
- **Terms dễ nhầm**: **State** (TERM-022) — cùng từ, dùng cả cho trạng thái **cấp ghế**.
- **Related rules**: §Trạng thái game, GRR-164.
- **Source**: `game-rules-inventory.md` §Trạng thái game.
- **Status**: `NEEDS CLARIFICATION` — còn một giá trị chưa đủ điều kiện vào/ra: `INTERMISSION` (GRR-077).

## TERM-022 — State `⚠ CONFLICT`

- **Definition**: dùng cho **hai thang bậc**:
  - **(S1) Trạng thái cấp TRẬN** — enum 5 giá trị của TERM-021, tại một thời điểm chỉ một giá trị.
  - **(S2) Trạng thái cấp GHẾ / cấp ván** — các cờ song song, nhiều cái cùng đúng: "đã bị loại khỏi VCNV", "NSHV đã tiêu", "miếng ghép đã mở", "gói câu đã chọn", "lượt chọn đã dùng", "đang bị cấm trả lời".
- **Alternative names**: (S1) match state · (S2) trạng thái phi-điểm.
- **Actor / entity liên quan**: Match, Ghế, Revert (TERM-029).
- **Allowed values**: (S1) 5 giá trị · (S2) chưa liệt kê đầy đủ ở đâu.
- **Unit**: —
- **Terms dễ nhầm**: nhầm lẫn cũ giữa hai thang bậc từng thể hiện rõ nhất ở `PAUSED` — giá trị này nay đã bị bỏ khỏi enum (TERM-023).
- **Related rules**: §Trạng thái game, Đ-5.3.
- **Source**: `game-rules-inventory.md` §Trạng thái game (S1) · `game-rules-decisions.md` §7.3 (S2).
- **Status**: `CONFLICT`

## TERM-023 — Tạm dừng trận `[ĐÃ BỎ]`

- **Definition**: **không tồn tại trong hệ thống.** Mọi thao tác của trận đều do admin thực hiện, nên khi trận cần dừng thì **admin ngừng thao tác**; hệ thống không có trạng thái nào ghi nhận việc đó. Không có đóng băng đồng hồ, không có tự động tạm dừng, không có giá trị `PAUSED` trong máy trạng thái.
- **Alternative names**: PAUSED · auto-pause · tạm dừng kỹ thuật — **đều là khái niệm đã bỏ**, giữ lại đây để không ai tra cứu lại.
- **Actor / entity liên quan**: Admin (người duy nhất quyết định dừng hay tiếp).
- **Allowed values**: —
- **Unit**: —
- **Terms dễ nhầm**: **INTERMISSION** (TERM-024) là chuyển tiếp bình thường giữa hai vòng theo luật, **không** phải cơ chế xử lý sự cố · **bỏ / chạy lại vòng** (`Đ-5.1`) mới là van thoát khi sự cố xảy ra giữa một cửa sổ có ràng buộc thời gian.
- **Related rules**: `Đ-5.1` (bỏ và chạy lại vòng).
- **Source**: quyết định chủ dự án 2026-07-25 — bãi bỏ R-GEN-08 và mọi nhánh phái sinh (U-12, U-13 phần auto-pause, GRR-165, GRR-166, GRR-167).
- **Status**: `CONFIRMED`

## TERM-024 — INTERMISSION

- **Definition**: trạng thái nghỉ giữa các vòng. Được liệt kê trong máy trạng thái; **điều kiện vào và ra chưa được phát biểu** ở bất kỳ nguồn nào.
- **Alternative names**: nghỉ giữa vòng.
- **Actor / entity liên quan**: Match, Admin.
- **Allowed values**: —
- **Unit**: —
- **Terms dễ nhầm**: INTERMISSION là chuyển tiếp bình thường theo luật; khái niệm "tạm dừng trận" đã bị bỏ (TERM-023).
- **Related rules**: §Trạng thái game, R-VCNV-07, R-TEAM-07 (đổi đại diện chỉ tại INTERMISSION), GRR-077.
- **Source**: `game-rules-inventory.md` §Trạng thái game, §R-VCNV-07.
- **Status**: `NEEDS CLARIFICATION`

## TERM-025 — TIE_BREAK

- **Definition**: trạng thái chạy vòng **Câu hỏi phụ** để phân định thí sinh hoà điểm. Điều kiện vào theo F26: *"Sau phần thi Về đích, các thí sinh có cùng số điểm…"*; repo thu hẹp còn **chỉ vị trí NHẤT** (`tieBreakPositions` default `[1]`).
- **Alternative names**: Câu hỏi phụ · vòng phụ · tie-break.
- **Actor / entity liên quan**: Nhóm hoà, Admin, Bốc thăm (TERM-048).
- **Allowed values**: 3 câu × 15 giây; `tieBreakPositions` default `[1]`.
- **Unit**: câu · giây.
- **Terms dễ nhầm**: **Câu hỏi phụ ≠ câu hỏi dự phòng** (câu thay thế khi media hỏng).
- **Related rules**: R-TB-01 → R-TB-05, K-14, GRR-155, GRR-156, GRR-158.
- **Source**: `docs/source/fandom-olympia-26-luat-choi.md` §Câu hỏi phụ · `game-rules-inventory.md` §R-TB-01.
- **Status**: `NEEDS CLARIFICATION` — GRR-155: nếu vòng Về đích bị bỏ thì tiền đề *"sau phần thi Về đích"* không xảy ra, chưa có mốc thay thế.

## TERM-026 — Event · MatchEvent

- **Definition**: bản ghi **append-only** của mọi việc xảy ra trong trận. **Điểm là hàm của event log**, không phải một con số bị sửa trực tiếp. Lịch sử **linear, không xoá**.
- **Alternative names**: sự kiện trận · event log · MatchEvent.
- **Actor / entity liên quan**: Server, Điểm (TERM-039), Revert (TERM-029).
- **Allowed values**: các loại đã đặt tên: `QUESTIONS_DRAWN` · `QUESTION_USED` · `SCORE_ADJUST` · `MEDIA_KEY`.
- **Unit**: —
- **Terms dễ nhầm**: **Audit log** — bảng chung ghi mọi thao tác mọi role (auth, CRUD, xuất/nhập), khác event trận.
- **Related rules**: Đ-5.3, R-GEN-06, R-GEN-07, R-KD-07.
- **Source**: `game-rules-inventory.md` §R-GEN-07, §R-GEN-06 · `game-rules-decisions.md` §7.1.
- **Status**: `CONFIRMED`

## TERM-027 — Event điểm `⚠ CONFLICT`

- **Definition**: đơn vị event sinh ra điểm. **Độ hạt được định nghĩa hai kiểu loại trừ nhau**:
  - **(E1) Theo thí sinh** — dedup ở server theo khoá `(câu, thí sinh, loại phán quyết)`; *"bấm Sai sau Đúng = revert + event mới"*.
  - **(E2) Theo câu, cho toàn bảng** — *"một câu = MỘT event điểm cho TOÀN BỘ người chơi"* ở vòng xếp hạng (Tăng tốc); revert = revert **cả bảng xếp hạng** của câu đó.
- **Alternative names**: score event · event chấm điểm.
- **Actor / entity liên quan**: Admin, Server, Tăng tốc.
- **Allowed values**: —
- **Unit**: điểm.
- **Terms dễ nhầm**: E1 có chiều **thí sinh**, E2 **không có** ⇒ không tồn tại một khoá dedup dùng chung cho cả hai; đổi phán quyết một người ở Tăng tốc rơi vào vùng chưa định nghĩa.
- **Related rules**: R-TT-01, R-GEN-03, R-GEN-07, GRR-147.
- **Source**: `game-rules-decisions.md` §3.3 (E1), §3.4 `[Đ-5.3.1]` (E2) · `docs/reviews/game-rules-review.md` GRR-147.
- **Status**: `CONFLICT`

## TERM-028 — Action

- **Definition**: **không có định nghĩa nghiệp vụ**. Từ chỉ xuất hiện ở tầng UX: *"Toast/message thành công hoặc thất bại phải xuất hiện sau mỗi action"*, *"nút action chỉ hiện trạng thái loading, KHÔNG disable"*. Không nguồn nào định nghĩa tập action của trận, cũng không phân biệt action với event.
- **Alternative names**: thao tác · nút hành động.
- **Actor / entity liên quan**: mọi UI.
- **Allowed values**: —
- **Unit**: —
- **Terms dễ nhầm**: **Event** (TERM-026) — event là cái đã xảy ra và được ghi; action là cái người dùng bấm và **có thể bị reject**.
- **Related rules**: Đ-7 (tín hiệu → hàng đợi → xác nhận).
- **Source**: `CLAUDE.md` §UX BẮT BUỘC.
- **Status**: `NEEDS CLARIFICATION`

## TERM-029 — Revert

- **Definition**: hoàn nguyên điểm bằng cách **thêm event đảo ngược** (tương tự `git revert`, **KHÔNG phải** `git reset --hard`). Event cũ không bị xoá; lịch sử luôn linear, append-only.
- **Alternative names**: reset điểm (cách gọi cũ) · hoàn nguyên.
- **Actor / entity liên quan**: Admin, Event log, Bỏ vòng (TERM-038).
- **Allowed values**: —
- **Unit**: —
- **Terms dễ nhầm**: **`SCORE_ADJUST`** (TERM-030) — revert là hoàn nguyên máy móc một event đã có; `SCORE_ADJUST` là phán quyết mới của người và **KHÔNG tự revert** theo vòng · **Undo** — ràng buộc "chỉ event gần nhất" đã bị Đ-5.3 thay thế.
- **Related rules**: Đ-5.2, Đ-5.3, Đ-5.3.X, GRR-159, GRR-160.
- **Source**: `game-rules-decisions.md` §6.3, §7.1, §7.2.
- **Status**: `NEEDS CLARIFICATION` — GRR-160: chưa nói lần bấm revert thứ hai là no-op hay sinh bộ event đảo ngược thứ hai.

## TERM-030 — SCORE_ADJUST

- **Definition**: event điều chỉnh điểm thủ công `{delta, reason}` — **bắt buộc nhập lý do**, vào audit log. Là **phán quyết của người** nên **KHÔNG tự revert** khi vòng bị bỏ.
- **Alternative names**: chỉnh điểm tay · điều chỉnh thủ công.
- **Actor / entity liên quan**: Admin, Event log.
- **Allowed values**: `delta` số nguyên (âm hoặc dương); `reason` bắt buộc.
- **Unit**: điểm.
- **Terms dễ nhầm**: **Revert** (TERM-029).
- **Related rules**: R-GEN-07, Đ-11.B, GRR-153, GRR-159.
- **Source**: `game-rules-inventory.md` §R-GEN-07 · `game-rules-decisions.md` §6.3.
- **Status**: `NEEDS CLARIFICATION` — GRR-159: chưa nói `SCORE_ADJUST` gắn với một vòng hay là điều chỉnh cấp trận.

---

# D. TÍN HIỆU VÀ THAO TÁC

## TERM-031 — Chuông

- **Definition**: tín hiệu thí sinh phát để **giành quyền trả lời**. **Chỉ nhận click chuột — không gán hotkey** (tránh bấm nhầm khi gõ đáp án). Phía thí sinh không có dialog, bấm là gửi ngay.
- **Alternative names**: buzz · buzzer · nút bấm chuông.
- **Actor / entity liên quan**: Thí sinh, Hàng đợi (TERM-032), Server time.
- **Allowed values**: các vòng có chuông: Khởi động lượt chung · VCNV ("Mở chướng ngại vật") · Về đích (cướp quyền) · Câu hỏi phụ.
- **Unit**: —
- **Terms dễ nhầm**: **Nút "Mở chướng ngại vật" ĐƯỢC XẾP LÀ CHUÔNG** ⇒ cũng chỉ nhận click chuột · **Nút gửi đáp án** không phải chuông, có hotkey Enter.
- **Related rules**: R-GEN-01, R-KD-03, Đ-4.3, GRR-138, GRR-140.
- **Source**: `CLAUDE.md` §UX BẮT BUỘC · `game-rules-inventory.md` §R-GEN-01 · `game-rules-decisions.md` §5.3.
- **Status**: `CONFIRMED`

## TERM-032 — Hàng đợi tín hiệu

- **Definition**: mọi tín hiệu của thí sinh vào hàng đợi **theo thứ tự tới (server timestamp)**. **KHÔNG có cơ chế drop.** Hàng đợi **đang hoạt động** reset sau mỗi **vòng**; **LỊCH SỬ tín hiệu KHÔNG BAO GIỜ XOÁ** (append-only). Queue **chỉ CHẶN ở VCNV**; ở Khởi động lượt chung và cướp quyền Về đích thì **không chặn** — có chuông là tính ngay.
- **Alternative names**: queue · hàng chờ duyệt.
- **Actor / entity liên quan**: Thí sinh, Admin, Server.
- **Allowed values**: chặn (VCNV) · không chặn (Khởi động lượt chung, Về đích) · không chặn nhưng xử lý sau khi hết 15 giây (Câu hỏi phụ).
- **Unit**: —
- **Terms dễ nhầm**: **"Không chặn" ≠ "không ghi nhận"** — queue vẫn ghi thứ tự để admin can thiệp khi có sự cố.
- **Related rules**: Đ-7, Đ-7.a, Đ-7.b, Đ-7.2, GRR-140, GRR-144.
- **Source**: `game-rules-decisions.md` §5.1, §5.2 · `CLAUDE.md` §UX BẮT BUỘC.
- **Status**: `CONFIRMED`

## TERM-033 — Reject

- **Definition**: admin bấm **No** cho một tín hiệu trong hàng đợi ⇒ tín hiệu kế tiếp lên; **reject KHÔNG làm thí sinh mất lượt**. Đây là chỗ sửa lỗi bấm nhầm của thí sinh.
- **Alternative names**: bấm No · từ chối tín hiệu · không duyệt.
- **Actor / entity liên quan**: Admin, Hàng đợi, Thí sinh.
- **Allowed values**: —
- **Unit**: —
- **Terms dễ nhầm**: **Bị loại** (TERM-036) — reject là từ chối một tín hiệu, không đụng tư cách dự thi.
- **Related rules**: Đ-7, GRR-143.
- **Source**: `game-rules-decisions.md` §5.1.
- **Status**: `NEEDS CLARIFICATION` — GRR-143: chưa nói các tác dụng phụ đã phát sinh (câu đã rút, hàng đã đánh dấu, timer đã chạy) có được hoàn nguyên khi reject không.

## TERM-034 — Mốc admin

- **Definition**: thời điểm do admin bấm, thay cho một mốc mà luật gốc mô tả bằng hành vi của MC. Ba mốc: *"MC đọc xong câu hỏi"* → **admin bấm start timer**; *"hiệu lệnh của MC"* (Câu hỏi phụ) → **admin bấm**; *"câu hỏi được đọc lên hoặc hiện lên màn hình"* (đóng cửa sổ NSHV) → **admin bấm hiển thị câu hỏi**. Mốc admin là **TUYỆT ĐỐI**: không có cửa sổ ân hạn, không trừ bù độ trễ tay người.
- **Alternative names**: mốc thời gian của admin · admin là cảm biến.
- **Actor / entity liên quan**: Admin, MC, Server time, Timer.
- **Allowed values**: —
- **Unit**: —
- **Terms dễ nhầm**: **"Bấm hiển thị câu hỏi" và "bấm start timer" là HAI thao tác khác nhau** — thứ tự và khoảng cách giữa chúng quyết định độ dài cửa sổ chuông (GRR-139).
- **Related rules**: Đ-6, Đ-6.1, Đ-6.3, `[GRR-005]`, `[GRR-048]`, GRR-137, GRR-139.
- **Source**: `game-rules-decisions.md` §8.1, §8.2.
- **Status**: `NEEDS CLARIFICATION` — GRR-137 (bấm hai lần) và GRR-139 (thứ tự hai thao tác) chưa có quy định.

## TERM-035 — Phán quyết Đúng/Sai

- **Definition**: hành vi **admin bấm Đúng hoặc Sai** để chốt kết quả một câu. **Máy KHÔNG tự chấm**: hệ thống chỉ (1) hiển thị đáp án thí sinh cạnh đáp án đúng, (2) **highlight ký tự khác** như gợi ý. Ở trận `official`, **`autoJudge` không tồn tại** dưới bất kỳ hình thức nào.
- **Alternative names**: chấm · chấm điểm · judge · hotkey C/X.
- **Actor / entity liên quan**: Admin, Server, MC (quyết bằng lời).
- **Allowed values**: `Đúng` · `Sai`.
- **Unit**: —
- **Terms dễ nhầm**: **Normalize / so khớp** (TERM-052 liên quan) — kết quả so khớp là **đầu vào của gợi ý**, KHÔNG phải phán quyết · **`autoJudge`** chỉ tồn tại ở `practice`.
- **Related rules**: R-GEN-03, Đ-1, K-16, GRR-148, GRR-161, GRR-162.
- **Source**: `game-rules-inventory.md` §R-GEN-03 · `game-rules-decisions.md` §3.1.
- **Status**: `CONFIRMED`

## TERM-036 — Bị loại

- **Definition**: **trạng thái của một thí sinh trong vòng VCNV** sau khi trả lời **sai Chướng ngại vật**: *"Nếu trả lời sai Chướng ngại vật, thí sinh sẽ bị loại khỏi phần thi này."* Chỉ mất quyền trong **vòng đó**, không rời trận.
- **Alternative names**: loại khỏi phần thi · eliminated.
- **Actor / entity liên quan**: Thí sinh, VCNV.
- **Allowed values**: —
- **Unit**: —
- **Terms dễ nhầm**: **KHÔNG phải "loser"** (TERM-046) — người bị loại khỏi VCNV vẫn thi tiếp Tăng tốc và Về đích, vẫn có thể thắng trận · **Mất quyền trả lời** (TERM-037) là hình phạt khác, theo **câu**, chỉ ở Câu hỏi phụ.
- **Related rules**: R-VCNV-04, R-TEAM-03, U-24, GRR-146.
- **Source**: `docs/source/fandom-olympia-26-luat-choi.md` §Vượt chướng ngại vật · `game-rules-inventory.md` §R-VCNV-04.
- **Status**: `NEEDS CLARIFICATION` — U-24 (người bị loại có giữ điểm hàng ngang đã kiếm không) và GRR-146 (sự kiện đến từ ghế đã bị loại xử lý ra sao) chưa chốt.

## TERM-037 — Mất quyền trả lời · Lệnh cấm

- **Definition**: hình phạt khi bấm chuông **trước hiệu lệnh của MC**; **chỉ áp ở Câu hỏi phụ**, phạm vi **theo CÂU**. Là một **TRẠNG THÁI CẤM** (không phải bỏ qua một lần bấm) và **gỡ được**: MC quyết bằng lời, admin bấm, trước khi trả lời và trong lúc câu còn mở. Lệnh cấm **chết cùng câu**.
- **Alternative names**: lệnh cấm · cấm trả lời · mất quyền.
- **Actor / entity liên quan**: Thí sinh, MC, Admin.
- **Allowed values**: phạm vi = một câu.
- **Unit**: —
- **Terms dễ nhầm**: **Bị loại** (TERM-036) — phạm vi vòng, không gỡ được theo cơ chế này. Hai vòng khác quy định **NGƯỢC LẠI**: Khởi động cho bấm *"trong khi MC đang đọc"*, VCNV cho bấm *"bất cứ lúc nào"* (K-3).
- **Related rules**: R-TB-03, K-3, Đ-6.2, Đ-6.4.
- **Source**: `docs/source/fandom-olympia-26-luat-choi.md` §Câu hỏi phụ · `game-rules-decisions.md` §8.3.
- **Status**: `CONFIRMED`

## TERM-038 — Bỏ vòng · Chạy lại vòng · Huỷ `⚠ CONFLICT`

- **Definition**: cụm khái niệm "cancelled" trong miền này gồm **ba thứ khác nhau**, không thay thế cho nhau:
  - **(C1) Bỏ vòng** — admin bỏ hẳn một vòng; điểm của vòng bị **revert**; biên bản giữ đầy đủ, vòng hiện **nhãn "đã bỏ"**; **câu đã dùng KHÔNG trả lại pool**.
  - **(C2) Chạy lại vòng** — cùng quy tắc với C1 rồi chạy mới, với điều kiện ngân hàng đề còn câu.
  - **(C3) Huỷ cửa sổ cướp** — câu bị skip khi đang mở cửa sổ cướp ⇒ huỷ cửa sổ, không ai cộng/trừ. **Còn nhãn "đề xuất", chưa chốt** (U-19).
  - **(C4) Reject tín hiệu** (TERM-033) — từ chối một tín hiệu, không đụng vòng lẫn điểm.
- **Alternative names**: skip vòng · reset vòng (cách gọi cũ, đã thay bằng **revert**) · huỷ cửa sổ.
- **Actor / entity liên quan**: Admin, Event log, Pool đề.
- **Allowed values**: —
- **Unit**: —
- **Terms dễ nhầm**: **"Reset" KHÔNG có nghĩa xoá** — reset điểm = revert (TERM-029) · C1 hoàn được **điểm** nhưng không hoàn được **đề đã lộ**.
- **Related rules**: Đ-5.1, Đ-5.2, Đ-5.2d, Đ-5.2f, U-19, GRR-160, GRR-161.
- **Source**: `game-rules-decisions.md` §6.3 · `game-rules-inventory.md` §R-VD-05 (U-19).
- **Status**: `CONFLICT`

---

# E. ĐIỂM VÀ KẾT QUẢ

## TERM-039 — Điểm

- **Definition**: đại lượng tích luỹ quyết định thứ hạng, tính bằng **reduce(event log)**. **Được phép ÂM, không có sàn.** Điểm âm tham gia bình thường vào xếp lượt Về đích và điều kiện hoà.
- **Alternative names**: score · điểm số.
- **Actor / entity liên quan**: Đơn vị điểm, Event log, Server.
- **Allowed values**: số nguyên, âm hoặc dương, không chặn dưới. Giá trị **lẻ** chỉ phát sinh khi admin cấu hình mức điểm lẻ — dưới luật 2026 không tồn tại (20/30 → nửa là 10/15).
- **Unit**: điểm.
- **Terms dễ nhầm**: **Giá trị câu** (TERM-040) — một câu có `value`, khác với điểm tích luỹ của người · **Thứ hạng** — dẫn xuất từ điểm.
- **Related rules**: Đ-2, Đ-3, Đ-5.3, U-20.
- **Source**: `game-rules-decisions.md` §3.2, §9.4 · `CLAUDE.md` §Hai mode trả lời.
- **Status**: `CONFIRMED` (quy tắc làm tròn khi −½ giá trị lẻ vẫn treo — U-20)

## TERM-040 — Giá trị câu · Mức điểm

- **Definition**: số điểm gắn với một câu hỏi (`value`). Ở Về đích, thí sinh **chọn gói 3 câu từ 2 mức {20, 30}**. Thời gian **KHÔNG suy từ mức điểm** — `timeSeconds` là metadata **từng câu** (TERM-052).
- **Alternative names**: value · mức điểm · điểm của câu hỏi.
- **Actor / entity liên quan**: Câu hỏi, Về đích, Setter.
- **Allowed values**: O26 Về đích: **{20, 30}**. Khởi động: 10 (đúng), −5 (sai, lượt chung). VCNV hàng ngang: 10. VCNV Chướng ngại vật: **60/50/40/30**, sau gợi ý cuối **20**; ô trung tâm 10. Tăng tốc: **40/30/20/10** theo thứ hạng tốc độ.
- **Unit**: điểm.
- **Terms dễ nhầm**: **`W26` ghi mức 20 và 40 cho Về đích — đã BỊ LOẠI** theo D8 (K-2). **Athena cũ**: VCNV 80/60/40/20, Về đích 10/20/30 — **đã bãi bỏ** (K-15).
- **Related rules**: R-VD-01, R-VCNV-04, R-TT-01, K-2, K-15.
- **Source**: `docs/source/fandom-olympia-26-luat-choi.md` (4 heading vòng) · `game-rules-inventory.md` §8A K-2, K-15.
- **Status**: `CONFIRMED` cho 4 ghế · thang cho ≠4 ghế chưa có (U-2, U-3, K-11).

## TERM-041 — Cướp quyền

- **Definition**: khi người thi chính ở Về đích trả lời **sai**, *"1 trong 3 thí sinh còn lại sẽ giành quyền trả lời bằng cách bấm chuông nhanh **trong 5 giây**"*. Cướp đúng **giành được điểm TỪ thí sinh trả lời sai** (`stealMode: 'transfer'` — người sai −value, người cướp +value). Cướp sai bị trừ **một nửa** giá trị câu.
- **Alternative names**: steal · giành quyền trả lời · transfer.
- **Actor / entity liên quan**: Thí sinh, Admin, Chuông.
- **Allowed values**: cửa sổ **5 giây**; người đủ điều kiện = các thí sinh **khác** người thi chính.
- **Unit**: giây · điểm.
- **Terms dễ nhầm**: **`stealMode: 'add'`** (cộng không trừ người sai) là option hợp lệ của hệ thống nhưng **KHÔNG được dùng cho preset O26** (K-12) · **Ghi nhận đáp án ngược nhau**: người thi chính tính **bản cuối**, người cướp chỉ tính **bản đầu tiên**.
- **Related rules**: R-VD-05, K-12, U-5, U-8, GRR-150, GRR-151, GRR-152.
- **Source**: `docs/source/fandom-olympia-26-luat-choi.md` §Về đích · `game-rules-inventory.md` §R-VD-05.
- **Status**: `NEEDS CLARIFICATION` — GRR-150 (tín hiệu đến trước khi cửa sổ mở), GRR-152 (giá trị câu có NSHV cho vế người cướp), U-5 (số người cướp khi ghế ≠ 4).

## TERM-042 — Ngôi sao hy vọng (NSHV)

- **Definition**: quyền đặt cược điểm ở Về đích, **1 lần / thí sinh / trận**. Đúng ⇒ **gấp đôi** giá trị câu. Sai ⇒ **trừ đi giá trị câu**, *"kể cả các thí sinh còn lại có giành quyền trả lời hay không"*. Phải đặt **trước khi câu được đọc lên hoặc hiện lên màn hình** — mốc đóng = **admin bấm hiển thị câu hỏi**.
- **Alternative names**: NSHV · ngôi sao hy vọng · hope star.
- **Actor / entity liên quan**: Thí sinh, Về đích, Admin.
- **Allowed values**: 1 lần / đơn vị điểm / trận (v2: 1 lần / ĐỘI).
- **Unit**: lần.
- **Terms dễ nhầm**: penalty NSHV **THAY THẾ** transfer chứ không cộng dồn — trừ **MỘT** lần (`[GRR-047]`), và đó là kết luận **chỉ cho người ĐẶT sao**, chưa cho người cướp (GRR-152).
- **Related rules**: R-VD-06, U-8, U-18, `[GRR-047]`, `[GRR-048]`, GRR-152, GRR-170.
- **Source**: `docs/source/fandom-olympia-26-luat-choi.md` §Về đích · `game-rules-decisions.md` §9.4.
- **Status**: `CONFIRMED` cho vế người đặt · vế người cướp `NEEDS CLARIFICATION` (GRR-152).

## TERM-043 — Thứ hạng tốc độ

- **Definition**: cơ chế tính điểm của **Tăng tốc**: điểm theo **thứ hạng nhanh trong số người trả lời ĐÚNG**, đo bằng **server-received timestamp của bản cuối**. Nhiều người cùng thời gian ⇒ **cùng nhận một mức điểm**.
- **Alternative names**: ranked-speed · xếp hạng tốc độ.
- **Actor / entity liên quan**: Thí sinh, Server, Admin (xác nhận Đúng).
- **Allowed values**: 40/30/20/10 cho 4 đơn vị điểm; độ phân giải "đồng thời gian" = **ms** (server-received).
- **Unit**: điểm · ms.
- **Terms dễ nhầm**: thứ hạng chỉ tính trên **tập người được admin chấm ĐÚNG**, không phải trên toàn bộ người gửi · `W26` dùng độ phân giải **2 chữ số thập phân** — **đã bị loại** (K-8).
- **Related rules**: R-TT-01, R-TT-02, R-TT-03, K-8, U-2, GRR-147, GRR-148.
- **Source**: `docs/source/fandom-olympia-26-luat-choi.md` §Tăng tốc · `game-rules-inventory.md` §R-TT-01, §8A K-8.
- **Status**: `CONFIRMED` cho 4 ghế · thang cho ≠4 ghế chưa có (U-2).

## TERM-044 — Result · Kết quả `⚠ CONFLICT`

- **Definition**: dùng cho **ba thứ khác nhau**:
  - **(R1) Kết quả chấm một câu** — Đúng / Sai do admin bấm.
  - **(R2) Kết quả trận** — bảng điểm cuối + thứ hạng, xuất biên bản/PDF.
  - **(R3) Kết quả tie-break** — người thắng Câu hỏi phụ; **không cộng điểm**, chỉ đổi thứ hạng.
- **Alternative names**: (R1) phán quyết · (R2) bảng điểm cuối, biên bản trận · (R3) người thắng cuộc.
- **Actor / entity liên quan**: Admin, Event log, TIE_BREAK.
- **Allowed values**: (R1) `Đúng` / `Sai`.
- **Unit**: —
- **Terms dễ nhầm**: R2 là hàm của event log; **R3 không được ghi bằng vật gì cả** — không nguồn nào nói nó có phải event không, có revert được không, và khi R2 mâu thuẫn R3 thì cái nào thắng (GRR-156).
- **Related rules**: R-TB-02, `[GRR-058]`, Đ-5.3, GRR-156.
- **Source**: `game-rules-decisions.md` §9.5, §7.1 · `docs/reviews/game-rules-review.md` GRR-156.
- **Status**: `CONFLICT`

## TERM-045 — Người thắng

- **Definition**: F26 dùng cụm *"thí sinh thắng cuộc"* **chỉ trong ngữ cảnh Câu hỏi phụ** (*"Sau 3 câu hỏi, nếu không tìm được thí sinh thắng cuộc…"*). Luật gốc **không định nghĩa riêng người thắng trận**; mặc nhiên là người điểm cao nhất, nhưng phát biểu đó không tồn tại thành câu ở nguồn nào.
- **Alternative names**: winner · thí sinh thắng cuộc · người vô địch · hạng nhất.
- **Actor / entity liên quan**: Thí sinh, TIE_BREAK, Kết quả trận.
- **Allowed values**: —
- **Unit**: —
- **Terms dễ nhầm**: **thắng tie-break ≠ điểm cao nhất** — GRR-156 chỉ ra kịch bản biên bản ghi "người thắng tie-break" là người **thua điểm** sau khi admin revert một event Về đích.
- **Related rules**: R-TB-02, R-TB-04, `[GRR-058]`, GRR-156.
- **Source**: `docs/source/fandom-olympia-26-luat-choi.md` §Câu hỏi phụ · `docs/reviews/game-rules-review.md` GRR-156.
- **Status**: `NEEDS CLARIFICATION`

## TERM-046 — Người thua

- **Definition**: **không tồn tại như một khái niệm luật.** F26 không có phát biểu nào về "người thua"; không có hệ quả nào gắn với việc thua (không loại khỏi trận, không mất điểm). Hai khái niệm gần nhất là **bị loại khỏi VCNV** (TERM-036) và **mất quyền trả lời một câu** (TERM-037) — cả hai đều **theo phạm vi hẹp**, không phải thua trận.
- **Alternative names**: loser.
- **Actor / entity liên quan**: —
- **Allowed values**: —
- **Unit**: —
- **Terms dễ nhầm**: **"bị loại" (eliminated) KHÔNG dịch sang "loser"** — đây là nhầm lẫn dễ đưa vào code nhất khi đặt tên cờ trạng thái.
- **Related rules**: R-VCNV-04, R-TB-03, U-24, `[GRR-065]` (thứ hạng của nhóm không được phân định).
- **Source**: `docs/source/fandom-olympia-26-luat-choi.md` (rà toàn văn, không có) · `game-rules-inventory.md` §R-VCNV-04.
- **Status**: `NEEDS CLARIFICATION`

## TERM-047 — Hoà

- **Definition**: **bằng điểm**. Là điều kiện kích hoạt Câu hỏi phụ (*"các thí sinh có cùng số điểm"*); repo thu hẹp còn **chỉ vị trí NHẤT**. Ở Tăng tốc còn có nghĩa hẹp hơn: **cùng server timestamp** ⇒ cùng nhận một mức điểm.
- **Alternative names**: tie · bằng điểm · đồng hạng · "đồng thời gian" (nghĩa Tăng tốc).
- **Actor / entity liên quan**: TIE_BREAK, Thứ hạng tốc độ.
- **Allowed values**: điểm âm cũng tham gia điều kiện hoà bình thường.
- **Unit**: điểm (hoà điểm) · ms (hoà thời gian).
- **Terms dễ nhầm**: **hoà ĐIỂM** (kích hoạt tie-break) và **hoà THỜI GIAN** (chia cùng mức điểm Tăng tốc) là hai thứ khác nhau · tiếng Anh "draw" cũng nghĩa là hoà ⇒ xem TERM-048.
- **Related rules**: R-TB-01, R-TT-02, K-14, Đ-2.
- **Source**: `docs/source/fandom-olympia-26-luat-choi.md` §Câu hỏi phụ, §Tăng tốc · `game-rules-inventory.md` §R-TB-01.
- **Status**: `CONFIRMED`

## TERM-048 — Draw `⚠ CONFLICT`

- **Definition**: từ này ứng với **ba khái niệm không liên quan nhau**:
  - **(D1) Rút đề ngẫu nhiên** — `Turn.drawConfig.mode = 'draw'`, server rút câu **trong danh sách đã gán**, phát event `QUESTIONS_DRAWN` để **replay được**.
  - **(D2) Bốc thăm phân định** — hết 3 câu tie-break vẫn hoà ⇒ *"các thí sinh sẽ phải **bốc thăm** để chọn ra thí sinh thắng cuộc"* (`exhaustedFallback: 'random-draw'`); hệ thống random, **admin xác nhận**.
  - **(D3) Hoà** — nghĩa tiếng Anh thông dụng, ứng với TERM-047.
- **Alternative names**: (D1) rút đề, draw câu · (D2) bốc thăm, random-draw · (D3) tie.
- **Actor / entity liên quan**: Server, Admin, Pool đề, TIE_BREAK.
- **Allowed values**: (D1) `mode: 'draw'` · (D2) `exhaustedFallback: 'random-draw'`.
- **Unit**: —
- **Terms dễ nhầm**: D1 **có** event ghi lại và replay được; D2 **chưa có phát biểu tương tự** ⇒ một nguồn ngẫu nhiên không ghi lại làm trận không tái dựng được (GRR-157). Đặt tên chung `draw*` cho cả hai sẽ che mất khác biệt này.
- **Related rules**: R-KD-07, R-TB-04, K-13, GRR-157.
- **Source**: `game-rules-inventory.md` §R-KD-07 (D1), §R-TB-04 (D2) · `docs/source/fandom-olympia-26-luat-choi.md` §Câu hỏi phụ (D2).
- **Status**: `CONFLICT`

## TERM-049 — Timeout · Hết giờ

- **Definition**: thời điểm server đóng cửa sổ nhận đáp án hoặc nhận chuông. **Server time là source of truth DUY NHẤT** — timeout, thứ tự chuông, thứ hạng tốc độ đều theo đồng hồ server; client chỉ hiển thị. Submission tới **sau server-timeout** bị loại.
- **Alternative names**: hết giờ · server-timeout · deadline · `endsAt`.
- **Actor / entity liên quan**: Server, Timer, mọi vòng.
- **Allowed values**: Khởi động 3s · VCNV hàng ngang 15s · VCNV sau gợi ý cuối 15s · Tăng tốc 20/20/30/30s · Về đích 15s (câu 20đ) / 20s (câu 30đ) · cửa sổ cướp 5s · Câu hỏi phụ 15s.
- **Unit**: giây (cấu hình) · ms (`remainingMs`).
- **Terms dễ nhầm**: **Ngưỡng N của auto-pause** (chưa có default — U-12) và **reconnect grace 120s** cũng là mốc thời gian nhưng **không phải timeout của câu** · **Biên** t = đúng mốc là trong hay ngoài — GRR-002.
- **Related rules**: R-GEN-05, R-TT-03, R-GEN-08, R-GEN-09, GRR-166, GRR-169.
- **Source**: `docs/source/fandom-olympia-26-luat-choi.md` (mọi heading vòng) · `game-rules-inventory.md` §R-GEN-05.
- **Status**: `CONFIRMED`

## TERM-050 — Reconnect grace

- **Definition**: cửa sổ **120 giây** giữ ghế cho thí sinh mất kết nối; trong grace: **giữ ghế + state-sync**, banner "đang kết nối lại". Quá grace ⇒ xử theo `dropoutPolicy`.
- **Alternative names**: grace · thời gian giữ ghế.
- **Actor / entity liên quan**: Thí sinh, Server, Admin.
- **Allowed values**: **120 giây**; `dropoutPolicy` — **danh sách giá trị chưa đầy đủ** (U-13).
- **Unit**: giây.
- **Terms dễ nhầm**: rớt mạng **đúng lượt riêng** thì engine pause + admin quyết, **ghi đè** grace (D13.4).
- **Related rules**: R-GEN-09, U-13, GRR-169.
- **Source**: `game-rules-inventory.md` §R-GEN-09.
- **Status**: `NEEDS CLARIFICATION` — U-13 (tập giá trị `dropoutPolicy`) và GRR-169 (submission tới đúng biên grace).

---

# F. ĐỀ VÀ KHO ĐỀ

## TERM-051 — Câu hỏi

- **Definition**: đơn vị đề thi. Người tạo contest **PHẢI chọn danh sách câu hỏi trước khi start**; hệ thống KHÔNG tự lấy đề — draw chỉ random **trong danh sách đã gán**.
- **Alternative names**: câu · question · đề.
- **Actor / entity liên quan**: Setter, Kho đề, Turn.
- **Allowed values**: field hiện có: `displayId, fieldId, wordCount, explanation, note, timeSeconds?, value?, clues[]?, everPublic`.
- **Unit**: câu.
- **Terms dễ nhầm**: F26 mô tả **các loại câu chưa khai được bằng field hiện có**: trắc nghiệm Khởi động (U-34), câu sắp xếp / chọn ảnh A-F ở Tăng tốc (U-36), **câu thực hành** ở Về đích (U-6), và trường phân biệt **câu miệng vs câu gõ** (U-7).
- **Related rules**: R-KD-05, R-TT-04, R-VD-04, U-6, U-7, U-34, U-36.
- **Source**: `docs/source/fandom-olympia-26-luat-choi.md` §Khởi động, §Tăng tốc, §Về đích · `game-rules-inventory.md` §PHẦN 9.
- **Status**: `NEEDS CLARIFICATION`

## TERM-052 — `timeSeconds`

- **Definition**: thời gian suy nghĩ, là **metadata của TỪNG CÂU HỎI** — cùng mức 20đ có thể câu 15s và câu 40s. Hệ thống chọn câu theo **MỨC ĐIỂM**, thời gian lấy **theo câu**; preset chỉ đặt default (`defaultTimeByValue`).
- **Alternative names**: thời gian suy nghĩ · thời gian câu.
- **Actor / entity liên quan**: Câu hỏi, Setter, Timer.
- **Unit**: giây.
- **Allowed values**: per-question override **thắng** default của preset.
- **Terms dễ nhầm**: nguồn ngoài **gắn chặt thời gian với mức điểm** (20đ→15s, 30đ→20s) — repo tổng quát hoá vượt nguồn, đây là **chủ đích** · Athena cũ suy `time = value/2 + 5` — **đã bãi bỏ**.
- **Related rules**: R-GEN-02, K-15, GRR-149.
- **Source**: `game-rules-inventory.md` §R-GEN-02 · `CLAUDE.md` §Luật chơi & đề thi.
- **Status**: `CONFIRMED`

## TERM-053 — Kho đề · Pool

- **Definition**: tập câu hỏi khả dụng. **Pool đã gán cho contest** là snapshot admin chọn trước khi start; pre-flight **chặn start** khi thiếu (thuật toán 2 bước + `reservePerField` default 2).
- **Alternative names**: pool · ngân hàng đề · bộ đề · danh sách đã gán.
- **Actor / entity liên quan**: Admin, Server, Contest.
- **Allowed values**: —
- **Unit**: câu.
- **Terms dễ nhầm**: **Kho đề toàn hệ thống** ≠ **pool đã gán cho contest** ≠ **pool còn lại sau no-repeat**.
- **Related rules**: R-KD-07, R-GEN-06, R-VD-07, U-9, U-10.
- **Source**: `game-rules-inventory.md` §R-KD-07, §R-GEN-06 · `CLAUDE.md` §Luật chơi & đề thi.
- **Status**: `NEEDS CLARIFICATION` — U-9 (pool cạn **giữa trận** do skip nhiều) và U-10 (pool câu phụ cạn giữa tie-break) chưa có xử lý.

## TERM-054 — `usedInContest` · No-repeat

- **Definition**: cờ đánh dấu câu **đã được hỏi** trong bất kỳ match/vòng nào của contest (event `QUESTION_USED`) ⇒ câu **không xuất hiện lại** trong contest, draw loại khỏi pool. **Câu đã dùng KHÔNG trả lại pool** ngay cả khi vòng bị bỏ — *"điểm hoàn được, đề đã lộ thì không"*.
- **Alternative names**: no-repeat · câu đã dùng · đã lộ đề.
- **Actor / entity liên quan**: Server, Contest, Bỏ vòng.
- **Allowed values**: —
- **Unit**: —
- **Terms dễ nhầm**: **"đã được hỏi" chưa định nghĩa rõ** = đã hiện màn hình hay đã chấm; câu skip vì media hỏng có set cờ không (U-30).
- **Related rules**: R-GEN-06, Đ-5.2f, U-30, GRR-143.
- **Source**: `game-rules-inventory.md` §R-GEN-06 · `game-rules-decisions.md` §6.3.
- **Status**: `NEEDS CLARIFICATION` — U-30.

## TERM-055 — `everPublic`

- **Definition**: cờ **một chiều** đánh dấu câu đã từng nằm trong bộ đề public. Câu `everPublic=true` ⇒ pre-flight **hard-block** mọi match; force cần confirm 2 bước + audit. Kiểm ở đơn vị **CÂU**, mỗi **match**. Ngoại lệ: match `practice` cho phép, gắn badge "đề public".
- **Alternative names**: đề đã công khai · cờ chống rò đề.
- **Actor / entity liên quan**: Admin, Server, Câu hỏi.
- **Allowed values**: `true` / `false`; **một chiều vĩnh viễn** — chưa có cách gỡ (U-29).
- **Unit**: —
- **Terms dễ nhầm**: **`Question.visibility`** — D16 định nghĩa là **cột set tay**, `SPEC` §9 định nghĩa lại là **derived, read-only** ⇒ mâu thuẫn K-10 chưa phân xử.
- **Related rules**: R-GEN-12, K-10, U-29.
- **Source**: `game-rules-inventory.md` §R-GEN-12, §8B K-10.
- **Status**: `CONFLICT` (ở nhánh `visibility` — K-10)

## TERM-056 — RuleConfig · Preset `O26_DEFAULT@1`

- **Definition**: nơi khai **mọi** timer và điểm. **KHÔNG hard-code luật trong code/UI.** Preset `O26_DEFAULT@1` là bộ giá trị mặc định theo luật O26, áp bằng nút **"Áp dụng luật 2026"**; mọi giá trị vẫn config được per-contest.
- **Alternative names**: cấu hình luật · preset luật 2026.
- **Actor / entity liên quan**: Admin, Engine, Contest.
- **Allowed values**: mọi giá trị số của TERM-040 và TERM-049.
- **Unit**: —
- **Terms dễ nhầm**: hai option **hợp lệ về mặt hệ thống nhưng KHÔNG được dùng cho preset O26**: `stealMode: 'add'` (K-12) và `exhaustedFallback: 'admin-decides'` (K-13).
- **Related rules**: K-12, K-13, R-GEN-02.
- **Source**: `game-rules-inventory.md` §PHẦN 0 (Lưu ý toàn cục), §8A K-12, K-13 · `CLAUDE.md` §Luật chơi & đề thi.
- **Status**: `CONFIRMED`

---

# G. THUẬT NGỮ RIÊNG CỦA VCNV

## TERM-057 — Hàng ngang

- **Definition**: một trong **4 từ** cần đoán, đồng thời là **4 gợi ý** liên quan đến Chướng ngại vật. Thời gian suy nghĩ mỗi hàng ngang **15 giây**; trả lời đúng **+10**. **Mọi thí sinh cùng trả lời bằng máy tính** khi một hàng ngang được chọn.
- **Alternative names**: từ hàng ngang · row.
- **Actor / entity liên quan**: Thí sinh, Miếng ghép, Admin.
- **Allowed values**: O26 = **4 hàng**; `SPEC` cho `rowCount: 4..8` (ngoài luật O26).
- **Unit**: hàng · giây · điểm.
- **Terms dễ nhầm**: **"hàng ngang được MỞ" ≠ "miếng ghép được MỞ"** — nguồn dùng hai chủ ngữ khác nhau; đây là chìa khoá đọc `[GRR-013]` · **Lượt CHỌN hàng ngang** (theo vị trí, tối đa 1 lượt/người) khác **việc TRẢ LỜI hàng ngang** (cả sân cùng trả lời).
- **Related rules**: R-VCNV-01, R-VCNV-03, `[GRR-013]`, U-4, U-37.
- **Source**: `docs/source/fandom-olympia-26-luat-choi.md` §Vượt chướng ngại vật · `game-rules-decisions.md` §9.2.
- **Status**: `NEEDS CLARIFICATION` — U-37 (ngưỡng số thí sinh đúng để mở miếng ghép) chưa chốt.

## TERM-058 — Miếng ghép

- **Definition**: mảnh của hình ảnh Chướng ngại vật. **5 miếng**: 4 miếng ở 4 góc **đánh số cố định** tương ứng 4 hàng ngang, 1 miếng ở **ô trung tâm**. Trả lời đúng hàng ngang ⇒ miếng tương ứng **mở**; không trả lời được ⇒ **không mở**.
- **Alternative names**: mảnh ghép · góc ảnh · piece.
- **Actor / entity liên quan**: Hàng ngang, Chướng ngại vật, Admin.
- **Allowed values**: 5 miếng (4 góc + 1 trung tâm) với luật O26.
- **Unit**: miếng.
- **Terms dễ nhầm**: xem TERM-057 — miếng ghép **không mở** vẫn không cản hàng ngang tiếp theo được hỏi.
- **Related rules**: R-VCNV-01, R-VCNV-05, `[GRR-013]`, U-37.
- **Source**: `docs/source/fandom-olympia-26-luat-choi.md` §Vượt chướng ngại vật.
- **Status**: `CONFIRMED`

## TERM-059 — Chướng ngại vật (CNV)

- **Definition**: đáp án ẩn mà cả vòng đi tìm. Thí sinh **bấm chuông trả lời bất cứ lúc nào**. Điểm giảm dần theo số hàng ngang đã hỏi: **60 / 50 / 40 / 30**; sau gợi ý cuối chỉ còn **20**. **Trả lời sai ⇒ bị loại khỏi phần thi này.**
- **Alternative names**: CNV · obstacle · từ khoá.
- **Actor / entity liên quan**: Thí sinh, Admin, Hàng ngang.
- **Allowed values**: 60/50/40/30/20 với 4 hàng; thang cho `rowCount` 5-8 **chưa định nghĩa** (U-3).
- **Unit**: điểm.
- **Terms dễ nhầm**: *"Trong N từ hàng ngang"* đếm theo **số hàng ĐÃ HỎI**, không phải số miếng ghép đã mở (`[GRR-019]`) · nút **"Mở chướng ngại vật" được xếp là CHUÔNG**, chỉ nhận click chuột.
- **Related rules**: R-VCNV-04, R-VCNV-06, `[GRR-019]`, `[GRR-025]`, U-3, GRR-142, GRR-145.
- **Source**: `docs/source/fandom-olympia-26-luat-choi.md` §Vượt chướng ngại vật · `game-rules-decisions.md` §9.2.
- **Status**: `NEEDS CLARIFICATION` — GRR-142 (mốc chốt băng điểm: lúc bấm hay lúc admin xác nhận).

## TERM-060 — Ô trung tâm · Gợi ý cuối

- **Definition**: miếng ghép thứ 5 kèm một câu hỏi, đưa ra **sau khi cả 4 hàng ngang đã được mở ra mà chưa ai trả lời CNV**. Trả lời đúng câu ô trung tâm **+10** và mở ô; sai thì ô **không mở**. Sau đó **15 giây** để đưa ra CNV, đúng chỉ được **20 điểm**.
- **Alternative names**: gợi ý cuối cùng · ô giữa · center piece.
- **Actor / entity liên quan**: Thí sinh, Admin.
- **Allowed values**: +10 (câu ô trung tâm) · 15 giây · 20 điểm (CNV sau gợi ý cuối).
- **Unit**: điểm · giây.
- **Terms dễ nhầm**: mốc **20 điểm** gắn với việc **gợi ý cuối đã được đưa ra**, KHÔNG phụ thuộc ô trung tâm đúng hay sai (`[GRR-025]`).
- **Related rules**: R-VCNV-05, `[GRR-013]`, `[GRR-025]`, U-25.
- **Source**: `docs/source/fandom-olympia-26-luat-choi.md` §Vượt chướng ngại vật · `game-rules-decisions.md` §9.2.
- **Status**: `NEEDS CLARIFICATION` — U-25 (ai được trả lời câu ô trung tâm: cả sân hay theo lượt).

---

# PHỤ LỤC A — Thuật ngữ được yêu cầu nhưng KHÔNG tồn tại trong miền

> Năm từ dưới đây thuộc miền **cờ bạc / giao dịch**, không có trong luật Olympia lẫn tài liệu repo. Theo quy tắc *"không tự sáng tạo định nghĩa"*, chúng **không được cấp mã TERM và không có Status** — ghi lại để lần sau không ai đi tìm nữa. Cột cuối chỉ **trỏ tới khái niệm gần nhất**, KHÔNG phải định nghĩa tương đương.

| Từ | Có trong tài liệu? | Khái niệm gần nhất trong miền (chỉ để định hướng) |
|---|---|---|
| `bet` | Không | **Ngôi sao hy vọng** (TERM-042) — đặt trước khi biết câu hỏi, đúng thì gấp đôi, sai thì trừ. Là cơ chế **rủi ro có chủ đích**, nhưng không có tiền cược, không đặt được số lượng, chỉ dùng **1 lần/trận**. |
| `balance` | Không | **Điểm** (TERM-039) — nhưng điểm **được phép âm không sàn**, không rút ra được, không chuyển tự do; ngoại lệ duy nhất là **cướp quyền transfer** (TERM-041) chuyển đúng giá trị câu giữa hai người. |
| `reward` | Không | **Giá trị câu** (TERM-040) và **thang điểm theo thứ hạng tốc độ** (TERM-043). Không có phần thưởng nào ngoài điểm. |
| `payout` | Không | **`stealMode: 'transfer'`** (TERM-041) — trường hợp duy nhất điểm đi từ chủ thể này sang chủ thể khác. |
| `dealer` | Không | Vai chia bài không tồn tại. Việc **rút đề** do **server** làm (TERM-048 D1); việc **bốc thăm** do server random + **admin xác nhận** (TERM-048 D2); việc **điều khiển trận** do **admin** (TERM-004). Ba việc, ba chủ thể — không gom được vào một vai. |

# PHỤ LỤC B — Bảng tra 25 từ được yêu cầu

| Từ yêu cầu | Tên chuẩn trong tài liệu | Mã | Status |
|---|---|---|---|
| player | Thí sinh | TERM-001 | `CONFIRMED` |
| user | *(chưa có entity)* | TERM-009 | `NEEDS CLARIFICATION` |
| host | MC **hoặc** Admin — tuỳ ngữ cảnh | TERM-006 | `CONFLICT` |
| dealer | *(không tồn tại)* | Phụ lục A | — |
| administrator | Admin | TERM-004 | `NEEDS CLARIFICATION` |
| game | Match **hoặc** sản phẩm/luật chơi | TERM-013 | `CONFLICT` |
| match | Match · Trận | TERM-012 | `CONFIRMED` |
| session | *(chưa có định nghĩa)* | TERM-014 | `NEEDS CLARIFICATION` |
| round | Vòng | TERM-015 | `CONFIRMED` |
| turn | Lượt — **4 nghĩa** | TERM-016 | `CONFLICT` |
| phase | Pha trong câu **hoặc** giai đoạn phát hành | TERM-017 | `CONFLICT` |
| state | Trạng thái trận **hoặc** trạng thái ghế | TERM-022 | `CONFLICT` |
| action | *(chỉ là thuật ngữ UX)* | TERM-028 | `NEEDS CLARIFICATION` |
| event | MatchEvent | TERM-026 | `CONFIRMED` |
| — *(event điểm)* | Độ hạt event điểm | TERM-027 | `CONFLICT` |
| bet | *(không tồn tại)* | Phụ lục A | — |
| balance | *(không tồn tại)* | Phụ lục A | — |
| score | Điểm | TERM-039 | `CONFIRMED` |
| reward | *(không tồn tại)* | Phụ lục A | — |
| payout | *(không tồn tại)* | Phụ lục A | — |
| result | Kết quả — **3 nghĩa** | TERM-044 | `CONFLICT` |
| winner | Người thắng | TERM-045 | `NEEDS CLARIFICATION` |
| loser | *(không tồn tại như khái niệm luật)* | TERM-046 | `NEEDS CLARIFICATION` |
| draw | Rút đề · Bốc thăm · Hoà — **3 nghĩa** | TERM-048 | `CONFLICT` |
| timeout | Hết giờ | TERM-049 | `CONFIRMED` |
| cancelled | Bỏ vòng · Chạy lại · Huỷ cửa sổ · Reject | TERM-038 | `CONFLICT` |
