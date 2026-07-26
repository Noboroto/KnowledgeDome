# Luật chơi & sản phẩm — ĐỀ XUẤT PHÂN XỬ cho các mục còn chờ

> **Ngày lập**: 2026-07-26
>
> **Mục đích**: phân xử toàn bộ mục còn treo ở `docs/reviews/game-rules-open-questions.md` (Tầng 1 → 7) và `docs/product-discovery.md` §6 (C-1 → C-17, S-1 → S-20, Q-A/Q-B/Q-C).
>
> **Trạng thái của chính file này**: **`[ĐỀ XUẤT]` — CHƯA phải quyết định đã chốt.** Theo `.specify/memory/constitution.md`, chỉ chủ dự án mới chuyển một mục sang `[CHỐT]`. Sau khi duyệt, nội dung ở đây sẽ được gộp vào `game-rules-decisions.md` (phần luật) và `product-discovery.md` (phần điều khiển hệ thống), và mục tương ứng bị xoá khỏi `game-rules-open-questions.md`.
>
> **Ba mức tin cậy**:
> - **`SUY RA`** — hệ quả logic bắt buộc của một quyết định đã chốt. Không duyệt thì tài liệu **tự mâu thuẫn**.
> - **`ĐỀ XUẤT`** — có phương án khác cũng hợp lý; đã nêu lý do chọn.
> - **`KHÔNG QUYẾT`** — dữ kiện chỉ chủ dự án biết. Không suy diễn.

---

## Mục lục

- [0. Tóm tắt — 4 nguyên tắc phái sinh](#0-tóm-tắt--4-nguyên-tắc-phái-sinh)
- [0.5. Bảng gộp — mục đã được quyết định SẴN CÓ trả lời](#05-bảng-gộp--mục-đã-được-quyết-định-sẵn-có-trả-lời)
- [1. Mục ĐÃ CHẾT vì quyết định sau đè lên](#1-mục-đã-chết-vì-quyết-định-sau-đè-lên)
- [2. Tầng 2 — Quy ước biên thời gian](#2-tầng-2--quy-ước-biên-thời-gian)
- [3. Tầng 3 — Phạm vi v1](#3-tầng-3--phạm-vi-v1)
- [4. Tầng 1 — Ba danh sách đang chặn](#4-tầng-1--ba-danh-sách-đang-chặn)
- [5. Tầng 4 — Duyệt các đề xuất chờ](#5-tầng-4--duyệt-các-đề-xuất-chờ)
- [6. Tầng 5 — Con số và chính sách](#6-tầng-5--con-số-và-chính-sách)
- [7. Tầng 7 — Câu hỏi nhỏ đi kèm quyết định đã chốt](#7-tầng-7--câu-hỏi-nhỏ-đi-kèm-quyết-định-đã-chốt)
- [8. product-discovery §6 — C-1 → C-17](#8-product-discovery-6--c-1--c-17)
- [9. product-discovery — S-1 → S-20](#9-product-discovery--s-1--s-20)
- [10. KHÔNG QUYẾT — cần chủ dự án](#10-không-quyết--cần-chủ-dự-án)
- [11. Căn cứ UX bên ngoài](#11-căn-cứ-ux-bên-ngoài)

---

## 0. Tóm tắt — 4 nguyên tắc phái sinh

Phần lớn 120 mục treo **không cần 120 quyết định**. Chúng rơi vào bốn nguyên tắc, mỗi nguyên tắc suy ra từ những gì đã chốt. Duyệt bốn nguyên tắc này là đóng được ~70% backlog.

### NT-A — Biên thời gian là NỬA MỞ `[start, end)`

Cửa sổ thời gian tính từ mốc admin bấm, **bao gồm** mốc mở, **không bao gồm** mốc đóng. Tín hiệu ở đúng mốc hết giờ (chênh 0 ms) là **quá hạn**.

> Đây là quyết định **rẻ** chứ không phải khắc nghiệt, vì `Đ-28`/điểm 21 đã gỡ ngòi nổ: quá hạn **không bị loại**, chỉ **tô đỏ** và admin phán quyết. Nếu bản quá hạn bị máy vứt đi thì mới phải cân nhắc chọn biên đóng.

Đóng: `GRR-002`, `GRR-035`, một nửa `GRR-031`.

### NT-B — Mốc thời gian nào cũng là MỘT NÚT của admin

`Đ-26` (hiển thị câu / start timer) và `Đ-33` (công bố đáp án) không phải hai ngoại lệ — chúng là **một mẫu chung**: *mọi mốc mà luật gốc mô tả bằng hành vi con người đều là một nút bấm của admin, cùng mẫu hiển thị, cùng vào audit log.*

Mọi câu hỏi dạng *"lúc nào thì X xuất hiện / chuyển pha"* trả lời bằng **"khi admin bấm"**, không cần luật mới.

Đóng: `GRR-028` (gợi ý ký tự), `GRR-050/051` (chuyển pha thực hành), `C-8` (ai bấm bắt đầu 3s), `Đ-5.3.1a` (chốt câu Tăng tốc), `GRR-054` (hạn chót đổi gói).

### NT-C — Cấu hình được SNAPSHOT vào match lúc start

Contest là **bản thiết kế**; match là **một lần chạy**. Mọi tham số luật — RuleConfig, mode trả lời, danh sách câu đã gán — được sao chép vào match tại thời điểm start và **không đổi theo contest nữa**.

Mọi câu hỏi dạng *"sửa cấu hình giữa chừng thì trận đã chạy ra sao"* trả lời bằng **"trận đã chạy giữ bản snapshot của nó"**.

Đóng: `Q-C1b`, `GRR-104` (nửa sau), `GRR-103`, `C-9` (một phần).

### NT-D — Chỉ có BA chỗ chặn cứng; mọi chỗ khác là cảnh báo ép được

1. Ngưỡng tối thiểu vật lý (`Đ-15.3`) — Về đích cửa sổ cướp ≥2, Câu hỏi phụ ≥2.
2. Kho đề tại cửa vào từng vòng (`Đ-31`).
3. Thao tác ở **invalid state** (điểm 9) — không phải "chặn" mà là **không tồn tại đường vào**.

Không thêm chỗ thứ tư. Mọi câu hỏi *"cái này chặn hay cảnh báo"* mặc định là **cảnh báo**.

Đóng: `Đ-15.c`, `Đ-5.1i`, `Đ-0.1a`, `§0.2 S-1`, `§0.2 S-2`, `GRR-081/084/090`.

---

## 0.5. Bảng gộp — mục đã được quyết định SẴN CÓ trả lời

> **Thứ tự ưu tiên khi phân xử** — áp cho mọi mục trong file này:
>
> 1. **Quyết định đã chốt** (`game-rules-decisions.md` Đ-1 → Đ-36 · `game-rules.md` nguyên tắc nền 1 → 23 · `CLAUDE.md`) — nếu một mục đã có câu trả lời ở đây thì **không** phân xử lại.
> 2. **Câu trùng pattern đã xử ở mục khác** — trả lời bằng cách **chỉ sang** mục đó, không viết luật thứ hai (DRY ở tầng tài liệu).
> 3. **Suy luận bắt buộc** từ (1) và (2).
> 4. **Chỉ khi cả ba đường trên cạn** mới đưa ra lựa chọn mới, và khi đó mới cần căn cứ UX bên ngoài.

**Kết quả của thứ tự đó:**

| Loại | Số mục | Chủ dự án phải làm gì |
|---|---|---|
| **Đã chết** — quyết định sau đè lên | ≈10 | Xoá khỏi backlog (§1) |
| **`SUY RA`** — hệ quả bắt buộc của cái đã chốt | ≈75 | Đọc lướt; **không duyệt thì tài liệu tự mâu thuẫn** |
| **Duyệt nguyên văn** — Tầng 4 đã có sẵn lập luận | 39 | Bấm duyệt cả lô (§5.1) |
| **`ĐỀ XUẤT`** — chỗ thật sự phải chọn | **≈22** | **Đọc kỹ. Đây là toàn bộ việc còn lại** |
| **`KHÔNG QUYẾT`** | 7 | Cần dữ kiện ngoài tài liệu (§10) |

### Bảy pattern gộp được nhiều câu hỏi

Mỗi dòng dưới đây là **một** câu trả lời đã tồn tại, bị hỏi lại dưới nhiều mã khác nhau.

| Pattern | Quyết định gốc | Các mã được đóng theo | Ghi ở |
|---|---|---|---|
| **P1 — Mốc nào cũng là một nút của admin** | `Đ-26`, `Đ-33`, nguyên tắc nền điểm 2 | `GRR-028` · `GRR-050/051` · `GRR-054` · `Đ-5.3.1a` · `C-8` | NT-B |
| **P2 — Cấu hình snapshot vào match lúc start** | `D21` (`matchPurpose` per-match) + spec §13 | `Q-C1b` · `GRR-104`(b) · `GRR-103`(phần kỹ thuật) · `Đ-4.5a`(phần cơ chế) | NT-C |
| **P3 — Chỉ ba chỗ chặn cứng, còn lại là cảnh báo ép được** | `Đ-15.3` · `Đ-31` · nguyên tắc nền điểm 9 | `Đ-15.c` · `Đ-5.1i` · `Đ-0.1a` · `§0.2 S-1` · `§0.2 S-2` · `GRR-081/084/090` · `GRR-120` · `Đ-4.e3` | NT-D |
| **P4 — Kho đề kiểm tại cửa vào từng vòng** | `Đ-31` | `Đ-5.g` · `S-12` · `Đ-5.1g` · `Đ-5.1h` · `GRR-087` · `GRR-091` · `GRR-133` | §6.1 |
| **P5 — Sửa điểm/trạng thái chỉ có MỘT đường: event đảo ngược** | Nguyên tắc nền điểm 6, `Đ-5.3` | `GRR-115` · `GRR-116` · `GRR-117` · `Đ-7.2b` · `Q-A3` · `GRR-106` · `Đ-6.3c` | §7 |
| **P6 — Đáp án và thông tin nội bộ chỉ tới ADMIN + MC** | `CLAUDE.md` §Zero-trust, `D15.2`, `S-13` | `Q-B1` · `Đ-5.f` · `S-4` · `Q-A7` | §7.1, §9 |
| **P7 — Nguồn nháp thua nguồn đang có hiệu lực** | `.specify/memory/constitution.md` (`plans/**` là nháp) | `C-1` · `C-2` · `C-3` · `C-5` · `C-10` | §8 |

### Ba mục tôi đã HẠ mức vì tìm thấy quyết định sẵn có

Rà lại theo đúng thứ tự ưu tiên trên, ba mục ban đầu tưởng phải chọn thật ra đã có câu trả lời:

| Mã | Trước | Sau | Quyết định sẵn có đã trả lời |
|---|---|---|---|
| **Q-A1** (đóng có dialog không) | `ĐỀ XUẤT` | **`SUY RA`** | `CLAUDE.md` §UX: dialog chỉ cho *"thao tác **không hoàn tác được**"*. Đóng thì mở lại được ⇒ rule sẵn có **đã** loại nó ra. UX bên ngoài chỉ còn là xác nhận, không phải căn cứ |
| **Q-A5** (mở tay ghi đè `revealAnswerAfterJudge`) | `ĐỀ XUẤT` | **`SUY RA`** | `GRR-112` đã duyệt đúng pattern này: *"câu không được chấm ⇒ không tự đẩy đáp án, **nhưng admin mở tay được**"* — thao tác tay của admin thắng chính sách tự động |
| **Đ-7.e** (thí sinh thấy trạng thái queue) | `ĐỀ XUẤT` | **`SUY RA`** | `GRR-135` đã thu hẹp *"visibility of system status"* về **người thực hiện thao tác**. Áp thẳng: thấy tín hiệu của mình = trong phạm vi; thấy vị trí trong hàng đợi = ngoài phạm vi |

Ba mục khác được **nâng** căn cứ (vẫn giữ kết luận cũ, nhưng lý do đổi từ UX bên ngoài sang quyết định nội bộ): **S-10** (`GRR-136` đã nêu thẳng hệ quả *"biên bản là hồ sơ duy nhất"*), **S-19** (nguyên tắc nền điểm 1 — *"mọi outcome là kết quả SAU KHI admin bấm"* — tự nó cấm khoá trước phán quyết), **C-5** (P7).

---

## 1. Mục ĐÃ CHẾT vì quyết định sau đè lên

> Không cần phân xử — cần **xoá khỏi backlog**. Đây là nợ tài liệu, không phải nợ quyết định.

| Mã | Vì sao chết | Việc cần làm |
|---|---|---|
| `GRR-080` — admin mất kết nối bao nhiêu giây thì tự tạm dừng | `Đ-21`: **không tồn tại trạng thái tạm dừng**. Không có cơ chế để đặt ngưỡng cho | Xoá. Rủi ro thật chuyển sang **S-17** |
| `GRR-079` — pause ⇒ lưu `remainingMs` cho mọi cửa sổ | Cùng lý do | Xoá |
| `GRR-076` — tín hiệu đến khi `PAUSED`/`FINISHED` | Nhánh `PAUSED` chết; nhánh `FINISHED` **giữ** (ghi lịch sử, không hiệu lực) | Viết lại chỉ còn `FINISHED` |
| `§1.2 S3` — *"tín hiệu đến trong lúc trận `PAUSED`"* | Cùng lý do | Viết lại thành *"tín hiệu đến **giữa hai pha**, ngoài mọi cửa sổ đang mở"* |
| `GRR-034` — thí sinh rút lại đáp án để nộp trống | `Đ-20.1` đã chốt: **bản rỗng không phải một đáp án**, bỏ qua, giữ bản hợp lệ trước. Vậy "không rút được" là **chủ đích**, không phải hệ quả phụ | Xoá, ghi câu trả lời vào `Đ-20.1` |
| `Đ-4.e2` — mode nhập liệu cho cả thí sinh click và admin click, ai quyết | `Đ-36`: **một đường vào mỗi mode**; nhập liệu ⇒ chỉ thí sinh, admin **không** chọn thay | Xoá |
| `Đ-5.g` / `S-12` — pre-flight một lần hay theo vòng | `Đ-31`: **theo vòng** | Xoá |
| `Đ-7.g` — trong lúc admin duyệt queue, timer có chạy tiếp không | Điểm 14: **tín hiệu không làm gián đoạn đồng hồ**. Có, chạy tiếp | Xoá |
| `C-8` — ai bấm để bắt đầu đếm 3 giây | `Đ-26` + nguyên tắc nền điểm 2: **admin bấm start timer**, mốc tuyệt đối, không trừ bù độ trễ tay người | Xoá khỏi §10 danh sách chặn của `product-discovery` |
| `Đ-15.2 S5`, `S6` | Đã ghi sẵn *(đã chốt: Đ-6.3, Đ-6.4)* ngay trong bảng | Chuyển sang `game-rules-decisions.md` |

---

## 2. Tầng 2 — Quy ước biên thời gian

| Mã | Phân xử | Mức | Lý do |
|---|---|---|---|
| **GRR-002** | Cửa sổ là **`[mở, đóng)`**. Chênh **0 ms** ⇒ **quá hạn** | `ĐỀ XUẤT` | (1) Quy ước khoảng nửa mở là chuẩn công nghiệp cho mốc thời gian (Google/AWS API: *start inclusive, end exclusive*) — hai cửa sổ liên tiếp **không chồng nhau**, không có mili-giây nào thuộc về hai pha. (2) Không cần "công bằng hoá" biên vì `Đ-28` đã cho bản quá hạn sống tiếp dưới dạng **tô đỏ** để admin phán quyết. Chọn biên chặt + van thoát bằng con người, **không** chọn biên lỏng |
| **GRR-035** | **Cùng quy ước GRR-002** — không có quy ước riêng cho submission | `SUY RA` | DRY. Ba mã này được yêu cầu quyết cùng lúc chính vì lý do đó |
| **GRR-031** | ✅ **CHỦ DỰ ÁN CHỐT 2026-07-26**: độ phân giải = **millisecond**; `tieRule` mặc định = **cùng ms ⇒ cùng bậc** (`share-high`), bậc kế nhảy `n+k` (`GRR-032`) | `CHỐT` | Lý do của chủ dự án: **máy tính dễ tính toán** — so hai số nguyên ms, không làm tròn, không số thực. **Điều tra tiền lệ 26/07 cho kết quả NGƯỢC** (chương trình thật dùng **2 chữ số thập phân** — xem §2.1), và cái giá đã được ghi nhận: cửa sổ hoà hẹp hơn thực tế **10 lần**. Quyết định giữ `ms` là **có chủ đích, không phải sai sót**. Đóng GR-014 |

### 2.1 Tiền lệ chương trình cho `GRR-031` — điều tra 2026-07-26

> Ghi lại vì kết quả **ngược** với quyết định cuối, và vì lập luận cũ của `K-8` có lỗi cần đóng lại cho đúng.

| Nguồn | Nói gì | Hạng |
|---|---|---|
| `docs/source/fandom-olympia-26-luat-choi.md` §Tăng tốc | *"trong cùng một **khoảng thời gian**"* — **không nêu độ phân giải** | **Source of truth** (D8) |
| [`W26` vi.wikipedia Olympia 26](https://vi.wikipedia.org/wiki/%C4%90%C6%B0%E1%BB%9Dng_l%C3%AAn_%C4%91%E1%BB%89nh_Olympia_n%C4%83m_th%E1%BB%A9_26) | *"cùng một thời gian hệ thống ghi nhận (**tính đến 2 chữ số thập phân**)"* | Nguồn ngoài, cụ thể |
| [`W` vi.wikipedia bài tổng](https://vi.wikipedia.org/wiki/%C4%90%C6%B0%E1%BB%9Dng_l%C3%AAn_%C4%91%E1%BB%89nh_Olympia) | *"thứ tự thời gian (**được tính đến hàng phần trăm**)"* | Xác nhận độc lập |
| `plans/.../research/rules-2026.md` §7 | *"độ phân giải **ms** server-received"* | **Nháp trong repo** |

**Ba kết luận:**

1. **Điều khoản hoà là THẬT và CŨ** — có từ **Olympia 7** ([Fandom — Tăng tốc](https://duong-len-dinh-olympia.fandom.com/vi/wiki/T%C4%83ng_t%E1%BB%91c)), khoảng **19 mùa** liên tục. Không phải trường hợp giả định.
2. **Lập luận cũ của `K-8` sai.** Nó loại `W26` *"theo D8"*, nhưng D8 chỉ cho phép loại `W26` ở chỗ Fandom nói **khác** — Fandom **im lặng** về độ phân giải nên không có gì để loại bằng. Con số `ms` đến từ **file nháp**, không phải nguồn. ⇒ Căn cứ thật của `ms` là **quyết định kỹ thuật của chủ dự án**, và tài liệu phải ghi đúng như vậy thay vì trích `D8`.
3. **Không tìm được ca trận cụ thể nào** có hai người trùng thời gian trên **một câu** Tăng tốc. Phải phân biệt với thứ **rất phổ biến và hay bị lẫn**: trùng **TỔNG** điểm sau Tăng tốc (Olympia 18 — ba người cùng 140 sau câu 1; Olympia 26 — Đức Minh và Nam Sơn cùng 105; 2024 — Diễm Quỳnh và Đức Huy cùng 160). Đó là **cộng dồn ra số bằng nhau**, không phải hoà thời gian.

**Kèm theo (kỹ thuật, cần ghi để luật đứng vững)** — `GRR-082`: thứ tự xếp hạng trong một trận phải tính trên **đồng hồ đơn điệu** (monotonic) của tiến trình server, đồng hồ tường chỉ dùng để **hiển thị và ghi log**. Khi đó đồng hồ hệ thống nhảy giữa trận **không thể** đảo thứ hạng đã ghi. Điều này biến `GRR-082` từ câu hỏi luật thành ràng buộc kỹ thuật.

---

## 3. Tầng 3 — Phạm vi v1

| Mã | Phân xử | Mức | Lý do |
|---|---|---|---|
| **GRR-010** — câu hỏi lựa chọn (Khởi động) | **CÓ, trong v1** | `ĐỀ XUẤT` | Rẻ nhất. Mode mặc định là sân khấu ⇒ thí sinh **đọc** đáp án ⇒ chỉ cần **hiển thị được các phương án**, không cần mô hình dữ liệu mới, không cần luật chấm mới (`Đ-1`: admin chấm). Ở mode nhập liệu, thí sinh gõ nhãn phương án như một đáp án text bình thường |
| **GRR-037** — câu hỏi sắp xếp (Tăng tốc) | **CÓ, trong v1 — nhưng ở dạng ĐÁP ÁN TEXT**, không có UI kéo-thả | `ĐỀ XUẤT` | Cái đắt của GRR-037 không phải bản thân loại câu, mà là **UI kéo-thả** + **quy tắc đúng/sai theo từng vị trí**. Cả hai biến mất nếu thí sinh gõ thứ tự (`3-1-4-2`): đó là một đáp án text, dùng nguyên `GR-027` highlight và `Đ-1` admin chấm. **Không phát sinh gì mới.** UI kéo-thả để v1.5 như một cải tiến nhập liệu, không phải một loại câu mới |
| **GRR-050/051** — câu hỏi thực hành (Về đích) | **CÓ, trong v1** | `SUY RA` (NT-B) | Phần chấm vốn không phải vấn đề (*"đạt yêu cầu"* là đánh giá của người). Cái thiếu — chuyển pha suy nghĩ → thực hành — chính là **một nút mốc của admin**, cùng mẫu `Đ-26`/`Đ-33`. Không cần cơ chế mới |
| **Đ-0.1a** — cấu hình 5-12 ghế thì start được không | **Start được, qua CẢNH BÁO `N2`** | `SUY RA` (NT-D) | `N2` (*"số thí sinh ≠ 4"*) đã nằm trong danh sách cảnh báo §1.1. Chặn cứng ở đây sẽ là **chỗ chặn thứ tư**, trái NT-D. **Kèm điều kiện**: contest builder phải bắt admin **điền đủ mảng thang điểm Tăng tốc** dài bằng số ghế — thiếu thì đó là *thiếu cấu hình*, chặn ở builder, không phải chặn ở start |
| **§0.2 S-1** — khoá cứng `rowCount = 4` | **KHÔNG khoá.** `rowCount` 4-8, thang điểm CNV là **mảng cấu hình bắt buộc** dài bằng `rowCount` | `SUY RA` | `CLAUDE.md`: *"mọi timer/điểm là RuleConfig — KHÔNG hard-code luật"*. Khoá cứng 4 là hard-code. Cái thật sự thiếu (`U-3`) là **giá trị thang điểm** cho 5-8 hàng — giải bằng bắt admin điền, đúng mô hình "luật tuỳ biến" |
| **§0.2 S-2** — khoá playlist chuẩn 4 vòng | **KHÔNG khoá** | `SUY RA` | `Đ-5` đã biến playlist thành **gợi ý**. Khoá playlist là lấy lại quyền đã trao cho admin |

---

## 4. Tầng 1 — Ba danh sách đang chặn

### 4.1 `Đ-15.a` — Danh sách conflict: bổ sung 4 mục

Danh sách 16 mục hiện có **đúng nhưng chưa đủ**. Bốn khoảng trống:

| # | Nhóm | Tình huống bổ sung | Vì sao thiếu |
|---|---|---|---|
| **L6** | Lượt | Chọn lượt cho **ghế trống / thí sinh đang mất kết nối** | `L3` chỉ phủ *bị loại / bị cấm / đã rời trận* — mất kết nối tạm thời là trạng thái khác, và là sự cố phổ biến nhất (`S1`) |
| **L7** | Lượt | Trong cùng một vòng Khởi động, gán **hai thể thức lượt riêng khác nhau** cho hai thí sinh | Đóng luôn `GRR-011`: cho phép về mặt cấu hình, nhưng thể thức không đồng nhất = **không công bằng** ⇒ phải hỏi |
| **V6** | Vòng | Mở một vòng **không theo thứ tự playlist** mà **chưa** đánh dấu các vòng bị nhảy qua là "đã bỏ" | `V2` phủ *bỏ hẳn một vòng* (thao tác tường minh); nhảy cóc **ngầm** thì không mục nào phủ, và nó tạo ra vòng ở trạng thái lửng |
| **Đ3** | Đề | Rút một câu **đã dùng trong contest này** | No-repeat trong một contest là tuyệt đối (`Đ-5.1`). Chạy lại vòng làm tăng khả năng chạm phải |

### 4.2 `Đ-15.b` — Cảnh báo có tắt được không

**CÓ, nhưng chia hai hạng.** Đây là câu hỏi UX kinh điển và cũng là câu hỏi có rủi ro cao nhất trong ba câu.

| Hạng | Gồm | Tắt được? | Cơ chế |
|---|---|---|---|
| **Thường trực** | `L1` `L2` `L3` `L4` `L5` `L6` `L7` `N1` `N2` `Đ1` `Đ3` `V3` `V4` `V6` | **CÓ** | Checkbox *"Không hỏi lại loại này trong vòng hiện tại"*. **Tự bật lại** ở cửa vào vòng kế tiếp |
| **Phá huỷ** | `V1` (chạy lại vòng) · `V2` (bỏ vòng) · `V5` (chạy lại sau `FINISHED`) · `E1` · `E2` · `Đ2` | **KHÔNG bao giờ** | Dialog đầy đủ + **bắt nhập lý do** (S-2) mỗi lần |

**Lý do**: NN/g cảnh báo trực tiếp về *dialog fatigue* — dialog xuất hiện quá nhiều thì người dùng bấm Yes theo phản xạ, và dialog mất hết tác dụng đúng vào lúc cần nhất. Với **20 loại conflict** trong một trận trực tiếp, không có cơ chế tắt thì `V1`/`V2` sẽ bị bấm qua như mọi cái khác. Tắt được nhóm thường trực chính là cách **giữ sức nặng** cho nhóm phá huỷ. Phạm vi tắt giới hạn trong **một vòng** để admin không lỡ tắt vĩnh viễn từ vòng đầu.

### 4.3 `Đ-15.c` — Ngưỡng vật lý có phải ngoại lệ duy nhất

**KHÔNG phải duy nhất — có ĐÚNG BA, và không thêm chỗ thứ tư.** Xem NT-D. Đề nghị gộp ba chỗ này thành **một mục duy nhất** trong `game-rules-decisions.md` §1.3; hiện chúng nằm rải ở §1.3, §11.16 và điểm 9 của `game-rules.md`, nên mới sinh ra câu hỏi lặp lại (`Đ-5.1i`, `Đ-0.1a`, `GRR-081`).

### 4.4 `Đ-15.2` — Danh sách sự cố: sửa S3, thêm S8

- **S3** viết lại (xem §1): *"tín hiệu đến giữa hai pha, ngoài mọi cửa sổ đang mở"*.
- **S8 (mới)**: *thí sinh vắng mặt hoặc rời sân khấu giữa vòng* — can thiệp: admin bỏ lượt của người đó, chuyển sang lượt kế, ghi lý do. Đây là sự cố duy nhất **không** sinh ra tín hiệu nào để hàng đợi xử lý, nên không mục nào từ S1-S7 phủ được.

### 4.5 `Đ-15.3` — Ngưỡng vật lý: giữ nguyên

Bảng 8 dòng hiện tại **đúng và đủ**. Hai ngưỡng ≥2 (cửa sổ cướp Về đích, Câu hỏi phụ) là hai chỗ mà pha đó **về mặt định nghĩa** cần hai người. Không có dòng nào cần sửa.

---

## 5. Tầng 4 — Duyệt các đề xuất chờ

> Toàn bộ Tầng 4 đã có lập luận trong `game-rules-open-questions.md`. Dưới đây chỉ ghi **duyệt / bác / sửa**.

### 5.1 Duyệt nguyên văn (không sửa)

`GRR-007` · `GRR-014/015` · `GRR-022/023` · `GRR-024` · `GRR-032` · `GRR-036` · `GRR-038` · `GRR-040` · `GRR-042` · `GRR-043` · `GRR-049` · `GRR-052` · `GRR-053` · `GRR-055` · `GRR-063` · `GRR-065` · `GRR-075` · `GRR-085` · `GRR-089` · `GRR-107` · `GRR-108` · `GRR-109` · `GRR-110` · `GRR-111` · `GRR-112` · `GRR-115` · `GRR-116` · `GRR-117` · `GRR-118` · `GRR-121` · `GRR-122` · `GRR-126` · `GRR-127` · `GRR-130` · `GRR-131` · `GRR-134` · `GRR-135` · `GRR-136`

Lập luận đã đủ; không mục nào mâu thuẫn với nguyên tắc nền.

### 5.2 Duyệt kèm sửa

| Mã | Sửa gì | Lý do |
|---|---|---|
| **GRR-113** | Duyệt phần *"khoá trong phạm vi CÂU"*, và **thi hành luôn khuyến nghị**: quy ba cụm từ (*"bị loại khỏi phần thi"* · *"mất quyền trả lời"* · *"khoá"*) về **một thang phạm vi duy nhất: `CÂU` / `VÒNG` / `TRẬN`** | Đây là lỗi DRY ở tầng thuật ngữ. Ba cụm khác nhau cho cùng một khái niệm sẽ sinh ba nhánh code. Cần cập nhật `docs/glossary.md` |
| **GRR-120** — chạy lại vòng sau `FINISHED` | **Cho phép**, nhưng: (a) cảnh báo `V5` thuộc hạng **phá huỷ**, không tắt được; (b) PDF in lại phải mang nhãn **"bản sửa đổi lần N"**; (c) `Đ-6.4d` được **thu hẹp** về đúng phạm vi gốc của nó — cấp **CÂU**, không phải cấp **TRẬN** | Mâu thuẫn với `Đ-6.4d` là mâu thuẫn **biểu kiến**: `Đ-6.4d` nói *một câu đã chấm thì kết quả đã chốt* (đúng, và điểm 12 củng cố). Suy rộng lên cấp trận chưa từng được chốt. Chặn ở cấp trận sẽ tạo **chỗ chặn cứng thứ tư**, trái NT-D |
| **GRR-081/084/090** | Duyệt *"cảnh báo, không chặn"*, nhưng ghi rõ: **`GRR-084` (hết câu giữa vòng) nay là tình huống KHÔNG TỒN TẠI** theo điểm 20 — kho đề đã kiểm đủ tại cửa vào vòng | Điểm 20 của `game-rules.md` phát biểu thẳng điều này. `Đ2` trong danh sách conflict chỉ còn là **lưới an toàn** cho trường hợp bất thường |
| **GRR-128** | Duyệt — nhưng đây là **việc sửa tài liệu**, chuyển khỏi backlog quyết định sang danh sách công việc | Tự nó đã ghi vậy |

### 5.3 Không mục nào bị bác

Không có đề xuất nào ở Tầng 4 mâu thuẫn với nguyên tắc nền hoặc với nhau.

---

## 6. Tầng 5 — Con số và chính sách

### 6.1 Con số

| Mã | Phân xử | Mức | Lý do |
|---|---|---|---|
| **GRR-087** — `reservePerField` = 2 dựa trên đâu | **Bỏ hằng số 2. Thay bằng công thức**: dự phòng cho một vòng = **đúng số câu của vòng đó** (bảng điểm 19) | `SUY RA` | `Đ-31` chặn cứng: chạy lại một vòng phải qua đúng cửa kiểm đề, tức cần **trọn một vòng câu mới**. Vậy "dự phòng đủ cho 1 lần chạy lại" = số câu của vòng, không phải một hằng số nhỏ tuỳ ý. Ví dụ Khởi động lượt chung cần dự phòng **12**, không phải 2. Đây cũng là số liệu mà **SM-3** cần đo |
| **Đ-5.1g** — *"còn câu hỏi"* đo ở mức nào | **Đủ TRỌN VÒNG**, và **theo từng mức điểm / lĩnh vực** ở vòng nào luật yêu cầu (Về đích: đủ cả mốc 20 và 30) | `SUY RA` | Điểm 19 + điểm 20: nhu cầu một vòng là con số **cố định**; "≥1 câu" không cho phép chạy trọn vòng nên vô nghĩa với `Đ-31` |
| **Đ-5.1h** — kiểm *"còn câu hỏi"* lúc nào | **Tại cửa vào vòng**, tức lúc bấm chạy lại | `SUY RA` | `Đ-31` |
| **Đ-5.1i** — chặn cứng hay cảnh báo | **Chặn cứng** — và **không phải ngoại lệ mới**, nó chính là chỗ chặn thứ hai đã chốt ở `Đ-31` | `SUY RA` | NT-D |
| **GRR-046 / Đ-3** — giá trị câu lẻ ở Về đích | **Cho nhập số nguyên bất kỳ. Định nghĩa −½ là LÀM TRÒN XUỐNG** (giá trị 25 ⇒ phạt 12) | `ĐỀ XUẤT` | Chặn bằng validation là hard-code luật (trái `CLAUDE.md`). Làm tròn xuống là **tất định** và **có lợi cho thí sinh** — chọn hướng đó khi luật gốc im lặng. Dưới `O26_DEFAULT@1` tình huống này không xảy ra (20/30 ⇒ 10/15) |

### 6.2 Chính sách

| Mã | Phân xử | Mức | Lý do |
|---|---|---|---|
| **GRR-011** — hai thể thức lượt riêng khác nhau trong một vòng | **Cho phép về cấu hình, kèm cảnh báo `L7`** (mới, §4.1) | `ĐỀ XUẤT` | Chặn là hard-code; im lặng là bỏ qua một bất công thấy rõ. Cảnh báo là đúng chỗ giữa hai thái cực |
| **GRR-028** — gợi ý ký tự hiển thị lúc nào | **Khi admin bấm** — một nút mốc, cùng mẫu `Đ-26`/`Đ-33` | `SUY RA` (NT-B) | — |
| **GRR-054** — hạn chót đổi gói Về đích | **Tới khi admin bấm "hiển thị câu đầu tiên của gói"** (mốc `Đ-26`) | `SUY RA` (NT-B) | Mốc này đã tồn tại và đã có nghĩa "đề đã ra khỏi server" |
| **GRR-057 / Đ-10.7a** — admin không phân định nhóm hoà | **Đồng hạng** | `SUY RA` | `GRR-032` đã duyệt standard competition ranking; đồng hạng là mặc định của thang đó |
| **GRR-077** — trận không hoàn thành kết thúc bằng trạng thái nào | **Thêm trạng thái `ABANDONED`** — do admin bấm, **bắt nhập lý do**, PDF in kèm nhãn *"trận không hoàn thành"* | `ĐỀ XUẤT` | `Đ-21` xoá `PAUSED` ⇒ máy trạng thái chỉ còn *đang chạy* và `FINISHED`. Không có `ABANDONED` thì một trận hỏng hoặc phải bị đánh dấu `FINISHED` sai sự thật, hoặc treo mãi — cả hai đều làm hỏng thống kê (`SM-11` đếm match `official` `FINISHED`) |
| **GRR-088** — khôi phục `everPublic` bị đánh dấu nhầm | **KHÔNG khôi phục được, với bất kỳ ai.** Cách khắc phục: tạo câu mới | `ĐỀ XUẤT` | `everPublic` là **hàng rào một chiều** chống rò đề. `GRR-111` đã duyệt nguyên tắc *"`everPublic` phải đi theo câu khi import, nếu không hàng rào bị vô hiệu bằng một thao tác hợp lệ"* — cho phép gỡ cờ là mở lại đúng lỗ hổng đó bằng đường khác |
| **GRR-091** — câu hỏi phụ lấy từ pool nào | **Từ chính snapshot của contest**, gán trước khi start như mọi vòng, kiểm tại cửa vào vòng (`Đ-31`, ngưỡng 3 câu) | `SUY RA` | `CLAUDE.md`: *"hệ thống KHÔNG tự lấy đề"*. Không có đường nào khác |
| **GRR-104** — miền giá trị hợp lệ + đổi cấu hình giữa contest | **(a)** Mỗi trường RuleConfig khai kèm `min`/`max` như **metadata của chính RuleConfig** (không hard-code trong code hay UI). **(b)** Đổi cấu hình **không** áp cho match đã chạy | `SUY RA` (NT-C) | (a) là cách duy nhất để có validation mà không vi phạm *"không hard-code luật"*. (b) là NT-C |

---

## 7. Tầng 7 — Câu hỏi nhỏ đi kèm quyết định đã chốt

### 7.1 Từ `Đ-1` — highlight

| Mã | Phân xử | Mức | Lý do |
|---|---|---|---|
| **Đ-1.a** / **Q-B2** / **S-9** — nhiều `acceptedAnswers` | Highlight so với **đáp án khớp nhất** (khoảng cách sửa đổi nhỏ nhất), kèm **dãy chip** để admin bấm đổi sang đáp án khác. **Không** hiển thị song song N bản diff | `ĐỀ XUẤT` | Admin đang phán quyết dưới sức ép thời gian trực tiếp. Nghiên cứu về giao diện chấm bài (khung *Marking*, arXiv 2404.14301) cho thấy giá trị nằm ở **một** bản so sánh sạch, mã màu rõ; N bản song song làm tăng tải nhận thức đúng lúc không được phép chậm. Chip đổi đáp án giữ lại toàn bộ thông tin mà không bắt admin đọc hết |
| **Đ-1.b** / **Q-B3** | So trên chuỗi **đã chuẩn hoá** (trim · gộp khoảng trắng · không phân biệt hoa thường · Unicode NFC) làm mặc định; có **công tắc xem bản nguyên văn**. *"Bỏ dấu tiếng Việt"* **giữ lại, chỉ là công tắc hiển thị**, không bao giờ đổi phán quyết | `SUY RA` | `CLAUDE.md` đã buộc TRIM hai đầu. `Đ-1`: máy không phán quyết ⇒ mọi tuỳ chọn chuẩn hoá chỉ đổi **cách tô màu** |
| **Đ-1.c** / **Q-B5** | **Giữ** `wordCount` / số ký tự, dạng **chip gợi ý phụ** cạnh ô diff, không phải kết luận | `ĐỀ XUẤT` | Rẻ, và đúng vai trò "máy đưa dữ kiện, người kết luận" |
| **Q-B1** — highlight cho ai thấy | **ADMIN và MC.** Không viewer, không thí sinh | `SUY RA` | `CLAUDE.md` §Zero-trust: *"đáp án chỉ rời server tới admin + MC"*. MC cần vì MC là người phán quyết trên sân khấu (mô hình ba tầng) |
| **Q-B4** — Tăng tốc chấm nhiều bài | **Một màn, danh sách tất cả thí sinh**, sắp theo thứ hạng tốc độ, mỗi dòng có diff riêng + nút phán quyết riêng, và **một nút "chốt câu"** ở cuối | `SUY RA` | `CLAUDE.md`: *"một câu = MỘT event điểm cho TOÀN BỘ người chơi"* ⇒ phải có một mốc chốt chung ⇒ phải là một màn. Xem `Đ-5.3.1a` |

### 7.2 Từ `Đ-4` — mode

| Mã | Phân xử | Mức | Lý do |
|---|---|---|---|
| **Đ-4.c** / **Q-C4** | Mode sân khấu: có **ô văn bản tuỳ chọn** để admin ghi lại nội dung thí sinh nói. **Không bắt buộc.** PDF in khi có | `ĐỀ XUẤT` | Bắt buộc gõ giữa trận trực tiếp là mâu thuẫn với chính lý do chọn mode sân khấu. Tuỳ chọn giữ được giá trị cho trận cần biên bản chi tiết, và **S-15** đã xác định xuất xứ quyết định nằm ở biên bản viết tay |
| **Đ-4.d** | **Đúng** — chỉ áp cho mode nhập liệu và hai vòng luôn gõ máy | `SUY RA` | `GRR-131` đã duyệt |
| **Đ-4.e3** | **Recommendation + cảnh báo `L5`**, không ràng buộc cứng | `SUY RA` (NT-D) | — |
| **Q-C1b** | Sửa mode sau khi có match `FINISHED`: **cho sửa**; match đã chạy giữ mode trong snapshot của nó | `SUY RA` (NT-C) | — |
| **Q-C1c** | **Đúng** — cả official lẫn rehearsal trong một contest dùng chung mode | `ĐỀ XUẤT` | Sự bất đối xứng với `revealAnswerAfterJudge` (per-match) là **có lý**, không phải thiếu nhất quán: mode trả lời là thuộc tính của **sân khấu và phần cứng** (có micro? thí sinh có bàn phím?) — không đổi giữa hai trận cùng ngày cùng phòng. `revealAnswerAfterJudge` là thuộc tính **sư phạm** của từng trận. Nên ghi câu này vào PRD để lần sau không ai đặt lại câu hỏi |
| **Q-C2** | **Có** trong preset. *"Áp dụng luật 2026"* đặt **mode sân khấu** | `SUY RA` | `Đ-4`: sân khấu là *"mode LUẬT được đặc tả theo"*. Preset luật 2026 phải đặt đúng mode mà luật 2026 giả định |
| **Q-C3** | **Có** — mode nằm trong contest bundle | `SUY RA` | Mode là contest config; `D23` xuất *"trọn gói"* |

### 7.3 Từ `Đ-5` — điều khiển trận

| Mã | Phân xử | Mức | Lý do |
|---|---|---|---|
| **Đ-5.d** — override rồi thì luật phái sinh tính sao | Luật phái sinh bám **event log thực tế**, không bám kế hoạch. Thi Về đích 2 lần ⇒ **2 gói câu** thật. **NSHV: một lần cho mỗi thí sinh trong cả contest**, không hồi sinh theo lượt | `ĐỀ XUẤT` | Điểm 6: *"điểm là hàm của event log"* — mở rộng tự nhiên: **mọi trạng thái phái sinh** là hàm của event log. NSHV là ngoại lệ vì luật gốc gắn nó với **con người trong phần thi**, không gắn với lượt; cho dùng lại là tặng lợi thế cho chính người được hưởng lỗi vận hành |
| **Đ-5.e** | **Có** — AuditLog riêng, **bắt nhập lý do dạng văn bản tự do** | `SUY RA` | `S-14` đã chốt không có trường *"người yêu cầu"* ⇒ ô lý do là **nơi duy nhất** ghi được xuất xứ. Trùng **S-2** |
| **Đ-5.f** / **S-4** | **Admin + MC. KHÔNG viewer** | `ĐỀ XUẤT` | Lộ thứ tự sắp tới cho khán giả là phá trình diễn. MC cần để dẫn |
| **Đ-5.1d** — bỏ Tăng tốc phá mốc của Về đích | **Tổng quát hoá mốc**: *"điểm sau phần thi Tăng tốc"* đọc thành **"điểm tại thời điểm admin mở vòng Về đích"** | `ĐỀ XUẤT` | Một phát biểu này đóng luôn `Đ-5.1e` và mọi biến thể bỏ-vòng khác. Nó **không đổi** kết quả trong playlist chuẩn (mốc trùng nhau), nên không phải sửa luật — chỉ là phát biểu lại cho tổng quát |
| **Đ-5.1e** — bỏ Khởi động ⇒ mọi điểm bằng 0 | **Không cần cảnh báo riêng**; `V2` đã cảnh báo việc bỏ vòng. Hoà toàn sân ở 0 điểm ⇒ xếp theo **vị trí** | `SUY RA` | `GRR-122` đã duyệt: hoà ở 0 hoặc âm vẫn xử lý bình thường |
| **Đ-5.1f** | **Có** — chính là `V1`, `V2`, hạng **phá huỷ** (§4.2) | `SUY RA` | — |
| **GRR-133** — *"còn câu hỏi"* đo trên snapshot hay kho contest | **Trên SNAPSHOT.** Gán thêm câu vào snapshot giữa trận **được phép** vì đó là thao tác **có chủ đích của admin**, không phải hệ thống tự lấy đề | `SUY RA` | `D22` cấm **hệ thống** tự lấy đề, không cấm admin gán thêm. Đo trên kho contest sẽ cho `Đ-31` một kết quả sai (báo đủ đề trong khi vòng không chạy được) |

### 7.4 Từ `Đ-5.3` — event log

| Mã | Phân xử | Mức | Lý do |
|---|---|---|---|
| **Đ-5.3.1a** — event điểm Tăng tốc phát lúc nào | Khi admin bấm **"chốt câu"** — một nút tường minh, sau khi đã phán quyết đủ mọi thí sinh. Trước mốc đó bảng điểm hiển thị **điểm cũ** kèm chỉ báo *"đang chấm"* | `SUY RA` (NT-B) | Một event cho cả bảng thì phải có một mốc phát. Cho máy tự phát khi chấm xong người cuối sẽ mất khả năng sửa trước khi công bố — trái điểm 12 (một phán quyết, không quay lại) áp ở cấp câu |
| **Đ-5.3.1b** — *"một câu = một event"* có áp cho vòng khác không | **KHÔNG.** Chỉ áp cho **vòng tính điểm theo thứ hạng** (Tăng tốc). Vòng khác: **một event cho mỗi chủ thể được tính điểm** | `SUY RA` | `CLAUDE.md` phát biểu quy tắc này **kèm lý do**: *"vòng tính điểm theo thứ hạng… revert là revert cả bảng xếp hạng của câu đó"*. Lý do không tồn tại ở vòng khác |
| **GRR-106** — điểm chốt tại mốc nào để xét hoà | Chốt tại **mốc admin bấm kết thúc vòng cuối** của playlist. Sửa điểm sau khi đã vào `TIE_BREAK` ⇒ cảnh báo `E1`; tie-break **đã chạy giữ nguyên**, admin quyết chạy lại | `SUY RA` | Đúng mẫu `GRR-121` đã duyệt cho tình huống song sinh |

### 7.5 Từ `Đ-6` — mốc thời gian, lệnh cấm

| Mã | Phân xử | Mức | Lý do |
|---|---|---|---|
| **Đ-6.3b** — *"bị loại khỏi phần thi này"* ở VCNV có gỡ được không | **Gỡ được**, cùng cơ chế `Đ-6.3` | `SUY RA` | Điểm 3 giữ lịch sử đầy đủ chính là để admin *"xem lại và gỡ lệnh cấm"* (`CLAUDE.md`). Không có lý do nào để một loại cấm gỡ được còn loại kia thì không |
| **Đ-6.3c** | **Có event revert. KHÔNG dialog** | `ĐỀ XUẤT` | Duyệt nguyên văn đề xuất: thao tác **đảo ngược được** và **đang bị ép thời gian** ⇒ hai tiêu chí đều nói không dialog. Khớp NN/g: dialog dành cho việc **không hoàn tác được** |
| **Đ-6.2c** — bấm chuông trước khi cửa sổ cướp mở | **Vô hiệu, phải bấm lại. Không phạt** | `SUY RA` | Duyệt đề xuất. Luật gốc chỉ định hình phạt bấm sớm cho **Câu hỏi phụ**; suy rộng sang Về đích là phát minh luật. Kỹ thuật: theo điểm 9, trước mốc mở cửa sổ nút chuông **chưa sống** ⇒ tình huống này gần như không tồn tại; quy tắc chỉ là lưới an toàn |

### 7.6 Từ `Đ-7` — hàng đợi

| Mã | Phân xử | Mức | Lý do |
|---|---|---|---|
| **Đ-7.b2** — vòng đời tín hiệu | **Duyệt nguyên văn**: *tín hiệu gắn với ĐÍCH của nó và vô hiệu khi đích đóng*. Ba phạm vi (vòng / lượt chọn / câu) trở thành **hệ quả**, không phải ba quy tắc | `SUY RA` | Đề xuất này tốt hơn hẳn phương án bảng-ngoại-lệ-theo-vòng. Nó cũng làm điểm 18 của `game-rules.md` (*"xoá hàng đợi khi chấm xong"*) thành hệ quả tự nhiên thay vì quy tắc rời |
| **Đ-7.c** — mốc tính điểm: thí sinh bấm hay admin xác nhận | **Admin xác nhận** | `SUY RA` | Ở VCNV hàng đợi **chặn** (điểm 4) ⇒ không hàng ngang nào mở được trong lúc chờ duyệt ⇒ *"số hàng đã mở"* **giống hệt nhau** ở hai mốc. Hai lựa chọn cho cùng kết quả; chọn *admin xác nhận* vì đó là mốc tín hiệu **có hiệu lực** |
| **Đ-7.d** — bị reject có bấm lại ngay không, bao nhiêu lần | **Bấm lại ngay được, KHÔNG giới hạn theo luật** | `SUY RA` | `Đ-7`: *"reject ⇒ thí sinh KHÔNG mất lượt"*. Giới hạn số lần là một hình phạt mà luật gốc không có. Lạm dụng xử lý bằng **S2** (thiết bị lỗi). Rate-limit kỹ thuật chống flood là chuyện hạ tầng, không phải luật |
| **Đ-7.e** — thí sinh thấy trạng thái queue không | **Thấy trạng thái CỦA RIÊNG MÌNH** (*"đã gửi — đang chờ duyệt"*), **không** thấy vị trí trong hàng đợi, **không** thấy người khác | `SUY RA` | `GRR-135` đã thu hẹp *"visibility of system status"* về **người THỰC HIỆN thao tác**. Áp thẳng phạm vi đã thu hẹp đó: tín hiệu của chính mình **thuộc** phạm vi (phải hiện); vị trí trong hàng đợi là thông tin về **người khác**, **ngoài** phạm vi (không hiện). Không cần cân nhắc mới — chỉ là áp một rule đã có |
| **Đ-7.2b** — chuyển quyền vì sự cố, điểm người đầu xử lý sao | **Revert bằng event đảo ngược** (`Đ-5.3`), kèm lý do | `SUY RA` | Điểm 6: sửa điểm chỉ có một đường |
| **Đ-7.2c** — cửa sổ đã đóng mới phát hiện sự cố | **Tín hiệu kế tiếp vẫn dùng được** | `SUY RA` | Điểm 18 chỉ xoá **hàng đợi đang hoạt động**; **lịch sử không bao giờ xoá** (điểm 3) và tồn tại chính vì mục đích này |
| **GRR-125** — tiêu chí để một nút là *"chuông"* | Tiêu chí ba điều kiện **đồng thời**: **(1)** một chiều không rút lại · **(2)** đua tốc độ · **(3)** quyền không chia được. ⇒ Chuông thật và *"Mở chướng ngại vật"* là chuông. **Chọn hàng ngang KHÔNG phải** (trượt điều kiện 2) ⇒ **giữ hotkey `1-8`** | `SUY RA` | Điểm 23 đã phát biểu chính tiêu chí này để biện minh cho dialog phía thí sinh. Áp lại đúng tiêu chí đó cho câu hỏi hotkey cho ra kết quả nhất quán với `CLAUDE.md` hiện hành — không phải sửa gì |

### 7.7 Từ `Đ-9`, `Đ-10` — Câu hỏi phụ

| Mã | Phân xử | Mức |
|---|---|---|
| **Đ-9.c** | **Duyệt cả ba nhánh**: sau khi bấm chuông thì **khoá ô nhập** · gõ mà không bấm chuông ⇒ **không nộp, không tính** · bấm khi ô rỗng ⇒ nộp rỗng, admin chấm Sai, **không trừ điểm** | `SUY RA` |
| **Đ-10.7b** | **Duyệt**: chọn trong tập hoà = bình thường; **thêm người KHÔNG hoà = cảnh báo `N1`** | `SUY RA` |

---

## 8. product-discovery §6 — C-1 → C-17

| Mã | Phân xử | Mức | Lý do |
|---|---|---|---|
| **C-1** — `revealAnswerAfterJudge` per-contest hay per-match | **PER-MATCH** | `SUY RA` | Spec `ruleconfig-v2-spec` nói per-match ở **hai** chỗ và **chủ động phủ định** per-contest; `CLAUDE.md` cũng per-match. Chỉ `P/PRD.md` NFR-4 nói contest, và `P/**` là **bản nháp**. Ngoài ra per-contest **phá use-case rehearsal của D21**. Việc cần làm: sửa câu chữ NFR-4 khi migrate |
| **C-2** — quy mô viewer | **<50 viewer/trận · ~5 trận song song** (D11 + NFR-2). Xoá 75 / 480 / 500 khỏi mọi tài liệu | `SUY RA` | D11 là mốc chốt muộn nhất; ba con số kia là tàn dư trước D11. **Nhưng giữ nguyên yêu cầu viewer responsive**: lý do đổi từ *"480/500 là điện thoại"* sang *"khán giả tại chỗ mặc định dùng điện thoại"* — vẫn đúng ở quy mô 50, chỉ là không còn biện minh cho đầu tư tối ưu hoá quy mô lớn |
| **C-3** — ai duyệt đề DRAFT→ACTIVE | **Permission `question.review`, seed role `REVIEWER`** (D15.1) | `SUY RA` | `product-gaps.md` là bản cũ chưa sync. Cùng lô: §2.2 (*"MC thấy đáp án không"*) đã chốt **CÓ** ở D15.2 |
| **C-4** — D26/D27 | **D26**: ba điểm có dấu vết *"user chốt 15/07"* ⇒ **CHỐT**; phần còn lại (merge policy username trùng, phiếu tài khoản PDF) ⇒ **treo**, đổi nhãn tiêu đề thành *"🟢 CHỐT một phần"*. **D27** (result bundle): **hoãn sang v1.5** | `ĐỀ XUẤT` | v1 đã có xuất PDF kết quả, tức nhu cầu trước mắt *"mang kết quả ra khỏi máy portable"* đã được phục vụ. D27 giải bài toán **tổng hợp thống kê nhiều trận**, mà v1 chưa có màn hình nào tiêu thụ dữ liệu đó. Ship D27 ở v1 là xây kho chứa trước khi có hàng |
| **C-5** — ngưỡng độ trễ | Giữ **một** bộ: p95 **<200 ms LAN / <500 ms Internet** (NFR-1). **Xoá con số 300 ms** của US-4.1. Phản hồi cục bộ optimistic: **≤100 ms** theo `CLAUDE.md` — **sửa FR-5.1 từ 50 ms lên 100 ms** | `SUY RA` (P7) | Cả hai vế đều giải bằng **quy tắc nguồn sẵn có**, không cần cân nhắc kỹ thuật mới: 300 ms của US-4.1 không nói mạng nào nên không kiểm chứng được ⇒ xoá; 50 vs 100 ms là `P/PRD.md` (**nháp**) chọi `CLAUDE.md` (**đang có hiệu lực**) ⇒ `CLAUDE.md` thắng, đúng như C-1/C-2/C-3/C-10 |
| **C-6** — disclaimer bản quyền chưa có FR | **Thêm FR mới** vào E-2/E-9: *"khi upload `backgroundTrack` hoặc media có bản quyền, hiển thị disclaimer và yêu cầu tick xác nhận; ghi AuditLog"* | `SUY RA` | D10 + spec §10 đã yêu cầu; thiếu FR là lỗi tài liệu, không phải câu hỏi mở |
| **C-9** — ranh giới Contest vs Match | **Contest** = cấu hình + playlist + danh sách đề đã gán + roster + theme (bản thiết kế, dùng lại được). **Match** = một lần chạy playlist đó (event log + điểm + kết quả). **Mã phòng thuộc CONTEST.** Nhiều match sinh ra từ: rehearsal, practice (v1.5), và chạy lại trọn contest. UI: admin bấm **"Bắt đầu trận mới"** trên màn chi tiết contest | `ĐỀ XUẤT` | Đây là ranh giới tối thiểu làm cho NT-C, `Q-C1b`, `GRR-104`, spec §13 (*"room code thuộc contest đang mở"*) và D21 (`matchPurpose` per-match) **cùng đứng vững**. Không có ranh giới này thì bốn mục đó mâu thuẫn nhau |
| **C-10** — `Question.visibility` | **Derived, read-only** (spec §9 + `GRR-089`) | `SUY RA` | Cột set tay tạo **hai nguồn sự thật** với "câu thuộc bộ đề public". Việc cần làm: sửa câu chữ D16 và FR-2.2b — setter không tự đặt PUBLIC, chỉ qua việc đưa câu vào bộ đề public |
| **C-11 Q-A1** — thao tác **đóng** có dialog không | **KHÔNG.** Chỉ **mở** mới có dialog | `SUY RA` | `CLAUDE.md` §UX đã giới hạn dialog phía admin cho *"mọi thao tác **không hoàn tác được**"*. Mở là không thu hồi được (khán giả đã thấy); **đóng thì mở lại được** ⇒ rule sẵn có tự loại nó ra, không cần quyết định mới. *(NN/g về dialog fatigue chỉ là xác nhận độc lập, không phải căn cứ)* |
| **C-11 Q-A2** — miếng ghép admin mở tay có tính vào thang điểm CNV không | **CÓ tính** | `ĐỀ XUẤT` | Thang điểm CNV đo **lượng thông tin đã lộ**, không đo **cách nó lộ ra**. Thí sinh nhận đúng chừng ấy dữ kiện dù nguồn là engine hay tay admin. Không tính sẽ tạo lỗ hổng: mở tay để giữ thang điểm cao |
| **C-11 Q-A3** — đóng lại có đổi điểm không | **Không đổi điểm, chỉ đổi hiển thị.** Bước thang CNV **không lùi lại** khi đóng | `SUY RA` | Hệ quả trực tiếp của Q-A2: thông tin đã lộ thì không thu hồi được. Điểm 6: muốn đổi điểm phải qua event đảo ngược |
| **C-11 Q-A4** — có chặn khi `PAUSED`/`FINISHED` không | `PAUSED` **không tồn tại** (`Đ-21`). `FINISHED`: **cho phép**, kèm cảnh báo | `SUY RA` | Xem lại trận đã kết thúc là nhu cầu thật (phân xử khiếu nại); thao tác này không sinh điểm |
| **C-11 Q-A5** — mở tay có ghi đè `revealAnswerAfterJudge` không | **CÓ ghi đè** | `SUY RA` | `GRR-112` đã duyệt **đúng pattern này**: *"câu không được chấm ⇒ không tự đẩy đáp án, **nhưng admin mở tay được**"*. Tức thao tác tay của admin **đã** được công nhận là thắng chính sách tự động — Q-A5 chỉ là cùng một câu hỏi ở một trường cấu hình khác |
| **C-11 Q-A6** — AuditLog phân biệt mở-do-admin và mở-do-engine | **CÓ** | `SUY RA` | Không phân biệt thì Q-A2 không kiểm toán được, và khiếu nại *"vì sao đáp án hiện sớm"* không tra được |
| **C-11 Q-A7** — role nào ngoài ADMIN | **Chỉ ADMIN** | `SUY RA` | `S-13`: `/mc` read-only. `Đ-18`: một admin cho mỗi contest |
| **C-12** — hệ thống không phán xử đáp án | Đã chốt. Việc còn lại là **sửa câu chữ**: PS-7 trong PRD **không được hứa auto-match**; thêm **non-goal mới NG-8**: *"Không tự động chấm đúng/sai, kể cả với câu gõ máy"*; FR về `acceptedAnswers` viết lại thành *"dữ liệu để đối chiếu và tô màu cho admin"* | `SUY RA` | — |
| **C-16 / C-17** | Xem §9 | — | — |

---

## 9. product-discovery — S-1 → S-20

| Mã | Phân xử | Mức | Lý do |
|---|---|---|---|
| **S-1** — permission cho các thao tác phá huỷ | Thêm 5 permission vào catalog CASL: `match.round.skip` · `match.round.rerun` · `match.conflict.override` · `match.event.revert` · `match.reveal.manual`. Cả 5 gán cho **tài khoản admin đang giữ quyền điều khiển contest** (xem S-16) | `SUY RA` | `CLAUDE.md` §Zero-trust: *"mọi request/socket event đều verify auth + permission ở server"*. Không có permission thì không enforce được — đây là lỗ hổng bảo mật, không chỉ lỗ hổng tài liệu |
| **S-2** — revert có bắt nhập lý do không | **CÓ**, cho cả năm thao tác của S-1 | `SUY RA` | `S-14` đã chốt AuditLog **không** có trường *"người yêu cầu"* ⇒ ô lý do là kênh duy nhất ghi xuất xứ. Không bắt nhập là mất nốt kênh cuối |
| **S-3** — nội dung dialog cảnh báo | Bốn phần cố định: **(1)** luật nào bị lệch · **(2)** recommendation của hệ thống là gì · **(3)** hệ quả nếu bấm Yes · **(4)** ô **lý do bắt buộc** (chỉ hạng phá huỷ). **KHÔNG có timeout tự đóng.** Tắt được theo §4.2 | `ĐỀ XUẤT` | Timeout tự đóng một dialog phán quyết là phản mẫu rõ ràng — hoặc nó tự chọn thay người, hoặc nó bắt bấm lại giữa lúc bận. Phần (2) là chỗ mô hình advisory thật sự tạo ra giá trị: máy nói *nó nghĩ gì*, người quyết |
| **S-4** | Admin + MC, không viewer — xem `Đ-5.f` | `ĐỀ XUẤT` | — |
| **S-5** — sound slot mới | Thêm 4 slot: `revert` · `round-skipped` · `round-rerun` · `override`. Mặc định **rỗng = im lặng** | `SUY RA` | D10 |
| **S-6** — trình bày biên bản / PDF | Mỗi vòng in thành **một hoặc nhiều "lần chạy"** đánh số thứ tự, mỗi lần mang một nhãn: `hiệu lực` · `đã bỏ` · `đã chạy lại`. Event revert in **xen kẽ đúng vị trí thời gian**, không gom cuối | `ĐỀ XUẤT` | `Đ-5.2d` chốt *"giữ như một vòng đã chạy kèm nhãn đã bỏ"*; ba nhãn là bộ tối thiểu để phân biệt. In xen kẽ giữ được tính **linear** của event log — gom cuối là dựng lại lịch sử |
| **S-7** — `Esc` trên màn thí sinh ở mode sân khấu | **Không làm gì.** Không back, không thoát fullscreen | `SUY RA` | `CLAUDE.md` đã chốt ngoại lệ: *"màn thi đấu của thí sinh: Esc CHỈ xoá ô nhập, không back"*. Mode sân khấu không có ô nhập ⇒ vế duy nhất còn lại là *"không back"* |
| **S-8** — admin thấy lịch sử submission trước đó | **CÓ** | `SUY RA` | `Đ-1` giao toàn quyền chấm cho admin và `Đ-28` buộc hiển thị bản hợp lệ + bản quá hạn cạnh nhau ⇒ lịch sử là dữ liệu bắt buộc, không phải tính năng phụ |
| **S-9** | Xem `Đ-1.a` (§7.1) | `ĐỀ XUẤT` | — |
| **S-10** — retention vs biên bản | Job retention **tự xuất PDF vào kho lưu trữ trước khi xoá**, và **từ chối xoá** nếu xuất thất bại | `SUY RA` | `GRR-136` đã duyệt và nêu thẳng hệ quả: *"sau retention điểm không tái tính được, biên bản/PDF thành hồ sơ duy nhất"*. Nếu hồ sơ duy nhất đó chưa từng được tạo thì retention là **mất dữ liệu**, không phải dọn dữ liệu |
| **S-11** — viewer thấy trận quay ngược | Banner *"Vòng X đang được tổ chức lại"* trong lúc chạy lại; bảng điểm đổi **không animation** | `SUY RA` | `GRR-130` đã duyệt mapping *"không animation"*. Banner là bù lại đúng chỗ `Đ-21` bỏ đi (viewer mất mọi tín hiệu về việc trận đang bất thường) |
| **S-16** — `Đ-18` *"một admin"* là ràng buộc tầng nào | **Nhiều tài khoản có quyền, nhưng MỘT PHIÊN điều khiển tại một thời điểm.** Phiên chuyển giao được, có dialog xác nhận ở cả hai đầu, ghi AuditLog | `ĐỀ XUẤT` | Gắn cứng một tài khoản vào contest sẽ **hỏng trận** khi máy admin chết — không có đường vào lại. Khoá theo phiên giữ đúng ý *"một người điều khiển tại một thời điểm"* mà vẫn cho đổi máy, đổi người. Đây cũng là câu trả lời cho **S-17** |
| **S-17** — mất phương án dự phòng khi admin rớt mạng | **Giải bằng S-16**: admin dự phòng **giành quyền điều khiển**. Trận không cần đóng băng vì không có gì tự chạy — mọi thứ chờ nút bấm | `SUY RA` | `Đ-21` bỏ auto-pause là hợp lý **với điều kiện** có đường vào lại. S-16 cung cấp đường đó. Thiệt hại còn lại chỉ là thời gian thật của cửa sổ đang mở — van thoát vẫn là `Đ-5.1` |
| **S-18** — bộ thông điệp toast cho invalid state | Mẫu **hai vế**: *"[không làm được gì] — [vì trạng thái nào]"*. Ví dụ: *"Chưa chấm được — câu chưa hiển thị"* · *"Chưa mở được vòng — kho đề thiếu 3 câu mức 30"* · *"Không chuyển câu được — câu này chưa có phán quyết"*. Cấm toast chung chung kiểu *"Thao tác không hợp lệ"* | `ĐỀ XUẤT` | Chuẩn UX cho trạng thái không khả dụng: điều tệ nhất là người dùng **không biết vì sao** không bấm được. Trong trận trực tiếp, một toast mơ hồ khiến admin tưởng hệ thống treo. Vế thứ hai (lý do) cũng là thứ chỉ cho admin **bước tiếp theo** |
| **S-19** — có khoá ô nhập hàng ngang của người vừa bấm CNV không | **KHÔNG khoá** | `SUY RA` | Tiền lệ Athena **nghiêm hơn luật gốc**: luật chỉ loại người đó khi họ trả lời **sai** CNV. Khoá ngay lúc bấm là trừng phạt trước khi có phán quyết — trái thẳng nguyên tắc nền điểm 1 (*"mọi outcome là kết quả SAU KHI admin bấm"*). Nếu BTC muốn hành vi Athena, đó là một **tuỳ chọn RuleConfig**, không phải mặc định |
| **S-20** — nút chọn hàng ngang có hiện trên máy thí sinh chưa tới lượt không | **KHÔNG hiện.** Mâu thuẫn `Đ-16` vs `Đ-5` là **biểu kiến** | `SUY RA` | Hai rule tác động ở **hai thời điểm khác nhau**: `Đ-5` cho admin ép **ở khâu gán lượt** (trước khi mở cửa sổ chọn); `Đ-16` chi phối **sau khi cửa sổ đã mở**. Khi cửa sổ mở thì "ai đang tới lượt" đã là quyết định của admin rồi ⇒ tín hiệu sai lượt **không cần tồn tại**. Ghi dòng này vào bảng quyết định `GR-007` |

---

## 10. KHÔNG QUYẾT — cần chủ dự án

> Bảy mục dưới đây **không suy ra được**. Không đề xuất, không gắn nhãn khuyến nghị.

| Mã | Câu hỏi | Vì sao không suy ra được |
|---|---|---|
| **Đ-9.e** | BGK có trùng A-9 (BTC/trọng tài) hay là stakeholder riêng? | Sự thật về **cơ cấu tổ chức thật** của đơn vị dùng sản phẩm. *(Suy được duy nhất một điều: dù trả lời thế nào, BGK **không cần role/permission** vì không thao tác hệ thống — mô hình ba tầng)* |
| **S-15** | Biên bản viết tay là **yêu cầu quy trình bắt buộc** hay **giả định vận hành**? PDF có cần chỗ ký? Có cần mẫu chuẩn? | Quyết định về **quy trình của BTC**, nằm ngoài phần mềm. Nhưng đây là mục **rủi ro cao nhất** trong danh sách: `S-14` đã chuyển toàn bộ xuất xứ quyết định ra ngoài hệ thống, mà chưa tài liệu nào nói ai chịu trách nhiệm tạo hồ sơ đó |
| **AS-4** | v1 giả định **1 người** hay **≥2 người** vận hành? | Ràng buộc nhân sự của người dùng. Ghi nhận khách quan: admin nay gánh **7 nhóm thao tác** (chọn vòng · chọn lượt · hiển thị câu · start timer · chấm · mở/đóng hiển thị · phán quyết mọi dialog) — nhiều hơn hẳn lúc `AS-4` được viết |
| **GRR-103** | Wiki gốc đổi sau ngày snapshot thì quy trình cập nhật preset là gì? | *(Suy được phần kỹ thuật: preset đã đánh version `O26_DEFAULT@1` ⇒ wiki đổi sinh `@2`, contest đang chạy không bị ảnh hưởng nhờ NT-C. **Phần không suy được**: ai theo dõi wiki, tần suất nào)* |
| **Đ-4.5a** | *"Vị trí"* thí sinh do **ai đặt**, **lúc nào**, đổi được giữa contest không? | *(Suy được phần cơ chế: luật gốc dùng **bốc thăm** ⇒ contest builder có ô nhập tay + nút bốc thăm ngẫu nhiên, khoá tại match start theo NT-C. **Không suy được**: quy trình bốc thăm thật của BTC diễn ra khi nào so với lúc dựng contest)* |
| **Bản quyền format Olympia** | Có được nhắc tên "Olympia" trong UI công khai không? | Câu hỏi **pháp lý**, cần ý kiến ngoài kỹ thuật. `product-gaps` §1.3 xếp P1; D7 bị xoá **không** trả lời nó |
| **License repo** | — | Cùng loại |

---

## 11. Căn cứ UX bên ngoài

> **Đây là căn cứ HẠNG BỐN**, chỉ dùng ở những mục mà quyết định sẵn có, câu trùng pattern và suy luận đều đã cạn — tức chỉ cho nhóm `ĐỀ XUẤT` (≈22 mục). Không mục `SUY RA` nào dựa vào bảng này; chỗ nào UX bên ngoài **trùng kết luận** với quyết định nội bộ thì nó là **xác nhận độc lập**, không phải lý do (rõ nhất ở `Q-A1`).

| Áp cho | Nguồn |
|---|---|
| `Đ-15.b` (tắt cảnh báo) · `Q-A1` (đóng không cần dialog) · `Đ-6.3c` | [NN/g — Confirmation Dialogs Can Prevent User Errors (If Not Overused)](https://www.nngroup.com/articles/confirmation-dialog/) — dialog fatigue; ưu tiên **undo** hơn dialog; dialog chỉ dành cho hành động hậu quả cao, không hoàn tác được; tuỳ chọn *"không hỏi lại"* là cách chuẩn để giữ sức nặng cho dialog quan trọng |
| `Đ-15.b` · `S-3` | [A UX guide to destructive actions](https://medium.com/design-bootcamp/a-ux-guide-to-destructive-actions-their-use-cases-and-best-practices-f1d8a9478d03) · [Confirmation dialogs without irritation](https://uxplanet.org/confirmation-dialogs-how-to-design-dialogues-without-irritation-7b4cf2599956) |
| `S-18` (toast invalid state) · `Đ-16` | [Disabled states — UX Best Practices](https://subux.pro/guides/article/disabled-states) — *"users don't know why they can't act"*; phải nói **lý do**, không chỉ nói **không được** · [LogRocket — Toast notification best practices](https://blog.logrocket.com/ux-design/toast-notifications/) |
| `S-18` (khả năng tiếp cận) | [Primer — Accessible notifications and messages](https://primer.style/accessibility/patterns/accessible-notifications-and-messages/) · [Designing Toast Messages for Accessibility](https://sheribyrnehaber.medium.com/designing-toast-messages-for-accessibility-fb610ac364be) |
| `Đ-1.a` / `Q-B2` / `S-9` (highlight nhiều đáp án) | [Marking: Visual Grading with Highlighting Errors and Annotating Missing Bits (arXiv 2404.14301)](https://arxiv.org/html/2404.14301) — chấm bằng mã màu ba loại (đúng / sai / thừa) trên **một** bản đối chiếu · [Moodle Assignment Grading UX](https://docs.moodle.org/dev/Assignment_Grading_UX) — giảm số lần tải trang và số chỗ phải tìm khi chấm |
| `GRR-002` / `GRR-035` (biên nửa mở) | Quy ước khoảng thời gian *start inclusive / end exclusive* ở [Google API `Interval`](https://developers.google.com/maps/documentation/weather/reference/rest/v1/Interval) và [AWS `exclusiveEndTime`](https://docs.aws.amazon.com/sdk-for-kotlin/api/latest/qldb/aws.sdk.kotlin.services.qldb.model/-export-journal-to-s3-request/exclusive-end-time.html) |
| `GRR-031` / `GRR-082` (đồng hồ) | [Quizmania — distributed question buzzer](https://medium.com/holisticon-consultants/quizmania-implementing-a-distributed-question-buzzer-d3616e4cadae) — thứ tự bấm phụ thuộc đồng hồ; mọi bên phải dùng chung một nguồn thời gian |
