using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TodoDb>(opt =>
    opt.UseInMemoryDatabase("TodoList"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Get all todo items
app.MapGet("/todoitems", async (TodoDb db) =>
    await db.Todos.ToListAsync());

// Get Completed Todos
app.MapGet("/todoitems/completed", async (TodoDb db) =>
    await db.Todos.Where(t => t.IsComplete).ToListAsync());

//Get a specific todo item by id
app.MapGet("/todoitems/{id:int}", async (int id, TodoDb db) =>
{
    var todo = await db.Todos.FindAsync(id);

    if (todo != null)
    {
        return Results.Ok(todo);
    }

    return Results.NotFound();
});


// Create a new todo item
app.MapPost("/todoitems", async (Todo todo, TodoDb db) =>
{
    db.Todos.Add(todo);
    await db.SaveChangesAsync();

    return Results.Created($"/todoitems/{todo.Id}", todo);
});

// Update an existing todo item
app.MapPut("/todoitems/{id}", async (int id, Todo inputTodo, TodoDb db) =>
{
    var todo = await db.Todos.FindAsync(id);

    if (todo != null)
    {
        todo.Name = inputTodo.Name;
        todo.IsComplete = inputTodo.IsComplete;

        await db.SaveChangesAsync();
        return Results.Ok(todo);
    }

    return Results.NoContent();
});

// Delete a todo item
app.MapDelete("/todoitems/{id}", async (int id, TodoDb db) =>
{
    var todo = await db.Todos.FindAsync(id);

    if (todo != null)
    {
        db.Todos.Remove(todo);
        await db.SaveChangesAsync();
        return Results.Ok(todo);
    }

    return Results.NotFound();
});

app.Run();