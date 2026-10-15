using System.ComponentModel.DataAnnotations;

namespace MusicShoppingCartMvcUI.Models.DTOs
{
    public class CheckoutModel
    {
        [Required]
        [MaxLength(30)]
        [Display(Name = "Full name")]
        public string? Name { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(30)]
        public string? Email { get; set; }

        [Required]
        [Display(Name = "Mobile number")]
        public string? MobileNumber { get; set; }
        [Required]
        [MaxLength(200)]
        public string? Address { get; set; }
        [Required]
        [Display(Name = "Payment method")]
        public string? PaymentMethod { get; set; }
    }
}
