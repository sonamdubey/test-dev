<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.BikeBooking.DealerPriceQuote" Trace="false" Async="true" EnableEventValidation="false"%>
<%@ Register TagPrefix="SB" TagName="SimilarBike" Src="~/controls/SimilarBikesHorizontal.ascx" %>
<%@ Import Namespace="Bikewale.Common" %>
<%@ Import Namespace="Bikewale.BikeBooking" %>
<%
    title =  objPrice.objMake.MakeName + " " + objPrice.objModel.ModelName + " " + objPrice.objVersion.VersionName + " Price Quote ";
	description =  objPrice.objMake.MakeName + " " + objPrice.objModel.ModelName + " " + objPrice.objVersion.VersionName + " price quote";
    keywords = "";
    AdId = "1395986297721";
    AdPath = "/1017752/Bikewale_PriceQuote_";
    //canonical = "http://www.bikewale.com/pricequote/";
    //alternate = "http://www.bikewale.com/m/pricequote/";
%>
<!-- #include file="/includes/headNew.aspx" -->
<%--<link rel="stylesheet"  href="<%= !String.IsNullOrEmpty(staticUrl) ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/bw-pq.css" />--%>
<link rel="stylesheet" href="<%= !String.IsNullOrEmpty(staticUrl) ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/bw-pq.css?30july2015" />
<link rel="stylesheet" href="<%= !String.IsNullOrEmpty(staticUrl) ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/bw-pq-new.css?23july2015" />
<link rel="stylesheet" type="text/css" href="/css/rsa.css?v=3.0"/>
<style>
    .minLength { width:95px !important;}
    .colours { list-style : outside none none; display:inline;}
    .colours li {float :left;}
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
<script type="text/javascript"  src="<%= !String.IsNullOrEmpty(staticUrl) ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/BikeBooking/BikeBooking.js?23july2015"></script>

<%@ Register TagPrefix="PW" TagName="PopupWidget" Src="/controls/PopupWidget.ascx" %>
<PW:PopupWidget runat="server" ID="PopupWidget" />

<!--bw contact popup code starts here-->
<div class="bw-contact-popup hide">
	<div class="popup-inner-container">
    	<div class="bw-sprite close-btn right-float"></div>
    	<h2>Provide Contact Details</h2>
        <div class="popup-inner-container">
    		<p class="margin-bottom20">For you to see BikeWale Dealer pricing and get a printable Certificate, we need your valid contact details. We promise to keep this information confidential and not use for any other purpose.</p>
            <span class="error" id="spnName"></span>
            <div class="input-div">
                <div class="input-icon-div">
                    <span class="bw-sprite user"></span>
                </div>
                <div class="left-float">
                    <input id="txtName" type="text" placeholder="Name" name="">
                </div>
                <div class="clear"></div>
            </div>
            <span class="error" id="spnMobile"></span>
            <div class="input-div">
                <div class="input-icon-div">
                    <span class="bw-sprite call"></span>
                </div>
                <div class="left-float">
                    <input id="txtMobile" type="text" placeholder="Mobile Number" name="" maxlength="10">
                </div>
                <div class="clear"></div>
            </div>
            <span class="error" id="spnEmail"></span>
            <div class="input-div">
                <div class="input-icon-div">
                    <span class="bw-sprite email"></span>
                </div>
                <div class="left-float">
                    <input id="txtEmail" type="text" placeholder="Email" name="">
                </div>
                <div class="clear"></div>
            </div>            
            <p>A verification code will be sent to the above Mobile Number. You will need the code for further Verification Process.</p>
        </div>
        <div class="mid-box margin-top15 center-align">
        	<input type="submit" name="btnNext" value="Next" id="btnNext" class="action-btn" onclick="dataLayer.push({ event: 'product_bw_gtm', cat: 'New Bike Booking - <%=BikeName.Replace("'","")%>        ', act: 'Click Button Get_Dealer_Details', lab: 'Provided User Info' });">
        </div>
    </div>
</div>
<!--bw popup code ends here-->
<!--bw verification popup code starts here-->
<div class="verify-popup bw-popup hide">
	<div class="popup-inner-container">
    <div class="bw-sprite close-btn right-float"></div>
    	<h2>Mobile Verification</h2>
        <div class="popup-inner-container">
        	<p class="margin-bottom20">We like to make sure that sellers get contacted by genuinely interested people like you. Kindly verify your Mobile Number</p>
            <p class="margin-bottom15">Enter the five digit verification code sent on 
                <span class="edit-mob new-line5 margin-bottom10">
                    <strong class="f-bold" id="mobNo"></strong>
                    <span class="blue" id="editNum">Edit</span>
                </span>
                <span class="edit-done-mob new-line5 margin-bottom10">
                    <input type="text" placeholder="Mobile Number" class="rounded-corner5" id="editedMobNo" maxlength="10" />
                    <span class="blue" id="done-btn">Done</span><br />
                    <span class="error" id="spnEditNo"></span>
                </span>
            </p>
            <p>
                <span class="margin-right10">Enter Verification Code:</span> 
                <input type="text" id="txtCwi" class="minLength"/>
                <a class="margin-left10 blue" id="resendCwiCode"> Resend Code</a><br />
                <span class="error" id="spnCwi"></span>
            </p>
        </div>
        <div class="mid-box margin-top15 center-align">
        	<input type="submit" name="btnSavePriceQuote" value="Verify" id="btnSavePriceQuote" class="action-btn" onclick="dataLayer.push({ event: 'product_bw_gtm', cat: 'New Bike Booking - <%=BikeName.Replace("'","")%>        ', act: 'Click Button Get_Dealer_Details', lab: 'Verified Mobile Number' });">
       </div>
    </div>
</div>
<!--bw verification popup ends starts here-->
<div class="main-container">
	<div class="container_12">
        <div class="grid_12">
            <ul class="breadcrumb">
                <li>You are here: </li>
                <li><a class="blue" href="/">Home</a></li>
                <li class="fwd-arrow">&rsaquo;</li>
                <li><a class="blue" href="/new/">New</a></li>
                <li class="fwd-arrow">&rsaquo;</li>
                <li><a class="blue" href="/pricequote/">On-Road Price Quote</a></li>
                <li class="fwd-arrow">&rsaquo;</li>
                <li class="current"><strong>Dealer Price Quote</strong></li>
            </ul><div class="clear"></div>
        </div>
        <div class="grid_12 margin-top10">
            <% if(objPrice != null) { %><h1 class="margin-bottom5">Dealer Price Quote - <%= objPrice.objMake.MakeName + " " + objPrice.objModel.ModelName + " " +objPrice.objVersion.VersionName%></h1><% } %>
            <div class="clear"></div>
        </div>
        <div class="grid_12 margin-top10" style="border:1px solid #eaeaea; padding-bottom:10px;">
    	<div class="grid_8 margin-top5 alpha">
            <div class="padding5" style="border-right:1px solid #eaeaea;">
            <div  id="div_GetPQ" runat="server">
            <div id="get-pq-new">
                <%--<h2 class="border-red"></h2>--%>
            	<div id="div_ShowPQ">
                <% if(objPrice != null) { %>
                <table cellspacing="0" cellpadding="0" width="100%" border="0" class="tbl-default margin-top10">
			        <tr>
                        <td style="width:100px;vertical-align:top;">
                             <div class="show-pq-pic">
                             	<img alt="<%= objPrice.objMake.MakeName + " " + objPrice.objModel.ModelName + " " + objPrice.objVersion.VersionName %> Photos" src="<%= Bikewale.Utility.Image.GetPathToShowImages(objPrice.OriginalImagePath,objPrice.HostUrl,Bikewale.Utility.ImageSize._210x118) %>" title="<%= objPrice.objMake.MakeName + " " + objPrice.objModel.ModelName + " " + objPrice.objVersion.VersionName %> Photos" />
                                 <%--<div class="margin-top5 dotted-hr margin-bottom5" style="margin-right:10px;"></div>--%>
                                <div class="hide">
                                    <div class="<%= objColors.Count == 0 ? "hide" : "" %>" style="float:left; margin-right:3px; padding-top:3px;">Color: </div>
                                    <div style="overflow:hidden;">
                                        <ul class="colours <%= objColors.Count == 0 ? "hide" : "" %>">
                                        
                                            <asp:Repeater id="rptColors" runat="server">
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
                                <% if(versionList.Count > 1) { %><div class="margin-top15" style="text-align:center;"><asp:DropDownList id="ddlVersion" runat="server" AutoPostBack="true"></asp:DropDownList></div><% } %>
                        	 </div>
                        </td>
                        <td valign="top" style="padding-left:20px;">
                            <table>
                                <%--<a href="<%="/" + objPrice.objMake.MaskingName + "-bikes/" + objPrice.objModel.MaskingName + "/" %>"><h2><%= objPrice.objMake.MakeName + " " + objPrice.objModel.ModelName + " " + objPrice.objVersion.VersionName %></h2></a>--%>
                                <asp:Repeater id="rptPriceList" runat="server">
                                    <ItemTemplate>
                                        <%-- Start 102155010 --%>
                                        <%--<tr class="font14">
                                            <td width="370">
                                                 <%# DataBinder.Eval(Container.DataItem,"CategoryName") %>
                                            </td>
                                            <td width="100" class="numeri-cell" align="right"><span class="WebRupee">Rs.</span><span id="exShowroomPrice"><%#CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"Price").ToString()) %></span></td>
                                        </tr>--%>
                                        <tr class="font14">
                                            <td width="370">
                                                 <%# DataBinder.Eval(Container.DataItem,"CategoryName") %> <%# Bikewale.common.DealerOfferHelper.HasFreeInsurance(dealerId.ToString(),"",DataBinder.Eval(Container.DataItem,"CategoryName").ToString(),Convert.ToUInt32(DataBinder.Eval(Container.DataItem,"Price").ToString()),ref insuranceAmount) ? "<img alt='Free_icon' src='http://imgd1.aeplcdn.com/0x0/bw/static/free_red.png' title='Free_icon'/>" : "" %>
                                            </td>
                                            <td width="100" class="numeri-cell" align="right"><span class="WebRupee">Rs.</span><span id="exShowroomPrice"><%#CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"Price").ToString()) %></span></td>
                                        </tr>
                                        <%-- End 102155010 --%>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <tr><td colspan="2"><div class="dotted-hr"></div><td></tr>
                                <%-- Start 102155010 --%>
                                <%--<tr>
                                    <td class="price2 font14">Total On Road Price</td>
                                    <td width="100" class="numeri-cell font14" align="right"><span class="WebRupee">Rs.</span><b><%= CommonOpn.FormatPrice(totalPrice.ToString()) %></b></td>
                                </tr>--%>                                
                                <%
                       if (IsInsuranceFree)
                       {
                           %>
                                <tr>
                                    <td class="font14">Total On Road Price</td>
                                    <td width="100" class="numeri-cell font14" align="right"><span class="WebRupee">Rs.</span><span style="text-decoration: line-through"><%= CommonOpn.FormatPrice(totalPrice.ToString()) %></span></td>
                                </tr>
                                <tr>
                                    <td class="font14">Minus Insurance</td>
                                    <td width="100" class="numeri-cell font14" align="right"><span class="WebRupee">Rs.</span><%= CommonOpn.FormatPrice(insuranceAmount.ToString()) %></td>
                                </tr>
                                <tr>
                                    <td class="price2 font14">BikeWale On Road (after insurance offer)</td>
                                    <td width="100" class="numeri-cell font14" align="right"><span class="WebRupee">Rs.</span><b><%= CommonOpn.FormatPrice((totalPrice - insuranceAmount).ToString()) %></b></td>
                                </tr>
                                <%
                       }
                       else
                       {
                           %>
                           <tr>
                                    <td class="price2 font14">Total On Road Price</td>
                                    <td width="100" class="numeri-cell font14" align="right"><span class="WebRupee">Rs.</span><b><%= CommonOpn.FormatPrice(totalPrice.ToString()) %></b></td>
                           </tr>

                    <%
                       }
                                     %>                                
                                <%-- End 102155010 --%>
                                <tr>
                                    <td colspan="2" align="right"><a id="dealerPriceQuote" class="blue font14" onclick="dataLayer.push({ event: 'product_bw_gtm', cat: 'New Bike Booking - <%=BikeName.Replace("'","")%>', act: 'Click Link Get_Dealer_Details',lab: 'Clicked on Link Get_Dealer_Details' });">Get Dealer Details</a></td>
                                </tr>
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
                        </td>
			        </tr>
		        </table>
                    <% } else { %>
                    <div class="grey-bg border-light padding5 margin-top10 margin-bottom20">
                        <h3>Dealer Prices for this Version is not available.</h3>
                    </div>
                    <% } %>
            </div>
            <%--<div class="dotted-hr margin-bottom10 margin-top10"></div>--%>

            </div>
                <%--<div id="blackOut-window" class="hide"></div>--%>
    <!--bw rsa popup starts here-->
    <div class="rsa-popup bw-popup hide">
        <!--header starts here-->
        <div class="rsa-header">
            <div class="bw-sprite white-close-btn right-float margin-top5"></div>
            <div class="left-float margin-right10 header-seperator">
                <img class="margin-right10" src="http://img1.carwale.com/bikewaleimg/images/bikebooking/images/rsa-logo.png" border="0">
                        
            </div>
            <div class=" left-float margin-top5">
                <h1>FREE Helmet & Bike RSA Offer</h1>
            </div>
            <div class="clear"></div>
        </div>
        <!--header ends here-->
        <!--inner content starts here-->
        <div class="popup-inner-content">
            <h2>Three simple steps to avail the offers:</h2>
            <div class="steps">
                <h2>Step 1:</h2>
                <p>Purchase your bike and get it registered with RTO.</p>
            </div>
            <div class="seperator"></div>
            
            <div class="steps">
                <h2>Step 2:</h2>
                <p>Provide your bike purchase details on <a class="blue" href="/pricequote/RSAOfferClaim.aspx">this link.</a></p>
                <p>OR</p>
                <p>Email the following details to <a class="blue" href="#">contact@bikewale.com</a> with subject as <span class="color-text">"Free Helmet and RSA Offer"</span>:</p>
                
                    <div class="rsa-details margin-left20 margin-top10">
                        <ol>
                            <li>Mobile number and Email address used to avail the Dealer Price Certificate</li>
                            <li>Vehicle Registration Number (e.g. MH 06 AT 8875)</li>
                            <li>Full Name as per Vehicle Registration</li>
                            <li>Complete Communication Address as per Vehicle Registration</li>
                            <li>Date of vehicle Delivery</li>
                            <li>Name and address of the dealership from where the bike was purchased  </li>
                        </ol>
                    </div>
            </div>
            <div class="seperator"></div>
            
            <div class="steps">
                <h2>Step 3:</h2>
                <p>On receipt of above details we will verify your purchase from the dealership and dispatch your<strong> FREE Helmet and Bike Roadside Assistance Certificate</strong> on your provided address within 30 days.
            </p>
            </div>
        </div>
        <!--header starts here-->
    </div>
    <!--bw rsa popup ends here-->
                </div>
            <div id="div_ShowErrorMsg" runat="server" class="grey-bg border-light content-block text-highlight margin-top15"></div>
            </div>
        </div>
        <div class="grid_4  margin-top5 alpha omega">
            <div class="padding5">
            <div class="dealer-offers red-bullets">
                    <!--Exciting offers div starts here-->
                <%if (objPrice.objOffers != null && objPrice.objOffers.Count > 0)
                  { %>
                <div id="divOffers" style="background:#fff;">                    
                    <h2><%= IsInsuranceFree ? "BikeWale Ganapati Offer" : "Get Absolutely Free"%></h2>
                    <div class="margin-top5 margin-left5 font14">
                          <asp:Repeater ID="rptOffers" runat="server">
                                <HeaderTemplate>
                                        <ul>                                        
                                </HeaderTemplate>
                                <ItemTemplate>                                        
                                    <%-- Start 102155010 --%>
                                        <%--<li style="<%# DataBinder.Eval(Container.DataItem,"OfferCategoryId").ToString() == "3" ? "display:none;" : ""%>"><%# DataBinder.Eval(Container.DataItem,"OfferText")%> </li>--%>
                                    <li><%# DataBinder.Eval(Container.DataItem,"OfferText")%></li>
                                    <%-- End 102155010 --%>
                                </ItemTemplate>                                                            
                                <FooterTemplate>                                                                        
                                    <%-- Start 102155010 --%>
                                        <%--<li>Vega Helmet worth Rs. 1500</li>
                                        <li>1 year of <a target="_blank" href="/Absure Bike RSA.pdf">Roadside Assistance</a>.</li>--%>
                                    <%-- End 102155010 --%>
                                    </ul>
                                </FooterTemplate>                              
                            </asp:Repeater>
                    <div style="text-align:center;" class="mid-box margin-top15 action-btn">
                        <a class="action-btn" id="btnGetDealerDetails" name="btnSavePriceQuote" onclick="dataLayer.push({ event: 'product_bw_gtm', cat: 'New Bike Booking - <%=BikeName.Replace("'","")%>', act: 'Click Button Get_Dealer_Details',lab: 'Clicked on Button Get_Dealer_Details' });">Get Dealer Details</a>
                    </div>
                    </div>
                  </div>                   
                <%} %>
                <!--Exciting offers div ends here-->
                </div>
            <%--<SB:SimilarBike ID="ctrl_similarBikes" TopCount="2" runat="server" Visible="false"/>--%>
            </div>
        </div>
        </div>
        <div class="grid_12 margin-top10 padding-bottom20">
            <SB:SimilarBike ID="ctrl_similarBikes" TopCount="3" runat="server"/>
        </div>     
    </div>
</div>
<script type="text/javascript">
    $("#rsa").click(function () {
        $('#blackOut-window').show();
        $('.rsa-popup').show();
    });

    $('.white-close-btn').click(function () {
        $("#blackOut-window").hide();
        $('.rsa-popup').hide();
    });
</script>    
<!-- #include file="/includes/footerInner.aspx" -->


