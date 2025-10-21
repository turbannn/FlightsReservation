/// <reference path="./types.js" />

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

        const response = await fetch(`${API_BASE_URL}${API_ENDPOINTS.getUserProfile}`, {
            credentials: 'include',
            headers: {
                'Accept': 'application/json'
            }
        });

        if (!response.ok) {
            if (response.status === 401) {
                window.location.href = 'login.html';
                return;
            }
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        const result = await response.json();

        if (result.isSuccess && result.value) {
            displayProfile(result.value);
        } else {
            throw new Error(result.message || 'Не вдалося завантажити профіль');
        }

    } catch (error) {
        errorEl.textContent = `Помилка: ${error.message}`;
        errorEl.style.display = 'block';
    } finally {
        loadingEl.style.display = 'none';
    }
}

/**
 * Відображення профілю користувача
 * @param {TotalUserReadDto} data 
 */
function displayProfile(data) {
    document.getElementById('userName').textContent = data.name;
    document.getElementById('userSurname').textContent = data.surname;
    document.getElementById('userEmail').textContent = data.email;
    document.getElementById('userLogin').textContent = data.login;
    document.getElementById('userMoney').textContent = data.money;
    document.getElementById('userRole').textContent = data.role;

    displayReservations(data.reservations);

    document.getElementById('profileContent').style.display = 'block';
}

function displayReservations(reservations) {
    const listEl = document.getElementById('reservationsList');

    if (!reservations || reservations.length === 0) {
        listEl.innerHTML = '<p>У вас немає бронювань</p>';
        return;
    }

    listEl.innerHTML = reservations.map(reservation => `
        <div class="reservation-card">
            <h3>Бронювання #${reservation.id.substring(0, 8)}</h3>
            <div class="flight-details">
                <div class="flight-detail">
                    <strong>Рейс:</strong> ${reservation.flight.departureLocation} → ${reservation.flight.arrivalLocation}
                </div>
                <div class="flight-detail">
                    <strong>Відправлення:</strong> ${formatDate(reservation.flight.departureDate)}
                </div>
                <div class="flight-detail">
                    <strong>Прибуття:</strong> ${formatDate(reservation.flight.arrivalDate)}
                </div>
                <div class="flight-detail">
                    <strong>Місце:</strong> ${reservation.seat.seatNumber}
                </div>
                <div class="flight-detail">
                    <strong>Дата бронювання:</strong> ${formatDate(reservation.reservationDate)}
                </div>
            </div>
            <h4>Пасажир:</h4>
            <div class="flight-details">
                <div class="flight-detail">
                    <strong>Ім'я:</strong> ${reservation.passenger.name} ${reservation.passenger.surname}
                </div>
                <div class="flight-detail">
                    <strong>Паспорт:</strong> ${reservation.passenger.passportNumber}
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