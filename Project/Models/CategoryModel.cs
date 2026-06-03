using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class CategoryModel
    {
        public Guid ID { get; set; }
        [Required]
        public string? Name { get; set; }
        public string? Code { get; set; }

        public ICollection<ProjectModel>? Projects { get; set; }
    }
}
