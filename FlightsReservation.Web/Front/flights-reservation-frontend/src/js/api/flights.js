// This file contains functions specifically for handling flight-related API requests.

const API_BASE_URL = 'http://localhost:5000/api'; // Adjust the port as necessary

// Function to fetch available flights
export const fetchAvailableFlights = async (searchParams) => {
    try {
        const response = await fetch(`${API_BASE_URL}/flights`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(searchParams),
        });

        if (!response.ok) {
            throw new Error('Failed to fetch flights');
        }

        const data = await response.json();
        return data;
    } catch (error) {
        console.error('Error fetching flights:', error);
        throw error;
    }
};

// Function to fetch flight details by ID
export const fetchFlightDetails = async (flightId) => {
    try {
        const response = await fetch(`${API_BASE_URL}/flights/${flightId}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
            },
        });

        if (!response.ok) {
            throw new Error('Failed to fetch flight details');
        }

        const data = await response.json();
        return data;
    } catch (error) {
        console.error('Error fetching flight details:', error);
        throw error;
    }
};