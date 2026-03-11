namespace MusicShoppingCartMvcUI.Models
{
    public class ProductImage
    {
        public int ProductImageId { get; set; }
        public required int ProductId { get; set; }
        public required string ImageUrl { get; set; }
        public int SortOrder { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public required Product Product { get; set; }
    }
}
