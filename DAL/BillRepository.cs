using GoMartApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace GoMartApplication.DAL
{
    class BillRepository : IBillRepository
    {
        private readonly GoMart_Manage _context;
        public BillRepository(GoMart_Manage context)
        {
            _context = context;
        }

        public void Add(Bill bill)
        {
            _context.Bills.Add(bill);
            _context.SaveChanges();
        }

        public void AddRange(IEnumerable<BillDetail> details)
        {
            _context.BillDetails.AddRange(details);
            _context.SaveChanges();
        }

        public Bill GetById(string billId)
        {
            return _context.Bills.FirstOrDefault(b => b.Bill_ID == billId);
        }
    }
}
