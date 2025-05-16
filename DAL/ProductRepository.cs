using GoMartApplication.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System;

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
        public void DecreaseQuantity(int prodId, int qty)
        {
            var p = _context.Products.Find(prodId);
            if (p == null) return;
            p.ProdQty -= qty;
            _context.SaveChanges();
        }

        public IEnumerable<Product> GetAllByCategory(int catId)
              => _context.Products
                         .Include(p => p.Category)
                         .Where(p => p.ProdCatID == catId)
                         .ToList();


        public Product GetByNameAndPrice(int catId, string name, decimal price)
        {
            return _context.Products
                           .FirstOrDefault(p =>
                               p.ProdCatID == catId
                            && p.ProdName.Equals(name, StringComparison.OrdinalIgnoreCase)
                            && p.ProdPrice == price);
        }

        public void IncreaseQuantity(int prodId, int additionalQty)
        {
            var existing = _context.Products.Find(prodId);
            if (existing != null)
            {
                existing.ProdQty += additionalQty;
                _context.SaveChanges();
            }
        }
    }
}
