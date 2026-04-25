namespace Project.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    namespace ProjectManager.Data.Model
    {
        public class TrackingModel
        {
            public Guid Id { get; set; } = Guid.NewGuid();
            public Guid userID { get; set; }
            public Guid ProjectID { get; set; }
            public Guid StatusID { get; set; }
            public DateTime assignedDate { get; set; }
            public DateTime? closingDate { get; set; }
            public DateTime createdOn { get; set; } = DateTime.Now;
            public DateTime? updatedOn { get; set; }
            //public ICollection<ProjectModel>? Projects { get; set; }
            //public ICollection<Status>? Statuses { get; set; }
            [ForeignKey("ProjectID")]
            public ProjectModel? Project { get; set; }
            [ForeignKey("StatusID")]
            public Status? Status { get; set; }
            public ReviewModel? Review { get; set; }

        }

    }

}
