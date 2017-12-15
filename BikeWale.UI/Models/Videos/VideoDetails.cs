
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Bikewale.Common;
using Bikewale.Entities;
using Bikewale.Entities.Schema;
using Bikewale.Entities.Videos;
using Bikewale.Interfaces.Videos;
using Bikewale.Utility;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by  :   Sumit Kate on 29 Mar 2017
    /// Description :   VideoDetails Page model
    /// </summary>
    public class VideoDetails
    {
        private readonly uint _videoId;
        private readonly IVideosCacheRepository _videosCache;
        private readonly BikeVideoEntity _videoEntity;

        public StatusCodes Status { get; private set; }
        public bool IsMobile { get; set; }

        /// <summary>
        /// Created by  :   Sumit Kate on 29 Mar 2017
        /// Description :   Constructor to initialize member variables
        /// </summary>
        /// <param name="videoId"></param>
        /// <param name="IVideos"></param>
        public VideoDetails(uint videoId, IVideosCacheRepository IVideos)
        {
            _videoId = videoId;
            _videosCache = IVideos;
            _videoEntity = _videosCache.GetVideoDetails(_videoId);
            if (_videoEntity != null)
            {
                Status = StatusCodes.ContentFound;
            }
            else
            {
                Status = StatusCodes.ContentNotFound;
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 29 Mar 2017
        /// Description :   Returns the View Model for Videos Details page
        /// </summary>
        /// <returns></returns>
        public VideoDetailsPageVM GetData()
        {
            VideoDetailsPageVM model = null;
            try
            {
                if (_videoEntity != null)
                {
                    model = new VideoDetailsPageVM();
                    model.Video = _videoEntity;
                    model.VideoId = _videoId;
                    if (model.Video.MakeName != null || model.Video.ModelName != null)
                        model.IsBikeTagged = true;
                    model.DisplayDate = FormatDate.GetFormatDate(model.Video.DisplayDate, "dd MMMM yyyy");
                    model.Description = FormatDescription.SanitizeHtml(model.Video.Description);
                    model.SmallDescription = model.IsSmallDescription ? StringHtmlHelpers.TruncateHtml(_videoEntity.Description, 200, " ..") : "";
                    CreateDescriptionTag(model);
                    SetBreadcrumList(model);

                    if (model.IsBikeTagged)
                        GetTaggedModel(model);
                    Status = StatusCodes.ContentFound;
                }
                else
                {
                    Status = StatusCodes.ContentNotFound;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("GetData({0})", _videoId));
                Status = StatusCodes.ContentNotFound;
            }
            return model;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 29 Mar 2017
        /// Description :   Checks for Tagged model
        /// </summary>
        /// <param name="model"></param>
        private void GetTaggedModel(VideoDetailsPageVM model)
        {
            try
            {
                model.TaggedModelId = model.Video.ModelId;
                model.CityId = GlobalCityArea.GetGlobalCityArea().CityId;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("GetTaggedModel({0})", _videoId));
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 29 Mar 2017
        /// Description :   Creates page description
        /// </summary>
        /// <param name="model"></param>
        private void CreateDescriptionTag(VideoDetailsPageVM model)
        {
            try
            {
                model.PageMetaTags.CanonicalUrl = string.Format("https://www.bikewale.com/bike-videos/{0}-{1}/", model.Video.VideoTitleUrl, model.Video.BasicId);
                model.PageMetaTags.AlternateUrl = string.Format("https://www.bikewale.com/m/bike-videos/{0}-{1}/", model.Video.VideoTitleUrl, model.Video.BasicId);

                if (model.IsBikeTagged)
                {
                    model.PageMetaTags.Keywords = String.Format("{0},{1}, {0} {1}", model.Video.MakeName, model.Video.ModelName);
                    string yeh = EnumVideosCategory.DoitYourself.ToString();
                    switch (model.Video.SubCatId)
                    {
                        case "55":
                            model.PageMetaTags.Description = String.Format("{0} Video Review-Watch BikeWale Expert's Take on {0} - Features, performance, price, fuel economy, handling and more.", model.TaggedBikeName);
                            model.PageMetaTags.Title = String.Format("Expert Video Review - {0} - BikeWale", model.TaggedBikeName);
                            model.Video.SubCatName = "Expert Reviews";
                            break;
                        case "57":
                            model.PageMetaTags.Description = String.Format("First Ride Video Review of {0} - Watch BikeWale Expert's Take on the First Ride of {0} - Features, performance, price, fuel economy, handling and more.", model.TaggedBikeName);
                            model.PageMetaTags.Title = String.Format("First Ride Video Review - {0} - BikeWale", model.TaggedBikeName);
                            break;
                        case "59":
                            model.PageMetaTags.Description = String.Format("Launch Video of {0} - {0} bike launched. Watch BikeWale's Expert's take on its Launch-Features, performance, price, fuel economy, handling and more.", model.TaggedBikeName);
                            model.PageMetaTags.Title = String.Format("Bike Launch Video Review - {0} - BikeWale", model.TaggedBikeName);
                            break;
                        case "53":
                            model.PageMetaTags.Description = String.Format("Do It Yourself tips for {0}.  Watch Do It Yourself tips for {0} from BikeWale's Experts.", model.TaggedBikeName);
                            model.PageMetaTags.Title = String.Format("Do It Yourself - {0} - BikeWale", model.TaggedBikeName);
                            break;
                        default:
                            model.PageMetaTags.Description = "Check latest bike and scooter videos, watch BikeWale expert's take on latest bikes and scooters-features, performance, price, fuel economy, handling and more.";
                            model.PageMetaTags.Title = String.Format("{0} - BikeWale", model.Video.VideoTitle);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("VideoDetailsHelper.CreateDescriptionTag() => videoBasicId {0}", _videoId));
            }
        }
        /// <summary>
        /// Created By :Snehal Dange on 8th Nov 2017
        /// Description : Function to create page level schema for breadcrum
        /// </summary>
        private void SetBreadcrumList(VideoDetailsPageVM objData)
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

            BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position, string.Format("{0}bike-videos/", bikeUrl), "Bike Videos"));

            if (!String.IsNullOrEmpty(objData.Video.SubCatName))
            {
               bikeUrl += String.Format("bike-videos/category/{0}-{1}/", Regex.Replace(objData.Video.SubCatName.Trim(), @"[\(\)\s]+", "-").ToLower(), Regex.Replace(objData.Video.SubCatId, @"[,]+", "-"));
               BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position, bikeUrl, objData.Video.SubCatName));
            }

            BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position, null, objData.Video.VideoTitle));

            objData.BreadcrumbList.BreadcrumListItem = BreadCrumbs;

        }
    }
}