<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.New.NewBikeDealerList" EnableViewState="false" %>
<%@ Register TagPrefix="BW" TagName="MPopupWidget" Src="/m/controls/MPopupWidget.ascx" %>
<!DOCTYPE html>
<html>
<head>

    <% 
        keywords = String.Format("{0} dealers city, Make showrooms  {1}, {1} bike dealers, {0} dealers, {1} bike showrooms, bike dealers, bike showrooms, dealerships", makeName, cityName);
        description = String.Format("{0} bike dealers/showrooms in {1}. Find {0} bike dealer information for more than 200 cities. Dealer information includes full address, phone numbers, email, pin code etc", makeName, cityName);
        title = String.Format("{0} Dealers in {1} city | {0} New bike Showrooms in {1} - BikeWale", makeName, cityName);
        canonical = String.Format("http://www.bikewale.com/new/{0}-dealers/{1}-{2}.html", makeMaskingName, cityId, cityMaskingName);              
    %>

    <!-- #include file="/includes/headscript_mobile.aspx" -->
    <style type="text/css">
        #listingHeader{background:#313131;color:#fff;width:100%;height:48px;position:fixed;overflow:hidden;z-index:2;}.listing-back-btn, .listing-filter-btn, #dealersFilterWrapper .filterBackArrow {padding:12px 15px;cursor:pointer;}#dealersFilterWrapper .filterTitle { margin-top:10px; }.fa-arrow-back{width:12px;height:20px;background-position:-63px -162px;}.filter-icon{width: 20px;height: 20px;background-position: -158px -144px;}.content-inner-block-1520 { padding:15px 20px; }.box-shadow { -webkit-box-shadow:0 0 1px #e2e2e2; -moz-box-shadow:0 0 1px #e2e2e2; box-shadow:0 0 1px #e2e2e2; }.text-pure-black { color:#000; }.featured-tag {position:relative;left:-20px;top:-5px;width:100px;background:#4d5057;z-index:1; line-height:28px; }.featured-tag:after {content:'';width:12px; height:28px;background: url(http://imgd1.aeplcdn.com/0x0/bw/static/sprites/m/upcoming-ribbon.png?v=15Mar2016) no-repeat right top;position:absolute;left:98px;}
        #dealersList li { border-top:1px solid #e2e2e2; padding-top:18px; margin-top:20px;}#dealersList li:first-child { margin-top:0; }#dealersList a { display:block; }#dealersList a:hover { text-decoration:none; }
        .tel-sm-grey-icon{width: 15px;height: 15px;background-position: 0 -435px;position: relative;top: 2px;}.mail-grey-icon{width: 15px;height: 9px;background-position: -19px -437px;}
        .btn-white-orange { background: #fff; color: #82888b; border: 1px solid #82888b;}.btn-white-orange:hover { background: #f04031; color: #fff; border: 1px solid #f04031; }.btn-white-orange:focus { background: #f04031; color: #fff; border: 1px solid #f04031; }
        /*filter popup*/
        #dealersFilterWrapper{background: #f5f5f5;position: fixed;overflow-x: hidden;overflow-y: scroll;z-index: 10000;top: 0;right: 0;bottom: 0;left: 100%;width: 100%;-webkit-overflow-scrolling: touch;}
        .dealers-brand-city-wrapper{background:#f5f5f5;z-index:11;position:fixed;left:100%;top:0;overflow-y: scroll;width: 100%;height: 100%;}
        .dealers-back-arrow-box{height: 30px;width: 40px;position: absolute;top: 5px;z-index: 11;}.dealers-back-arrow-box span {position: absolute;top: 7px;left: 10px;}
        .bwm-brand-city-box .form-control {padding: 10px 50px;}.dealers-city-popup-box { display:none; }
        .filter-brand-city-ul li{ border-top:1px solid #ccc;font-size:14px;padding:15px 10px;color:#333333;}.filter-brand-city-ul li:hover{background: #ededed;}
        /*get assistance*/
        #leadCapturePopup .errorIcon, #leadCapturePopup .errorText, #otpPopup, .update-mobile-box, .otp-notify-text {display: none;}
        .mobile-prefix {position: absolute;padding: 10px 13px 13px;color: #999;z-index: 2;}
        #getMobile {padding: 9px 40px;}.otp-icon { width:30px; height:40px; background-position: -107px -177px; }.edit-blue-icon { width:14px; height:16px; background-position: -114px -121px; }#otpPopup .otp-box p.resend-otp-btn { color:#0288d1; cursor:pointer; font-size:14px; }
        #brandSearchBar { padding:0; background: #f5f5f5; z-index: 11; position: fixed; left: 100%; top: 0; overflow-y: scroll; width: 100%; height: 100%;}#brandSearchBar li { border-top: 1px solid #ccc; font-size: 14px; padding: 15px 10px; color: #333333; cursor: pointer;}#brandSearchBar li:hover { background: #ededed; }.dealer-brand-wrapper { display:none; }.bwm-dealer-brand-box .back-arrow-box { height: 30px; width: 40px; position: absolute; top: 5px; z-index: 11; cursor: pointer; }.bwm-dealer-brand-box span.back-long-arrow-left {position: absolute;top: 7px;left: 10px;}.bwm-dealer-brand-box .back-arrow-box {position: absolute;left: 5px;}.bwm-dealer-brand-box .form-control {padding: 10px 50px;}.activeBrand {font-weight: bold;background-color: #ddd;}.dealer-search-brand-form { padding:10px 25px 10px 10px; text-align:left; cursor:pointer; background: #fff url(http://imgd1.aeplcdn.com/0x0/bw/static/design15/old-images/m/dropArrowBg.png?v1=19082015) no-repeat 96% 50%; text-align:left; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; border-radius:2px;border: 1px solid #ccc; }.dealer-search-brand { width:100%; height: 40px;}.border-red { border:1px solid #f00 !important; }
    </style>
    <script type="text/javascript">
        var makeName = "<%=makeName%>";
        var makeMaskingName = "<%=makeMaskingName%>";
        var makeId = "<%=makeId%>";
        var cityName = "<%=cityName%>";
        var cityId = "<%= cityId%>";
        var cityMaskingName = "<%= cityMaskingName%>";
        var clientIP = "<%= clientIP %>";
        var pageUrl = "<%= pageUrl%>";
        var leadSrcId = eval("<%= (int)(Bikewale.Entities.BikeBooking.LeadSourceEnum.DealerLocator_MobileListing) %>");
        var pqSrcId = eval("<%= (int)(Bikewale.Entities.PriceQuote.PQSourceEnum.Mobile_DealerLocator_Detail) %>");
        var pageSrcId = eval("<%= Bikewale.Utility.BWConfiguration.Instance.SourceId %>");

    </script>
</head>
<body class="bg-light-grey">
    <form runat="server">
        <header id="listingHeader">
            <div class="leftfloat listing-back-btn">
                <a href="javascript:history.back()"><span class="bwmsprite fa-arrow-back"></span></a>
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
                    <h1 class="font16 text-pure-black margin-bottom15"><%=makeName %> dealers in <%=cityName %> <span class="font14 text-light-grey">(<%=totalDealers %>)</span></h1>
                    <ul id="dealersList">
                        <asp:Repeater ID="rptDealers" runat="server">
                            <ItemTemplate>
                                <li>
                                    <div class="<%# ((DataBinder.Eval(Container.DataItem,"DealerType").ToString() == "3") || (DataBinder.Eval(Container.DataItem,"DealerType").ToString() == "2"))? "" : "hide" %> featured-tag text-white text-center font14 margin-bottom5">
                                        Featured
                                    </div>
                                    <div class="font14">
                                        <h2 class="font16 margin-bottom10"><%# GetDealerDetailLink(DataBinder.Eval(Container.DataItem,"DealerType").ToString(), DataBinder.Eval(Container.DataItem,"DealerId").ToString(), DataBinder.Eval(Container.DataItem,"CampaignId").ToString(), DataBinder.Eval(Container.DataItem,"Name").ToString()) %></h2>
                                        <p class="text-light-grey margin-bottom5"><%# (String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"objArea.AreaName").ToString()))?"":DataBinder.Eval(Container.DataItem,"objArea.AreaName") + "," %> <%# DataBinder.Eval(Container.DataItem,"City") %></p>
                                        <div class="<%# (String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"MaskingNumber").ToString()))?"hide":string.Empty %>"><a href="tel:<%#DataBinder.Eval(Container.DataItem,"MaskingNumber").ToString() %>" class="text-light-grey margin-bottom5"><span class="bwmsprite tel-sm-grey-icon"></span> <%# DataBinder.Eval(Container.DataItem,"MaskingNumber").ToString() %></a></div>
                                        <div class="<%# (String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"Email").ToString()))?"hide":string.Empty %>"><a href="mailto:<%# DataBinder.Eval(Container.DataItem,"Email") %>" class="text-light-grey"><span class="bwmsprite mail-grey-icon"></span> <%# DataBinder.Eval(Container.DataItem,"Email") %></a></div>
                                        <input data-item-id="<%# DataBinder.Eval(Container.DataItem,"DealerId") %>" data-item-type="<%# (DataBinder.Eval(Container.DataItem,"DealerType")) %>" campId="<%# (DataBinder.Eval(Container.DataItem,"CampaignId")) %>" type="button" class="btn btn-white-orange btn-full-width margin-top15 get-assistance-btn <%# ((DataBinder.Eval(Container.DataItem,"DealerType").ToString() == "3") || (DataBinder.Eval(Container.DataItem,"DealerType").ToString() == "2"))? "" : "hide" %>" value="Get assistance">
                                    </div>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
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
            <div class="user-selected-brand-city-container content-inner-block-20" id="divMakeCity">
                <div id="selectBrand" class="form-control text-left input-sm position-rel margin-bottom20">
                    <span class="position-abt progress-bar"></span>
                    <div class="user-selected" data-bind="text: makeName"></div>
                    <span class="fa fa-spinner fa-spin position-abt text-black btnSpinner"></span>
                    <span class="bwmsprite fa-angle-right position-abt pos-top10 pos-right10"></span>                  
                    <span class="bwsprite error-icon errorIcon hide" ></span>
                    <div class="bw-blackbg-tooltip errorText hide" ></div>
                </div>
                <div id="selectCity" class="form-control text-left input-sm position-rel margin-bottom20">
                    <span class="position-abt progress-bar"></span>
                    <div class="user-selected" data-bind="text: cityName"></div>
                    <span class="fa fa-spinner fa-spin position-abt text-black btnSpinner"></span>
                    <span class="bwmsprite fa-angle-right position-abt pos-top10 pos-right10"></span>
                    <span class="bwsprite error-icon errorIcon hide" ></span>
                    <div class="bw-blackbg-tooltip errorText hide" ></div>
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
                        <input class="form-control" type="text" id="dealersBrandInput" autocomplete="on" placeholder="Select brand" />
                    </div>
                    <ul id="filterBrandList" class="filter-brand-city-ul margin-top40" data-filter-type="brand-filter">
                        <asp:Repeater ID="rptMakes" runat="server">
                            <ItemTemplate>
                                <li maskingName="<%# DataBinder.Eval(Container.DataItem,"MaskingName") %>" value="<%# DataBinder.Eval(Container.DataItem,"MakeId") %>"><%# DataBinder.Eval(Container.DataItem,"MakeName") %></li>                                 
                             </ItemTemplate>
                        </asp:Repeater>                      
                    </ul>                    
                    <span class="bwsprite error-icon errorIcon hide" ></span>
                    <div class="bw-blackbg-tooltip errorText hide" ></div>
                </div>

                <div class="dealers-city-popup-box bwm-brand-city-box bike-city-list-container form-control-box text-left">
                    <div class="user-input-box">
                        <span class="dealers-back-arrow-box">
                            <span class="bwmsprite back-long-arrow-left"></span>
                        </span>
                        <input class="form-control" type="text" id="dealersCityInput" autocomplete="on" placeholder="Select city" />
                    </div>
                    <ul id="filterCityList" class="filter-brand-city-ul margin-top40" data-filter-type="city-filter">
                         <asp:Repeater ID="rptCities" runat="server">
                            <ItemTemplate>
                                <li maskingName="<%# DataBinder.Eval(Container.DataItem,"CityMaskingName") %>" value="<%# DataBinder.Eval(Container.DataItem,"CityId") %>"><%# DataBinder.Eval(Container.DataItem,"CityName") %></li>                                
                            </ItemTemplate>
                        </asp:Repeater>                         
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
                        <div class="dealer-search-brand form-control-box">
                            <div class="dealer-search-brand-form"><span>Select brand</span></div>
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
                                <input class="form-control" type="text" id="assistanceBrandInput" placeholder="Select brand" />
                            </div>
                            <ul id="sliderBrandList" class="slider-brand-list margin-top40"  data-bind="foreach: bikes">
                               <li data-bind="text: bike, click: function () { customerViewModel.versionId(this.version.versionId); customerViewModel.modelId( this.model.modelId); }"></li>
                            </ul>
                         </div>
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
    
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <!-- #include file="/includes/footerscript_Mobile.aspx" -->

        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/dealerlisting.js?<%= staticFileVersion %>"></script>
    </form>
</body>
</html>
