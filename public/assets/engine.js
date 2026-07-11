/* ============================================================
   engine.js — Mock game engine (window.Engine)
   - State machine các vòng thi theo rules-2026.md
   - Timer đếm ngược, tính điểm đúng luật, pub/sub cho UI
   - KHÔNG network: mọi thứ chạy local, phục vụ demo animation
   ============================================================ */
window.Engine = (function () {
  "use strict";

  /* ---------- RULES: default theo luật 2026 (admin config được) ---------- */
  var RULES = {
    KHOI_DONG: {
      riengSoCau: 6, riengTime: 5, riengDung: 10, riengSai: 0,
      chungSoCau: 12, chungTime: 5, chungDung: 10, chungSai: -5
    },
    VCNV: {
      soHangNgang: 4, rowTime: 15, rowDung: 10,
      /* điểm CNV theo số hàng ngang ĐÃ MỞ khi bấm: 0-1 → 80, 2 → 60, 3 → 40, 4+ → 20 */
      cnvPoints: [80, 80, 60, 40, 20]
    },
    TANG_TOC: {
      soCau: 4, times: [10, 20, 30, 40],
      /* điểm theo thứ hạng tốc độ trong số người ĐÚNG */
      points: [40, 30, 20, 10]
    },
    VE_DICH: {
      soCau: 3, goi: [20, 30],
      timeMap: { 20: 15, 30: 20 },
      cuopTime: 5
      /* NSHV: đúng ×2 giá trị, sai −giá trị. Cướp: đúng +giá trị, sai −½ giá trị */
    },
    TIE_BREAK: { time: 15, maxCau: 3 }
  };

  /* ---------- Pub/sub đơn giản ---------- */
  var listeners = {};
  function on(evt, cb) {
    (listeners[evt] = listeners[evt] || []).push(cb);
    return function () { off(evt, cb); };
  }
  function off(evt, cb) {
    var arr = listeners[evt];
    if (arr) listeners[evt] = arr.filter(function (f) { return f !== cb; });
  }
  function emit(evt, data) {
    (listeners[evt] || []).slice().forEach(function (cb) { cb(data); });
    (listeners["*"] || []).slice().forEach(function (cb) { cb(evt, data); });
  }

  /* ---------- State ---------- */
  var state = {
    phase: "CHO",              // CHO | KHOI_DONG_RIENG | KHOI_DONG_CHUNG | VCNV | TANG_TOC | VE_DICH | TIE_BREAK | KET_THUC
    scores: {},                // { cid: điểm }
    buzzWinner: null,          // cid đang giữ chuông
    buzzLocked: false,         // khoá chuông toàn cục
    lockedOut: {},             // { cid: true } — bị loại (VCNV sai CNV / khoá cá nhân)
    vcnvOpened: 0,             // số hàng ngang đã mở
    starUsed: {},              // { cid: true } — đã dùng Ngôi sao hy vọng
    log: []                    // audit log mock
  };
  (window.MOCK ? window.MOCK.contestants : []).forEach(function (c) {
    state.scores[c.id] = 0;
  });

  function log(msg) {
    var t = new Date().toTimeString().slice(0, 8);
    var entry = { time: t, msg: msg };
    state.log.push(entry);
    emit("log", entry);
  }

  /* ---------- Timer đếm ngược ---------- */
  var timer = { total: 0, remaining: 0, handle: null, running: false };
  function startTimer(seconds) {
    stopTimer();
    timer.total = seconds; timer.remaining = seconds; timer.running = true;
    emit("timer:start", { total: seconds });
    var last = performance.now();
    timer.handle = setInterval(function () {
      var now = performance.now();
      timer.remaining = Math.max(0, timer.remaining - (now - last) / 1000);
      last = now;
      emit("timer:tick", { remaining: timer.remaining, total: timer.total });
      if (timer.remaining <= 0) { stopTimer(); emit("timer:end", {}); }
    }, 100);
  }
  function pauseTimer() {
    if (timer.handle) { clearInterval(timer.handle); timer.handle = null; }
    timer.running = false;
    emit("timer:pause", { remaining: timer.remaining });
  }
  function resumeTimer() {
    if (timer.running || timer.remaining <= 0) return;
    timer.running = true;
    var last = performance.now();
    timer.handle = setInterval(function () {
      var now = performance.now();
      timer.remaining = Math.max(0, timer.remaining - (now - last) / 1000);
      last = now;
      emit("timer:tick", { remaining: timer.remaining, total: timer.total });
      if (timer.remaining <= 0) { stopTimer(); emit("timer:end", {}); }
    }, 100);
  }
  function stopTimer() {
    if (timer.handle) { clearInterval(timer.handle); timer.handle = null; }
    timer.running = false;
  }
  function setTimer(seconds) { // admin chỉnh tay
    timer.total = Math.max(timer.total, seconds); timer.remaining = seconds;
    emit("timer:tick", { remaining: timer.remaining, total: timer.total });
  }

  /* ---------- Điểm ---------- */
  function addScore(cid, delta, reason) {
    state.scores[cid] = (state.scores[cid] || 0) + delta;
    emit("score:change", { cid: cid, delta: delta, score: state.scores[cid], reason: reason });
    var c = getContestant(cid);
    log((c ? c.name : cid) + (delta >= 0 ? " +" : " ") + delta + " điểm (" + reason + ")");
  }
  function setScore(cid, value, reason) { // admin sửa tay
    var delta = value - (state.scores[cid] || 0);
    addScore(cid, delta, reason || "admin chỉnh tay");
  }
  function getContestant(cid) {
    return (window.MOCK.contestants || []).find(function (c) { return c.id === cid; });
  }
  function ranking() { // xếp hạng giảm dần theo điểm
    return window.MOCK.contestants.slice().sort(function (a, b) {
      return state.scores[b.id] - state.scores[a.id];
    });
  }

  /* ---------- Vòng thi / phase ---------- */
  function setPhase(phase, meta) {
    state.phase = phase;
    state.buzzWinner = null;
    emit("phase:change", { phase: phase, meta: meta || {} });
    log("Chuyển vòng: " + phase);
  }

  /* ---------- Chuông ---------- */
  function buzz(cid) {
    if (state.buzzLocked || state.buzzWinner || state.lockedOut[cid]) {
      emit("buzz:rejected", { cid: cid });
      return false;
    }
    state.buzzWinner = cid;
    emit("buzz:won", { cid: cid, name: (getContestant(cid) || {}).name });
    log((getContestant(cid) || { name: cid }).name + " bấm chuông giành quyền");
    return true;
  }
  function clearBuzz() { state.buzzWinner = null; emit("buzz:clear", {}); }
  function lockBuzz(v) { state.buzzLocked = v; emit("buzz:lock", { locked: v }); }
  function lockContestant(cid, v) {
    state.lockedOut[cid] = v;
    emit("contestant:lock", { cid: cid, locked: v });
  }

  /* ============================================================
     TÍNH ĐIỂM THEO LUẬT — mỗi hàm trả về delta & tự cộng điểm
     ============================================================ */

  /* Khởi động riêng: đúng +10, sai 0 */
  function scoreKhoiDongRieng(cid, correct) {
    var d = correct ? RULES.KHOI_DONG.riengDung : RULES.KHOI_DONG.riengSai;
    if (d) addScore(cid, d, "Khởi động riêng");
    emit("answer:judged", { cid: cid, correct: correct, round: "KHOI_DONG_RIENG", delta: d });
    return d;
  }

  /* Khởi động chung (chuông): đúng +10, sai −5; câu đó KHÔNG mở lại chuông */
  function scoreKhoiDongChung(cid, correct) {
    var d = correct ? RULES.KHOI_DONG.chungDung : RULES.KHOI_DONG.chungSai;
    addScore(cid, d, "Khởi động chung");
    emit("answer:judged", { cid: cid, correct: correct, round: "KHOI_DONG_CHUNG", delta: d });
    clearBuzz();
    return d;
  }

  /* VCNV hàng ngang: mỗi thí sinh trả lời đúng +10; mở miếng ghép nếu có ai đúng */
  function scoreVcnvRow(results /* {cid: true|false} */) {
    var anyCorrect = false;
    Object.keys(results).forEach(function (cid) {
      if (state.lockedOut[cid]) return; // đã bị loại → không tính
      if (results[cid]) { addScore(cid, RULES.VCNV.rowDung, "VCNV hàng ngang"); anyCorrect = true; }
    });
    if (anyCorrect) {
      state.vcnvOpened++;
      emit("vcnv:open", { opened: state.vcnvOpened });
    }
    return anyCorrect;
  }

  /* Bấm chuông giải CNV: đúng → điểm theo số hàng đã mở; sai → bị loại khỏi VCNV */
  function scoreVcnvObstacle(cid, correct) {
    if (correct) {
      var idx = Math.min(state.vcnvOpened, RULES.VCNV.cnvPoints.length - 1);
      var pts = RULES.VCNV.cnvPoints[idx];
      addScore(cid, pts, "Giải CNV (đã mở " + state.vcnvOpened + " hàng)");
      emit("vcnv:solved", { cid: cid, points: pts });
      return pts;
    }
    lockContestant(cid, true);
    emit("vcnv:eliminated", { cid: cid });
    log((getContestant(cid) || { name: cid }).name + " trả lời sai CNV — bị loại khỏi VCNV");
    return 0;
  }

  /* Tăng tốc: nhận mảng đáp án {cid, correct, ms}; đúng xếp theo tốc độ 40/30/20/10 */
  function scoreTangToc(answers) {
    var correct = answers.filter(function (a) { return a.correct; })
                         .sort(function (a, b) { return a.ms - b.ms; });
    var out = [];
    correct.forEach(function (a, rank) {
      var pts = RULES.TANG_TOC.points[rank] || 0;
      if (pts) addScore(a.cid, pts, "Tăng tốc hạng " + (rank + 1));
      out.push({ cid: a.cid, rank: rank + 1, points: pts, ms: a.ms });
    });
    emit("tangtoc:result", { ranked: out, all: answers });
    return out;
  }

  /* Về đích — thí sinh chính trả lời câu giá trị `value` (20|30), có thể kèm NSHV */
  function scoreVeDich(cid, value, correct, useStar) {
    var d;
    if (useStar) {
      state.starUsed[cid] = true;
      d = correct ? value * 2 : -value; // NSHV: đúng ×2, sai −giá trị
      emit("star:used", { cid: cid, correct: correct });
    } else {
      d = correct ? value : 0;          // thường: đúng +giá trị, sai 0 (mở quyền cướp)
    }
    if (d) addScore(cid, d, "Về đích" + (useStar ? " (NSHV)" : ""));
    emit("answer:judged", { cid: cid, correct: correct, round: "VE_DICH", delta: d, star: !!useStar });
    if (!correct && !useStar) emit("vedich:steal-open", { value: value });
    if (!correct && useStar) emit("vedich:steal-open", { value: value }); // sai vẫn mở cướp
    return d;
  }

  /* Về đích — cướp quyền: đúng +giá trị, sai −½ giá trị */
  function scoreCuop(cid, value, correct) {
    var d = correct ? value : -value / 2;
    addScore(cid, d, "Cướp quyền về đích");
    emit("vedich:steal-result", { cid: cid, correct: correct, delta: d });
    clearBuzz();
    return d;
  }

  /* ---------- Public API ---------- */
  return {
    RULES: RULES,
    state: state,
    on: on, off: off, emit: emit,
    startTimer: startTimer, pauseTimer: pauseTimer, resumeTimer: resumeTimer,
    stopTimer: stopTimer, setTimer: setTimer,
    getTimer: function () { return timer; },
    setPhase: setPhase,
    buzz: buzz, clearBuzz: clearBuzz, lockBuzz: lockBuzz, lockContestant: lockContestant,
    addScore: addScore, setScore: setScore, ranking: ranking, getContestant: getContestant,
    scoreKhoiDongRieng: scoreKhoiDongRieng,
    scoreKhoiDongChung: scoreKhoiDongChung,
    scoreVcnvRow: scoreVcnvRow,
    scoreVcnvObstacle: scoreVcnvObstacle,
    scoreTangToc: scoreTangToc,
    scoreVeDich: scoreVeDich,
    scoreCuop: scoreCuop,
    log: log
  };
})();
