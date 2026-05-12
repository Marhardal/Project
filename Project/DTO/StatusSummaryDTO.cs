using Project.Models;

namespace Project.DTO
{
    public class StatusSummaryDTO
    {
        public string? Name { get; set; }
        public ProjectType Type { get; set; }
        public int Total { get; set; } = 0;
        public string? Color { get; set; }
    }
}
