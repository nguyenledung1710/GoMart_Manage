using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using GoMartApplication.DAL;
using GoMartApplication.DTO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace GoMartApplication.BLL
{
    public class SellerService : IDisposable
    {
        private readonly GoMart_Manage _context;
        private readonly ISellerRepository _repo;

        public SellerService()
        {
            _context = new GoMart_Manage();
            _repo = new SellerRepository(_context);
        }

        public bool CreateSeller(string sellerId,
                                 string sellerName,
                                 int sellerAge,
                                 string sellerPhone,
                                 string sellerPass)
        {
            if (string.IsNullOrWhiteSpace(sellerId)
             || string.IsNullOrWhiteSpace(sellerName)
             || string.IsNullOrWhiteSpace(sellerPass)
             || string.IsNullOrWhiteSpace(sellerAge.ToString())
             || string.IsNullOrWhiteSpace(sellerPhone))
            {
                throw new ArgumentException("SellerID, Name và Password không được để trống.");
            }
            if (_repo.Exists(sellerId))
                return false;

            var seller = new Seller
            {
                SellerId = sellerId,
                SellerName = sellerName,
                SellerAge = sellerAge,
                SellerPhone = sellerPhone,
                SellerPass = sellerPass
            };
            _repo.Add(seller);
            return true;
        }

        public Seller GetSellerById(string sellerId)
            => _repo.GetById(sellerId);

        public IEnumerable<Seller> GetAllSellers()
            => _repo.GetAll();

        public bool UpdateSeller(string sellerId,
                                 string sellerName,
                                 int sellerAge,
                                 string sellerPhone,
                                 string sellerPass)
        {
            if (_repo.GetById(sellerId) == null) return false;
            var dto = new Seller
            {
                SellerId = sellerId,
                SellerName = sellerName,
                SellerAge = sellerAge,
                SellerPhone = sellerPhone,
                SellerPass = sellerPass
            };
            _repo.Update(dto);
            return true;
        }

        public bool DeleteSeller(string sellerId)
        {
            var existing = _repo.GetById(sellerId);
            if (existing == null)
                return false;

            _repo.Delete(existing);
            return true;
        }
        public List<Seller> SearchSellers(string keyword)
        {
            return _repo.SearchSellers(keyword).ToList();
        }

        public void Dispose()
            => _context.Dispose();
    }
}
