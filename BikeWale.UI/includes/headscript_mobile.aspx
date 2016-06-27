<meta charset="utf-8">
<script language="c#" runat="server">
    private string title = "", description = "", keywords = "", AdId = "", AdPath = "", canonical = "", TargetedModel = "", TargetedMakes = "", TargetedModels = "", TargetedCity = ""
        , OGImage = "";
    private ushort feedbackTypeId = 0;
    string staticUrl = System.Configuration.ConfigurationManager.AppSettings["staticUrl"];
    string staticFileVersion = System.Configuration.ConfigurationManager.AppSettings["staticFileVersion"];
    private bool Ad_320x50 = false, Ad_Bot_320x50 = false, Ad_300x250 = false, Ad320x150_I = false, Ad320x150_II = false,
        EnableOG = true;    
</script>
<meta http-equiv="X-UA-Compatible" content="IE=edge">
<title><%=title %></title>
<meta name="description" content="<%=description%>" />
<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0, user-scalable=no" />
<meta name="google-site-verification" content="fG4Dxtv_jDDSh1jFelfDaqJcyDHn7_TCJH3mbvq6xW8" />
<% if(!String.IsNullOrEmpty(keywords)) { %><meta name="keywords" content="<%= keywords %>" /><% } %>
<%if (!String.IsNullOrEmpty(canonical))
  { %>
    <link rel="canonical" href="<%=canonical %>" />
<% } %>
<link rel="SHORTCUT ICON" href="http://imgd1.aeplcdn.com/0x0/bw/static/sprites/d/favicon.png"  type="image/png"/>
<link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />
<link href="/m/css/bwm-common-style.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />

<%if (EnableOG)
  { %>
    <meta property="og:title" content="<%=title %>" />
    <meta property="og:type" content="website" />
    <meta property="og:description" content="<%=description%>" />
    <%if(!String.IsNullOrEmpty(canonical)) { %><meta property="og:url" content="<%=canonical %>" /> <% } %>
    <meta property="og:image" content="<%= string.IsNullOrEmpty(OGImage) ? Bikewale.Utility.BWConfiguration.Instance.BikeWaleLogo : OGImage %>" />
<% } %>


<script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>
<script type="text/javascript">
   var ga_pg_id = '0';    
</script>
<!-- #include file="\includes\gacode.aspx" -->
<script type="text/javascript">
    setTimeout(function () {
        var a = document.createElement("script");
        var b = document.getElementsByTagName("script")[0];
        a.src = document.location.protocol + "//script.crazyegg.com/pages/scripts/0012/9477.js?" + Math.floor(new Date().getTime() / 3600000);
        a.async = true; a.type = "text/javascript"; b.parentNode.insertBefore(a, b)
    }, 1);
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

<script type='text/javascript'>
    googletag.cmd.push(function () {
        <% if(Ad_320x50) { %>googletag.defineSlot('<%= AdPath%>_Top_320x50', [320, 50], 'div-gpt-ad-<%= AdId%>-0').addService(googletag.pubads());<% } %>
        <% if(Ad_Bot_320x50) { %>googletag.defineSlot('<%= AdPath%>_Bottom_320x50', [320, 50], 'div-gpt-ad-<%= AdId%>-1').addService(googletag.pubads());<% } %>
        <% if (Ad_300x250) { %>googletag.defineSlot('<%= AdPath%>_300x250', [300, 250], 'div-gpt-ad-<%= AdId%>-2').addService(googletag.pubads());<% } %>
        <% if (Ad320x150_I) { %>
        googletag.defineSlot('<%= AdPath%>_FirstSlot_320x150', [[320, 150], [320, 50], [320, 100], [320, 425]], 'div-gpt-ad-<%= AdId%>-3').addService(googletag.pubads());
        <% } %>
        <% if (Ad320x150_II) { %>
        googletag.defineSlot('<%= AdPath%>_SecondSlot_320x150', [[320, 150], [320, 50], [320, 100], [320, 425]], 'div-gpt-ad-<%= AdId%>-4').addService(googletag.pubads());
        <% } %>
        <% if (!String.IsNullOrEmpty(TargetedModel)) { %>googletag.pubads().setTargeting("Model", "<%= TargetedModel %>");<% } %>
        <% if (!String.IsNullOrEmpty(TargetedMakes)){ %>googletag.pubads().setTargeting("Make", "<%= TargetedMakes %>");<% } %>
        <% if (!String.IsNullOrEmpty(TargetedModels)){ %>googletag.pubads().setTargeting("CompareBike-M", "<%= TargetedModels %>");<% } %>
        <% if (!String.IsNullOrEmpty(TargetedCity)){%>googletag.pubads().setTargeting("City", "<%= TargetedCity %>");<%}%>
        googletag.pubads().enableSingleRequest();
        googletag.pubads().collapseEmptyDivs();
        googletag.enableServices();
    });
</script>
<% Bikewale.Utility.BWCookies.SetBWUtmz(); %>
<!-- for IE to understand the new elements of HTML5 like header, footer, section and so on -->
<!--[if lt IE 9]>
    <script src="/m/src/html5.js"></script>
<![endif]-->