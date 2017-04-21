using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.PWA.Articles;
using Bikewale.Models;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
using System.Collections.Generic;

namespace Bikewale.PWA.UI.Utilities
{
    public static class ConverterUtility
    {
        static string _defaultCityName = BWConfiguration.Instance.GetDefaultCityName;
        public static List<PwaArticleSummary> MapArticleSummaryListToPwaArticleSummaryList(IEnumerable<ArticleSummary> inpSumList)
        {

            var pwaArticleSummaryList = new List<PwaArticleSummary>();

            foreach (var inpSummary in inpSumList)
            {
                pwaArticleSummaryList.Add(ConverterUtility.MapArticleSummaryToPwaArticleSummary(inpSummary));
            }
            return pwaArticleSummaryList;
        }
        public static PwaArticleSummary MapArticleSummaryToPwaArticleSummary(ArticleSummary inpSum)
        {
            PwaArticleSummary outSummary = new PwaArticleSummary();
            if (inpSum != null && inpSum.BasicId>0)
            {
                string catName = GetContentCategory(inpSum.CategoryId);
                outSummary.ArticleUrl = string.Format("/m/{0}/{1}-{2}.html",catName.Replace(" ","-").ToLower(),inpSum.BasicId, inpSum.ArticleUrl);
                outSummary.ArticleApi = string.Format("api/pwa/id/{0}/page/", inpSum.BasicId);
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

        internal static string GetContentCategory(int contentType)
        {
            string _category = string.Empty;
            EnumCMSContentType _contentType = (EnumCMSContentType)contentType;
            try
            {

                switch (_contentType)
                {
                    case EnumCMSContentType.AutoExpo2016:
                    case EnumCMSContentType.News:
                        _category = "NEWS";
                        break;
                    case EnumCMSContentType.Features:
                    case EnumCMSContentType.SpecialFeature:
                        _category = "FEATURES";
                        break;
                    case EnumCMSContentType.ComparisonTests:
                    case EnumCMSContentType.RoadTest:
                        _category = "EXPERT REVIEWS";
                        break;
                    case EnumCMSContentType.TipsAndAdvices:
                        _category = "Bike Care";
                        break;
                    default:
                        break;
                }


            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Mobile.News.Default.GetContentCategory");
            }
            return _category;
        }

        internal static List<PwaBikeDetails> MapMostPopularBikesBaseToPwaBikeDetails(IEnumerable<MostPopularBikesBase> inpList, string cityName)
        {
            List<PwaBikeDetails> outList = new List<PwaBikeDetails>();

            string curCityName = cityName ?? _defaultCityName;
            string makeName;
            string makeMaskingName;
            foreach (var item in inpList)
            {
                makeName = item.objMake == null ? (item.MakeName ?? string.Empty) : item.objMake.MakeName;
                makeMaskingName = item.objMake == null ? (item.MakeMaskingName ?? string.Empty) : item.objMake.MaskingName;

                outList.Add(new PwaBikeDetails()
                {
                    Name = String.Format("{0} {1}", makeName, item.objModel.ModelName),
                    DetailPageUrl = "/m" + UrlFormatter.BikePageUrl(makeMaskingName, item.objModel.MaskingName),
                    ImgUrl = Image.GetPathToShowImages(item.OriginalImagePath, item.HostURL, ImageSize._174x98, QualityFactor._70),
                    Price = item.VersionPrice > 0 ? Format.FormatPrice(item.VersionPrice.ToString()) : string.Empty,
                    PriceDescription = item.VersionPrice > 0 ? "Ex-showroom," + curCityName : string.Empty,
                    PriceSuffix = item.VersionPrice > 0 ? "onwards" : "Price not available"
                }
                );
            }
            return outList;
        }

        internal static List<PwaBikeDetails> MapUpcomingBikeEntityToPwaBikeDetails(IEnumerable<UpcomingBikeEntity> inpList, string cityName)
        {
            List<PwaBikeDetails> outList = new List<PwaBikeDetails>();

            string curCityName = cityName ?? _defaultCityName;
            foreach (var item in inpList)
            {
                outList.Add(new PwaBikeDetails()
                {
                    Name = String.Format("{0} {1}", item.MakeBase.MakeName, item.ModelBase.ModelName),
                    DetailPageUrl = "/m" + UrlFormatter.BikePageUrl(item.MakeBase.MaskingName, item.ModelBase.MaskingName),
                    ImgUrl = Image.GetPathToShowImages(item.OriginalImagePath, item.HostUrl, ImageSize._174x98, QualityFactor._70),
                    Price = item.EstimatedPriceMin > 0 ? Format.FormatPrice(item.EstimatedPriceMin.ToString()) : string.Empty,
                    PriceDescription = item.EstimatedPriceMin > 0 ? "Ex-showroom," + curCityName : string.Empty,
                    PriceSuffix = item.EstimatedPriceMin > 0 ? "onwards" : "Price not available"
                }
                );
            }
            return outList;
        }


        internal static PwaArticleDetails MapArticleDetailsToPwaArticleDetails(ArticleDetails inpDet)
        {
            var outDetails = new PwaArticleDetails();
            if (inpDet != null && inpDet.BasicId>0)
            {
                outDetails.ArticleUrl = string.Format("/m/news/{0}-{1}.html", inpDet.BasicId, inpDet.ArticleUrl);
                outDetails.BasicId = inpDet.BasicId;
                outDetails.Title = inpDet.Title;
                outDetails.AuthorName = inpDet.AuthorName;
                outDetails.AuthorMaskingName = inpDet.AuthorMaskingName;
                outDetails.DisplayDate = inpDet.DisplayDate.ToString("MMM dd, yyyy");
                outDetails.DisplayDateTime = inpDet.DisplayDate.ToString("MMM dd, yyyy hh:mm tt");
                outDetails.HostUrl = inpDet.HostUrl;
                outDetails.Content = inpDet.Content;
                outDetails.PrevArticle = MapArticleSummaryToPwaArticleSummary((ArticleSummary)inpDet.PrevArticle);
                outDetails.NextArticle = MapArticleSummaryToPwaArticleSummary((ArticleSummary)inpDet.NextArticle);
                outDetails.CategoryId = inpDet.CategoryId;
                outDetails.ShareUrl = ReturnShareUrl(outDetails);
                outDetails.LargePicUrl = inpDet.LargePicUrl;
                outDetails.SmallPicUrl = inpDet.SmallPicUrl;
                outDetails.ArticleApi = string.Format("api/pwa/id/{0}/page/", inpDet.BasicId);
            }
            return outDetails;
        }


        internal static string ReturnShareUrl(PwaArticleSummary articleSummary)
        {
            string _bwHostUrl = BWConfiguration.Instance.BwHostUrlForJs;
            EnumCMSContentType contentType = (EnumCMSContentType)articleSummary.CategoryId;
            string shareUrl = string.Empty;
            switch (contentType)
            {
                case EnumCMSContentType.News:
                case EnumCMSContentType.AutoExpo2016:
                    shareUrl = string.Format("{0}/news/{1}-{2}.html", _bwHostUrl, articleSummary.BasicId, articleSummary.ArticleUrl);
                    break;
                case EnumCMSContentType.Features:
                    shareUrl = string.Format("{0}/features/{1}-{2}/", _bwHostUrl, articleSummary.ArticleUrl, articleSummary.BasicId);
                    break;
                case EnumCMSContentType.RoadTest:
                    shareUrl = string.Format("{0}/expert-reviews/{1}-{2}.html", _bwHostUrl, articleSummary.ArticleUrl, articleSummary.BasicId);
                    break;
                case EnumCMSContentType.SpecialFeature:
                    shareUrl = string.Format("{0}/features/{1}-{2}/", _bwHostUrl, articleSummary.ArticleUrl, articleSummary.BasicId);
                    break;
                default:
                    shareUrl = articleSummary.ShareUrl;
                    break;
            }
            return shareUrl;
        }

        internal static List<PwaBikeNews> MapNewBikeListToPwaNewBikeList(NewsDetailPageVM objData,string city)
        {
            List<PwaBikeNews> outData = new List<PwaBikeNews>();

            if(objData!=null)
            {
                PwaBikeNews popularBikes = new PwaBikeNews();
                var orgBikes = objData.MostPopularBikes;
                popularBikes.BikesList = MapMostPopularBikesBaseToPwaBikeDetails(orgBikes.Bikes,city);
                popularBikes.CompleteListUrl = orgBikes.WidgetHref;
                popularBikes.CompleteListUrlAlternateLabel = orgBikes.WidgetLinkTitle;
                popularBikes.CompleteListUrlLabel = "View all";
                popularBikes.Heading = orgBikes.WidgetHeading;
                outData.Add(popularBikes);

                if(objData.Model!=null && objData.Model.ModelId>0)
                {//bodystyle
                    PwaBikeNews bodyStyleBikes = new PwaBikeNews();
                    var orgBodyStyleBikes = objData.PopularBodyStyle;
                    bodyStyleBikes.BikesList = MapMostPopularBikesBaseToPwaBikeDetails(orgBodyStyleBikes.PopularBikes, city);
                    bodyStyleBikes.CompleteListUrl = orgBodyStyleBikes.WidgetHref;
                    bodyStyleBikes.CompleteListUrlAlternateLabel = orgBodyStyleBikes.WidgetLinkTitle;
                    bodyStyleBikes.CompleteListUrlLabel = "View all";
                    bodyStyleBikes.Heading = orgBodyStyleBikes.WidgetHeading;
                    outData.Add(bodyStyleBikes);
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
                    outData.Add(upcomingBikes);
                }
            }

            return outData;
        }

        internal static PwaBikeInfo MapGenericBikeInfoToPwaBikeInfo(BikeInfoVM objData)
        {
            PwaBikeInfo outBikeInfo = null;
            
            if (objData != null)
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
                        outBikeInfo.PriceDescription = String.Format("Ex-showroom, {0}", objData.CityDetails.CityName);
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
    }
}
