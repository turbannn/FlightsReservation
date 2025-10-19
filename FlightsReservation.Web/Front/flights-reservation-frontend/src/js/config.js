const config = {
    apiBaseUrl: 'http://localhost:5000/api', // Base URL for API requests
    endpoints: {
        flights: '/flights',
        reservations: '/reservations',
        seats: '/seats',
        users: '/users',
        login: '/login',
        searchFlightsWithReturn: '/flights/searchWithReturn',
        searchFlights: '/flights/search'
    }
};

export default config;