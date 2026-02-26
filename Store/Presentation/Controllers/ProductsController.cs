using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace Presentation.Controllers
{
    /// <summary>
    /// Ürünlerle ilgili API isteklerini yöneten controller sýnýfýdýr.
    /// </summary>
    /// <remarks>
    /// Bu controller, ürünleri listeleme, ekleme, güncelleme ve silme gibi iþlemler için API uç noktalarý sunar.
    /// `IServiceManager` aracýlýðýyla iþ katmanýndaki `ProductService`'e eriþim saðlar.
    /// Bu controller, yalnýzca API kullanýmý için `ApiController` niteliði ile iþaretlenmiþtir ve Razor View içermez.
    /// </remarks>
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IServiceManager _manager;

        public ProductsController(IServiceManager manager)
        {
            _manager = manager;
        }

        /// <summary>
        /// Tüm ürünleri listeler.
        /// </summary>
        /// <remarks>
        /// Varsayýlan olarak `trackChanges` parametresi `false` olarak ayarlanmýþtýr. 
        /// Bu nedenle EF Core, nesneleri izlemeyecektir. Performans açýsýndan daha uygundur.
        /// </remarks>
        /// <returns>200 OK yanýtý ile birlikte ürün listesi döner.</returns>
        [HttpGet]
        public IActionResult GetAllProducts()
        {
            return Ok(_manager.ProductService.GetAllProducts(false));
        }
    }
}
