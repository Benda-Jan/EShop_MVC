using System;
using System.ComponentModel.DataAnnotations;

namespace TestEShopMacNet7.Models.Account
{
    public record LoginViewModel
	{
		public string Id { get; private set; } = String.Empty;
        [Required(ErrorMessage = "Vyplňte, prosím, svůj e-mail.")]
		[Display(Name = "E-Mail")]
		public string Email { get; set; } = String.Empty;
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Vyplňte, prosím, heslo.")]
        [Display(Name = "Heslo")]
        public string Password { get; set; } = String.Empty;
        [Display(Name = "Pamatuj si mě?")]
        public bool StaySignedIn { get; set; }
    }
}

