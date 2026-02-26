using Entities.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Services.Contracts;

namespace StoreApp.Infrastructure.TagHelpers
{
    /// <summary>
    /// Belirtilen sayýda son eklenen ürünleri listeleyen TagHelper.
    /// <div> etiketi üzerinde çalýþýr ve "products" attribute'u ile etkinleþir.
    /// </summary>
    [HtmlTargetElement("div", Attributes = "products")]
    public class LastestProductTagHelper : TagHelper
    {
        private readonly IServiceManager _manager;

        /// <summary>
        /// Listelenecek ürün sayýsýný belirtir.
        /// </summary>
        [HtmlAttributeName("number")]
        public int Number { get; set; } = 3; // varsayýlan deðer

        /// <summary>
        /// Baðýmlýlýklarý almak için constructor.
        /// </summary>
        /// <param name="manager">Uygulamadaki servis yöneticisi arayüzü.</param>
        public LastestProductTagHelper(IServiceManager manager)
        {
            _manager = manager;
        }

        /// <summary>
        /// HTML çýktýsýný oluþturur ve son ürünleri listeleyen dinamik bir yapý üretir.
        /// </summary>
        /// <param name="context">TagHelper ile ilgili baðlam bilgilerini içerir.</param>
        /// <param name="output">Oluþturulacak HTML çýktýsýný temsil eder.</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            TagBuilder div = new TagBuilder("div");
            div.Attributes.Add("class", "my-3");

            TagBuilder h6 = new TagBuilder("h6");
            h6.Attributes.Add("class", "lead");

            TagBuilder icon = new TagBuilder("i");
            icon.Attributes.Add("class", "fa fa-box text-secondary");

            h6.InnerHtml.AppendHtml(icon);
            h6.InnerHtml.AppendHtml($" Lastest Products {Number}");

            TagBuilder ul = new TagBuilder("ul");
            var products = _manager.ProductService.GetLastestProducts(Number, false);
            foreach (Product product in products)
            {
                TagBuilder li = new TagBuilder("li");

                TagBuilder a = new TagBuilder("a");
                a.Attributes.Add("href", $"/product/get/{product.ProductId}");
                a.InnerHtml.AppendHtml(product.ProductName);

                li.InnerHtml.AppendHtml(a);
                ul.InnerHtml.AppendHtml(li);
            }

            div.InnerHtml.AppendHtml(h6);
            div.InnerHtml.AppendHtml(ul);
            output.Content.AppendHtml(div);
        }
    }
}
