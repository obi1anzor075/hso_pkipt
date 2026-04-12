// js/app/profile.index.js
document.addEventListener('DOMContentLoaded', () => {

  /* ─── Вкладки ─── */
  const navBtns = document.querySelectorAll('.profile-nav__item');
  const sections = document.querySelectorAll('.profile-section');

    navBtns.forEach(btn => {
        btn.addEventListener('click', () => {
            const target = btn.dataset.tab;

            navBtns.forEach(b => b.classList.remove('active'));
            sections.forEach(s => s.classList.remove('active'));

            btn.classList.add('active');
            const section = document.getElementById('tab-' + target);
            if (section) section.classList.add('active');

            if (target === 'events') {
                requestAnimationFrame(() => {
                    window.dispatchEvent(new Event('resize'));
                });
            }
        });
    });

  /* ─── Превью и загрузка фото ─── */
  const photoInput = document.getElementById('photoInput');
  const avatarPreview = document.getElementById('avatarPreview');
  const photoBanner = document.getElementById('photoBanner');
  const bannerImg = document.getElementById('photoBannerImg');
  const bannerHint = document.getElementById('photoBannerHint');
  const confirmBtn = document.getElementById('photoConfirmBtn');
  const cancelBtn = document.getElementById('photoCancelBtn');
  const photoForm = document.getElementById('photoForm');

  const MAX_SIZE_MB = 5;
  const ALLOWED_TYPES = ['image/jpeg', 'image/png', 'image/webp'];

  function showBanner(src) {
    bannerImg.src = src;
    bannerHint.textContent = 'Выглядит хорошо?';
    bannerHint.classList.remove('error');
    photoBanner.setAttribute('aria-hidden', 'false');
    photoBanner.classList.add('visible');
  }

  function hideBanner() {
    photoBanner.classList.remove('visible');
    photoBanner.setAttribute('aria-hidden', 'true');
    setTimeout(() => { bannerImg.src = ''; }, 400);
  }

  function showBannerError(msg) {
    bannerHint.textContent = msg;
    bannerHint.classList.add('error');
    photoBanner.classList.add('visible');
    // Автоскрыть ошибку через 3 сек
    setTimeout(hideBanner, 3000);
  }

  if (photoInput) {
    photoInput.addEventListener('change', () => {
      const file = photoInput.files[0];
      if (!file) return;

      if (!ALLOWED_TYPES.includes(file.type)) {
        showBannerError('Допустимые форматы: JPEG, PNG, WebP');
        photoInput.value = '';
        return;
      }

      if (file.size > MAX_SIZE_MB * 1024 * 1024) {
        showBannerError(`Файл слишком большой. Максимум ${MAX_SIZE_MB} МБ`);
        photoInput.value = '';
        return;
      }

      const reader = new FileReader();
      reader.onload = (e) => {
        // Обновляем аватар в сайдбаре сразу как превью
        if (avatarPreview) avatarPreview.src = e.target.result;
        showBanner(e.target.result);
      };
      reader.readAsDataURL(file);
    });
  }

  if (confirmBtn) {
    confirmBtn.addEventListener('click', () => {
      if (photoForm) photoForm.submit();
    });
  }

  if (cancelBtn) {
    cancelBtn.addEventListener('click', () => {
      hideBanner();
      if (photoInput) photoInput.value = '';
      // Возвращаем оригинальный аватар
      const originalSrc = document.querySelector('.profile-sidebar__avatar')?.dataset.original;
      if (avatarPreview && originalSrc) avatarPreview.src = originalSrc;
    });
  }

  // Сохраняем оригинальный src аватара для отмены
  if (avatarPreview) {
    avatarPreview.dataset.original = avatarPreview.src;
  }

  // Закрытие баннера по Escape
  document.addEventListener('keydown', (e) => {
    if (e.key === 'Escape' && photoBanner?.classList.contains('visible')) {
      cancelBtn?.click();
    }
  });

  /* ─── Показать/скрыть пароль ─── */
  document.querySelectorAll('.profile-form__eye').forEach(btn => {
    btn.addEventListener('click', () => {
      const input = document.getElementById(btn.dataset.target);
      if (!input) return;
      input.type = input.type === 'password' ? 'text' : 'password';
      btn.style.opacity = input.type === 'text' ? '0.8' : '0.4';
    });
  });

  /* ─── Сила пароля ─── */
  const newPasswordInput = document.getElementById('newPassword');
  const strengthFill = document.getElementById('strengthFill');
  const strengthLabel = document.getElementById('strengthLabel');
  const ruleItems = document.querySelectorAll('.password-rules__item');

  const rules = {
    length: { test: v => v.length >= 8 },
    upper: { test: v => /[A-Z]/.test(v) },
    digit: { test: v => /\d/.test(v) },
    special: { test: v => /[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]/.test(v) }
  };

  const strengthLevels = [
    { label: '', color: '', width: '0%' },
    { label: 'Слабый', color: '#ff4d4f', width: '25%' },
    { label: 'Средний', color: '#faad14', width: '50%' },
    { label: 'Хороший', color: '#52c41a', width: '75%' },
    { label: 'Сильный', color: '#389e0d', width: '100%' }
  ];

  if (newPasswordInput) {
    newPasswordInput.addEventListener('input', () => {
      const val = newPasswordInput.value;
      let score = 0;

      ruleItems.forEach(item => {
        const rule = rules[item.dataset.rule];
        if (rule && rule.test(val)) {
          item.classList.add('valid');
          score++;
        } else {
          item.classList.remove('valid');
        }
      });

      const level = val.length === 0 ? 0 : Math.max(1, score);
      const info = strengthLevels[level];

      strengthFill.style.width = info.width;
      strengthFill.style.backgroundColor = info.color;
      strengthLabel.textContent = info.label;
      strengthLabel.style.color = info.color;

      checkMatch();
    });
  }

  /* ─── Совпадение паролей ─── */
  const confirmInput = document.getElementById('confirmPassword');
  const matchHint = document.getElementById('matchHint');

  function checkMatch() {
    if (!confirmInput || !newPasswordInput || !matchHint) return;
    const val = confirmInput.value;
    if (!val) {
      matchHint.textContent = '';
      matchHint.className = 'profile-form__match-hint';
      return;
    }
    if (val === newPasswordInput.value) {
      matchHint.textContent = 'Пароли совпадают';
      matchHint.className = 'profile-form__match-hint match';
    } else {
      matchHint.textContent = 'Пароли не совпадают';
      matchHint.className = 'profile-form__match-hint no-match';
    }
  }

  if (confirmInput) {
    confirmInput.addEventListener('input', checkMatch);
  }

  /* ─── Автоскрытие алертов ─── */
  document.querySelectorAll('.profile-alert').forEach(alert => {
    setTimeout(() => {
      alert.style.transition = 'opacity 0.5s ease';
      alert.style.opacity = '0';
      setTimeout(() => alert.remove(), 500);
    }, 4000);
  });
});