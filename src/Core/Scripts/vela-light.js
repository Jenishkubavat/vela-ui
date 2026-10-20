/* Vela light: the optional motion layer. Without it every state still shows; each
   component draws its own focus outline and nothing moves between elements.

   - One focus light travels between whatever has keyboard focus inside a Vela component.
   - When an element gets data-ember (set it when the person finishes something, never for
     programmatic changes), a spark flies from it to the nearest progress wick.
   - Elements with data-roll turn their figures over when their text changes.
   - Vela.morph(from, to, update) opens an overlay out of its trigger with a view transition.

   Load it once, after the page body, with <script src="vela-light.js"></script>. It needs the
   motion rules at the end of vela.css. Ported from the Vela design system's reference
   implementation, keyed on data-slot instead of vela-* classes. */
(() => {
  if (window.Vela && window.Vela.light) return;
  const root = document.documentElement;
  const reduced = matchMedia("(prefers-reduced-motion: reduce)");
  const ease = "cubic-bezier(0.16, 1, 0.3, 1)";

  // ---------- The travelling focus light ----------
  const light = document.createElement("div");
  light.className = "vela-focus-light";
  light.setAttribute("aria-hidden", "true");
  let target = null;
  let raf = 0;

  // Blazor's enhanced navigation can replace the body and drop the light; put it back.
  const mount = () => {
    if (!light.isConnected && document.body) document.body.append(light);
    if (!root.hasAttribute("data-vela-light")) root.setAttribute("data-vela-light", "");
  };

  const place = jump => {
    if (!target || !target.isConnected) { light.dataset.state = "off"; return; }
    mount();
    const r = target.getBoundingClientRect();
    const inset = 3;
    const radius = parseFloat(getComputedStyle(target).borderTopLeftRadius) || 0;
    if (jump || reduced.matches) light.setAttribute("data-jump", "");
    light.style.width = `${r.width + inset * 2}px`;
    light.style.height = `${r.height + inset * 2}px`;
    light.style.transform = `translate(${r.left - inset}px, ${r.top - inset}px)`;
    light.style.borderRadius = `${radius + inset}px`;
    light.dataset.state = "on";
    if (light.hasAttribute("data-jump")) requestAnimationFrame(() => light.removeAttribute("data-jump"));
  };
  const follow = () => { cancelAnimationFrame(raf); raf = requestAnimationFrame(() => place(true)); };

  document.addEventListener("focusin", e => {
    const el = e.target;
    // Only keyboard focus, and only on or inside Vela components (they all carry data-slot).
    if (!el.matches || !el.matches(":focus-visible") || !el.closest("[data-slot]")) { target = null; place(); return; }
    const first = light.dataset.state !== "on";
    target = el;
    place(first);
  });
  document.addEventListener("focusout", () => setTimeout(() => {
    if (!document.activeElement || document.activeElement === document.body) { target = null; place(); }
  }, 0));
  addEventListener("scroll", follow, true);
  addEventListener("resize", follow);
  new ResizeObserver(follow).observe(root);

  // ---------- The ember's flight to the wick ----------
  const flyToWick = from => {
    const wick = document.querySelector("[data-slot='progress']:not([data-state='indeterminate'])");
    if (!wick) return;
    const bar = wick.querySelector("[data-slot='progress-indicator']");
    const a = from.getBoundingClientRect();
    const b = (bar || wick).getBoundingClientRect();
    const catchIt = () => { wick.setAttribute("data-receiving", ""); setTimeout(() => wick.removeAttribute("data-receiving"), 900); };
    if (reduced.matches) { catchIt(); return; }
    const spark = document.createElement("div");
    spark.className = "vela-ember-spark";
    document.body.append(spark);
    const x0 = a.left + a.width / 2, y0 = a.top + a.height / 2;
    const x1 = b.right, y1 = b.top + b.height / 2;
    const lift = Math.min(160, Math.abs(x1 - x0) * 0.35 + 40);
    spark.animate([
      { transform: `translate(${x0}px, ${y0}px) scale(1.6)`, opacity: 1 },
      { transform: `translate(${(x0 + x1) / 2}px, ${Math.min(y0, y1) - lift}px) scale(1)`, opacity: 1, offset: 0.55 },
      { transform: `translate(${x1}px, ${y1}px) scale(0.6)`, opacity: 0.9 },
    ], { duration: 700, easing: ease, fill: "forwards" }).finished.then(() => { spark.remove(); catchIt(); });
  };
  new MutationObserver(list => {
    for (const m of list) {
      // A toast confirming a completion carries the ember itself; it doesn't send a spark.
      if (m.type === "attributes" && m.attributeName === "data-ember" && m.target.hasAttribute("data-ember") && !m.target.closest("[data-slot='toast']")) flyToWick(m.target);
      if (m.type === "characterData" || m.type === "childList") {
        const el = (m.target.nodeType === 3 ? m.target.parentElement : m.target)?.closest("[data-roll]");
        if (el && !el.dataset.rolling) roll(el, m.oldValue);
      }
    }
  }).observe(document, { subtree: true, attributes: true, attributeFilter: ["data-ember"], characterData: true, characterDataOldValue: true, childList: true });

  // ---------- Rolling figures ----------
  function roll(el, old) {
    if (reduced.matches || old == null) return;
    const now = el.textContent;
    const up = parseFloat(now) >= parseFloat(old) || isNaN(parseFloat(now));
    el.dataset.rolling = "1";
    el.animate([{ transform: `translateY(${up ? 60 : -60}%)`, opacity: 0 }, { transform: "none", opacity: 1 }],
      { duration: 240, easing: ease }).finished.finally(() => delete el.dataset.rolling);
  }

  // ---------- Overlays that open out of their trigger ----------
  const morph = (from, to, update) => {
    if (!document.startViewTransition || reduced.matches) { update(); return Promise.resolve(); }
    from.style.viewTransitionName = "vela-morph";
    const t = document.startViewTransition(() => { from.style.viewTransitionName = ""; update(); to.style.viewTransitionName = "vela-morph"; });
    return t.finished.finally(() => { to.style.viewTransitionName = ""; });
  };

  document.body ? mount() : addEventListener("DOMContentLoaded", mount);
  window.Vela = Object.assign(window.Vela || {}, { light: true, morph, flyToWick });
})();
