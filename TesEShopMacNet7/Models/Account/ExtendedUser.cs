using System;
using Microsoft.AspNetCore.Identity;

namespace TestEShopMacNet7.Models.Account
{
	public class ExtendedUser : IdentityUser
	{
		public string Street { get; set; } = String.Empty;
		public string City { get; set; } = String.Empty;
		public int PostalCode { get; set; }
    }
}

