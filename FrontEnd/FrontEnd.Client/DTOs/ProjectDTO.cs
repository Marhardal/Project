namespace FrontEnd.Client.DTOs
{
    public class ProjectDTO
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ProponentID { get; set; }
        public string? Name { get; set; }
        public string? Location { get; set; }
        public string? Description { get; set; }
        public DateTime SubmissionDate { get; set; }
        public DateTime createdOn { get; set; } = DateTime.Now;
        public DateTime updatedOn { get; set; }
        public ICollection<ProponentsDTO>? Proponents { get; set; }
        //public ICollection<TrackingModel>? Trackings { get; set; }
        public ProponentsDTO? Proponent { get; set; }
        //public TrackingModel? Tracking { get; set; }
    }
}
