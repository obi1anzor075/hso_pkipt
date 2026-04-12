document.addEventListener('DOMContentLoaded', () => {
    const prices = {};
    document.querySelectorAll('[data-item-id]').forEach(el => {
        const id = el.dataset.itemId;
        if (!id || prices[id]) return;

        const infoEl = el.closest('.cart-item')?.querySelector('.cart-item__price');
        if (infoEl) {
            const match = infoEl.textContent.match(/([\d.,]+)\s*₽/);
            if (match) {
                prices[id] = parseFloat(match[1].replace(',', '.'));
            }
        }
    });

    const debounceTimers = {};

    function submitUpdate(id) {
        const form = document.querySelector(`[data-update-form="${id}"]`);
        if (form) form.submit();
    }

    document.querySelectorAll('[data-qty-dec], [data-qty-inc]').forEach(btn => {
        btn.addEventListener('click', () => {
            const id = btn.dataset.itemId;
            const isInc = btn.hasAttribute('data-qty-inc');

            const valEl = document.querySelector(`[data-qty-val="${id}"]`);
            const inputEl = document.querySelector(`[data-qty-input="${id}"]`);
            const decBtn = document.querySelector(`[data-qty-dec][data-item-id="${id}"]`);
            const totalEl = document.querySelector(`[data-item-total="${id}"]`);
            const cartTotalEl = document.getElementById('cartTotal');

            if (!id || !valEl || !inputEl) return;

            let qty = parseInt(valEl.textContent, 10) || 1;
            qty = isInc ? qty + 1 : Math.max(1, qty - 1);

            valEl.textContent = qty;
            inputEl.value = qty;

            if (decBtn) {
                decBtn.disabled = qty <= 1;
            }

            if (totalEl && prices[id]) {
                totalEl.textContent = `${(prices[id] * qty).toFixed(0)} ₽`;
            }

            // Можно сразу обновить общий итог на клиенте
            if (cartTotalEl) {
                let total = 0;
                document.querySelectorAll('[data-item-total]').forEach(el => {
                    const match = el.textContent.match(/([\d.,]+)/);
                    if (match) {
                        total += parseFloat(match[1].replace(',', '.'));
                    }
                });
                cartTotalEl.textContent = `${total.toFixed(0)} ₽`;
            }

            clearTimeout(debounceTimers[id]);
            debounceTimers[id] = setTimeout(() => submitUpdate(id), 600);
        });
    });
});