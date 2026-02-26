using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Xml.Linq;

namespace StoreApp.Pages
{
    /// <summary>
    /// Bu sayfa modeli, kullanýcýdan alýnan ismi session içerisinde saklar ve gerektiðinde geri getirir.
    /// </summary>
    public class DemoModel : PageModel
    {
        /// <summary>
        /// Kullanýcýnýn tam adýný session'dan alýr. Eðer session'da "name" anahtarý mevcut deðilse, boþ bir string döner.
        /// </summary>
        public String? FullName => HttpContext?.Session?.GetString("name") ?? "";

        /// <summary>
        /// HTTP GET isteði için çalýþtýrýlýr. Sayfa yüklendiðinde herhangi bir iþlem yapýlmaz.
        /// </summary>
        public void OnGet()
        {
        }

        /// <summary>
        /// Kullanýcýdan alýnan ismi session'a kaydeder.
        /// </summary>
        /// <param name="name">Kullanýcýdan alýnan isim.</param>
        public void OnPost([FromForm] string name)
        {
            // FullName = name; 
            HttpContext.Session.SetString("name", name);
        }
    }
}
