namespace TaskService.Models
{
    public class TaskItem
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }
        public Status Status { get; set; }
    }

    public enum Status
    {
        Created,
        Running,
        Finished
    }
}