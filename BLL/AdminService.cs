using System;
using System.Collections.Generic;
using GoMartApplication.DAL;
using GoMartApplication.DTO;

namespace GoMartApplication.BLL
{
    public class AdminService : IDisposable
    {
        private readonly GoMart_Manage _context;
        private readonly IAdminRepository _repo;

        public AdminService()
        {
            _context = new GoMart_Manage();
            _repo = new AdminRepository(_context);
        }

        public bool CreateAdmin(string adminId, string password, string fullName, string address)
        {
            if (string.IsNullOrWhiteSpace(adminId)
             || string.IsNullOrWhiteSpace(password)
             || string.IsNullOrWhiteSpace(fullName))
            {
                throw new ArgumentException("Các trường bắt buộc không được để trống.");
            }
            if (_repo.Exists(adminId))
                return false;

            var admin = new Admin
            {
                AdminId = adminId,
                Password = password,
                FullName = fullName,
                address = address  
            };
            _repo.Add(admin);
            return true;
        }
        public Admin GetAdminById(string adminId)
        {
            return _repo.GetById(adminId);
        }
        public IEnumerable<Admin> GetAllAdmins()
        {
            return _repo.GetAll();
        }
        public bool UpdateAdmin(string adminId, string password, string fullName, string address)
        {
            var existing = _repo.GetById(adminId);
            if (existing == null) return false;
            existing.AdminId = adminId;
            existing.Password = password;
            existing.FullName = fullName;
            existing.address = address; 

            _repo.Update(existing);
            return true;
        }

        public bool DeleteAdmin(string adminId)
        {
            var admin = _repo.GetById(adminId);
            if (admin == null) return false;
            _repo.Delete(admin);
            return true;
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
