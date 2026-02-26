using Entities.Models;
using StoreApp.Infrastructure.Extensions;
using System.Text.Json.Serialization;

namespace StoreApp.Models
{
    public class SessionCart : Cart
    {
        /// <summary>
        /// [JsonIgnore]:
        /// Session verisini içeren, ancak JSON serileştirilmesine dahil edilmeyen bir özellik.
        /// Bu, örneğin sepet verisinin session ile ilişkili tutulmasını sağlar,
        /// ancak sepeti JSON formatında saklamak veya dışa aktarmak gerektiğinde bu özellik dışlanır.
        /// </summary>
        /// /// <summary>
        /// public ISession? Session { get; set; }:
        /// Kullanıcının oturumuna ait Session nesnesini temsil eder.
        /// Bu sayede session içindeki verilerle etkileşim kurulabilir.
        /// </summary>
        [JsonIgnore]
        public ISession? Session { get; set; }

        /// <summary>
        /// Oturumdan alışveriş sepeti (`SessionCart`) nesnesini alır.
        /// Eğer session'da kayıtlı bir sepet yoksa yeni bir `SessionCart` oluşturur.
        /// </summary>
        public static Cart GetCart(IServiceProvider services)
        {
            /// <summary>
            /// IHttpContextAccessor üzerinden aktif kullanıcının Session nesnesine erişir.
            /// </summary>
            ISession? session = services.GetRequiredService<IHttpContextAccessor>()
                .HttpContext?.Session;

            /// <summary>
            /// Session içindeki "cart" anahtarına karşılık gelen veriyi
            /// `SessionCart` tipine deserialize ederek elde eder.
            /// Eğer bulunamazsa yeni bir `SessionCart` örneği oluşturur.
            /// </summary>
            SessionCart cart = session?.GetJson<SessionCart>("cart") ?? new SessionCart();

            /// <summary>
            /// SessionCart nesnesinin `Session` özelliğini session ile ilişkilendirir.
            /// Bu, ileride Session'a veri eklemek veya güncellemek için kullanılır.
            /// </summary>
            cart.Session = session;

            return cart;
        }

        /// <summary>
        /// Belirtilen ürün ve miktarı sepete ekler.
        /// İşlemden sonra güncellenmiş sepeti session'a kaydeder.
        /// </summary>
        /// <param name="product">Sepete eklenecek ürün.</param>
        /// <param name="quantity">Eklenecek ürün adedi.</param>
        public override void AddItem(Product product, int quantity)
        {
            base.AddItem(product, quantity);
            Session?.SetJson<SessionCart>("cart", this);
        }

        /// <summary>
        /// Sepetteki tüm ürünleri temizler.
        /// İşlem sonrası session'daki sepet verisini kaldırır.
        /// </summary>
        public override void Clear()
        {
            base.Clear();
            Session?.Remove("cart");
        }

        /// <summary>
        /// Belirtilen ürünü sepetteki ürünler arasından siler.
        /// İşlemden sonra güncellenmiş sepeti session'a kaydeder.
        /// </summary>
        /// <param name="product">Sepetten çıkarılacak ürün.</param>
        public override void RemoveLine(Product product)
        {
            base.RemoveLine(product);
            Session?.SetJson<SessionCart>("cart", this);
        }
    }
}
