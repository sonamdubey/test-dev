using Bikewale.Entities.Location;
using Bikewale.Entities.Schema;
using Bikewale.Interfaces.Videos;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
using System.Collections.Generic;

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
        /// Modified by : Snehal Dange on 29th Nov 2017
        /// Descritpion : Added ga for page
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
                SetBreadcrumList(objVideos);
                objVideos.Page = Entities.Pages.GAPages.Videos_Landing_Page;


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
        /// <summary>
        /// Created By : Snehal Dange on 10th Nov 2017
        /// Description : Function to create page level schema for breadcrum
        /// </summary>
        private void SetBreadcrumList(ScooterVideosVM objPageVM)
        {
            try
            {
                IList<BreadcrumbListItem> BreadCrumbs = new List<BreadcrumbListItem>();
                string bikeUrl;
                bikeUrl = string.Format("{0}/", Utility.BWConfiguration.Instance.BwHostUrl);
                ushort position = 1;
                if (IsMobile)
                {
                    bikeUrl += "m/";
                }

                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, bikeUrl, "Home"));

                bikeUrl = string.Format("{0}scooters/", bikeUrl);

                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, bikeUrl, "Scooters"));

                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position, null, "Scooter Videos"));
                if (objPageVM != null)
                {
                    objPageVM.BreadcrumbList.BreadcrumListItem = BreadCrumbs;
                }

            }
            catch (Exception ex)
            {

                Bikewale.Notifications.ErrorClass.LogError(ex, "Bikewale.Models.Videos.ScooterVideos.SetBreadcrumList()");
            }




        }
    }
}