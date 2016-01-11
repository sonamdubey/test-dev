<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.BikeBooking.BookingSummary_New" %>

<!DOCTYPE html>
<html>
<head>
    <%
        title = bikeName + " Bookingbooking-sprite buy-icon customize-icon-grey Summary";
        description = "Authorise dealer price details of a bike " + bikeName;
        keywords = bikeName + ", price, authorised, dealer,Booking ";
        isAd970x90Shown = false;     
    %>
    <!-- #include file="/includes/headscript.aspx" -->
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/booking.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css">
</head>
<body class="header-fixed-inner">
    <form runat="server">
        <!-- #include file="/includes/headBW.aspx" -->
        <script type="text/javascript">$("#header").find(".leftfloat .navbarBtn").hide();$("#header").find(".rightfloat ").hide();</script>
        <section class="bg-light-grey padding-top10">
            <div class="container">
                <div class="grid-12">
                    <%--<h1 class="font30 text-black margin-top10 margin-bottom10">Avail great offers in 3 simple steps</h1>--%>
                    <div class="clear"></div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section class="container" id="bookingFlow" style="display: none;" data-bind="visible: true">
            <div class="grid-12 margin-bottom20">
                <div class="content-box-shadow content-inner-block-20 rounded-corner2">
                    <div id="bookingTabsContainer" class="margin-bottom15">
                        <div class="horizontal-line position-rel margin-auto"></div>
                        <ul>
                            <li>
                                <div id="bikeSummaryTab" class="bike-book-step leftfloat" data-bind="click: function () { if (CurrentStep() > 1 ) CurrentStep(1); }, css: (CurrentStep() >= 1) ? 'active-tab' : ''">
                                    <p>Personal Details</p>
                                    <div class="booking-tabs-image">
                                        <span class="booking-flow-sprite booking-tab-icon" data-bind="css: (CurrentStep() == 1) ? 'personal-details-icon-selected' : 'booking-tick-blue'"></span>
                                    </div>
                                </div>
                            </li>
                            <li>
                                <div id="deliveryDetailsTab" class="bike-book-step" data-bind="click: function () { if (CurrentStep() > 2 || ActualSteps() > 1) CurrentStep(2); }, css: (CurrentStep() >= 2 || ActualSteps() > 1) ? 'active-tab' : 'disabled-tab'">
                                    <p>Summary</p>
                                    <div class="booking-tabs-image">
                                        <span class="booking-flow-sprite booking-tab-icon " data-bind="css: (CurrentStep() == 2) ? 'summary-icon-selected' : (CurrentStep() > 2 || ActualSteps() > 1) ? 'booking-tick-blue' : 'summary-icon-grey'"></span>
                                    </div>
                                </div>
                            </li>
                            <li>
                                <div id="bikePaymentTab" class="bike-book-step rightfloat" data-bind="click: function () { if ((CurrentStep() < -1) || ActualSteps() < -1) CurrentStep(-1); }, css: (CurrentStep() < -1 || ActualSteps() < -1) ? 'active-tab' : 'disabled-tab'">
                                    <p>Payment</p>
                                    <div class="booking-tabs-image">
                                        <span class="booking-flow-sprite booking-tab-icon " data-bind="css: (CurrentStep() == -1) ? 'payment-icon-selected' : (CurrentStep() < -1 || ActualSteps() < -1) ? 'booking-tick-blue' : 'payment-icon-grey'"></span>
                                    </div>
                                </div>
                            </li>
                        </ul>
                    </div>
                     
                    <div id="deliveryDetails" data-bind="visible: CurrentStep() == 1" style="display: block">
                        <div class="contact-offers-container border-light-bottom padding-bottom20">
                            <div class="grid-6 alpha" data-bind="with : Customer">
                                <h3 class="padding-bottom10 margin-right10 border-light-bottom"><span class="bwsprite personal-details-icon margin-right5"></span>Personal details:</h3>
                                <div class="person-details-form-wrapper">
                                    <div class="form-control-box margin-top20 margin-bottom20">
                                        <input type="text" class="form-control" placeholder="Name" id="getLeadName" data-bind="textInput : Name">
                                        <span class="bwsprite error-icon errorIcon" style="display: none;"></span>
                                        <div class="bw-blackbg-tooltip errorText" style="display: none;">Please enter your name</div>
                                    </div>
                                    <div class="form-control-box email-box margin-right20 leftfloat">
                                        <input type="text" class="form-control" placeholder="Email id" id="getEmailID" data-bind="textInput : EmailId">
                                        <span class="bwsprite error-icon errorIcon" style="display: none;"></span>
                                        <div class="bw-blackbg-tooltip errorText" style="display: none;">Please enter your email</div>
                                    </div>
                                    <div class="form-control-box mobile-box leftfloat">
                                        <span class="mobile-prefix">+91</span>
                                        <input type="text" class="form-control padding-left40" placeholder="Mobile no." maxlength="10" id="getMobile" data-bind="textInput : MobileNo" />
                                        <span class="bwsprite error-icon errorIcon" style="display: none;"></span>
                                        <div class="bw-blackbg-tooltip errorText" style="display: none;">Please enter your mobile no.</div>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                            </div>
                            <div class="grid-6 omega border-light-left contact-details-container">
                                <% if (isOfferAvailable)
                                   { %>
                                <h3 class="padding-left5 padding-bottom10 margin-left10 border-light-bottom" data-bind="visible : $root.Bike().bookingAmount() < 1"><span class="fa fa-gift margin-right5 font-24"></span>Book online and avail </h3>
                                <h3 class="padding-left5 padding-bottom10 margin-left10 border-light-bottom" data-bind="visible : $root.Bike().bookingAmount() > 0"><span class="bwsprite offers-icon margin-right5 text-red font-24"></span>Pay <span class="fa fa-rupee" style="font-size: 15px"></span><span class="font16" data-bind="    text : $root.Bike().bookingAmount()"></span> to book your bike and avail</h3>

                                <ul>
                                    <asp:Repeater ID="rptDealerFinalOffers" runat="server">
                                        <ItemTemplate>
                                            <li class="offertxt"> <%#DataBinder.Eval(Container.DataItem,"OfferText") %>
                                               <%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "isOfferTerms")) ==  true ? "<span class='tnc' id='"+ DataBinder.Eval(Container.DataItem, "offerId") +"' ><a class='viewterms'>View T&amp;C</a></span>" : "" %>
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                                <%}
                                   else
                                   {%>
                                <h3 class="padding-left5 padding-bottom10 margin-left10 border-light-bottom" data-bind="visible : $root.Bike().bookingAmount() > 0"><span class="bwsprite offers-icon margin-right5 font-24"></span>Pay <span class="fa fa-rupee" style="font-size: 15px"></span><span class="font16" data-bind="    text : $root.Bike().bookingAmount()"></span> to book your bike</h3>
                                <h3 class="padding-bottom10 padding-left5 margin-right20 border-light-bottom margin-bottom20" data-bind="visible : $root.Bike().bookingAmount() < 1"><span class="fa fa-map-marker text-red margin-right5"></span>Get following details on the bike</h3>
                                <ul>
                                    <li>Offers from the nearest dealers</li>
                                    <li>Waiting period on this bike at the dealership</li>
                                    <li>Nearest dealership from your place</li>
                                </ul>
                                <% } %>
                            </div>
                            <div class="clear"></div>

                            <div id="otpPopup" class="rounded-corner2 text-center" style="display: none;" data-bind="with : Customer">
                                <div class="otpPopup-close-btn position-abt pos-top10 pos-right10 bwsprite cross-lg-lgt-grey cur-pointer"></div>
                                <div class="margin-top10 margin-bottom20"><span class="booking-flow-sprite otp-icon"></span></div>
                                <p class="font18 margin-bottom20">Verify your mobile number</p>
                                <p class="font14 text-light-grey margin-bottom20">We have sent an OTP on the following mobile number. Please enter that OTP in the box provided below:</p>
                                <div>
                                    <div class="lead-mobile-box lead-otp-box-container font22" style="display: block;">
                                        <span class="fa fa-phone"></span>
                                        <span class="text-light-grey font24">+91</span>
                                        <span class="lead-mobile font24" data-bind="text : MobileNo"></span>
                                        <span class="bwsprite edit-blue-icon edit-mobile-btn"></span>
                                    </div>
                                    <div class="otp-box lead-otp-box-container" style="display: block;">
                                        <div class="form-control-box margin-bottom10">
                                            <input type="text" class="form-control" placeholder="Enter your OTP" data-bind="textInput : OtpCode" id="getOTP" />
                                            <span class="bwsprite error-icon errorIcon"></span>
                                            <div class="bw-blackbg-tooltip errorText"></div>
                                        </div>
                                        <p class="resend-otp-btn margin-bottom5" data-bind="visible: (OtpAttempts() < 2), click:regenerateOTP">Resend OTP</p>
                                        <p class=" otp-notify-text text-light-grey font12 " data-bind="visible: (OtpAttempts() >= 2)">
                                            OTP has been already sent to your mobile
                                        </p>
                                        <input type="button" class="btn btn-orange margin-top20" value="Submit OTP" data-bind="click : function(data,event){return validateOTP(data,event);}"  id="processOTP">
                                    </div>
                                    <div class="update-mobile-box" style="display: none;">
                                        <div class="form-control-box text-left">
                                            <span class="mobile-prefix">+91</span>
                                            <input type="text" class="form-control padding-left40" placeholder="Mobile no." maxlength="10" id="getUpdatedMobile" data-bind="textInput : MobileNo" />
                                            <span class="bwsprite error-icon errorIcon" style="display: none;"></span>
                                            <div class="bw-blackbg-tooltip errorText" style="display: none;">Please enter your Mobile Number</div>
                                        </div>
                                        <input type="button" class="btn btn-orange" value="Send OTP" data-bind="click : function(data,event){return $root.verifyCustomer(data,event);}" id="generateNewOTP">
                                    </div>
                                </div>
                            </div>

                        </div>
                        <div class="grid-12 alpha margin-top15 query-number-container">
                            <p class="font14 padding-left5 leftfloat"><span class="bwsprite call-icon inline-block margin-right10"></span>In case of any queries feel free to call us on <span class="text-bold font18">1800 120 8300</span></p>
                           <%-- <input type="button" value="Next" class="btn btn-orange rightfloat" id="bikeSummaryNextBtn" data-bind="click: $root.changedSteps">--%>
                           <input type="button"  value="Next" class="btn btn-orange rightfloat" id="bikeSummaryNextBtn" data-bind="click : function(data,event){return $root.verifyCustomer(data,event);}" />
                        </div>
                        <div class="clear"></div>

                    </div>

                    <div id="bikeSummary" data-bind="with: Bike,visible: CurrentStep() == 2, css: (CurrentStep() > 1) ? 'active-tab' : ''" class="margin-bottom15" style="display: none">
                        <div class="grid-5 text-center bike-image-wrapper">
                            <img data-bind="attr:{src : bikeImageUrl()}">
                        </div>
                        <div class="grid-7 bike-summary-wrapper omega">
                            <p class="font24 bike-title text-black text-bold margin-bottom10" data-bind="text : bikeName()"></p>
                            <div class="variant-color-wrapper padding-bottom10 border-light-bottom">
                                <div class="bike-variant-wrapper grid-6 alpha omega">
                                    <p class="variant-text text-light-grey margin-right10">Version:</p>
                                    <div class="variant-dropdown margin-bottom10">
                                        <div class="select-dropdown rounded-corner2">
                                            <div class="variant-selected-box">
                                                <span class="leftfloat select-btn font14" data-bind="text : selectedVersion().MinSpec.VersionName,attr:{versionId:selectedVersion().MinSpec.VersionId}"></span>
                                                <span class="clear"></span>
                                            </div>
                                            <span class="upDownArrow rightfloat fa fa-angle-down position-abt pos-right10"></span>
                                        </div>
                                        <div class="select-dropdown-list hide">
                                            <ul>
                                                <asp:Repeater ID="rptVarients" runat="server">
                                                    <ItemTemplate>
                                                        <li versionid="<%#DataBinder.Eval(Container.DataItem,"MinSpec.VersionId") %>" data-bind="click: function () { getVersion(<%#DataBinder.Eval(Container.DataItem,"MinSpec.VersionId") %>); $root.ActualSteps(1); }">
                                                            <p><%#DataBinder.Eval(Container.DataItem,"MinSpec.VersionName") %> </p>
                                                        </li>xxx
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </ul>
                                        </div>
                                    </div>
                                    <div class="clear"></div>
                                    <p class="variants-feature font14 text-light-grey" data-bind="text : displayMinSpec"></p>
                                </div>
                                <div class="bike-color-wrapper grid-6 alpha omega">
                                    <p class="colour-text text-light-grey margin-left5 margin-right10">Colour:</p>
                                    <div class="colour-dropdown">
                                        <div class="select-dropdown rounded-corner2">
                                            <div class="colour-selected-box">
                                                <span class="leftfloat select-color-box rounded-corner2" data-bind="style:{'background-color':('#'+selectedColor().HexCode)}"></span>
                                                <span class="leftfloat select-btn font14" data-bind="text:selectedColor().ColorName"></span>
                                                <span class="clear"></span>
                                            </div>
                                            <span class="upDownArrow rightfloat fa fa-angle-down position-abt pos-right10"></span>
                                        </div>
                                        <div class="select-dropdown-list hide">
                                            <ul data-bind="foreach: versionColors">
                                                <li class="text-light-grey" colorid="" data-bind="attr: { colorId: $data.Id},click: function() { $parent.getColor($data);$root.ActualSteps(1);}">
                                                    <span class="select-color-box rounded-corner2" data-bind="style: { 'background-color': '#' + HexCode}"></span>
                                                    <p data-bind="text: ColorName"></p>
                                                </li>
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                                <div class="clear"></div>
                            </div>
                            <div class="onRoad-price-wrapper padding-top10">
                                <ul>
                                    <li>
                                        <p>On road price <span class="viewBreakupText text-blue text-link">(View breakup)</span>:</p>
                                        <div>
                                            <!-- ko if : versionPrice() > 0 -->
                                            <span class="fa fa-rupee"></span>
                                            <span data-bind="CurrencyText: (isInsuranceFree())?(versionPrice() - insuranceAmount()):versionPrice()"></span>
                                            <!-- /ko -->
                                            <!-- ko ifnot : (versionPrice() > 0) -->
                                            <span>Price unavailable</span>
                                            <!-- /ko -->
                                        </div>
                                    </li>
                                    <li>
                                        <p>Booking amount:</p>
                                        <div>
                                            <span class="fa fa-rupee"></span>
                                            <span data-bind="CurrencyText: ($root.Bike().bookingAmount()> 0)?$root.Bike().bookingAmount():'Price unavailable'"></span>
                                        </div>
                                    </li>
                                    <li>
                                        <p>Balance amount payable:</p>
                                        <div>
                                            <span class="fa fa-rupee"></span>
                                            <span data-bind="CurrencyText: remainingAmount()"></span>
                                        </div>
                                    </li>
                                </ul>
                                <div class="clear"></div>
                            </div>


                            <!-- View BreakUp Popup Starts here-->
                            <div class="breakupPopUpContainer content-inner-block-20 hide" id="breakupPopUpContainer">
                                <div class="breakupCloseBtn position-abt pos-top20 pos-right20 bwsprite cross-lg-lgt-grey cur-pointer"></div>
                                <div class="breakup-text-container padding-bottom10">
                                    <h3 class="breakup-header font26 margin-bottom20"><span data-bind="text : bikeName()"></span><span class="font14 text-light-grey ">(On road price breakup)</span></h3>
                                    <table id="model-view-breakup" class="font16">
                                        <tbody>
                                            <!-- ko foreach: versionPriceBreakUp -->
                                            <tr>
                                                <td width="350" class="padding-bottom10" data-bind="text: ItemName"></td>
                                                <td align="right" class="padding-bottom10 text-bold"><span class="fa fa-rupee margin-right5"></span><span data-bind="CurrencyText: Price"></span></td>
                                            </tr>
                                            <!-- /ko -->
                                            <!-- ko if : isInsuranceFree()  && insuranceAmount() > 0 -->
                                            <tr>
                                                <td colspan="2">
                                                    <div class="border-solid-top padding-bottom10"></div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="padding-bottom10">Total on road price</td>
                                                <td align="right" class="padding-bottom10 text-bold" style="text-decoration: line-through;"><span class="fa fa-rupee margin-right5"></span><span data-bind="CurrencyText: versionPrice()"></span></td>
                                            </tr>

                                            <tr>
                                                <td class="padding-bottom10">Minus insurance</td>
                                                <td align="right" class="padding-bottom10 text-bold"><span class="fa fa-rupee margin-right5"></span><span data-bind="CurrencyText: insuranceAmount()"></span></td>
                                            </tr>
                                            <!-- /ko -->
                                            <tr>
                                                <td colspan="2">
                                                    <div class="border-solid-top padding-bottom10"></div>
                                                </td>
                                            </tr>
                                            <tr>

                                                <td class="padding-bottom10 text-bold">Total on road price</td>
                                                <td align="right" class="padding-bottom10 font20 text-bold"><span class="fa fa-rupee margin-right5"></span><span data-bind="CurrencyText: (versionPrice() - insuranceAmount())"></span></td>

                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <div class="border-solid-top padding-bottom10"></div>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    <!-- /ko -->

                                </div>
                            </div>
                            <!--View Breakup popup ends here-->

                        </div>
                        <div class="clear"></div>

                        <div id="dealerDetails" class="border-light-top margin-top10 padding-top20">
                            <div class="contact-offers-container border-light-bottom padding-bottom15">
                                <div class="grid-5 alpha contact-details-container border-light-right">
                                    <h3 class="padding-bottom10 padding-left5 margin-right10 border-light-bottom"><span class="bwsprite contact-icon margin-right5"></span>Dealership details:</h3>
                                    <ul>
                                        <li>
                                            <p class="text-bold">Offers from the nearest dealers</p>
                                            <p class="text-light-grey"><%= dealerAddress %></p>
                                        </li>
                                        <li>
                                            <p class="text-bold">Availability</p>
                                            <p class="text-light-grey" data-bind="visible : $root.Bike().waitingPeriod() > 0">Waiting period of <span class="text-default" data-bind="    text : ($root.Bike().waitingPeriod() == 1)?$root.Bike().waitingPeriod() + ' day' : $root.Bike().waitingPeriod() + ' days'"></span></p>
                                            <p class="text-green text-bold" data-bind="visible : $root.Bike().waitingPeriod() < 1">Now available</p>
                                        </li>
                                    </ul>
                                </div>
                                <div class="grid-7 omega offer-details-container">
                                    <% if (isOfferAvailable)
                                       { %>
                                    <h3 class="padding-left5 padding-bottom10 margin-left10 border-light-bottom" data-bind="visible : $root.Bike().bookingAmount() < 1"><span class="fa fa-gift margin-right5 font-24"></span>Available Offers </h3>
                                    <h3 class="padding-left5 padding-bottom10 margin-left10 border-light-bottom" data-bind="visible : $root.Bike().bookingAmount() > 0"><span class="bwsprite offers-icon margin-right5 text-red font-24"></span>Pay <span class="fa fa-rupee" style="font-size: 15px"></span><span class="font16" data-bind="    text : $root.Bike().bookingAmount()"></span> to book your bike and get:</h3>

                                    <ul>
                                        <asp:Repeater ID="rptDealerOffers" runat="server">
                                            <ItemTemplate>
                                                <li class="offertxt"><%#DataBinder.Eval(Container.DataItem,"OfferText") %>
                                                    offers<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "isOfferTerms")) ==  true ? "<span class='tnc' id='"+ DataBinder.Eval(Container.DataItem, "offerId") +"' ><a>View T&amp;C</a></span>" : "" %>
                                                </li>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </ul>
                                    <%}
                                       else
                                       {%>
                                    <h3 class="padding-left5 padding-bottom10 margin-left10 border-light-bottom" data-bind="visible : $root.Bike().bookingAmount() > 0"><span class="bwsprite offers-icon margin-right5 font-24"></span>Pay <span class="fa fa-rupee" style="font-size: 15px"></span><span class="font16" data-bind="    text : $root.Bike().bookingAmount()"></span> to book your bike</h3>
                                    <h3 class="padding-bottom10 padding-left5 margin-right20 border-light-bottom margin-bottom20" data-bind="visible : $root.Bike().bookingAmount() < 1"><span class="fa fa-map-marker text-red margin-right5"></span>Dealer's Location</h3>
                                    <div class="bikeModel-dealerMap-container margin-left5 margin-top15" style="width: 400px; height: 150px" data-bind="googlemap: { latitude: $root.Dealer().latitude(), longitude: $root.Dealer().longitude() }"></div>
                                    <script src="http://maps.googleapis.com/maps/api/js?key=AIzaSyDY0kkJiTPVd2U7aTOAwhc9ySH6oHxOIYM&sensor=false"></script>
                                    <% } %>
                                </div>
                                <div class="clear"></div>
                            </div>
                            <div class="grid-12 alpha margin-top15 query-number-container">
                                <p class="font14 padding-left5 leftfloat"><span class="bwsprite call-icon inline-block margin-right10"></span>In case of any queries feel free to call us on <span class="text-bold font18">1800 120 8300</span></p>
                                <%--<input type="button" value="Next" class="btn btn-orange rightfloat" id="bikeSummaryNextBtn" data-bind="click: $root.changedSteps">--%>
                                <input type="submit" runat="server" value="Make payment" class="btn btn-orange rightfloat" id="deliveryDetailsNextBtn" data-bind="click : function(data,event){return $root.bookNow(data,event);}">
                            </div>
                            <div class="clear"></div>
                        </div>
                    </div>

                    <div id="bikePayment" data-bind="visible: CurrentStep() > 5" style="display: none">
                    </div>

                </div>
            </div>
            <div class="clear"></div>


        </section>
        <!-- Terms and condition Popup start -->
           <div class="termsPopUpContainer content-inner-block-20 hide" id="termsPopUpContainer">
                                <h3>Terms and Conditions</h3>
                                <div style="vertical-align: middle; text-align: center;" id="termspinner">
                                    <%--<span class="fa fa-spinner fa-spin position-abt text-black bg-white" style="font-size: 50px"></span>--%>
                                    <img src="/images/search-loading.gif" />
                                </div>
                                <div class="termsPopUpCloseBtn position-abt pos-top20 pos-right20 bwsprite cross-lg-lgt-grey cur-pointer"></div>
                                <div id="terms" class="breakup-text-container padding-bottom10 font14">
                                </div>
                            </div>
         <!-- Terms and condition Popup Ends -->
        <section class="container margin-bottom30 lazy content-box-shadow booking-how-it-works" data-original="http://img.aeplcdn.com/bikewaleimg/images/howItWorks.png?<%= staticFileVersion %>">
            <div class="grid-12"></div>
            <div class="clear"></div>
        </section>

        <input id="hdnBikeData" type="hidden" value='<%= new System.Web.Script.Serialization.JavaScriptSerializer().Serialize((objBooking.Varients))%>' />

        <!-- #include file="/includes/footerscript.aspx" -->
        <!-- #include file="/includes/footerBW.aspx" -->

        <script type="text/javascript">
            $(document).ready(function() {
                applyLazyLoad();
            });

            var pqId = '<%= pqId %>'
            var verId = '<%= versionId %>';
            var cityId = '<%= cityId%>';
            var dealerId = '<%= dealerId%>';
            var areaId = '<%= areaId%>';
            //Need to uncomment the below script
            var thisBikename = "<%= this.bikeName %>";
            var clientIP = "<%= clientIP %>"; 
            var pageUrl = "<%= pageUrl %>";
            var abHostUrl = '<%= ConfigurationManager.AppSettings["ABApiHostUrl"]%>';
            
            //select bike version
            var bikeVersionId = "<%= (objCustomer!=null && objCustomer.SelectedVersionId > 0)?objCustomer.SelectedVersionId:versionId %>";
            var versionList = JSON.parse($("#hdnBikeData").val());
            var preSelectedColor = '<%= (objCustomer != null && objCustomer.objColor != null) ? objCustomer.objColor.ColorId : 0 %>';
            var insFree = <%= Convert.ToString(isInsuranceFree).ToLower() %>;          
            var insAmt = '<%= insuranceAmount %>';
            var BikeDealerDetails = function () {
                var self = this;
                self.DealerId = ko.observable(<%= dealerId %>);
                self.PQId = ko.observable(<%= pqId %>);
                self.CityId = ko.observable(<%= cityId %>);  
                self.IsInsuranceFree = ko.observable(insFree);
                self.InsuranceAmount = ko.observable(insAmt);
                self.latitude = ko.observable(<%= latitude %>);
                self.longitude = ko.observable(<%= longitude %>);
            }

        </script>

        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/booking.js?<%= staticFileVersion %>"></script>

        <script type="text/javascript">
            <% if (objCustomer != null && objCustomer.objCustomerBase != null && !String.IsNullOrEmpty(objCustomer.objCustomerBase.CustomerName))
               { %>
            viewModel.Customer().Name('<%= (objCustomer!=null && objCustomer.objCustomerBase!=null &&  !String.IsNullOrEmpty(objCustomer.objCustomerBase.CustomerName))?objCustomer.objCustomerBase.CustomerName:String.Empty %>');
            viewModel.Customer().EmailId('<%= (objCustomer!=null && objCustomer.objCustomerBase!=null &&  !String.IsNullOrEmpty(objCustomer.objCustomerBase.CustomerEmail))?objCustomer.objCustomerBase.CustomerEmail:String.Empty %>');
            viewModel.Customer().MobileNo('<%= (objCustomer!=null && objCustomer.objCustomerBase!=null &&  !String.IsNullOrEmpty(objCustomer.objCustomerBase.CustomerMobile))?objCustomer.objCustomerBase.CustomerMobile:String.Empty %>');
            <% }
               else
               {%>
            var arr = setuserDetails();
            if (arr != null && arr.length > 0) {
                viewModel.Customer().Name(arr[0]);
                viewModel.Customer().EmailId(arr[1]);
                viewModel.Customer().MobileNo(arr[2]);
            }
            <% } %>                    
            var getCityArea = GetGlobalCityArea();
        </script>
    </form>
</body>
</html>
