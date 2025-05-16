using GoMartApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace GoMartApplication.DAL
{
    class CategoryRepository : ICategoryRepository
    {
        private readonly GoMart_Manage _context;
        public CategoryRepository(GoMart_Manage context)
        {
            _context = context;
        }

        public bool Exists(int catId)
            => _context.Categories.Any(c => c.CatID == catId);

        public void Add(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public Category GetById(int catId)
            => _context.Categories.Find(catId);

        public IEnumerable<Category> GetAll()
        {
            return _context.Categories
                   .Include(c => c.Products)
                   .ToList();
        }

        public void Update(Category category)
        {
            var existing = _context.Categories.Find(category.CatID);
            if (existing != null)
            {
                existing.CategoryName = category.CategoryName;
                existing.CategoryDesc = category.CategoryDesc;
                _context.SaveChanges();
            }
        }

        public void Delete(Category category)
        {
            _context.Categories.Remove(category);
            _context.SaveChanges();
        }
    }
}
