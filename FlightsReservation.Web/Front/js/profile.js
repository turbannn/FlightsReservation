document.addEventListener('DOMContentLoaded', () => {
    loadUserProfile();
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