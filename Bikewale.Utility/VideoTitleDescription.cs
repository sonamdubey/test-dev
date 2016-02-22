
using System;
namespace Bikewale.Utility
{
    public class VideoTitleDescription
    {
        /// <summary>
        /// Author : Vivek Gupta
        /// Date : 19-02-2016
        /// this static method returns title and description of videos category vise
        /// </summary>
        /// <param name="cId"></param>
        /// <returns></returns>
        public static void VideoTitleDesc(uint categoryId, out string title, out string desc, string make = null, string model = null)
        {
            string descText = "Watch BikeWale's Expert's Take on New Bike and Scooter Launches - Features, performance, price, fuel economy, handling and more";

            switch (categoryId)
            {
                case 59:
                    title = "Bike Launch Video Review - BikeWale";
                    desc = "Bike Launch Videos- " + descText;
                    break;
                case 3:
                    title = String.Format("Expert Video Review - {0} {1} -  BikeWale", make, model);
                    desc = "Expert Video Reviews- " + descText;
                    break;
                case 57:
                    title = "First Ride Video Review - BikeWale";
                    desc = "First Ride Video Reviews - " + descText;
                    break;
                case 53:
                    title = "Do It Yourself - BikeWale";
                    desc = "Do It Yourself tips - " + descText;
                    break;

                default:
                    title = "Bike Videos, Expert Video Reviews with Road Test & Bike Comparison -   BikeWale";
                    desc = "Check latest bike and scooter videos, " + descText;
                    break;
            }
        }
    }
}
