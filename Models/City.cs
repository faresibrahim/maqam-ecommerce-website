namespace MusicShoppingCartMvcUI.Models
{
    public class City
    {
        public int CityId { get; set; }

        public required string Name { get; set; }

        public Boolean IsActive { get; set; } = true;

        public ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();

        public ICollection<Product> Listings { get; set; } = new List<Product>();
        

    }
}
