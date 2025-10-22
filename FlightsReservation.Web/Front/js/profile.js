document.addEventListener('DOMContentLoaded', () => {
    loadUserProfile();
    setupTopUpForm();
});

async function loadUserProfile() {
    const loadingEl = document.getElementById('profileLoading');
    const errorEl = document.getElementById('profileError');
    const contentEl = document.getElementById('profileContent');

    try {
        loadingEl.style.display = 'block';
        errorEl.style.display = 'none';
        contentEl.style.display = 'none';

        console.log('Profile request URL:', `${API_BASE_URL}${API_ENDPOINTS.getUserProfile}`);
        console.log('Cookies before profile request:', document.cookie);

        const response = await fetch(`${API_BASE_URL}${API_ENDPOINTS.getUserProfile}`, {
            credentials: 'include',
            headers: {
                'Accept': 'application/json'
            }
        });

        console.log('Profile response status:', response.status);
        console.log('Profile response ok:', response.ok);
        console.log('Profile response headers:', [...response.headers.entries()]);

        if (!response.ok) {
            if (response.status === 401) {
                console.warn('Unauthorized, redirecting to login...');
                window.location.href = 'login.html';
                return;
            }
            const errorText = await response.text();
            console.error('Profile response error text:', errorText);
            throw new Error(`HTTP error! status: ${response.status}, message: ${errorText}`);
        }

        const result = await response.json();
        console.log('Profile response data:', result);

        if (result.isSuccess && result.value) {
            displayProfile(result.value);
        } else {
            const errorMsg = result.errorMessage || 'Не вдалося завантажити профіль';
            console.error('Profile load failed:', errorMsg);
            console.error('Error code:', result.code);
            console.error('Full response:', result);
            throw new Error(errorMsg);
        }

    } catch (error) {
        console.error('Profile load error:', error);
        errorEl.textContent = `Помилка: ${error.message}`;
        errorEl.style.display = 'block';
    } finally {
        loadingEl.style.display = 'none';
    }
}

function displayProfile(data) {
    console.log('Displaying profile data:', data);

    document.getElementById('userName').textContent = data.username;
    document.getElementById('userSurname').textContent = data.surname || 'N/A';
    document.getElementById('userEmail').textContent = data.email || 'N/A';
    document.getElementById('userLogin').textContent = data.username;
    document.getElementById('userMoney').textContent = data.money;
    document.getElementById('userRole').textContent = data.role || 'User';

    displayReservations(data.reservations);

    document.getElementById('profileContent').style.display = 'block';
}

function displayReservations(reservations) {
    const listEl = document.getElementById('reservationsList');

    console.log('Displaying reservations:', reservations);

    if (!reservations || reservations.length === 0) {
        listEl.innerHTML = '<p>У вас немає бронювань</p>';
        return;
    }

    listEl.innerHTML = reservations.map(reservation => `
        <div class="reservation-card">
            <h3>Бронювання #${reservation.reservationNumber}</h3>
            <div class="flight-details">
                <div class="flight-detail">
                    <strong>Рейс:</strong> ${reservation.flight.departure} → ${reservation.flight.arrival}
                </div>
                <div class="flight-detail">
                    <strong>Відправлення:</strong> ${formatDate(reservation.flight.departureTime)}
                </div>
                <div class="flight-detail">
                    <strong>Прибуття:</strong> ${formatDate(reservation.flight.arrivalTime)}
                </div>
                <div class="flight-detail">
                    <strong>Компанія:</strong> ${reservation.flight.company}
                </div>
                <div class="flight-detail">
                    <strong>Дата бронювання:</strong> ${formatDate(reservation.reservationDate)}
                </div>
            </div>
        </div>
    `).join('');
}

function formatDate(dateString) {
    const date = new Date(dateString);
    return date.toLocaleString('uk-UA', {
        year: 'numeric',
        month: '2-digit',
        day: '2-digit',
        hour: '2-digit',
        minute: '2-digit'
    });
}

// Top Up Balance Modal Functions
function openTopUpModal() {
    const modal = document.getElementById('topUpModal');
    const emailInput = document.getElementById('topUpEmail');
    
    // Pre-fill email from profile if available
    const userEmail = document.getElementById('userEmail').textContent;
    if (userEmail && userEmail !== 'N/A') {
        emailInput.value = userEmail;
    }
    
    modal.style.display = 'flex';
    console.log('Top up modal opened');
}

function closeTopUpModal() {
    const modal = document.getElementById('topUpModal');
    const form = document.getElementById('topUpForm');
    const errorEl = document.getElementById('topUpError');
    
    modal.style.display = 'none';
    form.reset();
    errorEl.style.display = 'none';
    console.log('Top up modal closed');
}

function setupTopUpForm() {
    document.getElementById('topUpForm').addEventListener('submit', async (e) => {
        e.preventDefault();
        await processPayment();
    });
    
    // Close modal when clicking outside
    window.addEventListener('click', (e) => {
        const modal = document.getElementById('topUpModal');
        if (e.target === modal) {
            closeTopUpModal();
        }
    });
}

async function processPayment() {
    const btn = document.getElementById('commitPaymentBtn');
    const errorEl = document.getElementById('topUpError');
    
    try {
        btn.disabled = true;
        btn.textContent = 'Processing...';
        errorEl.style.display = 'none';

        const amount = parseInt(document.getElementById('topUpAmount').value * 100); // With cents conversion
        const email = document.getElementById('topUpEmail').value.trim();

        if (amount <= 0) {
            throw new Error('Amount must be greater than 0');
        }

        if (!email) {
            throw new Error('Email is required');
        }

        // Create payment request matching PayuOrderRequest DTO
        const paymentRequest = {
            TotalAmount: amount,
            BuyerEmail: email
        };

        console.log('Payment request:', paymentRequest);
        console.log('Request URL:', `${API_BASE_URL}${API_ENDPOINTS.createPayment}`);

        const response = await fetch(`${API_BASE_URL}${API_ENDPOINTS.createPayment}`, {
            method: 'POST',
            credentials: 'include',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            },
            body: JSON.stringify(paymentRequest)
        });

        console.log('Response status:', response.status);
        console.log('Response ok:', response.ok);

        if (!response.ok) {
            const errorText = await response.text();
            console.error('Response error text:', errorText);
            throw new Error(`HTTP error! status: ${response.status}, message: ${errorText}`);
        }

        const result = await response.json();
        console.log('Response data:', result);

        if (result.isSuccess && result.value) {
            const payuResult = result.value;
            console.log('Payment created successfully:', payuResult);
            console.log('Redirect URI:', payuResult.redirectUri);
            
            if (payuResult.redirectUri) {
                // Redirect to PayU payment page
                window.location.href = payuResult.redirectUri;
            } else {
                throw new Error('No redirect URI provided');
            }
        } else {
            const errorMsg = result.errorMessage || 'Failed to create payment';
            console.error('API error:', errorMsg);
            console.error('Error code:', result.code);
            throw new Error(errorMsg);
        }

    } catch (error) {
        console.error('Payment error:', error);
        errorEl.textContent = `Error: ${error.message}`;
        errorEl.style.display = 'block';
        btn.disabled = false;
        btn.textContent = 'Commit Payment';
    }
}