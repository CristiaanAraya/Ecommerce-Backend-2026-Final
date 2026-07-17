# TUDS-Backend-Ecommerce

Sistema de e-commerce con **Clean Architecture**, **CQRS** y **microservicio de Stock**. Consta de dos servicios independientes que se comunican vía HTTP.

---

## Tecnologías

| Tecnología            | Uso |
| .NET 10.0 SDK         | Runtime y compilación |
| ASP.NET Core          | Web API |
| Entity Framework Core | ORM y persistencia |
| SQLite                | Base de datos embebida (una por servicio) |
| MediatR               | Implementación de CQRS (Commands, Queries, Pipeline Behaviors) |
| FluentValidation      | Validación de entrada en la capa Application |
| JWT Bearer            | Autenticación en el E-commerce API |
| Swagger / OpenAPI     | Documentación interactiva |

---

## Arquitectura

### Clean Architecture (4 capas)

```
┌─────────────────────────────────────────────────────────────┐
│                     API (Presentación)                       │
│  Controllers, Middleware, Program.cs, DTOs, Swagger          │
│  Depende de: Application e Infrastructure                    │
├─────────────────────────────────────────────────────────────┤
│                  Application (Aplicación)                     │
│  Commands, Queries, Handlers, Validators, DTOs, Behaviors    │
│  Interfaces de servicios externos (IStockClient, IToken)     │
│  Depende de: Domain                                          │
├─────────────────────────────────────────────────────────────┤
│               Infrastructure (Infraestructura)                │
│  Repositorios (EF Core), DbContext, JWT, HTTP Clients        │
│  Depende de: Domain y Application                            │
├─────────────────────────────────────────────────────────────┤
│                   Domain (Dominio)                            │
│  Entidades, Value Objects, Repository Interfaces,            │
│  Excepciones de negocio, Enums                               │
│  Sin dependencias externas                                   │
└─────────────────────────────────────────────────────────────┘
```

### E-commerce API (Backend-Principal)

```
ECommerce.Api/               ← Capa de Presentación
├── Controllers/             ← Auth, Categories, Orders, Products, Stock
├── DTOs/                    ← Request/Response objects
├── Mappers/                 ← Entity → DTO (manual, sin AutoMapper)
├── Middleware/               ← GlobalExceptionHandler
└── Program.cs               ← DI, JWT, Swagger

ECommerce.Application/       ← Capa de Aplicación
├── Contracts/Infrastructure/ ← Interfaces de servicios (ITokenService, IHashService, IStockServiceClient)
├── Behaviors/                ← ValidationBehavior<T>
├── Features/                 ← CQRS agrupado por feature
│   ├── Auth/Commands/        ← SignUp, SignIn
│   ├── Categories/Queries/   ← GetAll, GetById
│   ├── Orders/Commands/      ← PlaceOrder
│   ├── Orders/Queries/       ← GetById, GetByUser
│   ├── Products/Commands/    ← Create, Update, Delete
│   └── Products/Queries/     ← GetAll, GetById, Search, GetPaged
└── ApplicationServiceExtensions.cs  ← DI de MediatR + Validators

ECommerce.Infrastructure/    ← Capa de Infraestructura
├── Persistence/
│   ├── ApplicationDbContext.cs
│   ├── Configurations/       ← EF EntityTypeConfiguration + ValueConverters
│   ├── Migrations/
│   ├── DbInitializer.cs     ← Seed data
│   └── UnitOfWork.cs
├── Repositories/             ← ProductRepository, OrderRepository, etc.
├── Services/                 ← TokenGenerator, PasswordHasher, StockServiceClient
└── InfrastructureServiceExtensions.cs  ← DI de repos, db, http clients

ECommerce.Domain/             ← Capa de Dominio
├── Contracts/Persistence/    ← Interfaces de repositorios (IProductRepository, etc.)
├── Entities/                 ← Product, Category, Order, OrderItem, User, ProductReview
├── ValueObjects/             ← Email, Money, Address
├── Common/                   ← PagedData<T>
├── Exceptions/               ← BusinessException, DomainException, etc.
└── Enums                     ← OrderStatus (dentro de Order.cs)
```

### Stock Service (microservicio)

```
Stock.Api/
├── Controllers/              ← StockController
├── DTOs/                     ← ReserveStockRequest, ReleaseStockRequest
├── Middleware/                ← GlobalExceptionHandler
└── Program.cs                ← DI, Swagger

Stock.Application/
├── Contracts/Infrastructure/  ← IProductoServiceClient
├── Behaviors/                 ← ValidationBehavior
├── Features/Stock/
│   ├── Commands/              ← AddStock, ReserveStock, ReleaseStock
│   └── Queries/               ← GetAllStock, GetStockByProductId
└── ApplicationServiceExtensions.cs

Stock.Infrastructure/
├── Persistence/
│   ├── StockDbContext.cs
│   ├── Configurations/
│   ├── Migrations/
│   ├── DbInitializer.cs
│   └── UnitOfWork.cs
├── Repositories/              ← ProductStockRepository
├── Services/                  ← ProductoServiceClient (consulta productos al Ecommerce API)
└── InfrastructureServiceExtensions.cs

Stock.Domain/
├── Contracts/Persistence/     ← IProductStockRepository, IUnitOfWork
├── Entities/                  ← ProductStock, StockMovement
├── ValueObjects/              ← Quantity, ProductName
└── Exceptions/                ← BusinessException, NotFoundException
```

---

## Decisiones de diseño

### ¿Por qué dos bases de datos?
Patrón **Database per Service**. Cada microservicio tiene su propio almacenamiento. El E-commerce API no puede modificar stock directamente — tiene que pedírselo al Stock Service vía HTTP. Esto fuerza el desacople real: un bug en órdenes no corrompe el inventario.

### ¿Por qué CQRS con MediatR?
- **Controllers ultralivianos**: solo reciben el request, llaman a `mediator.Send()` y devuelven la respuesta. No conocen repositorios ni lógica de negocio.
- **Handlers autocontenidos**: cada comando/query es una clase con sus propias dependencias. No hay `*Service` gigantes con 15 métodos.
- **Pipeline Behaviors**: la validación (`ValidationBehavior`) se ejecuta automáticamente antes de cada handler sin tocar el controller.
- **SRP puro**: cada handler hace una sola cosa.

### ¿Por qué Value Objects en lugar de primitivos?
`Email`, `Money`, `Address`, `Quantity` encapsulan validación y comportamiento. En lugar de tener `if (!email.Contains('@'))` esparcido por 10 lugares, la validación vive en una sola clase. Además, `Money` sabe sumarse, `Quantity` sabe compararse. Esto hace que el dominio sea expresivo y no permita estados inválidos.

### ¿Por qué mapping manual sin AutoMapper?
- **Compile-time safety**: si una propiedad cambia, el compilador falla. AutoMapper falla en runtime.
- **Sin magia**: se ve exactamente qué se mapea a qué.
- **Dependencia cero**: una librería menos que actualizar.
- **Proyecciones no triviales**: `ProductDto.CategoryName` viene de `Category.Name`, no de `Product`. Con AutoMapper necesitarías `.ForMember()`.

### ¿Por qué un microservicio separado para Stock?
El stock es un subdominio con alta volatilidad de cambios (reglas de reserva, liberación, auditoría). Separarlo permite:
- Escalarlo independientemente si la carga de consultas de stock crece.
- Cambiar la lógica de inventario sin tocar el código de órdenes/usuarios.
- Usar una tecnología de persistencia diferente si hiciera falta (Redis para stock en memoria, por ejemplo).

---

## Opción elegida: Stock Service

Regla de negocio principal: **ninguna orden puede crearse si no hay stock suficiente** en el Stock Service.

Flujo de creación de orden:
1. El handler `PlaceOrderCommandHandler` recibe el comando
2. Valida que el usuario existe
3. Llama al Stock Service → `POST /api/stock/reserve`
4. Si el Stock Service responde `{ reserved: true }` → la orden se crea con estado `Pending`
5. Si responde `{ reserved: false, ... }` → se lanza `BusinessException` y la orden **no se crea**

---

## Dominio

### ECommerce.Domain

| Entidad         | Propósito |
|---|---|
| `Product`       | Producto del catálogo (nombre, descripción, precio, stock, categoría) |
| `Category`      | Categoría para agrupar productos |
| `Order`         | Orden de compra (items, total, estado, dirección de envío) |
| `OrderItem`     | Línea de detalle de una orden (producto, cantidad, precio unitario) |
| `User`          | Usuario del sistema (Admin o Customer) |
| `ProductReview` | Reseña de un producto con calificación (1-5) |

| Value Object  | Propiedades                             | Reglas |
| `Email`       | `string Value`                          | Formato válido, se almacena en minúsculas |
| `Money`       | `decimal Amount`                        | No negativo, operadores `+` y `*` |
| `Address`     | `Street, City, State, ZipCode, Country` | Street y City obligatorios |

| Enum          | Valores |
| `OrderStatus` | `Pending, Confirmed, Shipped, Delivered, Cancelled` |

### Stock.Domain

| Entidad         | Propósito |
| `ProductStock`  | Stock de un producto (total, reservado, disponible) |
| `StockMovement` | Auditoría de cada cambio en el stock |

| Value Object  | Propiedades     | Reglas |
| `Quantity`    | `int Value`     | No negativo, operadores `+` y `-` |
| `ProductName` | `string Value`  | No vacío, max 200 caracteres |

| Enum              | Valores |
| `StockChangeType` | `Addition, Reservation, Release` |

---

## Requisitos

- .NET 10.0 SDK

---

## Puertos y URLs

| Servicio        | Puerto  | URL Base                | Swagger |
|---|---|
| Stock Service   | `5050`  | `http://localhost:5050` | `http://localhost:5050/swagger` |
| E-commerce API  | `5047`  | `http://localhost:5047` | `http://localhost:5047/swagger` |

Configurado en:
- `launchSettings.json` de cada proyecto
- `appsettings.json` → sección `StockService:BaseUrl` / `EcommerceApi:BaseUrl`

---

## Endpoints de la API

### E-commerce API (`http://localhost:5047`)

| Método    | Ruta                                   | Auth | Descripción |
|---|---|
| `POST`    | `/api/Auth/register`                   | No   | Registro de usuario |
| `POST`    | `/api/Auth/login`                      | No   | Login, devuelve JWT |
| `GET`     | `/api/Categories`                      | No   | Lista de categorías |
| `GET`     | `/api/Categories/{id}`                 | No   | Categoría por ID |
| `GET`     | `/api/Products`                        | No   | Lista de productos activos |
| `GET`     | `/api/Products/{id}`                   | No   | Producto por ID |
| `GET`     | `/api/Products/search?term=`           | No   | Búsqueda por nombre |
| `GET`     | `/api/Products/paged?page=&pageSize=` | No    | Productos paginados |
| `POST`    | `/api/Products`                       | Admin | Crear producto |
| `PUT`     | `/api/Products/{id}`                  | Admin | Actualizar producto |
| `DELETE`  | `/api/Products/{id}`                  | Admin | Desactivar producto |
| `POST`    | `/api/Orders`                         | Sí    | Crear orden (reserva stock) |
| `GET`     | `/api/Orders/mine`                    | Sí    | Órdenes del usuario autenticado |
| `GET`     | `/api/Orders/{id}`                    | Sí    | Detalle de orden |
| `GET`     | `/api/Stock`                          | No    | Stock de todos los productos (proxy al Stock Service) |
| `GET`     | `/api/Stock/{productId}`              | No    | Stock de un producto |

### Stock Service (`http://localhost:5050`)

| Método | Ruta                     | Descripción |
|---|---|
| `GET`  | `/api/Stock`             | Lista de todo el stock |
| `GET`  | `/api/Stock/{productId}` | Stock de un producto |
| `POST` | `/api/Stock`             | Agregar stock (crea si no existe) |
| `POST` | `/api/Stock/reserve`     | Reservar stock (atómico) |
| `POST` | `/api/Stock/release`     | Liberar reservas |

---

## Instrucciones de ejecución

### Paso 1: Iniciar el Stock Service (primero)

```powershell
cd D:\FACULTAD\TERCERO\Backend\Final\Ecommerce
dotnet run --project "StockService\src\Stock.Api\Stock.Api.csproj"
```

Esperar a que aparezca `Now listening on: http://localhost:5050`.

### Paso 2: Iniciar el E-commerce API

En una **nueva terminal**:

```powershell
cd D:\FACULTAD\TERCERO\Backend\Final\Ecommerce
dotnet run --project "Backend-Principal\src\ECommerce.Api\ECommerce.Api.csproj"
```

Esperar a que aparezca `Now listening on: http://localhost:5047`.

### Paso 3: Resetear bases de datos (opcional)

Para borrar datos y forzar un reseed desde cero:

```powershell
Remove-Item "StockService\src\Stock.Api\stock.db"
Remove-Item "Backend-Principal\src\ECommerce.Api\ecommerce.db"
# Luego reiniciar ambos servicios
```

---

## Usuarios precargados (seed data)

| Rol          | Email                    | Contraseña |
|---|---|
| **Admin**    | `admin@ecommerce.com`    | `Admin@2024` |
| **Customer** | `cliente@ecommerce.com`  | `Cliente@2024` |

El `DbInitializer` se ejecuta automáticamente al iniciar cada servicio. Si los usuarios ya existen, los saltea.

---

## Productos precargados

| # | Producto              | GUID                                   | Stock |
|---|---|
| 1 | Laptop Gamer          | `00000000-0000-0000-0000-000000000001` | 10 |
| 2 | Teclado Mecánico      | `00000000-0000-0000-0000-000000000002` | 50 |
| 3 | Mouse Inalámbrico     | `00000000-0000-0000-0000-000000000003` | 30 |
| 4 | Monitor 27" 4K        | `00000000-0000-0000-0000-000000000004` | 15 |
| 5 | Auriculares Bluetooth | `00000000-0000-0000-0000-000000000005` | 25 |
| 6 | Smartwatch Deportivo  | `00000000-0000-0000-0000-000000000006` | 20 |
| 7 | Parlante Portátil     | `00000000-0000-0000-0000-000000000007` | 40 |
| 8 | Disco SSD 1TB         | `00000000-0000-0000-0000-000000000008` | 35 |
| 9 | Tablet 10"            | `00000000-0000-0000-0000-000000000009` | 12 |
| 10 | Cargador Inalámbrico | `00000000-0000-0000-0000-00000000000a` | 60 |

Los GUIDs son idénticos en ambos servicios para que el E-commerce API y el Stock Service refieran al mismo producto.

---

## Flujo End-to-End

### 1. Verificar que los servicios responden

```powershell
curl http://localhost:5050/api/stock
curl http://localhost:5047/api/products
```

### 2. Autenticarse

```powershell
$token = curl -s -X POST http://localhost:5047/api/Auth/login `
  -H "Content-Type: application/json" `
  -d '{"email":"admin@ecommerce.com","password":"Admin@2024"}' | `
  ConvertFrom-Json | Select-Object -ExpandProperty accessToken
```

### 3. Consultar stock disponible

```powershell
curl -X GET http://localhost:5047/api/Stock -H "accept: text/plain"
```

### 4. Crear una orden (reserva stock automática)

```powershell
curl -s -X POST http://localhost:5047/api/Orders `
  -H "Content-Type: application/json" `
  -H "Authorization: Bearer $token" `
  -d '{"items":[{"productId":"00000000-0000-0000-0000-000000000005","quantity":3}]}'
```

### 5. Verificar que el stock se reservó

```powershell
curl -X GET http://localhost:5047/api/Stock/00000000-0000-0000-0000-000000000005
```

El campo `availableQuantity` debe haber disminuido.

### 6. Probar stock insuficiente

```powershell
curl -s -X POST http://localhost:5047/api/Orders `
  -H "Content-Type: application/json" `
  -H "Authorization: Bearer $token" `
  -d '{"items":[{"productId":"00000000-0000-0000-0000-000000000001","quantity":100}]}'
```

Respuesta esperada: **422 Unprocessable Entity** con mensaje `"No se pudo reservar stock para todos los productos"`.

---

## Tests

```powershell
dotnet test "Backend-Principal\tests\ECommerce.Tests\ECommerce.Tests.csproj"
```

Actualmente hay **9 tests** que cubren:
- `PlaceOrderCommandHandler` (4 tests): creación exitosa, sin líneas, usuario no encontrado, stock insuficiente
- `CreateProductCommandHandler` (2 tests): creación exitosa, categoría no encontrada
- `GetAllProductsQueryHandler` (1 test): listado
- `GetProductByIdQueryHandler` (2 tests): búsqueda exitosa, producto no encontrado

---

## Arquitectura de comunicación entre servicios

```
┌─────────────────────────────────────────────┐      HTTP      ┌──────────────────────────────────────┐
│                                             │ ──────────────> │                                      │
│           E-commerce API                    │   GET /stock    │          Stock Service                │
│         (localhost:5047)                    │   POST /reserve │        (localhost:5050)               │
│                                             │   POST /release │                                      │
│                                             │ <────────────── │                                      │
└─────────────────────────────────────────────┘     JSON        └──────────────────────────────────────┘
         │                                                               │
         │ SQLite                                                         │ SQLite
         ▼                                                               ▼
  ┌─────────────────────┐                                       ┌─────────────────────┐
  │    ecommerce.db     │                                       │     stock.db        │
  │                     │                                       │                     │
  │ Products            │                                       │ ProductStocks       │
  │ Categories          │                                       │ StockMovements      │
  │ Orders              │                                       └─────────────────────┘
  │ OrderItems          │
  │ Users               │
  │ ProductReviews      │
  └─────────────────────┘
```

**Comunicación:**

- `IStockServiceClient` (en ECommerce.Infrastructure) → llama al Stock Service vía `HttpClient`
- `IProductoServiceClient` (en Stock.Infrastructure) → consulta productos al E-commerce API vía `HttpClient`

Ambos usan `HttpClientFactory` registrado en DI con timeout de 5 segundos.

---

## DI Registration (Program.cs)

### E-commerce API
```csharp
builder.Services.AddApplication();      // MediatR + Validators
builder.Services.AddInfrastructure(config); // DbContext, Repos, HTTP Clients, JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => { ... });
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddSwaggerGen(options => { ... });
```

### Stock Service
```csharp
builder.Services.AddApplication();      // MediatR + Validators
builder.Services.AddInfrastructure(config); // DbContext, Repos, HTTP Client
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddSwaggerGen(options => { ... });
```
