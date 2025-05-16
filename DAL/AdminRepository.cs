using GoMartApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;


namespace GoMartApplication.DAL
{
    public class AdminRepository : IAdminRepository
    {
        private readonly GoMart_Manage _context;
        public AdminRepository(GoMart_Manage context)
        {
            _context = context;
        }
        public void Add(Admin admin)
        {
            _context.Admins.Add(admin);
            _context.SaveChanges();
        }

        public void Delete(Admin admin)
        {
            var existing = _context.Admins.Find(admin.AdminId);
            if (existing != null)
            {
                _context.Admins.Remove(existing);
                _context.SaveChanges();
            }
        }

        public bool Exists(string adminId)
        {
            return _context.Admins.Any(a => a.AdminId == adminId);
        }

        public IEnumerable<Admin> GetAll()
        {
            return _context.Admins.ToList();
        }

        public Admin GetById(string adminId)
        {
            return _context.Admins.Find(adminId);
        }

        public void Update(Admin admin)
        {
            var existing = _context.Admins.Find(admin.AdminId);
            if (existing != null)
            {
                existing.address = admin.address;
                existing.Password = admin.Password;
                existing.FullName = admin.FullName;
                _context.SaveChanges();
            }
        }
        public Admin GetByCredentials(string adminId, string password)
        {
            return _context.Admins
                           .FirstOrDefault(a =>
                               a.AdminId == adminId &&
                               a.Password == password);
        }
        public IEnumerable<Admin> Search(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return _context.Admins.ToList();

            keyword = keyword.ToLower();
            return _context.Admins
                .Where(a =>
                    a.AdminId.ToLower().Contains(keyword) ||
                    a.FullName.ToLower().Contains(keyword))
                .ToList();
        }
    }
}
