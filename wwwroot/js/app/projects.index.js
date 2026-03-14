// app\projects.index.js
document.addEventListener('DOMContentLoaded', () => {
    const projectsGrid = document.getElementById('projectsGrid');
    const btnLoadMore = document.getElementById('btnLoadMore');
    const loadMoreContainer = document.getElementById('loadMoreContainer');
    const loadingSpinner = document.getElementById('loadingSpinner');

    const ITEMS_PER_PAGE = 8;
    let currentPage = window.projectsPage?.currentPage ?? 1;
    const totalPages = window.projectsPage?.totalPages ?? 1;
    let isLoading = false;

    function init() {
        const cards = document.querySelectorAll('.card-project');

        if (cards.length < ITEMS_PER_PAGE || currentPage >= totalPages) {
            if (loadMoreContainer) {
                loadMoreContainer.classList.add('hidden');
            }
        }

        addCardClickHandlers();
    }

    async function loadMoreProject() {
        if (isLoading) return;

        isLoading = true;
        currentPage++;

        if (btnLoadMore) btnLoadMore.disabled = true;
        if (loadingSpinner) loadingSpinner.classList.remove('hidden');

        try {
            const response = await fetch(`/Projects/LoadMoreProjects?page=${currentPage}&pageSize=${ITEMS_PER_PAGE}`, {
                method: 'GET',
                headers: { 'X-Requested-With': 'XMLHttpRequest' }
            });

            if (!response.ok) throw new Error('Ошибка загрузки проектов');

            const html = await response.text();
            const tempDiv = document.createElement('div');
            tempDiv.innerHTML = html;

            const newCards = tempDiv.querySelectorAll('.card-project');

            if (newCards.length === 0) {
                if (loadMoreContainer) loadMoreContainer.classList.add('hidden');
                return;
            }

            newCards.forEach((card, index) => {
                setTimeout(() => {
                    card.style.opacity = '0';
                    card.style.transform = 'translateY(20px)';
                    projectsGrid.appendChild(card);

                    setTimeout(() => {
                        card.style.transition = 'opacity 0.3s ease, transform 0.3s ease';
                        card.style.opacity = '1';
                        card.style.transform = 'translateY(0)';
                    }, 50);
                }, index * 50);
            });

            if (newCards.length < ITEMS_PER_PAGE || currentPage >= totalPages) {
                setTimeout(() => {
                    if (loadMoreContainer) loadMoreContainer.classList.add('hidden');
                }, newCards.length * 50 + 500);
            }

            setTimeout(() => addCardClickHandlers(), newCards.length * 50 + 100);

            const firstNewCard = newCards[0];
            if (firstNewCard) {
                setTimeout(() => {
                    const actualCard = Array.from(projectsGrid.children).find(
                        child => child.getAttribute('data-project-id') === firstNewCard.getAttribute('data-project-id')
                    );
                    if (actualCard) {
                        actualCard.scrollIntoView({ behavior: 'smooth', block: 'nearest' });
                    }
                }, 200);
            }

        } catch (error) {
            console.error('Ошибка при загрузке проектов:', error);

            if (btnLoadMore) {
                const originalText = btnLoadMore.textContent.trim();
                btnLoadMore.textContent = 'Ошибка загрузки. Попробуйте ещё раз';
                btnLoadMore.style.backgroundColor = 'var(--error-color)';

                setTimeout(() => {
                    btnLoadMore.textContent = originalText;
                    btnLoadMore.style.backgroundColor = '';
                }, 3000);
            }

            currentPage--;
        } finally {
            if (btnLoadMore) btnLoadMore.disabled = false;
            if (loadingSpinner) loadingSpinner.classList.add('hidden');
            isLoading = false;
        }
    }

    function addCardClickHandlers() {
        document.querySelectorAll('.card-project').forEach(card => {
            if (card.dataset.clickHandlerAdded) return;

            card.dataset.clickHandlerAdded = 'true';
            card.style.cursor = 'pointer';

            card.addEventListener('click', function () {
                if (this.classList.contains('not-load')) return;

                const projectId = this.getAttribute('data-project-id');
                if (projectId) {
                    window.location.href = `/Projects/ProjectDetails/${projectId}`;
                }
            });
        });
    }

    if (btnLoadMore) btnLoadMore.addEventListener('click', loadMoreProject);

    init();
});