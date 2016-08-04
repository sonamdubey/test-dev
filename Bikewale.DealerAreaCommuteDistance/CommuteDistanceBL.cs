using BikewaleOpr.Entities;
using Consumer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;

namespace Bikewale.DealerAreaCommuteDistance
{
    public class CommuteDistanceBL
    {

        internal bool UpdateCommuteDistances()
        {
            bool isSuccess = false;
            try
            {
                CommuteDistanceRepository commuteDistance = new CommuteDistanceRepository();
                IEnumerable<DealerEntity> dealers = commuteDistance.FetchActiveContractDealer();
                if (dealers != null && dealers.Count() > 0)
                {
                    foreach (var dealer in dealers)
                    {
                        Logs.WriteInfoLog(String.Format("Begin Commute Distance update for dealerId : {0}", dealer.Id));
                        isSuccess = UpdateCommuteDistance(dealer.Id, dealer.LeadServingRadius);
                        Logs.WriteInfoLog(String.Format("End Commute Distance update for dealerId : {0}", dealer.Id));
                    }
                }
                else
                {
                    Logs.WriteInfoLog("No active contracted dealers present.");
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("UpdateCommuteDistances " + ex.Message);
            }
            return isSuccess;
        }

        private bool UpdateCommuteDistance(UInt16 dealerId, UInt16 leadServingDistance)
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
                    areasBatch = areas.Take(maxArea);
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
                                Logs.WriteInfoLog(String.Format("Distance updated for batch no. {0} dealerID : {1} ", (batchCounter + 1), dealer.Id));
                            }
                            else
                            {
                                Logs.WriteInfoLog(String.Format("Distance updated FAILED for batch no. {0} dealerID : {1} ", (batchCounter + 1), dealer.Id));
                            }
                        }
                        else
                        {
                            Logs.WriteInfoLog(String.Format("failed to get Google API responce for dealerID: {0} and Batchcount {1}", dealer.Id, (batchCounter + 1)));
                        }
                        ++batchCounter;
                        areasBatch = areas.Skip(maxArea * batchCounter).Take(maxArea);
                        Thread.Sleep(500);
                    } while (areasBatch.Count() > 0);
                    Logs.WriteInfoLog(String.Format("update for dealerID {0}", dealer.Id));
                }
                else
                {
                    Logs.WriteInfoLog(String.Format("Failed to get area for dealerId : {0}", dealer.Id));
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("UpdateCommuteDistance " + ex.Message);
                return false;
            }
            return true;
        }
    }
}
