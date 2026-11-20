using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace CampusMarket.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Cantitatea este obligatorie")]
        [Range(1, int.MaxValue, ErrorMessage = "Cantitatea trebuie sa fie minimum 1.")]
        public int Quantity { get; set;  }

        public int AdId { get; set; }
        public virtual Ad Ad { get; set; } = null!;

        [Required]
        public string UserId { get; set; } = string.Empty;
        public virtual IdentityUser User { get; set; } = null!;


    }
}
