using Entities.Models;
using Entities.RequestParameters;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using Repositories.Contracts;
using Services.Contracts;
using StoreApp.Models;

namespace StoreApp.Controllers
{
    public class ProductController : Controller
    {
        #region Tip 5

        /// <summary>
        /// Servis yöneticisi.
        /// </summary>
        /// <detail>
        /// Uygulama servislerini yönetmek için kullanılan servis yöneticisi.
        /// </detail>
        private readonly IServiceManager _manager;

        /// <summary>
        /// Servis yöneticisini başlatır.
        /// </summary>
        /// <detail>
        /// Dependency Injection ile sağlanan servis yöneticisini kullanır.
        /// </detail>
        public ProductController(IServiceManager manager)
        {
            _manager = manager;
        }

        #endregion

        #region Tip 4

        // /// <summary>
        // /// Repository yöneticisi.
        // /// </summary>
        // /// <detail>
        // /// Veritabanı işlemleri için kullanılan repository yöneticisi.
        // /// </detail>
        // private readonly IRepositoryManager _manager;

        // /// <summary>
        // /// Repository yöneticisini başlatır.
        // /// </summary>
        // /// <detail>
        // /// Dependency Injection ile sağlanan repository yöneticisini kullanır.
        // /// </detail>
        // public ProductController(IRepositoryManager manager)
        // {
        //     _manager = manager;
        // }

        #endregion

        #region Tip 3

        /// <summary>
        /// Veritabanı bağlamı. Dependency Injection
        /// </summary>
        /// <detail>
        /// Bu bağlam, veritabanı işlemleri için kullanılır.
        /// </detail>
        // private RepositoryContext _context;

        /// <summary>
        /// <summary>
        /// ProductController yapıcı metodu.
        /// </summary>
        /// <detail>
        /// Belirtilen RepositoryContext ile yeni bir ProductController örneği başlatır.
        /// </detail>
        // public ProductController(RepositoryContext context)
        // {
        //     _context = context;
        // }

        #endregion

        /// <summary>
        /// Ürünlerin listesini döner.
        /// </summary>
        /// <detail>
        /// Bu metod,
        ///     Tip 3: Dependency Injection ile sağlanan veritabanı bağlamını kullanarak ürün listesini döner.
        ///     Tip 2: yeni bir RepositoryContext örneği oluşturur ve veritabanı bağlantı dizesini kullanarak ürün listesini döner.
        ///     Tip 1: sabitlenmiş bir ürün listesi döner. Veritabanı bağlantısı gerektirmez.
        /// </detail>
        public IEnumerable<Product> IndexOutOfPresentation()
        {
            #region Tip 3

            ///<summary>
            /// Veritabanından ürün listesi döner.
            ///</summary>
            /// <detail>
            /// Bu kod, Dependency Injection ile sağlanan veritabanı bağlamını kullanarak ürün listesini döner.
            /// </detail>
            // return _context.Products;

            #endregion

            #region Tip 2

            ///<summary>
            /// Veritabanından ürün listesi döner.
            ///</summary>
            /// <detail>
            /// Bu kod, yeni bir RepositoryContext örneği oluşturur ve veritabanı bağlantı dizesini kullanarak ürün listesini döner.
            /// </detail>
            // var context = new RepositoryContext(
            //     new DbContextOptionsBuilder<RepositoryContext>()
            //     .UseSqlite("Data Source = E:\\ASP.Net MVC\\DERSLER\\MVC\\ProductDb.db")
            //     .Options
            // );

            // return context.Products;

            #endregion

            #region Tip 1

            ///<summary>
            /// Sabitlenmiş bir ürün listesi döner.
            ///</summary>
            /// <detail>
            /// Bu kod, sabitlenmiş bir ürün listesi döner. Veritabanı bağlantısı gerektirmez.
            /// </detail>
            // return new List<Product>(){
            //     new Product (){ProductId=1, ProductName="Computer",  Price=5}
            // };

            #endregion

            throw new NotImplementedException();
        }

        #region Filtresiz Index

        /////<summary>
        ///// Ürünlerin listesini döner.
        /////</summary>
        ///// <detail>
        ///// Veritabanından ürün listesini alır ve View'a gönderir.
        ///// </detail>
        //public IActionResult Index()
        //{

        //    /// <summary>
        //    /// Ürünlerin listesini döner.
        //    /// </summary>
        //    /// <detail>
        //    /// Veritabanından ürün listesini alır ve View'a gönderir.
        //    /// </detail>
        //    // var model = _context.Products.ToList();

        //    #region Tip 4 uygulması

        //    // /// <summary>
        //    // /// Tüm ürünlerin listesini döner.
        //    // /// </summary>
        //    // /// <detail>
        //    // /// Repository yöneticisini kullanarak veritabanından tüm ürünleri alır.
        //    // /// </detail>
        //    // var model = _manager.Product.GetAllProducts(false);

        //    #endregion

        //    #region Tip 5 uygulması

        //    /// <summary>
        //    /// Tüm ürünlerin listesini döner.
        //    /// </summary>
        //    /// <detail>
        //    /// Servis yöneticisini kullanarak veritabanından tüm ürünleri alır.
        //    /// </detail>
        //    var model = _manager.ProductService.GetAllProducts(false);

        //    #endregion

        //    return View(model);
        //}

        #endregion

        #region Filtreli Index

        ///<summary>
        /// Parametreye bağlı Ürünlerin listesini döner.
        ///</summary>
        /// <detail>
        /// Veritabanından parametredeki koşullara uygun ürün listesini alır ve View'a gönderir.
        /// </detail>
        public IActionResult Index(ProductRequestParameters p)
        {

            var products = _manager.ProductService.GetAllProductsWithDetails(p);
            var pagination = new Pagination()
            {
                CurrentPage = p.PageNumber,
                ItemsPerPage = p.PageSize,
                TotalItems = _manager.ProductService.GetAllProducts(false).Count()
            };


            return View(new ProductListViewModel()
            {
                Products = products,
                Pagination = pagination
            });
        }

        #endregion

        public IActionResult Get([FromRoute(Name = "id")] int id)
        {
            // Product product = _context.Products.First(p => p.ProductId.Equals(id));
            // return View(product);

            #region Tip 4 uygulması

            // /// <summary>
            // /// Belirtilen ID'ye sahip bir ürünü döner.
            // /// </summary>
            // /// <detail>
            // /// Repository yöneticisini kullanarak veritabanından belirtilen ID'ye sahip ürünü alır.
            // /// </detail>
            // var model = _manager.Product.GetOneProduct(id, false);

            #endregion

            #region Tip 5 uygulması

            /// <summary>
            /// Belirtilen ID'ye sahip bir ürünü döner.
            /// </summary>
            /// <detail>
            /// Servis yöneticisini kullanarak veritabanından belirtilen ID'ye sahip ürünü alır.
            /// </detail>
            var model = _manager.ProductService.GetOneProduct(id, false);

            #endregion

            ViewData["Title"] = model?.ProductName;

            return View(model);

        }
    }
}