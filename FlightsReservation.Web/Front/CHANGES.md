# Зміни у фронтенді

## Основні виправлення

### 1. Виправлено запити до API
**Проблема**: Backend очікував дані у `body` запиту, а фронтенд відправляв їх через query parameters.

**Рішення**: 
- Змінено метод відправки даних пошуку з query parameters на POST body
- Параметри пагінації (`page`, `size`) залишились у query string
- Дані пошуку (`departureLocation`, `arrivalLocation`, `departureDate`, `returnDate`) тепер відправляються в body

**Код у flights.js**:
```javascript
// Формуємо body request
const requestBody = {
    departureLocation: searchParams.departureLocation,
    arrivalLocation: searchParams.arrivalLocation,
    departureDate: searchParams.departureDate
};

// Додаємо returnDate тільки якщо він є
if (hasReturnDate) {
    requestBody.returnDate = searchParams.returnDate;
}

// Query параметри для пагінації
const queryParams = new URLSearchParams({
    page: currentPage ?? 1,
    size: 10
});

const response = await fetch(`${API_BASE_URL}${endpoint}?${queryParams}`, {
    method: 'POST',
    credentials: 'include',
    headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json'
    },
    body: JSON.stringify(requestBody)
});
```

### 2. Створено types.js
Файл з TypeScript-подібними типами для всіх DTO та запитів:
- `FlightSearchRequest`
- `FlightSearchWithReturnRequest`
- `FlightUserReadDto`
- `FlightReservationPagedResult`
- `UserCreateDto`
- `UserReadDto`
- `TotalUserReadDto`
- `ReservationUserReadDto`
- `PassengerReadDto`
- `SeatReadDto`

### 3. Додано JSDoc коментарі
Всі JavaScript файли тепер містять:
- `/// <reference path="./types.js" />` для IntelliSense
- JSDoc коментарі для функцій з типізацією параметрів
- Типізацію для об'єктів (наприклад, `/** @type {UserCreateDto} */`)

### 4. Оновлено HTML файли
Всі HTML файли тепер підключають `types.js` перед іншими скриптами:
```html
<script src="js/types.js"></script>
<script src="js/config.js"></script>
<script src="js/[page].js"></script>
```

## Файли

### Створені нові файли:
- `js/types.js` - TypeScript-подібні типи
- `test-examples.js` - Приклади запитів для тестування
- `README.md` - Документація (оновлена)
- `CHANGES.md` - Цей файл

### Змінені файли:
- `js/flights.js` - Виправлено запити до API
- `js/index.js` - Додано типи
- `js/profile.js` - Додано типи
- `js/login.js` - Додано типи
- `js/register.js` - Додано типи
- `index.html` - Додано types.js
- `flights.html` - Додано types.js
- `profile.html` - Додано types.js
- `login.html` - Додано types.js
- `register.html` - Додано types.js

## Перевірка

Для перевірки роботи:
1. Переконайтеся, що backend запущений
2. Відкрийте `index.html` у браузері
3. Заповніть форму пошуку
4. Перевірте в DevTools → Network, що запити відправляються правильно:
   - Method: POST
   - Request Payload містить JSON з даними пошуку
   - Query string містить page і size

## Backend Requirements

Backend повинен приймати:
- **Query parameters**: `page`, `size`
- **Body**: `FlightSearchRequest` або `FlightSearchWithReturnRequest` (JSON)

Приклад запиту:
```
POST http://localhost:7293/Flights/RequestFlightsPage?page=1&size=10
Content-Type: application/json

{
    "departureLocation": "Київ",
    "arrivalLocation": "Львів",
    "departureDate": "2025-10-25T10:00:00"
}
```
