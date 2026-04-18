using System.ComponentModel.DataAnnotations;

namespace Project.Models
{

    public class Proponent
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        
        [Required]
        public string? Name { get; set; }
        
        [Required]
        public string? Location { get; set; }
        
        [Required]
        public string? Address { get; set; }

        public DateTime createdOn { get; set; } = DateTime.Now;

        public DateTime updatedOn { get; set; }

        public ICollection<ContactPerson>? Contacts { get; set; }
        public ContactPerson? Contact { get; set; }
        public ICollection<ProjectModel>? Projects { get; set; }
        public ProjectModel? Project { get; set; }
    }
}
