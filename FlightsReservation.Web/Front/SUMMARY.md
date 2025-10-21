# FlightsReservation Frontend - Фінальна Документація

## 📁 Структура проєкту

```
FlightsReservation.Web/Front/
├── 📄 HTML Сторінки
│   ├── index.html          # Головна сторінка з пошуком рейсів
│   ├── flights.html        # Результати пошуку з пагінацією
│   ├── profile.html        # Профіль користувача
│   ├── login.html          # Сторінка входу
│   └── register.html       # Сторінка реєстрації
│
├── 🎨 Стилі
│   └── css/
│       └── style.css       # Основні стилі
│
├── 💻 JavaScript
│   └── js/
│       ├── config.js       # Конфігурація API (BASE_URL, ENDPOINTS)
│       ├── types.js        # TypeScript-подібні типи (JSDoc)
│       ├── index.js        # Логіка головної сторінки
│       ├── flights.js      # Логіка результатів пошуку
│       ├── profile.js      # Логіка профілю
│       ├── login.js        # Логіка входу
│       └── register.js     # Логіка реєстрації
│
├── 📚 Документація
│   ├── README.md           # Основна документація
│   ├── QUICKSTART.md       # Швидкий старт
│   ├── CHANGES.md          # Список змін
│   └── API-EXAMPLES.md     # Приклади API запитів/відповідей
│
└── 🧪 Тестування
    └── test-examples.js    # Приклади запитів для тестування
```

## ✅ Що реалізовано

### 1. Головна сторінка (index.html)
- ✅ Header з навігацією
- ✅ Форма пошуку рейсів з 4 полями:
  - Місце відправлення
  - Місце прибуття
  - Дата відправлення (обов'язково)
  - Дата повернення (опціонально)
- ✅ Автоматичний вибір endpoint залежно від наявності дати повернення:
  - З датою повернення → `/Flights/RequestFlightsPageWithReturn`
  - Без дати повернення → `/Flights/RequestFlightsPage`
- ✅ Збереження параметрів пошуку в `sessionStorage`

### 2. Сторінка результатів (flights.html)
- ✅ Відображення результатів з `FlightReservationPagedResult`
- ✅ Картки рейсів з інформацією:
  - Маршрут (звідки → куди)
  - Ціна
  - Дата/час відправлення та прибуття
  - Авіакомпанія
  - Кількість вільних місць
- ✅ Пагінація:
  - Кнопки "Попередня" / "Наступна"
  - Інформація про поточну сторінку
  - Автоматичне приховування при 1 сторінці
  - Блокування кнопок на крайніх сторінках
- ✅ Бокова панель з формою пошуку (дублює функціонал головної)
- ✅ Збереження стану при переході між сторінками

### 3. Сторінка профілю (profile.html)
- ✅ Запит на `/Users/GetUserProfile`
- ✅ Відображення `TotalUserReadDto`:
  - Ім'я, прізвище
  - Email, логін
  - Баланс
  - Роль
- ✅ Список бронювань користувача:
  - Інформація про рейс
  - Інформація про місце
  - Інформація про пасажира
  - Дата бронювання
- ✅ Автоматичне перенаправлення на login при 401

### 4. Сторінка реєстрації (register.html)
- ✅ Форма з полями:
  - Ім'я
  - Прізвище
  - Email
  - Логін
  - Пароль
- ✅ POST запит на `/Users/CommitRegistration`
- ✅ Відправка `UserCreateDto`
- ✅ Повідомлення про успіх/помилку
- ✅ Автоматичне перенаправлення на login після успішної реєстрації

### 5. Сторінка входу (login.html)
- ✅ Форма з полями логін і пароль
- ✅ GET запит на `/Users/CommitLogin?login=...&password=...`
- ✅ Збереження токену в HTTP-only cookie
- ✅ Перенаправлення на профіль після успішного входу
- ✅ Відображення помилок

## 🔧 Технічні деталі

### API Запити

#### Пошук рейсів
```javascript
// Request
POST /Flights/RequestFlightsPage?page=1&size=10
Content-Type: application/json

{
    "departureLocation": "Київ",
    "arrivalLocation": "Львів",
    "departureDate": "2025-10-25T10:00:00"
}

// Response
{
    "isSuccess": true,
    "value": {
        "items": [...],
        "pageNumber": 1,
        "totalPages": 5,
        ...
    }
}
```

#### Реєстрація
```javascript
// Request
POST /Users/CommitRegistration
Content-Type: application/json

{
    "name": "Іван",
    "surname": "Петренко",
    "email": "ivan@example.com",
    "login": "ivanp",
    "password": "pass123"
}
```

#### Вхід
```javascript
// Request
GET /Users/CommitLogin?login=ivanp&password=pass123

// Response включає Set-Cookie header з токеном
```

#### Профіль
```javascript
// Request (з cookie)
GET /Users/GetUserProfile
Cookie: _t=...

// Response - TotalUserReadDto
```

### Типізація (types.js)

Всі DTO типізовані з використанням JSDoc:
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

### Авторизація

- Токен зберігається в HTTP-only cookie `_t`
- Всі запити використовують `credentials: 'include'`
- Автоматичне перенаправлення на login при 401

## 🚀 Швидкий запуск

### Backend
```bash
cd FlightsReservation.Web
dotnet run
```

### Frontend
```bash
cd FlightsReservation.Web/Front

# Варіант 1: Live Server (VS Code)
# Right-click на index.html → Open with Live Server

# Варіант 2: Python
python -m http.server 8000

# Варіант 3: Node.js
npx http-server -p 8000
```

### Налаштування
Відкрийте `js/config.js` і перевірте `API_BASE_URL`:
```javascript
const API_BASE_URL = 'https://localhost:7293'; // Ваш порт
```

## 📝 Основні зміни

### Виправлено проблему з API
**Було**: Дані відправлялися через query parameters  
**Стало**: Дані відправляються в body POST запиту

### Додано типізацію
- Створено `types.js` з усіма DTO
- Додано JSDoc коментарі
- Покращено IntelliSense

### Документація
- `README.md` - повна документація
- `QUICKSTART.md` - швидкий старт
- `CHANGES.md` - список змін
- `API-EXAMPLES.md` - приклади API

## 🎯 Сценарії використання

### 1. Пошук рейсів без повернення
1. Відкрити `index.html`
2. Заповнити: звідки, куди, дата відправлення
3. **НЕ** заповнювати дату повернення
4. Натиснути "Знайти рейси"
5. → Запит на `/Flights/RequestFlightsPage`

### 2. Пошук рейсів з поверненням
1. Відкрити `index.html`
2. Заповнити: звідки, куди, дата відправлення, **дата повернення**
3. Натиснути "Знайти рейси"
4. → Запит на `/Flights/RequestFlightsPageWithReturn`

### 3. Перегляд інших сторінок результатів
1. На сторінці результатів натиснути "Наступна"
2. → Новий запит з `page=2`
3. Параметри пошуку зберігаються

### 4. Реєстрація та вхід
1. `register.html` → Заповнити форму → Зареєструватися
2. → Перенаправлення на `login.html`
3. Ввести логін/пароль → Увійти
4. → Перенаправлення на `profile.html`

## 🐛 Відомі обмеження

1. Немає валідації форм на фронтенді (покладаємось на backend)
2. Немає завантаження файлів (аватарів)
3. Немає можливості редагувати профіль
4. Немає пошуку/фільтрації в результатах
5. Немає можливості створити бронювання з інтерфейсу

## 📞 Підтримка

Для питань та помилок дивіться:
- `QUICKSTART.md` - поширені проблеми
- `API-EXAMPLES.md` - формати запитів/відповідей
- `test-examples.js` - приклади для тестування

## ✨ Готово до використання!

Всі файли створені, всі функції реалізовані відповідно до вимог. Backend не змінювався, фронтенд повністю підлаштований під існуючі API endpoints.
