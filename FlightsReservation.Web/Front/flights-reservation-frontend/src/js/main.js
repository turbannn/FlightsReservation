// This is the main JavaScript file that initializes the application and handles global functionality.

document.addEventListener('DOMContentLoaded', () => {
    // Initialize the application
    console.log("Application initialized");

    // Add event listeners for form submissions and other interactions
    const flightSearchForm = document.getElementById('flightSearchForm');
    if (flightSearchForm) {
        flightSearchForm.addEventListener('submit', handleFlightSearch);
    }
});

function handleFlightSearch(event) {
    event.preventDefault();

    const departureDate = document.getElementById('departureDate').value;
    const returnDate = document.getElementById('returnDate').value;
    const origin = document.getElementById('origin').value;
    const destination = document.getElementById('destination').value;

    const requestData = {
        origin,
        destination,
        departureDate,
        returnDate: returnDate || null
    };

    if (returnDate) {
        requestFlightsPageWithReturn(requestData);
    } else {
        requestFlightsPage(requestData);
    }
}

function requestFlightsPageWithReturn(data) {
    // Logic to send request to the backend for flights with return date
    console.log("Requesting flights with return:", data);
    // Example API call
    // fetch(`${API_BASE_URL}/flights/with-return`, {
    //     method: 'POST',
    //     headers: {
    //         'Content-Type': 'application/json'
    //     },
    //     body: JSON.stringify(data)
    // })
    // .then(response => response.json())
    // .then(data => {
    //     console.log("Flights with return data:", data);
    // })
    // .catch(error => console.error("Error fetching flights with return:", error));
}

function requestFlightsPage(data) {
    // Logic to send request to the backend for flights without return date
    console.log("Requesting flights without return:", data);
    // Example API call
    // fetch(`${API_BASE_URL}/flights`, {
    //     method: 'POST',
    //     headers: {
    //         'Content-Type': 'application/json'
    //     },
    //     body: JSON.stringify(data)
    // })
    // .then(response => response.json())
    // .then(data => {
    //     console.log("Flights data:", data);
    // })
    // .catch(error => console.error("Error fetching flights:", error));
}