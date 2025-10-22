// Debug: Check if config is loaded
console.log('=== LOGIN.JS LOADED ===');
console.log('API_BASE_URL:', typeof API_BASE_URL !== 'undefined' ? API_BASE_URL : 'UNDEFINED!');
console.log('API_ENDPOINTS:', typeof API_ENDPOINTS !== 'undefined' ? API_ENDPOINTS : 'UNDEFINED!');

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

        console.log('=== LOGIN REQUEST DEBUG ===');
        console.log('API_BASE_URL:', API_BASE_URL);
        console.log('API_ENDPOINTS.login:', API_ENDPOINTS.login);
        console.log('Full URL:', `${API_BASE_URL}${API_ENDPOINTS.login}?${queryParams}`);
        console.log('credentials:', 'include');
        console.log('Current origin:', window.location.origin);
        console.log('Current href:', window.location.href);

        const response = await fetch(`${API_BASE_URL}${API_ENDPOINTS.login}?${queryParams}`, {
            method: 'GET',
            credentials: 'include',
            headers: {
                'Accept': 'application/json'
            }
        });

        console.log('=== RESPONSE DEBUG ===');
        console.log('Response status:', response.status);
        console.log('Response ok:', response.ok);
        console.log('Response headers:', [...response.headers.entries()]);
        console.log('Response type:', response.type);
        console.log('Response url:', response.url);
        console.log('Document.cookie:', document.cookie);

        if (!response.ok) {
            const errorText = await response.text();
            console.error('Response error text:', errorText);
            throw new Error(`HTTP error! status: ${response.status}, message: ${errorText}`);
        }

        const result = await response.json();
        console.log('Response data:', result);

        if (result.isSuccess) {
            console.log('✅ Login successful, redirecting to profile...');
            window.location.href = 'profile.html';
        } else {
            const errorMsg = result.errorMessage || 'Invalid login or password';
            console.error('Login failed:', errorMsg);
            console.error('Error code:', result.code);
            throw new Error(errorMsg);
        }

    } catch (error) {
        console.error('❌ Login error:', error);
        errorEl.textContent = error.message;
        errorEl.style.display = 'block';
    }
});