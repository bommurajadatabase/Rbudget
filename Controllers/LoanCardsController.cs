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
    public class LoanCardsController : Controller
    {
        private EEntities db = new EEntities();


        // GET: LoanCards
        public ActionResult Index(DateTime? LoanCardFromDate, DateTime? LoanCardToDate,string statusradio,string HdnLoanId,bool? isActive)
        {
            isActive = (isActive == null) ? true : isActive;
            string DatabaseName = "Z";
            DatabaseName = db.Database.Connection.Database;
            Session.Add("ApplicationName", DatabaseName);
            var loanCards = new List<LoanCard>();
            if (HdnLoanId == null || HdnLoanId == "")
            {
                ViewBag.statusradioallcount = 0;
                ViewBag.statusradiopendingcount = 0;
                ViewBag.statusradiocompletedcount = 0;

                if (LoanCardFromDate == null)
                {
                    DateTime now = DateTime.Now;

                    switch (db.Database.Connection.Database)
                    {

                        case "E":
                            LoanCardFromDate = DateTime.Now.Date;
                            LoanCardToDate = Convert.ToDateTime(LoanCardFromDate).AddDays(6).Date;
                            break;

                        case "R":
                            LoanCardFromDate = new DateTime(now.Year, now.Month, 1).Date;
                            LoanCardToDate = Convert.ToDateTime(LoanCardFromDate).AddMonths(1).AddDays(-1).Date;
                            break;
                        case "S":
                            LoanCardFromDate = new DateTime(now.Year, now.Month, 1).Date;
                            LoanCardToDate = Convert.ToDateTime(LoanCardFromDate).AddMonths(1).AddDays(-1).Date;
                            break;
                        case "T":
                            LoanCardFromDate = new DateTime(now.Year, now.Month, 1).Date;
                            LoanCardToDate = Convert.ToDateTime(LoanCardFromDate).AddMonths(1).AddDays(-1).Date;
                            break;
                        case "Ro":
                            LoanCardFromDate = new DateTime(now.Year, now.Month, 1).Date;
                            LoanCardToDate = Convert.ToDateTime(LoanCardFromDate).AddMonths(1).AddDays(-1).Date;
                            break;
                        case "ts1":
                            LoanCardFromDate = new DateTime(now.Year, now.Month, 1).Date;
                            LoanCardToDate = Convert.ToDateTime(LoanCardFromDate).AddMonths(1).AddDays(-1).Date;
                            break;
                        case "ts2":
                            LoanCardFromDate = new DateTime(now.Year, now.Month, 1).Date;
                            LoanCardToDate = Convert.ToDateTime(LoanCardFromDate).AddMonths(1).AddDays(-1).Date;
                            break;
                        case "us1":
                            LoanCardFromDate = new DateTime(now.Year, now.Month, 1).Date;
                            LoanCardToDate = Convert.ToDateTime(LoanCardFromDate).AddMonths(1).AddDays(-1).Date;
                            break;
                        case "us2":
                            LoanCardFromDate = new DateTime(now.Year, now.Month, 1).Date;
                            LoanCardToDate = Convert.ToDateTime(LoanCardFromDate).AddMonths(1).AddDays(-1).Date;
                            break;
                        case "vs1":
                            LoanCardFromDate = new DateTime(now.Year, now.Month, 1).Date;
                            LoanCardToDate = Convert.ToDateTime(LoanCardFromDate).AddMonths(1).AddDays(-1).Date;
                            break;
                        case "vs2":
                            LoanCardFromDate = new DateTime(now.Year, now.Month, 1).Date;
                            LoanCardToDate = Convert.ToDateTime(LoanCardFromDate).AddMonths(1).AddDays(-1).Date;
                            break;
                        default:
                            LoanCardFromDate = DateTime.Now.Date;
                            LoanCardToDate = Convert.ToDateTime(LoanCardFromDate).AddDays(6).Date;
                            break;
                    }
                    if ((bool)isActive)
                    {
                        loanCards = db.LoanCards.Include(l => l.Loan).Where(m => m.PlannedCollectionDate >= LoanCardFromDate && m.PlannedCollectionDate <= LoanCardToDate && m.IsActive == isActive).ToList();
                    }
                    else
                    {
                        loanCards = db.LoanCards.Include(l => l.Loan).Where(m => m.PlannedCollectionDate >= LoanCardFromDate && m.PlannedCollectionDate <= LoanCardToDate).ToList();
                    }


                    ViewBag.statusradioallcount = loanCards.ToList().Count;
                    ViewBag.statusradiopendingcount = loanCards.Where(m => m.IsCollected == false).ToList().Count;
                    ViewBag.statusradiocompletedcount = loanCards.Where(m => m.IsCollected == true).ToList().Count;

                    if (statusradio == "pending")
                    {
                        loanCards = loanCards.Where(m => m.IsCollected == false).ToList();
                    }
                    if (statusradio == "completed")
                    {
                        loanCards = loanCards.Where(m => m.IsCollected == true).ToList();

                    }

                }
                else
                {
                    if ((bool)isActive)
                    {

                        loanCards = db.LoanCards.Include(l => l.Loan).Where(m => m.PlannedCollectionDate >= LoanCardFromDate && m.PlannedCollectionDate <= LoanCardToDate && m.IsActive == isActive).ToList();
                    }
                    else
                    {
                        loanCards = db.LoanCards.Include(l => l.Loan).Where(m => m.PlannedCollectionDate >= LoanCardFromDate && m.PlannedCollectionDate <= LoanCardToDate ).ToList();

                    }

                    ViewBag.statusradioallcount = loanCards.ToList().Count;
                    ViewBag.statusradiopendingcount = loanCards.Where(m => m.IsCollected == false).ToList().Count;
                    ViewBag.statusradiocompletedcount = loanCards.Where(m => m.IsCollected == true).ToList().Count;

                    if (statusradio == "pending")
                    {
                        loanCards = loanCards.Where(m => m.IsCollected == false).ToList();
                    }
                    if (statusradio == "completed")
                    {
                        loanCards = loanCards.Where(m => m.IsCollected == true).ToList();

                    }
                }

                ViewBag.statusradio = statusradio;
                ViewBag.loanCardFromDate = Convert.ToDateTime(LoanCardFromDate).ToString("dd-MMM-yyyy"); 
                ViewBag.loanCardToDate = Convert.ToDateTime(LoanCardToDate).ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewBag.statusradioallcount = 0;
                ViewBag.statusradiopendingcount = 0;
                ViewBag.statusradiocompletedcount = 0;
                int hdnLoanId = 0;
                hdnLoanId = Convert.ToInt32(HdnLoanId);
                if ((bool)isActive)
                {
                    loanCards = db.LoanCards.Include(l => l.Loan).Where(m => m.LoanId == hdnLoanId && m.IsActive == isActive).ToList();
                }
                else
                {
                    loanCards = db.LoanCards.Include(l => l.Loan).Where(m => m.LoanId == hdnLoanId ).ToList();

                }
                loanCards = loanCards.OrderBy(m=>m.LoanCardId).ToList();
            }

            //ViewBag.PendingCount = db.DbInfoes.Where(m=>m.DbInfoType.DbInfoType1.ToLower()=="pending" && m.IsCompleted==false).ToList().Count;
            //ViewBag.FollowUpCount = db.DbInfoes.Where(m => m.DbInfoType.DbInfoType1.ToLower() == "followup" && m.IsCompleted == false).ToList().Count; 
            //ViewBag.CardsCount = db.DbInfoes.Where(m => m.DbInfoType.DbInfoType1.ToLower() == "cards" && m.IsCompleted == false).ToList().Count; 
            //ViewBag.EnquiresCount = db.DbInfoes.Where(m => m.DbInfoType.DbInfoType1.ToLower() == "enquires" && m.IsCompleted == false ).ToList().Count; 


            return View(loanCards.ToList());

        }


        [HttpPost]
        public JsonResult SearchOneAccount(string OneAccountName)
        {

            //Searching records from list using LINQ query  
            var LoanCardDetails = (from N in db.Loans
                        where (N.LoanName.ToLower().Contains(OneAccountName.ToLower()))
                        select new
                        {
                            LoanName = N.LoanName
                            ,LoanId = N.LoanId
                        });
            return Json(LoanCardDetails, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteLoanAndItsCard(string LoanId)
        {
            int loanId = Convert.ToInt32(LoanId);
            db.LoanCards.RemoveRange(db.LoanCards.Where(m => m.LoanId == loanId));
            var loan = db.Loans.Where(m => m.LoanId == loanId).FirstOrDefault();
            db.Loans.Remove(loan);
            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        // GET: LoanCards/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoanCard loanCard = db.LoanCards.Find(id);
            if (loanCard == null)
            {
                return HttpNotFound();
            }
            return View(loanCard);
        }

        // GET: LoanCards/Create
        public ActionResult Create()
        {
            ViewBag.LoanId = new SelectList(db.Loans, "LoanId", "LoanName");
            return View();
        }

        // POST: LoanCards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LoanCardId,LoanId,LoanCardName,DueNumber,PlannedCollectionDate,OpeningBalance,ImpactTransValue,NonImpactTransValue,ClosingBalance,PlannedCashAccount,ActualCashAccount,PlannedProfitAccount,ActualProfitAccount,ActualCollectionDate,IsCollected")] LoanCard loanCard)
        {
            if (ModelState.IsValid)
            {
                db.LoanCards.Add(loanCard);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LoanId = new SelectList(db.Loans, "LoanId", "LoanName", loanCard.LoanId) ;
            return View(loanCard);
        }

        // GET: LoanCards/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoanCard loanCard = db.LoanCards.Find(id);
            if (loanCard == null)
            {
                return HttpNotFound();
            }
            ViewBag.LoanId = new SelectList(db.Loans, "LoanId", "LoanName", loanCard.LoanId);
            return View(loanCard);
        }

        // POST: LoanCards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LoanCardId,LoanId,LoanCardName,DueNumber,PlannedCollectionDate,OpeningBalance,ImpactTransValue,NonImpactTransValue,ClosingBalance,PlannedCashAccount,ActualCashAccount,PlannedProfitAccount,ActualProfitAccount,ActualCollectionDate,IsCollected,IsActive")] LoanCard loanCard)
        {
            if (ModelState.IsValid)
            {
                db.Entry(loanCard).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LoanId = new SelectList(db.Loans, "LoanId", "LoanName", loanCard.LoanId);
            return View(loanCard);
        }

        // GET: LoanCards/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoanCard loanCard = db.LoanCards.Find(id);
            if (loanCard == null)
            {
                return HttpNotFound();
            }
            return View(loanCard);
        }

        // POST: LoanCards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LoanCard loanCard = db.LoanCards.Find(id);
            db.LoanCards.Remove(loanCard);
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
