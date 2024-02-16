using System;
using System.ComponentModel.DataAnnotations;

namespace TestEShopMacNet7.Models.Product
{
	public record Brand
	{
        [Key]
        public string Id { get; set; } = String.Empty;
        [Display(Name = "Název značky")]
        public string Name { get; set; } = String.Empty;

        public Brand(string Name)
		{
            this.Id = Guid.NewGuid().ToString();
            this.Name = Name;
		}
	}
}

