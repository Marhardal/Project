using FrontEnd.Client.DTOs;
using System.ComponentModel.DataAnnotations;

public class UserProfileDTO
{
    public Guid ID { get; set; }
    [Required]
    public string UserID { get; set; }
    [Required]
    public string? FirstName { get; set; }
    [Required]
    public string? Surname { get; set; }
    [Required]
    public Gender Gender { get; set; }
    [Required]
    public Title Title { get; set; }
    [Required]
    public DateOnly DOB { get; set; }
    public DateTime createdOn { get; set; }
    public DateTime UpdateOn { get; set; } = DateTime.Now;
    public IdentityDTO? IdentityDTO { get; set; }

}



