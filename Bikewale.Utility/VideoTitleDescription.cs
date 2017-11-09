﻿
using Bikewale.Entities.Videos;
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
        public static void VideoTitleDesc(string categoryIds, out string title, out string desc, string make = null, string model = null)
        {
            string descText = "Watch BikeWale's Expert's Take on New Bike and Scooter Launches - Features, performance, price, fuel economy, handling and more";

            switch (categoryIds)
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
                case "61":
                    title = "First Look Video Review - BikeWale";
                    desc = "First Look Video Reviews - Watch BikeWale Expert's Take on the First Look of Bikes and Scooters - Features, performance, price, fuel economy, handling and more.";
                    break;
                case "62":
                case "63":
                    title = "Bike Videos, Expert Video Reviews with Road Test & Bike Comparison - BikeWale";
                    desc = "Check latest bike and scooter videos, " + descText;
                    break;
                case "51":
                    title = "Motorsport Videos - BikeWale";
                    desc = "Check all the latest action from the Indian motorsport world. Keep a close tab on various make championships, rallies and get to know the Indian mortorsport racers. ";
                    break;
                default:
                    title = "Bike Videos, Expert Video Reviews with Road Test & Bike Comparison - BikeWale";
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

        /// <summary>
        /// Created by : Aditi Srivastava on 25 Mar 2017
        /// Summary    : To create title for videos according to category
        /// </summary>
        public static string VideoCategoryTitle(string categoryId)
        {
            switch (categoryId)
            {
                case "51":
                    return "Motorsports";
                case "55":
                    return "Expert Reviews";
                case "57":
                    return "First Ride Impressions";
                case "58":
                    return "Miscellaneous";
                case "59":
                    return "Launch Alert";
                case "60":
                    return "PowerDrift Top Music";
                case "61":
                    return "First Look";
                case "62":
                    return "PowerDrift Blockbuster";
                case "63":
                    return "PowerDrift Specials";
                default:
                    return "";
            }
        }

        /// <summary>
        ///  Created by Prasad Gawde
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns>returns the Category title for the input Category Id</returns>
        public static string VideoCategoryTitle(int categoryId)
        {
            switch (categoryId)
            {
                case 51:
                    return "Motorsports";
                case 55:
                    return "Expert Reviews";
                case 57:
                    return "First Ride Impressions";
                case 58:
                    return "Miscellaneous";
                case 59:
                    return "Launch Alert";
                case 60:
                    return "PowerDrift Top Music";
                case 61:
                    return "First Look";
                case 62:
                    return "PowerDrift Blockbuster";
                case 63:
                    return "PowerDrift Specials";
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        ///  Created by Prasad Gawde
        ///  provides the Category name and the moreVideoUrl 
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="catTitle"></param>
        /// <param name="moreVideoUrl"></param>
        public static void VideoGetTitleAndUrl(int categoryId, out string catTitle, out string moreVideoUrl)
        {
            switch (categoryId)
            {
                case 51:
                    moreVideoUrl = @"/motorsports-51/";
                    catTitle = @"Motorsports";
                    break;
                case 55:
                    moreVideoUrl = @"/expert-reviews-55/";
                    catTitle = @"Expert Reviews";
                    break;
                case 57:
                    moreVideoUrl = @"/first-ride-impressions-57/";
                    catTitle = @"First Ride Impressions";
                    break;
                case 58:
                    moreVideoUrl = @"/miscellaneous-58/";
                    catTitle = @"Miscellaneous";
                    break;
                case 59:
                    moreVideoUrl = @"/launch-alert-59/";
                    catTitle = @"Launch Alert";
                    break;
                case 60:
                    moreVideoUrl = @"/powerdrift-top-music-60/";
                    catTitle = @"PowerDrift Top Music";
                    break;
                case 61:
                    moreVideoUrl = @"/first-look-61/";
                    catTitle = @"First Look";
                    break;
                case 62:
                    moreVideoUrl = @"/powerdrift-blockbuster-62/";
                    catTitle = @"PowerDrift Blockbuster";
                    break;
                case 63:
                    moreVideoUrl = @"/powerdrift-specials-63/";
                    catTitle = @"PowerDrift Specials";
                    break;
                default:
                    moreVideoUrl = string.Empty;
                    catTitle = string.Empty;
                    break;
            }
        }

    }//class end
}//namespae ended
