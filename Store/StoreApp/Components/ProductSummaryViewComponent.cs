using Microsoft.AspNetCore.Mvc;
using Repositories;
using Repositories.Contracts;
using Services.Contracts;

namespace StoreApp.Components
{
    public class ProductSummaryViewComponent : ViewComponent
    {
        private readonly IServiceManager _manager;

        public ProductSummaryViewComponent(IServiceManager manager)
        {
            _manager = manager;
        }


        public string Invoke()
        {
            // service üzerinden ürünlerin sayısını alıyoruz. Repository üzerinden değil, Service üzerinden almak daha mantıklı. Gerekli olan tüm işlemleri service katmanında yapıyoruz.
            return _manager.ProductService.GetAllProducts(false).Count().ToString();
        }
    }
}