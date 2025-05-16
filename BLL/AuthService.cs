using System;
using GoMartApplication.DAL;
using GoMartApplication.DTO;

namespace GoMartApplication.BLL
{
    public class AuthService : IDisposable
    {

        private readonly GoMart_Manage _context;
        private readonly IAdminRepository _adminRepo;
        private readonly ISellerRepository _sellerRepo;

        public AuthService()
        {
            _context = new GoMart_Manage();
            _adminRepo = new AdminRepository(_context);
            _sellerRepo = new SellerRepository(_context);
        }

        public bool AuthenticateAdmin(string adminId, string password, out Admin admin)
        {
            admin = _adminRepo.GetByCredentials(adminId, password);
            return admin != null;
        }

        public bool AuthenticateSeller(string sellerId, string password, out Seller seller)
        {
            seller = _sellerRepo.GetByCredentials(sellerId, password);
            return seller != null;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
