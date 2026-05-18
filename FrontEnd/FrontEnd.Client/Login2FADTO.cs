using System.ComponentModel.DataAnnotations;

namespace FrontEnd.Client
{
    public class Login2FADTO
    {
        public string? userID { get; set; }

        [Required]
        public string? Code { get; set; }
    }
}
