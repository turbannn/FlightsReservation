const API_BASE_URL = 'https://localhost:7293'; // Замініть на ваш порт

const API_ENDPOINTS = {
    // Flights
    searchFlights: '/Flights/RequestFlightsPage',
    searchFlightsWithReturn: '/Flights/RequestFlightsPageWithReturn',
    
    // Users
    getUserProfile: '/Users/GetUserProfile',
    login: '/Users/CommitLogin',
    register: '/Users/CommitRegistration',
    logout: '/Users/CommitLogout'
};