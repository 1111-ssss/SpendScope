# 🏗 SpendScope API Documentation

Бэкенд-часть системы SpendScope построена на базе ASP.NET Core с использованием современных паттернов проектирования и фокусом на безопасность и производительность.

## 🛠 Технологический стек и паттерны

- **Архитектура**: Гибрид **Clean Architecture** *(для четкого разделения слоев)* и **Vertical Slice Architecture** *(для инкапсуляции бизнес-логики по фичам)*.
- **База данных**: **PostgreSQL** с использованием **Ardalis.Specification** для построения гибких запросов.
- **Логика и Медиатор**: **MediatR** для реализации паттерна CQRS.
- **Валидация**: **FluentValidation** для строгой проверки входящих данных.
- **Логирование**: **Serilog** с записью в таблицу базы данных и поддержкой **Correlation ID** для сквозного отслеживания запросов.

---

## 🔐 Безопасность и Аутентификация

Проект реализует многоуровневую систему защиты:

1. **Hashing**: Пароли хешируются с помощью алгоритма **Argon2** *(устойчив к перебору и GPU-атакам)*.
2. **JWT Auth**: Доступ по токенам с разделением эндпоинтов по ролям.
3. **IP Validation**: Дополнительная проверка безопасности — сверка текущего IP-адреса пользователя с IP, «вшитым» в JWT токен при авторизации.
4. **Rate Limiting**: Защита от brute-force и спама запросами на уровне API.

---

## ⚙️ Технические особенности

**Обработка ответов и ошибок**

- **Custom Result**: Все эндпоинты возвращают унифицированный объект результата, что упрощает обработку ответов на стороне Flutter и WPF приложений.
- **Health Checks**: Реализован кастомный контроллер состояния системы, предоставляющий детальную информацию о нагрузке и доступности ресурсов сервера.

**Работа с медиа**

- **Image Optimization**: Для обработки аватаров и изображений используется **ImageSharp**. Реализован Image Formatter, который на лету оптимизирует и подгоняет изображения под нужные форматы и размеры.

---

## 📂 Структура проекта (Clean Architecture + Vertical Slices)

Каждая функциональная возможность *(например, "Создание транзакции" или "Регистрация")* содержит в себе всю основную логику работы: от валидирования данных до сохранения и удаления данных в базе данных.

```
SpendScope/
└── api/
    ├── Application/
    │   ├── Abstractions/    # Интерфейсы сервисов
    │   └── Features/        # Вертикальные срезы (Logic, Commands, Queries)
    ├── Infrastructure/
    │   ├── DataBase/        # База данных, Entity Framework, репозиторий
    │   └── Services/        # Image Formatter, Storage, JWT, Hashing и тп.
    ├── Domain/
    │   ├── Abstractions/    # Базовые интерфейсы, result
    │   ├── Specifications/  # Спецификации для Ardalis.Specification
    │   └── Entities/        # Сущности базы данных
    └── Web/                 # API и контроллеры
        ├── Controllers/     # Контроллеры для обработки запросов
        └── Program.cs       # Запуск API, DI контейнер
```

---

## 🚀 Развертывание и мониторинг

Для мониторинга работоспособности используется эндпоинт:
`GET /api/health/detailed` — возвращает статус БД, дискового пространства и памяти.

Данные БД хранятся в PostgreSQL. Для миграций используется EF Core.




# Архитектура Clean + Vertical Slice (Feature-first)

## 📋 Обзор

Проект **SpendScope** использует гибридный подход, сочетающий **Clean Architecture** с **Vertical Slice Architecture** (предметно-ориентированная архитектура). Этот подход обеспечивает высокую степень модульности, тестируемости и поддерживаемости кода.

---

## 🏗 Уровни архитектуры

```
┌─────────────────────────────────────────────────────────────┐
│                     Presentation Layer                      │
│  (Web/Controllers, Admin/Features, Mobile)                  │
├─────────────────────────────────────────────────────────────┤
│                     Application Layer                       │
│  (Features/, Abstractions/, Common/)                        │
├─────────────────────────────────────────────────────────────┤
│                       Domain Layer                          │
│  (Entities/, ValueObjects/, Specifications/)                │
├─────────────────────────────────────────────────────────────┤
│                    Infrastructure Layer                     │
│  (DataBase/, Services/, UnitOfWork/, Migrations/)           │
└─────────────────────────────────────────────────────────────┘
```

---

## 📁 Структура проекта

### API (Backend)

```
api/
├── Web/                          # Presentation Layer
│   ├── Controllers/              # HTTP endpoints (тонкий слой)
│   ├── Middleware/               # Обработчики ошибок, логирование
│   ├── Extensions/               # Расширения для DI, конфигурации
│   └── Program.cs                # Точка входа, настройка DI
│
├── Application/                  # Application Layer
│   ├── Features/                 # Вертикальные срезы (фичи)
│   │   ├── Auth/                 # Фича "Авторизация"
│   │   │   ├── Commands/         # Команды (CQRS)
│   │   │   │   ├── Login/
│   │   │   │   │   ├── LoginCommand.cs
│   │   │   │   │   ├── LoginCommandHandler.cs
│   │   │   │   │   └── LoginCommandValidator.cs
│   │   │   │   ├── Register/
│   │   │   │   │   ├── RegisterUserCommand.cs
│   │   │   │   │   ├── RegisterUserCommandHandler.cs
│   │   │   │   │   └── RegisterUserCommandValidator.cs
│   │   │   │   └── Refresh/
│   │   │   └── Common/           # Общие DTO для фичи
│   │   ├── Categories/
│   │   │   ├── Commands/
│   │   │   │   ├── AddCategory/
│   │   │   │   ├── DeleteCategory/
│   │   │   │   └── UpdateCategory/
│   │   │   ├── Queries/
│   │   │   │   └── GetCategories/
│   │   │   └── Common/
│   │   ├── Profiles/
│   │   ├── Achievements/
│   │   └── ...
│   ├── Abstractions/             # Интерфейсы репозиториев, сервисов
│   ├── Common/                   # Общие классы (behaviors, filters)
│   └── DI/                       # Регистрация зависимостей
│
├── Domain/                       # Domain Layer (ядро)
│   ├── Entities/                 # Агрегаты, сущности
│   │   ├── User.cs
│   │   ├── Category.cs
│   │   ├── Transaction.cs
│   │   └── RefreshToken.cs
│   ├── ValueObjects/             # Объекты-значения
│   ├── Specifications/           # Бизнес-спецификации
│   │   └── Auth/
│   │       └── UserByUsernameOrEmailSpec.cs
│   └── Abstractions/             # Базовые классы домена
│
└── Infrastructure/               # Infrastructure Layer
    ├── DataBase/                 # EF Core контексты
    ├── UnitOfWork/               # Паттерн Unit of Work
    ├── Services/                 # Внешние сервисы
    │   ├── JwtGenerator.cs
    │   ├── PasswordHasher.cs
    │   └── CurrentUserService.cs
    ├── Migrations/               # Миграции БД
    └── DI/                       # Регистрация инфраструктуры
```

---

## 🔑 Ключевые принципы

### 1. Vertical Slice Architecture (Feature-first)

Каждая **фича** (feature) инкапсулирует всю необходимую логику:

```
Фича "Регистрация пользователя":
├── Команда (Request)
├── Валидатор (Validation)
│   └── Валидация данных
├── Обработчик (Handler)
│   ├── Проверка бизнес-правил
│   ├── Сохранение в БД
│   └── Генерация токена
└── Response (DTO)
```

**Преимущества:**
- ✅ Вся логика фичи находится в одном месте
- ✅ Минимальные зависимости между фичами
- ✅ Легко добавлять новые функции
- ✅ Простое тестирование изолированных срезов

### 2. CQRS (Command Query Responsibility Segregation)

Разделение операций записи и чтения:

```
Commands/          # Изменение состояния (Write)
├── AddCategory/   # Создать категорию
├── UpdateCategory/# Обновить категорию
└── DeleteCategory/# Удалить категорию

Queries/           # Получение данных (Read)
└── GetCategories/ # Получить список категорий
```

### 3. Clean Architecture Dependencies

```
Presentation → Application → Domain ← Infrastructure
                                    ↑
                                    │
                              (реализует интерфейсы)
```

**Правило зависимостей:**
- Зависимости направлены **внутрь** (к Domain)
- Domain не зависит ни от чего
- Application зависит только от Domain
- Infrastructure реализует интерфейсы Application

---

## 📦 Поток данных (на примере регистрации)

### 1. Запрос от клиента
```
POST /api/auth/register
{
  "username": "john_doe",
  "email": "john@example.com",
  "password": "SecurePass123"
}
```

### 2. Controller (Web Layer)
```csharp
// Web/Controllers/AuthController.cs
[HttpPost("register")]
public async Task<IActionResult> Register(RegisterUserCommand command)
{
    var result = await _mediator.Send(command);

    return result.ToActionResult();
}
```

### 3. Application Layer
```
RegisterUserCommand
    ↓
RegisterUserCommandValidator (FluentValidation)
    ↓
RegisterUserCommandHandler
    ├── Проверка существования пользователя
    ├── Хеширование пароля
    ├── Создание сущности User
    ├── Сохранение через UnitOfWork
    └── Генерация JWT токена
```

### 4. Domain Layer
```csharp
// Domain/Entities/User.cs
public class User : BaseEntity
{
    public string Username { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    
    public static User Create(string username, string email, string passwordHash)
    {
        // Бизнес-логика создания
        return new User { ... };
    }
}
```

### 5. Infrastructure Layer
```csharp
// Infrastructure/DataBase/AppDbContext.cs
public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    // Конфигурация маппинга к БД
}

// Infrastructure/Services/JwtGenerator.cs
public class JwtGenerator : IJwtGenerator
{
    public Result<AuthResponse> GenerateToken(User user)
    {
        // Генерация JWT токена
    }
}
```

---

## 🛠 Используемые паттерны и библиотеки

| Паттерн/Библиотека | Назначение |
|:-|:-|
| **MediatR** | Реализация CQRS, посредник для команд/запросов |
| **FluentValidation** | Валидация команд перед обработкой |
| **Refit** | Типобезопасный HTTP клиент (Admin → API) |
| **CommunityToolkit.MVVM** | MVVM для WPF (ObservableObject, RelayCommand) |
| **Entity Framework Core** | ORM для работы с PostgreSQL |
| **Unit of Work** | Управление транзакциями БД |
| **Repository** | Абстракция доступа к данным |
| **Specification** | Инкапсуляция бизнес-правил фильтрации |

---

## 📊 Сравнение подходов

### ❌ Традиционная Layered Architecture

```
Controllers/
Services/
Repositories/
Entities/

// Проблема: 
// - Логика фичи размазана по слоям
// - Сложно отследить зависимости
// - Трудно удалять/добавлять функции
```

### ✅ Vertical Slice Architecture

```
Features/
├── Auth/
│   ├── Register/      # Всё для регистрации
│   ├── Login/         # Всё для входа
│   └── Refresh/       # Всё для обновления токена
├── Categories/
│   ├── Add/           # Всё для добавления
│   ├── Delete/        # Всё для удаления
│   └── Get/           # Всё для получения
```

---

## 🎯 Жизненный цикл фичи

### Добавление новой фичи "Создание транзакции"

1. **Domain Layer**: Создать сущность `Transaction`
2. **Application/Features/Transactions/Commands/CreateTransaction/**:
   - `CreateTransactionCommand.cs` — запрос с данными
   - `CreateTransactionCommandValidator.cs` — правила валидации
   - `CreateTransactionCommandHandler.cs` — бизнес-логика
3. **Infrastructure**: Добавить маппинг в DbContext
4. **Web/Controllers**: Добавить endpoint в `TransactionsController`
5. **Admin/Features** (опционально): Добавить UI для управления

---

## 🔒 Безопасность и валидация

### Многоуровневая валидация

```
┌──────────────────────────────────────┐
│ 1. Controller                        │
│    - Model State Validation          │
├──────────────────────────────────────┤
│ 2. FluentValidation                  │
│    - Business rules validation       │
├──────────────────────────────────────┤
│ 3. Domain Entities                   │
│    - Invariant validation            │
├──────────────────────────────────────┤
│ 4. Database Constraints              │
│    - FK, UK, Check constraints       │
└──────────────────────────────────────┘
```

### Пример валидатора

```csharp
public class RegisterUserCommandValidator 
    : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Логин обязателен")
            .MinimumLength(3).WithMessage("Минимум 3 символа")
            .MaximumLength(50).WithMessage("Максимум 50 символов")
            .Matches(@"^[a-zA-Z0-9]+$").WithMessage("Только буквы и цифры");
        
        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Некорректный email");
        
        RuleFor(x => x.Password)
            .MinimumLength(8).WithMessage("Минимум 8 символов")
            .Matches(@"[A-Z]").WithMessage("Заглавная буква")
            .Matches(@"[0-9]").WithMessage("Цифра");
    }
}
```

---

## 📚 Дополнительные ресурсы

- [Clean Architecture — Robert Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [Vertical Slice Architecture — Jimmy Bogard](https://jimmybogard.com/vertical-slice-architecture/)
- [MediatR Documentation](https://github.com/jbogard/MediatR)
- [FluentValidation Documentation](https://docs.fluentvalidation.net/)

---

**With ❤️ by [1111-ssss](https://github.com/1111-ssss)**