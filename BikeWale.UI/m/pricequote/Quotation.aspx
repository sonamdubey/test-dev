<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.PriceQuote.Quotation" Trace="false" %>
<%@ Register Src="~/m/controls/AlternativeBikes.ascx" TagPrefix="BW" TagName="AlternateBikes" %>
<%@ Register Src="~/m/controls/MUpcomingBikes.ascx" TagName="MUpcomingBikes" TagPrefix="BW" %>
<%@ Import Namespace="Bikewale.Common" %>
<!doctype html>
<html>
<head>
    <%
        title = "Instant Free New Bike Price Quote";
        description = "Bikewale.com New bike free price quote.";
        AdPath = "/1017752/Bikewale_Mobile_PriceQuote";
        AdId = "1398839030772";

    %>
    <script>var quotationPage = true;</script>
    <!-- #include file="/includes/headscript_mobile.aspx" -->
    <link rel="stylesheet" href="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/m/css/bw-new-style.css?<%= staticFileVersion %>" />
   
    <style type="text/css">
        .inner-section {
            background: #fff;
            clear: both;
            overflow: hidden;
        }

        .alternatives-carousel .jcarousel li.front {
            border: none;
        }

        .discover-bike-carousel .jcarousel li {
            height: auto;
        }

        .discover-bike-carousel .front {
            height: auto;
        }
        .city-unveil-offer-container { margin-top:20px; border:1px dashed #82888b; width:100%; min-height:115px; padding:10px; }
        .city-unveil-offer-container ul { margin-left:20px; list-style-type:disc; }
        .city-unveil-offer-container ul li { margin-top:5px; padding-bottom:5px; font-size: 14px;}
        .disclaimer-icon { background-position: -135px -308px; }
        .disclaimer-icon { height: 16px;width: 14px; }
        .float-button { background-color:#f5f5f5; padding: 10px; }
        .float-button.float-fixed{position:fixed; bottom:0; z-index:8; left:0; right:0;}
        #leadCapturePopup .error-icon, #leadCapturePopup .bw-blackbg-tooltip {display:none} 
        .mobile-prefix { position: absolute; padding: 10px 13px 13px; color: #999; z-index:2; }
        #getMobile { padding:9px 40px; }
    </style>
    
    <script type="text/javascript">

        var pqId = '<%= objQuotation.PriceQuoteId%>';
        var versionId = '<%= objQuotation.VersionId%>';
        var cityId = '<%= cityId %>';
        var bikeVersionLocation = '';
        var campaignId = "<%= objQuotation.CampaignId%>";
        var manufacturerId = "<%= objQuotation.ManufacturerId%>";
        var versionName = "<%= objQuotation.VersionName%>";
        var myBikeName = "<%=objVersionDetails.MakeBase.MakeName%>";

</script>

</head>
<body class="bg-light-grey">
    <form runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->
        <div class="box1 box-top bot-red bg-white">


            <div class="bike-img new-line10">
                <img src="<%= Bikewale.Utility.Image.GetPathToShowImages(objVersionDetails.OriginalImagePath,objVersionDetails.HostUrl,Bikewale.Utility.ImageSize._640x348) %>" alt="<%= objQuotation.MakeName + " " + objQuotation.ModelName + " " + objQuotation.VersionName%> photos" title="<%= objQuotation.MakeName + " " + objQuotation.ModelName + " " + objQuotation.VersionName%> photos" border="0" />
            </div>
            <h1 class="margin-top20 font18 padding-left10 padding-right10" style="margin-left: 0px;"><%= objQuotation.MakeName + " " + objQuotation.ModelName + " " + objQuotation.VersionName%></h1>

            <div class="<%= versionList.Count>1 ?"":"hide" %> margin-top20">
                <asp:DropDownList ID="ddlVersion" CssClass="form-control" runat="server" AutoPostBack="true"></asp:DropDownList>
            </div>

            <div class="new-line15 padding-left10 padding-right10" style="margin-top: 20px;">
                <%if (objQuotation != null && objQuotation.ExShowroomPrice > 0)
                  { %>
                <h2 class="font16" style="font-weight: normal">On-road price in 
                    <%= (String.IsNullOrEmpty(objQuotation.Area))?objQuotation.City:(objQuotation.Area + ", " + objQuotation.City) %>
                </h2>
                <% } %>

                <% if (objQuotation != null && objQuotation.ExShowroomPrice > 0)
                   {%>
                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="pqTable font14">
                    <tr>
                        <td class="text-medium-grey" align="left">Ex-Showroom Price</td>
                        <td class="text-grey text-bold" align="right"><span class="fa fa-rupee"></span><%= CommonOpn.FormatPrice(objQuotation.ExShowroomPrice.ToString()) %></td>
                    </tr>
                    <tr>
                        <td class="text-medium-grey" align="left">RTO</td>
                        <td class="text-grey text-bold" align="right"><span class="fa fa-rupee"></span><%= CommonOpn.FormatPrice(objQuotation.RTO.ToString()) %></td>
                    </tr>
                    <tr>
                        <td class="text-medium-grey" align="left">Insurance (<a target="_blank" onclick="dataLayer.push({ event: 'Bikewale_all', cat: 'BW_PQ', act: 'Insurance_Clicked',lab: '<%= (objQuotation!=null)?(objQuotation.MakeName + "_" + objQuotation.ModelName + "_" + objQuotation.VersionName + "_" + objQuotation.City):string.Empty %>' });" href="/m/insurance/" style="display: inline-block; position: relative; font-size: 11px; margin-top: 1px;">
                                Up to 60% off - PolicyBoss                                
                        </a>)<span style="margin-left: 5px; vertical-align: super; font-size: 9px;">Ad</span>
                        </td>
                        <td class="text-grey text-bold" align="right"><span class="fa fa-rupee"></span><%=CommonOpn.FormatPrice(objQuotation.Insurance.ToString()) %></td>
                    </tr>
                    <tr align="left">
                        <td height="10" colspan="2" style="padding: 0;"></td>
                    </tr>
                    <tr align="left">
                        <td height="1" colspan="2" class="break-line" style="padding: 0 0 10px;"></td>
                    </tr>
                    <tr>
                        <td class="text-grey font16" align="left">Total On Road Price</td>
                        <td class="text-grey text-bold font18" align="right" class="f-bold"><span class="fa fa-rupee"></span><%=CommonOpn.FormatPrice(objQuotation.OnRoadPrice.ToString()) %></td>
                    </tr>
                </table>
                <%}
                   else
                   {%>
                <div class="margin-top-10 padding5" style="background: #fef5e6;">Price for this bike is not available in this city.</div>
                <%} %>
            </div>

            <div class="city-unveil-offer-container <%= (objQuotation.CampaignId > 0) ? "" : "hide" %>">
                <h4 class="border-solid-bottom padding-bottom5 margin-bottom10"><span class="bwmsprite disclaimer-icon margin-right5"></span>                   
                        Get following details from <%=objVersionDetails.MakeBase.MakeName %>:                   
                </h4>
                <ul class="bike-details-list-ul">
                    <li>
                        <span class="show">Offers from the nearest dealers</span>
                    </li>
                    <li>

                        <span class="show">Waiting period on this bike at the dealership</span>
                    </li>
                    <li>

                        <span class="show">Nearest dealership from your place</span>
                    </li>
                    <li>
                        <span class="show">Finance options on this bike</span>
                    </li>
                </ul>
            </div>
            <div class="grid-12 float-button float-fixed clearfix <%= (objQuotation.CampaignId > 0) ? "" : "hide" %>">
                <input type="button" value="Get more details" class="btn btn-full-width btn-sm margin-right10 leftfloat btn-orange" id="getMoreDetailsBtnCampaign" />
            </div>

        </div>        
        <section class="<%= (ctrlAlternateBikes.FetchedRecordsCount > 0) ? "" : "hide" %>">
            <div class="container margin-bottom30">
                <div class="grid-12">
                    <h2 class="margin-top30px margin-bottom20 text-center padding-top20"><%= objVersionDetails.MakeBase.MakeName + " " + objVersionDetails.ModelBase.ModelName  %> alternatives</h2>
                    <div class="swiper-container discover-bike-carousel alternatives-carousel padding-bottom60">
                        <div class="swiper-wrapper">
                            <BW:AlternateBikes ID="ctrlAlternateBikes" runat="server" />
                        </div>
                        <div class="swiper-pagination"></div>
                        <div class="bwmsprite swiper-button-next hide"></div>
                        <div class="bwmsprite swiper-button-prev hide"></div>
                    </div>

                </div>
                <div class="clear"></div>
            </div>
        </section>

         <section class="<%= (ctrlUpcomingBikes.FetchedRecordsCount > 0) ? "" : "hide" %>" ><!--  Upcoming, New Launches and Top Selling code starts here -->        
            <div class="container">
                <div class="grid-12 margin-bottom30">
                    <h2 class="text-center margin-top30 margin-bottom20">Upcoming <%= objVersionDetails.MakeBase.MakeName %> bikes</h2>
                    <div class="swiper-container upComingBikes padding-bottom60">
                        <div class="swiper-wrapper">
                            <BW:MUpcomingBikes runat="server" ID="ctrlUpcomingBikes" />
                        </div>
                        <div class="text-center swiper-pagination"></div>
                        <div class="bwmsprite swiper-button-next hide"></div>
                        <div class="bwmsprite swiper-button-prev hide"></div>
                    </div>

                </div>
                <div class="clear"></div>
            </div>
        </section>

        <!-- Lead Capture pop up start  -->
        <div id="leadCapturePopup" class="bw-popup bwm-fullscreen-popup contact-details hide">
            <div class="popup-inner-container text-center">
                <div class="bwmsprite close-btn leadCapture-close-btn rightfloat"></div>
                <div id="contactDetailsPopup">
                    <!-- Contact details Popup starts here -->
                    <h2 class="margin-bottom5">Get more details on this bike</h2>
                    <p class="text-light-grey margin-bottom5">Please provide contact info to see more details</p>

                    <div class="personal-info-form-container margin-top10">
                        <div class="form-control-box">
                            <input type="text" class="form-control get-first-name" placeholder="Your name" id="getFullName" data-bind="value: fullName" />
                            <span class="bwmsprite error-icon "></span>
                            <div class="bw-blackbg-tooltip errorText">Please enter your name</div>
                        </div>
                        <div class="form-control-box margin-top20">
                            <input type="text" class="form-control get-email-id" placeholder="Email address" id="getEmailID" data-bind="value: emailId" />
                            <span class="bwmsprite error-icon"></span>
                            <div class="bw-blackbg-tooltip errorText">Please enter your email adress</div>
                        </div>

                        <div class="form-control-box margin-top20">
                            <p class="mobile-prefix">+91</p>
                            <input type="text" class="form-control get-mobile-no" maxlength="10" placeholder="Mobile no." id="getMobile" data-bind="value: mobileNo" />
                            <span class="bwmsprite error-icon"></span>
                            <div class="bw-blackbg-tooltip errorText">Please enter mobile number</div>
                        </div>
                        <div class="clear"></div>
                        <a class="btn btn-full-width btn-orange margin-top20" id="user-details-submit-btn" data-bind="event: { click: submitCampaignLead }">Submit</a>
                    </div>
                    <input type="button" class="btn btn-full-width btn-orange hide" value="Submit" onclick="validateDetails();" class="rounded-corner5" data-role="none" id="btnSubmit" />
                </div>
                <!-- Contact details Popup ends here -->
                 <!-- thank you message starts here -->
                <div id="notify-response" class="hide margin-top10 content-inner-block-20 text-center">
                        <p class="font18 text-bold margin-bottom20">Thank you <span class="notify-leadUser"></span></p>
                        <p class="font16 margin-bottom40"><%=objVersionDetails.MakeBase.MakeName%> Company would get back to you shortly with additional information.</p>
                        <input type="button" id="notifyOkayBtn" class="btn btn-orange" value="Okay" />
                </div>
				<!-- thank you message ends here -->               
            </div>
				<!-- thank you message ends here -->
                      
  
        </div>

        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <!-- all other js plugins -->
        <!-- #include file="/includes/footerscript_Mobile.aspx" -->
       
        <script type="text/javascript">
            ga_pg_id = "6";

            var bodHt, footerHt, scrollPosition;
            $(window).scroll(function () {
                bodHt = $('body').height();
                footerHt = $('footer').height();
                scrollPosition = $(this).scrollTop();
                if (scrollPosition + $(window).height() > (bodHt - footerHt))
                    $('.float-button').removeClass('float-fixed');
                else
                    $('.float-button').addClass('float-fixed');
            });

            var fullname = $("#getFullName");
            var emailid = $("#getEmailID");
            var mobile = $("#getMobile");
            var prevEmail = "";
            var prevMobile = "";
            var getCityArea = GetGlobalCityArea();

            $("#getMoreDetailsBtnCampaign").on("click", function () {
                $("#leadCapturePopup").show();
                $('body').addClass('lock-browser-scroll');                
                dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': 'Get_More_Details_Clicked', 'lab': bikeVersionLocation });
            });

            $(".leadCapture-close-btn").on("click mouseup", function () {
                $("#leadCapturePopup").hide();
                $('body').removeClass('lock-browser-scroll');               
            });

            $(".leadCapture-close-btn, #notifyOkayBtn").on("click", function () {
                $("#leadCapturePopup").hide();
                $('body').removeClass('lock-browser-scroll');               
                $("#contactDetailsPopup").show();
                $("#notify-response").hide();
            });

            if (bikeVersionLocation == '') {
                bikeVersionLocation = getBikeVersionLocation();
            }

            function getBikeVersionLocation() {
                var loctn = GetGlobalCityArea();
                if (loctn != null) {
                    if (loctn != '')
                        loctn = '_' + loctn;
                }
                else {
                    loctn = '';
                }
                var bikeVersionLocation = myBikeName + '_' + versionName + loctn;
                return bikeVersionLocation;
            }

            function setuserDetails() {
                var cookieName = "_PQUser";
                if (isCookieExists(cookieName)) {
                    var arr = getCookie(cookieName).split("&");
                    return arr;
                }
            }

            function setPQUserCookie() {
                var val = fullname.val() + '&' + emailid.val() + '&' + mobile.val();
                SetCookie("_PQUser", val);
            }

            function hideError(ele) {
                ele.removeClass("border-red");
                ele.siblings("span, div").hide();
            }


            function nameValTrue() {
                hideError(fullname)
                fullname.siblings("div").text('');
            };

            function ValidateUserDetail() {
                var isValid = true;
                isValid = validateEmail();
                isValid &= validateMobile();
                isValid &= validateName();
                return isValid;
            };


            /* Email validation */
            function validateEmail() {
                var isValid = true;
                var emailID = emailid.val();
                var reEmail = /^[A-z0-9._+-]+@[A-z0-9.-]+\.[A-z]{2,6}$/;

                if (emailID == "") {
                    setError(emailid, 'Please enter email address');
                    isValid = false;
                }
                else if (!reEmail.test(emailID)) {
                    setError(emailid, 'Invalid Email');
                    isValid = false;
                }
                return isValid;
            }

            function validateMobile() {
                var isValid = true;
                var reMobile = /^[0-9]{10}$/;
                var mobileNo = mobile.val();
                if (mobileNo == "") {
                    isValid = false;
                    setError(mobile, "Please enter your Mobile Number");
                }
                else if (!reMobile.test(mobileNo) && isValid) {
                    isValid = false;
                    setError(mobile, "Mobile Number should be 10 digits");
                }
                else {
                    hideError(mobile)
                }
                return isValid;
            }

            function validateName() {
                var isValid = true;
                var a = fullname.val().length;
                if ((/&/).test(fullname.val())) {
                    isValid = false;
                    setError(fullname, 'Invalid name');
                }
                else
                    if (a == 0) {
                        isValid = false;
                        setError(fullname, 'Please enter your first name');
                    }
                    else if (a >= 1) {
                        isValid = true;
                        nameValTrue()
                    }
                return isValid;
            }

            function setError(ele, msg) {
                ele.addClass("border-red");
                ele.siblings("span, div").show();
                ele.siblings("div").text(msg);
            }


            fullname.on("focus", function () {
                hideError(fullname);
            });

            emailid.on("focus", function () {
                hideError(emailid);
                prevEmail = emailid.val().trim();
            });

            mobile.on("focus", function () {
                hideError(mobile)
                prevMobile = mobile.val().trim();

            });

            emailid.on("blur", function () {
                if (prevEmail != emailid.val().trim()) {
                    if (validateEmail()) {                                                                       
                        hideError(emailid);
                    }
                }
            });

            mobile.on("blur", function () {
                if (mobile.val().length < 10) {                    
                    $(".mobile-verification-container").removeClass("show").addClass("hide");
                }
                if (prevMobile != mobile.val().trim()) {
                    if (validateMobile(getCityArea)) {                    
                        hideError(mobile);
                    }
                }
            });


            var customerViewModel = new CustomerModel();


            ko.applyBindings(customerViewModel, $('#leadCapturePopup')[0]);

            function CustomerModel() {
                var arr = setuserDetails();
                var self = this;
                if (arr != null && arr.length > 0) {
                    self.fullName = ko.observable(arr[0]);
                    self.emailId = ko.observable(arr[1]);
                    self.mobileNo = ko.observable(arr[2]);
                }
                else {
                    self.fullName = ko.observable();
                    self.emailId = ko.observable();
                    self.mobileNo = ko.observable();
                }


                self.submitCampaignLead = function () {

                    var isValidCustomer = ValidateUserDetail();

                    if (isValidCustomer && campaignId > 0) {

                        $('#processing').show();
                        var objCust = {
                            "dealerId": manufacturerId,
                            "pqId": pqId,
                            "name": self.fullName(),
                            "mobile": self.mobileNo(),
                            "email": self.emailId(),
                            "versionId": versionId,
                            "cityId": cityId,
                            "leadSourceId": 3,
                            "deviceId": getCookie('BWC')
                        }
                        $.ajax({
                            type: "POST",
                            url: "/api/ManufacturerLead/",
                            data: ko.toJSON(objCust),
                            beforeSend: function (xhr) {
                                xhr.setRequestHeader('utma', getCookie('__utma'));
                                xhr.setRequestHeader('utmz', getCookie('__utmz'));
                            },
                            async: false,
                            contentType: "application/json",
                            success: function (response) {
                                //var obj = ko.toJS(response);
                                $("#personalInfo,#otpPopup").hide();
                                $('#processing').hide();

                                //validationSuccess($(".get-lead-mobile"));
                                $("#contactDetailsPopup").hide();
                                $('#notify-response .notify-leadUser').text(self.fullName());
                                $('#notify-response').show();
                            },
                            error: function (xhr, ajaxOptions, thrownError) {
                                $('#processing').hide();
                                $("#contactDetailsPopup, #otpPopup").hide();
                                var leadMobileVal = mobile.val();
                                nameValTrue();
                                hideError(self.mobileNo());
                            }
                        });

                        setPQUserCookie();
                        dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Model_Page', 'act': 'Lead_Submitted', 'lab': bikeVersionLocation });
                    }
                };
            }

        </script>
    </form>
</body>
</html>
