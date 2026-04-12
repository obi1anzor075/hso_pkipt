// js/app/merch.index.js
document.addEventListener('DOMContentLoaded', () => {

    // Enter в строке поиска отправляет форму
    const searchInput = document.querySelector('.merch-search__input');
    if (searchInput) {
        searchInput.addEventListener('keydown', e => {
            if (e.key === 'Enter') {
                e.preventDefault();
                searchInput.closest('form')?.submit();
            }
        });

        // Автофокус при наличии поискового запроса
        if (searchInput.value) {
            searchInput.focus();
            searchInput.setSelectionRange(
                searchInput.value.length,
                searchInput.value.length
            );
        }
    }

});