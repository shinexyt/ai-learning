(function () {
    const storageKey = 'ai-theme';
    const prefersDark = window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches;

    function applyTheme(theme) {
        document.documentElement.setAttribute('data-theme', theme);
        document.body?.setAttribute('data-theme', theme);
    }

    window.aiTheme = {
        get: function () {
            const stored = window.localStorage.getItem(storageKey);
            const theme = stored || (prefersDark ? 'dark' : 'light');
            applyTheme(theme);
            return theme;
        },
        set: function (theme) {
            window.localStorage.setItem(storageKey, theme);
            applyTheme(theme);
        }
    };

    window.addEventListener('DOMContentLoaded', () => {
        window.aiTheme.get();
    });
})();
