using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;

namespace Repositories
{
    /// <summary>
    /// RepositoryManager sınıfı, IRepositoryManager arayüzünü uygular ve veritabanı işlemlerini yönetir.
    /// </summary>
    public class RepositoryManager : IRepositoryManager
    {
        /// <summary>
        /// Veritabanı bağlamı (context), tüm repository'lerin çalıştığı EF Core bağlantısıdır.
        /// </summary>
        private readonly RepositoryContext _context;

        /// <summary>
        /// Ürünler ile ilgili işlemleri gerçekleştiren repository örneğidir.
        /// </summary>
        private readonly IProductRepository _productRepository;

        /// <summary>
        /// Kategoriler ile ilgili işlemleri gerçekleştiren repository örneğidir.
        /// </summary>
        private readonly ICategoryRepository _categoryRepository;

        /// <summary>
        /// Siparişler ile ilgili işlemleri gerçekleştiren repository örneğidir.
        /// </summary>
        private readonly IOrderRepository _orderRepository;

        /// <summary>
        /// RepositoryManager sınıfının kurucusu.
        /// </summary>
        /// <param name="productRepository">Ürün deposu.</param>
        /// <param name="context">Veritabanı bağlamı.</param>
        /// <param name="categoryRepository">Kategori deposu.</param>
        /// <param name="orderRepository">Sipariş deposu.</param>
        public RepositoryManager(
            IProductRepository productRepository,
            RepositoryContext context,
            ICategoryRepository categoryRepository,
            IOrderRepository orderRepository)
        {
            _context = context;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _orderRepository = orderRepository;

        }

        /// <summary>
        /// Ürün deposunu döner.
        /// </summary>
        public IProductRepository Product => _productRepository;

        /// <summary>
        /// Kategori deposunu döner.
        /// </summary>
        public ICategoryRepository Category => _categoryRepository;

        /// <summary>
        /// Sipariş deposunu döner.
        /// </summary>
        public IOrderRepository Order => _orderRepository;

        /// <summary>
        /// Yapılan değişiklikleri veritabanına kaydeder.
        /// </summary>
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
