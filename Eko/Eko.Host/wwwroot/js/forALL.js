import { fetchData } from './fetch.js';  // Путь относительно auth.js

window.userEmail = localStorage.getItem("emailAddress") || "";

document.addEventListener("DOMContentLoaded", () => {
    const buttonLogout = document.getElementById("logout");

    
    if (buttonLogout) {
        buttonLogout.addEventListener("click", async (e) => {
            e.preventDefault();
            console.log("Logout");
            try {
                await fetchData("/Auth/Logout", "GET");
                window.location.href = "/";
            } catch (error) {
                console.error("Logout failed:", error);
                alert("Logout failed. Please try again.");
            }
        });
    }
});

// Объявляем переменную в глобальной области видимости


document.addEventListener("DOMContentLoaded", async () => {
    const dropdownToggle = document.querySelector('.dropdown-toggle');
    const dropdownMenu = document.querySelector('.dropdown-menu');

    // Функция проверки JWT токена
    function hasJwtToken() {
        // Проверка cookies
        const tokenNames = ['jwt', 'token', 'access_token', 'jwtToken'];
        const cookies = document.cookie.split(';');

        for (const cookie of cookies) {
            const trimmed = cookie.trim();
            for (const name of tokenNames) {
                if (trimmed.startsWith(`${name}=`)) {
                    return true;
                }
            }
        }

        const localStorageTokens = ['jwt', 'token', 'accessToken'];
        for (const key of localStorageTokens) {
            if (localStorage.getItem(key)) {
                return true;
            }
        }

        return false;
    }

    if (!hasJwtToken()) {
        dropdownToggle.removeAttribute('data-bs-toggle');
        dropdownToggle.removeAttribute('href');
        dropdownToggle.removeAttribute('role');
        dropdownToggle.removeAttribute('aria-expanded');

        dropdownToggle.style.cursor = 'default';

        dropdownToggle.addEventListener('click', (e) => {
            e.preventDefault();
            e.stopPropagation();
            return false;
        });

        dropdownToggle.style.opacity = '0.6';
        dropdownToggle.title = 'Please login to access this menu';

        if (dropdownMenu) {
            dropdownMenu.style.display = 'none';
        }
    } else {
        // Проверяем, есть ли email
        if (!userEmail) {
            console.error("Email not found in localStorage");
            return;
        }
    }
});

document.addEventListener("DOMContentLoaded", function() {
    const form = document.getElementById("loginForm");

    if (!form) {
        return;
    }

    form.addEventListener("submit", function(event) {
        const email = document.getElementById("emailAddress").value.trim();

        if (!email) {
            alert("Please enter your email");
            event.preventDefault();
            return;
        }

        // Сохраняем email в localStorage и глобальную переменную
        userEmail = email;
        localStorage.setItem("emailAddress", email);
    });
});
