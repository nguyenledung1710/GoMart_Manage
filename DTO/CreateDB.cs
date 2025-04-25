using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
