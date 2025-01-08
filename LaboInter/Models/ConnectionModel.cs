using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LaboInter.Models
{
    public class ConnectionModel
    {
        [DisplayName("Email: ")]
        [Required(ErrorMessage = "L'email est requis")]
        public required string Email { get; set; }
        [DisplayName("Password: ")]
        [Required(ErrorMessage = "Le Password est requis")]
        public required string Password { get; set; }
    }
}
