<%@ Page Language="C#" Inherits="Bikewale.New.BrowseNewBikeDealerDetails" AutoEventWireup="false" EnableViewState="false" %>

<!DOCTYPE html>
<html>
<head>
    <%
        isAd970x90Shown = false;

        keywords = String.Format("{0} dealers city, Make showrooms  {1}, {1} bike dealers, {0} dealers, {1} bike showrooms, bike dealers, bike showrooms, dealerships", makeName, cityName);
        description = String.Format("{0} bike dealers/showrooms in {1}. Find {0} bike dealer information for more than 200 cities. Dealer information includes full address, phone numbers, email, pin code etc", makeName, cityName);
        title = String.Format("{0} Dealers in {1} city | {0} New bike Showrooms in {1} - BikeWale", makeName, cityName);
        canonical = String.Format("http://www.bikewale.com/new/{0}-dealers/{1}-{2}.html", makeMaskingName, cityId, cityMaskingName);
        alternate = String.Format("http://www.bikewale.com/m/new/{0}-dealers/{1}-{2}.html", makeMaskingName, cityId, cityMaskingName);
        AdId = "1395986297721";
        AdPath = "/1017752/BikeWale_New_";
        isAd970x90Shown = false;
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
            <div class="dealer-filter-dropdown grid-11 text-center">
                <div id="locateDealerFilter" class="box-light-shadow bg-white content-inner-block-12">
                    <p class="font16 leftfloat margin-top8 margin-right20">Locate dealers:</p>
                    <div class="leftfloat margin-right10">
                        <div class="brand-city-filter form-control-box">
                            <select id="ddlMakes" class="form-control  chosen-select">
                                <asp:Repeater ID="rptMakes" runat="server">
                                    <ItemTemplate>
                                        <option maskingname="<%# DataBinder.Eval(Container.DataItem,"MaskingName") %>" value="<%# DataBinder.Eval(Container.DataItem,"MakeId") %>" <%# ((DataBinder.Eval(Container.DataItem,"MakeId")).ToString() != makeId.ToString())?string.Empty:"selected" %>><%# DataBinder.Eval(Container.DataItem,"MakeName") %> </option>
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
                                        <option maskingname="<%# DataBinder.Eval(Container.DataItem,"CityMaskingName") %>" value="<%# DataBinder.Eval(Container.DataItem,"CityId") %>" <%# ((DataBinder.Eval(Container.DataItem,"CityId")).ToString() != cityId.ToString())?string.Empty:"selected" %>><%# DataBinder.Eval(Container.DataItem,"CityName") %></option>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </select>
                            <div class="clear"></div>
                            <span class="bwsprite error-icon errorIcon hide"></span>
                            <div class="bw-blackbg-tooltip errorText hide"></div>
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
                                <li data-item-type="<%# (DataBinder.Eval(Container.DataItem,"DealerType")) %>" data-item-id="<%# DataBinder.Eval(Container.DataItem,"DealerId") %>" data-item-inquired="false" data-item-number="<%# DataBinder.Eval(Container.DataItem,"MaskingNumber") %>" data-lat="<%# DataBinder.Eval(Container.DataItem,"objArea.Latitude") %>" data-log="<%# DataBinder.Eval(Container.DataItem,"objArea.Longitude") %>" data-address="<%# DataBinder.Eval(Container.DataItem,"Address") %>">
                                    <div class="font14">
                                        <h2 class="font16 margin-bottom10">
                                            <div class="<%# ((DataBinder.Eval(Container.DataItem,"DealerType").ToString() == "3") || (DataBinder.Eval(Container.DataItem,"DealerType").ToString() == "2"))? "" : "hide" %>">
                                                <span class="featured-tag text-white text-center font14 margin-bottom5">Featured
                                                </span>
                                                <span class="dealer-pointer-arrow"></span>
                                            </div>
                                            <a href="javascript:void(0)" class="dealer-sidebar-link text-black text-bold"><%# DataBinder.Eval(Container.DataItem,"Name") %></a>
                                        </h2>
                                        <p class="text-light-grey margin-bottom5"><%# (String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"objArea.AreaName").ToString()))?"":DataBinder.Eval(Container.DataItem,"objArea.AreaName") + "," %> <%# DataBinder.Eval(Container.DataItem,"City") %></p>

                                        <div class="<%# (String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"MaskingNumber").ToString()))?"hide":string.Empty %>">
                                            <p class="text-light-grey margin-bottom5"><span class="bwsprite phone-grey-icon"></span><%# DataBinder.Eval(Container.DataItem,"MaskingNumber") %></p>
                                        </div>

                                        <div class="<%# (String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"Email").ToString()))?"hide":string.Empty %>">
                                            <a href="mailto:<%# DataBinder.Eval(Container.DataItem,"Email") %>" class="text-light-grey">
                                                <span class="bwsprite mail-grey-icon"></span><%# DataBinder.Eval(Container.DataItem,"Email") %></a>
                                        </div>

                                        <div class="<%# (DataBinder.Eval(Container.DataItem,"DealerType").ToString() == "0")?"hide":"" %>">
                                            <a data-item-id="<%# DataBinder.Eval(Container.DataItem,"DealerId") %>" href="Javascript:void(0)" class="btn btn-white-orange margin-top15 get-assistance-btn">Get assistance</a>
                                        </div>
                                    </div>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>

                        <li class="dummy-card"></li>
                    </ul>
                </div>
                <div id="dealerInfo">
                    <div id="dealerDetailsSliderCard" class="bg-white font14">
                        <div class="dealer-slider-close-btn position-abt pos-top20 pos-right20 bwsprite cross-lg-lgt-grey cur-pointer"></div>
                        <div class="padding-top20 padding-right20 padding-left20" data-bind="with: DealerDetails">
                            <p class="featured-tag text-white text-center margin-bottom5">
                                Featured
                            </p>
                            <div class="padding-bottom20 ">
                                <h3 class="font18 text-dark-black margin-bottom10" data-bind="text: name"></h3>
                                <p class="text-light-grey margin-bottom5" data-bind="visible :address() && address().length > 0,text: address()"></p>
                                <div class="margin-bottom5">
                                    <span class="font16 text-bold margin-right10" data-bind="visible : mobile() && mobile().length > 0"><span class="bwsprite phone-black-icon"></span><span data-bind="    text: mobile()"></span></span>
                                    <a href="#" class="text-light-grey" data-bind="visible : email() && email().length > 0,attr : { href :'mailto:' + email() }"><span class="bwsprite mail-grey-icon"></span><span data-bind="    text: email()"></span></a>
                                </div>

                                <p class="text-light-grey margin-bottom5" data-bind="visible: workingHours && workingHours.length > 0,text : workingHours">Working Hours : </p>
                                <a href="" target="_blank" data-bind="attr : { href : 'https://maps.google.com/?saddr=' + userLocation + '&daddr=' + lat() + ',' + lng() + '' }"><span class="bwsprite get-direction-icon"></span>Get directions</a>
                                <%-- <a href="" class="border-dark-left margin-left10 padding-left10"><span class="bwsprite sendto-phone-icon"></span>Send to phone</a>--%>
                            </div>
                            <%-- <div class="padding-top15">
                            <p class="font14 text-bold margin-bottom15">Get commute distance and time:</p>
                            <div class="commute-distance-form form-control-box">
                                <input type="text" class="form-control" placeholder="Enter your location" />
                            </div>
                        </div>--%>
                        </div>
                        <div id="buyingAssistanceForm" data-bind="with: CustomerDetails" class="border-solid-top content-inner-block-1520">
                            <div id="buying-assistance-form">
                                <p class="font14 text-bold margin-bottom15">Get buying assistance from this dealer:</p>
                                <div class="name-email-mobile-box form-control-box leftfloat margin-right20">
                                    <input type="text" class="form-control" placeholder="Name" id="assistanceGetName" data-bind="textInput: fullName" />
                                    <span class="bwsprite error-icon errorIcon"></span>
                                    <div class="bw-blackbg-tooltip errorText"></div>
                                </div>
                                <div class="name-email-mobile-box form-control-box leftfloat margin-right40">
                                    <input type="text" class="form-control" placeholder="Email id" id="assistanceGetEmail" data-bind="textInput: emailId" />
                                    <span class="bwsprite error-icon errorIcon"></span>
                                    <div class="bw-blackbg-tooltip errorText"></div>
                                </div>
                                <div class="name-email-mobile-box form-control-box leftfloat">
                                    <p class="mobile-prefix">+91</p>
                                    <input type="text" class="mobile-box form-control" placeholder="Mobile" maxlength="10" id="assistanceGetMobile" data-bind="textInput: mobileNo" />
                                    <span class="bwsprite error-icon errorIcon"></span>
                                    <div class="bw-blackbg-tooltip errorText"></div>
                                </div>
                                <div class="clear"></div>
                                <div class="margin-top20">
                                    <div class="select-model-box form-control-box leftfloat margin-right40">
                                        <select id="assistGetModel" data-placeholder="Choose a bike model" data-bind=" value: selectedBike, options: bikes, optionsText: 'bike',optionsCaption: 'Select a bike'" class="form-control chosen-select"></select>
                                        <span class="bwsprite error-icon errorIcon"></span>
                                        <div class="bw-blackbg-tooltip errorText"></div>
                                    </div>
                                    <input type="button" class="btn btn-orange btn-md" id="submitAssistanceFormBtn" value="Submit" data-bind="event: { click: submitLead }" />
                                </div>
                                <div class="clear"></div>
                            </div>
                            <div id="dealer-assist-msg" class="hide">
                                <p class="leftfloat font14">Thank you for your interest. <span data-bind="text: dealerName()"></span>will get in touch shortly</p>
                                <span class="rightfloat bwsprite cross-lg-lgt-grey cur-pointer"></span>
                                <div class="clear"></div>
                            </div>
                        </div>
                        <div class="border-solid-top" data-bind="visible : DealerBikes.length > 0">
                            <p class="font14 text-bold padding-top20 padding-right20 padding-left20 margin-bottom15"><span data-bind="text : (DealerBikes.length > 1 )?'Models':'Model'"></span>available with the dealer:</p>
                            <ul id="modelsAvailable" data-bind="template: { name: 'dealerBikesTemplate', foreach: DealerBikes }"></ul>

                            <script id="dealerBikesTemplate" type="text/html">
                                <li>
                                    <div class="contentWrapper">
                                        <div class="imageWrapper">
                                            <a href="#" data-bind="attr: { href: bikeUrl(), title: bikeName() }">
                                                <img data-bind="attr: { src : imagePath(), title: bikeName(), alt: bikeName() }" />
                                            </a>
                                        </div>
                                        <div class="bikeDescWrapper">
                                            <div class="bikeTitle margin-bottom7">
                                                <h3 class="font16 text-dark-black"><a href="#" title="" data-bind="text: bikeName(),attr: { href: bikeUrl(), title: bikeName() }"></a></h3>
                                            </div>
                                            <div class="font16 text-bold margin-bottom5">
                                                <span class="fa fa-rupee"></span>
                                                <span class="font18" data-bind="CurrencyText: bikePrice() "></span><span class="font16">&nbsp;onwards</span>
                                            </div>
                                            <div class="font14 text-light-grey">
                                                <span data-bind="html: displayMinSpec() "></span>
                                            </div>
                                        </div>
                                    </div>
                                </li>
                            </script>
                            <div class="clear"></div>
                        </div>
                    </div>
                    <!-- inquiry capture popup start-->
                    <div id="leadCapturePopup" data-bind="with: CustomerDetails" class="text-center rounded-corner2">
                        <div class="leadCapture-close-btn position-abt pos-top10 pos-right10 bwsprite cross-lg-lgt-grey cur-pointer"></div>
                        <!-- contact details starts here -->
                        <div id="contactDetailsPopup">
                            <div class="icon-outer-container rounded-corner50">
                                <div class="icon-inner-container rounded-corner50">
                                    <span class="bwsprite user-contact-details-icon margin-top25"></span>
                                </div>
                            </div>
                            <p class="font20 margin-top20 margin-bottom10">Provide contact details</p>
                            <p class="text-light-grey margin-bottom20">For you to see more details about this bike, please submit your valid contact details. It will be safe with us.</p>
                            <div class="personal-info-form-container">
                                <div class="form-control-box personal-info-list">
                                    <select id="getModelName" data-placeholder="Choose a bike model" data-bind=" value: selectedBike, options: bikes, optionsText: 'bike',optionsCaption: 'Select a bike'" class="form-control chosen-select"></select>
                                    <span class="bwsprite error-icon errorIcon"></span>
                                    <div class="bw-blackbg-tooltip errorText"></div>
                                </div>
                                <div class="form-control-box personal-info-list">
                                    <input type="text" class="form-control get-first-name" placeholder="Full name (mandatory)"
                                        id="getFullName" data-bind="textInput: fullName">
                                    <span class="bwsprite error-icon errorIcon"></span>
                                    <div class="bw-blackbg-tooltip errorText"></div>
                                </div>
                                <div class="form-control-box personal-info-list">
                                    <input type="text" class="form-control get-email-id" placeholder="Email address (mandatory)"
                                        id="getEmailID" data-bind="textInput: emailId">
                                    <span class="bwsprite error-icon errorIcon"></span>
                                    <div class="bw-blackbg-tooltip errorText"></div>
                                </div>
                                <div class="form-control-box personal-info-list">
                                    <p class="mobile-prefix">+91</p>
                                    <input type="text" class="form-control padding-left40 get-mobile-no" placeholder="Mobile no. (mandatory)"
                                        id="getMobile" maxlength="10" data-bind="textInput: mobileNo">
                                    <span class="bwsprite error-icon errorIcon"></span>
                                    <div class="bw-blackbg-tooltip errorText"></div>
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
                            <p class="font18 margin-top25 margin-bottom20">Thank you for providing your details. <span data-bind="dealerName"></span>, <span data-bind="    dealerArea"></span>will get in touch with you soon.</p>

                            <a href="javascript:void(0)" class="btn btn-orange okay-thanks-msg">Okay</a>
                        </div>
                    </div>
                    <!-- inquiry capture popup End-->
                </div>
            </div>
            <div class="clear"></div>
        </section>
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

        <script type="text/javascript">
            var $ddlCities = $("#ddlCities"), $ddlMakes = $("#ddlMakes"), $ddlModels = $("#getModelName,#assistGetModel");
            var bikeCityId = $("#ddlCities").val();
            var clientIP = "<%= clientIP %>";
            var pageUrl = "<%= pageUrl%>";
            var leadSrcId = eval("<%= (int)(Bikewale.Entities.BikeBooking.LeadSourceEnum.DealerLocator_Desktop) %>");
            var pageSrcId = eval("<%= Bikewale.Utility.BWConfiguration.Instance.SourceId %>");
            // lscache.flushExpired();
            $("#applyFiltersBtn").click(function () {

                ddlmakemasking = $("#ddlMakes option:selected").attr("maskingName");
                ddlcityId = $("#ddlCities option:selected").val();
                if(ddlcityId != "0")
                {
                    ddlcityMasking = $("#ddlCities option:selected").attr("maskingName");   
                    window.location.href = "/new/" + ddlmakemasking + "-dealers/" + ddlcityId + "-" + ddlcityMasking + ".html";
                }
                else{
                    toggleErrorMsg($ddlCities, true ,"Choose a city" );
                }

                
            });

            $ddlCities.chosen({ no_results_text: "No matches found!!" });
            $ddlMakes.chosen({ no_results_text: "No matches found!!" });  
            //$ddlModels.chosen({ no_results_text: "No matches found!!", width: "100%" });
            $('div.chosen-container').attr('style', 'width:100%;border:0');
            $("#bookingAreasList_chosen .chosen-single.chosen-default span").text("Please Select City");

            var key = "dealerCities_";
            lscache.setBucket('DLPage');
            
            $ddlMakes.change(function () {
                selMakeId = $ddlMakes.val();
                $ddlCities.empty();
                if (!isNaN(selMakeId) && selMakeId != "0") {
                    if (!checkCacheCityAreas(selMakeId)) {
                        $.ajax({
                            type: "GET",
                            url: "/api/v2/DealerCity/?makeId=" + selMakeId,
                            contentType: "application/json",
                            success: function (data) {
                                lscache.set(key + selMakeId, data.City, 30);
                                setOptions(data.City);
                            },
                            complete: function (xhr) {
                                if (xhr.status == 404 || xhr.status == 204) {
                                    lscache.set(key + selMakeId, null, 30);
                                    setOptions(null);
                                }
                            }
                        });
                    }
                    else {
                        data = lscache.get(key + selMakeId.toString());
                        setOptions(data);
                    }
                }
                else {                    
                    setOptions(null);
                }
            });

            $ddlCities.change(function(){
                toggleErrorMsg($ddlCities, false );
            }) ;

            function checkCacheCityAreas(cityId) {
                bKey = key + cityId;
                if (lscache.get(bKey)) return true;
                else return false;
            }

            function setOptions(optList) {
                toggleErrorMsg($ddlCities, false );
                if (optList != null)
                {
                    $ddlCities.append($('<option>').text(" Select City ").attr({ 'value': "0" }));
                    $.each(optList, function (i, value) {
                        $ddlCities.append($('<option>').text(value.cityName).attr({'value' : value.cityId , 'maskingName' : value.cityMaskingName }));
                    });
                }
                                
                $ddlCities.trigger('chosen:updated');
                $("#ddlCities_chosen .chosen-single.chosen-default span").text("No Areas available");
            }

        </script>

        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/dealerlisting.js?<%= staticFileVersion %>"></script>
    </form>
</body>
</html>
