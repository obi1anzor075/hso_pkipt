// js/effect/home.about.js
document.addEventListener('DOMContentLoaded', () => {
    initAboutAnimations();
});

function initAboutAnimations() {
    const elements = document.querySelectorAll('[data-about-animate]');
    if (!elements.length) return;

    const observer = new IntersectionObserver((entries, obs) => {
        entries.forEach(entry => {
            if (!entry.isIntersecting) return;

            const el = entry.target;

            // Дочерние карточки анимируем по очереди
            const cards = el.querySelectorAll(
                '.about-direction-card, .about-why-card, .about-timeline-item'
            );

            if (cards.length) {
                cards.forEach((card, i) => {
                    card.style.opacity = '0';
                    card.style.transform = 'translateY(16px)';
                    card.style.transition = `opacity 0.4s ease ${i * 0.08}s, transform 0.4s ease ${i * 0.08}s`;

                    // Запускаем через один frame чтобы transition сработал
                    requestAnimationFrame(() => {
                        requestAnimationFrame(() => {
                            card.style.opacity = '1';
                            card.style.transform = 'translateY(0)';
                        });
                    });
                });
            }

            el.classList.add('is-visible');
            obs.unobserve(el);
        });
    }, { threshold: 0.12 });

    elements.forEach(el => observer.observe(el));
}