using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repositories;

namespace StoreApp.Infrastructure.Extensions
{
    public static class ApplicationExtension
    {
        /// <summary>
        /// Uygulama ba�lat�ld���nda, veritaban�nda bekleyen g�� (migration) i�lemleri olup olmad���n� kontrol eder
        /// ve varsa bunlar� otomatik olarak uygular.
        /// </summary>
        /// <param name="app">Uygulaman�n IApplicationBuilder �rne�i.</param>
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
        /// Uygulama i�in yerelle�tirme (localization) ayarlar�n� yap�land�r�r.
        /// Varsay�lan k�lt�r� "tr-TR" olarak belirler ve yaln�zca bu k�lt�r� desteklenen dil olarak tan�mlar.
        /// Bu ayar, k�lt�re duyarl� i�eriklerin (tarih, say�, metin vb.) uygun formatta sunulmas�n� sa�lar.
        /// </summary>
        /// <param name="app">Yerelle�tirme ayarlar�n�n uygulanaca�� WebApplication �rne�i.</param>
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
        /// Uygulama ba�lat�ld���nda varsay�lan bir admin kullan�c�s� olu�turur.
        /// E�er "Admin" kullan�c� ad�yla bir kullan�c� yoksa, yeni bir IdentityUser olu�turur,
        /// �ifre belirler ve veritaban�nda mevcut rollerin t�m�ne bu kullan�c�y� dahil eder.
        /// </summary>
        /// <param name="app">IApplicationBuilder: Uygulaman�n yap�land�r�lmas�n� sa�layan nesne.</param>
        /// <exception cref="Exception">E�er kullan�c� olu�turulamaz veya roller atanamazsa �zel bir hata f�rlat�l�r.</exception>
        public static async void ConfigureDefaultAdminUser(this IApplicationBuilder app)
        {
            // Admin kullan�c�s� i�in sabit kullan�c� ad�
            const string adminUser = "Admin";

            // Admin kullan�c�s� i�in sabit �ifre
            const string adminPassword = "Admin+123456";

            // UserManager servisini almak i�in uygulama servislerinden yeni bir scope (ya�am s�resi) olu�turulur
            UserManager<IdentityUser> userManager = app
                .ApplicationServices
                .CreateScope()
                .ServiceProvider
                .GetRequiredService<UserManager<IdentityUser>>();

            // RoleManager servisini almak i�in yine bir scope olu�turulur (CreateAsyncScope kullan�lm��, alternatif olarak CreateScope da kullan�labilir)
            RoleManager<IdentityRole> roleManager = app
                .ApplicationServices
                .CreateAsyncScope()
                .ServiceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

            // Belirtilen kullan�c� ad�yla sistemde bir kullan�c� olup olmad��� kontrol edilir
            IdentityUser user = await userManager.FindByNameAsync(adminUser);

            // E�er b�yle bir kullan�c� yoksa, yeni bir admin kullan�c�s� olu�turulacak
            if (user is null)
            {
                // Admin kullan�c�s�n�n temel bilgileri tan�mlan�r
                user = new IdentityUser()
                {
                    Email = "adminname@xmail.com",           // Admin e-posta adresi
                    PhoneNumber = "5061112233",              // Admin telefon numaras�
                    UserName = adminUser                     // Kullan�c� ad�
                };

                // Belirtilen �ifre ile kullan�c� olu�turulmaya �al���l�r
                var result = await userManager.CreateAsync(user, adminPassword);

                // Kullan�c� olu�turulamazsa hata f�rlat�l�r
                if (!result.Succeeded)
                    throw new Exception("Admin user could not created.");

                // Veritaban�ndaki t�m rolleri getirip admin kullan�c�s�na atama yap�l�r
                var roleResult = await userManager.AddToRolesAsync(user,
                    roleManager
                        .Roles                     // Mevcut roller al�n�r
                        .Select(r => r.Name)       // Roller yaln�zca ad (string) olarak se�ilir
                        .ToList()                  // Liste haline getirilir
                );

                // E�er rol atama i�lemi ba�ar�s�z olursa hata f�rlat�l�r
                if (!roleResult.Succeeded)
                    throw new Exception("System have problems with role defination for admin.");
            }
        }
    }
}