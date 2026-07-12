# PRD — Olympia Contest System

| | |
|---|---|
| **Sản phẩm** | Hệ thống quản lý & mô phỏng chương trình "Đường lên đỉnh Olympia" (luật 2026) |
| **Phiên bản tài liệu** | 1.0 — 12/07/2026 |
| **Trạng thái** | Draft v2 (12/07, sau mở rộng scope + red-team) — đã chốt D1/D3/D15-MC/D18; chờ các mục còn lại trong `DEFERED.md` cùng thư mục (nổi bật: D7, D8, D17) |
| **Tài liệu liên quan** | `plan.md` (kiến trúc + 10 phase) · `user-stories.md` · `research/rules-2026.md` (luật) · `public/` (demo đã duyệt design) |

## 1. Bối cảnh & Vấn đề

Các trường học/CLB muốn tổ chức thi đấu theo format Đường lên đỉnh Olympia phải dùng PowerPoint thủ công hoặc phần mềm desktop LAN cũ (bản Athena C#/WPF): luật hard-code theo format lỗi thời, không chạy online, không có kho đề dùng lại, không tích hợp livestream, mất kết nối là hỏng trận.

**Sản phẩm này** là web app self-host cho phép tổ chức trọn vẹn một trận Olympia theo luật 2026: kho đề bảo mật có metadata, thi đấu realtime độ trễ thấp, admin toàn quyền điều khiển/can thiệp, khán giả xem qua màn viewer hoặc qua livestream OBS.

## 2. Mục tiêu & Không-mục-tiêu

### Mục tiêu (v1)
0. **Mục tiêu kép ngang hàng (chốt 12/07)**: (a) tổ chức contest chính thức; (b) luyện tập/rehearsal — cùng một engine, phân biệt bằng `matchPurpose` per-match (spec §13).
1. **Nền tảng gameshow tuỳ biến** (mở rộng 12/07 — spec: `research/ruleconfig-v2-spec.md`): round playlist tuỳ ý (số vòng/loại/thứ tự không cứng), 1-12 thí sinh hoặc gộp đội (buzz cá nhân, điểm về đội), mọi timer/điểm/phổ điểm/số câu là config, thời gian là thuộc tính từng câu; luật O26 chuẩn là preset mặc định `O26_DEFAULT@1`. Biến thể vòng: khởi động 4 kiểu lượt + rút đề ngẫu nhiên theo lĩnh vực; VCNV 4-8 hàng ± gợi ý ký tự; tăng tốc ranked-speed / clue-buzz (3-4 dữ kiện); về đích gói preset / custom-build.
2. Kho đề tập trung, bảo mật cao (đáp án không bao giờ tới client trước công bố), metadata đầy đủ, import/export được.
3. Thi đấu realtime công bằng: chuông xếp hạng theo server-timestamp, timer server-authoritative, chống gian lận mức hợp lý.
4. Vận hành trận tin cậy: pause/resume, undo chấm điểm, phục hồi sau sự cố server/mạng, audit log phân xử khiếu nại.
5. Trình diễn: màn viewer animation đẹp (tối ưu 4 ghế, adaptive 1-12/đội) + overlay OBS 1920×1080 nền trong suốt + **màn MC** (câu hỏi + đáp án + tóm tắt kết quả) cho livestream; **theming per contest** (màu sắc, logo, ảnh thí sinh, video hình hiệu) + **âm thanh tuỳ chỉnh mọi thành phần** (cue slots + nhạc nền soundboard).

### Không-mục-tiêu (v1)
- Không stream video (chỉ overlay data cho OBS — quyết định đã chốt).
- Không giải đấu nhiều trận/bracket tuần→tháng→quý→năm (P3, xem `research/ux-gaps.md`).
- Không speech-to-text tự chấm câu trả lời miệng (DEFERED D4 — admin chấm).
- Không đa ngôn ngữ (D2 — tiếng Việt, string tách file).
- Không multi-tenant nhiều tổ chức (một deployment = một đơn vị tổ chức).

## 3. Người dùng & Vai trò

| Role | Là ai | Quyền chính |
|---|---|---|
| **Admin** | BTC/kỹ thuật | Tạo user, tạo contest, gán đề & thí sinh, điều khiển trận (hoặc gán quyền host theo contest — D3b), chấm điểm, chỉnh timer/điểm, khoá cổng/kick viewer, xem toàn kho đề |
| **Người ra đề (Setter)** | Giáo viên/ban đề | CRUD câu hỏi của mình trong kho, upload media, import/export phần đề mình phụ trách |
| **Thí sinh (Contestant)** | Học sinh thi đấu | Đăng nhập **chỉ bằng username+password**, vào phòng bằng mã 6 số, thi đấu (chuông/gõ đáp án) |
| **Viewer** | Khán giả | nhập mã 6 số → xem NGAY (public, không duyệt — ✅ 12/07); read-only |
| **MC** (permission theo contest) | Người dẫn chương trình | Màn `/mc` read-only: câu hỏi + đáp án + tóm tắt kết quả — không điều khiển |
| *(OBS Overlay)* | Máy stream | Vào bằng mã phòng như viewer (không token — ✅ 12/07); read-only tuyệt đối |

## 4. Yêu cầu chức năng (FR)

### FR-1 Auth & Quản lý user
- FR-1.1 Đăng nhập username+password (Better-auth); không đăng ký tự do — admin tạo account.
- FR-1.2 RBAC theo **permission** (đã chốt 12/07), hiện thực bằng **@casl/ability** (+ @casl/prisma, @casl/react): 1 role = tập permission, 1 user có nhiều role, admin tạo được role mới từ permission catalog; 4 role mặc định (ADMIN/SETTER/CONTESTANT + VIEWER guest) là seed; authz check theo ability/permission với conditions (vd "câu hỏi của mình"), không theo tên role; quyền host trận là permission gắn theo contest.
- FR-1.3 Rate-limit đăng nhập; 1 phiên hoạt động/thí sinh; session thí sinh 24h.

### FR-2 Kho đề & Bộ đề
- FR-2.1 CRUD câu hỏi theo loại vòng (Khởi động / VCNV set / Tăng tốc / Về đích / Câu phụ) + trạng thái duyệt (DRAFT→ACTIVE→ARCHIVED, chỉ admin activate) + versioning khi sửa.
- FR-2.2 Mỗi câu: `displayId` tra cứu, lĩnh vực (taxonomy `Field` quản lý được), wordCount (auto), nội dung, đáp án + acceptedAnswers, giải thích, người thực hiện, ghi chú, media; thuộc tính riêng theo loại: `timeSeconds` (TT/VĐ), `value` (VĐ), `clues[]` (TT clue-buzz); metadata cũ (độ khó, tags) giữ nguyên.
- FR-2.2b **Bộ đề (QuestionSet)**: `displayId`; PRIVATE (owner+ACL; share-link password+TTL revoke được) / PUBLIC (xem/tải tự do, kèm đáp án theo setting per-set default có, cảnh báo trước khi public, chặn public khi gắn contest chưa diễn); item = reference câu kho theo ID hoặc nhập tay (checkbox lưu vào kho); tra cứu theo ID/lĩnh vực/người thực hiện/đáp án (đáp án cần quyền).
- FR-2.3 Media ảnh/video/audio lưu MinIO, truy cập qua presigned URL TTL ngắn; giới hạn dung lượng (D6).
- FR-2.4 **Bảo mật**: đáp án chỉ trong DTO của admin/setter-owner; audit log mọi truy cập đáp án/sửa/xuất. (trong trận: + kênh MC FR-5.5; + ngoại lệ revealAnswerAfterJudge NFR-4)
- FR-2.5 Import/export: **Excel theo template quy ước là format chính** (nhập/xuất bộ đề, roundtrip); ZIP bundle cho trường hợp kèm media; mapping cột linh hoạt cho file tự do.

### FR-3 Contest & Phòng thi
- FR-3.1 Admin tạo contest bằng **contest builder**: dựng round playlist (thêm/xoá/sắp xếp vòng, config từng vòng theo RuleConfig v2, chọn preset rồi tuỳ biến), setup **1-12 ghế** ± gộp đội (scoringUnit, individualTurnMode all-members/representative), theme (màu/logo/ảnh/video hình hiệu), sound (cue slots + nhạc nền), gán bộ đề (snapshot).
- FR-3.2 Mã phòng 6 số random, không reuse 24h; pre-flight validate đề đủ số câu + media trước khi start.
- FR-3.3 Lobby/tech-check: thí sinh thử chuông (hiện ping ms), thử âm thanh, báo sẵn sàng.
- FR-3.4 **Viewer + overlay OBS PUBLIC theo mã phòng** (user chốt 12/07 — không account, không duyệt): có mã 6 số là xem; read-only enforced server-side (zero-trust); kiểm soát = rate-limit IP + nút khoá cổng + kick. Thí sinh/MC/admin luôn cần auth.
- FR-3.4b Monitor kết nối trong trận (permission `contest.control`): thí sinh theo ghế 1,2,3... (online/offline, ping); viewer/MC chỉ số lượng.
- FR-3.5 Reconnect grace 120s giữ ghế + state-sync; rớt quá grace xử lý theo `dropoutPolicy`.

### FR-4 Thi đấu (Game Engine) — spec: `research/ruleconfig-v2-spec.md`; luật gốc + edge-cases: `research/rules-2026.md` §7
- FR-4.1 Orchestrator chạy **round playlist** server-authoritative; timer server; buzzer cá nhân xếp hạng theo server-timestamp, điểm reduce về seat/team theo scoringUnit; khởi động hỗ trợ **rút đề ngẫu nhiên theo lĩnh vực** (slot theo Field + RANDOM, noRepeatInMatch, xáo thứ tự — event `QUESTIONS_DRAWN` replay được).
- FR-4.2 Chấm: auto-match đáp án gõ (normalize) + admin confirm/override; câu miệng admin bấm Đúng/Sai.
- FR-4.3 Can thiệp admin: cộng/trừ điểm kèm lý do, undo, skip/thay câu dự phòng, pause/resume, chỉnh timer đang chạy.
- FR-4.4 Event-sourced match log (điểm = reduce(events)); mọi biến động truy vết được; phục hồi trận sau crash (persist-trước-broadcast).
- FR-4.5 Âm thanh: **cue slot theo từng thành phần × loại vòng** (intro/question/countdown/buzz/correct/wrong/timeup/reveal...) — bộ SFX mặc định royalty-free, admin thay từng slot (D10b); **nhạc nền playlist** điều khiển bằng soundboard admin (phát/dừng/next/volume/duck), phát ở viewer/overlay, contestant mặc định tắt.
- FR-4.6 Media preload phân tầng: viewer/overlay sớm, thí sinh chỉ lúc reveal (D12a).

### FR-5 Giao diện thi đấu
- FR-5.1 **Thí sinh**: tối giản, phản hồi cục bộ <50ms; **chuông CHỈ nhận click chuột** (không hotkey); keyboard cho phần còn lại (Enter gửi, 1-8 hàng ngang, Esc xoá); **không chặn gửi lại — tăng tốc tính bản trả lời CUỐI CÙNG trước server-timeout; server time là source of truth duy nhất**.
- FR-5.2 **Admin**: bàn điều khiển 3 cột (vòng/phòng/viewer — câu hỏi/chấm/timer — bảng điểm/log); hotkey chấm nhanh; control lock khi nhiều host.
- FR-5.3 **Viewer**: animation đầy đủ mọi vòng (bảng điểm count-up, VCNV lật ô + miếng ghép, tăng tốc lane, NSHV, podium+confetti) 60fps, animation-queue không chặn engine.
- FR-5.4 **OBS Overlay**: 1920×1080 nền trong suốt, phần tử bật/tắt từ admin, chỉ transform/opacity, <50% CPU 1 core trong OBS.
- FR-5.5 **Màn MC** (`/mc`, đã chốt 12/07): read-only chữ rất to — câu hỏi hiện tại + **đáp án** + tóm tắt kết quả/bảng điểm; permission `match.viewAnswer` theo contest, audit log (đáp án chỉ tới admin + MC channel).
- FR-5.6 **Theming per contest**: đổi màu thành tố đồ hoạ (design tokens), logo/banner cuộc thi, ảnh thí sinh, video hình hiệu (phát ở intro/INTERMISSION) — áp cho viewer/overlay/MC.
- FR-5.7 Layout adaptive: tối ưu ≤4 đơn vị điểm (sân khấu đầy đủ); 5-12 grid gọn vẫn đủ animation; hiển thị theo đội khi có đội.

### FR-6 Sau trận
- FR-6.1 Xuất kết quả PDF (bảng điểm, sự kiện chính, QR verify).
- FR-6.2 Thống kê câu hỏi (% đúng, thời gian TB) ghi ngược metadata kho đề.
- FR-6.3 Replay timeline trận từ event log (P2).

### FR-7 Dữ liệu cá nhân & privacy (bổ sung sau gap-analysis — học sinh là trẻ vị thành niên, NĐ 13/2023/NĐ-CP)
- FR-7.1 Thông tin hiển thị của thí sinh (displayName, ảnh, trường/lớp) là **seat profile thuộc contest** — xoá contest là xoá profile; account chỉ giữ username.
- FR-7.2 Chức năng **anonymize contestant**: thay tên bằng mã trong dữ liệu trận cũ mà không phá event log; retention mặc định 12 tháng, config được (DEFERED D14).
- FR-7.3 Overlay/viewer có toggle "dùng nickname" per-contestant cho livestream chưa có consent phụ huynh; docs kèm mẫu consent tham khảo.
- FR-7.4 Trách nhiệm pháp lý dữ liệu thuộc đơn vị tự host; hệ thống cung cấp công cụ (ghi rõ trong docs triển khai).

## 5. Yêu cầu phi chức năng (NFR)

| # | Yêu cầu | Chỉ tiêu |
|---|---|---|
| NFR-1 | Độ trễ realtime | p95 event < 200ms (LAN) / < 500ms (Internet); buzzer công bằng theo server-ts |
| NFR-2 | Quy mô | ≤500 viewer/contest, ≤2 contest song song (D11a); kiến trúc scale ngang sẵn (Redis adapter, single-writer lease) |
| NFR-3 | Tin cậy | Kill instance/Redis giữa trận → trận phục hồi, không mất event đã công bố; soak 2h không leak |
| NFR-4 | Bảo mật (zero-trust — chốt 12/07) | Không rò đáp án qua bất kỳ API/socket nào (test tự động; viewer/thí sinh/overlay không hiển thị đáp án — chỉ admin+MC; **ngoại lệ: contest bật `revealAnswerAfterJudge` [default TẮT] cho luyện tập → đáp án đẩy xuống SAU khi chấm**); mọi event verify permission server-side; HttpOnly cookie; URL media có TTL; magic-bytes sniffing upload |
| NFR-4b | **Audit MỌI thao tác, MỌI role** (chốt 12/07) | AuditLog chung append-only (actor, action, target, ts, IP): auth, CRUD đề/bộ đề/contest, event trong trận, viewer join/kick, import/export, xem đáp án |
| NFR-5 | Accessibility | WCAG AA contrast; `prefers-reduced-motion`; viewer chỉnh cỡ chữ |
| NFR-6 | Stack (ràng buộc cứng) | BE: TS, **NestJS + Express adapter** (✅ D9 12/07 — đổi từ Fastify vì tương thích), Zod, Prisma+Postgres, Redis, Better-auth, Socket.IO · FE: React+Vite, MUI, Motion, Zustand, TanStack Query · Storage: MinIO · **Chỉ tiếng Việt, múi giờ UTC+7 thống nhất (✅ D2)** |
| NFR-7 | **2 hình thức triển khai** (chốt 12/07) | (a) Docker compose trên server Internet (đủ PG+Redis+MinIO, multi-instance); (b) **portable trên Windows cá nhân KHÔNG Docker, mạng LAN, 1 máy** — hạ tầng qua abstraction layer (driver Redis/in-process, MinIO/filesystem), đang research queue + hạ tầng portable |

## 6. Tiêu chí thành công v1

1. Tổ chức 1 trận UAT người thật trọn vẹn 4 vòng + livestream OBS không sự cố chặn trận.
2. Điểm số cuối trận khớp 100% với tính tay theo rule-spec cho ≥3 kịch bản automated.
3. Admin đổi luật (timer/điểm) qua UI, trận chạy theo giá trị mới không sửa code.
4. Bộ test "không rò đáp án" + chaos drills (kill instance, kill Redis, restore backup) pass.

## 7. Rủi ro & Phụ thuộc chính

- ~~NestJS Fastify × Socket.IO/Better-auth~~ ✅ đã hoá giải: D9 chốt Express adapter (12/07) — không còn rủi ro tương thích.
- Luật O26 có chi tiết chưa xác nhận (D8/D13) — mọi giá trị là config nên không chặn dev.
- Bản quyền: nhạc hiệu (D10) VÀ **format/tên "Olympia" thuộc VTV** — tên sản phẩm nên trung tính, tên vòng thi là data trong preset, disclaimer không liên kết VTV (D7); license repo chốt trước commit đầu (D7b).
- Dữ liệu cá nhân học sinh vị thành niên — xem FR-7 + DEFERED D14.

## 8. Lộ trình

10 phase trong `plan.md`; critical path là Phase 6 (game engine, có milestone 6a/6b). Demo design (`public/`) đã hoàn thành và verify — là design reference cho Phase 7-9.
