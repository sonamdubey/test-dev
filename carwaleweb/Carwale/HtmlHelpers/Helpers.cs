using Carwale.DTOs.PriceQuote;
using Carwale.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Carwale.UI.HtmlHelpers
{
    public static class Helpers
    {

        public static MvcHtmlString PriceOverviewHelper(PriceOverviewDTOV2 CarPriceOverview)
        {
            string str = string.Format(@"<div class='darkgray new-line5'>{0} <span>{1}</span> {2}</div>
                             <div class='darkgray new-line5 {3}'>
                             {4}{5}
                             <span>{6}</span></div>",
                              CarPriceOverview.PricePrefix, CarPriceOverview.Price.ToString(),CarPriceOverview.PriceSuffix,
                              CarPriceOverview.LabelColor, CarPriceOverview.PriceLabel, !string.IsNullOrEmpty(CarPriceOverview.City.Name) ? ", "
                              + CarPriceOverview.City.Name : string.Empty,
                              CarPriceOverview.PriceStatus > (int)PriceBucket.HaveUserCity ?
                              "<div class='inline-block'><span class='average-info-tooltip class-ad-tooltip info-icon deals-car-sprite inline-block'></span><p class='average-info-content hide'>"
                              + CarPriceOverview.ReasonText + "</p></div>" : string.Empty);
            return new MvcHtmlString(str);
        }

        public static MvcHtmlString GetAdBar(string id,int width,int height,int margin1,int margin2,bool dataloadimmediate = false, int pos = 0)
        {
            string adId=string.Format("div-gpt-ad-{0}-{1}",id,pos);
            string style=string.Format("style='width:{0}px;{1}margin:{2}px auto {3}px auto;'",width,(height != 0 ? string.Format("height:{0}px;",height) : ""),margin1,margin2);

            string str = string.Format("<div id='{0}' data-load-immediate='{1}' {2}><script type='text/javascript'>googletag.cmd.push(function () {{ googletag.display('{3}'); }});</script></div>"
                ,adId,dataloadimmediate.ToString().ToLower(),style,adId);

            return new MvcHtmlString(str);
        }
        
        public static string GetAdBarForAspx(string id,int width,int height,int margin1,int margin2,bool dataloadimmediate = false, int pos = 0)
        {
            string str = "<div class='inline-block ad-slot'><div class='text-right font11 text-medium-grey'>Ad</div><div id='div-gpt-ad-" + id + "-" + pos + "' data-load-immediate='"+dataloadimmediate.ToString().ToLower()+"' style='margin:" + margin1 + "px auto " + margin2 + "px auto;'>" +
                        "<script type='text/javascript'>googletag.cmd.push(function () { googletag.display('div-gpt-ad-" + id + "-" + pos + "'); });</script></div></div>";
            return str;
        }

        public static MvcHtmlString GetOgTags(string ogUrl,string title,string description,string imageUrl)
        {
            string str=string.Format(@"<meta property='og:url' content='{0}' />
                 <meta property='og:type' content='website' />
                 <meta property='og:title' content='{1}' />
                 <meta property='og:description' content='{2}' />
                 <meta property='og:image' content='{3}' />
                 <meta property='og:site_name' content='CarWale' />
                 <meta property='fb:admins' content='154881297559' />
                 <meta property='fb:page' content = '154881297559' /> 
                 <meta property='fb:app_id' content = '481333722014878' />", ogUrl,title,description,imageUrl);
                 return new MvcHtmlString(str);
        }

        public static MvcHtmlString GetTwitterTags(string card, string title, string description, string imageUrl)
        {
            string str = string.Format(@"<meta name='twitter:card' content='{0}' />
    <meta name = 'twitter:site' content = '@CarWale' />
    <meta name = 'twitter:title' content = '{1}' />
    <meta name = 'twitter:description' content = '{2}' />
    <meta name = 'twitter:creator' content = '@CarWale' />
    <meta name = 'twitter:image' content = '{3}' /> ", card, title, description, imageUrl);
            return new MvcHtmlString(str);
        }

        public static MvcHtmlString GetPagination(string urlTemplate, int currentPage, int totalPages, int paginationWidth)
        {
            if(totalPages > 1)
            {
                int startPage = currentPage + paginationWidth - 1 > totalPages && totalPages > paginationWidth ? totalPages - paginationWidth + 1 : currentPage;
                startPage = totalPages <= paginationWidth ? 1 : startPage;
                StringBuilder paginationHtml = new StringBuilder();

                paginationHtml.Append(@"
                            <div class='text-center padding-top15'>
                            <ul class='pagination bg-white'>");
                string prevUrl = currentPage > 1 ? string.Format(urlTemplate, currentPage - 1) : "#";
                string prevClass = currentPage == 1 ? "active" : string.Empty;
                paginationHtml.Append($"<li><a href='{prevUrl}' class='{prevClass}'>Previous</a></li>");
                
                for (int i = startPage; i <= Math.Min(currentPage + paginationWidth - 1, totalPages) ; i++)
                {
                    string activeClass = i.Equals(currentPage) ? "active" : string.Empty;
                    paginationHtml.Append($"<li><a href='{string.Format(urlTemplate, i)}' class={activeClass}>{i}</a></li>");
                }

                string nexturl = currentPage != totalPages ? string.Format(urlTemplate, currentPage + 1) : "#";
                string nextClass = currentPage == totalPages ? "active" : string.Empty;
                paginationHtml.Append($"<li><a href='{nexturl}' class='{nextClass}'>Next</a></li>");

                paginationHtml.Append(@"</ul></div>");
                return new MvcHtmlString(paginationHtml.ToString());
            }
            return new MvcHtmlString(string.Empty);
        }

    }
}