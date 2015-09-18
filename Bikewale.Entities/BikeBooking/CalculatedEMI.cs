using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Entities.BikeBooking
{
    /// <summary>
    /// Written by Ashwini Todkar on 29 Oct 2014
    /// </summary>
    public class CalculatedEMI
    {
        public EMI objEMI { get; set; }
        public UInt32 DownPayment { get; set; }
        public double EMI { get; set; }
        public UInt32 LoanAmount { get; set; }
    }
}