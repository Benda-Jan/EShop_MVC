using System;
namespace TestEShopMacNet7.Models.Cart
{
	public record CartItem
	{
		public string Id { get; set; } = String.Empty;
		public string UserId { get; set; } = String.Empty;
		public string ProductId { get; set; } = String.Empty;
		public string ProductName { get; set; } = String.Empty;
		public int Price { get; set; }
		public int Count { get; set; }
	}
}

