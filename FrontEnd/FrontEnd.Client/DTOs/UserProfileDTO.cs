
using System.ComponentModel.DataAnnotations;

    public class UserProfileDTO
    {
        public Guid ID { get; set; }
        [Required]
        public Guid UserID { get; set; }
        //public IdentityUser identityUser { get; set; } = default!;
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? Surname { get; set; }
        [Required]
        public string? Username { get; set; }
        [Required, EmailAddress]
        public string? Email { get; set; }
        [Required, Phone]
        public string? Phone { get; set; }
        [Required]
        public Gender Gender { get; set; }
        [Required]
        public Title Title { get; set; }
        [Required]
        public DateOnly DOB { get; set; }
        public DateTime createdOn { get; set; }
        public DateTime UpdateOn { get; set; }

    }

    public enum Title
    {
        Mr,
        Mrs,
        Miss
    }

    public enum Gender
    {
        Male,
        Female
    }

