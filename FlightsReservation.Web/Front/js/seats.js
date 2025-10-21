let flightData = null;
let selectedSeats = [];

document.addEventListener('DOMContentLoaded', () => {
    loadFlightData();
    setupBeginReservationButton();
});

async function loadFlightData() {
    const loadingEl = document.getElementById('seatsLoading');
    const errorEl = document.getElementById('seatsError');
    const contentEl = document.getElementById('seatsContent');

    const flightId = sessionStorage.getItem('selectedFlightId');
    
    if (!flightId) {
        console.error('No flight ID found');
        errorEl.textContent = 'No flight selected. Please return to search and select a flight.';
        errorEl.style.display = 'block';
        loadingEl.style.display = 'none';
        return;
    }

    try {
        loadingEl.style.display = 'block';
        errorEl.style.display = 'none';

        console.log('Loading flight:', flightId);
        console.log('Request URL:', `${API_BASE_URL}${API_ENDPOINTS.getFlightById}?id=${flightId}`);

        const response = await fetch(`${API_BASE_URL}${API_ENDPOINTS.getFlightById}?id=${flightId}`, {
            method: 'GET',
            credentials: 'include',
            headers: {
                'Accept': 'application/json'
            }
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
            flightData = result.value;
            displayFlightInfo(flightData);
            displaySeats(flightData.seats);
            contentEl.style.display = 'block';
        } else {
            const errorMsg = result.errorMessage || 'Failed to load flight data';
            console.error('API error:', errorMsg);
            console.error('Error code:', result.code);
            throw new Error(errorMsg);
        }

    } catch (error) {
        console.error('Load flight error:', error);
        errorEl.textContent = `Error: ${error.message}`;
        errorEl.style.display = 'block';
    } finally {
        loadingEl.style.display = 'none';
    }
}

function displayFlightInfo(flight) {
    const infoEl = document.getElementById('flightInfo');
    
    infoEl.innerHTML = `
        <div class="flight-info-grid">
            <div class="flight-info-item">
                <strong>Flight Number:</strong> ${flight.flightNumber}
            </div>
            <div class="flight-info-item">
                <strong>Route:</strong> ${flight.departure} → ${flight.arrival}
            </div>
            <div class="flight-info-item">
                <strong>Departure:</strong> ${formatDate(flight.departureTime)}
            </div>
            <div class="flight-info-item">
                <strong>Arrival:</strong> ${formatDate(flight.arrivalTime)}
            </div>
            <div class="flight-info-item">
                <strong>Company:</strong> ${flight.company || 'N/A'}
            </div>
            <div class="flight-info-item">
                <strong>Aircraft:</strong> ${flight.airplaneType}
            </div>
            <div class="flight-info-item">
                <strong>Price:</strong> ${flight.price} ${flight.currency}
            </div>
            <div class="flight-info-item">
                <strong>Available Seats:</strong> ${flight.availableSeats}
            </div>
        </div>
    `;
}

function displaySeats(seats) {
    const seatsListEl = document.getElementById('seatsList');
    
    if (!seats || seats.length === 0) {
        seatsListEl.innerHTML = '<p>No available seats for this flight.</p>';
        return;
    }

    // Filter only available seats (Lock should be in the past or default value)
    const availableSeats = seats.filter(seat => {
        const lockDate = new Date(seat.lock);
        const now = new Date();
        return lockDate <= now;
    });

    console.log('Total seats:', seats.length);
    console.log('Available seats:', availableSeats.length);

    if (availableSeats.length === 0) {
        seatsListEl.innerHTML = '<p>All seats are currently locked or reserved.</p>';
        return;
    }

    seatsListEl.innerHTML = availableSeats.map(seat => `
        <div class="seat-item">
            <label class="seat-checkbox">
                <input 
                    type="checkbox" 
                    value="${seat.id}" 
                    data-seat-number="${seat.seatNumber}"
                    onchange="toggleSeat('${seat.id}', '${seat.seatNumber}', this.checked)"
                />
                <span class="seat-number">${seat.seatNumber}</span>
            </label>
        </div>
    `).join('');
}

function toggleSeat(seatId, seatNumber, isChecked) {
    console.log('Toggle seat:', seatId, seatNumber, isChecked);
    
    if (isChecked) {
        if (!selectedSeats.includes(seatId)) {
            selectedSeats.push(seatId);
        }
    } else {
        selectedSeats = selectedSeats.filter(id => id !== seatId);
    }

    console.log('Selected seats:', selectedSeats);
    updateBeginReservationButton();
}

function updateBeginReservationButton() {
    const btn = document.getElementById('beginReservationBtn');
    btn.disabled = selectedSeats.length === 0;
    
    if (selectedSeats.length > 0) {
        btn.textContent = `Begin Reservation (${selectedSeats.length} seat${selectedSeats.length > 1 ? 's' : ''})`;
    } else {
        btn.textContent = 'Begin Reservation';
    }
}

function setupBeginReservationButton() {
    document.getElementById('beginReservationBtn').addEventListener('click', async () => {
        if (selectedSeats.length === 0) {
            alert('Please select at least one seat.');
            return;
        }

        await beginReservation();
    });
}

async function beginReservation() {
    const btn = document.getElementById('beginReservationBtn');
    const errorEl = document.getElementById('seatsError');
    
    try {
        btn.disabled = true;
        btn.textContent = 'Processing...';
        errorEl.style.display = 'none';

        console.log('Beginning reservation with seats:', selectedSeats);
        console.log('Request URL:', `${API_BASE_URL}${API_ENDPOINTS.beginReservation}`);
        console.log('Request body:', selectedSeats);

        const response = await fetch(`${API_BASE_URL}${API_ENDPOINTS.beginReservation}`, {
            method: 'POST',
            credentials: 'include',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            },
            body: JSON.stringify(selectedSeats)
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

        if (result.isSuccess) {
            console.log('Reservation started successfully');
            // Save selected seat IDs and flight ID for the next page
            sessionStorage.setItem('lockedSeatIds', JSON.stringify(selectedSeats));
            sessionStorage.setItem('reservationFlightId', flightData.id);
            // Redirect to passengers page
            window.location.href = 'passengers.html';
        } else {
            const errorMsg = result.errorMessage || 'Failed to begin reservation';
            console.error('API error:', errorMsg);
            console.error('Error code:', result.code);
            throw new Error(errorMsg);
        }

    } catch (error) {
        console.error('Begin reservation error:', error);
        errorEl.textContent = `Error: ${error.message}`;
        errorEl.style.display = 'block';
        btn.disabled = false;
        updateBeginReservationButton();
    }
}

function formatDate(dateString) {
    const date = new Date(dateString);
    return date.toLocaleString('en-US', {
        year: 'numeric',
        month: '2-digit',
        day: '2-digit',
        hour: '2-digit',
        minute: '2-digit'
    });
}
