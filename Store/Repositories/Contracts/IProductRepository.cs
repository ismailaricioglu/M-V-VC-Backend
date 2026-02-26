using Entities.Models;
using Entities.RequestParameters;

namespace Repositories.Contracts
{
    /// <summary>
    /// Ürün deposu arayüzü
    /// </summary>
    public interface IProductRepository : IRepositoryBase<Product>
    {
        /// <summary>
        /// Tüm ürünleri getirir
        /// <param name="trackChanges">Değişiklikleri izleme durumu</param>
        /// </summary>
        IQueryable<Product> GetAllProducts(bool trackChanges);

        /// <summary>
        /// Parametreye bağlı Tüm ürünleri getirir
        /// </summary>
        /// <param name="p">Ürün istek parametreleri</param>
        /// <returns></returns>
        IQueryable<Product> GetAllProductsWithDetails(ProductRequestParameters p);

        /// <summary>
        /// Tüm vitrin ürünlerini getirir
        /// <param name="trackChanges">Değişiklikleri izleme durumu</param>
        /// </summary>
        IQueryable<Product> GetShowcaseProducts(bool trackChanges);

        /// <summary>
        /// Belirtilen ID'ye sahip bir ürünü getirir
        /// <param name="id">Ürün ID'si</param>
        /// <param name="trackChanges">Değişiklikleri izleme durumu</param>
        /// </summary>
        Product? GetOneProduct(int id, bool trackChanges);

        /// <summary>
        /// Yeni bir kayıt oluşturur.
        /// </summary>
        /// <param name="product">Ürün nesnesi</param>
        void CreateOneProduct(Product product);
        
        /// <summary>
        /// Ürünü siler.
        /// </summary>
        /// <param name="product">Silinecek ürün nesnesi</param>
        void DeleteOneProduct(Product product);

        /// <summary>
        /// Ürünü günceller.
        /// </summary>
        /// <param name="product">Güncellenecek ürün nesnesi</param>
        void UpdateOneProduct(Product product);
    }
}