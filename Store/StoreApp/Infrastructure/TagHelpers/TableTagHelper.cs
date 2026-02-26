using Microsoft.AspNetCore.Razor.TagHelpers;

namespace StoreApp.Infrastructure.TagHelpers
{
    /// <summary>
    /// Belirli bir <table> etiketine Bootstrap sýnýflarý eklemek için kullanýlan TagHelper.
    /// </summary>
    [HtmlTargetElement("table")]
    public class TableTagHelper : TagHelper
    {
        /// <summary>
        /// <table> etiketine "table table-hover" sýnýfýný ekler.
        /// </summary>
        /// <param name="context">TagHelper'ýn çalýþma zamanýnda içerik bilgilerini tutar.</param>
        /// <param name="output">TagHelper'ýn HTML çýktýsýný temsil eder ve deðiþtirilmesini saðlar.</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.SetAttribute("class", "table table-hover");
        }
    }

}