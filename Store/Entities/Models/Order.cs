using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    /// <summary>
    /// Sipariþ bilgilerini temsil eder. Ýçerisinde sipariþe ait ürünler, müþteri adresi ve durum bilgisi yer alýr.
    /// </summary>
    public class Order
    {
        /// <summary>
        /// Sipariþin benzersiz kimliði.
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Sipariþe ait ürün satýrlarý (ürün ve miktar bilgileri).
        /// </summary>
        public ICollection<CartLine> Lines { get; set; } = new List<CartLine>();

        /// <summary>
        /// Sipariþi veren kiþinin adý.
        /// </summary>
        [Required(ErrorMessage = "Name is required.")]
        public string? Name { get; set; }

        /// <summary>
        /// Adres satýrý 1 (zorunlu).
        /// </summary>
        [Required(ErrorMessage = "Line1 is required.")]
        public string? Line1 { get; set; }

        /// <summary>
        /// Adres satýrý 2 (opsiyonel).
        /// </summary>
        public string? Line2 { get; set; }

        /// <summary>
        /// Adres satýrý 3 (opsiyonel).
        /// </summary>
        public string? Line3 { get; set; }

        /// <summary>
        /// Þehir bilgisi.
        /// </summary>
        public string? City { get; set; }

        /// <summary>
        /// Hediye paketi isteði varsa true.
        /// </summary>
        public bool GiftWrap { get; set; }

        /// <summary>
        /// Sipariþin kargoya verilip verilmediði.
        /// </summary>
        public bool Shipped { get; set; }

        /// <summary>
        /// Sipariþin oluþturulma zamaný.
        /// </summary>
        public DateTime OrderedAt { get; set; } = DateTime.Now;
    }
}