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
    public class LeadersController : Controller
    {
        private EEntities db = new EEntities();

        // GET: Leaders
        public ActionResult Index()
        {
            return View(db.Leaders.ToList());
        }

        // GET: Leaders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Leader leader = db.Leaders.Find(id);
            if (leader == null)
            {
                return HttpNotFound();
            }
            return View(leader);
        }

        // GET: Leaders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Leaders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LeaderId,LeaderName,MobileNumber")] Leader leader)
        {
            if (ModelState.IsValid)
            {
                db.Leaders.Add(leader);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(leader);
        }

        // GET: Leaders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Leader leader = db.Leaders.Find(id);
            if (leader == null)
            {
                return HttpNotFound();
            }
            return View(leader);
        }

        // POST: Leaders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LeaderId,LeaderName,MobileNumber")] Leader leader)
        {
            if (ModelState.IsValid)
            {
                db.Entry(leader).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(leader);
        }

        // GET: Leaders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Leader leader = db.Leaders.Find(id);
            if (leader == null)
            {
                return HttpNotFound();
            }
            return View(leader);
        }

        // POST: Leaders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Leader leader = db.Leaders.Find(id);
            db.Leaders.Remove(leader);
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
