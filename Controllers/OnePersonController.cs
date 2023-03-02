using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using E1;
using E1.Models;

namespace E1.Controllers
{
    public class OnePersonController : Controller
    {
        private EEntities db = new EEntities();
        // GET: OnePerson
        public ActionResult Index(string CustomerId)
        {
            string DatabaseName = "Z";
            DatabaseName = db.Database.Connection.Database;
            Session.Add("ApplicationName", DatabaseName);

            var loanCards = new List<LoanCard>();

            if (CustomerId == null)
            {
                loanCards = db.LoanCards.Include(l => l.Loan).Where(m => m.Loan.Customer.CustomerId==(db.Customers.FirstOrDefault().CustomerId)).ToList();

            }
            else
            {
                int customerId = Convert.ToInt32(CustomerId);
                loanCards = db.LoanCards.Include(l => l.Loan).Where(m => m.Loan.Customer.CustomerId == customerId).ToList();

            }
            var loanCardsViewModel = new List<LoanCardViewModel>();

            foreach (var item in loanCards)
            {
                var loancardVM = new LoanCardViewModel();
                loancardVM.IsCollected = (item.IsCollected==null)?false:true;
                loancardVM.PlannedCollectionDate = item.PlannedCollectionDate;
                loancardVM.OpeningBalance = item.OpeningBalance;
                loancardVM.ImpactTransValue = item.ImpactTransValue;
                loancardVM.ClosingBalance = item.ClosingBalance;

                loancardVM.LoanCardIdString = item.LoanCardId.ToString();
                loanCardsViewModel.Add(loancardVM);
            }

            return View(loanCardsViewModel);
        }

        public JsonResult UpdateLoanCard(string CustomerId)
        {
            string DatabaseName = "Z";
            DatabaseName = db.Database.Connection.Database;
            Session.Add("ApplicationName", DatabaseName);

            var loanCards = new List<LoanCard>();

            if (CustomerId == null)
            {
                loanCards = db.LoanCards.Include(l => l.Loan).Where(m => m.Loan.Customer.CustomerId == (db.Customers.FirstOrDefault().CustomerId)).ToList();

            }
            else
            {
                int customerId = Convert.ToInt32(CustomerId);
                loanCards = db.LoanCards.Include(l => l.Loan).Where(m => m.Loan.Customer.CustomerId == customerId).ToList();


            }

            var loanCardsViewModel = new List<LoanCardViewModel>();

            foreach (var item in loanCards)
            {
                var loancardVM = new LoanCardViewModel();
                loancardVM.LoanCardId = item.LoanCardId;
                loancardVM.IsCollected = item.IsCollected;
                loancardVM.PlannedCollectionDate = item.PlannedCollectionDate;
                loancardVM.OpeningBalance = item.OpeningBalance;
                loancardVM.ImpactTransValue = item.ImpactTransValue;
                loancardVM.ClosingBalance = item.ClosingBalance;
                loancardVM.LoanCardIdString = item.LoanCardId.ToString();
                loanCardsViewModel.Add(loancardVM);
            }

            return Json(loanCardsViewModel, JsonRequestBehavior.AllowGet);

        }

        public JsonResult UpdateLoanCardIdStatus(List<LoanCardArray> loancards)
        {
            foreach (var item in loancards)
            {
                var loanCard = db.LoanCards.Where(m => m.LoanCardId == item.loanCardId).FirstOrDefault();
                loanCard.IsCollected = item.status;
                db.SaveChanges();

            }
            
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateAllLoanCardIdStatus(List<LoanCardArray> loancards)
        {
            int CustomerId = loancards[0].loanCardId;
            var loanCardList = db.LoanCards.Include(l => l.Loan).Where(m => m.Loan.Customer.CustomerId == CustomerId).ToList();
            foreach (var loanCard in loanCardList)
            {
                if (loanCard.PlannedCollectionDate <= DateTime.Now.Date)
                {
                    loanCard.IsCollected = loancards[0].status;
                    db.SaveChanges();
                }

            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SearchCustomer(string Prefix)
        {
            
            //Searching records from list using LINQ query  
            var Name = (from N in db.Customers
                        where ( N.CustomerName.ToLower().Contains(Prefix.ToLower()))
                        select new { CustomerName=N.CustomerName
                                    ,CustomerId=N.CustomerId
                                    ,GuardianName = N.GuardianName
                                    ,BranchName=N.Branch.BranchName
                                    ,ContactNumber= N.ContactNumber});
            return Json(Name, JsonRequestBehavior.AllowGet);
        }

        // GET: OnePerson/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: OnePerson/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OnePerson/Create
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

        // GET: OnePerson/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OnePerson/Edit/5
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

        // GET: OnePerson/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OnePerson/Delete/5
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

    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }

    public class LoanCardArray
    {
        public int loanCardId { get; set; }
        public bool status { get; set; }
    }
}
