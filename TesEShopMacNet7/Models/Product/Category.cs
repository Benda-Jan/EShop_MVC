using System;
using System.ComponentModel.DataAnnotations;

namespace TestEShopMacNet7.Models.Product
{
	public record Category
	{
		[Key]
		public string Id { get; set; } = String.Empty;
		[Display(Name = "Interní název")]
		public string InternalName { get; set; } = String.Empty;
        [Display(Name = "Zobrazený název")]
        public string DisplayName { get; set; } = String.Empty;
    }
}

