using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace StoreApp.Areas.Admin.Controllers
{
    /// <summary>
    /// Admin alanýndaki sipariþ yönetim iþlemlerini gerçekleþtiren denetleyici sýnýf.
    /// </summary>
    /// <details>
    /// <c>[Area("Admin")]</c> özniteliði ile "Admin" alanýnda çalýþacak þekilde tanýmlanmýþtýr.
    /// <c>[Authorize(Roles = "Admin")]</c> ile yalnýzca "Admin" rolüne sahip kullanýcýlarýn eriþimine izin verilir.
    /// Sipariþleri listeleme ve tamamlama iþlemleri gibi yönetimsel iþlevleri içerir.
    /// </details>
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        /// <summary>
        /// Servisleri yöneten birim. Sipariþ iþlemleri bu servis üzerinden gerçekleþtirilir.
        /// </summary>
        private readonly IServiceManager _manager;

        /// <summary>
        /// OrderController sýnýfýnýn kurucusudur.
        /// </summary>
        /// <param name="manager">Servis yöneticisi (ServiceManager)</param>
        public OrderController(IServiceManager manager)
        {
            _manager = manager;
        }

        /// <summary>
        /// Tüm sipariþleri listeleyen eylem.
        /// </summary>
        /// <returns>Index görünümü ile sipariþ listesi</returns>
        public IActionResult Index()
        {
            var orders = _manager.OrderService.Orders;
            return View(orders);
        }

        /// <summary>
        /// Bir sipariþi tamamlandý olarak iþaretleyen eylem.
        /// </summary>
        /// <param name="id">Tamamlanacak sipariþin ID'si</param>
        /// <returns>Index sayfasýna yönlendirme</returns>
        [HttpPost]
        public IActionResult Complete([FromForm] int id)
        {
            _manager.OrderService.Complete(id);
            return RedirectToAction("Index");
        }
    }
}
