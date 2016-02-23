
using System;
namespace Bikewale.Utility
{
    /// <summary>
    /// Created By : 
    /// </summary>
    public class VideoTitleDescription
    {
        /// <summary>
        /// Author : Vivek Gupta
        /// Date : 19-02-2016
        /// this static method returns title and description of videos category vise
        /// </summary>
        /// <param name="cId"></param>
        /// <returns></returns>
        public static void VideoTitleDesc(string categoryId, out string title, out string desc, string make = null, string model = null)
        {
            string descText = "Watch BikeWale's Expert's Take on New Bike and Scooter Launches - Features, performance, price, fuel economy, handling and more";

            switch (categoryId)
            {
                case "59":
                    title = "Bike Launch Video Review - BikeWale";
                    desc = "Bike Launch Videos- " + descText;
                    break;
                case "47,55":
                    title = String.Format("Expert Video Review - BikeWale");
                    desc = "Expert Video Reviews- " + descText;
                    break;
                case "57":
                    title = "First Ride Video Review - BikeWale";
                    desc = "First Ride Video Reviews - " + descText;
                    break;
                case "53":
                    title = "Do It Yourself - BikeWale";
                    desc = "Do It Yourself tips - " + descText;
                    break;
                default:
                    title = "Bike Videos, Expert Video Reviews with Road Test & Bike Comparison -   BikeWale";
                    desc = "Check latest bike and scooter videos, " + descText;
                    break;
            }
        }

        /// <summary>
        /// Created By : Lucky Rathore
        /// Created On : 23 Feb 2016
        /// Summary : For <H1> (Heading) of video by category page.
        /// </summary>
        /// <param name="categoryId">Specific category Id of video</param>
        /// <returns>String for heding corresponding to category Id</returns>
        public static string VideoHeading(uint categoryId)
        {
           switch (categoryId)
            {
                case 59:
                    return "Bike Launch Video";
                case 3:
                    return "Expert Video";//String.Format(- {0} {1} -  BikeWale"); //, make, model
                case 57:
                    return "First Ride Video";// - BikeWale";
                case 53:
                    return "Do It Yourself";
                default:
                    return "Bike Video";
                    
            }
        }
    }//class end
}//namespae ended
