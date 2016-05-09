<script language="c#" runat="server">	
    private string staticUrl = System.Configuration.ConfigurationManager.AppSettings["staticUrl"];
    private string staticFileVersion = System.Configuration.ConfigurationManager.AppSettings["staticFileVersion"];
    private string title = "", description = "", keywords = "", AdId = "", AdPath = "", alternate = "", ShowTargeting = "", TargetedModel = "", TargetedSeries = "", TargetedMake = "", TargetedModels = "", canonical = "", TargetedCity = ""
        , fbTitle = "", fbImage,
        ogImage;
    private ushort feedbackTypeId = 0;
    private bool isHeaderFix = true,
        isAd970x90Shown = true,
        isAd970x90BTFShown = false,
        isAd970x90BottomShown = true,
        isAd976x400FirstShown = false,
        isAd976x400SecondShown = false,
        isAd976x204 = false,
        isTransparentHeader = false,
        enableOG = false;  
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
    <meta property="og:image" content = "<%= string.IsNullOrEmpty(ogImage) ? Bikewale.Utility.Image.BikewaleLogo : ogImage %>" />
<% } %>



<script type="text/javascript">
    bwHostUrl = '<%= ConfigurationManager.AppSettings["bwHostUrlForJs"] %>';
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
<link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />
<link href="/css/bw-common-style.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/src/frameworks.js?<%=staticFileVersion %>"></script>
<script type='text/javascript'>
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
</script>
<script type='text/javascript'>
    googletag.cmd.push(function () {
        googletag.defineSlot('<%= AdPath%>300x250', [[300, 250]], 'div-gpt-ad-<%= AdId%>-1').addService(googletag.pubads());                    
        googletag.defineSlot('<%= AdPath%>300x250_BTF', [[300, 250]], 'div-gpt-ad-<%= AdId%>-2').addService(googletag.pubads());        
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
        <% if(!String.IsNullOrEmpty(TargetedModel)){%>googletag.pubads().setTargeting("Model", "<%= TargetedModel %>");<%}%>             
        <% if(!String.IsNullOrEmpty(TargetedMake)){%>googletag.pubads().setTargeting("Make", "<%= TargetedMake %>");<%}%>
        <% if(!String.IsNullOrEmpty(TargetedModels)){%>googletag.pubads().setTargeting("CompareBike-D", "<%= TargetedModels %>");<%}%>
        <% if (!String.IsNullOrEmpty(TargetedCity)){%>googletag.pubads().setTargeting("City", "<%= TargetedCity %>");<%}%>
        
        googletag.pubads().collapseEmptyDivs();
        googletag.pubads().enableSingleRequest();
        googletag.enableServices();
    });
</script>
<!-- for IE to understand the new elements of HTML5 like header, footer, section and so on -->
<!--[if lt IE 9]>
    <script src="/src/html5.js"></script>
<![endif]-->
