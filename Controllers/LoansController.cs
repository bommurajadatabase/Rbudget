using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using E1;

namespace E1.Controllers
{
    public class LoansController : Controller
    {
        private EEntities db = new EEntities();

        // GET: Loans
        public ActionResult Index()
        {
            var loans = db.Loans.Include(l => l.Branch).Include(l => l.Customer).Include(l => l.RepaymentType).ToList();

           
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
            ViewBag.RepaymentTypeId = new SelectList(db.RepaymentTypes, "RepaymentTypeId", "RepaymentTypeName");
            return View();
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
        public ActionResult Create([Bind(Include = "LoanId,LoanName,BranchId,CustomerId,DistributdedAmount,DistributdedDate,FirstDueDate,OpeningBalance,InterestPercentage,EMIAmount,NoOfDues,RepaymentTypeId")] Loan loan)
        {
            if (ModelState.IsValid)
            {
                db.Loans.Add(loan);
                db.SaveChanges();

                // Loancard automatically insert logic

                var NoOfDues = loan.NoOfDues;
                var collectionDate = loan.FirstDueDate;
                var openingBalance = loan.OpeningBalance;
                var impactTransValue = loan.EMIAmount;

                for (int i = 0; i < NoOfDues; i++)
                {
                    var loancard = new LoanCard();
                    loancard.LoanId = loan.LoanId;
                    loancard.LoanCardName = loan.LoanName;
                    loancard.DueNumber = i + 1;
                    loancard.PlannedCollectionDate = collectionDate;
                    loancard.OpeningBalance = openingBalance;
                    loancard.ImpactTransValue = impactTransValue;
                    loancard.NonImpactTransValue =0;
                    loancard.ClosingBalance = openingBalance-impactTransValue;
                    loancard.PlannedCashAccount = impactTransValue;
                    loancard.ActualCashAccount = impactTransValue;
                    loancard.PlannedProfitAccount = 0;
                    loancard.ActualProfitAccount = 0;
                    loancard.ActualCollectionDate = collectionDate;
                    loancard.IsCollected = false;

                    db.LoanCards.Add(loancard);
                    db.SaveChanges();

                    collectionDate = collectionDate.Value.AddDays(7);
                    openingBalance = loancard.ClosingBalance;

                }
              





                return RedirectToAction("Index");
            }

            ViewBag.BranchId = new SelectList(db.Branches, "BranchId", "BranchName", loan.BranchId);
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "CustomerName", loan.CustomerId);
            ViewBag.RepaymentTypeId = new SelectList(db.RepaymentTypes, "RepaymentTypeId", "RepaymentTypeName", loan.RepaymentTypeId);
            return View(loan);
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
}
