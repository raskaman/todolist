using System.Data.Entity;

namespace TodoList.Models
{
    public class TodoListContext : DbContext
    {
        public DbSet<Task> Tasks { get; set; }
        public DbSet<User> Users { get; set; }

    }
}