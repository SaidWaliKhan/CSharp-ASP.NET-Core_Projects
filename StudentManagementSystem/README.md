# 🎓 Student Management System

> **A production-minded backend learning project built with ASP.NET Core, Clean Architecture, DDD, CQRS, and MediatR.**

[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-10.0-512BD4?logo=dotnet\&logoColor=white)](https://dotnet.microsoft.com/)
[![Entity Framework Core](https://img.shields.io/badge/EF%20Core-10.0-512BD4?logo=dotnet\&logoColor=white)](https://learn.microsoft.com/ef/core/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-Database-CC2927?logo=microsoftsqlserver\&logoColor=white)](https://www.microsoft.com/sql-server)
[![MediatR](https://img.shields.io/badge/MediatR-CQRS-blue)](https://github.com/jbogard/MediatR)
[![FluentValidation](https://img.shields.io/badge/FluentValidation-Validation-green)](https://docs.fluentvalidation.net/)
[![Swagger](https://img.shields.io/badge/Swagger-OpenAPI-85EA2D?logo=swagger\&logoColor=black)](https://swagger.io/)
[![Status](https://img.shields.io/badge/status-work%20in%20progress-orange)]()

---

## 📖 About

**Student Management System (SMS)** is a backend-focused learning project designed to explore how a real-world ASP.NET Core application can be structured for **maintainability, scalability, testability, and clean separation of concerns**.

This isn't intended to be just another CRUD application.

The project is being built deliberately around production-oriented backend concepts such as:

* 🏛️ Clean Architecture
* 🧠 Domain-Driven Design (DDD)
* 🔀 CQRS with MediatR
* 🗄️ Entity Framework Core
* 🔐 JWT Authentication
* ✅ FluentValidation
* 🧩 Repository & Unit of Work
* 🛡️ Global Exception Handling
* 📝 Structured logging with Serilog

> **The goal:** understand not only *how* to build a backend, but *why* each architectural decision exists.

---

## 🚧 Project Status

**Work in Progress**

### ✅ Completed

* Core domain: **Students, Courses & Enrollments**
* CQRS architecture with MediatR
* FluentValidation pipeline behavior
* Global exception handling
* Repository pattern
* Unit of Work
* EF Core persistence
* Business-rule enforcement
* Consistent API error responses
* Swagger / OpenAPI

### ⏳ In Progress

* JWT authentication
* Role-based authorization
* Automated unit & integration tests
* Additional production hardening

### ⚠️ Known Issues

* Deleting a course with active enrollments currently results in a raw `500` instead of a clean business-rule error.

---

# 🏗️ Architecture

The application follows **Clean Architecture**, where dependencies point **inward** toward the business rules.

```text
                    ┌─────────────────────┐
                    │      SMS.WebApi     │
                    │ Controllers / API   │
                    └──────────┬──────────┘
                               │
                               ▼
                    ┌─────────────────────┐
                    │  SMS.Infrastructure │
                    │ EF Core / Repos /   │
                    │ Unit of Work         │
                    └──────────┬──────────┘
                               │
                               ▼
                    ┌─────────────────────┐
                    │   SMS.Application   │
                    │ CQRS / MediatR /    │
                    │ Validation / Use     │
                    │ Cases                │
                    └──────────┬──────────┘
                               │
                               ▼
                    ┌─────────────────────┐
                    │      SMS.Domain     │
                    │ Entities / Rules /  │
                    │ Invariants           │
                    └─────────────────────┘
```

### Dependency Direction

```text
WebApi
  ↓
Infrastructure
  ↓
Application
  ↓
Domain
```

The important rule is:

> **Business logic should not depend on infrastructure or frameworks.**

---

## 🧱 Layer Responsibilities

| Layer                  | Responsibility                                                    | Dependencies                 |
| ---------------------- | ----------------------------------------------------------------- | ---------------------------- |
| **SMS.Domain**         | Entities, business rules, invariants                              | None                         |
| **SMS.Application**    | Use cases, CQRS, MediatR, validation, repository interfaces       | Domain                       |
| **SMS.Infrastructure** | EF Core, DbContext, repositories, Unit of Work, external services | Application + Domain         |
| **SMS.WebApi**         | Controllers, middleware, dependency injection, HTTP concerns      | Application + Infrastructure |

### 🔵 Domain

The innermost layer contains the application's **business rules**.

It has:

* Entities
* Domain behavior
* Invariants
* Aggregate boundaries

The Domain does **not** depend on:

* EF Core
* ASP.NET Core
* SQL Server
* MediatR
* Infrastructure

---

### 🟢 Application

The Application layer represents **what the system can do**.

Examples:

```text
CreateStudentCommand
UpdateStudentCommand
DeleteStudentCommand

GetStudentByIdQuery
GetStudentsQuery

EnrollStudentCommand
AssignGradeCommand
```

MediatR dispatches these requests to their corresponding handlers.

---

### 🟡 Infrastructure

Infrastructure contains implementation details.

Examples:

```text
AppDbContext
StudentRepository
CourseRepository
EnrollmentRepository
UnitOfWork
EF Core configurations
```

The Application layer defines the abstraction:

```csharp
IStudentRepository
```

Infrastructure provides the implementation:

```csharp
StudentRepository
```

This keeps the Application layer independent from EF Core.

---

### 🔴 WebApi

The Web API is the application's **entry point**.

It handles:

* HTTP requests
* Controllers
* Dependency Injection
* Middleware
* Authentication
* Swagger/OpenAPI
* HTTP response formatting

Controllers should remain thin.

```text
HTTP Request
     ↓
Controller
     ↓
MediatR
     ↓
Command / Query Handler
     ↓
Domain + Repository
     ↓
Database
```

---

# 🧠 Key Design Patterns

## 🏛️ Clean Architecture

Keeps business logic independent from infrastructure and frameworks.

**Why?**

Because business rules should survive even if you replace:

```text
SQL Server → PostgreSQL
EF Core    → Dapper
REST API   → gRPC
ASP.NET    → another delivery mechanism
```

---

## 🧩 Domain-Driven Design

The domain contains behavior instead of exposing raw mutable state.

For example:

```csharp
student.EnrollIn(course);
```

instead of:

```csharp
student.Enrollments.Add(enrollment);
```

The first approach allows the domain to protect its own rules.

---

## 🔀 CQRS

The application separates **commands** from **queries**.

### Commands

Change state:

```text
CreateStudent
UpdateStudent
DeleteStudent
EnrollStudent
AssignGrade
```

### Queries

Read state:

```text
GetStudentById
GetStudents
GetCourseById
GetEnrollments
```

This makes each use case smaller and easier to reason about.

---

## 📨 MediatR

Controllers don't need to know which handler performs the business operation.

```text
Controller
    │
    ▼
MediatR
    │
    ├── Command ──→ CommandHandler
    │
    └── Query ────→ QueryHandler
```

This keeps controllers thin and application logic focused.

---

## 🗃️ Repository + Unit of Work

Repositories abstract persistence from the Application layer.

```text
Application
     │
     │ depends on
     ▼
IStudentRepository
     ▲
     │ implemented by
     │
Infrastructure
     │
     ▼
EF Core
```

The Unit of Work coordinates persistence across repositories.

---

## 🛡️ Pipeline Behaviors

Validation doesn't need to be repeated inside every handler.

Instead:

```text
Request
   ↓
Validation Behavior
   ↓
Handler
   ↓
Repository
```

For example:

```text
CreateStudentCommand
        ↓
FluentValidation
        ↓
CreateStudentHandler
```

This provides a centralized validation pipeline.

---

# 📚 Business Rules

The project intentionally demonstrates where business rules should live.

### Student Enrollment

A student cannot enroll in the same course twice.

```text
Student
   │
   ├── Course A
   ├── Course B
   └── Course C
```

Attempting:

```text
Student → Course A
```

again results in a business-rule conflict.

The rule is enforced inside the **Student domain entity**.

---

### Course Capacity

Courses have a maximum capacity.

```text
Course Capacity = 30

Current Enrollments = 30
        ↓
New enrollment ❌
```

This rule requires checking database-wide state, so the capacity check is performed at the **Application layer**.

---

### Grades

Grades are restricted to a valid set of values.

```text
A
B
C
D
F
```

Invalid grade values are rejected.

---

# ✨ Features

### 👨‍🎓 Students

* Create student
* Get student
* Get all students
* Update student
* Delete student
* Unique email enforcement

### 📚 Courses

* Create course
* Get course
* Get all courses
* Update course
* Delete course
* Course capacity management

### 📝 Enrollments

* Enroll student
* Prevent duplicate enrollment
* Enforce course capacity

### 🎓 Grades

* Assign grades
* Validate grade values

### 🛡️ Error Handling

Consistent HTTP responses:

| Status | Meaning                 |
| ------ | ----------------------- |
| `400`  | Validation error        |
| `404`  | Resource not found      |
| `409`  | Business-rule conflict  |
| `500`  | Unexpected server error |

Unexpected exception details are **not exposed to clients in production**.

Instead:

```text
Exception
   ↓
Global Exception Middleware
   ├── Log detailed error
   └── Return safe response
```

---

# 🛠️ Tech Stack

| Technology            | Purpose                    |
| --------------------- | -------------------------- |
| **ASP.NET Core**      | Web API                    |
| **C#**                | Backend language           |
| **EF Core**           | ORM / Data access          |
| **SQL Server**        | Relational database        |
| **MediatR**           | CQRS / mediator pattern    |
| **FluentValidation**  | Request validation         |
| **Serilog**           | Structured logging         |
| **Swagger / OpenAPI** | API documentation          |
| **JWT**               | Authentication *(planned)* |

---

# 📁 Project Structure

```text
StudentManagementSystem/
│
├── src/
│   │
│   ├── SMS.Domain/
│   │   ├── Entities/
│   │   ├── Enums/
│   │   └── ...
│   │
│   ├── SMS.Application/
│   │   ├── Features/
│   │   │   ├── Students/
│   │   │   ├── Courses/
│   │   │   └── Enrollments/
│   │   ├── Common/
│   │   └── Interfaces/
│   │
│   ├── SMS.Infrastructure/
│   │   ├── Persistence/
│   │   ├── Repositories/
│   │   └── ...
│   │
│   └── SMS.WebApi/
│       ├── Controllers/
│       ├── Middleware/
│       ├── Program.cs
│       └── appsettings.json
│
└── tests/
    ├── SMS.Domain.Tests/
    └── SMS.Application.Tests/
```

---

# 🚀 Getting Started

## Prerequisites

Make sure you have:

* [.NET 10 SDK](https://dotnet.microsoft.com/download)
* SQL Server
* Git
* An IDE such as Visual Studio or VS Code

SQL Server can run locally or inside Docker.

---

## 1️⃣ Clone the Repository

```bash
git clone <your-repository-url>

cd StudentManagementSystem
```

---

## 2️⃣ Configure User Secrets

**Never commit real database credentials to `appsettings.json`.**

Navigate to the Web API project:

```bash
cd src/SMS.WebApi
```

Initialize User Secrets:

```bash
dotnet user-secrets init
```

Add your connection string:

```bash
dotnet user-secrets set \
"ConnectionStrings:DefaultConnection" \
"Server=localhost,1433;Database=SmsDb;User Id=sa;Password=<your-password>;TrustServerCertificate=True;"
```

---

## 3️⃣ Apply EF Core Migrations

From the `SMS.WebApi` directory:

```bash
dotnet ef database update \
--project ../SMS.Infrastructure \
--startup-project .
```

This creates/updates the database using the existing EF Core migrations.

---

## 4️⃣ Run the API

```bash
dotnet run
```

The terminal will display the URL where the API is running.

---

## 5️⃣ Explore Swagger

Open:

```text
https://localhost:<port>/swagger
```

Swagger provides an interactive interface for exploring and testing the API.

---

# 🔄 Request Flow Example

Here's what happens when a client enrolls a student:

```text
Client
  │
  │ POST /api/enrollments
  ▼
WebApi Controller
  │
  ▼
EnrollStudentCommand
  │
  ▼
MediatR
  │
  ▼
Validation Behavior
  │
  ├── Invalid → 400
  │
  ▼
EnrollStudentHandler
  │
  ├── Check Student
  ├── Check Course
  ├── Check Capacity
  └── Call Domain
          │
          ▼
    Student.EnrollIn()
          │
          ▼
     Repository
          │
          ▼
       EF Core
          │
          ▼
      SQL Server
```

This is the kind of flow the project is designed to make understandable—not just functional.

---

# 🔮 Roadmap

### 🔐 Security

* [ ] JWT authentication
* [ ] Role-based authorization
* [ ] Admin role
* [ ] Teacher role
* [ ] Student role
* [ ] Refresh tokens

### 🧪 Testing

* [ ] Domain unit tests
* [ ] Application unit tests
* [ ] Integration tests
* [ ] API endpoint tests
* [ ] Testcontainers for database testing

### ⚡ Performance & Scalability

* [ ] Pagination
* [ ] Filtering
* [ ] Sorting
* [ ] Response caching
* [ ] Database indexing
* [ ] Redis caching

### 🌐 API

* [ ] API versioning
* [ ] Improved API documentation
* [ ] Consistent ProblemDetails responses
* [ ] Rate limiting

### 🛠️ Infrastructure

* [ ] Docker
* [ ] Docker Compose
* [ ] CI/CD pipeline
* [ ] Production configuration
* [ ] Health checks

---

# 🎯 Learning Goals

This project is primarily about **learning backend engineering principles**, not simply building features.

The main questions behind the project are:

> **Why should this code live here?**

> **Who should be responsible for this business rule?**

> **How can this system change without breaking everything?**

> **How do we keep infrastructure concerns away from business logic?**

> **How would this design behave as the application grows?**

---

# 📌 Not Yet Implemented

* ❌ JWT authentication
* ❌ Role-based authorization
* ❌ Automated unit tests
* ❌ Integration tests
* ❌ API versioning
* ❌ Pagination
* ❌ Proper handling of deleting courses with active enrollments

---

# 🤝 Contributing

This is currently a personal learning project, but suggestions, improvements, and discussions are welcome.

If you spot an architectural issue or a better approach, feel free to open an issue or submit a pull request.

---

# 📜 License

Personal learning project — **no license applied yet**.

---

<p align="center">
  Built with ❤️ while learning backend engineering with ASP.NET Core.
</p>

<p align="center">
  <strong>Learn → Build → Break → Debug → Refactor → Repeat 🚀</strong>
</p>
