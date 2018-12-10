using Carwale.DTOs.CMS.ThreeSixtyView;
using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Entity.CMS;
using Carwale.Entity.Enum;
using Carwale.Interfaces;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.CMS;
using Carwale.Interfaces.NewCars;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using System;
using System.Collections.Generic;

namespace Carwale.BL.CMS
{
    public class ThreeSixtyViewAdapter : IServiceAdapterV2
    {
        private readonly ICarMakesCacheRepository _carMakesCacheRepo;
        private readonly IMediaBL _mediaBL;
        private readonly IVideosBL _videosBL;
        private readonly IThreeSixtyCache _cacheThreeSixty;
        private readonly string _hotspotDetailsCacheKey = "hotspots_details_{0}_{1}";
        private readonly string _hotspotCacheKey = "hotspots_{0}_{1}";

        public ThreeSixtyViewAdapter(ICarMakesCacheRepository carMakesCacheRepo, IMediaBL mediaBL, IVideosBL videosBL, IThreeSixtyCache cacheThreeSixty)
        {
            _carMakesCacheRepo = carMakesCacheRepo;
            _videosBL = videosBL;
            _mediaBL = mediaBL;
            _cacheThreeSixty = cacheThreeSixty;
        }
        public T Get<T, U>(U input)
        {
            return (T)Convert.ChangeType(GetThreeSixtyDTO<U>(input), typeof(T));

        }

        private ThreeSixtyDTO GetThreeSixtyDTO<U>(U input)
        {
            ThreeSixtyDTO threeSixtyDTO = new ThreeSixtyDTO();
            try
            {
                Tuple<CarModelDetails, ThreeSixtyViewCategory> inputTuple = (Tuple<CarModelDetails, ThreeSixtyViewCategory>)Convert.ChangeType(input, typeof(U));

                CarModelDetails modelDetails = inputTuple.Item1;
                ThreeSixtyViewCategory category = inputTuple.Item2;

                string exteriorHotspotsCacheKey = string.Format(_hotspotDetailsCacheKey, modelDetails.ModelId, (int)ThreeSixtyViewCategory.Closed);
                string openHotspotsCacheKey = string.Format(_hotspotDetailsCacheKey, modelDetails.ModelId, (int)ThreeSixtyViewCategory.Open);
                string interiorHotspotsCacheKey = string.Format(_hotspotDetailsCacheKey, modelDetails.ModelId, (int)ThreeSixtyViewCategory.Interior);

                string exteriorXmlVersion = string.Format(_hotspotCacheKey, modelDetails.ModelId, (int)ThreeSixtyViewCategory.Closed);
                string openXmlVersion = string.Format(_hotspotCacheKey, modelDetails.ModelId, (int)ThreeSixtyViewCategory.Open);
                string interiorXmlVersion = string.Format(_hotspotCacheKey, modelDetails.ModelId, (int)ThreeSixtyViewCategory.Interior);

                var multiGetHotspots = _cacheThreeSixty.MultiGetHotspots(modelDetails.ModelId);
                var multiGetXmlVersions = _cacheThreeSixty.MultiGetXmlVersions(modelDetails.ModelId);

                threeSixtyDTO.ModelDetails = modelDetails;
                threeSixtyDTO.ActiveState = category.ToString().ToLower();
                threeSixtyDTO.BreadcrumbEntitylist = GetBreadCrumbList(threeSixtyDTO.ModelDetails);
                threeSixtyDTO.ModelImagesList = _mediaBL.GetModelImagesSlug(modelDetails.ModelId);
                threeSixtyDTO.ModelVideos = _videosBL.GetVideosByModelId(modelDetails.ModelId, CMSAppId.Carwale, 1, 5);
                threeSixtyDTO.ThreeSixtyMakes = _carMakesCacheRepo.GetCarMakesByType("360");
                threeSixtyDTO.ExteriorHotspots = multiGetHotspots.ContainsKey(exteriorHotspotsCacheKey) ? multiGetHotspots[exteriorHotspotsCacheKey] : null;
                threeSixtyDTO.OpenHotspots = multiGetHotspots.ContainsKey(openHotspotsCacheKey) ? multiGetHotspots[openHotspotsCacheKey] : null;
                threeSixtyDTO.InteriorHotspots = multiGetHotspots.ContainsKey(interiorHotspotsCacheKey) ? multiGetHotspots[interiorHotspotsCacheKey] : null;
                threeSixtyDTO.XmlVersion = new Dictionary<ThreeSixtyViewCategory, string>();
                threeSixtyDTO.XmlVersion.Add(ThreeSixtyViewCategory.Closed, (multiGetXmlVersions.ContainsKey(exteriorXmlVersion) && !string.IsNullOrEmpty(multiGetXmlVersions[exteriorXmlVersion].ImageVersion) ? multiGetXmlVersions[exteriorXmlVersion].ImageVersion : DateTime.Now.ToString("yyyyMMddhhmmss")));
                threeSixtyDTO.XmlVersion.Add(ThreeSixtyViewCategory.Open, (multiGetXmlVersions.ContainsKey(openXmlVersion) && !string.IsNullOrEmpty(multiGetXmlVersions[openXmlVersion].ImageVersion) ? multiGetXmlVersions[openXmlVersion].ImageVersion : DateTime.Now.ToString("yyyyMMddhhmmss")));
                threeSixtyDTO.XmlVersion.Add(ThreeSixtyViewCategory.Interior, (multiGetXmlVersions.ContainsKey(interiorXmlVersion) && !string.IsNullOrEmpty(multiGetXmlVersions[interiorXmlVersion].ImageVersion) ? multiGetXmlVersions[interiorXmlVersion].ImageVersion : DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return threeSixtyDTO;
        }

        private List<BreadcrumbEntity> GetBreadCrumbList(CarModelDetails modelDetails)
        {
            try
            {
                string makeName = Format.FormatSpecial(modelDetails.MakeName);

                List<BreadcrumbEntity> breadcrumbEntitylist = new List<BreadcrumbEntity>();

                breadcrumbEntitylist.Add(new BreadcrumbEntity { Title = string.Format("{0} Cars", modelDetails.MakeName), Link = string.Format("/{0}-cars/", makeName), Text = modelDetails.MakeName });
                breadcrumbEntitylist.Add(new BreadcrumbEntity { Title = string.Format("{0} {1}", modelDetails.MakeName, modelDetails.ModelName), Link = string.Format("/{0}-cars/{1}/", makeName, modelDetails.MaskingName), Text = modelDetails.ModelName });
                breadcrumbEntitylist.Add(new BreadcrumbEntity { Title = null, Link = null, Text = "360° View" });

                return breadcrumbEntitylist;
            }
            catch (Exception e)
            {
                Logger.LogException(e);
                return null;
            }
        }
    }
}
