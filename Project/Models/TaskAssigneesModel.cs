using Microsoft.AspNetCore.Identity;

namespace Project.Models
{
    public class TaskAssigneesModel
    {
        public Guid ID { get; set; }

        public Guid TaskID { get; set; }
        public TasksModel? Task { get; set; }

        public string? UserID { get; set; } = default!;
        public IdentityUser? User { get; set; }
    }
}
