let seatIds = [];
let flightId = null;

document.addEventListener('DOMContentLoaded', () => {
    loadPassengerForms();
    setupForm();
});

function loadPassengerForms() {
    const errorEl = document.getElementById('passengersError');
    const formsContainer = document.getElementById('passengerForms');

    // Get locked seat IDs from session storage
    const lockedSeatsJson = sessionStorage.getItem('lockedSeatIds');
    flightId = sessionStorage.getItem('reservationFlightId');

    if (!lockedSeatsJson || !flightId) {
        console.error('No seat IDs or flight ID found');
        errorEl.textContent = 'No seats selected. Please return to seat selection.';
        errorEl.style.display = 'block';
        return;
    }

    try {
        seatIds = JSON.parse(lockedSeatsJson);
        console.log('Locked seat IDs:', seatIds);
        console.log('Flight ID:', flightId);

        if (!Array.isArray(seatIds) || seatIds.length === 0) {
            throw new Error('Invalid seat IDs');
        }

        // Create a form for each seat
        formsContainer.innerHTML = seatIds.map((seatId, index) => `
            <div class="passenger-form-card">
                <h3>Passenger ${index + 1}</h3>
                <div class="form-group">
                    <label for="firstName_${index}">First Name:</label>
                    <input 
                        type="text" 
                        id="firstName_${index}" 
                        name="firstName_${index}" 
                        data-seat-id="${seatId}"
                        required
                    />
                </div>

                <div class="form-group">
                    <label for="lastName_${index}">Last Name:</label>
                    <input 
                        type="text" 
                        id="lastName_${index}" 
                        name="lastName_${index}" 
                        required
                    />
                </div>

                <div class="form-group">
                    <label for="passportNumber_${index}">Passport Number:</label>
                    <input 
                        type="text" 
                        id="passportNumber_${index}" 
                        name="passportNumber_${index}" 
                        required
                    />
                </div>

                <div class="form-group">
                    <label for="phoneNumber_${index}">Phone Number:</label>
                    <input 
                        type="tel" 
                        id="phoneNumber_${index}" 
                        name="phoneNumber_${index}" 
                        required
                    />
                </div>

                <div class="form-group">
                    <label for="email_${index}">Email:</label>
                    <input 
                        type="email" 
                        id="email_${index}" 
                        name="email_${index}" 
                        required
                    />
                </div>
            </div>
        `).join('');

    } catch (error) {
        console.error('Error loading passenger forms:', error);
        errorEl.textContent = `Error: ${error.message}`;
        errorEl.style.display = 'block';
    }
}

function setupForm() {
    document.getElementById('passengersForm').addEventListener('submit', async (e) => {
        e.preventDefault();
        await commitReservation();
    });
}

async function commitReservation() {
    const btn = document.getElementById('commitReservationBtn');
    const errorEl = document.getElementById('passengersError');

    try {
        btn.disabled = true;
        btn.textContent = 'Processing...';
        errorEl.style.display = 'none';

        // Collect passenger data
        const passengers = seatIds.map((seatId, index) => {
            return {
                FirstName: document.getElementById(`firstName_${index}`).value.trim(),
                LastName: document.getElementById(`lastName_${index}`).value.trim(),
                PassportNumber: document.getElementById(`passportNumber_${index}`).value.trim(),
                PhoneNumber: document.getElementById(`phoneNumber_${index}`).value.trim(),
                Email: document.getElementById(`email_${index}`).value.trim(),
                SeatId: seatId
            };
        });

        console.log('Passengers data:', passengers);

        // Create reservation DTO matching ReservationCreateDto structure
        const reservationDto = {
            FlightId: flightId,
            Passengers: passengers
        };

        console.log('Reservation DTO:', reservationDto);
        console.log('Request URL:', `${API_BASE_URL}${API_ENDPOINTS.commitReservation}`);

        const response = await fetch(`${API_BASE_URL}${API_ENDPOINTS.commitReservation}`, {
            method: 'POST',
            credentials: 'include',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            },
            body: JSON.stringify(reservationDto)
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
            console.log('Reservation completed successfully');
            // Save reservation data for confirmation page
            sessionStorage.setItem('reservationResult', JSON.stringify(result));
            sessionStorage.setItem('reservationPassengers', JSON.stringify(passengers));
            // Clear temporary data
            sessionStorage.removeItem('lockedSeatIds');
            sessionStorage.removeItem('reservationFlightId');
            sessionStorage.removeItem('selectedFlightId');
            // Redirect to confirmation page
            window.location.href = 'confirmation.html';
        } else {
            const errorMsg = result.errorMessage || 'Failed to complete reservation';
            console.error('API error:', errorMsg);
            console.error('Error code:', result.code);
            throw new Error(errorMsg);
        }

    } catch (error) {
        console.error('Commit reservation error:', error);
        errorEl.textContent = `Error: ${error.message}`;
        errorEl.style.display = 'block';
        btn.disabled = false;
        btn.textContent = 'Complete Reservation';
    }
}
