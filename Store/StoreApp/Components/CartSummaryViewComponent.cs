using Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace StoreApp.Components
{
    /// <summary>
    /// Bu ViewComponent, kullanýcýnýn sepetinde bulunan ürünlerin sayýsýný ve toplam adedini özet olarak döndürür.
    /// </summary>
    public class CartSummaryViewComponent : ViewComponent
    {
        /// <summary>
        /// Sepet verilerini tutan Cart nesnesi. Dependency Injection aracýlýðýyla alýnýr.
        /// </summary>
        private readonly Cart _cart;

        /// <summary>
        /// CartSummaryViewComponent sýnýfýnýn yapýcý metodu.
        /// </summary>
        /// <param name="cartService">Sepet verilerini saðlayan Cart servisi.</param>
        public CartSummaryViewComponent(Cart cartService)
        {
            _cart = cartService;
        }

        /// <summary>
        /// Sepetteki toplam ürün çeþidi ve toplam adet bilgisini ":" karakteriyle ayrýlmýþ þekilde string olarak döner.
        /// </summary>
        /// <returns>
        /// Örnek çýktý: "3:8" 3 farklý ürün, toplamda 8 adet.
        /// </returns>
        public string Invoke()
        {
            var memberAndTotal = _cart.Lines.Count().ToString() +
                ":" +
                _cart.Lines.Sum(e => e.Quantity);

            return memberAndTotal;

            // Alternatif olarak sadece ürün çeþidi için:
            // return _cart.Lines.Count().ToString();
        }
    }
}
