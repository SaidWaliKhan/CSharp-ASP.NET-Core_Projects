<div align="center">

# 🚀 CleanAuth

### Production-Ready JWT Authentication API built with ASP.NET Core 8

<p>
<img src="https://img.shields.io/badge/.NET-8-512BD4?style=for-the-badge&logo=dotnet&logoColor=white"/>
<img src="https://img.shields.io/badge/Clean%20Architecture-5C2D91?style=for-the-badge"/>
<img src="https://img.shields.io/badge/JWT-Authentication-black?style=for-the-badge&logo=jsonwebtokens"/>
<img src="https://img.shields.io/badge/BCrypt-Password%20Hashing-EA4AAA?style=for-the-badge"/>
<img src="https://img.shields.io/badge/License-MIT-success?style=for-the-badge"/>
</p>

*A simple, secure and scalable authentication service following Clean Architecture.*

</div>

---

# ✨ Features

✅ JWT Authentication

✅ User Registration & Login

✅ BCrypt Password Hashing

✅ Clean Architecture

✅ Dependency Injection

✅ Result Pattern

✅ Swagger Documentation

✅ Easily Replace InMemory Repository with SQL Server

---

# 🏛️ Clean Architecture

```text
                 🌐 API
                  │
                  ▼
         📦 Application Layer
                  │
                  ▼
            ❤️ Domain Layer
                  ▲
                  │
      ⚙️ Infrastructure Layer
```

---

# 🛠 Tech Stack

| Technology | Purpose |
|------------|---------|
| ASP.NET Core 8 | Web API |
| JWT | Authentication |
| BCrypt | Password Hashing |
| Swagger | API Documentation |
| Dependency Injection | Loose Coupling |
| Clean Architecture | Project Structure |

---

# 📂 Project Structure

```text
CleanAuth

├── 📁 CleanAuth.Api
│   ├── Controllers
│   ├── Program.cs
│   └── appsettings.json
│
├── 📁 CleanAuth.Application
│   ├── DTOs
│   ├── Interfaces
│   ├── Services
│   └── Common
│
├── 📁 CleanAuth.Domain
│   └── Entities
│
└── 📁 CleanAuth.Infrastructure
    ├── Persistence
    ├── Security
    └── DependencyInjection.cs
```

---

# 🚀 Getting Started

## Clone Repository

```bash
git clone https://github.com/yourusername/CleanAuth.git

cd CleanAuth
```

---

## Restore Packages

```bash
dotnet restore
```

---

## Build

```bash
dotnet build
```

---

## Run

```bash
dotnet run --project CleanAuth.Api
```

---

## Open Swagger

```text
https://localhost:5001/swagger
```

---

# 📡 API

## Register

```http
POST /api/auth/register
```

Request

```json
{
    "email":"john@example.com",
    "password":"Password123!"
}
```

---

## Login

```http
POST /api/auth/login
```

Request

```json
{
    "email":"john@example.com",
    "password":"Password123!"
}
```

---

## Protected Endpoint

```http
GET /api/auth/protected
```

Header

```
Authorization: Bearer YOUR_TOKEN
```

---

# 🔒 Security

- 🔐 BCrypt Password Hashing
- 🔑 JWT Authentication
- 🚫 Plain-text passwords are never stored
- 🧩 Dependency Injection
- 🏛️ Clean Architecture

---

# 📈 Roadmap

- [ ] SQL Server Support
- [ ] PostgreSQL Support
- [ ] Refresh Tokens
- [ ] Email Verification
- [ ] Docker
- [ ] Unit Tests
- [ ] Integration Tests

---

# ⭐ Support

If you like this project,

please consider giving it a ⭐ on GitHub.

---

<div align="center">

### Made with ❤️ using ASP.NET Core

</div>
