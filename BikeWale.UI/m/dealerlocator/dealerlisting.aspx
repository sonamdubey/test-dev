<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.m.dealerlocator.dealerlisting" %>
<%@ Register TagPrefix="BW" TagName="MPopupWidget" Src="/m/controls/MPopupWidget.ascx" %>
<!DOCTYPE html>
<html>
<head>
    <!-- #include file="/includes/headscript_mobile.aspx" -->
    <style type="text/css">
        #listingHeader{background:#313131;color:#fff;width:100%;height:48px;position:fixed;overflow:hidden;z-index:2;}
        .listing-back-btn, .listing-filter-btn, #dealersFilterWrapper .filterBackArrow {padding:12px 15px;cursor:pointer;}
        #dealersFilterWrapper .filterTitle { margin-top:10px; }
        .fa-arrow-back{width:12px;height:20px;background-position:-63px -162px;}
        .filter-icon{width: 20px;height: 20px;background-position: -158px -144px;}
        .content-inner-block-1520 { padding:15px 20px; }
        .box-shadow { -webkit-box-shadow:0 0 1px #e2e2e2; -moz-box-shadow:0 0 1px #e2e2e2; box-shadow:0 0 1px #e2e2e2; }
        .text-pure-black { color:#000; }  
        .featured-tag {position:relative;left:-20px;top:-5px;width:100px;background:#4d5057;z-index:1; line-height:28px; }
        .featured-tag:after {content:'';width:12px; height:28px;background: url(http://imgd1.aeplcdn.com/0x0/bw/static/sprites/m/upcoming-ribbon.png?v=15Mar2016) no-repeat right top;position:absolute;left:98px;}
        #dealersList li { border-top:1px solid #e2e2e2; padding-top:18px; margin-top:20px;}
        #dealersList li:first-child { margin-top:0; }
        #dealersList a { display:block; }
        #dealersList a:hover { text-decoration:none; }
        .tel-sm-grey-icon{width: 15px;height: 15px;background-position: 0 -435px;position: relative;top: 2px;}
        .mail-grey-icon{width: 15px;height: 9px;background-position: -19px -437px;}
        .btn-white-orange { background: #fff; color: #82888b; border: 1px solid #82888b;}
        .btn-white-orange:hover { background: #f04031; color: #fff; border: 1px solid #f04031; }
        .btn-white-orange:focus { background: #f04031; color: #fff; border: 1px solid #f04031; }
        /*filter popup*/
        #dealersFilterWrapper{background: #f5f5f5;position: fixed;overflow-x: hidden;overflow-y: scroll;z-index: 10000;top: 0;right: 0;bottom: 0;left: 100%;width: 100%;-webkit-overflow-scrolling: touch;}
        .dealers-brand-city-wrapper{background:#f5f5f5;z-index:11;position:fixed;left:100%;top:0;overflow-y: scroll;width: 100%;height: 100%;}
        .dealers-back-arrow-box{height: 30px;width: 40px;position: absolute;top: 5px;z-index: 11;}
        .dealers-back-arrow-box span {position: absolute;top: 7px;left: 10px;}
        .bwm-brand-city-box .form-control {padding: 10px 50px;}
        .dealers-city-popup-box { display:none; }
        .filter-brand-city-ul li{ border-top:1px solid #ccc;font-size:14px;padding:15px 10px;color:#333333;}  
        .filter-brand-city-ul li:hover{background: #ededed;}
        /*get assistance*/
        #leadCapturePopup .errorIcon, #leadCapturePopup .errorText, #otpPopup, .update-mobile-box, .otp-notify-text {display: none;}
        .mobile-prefix {position: absolute;padding: 10px 13px 13px;color: #999;z-index: 2;}
        #getMobile {padding: 9px 40px;}
        .otp-icon { width:30px; height:40px; background-position: -107px -177px; }
        .edit-blue-icon { width:14px; height:16px; background-position: -114px -121px; }
        #otpPopup .otp-box p.resend-otp-btn { color:#0288d1; cursor:pointer; font-size:14px; }
    </style>
</head>
<body class="bg-light-grey">
    <form runat="server">
        <header id="listingHeader">
            <div class="leftfloat listing-back-btn">
                <a href="javascript:void(0)"><span class="bwmsprite fa-arrow-back"></span></a>
            </div>
            <span class="leftfloat margin-top10 font18">Dealer locator</span>
            <div class="rightfloat listing-filter-btn margin-right5">
                <span class="bwmsprite filter-icon"></span>
            </div>
            <div class="clear"></div>
        </header>

        <section class="container padding-top60 margin-bottom10">
            <div class="grid-12">
                <div class="bg-white content-inner-block-1520 box-shadow">
                    <h1 class="font16 text-pure-black margin-bottom15">Hero dealers in Mumbai <span class="font14 text-light-grey">(4)</span></h1>
                    <ul id="dealersList">
                        <li>
                            <div class="featured-tag text-white text-center font14 margin-bottom5">
                                Featured
                            </div>
                            <div class="font14">
                                <h2 class="font16 margin-bottom10"><a href="" class="text-black">Kamala Landmarc Motorbikes</a></h2>
                                <p class="text-light-grey margin-bottom5">Andheri, Mumbai</p>
                                <a href="tel:9876543210" class="text-light-grey margin-bottom5"><span class="bwmsprite tel-sm-grey-icon"></span> 9876543210</a>
                                <a href="mailto:bikewale@motors.com" class="text-light-grey"><span class="bwmsprite mail-grey-icon"></span> bikewale@motors.com</a>
                                <input type="button" class="btn btn-white-orange btn-full-width margin-top15 get-assistance-btn" value="Get assistance">
                            </div>
                        </li>
                        <li>
                            <div class="font14">
                                <h2 class="font16 margin-bottom10"><a href="" class="text-black">Kamala Landmarc Motorbikes</a></h2>
                                <p class="text-light-grey margin-bottom5">Andheri, Mumbai</p>
                                <a href="tel:9876543210" class="text-light-grey margin-bottom5"><span class="bwmsprite tel-sm-grey-icon"></span> 9876543210</a>
                                <a href="mailto:bikewale@motors.com" class="text-light-grey"><span class="bwmsprite mail-grey-icon"></span> bikewale@motors.com</a>
                                <input type="button" class="btn btn-white-orange btn-full-width margin-top15 get-assistance-btn" value="Get assistance">
                            </div>
                        </li>
                        <li>
                            <div class="font14">
                                <h2 class="font16 margin-bottom10"><a href="" class="text-black">Kamala Landmarc Motorbikes</a></h2>
                                <p class="text-light-grey margin-bottom5">Andheri, Mumbai</p>
                                <a href="tel:9876543210" class="text-light-grey margin-bottom5"><span class="bwmsprite tel-sm-grey-icon"></span> 9876543210</a>
                                <a href="mailto:bikewale@motors.com" class="text-light-grey"><span class="bwmsprite mail-grey-icon"></span> bikewale@motors.com</a>
                                <input type="button" class="btn btn-white-orange btn-full-width margin-top15 get-assistance-btn" value="Get assistance">
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
            <div class="clear"></div>
        </section>

        <div id="dealersFilterWrapper">
            <div class="ui-corner-top">
                <div id="hideDealerFilter" class="filterBackArrow">
                    <span class="bwmsprite fa-arrow-back"></span>
                </div>
                <div class="filterTitle">Locate dealers</div>
                <div id="dealerFilterReset" class="resetrTitle">Reset</div>
                <div class="clear"></div>
            </div>
            <div class="user-selected-brand-city-container content-inner-block-20">
                <div id="selectBrand" class="form-control text-left input-sm position-rel margin-bottom20">
                    <span class="position-abt progress-bar"></span>
                    <div class="user-selected">Select brand</div>
                    <span class="fa fa-spinner fa-spin position-abt text-black btnSpinner"></span>
                    <span class="bwmsprite fa-angle-right position-abt pos-top10 pos-right10"></span>
                </div>
                <div id="selectCity" class="form-control text-left input-sm position-rel margin-bottom20">
                    <span class="position-abt progress-bar"></span>
                    <div class="user-selected">Select city</div>
                    <span class="fa fa-spinner fa-spin position-abt text-black btnSpinner"></span>
                    <span class="bwmsprite fa-angle-right position-abt pos-top10 pos-right10"></span>
                </div>
                <div class="text-center position-rel">
                    <span class="position-abt progress-bar btn-loader"></span>
                    <a id="applyDealerFilter" class="btn btn-orange">Apply filter</a>
                </div>
            </div>
            <div id="dealerFilterContent" class="dealers-brand-city-wrapper">
                <div class="dealers-brand-popup-box bwm-brand-city-box bike-brand-list-container form-control-box text-left">
                    <div class="user-input-box">
                        <span class="dealers-back-arrow-box">
                            <span class="bwmsprite back-long-arrow-left"></span>
                        </span>
                        <input class="form-control" type="text" id="dealersBrandInput" autocomplete="off" placeholder="Select brand" />
                    </div>
                    <ul id="filterBrandList" class="filter-brand-city-ul margin-top40" data-filter-type="brand-filter">
                        <li>Brand 1</li>
                        <li>Brand 2</li>
                        <li>Brand 3</li>
                        <li>Brand 4</li>
                        <li>Brand 5</li>
                    </ul>
                </div>

                <div class="dealers-city-popup-box bwm-brand-city-box bike-city-list-container form-control-box text-left">
                    <div class="user-input-box">
                        <span class="dealers-back-arrow-box">
                            <span class="bwmsprite back-long-arrow-left"></span>
                        </span>
                        <input class="form-control" type="text" id="dealersCityInput" autocomplete="off" placeholder="Select city" />
                    </div>
                    <ul id="filterCityList" class="filter-brand-city-ul margin-top40" data-filter-type="city-filter">
                        <li>City 1</li>
                        <li>City 2</li>
                        <li>City 3</li>
                        <li>City 4</li>
                        <li>City 5</li>
                    </ul>
                </div>
            </div>
        </div>

        <!-- Lead Capture pop up start  -->
        <div id="leadCapturePopup" class="bw-popup bwm-fullscreen-popup contact-details hide">
            <div class="popup-inner-container text-center">
                <div class="bwmsprite close-btn leadCapture-close-btn rightfloat"></div>
                <div id="contactDetailsPopup">
                    <h2 class="margin-top10 margin-bottom10">Get more details on this bike</h2>
                    <p class="text-light-grey margin-bottom10">Please provide contact info to see more details</p>

                    <div class="personal-info-form-container">
                        <div class="form-control-box">
                            <input type="text" class="form-control get-first-name" placeholder="Your name" id="getFullName">
                            <span class="bwmsprite error-icon errorIcon"></span>
                            <div class="bw-blackbg-tooltip errorText"></div>
                        </div>
                        <div class="form-control-box margin-top20">
                            <input type="text" class="form-control get-email-id" placeholder="Email address" id="getEmailID">
                            <span class="bwmsprite error-icon errorIcon"></span>
                            <div class="bw-blackbg-tooltip errorText"></div>
                        </div>
                        <div class="form-control-box margin-top20">
                            <p class="mobile-prefix">+91</p>
                            <input type="text" class="form-control get-mobile-no" maxlength="10" placeholder="Mobile no." id="getMobile">
                            <span class="bwmsprite error-icon errorIcon"></span>
                            <div class="bw-blackbg-tooltip errorText"></div>
                        </div>
                        <div class="form-control-box margin-top20">
                            <input type="text" class="form-control get-model-name" placeholder="Model" id="getModelName">
                            <span class="bwmsprite error-icon errorIcon"></span>
                            <div class="bw-blackbg-tooltip errorText"></div>
                        </div>
                        <div class="clear"></div>
                        <a class="btn btn-full-width btn-orange margin-top20" id="user-details-submit-btn">Submit</a>
                    </div>
                </div>
                 <!-- thank you message starts here -->
                <div id="notify-response" class="hide margin-top10 content-inner-block-20 text-center">
                    <p class="font18 text-bold margin-bottom20">Thank you <span class="notify-leadUser"></span></p>
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
                                <input type="text" class="form-control" placeholder="Enter your OTP" id="getOTP" maxlength="5" />
                                <span class="bwmsprite error-icon errorIcon"></span>
                                <div class="bw-blackbg-tooltip errorText"></div>
                            </div>
                            <a class="margin-left10 blue resend-otp-btn margin-top10" id="resendCwiCode">Resend OTP</a>
                            <p class="margin-left10 margin-top10 otp-notify-text text-light-grey font12">
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
            </div>
        </div>
        <!-- Lead Capture pop up end  -->
    
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <!-- #include file="/includes/footerscript_Mobile.aspx" -->
        <script type="text/javascript">
            $('.listing-filter-btn').on('click', function () {
                $('#dealersFilterWrapper').animate({ 'left': '0' }, 500);
            });
            $('.filterBackArrow').on('click', function () {
                $('#dealersFilterWrapper').animate({ 'left': '100%' }, 500);
            });

            var selectBrand = $('#selectBrand'),
                selectCity = $('#selectCity'),
                dealerFilterContent = $('#dealerFilterContent');

            selectBrand.on("click", function () {
                $("#dealerFilterContent .dealers-brand-popup-box").show().siblings("div.dealers-city-popup-box").hide();
                animateFilterList();
            });

            selectCity.on("click", function () {
                $("#dealerFilterContent .dealers-brand-popup-box").hide().siblings("div.dealers-city-popup-box").show();
                animateFilterList();
            });

            var animateFilterList = function() {
                dealerFilterContent.addClass("open").stop().animate({ 'left': '0' }, 500);
                $(".user-input-box").stop().animate({ 'left': '0' }, 500);
            }

            $(".dealers-brand-city-wrapper .dealers-back-arrow-box").on("click", function () {
                dealerFilterContent.removeClass("open").stop().animate({ 'left': '100%' }, 500);
                $(".user-input-box").stop().animate({ 'left': '100%' }, 500);
            });

            $("#dealersBrandInput, #dealersCityInput").on("keyup", function () {
                locationFilter($(this));
            });

            $(".filter-brand-city-ul").on("click", "li", function () {
                var selectedElement = $(this),
                    selectedElementValue = selectedElement.text(),
                    selectedElementParent = selectedElement.parent(),
                    selectedElementInputField = selectedElementParent.siblings("div.user-input-box"),
                    selectedElementParentAttr = selectedElementParent.attr("data-filter-type"),
                    userSelectionType;
                selectedElementInputField.find("input").val(selectedElementValue);
                if (selectedElementParentAttr == "brand-filter")
                    userSelectionType = $("#selectBrand");
                else
                    userSelectionType = $("#selectCity");
                setUserSelection(selectedElementValue, userSelectionType);
            });

            var setUserSelection = function (selectedElementValue, userSelectionType) {
                userSelectionType.find('.user-selected').text(selectedElementValue);
                $(".dealers-brand-city-wrapper .dealers-back-arrow-box").trigger("click");
            };

            $("#dealerFilterReset").on("click", function () {
                $("#selectBrand .user-selected").text("Select brand");
                $("#selectCity .user-selected").text("Select city");
                $("#dealerFilterContent").find("input").val("");
            });

            $("#applyDealerFilter").on("click", function () {
                $(".filterBackArrow").trigger("click");
            });

            //assistance form
            $(".get-assistance-btn").on('click', function () {
                $("#leadCapturePopup").show();
                appendHash("assistancePopup");
                $("div#contactDetailsPopup").show();
                $("#otpPopup").hide();
            });            $(".leadCapture-close-btn, #notifyOkayBtn").on("click", function () {
                assistancePopupClose();
                window.history.back();
            });            var assistancePopupClose = function () {                $("#leadCapturePopup").hide();
                $("#notify-response").hide();            };            $("#user-details-submit-btn").on("click", function () {                if (validateUserDetail()) {                    $("#contactDetailsPopup").hide();                    $("#otpPopup").show();                    $(".lead-mobile").text($("#getMobile").val());                    //$(".notify-leadUser").text($("#getFullName").val());                    //$("#notify-response").show();                }            });            var validateUserDetail = function () {
                var isValid = true;
                isValid = validateName();
                isValid &= validateEmail();
                isValid &= validateMobile();
                isValid &= validateModel();
                return isValid;
            };

            var validateName = function () {
                var isValid = true,
                    name = $("#getFullName"),
                    nameLength = name.val().length;
                if (name.val().indexOf('&') != -1) {
                    setError(name, 'Invalid name');
                    isValid = false;
                }
                else if (nameLength == 0) {
                    setError(name, 'Please enter your name');
                    isValid = false;
                }
                else if (nameLength >= 1) {
                    hideError(name);
                    isValid = true;
                }
                return isValid;
            };

            var validateEmail = function () {
                var isValid = true,
                    emailId = $("#getEmailID"),
                    emailVal = emailId.val(),
                    reEmail = /^[A-z0-9._+-]+@[A-z0-9.-]+\.[A-z]{2,6}$/;
                if (emailVal == "") {
                    setError(emailId, 'Please enter email address');
                    isValid = false;
                }
                else if (!reEmail.test(emailVal)) {
                    setError(emailId, 'Invalid Email');
                    isValid = false;
                }
                return isValid;
            };

            var validateMobile = function () {
                var isValid = true,
                    mobileNo = $("#getMobile"),
                    mobileVal = mobileNo.val(),
                    reMobile = /^[0-9]{10}$/;
                if (mobileVal == "") {
                    setError(mobileNo, "Please enter your Mobile Number");
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

            var validateModel = function () {
                var isValid = true,
                    model = $("#getModelName"),
                    modelLength = model.val().length;
                if (model.val().indexOf('&') != -1) {
                    setError(model, 'Invalid model name');
                    isValid = false;
                }
                else if (modelLength == 0) {
                    setError(model, 'Please enter model name');
                    isValid = false;
                }
                else if (modelLength >= 1) {
                    hideError(model);
                    isValid = true;
                }
                return isValid;
            };

            var setError = function (element, msg) {
                element.addClass("border-red").siblings("span.errorIcon, div.errorText").show();
                element.siblings("div.errorText").text(msg);
            };

            var hideError = function (element) {
                element.removeClass("border-red").siblings("span.errorIcon, div.errorText").hide();
            };

            $("#getMobile, #getFullName, #getEmailID, #getModelName, #getUpdatedMobile, #getOTP").on("focus", function () {
                hideError($(this));
            });

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

            otpBtn.on("click", function () {
                if (validateOTP()) {

                }
            });

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

        </script>
    </form>
</body>
</html>
