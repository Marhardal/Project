namespace Project.Models
{
    public class ContactPerson
    {
        // Primary key switched from int to Guid
        public Guid Id { get; set; } = Guid.NewGuid();

        // Note: change to Guid if Proponent.Id becomes Guid
        public Guid ProponentID { get; set; }

        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime createdOn { get; set; } = DateTime.Now;
        public DateTime onUpdate { get; set; } = new DateTime(2000, 1, 1);
        public Proponent? Proponent { get; set; }
        public ICollection<Proponent>? Proponents { get; set; }
    }
}
