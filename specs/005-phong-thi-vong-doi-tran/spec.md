# Feature Specification: Phòng thi và vòng đời trận

**Feature Branch**: `005-phong-thi-vong-doi-tran`

**Created**: 2026-07-30

**Status**: Draft

**Input**: EPIC-005 — Phòng thi và vòng đời trận (`docs/PRD.md` §11, §12)

**Nguồn**: `docs/PRD.md` (EPIC-005 §11 dòng 443-454, PRD-REQ-027→035 và PRD-REQ-091, PRD-REQ-105 §12, JOURNEY-004→007 §10, Core Game Loop §8.1) · `docs/glossary.md` (`TERM-010`, `TERM-011`, `TERM-018`, `TERM-019`, `TERM-020`, `TERM-031`, `TERM-039`) · `docs/game-rules.md` (`GR-022`, `GR-023`, `GR-025`, `GR-029`, `GR-030`, `GR-031` C3/C5/C6/C7/C8, `GR-036`) · `docs/game-state-machine.md` (`STATE-001` LOBBY, `STATE-007` TIE_BREAK, `STATE-008` FINISHED, `INV-020`, `INV-022`) · `docs/traceability.md`. `docs/reviews/**` và `plans/**` không phải nguồn.

## Phạm vi

EPIC-005 phủ **vòng đời của một trận** — từ lúc trận được tạo trong một contest đã dựng xong (EPIC-004), qua trạng thái nghỉ `LOBBY` là cửa vào/ra duy nhất của mọi vòng, bốn cửa ra chủ động của một vòng đang chạy, cú chốt trận và nhánh Câu hỏi phụ, cho tới khi trận đóng sổ **niêm phong** hoặc bị huỷ với nhãn *bỏ dở*, và việc tạo một trận mới trong cùng contest. Bao gồm việc sửa danh sách câu đã gán và đặt chỗ câu Câu hỏi phụ tại `LOBBY`. Đây là các JOURNEY-004 (Vào phòng), JOURNEY-005 (Thi đấu — phần vòng đời cấp trận), JOURNEY-006 (Xử lý sự cố) và JOURNEY-007 (Sau trận) trong PRD.

**Không thuộc phạm vi feature này** (thuộc epic khác, xem §Out of Scope): tạo contest, áp preset luật, cấu hình từng vòng, mode trả lời, gán ghế/vị trí, chọn danh sách câu **ban đầu**, chủ đề và âm thanh (EPIC-004); soạn câu hỏi và bộ đề (EPIC-002); nhập/xuất gói contest (EPIC-003); thực thi luật bên trong một vòng đang chạy — chấm điểm, hàng đợi tín hiệu, mốc thời gian, rút đề theo bộ VCNV, thể thức ba câu Câu hỏi phụ (EPIC-006); các mốc bấm và màn chấm chi tiết của admin (EPIC-007); xác thực và phân quyền (EPIC-001).

## User Scenarios & Testing *(mandatory)*

### US-001 — Contest chạy nhiều trận, mỗi trận giữ trạng thái riêng (Priority: P1)

- **Title**: Contest là bản thiết kế, trận là một lần chạy
- **Actor**: ACTOR-001 admin
- **Intent**: Tạo và vận hành nhiều trận trong cùng một contest mà không làm lẫn điểm, nhật ký sự kiện, hay ghế đã gán giữa các trận.
- **User value**: Giải `PS-8` — trạng thái thuộc về **trận**, không phải một biến toàn cục dùng chung của hệ thống; hai trận (kể cả của hai contest khác nhau) không giẫm lên nhau.
- **Priority**: P1
- **Priority rationale**: Đây là tiền đề cấu trúc của toàn bộ epic — nếu trạng thái không tách theo trận thì không trận nào tạo mới được sau khi trận trước `FINISHED`, và khán giả/lớp phủ sẽ phải rời phòng giữa hai trận.
- **Independent test**: Trên một contest đã có kho đề và danh sách câu gán sẵn, tạo trận A, chạy một vài vòng rồi chốt trận cho `FINISHED`; tạo trận B trong cùng contest đó; xác nhận điểm và nhật ký sự kiện của A không xuất hiện trong B, còn mã phòng và kho câu đã dùng (`usedInContest`) vẫn dùng chung xuyên cả hai trận.
- **Related PRD requirements**: PRD-REQ-027
- **Related game rules**: `GR-031` (phạm vi không-lặp-câu là toàn contest, xuyên mọi trận)
- **Related journey**: JOURNEY-005, JOURNEY-007

---

### US-002 — Bắt đầu trận đóng băng cấu hình, đúng một lần cho cả đời trận (Priority: P1)

- **Title**: Cú bấm bắt đầu trận là mốc đóng băng cấu hình
- **Actor**: ACTOR-001 admin
- **Intent**: Bấm bắt đầu trận để khoá vào trận đó cấu hình luật, mode trả lời, danh sách câu đã gán và cờ hiện đáp án — sao cho sửa contest sau đó không đụng trận đang chạy.
- **User value**: Giải `PS-7` — nhật ký sự kiện của một trận luôn dựng lại được đúng những gì đã xảy ra, vì luật áp dụng cho trận đó không đổi giữa chừng.
- **Priority**: P1
- **Priority rationale**: Nếu cấu hình có thể đổi giữa trận, biên bản trận mất giá trị phân xử và các con số trong nhật ký sự kiện không còn giải thích được.
- **Independent test**: Bắt đầu một trận (đóng băng cấu hình lần một), sau đó sửa RuleConfig hoặc danh sách câu ở cấp contest cho trận **kế tiếp**; xác nhận trận đang chạy hiện tại không đổi hành vi ở bất kỳ vòng nào.
- **Related PRD requirements**: PRD-REQ-028
- **Related game rules**: `GR-031`, `GR-037`
- **Related journey**: JOURNEY-005

---

### US-003 — LOBBY là cửa vào/ra duy nhất; admin chọn vòng nào mở tiếp (Priority: P1)

- **Title**: Trạng thái nghỉ của trận và quyền chọn vòng của admin
- **Actor**: ACTOR-001 admin
- **Intent**: Từ `LOBBY`, mở bất kỳ vòng nào theo lựa chọn thực tế của buổi thi (lý do sân khấu, MC, thời lượng), không bị ép theo một playlist cứng.
- **User value**: Hệ thống khuyến nghị nhưng không khoá cứng thứ tự — admin luôn có đường đi tiếp kể cả khi thứ tự thực tế lệch khuyến nghị.
- **Priority**: P1
- **Priority rationale**: Không có `LOBBY` làm cửa vào/ra duy nhất thì không có điểm neo chung để mọi vòng dựa vào — và ép cứng thứ tự vòng sẽ chặn đúng lúc buổi thi cần linh hoạt nhất.
- **Independent test**: Từ `LOBBY`, mở một vòng khác thứ tự khuyến nghị của hệ thống; xác nhận hệ thống hiện dialog cảnh báo nêu tên luật bị lệch, cho ép qua bằng Yes — độc lập với việc trận đó có vòng nào đã chạy hay chưa.
- **Related PRD requirements**: PRD-REQ-029, PRD-REQ-030
- **Related game rules**: `GR-030` C4 · `INV-020`
- **Related journey**: JOURNEY-005

---

### US-004 — Bốn cửa ra chủ động của một vòng đang chạy (Priority: P1)

- **Title**: Kết thúc vòng · kết thúc khẩn cấp · bỏ vòng · chạy lại vòng
- **Actor**: ACTOR-001 admin
- **Intent**: Đưa một vòng đang chạy về `LOBBY` qua đúng một trong bốn cửa, chọn đúng cửa theo tình huống (vòng chạy đủ, vòng hỏng giữa chừng cần giữ điểm, vòng cần huỷ và làm lại, vòng cần bỏ hẳn).
- **User value**: Giải `PS-7` — admin luôn có công cụ khép một vòng bất thường mà không bị kẹt, và mỗi lựa chọn để lại đúng dấu vết cần thiết trong biên bản.
- **Priority**: P1
- **Priority rationale**: Nếu chỉ có "kết thúc vòng" thường, một vòng hỏng giữa chừng (mất kết nối hàng loạt, lỗi thiết bị) sẽ khiến admin không có đường thoát nào giữ được điểm đã ghi.
- **Independent test**: Giả lập một vòng đang chạy dở (mới hỏi 2/4 câu Tăng tốc); dùng "kết thúc sớm" và xác nhận bảng điểm không đổi; ở một vòng khác dùng "bỏ vòng" và xác nhận điểm của vòng đó bị đảo bằng sự kiện mới được **thêm vào** nhật ký (event cũ không bị xoá).
- **Related PRD requirements**: PRD-REQ-031
- **Related game rules**: `GR-030` (C1→C5)
- **Related journey**: JOURNEY-006

---

### US-005 — Chốt trận: tính bảng điểm và điều kiện hoà tại đúng cú bấm (Priority: P1)

- **Title**: Chốt trận, rẽ nhánh Câu hỏi phụ, và chốt lần hai sau khi phân định
- **Actor**: ACTOR-001 admin
- **Intent**: Bấm Chốt trận để server tính bảng điểm cuối và điều kiện hoà **ngay tại thời điểm đó** — không sớm hơn, không tự động — và rẽ sang `TIE_BREAK` nếu có hoà ở vị trí Nhất.
- **User value**: Điểm sửa ở `LOBBY` trước khi chốt vẫn có hiệu lực đầy đủ trong phép phân định hoà, và trận không tự đóng sổ khi hết playlist, tránh xung đột với quyền sửa điểm ở `LOBBY`.
- **Priority**: P1
- **Priority rationale**: Đóng sổ tự động sẽ mâu thuẫn trực tiếp với quyền sửa điểm ở `LOBBY`, vì sửa điểm có thể tạo ra hoặc gỡ một nhóm hoà ngay trước cú chốt.
- **Independent test**: Ở `LOBBY` sau vòng Về đích, chỉnh tay điểm để tạo một nhóm hoà mới ở vị trí Nhất, rồi bấm Chốt trận; xác nhận trận rẽ sang `TIE_BREAK` dựa trên bảng điểm **đã sửa**, không phải bảng điểm trước khi sửa.
- **Related PRD requirements**: PRD-REQ-032, PRD-REQ-105
- **Related game rules**: `GR-022` (toàn bộ bảng quyết định)
- **Related journey**: JOURNEY-007

---

### US-006 — Trận đóng sổ là terminal và niêm phong; van thoát là tạo trận mới (Priority: P1)

- **Title**: `FINISHED` không sửa được; muốn tiếp tục thì tạo trận mới cùng contest
- **Actor**: ACTOR-001 admin
- **Intent**: Sau khi trận đóng sổ, không còn thao tác nào sửa được bảng điểm; muốn thi tiếp thì tạo một trận mới trong cùng contest, độc lập với trận đã đóng sổ.
- **User value**: Biên bản đã xuất giữ nguyên giá trị phân xử — không ai, kể cả admin, sửa ngược được kết quả sau khi đã công bố.
- **Priority**: P1
- **Priority rationale**: Nếu sửa được sau khi đóng sổ, biên bản mất ý nghĩa làm căn cứ phân xử khiếu nại.
- **Independent test**: Trên một trận đã `FINISHED`, thử điều chỉnh điểm, bỏ vòng, và chạy lại vòng; xác nhận cả ba đều không thực hiện được (nút không tồn tại và server từ chối nếu request lọt tới); sau đó tạo một trận mới trong cùng contest và xác nhận nó khởi đầu độc lập ở `LOBBY`.
- **Related PRD requirements**: PRD-REQ-033
- **Related game rules**: `GR-029` C6
- **Related journey**: JOURNEY-007

---

### US-007 — Huỷ trận bất kỳ lúc nào, đóng sổ với nhãn "bỏ dở" (Priority: P2)

- **Title**: Huỷ trận khi có sự cố nghiêm trọng
- **Actor**: ACTOR-001 admin
- **Intent**: Huỷ một trận đang chạy (mất điện, hỏng thiết bị, huỷ buổi thi) và khép nó lại với một nhãn ghi rõ đây là kết quả bỏ dở, không phải một kết quả hoàn chỉnh.
- **User value**: Sự cố nghiêm trọng có một lối thoát có kiểm soát — trận không bị treo mãi ở trạng thái đang chạy, và biên bản phản ánh đúng bản chất bỏ dở của nó.
- **Priority**: P2
- **Priority rationale**: Giữ nguyên mức ưu tiên đã chốt ở PRD (`PRD-REQ-034` P2) — đây là đường xử lý sự cố hiếm gặp, không phải luồng vận hành trận thường xuyên; không đáng thêm một trạng thái thứ năm vì không có transition nào rẽ nhánh theo nó.
- **Independent test**: Huỷ một trận đang ở giữa một vòng bất kỳ; xác nhận điểm giữ nguyên không revert, không có người thắng, hệ thống bắt buộc nhập lý do, và biên bản in nhãn "trận bỏ dở" kèm đủ các vòng đã chạy.
- **Related PRD requirements**: PRD-REQ-034
- **Related game rules**: `GR-022` C5
- **Related journey**: JOURNEY-006

---

### US-008 — Sửa danh sách câu đã gán tại LOBBY (Priority: P1)

- **Title**: Thêm/bớt câu ở cửa vào từng vòng, chạy lại pre-flight
- **Actor**: ACTOR-001 admin
- **Intent**: Ở `LOBBY`, thêm hoặc bớt câu trong danh sách đã gán của contest khi một vòng sắp mở báo thiếu đề, hoặc khi muốn đổi câu trước khi vòng chạy.
- **User value**: Đây là lối thoát duy nhất của chặn cứng "cửa vào vòng thiếu câu" — admin không bị kẹt khi kho câu đã gán không đủ cho một vòng.
- **Priority**: P1
- **Priority rationale**: Không có cách sửa danh sách giữa các vòng thì một trận thiếu câu ở vòng thứ ba sẽ không có đường tiếp tục nào khác ngoài huỷ trận.
- **Independent test**: Ở một trận có vòng bị chặn vì thiếu câu, bổ sung câu vào danh sách đã gán tại `LOBBY`; xác nhận pre-flight chạy lại ngay và vòng đó mở được; thử gỡ một câu đã hiển thị ở vòng trước và xác nhận bị chặn.
- **Related PRD requirements**: PRD-REQ-035
- **Related game rules**: `GR-031` C3, C5, C6, C7, C8
- **Related journey**: JOURNEY-006

---

### US-009 — Đặt chỗ câu Câu hỏi phụ tại LOBBY, trước khi biết có hoà (Priority: P3)

- **Title**: Chỉ định câu dự phòng cho Câu hỏi phụ dưới dạng đặt chỗ
- **Actor**: ACTOR-001 admin
- **Intent**: Ở bất kỳ `LOBBY` nào cho tới mốc bấm Chốt trận, chỉ định trước một số câu còn khả dụng (chưa hiển thị) làm câu dự phòng cho vòng Câu hỏi phụ, và gỡ chỉ định lại được nếu đổi ý.
- **User value**: Admin chọn được câu phù hợp làm câu phân định (ngắn gọn, dễ đọc) thay vì để hệ thống rút ngẫu nhiên một câu Về đích 30 điểm dài dòng cho một vòng chỉ có 15 giây suy nghĩ.
- **Priority**: P3
- **Priority rationale**: Giữ nguyên mức ưu tiên đã chốt ở PRD (`PRD-REQ-091` P3) — tính năng tồn tại vì sự **phù hợp** của câu hỏi, không phải vì thiếu đề; trận vẫn chạy được đầy đủ Câu hỏi phụ mà không cần chỉ định gì.
- **Independent test**: Ở `LOBBY` sau khi vòng Về đích đã chạy (`LOBBY` cuối), chỉ định 3 câu làm Câu hỏi phụ; xác nhận pre-flight của vòng Câu hỏi phụ không đòi thêm câu nào từ kho gốc. Độc lập kiểm ở `LOBBY` trước vòng Về đích: chỉ định 3 câu Về đích và xác nhận pre-flight của vòng Về đích đòi thêm đúng 3 câu.
- **Related PRD requirements**: PRD-REQ-091
- **Related game rules**: `GR-023` (phần chỉ định câu Câu hỏi phụ) · `GR-031` (ranh giới "đã hiển thị") · `GR-022`
- **Related journey**: JOURNEY-006, JOURNEY-007

---

### Edge Cases

- **Chốt trận phát hiện hoà nhưng thiếu câu Câu hỏi phụ**: khi đủ điều kiện kích hoạt `TIE_BREAK` (≥2 người hoà ở vị trí Nhất) nhưng không đủ 3 câu khả dụng, trận **ở lại `LOBBY`** — Chốt trận bị chặn cứng cùng cơ chế "cửa vào vòng thiếu câu" của mọi vòng khác; trận **không** chuyển sang `TIE_BREAK` trước rồi mới chặn (FR-018, AC-032).
- **Sửa điểm nhiều lần quanh mốc Chốt trận lần hai**: mỗi lần sửa điểm sau khi `TIE_BREAK` đã phân định có thể làm nhóm hoà "sống lại" hoặc "mất đối tượng" lặp đi lặp lại (`GR-022` §Còn hiệu lực nghĩa là gì) — hệ thống phải tính lại từ trạng thái điểm **hiện tại** ở mỗi cú bấm, không dựa vào lịch sử đã từng đụng tới.
- **Bấm trùng nút một chiều do độ trễ mạng**: Chốt trận, Bỏ vòng, Chạy lại vòng, Kết thúc sớm, Huỷ trận đều phải từ chối lần bấm thứ hai cho cùng một mục tiêu đã chuyển trạng thái (invalid state), không phải dedup theo nội dung yêu cầu.
- **Đặt chỗ câu Câu hỏi phụ rồi vòng gốc tiêu hết câu còn lại**: pre-flight của vòng gốc phải phản ánh đúng chi phí tăng thêm do câu bị giữ chỗ, và không được âm thầm bỏ qua chỉ định của admin khi kho co hẹp.
- **Gỡ chỉ định câu Câu hỏi phụ sau khi vòng gốc đã mở dựa trên chi phí đã tính**: câu được gỡ quay lại phép rút của vòng gốc — pre-flight của các vòng mở **sau đó** phải phản ánh đúng, các vòng đã mở trước đó không bị truy hồi.
- **Huỷ trận trong khi đang ở `TIE_BREAK`**: `GR-022` C5 cho phép huỷ trận **từ bất kỳ trạng thái nào**, gồm cả `TIE_BREAK` — trận đóng sổ với nhãn "bỏ dở", không phân định thứ hạng dù đã có một phần kết quả Câu hỏi phụ.
- **Tạo trận mới khi trận trước bị huỷ (nhãn "bỏ dở")**: `EVENT-036` tạo trận mới là thao tác cấp contest, không phụ thuộc lý do đóng sổ của trận trước (hoàn thành hay bỏ dở).
- **Sửa danh sách câu ở `LOBBY` ngay sau khi vừa `TIE_BREAK` phân định xong**: trận đã về `LOBBY` nhưng **chưa** đóng sổ — sửa danh sách câu ở mốc này vẫn hợp lệ như mọi `LOBBY` khác, và không ảnh hưởng tới kết quả `TIE_BREAK_RESOLVED` đã ghi (miễn không sửa điểm làm nhóm hoà đổi).
- **Trận vừa tạo, còn ở `LOBBY`, chưa bấm bắt đầu vòng nào**: vẫn tính là "đang chạy" đối với ràng buộc "một trận đang chạy mỗi contest" (FR-001) — hệ thống không cho tạo trận thứ hai trong cùng contest chừng nào trận này chưa `FINISHED`, kể cả khi chưa vòng nào của nó từng mở.
- **Đặt chỗ Câu hỏi phụ khi trận kết thúc (`FINISHED` hoặc "bỏ dở") rồi tạo trận mới**: danh sách đặt chỗ không mang sang trận mới — thuộc phạm vi trận, không phải phạm vi contest (FR-030, AC-033).

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: Hệ thống MUST cho một contest chứa được nhiều trận, và MUST đảm bảo tại một thời điểm chỉ có đúng **một** trận của contest đó ở trạng thái đang chạy. "Đang chạy" MUST được hiểu là **mọi trận chưa `FINISHED`**, kể cả một trận vừa tạo còn ở `LOBBY` và chưa bấm bắt đầu vòng nào — hệ thống MUST NOT cho tạo trận thứ hai trong cùng contest khi trận hiện tại chưa `FINISHED`. *(US-001 · PRD-REQ-027 · GR-031 · AC-001, AC-032)*
- **FR-002**: Hệ thống MUST giữ ở cấp **contest**: mã phòng **6 số** *(giữ nguyên xuyên mọi trận của contest, không cấp lại theo trận)*, kho đề đã gán, cấu hình gốc, và phạm vi không-lặp-câu; và MUST giữ ở cấp **trận**: điểm, nhật ký sự kiện, ghế đã gán, biên bản, cấu hình đã đóng băng. *(US-001 · PRD-REQ-027 · AC-001, AC-002)*
- **FR-003**: Không-lặp-câu MUST NOT được triển khai như một cờ cấu hình bật/tắt được — nó là hệ quả cấu trúc của phạm vi contest, không phải một lựa chọn. *(US-001 · PRD-REQ-027 · AC-002)*
- **FR-004**: Cú bấm bắt đầu trận MUST đóng băng vào trận đó: cấu hình luật (RuleConfig), mode trả lời, danh sách câu đã gán, và cờ hiện đáp án sau khi chấm (`revealAnswerAfterJudge`). Việc đóng băng MUST xảy ra đúng **một lần** cho cả đời trận. *(US-002 · PRD-REQ-028 · GR-031, GR-037 · AC-003)*
- **FR-005**: Sửa cấu hình ở cấp contest sau khi một trận đã bắt đầu MUST NOT làm đổi hành vi của trận đó; thay đổi chỉ MUST áp dụng cho trận **kế tiếp** được tạo trong contest đó. *(US-002 · PRD-REQ-028 · AC-004)*
- **FR-006**: Trạng thái nghỉ của trận (`LOBBY`) MUST có đúng **một** giá trị, dùng chung cho cả trước vòng đầu tiên lẫn giữa hai vòng bất kỳ. *(US-003 · PRD-REQ-029 · TERM-019 · AC-005)*
- **FR-007**: Mọi vòng MUST vào và ra qua `LOBBY`; hệ thống MUST NOT cung cấp một đường chuyển tiếp đi thẳng từ vòng này sang vòng khác. *(US-003 · PRD-REQ-029 · GR-030 C4 · AC-006)*
- **FR-008**: Admin MUST chọn được vòng nào mở tiếp từ `LOBBY`. Hệ thống MUST khuyến nghị vòng kế theo luật và cấu hình contest, và khi admin chọn khác khuyến nghị, hệ thống MUST hiện cảnh báo dạng dialog cho ép qua — MUST NOT chặn cứng lựa chọn đó. *(US-003 · PRD-REQ-030 · INV-020 · AC-007)*
- **FR-009**: Một vòng đang chạy MUST rời về `LOBBY` qua đúng bốn cửa: **kết thúc vòng** (đủ câu, giữ điểm), **kết thúc khẩn cấp** (bấm được cả khi chưa đủ câu, giữ nguyên điểm, nhãn "kết thúc sớm"), **bỏ vòng** (hoàn nguyên điểm, nhãn "đã bỏ"), **chạy lại vòng** (hoàn nguyên rồi chạy mới, qua lại cửa kiểm kho đề). *(US-004 · PRD-REQ-031 · GR-030 · AC-008 → AC-011)*
- **FR-010**: Ba cửa ra "kết thúc khẩn cấp", "bỏ vòng", "chạy lại vòng" MUST đi qua dialog hạng phá huỷ — không tắt được bằng cách khác ngoài Yes/No — và MUST bắt nhập lý do trước khi thực hiện. *(US-004 · PRD-REQ-031 · GR-030 · AC-008 → AC-010)*
- **FR-011**: "Kết thúc khẩn cấp" MUST giữ nguyên điểm hiện tại, MUST NOT sinh bất kỳ sự kiện hoàn nguyên nào. *(US-004 · PRD-REQ-031 · GR-030 C3 · AC-009)*
- **FR-012**: "Bỏ vòng" MUST hoàn nguyên điểm của vòng đó bằng cách **thêm** sự kiện đảo ngược cho mọi sự kiện điểm của vòng, và biên bản MUST giữ nhãn "đã bỏ" cho vòng đó mà không xoá lịch sử. *(US-004 · PRD-REQ-031 · GR-030 C1 · AC-010)*
- **FR-013**: "Chạy lại vòng" MUST hoàn nguyên như "bỏ vòng" rồi mở lại vòng từ đầu, và MUST đi qua cửa kiểm kho đề như một vòng mới trước khi mở. *(US-004 · PRD-REQ-031 · GR-030 C2 · AC-011)*
- **FR-014**: Cả ba cửa ra phá huỷ (kết thúc khẩn cấp, bỏ vòng, chạy lại vòng) và cửa "kết thúc vòng" thường MUST NOT trả câu **đã hiển thị** về kho đề. *(US-004 · PRD-REQ-031 · GR-030, GR-031 · AC-008 → AC-011)*
- **FR-015**: Khi hết playlist các vòng, hệ thống MUST chỉ **gợi ý** chốt trận cho admin, MUST NOT tự động chuyển trận sang `FINISHED` hoặc `TIE_BREAK`. *(US-005 · PRD-REQ-032 · GR-022 · AC-012)*
- **FR-016**: Phép tính bảng điểm tích luỹ và điều kiện hoà MUST chạy tại đúng thời điểm admin bấm **Chốt trận**, trên bảng điểm ở **đúng mốc đó** — bao gồm mọi lần điều chỉnh điểm thủ công đã thực hiện trước đó ở `LOBBY`. *(US-005 · PRD-REQ-032 · GR-022 · AC-012, AC-013)*
- **FR-017**: Phạm vi phân định hoà (`tieBreakPositions`) MUST luôn chỉ gồm **vị trí Nhất** ở v1; giao diện tạo/cấu hình contest MUST NOT cho chọn giá trị khác, và server MUST từ chối mọi **request ghi** `tieBreakPositions ≠ [1]` cho một contest v1, bất kể request đến từ giao diện hay gọi thẳng API (zero-trust — FR-034). Vế *"mô hình dữ liệu vẫn nhận không lỗi cấu trúc"* của `PRD-REQ-105` MUST được đọc là **sức chứa của schema/kiểu dữ liệu** (chuẩn bị cho phiên bản sau), MUST NOT được đọc là một đường ghi hợp lệ qua API cấu hình contest của v1; lần từ chối MUST để cấu hình contest nguyên trạng. *(US-005 · PRD-REQ-105 · GR-022 · `CLAUDE.md` §Quy ước code (zero-trust) · AC-014)*
- **FR-018**: Khi bấm Chốt trận và có ≥2 người cùng điểm cao nhất ở vị trí Nhất (và nhóm đó chưa có `TIE_BREAK_RESOLVED` còn hiệu lực) **và** đủ 3 câu khả dụng cho vòng Câu hỏi phụ, hệ thống MUST chuyển trận sang `TIE_BREAK`. Khi chỉ có đúng một người điểm cao nhất, hệ thống MUST đóng sổ thẳng sang `FINISHED` với nhãn "hoàn thành". Khi có hoà nhưng ở vị trí khác vị trí Nhất, hệ thống MUST đóng sổ sang `FINISHED` và MUST ghi cả nhóm đó **đồng hạng**, hạng kế tiếp MUST nhảy qua đúng số người đồng hạng. Khi đủ điều kiện hoà ở vị trí Nhất nhưng **không đủ 3 câu khả dụng**, hệ thống MUST giữ trận ở `LOBBY` — đây MUST được đọc là **chính** chỗ chặn cứng *"cửa vào vòng thiếu câu"* của `INV-014` áp tại cú bấm Chốt trận *(vì cú bấm đó là cửa vào của vòng `TIE_BREAK`)*, MUST NOT là một chỗ chặn cứng **mới**; hệ thống MUST NOT chuyển trận sang `TIE_BREAK` trước rồi mới chặn ở cửa vào câu đầu tiên, và bảng điểm MUST giữ nguyên không đổi ở lần bấm bị chặn. Phép tìm nhóm hoà MUST chỉ xét **ghế hoạt động** (FR-036). *(US-005 · PRD-REQ-032, PRD-REQ-105 · GR-022 C1–C4 · AC-012, AC-015, AC-016, AC-032)*
- **FR-019**: Khi bấm Chốt trận **lần hai** sau khi `TIE_BREAK` đã phân định và bảng điểm không đổi so với lúc phân định, hệ thống MUST đóng sổ trực tiếp theo thứ hạng đã phân định, MUST NOT đưa trận vào lại `TIE_BREAK`. *(US-005 · GR-022 C7 · AC-017)*
- **FR-020**: Nếu admin điều chỉnh điểm làm nhóm hoà đã có `TIE_BREAK_RESOLVED` **thay đổi** (không còn bằng điểm nhau ở đúng vị trí đó), hệ thống MUST coi sự kiện phân định cũ là **mất đối tượng** (không xoá, không đánh dấu vô hiệu, không cảnh báo) và MUST chạy lại phép phân định hoà trên bảng điểm mới tại cú bấm Chốt trận kế tiếp. *(US-005 · GR-022 C8 · AC-018)*
- **FR-021**: Trận ở trạng thái `FINISHED` MUST là terminal: hệ thống MUST NOT cho điều chỉnh điểm, chạy lại vòng, bỏ vòng, hay sửa phán quyết của bất kỳ câu nào thuộc trận đó, bất kể yêu cầu tới từ giao diện hay gọi thẳng API. *(US-006 · PRD-REQ-033 · GR-029 C6 · AC-019)*
- **FR-022**: Van thoát **duy nhất** khỏi một trận `FINISHED` MUST là tạo một trận mới trong cùng contest; thao tác này MUST NOT là một sự chuyển tiếp trạng thái rời khỏi `FINISHED` của trận cũ — trận cũ giữ nguyên `FINISHED`, trận mới là một thực thể độc lập bắt đầu ở `LOBBY`. *(US-006 · PRD-REQ-033 · AC-020)*
- **FR-023**: Admin MUST huỷ được một trận **từ bất kỳ trạng thái đang chạy nào** (`LOBBY` hoặc bất kỳ vòng nào, kể cả `TIE_BREAK`). Huỷ trận MUST đưa trận về `FINISHED` với nhãn `matchClosedReason = "bỏ dở"`, MUST giữ nguyên điểm tại mốc huỷ (không revert), và MUST NOT phân định thứ hạng hay công bố người thắng. *(US-007 · PRD-REQ-034 · GR-022 C5 · AC-021)*
- **FR-024**: Huỷ trận MUST bắt buộc nhập lý do trước khi thực hiện, và MUST ghi lại người bấm (`closedBy`), thời điểm (`closedAt`), và lý do (`reason`) vào biên bản trận. *(US-007 · PRD-REQ-034 · AC-021)*
- **FR-025**: Ở `LOBBY`, admin MUST thêm và bớt được câu trong danh sách đã gán của contest, và hệ thống MUST chạy lại phép kiểm kho đề (pre-flight) ngay sau mỗi lần sửa. *(US-008 · PRD-REQ-035 · GR-031 C5 · AC-022)*
- **FR-026**: Hệ thống MUST NOT cho gỡ khỏi danh sách đã gán bất kỳ câu nào **đã hiển thị** cho thí sinh ở bất kỳ trận nào của contest; thao tác gỡ một câu như vậy MUST không thực hiện được (invalid state), không có đường ép qua. *(US-008 · PRD-REQ-035 · GR-031 C6 · AC-023)*
- **FR-027**: Hệ thống MUST cho gỡ khỏi danh sách đã gán một câu **đã rút nhưng chưa hiển thị** — câu đó coi như chưa tiêu và MUST được trả lại pool hiệu dụng của kho đề. *(US-008 · PRD-REQ-035 · GR-031 C7 · AC-024)*
- **FR-028**: Việc sửa danh sách câu đã gán MUST chỉ thực hiện được ở `LOBBY`; trong lúc một vòng đang chạy, hệ thống MUST NOT mở cửa sửa danh sách câu. *(US-008 · PRD-REQ-035 · GR-031 C8 · AC-025)*
- **FR-029**: Thông điệp báo một vòng thiếu câu (chặn cứng cửa vào vòng) MUST dẫn thẳng người dùng sang màn sửa danh sách câu tại `LOBBY`. *(US-008 · PRD-REQ-035 · AC-026)*
- **FR-030**: Ở bất kỳ `LOBBY` nào cho tới mốc bấm Chốt trận, admin MUST chỉ định được một số câu **còn khả dụng** (chưa hiển thị) làm câu dự phòng cho vòng Câu hỏi phụ, và MUST gỡ chỉ định lại được đối xứng — câu được gỡ MUST quay lại phép rút của vòng gốc. Đặt chỗ MUST thuộc **phạm vi TRẬN**: một trận mới (dù được tạo sau khi trận trước `FINISHED`, hay sau khi Huỷ trận) MUST luôn bắt đầu với danh sách đặt chỗ **trống**, không kế thừa đặt chỗ của trận trước. *(US-009 · PRD-REQ-091 · GR-023 · AC-027, AC-028, AC-033)*
- **FR-031**: Chỉ định một câu làm Câu hỏi phụ MUST là một **đặt chỗ có hiệu lực từ thời điểm chỉ định**: câu đó MUST bị loại khỏi phép rút của các vòng mở **sau đó**, và pre-flight của các vòng đó MUST phản ánh đúng chi phí câu bị giữ chỗ. Chỉ định ở `LOBBY` **trước** một vòng nguồn còn phải chạy MUST làm tăng số câu vòng đó cần; chỉ định ở `LOBBY` **cuối** (sau khi mọi vòng nguồn đã chạy) MUST không tốn thêm câu nào từ kho gốc. *(US-009 · PRD-REQ-091 · GR-023 · AC-027, AC-029)*
- **FR-032**: Chỉ định **ít hơn 3 câu** cho Câu hỏi phụ MUST được hệ thống tự bù phần thiếu theo thứ tự ưu tiên rút của `GR-023` (Về đích → Khởi động → VCNV) tại thời điểm vòng Câu hỏi phụ mở, và MUST NOT chặn cứng vì lý do chỉ định chưa đủ 3 câu. Không chỉ định gì MUST khiến hệ thống rút hoàn toàn theo thứ tự ưu tiên đó. Hệ thống MUST NOT áp giới hạn trên cho số câu chỉ định — chỉ định **nhiều hơn 3 câu** MUST được chấp nhận, và khi vòng Câu hỏi phụ mở, hệ thống MUST chỉ dùng **3 câu đầu tiên** theo thứ tự đã chỉ định, phần dư MUST giữ nguyên chỉ định (không tự gỡ, không báo lỗi). *(US-009 · PRD-REQ-091 · GR-023 · AC-030, AC-034)*
- **FR-033**: Hệ thống MUST NOT cho chỉ định làm Câu hỏi phụ một câu **đã hiển thị** — cùng ranh giới "đã hiển thị" dùng cho quy tắc không-lặp-câu (`GR-031`). *(US-009 · PRD-REQ-091 · GR-031 C6 · AC-031)*
- **FR-034**: Mọi validate và chặn/khoá mô tả ở các FR của feature này (bốn cửa ra, chốt trận, đóng sổ terminal, huỷ trận, sửa danh sách câu, đặt chỗ Câu hỏi phụ, kiểm số ghế ở cú bấm bắt đầu trận) MUST được **server thực thi**, bất kể yêu cầu đến từ giao diện admin hay gọi thẳng API/socket; giao diện MUST chỉ là lớp tăng cường UX (ẩn nút, disable, báo lỗi nhanh), MUST NOT là hàng rào duy nhất. *(US-004 → US-009 · `CLAUDE.md` §Quy ước code — zero-trust · AC-019, AC-025)*
- **FR-035**: Cú bấm **bắt đầu trận** MUST kiểm số ghế đã gán trước khi đóng băng cấu hình (FR-004). Số ghế **lớn hơn 4** MUST bị **chặn cứng, không ép được**, kèm thông điệp nêu rõ v1 chưa có thang điểm cho số ghế đó; phép gán ghế MUST giữ nguyên khi cú bắt đầu bị chặn. Số ghế **từ 1 đến 4** MUST bắt đầu được, và hệ thống MUST bù các ghế thiếu thành **ghế bỏ thi**: đặt trạng thái vô hiệu hoá ngay **trước** mốc đóng băng, điểm **luôn 0**, không thao tác được gì, giữ nguyên **vị trí**. Trận đó MUST áp **nguyên luật 4 ghế**. Hàng rào này MUST áp như nhau cho trận `official` lẫn `practice`, và MUST thi hành ở server theo FR-034. Chặn ở đây thuộc hạng **giới hạn phiên bản** của `PRD §9.4`, MUST NOT được đọc là chỗ chặn cứng thứ tư của `INV-014`. *(US-002 · PRD-REQ-114, PRD-REQ-028 · GR-036 C5b, GR-020 C6 · `QĐ-105` · AC-035, AC-036)*
- **FR-036**: Ghế bỏ thi MUST xuất hiện trên bảng điểm, bảng xếp hạng và biên bản, và MUST được **xếp hạng bình thường như một ghế 0đ**. Phép tìm nhóm hoà của FR-018 MUST **chỉ xét ghế hoạt động** — ghế bỏ thi MUST NOT là ứng viên phân định. Ghế bỏ thi MUST **vẫn được cấp lượt** ở Khởi động lượt riêng và Về đích, và admin MUST bỏ qua lượt đó **bằng tay**; mọi phép kiểm kho đề MUST giữ nguyên mẫu số là **số ghế đã gán** (`GR-017` §Biên). *(US-002, US-005 · PRD-REQ-114 · GR-022, GR-016, GR-017 §Biên, GR-036 C5b · `QĐ-105` · AC-037)*

### Key Entities

- **Contest**: Bản thiết kế bao trùm nhiều trận — giữ mã phòng 6 số, kho đề đã gán, cấu hình gốc (RuleConfig, mode trả lời), và phạm vi của quy tắc không-lặp-câu. *(TERM-010)*
- **Trận (Match)**: Một lần chạy của contest — giữ điểm, nhật ký sự kiện, ghế đã gán, biên bản, và bản sao cấu hình đã đóng băng tại thời điểm bắt đầu. Có đúng một trận đang chạy trên mỗi contest tại một thời điểm. *(TERM-011)*
- **`LOBBY`**: Trạng thái nghỉ cấp trận — cửa vào và cửa ra duy nhất của mọi vòng, dùng cả trước vòng đầu tiên lẫn giữa hai vòng, và là nơi trận đợi ở giữa lúc `TIE_BREAK` phân định xong và cú Chốt trận lần hai. *(TERM-019, STATE-001)*
- **`TIE_BREAK` (Câu hỏi phụ)**: Vòng phân định thí sinh hoà điểm ở vị trí Nhất, không cộng/trừ điểm — chỉ đổi thứ hạng. *(TERM-020, STATE-007)*
- **`FINISHED`**: Trạng thái đóng sổ, niêm phong, chỉ đọc, terminal — mang nhãn `matchClosedReason` là `hoàn thành` hoặc `bỏ dở`. *(STATE-008)*
- **Danh sách câu đã gán**: Snapshot câu hỏi mà admin chọn cho contest, dùng làm nguồn rút đề duy nhất cho mọi vòng của mọi trận trong contest đó; sửa được ở `LOBBY`.
- **Đặt chỗ Câu hỏi phụ**: Một tập câu (không giới hạn trên, khuyến nghị đủ 3 — chỉ 3 câu đầu theo thứ tự chỉ định được dùng khi vòng mở) được admin chỉ định trước làm ưu tiên rút cho vòng Câu hỏi phụ, thuộc **phạm vi trận** (trận mới luôn bắt đầu trống), có hiệu lực từ thời điểm chỉ định tới khi gỡ hoặc tới mốc bấm Chốt trận.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: 100% số lần bấm "Chốt trận" tính điều kiện hoà trên đúng bảng điểm tại thời điểm bấm — không có trường hợp nào bảng điểm dùng để phân định lệch với bảng điểm hiển thị ngay trước cú bấm.
- **SC-002**: 100% thao tác "bỏ vòng" / "chạy lại vòng" / "kết thúc khẩn cấp" đều để lại đúng nhãn tương ứng trong biên bản trận và đều đi kèm lý do đã nhập, kiểm tra được ngay sau khi xuất biên bản.
- **SC-003**: Không có thao tác sửa điểm, sửa phán quyết, hay chạy lại/bỏ vòng nào thực hiện được trên một trận đã `FINISHED`, xác minh bằng cách thử cả qua giao diện lẫn gọi thẳng API cho 100% các loại thao tác liệt kê ở FR-021.
- **SC-004**: 100% thông điệp báo một vòng bị chặn vì thiếu câu có đường dẫn trực tiếp sang màn sửa danh sách câu — admin xử lý được (bổ sung câu, quay lại mở vòng) mà không cần rời `LOBBY` sang một màn hình khác để tự tìm đường sửa.
- **SC-005**: 100% số lần tạo trận mới trong một contest đã có trận `FINISHED` đều khởi đầu với điểm, nhật ký sự kiện, và ghế đã gán trống — không kế thừa dữ liệu của trận trước.

## Edge Cases

*(Xem §User Scenarios & Testing → Edge Cases ở trên.)*

## Out of Scope

- Tạo contest, áp preset luật `O26_DEFAULT@1`, cấu hình từng vòng, chọn mode trả lời cấp contest, gán ghế và vị trí, chọn danh sách câu hỏi **ban đầu**, cấu hình chủ đề và âm thanh — tất cả thuộc EPIC-004 (`specs/004-contest-builder-luat`).
- Soạn câu hỏi, bộ đề, vòng duyệt `DRAFT` → `ACTIVE` — thuộc EPIC-002 (`specs/002-kho-de-bo-de`).
- Nhập/xuất gói contest, chuyển sang bản portable — thuộc EPIC-003 (`specs/003-nhap-xuat-goi-contest`).
- Cơ chế rút đề theo bộ của VCNV, thể thức ba câu và chuông của Câu hỏi phụ, chấm điểm, hàng đợi tín hiệu, **kích hoạt tay một tín hiệu sau một cú *Huỷ kết quả*** (`PRD-REQ-113`, `QĐ-104`), mốc thời gian trong một câu, mất kết nối và giữ ghế (`GR-036` chi tiết), server time — thuộc EPIC-006 (`specs/006-game-engine-luat-thi-dau`).
- Bốn mốc bấm của một câu, màn chấm chi tiết với tô khác biệt ký tự, duyệt/từ chối tín hiệu, **bề mặt giao diện của kích hoạt tay**, ba hạng cảnh báo giao diện — thuộc EPIC-007 (Điều khiển và can thiệp của admin), chưa có spec riêng tại thời điểm viết tài liệu này.
- Xác thực, phân quyền, một phiên giữ quyền điều khiển, giành quyền điều khiển khi mất kết nối — thuộc EPIC-001 (`specs/001-xac-thuc-phan-quyen`).
- Luật cho số ghế **trên 4** (`NON-GOAL-012`) và playlist tuỳ ý (`NON-GOAL-011`) — không thuộc v1 dưới bất kỳ epic nào. *(Trận **dưới** 4 ghế thì **thuộc** feature này qua FR-035, FR-036 — xem `PRD-REQ-114`, `QĐ-105`.)*

- **Dựng danh sách câu gán khi contest chưa có trận nào đang chạy** — EPIC-004, `specs/004` FR-018 → FR-020. Feature này sở hữu việc sửa danh sách của một **trận đang tồn tại, tại `LOBBY` giữa các vòng** (`QĐ-161`).

## Open Questions

Không còn mục nào đang mở.

## Traceability Matrix

| User story | PRD requirement | Game rules | Functional requirements | Acceptance scenarios |
|---|---|---|---|---|
| US-001 | PRD-REQ-027 | GR-031 | FR-001, FR-002, FR-003 | AC-001, AC-002, AC-032 |
| US-002 | PRD-REQ-028, PRD-REQ-114 | GR-031, GR-037, GR-036 C5b, GR-020 C6 | FR-004, FR-005, FR-035, FR-036 | AC-003, AC-004, AC-035, AC-036, AC-037 |
| US-003 | PRD-REQ-029, PRD-REQ-030 | GR-030 C4, INV-020 | FR-006, FR-007, FR-008 | AC-005, AC-006, AC-007 |
| US-004 | PRD-REQ-031 | GR-030 (C1–C5) | FR-009, FR-010, FR-011, FR-012, FR-013, FR-014 | AC-008, AC-009, AC-010, AC-011 |
| US-005 | PRD-REQ-032, PRD-REQ-105 | GR-022 (C1–C8) | FR-015, FR-016, FR-017, FR-018, FR-019, FR-020 | AC-012, AC-013, AC-014, AC-015, AC-016, AC-017, AC-018, AC-032 |
| US-006 | PRD-REQ-033 | GR-029 C6 | FR-021, FR-022 | AC-019, AC-020 |
| US-007 | PRD-REQ-034 | GR-022 C5 | FR-023, FR-024 | AC-021 |
| US-008 | PRD-REQ-035 | GR-031 (C3, C5–C8) | FR-025, FR-026, FR-027, FR-028, FR-029 | AC-022, AC-023, AC-024, AC-025, AC-026 |
| US-009 | PRD-REQ-091 | GR-023, GR-031 C6, GR-022 | FR-030, FR-031, FR-032, FR-033 | AC-027, AC-028, AC-029, AC-030, AC-031, AC-033, AC-034 |
| (cross-cutting) | — | — | FR-034 | AC-019, AC-025 |

### Acceptance Scenarios (chi tiết)

**AC-001** — *Related user story*: US-001 · *Related FR*: FR-001 · *Related game rules*: GR-031
**Given** một contest đã có kho đề `ACTIVE` và danh sách câu đã gán đủ cho ít nhất một vòng, chưa có trận nào từng chạy,
**When** admin tạo trận A và bắt đầu chạy vòng Khởi động,
**Then** hệ thống ghi nhận đúng một trận đang chạy (trận A) cho contest đó; admin không tạo được một trận thứ hai **đang chạy** song song trong cùng contest.

**AC-002** — *Related user story*: US-001 · *Related FR*: FR-002, FR-003 · *Related game rules*: GR-031
**Given** trận A của một contest đã chạy xong vài vòng và bị Chốt trận thành `FINISHED`,
**When** admin tạo trận B mới trong cùng contest đó,
**Then** trận B khởi đầu với điểm và nhật ký sự kiện trống, ghế được gán lại từ đầu, **nhưng** cờ `usedInContest` của các câu đã hiển thị ở trận A vẫn được trận B tôn trọng — dữ liệu không thay đổi ở phía contest (không có cờ nào bị đặt lại chỉ vì tạo trận mới).

**AC-003** — *Related user story*: US-002 · *Related FR*: FR-004
**Given** một trận vừa được tạo, còn ở `LOBBY`, chưa từng bấm bắt đầu trận,
**When** admin bấm nút bắt đầu trận lần đầu tiên,
**Then** hệ thống chụp (snapshot) RuleConfig, mode trả lời, danh sách câu đã gán, và cờ `revealAnswerAfterJudge` hiện tại của contest vào trận đó, và không có đường thao tác nào lặp lại bước đóng băng này lần thứ hai cho cùng trận.

**AC-004** — *Related user story*: US-002 · *Related FR*: FR-005
**Given** một trận đã bắt đầu (đã đóng băng cấu hình) và đang chạy vòng VCNV,
**When** admin sửa RuleConfig ở cấp contest (ví dụ đổi thời gian suy nghĩ Tăng tốc),
**Then** trận đang chạy tiếp tục dùng cấu hình đã đóng băng ban đầu không đổi; thay đổi chỉ được áp dụng khi admin tạo một trận mới sau đó.

**AC-005** — *Related user story*: US-003 · *Related FR*: FR-006
**Given** một trận vừa tạo (chưa vòng nào từng chạy) và một trận khác đã chạy xong vòng Khởi động (đang giữa hai vòng),
**When** so sánh trạng thái nghỉ của cả hai,
**Then** cả hai đều ở cùng một giá trị trạng thái `LOBBY` — hệ thống không phân biệt "trước trận" và "giữa hai vòng" bằng hai trạng thái khác nhau.

**AC-006** — *Related user story*: US-003 · *Related FR*: FR-007 · *Related game rules*: GR-030 C4
**Given** trận đang ở vòng Khởi động (chưa kết thúc bằng bất kỳ cửa ra nào),
**When** admin tìm cách mở trực tiếp vòng VCNV từ màn hình đang chạy Khởi động,
**Then** không tồn tại nút hay đường API nào cho thao tác đó (invalid state) — admin buộc phải rời vòng Khởi động về `LOBBY` qua một trong bốn cửa ra trước khi mở VCNV.

**AC-007** — *Related user story*: US-003 · *Related FR*: FR-008 · *Related game rules*: GR-030 C4, INV-020
**Given** trận đang ở `LOBBY`, hệ thống khuyến nghị mở vòng VCNV kế tiếp theo playlist chuẩn,
**When** admin chọn mở vòng Tăng tốc thay vì VCNV,
**Then** hệ thống hiện dialog cảnh báo nêu rõ tên luật/thứ tự bị lệch, và cho admin bấm Yes để tiếp tục mở Tăng tốc — không chặn cứng lựa chọn này.

**AC-008** — *Related user story*: US-004 · *Related FR*: FR-009, FR-010, FR-014 · *Related game rules*: GR-030 (kết thúc vòng thường)
**Given** vòng Khởi động lượt chung đã xử lý xong đúng 12/12 câu theo luật,
**When** admin bấm "Kết thúc vòng",
**Then** trận về `LOBBY`, điểm giữ nguyên theo các phán quyết đã chấm, và không câu nào trong 12 câu đó được trả lại kho.

**AC-009** — *Related user story*: US-004 · *Related FR*: FR-009, FR-010, FR-011 · *Related game rules*: GR-030 C3
**Given** vòng Tăng tốc mới chấm xong 2/4 câu và gặp sự cố kỹ thuật buộc phải dừng,
**When** admin chọn "Kết thúc sớm", xác nhận dialog phá huỷ kèm lý do,
**Then** trận về `LOBBY` với nhãn vòng "kết thúc sớm", điểm của 2 câu đã chấm giữ nguyên không đổi, và không có sự kiện hoàn nguyên nào được sinh ra.

**AC-010** — *Related user story*: US-004 · *Related FR*: FR-009, FR-010, FR-012, FR-014 · *Related game rules*: GR-030 C1
**Given** vòng Khởi động đã cộng tổng cộng +30 điểm cho các thí sinh qua các câu đã chấm,
**When** admin chọn "Bỏ vòng", xác nhận dialog phá huỷ kèm lý do,
**Then** trận về `LOBBY` với nhãn vòng "đã bỏ", một sự kiện đảo ngược mới được **thêm** vào nhật ký làm điểm của vòng đó về lại mức trước khi vòng chạy, và sự kiện điểm gốc của vòng vẫn còn nguyên trong nhật ký (không bị xoá).

**AC-011** — *Related user story*: US-004 · *Related FR*: FR-009, FR-010, FR-013, FR-014 · *Related game rules*: GR-030 C2
**Given** vòng VCNV đã tiêu hết 4 hàng ngang của một bộ, và kho đề còn đủ ít nhất một bộ VCNV khác chưa dùng,
**When** admin chọn "Chạy lại vòng", xác nhận dialog phá huỷ kèm lý do,
**Then** điểm của vòng VCNV cũ được hoàn nguyên, vòng VCNV mới đi qua cửa kiểm kho đề (đếm bộ nguyên vẹn) trước khi mở, và vòng mới rút một bộ **khác** bộ đã dùng trước đó.

**AC-012** — *Related user story*: US-005 · *Related FR*: FR-015, FR-016, FR-018 · *Related game rules*: GR-022 C3, C6
**Given** trận vừa xong vòng Về đích, admin chưa bấm gì thêm, chỉ có một người điểm cao nhất duy nhất,
**When** một khoảng thời gian trôi qua mà admin chưa bấm Chốt trận,
**Then** trận vẫn đứng nguyên ở `LOBBY` (không tự đóng sổ); khi admin sau đó bấm Chốt trận, trận đóng sổ ngay thành `FINISHED` với nhãn "hoàn thành" vì chỉ có một người điểm cao nhất.

**AC-013** — *Related user story*: US-005 · *Related FR*: FR-016 · *Related game rules*: GR-022
**Given** trận đang ở `LOBBY` sau vòng Về đích với hai thí sinh A=100, B=100 đang hoà ở vị trí Nhất,
**When** admin điều chỉnh điểm tay cho A thành 110 (không còn hoà), **rồi sau đó** bấm Chốt trận,
**Then** phép phân định hoà chạy trên bảng điểm A=110, B=100 — không hoà — trận đóng sổ thẳng `FINISHED` với A hạng Nhất, **không** đi qua `TIE_BREAK`.

**AC-014** — *Related user story*: US-005 · *Related FR*: FR-017
**Given** một request gửi thẳng tới server (bỏ qua giao diện) yêu cầu đặt `tieBreakPositions = [1, 2]` cho một contest v1,
**When** server nhận request đó,
**Then** server từ chối ghi giá trị khác vị trí Nhất — mô hình dữ liệu chấp nhận cấu trúc danh sách nhiều phần tử ở tầng lưu trữ, nhưng API cấu hình contest của v1 không có đường ghi hợp lệ cho giá trị đó.

**AC-015** — *Related user story*: US-005 · *Related FR*: FR-018 · *Related game rules*: GR-022 C4
**Given** trận vừa xong vòng Về đích với điểm A=110, B=100, C=100, D=85 (hoà ở vị trí Nhì, không phải vị trí Nhất),
**When** admin bấm Chốt trận,
**Then** trận đóng sổ thẳng `FINISHED` — **không** kích hoạt `TIE_BREAK` — biên bản ghi B và C đồng hạng Nhì, D nhận hạng Tư, không có hạng Ba nào được cấp.

**AC-016** — *Related user story*: US-005 · *Related FR*: FR-018 · *Related game rules*: GR-022 C1
**Given** trận vừa xong vòng Về đích với ba thí sinh A=100, B=100, C=100 cùng hoà ở vị trí Nhất, D=85,
**When** admin bấm Chốt trận,
**Then** trận chuyển sang `TIE_BREAK` với cả ba A, B, C — nhóm hoà ở vị trí Nhất có ba người vẫn được xử lý như **một** nhóm, không tách thành các cặp phân định riêng.

**AC-017** — *Related user story*: US-005 · *Related FR*: FR-019 · *Related game rules*: GR-022 C7
**Given** trận đã qua `TIE_BREAK` (A thắng phân định giữa A=100, B=100), trận đã về `LOBBY`, admin chưa sửa điểm gì thêm,
**When** admin bấm Chốt trận lần thứ hai,
**Then** trận đóng sổ ngay thành `FINISHED` theo thứ hạng đã phân định (A hạng Nhất), **không** đưa trận vào lại `TIE_BREAK` một lần nữa.

**AC-018** — *Related user story*: US-005 · *Related FR*: FR-020 · *Related game rules*: GR-022 C8
**Given** trận đã qua `TIE_BREAK` (A thắng phân định giữa A=100, B=100), trận về `LOBBY`,
**When** admin sửa điểm A từ 100 lên 110 (nhóm {A,B} không còn hoà nữa), **rồi** bấm Chốt trận lần thứ hai,
**Then** sự kiện phân định cũ (`TIE_BREAK_RESOLVED` giữa A và B) bị coi là mất đối tượng — không áp dụng; trận đóng sổ `FINISHED` với A hạng Nhất tính **theo điểm 110** hiện tại, không cần dựa vào kết quả `TIE_BREAK` cũ.

**AC-019** — *Related user story*: US-006 · *Related FR*: FR-021, FR-034
**Given** một trận đã ở `FINISHED`,
**When** admin thử điều chỉnh điểm thủ công, hoặc một request gọi thẳng API server với cùng ý định,
**Then** cả hai đường đều bị từ chối — giao diện không hiện nút cho thao tác đó, và server từ chối request API tương ứng; điểm của trận không thay đổi.

**AC-020** — *Related user story*: US-006 · *Related FR*: FR-022
**Given** một trận ở `FINISHED` với nhãn "hoàn thành",
**When** admin tạo một trận mới trong cùng contest,
**Then** trận `FINISHED` cũ **giữ nguyên** trạng thái và dữ liệu, không có transition nào rời khỏi `FINISHED` của nó; trận mới là một thực thể riêng khởi đầu ở `LOBBY` với điểm và nhật ký trống.

**AC-021** — *Related user story*: US-007 · *Related FR*: FR-023, FR-024 · *Related game rules*: GR-022 C5
**Given** trận đang chạy giữa vòng Tăng tốc, mất điện đột ngột buộc phải dừng buổi thi,
**When** admin (qua phiên khác nếu cần) bấm "Huỷ trận" và bắt buộc nhập lý do,
**Then** trận chuyển ngay sang `FINISHED` với `matchClosedReason = "bỏ dở"`, điểm giữ nguyên tại mốc huỷ, biên bản không có mục người thắng, và ghi rõ người bấm, thời điểm, lý do đã nhập.

**AC-022** — *Related user story*: US-008 · *Related FR*: FR-025 · *Related game rules*: GR-031 C5
**Given** trận đang ở `LOBBY`, một vòng sắp mở báo pre-flight thiếu 2 câu,
**When** admin thêm 2 câu phù hợp vào danh sách đã gán,
**Then** pre-flight chạy lại ngay và báo đủ câu; cờ `usedInContest` của các câu khác trong danh sách không bị đụng tới.

**AC-023** — *Related user story*: US-008 · *Related FR*: FR-026 · *Related game rules*: GR-031 C6
**Given** một câu đã hiển thị cho thí sinh ở trận hiện tại hoặc một trận trước đó của cùng contest,
**When** admin thử gỡ câu đó khỏi danh sách đã gán,
**Then** thao tác không thực hiện được — không có nút hay đường API cho việc gỡ câu đã tiêu, và danh sách đã gán giữ nguyên.

**AC-024** — *Related user story*: US-008 · *Related FR*: FR-027 · *Related game rules*: GR-031 C7
**Given** một câu đã được rút cho một lượt (đã sinh `QUESTIONS_DRAWN`) nhưng chưa hiển thị cho thí sinh,
**When** admin gỡ câu đó khỏi danh sách đã gán,
**Then** hệ thống cho phép, và câu đó trở về pool hiệu dụng của kho đề (không tính là đã tiêu).

**AC-025** — *Related user story*: US-008 · *Related FR*: FR-028, FR-034 · *Related game rules*: GR-031 C8
**Given** một vòng đang chạy (không ở `LOBBY`),
**When** admin thử sửa danh sách câu đã gán qua giao diện, hoặc một request gọi thẳng API server với cùng ý định,
**Then** cả hai đều bị từ chối — cửa sửa chỉ mở ở `LOBBY`, và danh sách câu đã gán giữ nguyên trong suốt vòng đang chạy.

**AC-026** — *Related user story*: US-008 · *Related FR*: FR-029
**Given** admin thử mở một vòng đang thiếu câu theo pre-flight,
**When** hệ thống hiện thông điệp báo thiếu đề,
**Then** thông điệp đó có một đường dẫn/thao tác đưa admin thẳng sang màn sửa danh sách câu tại `LOBBY`, không cần admin tự tìm màn hình đó.

**AC-027** — *Related user story*: US-009 · *Related FR*: FR-030, FR-031 · *Related game rules*: GR-023
**Given** trận đang ở `LOBBY` **trước khi** vòng Về đích chạy, kho câu Về đích của contest đủ dùng cho vòng Về đích (24 câu) và còn dư,
**When** admin chỉ định 3 câu Về đích còn khả dụng làm câu dự phòng Câu hỏi phụ,
**Then** ba câu đó bị loại khỏi phép rút của vòng Về đích sắp mở; pre-flight của vòng Về đích đòi thêm đúng 3 câu so với mức thông thường (27 thay vì 24).

**AC-028** — *Related user story*: US-009 · *Related FR*: FR-030
**Given** admin đã chỉ định 3 câu làm Câu hỏi phụ ở một `LOBBY` trước đó,
**When** admin gỡ chỉ định của một trong ba câu đó,
**Then** câu được gỡ quay lại phép rút bình thường của vòng gốc mà nó thuộc về, và pre-flight của vòng gốc (nếu chưa mở) phản ánh đúng việc giảm chi phí giữ chỗ.

**AC-029** — *Related user story*: US-009 · *Related FR*: FR-031
**Given** trận đang ở `LOBBY` **sau khi** vòng Về đích đã chạy xong (kho Về đích còn dư đúng 12 câu theo cơ chế tất định của `GR-023`),
**When** admin chỉ định 3 câu từ phần dư đó làm câu dự phòng Câu hỏi phụ,
**Then** pre-flight của vòng Câu hỏi phụ **không đòi thêm câu nào** từ các vòng nguồn khác — chi phí chỉ định ở `LOBBY` cuối là 0.

**AC-030** — *Related user story*: US-009 · *Related FR*: FR-032 · *Related game rules*: GR-023
**Given** admin chỉ định 1 câu làm Câu hỏi phụ ở `LOBBY`, không chỉ định thêm,
**When** vòng Câu hỏi phụ mở,
**Then** hệ thống dùng câu đã chỉ định cho câu đầu tiên và tự bù 2 câu còn thiếu theo thứ tự ưu tiên Về đích → Khởi động → VCNV — không có chặn cứng nào phát sinh chỉ vì chỉ định chưa đủ 3 câu.

**AC-031** — *Related user story*: US-009 · *Related FR*: FR-033 · *Related game rules*: GR-031 C6
**Given** một câu đã hiển thị cho thí sinh ở một trận trước đó của contest,
**When** admin thử chỉ định câu đó làm câu dự phòng Câu hỏi phụ,
**Then** thao tác không thực hiện được — hệ thống chỉ liệt kê câu **còn khả dụng** (chưa hiển thị) trong danh sách chọn.

**AC-032** — *Related user story*: US-001, US-005 · *Related FR*: FR-001, FR-018 · *Related game rules*: GR-022, GR-031 C3
**Given** trận đang ở `LOBBY` sau vòng Về đích với hai thí sinh A=100, B=100 hoà ở vị trí Nhất, và kho câu khả dụng cho Câu hỏi phụ (ba kho nguồn, sau khi trừ câu đã dùng và đã đặt chỗ) chỉ còn **2** câu,
**When** admin bấm Chốt trận,
**Then** trận **ở lại `LOBBY`** — Chốt trận bị chặn cứng cùng cơ chế "cửa vào vòng thiếu câu" của mọi vòng khác, hệ thống **không** chuyển trận sang `TIE_BREAK` trước rồi mới chặn ở câu đầu tiên; admin phải bổ sung câu (US-008) hoặc gỡ bớt đặt chỗ (US-009) rồi bấm lại Chốt trận.

**AC-033** — *Related user story*: US-001, US-009 · *Related FR*: FR-030
**Given** admin đã chỉ định 3 câu làm đặt chỗ Câu hỏi phụ ở trận A, sau đó trận A `FINISHED`,
**When** admin tạo trận B mới trong cùng contest,
**Then** danh sách đặt chỗ Câu hỏi phụ của trận B **trống** — không kế thừa ba câu đã chỉ định ở trận A; admin phải chỉ định lại từ đầu cho trận B nếu muốn dùng cơ chế này.

**AC-034** — *Related user story*: US-009 · *Related FR*: FR-032
**Given** trận đang ở `LOBBY`, admin chỉ định 5 câu còn khả dụng làm đặt chỗ Câu hỏi phụ (nhiều hơn mức 3 câu cần dùng),
**When** vòng Câu hỏi phụ mở,
**Then** hệ thống dùng đúng **3 câu đầu tiên** theo thứ tự đã chỉ định cho ba câu của vòng, và **2 câu dư** vẫn giữ nguyên trạng thái đã chỉ định (không tự gỡ, không báo lỗi) dù không được dùng trong vòng đó.

**AC-035** — *Related user story*: US-002 · *Related FR*: FR-035 · *PRD*: `PRD-REQ-114`
**Given** một contest đã gán **5 ghế** *(giao diện gán ghế cho phép tới 12 — `specs/004` FR-030)*, kho đề đủ cho mọi vòng,
**When** admin bấm **bắt đầu trận**, kể cả khi yêu cầu được gửi thẳng tới API,
**Then** server **từ chối, không có đường ép qua** kèm thông điệp nêu rõ v1 chưa có thang điểm cho hơn 4 đơn vị điểm · cấu hình **chưa** bị đóng băng · **phép gán 5 ghế giữ nguyên**, không ghế nào bị xoá · hành vi **y hệt** với contest `practice`.

**AC-036** — *Related user story*: US-002 · *Related FR*: FR-035 · *PRD*: `PRD-REQ-114`
**Given** một contest chỉ gán **2 thí sinh** vào ghế, kho đề đủ,
**When** admin bấm **bắt đầu trận**,
**Then** trận bắt đầu bình thường · hai ghế còn lại tồn tại ở trạng thái **bỏ thi** *(vô hiệu hoá, 0đ, giữ vị trí)* ngay từ trước mốc đóng băng cấu hình · trận chạy trọn bốn vòng theo **nguyên luật 4 ghế** · hai ghế bỏ thi **không** phát được tín hiệu nào.

**AC-037** — *Related user story*: US-002, US-005 · *Related FR*: FR-036 · *PRD*: `PRD-REQ-114`
**Given** trận hai thí sinh thật ở AC-036 đã chạy xong, A được 90đ, B được 40đ, hai ghế bỏ thi 0đ,
**When** admin bấm **Chốt trận**,
**Then** bảng xếp hạng ghi đủ **bốn** ghế theo điểm — A hạng nhất, B hạng nhì, hai ghế bỏ thi **đồng hạng ba** với 0đ · phép tìm nhóm hoà **chỉ xét A và B** nên nhóm hai ghế bỏ thi **không** kích hoạt `TIE_BREAK` · biên bản in đủ bốn ghế kèm nhãn *bỏ thi*.

## Assumptions

- Việc "tạo trận mới" và mọi thao tác vòng đời trận mô tả ở đây đều do **admin** thực hiện qua phiên đang giữ quyền điều khiển của contest đó (`QĐ-008`); mô hình phân quyền chi tiết (ai được tạo trận, chuyển giao quyền điều khiển) thuộc EPIC-001, không lặp lại ở đây.
- Biên bản trận ("kết thúc sớm", "đã bỏ", "bỏ dở") được xuất dưới dạng đọc được (PDF hoặc tương đương) theo `GOAL-007`; định dạng cụ thể của biên bản không thuộc phạm vi đặc tả này.
