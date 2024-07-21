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
    public class DbInfoTypesController : Controller
    {
        private EEntities db = new EEntities();

        // GET: DbInfoTypes
        public ActionResult Index()
        {
            return View(db.DbInfoTypes.ToList());
        }

        // GET: DbInfoTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DbInfoType dbInfoType = db.DbInfoTypes.Find(id);
            if (dbInfoType == null)
            {
                return HttpNotFound();
            }
            return View(dbInfoType);
        }

        // GET: DbInfoTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DbInfoTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DbInfoTypeId,DbInfoType1")] DbInfoType dbInfoType)
        {
            if (ModelState.IsValid)
            {
                db.DbInfoTypes.Add(dbInfoType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dbInfoType);
        }

        // GET: DbInfoTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DbInfoType dbInfoType = db.DbInfoTypes.Find(id);
            if (dbInfoType == null)
            {
                return HttpNotFound();
            }
            return View(dbInfoType);
        }

        // POST: DbInfoTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DbInfoTypeId,DbInfoType1")] DbInfoType dbInfoType)
        {
            if (ModelState.IsValid)
            {
                try
                {
                  
                    db.Entry(dbInfoType).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {

                    throw;
                }
              
            }
            return View(dbInfoType);
        }

        // GET: DbInfoTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DbInfoType dbInfoType = db.DbInfoTypes.Find(id);
            if (dbInfoType == null)
            {
                return HttpNotFound();
            }
            return View(dbInfoType);
        }

        // POST: DbInfoTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DbInfoType dbInfoType = db.DbInfoTypes.Find(id);
            db.DbInfoTypes.Remove(dbInfoType);
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
