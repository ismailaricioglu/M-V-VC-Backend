using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;

namespace Repositories
{
    /// <summary>
    /// RepositoryBase sınıfı, temel veri erişim işlemlerini gerçekleştiren soyut bir sınıftır.
    /// </summary>
    /// <typeparam name="T">Veri modeli türü</typeparam>
    public abstract class RepositoryBase<T> : IRepositoryBase<T>
    where T : class, new()
    {
        /// <summary>
        /// Veri tabanı işlemleri için kullanılan context nesnesi.
        /// </summary>
        protected readonly RepositoryContext _context;

        /// <summary>
        /// RepositoryBase sınıfının kurucusu. Veritabanı işlemleri için kullanılan context nesnesini alır.
        /// </summary>
        /// <param name="context">Veritabanı işlemleri için kullanılan RepositoryContext nesnesi.</param>
        protected RepositoryBase(RepositoryContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Yeni varlığı ekler
        /// </summary>
        /// <param name="entity"></param>
        public void Create(T entity)
        {
            _context.Set<T>().Add(entity); // yeni varlığı ekler
        }

        /// <summary>
        /// Tüm verileri getirir.
        /// </summary>
        /// <param name="trackChanges">Veri değişikliklerini takip edip etmeyeceğini belirtir.</param>
        /// <returns>Veri sorgusu</returns>
        public IQueryable<T> FindAll(bool trackChanges)
        {
            return trackChanges
            ? _context.Set<T>() // takip eder vaziyette veriyi getir
            : _context.Set<T>().AsNoTracking(); // takipsizce veriyi getir
        }

        /// <summary>
        /// Belirli bir koşula göre ilk veriyi getirir.
        /// </summary>
        /// <param name="expression">Koşul ifadesi</param>
        /// <param name="trackChanges">Veri değişikliklerini takip edip etmeyeceğini belirtir</param>
        /// <returns>Koşula uyan ilk veri</returns>
        public T? FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges)
        {
            return trackChanges
            ? _context.Set<T>().Where(expression).SingleOrDefault() // takip eder vaziyette ilk veriyi getir
            : _context.Set<T>().Where(expression).AsNoTracking().SingleOrDefault(); // takipsizce ilk veriyi getir
        }

        /// <summary>
        /// Belirli bir varlığı siler.
        /// </summary>
        /// <param name="entity"></param>
        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity); // varlığı siler
        }

        /// <summary>
        /// Belirli bir varlığı günceller.
        /// </summary>
        /// <param name="entity"></param>
        public void Update(T entity)
        {
            _context.Set<T>().Update(entity); // varlığı günceller
        }
    }
}