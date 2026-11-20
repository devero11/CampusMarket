using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CampusMarket.Models
{
    public class SellerRequest
    {
        [Key]
        public int Id { get; set; }

        public string Status { get; set; } = "In asteptare";

        public DateTime RequestDate { get; set; } = DateTime.Now;

        [Required]
        public string UserId {get; set; } = string.Empty ;
        public virtual IdentityUser User { get; set; } = null!;
        
    }
}
