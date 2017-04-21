using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.PWA.Articles;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
using System.Collections.Generic;

namespace Bikewale.PWA.Service.Utilities
{
    internal static class ConverterUtility
    {
        static string _defaultCityName = BWConfiguration.Instance.GetDefaultCityName;

        internal static PwaArticleSummary MapArticleSummaryToPwaArticleSummary(ArticleSummary inpSum)
        {
            PwaArticleSummary outSummary = null;
            if (inpSum != null && inpSum.BasicId>0)
            {
                outSummary = new PwaArticleSummary();
                outSummary.ArticleUrl = string.Format("/m/news/{0}-{1}.html", inpSum.BasicId, inpSum.ArticleUrl);
                outSummary.ArticleApi = string.Format("api/pwa/id/{0}/page/", inpSum.BasicId);
                outSummary.AuthorName = inpSum.AuthorName;
                outSummary.Description = string.Empty;//inpSum.Description; empty for now
                outSummary.BasicId = inpSum.BasicId;
                outSummary.Title = inpSum.Title;
                outSummary.CategoryId = inpSum.CategoryId;
                outSummary.CategoryName = GetContentCategory(inpSum.CategoryId);
                outSummary.DisplayDate = inpSum.DisplayDate.ToString("MMM dd, yyyy");
                outSummary.DisplayDateTime = inpSum.DisplayDate.ToString("MMM dd, yyyy hh:mm tt");
                outSummary.HostUrl = inpSum.HostUrl;
                outSummary.SmallPicUrl = inpSum.SmallPicUrl;
                outSummary.LargePicUrl = inpSum.LargePicUrl;
            }
            return outSummary;
        }

        internal static PwaArticleDetails MapArticleDetailsToPwaArticleDetails(ArticleDetails inpDet)
        {
            var outDetails = new PwaArticleDetails();
            if (inpDet != null && inpDet.BasicId > 0)
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
                outDetails.CategoryName = GetContentCategory(inpDet.CategoryId);
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


        internal static PwaBikeInfo MapGenericBikeInfoToPwaBikeInfo(GenericBikeInfo inpBikeInfo, CityEntityBase cityDetails)
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
                        outBikeInfo.PriceDescription = String.Format("Ex-showroom, {0}", cityDetails.CityName);
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

      
    }
}
