using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace StoreApp.Areas.Admin.Controllers
{
    /// <summary>
    /// Rol yönetimi iþlemlerini gerçekleþtiren controller sýnýfýdýr.
    /// Sadece 'Admin' rolüne sahip kullanýcýlar eriþebilir.
    /// </summary>
    /// <details>
    /// Bu controller, sistemdeki kullanýcý rollerinin yönetimi için kullanýlýr.
    /// 'Index' metodu ile tüm roller listelenir.
    /// Geniþletilebilir bir yapýya sahiptir; ileride rol ekleme, silme ve güncelleme gibi iþlemler eklenebilir.
    /// </details>
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly IServiceManager _manager;

        /// <summary>
        /// RoleController sýnýfý için baðýmlýlýklarý enjekte eder.
        /// </summary>
        /// <details>
        /// IServiceManager arayüzü üzerinden AuthService'e eriþilerek
        /// sistemdeki rollerin yönetimi gerçekleþtirilir.
        /// </details>
        /// <param name="manager">Servis yöneticisi arayüzü.</param>
        public RoleController(IServiceManager manager)
        {
            _manager = manager;
        }

        /// <summary>
        /// Tüm rollerin listelendiði sayfayý döner.
        /// </summary>
        /// <details>
        /// AuthService içindeki Roles koleksiyonu kullanýlarak,
        /// sistemde tanýmlý olan tüm roller view'e gönderilir.
        /// Bu iþlem sadece görüntüleme amacý taþýr.
        /// </details>
        /// <returns>Rol listesini içeren görünüm (View).</returns>
        public IActionResult Index()
        {
            return View(_manager.AuthService.Roles);
        }
    }
}
