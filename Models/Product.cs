using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MusicShoppingCartMvcUI.Models
{

    [Table("Product")]
    public class Product
    {

        public int ProductId { get; set; }

        [Required]
        [MaxLength(40)]
        public string? ProductName { get; set; }
        public string? Image { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }
        
        public string? Status { get; set; }
        
       

        [Required]
        public int CategoryId { get; set; }


        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? PublishedAt { get; set; }

        public DateTime? SoldAt { get; set; }

        public Category Category { get; set; }
      
        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();

        public List<OrderDetail> OrderDetail { get; set; }
        public List<CartDetail> CartDetail { get; set; }
        
        public Stock? Stocks { get; set; }


        [NotMapped]
        public string CategoryName { get; set; }
        [NotMapped]
        public int Quantity { get; set; }
    }
}
