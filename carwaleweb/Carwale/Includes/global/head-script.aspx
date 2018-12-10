<%@ Import Namespace="Carwale.UI.NewCars.RecommendCars" %>
<meta charset="utf-8">
<meta http-equiv="X-UA-Compatible" content="IE=edge">
<title itemprop="name"><%=Title%> - CarWale</title>
<meta name="keywords" content="<%="" + Keywords%>" />
<meta name="viewport" content="width=device-width, initial-scale=1.0">
<meta itemprop="description" name="description" content="<%="" + Description %>" />
<meta name="google-site-verification" content="y33JBqCALrAtfZBeI-VVHz7jyLJrZqpOsggckAZlBj8" />
<% if (noIndex) { %>
<meta name="robots" content="noindex" />
<% } %>
<% if( canonical != string.Empty ) { %>
<link rel="canonical" href="<%="" + canonical%>" />
<% } %>
<% if( altUrl != string.Empty ){%>
<link rel="alternate" type="text/html" media="handheld" href="<%= "" + altUrl%>"
    title="Mobile/PDA" />
<%} %>
<% if(!string.IsNullOrEmpty(prevPageUrl)) { %><link rel="prev" href="<%=prevPageUrl %>" /><% } %>
<% if(!string.IsNullOrEmpty(nextPageUrl)) { %><link rel="next" href="<%=nextPageUrl %>" /><% } %>
<% if(!string.IsNullOrEmpty(ampUrl)) { %><link rel="amphtml" href="<%=ampUrl %>" /><% } %>
<link rel="SHORTCUT ICON" href="<%= Carwale.Utility.CWConfiguration._imgHostUrl %>0x0/cw/design15/carwale.png?staticimgsfix" />
<%if (!string.IsNullOrWhiteSpace(DeeplinkAlternativesAndroid))
  {%>
<link rel="alternate" href="android-app://com.carwale/carwaleandroid1/www.carwale.com<%=DeeplinkAlternativesAndroid %>" />
<%} 
if(!string.IsNullOrWhiteSpace(DeeplinkAlternatives)) {%>
<%--<link rel="alternate" href="ios-app://910137745/ioscarwale/www.carwale.com<%=DeeplinkAlternatives %>" />--%>
<%}%>
<link rel="stylesheet" href="/static/css/cw-common-style.css" type="text/css" >
<link rel="stylesheet" href="/static/cwfontawesome/cw-font-awesome.css" type="text/css">
<link rel="stylesheet" href="/static/css/location-plugin.css" type="text/css" >
<script  type="text/javascript"  src="/static/js/frameworks.js" ></script>
<% RazorPartialBridge.RenderPartial("~/Views/StaticPartials/FontCss/CwFonts.cshtml"); %>

<script type="text/javascript">
    var FACEBOOKAPPID = '<%=System.Configuration.ConfigurationManager.AppSettings["FacebookAppId"]%>';
    var FACEBOOKPIXELID = '<%=System.Configuration.ConfigurationManager.AppSettings["FacebookPixelId"]%>';
    var CLIENTID = '<%=System.Configuration.ConfigurationManager.AppSettings["GoogleClientId"]%>';
    var SCOPE = '<%=System.Configuration.ConfigurationManager.AppSettings["GoogleProjectScope"]%>';
    var REDIRECT = '<%=System.Configuration.ConfigurationManager.AppSettings["GoogleRedirectURL"]%>';     
</script>
<script language="c#" runat="server">	
    string adPath = System.Configuration.ConfigurationManager.AppSettings["adPath"];
    public string Title = "", Description = "", Keywords = "", Revisit = "", DocumentState = "Static";
    public string OEM = "", BodyType = "", Segment = "";//for keyword based google Ads
    public string canonical = "", prevPageUrl = "", nextPageUrl = "", ampUrl = "";
    public string AdId = "", AdPath = "", altUrl = "";  // variables for google ad script
    public string targetKey="", targetValue="";
    public bool Ad643 = false;
    public string DeeplinkAlternatives = string.Empty,DeeplinkAlternativesAndroid=string.Empty;
    public bool noIndex = false;
</script>
<script type='text/javascript'>
    var googletag = googletag || {};
    googletag.cmd = googletag.cmd || [];
    (function () {
        var gads = document.createElement('script');
        gads.async = true;
        gads.type = 'text/javascript';
        var useSSL = 'https:' == document.location.protocol;
        gads.src = (useSSL ? 'https:' : 'http:') +
        '//www.googletagservices.com/tag/js/gpt.js';
        var node = document.getElementsByTagName('script')[0];
        node.parentNode.insertBefore(gads, node);
    })();
</script>
<script>
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
<script>(function (w, d, t, r, u) { var f, n, i; w[u] = w[u] || [], f = function () { var o = { ti: "5440133" }; o.q = w[u], w[u] = new UET(o), w[u].push("pageLoad") }, n = d.createElement(t), n.src = r, n.async = 1, n.onload = n.onreadystatechange = function () { var s = this.readyState; s && s !== "loaded" && s !== "complete" || (f(), n.onload = n.onreadystatechange = null) }, i = d.getElementsByTagName(t)[0], i.parentNode.insertBefore(n, i) })(window, document, "script", "//bat.bing.com/bat.js", "uetq");</script>
<!--CrazyEgg heatmaps  -->
<script type="text/javascript">
    setTimeout(function () {
        var a = document.createElement("script");
        var b = document.getElementsByTagName("script")[0];
        a.src = document.location.protocol + "//script.crazyegg.com/pages/scripts/0012/9477.js?" + Math.floor(new Date().getTime() / 3600000);
        a.async = true; a.type = "text/javascript"; b.parentNode.insertBefore(a, b)
    }, 1);
</script>
<!-- Global Site Tag (gtag.js) - Google AdWords: 999894061 -->
<script async src="https://www.googletagmanager.com/gtag/js?id=AW-999894061"></script>
<script>
    window.dataLayer = window.dataLayer || [];
    function gtag() { dataLayer.push(arguments); }
    gtag('js', new Date());
    gtag('config', 'AW-999894061');
</script>
<% RazorPartialBridge.RenderAction("GetAdTagetingData", "UserProfile", new Dictionary<string,object>{ {"cwcCookieId" , Carwale.UI.Common.CurrentUser.CWC}, {"platform" , Carwale.Entity.Enum.Platform.CarwaleDesktop }}); %>  