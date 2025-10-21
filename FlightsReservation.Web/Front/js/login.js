/// <reference path="./types.js" />

document.getElementById('loginForm').addEventListener('submit', async (e) => {
    e.preventDefault();

    const login = document.getElementById('login').value;
    const password = document.getElementById('password').value;
    const errorEl = document.getElementById('loginError');

    try {
        errorEl.style.display = 'none';

        const queryParams = new URLSearchParams({
            login: login,
            password: password
        });

        const response = await fetch(`${API_BASE_URL}${API_ENDPOINTS.login}?${queryParams}`, {
            method: 'GET',
            credentials: 'include',
            headers: {
                'Accept': 'application/json'
            }
        });

        const result = await response.json();

        if (result.isSuccess) {
            window.location.href = 'profile.html';
        } else {
            throw new Error(result.message || 'Невірний логін або пароль');
        }

    } catch (error) {
        errorEl.textContent = error.message;
        errorEl.style.display = 'block';
    }
});