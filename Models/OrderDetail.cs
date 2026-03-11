using System.ComponentModel.DataAnnotations;

namespace MusicShoppingCartMvcUI.Models
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        [Required]
        public int OrderId { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }
        public Order Order { get; set; }
        public Product Product { get; set; }

    }
}
