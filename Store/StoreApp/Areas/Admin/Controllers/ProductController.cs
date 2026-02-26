using Entities.Dtos;
using Entities.Models;
using Entities.RequestParameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.Contracts;
using StoreApp.Models;

namespace StoreApp.Areas.Admin.Controllers
{
    /// <summary>
    /// Admin gösterge paneli alanı için denetleyici.
    /// Bu denetleyici, Admin panosuna gelen istekleri işler ve uygun görünümü döndürür.
    /// </summary>
    /// <details>
    /// Yapısı gereği AtributeRoute ile işaretlenmiştir.
    /// Bu, denetleyicinin belirli bir alan altında çalıştığını belirtir.
    /// Örneğin, bu denetleyici "/Admin/Product" yoluna sahip olacaktır.
    /// </details>
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly IServiceManager _manager;

        public ProductController(IServiceManager manager)
        {
            _manager = manager;
        }

        /// <summary>
        /// Ürünlerin listelendiği ana sayfa görünümünü döner.
        /// </summary>
        /// <param name="p">Ürün filtreleme ve sayfalama parametrelerini içeren istek nesnesi.</param>
        /// <returns>Ürün listesi ve sayfalama bilgileri ile birlikte ViewResult döner.</returns>
        /// <remarks>
        /// Bu metot, `ProductRequestParameters` nesnesi ile gelen filtreleme, sıralama ve sayfalama ayarlarına göre ürünleri listeler.
        /// Ayrıca `Pagination` nesnesi aracılığıyla sayfalama bilgilerini view model'e dahil eder.
        /// </remarks>
        public IActionResult Index([FromQuery] ProductRequestParameters p)
        {
            // Sayfa başlığı olarak "Products" değerini ViewData sözlüğüne ekler.
            // Bu değer genellikle Layout dosyasında <title> etiketi içinde kullanılarak tarayıcı sekmesinde başlık olarak gösterilir.
            ViewData["Title"] = "Products";

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


        /// <summary>
        /// Yeni bir ürün eklemek için formu görüntüler.
        /// </summary>
        /// <returns>Yeni ürün oluşturma formu görünümü</returns>
        public IActionResult Create()
        {
            /// <summary>
            /// Kullanıcıyı bilgilendirmek amacıyla geçici bir mesaj ayarlanır.
            /// </summary>
            /// <remarks>
            /// Bu mesaj, bir sonraki istekte (örneğin bir yönlendirme sonrası) okunabilir.
            /// Genellikle form doğrulama veya kullanıcı bilgilendirme senaryolarında kullanılır.
            /// </remarks>
            TempData["info"] = "Please fill the form.";

            /// <summary>
            /// Ürün oluşturma sayfasını görüntülemek için gerekli olan kategorileri alır.
            /// Bu kategoriler, ürün oluşturma formunda kullanılacak olan bir açılır liste (dropdown) için gereklidir.
            /// </summary>
            /// <details>
            /// Kategoriler, CategoryService aracılığıyla alınır ve ViewBag'e atanır.
            /// Bu sayede, görünümde bu verilere erişim sağlanır.
            /// ViewBag, dinamik bir nesne olduğu için, herhangi bir türde veri saklayabiliriz.
            /// </details>
            ViewBag.Categories = GetCategoriesSelectList();
            return View();
        }

        /// <summary>
        /// Yeni ürün oluşturma işlemini gerçekleştiren asenkron eylem.
        /// </summary>
        /// <param name="productDto">Eklenmek istenen ürün verileri</param>
        /// <param name="file">Ürüne ait resim dosyası</param>
        /// <returns>Başarılıysa ürün listesine yönlendirir, aksi halde aynı form görünümüne geri döner</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] ProductDtoForInsertion productDto, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                // file operation
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", file.FileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                productDto.ImageUrl = String.Concat("/images/", file.FileName);
                _manager.ProductService.CreateOneProduct(productDto);
                /// <summary>
                /// Kullanıcıyı bilgilendirmek amacıyla geçici bir mesaj ayarlanır.
                /// </summary>
                /// <remarks>
                /// Bu mesaj, bir sonraki istekte (örneğin bir yönlendirme sonrası) okunabilir.
                /// Genellikle form doğrulama veya kullanıcı bilgilendirme senaryolarında kullanılır.
                /// </remarks>
                TempData["success"] = $"{productDto.ProductName} has been created.";
                return RedirectToAction("Index");
            }
            return View();
        }

        /// <summary>
        /// Ürün kategorilerini dropdown listesi şeklinde elde eder.
        /// </summary>
        /// <returns>Seçilebilir kategori listesi</returns>
        private SelectList GetCategoriesSelectList()
        {
            return new SelectList(_manager.CategoryService.GetAllCategories(false),
            "CategoryId",
            "CategoryName", "1");
        }

        /// <summary>
        /// Belirli bir ürünün güncelleme formunu getirir.
        /// </summary>
        /// <param name="id">Güncellenecek ürünün kimliği</param>
        /// <returns>Ürün güncelleme formu görünümü</returns>
        public IActionResult Update([FromRoute(Name = "id")] int id)
        {
            ViewBag.Categories = GetCategoriesSelectList();
            var model = _manager.ProductService.GetOneProductForUpdate(id, false);
            // Sayfa başlığını görüntülenecek ürünün adıyla dinamik olarak ayarlar.
            // Eğer model null değilse, ProductName değeri başlık olarak kullanılır.
            ViewData["Title"] = model?.ProductName;
            return View(model);
        }

        /// <summary>
        /// Ürün güncelleme işlemini gerçekleştiren asenkron eylem.
        /// </summary>
        /// <param name="productDto">Güncellenecek ürün bilgileri</param>
        /// <param name="file">Yeni resim dosyası (varsa)</param>
        /// <returns>Başarılıysa ürün listesine yönlendirir, aksi halde form görünümüne geri döner</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromForm] ProductDtoForUpdate productDto, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                // file operation
                string path = Path.Combine(Directory.GetCurrentDirectory(),
                "wwwroot", "images", file.FileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                productDto.ImageUrl = String.Concat("/images/", file.FileName);

                _manager.ProductService.UpdateOneProduct(productDto);
                return RedirectToAction("Index");
            }
            return View();
        }

        /// <summary>
        /// Belirtilen ürünü silen işlem.
        /// </summary>
        /// <param name="id">Silinecek ürünün kimliği</param>
        /// <returns>Ürün listesi sayfasına yönlendirme</returns>
        [HttpGet]
        public IActionResult Delete([FromRoute(Name = "id")] int id)
        {
            _manager.ProductService.DeleteOneProduct(id);
            /// <summary>
            /// Kullanıcıyı bilgilendirmek amacıyla geçici bir mesaj ayarlanır.
            /// </summary>
            /// <remarks>
            /// Bu mesaj, bir sonraki istekte (örneğin bir yönlendirme sonrası) okunabilir.
            /// Genellikle form doğrulama veya kullanıcı bilgilendirme senaryolarında kullanılır.
            /// </remarks>
            TempData["danger"] = "The product has been removed.";
            return RedirectToAction("Index");
        }
    }
}