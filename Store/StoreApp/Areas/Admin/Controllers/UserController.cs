using Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace StoreApp.Areas.Admin.Controllers
{
    /// <summary>
    /// Admin kullanýcý yönetimini saðlayan denetleyici sýnýf.
    /// </summary>
    /// <details>
    /// Bu controller, sadece "Admin" rolüne sahip kullanýcýlar tarafýndan eriþilebilir.
    /// Kullanýcýlarýn listelenmesi, oluþturulmasý, güncellenmesi, silinmesi ve þifre sýfýrlama iþlemlerini yönetir.
    /// "/Admin/User" rotasý altýnda çalýþýr.
    /// </details>
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IServiceManager _manager;

        public UserController(IServiceManager manager)
        {
            _manager = manager;
        }

        /// <summary>
        /// Tüm kullanýcýlarý listeler.
        /// </summary>
        /// <returns>Kullanýcý listesini içeren görünüm.</returns>
        /// <details>
        /// AuthService üzerinden tüm kullanýcýlar alýnýr ve ilgili görünüm ile kullanýcýya sunulur.
        /// </details>
        public IActionResult Index()
        {
            var users = _manager.AuthService.GetAllUsers();
            return View(users);
        }

        /// <summary>
        /// Yeni kullanýcý oluþturma formunu döner.
        /// </summary>
        /// <returns>Boþ bir kullanýcý DTO'su ve roller listesiyle form görünümü.</returns>
        /// <details>
        /// Kullanýcý oluþturma formu için boþ bir `UserDtoForCreation` nesnesi oluþturulur.
        /// Sistem içerisindeki roller çekilerek kullanýcýya seçim için sunulur.
        /// </details>
        public IActionResult Create()
        {
            return View(new UserDtoForCreation()
            {
                Roles = new HashSet<string>(_manager
                    .AuthService
                    .Roles
                    .Select(r => r.Name)
                    .ToList())
            });
        }

        /// <summary>
        /// Yeni kullanýcý oluþturma iþlemini gerçekleþtirir.
        /// </summary>
        /// <param name="userDto">Oluþturulacak kullanýcý verileri.</param>
        /// <returns>Baþarýlýysa listeye yönlendirir, aksi takdirde ayný formu döner.</returns>
        /// <details>
        /// Formdan gelen kullanýcý bilgileri doðrultusunda `CreateUser` metodu çaðrýlýr.
        /// Kullanýcý baþarýyla oluþturulmuþsa Index sayfasýna yönlendirilir.
        /// Aksi halde form sayfasý yeniden yüklenir.
        /// </details>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] UserDtoForCreation userDto)
        {
            var result = await _manager.AuthService.CreateUser(userDto);
            return result.Succeeded
                ? RedirectToAction("Index")
                : View();
        }

        /// <summary>
        /// Belirtilen kullanýcýyý güncelleme formunu döner.
        /// </summary>
        /// <param name="id">Güncellenecek kullanýcýnýn kullanýcý adý.</param>
        /// <returns>Kullanýcý verilerini içeren form görünümü.</returns>
        /// <details>
        /// Veritabanýndan ilgili kullanýcý bilgileri alýnarak güncelleme formuna aktarýlýr.
        /// </details>
        public async Task<IActionResult> Update([FromRoute(Name = "id")] string id)
        {
            var user = await _manager.AuthService.GetOneUserForUpdate(id);
            return View(user);
        }

        /// <summary>
        /// Kullanýcý güncelleme iþlemini gerçekleþtirir.
        /// </summary>
        /// <param name="userDto">Güncellenecek kullanýcý verileri.</param>
        /// <returns>Baþarýlýysa listeye yönlendirir, aksi takdirde formu tekrar döner.</returns>
        /// <details>
        /// Kullanýcý bilgileri güncellenerek kayýt edilir.
        /// Form verileri geçerliyse Index sayfasýna yönlendirilir.
        /// </details>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromForm] UserDtoForUpdate userDto)
        {
            if (ModelState.IsValid)
            {
                await _manager.AuthService.Update(userDto);
                return RedirectToAction("Index");
            }
            return View();
        }

        /// <summary>
        /// Parola sýfýrlama formunu döner.
        /// </summary>
        /// <param name="id">Parolasý sýfýrlanacak kullanýcýnýn kullanýcý adý.</param>
        /// <returns>Boþ parola DTO'su ile form görünümü.</returns>
        /// <details>
        /// Kullanýcýnýn kullanýcý adýna göre boþ bir ResetPasswordDto hazýrlanýr ve görünüm döndürülür.
        /// </details>
        public async Task<IActionResult> ResetPassword([FromRoute(Name = "id")] string id)
        {
            return View(new ResetPasswordDto()
            {
                UserName = id
            });
        }

        /// <summary>
        /// Kullanýcýnýn parolasýný sýfýrlar.
        /// </summary>
        /// <param name="model">Yeni parola bilgilerini içeren DTO.</param>
        /// <returns>Baþarýlýysa listeye yönlendirir, aksi takdirde formu tekrar döner.</returns>
        /// <details>
        /// Girilen yeni parola ile AuthService üzerinden parola sýfýrlama iþlemi gerçekleþtirilir.
        /// </details>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword([FromForm] ResetPasswordDto model)
        {
            var result = await _manager.AuthService.ResetPassword(model);
            return result.Succeeded
                ? RedirectToAction("Index")
                : View();
        }

        /// <summary>
        /// Belirtilen kullanýcýyý siler.
        /// </summary>
        /// <param name="userDto">Silinecek kullanýcýnýn bilgileri.</param>
        /// <returns>Baþarýlýysa listeye yönlendirir, aksi takdirde formu tekrar döner.</returns>
        /// <details>
        /// Kullanýcýnýn kullanýcý adý üzerinden AuthService aracýlýðýyla silme iþlemi gerçekleþtirilir.
        /// </details>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteOneUser([FromForm] UserDto userDto)
        {
            var result = await _manager
                .AuthService
                .DeleteOneUser(userDto.UserName);

            return result.Succeeded
                ? RedirectToAction("Index")
                : View();
        }
    }

}
