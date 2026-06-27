using Microsoft.AspNetCore.Identity;

namespace Project.Models
{
    public class TaskAssigneesModel
    {
        public Guid ID { get; set; }

        public Guid TaskID { get; set; }
        public TasksModel? Task { get; set; }

        public Guid UserID { get; set; } = default!;
        public UserModel? User { get; set; }
    }
}
