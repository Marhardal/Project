using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{

    public class ProjectModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public Guid ProponentID { get; set; }
        [Required]
        public Guid CategoryID { get; set; }
        public Guid ContactPersonID { get; set; }
        
        [Required, NotMapped]
        public Guid StatusID { get; set; }
        [Required]
        public string? Name { get; set; }
        
        [Required]
        public string? Description { get; set; }
        [Required]
        public ProjectType ProjectType { get; set; }
        public DateTime assignedDate { get; set; }
        public DateTime? closingDate { get; set; }
        public DateTime SubmissionDate { get; set; } = DateTime.Now;
        public DateTime createdOn { get; set; } 
        public DateTime updatedOn { get; set; }
        //public ICollection<Proponent>? Proponents { get; set; }
        public Proponent? Proponent { get; set; }
        public CategoryModel? Category { get; set; }
        public ICollection<ProjectLocation>? ProjectLocations { get; set; }
        public ICollection<TrackingModel>? Trackings { get; set; }

        public ICollection<TasksModel>? Tasks { get; set; }

        //public TrackingModel? Tracking { get; set; }
        //public LocationModel? Location { get; set; }
        [NotMapped]
        public HashSet<Guid> SelectedLocationIds { get; set; } = new();
        public ContactPerson? Contact { get; set; }
    }

    public enum ProjectType
    {
        Brief,
        ESMP,
        Audit,
        ESIA
    }

    //public enum Proposal
    //{
    //    NotStarted,
    //    Submitted,
    //    Approved,
    //    Rejected
    //}
}
