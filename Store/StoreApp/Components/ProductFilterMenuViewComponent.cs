using Microsoft.AspNetCore.Mvc;

namespace StoreApp.Components
{
    public class ProductFilterMenuViewComponent : ViewComponent
    {
        /// <summary>
        /// Ürün filtreleme menüsünü render eden ViewComponent.
        /// Kullanýcýya ürünleri filtrelemek için gerekli seçenekleri sunar.
        /// </summary>
        /// <returns>Filtreleme menüsünü içeren View.</returns>
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}