using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace StoreApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly IServiceManager _manager;
        private readonly Cart _cart;

        /// <summary>
        /// Sipariþ iþlemleri için gerekli servisleri ve alýþveriþ sepetini enjekte eder.
        /// </summary>
        /// <param name="manager">Servis yönetim arayüzü.</param>
        /// <param name="cart">Mevcut kullanýcýya ait alýþveriþ sepeti.</param>
        public OrderController(IServiceManager manager, Cart cart)
        {
            _manager = manager;
            _cart = cart;
        }

        /// <summary>
        /// Sipariþ oluþturma (checkout) ekranýný getirir.
        /// </summary>
        /// <details>
        /// Bu metot, yalnýzca oturum açmýþ kullanýcýlar tarafýndan eriþilebilir.
        /// Yeni bir sipariþ oluþturmak üzere boþ bir `Order` nesnesi ile birlikte görüntü döndürülür.
        /// Kullanýcýlar, bu ekranda teslimat ve ödeme bilgilerini girerek sipariþi tamamlar.
        /// </details>
        /// <returns>Boþ bir Order modeli ile ViewResult döner.</returns>
        [Authorize]
        public ViewResult Checkout() => View(new Order());


        /// <summary>
        /// Sipariþ gönderimi sonrasý iþleme alýnmasýný saðlar.
        /// Sepet boþsa hata döner, aksi halde sipariþi kaydeder.
        /// </summary>
        /// <param name="order">Formdan gelen sipariþ bilgisi.</param>
        /// <returns>
        /// Sipariþ geçerliyse "/Complete" sayfasýna yönlendirme yapýlýr.
        /// Hatalýysa mevcut checkout görünümü tekrar gösterilir.
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Checkout([FromForm] Order order)
        {
            if (_cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry, your cart is empty.");
            }

            if (ModelState.IsValid)
            {
                order.Lines = _cart.Lines.ToArray();
                _manager.OrderService.SaveOrder(order);
                _cart.Clear();
                return RedirectToPage("/Complete", new { OrderId = order.OrderId });
            }
            else
            {
                return View();
            }
        }
    }
}