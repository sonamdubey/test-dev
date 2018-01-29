using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Schema;
using Bikewale.Models.Photos;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Models.Photos
{
    /// <summary>
    /// Created By  : Rajan Chauhan on 29 Jan 2018
    /// Description : To bind Model Photos Page 
    /// </summary>
    public class MakePhotosPage
    {
        private readonly IBikeMakesCacheRepository _objMakeCache = null;
        private readonly IBikeModels<BikeModelEntity, int> _objModelEntity = null;
        public StatusCodes Status { get; set; }
        public MakeMaskingResponse objResponse { get; set; }
        public string RedirectUrl { get; set; }
        public uint _makeId;
        public bool IsMobile { get; set; }
        

        public MakePhotosPage(bool isMobile, string makeMaskingName, IBikeModels<BikeModelEntity, int> objModelEntity, IBikeMakesCacheRepository objMakeCache)
        {
            IsMobile = isMobile;
            _objModelEntity = objModelEntity;
            _objMakeCache = objMakeCache;
            ProcessQuery(makeMaskingName);
        }

        public MakePhotosPageVM GetData()
        {
            MakePhotosPageVM _objData = null;
            try
            {
                _objData = new MakePhotosPageVM();
                BindModelPhotos(_objData);
                BindModelBodyStyleLookupArray(_objData);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.Photos.MakePhotosPage : GetData");
            }
            return _objData;
            
        }

        private void BindModelPhotos(MakePhotosPageVM objData)
        {
            try
            {
                IEnumerable<ModelIdWithBodyStyle> objModelIds = _objModelEntity.GetModelIdsForImages(_makeId, EnumBikeBodyStyles.AllBikes);
                string modelIds = string.Join(",", objModelIds.Select(m => m.ModelId));
                int requiredImageCount = 4;
                string categoryIds = CommonApiOpn.GetContentTypesString(
                    new List<EnumCMSContentType>()
                    {
                        EnumCMSContentType.PhotoGalleries,
                        EnumCMSContentType.RoadTest
                    }
                );
                objData.BikeModelsPhotos = _objModelEntity.GetBikeModelsPhotos(modelIds, categoryIds, requiredImageCount);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Models.MakePhotosPage.BindModelPhotos : BindModelPhotos({0})", objData));
            }
        }

        private void BindModelBodyStyleLookupArray(MakePhotosPageVM objData)
        {
            try
            {
                objData.ModelBodyStyleArray = _objModelEntity.GetModelsWithBodyStyleLookupArray(_makeId);

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Models.MakePhotosPage.BindModelBodyStyleLookupArray : BindModelBodyStyleLookupArray({0})", objData));
            }
        }

        private void ProcessQuery(string makeMaskingName)
        {
            try
            {
                objResponse = _objMakeCache.GetMakeMaskingResponse(makeMaskingName);
                if (objResponse != null)
                {
                    Status = (StatusCodes)objResponse.StatusCode;
                    if (objResponse.StatusCode == 200)
                    {
                        _makeId = objResponse.MakeId;
                    }
                    else if (objResponse.StatusCode == 301)
                    {
                        RedirectUrl = HttpContext.Current.Request.RawUrl.Replace(makeMaskingName, objResponse.MaskingName);
                        Status = StatusCodes.RedirectPermanent;
                    }
                    else
                    {
                        Status = StatusCodes.ContentNotFound;
                    }
                }
                else
                {
                    Status = StatusCodes.ContentNotFound;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("MakePhotosPage.ProcessQuery() makeMaskingName : {0}", makeMaskingName));
            }
        }

    }
}