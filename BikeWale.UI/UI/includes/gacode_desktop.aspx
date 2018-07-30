<%@ Import namespace ="Bikewale.Utility" %>
<%@ Import namespace ="Bikewale.Controllers.Shared" %>
bwHostUrl = '<%= ConfigurationManager.AppSettings["bwHostUrlForJs"] %>';
var ga_pg_id = '0';

(function (w, d, s, l, i) {
w[l] = w[l] || []; w[l].push({
'gtm.start':
new Date().getTime(), event: 'gtm.js'
}); var f = d.getElementsByTagName(s)[0],
j = d.createElement(s), dl = l != 'dataLayer' ? '&l=' + l : ''; j.async = true; j.src =
'//www.googletagmanager.com/gtm.js?id=' + i + dl; f.parentNode.insertBefore(j, f);
})(window, document, 'script', 'dataLayer', 'GTM-5CSHD6');

var googletag = googletag || {};
googletag.cmd = googletag.cmd || [];
(function () {
    var gads = document.createElement('script');
    gads.async = true;
    gads.type = 'text/javascript';
    var useSSL = 'https:' == document.location.protocol;
    gads.src = (useSSL ? 'https:' : 'http:') +
    '//www.googletagservices.com/tag/js/gpt.js?v=1.0';
    var node = document.getElementsByTagName('script')[0];
    node.parentNode.insertBefore(gads, node);
})();

googletag.cmd.push(function () {
    <% if(isAd300x250Shown){ %>
    googletag.defineSlot('<%= AdPath%>300x250', [[300, 250]], 'div-gpt-ad-<%= AdId%>-1').addService(googletag.pubads());                    
    <% } %>
    <% if(isAd300x250BTFShown){ %>
    googletag.defineSlot('<%= AdPath%>300x250_BTF', [[300, 250]], 'div-gpt-ad-<%= AdId%>-2').addService(googletag.pubads());        
    <% } %>
    <% if(isAd970x90Shown){ %>
    googletag.defineSlot('<%= AdPath%>970x90', [[970, 66], [970, 60], [960, 90], [950, 90], [960, 66], [728, 90], [960, 60], [970, 90]], 'div-gpt-ad-<%= AdId%>-3').addService(googletag.pubads()); 
    <% } %>
    <% if(isAd970x90BTFShown){ %>
    googletag.defineSlot('<%= AdPath%>970x90_BTF', [[970, 200],[970, 150],[960, 60], [970, 66], [960, 90], [970, 60], [728, 90], [970, 90], [960, 66]], 'div-gpt-ad-<%= AdId%>-4').addService(googletag.pubads());
    <% } %>
    <% if(isAd970x90BottomShown){ %>
    googletag.defineSlot('<%= AdPath%>Bottom_970x90', [[970, 60], [960, 90], [970, 66], [960, 66], [728, 90], [970, 90], [950, 90], [960, 60]], 'div-gpt-ad-<%= AdId%>-5').addService(googletag.pubads());
    <% } %>
    <% if (isAd976x400FirstShown){ %>
    googletag.defineSlot('<%= AdPath%>FirstSlot_976x400',[[976, 150], [976, 100], [976, 250], [976, 300], [976, 350], [976, 400], [970, 90], [976, 200]],'div-gpt-ad-<%= AdId%>-6').addService(googletag.pubads());
    <% } %>
    <% if (isAd976x400SecondShown) { %>
    googletag.defineSlot('<%= AdPath%>SecondSlot_976x400',[[976, 150], [976, 100], [976, 250], [976, 300], [976, 350], [976, 400], [970, 90], [976, 200]],'div-gpt-ad-<%= AdId%>-7').addService(googletag.pubads());
    <% } %>
    <% if (isAd976x204) { %>
    googletag.defineSlot('/1017752/BikeWale_HomePageNews_FirstSlot_976x204', [[976, 200], [976, 250], [976, 204]], 'div-gpt-ad-1395985604192-8').addService(googletag.pubads());
    <% } %>
        
    googletag.pubads().enableSingleRequest();
    <% if(!String.IsNullOrEmpty(TargetedModel)){%>googletag.pubads().setTargeting("Model", "<%= TargetedModel.RemoveSpecialCharacters() %>");<%}%>             
    <% if(!String.IsNullOrEmpty(TargetedMake)){%>googletag.pubads().setTargeting("Make", "<%= TargetedMake.RemoveSpecialCharacters() %>");<%}%>
    <% if(!String.IsNullOrEmpty(TargetedModels)){%>googletag.pubads().setTargeting("CompareBike-D", "<%= TargetedModels.RemoveSpecialCharacters() %>");<%}%>
    <% if (!String.IsNullOrEmpty(TargetedCity)){%>googletag.pubads().setTargeting("City", "<%= TargetedCity.RemoveSpecialCharacters() %>");<%}%>
        
    <% MvcHelper.RenderAction("GetUserProfileTargeting", "GoogleAds"); %>
    
    googletag.pubads().collapseEmptyDivs();
    googletag.pubads().enableSingleRequest();
    googletag.enableServices();
});
