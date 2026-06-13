namespace Project.DTO
{
    public record PermissionDto
    {
        public Guid PageActionId { get; init; }
        public string PageName { get; init; } = string.Empty;
        public string Icon { get; init; } = string.Empty;
        public string ActionName { get; init; } = string.Empty;
        public string Slug { get; init; } = string.Empty;
    }
}
