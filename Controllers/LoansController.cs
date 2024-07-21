using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Web;
using System.Web.Mvc;
using E1;
using E1.Models;

namespace E1.Controllers
{
    public class LoansController : Controller
    {
        private EEntities db = new EEntities();

        // GET: Loans
        public ActionResult Index()
        {
            var loans = db.Loans.Include(l => l.Branch).Include(l => l.Customer).ToList();


            return View(loans);
        }

        // GET: Loans/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Loan loan = db.Loans.Find(id);
            if (loan == null)
            {
                return HttpNotFound();
            }
            return View(loan);
        }

        // GET: Loans/Create
        public ActionResult Create()
        {
            ViewBag.BranchId = new SelectList(db.Branches, "BranchId", "BranchName");
            var CustomersList = db.Customers.Where(m => m.BranchId == db.Branches.FirstOrDefault().BranchId).ToList();
            ViewBag.CustomerId = new SelectList(CustomersList, "CustomerId", "CustomerName");
            var RepaymentTypesList = new List<RepaymentTypes>(); var Months = new RepaymentTypes { RepaymentTypeId = 1, RepaymentTypeName = "Month" };
            var Weeks = new RepaymentTypes { RepaymentTypeId = 2, RepaymentTypeName = "Week" }; RepaymentTypesList.Add(Months); RepaymentTypesList.Add(Weeks);
            ViewBag.RepaymentTypeId = new SelectList(RepaymentTypesList, "RepaymentTypeId", "RepaymentTypeName");
            LoanViewModel loanVM = new LoanViewModel();
            return View(loanVM);
        }

        [HttpPost]
        public ActionResult GetCustomers(string BranchId)
        {
            int branchId;
            List<SelectListItem> districtNames = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(BranchId))
            {
                branchId = Convert.ToInt32(BranchId);
                var CustomersList = db.Customers.Where(m => m.BranchId == branchId).ToList();
                CustomersList.ForEach(x =>
                {
                    districtNames.Add(new SelectListItem { Text = x.CustomerName, Value = x.CustomerId.ToString() });
                });
            }
            return Json(districtNames, JsonRequestBehavior.AllowGet);
        }

        // POST: Loans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LoanId,LoanName,BranchId,CustomerId,DistributdedAmount,DistributdedDate,FirstDueDate,OpeningBalance,InterestPercentage,EMIAmount,NoOfDues,RepaymentTypeId,CashFlow,InterestType,StartDueNumber")] LoanViewModel loanVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Part 1 : Loan Insert

                    var loan = new Loan
                    {
                        LoanId = loanVM.LoanId,
                        LoanName = db.Customers.Where(m => m.CustomerId == loanVM.CustomerId).FirstOrDefault().CustomerName+"-"+loanVM.DistributdedDate.Value.ToString("dd-MM-yy").Replace("-", String.Empty),
                        BranchId = loanVM.BranchId,
                        CustomerId = loanVM.CustomerId,
                        DistributdedAmount = loanVM.OpeningBalance,
                        DistributdedDate = loanVM.DistributdedDate,
                        FirstDueDate = loanVM.FirstDueDate,
                        InterestPercentage = loanVM.InterestPercentage,
                        EMIAmount = loanVM.EMIAmount,
                        NoOfDues = loanVM.NoOfDues,
                        RepaymentTypeId = loanVM.RepaymentTypeId

                    };
                    db.Loans.Add(loan);
                    db.SaveChanges();
                    loanVM.LoanId = loan.LoanId;



                    // Part 2 : Loancard Insert 
                    var PlannedCollectionDate = loanVM.FirstDueDate; //5 *
                    var OpeningBalance = loanVM.OpeningBalance; //7 *

                    int startDueNumber = 1;
                    //if (loanVM.StartDueNumber != null)
                    //{
                    //    startDueNumber = (int)loanVM.StartDueNumber;
                    //}

                    for (int i = startDueNumber; i <= loanVM.NoOfDues; i++)
                    {

                        var LoanCardId = 0; // 1
                        var LoanId = loanVM.LoanId; // 2
                        var LoanCardName = (i.ToString().Length > 1) ? (loan.LoanName + "-" + PlannedCollectionDate.Value.ToString("dd-MMM-yy").Replace("-", String.Empty) + "-D" + i.ToString()) : (loan.LoanName + "-" + PlannedCollectionDate.Value.ToString("dd-MMM-yy").Replace("-", String.Empty) + "-D0" + i.ToString()); // 3  LTOP-CIN-RAMES-301123-06MAY23D01
                        var DueNumber = i; //4 // no need
                        var ActualCollectionDate = PlannedCollectionDate; //6
                        var PlannedProfitAccount = Convert.ToDecimal(OpeningBalance * (loanVM.InterestPercentage / 100));
                        var ActualProfitAccount = PlannedProfitAccount; //9
                        var ImpactTransValue = (loan.Customer.Branch.BranchName.ToLower().Contains("-cin")) ? (loanVM.EMIAmount - PlannedProfitAccount) : (loanVM.EMIAmount + PlannedProfitAccount);  //10
                        var ClosingBalance = (loan.Customer.Branch.BranchName.ToLower().Contains("-cin")) ? (OpeningBalance - ImpactTransValue): (OpeningBalance + ImpactTransValue); //11
                        var PlannedCashAccount = (loan.Customer.Branch.BranchName.ToLower().Contains("-cin")) ? loanVM.EMIAmount : -(loanVM.EMIAmount);  //12
                        var ActualCashAccount = PlannedCashAccount; //13
                        var IsCollected = false;   //14
                        if (loan.Customer.Branch.Leader.LeaderName.ToLower()=="lfom")
                        {
                            ImpactTransValue = OpeningBalance = ClosingBalance =0;
                            PlannedProfitAccount = -Convert.ToDecimal(loanVM.EMIAmount);
                            PlannedCashAccount = ActualCashAccount = ActualProfitAccount = PlannedProfitAccount;
                        }
                      
                        switch (loan.Customer.Branch.Leader.LeaderName.ToLower())
                        {
                            case "ltoe":
                                ImpactTransValue = -(ImpactTransValue);
                                break;                           
                            default:
                                ImpactTransValue = ImpactTransValue;
                                break;
                        }
                        var loancard = new LoanCard
                        {
                            LoanCardId = LoanCardId,
                            LoanId = LoanId,
                            LoanCardName = LoanCardName,
                            DueNumber = DueNumber,
                            PlannedCollectionDate = PlannedCollectionDate,
                            ActualCollectionDate = ActualCollectionDate,
                            OpeningBalance = OpeningBalance,
                            PlannedProfitAccount = PlannedProfitAccount,
                            ActualProfitAccount = ActualProfitAccount,
                            ImpactTransValue = ImpactTransValue,
                            ClosingBalance = ClosingBalance,
                            PlannedCashAccount = PlannedCashAccount,
                            ActualCashAccount = ActualCashAccount,
                            IsCollected = IsCollected,
                            IsActive=true
                        };

                        db.LoanCards.Add(loancard);
                        db.SaveChanges();

                        PlannedCollectionDate = (loanVM.RepaymentTypeId == 1) ? PlannedCollectionDate.Value.AddMonths(1) : PlannedCollectionDate.Value.AddDays(7);
                        OpeningBalance = ClosingBalance;
                    }



                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {

                throw;
            }
           

            //ViewBag.BranchId = new SelectList(db.Branches, "BranchId", "BranchName", loan.BranchId);
            //ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "CustomerName", loan.CustomerId);
            //ViewBag.RepaymentTypeId = new SelectList(db.RepaymentTypes, "RepaymentTypeId", "RepaymentTypeName", loan.RepaymentTypeId);
            return RedirectToAction("Index");
        }

        // GET: Loans/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Loan loan = db.Loans.Find(id);
            if (loan == null)
            {
                return HttpNotFound();
            }
            ViewBag.BranchId = new SelectList(db.Branches, "BranchId", "BranchName", loan.BranchId);
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "CustomerName", loan.CustomerId);
            ViewBag.RepaymentTypeId = new SelectList(db.RepaymentTypes, "RepaymentTypeId", "RepaymentTypeName", loan.RepaymentTypeId);
            return View(loan);
        }

        // POST: Loans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LoanId,LoanName,BranchId,CustomerId,DistributdedAmount,DistributdedDate,FirstDueDate,OpeningBalance,InterestPercentage,EMIAmount,NoOfDues,RepaymentTypeId")] Loan loan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(loan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BranchId = new SelectList(db.Branches, "BranchId", "BranchName", loan.BranchId);
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "CustomerName", loan.CustomerId);
            ViewBag.RepaymentTypeId = new SelectList(db.RepaymentTypes, "RepaymentTypeId", "RepaymentTypeName", loan.RepaymentTypeId);
            return View(loan);
        }

        // GET: Loans/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Loan loan = db.Loans.Find(id);
            if (loan == null)
            {
                return HttpNotFound();
            }
            return View(loan);
        }

        // POST: Loans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Loan loan = db.Loans.Find(id);
            List<LoanCard> loanCards = db.LoanCards.Where(s => s.LoanId == id).ToList();
            db.LoanCards.RemoveRange(loanCards);
            db.SaveChanges();

            db.Loans.Remove(loan);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }


    public class RepaymentTypes
    {
        public int RepaymentTypeId { get; set; }
        public string RepaymentTypeName { get; set; }
    }
}
