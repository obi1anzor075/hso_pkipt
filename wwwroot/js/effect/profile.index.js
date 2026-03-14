// js/effect/profile.index.js
document.addEventListener('DOMContentLoaded', () => {

  /* ── Данные из Razor ── */
  const events = (window.profileEvents || []).map(e => ({
    ...e,
    date: new Date(e.date)
  }));

  /* ── Константы ── */
  const HOUR_HEIGHT = 60;   // px — высота одного часа (совпадает с --cal-hour-height)
  const TIME_START = 8;    // 08:00 — начало сетки
  const TIME_END = 20;   // 20:00 — конец сетки
  const HOURS = TIME_END - TIME_START;
  const TOTAL_HEIGHT = HOURS * HOUR_HEIGHT;

  const DAYS_SHORT = ['Вс', 'Пн', 'Вт', 'Ср', 'Чт', 'Пт', 'Сб'];
  const MONTHS_RU = ['Январь', 'Февраль', 'Март', 'Апрель', 'Май', 'Июнь',
    'Июль', 'Август', 'Сентябрь', 'Октябрь', 'Ноябрь', 'Декабрь'];
  const MONTHS_GEN = ['января', 'февраля', 'марта', 'апреля', 'мая', 'июня',
    'июля', 'августа', 'сентября', 'октября', 'ноября', 'декабря'];

  /* ── Состояние ── */
  let currentView = 'week';
  let anchorDate = new Date();
  let miniMonth = new Date(anchorDate.getFullYear(), anchorDate.getMonth(), 1);

  /* ── DOM ── */
  const calRange = document.getElementById('calRange');
  const calendarGrid = document.getElementById('calendarGrid');
  const calPrev = document.getElementById('calPrev');
  const calNext = document.getElementById('calNext');
  const miniEl = document.getElementById('calendarMini');
  const upcomingEl = document.getElementById('calendarUpcoming');

  if (!calendarGrid) return;

  /* ─────────────────────────────────────────
     Утилиты
  ───────────────────────────────────────── */
  function addDays(date, n) {
    const d = new Date(date);
    d.setDate(d.getDate() + n);
    return d;
  }

  function isSameDay(a, b) {
    return a.getFullYear() === b.getFullYear() &&
      a.getMonth() === b.getMonth() &&
      a.getDate() === b.getDate();
  }

  function isToday(d) { return isSameDay(d, new Date()); }

  function getWeekStart(date) {
    const d = new Date(date);
    const day = d.getDay();
    const diff = day === 0 ? -6 : 1 - day; // Пн — первый
    d.setDate(d.getDate() + diff);
    d.setHours(0, 0, 0, 0);
    return d;
  }

  function pad2(n) { return String(n).padStart(2, '0'); }

  function formatTime(date) {
    return pad2(date.getHours()) + ':' + pad2(date.getMinutes());
  }

  function eventsOnDay(day) {
    return events.filter(e => isSameDay(e.date, day));
  }

  /* Перевести время события в px от верха сетки */
  function timeToPx(date) {
    const totalMin = (date.getHours() - TIME_START) * 60 + date.getMinutes();
    return (totalMin / 60) * HOUR_HEIGHT;
  }

  /* ─────────────────────────────────────────
     Главный рендер
  ───────────────────────────────────────── */
  function render() {
    renderGrid();
    renderMini();
    renderUpcoming();
  }

  /* ─────────────────────────────────────────
     Сетка
  ───────────────────────────────────────── */
  function renderGrid() {
    calendarGrid.innerHTML = '';

    const days = currentView === 'week' ? getWeekDays() : [new Date(anchorDate)];

    /* ── Заголовок дней ── */
    const head = document.createElement('div');
    head.className = 'cal-head';

    const spacer = document.createElement('div');
    spacer.className = 'cal-head__spacer';
    head.appendChild(spacer);

    days.forEach(day => {
      const cell = document.createElement('div');
      cell.className = 'cal-head__day';

      const nameEl = document.createElement('div');
      nameEl.className = 'cal-head__day-name';
      nameEl.textContent = DAYS_SHORT[day.getDay()];

      const numEl = document.createElement('div');
      numEl.className = 'cal-head__day-num' + (isToday(day) ? ' today' : '');
      numEl.textContent = day.getDate();

      cell.appendChild(nameEl);
      cell.appendChild(numEl);
      head.appendChild(cell);
    });

    calendarGrid.appendChild(head);

    /* ── Тело сетки ── */
    const body = document.createElement('div');
    body.className = 'cal-body';

    /* Колонка времени */
    const timesCol = document.createElement('div');
    timesCol.className = 'cal-body__times';
    timesCol.style.height = TOTAL_HEIGHT + 'px';

    for (let h = TIME_START; h <= TIME_END; h++) {
      if (h === TIME_START) continue; // первую метку не рисуем
      const label = document.createElement('div');
      label.className = 'cal-body__time-label';
      label.textContent = pad2(h) + ':00';
      label.style.top = ((h - TIME_START) * HOUR_HEIGHT) + 'px';
      timesCol.appendChild(label);
    }

    body.appendChild(timesCol);

    /* Колонки дней */
    days.forEach(day => {
      const col = document.createElement('div');
      col.className = 'cal-body__day-col' + (isToday(day) ? ' today-col' : '');
      col.style.height = TOTAL_HEIGHT + 'px';

      /* Линии часов и получасов */
      for (let h = 0; h < HOURS; h++) {
        // Линия часа
        const hourLine = document.createElement('div');
        hourLine.className = 'cal-body__hour-line';
        hourLine.style.top = (h * HOUR_HEIGHT) + 'px';
        col.appendChild(hourLine);

        // Линия получаса
        const halfLine = document.createElement('div');
        halfLine.className = 'cal-body__half-line';
        halfLine.style.top = (h * HOUR_HEIGHT + HOUR_HEIGHT / 2) + 'px';
        col.appendChild(halfLine);
      }
      // Нижняя граница
      const bottomLine = document.createElement('div');
      bottomLine.className = 'cal-body__hour-line';
      bottomLine.style.top = TOTAL_HEIGHT + 'px';
      col.appendChild(bottomLine);

      /* События */
      eventsOnDay(day).forEach(ev => {
        const topPx = timeToPx(ev.date);

        // Событие вне диапазона — пропускаем
        if (topPx < 0 || topPx > TOTAL_HEIGHT) return;

        const evEl = document.createElement('div');
        evEl.className = 'cal-event';
        evEl.style.top = topPx + 'px';
        evEl.style.minHeight = HOUR_HEIGHT * 0.8 + 'px';
        evEl.title = ev.description || ev.title;

        const titleEl = document.createElement('div');
        titleEl.className = 'cal-event__title';
        titleEl.textContent = ev.title;

        const timeEl = document.createElement('div');
        timeEl.className = 'cal-event__time';
        timeEl.textContent = formatTime(ev.date);

        evEl.appendChild(titleEl);
        evEl.appendChild(timeEl);
        col.appendChild(evEl);
      });

      /* Линия текущего времени (только сегодня) */
      if (isToday(day)) {
        const now = new Date();
        const nowPx = timeToPx(now);
        if (nowPx >= 0 && nowPx <= TOTAL_HEIGHT) {
          const nowLine = document.createElement('div');
          nowLine.className = 'cal-now-line';
          nowLine.style.top = nowPx + 'px';
          col.appendChild(nowLine);
        }
      }

      body.appendChild(col);
    });

    calendarGrid.appendChild(body);

    /* Прокрутить к 8:00 или к текущему времени */
    requestAnimationFrame(() => {
      const wrap = calendarGrid.closest('.calendar__grid-wrap');
      if (!wrap) return;
      const now = new Date();
      const scrollH = isToday(anchorDate) || (currentView === 'week' && getWeekDays().some(d => isToday(d)))
        ? Math.max(0, timeToPx(now) - 80)
        : 0;
      wrap.scrollTop = scrollH;
    });

    updateRange(days);
  }

  function getWeekDays() {
    const start = getWeekStart(anchorDate);
    return Array.from({ length: 7 }, (_, i) => addDays(start, i));
  }

  function updateRange(days) {
    if (!calRange) return;
    if (currentView === 'day') {
      const d = days[0];
      calRange.textContent =
        d.getDate() + ' ' + MONTHS_GEN[d.getMonth()] + ' ' + d.getFullYear();
    } else {
      const first = days[0];
      const last = days[days.length - 1];
      if (first.getMonth() === last.getMonth()) {
        calRange.textContent =
          first.getDate() + '–' + last.getDate() + ' ' +
          MONTHS_GEN[first.getMonth()] + ' ' + first.getFullYear();
      } else {
        calRange.textContent =
          first.getDate() + ' ' + MONTHS_GEN[first.getMonth()] + ' – ' +
          last.getDate() + ' ' + MONTHS_GEN[last.getMonth()] + ' ' + last.getFullYear();
      }
    }
  }

  /* ─────────────────────────────────────────
     Мини-календарь
  ───────────────────────────────────────── */
  function renderMini() {
    if (!miniEl) return;
    miniEl.innerHTML = '';

    const year = miniMonth.getFullYear();
    const month = miniMonth.getMonth();

    /* Заголовок */
    const header = document.createElement('div');
    header.className = 'cal-mini__header';

    const prevBtn = document.createElement('button');
    prevBtn.className = 'cal-mini__nav';
    prevBtn.textContent = '‹';
    prevBtn.addEventListener('click', () => {
      miniMonth = new Date(year, month - 1, 1);
      renderMini();
    });

    const monthLabel = document.createElement('span');
    monthLabel.className = 'cal-mini__month';
    monthLabel.textContent = MONTHS_RU[month];

    const nextBtn = document.createElement('button');
    nextBtn.className = 'cal-mini__nav';
    nextBtn.textContent = '›';
    nextBtn.addEventListener('click', () => {
      miniMonth = new Date(year, month + 1, 1);
      renderMini();
    });

    header.appendChild(prevBtn);
    header.appendChild(monthLabel);
    header.appendChild(nextBtn);
    miniEl.appendChild(header);

    /* Сетка */
    const grid = document.createElement('div');
    grid.className = 'cal-mini__grid';

    ['Пн', 'Вт', 'Ср', 'Чт', 'Пт', 'Сб', 'Вс'].forEach(d => {
      const dow = document.createElement('div');
      dow.className = 'cal-mini__dow';
      dow.textContent = d;
      grid.appendChild(dow);
    });

    /* Отступ первого дня: Пн=0..Вс=6 */
    const firstDay = new Date(year, month, 1);
    let startOffset = firstDay.getDay() - 1;
    if (startOffset < 0) startOffset = 6;

    const prevMonthDays = new Date(year, month, 0).getDate();
    for (let i = startOffset - 1; i >= 0; i--) {
      appendMiniDay(grid, new Date(year, month - 1, prevMonthDays - i), true);
    }

    const daysInMonth = new Date(year, month + 1, 0).getDate();
    for (let d = 1; d <= daysInMonth; d++) {
      appendMiniDay(grid, new Date(year, month, d), false);
    }

    const totalCells = Math.ceil((startOffset + daysInMonth) / 7) * 7;
    let nextDay = 1;
    for (let i = startOffset + daysInMonth; i < totalCells; i++) {
      appendMiniDay(grid, new Date(year, month + 1, nextDay++), true);
    }

    miniEl.appendChild(grid);
  }

  function appendMiniDay(grid, date, otherMonth) {
    const el = document.createElement('div');
    el.className = 'cal-mini__day';
    if (otherMonth) el.classList.add('other-month');
    if (isToday(date)) el.classList.add('today');
    if (isSameDay(date, anchorDate)) el.classList.add('selected');

    if (eventsOnDay(date).length > 0) {
      const now = new Date();
      const in7days = addDays(now, 7);
      const in28days = addDays(now, 28);

      if (date >= now && date <= in7days) {
        el.classList.add('has-event-week');
      } else if (date > in7days && date <= in28days) {
        el.classList.add('has-event-month');
      }
    }

    el.textContent = date.getDate();
    el.addEventListener('click', () => {
      anchorDate = new Date(date);
      currentView = 'day';
      document.querySelectorAll('.calendar__view-btn').forEach(b =>
        b.classList.toggle('active', b.dataset.view === 'day')
      );
      render();
    });
    grid.appendChild(el);
  }

  /* ─────────────────────────────────────────
     Блок ближайших событий
  ───────────────────────────────────────── */
  function renderUpcoming() {
    if (!upcomingEl) return;
    upcomingEl.innerHTML = '';

    const now = new Date();
    const todayStart = new Date(now.getFullYear(), now.getMonth(), now.getDate());
    const todayEnd = new Date(now.getFullYear(), now.getMonth(), now.getDate(), 23, 59, 59);
    const tomorrowStart = addDays(todayStart, 1);
    const tomorrowEnd = addDays(todayEnd, 1);
    const weekEnd = addDays(todayStart, 7);
    const monthEnd = addDays(todayStart, 28);

    const todayList = events.filter(e => e.date >= now && e.date <= todayEnd);
    const tomorrowList = events.filter(e => e.date >= tomorrowStart && e.date <= tomorrowEnd);
    const weekList = events.filter(e => e.date > tomorrowEnd && e.date <= weekEnd);
    const monthList = events.filter(e => e.date > weekEnd && e.date <= monthEnd);

    const hasAny = todayList.length || tomorrowList.length || weekList.length || monthList.length;

    if (!hasAny) {
      const empty = document.createElement('div');
      empty.className = 'cal-upcoming__empty';
      empty.textContent = 'Ближайших событий нет';
      upcomingEl.appendChild(empty);
      return;
    }

    function addGroup(label, list, dotColor) {
      if (!list.length) return;

      const lbl = document.createElement('div');
      lbl.className = 'cal-upcoming__label';
      lbl.textContent = label;
      upcomingEl.appendChild(lbl);

      list.forEach(ev => {
        const item = document.createElement('div');
        item.className = 'cal-upcoming__event';

        const dot = document.createElement('div');
        dot.className = 'cal-upcoming__dot';
        dot.style.backgroundColor = dotColor;

        const text = document.createElement('div');
        text.className = 'cal-upcoming__text';

        const row = document.createElement('div');
        row.className = 'cal-upcoming__row';

        const title = document.createElement('span');
        title.className = 'cal-upcoming__title';
        title.textContent = ev.title;

        const sep = document.createElement('span');
        sep.className = 'cal-upcoming__sep';
        sep.textContent = '|';

        const time = document.createElement('span');
        time.className = 'cal-upcoming__time';
        time.textContent = formatTime(ev.date);

        row.appendChild(title);
        row.appendChild(sep);
        row.appendChild(time);
        text.appendChild(row);
        item.appendChild(dot);
        item.appendChild(text);
        upcomingEl.appendChild(item);
      });
    }

    addGroup('Сегодня', todayList, 'var(--accent-color)');
    addGroup('Завтра', tomorrowList, 'var(--role-color)');
    addGroup('На неделе', weekList, 'var(--second-color-hover)');
    addGroup('В этом месяце', monthList, 'var(--second-color)');
  }

  /* ─────────────────────────────────────────
     Навигация
  ───────────────────────────────────────── */
  calPrev.addEventListener('click', () => {
    anchorDate = addDays(anchorDate, currentView === 'week' ? -7 : -1);
    render();
  });

  calNext.addEventListener('click', () => {
    anchorDate = addDays(anchorDate, currentView === 'week' ? 7 : 1);
    render();
  });

  document.querySelectorAll('.calendar__view-btn').forEach(btn => {
    btn.addEventListener('click', () => {
      document.querySelectorAll('.calendar__view-btn')
        .forEach(b => b.classList.remove('active'));
      btn.classList.add('active');
      currentView = btn.dataset.view;
      render();
    });
  });

  /* ── Старт ── */
  render();
});