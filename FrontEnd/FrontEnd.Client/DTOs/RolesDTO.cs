using System.ComponentModel.DataAnnotations;

namespace FrontEnd.Client.DTOs
{
    public class RolesDTO
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public List<PermissionDto>? Permissions { get; set; }
    }

    public record PermissionDto
    {
        public Guid PageActionId { get; init; }
        public string PageName { get; init; } = string.Empty;
        public string ActionName { get; init; } = string.Empty;
    }

    public record SavePermissionDto
    {
        public Guid PageActionId { get; init; }
        public bool IsGranted { get; init; }
    }

    public class PagesDTO
    {
        public Guid Id { get; set; } = new Guid();

        [Required]
        public string? Name { get; set; }

        //[Required]
        public string? Slug { get; set; }

        [Required]
        public string? Icon { get; set; }

        [Required]
        public int SortOrder { get; set; }

        //public string? Action { get; set; }
        public ICollection<PageActionsDTO>? PageActions { get; set; }
    }

    public class PageActionsDTO
    {
        public Guid Id { get; set; }

        [Required]
        public Guid PageID { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        //[Required]
        public string Slug { get; set; } = string.Empty;

        public PagesDTO? Page { get; set; }

        public ICollection<PermissionDto>? Permissions { get; set; }
    }
}
