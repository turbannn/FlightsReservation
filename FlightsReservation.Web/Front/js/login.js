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

        console.log('Login request URL:', `${API_BASE_URL}${API_ENDPOINTS.login}?${queryParams}`);

        const response = await fetch(`${API_BASE_URL}${API_ENDPOINTS.login}?${queryParams}`, {
            method: 'GET',
            credentials: 'include',
            headers: {
                'Accept': 'application/json'
            }
        });

        console.log('Login response status:', response.status);
        console.log('Login response ok:', response.ok);
        console.log('Login response headers:', [...response.headers.entries()]);
        console.log('Cookies after login:', document.cookie);

        if (!response.ok) {
            const errorText = await response.text();
            console.error('Login response error text:', errorText);
            throw new Error(`HTTP error! status: ${response.status}, message: ${errorText}`);
        }

        const result = await response.json();
        console.log('Login response data:', result);

        if (result.isSuccess) {
            console.log('Login successful, redirecting to profile...');
            window.location.href = 'profile.html';
        } else {
            const errorMsg = result.errorMessage || 'Невірний логін або пароль';
            console.error('Login failed:', errorMsg);
            console.error('Error code:', result.code);
            console.error('Full response:', result);
            throw new Error(errorMsg);
        }

    } catch (error) {
        console.error('Login error:', error);
        errorEl.textContent = error.message;
        errorEl.style.display = 'block';
    }
});