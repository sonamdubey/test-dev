<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.BikeBooking.DealerPriceQuote" trace="false" Async="true" %>
<%@ Import Namespace="Bikewale.Common" %>
<%@ Import Namespace="Bikewale.BikeBooking" %>
<%
    title = "";
    keywords = "";
    description = "";
    canonical = "";
    AdPath = "/1017752/Bikewale_Mobile_PriceQuote";
    AdId = "1398766000399";
%>
<!-- #include file="/includes/headermobile_noad.aspx" -->
<link rel="stylesheet"  href="<%= !String.IsNullOrEmpty(staticUrl) ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bw-new-style.css?26june2015" />
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
<style>
    .grey-bullet li{ background:url(http://img1.carwale.com/bikewaleimg/images/bikebooking/images/bw-grey-bullet.png) no-repeat 0px 9px;display: block;list-style: square outside none;padding: 3px 0 3px 10px;}
</style>
<script type="text/javascript" src="<%= !String.IsNullOrEmpty(staticUrl) ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/BikeBooking/BikeBooking.js?29072005"></script>

<div class="padding5">
    <form id="Form1" runat="server">
    <h1 class="margin-top-10" style="margin-left:0px;"><%= objPrice.objMake.MakeName + " " + objPrice.objModel.ModelName + " " + objPrice.objVersion.VersionName %> Price Quote</h1>
    <div class="box1 box-top new-line5 bot-red new-line10">
        <%--<h1 class="margin-bottom10"><%= objPrice.objMake.MakeName + " " + objPrice.objModel.ModelName + " " + objPrice.objVersion.VersionName %> Price Quote</h1>--%>
        <div class="full-border bike-img">
            <%--<img src="<%= ImagingFunctions.GetPathToShowImages("/bikewaleimg/models/"+objPrice.LargePicUrl,objPrice.HostUrl) %>" alt="" title="" border="0" />--%>
            <img src="<%= Bikewale.Utility.Image.GetPathToShowImages(objPrice.OriginalImagePath,objPrice.HostUrl,Bikewale.Utility.ImageSize._640x348) %>" alt="" title="" border="0" />
        </div>
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
        <div class="<%= versionList.Count>1 ?"":"hide" %>">
            <asp:DropDownList id="ddlVersion" runat="server" AutoPostBAck="true"></asp:DropDownList>
        </div>            
        <!--Price Breakup starts here-->
        <div class="new-line15" style="margin-top:20px;">
            <h2 class="f-bold">On Road Price Breakup</h2>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                  
                <asp:Repeater ID="rptPriceList" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td height="30" align="left"><%# DataBinder.Eval(Container.DataItem,"CategoryName") %></td>
                            <td height="30" align="right"><%# CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"Price").ToString()) %></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                <tr align="left"><td height="1" colspan="2" class="break-line"></td></tr>
                <tr>
                    <td height="30" align="left" style="vertical-align:top; padding-top:5px;"><b>Total On Road Price</b></td>
                    <td height="30" align="right" class="f-bold" style="padding-top:5px;">
                        <div><span class="WebRupee">Rs.</span><%= CommonOpn.FormatPrice(totalPrice.ToString()) %></div>
                        <div class="margin-top-5"><a id="dealerPriceQuote" class="blue" style="color:#0056cc!important; font-weight:normal; text-decoration:none;" onclick="dataLayer.push({ event: 'product_bw_gtm', cat: 'New Bike Booking - <%=MakeModel.Replace("'","") %>', act: 'Click Link Get_Dealer_Details',lab: 'Clicked on Link Get_Dealer_Details' });">Get Dealer Details</a></div>
                    </td>
                </tr>
                <tr align="left"><td height="1" colspan="2" class="break-line">&nbsp;</td></tr>
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
        <div class="new-line10" id="divOffers"  style="background:#fff;">        
            <h2 class="f-bold">Get Absolutely Free</h2>
            <div class="margin-top5 margin-left5 new-line10">
                <asp:Repeater ID="rptOffers" runat="server">
                    <HeaderTemplate>
                            <ul class="grey-bullet">                                        
                    </HeaderTemplate>
                    <ItemTemplate>                                 
                        <li style="<%# DataBinder.Eval(Container.DataItem,"OfferCategoryId").ToString() == "3" ? "display:none;" : ""%>"><%# DataBinder.Eval(Container.DataItem,"OfferText")%> </li>
                    </ItemTemplate>
                    <FooterTemplate>                            
                            <li>Vega Helmet worth Rs. 1500</li>
                            <li>1 year of <a target="_blank" style="text-decoration:underline" href="/Absure Bike RSA.pdf">Roadside Assistance</a>.</li>
                        </ul>                        
                    </FooterTemplate>
                </asp:Repeater>
            </div>
        </div>       
    <%} %>        
           
    <!--Exciting Offers section ends here-->
        <button type="button" data-role="none" id="getDealerDetails" class="rounded-corner5" style="margin-bottom:20px;" onclick="dataLayer.push({ event: 'product_bw_gtm', cat: 'New Bike Booking - <%=MakeModel.Replace("'","") %>', act: 'Click Button Get_Dealer_Details',lab: 'Clicked on Button Get_Dealer_Details' });">Get Dealer Details</button>
    </div>
      </form>
    <%--<button data-role="none" id="getDealerDetails" class="rounded-corner5" onclick="dataLayer.push({ event: 'product_bw_gtm', cat: 'New Bike Booking - <%=MakeModel.Replace("'","") %>', act: 'Click Button Get_Dealer_Details',lab: 'Clicked on Button Get_Dealer_Details' });">Get Dealer Details & Book Now</button>--%>
</div>

<!--contact details starts here-->
<div class="bw-popup contact-details hide">
    <div class="popup-inner-container">
        <div class="bw-sprite close-btn floatright"></div>
        <h1>Provide Contact Details</h1>
        <div class="new-line10 font12">For you to see BikeWale Dealer pricing and get a printable Certificate, we need your valid contact details. We promise to keep this information confidential and not use for any other purpose.</div>
        <div class="input-div rounded-corner5">
            <div class="input-icon-div">
                <span class="bw-sprite name-icon"></span>
            </div>
            <div class="floatleft">
                <input id="txtName" name="" type="text" data-role="none" placeholder="Name" />
            </div>
            <div class="clear"></div>
        </div>
        <div class="input-div rounded-corner5">
            <div class="input-icon-div">
                <span class="bw-sprite mobile-icon"></span>
            </div>
            <div class="floatleft">
                <input name="" type="tel" data-role="none" placeholder="Mobile Number" maxlength="10" id="verify-mobile" />
            </div>
            <div class="clear"></div>
        </div>
        <div class="input-div rounded-corner5">
            <div class="input-icon-div">
                <span class="bw-sprite email-icon"></span>
            </div>
            <div class="floatleft">
                <input id="txtEmail" name="" type="email" data-role="none" placeholder="Email ID" />
            </div>
            <div class="clear"></div>
        </div>
        <div class="new-line10 lightgray font12">A verification code will be sent to the above Mobile Number. You will need the code for further Verification Process.</div>
        <button id="btnSubmit" data-role="none" class="rounded-corner5" onclick="validateDetails();dataLayer.push({ event: 'product_bw_gtm', cat: 'New Bike Booking - <%=MakeModel.Replace("'","") %>>', act: 'Click Button Get_Dealer_Details',lab:'Provided User Info' });">Submit</button>
    </div>
</div>
<!--contact details ends here-->
    
<!--Mobile Verification starts here-->
<div class="bw-popup mobile-verification hide">
    <div class="popup-inner-container">
        <div class="bw-sprite close-btn floatright"></div>
        <h1>Provide Contact Details</h1>
        <h2 class="f-bold new-line10">Mobile Verification</h2>
        <div class="new-line font12">We like to make sure that sellers get contacted by genuinely interested people like you. Kindly verify your Mobile Number.</div>
        <div class="new-line15">Enter the five digit verification code sent on</div>
        <div class="edit-mob new-line5">
            <span class="f-bold" id="mobNo">9090909090</span>
            <span class="blue-text" id="editNum">Edit</span>
        </div>
        <div class="edit-done-mob new-line5 hide">
            <span>
                <input name="" type="tel" data-role="none" placeholder="Mobile Number" maxlength="10" class="rounded-corner5" id="editedMobNo" />
            </span>
            <span class="blue-text" id="done-btn" onclick="UpdateMobile();">Done</span>
        </div>
        <div class="new-line15 font12 lightgray">Enter Verification Code</div>
        <div class="new-line10">
            <div class="floatleft verify-code rounded-corner5 selection-box">
                <input name="" type="text" data-role="none" id="txtCwi" />
            </div>
            <div class="floatleft verify-btn">
                <button data-role="none" class="rounded-corner5" id="btnVerify" onclick="VerifyCustomer();dataLayer.push({ event: 'product_bw_gtm', cat: 'New Bike Booking - <%=MakeModel.Replace("'","") %>', act: 'Click Button Get_Dealer_Details',lab:'Verified Mobile Number' });">Verify</button>
            </div>
            <div class="clear"></div>
        </div>
        <div class="blue-text new-line10 font12" id="resendCwiCode" onclick="ResendCode();">Resend Verification Code</div>
    </div>
    </div>
        <div data-role="popup" id="popupDialog" data-overlay-theme="a" data-theme="c" data-dismissible="false"  class="ui-corner-all">
        <div data-role="header" data-theme="a" class="ui-corner-top" style="background-color:#000;">
            <h3 class="ui-title" style="color:#fff;" role="heading" aria-level="1">Error !!</h3>
        </div>
        <div data-role="content" data-theme="d" class="ui-corner-bottom ui-content" style="background-color:#fff;">
            <span id="spnError" style="font-size:14px;line-height:20px;" class="error" ></span>
            <a href="#" data-role="button" data-rel="back" data-theme="c" data-mini="true">OK</a>
        </div>
    </div>


<!-- #include file="/includes/footermobile_noad.aspx" -->

