using Entities.Models;
using Repositories.Contracts;

namespace Repositories
{
    /// <summary>
    /// Kategori deposu sınıfı, RepositoryBase ve ICategoryRepository arayüzünü uygular.
    /// </summary>
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(RepositoryContext context) : base(context)
        {
        }
    }
}