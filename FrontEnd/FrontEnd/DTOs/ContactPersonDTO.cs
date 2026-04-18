using System.ComponentModel.DataAnnotations;

namespace FrontEnd.DTOs
{
    public class ContactPersonDTO
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        // Note: change to Guid if Proponent.Id becomes Guid
        public Guid ProponentID { get; set; }
        [Required]
        public string? FullName { get; set; }
        [EmailAddress, Required]
        public string? Email { get; set; }
        [Phone, Required]
        public string? Phone { get; set; }
        public DateTime createdOn { get; set; } = DateTime.Now;
        public DateTime onUpdate { get; set; } = new DateTime(2000, 1, 1);
        public ProponentsDTO? Proponent { get; set; }
        public ICollection<ProponentsDTO>? Proponents { get; set; }
    }
}
