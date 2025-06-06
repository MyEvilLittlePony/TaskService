using TaskService;
using TaskServiceVer._2._0.Queues;

namespace TaskServiceVer._2._0.Services
{
    public class TaskWorker : BackgroundService
    {
        private readonly ITaskQueue _queue;
        private readonly ILogger<TaskWorker> _logger;
        private readonly TaskProcessor _taskProcessor;

        public TaskWorker(ITaskQueue queue, ILogger<TaskWorker> logger, TaskProcessor taskProcessor)
        {
            _queue = queue;
            _logger = logger;
            _taskProcessor = taskProcessor;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_queue.TryDequeue(out var taskId))
                {
                    try
                    {
                        await _taskProcessor.ProcessTaskAsync(taskId);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Ошибка при обработке фоновой задачи {TaskId}", taskId);
                    }
                }

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
