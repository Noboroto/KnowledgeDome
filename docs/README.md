# Tài liệu KnowledgeDome

Nền tảng tổ chức thi đấu gameshow kiến thức tuỳ biến theo mô hình *Đường lên đỉnh Olympia* (luật O26).

Thư mục này là **nguồn sự thật của dự án**. Mọi thứ ngoài nó — `plans/**`, `public/`, ghi chú trong chat — đều **không** phải requirement.

---

## Bắt đầu từ đâu

| Bạn muốn... | Đọc |
|---|---|
| Hiểu **luật chơi** hệ thống thực thi | [`game-rules.md`](game-rules.md) |
| Biết **vì sao** một điều được quyết như vậy | [`decisions.md`](decisions.md) |
| Cài đặt **luồng vận hành** — trạng thái, sự kiện, chuyển tiếp | [`game-state-machine.md`](game-state-machine.md) |
| Tra **một thuật ngữ** | [`glossary.md`](glossary.md) |
| Truy nguyên *"điều này từ đâu ra?"* | [`traceability.md`](traceability.md) |
| Đọc **luật gốc nguyên văn** | [`source/`](source/) |
| Xem câu hỏi **cấp sản phẩm** ngoài phạm vi luật chơi | [`product-discovery.md`](product-discovery.md) |
| Lục lại **quá trình thảo luận** | [`reviews/`](reviews/) — **kho lưu, không phải nguồn** |

---

## Từng tài liệu

### [`source/`](source/) — luật gốc

Bản lưu nguyên văn [Luật chơi/Olympia 26](https://duong-len-dinh-olympia.fandom.com/vi/wiki/Lu%E1%BA%ADt_ch%C6%A1i/Olympia_26) (Fandom), lấy 2026-07-23. **Source of truth duy nhất về LUẬT.**

> ⚠️ **Không sửa file trong thư mục này** trừ khi được yêu cầu rõ ràng.

### [`decisions.md`](decisions.md) — vì sao

**71 quyết định `QĐ-001` → `QĐ-071`.** Mỗi mục trả lời ba câu: quyết định là gì · vì sao chọn nó và bác cái gì · hệ quả kéo theo.

Chia theo chủ đề: nguyên tắc nền · phạm vi phiên bản · điểm và event log · hai mode trả lời · tín hiệu và hàng đợi · đồng hồ · điều khiển trận · kho đề · ghế và kết nối · hiển thị và bảo mật · theo vòng · mô hình dữ liệu và quyền.

Có **bảng tra mã CŨ → MỚI** ở §M cho ai đang cầm mã `Đ-*`, `U-*`, `K-*`, `GRR-*` từ tài liệu cũ. §N ghi một mục **đã hoãn có chủ đích** — không còn mục treo nào.

### [`game-rules.md`](game-rules.md) — luật chơi

**37 rule `GR-001` → `GR-037`**, phủ 5 vòng thi và luật xuyên vòng. Đặc tả **nghiệp vụ**: không code, không tên bảng dữ liệu, không tên framework.

Mở đầu bằng **23 nguyên tắc nền** và **bốn bảng dùng chung** mà nhiều rule cùng đọc. Mỗi rule có bảng quyết định, biên, ví dụ có số, và dẫn về `QĐ-*`.

### [`game-state-machine.md`](game-state-machine.md) — máy trạng thái

Trạng thái `STATE-*` · sự kiện `EVENT-*` · chuyển tiếp `T-*` · bất biến `INV-*`, kèm 8 sơ đồ. Đây là tài liệu để **cài đặt engine**.

`game-rules.md` nói *cái gì đúng*; file này nói *hệ thống đi qua những bước nào*. Hai bên phải khớp — lệch nhau thì `game-rules.md` đúng về luật, file này đúng về vận hành.

### [`glossary.md`](glossary.md) — thuật ngữ

**56 thuật ngữ `TERM-001` → `TERM-056`**, kèm mục **Đừng nhầm với** cho những từ nhiều nghĩa. Có bảng tra tên tiếng Anh.

Dùng khi bạn thấy một từ trong tài liệu khác mà không chắc nó chỉ đúng cái gì.

### [`traceability.md`](traceability.md) — truy nguyên

Ma trận **rule ↔ luật gốc ↔ quyết định**. Trả lời *"câu này của luật gốc thành rule nào"* và ngược lại.

Còn giữ hai danh sách quan trọng: **biến thể bị loại** (để không cài nhầm) và **biến thể ngoài luật O26** (cấu hình được nhưng không bật mặc định).

### [`product-discovery.md`](product-discovery.md) — cấp sản phẩm

Vấn đề, người dùng, mục tiêu, hành trình, epic, và những chỗ requirement còn mơ hồ. Đây là nơi chứa câu hỏi **không thuộc luật chơi** — vận hành, triển khai, quyền hạn, phạm vi MVP.

### [`reviews/`](reviews/) — kho lưu

Biên bản rà soát và đề xuất `GRR-*` của giai đoạn 2026-07. **Không trích như requirement.** Xem [`reviews/README.md`](reviews/README.md).

---

## Quy ước đọc

**Thứ bậc nguồn.** `source/` → `decisions.md` → `game-rules.md` và `game-state-machine.md`. Trên thắng dưới. `plans/**` là **bản nháp**, không thắng thứ gì.

**Một chiều.** Tài liệu đặc tả nói **cái đang là** và dẫn về `QĐ-*`; `decisions.md` **không** dẫn ngược lại mã đặc tả. Nhờ vậy đổi số hiệu rule không làm hỏng sổ quyết định.

**Ba tầng nguồn.** `[LUẬT GỐC]` không ai đổi được · `[CHỦ DỰ ÁN]` chủ dự án đổi được · `[SUY RA]` đổi được nhưng phải đổi cả tiền đề.

**Chỉ mô tả cái hệ thống CÓ.** Nhánh không tồn tại thì không có mục cho nó. Ngoại lệ có chủ ý là dòng **Không đổi gì** và trường **Cấm** — chúng là **yêu cầu phủ định**, chặn một hiện thực sai dễ xảy ra.

**Hệ mã đang dùng**

| Mã | Ở đâu | Là gì |
|---|---|---|
| `QĐ-001` → `QĐ-071` | `decisions.md` | Quyết định |
| `GR-001` → `GR-037` | `game-rules.md` | Rule luật chơi |
| `TERM-001` → `TERM-056` | `glossary.md` | Thuật ngữ |
| `STATE-*` `EVENT-*` `T-*` `INV-*` | `game-state-machine.md` | Trạng thái · sự kiện · chuyển tiếp · bất biến |
| `GRR-*` | **chỉ** `reviews/` | Đề xuất trong quá trình rà soát |

Hệ mã cũ — `Đ-*`, `U-*`, `K-*`, `R-*`, `S-*` — **đã ngừng dùng**. Tra sang mã mới ở `decisions.md` §M.
