using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    /// <summary>
    /// Bir kimlik, ad ve fiyata sahip bir ürünü temsil eder.
    /// </summary>
    public class Product
    {
        public int ProductId { get; set; }
        public String? ProductName { get; set; } = String.Empty;
        public decimal Price { get; set; }
        public String? Summary { get; set; } = String.Empty;
        public String? ImageUrl { get; set; }
        public int? CategoryId { get; set; } // (Foreign key) Kategori için yabancı anahtar
        public Category? Category { get; set; } // (Navigation property) Kategori için gezinme özelliği
        public bool ShowCase { get; set; } // Vitrin ürünü mü?
    }
}