using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace FrontEnd.Client.DTOs
{
    public class ProjectDTO
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public Guid ProponentID { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Location { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public ProjectType ProjectType { get; set; }
        public DateTime assignedDate { get; set; }
        public DateTime? closingDate { get; set; }
        public DateTime SubmissionDate { get; set; } = DateTime.Now;
        public DateTime createdOn { get; set; } = DateTime.Now;
        public DateTime updatedOn { get; set; }
        //public ICollection<Proponent>? Proponents { get; set; }
        public ProponentsDTO? Proponent { get; set; }
        // Initialize Trackings to an empty collection to avoid null reference or empty-sequence issues in the UI
        public ICollection<TrackingDTO> Trackings { get; set; } = new List<TrackingDTO>();
        //public TrackingDTO? Tracking { get; set; }

    }
}
