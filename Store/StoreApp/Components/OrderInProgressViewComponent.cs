using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace StoreApp.Components
{
    /// <summary>
    /// Devam eden (iþlemde olan) sipariþ sayýsýný döndüren ViewComponent sýnýfýdýr.
    /// Bu bileþen genellikle yönetim panelinde özet bilgi göstermek amacýyla kullanýlýr.
    /// </summary>
    public class OrderInProgressViewComponent : ViewComponent
    {
        private readonly IServiceManager _manager;

        /// <summary>
        /// Servis yöneticisi baðýmlýlýðýný alýr.
        /// </summary>
        /// <param name="manager">Servis yöneticisi arayüzü.</param>
        public OrderInProgressViewComponent(IServiceManager manager)
        {
            _manager = manager;
        }

        /// <summary>
        /// Ýþlemde olan sipariþlerin sayýsýný string olarak döner.
        /// </summary>
        /// <returns>Ýþlemdeki sipariþ sayýsýný temsil eden string deðer.</returns>
        public string Invoke()
        {
            return _manager
                .OrderService
                .NumberOfInProcess
                .ToString();
        }
    }
}
