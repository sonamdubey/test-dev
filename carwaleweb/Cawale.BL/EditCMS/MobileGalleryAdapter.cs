using Carwale.DTOs.CarData;
using Carwale.DTOs.CMS.Media;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.URIs;
using Carwale.Entity.Enum;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.CMS;
using Carwale.Interfaces.CMS.Articles;
using Carwale.Interfaces.CMS.Photos;
using Carwale.Interfaces.NewCars;
using Carwale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Carwale.BL.EditCMS
{
    public class MobileGalleryAdapter : IServiceAdapterV2
    {
        private readonly ICMSContent _contentBL;
        private readonly IPhotos _photosCacheRepo;
        private readonly IVideosBL _videosRepository;
        private readonly ICarModelCacheRepository _carModelsCache;
        private readonly IMediaBL _mediaBL;
        private readonly IVideosBL _video;
        public MobileGalleryAdapter(ICMSContent contentBL, IPhotos photosCacheRepo, IVideosBL videosRepository, ICarModelCacheRepository carModelsCache, IMediaBL mediaBL, IVideosBL video)
        {
            _contentBL = contentBL;
            _photosCacheRepo = photosCacheRepo;
            _videosRepository = videosRepository;
            _carModelsCache = carModelsCache;
            _mediaBL = mediaBL;
            _video = video;
        }
        public T Get<T, U>(U input)
        {
            return (T)Convert.ChangeType(GetGalleryDTO(input), typeof(T));
        }
        public PhotoGalleryDTO_V2 GetGalleryDTO<U>(U queryParameters)
        {
            var galleryDTO = new PhotoGalleryDTO_V2();
            try
            {
                Dictionary<string, string> queryString = (Dictionary<string, string>)Convert.ChangeType(queryParameters, typeof(U));
                var section = queryString.Keys.Contains("section") ? queryString["section"] : "";
                bool isPartial = queryString.Keys.Contains("isPartial") && queryString["isPartial"] == "true";
                int modelId;
                if (queryString.Keys.Contains("modelId"))
                    int.TryParse(queryString["modelId"], out modelId);
                else
                    modelId = 0;
                GalleryStateDTO galleryState = new GalleryStateDTO();

                switch (section)
                {
                    case "images":
                        galleryState.ActiveSection = GallerySections.Photos;
                        break;
                    case "videos":
                        galleryState.ActiveSection = GallerySections.VideosSection;
                        break;
                    case "colors":
                        galleryState.ActiveSection = GallerySections.ColorSection;
                        break;
                }

                int videoId = 0;
                if (queryString.Keys.Contains("videos") && bool.Parse(queryString["videos"]))
                {
                    galleryState.ActiveSection = GallerySections.VideosSection;
                    if (queryString.Keys.Contains("videoName"))
                    {
                        string videoName = queryString["videoName"];
                        string[] tokens = videoName.Split('-');
                        int.TryParse(tokens[tokens.Length - 1], out videoId);
                    }
                }

                if (modelId > 0 || galleryState.ActiveSection == GallerySections.VideosSection)
                {
                    ModelPhotosBycountURI galleryUri = new ModelPhotosBycountURI();
                    galleryUri.ApplicationId = (int)CMSAppId.Carwale;
                    galleryUri.ModelId = modelId;
                    if (string.IsNullOrWhiteSpace(galleryUri.CategoryIdList))
                        galleryUri.CategoryIdList = "8,10";
                    var filter = queryString.Keys.Contains("filter") ? queryString["filter"] : null;
                    galleryDTO.ModelVideos = new List<Video>();
                    if (modelId > 0)
                    {
                        galleryDTO.ModelImages = _photosCacheRepo.GetModelPhotosByCount(galleryUri);
                        galleryDTO.ModelColors = _carModelsCache.GetModelColorsByModel(modelId);
                        galleryDTO.ShowModelColors = _mediaBL.IsModelColorPhotosPresent(galleryDTO.ModelColors);
                        galleryDTO.ModelVideos = _videosRepository.GetVideosByModelId(galleryUri.ModelId, CMSAppId.Carwale, 0, 0);
                    }
                    else
                    {
                        galleryDTO.ModelVideos.Add(_videosRepository.GetVideoByBasicId(videoId, CMSAppId.Carwale));
                    }

                    if (galleryState.ActiveSection == GallerySections.VideosSection)
                    {
                        galleryDTO.RecommendedVideos = GetRecommendedVideos(modelId);
                    }

                    if (galleryDTO.ModelImages != null && galleryDTO.ModelImages.Count > 0)
                    {
                        int length = galleryDTO.ModelImages.Count;
                        for (int i = 0; i < (length + 1) / 2; i++)
                        {
                            if (galleryDTO.ModelImages[i].MainImgCategoryId == (int)ImageTypes.Interior || galleryDTO.ModelImages[length - i - 1].MainImgCategoryId == (int)ImageTypes.Interior)
                                galleryDTO.IsInteriorPresent = true;
                            if (galleryDTO.ModelImages[i].MainImgCategoryId == (int)ImageTypes.Exterior || galleryDTO.ModelImages[length - i - 1].MainImgCategoryId == (int)ImageTypes.Exterior)
                                galleryDTO.IsExteriorPresent = true;
                            if (galleryDTO.IsInteriorPresent && galleryDTO.IsExteriorPresent)
                                break;
                        }
                    }
                    
                    switch (filter)
                    {
                        case "interior":
                            galleryState.ActiveFilter = GalleryFilters.InteriorPhotos;
                            galleryDTO.ModelImages = galleryDTO.ModelImages.Where(x => x.MainImgCategoryId == 1).ToList();
                            break;
                        case "exterior":
                            galleryState.ActiveFilter = GalleryFilters.ExteriorPhotos;
                            galleryDTO.ModelImages = galleryDTO.ModelImages.Where(x => x.MainImgCategoryId == 2).ToList();
                            break;
                        default:
                            galleryState.ActiveFilter = GalleryFilters.AllPhotos;
                            break;
                    }
                    if (queryString.Keys.Contains("imageName"))
                    {
                        LoadSpecificImage(galleryDTO, queryString["imageName"], galleryState);
                    }
                    else if ((queryString.Keys.Contains("videoName") || queryString.Keys.Contains("videos")) && galleryDTO.ModelVideos.Count > 0)
                    {
                        if (videoId > 0)
                        {
                            var videoIndex = galleryDTO.ModelVideos.Find(x => x.BasicId == videoId);
                            if(videoIndex != null)
                            {
                                galleryDTO.ModelVideos.Remove(videoIndex);
                                galleryDTO.ModelVideos.Insert(0, videoIndex);
                            }
                            
                            galleryState.IsSpecificUrl = true;
                        }
                    }
                    galleryDTO.GalleryState = galleryState;
                    galleryDTO.ModelDetails = _carModelsCache.GetModelDetailsById(modelId);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "GalleryController.Index()\n Exception : " + ex.Message);
                objErr.LogException();
            }
            return galleryDTO;
        }

        private static void LoadSpecificImage(PhotoGalleryDTO_V2 galleryDTO, string imageName, GalleryStateDTO galleryState)
        {
            galleryState.IsSpecificUrl = true;
            string[] tokens = imageName.Split('-');
            int imageId;
            int.TryParse(Regex.Replace(tokens[tokens.Length - 1], @"_\w+", string.Empty), out imageId);
            if (galleryState.ActiveSection == GallerySections.ColorSection)
            {
                galleryState.ActiveSlideIndex = (galleryDTO.ModelColors != null && galleryDTO.ModelColors.Count > 0) ? galleryDTO.ModelColors.FindIndex(x => x.ColorId == imageId) : 0;
            }
            else if(galleryState.ActiveSection == GallerySections.Photos)
            {
                galleryState.ActiveSlideIndex = (galleryDTO.ModelImages != null && galleryDTO.ModelImages.Count > 0) ? galleryDTO.ModelImages.FindIndex(x => x.ImageId == imageId) : 0;
                galleryState.ActiveSection = GallerySections.Photos;
            }
        }



        private List<Video> GetRecommendedVideos(int modelId)
        {
            List<Video> videos = null;
            try
            {
                if (modelId > 0)
                {
                    ArticleByCatURI queryString = new ArticleByCatURI() { ApplicationId = (int)CMSAppId.Carwale, ModelId = modelId };
                    videos = _video.GetSimilarVideos(queryString);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "MobileGalleryAdapter.GetSimilarVideosByModel()");
                objErr.LogException();
            }
            return videos;
        }
    }
}
