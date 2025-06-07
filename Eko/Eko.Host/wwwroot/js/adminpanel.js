document.addEventListener('DOMContentLoaded', function() {
    // API базовый URL
    const API_BASE_URL = 'http://localhost:5252/';

    // DOM элементы
    const menuItems = document.querySelectorAll('.admin-sidebar li');
    const sections = document.querySelectorAll('.content-section');
    const userSelects = document.querySelectorAll('.user-select');
    const logoutBtn = document.querySelector('.logout-btn');
    const forms = document.querySelectorAll('.admin-form');

    // Текущий выбранный пользователь
    let selectedUser = null;

    // Инициализация приложения
    init();

    function init() {
        setupEventListeners();
        loadUsers();
    }

    function setupEventListeners() {
        // Переключение между разделами
        menuItems.forEach(item => {
            item.addEventListener('click', function() {
                menuItems.forEach(i => i.classList.remove('active'));
                sections.forEach(s => s.classList.remove('active'));

                this.classList.add('active');
                const sectionId = this.getAttribute('data-section');
                document.getElementById(sectionId).classList.add('active');
            });
        });

        // Изменение выбранного пользователя
        userSelects.forEach(select => {
            select.addEventListener('change', function() {
                selectedUser = this.options[this.selectedIndex];
            });
        });

        // Отправка форм
        forms.forEach(form => {
            form.addEventListener('submit', handleFormSubmit);
        });
    }

    // Загрузка пользователей с сервера
    async function loadUsers() {
        try {
            const response = await fetch(`${API_BASE_URL}AdminPanel/GetPersons`, {
                method: 'GET'
            });

            if (!response.ok) {
                throw new Error('Ошибка загрузки пользователей');
            }

            const persons = await response.json();
            populateUserSelects(persons);
        } catch (error) {
            console.error('Ошибка:', error);
            showError('Не удалось загрузить список пользователей');
        }
    }

    async function loadUserProfile(email) {
        try {
            const response = await fetch(`${API_BASE_URL}AdminPanel/GetProfile?email=${email}`);

            if (!response.ok) {
                throw new Error('Ошибка загрузки профиля');
            }

            const profile = await response.json();
            fillProfileForm(profile);
        } catch (error) {
            console.error('Ошибка загрузки профиля:', error);
            showError('Не удалось загрузить данные профиля');
        }
    }

    function fillProfileForm(profile) {
        const form = document.querySelector('#edit .admin-form');
        if (!form) return;

        console.log('Received profile data:', profile); // Для отладки

        if (profile.dataOfBirth) {
            const birthDate = new Date(profile.dataOfBirth);
            form.querySelector('#edit-birthdate').value = birthDate.toISOString().split('T')[0];
        } else {
            form.querySelector('#edit-birthdate').value = '';
        }

        form.querySelector('#edit-experience').value = profile.experience || '';
        form.querySelector('#edit-bio').value = profile.biography || '';

        // Обработка skills (может быть null или строкой)
        const skillsValue = profile.skills ?
            (Array.isArray(profile.skills) ? profile.skills.join(', ') : profile.skills) : '';
        form.querySelector('#edit-skills').value = skillsValue;

        form.querySelector('#edit-specialty').value = profile.specialization || '';
    }
    // Заполнение выпадающих списков пользователями
    function populateUserSelects(persons) {
        userSelects.forEach(select => {
            while (select.options.length > 1) {
                select.remove(1);
            }

            if (select.options.length > 0) {
                select.options[0].text = '-- Выберите пользователя --';
            }

            persons.forEach(person => {
                const option = document.createElement('option');
                option.value = person.id;
                option.text = `${person.firstName} ${person.lastName} (${person.email})`;
                option.setAttribute('data-user', JSON.stringify(person));
                select.add(option);
            });
        });

        // Добавляем обработчик изменения выбора пользователя для загрузки профиля
        const editUserSelect = document.getElementById('edit-user');
        if (editUserSelect) {
            editUserSelect.addEventListener('change', async function() {
                const selectedOption = this.options[this.selectedIndex];
                if (selectedOption && selectedOption.value) {
                    const userData = JSON.parse(selectedOption.getAttribute('data-user'));
                    await loadUserProfile(userData.email);
                }
            });
        }
    }

    // Обработка отправки форм
    async function handleFormSubmit(e) {
        e.preventDefault();
        const form = e.target;
        const sectionId = form.closest('.content-section').id;

        try {
            if (sectionId === 'delete' && !form.querySelector('#confirm-delete').checked) {
                throw new Error('Пожалуйста, подтвердите удаление пользователя');
            }

            let response;

            if (sectionId === 'register') {
                response = await registerUser(form);
            } else if (sectionId === 'edit') {
                response = await updateUser(form);
            } else if (sectionId === 'delete') {
                response = await deleteUser(form);
            }

            if (response.ok) {
                showSuccess('Операция выполнена успешно!');
                form.reset();
                // Обновляем список пользователей
                await loadUsers();
            } else {
                const errorText = await response.text();
                throw new Error(errorText || 'Произошла ошибка');
            }
        } catch (error) {
            showError(error.message);
        }
    }

    // Регистрация нового пользователя
    async function registerUser(form) {
        const formData = new FormData();
        formData.append('email', form.querySelector('#reg-email').value);
        formData.append('firstName', form.querySelector('#reg-firstname').value);
        formData.append('lastName', form.querySelector('#reg-lastname').value);
        formData.append('isAdmin', form.querySelector('#reg-isadmin').checked);
        formData.append('password', form.querySelector('#reg-password').value);

        return await fetch(`${API_BASE_URL}AdminPanel/RegisterPerson`, {
            method: 'POST',
            body: formData
        });
    }

    // Обновление пользователя
    // Обновление пользователя
    async function updateUser(form) {
        if (!selectedUser || !selectedUser.value) {
            throw new Error('Пользователь не выбран');
        }

        const userData = JSON.parse(selectedUser.getAttribute('data-user'));
        const userEmail = userData.email;

        if (!userEmail) {
            throw new Error('Не удалось определить email пользователя');
        }

        try {
            const formData = new FormData();

            // Добавляем email как query параметр
            const url = `${API_BASE_URL}AdminPanel/UpdateProfile?email=${encodeURIComponent(userEmail)}`;

            // Добавляем остальные данные как form-data
            if (form.querySelector('#edit-birthdate').value) {
                formData.append('birthDate', form.querySelector('#edit-birthdate').value);
            }
            if (form.querySelector('#edit-experience').value) {
                formData.append('experience', form.querySelector('#edit-experience').value);
            }
            if (form.querySelector('#edit-bio').value) {
                formData.append('bio', form.querySelector('#edit-bio').value);
            }
            if (form.querySelector('#edit-skills').value) {
                formData.append('skills', form.querySelector('#edit-skills').value);
            }
            if (form.querySelector('#edit-specialty').value) {
                formData.append('specialization', form.querySelector('#edit-specialty').value); // Обратите внимание на имя поля!
            }

            const response = await fetch(url, {
                method: 'PUT',
                body: formData
            });

            if (!response.ok) {
                const errorData = await response.json();
                throw new Error(errorData.message || 'Ошибка при обновлении профиля');
            }

            return response;
        } catch (error) {
            console.error('Ошибка при обновлении:', error);
            throw error;
        }
    }

    // Удаление пользователя
    async function deleteUser(form) {
        if (!selectedUser || !selectedUser.value) {
            throw new Error('Пользователь не выбран');
        }

        const userData = JSON.parse(selectedUser.getAttribute('data-user'));
        const userEmail = userData.email;

        if (!userEmail) {
            throw new Error('Не удалось определить email пользователя');
        }

        try {
            const response = await fetch(`${API_BASE_URL}AdminPanel/DeletePerson?email=${encodeURIComponent(userEmail)}`, {
                method: 'DELETE'
            });

            if (!response.ok) {
                const errorData = await response.json();
                throw new Error(errorData.message || 'Ошибка при удалении пользователя');
            }

            return response;
        } catch (error) {
            console.error('Ошибка при удалении:', error);
            throw error;
        }
    }

    // Показать сообщение об успехе
    function showSuccess(message) {
        alert(message);
    }

    // Показать сообщение об ошибке
    function showError(message) {
        alert(`Ошибка: ${message}`);
    }
});