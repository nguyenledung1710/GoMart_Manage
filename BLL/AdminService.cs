using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public bool CreateAdmin(string adminId, string password, string fullName)
        {
            // 1) Validate đầu vào
            if (string.IsNullOrWhiteSpace(adminId)
             || string.IsNullOrWhiteSpace(password)
             || string.IsNullOrWhiteSpace(fullName))
            {
                throw new ArgumentException("Các trường bắt buộc không được để trống.");
            }

            // 2) Kiểm tra trùng
            if (_repo.Exists(adminId))
                return false;

            // 3) Tạo và lưu
            var admin = new Admin
            {
                AdminId = adminId,
                Password = password,
                FullName = fullName
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
        public bool UpdateAdmin(string id, string pwd, string fullName)
        {
            if (!_repo.Exists(id)) return false;
            var admin = new Admin { AdminId = id, Password = pwd, FullName = fullName };
            _repo.Update(admin);
            return true;
        }

        public bool DeleteAdmin(string id)
        {
            var admin = _repo.GetById(id);
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
