# Tài liệu KnowledgeDome

Nền tảng tổ chức thi đấu gameshow kiến thức tuỳ biến theo mô hình *Đường lên đỉnh Olympia* (luật O26).

Thư mục này là **nguồn sự thật của dự án**. Mọi thứ ngoài nó — `public/`, ghi chú trong chat — đều **không** phải requirement.

> **`plans/` đã bị xoá ngày 2026-07-29** sau khi migrate xong. Nó từng là bản nháp planning và chưa bao giờ là nguồn. Phần chưa migrate *(spec RuleConfig v2, kiến trúc kỹ thuật, kế hoạch theo phase, red-team, khảo sát UX, hồ sơ portable)* chỉ còn trong **lịch sử git**; tài liệu trong `reviews/` và `source/` vẫn nhắc tới đường dẫn cũ vì chúng ghi lại **lịch sử**, không phải chỉ chỗ đọc.

---

## Bắt đầu từ đâu

| Bạn muốn... | Đọc |
|---|---|
| Biết **v1 phải làm gì** — mục tiêu, actor, epic, yêu cầu | [`PRD.md`](PRD.md) |
| Biết **cái gì để lại cho v1.5 / v2** | [`roadmap-post-v1.md`](roadmap-post-v1.md) |
| Hiểu **luật chơi** hệ thống thực thi | [`game-rules.md`](game-rules.md) |
| Biết **vì sao** một điều được quyết như vậy | [`decisions.md`](decisions.md) |
| Cài đặt **luồng vận hành** — trạng thái, sự kiện, chuyển tiếp | [`game-state-machine.md`](game-state-machine.md) |
| Tra **một thuật ngữ** | [`glossary.md`](glossary.md) |
| Biết **ai được làm gì** — permission, vai, phạm vi | [`permissions.md`](permissions.md) |
| Truy nguyên *"điều này từ đâu ra?"* | [`traceability.md`](traceability.md) |
| Đọc **luật gốc nguyên văn** | [`source/`](source/) |
| Biết **giao diện phải làm gì** để đúng các quyết định | [`product-discovery.md`](product-discovery.md) §6 |
| Lục lại **quá trình thảo luận** | [`reviews/`](reviews/) — **kho lưu, không phải nguồn** |

---

## Từng tài liệu

### [`source/`](source/) — luật gốc

Bản lưu nguyên văn [Luật chơi/Olympia 26](https://duong-len-dinh-olympia.fandom.com/vi/wiki/Lu%E1%BA%ADt_ch%C6%A1i/Olympia_26) (Fandom), lấy 2026-07-23. **Source of truth duy nhất về LUẬT.**

> ⚠️ **Không sửa file trong thư mục này** trừ khi được yêu cầu rõ ràng.

### [`decisions.md`](decisions.md) — vì sao

**103 quyết định `QĐ-001` → `QĐ-103`.** Mỗi mục trả lời ba câu: quyết định là gì · vì sao chọn nó và bác cái gì · hệ quả kéo theo.

Chia theo chủ đề: nguyên tắc nền · phạm vi phiên bản · điểm và event log · hai mode trả lời · tín hiệu và hàng đợi · đồng hồ · điều khiển trận · kho đề · ghế và kết nối · hiển thị và bảo mật · theo vòng · mô hình dữ liệu và quyền.

Có **bảng tra mã CŨ → MỚI** ở §M cho ai đang cầm mã `Đ-*`, `U-*`, `K-*`, `GRR-*` từ tài liệu cũ. §N ghi một mục **đã hoãn có chủ đích** — không còn mục treo nào.

### [`game-rules.md`](game-rules.md) — luật chơi

**37 rule `GR-001` → `GR-037`**, phủ 5 vòng thi và luật xuyên vòng. Đặc tả **nghiệp vụ**: không code, không tên bảng dữ liệu, không tên framework.

Mở đầu bằng **23 nguyên tắc nền** và **bốn bảng dùng chung** mà nhiều rule cùng đọc. Mỗi rule có bảng quyết định, biên, ví dụ có số, và dẫn về `QĐ-*`.

### [`game-state-machine.md`](game-state-machine.md) — máy trạng thái

Trạng thái `STATE-*` · sự kiện `EVENT-*` · chuyển tiếp `T-*` · bất biến `INV-*`, kèm 8 sơ đồ. Đây là tài liệu để **cài đặt engine**.

`game-rules.md` nói *cái gì đúng*; file này nói *hệ thống đi qua những bước nào*. Hai bên phải khớp — lệch nhau thì `game-rules.md` đúng về luật, file này đúng về vận hành.

### [`glossary.md`](glossary.md) — thuật ngữ

**62 thuật ngữ `TERM-001` → `TERM-062`**, kèm mục **Đừng nhầm với** cho những từ nhiều nghĩa. Có bảng tra tên tiếng Anh.

Dùng khi bạn thấy một từ trong tài liệu khác mà không chắc nó chỉ đúng cái gì.

### [`permissions.md`](permissions.md) — catalog phân quyền

**61 permission `PERM-001` → `PERM-061`**, chia theo phạm vi `HỆ THỐNG` và `CONTEST`, kèm **bốn vai seed**.

Là **catalog tra cứu**, cùng loại với `glossary.md` — nó **không đặt ra luật**. Mô hình RBAC và lý do nằm ở `decisions.md` `QĐ-094`.

Ba mục đáng đọc trước khi cài đặt: **§4 bốn cổng đứng NGOÀI RBAC** *(có permission là điều kiện cần, không phải đủ)* · **§5 bốn điều CẤM** — dẫn đầu là *cấm kiểm vai thay cho kiểm permission* · **§7 vòng đời phép cấp** — trong lúc trận chưa đóng sổ, quyền chỉ **nở ra**, không bao giờ **co lại**.

### [`traceability.md`](traceability.md) — truy nguyên

Ma trận **rule ↔ luật gốc ↔ quyết định**. Trả lời *"câu này của luật gốc thành rule nào"* và ngược lại.

Còn giữ hai danh sách quan trọng: **biến thể bị loại** (để không cài nhầm) và **biến thể ngoài luật O26** (cấu hình được nhưng không bật mặc định).

### [`product-discovery.md`](product-discovery.md) — cấp sản phẩm

Vấn đề, người dùng, mục tiêu, hành trình, epic.

Phần đáng dùng nhất là **§6 — giao diện phải làm gì**: danh sách kiểm cho màn admin, màn thí sinh và viewer, mỗi dòng dẫn về `QĐ-*` ràng buộc nó. Đây cũng là nơi giữ giả định chưa kiểm chứng và cách đo thành công.

### [`PRD.md`](PRD.md) — yêu cầu cấp sản phẩm cho **phiên bản 1.0**

Mục tiêu sản phẩm, actor, hành trình, epic, **107 yêu cầu `PRD-REQ-*`**, NFR, ma trận truy nguyên. **Không còn câu hỏi mở nào ở mức chặn nghiệm thu.**

**Đặc tả đúng bằng phiên bản 1.0** — không nói về phiên bản nào khác.

**Mục đã chốt thì bị XOÁ, không giữ làm ghi chú lịch sử.** Câu hỏi đã đóng, mâu thuẫn đã sửa, giả định đã bác, rủi ro đã xử lý — tất cả rời tài liệu, và dãy số liên quan được **đánh lại cho liền**. Hệ quả: mã định danh là **chỉ mục của bản hiện hành**, nên trích dẫn `PRD-REQ-*` / `QUESTION-*` / `METRIC-*` từ nơi khác phải **kèm tên** hạng mục. Lịch sử tra ở `git log` và [`reviews/`](reviews/).

### [`roadmap-post-v1.md`](roadmap-post-v1.md) — hạng mục ngoài phạm vi phiên bản 1.0

`ACTOR-007`, `JOURNEY-008`, `EPIC-013` luyện tập, `EPIC-014` thi đội, `PRD-REQ-099` → `103`, ba câu hỏi mở `QUESTION-009` → `011`, và danh sách hạng mục **chưa gắn mốc**. Đánh số **nối tiếp** PRD nên hai tài liệu không bao giờ trùng mã.

**Không phải requirement đang thi hành.** Không mục nào ở đây vào `/speckit.specify` cho tới khi chủ dự án mở phạm vi phiên bản tương ứng — khi đó mục chuyển ngược về `PRD.md`.

### [`reviews/`](reviews/) — kho lưu

Biên bản rà soát và đề xuất `GRR-*` của giai đoạn 2026-07. **Không trích như requirement.** Xem [`reviews/README.md`](reviews/README.md).

---

## Quy ước đọc

**Thứ bậc nguồn.** `source/` → `decisions.md` → `game-rules.md` và `game-state-machine.md`. Trên thắng dưới. Không có nguồn nào ngoài `docs/`.

**Một chiều.** Tài liệu đặc tả nói **cái đang là** và dẫn về `QĐ-*`; `decisions.md` **không** dẫn ngược lại mã đặc tả. Nhờ vậy đổi số hiệu rule không làm hỏng sổ quyết định.

**Ba tầng nguồn.** `[LUẬT GỐC]` không ai đổi được · `[CHỦ DỰ ÁN]` chủ dự án đổi được · `[SUY RA]` đổi được nhưng phải đổi cả tiền đề.

**Chỉ mô tả cái hệ thống CÓ.** Nhánh không tồn tại thì không có mục cho nó. Ngoại lệ có chủ ý là dòng **Không đổi gì** và trường **Cấm** — chúng là **yêu cầu phủ định**, chặn một hiện thực sai dễ xảy ra.

**Hệ mã đang dùng**

| Mã | Ở đâu | Là gì |
|---|---|---|
| `QĐ-001` → `QĐ-103` | `decisions.md` | Quyết định |
| `GR-001` → `GR-037` | `game-rules.md` | Rule luật chơi |
| `TERM-001` → `TERM-062` | `glossary.md` | Thuật ngữ |
| `PERM-001` → `PERM-061` | `permissions.md` | Permission |
| `STATE-*` `EVENT-*` `T-*` `INV-*` | `game-state-machine.md` | Trạng thái · sự kiện · chuyển tiếp · bất biến |
| `GRR-*` | **chỉ** `reviews/` | Đề xuất trong quá trình rà soát |

Hệ mã cũ — `Đ-*`, `U-*`, `K-*`, `R-*`, `S-*` — **đã ngừng dùng**. Tra sang mã mới ở `decisions.md` §M.
