using Microsoft.AspNetCore.Mvc;
using StoreApp.Infrastructure.Extensions;

// ---------------------- MIDDLEWARE AÇIKLAMASI ----------------------
/*
 * Middleware, ASP.NET Core uygulamalarında HTTP isteklerinin geçtiği işlem katmanlarıdır.
 * Her middleware bileşeni, isteği işler, bir sonraki bileşene aktarır veya işlemi sonlandırabilir.
 * Middleware'ler genellikle aşağıdaki amaçlarla kullanılır:
 * 
 * - İsteklerin günlüğe kaydedilmesi (Logging)
 * - Güvenlik kontrolleri (Authentication, Authorization)
 * - Hata işleme (Exception Handling)
 * - Statik dosya sunumu
 * - Oturum yönetimi (Session)
 * - Yönlendirme (Routing)
 * 
 * Middleware sırası önemlidir. Örneğin:
 * - `UseRouting()` çağrılmadan önce yönlendirme yapılamaz.
 * - `UseAuthentication()` mutlaka `UseAuthorization()`'dan önce gelmelidir.
 * - Statik dosyalar önce sunulmalıdır, aksi takdirde gereksiz işlemler yapılır.
 * 
 * ASP.NET Core pipeline şu şekilde işler:
 * -> Middleware1
 *    -> Middleware2
 *       -> Middleware3
 *          ...
 *       <- Middleware3
 *    <- Middleware2
 * <- Middleware1
 *
 * Bu yapı sayesinde uygulama modüler, kontrol edilebilir ve genişletilebilir olur.
 */


/// <summary>
/// WebApplicationBuilder sınıfının önceden yapılandırılmış varsayılanlarla yeni bir örneğini oluşturur.
/// </summary>
/// <detail>
/// Bu, uygulamanızın yapılandırmasını ve bağımlılık enjeksiyonunu yönetmek için kullanılır.
/// </detail>
var builder = WebApplication.CreateBuilder(args);

/// <summary>
/// MVC mimarisi ile çalışan controller sınıflarını servis koleksiyonuna ekler.
/// </summary>
/// <remarks>
/// Uygulama içinde API veya klasik MVC controller'larının kullanılabilmesi için gerekli servisleri yapılandırır.
/// `AddControllers()` metodu yalnızca controller sınıflarını etkinleştirir, Razor Pages veya View desteği sağlamaz.
/// Eğer Razor view kullanımı gerekiyorsa `AddControllersWithViews()` ya da `AddMvc()` tercih edilmelidir.
/// 
/// `AddApplicationPart(...)` ifadesi, başka bir assembly'de tanımlanmış controller sınıflarının da projeye dahil edilmesini sağlar.
/// Bu örnekte, `Presentation` katmanındaki controller'ların tanınabilmesi için ilgili assembly referansı eklenmiştir.
/// </remarks>
builder.Services.AddControllers()
    .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);

/// <summary>
/// MVC mimarisini kullanarak Controller ve View'ları desteklemek için gerekli hizmetleri ekler.
/// </summary>
/// <detail>
/// Bu, uygulamanızın MVC (Model-View-Controller) mimarisini kullanarak çalışmasını sağlar.
/// Controller'lar ve View'lar arasındaki etkileşimi yönetir.
/// </detail>
builder.Services.AddControllersWithViews();

/// <summary>
/// Razor Pages hizmetlerini uygulamaya ekler.
/// </summary>
/// <detail>
/// ASP.NET Core uygulamasında Razor Pages desteğini etkinleştirir.
/// Bu satır, Razor Pages için gerekli olan yönlendirme, sayfa model bağlamaları, filtreler ve diğer altyapı bileşenlerini DI (Dependency Injection) sistemine dahil eder.
/// Genellikle programın başlatılma aşamasında (Program.cs içinde) çağrılır ve 
/// `app.MapRazorPages()` ile birlikte kullanılır.
/// 
/// Örnek kullanım:
/// builder.Services.AddRazorPages();  // Razor Pages hizmetlerini ekle
/// app.MapRazorPages();               // Razor Pages yönlendirmesini etkinleştir
/// 
/// Bu yapı sayesinde .cshtml sayfaları doğrudan çalıştırılabilir ve kendi model sınıflarıyla bağlanabilir.
/// </detail>
builder.Services.AddRazorPages();

builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.ConfigureIdentity();
builder.Services.ConfigureSession();
builder.Services.ConfigureRepositoryRegistration();
builder.Services.ConfigureServiceRegistration();
builder.Services.ConfigureRouting();
builder.Services.ConfigureApplicationCookie();

/// <summary>
/// AutoMapper'ı yapılandırır.
/// </summary>
/// <detail>    
/// Bu, nesneler arasında veri aktarımını kolaylaştırmak için AutoMapper kütüphanesini kullanır.
/// AutoMapper, nesneler arasındaki dönüşümleri otomatikleştirir ve kodunuzu daha temiz ve okunabilir hale getirir.
/// </detail>
builder.Services.AddAutoMapper(typeof(Program));

/// <summary>
/// WebApplication'ı oluşturur.
/// </summary>
/// <detail>
/// Bu, yapılandırılmış hizmetleri ve ara yazılımları kullanarak uygulamanın çalıştırılabilir bir örneğini oluşturur.
/// </detail>
var app = builder.Build();

/// <summary>
/// Statik dosya ara yazılımını ekler.
/// </summary>
/// <detail>
/// Bu ara yazılım, uygulamanızın wwwroot klasöründeki statik dosyalara (CSS, JavaScript, resimler vb.) erişimini sağlar.
/// </detail>
app.UseStaticFiles();

/// <summary>
/// ASP.NET Core middleware pipeline’ına oturum (session) desteğini ekler.
/// Bu middleware, gelen her istekte oturum verilerini okuyup yazar.
/// `AddSession()` ve `AddDistributedMemoryCache()` ile birlikte kullanılmalıdır.
/// </summary>
app.UseSession();

/// <summary>
/// HTTP isteklerini HTTPS'ye yönlendirir.
/// </summary>
/// <detail>
/// Bu ara yazılım, gelen tüm HTTP isteklerini HTTPS'ye yönlendirir. Bu, uygulamanızın güvenliğini artırmak için önemlidir.
/// </detail>
app.UseHttpsRedirection();

/// <summary>
/// Yönlendirme ara yazılımını ekler.
/// </summary>
/// <detail>
/// Bu ara yazılım, gelen isteklerin yönlendirilmesini sağlar. 
/// Uygulamanızın belirli URL desenlerine göre istekleri uygun işlemcilere yönlendirmesine olanak tanır.
/// </detail>
app.UseRouting();

#region ilgili middleware'lerin Routing ile Endpoints arasında tanımlanması gerekmekte

// Uygulamada kimlik doğrulama sistemini devreye alır.
// Bu middleware, gelen isteklerde kimlik bilgilerini (örneğin cookie veya token) kontrol ederek
// kullanıcının kimliğini belirlemeye çalışır. Login olan kullanıcıyı tanımak için gereklidir.
app.UseAuthentication();

// Uygulamada yetkilendirme sistemini devreye alır.
// Kimliği doğrulanmış kullanıcının belirli işlemleri yapma yetkisine sahip olup olmadığını kontrol eder.
// [Authorize] attribute'lerinin çalışabilmesi için mutlaka çağrılmalıdır.
app.UseAuthorization();

#endregion

/// <summary>
/// 1: Admin alanı için özel bir rota yapılandırmasını ekler.
/// 2: Varsayılan rota yapılandırmasını ekler.
/// </summary>
/// <detail>
/// 1: Bu yapılandırma, "Admin" alanındaki controller'ların ve action'ların URL desenini belirler.
/// 1: "Admin" alanındaki controller'lar için varsayılan olarak "Dashboard" controller'ını ve "Index" action'ını kullanır.
/// 2: Bu yapılandırma, gelen isteklerin varsayılan olarak "Home" controller'ına ve "Index" action'ına yönlendirilmesini sağlar.
/// 2: Ayrıca, isteğe bağlı bir "Id" parametresi de kabul eder.
/// </detail>
app.UseEndpoints(endpoints =>
{
    endpoints.MapAreaControllerRoute(
        name: "Admin",
        areaName: "Admin",
        pattern: "Admin/{controller=Dashboard}/{action=Index}/{id?}"
    );

    endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

    endpoints.MapRazorPages();

    endpoints.MapControllers();
});

// Uygulama başlatılırken veritabanı migration işlemleri tetiklenir.
app.ConfigureAndCheckMigration();

// Uygulama düzeyinde kültürel yerelleştirme (localization) ayarlarını uygular.
app.ConfigureLocalization();

// Uygulama başlatıldığında, sistemde "Admin" isimli bir kullanıcı yoksa oluşturur.
// Bu kullanıcıya sistemde tanımlı tüm roller atanır (Admin, Editor, User).
// Not: Bu işlem yalnızca uygulama ilk kez başlatıldığında çalışır ve her seferinde yeniden kullanıcı oluşturmaz.
// Kullanıcı zaten varsa işlem atlanır.
app.ConfigureDefaultAdminUser();

/// <summary>
/// Uygulamayı çalıştırır.
/// </summary>
app.Run();