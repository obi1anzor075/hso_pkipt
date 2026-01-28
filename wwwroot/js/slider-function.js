document.addEventListener('DOMContentLoaded', () => {
  const track = document.querySelector('.track');
  const slides = document.querySelectorAll('.card-news');
  const prevBtn = document.querySelector('[data-fn="slide-prev"]');
  const nextBtn = document.querySelector('[data-fn="slide-next"]');

  const visibleSlides = 2;
  const gap = 50;

  if (slides.length <= visibleSlides) {
    prevBtn.disabled = true;
    nextBtn.disabled = true;
    return;
  }

  let index = 0;
  const slideWidth = slides[0].offsetWidth + gap;
  const maxIndex = slides.length - visibleSlides;

  function update() {
    track.style.transform = `translateX(-${index * slideWidth}px)`;

    prevBtn.disabled = index === 0;
    nextBtn.disabled = index === maxIndex;
  }

  nextBtn.addEventListener('click', () => {
    if (index < maxIndex) {
      index++;
      update();
    }
  });

  prevBtn.addEventListener('click', () => {
    if (index > 0) {
      index--;
      update();
    }
  });

  update();
});