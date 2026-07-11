/* ============================================================
   mock-data.js — Dữ liệu giả lập cho toàn bộ demo (window.MOCK)
   Câu hỏi + cấu trúc bám rule-spec luật 2026 (rules-2026.md)
   ============================================================ */
window.MOCK = {
  roomCode: "482913",

  /* ---------- 4 thí sinh ---------- */
  contestants: [
    { id: "c1", name: "Nguyễn Minh Khang", school: "THPT Chuyên Lê Hồng Phong", color: "#4d8dff", score: 0 },
    { id: "c2", name: "Trần Thuỳ Dương",   school: "THPT Chuyên Hà Nội – Amsterdam", color: "#2ed573", score: 0 },
    { id: "c3", name: "Lê Hoàng Phúc",     school: "THPT Chuyên Quốc Học Huế", color: "#ff9f43", score: 0 },
    { id: "c4", name: "Phạm Ngọc Ánh",     school: "THPT Chuyên Lam Sơn", color: "#8c6cff", score: 0 }
  ],

  /* ---------- Viewer chờ duyệt ---------- */
  pendingViewers: [
    { id: "v1", name: "Hoàng Văn Nam", requestedAt: "20:01:12" },
    { id: "v2", name: "Đỗ Thị Hồng",   requestedAt: "20:01:45" },
    { id: "v3", name: "Vũ Quốc Bảo",   requestedAt: "20:02:03" }
  ],

  /* ---------- Kho câu hỏi ----------
     Mỗi câu: id, round, text, answer, difficulty (de|trung-binh|kho),
     topics[], media (null | image | video | audio), creator, status */
  questions: {
    /* Khởi động — lượt riêng: 6 câu/thí sinh (mock 6 câu × 4) */
    khoiDongRieng: {
      c1: [
        { id: "kdr-01", text: "Thủ đô của Việt Nam là thành phố nào?", answer: "Hà Nội", difficulty: "de", topics: ["Địa lý"] },
        { id: "kdr-02", text: "2 mũ 10 bằng bao nhiêu?", answer: "1024", difficulty: "de", topics: ["Toán"] },
        { id: "kdr-03", text: "Tác giả 'Truyện Kiều' là ai?", answer: "Nguyễn Du", difficulty: "de", topics: ["Văn học"] },
        { id: "kdr-04", text: "Nguyên tố hoá học có ký hiệu Fe?", answer: "Sắt", difficulty: "de", topics: ["Hoá học"] },
        { id: "kdr-05", text: "Sông dài nhất Việt Nam?", answer: "Sông Đồng Nai", difficulty: "trung-binh", topics: ["Địa lý"] },
        { id: "kdr-06", text: "Từ tiếng Anh 'photosynthesis' nghĩa là gì?", answer: "Quang hợp", difficulty: "trung-binh", topics: ["Tiếng Anh", "Sinh học"] }
      ],
      c2: [
        { id: "kdr-07", text: "Quốc gia nào có diện tích lớn nhất thế giới?", answer: "Nga", difficulty: "de", topics: ["Địa lý"] },
        { id: "kdr-08", text: "Căn bậc hai của 144?", answer: "12", difficulty: "de", topics: ["Toán"] },
        { id: "kdr-09", text: "Chiến thắng Điện Biên Phủ diễn ra năm nào?", answer: "1954", difficulty: "de", topics: ["Lịch sử"] },
        { id: "kdr-10", text: "Đơn vị đo cường độ dòng điện?", answer: "Ampe", difficulty: "de", topics: ["Vật lý"] },
        { id: "kdr-11", text: "Loài chim nào không biết bay, sống ở Nam Cực?", answer: "Chim cánh cụt", difficulty: "de", topics: ["Sinh học"] },
        { id: "kdr-12", text: "Ai viết 'Bình Ngô đại cáo'?", answer: "Nguyễn Trãi", difficulty: "trung-binh", topics: ["Văn học", "Lịch sử"] }
      ],
      c3: [
        { id: "kdr-13", text: "Hành tinh nào gần Mặt Trời nhất?", answer: "Sao Thuỷ", difficulty: "de", topics: ["Thiên văn"] },
        { id: "kdr-14", text: "15% của 200 bằng bao nhiêu?", answer: "30", difficulty: "de", topics: ["Toán"] },
        { id: "kdr-15", text: "Vịnh Hạ Long thuộc tỉnh nào?", answer: "Quảng Ninh", difficulty: "de", topics: ["Địa lý"] },
        { id: "kdr-16", text: "Công thức hoá học của nước?", answer: "H2O", difficulty: "de", topics: ["Hoá học"] },
        { id: "kdr-17", text: "Bác Hồ đọc Tuyên ngôn Độc lập tại đâu?", answer: "Quảng trường Ba Đình", difficulty: "de", topics: ["Lịch sử"] },
        { id: "kdr-18", text: "Ngôn ngữ lập trình nào đặt tên theo loài rắn?", answer: "Python", difficulty: "trung-binh", topics: ["Tin học"] }
      ],
      c4: [
        { id: "kdr-19", text: "Một năm nhuận có bao nhiêu ngày?", answer: "366", difficulty: "de", topics: ["Kiến thức chung"] },
        { id: "kdr-20", text: "Kim loại nào ở thể lỏng ở nhiệt độ phòng?", answer: "Thuỷ ngân", difficulty: "de", topics: ["Hoá học"] },
        { id: "kdr-21", text: "Đỉnh núi cao nhất Việt Nam?", answer: "Fansipan", difficulty: "de", topics: ["Địa lý"] },
        { id: "kdr-22", text: "Tam giác có 3 cạnh bằng nhau gọi là gì?", answer: "Tam giác đều", difficulty: "de", topics: ["Toán"] },
        { id: "kdr-23", text: "Quốc hoa của Việt Nam?", answer: "Hoa sen", difficulty: "de", topics: ["Văn hoá"] },
        { id: "kdr-24", text: "Ai phát minh ra bóng đèn sợi đốt thương mại?", answer: "Thomas Edison", difficulty: "trung-binh", topics: ["Khoa học"] }
      ]
    },

    /* Khởi động — lượt chung: 12 câu bấm chuông (+10 / −5) */
    khoiDongChung: [
      { id: "kdc-01", text: "Nước nào đăng cai World Cup 2026 cùng Mỹ và Canada?", answer: "Mexico", difficulty: "de", topics: ["Thể thao"] },
      { id: "kdc-02", text: "Số nguyên tố nhỏ nhất lớn hơn 10?", answer: "11", difficulty: "de", topics: ["Toán"] },
      { id: "kdc-03", text: "Tác phẩm 'Số đỏ' của nhà văn nào?", answer: "Vũ Trọng Phụng", difficulty: "trung-binh", topics: ["Văn học"] },
      { id: "kdc-04", text: "ADN là viết tắt của cụm từ nào?", answer: "Axit deoxyribonucleic", difficulty: "trung-binh", topics: ["Sinh học"] },
      { id: "kdc-05", text: "Đồng bằng sông Cửu Long có bao nhiêu tỉnh thành (trước 2025)?", answer: "13", difficulty: "trung-binh", topics: ["Địa lý"] },
      { id: "kdc-06", text: "Ai là vị vua cuối cùng của triều Nguyễn?", answer: "Bảo Đại", difficulty: "de", topics: ["Lịch sử"] },
      { id: "kdc-07", text: "Ánh sáng đi từ Mặt Trời đến Trái Đất mất khoảng bao lâu?", answer: "8 phút 20 giây", difficulty: "trung-binh", topics: ["Vật lý"] },
      { id: "kdc-08", text: "'Break a leg' trong tiếng Anh nghĩa là gì?", answer: "Chúc may mắn", difficulty: "trung-binh", topics: ["Tiếng Anh"] },
      { id: "kdc-09", text: "Hợp âm Đô trưởng gồm 3 nốt nào?", answer: "Đô - Mi - Son", difficulty: "kho", topics: ["Âm nhạc"] },
      { id: "kdc-10", text: "Quốc gia nào phát minh ra giấy?", answer: "Trung Quốc", difficulty: "de", topics: ["Lịch sử"] },
      { id: "kdc-11", text: "1 hải lý bằng khoảng bao nhiêu km?", answer: "1,852 km", difficulty: "kho", topics: ["Kiến thức chung"] },
      { id: "kdc-12", text: "Trò chơi dân gian nào dùng 10 quân cờ và ô vuông vẽ trên đất?", answer: "Ô ăn quan", difficulty: "trung-binh", topics: ["Văn hoá"] }
    ],

    /* VCNV: 4 hàng ngang + ô trung tâm, CNV = "TRƯỜNG SA" */
    vcnv: {
      obstacle: { answer: "TRƯỜNG SA", hint: "Quần đảo thiêng liêng của Tổ quốc", image: "5 miếng ghép ảnh vệ tinh quần đảo" },
      rows: [
        { id: "vcnv-1", index: 1, len: 7, text: "Loại cây đặc trưng che bóng mát trên đảo, tên một loài... (7 chữ cái)", answer: "PHONG BA", difficulty: "trung-binh", topics: ["Địa lý"] },
        { id: "vcnv-2", index: 2, len: 6, text: "Lực lượng ngày đêm canh giữ biển đảo (6 chữ cái)", answer: "HAI QUAN", difficulty: "de", topics: ["Xã hội"] },
        { id: "vcnv-3", index: 3, len: 8, text: "Tỉnh quản lý hành chính quần đảo này (8 chữ cái)", answer: "KHANH HOA", difficulty: "trung-binh", topics: ["Địa lý"] },
        { id: "vcnv-4", index: 4, len: 5, text: "Nguồn tài nguyên quý dưới đáy biển Đông (5 chữ cái)", answer: "DAU KHI", difficulty: "trung-binh", topics: ["Kinh tế"] }
      ],
      centerPiece: { id: "vcnv-5", index: 5, text: "Ô trung tâm — hình ảnh gợi ý chính", answer: "CỘT MỐC CHỦ QUYỀN" }
    },

    /* Tăng tốc: 4 câu, độ khó tăng dần, thường có media */
    tangToc: [
      { id: "tt-01", text: "Có bao nhiêu hình tam giác trong hình vẽ? (ảnh)", answer: "12", time: 10, difficulty: "de", topics: ["IQ"], media: "image" },
      { id: "tt-02", text: "Sắp xếp các sự kiện lịch sử theo thứ tự thời gian (video)", answer: "B-A-D-C", time: 20, difficulty: "trung-binh", topics: ["Lịch sử"], media: "video" },
      { id: "tt-03", text: "Quy luật dãy số: 2, 6, 12, 20, 30, ... Số tiếp theo?", answer: "42", time: 30, difficulty: "trung-binh", topics: ["Toán", "IQ"] },
      { id: "tt-04", text: "Đoạn video thí nghiệm: hiện tượng vật lý nào được mô tả?", answer: "Khúc xạ ánh sáng", time: 40, difficulty: "kho", topics: ["Vật lý"], media: "video" }
    ],

    /* Về đích: pool câu 20 điểm và 30 điểm */
    veDich: {
      20: [
        { id: "vd20-01", text: "Nêu tên 3 quốc gia thuộc bán đảo Đông Dương.", answer: "Việt Nam, Lào, Campuchia", difficulty: "de", topics: ["Địa lý"] },
        { id: "vd20-02", text: "Định luật bảo toàn khối lượng do ai phát biểu?", answer: "Lavoisier (và Lomonosov)", difficulty: "trung-binh", topics: ["Hoá học"] },
        { id: "vd20-03", text: "Bài thơ 'Tây Tiến' của tác giả nào?", answer: "Quang Dũng", difficulty: "de", topics: ["Văn học"] },
        { id: "vd20-04", text: "Thuật toán tìm kiếm nhị phân có độ phức tạp bao nhiêu?", answer: "O(log n)", difficulty: "trung-binh", topics: ["Tin học"] },
        { id: "vd20-05", text: "Hiệp định Genève được ký năm nào?", answer: "1954", difficulty: "de", topics: ["Lịch sử"] },
        { id: "vd20-06", text: "Cơ quan nào trong tế bào được ví là 'nhà máy năng lượng'?", answer: "Ty thể", difficulty: "de", topics: ["Sinh học"] }
      ],
      30: [
        { id: "vd30-01", text: "Giải thích ngắn gọn hiệu ứng nhà kính và 2 khí gây ra chủ yếu.", answer: "CO2 và CH4 giữ nhiệt bức xạ hồng ngoại trong khí quyển", difficulty: "kho", topics: ["Môi trường"] },
        { id: "vd30-02", text: "Chứng minh tổng 3 góc trong tam giác bằng 180°, nêu ý tưởng chính.", answer: "Kẻ đường thẳng song song với một cạnh qua đỉnh đối diện", difficulty: "kho", topics: ["Toán"] },
        { id: "vd30-03", text: "Phân tích ý nghĩa nhan đề 'Chiếc thuyền ngoài xa' của Nguyễn Minh Châu.", answer: "Khoảng cách giữa nghệ thuật và hiện thực đời sống", difficulty: "kho", topics: ["Văn học"] },
        { id: "vd30-04", text: "Trình bày nguyên lý hoạt động của pin mặt trời.", answer: "Hiệu ứng quang điện trong tiếp giáp bán dẫn p-n", difficulty: "kho", topics: ["Vật lý"] },
        { id: "vd30-05", text: "So sánh chiến lược 'chiến tranh đặc biệt' và 'chiến tranh cục bộ' của Mỹ.", answer: "Đặc biệt: quân nguỵ + cố vấn; Cục bộ: quân viễn chinh Mỹ trực tiếp", difficulty: "kho", topics: ["Lịch sử"] },
        { id: "vd30-06", text: "Vì sao nước biển có màu xanh?", answer: "Nước hấp thụ ánh sáng đỏ, tán xạ ánh sáng xanh", difficulty: "kho", topics: ["Vật lý"] }
      ]
    },

    /* Câu hỏi phụ (tie-break): tối đa 3 câu, 15s, bấm chuông */
    tieBreak: [
      { id: "tb-01", text: "Ai là chủ tịch Hồ Chí Minh đọc bản Tuyên ngôn Độc lập năm nào?", answer: "1945", difficulty: "de", topics: ["Lịch sử"] },
      { id: "tb-02", text: "Số pi làm tròn 2 chữ số thập phân?", answer: "3,14", difficulty: "de", topics: ["Toán"] },
      { id: "tb-03", text: "Đại dương lớn nhất thế giới?", answer: "Thái Bình Dương", difficulty: "de", topics: ["Địa lý"] }
    ]
  },

  /* ---------- Danh sách phẳng cho trang quản lý kho đề ---------- */
  questionBank: [] // được build bên dưới
};

/* Build danh sách phẳng + metadata quản trị (người tạo, trạng thái duyệt) */
(function buildBank() {
  var q = window.MOCK.questions;
  var creators = ["Cô Lan (GV Sử)", "Thầy Tuấn (GV Lý)", "Ban ra đề", "Thầy Minh (GV Toán)"];
  var statuses = ["approved", "approved", "approved", "pending", "approved", "rejected"];
  var all = [];
  var i = 0;
  function push(item, round) {
    all.push(Object.assign({}, item, {
      round: round,
      media: item.media || null,
      creator: creators[i % creators.length],
      status: statuses[i % statuses.length]
    }));
    i++;
  }
  Object.keys(q.khoiDongRieng).forEach(function (cid) {
    q.khoiDongRieng[cid].forEach(function (it) { push(it, "KHOI_DONG_RIENG"); });
  });
  q.khoiDongChung.forEach(function (it) { push(it, "KHOI_DONG_CHUNG"); });
  q.vcnv.rows.forEach(function (it) { push(it, "VCNV"); });
  q.tangToc.forEach(function (it) { push(it, "TANG_TOC"); });
  q.veDich[20].forEach(function (it) { push(it, "VE_DICH_20"); });
  q.veDich[30].forEach(function (it) { push(it, "VE_DICH_30"); });
  q.tieBreak.forEach(function (it) { push(it, "TIE_BREAK"); });
  window.MOCK.questionBank = all;
})();
