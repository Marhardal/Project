using System.ComponentModel.DataAnnotations;

namespace FrontEnd.Client.DTOs
{
    public class StatusDTO
    {
        public Guid ID { get; set; } = Guid.NewGuid();

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Color { get; set; }
        [Required]
        public int SortOrder { get; set; }
        [Required]
        public Category Category { get; set; }

        public DateTime createdOn { get; set; }
        public DateTime updatedOn { get; set; }

        public TrackingDTO? Tracking { get; set; }
        public ICollection<TrackingDTO>? Trackings { get; set; }

    }

}
