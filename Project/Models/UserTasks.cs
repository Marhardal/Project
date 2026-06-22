namespace Project.Models
{
    public class UserTasks
    {
        public Guid ID { get; set; }

        public Guid TaskID { get; set; }

        public string? UserID { get; set; } = default!;

        public ICollection<UserTasks>? UsersTasks { get; set; }
    }
}
