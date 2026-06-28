namespace FrontEnd.Client.DTOs
{
    public class TaskAssigneesDTO
    {
        public Guid ID { get; set; }

        public Guid TaskID { get; set; }
        public TasksDTO? Task { get; set; }

        public Guid UserID { get; set; } = default!;
        public UserProfileDTO? User { get; set; }
    }
}
