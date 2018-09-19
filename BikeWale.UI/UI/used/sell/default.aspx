<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="Default.aspx.cs" Inherits="Bikewale.Used.Sell.Default" EnableViewState="false" %>

<!DOCTYPE html>

<html>
<head>
    <%
        title = "Sell Bike | Sell Used Bike in India - BikeWale";
        description = "Sell your used/second-hand bike on BikeWale for free. Post your Ad for free and get verified buyers on BikeWale. Easy, Quick & Effective.";
        keywords = "sell bike, bike sale, used bike sell, second-hand bike sell, sell bike India, list your bike";
        canonical = "https://www.bikewale.com/used/sell/";
        alternate = "https://www.bikewale.com/m/used/sell/";
        AdId = "1475577527140";
        AdPath = "/1017752/BikeWale_UsedSellBikes_";
        isAd300x250Shown = false;
        isAd300x250BTFShown = false;
        isAd970x90Shown = false;
%>
    <!-- #include file="/UI/includes/headscript_desktop_min.aspx" -->
    <link href="<%= staticUrl %>/UI/sass/sell-bike/sell-bike.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
    <link href="<%= staticUrl %>/UI/css/zebra-datepicker.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
    <link href="<%= staticUrl %>/UI/css/dropzone.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        <!-- #include file="\UI\includes\gacode_desktop.aspx" -->
    </script>
    
</head>
<body class="bg-light-grey header-fixed-inner" data-contestslug="true">
    <form id="form1" runat="server">
        <!-- #include file="/UI/includes/headBW.aspx" -->

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
                            <h1>Sell Your Used Bike - Post Ad for FREE</h1>
                        </div>
                        <div id="sell-bike-content">
                        
                            <div id="sell-bike-left-col" class="grid-7 panel-group">
                                <!-- start of form steps -->

                                <% if(isAuthorized) { %>
                            <div data-bind="if: !isFakeCustomer()">

                                <div data-bind="visible: formStep() < 4">
                                    <div class="panel panel-divider">
                                        <div class="panel-head">
                                            <span class="sell-bike-sprite" data-bind="click: gotoStep1, css: formStep() == 1 ? 'step-1-active' : 'edit-step'"></span>
                                            <span class="panel-title">Bike details</span>
                                        </div>
                                        <div class="panel-body" data-bind="visible: formStep() == 1, with: bikeDetails ">
                                            <div class="panel-row margin-bottom10">
                                                <div id="make-select-element" class="grid-4 alpha select-box">
                                                    <p class="select-label">Make<sup>*</sup></p>
                                                    <select class="chosen-select" data-placeholder="Select make" data-bind="chosen: {width: '100%'}, value: make, validationElement: make, event: { change: makeChanged }">
                                                         <option value></option>
                                                            <% if (objMakeList != null)
                                                                { %>
                                                                 <% foreach (var make in objMakeList)
                                                                { %>
                                                                    <option value="<%= make.MakeId %>" data-masking="<%= make.MaskingName %>"><%=make.MakeName %></option>
                                                             <% } %>
                                                             <% } %>
                                                    </select>
                                                    <span class="boundary"></span>
                                                    <span class="error-text" data-bind="validationMessage: make"></span>
                                                </div>
                                                <div id="model-select-element" class="grid-4 select-box">
                                                    <p class="select-label">Model<sup>*</sup></p>
                                                    <select class="chosen-select" data-placeholder="Select model" data-bind="options: modelArray(), chosen: { width: '100%' }, value: model, optionsText: 'modelName', optionsValue: 'modelId', validationElement: model, event: { change: modelChanged }" disabled>                                                       
                                                    </select>
                                                    <span class="boundary"></span>
                                                    <span class="error-text" data-bind="validationMessage: model"></span>
                                                </div>
                                                <div id="version-select-element" class="grid-4 omega select-box">
                                                    <p class="select-label">Version<sup>*</sup></p>                                                                                                 
                                                    <select class="chosen-select" data-placeholder="Select version" data-bind="options: versionArray(), chosen: { width: '100%' }, value: version, optionsText: 'versionName', optionsValue: 'versionId', validationElement: version, event: { change: versionChanged }" disabled>  
                                                    </select>                                                   
                                                    <span class="boundary"></span>
                                                    <span class="error-text" data-bind="validationMessage: version"></span>
                                                </div>
                                                <div class="clear"></div>
                                            </div>

                                            <div class="panel-row margin-bottom20">
                                                <div class="input-box form-control-box input-no-focus" data-bind="css: manufacturingDate().length > 0 ? 'not-empty' : ''">
                                                    <input type="text" id="manufacturingDate" data-bind="textInput: manufacturingDate, validationElement: manufacturingDate" />
                                                    <label for="manufacturingDate">Year of manufacturing<sup>*</sup></label>
                                                    <span class="boundary"></span>
                                                    <span class="error-text" data-bind="validationMessage: manufacturingDate"></span>
                                                </div>
                                            </div>

                                            <div class="panel-row margin-bottom20">
                                                <div id="div-kmsRidden" class="input-box form-control-box" data-bind="css: kmsRidden().length > 0 ? 'not-empty' : ''">
                                                    <input type="number" id="kmsRidden" min="1" data-bind="textInput: kmsRidden, validationElement: kmsRidden" />
                                                    <label for="kmsRidden">Kms ridden<sup>*</sup></label>
                                                    <span class="boundary"></span>
                                                    <span class="error-text" data-bind="validationMessage: kmsRidden"></span>
                                                </div>
                                            </div>

                                            <div class="panel-row margin-bottom20">
                                                <div id="city-select-element" class="select-box">
                                                    <p class="select-label">City<sup>*</sup></p>
                                                    <select class="chosen-select" data-placeholder="Select city" data-bind="chosen: { width: '100%' }, value: city, validationElement: city">
                                                        <option value></option>
                                                        <% if (objCityList != null)
                                                                { %>
                                                                 <% foreach (var city in objCityList)
                                                                { %>
                                                                    <option value="<%= city.CityId %>" ><%=city.CityName %></option>
                                                             <% } %>
                                                             <% } %>
                                                    </select>
                                                    <span class="boundary"></span>
                                                    <span class="error-text" data-bind="validationMessage: city"></span>
                                                </div>
                                            </div>

                                            <div class="panel-row margin-bottom20">
                                                <div id="div-expectedPrice" class="input-box form-control-box" data-bind="css: expectedPrice().length > 0 ? 'not-empty' : ''">
                                                    <input type="number" min="1" id="expectedPrice" data-bind="textInput: expectedPrice, validationElement: expectedPrice" />
                                                    <label for="expectedPrice">Expected price<sup>*</sup></label>
                                                    <span class="boundary"></span>
                                                    <span class="error-text" data-bind="validationMessage: expectedPrice"></span>
                                                </div>
                                            </div>

                                            <div class="panel-row margin-bottom20">
                                                <div class="select-box select-box-no-input">
                                                    <p class="select-label">Owner<sup>*</sup></p>
                                                    <select class="chosen-select" data-bind="chosen: { width: '100%' }, value: owner, validationElement: owner" data-title="Owner">
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
                                            </div>

                                            <div class="panel-row margin-bottom20">
                                                <div class="select-box">
                                                    <p class="select-label">Bike registered at<sup>*</sup></p>
                                                    <select id="select-registeredCity" class="chosen-select" data-placeholder="Select city" data-bind="chosen: { width: '100%' }, value: registeredCity, validationElement: registeredCity">
                                                        <option value></option>
                                                        <% if (objCityList != null)
                                                                { %>
                                                                 <% foreach (var city in objCityList)
                                                                { %>
                                                                    <option value="<%= city.CityName %>" ><%=city.CityName %></option>
                                                             <% } %>
                                                             <% } %>
                                                    </select>
                                                    <span class="boundary"></span>
                                                    <span class="error-text" data-bind="validationMessage: registeredCity"></span>
                                                </div>
                                            </div>

                                            <div class="panel-row margin-bottom10">
                                                <div class="color-box-content">
                                                    <div id="select-color-box" class="select-color-box">
                                                        <p class="select-color-label color-box-default">Colour<sup>*</sup></p>
                                                        <p id="selected-color" class="color-box-default" data-bind="text: color, validationElement: color "></p>
                                                        <span class="boundary"></span>
                                                        <span class="error-text" data-bind="validationMessage: color"></span>

                                                        <div class="color-dropdown">
                                                            <p class="dropdown-label">Colour</p>
                                                            <ul data-bind="foreach: colorArray" >
                                                             <li class="color-list-item" data-bind="click: $parent.colorSelection">                                                                
                                                                    <div class="color-box " data-bind="foreach: hexCode, css: (hexCode.length >= 3) ? 'color-count-three' : (hexCode.length == 2) ? 'color-count-two' : 'color-count-one' ">
                                                                    <span data-bind="style: { 'background-color': '#' + $data }"></span>
                                                                    </div>

                                                                 <p class="color-box-label" data-bind="text: colorName , attr: {value: colorId}"></p>      
    
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

                                                            <div class="text-center padding-bottom20" data-bind="visible: otherColor().length > 0">
                                                                <button type="button" class="btn btn-orange btn-secondary-small" data-bind="click: submitOtherColor">Done</button>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="panel-row">
                                                <input type="button" id="btnSaveBikeDetails" class="btn btn-orange btn-primary-big bw-ga" value="Save and Continue" data-bind="click: saveBikeDetails" />
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
                                            <div class="panel-row margin-bottom30" data-bind="style: {'pointer-events' : personalDetails().isEdit() ? 'none' : ''}">
                                                <ul id="seller-type-list">
                                                    <li data-bind="click: personalDetails().sellerType, attr: { value: 2 }, css: personalDetails().sellerTypeVal() == 2 ? 'checked' : ''" >
                                                        <span class="bwsprite radio-icon"></span>
                                                        <span class="seller-label">I am an Individual</span>
                                                    </li>
                                                    <li data-bind="click: personalDetails().sellerType, attr: { value: 1 }, css: personalDetails().sellerTypeVal() == 1 ? 'checked' : ''">
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
                                                    <p>I agree with BikeWale sell bike <a href="/TermsConditions.aspx" target="_blank" rel="noopener">Terms & Conditions</a>, <a target="_blank" rel="noopener" href="/visitoragreement.aspx">visitor agreement</a> and <a target="_blank" rel="noopener" href="/privacypolicy.aspx">privacy policy</a> *. I agree that by clicking 'List your bike’ button, I am permitting buyers to contact me on my Mobile number.</p>
                                                    <span class="error-text" data-bind="validationMessage: personalDetails().termsCheckbox"></span>
                                                </div>
                                            </div>
                                        
                                            <div class="panel-row">
                                                <input type="button" id ="btnListBike" class="btn btn-orange btn-primary-big margin-right20" value="List your bike" data-bind="click: personalDetails().listYourBike" />
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
                                                        <span id="otpErrorText" class="error-text" data-bind="validationMessage: verificationDetails().otpCode"></span>
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
                                        <div class="panel-head" id="uploadphoto">
                                            <span class="sell-bike-sprite" data-bind="css: (formStep() == 3) ? 'step-3-active' : 'step-3-inactive'"></span>
                                            <span class="panel-title">More details</span>
                                        </div>
                                        <div class="panel-body panel-body-3" data-bind="visible: formStep() == 3">
                                            <div class="panel-row margin-bottom40">
                                                <p class="font16 margin-bottom5 text-black">Add Images</p>
                                                <p class="font14 text-light-grey margin-bottom20">Ads with images are likely to get 50% more responses! You can upload upto 10 photos with first photo being the profile photo for the ad.<br />Supported formats: .jpg, .png; Image Size < 4 MB
                                                </p>
                                                <div id="add-photos-dropzone" class="dropzone dz-clickable">
                                                    <div class="dz-message">
                                                        <div id="dz-custom-message">
                                                            <span class="sell-bike-sprite add-photo-icon"></span><br />
                                                            <button type="button" class="btn btn-primary-big btn-orange margin-top20 margin-bottom15">Add images</button>
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
                                                    <label for="registrationNumber">Registration number</label>
                                                    <span class="boundary"></span>
                                                </div>
                                            </div>

                                            <div class="panel-row">
                                                <div class="select-box select-box-no-input">
                                                    <p class="select-label">Insurance</p>
                                                    <select id="select-insuranceType" class="chosen-select" data-bind="chosen: {}, value: moreDetails().insuranceType" data-title="Insurance">
                                                        <option value></option>
                                                        <option value="Comprehensive">Comprehensive</option>
                                                        <option value="Third Party">Third Party</option>
                                                        <option value="No Insurance">No Insurance</option>
                                                    </select>
                                                    <span class="boundary"></span>
                                                </div>
                                            </div>

                                            <div class="panel-row margin-bottom30">
                                                <div class="textarea-box form-control-box">
                                                    <p class="textarea-label">Ad description</p>
                                                    <textarea maxlength="250" rows="2" cols="20" data-bind=" value: moreDetails().adDescription "></textarea>
                                                    <span class="boundary"></span>
                                                </div>
                                            </div>

                                            <div class="panel-row">
                                                <input type="button" id="btnUpdateAd" class="btn btn-orange btn-primary-big margin-right20" value="Update my Ad" data-bind="click: moreDetails().updateAd" />
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
                                        <% if(!isEdit) { %>
                                        <p class="font14">Your profile ID is <span data-bind="text: profileId"></span>. You can find and edit your ad later using this id. Your bike ad will be live after verification.</p>
                                        <% } else { %>
                                        <p class="font14">Your changes have been recorded. Your ad will be live after verification.</p>
                                        <% } %>
                                    </div>
                                    <div id="form-success-btn-group">
                                        <input type="button" class="btn btn-orange btn-primary-small margin-right20" value="Done" data-bind="click: congratsScreenDoneFunction" />
                                        <input  type="button" id ="btnEditAd" class="btn btn-white btn-primary-small" value="Edit my Ad " data-bind="click: editMyAd" />
                                    </div>
                                </div>

                            </div>
                            <div data-bind="if: isFakeCustomer()">
                                <div class="icon-outer-container rounded-corner50 text-center inline-block">
                                    <div class="icon-inner-container rounded-corner50">
                                        <span class="sell-bike-sprite no-auth-edit-icon margin-top20"></span>
                                    </div>
                                </div>
                                <div class="margin-left20 inline-block">
                                    <p class="font18 text-bold margin-bottom10">Sorry!</p>
                                    <p class="font14">You are not authorised to edit this listing</p>
                                </div>
                            </div>
                        <%} else { %>
                            <div data-bind="if: isFakeCustomer()">
                                <div class="icon-outer-container rounded-corner50 text-center inline-block">
                                    <div class="icon-inner-container rounded-corner50">
                                        <span class="sell-bike-sprite no-auth-icon margin-top20"></span>
                                    </div>
                                </div>
                                <div class="margin-left20 inline-block">
                                    <p class="font18 text-bold margin-bottom10">Sorry!</p>
                                    <p class="font14">You are not authorised to add any listing.<br />Please contact us at <a href="mailto:contact@bikewale.com">contact@bikewale.com</a></p>
                                </div>
                            </div>
                        <% } %>
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
                                                    <p>Over 3.5 million users on BikeWale are looking a used bike</p>
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
                                                    <p class="feature-title">Unlimited listing duration</p>
                                                    <p>Your bike ad will be visible to our users until you've sold your bike</p>
                                                </div>
                                            </li>
                                            <li>
                                                <span class="sell-bike-sprite message-icon"></span>
                                                <div class="feature-item inline-block">
                                                    <p class="feature-title">Get contact details of buyers</p>
                                                    <p>We will send you the details of buyers through SMS and mail</p>
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
        <section>
            <div class="container margin-bottom20">
                <h2 class="section-heading">FAQs- Selling Bike on BikeWale</h2>
                <div class="grid-12">
                    <div class="content-box-shadow">
                        <ul class="accordion-list">
                            <li>
	                            <div class="accordion-head">
		                            <p class="accordion-head-title">Why should you sell your second hand bike on BikeWale?</p>
		                            <span class="bwsprite arrow-sm-down"></span>
	                            </div>                        
	                            <div class="accordion-body" style="overflow: hidden; display: none;">
		                            <p>Every month more than 3.5+ million users visit BikeWale to research about used and new bikes. BikeWale has 5,000+ listings posted across more than 200 cities in India. BikeWale doesn’t charge any fee for posting your Ad. You can post your Ad for FREE. There are no limits on the number of postings.
BikeWale ensures that only verified buyers can reach out to you. You can re-post your Ad after 90 days if your bike is not sold. It’s Easy, Quick and Reliable!</p>
	                            </div>
                            </li>
                            <li>
	                            <div class="accordion-head">
		                            <p class="accordion-head-title">How to post an effective used bike Ad on BikeWale to reduce the selling period?</p>
		                            <span class="bwsprite arrow-sm-down"></span>
	                            </div>                        
	                            <div class="accordion-body">
		                            <p>To sell your bike quickly, you should provide correct and comprehensive description about your bike. It is recommended to upload more than 5 high resolution images to make the Ad more effective. The registration city, colors, and make year should be provided with best of your knowledge. You should look out for price of similar bikes and provide a reasonable and competitive pricing to get better responses. Once you provide all the above mentioned information, you are very likely to sell your bike within few days from date of posting.</p>
	                            </div>
                            </li>
                            <li>
	                            <div class="accordion-head">
		                            <p class="accordion-head-title">How long will your bike be posted on BikeWale?</p>
		                            <span class="bwsprite arrow-sm-down"></span>
	                            </div>                        
	                            <div class="accordion-body">
		                            <p>Your Ad will be posted for 90 days. If you are not able to sell your bike till then, you can re-post your Ad for FREE. Your bike will be made available again to the buyers.</p>
	                            </div>
                            </li>
							<li>
	                            <div class="accordion-head">
		                            <p class="accordion-head-title">Can the details of posted Ad be changed/edited?</p>
		                            <span class="bwsprite arrow-sm-down"></span>
	                            </div>                        
	                            <div class="accordion-body">
		                            <p>Yes, you can edit your listing by logging in your account. However, we have restricted certain fields like bike make, models, city etc. for the ease of buyers. Your bike will again be made available after approval from BikeWale team and you can continue selling your used bike on BikeWale.</p>
	                            </div>
                            </li>
							<li>
	                            <div class="accordion-head">
		                            <p class="accordion-head-title">What can improve the number of responses for your listed bike?</p>
		                            <span class="bwsprite arrow-sm-down"></span>
	                            </div>                        
	                            <div class="accordion-body">
		                            <p>You should provide comprehensive and correct details of your bike and upload multiple images of good quality to improve the responses from buyers. You should provide a reasonable and competitive pricing to get better results. Once you provide the above mentioned details correctly, you can sit back and relax. Your bike will likely be sold within few days of posting.</p>
	                            </div>
                            </li>
							<li>
	                            <div class="accordion-head">
		                            <p class="accordion-head-title">How can you remove your second hand bike listing from BikeWale?</p>
		                            <span class="bwsprite arrow-sm-down"></span>
	                            </div>                        
	                            <div class="accordion-body">
		                            <p>It is very easy to remove/delete your listing from BikeWale. You can remove/delete your listing from BikeWale after logging in your myBikeWale account and your bike will no more be available on BikeWale for sell. If you don’t remove the listing, your Ad will be automatically removed after 90 days from the date of posting.</p>
	                            </div>
                            </li>
                        </ul>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <script type="text/javascript" src="<%= staticUrl  %>/UI/src/frameworks.js?<%=staticFileVersion %>"></script>

        <script type="text/javascript"> 
            var userId = '<%= userId%>';    
            var isEdit = '<%= isEdit %>';
            var inquiryId = '<%= inquiryId %>';
            var isAuthorized = '<%= isAuthorized%>';
            var inquiryDetailsJSON = '<%= Newtonsoft.Json.JsonConvert.SerializeObject(inquiryDTO) %>';
            var userName = '<%= userName%>';
            var userEmail = '<%= userEmail%>';
            var imgEnv = "<%= Bikewale.Utility.BWConfiguration.Instance.AWSEnvironment %>";
            var gaObj = { 'id': '<%= (int)Bikewale.Entities.Pages.GAPages.Sell_Page%>', 'name': '<%= Bikewale.Entities.Pages.GAPages.Sell_Page%>' };
        </script>
        
        <!-- #include file="/UI/includes/footerBW.aspx" -->
        <link href="<%= staticUrl %>/UI/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
        <!-- #include file="/UI/includes/footerscript.aspx" -->
        <script type="text/javascript" src="<%= staticUrl %>/UI/src/knockout.validation.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" src="<%= staticUrl %>/UI/src/zebra-datepicker.js?<%=staticFileVersion %>"></script>
        <script type="text/javascript" src="<%= staticUrl %>/UI/src/imageUpload.js?<%=staticFileVersion %>"></script>
        <script type="text/javascript" src="<%= staticUrl %>/UI/src/dropzone.js?<%=staticFileVersion %>"></script>
        <% if(isAuthorized) { %>
        <script type="text/javascript" src="<%= staticUrl %>/UI/src/sell-bike.js?<%=staticFileVersion %>"></script>
        <%} %>
        <!-- #include file="/UI/includes/fontBW.aspx" -->
        <script type="text/javascript">

            window.onbeforeunload = function () {
                if (vmSellBike.formStep() < 4) {
                    return true;
                }                
            }
        </script>
        <style type="text/css">
            .select-box .chosen-container .chosen-drop {
            width:300px;
            }
            </style>
    </form>
</body>
</html>
