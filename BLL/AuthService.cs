using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoMartApplication.DTO;

namespace GoMartApplication.BLL
{
    class AuthService : IDisposable
    {
        private readonly GoMart_Manage _context;

        public AuthService()
        {
            _context = new GoMart_Manage();
        }

        public bool AuthenticateAdmin(string adminId, string password, out Admin admin)
        {
            admin = _context.Admins
                .FirstOrDefault(a => a.AdminId == adminId && a.Password == password);
            return admin != null;
        }
        public bool AuthenticateSeller(string sellerName, string password, out Seller seller)
        {
            seller = _context.Sellers
                .FirstOrDefault(s => s.SellerName == sellerName && s.SellerPass == password);
            return seller != null;
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
