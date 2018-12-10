using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.LeadScore
{
    [Serializable]
    public class LeadScoreParams
    {
        public ulong LeadId { get; set; }
        public int LatestBuyTime { get; set; }
        public int DistinctBuyTime { get; set; }
        public int DistinctBodyStyles { get; set; }
        public int PreviousLeads { get; set; }
        public int PreviousVerified { get; set; }
        public string Cookie { get; set; }       
    }

    public class HBaseParams 
    {
        public string PageViews { get; set; }
        public string Visits { get; set; }
        public string LastVisit { get; set; }
        public string TotTimeSpent { get; set; }
        public double TotTimeSpentGp1 { get; set; }
        public double TimeDiff { get; set; }
    }

    public class RealTimeParams 
    {
        public string PageViews { get; set; }
        public string Visits { get; set; }
        public string LastVisit { get; set; }
        public string TotTimeSpent { get; set; }
        public double TotTimeSpentGp1 { get; set; }
        public double TimeDiff { get; set; }
    }
}
