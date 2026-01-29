document.addEventListener('DOMContentLoaded', () => {
  const track = document.querySelector('.track');
  const slides = document.querySelectorAll('.card-news');
  const images = document.querySelectorAll('.card-news img');

  const prevBtn = document.querySelector('[data-fn="slide-prev"]');
  const nextBtn = document.querySelector('[data-fn="slide-next"]');

  const loader = document.getElementById('newsLoader');
  const slider = document.getElementById('newsSlider');

  const visibleSlides = 2;
  const gap = 50;

  let loadedImages = 0;

  // Если вообще нет карточек (на всякий случай)
  if (!slides.length) {
    loader.style.display = 'none';
    return;
  }

  function initSlider() {
    loader.style.display = 'none';
    slider.classList.remove('hidden');

    let index = 0;
    const slideWidth = slides[0].offsetWidth + gap;
    const maxIndex = Math.max(0, slides.length - visibleSlides);

    function update() {
      track.style.transform = `translateX(-${index * slideWidth}px)`;
      prevBtn.disabled = index === 0;
      nextBtn.disabled = index === maxIndex;
    }

    nextBtn.onclick = () => {
      if (index < maxIndex) {
        index++;
        update();
      }
    };

    prevBtn.onclick = () => {
      if (index > 0) {
        index--;
        update();
      }
    };

    update();
  }

  images.forEach(img => {
    if (img.complete) {
      loadedImages++;
    } else {
      img.addEventListener('load', checkLoaded);
      img.addEventListener('error', checkLoaded);
    }
  });

  function checkLoaded() {
    loadedImages++;
    if (loadedImages === images.length) {
      initSlider();
    }
  }

  if (loadedImages === images.length) {
    initSlider();
  }
});