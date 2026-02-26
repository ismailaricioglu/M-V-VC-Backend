namespace StoreApp.Infrastructure.Extensions
{
    public static class HttpRequestExtension
    {
        /// <summary>
        /// Ýstek (request) nesnesinin tam yolunu ve varsa sorgu parametrelerini birleþtirerek döndürür.
        /// Örneðin: "/products?page=2" gibi bir sonuç üretir.
        /// </summary>
        /// <param name="request">HttpRequest nesnesi</param>
        /// <returns>Yol ve sorgu parametrelerinin birleþtirilmiþ hali</returns>
        public static string PathAndQuery(this HttpRequest request)
        {
            return request.QueryString.HasValue
                ? $"{request.Path}{request.QueryString}"
                : request.Path.ToString();
        }
    }
}
