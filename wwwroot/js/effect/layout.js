// effect/layout.js
(function () {
    const btn = document.getElementById('burgerBtn');
    const nav = document.getElementById('navCenter');
    if (!btn || !nav) return;

    function openNav() {
        nav.classList.add('open');
        btn.classList.add('open');
        btn.setAttribute('aria-expanded', 'true');
        document.body.classList.add('nav-open');
    }

    function closeNav() {
        nav.classList.remove('open');
        btn.classList.remove('open');
        btn.setAttribute('aria-expanded', 'false');
        document.body.classList.remove('nav-open');
    }

    btn.addEventListener('click', function () {
        nav.classList.contains('open') ? closeNav() : openNav();
    });

    // Закрытие по клику на оверлей (::before псевдоэлемент)
    nav.addEventListener('click', function (e) {
        // Клик по самому .center (т.е. за пределами ссылок) = клик по оверлею
        if (e.target === nav) closeNav();
    });

    // Закрытие при клике на ссылку
    nav.querySelectorAll('.link').forEach(function (link) {
        link.addEventListener('click', closeNav);
    });

    // Закрытие по Escape
    document.addEventListener('keydown', function (e) {
        if (e.key === 'Escape' && nav.classList.contains('open')) closeNav();
    });
})();