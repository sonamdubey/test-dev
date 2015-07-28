using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeBookingInquiries
{
    class Program
    {
        static void Main(string[] args)
        {
            PushInquiryToAutoBiz objProcess = new PushInquiryToAutoBiz();
            objProcess.ProcessInquiries();
        }
    }
}
