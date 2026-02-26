using AutoMapper;
using Entities.Dtos;
using Entities.Models;
using Entities.RequestParameters;
using Repositories.Contracts;
using Services.Contracts;

namespace Services
{

    public class ProductManager : IProductService
    {
        private readonly IRepositoryManager _manager;
        private readonly IMapper _mapper;

        public ProductManager(IRepositoryManager manager, IMapper mapper)
        {
            _manager = manager;
            _mapper = mapper;
        }

        /// <summary>
        /// Yeni bir ürün oluşturur ve veritabanına kaydeder.
        /// Alınan DTO nesnesi, ürün nesnesine dönüştürülür ve veritabanına eklenir.
        /// </summary>
        /// <param name="productDto">Oluşturulacak ürünün bilgilerini içeren veri transfer nesnesi (DTO).</param>
        public void CreateOneProduct(ProductDtoForInsertion productDto)
        {
            /// <summary>
            /// Maple işlemi için gerekli olan DTO nesnesini alır.
            /// Bu DTO, ürünün adını, fiyatını ve kategori kimliğini içerir.
            /// </summary>
            /// <details>
            /// DTO nesnesi, Product nesnesine dönüştürülür ve veritabanına eklenir.
            /// Bu işlem, ProductRepository aracılığıyla gerçekleştirilir.
            /// </details>
            // Product product = new Product
            // {
            //     ProductName = productDto.ProductName,
            //     Price = productDto.Price,
            //     CategoryId = productDto.CategoryId
            // };

            /// <summary>
            /// ProductDtoForInsertion nesnesini Product nesnesine dönüştürür.
            /// Bu işlem, AutoMapper kütüphanesi kullanılarak gerçekleştirilir.
            /// </summary>
            /// <details>
            /// Product nesnesi, veritabanına eklenir ve değişiklikler kaydedilir.
            /// Bu işlem, ProductRepository aracılığıyla gerçekleştirilir.
            /// </details>
            Product product = _mapper.Map<Product>(productDto);

            _manager.Product.CreateOneProduct(product);
            _manager.Save();
        }

        /// <summary>
        /// Belirtilen ID'ye sahip ürünü siler.
        /// Ürün var ise, silme işlemi gerçekleştirilir ve değişiklikler kaydedilir.
        /// </summary>
        /// <param name="id">Silinecek ürünün benzersiz ID'si.</param>
        public void DeleteOneProduct(int id)
        {
            Product product = GetOneProduct(id, false);
            if (product is not null)
            {
                _manager.Product.DeleteOneProduct(product);
                _manager.Save();
            }
        }

        /// <summary>
        /// Tüm ürünleri getirir. Değişikliklerin takibi yapılabilir veya yapılmaz.
        /// </summary>
        /// <param name="trackChanges">Değişikliklerin takip edilip edilmeyeceğini belirten bayrak.</param>
        /// <returns>Tüm ürünlerin listesi.</returns>
        public IEnumerable<Product> GetAllProducts(bool trackChanges)
        {
            return _manager.Product.GetAllProducts(trackChanges);
        }

        /// <summary>
        /// Ürünleri, verilen parametrelere göre filtreleyerek detaylı şekilde getirir.
        /// Parametreler, kategori, arama terimi ve fiyat aralığı gibi filtreleme koşullarını içerir.
        /// </summary>
        /// <param name="p">Filtreleme parametrelerini içeren nesne (kategori, arama terimi, fiyat aralığı vb.).</param>
        /// <returns>Belirtilen filtreleme parametrelerine göre detaylı şekilde getirilmiş ürünlerin listesi.</returns>
        public IEnumerable<Product> GetAllProductsWithDetails(ProductRequestParameters p)
        {
            return _manager.Product.GetAllProductsWithDetails(p);
        }

        /// <summary>
        /// En son eklenen ürünlerden belirtilen sayıda ürünü döner.
        /// </summary>
        /// <param name="n">Döndürülecek ürün sayısı.</param>
        /// <param name="trackChanges">Entity Framework'ün değişiklik izleme özelliğinin kullanılıp kullanılmayacağını belirtir.</param>
        /// <returns>Son eklenen ürünlerden oluşan bir liste.</returns>
        public IEnumerable<Product> GetLastestProducts(int n, bool trackChanges)
        {
            return _manager.Product
                .FindAll(false)
                .OrderByDescending(prd => prd.ProductId)
                .Take(n);
        }

        /// <summary>
        /// Verilen ID'ye sahip ürünü getirir. Ürün bulunamazsa bir hata fırlatır.
        /// </summary>
        /// <param name="id">Getirilecek ürünün benzersiz ID'si.</param>
        /// <param name="trackChanges">Değişikliklerin takip edilip edilmeyeceğini belirten bayrak.</param>
        /// <returns>Belirtilen ID'ye sahip ürün, eğer ürün bulunmazsa hata fırlatılır.</returns>
        /// <exception cref="Exception">Ürün bulunamazsa bir hata fırlatılır.</exception>
        public Product? GetOneProduct(int id, bool trackChanges)
        {
            var product = _manager.Product.GetOneProduct(id, trackChanges);
            if (product is null)
                throw new Exception("Product not found.");
            return product;
        }

        /// <summary>
        /// Ürünü güncellemek için gerekli olan DTO nesnesini alır.
        /// Bu DTO, ürünün adını, fiyatını ve kategori kimliğini içerir.
        /// </summary>
        /// <param name="id">Güncellenecek ürünün kimliği.</param>
        /// <param name="trackChanges">Veri değişikliklerini takip edip etmeyeceğini belirtir.</param>
        /// <returns>Güncellenmesi planlanan ürün bilgilerini içeren DTO nesnesi.</returns>
        public ProductDtoForUpdate GetOneProductForUpdate(int id, bool trackChanges)
        {
            var product = GetOneProduct(id, trackChanges);
            var productDto = _mapper.Map<ProductDtoForUpdate>(product);
            return productDto;
        }

        /// <summary>
        /// Vitrin ürünlerini getiren metottur. Ürünlerin, değişiklik takibi yapılmadan veya takibiyle alınmasını sağlar.
        /// </summary>
        /// <param name="trackChanges">Değişikliklerin takip edilip edilmeyeceğini belirten bayrak.</param>
        /// <returns>Vitrin ürünlerinin listesi.</returns>
        public IEnumerable<Product> GetShowcaseProducts(bool trackChanges)
        {
            var products = _manager.Product.GetShowcaseProducts(trackChanges);
            return products;
        }

        /// <summary>
        /// Ürünü günceller.
        /// Bu işlem, ürünün adını, fiyatını ve kategori kimliğini günceller.
        /// </summary>
        /// <param name="productDto">Güncellenmiş ürün bilgilerini içeren DTO nesnesi.</param>
        public void UpdateOneProduct(ProductDtoForUpdate productDto)
        {
            // var entity = _manager.Product.GetOneProduct(productDto.ProductId, true);
            // entity.ProductName = productDto.ProductName;
            // entity.Price = productDto.Price;
            // entity.CategoryId = productDto.CategoryId;

            /// <summary>
            /// Maple işlemi için gerekli olan DTO nesnesini alır.
            /// Bu DTO, ürünün adını, fiyatını ve kategori kimliğini içerir.
            /// </summary>
            /// <details>
            /// DTO nesnesi, Product nesnesine dönüştürülür ve veritabanına güncellenir.
            /// Bu işlem, ProductRepository aracılığıyla gerçekleştirilir.
            /// </details>
            var entity = _mapper.Map<Product>(productDto);
            _manager.Product.UpdateOneProduct(entity);
            _manager.Save();
        }
    }
}