using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace BikeBookingInquiries
{
    public static class Logs
    {        
        private static readonly log4net.ILog Infolog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly log4net.ILog Errorlog = LogManager.GetLogger("ErrorLog");
        
        public static void WriteErrorLog(string log)
        {
            Errorlog.Error(log);
        }
    }
}
