<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.BikeBooking.DealerPriceQuote_old" Trace="false" Async="true" %>

<%@ Register Src="~/m/controls/AlternativeBikes.ascx" TagPrefix="BW" TagName="AlternateBikes" %>
<%@ Import Namespace="Bikewale.Common" %>
<%@ Import Namespace="Bikewale.BikeBooking" %>
<!doctype html>
<html>
<head>
    <%
        title = objPrice.objMake.MakeName + " " + objPrice.objModel.ModelName + " " + objPrice.objVersion.VersionName + " Price Quote ";
        description = objPrice.objMake.MakeName + " " + objPrice.objModel.ModelName + " " + objPrice.objVersion.VersionName + " price quote";
        keywords = "";
        canonical = "";
        AdPath = "/1017752/Bikewale_Mobile_PriceQuote";
        AdId = "1398766000399";
    %>
    <script>var quotationPage = true;</script>
    <!-- #include file="/includes/headscript_mobile.aspx" -->
    <link rel="stylesheet" href="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/m/css/bw-new-style.css?<%= staticFileVersion %>" />

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
    <style type="text/css">
        .inner-section {
            background: #fff;
            clear: both;
            overflow: hidden;
        }

        .alternatives-carousel .jcarousel li.front {
            border: none;
        }

        .discover-bike-carousel .jcarousel li {
            height: auto;
        }

        .discover-bike-carousel .front {
            height: auto;
        }

        #leadCapturePopup .leadCapture-close-btn {
            z-index: 2;
        }

        #leadCapturePopup .error-icon, #leadCapturePopup .bw-blackbg-tooltip {
            display: none;
        }

        .btn-grey {
            background: #fff;
            color: #82888b;
            border: 1px solid #82888b;
        }

            .btn-grey:hover {
                background: #82888b;
                color: #fff;
                text-decoration: none;
                border: 1px solid #82888b;
            }

        /*notify availability*/
        #notifyAvailabilityContainer {
            min-height: 320px;
            background: #fff;
            margin: 0 auto;
            padding: 10px;
            position: fixed;
            top: 10%;
            right: 5%;
            left: 5%;
            z-index: 10;
        }

        #notify-form .grid-12 {
            padding: 10px 20px;
        }

        .personal-info-notify-container input {
            margin: 0 auto;
        }

        .notify-offers-list {
            list-style: disc;
            margin-left: 10px;
        }

        #notifyAvailabilityContainer .notify-close-btn {
            z-index: 2;
        }

        #leadCapturePopup .error-icon, #leadCapturePopup .bw-blackbg-tooltip {
            display: none;
        }

        .float-button {
            background-color: #f5f5f5;
            padding: 10px;
        }

            .float-button.float-fixed {
                position: fixed;
                bottom: 0;
                z-index: 8;
                left: 0;
                right: 0;
            }

        /**/
        #otpPopup {
            display: none;
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
            background-position: -107px -227px;
        }

        .otp-icon {
            width: 30px;
            height: 40px;
            background-position: -107px -177px;
        }

        .edit-blue-icon {
            width: 16px;
            height: 16px;
            background-position: -114px -123px;
        }

        #getMobile {
            padding: 9px 40px;
        }

        .mobile-prefix {
            position: absolute;
            padding: 10px 13px 13px;
            color: #999;
            z-index: 2;
        }

        #otpPopup .errorIcon, #otpPopup .errorText {
            display: none;
        }

        #otpPopup .otp-box p.resend-otp-btn {
            color: #0288d1;
            cursor: pointer;
            font-size: 14px;
        }

        #otpPopup .update-mobile-box {
            display: none;
        }

        #otpPopup .edit-mobile-btn {
            cursor: pointer;
        }
    </style>
</head>
<body class="bg-light-grey">
    <form runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->
        <div class="bg-white box1 box-top new-line5 bot-red new-line10">

            <div class="bike-img">
                <img src="<%= Bikewale.Utility.Image.GetPathToShowImages(objPrice.OriginalImagePath,objPrice.HostUrl,Bikewale.Utility.ImageSize._640x348) %>" alt="" title="" border="0" />
            </div>
            <h1 class="margin-top20 font18 padding-left10 padding-right10" style="margin-left: 0px;"><%= objPrice.objMake.MakeName + " " + objPrice.objModel.ModelName + " " + objPrice.objVersion.VersionName %> Price Quote</h1>
            <div class="<%= objColors.Count == 0 ?"hide":"hide" %>">
                <div class="full-border new-line10 selection-box">
                    <b>Color Options: </b>
                    <table width="100%">
                        <tr style="margin-bottom: 5px;">
                            <td class="break-line" colspan="2"></td>
                        </tr>
                        <asp:Repeater ID="rptColors" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td style="width: 30px;">
                                        <div style="width: 30px; height: 20px; margin: 0px 10px 0px 0px; border: 1px solid #a6a9a7; padding-top: 5px; background-color: #<%# DataBinder.Eval(Container.DataItem,"ColorCode")%>"></div>
                                    </td>
                                    <td>
                                        <div class="new-line"><%# DataBinder.Eval(Container.DataItem,"ColorName") %></div>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </div>
            </div>
            <div class="<%= versionList.Count>1 ?"":"hide" %> margin-top20">
                <asp:DropDownList ID="ddlVersion" CssClass="form-control" runat="server" AutoPostBack="true"></asp:DropDownList>
            </div>
            <!--Price Breakup starts here-->
            <div class="new-line15 padding-left10 padding-right10" style="margin-top: 20px;">

                <% if (!String.IsNullOrEmpty(cityArea))
                   { %>
                <h2 class="font16" style="font-weight: normal">On-road price in <%= cityArea %></h2>
                <% } %>
                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="pqTable font14">

                    <asp:Repeater ID="rptPriceList" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td align="left" width="75%" class="text-medium-grey"><%# DataBinder.Eval(Container.DataItem,"CategoryName") %> <%# Bikewale.common.DealerOfferHelper.HasFreeInsurance(dealerId.ToString(),"",DataBinder.Eval(Container.DataItem,"CategoryName").ToString(),Convert.ToUInt32(DataBinder.Eval(Container.DataItem,"Price").ToString()),ref insuranceAmount) ? "<img class='insurance-free-icon' alt='Free_icon' src='http://imgd1.aeplcdn.com/0x0/bw/static/free_red.png' title='Free_icon'/>" : "" %></td>
                                <td align="right" width="25%" class="text-grey text-bold"><span class="bwmsprite inr-xxsm-icon"></span> <%# CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"Price").ToString()) %></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr align="left">
                        <td height="10" colspan="2" style="padding: 0;"></td>
                    </tr>
                    <tr align="left">
                        <td height="1" colspan="2" class="break-line" style="padding: 0 0 10px;"></td>
                    </tr>
                    <%-- Start 102155010 --%>

                    <%
                        if (IsDiscount)
                        {
                    %>
                    <tr>
                        <td align="left" class="text-medium-grey">Total On Road Price</td>
                        <td align="right" class="text-grey text-bold">
                            <div><span class="bwmsprite inr-xxsm-icon"></span> <span style="text-decoration: line-through"> <%= CommonOpn.FormatPrice(totalPrice.ToString()) %></span></div>
                        </td>
                    </tr>
                    <asp:Repeater ID="rptDiscount" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td align="left" class="text-medium-grey">Minus <%# DataBinder.Eval(Container.DataItem,"CategoryName") %> <%# Bikewale.common.DealerOfferHelper.HasFreeInsurance(dealerId.ToString(),"",DataBinder.Eval(Container.DataItem,"CategoryName").ToString(),Convert.ToUInt32(DataBinder.Eval(Container.DataItem,"Price").ToString()),ref insuranceAmount) ? "<img class='insurance-free-icon' alt='Free_icon' src='http://imgd1.aeplcdn.com/0x0/bw/static/free_red.png' title='Free_icon'/>" : "" %></td>
                                <td align="right" class="text-grey text-bold"><span class="bwmsprite inr-xxsm-icon"></span> <%# CommonOpn.FormatPrice(DataBinder.Eval(Container.DataItem,"Price").ToString()) %></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                     <tr align="left">
                        <td height="1" colspan="2" class="break-line" style="padding: 0 0 10px;"></td>
                    </tr>
                    <tr>
                        <td align="left" class="text-medium-grey">BikeWale On Road</td>
                        <td align="right" class="text-grey text-bold">
                            <div><span class="bwmsprite inr-xxsm-icon"></span> <%= CommonOpn.FormatPrice((totalPrice - totalDiscount).ToString()) %></div>

                        </td>
                    </tr>
                    <%
                        }
                        else
                        {%>
                    <tr>
                        <td align="left" class="text-grey font16">Total On Road Price</td>
                        <td align="right" class="text-grey text-bold font18">
                            <div><span class="bwmsprite inr-sm-icon"></span> <%= CommonOpn.FormatPrice(totalPrice.ToString()) %></div>

                        </td>
                    </tr>
                    <%
                        }
                    %>
                    <%-- End 102155010 --%>
                    <tr>
                        <td colspan="2" align="right">
                            <a data-role="none" id="leadLink" name="leadLink" class="text-bold font16 text-link" onclick="dataLayer.push({ event: 'Bikewale_all', cat: 'DealerQuotation_Page - <%=MakeModel.Replace("'","") %>        ', act: 'Click Button Book Now',lab: 'Clicked on Button Get_Dealer_Details' });">Get dealer details</a>
                        </td>
                    </tr>
                    <tr align="left">
                        <td height="20" colspan="2" style="padding: 0;"></td>
                    </tr>

                    <tr align="left">
                        <td height="1" colspan="2" class="break-line-light" style="padding: 0;">&nbsp;</td>
                    </tr>
                </table>
                <ul class="grey-bullet hide">
                    <asp:Repeater ID="rptDisclaimer" runat="server">
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
            <div class="new-line10 padding-left10 padding-right10 margin-bottom15" id="divOffers" style="background: #fff;">
                <h2 class="font18 text-grey"><%= (Convert.ToUInt32(bookingAmount) > 0)?"Book online and avail":"Avail offers" %></h2>
                <div class="new-line10">
                    <asp:Repeater ID="rptOffers" runat="server">
                        <HeaderTemplate>
                            <ul class="grey-bullet">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <li><%# DataBinder.Eval(Container.DataItem,"OfferText")%>
                                <%--<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "isOfferTerms")) ==  true ? "<span class='tnc' id='"+ DataBinder.Eval(Container.DataItem, "offerId") +"' ><a class='viewterms'>View terms</a></span>" : "" %>--%>
                                <%# "<span class='tnc' id='"+ DataBinder.Eval(Container.DataItem, "offerId") +"' ><a class='viewterms'>View terms</a></span>"  %>
                            </li>
                        </ItemTemplate>
                        <FooterTemplate>
                            </ul>                        
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </div>

            <% }
               else
               {%>

            <div class="new-line10 padding-left10 padding-right10 margin-bottom15" style="background: #fff;">
                <h2 class="font24 margin-left10 text-grey">Get following details on the bike</h2>
                <div class="new-line10">
                    <ul class="grey-bullet">
                        <li>Offers from the nearest dealers</li>
                        <li>Waiting period on this bike at the dealership</li>
                        <li>Nearest dealership from your place</li>
                    </ul>
                </div>
            </div>
            <% } %>

            <div class="grid-12 float-button float-fixed">
                <input type="button" data-role="none" id="leadBtnBookNow" name="leadBtnBookNow" class="btn btn-full-width btn-orange" value="Get more details" />
            </div>
            <div class="clear"></div>
            <!--Exciting Offers section ends here-->
        </div>

        <section class="<%= (ctrlAlternateBikes.FetchedRecordsCount > 0) ? "" : "hide" %>">
            <div class="container margin-bottom30">
                <div class="grid-12">
                    <h2 class="margin-top30px margin-bottom20 text-center padding-top20"><%= objPrice.objMake.MakeName + " " + objPrice.objModel.ModelName  %> alternatives</h2>

                    <div class="swiper-container discover-bike-carousel alternatives-carousel padding-bottom60">
                        <div class="swiper-wrapper">
                            <BW:AlternateBikes ID="ctrlAlternateBikes" runat="server" />
                        </div>
                        <!-- Add Pagination -->
                        <div class="swiper-pagination"></div>
                        <!-- Navigation -->
                        <div class="bwmsprite swiper-button-next hide"></div>
                        <div class="bwmsprite swiper-button-prev hide"></div>
                    </div>

                </div>
                <div class="clear"></div>
            </div>
        </section>


        <!-- Lead Capture pop up start  -->
        <div id="leadCapturePopup" class="bw-popup bwm-fullscreen-popup contact-details hide">
            <div class="popup-inner-container text-center">
                <div class="bwmsprite close-btn leadCapture-close-btn rightfloat"></div>
                <div id="contactDetailsPopup">
                    <!-- Contact details Popup starts here -->
                    <%--<div class="icon-outer-container rounded-corner50percent">
                        <div class="icon-inner-container rounded-corner50percent">
                            <span class="bwmsprite user-contact-details-icon margin-top25"></span>
                        </div>
                    </div>--%>
                    <h2 class="margin-top10 margin-bottom10">Get more details on this bike</h2>
                    <p class="text-light-grey margin-bottom10">Please provide contact info to see more details</p>

                    <div class="personal-info-form-container">
                        <div class="form-control-box">
                            <input type="text" class="form-control get-first-name" placeholder="Your name" id="getFullName" data-bind="value: fullName">
                            <span class="bwmsprite error-icon "></span>
                            <div class="bw-blackbg-tooltip errorText">Please enter your name</div>
                        </div>
                        <div class="form-control-box margin-top20">
                            <input type="text" class="form-control get-email-id" placeholder="Email address" id="getEmailID" data-bind="value: emailId">
                            <span class="bwmsprite error-icon"></span>
                            <div class="bw-blackbg-tooltip errorText">Please enter your email adress</div>
                        </div>
                        <div class="form-control-box margin-top20">
                            <p class="mobile-prefix">+91</p>
                            <input type="text" class="form-control get-mobile-no" maxlength="10" placeholder="Mobile no." id="getMobile" data-bind="value: mobileNo">
                            <span class="bwmsprite error-icon"></span>
                            <div class="bw-blackbg-tooltip errorText">Please enter mobile number</div>
                        </div>
                        <div class="clear"></div>
                        <a class="btn btn-full-width btn-orange margin-top20" id="user-details-submit-btn" data-bind="event: { click: submitLead }">Submit</a>
                    </div>                 
		<input type="button" class="btn btn-full-width btn-orange hide rounded-corner5" value="Submit" onclick="validateDetails();" data-role="none" id="btnSubmit" />
                </div>
                <!-- Contact details Popup ends here -->
                <div id="otpPopup">
                    <!-- OTP Popup starts here -->
                    <%--<div class="icon-outer-container rounded-corner50percent">
                        <div class="icon-inner-container rounded-corner50percent">
                            <span class="bwmsprite otp-icon margin-top25"></span>
                        </div>
                    </div>--%>
                    <p class="font18 margin-top10 margin-bottom10">Verify your mobile number</p>
                    <p class="font14 text-light-grey margin-bottom10">We have sent OTP on your mobile. Please enter that OTP in the box provided below:</p>
                    <div>
                        <div class="lead-mobile-box lead-otp-box-container margin-bottom10 font22">
                            <span class="bwmsprite tel-grey-icon"></span>
                            <span class="text-light-grey font24">+91</span>
                            <span class="lead-mobile font24"></span>
                            <span class="bwmsprite edit-blue-icon edit-mobile-btn"></span>
                        </div>
                        <div class="otp-box lead-otp-box-container">
                            <div class="form-control-box margin-bottom10">
                                <input type="text" class="form-control" placeholder="Enter your OTP" id="getOTP" maxlength="5" data-bind="value: otpCode" />
                                <span class="bwmsprite error-icon errorIcon"></span>
                                <div class="bw-blackbg-tooltip errorText"></div>
                            </div>
                            <a class="margin-left10 blue resend-otp-btn margin-top10" id="resendCwiCode" data-bind="visible: (NoOfAttempts() < 2), click: function () { regenerateOTP() }">Resend OTP</a>
                            <p class="margin-left10 margin-top10 otp-notify-text text-light-grey font12" data-bind="visible: (NoOfAttempts() >= 2)">
                                OTP has been already sent to your mobile
                            </p>
                            <a class="btn btn-full-width btn-orange margin-top20" id="otp-submit-btn">Confirm</a>
                        </div>
                        <div class="update-mobile-box">
                            <div class="form-control-box text-left">
                                <p class="mobile-prefix">+91</p>
                                <input type="text" class="form-control padding-left40" placeholder="Mobile no." maxlength="10" id="getUpdatedMobile" />
                                <span class="bwmsprite error-icon errorIcon"></span>
                                <div class="bw-blackbg-tooltip errorText"></div>
                            </div>
                            <input type="button" class="btn btn-orange margin-top20" value="Send OTP" id="generateNewOTP" />
                        </div>
                    </div>

                </div>
                <!-- OTP Popup ends here -->
            </div>
        </div>
        <!-- Lead Capture pop up end  -->

        <!-- Terms and condition Popup start -->
        <div class="termsPopUpContainer content-inner-block-20 hide" id="termsPopUpContainer">
            <div class="fixed-close-btn-wrapper">
                <div class="termsPopUpCloseBtn bwmsprite fixed-close-btn cross-lg-lgt-grey cur-pointer"></div>
            </div>
            <h3>Terms and Conditions</h3>
            <div class="hide" style="vertical-align: middle; text-align: center;" id="termspinner">
                <img src="/images/search-loading.gif" />
            </div>
            <div id="terms" class="breakup-text-container padding-bottom10 font14">
            </div>
            <div id='orig-terms' class='hide'>
					<h1>Offers and Gifts Promotion Terms and Conditions</h1>
                    <p><strong>Definitions:</strong></p>
                    <p>"BikeWale" refers to Automotive Exchange Private Limited, a private limited company having its head office at 12<sup>th</sup> Floor, Vishwaroop IT Park, Sector 30A, Vashi, Navi Mumbai 400705, India, who owns and operates www.bikewale.com, one of India's leading automotive web portals.</p>
                    <p>"Bike Manufacturer" or "manufacturer" refers to the company that manufactures and / or markets and sells bikes in India through authorised dealers.</p>
                    <p>"Dealership" or "dealer" refers to companies authorised by a Bike Manufacturer to sell their bikes. Each Bike Manufacturer many have more than one Dealership and / or Dealer.</p>
                    <p>"Offer" refers to the promotions, discounts and gifts that are available as displayed on BikeWale.</p>
                    <p>"Buyer" or "user" or "participant" refers to the individual who purchases a Bike and / or avails any of the offers.</p>
                    <p><strong>Offers from Bike Manufacturers and Dealers</strong></p>
                    <p>1. All offers are from Bike manufacturers and / or their dealers, and BikeWale makes no representation or warranty regarding the accuracy, truth, quality, suitability or reliability of such information.</p>
                    <p>2. These terms and conditions are to be read in conjunction with the terms and conditions of the manufacturers / dealers. Please refer to the manufacturers and / or their dealers' websites for a detailed list of terms and conditions that apply to these offers.</p>
                    <p>3. In the event of any discrepancy between the manufacturers / dealers' offer terms and conditions, and the terms and conditions mentioned herewith, the manufacturers / dealers' terms and conditions will apply.</p>
                    <p>4. All questions, clarifications, complaints and any other communication pertaining to these offers should be addressed directly to the manufacturer and / or their dealers. BikeWale will not be able to entertain any communication in this regard.</p>
                    <p>5. The offers may be modified and / or withdrawn by manufacturers and / or their dealers without notice, and buyers are strongly advised to check the availability and detailed terms and conditions of the offer before making a booking.</p>
                    <p>6. Buyers are strongly advised to verify the offer details with the manufacturer and / or the nearest dealer before booking the bike.</p>
                    <p>7. Any payments made towards purchase of the Bike are governed by the terms and conditions agreed between the buyer and the manufacturer and / or the dealer. BikeWale is in no way related to the purchase transaction and cannot be held liable for any refunds, financial loss or any other liability that may arise directly or indirectly out of participating in this promotion.</p>
                    <p><strong>Gifts from BikeWale</strong></p>
                    <p>8. In select cases, BikeWale may offer a limited number of free gifts to buyers, for a limited period only, over and above the offers from Bike manufacturers and / or their dealers. The quantity and availability period (also referred to as 'promotion period' hereafter) will be displayed prominently along with the offer and gift information on www.bikewale.com.</p>
                    <p>9. These free gifts are being offered solely by BikeWale, and entirely at BikeWale's own discretion, without any additional charges or fees to the buyer.</p>
                    <p>10. In order to qualify for the free gift, the buyer must fulfil the following:</p>
                    <div class="margin-left20 margin-top10">
                        <p>a. Be a legally recognised adult Indian resident, age eighteen (18) years or above as on 01 Dec 2014, and be purchasing the Bike in their individual capacity</p>
                        <p>b. Visit www.bikewale.com and pay the booking amount online against purchase of selected vehicle from BikeWale’s assigned dealer.</p>
                        <p>c. Complete all payment formalities and take delivery of the bike from the same dealership. </p>
                        <p>d. Inform BikeWale through any of the means provided about the completion of the delivery of the bike.</p>
                        
                    </div>
                    <p>11. By virtue of generating an offer code and / or providing BikeWale with Bike booking and / or delivery details, the buyer agrees that s/he is:</p>
                    <div class="margin-left20 margin-top10">
                        <p>a. Confirming his/her participation in this promotion; and</p>
                        <p>b. Actively soliciting contact from BikeWale and / or Bike manufacturers and / or dealers; and</p>
                        <p>c. Expressly consenting for BikeWale to share the information they have provided, in part or in entirety, with Bike manufacturers and / or dealers, for the purpose of being contacted by them to further assist in the Bike buying process; and</p>
                        <p>d. Expressly consenting to receive promotional phone calls, emails and SMS messages from BikeWale, Bike manufacturers and / or dealers; and</p>
                        <p>e. Expressly consenting for BikeWale to take photographs and record videos of the buyer and use their name, photographs, likeness, voice and comments for advertising, promotional or any other purposes on any media worldwide and in any way as per BikeWale's discretion throughout the world in perpetuity without any compensation to the buyer whatsoever; and</p>
                        <p>f. Confirming that, on the request of BikeWale, s/he shall also make arrangements for BikeWale to have access to his / her residence, work place, favourite hangouts, pets etc. and obtain necessary permissions from his / her parents, siblings, friends, colleagues to be photographed, interviewed and to record or take their photographs, videos etc. and use this content in the same manner as described above; and</p>
                        <p>g. Hereby agreeing to fully indemnify BikeWale against any claims for expenses, damages or any other payments of any kind, including but not limited to that arising from his / her actions or omissions or arising from any representations, misrepresentations or concealment of material facts; and</p>
                        <p>h. Expressly consenting that BikeWale may contact the Bike manufacturer and / or dealer to verify the booking and / or delivery details provided by the buyer; and</p>
                        <p>i. Waiving any right to raise disputes and question the process of allocation of gifts</p>
                    </div>
                    <p>12. Upon receiving complete booking and delivery details from the buyer, BikeWale may at its own sole discretion verify the details provided with the Bike manufacturer and / or dealer. The buyer will be eligible for the free gift only if the details can be verified as matching the records of the manufacturer and / or dealer.</p>
                    <p>13. The gifts will be allocated in sequential order at the time of receiving confirmed booking details. Allocation of a gift merely indicates availability of that specific gift for the selected Bike at that specific time, and does not guarantee, assure or otherwise entitle the buyer in any way whatsoever to receive the gift. Allocation of gifts will be done entirely at BikeWale's own sole discretion. BikeWale may change the allocation of gifts at their own sole discretion without notice and without assigning a reason.</p>
                    <p>14. The quantity of gifts available, along with the gift itself, varies by Bike and city. The availability of gifts displayed on www.bikewale.com is indicative in nature. Buyers are strongly advised to check availability of gifts by contacting BikeWale via phone before booking the bike.</p>
                    <p>15. The gift will be despatched to buyers only after the dealer has confirmed delivery of the bike.</p>
                    <p>16. Gifts will be delivered to addresses in India only. In the event that delivery is not possible at certain locations, BikeWale may at its own sole discretion, accept an alternate address for delivery, or arrange for the gift to be made at the nearest convenient location for the buyer to collect.</p>
                    <p>17. Ensuring that the booking and / or delivery information reaches BikeWale in a complete and timely manner is entirely the responsibility of the buyer, and BikeWale, Bike manufacturers, dealers and their employees and contracted staff cannot be held liable for incompleteness of information and / or delays of any nature under any circumstances whatsoever.</p>
                    <p>18. The buyer must retain the offer code, booking confirmation form, invoice of the bike, and delivery papers provided by the dealer, and provide any or all of the same on demand along with necessary identity documents and proof of age. BikeWale may at its own sole discretion declare a buyer ineligible for the free gift in the event the buyer is not able to provide / produce any or all of the documents as required.</p>
                    <p>19. In the event of cancellation of a booking, or if the buyer fails to take delivery of the Bike for any reason, the buyer becomes ineligible for the gift.</p>
                    <p>20. BikeWale's sole decision in all matters pertaining to the free gift, including the choice and value of product, is binding and non-contestable in all respects.</p>
                    <p>21. The buyer accepts and agrees that BikeWale, Bike manufacturers, dealers and other associates of BikeWale, including agencies and third parties contracted by BikeWale, and / or their directors, employees, officers, affiliates or subsidiaries, cannot be held liable for any damage or loss, including but not limited to lost opportunity, lost profit, financial loss, bodily harm, injuries or even death, directly or indirectly, arising out of the use or misuse of the gift, or a defect of any nature in the gift, or out of participating in this promotion in any way whatsoever.</p>
                    <p>22. The buyer specifically agrees not to file in person / through any family member and / or any third party any applications, criminal and/or civil proceedings in any courts or forum in India against BikeWale, Bike manufacturers, dealers and other associates of BikeWale, including agencies and third parties contracted by BikeWale, and/or their directors, employees, officers, affiliates or subsidiaries, and / or their directors, employees, officers, affiliates or subsidiaries to claim any damages or relief in connection with this promotion.</p>
                    <p>23. All gifts mentioned, including the quantity available, are indicative only. Pictures are used for representation purposes only and may not accurately depict the actual gift.</p>
                    <p>24. BikeWale reserves the right to substitute any gift with a suitable alternative or provide gift vouchers of an equivalent value to the buyer, without assigning a reason for the same. Equivalent value of the gift shall be determined solely by BikeWale, irrespective of the market / retail / advertised prices or Maximum Retail Price (MRP) of the product at the time of despatch of the gift. An indicative “gift value” table is provided below.</p>
                    <p>25. Delivery of the product shall be arranged through a third party logistics partner and BikeWale is in no way or manner liable for any damage to the product during delivery.</p>
                    <p>26. Warranty on the gift, if any, will be provided as per the gift manufacturer's terms and directly by the gift manufacturer.</p>
                    <p>27. Gifts cannot be transferred or redeemed / exchanged for cash.</p>
                    <p>28. Income tax, gift tax and / or any other statutory taxes, duties or levies as may be applicable from time to time, arising out of the free gifts, shall be payable entirely by the buyer on his/her own account.</p>
                    <p>29. BikeWale makes no representation or warranties as to the quality, suitability or merchantability of any of the gifts whatsoever, and no claim or request, whatsoever, in this respect shall be entertained.</p>
                    <p>30. Certain gifts may require the buyer to incur additional expenses such as installation expenses or subscription fees or purchasing additional services, etc. The buyer agrees to bear such expenses entirely on their own account.</p>
                    <p>31. Availing of the free gift and offer is purely voluntary. The buyer may also purchase the Bike without availing the free gift and / or the offer.</p>
                    <p>32. For the sake of clarity it is stated that the Bike manufacturer and / or dealer shall not be paid any consideration by BikeWale to display their offers and / or offer free gifts for purchasing bikes from them. Their only consideration will be the opportunity to sell a Bike to potential Bike buyers who may discover their offer on www.bikewale.com.</p>
                    <p>33. Each buyer is eligible for only one free gift under this promotion, irrespective of the number of bikes they purchase.</p>
                    <p>34. This promotion cannot be used in conjunction with any other offer, promotion, gift or discount scheme.</p>
                    <p>35. In case of any dispute, BikeWale's decision will be final and binding and non-contestable. The existence of a dispute, if any, does not constitute a claim against BikeWale.</p>
                    <p>36. This promotion shall be subject to jurisdiction of competent court/s at Mumbai alone.</p>
                    <p>37. Employees of BikeWale and their associate / affiliate companies, and their immediate family members, are not eligible for any free gifts under this promotion.</p>
                    <p>38. This promotion is subject to force majeure circumstances i.e. Act of God or any circumstances beyond the reasonable control of BikeWale.</p>
                    <p>39. Any and all information of the buyers or available with BikeWale may be shared with the government if any authority calls upon BikeWale / manufacturers / dealers to do so, or as may be prescribed under applicable law.</p>
                    <p>40. In any case of any dispute, inconvenience or loss, the buyer agrees to indemnify BikeWale, its representing agencies and contracted third parties without any limitation whatsoever.</p>
                    <p>41. The total joint or individual liability of BikeWale, its representing agencies and contracted third parties, along with Bike manufacturers and dealers, will under no circumstances exceed the value of the free gift the buyer may be eligible for.</p>
                    <p>42. BikeWale reserves the right to modify any and all of the terms and conditions mentioned herein at its own sole discretion, including terminating this promotion, without any notice and without assigning any reason whatsoever, and the buyers agree not to raise any claim due to such modifications and / or termination.</p>
                    <p>By participating in this promotion, the buyer / user agrees to the terms and conditions above in toto.</p>
					</div>
        </div>
        <!-- Terms and condition Popup Ends -->
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <!-- all other js plugins -->
        <!-- #include file="/includes/footerscript_Mobile.aspx" -->
        <script type="text/javascript">
            var bikeName = '<%= BikeName %>';
            var getCityArea = GetGlobalCityArea();
            var areaId = '<%= areaId %>';
            $('#getDealerDetails,#btnBookBike').click(function () {
                var cookieValue = "CityId=" + cityId + "&AreaId=" + areaId + "&PQId=" + pqId + "&VersionId=" + versionId + "&DealerId=" + dealerId;                
                window.location.href = '/m/pricequote/bookingsummary_new.aspx?MPQ=' + Base64.encode(cookieValue);
            });

            var freeInsurance = $("img.insurance-free-icon");
            if (!freeInsurance.length) {
                cityArea = GetGlobalCityArea();
                $("table tr td.text-medium-grey:contains('Insurance')").first().html("Insurance  (<a href='/m/insurance/' style='position: relative; font-size: 12px; margin-top: 1px;' target='_blank' onclick=\"dataLayer.push({ event: 'Bikewale_all', cat: 'Dealer_PQ', act: 'Insurance_Clicked',lab: '<%= String.Format("{0}_{1}_{2}_",objPrice.objMake.MakeName,objPrice.objModel.ModelName,objPrice.objVersion.VersionName)%>" + cityArea + "' });\">Up to 60% off - PolicyBoss </a>)<span style='margin-left: 5px; vertical-align: super; font-size: 9px;'>Ad</span>");
            }

           
            var leadBtnBookNow = $("#leadBtnBookNow,#leadLink"), leadCapturePopup = $("#leadCapturePopup");
            var fullname = $("#getFullName");
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
                    appendHash("dpqPopup");
                    $("div#contactDetailsPopup").show();
                    $("#otpPopup").hide();
                    //$('body').addClass('lock-browser-scroll');
                    //$(".blackOut-window").show();
                    

                    

                });

                $(".leadCapture-close-btn").on("click", function () {
                    leadCapturePopup.hide();
                    //$('body').removeClass('lock-browser-scroll');
                    //$(".blackOut-window").hide();
                    window.history.back();
                });

                $(document).on('keydown', function (e) {
                    if (e.keyCode === 27) {
                        $("#leadCapturePopup .leadCapture-close-btn").click();
                        $("div.termsPopUpCloseBtn").click();
                    }
                });

            });


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
                            "leadSourceId": eval("<%= Convert.ToInt16(Bikewale.Entities.BikeBooking.LeadSourceEnum.DealerPQ_Mobile) %>"),
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
                            "source": 2
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
                            var cookieValue = "CityId=" + cityId + "&AreaId=" + areaId + "&PQId=" + pqId + "&VersionId=" + versionId + "&DealerId=" + dealerId;
                            window.location.href = "/m/pricequote/BikeDealerDetails.aspx?MPQ=" + Base64.encode(cookieValue);
                        }
                        else {
                            $("#contactDetailsPopup").hide();
                            $("#otpPopup").show();
                            var leadMobileVal = mobile.val();
                            $("#otpPopup .lead-mobile-box").find("span.lead-mobile").text(leadMobileVal);
                            otpContainer.removeClass("hide").addClass("show");
                            //detailsSubmitBtn.hide();
                            nameValTrue();
                            hideError(mobile);
                            otpText.val('').removeClass("border-red").siblings("span, div").hide();
                        }
                        setPQUserCookie();
                        dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Dealer_PQ', 'act': 'Lead_Submitted', 'lab': bikeName + '_' + getCityArea });
                    }

                };

                otpBtn.click(function () {
                    $('#processing').show();
                    if (!validateOTP())
                        $('#processing').hide();

                    if (validateOTP() && ValidateUserDetail()) {
                        customerViewModel.generateOTP();
                        if (customerViewModel.IsVerified()) {
                            // $.customizeState();
                            $("#personalInfo").hide();
                            $(".booking-dealer-details").removeClass("hide").addClass("show");
                            $('#processing').hide();

                            detailsSubmitBtn.show();
                            otpText.val('');
                            otpContainer.removeClass("show").addClass("hide");

                            // OTP Success
                            dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'DealerQuotation_Page', 'act': 'Step_1_OTP_Successful_Submit', 'lab': getCityArea });
                            $("#leadCapturePopup .leadCapture-close-btn").click();                            
                            var cookieValue = "CityId=" + cityId + "&AreaId=" + areaId + "&PQId=" + pqId + "&VersionId=" + versionId + "&DealerId=" + dealerId;
                            window.location.href = "/pricequote/BikeDealerDetails.aspx?MPQ=" + Base64.encode(cookieValue);
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
                var a = fullname.val().length;
                if ((/&/).test(fullname.val())) {
                    isValid = false;
                    setError(fullname, 'Invalid name');
                }
                else if (a == 0) {
                    isValid = false;
                    setError(fullname, 'Please enter your name');
                }
                else if (a >= 1) {
                    isValid = true;
                    nameValTrue()
                }
                 return isValid;
            }

            function nameValTrue() {
                hideError(fullname)
                fullname.siblings("div").text('');
            };

            fullname.on("focus", function () {
                hideError(fullname);
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
                    if (validateMobile(getCityArea)) {
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
                var val = fullname.val() + '&' + emailid.val() + '&' + mobile.val();
                SetCookie("_PQUser", val);
            }

            $("#otpPopup .edit-mobile-btn").on("click", function () {
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
            $('#bookNowBtn').on('click', function (e) {
                window.location.href = "/m/pricequote/bookingSummary_new.aspx";
            });
            ko.applyBindings(customerViewModel, $('#leadCapturePopup')[0]);
            // GA Tags
            $("#leadBtnBookNow").on("click", function () {
                dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Dealer_PQ', 'act': 'Get_More_Details_Clicked_Button', 'lab': bikeName + '_' + getCityArea });
            });
            $("#leadLink").on("click", function () {
                dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Dealer_PQ', 'act': 'Get_More_Details_Clicked_Link', 'lab': bikeName + '_' + getCityArea });
            });
            ga_pg_id = "7";

            $('.tnc').on('click', function (e) {
                LoadTerms($(this).attr("id"));
            });

            function LoadTerms(offerId) {
                $("div#termsPopUpContainer").show();
                $(".blackOut-window").show();
                $('#terms').empty();
                if (offerId != 0 && offerId != null) {
                    $('#termspinner').show();
                    $.ajax({
                        type: "GET",
                        url: "/api/Terms/?offerMaskingName=&offerId=" + offerId,
                        dataType: 'json',
                        success: function (response) {
                            if (response != null)
                                $('#terms').html(response);
                        },
                        error: function (request, status, error) {
                            $("div#termsPopUpContainer").hide();
                            $(".blackOut-window").hide();
                        }
                    });
                }
                else {
                    $('#terms').html($("#orig-terms").html());
                }
                $('#termspinner').hide();
            }

            $(".termsPopUpCloseBtn").on('mouseup click', function (e) {
                $("div#termsPopUpContainer").hide();
                $(".blackOut-window").hide();
            });

        </script>

    </form>
</body>
</html>
