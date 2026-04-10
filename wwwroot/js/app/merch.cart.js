// js/app/merch.cart.js
document.addEventListener('DOMContentLoaded', () => {

    // Данные о ценах по id товара
    const prices = {};
    document.querySelectorAll('[data-item-id]').forEach(el => {
        const id = el.dataset.itemId;
        if (!prices[id]) {
            // Получаем цену из текста «XX ₽ за шт.»
            const infoEl = el.closest('.cart-item')?.querySelector('.cart-item__price');
            if (infoEl) {
                const match = infoEl.textContent.match(/([\d.,]+)\s*₽/);
                if (match) prices[id] = parseFloat(match[1].replace(',', '.'));
            }
        }
    });

    let debounceTimers = {};

    function submitUpdate(id) {
        const form = document.querySelector(`[data-update-form="${id}"]`);
        if (form) form.submit();
    }

    // Кнопки «+» и «−»
    document.querySelectorAll('[data-qty-dec], [data-qty-inc]').forEach(btn => {
        btn.addEventListener('click', () => {
            const id = btn.dataset.qtyDec ?? btn.dataset.qtyInc;
            const isInc = btn.hasAttribute('data-qty-inc');

            const valEl = document.querySelector(`[data-qty-val="${id}"]`);
            const inputEl = document.querySelector(`[data-qty-input="${id}"]`);
            const decBtn = document.querySelector(`[data-qty-dec="${id}"]`);
            const totalEl = document.querySelector(`[data-item-total="${id}"]`);

            if (!valEl || !inputEl) return;

            let qty = parseInt(valEl.textContent, 10);
            qty = isInc ? qty + 1 : Math.max(1, qty - 1);

            valEl.textContent = qty;
            inputEl.value = qty;
            if (decBtn) decBtn.disabled = qty <= 1;

            // Пересчёт суммы позиции
            if (totalEl && prices[id]) {
                totalEl.textContent = (prices[id] * qty).toFixed(0) + ' ₽';
            }

            // Дебаунс: отправляем форму через 600мс после последнего клика
            clearTimeout(debounceTimers[id]);
            debounceTimers[id] = setTimeout(() => submitUpdate(id), 600);
        });
    });

});