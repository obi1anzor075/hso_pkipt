document.addEventListener('DOMContentLoaded', () => {
  const newsGrid = document.getElementById('newsGrid');
  const btnLoadMore = document.getElementById('btnLoadMore');
  const loadMoreContainer = document.getElementById('loadMoreContainer');
  const loadingSpinner = document.getElementById('loadingSpinner');

  const ITEMS_PER_PAGE = 9;
  let currentPage = 1;
  let isLoading = false;

  // Инициализация
  function init() {
    const newsCards = document.querySelectorAll('.card-news');

    // Если на странице меньше ITEMS_PER_PAGE новостей, скрываем кнопку
    if (newsCards.length < ITEMS_PER_PAGE) {
      if (loadMoreContainer) {
        loadMoreContainer.classList.add('hidden');
      }
    }

    // Добавляем обработчики клика на карточки
    addCardClickHandlers();
  }

  // Загрузка дополнительных новостей через AJAX
  async function loadMoreNews() {
    if (isLoading) return;

    isLoading = true;
    currentPage++;

    // Показываем индикатор загрузки
    if (btnLoadMore) {
      btnLoadMore.disabled = true;
    }
    if (loadingSpinner) {
      loadingSpinner.classList.remove('hidden');
    }

    try {
      // Отправляем запрос на сервер (путь к HomeController)
      const response = await fetch(`/Home/LoadMoreNews?page=${currentPage}&pageSize=${ITEMS_PER_PAGE}`, {
        method: 'GET',
        headers: {
          'X-Requested-With': 'XMLHttpRequest'
        }
      });

      if (!response.ok) {
        throw new Error('Ошибка загрузки новостей');
      }

      const html = await response.text();

      // Создаем временный контейнер для парсинга HTML
      const tempDiv = document.createElement('div');
      tempDiv.innerHTML = html;

      // Получаем новые карточки
      const newCards = tempDiv.querySelectorAll('.card-news');

      // Если новых карточек нет, скрываем кнопку
      if (newCards.length === 0) {
        if (loadMoreContainer) {
          loadMoreContainer.classList.add('hidden');
        }
        if (btnLoadMore) {
          btnLoadMore.textContent = 'Все новости загружены';
          setTimeout(() => {
            loadMoreContainer.classList.add('hidden');
          }, 2000);
        }
        return;
      }

      // Добавляем новые карточки в сетку с небольшой анимацией
      newCards.forEach((card, index) => {
        setTimeout(() => {
          card.style.opacity = '0';
          card.style.transform = 'translateY(20px)';
          newsGrid.appendChild(card);

          // Анимация появления
          setTimeout(() => {
            card.style.transition = 'opacity 0.3s ease, transform 0.3s ease';
            card.style.opacity = '1';
            card.style.transform = 'translateY(0)';
          }, 50);
        }, index * 50);
      });

      // Если загружено меньше карточек, чем ITEMS_PER_PAGE, значит это последняя страница
      if (newCards.length < ITEMS_PER_PAGE) {
        setTimeout(() => {
          if (loadMoreContainer) {
            loadMoreContainer.classList.add('hidden');
          }
        }, newCards.length * 50 + 500);
      }

      // Добавляем обработчики клика на новые карточки
      setTimeout(() => {
        addCardClickHandlers();
      }, newCards.length * 50 + 100);

      // Плавная прокрутка к первой новой карточке
      const firstNewCard = newCards[0];
      if (firstNewCard) {
        setTimeout(() => {
          const actualCard = Array.from(newsGrid.children).find(
            child => child.getAttribute('data-news-id') === firstNewCard.getAttribute('data-news-id')
          );
          if (actualCard) {
            actualCard.scrollIntoView({ behavior: 'smooth', block: 'nearest' });
          }
        }, 200);
      }

    } catch (error) {
      console.error('Ошибка при загрузке новостей:', error);

      // Показываем сообщение об ошибке
      if (btnLoadMore) {
        const originalText = btnLoadMore.textContent;
        btnLoadMore.textContent = 'Ошибка загрузки. Попробуйте еще раз';
        btnLoadMore.style.backgroundColor = '#dc3545';

        setTimeout(() => {
          btnLoadMore.textContent = originalText;
          btnLoadMore.style.backgroundColor = '';
        }, 3000);
      }

      currentPage--; // Откатываем номер страницы при ошибке
    } finally {
      // Скрываем индикатор загрузки
      if (btnLoadMore) {
        btnLoadMore.disabled = false;
      }
      if (loadingSpinner) {
        loadingSpinner.classList.add('hidden');
      }

      isLoading = false;
    }
  }

  // Обработчик клика на карточку
  function addCardClickHandlers() {
    const cards = document.querySelectorAll('.card-news');
    cards.forEach(card => {
      // Проверяем, что обработчик еще не добавлен
      if (card.dataset.clickHandlerAdded) return;

      card.dataset.clickHandlerAdded = 'true';
      card.style.cursor = 'pointer';

      card.addEventListener('click', function (e) {
        // Игнорируем клик, если это карточка с ошибкой
        if (this.classList.contains('not-load')) {
          return;
        }

        const newsId = this.getAttribute('data-news-id');
        if (newsId) {
          // Переход на страницу новости
          window.location.href = `/Home/NewsDetails/${newsId}`;
        }
      });
    });
  }

  // Обработчик кнопки "Показать еще"
  if (btnLoadMore) {
    btnLoadMore.addEventListener('click', loadMoreNews);
  }

  // Инициализация при загрузке страницы
  init();
});