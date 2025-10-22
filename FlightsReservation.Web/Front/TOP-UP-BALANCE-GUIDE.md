# Top Up Balance Feature - Implementation Guide

## ✅ Що Реалізовано

### 1. **Кнопка "Top Up Balance" в Профілі**
- Розміщена поряд з балансом користувача
- Відкриває модальне вікно для поповнення

### 2. **Модальне Вікно Поповнення**
**Поля:**
- **Amount (PLN)** - сума поповнення в польських злотих
  - Тип: число (integer)
  - Мінімум: 1 PLN
  - Обов'язкове поле
  
- **Email** - електронна пошта покупця
  - Автоматично заповнюється з профілю користувача
  - Обов'язкове поле
  - Валідація email формату

**Кнопки:**
- **Commit Payment** - створює платіж та перенаправляє на PayU
- **Cancel** - закриває вікно без дій

### 3. **API Integration**
**Endpoint:** `POST /Payments/CreatePayment`

**Request Body (PayuOrderRequest):**
```json
{
  "TotalAmount": 100,
  "BuyerEmail": "user@example.com"
}
```

**Response (PayuOrderResult):**
```json
{
  "isSuccess": true,
  "value": {
    "status": "SUCCESS",
    "redirectUri": "https://secure.payu.com/...",
    "orderId": "XXXXX"
  },
  "errorMessage": null,
  "code": 200
}
```

### 4. **Автоматичне Перенаправлення**
Після успішного створення платежу:
```javascript
window.location.href = payuResult.redirectUri;
```

## 🔄 Повний Flow

1. **Користувач відкриває профіль** → Бачить свій баланс
2. **Натискає "Top Up Balance"** → Відкривається модальне вікно
3. **Вводить суму та email** → Email автоматично заповнений з профілю
4. **Натискає "Commit Payment"** → Відправляється POST запит на `/Payments/CreatePayment`
5. **Backend створює замовлення в PayU** → Повертає `RedirectUri`
6. **Автоматичний редірект** → Користувач перенаправляється на сторінку оплати PayU
7. **Користувач оплачує** → PayU обробляє платіж
8. **Callback від PayU** → Backend оновлює баланс користувача

## 📋 Структура Коду

### HTML (profile.html)
```html
<!-- Top Up button in profile info -->
<button class="btn btn-primary btn-small" onclick="openTopUpModal()">
    Top Up Balance
</button>

<!-- Modal window -->
<div id="topUpModal" class="modal">
    <div class="modal-content">
        <form id="topUpForm">
            <input type="number" id="topUpAmount" />
            <input type="email" id="topUpEmail" />
            <button type="submit">Commit Payment</button>
        </form>
    </div>
</div>
```

### JavaScript (profile.js)
**Key Functions:**
- `openTopUpModal()` - Відкриває вікно, заповнює email
- `closeTopUpModal()` - Закриває вікно, очищає форму
- `setupTopUpForm()` - Налаштовує обробник submit
- `processPayment()` - Створює платіж та перенаправляє

**Payment Request:**
```javascript
const paymentRequest = {
    TotalAmount: amount,  // Integer (PLN)
    BuyerEmail: email     // String
};
```

### CSS (style.css)
**New Styles:**
- `.modal` - Overlay з затемненням
- `.modal-content` - Контент вікна
- `.modal-close` - Кнопка закриття (×)
- `.modal-actions` - Кнопки в модалці
- `.btn-small` - Малі кнопки

## 🎨 UI/UX Features

### Зручність:
- ✅ Email автоматично заповнюється з профілю
- ✅ Закриття по кліку поза вікном
- ✅ Закриття по кнопці Cancel
- ✅ Закриття по символу ×
- ✅ Валідація полів перед відправкою
- ✅ Показ помилок в модальному вікні

### Валідація:
- Сума повинна бути > 0
- Email повинен бути валідним
- Обов'язкові поля позначені `required`

### Error Handling:
- Показ помилок в червоному блоці
- Консольні логи для debugging
- Відновлення кнопки після помилки

## 🔧 Backend Integration

### PayuOrderRequest Fields:
```csharp
public class PayuOrderRequest
{
    public string Description { get; set; }      // Auto-filled by backend
    public string CurrencyCode { get; set; }     // Auto: "PLN"
    public int TotalAmount { get; set; }         // From user input
    public string CustomerIp { get; set; }       // Auto: "127.0.0.1"
    public string BuyerEmail { get; set; }       // From user input
}
```

### PayuOrderResult Fields:
```csharp
public class PayuOrderResult
{
    public string Status { get; set; }           // e.g., "SUCCESS"
    public string RedirectUri { get; set; }      // PayU payment URL
    public string OrderId { get; set; }          // PayU order ID
}
```

## 🧪 Testing Steps

1. **Відкрийте профіль користувача**
   - Перевірте, чи відображається кнопка "Top Up Balance"

2. **Натисніть кнопку**
   - Модальне вікно має відкритись
   - Email має бути заповнений автоматично

3. **Введіть суму** (наприклад, 100 PLN)
   - Перевірте валідацію мінімального значення

4. **Натисніть "Commit Payment"**
   - Перевірте console.log для request/response
   - Має відбутись redirect на PayU

5. **Перевірте закриття модалки**
   - По кнопці Cancel
   - По символу ×
   - По кліку поза вікном

## 📝 Debug Information

**Console Logs:**
```javascript
'Top up modal opened'
'Payment request:', paymentRequest
'Request URL:', API_URL
'Response status:', status
'Response data:', result
'Redirect URI:', redirectUri
```

**Error Messages:**
- "Amount must be greater than 0"
- "Email is required"
- "HTTP error! status: 400"
- "No redirect URI provided"

## 🚀 Future Enhancements

Можливі покращення:
- [ ] Показ історії платежів
- [ ] Різні валюти (USD, EUR)
- [ ] Збереження обраної суми
- [ ] Прогрес бар під час обробки
- [ ] Успішне повідомлення після оплати
- [ ] Інтеграція з callback endpoint

## 🔗 Related Files

**Modified:**
- `Front/profile.html` - Додано кнопку та модальне вікно
- `Front/js/profile.js` - Додано функції для модалки та платежу
- `Front/js/config.js` - Додано endpoint `createPayment`
- `Front/css/style.css` - Додано стилі для модалки

**Backend Files (No changes needed):**
- `Modules/PaymentModule.cs` - Вже існуючий endpoint
- `Entities/Utilities/Requests/PayuOrderRequest.cs` - DTO для запиту
- `Entities/Utilities/Results/PayuOrderResult.cs` - DTO для відповіді

---

**Ready to use!** 🎉
