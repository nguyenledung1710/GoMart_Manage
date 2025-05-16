using GoMartApplication.DTO;
using System.Collections.Generic;

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
        IEnumerable<Seller> SearchSellers(string keyword);
        Seller GetByCredentials(string sellerId, string password);
    }
}
