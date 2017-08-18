using Bikewale.Entities;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.Videos;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Models.Videos
{
    /// <summary>
    /// Created by : Ashutosh Sharma on 17-Aug-2017
    /// Description : Provide method to get list of scooter videos.
    /// </summary>
    public class ScooterVideos
    {
        private readonly IVideos _videos = null;
        private uint _cookieCityId;


        public ScooterVideos(IVideos videos)
        {
            _videos = videos;
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 17-Aug-2017
        /// Description : Method to get list of scooter videos.
        /// </summary>
        /// <returns>List of scooter videos.</returns>
        public ScooterVideosVM GetData()
        {
            ScooterVideosVM objVideos = null;

            try
            {
                string bodyStyleId = "5";

                objVideos = new ScooterVideosVM();
                objVideos.VideosList = _videos.GetVideosByMakeModel(0, 0, bodyStyleId, 0, 0);

                GlobalCityAreaEntity currentCityArea = GlobalCityArea.GetGlobalCityArea();
                _cookieCityId = currentCityArea.CityId;

                objVideos.CityId = _cookieCityId;

                objVideos.PageMetaTags.Title = "Scooter Videos | Expert Review & First Launch  videos on Scooters- BikeWale";
                objVideos.PageMetaTags.Description = "Watch latest videos on scooters by experts. Know more about latest scooter launches, road test and comparison of scooters";

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Bikewale.Models.Videos.ScooterVideos.GetData"));
                
            }
            return objVideos;
        }
    }
}