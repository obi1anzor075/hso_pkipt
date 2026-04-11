// js/app/home.about.js
document.addEventListener('DOMContentLoaded', () => {
    initCounters();
});

function initCounters() {
    const stats = document.querySelectorAll('.about-hero__stat .value');
    if (!stats.length) return;

    const observer = new IntersectionObserver((entries, obs) => {
        entries.forEach(entry => {
            if (!entry.isIntersecting) return;
            animateCounter(entry.target);
            obs.unobserve(entry.target);
        });
    }, { threshold: 0.5 });

    stats.forEach(el => observer.observe(el));
}

function animateCounter(el) {
    const raw = el.textContent.trim();
    const suffix = raw.replace(/[\d]/g, '');   // «+», «лет», «год» и т.д.
    const numStr = raw.replace(/\D/g, '');
    const target = parseInt(numStr, 10);

    // Для года «2016» просто показываем без анимации
    if (target > 500) return;

    const duration = 900;
    const start = performance.now();
    const from = 0;

    function tick(now) {
        const elapsed = now - start;
        const progress = Math.min(elapsed / duration, 1);
        // ease-out cubic
        const eased = 1 - Math.pow(1 - progress, 3);
        const current = Math.round(from + (target - from) * eased);

        el.textContent = current + suffix;

        if (progress < 1) requestAnimationFrame(tick);
    }

    requestAnimationFrame(tick);
}