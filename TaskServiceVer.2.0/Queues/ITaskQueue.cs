namespace TaskServiceVer._2._0.Queues
{
    public interface ITaskQueue
    {
        void Enqueue(Guid taskId);
        bool TryDequeue(out Guid taskId);
    }
}
