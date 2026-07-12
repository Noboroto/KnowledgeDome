# Gap-analysis vòng 2 — "vòng ngoài trận đấu" (pháp lý, con người, vòng đời)

> Fresh-eyes review 12/07/2026, theo yêu cầu user "đào sâu xem tôi bỏ sót gì". KHÔNG lặp ux-gaps/red-team. ✅ = đã đưa vào plan/PRD; 🟡 = DEFERED.

## Pháp lý & Privacy

| # | Gap | Ưu tiên | Xử lý |
|---|---|---|---|
| 1.1 | **Dữ liệu cá nhân học sinh vị thành niên** (tên, trường, kết quả, IP) — không có retention/xoá/ẩn danh; NĐ 13/2023/NĐ-CP áp dụng | P1 | ✅ PRD FR-7 + phase-05 (profile theo ghế, thuộc contest), phase-06 (anonymize không phá event log); 🟡 D14 retention |
| 1.2 | Consent livestream mặt + tên học sinh | P2 | ✅ Overlay toggle ẩn tên thật/nickname (phase-09); mẫu consent tham khảo vào docs |
| 1.3 | **Bản quyền FORMAT Olympia** (tên, logo, cấu trúc vòng là tài sản VTV) — D10 mới chỉ lo nhạc | P1 | 🟡 gộp vào D7 (tên sản phẩm); UI dùng thuật ngữ generic từ RuleConfig; disclaimer không liên kết VTV |
| 1.4 | License repo chưa chọn | P1 | 🟡 D7 (cùng quyết định) |

## Quy trình con người

| # | Gap | Ưu tiên | Xử lý |
|---|---|---|---|
| 2.1 | Ai duyệt đề DRAFT→ACTIVE? Trùng đề giữa setters? | P1/P2 | ✅ v1: chỉ admin activate (phase-03); dedup fuzzy cảnh báo = P2; 🟡 D15 nếu cần role reviewer riêng |
| 2.2 | Màn hình MC riêng (đọc câu hỏi, chữ to, không phải bàn admin) | P2 | ✅ route `/mc` read-only (phase-09); 🟡 D15: MC thấy đáp án trước công bố không |
| 2.3 | Thay thí sinh phút chót / xác minh danh tính | P2 | ✅ reassign seat trước match start có audit (phase-05); danh tính = quy trình offline (runbook) |
| 2.4 | Nhân sự tối thiểu ngày thi — admin 1 người quá tải | P1 | ✅ tách ViewerPanel (kick/khoá cổng) khỏi control lock để co-host làm song song (phase-08); runbook crew ≥2; UAT đo tải 1 người |

## Vòng đời sản phẩm

| # | Gap | Ưu tiên | Xử lý |
|---|---|---|---|
| 3.1 | **Event schema versioning** — không có thì release v1.1 làm event log cũ không replay được | P1 | ✅ phase-06: mọi event có `schemaVersion` từ event đầu tiên |
| 3.2 | Docs cho NGƯỜI DÙNG (giáo viên/học sinh), không chỉ dev | P1 | ✅ phase-10: guide per role + hotkey in-app ở lobby |
| 3.3 | **Account admin đầu tiên** — chống default `admin/admin` | P1 | ✅ phase-02: CLI `create-admin` bắt nhập password, cấm default creds |
| 3.4 | Backup fail âm thầm | P2 | ✅ phase-10: backup exit≠0 → cảnh báo trên admin dashboard |

## Trải nghiệm chưa xét

| # | Gap | Ưu tiên | Xử lý |
|---|---|---|---|
| 4.1 | Practice solo cá nhân (mâu thuẫn "đáp án không rời server") | P3, schema P1 | ✅ cột `visibility PRIVATE\|PUBLIC` vào Question ngay phase-03; feature để sau; 🟡 D16 hướng sản phẩm |
| 4.2 | Trạng thái GIỮA các vòng (giao lưu, giải lao, intro thí sinh) = 30-40% thời lượng buổi thi | P2 | ✅ state `INTERMISSION` + intro animation vào phase-06/09 |
| 4.3 | Ảnh/trường/lớp thí sinh — ai nhập, lưu đâu | P2 | ✅ profile theo GHẾ (thuộc contest, xoá contest = xoá profile — lợi cho privacy) phase-05 |
| 4.4 | Viewer mobile (480/500 viewer là điện thoại) + projector kiosk | P1 (SC) | ✅ phase-09 SC: responsive ≥360px, test Android tầm trung, `?kiosk=1` |

## Kỹ thuật còn hở

| # | Gap | Ưu tiên | Xử lý |
|---|---|---|---|
| 5.1 | **Buzzer timestamp với 2 instance** — clock lệch giữa máy = bất công | P1 | ✅ phase-06: timestamp gắn tại instance OWNER; NTP/chrony vào deployment docs + checklist ngày thi |
| 5.2 | Disk full giữa trận (persist-trước-broadcast → trận đứng hình) | P2 | ✅ phase-10: log rotation, disk vào monitoring + drill disk 95% |
| 5.3 | Preset RuleConfig versioning khi O27 ra | P2 | ✅ phase-05: preset = row DB immutable có version (`O26_DEFAULT@1`) |
| 5.4 | Product telemetry (SCORE_ADJUST/trận = chỉ số chất lượng auto-match) | P3 | Ghi nhận — query từ event log sẵn, trang thống kê admin sau v1 |

## Khác

- 6.1 **Người thật dùng demo trước Phase 7-9** (P1): toàn bộ review đến giờ là AI-review-AI → thêm gate vào plan.md: 1 giáo viên + 2 học sinh thử `public/` 30 phút. ✅
- 6.2 Hết câu phụ khi hoà nhiều lần (P2): preflight yêu cầu tối thiểu N câu phụ + fallback rule → rules §7 + 🟡 D13.6. ✅
