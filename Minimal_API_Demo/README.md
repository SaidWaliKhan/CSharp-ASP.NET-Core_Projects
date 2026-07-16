# TodoApi — Minimal API

A beginner-friendly REST API built with **ASP.NET Core 10 Minimal APIs**.  
No Controllers. No bloat. Just clean, readable code.

---

## What this project does

A simple Todo list API with full **CRUD** support — Create, Read, Update, and Delete.  
Built with an in-memory database so there's nothing to install or configure.  
Open Swagger in your browser and start testing in under a minute.

---

## Tech stack

| | |
|---|---|
| Framework | ASP.NET Core 10 — Minimal API |
| Language | C# 13 |
| Database | EF Core InMemory (no SQL setup needed) |
| Docs | Swagger UI (auto-generated) |

---

## Folder structure

```
TodoApi/
├── Models/
│   └── Todo.cs          # The Todo data shape
├── Data/
│   └── TodoDb.cs        # EF Core database context
├── Program.cs           # All routes live here
├── appsettings.json
└── TodoApi.csproj
```

---

## Getting started

**1. Clone and enter the folder**

```bash
git clone https://github.com/YOUR_USERNAME/csharp-aspnetcore-projects.git
cd csharp-aspnetcore-projects/01-TodoApi
```

**2. Run it**

```bash
dotnet watch
```

**3. Open Swagger**

```
https://localhost:5001/swagger
```

That's it. Your API is live.

---

## API endpoints

| Method | Route | What it does |
|--------|-------|--------------|
| `GET` | `/todos` | Get all todos |
| `GET` | `/todos/{id}` | Get one todo by ID |
| `GET` | `/todos/complete` | Get only completed todos |
| `POST` | `/todos` | Create a new todo |
| `PUT` | `/todos/{id}` | Update a todo |
| `DELETE` | `/todos/{id}` | Delete a todo |

**Example request body for POST / PUT:**

```json
{
  "title": "Learn Minimal APIs",
  "isComplete": false
}
```

---

## What you need

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [VS Code](https://code.visualstudio.com/) + [C# Dev Kit extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit)

---

## Key concept

In Minimal APIs, your entire API lives in `Program.cs` — no separate Controller files.  
Each route is one clean line:

```csharp
app.MapGet("/todos", async (TodoDb db) => await db.Todos.ToListAsync());
```

That's a full working endpoint. Read it, understand it, done.

---

*Part of the [C# and ASP.NET Core projects](../README.md) collection.*
