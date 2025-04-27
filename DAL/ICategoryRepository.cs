using GoMartApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoMartApplication.DAL
{
    interface ICategoryRepository
    {
        bool Exists(int catId);
        void Add(Category category);
        Category GetById(int catId);
        IEnumerable<Category> GetAll();
        void Update(Category category);
        void Delete(Category category);
    }
}
