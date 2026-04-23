namespace FrontEnd.Client.DTOs
{
    public class ReviewDTO
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public Guid TrackingID { get; set; }
        public string? Remarks { get; set; }
        public DateTime Date { get; set; }
        //public ResolvedStatus Status { get; set; }
        public DateTime createdOn { get; set; } = DateTime.UtcNow;
        public DateTime updatedOn { get; set; }
        public TrackingDTO? Tracking { get; set; }
    }
}
