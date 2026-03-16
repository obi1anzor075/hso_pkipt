document.addEventListener('DOMContentLoaded', () => {
  initSliders();
  initScrollAnimations();
});

function initSliders() {
  const sliders = document.querySelectorAll('[data-slider]');
  if (!sliders.length) return;

  sliders.forEach((slider) => {
    const track = slider.querySelector('[data-slider-track]');
    const slides = slider.querySelectorAll('.card-news');
    const images = slider.querySelectorAll('.card-news img');
    const prevBtn = slider.querySelector('[data-slider-prev]');
    const nextBtn = slider.querySelector('[data-slider-next]');
    const loader = slider.querySelector('[data-slider-loader]');
    const wrapper = slider.querySelector('[data-slider-wrapper]');

    let loadedImages = 0;
    let index = 0;
    let visibleSlides = 2;
    let slideWidth = 0;
    let maxIndex = 0;
    let resizeTimeout = null;
    let touchStartX = 0;
    let touchEndX = 0;
    const minSwipeDistance = 50;

    if (!slides.length) {
      if (loader) loader.style.display = 'none';
      return;
    }

    function getVisibleSlides() {
      return window.innerWidth <= 768 ? 1 : 2;
    }

    function getGap() {
      const width = window.innerWidth;

      if (width <= 768) return 20;
      if (width <= 1024) return 30;
      return 50;
    }

    function calculateSliderParams() {
      visibleSlides = getVisibleSlides();
      const currentGap = getGap();

      if (slides.length > 0) {
        slideWidth = slides[0].offsetWidth + currentGap;
        maxIndex = Math.max(0, slides.length - visibleSlides);
      }

      if (index > maxIndex) {
        index = maxIndex;
      }
    }

    function update() {
      calculateSliderParams();

      if (track) {
        track.style.transform = `translateX(-${index * slideWidth}px)`;
      }

      if (prevBtn) {
        prevBtn.disabled = index === 0;
      }

      if (nextBtn) {
        nextBtn.disabled = index === maxIndex;
      }
    }

    function initSlider() {
      if (loader) {
        loader.style.display = 'none';
      }

      if (wrapper) {
        wrapper.classList.remove('hidden');
      }

      calculateSliderParams();
      update();
    }

    function checkLoaded() {
      loadedImages++;
      if (loadedImages === images.length) {
        initSlider();
      }
    }

    images.forEach((img) => {
      if (img.complete) {
        loadedImages++;
      } else {
        img.addEventListener('load', checkLoaded);
        img.addEventListener('error', checkLoaded);
      }
    });

    if (loadedImages === images.length) {
      initSlider();
    }

    if (nextBtn) {
      nextBtn.addEventListener('click', () => {
        if (index < maxIndex) {
          index++;
          update();
        }
      });
    }

    if (prevBtn) {
      prevBtn.addEventListener('click', () => {
        if (index > 0) {
          index--;
          update();
        }
      });
    }

    window.addEventListener('resize', () => {
      clearTimeout(resizeTimeout);
      resizeTimeout = setTimeout(update, 150);
    });

    if (track) {
      track.addEventListener('touchstart', (e) => {
        touchStartX = e.changedTouches[0].screenX;
      }, { passive: true });

      track.addEventListener('touchend', (e) => {
        touchEndX = e.changedTouches[0].screenX;
        const swipeDistance = touchStartX - touchEndX;

        if (Math.abs(swipeDistance) > minSwipeDistance) {
          if (swipeDistance > 0 && index < maxIndex) {
            index++;
            update();
          } else if (swipeDistance < 0 && index > 0) {
            index--;
            update();
          }
        }
      }, { passive: true });
    }
  });
}

function initScrollAnimations() {
  const elements = document.querySelectorAll('[data-animate]');
  if (!elements.length) return;

  const observer = new IntersectionObserver((entries, obs) => {
    entries.forEach((entry) => {
      if (entry.isIntersecting) {
        entry.target.classList.add('is-visible');
        obs.unobserve(entry.target);
      }
    });
  }, {
    threshold: 0.15
  });

  elements.forEach((el) => observer.observe(el));
}