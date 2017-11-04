﻿using Bikewale.Entities;
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
        public bool IsMobile = false;
        


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
                string bodyStyleId = Convert.ToString((int)Entities.GenericBikes.EnumBikeBodyStyles.Scooter);

                objVideos = new ScooterVideosVM();
                objVideos.VideosList = _videos.GetVideosByMakeModel(0, 0, bodyStyleId, 0, 0);

                GlobalCityAreaEntity currentCityArea = GlobalCityArea.GetGlobalCityArea();
                objVideos.CityId = currentCityArea.CityId;

                BindPageMetas(objVideos);
                

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.Videos.ScooterVideos.GetData");
                
            }
            return objVideos;
        }


        /// <summary>
        /// Created by : Ashutosh Sharma on 17-Aug-2017
        /// Description : Method to bind page metas..
        /// </summary>
        /// <param name="scooterVideo"></param>
        private void BindPageMetas(ScooterVideosVM scooterVideo)
        {
            try
            {
                if (scooterVideo != null)
                {
                    scooterVideo.PageMetaTags.Title = "Scooter Videos | Expert Review & First Launch  videos on Scooters- BikeWale";
                    scooterVideo.PageMetaTags.Description = "Watch latest videos on scooters by experts. Know more about latest scooter launches, road test and comparison of scooters";

                    scooterVideo.PageMetaTags.CanonicalUrl = "https://www.bikewale.com/scooters/videos/";
                    if (!IsMobile)
                    {
                        scooterVideo.PageMetaTags.AlternateUrl = "https://www.bikewale.com/m/scooters/videos/";
                    } 
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Models.Videos.ScooterVideos.BindPageMetas_scooterVideo_{0}", scooterVideo));
            }
        }
    }
}