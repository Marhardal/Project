using Project.Models;
using System.ComponentModel.DataAnnotations;

namespace Project.DTO
{
    public class ProjectDTO
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public Guid? ProponentID { get; set; }
        [Required]
        public Guid? CategoryID { get; set; }
        public Guid? StatusID { get; set; }
        public Guid? ContactPersonID { get; set; }
        
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
        //public ICollection<TrackingDTO> Trackings { get; set; } = new List<TrackingDTO>();

        public ICollection<ProjectLocation>? ProjectLocations { get; set; }
        public TrackingModel? Tracking { get; set; }
        public CategoryModel? Category { get; set; }
        public Proponent? Proponent { get; set; }
        public HashSet<Guid> SelectedLocationIds { get; set; } = new();

    }

}
