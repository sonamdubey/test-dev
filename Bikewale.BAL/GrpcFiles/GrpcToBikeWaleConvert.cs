using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.CMS.Photos;
using EditCMSWindowsService.Messages;
using Google.Protobuf.Collections;
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

            foreach (var item in data.Articles.LstGrpcArticleSummary)
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

        public static List<ArticleSummary> ConvertFromGrpcToBikeWale(GrpcArticleSummaryList data)
        {
            List<ArticleSummary> retData = new List<ArticleSummary>();
            ArticleSummary curArticleSummary;
            foreach (var curGrpcArticleSummary in data.LstGrpcArticleSummary)
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

        public static List<ModelImage> ConvertFromGrpcToBikeWale(GrpcModelImageList data)
        {
            List<ModelImage> retData = new List<ModelImage>();
            ModelImage curModelImage;
            foreach (var curGrpcModelImage in data.LstGrpcModelImage)
            {
                curModelImage = new ModelImage()
                {
                    AltImageName = curGrpcModelImage.AltImageName,
                    Caption = curGrpcModelImage.Caption,
                    HostUrl = curGrpcModelImage.HostUrl,
                    ImageCategory = curGrpcModelImage.ImageCategory,
                    ImageDescription = curGrpcModelImage.ImageDescription,
                    ImageId = curGrpcModelImage.ImageId,
                    ImageName = curGrpcModelImage.ImageName,
                    ImagePathLarge = curGrpcModelImage.ImagePathLarge,
                    ImagePathThumbnail = curGrpcModelImage.ImagePathThumbnail,
                    ImageTitle = curGrpcModelImage.ImageTitle,
                    MainImgCategoryId = (short)curGrpcModelImage.MainImgCategoryId,
                    MakeBase = ConvertFromGrpcToBikeWale(curGrpcModelImage.MakeBase),
                    ModelBase = ConvertFromGrpcToBikeWale(curGrpcModelImage.ModelBase),
                    OriginalImgPath = curGrpcModelImage.OriginalImgPath                    
                };


                retData.Add(curModelImage);
            }

            return retData;
        }

        public static BikeMakeEntityBase ConvertFromGrpcToBikeWale(GrpcCarMakeEntityBase grpcMake)
        {
            if (grpcMake != null)
            {
                BikeMakeEntityBase bwMake = new BikeMakeEntityBase()
                {
                    MakeId = grpcMake.MakeId,
                    MakeName = grpcMake.MakeName
                };
                return bwMake;
            }
            else
                return null;
        }

        public static BikeModelEntityBase ConvertFromGrpcToBikeWale(GrpcCarModelEntityBase grpcModel)
        {
            if (grpcModel != null)
            {
                BikeModelEntityBase bwModel = new BikeModelEntityBase()
                {
                    ModelId = grpcModel.ModelId,
                    ModelName = grpcModel.ModelName,
                    MaskingName=grpcModel.MaskingName
                };
                return bwModel;
            }
            else
                return null;
        }

        public static BikeVersionEntityBase ConvertFromGrpcToBikeWale(GrpcCarVersionEntity grpcVersion)
        {
            if (grpcVersion != null)
            {
                BikeVersionEntityBase bwVersion = new BikeVersionEntityBase()
                {
                    VersionId = grpcVersion.Id,
                    VersionName = grpcVersion.Name
                };
                return bwVersion;
            }
            else
                return null;
        }

        public static ArticleDetails ConvertFromGrpcToBikeWale(GrpcArticleDetails grpcAtricleDet)
        {
            if (grpcAtricleDet != null)
            {
                var artBase = grpcAtricleDet.ArticleSummary.ArticleBase;
                var artSummary = grpcAtricleDet.ArticleSummary;

                ArticleDetails bwArticleDetails = new ArticleDetails()
                {
                    ArticleUrl = artBase.ArticleUrl,
                    Title = artBase.Title,
                    BasicId = artBase.BasicId,
                    CategoryId = (ushort)artSummary.CategoryId,
                    AuthorName = artSummary.AuthorName,
                    Description = artSummary.Description,
                    DisplayDate = Convert.ToDateTime(artSummary.DisplayDate),
                    FacebookCommentCount = artSummary.FacebookCommentCount,
                    HostUrl = artSummary.HostUrl,
                    IsSticky = artSummary.IsSticky,
                    LargePicUrl = artSummary.LargePicUrl,
                    SmallPicUrl = artSummary.SmallPicUrl,
                    OriginalImgUrl = artSummary.OriginalImgUrl,
                    Views = artSummary.Views,
                    AuthorMaskingName = artSummary.AuthorMaskingName,
                    Content=grpcAtricleDet.Content,
                    PrevArticle = ConvertFromGrpcToBikeWale(grpcAtricleDet.PrevArticle),
                    NextArticle=ConvertFromGrpcToBikeWale(grpcAtricleDet.NextArticle),
                    TagsList = ConvertFromGrpcToBikeWale(grpcAtricleDet.TagsList),
                    VehiclTagsList = ConvertFromGrpcToBikeWale(grpcAtricleDet.VehiclTagsList)
                };
                return bwArticleDetails;
            }
            else
                return null;
        }

        public static ArticlePageDetails ConvertFromGrpcToBikeWale(GrpcArticlePageDetails grpcAtricleDet)
        {
            if (grpcAtricleDet != null)
            {
                var artBase = grpcAtricleDet.ArticleSummary.ArticleBase;
                var artSummary = grpcAtricleDet.ArticleSummary;

                ArticlePageDetails bwArticleDetails = new ArticlePageDetails()
                {
                    ArticleUrl = artBase.ArticleUrl,
                    Title = artBase.Title,
                    BasicId = artBase.BasicId,
                    CategoryId = (ushort)artSummary.CategoryId,
                    AuthorName = artSummary.AuthorName,
                    Description = artSummary.Description,
                    DisplayDate = Convert.ToDateTime(artSummary.DisplayDate),
                    FacebookCommentCount = artSummary.FacebookCommentCount,
                    HostUrl = artSummary.HostUrl,
                    IsSticky = artSummary.IsSticky,
                    LargePicUrl = artSummary.LargePicUrl,
                    SmallPicUrl = artSummary.SmallPicUrl,
                    OriginalImgUrl = artSummary.OriginalImgUrl,
                    Views = artSummary.Views,                
                    MainImgCaption=grpcAtricleDet.MainImgCaption,
                    IsMainImageSet=grpcAtricleDet.IsMainImageSet,
                    PageList = ConvertFromGrpcToBikeWale(grpcAtricleDet.PageList),
                    TagsList = ConvertFromGrpcToBikeWale(grpcAtricleDet.TagsList),
                    VehiclTagsList = ConvertFromGrpcToBikeWale(grpcAtricleDet.VehiclTagsList)
                };
                return bwArticleDetails;
            }
            else
                return null;
        }

        public static List<VehicleTag> ConvertFromGrpcToBikeWale(RepeatedField<GrpcVehicleTag> data)
        {
            List<VehicleTag> retData = new List<VehicleTag>();
            VehicleTag curVehicleTag;
            foreach (var curGrpcVehicleTag in data)
            {
                curVehicleTag = new VehicleTag()
                {
                    MakeBase = ConvertFromGrpcToBikeWale(curGrpcVehicleTag.MakeBase),
                    ModelBase = ConvertFromGrpcToBikeWale(curGrpcVehicleTag.ModelBase),
                    VersionBase = ConvertFromGrpcToBikeWale(curGrpcVehicleTag.VersionBase)
                };

                retData.Add(curVehicleTag);
            }

            return retData;
        }

        public static List<Page> ConvertFromGrpcToBikeWale(RepeatedField<GrpcPage> data)
        {
            List<Page> retData = new List<Page>();
            Page curPage;
            foreach (var curGrpcPage in data)
            {
                curPage = new Page()
                {
                    Content=curGrpcPage.Content,
                    pageId=curGrpcPage.PageId,
                    PageName=curGrpcPage.PageName,
                    Priority=(ushort)curGrpcPage.Priority
                };

                retData.Add(curPage);
            }

            return retData;
        }

        public static ArticleBase ConvertFromGrpcToBikeWale(GrpcArticleBase grpcArtBase)
        {
            if (grpcArtBase != null)
            {
                ArticleBase bwArticleBase = new ArticleBase()
                {
                    ArticleUrl=grpcArtBase.ArticleUrl,
                    BasicId = grpcArtBase.BasicId,
                    Title = grpcArtBase.Title
                };
                return bwArticleBase;
            }
            else
                return null;
        }

        public static List<string> ConvertFromGrpcToBikeWale(RepeatedField<string> lstString)
        {
            List<string> retList = new List<string>();
            if (lstString != null)
            {
                retList.AddRange(lstString);                
            }
            return retList;
        }
    }
}