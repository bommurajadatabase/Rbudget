using System;
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
            if (ModelState.IsValid)
            {
                var loan = new Loan
                {
                    LoanId = loanVM.LoanId,
                    LoanName = loanVM.LoanName,
                    BranchId = loanVM.BranchId,
                    CustomerId = loanVM.CustomerId,
                    DistributdedAmount = loanVM.DistributdedAmount,
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

                // Loancard automatically insert logic

                var NoOfDues = loanVM.NoOfDues;
                var collectionDate = loanVM.FirstDueDate;
                var openingBalance = loanVM.OpeningBalance;
                var NonImpactTransValue = Convert.ToDecimal(0);
                if (loanVM.InterestPercentage > 0)
                {
                    NonImpactTransValue = Convert.ToDecimal(openingBalance * (loanVM.InterestPercentage / 100));
                }
                var LogicSign = 1;
                var EMISign = 1;
              

                var StartDueNumber = 1;
                if(loanVM.StartDueNumber>1)
                {
                    StartDueNumber=loanVM.StartDueNumber;
                }

                for (int i = StartDueNumber; i <= NoOfDues; i++)
                {
                    var loancard = new LoanCard();
                    if (loanVM.InterestType)
                    {
                        NonImpactTransValue = Convert.ToDecimal(openingBalance * (loanVM.InterestPercentage / 100));
                        if (NonImpactTransValue < 0)
                        {
                            LogicSign = -1;
                            EMISign = 1;
                        }
                        else
                        {
                            LogicSign = 1;
                            EMISign = -1;

                        }
                        //if(NonImpactTransValue== loanVM.EMIAmount)
                        //{
                        //    LogicSign = -1;
                        //    EMISign = -1;
                        //}
                            loancard.ImpactTransValue = (loanVM.EMIAmount - ( NonImpactTransValue * LogicSign));
                        loancard.NonImpactTransValue = NonImpactTransValue * LogicSign;

                        //logic
                        loancard.PlannedCollectionDate = collectionDate;
                        //logic
                        loancard.OpeningBalance = openingBalance;
                      
                        //logic
                        if (LogicSign == 1)
                        {
                            loancard.ClosingBalance = loancard.OpeningBalance - loancard.ImpactTransValue;
                        }
                        if (LogicSign == -1)
                        {
                            loancard.ClosingBalance = loancard.OpeningBalance + loancard.ImpactTransValue;
                        }

                        loancard.ImpactTransValue = loancard.ImpactTransValue * EMISign;
                        loancard.NonImpactTransValue = loancard.NonImpactTransValue * EMISign;
                    }
                    else
                    {
                        if (openingBalance != 1 && openingBalance > 1)
                        {
                            NonImpactTransValue = Convert.ToDecimal(openingBalance * (loanVM.InterestPercentage / 100));
                            if (NonImpactTransValue < 0)
                            {
                                LogicSign = -1;
                            }
                            loancard.ImpactTransValue = (loanVM.EMIAmount + (NonImpactTransValue * LogicSign));
                            loancard.NonImpactTransValue = NonImpactTransValue * LogicSign;

                            //logic
                            loancard.PlannedCollectionDate = collectionDate;
                            //logic
                            loancard.OpeningBalance = openingBalance;

                            //logic
                            if (LogicSign == 1)
                            {
                                loancard.ClosingBalance = loancard.OpeningBalance + loancard.ImpactTransValue;
                            }
                            if (LogicSign == -1)
                            {
                                loancard.ClosingBalance = loancard.OpeningBalance - loancard.ImpactTransValue;
                            }
                        }
                        else
                        {
                            loancard.ImpactTransValue = Convert.ToDecimal(openingBalance * (loanVM.InterestPercentage / 100));
                            if (loancard.ImpactTransValue <= 0)
                            {
                                LogicSign = -1;
                            }
                            loancard.NonImpactTransValue = (loanVM.EMIAmount + (loancard.ImpactTransValue * LogicSign));
                            loancard.NonImpactTransValue = loancard.NonImpactTransValue * LogicSign;

                            //logic
                            loancard.PlannedCollectionDate = collectionDate;
                            //logic
                            loancard.OpeningBalance = openingBalance;

                            //logic
                            if (LogicSign == 1)
                            {
                                //loancard.ClosingBalance = loancard.OpeningBalance + loancard.ImpactTransValue;
                                loancard.ClosingBalance = loancard.OpeningBalance + loanVM.EMIAmount;
                            }
                            if (LogicSign == -1)
                            {
                                loancard.ClosingBalance = loancard.OpeningBalance - loancard.ImpactTransValue;
                            }
                        }
                    }
                    
                    loancard.LoanId = loanVM.LoanId;
                    loancard.LoanCardName = loanVM.LoanName;
                    loancard.DueNumber = i ;
                    
                    //logic
                    var CashFlowSign = (loanVM.CashFlow)?1:-1;
                    
                    loancard.PlannedCashAccount = loanVM.EMIAmount * CashFlowSign;
                    //logic
                    loancard.ActualCashAccount = loanVM.EMIAmount * CashFlowSign;
                    //logic
                    if (openingBalance != 1)
                    {
                        loancard.PlannedProfitAccount = NonImpactTransValue;
                        //logic
                        loancard.ActualProfitAccount = NonImpactTransValue;
                    }
                    else
                    {
                        //Logic
                        loancard.PlannedProfitAccount = loancard.NonImpactTransValue * CashFlowSign;
                        //logic
                        loancard.ActualProfitAccount = loancard.NonImpactTransValue * CashFlowSign;
                    }
                    loancard.ActualCollectionDate = collectionDate;
                    loancard.IsCollected = false;

                    db.LoanCards.Add(loancard);
                    db.SaveChanges();
                    var RepaymentType = db.RepaymentTypes.Where(m=>m.RepaymentTypeId==loanVM.RepaymentTypeId).FirstOrDefault();
                    //logic
                    if ((RepaymentType.IsWeekBased != null) ? Convert.ToBoolean(RepaymentType.IsWeekBased) : false)
                    {
                        collectionDate = collectionDate.Value.AddDays(7);

                    }
                    else if ((RepaymentType.IsMonthBased != null) ? Convert.ToBoolean(RepaymentType.IsMonthBased) : false)
                    {
                        collectionDate = collectionDate.Value.AddMonths(1);

                    }
                    else 
                    {
                        collectionDate = collectionDate.Value.AddDays(7);

                    }
                    //logic
                    openingBalance = loancard.ClosingBalance;

                }
              





                return RedirectToAction("Index");
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
}
