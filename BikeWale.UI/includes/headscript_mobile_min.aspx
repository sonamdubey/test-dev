<meta charset="utf-8">
<script language="c#" runat="server">
    private string title = "", relPrevPageUrl = string.Empty, relNextPageUrl = string.Empty,description = "", keywords = "", AdId = "", AdPath = "", canonical = "", TargetedModel = "", TargetedMakes = "", TargetedModels = "", TargetedCity = ""
        , OGImage = "";
    private ushort feedbackTypeId = 0;
    string staticUrl = System.Configuration.ConfigurationManager.AppSettings["staticUrl"];
    string staticFileVersion = System.Configuration.ConfigurationManager.AppSettings["staticFileVersion"];
    private bool Ad_320x50 = false, Ad_Bot_320x50 = false, Ad_300x250 = false, Ad320x150_I = false, Ad320x150_II = false,
        EnableOG = true, Ad_Mid_320x50 = false, ShowSellBikeLink = false;    
</script>
<meta http-equiv="X-UA-Compatible" content="IE=edge">
<title><%=title %></title>
<meta name="description" content="<%=description%>" />
<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0, user-scalable=no" />
<meta name="google-site-verification" content="fG4Dxtv_jDDSh1jFelfDaqJcyDHn7_TCJH3mbvq6xW8" />
<meta name="theme-color" content="#2a2a2a" />
<% if(!String.IsNullOrEmpty(keywords)) { %><meta name="keywords" content="<%= keywords %>" /><% } %>
<%if (EnableOG)
  { %>
    <meta property="og:title" content="<%=title %>" />
    <meta property="og:type" content="website" />
    <meta property="og:description" content="<%=description%>" />
    <%if(!String.IsNullOrEmpty(canonical)) { %><meta property="og:url" content="<%=canonical %>" /> <% } %>
    <meta property="og:image" content="<%= string.IsNullOrEmpty(OGImage) ? Bikewale.Utility.BWConfiguration.Instance.BikeWaleLogo : OGImage %>" />
<% } %>
<%if (!String.IsNullOrEmpty(canonical)) { %> <link rel="canonical" href="<%=canonical %>" /><% } %>
<%if (!String.IsNullOrEmpty(relPrevPageUrl)){ %> <link rel="prev" href="<%= relPrevPageUrl %>" /><% } %>
<%if(!String.IsNullOrEmpty(relNextPageUrl)){ %> <link rel="next" href="<%= relNextPageUrl %>" /><% }%>

<link rel="SHORTCUT ICON" href="https://imgd1.aeplcdn.com/0x0/bw/static/sprites/d/favicon.png"  type="image/png"/>

<link rel="stylesheet" type="text/css" href="/m/css/bwm-common-atf.css" />

<noscript><iframe src="//www.googletagmanager.com/ns.html?id=GTM-5CSHD6"
height="0" width="0" style="display:none;visibility:hidden"></iframe></noscript>

<% Bikewale.Utility.BWCookies.SetBWUtmz(); %>
