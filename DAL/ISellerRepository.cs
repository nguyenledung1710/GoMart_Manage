using GoMartApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoMartApplication.DAL
{
    interface ISellerRepository
    {
        bool Exists(string sellerId);
        void Add(Seller seller);
        Seller GetById(string sellerId);
        IEnumerable<Seller> GetAll();
        void Update(Seller seller);
        void Delete(Seller seller);
    }
}
