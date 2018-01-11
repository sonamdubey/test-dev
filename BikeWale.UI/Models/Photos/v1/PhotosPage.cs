using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Entities.Schema;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.Videos;
using Bikewale.Models.Gallery;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace Bikewale.Models.Photos.v1
{
    /// <summary>
    /// Created by  : Rajan Chauhan on 11 Jan 2017
    /// Description :  To bind photo page
    /// </summary>
    public class PhotosPage
    {
        private readonly IBikeMakesCacheRepository _objMakeCache = null;
        private PhotosPageVM _objData = null;
        public bool IsMobile { get; set; }

        /// <summary>
        /// Created by  :  Rajan Chauhan on 11 jan 2017
        /// Description :  To resolve depedencies for photo page
        /// </summary>
        public PhotosPage(bool isMobile,IBikeMakesCacheRepository objMakeCache)
        {
            IsMobile = isMobile;
            _objMakeCache = objMakeCache;
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
                BindMakesWidget();
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("Bikewale.Models.Photos.PhotosPage.v1.GetData : GetData()"));
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
            IEnumerable<BikeMakeEntityBase> makes = _objMakeCache.GetMakesByType(EnumBikeType.Photos);
            _objData.OtherPopularMakes = new OtherMakesVM()
            {
                Makes = makes,
                PageLinkFormat = "/{0}-bikes/",
                PageTitleFormat = "{0} Bikes",
                CardText = "bike"
            };
        }
        private void BindPageWidgets()
        {
            PQSourceEnum pqSource = IsMobile ? PQSourceEnum.Mobile_Photos_Page : PQSourceEnum.Desktop_Photos_page;
            try
            {
               
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("Bikewale.Models.Photos.PhotosPage.BindPageWidgets : BindPageWidgets()"));
            }
        }

        private void SetPageMetas()
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("Bikewale.Models.Photos.PhotosPage.SetPageMetas : SetPageMetas()"));
            }

        }

        private void SetBreadcrumList()
        {
        }
    }
}