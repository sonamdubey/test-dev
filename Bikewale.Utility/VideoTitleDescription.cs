
using System;
namespace Bikewale.Utility
{
    public static class VideoTitleDescription
    {
        /// <summary>
        /// Author : Vivek Gupta
        /// Date : 19-02-2016
        /// this static method returns title and description of videos category vise
        /// </summary>
        /// <param name="cId"></param>
        /// <returns></returns>
        public static string[] VideoTitleDesc(uint cId, string make, string model)
        {
            string[] strVideos = new string[2];
            switch (cId)
            {
                case 1:
                    strVideos[0] = "Bike Launch Video Review - BikeWale";
                    strVideos[1] = "Bike Launch Videos. Watch BikeWale's Expert's Take on New Bike and Scooter Launches - Features, performance, price, fuel economy, handling and more";
                    break;
                case 3:
                    strVideos[0] = String.Format("Expert Video Review - {0} {1} -  BikeWale", make, model);
                    strVideos[1] = "Expert Video Reviews- Watch BikeWale Expert's Take on Reviews of Bikes and Scooters- Features, performance, price, fuel economy, handling and more.";
                    break;
                case 6:
                    strVideos[0] = "Bike Launch Video Review - BikeWale";
                    strVideos[1] = "Bike Launch Videos. Watch BikeWale's Expert's Take on New Bike and Scooter Launches - Features, performance, price, fuel economy, handling and more";
                    break;
                case 7:
                    strVideos[0] = "First Ride Video Review - BikeWale";
                    strVideos[1] = "First Ride Video Reviews - Watch BikeWale Expert's Take on the First Ride of Bikes and Scooters - Features, performance, price, fuel economy, handling and more.";
                    break;

                default:
                    strVideos[0] = "Bike Videos, Expert Video Reviews with Road Test & Bike Comparison -   BikeWale";
                    strVideos[1] = "Check latest bike and scooter videos, watch BikeWale expert's Take on latest bikes and scooters - features, performance, price, fuel economy, handling and more.";
                    break;
            }

            return strVideos;
        }
    }
}
