using Microsoft.AspNetCore.Mvc;
using Repositories;
using Repositories.Contracts;
using Services.Contracts;

namespace StoreApp.Components
{
    public class CategoriesMenuViewComponent : ViewComponent
    {
        private readonly IServiceManager _manager;

        public CategoriesMenuViewComponent(IServiceManager manager)
        {
            _manager = manager;
        }


        public IViewComponentResult Invoke()
        {
            // service üzerinden ürünlerin sayısını alıyoruz. Repository üzerinden değil, Service üzerinden almak daha mantıklı. Gerekkli olan tüm işlemleri service katmanında yapıyoruz.
            var categories = _manager.CategoryService.GetAllCategories(false);
            return View(categories);
        }
    }
}