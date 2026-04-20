using System.ComponentModel.DataAnnotations;

namespace FrontEnd.DTOs
{
    public class StatusDTO
    {
        public Guid ID { get; set; } = Guid.NewGuid();

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Color { get; set; }

        public DateTime createdOn { get; set; }
        public DateTime updatedOn { get; set; }

    }
}
