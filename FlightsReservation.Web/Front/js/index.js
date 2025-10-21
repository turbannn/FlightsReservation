document.getElementById('flightSearchForm').addEventListener('submit', async (e) => {
    e.preventDefault();

    const departureCity = document.getElementById('departureLocation').value;
    const arrivalCity = document.getElementById('arrivalLocation').value;
    const departureDate = document.getElementById('departureDate').value;
    const returnDate = document.getElementById('returnDate').value;

    const searchData = {
        DepartureCity: departureCity,
        ArrivalCity: arrivalCity,
        DepartureDate: departureDate,
        ReturnDate: returnDate || null
    };

    console.log('Search data:', searchData);

    sessionStorage.setItem('searchParams', JSON.stringify(searchData));
    sessionStorage.setItem('currentPage', '1');

    window.location.href = 'flights.html';
});