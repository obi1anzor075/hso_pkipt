// effect/admin-news.edit.js
document.addEventListener('DOMContentLoaded', () => {
    const input = document.getElementById('imageUrlInput');
    const preview = document.getElementById('imagePreview');

    if (!input || !preview) return;

    function updatePreview() {
        const url = input.value.trim();
        if (!url) {
            preview.classList.remove('visible');
            preview.src = '';
            return;
        }
        preview.src = url;
        preview.onload = () => preview.classList.add('visible');
        preview.onerror = () => preview.classList.remove('visible');
    }

    input.addEventListener('input', updatePreview);
    // Показываем уже заполненное при загрузке
    updatePreview();
});