using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicShoppingCartMvcUI.Models
{
    [Table("ShoppingCart")]
    public class ShoppingCart
    {

        public int ShoppingCartId { get; set; }
        [Required]
        public string UserId { get; set; }
        public bool isDeleted { get; set; } = false;

        //The shopping cart have multiple cartDetails
        public ICollection<CartDetail> CartDetails { get; set; }
    }
}
