(function () {
    'use strict';

    const page = document.querySelector('.docs-page');
    if (!page) return;

    const slug = page.dataset.docSlug || '';
    if (!slug) return;

    const target = document.getElementById('docsRenderedContent');
    const toc = document.getElementById('docsToc');
    const docsMap = window.documentsMap || {};
    const docItem = docsMap[slug];

    const sidebar = document.getElementById('docsSidebar');
    const overlay = document.getElementById('docsOverlay');
    const openBtn = document.getElementById('docsSidebarToggle');
    const closeBtn = document.getElementById('docsSidebarClose');

    if (!docItem || !target) return;

    function escapeHtml(text) {
        return text
            .replace(/&/g, '&amp;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;');
    }

    function parseInline(text) {
        let parsed = escapeHtml(text);

        parsed = parsed.replace(/\?\[(https?:\/\/[^\]]+)\]\(([^)]+)\)/g, '<a class="docs-link" href="$1" target="_blank" rel="noopener noreferrer">$2</a>');
        parsed = parsed.replace(/\*\*(.*?)\*\*/g, '<b>$1</b>');
        parsed = parsed.replace(/\*(.*?)\*/g, '<i>$1</i>');
        parsed = parsed.replace(/\[(.*?)\]/g, '<strong class="docs-mark">$1</strong>');

        return parsed;
    }

    function slugify(text) {
        return text
            .toLowerCase()
            .trim()
            .replace(/[^\p{L}\p{N}\s-]/gu, '')
            .replace(/\s+/g, '-');
    }

    function renderMarkdown(raw) {
        const lines = raw.split('\n');
        let html = '';
        let inList = false;
        const headings = [];

        for (let i = 0; i < lines.length; i++) {
            const line = lines[i].trim();

            if (!line) {
                if (inList) {
                    html += '</ul>';
                    inList = false;
                }
                continue;
            }

            if (line.startsWith('##')) {
                if (inList) {
                    html += '</ul>';
                    inList = false;
                }

                const title = line.replace(/^##\s*/, '').trim();
                const id = slugify(title);
                headings.push({ id, title });
                html += `<h2 id="${id}">${parseInline(title)}</h2>`;
                continue;
            }

            if (line.startsWith('* ')) {
                if (!inList) {
                    html += '<ul>';
                    inList = true;
                }

                html += `<li>${parseInline(line.substring(2).trim())}</li>`;
                continue;
            }

            if (inList) {
                html += '</ul>';
                inList = false;
            }

            html += `<p>${parseInline(line)}</p>`;
        }

        if (inList) {
            html += '</ul>';
        }

        return { html, headings };
    }

    const { html, headings } = renderMarkdown(docItem.content || '');
    target.innerHTML = html;

    if (toc && headings.length) {
        toc.innerHTML = headings
            .map(h => `<a href="#${h.id}" data-toc-link="${h.id}">${h.title}</a>`)
            .join('');
    }

    function closeSidebar() {
        sidebar?.classList.remove('open');
        overlay?.classList.add('hidden');
        document.body.style.overflow = '';
    }

    function openSidebar() {
        sidebar?.classList.add('open');
        overlay?.classList.remove('hidden');
        document.body.style.overflow = 'hidden';
    }

    openBtn?.addEventListener('click', openSidebar);
    closeBtn?.addEventListener('click', closeSidebar);
    overlay?.addEventListener('click', closeSidebar);

    document.querySelectorAll('#docsToc a').forEach(link => {
        link.addEventListener('click', () => {
            if (window.innerWidth <= 900) {
                closeSidebar();
            }
        });
    });

    const headingEls = Array.from(target.querySelectorAll('h2[id]'));
    const tocLinks = Array.from(document.querySelectorAll('[data-toc-link]'));

    if (headingEls.length && tocLinks.length) {
        const observer = new IntersectionObserver((entries) => {
            entries.forEach(entry => {
                if (!entry.isIntersecting) return;

                const id = entry.target.id;
                tocLinks.forEach(link => {
                    link.classList.toggle('active', link.getAttribute('data-toc-link') === id);
                });
            });
        }, {
            rootMargin: '-20% 0px -65% 0px',
            threshold: 0
        });

        headingEls.forEach(h => observer.observe(h));
    }
})();