using GoMartApplication.DTO;
using System;
using System.Collections.Generic;

namespace GoMartApplication.DAL
{
    public interface IAdminRepository
    {
        bool Exists(string adminId);
        void Add(Admin admin);
        Admin GetById(string adminId);
        IEnumerable<Admin> GetAll();
        void Update(Admin admin);
        void Delete(Admin admin);
        IEnumerable<Admin> Search(string keyword);
        Admin GetByCredentials(string adminId, string password);
    }
}
