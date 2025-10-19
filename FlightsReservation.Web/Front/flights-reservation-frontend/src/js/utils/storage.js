// This file contains utility functions for managing local storage.

const storageKey = 'flightReservationData';

export const saveToLocalStorage = (data) => {
    localStorage.setItem(storageKey, JSON.stringify(data));
};

export const getFromLocalStorage = () => {
    const data = localStorage.getItem(storageKey);
    return data ? JSON.parse(data) : null;
};

export const clearLocalStorage = () => {
    localStorage.removeItem(storageKey);
};