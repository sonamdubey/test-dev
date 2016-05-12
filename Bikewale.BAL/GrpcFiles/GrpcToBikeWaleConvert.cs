using Bikewale.Entities.CMS.Articles;
using Grpc.CMS;
using System;
using System.Collections.Generic;

namespace Bikewale.BAL.GrpcFiles
{
    public class GrpcToBikeWaleConvert
    {
        public static CMSContent ConvertFromGrpcToBikeWale(GrpcCMSContent data)
        {
            CMSContent dataNew = new CMSContent();
            dataNew.RecordCount = data.RecordCount;
            dataNew.Articles = new List<ArticleSummary>();

            foreach (var item in data.Articles)
            {
                var curArt = new ArticleSummary();
                curArt.ArticleUrl = item.ArticleBase.ArticleUrl;
                curArt.BasicId = item.ArticleBase.BasicId;
                //curArt.url = item.ArticleBaseObj.CompleteArticleUrl;
                curArt.Title = item.ArticleBase.Title;
                curArt.AuthorName = item.AuthorName;
                curArt.CategoryId = (ushort)item.CategoryId;
                curArt.Description = item.Description;
                curArt.DisplayDate = Convert.ToDateTime(item.DisplayDate);
                curArt.FacebookCommentCount = item.FacebookCommentCount;
                curArt.HostUrl = item.HostUrl;
                curArt.IsFeatured = false;
                curArt.IsSticky = item.IsSticky;
                curArt.LargePicUrl = item.LargePicUrl;
                curArt.OriginalImgUrl = item.OriginalImgUrl;
                curArt.SmallPicUrl = item.SmallPicUrl;
                curArt.Views = item.Views;
                dataNew.Articles.Add(curArt);
            }
            return dataNew;
        }


        public static IList<ArticleSummary> ConvertFromGrpcToBikeWale(GrpcArticleSummaryList data)
        {
            List<ArticleSummary> retData = new List<ArticleSummary>();
            ArticleSummary curArticleSummary;
            foreach (var curGrpcArticleSummary in data.Summary)
            {
                curArticleSummary = new ArticleSummary()
                {
                    ArticleUrl = curGrpcArticleSummary.ArticleBase.ArticleUrl,
                    AuthorName = curGrpcArticleSummary.AuthorName,
                    BasicId = curGrpcArticleSummary.ArticleBase.BasicId,
                    CategoryId = (ushort)curGrpcArticleSummary.CategoryId,
                    Description = curGrpcArticleSummary.Description,
                    DisplayDate = Convert.ToDateTime(curGrpcArticleSummary.DisplayDate),
                    FacebookCommentCount = curGrpcArticleSummary.FacebookCommentCount,
                    HostUrl = curGrpcArticleSummary.HostUrl,
                    //IsFeatured=curGrpcArticleSummary.fe
                    IsSticky = curGrpcArticleSummary.IsSticky,
                    LargePicUrl = curGrpcArticleSummary.LargePicUrl,
                    OriginalImgUrl = curGrpcArticleSummary.OriginalImgUrl,
                    SmallPicUrl = curGrpcArticleSummary.SmallPicUrl,
                    Title = curGrpcArticleSummary.ArticleBase.Title,
                    Views = curGrpcArticleSummary.Views
                };


                retData.Add(curArticleSummary);
            }

            return retData;
        }
    }
}