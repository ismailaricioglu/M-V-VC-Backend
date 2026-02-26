using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace StoreApp.Components
{
    /// <summary>
    /// Vitrin ürünlerini render eden ViewComponent.
    /// Ürünleri, kullanýcýya göstermek için alýr ve ilgili View'e gönderir.
    /// </summary>
    public class ShowcaseViewComponent : ViewComponent
    {
        private readonly IServiceManager _manager;

        /// <summary>
        /// ShowcaseViewComponent sýnýfýnýn yapýcý metodu. 
        /// IServiceManager kullanýlarak gerekli servisler baþlatýlýr.
        /// </summary>
        /// <param name="manager">Servis yöneticisi nesnesi, ürün verilerini almak için kullanýlýr.</param>
        public ShowcaseViewComponent(IServiceManager manager)
        {
            _manager = manager;
        }

        /// <summary>
        /// Vitrin (showcase) ürünlerini döner ve belirtilen sayfa adýna göre uygun görünümü render eder.
        /// </summary>
        /// <param name="page">Render edilecek görünüm adý. Varsayýlan deðer "default" olup, özel liste görünümü için "list" gibi alternatifler kullanýlabilir.</param>
        /// <returns>Ýlgili görünüm ile birlikte vitrin ürünlerini içeren <see cref="IViewComponentResult"/> nesnesi.</returns>
        /// <remarks>
        /// Bu metot, ürünlerin öne çýkarýldýðý vitrin bölümü için kullanýlýr.
        /// `page` parametresi "default" deðilse, özel bir görünüm ("List" gibi) kullanýlýr.
        /// </remarks>
        public IViewComponentResult Invoke(string page = "default")
        {
            var products = _manager.ProductService.GetShowcaseProducts(false);
            return page.Equals("default")
                ? View(products)
                : View("List", products);
        }

    }
}