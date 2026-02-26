using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repositories;

namespace StoreApp.Infrastructure.Extensions
{
    public static class ApplicationExtension
    {
        /// <summary>
        /// Uygulama baþlatýldýðýnda, veritabanýnda bekleyen göç (migration) iþlemleri olup olmadýðýný kontrol eder
        /// ve varsa bunlarý otomatik olarak uygular.
        /// </summary>
        /// <param name="app">Uygulamanýn IApplicationBuilder örneði.</param>
        public static void ConfigureAndCheckMigration(this IApplicationBuilder app)
        {
            RepositoryContext context = app
                .ApplicationServices
                .CreateScope()
                .ServiceProvider
                .GetRequiredService<RepositoryContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
        }


        /// <summary>
        /// Uygulama için yerelleþtirme (localization) ayarlarýný yapýlandýrýr.
        /// Varsayýlan kültürü "tr-TR" olarak belirler ve yalnýzca bu kültürü desteklenen dil olarak tanýmlar.
        /// Bu ayar, kültüre duyarlý içeriklerin (tarih, sayý, metin vb.) uygun formatta sunulmasýný saðlar.
        /// </summary>
        /// <param name="app">Yerelleþtirme ayarlarýnýn uygulanacaðý WebApplication örneði.</param>
        public static void ConfigureLocalization(this WebApplication app)
        {
            app.UseRequestLocalization(options =>
            {
                options.AddSupportedCultures("tr-TR")
                       .AddSupportedUICultures("tr-TR")
                       .SetDefaultCulture("tr-TR");
            });
        }

        /// <summary>
        /// Uygulama baþlatýldýðýnda varsayýlan bir admin kullanýcýsý oluþturur.
        /// Eðer "Admin" kullanýcý adýyla bir kullanýcý yoksa, yeni bir IdentityUser oluþturur,
        /// þifre belirler ve veritabanýnda mevcut rollerin tümüne bu kullanýcýyý dahil eder.
        /// </summary>
        /// <param name="app">IApplicationBuilder: Uygulamanýn yapýlandýrýlmasýný saðlayan nesne.</param>
        /// <exception cref="Exception">Eðer kullanýcý oluþturulamaz veya roller atanamazsa özel bir hata fýrlatýlýr.</exception>
        public static async void ConfigureDefaultAdminUser(this IApplicationBuilder app)
        {
            // Admin kullanýcýsý için sabit kullanýcý adý
            const string adminUser = "Admin";

            // Admin kullanýcýsý için sabit þifre
            const string adminPassword = "Admin+123456";

            // UserManager servisini almak için uygulama servislerinden yeni bir scope (yaþam süresi) oluþturulur
            UserManager<IdentityUser> userManager = app
                .ApplicationServices
                .CreateScope()
                .ServiceProvider
                .GetRequiredService<UserManager<IdentityUser>>();

            // RoleManager servisini almak için yine bir scope oluþturulur (CreateAsyncScope kullanýlmýþ, alternatif olarak CreateScope da kullanýlabilir)
            RoleManager<IdentityRole> roleManager = app
                .ApplicationServices
                .CreateAsyncScope()
                .ServiceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

            // Belirtilen kullanýcý adýyla sistemde bir kullanýcý olup olmadýðý kontrol edilir
            IdentityUser user = await userManager.FindByNameAsync(adminUser);

            // Eðer böyle bir kullanýcý yoksa, yeni bir admin kullanýcýsý oluþturulacak
            if (user is null)
            {
                // Admin kullanýcýsýnýn temel bilgileri tanýmlanýr
                user = new IdentityUser()
                {
                    Email = "zcomert@samsun.edu.tr",         // Admin e-posta adresi
                    PhoneNumber = "5061112233",              // Admin telefon numarasý
                    UserName = adminUser                     // Kullanýcý adý
                };

                // Belirtilen þifre ile kullanýcý oluþturulmaya çalýþýlýr
                var result = await userManager.CreateAsync(user, adminPassword);

                // Kullanýcý oluþturulamazsa hata fýrlatýlýr
                if (!result.Succeeded)
                    throw new Exception("Admin user could not created.");

                // Veritabanýndaki tüm rolleri getirip admin kullanýcýsýna atama yapýlýr
                var roleResult = await userManager.AddToRolesAsync(user,
                    roleManager
                        .Roles                     // Mevcut roller alýnýr
                        .Select(r => r.Name)       // Roller yalnýzca ad (string) olarak seçilir
                        .ToList()                  // Liste haline getirilir
                );

                // Eðer rol atama iþlemi baþarýsýz olursa hata fýrlatýlýr
                if (!roleResult.Succeeded)
                    throw new Exception("System have problems with role defination for admin.");
            }
        }
    }
}