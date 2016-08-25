using BikewaleOpr.Entities;
using Consumer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
namespace Bikewale.DealerAreaCommuteDistance
{
    /// <summary>
    /// Created by  :   Sumit Kate on 16 Aug 2016
    /// Description :   Business Layer to manage the commute distance between Dealer and area
    /// </summary>
    public class CommuteDistanceBL
    {
        private readonly CommuteDistanceRepository commuteDistance = null;

        /// <summary>
        /// Constructor to initialize the DAL object
        /// </summary>
        public CommuteDistanceBL()
        {
            commuteDistance = new CommuteDistanceRepository();
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 18 Apr 2016
        /// Description :   Calculates the Commute Distance between dealer and areas while area lat-long is changed
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="leadServingDistance"></param>
        /// <returns></returns>
        internal bool UpdateCommuteDistanceForArea(UInt32 areaId, double lattitude, double longitude)
        {
            GoogleDistanceAPIHelper googleApi = null;
            IEnumerable<GeoLocationDestinationEntity> apiResp = null;
            string formatedResp = string.Empty;
            UInt16 maxArea = Convert.ToUInt16(ConfigurationManager.AppSettings["areaMaxPerCall"]);
            GeoLocationEntity area = null;
            try
            {
                IEnumerable<GeoLocationEntity> dealers = null, dealersBatch = null;
                //get the Areas served by the dealers by lead serving distance(straight line distance calculation)
                dealers = commuteDistance.GetDealersByArea(areaId);
                area = new GeoLocationEntity();
                area.Id = areaId;
                area.Latitude = lattitude;
                area.Longitude = longitude;
                if (area != null && dealers != null && dealers.Count() > 0)
                {
                    googleApi = new GoogleDistanceAPIHelper();
                    dealersBatch = dealers.Take(maxArea);
                    int batchCounter = 0;
                    do
                    {
                        //Call the Google API
                        apiResp = googleApi.GetDistanceUsingArrayIndex(area, dealersBatch);
                        if (apiResp != null && apiResp.Count() > 0)
                        {
                            formatedResp = googleApi.FormatApiResp(apiResp);
                            //update the areas with Commute Distance
                            if (commuteDistance.UpdateDistances(2, areaId, formatedResp))
                            {
                                Logs.WriteInfoLog(String.Format("Distance updated for batch no. {0} dealerID : {1} ", (batchCounter + 1), area.Id));
                            }
                            else
                            {
                                Logs.WriteInfoLog(String.Format("Distance updated FAILED for batch no. {0} dealerID : {1} ", (batchCounter + 1), area.Id));
                            }
                        }
                        else
                        {
                            Logs.WriteInfoLog(String.Format("failed to get Google API responce for dealerID: {0} and Batchcount {1}", area.Id, (batchCounter + 1)));
                        }
                        ++batchCounter;
                        dealersBatch = dealers.Skip(maxArea * batchCounter).Take(maxArea);
                        Thread.Sleep(500);
                    } while (dealersBatch.Count() > 0);
                    Logs.WriteInfoLog(String.Format("update for dealerID {0}", area.Id));
                }
                else
                {
                    Logs.WriteInfoLog(String.Format("Failed to get area for dealerId : {0}", area.Id));
                }
            }
            catch (Exception ex)
            {

                return false;
            }
            return true;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 18 Apr 2016
        /// Description :   Calculates the Commute Distance between dealer and areas while adding new area
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="leadServingDistance"></param>
        /// <returns></returns>
        internal bool UpdateCommuteDistanceForAreaAdd(Int32 cityId, UInt32 areaId, double lattitude, double longitude)
        {
            GoogleDistanceAPIHelper googleApi = null;
            IEnumerable<GeoLocationDestinationEntity> apiResp = null;
            string formatedResp = string.Empty;
            UInt16 maxArea = Convert.ToUInt16(ConfigurationManager.AppSettings["areaMaxPerCall"]);
            GeoLocationEntity area = null;
            try
            {
                IEnumerable<GeoLocationEntity> dealers = null, dealersBatch = null;
                //get the Areas served by the dealers by lead serving distance(straight line distance calculation)
                dealers = commuteDistance.GetCampaignDealerByCity(cityId);
                area = new GeoLocationEntity();
                area.Id = areaId;
                area.Latitude = lattitude;
                area.Longitude = longitude;
                if (area != null && dealers != null && dealers.Count() > 0)
                {
                    googleApi = new GoogleDistanceAPIHelper();
                    dealersBatch = dealers.Take(maxArea);
                    int batchCounter = 0;
                    do
                    {
                        //Call the Google API
                        apiResp = googleApi.GetDistanceUsingArrayIndex(area, dealersBatch);
                        if (apiResp != null && apiResp.Count() > 0)
                        {
                            formatedResp = googleApi.FormatApiResp(apiResp);
                            //update the areas with Commute Distance
                            if (commuteDistance.UpdateDistances(2, areaId, formatedResp))
                            {
                                Logs.WriteInfoLog(String.Format("Distance updated for batch no. {0} dealerID : {1} ", (batchCounter + 1), area.Id));
                            }
                            else
                            {
                                Logs.WriteInfoLog(String.Format("Distance updated FAILED for batch no. {0} dealerID : {1} ", (batchCounter + 1), area.Id));
                            }
                        }
                        else
                        {
                            Logs.WriteInfoLog(String.Format("failed to get Google API responce for dealerID: {0} and Batchcount {1}", area.Id, (batchCounter + 1)));
                        }
                        ++batchCounter;
                        dealersBatch = dealers.Skip(maxArea * batchCounter).Take(maxArea);
                        Thread.Sleep(500);
                    } while (dealersBatch.Count() > 0);
                    Logs.WriteInfoLog(String.Format("update for dealerID {0}", area.Id));
                }
                else
                {
                    Logs.WriteInfoLog(String.Format("Failed to get area for dealerId : {0}", area.Id));
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("UpdateCommuteDistanceForAreaAdd " + ex.Message);
                return false;
            }
            return true;
        }


        /// <summary>
        /// Created by  :   Sumit Kate on 18 Apr 2016
        /// Description :   Calculates the Commute Distance between dealer and areas
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="leadServingDistance"></param>
        /// <returns></returns>
        internal bool UpdateCommuteDistanceForDealerUpdate(UInt32 dealerId, double lattitude, double longitude)
        {
            GoogleDistanceAPIHelper googleApi = null;
            IEnumerable<GeoLocationDestinationEntity> apiResp = null;
            string formatedResp = string.Empty;
            UInt16 maxArea = Convert.ToUInt16(ConfigurationManager.AppSettings["areaMaxPerCall"]);
            GeoLocationEntity dealer = null;
            try
            {
                IEnumerable<GeoLocationEntity> areas = null, areasBatch = null;
                //get the Areas served by the dealers by lead serving distance(straight line distance calculation)
                areas = commuteDistance.GetAreasByDealer(dealerId);
                dealer = new GeoLocationEntity();
                dealer.Id = dealerId;
                dealer.Latitude = lattitude;
                dealer.Longitude = longitude;
                if (dealer != null && areas != null && areas.Count() > 0)
                {
                    googleApi = new GoogleDistanceAPIHelper();
                    areasBatch = areas.Take(maxArea);
                    int batchCounter = 0;
                    do
                    {
                        //Call the Google API
                        apiResp = googleApi.GetDistanceUsingArrayIndex(dealer, areasBatch);
                        if (apiResp != null && apiResp.Count() > 0)
                        {
                            formatedResp = googleApi.FormatApiResp(apiResp);
                            //update the areas with Commute Distance
                            if (commuteDistance.UpdateDistances(1, dealer.Id, formatedResp))
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
                }
                else
                {

                }
            }
            catch (Exception ex)
            {

                return false;
            }
            return true;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 16 Aug 2016
        /// Description :   Calls DAL to check area lat-lon is modified
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name="lattitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        internal bool IsAreaGeoLocationChanged(UInt32 areaId, double lattitude, double longitude)
        {
            return commuteDistance.IsAreaGeoLocationChanged(areaId, lattitude, longitude);
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 16 Aug 2016
        /// Description :   Calls DAL to check dealer lat-lon is modified
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="lattitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        internal bool IsDealerGeoLocationChanged(UInt32 dealerId, double lattitude, double longitude)
        {
            return commuteDistance.IsDealerGeoLocationChanged(dealerId, lattitude, longitude);
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 16 Aug 2016
        /// Description :   Checks whether Area exists
        /// </summary>
        /// <param name="areaId"></param>
        /// <returns></returns>
        internal bool IsAreaExists(UInt32 areaId)
        {
            return commuteDistance.IsAreaExists(areaId);
        }
    }
}
