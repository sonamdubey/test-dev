<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.BikeBooking.DealerPriceQuote" Trace="false" Async="true" EnableEventValidation="false"%>
<%@ Register Src="~/controls/AlternativeBikes.ascx" TagName="AlternativeBikes" TagPrefix="BW" %>
<%@ Register TagPrefix="PW" TagName="PopupWidget" Src="/controls/PopupWidget.ascx" %>
<%@ Import Namespace="Bikewale.Common" %>
<%@ Import Namespace="Bikewale.BikeBooking" %>

<!doctype html>
<html>
<head>
<%
    title =  objPrice.objMake.MakeName + " " + objPrice.objModel.ModelName + " " + objPrice.objVersion.VersionName + " Price Quote ";
	description =  objPrice.objMake.MakeName + " " + objPrice.objModel.ModelName + " " + objPrice.objVersion.VersionName + " price quote";
    keywords = "";
    AdId = "1395986297721";
    AdPath = "/1017752/Bikewale_PriceQuote_";
    isAd970x90Shown = true;
%>
<!-- #include file="/includes/headscript.aspx" -->
<style type="text/css">
    #PQImageVariantContainer img { width:100%; }
    .PQDetailsTableTitle { color:#82888b; }
    .PQDetailsTableAmount, .PQOnRoadPrice { color:#4d5057; }
    .PQOffersUL { margin-left:18px; list-style:disc; }
    .PQOffersUL li { padding-bottom:15px; }
    .pqVariants .form-control-box { width:92%; }
    .form-control-box select.form-control { color:#4d5057; }

    .jcarousel-wrapper.alternatives-carousel { width: 974px; }
    .alternatives-carousel .jcarousel li {height: auto; margin-right:18px;}
    .alternatives-carousel .jcarousel li.front { border:none;}
    .alternative-section .jcarousel-control-left { left:-24px; }
    .alternative-section .jcarousel-control-right { right:-24px; }
    .alternative-section .jcarousel-control-left, .alternative-section .jcarousel-control-right { top:50%; }
    .newBikes-latest-updates-container .grid-4 { padding-left:10px;}
    .available-colors { display:inline-block; width:150px; text-align:center; margin-bottom: 20px; padding:0 5px; vertical-align: top; }
    .available-colors .color-box {width:60px; height:60px; margin:0 auto 15px; border-radius:3px; background:#f00; border:1px solid #ccc;}
</style>
<script type="text/javascript">
    var dealerId = '<%= dealerId%>';
    var pqId = '<%= pqId%>';
    var ABHostUrl = '<%= System.Configuration.ConfigurationManager.AppSettings["ApiHostUrl"]%>';
    var versionId = '<%= versionId%>';
    var cityId = '<%= cityId%>';
    var Customername = "", email = "", mobileNo = "";
    var CustomerId = '<%= CurrentUser.Id %>';
    if (CustomerId != '-1') {
        Customername = '<%= objCustomer.CustomerName%>', email = '<%= objCustomer.CustomerEmail%>', mobileNo = '<%= objCustomer.CustomerMobile%>';
    } else {
        Customername = '<%= CustomerDetailCookie.CustomerName%>', email = '<%= CustomerDetailCookie.CustomerEmail%>', mobileNo = '<%= CustomerDetailCookie.CustomerMobile %>';
    }
</script>
</head>
<body class="bg-light-grey">
    <form runat="server">
    <!-- #include file="/includes/headBW.aspx" -->
    <section class="bg-light-grey padding-top10">
        <div class="container">
            <div class="grid-12">
                <div class="breadcrumb margin-bottom15"><!-- breadcrumb code starts here -->
                    <ul>
                        <li><a href="/">Home</a></li>
                        <li><span class="fa fa-angle-right margin-right10"></span><a href="/new/">New</a></li>
                        <li><span class="fa fa-angle-right margin-right10"></span><a href="/pricequote/">On-Road Price Quote</a></li>
                        <li><span class="fa fa-angle-right margin-right10"></span>Dealer Price Quote</li>
                    </ul>
                    <div class="clear"></div>
                </div>
                <h1 class="font30 text-black margin-top10 margin-bottom10">On-road price quote</h1>
                <div class="clear"></div>
            </div>
            <div class="clear"></div>
        </div>
    </section>
    
    <section class="container">
        <div class="grid-12 margin-bottom20" id="dealerPriceQuoteContainer">
            <div class="content-box-shadow content-inner-block-20 rounded-corner2">
    	        <div class="grid-3 alpha" id="PQImageVariantContainer">
                    <% if(objPrice != null) { %>
                    <div class="pqBikeImage margin-bottom20 margin-top5">
                        <img alt="<%= objPrice.objMake.MakeName + " " + objPrice.objModel.ModelName + " " + objPrice.objVersion.VersionName %> Photos" src="<%= Bikewale.Utility.Image.GetPathToShowImages(objPrice.OriginalImagePath,objPrice.HostUrl,Bikewale.Utility.ImageSize._210x118) %>" title="<%= objPrice.objMake.MakeName + " " + objPrice.objModel.ModelName + " " + objPrice.objVersion.VersionName %> Photos" />
                    </div>
                    <% } %>
                    <div class="hide">
                        <div class="<%= objColors.Count == 0 ? "hide" : "" %>" style="float:left; margin-right:3px; padding-top:3px;">Color: </div>
                        <div style="overflow:hidden;">
                            <ul class="colours <%= objColors.Count == 0 ? "hide" : "" %>">
                                        
                                <asp:Repeater id="Repeater1" runat="server">
                                    <ItemTemplate>
                                        <li>
                                            <div title="<%#DataBinder.Eval(Container.DataItem,"ColorName") %>" style="background-color:#<%# DataBinder.Eval(Container.DataItem,"ColorCode")%>;height:15px;width:15px;margin:5px;border:1px solid #a6a9a7;"></div>
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                            <div class="clear"></div>
                        </div>
                        <div class="clear"></div>
                    </div>
                    <% if(versionList.Count > 1) { %>
                    <div class="pqVariants">
                        <p class="font14 margin-bottom5">Variants</p>
                        <div class="form-control-box">
                            <asp:DropDownList id="ddlVersion" CssClass="form-control" runat="server" AutoPostBack="true"></asp:DropDownList>
                        </div>
                    </div>
                    <% } %>
                </div>
                <div class="grid-5 padding-right20 <%= (objPrice.objOffers != null && objPrice.objOffers.Count > 0) ? "border-solid-right" : string.Empty %>" id="PQDetailsContainer">
                    <% if(objPrice != null) { %>
                        <p class="font20 text-bold margin-bottom20"><%= objPrice.objMake.MakeName + " " + objPrice.objModel.ModelName + " " +objPrice.objVersion.VersionName%></p>
                    <% } %>
                    <% if(!String.IsNullOrEmpty(cityArea)){ %>
                    <p class="font16 margin-bottom15">On-road price in <%= cityArea %></p>
                    <% } %>
                    <div runat="server">
                        <div>
                        <% if(objPrice != null) { %>
                        <table class="font14" cellspacing="0" cellpadding="0" width="100%" border="0">
                            <asp:Repeater id="rptPriceList" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td width:"245" class="PQDetailsTableTitle padding-bottom10">
                                            <%# DataBinder.Eval(Container.DataItem,"CategoryName") %> <%# Bikewale.common.DealerOfferHelper.HasFreeInsurance(dealerId.ToString(),"",DataBinder.Eval(Container.DataItem,"CategoryName").ToString(),Convert.ToUInt32(DataBinder.Eval(Container.DataItem,"Price").ToString()),ref insuranceAmount) ? "<img alt='Free_icon' src='http://imgd1.aeplcdn.com/0x0/bw/static/free_red.png' title='Free_icon'/>" : "" %>
                                        </td>
                                        <td align="right" class="PQDetailsTableAmount text-bold padding-bottom10">
                                            <span class="fa fa-rupee margin-right5"></span><span id="exShowroomPrice"><%#CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"Price").ToString()) %></span>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                            <tr><td colspan="2"><div class="border-solid-top padding-bottom10"></div><td></tr>                                 
                         <%
                            if (IsInsuranceFree)
                            {
                                %>
                                    <tr>
                                        <td class="PQDetailsTableTitle padding-bottom10">Total on road price</td>
                                        <td align="right" class="PQDetailsTableAmount text-bold padding-bottom10">
                                            <span class="fa fa-rupee"></span><span style="text-decoration:line-through;"><%= CommonOpn.FormatPrice(totalPrice.ToString()) %></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="PQDetailsTableTitle padding-bottom10">Minus insurance</td>
                                        <td align="right" class="PQDetailsTableAmount text-bold padding-bottom10">
                            	            <span class="fa fa-rupee"></span><span><%= CommonOpn.FormatPrice(insuranceAmount.ToString()) %></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="middle" class="PQDetailsTableTitle font18 text-bold PQOnRoadPrice">Total on road price</td>
                                        <td align="right" class="PQDetailsTableAmount font20 text-bold">
                            	            <span class="fa fa-rupee"></span><span><%= CommonOpn.FormatPrice((totalPrice - insuranceAmount).ToString()) %></span>
                                        </td>
                                    </tr>
                                    <%
                            }
                            else
                            {
                                %>
                                <tr>
                                    <td class="PQDetailsTableTitle font18 text-bold PQOnRoadPrice padding-bottom10">Total on road price</td>
                                    <td align="right" class="PQDetailsTableAmount padding-bottom10 font20 text-bold">
                                        <span class="fa fa-rupee margin-right5"></span><span><%= CommonOpn.FormatPrice(totalPrice.ToString()) %></span>
                                    </td>
                                </tr>

                        <% } %>     
                        <% if(!(objPrice.objOffers != null && objPrice.objOffers.Count > 0)) { %>
                            <tr>
                                <td colspan="2" class="border-solid-top" align="right"><a class="margin-top15 btn btn-orange" id="btnBikeBooking" name="btnSavePriceQuote" onclick="dataLayer.push({ event: 'Bikewale_all', cat: 'New Bike Booking - <%=BikeName.Replace("'","")%>', act: 'Click Button Book Now',lab: 'Clicked on Button Get_Dealer_Details' });">Book Now</a></td>
                            </tr>
                        <% } %>
                            <tr class="hide">
                                <td colspan="3">
                                    <ul class="std-ul-list">
                                        <asp:Repeater id="rptDisclaimer" runat="server">
                                            <ItemTemplate>
                                                <li><i><%# Container.DataItem %></i></li>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </ul>
                                </td>
                            </tr>	
			            </table>
                        <% } else { %>
                        <div class="grey-bg border-light padding5 margin-top10 margin-bottom20">
                            <h3>Dealer Prices for this Version is not available.</h3>
                        </div>
                        <% } %>
                    </div>
                    
                    </div>

                    <div id="div_ShowErrorMsg" runat="server" class="grey-bg border-light content-block text-highlight margin-top15"></div>
                    </div>
                <div class="grid-4 omega padding-left20" id="PQOffersContainer">
                    <!--Exciting offers div starts here-->
                    <% if (objPrice.objOffers != null && objPrice.objOffers.Count > 0)
                    { %>
                    <div id="divOffers">                    
                        <p class="font20 text-bold margin-bottom10 border-solid-bottom padding-bottom5"><%= IsInsuranceFree ? "BikeWale Offer" : "Available Offer"%></p>
                        <div>
                            <asp:Repeater ID="rptOffers" runat="server">
                                <HeaderTemplate>
                                    <ul class="font14 text-light-grey PQOffersUL">                                        
                                </HeaderTemplate>
                                <ItemTemplate>                                        
                                    <li><%# DataBinder.Eval(Container.DataItem,"OfferText")%></li>
                                </ItemTemplate>                                                            
                                <FooterTemplate>                                                                        
                                    </ul>
                                </FooterTemplate>                              
                            </asp:Repeater>
                            <div class="margin-top10">
                                <a class="btn btn-orange" id="btnGetDealerDetails" name="btnSavePriceQuote" onclick="dataLayer.push({ event: 'Bikewale_all', cat: 'New Bike Booking - <%=BikeName.Replace("'","")%>', act: 'Click Button Get_Dealer_Details',lab: 'Clicked on Button Get_Dealer_Details' });">Avail offer</a>
                            </div>
                        </div>
                    </div>                   
                    <%}%>
                    
                    <!--Exciting offers div ends here-->
                        
                    <%--<SB:SimilarBike ID="ctrl_similarBikes" TopCount="2" runat="server" Visible="false"/>--%>
                </div>
                <div class="clear"></div>
            </div>
        </div>
        <div class="clear"></div>
    </section>

    <section class="margin-bottom30 <%= (ctrlAlternativeBikes.FetchedRecordsCount > 0) ? string.Empty : "hide" %>">
        <div class="container">
        <div class="grid-12 alternative-section" id="alternative-bikes-section">
            <h2 class="text-bold text-center margin-top20 margin-bottom30"><%= BikeName %> alternatives</h2>
            <div class="content-box-shadow">
                <div class="jcarousel-wrapper alternatives-carousel margin-top20">
                    <div class="jcarousel">
                        <ul>
                            <BW:AlternativeBikes ID="ctrlAlternativeBikes" runat="server" />
                        </ul>
                    </div>
                    <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev"></a></span>
                    <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next"></a></span>
                </div>
            </div>
        </div>
        <div class="clear"></div>
        </div>
    </section>

<PW:PopupWidget runat="server" ID="PopupWidget" />   
<!-- #include file="/includes/footerBW.aspx" -->
<!-- #include file="/includes/footerscript.aspx" -->
<script type="text/javascript">
$('#btnGetDealerDetails, #btnBikeBooking').click(function () {
    window.location.href = '/pricequote/bookingsummary_new.aspx';
});
</script> 
</form>
</body>
</html>
