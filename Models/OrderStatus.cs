using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicShoppingCartMvcUI.Models
{
    [Table("OrderStatus")]
    public class OrderStatus
    {
        public int Id { get; set; }

        [Required, MaxLength(20)]
        public string? StatusName { get; set; } 
        public int StatusId { get; set; }
    }
}
