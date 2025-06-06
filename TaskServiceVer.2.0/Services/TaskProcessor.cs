using System.Collections.Concurrent;
using TaskService.Models;
using TaskServiceVer._2._0.Queues;

namespace TaskServiceVer._2._0.Services
{
    public class TaskProcessor
    {
        private readonly ConcurrentDictionary<Guid, TaskItem> _tasks = new();
        private readonly ITaskQueue _queue;

        public TaskProcessor(ITaskQueue queue)
        {
            _queue = queue;
        }

        public TaskItem CreateTask()
        {
            var task = new TaskItem
            {
                Id = Guid.NewGuid(),
                Status = Status.Created,
                Timestamp = DateTime.UtcNow
            };

            _tasks[task.Id] = task;

            _queue.Enqueue(task.Id);

            return task;
        }

        public TaskItem? GetTaskById(Guid id)
        {
            return _tasks.TryGetValue(id, out var task) ? task : null;
        }

        public async Task ProcessTaskAsync(Guid id)
        {
            if (_tasks.TryGetValue(id, out var task))
            {
                task.Status = Status.Running;
                task.Timestamp = DateTime.UtcNow;

                await Task.Delay(TimeSpan.FromSeconds(30));

                task.Status = Status.Finished;
                task.Timestamp = DateTime.UtcNow;

            }
        }
    }
}
