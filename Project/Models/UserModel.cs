using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class UserModel
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        [Required]
        public string UserID { get; set; } = default!;
        public IdentityUser? identityUser { get; set; } = default!;
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? Surname { get; set; }
        [Required]
        public Gender Gender { get; set; }
        [Required]
        public Title Title { get; set; }
        [Required]
        public DateOnly DOB { get; set; }
        public DateTime createdOn { get; set; }
        public DateTime UpdateOn { get; set; } = DateTime.Now;

        public ICollection<TrackingModel>? Trackings { get; set; }

        public ICollection<TaskAssigneesModel>? TaskAssignees { get; set; }
    }
    
    public enum Title
    {
        Mr,
        Mrs,
        Miss
    }
 
    public enum Gender
    {
        Male,
        Female
    }
}
