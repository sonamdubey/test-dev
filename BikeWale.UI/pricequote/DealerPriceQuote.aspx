<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.BikeBooking.DealerPriceQuote" Trace="false" Async="true" EnableEventValidation="false" %>

<%@ Register Src="~/controls/AlternativeBikes.ascx" TagName="AlternativeBikes" TagPrefix="BW" %>
<%@ Register TagPrefix="PW" TagName="PopupWidget" Src="/controls/PopupWidget.ascx" %>
<%@ Import Namespace="Bikewale.Common" %>
<%@ Import Namespace="Bikewale.BikeBooking" %>

<!doctype html>
<html>
<head>
    <%
        title = objPrice.objMake.MakeName + " " + objPrice.objModel.ModelName + " " + objPrice.objVersion.VersionName + " Price Quote ";
        description = objPrice.objMake.MakeName + " " + objPrice.objModel.ModelName + " " + objPrice.objVersion.VersionName + " price quote";
        keywords = "";
        AdId = "1395986297721";
        AdPath = "/1017752/Bikewale_PQ_";
    isAd970x90Shown = true;
    %>
    <!-- #include file="/includes/headscript.aspx" -->
    <style type="text/css">
        #PQImageVariantContainer img {
            width: 100%;
        }

        .PQDetailsTableTitle {
            color: #82888b;
        }

        .PQDetailsTableAmount, .PQOnRoadPrice {
            color: #4d5057;
        }

        .PQOffersUL {
            margin-left: 18px;
            list-style: disc;
        }

            .PQOffersUL li {
                padding-bottom: 15px;
            }

        .pqVariants .form-control-box {
            width: 92%;
        }

        .form-control-box select.form-control {
            color: #4d5057;
        }

        .jcarousel-wrapper.alternatives-carousel {
            width: 974px;
        }

        .alternatives-carousel .jcarousel li {
            height: auto;
            margin-right: 18px;
        }

            .alternatives-carousel .jcarousel li.front {
                border: none;
            }

        .alternative-section .jcarousel-control-left {
            left: -24px;
        }

        .alternative-section .jcarousel-control-right {
            right: -24px;
        }

        .alternative-section .jcarousel-control-left, .alternative-section .jcarousel-control-right {
            top: 50%;
        }

        .newBikes-latest-updates-container .grid-4 {
            padding-left: 10px;
        }

        .available-colors {
            display: inline-block;
            width: 150px;
            text-align: center;
            margin-bottom: 20px;
            padding: 0 5px;
            vertical-align: top;
        }

            .available-colors .color-box {
                width: 60px;
                height: 60px;
                margin: 0 auto 15px;
                border-radius: 3px;
                background: #f00;
                border: 1px solid #ccc;
            }


        /* lead capture popup */
        .edit-blue-icon {
            width: 16px;
            height: 16px;
        }

        .edit-blue-icon {
            background-position: -115px -250px;
        }

        #leadCapturePopup {
            display: none;
            width: 450px;
            min-height: 470px;
            background: #fff;
            margin: 0 auto;
            position: fixed;
            top: 10%;
            right: 5%;
            left: 5%;
            z-index: 10;
            padding: 30px 40px;
        }

        .personal-info-form-container {
            margin: 10px auto;
            width: 300px;
            min-height: 100px;
        }

            .personal-info-form-container .personal-info-list {
                margin: 0 auto;
                width: 280px;
                float: left;
                margin-bottom: 20px;
                border-radius: 0;
            }

        .personal-info-list .errorIcon, .personal-info-list .errorText {
            display: none;
        }

        #otpPopup {
            display: none;
        }

            #otpPopup .otp-box, #otpPopup .update-mobile-box {
                width: 280px;
                margin: 15px auto 0;
            }

                #otpPopup .update-mobile-box .form-control-box {
                    margin-top: 25px;
                    margin-bottom: 50px;
                }

                #otpPopup .otp-box p.resend-otp-btn {
                    color: #0288d1;
                    cursor: pointer;
                    font-size: 14px;
                }

            #otpPopup .update-mobile-box {
                display: none;
            }

            #otpPopup .edit-mobile-btn, .resend-otp-btn {
                cursor: pointer;
            }

        .icon-outer-container {
            width: 102px;
            height: 102px;
            margin: 0 auto;
            background: #fff;
            border: 1px solid #ccc;
        }

        .icon-inner-container {
            width: 92px;
            height: 92px;
            margin: 4px auto;
            background: #fff;
            border: 1px solid #666;
        }

        .user-contact-details-icon {
            width: 36px;
            height: 44px;
            background-position: 0 -391px;
        }

        .otp-icon {
            width: 30px;
            height: 40px;
            background-position: -46px -391px;
        }

        .mobile-prefix {
            position: absolute;
            padding: 10px 13px 13px;
            color: #999;
        }

        #otpPopup .errorIcon, #otpPopup .errorText {
            display: none;
        }

        .input-border-bottom {
            border-bottom: 1px solid #ccc;
        }
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
        var clientIP = "<%= clientIP%>";
        var pageUrl = "www.bikewale.com/quotation/dealerpricequote.aspx?versionId=" + versionId + "&cityId=" + cityId;
    </script>
</head>
<body class="bg-light-grey header-fixed-inner">
    <form runat="server">
        <!-- #include file="/includes/headBW.aspx" -->
        <section class="bg-light-grey padding-top10">
            <div class="container">
                <div class="grid-12">
                    <div class="breadcrumb margin-bottom15">
                        <!-- breadcrumb code starts here -->
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
                    <div class="alpha grid-3" id="PQImageVariantContainer">
                        <% if (objPrice != null)
                           { %>
                        <div class="pqBikeImage margin-bottom20 margin-top5">
                            <img alt="<%= objPrice.objMake.MakeName + " " + objPrice.objModel.ModelName + " " + objPrice.objVersion.VersionName %> Photos" src="<%= Bikewale.Utility.Image.GetPathToShowImages(objPrice.OriginalImagePath,objPrice.HostUrl,Bikewale.Utility.ImageSize._210x118) %>" title="<%= objPrice.objMake.MakeName + " " + objPrice.objModel.ModelName + " " + objPrice.objVersion.VersionName%> Photos" />
                        </div>
                        <% } %>
                        <% if (versionList.Count > 1)
                           { %>
                        <div class="pqVariants margin-top15 ">
                            <div class="form-control-box">
                                <asp:DropDownList ID="ddlVersion" CssClass="form-control" runat="server" AutoPostBack="true"></asp:DropDownList>
                            </div>
                        </div>
                        <% } %>
                        <div class="hide">
                            <div class="<%= objColors.Count == 0 ? "hide" : "" %>" style="float: left; margin-right: 3px; padding-top: 3px;">Color: </div>
                            <div style="overflow: hidden;">
                                <ul class="colours <%= objColors.Count == 0 ? "hide" : ""%>">

                                    <asp:Repeater ID="rptColors" runat="server">
                                        <ItemTemplate>
                                            <li>
                                                <div title="<%#DataBinder.Eval(Container.DataItem,"ColorName") %>" style="background-color: #<%# DataBinder.Eval(Container.DataItem,"ColorCode")%>; height: 15px; width: 15px; margin: 5px; border: 1px solid #a6a9a7;"></div>
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                                <div class="clear"></div>
                            </div>
                            <div class="clear"></div>
                        </div>
                    </div>
                    <div class="padding-right20 border-solid-right grid-5 " id="PQDetailsContainer">
                        <% if (objPrice != null)
                           { %>
                        <p class="font20 text-bold margin-bottom20"><%= objPrice.objMake.MakeName + " " + objPrice.objModel.ModelName + " " +objPrice.objVersion.VersionName%></p>
                        <% } %>
                        <% if (!String.IsNullOrEmpty(cityArea))
                           { %>
                        <p class="font16 margin-bottom15">On-road price in <%= cityArea%></p>
                        <% } %>
                        <div runat="server">
                            <div>
                                <% if (objPrice != null)
                                   { %>
                                <table class="font14" cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <asp:Repeater ID="rptPriceList" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td width="245" class="PQDetailsTableTitle padding-bottom10">
                                                    <%# DataBinder.Eval(Container.DataItem,"CategoryName") %> <%# Bikewale.common.DealerOfferHelper.HasFreeInsurance(dealerId.ToString(),"",DataBinder.Eval(Container.DataItem,"CategoryName").ToString(),Convert.ToUInt32(DataBinder.Eval(Container.DataItem,"Price").ToString()),ref insuranceAmount) ? "<img class='insurance-free-icon' alt='Free_icon' src='http://imgd1.aeplcdn.com/0x0/bw/static/free_red.png' title='Free_icon'/>" : "" %>
                                                </td>
                                                <td align="right" class="PQDetailsTableAmount text-bold padding-bottom10">
                                                    <span class="fa fa-rupee margin-right5"></span><span id="exShowroomPrice"><%#CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"Price").ToString()) %></span>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <tr>
                                        <td colspan="2">
                                            <div class="border-solid-top padding-bottom10"></div>
                                            <td>
                                    </tr>
                                    <%
                                       if (IsDiscount)//if (IsInsuranceFree)
                                       {
                                    %>
                                    <%--<tr>
                                        <td class="PQDetailsTableTitle padding-bottom10">Total on road price</td>
                                        <td align="right" class="PQDetailsTableAmount text-bold padding-bottom10">
                                            <span class="fa fa-rupee"></span><span style="text-decoration: line-through;"><%= CommonOpn.FormatPrice(totalPrice.ToString()) %></span>
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
                                    </tr>--%>
                                    <tr>
                                        <td class="PQDetailsTableTitle padding-bottom10">Total on road price</td>
                                        <td align="right" class="PQDetailsTableAmount text-bold padding-bottom10">
                                            <span class="fa fa-rupee"></span><span style="text-decoration: line-through;"><%= CommonOpn.FormatPrice(totalPrice.ToString()) %></span>
                                        </td>
                                    </tr>
                                    <asp:Repeater ID="rptDiscount" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td width="245" class="PQDetailsTableTitle padding-bottom10">
                                                   Minus <%# DataBinder.Eval(Container.DataItem,"CategoryName") %> 
                                                </td>
                                                <td align="right" class="PQDetailsTableAmount text-bold padding-bottom10">
                                                    <span class="fa fa-rupee margin-right5"></span><span id="exShowroomPrice">
                                                        <%#CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"Price").ToString()) %></span>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <tr>
                                        <td colspan="2">
                                            <div class="border-solid-top padding-bottom10"></div>
                                            <td>
                                    </tr>
                                    <tr>
                                        <td valign="middle" class="PQDetailsTableTitle font18 text-bold PQOnRoadPrice">Total on road price</td>
                                        <td align="right" class="PQDetailsTableAmount font20 text-bold">
                                            <span class="fa fa-rupee"></span><span><%= CommonOpn.FormatPrice((totalPrice - totalDiscount).ToString()) %></span>
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

                                    <tr>
                                        <td colspan="2" class="text-right"><a class="font16 text-bold  text-link" id="leadLink" name="leadLink" onclick="dataLayer.push({ event: 'Bikewale_all', cat: 'New Bike Booking - <%=BikeName.Replace("'","")%>', act: 'Click Button Get dealer details',lab: 'Clicked on Button Get_Dealer_Details' });">Get dealer details</a></td>
                                    </tr>
                                    <tr class="hide">
                                        <td colspan="3">
                                            <ul class="std-ul-list">
                                                <asp:Repeater ID="rptDisclaimer" runat="server">
                                                    <ItemTemplate>
                                                        <li><i><%# Container.DataItem %></i></li>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </ul>
                                        </td>
                                    </tr>
                                </table>
                                <% }
                                   else
                                   { %>
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
                            <p class="font20 text-bold margin-bottom10 border-solid-bottom padding-bottom5"><%= (Convert.ToUInt32(bookingAmount) > 0)?"Book online and avail":"Avail offers" %></p>
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
                            </div>
                        </div>
                        <%}
                           else
                           {%>
                        <div>
                            <p class="font20 text-bold margin-bottom10 border-solid-bottom padding-bottom5">Get following details on bike</p>  
                            <ul class="font14 text-light-grey PQOffersUL">
                                <li>Offers from the nearest dealers</li>
                                <li>Waiting period on this bike at the dealership</li>
                                <li>Nearest dealership from your place</li>
                            </ul>
                        </div>

                        <%} %>

                        <!--Exciting offers div ends here-->
                        <a class="margin-top15 btn btn-orange" id="leadBtnBookNow" name="leadBtnBookNow" >Get more details</a>

                    </div>
                    <div class="clear"></div>
                    <div class="omega grid-3">
                    </div>
                    <div class="text-right alpha omega <%= (objPrice.objOffers != null && objPrice.objOffers.Count > 0) ? "grid-5 " : "grid-6"%>">
                        <%--  <% if (bookingAmount > 0)
                           { %>
                        <a class="margin-top15 margin-left5 btn btn-grey" id="btnBikeBooking" name="btnBikeBooking" onclick="dataLayer.push({ event: 'Bikewale_all', cat: 'New Bike Booking - <%=BikeName.Replace("'","")%>', act: 'Click Button Book Now',lab: 'Clicked on Button Get_Dealer_Details' });">Book Now</a>
                        <% } %>--%>
                    </div>
                    <div class="grid-4">
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


        <!-- lead capture popup start-->
        <div id="leadCapturePopup" class="text-center rounded-corner2">
            <div class="leadCapture-close-btn position-abt pos-top10 pos-right10 bwsprite cross-lg-lgt-grey cur-pointer"></div>
            <!-- contact details starts here -->
            <div id="contactDetailsPopup">
                <div class="icon-outer-container rounded-corner50">
                    <div class="icon-inner-container rounded-corner50">
                        <span class="bwsprite user-contact-details-icon margin-top25"></span>
                    </div>
                </div>
                <p class="font20 margin-top25 margin-bottom10">Provide contact details</p>
                <p class="text-light-grey margin-bottom20">For you to see more details about this bike, please submit your valid contact details. It will be safe with us.</p>
                <div class="personal-info-form-container">
                    <div class="form-control-box personal-info-list">
                        <input type="text" class="form-control get-first-name" placeholder="Full name (mandatory)"
                            id="getFullName" data-bind="value: fullName">
                        <span class="bwsprite error-icon errorIcon"></span>
                        <div class="bw-blackbg-tooltip errorText">Please enter your first name</div>
                    </div>
                    <div class="form-control-box personal-info-list">
                        <input type="text" class="form-control get-email-id" placeholder="Email address (mandatory)"
                            id="getEmailID" data-bind="value: emailId">
                        <span class="bwsprite error-icon errorIcon"></span>
                        <div class="bw-blackbg-tooltip errorText">Please enter email address</div>
                    </div>
                    <div class="form-control-box personal-info-list">
                        <p class="mobile-prefix">+91</p>
                        <input type="text" class="form-control padding-left40 get-mobile-no" placeholder="Mobile no. (mandatory)"
                            id="getMobile" maxlength="10" data-bind="value: mobileNo">
                        <span class="bwsprite error-icon errorIcon"></span>
                        <div class="bw-blackbg-tooltip errorText">Please enter mobile number</div>
                    </div>
                    <div class="clear"></div>
                    <a class="btn btn-orange margin-top10" id="user-details-submit-btn" data-bind="event: { click: submitLead }">Submit</a>
                </div>
            </div>
            <!-- contact details ends here -->
            <!-- otp starts here -->
            <div id="otpPopup">
                <div class="icon-outer-container rounded-corner50">
                    <div class="icon-inner-container rounded-corner50">
                        <span class="bwsprite otp-icon margin-top25"></span>
                    </div>
                </div>
                <p class="font18 margin-top25 margin-bottom20">Verify your mobile number</p>
                <p class="font14 text-light-grey margin-bottom20">We have sent an OTP on the following mobile number. Please enter that OTP in the box provided below:</p>
                <div>
                    <div class="lead-mobile-box lead-otp-box-container font22">
                        <span class="fa fa-phone"></span>
                        <span class="text-light-grey font24">+91</span>
                        <span class="lead-mobile font24"></span>
                        <span class="bwsprite edit-blue-icon edit-mobile-btn"></span>
                    </div>
                    <div class="otp-box lead-otp-box-container">
                        <div class="form-control-box margin-bottom10">
                            <input type="text" class="form-control" maxlength="5" placeholder="Enter your OTP" id="getOTP" data-bind="value: otpCode">
                            <span class="bwsprite error-icon errorIcon"></span>
                            <div class="bw-blackbg-tooltip errorText"></div>
                        </div>
                        <a class="resend-otp-btn margin-left10 blue rightfloat resend-otp-btn" id="resendCwiCode" data-bind="visible: (NoOfAttempts() < 2), click: function () { regenerateOTP() }">Resend OTP
                        </a>
                        <p class="otp-alert-text margin-left10 otp-notify-text text-light-grey font12 margin-top10" data-bind="visible: (NoOfAttempts() >= 2)">
                            OTP has been already sent to your mobile
                        </p>
                        <div class="clear"></div>
                        <input type="button" class="btn btn-orange margin-top20" value="Confirm OTP" id="otp-submit-btn">
                    </div>
                    <div class="update-mobile-box">
                        <div class="form-control-box text-left">
                            <p class="mobile-prefix">+91</p>
                            <input type="text" class="form-control padding-left40" placeholder="Mobile no." maxlength="10" id="getUpdatedMobile" data-bind="value: mobileNo" />
                            <span class="bwsprite error-icon errorIcon"></span>
                            <div class="bw-blackbg-tooltip errorText"></div>
                        </div>
                        <input type="button" class="btn btn-orange" value="Send OTP" id="generateNewOTP" data-bind="event: { click: submitLead }" />
                    </div>
                </div>
            </div>
            <!-- otp ends here -->
        </div>
        <!-- lead capture popup End-->


        <PW:PopupWidget runat="server" ID="PopupWidget" />
        <!-- #include file="/includes/footerBW.aspx" -->
        <!-- #include file="/includes/footerscript.aspx" -->
        <script type="text/javascript">
            var bikeName = '<%= BikeName %>';
            var getCityArea = GetGlobalCityArea();
            $('#btnGetDealerDetails, #btnBikeBooking').click(function () {
                window.location.href = '/pricequote/bookingsummary_new.aspx';
            });

            var freeInsurance = $("img.insurance-free-icon");
            if (!freeInsurance.length) {
                cityArea = GetGlobalCityArea();
                $("table tr td.PQDetailsTableTitle:contains('Insurance')").append(" <br/><div style='position: relative; color: #999; font-size: 11px; margin-top: 1px;'>Save up to 60% on insurance - <a target='_blank' href='/insurance/' onclick=\"dataLayer.push({ event: 'Bikewale_all', cat: 'Dealer_PQ', act: 'Insurance_Clicked',lab: '<%= String.Format("{0}_{1}_{2}_",objPrice.objMake.MakeName,objPrice.objModel.ModelName,objPrice.objVersion.VersionName)%>" + cityArea + "' });\">PolicyBoss</a> <span style='margin-left: 8px; vertical-align: super; font-size: 9px;'>Ad</span></div>");
            }

            // JavaScript Document

            var leadBtnBookNow = $("#leadBtnBookNow,#leadLink"), leadCapturePopup = $("#leadCapturePopup");
            var fullName = $("#getFullName");
            var emailid = $("#getEmailID");
            var mobile = $("#getMobile");
            var otpContainer = $(".mobile-verification-container");


            var detailsSubmitBtn = $("#user-details-submit-btn");
            var otpText = $("#getOTP");
            var otpBtn = $("#otp-submit-btn");

            var prevEmail = "";
            var prevMobile = "";

            var getCityArea = GetGlobalCityArea();
            var customerViewModel = new CustomerModel();

            $(function () {
                leadBtnBookNow.on('click', function () {
                    leadCapturePopup.show();
                    $("div#contactDetailsPopup").show();
                    $("#otpPopup").hide();
                    $('body').addClass('lock-browser-scroll');
                    $(".blackOut-window").show();

                });
                $(".leadCapture-close-btn, .blackOut-window").on("click mouseup", function () {
                    leadCapturePopup.hide();
                    $('body').removeClass('lock-browser-scroll');
                    $(".blackOut-window").hide();
                });

                $(document).on('keydown', function (e) {
                    if (e.keyCode === 27) {
                        $("#leadCapturePopup .leadCapture-close-btn").click();
                    }
                });
            });

            ko.applyBindings(customerViewModel, $('#leadCapturePopup')[0]);

            function CustomerModel() {
                var arr = setuserDetails();
                var self = this;
                if (arr != null && arr.length > 0) {
                    self.fullName = ko.observable(arr[0]);
                    self.emailId = ko.observable(arr[1]);
                    self.mobileNo = ko.observable(arr[2]);
                }
                else {
                    self.fullName = ko.observable();
                    self.emailId = ko.observable();
                    self.mobileNo = ko.observable();
                }
                self.IsVerified = ko.observable(false);
                self.NoOfAttempts = ko.observable(0);
                self.IsValid = ko.computed(function () { return self.IsVerified(); }, this);
                self.otpCode = ko.observable();

                self.verifyCustomer = function () {
                    if (!self.IsVerified()) {
                        var objCust = {
                            "dealerId": dealerId,
                            "pqId": pqId,
                            "customerName": self.fullName(),
                            "customerMobile": self.mobileNo(),
                            "customerEmail": self.emailId(),
                            "clientIP": clientIP,
                            "pageUrl": pageUrl,
                            "versionId": versionId,
                            "cityId": cityId,
                            "leadSourceId": 1,
                            "deviceId": getCookie('BWC')
                        }
                        $.ajax({
                            type: "POST",
                            url: "/api/PQCustomerDetail/",
                            data: ko.toJSON(objCust),
                            beforeSend: function (xhr) {
                                xhr.setRequestHeader('utma', getCookie('__utma'));
                                xhr.setRequestHeader('utmz', getCookie('__utmz'));
                            },
                            async: false,
                            contentType: "application/json",
                            success: function (response) {
                                var obj = ko.toJS(response);
                                self.IsVerified(obj.isSuccess);
                                if (!self.IsVerified()) {
                                    self.NoOfAttempts(obj.noOfAttempts);
                                }
                            },
                            error: function (xhr, ajaxOptions, thrownError) {
                                self.IsVerified(false);
                            }
                        });
                    }
                };
                self.generateOTP = function () {
                    if (!self.IsVerified()) {
                        var objCust = {
                            "pqId": pqId,
                            "customerMobile": self.mobileNo(),
                            "customerEmail": self.emailId(),
                            "cwiCode": self.otpCode(),
                            "branchId": dealerId,
                            "customerName": self.fullName(),
                            "versionId": versionId,
                            "cityId": cityId
                        }
                        $.ajax({
                            type: "POST",
                            url: "/api/PQMobileVerification/",
                            data: ko.toJSON(objCust),
                            async: false,
                            contentType: "application/json",
                            success: function (response) {
                                var obj = ko.toJS(response);
                                self.IsVerified(obj.isSuccess);
                            },
                            error: function (xhr, ajaxOptions, thrownError) {
                                self.IsVerified(false);
                            }
                        });
                    }
                };

                self.regenerateOTP = function () {
                    if (self.NoOfAttempts() <= 2 && !self.IsVerified()) {
                        var url = '/api/ResendVerificationCode/';
                        var objCustomer = {
                            "customerName": self.fullName(),
                            "customerMobile": self.mobileNo(),
                            "customerEmail": self.emailId(),
                            "source": 1
                        }
                        $.ajax({
                            type: "POST",
                            url: url,
                            async: false,
                            data: ko.toJSON(objCustomer),
                            contentType: "application/json",
                            success: function (response) {
                                self.IsVerified(false);
                                self.NoOfAttempts(response.noOfAttempts);
                                alert("You will receive the new OTP via SMS shortly.");
                            },
                            error: function (xhr, ajaxOptions, thrownError) {
                                self.IsVerified(false);
                            }
                        });
                    }
                };

                self.submitLead = function () {
                    if (ValidateUserDetail()) {
                        self.verifyCustomer();
                        if (self.IsValid()) {
                            $("#personalInfo").hide();
                            $("#leadCapturePopup .leadCapture-close-btn").click();
                            window.location.href = "/pricequote/BikeDealerDetails.aspx";
                        }
                        else {
                            $("#contactDetailsPopup").hide();
                            $("#otpPopup").show();
                            var leadMobileVal = mobile.val();
                            $("#otpPopup .lead-mobile-box").find("span.lead-mobile").text(leadMobileVal);
                            otpContainer.removeClass("hide").addClass("show");
                            nameValTrue();
                            hideError(mobile);
                            otpText.val('').removeClass("border-red").siblings("span, div").hide();
                        }
                        setPQUserCookie();
                        dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Dealer_PQ', 'act': 'Lead_Submitted', 'lab': bikeName + '_' +getCityArea });
                    }
                };

                otpBtn.click(function () {
                    $('#processing').show();
                    if (!validateOTP())
                        $('#processing').hide();

                    if (validateOTP() && ValidateUserDetail()) {
                        customerViewModel.generateOTP();
                        if (customerViewModel.IsVerified()) {
                            $("#personalInfo").hide();
                            $(".booking-dealer-details").removeClass("hide").addClass("show");
                            $('#processing').hide();

                            detailsSubmitBtn.show();
                            otpText.val('');
                            otpContainer.removeClass("show").addClass("hide");

                            // OTP Success
                            dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'DealerQuotation_Page', 'act': 'Step_1_OTP_Successful_Submit', 'lab': getCityArea });
                            $("#leadCapturePopup .leadCapture-close-btn").click();
                            window.location.href = "/pricequote/BikeDealerDetails.aspx";
                        }
                        else {
                            $('#processing').hide();
                            otpVal("Please enter a valid OTP.");
                            // push OTP invalid
                            dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'DealerQuotation Page', 'act': 'Step_1_OTP_Submit_Error', 'lab': getCityArea });
                        }
                    }
                });
            }

            function ValidateUserDetail() {
                var isValid = true;
                isValid = validateEmail();
                isValid &= validateMobile();
                isValid &= validateName();
                return isValid;
            };

            function validateName() {
                var isValid = true;
                var a = fullName.val().length;
                if ((/&/).test(fullName.val())) {
                    isValid = false;
                    setError(fullName, 'Invalid name');
                }
                else
                    if (a == 0) {
                        isValid = false;
                        setError(fullName, 'Please enter your first name');
                    }
                    else if (a >= 1) {
                        isValid = true;
                        nameValTrue()
                    }
                if (!isValid) { dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'DealerQuotation Page', 'act': 'Step_1_Submit_Error_Name', 'lab': getCityArea }); }
                return isValid;
            }

            function nameValTrue() {
                hideError(fullName)
                fullName.siblings("div").text('');
            };

            fullName.on("focus", function () {
                hideError(fullName);
            });

            emailid.on("focus", function () {
                hideError(emailid);
                prevEmail = emailid.val().trim();
            });

            mobile.on("focus", function () {
                hideError(mobile)
                prevMobile = mobile.val().trim();

            });

            emailid.on("blur", function () {
                if (prevEmail != emailid.val().trim()) {
                    if (validateEmail()) {
                        customerViewModel.IsVerified(false);
                        detailsSubmitBtn.show();
                        otpText.val('');
                        otpContainer.removeClass("show").addClass("hide");
                        hideError(emailid);
                    }
                    $('#confirmation-tab').addClass('disabled-tab').removeClass('active-tab text-bold');
                    $('#customize-tab').addClass('disabled-tab').removeClass('active-tab text-bold');
                }
            });

            mobile.on("blur", function () {
                if (mobile.val().length < 10) {
                    $("#user-details-submit-btn").show();
                    $(".mobile-verification-container").removeClass("show").addClass("hide");
                }
                if (prevMobile != mobile.val().trim()) {
                    if (validateMobile()) {
                        customerViewModel.IsVerified(false);
                        detailsSubmitBtn.show();
                        otpText.val('');
                        otpContainer.removeClass("show").addClass("hide");
                        hideError(mobile);
                    }
                    $('#confirmation-tab').addClass('disabled-tab').removeClass('active-tab text-bold');
                    $('#customize-tab').addClass('disabled-tab').removeClass('active-tab text-bold');
                }
            });

            function mobileValTrue() {
                mobile.removeClass("border-red");
                mobile.siblings("span, div").hide();
            };


            otpText.on("focus", function () {
                otpText.val('');
                otpText.siblings("span, div").hide();
            });

            function setError(ele, msg) {
                ele.addClass("border-red");
                ele.siblings("span, div").show();
                ele.siblings("div").text(msg);
            }

            function hideError(ele) {
                ele.removeClass("border-red");
                ele.siblings("span, div").hide();
            }
            /* Email validation */
            function validateEmail() {
                var isValid = true;
                var emailID = emailid.val();
                var reEmail = /^[A-z0-9._+-]+@[A-z0-9.-]+\.[A-z]{2,6}$/;

                if (emailID == "") {
                    setError(emailid, 'Please enter email address');
                    isValid = false;
                }
                else if (!reEmail.test(emailID)) {
                    setError(emailid, 'Invalid Email');
                    isValid = false;
                }
                if (!isValid) { dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'DealerQuotation Page', 'act': 'Step_1_Submit_Error_Email', 'lab': getCityArea }); }
                return isValid;
            }

            function validateMobile() {
                var isValid = true;
                var reMobile = /^[0-9]{10}$/;
                var mobileNo = mobile.val();
                if (mobileNo == "") {
                    isValid = false;
                    setError(mobile, "Please enter your Mobile Number");
                }
                else if (!reMobile.test(mobileNo) && isValid) {
                    isValid = false;
                    setError(mobile, "Mobile Number should be 10 digits");
                }
                else {
                    hideError(mobile)
                }
                if (!isValid) { dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'DealerQuotation Page', 'act': 'Step_1_Submit_Error_Mobile', 'lab': getCityArea }); }
                return isValid;
            }

            var otpVal = function (msg) {
                otpText.addClass("border-red");
                otpText.siblings("span, div").show();
                otpText.siblings("div").text(msg);
            };

            function validateOTP() {
                var retVal = true;
                var isNumber = /^[0-9]{5}$/;
                var cwiCode = otpText.val();
                customerViewModel.IsVerified(false);
                if (cwiCode == "") {
                    retVal = false;
                    otpVal("Please enter your Verification Code");
                    bindInsuranceText();
                }
                else {
                    if (isNaN(cwiCode)) {
                        retVal = false;
                        otpVal("Verification Code should be numeric");
                    }
                    else if (cwiCode.length != 5) {
                        retVal = false;
                        otpVal("Verification Code should be of 5 digits");
                    }
                }
                return retVal;
            }

            function setuserDetails() {
                var cookieName = "_PQUser";
                if (isCookieExists(cookieName)) {
                    var arr = getCookie(cookieName).split("&");
                    return arr;
                }
            }

            function setPQUserCookie() {
                var val = fullName.val() + '&' + emailid.val() + '&' + mobile.val();
                SetCookie("_PQUser", val);
            }

            $(".edit-mobile-btn").on("click", function () {
                var prevMobile = $(this).prev("span.lead-mobile").text();
                $(".lead-otp-box-container").hide();
                $(".update-mobile-box").show();
                $("#getUpdatedMobile").val(prevMobile).focus();
            });

            $("#generateNewOTP").on("click", function () {
                if (validateUpdatedMobile()) {
                    var updatedNumber = $(".update-mobile-box").find("#getUpdatedMobile").val();
                    $(".update-mobile-box").hide();
                    $(".lead-otp-box-container").show();
                    $(".lead-mobile-box").find(".lead-mobile").text(updatedNumber);
                }
            });

            var validateUpdatedMobile = function () {
                var isValid = true,
                    mobileNo = $("#getUpdatedMobile"),
                    mobileVal = mobileNo.val(),
                    reMobile = /^[0-9]{10}$/;
                if (mobileVal == "") {
                    setError(mobileNo, "Please enter your Mobile Number");
                    isValid = false;
                }
                else if (!reMobile.test(mobileVal) && isValid) {
                    setError(mobileNo, "Mobile Number should be 10 digits");
                    isValid = false;
                }
                else
                    hideError(mobileNo)
                return isValid;
            };
            $("#leadBtnBookNow").on("click", function () {
                dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Dealer_PQ', 'act': 'Get_More_Details_Clicked_Button', 'lab': bikeName + '_' + getCityArea });
            });
            $("#leadLink").on("click", function () {
                dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Dealer_PQ', 'act': 'Get_More_Details_Clicked_Link', 'lab': bikeName + '_' + getCityArea });
            });
        </script>
    </form>
</body>
</html>
