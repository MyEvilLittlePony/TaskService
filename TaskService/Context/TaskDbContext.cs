using Microsoft.EntityFrameworkCore;
using TaskService.Models;

namespace TaskService.Context
{
    public class TaskDbContext : DbContext
    {
        public DbSet<TaskItem> Tasks => Set<TaskItem>();

        public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options) { }
    }
}