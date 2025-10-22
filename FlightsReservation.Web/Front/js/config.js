const API_BASE_URL = 'https://localhost:7293';

const API_ENDPOINTS = {
    // Flights
    searchFlights: '/Flights/RequestFlightsPage',
    searchFlightsWithReturn: '/Flights/RequestFlightsPageWithReturn',
    getFlightById: '/Flights/GetFlight',
    
    // Users
    getUserProfile: '/Users/GetUserProfile',
    login: '/Users/CommitLogin',
    COMMIT_LOGIN: '/Users/CommitLogin',
    register: '/Users/CommitRegistration',
    logout: '/Users/CommitLogout',
    
    // Reservations
    beginReservation: '/Reservations/BeginReservation',
    commitReservation: '/Reservations/CommitReservation',
    
    // Payments
    createPayment: '/Payments/CreatePayment'
};