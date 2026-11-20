
using System.ComponentModel.DataAnnotations;
namespace CampusMarket.Models
{
    public class Faq
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Question { get; set;  } = string.Empty;

        [Required]
        public string Answer { get; set; } = string.Empty;


        public int AdId { get; set; }
        public virtual Ad Ad { get; set; } = null!;
    }
}
