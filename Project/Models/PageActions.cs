using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class PageActions
    {
        public Guid Id { get; set; }

        [Required]
        public Guid PageID { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Slug { get; set; } = string.Empty;

        public PagesModel? Page { get; set; }

        public ICollection<PermissionModel>? Permissions { get; set; }
    }
}
