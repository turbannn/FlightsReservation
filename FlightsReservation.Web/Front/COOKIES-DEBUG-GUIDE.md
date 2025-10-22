# Вирішення Проблеми з Cookies (_t) при Login

## 🔴 Проблема

Після логіну через фронтенд (WebStorm), cookie `_t` не додається в браузер.

## 🔍 Причини

### 1. **Cross-Origin Requests (CORS)**
- **Frontend**: `http://localhost:63342` (WebStorm)
- **Backend**: `https://localhost:7293` (ASP.NET Core)
- **Проблема**: Різні порти + різні протоколи (HTTP vs HTTPS)

### 2. **Cookie Security Settings**
Cookies з `HttpOnly=true` не можна побачити через `document.cookie` в JavaScript, але вони мають передаватись автоматично.

### 3. **SameSite Policy**
При cross-origin запитах потрібно правильно налаштувати `SameSite` атрибут.

## ✅ Рішення

### Крок 1: Оновлено CORS в Program.cs

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.WithOrigins(
                "http://localhost:63342",   // WebStorm
                "http://127.0.0.1:63342",   // Alternative
                "http://localhost:5500",    // Live Server
                "http://127.0.0.1:5500"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()  // КРИТИЧНО для cookies!
            .SetIsOriginAllowedToAllowWildcardSubdomains();
    });
});
```

**Ключові моменти:**
- ✅ `.AllowCredentials()` - дозволяє передавати cookies
- ✅ Додано всі можливі localhost варіанти
- ✅ Підтримка wildcard subdomains

### Крок 2: Покращено Cookie Options в UserModule.cs

```csharp
#if DEBUG
var cookieOptions = new CookieOptions
{
    HttpOnly = true,          // Захист від XSS
    Secure = false,           // false для HTTP (localhost)
    SameSite = SameSiteMode.Lax,  // Lax дозволяє cross-site GET
    Expires = DateTime.Now.AddMinutes(15),
    Path = "/",
    Domain = null             // ВАЖЛИВО для localhost!
};

res.Cookies.Append("_t", tokenstr, cookieOptions);
#endif
```

**Чому `Domain = null`?**
- При localhost не треба вказувати domain
- Браузер автоматично використає поточний host

### Крок 3: Додано Логування

```csharp
Console.WriteLine("Generated Token: " + tokenstr);
Console.WriteLine("Request Origin: " + context.Request.Headers["Origin"]);
Console.WriteLine("Cookie set with options: HttpOnly=true, Secure=false, SameSite=Lax");
```

## 🧪 Як Перевірити

### 1. Запустіть Backend
```bash
dotnet run
```

### 2. Відкрийте Frontend в WebStorm
- URL: `http://localhost:63342/...`

### 3. Виконайте Login
- Відкрийте Developer Tools (F12)
- Перейдіть на вкладку **Network**
- Виконайте логін
- Знайдіть запит `/CommitLogin`

### 4. Перевірте Response Headers
Повинні бути:
```
Set-Cookie: _t=<token>; path=/; expires=...; httponly; samesite=lax
Access-Control-Allow-Credentials: true
Access-Control-Allow-Origin: http://localhost:63342
```

### 5. Перевірте Cookies в Application Tab
- Developer Tools → **Application** → **Cookies**
- Виберіть `https://localhost:7293`
- Має бути cookie `_t` зі значенням JWT токена

### 6. Перевірте Console Output (Backend)
```
Generated Token: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
Request Origin: http://localhost:63342
Cookie set with options: HttpOnly=true, Secure=false, SameSite=Lax, Path=/, Domain=null
```

## 🔧 Додаткові Налаштування

### Якщо Cookies Все Одно Не Працюють

#### Варіант 1: Вимкніть HTTPS Redirect (тимчасово)
В `Program.cs` закоментуйте:
```csharp
// app.UseHttpsRedirection();
```

#### Варіант 2: Запустіть Backend на HTTP
В `launchSettings.json`:
```json
"applicationUrl": "http://localhost:7293"
```

#### Варіант 3: Перевірте Browser Settings
Деякі браузери блокують third-party cookies:
- Chrome: Settings → Privacy → Allow all cookies (тимчасово для розробки)
- Firefox: Settings → Privacy → Standard

## 📋 Checklist для Debug

- [ ] CORS policy включає `.AllowCredentials()`
- [ ] Frontend використовує `credentials: 'include'` в fetch
- [ ] Cookie `Secure` = false для HTTP localhost
- [ ] Cookie `SameSite` = Lax (не Strict!)
- [ ] Cookie `Domain` = null для localhost
- [ ] Backend логує "Cookie set with options"
- [ ] Network tab показує Set-Cookie header
- [ ] Application tab показує cookie `_t`

## 🎯 Типові Помилки

### ❌ Помилка 1: Secure = true на HTTP
```csharp
Secure = true  // НЕ працює на http://localhost!
```
**Рішення:** `Secure = false` для DEBUG

### ❌ Помилка 2: SameSite = Strict
```csharp
SameSite = SameSiteMode.Strict  // Блокує cross-origin
```
**Рішення:** `SameSite = SameSiteMode.Lax` для localhost

### ❌ Помилка 3: Відсутній AllowCredentials
```csharp
.AllowAnyMethod()  // Забули AllowCredentials!
```
**Рішення:** Додати `.AllowCredentials()`

### ❌ Помилка 4: credentials: 'omit'
```javascript
credentials: 'omit'  // Блокує cookies
```
**Рішення:** `credentials: 'include'`

## 🚀 Для Production

В production використовуйте:
```csharp
#else
res.Cookies.Append("_t", tokenstr, new CookieOptions
{
    HttpOnly = true,
    Secure = true,              // true для HTTPS
    SameSite = SameSiteMode.Strict,  // Strict для production
    Expires = DateTime.Now.AddMinutes(15),
    Path = "/"
});
#endif
```

## 📚 Додаткова Інформація

### SameSite Режими:
- **None**: Cookies передаються завжди (потрібен Secure=true)
- **Lax**: Cookies передаються на GET requests з інших сайтів
- **Strict**: Cookies НЕ передаються на cross-site requests

### HttpOnly:
- `true` - JavaScript не може читати cookie (захист від XSS)
- Cookie автоматично додається до всіх HTTP requests

### Secure:
- `true` - Cookie передається лише через HTTPS
- `false` - Cookie передається через HTTP (тільки для розробки!)

---

**Після цих змін cookies повинні працювати правильно!** 🎉
