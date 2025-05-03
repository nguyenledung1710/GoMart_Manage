using GoMartApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace GoMartApplication.DAL
{
    class ProductRepository : IProductRepository
    {
        private readonly GoMart_Manage _context;
        public ProductRepository(GoMart_Manage context)
        {
            _context = context;
        }

        public bool Exists(int prodId)
            => _context.Products.Any(p => p.ProdID == prodId);

        public void Add(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public Product GetById(int prodId)
            => _context.Products.Find(prodId);

        public IEnumerable<Product> GetAll()
        {
            return _context.Products
                  .Include(p => p.Category) 
                  .ToList();
        }
            

        public IEnumerable<Product> GetByCategory(int catId)
        {
            return _context.Products
                  .Include(p => p.Category)
                  .Where(p => p.ProdCatID == catId)
                  .ToList();
        }
            

        public void Update(Product product)
        {
            var existing = _context.Products.Find(product.ProdID);
            if (existing != null)
            {
                existing.ProdName = product.ProdName;
                existing.ProdCatID = product.ProdCatID;
                existing.ProdPrice = product.ProdPrice;
                existing.ProdQty = product.ProdQty;
                _context.SaveChanges();
            }
        }

        public void Delete(Product product)
        {
            _context.Products.Remove(product);
            _context.SaveChanges();
        }
    }
}
