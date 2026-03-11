using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MusicShoppingCartMvcUI.Models.DTOs
{
    public class ProductDTO
    {
        public int ProductId { get; set; }

        [Required]
        [MaxLength(40)]
        public string? ProductName { get; set; }

        [Required]
        [MaxLength(40)]
        public string? Description { get; set; }
        public decimal Price { get; set; }

        public string? Status { get; set; }

        public string? Image { get; set; }
        public int CategoryId { get; set; }

        public IFormFile? ImageFile { get; set; }
        public IEnumerable<SelectListItem>? CategoryList { get; set; } //dropdown

    }
}
