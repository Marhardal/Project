namespace Project.Models
{

    public class Proponent
    {
        public Guid ID { get; set; } = Guid.NewGuid();

        public string? Name { get; set; }

        public string? Location { get; set; }

        public DateTime createdOn { get; set; } = DateTime.Now;

        public DateTime updatedOn { get; set; }

        //public ICollection<ContactPerson>? Contacts { get; set; }
        //public ContactPerson? Contact { get; set; }
        //public ICollection<ProjectModel>? Projects { get; set; }
        //public ProjectModel? Project { get; set; }
    }
}
