# Приклади відповідей API

## 1. Успішний пошук рейсів

**Request:**
```
POST http://localhost:7293/Flights/RequestFlightsPage?page=1&size=10
Content-Type: application/json

{
    "departureLocation": "Київ",
    "arrivalLocation": "Львів",
    "departureDate": "2025-10-25T10:00:00"
}
```

**Response:**
```json
{
    "isSuccess": true,
    "value": {
        "items": [
            {
                "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                "departureLocation": "Київ",
                "arrivalLocation": "Львів",
                "departureDate": "2025-10-25T10:00:00",
                "arrivalDate": "2025-10-25T11:30:00",
                "price": 1500,
                "company": "Ukraine International Airlines",
                "availableSeats": 120
            }
        ],
        "pageNumber": 1,
        "pageSize": 10,
        "totalPages": 1,
        "totalCount": 1,
        "hasPreviousPage": false,
        "hasNextPage": false
    },
    "message": null,
    "statusCode": 200
}
```

## 2. Реєстрація користувача

**Request:**
```
POST http://localhost:7293/Users/CommitRegistration
Content-Type: application/json

{
    "name": "Іван",
    "surname": "Петренко",
    "email": "ivan.petrenko@example.com",
    "login": "ivanp",
    "password": "SecurePassword123"
}
```

**Response (успіх):**
```json
{
    "isSuccess": true,
    "value": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "name": "Іван",
        "surname": "Петренко",
        "email": "ivan.petrenko@example.com",
        "login": "ivanp",
        "money": 0,
        "role": "User"
    },
    "message": "User created successfully",
    "statusCode": 201
}
```

**Response (помилка - логін зайнятий):**
```json
{
    "isSuccess": false,
    "value": null,
    "message": "User with this login already exists",
    "statusCode": 400
}
```

## 3. Вхід

**Request:**
```
GET http://localhost:7293/Users/CommitLogin?login=ivanp&password=SecurePassword123
```

**Response (успіх):**
```json
{
    "isSuccess": true,
    "value": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "name": "Іван",
        "surname": "Петренко",
        "email": "ivan.petrenko@example.com",
        "login": "ivanp",
        "money": 0,
        "role": "User"
    },
    "message": "Login successful",
    "statusCode": 200
}
```

**Response Headers:**
```
Set-Cookie: _t=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...; HttpOnly; Secure; SameSite=Strict; Expires=...
```

**Response (помилка):**
```json
{
    "isSuccess": false,
    "value": null,
    "message": "Invalid login or password",
    "statusCode": 401
}
```

## 4. Профіль користувача (авторизований)

**Request:**
```
GET http://localhost:7293/Users/GetUserProfile
Cookie: _t=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Response:**
```json
{
    "isSuccess": true,
    "value": {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "name": "Іван",
        "surname": "Петренко",
        "email": "ivan.petrenko@example.com",
        "login": "ivanp",
        "money": 5000,
        "role": "User",
        "reservations": [
            {
                "id": "7fa85f64-5717-4562-b3fc-2c963f66afa6",
                "reservationDate": "2025-10-20T14:30:00",
                "flight": {
                    "id": "8fa85f64-5717-4562-b3fc-2c963f66afa6",
                    "departureLocation": "Київ",
                    "arrivalLocation": "Львів",
                    "departureDate": "2025-10-25T10:00:00",
                    "arrivalDate": "2025-10-25T11:30:00",
                    "price": 1500,
                    "company": "Ukraine International Airlines",
                    "availableSeats": 119
                },
                "seat": {
                    "id": "9fa85f64-5717-4562-b3fc-2c963f66afa6",
                    "seatNumber": "12A",
                    "flightId": "8fa85f64-5717-4562-b3fc-2c963f66afa6",
                    "isLocked": true
                },
                "passenger": {
                    "id": "afa85f64-5717-4562-b3fc-2c963f66afa6",
                    "name": "Іван",
                    "surname": "Петренко",
                    "passportNumber": "AA1234567"
                }
            }
        ]
    },
    "message": null,
    "statusCode": 200
}
```

**Response (не авторизований):**
```json
{
    "isSuccess": false,
    "value": null,
    "message": "Unauthorized",
    "statusCode": 401
}
```

## 5. Помилки валідації

**Request (некоректні дані):**
```
POST http://localhost:7293/Users/CommitRegistration
Content-Type: application/json

{
    "name": "",
    "surname": "П",
    "email": "invalid-email",
    "login": "ab",
    "password": "123"
}
```

**Response:**
```json
{
    "isSuccess": false,
    "value": null,
    "message": "Validation failed: Name is required. Surname must be at least 2 characters. Email is not valid. Login must be at least 3 characters. Password must be at least 6 characters.",
    "statusCode": 400
}
```

## Типові HTTP статус коди

- `200 OK` - Успішний запит
- `201 Created` - Ресурс створено
- `400 Bad Request` - Некоректні дані
- `401 Unauthorized` - Не авторизований
- `403 Forbidden` - Доступ заборонено
- `404 Not Found` - Ресурс не знайдено
- `500 Internal Server Error` - Помилка сервера
