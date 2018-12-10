using AutoMapper;
using AEPLCore.Cache;
using Carwale.Cache.SponsoredData;
using Carwale.DAL.SponsoredCar;
using Carwale.DTOs.CarData;
using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Entity.Enum;
using Carwale.Interfaces.SponsoredCar;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.BL.Advertizing
{
    public class SponsoredCampaignApp
    {
        private readonly IUnityContainer _unity;
        private readonly ISponsoredCarCache _sponsoredCarCache;
        public static Dictionary<string, string> MiscellaneousScripts = new Dictionary<string, string>();

        public SponsoredCampaignApp(IUnityContainer unity, ISponsoredCarCache sponsoredCarCache)
        {
            _unity = unity;
            _sponsoredCarCache = sponsoredCarCache;
        }

        public AppDTO Response(int platformId, int categorySection = -1, string param="")
        {
            List<int> carIds = null;
            if (!string.IsNullOrWhiteSpace(param) && RegExValidations.ValidateCommaSeperatedNumbers(param))
            {
                carIds=param.Split(',').Select(i => int.Parse(i)).ToList();
            }
            
            List<Sponsored_Car> nativeAds = null;
            int counter =0;
            do
            {
                nativeAds = _sponsoredCarCache.GetSponsoredCampaigns((int)(CampaignCategory.NativeAppAds), platformId, categorySection, carIds==null?string.Empty:carIds[counter].ToString() ) ?? new List<Sponsored_Car>();
                if ((nativeAds != null && nativeAds.Count > 0) || carIds==null) break;
                counter++;
            } while (counter < carIds.Count && counter < 2);

            var nativeAd = nativeAds.FirstOrDefault();
            if (categorySection == 6 && (nativeAds != null && nativeAds.Count>0))
            {
                for (int c = 0 ; c < nativeAd.Links.Count ; c++)
                {
                    int match = 0;
                    var payload = (Dictionary<string, string>)nativeAd.Links[c].PayLoad;
                    if (payload == null) break;

                    foreach (var key in payload.Keys)
                    {
                        if (key.ToLower().StartsWith("versionid") && RegExValidations.IsPositiveNumber(payload[key]))
                        {
                            if (carIds.Contains(Convert.ToInt32(payload[key])))
                                match++;
                            if (match == 2)
                            {
                                nativeAds.Clear();
                                c = nativeAd.Links.Count;
                                break;
                            }
                        }
                    }
                }
            }

            var feature = nativeAds.Count == 0 ? null : new List<AdMonetizationDTO>(){
                    new AdMonetizationDTO{
                            Position = nativeAd.Postion,
                            ImageUrl = nativeAd.ImageUrl,
                            Title = nativeAd.SponsoredTitle,
                            Subtitle = nativeAd.Subtitle,
                            Content = nativeAd.Ad_Html,
                            CardHeader = nativeAd.CardHeader,
                            Links = Mapper.Map<List<Sponsored_CarLink>,List<AdLinkDTO>>(nativeAd.Links)
                        }
                    };
            
            //hybrid dto for homepage
            if (categorySection == (int)MobilePlatformScreenType.homepage)
            {
                SponsoredCarHomepageDTO dto = new SponsoredCarHomepageDTO();

                var banners = _sponsoredCarCache.GetSponsoredCampaigns((int)(CampaignCategory.Banner), platformId, categorySection);

                if (banners == null || banners.Count == 0)
                {
                    dto.SponsoredAdContentDTO = null;
                }
                else
                {
                    dto.SponsoredAdContentDTO = new List<SponsoredAdContentDTO>() { 
                    new SponsoredAdContentDTO{
                        AdHtmlContent = new List<SponsoredAdHtmlContentDTO>(){
                            new SponsoredAdHtmlContentDTO { HtmlContent = banners.First().Ad_Html}
                        },
                        AdType = "single",
                        CategoryId = 1 },
                    new SponsoredAdContentDTO{
                        AdHtmlContent = new List<SponsoredAdHtmlContentDTO>(){
                            new SponsoredAdHtmlContentDTO { HtmlContent = banners.First().Ad_Html}
                        },
                        AdType = "single",
                        CategoryId = 2 }
                    };//1-New , 2-Used
                }

                dto.NativeAds = feature;
                return dto;
            }

            return new OnlyAppAdsDTO() { NativeAds = feature };
        }

        public static string GetScript(string key)
        {
            string retval = string.Empty;
            return MiscellaneousScripts != null && MiscellaneousScripts.TryGetValue(key, out retval) ? retval : string.Empty;
        }

    }
}
