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
    public class DbInfoStatusController : Controller
    {
        private EEntities db = new EEntities();

        // GET: DbInfoStatus
        public ActionResult Index(string InfoTypeId, string InfoPeriodId, string allData)
        {
            string DatabaseName = "Z";
            DatabaseName = db.Database.Connection.Database;
            Session.Add("ApplicationName", DatabaseName);
            //Dropdown1
            //var ddlDbInfoTypes = new List<SelectListItem>();
            //foreach (var item in db.DbInfoTypes)
            //{
            //    var infoTypeID = 1;
            //    if(InfoTypeId != null)
            //    {
            //        infoTypeID = Convert.ToInt32(InfoTypeId);
            //    }
            //    if (infoTypeID == item.DbInfoTypeId)
            //    {
            //        ddlDbInfoTypes.Add(new SelectListItem
            //        {
            //            Text = item.DbInfoType1,
            //            Value = item.DbInfoTypeId.ToString(),
            //            Selected = true
            //        });
            //    }
            //    else
            //    {
            //        ddlDbInfoTypes.Add(new SelectListItem
            //        {
            //            Text = item.DbInfoType1,
            //            Value = item.DbInfoTypeId.ToString()
            //        });
            //    }

       
            //}
            //ViewBag.ddlDbInfoTypes = ddlDbInfoTypes;
            //Dropdown2
            //var ddlDbInfoPeriods = new List<SelectListItem>();
            //foreach (var item in db.DbInfoPeriods)
            //{
            //    var infoPeriodId = 1;
            //    if (InfoPeriodId != null)
            //    {
            //        infoPeriodId = Convert.ToInt32(InfoPeriodId);
            //    }
            //    if (infoPeriodId == item.DbInfoPeriodId)
            //    {
            //        ddlDbInfoPeriods.Add(new SelectListItem
            //        {
            //            Text = item.DbInfoPeriod1,
            //            Value = item.DbInfoPeriodId.ToString(),
            //            Selected = true
            //        });
            //    }
            //    else
            //    {
            //        ddlDbInfoPeriods.Add(new SelectListItem
            //        {
            //            Text = item.DbInfoPeriod1,
            //            Value = item.DbInfoPeriodId.ToString()
            //        });
            //    }


            //}
            //ViewBag.ddlDbInfoPeriods = ddlDbInfoPeriods;
            //
            string DbInfoType1 = "pending";
            string DbInfoPeriod1 = "jan-2024";
            bool AllData = false;
            if(InfoTypeId != null)
            {
                int infoTypeId = Convert.ToInt32(InfoTypeId);
                DbInfoType1 = db.DbInfoTypes.Where(n => n.DbInfoTypeId == infoTypeId).SingleOrDefault().DbInfoType1.ToString();
            }
            if (InfoPeriodId != null)
            {
                int infoPeriodId = Convert.ToInt32(InfoPeriodId);
                DbInfoPeriod1 = db.DbInfoPeriods.Where(n => n.DbInfoPeriodId == infoPeriodId).SingleOrDefault().DbInfoPeriod1.ToString();
            }
          
            var dbInfoStatus = db.DbInfoStatus.Include(d => d.Loan).Include(d => d.DbInfoPeriod).Include(d => d.DbInfoType).Where(m=>m.DbInfoType.DbInfoType1.ToLower()== DbInfoType1.ToLower()&& m.DbInfoPeriod.DbInfoPeriod1.ToLower()== DbInfoPeriod1.ToLower()&& m.DbInfoStatus== false);

            if (allData != null)
            {
                AllData = Convert.ToBoolean(allData);
                if (AllData)
                {
                    dbInfoStatus = db.DbInfoStatus.Include(d => d.Loan).Include(d => d.DbInfoPeriod).Include(d => d.DbInfoType).Where(m => m.DbInfoType.DbInfoType1.ToLower() == DbInfoType1.ToLower() && m.DbInfoPeriod.DbInfoPeriod1.ToLower() == DbInfoPeriod1.ToLower());
                  
                }
            }

            //var asdf = dbInfoStatus.ToList().Count;
            ViewBag.chkAllData = AllData;
            return View(dbInfoStatus.ToList());
        }

        public JsonResult UpdateDbInfoStatus(List<DbInfoStatusArray> dbInfoStatus)
        {
            foreach (var item in dbInfoStatus)
            {
                var dbinfoStatus = db.DbInfoStatus.Where(m => m.DbInfoStatusId == item.DbInfoStatusId).FirstOrDefault();
                dbinfoStatus.DbInfoStatus = !dbinfoStatus.DbInfoStatus;
                db.SaveChanges();
               

            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        // GET: DbInfoStatus/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DbInfoStatu dbInfoStatu = db.DbInfoStatus.Find(id);
            if (dbInfoStatu == null)
            {
                return HttpNotFound();
            }
            return View(dbInfoStatu);
        }

        // GET: DbInfoStatus/Create
        public ActionResult Create()
        {
            ViewBag.DbInfoNameId = new SelectList(db.DbInfoNames, "DbInfoNameId", "DbInfoName1");
            ViewBag.DbInfoPeriodId = new SelectList(db.DbInfoPeriods, "DbInfoPeriodId", "DbInfoPeriod1");
            ViewBag.DbInfoTypeId = new SelectList(db.DbInfoTypes, "DbInfoTypeId", "DbInfoType1");
            return View();
        }

        // POST: DbInfoStatus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DbInfoStatusId,DbInfoTypeId,DbInfoNameId,DbInfoPeriodId,DbInfoStatus")] DbInfoStatu dbInfoStatu)
        {
            if (ModelState.IsValid)
            {
                db.DbInfoStatus.Add(dbInfoStatu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DbInfoNameId = new SelectList(db.DbInfoNames, "DbInfoNameId", "DbInfoName1", dbInfoStatu.DbInfoLoanId);
            ViewBag.DbInfoPeriodId = new SelectList(db.DbInfoPeriods, "DbInfoPeriodId", "DbInfoPeriod1", dbInfoStatu.DbInfoPeriodId);
            ViewBag.DbInfoTypeId = new SelectList(db.DbInfoTypes, "DbInfoTypeId", "DbInfoType1", dbInfoStatu.DbInfoTypeId);
            return View(dbInfoStatu);
        }

        // GET: DbInfoStatus/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DbInfoStatu dbInfoStatu = db.DbInfoStatus.Find(id);
            if (dbInfoStatu == null)
            {
                return HttpNotFound();
            }
            ViewBag.DbInfoNameId = new SelectList(db.DbInfoNames, "DbInfoNameId", "DbInfoName1", dbInfoStatu.DbInfoLoanId);
            ViewBag.DbInfoPeriodId = new SelectList(db.DbInfoPeriods, "DbInfoPeriodId", "DbInfoPeriod1", dbInfoStatu.DbInfoPeriodId);
            ViewBag.DbInfoTypeId = new SelectList(db.DbInfoTypes, "DbInfoTypeId", "DbInfoType1", dbInfoStatu.DbInfoTypeId);
            return View(dbInfoStatu);
        }

        // POST: DbInfoStatus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DbInfoStatusId,DbInfoTypeId,DbInfoNameId,DbInfoPeriodId,DbInfoStatus")] DbInfoStatu dbInfoStatu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dbInfoStatu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DbInfoNameId = new SelectList(db.DbInfoNames, "DbInfoNameId", "DbInfoName1", dbInfoStatu.DbInfoLoanId);
            ViewBag.DbInfoPeriodId = new SelectList(db.DbInfoPeriods, "DbInfoPeriodId", "DbInfoPeriod1", dbInfoStatu.DbInfoPeriodId);
            ViewBag.DbInfoTypeId = new SelectList(db.DbInfoTypes, "DbInfoTypeId", "DbInfoType1", dbInfoStatu.DbInfoTypeId);
            return View(dbInfoStatu);
        }

        // GET: DbInfoStatus/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DbInfoStatu dbInfoStatu = db.DbInfoStatus.Find(id);
            if (dbInfoStatu == null)
            {
                return HttpNotFound();
            }
            return View(dbInfoStatu);
        }

        // POST: DbInfoStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            DbInfoStatu dbInfoStatu = db.DbInfoStatus.Find(id);
            db.DbInfoStatus.Remove(dbInfoStatu);
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

public class DbInfoStatusArray
{
    public int DbInfoStatusId { get; set; }
    public bool status { get; set; }
}
