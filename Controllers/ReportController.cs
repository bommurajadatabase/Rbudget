using Microsoft.Ajax.Utilities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebGrease.Configuration;

namespace E1.Controllers
{
    public class ReportController : Controller
    {
        private EEntities db = new EEntities();
        
        
        // GET: Report
        public ActionResult Index(DateTime? ReportFromDate, DateTime? ReportToDate)
        {
            var reportList = new List<SP_WeeklyReport_Result>();
            if(ReportFromDate == null)
            {
                string DatabaseName = "Z";
                DatabaseName = db.Database.Connection.Database;
                Session.Add("ApplicationName", DatabaseName);
                switch (DatabaseName)
                {

                    case "E": 
                        ReportFromDate = DateTime.Now.Date;
                        ReportToDate = Convert.ToDateTime(ReportFromDate).AddDays(6).Date;
                        break;

                    case "R":
                        DateTime now = DateTime.Now;
                        ReportFromDate = new DateTime(now.Year, now.Month, 1).Date;
                        ReportToDate = Convert.ToDateTime(ReportFromDate).AddMonths(1).AddDays(-1).Date;
                        break;

                    default:
                        ReportFromDate = DateTime.Now.Date;
                        ReportToDate = Convert.ToDateTime(ReportFromDate).AddDays(6).Date;
                        break;
                }
                          
            }
            reportList = db.FunctionWeeklyReport(ReportFromDate, ReportToDate).ToList();


            //ViewBag.OpeningBalanceTotal = reportList.Sum(x => x.OpeningBalance);
            //ViewBag.CollectionAmountTotal = reportList.Sum(x => x.ImpactTransValue);
            //ViewBag.ClosingBalanceTotal = reportList.Sum(x => x.ClosingBalance);
            return View(reportList);
        }

        // GET: Report/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Report/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Report/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Report/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Report/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Report/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Report/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }


}
