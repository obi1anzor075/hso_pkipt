// js/effect/merch.index.js
document.addEventListener('DOMContentLoaded', () => {

    // Toast-уведомление при отправке формы «В корзину»
    const toast = document.getElementById('merchToast');

    document.querySelectorAll('form:has([data-add-btn])').forEach(form => {
        form.addEventListener('submit', e => {
            if (!toast) return;

            const btn = form.querySelector('[data-add-btn]');
            const name = btn?.closest('.merch-card')
                ?.querySelector('.merch-card__name')?.textContent?.trim();

            toast.textContent = name ? `«${name}» добавлен в корзину` : 'Добавлено в корзину';
            toast.classList.add('visible');

            clearTimeout(toast._hideTimer);
            toast._hideTimer = setTimeout(() => {
                toast.classList.remove('visible');
            }, 2200);
        });
    });

});