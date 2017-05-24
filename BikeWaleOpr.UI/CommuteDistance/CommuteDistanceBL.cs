using BikewaleOpr.Entities;
using BikeWaleOpr.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using BikewaleOpr.Interface.ContractCampaign;
using BikewaleOpr.Entity.DealerCampaign;
using BikewaleOpr.Interface;

namespace BikewaleOpr.CommuteDistance
{
    /// <summary>
    /// Created By  :   Sumit Kate on 18 Apr 2016
    /// Description :   Commute Distance Business Layer
    /// Modified BY : Ashish G. Kamble on 16 May 2017
    /// Summary : Removed old asyncrounous code and replaced with async and await. Also changed the process to assign the areas to the dealer.
    /// </summary>
    public class CommuteDistanceBL : ICommuteDistance
    {
        private readonly IDealerCampaignRepository _campaignRepo = null;

        public CommuteDistanceBL(IDealerCampaignRepository campaignRepo)
        {
            _campaignRepo = campaignRepo;
        }

        /// <summary>
        /// Written By : Ashish G. Kamble on 12 May 2017
        /// Summary : Function to map campaign areas with dealer location.
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="campaignServingStatus">Status of the serving areas to the particular campaign.</param>
        /// <param name="servingRadius">Serving radius for the given dealer (campaign serving radius).</param>
        /// <param name="cityIdList">Comma separated city id list. e.g. cityid1, cityid2, cityid3</param>
        /// <returns></returns>
        public bool SaveCampaignAreas(uint dealerId, ushort campaignServingStatus, ushort servingRadius, string cityIdList)
        {
            bool isUpdated = false;

            try
            {
                //get the Areas served by the dealers by lead serving distance(straight line distance calculation)
                DealerAreaDistance objDealerAreaDist = _campaignRepo.GetDealerToAreasDistance(dealerId, campaignServingStatus, servingRadius, cityIdList);

                // get dealer to areas distance from google api
                isUpdated = UpdateCommuteDistance(dealerId, objDealerAreaDist);

                // map campaign areas
                _campaignRepo.SaveDealerCampaignAreaMapping(dealerId, campaignServingStatus, servingRadius, cityIdList);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "SaveCampaignAreas");
                objErr.SendMail();
            }

            return isUpdated;
        }


        /// <summary>
        /// Written By : Ashish G. Kamble on 15 May 2017
        /// Summary : Function to map the additional areas to the dealer's location
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="areasList">Provide list in (,) separated format separated by comma e.g. 'areaid1,'areaid2'</param>
        public bool SaveAdditionalCampaignAreas(uint dealerId, string areaIdList)
        {
            bool isUpdated = false;

            try
            {
                // Get dealer location along with areas latitude and longitude 
                DealerAreaDistance objDealerAreaDist = _campaignRepo.GetDealerAreasWithLatLong(dealerId, areaIdList);

                // get dealer to areas distance from google api
                isUpdated = UpdateCommuteDistance(dealerId, objDealerAreaDist);

                // map additional campaign areas
                _campaignRepo.SaveAdditionalAreasMapping(dealerId, areaIdList);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "SaveAdditionalCampaignAreas");
                objErr.SendMail();                
            }

            return isUpdated;
        }


        #region UpdateCommuteDistance method
        /// <summary>
        /// Modified By : Ashish G. Kamble on 17 May 2017
        /// Summary : Function to get the distance between dealer location and the area using google api. Updates dealertoarea distance in the master table for distances.
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="objDealerAreaDist"></param>
        /// <returns></returns>
        public bool UpdateCommuteDistance(uint dealerId, DealerAreaDistance objDealerAreaDist)
        {
            try
            {
                CommuteDistanceRepository commuteDistance = new CommuteDistanceRepository();

                GeoLocationEntity dealer = objDealerAreaDist.DealerLocation;
                IEnumerable<GeoLocationEntity> areas = objDealerAreaDist.Areas;

                if (dealer != null && (areas != null && areas.Count() > 0))
                {
                    GoogleDistanceAPIHelper googleApi = new GoogleDistanceAPIHelper();

                    IEnumerable<GeoLocationEntity> areasBatch = areas.Take(50);

                    int batchCounter = 0;

                    do
                    {
                        //Call the Google API and get distances between areas and dealer location
                        IEnumerable<GeoLocationDestinationEntity> apiResp = googleApi.GetDistanceUsingArrayIndex(dealer, areasBatch, false);

                        if (apiResp != null && apiResp.Count() > 0)
                        {
                            string formatedResp = googleApi.FormatApiResp(apiResp);

                            //update the areas with Commute Distance
                            commuteDistance.UpdateArea(dealer.Id, formatedResp);
                        }

                        ++batchCounter;
                        areasBatch = areas.Skip(50 * batchCounter).Take(50);
                        Thread.Sleep(100);

                    } while (areasBatch.Count() > 0);
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
        #endregion

    }   // class
}   // namespace