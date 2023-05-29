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
    public class RepaymentTypesController : Controller
    {
        private EEntities db = new EEntities();

        // GET: RepaymentTypes
        public ActionResult Index()
        {
            return View(db.RepaymentTypes.ToList());
        }

        // GET: RepaymentTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RepaymentType repaymentType = db.RepaymentTypes.Find(id);
            if (repaymentType == null)
            {
                return HttpNotFound();
            }
            return View(repaymentType);
        }

        // GET: RepaymentTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RepaymentTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RepaymentTypeId,RepaymentTypeName,IsMonthBased,IsWeekBased,IsDaysBased,DueCycleDays")] RepaymentType repaymentType)
        {
            if (ModelState.IsValid)
            {
                db.RepaymentTypes.Add(repaymentType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(repaymentType);
        }

        // GET: RepaymentTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RepaymentType repaymentType = db.RepaymentTypes.Find(id);
            if (repaymentType == null)
            {
                return HttpNotFound();
            }
            return View(repaymentType);
        }

        // POST: RepaymentTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RepaymentTypeId,RepaymentTypeName,IsMonthBased,IsWeekBased,IsDaysBased,DueCycleDays")] RepaymentType repaymentType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(repaymentType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(repaymentType);
        }

        // GET: RepaymentTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RepaymentType repaymentType = db.RepaymentTypes.Find(id);
            if (repaymentType == null)
            {
                return HttpNotFound();
            }
            return View(repaymentType);
        }

        // POST: RepaymentTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RepaymentType repaymentType = db.RepaymentTypes.Find(id);
            db.RepaymentTypes.Remove(repaymentType);
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
