
using System.Collections.Generic;
using System.Linq;
using GoMartApplication.DTO;

namespace GoMartApplication.DAL
{
    public class SellerRepository : ISellerRepository
    {
        private readonly GoMart_Manage _context;
        public SellerRepository(GoMart_Manage context)
        {
            _context = context;
        }

        public void Add(Seller seller)
        {
            _context.Sellers.Add(seller);
            _context.SaveChanges();
        }

        public void Delete(Seller seller)
        {
            var existing = _context.Sellers.Find(seller.SellerId);
            if (existing != null)
            {
                _context.Sellers.Remove(existing);
                _context.SaveChanges();
            }
        }

        public bool Exists(string sellerId)
        {
            return _context.Sellers.Any(s => s.SellerId == sellerId);
        }

        public IEnumerable<Seller> GetAll()
        {
            return _context.Sellers.ToList();
        }

        public Seller GetById(string sellerId)
        {
            return _context.Sellers.Find(sellerId);
        }

        public void Update(Seller seller)
        {
            var existing = _context.Sellers.Find(seller.SellerId);
            if (existing != null)
            {
                existing.SellerName = seller.SellerName;
                existing.SellerAge = seller.SellerAge;  
                existing.SellerPhone = seller.SellerPhone;
                existing.SellerPass = seller.SellerPass;
                _context.SaveChanges();
            }
        }
        public Seller GetByCredentials(string sellerId, string password)
        {
            return _context.Sellers
                           .FirstOrDefault(s =>
                               s.SellerId == sellerId &&
                               s.SellerPass == password);
        }
        public IEnumerable<Seller> SearchSellers(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return _context.Sellers.ToList();

            keyword = keyword.ToLower();
            return _context.Sellers
                .Where(s => s.SellerId.ToLower().Contains(keyword)
                         || s.SellerName.ToLower().Contains(keyword))
                .ToList();
        }
    }
}
