using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;

namespace Services
{
    /// <summary>
    /// OrderManager sýnýfý, sipariþlerle ilgili iþ mantýðýný yönetir ve IOrderService arayüzünü uygular.
    /// </summary>
    public class OrderManager : IOrderService
    {
        /// <summary>
        /// RepositoryManager örneði üzerinden ilgili repository'lere eriþim saðlanýr.
        /// </summary>
        private readonly IRepositoryManager _manager;

        /// <summary>
        /// OrderManager sýnýfýnýn kurucusudur.
        /// </summary>
        /// <param name="manager">RepositoryManager nesnesi</param>
        public OrderManager(IRepositoryManager manager)
        {
            _manager = manager;
        }

        /// <summary>
        /// Sipariþleri döner.
        /// </summary>
        public IQueryable<Order> Orders => _manager.Order.Orders;

        /// <summary>
        /// Teslim edilmemiþ sipariþ sayýsýný döner.
        /// </summary>
        public int NumberOfInProcess => _manager.Order.NumberOfInProcess;

        /// <summary>
        /// Belirtilen sipariþi tamamlanmýþ olarak iþaretler.
        /// </summary>
        /// <param name="id">Tamamlanacak sipariþin ID'si</param>
        public void Complete(int id)
        {
            _manager.Order.Complete(id);
            _manager.Save();
        }

        /// <summary>
        /// Belirli bir sipariþi getirir.
        /// </summary>
        /// <param name="id">Sipariþ ID</param>
        /// <returns>Sipariþ nesnesi</returns>
        public Order? GetOneOrder(int id)
        {
            return _manager.Order.GetOneOrder(id);
        }

        /// <summary>
        /// Yeni bir sipariþ kaydeder veya mevcut bir sipariþi günceller.
        /// </summary>
        /// <param name="order">Kaydedilecek sipariþ nesnesi</param>
        public void SaveOrder(Order order)
        {
            _manager.Order.SaveOrder(order);
        }
    }
}
