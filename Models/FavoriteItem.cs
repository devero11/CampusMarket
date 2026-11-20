using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace CampusMarket.Models
{
    public class FavoriteItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        public virtual IdentityUser User { get; set; } = null!;

        public int AdId { get; set; }
        public virtual Ad Ad { get; set; } = null!;


    }
}
