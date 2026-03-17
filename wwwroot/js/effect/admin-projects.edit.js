// js/effect/admin-projects.edit.js
document.addEventListener('DOMContentLoaded', () => {
  const input = document.getElementById('imageUrlInput');
  const preview = document.getElementById('imagePreview');
  if (!input || !preview) return;

  // Показываем уже загруженный URL
  if (input.value.trim()) {
    preview.classList.add('visible');
    preview.onerror = () => preview.classList.remove('visible');
  }

  input.addEventListener('input', () => {
    const url = input.value.trim();
    if (url) {
      preview.src = url;
      preview.classList.add('visible');
      preview.onerror = () => preview.classList.remove('visible');
    } else {
      preview.src = '';
      preview.classList.remove('visible');
    }
  });
});