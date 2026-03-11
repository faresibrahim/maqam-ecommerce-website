using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = Microsoft.Build.Framework.RequiredAttribute;

namespace MusicShoppingCartMvcUI.Models.DTOs
{
    public class CheckoutModel
    {
        [Required]
        [MaxLength(30)]
        public string? Name { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(30)]
        public string? Email { get; set; }

        [Required]
        public string? MobileNumber { get; set; }
        [Required]
        [MaxLength]
        public string? Address { get; set; }
        [Required]
        public string? PaymentMethod { get; set; }
    }
}
