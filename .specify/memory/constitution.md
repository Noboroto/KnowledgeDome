<!--
SYNC IMPACT REPORT
==================
Version change: TEMPLATE (chưa khởi tạo) → 1.0.0 → 1.1.0 → 1.2.0 → 1.3.0
Bump rationale: Lần phê chuẩn đầu tiên — mọi placeholder được thay bằng nội dung cụ thể,
  toàn bộ nguyên tắc là mới (MAJOR-level khởi tạo, đánh số 1.0.0 theo quy ước ratification).

Modified principles:
  - [PRINCIPLE_1_NAME] → I. Tài liệu docs/ là nguồn sự thật duy nhất
  - [PRINCIPLE_2_NAME] → II. Mâu thuẫn tài liệu phải được phơi bày, không được tự hoà giải
  - [PRINCIPLE_3_NAME] → III. Thiếu thông tin quan trọng phải đánh dấu NEEDS CLARIFICATION
  - [PRINCIPLE_4_NAME] → IV. Requirement phải kiểm thử được
  - [PRINCIPLE_5_NAME] → V. User story viết theo góc nhìn người dùng
  - (thêm mới)         → VI. Mỗi user story phát triển và kiểm thử độc lập

Added sections:
  - Ràng buộc tài liệu & phạm vi (SECTION_2)
  - Quy trình sinh specification & cổng chất lượng (SECTION_3)
  - Governance

Removed sections: (không có)

Templates requiring updates:
  ✅ .specify/templates/spec-template.md — đã tương thích (user story P1/P2/P3 độc lập,
     "Independent Test", marker NEEDS CLARIFICATION, Success Criteria đo được). Không cần sửa.
  ✅ .specify/templates/plan-template.md — mục "Constitution Check" là placeholder mở,
     nhận gate từ file này. Không cần sửa.
  ✅ .specify/templates/tasks-template.md — phân loại task theo user story, khớp Nguyên tắc VI.
  ✅ .specify/templates/checklist-template.md — không tham chiếu nguyên tắc lỗi thời.
  ✅ .claude/skills/speckit-*/SKILL.md — dùng tên lệnh generic, không có tham chiếu
     agent-specific lỗi thời.

Deferred TODOs:
  - (không còn) TODO(DOCS_MIGRATION) đã ĐÓNG ở sửa đổi 1.3.0 — xem bên dưới.

AMENDMENT 1.1.0 (2026-07-23)
  - Làm rõ theo chỉ đạo chủ dự án: `plans/**` là bản nháp, KHÔNG được coi là `docs/` tạm thời.
  - Bump MINOR (không phải PATCH) vì thay đổi thực chất phạm vi nguồn có thẩm quyền:
    trước sửa đổi, `plans/**` được coi ngang tài liệu chính thức; sau sửa đổi, nó là nháp
    và mọi requirement lấy từ đó phải mang `[NEEDS CLARIFICATION]` hoặc được chủ dự án xác nhận.
  - Không nguyên tắc I–VI nào bị sửa; chỉ mục "Ràng buộc tài liệu & phạm vi" thay đổi.

AMENDMENT 1.2.0 (2026-07-23)
  - Thêm "Thứ bậc nguồn" 5 tầng (docs/source/ → docs/PRD.md → specs/<feature>/{spec,plan,tasks}.md)
    và quy tắc quyền sở hữu Spec Kit vs ClaudeKit. Định nghĩa đầy đủ nằm ở CLAUDE.md
    → "Product and Specification Workflow"; constitution chỉ trỏ tới, không sao chép.
  - Bump MINOR: mở rộng hướng dẫn đáng kể (làm rõ "docs/" gồm những gì và thứ tự ưu tiên),
    không sửa nguyên tắc I–VI.

AMENDMENT 1.3.0 (2026-07-29) — ĐÓNG TODO(DOCS_MIGRATION)
  Nguyên nhân: hai tiền đề của mục "Trạng thái chuyển tiếp" đã hết hiệu lực.
    (a) `docs/` NAY ĐÃ TỒN TẠI và có nội dung đầy đủ: `source/`, `decisions.md` (QĐ-001→092),
        `game-rules.md` (GR-001→037), `game-state-machine.md`, `glossary.md`, `traceability.md`,
        `product-discovery.md`, `PRD.md`, `roadmap-post-v1.md`, `reviews/`, `README.md`.
        `PRD.md` bản 2.2.0 khai **0 CONFLICT, 0 NEEDS CLARIFICATION** trên 102 requirement.
    (b) `plans/` ĐÃ BỊ XOÁ khỏi repo (2026-07-29) sau khi migrate xong; nội dung chưa migrate
        chỉ còn trong lịch sử git.

  Mục bị ảnh hưởng:
    - "Ràng buộc tài liệu & phạm vi" → GỠ tiểu mục "Trạng thái chuyển tiếp — TODO(DOCS_MIGRATION)",
      thay bằng "Nguồn đã migrate xong" nêu vị thế hiện hành của `plans/**` và `docs/reviews/**`.
    - "Quy trình sinh specification & cổng chất lượng" → Cổng 1 bỏ câu trỏ tới mục chuyển tiếp;
      diễn đạt lại vị thế của nguồn ngoài `docs/` cho khỏi phụ thuộc sự tồn tại của `plans/`.
    - KHÔNG nguyên tắc I–VI nào bị sửa.

  Bump MINOR (không phải PATCH) theo đúng chỉ dẫn tự đặt ở 1.2.0 dòng "Khi migrate xong…":
    phạm vi nguồn có thẩm quyền thay đổi thực chất — trước sửa đổi, một requirement được phép
    tồn tại dưới dạng marker trỏ về nháp; sau sửa đổi, đường đó KHÔNG còn, mọi requirement
    phải truy về `docs/`.

  Templates đã rà lại (mục (d) của quy tắc Sửa đổi): `.specify/templates/` — spec-template.md,
  plan-template.md, tasks-template.md, checklist-template.md, constitution-template.md
  đều KHÔNG tham chiếu `plans/**` hay trạng thái chuyển tiếp. Không cần sửa file nào.
-->

# KnowledgeDome Constitution

## Core Principles

### I. Tài liệu docs/ là nguồn sự thật duy nhất

Các file trong `docs/` là nguồn sự thật chính thức của sản phẩm. Mọi specification, plan,
task và implementation MUST truy nguyên được về một câu/đoạn cụ thể trong `docs/`.

- KHÔNG BAO GIỜ tự bịa thêm business requirement. Nếu một hành vi không có trong `docs/`,
  nó không phải requirement — nó là câu hỏi (xem Nguyên tắc III) hoặc là đề xuất phải được
  chủ dự án chấp thuận và ghi vào `docs/` TRƯỚC khi xuất hiện trong spec.
- Suy luận kỹ thuật (chọn thư viện, cấu trúc bảng, tên hàm) được phép; suy luận về
  *ý muốn nghiệp vụ* thì không.
- Mọi requirement trong spec SHOULD kèm con trỏ nguồn (đường dẫn file + mục) để người
  đọc kiểm chứng được.

**Lý do**: Đây là hệ thống thi đấu có tính thi cử — một luật chơi bịa ra sẽ tạo trận đấu
sai kết quả mà không ai phát hiện cho đến khi lên sóng.

### II. Mâu thuẫn tài liệu phải được phơi bày, không được tự hoà giải

Khi hai nguồn trong `docs/` nói ngược nhau, spec MUST ghi rõ điểm mâu thuẫn — trích cả hai
phía kèm đường dẫn — và MUST NOT tự chọn một phía rồi im lặng.

- Định dạng bắt buộc: `**CONFLICT**: <mô tả> — Nguồn A: <path §> nói "<trích>"; Nguồn B:
  <path §> nói "<trích>".`
- Mâu thuẫn chưa được chủ dự án phân xử là một blocker: requirement liên quan giữ trạng
  thái chưa chốt, KHÔNG được đưa vào task implementation.

**Lý do**: Hoà giải ngầm biến một bất đồng hiển thị thành một quyết định vô hình mà không
ai đã từng phê duyệt.

### III. Thiếu thông tin quan trọng phải đánh dấu NEEDS CLARIFICATION

Thông tin quan trọng nhưng thiếu MUST được đánh dấu `[NEEDS CLARIFICATION: <câu hỏi cụ thể>]`
ngay tại vị trí requirement, thay vì điền giá trị mặc định thầm lặng.

- "Quan trọng" = nếu đoán sai thì kết quả công việc sai hoặc phải làm lại: luật tính điểm,
  quyền hạn, ngưỡng thời gian, hành vi biên, retention, quy tắc phân xử.
- Giá trị mặc định hợp lý cho chi tiết KHÔNG quan trọng được phép, nhưng MUST ghi vào mục
  "Assumptions" của spec.
- Một câu hỏi mỗi marker, và câu hỏi MUST trả lời được bằng một câu — không marker mơ hồ
  kiểu "cần làm rõ thêm".

**Lý do**: Giả định không ghi lại là nợ kỹ thuật vô hình; giả định có ghi lại là một câu hỏi
chờ trả lời.

### IV. Requirement phải kiểm thử được

Mỗi functional requirement MUST phát biểu ở dạng quan sát được và có tiêu chí đạt/không đạt
rõ ràng.

- Dùng MUST / MUST NOT / SHOULD với chủ ngữ cụ thể ("Hệ thống MUST…", "Thí sinh MUST be able to…").
- CẤM từ ngữ không đo được nếu không kèm ngưỡng: "nhanh", "mượt", "thân thiện", "tối ưu",
  "hợp lý". Thay bằng số + đơn vị + điều kiện đo.
- Requirement nào không viết được Acceptance Scenario `Given / When / Then` thì chưa phải
  requirement — hoặc làm rõ (Nguyên tắc III), hoặc loại khỏi spec.
- Khi phải chọn giữa hai cách diễn đạt, ưu tiên cách kiểm thử được, kể cả khi hẹp hơn.

**Lý do**: Requirement không kiểm thử được không thể biết là đã hoàn thành hay chưa.

### V. User story viết theo góc nhìn người dùng

User story MUST mô tả điều một con người muốn đạt được, KHÔNG MUST NOT mô tả component
kỹ thuật hay bước triển khai.

- Chủ ngữ là một vai người dùng có thật trong sản phẩm (thí sinh, MC, admin, setter,
  viewer, trainer), không phải "service", "module", "endpoint", "bảng DB".
- Tiêu đề story nói về kết quả người dùng nhận được, không phải về công nghệ dùng để làm ra nó.
- Chi tiết kỹ thuật thuộc về plan/tasks, không thuộc về spec.

**Lý do**: Story viết theo component sẽ được nghiệm thu theo component — code chạy đúng
nhưng người dùng vẫn không làm được việc của họ.

### VI. Mỗi user story phát triển và kiểm thử độc lập

Mỗi user story MUST là một lát cắt dọc: có thể phát triển, kiểm thử, demo độc lập với
các story khác.

- Story MUST có mục "Independent Test" nêu rõ cách nghiệm thu nó khi CHƯA có story nào khác.
- Story MUST được gán độ ưu tiên (P1, P2, P3…); riêng P1 khi hoàn thành phải tự nó tạo ra
  một MVP dùng được.
- Nếu story A chỉ có nghĩa khi story B đã xong, chúng là MỘT story — gộp lại, hoặc cắt lại
  ranh giới cho đến khi mỗi phần tự đứng được.

**Lý do**: Story độc lập cho phép giao hàng theo lát, dừng đúng lúc, và biết chắc phần đã
làm là phần chạy được.

## Ràng buộc tài liệu & phạm vi

- **Thư mục nguồn sự thật**: `docs/`. Nguyên tắc I áp cho toàn bộ nội dung trong đó.
  Chỉ nội dung đã nằm trong `docs/` mới là requirement chính thức.
- **Thứ bậc nguồn** (định nghĩa đầy đủ tại `CLAUDE.md` → "Product and Specification Workflow";
  KHÔNG sao chép lại ở đây để tránh hai bản sự thật):
  `docs/source/` (tài liệu gốc, chỉ đọc) → `docs/PRD.md` (requirement cấp sản phẩm, truy được
  về `docs/source/`) → `specs/<feature>/spec.md` → `plan.md` → `tasks.md`.
  Tầng dưới MUST truy nguyên về tầng trên; `spec.md` thắng mọi mô tả không chính thức trong
  chat hoặc plan. Spec Kit sở hữu `specs/**`; KHÔNG tạo plan song song ngoài Spec Kit cho
  feature đã do Spec Kit quản lý, trừ khi được yêu cầu rõ ràng.

### Nguồn đã migrate xong (2026-07-29)

`docs/` đã có nội dung đầy đủ và là nguồn thẩm quyền **duy nhất**. Bản đồ tài liệu nằm ở
`docs/README.md`; KHÔNG sao chép lại ở đây.

- **KHÔNG còn đường nào đưa nội dung ngoài `docs/` vào spec.** Trước đây một requirement được
  phép tồn tại tạm dưới dạng marker trỏ về nháp; đường đó đã ĐÓNG. Nội dung không nằm trong
  `docs/` thì hoặc là câu hỏi (Nguyên tắc III), hoặc là đề xuất phải được chủ dự án chấp thuận
  và ghi vào `docs/` TRƯỚC khi vào spec (Nguyên tắc I). Không có đường thứ ba.
- **`docs/reviews/**` là KHO LƯU, không phải nguồn.** Nó ghi lại các đợt rà soát và đề xuất
  `GRR-*`. Dùng được để *phát hiện* câu hỏi và mâu thuẫn; MUST NOT trích như requirement.
  Cùng vị thế: `public/` (demo tĩnh, làm trước tài liệu nên có chỗ lệch — lệch thì demo sai).
- **`plans/**` đã bị XOÁ khỏi repo.** Nó từng là bản nháp planning và chưa bao giờ là nguồn.
  Phần chưa migrate — spec RuleConfig v2, kiến trúc kỹ thuật, kế hoạch theo phase, red-team,
  khảo sát UX, hồ sơ portable — chỉ còn trong lịch sử git. Lấy ra tham khảo **kỹ thuật** thì
  được; trích như requirement thì MUST NOT, y như trước khi xoá.
- **Mật độ marker `[NEEDS CLARIFICATION]` trong spec nay là TÍN HIỆU, không còn là trạng thái
  nền.** `docs/PRD.md` bản 2.2.0 khai 0 CONFLICT và 0 NEEDS CLARIFICATION trên 102 requirement.
  Một spec sinh ra với nhiều marker nghĩa là **hoặc** feature đó chạm vào vùng `docs/` thật sự
  chưa phủ, **hoặc** người viết spec chưa đọc hết `docs/` — cả hai đều đáng dừng lại kiểm,
  khác hẳn giai đoạn trước khi marker dày là chuyện bình thường.

### CLAUDE.md

`CLAUDE.md` là ràng buộc KỸ THUẬT có hiệu lực ngay (không phải nháp, không phải business
requirement): các quy ước bắt buộc toàn repo — UX, zero-trust security, server-authoritative,
DRY, i18n/timezone, font, conventional commits. Spec MUST tham chiếu tới nó, MUST NOT sao
chép lại nội dung — sao chép tạo hai bản sự thật.
- **Ngôn ngữ**: mọi spec, plan, task và tài liệu trong repo viết bằng tiếng Việt.

## Quy trình sinh specification & cổng chất lượng

Cổng bắt buộc, kiểm tra trước khi một spec được coi là sẵn sàng cho `/speckit-plan`:

1. **Cổng truy nguyên**: mọi FR truy về được `docs/`, hoặc mang marker
   `[NEEDS CLARIFICATION]`, hoặc nằm trong mục "Assumptions". Không có loại thứ tư.
   Trích dẫn nguồn NGOÀI `docs/` KHÔNG thoả cổng này — gồm `docs/reviews/**`, `public/`,
   lịch sử git, và ghi chú trong chat (xem "Nguồn đã migrate xong").
2. **Cổng mâu thuẫn**: mọi mâu thuẫn phát hiện được đã ghi theo định dạng `**CONFLICT**`;
   không FR nào phụ thuộc vào mâu thuẫn chưa phân xử.
3. **Cổng kiểm thử được**: mọi FR có ít nhất một Acceptance Scenario `Given/When/Then`;
   không còn tính từ không đo được.
4. **Cổng góc nhìn**: mọi user story có chủ ngữ là vai người dùng; không có tên component,
   endpoint, bảng DB trong tiêu đề story.
5. **Cổng độc lập**: mọi story có mục "Independent Test" và độ ưu tiên; P1 tự nó là MVP.
6. **Cổng đo được**: mục Success Criteria có metric không phụ thuộc công nghệ, kèm số và đơn vị.

Spec không qua đủ 6 cổng MUST NOT chuyển sang giai đoạn plan. Marker `[NEEDS CLARIFICATION]`
còn tồn đọng không chặn việc *viết* spec, nhưng chặn việc implement phần requirement mang marker.

## Governance

- Constitution này có hiệu lực cao hơn mọi thói quen, template và tiện lợi ngắn hạn. Khi
  hướng dẫn khác mâu thuẫn với nó, constitution thắng — và mâu thuẫn đó phải được báo cáo
  theo Nguyên tắc II.
- **Sửa đổi**: mọi thay đổi MUST (a) ghi rõ nguyên tắc/mục bị ảnh hưởng, (b) bump version
  theo quy tắc dưới, (c) cập nhật Sync Impact Report ở đầu file, (d) rà lại các template
  trong `.specify/templates/` để giữ đồng bộ.
- **Versioning** (semantic):
  - MAJOR — gỡ bỏ hoặc định nghĩa lại nguyên tắc theo hướng không tương thích ngược.
  - MINOR — thêm nguyên tắc/mục mới, hoặc mở rộng hướng dẫn một cách đáng kể.
  - PATCH — làm rõ câu chữ, sửa lỗi, tinh chỉnh không đổi ngữ nghĩa.
- **Tuân thủ**: mọi spec review và code review MUST kiểm 6 cổng ở mục trên. Vi phạm được
  chấp nhận chỉ khi ghi rõ lý do trong mục "Complexity Tracking" của plan, kèm phương án
  đơn giản hơn đã bị loại và vì sao.
- **Hướng dẫn vận hành hằng ngày**: `CLAUDE.md` (quy ước code, UX, bảo mật, commit).

**Version**: 1.3.0 | **Ratified**: 2026-07-23 | **Last Amended**: 2026-07-29
