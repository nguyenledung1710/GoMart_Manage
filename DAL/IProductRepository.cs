using GoMartApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoMartApplication.DAL
{
    interface IProductRepository
    {
        bool Exists(int prodId);
        void Add(Product product);
        Product GetById(int prodId);
        IEnumerable<Product> GetAll();
        IEnumerable<Product> GetByCategory(int catId);
        void Update(Product product);
        void Delete(Product product);
    }
}
