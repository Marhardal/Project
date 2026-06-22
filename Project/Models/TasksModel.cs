namespace Project.Models
{
    public class TasksModel
    {
        public Guid ID { get; set; }

        public Guid ProjectID { get; set; }

        public Guid StatusID { get; set; }

        public string? Name { get; set; }

        public DateTime DueDate { get; set; }

        public Priority Priority { get; set; }

        public string? Description { get; set; }

        public DateTime createdOn { get; set; } = DateTime.Now;

        public DateTime updatedOn { get; set; } = DateTime.Now;

        public ProjectModel? Project { get; set; }

        public ICollection<TaskAssigneesModel>? TaskAssignees { get; set; }

    }

    public enum Priority
    {
        Urgent,
        High,
        Normal
    }
}
