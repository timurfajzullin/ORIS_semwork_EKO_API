document.addEventListener('DOMContentLoaded', function() {
    console.log('DOM fully loaded');

    const chatForm = document.getElementById('chatForm');
    const messageInput = document.getElementById('messageInput');
    const chatContainer = document.querySelector('.single-item-generator');

    if (!chatForm || !messageInput || !chatContainer) {
        console.error('One of the required elements is missing:', {
            chatForm, messageInput, chatContainer
        });
        return;
    }

    // Загружаем историю чата при загрузке страницы
    loadChatHistory();

    chatForm.addEventListener('submit', async function(e) {
        e.preventDefault();
        console.log('Form submitted');

        const message = messageInput.value.trim();
        if (!message) {
            console.warn('Empty message');
            return;
        }

        addUserMessage(message);
        messageInput.value = '';

        try {
            console.log('Sending request to server with message:', message);
            const apiUrl = '/Generators/AskDeepSeek';

            const response = await fetch(apiUrl, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Type': 'Text'
                },
                body: JSON.stringify(message)
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            const data = await response.json();
            console.log('Received response:', data);

            addBotMessage(data.response);
        } catch (error) {
            console.error('Error:', error);
            addBotMessage(`Произошла ошибка: ${error.message}`);
        }
    });

    async function loadChatHistory() {
        try {
            console.log('Loading chat history...');

            // Очищаем контейнер перед загрузкой новых сообщений
            chatContainer.innerHTML = '';

            const response = await fetch('/Generators/GetChatsItems');
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            const chats = await response.json();
            console.log('Loaded chat history:', chats);

            // Фильтруем и добавляем сообщения
            chats
                .filter(chat => chat["type"] === "Text")
                .forEach(chat => {
                    if (!chat["message"]) return; // Пропускаем пустые сообщения

                    if (chat["whoseMessage"] === "Person") {
                        console.log(chat.Message);
                        addUserMessage(chat["message"]);
                    } else if (chat["whoseMessage"]  === "Ai") {
                        addBotMessage(chat["message"]);
                    }
                });
        } catch (error) {
            console.error('Failed to load chat history:', error);
            // Можно добавить уведомление пользователю об ошибке
        }
    }

    function addUserMessage(message) {
        if (!message) return;

        const messageHtml = `
            <div class="chat-box">
                <div class="chat-box-author d-flex flex-wrap justify-end align-start gap-20">
                    <div class="author-text-box d-flex align-center gap-15">
                        <div class="text">${escapeHtml(message)}</div>
                    </div>
                </div>
            </div>
        `;
        chatContainer.insertAdjacentHTML('beforeend', messageHtml);
        scrollToBottom();
    }

    function addBotMessage(message) {
        if (!message) return;

        const messageHtml = `
            <div class="chat-box">
                <div class="d-flex flex-wrap justify-start align-start gap-20">
                    <div class="ekoai-thumb"><img src="../images/logo/favicon.svg" alt=""></div>
                    <div class="author-text-box d-flex align-center gap-15">
                        <div class="text">${escapeHtml(message)}</div>
                    </div>
                </div>
            </div>
        `;
        chatContainer.insertAdjacentHTML('beforeend', messageHtml);
        scrollToBottom();
    }

    function scrollToBottom() {
        chatContainer.scrollTop = chatContainer.scrollHeight;
    }

    function escapeHtml(unsafe) {
        if (!unsafe) return '';
        return unsafe
            .replace(/&/g, "&amp;")
            .replace(/</g, "&lt;")
            .replace(/>/g, "&gt;")
            .replace(/"/g, "&quot;")
            .replace(/'/g, "&#039;");
    }
});