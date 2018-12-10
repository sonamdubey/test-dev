<%@ Page Language="C#" AutoEventWireup="false" Inherits="MobileWeb.Insurance.Default" Trace="false" Debug="false" %>
<%@ Import Namespace="Carwale.BL.Experiments" %>
<% SectionName = "Insurance"; IsNotHomePage = false;%>
<!DOCTYPE html>
<html itemscope itemtype="http://schema.org/WebPage">
<head>
    <%
        Title = "Buy or renew car insurance policy in India - CarWale";
        Description = "Buy or renew your car insurance policy. Get insurance premium quotes from CarWale. Calculate auto insurance premium in India.";
        Keywords = "Auto insurance, car insurance, renew car insurance, buy car insurance, insurance calculator, used car insurance, insurance policy, renew insurance policy online, get insurance quotes, know insurance premium, claim, carwale, commercial car insurance, car insurance quotes, online insurance, insurance for car.";
        Canonical = "https://www.carwale.com/insurance/";
        bool showExperimentalColor = ProductExperiments.IsShowExperimentalColor(CookiesCustomers.AbTest);
    %>
    <!-- #include file="../includes/global/head-script-old.aspx" -->

    <link rel="stylesheet" href="/static/m/css/insurance.css" type="text/css" >
    <link rel="stylesheet" href="/static/m/css/zebra_datepicker.css" type="text/css" >

</head>
<body class="bg-light-grey m-special-skin-body m-no-bg-color <%= (showExperimentalColor ? "btn-abtest" : "")%>">
    <!-- #include file="/m/includes/header.aspx" -->
    <section class="clearfix">
        <div class="container">
            <div class="newcars-banner-div">
                <!-- Top banner code starts here -->
                <h2 class="text-uppercase text-white text-center padding-top40">INSURANCE</h2>
            </div>
            <!-- Top banner code ends here -->
        </div>
    </section>

    <section class="clearfix">
        <div class="container grid-12">
            <div class="ins-pg-logo grid-12 alpha omega rounded-corner2 margin-minus30 position-rel">
                <!-- CarWale_Mobile_ROS_320x50 -->
                <div id='div-gpt-ad-1444731593680-0' class="content-box-shadow grid-12">
                    <script type='text/javascript'>
                        googletag.cmd.push(function () { googletag.display('div-gpt-ad-1444731593680-0'); });
                    </script>
                </div>
            </div>
        </div>
    </section>
    <!--Outer div starts here-->
    <section id="mainForm" class="container margin-top10">
        <div data-role="page">
            <!--Main container starts here-->
            <div id="main-container">
                <div class="grid-12">
                    <h2 class="text-uppercase text-center margin-top30 margin-bottom20 m-special-skin-text">Buy or Renew your Car Insurance</h2>
                    <form id="Form1" runat="server">
                        <div runat="server" id="divErrMsg" class="inner-container" style="margin-bottom: 150px;">
                            <asp:Label runat="server" ID="lblErrorMsg" Style="color: red; display: inline;"></asp:Label>
                        </div>
                        <div id="divInsuranceForm" runat="server" class="divInsuranceForm margin-bottom20">

                            <div class="text-black box content-box-shadow rounded-corner2 accordion open">
                                <h3 id="step1" class="stepStrip">Step 1: Type of policy <span class="icon edit-icon hide"></span></h3>
                                <!--Select Insurance Type section starts here-->
                                <div class="margin-top10 ins-tabs">
                                    <div class="content-inner-block-10" style="line-height: 15px;">
                                        <ul id="Ul1" class="type-tab-wrap" runat="server">
                                            <li id="rdRenew" class="rounded-corner5" value="1" onclick="rdRenew_Click()">
                                                <span class="policybtn policy-type renew-type inactive-radio-btn"></span>
                                                <span class="font14">I want to renew my car insurance</span>
                                            </li>
                                            <li class="rounded-corner5" id="rdNew" value="2" onclick="rdNew_Click()">
                                                <span class="policybtn policy-type new-type inactive-radio-btn"></span>
                                                <span class="font14">I am buying a brand new car</span>
                                            </li>
                                        </ul>
                                        <input id="hdn_radio" type="hidden" runat="server" value="" />
                                        <div class="clear"></div>
                                        <div class="font14 margin-bottom20 hide" style="color: red" id="errMsgInsType">Select Insurance Type*</div>
                                    </div>
                                </div>
                                <!--Select Insurance Type ends starts here-->
                            </div>

                            <div class="text-black box content-box-shadow rounded-corner2 accordion">
                                <h3 id="step2" class="stepStrip disabled">Step 2: Car details <span class="icon edit-icon hide"></span></h3>
                                <!--Form starts here-->
                                <div class="hide">
                                    <div class="content-inner-block-10">
                                        <h2 class="font14 text-center text-bold margin-top20 margin-bottom30 text-grey">My car details are</h2>

                                        <!--Select Car box starts here-->
                                        <div class="text-light-grey main-select-div form-control position-rel" id="insSelectCar" runat="server" onclick="openMakePopup();">
                                            <div class="selected-text font14">Select Car</div>
                                            <span class="select-box fa fa-angle-down" style="display: block;"></span>
                                        </div>
                                        <span class="hide position-rel pos-top5" id="errCar" style="color: red;"></span>
                                        <!--Select Car box ends here-->

                                        <!--List box for Make, Model & Version starts here-->
                                        <div>
                                            <div class="MakeDiv fixedSearchPopup hide">
                                                <div data-role="header" data-theme="b" class="ui-corner-top" data-icon="delete">
                                                    <div class="filterBackArrow padding-right30" onclick="hidepopup(this,'insSelectCar')">
                                                        <span class="cwmsprite back-long-arrow-left-white"></span>
                                                    </div>
                                                    <a href="javascript:void(0)" onclick="CloseWindow()" data-role="button" data-theme="b" data-icon="delete" data-iconpos="notext"></a>
                                                    <div class="filterTitle">Select Make</div>
                                                    <div class="clear"></div>
                                                </div>
                                                <div class="m-loading-popup">
                                                    <span class="m-defaultAlert-window"></span>
                                                    <span class="m-loading-icon"></span>
                                                    <div class="clear"></div>
                                                </div>
                                                <div class="search-box cross-box-wrap">
                                                    <span class="cross-box hide" id="search-cross" onclick="clearSearchText(this)">
                                                        <span class="cwmsprite cross-md-dark-grey"></span>
                                                    </span>
                                                    <input type="text" id="txtMake" class="form-control rounded-corner0 txtSearchBar" placeholder="Filter makes here..." />
                                                </div>
                                                <div class="popup_content">
                                                    <div class="ui-content">
                                                        <ul id="drpMake" data-role="listview" data-bind="template: { name: 'car-make-template', foreach: CarMakes }">
                                                        </ul>
                                                        <script type="text/html" id="car-make-template">
                                                            <li onclick="makeChanged(this);" data-bind='attr: { value: makeId, text: makeName }'>
                                                                <a href="javascript:void(0)"><span data-bind="text: makeName"></span></a>
                                                            </li>
                                                        </script>
                                                        <input id="hdn_Make" type="hidden" value="" runat="server" />
                                                    </div>
                                                </div>
                                                <input type="hidden" runat="server" id="hdn_MakeId" />
                                            </div>
                                            <!--List box for Make ends here-->

                                            <!--List box for Model starts here-->
                                            <div class="modelDiv fixedSearchPopup hide">
                                                <div data-role="header" data-theme="b" class="ui-corner-top" data-icon="delete">
                                                    <div class="filterBackArrow padding-right30" onclick="hidepopup(this,'')">
                                                        <span class="cwmsprite back-long-arrow-left-white"></span>
                                                    </div>
                                                    <a href="javascript:void(0)" onclick="CloseWindow()" data-role="button" data-theme="b" data-icon="delete" data-iconpos="notext"></a>
                                                    <div class="filterTitle">Select Model</div>
                                                    <div class="clear"></div>
                                                </div>
                                                <div>
                                                    <div class="m-loading-popup">
                                                        <span class="m-defaultAlert-window"></span>
                                                        <span class="m-loading-icon"></span>
                                                        <div class="clear"></div>
                                                    </div>
                                                    <div class="search-box cross-box-wrap">
                                                        <span class="cross-box hide" id="search-cross" onclick="clearSearchText(this)">
                                                            <span class="cwmsprite cross-md-dark-grey"></span>
                                                        </span>
                                                        <input type="text" id="txtModel" class="form-control rounded-corner0 txtSearchBar" placeholder="Filter models here..." />
                                                    </div>
                                                    <div class="popup_content hide">
                                                        <div class="ui-content">
                                                            <ul id="model" data-role="listview" data-bind="template: { name: 'car-model-template', foreach: CarModels }">
                                                            </ul>
                                                            <script type="text/html" id="car-model-template">
                                                                <li onclick="modelChanged(this);" data-bind='attr: { value: ModelId, text: ModelName }'>
                                                                    <a href="javascript:void(0)"><span data-bind="text: ModelName"></span></a>
                                                                </li>
                                                            </script>
                                                            <input id="hdn_Model" type="hidden" value="" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <input type="hidden" runat="server" id="hdn_ModelId" />
                                            </div>
                                            <!--List box for Model ends here-->

                                            <!--List box for Version starts here-->
                                            <div class="versionDiv fixedSearchPopup hide">
                                                <div data-role="header" data-theme="b" class="ui-corner-top" data-icon="delete">
                                                    <div class="filterBackArrow padding-right30" onclick="hidepopup(this,'')">
                                                        <span class="cwmsprite back-long-arrow-left-white"></span>
                                                    </div>
                                                    <a href="javascript:void(0)" onclick="CloseWindow()" data-role="button" data-theme="b" data-icon="delete" data-iconpos="notext"></a>
                                                    <div class="filterTitle">Select Version</div>
                                                    <div class="clear"></div>
                                                </div>
                                                <div class="m-loading-popup">
                                                    <span class="m-defaultAlert-window"></span>
                                                    <span class="m-loading-icon"></span>
                                                    <div class="clear"></div>
                                                </div>
                                                <div class="search-box cross-box-wrap">
                                                    <span class="cross-box hide" id="search-cross" onclick="clearSearchText(this)">
                                                        <span class="cwmsprite cross-md-dark-grey"></span>
                                                    </span>
                                                    <input type="text" id="txtVersion" class="form-control rounded-corner0 txtSearchBar" placeholder="Filter versions here..." />
                                                </div>
                                                <div class="popup_content hide">
                                                    <div class="ui-content">
                                                        <ul id="version" data-role="listview" data-bind="template: { name: 'car-version-template', foreach: CarVersions }">
                                                        </ul>
                                                        <script type="text/html" id="car-version-template">
                                                            <li onclick="versionChanged(this);" data-bind='attr: { value: ID, text: Name }'>
                                                                <a href="javascript:void(0)"><span data-bind="text: Name"></span></a>
                                                            </li>
                                                        </script>
                                                        <input id="hdn_VersionId" type="hidden" runat="server" value="" />
                                                        <input id="hdn_Version" type="hidden" value="" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                            <!--List box for Version ends here-->
                                        </div>

                                        <!--Select State box starts here-->
                                        <div class="text-light-grey margin-top20 main-select-div font14 form-control" id="insSelectState" onclick="OpenPopupState()" runat="server">
                                            <div class="selected-text">Select State</div>
                                            <span class="select-box fa fa-angle-down" style="display: block;"></span>
                                        </div>
                                        <span class="hide position-rel pos-top5" id="errState" style="color: red;"></span>
                                        <!--Select State box ends here-->

                                        <!--Select State List starts here-->
                                        <div>
                                            <div class="StateDiv fixedSearchPopup hide">
                                                <div data-role="header" data-theme="b" class="ui-corner-top" data-icon="delete">
                                                    <div class="filterBackArrow padding-right30" onclick="hidepopup(this,'insSelectState')">
                                                        <span class="cwmsprite back-long-arrow-left-white"></span>
                                                    </div>
                                                    <a href="javascript:void(0)" onclick="CloseWindow()" data-role="button" data-theme="b" data-icon="delete" data-iconpos="notext"></a>
                                                    <div class="filterTitle">Select State</div>
                                                    <div class="clear"></div>
                                                </div>
                                                <div class="m-loading-popup">
                                                    <span class="m-defaultAlert-window"></span>
                                                    <span class="m-loading-icon"></span>
                                                    <div class="clear"></div>
                                                </div>
                                                <div class="search-box cross-box-wrap">
                                                    <span class="cross-box hide" id="search-cross" onclick="clearSearchText(this)">
                                                        <span class="cwmsprite cross-md-dark-grey"></span>
                                                    </span>
                                                    <input type="text" id="txtState" class="form-control rounded-corner0 txtState" placeholder="Filter states here..." />
                                                </div>
                                                <div class="popup_content">
                                                    <div class="ui-content">

                                                        <ul id="state" data-role="listview" data-bind="template: { name: 'car-state-template', foreach: CarState }">
                                                        </ul>
                                                        <script type="text/html" id="car-state-template">
                                                            <li onclick="stateChanged(this);" data-bind='attr: { value: StateId, text: StateName }'>
                                                                <a href="javascript:void(0)"><span data-bind="text: StateName"></span></a>
                                                            </li>
                                                        </script>
                                                        <input id="hdn_StateId" type="hidden" runat="server" value="" />
                                                        <input id="hdn_StateName" type="hidden" runat="server" value="" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <!--Select City box starts here-->
                                        <div class="text-light-grey margin-top20 main-select-div font14 form-control" id="insSelectCity" onclick="OpenPopupCity(this)" runat="server">
                                            <div class="selected-text">Select City</div>
                                            <span class="select-box fa fa-angle-down" style="display: block;"></span>
                                        </div>
                                        <span class="hide position-rel pos-top5" id="errCity" style="color: red;"></span>
                                        <!--Select City box ends here-->

                                        <!--Select City list starts here-->
                                        <div class="CityDiv fixedSearchPopup hide">
                                            <div data-role="header" data-theme="b" class="ui-corner-top" data-icon="delete">
                                                <div class="filterBackArrow padding-right30" onclick="hidepopup(this,'insSelectCity')">
                                                    <span class="cwmsprite back-long-arrow-left-white"></span>
                                                </div>
                                                <a href="javascript:void(0)" onclick="CloseWindow()" data-role="button" data-theme="b" data-icon="delete" data-iconpos="notext"></a>
                                                <div class="filterTitle">Select City</div>
                                                <div class="clear"></div>
                                            </div>
                                            <div class="m-loading-popup">
                                                <span class="m-defaultAlert-window"></span>
                                                <span class="m-loading-icon"></span>
                                                <div class="clear"></div>
                                            </div>
                                            <div class="search-box cross-box-wrap">
                                                <span class="cross-box hide" id="search-cross" onclick="clearSearchText(this)">
                                                    <span class="cwmsprite cross-md-dark-grey"></span>
                                                </span>
                                                <input type="text" id="txtCity" class="form-control rounded-corner0 txtCity" placeholder="Filter cities here..." />
                                            </div>
                                            <div class="popup_content">
                                                <div class="ui-content">
                                                    <ul id="city" data-role="listview" data-bind="template: { name: 'car-city-template', foreach: CarCity }">
                                                    </ul>
                                                    <script type="text/html" id="car-city-template">
                                                        <li onclick="cityChanged(this);" data-bind='attr: { value: CityId, text: CityName }'>
                                                            <a href="javascript:void(0)"><span data-bind="text: CityName"></span></a>
                                                        </li>
                                                    </script>
                                                    <input id="hdn_CityId" type="hidden" runat="server" value="" />
                                                    <input id="hdn_CityName" type="hidden" runat="server" value="" />
                                                </div>
                                            </div>
                                        </div>
                                        <!--Select City list ends here-->

                                        <!--Car make year starts here-->
                                        <div class="car-month-year margin-top20 main-select-div font14 form-control hide" style="padding: 0;" id="divRegDate">
                                            <asp:TextBox data-role="none" CssClass="" data-inline="true" type="text" ID="txtRegDate" data-zdp_direction="-1" runat="server" placeholder="Registration date" />
                                            <input id="hdn_RegDate" type="hidden" runat="server" value="" />
                                        </div>
                                        <span class="hide position-rel pos-top5" id="errRegDate" style="color: red; display: inline;"></span>

                                        <div class="margin-top20 margin-bottom10 text-center">
                                            <input type="button" id="next-btn" value="NEXT" class="btn btn-orange" onclick="showDetails()" style="min-width: 160px;" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!-- Contact Details starts here -->
                            <div class="text-black box content-box-shadow rounded-corner2 accordion">
                                <h3 id="step3" class="stepStrip disabled">Step 3: My details <span class="icon edit-icon hide"></span></h3>
                                <div class="hide">
                                    <div class="content-inner-block-10">
                                        <p class="font14 text-grey margin-top10 margin-bottom10 text-center padding-left30 padding-right30 line-height">Please share your details to get assistance</p>
                                        <div class="margin-top10 main-select-div font14 form-control">
                                            <asp:TextBox ID="txtName" MaxLength="30" runat="server" type="text" placeholder="Enter Name*" data-role="none" />
                                        </div>
                                        <span class="hide position-rel pos-top5" id="errName" style="color: red;"></span>
                                        <div class="margin-top10 main-select-div font14 form-control">
                                            <asp:TextBox ID="txtEmail" MaxLength="50" type="email" placeholder="Enter Email*" runat="server" data-role="none" Enabled="true" />
                                        </div>
                                        <span class="hide position-rel pos-top5" id="errEmail" style="color: red;"></span>
                                        <div class="margin-top10 main-select-div font14 form-control">
                                            <asp:TextBox ID="txtMobile" type="tel" MaxLength="10" placeholder="Enter Mobile Number*" runat="server" data-role="none" />
                                            <div class="cw-m-sprite wrong floatright"></div>
                                        </div>
                                        <span class="hide position-rel pos-top5" id="errMobile" style="color: red;"></span>
                                        <div class="text-center margin-top20 margin-bottom20">
                                            <asp:Button data-role="click-tracking" data-event="CWInteractive" data-action="Submit_clicked" data-cat="Insurance OTP_lead drop_m" data-label="Submit_clicked" ID="btnCalculate" CssClass="text-uppercase btn btn-orange" Text="Submit" runat="server" OnClientClick="javascript:if(form_Submit()==false)return false;" />
                                        </div>
                                        <p class="text-grey font14 margin-bottom10 line-height">By entering your information, you accept that we or our Insurance partner may contact you regarding your query.</p>
                                    </div>
                                </div>
                            </div>
                            <!-- Contact Details ends here -->

                        </div>
                        <!--Form ends here-->
                    </form>
                </div>
                <div class="clear"></div>
            </div>
            <!--Main container ends here-->
        </div>
    </section>

    <div class="clear"></div>
    <div id="thankYou1" class="content-box-shadow text-center padding-top10 padding-bottom10  text-center padding-left20 padding-right20 hide">
        <p class="font14 text-black text-bold margin-top10 margin-bottom20">Thank you for submitting your details.</p>
        <p class="font18 text-black text-bold margin-top10 margin-bottom20">Insurance offers starting from
            <br>
            ₹<strong class="font18" id="premiumAmt">NA</strong>.</p>
    </div>
    <div id="thankYou2" class="content-box-shadow text-center padding-top10 padding-bottom10  text-center padding-left20 padding-right20 hide">
        <p class="font14 text-black text-bold margin-top10 margin-bottom20">Thank you for providing your details. </p>
        <p id="clientMsg" class="font14 text-black text-bold margin-top10 margin-bottom20"><%= insuranceThanksMsg %></p>
    </div>

    <!--OTP Verification-->
    <div id="m-blackOut-window" class="hide" ></div>
    <div id="buyerForm" class="buyerProcess padding-top10 text-center hide">
        <span id="otp-close" class="cur-pointer cwmsprite cross-lg-dark-grey otp-close"></span>
        <div class="screen2 hide">
            <div class="upeer-box">
                <h2 class="text-black text-bold">Enter OTP here</h2>
                <p class="font12 text-light-grey padding-bottom10">(you will receive OTP within 5 sec.)</p>
                <div class="position-rel">
                    <input type="tel" class="input-txt padding-right60" placeholder="Enter 5 digit OTP" id="txtOTP" maxlength="5">
                    <a class="font14 text-uppercase text-link padding-top5 position-abt pos-right0 pos-top0" data-role="click-tracking" data-event="CWInteractive" data-action="Verify_clicked" data-cat="Insurance OTP_lead drop_m" data-label="Verify_clicked" id="btnVerify">verify</a>
                    <img id="imgLoadingBtnVerify" class="position-abt hide" src="https://imgd.aeplcdn.com/0x0/statics/loader.gif" width="16" height="16">
                </div>
                <div class="error hide padding-top5 padding-bottom5" id="otpError">Please enter 5 digit OTP</div>
            </div>
            <div class="otp-call">
                <div class="buyer-or-text">
                    <div><span>or</span></div>
                </div>
                <p class="font14 padding-bottom10">Give a missed call for verification</p>
                <a id="btnMissCallAnchorTag" class="btn btn-green font24 buyer-call-btn" data-role="click-tracking" data-event="CWInteractive" data-action="MissCall_clicked" data-cat="Insurance OTP_lead drop_m" data-label="MissCall_clicked" >
                    <span class="fa fa-phone margin-right10 position-rel pos-top3"></span><span id="btnMissCall" class="font18"></span>
                </a>
                <img id="imgLoadingBtnMissCall" src="https://imgd.aeplcdn.com/0x0/statics/loader.gif" class="position-rel pos-top5 pos-left5 hide" width="16" height="16">
            </div>
        </div>
    </div>


    <section class="container margin-bottom20">
        <div class="grid-12 faqs">
            <h2 class="text-uppercase text-center margin-top30 margin-bottom20 m-special-skin-text">FAQs</h2>
            <div id="faqs" class="clear">
                <div class="text-black box content-box-shadow rounded-corner2 accordion">
                    <h3 class="stepStrip">What is car insurance? <span class="icon plus-minus"><span class="fa fa-plus"></span></span></h3>
                    <div class="hide">
                        <p class="content-inner-block-10 font14 text-grey line-height">
                            Car insurance provides protection to the 
                        car's owner against third-party liabilities, 
                        theft or damage to the car. It also 
                        provides cover against accidental injuries. 
                        There are two types of car insurance. 1. 
                        Third Party only. 2. Comprehensive or 
                        Package policy.
                        </p>
                    </div>
                </div>
                <div class="text-black box content-box-shadow rounded-corner2 accordion">
                    <h3 class="stepStrip">Is insurance mandatory to have?<span class="icon plus-minus"><span class="fa fa-plus"></span></span></h3>
                    <div class="hide">
                        <p class="content-inner-block-10 font14 text-grey line-height">
                            As per law, it's mandatory to have third-
                            party insurance for your car. Opting for a 
                            comprehensive policy is a matter of choice. 
                            CarWale recommends you a comprehensive
                             policy, though.
                        </p>

                    </div>
                </div>
                <div class="text-black box content-box-shadow rounded-corner2 accordion">
                    <h3 class="stepStrip">Benefits of comprehensive car insurance?<span class="icon plus-minus"><span class="fa fa-plus"></span></span></h3>
                    <div class="hide">
                        <p class="content-inner-block-10 font14 text-grey line-height">
                            Comprehensive car insurance not only 
                            protects you against third party liabilities 
                            but also helps you avoid unnecessary 
                            expenses that might occur due to accidents 
                            or theft.
                        </p>
                    </div>
                </div>
                <div class="text-black box content-box-shadow rounded-corner2 accordion">
                    <h3 class="stepStrip">What is No Claim Bonus (NCB)?<span class="icon plus-minus"><span class="fa fa-plus"></span></span></h3>
                    <div class="hide">
                        <p class="content-inner-block-10 font14 text-grey line-height">
                            It is a benefit offered by the insurer to 
                            those who have not claimed insurance 
                            during the previous year of cover. It means 
                            that the next premium amount to be paid 
                            would be lower.
                        </p>
                    </div>
                </div>
            </div>
        </div>
        <div class="clear"></div>
    </section>
    <div class="extraDivHt"></div>
    <section id="contact-fixed" class="container <%= string.IsNullOrEmpty(cholaAssitanceNo) ? "hide":"" %>">
        <a href="tel:+91<%=cholaAssitanceNo%>" class="text-white font16" style="text-decoration: none;">
            <div class="btn btn-green btn-full-width text-bold font24"><span class="fa fa-phone position-rel"></span><span class="font18 padding-left5">Call for premium quote</span></div>
        </a>
    </section>
    <!--Outer div ends here-->
    <!-- #include file="/m/includes/footer.aspx" -->
    <!-- #include file="/m/includes/global/footer-script.aspx" -->

    <link rel="stylesheet" href="/static/m/css/jquery.mobile-1.4.2.min.css" type="text/css" >
    <script   src="/static/m/js/insurance.js" ></script>
    <script  type="text/javascript"  src="/static/src/ajaxfunctions.js" ></script>
    <script  type="text/javascript"  src="/static/m/js/contactdetailsvalidation.js" ></script>
    <script   src="/static/m/js/zebra_datepicker.js" ></script>
    <script>
        $("#hdn_StateId").val("<%=hdn_StateId.Value%>");
        $("#hdn_CityId").val("<%=hdn_CityId.Value%>");
        $("#hdn_VersionId").val("<%=hdn_VersionId.Value%>");        
        var clientId = <%=System.Configuration.ConfigurationManager.AppSettings["InsuranceClient"]??"0"%>;
    </script>
    <style type="text/css">
        .ui-content {
            padding: 0;
        }
    </style>
</body>
</html>
