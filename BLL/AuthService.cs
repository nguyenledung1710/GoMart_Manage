using System;
using System.Linq;
using GoMartApplication.DTO;

namespace GoMartApplication.BLL
{
    public class AuthService : IDisposable
    {
        private readonly GoMart_Manage _context;

        public AuthService()
        {
            _context = new GoMart_Manage();
        }

        public bool AuthenticateAdmin(string adminId, string password, out Admin admin)
        {
            admin = _context.Admins
                .FirstOrDefault(a => a.AdminId == adminId
                                  && a.Password == password);
            return admin != null;
        }

     
        public bool AuthenticateSeller(string sellerId, string password, out Seller seller)
        {
            seller = _context.Sellers
                .FirstOrDefault(s => s.SellerId == sellerId
                                  && s.SellerPass == password);
            return seller != null;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
