// This file contains functions for managing reservation-related API requests.

const apiBaseUrl = 'http://localhost:5000/api'; // Adjust the port as necessary

// Function to create a new reservation
async function createReservation(reservationData) {
    const response = await fetch(`${apiBaseUrl}/reservations`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(reservationData),
    });
    return response.json();
}

// Function to get a reservation by ID
async function getReservation(reservationId) {
    const response = await fetch(`${apiBaseUrl}/reservations/${reservationId}`);
    return response.json();
}

// Function to update an existing reservation
async function updateReservation(reservationId, reservationData) {
    const response = await fetch(`${apiBaseUrl}/reservations/${reservationId}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(reservationData),
    });
    return response.json();
}

// Function to delete a reservation
async function deleteReservation(reservationId) {
    const response = await fetch(`${apiBaseUrl}/reservations/${reservationId}`, {
        method: 'DELETE',
    });
    return response.ok;
}

// Function to get all reservations for a user
async function getUserReservations(userId) {
    const response = await fetch(`${apiBaseUrl}/users/${userId}/reservations`);
    return response.json();
}