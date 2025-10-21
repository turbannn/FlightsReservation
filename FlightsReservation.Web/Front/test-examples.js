// Приклад об'єктів для тестування API

// 1. FlightSearchRequest (без зворотного рейсу)
const searchRequest = {
    departureLocation: "Київ",
    arrivalLocation: "Львів",
    departureDate: "2025-10-25T10:00:00"
};

// 2. FlightSearchWithReturnRequest (зі зворотним рейсом)
const searchWithReturnRequest = {
    departureLocation: "Київ",
    arrivalLocation: "Львів",
    departureDate: "2025-10-25T10:00:00",
    returnDate: "2025-10-28T15:00:00"
};

// 3. UserCreateDto (реєстрація)
const newUser = {
    name: "Іван",
    surname: "Петренко",
    email: "ivan.petrenko@example.com",
    login: "ivanp",
    password: "SecurePassword123"
};

// Приклади запитів:

// Пошук рейсів без повернення
fetch('http://localhost:5000/Flights/RequestFlightsPage?page=1&size=10', {
    method: 'POST',
    credentials: 'include',
    headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json'
    },
    body: JSON.stringify(searchRequest)
})
    .then(res => res.json())
    .then(data => console.log('Flights:', data))
    .catch(err => console.error('Error:', err));

// Пошук рейсів з поверненням
fetch('http://localhost:5000/Flights/RequestFlightsPageWithReturn?page=1&size=10', {
    method: 'POST',
    credentials: 'include',
    headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json'
    },
    body: JSON.stringify(searchWithReturnRequest)
})
    .then(res => res.json())
    .then(data => console.log('Flights with return:', data))
    .catch(err => console.error('Error:', err));

// Реєстрація
fetch('http://localhost:5000/Users/CommitRegistration', {
    method: 'POST',
    headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json'
    },
    body: JSON.stringify(newUser)
})
    .then(res => res.json())
    .then(data => console.log('Registration:', data))
    .catch(err => console.error('Error:', err));

// Вхід
fetch('http://localhost:5000/Users/CommitLogin?login=ivanp&password=SecurePassword123', {
    method: 'GET',
    credentials: 'include',
    headers: {
        'Accept': 'application/json'
    }
})
    .then(res => res.json())
    .then(data => console.log('Login:', data))
    .catch(err => console.error('Error:', err));

// Отримання профілю (потрібна авторизація)
fetch('http://localhost:5000/Users/GetUserProfile', {
    method: 'GET',
    credentials: 'include',
    headers: {
        'Accept': 'application/json'
    }
})
    .then(res => res.json())
    .then(data => console.log('Profile:', data))
    .catch(err => console.error('Error:', err));
