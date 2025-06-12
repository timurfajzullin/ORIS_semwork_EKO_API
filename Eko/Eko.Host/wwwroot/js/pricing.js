document.addEventListener('DOMContentLoaded', function() {
    // Функция для получения значения cookie по имени
    function getCookie(name) {
        const cookies = document.cookie.split(';');
        for (let cookie of cookies) {
            const [cookieName, cookieValue] = cookie.trim().split('=');
            if (cookieName === name) {
                return cookieValue;
            }
        }
        return null;
    }

    // Проверяем наличие токена и получаем план
    const hasToken = getCookie('jwt') || getCookie('token') || getCookie('access_token');
    const userPlan = parseInt(getCookie('plan')) || 1; // По умолчанию Basic (1)

    // Находим все элементы с классом 'bd-btn'
    const buttons = document.querySelectorAll('.bd-btn');

    // Функция для блокировки кнопки
    function disableButton(button, text = 'Already subscribed') {
        button.textContent = text;
        button.setAttribute('disabled', 'disabled');
        button.style.opacity = '0.6';
        button.style.cursor = 'not-allowed';
        button.style.pointerEvents = 'none';
        button.removeAttribute('href');
        button.classList.remove('btn-gradient', 'btn-outline-primary');
        button.classList.add('btn-secondary');
    }

    // Перебираем найденные кнопки
    buttons.forEach((button, index) => {
        if (!hasToken) {
            // Если токена нет - меняем ВСЕ кнопки на ссылку Auth/SignIn
            button.setAttribute('href', '/Auth/SignIn');
            if (button.textContent.trim() === 'Enjoy now') {
                button.textContent = 'Sign In to Enjoy';
            }
            return;
        }

        // Если токен есть - проверяем план и блокируем соответствующие кнопки
        // Индекс 0 - Enjoy now (Basic)
        // Индекс 1 - Get started (Pro)
        // Индекс 2 - Get started (Business)

        // Блокируем кнопку "Enjoy now" если есть токен
        if (index === 0) {
            disableButton(button, 'Already subscribed');
            return;
        }

        // Блокируем кнопки в зависимости от плана
        if (index === 1 && userPlan >= 2) { // Pro кнопка
            disableButton(button, 'Already Pro');
        } else if (index === 2 && userPlan >= 3) { // Business кнопка
            disableButton(button, 'Already Business');
        }
    });
});