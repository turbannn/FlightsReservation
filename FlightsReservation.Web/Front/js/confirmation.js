document.addEventListener('DOMContentLoaded', () => {
    loadConfirmation();
});

function loadConfirmation() {
    const errorEl = document.getElementById('confirmationError');
    const contentEl = document.getElementById('confirmationContent');

    // Get reservation result from session storage
    const resultJson = sessionStorage.getItem('reservationResult');
    const passengersJson = sessionStorage.getItem('reservationPassengers');

    if (!resultJson) {
        console.error('No reservation result found');
        errorEl.textContent = 'No reservation found. Please complete a reservation first.';
        errorEl.style.display = 'block';
        return;
    }

    try {
        const result = JSON.parse(resultJson);
        const passengers = passengersJson ? JSON.parse(passengersJson) : [];

        console.log('Reservation result:', result);
        console.log('Passengers:', passengers);

        displayReservationInfo(result);
        displayPassengers(passengers);

        contentEl.style.display = 'block';

        // Clear the stored data after displaying
        sessionStorage.removeItem('reservationResult');
        sessionStorage.removeItem('reservationPassengers');

    } catch (error) {
        console.error('Error loading confirmation:', error);
        errorEl.textContent = `Error: ${error.message}`;
        errorEl.style.display = 'block';
    }
}

function displayReservationInfo(result) {
    const infoEl = document.getElementById('reservationInfo');
    
    // The result.value should contain the reservation ID or relevant data
    infoEl.innerHTML = `
        <div class="confirmation-info-grid">
            <div class="confirmation-info-item">
                <strong>Status:</strong>
                <span class="status-success">Confirmed</span>
            </div>
            ${result.value ? `
                <div class="confirmation-info-item">
                    <strong>Reservation ID:</strong>
                    <span>${result.value}</span>
                </div>
            ` : ''}
            <div class="confirmation-info-item">
                <strong>Confirmation Time:</strong>
                <span>${new Date().toLocaleString('en-US', {
                    year: 'numeric',
                    month: 'long',
                    day: 'numeric',
                    hour: '2-digit',
                    minute: '2-digit'
                })}</span>
            </div>
        </div>
        <div class="confirmation-note">
            <p><strong>Important:</strong> Please check your email for confirmation details and booking information. 
            You can also view your reservation in your profile.</p>
        </div>
    `;
}

function displayPassengers(passengers) {
    const passengersListEl = document.getElementById('passengersList');
    
    if (!passengers || passengers.length === 0) {
        passengersListEl.innerHTML = '<p>No passenger information available.</p>';
        return;
    }

    passengersListEl.innerHTML = passengers.map((passenger, index) => `
        <div class="passenger-card">
            <h3>Passenger ${index + 1}</h3>
            <div class="passenger-info-grid">
                <div class="passenger-info-item">
                    <strong>Name:</strong> ${passenger.FirstName} ${passenger.LastName}
                </div>
                <div class="passenger-info-item">
                    <strong>Passport:</strong> ${passenger.PassportNumber}
                </div>
                <div class="passenger-info-item">
                    <strong>Phone:</strong> ${passenger.PhoneNumber}
                </div>
                <div class="passenger-info-item">
                    <strong>Email:</strong> ${passenger.Email}
                </div>
            </div>
        </div>
    `).join('');
}
