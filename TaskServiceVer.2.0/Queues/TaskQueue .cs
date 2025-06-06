using System.Collections.Concurrent;

namespace TaskServiceVer._2._0.Queues
{
    public class TaskQueue : ITaskQueue
    {
        private readonly ConcurrentQueue<Guid> _queue = new();

        public void Enqueue(Guid taskId) => _queue.Enqueue(taskId);
        public bool TryDequeue(out Guid taskId) => _queue.TryDequeue(out taskId);
    }
}
