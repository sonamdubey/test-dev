<%@ Page Language="C#" Inherits="Bikewale.New.BrowseNewBikeDealerDetails" AutoEventWireup="false" EnableViewState="false" %>

<!DOCTYPE html>
<html>
<head>
    <%
        isAd970x90Shown = false;
        //keywords = makeName + " dealers city, Make showrooms " + strCity + "," + strCity + " bike dealers, " + makeName + " dealers, " + strCity + " bike showrooms, bike dealers, bike showrooms, dealerships";
        //description = makeName + " bike dealers/showrooms in " + strCity + ". Find " + makeName + " bike dealer information for more than 200 cities. Dealer information includes full address, phone numbers, email, pin code etc.";
        //title = makeName + " Dealers in city | " + makeName + " New bike Showrooms in " + strCity + " - BikeWale";
        //canonical = "http://www.bikewale.com/new/" + MakeMaskingName + "-dealers/" + cityId + "-" + strCity + ".html";
        //alternate = "http://www.bikewale.com/m/new/" + MakeMaskingName + "-dealers/" + cityId + "-" + strCity + ".html";
        //AdId = "1395986297721";
        //AdPath = "/1017752/BikeWale_New_";
        //isAd970x90Shown = false;
    %>
    <title></title>
    <!-- #include file="/includes/headscript.aspx" -->
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/dealerlisting.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
    <script src="http://maps.googleapis.com/maps/api/js?key=AIzaSyDjG8tpNdQI86DH__-woOokTaknrDQkMC8"></script>
</head>
<body class="bg-light-grey padding-top50">
    <form runat="server">
        <!-- #include file="/includes/headBW.aspx" -->
        <section>
            <div class="opacity0 grid-12 padding-right20 padding-left20">
                <div class="breadcrumb">
                    <!-- breadcrumb code starts here -->
                    <ul>
                        <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                            <a itemprop="url" href="/"><span itemprop="title">Home</span></a>
                        </li>
                        <li class="fwd-arrow">&rsaquo;</li>
                        <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                            <a itemprop="url" href="/new/"><span itemprop="title">New Bikes</span></a>
                        </li>
                        <li class="fwd-arrow">&rsaquo;</li>
                        <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                            <a itemprop="url" href="/new/locate-dealers/"><span itemprop="title">New Bike Dealer</span></a>
                        </li>
                        <li class="fwd-arrow">&rsaquo;</li>
                        <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                            <a itemprop="url" href="/new/<%=makeMaskingName %>-dealers/"><span itemprop="title"><%=makeName%> Dealers</span></a>
                        </li>
                        <li class="fwd-arrow">&rsaquo;</li>
                        <li class="current"><strong><%=makeName%> Dealers in <%=cityName%></strong></li>
                    </ul>
                    <div class="clear"></div>
                </div>
            </div>
            <div class="clear"></div>
        </section>

        <section>
            <div class="dealer-filter-dropdown grid-11 text-center margin-left30">
                <div id="locateDealerFilter" class="box-light-shadow bg-white content-inner-block-12">
                    <p class="font16 leftfloat margin-top8 margin-right20">Locate dealers:</p>
                    <div class="leftfloat margin-right10">
                        <div class="brand-city-filter form-control-box">
                            <select id="ddlMakes" class="form-control  chosen-select">
                                <asp:Repeater ID="rptMakes" runat="server">
                                    <ItemTemplate>
                                        <option maskingname="<%# DataBinder.Eval(Container.DataItem,"MaskingName") %>" value="<%# DataBinder.Eval(Container.DataItem,"MakeId") %>" <%# ((DataBinder.Eval(Container.DataItem,"MakeId")) != makeId.ToString())?string.Empty:"selected" %>><%# DataBinder.Eval(Container.DataItem,"MakeName") %> </option>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </select>
                            <div class="clear"></div>
                        </div>
                    </div>
                    <div class="leftfloat margin-right10">
                        <div class="brand-city-filter form-control-box">
                            <select id="ddlCities" class="form-control  chosen-select">
                                <asp:Repeater ID="rptCities" runat="server">
                                    <ItemTemplate>
                                        <option maskingname="<%# DataBinder.Eval(Container.DataItem,"CityMaskingName") %>" value="<%# DataBinder.Eval(Container.DataItem,"CityId") %>" <%# ((DataBinder.Eval(Container.DataItem,"CityId")) != cityId.ToString())?string.Empty:"selected" %>><%# DataBinder.Eval(Container.DataItem,"CityName") %></option>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </select>
                            <div class="clear"></div>
                        </div>
                    </div>
                    <input type="button" id="applyFiltersBtn" class="btn btn-orange leftfloat" value="Apply" />
                    <div class="clear"></div>
                </div>
            </div>
            <div class="clear"></div>
        </section>

        <section>
            <div class="grid-12 alpha omega">

                <div id="dealerListingSidebar" class="bg-white position-abt pos-right0">
                    <div class="dealerSidebarHeading padding-top15 padding-right20 padding-left20">
                        <h1 id="sidebarHeader" class="font16 border-solid-bottom padding-bottom15"><%= makeName %> dealers in <%= cityName %> <span class="font14 text-light-grey">(<%= totalDealers %>)</span></h1>
                    </div>
                    <ul id="dealersList">
                        <asp:Repeater ID="rptDealers" runat="server">
                            <ItemTemplate>
                                <li data-item-id="<%# DataBinder.Eval(Container.DataItem,"DealerId") %>" data-lat="<%# DataBinder.Eval(Container.DataItem,"objArea.Latitude") %>" data-log="<%# DataBinder.Eval(Container.DataItem,"objArea.Longitude") %>" data-address ="<%# DataBinder.Eval(Container.DataItem,"Address") %>">
                                    <div class="font14">
                                        <h2 class="font16 margin-bottom10">
                                            <span class="<%# ((int)DataBinder.Eval(Container.DataItem,"DealerPkgType")!=1)?"":"hide" %> featured-tag text-white text-center font14 margin-bottom5">Featured
                                            </span>
                                            <span class="<%# ((int)DataBinder.Eval(Container.DataItem,"DealerPkgType")!=1)?"":"hide" %> dealer-pointer-arrow"></span>
                                            <a href="javascript:void(0)" class="dealer-sidebar-link text-black text-bold"><%# DataBinder.Eval(Container.DataItem,"Name") %></a>
                                        </h2>
                                        <p class="text-light-grey margin-bottom5"><%# (String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"objArea.AreaName").ToString()))?"":DataBinder.Eval(Container.DataItem,"objArea.AreaName") + "," %> <%# DataBinder.Eval(Container.DataItem,"City") %></p>
                                        <div class="<%# (String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"MaskingNumber").ToString()))?"hide":string.Empty %>"><p class="text-light-grey margin-bottom5"><span class="bwsprite phone-grey-icon"></span><%# DataBinder.Eval(Container.DataItem,"MaskingNumber") %></p>  </div>
                                        <div class="<%# (String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"Email").ToString()))?"hide":string.Empty %>"><a href="mailto:<%# DataBinder.Eval(Container.DataItem,"Email") %>" class="text-light-grey"><span class="bwsprite mail-grey-icon"></span><%# DataBinder.Eval(Container.DataItem,"Email") %></a> </div>
                                        <a href="Javascript:void(0)" class="btn btn-white-orange margin-top15 get-assistance-btn">Get assistance</a>
                                    </div>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>

                        <li class="dummy-card"></li>
                    </ul>
                </div>

                <div id="dealerDetailsSliderCard" class="bg-white font14">
                    <div class="dealer-slider-close-btn position-abt pos-top20 pos-right20 bwsprite cross-lg-lgt-grey cur-pointer"></div>
                    <div class="padding-top20 padding-right20 padding-left20">
                        <p class="featured-tag text-white text-center margin-bottom5">
                            Featured
                        </p>
                        <div class="padding-bottom20 border-solid-bottom">
                            <h3 class="font18 text-dark-black margin-bottom10">Kamala Landmarc Motorbikes</h3>
                            <p class="text-light-grey margin-bottom5">Vishwaroop IT Park, Sector 30, Navi Mumbai, Maharashtra, 400067</p>
                            <div class="margin-bottom5">
                                <span class="font16 text-bold margin-right10"><span class="bwsprite phone-black-icon"></span>9876543210</span>
                                <a href="mailto:bikewale@motors.com" class="text-light-grey"><span class="bwsprite mail-grey-icon"></span>bikewale@motors.com</a>
                            </div>
                            <p class="text-light-grey margin-bottom5">Working hours: Monday- Saturday 9.00 am- 6.00 pm<span class="border-dark-left margin-left10 padding-left10">Sunday 9.00 am- 2.00 pm</span></p>
                            <a href=""><span class="bwsprite get-direction-icon"></span>Get directions</a>
                            <a href="" class="border-dark-left margin-left10 padding-left10"><span class="bwsprite sendto-phone-icon"></span>Send to phone</a>
                        </div>
                        <div class="padding-top15">
                            <p class="font14 text-bold margin-bottom15">Get commute distance and time:</p>
                            <div class="commute-distance-form form-control-box">
                                <input type="text" class="form-control" placeholder="Enter your location" />
                            </div>
                        </div>
                    </div>
                    <div id="buyingAssistanceForm" class="margin-top20 content-inner-block-1520">
                        <p class="font14 text-bold margin-bottom15">Get buying assistance from this dealer:</p>
                        <div class="name-email-mobile-box form-control-box leftfloat margin-right20">
                            <input type="text" class="form-control" placeholder="Name" id="assistGetName" data-bind="textInput: fullName" />
                            <span class="bwsprite error-icon errorIcon"></span>
                            <div class="bw-blackbg-tooltip errorText"></div>
                        </div>
                        <div class="name-email-mobile-box form-control-box leftfloat margin-right40">
                            <input type="text" class="form-control" placeholder="Email id" id="assistGetEmail"  data-bind="textInput: emailId" />
                            <span class="bwsprite error-icon errorIcon"></span>
                            <div class="bw-blackbg-tooltip errorText"></div>
                        </div>
                        <div class="name-email-mobile-box form-control-box leftfloat">
                            <p class="mobile-prefix">+91</p>
                            <input type="text" class="mobile-box form-control" placeholder="Mobile" maxlength="10" id="assistGetMobile"  data-bind="textInput: mobileNo" />
                            <span class="bwsprite error-icon errorIcon"></span>
                            <div class="bw-blackbg-tooltip errorText"></div>
                        </div>
                        <div class="clear"></div>
                        <div class="margin-top20">
                            <div class="select-model-box form-control-box leftfloat margin-right40">
                                <input type="text" class="form-control" placeholder="Type to select model" id="assistGetModel" />
                                <span class="bwsprite error-icon errorIcon"></span>
                                <div class="bw-blackbg-tooltip errorText"></div>
                            </div>
                            <input type="button" class="btn btn-orange btn-md" id="submitAssistanceFormBtn" value="Submit"  data-bind="event: { click: submitLead }" />
                        </div>
                        <div class="clear"></div>
                        <div class="hide">
                            <p class="leftfloat font14">Thank you for your interest. Kamala Landmarc Motorbikes will get in touch shortly</p>
                            <span class="rightfloat bwsprite cross-lg-lgt-grey cur-pointer"></span>
                            <div class="clear"></div>
                        </div>
                    </div>
                    <div>
                        <p class="font14 text-bold padding-top20 padding-right20 padding-left20 margin-bottom15">Models available with the dealer:</p>
                        <ul id="modelsAvailable">
                            <li>
                                <div class="contentWrapper">
                                    <div class="imageWrapper">
                                        <a href="">
                                            <img title="" alt="" src="http://imgd1.aeplcdn.com//310X174//bw/models/royal-enfield-classic-350-standard-136.jpg?20151209202137">
                                        </a>
                                    </div>
                                    <div class="bikeDescWrapper">
                                        <div class="bikeTitle margin-bottom7">
                                            <h3 class="font16 text-dark-black"><a href="" title="">Royal Enfield Classic 350</a></h3>
                                        </div>
                                        <div class="font16 text-bold margin-bottom5">
                                            <span class="fa fa-rupee"></span>
                                            <span class="font18">1,29,356</span> <span class="font16">onwards</span>
                                        </div>
                                        <div class="font14 text-light-grey">
                                            <span>346 CC, 37 Kmpl, 19.8 bhp</span>
                                        </div>
                                    </div>
                                </div>
                            </li>
                            <li>
                                <div class="contentWrapper">
                                    <div class="imageWrapper">
                                        <a href="">
                                            <img title="" alt="" src="http://imgd1.aeplcdn.com//310X174//bw/models/honda-cb-shine-kick/drum/spokes-111.jpg?20151209184344">
                                        </a>
                                    </div>
                                    <div class="bikeDescWrapper">
                                        <div class="bikeTitle margin-bottom7">
                                            <h3 class="font16 text-dark-black"><a href="" title="">Royal Enfield Classic 350</a></h3>
                                        </div>
                                        <div class="font16 text-bold margin-bottom5">
                                            <span class="fa fa-rupee"></span>
                                            <span class="font18">1,29,356</span> <span class="font16">onwards</span>
                                        </div>
                                        <div class="font14 text-light-grey">
                                            <span>346 CC, 37 Kmpl, 19.8 bhp</span>
                                        </div>
                                    </div>
                                </div>
                            </li>
                            <li>
                                <div class="contentWrapper">
                                    <div class="imageWrapper">
                                        <a href="">
                                            <img title="" alt="" src="http://imgd1.aeplcdn.com//310X174//bw/models/bajaj-pulsar-rs200.jpg?20150710124439">
                                        </a>
                                    </div>
                                    <div class="bikeDescWrapper">
                                        <div class="bikeTitle margin-bottom7">
                                            <h3 class="font16 text-dark-black"><a href="" title="">Royal Enfield Classic 350</a></h3>
                                        </div>
                                        <div class="font16 text-bold margin-bottom5">
                                            <span class="fa fa-rupee"></span>
                                            <span class="font18">1,29,356</span> <span class="font16">onwards</span>
                                        </div>
                                        <div class="font14 text-light-grey">
                                            <span>346 CC, 37 Kmpl, 19.8 bhp</span>
                                        </div>
                                    </div>
                                </div>
                            </li>
                            <li>
                                <div class="contentWrapper">
                                    <div class="imageWrapper">
                                        <a href="">
                                            <img title="" alt="" src="http://imgd2.aeplcdn.com//310X174//bw/models/honda-cb-hornet-160r.jpg?20151012195209">
                                        </a>
                                    </div>
                                    <div class="bikeDescWrapper">
                                        <div class="bikeTitle margin-bottom7">
                                            <h3 class="font16 text-dark-black"><a href="" title="">Royal Enfield Classic 350</a></h3>
                                        </div>
                                        <div class="font16 text-bold margin-bottom5">
                                            <span class="fa fa-rupee"></span>
                                            <span class="font18">1,29,356</span> <span class="font16">onwards</span>
                                        </div>
                                        <div class="font14 text-light-grey">
                                            <span>346 CC, 37 Kmpl, 19.8 bhp</span>
                                        </div>
                                    </div>
                                </div>
                            </li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                </div>

            </div>
            <div class="clear"></div>
        </section>

        <!-- lead capture popup start-->
        <div id="leadCapturePopup" class="text-center rounded-corner2">
            <div class="leadCapture-close-btn position-abt pos-top10 pos-right10 bwsprite cross-lg-lgt-grey cur-pointer"></div>
            <!-- contact details starts here -->
            <div id="contactDetailsPopup">
                <div class="icon-outer-container rounded-corner50">
                    <div class="icon-inner-container rounded-corner50">
                        <span class="bwsprite user-contact-details-icon margin-top25"></span>
                    </div>
                </div>
                <p class="font20 margin-top25 margin-bottom10">Provide contact details</p>
                <p class="text-light-grey margin-bottom20">For you to see more details about this bike, please submit your valid contact details. It will be safe with us.</p>
                <div class="personal-info-form-container">
                    <div class="form-control-box personal-info-list">
                        <input type="text" class="form-control get-first-name" placeholder="Full name (mandatory)"
                            id="getFullName" data-bind="textInput: fullName">
                        <span class="bwsprite error-icon errorIcon"></span>
                        <div class="bw-blackbg-tooltip errorText">Please enter your first name</div>
                    </div>
                    <div class="form-control-box personal-info-list">
                        <input type="text" class="form-control get-email-id" placeholder="Email address (mandatory)"
                            id="getEmailID" data-bind="textInput: emailId">
                        <span class="bwsprite error-icon errorIcon"></span>
                        <div class="bw-blackbg-tooltip errorText">Please enter email address</div>
                    </div>
                    <div class="form-control-box personal-info-list">
                        <p class="mobile-prefix">+91</p>
                        <input type="text" class="form-control padding-left40 get-mobile-no" placeholder="Mobile no. (mandatory)"
                            id="getMobile" maxlength="10" data-bind="textInput: mobileNo">
                        <span class="bwsprite error-icon errorIcon"></span>
                        <div class="bw-blackbg-tooltip errorText">Please enter mobile number</div>
                    </div>
                    <div class="clear"></div>
                    <a class="btn btn-orange margin-top10" id="user-details-submit-btn" data-bind="event: { click: submitLead }">Submit</a>
                </div>
            </div>
            <!-- contact details ends here -->
            <!-- otp starts here -->
            <div id="otpPopup">
                <div class="icon-outer-container rounded-corner50">
                    <div class="icon-inner-container rounded-corner50">
                        <span class="bwsprite otp-icon margin-top25"></span>
                    </div>
                </div>
                <p class="font18 margin-top25 margin-bottom20">Verify your mobile number</p>
                <p class="font14 text-light-grey margin-bottom20">We have sent an OTP on the following mobile number. Please enter that OTP in the box provided below:</p>
                <div>
                    <div class="lead-mobile-box lead-otp-box-container font22">
                        <span class="fa fa-phone"></span>
                        <span class="text-light-grey font24">+91</span>
                        <span class="lead-mobile font24"></span>
                        <span class="bwsprite edit-blue-icon edit-mobile-btn"></span>
                    </div>
                    <div class="otp-box lead-otp-box-container">
                        <div class="form-control-box margin-bottom10">
                            <input type="text" class="form-control" maxlength="5" placeholder="Enter your OTP" id="getOTP" data-bind="value: otpCode">
                            <span class="bwsprite error-icon errorIcon"></span>
                            <div class="bw-blackbg-tooltip errorText"></div>
                        </div>
                        <a class="resend-otp-btn margin-left10 blue rightfloat resend-otp-btn" id="resendCwiCode" data-bind="visible: (NoOfAttempts() < 2), click: function () { regenerateOTP() }">Resend OTP
                        </a>
                        <p class="otp-alert-text margin-left10 otp-notify-text text-light-grey font12 margin-top10" data-bind="visible: (NoOfAttempts() >= 2)">
                            OTP has been already sent to your mobile
                        </p>
                        <div class="clear"></div>
                        <input type="button" class="btn btn-orange margin-top20" value="Confirm OTP" id="otp-submit-btn">
                    </div>
                    <div class="update-mobile-box">
                        <div class="form-control-box text-left">
                            <p class="mobile-prefix">+91</p>
                            <input type="text" class="form-control padding-left40" placeholder="Mobile no." maxlength="10" id="getUpdatedMobile" data-bind="value: mobileNo" />
                            <span class="bwsprite error-icon errorIcon"></span>
                            <div class="bw-blackbg-tooltip errorText"></div>
                        </div>
                        <input type="button" class="btn btn-orange" value="Send OTP" id="generateNewOTP" data-bind="event: { click: submitLead }" />
                    </div>
                </div>
            </div>
            <!-- otp ends here -->
            <div id="dealer-lead-msg" class="hide">
                <div class="icon-outer-container rounded-corner50">
                    <div class="icon-inner-container rounded-corner50">
                        <span class="bwsprite otp-icon margin-top25"></span>
                    </div>
                </div>
                <p class="font18 margin-top25 margin-bottom20">Thank you for providing your details. DealerName, DealerArea will get in touch with you soon.</p>

                <a href="javascript:void(0)" class="btn btn-orange okay-thanks-msg">Okay</a>
            </div>
        </div>
        <!-- lead capture popup End-->

        <section>
            <div class="grid-12 alpha omega">
                <div class="dealer-map-wrapper">
                    <div id="dealerMapWrapper" style="position: fixed; top: 50px; width: 100%; height: 530px;">
                        <div id="dealersMap" style="width: 100%; height: 530px;">
                        </div>
                    </div>
                </div>
            </div>
            <div class="clear"></div>
        </section>

        <!-- #include file="/includes/footerBW.aspx" -->
        <!-- #include file="/includes/footerscript.aspx" -->
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/dealerlisting.js?<%= staticFileVersion %>1234"></script>
        <script type="text/javascript">
            $ddlCities = $("#ddlCities");
            $ddlMakes = $("#ddlMakes");

            $("#applyFiltersBtn").click(function () {
                window.location.href = "/new/";
            });

            $ddlCities.chosen({ no_results_text: "No matches found!!" });
            $ddlMakes.chosen({ no_results_text: "No matches found!!" });
            $('div.chosen-container').attr('style', 'width:100%;border:0');
            $("#bookingAreasList_chosen .chosen-single.chosen-default span").text("Please Select City");

        </script>
    </form>
</body>
</html>
