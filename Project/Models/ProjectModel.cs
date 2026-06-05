using System.ComponentModel.DataAnnotations;

namespace Project.Models
{

    public class ProjectModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public Guid ProponentID { get; set; }
        [Required]
        public Guid CategoryID { get; set; }
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
        public TrackingModel? Tracking { get; set; }
        //public LocationModel? Location { get; set; }
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
