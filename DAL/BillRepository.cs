using GoMartApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public void Delete(int id)
        {
            var bill = _context.Bills.Find(id);
            if (bill != null)
            {
                _context.Bills.Remove(bill);
                _context.SaveChanges();
            }
        }

        public List<Bill> GetAll()
        {
            return _context.Bills
                .Include(b => b.Seller)
                .OrderByDescending(b => b.SellDate)
                .ToList();
        }

        public Bill GetById(string billId)
        {
            
            return _context.Bills.Include(b => b.Seller).FirstOrDefault(b => b.Bill_ID == billId);
        }

        public List<BillDetail> GetDetails(string billId) =>
             _context.BillDetails
                 .Include(d => d.Product)
                 .Where(d => d.Bill_ID == billId)
                 .ToList();

        public void Update(Bill bill)
        {
            _context.Entry(bill).State = EntityState.Modified;
            _context.SaveChanges();
        }
        public string CreateBill(string sellerId, DateTime sellDate, decimal totalAmt)
        {
            var billId = Guid.NewGuid().ToString();
            var bill = new Bill
            {
                Bill_ID = billId,
                SellerID = sellerId,
                SellDate = sellDate,
                TotalAmt = totalAmt
            };
            _context.Bills.Add(bill);
            _context.SaveChanges();
            return billId;
        }
        public int CreateBillDetails(IEnumerable<BillDetail> details)
        {
            _context.BillDetails.AddRange(details);
            return _context.SaveChanges();
        }
        public IEnumerable<Bill> GetAllBills()
        {
            return _context.Bills
                .Include(b => b.Seller)
                .OrderByDescending(b => b.SellDate)
                .ToList();
        }

        public IEnumerable<Bill> GetBillsByDate(DateTime date)
        {
            return _context.Bills
                .Include(b => b.Seller)
                .Where(b => DbFunctions.TruncateTime(b.SellDate) == date.Date)
                .OrderByDescending(b => b.SellDate)
                .ToList();
        }

        public IEnumerable<Bill> GetBillsBySeller(string sellerId)
        {
            return _context.Bills
                .Include(b => b.Seller)
                .Where(b => b.Seller.SellerId == sellerId)
                .OrderByDescending(b => b.SellDate)
                .ToList();
        }
    }
}
