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
<% if(canonical != "") { %> <link rel="canonical" href="<%=canonical%>" /> <% } %>

 
<!-- #include file="/includes/global/globalScript.aspx"-->
<%--<link rel="stylesheet" type="text/css" href="<%= staticUrl != "" ? "https://st.aeplcdn.com" + stagingPath : "" %>/css/cw-misk.css?20160419032055" />--%>
<link rel="stylesheet" href="/static/css/mycarwale.css" type="text/css" >
<script  language="javascript"  src="/static/src/ui-tabs-dp.js"  type="text/javascript"></script>
<!-- Script for google add -->
    <script type='text/javascript'>
        (function () {
            var useSSL = 'https:' == document.location.protocol;
            var src = (useSSL ? 'https:' : 'http:') + '//www.googletagservices.com/tag/js/gpt.js';
            document.write('<scr' + 'ipt src="' + src + '"></scr' + 'ipt>');
        })();
</script>

<script type='text/javascript'>
    googletag.defineSlot('<%= AdPath %>300x250', [300, 250], 'div-gpt-ad-<%= AdId %>-0').addService(googletag.pubads());
    googletag.defineSlot('<%= AdPath %>300x250_BTF', [300, 250], 'div-gpt-ad-<%= AdId %>-1').addService(googletag.pubads());
    googletag.defineSlot('<%= AdPath %>970x90', [[728, 90], [950, 90], [960, 90], [970, 66], [970, 90]], 'div-gpt-ad-<%= AdId %>-2').addService(googletag.pubads());
    googletag.defineSlot('<%= AdPath %>220x90', [220, 90], 'div-gpt-ad-<%= AdId %>-3').addService(googletag.pubads());
    googletag.defineSlot('<%= AdPath %>160X600', [160, 600], 'div-gpt-ad-<%= AdId %>-4').addService(googletag.pubads());
    googletag.pubads().enableSyncRendering();
    googletag.pubads().collapseEmptyDivs();
    googletag.pubads().enableSingleRequest();
    googletag.enableServices();
</script>
<!-- End of Script for google add -->
<script language="c#" runat="server">
	private int PageId = 1;
	private string Title = "", Description = "", Keywords = "", Revisit = "", DocumentState = "Static";	
	private string OEM = "", BodyType = "", Segment = "";//for keyword based google Ads
    private string canonical = ""; 
    private string AdId = "", AdPath = "";  // variables for google ad script
</script>
<!--[if IE 6]>
<script src="https://st.carwale.com/ie-png-fix.js"></script>
<script>
  DD_belatedPNG.fix('.icons_bg,.my-cw-icons, .home-icon');/* fix png transparency problem with IE6 */
</script>
<![endif]-->
</head>
<body id="ros">
<!-- Top Navbar code start here -->
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
    
    <!-- Sub Navbar code start here -->
    <div class="sub-navbar">
        <div class="row">
            <div class="column grid_12">            	
                <div id="sub-navbar" class="leftfloat">
                    <ul class="sub-navbar-list">
                        <li <%=PageId==71 ? "class='active-sub'" : "" %>><a href="/mycarwale/mycontactdetails.aspx">My Contact Details</a></li>				             
				        <li <%=PageId==72 ? "class='active-sub'" : "" %>><a href="/mycarwale/myinquiries/">My Inquiries</a></li>				          
				        <li <%=PageId==73 ? "class='active-sub'" : "" %>><a rel="nofollow" href="/users/activesessions.aspx">Active Sessions</a></li>
                    </ul>
            	</div>
                <div class="clear"></div>
            </div>
        </div>
	</div>
	<!-- Sub Navbar code end here -->
    <!-- Ad code start here -->
    <div id="row_content">
        <div class="row">
<%--            <div id="ad-container" class="grid_12 column margin-top15">
                <div class="ad-hdr"><%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 0, 90, 0, 0, true, 2) %></div>
                <div class="small-ad"><%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 220, 90, 0, 0, false, 3) %></div>
                <div class="clear"></div>
            </div>
            <div class="clear"></div>--%>
        </div>
        <!-- Ad code end here -->	
        <!-- Content code start here -->
        <div class="row">        
            <div>
                <div>	
                    
         <!--Citypopup User control code -->
         <%if(System.Configuration.ConfigurationManager.AppSettings["showcitypopup"].ToString() == "true" )
        
        {%>
         <uc:GetUserCity ID="UserCity"  runat="server"/>
   
      <%}%>	