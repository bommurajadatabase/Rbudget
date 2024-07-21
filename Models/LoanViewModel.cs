using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E1.Models
{
    public class LoanViewModel
    {
        public LoanViewModel()
        {
            this.LoanDocuments = new HashSet<LoanDocument>();
            this.LoanCards = new HashSet<LoanCard>();
        }

        public int LoanId { get; set; }
        public Nullable<int> BranchId { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public Nullable<System.DateTime> DistributdedDate { get; set; }
        public Nullable<System.DateTime> FirstDueDate { get; set; }
        public Nullable<decimal> OpeningBalance { get; set; }
        public Nullable<decimal> InterestPercentage { get; set; }
        public Nullable<decimal> EMIAmount { get; set; }
        //public Nullable<int> StartDueNumber { get; set; }
        public Nullable<decimal> NoOfDues { get; set; }
        public Nullable<int> RepaymentTypeId { get; set; }

        public virtual Branch Branch { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<LoanDocument> LoanDocuments { get; set; }
        public virtual RepaymentType RepaymentType { get; set; }
        public virtual ICollection<LoanCard> LoanCards { get; set; }

      
    }
}