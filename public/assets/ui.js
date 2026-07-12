/* ============================================================
   ui.js — Component UX dùng chung (window.UI)
   - Toast: pattern loading → success/error sau MỖI action
   - mockAsync: giả lập network 300-600ms, khoá nút chống double-submit
   - Điều hướng: Esc = back (đóng modal trước nếu có), H = về hub
   ============================================================ */
window.UI = (function () {
  "use strict";

  /* ---------- Toast stack ---------- */
  var stack = null;
  function ensureStack() {
    if (stack) return stack;
    stack = document.createElement("div");
    stack.className = "toast-stack";
    stack.setAttribute("aria-live", "polite");
    document.body.appendChild(stack);
    return stack;
  }
  var ICONS = { loading: null, success: "✓", error: "✕", info: "ℹ" };

  /* toast(msg, type) → handle { update(msg, type), close() }
     type: "loading" (spinner, không tự tắt) | "success" | "error" | "info" */
  function toast(msg, type, opts) {
    type = type || "info"; opts = opts || {};
    var el = document.createElement("div");
    el.className = "toast toast--" + type;
    el.setAttribute("role", type === "error" ? "alert" : "status");
    render(el, msg, type);
    ensureStack().appendChild(el);
    var closeTimer = null;
    function render(node, m, t) {
      node.innerHTML = "";
      if (t === "loading") {
        var sp = document.createElement("span");
        sp.className = "spinner"; node.appendChild(sp);
      } else {
        var ic = document.createElement("span");
        ic.className = "toast__icon"; ic.textContent = ICONS[t] || "ℹ";
        node.appendChild(ic);
      }
      var tx = document.createElement("span");
      tx.textContent = m; node.appendChild(tx);
    }
    function close() {
      clearTimeout(closeTimer);
      el.classList.add("is-leaving");
      setTimeout(function () { el.remove(); }, 200);
    }
    function scheduleClose() {
      clearTimeout(closeTimer);
      closeTimer = setTimeout(close, opts.duration || 2400);
    }
    if (type !== "loading") scheduleClose();
    return {
      el: el,
      update: function (m, t) { // loading → success/error
        el.className = "toast toast--" + t;
        render(el, m, t);
        if (t !== "loading") scheduleClose();
      },
      close: close
    };
  }

  /* ---------- mockAsync: giả lập request, khoá nút, toast lifecycle ----------
     UI.mockAsync({ btn, loading, success, error, fail, delay }).then(ok => …) */
  function mockAsync(opts) {
    opts = opts || {};
    var btn = opts.btn, spin = null;
    if (btn) { // Prevent Double Submission: disable đến khi có phản hồi
      btn.disabled = true;
      btn.classList.add("is-loading");
      spin = document.createElement("span");
      spin.className = "spinner";
      btn.insertBefore(spin, btn.firstChild);
    }
    var t = opts.loading ? toast(opts.loading, "loading") : null;
    var delay = opts.delay || (300 + Math.random() * 300); // mock 300-600ms
    return new Promise(function (resolve) {
      setTimeout(function () {
        if (btn) {
          btn.disabled = false;
          btn.classList.remove("is-loading");
          if (spin) spin.remove();
        }
        if (opts.fail) {
          t ? t.update(opts.error || "Có lỗi xảy ra", "error")
            : toast(opts.error || "Có lỗi xảy ra", "error");
        } else if (opts.success) {
          t ? t.update(opts.success, "success") : toast(opts.success, "success");
        } else if (t) t.close();
        resolve(!opts.fail);
      }, delay);
    });
  }

  /* ---------- Query param ---------- */
  function param(name) {
    return new URLSearchParams(location.search).get(name);
  }

  /* ---------- Back: về trang trước / hub ---------- */
  function goBack() {
    if (document.referrer && history.length > 1) history.back();
    else location.href = "index.html";
  }

  /* ---------- Hotkey toàn cục: Esc = back, H = hub ----------
     - Esc khi có modal mở → đóng modal trước
     - Bỏ qua khi đang gõ trong input/textarea/select
     - body[data-no-back] (hub) → không back */
  document.addEventListener("keydown", function (e) {
    var tag = (e.target.tagName || "").toUpperCase();
    var typing = tag === "INPUT" || tag === "TEXTAREA" || tag === "SELECT";
    if (e.key === "Escape") {
      var open = document.querySelector(".modal-backdrop.is-open");
      if (open) { open.classList.remove("is-open"); return; }
      if (typing) return; // trang tự xử lý (vd xoá ô nhập)
      if (document.body.hasAttribute("data-no-back")) return;
      goBack();
    } else if ((e.key === "h" || e.key === "H") && !typing) {
      if (location.pathname.slice(-10) === "index.html" ||
          /\/$/.test(location.pathname)) return;
      location.href = "index.html";
    }
  });

  return { toast: toast, mockAsync: mockAsync, param: param, goBack: goBack };
})();
