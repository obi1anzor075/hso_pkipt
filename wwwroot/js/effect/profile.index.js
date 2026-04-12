// js/effect/profile.index.js
document.addEventListener('DOMContentLoaded', () => {

    /* ══════════ Аккордеон заказов ══════════ */
    document.querySelectorAll('[data-order-toggle]').forEach(head => {
        head.addEventListener('click', () => {
            const body = head.nextElementSibling;
            const toggle = head.querySelector('.order-card__toggle svg');
            if (!body) return;
            const isOpen = !body.classList.contains('collapsed');
            body.classList.toggle('collapsed', isOpen);
            if (toggle) toggle.style.transform = isOpen ? '' : 'rotate(180deg)';
        });
    });

    /* ══════════ Данные ══════════ */
    function parseLocalDate(str) {
        const [datePart, timePart = '00:00:00'] = str.split('T');
        const [y, mo, d] = datePart.split('-').map(Number);
        const [h, mi, s] = timePart.split(':').map(Number);
        return new Date(y, mo - 1, d, h, mi, s);
    }

    const events = (window.profileEvents || []).map(e => ({
        ...e,
        date: parseLocalDate(e.date)
    }));

    /* ══════════ Константы ══════════ */
    /* ══════════ Константы ══════════ */
    const HOUR_HEIGHT = 52;
    const TIME_START = 0;
    const TIME_END = 24;
    const HOURS = TIME_END - TIME_START;
    const TOTAL_HEIGHT = HOURS * HOUR_HEIGHT;

    const DAYS_SHORT = ['Вс', 'Пн', 'Вт', 'Ср', 'Чт', 'Пт', 'Сб'];
    const MONTHS_RU = ['Январь', 'Февраль', 'Март', 'Апрель', 'Май', 'Июнь', 'Июль', 'Август', 'Сентябрь', 'Октябрь', 'Ноябрь', 'Декабрь'];
    const MONTHS_GEN = ['января', 'февраля', 'марта', 'апреля', 'мая', 'июня', 'июля', 'августа', 'сентября', 'октября', 'ноября', 'декабря'];

    /* ══════════ Состояние ══════════ */
    let currentView = 'week';
    let anchorDate = new Date();
    let miniMonth = new Date(anchorDate.getFullYear(), anchorDate.getMonth(), 1);

    /* ══════════ DOM ══════════ */
    const calRange = document.getElementById('calRange');
    const calGrid = document.getElementById('calendarGrid');
    const calPrev = document.getElementById('calPrev');
    const calNext = document.getElementById('calNext');
    const miniEl = document.getElementById('calendarMini');
    const upcomingEl = document.getElementById('calendarUpcoming');

    if (!calGrid) return;

    /* ══════════ Утилиты ══════════ */
    function addDays(date, n) {
        const d = new Date(date);
        d.setDate(d.getDate() + n);
        return d;
    }

    function isSameDay(a, b) {
        return a.getFullYear() === b.getFullYear()
            && a.getMonth() === b.getMonth()
            && a.getDate() === b.getDate();
    }

    function isToday(d) { return isSameDay(d, new Date()); }
    function pad2(n) { return String(n).padStart(2, '0'); }
    function formatTime(d) { return pad2(d.getHours()) + ':' + pad2(d.getMinutes()); }

    function formatDateFull(d) {
        return d.getDate() + ' ' + MONTHS_GEN[d.getMonth()] + ' ' + d.getFullYear() + ', ' + formatTime(d);
    }

    function eventsOnDay(day) { return events.filter(e => isSameDay(e.date, day)); }

    function timeToPx(d) {
        return ((d.getHours() - TIME_START) * 60 + d.getMinutes()) / 60 * HOUR_HEIGHT;
    }

    function isNightHour(hour) {
        return hour < 6 || hour >= 22;
    }

    function getFirstEventTop(days) {
        const dayEvents = days
            .flatMap(day => eventsOnDay(day))
            .sort((a, b) => a.date - b.date);

        if (!dayEvents.length) return 0;

        return Math.max(0, timeToPx(dayEvents[0].date) - 80);
    }

    function getWeekStart(date) {
        const d = new Date(date);
        const day = d.getDay();
        d.setDate(d.getDate() + (day === 0 ? -6 : 1 - day));
        d.setHours(0, 0, 0, 0);
        return d;
    }

    function getWeekDays() {
        const s = getWeekStart(anchorDate);
        return Array.from({ length: 7 }, (_, i) => addDays(s, i));
    }

    /* ══════════ Модалка события ══════════ */
    let eventModal = null;

    function openEventModal(ev) {
        closeEventModal();
        const overlay = document.createElement('div');
        overlay.className = 'cal-event-modal-overlay';
        overlay.innerHTML = `
      <div class="cal-event-modal" role="dialog" aria-modal="true">
        <div class="cal-event-modal__stripe"></div>
        <div class="cal-event-modal__body">
          <button class="cal-event-modal__close" type="button" aria-label="Закрыть">×</button>
          <div class="cal-event-modal__header">
            <div class="cal-event-modal__icon">
              <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                <rect x="3" y="4" width="18" height="18" rx="2" ry="2"/>
                <line x1="16" y1="2" x2="16" y2="6"/>
                <line x1="8" y1="2" x2="8" y2="6"/>
                <line x1="3" y1="10" x2="21" y2="10"/>
              </svg>
            </div>
            <div style="min-width:0">
              <p class="cal-event-modal__title">${ev.title}</p>
              <p class="cal-event-modal__date">${formatDateFull(ev.date)}</p>
            </div>
          </div>
          ${ev.description && ev.description !== ev.title
                ? `<p class="cal-event-modal__desc">${ev.description}</p>`
                : ''}
        </div>
      </div>`;

        function close() { closeEventModal(); }
        overlay.querySelector('.cal-event-modal__close').addEventListener('click', close);
        overlay.addEventListener('click', e => { if (e.target === overlay) close(); });

        function escH(e) { if (e.key === 'Escape') { close(); document.removeEventListener('keydown', escH); } }
        document.addEventListener('keydown', escH);

        document.body.appendChild(overlay);
        requestAnimationFrame(() => overlay.classList.add('is-active'));
        document.body.style.overflow = 'hidden';
        eventModal = overlay;
    }

    function closeEventModal() {
        if (!eventModal) return;
        const m = eventModal;
        eventModal = null;
        m.classList.remove('is-active');
        m.classList.add('is-closing');
        setTimeout(() => { if (m.parentNode) m.remove(); document.body.style.overflow = ''; }, 260);
    }

    /* ══════════ Рендер ══════════ */
    function render() { renderGrid(); renderMini(); renderUpcoming(); }

    function renderGrid() {
        calGrid.innerHTML = '';
        const days = currentView === 'week' ? getWeekDays() : [new Date(anchorDate)];

        const head = document.createElement('div');
        head.className = 'cal-head';

        const sp = document.createElement('div');
        sp.className = 'cal-head__spacer';
        head.appendChild(sp);

        days.forEach(day => {
            const cell = document.createElement('div');
            cell.className = 'cal-head__day';

            const ne = document.createElement('div');
            ne.className = 'cal-head__day-name';
            ne.textContent = DAYS_SHORT[day.getDay()];

            const nu = document.createElement('div');
            nu.className = 'cal-head__day-num' + (isToday(day) ? ' today' : '');
            nu.textContent = day.getDate();

            cell.appendChild(ne);
            cell.appendChild(nu);
            head.appendChild(cell);
        });

        calGrid.appendChild(head);

        const body = document.createElement('div');
        body.className = 'cal-body';

        const tc = document.createElement('div');
        tc.className = 'cal-body__times';
        tc.style.height = TOTAL_HEIGHT + 'px';

        for (let h = TIME_START; h <= TIME_END; h++) {
            const lbl = document.createElement('div');
            lbl.className = 'cal-body__time-label';
            lbl.textContent = `${pad2(h % 24)}:00`;
            lbl.style.top = (h * HOUR_HEIGHT) + 'px';
            tc.appendChild(lbl);
        }

        body.appendChild(tc);

        days.forEach(day => {
            const col = document.createElement('div');
            col.className = 'cal-body__day-col' + (isToday(day) ? ' today-col' : '');
            col.style.height = TOTAL_HEIGHT + 'px';

            for (let h = 0; h < HOURS; h++) {
                if (isNightHour(h)) {
                    const night = document.createElement('div');
                    night.className = 'cal-body__night-block';
                    night.style.top = (h * HOUR_HEIGHT) + 'px';
                    night.style.height = HOUR_HEIGHT + 'px';
                    col.appendChild(night);
                }

                const hl = document.createElement('div');
                hl.className = 'cal-body__hour-line';
                hl.style.top = (h * HOUR_HEIGHT) + 'px';
                col.appendChild(hl);

                const hf = document.createElement('div');
                hf.className = 'cal-body__half-line';
                hf.style.top = (h * HOUR_HEIGHT + HOUR_HEIGHT / 2) + 'px';
                col.appendChild(hf);
            }

            const bl = document.createElement('div');
            bl.className = 'cal-body__hour-line';
            bl.style.top = TOTAL_HEIGHT + 'px';
            col.appendChild(bl);

            eventsOnDay(day).forEach(ev => {
                const topPx = timeToPx(ev.date);
                const top = Math.max(0, Math.min(topPx, TOTAL_HEIGHT - 28));

                const evEl = document.createElement('div');
                evEl.className = 'cal-event';
                evEl.style.top = top + 'px';
                evEl.style.minHeight = '42px';
                evEl.style.cursor = 'pointer';
                evEl.title = ev.description || ev.title;

                const tEl = document.createElement('div');
                tEl.className = 'cal-event__title';
                tEl.textContent = ev.title;

                const tmEl = document.createElement('div');
                tmEl.className = 'cal-event__time';
                tmEl.textContent = formatTime(ev.date);

                evEl.appendChild(tEl);
                evEl.appendChild(tmEl);
                evEl.addEventListener('click', e => {
                    e.stopPropagation();
                    openEventModal(ev);
                });

                col.appendChild(evEl);
            });

            if (isToday(day)) {
                const nowPx = timeToPx(new Date());
                if (nowPx >= 0 && nowPx <= TOTAL_HEIGHT) {
                    const nl = document.createElement('div');
                    nl.className = 'cal-now-line';
                    nl.style.top = nowPx + 'px';
                    col.appendChild(nl);

                    const nd = document.createElement('div');
                    nd.className = 'cal-now-dot';
                    nd.style.top = nowPx + 'px';
                    col.appendChild(nd);
                }
            }

            body.appendChild(col);
        });

        calGrid.appendChild(body);

        requestAnimationFrame(() => {
            const wrap = calGrid.closest('.calendar__grid-wrap');
            if (!wrap) return;

            const hasToday = getWeekDays().some(d => isToday(d)) || isToday(anchorDate);

            wrap.scrollTop = hasToday
                ? Math.max(0, timeToPx(new Date()) - 140)
                : getFirstEventTop(days);
        });

        updateRange(days);
    }

    function updateRange(days) {
        if (!calRange) return;
        if (currentView === 'day') {
            const d = days[0];
            calRange.textContent = d.getDate() + ' ' + MONTHS_GEN[d.getMonth()] + ' ' + d.getFullYear();
        } else {
            const f = days[0], l = days[days.length - 1];
            calRange.textContent = f.getMonth() === l.getMonth()
                ? f.getDate() + '–' + l.getDate() + ' ' + MONTHS_GEN[f.getMonth()] + ' ' + f.getFullYear()
                : f.getDate() + ' ' + MONTHS_GEN[f.getMonth()] + ' – ' + l.getDate() + ' ' + MONTHS_GEN[l.getMonth()] + ' ' + l.getFullYear();
        }
    }

    function renderMini() {
        if (!miniEl) return;
        miniEl.innerHTML = '';
        const y = miniMonth.getFullYear(), m = miniMonth.getMonth();

        const hd = document.createElement('div');
        hd.className = 'cal-mini__header';

        const pb = document.createElement('button');
        pb.className = 'cal-mini__nav'; pb.textContent = '‹';
        pb.addEventListener('click', () => { miniMonth = new Date(y, m - 1, 1); renderMini(); });

        const ml = document.createElement('span');
        ml.className = 'cal-mini__month';
        ml.textContent = MONTHS_RU[m] + ' ' + y;

        const nb = document.createElement('button');
        nb.className = 'cal-mini__nav'; nb.textContent = '›';
        nb.addEventListener('click', () => { miniMonth = new Date(y, m + 1, 1); renderMini(); });

        hd.appendChild(pb); hd.appendChild(ml); hd.appendChild(nb);
        miniEl.appendChild(hd);

        const g = document.createElement('div');
        g.className = 'cal-mini__grid';
        ['Пн', 'Вт', 'Ср', 'Чт', 'Пт', 'Сб', 'Вс'].forEach(d => {
            const el = document.createElement('div');
            el.className = 'cal-mini__dow';
            el.textContent = d;
            g.appendChild(el);
        });

        const fd = new Date(y, m, 1);
        let off = fd.getDay() - 1; if (off < 0) off = 6;
        const prev = new Date(y, m, 0).getDate();
        for (let i = off - 1; i >= 0; i--) appendMiniDay(g, new Date(y, m - 1, prev - i), true);

        const dim = new Date(y, m + 1, 0).getDate();
        for (let d = 1; d <= dim; d++) appendMiniDay(g, new Date(y, m, d), false);

        const total = Math.ceil((off + dim) / 7) * 7;
        let nd = 1;
        for (let i = off + dim; i < total; i++) appendMiniDay(g, new Date(y, m + 1, nd++), true);

        miniEl.appendChild(g);
    }

    function appendMiniDay(grid, date, other) {
        const el = document.createElement('div');
        el.className = 'cal-mini__day';
        if (other) el.classList.add('other-month');
        if (isToday(date)) el.classList.add('today');
        if (isSameDay(date, anchorDate)) el.classList.add('selected');

        if (eventsOnDay(date).length > 0) {
            const now = new Date();
            const i7 = addDays(now, 7);
            const i28 = addDays(now, 28);
            if (date >= now && date <= i7) el.classList.add('has-event-week');
            else if (date > i7 && date <= i28) el.classList.add('has-event-month');
        }

        el.textContent = date.getDate();
        el.addEventListener('click', () => {
            anchorDate = new Date(date.getFullYear(), date.getMonth(), date.getDate());
            currentView = 'day';
            document.querySelectorAll('.calendar__view-btn').forEach(b =>
                b.classList.toggle('active', b.dataset.view === 'day')
            );
            render();
        });
        grid.appendChild(el);
    }

    function renderUpcoming() {
        if (!upcomingEl) return;
        upcomingEl.innerHTML = '';

        const now = new Date();
        const td = new Date(now.getFullYear(), now.getMonth(), now.getDate(), 23, 59, 59);
        const tmS = addDays(new Date(now.getFullYear(), now.getMonth(), now.getDate()), 1);
        const tmE = new Date(tmS.getFullYear(), tmS.getMonth(), tmS.getDate(), 23, 59, 59);
        const wE = addDays(new Date(now.getFullYear(), now.getMonth(), now.getDate()), 7);
        const mE = addDays(new Date(now.getFullYear(), now.getMonth(), now.getDate()), 28);

        const tL = events.filter(e => e.date >= now && e.date <= td);
        const tmL = events.filter(e => e.date >= tmS && e.date <= tmE);
        const wL = events.filter(e => e.date > tmE && e.date <= wE);
        const mL = events.filter(e => e.date > wE && e.date <= mE);

        if (!tL.length && !tmL.length && !wL.length && !mL.length) {
            const emp = document.createElement('div');
            emp.className = 'cal-upcoming__empty';
            emp.textContent = 'Ближайших событий нет';
            upcomingEl.appendChild(emp);
            return;
        }

        function addGroup(label, list, color) {
            if (!list.length) return;
            const lbl = document.createElement('div');
            lbl.className = 'cal-upcoming__label';
            lbl.textContent = label;
            upcomingEl.appendChild(lbl);

            list.forEach(ev => {
                const item = document.createElement('div');
                item.className = 'cal-upcoming__event';
                item.style.cursor = 'pointer';

                const dot = document.createElement('div');
                dot.className = 'cal-upcoming__dot';
                dot.style.backgroundColor = color;

                const text = document.createElement('div');
                text.className = 'cal-upcoming__text';

                const row = document.createElement('div');
                row.className = 'cal-upcoming__row';

                const tit = document.createElement('span');
                tit.className = 'cal-upcoming__title';
                tit.textContent = ev.title;

                const sep = document.createElement('span');
                sep.className = 'cal-upcoming__sep';
                sep.textContent = '|';

                const tm = document.createElement('span');
                tm.className = 'cal-upcoming__time';
                tm.textContent = formatTime(ev.date);

                row.appendChild(tit); row.appendChild(sep); row.appendChild(tm);
                text.appendChild(row);
                item.appendChild(dot); item.appendChild(text);
                item.addEventListener('click', () => openEventModal(ev));
                upcomingEl.appendChild(item);
            });
        }

        addGroup('Сегодня', tL, 'var(--accent-color)');
        addGroup('Завтра', tmL, 'var(--role-color)');
        addGroup('На неделе', wL, 'var(--second-color-hover)');
        addGroup('В этом месяце', mL, 'var(--second-color)');
    }

    /* ══════════ Навигация ══════════ */
    calPrev.addEventListener('click', () => { anchorDate = addDays(anchorDate, currentView === 'week' ? -7 : -1); render(); });
    calNext.addEventListener('click', () => { anchorDate = addDays(anchorDate, currentView === 'week' ? 7 : 1); render(); });

    document.querySelectorAll('.calendar__view-btn').forEach(btn => {
        btn.addEventListener('click', () => {
            document.querySelectorAll('.calendar__view-btn').forEach(b => b.classList.remove('active'));
            btn.classList.add('active');
            currentView = btn.dataset.view;
            render();
        });
    });

    render();
});

window.addEventListener('resize', () => {
    render();
});