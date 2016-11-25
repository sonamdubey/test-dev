<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Bikewale.Mobile.Used.Sell.Default" %>

<!DOCTYPE html>
<html>
<head>
    <title>Sell bikes</title>
    <!-- #include file="/includes/headscript_mobile_min.aspx" -->
    <link href="/build/min/m/css/sell-bike.css" rel="stylesheet" type="text/css" />
    <link href="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/css/dropzone.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        <!-- #include file="\includes\gacode_mobile.aspx" -->
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->

        <section>
            <div id="sell-bike-content" class="container bg-white box-shadow margin-bottom20">
                <div data-bind="visible: formStep() < 4">

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

                        <div class="slideIn-input-box-content row-bottom-margin" data-bind="css: bikeStatus() ? 'selection-done' : ''">
                            <div id="bike-select-element" class="slideIn-input-box">
                                <p class="slideIn-input-label bike-box-default">Bike<sup>*</sup></p>
                                <p class="selected-option-box bike-box-default text-truncate" data-bind="text: bike, validationElement: bike"></p>
                                <span class="boundary"></span>
                                <span class="error-text" data-bind="validationMessage: bike"></span>
                                <span class="bwmsprite grey-right-icon"></span>
                            </div>
                        </div>
                    
                        <div id="div-kmsRidden" class="input-box form-control-box row-bottom-margin" data-bind="css: kmsRidden().length > 0 ? 'not-empty' : ''">
                            <input type="text" id="kmsRidden" data-value="" data-bind="textInput: kmsRidden, validationElement: kmsRidden" />
                            <label for="kmsRidden">Kms ridden<sup>*</sup></label>
                            <span class="boundary"></span>
                            <span class="error-text" data-bind="validationMessage: kmsRidden"></span>
                        </div>

                        <div class="slideIn-input-box-content row-bottom-margin" data-bind="css: city().length > 0 ? 'selection-done' : ''">
                            <div id="city-select-element" class="slideIn-input-box">
                                <p class="slideIn-input-label city-box-default">City<sup>*</sup></p>
                                <p class="selected-option-box city-box-default text-truncate" data-bind="text: city, validationElement: city"></p>
                                <span class="boundary"></span>
                                <span class="error-text" data-bind="validationMessage: city"></span>
                                <span class="bwmsprite grey-right-icon"></span>
                            </div>
                        </div>                    

                        <div id="div-expectedPrice" class="input-box form-control-box row-bottom-margin" data-bind="css: expectedPrice().length > 0 ? 'not-empty' : ''">
                            <input type="text" id="expectedPrice" data-value="" data-bind="textInput: expectedPrice, validationElement: expectedPrice" />
                            <label for="expectedPrice">Expected price<sup>*</sup></label>
                            <span class="boundary"></span>
                            <span class="error-text" data-bind="validationMessage: expectedPrice"></span>
                        </div>

                        <div class="select-box select-box-no-input row-bottom-margin">
                            <p class="select-label">Owner<sup>*</sup></p>
                            <select class="chosen-select" data-bind="chosen: {}, value: owner, validationElement: owner" data-title="Owner">
                                <option value></option>
                                <option value="1">I bought it new</option>
                                <option value="2">I'm the second owner</option>
                                <option value="3">I'm the third owner</option>
                                <option value="4">I'm the fourth owner</option>
                                <option value="5">Four or more previous owners</option>
                            </select>
                            <span class="boundary"></span>
                            <span class="error-text" data-bind="validationMessage: owner"></span>
                        </div>

                        <div class="slideIn-input-box-content row-bottom-margin" data-bind="css: registeredCity().length > 0 ? 'selection-done' : ''">
                            <div id="registration-select-element" class="slideIn-input-box">
                                <p class="slideIn-input-label city-box-default">Bike registered at<sup>*</sup></p>
                                <p class="selected-option-box city-box-default text-truncate" data-bind="text: registeredCity, validationElement: registeredCity"></p>
                                <span class="boundary"></span>
                                <span class="error-text" data-bind="validationMessage: registeredCity"></span>
                                <span class="bwmsprite grey-right-icon"></span>
                            </div>
                        </div>

                        <div class="color-box-content row-bottom-margin">
                            <div id="select-color-box" class="select-color-box">
                                <p class="select-color-label color-box-default">Colour<sup>*</sup></p>
                                <p id="selected-color" class="color-box-default" data-bind="text: color, validationElement: color "></p>
                                <span class="boundary"></span>
                                <span class="error-text" data-bind="validationMessage: color"></span>
                            </div>
                        </div>
                    
                        <div class="text-center margin-bottom15">
                            <input type="button" id="btnSaveBikeDetails" class="btn btn-orange btn-primary-big" value="Save and Continue"  data-bind="click: saveBikeDetails"/>
                        </div>

                        <!-- select bike starts here -->
                        <div id="select-bike-cover-popup" class="cover-window-popup">
                            <div class="ui-corner-top">
                                <div id="close-bike-popup" class="cover-popup-back cur-pointer leftfloat">
                                    <span class="bwmsprite fa-angle-left"></span>
                                </div>
                                <div class="cover-popup-header leftfloat">Select bikes</div>
                                <div class="clear"></div>
                            </div>
                            <div class="bike-banner"></div>
                            <div id="select-make-wrapper" class="cover-popup-body">
                                <div class="cover-popup-body-head">
                                    <p class="no-back-btn-label head-label inline-block">Select Make</p>
                                </div>
                                <ul class="cover-popup-list with-arrow">
                                    <li data-bind="click: makeChanged"><span>Bajaj</span></li>
                                    <li data-bind="click: makeChanged"><span>Honda</span></li>
                                    <li data-bind="click: makeChanged"><span>Hero</span></li>
                                    <li data-bind="click: makeChanged"><span>Royal Enfield</span></li>
                                </ul>
                            </div>

                            <div id="select-model-wrapper" class="cover-popup-body">
                                <div class="cover-popup-body-head">
                                    <div id="select-model-back-btn" class="body-popup-back cur-pointer inline-block">
                                        <span class="bwmsprite back-long-arrow-left"></span>
                                    </div><p class="head-label inline-block">Select Model</p>
                                </div>
                                <ul class="cover-popup-list with-arrow">
                                    <li data-bind="click: modelChanged"><span>CT 100</span></li>
                                    <li data-bind="click: modelChanged"><span>Platina</span></li>
                                    <li data-bind="click: modelChanged"><span>Discover 125</span></li>
                                    <li data-bind="click: modelChanged"><span>Discover 150S</span></li>
                                </ul>
                            </div>

                            <div id="select-version-wrapper" class="cover-popup-body">
                                <div class="cover-popup-body-head">
                                    <div id="select-version-back-btn" class="body-popup-back cur-pointer inline-block">
                                        <span class="bwmsprite back-long-arrow-left"></span>
                                    </div><p class="head-label inline-block">Select Version</p>
                                </div>
                                <ul class="cover-popup-list">
                                    <li data-bind="click: versionChanged"><span>Kick/Drum/Spokes</span></li>
                                    <li data-bind="click: versionChanged"><span>Electric Start/Drum/Alloy</span></li>
                                    <li data-bind="click: versionChanged"><span>CBS</span></li>
                                </ul>
                            </div>

                            <div class="cover-popup-loader-body">
                                <div class="cover-popup-loader"></div>
                                <div class="cover-popup-loader-text font14">Loading...</div>
                            </div>
                        </div>
                        <!-- select bike ends here -->

                        <!-- city drawer starts here -->
                       <div id="city-slideIn-drawer" class="slideIn-drawer-container">
                            <div class="form-control-box text-left">
                                <div class="drawer-top-header">
                                    <div>
                                        <span id="close-city-filter" class="back-arrow-box inline-block">
                                            <span class="bwmsprite back-long-arrow-left"></span>
                                        </span><p class="head-label inline-block">Select city</p>
                                    </div>
                                    <div class="filter-input-box">
                                        <div class="form-control-box">
                                            <input type="text" class="form-control padding-right40" placeholder="Type to select city" autocomplete="off">
                                            <span class="bwmsprite search-icon-grey"></span>
                                        </div>
                                    </div>
                                </div>
                    
                                <ul class="filter-list" >
                                    <li><span>Ahmedabad</span></li>
                                    <li><span>Bangalore</span></li>
                                    <li><span>Chennai</span></li>
                                    <li><span>Kolkata</span></li>
                                    <li><span>Mumbai</span></li>
                                    <li><span>Ahmedabad</span></li>
                                    <li><span>Bangalore</span></li>
                                    <li><span>Chennai</span></li>
                                    <li><span>Kolkata</span></li>
                                    <li><span>Mumbai</span></li>
                                    <li><span>Ahmedabad</span></li>
                                    <li><span>Bangalore</span></li>
                                    <li><span>Chennai</span></li>
                                    <li><span>Kolkata</span></li>
                                    <li><span>Mumbai</span></li>
                                </ul>
                            </div>
                        </div>
                        <!-- city drawer ends here -->

                        <!-- color popup starts here -->
                        <div class="modal-background"></div>
                        <div id="color-popup" class="modal-popup-container with-footer">
                            <div class="popup-header">Colour</div>
                            <div class="popup-body">
                                <ul class="popup-list popup-color-list margin-bottom15">
                                    <li class="color-list-item" data-bind="click: colorSelection">
                                        <div class="color-box color-count-one">
                                            <span style="background-color: rgb(19, 22, 29);"></span>
                                        </div>
                                        <p class="color-box-label">Black</p>
                                    </li>
                                    <li class="color-list-item" data-bind="click: colorSelection">
                                        <div class="color-box color-count-one">
                                            <span style="background-color: rgb(124, 80, 41)"></span>
                                        </div>
                                        <p class="color-box-label">Divine Black</p>
                                    </li>
                                    <li class="color-list-item" data-bind="click: colorSelection">
                                        <div class="color-box color-count-one">
                                            <span style="background-color: rgb(19, 22, 29);"></span>
                                        </div>
                                        <p class="color-box-label">Black</p>
                                    </li>
                                    <li class="color-list-item" data-bind="click: colorSelection">
                                        <div class="color-box color-count-one">
                                            <span style="background-color: rgb(19, 22, 29);"></span>
                                        </div>
                                        <p class="color-box-label">Black</p>
                                    </li>
                                    <li class="color-list-item" data-bind="click: colorSelection">
                                        <div class="color-box color-count-one">
                                            <span style="background-color: rgb(124, 80, 41)"></span>
                                        </div>
                                        <p class="color-box-label">Divine Black</p>
                                    </li>
                                    <li class="color-list-item" data-bind="click: colorSelection">
                                        <div class="color-box color-count-one">
                                            <span style="background-color: rgb(19, 22, 29);"></span>
                                        </div>
                                        <p class="color-box-label">Black</p>
                                    </li>                       
                                </ul>
                                <ul>
                                    <li class="other-color-item">
                                        <div class="color-box">
                                            <span></span>
                                        </div>
                                        <div class="input-box input-color-box form-control-box" data-bind="css: otherColor().length > 0 ? 'not-empty' : '', validationElement: otherColor">
                                            <input type="text" id="otherColor" data-bind="textInput: otherColor" />
                                            <label for="otherColor">Other, please specify</label>
                                            <span class="boundary"></span>
                                            <span class="error-text" data-bind="validationMessage: otherColor"></span>
                                        </div>
                                    </li>
                                </ul>
                            </div>
                            <div class="popup-footer">
                                <div class="grid-6 alpha">
                                    <button type="button" class="btn btn-white btn-full-width btn-size-sm cancel-popup-btn">Cancel</button>
                                </div>
                                <div class="grid-6 omega">
                                    <button type="button" class="btn btn-orange btn-full-width btn-size-sm" data-bind="click: otherColor().length > 0 ? submitOtherColor : submitColor">Done</button>
                                </div>
                                <div class="clear"></div>
                            </div>

                            <div class="clear"></div>
                        </div>
                        <!-- color popup ends here -->
                    
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
                                <input type="button" class="btn btn-orange btn-primary-small" value="Verify" data-bind="click: verificationDetails().verifySeller" />
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

                    <div class="form-step-body" data-bind="visible: formStep() == 3, with: moreDetails">
                        <h2 class="margin-bottom15">More details</h2>
                        <p class="font16 text-black margin-bottom5">Add Photos</p>
                        <p class="font14 text-light-grey margin-bottom20">Ads with photos are likely to get 50% more responses! You can upload upto 10 photos with first photo being the profile photo for the ad. Supported formats: .jpg, .png; Image Size < 4 MB</p>

                        <div id="add-photos-dropzone" class="dropzone dz-clickable">
                            <div class="dz-message">
                                <div id="dz-custom-message">
                                    <span class="sell-bike-sprite add-photo-icon"></span><br />
                                    <button type="button" class="btn btn-primary-big btn-orange margin-top20 margin-bottom15">Add photos</button>
                                </div>
                                <ul id="dz-image-placeholder" class="margin-top10">
                                    <li></li>
                                    <li></li>
                                </ul>
                                <div class="clear"></div>
                            </div>
                        </div>

                        <div class="add-photos-separator"></div>

                        <div class="input-box form-control-box row-bottom-margin" data-bind="css: registrationNumber().length > 0 ? 'not-empty' : ''">
                            <input type="text" id="registrationNumber" data-bind="textInput: registrationNumber" />
                            <label for="registrationNumber">Registration number</label>
                            <span class="boundary"></span>
                        </div>

                        <div class="select-box select-box-no-input">
                            <p class="select-label">Insurance</p>
                            <select id="select-insuranceType" class="chosen-select" data-bind="chosen: {}, value: insuranceType" data-title="Insurance">
                                <option value></option>
                                <option value="Comprehensive">Comprehensive</option>
                                <option value="Third Party">Third Party</option>
                                <option value="No Insurance">No Insurance</option>
                            </select>
                            <span class="boundary"></span>
                        </div>

                        <div class="textarea-box form-control-box margin-bottom30">
                            <p class="textarea-label">Ad description</p>
                            <textarea rows="2" cols="20" data-bind=" value: adDescription "></textarea>
                            <span class="boundary"></span>
                        </div>

                        <div class="text-center">
                            <input type="button" class="btn btn-white btn-primary-small margin-right20" value="No Thanks" data-bind="click: noThanks" /><input type="button" id="btnUpdateAd" class="btn btn-orange btn-primary-big" value="Update my Ad" data-bind="click: updateAd" />
                        </div>

                    </div>
                </div>

                <div id="form-success" class="form-response-body text-center icon-size-small" data-bind="visible: formStep() == 4">
                    <div class="icon-outer-container rounded-corner50percent">
                        <div class="icon-inner-container rounded-corner50percent">
                            <span class="bwmsprite thankyou-icon margin-top15"></span>
                        </div>
                    </div>
                    <div class="margin-top15 margin-bottom15">
                        <p class="font18 text-bold">Congratulations!</p><br />
                        <p class="font14">Your profile ID is <span>138462384</span>.<br />You can find and edit your ad later using this id. Your bike ad will be live after verification.</p>
                    </div>
                    <div id="form-success-btn-group">
                        <input type="button" class="btn btn-orange btn-primary-small margin-right20" value="Done" />
                        <input  type="button" id ="btnEditAd" class="btn btn-white btn-primary-small" value="Edit my Ad" />
                    </div>
                </div>

                <!-- not auth to edit -->

                <%--<div class="form-response-body text-center icon-size-small">
                    <div class="icon-outer-container rounded-corner50percent">
                        <div class="icon-inner-container rounded-corner50percent">
                            <span class="sell-bike-sprite no-auth-edit-icon margin-top15"></span>
                        </div>
                    </div>
                    <div class="margin-top15 margin-bottom15">
                        <p class="font18 text-bold">Sorry!</p><br />
                        <p class="font14">You are not authorised to edit this listing</p>
                    </div>
                </div>--%>

                <!-- not auth to add -->

                <%--<div class="form-response-body text-center icon-size-small">
                    <div class="icon-outer-container rounded-corner50percent">
                        <div class="icon-inner-container rounded-corner50percent">
                            <span class="sell-bike-sprite no-auth-icon margin-top15"></span>
                        </div>
                    </div>
                    <div class="margin-top15 margin-bottom15">
                        <p class="font18 text-bold margin-bottom10">Sorry!</p>
                        <p class="font14">You are not authorised to add any listing.<br />Please contact us at <a href="mailto:contact@bikewale.com">contact@bikewale.com</a></p>
                    </div>
                </div>--%>

            </div>
        </section>
        
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <link href="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/css/chosen.min.css?<%= staticFileVersion %>" type="text/css" rel="stylesheet" />
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/common.min.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/m/src/chosen-jquery-min-mobile.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/knockout.validation.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/dropzone.js?<%=staticFileVersion %>"></script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/sell-bike.js?<%= staticFileVersion %>"></script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />
        
    </form>
</body>
</html>
