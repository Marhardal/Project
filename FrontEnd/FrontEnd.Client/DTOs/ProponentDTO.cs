using System.ComponentModel.DataAnnotations;

namespace FrontEnd.Client.DTOs
{
    public class ProponentsDTO
    {
        public Guid ID { get; set; } = Guid.NewGuid();

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Location { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;

        public DateTime createdOn { get; set; } = DateTime.Now;

        public DateTime updatedOn { get; set; }

        //public ICollection<ContactPersonDTO>? Contacts { get; set; }
        //public ContactPersonDTO? Contact { get; set; }
        //public ICollection<ProjectModel>? Projects { get; set; }
        //public ProjectModel? Project { get; set; }
    }

}
