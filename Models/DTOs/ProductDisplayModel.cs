namespace MusicShoppingCartMvcUI.Models.DTOs
{
    public class ProductDisplayModel
    {
        
        public IEnumerable<Product> Products { get; set; } //a property to store the list of products displayed in the products page
        public IEnumerable<Category> Categories { get; set; } //a property to display the list of categories in the dropdown list

        public string STerm { get; set; } = "";
        public int CategoryId { get; set; } = 0;

    }
}
