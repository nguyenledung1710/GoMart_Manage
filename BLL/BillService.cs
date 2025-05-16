using GoMartApplication.DAL;
using GoMartApplication.DTO;
using System;
using System.Collections.Generic;
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

        public void Dispose() => _context.Dispose();
    }
}
