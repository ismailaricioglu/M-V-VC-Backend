using Entities.Models;

namespace Repositories.Contracts
{
    /// <summary>
    /// Sipariþ iþlemleri için temel metotlarý tanýmlar.
    /// </summary>
    public interface IOrderRepository
    {
        /// <summary>
        /// Sistemdeki tüm sipariþleri sorgulamak için kullanýlýr.
        /// </summary>
        IQueryable<Order> Orders { get; }

        /// <summary>
        /// Belirli bir ID deðerine sahip sipariþi getirir.
        /// </summary>
        /// <param name="id">Sipariþin ID deðeri.</param>
        /// <returns>Ýlgili sipariþ nesnesi, bulunamazsa null döner.</returns>
        Order? GetOneOrder(int id);

        /// <summary>
        /// Belirtilen sipariþi tamamlanmýþ olarak iþaretler.
        /// </summary>
        /// <param name="id">Tamamlanacak sipariþin ID'si.</param>
        void Complete(int id);

        /// <summary>
        /// Yeni bir sipariþi veritabanýna kaydeder.
        /// </summary>
        /// <param name="order">Kaydedilecek sipariþ nesnesi.</param>
        void SaveOrder(Order order);

        /// <summary>
        /// Henüz tamamlanmamýþ (Shipped = false) sipariþlerin sayýsýný döner.
        /// </summary>
        int NumberOfInProcess { get; }
    }

}