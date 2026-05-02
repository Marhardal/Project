using Microsoft.AspNetCore.Identity;
using Project.Models;
using System.ComponentModel.DataAnnotations;

namespace Project.DTO
{
    public class UserDTO
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
    }
}
