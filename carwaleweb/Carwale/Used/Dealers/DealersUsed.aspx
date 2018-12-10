<%@ Page Language="C#" Trace="false" Debug="false" Inherits="Carwale.UI.Used.Dealers.DealersUsedNew" %>
<%@ Import Namespace="Carwale.UI.Common" %>
<%@ Import Namespace="Carwale.Utility" %>
<%
	// Define all the necessary meta-tags info here.
	// To know what are the available parameters,
	// check page, headerCommon.aspx in common folder.
	
	PageId 			= 81;
	Title 			= "Used Car Dealers in " + cityName + "- CarWale";
	Description 	= " Used car dealers in " + cityName + ". Find second hand car dealers in "+cityName+" along with address & contact details. View stock of dealers selling old cars in "+cityName;
	Keywords		= " used car dealers, used car showrooms";
	Revisit 		= "15";
	DocumentState 	= "Static";
    canonical       = "https://www.carwale.com/used/dealers-in-" + Format.FormatURL(cityName);
    AdId            = "1396440544094";
    AdPath          = "/1017752/UsedCar_";
    //Ad300           = true;
    targetKey       = "City";
    targetValue     = cityName.Trim();
%>
<!doctype html>
<html itemscope itemtype="http://schema.org/WebPage">
<head>
<!-- #include file="/includes/global/head-script.aspx" -->
<style type="text/css">
    .slot-cont li {list-style-type: none;background: transparent url(https://imgd.aeplcdn.com/0x0/cw-common/ul-arrow.gif) no-repeat 2px;border: 0;margin: 0;padding: 2px 0 5px 9px;}
    .hidephoneicon {
        display:none;
    }
</style>
<script type='text/javascript'>
    googletag.cmd.push(function () {
        googletag.defineSlot('<%= AdPath %>300x250', [300, 250], 'div-gpt-ad-<%= AdId %>-0').addService(googletag.pubads());
        googletag.defineSlot('<%= AdPath %>970x90', [[220, 90], [728, 90], [950, 90], [960, 90], [970, 66], [970, 90]], 'div-gpt-ad-<%= AdId %>-2').addService(googletag.pubads());

        //googletag.pubads().enableSyncRendering();
        googletag.pubads().setTargeting('<%=targetKey%>', '<%=targetValue%>');
        googletag.pubads().collapseEmptyDivs();
        googletag.pubads().enableSingleRequest();
        googletag.enableServices();
    });
</script>
</head>
<body class="header-fixed-inner">
    <!-- #include file="/includes/header.aspx" -->
    <section class="container">
            <div class="grid-12">
                <div class="padding-bottom15 text-center">
                    <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 0, 90, 0, 0, true, 2) %>
                </div>
            </div>
        </section>
    <section>
        <div class="container margin-top70">
            <div class="grid-12">
                <div id="youHere" class="margin-top10 margin-bottom10">
                    <a class=" margin-right10" href="/">Home</a><span class="fa fa-angle-right margin-right10"></span>
                    <a class=" margin-right10" href="/used/">Used Cars</a><span class="fa fa-angle-right margin-right10"></span>
                    <strong>Used Car Dealers in <%= cityName %></strong>
                </div>
            <h1 class="font30 text-black special-skin-text">Used Car Dealers in <%= cityName %></h1>
            <div class="border-solid-bottom margin-top10 margin-bottom15"></div>
        </div>
        </div>
    </section>
    <div class="clear"></div>
    
    <section  class="padding-bottom20">
        <div class="container">
            <div class="grid-8">
                <div class="left_grid content-box-shadow content-inner-block-10 rounded-corner2 bg-white">
                    <div id="content">
        
                        <table>
                            <tr>
                                <td>
                                    <div class="h1Font">
                                        <a name='<%= cityName%>'></a>
                                        <h2>Used Car Dealer Showrooms in <%= cityName%></h2>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <div class="info">
                                        <asp:datalist id="dlShowroom" runat="server" width="100%" itemstyle-verticalalign="top"
                                            repeatcolumns="3" repeatdirection="Horizontal" itemstyle-horizontalalign="left">
								                <itemtemplate>
									                <table align="left" border="0" cellpadding="5" cellspacing="0" height="130">
										                <tr>
											                <td valign="top" height="90" align="center">
												                <%--<a title="Click here to View Showroom" href="/used/dealers-in-<%= UrlRewrite.FormatSpecial(cityName) %>/<%# UrlRewrite.FormatSpecial(DataBinder.Eval(Container.DataItem, "Organization").ToString())%>-<%# UrlRewrite.FormatSpecial(DataBinder.Eval(Container.DataItem, "DealerId").ToString())%>/"><%# GetThumbNail(DataBinder.Eval(Container.DataItem, "ShowroomImg").ToString(), DataBinder.Eval(Container.DataItem, "dealerId").ToString(), DataBinder.Eval(Container.DataItem, "HostUrl").ToString())%><br></a>--%>
											                    <a title="Click here to View Showroom" href="/used/dealers-in-<%= UrlRewrite.FormatSpecial(cityName) %>/<%# UrlRewrite.FormatSpecial(DataBinder.Eval(Container.DataItem, "Organization").ToString())%>-<%# UrlRewrite.FormatSpecial(DataBinder.Eval(Container.DataItem, "DealerId").ToString())%>/"><%# GetThumbNail(DataBinder.Eval(Container.DataItem, "ShowroomImg").ToString(), DataBinder.Eval(Container.DataItem, "dealerId").ToString(), DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),DataBinder.Eval(Container.DataItem, "OriginalImgPath").ToString())%><br></a>
                                                            </td>
										                </tr>	
										                <tr>
											                <td valign="top" width="180" align="center">
												                <a style="font-size:12px; font-weight:bold; color:#555555;" href="/used/dealers-in-<%= UrlRewrite.FormatSpecial(cityName) %>/<%# UrlRewrite.FormatSpecial(DataBinder.Eval(Container.DataItem, "Organization").ToString())%>-<%# UrlRewrite.FormatSpecial(DataBinder.Eval(Container.DataItem, "DealerId").ToString())%>/">
													                <%# DataBinder.Eval(Container.DataItem, "Organization")%>
												                </a><br>
												              <%--  Showroom viewed <%# DataBinder.Eval(Container.DataItem, "TotalView")%> times--%>
                                                                   Views: <%# Carwale.Utility.Format.Numeric(DataBinder.Eval(Container.DataItem, "TotalView").ToString())%>  <br>
                                                              <a href="<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"IsWebSiteAvailable")) && DataBinder.Eval(Container.DataItem,"WebsiteUrl").ToString().IndexOf("http", StringComparison.OrdinalIgnoreCase) == 0 ? DataBinder.Eval(Container.DataItem,"WebsiteUrl") :"http://" + DataBinder.Eval(Container.DataItem,"WebsiteUrl") %>" class="<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"IsWebSiteAvailable")) ? "":"hide" %>" target="_blank">Visit Website</a>
                                                            </td>
										                </tr>
									                </table>
								                </itemtemplate>	
							                </asp:datalist>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="info">
                                    <div class="margin-top5">
                                        <a href="/used/cars-in-<%= UrlRewrite.FormatSpecial(cityName) %>/" style="color: #023469;">View all Used Cars in <%= cityName%></a>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
            <div class="grid-4">
                <div>
                    <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 300, 250, 20, 20, false, 0) %>
                </div>                    
                    <!--Popular Cars in India Start-->
                        <div class="content-box-shadow content-inner-block-5 rounded-corner2">
                            <h3 class="margin-bottom5">Popular Cars in <%= cityName %></h3>
                            <div>
                                <div class="slot-cont">
                                   <ul>
                                        <li><a title='Hyundai i10 in <%= UrlRewrite.FormatSpecial(cityName) %>' href='/used/hyundai-i10-cars-in-<%= UrlRewrite.FormatSpecial(cityName) %>/'>Hyundai i10</a></li>
                                        <li><a title='Hyundai Santro Xing in <%= UrlRewrite.FormatSpecial(cityName) %>' href='/used/hyundai-santroxing-cars-in-<%= UrlRewrite.FormatSpecial(cityName) %>/'>Hyundai Santro Xing</a></li>
                                        <li><a title='Maruti Suzuki Alto [2000-2012] in <%= UrlRewrite.FormatSpecial(cityName) %>' href='/used/marutisuzuki-alto20002012-cars-in-<%= UrlRewrite.FormatSpecial(cityName) %>/'>Maruti Suzuki Alto [2000-2012]</a></li>
                                        <li><a title='Maruti Suzuki Swift in <%= UrlRewrite.FormatSpecial(cityName) %>' href='/used/marutisuzuki-swift-cars-in-<%= UrlRewrite.FormatSpecial(cityName) %>/'>Maruti Suzuki Swift</a></li>
                                        <li><a title='Toyota Innova in <%= UrlRewrite.FormatSpecial(cityName) %>' href='/used/toyota-innova-cars-in-<%= UrlRewrite.FormatSpecial(cityName) %>/'>Toyota Innova</a></li>
                                        <li><a title='Hyundai Verna in <%= UrlRewrite.FormatSpecial(cityName) %>' href='/used/hyundai-verna-cars-in-<%= UrlRewrite.FormatSpecial(cityName) %>/'>Hyundai Verna</a></li>
                                        <li><a title='Maruti Suzuki 800 in <%= UrlRewrite.FormatSpecial(cityName) %>' href='/used/marutisuzuki-800-cars-in-<%= UrlRewrite.FormatSpecial(cityName) %>/'>Maruti Suzuki 800</a></li>
                                        <li><a title='Hyundai i20 in <%= UrlRewrite.FormatSpecial(cityName) %>' href='/used/hyundai-i20-cars-in-<%= UrlRewrite.FormatSpecial(cityName) %>/'>Hyundai i20</a></li>
                                        <li><a title='Maruti Suzuki Swift [2005-2011] in <%= UrlRewrite.FormatSpecial(cityName) %>' href='/used/marutisuzuki-swift20052011-cars-in-<%= UrlRewrite.FormatSpecial(cityName) %>/'>Maruti Suzuki Swift [2005-2011]</a></li>
                                        <li><a title='Maruti Suzuki Wagon R [1999-2010] in <%= UrlRewrite.FormatSpecial(cityName) %>' href='/used/marutisuzuki-wagonr19992010-cars-in-<%= UrlRewrite.FormatSpecial(cityName) %>/'>Maruti Suzuki Wagon R [1999-2010]</a></li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                        <!--Popular Cars in India End-->
                </div>
            </div>
            <div class="clear"></div>
        </div>
    </section>
    <div class="clear"></div>
                
                
            
    <script type="text/javascript">
        $(document).ready(function () {
            $('img.lazy').lazyload();
        });
    </script>
    <!-- #include file="/includes/footer.aspx" -->
    <!-- #include file="/includes/global/footer-script.aspx" -->
</body>
</html>