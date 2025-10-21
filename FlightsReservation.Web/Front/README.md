# FlightsReservation - Frontend

Фронтенд для системи бронювання авіаквитків.

## Структура проєкту

```
Front/
├── index.html          # Головна сторінка з формою пошуку
├── flights.html        # Сторінка результатів пошуку
├── profile.html        # Профіль користувача
├── login.html          # Сторінка входу
├── register.html       # Сторінка реєстрації
├── css/
│   └── style.css      # Стилі
└── js/
    ├── config.js      # Конфігурація API endpoints
    ├── types.js       # TypeScript-подібні типи (JSDoc)
    ├── index.js       # Логіка головної сторінки
    ├── flights.js     # Логіка сторінки результатів
    ├── profile.js     # Логіка профілю
    ├── login.js       # Логіка входу
    └── register.js    # Логіка реєстрації
```

## Налаштування

1. Відкрийте `js/config.js` та змініть `API_BASE_URL` на адресу вашого backend:
   ```javascript
   const API_BASE_URL = 'http://localhost:5000'; // Змініть порт на ваш
   ```

2. Переконайтеся, що backend запущений та доступний.

## API Endpoints

### Flights
- `POST /Flights/RequestFlightsPage` - Пошук рейсів без зворотного рейсу
- `POST /Flights/RequestFlightsPageWithReturn` - Пошук рейсів зі зворотним рейсом

### Users
- `GET /Users/GetUserProfile` - Отримання профілю користувача (авторизований)
- `GET /Users/CommitLogin?login=...&password=...` - Вхід в систему
- `POST /Users/CommitRegistration` - Реєстрація нового користувача

## Типи даних

Всі типи даних описані в `js/types.js` з використанням JSDoc.

### FlightSearchRequest
```javascript
{
    departureLocation: string,
    arrivalLocation: string,
    departureDate: string  // ISO 8601 format
}
```

### FlightSearchWithReturnRequest
```javascript
{
    departureLocation: string,
    arrivalLocation: string,
    departureDate: string,  // ISO 8601 format
    returnDate: string      // ISO 8601 format
}
```

### UserCreateDto
```javascript
{
    name: string,
    surname: string,
    email: string,
    login: string,
    password: string
}
```

## Особливості

### Пошук рейсів
- Якщо користувач вводить дату повернення - використовується endpoint `RequestFlightsPageWithReturn`
- Якщо дата повернення пуста - використовується endpoint `RequestFlightsPage`
- Параметри пошуку зберігаються в `sessionStorage` для збереження стану при переході між сторінками

### Пагінація
- Автоматично приховується, якщо загальна кількість сторінок ≤ 1
- Кнопки "Попередня" та "Наступна" автоматично блокуються на крайніх сторінках

### Аутентифікація
- Токен зберігається в HTTP-only cookie `_t`
- Всі запити до API використовують `credentials: 'include'` для відправки cookies
- При помилці 401 користувач автоматично перенаправляється на сторінку входу

## Запуск

Просто відкрийте `index.html` у браузері або використовуйте веб-сервер (наприклад, Live Server в VS Code).

**Важливо**: Для роботи з API необхідно, щоб backend підтримував CORS для вашого frontend адреси.

## Формати дат

Всі дати очікуються у форматі ISO 8601:
```
2025-10-21T14:30:00
```

HTML5 `<input type="datetime-local">` автоматично генерує дати в правильному форматі.
