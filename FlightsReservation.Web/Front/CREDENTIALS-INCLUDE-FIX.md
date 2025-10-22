# Проблема: Cookies не працювали з login.html

## 🔴 Проблема

Коли логін виконувався через `test-login-cookie.html` - cookies працювали ✅  
Але коли через `login.html` - cookies НЕ працювали ❌

## 🔍 Причина

В файлі `login.js` був **закоментований** параметр `credentials: 'include'`:

```javascript
const response = await fetch(`${API_BASE_URL}${API_ENDPOINTS.login}?${queryParams}`, {
    method: 'GET',
    //credentials: 'include',  // ← ЗАКОМЕНТОВАНО!
    headers: {
        'Accept': 'application/json'
    }
});
```

## ⚠️ Чому це Критично?

### Без `credentials: 'include'`:
- ❌ Браузер НЕ відправляє cookies в запиті
- ❌ Браузер НЕ зберігає cookies з відповіді
- ❌ Cross-origin запити не працюють з cookies
- ❌ Set-Cookie header ігнорується

### З `credentials: 'include'`:
- ✅ Браузер відправляє cookies в запиті
- ✅ Браузер зберігає cookies з відповіді  
- ✅ Cross-origin запити працюють з cookies
- ✅ Set-Cookie header обробляється

## ✅ Рішення

Розкоментував `credentials: 'include'` в `login.js`:

```javascript
const response = await fetch(`${API_BASE_URL}${API_ENDPOINTS.login}?${queryParams}`, {
    method: 'GET',
    credentials: 'include',  // ← ТЕПЕР АКТИВНО!
    headers: {
        'Accept': 'application/json'
    }
});
```

## 📋 Перевірка Всіх Файлів

Переконався, що `credentials: 'include'` є у всіх потрібних місцях:

### ✅ Файли з `credentials: 'include'`:
- **login.js** (тепер виправлено) ✅
- **profile.js** (2 запити) ✅
- **flights.js** ✅
- **seats.js** (2 запити) ✅
- **passengers.js** ✅

### ℹ️ Файли БЕЗ `credentials: 'include'` (не потрібно):
- **register.js** - реєстрація, cookies ще немає
- **index.js** - просто sessionStorage, без API запитів
- **confirmation.js** - тільки читання з sessionStorage

## 🎯 Правило

**ЗАВЖДИ** використовуйте `credentials: 'include'` для запитів, що потребують authentication:

```javascript
fetch(url, {
    method: 'GET/POST/PUT/DELETE',
    credentials: 'include',  // ← ОБОВ'ЯЗКОВО для auth!
    headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json'
    },
    body: JSON.stringify(data) // якщо потрібно
})
```

## 🧪 Тестування

### Як перевірити чи працює:

1. **Очистіть cookies:**
   - DevTools (F12) → Application → Cookies → Clear all

2. **Виконайте логін через login.html**

3. **Перевірте cookies:**
   - DevTools → Application → Cookies → https://localhost:7293
   - Має бути cookie `_t` з JWT токеном

4. **Перевірте консоль:**
   ```
   Login response status: 200
   Login response ok: true
   Cookies after login: (може бути пусто, бо HttpOnly)
   Login successful, redirecting to profile...
   ```

5. **Перевірте backend консоль:**
   ```
   Generated Token: eyJhbGci...
   Request Origin: http://localhost:63342
   Cookie set with options: HttpOnly=true, Secure=false, SameSite=Lax
   ```

## 💡 Додаткова Інформація

### Fetch Credentials Options:

- **`omit`** (default) - НІКОЛИ не відправляти/приймати cookies
- **`same-origin`** - Тільки для same-origin запитів
- **`include`** - ЗАВЖДИ відправляти/приймати cookies (потрібно для cross-origin)

### Для Cross-Origin (різні порти/домени):
- Frontend: `http://localhost:63342`
- Backend: `https://localhost:7293`
- **ОБОВ'ЯЗКОВО**: `credentials: 'include'`

### Для Same-Origin:
- Frontend і Backend на одному домені/порті
- Можна використовувати `same-origin`

## 🚨 Типові Помилки

### ❌ Помилка 1: Закоментований credentials
```javascript
//credentials: 'include',  // НЕ ПРАЦЮЄ!
```

### ❌ Помилка 2: Неправильне значення
```javascript
credentials: 'omit',  // Блокує cookies!
credentials: 'same-origin',  // Не працює для cross-origin!
```

### ❌ Помилка 3: Відсутність параметра
```javascript
fetch(url, {
    method: 'GET'
    // credentials відсутній - за замовчуванням 'omit'!
})
```

### ✅ Правильно:
```javascript
fetch(url, {
    method: 'GET',
    credentials: 'include'  // ✅ ТАК!
})
```

---

**Після цього виправлення login.html працює так само добре, як test-login-cookie.html!** 🎉

## 📚 Джерела
- [MDN: Fetch API - credentials](https://developer.mozilla.org/en-US/docs/Web/API/fetch#credentials)
- [MDN: Request.credentials](https://developer.mozilla.org/en-US/docs/Web/API/Request/credentials)
