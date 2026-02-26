using Entities.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.Contracts;
using Services;
using Services.Contracts;
using StoreApp.Models;

namespace StoreApp.Infrastructure.Extensions
{
    public static class ServiceExtension
    {
        /// <summary>
        /// Veritabaný baðlamýný yapýlandýrýr ve SQLite kullanarak baðlantý dizesini ayarlar.
        /// </summary>
        /// <param name="services">Baðýmlýlýk enjeksiyon hizmet koleksiyonu.</param>
        /// <param name="configuration">Uygulama yapýlandýrmasý (appsettings.json gibi).</param>
        public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            // RepositoryContext (DbContext) için veritabaný yapýlandýrmasýný yapýyoruz
            services.AddDbContext<RepositoryContext>(options =>
            {
                #region SQLite için

                // SQLite baðlantýsýný yapýlandýrýyoruz. connection string'i appsettings.json'dan alýyoruz.
                //options.UseSqlite(configuration.GetConnectionString("sqlconnection"),
                //    b => b.MigrationsAssembly("StoreApp")); // MigrationsAssembly, göç (migration) iþlemleri için kullanýlan assembly'i belirtir.

                #endregion

                #region SqlServer için

                // SqlServer baðlantýsýný yapýlandýrýyoruz. connection string'i appsettings.json'dan alýyoruz.
                options.UseSqlServer(configuration.GetConnectionString("mssqlconnection"),
                    b => b.MigrationsAssembly("StoreApp")); // MigrationsAssembly, göç (migration) iþlemleri için kullanýlan assembly'i belirtir.

                #endregion

                // Hata ayýklama sýrasýnda hassas verilerin günlüðe kaydedilmesini saðlar (gereksizse devre dýþý býrakýlabilir)
                options.EnableSensitiveDataLogging(true);
            });
        }


        /// <summary>
        /// Uygulama için Identity (kimlik doðrulama) yapýlandýrmasýný yapar.
        /// Bu yapýlandýrma, kullanýcýlarýn oturum açma, parola güvenliði ve e-posta onayý gereksinimlerini içerir.
        /// </summary>
        /// <param name="services">ASP.NET Core servis koleksiyonu.</param>
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                // Kullanýcýlarýn e-posta onayýný gerektirmeden giriþ yapmalarýna izin verir.
                options.SignIn.RequireConfirmedAccount = false;

                // Her kullanýcý için benzersiz bir e-posta adresi gerektirir.
                options.User.RequireUniqueEmail = true;

                // Parolada büyük harf bulunmasý zorunlu deðildir.
                options.Password.RequireUppercase = false;

                // Parolada küçük harf bulunmasý zorunlu deðildir.
                options.Password.RequireLowercase = false;

                // Parolada rakam bulunmasý zorunlu deðildir.
                options.Password.RequireDigit = false;

                // Parolanýn en az 6 karakter uzunluðunda olmasý gerektiðini belirtir.
                options.Password.RequiredLength = 6;
            })
            // Kullanýcý ve rol verilerini Entity Framework Core üzerinden RepositoryContext ile saklar.
            .AddEntityFrameworkStores<RepositoryContext>(); // Identity kullanýcý ve rol verilerini EF Core aracýlýðýyla RepositoryContext (DbContext) üzerinden veritabanýnda saklamak için kullanýlýr
        }


        /// <summary>
        /// Uygulamaya session ve bellek içi cache yapýlandýrmasýný ekler.
        /// </summary>
        /// <param name="services">Hizmet koleksiyonu.</param>
        public static void ConfigureSession(this IServiceCollection services)
        {
            // Bellek içi (in-memory) daðýtýlmýþ önbellek eklenir.
            // Bu, session gibi servislerin çalýþmasý için gereklidir.
            services.AddDistributedMemoryCache();

            // Oturum (session) hizmeti eklenir.
            services.AddSession(options =>
            {
                // Session için kullanýlacak cookie adý belirlenir.
                options.Cookie.Name = "StoreApp.Session";

                // Oturumun kullanýcý etkileþimi olmadan ne kadar süre geçerli kalacaðý ayarlanýr.
                options.IdleTimeout = TimeSpan.FromMinutes(10);
            });

            // IHttpContextAccessor, DI konteynerine singleton olarak kaydedilir.
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Her istek için yeni bir Cart nesnesi üretir; session içeriðine göre sepeti getirir.
            services.AddScoped<Cart>(c => SessionCart.GetCart(c));
        }

        /// <summary>
        /// Uygulama içinde kullanýlacak repository sýnýflarýný DI (Dependency Injection) konteynerine kaydeder.
        /// Bu sayede ilgili interface'ler, ihtiyaç duyulan sýnýflara otomatik olarak enjekte edilebilir.
        /// </summary>
        /// <param name="services">Uygulama hizmet koleksiyonu.</param>
        public static void ConfigureRepositoryRegistration(this IServiceCollection services)
        {
            // IRepositoryManager arayüzü için RepositoryManager sýnýfý DI konteynerine kaydedilir.
            // Bu sýnýf, veritabaný iþlemlerini yönetir ve tüm repository'lerin yöneticisi olarak iþlev görür.
            services.AddScoped<IRepositoryManager, RepositoryManager>();

            // IProductRepository arayüzü için ProductRepository sýnýfý DI konteynerine kaydedilir.
            // Bu sýnýf, ürünle ilgili veritabaný iþlemlerini yönetir.
            services.AddScoped<IProductRepository, ProductRepository>();

            // ICategoryRepository arayüzü için CategoryRepository sýnýfý DI konteynerine kaydedilir.
            // Bu sýnýf, kategoriyle ilgili veritabaný iþlemlerini yönetir.
            services.AddScoped<ICategoryRepository, CategoryRepository>();

            // IOrderRepository arayüzü için OrderRepository sýnýfý DI konteynerine kaydedilir.
            // Bu sýnýf, sipariþle ilgili veritabaný iþlemlerini yönetir.
            services.AddScoped<IOrderRepository, OrderRepository>();
        }

        /// <summary>
        /// Uygulama içindeki servis katmaný sýnýflarýný dependency injection (DI) konteynerine kaydeder.
        /// Bu sayede servis arabirimleri controller veya diðer sýnýflarda kolayca kullanýlabilir.
        /// </summary>
        /// <param name="services">Uygulama hizmet koleksiyonu.</param>
        public static void ConfigureServiceRegistration(this IServiceCollection services)
        {
            // IServiceManager arayüzü için ServiceManager sýnýfý DI konteynerine kaydedilir.
            // Bu sýnýf, uygulama içindeki servislerin yönetimi ve koordinasyonundan sorumludur.
            services.AddScoped<IServiceManager, ServiceManager>();

            // IProductService arayüzü için ProductManager sýnýfý DI konteynerine kaydedilir.
            // Bu sýnýf, ürünler ile ilgili iþ mantýðýný yönetir.
            services.AddScoped<IProductService, ProductManager>();

            // ICategoryService arayüzü için CategoryManager sýnýfý DI konteynerine kaydedilir.
            // Bu sýnýf, kategoriler ile ilgili iþ mantýðýný yönetir.
            services.AddScoped<ICategoryService, CategoryManager>();

            // IOrderService arayüzü için OrderManager sýnýfý DI konteynerine kaydedilir.
            // Bu sýnýf, sipariþler ile ilgili iþ mantýðýný yönetir.
            services.AddScoped<IOrderService, OrderManager>();

            // IAuthService arayüzü için AuthManager sýnýfý DI konteynerine kaydedilir.
            // Bu sýnýf, kimlik doðrulama ve kullanýcý yönetimi iþlemlerini yönetir.
            services.AddScoped<IAuthService, AuthManager>();
        }

        /// <summary>
        /// Uygulama için cookie ayarlarýný yapýlandýrýr.
        /// </summary>
        /// <param name="services">ASP.NET Core servis koleksiyonu.</param>
        public static void ConfigureApplicationCookie(this IServiceCollection services)
        {
            services.ConfigureApplicationCookie(options =>
            {
                // Kullanýcý giriþ yapmamýþsa yönlendirileceði giriþ sayfasý.
                options.LoginPath = new PathString("/Account/Login");

                // Kullanýcý giriþ yapmaya çalýþtýðýnda, dönüþ URL'si parametresi için ayar.
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;

                // Kullanýcý oturumunun süresi (10 dakika).
                options.ExpireTimeSpan = TimeSpan.FromMinutes(10);

                // Kullanýcýya eriþim izni verilmediðinde yönlendirileceði sayfa.
                options.AccessDeniedPath = new PathString("/Account/AccessDenied");
            });
        }


        /// <summary>
        /// Uygulamanýn yönlendirme ayarlarýný yapýlandýrýr.
        /// URL'leri küçük harfe çevirir ve sonuna eðik çizgi (slash) eklemez.
        /// </summary>
        /// <param name="services">IServiceCollection örneði.</param>
        public static void ConfigureRouting(this IServiceCollection services)
        {
            // Yönlendirme ayarlarýný yapýlandýrýyoruz
            services.AddRouting(options =>
            {
                // URL'lerin tüm harflerini küçük yapar. Örneðin, "/MyPage" yerine "/mypage" kullanýlacaktýr.
                options.LowercaseUrls = true;

                // URL'lerin sonuna eðik çizgi (slash) eklemeyi devre dýþý býrakýr. 
                // Örneðin, "/mypage/" yerine "/mypage" kullanýlacaktýr.
                options.AppendTrailingSlash = false;
            });
        }

    }
}