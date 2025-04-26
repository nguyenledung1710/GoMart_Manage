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

        /// <summary>
        /// Tạo Admin mới, có thêm trường Address.
        /// </summary>
        public bool CreateAdmin(string adminId, string password, string fullName, string address)
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
                FullName = fullName,
                address = address    // ← gán Address
            };
            _repo.Add(admin);
            return true;
        }

        /// <summary>
        /// Lấy Admin theo Id.
        /// </summary>
        public Admin GetAdminById(string adminId)
        {
            return _repo.GetById(adminId);
        }

        /// <summary>
        /// Lấy danh sách tất cả Admin.
        /// </summary>
        public IEnumerable<Admin> GetAllAdmins()
        {
            return _repo.GetAll();
        }

        /// <summary>
        /// Cập nhật Admin (có Address).
        /// </summary>
        public bool UpdateAdmin(string adminId, string password, string fullName, string address)
        {
            // kiểm tra tồn tại
            var existing = _repo.GetById(adminId);
            if (existing == null) return false;

            // cập nhật các field
            existing.Password = password;
            existing.FullName = fullName;
            existing.address = address;  // ← cập nhật Address

            _repo.Update(existing);
            return true;
        }

        /// <summary>
        /// Xóa Admin theo Id.
        /// </summary>
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
