<script language="c#" runat="server">	
    private string staticUrl = System.Configuration.ConfigurationManager.AppSettings["staticUrl"];
    private string staticFileVersion = System.Configuration.ConfigurationManager.AppSettings["staticFileVersion"];
    private string title = "", description = "", keywords = "", AdId = "", AdPath = "", alternate = "", ShowTargeting = "", TargetedModel = "", TargetedSeries = "", TargetedMake = "", TargetedModels = "", canonical = "", TargetedCity = ""
        , fbTitle = "", fbImage,
        ogImage = "";
    private ushort feedbackTypeId = 0;
    private bool isHeaderFix = true,
        isAd970x90Shown = true,
        isAd970x90BTFShown = false,
        isAd970x90BottomShown = true,
        isAd976x400FirstShown = false,
        isAd976x400SecondShown = false,
        isAd976x204 = false,
        isAd300x250BTFShown=true,
        isAd300x250Shown=true,
        isTransparentHeader = false,
        enableOG = true;
</script>

<title><%= title %></title>
<meta name="description" content="<%= description %>" />
<meta charset="utf-8"/>
<meta http-equiv="X-UA-Compatible" content="IE=edge"/>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />    
<meta name="google-site-verification" content="fG4Dxtv_jDDSh1jFelfDaqJcyDHn7_TCJH3mbvq6xW8" />
<% if(!String.IsNullOrEmpty(keywords)) { %><meta name="keywords" content="<%= keywords %>" /><% } %>
<%if(!String.IsNullOrEmpty(alternate)) { %><meta name="alternate" content="<%= alternate %>" /><% } %>
<%if(!String.IsNullOrEmpty(canonical)) { %>
<link rel="canonical" href="<%=canonical %>" /> 
<% } %>
<%if(enableOG) { %>
<meta property="og:title" content="<%= title %>" />
<meta property="og:type" content="website" />
<meta property="og:description" content="<%= description %>" />
<%if(!String.IsNullOrEmpty(canonical)) { %><meta property="og:url" content="<%=canonical %>" /> <% } %>
<meta property="og:image" content = "<%= string.IsNullOrEmpty(ogImage) ? Bikewale.Utility.BWConfiguration.Instance.BikeWaleLogo : ogImage %>" />
<% } %>

<link rel="SHORTCUT ICON" href="http://imgd1.aeplcdn.com/0x0/bw/static/sprites/d/favicon.png"  type="image/png"/>
<link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/bw-common-atf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />

<noscript><iframe src="//www.googletagmanager.com/ns.html?id=GTM-5CSHD6"
height="0" width="0" style="display:none;visibility:hidden"></iframe></noscript>

<% Bikewale.Utility.BWCookies.SetBWUtmz(); %>
