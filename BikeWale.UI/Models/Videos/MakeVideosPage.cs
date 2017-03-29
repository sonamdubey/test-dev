using Bikewale.Common;
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.Videos;
using Bikewale.Utility;
using System;
using System.Linq;
using System.Web;
namespace Bikewale.Models
{
    /// <summary>
    /// Created by  :   Sumit Kate on 29 Mar 2017
    /// Description :   Make Wise Videos page model
    /// </summary>
    public class MakeVideosPage
    {
        private readonly IVideosCacheRepository _videosCache = null;
        private readonly string _makeMaskingName;
        private uint _makeId;
        public StatusCodes Status { get; private set; }
        public String RedirectUrl { get; private set; }

        /// <summary>
        /// Created by  :   Sumit Kate on 29 Mar 2017
        /// Description :   Constructor to initialize the member variables
        /// </summary>
        /// <param name="makeMaskingName"></param>
        /// <param name="videosCache"></param>
        public MakeVideosPage(string makeMaskingName, IVideosCacheRepository videosCache)
        {
            _makeMaskingName = makeMaskingName;
            _videosCache = videosCache;
            ProcessQueryString();
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 29 Mar 2017
        /// Description :   Builds the view model to bind the make videos view
        /// </summary>
        /// <returns></returns>
        public MakeVideosPageVM GetData()
        {
            MakeVideosPageVM objVM = null;
            try
            {
                objVM = new MakeVideosPageVM();
                objVM.CityId = GlobalCityArea.GetGlobalCityArea().CityId > 0 ? GlobalCityArea.GetGlobalCityArea().CityId : Convert.ToUInt32(BWConfiguration.Instance.DefaultCity);

                var videos = _videosCache.GetModelVideos(_makeId);
                if (videos != null && videos.Count() > 0)
                {
                    objVM.Videos = videos;
                    objVM.Make = videos.FirstOrDefault().objMake;
                    objVM.Make.MaskingName = _makeMaskingName;
                    BindPageMetas(objVM);
                }

            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, String.Format("GetData({0})", _makeMaskingName));
            }
            return objVM;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 29 Mar 2017
        /// Description :   Binds page metas
        /// </summary>
        /// <param name="objVM"></param>
        private void BindPageMetas(MakeVideosPageVM objVM)
        {
            try
            {
                objVM.PageMetaTags.Title = string.Format("{0} Bike Videos - BikeWale", objVM.Make.MakeName);
                objVM.PageMetaTags.Description = string.Format("Check latest {0} bikes videos, watch BikeWale expert's take on {0} bikes - features, performance, price, fuel economy, handling and more.", objVM.Make.MakeName);
                objVM.PageMetaTags.Keywords = string.Format("{0},{0} bikes,{0} videos", objVM.Make.MakeName);
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, String.Format("BindPageMetas({0})", _makeMaskingName));
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 29 Mar 2017
        /// Description :   ProcessQueryString. It processes make masking name
        /// </summary>
        private void ProcessQueryString()
        {
            String rawUrl = HttpContext.Current.Request.RawUrl;
            MakeMaskingResponse objMakeResponse = null;
            try
            {
                objMakeResponse = new MakeHelper().GetMakeByMaskingName(_makeMaskingName);
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, String.Format("ProcessQueryString({0})", _makeMaskingName));
                Status = StatusCodes.ContentNotFound;
            }
            finally
            {
                if (objMakeResponse != null)
                {
                    if (objMakeResponse.StatusCode == 200)
                    {
                        _makeId = objMakeResponse.MakeId;
                        Status = StatusCodes.ContentFound;
                    }
                    else if (objMakeResponse.StatusCode == 301)
                    {
                        rawUrl = rawUrl.Replace(_makeMaskingName, objMakeResponse.MaskingName);
                        Status = StatusCodes.RedirectPermanent;
                    }
                    else
                    {
                        Status = StatusCodes.ContentNotFound;
                    }
                    RedirectUrl = rawUrl;
                }
                else
                {
                    Status = StatusCodes.ContentNotFound;
                }
            }
        }

    }
}