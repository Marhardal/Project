using Project.Migrations;
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class PagesModel
    {
        public Guid Id { get; set; } = new Guid();

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Slug { get; set; }

        [Required]
        public string? Icon { get; set; }

        [Required]
        public int SortOrder { get; set; }
        public ICollection<PageActions>? PageActions { get; set; }
    }
}
