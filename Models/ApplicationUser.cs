using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicShoppingCartMvcUI.Models
{

    //[Table("ApplicationUser")]
    public class ApplicationUser : IdentityUser
    {
      //  public int ApplicationUserId { get; set; }

        [MaxLength(50)]
        //public required string UserName { get; set; }

        
      //  public required string PhoneNumber { get; set; }

        public int CityId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public City City { get; set; } = default!;

        
    }
}
