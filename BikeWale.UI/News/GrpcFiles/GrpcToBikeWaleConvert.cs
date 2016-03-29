using Bikewale.Entities.CMS.Articles;
using Grpc.CMS;
using System;
using System.Collections.Generic;
using Grpc.Core;

namespace Bikewale.News.GrpcFiles
{
    public class GrpcToBikeWaleConvert
    {
        internal static CMSContent ConvertFromCarwaleToBikeWale(GrpcCMSContent data)
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
    }
}