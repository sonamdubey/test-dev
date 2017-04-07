using Bikewale.Entities.CMS;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.PWA.Articles;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;

namespace Bikewale.PWA.UI.Utilities
{
    public static class ConverterUtility
    {
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
            if (inpSum != null)
            {
                outSummary.ArticleUrl = inpSum.ArticleUrl;
                outSummary.ArticleApi = string.Format("api/pwa/id/{0}/page/", inpSum.BasicId);
                outSummary.AuthorName = inpSum.AuthorName;
                outSummary.Description = inpSum.Description;
                outSummary.BasicId = inpSum.BasicId;
                outSummary.Title = inpSum.Title;
                outSummary.CategoryId = inpSum.CategoryId;
                outSummary.CategoryName = GetContentCategory(inpSum.CategoryId);
                outSummary.DisplayDate = inpSum.DisplayDate.ToString("MMM dd, yyyy");
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
    }
}
