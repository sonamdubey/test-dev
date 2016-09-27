<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.DealerDetails" EnableViewState="false" %>
<%@ Register Src="~/m/controls/DealersCard.ascx" TagName="DealerCard" TagPrefix="BW" %>
<%@ Register Src="~/m/controls/LeadCaptureControl.ascx" TagName="LeadCapture" TagPrefix="BW" %>
<!DOCTYPE html>
<html>
<head>
    <%
        keywords = String.Format("{0}, {0} dealer,  {0} Showroom, {0} {1}", dealerName, dealerCity);
        description = String.Format("{2} is dealer of {0} bikes in {1}. Get best offers on {0} bikes at {2} showroom", makeName, dealerCity, dealerName);
        title = String.Format("{0}  {1} - {0} Showroom in {1} - BikeWale", makeName, dealerCity);
    %>
    <!-- #include file="/includes/headscript_mobile_min.aspx" -->
    <style type="text/css">
        @charset "utf-8";#makeDealersContent .swiper-slide:first-child,.swiper-wrapper.upcoming-carousel-content .swiper-slide:first-child{margin-left:10px}header{width:100%;height:50px}#bw-header.fixed{position:fixed;left:0;right:0;top:0;z-index:3}.discover-bike-carousel .back,.discover-bike-carousel .front,.discover-bike-carousel .jcarousel li{height:auto}.content-inner-block-1520{padding:15px 20px}.pos-top3{top:3px}.text-x-black{color:#1a1a1a}.search-bike-item{background:#fff}.search-bike-item .front{height:auto;border-radius:0;box-shadow:none;-moz-box-shadow:none;-ms-box-shadow:none;border:0}.search-bike-item div.border-top1:last-child{display:none}.sort-icon{background-position:-193px -144px!important;height:20px;width:20px}.sort-btn{color:#FFF;padding:0 10px;position:relative;cursor:pointer;font-size:20px}.filter-sort-div{display:table;width:100%;border-top:solid 1px #d9d9d9;padding:10px 0;background:#fff;margin-bottom:10px;box-shadow:0 3px 4px 0 #e5e5e5}.filter-sort-div div{display:table-cell;width:33.333%}.filter-sort-div div a{padding:0;color:#565a5c;display:block;text-decoration:none;text-align:center}.filter-sort-div .filter-icon{position:relative;top:2px}#sort-by-div.fixed{position:fixed;left:0;right:0;top:50px;z-index:999}.modelurl img{width:292px}#listitems .bikeDescWrapper{padding-right:20px;padding-left:20px}.text-default{color:#4d5057}.text-truncate{width:100%;text-align:left;text-overflow:ellipsis;white-space:nowrap;overflow:hidden}.swiper-slide.bike-carousel-swiper{width:184px;min-height:250px;background:#fff;border:1px solid #e2e2e2}.bike-carousel-swiper img{width:100%;height:103px}.bike-swiper-image-wrapper{width:100%;height:103px;display:block;overflow:hidden;text-align:center;position:relative}.bike-swiper-image-wrapper a{width:100%;height:100%;display:block}.bike-swiper-details-wrapper{padding:5px 20px 10px}.bike-swiper-details-wrapper p{font-size:12px}.block{display:block}#makeOverallTabsWrapper{height:44px}.overall-specs-tabs-container{width:100%;display:block;background:#fff;overflow-x:scroll;z-index:2}.overall-specs-tabs-container::-webkit-scrollbar{width:0;height:0}.overall-specs-tabs-container::-webkit-scrollbar-thumb,.overall-specs-tabs-container::-webkit-scrollbar-track{display:none}.overall-specs-tabs-container.fixed-tab-nav{position:fixed;top:0;left:0}.overall-specs-tabs-wrapper{width:100%;display:table;border-bottom:1px solid #e2e2e2}.overall-specs-tabs-wrapper li{font-size:14px;padding:10px 15px;display:table-cell;text-align:center;white-space:nowrap;color:#82888b;cursor:pointer}.overall-specs-tabs-wrapper li.active{border-bottom:3px solid #ef3f30;color:#4d5057;font-weight:700}#makeTabsContentWrapper h2{margin-bottom:13px}#makeTabsContentWrapper h3{margin-bottom:12px}.model-preview-more-content{display:none}.news-image-wrapper,.review-image-wrapper{display:inline-block;vertical-align:top;margin-right:10px;border:1px solid #e2e2e2}.border-divider{border-top:1px solid #e2e2e2}.news-image-wrapper{width:148px;height:83px;overflow:hidden}.review-image-wrapper{width:120px;height:68px}.news-image-wrapper a,.review-image-wrapper a{width:100%;display:block}.make-user-review-rating-container,.make-user-review-title-container,.news-heading-wrapper,.review-heading-wrapper{display:inline-block;vertical-align:top}.news-image-wrapper img,.review-image-wrapper img{width:100%;height:68px}.blue-right-arrow-icon{width:10px;height:12px;background-position:-56px -435px;position:relative;top:1px;left:7px}#makeNewsContent .news-heading-wrapper h4,#makeReviewsContent .make-user-review-title-container h4,#makeReviewsContent .review-heading-wrapper h4{margin-top:2px;margin-bottom:7px}.news-heading-wrapper{width:47%}.review-heading-wrapper{width:55%}.margin-top17{margin-top:17px}.make-user-review-rating-container{width:120px;height:68px;border:1px solid #e2e2e2;background:#f9f9f9;padding-top:10px;text-align:center;margin-right:15px}.make-user-review-title-container{width:55%}.dealer-city-image-preview{height:75px;display:block;text-align:center;margin-bottom:25px}.city-sprite{background:url(http://imgd2.aeplcdn.com/0x0/bw/static/sprites/m/bwm-city-sprite.png) no-repeat;display:inline-block}.ahmedabad-icon,.bangalore-icon,.chandigarh-icon,.chennai-icon,.delhi-icon,.hyderabad-icon,.kolkata-icon,.lucknow-icon,.mumbai-icon,.pune-icon{height:70px}.mumbai-icon{width:98px;background-position:0 0}.pune-icon{width:140px;background-position:-108px 0}.bangalore-icon{width:102px;background-position:-258px 0}.delhi-icon{width:54px;background-position:-370px 0}.chennai-icon{width:41px;background-position:-434px 0}.hyderabad-icon{width:49px;background-position:-485px 0}.kolkata-icon{width:138px;background-position:-544px 0}.lucknow-icon{width:132px;background-position:-692px 0}.ahmedabad-icon,.chandigarh-icon{width:0;background-position:0 0}#makeUsedBikeContent li{margin-top:20px}#makeUsedBikeContent li:first-child{margin-top:0}#makeDealersContent h2{margin-bottom:20px}.swiper-slide.bike-carousel-swiper.dealer-by-city{width:184px;min-height:210px;padding:17px 10px}.swiper-slide.bike-carousel-swiper.dealer-no-city{width:184px;min-height:210px}.vertical-top{display:inline-block;vertical-align:top}.dealership-loc-icon{width:10px;height:15px;background-position:-40px -435px;position:relative;top:4px}.dealership-address,.dealership-email{width:87%}.tel-sm-grey-icon{width:15px;height:15px;background-position:0 -435px;position:relative;top:2px}.mail-grey-icon{width:15px;height:9px;background-position:-19px -437px;position:relative;top:6px}.dealer-no-city a{width:184px;min-height:210px;color:#2a2a2a;display:block;padding:20px}@media all and (max-width:355px){.make-user-review-title-container,.review-heading-wrapper{width:50%}.news-heading-wrapper{width:40%}}
    </style>
    <style type="text/css">
        .dealer-details a:hover,.float-button a:hover{text-decoration:none}.padding-15-20{padding:15px 20px}.pos-top-3{top:3px}.featured-tag{width:74px;text-align:center;background:#3799a7;font-size:12px;color:#fff;line-height:20px;-webkit-border-radius:2px;-moz-border-radius:2px;-o-border-radius:2px;border-radius:2px}.dealership-details{width:92%}.vertical-top{display:inline-block;vertical-align:top}.location-details{display:none}.btn-green{background:#1b9618;color:#fff;border:1px solid #1b9618}.float-button .btn{padding:8px 0;font-size:18px}.dealership-loc-icon{width:10px;height:14px;background-position:-40px -436px;position:relative;top:4px;margin-right:3px}.star-white{width:8px;height:8px;background-position:-174px -447px;margin-right:4px}.tel-sm-grey-icon{width:10px;height:10px;background-position:0 -437px;position:relative;top:5px;margin-right:3px}.mail-grey-icon{width:15px;height:10px;background-position:-19px -437px;position:relative;top:6px}.crosshair-icon{width:20px;height:20px;background:url(http://imgd1.aeplcdn.com/0x0/bw/static/sprites/d/detect-location-icon.png) no-repeat}.clock-icon{width:12px;height:12px;background-position:-160px -462px;position:relative;top:4px;margin-right:4px}.get-direction-icon{width:12px;height:10px;background-position:-31px -421px}#model-available-list li{border-top:1px solid #e2e2e2;padding-top:10px;padding-bottom:15px}#model-available-list li:first-child{border-top:0}.image-block{width:100%;height:163px;line-height:0;display:table;text-align:center;margin-bottom:15px}.image-block .image-content{display:table-cell;vertical-align:middle}.image-block img{width:290px;max-width:100%}.text-truncate{width:100%;text-align:left;text-overflow:ellipsis;white-space:nowrap;overflow:hidden}@media only screen and (max-width:320px){.location-details{font-size:13px}}#leadCapturePopup.bwm-fullscreen-popup{padding:30px 30px 100px}#leadCapturePopup .errorIcon,#leadCapturePopup .errorText,#otpPopup,.otp-notify-text,.update-mobile-box{display:none}.otp-icon{width:29px;height:29px;background-position:-109px -177px}.edit-blue-icon{width:20px;height:20px;background-position:-114px -122px}#otpPopup .otp-box p.resend-otp-btn{color:#0288d1;cursor:pointer;font-size:14px}input[type=text]:focus,input[type=number]:focus{outline:0;box-shadow:none}.input-box{height:60px;text-align:left}.input-box input{width:100%;display:block;padding:7px 0;border-bottom:1px solid #82888b;font-weight:700;color:#4d5057}.input-box label{position:absolute;top:4px;left:0;font-size:16px;color:#82888b;-webkit-transition:.2s ease all;-moz-transition:.2s ease all;-o-transition:.2s ease all;transition:.2s ease all}.input-number-box input{padding-left:25px}.input-number-prefix{display:none;position:absolute;top:7px;font-weight:700;color:#82888b}.boundary{position:relative;width:100%;display:block}.boundary:after,.boundary:before{content:'';position:absolute;bottom:0;width:0;height:2px;background-color:#41b4c4;-webkit-transition:.2s ease all;-moz-transition:.2s ease all;-o-transition:.2s ease all;transition:.2s ease all}.boundary:before{left:50%}.boundary:after{right:50%}.error-text{display:none;font-size:12px;position:relative;top:4px;left:0;color:#d9534f}.input-box.input-number-box input:focus~.input-number-prefix,.input-box.input-number-box.not-empty .input-number-prefix,.input-box.invalid .error-text{display:inline-block}.input-box input:focus~label,.input-box.not-empty label{top:-14px;font-size:12px}.input-box input:focus~.boundary:after,.input-box input:focus~.boundary:before,.select-box.invalid .boundary:after,.select-box.invalid .boundary:before{width:50%}.input-box.invalid .boundary:after,.input-box.invalid .boundary:before,.select-box.invalid .boundary:after,.select-box.invalid .boundary:before{background-color:#d9534f;width:50%}.btn-fixed-width{padding-right:0;padding-left:0;width:205px}
    </style>
    <script src="http://maps.googleapis.com/maps/api/js?key=<%= Bikewale.Utility.BWConfiguration.Instance.GoogleMapApiKey %>&libraries=places"></script>
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_mobile.aspx" -->
    </script>
</head>
<body class="bg-light-grey">
    <form runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->
        
        <section class="container bg-white margin-bottom10">
            <div class="bg-white box-shadow">
                <h1 class="font20 box-shadow padding-15-20"><%= dealerDetails.Name %></h1>
                <div class="dealer-details position-rel pos-top-3 content-inner-block-20 font14">
                    <%if (dealerDetails.DealerType == (int)(Bikewale.Entities.PriceQuote.DealerPackageTypes.Premium) || dealerDetails.DealerType == (int)(Bikewale.Entities.PriceQuote.DealerPackageTypes.Deluxe))
                    { %>
                    <div class="margin-bottom20">
                        <span class="featured-tag inline-block margin-right10"><span class="bwmsprite star-white"></span>Featured</span>
                        <h2 class="font14 text-bold inline-block">Authorized Bajaj dealer in Thane</h2>
                    </div>
                    <%} %>
                    <p class="margin-bottom10">
                        <span class="bwmsprite dealership-loc-icon vertical-top"></span>
                        <span class="vertical-top dealership-details text-light-grey"><%= dealerDetails.Address %></span>
                    </p>
                    <% if (!string.IsNullOrEmpty(dealerDetails.MaskingNumber))
                    { %>
                    <div class="margin-bottom10">
                        <a href="tel:<%= dealerDetails.MaskingNumber %>" class="text-default text-bold maskingNumber">
                            <span class="bwmsprite tel-sm-grey-icon vertical-top"></span>
                            <span class="vertical-top text-bold dealership-details"><%= dealerDetails.MaskingNumber %></span>
                        </a>
                    </div>
                    <% } %>
                    <% if (!string.IsNullOrEmpty(dealerDetails.EMail))
                    { %>
                    <div class="margin-bottom10">
                        <a href="mailto:<%= dealerDetails.EMail %>" class="text-light-grey">
                            <span class="bwmsprite mail-grey-icon vertical-top"></span>
                            <span class="vertical-top dealership-details text-light-grey"><%= dealerDetails.EMail %></span>
                        </a>
                    </div>
                    <% } %>
                    <% if (!string.IsNullOrEmpty(dealerDetails.WorkingHours))
                    { %>
                    <div class="margin-bottom10">
                        <span class="bwmsprite clock-icon vertical-top"></span>
                        <span class="vertical-top dealership-details text-light-grey">Working hours: <%= dealerDetails.WorkingHours %></span>
                    </div>
                    <%} %>
                    <div class="border-solid-bottom margin-bottom15 padding-top10"></div>

                    <h2 class="font14 text-default margin-bottom15">Get commute distance and time:</h2>
                    <div class="form-control-box margin-bottom15">
                        <input id="locationSearch" type="text" class="form-control padding-right40" placeholder="Enter your location" />
                        <span id="getUserLocation" class="crosshair-icon position-abt pos-right10 pos-top10"></span>
                    </div>
                    <div class="location-details margin-bottom15">
                        Distance: <span id="commuteDistance" class="margin-right10"></span>
                        Time: <span id="commuteDuration"></span>
                    </div>
                    <div id="commuteResults"></div>
                    <a id="anchorGetDir" href="http://maps.google.com/maps?z=12&t=m&q=loc:<%= dealerLat %>,<%= dealerLong %>" target="_blank"><span class="bwmsprite get-direction-icon margin-right5"></span>Get directions</a>
                </div>

                <% if (campaignId != 0)
                   { %>
                <div class="grid-12 float-button clearfix float-fixed">
                    <div class="grid-6 alpha omega padding-right5">
                        <a data-leadsourceid="21" class=" btn btn-orange btn-full-width rightfloat leadcapturebtn" href="javascript:void(0);">Get offers</a>
                    </div>
                    <div class="grid-6 alpha omega padding-left5">
                        <a id="calldealer" class="btn btn-green btn-full-width rightfloat" href="tel:<%= dealerDetails.MaskingNumber %>">
                            <span class="bwmsprite tel-white-icon margin-right5"></span>Call dealer</a>
                    </div>
                    <div class="clear"></div>
                </div>
                <div class="clear"></div>
            </div>

            <% } %>
        </section>

        <%if (dealerBikesCount > 0)
          { %>
        <section class="container bg-white margin-bottom10">
            <div class="box-shadow padding-top15 padding-right20 padding-bottom5 padding-left20">
                <h2 class="font18 margin-bottom15">Models available at <%= dealerName %></h2>
                <ul id="model-available-list">
                    <asp:Repeater ID="rptModels" runat="server">
                        <ItemTemplate>
                            <li>
                                <div class="image-block">
                                    <div class="image-content">
                                        <img class="lazy"
                                            data-original="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImagePath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._310x174) %>"
                                            alt="<%# DataBinder.Eval(Container.DataItem, "BikeName") %>" src="http://imgd3.aeplcdn.com/0x0/bw/static/sprites/m/circleloader.gif">
                                    </div>
                                </div>

                                <div class="details-block">
                                    <h3 class="font16 margin-bottom10 text-black text-truncate"><%# DataBinder.Eval(Container.DataItem, "BikeName") %></h3>
                                    <div class="font14 text-x-light margin-bottom10">
                                        <%# Bikewale.Utility.FormatMinSpecs.GetMinSpecs(Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.Displacement")),Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.FuelEfficiencyOverall")),Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.MaxPower"))) %>
                                    </div>
                                    <div>
                                        <span class="bwmsprite inr-sm-icon"></span>
                                        <span class="font18 text-bold"><%# Bikewale.Utility.Format.FormatPrice(Convert.ToString(DataBinder.Eval(Container.DataItem, "VersionPrice"))) %></span>
                                        <span class="font14 text-light-grey"> onwards</span>
                                    </div>                                    
                                </div>                                
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </div>
        </section>
        <%} %>
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>
        <section class="container bg-white margin-bottom10">
            <div class="box-shadow padding-15-20">
                <!-- dealer card -->
                <% if (ctrlDealerCard.showWidget) { %>
                    <BW:DealerCard runat="server" ID="ctrlDealerCard" />
                <% }  %>
            </div>
        </section>
        
       <%-- <!-- Lead Capture pop up start  -->
        <div id="leadCapturePopup" class="bw-popup bwm-fullscreen-popup contact-details hide">
            <div class="popup-inner-container text-center">
                <div class="bwmsprite close-btn leadCapture-close-btn position-abt pos-top20 pos-right20"></div>
                <div id="contactDetailsPopup">
                    <p class="font18 text-bold margin-bottom10">Provide contact details</p>
                    <p class="font14 text-light-grey margin-bottom25">Dealership will get back to you with offers</p>

                    <div class="personal-info-form-container">
                        <div class="dealer-search-brand select-box margin-bottom10">
                            <div class="dealer-search-brand-form"><span>Select a bike<sup>*</sup></span></div>
                            <span class="boundary"></span>
                            <span class="error-text"></span>
                        </div>
                        <div class="input-box form-control-box margin-bottom10">
                            <input type="text" id="getFullName" data-bind="textInput: fullName" />
                            <label for="getFullName">Name<sup>*</sup></label>
                            <span class="boundary"></span>
                            <span class="error-text"></span>
                        </div>
                        <div class="input-box form-control-box margin-bottom10">
                            <input type="email" id="getEmailID" data-bind="textInput: emailId" />
                            <label for="getEmailID">Email<sup>*</sup></label>
                            <span class="boundary"></span>
                            <span class="error-text"></span>
                        </div>
                        <div class="input-box input-number-box form-control-box margin-bottom10">
                            <input type="tel" id="getMobile" data-bind="textInput: mobileNo" maxlength="10" />
                            <label for="getMobile">Mobile number<sup>*</sup></label>
                            <span class="input-number-prefix">+91</span>
                            <span class="boundary"></span>
                            <span class="error-text"></span>
                        </div>
                        <a class="btn btn-orange btn-fixed-width margin-top15" id="user-details-submit-btn" data-bind="event: { click: submitLead }">Submit</a>
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
                            <span class="bwmsprite thankyou-icon margin-top15"></span>
                        </div>
                    </div>
                    <p class="font18 text-bold margin-top20 margin-bottom20">Thank you <span class="notify-leadUser"></span></p>
                    <p class="font16 margin-bottom40">Dealer would get back to you shortly with additional information.</p>
                    <input type="button" id="notifyOkayBtn" class="btn btn-orange" value="Okay" />
                </div>
				<!-- thank you message ends here -->
                <div id="otpPopup">
                    <p class="font18 text-bold margin-bottom10">Verify your mobile number</p>
                    <p class="font14 text-light-grey margin-bottom10">We have sent OTP on your mobile. Please enter that OTP in the box provided below:</p>
                    <div>
                        <div class="lead-mobile-box lead-otp-box-container margin-bottom30">
                            <span class="bwmsprite tel-grey-icon"></span>
                            <span class="text-light-grey font24">+91</span>
                            <span class="lead-mobile font24"></span>
                            <span class="bwmsprite edit-blue-icon edit-mobile-btn"></span>
                        </div>
                        <div class="otp-box lead-otp-box-container">
                            <div class="input-box form-control-box">
                                <input type="tel" id="getOTP" maxlength="5" data-bind="value: otpCode" />
                                <label for="getOTP">One-time password</label>
                                <span class="boundary"></span>
                                <span class="error-text"></span>
                            </div>
                            <div>
                                <a class="margin-left10 blue resend-otp-btn margin-top10" id="resendCwiCode" data-bind="visible: (NoOfAttempts() < 2), click: function () { regenerateOTP() }">Resend OTP</a>
                            </div>
                            <p class="margin-left10 margin-top10 otp-notify-text text-light-grey font12" data-bind="visible: (NoOfAttempts() >= 2)">
                                OTP has been already sent to your mobile
                            </p>
                            <a class="btn btn-fixed-width btn-orange margin-top20" id="otp-submit-btn">Confirm</a>
                        </div>
                        <div class="update-mobile-box padding-top30">
                            <div class="input-box input-number-box form-control-box margin-bottom15">
                                <input type="tel" id="getUpdatedMobile" maxlength="10" data-bind="value: mobileNo" />
                                <label for="getUpdatedMobile">Mobile number<sup>*</sup></label>
                                <span class="input-number-prefix">+91</span>
                                <span class="boundary"></span>
                                <span class="error-text"></span>
                            </div>
                            <input type="button" class="btn btn-orange margin-top20" value="Send OTP" id="generateNewOTP" data-bind="event: { click: submitLead }" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- Lead Capture pop up end  -->
        
        --%>

         <BW:LeadCapture ID="ctrlLeadCapture" runat="server" />


       


        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/common.min.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript">
            var versionId, dealerId = "<%= dealerId %>", cityId = "<%= cityId %>", clientIP = "<%= Bikewale.Common.CommonOpn.GetClientIP()%>",campaignId = "<%= campaignId %>";                                              
             var dealerLat = "<%= dealerLat %>", dealerLong = "<%= dealerLong%>";
             var pqSource = "<%= Convert.ToUInt16(Bikewale.Entities.PriceQuote.PQSourceEnum.Mobile_DealerLocator_Detail) %>";
             var bodHt, footerHt, scrollPosition, leadSourceId;                         
             var googleMapAPIKey = "<%= Bikewale.Utility.BWConfiguration.Instance.GoogleMapApiKey%>";
             var makeName = "<%= makeName%>";
            var cityArea = "<%= dealerCity + "_" + dealerArea%>";
            var pageUrl = window.location.href;

            $(window).scroll(function () {
                bodHt = $('body').height();
                footerHt = $('footer').height();
                scrollPosition = $(this).scrollTop();
                if ($('.float-button').hasClass('float-fixed')) {
                    if (scrollPosition + $(window).height() > (bodHt - footerHt))
                        $('.float-button').removeClass('float-fixed').hide();
                }
                if (scrollPosition + $(window).height() < (bodHt - footerHt))
                    $('.float-button').addClass('float-fixed').show();
            });
           

            
           $(".leadcapturebtn").click(function(e){
               ele = $(this);
               debugger;
               var leadOptions = {
                   "dealerid" : dealerId,                    
                   "leadsourceid" : ele.attr('data-leadsourceid'),
                   "pqsourceid" : ele.attr('data-pqsourceid'),
                   "pageurl" : pageUrl,
                   "clientip" : clientIP,
                   "isregisterpq": true,
                   "isdealerbikes": true,
                   "campid": campaignId
                    
               };

               dleadvm.setOptions(leadOptions);

           });
          

        </script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/dealer/details.js?<%= staticFileVersion %>"></script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />
    </form>
</body>
</html>
