using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using StoreApp.Models;

namespace StoreApp.Infrastructure.TagHelpers
{
    /// <summary>
    /// Sayfalama (pagination) baðlantýlarý oluþturmak için kullanýlan özel bir TagHelper.
    /// </summary>
    [HtmlTargetElement("div", Attributes = "page-model")]
    public class PageLinkTagHelper : TagHelper
    {
        /// <summary>
        /// URL oluþturmak için kullanýlan yardýmcý fabrika sýnýfý.
        /// </summary>
        private readonly IUrlHelperFactory _urlHelperFactory;

        /// <summary>
        /// Görünüm baðlamý (ViewContext) bilgilerini tutar. Otomatik olarak ayarlanýr.
        /// </summary>
        [ViewContext] // ASP.NET Core'un ViewContext nesnesini otomatik olarak TagHelper'a enjekte etmesini saðlar.
        [HtmlAttributeNotBound] // Bu özelliðin HTML üzerinden bir öznitelik olarak baðlanmasýný engeller.
        public ViewContext? ViewContext { get; set; }

        /// <summary>
        /// Sayfalama iþlemi için gerekli verileri içeren model.
        /// </summary>
        public Pagination PageModel { get; set; }

        /// <summary>
        /// Sayfalama baðlantýlarýnýn yönlendireceði eylem adý (Action).
        /// </summary>
        public String? PageAction { get; set; }

        /// <summary>
        /// Sayfa baðlantýlarýnda CSS sýnýfý kullanýlsýn mý belirten bayrak.
        /// </summary>
        public bool PageClassesEnabled { get; set; } = false;

        /// <summary>
        /// Her baðlantýya eklenecek genel CSS sýnýfý.
        /// </summary>
        public string PageClass { get; set; } = String.Empty;

        /// <summary>
        /// Seçili olmayan sayfa baðlantýlarýnýn CSS sýnýfý.
        /// </summary>
        public string PageClassNormal { get; set; } = String.Empty;

        /// <summary>
        /// Seçili (aktif) sayfa baðlantýsýnýn CSS sýnýfý.
        /// </summary>
        public string PageClassSelected { get; set; } = String.Empty;

        /// <summary>
        /// PageLinkTagHelper sýnýfýnýn yapýlandýrýcýsý.
        /// </summary>
        /// <param name="urlHelperFactory">URL yardýmcý fabrikasý</param>
        public PageLinkTagHelper(IUrlHelperFactory urlHelperFactory)
        {
            _urlHelperFactory = urlHelperFactory;
        }

        /// <summary>
        /// Sayfalama baðlantýlarýný HTML olarak üretir.
        /// </summary>
        /// <param name="context">TagHelper baðlamý</param>
        /// <param name="output">Çýktý nesnesi</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (ViewContext is not null && PageModel is not null)
            {
                IUrlHelper urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);
                TagBuilder result = new TagBuilder("div");
                for (int i = 1; i <= PageModel.TotalPages; i++)
                {
                    TagBuilder tag = new TagBuilder("a");
                    tag.Attributes["href"] = urlHelper.Action(PageAction, new { PageNumber = i });
                    if (PageClassesEnabled)
                    {
                        tag.AddCssClass(PageClass);
                        tag.AddCssClass(i == PageModel.CurrentPage ? PageClassSelected : PageClassNormal);
                    }
                    tag.InnerHtml.Append(i.ToString());
                    result.InnerHtml.AppendHtml(tag);
                }
                output.Content.AppendHtml(result.InnerHtml);
            }
        }
    }
}