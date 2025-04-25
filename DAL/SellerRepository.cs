using GoMartApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoMartApplication.DAL
{
    class SellerRepository : ISellerRepository
    {
        private readonly GoMart_Manage _context;
        public SellerRepository(GoMart_Manage contest)
        {
            _context = contest;
        }
        public void Add(Seller seller)
        {
            _context.Sellers.Add(seller);
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
            throw new NotImplementedException();
        }

        public IEnumerable<Seller> GetAll()
        {
            throw new NotImplementedException();
        }

        public Admin GetById(string sellerId)
        {
            throw new NotImplementedException();
        }

        public void Update(Seller seller)
        {
            throw new NotImplementedException();
        }
    }
}

