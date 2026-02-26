using Entities.Models;

namespace Repositories.Extensions
{
    public static class ProductRepositoryExtension
    {
        /// <summary>
        /// Verilen ürün listesi üzerinde kategori ID'sine göre filtreleme yapan uzantý metodudur.
        /// Eðer categoryId null ise, tüm ürünleri döndürür; aksi halde eþleþen kategoriye sahip ürünleri döndürür.
        /// </summary>
        /// <param name="products">Filtrelenecek ürün koleksiyonu.</param>
        /// <param name="categoryId">Filtreleme için kullanýlacak kategori ID'si.</param>
        /// <returns>Kategori ID'sine göre filtrelenmiþ ürünler koleksiyonu.</returns>
        public static IQueryable<Product> FilteredByCategoryId(this IQueryable<Product> products,
            int? categoryId)
        {
            if (categoryId is null)
                return products;
            else
                return products.Where(prd => prd.CategoryId.Equals(categoryId));
        }

        /// <summary>
        /// Ürün koleksiyonu üzerinde ürün adýný içeren arama terimine göre filtreleme yapan uzantý metodudur.
        /// Arama terimi boþ, null veya sadece boþluklardan oluþuyorsa filtre uygulanmadan tüm ürünler döndürülür.
        /// </summary>
        /// <param name="products">Filtreleme yapýlacak ürün koleksiyonu.</param>
        /// <param name="searchTerm">Ürün adýnda aranacak ifade.</param>
        /// <returns>Arama terimini içeren ürün adlarýna sahip filtrelenmiþ ürün koleksiyonu.</returns>
        public static IQueryable<Product> FilteredBySearchTerm(this IQueryable<Product> products,
            String? searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return products;
            else
                return products.Where(prd => prd.ProductName.ToLower()
                    .Contains(searchTerm.ToLower()));
        }

        /// <summary>
        /// Ürün koleksiyonunu, geçerli minimum ve maksimum fiyat aralýðýna göre filtreleyen uzantý metodudur.
        /// Fiyat aralýðý geçerli deðilse (isValidPrice false ise), filtre uygulanmadan tüm ürünler döndürülür.
        /// </summary>
        /// <param name="products">Filtreleme yapýlacak ürün koleksiyonu.</param>
        /// <param name="minPrice">Alt fiyat sýnýrý.</param>
        /// <param name="maxPrice">Üst fiyat sýnýrý.</param>
        /// <param name="isValidPrice">Fiyat aralýðýnýn geçerli olup olmadýðýný belirten bayrak.</param>
        /// <returns>Belirtilen fiyat aralýðýnda olan ürünlerin koleksiyonu.</returns>
        public static IQueryable<Product> FilteredByPrice(this IQueryable<Product> products,
            int minPrice, int maxPrice, bool isValidPrice)
        {
            if (isValidPrice)
                return products.Where(prd => prd.Price >= minPrice && prd.Price <= maxPrice);
            else
                return products;
        }

        /// <summary>
        /// Ürün sorgusu üzerinde sayfalama (pagination) uygular.
        /// </summary>
        /// <param name="products">Sayfalama iþlemi uygulanacak ürün sorgusu.</param>
        /// <param name="pageNumber">Görüntülenecek sayfa numarasý (1 tabanlý).</param>
        /// <param name="pageSize">Her sayfada gösterilecek ürün sayýsý.</param>
        /// <returns>Belirtilen sayfa numarasýna ve sayfa boyutuna göre filtrelenmiþ ürün sorgusu.</returns>
        public static IQueryable<Product> ToPaginate(this IQueryable<Product> products,
            int pageNumber, int pageSize)
        {
            return products
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);
        }
    }
}