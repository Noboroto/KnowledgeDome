---
phase: 4
title: "Import / Export đề"
status: pending
priority: P2
dependencies: [3]
---

# Phase 4: Import / Export đề

## Overview
Import/export kho đề dạng ZIP bundle tự định nghĩa (manifest + questions.json + media/), thêm import Excel/CSV cho người ra đề quen bảng tính. Roundtrip an toàn, không rò đáp án cho role không đủ quyền.

## Requirements
- Functional: export theo filter (vòng/topic/bộ đề đã chọn) ra `.zip`; import zip có validate + preview + báo lỗi từng dòng; import Excel/CSV với cột mapping linh hoạt (không hard-code vùng ô như Athena cũ).
- Non-functional: file lớn (media nhiều) xử lý streaming, không load hết vào RAM; chỉ ADMIN và SETTER được export (SETTER: câu của mình); export ghi audit log.

## Architecture

Format bundle:
```
olympia-bank-export.zip
├── manifest.json    # { formatVersion: 1, exportedAt, counts, checksums }
├── questions.json   # QuestionExport[] — Zod schema trong packages/shared
└── media/<sha256>.<ext>
```

```mermaid
sequenceDiagram
  participant U as Setter/Admin
  participant API as apps/api
  participant M as MinIO
  U->>API: POST /question-bank/import (zip stream)
  API->>API: unzip stream, validate manifest + Zod từng question
  API-->>U: preview (n hợp lệ, m lỗi kèm dòng/lý do)
  U->>API: confirm import (chọn: bỏ qua lỗi / hủy)
  API->>M: upload media (dedupe theo sha256)
  API->>API: insert Questions (status DRAFT)
  API-->>U: kết quả + audit log
```

## Related Code Files
- Create: `apps/api/src/question-bank/` (export.service, import.service, excel-parser)
- Modify: `packages/shared` (Zod: `QuestionExportSchema`, `BundleManifestSchema` — formatVersion để migrate sau)
- Modify: `apps/web/src/pages/questions/` (modal import có bảng preview lỗi, nút export theo filter)

## Implementation Steps
1. Định nghĩa `QuestionExportSchema` (bao gồm answer — vì file export nằm ngoài hệ thống, cảnh báo rõ trong UI) + manifest.
2. Export: stream zip (archiver), media lấy từ MinIO stream-to-stream, checksum sha256.
3. Import: unzip stream (yauzl), validate 2 bước (manifest → từng question), dedupe media theo sha256, tất cả vào DRAFT để duyệt lại.
4. Excel/CSV parser (exceljs / papaparse): bước 1 upload file → server đọc header → FE cho map cột ↔ field; template mẫu tải về được.
5. Idempotency: import lại cùng bundle không nhân đôi (so `sourceId` + checksum, hỏi user overwrite/skip).
6. Audit: EXPORT log kèm số câu + filter.

## Success Criteria
- [ ] Roundtrip: export 50 câu đủ 5 loại + media → import vào DB sạch → diff logic = 0 khác biệt.
- [ ] Import file hỏng/thiếu media → báo lỗi từng mục, không ghi nửa vời (transaction).
- [ ] Import Excel với template mẫu hoạt động; cột lệch vẫn map được bằng tay.

## Risk Assessment
- Zip bomb / file độc → giới hạn tổng dung lượng giải nén + số entry; chỉ nhận MIME whitelist **kèm sniff magic bytes**; sanitize entry path chống **zip-slip** (`media/../../etc` — reject mọi entry có `..` hoặc path tuyệt đối) (red-team M4).
- Export chứa đáp án là điểm rò rỉ chính của "bảo mật kho đề" → chỉ ADMIN/SETTER-owner, audit bắt buộc, cân nhắc export có mật khẩu (zip AES) — ghi DEFERED nếu user muốn.
