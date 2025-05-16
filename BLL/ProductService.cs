using GoMartApplication.DAL;
using GoMartApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GoMartApplication.BLL
{
    class ProductService : IDisposable
    {
        private readonly GoMart_Manage _context;
        private readonly IProductRepository _repo;

        public ProductService()
        {
            _context = new GoMart_Manage();
            _repo = new ProductRepository(_context);
        }

        public bool CreateProduct(string name, int catId, decimal price, int qty)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Product name không được để trống.");
            var existing = _repo.GetByNameAndPrice(catId, name, price);

            if (existing != null)
            {
                _repo.IncreaseQuantity(existing.ProdID, qty);
            }
            else
            {
                var prod = new Product
                {
                    ProdName = name,
                    ProdCatID = catId,
                    ProdPrice = price,
                    ProdQty = qty
                };
                _repo.Add(prod);
            }
            return true;
        }

        public IEnumerable<Product> GetAllProducts()
            => _repo.GetAll();

        public IEnumerable<Product> GetProductsByCategory(int catId)
        {
            return _repo.GetAllByCategory(catId);
        }

        public bool UpdateProduct(int id, string name, int catId, decimal price, int qty)
        {
            if (_repo.GetById(id) == null) return false;
            var dto = new Product
            {
                ProdID = id,
                ProdName = name,
                ProdCatID = catId,
                ProdPrice = price,
                ProdQty = qty
            };
            _repo.Update(dto);
            return true;
        }

        public bool DeleteProduct(int id)
        {
            var existing = _repo.GetById(id);
            if (existing == null) return false;
            _repo.Delete(existing);
            return true;
        }
        public IEnumerable<Product> GetAllByCategory(int catId)
        {
            return _repo.GetAllByCategory(catId);
        }
        public void Dispose() => _context.Dispose();
    }
}
