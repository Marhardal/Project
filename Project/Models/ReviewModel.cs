using Project.Models.ProjectManager.Data.Model;

namespace Project.Models
{
    public class ReviewModel
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public Guid TrackingID { get; set; }
        public string? Remarks { get; set; }
        public DateTime Date { get; set; }
        public ResolvedStatus Status { get; set; }
        public DateTime createdOn { get; set; } = DateTime.UtcNow;
        public DateTime updatedOn { get; set; }
        public TrackingModel? Tracking { get; set; }
    }
    public enum ResolvedStatus
    {
        Resolved,
        Unresolved
    }
}
