using Microsoft.EntityFrameworkCore;

public class TodoDb(DbContextOptions options) : DbContext(options)
{
    public DbSet<TodoItem> Todos { get; set; }
}