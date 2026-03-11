namespace MusicShoppingCartMvcUI.Models.DTOs
{
    public class OrderDetailModelDTO
    {
        public string DivId { get; set; }
        public IEnumerable<OrderDetail> OrderDetails { get; set; }
    }
}
