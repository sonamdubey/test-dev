<script language="c#" runat="server">
    public string google_dynx_itemid = "", google_dynx_itemid2 = "", google_dynx_pagetype = "", google_dynx_totalvalue = "";
</script>
<script type="text/javascript">
    var cwTrackingPath = '<%= ConfigurationManager.AppSettings["HostUrl"] %>';
    var insuranceKeys = {
        cholaStates: [<%=System.Configuration.ConfigurationManager.AppSettings["InsuranceStates"]??""%>]
    }
    var defaultCookieDomain = '<%= Carwale.UI.Common.CookiesCustomers.CookieDomain%>';
     var isEligibleForORP = '<%=Carwale.UI.Common.CookiesCustomers.IsEligibleForORP%>' === 'True';
    var abTestKey = "<%= System.Configuration.ConfigurationManager.AppSettings["AbTestVersion"] %>";
    var abTestKeyMaxValue = "<%= System.Configuration.ConfigurationManager.AppSettings["AbTestKeyMaxValue"] %>";
    var abTestKeyMinValue = "<%= System.Configuration.ConfigurationManager.AppSettings["AbTestKeyMinValue"] %>";
    var askingAreaCityId = [<%=System.Configuration.ConfigurationManager.AppSettings["AskingAreaCityIds"]%>];
</script>
<script  type="text/javascript" src="/static/m/js/plugins.js"></script>
<script type="text/javascript" src="/static/m/js/easy-autocomplete.js"></script>
<script  type="text/javascript" src="/static/js/adsense.js"></script>
<script  type="text/javascript" src="/static/js/v2/app/clientcache.js"></script>
<script  type="text/javascript" src="/static/js/globalSearch.js"></script>
<script type="text/javascript" src="/static/m/js/sponsored-navigation.js"></script>
<script type="text/javascript" src="/static/js/error-logger.js"></script>
<script  type="text/javascript" src="/static/m/js/v2/app/common.js"></script>
<script  type="text/javascript" src="/static/js/lead-conversion-tracking.js"></script>
<script  type="text/javascript" src="/static/m/js/location.js"></script>
<script  type="text/javascript" src="/static/js/advantage-citypopup.js"></script>
<script  type="text/javascript" src="/static/js/cookie-migration.js"></script>
<script  type="text/javascript" src="/static/js/v2/app/cwTracking.js"></script>
<script>
    Common.googleApiKey = '<%= (System.Configuration.ConfigurationManager.AppSettings["APIKey"] ?? "") %>';
    </script>
<!-- Google Tag Manager -->
<noscript>
    <iframe src="//www.googletagmanager.com/ns.html?id=GTM-W2Z3ZM"
        height="0" width="0" style="display: none; visibility: hidden"></iframe>
</noscript>
<script>(function (w, d, s, l, i) {
    w[l] = w[l] || []; w[l].push({
        'gtm.start':
        new Date().getTime(), event: 'gtm.js'
    }); var f = d.getElementsByTagName(s)[0],
    j = d.createElement(s), dl = l != 'dataLayer' ? '&l=' + l : ''; j.async = true; j.src =
    '//www.googletagmanager.com/gtm.js?id=' + i + dl; f.parentNode.insertBefore(j, f);
})(window, document, 'script', 'dataLayer', 'GTM-W2Z3ZM');</script>
<!-- End Google Tag Manager -->

<!-- Google Code for Remarketing Tag -->
<!--------------------------------------------------
Remarketing tags may not be associated with personally identifiable information or placed on pages related to sensitive categories. See more information and instructions on how to setup the tag on: http://google.com/ads/remarketingsetup
--------------------------------------------------->
<div class="hide">
    <script type="text/javascript">
        var google_tag_params = {
            dynx_itemid: '<%= google_dynx_itemid %>',
        dynx_CarName: '<%= google_dynx_itemid2 %>',
        dynx_pagetype: '<%= google_dynx_pagetype %>s',
        dynx_totalvalue: '<%= google_dynx_totalvalue %>',
        };
    </script>

    <script>
        var FACEBOOKPIXELID = '<%=System.Configuration.ConfigurationManager.AppSettings["FacebookPixelId"]%>';
        !function (f, b, e, v, n, t, s) {
            if (f.fbq) return; n = f.fbq = function () {
                n.callMethod ?
                n.callMethod.apply(n, arguments) : n.queue.push(arguments)
            }; if (!f._fbq) f._fbq = n;
            n.push = n; n.loaded = !0; n.version = '2.0'; n.queue = []; t = b.createElement(e); t.async = !0;
            t.src = v; s = b.getElementsByTagName(e)[0]; s.parentNode.insertBefore(t, s)
        }(window,
        document, 'script', 'https://connect.facebook.net/en_US/fbevents.js');
        fbq('init', FACEBOOKPIXELID);
        fbq('track', 'PageView');
</script>
    <% Carwale.UI.NewCars.RecommendCars.RazorPartialBridge.RenderPartial("~/Views/Shared/_OutbrainScriptView.cshtml"); %>
<script>(function (w, d, t, r, u) { var f, n, i; w[u] = w[u] || [], f = function () { var o = { ti: "5440133" }; o.q = w[u], w[u] = new UET(o), w[u].push("pageLoad") }, n = d.createElement(t), n.src = r, n.async = 1, n.onload = n.onreadystatechange = function () { var s = this.readyState; s && s !== "loaded" && s !== "complete" || (f(), n.onload = n.onreadystatechange = null) }, i = d.getElementsByTagName(t)[0], i.parentNode.insertBefore(n, i) })(window, document, "script", "//bat.bing.com/bat.js", "uetq");</script>
    <script>
        dataLayer.push({
            'event': 'remarketingTriggered',
            'google_tag_params': window.google_tag_params
        });
    </script>
    <script type="text/javascript">
        /* <![CDATA[ */
        var google_conversion_id = 1013901406;
        var google_custom_params = window.google_tag_params;
        var google_remarketing_only = true;
        var google_conversion_format = 3;
        /* ]]> */
    </script>
    <script type="text/javascript" src="//www.googleadservices.com/pagead/conversion.js"></script>
    <script type="text/javascript" src="//www.googleadservices.com/pagead/conversion_async.js"></script>
    <noscript>
        <div style="display: inline;">
            <img height="1" width="1" style="border-style: none;" alt="" src="//googleads.g.doubleclick.net/pagead/viewthroughconversion/1013901406/?value=0&amp;guid=ON&amp;script=0" />
        </div>
    </noscript>
</div>
