using GoMartApplication.DAL;
using GoMartApplication.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;



namespace GoMartApplication.BLL
{
    class BillService : IDisposable
    {
        private readonly GoMart_Manage _context;
        private readonly IBillRepository _billRepo;
        private readonly IProductRepository _prodRepo;
        private readonly ISellerRepository _sellerRepo;

        public BillService()
        {
            _context = new GoMart_Manage();
            _billRepo = new BillRepository(_context);
            _prodRepo = new ProductRepository(_context);
            _sellerRepo = new SellerRepository(_context);
        }

        public string CreateSale(string sellerId, List<(int ProdID, int Qty, decimal Price)> items)
        {
            if (_sellerRepo.GetById(sellerId) == null)
                throw new InvalidOperationException("Chỉ Seller mới được bán!");
            var totalAmt = items.Sum(x => x.Price * x.Qty);
            var billId = _billRepo.CreateBill(sellerId, DateTime.Now, totalAmt);
            var details = items.Select(x => new BillDetail
            {
                Bill_ID = billId,
                ProdID = x.ProdID,
                Price = x.Price,
                Qty = x.Qty,
                Total = x.Price * x.Qty
            });
            _billRepo.CreateBillDetails(details);
            foreach (var it in items)
                _prodRepo.DecreaseQuantity(it.ProdID, it.Qty);

            return billId;
        }

        public List<Bill> GetAllBills() =>
            _billRepo.GetAll();

        public Bill GetById(string billId) =>
            _billRepo.GetById(billId);

        public List<BillDetail> GetBillDetails(string billId) =>
            _billRepo.GetDetails(billId);


        public (DataTable Table, int TotalBills, int TotalItems, decimal TotalRevenue)
            GenerateStatistics(string mode, DateTime? date = null, string sellerId = null)
        {
            IEnumerable<Bill> bills = _billRepo.GetAll();

            switch (mode)
            {
                case "By Date" when date.HasValue:
                    bills = _billRepo.GetBillsByDate(date.Value);
                    break;
                case "By Seller" when sellerId != null:
                    bills = _billRepo.GetBillsBySeller(sellerId);
                    break;
            }

            var dt = new DataTable();
            dt.Columns.Add("ProdID", typeof(int));
            dt.Columns.Add("ProdName", typeof(string));
            dt.Columns.Add("Qty", typeof(int));
            dt.Columns.Add("Price", typeof(decimal));
            dt.Columns.Add("Total", typeof(decimal));
            dt.Columns.Add("SellerName", typeof(string));
            dt.Columns.Add("SellDate", typeof(DateTime));

            int countBills = bills.Count();
            int countItems = 0;
            decimal revenue = 0m;

            foreach (var b in bills)
            {
                var details = _billRepo.GetDetails(b.Bill_ID);
                foreach (var d in details)
                {
                    dt.Rows.Add(
                        d.ProdID,
                        d.Product.ProdName,
                        d.Qty,
                        d.Price,
                        d.Total,
                        b.Seller.SellerName,
                        b.SellDate
                    );
                    countItems += d.Qty;
                    revenue += d.Total;
                }
            }

            return (dt, countBills, countItems, revenue);
        }
        public void Dispose() => _context.Dispose();
    }
}
