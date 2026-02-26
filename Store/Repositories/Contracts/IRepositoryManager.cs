namespace Repositories.Contracts
{
    /// <summary>
    /// Repository yöneticisi arayüzü.
    /// </summary>
    public interface IRepositoryManager
    {
        /// <summary>
        /// Ürün deposu.
        /// </summary>
        IProductRepository Product { get; }

        /// <summary>
        /// Kategori deposu.
        /// </summary>
        ICategoryRepository Category { get; }

        /// <summary>
        /// Sipariş deposu.
        /// </summary>
        IOrderRepository Order { get; }

        /// <summary>
        /// Değişiklikleri kaydeder.
        /// </summary>
        void Save();
    }
}