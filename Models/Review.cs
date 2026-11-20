using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
namespace CampusMarket.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        public string? Text { get; set; }
        [Range(1,5, ErrorMessage = "Rating-ul trebuie sa fie intre 1 si 5.")]
        public int? Rating { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;


        //cheie externa- rel cu tabel ad 
        public int AdId { get; set; }
        public virtual Ad Ad { get; set; } = null!;


        //cheie externa-rel cu tab user
        public string UserId { get; set; } = string.Empty;
        public virtual IdentityUser User { get; set; } = null!;
    }
}
