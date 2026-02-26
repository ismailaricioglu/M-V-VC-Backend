using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace StoreApp.Areas.Admin.Controllers
{
    /// <summary>
    /// Admin gösterge paneli alanı için denetleyici.
    /// Bu denetleyici, Admin panosuna gelen istekleri işler ve uygun görünümü döndürür.
    /// </summary>
    /// <details>
    /// Yapısı gereği <c>[Area("Admin")]</c> özniteliği ile işaretlenmiştir.
    /// Bu, denetleyicinin belirli bir alan altında çalıştığını belirtir.
    /// Örneğin, bu denetleyici "/Admin/Category" yoluna sahip olacaktır.
    /// Ayrıca sadece "Admin" rolüne sahip kullanıcıların erişimine izin verilir.
    /// </details>
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly IServiceManager _manager;

        public CategoryController(IServiceManager manager)
        {
            _manager = manager;
        }

        public IActionResult Index()
        {
            return View(_manager.CategoryService.GetAllCategories(false));
        }
    }

}