using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.PWA.Articles;
using Bikewale.Entities.Videos;
using Bikewale.Models.Shared;
using Bikewale.Models;
using Bikewale.PWA.Entities.Photos;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using Bikewale.Models.BikeModels;
using Bikewale.Models.BikeMakes;

namespace Bikewale.PWA.Utils
{
    public static class ConverterUtility
    {
        static string _defaultCityName = BWConfiguration.Instance.GetDefaultCityName;

        /// <summary>
        /// Converts ArticleSummary to PwaArticleSummary
        /// </summary>
        /// Modified by : Rajan Chauhan on 05 Mar 2018
        /// Description : Added condition on ArticleApi for categoryId 8 and 2 (if RoadTest category articles are requested than BAL method also returns ComparisonTests articles)
        /// <param name="inpSum"></param>
        /// <returns></returns>
        public static PwaArticleSummary MapArticleSummaryToPwaArticleSummary(ArticleSummary inpSum)
        {
            PwaArticleSummary outSummary = null;
            if (inpSum != null && inpSum.BasicId > 0)
            {
                outSummary = new PwaArticleSummary();
                string catName = PwaCmsHelper.GetContentCategory(inpSum.CategoryId);
                outSummary.ArticleUrl = PwaCmsHelper.GetArticleUrl(inpSum.CategoryId, inpSum.ArticleUrl, (int)inpSum.BasicId);
                outSummary.ArticleApi = string.Format("api/pwa/cms/id/{0}/{1}/", inpSum.BasicId, (inpSum.CategoryId==8 || inpSum.CategoryId == 2 ? "pages":"page"));
                outSummary.AuthorName = inpSum.AuthorName;
                outSummary.Description = inpSum.Description;
                outSummary.BasicId = inpSum.BasicId;
                outSummary.Title = inpSum.Title;
                outSummary.CategoryId = inpSum.CategoryId;
                outSummary.CategoryName = catName;
                outSummary.DisplayDate = inpSum.DisplayDate.ToString("MMM dd, yyyy");
                outSummary.DisplayDateTime = inpSum.DisplayDate.ToString("MMM dd, yyyy hh:mm tt");
                outSummary.HostUrl = inpSum.HostUrl;
                outSummary.SmallPicUrl = inpSum.SmallPicUrl;
                outSummary.LargePicUrl = inpSum.LargePicUrl;
            }
            return outSummary;
        }
        /// <summary>
        ///  Converts ArticleDetails to PwaArticleDetails
        /// Modified By : Ashish G. Kamble on 5 Jan 2018
        /// Modified : Assigend Tags property value in PwaArticleDetails response object
        /// </summary>
        /// <param name="inpDet"></param>
        /// <returns></returns>
        public static PwaArticleDetails MapArticleDetailsToPwaArticleDetails(ArticleDetails inpDet)
        {
            var outDetails = new PwaArticleDetails();
            if (inpDet != null && inpDet.BasicId > 0)
            {
                outDetails.ArticleUrl = PwaCmsHelper.GetArticleUrl(inpDet.CategoryId, inpDet.ArticleUrl, (int)inpDet.BasicId);
                outDetails.BasicId = inpDet.BasicId;
                outDetails.Title = inpDet.Title;
                outDetails.AuthorName = inpDet.AuthorName;
                outDetails.AuthorMaskingName = inpDet.AuthorMaskingName;
                outDetails.DisplayDate = inpDet.DisplayDate.ToString("MMM dd, yyyy");
                outDetails.DisplayDateTime = inpDet.DisplayDate.ToString("MMM dd, yyyy hh:mm tt");
                outDetails.HostUrl = inpDet.HostUrl;
                outDetails.TopContent = inpDet.Content;
                outDetails.PrevArticle = MapArticleSummaryToPwaArticleSummary((ArticleSummary)inpDet.PrevArticle);
                outDetails.NextArticle = MapArticleSummaryToPwaArticleSummary((ArticleSummary)inpDet.NextArticle);
                outDetails.CategoryId = inpDet.CategoryId;
                outDetails.CategoryName = PwaCmsHelper.GetContentCategory(inpDet.CategoryId);
                outDetails.LargePicUrl = inpDet.LargePicUrl;
                outDetails.SmallPicUrl = inpDet.SmallPicUrl;
                outDetails.ArticleApi = string.Format("api/pwa/cms/id/{0}/page/", inpDet.BasicId);
                outDetails.Tags = (inpDet.TagsList != null && inpDet.TagsList.Count() > 0) ? String.Join(",", inpDet.TagsList) : string.Empty;
                outDetails.ShareUrl = PwaCmsHelper.ReturnShareUrl(inpDet);
            }
            return outDetails;
        }
        /// <summary>
        /// Converts ArticlePageDetails to PwaArticleDetails
        /// Created By : Pratibha Verma on 24 Feb, 2018
        /// </summary>
        /// <param name="inpDet"></param>
        /// <returns></returns>
        public static PwaArticleDetails MapArticleDetailsToPwaExpertReviewDetails(ArticlePageDetails inpDet, int matchedPage)
        {
            var outDetails = new PwaArticleDetails();
            if (inpDet != null && inpDet.BasicId > 0)
            {
                outDetails.ArticleUrl = PwaCmsHelper.GetArticleUrl(inpDet.CategoryId, inpDet.ArticleUrl, (int)inpDet.BasicId);
                outDetails.BasicId = inpDet.BasicId;
                outDetails.Title = inpDet.Title;
                outDetails.AuthorName = inpDet.AuthorName;
                outDetails.AuthorMaskingName = inpDet.AuthorMaskingName;
                outDetails.DisplayDate = inpDet.DisplayDate.ToString("MMM dd, yyyy");
                outDetails.DisplayDateTime = inpDet.DisplayDate.ToString("MMM dd, yyyy hh:mm tt");
                outDetails.HostUrl = inpDet.HostUrl;
                outDetails.TopContent = MapPageToContent(inpDet.PageList, 0, matchedPage + 1);
                outDetails.BottomContent = MapPageToContent(inpDet.PageList, matchedPage + 1);
                outDetails.CategoryId = inpDet.CategoryId;
                outDetails.CategoryName = PwaCmsHelper.GetContentCategory(inpDet.CategoryId);
                outDetails.LargePicUrl = inpDet.LargePicUrl;
                outDetails.SmallPicUrl = inpDet.SmallPicUrl;
                outDetails.ArticleApi = string.Format("api/pwa/cms/id/{0}/pages/", inpDet.BasicId);
                outDetails.Tags = (inpDet.TagsList != null && inpDet.TagsList.Count() > 0) ? String.Join(",", inpDet.TagsList) : string.Empty;
                outDetails.ShareUrl = PwaCmsHelper.ReturnSharePageUrl(inpDet);
            }
            return outDetails;
        }
        /// <summary>
        /// Converts Page to Content
        /// Created By : Pratibha Verma on 24 Feb, 2018
        /// Modified by : Ashutosh Sharma on 01 Mar 2018.
        /// Description : Method to fetch html string from startIndex to endIndex of pagelist.
        /// </summary>
        /// <param name = "PageList" ></ param >
        /// <param name="startIndex">Index of pagelist from html to be fetched.</param>
        /// <param name="endIndex">Upto this index of pagelist html to be fetched. If not set than html upto end of pagelist will be returned.</param>
        /// <returns></returns>

        public static string MapPageToContent(List<Page> PageList, int startIndex, int endIndex = 0)
        {
            string content = null;
            try
            {
                content = string.Empty;

                if (PageList != null && PageList.Any())
                {
                    endIndex = endIndex == 0 ? PageList.Count : endIndex;
                    for (int i = startIndex; i < endIndex; i++)
                    {
                        content += "<div class=\"margin - bottom10\"><h3 class=\"margin-bottom10\" role=\"heading\">" + PageList[i].PageName +
                                "</h3><div id='@page.pageId' class=\"margin-top-10 article-content\">" + PageList[i].Content + "</div></div>";
                    } 
                }
            }
            catch (Exception ex)
            {
                throw;  
            }
            return content;
        }

        /// <summary>
        /// Created by  : Rajan Chauhan on 26 Feb 2018
        /// Description : Converts EditCMSPhotoGalleryVM to PwaImageList
        /// </summary>
        /// <param name="photoGalleryVM"></param>
        /// <returns></returns>
        public static PwaImageList MapPhotoGalleryToPwaImageList(EditCMSPhotoGalleryVM photoGalleryVM)
        {
            PwaImageList imageGallery = new PwaImageList();

            if (photoGalleryVM != null && photoGalleryVM.Images != null && photoGalleryVM.Images.Any())
            {
                imageGallery.ModelImages = PwaConvert(photoGalleryVM.Images);
                imageGallery.RecordCount = photoGalleryVM.ImageCount;
            }
            return imageGallery;
        }

        /// <summary>
        /// Converts MostPopularBikes to PwaBikeDetails for the specified City
        /// </summary>
        /// <param name="inpList"></param>
        /// <param name="cityName"></param>
        /// <returns></returns>
        public static List<PwaBikeDetails> MapMostPopularBikesBaseToPwaBikeDetails(IEnumerable<MostPopularBikesBase> inpList)
        {
            List<PwaBikeDetails> outList = null;
            try
            {

                outList = new List<PwaBikeDetails>();
                string makeName, modelName;
                int modelId;
                string makeMaskingName;
                if (inpList != null)
                {
                    foreach (var item in inpList)
                    {
                        makeName = item.objMake == null ? (item.MakeName ?? string.Empty) : item.objMake.MakeName;
                        modelName = item.objModel == null ? "" : item.objModel.ModelName;
                        modelId = item.objModel == null ? 0 : item.objModel.ModelId;
                        makeMaskingName = item.objMake == null ? (item.MakeMaskingName ?? string.Empty) : item.objMake.MaskingName;
                        outList.Add(new PwaBikeDetails()
                        {
                            Name = String.Format("{0} {1}", makeName, modelName),
                            DetailPageUrl = "/m" + UrlFormatter.BikePageUrl(makeMaskingName, item.objModel.MaskingName),
                            ImgUrl = Image.GetPathToShowImages(item.OriginalImagePath, item.HostURL, ImageSize._174x98, QualityFactor._70),
                            Price = item.VersionPrice > 0 ? Format.FormatPrice(item.VersionPrice.ToString()) : string.Empty,
                            PriceDescription = "Ex-showroom, " + (!string.IsNullOrEmpty(item.CityName) ? item.CityName : _defaultCityName),
                            PriceSuffix = item.VersionPrice > 0 ? "onwards" : "Price not available",
                            ModelId = modelId,
                            MakeName = makeName,
                            ModelName = modelName,
                            IsNew = true
                        }
                        );
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            
            return outList;
        }
        /// <summary>
        /// Modified by : Rajan Chauhan on 26 Feb 2018
        /// Description : Added null check on bikeList
        /// </summary>
        /// <param name="bikeList"></param>
        /// <returns></returns>
        public static List<PwaMakeBikeEntity> MapBikeMakeEntityBaseToPwaMakeBikeEntity(IEnumerable<BikeMakeEntityBase> bikeList) {
            List<PwaMakeBikeEntity> outList = new List<PwaMakeBikeEntity>();
            if (bikeList != null && bikeList.Any())
            {
                foreach (var make in bikeList)
                {
                    outList.Add(new PwaMakeBikeEntity()
                    {
                        MakeId = make.MakeId,
                        MakeName = make.MakeName,
                        MaskingName = make.MaskingName,
                        TotalCount = make.TotalCount
                    }
                    );
                }
            }
            return outList;
        }
        /// <summary>
        /// Converts the list of upcoming bikes to the list of PwaBikeDetails for the given city
        /// Modified by : Rajan Chauhan on 26 Feb 2018
        /// Description : Added null check on inpList
        /// </summary>
        /// <param name="inpList"></param>
        /// <param name="cityName"></param>
        /// <returns></returns>
        public static List<PwaBikeDetails> MapUpcomingBikeEntityToPwaBikeDetails(IEnumerable<UpcomingBikeEntity> inpList, string cityName)
        {
            List<PwaBikeDetails> outList = new List<PwaBikeDetails>();
            try
            {
                string curCityName = cityName ?? _defaultCityName;
                if (inpList != null && inpList.Any())
                {
                    foreach (var item in inpList)
                    {
                        outList.Add(new PwaBikeDetails()
                        {
                            Name = String.Format("{0} {1}", item.MakeBase.MakeName, item.ModelBase.ModelName),
                            DetailPageUrl = "/m" + UrlFormatter.BikePageUrl(item.MakeBase.MaskingName, item.ModelBase.MaskingName),
                            ImgUrl = Image.GetPathToShowImages(item.OriginalImagePath, item.HostUrl, ImageSize._174x98, QualityFactor._70),
                            Price = item.EstimatedPriceMin > 0 ? Format.FormatPrice(item.EstimatedPriceMin.ToString()) : string.Empty,
                            PriceDescription = item.EstimatedPriceMin > 0 ? "Expected price" : string.Empty,
                            PriceSuffix = item.EstimatedPriceMin > 0 ? "onwards" : "Price not available",

                        }
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return outList;
        }
        /// <summary>
        /// Converts the GenericBikeInfo to PwaBikeInfo for the given city
        /// </summary>
        /// <param name="inpBikeInfo"></param>
        /// <param name="cityDetails"></param>
        /// <returns></returns>
        public static PwaBikeInfo MapGenericBikeInfoToPwaBikeInfo(GenericBikeInfo inpBikeInfo, CityEntityBase cityDetails)
        {
            PwaBikeInfo outBikeInfo = null;
            if (inpBikeInfo != null)
            {
                bool isUpComing = inpBikeInfo.IsFuturistic;
                bool isDiscontinued = (!inpBikeInfo.IsNew && !inpBikeInfo.IsFuturistic);
                outBikeInfo = new PwaBikeInfo();
                outBikeInfo.ModelName = string.Format("{0} {1}", inpBikeInfo.Make.MakeName, inpBikeInfo.Model.ModelName);
                outBikeInfo.ModelId = inpBikeInfo.Model.ModelId;
                outBikeInfo.Upcoming = isUpComing.ToString();
                outBikeInfo.Discontinued = isDiscontinued.ToString();
                outBikeInfo.ModelDetailUrl = string.Format("/m{0}", UrlFormatter.BikePageUrl(inpBikeInfo.Make.MaskingName, inpBikeInfo.Model.MaskingName));
                outBikeInfo.ImageUrl = Image.GetPathToShowImages(inpBikeInfo.OriginalImagePath, inpBikeInfo.HostUrl, ImageSize._110x61, QualityFactor._70);

                var bikeRatings = new PwaBikeRating();
                bikeRatings.Rating = inpBikeInfo.Rating.ToString("0.0");
                bikeRatings.Count = inpBikeInfo.RatingCount;
                bikeRatings.ReviewCount = inpBikeInfo.UserReviewCount;
                bikeRatings.ReviewUrl = string.Format("/m{0}", UrlFormatter.FormatUserReviewUrl(inpBikeInfo.Make.MaskingName, inpBikeInfo.Model.MaskingName));
                outBikeInfo.Rating = bikeRatings;


                if (isDiscontinued)
                {
                    outBikeInfo.PriceDescription = "Last known Ex-showroom price";
                    outBikeInfo.Price = Format.FormatPrice(Convert.ToString(inpBikeInfo.BikePrice));

                }
                else if (isUpComing)
                {
                    outBikeInfo.PriceDescription = "Expected price";
                    outBikeInfo.Price = String.Format("{0} - {1}",
                        Format.FormatNumeric(Convert.ToString(inpBikeInfo.EstimatedPriceMin)),
                        Format.FormatNumeric(Convert.ToString(inpBikeInfo.EstimatedPriceMax)));

                }
                else
                {
                    if (inpBikeInfo.PriceInCity > 0 && cityDetails != null)
                    {
                        outBikeInfo.PriceDescription = String.Format("On-road price, {0}", cityDetails.CityName);
                        outBikeInfo.Price = Format.FormatPrice(Convert.ToString(inpBikeInfo.PriceInCity));

                    }
                    else
                    {
                        outBikeInfo.PriceDescription = String.Format("Ex-showroom, {0}", BWConfiguration.Instance.DefaultName);
                        outBikeInfo.Price = Format.FormatPrice(Convert.ToString(inpBikeInfo.BikePrice));
                    }
                }

                //usedbikelist
                PwaBikeInfoUsedBikeDetails usedBikeInfo = null;
                if (inpBikeInfo.UsedBikeCount > 0)
                {
                    usedBikeInfo = new PwaBikeInfoUsedBikeDetails();
                    usedBikeInfo.DescriptionLabel = string.Format("{0} Used {1} bikes",
                        inpBikeInfo.UsedBikeCount, inpBikeInfo.Model.ModelName);
                    usedBikeInfo.PricePrefix = "starting at";
                    usedBikeInfo.Price = Format.FormatNumeric(inpBikeInfo.UsedBikeMinPrice.ToString());
                    usedBikeInfo.UsedBikesLinkUrl = string.Format("/m{0}", UrlFormatter.UsedBikesUrlNoCity(inpBikeInfo.Make.MaskingName,
                        inpBikeInfo.Model.MaskingName, (cityDetails != null) ? cityDetails.CityMaskingName : "india"));
                }
                outBikeInfo.UsedBikesLink = usedBikeInfo;
                outBikeInfo.MoreDetailsList = new List<PwaBikeInfoExtraDetails>();
                //moredetailsList
                PwaBikeInfoExtraDetails curInfo;
                foreach (var item in inpBikeInfo.Tabs)
                {

                    curInfo = new PwaBikeInfoExtraDetails();
                    curInfo.Type = item.TabText;
                    curInfo.Title = string.Format("{0} {1}", outBikeInfo.ModelName, item.Title);
                    curInfo.DetailUrl = string.Format("/m{0}", item.URL);
                    outBikeInfo.MoreDetailsList.Add(curInfo);
                }
            }
            return outBikeInfo;
        }
        /// <summary>
        /// Converts the NewBikeList to PwaNewBikeList for the given city
        /// </summary>
        /// <param name="objData"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        public static List<PwaBikeNews> MapNewBikeListToPwaNewBikeList<T>(T objData, string city) where T: CMSArticleDetailPageVM
        {
            List<PwaBikeNews> outData = new List<PwaBikeNews>();

            if (objData != null)
            {
                PwaBikeNews popularBikes = new PwaBikeNews();
                var orgBikes = objData.MostPopularBikes;
                popularBikes.BikesList = MapMostPopularBikesBaseToPwaBikeDetails(orgBikes.Bikes);
                popularBikes.CompleteListUrl = orgBikes.WidgetHref;
                popularBikes.CompleteListUrlAlternateLabel = orgBikes.WidgetLinkTitle;
                popularBikes.CompleteListUrlLabel = "View all";
                popularBikes.Heading = orgBikes.WidgetHeading;
                outData.Add(popularBikes);

                if (objData.Model != null && objData.Model.ModelId > 0)
                {//bodystyle
                    PwaBikeNews bodyStyleBikes = new PwaBikeNews();
                    var orgBodyStyleBikes = objData.PopularBodyStyle;
                    if (orgBodyStyleBikes != null)
                    {
                        bodyStyleBikes.BikesList = MapMostPopularBikesBaseToPwaBikeDetails(orgBodyStyleBikes.PopularBikes);
                        bodyStyleBikes.CompleteListUrl = orgBodyStyleBikes.WidgetHref;
                        bodyStyleBikes.CompleteListUrlAlternateLabel = orgBodyStyleBikes.WidgetLinkTitle;
                        bodyStyleBikes.CompleteListUrlLabel = "View all";
                        bodyStyleBikes.Heading = orgBodyStyleBikes.WidgetHeading;
                        if (bodyStyleBikes.BikesList.Count > 0)
                            outData.Add(bodyStyleBikes);
                    }
                }
                else
                {///upcoming
                    PwaBikeNews upcomingBikes = new PwaBikeNews();
                    var orgUpcomingBikes = objData.UpcomingBikes;
                    upcomingBikes.BikesList = MapUpcomingBikeEntityToPwaBikeDetails(orgUpcomingBikes.UpcomingBikes, city);
                    upcomingBikes.CompleteListUrl = orgUpcomingBikes.WidgetHref;
                    upcomingBikes.CompleteListUrlAlternateLabel = orgUpcomingBikes.WidgetLinkTitle;
                    upcomingBikes.CompleteListUrlLabel = "View all";
                    upcomingBikes.Heading = orgUpcomingBikes.WidgetHeading;
                    if (upcomingBikes.BikesList.Count > 0)
                        outData.Add(upcomingBikes);
                }
            }

            return outData;
        }

        /// <summary>
        /// Created by : Rajan Chauhan on 27 Feb 2018
        /// Description : To convert Article page PopularScooterMakesWidget data to List<PwaMakeBikeBase>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objData"></param>
        /// <returns></returns>
        public static List<PwaMakeBikeBase> MapBikeMakeEntityBaseListToPwaMakeBikeBaseList<T>(T objData) where T : CMSArticleDetailPageVM
        {
            List<PwaMakeBikeBase> outData = null;
            try
            {
                if (objData != null && objData.PopularScooterMakesWidget != null && objData.PopularScooterMakesWidget.Any())
                {
                    PwaMakeBikeBase makeListBase = new PwaMakeBikeBase()
                    {
                        MakeList = MapBikeMakeEntityBaseListToPwaMakeBikeEntityList(objData.PopularScooterMakesWidget),
                        Heading = string.Format("Popular {0} Brands", BodyStyleLinks.BodyStyleHeadingText(EnumBikeBodyStyles.Scooter)),
                        CompleteListUrlAlternateLabel = string.Format("{0} Brands", BodyStyleLinks.BodyStyleHeadingText(EnumBikeBodyStyles.Scooter)),
                        CompleteListUrl = "/m/scooters/",
                        CompleteListUrlLabel = "View all"
                    };
                    outData = new List<PwaMakeBikeBase>();
                    outData.Add(makeListBase);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return outData;
        }


        /// <summary>
        /// Created by : Sanskar Gupta on 27 Feb 2018
        /// Description : To convert Article page OtherBrands data to List<PwaMakeBikeBase>
        /// </summary>
        /// <returns></returns>
        public static List<PwaMakeBikeBase> MapOtherBrandsWidgetDataToPWA(IDictionary<EditorialPageWidgetPosition, EditorialWidgetVM> PageWidgets)
        {
            if (PageWidgets == null)
            {
                return null;
            }

            List<PwaMakeBikeBase> widgetList = new List<PwaMakeBikeBase>();
            try
            {
                foreach (KeyValuePair<EditorialPageWidgetPosition, EditorialWidgetVM> widgetItem in PageWidgets)
                {
                    IDictionary<EditorialWidgetColumnPosition, EditorialWidgetInfo> widgets = widgetItem.Value.WidgetColumns;
                    EditorialWidgetColumnPosition populatedKey = widgets.ContainsKey(EditorialWidgetColumnPosition.Left) ? EditorialWidgetColumnPosition.Left : EditorialWidgetColumnPosition.Right;
                    EditorialWidgetInfo widget = widgets[populatedKey];

                    switch (widget.WidgetType)
                    {
                        case EditorialWidgetType.Popular:
                            break;
                        case EditorialWidgetType.Upcoming:
                            break;
                        case EditorialWidgetType.OtherBrands:
                            EditorialOtherBrandsWidget otherBrandsWidget = widget as EditorialOtherBrandsWidget;
                            IEnumerable<BikeMakeEntityBase> otherBrandsList = null;
                            if(otherBrandsWidget != null)
                            {
                                otherBrandsList = otherBrandsWidget.OtherBrandsList;
                            }

                            if (otherBrandsList != null && otherBrandsList.Any())
                            {
                                PwaMakeBikeBase makeListBase = new PwaMakeBikeBase()
                                {
                                    MakeList = MapBikeMakeEntityBaseListToPwaMakeBikeEntityList(otherBrandsList),
                                    Heading = string.Format("Popular {0} Brands", BodyStyleLinks.BodyStyleHeadingText(EnumBikeBodyStyles.Scooter)),
                                    CompleteListUrlAlternateLabel = string.Format("{0} Brands", BodyStyleLinks.BodyStyleHeadingText(EnumBikeBodyStyles.Scooter)),
                                    CompleteListUrl = "/m/scooters/",
                                    CompleteListUrlLabel = "View all"
                                };
                                widgetList.Add(makeListBase);
                            }
                            break;
                        default:
                            break;
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return widgetList;
        }



        /// <summary>
        /// Created by  : Sanskar Gupta on 19 April 2018
        /// Description : Utility to Map Popular Bikes from normal list to PWA list.  
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objData"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        public static List<PwaBikeNews> MapPopularAndUpcomingWidgetDataToPwa(IDictionary<EditorialPageWidgetPosition, EditorialWidgetVM> PageWidgets)
        {
            if(PageWidgets == null)
            {
                return null;
            }
            List<PwaBikeNews> widgetList = new List<PwaBikeNews>();

            try
            {
                foreach (KeyValuePair<EditorialPageWidgetPosition, EditorialWidgetVM> widgetItem in PageWidgets)
                {
                    IDictionary<EditorialWidgetColumnPosition, EditorialWidgetInfo> widgets = widgetItem.Value.WidgetColumns;
                    EditorialWidgetColumnPosition populatedKey = widgets.ContainsKey(EditorialWidgetColumnPosition.Left) ? EditorialWidgetColumnPosition.Left : EditorialWidgetColumnPosition.Right;
                    EditorialWidgetInfo widget = widgets[populatedKey];

                    PwaBikeNews bikes = new PwaBikeNews();

                    switch (widget.WidgetType)
                    {
                        case EditorialWidgetType.Popular:
                            {
                                var widgetData = widget as EditorialPopularBikesWidget;
                                if (widgetData != null)
                                {
                                    bikes.CompleteListUrl = widgetData.ViewAllUrl;
                                    bikes.CompleteListUrlAlternateLabel = widgetData.ViewAllTitle;
                                    bikes.CompleteListUrlLabel = "View all";
                                    bikes.Heading = widgetData.Title;
                                    bikes.BikesList = MapMostPopularBikesBaseToPwaBikeDetails(widgetData.MostPopularBikeList);
                                }
                            }
                            break;
                        case EditorialWidgetType.Upcoming:
                            {
                                var widgetData = widget as EditorialUpcomingBikesWidget;
                                if (widgetData != null)
                                {
                                    bikes.CompleteListUrl = widgetData.ViewAllUrl;
                                    bikes.CompleteListUrlAlternateLabel = widgetData.ViewAllTitle;
                                    bikes.CompleteListUrlLabel = "View all";
                                    bikes.Heading = widgetData.Title;
                                    bikes.BikesList = MapUpcomingBikeEntityToPwaBikeDetails(widgetData.UpcomingBikeList, ""); 
                                }
                            }
                            break;
                        case EditorialWidgetType.OtherBrands:
                            break;
                        default:
                            break;
                    }
                    widgetList.Add(bikes);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return widgetList;
        }

        
        /// <summary>
        /// Converts the BikeInfo to the PWABikeInfo
        /// </summary>
        /// <param name="objData"></param>
        /// <returns></returns>
        public static PwaBikeInfo MapGenericBikeInfoToPwaBikeInfo(BikeInfoVM objData)
        {
            PwaBikeInfo outBikeInfo = null;

            if (objData != null && objData.BikeInfo != null)
            {
                var orgBikeInfo = objData.BikeInfo;
                bool isUpComing = objData.IsUpcoming;
                bool isDiscontinued = objData.IsDiscontinued;
                outBikeInfo = new PwaBikeInfo();
                outBikeInfo.ModelName = objData.BikeName;
                outBikeInfo.ModelId = orgBikeInfo.Model.ModelId;
                outBikeInfo.Upcoming = isUpComing.ToString();
                outBikeInfo.Discontinued = isDiscontinued.ToString();
                outBikeInfo.ModelDetailUrl = string.Format("/m{0}", objData.BikeUrl);
                outBikeInfo.ImageUrl = Image.GetPathToShowImages(orgBikeInfo.OriginalImagePath, orgBikeInfo.HostUrl, ImageSize._110x61, QualityFactor._70);


                if (isDiscontinued)
                {
                    outBikeInfo.PriceDescription = "Last known Ex-showroom price";
                    outBikeInfo.Price = Format.FormatPrice(Convert.ToString(orgBikeInfo.BikePrice));

                }
                else if (isUpComing)
                {
                    outBikeInfo.PriceDescription = "Expected price";
                    outBikeInfo.Price = String.Format("{0} - {1}",
                        Format.FormatNumeric(Convert.ToString(orgBikeInfo.EstimatedPriceMin)),
                        Format.FormatNumeric(Convert.ToString(orgBikeInfo.EstimatedPriceMax)));

                }
                else
                {
                    if (orgBikeInfo.PriceInCity > 0 && objData.CityDetails != null)
                    {
                        outBikeInfo.PriceDescription = String.Format("On-road price, {0}", objData.CityDetails.CityName);
                        outBikeInfo.Price = Format.FormatPrice(Convert.ToString(orgBikeInfo.PriceInCity));

                    }
                    else
                    {
                        outBikeInfo.PriceDescription = String.Format("Ex-showroom, {0}", BWConfiguration.Instance.DefaultName);
                        outBikeInfo.Price = Format.FormatPrice(Convert.ToString(orgBikeInfo.BikePrice));
                    }
                }

                //usedbikelist
                PwaBikeInfoUsedBikeDetails usedBikeInfo = null;
                if (orgBikeInfo.UsedBikeCount > 0)
                {
                    usedBikeInfo = new PwaBikeInfoUsedBikeDetails();
                    usedBikeInfo.DescriptionLabel = string.Format("{0} Used {1} bikes",
                        orgBikeInfo.UsedBikeCount, orgBikeInfo.Model.ModelName);
                    usedBikeInfo.PricePrefix = "starting at";
                    usedBikeInfo.Price = Format.FormatNumeric(orgBikeInfo.UsedBikeMinPrice.ToString());
                    usedBikeInfo.UsedBikesLinkUrl = string.Format("/m{0}", UrlFormatter.UsedBikesUrlNoCity(orgBikeInfo.Make.MaskingName,
                        orgBikeInfo.Model.MaskingName, (objData.CityDetails != null) ? objData.CityDetails.CityMaskingName : "india"));
                }
                outBikeInfo.UsedBikesLink = usedBikeInfo;
                outBikeInfo.MoreDetailsList = new List<PwaBikeInfoExtraDetails>();
                //moredetailsList
                PwaBikeInfoExtraDetails curInfo;
                foreach (var item in orgBikeInfo.Tabs)
                {

                    curInfo = new PwaBikeInfoExtraDetails();
                    curInfo.Type = item.TabText;
                    curInfo.Title = string.Format("{0} {1}", outBikeInfo.ModelName, item.Title);
                    curInfo.DetailUrl = string.Format("/m{0}", item.URL);
                    outBikeInfo.MoreDetailsList.Add(curInfo);
                }
            }
            return outBikeInfo;
        }
        /// <summary>
        /// Converts the list of ArticleSummary to PwaArticleSummary list
        /// </summary>
        /// <param name="inpSumList"></param>
        /// <returns></returns>
        public static List<PwaArticleSummary> MapArticleSummaryListToPwaArticleSummaryList(IEnumerable<ArticleSummary> inpSumList)
        {

            var pwaArticleSummaryList = new List<PwaArticleSummary>();

            foreach (var inpSummary in inpSumList)
            {
                pwaArticleSummaryList.Add(ConverterUtility.MapArticleSummaryToPwaArticleSummary(inpSummary));
            }
            return pwaArticleSummaryList;
        }

        /// <summary>
        /// Converts BikeVideoEntity to PwaVikeVideoEntity
        /// </summary>
        /// <param name="inpList"></param>
        /// <returns></returns>
        public static IEnumerable<PwaBikeVideoEntity> PwaConvert(IEnumerable<BikeVideoEntity> inpList, bool addShortDesc = false)
        {
            List<PwaBikeVideoEntity> outList = new List<PwaBikeVideoEntity>();

            if (inpList != null && inpList.Count() > 0)
            {
                PwaCovertAndAppend(outList, inpList, addShortDesc);
            }
            return outList;
        }
        /// <summary>
        /// Converts the items from inpList to PwaBikeVideoEntity and adds them to outList
        /// </summary>
        /// <param name="outList"></param>
        /// <param name="inpList"></param>
        public static void PwaCovertAndAppend(List<PwaBikeVideoEntity> outList, IEnumerable<BikeVideoEntity> inpList, bool addShortDesc = false)
        {

            PwaBikeVideoEntity tempData;
            if (outList != null && inpList != null && inpList.Count() > 0)
            {
                foreach (var inp in inpList)
                {
                    tempData = PwaConvert(inp, addShortDesc);
                    if (tempData != null)
                        outList.Add(tempData);
                }
            }

        }
        /// <summary>
        /// Converts BikeVideoEntity to PwaBikeVideoEntity
        /// </summary>
        /// <param name="inpEntity"></param>
        /// <param name="addShortDesc">Decides whether to add the Short Description and Description to the Converted Entity. By default it's false i.e. don't add</param>
        /// <returns></returns>
        public static PwaBikeVideoEntity PwaConvert(BikeVideoEntity inpEntity, bool addShortDesc = false)
        {
            PwaBikeVideoEntity outEntity = null;
            if (inpEntity != null)
            {
                outEntity = new PwaBikeVideoEntity();
                outEntity.BasicId = inpEntity.BasicId;
                if (addShortDesc)
                {
                    outEntity.Description = FormatDescription.SanitizeHtml(inpEntity.Description);
                    outEntity.ShortDescription = outEntity.Description.Length > 200 ? StringHtmlHelpers.TruncateHtml(outEntity.Description, 200, " ..") : "";
                }
                outEntity.DisplayDate = FormatDate.GetFormatDate(inpEntity.DisplayDate, "dd MMMM yyyy");
                outEntity.Likes = Bikewale.Utility.Format.FormatPrice(Convert.ToString(inpEntity.Likes));
                outEntity.VideoId = inpEntity.VideoId;
                outEntity.VideoTitle = inpEntity.VideoTitle;
                outEntity.VideoTitleUrl = inpEntity.VideoTitleUrl;
                outEntity.VideoUrl = inpEntity.VideoUrl;
                outEntity.Views = Bikewale.Utility.Format.FormatPrice(Convert.ToString(inpEntity.Views));
                string secTitle, moreVidUrl;
                int catId;
                if (Int32.TryParse(inpEntity.SubCatId, out catId))
                {
                    VideoTitleDescription.VideoGetTitleAndUrl(catId, out secTitle, out moreVidUrl);
                    outEntity.SectionTitle = secTitle;
                    outEntity.SectionUrl = moreVidUrl;
                }
            }
            return outEntity;

        }
        /// <summary>
        /// Converts the list of BikeMakeEntityBase to the list of PwaBikeMakeEntityBase
        /// </summary>
        /// <param name="inpList"></param>
        /// <returns></returns>
        public static IEnumerable<PwaBikeMakeEntityBase> PwaConvert(IEnumerable<BikeMakeEntityBase> inpList)
        {
            List<PwaBikeMakeEntityBase> outList = null;
            if (inpList != null)
            {
                outList = new List<PwaBikeMakeEntityBase>();
                foreach (var make in inpList)
                {
                    outList.Add(PwaConvert(make));
                }
            }
            return outList;
        }

        /// <summary>
        /// Converts BikeMakeEntityBase to PwaBikeMakeEntityBase
        /// </summary>
        /// <param name="inp"></param>
        /// <returns></returns>
        public static PwaBikeMakeEntityBase PwaConvert(BikeMakeEntityBase inp)
        {
            PwaBikeMakeEntityBase outEntity = null;
            if (inp != null)
            {
                outEntity = new PwaBikeMakeEntityBase();
                outEntity.MakeId = inp.MakeId;
                outEntity.MakeName = inp.MakeName;
                outEntity.Href = String.Format("/{0}-bikes/videos/", inp.MaskingName);
                outEntity.Title = String.Format("{0} bikes videos", inp.MakeName);
            }
            return outEntity;
        }

        /// <summary>
        /// Created by : Ashutosh Sharam on 12 Feb 2018.
        /// Description : Method to convert `IEnumerable<ModelImages>` to `IEnumerable<PwaModelImages>`.
        /// </summary>
        /// <param name="modelImagesList">List of Models with list of their image list.</param>
        /// <returns>PWA entity which contains list of models with their image list.</returns>
        public static IEnumerable<PwaModelImages> PwaConvert(IEnumerable<ModelImages> modelImagesList)
        {
            IList<PwaModelImages> pwaModelImagesList = null;
            try
            {
                if (modelImagesList != null)
                {
                    pwaModelImagesList = new List<PwaModelImages>();
                    foreach (var modelImages in modelImagesList)
                    {
                        pwaModelImagesList.Add(new PwaModelImages() {
                            BikeName = modelImages.BikeName,
                            ModelName = modelImages.ModelBase.ModelName,
                            MakeName = modelImages.MakeBase.MakeName,
                            ModelId = modelImages.ModelId,
                            ModelImagePageUrl = string.Format("/m/{0}-bikes/{1}/images/", modelImages.MakeBase.MaskingName, modelImages.ModelBase.MaskingName),
                            ModelImages = PwaConvert(modelImages.ModelImage),
                            RecordCount = modelImages.RecordCount
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return pwaModelImagesList;
        }

        /// <summary>
        /// Created by : Ashutosh Sharam on 12 Feb 2018.
        /// Description : Method to convert `IEnumerable<ModelImage>` to `IEnumerable<PwaImageBase>`.
        /// Modified by : Rajan Chauhan on 24 Feb 2018
        /// Description : Added ImageName, ImageCategory, ModelName
        /// </summary>
        /// <param name="modelImageList">List of images.</param>
        /// <returns>PWA entity which contains list of images.</returns>
        public static IEnumerable<PwaImageBase> PwaConvert(IEnumerable<ModelImage> modelImageList)
        {
            IList<PwaImageBase> pwaImageBaseList = null;
            try
            {
                if (modelImageList != null)
                {
                    pwaImageBaseList = new List<PwaImageBase>();
                    foreach (var modelImage in modelImageList)
                    {
                        pwaImageBaseList.Add(new PwaImageBase()
                        {
                            HostUrl = modelImage.HostUrl,
                            OriginalImgPath = modelImage.OriginalImgPath,
                            ImageName = modelImage.ImageName,
                            ImageCategory = modelImage.ImageCategory,
                            ModelName = modelImage.ModelBase.ModelName
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return pwaImageBaseList;
        }

        /// <summary>
        /// Created By : Rajan Chauhan on 26 Feb 2018
        /// Summary    : To convert IEnumerable<BikeMakeEntityBase> to List<PwaMakeBikeEntity>
        /// </summary>
        /// <param name="inpList"></param>
        /// <returns></returns>
        public static List<PwaMakeBikeEntity> MapBikeMakeEntityBaseListToPwaMakeBikeEntityList(IEnumerable<BikeMakeEntityBase> inpList)
        {
            List<PwaMakeBikeEntity> outList = null;
            try {
                if (inpList != null && inpList.Any())
                {
                    outList = new List<PwaMakeBikeEntity>();
                    foreach (BikeMakeEntityBase make in inpList)
                    {
                        outList.Add(new PwaMakeBikeEntity()
                        {
                            MakeId = make.MakeId,
                            MakeName = make.MakeName,
                            MaskingName = make.MaskingName,
                            TotalCount = make.TotalCount
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return outList;
        }
    }
}
