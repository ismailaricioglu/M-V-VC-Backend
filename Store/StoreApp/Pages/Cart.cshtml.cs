using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Contracts;
using StoreApp.Infrastructure.Extensions;

namespace StoreApp.Pages
{
    /// <summary>
    /// Sepet iþlemleriyle ilgili sayfa modeli. Bu sýnýf, kullanýcýnýn sepetine ürün ekleme, çýkarma ve sepeti görüntüleme iþlemlerini yönetir.
    /// </summary>
    public class CartModel : PageModel
    {
        /// <summary>
        /// Uygulama servislerini yöneten servis yöneticisi.
        /// </summary>
        private readonly IServiceManager _manager;

        /// <summary>
        /// Kullanýcýnýn sepetini temsil eden Cart nesnesi. 
        /// IoC (Inversion of Control) ile enjekte edilir.
        /// </summary>
        public Cart Cart { get; set; } // IoC

        /// <summary>
        /// Sepete ürün eklendikten sonra yönlendirilecek URL. Varsayýlan olarak anasayfaya yönlendirilir.
        /// </summary>
        public string ReturnUrl { get; set; } = "/";

        /// <summary>
        /// CartModel sýnýfýnýn constructor'ý. Sepet servisi ve hizmet yöneticisini alarak gerekli baðýmlýlýklarý enjekte eder.
        /// </summary>
        /// <param name="manager">Servis yöneticisi (IServiceManager), uygulamanýn servislerini yönetir.</param>
        /// <param name="cartService">Sepet servisi (Cart), sepet iþlemlerini yönetir ve uygulamaya sepet verisini saðlar.</param>
        public CartModel(IServiceManager manager, Cart cartService)
        {
            _manager = manager;
            // tekrar edilen yapýlarý tek yerden yönetmek için;
            // SessionCart : Cart ile uygulandý
            // IoC ile çaðrýlan Cart nesnesi kalýtým alan SessionCart üzerinden yapýldý
            Cart = cartService;
        }

        /// <summary>
        /// Kullanýcýyý sepet sayfasýna yönlendirir ve geri dönülecek URL'yi belirler.
        /// </summary>
        /// <param name="returnUrl">Kullanýcýnýn iþlem sonrasý döneceði URL. Eðer null ise, varsayýlan olarak anasayfa ("/") kullanýlýr.</param>
        public void OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl ?? "/"; // Eðer returnUrl null ise, varsayýlan olarak anasayfaya yönlendirilir.
                                          // Tekrar edilen yapý
                                          // Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
        }

        /// <summary>
        /// Sepete bir ürün ekler ve kullanýcýyý belirli bir URL'ye yönlendirir.
        /// </summary>
        /// <param name="productId">Sepete eklenmek istenen ürünün ID'si.</param>
        /// <param name="returnUrl">Yönlendirilecek URL.</param>
        /// <returns>Yönlendirme iþlemi yapýlýr.</returns>
        public IActionResult OnPost(int productId, string returnUrl)
        {
            Product? product = _manager
                .ProductService
                .GetOneProduct(productId, false);

            if (product is not null)
            {
                // Tekrar edilen yapý
                // Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
                Cart.AddItem(product, 1);
                // Tekrar edilen yapý
                // HttpContext.Session.SetJson<Cart>("cart", Cart);
            }
            // return Page(); // returnUrl

            /// <summary>
            /// Kullanýcýyý, belirlenen sayfaya `returnUrl` parametresiyle birlikte yönlendirir.
            /// Bu yöntem genellikle giriþ, sepet veya iþlem sonrasý eski sayfaya dönüþlerde kullanýlýr.
            /// </summary>
            return RedirectToPage(new { returnUrl = returnUrl }); // returnUrl
        }

        /// <summary>
        /// Sepetten bir ürün çýkarýr ve sayfayý tekrar render eder.
        /// </summary>
        /// <param name="id">Çýkarýlacak ürünün ID'si.</param>
        /// <param name="returnUrl">Yönlendirilecek URL.</param>
        /// <returns>Sayfa yeniden render edilir.</returns>
        public IActionResult OnPostRemove(int id, string returnUrl)
        {
            // Tekrar edilen yapý
            // Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
            Cart.RemoveLine(Cart.Lines.First(cl => cl.Product.ProductId.Equals(id)).Product);
            // Tekrar edilen yapý
            // HttpContext.Session.SetJson<Cart>("cart", Cart);
            return Page();
        }
    }
}