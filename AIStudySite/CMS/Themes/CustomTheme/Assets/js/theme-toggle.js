(function () {
    const root = document.documentElement;
    const prefersDark = window.matchMedia('(prefers-color-scheme: dark)').matches;
    const stored = localStorage.getItem('ai-theme');
    const initial = stored || (prefersDark ? 'dark' : 'light');
    root.setAttribute('data-theme', initial);

    document.querySelectorAll('.theme-toggle').forEach(btn => {
        btn.addEventListener('click', () => {
            const current = root.getAttribute('data-theme');
            const next = current === 'dark' ? 'light' : 'dark';
            root.setAttribute('data-theme', next);
            localStorage.setItem('ai-theme', next);
            btn.textContent = next === 'dark' ? '🌞' : '🌙';
        });
    });
})();
