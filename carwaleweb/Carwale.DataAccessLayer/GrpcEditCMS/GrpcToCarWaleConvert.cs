using Carwale.Entity;
using Carwale.Entity.Author;
using Carwale.Entity.CarData;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.Articles;
using Carwale.Entity.CMS.Media;
using Carwale.Entity.CMS.Photos;
using Carwale.Notifications;
using Carwale.Utility;
using EditCMSWindowsService.Messages;
using Google.Protobuf.Collections;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Carwale.BL.GrpcFiles
{
    /// <summary>
    /// Author: Prasad Gawde
    /// Summary: This class is responsible for converting the GRPC Object into the Format Bikewale needs it to be in for further processing
    /// </summary>
    public class GrpcToCarwaleConvert
    {
        
        public static CMSContent ConvertFromGrpcToCarwale(GrpcCMSContent data,string categoryList)
        {
            if (data == null)
                return (new CMSContent());

            try
            {
                CMSContent dataNew = new CMSContent();
                dataNew.RecordCount = data.RecordCount;
                dataNew.Articles = new List<ArticleSummary>();

                foreach (var item in data.Articles.LstGrpcArticleSummary)
                {
                    var curArt = ConvertFromGrpcToCarwale(item);                    
                    dataNew.Articles.Add(curArt);
                }

                if (data.LstGrpcCatArticle != null && data.LstGrpcCatArticle.Count > 0)
                {
                    List<int> all = new List<int>();
                    List<int> currentCats = string.IsNullOrEmpty(categoryList) ? new List<int>() : categoryList.Split(',').Select(int.Parse).ToList(); ;
                    all.AddRange(CWConfiguration.NewsCategories);
                    all.AddRange(CWConfiguration.ExpertCategories);
                    all.AddRange(CWConfiguration.FeatureCategories);

                    foreach (var catarticle in data.LstGrpcCatArticle)
                    {
                        int catId = (int)catarticle.CategoryId;
                        int catIdValue = (int)catarticle.RecordCount;                       
                        if (currentCats.Contains(catId)) dataNew.RecordCount += (uint)catIdValue;
                        if (all.Contains(catId)) dataNew.NewsRecordCount += catIdValue;
                        if (CWConfiguration.ExpertCategories.Contains(catId)) dataNew.ExpertReviewsRecordCount += catIdValue;
                        if (CWConfiguration.FeatureCategories.Contains(catId)) dataNew.FeaturesRecordCount += catIdValue;
                    }
                }
                return dataNew;
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcToCarwaleConvert ConvertFromGrpcToCarwale(GrpcCMSContent,string) Exception");
                objErr.LogException();
                throw e;
            }

        }

        public static ArticleSummary ConvertFromGrpcToCarwale(GrpcArticleSummary data)
        {
            try
            {
                var curArt = new ArticleSummary();

                if (data.ArticleBase != null)
                {
                    curArt.ArticleUrl = data.ArticleBase.ArticleUrl;
                    curArt.BasicId = data.ArticleBase.BasicId;
                    curArt.Title = data.ArticleBase.Title;
                }
                curArt.AuthorName = data.AuthorName;
                curArt.AuthorMaskingName = data.AuthorMaskingName;
                curArt.CategoryId = (ushort)data.CategoryId;
                curArt.CategoryMaskingName = data.CategoryMaskingName;
                curArt.Description = data.Description;
                curArt.DisplayDate = CustomParser.parseDateObject<string>(data.DisplayDate);
                curArt.FacebookCommentCount = data.FacebookCommentCount;
                curArt.HostUrl = CWConfiguration._imgHostUrl;
                curArt.IsFeatured = data.IsFeatured;
                curArt.IsSticky = data.IsSticky;
                curArt.LargePicUrl = data.LargePicUrl;
                curArt.OriginalImgUrl = data.OriginalImgUrl;
                curArt.SmallPicUrl = data.SmallPicUrl;
                curArt.Views = data.Views;
                curArt.SubCategory = data.SubCategory;
                curArt.MakeName = data.MakeName;
                curArt.MaskingName = data.MaskingName;
                curArt.AuthorId = data.AuthorId;
                return curArt;
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcToCarwaleConvert ConvertFromGrpcToCarwale(GrpcArticleSummary) Exception");
                objErr.LogException();
                throw e;
            }
        }

        public static bool ConvertFromGrpcToCarwale(GrpcBool data)
        {
            if (data == null) return false;

            return data.BoolOutput;
        }
        public static List<MakeAndModelDetail> ConvertFromGrpcToCarwale(GrpcMakeAndModelDetailList data)
        {
            if (data == null) return new List<MakeAndModelDetail>();
            try
            {
                var result = new List<MakeAndModelDetail>();
                foreach (var makemodel in data.LstGrpcMakeAndModelDetail)
                {
                    result.Add(ConvertFromGrpcToCarwale(makemodel));
                }
                return result;
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcToCarwaleConvert ConvertFromGrpcToCarwale(GrpcMakeAndModelDetailList) Exception");
                objErr.LogException();
                throw e;
            }
        }
        public static MakeAndModelDetail ConvertFromGrpcToCarwale(GrpcMakeAndModelDetail data)
        {
            if (data == null) return new MakeAndModelDetail();
            try
            {
                return new MakeAndModelDetail() { MaskingName = data.MaskingName, Text = data.Text, Value = (uint)data.Value };
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcToCarwaleConvert ConvertFromGrpcToCarwale(GrpcMakeAndModelDetail) Exception");
                objErr.LogException();
                throw e;
            }
                
        }
        public static CarSynopsisEntity ConvertFromGrpcToCarwale(GrpcCarSynopsis data)
        {
            if (data == null) return (new CarSynopsisEntity());
            try
            {
                return new CarSynopsisEntity()
                {
                    Description = data.Description,
                    ModelId = data.ModelId,
                    Content = data.Content,
                    PageName = data.PageName,
                    Priority = data.Priority
                };
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcToCarwaleConvert ConvertFromGrpcToCarwale(GrpcCarSynopsis) Exception");
                objErr.LogException();
                throw e;
            }
        }
        public static List<CMSSubCategoryV2> ConvertFromGrpcToCarwale(GrpcCMSSubCategoryList data)
        {
            if (data == null) return (new List<CMSSubCategoryV2>());
            try
            {
                var categoryList = new List<CMSSubCategoryV2>();
                foreach (var category in data.LstGrpcCMSSubCategory)
                {
                    categoryList.Add(
                        new CMSSubCategoryV2()
                        {
                            RecordCount = category.RecordCount,
                            SubCategoryId = category.SubCategoryId,
                            SubCategoryName = category.SubCategoryName
                        });
                }
                return categoryList;
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcToCarwaleConvert ConvertFromGrpcToCarwale(GrpcCMSSubCategoryList) Exception");
                objErr.LogException();
                throw e;
            }
        }

        public static List<ArticleSummary> ConvertFromGrpcToCarwale(GrpcArticleSummaryList data)
        {
            if (data == null)
                return (new List<ArticleSummary>());
            try
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
                        DisplayDate = CustomParser.parseDateObject(curGrpcArticleSummary.DisplayDate),
                        FacebookCommentCount = curGrpcArticleSummary.FacebookCommentCount,
                        HostUrl = curGrpcArticleSummary.HostUrl,
                        IsSticky = curGrpcArticleSummary.IsSticky,
                        LargePicUrl = curGrpcArticleSummary.LargePicUrl,
                        OriginalImgUrl = curGrpcArticleSummary.OriginalImgUrl,
                        SmallPicUrl = curGrpcArticleSummary.SmallPicUrl,
                        Title = curGrpcArticleSummary.ArticleBase.Title,
                        Views = curGrpcArticleSummary.Views,
                        Tags= curGrpcArticleSummary.Tag,
                        CategoryMaskingName=curGrpcArticleSummary.CategoryMaskingName,
                        SubCategory=curGrpcArticleSummary.SubCategory,
                        IsFeatured = curGrpcArticleSummary.IsFeatured,
                        AuthorId = curGrpcArticleSummary.AuthorId
                    };

                    retData.Add(curArticleSummary);
                }

                return retData;
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcToCarwaleConvert ConvertFromGrpcToCarwale(GrpcArticleSummaryList) Exception");
                objErr.LogException();
                throw e;
            }
        }
        public static List<RelatedArticles> ConvertFromGrpcToCarwale(GrpcRelatedArticlesList data)
        {
            if (data == null) return (new List<RelatedArticles>());
            try
            {
                var relatedList = new List<RelatedArticles>();
                foreach (var article in data.LstGrpcRelatedArticles)
                {
                    relatedList.Add(new RelatedArticles()
                    {
                        BasicId = article.BasicId,
                        CategoryId = article.CategoryId,
                        CategoryMaskingName = article.CategoryMaskingName,
                        ParentCatId = article.ParentCatId
                    });
                }
                return relatedList;
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcToCarwaleConvert ConvertFromGrpcToCarwale(GrpcRelatedArticlesList) Exception");
                objErr.LogException();
                throw e;
            }
        }
        public static List<VideosEntity> ConvertFromGrpcToCarwale(GrpcVideosEntityList data)
        {
            if (data == null) return (new List<VideosEntity>());
            try
            {
                var videos = new List<VideosEntity>();
                foreach (var grpcvideo in data.LstGrpcVideosEntity)
                {
                    videos.Add(new VideosEntity() {
                        BasicId = grpcvideo.BasicId,
                        Description = grpcvideo.Description,
                        IsSticky = grpcvideo.IsSticky,
                        Likes = grpcvideo.Likes,
                        MakeName = grpcvideo.MakeName,
                        MaskingName = grpcvideo.MaskingName,
                        ModelName = grpcvideo.ModelName,
                        Popularity = grpcvideo.Popularity,
                        PublishDate = CustomParser.parseDateObject(grpcvideo.PublishDate),
                        SubCatId = grpcvideo.SubCatId,
                        SubCatName = grpcvideo.SubCatName,
                        Title = grpcvideo.Title,
                        VideoId = grpcvideo.VideoId,
                        VideoSrc = grpcvideo.VideoSrc,
                        Views = grpcvideo.Views
                    });
                }
                return videos;
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcToCarwaleConvert ConvertFromGrpcToCarwale(GrpcVideosEntityList) Exception");
                objErr.LogException();
                throw e;
            }
        }
        public static List<ModelImage> ConvertFromGrpcToCarwale(GrpcModelImageList data)
        {
            if (data == null)
                return (new List<ModelImage>());
            try
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
                        MakeBase = ConvertFromGrpcToCarwale(curGrpcModelImage.MakeBase),
                        ModelBase = ConvertFromGrpcToCarwale(curGrpcModelImage.ModelBase),
                        OriginalImgPath = curGrpcModelImage.OriginalImgPath
                    };


                    retData.Add(curModelImage);
                }

                return retData;
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcToCarwaleConvert ConvertFromGrpcToCarwale(GrpcModelImageList) Exception");
                objErr.LogException();
                throw e;
            }
        }

        public static PhotosListing ConvertFromGrpcToCarwale(GrpcPopularModelPhotosData data)
        {
            if (data == null) return (new PhotosListing());
            try
            {
                PhotosListing photos = new PhotosListing();
                photos.ImageRecordCount = Convert.ToUInt32(data.RecordCount);
                photos.PhotosList = ConvertFromGrpcToCarwale(data.ModelPhotoList.LstGrpcModelPhotos);
                return photos;
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcToCarwaleConvert ConvertFromGrpcToCarwale(GrpcPopularModelPhotosData) Exception");
                objErr.LogException();
                throw e;
            }
        }

        public static VideoListing ConvertFromGrpcToCarwale(GrpcPopularModelVideoData data)
        {
            if (data == null) return (new VideoListing());
            try
            {
                VideoListing videos = new VideoListing();
                videos.VideoRecordCount = Convert.ToUInt32(data.RecordCount);
                videos.VideosList = ConvertFromGrpcToCarwale(data.ModelVideoList.LstGrpcModelVideo);
                return videos;
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcToCarwaleConvert ConvertFromGrpcToCarwale(GrpcPopularModelVideoData) Exception");
                objErr.LogException();
                throw e;
            }
        }

        public static List<ModelPhotos> ConvertFromGrpcToCarwale(RepeatedField<GrpcModelPhotos> photoList)
        {
            if (photoList == null) return (new List<ModelPhotos>());
            try
            {
                var modelPhotoList = new List<ModelPhotos>();

                foreach (var photo in photoList)
                {
                    var modelPhoto = new ModelPhotos()
                    {
                        GalleryImagePath = photo.GalleryImagePath,
                        HostURL = photo.HostURL,
                        OriginalImgPath = photo.OriginalImgPath,
                        ImageCount = Convert.ToInt32(photo.ImageCount),
                        IsFuturistic = photo.MakeModelEntity.IsFuturistic,
                        IsNew = photo.MakeModelEntity.IsNew,
                        MakeId = photo.MakeModelEntity.MakeId,
                        MakeName = photo.MakeModelEntity.MakeName,
                        ModelId = photo.MakeModelEntity.ModelId,
                        ModelMaskingName = photo.MakeModelEntity.ModelMaskingName,
                        ModelName = photo.MakeModelEntity.ModelName,
                        Is360ExteriorAvailable = photo.MakeModelEntity.Is360ExteriorAvailable,
                        Is360OpenAvailable = photo.MakeModelEntity.Is360OpenAvailable,
                        Is360InteriorAvailable = photo.MakeModelEntity.Is360InteriorAvailable
                    };
                    modelPhotoList.Add(modelPhoto);
                }
                return modelPhotoList;
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcToCarwaleConvert ConvertFromGrpcToCarwale(RepeatedField<GrpcModelPhotos>) Exception");
                objErr.LogException();
                throw e;
            }
        }

        public static List<ModelVideo> ConvertFromGrpcToCarwale(RepeatedField<GrpcModelVideo> videoList)
        {
            if (videoList == null) return (new List<ModelVideo>());
            try
            {
                var modelVideosList = new List<ModelVideo>();

                foreach (var video in videoList)
                {
                    var modelvideo = new ModelVideo
                    {
                        BasicId = video.BasicId,
                        SubCategoryName = video.SubCategoryName,
                        VideoCount = video.VideoCount,
                        VideoId = video.VideoId,
                        IsFuturistic = video.MakeModelEntity.IsFuturistic,
                        IsNew = video.MakeModelEntity.IsNew,
                        MakeId = video.MakeModelEntity.MakeId,
                        MakeName = video.MakeModelEntity.MakeName,
                        ModelId = video.MakeModelEntity.ModelId,
                        ModelMaskingName = video.MakeModelEntity.ModelMaskingName,
                        ModelName = video.MakeModelEntity.ModelName,
                        Title = video.Title,
                        VideoUrl = video.VideoUrl,
                        Duration = video.Duration,
                        DisplayDate = CustomParser.parseDateObject(video.DisplayDate),
                        Views = video.Views
                    };
                    modelVideosList.Add(modelvideo);
                }
                return modelVideosList;
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcToCarwaleConvert ConvertFromGrpcToCarwale(RepeatedField<GrpcModelVideo>) Exception");
                objErr.LogException();
                throw e;
            }
        }

        public static CarMakeEntityBase ConvertFromGrpcToCarwale(GrpcCarMakeEntityBase grpcMake)
        {
            if (grpcMake != null)
            {
                CarMakeEntityBase bwMake = new CarMakeEntityBase()
                {
                    MakeId = grpcMake.MakeId,
                    MakeName = grpcMake.MakeName
                };
                return bwMake;
            }
            else
                return (new CarMakeEntityBase());
        }

        public static CarModelEntityBase ConvertFromGrpcToCarwale(GrpcCarModelEntityBase grpcModel)
        {
            if (grpcModel != null)
            {
                CarModelEntityBase bwModel = new CarModelEntityBase()
                {
                    ModelId = grpcModel.ModelId,
                    ModelName = grpcModel.ModelName,
                    MaskingName = grpcModel.MaskingName
                };
                return bwModel;
            }
            else
                return (new CarModelEntityBase());
        }

        public static CarVersionEntity ConvertFromGrpcToCarwale(GrpcCarVersionEntity grpcVersion)
        {
            if (grpcVersion != null)
            {
                CarVersionEntity bwVersion = new CarVersionEntity()
                {
                    ID = grpcVersion.Id,
                    Name = grpcVersion.Name,
                    MaskingName = grpcVersion.MaskingName
                };
                return bwVersion;
            }
            else
                return (new CarVersionEntity());
        }

        public static ArticleDetails ConvertFromGrpcToCarwale(GrpcArticleDetails grpcAtricleDet)
        {
            if (grpcAtricleDet == null)
                return null;
            if (grpcAtricleDet.ArticleSummary != null)
            {
                try
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
                        DisplayDate = CustomParser.parseDateObject(artSummary.DisplayDate),
                        FacebookCommentCount = artSummary.FacebookCommentCount,
                        HostUrl = artSummary.HostUrl,
                        IsSticky = artSummary.IsSticky,
                        LargePicUrl = artSummary.LargePicUrl,
                        SmallPicUrl = artSummary.SmallPicUrl,
                        OriginalImgUrl = artSummary.OriginalImgUrl,
                        Views = artSummary.Views,
                        AuthorMaskingName = artSummary.AuthorMaskingName ?? string.Empty,
                        Content = grpcAtricleDet.Content,
                        PrevArticle =  ConvertFromGrpcToCarwale(grpcAtricleDet.PrevArticle),
                        NextArticle = ConvertFromGrpcToCarwale(grpcAtricleDet.NextArticle),
                        TagsList = ConvertFromGrpcToCarwale(grpcAtricleDet.TagsList),
                        VehiclTagsList = ConvertFromGrpcToCarwale(grpcAtricleDet.VehiclTagsList),
                        AuthorId = artSummary.AuthorId,
                        ModifiedDate = CustomParser.parseDateObject(artSummary.ModifiedDate)
                    };
                    return bwArticleDetails;
                }
                catch (Exception e)
                {
                    ExceptionHandler objErr = new ExceptionHandler(e, "GrpcToCarwaleConvert ConvertFromGrpcToCarwale(GrpcArticleDetails) Exception");
                    objErr.LogException();
                    throw e;
                }
            }
            return new ArticleDetails();
        }

        public static List<ContentFeedSummary> ConvertFromGrpcToCarwale(GrpcContentFeedSummaryList data)
        {
            if (data == null) return (new List<ContentFeedSummary>());

            try
            {
                var result = new List<ContentFeedSummary>();
                foreach (var feed in data.LstGrpcContentFeedSummary)
                {
                    result.Add(new ContentFeedSummary() {
                        AuthorName = feed.AuthorName,
                        BasicId = feed.BasicId,
                        Content = feed.Content,
                        Description = feed.Description,
                        DisplayDate = feed.DisplayDate,
                        HostUrl = feed.HostUrl,
                        ImagePathLarge = feed.ImagePathLarge,
                        ImagePathThumbnail = feed.ImagePathThumbnail,
                        IsMainImage = feed.IsMainImage,
                        MainImageSet = feed.MainImageSet,
                        OriginalImgPath = feed.OriginalImgPath,
                        Title = feed.Title,
                        Url= feed.Url,
                        Views = feed.Views
                    });
                }
                return result;
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcToCarwaleConvert ConvertFromGrpcToCarwale(GrpcContentFeedSummaryList) Exception");
                objErr.LogException();
                throw e;
            }
        }

        public static ArticlePageDetails ConvertFromGrpcToCarwale(GrpcArticlePageDetails grpcAtricleDet)
        {
            if (grpcAtricleDet == null)
                return null;
            if (grpcAtricleDet.ArticleSummary != null)
            {
                try
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
                        DisplayDate = CustomParser.parseDateObject(artSummary.DisplayDate),
                        FacebookCommentCount = artSummary.FacebookCommentCount,
                        HostUrl = artSummary.HostUrl,
                        IsSticky = artSummary.IsSticky,
                        LargePicUrl = artSummary.LargePicUrl,
                        SmallPicUrl = artSummary.SmallPicUrl,
                        OriginalImgUrl = artSummary.OriginalImgUrl,
                        Views = artSummary.Views,
                        ShowGallery = grpcAtricleDet.ShowGallery,
                        MainImgCaption = grpcAtricleDet.MainImgCaption,
                        IsMainImageSet = grpcAtricleDet.IsMainImageSet,
                        PageList = ConvertFromGrpcToCarwale(grpcAtricleDet.PageList),
                        TagsList = ConvertFromGrpcToCarwale(grpcAtricleDet.TagsList),
                        VehiclTagsList = ConvertFromGrpcToCarwale(grpcAtricleDet.VehiclTagsList),
                        AuthorMaskingName = artSummary.AuthorMaskingName,
                        CategoryMaskingName = artSummary.CategoryMaskingName,
                        CategoryName = grpcAtricleDet.CategoryName,
                        MakeName = artSummary.MakeName,
                        ModelName = artSummary.ModelName,
                        MaskingName = artSummary.MaskingName,
                        SubCategory = artSummary.SubCategory,
                        AuthorId = artSummary.AuthorId,
                        ModifiedDate = CustomParser.parseDateObject(artSummary.ModifiedDate)

                    };
                    return bwArticleDetails;
                }
                catch (Exception e)
                {
                    ExceptionHandler objErr = new ExceptionHandler(e, "GrpcToCarwaleConvert ConvertFromGrpcToCarwale(GrpcArticlePageDetails) Exception");
                    objErr.LogException();
                    throw e;
                }
            }
            return new ArticlePageDetails();
        }

        public static List<VehicleTag> ConvertFromGrpcToCarwale(RepeatedField<GrpcVehicleTag> data)
        {
            if (data == null)
                return (new List<VehicleTag>());
            try
            {
                List<VehicleTag> retData = new List<VehicleTag>();
                VehicleTag curVehicleTag;
                foreach (var curGrpcVehicleTag in data)
                {
                    curVehicleTag = new VehicleTag()
                    {
                        MakeBase = ConvertFromGrpcToCarwale(curGrpcVehicleTag.MakeBase),
                        ModelBase = ConvertFromGrpcToCarwale(curGrpcVehicleTag.ModelBase),
                        VersionBase = ConvertFromGrpcToCarwale(curGrpcVehicleTag.VersionBase)
                    };

                    retData.Add(curVehicleTag);
                }

                return retData;
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcToCarwaleConvert ConvertFromGrpcToCarwale(RepeatedField<GrpcVehicleTag>) Exception");
                objErr.LogException();
                throw e;
            }
        }

        public static List<Page> ConvertFromGrpcToCarwale(RepeatedField<GrpcPage> data)
        {
            if (data == null)
                return (new List<Page>());
            try
            {
                List<Page> retData = new List<Page>();
                Page curPage;
                foreach (var curGrpcPage in data)
                {
                    curPage = new Page()
                    {
                        Content = curGrpcPage.Content,
                        pageId = curGrpcPage.PageId,
                        PageName = curGrpcPage.PageName,
                        Priority = (ushort)curGrpcPage.Priority
                    };

                    retData.Add(curPage);
                }

                return retData;
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcToCarwaleConvert ConvertFromGrpcToCarwale(RepeatedField<GrpcPage>) Exception");
                objErr.LogException();
                throw e;
            }
        }

        public static ArticleBase ConvertFromGrpcToCarwale(GrpcArticleBase grpcArtBase)
        {
            if (grpcArtBase != null)
            {
                ArticleBase bwArticleBase = new ArticleBase()
                {
                    ArticleUrl = grpcArtBase.ArticleUrl,
                    BasicId = grpcArtBase.BasicId,
                    Title = grpcArtBase.Title
                };
                return bwArticleBase;
            }
            else
                return (new ArticleBase());
        }

        public static List<string> ConvertFromGrpcToCarwale(RepeatedField<string> lstString)
        {
            List<string> retList = new List<string>();
            if (lstString != null)
            {
                retList.AddRange(lstString);
            }
            return retList;
        }

        public static List<AuthorList> ConvertFromGrpcToCarwale(GrpcAuthorList data)
        {
            if (data == null) return (new List<AuthorList>());
            try
            {
                var result = new List<AuthorList>();
                foreach (var author in data.LstGrpcAuthor)
                {
                    result.Add(ConvertFromGrpcToCarwale(author));
                }
                return result;
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcToCarwaleConvert ConvertFromGrpcToCarwale(GrpcAuthorList) Exception");
                objErr.LogException();
                throw e;
            }
        }
        public static AuthorEntity ConvertFromGrpcToCarwale(GrpcAuthor data)
        {
            if (data == null) return (new AuthorEntity());
            try
            {
                return new AuthorEntity()
                {
                    AuthorName = data.AuthorName,
                    Designation = data.Designation,
                    HostUrl = data.HostUrl,
                    ImageName = data.ImageName,
                    MaskingName = data.MaskingName,
                    ProfileImage = data.ProfileImage,
                    ShortDescription = data.ShortDescription,
                    EmailId = data.EmailId,
                    FacebookProfile = data.FacebookProfile,
                    FullDescription = data.FullDescription,
                    GooglePlusProfile = data.GooglePlusProfile,
                    LinkedInProfile = data.LinkedInProfile,
                    TwitterProfile = data.TwitterProfile
                };
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcToCarwaleConvert ConvertFromGrpcToCarwale(GrpcAuthor) Exception");
                objErr.LogException();
                throw e;
            }
        }

        public static List<ExpertReviews> ConvertFromGrpcToCarwale(GrpcAuthorContentList data)
        {
            if (data == null) return (new List<ExpertReviews>());
            try
            {
                var result = new List<ExpertReviews>();
                foreach (var content in data.LstGrpcAuthorContent)
                {
                    result.Add
                    (
                        new ExpertReviews() {
                        BasicId = content.BasicId,
                        CategoryId = content.CategoryId,
                        MakeName = content.MakeName,
                        MaskingName = content.MaskingName,
                        Title = content.Title,
                        Url = content.Url
                    });
                }
                return result;
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcToCarwaleConvert ConvertFromGrpcToCarwale(GrpcAuthorContentList) Exception");
                objErr.LogException();
                throw e;
            }
        }

        public static List<Video> ConvertFromGrpcToCarwale(GrpcVideosList data)
        {
            if (data == null)
                return (new List<Video>());

            try
            {
                List<Video> lstVideos = new List<Video>();
                foreach (var curGrpcVideo in data.LstGrpcVideos)
                {
                    Video curVid = new Video()
                    {
                        BasicId = curGrpcVideo.BasicId,
                        Description = curGrpcVideo.Description,
                        DisplayDate = CustomParser.parseDateObject(curGrpcVideo.DisplayDate),
                        Duration = curGrpcVideo.Duration,
                        ImagePath = curGrpcVideo.ImagePath,
                        ImgHost = curGrpcVideo.ImgHost,
                        Likes = curGrpcVideo.Likes,
                        MakeName = curGrpcVideo.MakeName,
                        MaskingName = curGrpcVideo.MaskingName,
                        ModelName = curGrpcVideo.ModelName,
                        SubCatId = curGrpcVideo.SubCatId,
                        SubCatName = curGrpcVideo.SubCatName,
                        Tags = curGrpcVideo.Tags,
                        ThumbnailPath = curGrpcVideo.ThumbnailPath,
                        VideoId = curGrpcVideo.VideoId,
                        VideoTitle = curGrpcVideo.VideoTitle,
                        VideoTitleUrl = curGrpcVideo.VideoTitleUrl,
                        VideoUrl = curGrpcVideo.VideoUrl,
                        Views = curGrpcVideo.Views
                    };

                    lstVideos.Add(curVid);
                }
                return lstVideos;
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcToCarwaleConvert ConvertFromGrpcToCarwale(GrpcVideosList) Exception");
                objErr.LogException();
                throw e;
            }
        }

        public static int ConvertFromGrpcToCarwale(GrpcInt data)
        {
            if(data == null)
                return 0;

            try
            {
                return data.IntOutput;
            }
            catch(Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcToCarwaleConvert ConvertFromGrpcToCarwale(GrpcInt) Exception");
                objErr.LogException();
                throw e;
            }
        }

        public static string ConvertFromGrpcToCarwale(GrpcString data)
        {
            if (data == null)
                return null;

            try
            {
                return data.StringOutput;
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcToCarwaleConvert ConvertFromGrpcToCarwale(GrpcInt) Exception");
                objErr.LogException();
                throw e;
            }
        }

        public static List<Video> ConvertFromGrpcToCarwale(RepeatedField<GrpcVideo> data)
        {
            if (data == null)
                return (new List<Video>());
            try
            {
                List<Video> lstVideos = new List<Video>();
                foreach (var curGrpcVideo in data)
                {
                    lstVideos.Add(ConvertFromGrpcToCarwale(curGrpcVideo));
                }
                return lstVideos;
            }
            catch(Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcToCarwaleConvert ConvertFromGrpcToCarwale(RepeatedField<GrpcVideo>) Exception");
                objErr.LogException();
                throw e;
            }
        }

        public static Video ConvertFromGrpcToCarwale(GrpcVideo curGrpcVideo)
        {
            if (curGrpcVideo != null)
            {
                try
                {
                    return new Video()
                    {
                        BasicId = curGrpcVideo.BasicId,
                        Description = curGrpcVideo.Description,
                        DisplayDate = CustomParser.parseDateObject(curGrpcVideo.DisplayDate),
                        Duration = curGrpcVideo.Duration,
                        ImagePath = curGrpcVideo.ImagePath,
                        ImgHost = curGrpcVideo.ImgHost,
                        Likes = curGrpcVideo.Likes,
                        MakeName = curGrpcVideo.MakeName,
                        MaskingName = curGrpcVideo.MaskingName,
                        ModelName = curGrpcVideo.ModelName,
                        SubCatId = curGrpcVideo.SubCatId,
                        SubCatName = curGrpcVideo.SubCatName,
                        Tags = curGrpcVideo.Tags,
                        ThumbnailPath = curGrpcVideo.ThumbnailPath,
                        VideoId = curGrpcVideo.VideoId,
                        VideoTitle = curGrpcVideo.VideoTitle,
                        VideoTitleUrl = curGrpcVideo.VideoTitleUrl,
                        VideoUrl = curGrpcVideo.VideoUrl,
                        Views = curGrpcVideo.Views,
                        CategoryId = curGrpcVideo.CategoryId
                    };
                }
                catch (Exception e)
                {
                    ExceptionHandler objErr = new ExceptionHandler(e, "GrpcToCarwaleConvert ConvertFromGrpcToCarwale(GrpcVideo) Exception");
                    objErr.LogException();
                    throw e;
                }
            }
            return (new Video());
        }

        public static VideoListEntity ConvertFromGrpcToCarwale(GrpcVideoListEntity inp)
        {
            if (inp != null)
            {
                VideoListEntity retVal = new VideoListEntity();
                retVal.NextPageUrl = inp.NextPageUrl;
                retVal.PrevPageUrl = inp.PrevPageUrl;
                retVal.TotalRecords = inp.TotalRecords;
                retVal.Videos = ConvertFromGrpcToCarwale(inp.Videos.LstGrpcVideos);

                return retVal;
            }
            return (new VideoListEntity());
        }

        public static CMSImage ConvertFromGrpcToCarwale(GrpcCMSImage data)
        {
            if (data == null)
                return (new CMSImage());

            try
            {
                CMSImage dataNew = new CMSImage();
                dataNew.RecordCount = Convert.ToUInt32(data.RecordCount);
                dataNew.Images = new List<ModelImage>();
                GrpcModelImageList _imageList = new GrpcModelImageList();
                _imageList.LstGrpcModelImage.AddRange(data.LstGrpcModelImage);
                var curArt = ConvertFromGrpcToCarwale(_imageList);
                dataNew.Images.AddRange(curArt);
                return dataNew;
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcToCarwaleConvert ConvertFromGrpcToCarwale(GrpcCMSImage) Exception");
                objErr.LogException();
                throw e;
            }
        }

        public static List<AutoExpoGallerySubCategory> ConvertFromGrpcToCarwale(GrpcAutoExpoGalleryListing data)
        {
            if (data == null)
                return (new List<AutoExpoGallerySubCategory>());

            try
            {
                List<AutoExpoGallerySubCategory> lstSubCat = new List<AutoExpoGallerySubCategory>();
                foreach (var curGrpcSubCat in data.LstGrpcAutoExpoGalleryListing)
                {
                    AutoExpoGallerySubCategory curSubCat = new AutoExpoGallerySubCategory()
                    {
                        Description = curGrpcSubCat.Description,
                        MainImagePath = curGrpcSubCat.MainImagePath,
                        SubCategoryId = curGrpcSubCat.SubCategoryId,
                        SubCategoryName = curGrpcSubCat.SubCategoryName,
                        ImageCount = curGrpcSubCat.ImageCount,
                        Url = curGrpcSubCat.Url,
                        IsFeatured = curGrpcSubCat.IsFeatured,
                        Title = curGrpcSubCat.Title
                    };

                    lstSubCat.Add(curSubCat);
                }
                return lstSubCat;
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcToCarwaleConvert ConvertFromGrpcToCarwale(GrpcAutoExpoGalleryListing) Exception");
                objErr.LogException();
                throw e;
            }
        }

        public static List<AutoExpoGalleryImage> ConvertFromGrpcToCarwale(GrpcAutoExpoGalleryDetails data)
        {
            if (data == null)
                return (new List<AutoExpoGalleryImage>());

            try
            {
                List<AutoExpoGalleryImage> lstImage = new List<AutoExpoGalleryImage>();
                foreach (var curGrpcImg in data.LstGrpcAutoExpoGalleryImage)
                {
                    AutoExpoGalleryImage curImage = new AutoExpoGalleryImage()
                    {
                        Title = curGrpcImg.Title,
                        Name = curGrpcImg.Name,
                        Url = curGrpcImg.Url
                    };

                    lstImage.Add(curImage);
                }
                return lstImage;
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcToCarwaleConvert ConvertFromGrpcToCarwale(GrpcAutoExpoGalleryDetails) Exception");
                objErr.LogException();
                throw e;
            }
        }

        public static List<CMSImage> ConvertFromGrpcToCarwale(GrpcModelsImageList data)
        {
            if (data == null)
                return (new List<CMSImage>());
            
            List<CMSImage> cmsImages = new List<CMSImage>();
            foreach (var curGrpcImg in data.LstGrpcModelImaegs)
            {
                CMSImage curImage = new CMSImage()
                {
                    RecordCount = Convert.ToUInt32(curGrpcImg.RecordCount),
                    Images = ConvertFromGrpcToCarwale(curGrpcImg)
                };

                cmsImages.Add(curImage);
            }
            return cmsImages;
        }
    }
}