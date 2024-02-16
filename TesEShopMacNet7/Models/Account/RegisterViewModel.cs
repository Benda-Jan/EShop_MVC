using System;
using System.ComponentModel.DataAnnotations;

namespace TestEShopMacNet7.Models.Account
{
	public record RegisterViewModel
	{
        public string Id { get; private set; } = String.Empty;
        [Required(ErrorMessage = "Vyplňte, prosím, svůj e-mail.")]
        [Display(Name = "E-Mail")]
        public string Email { get; set; } = String.Empty;
        [Required(ErrorMessage = "Vyplňte, prosím, heslo.")]
        [DataType(DataType.Password)]
        [Display(Name = "Heslo")]
        public string Password { get; set; } = String.Empty;
        [Required(ErrorMessage = "Potvrďte heslo.")]
        [Display(Name = "Ověření hesla")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Hesla nejsou shodná.")]
        public string ConfirmPassword { get; set; } = String.Empty;
        [Display(Name = "Pamatuj si mě?")]
        public bool StaySignedIn { get; set; }
    }
}

