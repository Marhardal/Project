using System.ComponentModel.DataAnnotations;

namespace FrontEnd.Client.DTOs
{
    public class ProponentsDTO
    {
        // Do not generate a new Guid by default. When deserializing server responses
        // we want the incoming ID (if provided) to be preserved. If the server omits
        // the ID it should remain Guid.Empty instead of creating a misleading value.
        public Guid ID { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Location { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;

        public DateTime createdOn { get; set; } = DateTime.Now;

        public DateTime updatedOn { get; set; }

        public ICollection<ContactPersonDTO>? Contacts { get; set; }
        //public ContactPersonDTO? Contact { get; set; }
        public ICollection<ProjectDTO>? Projects { get; set; }
        public ProjectDTO? Project { get; set; }
    }

}
