using Carwale.DAL.Forums;
using Carwale.Interfaces.Forums;
using Carwale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.BL.Forums
{
    public class ThreadsBL : IThreadsBL
    {
        /// <summary>
        /// Calls DeleteThread in ThreadsDAL. if no exception there, then publishes to queue.
        /// </summary>
        /// <param name="threadId"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public bool DeleteThread(string threadId, string customerId)
        {
            ThreadsDAL dal = new ThreadsDAL();
            if (dal.DeleteThreadFromDB(threadId, customerId))
            { 
                CreateNVC publishToRMQ = new CreateNVC();
                try
                {
                    publishToRMQ.UpdateLuceneIndex(threadId, "0");
                }
                catch (Exception err)
                {
                    ErrorClass objErr = new ErrorClass(err, "Thread BL-delete thread method - Error");
                    objErr.SendMail();
                }
                return true;
            }
            return false;
        }
    }
}
