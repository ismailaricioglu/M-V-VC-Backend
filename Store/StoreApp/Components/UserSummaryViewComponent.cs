using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace StoreApp.Components
{
    /// <summary>
    /// Sistemde kayýtlý toplam kullanýcý sayýsýný döndüren ViewComponent sýnýfýdýr.
    /// Bu bileþen, genellikle yönetim panelinde kullanýcýya dair özet bilgi göstermek için kullanýlýr.
    /// </summary>
    public class UserSummaryViewComponent : ViewComponent
    {
        private readonly IServiceManager _manager;

        /// <summary>
        /// Servis yöneticisi baðýmlýlýðý constructor üzerinden alýnýr.
        /// </summary>
        /// <param name="manager">Servis yöneticisi arayüzü (IServiceManager).</param>
        public UserSummaryViewComponent(IServiceManager manager)
        {
            _manager = manager;
        }

        /// <summary>
        /// Sistemde kayýtlý kullanýcý sayýsýný string olarak döner.
        /// </summary>
        /// <returns>Kullanýcý sayýsýný temsil eden string deðer.</returns>
        public string Invoke()
        {
            return _manager
                .AuthService
                .GetAllUsers()
                .Count()
                .ToString();
        }
    }
}
