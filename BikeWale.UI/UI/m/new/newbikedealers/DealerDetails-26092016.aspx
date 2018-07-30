<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.DealerDetails" EnableViewState="false" %>

<%@ Register TagPrefix="BW" TagName="MPopupWidget" Src="/m/controls/MPopupWidget.ascx" %>
<!DOCTYPE html>
<html>
<head>
    <%
        title = string.Format("{0} in {1} {2} - BikeWale", dealerName, dealerArea, dealerCity);
        description = string.Format("{0} is a {1} dealer in {2} {3}. Check out the on road price, offers and benefits of buying bike from {0}.",
            dealerName, makeName, dealerArea, dealerCity);
        keywords = string.Format("{0}, {0} in {1} {2}, {3} dealer in {1} {2}", dealerName, dealerArea, dealerCity, makeName);
    %>
    <!-- #include file="/includes/headscript_mobile.aspx" -->
    <style type="text/css">
        #dealerHeader{background:#313131;color:#fff;width:100%;height:48px;position:fixed;overflow:hidden;z-index:2;}.dealer-back-btn {padding:12px 15px;cursor:pointer;}.fa-arrow-back{width:12px;height:20px;background-position:-63px -162px;}.dealer-header-text { width:80%; text-align:left; text-overflow:ellipsis; white-space:nowrap; overflow:hidden; }.padding-top48 { padding-top:48px; }.box-shadow { -webkit-box-shadow:0 0 1px #e2e2e2; -moz-box-shadow:0 0 1px #e2e2e2; box-shadow:0 0 1px #e2e2e2; }.text-pure-black { color:#1a1a1a; }.featured-tag {position:relative;left:-20px;top:-5px;width:100px;background:#4d5057;z-index:1; line-height:28px; }.featured-tag:after {content:'';width:12px; height:28px;background: url(http://imgd1.aeplcdn.com/0x0/bw/static/sprites/m/upcoming-ribbon.png?v=9Sep2016) no-repeat right top;position:absolute;left:98px;}.dealer-details-section {line-height:1.8;}.tel-sm-grey-icon{width: 12px;height: 15px;background-position: -86px -323px;position: relative;top: 2px;}.mail-grey-icon{width: 15px;height: 9px;background-position: -19px -437px;}.text-default { color:#4d5057; }.get-direction-icon, .sendto-phone-icon { width:12px; height:10px; }.get-direction-icon { background-position: -31px -421px; }.sendto-phone-icon { background-position: -49px -421px; }.divider-left { border-left: 1px solid #82888b; padding-left:7px; margin-left:7px; }.border-light-bottom { border-bottom:1px solid #f1f1f1; }.tel-grey-icon { position:relative;top:2px; }.float-button.float-fixed {position: fixed;bottom: 0;z-index: 8;left: 0;right: 0;}.float-button {background-color: #f5f5f5; padding: 0px 10px 10px 10px;}#bikesAvailableList .front {margin-top:20px; height: auto;border-radius: 0;box-shadow: none;-moz-box-shadow: none;-ms-box-shadow: none;border: 0 none;}#bikesAvailableList .bikeDescWrapper { padding:0; }#bikesAvailableList .imageWrapper { height:143px; }#bikesAvailableList .imageWrapper img { width: 254px;height: 143px;}.btn-sm {padding:8px 14px;}
        #leadCapturePopup .errorIcon, #leadCapturePopup .errorText, #otpPopup, .update-mobile-box, .otp-notify-text {display: none;}.mobile-prefix {position: absolute;padding: 10px 13px 13px;color: #999;z-index: 2;}#getMobile {padding: 9px 40px;}.otp-icon { width:29px; height:29px; background-position: -109px -177px; }.edit-blue-icon { width:20px; height:20px; background-position: -114px -122px; }#otpPopup .otp-box p.resend-otp-btn { color:#0288d1; cursor:pointer; font-size:14px; }
        #brandSearchBar { padding:0; background: #f5f5f5; z-index: 11; position: fixed; left: 100%; top: 0; overflow-y: scroll; width: 100%; height: 100%;}#brandSearchBar li { border-top: 1px solid #ccc; font-size: 14px; padding: 15px 10px; color: #333333; cursor: pointer;}#brandSearchBar li:hover { background: #ededed; }.dealer-brand-wrapper { display:none; }.bwm-dealer-brand-box .back-arrow-box { height: 30px; width: 40px; position: absolute; top: 5px; z-index: 11; cursor: pointer; }.bwm-dealer-brand-box span.back-long-arrow-left {position: absolute;top: 7px;left: 10px;}.bwm-dealer-brand-box .back-arrow-box {position: absolute;left: 5px;}.bwm-dealer-brand-box .form-control {padding: 10px 50px;}.activeBrand {font-weight: bold;background-color: #ddd;}.dealer-search-brand-form { padding:10px 25px 10px 10px; text-align:left; cursor:pointer; background: #fff url(http://imgd1.aeplcdn.com/0x0/bw/static/design15/old-images/m/dropArrowBg.png?v1=19082015) no-repeat 96% 50%; text-align:left; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; border-radius:2px;border: 1px solid #ccc; }.dealer-search-brand { width:100%; height: 40px;}.border-red { border:1px solid #f00 !important; }
        

        .dealer-back-btn {
            padding: 12px 15px;
            cursor: pointer;
        }

        .fa-arrow-back {
            width: 12px;
            height: 20px;
            background-position: -63px -162px;
        }

        .dealer-header-text {
            width: 80%;
            text-align: left;
            text-overflow: ellipsis;
            white-space: nowrap;
            overflow: hidden;
        }

        .padding-top48 {
            padding-top: 48px;
        }

        .box-shadow {
            -webkit-box-shadow: 0 0 1px #e2e2e2;
            -moz-box-shadow: 0 0 1px #e2e2e2;
            box-shadow: 0 0 1px #e2e2e2;
        }

        .text-pure-black {
            color: #1a1a1a;
        }

        .featured-tag {
            position: relative;
            left: -20px;
            top: -5px;
            width: 100px;
            background: #4d5057;
            z-index: 1;
            line-height: 28px;
        }

            .featured-tag:after {
                content: '';
                width: 12px;
                height: 28px;
                background: url(http://imgd1.aeplcdn.com/0x0/bw/static/sprites/m/upcoming-ribbon.png?v=9Sep2016) no-repeat right top;
                position: absolute;
                left: 98px;
            }

        .dealer-details-section {
            line-height: 1.8;
        }

        .tel-sm-grey-icon {
            width: 12px;
            height: 15px;
            background-position: -86px -323px;
            position: relative;
            top: 2px;
        }

        .mail-grey-icon {
            width: 15px;
            height: 9px;
            background-position: -19px -437px;
        }

        .text-default {
            color: #4d5057;
        }

        .get-direction-icon, .sendto-phone-icon {
            width: 12px;
            height: 10px;
        }

        .get-direction-icon {
            background-position: -31px -421px;
        }

        .sendto-phone-icon {
            background-position: -49px -421px;
        }

        .divider-left {
            border-left: 1px solid #82888b;
            padding-left: 7px;
            margin-left: 7px;
        }

        .border-light-bottom {
            border-bottom: 1px solid #f1f1f1;
        }

        .tel-grey-icon {
            position: relative;
            top: 2px;
        }

        .float-button.float-fixed {
            position: fixed;
            bottom: 0;
            z-index: 8;
            left: 0;
            right: 0;
        }

        .float-button {
            background-color: #f5f5f5;
            padding: 0px 10px 10px 10px;
        }

        #bikesAvailableList .front {
            margin-top: 20px;
            height: auto;
            border-radius: 0;
            box-shadow: none;
            -moz-box-shadow: none;
            -ms-box-shadow: none;
            border: 0 none;
        }

        #bikesAvailableList .bikeDescWrapper {
            padding: 0;
        }

        #bikesAvailableList .imageWrapper {
            height: 143px;
        }

            #bikesAvailableList .imageWrapper img {
                width: 254px;
                height: 143px;
            }
            #getUserLocation {position:absolute;cursor:pointer}
            .thankyou-icon {width:24px; height:28px; background-position: -138px -394px;}
            .crosshair-icon {width: 20px;height: 20px;background: url(http://imgd1.aeplcdn.com/0x0/bw/static/design15/detect-location-icon.jpg) no-repeat;}
            .dealership-loc-icon {width: 12px;height: 15px;background-position: -38px -435px;position: relative;top: 4px;}
            .vertical-top { display:inline-block;vertical-align:top; }
    </style>
    <script src="http://maps.googleapis.com/maps/api/js?key=<%= Bikewale.Utility.BWConfiguration.Instance.GoogleMapApiKey %>&libraries=places"></script>
</head>
<body class="bg-light-grey">
    <form runat="server">
        <header id="dealerHeader">
            <div class="leftfloat dealer-back-btn">
                <a href="javascript:history.back()"><span class="bwmsprite fa-arrow-back"></span></a>
            </div>
            <div class="dealer-header-text leftfloat margin-top10 font18"><%= dealerName %></div>
            <div class="clear"></div>
        </header>
        <!--Dealer Details section-->
        <section class="container bg-white padding-top48">
            <div id="dealerDetailsCard" class="padding-top20 padding-right20 padding-left20 font14">
                <%if (dealerDetails.DealerType == (int)(Bikewale.Entities.PriceQuote.DealerPackageTypes.Premium) || dealerDetails.DealerType == (int)(Bikewale.Entities.PriceQuote.DealerPackageTypes.Deluxe))
                  { %>
                <div class="featured-tag text-white text-center margin-bottom10">
                    Featured
                </div>
                <%} %>
                <h1 class="font18 text-pure-black margin-bottom5"><%= dealerDetails.Name %></h1>
                <div class="dealer-details-section text-light-grey padding-bottom15 border-light-bottom">
                    <p class="margin-bottom5"><span class="bwmsprite dealership-loc-icon vertical-top margin-right5"></span><%= dealerDetails.Address %></p>
                    <% if (!string.IsNullOrEmpty(dealerDetails.EMail))
                       { %>
                    <div class="margin-bottom5">
                        <a href="mailto:<%= dealerDetails.EMail %>" class="text-light-grey"><span class="bwmsprite mail-grey-icon"></span><%= dealerDetails.EMail %></a>
                    </div>
                    <% } if (!string.IsNullOrEmpty(dealerDetails.MaskingNumber))
                       { %>
                    <div class="margin-bottom5">
                        <a href="tel:<%= dealerDetails.MaskingNumber %>" class="text-default font16 text-bold maskingNumber"><span class="bwmsprite tel-sm-grey-icon"></span><%= dealerDetails.MaskingNumber %></a>
                    </div>
                    <% } if (!string.IsNullOrEmpty(dealerDetails.WorkingHours))
                       { %>
                    <p>
                        Working hours:<br />
                        <%= dealerDetails.WorkingHours %>
                    </p>
                    <%} %>
                    <a id="anchorGetDir" href="http://maps.google.com/maps?z=12&t=m&q=loc:<%= dealerLat %>,<%= dealerLong %>" target="_blank"><span class="bwmsprite get-direction-icon margin-right5"></span>Get directions</a>
                    <%--<a href="" class="divider-left"><span class="bwmsprite sendto-phone-icon margin-right5"></span>Send to phone</a>--%>
                </div>
                <div class="padding-top15 padding-bottom20 border-light-bottom">
                    <h3 class="font14 margin-bottom15">Get commute distance and time:</h3>
                    <div class="form-control-box">
                        <input id="locationSearch" type="text" class="form-control padding-right50" placeholder="Enter your location" />
                        <span id="getUserLocation" class="crosshair-icon fa-stack font12 position-abt pos-right20 pos-top10 text-grey"></span>
                    </div>
                    <div class="location-details padding-top10 padding-bottom10 leftfloat hide">
                        <span class="fa fa-clock-o"></span>&nbsp;Time : <span id="commuteDuration"></span>&nbsp; &nbsp;
                        <span class="fa fa-road"></span>&nbsp;Distance : <span id="commuteDistance"></span>
                    </div>
                    <div class="clear"></div>
                    <div id="commuteResults"></div>
                </div>
            </div>
            <div class="grid-12 float-button clearfix float-fixed">
                <div class="show padding-top10">
                    <div class="grid-6 alpha omega">
                        <a id="calldealer" class="btn btn-white btn-full-width btn-sm rightfloat text-bold text-default font14" href="tel:<%= dealerDetails.MaskingNumber %>"><span class="bwmsprite tel-grey-icon margin-right5"></span>Call dealer</a>
                    </div>
                    <div class="grid-6 alpha omega padding-left10">
                        <a id="getAssistance" leadSourceId="21" class="btn btn-orange btn-full-width btn-sm rightfloat font14" href="javascript:void(0);">Get offers</a>
                    </div>
                </div>
            </div>
        </section>
        <!--Dealer Deatail section end and models section start.-->
        <!-- Lead Capture pop up start  -->
        <div id="leadCapturePopup" class="bw-popup bwm-fullscreen-popup contact-details hide">
            <div class="popup-inner-container text-center">
                <div class="bwmsprite close-btn leadCapture-close-btn rightfloat"></div>
                <div id="contactDetailsPopup">
                    <h2 class="margin-top10 margin-bottom10">Provide contact details</h2>
                    <p class="text-light-grey margin-bottom10">Dealership will get back to you with offers</p>

                    <div class="personal-info-form-container">
                        <div class="dealer-search-brand form-control-box">
                            <div class="dealer-search-brand-form"><span>Select a bike</span></div>
                            <span class="bwmsprite error-icon errorIcon"></span>
                            <div class="bw-blackbg-tooltip errorText"></div>
                        </div>
                        <div class="form-control-box margin-top20">
                            <input type="text" class="form-control get-first-name" placeholder="Your name" id="getFullName" data-bind="textInput: fullName">
                            <span class="bwmsprite error-icon errorIcon"></span>
                            <div class="bw-blackbg-tooltip errorText"></div>
                        </div>
                        <div class="form-control-box margin-top20">
                            <input type="text" class="form-control get-email-id" placeholder="Email address" id="getEmailID" data-bind="textInput: emailId">
                            <span class="bwmsprite error-icon errorIcon"></span>
                            <div class="bw-blackbg-tooltip errorText"></div>
                        </div>
                        <div class="form-control-box margin-top20">
                            <p class="mobile-prefix">+91</p>
                            <input type="text" class="form-control get-mobile-no" maxlength="10" placeholder="Mobile no." id="getMobile" data-bind="textInput: mobileNo">
                            <span class="bwmsprite error-icon errorIcon"></span>
                            <div class="bw-blackbg-tooltip errorText"></div>
                        </div>                        
                        <div class="clear"></div>
                        <a class="btn btn-full-width btn-orange margin-top20" id="user-details-submit-btn" data-bind="event: { click: submitLead }">Submit</a>
                    </div>

                    <div id="brandSearchBar">
                        <div class="dealer-brand-wrapper bwm-dealer-brand-box form-control-box text-left">
                            <div class="user-input-box">
                                <span class="back-arrow-box"><span class="bwmsprite back-long-arrow-left"></span></span>
                                <input class="form-control" type="text" id="assistanceBrandInput" placeholder="Select a bike" />
                            </div>
                            
                            <ul id="sliderBrandList" class="slider-brand-list margin-top40">
                                <asp:Repeater ID="rptModelList" runat="server">
                                    <ItemTemplate>
                                        <li modelId="<%# DataBinder.Eval(Container.DataItem, "objModel.ModelId") %>" versionId="<%# DataBinder.Eval(Container.DataItem, "objVersion.VersionId") %>" bikeName="<%# DataBinder.Eval(Container.DataItem, "BikeName") + "_" + DataBinder.Eval(Container.DataItem, "objVersion.VersionName") %>"><%# DataBinder.Eval(Container.DataItem, "BikeName") %></li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                         </div>
                    </div>

                </div>
                 <!-- thank you message starts here -->
                <div id="notify-response" class="hide margin-top10 content-inner-block-20 text-center">
                    <div class="icon-outer-container rounded-corner50percent">
                        <div class="icon-inner-container rounded-corner50percent">
                            <span class="bwmsprite thankyou-icon margin-top25"></span>
                        </div>
                    </div>
                    <p class="font18 text-bold margin-top20 margin-bottom20">Thank you <span class="notify-leadUser"></span></p>
                    <p class="font16 margin-bottom40">Dealer would get back to you shortly with additional information.</p>
                    <input type="button" id="notifyOkayBtn" class="btn btn-orange" value="Okay" />
                </div>
				<!-- thank you message ends here -->
                <div id="otpPopup">
                    <p class="font18 margin-bottom5">Verify your mobile number</p>
                    <p class="text-light-grey margin-bottom5">We have sent OTP on your mobile. Please enter that OTP in the box provided below:</p>
                    <div>
                        <div class="lead-mobile-box lead-otp-box-container margin-bottom10 font22">
                            <span class="bwmsprite tel-grey-icon"></span>
                            <span class="text-light-grey font24">+91</span>
                            <span class="lead-mobile font24"></span>
                            <span class="bwmsprite edit-blue-icon edit-mobile-btn"></span>
                        </div>
                        <div class="otp-box lead-otp-box-container">
                            <div class="form-control-box margin-bottom10">
                                <input type="text" class="form-control" placeholder="Enter your OTP" id="getOTP" maxlength="5" data-bind="value: otpCode"/>
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
                                <input type="text" class="form-control padding-left40" placeholder="Mobile no." maxlength="10" id="getUpdatedMobile" data-bind="value: mobileNo"  />
                                <span class="bwmsprite error-icon errorIcon"></span>
                                <div class="bw-blackbg-tooltip errorText"></div>
                            </div>
                            <input type="button" class="btn btn-orange margin-top20" value="Send OTP" id="generateNewOTP" data-bind="event: { click: submitLead }" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- Lead Capture pop up end  -->
        <%if (dealerBikesCount > 0)
          { %>
        <section class="container bg-white margin-bottom20">
            <div class="padding-right20 padding-bottom10 padding-left20 box-shadow font14">
                <div class="padding-top15">
                    <h3 class="font14 margin-bottom15">Models available with the dealer:</h3>
                    <div id="bikesAvailableList">
                        <asp:Repeater ID="rptModels" runat="server">
                            <ItemTemplate>
                                <div class="front">
                                    <div class="contentWrapper">
                                        <div class="imageWrapper margin-bottom20">
                                            <a class="modelurl">
                                                <img class="lazy"
                                                    data-original="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImagePath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._310x174) %>"
                                                    title="<%# DataBinder.Eval(Container.DataItem, "BikeName") %>"
                                                    alt="<%# DataBinder.Eval(Container.DataItem, "BikeName") %>" src="http://imgd3.aeplcdn.com/0x0/bw/static/sprites/m/circleloader.gif">
                                            </a>
                                        </div>
                                        <div class="bikeDescWrapper">
                                            <h3 class="margin-bottom5 text-black"><%# DataBinder.Eval(Container.DataItem, "BikeName") %></h3>
                                            <div class="margin-bottom5 text-default text-bold">
                                                <span class="bwmsprite inr-sm-icon"></span>
                                                <span class="font18"><%# Bikewale.Utility.Format.FormatPrice(Convert.ToString(DataBinder.Eval(Container.DataItem, "VersionPrice"))) %><span class="font16"> Onwards</span></span>
                                            </div>
                                            <div class="font14 text-light-grey">
                                                <%# Bikewale.Utility.FormatMinSpecs.GetMinSpecs(Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.Displacement")),Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.FuelEfficiencyOverall")),Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.MaxPower"))) %>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>
        </section>
        <%} %>
        <!--Dealer models section start.-->
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <!-- #include file="/includes/footerscript_Mobile.aspx" -->
         <script type="text/javascript">
             var versionId, dealerId = "<%= dealerId %>", cityId = "<%= cityId %>", clientIP = "<%= Bikewale.Common.CommonOpn.GetClientIP()%>";                                              
             var dealerLat = "<%= dealerLat %>", dealerLong = "<%= dealerLong%>";
             var pqSource = "<%= Convert.ToUInt16(Bikewale.Entities.PriceQuote.PQSourceEnum.Mobile_DealerLocator_Detail) %>";
             var bodHt, footerHt, scrollPosition, leadSourceId;                         
             var googleMapAPIKey = "<%= Bikewale.Utility.BWConfiguration.Instance.GoogleMapApiKey%>";
             var makeName = "<%= makeName%>";
             var cityArea = "<%= dealerCity + "_" + dealerArea%>";

            $(window).scroll(function () {
                bodHt = $('body').height();
                footerHt = $('footer').height();
                scrollPosition = $(this).scrollTop();
                if ($('.float-button').hasClass('float-fixed')) {
                    if (scrollPosition + $(window).height() > (bodHt - footerHt))
                        $('.float-button').removeClass('float-fixed');
                }
                if (scrollPosition + $(window).height() + 60 < (bodHt - footerHt))
                    $('.float-button').addClass('float-fixed');
            });
           
            $("#getAssistance").on('click', function () {
                leadSourceId = $(this).attr("leadSourceId");
                $("#leadCapturePopup").show();
                appendHash("assistancePopup");
                $("div#contactDetailsPopup").show();
                $("#otpPopup").hide();
                triggerGA("Dealer_Locator_Detail", "Get_Offers_Clicked", makeName + "_" + cityArea);
            });

            /*need needmodification*/   
            var assistancePopupClose = function () {
                $("#leadCapturePopup").hide();
                $("#notify-response").hide();
            };

            $("#user-details-submit-btn").on("click", function () {
                if (validateUserDetail()) {
                    $("#contactDetailsPopup").hide();
                    $("#otpPopup").show();
                    $(".lead-mobile").text($("#getMobile").val());
                }
            });

            var validateModel = function () {
                var isValid = true,
                    model = $('.dealer-search-brand-form');

                if (!model.hasClass('selection-done')) {
                    setError(model, 'Please select a bike');
                    isValid = false;
                }
                else if (model.hasClass('selection-done')) {
                    hideError(model);
                    isValid = true;
                }
                return isValid;
            };
            //otp form
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
                    setError(mobileNo, "Please enter your Mobile number");
                    isValid = false;
                }
                else if (!reMobile.test(mobileVal) && isValid) {
                    setError(mobileNo, "Mobile number should be 10 digits");
                    isValid = false;
                }
                else
                    hideError(mobileNo)
                return isValid;
            };
            var otpText = $("#getOTP"),
                otpBtn = $("#otp-submit-btn");
            var otpVal = function (msg) {
                otpText.addClass("border-red");
                otpText.siblings("span, div").show();
                otpText.siblings("div").text(msg);
            };
            function validateOTP() {
                var retVal = true;
                var isNumber = /^[0-9]{5}$/;
                var cwiCode = otpText.val();
                if (cwiCode == "") {
                    retVal = false;
                    otpVal("Please enter your Verification Code");
                }
                else {
                    if (isNaN(cwiCode)) {
                        retVal = false;
                        otpVal("Verification code should be numeric");
                    }
                    else if (cwiCode.length != 5) {
                        retVal = false;
                        otpVal("Verification code should be of 5 digits");
                    }
                }
                return retVal;
            }
            var brandSearchBar = $("#brandSearchBar"),
                dealerSearchBrand = $(".dealer-search-brand"),
                dealerSearchBrandForm = $(".dealer-search-brand-form");
            dealerSearchBrand.on('click', function () {
                $('.dealer-brand-wrapper').show();
                brandSearchBar.addClass('open').animate({ 'left': '0px' }, 500);
                brandSearchBar.find(".user-input-box").animate({ 'left': '0px' }, 500);
                $("#assistanceBrandInput").focus();
            });
            function setSelectedElement(_self, selectedElement) {
                _self.parent().prev("input[type='text']").val(selectedElement);
                brandSearchBar.addClass('open').animate({ 'left': '100%' }, 500);
            };
            $(".dealer-brand-wrapper .back-arrow-box").on("click", function () {
                brandSearchBar.removeClass("open").animate({ 'left': '100%' }, 500);
                brandSearchBar.find(".user-input-box").animate({ 'left': '100%' }, 500);
            });

        </script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/dealerDetail.js?<%= staticFileVersion %>"></script>
       
    </form>
</body>
</html>
