using System;
using System.Collections.Generic;
using System.Linq;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;

namespace Bikewale.Models.Photos.v1
{
    /// <summary>
    /// Created by  : Rajan Chauhan on 11 Jan 2017
    /// Description :  To bind photo page
    /// </summary>
    public class PhotosPage
    {
        private readonly IBikeMakesCacheRepository _objMakeCache = null;
        private readonly IBikeModels<BikeModelEntity, int> _objModelEntity = null;
        private PhotosPageVM _objData = null;
        public bool IsMobile { get; set; }

        /// <summary>
        /// Created by  :  Rajan Chauhan on 11 jan 2017
        /// Description :  To resolve depedencies for photo page
        /// </summary>
        public PhotosPage(bool isMobile, IBikeMakesCacheRepository objMakeCache, IBikeModels<BikeModelEntity, int> objModelEntity)
        {
            IsMobile = isMobile;
            _objMakeCache = objMakeCache;
            _objModelEntity = objModelEntity;
        }

        /// <summary>
        /// Created by  :  Rajan Chauhan on 11 jan 2017
        /// Descritpion :  Added PageData for page
        /// </summary>
        /// <returns></returns>
        public PhotosPageVM GetData()
        { 
            try
            {
                _objData = new PhotosPageVM();
                _objData.ModelsImages = BindPopularSportsBikeWidget();
                BindMakesWidget();
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "Bikewale.Models.Photos.v1.PhotosPage.GetData()");
            }

            return _objData;
        }

        /// <summary>
        /// Created by  :  Rajan Chauhan on 11 jan 2017
        /// Descritpion :  To Add otherPopularMakes Widget in VM
        /// </summary>
        /// <returns></returns>
        private void BindMakesWidget()
        {
            IEnumerable<BikeMakeEntityBase> makes = _objMakeCache.GetMakesByType(EnumBikeType.Photos).Take(9);
            _objData.OtherPopularMakes = new OtherMakesVM()
            {
                Makes = makes,
                PageLinkFormat = "/{0}-bikes/images/",
                PageTitleFormat = "{0} Bikes",
                CardText = "Bike"
            };
        }
        private void BindPageWidgets()
        {
            try
            {
               
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "Bikewale.Models.Photos.v1.PhotosPage.BindPageWidgets()");
            }
        }

        private void SetPageMetas()
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "Bikewale.Models.Photos.v1.PhotosPage.SetPageMetas()");
            }

        }

        private void SetBreadcrumList()
        {
        }

        /// <summary>
        /// Created by  : Vivek Singh Tomar on 12th Jan 2018
        /// Description : Bind sports bike image widget
        /// </summary>
        /// <returns></returns>
        private IEnumerable<ModelImages> BindPopularSportsBikeWidget()
        {
            IEnumerable<ModelImages> modelsImages = null;
            try
            {
                IEnumerable<ModelIdWithBodyStyle> modelIdsWithBodyStyle = _objModelEntity.GetModelIdsForImages(0, Entities.GenericBikes.EnumBikeBodyStyles.Sports, 1, 9);
                var modelIds = string.Join(",", modelIdsWithBodyStyle.Select(m => m.ModelId.ToString()));
                if (!string.IsNullOrEmpty(modelIds))
                {
                    modelsImages = _objModelEntity.GetBikeModelsPhotoGallery(modelIds, 7);
                }
            }
            catch(Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.Photos.v1.PhotosPage");
            }
            return modelsImages;
        }
    }
}