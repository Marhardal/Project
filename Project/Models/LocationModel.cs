namespace Project.Models
{
    public class LocationModel
    {
        public Guid ID { get; set; }

        public string? Location { get; set; }   

        public string? Code { get; set; }

        public ICollection<ProjectLocation>? ProjectLocations { get; set; }

        //public ProjectModel? Project { get; set; }
    }
}
