<%@ Page Language="C#" Inherits="Bikewale.New.BrowseNewBikeDealerDetails" AutoEventWireup="false" EnableViewState="false" %>

<!DOCTYPE html>
<html>
<head>
    <%
        isAd970x90Shown = false;

        keywords = String.Format("{0} dealers city, Make showrooms  {1}, {1} bike dealers, {0} dealers, {1} bike showrooms, bike dealers, bike showrooms, dealerships", makeName, cityName);
        description = String.Format("{0} bike dealers/showrooms in {1}. Find {0} bike dealer information for more than 200 cities. Dealer information includes full address, phone numbers, email, pin code etc", makeName, cityName);
        title = String.Format("{0} Dealers in {1} city | {0} New bike Showrooms in {1} - BikeWale", makeName, cityName);
        canonical = String.Format("http://www.bikewale.com/new/{0}-dealers/{1}-{2}.html", makeMaskingName, cityId, cityMaskingName);
        alternate = String.Format("http://www.bikewale.com/m/new/{0}-dealers/{1}-{2}.html", makeMaskingName, cityId, cityMaskingName);
        AdId = "1395986297721";
        AdPath = "/1017752/BikeWale_New_";
        isAd970x90Shown = false;
    %>
    <!-- #include file="/includes/headscript.aspx" -->
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/dealerlisting.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
    <script src="http://maps.googleapis.com/maps/api/js?key=AIzaSyC9JjTQyUpYSQMKBsYi5fQQwv_qRuP-k-s&libraries=places"></script>
    <style>
        .popup-btn-progress-wrapper{width:138px;margin:0 auto}.popup-otp-progress-wrapper{width:180px;margin:0 auto}.popup-btn-progress-wrapper .btn,.popup-otp-progress-wrapper .btn{width:100%}.progress-bar{width:0;height:4px;background:#16A085;bottom:0;left:0;border-radius:2px}.btn-loader{background-color:#822821}.btnSpinner{right:22px;top:10px;z-index:9;background:#fff}#BWloader{text-align:center;position:relative;font-size:16px;margin-bottom:20px;bottom:0;width:100%}
        #getUserLocation {position:absolute;cursor:pointer;font-size:9px;}
        .thankyou-icon {width:52px; height:58px; background-position: -165px -436px; }
    </style>
</head>
<body class="bg-light-grey padding-top50">
    <form runat="server">
        <!-- #include file="/includes/headBW.aspx" -->
        <section>
            <div class="opacity0 grid-12 padding-right20 padding-left20">
                <div class="breadcrumb">
                    <!-- breadcrumb code starts here -->
                    <ul>
                        <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                            <a itemprop="url" href="/"><span itemprop="title">Home</span></a>
                        </li>
                        <li class="fwd-arrow">&rsaquo;</li>
                        <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                            <a itemprop="url" href="/new/"><span itemprop="title">New Bikes</span></a>
                        </li>
                        <li class="fwd-arrow">&rsaquo;</li>
                        <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                            <a itemprop="url" href="/new/locate-dealers/"><span itemprop="title">New Bike Dealer</span></a>
                        </li>
                        <li class="fwd-arrow">&rsaquo;</li>
                        <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                            <a itemprop="url" href="/new/<%=makeMaskingName %>-dealers/"><span itemprop="title"><%=makeName%> Dealers</span></a>
                        </li>
                        <li class="fwd-arrow">&rsaquo;</li>
                        <li class="current"><strong><%=makeName%> Dealers in <%=cityName%></strong></li>
                    </ul>
                    <div class="clear"></div>
                </div>
            </div>
            <div class="clear"></div>
        </section>

        <section>
            <div class="dealer-filter-dropdown grid-11 text-center">
                <div id="locateDealerFilter" class="box-light-shadow bg-white content-inner-block-12">
                    <p class="font16 leftfloat margin-top8 margin-right20">Locate dealers:</p>
                    <div class="leftfloat margin-right10">
                        <div class="brand-city-filter form-control-box">
                            <select id="ddlMakes" class="form-control  chosen-select">
                                <asp:Repeater ID="rptMakes" runat="server">
                                    <ItemTemplate>
                                        <option maskingname="<%# DataBinder.Eval(Container.DataItem,"MaskingName") %>" value="<%# DataBinder.Eval(Container.DataItem,"MakeId") %>" <%# ((DataBinder.Eval(Container.DataItem,"MakeId")).ToString() != makeId.ToString())?string.Empty:"selected" %>><%# DataBinder.Eval(Container.DataItem,"MakeName") %> </option>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </select>
                            <div class="clear"></div>
                        </div>
                    </div>
                    <div class="leftfloat margin-right10">
                        <div class="brand-city-filter form-control-box">
                            <select id="ddlCities" class="form-control  chosen-select">
                                <asp:Repeater ID="rptCities" runat="server">
                                    <ItemTemplate>
                                        <option maskingname="<%# DataBinder.Eval(Container.DataItem,"CityMaskingName") %>" value="<%# DataBinder.Eval(Container.DataItem,"CityId") %>" <%# ((DataBinder.Eval(Container.DataItem,"CityId")).ToString() != cityId.ToString())?string.Empty:"selected" %>><%# DataBinder.Eval(Container.DataItem,"CityName") %></option>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </select>
                            <div class="clear"></div>
                            <span class="bwsprite error-icon errorIcon hide"></span>
                            <div class="bw-blackbg-tooltip errorText hide"></div>
                        </div>
                    </div>
                    <input type="button" id="applyFiltersBtn" class="btn btn-orange leftfloat" value="Apply" />
                    <div class="clear"></div>
                </div>
            </div>
            <div class="clear"></div>
        </section>

        <section>
            <div class="grid-12 alpha omega">
                <div id="dealerListingSidebar" class="bg-white position-abt pos-right0">
                    <div class="dealerSidebarHeading padding-top15 padding-right20 padding-left20">
                        <h1 id="sidebarHeader" class="font16 border-solid-bottom padding-bottom15"><%= makeName %> dealers in <%= cityName %> <span class="font14 text-light-grey">(<%= totalDealers %>)</span></h1>
                    </div>
                    <ul id="dealersList">
                        <asp:Repeater ID="rptDealers" runat="server">
                            <ItemTemplate>
                                <li data-item-type="<%# (DataBinder.Eval(Container.DataItem,"DealerType")) %>" data-item-id="<%# DataBinder.Eval(Container.DataItem,"DealerId") %>" data-item-inquired="false" data-item-number="<%# DataBinder.Eval(Container.DataItem,"MaskingNumber") %>" data-lat="<%# DataBinder.Eval(Container.DataItem,"objArea.Latitude") %>" data-log="<%# DataBinder.Eval(Container.DataItem,"objArea.Longitude") %>" data-address="<%# DataBinder.Eval(Container.DataItem,"Address") %>" data-campid="<%# DataBinder.Eval(Container.DataItem,"CampaignId") %>">
                                    <div class="font14">
                                        <h2 class="font16 margin-bottom10">
                                            <div class="<%# ((DataBinder.Eval(Container.DataItem,"DealerType").ToString() == "3") || (DataBinder.Eval(Container.DataItem,"DealerType").ToString() == "2"))? "" : "hide" %>">
                                                <span class="featured-tag text-white text-center font14 margin-bottom5">Featured
                                                </span>
                                                <span class="dealer-pointer-arrow"></span>
                                            </div>
                                            <a href="javascript:void(0)" class="dealer-sidebar-link text-black text-bold"><%# DataBinder.Eval(Container.DataItem,"Name") %></a>
                                        </h2>
                                        <p class="text-light-grey margin-bottom5"><%# (String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"objArea.AreaName").ToString()))?"":DataBinder.Eval(Container.DataItem,"objArea.AreaName") + "," %> <%# DataBinder.Eval(Container.DataItem,"City") %></p>

                                        <div class="<%# (String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"MaskingNumber").ToString()))?"hide":string.Empty %>">
                                            <p class="text-light-grey margin-bottom5"><span class="bwsprite phone-grey-icon"></span><%# DataBinder.Eval(Container.DataItem,"MaskingNumber") %></p>
                                        </div>

                                        <div class="<%# (String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"Email").ToString()))?"hide":string.Empty %>">
                                            <a href="mailto:<%# DataBinder.Eval(Container.DataItem,"Email") %>" class="text-light-grey">
                                                <span class="bwsprite mail-grey-icon"></span><%# DataBinder.Eval(Container.DataItem,"Email") %></a>
                                        </div>

                                        <div class="<%# ((DataBinder.Eval(Container.DataItem,"DealerType").ToString() == "3") || (DataBinder.Eval(Container.DataItem,"DealerType").ToString() == "2"))? "" : "hide" %>">
                                            <a data-item-id="<%# DataBinder.Eval(Container.DataItem,"DealerId") %>" href="Javascript:void(0)" class="btn btn-white-orange margin-top15 get-assistance-btn">Get assistance</a>
                                        </div>
                                    </div>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                        <%if (areDealersPremium)
                          { %>
                        <li class="dummy-card"></li>
                        <% } %>
                    </ul>
                </div>
                <div id="dealerInfo">
                    <div id="dealerDetailsSliderCard" class="bg-white font14 position-rel">

                        <div class="dealer-slider-close-btn position-abt pos-top20 pos-right20 bwsprite cross-lg-lgt-grey cur-pointer"></div>
                        <div class="padding-top20 padding-right20 padding-left20" data-bind="with: DealerDetails">
                            <p class="featured-tag text-white text-center margin-bottom5">
                                Featured
                            </p>
                            <div class="padding-bottom20 position-rel" id="dealerPersonalInfo">
                                <h3 class="font18 text-dark-black margin-bottom10" data-bind="text: name"></h3>
                                <p class="text-light-grey margin-bottom5" data-bind="visible :address() && address().length > 0,text: address()"></p>
                                <div class="margin-bottom5">
                                    <span class="font16 text-bold margin-right10" data-bind="visible : mobile() && mobile().length > 0"><span class="bwsprite phone-black-icon"></span><span data-bind="    text: mobile()"></span></span>
                                    <a href="#" class="text-light-grey" data-bind="visible : email() && email().length > 0,attr : { href :'mailto:' + email() }"><span class="bwsprite mail-grey-icon"></span><span data-bind="    text: email()"></span></a>
                                </div>

                                <p class="text-light-grey margin-bottom5" data-bind="visible: workingHours && workingHours.length > 0,text : workingHours">Working Hours : </p>
                                <a href="javscript:void(0)" target="_blank" data-bind="attr: { href: 'https://maps.google.com/?saddr=' + $root.CustomerDetails().userSrcLocation() + '&daddr=' + lat() + ',' + lng() + '' }"><span class="bwsprite get-direction-icon"></span>Get directions</a>
                                <%-- <a href="" class="border-dark-left margin-left10 padding-left10"><span class="bwsprite sendto-phone-icon"></span>Send to phone</a>--%>
                            </div>
                            
                            <div id="commute-distance-form" class="padding-top15 margin-bottom15 border-solid-top">
                                <p class="font14 text-bold margin-bottom15">Get commute distance and time:</p>
                                <div class="commute-distance-form">
                                    <div class="leftfloat form-control-box">
                                        <input id="locationSearch" type="text" class="form-control margin-right10" placeholder="Enter your location" />
                                        <span id="getUserLocation" class="fa-stack font12 position-abt pos-right20 pos-top10 text-grey">
                                            <i class="fa fa-crosshairs fa-stack-2x"></i>
                                            <i class="fa fa-circle fa-stack-1x"></i>
                                        </span>
                                        
                                    </div>
                                    <div class="location-details padding-top10 padding-bottom10 leftfloat">
                                        <span class="fa fa-clock-o"></span>&nbsp;Time : <span id="commuteDuration"></span>&nbsp; &nbsp;
                                        <span class="fa fa-road"></span>&nbsp;Distance : <span id="commuteDistance"></span>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                                <div id="commuteResults"></div>
                            </div>

                            <div id="BWloader" style="display: block;"></div>
                        </div>

                        <div id="buyingAssistanceForm" data-bind="with: CustomerDetails" class="border-solid-top content-inner-block-1520  position-rel">
                            <span class="position-abt progress-bar" style="top: 0"></span>
                            <div id="buying-assistance-form">
                                <p class="font14 text-bold margin-bottom15">Get buying assistance from this dealer:</p>
                                <div class="name-email-mobile-box form-control-box leftfloat margin-right20">
                                    <input type="text" class="form-control" placeholder="Name" id="assistanceGetName" data-bind="textInput: fullName" />
                                    <span class="bwsprite error-icon errorIcon"></span>
                                    <div class="bw-blackbg-tooltip errorText"></div>
                                </div>
                                <div class="name-email-mobile-box form-control-box leftfloat margin-right40">
                                    <input type="text" class="form-control" placeholder="Email id" id="assistanceGetEmail" data-bind="textInput: emailId" />
                                    <span class="bwsprite error-icon errorIcon"></span>
                                    <div class="bw-blackbg-tooltip errorText"></div>
                                </div>
                                <div class="name-email-mobile-box form-control-box leftfloat">
                                    <p class="mobile-prefix">+91</p>
                                    <input type="text" class="mobile-box form-control" placeholder="Mobile" maxlength="10" id="assistanceGetMobile" data-bind="textInput: mobileNo" />
                                    <span class="bwsprite error-icon errorIcon"></span>
                                    <div class="bw-blackbg-tooltip errorText"></div>
                                </div>
                                <div class="clear"></div>
                                <div class="margin-top20">
                                    <div class="select-model-box form-control-box leftfloat margin-right40">
                                        <select id="assistGetModel" data-placeholder="Choose a bike model" data-bind=" value: selectedBike, options: bikes, optionsText: 'bike',optionsCaption: 'Select a bike'" class="form-control chosen-select"></select>
                                        <span class="bwsprite error-icon errorIcon"></span>
                                        <div class="bw-blackbg-tooltip errorText"></div>
                                    </div>
                                    <input type="button" class="btn btn-orange btn-md" id="submitAssistanceFormBtn" value="Submit" data-bind="event: { click: submitLead }" />
                                </div>
                                <div class="clear"></div>
                            </div>
                            <div id="dealer-assist-msg" class="hide">
                                <p class="leftfloat font14">Thank you for your interest. <span data-bind="text: dealerName()"></span>&nbsp;will get in touch shortly</p>
                                <span class="rightfloat bwsprite cross-lg-lgt-grey cur-pointer"></span>
                                <div class="clear"></div>
                            </div>
                        </div>
                        <div id="dealerModelwiseBikes" class="border-solid-top" data-bind="visible : DealerBikes.length > 0">
                            <p class="font14 text-bold padding-top20 padding-right20 padding-left20 margin-bottom15"><span data-bind="text : (DealerBikes.length > 1 )?'Models':'Model'"></span>available with the dealer:</p>
                            <ul id="modelsAvailable" data-bind="template: { name: 'dealerBikesTemplate', foreach: DealerBikes }"></ul>

                            <script id="dealerBikesTemplate" type="text/html">
                                <li>
                                    <div class="contentWrapper">
                                        <div class="imageWrapper">
                                            <a href="#" data-bind="attr: {title: bikeName() }">
                                                <img data-bind="attr: { src : imagePath(), title: bikeName(), alt: bikeName() }" />
                                            </a>
                                        </div>
                                        <div class="bikeDescWrapper">
                                            <div class="bikeTitle margin-bottom7">
                                                <h3 class="font16 text-dark-black"><a href="#" title="" data-bind="text: bikeName(),attr: { title: bikeName() }"></a></h3>
                                            </div>
                                            <div class="font16 text-bold margin-bottom5">
                                                <span class="fa fa-rupee"></span>
                                                <span class="font18" data-bind="CurrencyText: bikePrice() "></span><span class="font16">&nbsp;onwards</span>
                                            </div>
                                            <div class="font14 text-light-grey">
                                                <span data-bind="html: displayMinSpec() "></span>
                                            </div>
                                        </div>
                                    </div>
                                </li>
                            </script>
                            <div class="clear"></div>
                        </div>
                    </div>
                    <!-- inquiry capture popup start-->
                    <div id="leadCapturePopup" data-bind="with: CustomerDetails" class="text-center rounded-corner2">
                        <div class="leadCapture-close-btn position-abt pos-top10 pos-right10 bwsprite cross-lg-lgt-grey cur-pointer"></div>
                        <!-- contact details starts here -->
                        <div id="contactDetailsPopup">
                            <div class="icon-outer-container rounded-corner50">
                                <div class="icon-inner-container rounded-corner50">
                                    <span class="bwsprite user-contact-details-icon margin-top25"></span>
                                </div>
                            </div>
                            <p class="font20 margin-top20 margin-bottom10">Provide contact details</p>
                            <p class="text-light-grey margin-bottom20">For you to see more details about this bike, please submit your valid contact details. It will be safe with us.</p>
                            <div class="personal-info-form-container">
                                <div class="form-control-box personal-info-list position-rel">
                                    <div class="placeholder-loading-text position-abt form-control border-solid" style="display: none; height: 40px; border: 1px solid #e2e2e2;">Loading dealer bikes..<span class="fa fa-spinner fa-spin position-abt text-black btnSpinner"></span></div>
                                    <select id="getModelName" data-placeholder="Choose a bike model" data-bind=" value: selectedBike, options: bikes, optionsText: 'bike',optionsCaption: 'Select a bike'" class="form-control chosen-select"></select>
                                    <span class="bwsprite error-icon errorIcon"></span>
                                    <div class="bw-blackbg-tooltip errorText"></div>
                                    <span class="position-abt progress-bar" style="width: 100%; overflow: hidden; display: none;"></span>
                                </div>
                                <div class="form-control-box personal-info-list">
                                    <input type="text" class="form-control get-first-name" placeholder="Full name (mandatory)"
                                        id="getFullName" data-bind="textInput: fullName">
                                    <span class="bwsprite error-icon errorIcon"></span>
                                    <div class="bw-blackbg-tooltip errorText"></div>
                                </div>
                                <div class="form-control-box personal-info-list">
                                    <input type="text" class="form-control get-email-id" placeholder="Email address (mandatory)"
                                        id="getEmailID" data-bind="textInput: emailId">
                                    <span class="bwsprite error-icon errorIcon"></span>
                                    <div class="bw-blackbg-tooltip errorText"></div>
                                </div>
                                <div class="form-control-box personal-info-list">
                                    <p class="mobile-prefix">+91</p>
                                    <input type="text" class="form-control padding-left40 get-mobile-no" placeholder="Mobile no. (mandatory)"
                                        id="getMobile" maxlength="10" data-bind="textInput: mobileNo">
                                    <span class="bwsprite error-icon errorIcon"></span>
                                    <div class="bw-blackbg-tooltip errorText"></div>
                                </div>
                                <div class="clear"></div>
                                <div class="form-control-box personal-info-list text-center">
                                    <div class="popup-btn-progress-wrapper position-rel">
                                        <a class="btn btn-orange margin-top10" id="user-details-submit-btn" data-bind="event: { click: submitLead }">Submit</a>
                                        <span class="position-abt progress-bar btn-loader" style="width: 100%; overflow: hidden; display: none;"></span>
                                    </div>
                                </div>
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
                                    <div class="form-control-box personal-info-list">
                                        <div class="popup-otp-progress-wrapper position-rel">
                                            <input type="button" class="btn btn-orange margin-top20" value="Confirm OTP" id="otp-submit-btn">
                                            <span class="position-abt progress-bar btn-loader" style="width: 100%; overflow: hidden; display: none;"></span>
                                        </div>
                                    </div>
                                </div>
                                <div class="update-mobile-box">
                                    <div class="form-control-box text-left">
                                        <p class="mobile-prefix">+91</p>
                                        <input type="text" class="form-control padding-left40" placeholder="Mobile no." maxlength="10" id="getUpdatedMobile" data-bind="value: mobileNo" />
                                        <span class="bwsprite error-icon errorIcon"></span>
                                        <div class="bw-blackbg-tooltip errorText"></div>
                                    </div>

                                    <div class="form-control-box personal-info-list position-rel">
                                        <input type="button" class="btn btn-orange" value="Send OTP" id="generateNewOTP" data-bind="event: { click: submitLead }" />
                                        <span class="position-abt progress-bar" style="width: 100%; overflow: hidden; display: none;"></span>
                                    </div>
                                </div>
                                
                            </div>
                            <!-- otp ends here -->

                        </div>
                        <div id="dealer-lead-msg" class="hide">
                            <div class="icon-outer-container rounded-corner50">
                                <div class="icon-inner-container rounded-corner50">
                                    <span class="bwsprite thankyou-icon margin-top15"></span>
                                </div>
                            </div>
                            <p class="font18 margin-top25 margin-bottom20">Thank you for providing your details. <span data-bind="text : dealerName()"></span>&nbsp; will get in touch with you soon.</p>

                            <a href="javascript:void(0)" class="btn btn-orange okay-thanks-msg">Okay</a>
                        </div>
                        <!-- inquiry capture popup End-->
                    </div>
                </div>
                <div class="clear"></div>
        </section>
        <section>
            <div class="grid-12 alpha omega">
                <div class="dealer-map-wrapper">
                    <div id="dealerMapWrapper" style="position: fixed; top: 50px; width: 100%; height: 530px;">
                        <div id="dealersMap" style="width: 100%; height: 530px;">
                        </div>
                    </div>
                </div>
            </div>
            <div class="clear"></div>
        </section>

        <!-- #include file="/includes/footerBW.aspx" -->
        <!-- #include file="/includes/footerscript.aspx" -->

        <script type="text/javascript">
            var $ddlCities = $("#ddlCities"), $ddlMakes = $("#ddlMakes"), $ddlModels = $("#getModelName,#assistGetModel");
            var bikeCityId = $("#ddlCities").val();
            var clientIP = "<%= clientIP %>";
            var currentCityName = "<%= cityName %>";
            var pageUrl = "<%= pageUrl%>";
            var key = "dealerCities_";
            lscache.setBucket('DLPage');
            var leadSrcId = eval("<%= (int)(Bikewale.Entities.BikeBooking.LeadSourceEnum.DealerLocator_Desktop) %>");
            var pageSrcId = eval("<%= Bikewale.Utility.BWConfiguration.Instance.SourceId %>");
            lscache.flushExpired();
            $("#applyFiltersBtn").click(function () {
                ddlmakemasking = $("#ddlMakes option:selected").attr("maskingName");
                ddlcityId = $("#ddlCities option:selected").val();
                if(!isNaN(ddlcityId) && ddlcityId != "0")
                {
                    ddlcityMasking = $("#ddlCities option:selected").attr("maskingName");   
                    window.location.href = "/new/" + ddlmakemasking + "-dealers/" + ddlcityId + "-" + ddlcityMasking + ".html";
                }
                else{
                    if($ddlCities.find("option").length < 2)
                        toggleErrorMsg($ddlCities, true ,"No cities available. Choose another brand !" );
                    else
                        toggleErrorMsg($ddlCities, true ,"Choose a city" );
                }

                
            });

            $ddlCities.chosen({ no_results_text: "No matches found!!" });
            $ddlMakes.chosen({ no_results_text: "No matches found!!" });  
            //$ddlModels.chosen({ no_results_text: "No matches found!!", width: "100%" });
            $('div.chosen-container').attr('style', 'width:100%;border:0');
            $("#bookingAreasList_chosen .chosen-single.chosen-default span").text("Please Select City");

            
            
            $ddlMakes.change(function () {
                selMakeId = $ddlMakes.val();
                $ddlCities.empty();
                if (!isNaN(selMakeId) && selMakeId != "0") {
                    if (!checkCacheCityAreas(selMakeId)) {
                        $.ajax({
                            type: "GET",
                            url: "/api/v2/DealerCity/?makeId=" + selMakeId,
                            contentType: "application/json",
                            success: function (data) {
                                lscache.set(key + selMakeId, data.City, 30);
                                setOptions(data.City);
                            },
                            complete: function (xhr) {
                                if (xhr.status == 404 || xhr.status == 204) {
                                    lscache.set(key + selMakeId, null, 30);
                                    setOptions(null);
                                }
                            }
                        });
                    }
                    else {
                        data = lscache.get(key + selMakeId.toString());
                        setOptions(data);
                    }
                }
                else {                    
                    setOptions(null);
                }
            });

            $ddlCities.change(function(){
                toggleErrorMsg($ddlCities, false );
            }) ;

            function checkCacheCityAreas(cityId) {
                bKey = key + cityId;
                if (lscache.get(bKey)) return true;
                else return false;
            }

            function setOptions(optList) {
                toggleErrorMsg($ddlCities, false );
                if (optList != null)
                {
                    $ddlCities.append($('<option>').text(" Select City ").attr({ 'value': "0" }));
                    $.each(optList, function (i, value) {
                        $ddlCities.append($('<option>').text(value.cityName).attr({'value' : value.cityId , 'maskingName' : value.cityMaskingName }));
                    });
                }
                                
                $ddlCities.trigger('chosen:updated');
                $("#ddlCities_chosen .chosen-single.chosen-default span").text("No cities available");
            }

            $(document).on("change",$ddlModels,function(){
                hideError($ddlModels);
            });

        </script>

        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/dealerlisting.js?<%= staticFileVersion %>"></script>
    </form>
</body>
</html>
