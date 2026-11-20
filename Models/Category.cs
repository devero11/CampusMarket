
using System.ComponentModel.DataAnnotations;

namespace CampusMarket.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Numele Categoriei este obligatoriu!")]
        [Display(Name = "Nume Categorie")]
        public string Name { get; set; } = string.Empty;

        public virtual ICollection<Ad> Ads { get; set; } =  new List<Ad>();

    }
}
