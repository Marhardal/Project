namespace Project.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations.Schema;


    public class TrackingModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? userID { get; set; }
        public Guid ProjectID { get; set; }
        public Guid StatusID { get; set; }
        public DateTime assignedDate { get; set; }
        public DateTime? closingDate { get; set; }
        public DateTime createdOn { get; set; } = DateTime.Now;
        public DateTime? updatedOn { get; set; }

        [ForeignKey("ProjectID")]
        public ProjectModel? Project { get; set; }
        public Status? Status { get; set; }
        public ReviewModel? Review { get; set; }
        //public IdentityUser? Identity { get; set; }
        public UserModel? User { get; set; }

    }

}
