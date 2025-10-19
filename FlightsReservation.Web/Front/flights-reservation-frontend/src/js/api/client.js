// This file contains functions for making API requests to the backend.

const API_BASE_URL = 'http://localhost:5000/api'; // Adjust the port as necessary

async function fetchData(endpoint, options = {}) {
    try {
        const response = await fetch(`${API_BASE_URL}/${endpoint}`, options);
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        return await response.json();
    } catch (error) {
        console.error('Error fetching data:', error);
        throw error;
    }
}

function get(endpoint) {
    return fetchData(endpoint);
}

function post(endpoint, data) {
    return fetchData(endpoint, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
    });
}

function put(endpoint, data) {
    return fetchData(endpoint, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
    });
}

function del(endpoint) {
    return fetchData(endpoint, {
        method: 'DELETE',
    });
}

export { get, post, put, del };