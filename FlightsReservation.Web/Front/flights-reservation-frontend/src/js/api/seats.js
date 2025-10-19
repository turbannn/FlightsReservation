// This file contains functions for handling seat-related API requests.

const API_BASE_URL = 'http://localhost:5000/api'; // Adjust the port as necessary

// Function to get available seats for a specific flight
export const getAvailableSeats = async (flightId) => {
    try {
        const response = await fetch(`${API_BASE_URL}/seats/available/${flightId}`);
        if (!response.ok) {
            throw new Error('Failed to fetch available seats');
        }
        return await response.json();
    } catch (error) {
        console.error('Error fetching available seats:', error);
        throw error;
    }
};

// Function to reserve a seat
export const reserveSeat = async (seatData) => {
    try {
        const response = await fetch(`${API_BASE_URL}/seats/reserve`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(seatData),
        });
        if (!response.ok) {
            throw new Error('Failed to reserve seat');
        }
        return await response.json();
    } catch (error) {
        console.error('Error reserving seat:', error);
        throw error;
    }
};

// Function to cancel a seat reservation
export const cancelReservation = async (reservationId) => {
    try {
        const response = await fetch(`${API_BASE_URL}/seats/cancel/${reservationId}`, {
            method: 'DELETE',
        });
        if (!response.ok) {
            throw new Error('Failed to cancel reservation');
        }
        return await response.json();
    } catch (error) {
        console.error('Error canceling reservation:', error);
        throw error;
    }
};