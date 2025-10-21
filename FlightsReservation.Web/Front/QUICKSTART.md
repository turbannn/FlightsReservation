# Швидкий старт

## Кроки для запуску

### 1. Налаштування Backend
```bash
# Перейдіть в папку з проєктом
cd FlightsReservation.Web

# Запустіть backend
dotnet run
```

Backend запуститься на порту, вказаному в `Properties/launchSettings.json` (зазвичай `https://localhost:7293`).

### 2. Налаштування Frontend

Відкрийте `Front/js/config.js` і переконайтеся, що `API_BASE_URL` вказує на правильний порт:

```javascript
const API_BASE_URL = 'https://localhost:7293'; // Ваш порт backend
```

### 3. Запуск Frontend

#### Варіант А: Через Live Server (рекомендовано)
1. Встановіть розширення "Live Server" в VS Code
2. Клікніть правою кнопкою на `Front/index.html`
3. Виберіть "Open with Live Server"

#### Варіант Б: Просто відкрити файл
1. Відкрийте `Front/index.html` у браузері
2. **Примітка**: Можуть бути проблеми з CORS

#### Варіант В: Через Python (якщо встановлений)
```bash
cd Front
python -m http.server 8000
```
Потім відкрийте http://localhost:8000

#### Варіант Г: Через Node.js (якщо встановлений)
```bash
cd Front
npx http-server -p 8000
```
Потім відкрийте http://localhost:8000

### 4. Тестування

#### 4.1 Реєстрація
1. Відкрийте http://localhost:8000/register.html
2. Заповніть форму
3. Натисніть "Зареєструватися"

#### 4.2 Вхід
1. Відкрийте http://localhost:8000/login.html
2. Введіть логін і пароль
3. Натисніть "Увійти"
4. Ви будете перенаправлені на сторінку профілю

#### 4.3 Пошук рейсів
1. Відкрийте http://localhost:8000/index.html (головна сторінка)
2. Заповніть форму пошуку:
   - Звідки: наприклад, "Київ"
   - Куди: наприклад, "Львів"
   - Дата відправлення: виберіть дату в майбутньому
   - Дата повернення: (опціонально)
3. Натисніть "Знайти рейси"
4. Ви будете перенаправлені на сторінку результатів

## Налаштування CORS (якщо потрібно)

Якщо виникають помилки CORS, додайте в `Program.cs` (backend):

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:8000", "http://127.0.0.1:8000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// ... після var app = builder.Build();

app.UseCors("AllowFrontend");
```

## Структура URL

- **Головна**: `/index.html`
- **Результати пошуку**: `/flights.html`
- **Реєстрація**: `/register.html`
- **Вхід**: `/login.html`
- **Профіль**: `/profile.html`

## Перевірка через DevTools

### Перевірка запиту пошуку рейсів
1. Відкрийте DevTools (F12)
2. Перейдіть на вкладку "Network"
3. Виконайте пошук рейсів
4. Знайдіть запит `RequestFlightsPage` або `RequestFlightsPageWithReturn`
5. Перевірте:
   - **Method**: POST
   - **Request URL**: містить `?page=1&size=10`
   - **Request Headers**: `Content-Type: application/json`
   - **Request Payload**: JSON з даними пошуку

### Перевірка авторизації
1. Виконайте вхід
2. Знайдіть запит `CommitLogin`
3. Перевірте в Response Headers наявність `Set-Cookie: _t=...`
4. Перейдіть на вкладку "Application" → "Cookies"
5. Переконайтеся, що cookie `_t` створено

## Можливі проблеми

### 1. Помилка "Required parameter was not provided from body"
**Причина**: Дані не відправляються в body запиту.
**Рішення**: Перевірте, що запит містить `Content-Type: application/json` і `body: JSON.stringify(data)`.

### 2. Помилка CORS
**Причина**: Backend не дозволяє запити з вашого домену.
**Рішення**: Додайте CORS policy в `Program.cs` (див. вище).

### 3. Cookie не зберігається
**Причина**: `credentials: 'include'` не вказано або домени не збігаються.
**Рішення**: Переконайтеся, що:
- Всі запити містять `credentials: 'include'`
- Backend і frontend на одному домені (або CORS налаштовано правильно)

### 4. Дати неправильного формату
**Причина**: Браузер не підтримує `<input type="datetime-local">`.
**Рішення**: Використовуйте сучасний браузер (Chrome, Firefox, Edge).

## Тестові дані

Для тестування можна використати файл `test-examples.js` - відкрийте його в DevTools Console.

Приклад:
```javascript
// В консолі браузера
const searchRequest = {
    departureLocation: "Київ",
    arrivalLocation: "Львів",
    departureDate: "2025-10-25T10:00:00"
};

fetch('https://localhost:7293/Flights/RequestFlightsPage?page=1&size=10', {
    method: 'POST',
    credentials: 'include',
    headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json'
    },
    body: JSON.stringify(searchRequest)
})
    .then(res => res.json())
    .then(data => console.log(data));
```
