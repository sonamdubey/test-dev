<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.New.NewBikeDealerList" EnableViewState="false" %>
<%@ Register Src="~/m/controls/UsedBikes.ascx" TagName="MostRecentusedBikes" TagPrefix="BW" %>
<%@ Register Src="~/m/controls/MMostPopularBikes.ascx" TagName="PopularBikeMake" TagPrefix="BW" %>
<!DOCTYPE html>
<html>
<head>

    <% 
        keywords = String.Format("{0} showroom {1}, {0} dealers {1}, {1} bike showroom, {1} bike dealers,{1} dealers, {1} bike showroom, bike dealers, bike showroom, dealerships", makeName, cityName);
        description = String.Format("There are {2} {0} dealer showrooms in {1}. Get in touch with {0} showroom for prices, availability, test rides, EMI options and more!", makeName, cityName,totalDealers);
        title = String.Format("{0} Showrooms in {1} | {2} {0} Bike Dealers  - BikeWale", makeName, cityName,totalDealers);
        canonical = String.Format("http://www.bikewale.com/{0}-bikes/dealers-in-{1}/", makeMaskingName, cityMaskingName);              
    %>

    <!-- #include file="/includes/headscript_mobile_min.aspx" -->
    <style type="text/css">
        .swiper-card,.swiper-slide:first-child{margin-left:5px}#dealersList a:hover,.swiper-card a:hover{text-decoration:none}.padding-15-20{padding:15px 20px}#dealersList{padding:0 20px 20px}#dealersList li{border-top:1px solid #e2e2e2;padding-top:15px;margin-top:20px;font-size:14px}#dealersList li:first-child{border-top:0;margin-top:0}#dealersList a{display:block}.featured-tag{width:74px;text-align:center;background:#3799a7;margin-bottom:5px;z-index:1;font-size:12px;color:#fff;line-height:20px;-webkit-border-radius:2px;-moz-border-radius:2px;-o-border-radius:2px;border-radius:2px}.vertical-top{display:inline-block;vertical-align:top}.dealership-details{width:92%}.get-assistance-btn.btn{font-size:14px;padding:9px 21px}.dealership-loc-icon{width:10px;height:14px;background-position:-40px -436px;position:relative;top:4px;margin-right:3px}.star-white{width:8px;height:8px;background-position:-174px -447px;margin-right:4px}.tel-sm-grey-icon{width:10px;height:10px;background-position:0 -437px;position:relative;top:5px;margin-right:3px}.card-container{padding-top:5px;padding-bottom:5px}.card-container .swiper-slide{width:200px}.swiper-card{width:200px;min-height:210px;border:1px solid #e2e2e2\9;background:#fff;-webkit-box-shadow:0 1px 4px rgba(0,0,0,.2);-moz-box-shadow:0 1px 4px rgba(0,0,0,.2);-ms-box-shadow:0 1px 4px rgba(0,0,0,.2);box-shadow:0 1px 4px rgba(0,0,0,.2);-webkit-border-radius:2px;-moz-border-radius:2px;-ms-border-radius:2px;border-radius:2px}.swiper-image-preview{height:95px;padding:5px 5px 0}.swiper-image-preview img{height:90px}.padding-10-15{padding:10px 15px}.btn-card{padding:6px;overflow:hidden}.text-truncate{width:100%;text-align:left;text-overflow:ellipsis;white-space:nowrap;overflow:hidden}#makeTabsContentWrapper h2{margin-bottom:13px}
    </style>
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_mobile.aspx" -->

        var makeName = "<%=makeName%>";
        var makeMaskingName = "<%=makeMaskingName%>";
        var makeId = "<%=makeId%>";
        var cityName = "<%=cityName%>";
        var cityId = "<%= cityId%>";
        var cityMaskingName = "<%= cityMaskingName%>";
        var clientIP = "<%= clientIP %>";
        var pageUrl = "<%= pageUrl%>";
        var pqSrcId = "<%= Convert.ToUInt16(Bikewale.Entities.PriceQuote.PQSourceEnum.Mobile_DealerLocator_Listing) %>";
        var pageSrcId = eval("<%= Bikewale.Utility.BWConfiguration.Instance.SourceId %>");
        var googleMapAPIKey = "<%= Bikewale.Utility.BWConfiguration.Instance.GoogleMapApiKey %>";
    </script>
</head>
<body class="bg-light-grey">
    <form runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->

        <section>
            <div class="container margin-bottom10">
                <div class="bg-white">
                    <h1 class="box-shadow padding-15-20"><%=makeName%> Dealer Showrooms in <%=cityName%></h1>
                    <div class="box-shadow font14 text-light-grey padding-15-20">
                        <%=makeName %> has <%=totalDealers %> authorized dealers in <%= cityName %>. Apart from the authorized dealerships, <%= makeName %> bikes are also available at unauthorized showrooms and broker outlets.
                    </div>
                </div>
            </div>
        </section>

        <section>
            <div class="container bg-white box-shadow margin-bottom10">
                <h2 class="font16 text-black text-bold padding-15-20 border-solid-bottom"><%=totalDealers %> <%=makeName%> showrooms in <%=cityName%></h2>
                <ul id="dealersList">
                    <asp:Repeater ID="rptDealers" runat="server">
                        <ItemTemplate>
                            <li>
                                
                                <div class="<%# ((DataBinder.Eval(Container.DataItem,"DealerType").ToString() == "3") || (DataBinder.Eval(Container.DataItem,"DealerType").ToString() == "2"))? "" : "hide" %> featured-tag">
                                    <span class="bwmsprite star-white"></span>Featured
                                </div>
                                <h3 class="text-truncate margin-bottom5">
                                    <%# GetDealerDetailLink(DataBinder.Eval(Container.DataItem,"DealerType").ToString(), DataBinder.Eval(Container.DataItem,"DealerId").ToString(), DataBinder.Eval(Container.DataItem,"CampaignId").ToString(), DataBinder.Eval(Container.DataItem,"Name").ToString()) %>
                                </h3>
                                <p class="<%# (String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"Address").ToString()))?"hide":"margin-bottom5" %>">
                                    <span class="bwmsprite dealership-loc-icon vertical-top"></span>
                                    <span class="vertical-top dealership-details text-light-grey"><%# DataBinder.Eval(Container.DataItem,"Address") %></span>
                                </p>                                
                                <div class="<%# (String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"MaskingNumber").ToString()))?"hide":"" %>">
                                    <a href="tel:<%#DataBinder.Eval(Container.DataItem,"MaskingNumber").ToString() %>" class="text-default text-bold maskingNumber">
                                        <span class="vertical-top bwmsprite tel-sm-grey-icon"></span>
                                        <span class="vertical-top dealership-details"><%# DataBinder.Eval(Container.DataItem,"MaskingNumber").ToString() %></span>
                                    </a>
                                </div>
                                <input leadSourceId="20" data-item-id="<%# DataBinder.Eval(Container.DataItem,"DealerId") %>" data-item-type="<%# (DataBinder.Eval(Container.DataItem,"DealerType")) %>" campId="<%# (DataBinder.Eval(Container.DataItem,"CampaignId")) %>" type="button" class="btn btn-white margin-top10 get-assistance-btn <%# ((DataBinder.Eval(Container.DataItem,"DealerType").ToString() == "3") || (DataBinder.Eval(Container.DataItem,"DealerType").ToString() == "2"))? "" : "hide" %>" value="Get offers from dealer">
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </div>
        </section>

        <section>
            <div class="container bg-white box-shadow margin-bottom15">
                
 <div class="container bg-white box-shadow margin-bottom15">
                <% if (ctrlPopoularBikeMake.FetchedRecordsCount > 0)
                                   {%> 
                 <BW:PopularBikeMake runat="server" ID="ctrlPopoularBikeMake" />
                <%} %>
                    
                
                <div class="padding-top20 padding-bottom20 text-center">
                    <!-- Ad -->
                </div>
                <div class="margin-right10 margin-left10 border-solid-bottom"></div>

                <% if (ctrlRecentUsedBikes.fetchedCount > 0)
                                   {%> 
                 <BW:MostRecentUsedBikes runat="server" ID="ctrlRecentUsedBikes" />
                <%} %>
            </div>
                </div>
            
        </section>

        <section>
            <div class="container margin-bottom30">
                <div class="grid-12 font12">
                    <span class="font14"><strong>Disclaimer:</strong></span> The above mentioned information about <%=makeName %> dealership showrooms in <%=cityName %> is furnished to the best of our knowledge. 
                        All <%=makeName %> bike models and colour options may not be available at each of the <%=makeName %> dealers. 
                        We recommend that you call and check with your nearest <%=makeName %> dealer before scheduling a showroom visit.
                </div>
                <div class="clear"></div>
            </div>
        </section>

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
                            <ul id="sliderBrandList" class="slider-brand-list margin-top40"  data-bind="foreach: bikes">
                               <li data-bind="text: bike, click: function () { customerViewModel.versionId(this.version.versionId); customerViewModel.modelId(this.model.modelId); customerViewModel.selectedBikeName(this.make.makeName + ' ' + this.model.modelName + '_' + this.version.versionName);  }"></li>
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
                    <p class="font18 text-bold margin-top20 margin-bottom20">Thank you</p>
                    <p class="font16 margin-bottom40"><span class="notify-dealerName"></span> would get back to you shortly with additional information.</p>
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
    
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/common.min.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/dealer/listing.js?<%= staticFileVersion %>"></script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />
        
    </form>
</body>
</html>
