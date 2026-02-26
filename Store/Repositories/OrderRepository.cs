using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;

namespace Repositories
{
    /// <summary>
    /// Sipariþlerle ilgili veritabaný iþlemlerini gerçekleþtiren repository sýnýfýdýr.
    /// </summary>
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        /// <summary>
        /// OrderRepository sýnýfýnýn yapýcýsýdýr.
        /// </summary>
        /// <param name="context">Veritabaný baðlamý (RepositoryContext).</param>
        public OrderRepository(RepositoryContext context) : base(context)
        {
        }

        /// <summary>
        /// Sipariþleri ürün ve ürün satýrlarý ile birlikte getirir; gönderilme durumuna göre sýralar.
        /// </summary>
        public IQueryable<Order> Orders => _context.Orders
            .Include(o => o.Lines)
            .ThenInclude(cl => cl.Product)
            .OrderBy(o => o.Shipped)
            .ThenByDescending(o => o.OrderId);

        /// <summary>
        /// Henüz gönderilmemiþ sipariþlerin sayýsýný döner.
        /// </summary>
        public int NumberOfInProcess =>
            _context.Orders.Count(o => o.Shipped.Equals(false));

        /// <summary>
        /// Belirtilen ID'ye sahip sipariþi tamamlanmýþ olarak iþaretler.
        /// </summary>
        /// <param name="id">Tamamlanacak sipariþin ID'si.</param>
        public void Complete(int id)
        {
            var order = FindByCondition(o => o.OrderId.Equals(id), true);
            if (order is null)
                throw new Exception("Order could not found!");
            order.Shipped = true;
        }

        /// <summary>
        /// Belirtilen ID'ye sahip tek bir sipariþi getirir.
        /// </summary>
        /// <param name="id">Sipariþ ID'si.</param>
        /// <returns>Ýlgili sipariþ nesnesi veya null.</returns>
        public Order? GetOneOrder(int id)
        {
            return FindByCondition(o => o.OrderId.Equals(id), false);
        }

        /// <summary>
        /// Sipariþi kaydeder. Yeni sipariþse ekler, ardýndan deðiþiklikleri veritabanýna yazar.
        /// </summary>
        /// <param name="order">Kaydedilecek sipariþ nesnesi.</param>
        public void SaveOrder(Order order)
        {
            _context.AttachRange(order.Lines.Select(l => l.Product));
            if (order.OrderId == 0)
            {
                _context.Orders.Add(order);
            }
            _context.SaveChanges();
        }
    }
}
