using Bikewale.Entities;
using Bikewale.Interfaces.Videos;
using Bikewale.Notifications;
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
            ScooterVideosVM objVideosList = null;

            try
            {
                string bodyStyleId = "5";

                objVideosList = new ScooterVideosVM();
                objVideosList.VideosList = _videos.GetVideosByMakeModel(0, 0, bodyStyleId, 0, 0);

                

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Bikewale.Models.Videos.ScooterVideos.GetData"));
                
            }
            return objVideosList;
        }
    }
}