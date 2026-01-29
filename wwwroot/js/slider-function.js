document.addEventListener('DOMContentLoaded', () => {
  const track = document.querySelector('.track');
  const slides = document.querySelectorAll('.card-news');
  const images = document.querySelectorAll('.card-news img');

  const prevBtn = document.querySelector('[data-fn="slide-prev"]');
  const nextBtn = document.querySelector('[data-fn="slide-next"]');

  const loader = document.getElementById('newsLoader');
  const slider = document.getElementById('newsSlider');

  const gap = 50;
  let loadedImages = 0;
  let index = 0;
  let visibleSlides = 2;
  let slideWidth = 0;
  let maxIndex = 0;
  let resizeTimeout = null;

  // Если нет карточек, скрыть лоадер
  if (!slides.length) {
    if (loader) loader.style.display = 'none';
    return;
  }

  // Функция определения количества видимых слайдов в зависимости от ширины экрана
  function getVisibleSlides() {
    const width = window.innerWidth;
    if (width <= 768) {
      return 1; // На мобильных - 1 слайд
    } else {
      return 2; // На десктопе и планшете - 2 слайда
    }
  }

  // Функция получения gap в зависимости от ширины экрана
  function getGap() {
    const width = window.innerWidth;
    if (width <= 480) {
      return 20;
    } else if (width <= 768) {
      return 20;
    } else if (width <= 1024) {
      return 30;
    } else {
      return 50;
    }
  }

  // Функция расчета параметров слайдера
  function calculateSliderParams() {
    visibleSlides = getVisibleSlides();
    const currentGap = getGap();

    if (slides.length > 0) {
      slideWidth = slides[0].offsetWidth + currentGap;
      maxIndex = Math.max(0, slides.length - visibleSlides);
    }

    // Сброс индекса, если он вышел за пределы
    if (index > maxIndex) {
      index = maxIndex;
    }
  }

  // Функция обновления состояния слайдера
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

  // Инициализация слайдера
  function initSlider() {
    if (loader) {
      loader.style.display = 'none';
    }

    if (slider) {
      slider.classList.remove('hidden');
    }

    calculateSliderParams();
    update();
  }

  // Обработчик клика на кнопку "Вперед"
  if (nextBtn) {
    nextBtn.onclick = () => {
      if (index < maxIndex) {
        index++;
        update();
      }
    };
  }

  // Обработчик клика на кнопку "Назад"
  if (prevBtn) {
    prevBtn.onclick = () => {
      if (index > 0) {
        index--;
        update();
      }
    };
  }

  // Обработчик изменения размера окна с debounce
  window.addEventListener('resize', () => {
    if (resizeTimeout) {
      clearTimeout(resizeTimeout);
    }

    resizeTimeout = setTimeout(() => {
      update();
    }, 150);
  });

  // Обработчик загрузки изображений
  function checkLoaded() {
    loadedImages++;
    if (loadedImages === images.length) {
      initSlider();
    }
  }

  // Проверка загрузки всех изображений
  images.forEach(img => {
    if (img.complete) {
      loadedImages++;
    } else {
      img.addEventListener('load', checkLoaded);
      img.addEventListener('error', checkLoaded); // Обработка ошибок загрузки
    }
  });

  // Если все изображения уже загружены
  if (loadedImages === images.length) {
    initSlider();
  }

  // Touch события для свайпа на мобильных устройствах
  let touchStartX = 0;
  let touchEndX = 0;
  const minSwipeDistance = 50;

  if (track) {
    track.addEventListener('touchstart', (e) => {
      touchStartX = e.changedTouches[0].screenX;
    }, { passive: true });

    track.addEventListener('touchend', (e) => {
      touchEndX = e.changedTouches[0].screenX;
      handleSwipe();
    }, { passive: true });
  }

  function handleSwipe() {
    const swipeDistance = touchStartX - touchEndX;

    if (Math.abs(swipeDistance) > minSwipeDistance) {
      if (swipeDistance > 0 && index < maxIndex) {
        // Свайп влево - следующий слайд
        index++;
        update();
      } else if (swipeDistance < 0 && index > 0) {
        // Свайп вправо - предыдущий слайд
        index--;
        update();
      }
    }
  }

  // Поддержка навигации с клавиатуры
  document.addEventListener('keydown', (e) => {
    if (e.key === 'ArrowLeft' && index > 0) {
      index--;
      update();
    } else if (e.key === 'ArrowRight' && index < maxIndex) {
      index++;
      update();
    }
  });
});