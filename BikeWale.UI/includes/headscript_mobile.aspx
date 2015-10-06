<meta charset="utf-8">
<script language="c#" runat="server">
    private string title = "", description = "", keywords = "", AdId = "", AdPath = "", alternate = "", canonical = "", TargetedModel = "", TargetedMakes = "", TargetedModels = "";
    private ushort feedbackTypeId = 0;
    string staticUrl = System.Configuration.ConfigurationManager.AppSettings["staticUrl"];
    string staticFileVersion = System.Configuration.ConfigurationManager.AppSettings["staticFileVersion"];
    private bool Ad_320x50 = false, Ad_Bot_320x50 = false, Ad_300x250 = false;    
</script>
<meta http-equiv="X-UA-Compatible" content="IE=edge">
<title><%=title %></title>
<meta name="description" content="<% =description%>" />
<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0, user-scalable=no" />
<% if(!String.IsNullOrEmpty(keywords)) { %><meta name="keywords" content="<%= keywords %>" /><% } %>
<%if(!String.IsNullOrEmpty(canonical)) { %><link rel="canonical" href="<%=canonical %>" /><% } %>
<link rel="SHORTCUT ICON" href="http://img2.aeplcdn.com/bikewaleimg/images/favicon.png"  type="image/png"/>
<link href="/m/css/bwm-common-style.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css">
<script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>
<script type="text/javascript">
   var ga_pg_id = '0';
    //	new\default.aspx  = 3//	new\versions.aspx = 2//	default.aspx = 1//	includes\headscript.aspx = 0
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
        <% if (!String.IsNullOrEmpty(TargetedModel)) { %>googletag.pubads().setTargeting("Model", "<%= TargetedModel %>");<% } %>
        <% if (!String.IsNullOrEmpty(TargetedMakes)){ %>googletag.pubads().setTargeting("Make", "<%= TargetedMakes %>");<% } %>
        <% if (!String.IsNullOrEmpty(TargetedModels)){ %>googletag.pubads().setTargeting("CompareBike-M", "<%= TargetedModels %>");<% } %>
        googletag.pubads().enableSingleRequest();
        googletag.pubads().collapseEmptyDivs();
        googletag.enableServices();
    });
</script>
<!-- for IE to understand the new elements of HTML5 like header, footer, section and so on -->
<!--[if lt IE 9]>
    <script src="/m/src/html5.js"></script>
<![endif]-->
<!-- #include file="\includes\gacode.aspx" -->