
using System.Data.Entity;


namespace GoMartApplication.DTO
{
    class CreateDB : CreateDatabaseIfNotExists<GoMart_Manage>
    {
        protected override void Seed(GoMart_Manage context)
        {
            base.Seed(context);
        }

    }

}
