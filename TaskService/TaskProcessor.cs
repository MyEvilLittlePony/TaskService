using Microsoft.EntityFrameworkCore;
using TaskService.Context;
using TaskService.Models;

namespace TaskService
{
    public class TaskProcessor
    {
        private readonly IDbContextFactory<TaskDbContext> _dbFactory;

        public TaskProcessor(IDbContextFactory<TaskDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<Guid> CreateTaskAsync()
        {
            using var db = _dbFactory.CreateDbContext();

            var task = new TaskItem
            {
                Id = Guid.NewGuid(),
                Timestamp = DateTime.UtcNow,
                Status = Status.Created
            };

            db.Tasks.Add(task);
            await db.SaveChangesAsync();

            _ = ProcessTaskAsync(task.Id);

            return task.Id;
        }

        public async Task ProcessTaskAsync(Guid id)
        {
            using var db = _dbFactory.CreateDbContext();

            var task = await db.Tasks.FindAsync(id);
            if (task == null) return;

            task.Timestamp = DateTime.UtcNow;
            task.Status = Status.Running;
            await db.SaveChangesAsync();

            await Task.Delay(TimeSpan.FromMinutes(2));

            task.Timestamp = DateTime.UtcNow;
            task.Status = Status.Finished;
            await db.SaveChangesAsync();
        }

        public async Task<TaskItem?> GetTaskAsync(Guid id)
        {
            using var db = _dbFactory.CreateDbContext();
            return await db.Tasks.FindAsync(id);
        }
    }
}
