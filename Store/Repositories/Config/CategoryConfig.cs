using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Config
{
    public class CategoryConfig : IEntityTypeConfiguration<Category>
    {
        /// <summary>
        /// Veritabanı yapılandırması için gerekli olan metot
        /// </summary>
        /// <param name="builder">EntityTypeBuilder nesnesi</param>
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            // Anahtar alanı
            builder.HasKey(c => c.CategoryId);

            // Zorunlu alanlar
            builder.Property(c => c.CategoryName).IsRequired();

            // Başlangıç verileri
            builder.HasData(
                new Category() { CategoryId = 1, CategoryName = "Books" },
                new Category() { CategoryId = 2, CategoryName = "Electronics" }
            );
        }
    }
}