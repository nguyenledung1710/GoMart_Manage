using GoMartApplication.DTO;
using System.Collections.Generic;


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
