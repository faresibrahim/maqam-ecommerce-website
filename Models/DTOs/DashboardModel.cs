using System.ComponentModel.DataAnnotations;

namespace MusicShoppingCartMvcUI.Models.DTOs
{
    public class DashboardModel
    {
        [Required]
        public int TotalProducts { get; set; }
        [Required]
        public int TotalCategories { get; set; }
        
        public int TotalOrders { get; set; }
    }
}
