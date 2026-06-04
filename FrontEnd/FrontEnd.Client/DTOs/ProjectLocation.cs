namespace FrontEnd.Client.DTOs
{
    public class ProjectLocation
    {
        public Guid ID { get; set; }
        public Guid ProjectID { get; set; }
        public Guid LocationID { get; set; }
        //public ICollection<LocationModel>? Locations { get; set; }
        //public ICollection<ProjectModel>? Projects { get; set; }
        public ProjectDTO? Project { get; set; }
        public LocationDTO? Location { get; set; }
        public IEnumerable<Guid> SelectedLocationIds { get; set; } = new List<Guid>();
    }
}
