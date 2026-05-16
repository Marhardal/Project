using System.ComponentModel.DataAnnotations;

namespace Project.DTO
{
    public class Login2FADTO
    {
        public string? userID { get; set; }

        [Required]
        public string? Code { get; set; }
    }
}
