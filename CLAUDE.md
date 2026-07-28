# KnowledgeDome — Olympia Contest System

Nền tảng web tổ chức thi đấu gameshow kiến thức tuỳ biến (mô hình Đường lên đỉnh Olympia). Planning **(bản nháp, sẽ migrate vào `docs/`)** tại `plans/260711-2340-olympia-contest-system/` (spec engine: `research/ruleconfig-v2-spec.md`); sổ quyết định: `plans/260711-2340-olympia-contest-system/DEFERED.md` (D1-D23, đa số đã chốt); demo tĩnh: `public/`.

Stack (đã chốt): NestJS + **Express adapter**, Zod, Prisma+Postgres, Redis, Better-auth, Socket.IO, @casl/ability · React+Vite, MUI, Motion for React, Zustand, TanStack Query · MinIO.

## Product and Specification Workflow

### Source-of-truth hierarchy

1. `docs/source/`
   - Original business and product documents.
   - Never modify these files unless explicitly requested.

2. `docs/PRD.md`
   - Product-level requirements and epic definitions.
   - Must be traceable to documents under `docs/source/`.

3. `specs/<feature>/spec.md`
   - Canonical feature requirements and user stories.
   - Overrides informal descriptions in chats or implementation plans.

4. `specs/<feature>/plan.md`
   - Canonical technical implementation plan for that feature.

5. `specs/<feature>/tasks.md`
   - Canonical executable task list.

### Rules

- Never invent business requirements.
- Mark missing information as `NEEDS CLARIFICATION`.
- Mark contradictions as `CONFLICT`.
- Every requirement must reference its source.
- ClaudeKit may scout, research, review, test, and implement.
- Spec Kit owns feature specifications, implementation plans, and tasks.
- Do not create a separate ClaudeKit plan for a Spec Kit-managed feature
  unless explicitly requested.
- Do not expand feature scope during implementation.

> Chi tiết cưỡng chế (6 cổng chất lượng, định dạng marker, quy tắc sửa đổi):
> `.specify/memory/constitution.md`. **`plans/**` là BẢN NHÁP** — chưa migrate sang `docs/`,
> KHÔNG được trích như requirement đã chốt.

## Lộ trình version (✅ D18 chốt 12/07 · ✅ sửa phạm vi số ghế 24/07)

- **v1 — Solo contest** (Phase 1-10): contest chính thức, thí sinh CÁ NHÂN, đủ loại vòng + biến thể, kho đề, viewer/overlay/MC/admin, 2 profile deploy (compose + portable Windows).
  - **LUẬT v1 chỉ đặc tả cho ĐÚNG 4 THÍ SINH** (✅ chốt 24/07). Luật gốc O26 viết cho đúng 4 người; không phát minh luật cho số ghế khác.
  - **Nhưng v1 VẪN hỗ trợ LƯU TRỮ DỮ LIỆU và UI cho 1-12 người** — schema, seat model, `scoringUnit`, RuleConfig dạng mảng, và giao diện đều làm cho 1-12 ngay từ đầu. Chỉ **LUẬT/engine-path** cho số ghế ≠ 4 là chưa có.
- **v1.5 — Practice + luật đa ghế** (Phase 11): `matchPurpose: practice`, bộ đề PUBLIC + share-link, UI luyện tập solo, trainer role, retention riêng (practice 3 tháng / official 12 tháng) · **+ LUẬT cho 1-12 thí sinh** (✅ chuyển từ v2 sang v1.5 ngày 24/07) — chỉ ship luật + sửa controller, KHÔNG migrate schema.
- **v2 — Teams** (Phase 12): thi đội — buzz cá nhân, điểm về đội (semantics spec §2b).
- **DB + Zod schema chuẩn bị ĐẦY ĐỦ ngay từ v1** (Team/seat.teamId/scoringUnit, matchPurpose, visibility/everPublic, ACL, retention) — KHÔNG để dành schema cho version sau, tránh migrate; v1 chỉ chưa bật engine-path tương ứng.

## Luật chơi & đề thi (chốt 12/07)

- **Source of truth luật O26 = Fandom wiki** ([Luật chơi/Olympia 26](https://duong-len-dinh-olympia.fandom.com/vi/wiki/Lu%E1%BA%ADt_ch%C6%A1i/Olympia_26)) — bảng giá trị đã đối chiếu: `plans/.../research/rules-2026.md`. Mọi giá trị vẫn là RuleConfig custom được; contest builder có nút **"Áp dụng luật 2026"** áp preset `O26_DEFAULT@1`.
- **Điểm ĐỘC LẬP thời gian**: `timeSeconds` là metadata TỪNG CÂU HỎI (cùng mức 20đ có thể câu 15s và 40s) — hệ thống chọn câu theo MỨC ĐIỂM, thời gian lấy theo câu; preset chỉ đặt default.
- **Người tạo contest PHẢI chọn danh sách câu hỏi trước khi start** (full-text search + filter + sort trên kho đề); hệ thống KHÔNG tự lấy đề — draw chỉ RANDOM TRONG danh sách đã gán (snapshot). Pre-flight chặn start khi thiếu.
- **Contest config import/export trọn gói** (D23): ZIP = Excel câu hỏi (default; nhận CSV/Google Sheet) + JSON metadata media + media theo subfolder từng vòng — use-case soạn trên bản Internet → import vào portable.

### Hệ thống ADVISORY — người vận hành phán quyết (✅ chốt 24/07)

Nguyên tắc nền: **máy độc quyền SỰ KIỆN, người độc quyền PHÁN QUYẾT.** Mô hình **BA TẦNG** (áp cho mọi câu hỏi "ai làm X"):

| Tầng | Vai trò | Phương tiện |
|---|---|---|
| **MC** | Thẩm quyền phán quyết **trên sân khấu** | **Nói** |
| **Admin** | **Cảm biến + cơ cấu chấp hành DUY NHẤT** của hệ thống | **Bấm** |
| **Server** | Sự kiện và thời gian (server time, thứ tự chuông, timer) | Không ai sửa được |

⇒ Màn `/mc` là **READ-ONLY**: MC không thao tác hệ thống, MC quyết bằng lời và **admin bấm**. Không tạo bề mặt quyền ghi mới cho MC.

- **Máy KHÔNG tự chấm Đúng/Sai.** Với câu gõ, máy chỉ **highlight ký tự khác** giữa bài làm và đáp án; admin tự đánh giá (chính tả, ý nghĩa tương đồng). Không viết đường code nào tự cộng/trừ điểm từ so khớp.
- **Admin chọn vòng nào bắt đầu và lượt của ai. THỨ TỰ DO ADMIN QUYẾT ĐỊNH**; hệ thống chỉ **recommend theo luật + contest settings** (vị trí thí sinh, thứ tự lượt riêng Khởi động — đều là đầu vào của recommendation, KHÔNG phải ràng buộc cưỡng chế). Conflict luật (một người thi 2 lần, đổi lượt…) → **dialog cảnh báo**, admin bấm Yes/No để vẫn thực hiện. KHÔNG chặn cứng.
- **Admin toàn quyền mở/đóng đáp án và ô chữ.**
- **Mọi mốc thời gian mà luật gốc mô tả bằng hành vi của MC đều ánh xạ thành MỘT THAO TÁC BẤM CỦA ADMIN** — máy không quan sát được sân khấu, **admin là cảm biến**. Cụ thể: *"MC đọc xong câu hỏi"* → **admin start timer**; *"hiệu lệnh của người dẫn chương trình"* (Câu hỏi phụ) → **admin bấm**, admin là người nghe hiệu lệnh.
- **Mốc do admin bấm là TUYỆT ĐỐI — KHÔNG có cửa sổ ân hạn, KHÔNG trừ bù độ trễ tay người** (thời gian do server quyết định). Van thoát không phải grace mà là: lịch sử được giữ đầy đủ để admin **xem lại và gỡ lệnh cấm** nếu cần.
- **Bỏ vòng / chạy lại vòng** được phép, với điều kiện còn câu hỏi. **Điểm = event log; reset = REVERT** (như `git revert`, KHÔNG phải `reset --hard`) — lịch sử **linear, append-only, không xoá**; biên bản trận giữ đầy đủ, vòng bị bỏ hiện kèm nhãn "đã bỏ". Câu đã dùng **không** trả lại pool.
- **Vòng tính điểm theo thứ hạng (Tăng tốc): một câu = MỘT event điểm cho TOÀN BỘ người chơi**, không phải mỗi người một event — revert là revert cả bảng xếp hạng của câu đó.

### Hai mode trả lời (✅ chốt 24/07) — cấu hình ở cấp CONTEST, một giá trị chung cho toàn bộ vòng

- **Mode sân khấu (MẶC ĐỊNH, và là mode LUẬT được đặc tả theo)**: thí sinh **đọc** đáp án, máy chỉ dùng để **giành quyền trả lời**.
- **Mode nhập liệu**: thí sinh gõ đáp án.
- Áp cho **Khởi động, Về đích, Câu hỏi phụ**. **VCNV và Tăng tốc LUÔN gõ máy** — ở VCNV mode chỉ đổi cách **chọn hàng ngang** (sân khấu → admin điều khiển; nhập liệu → thí sinh click), và thí sinh dùng máy để chọn **"Mở chướng ngại vật"**.
- **Chọn hàng ngang: MỘT đường vào cho mỗi mode** (✅ D36 chốt 25/07 — thay cho "hai đường vào"): sân khấu → **chỉ admin** click, máy thí sinh không có nút chọn; nhập liệu → **chỉ thí sinh** click, **admin không chọn thay**. Cả hai đều qua **hàng đợi** + admin xác nhận Yes/No. **KHÔNG drop tín hiệu.**
- **Điểm được phép ÂM**, không có sàn.

## UX — BẮT BUỘC

Áp cho MỌI UI trong repo này (app React lẫn demo tĩnh `public/`):

- **Visibility of System Status**: Mọi thao tác async (submit, save, validate, load) PHẢI hiển thị trạng thái rõ ràng — spinner/loading state, progress indicator, hoặc skeleton. Không để UI im lặng khi đang xử lý.
- **Immediate Feedback**: Phản hồi người dùng **ngay lập tức**, không có độ trễ cảm nhận được. Toast/message thành công hoặc thất bại phải xuất hiện sau mỗi action. Luồng async theo pattern `loading → success/error` (MUI: `Snackbar`/`Alert`; demo tĩnh: toast component chung).
- **KHÔNG chặn gửi lại (rule chống double-submit đã bị chủ dự án gỡ bỏ)**: nút action chỉ hiện trạng thái loading, KHÔNG disable; người dùng gửi lại được — server nhận **bản cuối cùng** trước timeout. Dedup/idempotency là việc của SERVER (event log), không phải của UI. *Ngoại lệ: khoá-theo-LUẬT-CHƠI (chuông bị khoá khi sai, NSHV đã dùng, không tới lượt) vẫn disable bình thường — đó là trạng thái game, không phải chống double-submit.*
- **Nút bấm chuông CHỈ nhận click chuột** — không gán hotkey cho chuông (tránh bấm nhầm khi gõ đáp án); các hotkey khác (Enter gửi, 1-8 chọn hàng...) giữ nguyên. **Nút "Mở chướng ngại vật" (VCNV) được xếp là CHUÔNG** ⇒ cũng chỉ nhận click chuột.
- **Dialog xác nhận đặt ở phía ADMIN; phía thí sinh CHỈ có ở thao tác KHÔNG đua tốc độ** (✅ chốt 24/07, sửa 25/07 theo D36). Đây là **ngoại lệ có chủ đích** của rule "không chặn gửi lại" ở trên: nó chống bấm nhầm hành động không thu hồi được, không phải chống double-submit.
  - **Phía thí sinh: tức thời, không dialog, không rút lại** — *"thí sinh cần tốc độ, và tự chịu trách nhiệm sai lầm của mình"*. Áp cho **mọi thao tác đua tốc độ**: chuông, "Mở chướng ngại vật", gửi đáp án.
  - **NGOẠI LỆ DUY NHẤT — chọn hàng ngang ở mode nhập liệu** (D36): thao tác **một chiều, hậu quả nặng, KHÔNG bị ép thời gian** ⇒ có **dialog xác nhận trên máy thí sinh**, xác nhận xong thì **khoá nút chọn**. Khoá là **TẠM**: admin bấm No ⇒ **mở lại** (bắt buộc bởi rule "reject ⇒ thí sinh không mất lượt"). Dialog này **không thay thế** bước admin duyệt Yes/No — hai lớp khác mục đích: dialog chống bấm nhầm, admin duyệt là phán quyết.
  - **Phía admin: mọi thao tác không hoàn tác được đều qua dialog Yes/No** — mở đáp án/ô chữ, xác nhận chọn hàng ngang, **và xác nhận nút "Mở chướng ngại vật" của thí sinh**.
  - **Mọi tín hiệu của thí sinh đều vào HÀNG ĐỢI theo thứ tự tới** (server timestamp). **KHÔNG có cơ chế drop.** **Hàng đợi đang hoạt động** reset sau mỗi VÒNG rồi tái sử dụng — nhưng **LỊCH SỬ tín hiệu KHÔNG BAO GIỜ XOÁ** (append-only), để admin xem lại và **gỡ lệnh cấm** khi cần. Queue **chỉ CHẶN ở VCNV**:
    - **VCNV** (chọn hàng ngang, "Mở chướng ngại vật") — queue **chặn**: admin duyệt lần lượt, xác nhận mới có hiệu lực. Reject ⇒ tín hiệu kế tiếp lên, **thí sinh KHÔNG mất lượt**. Đây là chỗ sửa lỗi bấm nhầm của thí sinh — admin bấm No, không phải bắt thí sinh xác nhận.
    - **Khởi động lượt chung, Về đích cướp quyền** — queue **KHÔNG chặn**: **có chuông là tính ngay** theo server timestamp, không chờ duyệt. Queue vẫn ghi nhận thứ tự để admin **can thiệp khi có sự cố**.
  - **Tiêu chí phân biệt**: tín hiệu *một chiều, hậu quả nặng, không bị ép thời gian* ⇒ queue chặn. Tín hiệu *đua tốc độ, cửa sổ chặt* ⇒ không chặn, server phân xử ngay (giữ nguyên "server time là quyết định cuối cùng"); queue chỉ là lưới an toàn.
- **Tăng tốc: nhận MỌI lần trả lời đến khi hết giờ, tính BẢN CUỐI CÙNG** — không khoá input/nút gửi sau khi trả lời; ranking theo server-received timestamp của bản cuối.
- **Server time là source of truth DUY NHẤT và là quyết định cuối cùng**: timeout, thứ tự chuông, thứ hạng tốc độ đều theo đồng hồ server; client chỉ hiển thị.
- **Viewport-Conscious Balanced Layout**: Design chủ đích theo kích thước màn hình. Giữ **nội dung chính của mỗi page/tab trong 1 viewport** trên khung nhìn tham chiếu của dự án — không thứ gì quan trọng phải scroll mới thấy — nhưng **không được nhồi nhét**: giữ breathing room và whitespace dễ đọc. Cân bằng cả hai chiều hỏng:
  - **Không ép scroll** — nếu phần tử quan trọng nằm dưới fold, sửa layout (bỏ page header trùng breadcrumb; nén hàng stat-card "hero" thành metric strip mỏng; form 2 cột compact thay vì card xếp dọc từng field). **Bảng dài ưu tiên pagination** với page size theo viewport (~10-12 dòng) để header + toolbar + rows + pager vừa 1 màn **không scroll trang** — đây là cách sửa chính; table body scroll nội bộ + sticky header + pager ghim là fallback khi ranh giới trang bất tiện (vd ma trận cố định). Chỉ giữ scroll cả trang khi không còn cách hợp lý, và phải giải thích được.
  - **Không phí không gian / không quá đặc** — không để mảng trống lớn hoặc info giá trị thấp chiếm chỗ đẹp; cũng không nén chặt đến rối. Ưu tiên info quan trọng với **role hiện tại**.
  - **Dashboard**: bố cục theo **Z / F reading model**; đưa info liên quan nhất của từng role lên trước (admin → điều khiển trận + hàng chờ duyệt ĐỀ (DRAFT→ACTIVE); setter → câu hỏi của mình + trạng thái duyệt; thí sinh → trạng thái thi + điểm; viewer → sân khấu + bảng điểm).

## Điều hướng (demo `public/` và app)

- Mọi màn có nút **Back** rõ ràng về màn trước/menu; `Esc` = back (đóng modal trước nếu đang mở), `H` = về hub/menu chính. *Ngoại lệ màn thi đấu của thí sinh: Esc CHỈ xoá ô nhập, không back (browser dùng Esc thoát fullscreen — tránh văng fullscreen giữa trận).*

## Nguyên tắc code — BẮT BUỘC toàn repo

- **DRY**: không lặp logic/hằng số/schema — Zod schemas, RuleConfig, permission catalog, socket event contracts đều ở `packages/shared` dùng chung FE+BE; validation viết MỘT lần (Zod) chạy cả hai đầu; component/hook/util lặp ≥2 lần phải trích xuất.
- **Zero-trust security**: KHÔNG BAO GIỜ tin client — mọi request/socket event đều verify auth + permission (CASL) ở server bất kể client là ai, đã join room gì, UI có ẩn nút hay không; viewer/overlay là public nhưng server vẫn enforce read-only (drop mọi event ghi từ namespace này); mọi input validate lại ở server (validate FE chỉ là UX); **TRƯỚC mốc CÂU KHÉP đáp án chỉ rời server tới admin + MC (authenticated + audit); viewer/thí sinh/overlay không nhận. TỪ mốc câu khép, server đẩy đáp án tới cả ba vai đó nếu `revealAnswerAfterJudge` bật (cờ CẤP TRẬN — `QĐ-062`; mặc định BẬT cho cả official lẫn practice — `QĐ-080`; đổi được từng trận). CÂU KHÉP ≠ "đã chấm": ở Về đích cú bấm chấm Sai MỞ cửa sổ cướp quyền 5s, câu chỉ khép sau khi cửa sổ đóng và người cướp đã được chấm — công bố sớm là xoá sổ cướp quyền. Ba ca biên: câu bị bỏ qua VẪN công bố; "Huỷ kết quả" KHÔNG tự công bố; đáp án Chướng ngại vật theo `GR-012`, ngoài cơ chế này. CẤM đẩy đáp án xuống client trước mốc rồi ẩn bằng cờ hiển thị (lỗi của tiền lệ Athena)**; timer/điểm/chuông chỉ tính ở server.

## Mô hình truy cập (chốt 12/07)

- **Public (chỉ cần MÃ PHÒNG 6 số, không account, không duyệt)**: màn viewer + overlay OBS (frame stream) — read-only tuyệt đối, có rate-limit + nút "khoá cổng" của admin.
- **Cần AUTH (username+password + permission)**: thí sinh, MC, admin/setter — mọi giao diện có thể gửi event hoặc thấy đáp án.

## Quy ước khác

- Server-authoritative tuyệt đối: timer/điểm/chuông tính ở server, client chỉ render; **server time là source of truth duy nhất**.
- **Audit log MỌI thao tác, MỌI role** (user chốt 12/07): auth (login/logout/fail), CRUD kho đề/bộ đề, contest management, mọi event trong trận (đã có MatchEvent log), viewer join/kick, xuất/nhập, xem đáp án — bảng AuditLog chung append-only (actor, action, target, ts, ip) ngoài các log chuyên biệt.
- **TRIM mọi input text ở CẢ frontend lẫn backend** (đề, đáp án, submission — tránh space đầu/cuối phá so khớp).
- **Chỉ tiếng Việt** (không i18n); **múi giờ thống nhất UTC+7** — DB lưu UTC, mọi hiển thị/log/PDF format theo UTC+7, không có timezone setting per-user.
- **Stack: NestJS + Express adapter** (✅ D9 12/07 — không dùng Fastify).
- Mọi timer/điểm là RuleConfig — KHÔNG hard-code luật trong code/UI.
- **Animation là module ĐỘC LẬP với engine/rule** (D22): engine chỉ emit semantic event; mapping event→animation là config client-side — sửa rule (vd cách chọn câu hỏi) không đụng animation và ngược lại.
- **Media thí sinh preload MÃ HOÁ qua service worker** (D12b): key phát đúng lúc reveal theo server time; fallback reveal-only khi SW không khả dụng. Viewer/overlay preload URL thường.
- **Sound**: engine emit `sound-cue {slot}`; admin tự upload file per slot (slot trống = silent, không có bộ SFX default); client pre-download toàn bộ SFX khi vào phòng.
- **Giới hạn kích thước media là env config** — ảnh, video, audio mỗi loại một ngưỡng riêng. **Không hard-code con số** ở bất kỳ đâu trong code hay UI; thông điệp từ chối phải đọc ngưỡng từ config.
- **Font: "Be Vietnam Pro"** + fallback font hệ thống hỗ trợ tiếng Việt (stack chuẩn trong `public/assets/tokens.css` `--font-sans`); app thật SELF-HOST woff2 trong bundle (portable LAN offline — không dùng CDN), demo dùng Google Fonts.
- String UI tiếng Việt tách file constants (`vi.ts`), không hard-code trong JSX.
- Commit theo Conventional Commits, KHÔNG AI attribution.
