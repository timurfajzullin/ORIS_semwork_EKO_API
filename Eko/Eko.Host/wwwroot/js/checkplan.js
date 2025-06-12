document.addEventListener('DOMContentLoaded', function() {
    // Функция для получения значения cookie по имени
    function getCookie(name) {
        const value = `; ${document.cookie}`;
        const parts = value.split(`; ${name}=`);
        if (parts.length === 2) return parts.pop().split(';').shift();
    }

    // Получаем значение plan из cookie
    const plan = getCookie('plan');

    // Если plan не существует, блокируем все ссылки
    if (!plan) {
        blockAllGeneratorLinks();
        return;
    }

    // Блокируем все ссылки генераторов
    blockAllGeneratorLinks();

    // Разблокируем ссылки в зависимости от плана
    switch(plan) {
        case '1':
            unblockLink('/Generators/Text');
            break;
        case '2':
            unblockLink('/Generators/Text');
            unblockLink('/Generators/Email');
            break;
        case '3':
            unblockLink('/Generators/Text');
            unblockLink('/Generators/Email');
            unblockLink('/Generators/Code');
            break;
    }

    // Функция для блокировки всех ссылок генераторов
    function blockAllGeneratorLinks() {
        const generatorPaths = ['/Generators/Text', '/Generators/Code', '/Generators/Email'];
        generatorPaths.forEach(path => {
            blockLink(path);
        });
    }

    // Функция для блокировки конкретной ссылки
    function blockLink(path) {
        const links = document.querySelectorAll(`a[href="${path}"]`);
        links.forEach(link => {
            link.addEventListener('click', preventDefault);
            link.style.opacity = '0.5';
            link.style.pointerEvents = 'none';
            link.title = 'Доступно в более высоком тарифе';
        });
    }

    // Функция для разблокировки конкретной ссылки
    function unblockLink(path) {
        const links = document.querySelectorAll(`a[href="${path}"]`);
        links.forEach(link => {
            link.removeEventListener('click', preventDefault);
            link.style.opacity = '1';
            link.style.pointerEvents = 'auto';
            link.title = '';
        });
    }

    // Функция-заглушка для предотвращения действия по умолчанию
    function preventDefault(e) {
        e.preventDefault();
        alert('Эта функция доступна в более высоком тарифе');
    }
});