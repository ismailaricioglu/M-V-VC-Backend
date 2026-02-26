using System.Text.Json;

namespace StoreApp.Infrastructure.Extensions
{
    /// <summary>
    /// Session nesnesi ile JSON formatýnda veri saklama ve okuma iþlemlerini kolaylaþtýran geniþletme (extension) metodlarýný içerir.
    /// </summary>
    public static class SessionExtension
    {
        /// <summary>
        /// Verilen nesneyi JSON formatýna dönüþtürerek session içerisinde belirtilen anahtar ile saklar.
        /// </summary>
        /// <param name="session">Verinin saklanacaðý session nesnesi.</param>
        /// <param name="key">Verinin session içerisinde saklanacaðý anahtar (key).</param>
        /// <param name="value">JSON formatýnda saklanacak nesne.</param>
        public static void SetJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        /// <summary>
        /// Belirtilen generic türdeki nesneyi JSON formatýnda session içinde saklar.
        /// </summary>
        /// <typeparam name="T">Saklanacak verinin tipi.</typeparam>
        /// <param name="session">Verinin saklanacaðý session nesnesi.</param>
        /// <param name="key">Verinin session içinde saklanacaðý anahtar (key).</param>
        /// <param name="value">Saklanacak nesne.</param>
        public static void SetJson<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        /// <summary>
        /// Session içerisindeki belirtilen anahtardan JSON formatýnda veri okur ve belirtilen türe dönüþtürür.
        /// </summary>
        /// <typeparam name="T">Dönüþtürülecek nesnenin tipi.</typeparam>
        /// <param name="session">Verinin okunacaðý session nesnesi.</param>
        /// <param name="key">Verinin saklandýðý anahtar (key).</param>
        /// <returns>Deserilize edilen veri. Veri yoksa varsayýlan (default) deðer döner.</returns>
        public static T? GetJson<T>(this ISession session, string key)
        {
            var data = session.GetString(key);
            return data is null
                ? default(T)
                : JsonSerializer.Deserialize<T>(data);
        }
    }
}
