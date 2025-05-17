using GoMartApplication.DTO;
using System;
using System.Collections.Generic;

namespace GoMartApplication.DAL
{
    interface IBillRepository
    {
        void Add(Bill bill);
        void AddRange(IEnumerable<BillDetail> details);
        Bill GetById(string billId);
        List<Bill> GetAll();
        void Update(Bill bill);
        void Delete(int id);
        List<BillDetail> GetDetails(string billId);
        string CreateBill(string sellerId, DateTime sellDate, decimal totalAmt);
        int CreateBillDetails(IEnumerable<BillDetail> details);
        IEnumerable<Bill> GetAllBills();
        IEnumerable<Bill> GetBillsByDate(DateTime date);
        IEnumerable<Bill> GetBillsBySeller(string sellerId);

    }
}
