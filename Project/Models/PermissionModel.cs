using Microsoft.AspNetCore.Identity;
using Project.Migrations;

namespace Project.Models
{
    public class PermissionModel
    {
        public Guid Id { get; set; }
        public string RoleId { get; set; } = string.Empty;  // FK → AspNetRoles.Id
        public Guid PageActionId { get; set; }
        public bool IsGranted { get; set; }
        public IdentityRole? Role { get; set; }
        public PageActions? PageAction { get; set; }
    }
}
