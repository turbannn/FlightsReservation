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
        document.getElementById('departureLocation').value = searchParams.DepartureCity || '';
        document.getElementById('arrivalLocation').value = searchParams.ArrivalCity || '';
        document.getElementById('departureDate').value = searchParams.DepartureDate || '';
        document.getElementById('returnDate').value = searchParams.ReturnDate || '';
    }
}

async function loadFlights() {
    const loadingEl = document.getElementById('flightsLoading');
    const errorEl = document.getElementById('flightsError');
    const listEl = document.getElementById('flightsList');

    if (!searchParams) {
        const errorMsg = 'Параметри пошуку не знайдено';
        console.error(errorMsg);
        errorEl.textContent = errorMsg;
        errorEl.style.display = 'block';
        loadingEl.style.display = 'none';
        return;
    }

    try {
        loadingEl.style.display = 'block';
        errorEl.style.display = 'none';
        listEl.innerHTML = '';

        const hasReturnDate = searchParams.ReturnDate && searchParams.ReturnDate.trim() !== '';
        const endpoint = hasReturnDate ? API_ENDPOINTS.searchFlightsWithReturn : API_ENDPOINTS.searchFlights;

        // Формуємо body request
        const requestBody = {
            DepartureCity: searchParams.DepartureCity,
            ArrivalCity: searchParams.ArrivalCity,
            DepartureDate: searchParams.DepartureDate
        };

        // Додаємо returnDate тільки якщо він є
        if (hasReturnDate) {
            requestBody.ReturnDate = searchParams.ReturnDate;
        }

        console.log('Request URL:', `${API_BASE_URL}${endpoint}?page=${currentPage}&size=10`);
        console.log('Request body:', requestBody);

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
            // result.value - це масив рейсів, а totalCount - окреме поле
            const data = {
                items: result.value,
                totalCount: result.totalCount,
                pageNumber: currentPage,
                pageSize: 10
            };
            
            // Обчислюємо totalPages
            data.totalPages = Math.ceil(result.totalCount / 10);
            data.hasPreviousPage = currentPage > 1;
            data.hasNextPage = currentPage < data.totalPages;
            
            displayFlights(data);
            updatePagination(data);
        } else {
            const errorMsg = result.errorMessage || 'Не вдалося завантажити рейси';
            console.error('API error:', errorMsg);
            console.error('Error code:', result.code);
            console.error('Full response:', result);
            throw new Error(errorMsg);
        }

    } catch (error) {
        console.error('Load flights error:', error);
        errorEl.textContent = `Помилка: ${error.message}`;
        errorEl.style.display = 'block';
    } finally {
        loadingEl.style.display = 'none';
    }
}

function displayFlights(data) {
    const listEl = document.getElementById('flightsList');
    
    console.log('Displaying flights:', data);
    
    if (!data.items || data.items.length === 0) {
        listEl.innerHTML = '<p>Рейси не знайдено</p>';
        return;
    }

    listEl.innerHTML = data.items.map(flight => `
        <div class="flight-card">
            <div class="flight-header">
                <div class="flight-route">
                    ${flight.departure} → ${flight.arrival}
                </div>
                <div class="flight-price">${flight.price} ${flight.currency}</div>
            </div>
            <div class="flight-details">
                <div class="flight-detail">
                    <strong>Flight Number:</strong> ${flight.flightNumber}
                </div>
                <div class="flight-detail">
                    <strong>Departure:</strong> ${formatDate(flight.departureTime)}
                </div>
                <div class="flight-detail">
                    <strong>Arrival:</strong> ${formatDate(flight.arrivalTime)}
                </div>
                <div class="flight-detail">
                    <strong>Company:</strong> ${flight.company || 'N/A'}
                </div>
                <div class="flight-detail">
                    <strong>Aircraft:</strong> ${flight.airplaneType}
                </div>
                <div class="flight-detail">
                    <strong>Available Seats:</strong> ${flight.availableSeats}
                </div>
            </div>
            <div class="flight-actions">
                <button class="btn btn-primary" onclick="startReservation('${flight.id}')">
                    Start Reservation
                </button>
            </div>
        </div>
    `).join('');
}

function startReservation(flightId) {
    console.log('Starting reservation for flight:', flightId);
    // Save flight ID to session storage and redirect to seats page
    sessionStorage.setItem('selectedFlightId', flightId);
    window.location.href = 'seats.html';
}

function updatePagination(data) {
    totalPages = data.totalPages;
    const paginationEl = document.getElementById('pagination');
    const pageInfoEl = document.getElementById('pageInfo');
    const prevBtn = document.getElementById('prevPage');
    const nextBtn = document.getElementById('nextPage');

    console.log('Pagination info:', { currentPage, totalPages });

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
            console.log('Going to previous page:', currentPage);
            sessionStorage.setItem('currentPage', currentPage.toString());
            loadFlights();
        }
    });

    document.getElementById('nextPage').addEventListener('click', () => {
        if (currentPage < totalPages) {
            currentPage++;
            console.log('Going to next page:', currentPage);
            sessionStorage.setItem('currentPage', currentPage.toString());
            loadFlights();
        }
    });
}

function setupSearchForm() {
    document.getElementById('flightSearchForm').addEventListener('submit', (e) => {
        e.preventDefault();

        const departureCity = document.getElementById('departureLocation').value;
        const arrivalCity = document.getElementById('arrivalLocation').value;
        const departureDate = document.getElementById('departureDate').value;
        const returnDate = document.getElementById('returnDate').value;

        searchParams = {
            DepartureCity: departureCity,
            ArrivalCity: arrivalCity,
            DepartureDate: departureDate,
            ReturnDate: returnDate || null
        };

        console.log('New search params:', searchParams);

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