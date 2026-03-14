// app\news.details.js
document.addEventListener('DOMContentLoaded', () => {
  const lightbox = document.getElementById('lightbox');
  const lightboxImg = document.getElementById('lightboxImg');
  const lightboxClose = document.getElementById('lightboxClose');
  const lightboxBackdrop = document.getElementById('lightboxBackdrop');

  if (!lightbox || !lightboxImg) return;

  // Открыть лайтбокс
  function openLightbox(src, alt) {
    lightboxImg.src = src;
    lightboxImg.alt = alt || '';
    lightbox.classList.add('open');
    lightbox.setAttribute('aria-hidden', 'false');
    document.body.style.overflow = 'hidden';
  }

  // Закрыть лайтбокс
  function closeLightbox() {
    lightbox.classList.remove('open');
    lightbox.setAttribute('aria-hidden', 'true');
    document.body.style.overflow = '';

    // Очищаем src после завершения анимации
    setTimeout(() => {
      lightboxImg.src = '';
    }, 300);
  }

  // Клик на главное изображение статьи
  const mainImageWrap = document.querySelector('[data-zoom]');
  if (mainImageWrap) {
    mainImageWrap.addEventListener('click', () => {
      const img = mainImageWrap.querySelector('img');
      if (img) openLightbox(img.src, img.alt);
    });
  }

  // Клик на изображения внутри контента
  document.querySelectorAll('.news-details__content img').forEach(img => {
    img.addEventListener('click', () => {
      openLightbox(img.src, img.alt);
    });
  });

  // Закрытие
  if (lightboxClose) lightboxClose.addEventListener('click', closeLightbox);
  if (lightboxBackdrop) lightboxBackdrop.addEventListener('click', closeLightbox);

  // Закрытие клавишей Escape
  document.addEventListener('keydown', (e) => {
    if (e.key === 'Escape' && lightbox.classList.contains('open')) {
      closeLightbox();
    }
  });
});