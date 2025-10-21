/// <reference path="./types.js" />

let currentPage = 1;
let totalPages = 1;
let searchParams = null;

document.addEventListener('DOMContentLoaded', () => {
    loadSearchParams();
    loadFlights();
    setupPagination();
    setupSearchForm();
});

function loadSearchParams() {
    const savedParams = sessionStorage.getItem('searchParams');
    const savedPage = sessionStorage.getItem('currentPage');
    
    if (savedParams) {
        searchParams = JSON.parse(savedParams);
        currentPage = parseInt(savedPage) || 1;
        fillSearchForm();
    }
}

function fillSearchForm() {
    if (searchParams) {
        document.getElementById('departureLocation').value = searchParams.departureLocation || '';
        document.getElementById('arrivalLocation').value = searchParams.arrivalLocation || '';
        document.getElementById('departureDate').value = searchParams.departureDate || '';
        document.getElementById('returnDate').value = searchParams.returnDate || '';
    }
}

async function loadFlights() {
    const loadingEl = document.getElementById('flightsLoading');
    const errorEl = document.getElementById('flightsError');
    const listEl = document.getElementById('flightsList');

    if (!searchParams) {
        errorEl.textContent = 'Параметри пошуку не знайдено';
        errorEl.style.display = 'block';
        loadingEl.style.display = 'none';
        return;
    }

    try {
        loadingEl.style.display = 'block';
        errorEl.style.display = 'none';
        listEl.innerHTML = '';

        const hasReturnDate = searchParams.returnDate && searchParams.returnDate.trim() !== '';
        const endpoint = hasReturnDate ? API_ENDPOINTS.searchFlightsWithReturn : API_ENDPOINTS.searchFlights;

        // Формуємо body request
        const requestBody = {
            departureLocation: searchParams.departureLocation,
            arrivalLocation: searchParams.arrivalLocation,
            departureDate: searchParams.departureDate
        };

        // Додаємо returnDate тільки якщо він є
        if (hasReturnDate) {
            requestBody.returnDate = searchParams.returnDate;
        }

        // Query параметри для пагінації
        const queryParams = new URLSearchParams({
            page: currentPage ?? 1,
            size: 10
        });

        const response = await fetch(`${API_BASE_URL}${endpoint}?${queryParams}`, {
            method: 'POST',
            credentials: 'include',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            },
            body: JSON.stringify(requestBody)
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        const result = await response.json();

        if (result.isSuccess && result.value) {
            displayFlights(result.value);
            updatePagination(result.value);
        } else {
            throw new Error(result.message || 'Не вдалося завантажити рейси');
        }

    } catch (error) {
        errorEl.textContent = `Помилка: ${error.message}`;
        errorEl.style.display = 'block';
    } finally {
        loadingEl.style.display = 'none';
    }
}

/**
 * Відображення списку рейсів
 * @param {FlightReservationPagedResult} data 
 */
function displayFlights(data) {
    const listEl = document.getElementById('flightsList');
    
    if (!data.items || data.items.length === 0) {
        listEl.innerHTML = '<p>Рейси не знайдено</p>';
        return;
    }

    listEl.innerHTML = data.items.map(flight => `
        <div class="flight-card">
            <div class="flight-header">
                <div class="flight-route">
                    ${flight.departureLocation} → ${flight.arrivalLocation}
                </div>
                <div class="flight-price">${flight.price} грн</div>
            </div>
            <div class="flight-details">
                <div class="flight-detail">
                    <strong>Відправлення:</strong> ${formatDate(flight.departureDate)}
                </div>
                <div class="flight-detail">
                    <strong>Прибуття:</strong> ${formatDate(flight.arrivalDate)}
                </div>
                <div class="flight-detail">
                    <strong>Компанія:</strong> ${flight.company}
                </div>
                <div class="flight-detail">
                    <strong>Вільних місць:</strong> ${flight.availableSeats}
                </div>
            </div>
        </div>
    `).join('');
}

function updatePagination(data) {
    totalPages = data.totalPages;
    const paginationEl = document.getElementById('pagination');
    const pageInfoEl = document.getElementById('pageInfo');
    const prevBtn = document.getElementById('prevPage');
    const nextBtn = document.getElementById('nextPage');

    if (totalPages <= 1) {
        paginationEl.style.display = 'none';
        return;
    }

    paginationEl.style.display = 'flex';
    pageInfoEl.textContent = `Сторінка ${currentPage} з ${totalPages}`;
    
    prevBtn.disabled = currentPage === 1;
    nextBtn.disabled = currentPage === totalPages;
}

function setupPagination() {
    document.getElementById('prevPage').addEventListener('click', () => {
        if (currentPage > 1) {
            currentPage--;
            sessionStorage.setItem('currentPage', currentPage.toString());
            loadFlights();
        }
    });

    document.getElementById('nextPage').addEventListener('click', () => {
        if (currentPage < totalPages) {
            currentPage++;
            sessionStorage.setItem('currentPage', currentPage.toString());
            loadFlights();
        }
    });
}

function setupSearchForm() {
    document.getElementById('flightSearchForm').addEventListener('submit', (e) => {
        e.preventDefault();

        const departureLocation = document.getElementById('departureLocation').value;
        const arrivalLocation = document.getElementById('arrivalLocation').value;
        const departureDate = document.getElementById('departureDate').value;
        const returnDate = document.getElementById('returnDate').value;

        searchParams = {
            departureLocation,
            arrivalLocation,
            departureDate,
            returnDate: returnDate || null
        };

        currentPage = 1;
        sessionStorage.setItem('searchParams', JSON.stringify(searchParams));
        sessionStorage.setItem('currentPage', '1');
        
        loadFlights();
    });
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