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
    public class DbInfoNamesController : Controller
    {
        private EEntities db = new EEntities();

        // GET: DbInfoNames
        public ActionResult Index()
        {
            return View(db.DbInfoNames.ToList());
        }

        // GET: DbInfoNames/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DbInfoName dbInfoName = db.DbInfoNames.Find(id);
            if (dbInfoName == null)
            {
                return HttpNotFound();
            }
            return View(dbInfoName);
        }

        // GET: DbInfoNames/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DbInfoNames/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DbInfoNameId,DbInfoName1")] DbInfoName dbInfoName)
        {
            if (ModelState.IsValid)
            {
                db.DbInfoNames.Add(dbInfoName);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dbInfoName);
        }

        // GET: DbInfoNames/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DbInfoName dbInfoName = db.DbInfoNames.Find(id);
            if (dbInfoName == null)
            {
                return HttpNotFound();
            }
            return View(dbInfoName);
        }

        // POST: DbInfoNames/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DbInfoNameId,DbInfoName1")] DbInfoName dbInfoName)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(dbInfoName).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(dbInfoName);
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }

        // GET: DbInfoNames/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DbInfoName dbInfoName = db.DbInfoNames.Find(id);
            if (dbInfoName == null)
            {
                return HttpNotFound();
            }
            return View(dbInfoName);
        }

        // POST: DbInfoNames/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DbInfoName dbInfoName = db.DbInfoNames.Find(id);
            db.DbInfoNames.Remove(dbInfoName);
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
