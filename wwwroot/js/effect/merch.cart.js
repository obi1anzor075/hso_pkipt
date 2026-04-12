// js/effect/merch.cart.js
document.addEventListener('DOMContentLoaded', () => {

    // Плавное появление строк
    const items = document.querySelectorAll('.cart-item');
    items.forEach((item, i) => {
        item.style.opacity = '0';
        item.style.transform = 'translateY(12px)';
        item.style.transition = 'opacity 0.3s ease, transform 0.3s ease';
        setTimeout(() => {
            item.style.opacity = '1';
            item.style.transform = 'translateY(0)';
        }, 60 + i * 60);
    });

    // Подтверждение очистки корзины
    const clearForm = document.querySelector('form[action*="ClearCart"]');
    if (clearForm) {
        clearForm.addEventListener('submit', e => {
            if (!confirm('Очистить всю корзину?')) e.preventDefault();
        });
    }

});