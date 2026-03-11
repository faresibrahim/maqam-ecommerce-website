using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicShoppingCartMvcUI.Models
{
    [Table("Category")]
    public class Category
    {
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(40)]
        public required string CategoryName { get; set; }

        public string? Icon { get; set; }
        
        public List<Product> Products { get; set; }

        

        //[MaxLength(100)]
        //public required string Slug { get; set; }

        //public int? ParentCategoryId { get; set; }


        //public int SortOrder { get; set; }

        //public Category? ParentCategory { get; set; }

        //public ICollection<Category> ChildCategories { get; set; }
        //= new List<Category>();

        //public ICollection<Product> Products { get; set; }
        //= new List<Product>();

    }
}
