document.getElementById('registerForm').addEventListener('submit', async (e) => {
    e.preventDefault();

    const login = document.getElementById('login').value;
    const password = document.getElementById('password').value;

    const errorEl = document.getElementById('registerError');
    const successEl = document.getElementById('registerSuccess');

    try {
        errorEl.style.display = 'none';
        successEl.style.display = 'none';

        const userData = {
            Username: login,
            Password: password
        };

        console.log('Registration request URL:', `${API_BASE_URL}${API_ENDPOINTS.register}`);
        console.log('Registration data:', userData);

        const response = await fetch(`${API_BASE_URL}${API_ENDPOINTS.register}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            },
            body: JSON.stringify(userData)
        });

        console.log('Registration response status:', response.status);
        console.log('Registration response ok:', response.ok);

        if (!response.ok) {
            const errorText = await response.text();
            console.error('Registration response error text:', errorText);
            throw new Error(`HTTP error! status: ${response.status}, message: ${errorText}`);
        }

        const result = await response.json();
        console.log('Registration response data:', result);

        if (result.isSuccess || result.IsSuccess) {
            console.log('Registration successful');
            successEl.textContent = 'Registration successful! Redirecting to login page...';
            successEl.style.display = 'block';
            
            setTimeout(() => {
                window.location.href = 'login_profile.html';
            }, 2000);
        } else {
            const errorMsg = result.errorMessage || result.ErrorMessage || 'Registration failed';
            console.error('Registration failed:', errorMsg);
            console.error('Error code:', result.code);
            console.error('Full response:', result);
            throw new Error(errorMsg);
        }

    } catch (error) {
        console.error('Registration error:', error);
        errorEl.textContent = error.message;
        errorEl.style.display = 'block';
    }
});