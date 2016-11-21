<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Bikewale.Mobile.Used.Sell.Default" %>

<!DOCTYPE html>
<html>
<head>
    <title>Sell bikes</title>
    <!-- #include file="/includes/headscript_mobile_min.aspx" -->
    <link href="/min/m/css/sell-bike.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        <!-- #include file="\includes\gacode_mobile.aspx" -->
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->

        <section>
            <div id="sell-bike-content" class="container bg-white box-shadow margin-bottom20">
                <div id="form-step-tabs" class="text-center">
                    <h1 class="margin-bottom20">Sell your bike in 3 easy steps</h1>
                    <ul id="form-tab-list">
                        <li>
                            <div class="tab-item" data-bind="click: gotoStep1">
                                <span class="sell-bike-sprite" data-bind="css: formStep() == 1 ? 'step-1-active' : 'edit-step'"></span>
                                <p class="tab-title">Bike details</p>
                            </div>
                        </li><li>
                            <div class="tab-item" data-bind="click: gotoStep2">
                                <span class="sell-bike-sprite" data-bind="css: (formStep() == 2) ? 'step-2-active' : (formStep() > 1) ? 'edit-step' : 'step-2-inactive'"></span>
                                <p class="tab-title">Personal details</p>
                            </div>
                        </li><li>
                            <div class="tab-item">
                                <span class="sell-bike-sprite" data-bind="css: (formStep() == 3) ? 'step-3-active' : 'step-3-inactive'"></span>
                                <p class="tab-title">More details</p>
                            </div>
                        </li>
                    </ul>
                </div>

                <div class="form-step-body" data-bind="visible: formStep() == 1, with: bikeDetails">
                    <h2 class="form-step-title">Bike details</h2>
                    
                    <div id="div-kmsRidden" class="input-box form-control-box row-bottom-margin" data-bind="css: kmsRidden().length > 0 ? 'not-empty' : ''">
                        <input type="number" id="kmsRidden" min="1" data-bind="textInput: kmsRidden, validationElement: kmsRidden" />
                        <label for="kmsRidden">Kms ridden<sup>*</sup></label>
                        <span class="boundary"></span>
                        <span class="error-text" data-bind="validationMessage: kmsRidden"></span>
                    </div>

                    <div id="div-expectedPrice" class="input-box form-control-box row-bottom-margin" data-bind="css: expectedPrice().length > 0 ? 'not-empty' : ''">
                        <input type="number" min="1" id="expectedPrice" data-bind="textInput: expectedPrice, validationElement: expectedPrice" />
                        <label for="expectedPrice">Expected price<sup>*</sup></label>
                        <span class="boundary"></span>
                        <span class="error-text" data-bind="validationMessage: expectedPrice"></span>
                    </div>
                    
                    <div class="text-center margin-bottom15">
                        <input type="button" id="btnSaveBikeDetails" class="btn btn-orange btn-primary-big" value="Save and Continue"  data-bind="click: saveBikeDetails"/>
                    </div>

                </div>

                <div class="form-step-body" data-bind="visible: formStep() == 2 && !verificationDetails().status(), with: personalDetails">
                    <h2 class="form-step-title">Personal details</h2>

                    <ul id="seller-type-list">
                        <li data-bind="click: sellerType, attr: { value: 2 }, css: sellerTypeVal() == 2 ? 'checked' : ''" >
                            <span class="bwmsprite radio-icon"></span>
                            <span class="seller-label font16">I am an Individual</span>
                        </li>
                        <li data-bind="click: sellerType, attr: { value: 1 }, css: sellerTypeVal() == 1 ? 'checked' : ''">
                            <span class="bwmsprite radio-icon"></span>
                            <span class="seller-label font16">I am a dealer</span>
                        </li>
                    </ul>
                    <div class="clear"></div>

                    <div class="input-box form-control-box row-bottom-margin" data-bind="css: sellerName().length > 0 ? 'not-empty' : ''">
                        <input type="text" id="sellerName" data-bind="textInput: sellerName, validationElement: sellerName" />
                        <label for="sellerName">Name<sup>*</sup></label>
                        <span class="boundary"></span>
                        <span class="error-text" data-bind="validationMessage: sellerName"></span>
                    </div>

                    <div class="input-box form-control-box row-bottom-margin" data-bind="css: sellerEmail().length > 0 ? 'not-empty' : ''">
                        <input type="text" id="sellerEmail" data-bind="textInput: sellerEmail, validationElement: sellerEmail" />
                        <label for="sellerEmail">Email<sup>*</sup></label>
                        <span class="boundary"></span>
                        <span class="error-text" data-bind="validationMessage: sellerEmail"></span>
                    </div>
                    
                    <div class="input-box input-number-box form-control-box row-bottom-margin" data-bind="css: sellerMobile().length > 0 ? 'not-empty' : ''">
                        <input type="tel" maxlength="10" id="sellerMobile" data-bind="textInput: sellerMobile, validationElement: sellerMobile" />
                        <label for="sellerMobile">Mobile number<sup>*</sup></label>
                        <span class="input-number-prefix">+91</span>
                        <span class="boundary"></span>
                        <span class="input-number-label" data-bind="visible: mobileLabel">Responses from buyers will be sent to this number</span>
                        <span class="error-text" data-bind="validationMessage: sellerMobile"></span>
                    </div>

                    <div id="terms-content" class="row-bottom-margin padding-top10">
                        <span class="bwmsprite unchecked-box" data-bind="click: terms, css: termsCheckbox ? 'active' : ''"></span>
                        <p>I agree with BikeWale sell bike <a href="/TermsConditions.aspx" target="_blank">Terms & Conditions</a>, <a target="_blank" href="/visitoragreement.aspx">visitor agreement</a> and <a target="_blank" href="/privacypolicy.aspx">privacy policy</a> *.<br /><br />I agree that by clicking 'List your bike’ button, I am permitting buyers to contact me on my Mobile number.</p>
                        <span class="error-text" data-bind="validationMessage: termsCheckbox"></span>
                    </div>

                    <div class="text-center row-bottom-margin">
                        <input type="button" class="btn btn-white btn-primary-small margin-right20" value="Previous" data-bind="click: backToBikeDetails" /><input type="button" id ="btnListBike" class="btn btn-orange btn-primary-big" value="List your bike" data-bind="click: listYourBike" />
                    </div>

                </div>

                <div class="form-step-body" data-bind="visible: formStep() == 2 && verificationDetails().status()">
                    <h2 class="verify-title">Verification</h2>
                    <p class="verify-desc">We have just sent a 5 digit verification code on your mobile number.</p>

                    <div data-bind="visible: !verificationDetails().updateMobileStatus()">
                        <div class="margin-bottom30">
                            <div class="leftfloat">
                                <p class="font12 text-light-grey">Mobile number</p>
                                <p class="font16 text-bold" data-bind="text: personalDetails().sellerMobile()"></p>
                            </div>
                            <div class="rightfloat bwmsprite edit-blue-icon" data-bind="click: verificationDetails().updateSellerMobile"></div>
                            <div class="clear"></div>
                        </div>

                        <div class="input-box form-control-box margin-bottom10" data-bind="css: verificationDetails().otpCode().length > 0 ? 'not-empty' : ''">
                            <input type="tel" id="otpCode" maxlength="5" data-bind="textInput: verificationDetails().otpCode, validationElement: verificationDetails().otpCode" />
                            <label for="otpCode">One-time password<sup>*</sup></label>
                            <span class="boundary"></span>
                            <span id="otpErrorText" class="error-text" data-bind="validationMessage: verificationDetails().otpCode"></span>
                        </div>

                        <div class="text-center row-bottom-margin">
                            <input type="button" class="btn btn-orange btn-primary-big" value="Verify" data-bind="click: verificationDetails().verifySeller" />
                        </div>
                    </div>

                    <div data-bind="visible: verificationDetails().updateMobileStatus()">
                        <div class="input-box input-number-box form-control-box margin-bottom10" data-bind="css: verificationDetails().updatedMobile().length > 0 ? 'not-empty' : ''">
                            <input type="tel" maxlength="10" id="updatedMobile" data-bind="textInput: verificationDetails().updatedMobile, validationElement: verificationDetails().updatedMobile" />
                            <label for="updatedMobile">Mobile number<sup>*</sup></label>
                            <span class="input-number-prefix">+91</span>
                            <span class="boundary"></span>
                            <span class="error-text" data-bind="validationMessage: verificationDetails().updatedMobile"></span>
                        </div>

                        <div class="text-center row-bottom-margin">
                            <input type="button" class="btn btn-orange btn-primary-small" value="Done" data-bind="click: verificationDetails().submitUpdatedMobile" />
                        </div>
                    </div>

                </div>

                <div class="form-step-body" data-bind="visible: formStep() == 3">
                    <h2>More details</h2>
                </div>

            </div>
        </section>


        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/common.min.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/knockout.validation.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/sell-bike.js?<%= staticFileVersion %>"></script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />
    </form>
</body>
</html>
