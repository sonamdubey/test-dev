<script language="c#" runat="server">	
    string staticUrl = System.Configuration.ConfigurationManager.AppSettings["staticUrl"];
    private string title = "", description = "", keywords = "", AdId = "", AdPath = "", alternate = "", ShowTargeting = "", TargetedModel = "", TargetedSeries = "", TargetedMake = "", TargetedModels = "", canonical = "";
    private string fbTitle = "", fbImage;
    private ushort feedbackTypeId = 0; 	 
    private bool isHeaderFix = true;   
</script>

<meta charset="utf-8">
<meta http-equiv="X-UA-Compatible" content="IE=edge">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />    
<meta name="keywords" content="<%= keywords %>" />
<meta name="description" content="<%= description %>" />
<meta name="alternate" content="<%= alternate %>" />
<link rel="canonical" href="<%=canonical %>" /> 

<title><%= title %></title>
<link rel="SHORTCUT ICON" href="#" />
<link href="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/css/bw-common-style.css?15sept2015" rel="stylesheet" type="text/css">
<script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/src/frameworks.js?14sept2015"></script>
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
        googletag.defineSlot('<%= AdPath%>728x90', [728, 90], 'div-gpt-ad-<%= AdId%>-0').addService(googletag.pubads());
        googletag.defineSlot('<%= AdPath%>300x250', [300, 250], 'div-gpt-ad-<%= AdId%>-1').addService(googletag.pubads());                    
        googletag.defineSlot('<%= AdPath%>_300x250_BTF', [300, 250], 'div-gpt-ad-<%= AdId%>-2').addService(googletag.pubads());                    
        <% if(!String.IsNullOrEmpty(ShowTargeting)) { %>
            googletag.pubads().setTargeting("Model", "<%= TargetedModel %>");
            googletag.pubads().setTargeting("Series", "<%= TargetedSeries %>");
            googletag.pubads().setTargeting("Make", "<%= TargetedMake %>");
            googletag.pubads().setTargeting("CompareBike-D", "<%= TargetedModels %>");
        <% } %>
        googletag.pubads().collapseEmptyDivs();
        googletag.pubads().enableSingleRequest();
        googletag.enableServices();
    });
</script>
<!-- for IE to understand the new elements of HTML5 like header, footer, section and so on -->
<!--[if lt IE 9]>
    <script src="/src/html5.js"></script>
<![endif]-->
<script type="text/javascript">
    bwHostUrl = '<%= ConfigurationManager.AppSettings["bwHostUrl"] %>';
</script>
