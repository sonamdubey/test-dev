using BikewaleOpr.Entities;
using BikeWaleOpr.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace BikewaleOpr.CommuteDistance
{
    /// <summary>
    /// Created By  :   Sumit Kate on 18 Apr 2016
    /// Description :   Commute Distance Business Layer
    /// </summary>
    public class CommuteDistanceBL
    {
        private String _taskprogress;
        private AsyncTaskDelegate _dlgt;
        public UInt16 DealerID { get; set; }
        public UInt16 LeadServingDistance { get; set; }

        /// <summary>
        /// Call back delegate
        /// </summary>
        protected delegate void AsyncTaskDelegate();

        /// <summary>
        /// Task progress string
        /// </summary>
        public String TaskProgress
        {
            get
            {
                return _taskprogress;
            }
        }

        /// <summary>
        /// Calls the heavy task
        /// </summary>
        public void DoTheAsyncTask()
        {
            UpdateCommuteDistance(DealerID, LeadServingDistance);
        }

        /// <summary>        
        /// Define the method that will get called to
        /// start the asynchronous task.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="cb"></param>
        /// <param name="extraData"></param>
        /// <returns></returns>
        public IAsyncResult OnBegin(object sender, EventArgs e,
            AsyncCallback cb, object extraData)
        {
            _taskprogress = "Beginning async task.";

            _dlgt = new AsyncTaskDelegate(DoTheAsyncTask);
            IAsyncResult result = _dlgt.BeginInvoke(cb, extraData);

            return result;
        }

        /// <summary>
        /// Define the method that will get called when
        /// the asynchronous task is ended.
        /// </summary>
        /// <param name="ar"></param>
        public void OnEnd(IAsyncResult ar)
        {
            _taskprogress = "Asynchronous task completed.";
            _dlgt.EndInvoke(ar);
        }



        /// <summary>
        /// Define the method that will get called if the task
        /// is not completed within the asynchronous timeout interval.
        /// </summary>
        /// <param name="ar"></param>
        public void OnTimeout(IAsyncResult ar)
        {
            _taskprogress = "Ansynchronous task failed to complete, because it exceeded the AsyncTimeout parameter.";
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 18 Apr 2016
        /// Description :   Calculates the Commute Distance between dealer and areas
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="leadServingDistance"></param>
        /// <returns></returns>
        public bool UpdateCommuteDistance(UInt16 dealerId, UInt16 leadServingDistance)
        {
            CommuteDistanceRepository commuteDistance = new CommuteDistanceRepository();
            GoogleDistanceAPIHelper googleApi = null;
            IEnumerable<GeoLocationDestinationEntity> apiResp = null;
            string formatedResp = string.Empty;
            UInt16 maxArea = Convert.ToUInt16(ConfigurationManager.AppSettings["areaMaxPerCall"]);
            GeoLocationEntity dealer = null;
            try
            {
                IEnumerable<GeoLocationEntity> areas = null, areasBatch = null;
                //get the Areas served by the dealers by lead serving distance(straight line distance calculation)
                areas = commuteDistance.GetAreaByDealer(dealerId, leadServingDistance, out dealer);
                if (dealer != null && (areas != null && areas.Count() > 0))
                {
                    googleApi = new GoogleDistanceAPIHelper();
                    areasBatch = areas.Take(50);
                    int batchCounter = 0;
                    do
                    {
                        //Call the Google API
                        apiResp = googleApi.GetDistanceUsingArrayIndex(dealer, areasBatch, false);
                        if (apiResp != null && apiResp.Count() > 0)
                        {
                            formatedResp = googleApi.FormatApiResp(apiResp);
                            //update the areas with Commute Distance
                            if (commuteDistance.UpdateArea(dealer.Id, formatedResp))
                            {
                                Debug.WriteLine(String.Format("Distance updated for batch no. {0} dealerID : {1} ", (batchCounter + 1), dealer.Id));
                            }
                            else
                            {
                                Debug.WriteLine(String.Format("Distance updated FAILED for batch no. {0} dealerID : {1} ", (batchCounter + 1), dealer.Id));
                            }
                        }
                        else
                        {
                            Debug.WriteLine(String.Format("failed to get Google API responce for dealerID: {0} and Batchcount {1}", dealer.Id, (batchCounter + 1)));
                        }
                        ++batchCounter;
                        areasBatch = areas.Skip(50 * batchCounter).Take(50);
                        Thread.Sleep(500);
                    } while (areasBatch.Count() > 0);
                    Debug.WriteLine(String.Format("update for dealerID {0}", dealer.Id));
                }
                else
                {
                    Debug.WriteLine(String.Format("Failed to get area for dealerId : {0}", dealer.Id));
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "UpdateCommuteDistance");
                objErr.SendMail();
                return false;
            }
            return true;
        }


    }
}