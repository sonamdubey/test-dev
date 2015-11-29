<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.PriceQuote.Quotation" Trace="false"%>
<%@ Register Src="~/m/controls/AlternativeBikes.ascx" TagPrefix="BW" TagName="AlternateBikes" %>
<%@ Register Src="~/m/controls/MUpcomingBikes.ascx" TagName="MUpcomingBikes" TagPrefix="BW" %>
<%@ Register TagPrefix="BW" TagName="MPopupWidget" Src="/m/controls/MPopupWidget.ascx" %>
<%@ Import Namespace="Bikewale.Common" %>
<!doctype html>
<html>
<head>
<%
    title = "Instant Free New Bike Price Quote";
    description = "Bikewale.com New bike free price quote.";
    AdPath = "/1017752/Bikewale_Mobile_PriceQuote";
    AdId = "1398839030772";

%>
<script>var quotationPage = true;</script>
<!-- #include file="/includes/headscript_mobile.aspx" -->
<link rel="stylesheet"  href="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/m/css/bw-new-style.css?<%= staticFileVersion %>" />
<link href="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/css/chosen.min.css?<%= staticFileVersion %>" type="text/css"rel="stylesheet" />
 <style type="text/css">
    .inner-section{background:#fff; clear:both; overflow:hidden;}
    .alternatives-carousel .jcarousel li.front { border:none;}
    .discover-bike-carousel .jcarousel li { height: auto; }
    .discover-bike-carousel .front { height:auto; }
</style>
</head>
<body class="bg-light-grey">
    <form runat="server">
    <!-- #include file="/includes/headBW_Mobile.aspx" --> 
    	<div class="box1 box-top bot-red bg-white">
            
            
            <div class="bike-img new-line10">
                <img src="<%= Bikewale.Utility.Image.GetPathToShowImages(objVersionDetails.OriginalImagePath,objVersionDetails.HostUrl,Bikewale.Utility.ImageSize._640x348) %>" alt="<%= objQuotation.MakeName + " " + objQuotation.ModelName + " " + objQuotation.VersionName%> photos" title="<%= objQuotation.MakeName + " " + objQuotation.ModelName + " " + objQuotation.VersionName%> photos" border="0" />
            </div>
            <h1 class="margin-top20 font18 padding-left10 padding-right10" style="margin-left:0px;"><%= objQuotation.MakeName + " " + objQuotation.ModelName + " " + objQuotation.VersionName%></h1>
            
                <div class="<%= versionList.Count>1 ?"":"hide" %> margin-top20">
                    <asp:DropDownList id="ddlVersion" CssClass="form-control" runat="server" AutoPostBAck="true"></asp:DropDownList>
                </div>

            <div class="new-line15 padding-left10 padding-right10" style="margin-top: 20px;">
                <%if (objQuotation != null && objQuotation.ExShowroomPrice > 0)
                  { %>
                <h2 class="font16" style="font-weight: normal">On-road price in 
                    <%= (String.IsNullOrEmpty(objQuotation.Area))?objQuotation.City:(objQuotation.Area + ", " + objQuotation.City) %>
                </h2>
                <% } %>

                <% if (objQuotation != null && objQuotation.ExShowroomPrice > 0)
                   {%>
                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="pqTable font14">
                    <tr>
                        <td class="text-medium-grey" align="left">Ex-Showroom Price</td>
                        <td class="text-grey text-bold" align="right"><span class="fa fa-rupee"></span><%= CommonOpn.FormatPrice(objQuotation.ExShowroomPrice.ToString()) %></td>
                    </tr>
                    <tr>
                        <td class="text-medium-grey" align="left">RTO</td>
                        <td class="text-grey text-bold" align="right"><span class="fa fa-rupee"></span><%= CommonOpn.FormatPrice(objQuotation.RTO.ToString()) %></td>
                    </tr>
                    <tr>
                        <td class="text-medium-grey" align="left">Insurance (<a target="_blank" onclick="dataLayer.push({ event: 'Bikewale_all', cat: 'BW_PQ', act: 'Insurance_Clicked',lab: '<%= (objQuotation!=null)?(objQuotation.MakeName + "_" + objQuotation.ModelName + "_" + objQuotation.VersionName + "_" + objQuotation.City):string.Empty %>' });" href="/insurance/" style="display:inline-block;position: relative;font-size: 11px; margin-top: 1px;">
                                Up to 60% off - PolicyBoss                                
                            </a>)<span style="margin-left: 5px; vertical-align: super; font-size: 9px;">Ad</span>
                        </td>
                        <td class="text-grey text-bold" align="right"><span class="fa fa-rupee"></span><%=CommonOpn.FormatPrice(objQuotation.Insurance.ToString()) %></td>
                    </tr>
                    <tr align="left">
                        <td height="10" colspan="2" style="padding: 0;"></td>
                    </tr>
                    <tr align="left">
                        <td height="1" colspan="2" class="break-line" style="padding: 0 0 10px;"></td>
                    </tr>
                    <tr>
                        <td class="text-grey font16" align="left">Total On Road Price</td>
                        <td class="text-grey text-bold font18" align="right" class="f-bold"><span class="fa fa-rupee"></span><%=CommonOpn.FormatPrice(objQuotation.OnRoadPrice.ToString()) %></td>
                    </tr>
                </table>
                <%}
                   else
                   {%>
                <div class="margin-top-10 padding5" style="background: #fef5e6;">Price for this bike is not available in this city.</div>
                <%} %>
            </div>
        </div>

    <section class="<%= (ctrlAlternateBikes.FetchedRecordsCount > 0) ? "" : "hide" %>">
        <div class="container margin-bottom30">
            <div class="grid-12">
                <!-- Most Popular Bikes Starts here-->
                <h2 class="margin-top30px margin-bottom20 text-center padding-top20"><%= objVersionDetails.MakeBase.MakeName + " " + objVersionDetails.ModelBase.ModelName  %> alternatives</h2>

                <div class="jcarousel-wrapper discover-bike-carousel alternatives-carousel">
                    <div class="jcarousel">
                        <BW:AlternateBikes ID="ctrlAlternateBikes" runat="server" />
                    </div>
                    <span class="jcarousel-control-left"><a href="javascript:void(0)" class="bwmsprite jcarousel-control-prev"></a></span>
                    <span class="jcarousel-control-right"><a href="javascript:void(0)" class="bwmsprite jcarousel-control-next"></a></span>
                    <p class="text-center jcarousel-pagination"></p>
                </div>

            </div>
            <div class="clear"></div>
        </div>
    </section>

         <section class="<%= (Convert.ToInt32(ctrlUpcomingBikes.FetchedRecordsCount) > 0) ? "" : "hide" %>" ><!--  Upcoming, New Launches and Top Selling code starts here -->        
    	<div class="container" >
                <div class="grid-12 ">
                    <h2 class="text-center margin-top30 margin-bottom20">Upcoming <%= objVersionDetails.MakeBase.MakeName %> bikes</h2>
                    <div class="jcarousel-wrapper upComingBikes">
                        <div class="jcarousel">
                            <ul>
                                <BW:MUpcomingBikes runat="server" ID="ctrlUpcomingBikes" />
                            </ul>
                        </div>
                        <span class="jcarousel-control-left"><a href="javascript:void(0)" class="bwmsprite jcarousel-control-prev"></a></span>
                        <span class="jcarousel-control-right"><a href="javascript:void(0)" class="bwmsprite jcarousel-control-next"></a></span>
                        <p class="text-center jcarousel-pagination"></p>
                    </div>

                </div>
                <div class="clear"></div>
            </div>
        </section>
    
 <BW:MPopupWidget runat="server" ID="MPopupWidget" />

<!-- #include file="/includes/footerBW_Mobile.aspx" -->
<!-- all other js plugins -->
<!-- #include file="/includes/footerscript_Mobile.aspx" -->
<script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/m/src/chosen-jquery-min-mobile.js?<%= staticFileVersion %>"></script>

</form>
</body>
</html>