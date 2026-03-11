using System.ComponentModel.DataAnnotations;

namespace MusicShoppingCartMvcUI.Models.DTOs
{
    public class CategoryDTO
    {
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(40)]
        public string CategoryName { get; set; }

        [Required]
        public string Icon { get; set; }
    }
}