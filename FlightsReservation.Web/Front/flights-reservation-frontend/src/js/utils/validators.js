function validateFlightSearchRequest(request) {
    const { departureDate, returnDate, origin, destination } = request;

    if (!origin || !destination) {
        return { valid: false, message: "Origin and destination are required." };
    }

    if (new Date(departureDate) < new Date()) {
        return { valid: false, message: "Departure date must be in the future." };
    }

    if (returnDate && new Date(returnDate) < new Date(departureDate)) {
        return { valid: false, message: "Return date must be after departure date." };
    }

    return { valid: true, message: "Request is valid." };
}

function validateLoginRequest(request) {
    const { username, password } = request;

    if (!username || !password) {
        return { valid: false, message: "Username and password are required." };
    }

    if (password.length < 6) {
        return { valid: false, message: "Password must be at least 6 characters long." };
    }

    return { valid: true, message: "Login request is valid." };
}

function validateReservationRequest(request) {
    const { flightId, passengerDetails } = request;

    if (!flightId) {
        return { valid: false, message: "Flight ID is required." };
    }

    if (!passengerDetails || passengerDetails.length === 0) {
        return { valid: false, message: "At least one passenger detail is required." };
    }

    return { valid: true, message: "Reservation request is valid." };
}

export { validateFlightSearchRequest, validateLoginRequest, validateReservationRequest };