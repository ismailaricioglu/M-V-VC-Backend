using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace StoreApp.Areas.Admin.Controllers
{
    /// <summary>
    /// Admin gösterge paneli alanı için denetleyici.
    /// Bu denetleyici, Admin panosuna gelen istekleri işler ve uygun görünümü döndürür.
    /// </summary>
    /// <details>
    /// Yapısı gereği <c>[Area("Admin")]</c> ve <c>[Authorize(Roles = "Admin")]</c> öznitelikleri ile işaretlenmiştir.
    /// Bu, denetleyicinin yalnızca "Admin" rolüne sahip kullanıcılar tarafından erişilebileceğini ve
    /// "/Admin/Dashboard" yolunda çalıştığını belirtir.
    /// </details>
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            /// <summary>
            /// Kullanıcıyı bilgilendirmek amacıyla geçici bir mesaj ayarlanır.
            /// </summary>
            /// <remarks>
            /// Bu mesaj, bir sonraki istekte (örneğin bir yönlendirme sonrası) okunabilir.
            /// Genellikle form doğrulama veya kullanıcı bilgilendirme senaryolarında kullanılır.
            /// </remarks>
            TempData["info"] = $"Welcome back, {DateTime.Now.ToShortTimeString()}";
            return View();
        }
    }


}