using System;
namespace TestEShopMacNet7.Models.Account
{
	public class ResetPasswordViewModel
	{
		public string userId { get; set; } = String.Empty;
		public string resetToken { get; set; } = String.Empty;
		public string firstPassword { get; set; } = String.Empty;
		public string secondPassword { get; set; } = String.Empty;
    }
}

