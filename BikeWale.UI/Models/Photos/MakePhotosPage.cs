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
        public MakeMaskingResponse ObjResponse { get; set; }
        public string RedirectUrl { get; set; }
        private uint _makeId;
        public bool IsMobile { get; set; }
        private string MakeName = string.Empty;
        private int ModelsCount;
        private string MakeMaskingName = string.Empty;
        


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
                BindOtherMakesWidget(_objData);
                if (_objData.BikeModelsPhotos != null && _objData.BikeModelsPhotos.Any())
                {
                    MakeName = _objData.BikeModelsPhotos.First().MakeBase.MakeName;
                    ModelsCount = _objData.BikeModelsPhotos.Count();
                    _objData.Make = _objData.BikeModelsPhotos.First().MakeBase;
                }
                BindImageSynopsis(_objData);
                SetBreadcrumList(_objData);
                SetPageMetas(_objData);
                SetPageJSONLDSchema(_objData);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.Photos.MakePhotosPage.GetData()");
            }
            return _objData;
            
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 29 Jan 2018
        /// Description : Method to bind synopsis of make images page.
        /// </summary>
        /// <param name="objData"></param>
        private void BindImageSynopsis(MakePhotosPageVM objData)
        {
            try
            {
                objData.ImagesSynopsis = string.Format("{0} offers {1} bike models in India. BikeWale brings you high-quality images of {0} bikes to make your bike buying decision easier. View images of all models of {0} in different colors and different angles.", MakeName, ModelsCount);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.Photos.MakePhotosPage : BindImageSynopsis");
            }
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 29 Jan 2018
        /// Description : Method to set breadcrum list.
        /// </summary>
        /// <param name="objData"></param>
        private void SetBreadcrumList(MakePhotosPageVM objData)
        {
            try
            {
                IList<BreadcrumbListItem> BreadCrumbs = new List<BreadcrumbListItem>();
                string url = string.Format("{0}/", Utility.BWConfiguration.Instance.BwHostUrl);
                ushort position = 1;
                if (IsMobile)
                {
                    url += "m/";
                }
                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, url, "Home"));
                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, string.Format("{0}new-bikes-in-india/", url), "New Bikes"));
                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, string.Format("{0}images/", url), "Images"));
                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position, null,string.Format("{0} Images", MakeName)));
                objData.BreadcrumbList.BreadcrumListItem = BreadCrumbs;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.Photos.MakePhotosPage.SetBreadcrumList()");
            }
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 29 Jan 2018
        /// Description : Method to set page metas. 
        /// </summary>
        /// <param name="objData"></param>
        private void SetPageMetas(MakePhotosPageVM objData)
        {
            try
            {
                objData.PageMetaTags.Title = string.Format("Images of {0} Bikes | Photo Gallery of {0} Models - BikeWale", MakeName);
                objData.PageMetaTags.Description = string.Format("View images and photo gallery of all models of {0}. BikeWale brings you images of all {1} models of {0} bikes in different colors and angles. Explore images of your favorite {0} Model.", MakeName, ModelsCount);

                objData.PageMetaTags.CanonicalUrl = string.Format("{0}/{1}-bikes/images/", BWConfiguration.Instance.BwHostUrl, MakeMaskingName);
                objData.PageMetaTags.AlternateUrl = string.Format("{0}/m/{1}-bikes/images/", BWConfiguration.Instance.BwHostUrl, MakeMaskingName);
                objData.PageMetaTags.Keywords = string.Format("{0} bike images, {0} bike photos, {0} bike wallpapers, {0} bike image gallery, {0} bike photo gallery", MakeName);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.Photos.Photos.MakePhotosPage.SetPageMetas()");
            }

        }
        /// <summary>
        /// Created by : Ashutosh Sharma on 29 Jan 2018
        /// Description : Method to set JSON/LD schema.
        /// </summary>
        /// <param name="objData"></param>
        private void SetPageJSONLDSchema(MakePhotosPageVM objData)
        {
            try
            {
                WebPage webpage = SchemaHelper.GetWebpageSchema(objData.PageMetaTags, objData.BreadcrumbList);

                if (webpage != null)
                {
                    objData.PageMetaTags.SchemaJSON = SchemaHelper.JsonSerialize(webpage);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.Photos.Photos.MakePhotosPage.SetPageJSONLDSchema()");
            }
        }

        /// <summary>
        /// Created By  : Rajan Chauhan on 30 Jan 2018
        /// Description : Binding of Bike models of particular make
        /// </summary>
        /// <param name="objData"></param>
        private void BindModelPhotos(MakePhotosPageVM objData)
        {
            try
            {
                IEnumerable<ModelIdWithBodyStyle> objModelIds = _objModelEntity.GetModelIdsForImages(_makeId, EnumBikeBodyStyles.AllBikes);
                if (objModelIds != null && objModelIds.Any())
                {
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
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Models.MakePhotosPage.BindModelPhotos : BindModelPhotos({0})", objData));
            }
        }

        /// <summary>
        /// Created By  : Rajan Chauhan on 30 Jan 2018
        /// Description : Creation of Lookup array
        /// </summary>
        /// <param name="objData"></param>
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
        /// <summary>
        /// Created By  : Rajan Chauhan on 30 Jan 2018
        /// Description : Bind Other popular makes skipping current make
        /// </summary>
        /// <param name="objData"></param>
        private void BindOtherMakesWidget(MakePhotosPageVM objData)
        {
            try
            {
                IEnumerable<BikeMakeEntityBase> makes = _objMakeCache.GetMakesByType(EnumBikeType.Photos).Where(make => make.MakeId != _makeId).Take(9);
                objData.OtherPopularMakes = new OtherMakesVM()
                {
                    Makes = makes,
                    PageLinkFormat = "/{0}-bikes/",
                    PageTitleFormat = "{0} Bikes",
                    CardText = "bike"
                };
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Models.MakePhotosPage.BindOtherMakesWidget : BindOtherMakesWidget({0})", objData));
            }
            
        }

        private void ProcessQuery(string makeMaskingName)
        {
            try
            {
                ObjResponse = _objMakeCache.GetMakeMaskingResponse(makeMaskingName);
                if (ObjResponse != null)
                {
                    Status = (StatusCodes)ObjResponse.StatusCode;
                    if (ObjResponse.StatusCode == 200)
                    {
                        _makeId = ObjResponse.MakeId;
                        MakeMaskingName = ObjResponse.MaskingName;
                    }
                    else if (ObjResponse.StatusCode == 301)
                    {
                        RedirectUrl = HttpContext.Current.Request.RawUrl.Replace(makeMaskingName, ObjResponse.MaskingName);
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