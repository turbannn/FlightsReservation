const FlightCard = ({ flight }) => {
    return `
        <div class="flight-card">
            <h3>${flight.flightNumber}</h3>
            <p>Departure: ${flight.departureLocation} at ${flight.departureTime}</p>
            <p>Arrival: ${flight.arrivalLocation} at ${flight.arrivalTime}</p>
            <p>Duration: ${flight.duration} hours</p>
            <p>Price: $${flight.price}</p>
            <button class="btn-book" data-flight-id="${flight.id}">Book Now</button>
        </div>
    `;
};

const renderFlightCards = (flights) => {
    const flightContainer = document.getElementById('flight-container');
    flightContainer.innerHTML = flights.map(flight => FlightCard({ flight })).join('');
};

export { renderFlightCards };