﻿using System.Reflection;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Repositories.Config;

namespace Repositories
{
    /// <summary>
    /// Uygulamanın Entity Framework Core veritabanı bağlamını (DbContext) temsil eder.
    /// Identity kullanıcı yönetimi için IdentityDbContext'ten türetilmiştir.
    /// </summary>
    public class RepositoryContext : IdentityDbContext<IdentityUser>
    {
        /// <summary>
        /// Ürünler veritabanı tablosu
        /// </summary>
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// Kategoriler veritabanı tablosu
        /// </summary>
        public DbSet<Category> Categories { get; set; }

        /// <summary>
        /// Siparişler veritabanı tablosu
        /// </summary>
        public DbSet<Order> Orders { get; set; }

        /// <summary>
        /// Veritabanı bağlamı için yapılandırıcı.
        /// </summary>
        /// <param name="options">Bağlam seçenekleri</param>
        public RepositoryContext(DbContextOptions<RepositoryContext> options)
        : base(options)
        {

        }

        /// <summary>
        /// Veritabanı modellerini yapılandırmak için kullanılır.
        /// </summary>
        /// <param name="modelBuilder">Model oluşturucu nesnesi</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            /// <summary>
            /// Veritabanı yapılandırmalarını manuel olarak yükler.
            /// </summary>
            // modelBuilder.ApplyConfiguration(new ProductConfig());
            // modelBuilder.ApplyConfiguration(new CategoryConfig());

            /// <summary>
            /// Veritabanı yapılandırmalarını otomatik olarak yükler.
            /// </summary>
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}