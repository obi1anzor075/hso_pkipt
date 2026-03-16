document.addEventListener('DOMContentLoaded', () => {
  initJoinUsModal();
});

function initJoinUsModal() {
  const triggerSelector = '[data-fn="modal__join-us"]';
  let activeModal = null;
  let isClosing = false;

  function buildModal() {
    const overlay = document.createElement('div');
    overlay.className = 'join-us-modal-overlay';

    overlay.innerHTML = `
      <div class="join-us-modal" role="dialog" aria-modal="true" aria-labelledby="joinUsTitle">
        <button class="join-us-close" type="button" aria-label="Закрыть модальное окно">×</button>

        <div class="join-us-content">
          <div class="join-us-badge">ШСО ПКИПТ</div>
          <h2 id="joinUsTitle" class="join-us-title">Присоединяйся к студенческим отрядам</h2>
          <p class="join-us-text">
            Хочешь быть частью команды, участвовать в проектах, получать опыт,
            знакомиться с активными ребятами и делать студенческую жизнь ярче?
            Присоединяйся к ШСО ПКИПТ — мы ждём именно тебя.
          </p>

          <a href="https://vk.com/phaza58"
             target="_blank"
             rel="noopener noreferrer"
             class="join-us-vk-btn">
            <img src="/assets/icon/vk.svg" alt="VK">
            <span>Перейти в сообщество ВКонтакте</span>
          </a>
        </div>
      </div>
    `;

    return overlay;
  }

  function lockBodyScroll() {
    document.body.classList.add('modal-open');
  }

  function unlockBodyScroll() {
    document.body.classList.remove('modal-open');
  }

  function forceRemoveExistingModals() {
    const modals = document.querySelectorAll('.join-us-modal-overlay');
    modals.forEach((modal) => modal.remove());
    activeModal = null;
    unlockBodyScroll();
  }

  function openModal() {
    forceRemoveExistingModals();

    const modal = buildModal();
    document.body.appendChild(modal);

    requestAnimationFrame(() => {
      modal.classList.add('is-active');
    });

    const closeBtn = modal.querySelector('.join-us-close');

    function closeModal() {
      if (!modal || isClosing) return;
      isClosing = true;

      modal.classList.remove('is-active');
      modal.classList.add('is-closing');

      setTimeout(() => {
        if (modal.parentNode) {
          modal.remove();
        }

        if (activeModal === modal) {
          activeModal = null;
        }

        unlockBodyScroll();
        isClosing = false;
      }, 250);
    }

    closeBtn.addEventListener('click', closeModal);

    modal.addEventListener('click', (e) => {
      if (e.target === modal) {
        closeModal();
      }
    });

    document.addEventListener('keydown', function escHandler(e) {
      if (e.key === 'Escape' && activeModal === modal) {
        closeModal();
        document.removeEventListener('keydown', escHandler);
      }
    });

    activeModal = modal;
    lockBodyScroll();
  }

  document.addEventListener('click', (e) => {
    const trigger = e.target.closest(triggerSelector);
    if (!trigger) return;

    e.preventDefault();
    openModal();
  });
}