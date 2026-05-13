namespace FrontEnd.Client.DTOs
{
    public class ProjectMonthDTO
    {
        public int Year { get; set; }

        public int Month { get; set; }

        public string? MonthName { get; set; }

        public int Total { get; set; }

        public ProjectType Type { get; set; }
    }
}
