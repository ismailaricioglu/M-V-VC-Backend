using System.Data.Common;
using Entities.Models;
using Entities.RequestParameters;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using Repositories.Extensions;
using SQLitePCL;

namespace Repositories
{
    /// <summary>
    /// Ürün deposu sınıfı, RepositoryBase ve IProductRepository arayüzünü uygular.
    /// Bu sınıf sealed ifadesi ile mühürlenmiştir, sınıf kalıtılamaz yani geliştirilemez
    /// Extensions ile Genişletmeye açıktır
    /// </summary>
    public sealed class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        /// <summary>
        /// RepositoryContext parametresi ile ProductRepository sınıfının yapıcı metodunu başlatır.
        /// </summary>
        /// <param name="context">Veritabanı bağlamı</param>
        public ProductRepository(RepositoryContext context) : base(context)
        {
        }

        /// <summary>
        /// Yeni bir ürün oluşturur.
        /// /// </summary>
        /// <param name="product">Oluşturulacak ürün</param>
        public void CreateOneProduct(Product product) => Create(product);

        /// <summary>
        /// Ürünü siler.
        /// /// </summary>
        /// <param name="product">Silinecek ürün</param>
        public void DeleteOneProduct(Product product) => Remove(product);

        /// <summary>
        /// Tüm ürünleri getirir.
        /// </summary>
        /// <param name="trackChanges">Değişiklikleri izleme durumu</param>
        /// <returns>Tüm ürünlerin sorgulanabilir listesi</returns>
        public IQueryable<Product> GetAllProducts(bool trackChanges) => FindAll(trackChanges);

        /// <summary>
        /// Belirtilen filtre ve sayfalama parametrelerine göre ürünleri getirir.
        /// </summary>
        /// <param name="p">Kategori, arama terimi, fiyat aralığı ve sayfalama bilgilerini içeren istek parametresi.</param>
        /// <returns>Filtrelenmiş ve sayfalanmış ürün sorgusu.</returns>
        public IQueryable<Product> GetAllProductsWithDetails(ProductRequestParameters p)
        {
            return _context
                .Products
                .FilteredByCategoryId(p.CategoryId)
                .FilteredBySearchTerm(p.SearchTerm)
                .FilteredByPrice(p.MinPrice, p.MaxPrice, p.IsValidPrice)
                .ToPaginate(p.PageNumber, p.PageSize);
        }

        /// <summary>
        /// Belirtilen kimliğe sahip tek bir ürünü getirir.
        /// </summary>
        /// <param name="id">Ürün kimliği</param>
        /// <param name="trackChanges">Değişiklikleri izleme durumu</param>
        /// <returns>Belirtilen kimliğe sahip ürün</returns>
        public Product? GetOneProduct(int id, bool trackChanges)
        {
            return FindByCondition(p => p.ProductId.Equals(id), trackChanges);
        }

        /// <summary>
        /// Tüm vitrin ürünlerini getirir.
        /// </summary>
        /// <param name="trackChanges"></param>
        /// <returns>Tüm vitrin ürünlerinin sorgulanabilir listesi</returns>
        public IQueryable<Product> GetShowcaseProducts(bool trackChanges)
        {
            return FindAll(trackChanges)
                .Where(p => p.ShowCase.Equals(true));
        }

        /// <summary>
        /// Ürünü günceller.
        /// </summary>
        /// <param name="product">Güncellenecek ürün</param>
        public void UpdateOneProduct(Product product) => Update(product);
    }
}