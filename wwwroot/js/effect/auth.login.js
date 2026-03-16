// effect/auth.login.js
document.addEventListener('DOMContentLoaded', () => {

  document.querySelectorAll('[data-fn="visible__password"]').forEach(btn => {
    btn.addEventListener('click', () => {
      const input = btn.closest('div').querySelector('[data-inc="password"]');
      if (!input) return;

      const isHidden = input.type === 'password';
      input.type = isHidden ? 'text' : 'password';

      btn.classList.toggle('close', !isHidden);
      btn.classList.toggle('open', isHidden);
    });
  });

});