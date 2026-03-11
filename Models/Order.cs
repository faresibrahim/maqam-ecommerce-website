using System.ComponentModel.DataAnnotations;

namespace MusicShoppingCartMvcUI.Models
{
    public class Order
    {

        public int OrderId { get; set; }
        [Required]
        public string UserId { get; set; }

        public DateTime CreatedAt {  get; set; } = DateTime.Now;
        [Required]
        public int OrderStatusId { get; set; }
        public bool isDeleted { get; set; } = false;
        [Required]
        [MaxLength(30)]
        public string? Name { get; set; }

        [Required]
        public string? Email { get; set;}

        [Required]
        public string? MobileNumber { get; set; }
        [Required]
        [MaxLength(200)]
        public string? Address { get; set; }

        [Required]
        [MaxLength(30)]
        public string? PaymentMethod { get; set; }
        public bool IsPaid { get; set; }


        public OrderStatus OrderStatus { get; set; }

        public List<OrderDetail> OrderDetail { get; set; }
    }
}
