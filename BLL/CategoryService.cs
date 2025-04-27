using GoMartApplication.DAL;
using GoMartApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoMartApplication.BLL
{
   public class CategoryService : IDisposable
    {
        private readonly GoMart_Manage _context;
        private readonly ICategoryRepository _repo;

        public CategoryService()
        {
            _context = new GoMart_Manage();
            _repo = new CategoryRepository(_context);
        }

        public bool CreateCategory(string name, string desc)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("CategoryName không được để trống.");


            var category = new Category
       {
                CategoryName = name,
                CategoryDesc = desc
        };

            _repo.Add(category);
            return true;
        }

        public IEnumerable<Category> GetAllCategories()
            => _repo.GetAll();

        public bool UpdateCategory(int id, string name, string desc)
        {
            var existing = _repo.GetById(id);
            if (existing == null)
                return false;

            existing.CategoryName = name;
            existing.CategoryDesc = desc;
            _repo.Update(existing);
            return true;
        }

        public bool DeleteCategory(int id)
        {
            var existing = _repo.GetById(id);
            if (existing == null)
                return false;

            _repo.Delete(existing);
            return true;
        }

        public void Dispose() => _context.Dispose();
    }
}
