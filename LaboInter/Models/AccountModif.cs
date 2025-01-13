using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace LaboInter.Models
{
    public class AccountModif
    {
        [DisplayName("Nouvelle Addresse: ")]
        [Required(ErrorMessage = "La Nouvelle addresse est requis")]
        public required string NewAddress { get; set; }
        [DisplayName("Ancien Password: ")]
        [Required(ErrorMessage = "L'Ancient Password est requis")]
        public required string OldPassword { get; set; }
        [DisplayName("Nouveau Password: ")]
        [Required(ErrorMessage = "Le Nouveau Password est requis")]
        public required string NewPassword { get; set; }
    }
}
