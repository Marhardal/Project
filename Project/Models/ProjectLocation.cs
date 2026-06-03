namespace Project.Models
{
    public class ProjectLocation
    {
        public Guid ID { get; set; }
        public Guid ProjectID { get; set; }
        public Guid LocationID { get; set; }
        //public ICollection<LocationModel>? Locations { get; set; }
        //public ICollection<ProjectModel>? Projects { get; set; }
        public ProjectModel? Project { get; set; }
        public LocationModel? Location { get; set; }
        public ICollection<ProjectLocation>? ProjectLocations { get; set; }
    }
}
