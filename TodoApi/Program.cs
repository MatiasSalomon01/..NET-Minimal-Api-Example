using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapGet("/todoitems", async (TodoDb db) => await db.Todos.ToListAsync());

app.MapGet("/todoitems/{id}", async (int id, TodoDb db) => await db.Todos.FindAsync(id));

app.MapPost("/todoitems", async (TodoItem item, TodoDb db) =>
{
    db.Todos.Add(item);
    await db.SaveChangesAsync();
    return Results.Created($"/todoitems/{item.Id}", item);
});

app.MapPut("/todoitems/{id}", async (int id, TodoItem item, TodoDb db) =>
{
    var todo = await db.Todos.FindAsync(id);

    if (todo is null) return Results.NotFound();

    todo.Name = item.Name;
    todo.IsComplete = item.IsComplete;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/todoitems/{id}", async (int id, TodoDb db) =>
{
    if(await db.Todos.FindAsync(id) is TodoItem todo){
        db.Todos.Remove(todo);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});


app.Run();