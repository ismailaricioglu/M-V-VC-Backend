namespace Entities.Models
{
    /// <summary>
    /// Bir kimlik ve isme sahip bir kategoriyi temsil eder.
    /// </summary>
    public class Category
    {
        public int CategoryId { get; set; }
        public String? CategoryName { get; set; } = String.Empty;

        public ICollection<Product> Products { get; set; } // İlgili ürünler için navigasyon özelliği
    }
}