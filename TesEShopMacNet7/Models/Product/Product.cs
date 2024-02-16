using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestEShopMacNet7.Models.Product
{
	public class Product
	{
		[Key]
		public string Id { get; set; } = String.Empty;

		[Required (ErrorMessage = "Zadejte název produktu")]
        [Display(Name = "Název produktu")]
		public string Name { get; set; } = String.Empty;

        [Required(ErrorMessage = "Zadejte popis produktu")]
        [Display(Name = "Popis produktu")]
        public string Description { get; set; } = String.Empty;

        [Required(ErrorMessage = "Zadejte cenu produktu")]
        [Display(Name = "Cena produktu")]
        public int Price { get; set; }

        [Display(Name = "Cena po slevě")]
        public int SalePrice { get; set; }

        [Range(0, 5, ErrorMessage = "Hodnota je mimo rozsah [0 - 5]")]
		public double Rating { get; set; }

        //[ForeignKey(name: "Category")]
        public string CategoryId { get; set; } = String.Empty;

        //[Required(ErrorMessage = "Zadejte kategorii produktu")]
        [Display(Name = "Kategorie produktu")]
        public Category? Category { get; set; }

        //[ForeignKey(name:"Brand")]
        public string BrandId { get; set; } = String.Empty;

        //[Required(ErrorMessage = "Zadejte značku produktu")]
        [Display(Name = "Značka produktu")]
        public Brand? Brand { get; set; }

        public long TimeAdded { get; set; }

        [Required(ErrorMessage = "Zadejte počet produktů na skladě")]
        [Display(Name = "Počet produktů na skladě")]
        public int InStock { get; set; }
    }
}

