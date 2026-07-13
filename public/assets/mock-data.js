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

  /* ---------- 12 thí sinh (demo adaptive layout 4/8/12) ----------
     4 người đầu = contestants ở trên; slice(0, n) để lấy số ghế active */
  contestantsAll: [
    { id: "c1",  name: "Nguyễn Minh Khang", school: "THPT Chuyên Lê Hồng Phong", color: "#4d8dff", score: 0 },
    { id: "c2",  name: "Trần Thuỳ Dương",   school: "THPT Chuyên Hà Nội – Amsterdam", color: "#2ed573", score: 0 },
    { id: "c3",  name: "Lê Hoàng Phúc",     school: "THPT Chuyên Quốc Học Huế", color: "#ff9f43", score: 0 },
    { id: "c4",  name: "Phạm Ngọc Ánh",     school: "THPT Chuyên Lam Sơn", color: "#8c6cff", score: 0 },
    { id: "c5",  name: "Đặng Quốc Huy",     school: "THPT Chuyên Trần Đại Nghĩa", color: "#4d8dff", score: 0 },
    { id: "c6",  name: "Bùi Khánh Linh",    school: "THPT Chuyên Phan Bội Châu", color: "#2ed573", score: 0 },
    { id: "c7",  name: "Võ Thanh Tùng",     school: "THPT Chuyên Lê Quý Đôn", color: "#ff9f43", score: 0 },
    { id: "c8",  name: "Hồ Mai Phương",     school: "THPT Chuyên Nguyễn Trãi", color: "#8c6cff", score: 0 },
    { id: "c9",  name: "Trịnh Đức Anh",     school: "THPT Chuyên Bắc Ninh", color: "#4d8dff", score: 0 },
    { id: "c10", name: "Lý Thu Hà",         school: "THPT Chuyên Hùng Vương", color: "#2ed573", score: 0 },
    { id: "c11", name: "Phan Gia Bảo",      school: "THPT Chuyên Thái Bình", color: "#ff9f43", score: 0 },
    { id: "c12", name: "Ngô Diệu Anh",      school: "THPT Chuyên Vĩnh Phúc", color: "#8c6cff", score: 0 }
  ],

  /* ---------- Đội (chế độ team: 12 người → 4 đội × 3) ----------
     scoringUnit=team: điểm cá nhân reduce về đội (Engine.teamScores) */
  teams: [
    { id: "t1", name: "Đội Sao Mai",    color: "#4d8dff", seatIds: ["c1", "c5", "c9"]  },
    { id: "t2", name: "Đội Bình Minh",  color: "#2ed573", seatIds: ["c2", "c6", "c10"] },
    { id: "t3", name: "Đội Rạng Đông",  color: "#ff9f43", seatIds: ["c3", "c7", "c11"] },
    { id: "t4", name: "Đội Hừng Sáng",  color: "#8c6cff", seatIds: ["c4", "c8", "c12"] }
  ],

  /* ---------- Round playlist (spec v2: KHÔNG cứng 5 bước) ----------
     Admin stepper render từ mảng này — thêm/bớt/lặp vòng tuỳ ý */
  playlist: [
    { id: "r1", type: "KHOI_DONG", label: "Khởi động",   note: "riêng 6 câu + chung 12 câu" },
    { id: "r2", type: "VCNV",      label: "Vượt CNV",    note: "4-8 hàng + CNV 60/50/40/30 (trung tâm 20)" },
    { id: "r3", type: "TANG_TOC",  label: "Tăng tốc",    note: "ranked-speed / clue-buzz" },
    { id: "r4", type: "VE_DICH",   label: "Về đích",     note: "gói 20/30 · NSHV · cướp" },
    { id: "r5", type: "TIE_BREAK", label: "Câu hỏi phụ", note: "hoà điểm · tối đa 3 câu" }
  ],

  /* ---------- Soundboard (mock — không phát audio thật) ---------- */
  sound: {
    backgroundTracks: [
      { id: "bg1", name: "Nhạc nền chờ (lobby loop)",     duration: 154 },
      { id: "bg2", name: "Nhạc hình hiệu vòng thi",       duration: 92  },
      { id: "bg3", name: "Nhạc suy nghĩ (countdown bed)", duration: 121 }
    ]
  },

  /* ---------- Taxonomy lĩnh vực (Field — spec v2 phase-03) ---------- */
  fields: [
    { id: "tu-nhien",   name: "Tự nhiên",  color: "blue"   },
    { id: "xa-hoi",     name: "Xã hội",    color: "purple" },
    { id: "van-hoa",    name: "Văn hoá – Nghệ thuật", color: "gold" },
    { id: "the-thao",   name: "Thể thao",  color: "green"  },
    { id: "tieng-anh",  name: "Tiếng Anh", color: "orange" },
    { id: "hieu-biet",  name: "Hiểu biết chung", color: "red" }
  ],

  /* ---------- Bộ đề (QuestionSet — demo PRIVATE/PUBLIC + share-link) ---------- */
  sets: [
    { id: "s1", displayId: "SET-000012", name: "Bộ đề luyện tập tuần 42", count: 24,
      visibility: "PRIVATE", everPublic: false, shareLink: null, owner: "Ban ra đề" },
    { id: "s2", displayId: "SET-000015", name: "Đề thi thử Olympia cụm Hà Nội", count: 48,
      visibility: "PRIVATE", everPublic: false, owner: "Cô Lan (GV Sử)",
      shareLink: { token: "8f3kQz…Xp29", password: "olp-2026", ttl: "7 ngày", withAnswer: false } },
    { id: "s3", displayId: "SET-000009", name: "Bộ đề mẫu công khai 2025", count: 36,
      visibility: "PUBLIC", everPublic: true, shareLink: null, owner: "Ban ra đề" }
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

    /* VCNV: 4 hàng ngang + ô trung tâm, CNV = "TRƯỜNG SA"
       hintPos: vị trí ký tự trong đáp án hàng ngang THUỘC CNV (hintMap spec v2 §5) */
    vcnv: {
      obstacle: { answer: "TRƯỜNG SA", hint: "Quần đảo thiêng liêng của Tổ quốc", image: "5 miếng ghép ảnh vệ tinh quần đảo" },
      rows: [
        { id: "vcnv-1", index: 1, len: 7, text: "Loại cây đặc trưng che bóng mát trên đảo, tên một loài... (7 chữ cái)", answer: "PHONG BA", hintPos: [3, 4, 7], difficulty: "trung-binh", topics: ["Địa lý"] },
        { id: "vcnv-2", index: 2, len: 6, text: "Lực lượng ngày đêm canh giữ biển đảo (6 chữ cái)", answer: "HAI QUAN", hintPos: [1, 5, 7], difficulty: "de", topics: ["Xã hội"] },
        { id: "vcnv-3", index: 3, len: 8, text: "Tỉnh quản lý hành chính quần đảo này (8 chữ cái)", answer: "KHANH HOA", hintPos: [2, 3, 7, 8], difficulty: "trung-binh", topics: ["Địa lý"] },
        { id: "vcnv-4", index: 4, len: 5, text: "Nguồn tài nguyên quý dưới đáy biển Đông (5 chữ cái)", answer: "DAU KHI", hintPos: [1, 2], difficulty: "trung-binh", topics: ["Kinh tế"] }
      ],
      centerPiece: { id: "vcnv-5", index: 5, text: "Ô trung tâm — hình ảnh gợi ý chính", answer: "CỘT MỐC CHỦ QUYỀN" }
    },

    /* VCNV biến thể 8 hàng ngang (ObstacleSet.rowCount = 8, spec v2 §5) */
    vcnv8: {
      obstacle: { answer: "SÔNG HỒNG", hint: "Dòng sông gắn với nền văn minh lúa nước Bắc Bộ", image: "9 miếng ghép (8 + trung tâm)" },
      rows: [
        { id: "v8-1", index: 1, text: "Chất màu mỡ sông bồi đắp cho ruộng đồng", answer: "PHU SA",     hintPos: [1, 4] },
        { id: "v8-2", index: 2, text: "Địa hình do sông tạo ra ở hạ lưu",         answer: "DONG BANG",  hintPos: [1, 2, 3, 8] },
        { id: "v8-3", index: 3, text: "Tên cũ của Hà Nội thời Lý",                answer: "THANG LONG", hintPos: [4, 6, 7, 8, 9] },
        { id: "v8-4", index: 4, text: "Thành phố ngã ba sông ở Phú Thọ",          answer: "VIET TRI",   hintPos: [2] },
        { id: "v8-5", index: 5, text: "Công trình ngăn lũ dọc hai bờ sông",       answer: "DE DIEU",    hintPos: [] },
        { id: "v8-6", index: 6, text: "Vùng đất bồi hình tam giác nơi cửa sông",  answer: "CHAU THO",   hintPos: [2, 7] },
        { id: "v8-7", index: 7, text: "Hiện tượng nước dâng cao mùa mưa",         answer: "NUOC LU",    hintPos: [1, 2] },
        { id: "v8-8", index: 8, text: "Cây cầu thép lịch sử bắc qua sông này",    answer: "LONG BIEN",  hintPos: [1, 2, 3] }
      ],
      centerPiece: { id: "v8-9", index: 9, text: "Ô trung tâm — hình ảnh gợi ý chính", answer: "BÃI GIỮA" }
    },

    /* Tăng tốc: 4 câu, độ khó tăng dần, thường có media */
    tangToc: [
      { id: "tt-01", text: "Có bao nhiêu hình tam giác trong hình vẽ? (ảnh)", answer: "12", time: 20, difficulty: "de", topics: ["IQ"], media: "image" },
      { id: "tt-02", text: "Sắp xếp các sự kiện lịch sử theo thứ tự thời gian (video)", answer: "B-A-D-C", time: 20, difficulty: "trung-binh", topics: ["Lịch sử"], media: "video" },
      { id: "tt-03", text: "Quy luật dãy số: 2, 6, 12, 20, 30, ... Số tiếp theo?", answer: "42", time: 30, difficulty: "trung-binh", topics: ["Toán", "IQ"] },
      { id: "tt-04", text: "Đoạn video thí nghiệm: hiện tượng vật lý nào được mô tả?", answer: "Khúc xạ ánh sáng", time: 30, difficulty: "kho", topics: ["Vật lý"], media: "video" }
    ],

    /* Tăng tốc — format clue-buzz (spec v2 §6): 3-4 dữ kiện mở dần,
       bấm chuông bất kỳ lúc nào; điểm theo MỐC dữ kiện đang mở 40/30/20/10 */
    tangTocClue: [
      { id: "ttc-01", answer: "TRÁI ĐẤT", cluePoints: [40, 30, 20, 10], clueInterval: 10, difficulty: "trung-binh", topics: ["Thiên văn"],
        clues: [
          "Hành tinh thứ ba tính từ Mặt Trời.",
          "Khoảng 71% bề mặt được bao phủ bởi nước.",
          "Có đúng một vệ tinh tự nhiên.",
          "Nơi duy nhất được biết là có sự sống."
        ] },
      { id: "ttc-02", answer: "NGUYỄN DU", cluePoints: [40, 30, 20, 10], clueInterval: 10, difficulty: "trung-binh", topics: ["Văn học"],
        clues: [
          "Đại thi hào dân tộc, danh nhân văn hoá thế giới.",
          "Quê Hà Tĩnh, sống cuối thế kỷ XVIII – đầu XIX.",
          "Tác giả 'Văn tế thập loại chúng sinh'.",
          "Nổi tiếng nhất với 'Truyện Kiều'."
        ] }
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

/* Build danh sách phẳng + metadata quản trị (người tạo, trạng thái duyệt)
   ✅ D24 12/07: câu hỏi trong kho KHÔNG chia theo vòng thi — taxonomy 3 POOL:
   KV (chung Khởi động + Về đích + câu phụ — dùng được ở Về đích khi value ∈ valueChoices),
   TT (tăng tốc), CN (bộ đề VCNV, hàng ngang -Rn).
   Mức điểm (value) + thời gian (timeSeconds) là METADATA riêng TỪNG CÂU, không phải loại câu
   (cùng 20đ có thể câu 15s và câu 40s — điểm độc lập thời gian).
   displayId = <POOL 2 ký tự>-<4 HEX đầu UUID>-<4 HEX cuối UUID> uppercase —
   demo derive hex giả lập ổn định từ item id; fieldId, wordCount (auto) */
(function buildBank() {
  var q = window.MOCK.questions;
  var creators = ["Cô Lan (GV Sử)", "Thầy Tuấn (GV Lý)", "Ban ra đề", "Thầy Minh (GV Toán)"];
  var statuses = ["approved", "approved", "approved", "pending", "approved", "rejected"];
  /* Map chủ đề → lĩnh vực (taxonomy Field) */
  var TOPIC_FIELD = {
    "Toán": "tu-nhien", "Vật lý": "tu-nhien", "Hoá học": "tu-nhien",
    "Sinh học": "tu-nhien", "Thiên văn": "tu-nhien", "Tin học": "tu-nhien",
    "Khoa học": "tu-nhien", "Môi trường": "tu-nhien",
    "Địa lý": "xa-hoi", "Lịch sử": "xa-hoi", "Xã hội": "xa-hoi", "Kinh tế": "xa-hoi",
    "Văn học": "van-hoa", "Văn hoá": "van-hoa", "Âm nhạc": "van-hoa",
    "Thể thao": "the-thao", "Tiếng Anh": "tieng-anh",
    "Kiến thức chung": "hieu-biet", "IQ": "hieu-biet"
  };
  var all = [];
  var i = 0;
  /* 4 hex uppercase giả lập UUID, deterministic từ string (demo — app thật lấy từ UUID) */
  function h4(s, salt) {
    var h = 5381 + (salt || 0);
    for (var k = 0; k < s.length; k++) h = ((h << 5) + h + s.charCodeAt(k)) >>> 0;
    return ("0000" + (h % 65536).toString(16).toUpperCase()).slice(-4);
  }
  function makeId(pool, key) { return pool + "-" + h4(key, 7) + "-" + h4(key, 131); }
  function push(item, pool, extra) {
    extra = extra || {};
    var topic = (item.topics || [])[0];
    all.push(Object.assign({}, item, {
      pool: pool,
      value: extra.value != null ? extra.value : null,           // mức điểm — metadata riêng (điều kiện dùng ở Về đích)
      timeSeconds: extra.timeSeconds != null ? extra.timeSeconds : null, // thời gian TỪNG câu — độc lập với value
      media: item.media || null,
      creator: creators[i % creators.length],
      status: statuses[i % statuses.length],
      displayId: extra.idOverride || makeId(pool, item.id || String(i)),
      fieldId: TOPIC_FIELD[topic] || "hieu-biet",
      wordCount: item.text.trim().split(/\s+/).length
    }));
    i++;
  }
  /* Khởi động (cả 2 lượt của trận rút chung 1 pool KV, không gán value) */
  Object.keys(q.khoiDongRieng).forEach(function (cid) {
    q.khoiDongRieng[cid].forEach(function (it) { push(it, "KV"); });
  });
  q.khoiDongChung.forEach(function (it) { push(it, "KV"); });
  /* Bộ đề VCNV: 1 Id cấp SET, hàng ngang là item con <setId>-Rn */
  var cnSetId = makeId("CN", "vcnv-set-demo");
  q.vcnv.rows.forEach(function (it, r) { push(it, "CN", { idOverride: cnSetId + "-R" + (r + 1) }); });
  q.tangToc.forEach(function (it) { push(it, "TT", { timeSeconds: 30 }); });
  q.tangTocClue.forEach(function (it) {
    push(Object.assign({ text: "[Clue-buzz] " + it.clues[0] + " (+" + (it.clues.length - 1) + " dữ kiện)" }, it), "TT", { timeSeconds: 40 });
  });
  /* Câu KV có value = đủ điều kiện Về đích; cùng mức điểm nhưng timeSeconds khác nhau (điểm độc lập thời gian) */
  q.veDich[20].forEach(function (it, k) { push(it, "KV", { value: 20, timeSeconds: k % 3 === 2 ? 40 : 15 }); });
  q.veDich[30].forEach(function (it, k) { push(it, "KV", { value: 30, timeSeconds: k % 3 === 2 ? 60 : 20 }); });
  /* Câu phụ (tie-break) rút từ pool KV — không phải loại riêng (D24) */
  q.tieBreak.forEach(function (it) { push(it, "KV"); });
  window.MOCK.questionBank = all;
})();
