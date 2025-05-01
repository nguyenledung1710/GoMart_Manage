using GoMartApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoMartApplication.DAL
{
    class BillService : IDisposable
    {
        private readonly GoMart_Manage _context;
        private readonly IBillRepository _billRepo;

        public BillService()
        {
            _context = new GoMart_Manage();
            _billRepo = new BillRepository(_context);
        }

        /// <summary>
        /// Tạo sale, lưu Bill và BillDetails, cập nhật tồn kho
        /// </summary>
        // CHỈNH: trả về string (mã hoá đơn) thay vì int
        public string CreateSale(string sellerId, List<(int prodId, int qty, decimal price)> items)
        {
            // Sinh mã hoá đơn (ví dụ dùng GUID)
            var newBillId = Guid.NewGuid().ToString();

            // 1) Tạo Bill
            var bill = new Bill
            {
                Bill_ID = newBillId,
                SellDate = DateTime.Now,
                SellerID = sellerId,
                TotalAmt = items.Sum(i => i.qty * i.price)
            };
            _billRepo.Add(bill);

            // 2) Tạo BillDetails
            var details = items.Select(i => new BillDetail
            {
                Bill_ID = newBillId,
                ProdID = i.prodId,
                Qty = i.qty,
                Price = i.price
            }).ToList();
            _billRepo.AddRange(details);

            // 3) Cập nhật tồn kho
            foreach (var i in items)
            {
                var p = _context.Products.FirstOrDefault(x => x.ProdID == i.prodId);
                if (p != null) p.ProdQty -= i.qty;
            }
            _context.SaveChanges();

            return newBillId;
        }

        /// <summary>
        /// CHỈNH: nếu bạn có wrapper GetById ở BLL
        /// </summary>
        public Bill GetById(string billId)
        {
            return _billRepo.GetById(billId);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
