<%@ Import Namespace="Carwale.UI.Common" %>
<%@ Register TagPrefix="Vspl" TagName="LoginStatus" src="/Controls/loginStatus.ascx" %>
<%@ Register TagPrefix="uc" TagName="GetUserCity" Src="/Controls/GetUserCity.ascx"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<title><%=Title%> - CarWale</title>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<meta name="keywords" content="<%=Keywords%>" />
<meta name="description" content="<%=Description%>" />

<!-- #include file="/includes/global/globalScript.aspx"-->
<link rel="stylesheet" href="/static/css/cw-misk.css" type="text/css" >
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
        googletag.defineSlot('<%= AdPath %>970x90', [[220, 90], [728, 90], [950, 90], [960, 90], [970, 66], [970, 90]], 'div-gpt-ad-<%= AdId %>-2').addService(googletag.pubads());
        googletag.defineSlot('<%= AdPath %>220x90', [220, 90], 'div-gpt-ad-<%= AdId %>-3').addService(googletag.pubads());

        //googletag.pubads().enableSyncRendering();
        googletag.pubads().collapseEmptyDivs();
        googletag.pubads().enableSingleRequest();
        googletag.enableServices();
    });
</script>
<script language="c#" runat="server">
	private int PageId = 1;
	private string Title = "", Description = "", Keywords = "", Revisit = "", DocumentState = "Static";	
	private string OEM = "", BodyType = "", Segment = "";//for keyword based google Ads 
    private string AdId = "", AdPath = "";  // variables for google ad script
</script>
<!--[if IE 6]>
<script src="https://st.carwale.com/ie-png-fix.js"></script>
<script>
 DD_belatedPNG.fix('.home-icon');/* fix png transparency problem with IE6 */
</script>
<![endif]-->
</head>
<body id="ros">
    <!-- #include file="/includes/global/headTopNavNew.aspx"-->    
    <!-- primary Navbar code start here -->
    <div class="primary-navbar">
        <div class="row">
            <div class="column grid_12">
            	<div class="leftfloat margin-top15"><a href="/" class="cw-sprite cw-logo"></a></div>
                <div id="primary-navbar" class="leftfloat">
                    <ul class="primary-navbar-list">
                        <li class="sept-prim"></li>
                        <li><a href="/" class="cw-sprite home-icon" title="Home"></a></li>
                    	<li class="sept-prim"></li>
                        <li><a href="/new/">New Cars</a></li>
                        <li class="sept-prim"></li>
                        <li><a href="/new/prices.aspx">On-Road Price</a></li>
                        <li class="sept-prim"></li>
                        <li><a href="/used/">Used Cars</a></li>
                        <li class="sept-prim"></li>
                        <li><a href="/used/sell/" >Sell Car</a></li>
                        <li class="sept-prim"></li>
                        <li><a href="/reviews-news/">Reviews & News</a></li>
                        <li class="sept-prim"></li>
                        <li><a href="/insurance/">Insurance</a></li>
                        <li class="sept-prim"></li>
                        <li><a href="/forums/">Forums</a></li>
                        <li class="sept-prim"></li>
                    </ul>
            	</div>
                <div class="rightfloat search"><input id="queryText" type="text" name="search" value="Search" /><a id="doSearch" class="cw-sprite search-icon"></a></div>
                <div class="clear"></div>
            </div>
        </div>
	</div>
	<!-- primary Navbar code end here -->   
    
    <!-- Ad code start here -->
    <div class="row">
    	<div class="grid_12 column">
        	 <div id="ad-container">
			    <div class="ad-hdr"><%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 0, 90, 0, 0, true, 2) %></div>
                <div class="clear"></div>
		    </div>
        </div>
        <div class="clear"></div>
    </div>
    <!-- Ad code end here.. -->
     <div class="row">
        <div id="home-inner" class="block-space white_contener">
            <div class="content_padding">
               
         <!--Citypopup User control code -->
         <%if(System.Configuration.ConfigurationManager.AppSettings["showcitypopup"].ToString() == "true" )
        
        {%>
         <uc:GetUserCity ID="UserCity"  runat="server"/>
   
      <%}%>