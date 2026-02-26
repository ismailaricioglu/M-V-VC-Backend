using Entities.Models;

namespace StoreApp.Models
{
    /// <summary>
    /// Ürün listeleme sayfasý için ürünler ve sayfalama bilgilerini taþýyan view modeli.
    /// </summary>
    public class ProductListViewModel
    {
        public IEnumerable<Product> Products { get; set; } = Enumerable.Empty<Product>();
        public Pagination Pagination { get; set; } = new();
        public int TotalCount => Products.Count();
    }
}