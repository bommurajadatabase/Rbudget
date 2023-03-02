using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E1.Models
{
    public class LoanCardViewModel
    {
      
        public int LoanCardId { get; set; }
        public Nullable<System.DateTime> PlannedCollectionDate { get; set; }
        public Nullable<decimal> OpeningBalance { get; set; }
        public Nullable<decimal> ImpactTransValue { get; set; }
      
        public Nullable<decimal> ClosingBalance { get; set; }
        public Nullable<bool> IsCollected { get; set; }
        public string LoanCardIdString { get; set; }

        public string BranchName { get; set; }
    }
}