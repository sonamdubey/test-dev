<%@ Import Namespace="Carwale.UI.Common" %>
<%@ Register TagPrefix="Vspl" TagName="LoginStatus" src="/Controls/loginStatus.ascx" %>
<%@ Register TagPrefix="uc" TagName="GetUserCity" Src="/Controls/GetUserCity.ascx"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<title><%=Title%> - CarWale</title>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <%if(PageId == 10){ %>
    <meta name="keywords" content="<%=Keywords%>"/>
    <meta name="description" content="<%=Description%>" />
    <link rel="alternate" type="text/html" media="handheld" href="https://carwale.com/m/social/" title="Mobile/PDA" />  
    <%} %>
<%else{ %>
<meta name="keywords" content="<%=Keywords%>, Car Market, Corporate world,Carwale in News" />
<meta name="description" content="<%=Description%>, CarWale dare section involves carwale's involvement in Sector world and its market Success" />
    <%} %>
<% if( fbTitle != "") { %>
<meta property="fb:page_id" content="154881297559" />
<meta property="fb:admins" content="762840819" />
<meta property="og:title" content="<%=fbTitle%>"/> 
<meta property="og:description" content="<%=Description%>"/> 
<meta property="og:type" content="product"/> 
<meta property="og:url" content="<%=canonical%>"/> 
<meta property="og:image" content="<%=fbImage%>"/> 
<meta property="og:site_name" content="CarWale"/> 
<% } %>
<% if (noIndex == true) { %> <meta name="robots" content="noindex" /> <% }
else if( canonical != "" ) { %><link rel="canonical" href="<%=canonical%>" /> <% } %>

<!-- #include file="/includes/global/globalScript.aspx"-->
<link rel="stylesheet" href="/static/css/cw-misk.css" type="text/css" >
<script language="c#" runat="server">
	private int PageId = 1;
	private string Title = "", Description = "", Keywords = "", Revisit = "", DocumentState = "Static";	
	private string OEM = "", BodyType = "", Segment = "";//for keyword based google Ads 
    private string canonical = "";
    private bool noIndex = false;
	private string fbTitle = "", fbImage = "";
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
    <!-- Ad code end here.. -->
     <div class="row">
        <div>
         <!--Citypopup User control code -->
         <%if(System.Configuration.ConfigurationManager.AppSettings["showcitypopup"].ToString() == "true" )
        
        {%>
         <uc:GetUserCity ID="UserCity"  runat="server"/>
   
      <%}%>