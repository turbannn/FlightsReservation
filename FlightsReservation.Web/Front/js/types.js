/**
 * @typedef {Object} FlightSearchRequest
 * @property {string} departureLocation - Місце відправлення
 * @property {string} arrivalLocation - Місце прибуття
 * @property {string} departureDate - Дата відправлення (ISO 8601 format)
 */

/**
 * @typedef {Object} FlightSearchWithReturnRequest
 * @property {string} departureLocation - Місце відправлення
 * @property {string} arrivalLocation - Місце прибуття
 * @property {string} departureDate - Дата відправлення (ISO 8601 format)
 * @property {string} returnDate - Дата повернення (ISO 8601 format)
 */

/**
 * @typedef {Object} FlightUserReadDto
 * @property {string} id - ID рейсу (GUID)
 * @property {string} departureLocation - Місце відправлення
 * @property {string} arrivalLocation - Місце прибуття
 * @property {string} departureDate - Дата відправлення
 * @property {string} arrivalDate - Дата прибуття
 * @property {number} price - Ціна квитка
 * @property {string} company - Авіакомпанія
 * @property {number} availableSeats - Кількість вільних місць
 */

/**
 * @typedef {Object} FlightReservationPagedResult
 * @property {FlightUserReadDto[]} items - Список рейсів
 * @property {number} pageNumber - Номер поточної сторінки
 * @property {number} pageSize - Розмір сторінки
 * @property {number} totalPages - Загальна кількість сторінок
 * @property {number} totalCount - Загальна кількість елементів
 * @property {boolean} hasPreviousPage - Чи є попередня сторінка
 * @property {boolean} hasNextPage - Чи є наступна сторінка
 */

/**
 * @typedef {Object} FlightReservationResult
 * @template T
 * @property {boolean} isSuccess - Чи успішний запит
 * @property {T} value - Значення результату
 * @property {string} message - Повідомлення
 * @property {number} statusCode - HTTP статус код
 */

/**
 * @typedef {Object} UserCreateDto
 * @property {string} name - Ім'я
 * @property {string} surname - Прізвище
 * @property {string} email - Email
 * @property {string} login - Логін
 * @property {string} password - Пароль
 */

/**
 * @typedef {Object} UserReadDto
 * @property {string} id - ID користувача (GUID)
 * @property {string} name - Ім'я
 * @property {string} surname - Прізвище
 * @property {string} email - Email
 * @property {string} login - Логін
 * @property {number} money - Баланс
 * @property {string} role - Роль (User/Admin)
 */

/**
 * @typedef {Object} SeatReadDto
 * @property {string} id - ID місця (GUID)
 * @property {string} seatNumber - Номер місця
 * @property {string} flightId - ID рейсу (GUID)
 * @property {boolean} isLocked - Чи заблоковане місце
 */

/**
 * @typedef {Object} PassengerReadDto
 * @property {string} id - ID пасажира (GUID)
 * @property {string} name - Ім'я
 * @property {string} surname - Прізвище
 * @property {string} passportNumber - Номер паспорта
 */

/**
 * @typedef {Object} ReservationUserReadDto
 * @property {string} id - ID бронювання (GUID)
 * @property {string} reservationDate - Дата бронювання
 * @property {FlightUserReadDto} flight - Інформація про рейс
 * @property {SeatReadDto} seat - Інформація про місце
 * @property {PassengerReadDto} passenger - Інформація про пасажира
 */

/**
 * @typedef {Object} TotalUserReadDto
 * @property {string} id - ID користувача (GUID)
 * @property {string} name - Ім'я
 * @property {string} surname - Прізвище
 * @property {string} email - Email
 * @property {string} login - Логін
 * @property {number} money - Баланс
 * @property {string} role - Роль (User/Admin)
 * @property {ReservationUserReadDto[]} reservations - Список бронювань
 */

// Експортуємо типи для JSDoc
export {};
