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
    public class LoanCardsEditController : Controller
    {
        private EEntities db = new EEntities();

        // GET: LoanCardsEdit
        public ActionResult Index()
        {
            var loanCards = db.LoanCards.Include(l => l.Loan);
            return View(loanCards.ToList());
        }

        // GET: LoanCardsEdit/Details/5
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

        // GET: LoanCardsEdit/Create
        public ActionResult Create()
        {
            ViewBag.LoanId = new SelectList(db.Loans, "LoanId", "LoanName");
            return View();
        }

        // POST: LoanCardsEdit/Create
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

        // GET: LoanCardsEdit/Edit/5
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

        // POST: LoanCardsEdit/Edit/5
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

        // GET: LoanCardsEdit/Delete/5
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

        // POST: LoanCardsEdit/Delete/5
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
