
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace CampusMarket.Models
{
    public class Ad
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Titlul este obligatoriu.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Descrierea este obligatorie.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Imaginea este obligatorie.")]
        public string ImageUrl { get; set; } = string.Empty;

        [Required(ErrorMessage = "Pretul este obligatoriu.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Pretul trebuie sa fie mai mare ca 0.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Stocul este obligatoriu.")]
        [Range(0, int.MaxValue, ErrorMessage = "Stocul trebuie sa fie mai mare sau egal ca 0.")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "Starea produsului este obligatorie.")]
        [Display(Name = "Starea produsului")]
        public string Condition { get; set; } = string.Empty;

        public string Status { get; set; } = "In asteptare";

        public string? AdminFeedback { get; set; }

        public double AverageRating { get; set; } = 0;
        public bool isActive { get; set; } = true;


        [Required(ErrorMessage = "Categoria este obligatorie.")]
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; } 


        [Required]
        public string SellerId { get; set; } = string.Empty;
        public virtual IdentityUser? Seller { get; set; } 

        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
        public virtual ICollection<Faq> Faqs { get; set; } = new List<Faq>();
        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public virtual ICollection<FavoriteItem> FavoriteItems { get; set; } = new List<FavoriteItem>();


        [NotMapped]
        public IEnumerable<SelectListItem>? Categ { get; set; }
    }
}
