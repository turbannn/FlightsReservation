/// <reference path="./types.js" />

/**
 * Обробка форми пошуку рейсів
 */
document.getElementById('flightSearchForm').addEventListener('submit', async (e) => {
    e.preventDefault();

    const departureLocation = document.getElementById('departureLocation').value;
    const arrivalLocation = document.getElementById('arrivalLocation').value;
    const departureDate = document.getElementById('departureDate').value;
    const returnDate = document.getElementById('returnDate').value;

    /** @type {FlightSearchWithReturnRequest} */
    const searchData = {
        departureLocation,
        arrivalLocation,
        departureDate,
        returnDate: returnDate || null
    };

    // Зберігаємо параметри пошуку
    sessionStorage.setItem('searchParams', JSON.stringify(searchData));
    sessionStorage.setItem('currentPage', '1');

    // Перенаправляємо на сторінку результатів
    window.location.href = 'flights.html';
});