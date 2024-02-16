using System;
using TestEShopMacNet7.Models.Account;
using TestEShopMacNet7.Data;
using Microsoft.AspNetCore.Identity;

namespace TestEShopMacNet7.Models.Cart
{
	public class Order
	{
		public string[]? ItemIds { get; set; }
        public string Street { get; set; } = String.Empty;
        public string City { get; set; } = String.Empty;
        public int PostalCode { get; set; }
		public string PaymentMethod { get; set; } = String.Empty;
    }
}

