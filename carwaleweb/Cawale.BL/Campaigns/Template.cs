using AutoMapper;
using Campaigns.DealerCampaignClient;
using Carwale.Entity.Campaigns;
using Carwale.Entity.Common;
using Carwale.Entity.Dealers;
using Carwale.Entity.Enum;
using Carwale.Entity.PageProperty;
using Carwale.Entity.Template;
using Carwale.Interfaces.Campaigns;
using Carwale.Interfaces.Template;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using ProtoBufClass.Campaigns;
using RazorEngine;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web;

namespace Carwale.BL.Campaigns
{
    // This class handles all campaignBL related to template 
    public class Template : ITemplate
    {
        private readonly ITemplatesCacheRepository _tempCache;
        private readonly ICampaignCacheRepository _campaignCacheRepo;
        private readonly int _mobilePaidCrossSellDefaultTemplateId = CustomParser.parseIntObject(ConfigurationManager.AppSettings["MobilePaidCrossSellDefaultTemplate"]);
        private readonly int _androidPaidCrossSellDefaultTemplate = CustomParser.parseIntObject(ConfigurationManager.AppSettings["AndroidPaidCrossSellDefaultTemplate"]);
        private readonly int _IOSPaidCrossSellDefaultTemplate = CustomParser.parseIntObject(ConfigurationManager.AppSettings["IOSPaidCrossSellDefaultTemplate"]);
        private readonly int _mobilePaidCrossSellNewTemplateId = CustomParser.parseIntObject(ConfigurationManager.AppSettings["MobilePaidCrossSellNewTemplate"]);
        private readonly int _webViewTemplateId = CustomParser.parseIntObject(ConfigurationManager.AppSettings["WebViewTemplate"]);
        private readonly int _emiCalculatorTemplateId1 = CustomParser.parseIntObject(ConfigurationManager.AppSettings["EmiCalculatorTemplateId1"]);
        private readonly int _emiCalculatorTemplateId2 = CustomParser.parseIntObject(ConfigurationManager.AppSettings["EmiCalculatorTemplateId2"]);
        private readonly int _emiCalculatorTemplateABTestMinVal = CustomParser.parseIntObject(ConfigurationManager.AppSettings["EmiCalculatorTemplateABTestMinValue"]);
        private readonly int _emiCalculatorTemplateABTestMaxVal = CustomParser.parseIntObject(ConfigurationManager.AppSettings["EmiCalculatorTemplateABTestMaxValue"]);
        private readonly string _iosModelPropertyTemplates = ConfigurationManager.AppSettings["IOSModelPropertyTemplates"] ?? "";
        private readonly string _iosVersionPropertyTemplates = ConfigurationManager.AppSettings["IOSVersionPropertyTemplates"] ?? "";

        public Template(ITemplatesCacheRepository templateCache, ICampaignCacheRepository campaignCacheRepo)
        {
            _tempCache = templateCache;
            _campaignCacheRepo = campaignCacheRepo;
        }

        public string GetCampaignTemplate(Entity.Campaigns.Campaign campaign, string pageId, int sourceId)
        {
            SponsoredDealer sponsoredDealer = Mapper.Map<SponsoredDealer>(campaign);
            sponsoredDealer = CampaignDealerInfo(sponsoredDealer, pageId, sourceId);

            return sponsoredDealer.TemplateHtml;
        }

        public Templates GetCampaignTemplates(Entity.Campaigns.Campaign campaign, string pageId, int sourceId)
        {

            SponsoredDealer sponsoredDealer = Mapper.Map<SponsoredDealer>(campaign);

            sponsoredDealer = CampaignDealerInfo(sponsoredDealer, pageId, sourceId);

            Templates template = Mapper.Map<Templates>(sponsoredDealer);

            return template;
        }

        public SponsoredDealer CampaignDealerInfo(SponsoredDealer sponsoredDealer, string pageId, int sourceId)
        {
            try
            {
                if (sponsoredDealer != null && sponsoredDealer.DealerId > 0)
                {
                    Templates template;

                    if (pageId == SponsoredCarPageId.PaidCrossSell.GetHashCode().ToString())
                    {
                        switch (sourceId)
                        {
                            case (int)Carwale.Entity.Enum.Platform.CarwaleMobile:
                                template = _tempCache.GetById(_mobilePaidCrossSellNewTemplateId);
                                break;

                            case (int)Carwale.Entity.Enum.Platform.CarwaleAndroid:
                                if (BrowserUtils.IsWebView())
                                {
                                    template = _tempCache.GetById(_webViewTemplateId);
                                    break;
                                }
                                else
                                {
                                    template = _tempCache.GetById(_androidPaidCrossSellDefaultTemplate);
                                    break;
                                }
                            case (int)Carwale.Entity.Enum.Platform.CarwaleiOS:
                                if (BrowserUtils.IsWebView())
                                {
                                    template = _tempCache.GetById(_webViewTemplateId);
                                    break;
                                }
                                else
                                {
                                    template = _tempCache.GetById(_IOSPaidCrossSellDefaultTemplate);
                                    break;
                                }
                            default:
                                template = null;
                                break;
                        }
                    }
                    else
                    {
                        int templateId;

                        if (sourceId == (int)Carwale.Entity.Enum.Platform.CarwaleMobile || sourceId == (int)Carwale.Entity.Enum.Platform.CarwaleDesktop)
                        {
                            templateId = GetCampaignGroupTemplateId(sponsoredDealer.AssignedTemplateId, sponsoredDealer.AssignedGroupId, sourceId);
                        }
                        else if (BrowserUtils.IsWebView())
                        {
                            templateId = _webViewTemplateId;
                        }
                        else
                        {
                            templateId = sponsoredDealer.AssignedTemplateId;
                        }
                        template = _tempCache.GetById(templateId);
                    }
                    if (template == null)
                    {
                        return null;
                        //Error mail to be send
                    }

                    ResolveTemplate(template, ref sponsoredDealer);
                }
            }
            catch (Exception err)
            {
                var exception = new ExceptionHandler(err, HttpContext.Current.Request.ServerVariables["URL"]);
                exception.LogException();
            }
            return sponsoredDealer;
        }

        public int GetCampaignGroupTemplateId(int assignedTemplateId, int assignedGroupId, int platformId)//remove
        {
            try
            {
                Random rnd = new Random();
                int templateId = 0;
                int abTestCookieVal = CustomParser.parseIntObject((HttpContext.Current.Request.Cookies["_abtest"].Value));

                abTestCookieVal = abTestCookieVal < 1 ? rnd.Next(1, 11) : abTestCookieVal;

                var templateGroupList = _campaignCacheRepo.GetCampaignGroupTemplateIdCache(assignedGroupId, platformId);
                if (templateGroupList != null && templateGroupList.Count > 0)
                {
                    bool hasTemplateId = templateGroupList.TryGetValue(-1, out templateId); // Check for single template in a group
                    if (hasTemplateId)
                    {
                        return templateId;
                    }
                    else
                    {
                        hasTemplateId = templateGroupList.TryGetValue(abTestCookieVal, out templateId); // Check for template against abTestCookieVal in a group
                        if (hasTemplateId)
                        {
                            return templateId;
                        }
                    }
                }
                return assignedTemplateId;
            }
            catch (Exception err)
            {
                var exception = new ExceptionHandler(err, HttpContext.Current.Request.ServerVariables["URL"]);
                exception.LogException();
                return 0;
            }
        }

        public void ResolveTemplate(Templates template, ref SponsoredDealer dealerSponsoredDetails)//remove
        {
            try
            {
                dealerSponsoredDetails.TemplateName = template.UniqueName;
                dealerSponsoredDetails.TemplateHeight = CustomParser.parseIntObject(ConfigurationManager.AppSettings["TemplateHeight"]);
                dealerSponsoredDetails.TemplateHtml = GetRendredContent(dealerSponsoredDetails.TemplateName, template.Html, dealerSponsoredDetails);
            }
            catch (Exception err)
            {
                var exception = new ExceptionHandler(err, HttpContext.Current.Request.ServerVariables["URL"]);
                exception.LogException();
            }
        }

        public string GetRendredContent<T>(string templateName, string template, T model)
        {
            string renderedContent = "";
            try
            {
                if (Razor.Resolve(templateName) == null)
                {
                    // we've never seen this template before, so compile it and stick it in cache.                
                    Razor.Compile(template, typeof(T), templateName);
                }
                renderedContent = Razor.Run(templateName, model);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return renderedContent;
        }

        public Templates GetTemplateById(int id)
        {
            try
            {
                return _tempCache.GetById(id);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return null;
        }

        public Templates GetEmiCalculatorTemplate()
        {
            try
            {
                int abTestCookieVal = CustomParser.parseIntObject((HttpContext.Current.Request.Cookies["_abtest"].Value));
                var templateId = _emiCalculatorTemplateId2;
                if (_emiCalculatorTemplateABTestMinVal <= abTestCookieVal && abTestCookieVal <= _emiCalculatorTemplateABTestMaxVal)
                {
                    templateId = _emiCalculatorTemplateId1;
                }
                return _tempCache.GetById(templateId);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return null;
        }

        /// <summary>
        /// To get template details of all properties for a page
        /// </summary>
        /// <param name="campaignInput"></param>
        /// <returns></returns>
        public Dictionary<int, IdName> GetTemplatesByPage<T>(CampaignInputv2 campaignInput, T model)
        {
            try
            {
                var response = (DealerCampaignClient.GetAllTemplatesByPage(Mapper.Map<TemplateInput>(campaignInput)));
                if (response != null)
                {
                    List<PageTemplates> pageTemplates = Mapper.Map<List<PageTemplates>>(response.PageTemplates);

                    return AddTemplatesToDict(pageTemplates, model, campaignInput.PageId);
                }
            }
            catch (Exception e)
            {
                Logger.LogException(e);
            }
            return null;
        }

        public Dictionary<int, IdName> AddTemplatesToDict<T>(List<PageTemplates> pageTemplates, T model, int pageId)
        {
            Dictionary<int, IdName> templateDict = new Dictionary<int, IdName>();
            try
            {
                foreach (var template in pageTemplates)
                {
                    if (!templateDict.ContainsKey(template.PropertyId))
                    {
                        var templateDetails = _tempCache.GetById(template.TemplateId);
                        if (templateDetails != null && !string.IsNullOrWhiteSpace(templateDetails.Html))
                        {
                            IdName temp = new IdName();
                            temp.Name = GetRendredContent(string.Format("{0}-{1}", templateDetails.UniqueName, pageId), templateDetails.Html, model);
                            temp.Id = template.CampaignId;
                            templateDict.Add(template.PropertyId, temp);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogException(e);
            }
            return templateDict;
        }

        public void AddTemplatesToDict<T>(Templates template, int propertyId, T model, ref Dictionary<int, IdName> templateDictionary)
        {
            try
            {
                if (!templateDictionary.ContainsKey(template.TemplateType))
                {
                    if (!string.IsNullOrWhiteSpace(template.Html))
                    {
                        IdName temp = new IdName();
                        temp.Name = GetRendredContent(template.UniqueName, template.Html, model);
                        temp.Id = template.TemplateType;
                        templateDictionary.Add(propertyId, temp);
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogException(e);
            }
        }

        public List<PageProperty> GetPageProperties(int platformId, int pageId, Entity.Campaigns.Campaign campaignDetail)
        {
            List<PageProperty> pageProperty = new List<PageProperty>();
            try
            {
                //27 implies IOS Webview
                if (platformId == (int)Platform.CarwaleiOS && pageId != 27)
                {
                    var iosPropertyTemplates = pageId == 1 ? _iosModelPropertyTemplates : pageId == 3 ? _iosVersionPropertyTemplates : "";

                    var templateArray = iosPropertyTemplates.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    for (int index = 0; index < templateArray.Length; index++)
                    {
                        var splitTemplateArray = templateArray[index].Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
                        var template = GetTemplateById(CustomParser.parseIntObject(splitTemplateArray[1]));
                        template.Html = Regex.Replace(template.Html, "<.*?>", String.Empty);
                        pageProperty.Add(new PageProperty { Id = CustomParser.parseIntObject(splitTemplateArray[0]), Template = template });
                    }
                }
                else
                {
                    var template = GetCampaignTemplates(campaignDetail, SponsoredCarPageId.PQCampaign.GetHashCode().ToString(), platformId);
                    if (template != null && !string.IsNullOrWhiteSpace(template.Html))
                    {
                        pageProperty.Add(new PageProperty { Template = template });
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogException(e);
            }

            return pageProperty;
        }
    }
}
