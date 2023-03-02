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
        public ActionResult Index(DateTime? LoanCardFromDate, DateTime? LoanCardToDate)
        {
            var loanCards = new List<LoanCard>();

            if (LoanCardFromDate == null)
            {
                switch (db.Database.Connection.Database)
                {

                    case "E":
                        LoanCardFromDate = DateTime.Now.Date;
                        LoanCardToDate = Convert.ToDateTime(LoanCardFromDate).AddDays(6).Date;
                        break;

                    case "R":
                        DateTime now = DateTime.Now;
                        LoanCardFromDate = new DateTime(now.Year, now.Month, 1).Date;
                        LoanCardToDate = Convert.ToDateTime(LoanCardFromDate).AddMonths(1).AddDays(-1).Date;
                        break;

                    default:
                        LoanCardFromDate = DateTime.Now.Date;
                        LoanCardToDate = Convert.ToDateTime(LoanCardFromDate).AddDays(6).Date;
                        break;
                }
                loanCards = db.LoanCards.Include(l => l.Loan).Where(m=>m.PlannedCollectionDate>= LoanCardFromDate && m.PlannedCollectionDate<= LoanCardToDate).ToList();

            }
            else
            {
                loanCards = db.LoanCards.Include(l => l.Loan).Where(m => m.PlannedCollectionDate >= LoanCardFromDate && m.PlannedCollectionDate <= LoanCardToDate).ToList();

            }


            //ViewBag.OpeningBalanceTotal = reportList.Sum(x => x.OpeningBalance);
            //ViewBag.CollectionAmountTotal = reportList.Sum(x => x.ImpactTransValue);
            //ViewBag.ClosingBalanceTotal = reportList.Sum(x => x.ClosingBalance);

            return View(loanCards.ToList());

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

            ViewBag.LoanId = new SelectList(db.Loans, "LoanId", "LoanName", loanCard.LoanId);
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
        public ActionResult Edit([Bind(Include = "LoanCardId,LoanId,LoanCardName,DueNumber,PlannedCollectionDate,OpeningBalance,ImpactTransValue,NonImpactTransValue,ClosingBalance,PlannedCashAccount,ActualCashAccount,PlannedProfitAccount,ActualProfitAccount,ActualCollectionDate,IsCollected")] LoanCard loanCard)
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
