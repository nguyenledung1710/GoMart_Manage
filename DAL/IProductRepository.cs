using GoMartApplication.DTO;
using System.Collections.Generic;


namespace GoMartApplication.DAL
{
    interface IProductRepository
    {
        bool Exists(int prodId);
        void Add(Product product);
        Product GetById(int prodId);
        IEnumerable<Product> GetAll();
        void Update(Product product);
        void Delete(Product product);
        void DecreaseQuantity(int prodId, int qty);
        IEnumerable<Product> GetAllByCategory(int catId);
        Product GetByNameAndPrice(int catId, string name, decimal price);
        void IncreaseQuantity(int prodId, int additionalQty);
    }
}
