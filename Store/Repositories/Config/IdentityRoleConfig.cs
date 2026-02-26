using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Config
{
    public class IdentityRoleConfig : IEntityTypeConfiguration<IdentityRole>
    {
        /// <summary>
        /// Uygulama baþlatýldýðýnda Identity rol verilerini ön tanýmlý olarak veritabanýna ekler.
        /// Bu, yetkilendirme iþlemlerinde kullanýlacak rollerin (User, Editor, Admin) hazýr olmasýný saðlar.
        /// </summary>
        /// <param name="builder">IdentityRole varlýðý için yapýlandýrma nesnesi.</param>
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            // Uygulama ilk çalýþtýðýnda veri tabanýna default rollerin eklenmesini saðlar.
            // EF Core HasData() ile IdentityRole eklerken her rolün Id property'si de zorunludur
            // Eðer Id vermezsen, migration sýrasýnda EF Core bu deðiþikliði her seferinde “pending” olarak algýlar.
            // *** Id verilmeyen IdentityRole nesneleri her migration kontrolünde yeniden eklenmiþ gibi algýlanýr.
            builder.HasData(
                new IdentityRole() { Id = "1", Name = "User", NormalizedName = "USER" },
                new IdentityRole() { Id = "2", Name = "Editor", NormalizedName = "EDITOR" },
                new IdentityRole() { Id = "3", Name = "Admin", NormalizedName = "ADMIN" }
            );
        }
    }
}