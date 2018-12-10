<%@ Import Namespace="Carwale.Utility" %>
<%@ Import Namespace="Carwale.UI.Common" %>
<%@ Import Namespace="Carwale.UI.Users" %>
<%@ Import Namespace="Carwale.UI.PresentationLogic" %>
<%@ Import Namespace="System.Web.Script.Serialization" %>
<%@ Import Namespace="Carwale.UI.NewCars.RecommendCars" %>
<%@ Import Namespace="Carwale.BL.Experiments" %>

<script language="c#" runat="server">
    public string CurrentUserId = CurrentUser.Id;
    bool IsLoggedIn = CurrentUser.Id != "-1" ? true : false;
    //public CustomerDetails CurrentCustomer = new CustomerDetails(CurrentUser.Id);
    public bool landingPage = false;
    bool showAdvantageDiscountLink = Advanatge.ShowAdvantageDiscountLink;
    //static int[] NewMenuCategories = { 1, 3, 5, 7, 9 };
    bool IsEligibleForNewMenu = true; //Array.Exists(NewMenuCategories, element => element == CookiesCustomers.AbTest);    
    public int[] testDriveCampaignIds = SponsoredCampaignLogic.TestDriveCampaignIds;
    bool isMobile = Carwale.UI.ClientBL.DeviceDetectionManager.IsMobile(new HttpContextWrapper(HttpContext.Current));    
</script>
<script type="text/javascript">
    landingPage = <%=landingPage.ToString().ToLower()%>
    testDriveCampaignIds = <%=new JavaScriptSerializer().Serialize(testDriveCampaignIds)%>
</script>
<script type="text/javascript">
    var abTestValue = "<%= Carwale.Utility.CustomerCookie.AbTest%>";
    var abTestMinValForNewPqDesktop = "<%= CustomParser.parseIntObject(System.Configuration.ConfigurationManager.AppSettings["AbTestMinValForNewPqDesktop"])%>";
    var abTestMaxValForNewPqDesktop = "<%= CustomParser.parseIntObject(System.Configuration.ConfigurationManager.AppSettings["AbTestMaxValForNewPqDesktop"])%>";
</script>

<div id="globalPopupBlackOut" class="blackOut-window"></div>
<nav id="nav">
    <!-- nav code starts here -->
    <ul class="navUL">
        <li>
            <a href="/" class="open">
                <span class="cwsprite home-icon"></span>
                <span class="navbarTitle">Home</span>
            </a>
        </li>
        <li id="navNewCars">
            <a href="javascript:;" class="newCarDropDown">
                <span class="cwsprite newCars-icon"></span>
                <span class="navbarTitle">New Cars</span>
                <span class="nav-drop fa fa-angle-down"></span>
            </a>
            <ul class="nestedUL">
                <li><a href="/new/">Find New Cars</a></li>
                <% if (showAdvantageDiscountLink)
                   { %>
                <li class="adv-menu-link">
                    <a href="/advantage" id="menuAdvantage">
                        New Car Discounts
                        <span class="ae-sprite ae-new-label-icon"></span>
                    </a>
                </li>
                <% } %>
                <!-- ko template: { name: 'navigationSliderAd' , foreach: SponsoredNavigation.koNewCarsModel.newCarsAds } --><!-- /ko -->                    
			          <li><a href="/comparecars/">Compare Cars</a></li>
                <li><a href="/new/prices.aspx">Check On-Road Price</a></li>
                <li><a href="/new/locatenewcardealers.aspx">Find Dealer</a></li>
                <li><a href="/upcoming-cars/">Upcoming Cars</a></li>
                <li><a href="/new-car-launches/">New Launches</a></li>
                <li><a href="/offers/">Offers</a></li>
            </ul>
        </li>
        <li>
            <a href="javascript:;">
                <span class="cwsprite usedCars-icon"></span>
                <span class="navbarTitle">Used Cars</span>
                <span class="nav-drop fa fa-angle-down"></span>
            </a>
            <ul class="nestedUL">
                <li><a href="/used/cars-for-sale/">All Used Cars</a></li>
                <li><a href="/used/">Find Used Cars</a></li>
                <li><a href="/used/carvaluation/">Check Car Valuation</a></li>
            </ul>
        </li>
        <li>
            <a href="/used/sell/">
                <span class="cwsprite sellCars-icon"></span>
                <span class="navbarTitle">Sell Your Car</span>
            </a>
        </li>
       
        <li>
            <a href="javascript:;">
                <span class="cwsprite reviews-icon"></span>
                <span class="navbarTitle">Reviews and News</span>
                <span class="nav-drop fa fa-angle-down"></span>
            </a>
            <ul class="nestedUL">
                <li><a href="/news/">News</a></li>
                <li><a href="/expert-reviews/">Expert Reviews</a></li>
                <li><a data-cwtccat="ReviewLandingLinkage" data-cwtcact="NavBarLinkClick" data-cwtclbl="source=1" href="/userreviews/">User Reviews</a></li>
                <li><a href="/videos/">Videos</a></li>
                <li><a href="/tipsadvice/">Tips and Advice</a></li>
                <li><a href="/features/">Special Reports</a></li>
                <li><a href="/images/">Images<span class='margin-left5 cwsprite new-red-tag-icon'></span></a></li>
            </ul>
        </li>  
        <li id="navSpecials">
            <a href="javascript:;" rel="nofollow" class="specialCarDropDown">
                <span class="cwsprite special-icon"></span>
                <span class="navbarTitle">Specials</span>
                <span class="nav-drop fa fa-angle-down"></span>
            </a>
            <ul class="nestedUL">
                <li><a href="/tyres/" data-role="click-tracking" data-event="CWInteractive" data-action="Main-Menu-item-click" data-cat="TopMenuabtest" data-label="Tyres_Click">Car Tyres</a></li>
                <li><a href="/electriccars/" data-role="click-tracking" data-event="CWInteractive" data-action="Main-Menu-item-click" data-cat="TopMenuabtest" data-label="Electric_Click">All About Electric</a></li>
                <li><a href="/the-great-indian-hatchback-of-2016/" data-role="click-tracking" data-event="CWInteractive" data-action="Main-Menu-item-click" data-cat="TopMenuabtest" data-label="KwidResultLinkClick">Great Indian Hatchback</a></li>
                <!-- ko template: { name: 'navigationSliderAd' , foreach: SponsoredNavigation.koSpecialsModel.specialsAds } --><!-- /ko -->
            </ul>
        </li>
        <li class="navInsuranceLink" style="display:none">
                <a href="/insurance/?utm=dnavigation/">
                    <span class="cwsprite insurance-icon"></span>
                    <span class="navbarTitle">Insurance</span>
                </a>
            </li>
        <li>
            <a href="https://www.bankbazaar.com/car-loan.html?WT.mc_id=bb01|CL|form_page_oops&utm_source=bb01&utm_medium=display&utm_campaign=bb01&variant=slide&variantOptions=mobileRequired" class="text-black block" data-role="click-tracking" data-event="CWInteractive" data-action="Link_clicked" data-cat="BBLink_navigation_desktop" rel="nofollow" target="_blank">
                <span class="cwsprite nav-loan-icon opacity60"></span>
                <span class="navbarTitle">Loans</span>
                <span class="font9 ad-text-icon">Ad</span>
            </a>
        </li>
        <li>
            <a href="/forums/">
                <span class="cwsprite forum-icon"></span>
                <span class="navbarTitle">Forums</span>
            </a>
        </li>
        <li>
            <a href="/mycarwale/">
                <span class="cwsprite myCarWale-icon"></span>
                <span class="navbarTitle">My CarWale</span>
            </a>
        </li>
    </ul>
    <div class="margin-top40 margin-bottom40 margin-left20">
        <a href="https://play.google.com/store/apps/details?id=com.carwale&referrer=utm_source%3DCarWaleDesktopWebsite%26utm_medium%3DFooter%26utm_campaign%3DCarWale%2520Website%2520Footer" target="_blank" class="cwsprite gplay-icon margin-right10"></a>
        <a href="https://itunes.apple.com/in/app/carwale/id910137745?mt=8" target="_blank" class="cwsprite app-store-icon"></a>
    </div>
</nav>
<!-- ends here -->

 <div id="loc-widget" class="loc-widget position-rel">
        <span id="closeLocIcon" class="cwsprite cross-md-dark-grey position-abt pos-top20 pos-right20 z-index1 font20 cur-pointer"></span>
        <div id="locationProceed" class="position-abt pos-left0 pos-right0 pos-top0 tran-ease-out-all opacity0 z-index-minus1">
            <div class="loc-figure text-center padding-top20 margin-bottom20">
                <p id="loc-title" class="font24 text-black text-bold">Please Tell Us Your City</p>
                <p id="loc-sub-content" class="font14 text-grey margin-top5">Knowing your city will help us provide relevant content to you.</p>
            </div>
            <div class="cityNameWrap table margin-auto position-rel padding-bottom10 validWidget">
                <input type="text" placeholder="Type your city, e.g. Mumbai; New Delhi" class="cityName inputBlur" tabindex="-1" searchtype="citySearch" id="placesQuery" autocomplete="off" />
                <span class="fa fa-spinner fa-spin position-abt pos-right10 pos-top0 text-black" style="display:none;"></span>
                <div class="error noMatchError position-rel font20 hide">
                    <span class="fa fa-exclamation-circle position-rel pos-top3"></span> 
                    <span id="errorId" class="font14 inline-block hide">Sorry! No matching results found. Try again.</span>
                    <span id="errorIpNoCity" class="font14 inline-block hide">Error Identifying Your Location.</span>
                </div>
            </div>
        </div>
    </div>
    <div class="clear"></div>

<div class="loginPopUpWrapper" id="loginPopUpWrapper">
    <!-- login code starts here --> 
    <div class="loginBoxContent" id="Testlogin-box">
        <div class="loginCloseBtn position-abt pos-top10 pos-left10 infoBtn cwsprite cross-md-dark-grey cur-pointer"></div>
        <div class="loginStage">
            <div class="loginWithCW margin-bottom40">
                <h2 class="margin-bottom30">Log in to CarWale</h2>
                <div class="form-control-box margin-bottom20 login-box-form">
                    <input type="text" class="form-control border-red" name="email id" id="txtloginemail" placeholder="Email">
                    <span class="cwsprite error-icon"></span>
                    <div class="cw-blackbg-tooltip">Please enter your name</div>
                </div>
                <div class="form-control-box margin-bottom20 password-box-form">
                    <input type="password" class="form-control" id="txtpasswordlogin" placeholder="Password">
                    <span class="cwsprite error-icon hide"></span>
                    <div class="cw-blackbg-tooltip hide">Please enter your password</div>
                </div>
                <div class="margin-bottom20 font12">
                    <input id="chkRemMe" type="checkbox">
                    <label for="agreecheck">Remember me</label>
                </div>
                <button type="button" id="btnLogin" class="btn btn-orange text-uppercase margin-bottom15 margin-right10" onclick="doLoginCustomer();">log in</button>
                <button type="button" class="loginBtnSignUp btn btn-white btn-md text-uppercase margin-bottom15">sign up</button>
                <div><a class="cur-pointer font12" id="forgotpass">Forgot password?</a></div>
                <!-- Forget Password -->
                <div id="forgotpassdiv">
                    <div class="hide margin-top20 margin-bottom30" id="forgotpassbox">
                        <div class="form-control-box margin-bottom20">
                            <input type="text" class="form-control" name="emailId" id="txtForgotPass" placeholder="Enter your registered email">
                            <span class="cwsprite error-icon hide"></span>
                            <div class="cw-blackbg-tooltip hide">Please enter your registered email</div>
                        </div>
                        <button type="button" id="frgPwd" class="btn btn-orange text-uppercase" onclick="forgotPwdTrigger()">send</button>
                    </div>
                </div>
            </div>
            <div class="loginWithFbGp">
                <p class="font14 margin-bottom30">Or Login with</p>
                <a onclick="fb_login_main()" class="btn fbLoginBtn margin-bottom30">
                    <span class="fbIcon">
                        <span class="cwsprite fb-login-icon"></span>
                    </span>
                    <span class="textWithFbGp">Sign in with facebook</span>
                </a>
                <a onclick="gplus_login()" class="btn gplusLoginBtn">
                    <span class="gplusIcon">
                        <span class="cwsprite gplus-login-icon"></span>
                    </span>
                    <span class="textWithFbGp">Sign in with Google</span>
                </a>
            </div>
        </div>
        <div class="signUpStage">
            <div class="signUpWithCW">
                <h2 class="margin-bottom30">Sign up to CarWale</h2>
                <div class="form-control-box margin-bottom20">
                    <input type="text" class="form-control name-box-form" name="name" placeholder="Name" id="txtnamelogin">
                    <span class="cwsprite error-icon hide"></span>
                    <div class="cw-blackbg-tooltip hide">Please enter your name</div>
                </div>
                <div class="form-control-box margin-bottom20 email-box-form">
                    <input type="text" class="form-control" name="emailId" id="txtemailsignup" placeholder="Email Id">
                    <span class="cwsprite error-icon hide"></span>
                    <div class="cw-blackbg-tooltip hide">Please enter your email id</div>
                </div>
                <div class="form-control-box margin-bottom20 mobile-box-form">
                    <span class="form-mobile-prefix">+91</span>
                    <input type="text" class="form-control padding-left40" name="mobile" placeholder="Mobile no." id="txtmobilelogin">
                    <span class="cwsprite error-icon hide"></span>
                    <div class="cw-blackbg-tooltip hide">Please enter your mobile number</div>
                </div>
                <div class="form-control-box margin-bottom20 pw-box-form">
                    <input type="password" class="form-control" name="password" placeholder="Password" id="txtRegPasswd">
                    <span class="cwsprite error-icon hide"></span>
                    <div class="cw-blackbg-tooltip hide">Please enter your password</div>
                </div>
                <div class="form-control-box margin-bottom20 confrm-pw-box-form">
                    <input type="password" class="form-control" name="confirmPassword" placeholder="Confirm Password" id="txtConfPasswdlogin">
                    <span class="cwsprite error-icon hide"></span>
                    <div class="cw-blackbg-tooltip hide">Please enter your password</div>
                </div>
                <div class="margin-bottom30 font12">
                    <label for="agreecheck">
                        By signing up, you agree with the 
                       
                        <a href="#">User Agreement</a> & <a href="#">Privacy Policy</a>
                    </label>
                </div>
                <button type="button" id="btnSignUp" class="signupBtnLogin btn btn-orange text-uppercase margin-bottom15 margin-right10" onclick="initRegisterCust();">sign up</button>
                <button type="button" id="btnSignUpBack" class="btn btn-white btn-md text-uppercase margin-bottom15">back</button>
            </div>
        </div>
    </div>
</div>
<!-- ends here -->
<div class="loggedinProfileWrapper" id="loggedinProfileWrapper">
    <!-- Logged in user profile code starts here -->
    <div class="profileBoxContent">
        <div class="loginCloseBtn afterLoginCloseBtn position-abt pos-top10 pos-left10 infoBtn cwsprite cross-md-dark-grey cur-pointer"></div>
        <div class="user-profile-banner">
            <div class="user-profile-details padding-left10 padding-top70">
                <div class="user-profile-image rounded-corner50 text-center">
                    <span class="cwsprite login-no-photo-icon margin-top5"></span>
                </div>
                <div class="user-profile-name">
                    <p class="font16 text-white padding-left10"><%=Carwale.UI.Common.CurrentUser.Name %></p>
                    <a href="/mycarwale/MyContactDetails.aspx" class="font12 text-white padding-left10">Edit profile</a>
                </div>
            </div>
        </div>
        <div class="user-profile-option-list padding-top20">
            <ul class="profileUL">
                <li>
                    <a href="/mycarwale/myinquiries/mysellinquiry.aspx">
                        <span class="margin-left15 cwsprite login-edit-icon"></span>
                        <span class="margin-left20 profile-option-title">Manage my car listing</span>
                    </a>
                </li>
                <li>
                    <a href="/mycarwale/">
                        <span class="margin-left15 cwsprite login-garage-icon"></span>
                        <span class="margin-left20 profile-option-title">My Carwale</span>
                    </a>
                </li>
                <li>
                    <a href="/users/changePassword.aspx">
                        <span class="margin-left15 cwsprite login-password-icon"></span>
                        <span class="margin-left20 profile-option-title">Change password</span>
                    </a>
                </li>
                <li onclick="clicklogout();">
                    <a>
                        <span class="margin-left15 cwsprite login-logout-icon"></span>
                        <span class="margin-left20 profile-option-title">Log out</span>
                    </a>
                </li>
            </ul>
        </div>
    </div>
</div>
<div id="detailsNode"></div>

<div id="header" class="<%=landingPage==true?"header-landing ":"header-fixed " %><%=IsEligibleForNewMenu?"main-nav-header":"" %>">
    <!-- Fixed Header code starts here -->
    <!-- CW Doodle start-->
    <% RazorPartialBridge.RenderAction("GetDoodle", "SponsoredCampaign",new Dictionary<string,object>{{"platform" , Carwale.Entity.Enum.Platform.CarwaleDesktop }});%>
    <!-- CW Doodle end-->  
    <div class="leftfloat margin-top10 min-width230">
        <span id="left-navbar-btn" class="navbarBtn cwsprite nav-icon margin-right25 <%= (IsEligibleForNewMenu) ? " hide":" inline-block-noalign"%>"></span>
        <a href="/" class="cwsprite cw-logo" onclick="trackTopMenu('CarwaleLogo-Click',window.location.href)"></a>    
        <% if (showAdvantageDiscountLink && !IsEligibleForNewMenu)
           { %>  
        <span class="advantage-logo-text font14 text-white margin-left10 text-bold padding-left10 position-rel">
            <a href="/advantage/" data-role="click-tracking" data-event="Advantage" data-action="landingPageAccess_Desktop" data-cat="deals_desktop" data-label="headerAdvantageLink"><span class="fa fa-tags"></span> Discounts On New Cars » </a></span>
        <% } %>
    </div>
    <% if(IsEligibleForNewMenu)
       { %>
     <nav id="cw-top-navigation-wrapper">
            <div id="top-navbar-list-content" class="leftfloat">
                <ul id="cw-top-navbar">
                    <li class="navbar-primary-link">
                        <div class="top-nav-label">
                            <span class="margin-right5">New Cars</span>
                            <span class="fa fa-caret-down"></span>
                        </div>
                        <div class="top-nav-nested-panel box-shadow">
                            <div class="container padding-top20 padding-bottom20 new-cars-nested-panel">
                                <div class="grid-8 font15 text-black">
                                    <div class="nested-lt-column1 leftfloat">
                                        <ul class="nested-panel-list">
                                            <li><a href="/new/">Find New Cars</a></li>
                                            <% if(showAdvantageDiscountLink)
                                               { %>
                                            <li><a href="/advantage" data-role="click-tracking" data-event="CWNonInteractive" data-action="landingPageAccess_Desktop" data-cat="deals_desktop" data-label="CarWale_TopMenu">
                                                <span id="advMenuLink">New Car Offers</span></a></li>
                                            <%} 
                                            else { %>
                                             <li><a href="/new-car-launches/">New Launches</a>
                                            <% } %>
                                            <li><a href="/new/locatenewcardealers.aspx">Find Dealer</a></li>
                                        </ul>
                                    </div>
                                    <div class="nested-lt-column2 leftfloat">
                                        <ul class="nested-panel-list">
                                            <li><a href="/new/prices.aspx">Check On-Road Price</a></li>
											<% if(showAdvantageDiscountLink)
                                               { %>
                                           <li><a href="/new-car-launches/">New Launches</a>
                                            <% } %>
                                            <li><a href="https://www.bankbazaar.com/car-loan.html?WT.mc_id=bb01|CL|form_page_oops&utm_source=bb01&utm_medium=display&utm_campaign=bb01&variant=slide&variantOptions=mobileRequired" class="text-black block" data-role="click-tracking" data-event="CWInteractive" data-action="Link_clicked" data-cat="BBLink_navigation_desktop" rel="nofollow" target="_blank">Loans<span class="font10 position-abt text-grey">&nbsp;Ad</span></a></li>
                                        </ul>
                                    </div>
                                    <div class="nested-lt-column3 leftfloat">
                                        <ul class="nested-panel-list">
                                            <li><a href="/upcoming-cars/">Upcoming Cars</a></li>
											<li><a href="/comparecars/">Compare Cars</a></li>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                                <div class="grid-4 omega padding-left50 border-light-left">
                                    <div class="nc-nested-right-panel single-card" id="quickResearch" data-bind="template: { name: 'navigationAd' , foreach: newCarsAds }"></div>
                                    <!-- conditional single-card class -->
                                    <script type="text/html" id="navigationAd">
                                        <div class="grid-6 sponsored-dummy-class">
                                            <p class="text-bold margin-bottom5 font14 sponsored-nav"><span class="sponsored-text-title" data-bind="text: title + ' '"></span><span class="font9" data-bind="text: isSponsored ? 'Ad' : '', css: isSponsored ? 'ad-text-icon' : 'hide'"></span></p>
                                            <a class="nested-panel-image-preview margin-bottom10 pos-relative" data-role="click-tracking" data-event="CWInteractive" data-cat="SponsoredNavigation_d" data-bind="attr:{href:linkUrl, title: title, 'data-label': title, 'data-action': sectionId == 1 ? 'NewCars' : 'Specials'}"> 
                                                <img  data-bind="attr:{alt: title, title: title, 'data-original': hostUrl + '144x81' + originalImgPath + '?wm=1&q=85'}" src="https://imgd.aeplcdn.com/0x0/statics/grey.gif" class="nested-panel-image-preview lazy">     
                                            </a>
                                            <div class="clear"></div>
                                            <div class="nested-panel-image-details margin-top5">
                                                <a class="nested-rt-target-link margin-top5 font13" data-role="click-tracking" data-event="CWInteractive" data-cat="SponsoredNavigation_d" data-bind="attr:{href:linkUrl, title: title, 'data-label': title, 'data-action': sectionId == 1 ? 'NewCars' : 'Specials'}"><span data-bind="text:cta"></span></a>
                                            </div>
                                        </div>
                                    </script>
                                    <div class="clear"></div>
                                </div>
                                <div class="clear"></div>
                            </div>
                        </div>
                    </li>
                    <li class="navbar-primary-link">
                        <div class="top-nav-label">
                            <span class="margin-right5">Used Cars</span>
                            <span class="fa fa-caret-down"></span>
                        </div>
                        <div class="top-nav-nested-panel">
                            <div class="container padding-top20 padding-bottom20 uc-rv-nested-panel">
                                <div class="grid-4 omega font15 text-black">
                                    <div class="nested-lt-column1 leftfloat">
                                        <ul class="nested-panel-list">
                                            <li><a href="/used/cars-for-sale/">All Used Cars</a></li>
                                            <li><a href="/used/">Find Used Cars</a></li>
                                            <li><a href="/used/sell/">Sell Your Car</a></li>
                                        </ul>
                                    </div>
                                    <div class="nested-lt-column4 leftfloat">
                                        <ul class="nested-panel-list">
                                            <li><a href="/used/carvaluation/">Check Car Valuation</a></li>
                                             <li class="navInsuranceLink hide"><a href="/insurance/?utm=dnavigation/">Insurance</a></li>
                                            <li><span class="navChat global-chat-icon chat-launcher-icon applozic-launcher mck-button-launcher" style="display: none" title="My Chats">My Chats<span class='margin-left5 cwsprite new-red-tag-icon'></span></span></li>
                                        </ul>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                                <div class="grid-8 used-and-reviews-right-panel omega"> <!-- conditional two-cards class -->
                                     <div class="uc-rv-nested-right-wrapper">
                                        <div class="uc-rv-nested-right-panel text-center">
                                            <div class="nested-rt-column bg-white">
                                                <a class="nested-panel-image-preview margin-bottom10" href="/used/cars-for-sale/#kms=0-&year=0-&budget=0-4">
                                                    <img src="https://imgd.aeplcdn.com/0x0/design15/d/swift.jpg?v=2" />
                                                </a>
                                                <div class="nested-panel-image-details">
                                                    <div class="font13">
                                                        Upto
                                                    </div>
                                                    <span class="font20">
                                                        ₹
                                                        <span> 4</span>
                                                    </span>
                                                    <span> Lakhs</span>
                                                    <a href="/used/cars-for-sale/#kms=0-&year=0-&budget=0-4" class="nested-rt-target-link margin-top5 font13" >All cars in this Budget</a>
                                                </div>
                                            </div>
                                            <div class="nested-rt-column bg-white">
                                                <a class="nested-panel-image-preview margin-bottom10" href="/used/cars-for-sale/#kms=0-&year=0-&budget=4-8" >
                                                    <img src="https://imgd.aeplcdn.com/0x0/design15/d/verna.jpg" />
                                                </a>
                                                <div class="nested-panel-image-details">
                                                    <div class="font13">
                                                        Between
                                                    </div>
                                                    <span class="font20">
                                                        ₹
                                                        <span> 4-8</span>
                                                    </span>
                                                    <span> Lakhs</span>
                                                    <a href="/used/cars-for-sale/#kms=0-&year=0-&budget=4-8" class="nested-rt-target-link margin-top5 font13" >All cars in this Budget</a>
                                                </div>
                                            </div>
                                            
                                            <div class="clear"></div>
                                        </div>
                                    </div>
                                </div>
                                <div class="clear"></div>
                            </div>
                        </div>
                    </li>
                    <li class="navbar-primary-link">
                        <div class="top-nav-label">
                            <span class="margin-right5">Reviews &amp; News</span>
                            <span class="fa fa-caret-down"></span>
                        </div>
                        <div class="top-nav-nested-panel">
                            <div class="container padding-top20 padding-bottom20 uc-rv-nested-panel">
                                <div class="grid-4 omega font15 text-black">
                                    <div class="nested-lt-column1 leftfloat">
                                        <ul class="nested-panel-list">
                                            <li><a href="/news/">News</a></li>
                                            <li><a href="/expert-reviews/">Expert Reviews</a></li>
                                            <li><a data-cwtccat="ReviewLandingLinkage" data-cwtcact="NavBarLinkClick" data-cwtclbl="source=1" href="/userreviews/">User Reviews</a></li>
                                            <li><a href="/features/">Special Reports</a></li>
                                        </ul>
                                    </div>
                                    <div class="nested-lt-column4 leftfloat">
                                        <ul class="nested-panel-list">
                                            <li><a href="/images/">Images<span class='margin-left5 cwsprite new-red-tag-icon'></span></a></li>
                                            <li><a href="/videos/">Videos</a></li>
                                            <li><a href="/tipsadvice/">Tips and Advice</a></li>
                                            <li><a href="/forums/">Forums</a></li>
                                        </ul>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                                <div class="grid-8 used-and-reviews-right-panel omega"> <!-- conditional two-cards class -->
                                    <div class="uc-rv-nested-right-wrapper padding-top10">
                                        <h3 class="font17 text-black line-height08">Latest Articles</h3>
                                        <div class="rv-nested-right-panel">
                                              <div id="latest-articles" data-bind="template: { name: 'article-gist', foreach: articles }"></div>
                                            <div class="clear"></div>
                                    <script type="text/html" id="article-gist">
                                        <div class="nested-rt-column">
                                            <a class="nested-panel-image-preview margin-bottom10 position-rel" data-bind="attr:{href:articleUrl}">
                                                <span class="image-category-tag" data-bind="text:categoryMaskingName"></span>
                                                <img data-bind="attr:{src: hostUrl + smallPicUrl}" />
                                            </a>
                                            <div class="nested-panel-image-details">
                                                <a class="font13 text-default" data-bind="attr:{href:articleUrl},text:title"></a>
                                            </div>
                                        </div>
                                    </script>
                                        </div>
                                    </div>
                                </div>
                                <div class="clear"></div>
                            </div>
                        </div>
                    </li>

                    <li class="navbar-primary-link">
                        <div class="top-nav-label">
                            <span class="cwsprite"></span>
                            <span class="margin-right5">Specials</span>
                            <span class="fa fa-caret-down"></span>
                        </div>
                        <div class="top-nav-nested-panel">
                                <div class="container padding-top20 padding-bottom20">
                                    <div class="grid-8 omega alpha">
                                        <div class="nc-nested-right-panel single-card">
                                            <!-- conditional single-card class -->
                                            <div class="grid-4 alpha">
                                                <p class="text-bold margin-bottom5 font14 sponsored-nav">Car Tyres</p>
                                                <a title="Car Tyres" class="nested-panel-image-preview margin-bottom10" href="/tyres/" data-role="click-tracking" data-event="CWInteractive" data-action="Tyres" data-cat="CarwaleSpecials" data-label="Tyres">
                                                    <img data-original="https://imgd.aeplcdn.com/0x0/cw/es/tyres/tyres-nav.jpg" src="https://imgd.aeplcdn.com/0x0/statics/grey.gif" height="82" class="lazy" alt="Car Tyres" />
                                                </a>
                                                <div class="nested-panel-image-details img-lg-text margin-top5">
                                                    <a class="nested-rt-target-link margin-top5 font13" data-role="click-tracking" data-event="CWInteractive" data-action="Tyres" data-cat="CarwaleSpecials" data-label="Tyres" href="/tyres/">Know More</a>
                                                </div>
                                            </div>
                                            <div class="grid-4 alpha">
                                                <p class="text-bold margin-bottom5 font14 sponsored-nav">All About Electric</p>
                                                <a  title="e2oPlus" class="nested-panel-image-preview margin-bottom10" href="/electriccars/" data-role="click-tracking" data-event="CWInteractive" data-action="All About Electric" data-cat="CarwaleSpecials" data-label="All About Electric">
                                                    <img data-original="https://imgd1.aeplcdn.com/0x0/cw/es/e2o/e2oplus/e2oplus-car-nav-v2.jpg?q=85" src="https://imgd.aeplcdn.com/0x0/statics/grey.gif" height="82" class="lazy" alt="e2oPlus" />
                                                </a>
                                                <div class="nested-panel-image-details margin-top5">
                                                    <a class="nested-rt-target-link margin-top5 font13" data-role="click-tracking" data-event="CWInteractive" data-action="Mahindra e2o" data-cat="CarwaleSpecials" data-label="Mahindra e2o" href="/electriccars/">Explore More</a>
                                                </div>
                                            </div>
                                            <div class="grid-4 alpha">
                                                <p class="text-bold margin-bottom5 font14 sponsored-nav">Great Indian Hatchback</p>
                                                <a title="Renault Kwid" class="nested-panel-image-preview margin-bottom10" href="/the-great-indian-hatchback-of-2016/" data-role="click-tracking" data-event="CWInteractive" data-action="Great Indian Hatchback" data-cat="CarwaleSpecials" data-label="Great Indian Hatchback">
                                                    <img data-original="https://imgd2.aeplcdn.com/0x0/cw/es/renault-kwid/result-imgs/compressed/kwid-car-nav-new.jpg?q=85" src="https://imgd.aeplcdn.com/0x0/statics/grey.gif" height="82" class="lazy" alt="Renault Kwid" />
                                                </a>
                                                <div class="nested-panel-image-details margin-top5">
                                                    <a class="nested-rt-target-link margin-top5 font13" data-role="click-tracking" data-event="CWInteractive" data-action="Great Indian Hatchback" data-cat="CarwaleSpecials" data-label="Great Indian Hatchback" href="/the-great-indian-hatchback-of-2016/">Explore More</a>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="grid-4 omega padding-left20 border-light-left">
                                        <div class="nc-nested-right-panel single-card"  id="specialsAds" data-bind="template: { name: 'navigationAd' , foreach: specialsAds }"></div>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                            </div>
                    </li>
                </ul>
                <div class="clear"></div>
            </div>
            <div class="global-search global-search-new global-search-wrapper position-rel">

                <span class="fa fa-search" id="do_search"></span>
                <input type="text" name="globalSearch" placeholder="Search" id="globalSearch">
                <span class="fa fa-spinner fa-spin position-abt pos-right10 pos-top10 text-black" style="display:none;right:35px;"></span>
            <!-- #include file="/Views/Shared/_GlobalSearch.cshtml" -->
            </div>
            <div class="clear"></div>
        </nav>
    
        <div id="cw-topnav-right-section" class="rightfloat">

            <div id="global-location" class="global-location global-location-new position-rel">

                <div class="global-search">
                <input type="hidden" id="hdnGlobalCity" value="<%=Carwale.UI.Common.CookiesCustomers.MasterCityId %>"/>
                    <span id="global-place" class="font13">Enter your location </span>
                    <span class="fa fa-map-marker position-abt text-white"></span>
                </div>
            </div>

            <div class="login-box login-box-new  <%= (IsLoggedIn) ? "hide" : "" %>" id="firstLogin"><span class="cwsprite user-login-icon"></span></div>
            <div id="userLoggedin" class="loggedin-box rounded-corner50 <%= (IsLoggedIn) ? "" : "hide" %>">
                <span class="cwsprite login-no-photo-icon" id="profilepic1"></span>
                <span class="login-with-photo hide" id="profilepic2">
                    <img src="https://imgd.aeplcdn.com/0x0/statics/grey.gif">
                </span>
            </div>
            <div class="clear"></div>
        </div>
    <% } else { %>
    <div class="rightfloat margin-top10">
            <div class="global-search global-search-old global-search-wrapper position-rel focused">

                <span class="fa fa-search" id="do_search"></span>
                <input type="text" name="globalSearch" placeholder="Search" id="globalSearch" class="blur ui-autocomplete-input" autocomplete="off">
                <span class="fa fa-spinner fa-spin position-abt pos-right10 pos-top10 text-black" style="display:none;right:35px;"></span>
            </div>

            <div id="global-location" class="global-location position-rel">

                <div class="global-search">
                    <input type="hidden" id="hdnGlobalCity" value="<%=Carwale.UI.Common.CookiesCustomers.MasterCityId %>"/>
                    <span id="global-place" class="font13">Enter your location</span>
                    <span class="fa fa-map-marker position-abt text-white"></span>
                </div>

            </div>


            <div class="login-box <%= (IsLoggedIn) ? "hide" : "" %>" id="firstLogin">Log In</div>
            <div id="userLoggedin" class="loggedin-box rounded-corner50 <%= (IsLoggedIn) ? "" : "hide" %>">
                <span class="cwsprite login-no-photo-icon" id="profilepic1"></span>
                <span class="login-with-photo hide" id="profilepic2">
                    <img src="https://imgd.aeplcdn.com/0x0/statics/grey.gif">
                </span>
            </div>
            <div class="clear"></div>
        </div>
    <% } %>
    <div class="clear"></div>
    <div class="globalLocBlackOut"></div>
</div>
<!-- ends here -->
<div class="globalcity-popup rounded-corner2 hide" id="globalcity-popup">
    <!-- global city pop up code starts here -->
    <div class="globalcity-popup-data text-center">
        <div class="globalcity-close-btn position-abt pos-top10 pos-right10 cwsprite cross-lg-lgt-grey cur-pointer"></div>
        <div class="cityPopup-box rounded-corner50 margin-bottom20">
            <span class="cwsprite cityPopup-icon margin-top10"></span>
        </div>
        <p class="font20 margin-bottom15 text-capitalize">Please tell us your city</p>
        <p class="text-light-grey margin-bottom15">Knowing your city helps us provide relevant content for you</p>
        <div class="form-control-box globalcity-input-box">
            <div class="margin-bottom20">
                <span class="position-abt pos-right15 pos-top15 cwmsprite cross-sm-dark-grey cur-pointer"></span>
                <input type="text" class="form-control padding-right30" placeholder="Type to select city name, eg: Mumbai" name="globalCityPopUp" id="globalCityPopUp">
                <span class="fa fa-spinner fa-spin position-abt pos-right10 pos-top15 text-black" style="display: none"></span>
                <span class="cwmsprite error-icon hide global-city-error-icon"></span>
                <div class="cw-blackbg-tooltip global-city-error-msg hide">No city match.</div>
            </div>
        </div>
        <div>
            <button id="btnConfirmCity" class="btn btn-orange font18 text-uppercase" type="button">Confirm city</button>
        </div>
    </div>
    <div class="clear"></div>
</div>
<% if (!(Request.RawUrl.StartsWith("/used") || Request.RawUrl.Contains("/advantage")))
           {
               RazorPartialBridge.RenderPartial(ProductExperiments.showCustomNotificationPopup(isMobile) ? "~/Views/Shared/Notification/_CustomNotificationPopup.cshtml" : "~/Views/Shared/Notification/_NotificationPopupScript.cshtml"); 
         } %>
<script type="text/javascript">
    var logoUrl = '<%=System.Configuration.ConfigurationManager.AppSettings["BridgestoneUrl"]%>';
    $('#bridgestoneurl').attr('href', logoUrl);
</script>
<!-- Begin comScore Tag -->
<script>
  var _comscore = _comscore || [];
  _comscore.push({ c1: "2", c2: "19261200" });
  (function() {
    var s = document.createElement("script"), el = document.getElementsByTagName("script")[0]; s.async = true;
    s.src = (document.location.protocol == "https:" ? "https://sb" : "http://b") + ".scorecardresearch.com/beacon.js";
    window.addEventListener("load", function () { setTimeout(function () { el.parentNode.insertBefore(s, el); }, 0) }, false);
  })();
</script>
<script type="text/html" id="navigationSliderAd">
    <li class="sponsoredLi">
        <a data-bind="attr:{href: linkUrl, 'data-label': title, 'data-action': sectionId == 1 ? 'NewCars' : 'Specials'}" data-role="click-tracking" data-event="CWInteractive" data-cat="SponsoredNavigation_d">
            <span data-bind="text: title + ' '"></span><span class="font9" data-bind="text: isSponsored ? 'Ad' : '', css: isSponsored ? 'ad-text-icon' : 'hide'"></span>
        </a>
    </li>
</script>
<noscript>
    <img src="http://b.scorecardresearch.com/p?c1=2&c2=19261200&cv=2.0&cj=1" />
</noscript>
<!-- End comScore Tag -->