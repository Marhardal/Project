using System.ComponentModel.DataAnnotations;

namespace FrontEnd.Client.DTOs
{
    public class CategoryDTO
    {
        public Guid ID { get; set; }
        [Required]
        public string? Name { get; set; }
        public ICollection<ProjectDTO>? Projects { get; set; }
    }
}
