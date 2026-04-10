namespace Project.Models
{
    namespace ProjectManager.Data.Model
    {
        public class Status
        {
            public Guid ID { get; set; } = Guid.NewGuid();

            public string? Name { get; set; }

            public string? Color { get; set; }

            public DateTime createdOn { get; set; }
            public DateTime updatedOn { get; set; }

            public TrackingModel? Tracking { get; set; }
            public ICollection<TrackingModel>? Trackings { get; set; }

        }
    }

}
