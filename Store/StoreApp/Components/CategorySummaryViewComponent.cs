using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace StoreApp.Components
{
    /// <summary>
    /// Kategorilerin toplam sayýsýný döndüren ViewComponent sýnýfýdýr.
    /// Bu bileþen genellikle admin paneli gibi özet bilgiler sunan sayfalarda kullanýlýr.
    /// </summary>
    public class CategorySummaryViewComponent : ViewComponent
    {
        private readonly IServiceManager _manager;

        /// <summary>
        /// Baðýmlýlýklarý enjekte ederek servis yöneticisini yapýlandýrýr.
        /// </summary>
        /// <param name="manager">Servis yöneticisi arayüzü.</param>
        public CategorySummaryViewComponent(IServiceManager manager)
        {
            _manager = manager;
        }

        /// <summary>
        /// Tüm kategorilerin sayýsýný string olarak döner.
        /// </summary>
        /// <returns>Kategori sayýsýný temsil eden string deðer.</returns>
        public string Invoke()
        {
            return _manager
                .CategoryService
                .GetAllCategories(false)
                .Count()
                .ToString();
        }
    }
}
