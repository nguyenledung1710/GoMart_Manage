using GoMartApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoMartApplication.DAL
{
    interface IBillRepository
    {
        void Add(Bill bill);
        void AddRange(IEnumerable<BillDetail> details);
        Bill GetById(string billId);
    }
}
