﻿using GoMartApplication.DAL;
using GoMartApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Windows.Forms;


namespace GoMartApplication.BLL
{
    class BillService : IDisposable
    {
        private readonly GoMart_Manage _context;
        private readonly IBillRepository _repo;

        public BillService()
        {
            _context = new GoMart_Manage();
            _repo = new BillRepository(_context);
        }

        public string CreateSale(string sellerId, List<(int ProdID, int Qty, decimal Price)> items)
        {
            var seller = _context.Sellers.SingleOrDefault(s => s.SellerId == sellerId);
            if (seller == null)
            {
                MessageBox.Show("Bạn đang đăng nhập với tư cách là Admin nên không được bán!");
                
            }

            var billId = Guid.NewGuid().ToString();

            var bill = new Bill
            {
                Bill_ID = billId,
                SellerID = sellerId,
                SellDate = DateTime.Now,
                TotalAmt = items.Sum(x => x.Price * x.Qty)
            };
            _repo.Add(bill);

            var details = items.Select(x => new BillDetail
            {
                Bill_ID = billId,
                ProdID = x.ProdID,
                Price = x.Price,
                Qty = x.Qty,
                Total = x.Price * x.Qty
            }).ToList();
            _repo.AddRange(details);

            foreach (var i in items)
            {
                var p = _context.Products.FirstOrDefault(p0 => p0.ProdID == i.ProdID);
                if (p != null) p.ProdQty -= i.Qty;
            }
            _context.SaveChanges();

            return billId;
        }
        public List<Bill> GetAllBills()
        {
            return _context.Bills
                   .Include(b => b.Seller)
                   .OrderByDescending(b => b.SellDate)
                   .ToList();
        }
        public Bill GetById(string billId)
        {
            return _context.Bills
                           .Include(b => b.Seller)          
                           .FirstOrDefault(b => b.Bill_ID == billId);
        }
        public List<BillDetail> GetBillDetails(string billId)
        {
            return _context.BillDetails
                .Include(d => d.Product)
                .Where(d => d.Bill_ID == billId)
                .ToList();
        }

        public void Dispose() => _context.Dispose();
    }
}
