using Bikewale.Cache.Core;
using Bikewale.Cache.Used;
using Bikewale.DAL.Used;
using Bikewale.Entities.Used;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Used;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Bikewale.BindViewModels.Webforms.Used
{
    public class UsedBikeDetailsPage
    {

        public uint InquiryId { get; set; }
        public string BikeName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public string CanonicalUrl { get; set; }
        public BikePhoto FirstImage { get; set; }
        public string ModelYear { get; set; }
        public ClassifiedInquiryDetails InquiryDetails = null;
        public string MoreBikeSpecsUrl { get; set; }
        public string MoreBikeFeaturesUrl { get; set; }

        public void BindUsedBikeDetailsPage(Repeater rptUsedBikePhotos)
        {
            try
            {
                //ParseQueryString();
                GetProfileDetails();
                BindUsedBikePhotos(rptUsedBikePhotos);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

        }

        /// <summary>
        /// Created by  : Sushil Kumar on 04 Mar 2016
        /// Bind profile details for the used bike
        /// </summary>
        private void GetProfileDetails()
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IUsedBikeDetailsCacheRepository, UsedBikeDetailsCache>()
                             .RegisterType<ICacheManager, MemcacheManager>()
                             .RegisterType<IUsedBikeDetails, UsedBikeDetailsRepository>()
                            ;
                    var objCache = container.Resolve<IUsedBikeDetailsCacheRepository>();

                    InquiryDetails = objCache.GetProfileDetails(InquiryId);
                    if (InquiryDetails != null && InquiryDetails.MinDetails != null)
                    {
                        BikeName = string.Format("{0} {1} {2}", InquiryDetails.Make.MakeName, InquiryDetails.Model.ModelName, InquiryDetails.Version.VersionName);
                        ModelYear = (InquiryDetails.MinDetails.ModelYear != null) ? InquiryDetails.MinDetails.ModelYear.Year.ToString() : string.Empty;
                        Title = string.Format("Used {0} {1} (S{2}) for sale in {3} | BikeWale", ModelYear, BikeName, InquiryDetails.OtherDetails.Id, InquiryDetails.City.CityName);
                        Description = string.Format("used {0}, used {0} for sale, used {0} in {1}", BikeName, InquiryDetails.City.CityName);
                        Keywords = string.Format("BikeWale - Used {0} {1} for sale in {2}. This second hand bike is of {3} model and its profile id is S{4}. Get phone number of the seller and call directly to inspect and test ride the bike.", InquiryDetails.Make.MakeName, InquiryDetails.Model.ModelName, InquiryDetails.City.CityName, ModelYear, InquiryDetails.OtherDetails.Id);
                        MoreBikeSpecsUrl = string.Format("/{0}-bikes/{1}/specifications-features/?vid={2}#specs", InquiryDetails.Make.MaskingName, InquiryDetails.Model.MaskingName, InquiryDetails.Version.VersionId);
                        MoreBikeFeaturesUrl = string.Format("/{0}-bikes/{1}/specifications-features/?vid={2}#features", InquiryDetails.Make.MaskingName, InquiryDetails.Model.MaskingName, InquiryDetails.Version.VersionId);
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "GetProfileDetails");
                objErr.SendMail();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rptUsedBikePhotos"></param>
        private void BindUsedBikePhotos(Repeater rptUsedBikePhotos)
        {
            if (InquiryDetails.PhotosCount > 0 && InquiryDetails.Photo != null)
            {
                rptUsedBikePhotos.DataSource = InquiryDetails.Photo;
                rptUsedBikePhotos.DataBind();
                FirstImage = InquiryDetails.Photo.FirstOrDefault();
            }
        }

        ///// <summary>
        ///// Function to do the redirection on different pages.
        ///// </summary>
        //private void DoPageNotFounRedirection()
        //{
        //    // Redirection
        //    if (redirectToPageNotFound)
        //    {
        //        Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
        //    }
        //    else if (redirectPermanent)
        //        CommonOpn.RedirectPermanent(redirectUrl);
        //}

        ///// <summary>
        ///// Function to get parameters from the query string.
        ///// </summary>
        //private void ParseQueryString()
        //{
        //    ModelMaskingResponse objModelResponse = null;
        //    CityMaskingResponse objCityResponse = null;
        //    string model = string.Empty, city = string.Empty, _make = string.Empty;
        //    try
        //    {
        //        model = Request.QueryString["model"];
        //        city = Request.QueryString["city"];

        //        if (!string.IsNullOrEmpty(city))
        //        {
        //            using (IUnityContainer container = new UnityContainer())
        //            {
        //                container.RegisterType<ICityMaskingCacheRepository, CityMaskingCache>()
        //                         .RegisterType<ICacheManager, MemcacheManager>()
        //                         .RegisterType<ICity, CityRepository>()
        //                        ;
        //                var objCache = container.Resolve<ICityMaskingCacheRepository>();
        //                objCityResponse = objCache.GetCityMaskingResponse(city);
        //            }
        //        }

        //        if (!string.IsNullOrEmpty(model))
        //        {
        //            using (IUnityContainer container = new UnityContainer())
        //            {
        //                container.RegisterType<IBikeMaskingCacheRepository<BikeModelEntity, int>, BikeModelMaskingCache<BikeModelEntity, int>>()
        //                         .RegisterType<ICacheManager, MemcacheManager>()
        //                         .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
        //                        ;
        //                var objCache = container.Resolve<IBikeMaskingCacheRepository<BikeModelEntity, int>>();
        //                objModelResponse = objCache.GetModelMaskingResponse(model);
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"] + "ParseQueryString");
        //        objErr.SendMail();

        //        Response.Redirect("/customerror.aspx", false);
        //        HttpContext.Current.ApplicationInstance.CompleteRequest();
        //        this.Page.Visible = false;
        //    }
        //    finally
        //    {
        //        if (objCityResponse != null && objModelResponse != null)
        //        {
        //            // Get cityId
        //            // Code to check whether masking name is changed or not. If changed redirect to appropriate url
        //            if (objCityResponse.StatusCode == 200)
        //            {
        //                cityId = objCityResponse.CityId;
        //            }
        //            else if (objCityResponse.StatusCode == 301)
        //            {
        //                //redirect permanent to new page                         
        //                redirectUrl = Request.RawUrl.Replace(city, objCityResponse.MaskingName);
        //                redirectPermanent = true;
        //            }
        //            else
        //            {
        //                redirectToPageNotFound = true;
        //            }

        //            // Get ModelId
        //            // Code to check whether masking name is changed or not. If changed redirect to appropriate url
        //            if (objModelResponse.StatusCode == 200)
        //            {
        //                modelId = objModelResponse.ModelId;
        //            }
        //            else if (objModelResponse.StatusCode == 301)
        //            {
        //                //redirect permanent to new page                         
        //                redirectUrl = Request.RawUrl.Replace(model, objModelResponse.MaskingName);
        //                redirectPermanent = true;
        //            }
        //            else
        //            {
        //                redirectToPageNotFound = true;
        //            }
        //        }
        //        else
        //        {
        //            redirectToPageNotFound = true;
        //        }

        //    }
        //}





    }

}