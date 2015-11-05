<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.BikeBooking.DealerPriceQuote" trace="false" Async="true" %>
<%@ Register Src="~/m/controls/AlternativeBikes.ascx" TagPrefix="BW" TagName="AlternateBikes" %>
<%@ Import Namespace="Bikewale.Common" %>
<%@ Import Namespace="Bikewale.BikeBooking" %>
<!doctype html>
<html>
<head>
<%
    title = "";
    keywords = "";
    description = "";
    canonical = "";
    AdPath = "/1017752/Bikewale_Mobile_PriceQuote";
    AdId = "1398766000399";
%>
<script>var quotationPage = true;</script>
<!-- #include file="/includes/headscript_mobile.aspx" -->
<link rel="stylesheet"  href="/m/css/bw-new-style.css?<%= staticFileVersion %>" />
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
    <div class="bg-white box1 box-top new-line5 bot-red new-line10">
       
        <div class="bike-img">
            <img src="<%= Bikewale.Utility.Image.GetPathToShowImages(objPrice.OriginalImagePath,objPrice.HostUrl,Bikewale.Utility.ImageSize._640x348) %>" alt="" title="" border="0" />
        </div>
        <h1 class="margin-top20 font18 padding-left10 padding-right10" style="margin-left:0px;"><%= objPrice.objMake.MakeName + " " + objPrice.objModel.ModelName + " " + objPrice.objVersion.VersionName %> Price Quote</h1>
        <div class="<%= objColors.Count == 0 ?"hide":"hide" %>">
            <div class="full-border new-line10 selection-box"><b>Color Options: </b>
                <table width="100%">
                    <tr style="margin-bottom:5px;">
                        <td class="break-line" colspan="2"></td>
                    </tr>
                <asp:Repeater id="rptColors" runat="server">
                    <ItemTemplate>
                        <tr>
                        <td  style="width:30px;"><div style="width:30px;height:20px;margin:0px 10px 0px 0px;border: 1px solid #a6a9a7;padding-top:5px;background-color:#<%# DataBinder.Eval(Container.DataItem,"ColorCode")%>"></div></td>
                        <td><div class="new-line"><%# DataBinder.Eval(Container.DataItem,"ColorName") %></div></td>
                            </tr>
                    </ItemTemplate>
                </asp:Repeater>
                </table>
            </div>
        </div>
        <div class="<%= versionList.Count>1 ?"":"hide" %> margin-top20">
            <asp:DropDownList id="ddlVersion" CssClass="form-control" runat="server" AutoPostBAck="true"></asp:DropDownList>
        </div>            
        <!--Price Breakup starts here-->
        <div class="new-line15 padding-left10 padding-right10" style="margin-top:20px;">
            
            <% if(!String.IsNullOrEmpty(cityArea)){ %>
            <h2 class="font16" style="font-weight:normal">On-road price in <%= cityArea %></h2>
                    <% } %>
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="pqTable font14">
                  
                <asp:Repeater ID="rptPriceList" runat="server">
                    <ItemTemplate>
                        <%-- Start 102155010 --%>
                       
                        <tr>
                            <td align="left" class="text-medium-grey"><%# DataBinder.Eval(Container.DataItem,"CategoryName") %> <%# Bikewale.common.DealerOfferHelper.HasFreeInsurance(dealerId.ToString(),"",DataBinder.Eval(Container.DataItem,"CategoryName").ToString(),Convert.ToUInt32(DataBinder.Eval(Container.DataItem,"Price").ToString()),ref insuranceAmount) ? "<img alt='Free_icon' src='http://imgd1.aeplcdn.com/0x0/bw/static/free_red.png' title='Free_icon'/>" : "" %></td>
                            <td align="right" class="text-grey text-bold"><span class="fa fa-rupee"></span> <%# CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"Price").ToString()) %></td>
                        </tr>
                        <%-- End 102155010 --%>
                    </ItemTemplate>
                </asp:Repeater>
                <tr align="left"><td height="10" colspan="2" style="padding:0;"></td></tr>
                <tr align="left"><td height="1" colspan="2" class="break-line" style="padding:0 0 10px;"></td></tr>
                <%-- Start 102155010 --%>
                            
                <%
                    if (IsInsuranceFree)
                    {
                        %>
                <tr>
                    <td align="left" class="text-medium-grey">Total On Road Price</td>
                    <td align="right" class="text-grey text-bold">
                        <div><span class="fa fa-rupee"></span> <span style="text-decoration: line-through"><%= CommonOpn.FormatPrice(totalPrice.ToString()) %></span></div>                        
                    </td>
                </tr>
                <tr>
                    <td align="left" class="text-medium-grey">Minus Insurance</td>
                    <td align="right" class="text-grey text-bold">
                        <div><span class="fa fa-rupee"></span> <%= CommonOpn.FormatPrice(insuranceAmount.ToString()) %></div>                        
                    </td>
                </tr>
                <tr>
                    <td align="left" class="text-medium-grey">BikeWale On Road (after insurance offer)</td>
                    <td align="right" class="text-grey text-bold">
                        <div><span class="fa fa-rupee"></span> <%= CommonOpn.FormatPrice((totalPrice - insuranceAmount).ToString()) %></div>                        
                     
                    </td>
                </tr>
                <%
                    }
                    else
                    {%>
                        <tr>
                    <td align="left" class="text-grey font16">Total On Road Price</td>
                    <td align="right" class="text-grey text-bold font18">
                        <div><span class="fa fa-rupee"></span> <%= CommonOpn.FormatPrice(totalPrice.ToString()) %></div>
                      
                    </td>
                </tr>
        <%
                    }
                     %>
                <%-- End 102155010 --%>
                <tr align="left"><td height="20" colspan="2" style="padding:0;"></td></tr>
                <tr align="left"><td height="1" colspan="2" class="break-line-light" style="padding:0;">&nbsp;</td></tr>
            </table>            
            <ul class="grey-bullet hide">
                <asp:Repeater id="rptDisclaimer" runat="server">
                    <ItemTemplate>
                        <li><i><%# Container.DataItem %></i></li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
        </div>
        <!--Price Breakup ends here-->
        <!--Exciting Offers section starts here-->                   
        <% if (objPrice.objOffers != null && objPrice.objOffers.Count > 0)
        { %>        
        <div class="new-line10 padding-left10 padding-right10 margin-bottom15" id="divOffers"  style="background:#fff;">        
            <h2 class="font24 text-center text-grey"><%= IsInsuranceFree ? "BikeWale Offer" : "Get Absolutely Free"%></h2>
            <div class="new-line10">
                <asp:Repeater ID="rptOffers" runat="server">
                    <HeaderTemplate>
                            <ul class="grey-bullet">                                        
                    </HeaderTemplate>
                    <ItemTemplate>            
                      
                        <li><%# DataBinder.Eval(Container.DataItem,"OfferText")%></li>
                      
                    </ItemTemplate>
                    <FooterTemplate>             
                      
                        </ul>                        
                    </FooterTemplate>
                </asp:Repeater>
            </div>
        </div>  
        <div class="padding-left10 padding-right10">
        <button type="button" data-role="none" id="getDealerDetails" class="btn btn-full-width btn-orange" style="margin-bottom:20px;" onclick="dataLayer.push({ event: 'Bikewale_all', cat: 'New Bike Booking - <%=MakeModel.Replace("'","") %>', act: 'Click Button Get_Dealer_Details',lab: 'Clicked on Button Get_Dealer_Details' });">Avail offer</button>     
    <%}else { %>        
           <button type="button" data-role="none" id="btnBookBike" class="btn btn-full-width btn-orange" style="margin-bottom:20px;" onclick="dataLayer.push({ event: 'Bikewale_all', cat: 'New Bike Booking - <%=MakeModel.Replace("'","") %>', act: 'Click Button Book Now',lab: 'Clicked on Button Get_Dealer_Details' });">Book Now</button>
        <% } %>
            </div>
    <!--Exciting Offers section ends here-->
        
    </div>


    <section class="<%= (ctrlAlternateBikes.FetchedRecordsCount > 0) ? "" : "hide" %>">
        <div class="container margin-bottom30">
            <div class="grid-12">
                <!-- Most Popular Bikes Starts here-->
                <h2 class="margin-top30px margin-bottom20 text-center padding-top20"><%= objPrice.objMake.MakeName + " " + objPrice.objModel.ModelName  %> alternatives</h2>

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
    

       
<script type="text/javascript">
    $('#getDealerDetails,#btnBookBike').click(function(){
        window.location.href='/m/pricequote/bookingsummary_new.aspx';
    });
</script>

<!-- #include file="/includes/footerBW_Mobile.aspx" -->
<!-- all other js plugins -->
<!-- #include file="/includes/footerscript_Mobile.aspx" -->
</form>
</body>
</html>