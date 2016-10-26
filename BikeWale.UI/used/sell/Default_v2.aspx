<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default_v2.aspx.cs" Inherits="Bikewale.Used.Sell.Default_v2" %>

<!DOCTYPE html>

<html>
<head>
    <title>Sell Bike</title>

    <!-- #include file="/includes/headscript_desktop_min.aspx" -->

    <link href="/css/sell-bike.css" rel="stylesheet" type="text/css" />
    <link href="/css/dropzone.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_desktop.aspx" -->
    </script>
    
</head>
<body class="bg-light-grey header-fixed-inner">
    <form id="form1" runat="server">
        <!-- #include file="/includes/headBW.aspx" -->

        <section class="bg-light-grey padding-top10" id="breadcrumb">
            <div class="container">
                <div class="grid-12">
                    <div class="breadcrumb margin-bottom15">
                        <!-- breadcrumb code starts here -->
                        <ul>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb"><a href="/" itemprop="url">
                                <span itemprop="title">Home</span></a>
                            </li>
                            <li itemtype="http://data-vocabulary.org/Breadcrumb"><span class="bwsprite fa-angle-right margin-right10"></span>
                              <a href="/used/" itemprop="url"><span>Used Bikes</span></a>
                            </li>
                            <li itemtype="http://data-vocabulary.org/Breadcrumb"><span class="bwsprite fa-angle-right margin-right10"></span>
                              <span>Sell Your Bike</span>
                            </li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section>
            <div class="container margin-bottom20">
                <div class="grid-12">
                    <div class="content-box-shadow">
                        <div class="content-box-shadow card-header">
                            <h1>Sell your bike</h1>
                        </div>
                        <div id="sell-bike-content">
                            <div id="sell-bike-left-col" class="grid-7 panel-group">
                                <!-- start of form steps -->
                                <div data-bind="visible: formStep() < 4">
                                    <div class="panel panel-divider">
                                        <div class="panel-head">
                                            <span class="sell-bike-sprite" data-bind="click: gotoStep1, css: formStep() == 1 ? 'step-1-active' : 'edit-step'"></span>
                                            <span class="panel-title">Bike details</span>
                                        </div>
                                        <div class="panel-body" data-bind="visible: formStep() == 1">
                                            <div class="panel-row margin-bottom10">
                                                <div class="grid-4 alpha select-box">
                                                    <p class="select-label">Make<sup>*</sup></p>
                                                    <select class="chosen-select" data-placeholder="Select make" data-bind="chosen: {}, value: bikeDetails().make, validationElement: bikeDetails().make">
                                                        <option value></option>
                                                        <option value="10">Honda</option>
                                                        <option value="11">Bajaj</option>
                                                        <option value="12">Hero</option>
                                                        <option value="13">TVS</option>
                                                        <option value="14">Royal Enfield</option>
                                                        <option value="15">Harley Davidson</option>
                                                        <option value="16">KTM</option>
                                                        <option value="17">Aprilia</option>
                                                        <option value="18">Benelli</option>
                                                        <option value="19">Yamaha</option>
                                                    </select>
                                                    <span class="boundary"></span>
                                                    <span class="error-text" data-bind="validationMessage: bikeDetails().make"></span>
                                                </div>
                                                <div class="grid-4 select-box">
                                                    <p class="select-label">Model<sup>*</sup></p>
                                                    <select class="chosen-select" data-placeholder="Select model" data-bind="chosen: {}, value: bikeDetails().model, validationElement: bikeDetails().model">
                                                        <option value></option>
                                                        <option value="50">125 Scooter</option>
                                                        <option value="51">Activa</option>
                                                        <option value="52">CB Hornet 160R</option>
                                                        <option value="53">CB Shine</option>
                                                        <option value="54">Avenger 220 Cruise</option>
                                                        <option value="55">Avenger 220 Street</option>
                                                    </select>
                                                    <span class="boundary"></span>
                                                    <span class="error-text" data-bind="validationMessage: bikeDetails().model"></span>
                                                </div>
                                                <div class="grid-4 omega select-box">
                                                    <p class="select-label">Version<sup>*</sup></p>
                                                    <select class="chosen-select" data-placeholder="Select version" data-bind="chosen: {}, value: bikeDetails().version, validationElement: bikeDetails().version">
                                                        <option value></option>
                                                        <option value="80">Kick/Drum/Spokes</option>
                                                        <option value="81">Electric Start/Drum/Alloy</option>
                                                        <option value="82">CBS</option>
                                                    </select>
                                                    <span class="boundary"></span>
                                                    <span class="error-text" data-bind="validationMessage: bikeDetails().version"></span>
                                                </div>
                                                <div class="clear"></div>
                                            </div>

                                            <div class="panel-row margin-bottom10">
                                                <div class="calendar-box-content">
                                                    <div id="select-calendar-box" class="select-calendar-box">
                                                        <p class="select-calendar-label calendar-box-default">Year of manufacturing<sup>*</sup></p>
                                                        <p id="selected-calendar-date" class="calendar-box-default" data-bind="text: bikeDetails().manufacturingDate, validationElement: bikeDetails().manufacturingDate"></p>
                                                        <span class="boundary"></span>
                                                        <span class="error-text" data-bind="validationMessage: bikeDetails().manufacturingDate"></span>
                                                        <div id="calendar-content">
                                                            <p class="dropdown-label">Year of manufacturing</p>
                                                            <div id="year-content">
                                                                <ul id="year-list"></ul>
                                                                <span class="year-control bwsprite year-prev"></span>
                                                                <span class="year-control bwsprite year-next"></span>
                                                            </div>
                                                            <ul id="month-list"></ul>
                                                            <div class="clear"></div>
                                                            <div id="calendar-error" class="error-text"></div>
                                                            <div class="text-center padding-bottom25">
                                                                <input type="button" id="submit-calendar-btn" class="btn btn-orange btn-primary-big" value="Select" data-bind="click: bikeDetails().submitManufacturingDate" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="panel-row margin-bottom20">
                                                <div class="input-box form-control-box" data-bind="css: bikeDetails().kmsRidden().length > 0 ? 'not-empty' : ''">
                                                    <input type="number" id="kmsRidden" min="1" data-bind="textInput: bikeDetails().kmsRidden, validationElement: bikeDetails().kmsRidden" />
                                                    <label for="kmsRidden">Kms ridden<sup>*</sup></label>
                                                    <span class="boundary"></span>
                                                    <span class="error-text" data-bind="validationMessage: bikeDetails().kmsRidden"></span>
                                                </div>
                                            </div>

                                            <div class="panel-row margin-bottom20">
                                                <div class="select-box">
                                                    <p class="select-label">City<sup>*</sup></p>
                                                    <select class="chosen-select" data-placeholder="Select city" data-bind="chosen: {}, value: bikeDetails().city, validationElement: bikeDetails().city">
                                                        <option value></option>
                                                        <option value="14">Ahmednagar</option>
                                                        <option value="361">Alibag</option>
                                                        <option value="1">Mumbai</option>
                                                        <option value="13">Navi Mumbai</option>
                                                        <option value="8">Panvel</option>
                                                        <option value="15">Ahmednagar</option>
                                                        <option value="362">Alibag</option>
                                                        <option value="2">Mumbai</option>
                                                        <option value="15">Navi Mumbai</option>
                                                        <option value="9">Panvel</option>
                                                    </select>
                                                    <span class="boundary"></span>
                                                    <span class="error-text" data-bind="validationMessage: bikeDetails().city"></span>
                                                </div>
                                            </div>

                                            <div class="panel-row margin-bottom20">
                                                <div class="input-box form-control-box" data-bind="css: bikeDetails().expectedPrice().length > 0 ? 'not-empty' : ''">
                                                    <input type="number" id="expectedPrice" min="1" data-bind="textInput: bikeDetails().expectedPrice, validationElement: bikeDetails().expectedPrice" />
                                                    <label for="expectedPrice">Expected price<sup>*</sup></label>
                                                    <span class="boundary"></span>
                                                    <span class="error-text" data-bind="validationMessage: bikeDetails().expectedPrice"></span>
                                                </div>
                                            </div>

                                            <div class="panel-row margin-bottom20">
                                                <div class="select-box select-box-no-input">
                                                    <p class="select-label">Owner<sup>*</sup></p>
                                                    <select class="chosen-select" data-bind="chosen: {}, value: bikeDetails().owner, validationElement: bikeDetails().owner" data-title="Owner">
                                                        <option value></option>
                                                        <option value="1">I bought it new</option>
                                                        <option value="2">I'm the second owner</option>
                                                        <option value="3">I'm the third owner</option>
                                                        <option value="4">I'm the fourth owner</option>
                                                        <option value="5">Four or more previous owners</option>
                                                    </select>
                                                    <span class="boundary"></span>
                                                    <span class="error-text" data-bind="validationMessage: bikeDetails().owner"></span>
                                                </div>
                                            </div>

                                            <div class="panel-row margin-bottom20">
                                                <div class="select-box">
                                                    <p class="select-label">Bike registered at<sup>*</sup></p>
                                                    <select class="chosen-select" data-placeholder="Select city" data-bind="chosen: {}, value: bikeDetails().registeredCity, validationElement: bikeDetails().registeredCity">
                                                        <option value></option>
                                                        <option value="14">Ahmednagar</option>
                                                        <option value="361">Alibag</option>
                                                        <option value="1">Mumbai</option>
                                                        <option value="13">Navi Mumbai</option>
                                                        <option value="8">Panvel</option>
                                                        <option value="15">Ahmednagar</option>
                                                        <option value="362">Alibag</option>
                                                        <option value="2">Mumbai</option>
                                                        <option value="15">Navi Mumbai</option>
                                                        <option value="9">Panvel</option>
                                                    </select>
                                                    <span class="boundary"></span>
                                                    <span class="error-text" data-bind="validationMessage: bikeDetails().registeredCity"></span>
                                                </div>
                                            </div>

                                            <div class="panel-row margin-bottom10">
                                                <div class="color-box-content">
                                                    <div id="select-color-box" class="select-color-box">
                                                        <p class="select-color-label color-box-default">Colour<sup>*</sup></p>
                                                        <p id="selected-color" class="color-box-default" data-bind="text: bikeDetails().color, validationElement: bikeDetails().color"></p>
                                                        <span class="boundary"></span>
                                                        <span class="error-text" data-bind="validationMessage: bikeDetails().color"></span>

                                                        <div class="color-dropdown">
                                                            <p class="dropdown-label">Colour</p>
                                                            <ul>
                                                                <li class="color-list-item" data-bind="click: bikeDetails().colorSelection">
                                                                    <div class="color-box color-count-one">
                                                                        <span style="background-color:#c83333"></span>
                                                                    </div>
                                                                    <p class="color-box-label">Red</p>
                                                                </li>
                                                                <li class="color-list-item" data-bind="click: bikeDetails().colorSelection">
                                                                    <div class="color-box color-count-two">
                                                                        <span style="background-color:#c83333"></span>
                                                                        <span style="background-color:#1b1a1a"></span>
                                                                    </div>
                                                                    <p class="color-box-label">Black and Red</p>
                                                                </li>
                                                                <li class="color-list-item" data-bind="click: bikeDetails().colorSelection">
                                                                    <div class="color-box color-count-three">
                                                                        <span style="background-color:#c83333"></span>
                                                                        <span style="background-color:#1b1a1a"></span>
                                                                        <span style="background-color:#3a5cee"></span>
                                                                    </div>
                                                                    <p class="color-box-label">Black, Red and Blue</p>
                                                                </li>
                                                                <li class="other-color-item">
                                                                    <div class="color-box">
                                                                        <span></span>
                                                                    </div>
                                                                    <div class="input-box input-color-box form-control-box" data-bind="css: bikeDetails().otherColor().length > 0 ? 'not-empty' : '', validationElement: bikeDetails().otherColor">
                                                                        <input type="text" id="otherColor" data-bind="textInput: bikeDetails().otherColor" />
                                                                        <label for="otherColor">Other, please specify</label>
                                                                        <span class="boundary"></span>
                                                                        <span class="error-text" data-bind="validationMessage: bikeDetails().otherColor"></span>
                                                                    </div>
                                                                </li>
                                                            </ul>
                                                            <div class="text-center padding-bottom20" data-bind="visible: bikeDetails().otherColor().length > 0">
                                                                <button type="button" class="btn btn-orange btn-secondary-small" data-bind="click: bikeDetails().submitOtherColor">Done</button>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="panel-row">
                                                <input type="button" class="btn btn-orange btn-primary-big" value="Save and Continue" data-bind="click: bikeDetails().saveBikeDetails" />
                                            </div>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                    <div class="panel panel-divider">
                                        <div class="panel-head">
                                            <span class="sell-bike-sprite" data-bind="click: gotoStep2, css: (formStep() == 2) ? 'step-2-active' : (formStep() > 1) ? 'edit-step' : 'step-2-inactive'"></span>
                                            <span class="panel-title">Personal details</span>
                                        </div>
                                        <div class="panel-body" data-bind="visible: formStep() == 2 && !verificationDetails().status()">
                                            <div class="panel-row margin-bottom30">
                                                <ul id="seller-type-list">
                                                    <li data-bind="click: personalDetails().sellerType" class="checked">
                                                        <span class="bwsprite radio-icon"></span>
                                                        <span class="seller-label">I am an Individual</span>
                                                    </li>
                                                    <li data-bind="click: personalDetails().sellerType">
                                                        <span class="bwsprite radio-icon"></span>
                                                        <span class="seller-label">I am a dealer</span>
                                                    </li>
                                                </ul>
                                                <div class="clear"></div>
                                            </div>

                                            <div class="panel-row margin-bottom20">
                                                <div class="input-box form-control-box" data-bind="css: personalDetails().sellerName().length > 0 ? 'not-empty' : ''">
                                                    <input type="text" id="sellerName" data-bind="textInput: personalDetails().sellerName, validationElement: personalDetails().sellerName" />
                                                    <label for="sellerName">Name<sup>*</sup></label>
                                                    <span class="boundary"></span>
                                                    <span class="error-text" data-bind="validationMessage: personalDetails().sellerName"></span>
                                                </div>
                                            </div>
                                        
                                            <div class="panel-row margin-bottom20">
                                                <div class="input-box form-control-box" data-bind="css: personalDetails().sellerEmail().length > 0 ? 'not-empty' : ''">
                                                    <input type="text" id="sellerEmail" data-bind="textInput: personalDetails().sellerEmail, validationElement: personalDetails().sellerEmail" />
                                                    <label for="sellerEmail">Email<sup>*</sup></label>
                                                    <span class="boundary"></span>
                                                    <span class="error-text" data-bind="validationMessage: personalDetails().sellerEmail"></span>
                                                </div>
                                            </div>
                                        
                                            <div class="panel-row margin-bottom20">
                                                <div class="input-box input-number-box form-control-box" data-bind="css: personalDetails().sellerMobile().length > 0 ? 'not-empty' : ''">
                                                    <input type="tel" maxlength="10" id="sellerMobile" data-bind="textInput: personalDetails().sellerMobile, validationElement: personalDetails().sellerMobile" />
                                                    <label for="sellerMobile">Mobile number<sup>*</sup></label>
                                                    <span class="input-number-prefix">+91</span>
                                                    <span class="boundary"></span>
                                                    <span class="input-number-label" data-bind="visible: personalDetails().mobileLabel">Responses from buyers will be sent to this number</span>
                                                    <span class="error-text" data-bind="validationMessage: personalDetails().sellerMobile"></span>
                                                </div>
                                            </div>
                                        
                                            <div class="panel-row margin-bottom20">
                                                <div id="terms-content">
                                                    <span class="bwsprite unchecked-box" data-bind="click: personalDetails().terms, css: personalDetails().termsCheckbox ? 'checked': ''"></span>
                                                    <p>I agree with BikeWale sell bike <a href="" target="_blank">Terms & Conditions</a>, visitor agreement and privacy policy *. I agree that by clicking 'List your bike’ button, I am permitting buyers to contact me on my Mobile number.</p>
                                                </div>
                                            </div>
                                        
                                            <div class="panel-row">
                                                <input type="button" class="btn btn-orange btn-primary-big margin-right20" value="List your bike" data-bind="click: personalDetails().listYourBike" />
                                                <input type="button" class="btn btn-white btn-primary-small" value="Previous" data-bind="click: personalDetails().backToBikeDetails" />
                                            </div>

                                        </div>

                                        <div class="panel-body" data-bind="visible: formStep() == 2 && verificationDetails().status()">
                                            <p class="verify-title">Verification</p>
                                            <p class="verify-desc">We have just sent a 5 digit verification code on your mobile number.</p>

                                            <div data-bind="visible: !verificationDetails().updateMobileStatus()">
                                                <div class="panel-row margin-bottom35">
                                                    <div class="leftfloat">
                                                        <p class="font12 text-light-grey">Mobile number</p>
                                                        <p class="font16 text-bold" data-bind="text: personalDetails().sellerMobile"></p>
                                                    </div>
                                                    <div class="rightfloat bwsprite edit-blue-icon" data-bind="click: verificationDetails().updateSellerMobile"></div>
                                                    <div class="clear"></div>
                                                </div>

                                                <div class="panel-row margin-bottom10">
                                                    <div class="input-box form-control-box" data-bind="css: verificationDetails().otpCode().length > 0 ? 'not-empty' : ''">
                                                        <input type="tel" id="otpCode" maxlength="5" data-bind="textInput: verificationDetails().otpCode, validationElement: verificationDetails().otpCode" />
                                                        <label for="otpCode">One-time password<sup>*</sup></label>
                                                        <span class="boundary"></span>
                                                        <span class="error-text" data-bind="validationMessage: verificationDetails().otpCode"></span>
                                                    </div>
                                                </div>

                                                <div class="panel-row">
                                                    <input type="button" class="btn btn-orange btn-secondary-small margin-right20" value="Verify" data-bind="click: verificationDetails().verifySeller" />
                                                </div>
                                            </div>

                                            <div data-bind="visible: verificationDetails().updateMobileStatus()">
                                                <div class="panel-row margin-bottom20">
                                                    <div class="input-box input-number-box form-control-box" data-bind="css: verificationDetails().updatedMobile().length > 0 ? 'not-empty' : ''">
                                                        <input type="tel" maxlength="10" id="updatedMobile" data-bind="textInput: verificationDetails().updatedMobile, validationElement: verificationDetails().updatedMobile" />
                                                        <label for="updatedMobile">Mobile number<sup>*</sup></label>
                                                        <span class="input-number-prefix">+91</span>
                                                        <span class="boundary"></span>
                                                        <span class="error-text" data-bind="validationMessage: verificationDetails().updatedMobile"></span>
                                                    </div>
                                                </div>

                                                <div class="panel-row">
                                                    <input type="button" class="btn btn-orange btn-secondary-small" value="Done" data-bind="click: verificationDetails().submitUpdatedMobile" />
                                                </div>
                                            </div>

                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                    <div class="panel">
                                        <div class="panel-head">
                                            <span class="sell-bike-sprite" data-bind="css: (formStep() == 3) ? 'step-3-active' : 'step-3-inactive'"></span>
                                            <span class="panel-title">More details</span>
                                        </div>
                                        <div class="panel-body panel-body-3" data-bind="visible: formStep() == 3">
                                            <div class="panel-row margin-bottom40">
                                                <p class="font16 margin-bottom5 text-black">Add Photos</p>
                                                <p class="font14 text-light-grey margin-bottom20">Listings with photos are likely to get 3x more responses! The first photo will be made profile photo of your listing.<br />(Supported formats: .jpg, .png; Image Size < 4 MB and Image Count < 10).
                                                </p>
                                                <div id="add-photos-dropzone" class="dropzone dz-clickable">
                                                    <div class="dz-message">
                                                        <div id="dz-custom-message">
                                                            <span class="sell-bike-sprite add-photo-icon"></span><br />
                                                            <button type="button" class="btn btn-primary-big btn-orange margin-top20 margin-bottom15">Add photos</button>
                                                            <p class="font14 text-light-grey">Select images to upload. You can also drag and drop images</p>
                                                        </div>
                                                        <ul id="dz-image-placeholder" class="margin-top10">
                                                            <li></li>
                                                            <li></li>
                                                            <li></li>
                                                            <li></li>
                                                            <li></li>
                                                            <li></li>
                                                        </ul>
                                                        <div class="clear"></div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="panel-row margin-bottom20">
                                                <div class="input-box form-control-box" data-bind="css: moreDetails().registrationNumber().length > 0 ? 'not-empty' : ''">
                                                    <input type="text" id="registrationNumber" data-bind="textInput: moreDetails().registrationNumber" />
                                                    <label for="registrationNumber">Registration number<sup>*</sup></label>
                                                    <span class="boundary"></span>
                                                </div>
                                            </div>

                                            <div class="panel-row">
                                                <div class="select-box select-box-no-input">
                                                    <p class="select-label">Insurance<sup>*</sup></p>
                                                    <select class="chosen-select" data-bind="chosen: {}" data-title="Insurance">
                                                        <option value></option>
                                                        <option value="1">Comprehensive</option>
                                                        <option value="2">Third Party</option>
                                                        <option value="3">No Insurance</option>
                                                    </select>
                                                    <span class="boundary"></span>
                                                </div>
                                            </div>

                                            <div class="panel-row margin-bottom30">
                                                <div class="textarea-box form-control-box">
                                                    <p class="textarea-label">Ad description</p>
                                                    <textarea rows="2" cols="20"></textarea>
                                                    <span class="boundary"></span>
                                                </div>
                                            </div>

                                            <div class="panel-row">
                                                <input type="button" class="btn btn-orange btn-primary-big margin-right20" value="Update my Ad" data-bind="click: moreDetails().updateAd" />
                                                <input type="button" class="btn btn-white btn-primary-small" value="No Thanks" data-bind="click: moreDetails().noThanks" />
                                            </div>

                                        </div>
                                    </div>
                                </div>
                                <!-- end of form steps -->
                                <div id="form-success" data-bind="visible: formStep() == 4">
                                    <div class="icon-outer-container rounded-corner50 text-center inline-block">
                                        <div class="icon-inner-container rounded-corner50">
                                            <span class="bwsprite thankyou-icon margin-top15"></span>
                                        </div>
                                    </div>
                                    <div class="success-text inline-block">
                                        <p class="font18 text-bold margin-bottom10">Congratulations!</p>
                                        <p class="font14">Your profile ID is 138462384. You can find and edit your ad later using this id. Your bike ad will be live after verification.</p>
                                    </div>
                                    <div id="form-success-btn-group">
                                        <input type="button" class="btn btn-orange btn-primary-small margin-right20" value="Done" />
                                        <input type="button" class="btn btn-white btn-primary-small" value="Edit my Ad" />
                                    </div>
                                </div>
                            </div>
                            <div id="sell-bike-right-col" class="grid-5 omega">
                                <div class="sell-bike-banner">
                                    <div id="sell-bike-banner-content">
                                        <div class="text-center">
                                            <span class="sell-bike-sprite banner-icon"></span>
                                        </div>
                                        <ul>
                                            <li>
                                                <span class="sell-bike-sprite buyers-icon"></span>
                                                <div class="feature-item inline-block">
                                                    <p class="feature-title">Genuine buyers</p>
                                                    <p>Over x million are online on BikeWale looking a used bike</p>
                                                </div>
                                            </li>
                                            <li>
                                                <span class="sell-bike-sprite cost-icon"></span>
                                                <div class="feature-item inline-block">
                                                    <p class="feature-title">Free of cost</p>
                                                    <p>You can upload your bike ad absolutely free</p>
                                                </div>
                                            </li>
                                            <li>
                                                <span class="sell-bike-sprite listing-icon"></span>
                                                <div class="feature-item inline-block">
                                                    <p class="feature-title">Listing is here to stay</p>
                                                    <p>Your bike ad will be visible to our users until you've sold your bike</p>
                                                </div>
                                            </li>
                                            <li>
                                                <span class="sell-bike-sprite message-icon"></span>
                                                <div class="feature-item inline-block">
                                                    <p class="feature-title">Get contact details of buyers</p>
                                                    <p>Keep yourself away from frenzy calls as we will send buyers details</p>
                                                </div>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                            <div class="clear"></div>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>


        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/src/frameworks.js?<%=staticFileVersion %>"></script>
        <script type="text/javascript">
            
        </script>
        <!-- #include file="/includes/footerBW.aspx" -->
        <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%= staticUrl != string.Empty ? "http://st2.aeplcdn.com" + staticUrl : string.Empty %>/src/common.min.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" src="/src/knockout.validation.js"></script>
        <script type="text/javascript" src="/src/dropzone.js"></script>
        <script type="text/javascript" src="/src/sell-bike.js"></script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />
        <!--[if lt IE 9]>
            <script src="/src/html5.js"></script>
        <![endif]-->

    </form>
</body>
</html>
