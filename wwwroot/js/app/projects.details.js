// app/projects.details.js
document.addEventListener('DOMContentLoaded', () => {
  const lightbox = document.getElementById('lightbox');
  const lightboxImg = document.getElementById('lightboxImg');
  const lightboxClose = document.getElementById('lightboxClose');
  const lightboxBackdrop = document.getElementById('lightboxBackdrop');

  if (!lightbox || !lightboxImg) return;

  function openLightbox(src, alt) {
    lightboxImg.src = src;
    lightboxImg.alt = alt || '';
    lightbox.classList.add('open');
    lightbox.setAttribute('aria-hidden', 'false');
    document.body.style.overflow = 'hidden';
  }

  function closeLightbox() {
    lightbox.classList.remove('open');
    lightbox.setAttribute('aria-hidden', 'true');
    document.body.style.overflow = '';
    setTimeout(() => { lightboxImg.src = ''; }, 300);
  }

  // Главное изображение
  const mainImageWrap = document.querySelector('[data-zoom]');
  if (mainImageWrap) {
    mainImageWrap.addEventListener('click', () => {
      const img = mainImageWrap.querySelector('img');
      if (img) openLightbox(img.src, img.alt);
    });
  }

  // Изображения внутри контента
  document.querySelectorAll('.project-details__content img').forEach(img => {
    img.addEventListener('click', () => openLightbox(img.src, img.alt));
  });

  if (lightboxClose) lightboxClose.addEventListener('click', closeLightbox);
  if (lightboxBackdrop) lightboxBackdrop.addEventListener('click', closeLightbox);

  document.addEventListener('keydown', (e) => {
    if (e.key === 'Escape' && lightbox.classList.contains('open')) closeLightbox();
  });
});