# Product Discovery — Olympia Contest System

> **Mục đích**: đầu vào cho PRD chính thức (`docs/PRD.md`). Tài liệu này KHÔNG phải PRD, KHÔNG phải spec, KHÔNG chứa technical architecture.
>
> **Ngày lập**: 2026-07-23
>
> **Trạng thái nguồn**: `docs/source/` CHƯA tồn tại. Toàn bộ nhận định dưới đây rút từ **bản nháp** `plans/260711-2340-olympia-contest-system/**` + `CLAUDE.md`. Theo `.specify/memory/constitution.md` v1.2.0, nháp KHÔNG phải nguồn sự thật — mọi mục ở đây là **ứng viên requirement**, phải được chủ dự án xác nhận và ghi vào `docs/source/` trước khi vào spec.
>
> **Quy ước viết tắt nguồn**: `P/` = `plans/260711-2340-olympia-contest-system/`
>
> **Quy ước trạng thái**:
> - `[CHỐT]` — có dấu vết phê duyệt của chủ dự án trong `P/DEFERED.md` (dòng "User chốt:") → coi là đã xác nhận.
> - `[NHÁP]` — chỉ tồn tại trong tài liệu nháp, chưa có dấu vết phê duyệt.
> - `[CONFLICT]` — hai nguồn nói ngược nhau, chưa phân xử.
> - `[NEEDS CLARIFICATION]` — thiếu thông tin quan trọng.
> - `[GIẢ ĐỊNH]` — phát biểu không có bằng chứng trong tài liệu.
> - `[ĐỀ XUẤT]` — đề xuất của tài liệu này, KHÔNG phải requirement từ nguồn.

---

## 1. Problem statement

### 1.1 Vấn đề được nêu trong nguồn

> Nguồn: `P/PRD.md` §1 "Bối cảnh & Vấn đề"

Các trường học/CLB muốn tổ chức thi đấu theo format Đường lên đỉnh Olympia hiện phải dùng **PowerPoint thủ công** hoặc **phần mềm desktop LAN cũ** (bản Athena C#/WPF). Các hạn chế được liệt kê:

| # | Hạn chế | Nguồn xác nhận |
|---|---|---|
| PS-1 | Luật hard-code theo format lỗi thời — đổi luật = sửa code + build lại | `P/PRD.md` §1; `P/research/athena-scout.md` "Pain points" #3 |
| PS-2 | Không chạy online (LAN-only, TCP socket thô port 2644) | `P/PRD.md` §1; `P/research/athena-scout.md` "Kiến trúc cũ" |
| PS-3 | Không có kho đề dùng lại (file `.etai` rời, media theo hashcode trên đĩa) | `P/PRD.md` §1; `P/research/athena-scout.md` "Data & bảo mật đề" |
| PS-4 | Không tích hợp livestream | `P/PRD.md` §1 |
| PS-5 | Mất kết nối là hỏng trận (không reconnect, static TcpClient duy nhất) | `P/PRD.md` §1; `P/research/athena-scout.md` "Pain points" #1 |
| PS-6 | "Bảo mật" đề chỉ là char-shift +1372 (Caesar) — chống mở bằng Notepad | `P/research/athena-scout.md` "Data & bảo mật đề" |
| PS-7 | Chấm điểm hoàn toàn thủ công bằng chuột, không auto-match đáp án | `P/research/athena-scout.md` "Pain points" #6 |
| PS-8 | Global mutable static state → không chạy 2 trận song song | `P/research/athena-scout.md` "Pain points" #2 |

### 1.2 Phát biểu vấn đề cô đọng

> Diễn giải từ `P/PRD.md` §1 — không thêm nội dung mới.

Người tổ chức một trận Olympia ở quy mô trường/CLB **không có công cụ nào vừa (a) cho phép tuỳ biến luật, (b) giữ kín đề tới đúng thời điểm công bố, (c) chạy được realtime công bằng, và (d) lên hình livestream được**. Họ phải chọn giữa PowerPoint thủ công (linh hoạt nhưng không có engine/bảo mật/realtime) và Athena (có engine nhưng luật cứng, LAN-only, đề không bảo mật, sập là hỏng trận).

### 1.3 Vấn đề chưa được chứng minh

- `[GIẢ ĐỊNH]` Không có tài liệu nghiên cứu người dùng nào trong `P/**`. Toàn bộ phát biểu vấn đề rút từ suy luận + đối chiếu Athena. `P/research/product-gaps.md` §6.1 **tự thừa nhận điều này**: *"toàn bộ review đến giờ là AI-review-AI"*, và đề xuất gate "1 giáo viên + 2 học sinh thử demo 30 phút" trước Phase 7-9.
- `[NEEDS CLARIFICATION: Có bao nhiêu trường/CLB thực sự đang chờ sản phẩm này? Ai là người dùng đầu tiên đã cam kết?]` — Không nguồn nào nêu.

---

## 2. Target users & Actors

### 2.1 Actor trực tiếp dùng hệ thống

> Nguồn chính: `P/PRD.md` §3 "Người dùng & Vai trò"; `P/user-stories.md` (nhóm theo role)

| ID | Actor | Là ai | Nhu cầu cốt lõi | Trạng thái |
|---|---|---|---|---|
| A-1 | **Admin / BTC kỹ thuật** | Ban tổ chức, người vận hành trận | Tạo user, dựng contest, điều khiển trận, chấm điểm, can thiệp sự cố | `[CHỐT]` D3 |
| A-2 | **Người ra đề (Setter)** | Giáo viên / ban đề | CRUD câu hỏi của mình, upload media, import/export phần đề phụ trách | `[CHỐT]` `P/PRD.md` §3 |
| A-3 | **Reviewer** | Người duyệt đề | Duyệt DRAFT→ACTIVE, xem đáp án câu đang duyệt | `[CHỐT]` D15.1 |
| A-4 | **Thí sinh (Contestant)** | Học sinh thi đấu | Đăng nhập nhanh, bấm chuông, gõ đáp án, thấy phản hồi tức thì | `[CHỐT]` `P/PRD.md` §3 |
| A-5 | **MC / người dẫn** | Người dẫn chương trình | Màn riêng chữ rất to: câu hỏi + **đáp án** + tóm tắt kết quả | `[CHỐT]` D15.2 |
| A-6 | **Viewer / khán giả** | Khán giả tại chỗ hoặc từ xa | Nhập mã 6 số → xem ngay, không account, không chờ duyệt | `[CHỐT]` D5 (superseded 12/07) |
| A-7 | **Người dựng stream (OBS)** | Máy stream | Overlay 1920×1080 nền trong suốt đè lên khung quay | `[CHỐT]` `P/user-stories.md` US-6.3/6.4 |
| A-8 | **Trainer** | Phụ trách CLB | Tạo practice match + Rematch nhanh | `[CHỐT]` D21.2 — **mốc v1.5** |

### 2.2 Actor gián tiếp / stakeholder

| ID | Stakeholder | Quan tâm | Nguồn |
|---|---|---|---|
| A-9 | **BTC / trọng tài giải** | Công bằng: không rò đề, phân xử "em bấm trước", luật đội rõ ràng | `P/user-stories.md` mục "BTC / TRỌNG TÀI GIẢI" (US-5.6, US-10.4, US-10.5) |
| A-10 | **Đơn vị tự host** | Chịu trách nhiệm pháp lý dữ liệu + bản quyền nội dung âm thanh | `P/PRD.md` FR-7.4; D10 |
| A-11 | **Phụ huynh học sinh** | Consent livestream mặt + tên con | `P/research/product-gaps.md` §1.2; `P/PRD.md` FR-7.3 |
| **A-12** | **BGK — Ban giám khảo** `[NEEDS CLARIFICATION]` | Xuất hiện lần đầu 2026-07-24 trong luồng Câu hỏi phụ (`game-rules-review` Đ-9 phase 7: *"Admin bấm Đúng/Sai **theo hướng dẫn của BTC, BGK, MC**"*). **Chưa có trong bảng vai trò**, chưa rõ khác A-9 (BTC/trọng tài) ở điểm nào. Không thao tác hệ thống (mô hình ba tầng: chỉ admin bấm) nên **không cần role/permission** — nhưng cần xác nhận đây là stakeholder riêng hay trùng A-9. | phát biểu chủ dự án 24/07 |

### 2.3 Vấn đề về mô hình actor

- `[CONFLICT]` **Số role không nhất quán.**
  `P/PRD.md` §3 lập bảng **8 vai trò** (Admin, Setter, Reviewer, Trainer, Contestant, Viewer, MC, OBS Overlay).
  `P/PRD.md` FR-1.2 nói **"5 role mặc định (ADMIN/SETTER/REVIEWER/CONTESTANT + VIEWER guest) là seed"**.
  MC, Trainer, Host là **permission** chứ không phải role seed (`P/PRD.md` FR-5.5 "permission `match.viewAnswer` theo contest"; D21.2 "trainer = role admin gán qua RBAC sẵn có"). Bảng §3 trộn "role hệ thống" với "vai trò vận hành" mà không phân biệt.
  → PRD phải tách rõ 2 khái niệm.
- `[NEEDS CLARIFICATION: Một người có thể vừa là Setter vừa là MC của cùng một contest không? Nếu có, ràng buộc xung đột lợi ích (người ra đề dẫn chính trận dùng đề của mình) xử lý thế nào?]` — Không nguồn nào nêu.

---

## 3. Goals & Non-goals

### 3.1 Goals

> Nguồn: `P/PRD.md` §2 "Mục tiêu"

| ID | Goal | Trạng thái |
|---|---|---|
| G-1 | **Mục tiêu kép**: (a) tổ chức contest chính thức; (b) luyện tập/rehearsal — cùng một engine, phân biệt bằng `matchPurpose` per-match | `[CHỐT]` D21 |
| G-2 | **Nền tảng gameshow tuỳ biến**: round playlist tuỳ ý (số vòng/loại/thứ tự không cứng), 1-12 thí sinh, mọi thời gian/số câu/điểm là config | `[CHỐT]` D1, D8 |
| G-3 | **Nút "Áp dụng luật 2026"** áp preset `O26_DEFAULT@1` theo Fandom wiki | `[CHỐT]` D8 |
| G-4 | **Điểm độc lập thời gian**: `timeSeconds` là metadata TỪNG CÂU, không suy ra từ mức điểm | `[CHỐT]` D8 |
| G-5 | **Kho đề tập trung, bảo mật cao** — đáp án không bao giờ tới client trước công bố; metadata đầy đủ; import/export | `[CHỐT]` `P/PRD.md` §2 mục 2 |
| G-6 | **Thi đấu realtime công bằng**: chuông xếp hạng theo server-timestamp, timer server-authoritative | `[CHỐT]` `P/PRD.md` §2 mục 3 |
| G-7 | **Vận hành trận tin cậy**: hoàn nguyên chấm điểm, phục hồi sau sự cố, audit log phân xử khiếu nại | `[CHỐT]` `P/PRD.md` §2 mục 4 |
| G-8 | **Trình diễn**: viewer animation + overlay OBS + màn MC; theming per contest; âm thanh tuỳ chỉnh mọi thành phần | `[CHỐT]` `P/PRD.md` §2 mục 5 |
| G-9 | **Chuyển trọn gói giữa 2 môi trường**: soạn trên bản Internet → import vào bản portable ngày thi | `[CHỐT]` D23 |
| G-10 | **2 hình thức triển khai**: Docker compose trên server Internet; portable Windows LAN không Docker | `[CHỐT]` `P/PRD.md` NFR-7 |

### 3.2 Non-goals (được tuyên bố rõ)

> Nguồn: `P/PRD.md` §2 "Không-mục-tiêu"; `P/user-stories.md` mục "Ngoài phạm vi"

| ID | Non-goal | Lý do ghi trong nguồn |
|---|---|---|
| NG-1 | Không stream video (chỉ overlay data cho OBS) | `[CHỐT]` — quyết định đã chốt |
| NG-2 | Không giải đấu nhiều trận / bracket tuần→tháng→quý→năm / season leaderboard | P3, `P/research/ux-gaps.md` |
| NG-3 | Không speech-to-text tự chấm câu trả lời miệng | D4 — tiếng Việt + tên riêng + tiếng ồn = sai số cao |
| NG-4 | Không đa ngôn ngữ | D2 — chỉ tiếng Việt, string tách file |
| NG-5 | Không multi-tenant nhiều tổ chức (một deployment = một đơn vị tổ chức) | `P/PRD.md` §2 |
| NG-6 | Không chặn copy đề | P3 — "giá trị thấp, dễ bypass" (`P/research/ux-gaps.md`) |
| NG-7 | Không auto-transcode media ở v1 | D6 — chỉ khuyến nghị trong docs |

### 3.3 Non-goal chưa được quyết

- `[NEEDS CLARIFICATION: Bản quyền FORMAT Olympia (tên chương trình, logo, cấu trúc vòng là tài sản VTV) — sản phẩm có được đặt tên/nhắc "Olympia" trong UI công khai không?]`
  `P/research/product-gaps.md` §1.3 nêu đây là gap **P1**, ghi ⏸️ *"User bỏ mục quyết định (D7 đã xoá 12/07)"*. Thiết kế trung lập vẫn giữ (tên vòng là data trong RuleConfig) nhưng **câu hỏi pháp lý chưa được trả lời, chỉ bị gỡ khỏi sổ quyết định**.
- `[NEEDS CLARIFICATION: License của repo?]` — `P/research/product-gaps.md` §1.4, cũng P1, cũng ⏸️ bị gỡ cùng D7.

---

## 4. User journeys chính

> Suy ra từ `P/user-stories.md` (nhóm theo role) + `P/PRD.md` §4. Mỗi journey ghi actor chính và các US cấu thành.

### J-1. Chuẩn bị kho đề (trước mùa thi)
**Actor**: Setter (A-2) → Reviewer (A-3)
Setter tạo câu hỏi theo 3 pool KV/TT/CN, gắn metadata + media → câu ở trạng thái DRAFT → Reviewer duyệt DRAFT→ACTIVE → câu vào kho dùng được.
**Nguồn**: US-2.1, US-2.2, US-2.3, US-2.5, US-2.7, US-9.1, US-9.2 (`P/user-stories.md`)

### J-2. Dựng contest
**Actor**: Admin (A-1)
Tạo contest → dựng round playlist (thêm/xoá/sắp xếp vòng) hoặc bấm "Áp dụng luật 2026" → config từng vòng → gán 1-12 ghế → **chọn danh sách câu hỏi bằng QuestionPicker (bắt buộc, hệ thống không tự lấy đề)** → tick checkbox "câu này dùng ở phần nào" → đặt theme + sound → pre-flight.
**Nguồn**: US-3.1, US-3.2, US-3.9, US-3.10, US-8.1 → US-8.9

### J-3. Chuyển bundle Internet → portable
**Actor**: Admin (A-1)
Export contest bundle (ZIP: contest.json + questions.xlsx + media theo subfolder vòng + assets + `roster.bin` mã hoá) → mang sang máy portable → import (nhập passphrase nếu có roster) → contest nháp + chạy pre-flight.
**Nguồn**: US-2.8, D23, D26
**Trạng thái**: phần roster `[NEEDS CLARIFICATION]` — xem §6.

### J-4. Ngày thi — vào phòng
**Actor**: Thí sinh (A-4), Viewer (A-6), OBS (A-7)
Admin phát mã phòng 6 số → thí sinh đăng nhập username+password + nhập mã → lobby/tech-check (thử chuông thấy ping ms, thử âm thanh, báo sẵn sàng) → viewer/overlay nhập cùng mã, xem ngay không cần account.
**Nguồn**: US-3.3, US-3.5, US-3.6, US-6.4, US-1.2

### J-5. Thi đấu & điều khiển
**Actor**: Admin (A-1) chính; Thí sinh (A-4), MC (A-5) phụ
Admin điều khiển tuần tự: mở vòng → hiện câu → chạy timer → thí sinh bấm chuông/gõ đáp án → admin chấm Đúng/Sai (hotkey C/X) → câu tiếp. Xen kẽ: chỉnh điểm kèm lý do, hoàn nguyên, skip/thay câu, chỉnh timer, khoá cổng/kick viewer. MC đọc câu hỏi từ màn `/mc` (thấy đáp án).
**Nguồn**: US-4.1 → US-4.8, US-5.1 → US-5.5, US-8.10, US-3.7, US-3.8

### J-6. Xử lý sự cố giữa trận
**Actor**: Admin (A-1), Thí sinh (A-4)
Thí sinh rớt mạng → grace 120s giữ ghế + state-sync; rớt đúng lượt riêng → engine dừng lại chờ admin quyết. Admin rớt → hệ thống **không** tự phản ứng (`Đ-21`); xem **S-17**. Media hỏng → skip/thay câu dự phòng. VCNV cả 4 bị loại → admin bấm nút mở miếng ghép thủ công.
**Nguồn**: US-5.4, US-4.4, US-4.7, D13.1, D13.4, FR-3.5

### J-7. Sau trận
**Actor**: Admin (A-1)
Xuất kết quả PDF (bảng điểm + sự kiện chính + QR verify) → thống kê câu hỏi ghi ngược kho đề → (P2) replay timeline → retention job dọn dữ liệu theo thời hạn.
**Nguồn**: US-7.1, US-7.2, US-7.4, US-10.3, FR-6, FR-7.2

### J-8. Luyện tập CLB *(mốc v1.5)*
**Actor**: Trainer (A-8), Thí sinh (A-4)
Trainer tạo practice match từ đề public → pre-flight chỉ cảnh báo → thí sinh luyện tập solo, tự bấm giờ, thấy đáp án sau chấm → Rematch giữ seats + room code.
**Nguồn**: US-8.11, US-10.1, US-10.2, spec §13

### 4.1 Journey chưa được mô tả

- `[NEEDS CLARIFICATION: Journey "kết quả từ portable quay về central" chưa tồn tại.]` `P/DEFERED.md` D27 nêu vấn đề: trận chạy trên portable sinh ra kết quả + stats, không có đường mang về central → stats mất khi xoá portable, không tổng hợp lịch sử nhiều trận. D27 **chưa chốt**.
- `[NEEDS CLARIFICATION: Journey onboarding admin đầu tiên.]` `P/research/product-gaps.md` §3.3 chỉ nêu giải pháp kỹ thuật (CLI `create-admin`), không mô tả trải nghiệm người dùng lần đầu cài đặt.

---

## 5. Epics

> Gom từ `P/PRD.md` §4 (FR-1…FR-7) + `P/user-stories.md`. Cột "Mốc" theo D18.

| Epic | Tên | FR nguồn | US nguồn | Mốc |
|---|---|---|---|---|
| **E-1** | Auth & phân quyền | FR-1.1 → FR-1.3 | US-1.1, US-1.2, US-1.3, US-1.4, US-1.5, US-2.7 | v1 |
| **E-2** | Kho đề & bộ đề | FR-2.1 → FR-2.4 | US-2.1 → US-2.5, US-9.1 → US-9.5 | v1 |
| **E-3** | Import/Export & contest bundle | FR-2.5, FR-2.6 | US-2.6, US-2.8, US-9.6 | v1 |
| **E-4** | Contest builder & luật tuỳ biến | FR-3.1 | US-3.1, US-3.2, US-3.9, US-3.10, US-8.1 → US-8.7 | v1 |
| **E-5** | Phòng thi & vòng đời trận | FR-3.2 → FR-3.5 | US-3.3 → US-3.8 | v1 |
| **E-6** | Game engine & luật thi đấu | FR-4.1, FR-4.2, FR-4.4, FR-4.6, FR-4.7 | US-4.1, US-5.5, US-5.6, US-8.3 → US-8.7 | v1 |
| **E-7** | Điều khiển & can thiệp của admin | FR-4.3, FR-5.2 | US-4.2 → US-4.8, US-8.12 | v1 |
| **E-8** | Trải nghiệm thí sinh | FR-5.1 | US-5.1 → US-5.4 | v1 |
| **E-9** | Trình diễn: viewer / overlay / MC / theming / âm thanh | FR-4.5, FR-5.3 → FR-5.7 | US-6.1 → US-6.5, US-8.8, US-8.9, US-8.10 | v1 |
| **E-10** | Sau trận: kết quả, thống kê, replay | FR-6.1 → FR-6.3 | US-7.1, US-7.2, US-7.4 | v1 (replay = P2) |
| **E-11** | Dữ liệu cá nhân & privacy | FR-7.1 → FR-7.4 | US-10.3 | v1 (job = v1.5) |
| **E-12** | Vận hành & 2 profile triển khai | NFR-7 | US-7.3 | v1 |
| **E-13** | Practice mode | spec §13 | US-8.11, US-9.4, US-10.1, US-10.2, US-10.3 | **v1.5** |
| **E-14** | Thi đội (Teams) | spec §2b | US-10.4, US-10.5, US-10.6 | **v2** |

### 5.1 Epic không có FR nguồn

- **E-15 `[NHÁP]` Roster trong bundle** — D26 mô tả chi tiết (roster.bin mã hoá AES-256-GCM, passphrase→Argon2id, merge policy username trùng, phiếu tài khoản PDF) nhưng **không có FR nào trong `P/PRD.md` §4 tương ứng**, cũng không có US. Tài liệu ghi trạng thái "🟡 CHỜ CHỐT".
- **E-16 `[NHÁP]` Result bundle chiều ngược** — D27, cùng tình trạng: không FR, không US, "🟡 CHỜ CHỐT".

---

## 6. Requirement mơ hồ hoặc mâu thuẫn

### C-1 `[CHỐT 2026-07-27]` — `revealAnswerAfterJudge` là per-MATCH

> **Phân xử**: cờ này thuộc **match**, lấy mặc định theo `matchPurpose`. Lý do và dẫn chứng: `decisions.md` `QĐ-062`. Phát biểu *"contest bật"* trong `PRD` NFR-4 là **câu chữ lạc hậu, phải sửa**.

**Bối cảnh cũ:**

| Nguồn | Phát biểu |
|---|---|
| `P/PRD.md` NFR-4 | *"**contest** bật `revealAnswerAfterJudge`"* |
| `CLAUDE.md` §Nguyên tắc code | *"config **per-match** theo matchPurpose"* |
| `P/research/ruleconfig-v2-spec.md` §11 | *"`revealAnswerAfterJudge: boolean` (**per-MATCH** — thuộc bundle matchPurpose §13...)"* |
| `P/research/ruleconfig-v2-spec.md` §13 | bảng ghi *"`revealAnswerAfterJudge` (**per-MATCH, không phải per-contest**)"* |

→ Spec nói rõ per-match và **chủ động phủ định** per-contest; PRD nói contest. Ảnh hưởng thật: một contest có thể có cả match official (tắt) lẫn match rehearsal (bật) — nếu là per-contest thì không làm được, phá luôn use-case rehearsal của D21.

### C-2 `[CONFLICT]` — Quy mô viewer: 4 con số khác nhau

| Con số | Nguồn |
|---|---|
| **<50 viewer/trận, ~5 trận song song** | `P/DEFERED.md` D11 `[CHỐT 12/07]`; `P/PRD.md` NFR-2 |
| **<10 viewer thực tế** (load test 30) | `P/DEFERED.md` D19.2 `[CHỐT 12/07]` — cho profile portable |
| **~75 điện thoại viewer** | `P/DEFERED.md` D19.1 phương án (b) |
| **480/500 viewer là điện thoại** | `P/research/product-gaps.md` §4.4; `P/DEFERED.md` D5 phương án (b) |
| **500/trận = "trần kiến trúc, headroom"** | `P/PRD.md` NFR-2 |

→ 75/480/500 là tàn dư trước D11. Ảnh hưởng thật: yêu cầu responsive "đa số viewer là điện thoại" (US-6.2) được biện minh bằng con số 500 đã bị bãi bỏ. Với <50 viewer/trận thì mức đầu tư cho viewer mobile cần định lại.

### C-3 `[CHỐT 2026-07-27]` — ADMIN duyệt đề DRAFT→ACTIVE

> **Phân xử**: vai **admin**, hàng chờ nằm trên dashboard admin; không có vai reviewer riêng. Lý do: `decisions.md` `QĐ-064`.

**Bối cảnh cũ:**

| Nguồn | Phát biểu |
|---|---|
| `P/research/product-gaps.md` §2.1 | *"✅ v1: **chỉ admin activate** (phase-03); 🟡 D15 nếu cần role reviewer riêng"* |
| `P/DEFERED.md` D15.1 `[CHỐT 12/07]` | *"activate gate bằng **permission `question.review`**; seed thêm role **REVIEWER**"* |
| `P/PRD.md` FR-2.1 | theo D15.1 |

→ `product-gaps.md` là bản cũ chưa sync. Cùng loại: §2.2 ghi *"🟡 D15: MC thấy đáp án trước công bố không"* trong khi D15.2 đã chốt **CÓ**.

### C-4 `[CONFLICT]` — D26 vừa "chờ chốt" vừa "đã chốt"

`P/DEFERED.md` D26 tiêu đề ghi **"🟡 CHỜ CHỐT (mở 15/07)"**, nhưng thân mục chứa 3 phát biểu *"user chốt 15/07"*: (a) cách xử lý mật khẩu là setting admin chọn lúc export; (b) thông tin user LUÔN mã hoá thành `roster.bin`; (c) setting (2) mặc định (a) nhưng đổi được. D27 cũng ghi "🟡 CHỜ CHỐT" nhưng không có dấu vết chốt nào.
→ Không xác định được D26 đã chốt phần nào, còn treo phần nào.

### C-5 `[CONFLICT]` — Ngưỡng độ trễ

| Ngưỡng | Nguồn |
|---|---|
| p95 event **<200ms LAN / <500ms Internet** | `P/PRD.md` NFR-1 |
| *"mọi client đổi trạng thái theo lệnh admin **<300ms**"* | `P/user-stories.md` US-4.1 |
| phản hồi cục bộ **<50ms** (optimistic, phía client) | `P/PRD.md` FR-5.1; US-5.1 |

→ 300ms của US-4.1 không nói mạng nào. Nếu là Internet thì nó chặt hơn NFR-1 (500ms); nếu là LAN thì lỏng hơn (200ms). Không nguồn nào giải quyết.

### C-6 `[CONFLICT]` — "Hệ thống trung lập bản quyền" vs. FR chưa có disclaimer

D10 và spec §10 yêu cầu **disclaimer bản quyền khi upload backgroundTrack**. `P/PRD.md` §7 Rủi ro nhắc *"disclaimer khi upload"*. Nhưng **không FR nào trong §4** quy định disclaimer này → nó chỉ tồn tại ở mục Rủi ro và spec, không phải requirement chính thức.

### C-7 `[ĐÃ PHÂN XỬ 2026-07-24]` — Luật cho >4 thí sinh chưa định nghĩa ở cấp sản phẩm

> **Chủ dự án chốt 2026-07-24**: **v1 = ĐÚNG 4 thí sinh** — luật chỉ đặc tả cho cấu hình 4. Luật cho số thí sinh khác 4 để **v2**. **Hạ tầng (schema, seat model, `scoringUnit`, RuleConfig dạng mảng) vẫn hỗ trợ nhiều thí sinh ngay từ v1** — v2 chỉ ship luật và sửa controller, KHÔNG migrate schema.
>
> Hệ quả: bảng bên dưới **không còn chặn v1**; nó trở thành backlog v2. Chi tiết các mục bị hoãn: `docs/reviews/game-rules-review-old.md` §0.1 (GRR-017, GRR-029, GRR-030, GRR-045, GRR-083, GRR-105).
>
> **Vẫn còn treo, KHÔNG được quyết định này cover** (biến thiên theo trục khác, không phải số thí sinh):
> - `[NEEDS CLARIFICATION: v1 có khoá cứng rowCount = 4 không?]` — thang điểm CNV cho `rowCount` 5-8 vẫn không tồn tại trong nguồn nào.
> - `[NEEDS CLARIFICATION: v1 có khoá cứng playlist chuẩn 4 vòng không?]` — playlist tuỳ ý (G-2) cho phép lặp vòng / đổi thứ tự, chưa có luật.
> - `[NEEDS CLARIFICATION: G-2 ghi "1-12 thí sinh" — có sửa thành "v1: 4; v2: 1-12" không?]` — Goal G-2 và D1 hiện vẫn phát biểu 1-12 mà không phân mốc.

Bối cảnh gốc: D1 chốt **1-12 thí sinh**. Nhưng luật O26 (`P/research/rules-2026.md`) được viết cho **đúng 4 người**:

| Vòng | Ràng buộc 4-người | Với 12 người thì sao? |
|---|---|---|
| Tăng tốc | điểm theo hạng **40/30/20/10** = 4 mức | Spec §1.4 nói "mảng độ dài = số đơn vị điểm đã khai báo, runtime dùng prefix" — nhưng **ai điền 12 giá trị đó và theo nguyên tắc nào** thì không nguồn nào nói |
| VCNV | 4 hàng ngang, mỗi TS tối đa 1 lượt chọn | `rowCount` tối đa 8 (spec §5) → **12 thí sinh không đủ lượt chọn hàng ngang**. Không nguồn nào xử lý |
| Về đích | mỗi TS 1 lượt × 3 câu | 12 lượt × 3 câu = 36 câu một vòng. Không nguồn nào bàn thời lượng |
| Cướp về đích | "3 TS còn lại bấm chuông" | Với 12 người là 11 người. Ngưỡng/công bằng không định nghĩa |

Đáng chú ý: D1 **bối cảnh cũ** đã cảnh báo chính xác điều này (*"phá vỡ nhiều giả định luật: thang điểm tăng tốc, thời lượng khởi động, layout"* — lý do bác phương án (c) 2-8 ghế), nhưng user vẫn chốt 1-12 và **cảnh báo đó không được giải quyết ở đâu**.

### C-8 `[MƠ HỒ]` — "Khởi động lượt riêng 3s/câu tính từ lúc MC đọc xong"

`P/research/rules-2026.md` §1 ghi *"3s (từ lúc MC đọc xong)"*. Hệ thống không biết MC đọc xong lúc nào.
`[NEEDS CLARIFICATION: Ai bấm để bắt đầu đếm 3 giây — admin, MC, hay tự động khi hiện câu? Nếu admin bấm thì độ trễ tay người có được trừ vào 3s không?]` Không nguồn nào định nghĩa. Ảnh hưởng trực tiếp tới công bằng vì 3s là rất ngắn.

### C-9 `[CHỐT 2026-07-27]` — Ranh giới Contest vs Match

> **Nguồn**: phát biểu của chủ dự án 2026-07-27 — *"cho phép tạo trận mới khi câu hỏi còn đủ"*. Ghi ở `game-state-machine.md` mã **`Đ-49`**.

**Bối cảnh cũ**: `P/research/red-team.md` M6 ghi *"thêm model `Match` (Contest 1-n)"*. Spec §13 P13 nói *"room code thuộc **contest đang mở** — match FINISHED không giết mã nếu contest còn match khác"*. Nhưng `P/PRD.md` và `P/user-stories.md` gần như luôn nói "contest"/"trận" lẫn lộn (vd US-3.3 "mã phòng để vào đúng phòng", FR-3.2 "pre-flight trước khi start").

**Phân xử**: giữ nguyên ranh giới **Contest = bản thiết kế · Match = một lần chạy**, và **mã phòng thuộc CONTEST**.

| Câu hỏi cũ | Trả lời |
|---|---|
| Một contest có nhiều match trong hoàn cảnh nào? | Bất cứ khi nào **kho đề còn đủ** — thi nhiều trận trên cùng bộ đề, hoặc **chạy lại buổi thi** sau một trận `bỏ dở` (`Đ-47`) |
| Người dùng tạo match ở đâu trong luồng UI? | Nút *"Bắt đầu trận mới"* ở màn trận đã đóng sổ — `EVENT-046`, guard **kho đề còn lại đủ pre-flight** |
| Mã phòng thuộc về đâu? | **Contest** ⇒ viewer/overlay **không phải join lại** giữa hai trận |
| Cái gì reset theo trận? | Điểm · event log · ghế đã gán · biên bản · cấu hình đóng băng (`NT-C` đóng băng lại ở vòng đầu **của trận mới**) |
| Cái gì đi xuyên qua? | Cờ **no-repeat**: câu đã hiển thị **không bao giờ** trả về kho (`INV-11`) |

**Ràng buộc suy ra** `[SUY RA]`: **một contest chỉ có MỘT trận đang chạy tại một thời điểm** — vì mã phòng thuộc contest, hai trận song song sẽ đụng nhau ở cùng một phòng. `EVENT-046` vì vậy đòi trận trước **đã ở `FINISHED`**.

Chi tiết máy trạng thái: `game-state-machine.md` §`EVENT-046`, `T-086`, `T-087`.

### C-10 `[CHỐT 2026-07-27]` — `Question.visibility` là giá trị DẪN XUẤT

> **Phân xử**: `visibility` là **derived, read-only**; setter làm câu thành public bằng cách đưa nó vào bộ đề public. Lý do: `decisions.md` `QĐ-063`.

**Bối cảnh cũ:**

D16 chốt *"thêm sẵn **cột** `visibility: PRIVATE|PUBLIC` vào Question"* (cột set được).
Spec §9 định nghĩa lại: *"`Question.visibility` … **derived, read-only** = PUBLIC nếu câu đang thuộc ≥1 set public, else PRIVATE — **không phải cột set tay** (vá H-v2-10)"*.
→ Spec cố tình sửa D16. Về mặt sản phẩm: setter **không** tự đặt câu là public được — chỉ qua việc đưa câu vào bộ đề public. Điều này chưa phản ánh trong `P/PRD.md` FR-2.2b.

### C-11 `[CHỐT 2026-07-24]` — Quyền mở/đóng hiển thị của admin

> **Nguồn**: phát biểu trực tiếp của chủ dự án, 2026-07-24. **Chưa có file trong `docs/source/`** — phải được ghi vào đó trước khi vào PRD.

Phát biểu nguyên văn:

> **Admin có TOÀN QUYỀN mở và đóng các đáp án, ô chữ.** Tuy nhiên, khi **mở** sẽ có **dialog Yes/No** để tránh bấm nhầm.

Phân loại: đây là **điều khiển hệ thống**, không phải luật game → thuộc **E-7 (Điều khiển & can thiệp của admin)**, không thuộc E-6 (Game engine & luật thi đấu). Vì vậy nó **không** được ghi trong `docs/reviews/game-rules-review-old.md`.

**Quan hệ với nguồn hiện có**:

| Nguồn | Quan hệ |
|---|---|
| J-6 (§4) — *"VCNV cả 4 bị loại → admin bấm nút mở miếng ghép thủ công"* | C-11 **mở rộng**: không chỉ khi cả 4 bị loại, mà là quyền không điều kiện, và áp cho cả **đáp án** chứ không chỉ miếng ghép. |
| `CLAUDE.md` §UX — *"KHÔNG chặn gửi lại… nút action chỉ hiện loading, KHÔNG disable"* | **Không mâu thuẫn nhưng cần ghi rõ**: dialog Yes/No ở đây là chống **bấm nhầm hành động không hoàn tác được** (viewer đã thấy đáp án là không thu hồi được), không phải chống double-submit. `[NEEDS CLARIFICATION: có bổ sung ngoại lệ này vào CLAUDE.md §UX không?]` |
| FR-4.3 / E-7 | Chưa có FR nào quy định quyền này → **cần thêm FR mới** khi viết PRD. |
| `game-rules-review-old.md` GRR-013/014/024/027/028 | C-11 **không đóng** các mục đó: quyền thao tác tay không thay thế *điều kiện luật* để miếng ghép/ô trung tâm được mở. |

**Câu hỏi chưa được trả lời** (không tự suy diễn):

| ID | Câu hỏi |
|---|---|
| Q-A1 | Thao tác **đóng** có dialog xác nhận không, hay chỉ **mở** mới có? |
| ~~Q-A2~~ | ✅ **ĐÃ TRẢ LỜI 2026-07-27 (`Đ-44`): CÓ.** Hàng ngang admin **đánh dấu đã hỏi** bằng tay **có** vào *số hàng ngang đã hỏi* ⇒ băng điểm CNV tụt bậc — lý do là **khôi phục trạng thái**; bước **lộ đáp án** thì không tính. Thao tác **không tự cộng điểm** cho ai; ngoại lệ thì admin **tự cộng tay**. Xem `game-rules.md` §GR-009 C12→C14 · `game-state-machine.md` `Đ-44` |
| Q-A3 | Đóng lại một đáp án/ô chữ đã mở có làm thay đổi **điểm đã ghi nhận** không, hay chỉ đổi hiển thị? |
| Q-A4 | Mở/đóng tay có bị chặn khi trận đã `FINISHED` không? |
| Q-A5 | "Đáp án" ở đây gồm cả đáp án **đẩy tới viewer/thí sinh** không — tức có ghi đè `revealAnswerAfterJudge` (C-1) không? |
| Q-A6 | Có ghi **AuditLog** riêng để phân biệt "mở do admin" với "mở do engine" không? |
| Q-A7 | Ngoài ADMIN, role nào khác có quyền này (MC? Host?) — hay độc quyền ADMIN? |

### C-12 `[CHỐT 2026-07-24]` — Hệ thống không phân xử đáp án, chỉ highlight khác biệt

> **Nguồn**: phát biểu trực tiếp của chủ dự án, 2026-07-24. **Chưa có file trong `docs/source/`.**

Phát biểu nguyên văn:

> **Vì chương trình do ban tổ chức quyết định đúng, sai — nên repo chỉ hiển thị highlight các ký tự khác với đáp án, còn lại do admin tự đánh giá.**

Phân loại: **điều khiển hệ thống + trải nghiệm admin** → **E-7**. Phần luật tương ứng (hệ thống không có nhánh máy nào công nhận/bác bỏ đáp án) ghi tại `docs/reviews/game-rules-review-old.md` §0.3 Đ-1.

**Tác động ở cấp sản phẩm**:

| Mục | Tác động |
|---|---|
| PS-7 (§1.1) — *"Chấm điểm hoàn toàn thủ công, không auto-match đáp án"* | C-12 **giữ nguyên** việc chấm là thủ công; giá trị so với Athena rút về **highlight khác biệt ký tự**, không phải auto-match. Câu chữ mô tả PS-7 trong PRD cần sửa để không hứa auto-match. |
| NG-3 — *"Không speech-to-text tự chấm"* | Cùng hướng: hệ thống không tự chấm ở bất kỳ kênh nào. **Đề xuất bổ sung NG mới**: *"Không tự động chấm đúng/sai kể cả với câu gõ máy"*. |
| G-5 / FR-2 | Trường `acceptedAnswers` chuyển vai trò: từ *dữ liệu để so khớp* → *dữ liệu để đối chiếu và tô màu cho admin*. FR tương ứng cần viết lại. |
| SM-8 (§9.2) | Không đổi — khiếu nại "em bấm trước" vẫn giải bằng log timestamp, không liên quan chấm nội dung. |

**Câu hỏi chưa được trả lời**:

| ID | Câu hỏi |
|---|---|
| Q-B1 | Highlight hiển thị cho **những ai**: chỉ ADMIN, hay cả MC? (Viewer/thí sinh chắc chắn không — đáp án không rời server tới họ.) |
| Q-B2 | Khi câu có **nhiều `acceptedAnswers`**, so sánh với đáp án nào để tô: đáp án đầu, đáp án gần nhất, hay hiển thị song song tất cả? (= Đ-1.a) |
| Q-B3 | So sánh trên chuỗi **nguyên văn** hay **đã normalize**? Tuỳ chọn "bỏ dấu tiếng Việt" còn tồn tại không — và nếu còn thì nó chỉ đổi cách tô màu chứ không đổi phán quyết, đúng không? (= Đ-1.b) |
| Q-B4 | Ở **Tăng tốc** admin phải chấm nhiều bài cùng lúc — highlight hiển thị theo danh sách tất cả thí sinh trên một màn, hay từng người một? |
| Q-B5 | Có giữ hiển thị `wordCount` / "cùng tổng số chữ cái" như gợi ý phụ cho admin không? (= Đ-1.c) |
| Q-B6 | Ở **practice mode không có admin** (C-x / AS-*), câu gõ máy được xử lý ra sao khi không ai chấm? (trùng GRR-070 — nay nặng hơn vì đã bỏ hẳn auto-judge) |

### C-13 `[CHỐT 2026-07-24]` — Hai mode trả lời; mặc định là mode sân khấu

> **Nguồn**: phát biểu trực tiếp của chủ dự án, 2026-07-24. **Chưa có file trong `docs/source/`.**

Phát biểu nguyên văn:

> **Vòng Khởi động, Về đích, Câu hỏi phụ sẽ có 2 mode: mode sân khấu (thí sinh đọc câu trả lời, chỉ dùng máy tính để giành quyền trả lời — đây là mode mặc định) và mode nhập liệu.**
> **Game rule sẽ dùng mode sân khấu.**
>
> *(đính chính cùng ngày)* **Vòng Vượt chướng ngại vật: thí sinh trả lời hoàn toàn bằng máy tính. Khi chọn hàng ngang: mode sân khấu sẽ do admin điều khiển, mode nhập liệu sẽ do thí sinh click chuột hoặc admin điều khiển.**
>
> *(bổ sung cùng ngày)* **Tại VCNV, thí sinh dùng máy tính để chọn "Mở chướng ngại vật".**

Phần **luật** ghi tại `docs/reviews/game-rules-review-old.md` §0.3 Đ-4. Phần dưới đây là tác động **cấp sản phẩm**.

| Mục | Tác động |
|---|---|
| A-4 Thí sinh (§2.1) — *"bấm chuông, gõ đáp án"* | Ở mode mặc định, thí sinh **không gõ**. Mô tả nhu cầu của actor cần sửa. |
| AS-10 (§7) — *"Thí sinh có thiết bị riêng để gõ đáp án"* | **KHÔNG được giảm nhẹ.** Sau đính chính VCNV, **VCNV và Tăng tốc luôn yêu cầu gõ máy** ⇒ mỗi thí sinh vẫn cần một thiết bị nhập liệu đầy đủ cho cả trận. Mode sân khấu chỉ bớt việc gõ ở 3 vòng còn lại, không bớt yêu cầu phần cứng. AS-10 giữ nguyên mức rủi ro. |
| C-12 (highlight khác biệt ký tự) | **Luôn có nghĩa ở VCNV và Tăng tốc** (luôn gõ máy); ở Khởi động / Về đích / Câu hỏi phụ chỉ có nghĩa khi contest đặt mode nhập liệu. Mode sân khấu ở 3 vòng đó admin chấm bằng tai, không có gì để tô. Hai quyết định 24/07 này phải được đọc cùng nhau. |
| E-8 Trải nghiệm thí sinh | UI thí sinh có **hai hình thái** ở 3 vòng (chỉ-chuông vs chuông+ô nhập), **cộng** giao diện lưới ô chữ VCNV có click-chọn-hàng bật/tắt theo mode → ảnh hưởng phạm vi E-8, chưa phản ánh trong US-5.x. |
| E-7 Điều khiển admin | Mode sân khấu ở VCNV **thêm một thao tác admin mới**: chọn hàng ngang thay thí sinh, kèm dialog Yes/No xác nhận (Đ-4.2). Chưa có FR/US nào cho thao tác này. |
| **Nguyên tắc chống-bấm-nhầm** | Chốt 24/07 (Đ-4.3): bước xác nhận chống bấm nhầm **chỉ dành cho ADMIN**; thao tác của **thí sinh không bao giờ có xác nhận** — *"thí sinh cần tốc độ, và tự chịu trách nhiệm sai lầm của mình"*. Đây là cách phân xử rõ ràng cho ngoại lệ của `CLAUDE.md` §UX đã nêu ở C-11 → **nên ghi thẳng vào `CLAUDE.md`**. |
| J-5 (§4) | Luồng điều khiển của admin khác nhau giữa 2 mode (có/không màn đối chiếu đáp án) — journey cần tách nhánh. |
| E-13 Practice (v1.5) | Practice solo không có admin **và** mode mặc định không có dữ liệu để máy đối chiếu → practice khả thi tới đâu là câu hỏi mở (trùng Q-B6). |

**Câu hỏi chưa được trả lời**:

| ID | Câu hỏi |
|---|---|
| Q-C1 | Mode là cấu hình ở cấp nào — contest, match, hay từng vòng trong playlist? Ai đổi được và tới thời điểm nào (trước start, hay giữa trận)? |
| Q-C2 | Mode có nằm trong preset `O26_DEFAULT@1` không, và nút "Áp dụng luật 2026" đặt mode nào? |
| Q-C3 | Mode có được ghi vào **contest bundle** export/import (D23) không? |
| Q-C4 | Ở mode sân khấu, kết quả PDF sau trận (US-7.1) có ghi **nội dung đáp án** của thí sinh không, hay chỉ ghi Đúng/Sai? (= Đ-4.c) |
| Q-C5 | Tăng tốc có mode không, hay luôn nhập liệu? (= Đ-4.b) |

### C-14 `[CHỐT 2026-07-24]` — v1 luôn có người điều khiển; mode cấu hình ở cấp contest

> **Nguồn**: phát biểu trực tiếp của chủ dự án, 2026-07-24. **Chưa có file trong `docs/source/`.**

Hai phát biểu nguyên văn:

> **Toàn bộ version 1 đều phải có người điều khiển.**
> **Cấu hình [mode trả lời] ở cấp contest.**

#### (a) v1 luôn có người điều khiển

Không tồn tại luồng v1 nào chạy không có admin. Hệ quả:

| Mục | Tác động |
|---|---|
| C-12 Q-B6 / Đ-4.h | **ĐÓNG cho v1** — câu hỏi "ai chấm khi không có admin" chỉ còn là vấn đề của **practice (v1.5)**. GRR-070 chuyển sang v1.5. |
| E-13 Practice (v1.5) | Trở thành nơi tập trung toàn bộ rủi ro "không người chấm". Khi mở v1.5, phải quyết: practice chỉ dùng mode nhập liệu? có `autoJudge` riêng cho practice? — chưa quyết. |
| AS-4 (§7) — *"1 admin đủ vận hành 1 trận"* | **Chưa được đóng.** C-14 nói *có* người điều khiển, không nói *bao nhiêu* người. `product-gaps.md` §2.4 vẫn giảm thiểu bằng "runbook crew ≥2". `[NEEDS CLARIFICATION: v1 giả định 1 người hay ≥2 người vận hành?]` |
| GRR-027, GRR-062, ~~GRR-077~~ (`game-rules-review`) | **KHÔNG đóng** — trừ `GRR-077`, ✅ **đã đóng 2026-07-27** (`Đ-38` + `Đ-47`). "Có người điều khiển" ≠ "người đó luôn bấm". Hai mục còn lại (mở Chướng ngại vật, xác nhận bốc thăm) vẫn không có đường thoát nếu admin không thao tác. Riêng *"kết thúc trận dở"* nay có: **"Huỷ trận"** → `FINISHED` + nhãn `bỏ dở` (`EVENT-044`). |

#### (b) Mode trả lời cấu hình ở cấp contest

Trả lời Đ-4.a / Q-C1. Hệ quả và điểm còn hở:

| ID | Câu hỏi còn lại |
|---|---|
| ~~Q-C1a~~ | **ĐÃ CHỐT 2026-07-24: mode là MỘT giá trị chung cho toàn bộ vòng.** Một contest không trộn hai mode. |
| Q-C1d | **Contest settings** nay chứa thêm (sau rà soát nguồn — `game-rules-review` Đ-4.5): (1) **mode trả lời** — một giá trị chung; (2) **vị trí** của từng thí sinh — dùng cho thứ tự lượt VCNV *và* tie-break Về đích; (3) **thứ tự lượt riêng Khởi động** — chỗ duy nhất luật im lặng. **KHÔNG** thêm trường "thứ tự lượt VCNV" riêng: luật buộc theo vị trí, thêm trường sẽ tạo hai nguồn sự thật. Chưa có FR nào liệt kê danh sách trường contest settings. |
| Q-C1b | Sửa mode **sau khi contest đã có match FINISHED** thì sao — chặn, hay cho sửa và các trận đã chạy giữ mode cũ? (cùng họ với GRR-104: đổi cấu hình giữa contest) |
| Q-C1c | Mode ở cấp contest **không phụ thuộc `matchPurpose`** — tức một contest chứa cả match official lẫn rehearsal thì cả hai dùng chung mode, đúng không? (đối chiếu C-1, nơi `revealAnswerAfterJudge` được chốt là **per-match** chính vì lý do ngược lại) |

Q-C2 (mode có nằm trong preset `O26_DEFAULT@1` không) và Q-C3 (mode có vào contest bundle không) **vẫn treo**.

### C-15 `[CHỐT 2026-07-24]` — Hệ thống advisory: admin điều khiển vòng và lượt, máy chỉ khuyến nghị

> **Nguồn**: phát biểu trực tiếp của chủ dự án, 2026-07-24, tham chiếu mô hình tương tác của bản cũ `Athena-Intelligent-Olympia`. **Chưa có file trong `docs/source/`.**

> 1. **Admin là người chọn vòng nào sẽ bắt đầu.**
> 2. **Admin là người chọn đây là lượt của ai** — hệ thống chỉ hiển thị **recommendation dựa trên luật**.
> Mục đích: dễ đổi thứ tự vòng thi khi có sự cố. Nếu conflict luật (một người thi 2 lần, đổi lượt) → **dialog cảnh báo**, admin bấm **Yes/No**.

Phần luật + rà soát "lượt thi" theo nguồn: `docs/reviews/game-rules-review-old.md` §0.3 Đ-5.

**Đây là mảnh thứ ba của cùng một triết lý sản phẩm** — hệ thống **advisory**, người vận hành là quyền lực cuối cùng:

| Quyết định | Nội dung |
|---|---|
| [đã có, 23/07] | Máy không bao giờ tự chấm Đúng/Sai |
| C-11 (24/07) | Admin toàn quyền mở/đóng đáp án, ô chữ |
| C-12 (24/07) | Máy không phân xử đáp án, chỉ highlight khác biệt |
| **C-15 (24/07)** | **Máy không cưỡng chế thứ tự vòng/lượt, chỉ khuyến nghị + cảnh báo** |

→ **Nên phát biểu thành một nguyên tắc sản phẩm cấp cao trong PRD**, thay vì 4 quyết định rời rạc. Đề xuất tên: *"Server-authoritative về SỰ KIỆN, admin-authoritative về PHÁN QUYẾT"* — máy giữ độc quyền timestamp/thứ tự chuông/timer (không ai sửa được), người giữ độc quyền mọi phán quyết game.

**Tác động:**

| Mục | Tác động |
|---|---|
| E-7 Điều khiển admin | **Phình đáng kể**: thêm chọn vòng bắt đầu, chọn lượt, engine tính recommendation, hệ thống phát hiện conflict + dialog cảnh báo. Chưa có FR/US nào cho nhóm này. |
| PS-1 (§1.1) — *"Luật hard-code theo format lỗi thời"* | C-15 giải PS-1 theo hướng **khác** lời hứa ban đầu: không phải "engine chạy đúng luật tuỳ biến" mà "engine gợi ý theo luật, người quyết". Câu chữ PRD cần sửa cho khớp. |
| AS-4 — *"1 admin đủ vận hành 1 trận"* | **Rủi ro tăng.** Admin nay gánh thêm: chọn vòng, chọn lượt, đọc và phán quyết mọi dialog cảnh báo — cộng dồn với chấm Đúng/Sai và mở/đóng hiển thị. Giả định 1 người càng khó đứng vững. |
| SM-5 (§9.2) — *"số lần admin can thiệp ngoài luật ≤3/trận"* | Metric này **mất ý nghĩa**: can thiệp nay là cơ chế vận hành bình thường, không phải dấu hiệu bất thường. Cần định nghĩa lại (vd đếm riêng số lần **override qua cảnh báo**). |
| GRR-078 / playlist | Thứ tự playlist trở thành **gợi ý**, không phải ràng buộc. |

**Bổ sung cùng ngày** — admin được **bỏ hẳn một vòng** và **chạy lại một vòng**, với điều kiện **ngân hàng đề của contest còn câu hỏi** (chi tiết + 9 câu hỏi treo: `game-rules-review` Đ-5.1). Tác động cấp sản phẩm:

| Mục | Tác động |
|---|---|
| AS-3 (§7) — *"admin chấp nhận chọn thủ công toàn bộ đề trước mỗi trận"* | **Rủi ro tăng rõ rệt.** Chạy lại vòng rút câu MỚI ⇒ danh sách câu gán cho contest phải **dư** đủ để chịu được vài lần chạy lại. Chưa nguồn nào ước lượng con số này (SM-3 vốn để đo). |
| GRR-084 (pool cạn giữa trận) | **Nặng hơn** — nay có một cơ chế tiêu pool nhanh mà trước không có. |
| E-3 / D23 contest bundle | Bundle mang sang máy portable phải chứa **đủ đề dự phòng cho kịch bản chạy lại**, không chỉ đủ cho một lượt chạy. |
| SM-4 (§9.2) — tỉ lệ tái sử dụng câu hỏi ≥30% | Không đổi ở phạm vi nhiều trận, nhưng **trong một contest** no-repeat là tuyệt đối (xác nhận qua Đ-5.1). |
| E-10 Sau trận / US-7.1 PDF | Chốt 24/07 (Đ-5.2): **bỏ vòng thì điểm reset về trước đó, nhưng "biên bản trận đấu vẫn phải đầy đủ thông tin"** ⇒ PDF kết quả và event log phải thể hiện được **cả những vòng đã bị bỏ**. Chưa có FR/US nào mô tả cách trình bày này. |
| E-9 Trình diễn (viewer/overlay) | Điểm **quay lui trước mắt khán giả** là tình huống trình diễn chưa từng được mô tả trong US-6.x — cần quyết cách hiển thị (Đ-5.2e). |

**Câu hỏi chưa được trả lời**: xem Đ-5.a → Đ-5.g và Đ-5.1a → Đ-5.1i ở `game-rules-review`. Đáng chú ý nhất ở cấp sản phẩm:

- **Đ-5.b** — chưa có **danh sách các loại conflict được cảnh báo**; không có danh sách thì tính năng cảnh báo chưa kiểm chứng được.
- **Đ-5.f** — recommendation hiển thị cho ai (lộ thứ tự sắp tới cho viewer là vấn đề trình diễn).
- **Đ-5.g** — admin chọn vòng bắt đầu ⇒ **pre-flight** chạy một lần cho cả playlist hay theo từng vòng.

### C-16 `[NEEDS CLARIFICATION]` — Khoảng trống điều khiển hệ thống phát sinh từ các quyết định 24/07

> Nguồn: rà soát lượt 2 của `docs/reviews/game-rules-review-old.md` (2026-07-24). Các mục dưới đây là **điều khiển hệ thống**, không phải luật game, nên **không** ghi ở file luật.

| ID | Vấn đề | Epic |
|---|---|---|
| S-1 | **Ai** có quyền bỏ vòng / chạy lại vòng / override cảnh báo? Đ-5 và Đ-5.1 nói "admin" nhưng không gắn permission cụ thể trong catalog CASL. Đây là các thao tác phá huỷ nhất trong hệ thống. | E-1, E-7 |
| ~~S-13~~ | ✅ **ĐÃ CHỐT 2026-07-24: nhánh (A) — MC quyết bằng lời, admin bấm.** `/mc` **giữ nguyên read-only**; không permission CASL mới, không đổi mô hình truy cập, không phát sinh bề mặt quyền mới. **Mô hình ba tầng**: MC = thẩm quyền phán quyết trên sân khấu (nói) · Admin = cảm biến + cơ cấu chấp hành duy nhất (bấm) · Server = sự kiện và thời gian (không ai sửa). | E-1, E-7, E-9 |
| ~~S-14~~ | ✅ **ĐÃ CHỐT 2026-07-24: KHÔNG** — AuditLog **không** có trường "người yêu cầu" tách khỏi "người thao tác". Mọi event giữ một `actor` duy nhất = người bấm (admin). ⇒ **Hệ thống KHÔNG phải là hồ sơ duy nhất của trận**: log chịu trách nhiệm về **thời gian và thao tác**; **việc AI quyết định được ghi ở BIÊN BẢN VIẾT TAY** ngoài hệ thống (chốt 24/07). SM-8 đã sửa theo. Xem thêm **S-15**. | E-11 |
| **S-15** | ⚠️ **Biên bản viết tay là một PHẦN CHÍNH THỨC của hồ sơ trận, nhưng chưa xuất hiện ở bất kỳ tài liệu nào.** Chốt 24/07: xuất xứ quyết định (MC quyết gì, vì sao) nằm ở biên bản viết tay của BTC, không nằm trong hệ thống. Đây là **phụ thuộc vận hành mới** — nếu BTC không lập biên bản thì khiếu nại về "ai quyết" **không có nguồn nào** phân xử. Câu hỏi: **(a)** đây là giả định vận hành hay yêu cầu quy trình bắt buộc? **(b)** PDF kết quả (US-7.1) có cần in kèm phần trống để ghi tay / chỗ ký xác nhận không? **(c)** có cần mẫu biên bản chuẩn trong tài liệu triển khai không? | E-10, E-12 |
| S-2 | **Revert có bắt nhập lý do** như `SCORE_ADJUST` (`R-GEN-07`: *"bắt buộc nhập lý do"*) không? Có audit entry riêng cho revert / bỏ vòng / chạy lại không? | E-7, E-11 |
| S-3 | **Dialog cảnh báo conflict** (Đ-5): nội dung hiển thị gì, có bắt nhập lý do, có timeout tự đóng không? | E-7 |
| S-4 | **Recommendation hiển thị cho ai** (Đ-5.f) — admin, MC, viewer? Hiển thị cho viewer là lộ thứ tự sắp tới. | E-7, E-9 |
| S-5 | **`sound-cue` slot cho các sự kiện mới**: revert, bỏ vòng, chạy lại, override. Theo D10 slot trống = silent, nhưng danh sách slot chưa có các sự kiện này. | E-9 |
| S-6 | **Trình bày biên bản/PDF**: Đ-5.2d chốt *"giữ như một vòng đã chạy kèm nhãn đã bỏ"* — nhưng chưa nói lần chạy cũ vs mới phân biệt thế nào, event revert đánh dấu ra sao. | E-10 |
| S-7 | **Hành vi `Esc` / hotkey trên màn thí sinh ở mode sân khấu**: `CLAUDE.md` quy định *"Esc CHỈ xoá ô nhập, không back"* — nhưng mode sân khấu **không có ô nhập** ở 3 vòng. Esc làm gì? | E-8 |
| S-8 | Admin có thấy **lịch sử submission** trước đó để phân xử không (U-31)? Nay quan trọng hơn vì Đ-1 giao toàn quyền chấm cho admin. | E-7 |
| S-9 | **Giao diện highlight ký tự** (Đ-1 / C-12) khi có **nhiều `acceptedAnswers`** — hiển thị song song hay chọn một. | E-7 |
| S-10 | **Quy trình retention vs biên bản**: có bắt buộc xuất/lưu trữ biên bản trước khi job dọn dữ liệu chạy không? | E-11, E-12 |
| S-11 | **Viewer đang xem thấy trận "quay ngược"** khi admin chạy lại vòng — có khoá cổng phòng / thông báo gì không? (Đ-5.2e đã chốt điểm đổi đột ngột, nhưng chưa nói về việc cả một vòng chạy lại) | E-9 |
| ~~S-12~~ | ✅ **ĐÃ CHỐT 2026-07-25 (`Đ-31`): theo VÒNG** — pre-flight chạy tại cửa vào mỗi vòng, không phải một lần trước trận. Thiếu đề thì không mở được vòng đó, các vòng khác vẫn chạy. | E-5 |

**Đáng chú ý nhất: S-1.** Bỏ vòng, chạy lại vòng và override cảnh báo là ba thao tác có sức phá huỷ lớn nhất trong hệ thống, nhưng chưa mục nào gắn chúng vào permission catalog. Với `CLAUDE.md` §Zero-trust ("mọi request verify auth + permission ở server"), đây là khoảng trống bảo mật, không chỉ khoảng trống tài liệu.

---

### C-17 `[CHỐT 2026-07-25]` — Sáu quyết định về hành vi giao diện và mô hình điều khiển

> Phần **luật** của cùng bộ quyết định này ghi ở `docs/reviews/game-rules-decisions.md` §11, mã **Đ-16 → Đ-36**. Mục này chỉ ghi **hệ quả cấp sản phẩm**: giao diện phải làm gì, epic nào chịu ảnh hưởng, khoảng trống nào mới mở ra.

| Mã | Quyết định | Hệ quả cấp sản phẩm | Epic |
|---|---|---|---|
| **Đ-16** | Thao tác ở **invalid state** không thực hiện được | **Hai hành vi UI khác nhau theo vai**: máy thí sinh **không render** control và bấm **không phản hồi**; máy admin render nhưng bấm ra **toast** báo không hợp lệ. ⇒ Cần một quy ước toast riêng, **phân biệt với dialog cảnh báo conflict luật** (S-3) — hai thứ trông giống nhau với người dùng nhưng một cái ép được, một cái không | E-7, E-8 |
| **Đ-17** | Một câu = **một phán quyết**; nút chấm khoá sau khi bấm, nút "Câu kế tiếp" hiện lên | Luồng chấm là **một chiều, không quay lại**: admin không thể bấm nhầm, bấm lại hay đổi phán quyết tại chỗ; cũng không thể vô tình bỏ sót câu. **Bỏ được** nhánh UI "câu chưa chấm khi đóng vòng". Đổi lại: **sửa sai phải đi đường vòng** qua hoàn nguyên event hoặc điều chỉnh thủ công — cần thiết kế lối vào cho hai thao tác đó ngay trên màn điều khiển, nếu không admin sẽ bị kẹt khi MC đổi ý ngay sau khi bấm | E-7, E-10 |
| **Đ-18** | Mỗi contest chỉ có **một admin duy nhất** | **Ràng buộc mô hình truy cập, không chỉ là ràng buộc luật.** Cần chốt: một tài khoản admin gắn cứng theo contest, hay nhiều tài khoản nhưng chỉ một phiên điều khiển tại một thời điểm? Ảnh hưởng trực tiếp permission catalog ở **S-1** | E-1, E-7 |
| **Đ-19** | "Trả lời sai" và "không trả lời" là **cùng một thao tác** | UI chấm chỉ cần **hai nút** (Đúng / Sai), không cần nút thứ ba kiểu "không trả lời". Biên bản vì vậy **không phân biệt** được hai tình huống — nếu BTC cần phân biệt thì phải ghi ở biên bản viết tay (**S-15**) | E-7, E-10 |
| **Đ-20** | Đồng hồ **khoá thí sinh, không khoá admin**; nút start timer tự khoá sau lần bấm đầu | Quy tắc render theo vai: control của thí sinh gắn vòng đời cửa sổ thời gian; control của admin thì không. Ngoại lệ **start timer** là chỗ **duy nhất** được disable nút phía admin — cần ghi rõ trong hướng dẫn UI để không bị nhầm là vi phạm `CLAUDE.md` §UX (*"không disable nút để chống double-submit"*), vì đây là **chống bấm trùng tạo mốc thời gian**, không phải chống double-submit | E-7, E-8 |
| **Đ-28** | **Biên thời gian là biên ĐÓNG**; bản quá hạn **chỉ bị đánh dấu đỏ**, admin quyết cả **hiển thị** lẫn **chấm điểm** | Màn admin cần **hai lớp tách bạch**: lịch sử nội bộ (thấy cả bản quá hạn, tô đỏ) và nội dung đẩy ra màn thí sinh/viewer (do admin bấm). Nếu gộp một lớp thì hoặc admin mất dữ liệu phân xử, hoặc bản quá hạn lộ ra ngoài ngoài ý muốn. **Nút chấm phải bật được cho cả bản tô đỏ** — nếu UI khoá cứng bản quá hạn thì admin mất quyền phán quyết mà `Đ-1` giao cho | E-7, E-8, E-9 |
| **Đ-33** | Mốc **"MC công bố đáp án"** là một **thao tác bấm của admin** | Màn admin có thêm một nút mốc nữa, nối vào chuỗi đã có (hiển thị câu → start timer → công bố đáp án). Cả ba đều là mốc server timestamp, cần cùng một mẫu hiển thị và cùng vào audit log | E-7 |
| **Đ-35** | **Phán quyết là quyết định cuối cùng**; nút chấm khoá tới hết giờ ở vòng gõ máy | Màn admin có **hai chế độ theo kênh trả lời của vòng**: vòng nói thì nút chấm sống suốt, vòng gõ thì nút chấm **mờ tới khi đồng hồ về 0**. Cần chỉ báo rõ *vì sao* nút đang mờ, nếu không admin sẽ tưởng hệ thống treo giữa trận | E-7, E-8 |
| **Đ-34** | Phán quyết **chỉ nhị phân khi "Sai" trừ 0 điểm** | Màn chấm **không dùng chung một layout hai nút** cho mọi vòng: vòng có hình phạt cần nút thứ ba **"Huỷ kết quả"**. Số nút phụ thuộc **vòng đang chạy** và **có bản quá hạn hay không** — nên đây là component có trạng thái, không phải hai nút cố định | E-7, E-8 |
| **Đ-32** | **Luôn ghi nhận đáp án cuối cùng**; bản quá hạn giữ lại cho admin phán quyết | Nút gửi và nút chuông có **vòng đời ngược nhau** — tách thành hai component, đừng dùng chung mẫu "bấm xong thì tắt". Màn thí sinh cần cho thấy **bản đang được ghi nhận**. Màn admin phải hiển thị **bản hợp lệ và bản quá hạn CẠNH NHAU**, bản quá hạn tô đỏ. **Thao tác chấm không còn là nhị phân**: khi chỉ có bản quá hạn, cần nút thứ ba **"Huỷ kết quả"** — mọi màn chấm và mọi mô tả "hai nút Đúng/Sai" phải rà lại | E-7, E-8 |
| **Đ-31** | **Kho đề kiểm tại cửa vào từng vòng**; thiếu thì không mở được vòng đó | Pre-flight chuyển từ **một lần trước trận** sang **mỗi lần mở vòng** — đổi cả thời điểm chạy lẫn thông điệp hiển thị (phải nói rõ *vòng nào* thiếu *bao nhiêu* câu). Màn điều khiển cần thể hiện **vòng nào mở được, vòng nào không**, vì trận vẫn chạy tiếp với các vòng còn đủ đề. Đây là **ngưỡng chặn cứng thứ ba** của hệ thống — cần vào cùng danh sách với hai ngưỡng số người | E-5, E-7 |
| **Đ-30** | **Luật cho bao nhiêu câu thì đúng bấy nhiêu** — không có câu thứ N+1 | Số câu là **ràng buộc dựng sẵn của luồng**, không phải một cảnh báo lúc chạy. Bỏ được toàn bộ nhánh UI "admin cố hỏi thêm câu" và dialog cảnh báo đi kèm. Kéo theo yêu cầu cho pre-flight: **kho đề phải đủ đúng số câu của từng vòng** trước khi start, vì lúc chạy không có đường xin thêm | E-5, E-7 |
| **Đ-29** | **Nút thao tác một chiều tự tắt sau khi bấm** | Một mẫu UI dùng chung cho 4 nút (chấm · start timer · chuông · chuyển câu): bấm xong thì tắt, bước kế tiếp bật lên. Nên làm thành **một component chung** thay vì lặp logic ở từng màn — và ghi rõ đây là **khoá-theo-luật-chơi**, không phải chống double-submit, để không bị gỡ nhầm khi rà quy tắc UX | E-7, E-8 |
| **Đ-27** | **Chấm xong là kết thúc câu**; xoá hàng đợi đang hoạt động, gỡ khoá chuông | Vòng đời hàng đợi gắn với **một câu**, không phải cả vòng. Màn admin cần phân biệt rõ **hàng đợi đang hoạt động** (xoá theo câu) với **lịch sử tín hiệu** (giữ vĩnh viễn) — nếu UI trộn hai thứ, admin sẽ tưởng đã mất căn cứ xử lý khiếu nại của `Đ-23` và `Đ-25` | E-7, E-11 |
| **Đ-26** | **"Hiển thị câu hỏi"** và **"start timer"** là hai nút riêng, thứ tự cố định | Màn admin có **hai nút tuần tự** cho mỗi câu, nút sau chỉ bật khi nút trước đã bấm. Xác định luôn vòng đời control của thí sinh: chuông sống từ mốc hiển thị, ô nhập sống từ mốc start timer. Cần nêu rõ trong hướng dẫn UI rằng **gộp hai nút là phá luật** — cửa sổ chuông sẽ co lại đúng bằng thời gian suy nghĩ, và cửa sổ Ngôi sao hy vọng mất mốc đóng | E-7, E-8 |
| **Đ-25** | Tín hiệu đến **sau khi đã có người giành quyền**: ghi nhận nhưng **trơ** | Màn admin cần hiển thị hàng đợi **đầy đủ, kèm timestamp và trạng thái "không có hiệu lực"** — nếu chỉ hiện người thắng thì mất luôn căn cứ mà quyết định này sinh ra. Đây cũng là dữ liệu để trả lời khiếu nại ở **Đ-23** (hai chuông cùng mốc) | E-7, E-11 |
| **Đ-24** | **Nút chuông tự khoá ngay khi bấm** (frontend, trước khi gửi) | Quy tắc render nút chuông: khoá đồng bộ trong handler, mở lại khi sang câu mới. Cần ghi rõ trong hướng dẫn UI đây là **khoá-theo-luật-chơi**, không phải vi phạm quy tắc "không disable nút" — nếu không, người implement sau sẽ gỡ mất. Áp cho cả nút "Mở chướng ngại vật"; **không** áp cho click chọn hàng ngang | E-7, E-8 |
| **Đ-23** | Hai tín hiệu **cùng mốc thời gian** ⇒ hàng đợi tự quyết định, **ngẫu nhiên** | Bỏ được nhu cầu thiết kế quy tắc ưu tiên (theo ghế/vị trí) và giao diện giải thích nó. Đổi lại: khi có khiếu nại *"vì sao là bạn ấy chứ không phải tôi"*, câu trả lời **chỉ có thể** là "hàng đợi ghi nhận thế" ⇒ màn admin cần **xem lại được thứ tự hàng đợi kèm timestamp** để đối chất, và BTC nên biết trước rằng hệ thống không có tiêu chí ưu tiên nào | E-7, E-11 |
| **Đ-22** | Tín hiệu thí sinh **không làm gián đoạn đồng hồ**; hoãn **hiển thị** chứ không hoãn thời gian | Màn admin cần **nút hiển thị đáp án** tách khỏi sự kiện hết giờ — hết giờ chỉ khoá ô nhập của thí sinh, không tự bung gì cả. Màn thí sinh và viewer phải ẩn **hai** thứ cho tới khi admin bấm: **đáp án chuẩn của chương trình** và **bài làm của các thí sinh khác** — kể cả khi đồng hồ đã về 0. **Tiền lệ Athena**: `ShowAnswer_Click` là nút riêng cho bài làm; đáp án chuẩn thì ẩn theo vai ngay ở tầng hiển thị (`ObstacleUI.cs` dòng 104: ghế thí sinh `Collapsed`, chỉ màn MC thấy) | E-7, E-8, E-9 |
| **Đ-21** | **Đồng hồ chạy liên tục**; trận dừng bằng cách admin ngừng thao tác | **Bỏ khỏi phạm vi sản phẩm**: nút dừng/tiếp, banner báo gián đoạn cho viewer, cơ chế tự dừng khi admin mất kết nối, và mọi cấu hình ngưỡng đi kèm. Viewer **không** có tín hiệu nào cho biết trận đang gián đoạn | E-7, E-9 |
| **Đ-36** | **Chọn hàng ngang: một đường vào mỗi mode**; mode nhập liệu dedup bằng **dialog phía thí sinh** + khoá tạm | **Phá vỡ một quy ước UI tưởng là tuyệt đối**: `CLAUDE.md` §UX ghi *"dialog xác nhận KHÔNG BAO GIỜ ở phía thí sinh"*, nay có đúng **một** ngoại lệ. Cần ghi tiêu chí vào hướng dẫn UI — **đua tốc độ thì không dialog, không đua tốc độ thì có** — nếu không, người implement sau hoặc gỡ mất dialog này, hoặc nhân bản nó sang chuông. Kéo theo: màn VCNV của thí sinh **khác nhau theo mode** (sân khấu: không render nút chọn; nhập liệu: render + dialog), nên đây là **hai layout**, không phải một layout bật/tắt một nút. Nút chọn cần **ba trạng thái** (sống · khoá chờ duyệt · mở lại sau khi admin từ chối) — trạng thái thứ ba dễ bị bỏ sót và sẽ khiến thí sinh mất lượt trái luật | E-7, E-8 |

**Khoảng trống mới phát sinh — cần quyết:**

| ID | Vấn đề | Epic |
|---|---|---|
| **S-16** | **Đ-18 chưa nói "một admin" là ràng buộc ở tầng nào**: một tài khoản duy nhất cho mỗi contest, hay nhiều tài khoản nhưng khoá còn một phiên điều khiển? Hai cách cho hai thiết kế permission khác nhau, và ảnh hưởng luôn kịch bản admin đổi máy giữa trận | E-1, E-7 |
| **S-17** | **Không có phương án dự phòng khi admin mất kết nối giữa trận** (`Đ-21`): trận cứ chạy, thí sinh vẫn bị đồng hồ khoá. Van thoát duy nhất là bỏ / chạy lại vòng (`Đ-5.1`), mà cách đó **tiêu đề** (câu đã dùng không trả lại kho). Có cần quy trình vận hành riêng cho tình huống này không? | E-7, E-12 |
| **S-19** | **Athena tự khoá ô nhập hàng ngang của chính người vừa bấm "Mở chướng ngại vật"** (`ObstacleUI.cs` `Obstacle_Click`) — coi như người đó đã dồn hết vào CNV. `Đ-22` **không nói** tới điểm này. Người bấm CNV, trong lúc chờ admin duyệt, **có còn được trả lời hàng ngang không**? Luật gốc chỉ loại họ khi trả lời **sai** CNV, nên khoá ngay lúc bấm là **nghiêm hơn luật** | E-7, E-8 |
| **S-18** | **Đ-16 cần bộ thông điệp toast chuẩn** — mỗi loại invalid state nói gì. Nếu chỉ hiện một câu chung chung thì admin không biết vì sao thao tác bị chặn, giữa lúc đang phải xử lý nhanh | E-7, E-8 |
| **S-20** | **Đ-36 chưa nói nút chọn hàng ngang có hiện trên máy thí sinh CHƯA TỚI LƯỢT không** (mode nhập liệu). `Đ-16` (invalid state → máy thí sinh không render gì) đẩy về "không hiện" ⇒ tín hiệu sai lượt **không tồn tại**; còn `Đ-5` (cảnh báo, không chặn cứng) giả định tín hiệu sai lượt **tới được** server và admin ép được. Hai rule cho hai thiết kế màn thí sinh khác nhau, và quyết định luôn một hàng trong bảng quyết định GR-007 | E-7, E-8 |

---

### C-18 `[CHỐT 2026-07-27]` — Admin sửa danh sách câu đã gán tại cửa vào vòng

> Phần **luật** ghi ở `docs/reviews/game-rules-decisions.md` §11.22, mã **Đ-37**. Mục này chỉ ghi **hệ quả cấp sản phẩm**.
>
> **Quyết định**: admin **thêm / bớt câu hỏi** trong danh sách đã gán ở **`LOBBY`** — **cửa vào vòng**, tức trước khi mở bất kỳ vòng nào. Đang trong một vòng thì **không**. Câu **đã hiển thị** cho thí sinh thì **không gỡ được** (invalid state, toast, không ép được).

**Đây là phân xử một mâu thuẫn đã tồn tại, không phải mở rộng phạm vi.** `Đ-31` và GR-025 đều đã viết *"bổ sung đề rồi mở lại"*; chỉ GR-031 nói ngược (*"snapshot không bị sửa giữa trận"*) và câu đó là tàn dư từ thời pre-flight chạy một lần trước trận.

| Hệ quả cấp sản phẩm | Epic |
|---|---|
| **Màn điều khiển cần một lối vào "sửa danh sách đề" ở **cửa vào vòng** (`LOBBY`). Đây là lối thoát cho ngưỡng chặn cứng của `Đ-31` — nếu không có nó, một vòng thiếu đề là **mất hẳn vòng đó**, mà `Đ-31` lại là ngưỡng **không ép được**. Chặn cứng mà không có đường xử lý là lỗi thiết kế, không phải tính năng an toàn | E-5, E-7 |
| **Thông điệp thiếu đề của `Đ-31` phải dẫn thẳng sang thao tác này.** `Đ-31` yêu cầu nói rõ *vòng nào thiếu bao nhiêu câu*; nay thông điệp đó cần kèm lối đi tới màn sửa danh sách, nếu không admin phải tự mò giữa lúc đang chạy chương trình | E-5, E-7 |
| **Ba trạng thái câu phải phân biệt được trên giao diện chọn đề**: *chưa rút* (gỡ được) · *đã rút, chưa hiển thị* (gỡ được — `GRR-118`: chưa tiêu, trả lại kho) · *đã hiển thị* (**không gỡ được**). Nếu UI chỉ có hai trạng thái thì hoặc admin gỡ nhầm câu đã lộ, hoặc mất quyền dọn câu chưa dùng | E-5, E-7 |
| **Gỡ khỏi danh sách ≠ hoàn tác việc đã dùng.** Nhãn trên nút phải nói đúng nghĩa *"không rút nữa"*. Nếu người dùng hiểu là "trả câu về kho" thì sẽ trông đợi hỏi lại được câu đó — trái no-repeat toàn contest (`R-GEN-06`) | E-5, E-7 |
| **Thao tác này vào AuditLog** như mọi thao tác khác (`CLAUDE.md` §Quy ước: *audit log MỌI thao tác, MỌI role*). Nó đổi tập tài nguyên của trận đang chạy nên là dữ liệu phân xử khiếu nại | E-11 |
| **Phân quyền**: cùng họ với S-1 (bỏ vòng / chạy lại vòng / override). Sửa danh sách đề giữa trận **đổi được kết quả trận**, nên phải nằm trong permission catalog, không mặc định theo vai | E-1, E-7 |
| **Pre-flight chạy lại sau khi sửa.** Sửa xong mà không kiểm lại thì admin vẫn không biết vòng đã mở được chưa. Kết quả kiểm nên hiện **ngay tại màn sửa**, không đợi tới lúc bấm mở vòng | E-5, E-7 |

**Khoảng trống mới phát sinh — cần quyết:**

| ID | Vấn đề | Epic |
|---|---|---|
| **S-21** | **Thêm câu vào danh sách gán có bị hàng rào `everPublic` chặn không?** `R-GEN-12` quy định câu `everPublic` **hard-block mọi match**, force cần confirm 2 bước + audit — nhưng hàng rào đó được đặc tả ở **pre-flight trước trận**. Nay có đường thêm câu **giữa trận**, cần chốt hàng rào có áp ở đó không. Nếu không áp thì đây là **đường vòng qua toàn bộ cơ chế chống rò đề** | E-5, E-11 |
| **S-22** | **Viewer có thấy gì khi admin sửa danh sách đề giữa trận không?** `Đ-5.2e` đã chốt viewer thấy điểm đổi đột ngột khi bỏ vòng, `S-11` hỏi về việc chạy lại vòng. Đây là trường hợp thứ ba: trận đứng yên nhưng tài nguyên bên dưới đổi. Nhiều khả năng là **không hiện gì** — nhưng chưa ai nói | E-9 |
| ~~**S-23**~~ | ✅ **ĐÃ CHỐT 2026-07-27 (`Đ-38`).** `Đ-38` đặc tả đủ điều kiện vào/ra của `LOBBY` — **cửa vào vòng**. **Vào**: trận được tạo · **mọi** nút *"Kết thúc vòng"* · bỏ / chạy lại vòng. **Ra**: mở bất kỳ vòng nào · `TIE_BREAK` · `FINISHED`. ⇒ Cửa sửa đề mở **đúng khi không vòng nào đang chạy**. Đóng **vế (1)** của `GRR-077` (trạng thái nghỉ giữa các vòng); **vế (2) — trận bỏ dở — vẫn treo** | E-7 |

---

### C-19 `[CHỐT 2026-07-27]` — Màn công bố kết quả

> **Quyết định**: hệ thống có một **lớp hiển thị công bố bảng xếp hạng**. Hệ thống **gợi ý mở** ở hai mốc — **sau mỗi vòng** và **khi trận kết thúc** — nhưng **admin mở/đóng được tuỳ ý, bất cứ lúc nào**. Cùng họ với **C-11** (quyền mở/đóng hiển thị của admin).
>
> Máy trạng thái ghi ở `docs/game-state-machine.md` **§F**, mã **STATE-036** + **EVENT-039/040**. **Không** phát sinh `GR-` mới: quyền hiển thị xưa nay nằm ở tài liệu này, `game-rules.md` chỉ trích dẫn (tiền lệ: GR-012 Evaluation order trích `C-11`).

**Vì sao cần chốt**: tài liệu chuẩn **im lặng** về việc công bố, và tiền lệ hiện thực duy nhất thì đi xa hơn những gì tài liệu mô tả.

| Nguồn | Có gì | Hạng nguồn |
|---|---|---|
| **Athena** (`AIServer/ScoreResulting.cs`, `AICtrlLib/ResultUI.cs`) | `ResultUI` là giá trị **first-class** trong enum chuyển màn, ngang hàng 5 vòng thi; **mọi** vòng đều điều hướng tới nó khi xong ⇒ công bố **sau mỗi vòng** | Tiền lệ hiện thực, **không phải requirement** |
| `docs/glossary.md` TERM-043 (R2) | *"Kết quả trận — bảng điểm cuối + thứ hạng, xuất biên bản/PDF"* | Tài liệu chuẩn — nhưng mô tả một **hiện vật dữ liệu**, không phải luồng công bố |
| ~~Demo `public/`~~ | — | ❌ **KHÔNG dùng làm căn cứ** — xem **AS-8** |

> **Demo tĩnh `public/` không có giá trị tham chiếu** (chủ dự án xác nhận 2026-07-27): bản xem thử làm vội. Đây chính là **AS-8** — giả định *"demo là design reference đủ tin cậy"* — nay đã **bác bỏ dứt khoát**. Mọi chỗ demo lệch với tài liệu là **demo sai**, không phải một ý kiến thứ hai cần cân nhắc.

**Bốn ràng buộc suy ra từ quyết định sẵn có** — không phát minh thêm:

| Ràng buộc | Suy ra từ |
|---|---|
| **Thứ hạng do SERVER tính và đẩy xuống.** Client **không** tự suy ra từ bản sao điểm của mình | `CLAUDE.md` §Quy ước (server-authoritative tuyệt đối) · INV-04 |
| **Hoà điểm ⇒ ĐỒNG HẠNG**, theo standard competition ranking (hạng kế nhảy qua số người đồng hạng) | `GRR-032` (đã duyệt) · `GRR-057` / `Đ-10.7a` |
| **Nhịp lộ từng người là ANIMATION, không phải engine.** Engine emit **một** sự kiện công bố kèm bảng xếp hạng; cách trình bày (lộ dần, bục, confetti) là config client-side | `D22` (animation là module độc lập với engine/rule) |
| **Không tự đóng.** Mở và đóng đều là **thao tác bấm của admin**; không có bộ đếm tự quay về | Nguyên tắc nền điểm 2 · `Đ-11` / **C-11** |

| Hệ quả cấp sản phẩm | Epic |
|---|---|
| **Màn viewer và overlay cần một lớp phủ công bố**, bật/tắt theo lệnh admin — không phải một trang riêng thay thế sân khấu. Vì admin mở được **giữa lúc một vòng đang chạy**, lớp này phải chồng lên được mọi màn mà không huỷ trạng thái vòng bên dưới | E-9 |
| **Màn admin cần nút mở/đóng công bố**, kèm **gợi ý** ở hai mốc mặc định (hết vòng · hết trận). Gợi ý theo đúng mô hình advisory của `Đ-5`: hệ thống nhắc, admin quyết | E-7, E-9 |
| **Công bố là READ-ONLY tuyệt đối**: không sinh event điểm, không đổi trạng thái trận, không lộ đáp án (GR-037 giữ nguyên). Nó chỉ **trình bày** kết quả của `reduce(event log)` | E-7, E-10 |
| **Bảng xếp hạng phải chịu được điểm ÂM và ĐỒNG HẠNG**, và **không hard-code số ghế** — hạ tầng UI làm cho 1-12 ghế ngay từ v1 (`CLAUDE.md` §Lộ trình version) | E-9 |
| **`sound-cue` slot mới** cho thao tác công bố — cùng danh sách với **S-5** (revert, bỏ vòng, chạy lại, override). Athena có nhạc riêng (`Sounds\ShowingResult.mp3`), nên slot này gần như chắc chắn sẽ cần | E-9 |

**Khoảng trống mới phát sinh — cần quyết:**

| ID | Vấn đề | Epic |
|---|---|---|
| ~~**S-24**~~ | ✅ **ĐÃ CHỐT 2026-07-27 — xem C-20.** Tiền đề của câu hỏi này **không đứng**: điểm số **luôn hiển thị** cho thí sinh, nên "lộ bảng điểm giữa vòng" không phải là việc lộ thêm gì. Lớp phủ công bố áp cho **mọi vai**, gồm cả máy thí sinh | E-8, E-9 |
| **S-25** | **Công bố ở `TIE_BREAK` hiển thị thế nào?** Nhóm hoà chưa phân định thì bảng xếp hạng chưa có thứ tự thật. Hiện đồng hạng, hay ẩn cho tới khi phân định xong? | E-9 |

---

### C-20 `[CHỐT 2026-07-27]` — Điểm số là thông tin CÔNG KHAI với mọi vai

> **Quyết định**: thí sinh **không hề bị che giấu** điểm số — của chính mình **lẫn của mọi thí sinh khác**. Điểm **luôn hiển thị trên màn hình**, ở mọi vòng, mọi thời điểm.

**Vì sao cần phát biểu tường minh**: repo đang trộn **ba loại thông tin rất khác nhau** vào chung một chữ *"hiển thị"*, và mới chỉ có hai loại được quy định. Người đọc `GR-037` (*"Phạm vi hiển thị đáp án"*) rất dễ tổng quát hoá nhầm sang điểm.

| Loại thông tin | Chế độ | Nguồn |
|---|---|---|
| **Đáp án chuẩn của chương trình** | **MẬT** — chỉ admin + MC. Ngoại lệ duy nhất: `revealAnswerAfterJudge` bật **và** câu đã chấm. Overlay **không bao giờ** thấy | GR-037 · `CLAUDE.md` §Zero-trust |
| **Bài làm của thí sinh khác** | **ẨN TẠM THỜI** trong lúc câu còn mở; lộ khi **admin bấm hiển thị**. Không phải bí mật vĩnh viễn — là cơ chế chống nhìn bài trong cửa sổ trả lời | GR-008 C9 · nguyên tắc nền điểm 14 |
| **ĐIỂM SỐ** | **CÔNG KHAI, LUÔN LUÔN** — mọi vai, mọi lúc, gồm cả máy thí sinh | **C-20** (mục này) |

**Ba thứ này độc lập nhau.** Việc điểm luôn công khai **không** nới lỏng hai dòng trên: đáp án vẫn mật, bài làm vẫn ẩn trong cửa sổ trả lời.

| Hệ quả cấp sản phẩm | Epic |
|---|---|
| **Màn thí sinh phải có bảng điểm của TẤT CẢ các ghế**, không chỉ điểm của mình. Đây là chỗ dễ cài thiếu nhất: bản năng thiết kế màn thí sinh là chỉ hiện "điểm của tôi" | E-8 |
| **Điểm cập nhật realtime cho máy thí sinh** như cho viewer. Mọi sự kiện đổi điểm — gồm cả **revert khi bỏ vòng** (`Đ-5.2e`: *"viewer thấy số đột ngột thay đổi"*) — đẩy tới máy thí sinh y như tới viewer | E-8, E-9 |
| **Lớp phủ công bố (C-19) áp cho cả máy thí sinh**, không riêng viewer/overlay. Đóng **S-24** | E-8, E-9 |
| **Không có cấu hình "ẩn điểm".** Đây là quy tắc cứng, không phải RuleConfig — nếu để cấu hình được thì lại sinh nhánh "trận ẩn điểm" mà không luật nào đặc tả | E-7 |
| **Điểm ÂM cũng hiển thị bình thường** trên máy thí sinh (`Đ-2`: điểm được phép âm, không sàn) | E-8 |

**Không phát sinh `GR-` mới**: cùng họ với `C-11`/`C-19` — quyền và phạm vi hiển thị nằm ở tài liệu này; `game-rules.md` §GR-037 chỉ được bổ sung **một dòng phân biệt** để không bị đọc lấn sang điểm.

---

## 7. Giả định không có bằng chứng

| ID | Giả định | Vì sao đáng ngờ | Nguồn |
|---|---|---|---|
| AS-1 | **Có nhu cầu thị trường thực** cho sản phẩm này | Không có user research, không có người dùng đầu tiên cam kết. `product-gaps.md` §6.1 tự nhận "toàn bộ review là AI-review-AI" | `P/PRD.md` §1; `P/research/product-gaps.md` §6.1 |
| AS-2 | **~5 trận live song song** là nhu cầu thật | D11 chốt con số nhưng không dẫn nguồn nhu cầu. Với mô hình "một deployment = một đơn vị tổ chức" (NG-5), 5 trận cùng lúc của cùng một trường là kịch bản bất thường | `P/DEFERED.md` D11; `P/PRD.md` §2 |
| AS-3 | **Người dùng chấp nhận admin phải chọn thủ công toàn bộ đề trước mỗi trận** | D22 loại bỏ khả năng hệ thống tự lấy đề. Với contest 12 thí sinh × 5 vòng, số câu phải chọn tay là rất lớn; không nguồn nào ước lượng chi phí thao tác này | `P/DEFERED.md` D22; US-3.9 |
| AS-4 | **1 admin đủ vận hành 1 trận** | `product-gaps.md` §2.4 nêu chính rủi ro này (P1) và giảm thiểu bằng "runbook crew ≥2" — tức **thừa nhận 1 người là không đủ**, nhưng PRD/US vẫn viết như thể admin là một người | `P/research/product-gaps.md` §2.4 |
| AS-5 | **Encrypted preload qua service worker chạy được ổn định** | Được chính nguồn xếp là *"hạng mục kỹ thuật phức tạp nhất phía client"*; đã phải chuẩn bị fallback. Đây là quyết định user **bác đề xuất của Claude** (Claude đề xuất (a), user chọn (b)) | `P/DEFERED.md` D12; `P/PRD.md` §7 |
| AS-6 | **Viewer public bằng mã 6 số là chấp nhận được về privacy** | Bất kỳ ai có mã đều thấy tên/ảnh/trường/lớp học sinh vị thành niên. FR-7.3 chỉ có toggle nickname, không mặc định bật. Không nguồn nào đánh giá rủi ro này với NĐ 13/2023 | `P/DEFERED.md` D5; `P/PRD.md` FR-7.1, FR-7.3 |
| AS-7 | **Người tổ chức có sẵn file nhạc/SFX** | D10 chốt "admin tự upload, slot trống = silent, không có bộ SFX default" — nhưng `ux-gaps.md` xếp sound cue là **P1** vì *"là linh hồn gameshow"*. Sản phẩm ra mắt sẽ im lặng hoàn toàn nếu admin không chuẩn bị | `P/DEFERED.md` D10; `P/research/ux-gaps.md` |
| AS-8 | **Demo tĩnh `public/` là design reference đủ tin cậy cho Phase 7-9** | Demo dùng mock data, chưa qua người thật (gate 6.1 chưa chạy). Được tuyên bố "đã verify" nhưng verify bằng Playwright = kiểm render, không phải kiểm UX | `P/plan.md` §Demo; `P/research/product-gaps.md` §6.1 |
| AS-9 | **Fandom wiki chính xác và ổn định** như source of truth luật O26 | Wiki cộng đồng, sửa được bất kỳ lúc nào, không có versioning. D8 chốt nó là source of truth mà không có cơ chế phát hiện wiki thay đổi | `P/DEFERED.md` D8 |
| AS-10 | **Thí sinh có thiết bị riêng để gõ đáp án** | Tăng tốc/VCNV yêu cầu gõ trên máy; portable LAN 12 máy. Không nguồn nào nêu yêu cầu phần cứng tối thiểu phía trường | `P/research/rules-2026.md` §2, §3 |

---

## 8. Đề xuất phạm vi MVP

> `[ĐỀ XUẤT]` — **KHÔNG phải requirement**. Nguồn hiện tại (D18) đã chốt v1 = Phase 1-10.

### 8.1 Nhận xét thẳng về scope hiện tại

`P/DEFERED.md` D18 định nghĩa **v1 = "contest chính thức, 1-12 ghế, ĐỦ mọi loại vòng + biến thể, kho đề, viewer/overlay/MC/admin, 2 profile deploy"** = 10 phase. Đây **không phải MVP** — đây là sản phẩm hoàn chỉnh.

Bằng chứng nội tại rằng scope đang căng:
- `P/research/red-team.md` VÒNG 2 ghi: *"Đề xuất cắt scope → **❌ USER TỪ CHỐI** — ghi nhận tại D18, hệ quả: **Phase 6 dài hơn**, milestone chia lại"*.
- D18 sau đó **vẫn phải cắt** (teams → v2, practice → v1.5), tức chính nguồn đã lùi một bước khỏi lập trường "không cắt scope".
- `P/plan.md` xếp Phase 6 là **critical path**, phải chia milestone 6a/6b để không chặn 3 track UI.

Nói thẳng: đã cắt teams và practice, nhưng v1 vẫn ôm **cả 4 biến thể khởi động, VCNV 4-8 hàng ± gợi ý ký tự, tăng tốc 2 format, về đích 2 package mode, encrypted preload, 2 profile deploy, theming + soundboard đầy đủ**. Phần lớn là *biến thể* của tính năng, không phải tính năng.

### 8.2 MVP đề xuất — "chạy được một trận thật, một cách"

Nguyên tắc: **một đường đi hoàn chỉnh từ soạn đề tới xuất kết quả**, cắt biến thể chứ không cắt giai đoạn.

**Trong MVP:**

| Epic | Giữ gì | Cắt gì so với v1 hiện tại |
|---|---|---|
| E-1 Auth | Login, RBAC permission, 5 role seed | — (không cắt được, là nền) |
| E-2 Kho đề | CRUD 3 pool, metadata, media, duyệt DRAFT→ACTIVE, search/filter | Versioning khi sửa câu ACTIVE (US-2.5) → sau |
| E-3 Import/Export | Import Excel theo template | Contest bundle trọn gói (D23), roster (D26), result bundle (D27) → sau |
| E-4 Contest builder | Preset `O26_DEFAULT@1` + sửa số; QuestionPicker | Round playlist tuỳ ý; biến thể vòng (4 turn kinds → giữ 2; VCNV cố định 4 hàng; tăng tốc chỉ ranked-speed; về đích chỉ preset package) |
| E-5 Phòng thi | Mã 6 số, lobby/tech-check, reconnect grace | — |
| E-6 Engine | Đúng 4 vòng chuẩn O26 + tie-break, **4 ghế** | 1-12 ghế → giữ schema, chỉ mở 4 ở UI (né C-7 hoàn toàn) |
| E-7 Admin control | Stepper, chấm, chỉnh điểm, hoàn nguyên, skip, timer | Control lock 2 admin (US-4.8) → sau |
| E-8 Thí sinh | Chuông click, gõ đáp án, last-wins, reconnect | — |
| E-9 Trình diễn | Viewer + overlay OBS | Màn MC, theming, soundboard → sau (**xem cảnh báo dưới**) |
| E-10 Sau trận | PDF kết quả | Thống kê ghi ngược, replay → sau |
| E-11 Privacy | Seat profile thuộc contest, anonymize | Retention job → sau |
| E-12 Deploy | **Chọn MỘT profile** | Profile còn lại → sau |

**Ngoài MVP** (giữ nguyên như nguồn đã quyết): practice (v1.5), teams (v2), encrypted preload (dùng `reveal-only` — chính là fallback đã có sẵn trong D12b).

### 8.3 Cảnh báo về chính đề xuất này

- Cắt **màn MC** là cắt vào chỗ đau: D15.2 chốt MC thấy đáp án, và trong trận thật MC là người đọc câu hỏi. Nếu không có `/mc`, MC phải nhìn màn admin → phá luồng J-5. **Nếu chỉ giữ được một thứ ở E-9 ngoài viewer, giữ `/mc`.**
- Cắt **âm thanh** mâu thuẫn với `ux-gaps.md` xếp sound cue là P1 *"linh hồn gameshow"*. Nhưng D10 đã chốt slot trống = silent, nên MVP không âm thanh là trạng thái hợp lệ theo thiết kế — chỉ là sản phẩm sẽ demo rất tệ.
- **Khoá 4 ghế** là cách duy nhất né C-7 mà không cần chủ dự án trả lời câu hỏi luật khó. Nếu 1-12 ghế là bắt buộc cho người dùng đầu tiên, phải giải C-7 TRƯỚC khi viết engine.
- Đề xuất này **trái D18 đã chốt**. Chủ dự án đã từ chối cắt scope một lần (red-team vòng 2). Đây là ý kiến, không phải yêu cầu thay đổi quyết định.

---

## 9. Success metrics đề xuất

### 9.1 Tiêu chí hiện có trong nguồn

`P/PRD.md` §6 có 4 tiêu chí:

| # | Tiêu chí | Đo được? | Nhận xét |
|---|---|---|---|
| 1 | Tổ chức 1 trận UAT người thật trọn vẹn 4 vòng + livestream OBS không sự cố chặn trận | ✅ | Nhị phân, rõ ràng |
| 2 | Điểm cuối trận khớp 100% tính tay theo rule-spec cho ≥3 kịch bản automated | ✅ | Rõ ràng |
| 3 | Admin đổi luật (timer/điểm) qua UI, trận chạy theo giá trị mới không sửa code | ✅ | Rõ ràng |
| 4 | Bộ test "không rò đáp án" + chaos drills pass | ✅ | Rõ ràng |

→ **Cả 4 đều là tiêu chí kỹ thuật/QA**. Không có metric nào đo **giá trị sản phẩm với người dùng**: không đo thời gian setup, không đo tỉ lệ hoàn thành, không đo tái sử dụng, không đo sự cố vận hành thực.

### 9.2 Metric đề xuất bổ sung

> `[ĐỀ XUẤT]` — chưa có trong nguồn. Con số là ngưỡng gợi ý, chủ dự án phải chốt lại.

**Nhóm A — Sản phẩm có thực sự thay được PowerPoint/Athena không** (đối chiếu trực tiếp PS-1…PS-8):

| ID | Metric | Cách đo | Ngưỡng gợi ý |
|---|---|---|---|
| SM-1 | Thời gian dựng contest hoàn chỉnh từ đầu (kho đề đã có) | Bấm giờ 1 admin thật, từ "Tạo contest" đến pre-flight pass | ≤ 30 phút |
| SM-2 | Tỉ lệ admin hoàn thành dựng contest lần đầu **không cần hỏi ai** | 5 admin thật, đếm số người pass | ≥ 4/5 |
| SM-3 | Số câu hỏi phải chọn tay cho một trận chuẩn O26 | Đếm trên QuestionPicker | Đo trước, đặt ngưỡng sau — kiểm chứng AS-3 |
| SM-4 | Tỉ lệ tái sử dụng câu hỏi giữa các trận | (số câu dùng ≥2 lần)/(tổng câu) sau 5 trận | ≥ 30% — chứng minh PS-3 đã được giải |

**Nhóm B — Trận chạy có tin cậy không** (đối chiếu PS-5, G-6, G-7):

| ID | Metric | Cách đo | Ngưỡng gợi ý |
|---|---|---|---|
| SM-5 | Số lần admin phải can thiệp ngoài luật/trận | Đếm event `SCORE_ADJUST` + `UNDO` + `SUBSTITUTE` trong event log | ≤ 3/trận |
| SM-6 | Số lần trận bị gián đoạn vì lỗi hệ thống (không phải sự cố ngoài) | Đếm lần admin phải bỏ hoặc chạy lại vòng vì nguyên nhân kỹ thuật | 0/trận |
| SM-7 | Tỉ lệ thí sinh reconnect thành công trong grace | (reconnect OK)/(số lần rớt) | ≥ 95% |
| SM-8 | ~~Khiếu nại "em bấm trước" giải quyết được tại chỗ bằng log~~ → **sửa 2026-07-24**: **Log cung cấp đầy đủ dữ liệu THỜI GIAN và THAO TÁC** cho mọi sự kiện bị khiếu nại — log là **nguồn tham khảo tối đa có thể** của hệ thống, KHÔNG phải nguồn phân xử duy nhất. **Việc AI quyết định được ghi ở BIÊN BẢN VIẾT TAY** (ngoài hệ thống). | (sự kiện bị khiếu nại có đủ timestamp + chuỗi thao tác trong log)/(tổng sự kiện bị khiếu nại) | 100% |

**Nhóm C — Bảo mật đề** (đối chiếu PS-6, G-5):

| ID | Metric | Cách đo | Ngưỡng gợi ý |
|---|---|---|---|
| SM-9 | Số đáp án rò ra kênh không có quyền | Bộ test tự động, cả 2 trạng thái `revealAnswerAfterJudge` | 0 (đã có ở §6 tiêu chí 4 — giữ) |
| SM-10 | Thời gian sớm nhất thí sinh có thể thấy nội dung đề trước reveal | Đo bằng DevTools trong trận mô phỏng | 0 giây (kiểm chứng AS-5) |

**Nhóm D — Vận hành thật** (đối chiếu AS-1, AS-4):

| ID | Metric | Cách đo | Ngưỡng gợi ý |
|---|---|---|---|
| SM-11 | Số trận thật (không phải test) tổ chức trong 3 tháng đầu | Đếm match `official` FINISHED | ≥ 3 — kiểm chứng AS-1 |
| SM-12 | Nhân sự tối thiểu vận hành được 1 trận | Quan sát UAT | Ghi nhận thực tế; nếu >1 thì AS-4 sai và UI admin cần chia lại |

### 9.3 Metric KHÔNG nên đặt

- Không đặt metric về số viewer đồng thời cho tới khi C-2 được phân xử.
- Không đặt metric adoption/tăng trưởng khi chưa có người dùng đầu tiên cam kết (AS-1).

---

## 10. Việc cần làm trước khi viết PRD

Theo thứ tự chặn:

| # | Việc | Chặn cái gì |
|---|---|---|
| 1 | Chủ dự án phân xử **C-1** (per-contest vs per-match) | Toàn bộ mô hình reveal đáp án + use-case rehearsal |
| 2 | ~~Chủ dự án phân xử **C-7** (luật cho >4 thí sinh)~~ → **ĐÃ CHỐT 24/07: v1 = 4 thí sinh, schema vẫn N ghế**. Còn lại: khoá `rowCount`? khoá playlist? sửa phát biểu G-2/D1? | Epic E-6 — phần >4 thí sinh đã gỡ khỏi đường chặn v1 |
| 2b | Trả lời **C-11 Q-A1→Q-A7** (quyền mở/đóng của admin) | Epic E-7 — chưa có FR nào cho quyền này; Q-A2/Q-A5 còn giao với E-6 và C-1 |
| 3 | Chủ dự án chốt hoặc đóng **D26/D27** (C-4) | Epic E-3, journey J-3, và có/không journey "portable → central" |
| 4 | Trả lời **C-8** (ai bấm bắt đầu 3s) | Acceptance criteria vòng Khởi động |
| 5 | ~~Làm rõ **C-9** (Contest vs Match trong UI)~~ → **ĐÃ CHỐT 27/07** (`Đ-49`): mã phòng thuộc **contest**, nút *"Bắt đầu trận mới"* khi kho đề còn đủ, một trận chạy tại một thời điểm | Journey J-2, J-4 nay viết được |
| 6 | Đồng bộ **C-2, C-3, C-6, C-10** (tài liệu cũ chưa sync) | Chất lượng nguồn — nên sửa ngay khi migrate vào `docs/source/` |
| 7 | Quyết định về **NG chưa chốt**: bản quyền format Olympia + license repo | Có thể chặn phát hành, không chặn viết PRD |
| 8 | Chạy gate người thật (`product-gaps.md` §6.1) | Kiểm chứng AS-1, AS-3, AS-4, AS-8 |

---

## 11. Nguồn đã đọc

Toàn bộ `plans/260711-2340-olympia-contest-system/` (28 file) + `CLAUDE.md`:

- `PRD.md` — §1 bối cảnh, §2 mục tiêu/không-mục-tiêu, §3 vai trò, §4 FR-1→FR-7, §5 NFR-1→NFR-7, §6 tiêu chí thành công, §7 rủi ro, §8 lộ trình
- `user-stories.md` — US-1.x → US-10.x nhóm theo 8 role
- `DEFERED.md` — D1→D27 (D7/D14 đã bỏ; D26/D27 mở 15/07)
- `plan.md` — lộ trình 12 phase, 3 mốc release, sơ đồ track song song
- `research/rules-2026.md` — luật O26 theo Fandom + §7 edge-cases
- `research/ruleconfig-v2-spec.md` — §1→§13 (spec engine)
- `research/product-gaps.md` — gap pháp lý/privacy/quy trình/vòng đời
- `research/ux-gaps.md` — P1/P2/P3 UX bị bỏ quên
- `research/red-team.md` — 3 vòng review, findings C/H/M/L
- `research/athena-scout.md` — bài học từ bản C#/WPF cũ
- `research/tech-architecture.md`, `research/sound-cues.md`, `research/queue-decision.md`, `research/queue/**` — tham chiếu (kiến trúc kỹ thuật, ngoài phạm vi tài liệu này)
- `phase-01` → `phase-12` — chi tiết triển khai từng phase (tham chiếu)
