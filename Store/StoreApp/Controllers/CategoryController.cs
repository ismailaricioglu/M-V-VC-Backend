using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Repositories.Contracts;

namespace StoreApp.Controllers
{
    public class CategoryController : Controller
    {
        /// <summary>
        /// Depo yöneticisi örneği
        /// </summary>
        private readonly IRepositoryManager _manager;

        /// <summary>
        /// CategoryController constructor
        /// </summary>
        /// <param name="manager">Repository manager instance</param>
        public CategoryController(IRepositoryManager manager)
        {
            _manager = manager;
        }

        /// <summary>
        /// Kategorileri döndüren metot
        /// </summary>
        /// <returns>Kategori listesi</returns>
        public IEnumerable<Category> Index2()
        {
            return _manager.Category.FindAll(false);

        }

        /// <summary>
        /// Kategorileri döndüren metot
        /// </summary>
        /// <returns>Kategori listesi</returns>
        public IActionResult Index()
        {
            var model = _manager.Category.FindAll(false);
            return View(model);
        }
    }

}