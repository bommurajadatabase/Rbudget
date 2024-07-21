using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E1.Controllers
{
    public class HomeController : Controller
    {
        private EEntities db = new EEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Config()
        {
            var dbInfoStatusList = new List<DbInfoStatu>();

            // 1.pending 2. enquiry
            var dbInfoTypeList = db.DbInfoTypes.ToList();
            // 1.RAG-AND 2. SUM-ATHI
            var dbInfoLoanList = db.Loans.ToList();
            // 1. JAN 2. FEB 3. MAR
            var dbInforPeriodId = db.DbInfoPeriods.Where(m => m.DbInfoPeriod1 == "APR-2024").FirstOrDefault().DbInfoPeriodId;
            // 1. DB Status
            var dbInfoStatusId = false;

            var oldDbInfoStatusList = db.DbInfoStatus.Where(m => m.DbInfoPeriodId == dbInforPeriodId).ToList();
            if (oldDbInfoStatusList.Count > 0)
            {
                db.DbInfoStatus.RemoveRange(oldDbInfoStatusList);
                db.SaveChanges();
            }

            foreach (var item1 in dbInfoTypeList)
            {
                foreach (var item2 in dbInfoLoanList)
                {
                    var objDbInfo = new DbInfoStatu();

                    objDbInfo.DbInfoTypeId = item1.DbInfoTypeId;
                    objDbInfo.DbInfoLoanId = item2.LoanId;
                    objDbInfo.DbInfoPeriodId = dbInforPeriodId;
                    objDbInfo.DbInfoStatus = dbInfoStatusId;
                    dbInfoStatusList.Add(objDbInfo);
                }
            }
            for (int i = 0; i < dbInfoStatusList.Count(); i++)
            {
                db.DbInfoStatus.Add(dbInfoStatusList[i]);
            }
            db.SaveChanges();
            return View();
        }
    }
}