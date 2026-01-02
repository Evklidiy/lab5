using System.ComponentModel.DataAnnotations;

namespace AutoPartsWarehouse.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Не указан Email")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Запомнить меня?")]
        public bool RememberMe { get; set; }

        public string? ReturnUrl { get; set; }
    }
}