using System.Linq.Expressions;

namespace Repositories.Contracts
{
    /// <summary>
    /// Temel depo arayüzü
    /// </summary>
    public interface IRepositoryBase<T>
    {
        /// <summary>
        /// Tüm kayıtları getirir.
        /// </summary>
        /// <param name="trackChanges">Değişikliklerin izlenip izlenmeyeceğini belirtir.</param>
        IQueryable<T> FindAll(bool trackChanges);

        /// <summary>
        /// Belirli bir koşula göre kayıtları getirir.
        /// </summary>
        /// <param name="expression">Koşul ifadesi.</param>
        /// <param name="trackChanges">Değişikliklerin izlenip izlenmeyeceğini belirtir.</param>
        T? FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges);

        /// <summary>
        /// Yeni bir kayıt oluşturur.
        /// </summary>
        /// <param name="entity">Oluşturulacak varlık.</param>
        void Create(T entity);
        
        /// <summary>
        /// Varlığı siler.
        /// </summary>
        /// <param name="entity">Silinecek varlık.</param>
        void Remove(T entity);

        /// <summary>
        /// Varlığı günceller.
        /// </summary>
        /// <param name="entity">Güncellenecek varlık.</param>
         void Update(T entity);
    }
}