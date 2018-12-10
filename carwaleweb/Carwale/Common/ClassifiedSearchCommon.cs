using Carwale.Interfaces.Classified;
using Carwale.Notifications;
using Carwale.Service;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Carwale.Entity.Classified;
using Carwale.Entity;

namespace Carwale.UI.Common
{
    public class ClassifiedSearchCommon
    {
        private const string c_strRedirectURLValidCity = "/used/{0}-{1}-cars-in-{2}/";
        private const string c_strRedirectURLNullCity = "/used/{0}-{1}-cars/";
        private const string c_strRedirectURLValidCityAndUnknownMake = "/used/cars-in-{0}/";
        private const string c_strRedirectURLForSoldCars = "/used/cars-for-sale/";
        private const string c_strValidMake = "/used/{0}-cars/";
        private const string c_strIsSold = "issold";
        private ICommonOperationsCacheRepository _commonOperationCache;
        private CarModelMaskingResponse carModelMaskingResponse = null;
        public ClassifiedSearchCommon(){
            using (IUnityContainer container = UnityBootstrapper.Resolver.GetContainer())
            {
                _commonOperationCache = container.Resolve<ICommonOperationsCacheRepository>();
            }
        }
        public CarModelMaskingResponse GetMakeDetailsByRootName(string root)
        {
            carModelMaskingResponse = null;
            try
            {
                carModelMaskingResponse = _commonOperationCache.GetMakeDetailsByRootName(root);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ClassifiedSearchCommon.GetMakeDetailsByRootName()");
                objErr.SendMail();
            }
            return carModelMaskingResponse;
        }
        public string GetRedirectURl(string cityName,Entity.CarModelMaskingResponse cmr,HttpRequest Request)
        {
            string redirectUrl = string.Format(c_strRedirectURLForSoldCars);
            string rootLowercase =(!string.IsNullOrEmpty(cmr.RootName))?cmr.RootName.ToLower().Replace(" ",string.Empty):string.Empty;
            if (cmr.MakeId != -1) {
                if (!string.IsNullOrEmpty(cityName))
                {
                    if (!string.IsNullOrEmpty(rootLowercase))
                        redirectUrl = string.Format(c_strRedirectURLValidCity, UrlRewrite.FormatSpecial(cmr.MakeName), rootLowercase, UrlRewrite.FormatSpecial(cityName));
                    else
                        redirectUrl = string.Format(c_strRedirectURLValidCityAndUnknownMake, UrlRewrite.FormatSpecial(cityName));
                }
                else if (!string.IsNullOrEmpty(cmr.RootName))
                    redirectUrl = string.Format(c_strRedirectURLNullCity, UrlRewrite.FormatSpecial(cmr.MakeName), rootLowercase);
                else
                    redirectUrl = string.Format(c_strValidMake, UrlRewrite.FormatSpecial(cmr.MakeName));
            }
            else if(!string.IsNullOrEmpty(cityName)){
                redirectUrl = string.Format(c_strRedirectURLValidCityAndUnknownMake, UrlRewrite.FormatSpecial(cityName));
            }
            return redirectUrl;
        }

        public void RedirectToNewURL(string redirectURL,HttpResponse Response)
        {
            if (Response != null)
            {
                Response.Redirect(redirectURL, false);
                Response.StatusCode = 301;
                Response.End();
            }
        }
        public bool IsRedirect(Entity.CarModelMaskingResponse cmr)
        {
            bool redirect = false;
            carModelMaskingResponse = null;
            try
            {   
                if((cmr!=null && (cmr.ModelId > 0 && !string.IsNullOrEmpty(cmr.RootName) && !cmr.MaskingName.ToLower().Equals(cmr.RootName.ToLower())))){
                    carModelMaskingResponse = _commonOperationCache.GetMakeDetailsByRootName(cmr.RootName.ToLower().Replace(" ", string.Empty));
                    redirect = (carModelMaskingResponse != null);
                    if (!redirect)
                        cmr.RootName = string.Empty;
                }
                redirect = redirect || cmr.Redirect;
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ClassifiedSearchCommon.IsRedirect()");
                objErr.SendMail();
            }
            return redirect;
        }
        public void GetListingIndexesForCurrentPage(FilterInputs filtersInput, int minlcr, int minldr, int minlir,int pazeSize,int maxFeaturedListings)
        {
            int ldr = 0, lcr = 0, lir = 0, pn = 0;
            try
            {
                if (int.TryParse(filtersInput.pn, out pn) && pn != 1)
                {
                    lcr = (pn - 1) * minlcr + GetRandomNumber(1, (pazeSize-minlcr)); // assumption : max index of lcr for previous page : (pn-1)*minlcr 
                    ldr = (pn - 1) * minldr + GetRandomNumber(1, (maxFeaturedListings-minldr));  // assumption : max index of ldr for previous page : (pn-1)*minldr
                    lir = (pn - 1) * minlir + GetRandomNumber(1, (maxFeaturedListings - minlir));  //  assumption  : max index of lir for previous page : (pn-1)
                    filtersInput.lir = (short)lir;
                    filtersInput.ldr = (short)ldr;
                    filtersInput.lcr = (short)lcr;
                }
            }
            catch (Exception ex)
            {
                var objErr = new ExceptionHandler(ex, "ClassifiedSearchCommon.GetListingIndexesForCurrentPage()");
                objErr.LogException();
            }
        }
        private int GetRandomNumber(int from, int to)
        {
            return (from <= to) ? new Random().Next(from, to) : 0;
        }
    }
}