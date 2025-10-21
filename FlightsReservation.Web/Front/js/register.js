/// <reference path="./types.js" />

document.getElementById('registerForm').addEventListener('submit', async (e) => {
    e.preventDefault();

    const name = document.getElementById('name').value;
    const surname = document.getElementById('surname').value;
    const email = document.getElementById('email').value;
    const login = document.getElementById('login').value;
    const password = document.getElementById('password').value;

    const errorEl = document.getElementById('registerError');
    const successEl = document.getElementById('registerSuccess');

    try {
        errorEl.style.display = 'none';
        successEl.style.display = 'none';

        /** @type {UserCreateDto} */
        const userData = {
            name,
            surname,
            email,
            login,
            password
        };

        const response = await fetch(`${API_BASE_URL}${API_ENDPOINTS.register}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            },
            body: JSON.stringify(userData)
        });

        const result = await response.json();

        if (result.isSuccess) {
            successEl.textContent = 'Реєстрація успішна! Перенаправлення на сторінку входу...';
            successEl.style.display = 'block';
            
            setTimeout(() => {
                window.location.href = 'login.html';
            }, 2000);
        } else {
            throw new Error(result.message || 'Помилка реєстрації');
        }

    } catch (error) {
        errorEl.textContent = error.message;
        errorEl.style.display = 'block';
    }
});