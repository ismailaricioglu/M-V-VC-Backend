using Entities.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StoreApp.Models;

namespace StoreApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        /// Kullanýcýnýn giriþ yapmasý için Login sayfasýný görüntüler.
        /// Giriþ iþlemi tamamlandýktan sonra, kullanýcýnýn yönlendirileceði adres bilgisi (ReturnUrl) ile birlikte LoginModel nesnesi döner.
        /// </summary>
        /// <param name="ReturnUrl">Giriþ baþarýlý olduktan sonra kullanýcýyý yönlendirmek için kullanýlacak URL. Varsayýlan olarak ana sayfaya ("/") yönlendirilir.</param>
        /// <returns>Login sayfasýný ve gerekli yönlendirme bilgisini içeren bir ViewResult nesnesi döner.</returns>
        public IActionResult Login([FromQuery(Name = "ReturnUrl")] string ReturnUrl = "/")
        {
            return View(new LoginModel()
            {
                ReturnUrl = ReturnUrl
            });
        }


        /// <summary>
        /// Kullanýcýnýn giriþ yapmasýný saðlayan POST metodu.
        /// Kullanýcý adý ve þifresi doðruysa oturum baþlatýr ve istenilen sayfaya yönlendirir.
        /// Giriþ baþarýsýz olursa ayný sayfaya geri döner ve hata mesajý gösterir.
        /// </summary>
        /// <param name="model">Kullanýcýdan gelen giriþ bilgilerini (kullanýcý adý, þifre ve geri dönüþ URL'si) içerir.</param>
        /// <returns>Giriþ baþarýlýysa yönlendirme, deðilse giriþ sayfasý görünümü döner.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm] LoginModel model)
        {
            if (ModelState.IsValid) // Model doðrulama baþarýlýysa iþlemlere devam edilir.
            {
                IdentityUser user = await _userManager.FindByNameAsync(model.Name); // Kullanýcý adýyla veritabanýndan kullanýcý aranýr.

                if (user is not null) // Kullanýcý bulunduysa
                {
                    await _signInManager.SignOutAsync(); // Var olan oturum varsa sonlandýrýlýr.

                    if ((await _signInManager.PasswordSignInAsync(user, model.Password, false, false)).Succeeded) // Þifre kontrolü yapýlýr ve baþarýlýysa
                    {
                        return Redirect(model?.ReturnUrl ?? "/"); // Geri dönüþ URL'si varsa oraya yönlendirilir, yoksa ana sayfaya.
                    }
                }

                ModelState.AddModelError("Error", "Invalid username or password."); // Hatalý giriþ bilgileri için genel hata mesajý.
            }

            return View(); // Giriþ baþarýsýzsa ayný sayfa tekrar gösterilir.
        }

        /// <summary>
        /// Kullanýcýnýn oturumunu sonlandýrýr ve belirli bir URL'ye yönlendirir.
        /// </summary>
        /// <param name="ReturnUrl">
        /// Oturum kapatma iþleminden sonra yönlendirilecek adres. 
        /// Eðer belirtilmezse varsayýlan olarak ana sayfa ("/") kullanýlýr.
        /// </param>
        /// <returns>
        /// Belirtilen URL'ye yönlendiren bir <see cref="RedirectResult"/> nesnesi döner.
        /// </returns>
        public async Task<IActionResult> Logout([FromQuery(Name = "ReturnUrl")] string ReturnUrl = "/")
        {
            await _signInManager.SignOutAsync(); // Aktif kullanýcý oturumunu sonlandýrýr.
            return Redirect(ReturnUrl); // Belirtilen sayfaya yönlendirir.
        }

        /// <summary>
        /// Yeni kullanýcý kaydý (Register) sayfasýný görüntüler.
        /// Bu sayfa, kullanýcýlarýn sistemde hesap oluþturabilmesi için form içeren bir arayüz sunar.
        /// </summary>
        /// <returns>Kayýt (Register) sayfasýný temsil eden bir ViewResult döner.</returns>
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Yeni bir kullanýcýyý sistemde kaydeder.
        /// Gönderilen form verilerine göre bir kullanýcý oluþturur, "User" rolüne ekler ve baþarýlýysa giriþ sayfasýna yönlendirir.
        /// </summary>
        /// <param name="model">Kullanýcýdan gelen kayýt verilerini (kullanýcý adý, e-posta, þifre) içeren veri transfer nesnesi.</param>
        /// <returns>Kayýt iþlemi baþarýlýysa Login sayfasýna yönlendirir, aksi halde hata mesajlarý ile birlikte kayýt sayfasýný yeniden gösterir.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([FromForm] RegisterDto model)
        {
            // Yeni kullanýcý nesnesi oluþturulur
            var user = new IdentityUser
            {
                UserName = model.UserName,
                Email = model.Email,
            };

            // Kullanýcý sisteme eklenir
            var result = await _userManager.CreateAsync(user, model.Password);

            // Kayýt iþlemi baþarýlýysa
            if (result.Succeeded)
            {
                // Kullanýcý "User" rolüne eklenir
                var roleResult = await _userManager.AddToRoleAsync(user, "User");

                // Rol atamasý baþarýlýysa giriþ sayfasýna yönlendirilir
                if (roleResult.Succeeded)
                    return RedirectToAction("Login", new { ReturnUrl = "/" });
            }
            else
            {
                // Hatalar modele eklenerek kullanýcýya bildirilir
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
            }

            // Kayýt sayfasý tekrar gösterilir
            return View();
        }

        /// <summary>
        /// Eriþim engellendiðinde yönlendirilen sayfayý döner.
        /// </summary>
        /// <param name="returUrl">Kullanýcýnýn eriþmeye çalýþtýðý, ancak yetkisi olmadýðý URL.</param>
        /// <returns>Kullanýcýnýn yetkisiz eriþim nedeniyle bilgilendirildiði görünüm.</returns>
        public IActionResult AccessDenied([FromQuery(Name = "ReturnUrl")] string returUrl)
        {
            return View();
        }

    }
}