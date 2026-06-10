using Project.Models;

namespace Project.DTO
{
    public class RoleDTO
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public List<PermissionDto>? Permissions { get; set; }
    }

    public record SavePermissionDto
    {
        public Guid PageActionId { get; init; }
        public bool IsGranted { get; init; }
    }
}
