<%@ Page Language="C#" AutoEventWireup="false" Inherits="Carwale.UI.NewCars.Dealers.newCarDealerShowroom" Trace="false" EnableEventValidation="false" Debug="true" %>

<%@ Import Namespace="Carwale.UI.Common" %>
<%@ Register TagPrefix="slider" TagName="Corousel" Src="/Controls/Carousel_Home_940x320.ascx" %>
<!doctype html>
<html itemscope itemtype="http://schema.org/WebPage">
<head>

    <script lang="c#" runat="server">
        private string fbTitle = "", fbImage = "";
    </script>

    <%
        // Define all the necessary meta-tags info here.
        // To know what are the available parameters,
        // check page, headerCommon.aspx in common folder.


        PageId = 81;
        Title = pageTitle;
        Keywords = keywords;
        Description = description;
        Revisit = "5";
        AdId = "1397028284211";
        AdPath = "/1017752/Carwale_Dealers_";
        DocumentState = "Static";
        fbTitle = DealerName;
        fbImage = "https://img.carwale.com/pq/cwlogo.gif";
        canonical = "https://www.carwale.com/new/" + canon;
        altUrl = "https://www.carwale.com/m/new/" + alternateUrl;

    %>

    <!-- #include file="/includes/global/head-script.aspx" -->
    <link rel="stylesheet" href="/static/css/colorbox.css" type="text/css" >
    <link rel="stylesheet" href="/static/css/dealershowroomdesktop.css" type="text/css" >
    <link rel="stylesheet" href="/static/css/datepicker.css" type="text/css" >
    <script   src="/static/src/jquery.jcarousel.min.js"  type="text/javascript"></script>
    <script   src="/static/src/jquery.colorbox.js" ></script>
    <script   src="/static/src/datepicker.js" ></script>
    <script   src="/static/src/dealershowroomdesktop.js" ></script>
    <script  lang="javascript"  src="/static/src/ajaxfunctions.js" ></script>

    <style>
        html, body, #map-canvas { height: 100%; margin: 0px; padding: 0px; }

        .dealer-left-lg { width: 600px; }

        .contact-details li input, .mob-no-box input { width: 100% !important; }

        .cw-sprite { background: url(https://imgd.aeplcdn.com/0x0/cw/static/sprites/cw-sprite_v1.3.png?28052015) no-repeat; display: inline-block; }

        .arrow-position-prev, .arrow-position-next { z-index: 10; }

        .temp-height { height: 250px; }

        .jcarousel-clip-horizontal, .jcarousel-clip-vertical { width: 500px; }

        #authorCarousel { width: 10000em !important; }

        #banner { width: 934px; }
    </style>

    <script type='text/javascript'>

        googletag.cmd.push(function () {
            googletag.defineSlot('/1017752/Carwale_Dealers_300x250', [[300, 250], [300, 600]], 'div-gpt-ad-1399360865223-2').addService(googletag.pubads());
            googletag.defineSlot('/1017752/Carwale_Dealers_970x90', [[220, 90], [728, 90], [950, 90], [960, 90], [970, 66], [970, 90], [960, 60], [970, 60]], 'div-gpt-ad-1397028284211-2').addService(googletag.pubads());
            googletag.pubads().setTargeting("make", "<%=MakeName%>");
            googletag.pubads().setTargeting("City", "Mumbai");
            googletag.pubads().setTargeting('UserModelHistory', '<%= CookiesCustomers.UserModelHistory.Replace('~', ',')%>');
            //googletag.pubads().enableSyncRendering();
            googletag.pubads().collapseEmptyDivs();
            googletag.pubads().enableSingleRequest();
            googletag.enableServices();
        });
    </script>

</head>
<body class="bg-white header-fixed-inner special-page special-skin-body no-bg-color">
    <!-- #include file="/includes/header.aspx" -->
    <form runat="server">
        <section class="container">
            <div class="grid-12">
                <div class="padding-bottom15 text-center">
                    <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 0, 90, 0, 0, true, 2) %>
                </div>
            </div>
            <div class="clear"></div>
        </section>
        <section class="bg-light-grey padding-top10 padding-bottom20 no-bg-color">
            <div class="container">
                <div class="column grid-12" id="pagetop">
                    <%--<span class="redirect-lt" style="margin-top: 8px;">You are here: </span>--%>
                    <ul class="breadcrumb margin-top10 margin-bottom15 redirect-lt" itemprop="breadcrumb">
                        <li>You are here: </li>
                        <li><a href="/">Home</a></li>
                        <li>&nbsp;&rsaquo;&nbsp;<a href="/new/">New Cars</a></li>
                        <li>&nbsp;&rsaquo;&nbsp;<a href="/new/locatenewcardealers.aspx">New Car Dealers</a></li>
                        <li>&nbsp;&rsaquo;&nbsp;<a href="/new/<%= Carwale.UI.Common.UrlRewrite.FormatSpecial(urlMakeName) %>-dealers/"><%=urlMakeName%> dealers</a></li>
                        <li>&nbsp;&rsaquo;&nbsp;<a href="/<%= Carwale.UI.Common.UrlRewrite.FormatSpecial(urlMakeName) %>-dealer-showrooms/<%= Carwale.UI.Common.UrlRewrite.FormatSpecial(DealerCityName) %>-<%=DealerCityId %>"><%=urlMakeName%> Dealers in <%=DealerCityName%></a></li>
                        <li>&nbsp;&rsaquo;&nbsp;<strong><%=DealerName%></strong></li>
                    </ul>
                    <h1 class="font30 text-black leftfloat header-dealer-name special-skin-text"><%= DealerName %></h1>
                    <%= !String.IsNullOrWhiteSpace(DealerMobileNo) ? "<div class='font20 leftfloat margin-top5 margin-left15' style='margin-top:12px;'><span class='leftfloat dealer-sprite dealer-call-icon-red margin-right5'></span><strong class='leftfloat header-mob-no' style='margin-top:0;'><span>"+ DealerMobileNo + "</span></strong><div class='clear'></div></div>" : "" %>
                    <div class="clear"></div>
                    <div class="border-solid-bottom margin-top10 margin-bottom15"></div>
                </div>

                <div class="content-inner-block white-shadow grid-12">
                    <!-- main slider starts here -->
                    <div>
                        <%if (!string.IsNullOrEmpty(BannerImageUrl))
                          { %>
                        <div id="banner">
                            <img id="imgBanner" alt="" src="<%=BannerImageUrl %>">
                        </div>
                        <%} %>
                    </div>
                    <!-- main slider ends here -->
                    <!-- tabs starts here -->
                    <div class="margin-top10 tabs-box">
                        <div class="panel-group">
                            <div class="dealer-tabs ">
                                <ul>
                                    <li data-id="buyingassist" class="active">Buying Assistance</li>
                                    <li data-id="contactdetails">Contact Details</li>
                                    <li data-id="photogallery">Photo Gallery</li>
                                </ul>
                                <div class="clear"></div>
                            </div>
                            <!-- contact details div starts here -->
                            <div class="dealer-data hide" id="contactdetails">
                                <div class="grid-6">
                                    <div class="content-inner-block-10">
                                        <ul class="dealer-address">
                                            <li>
                                                <span class="dealer-sprite dealer-search dealer-contacts-icon"></span>
                                                <p><%=DealerAddress %>, <%=DealerCityName%><%=string.IsNullOrWhiteSpace(StateName+" "+Pincode)?"":", " + StateName + " " + Pincode %></p>
                                                <div class="clear"></div>
                                            </li>
                                            <%= !String.IsNullOrWhiteSpace(DealerMobileNo) ? "<li><span class='dealer-sprite dealer-call-icon-grey dealer-contacts-icon'></span><p>" + DealerMobileNo + "</p><div class='clear'></div></li>" : "" %>
                                            <li>
                                                <span class="dealer-sprite dealer-email-icon-sm dealer-contacts-icon"></span>
                                                <p><%=EmailId %></p>
                                                <div class="clear"></div>
                                            </li>
                                            <li>
                                                <span class="dealer-sprite dealer-clock-pic dealer-contacts-icon"></span>
                                                <%if (!string.IsNullOrWhiteSpace(StartTime))
                                                  { %><p><%= StartTime%> to <%=EndTime %></p>
                                                <%} %>
                                                <div class="clear"></div>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                                <div class="grid-6 border-solid-left map-hide">
                                    <div class="content-inner-block-10">
                                        <h2 class="margin-bottom10">How to get there?</h2>
                                        <%-- <div id="map" style="height: 300px;"></div>--%>
                                        <div id="map-canvas" style="height: 300px;"></div>
                                    </div>
                                </div>
                                <div class="clear"></div>
                            </div>
                            <!-- contact details div ends here -->
                            <!-- buying assistance div starts here -->
                            <div class="dealer-data" id="buyingassist">
                                <div class="dealer-left-lg border-solid-right">
                                    <div class="content-inner-block-10">
                                        <h2 class="font16">1. Select Car</h2>
                                        <!-- jcarousel starts here -->
                                        <div class="content-inner-block-10">
                                            <div class="main-carousel-div">
                                                <%if (dealer.objModelDetails.Count > 3)
                                                  {%><div class="arrow-position-prev prev_next">
                                                        <a id="list_carousel_widget_prev" class="cw-sprite prev"></a>
                                                    </div>
                                                <div class="arrow-position-next prev_next">
                                                    <a id="list_carousel_widget_next" class="cw-sprite next"></a>
                                                </div>
                                                <%} %>
                                                <div class="list_carousel_widget">
                                                    <div class="jcarousel-container jcarousel-container-horizontal" style="position: relative; display: block;">
                                                        <div id="authorCarouselContainer" class="jcarousel-clip jcarousel-clip-horizontal" style="position: relative;">
                                                            <ul id="authorCarousel" class="jcarousel-list jcarousel-list-horizontal" style="overflow: hidden; position: relative; top: 0px; margin: 0px; padding: 0px; left: 0px; width: 1796px;">
                                                                <asp:Repeater ID="rptImages" runat="server">
                                                                    <ItemTemplate>
                                                                        <li class="jcarousel-item jcarousel-item-horizontal jcarousel-item-1 jcarousel-item-1-horizontal" style="float: left; list-style: outside none none;">
                                                                            <div id="carousel" class="dealer-carousel-pic">
                                                                                <a href="<%# Carwale.Utility.ImageSizes.CreateImageUrl(((Carwale.Entity.CarData.CarModelSummary)Container.DataItem).HostUrl.ToString(), Carwale.Utility.ImageSizes._559X314,((Carwale.Entity.CarData.CarModelSummary)Container.DataItem).OriginalImage.ToString()) %>">
                                                                                    <img src="<%#Carwale.Utility.ImageSizes.CreateImageUrl(((Carwale.Entity.CarData.CarModelSummary)Container.DataItem).HostUrl.ToString(), Carwale.Utility.ImageSizes._110X61,((Carwale.Entity.CarData.CarModelSummary)Container.DataItem).OriginalImage.ToString())%>">
                                                                                </a>
                                                                            </div>
                                                                            <span modelid="<%#((Carwale.Entity.CarData.CarModelSummary)Container.DataItem).ModelId %>"><%= urlMakeName%> <%#((Carwale.Entity.CarData.CarModelSummary)Container.DataItem).ModelName %></span>
                                                                        </li>
                                                                    </ItemTemplate>
                                                                </asp:Repeater>
                                                            </ul>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                        <div class="clear"></div>
                                        <!-- jcarousel ends here -->
                                        <div>
                                            <div class="highlighted-image">
                                                <img src="<%= InitialModelImage %>">
                                            </div>
                                            <h3 class="text-center active-img-text"><%=urlMakeName %> <%=InitialModelName%></h3>
                                        </div>
                                    </div>
                                </div>
                                <div class="dealer-right-sm border-none dealerlocator">
                                    <!-- select services stsrts here-->
                                    <div class="content-inner-block-10 selectservices">
                                        <h2 class="margin-bottom20 font16">2. Select Services</h2>
                                        <div class="select-services">
                                            <ul class="checkboxes" id="checkboxes">
                                                <li id="CompleteProductBrochure">Complete Product Brochure </li>
                                                <li id="AvailabilityEnquiry">Availability Enquiry</li>
                                                <li id="Door-stepTestDrive">Door-step Test Drive</li>
                                                <li id="Offer&DiscountInformation">Offer & Discount Information</li>
                                                <li id="OtherAssistance">Other Assistance</li>
                                            </ul>
                                        </div>
                                    </div>
                                    <!-- select services ends here-->
                                    <!-- contact details stsrts here -->
                                    <div class="content-inner-block-10 ">
                                        <div class="contactdetails">
                                            <h2 class="margin-bottom20 font16">3. Provide Contact Details</h2>
                                            <ul class="contact-details">
                                                <li>
                                                    <div class="form-control-box">
                                                        <span id="nameIcon" class="dealer-sprite dealer-user-icon"></span>
                                                        <input id="custName" class="customername form-control" type="text" placeholder="Enter Your Name" maxlength="100">
                                                        <span class="cwsprite error-icon err-name-icon hide" id="errNameIcon"></span>
                                                        <div class="cw-blackbg-tooltip err-name-msg hide">Please enter your name</div>
                                                    </div>
                                                </li>
                                                <li>
                                                    <div class="mob-no-box form-control-box">
                                                        <span id="mobileIcon" class="dealer-sprite dealer-call-icon-small"></span>
                                                        <span class="country-code">+91</span>
                                                        <input class="form-control" type="text" maxlength="20" id="custMobile" placeholder="Enter your mobile number">
                                                        <span class="cwsprite error-icon err-mobile-icon hide"></span>
                                                        <div class="cw-blackbg-tooltip hide err-mobile-msg">Please enter your mobile number</div>
                                                    </div>
                                                </li>
                                                <input type="hidden" id="encryptedResponse" />
                                            </ul>
                                            <div>
                                                <input id="btnSubmitDealer" type="button" class="btn btn-orange" name="price" value="Submit">
                                            </div>
                                        </div>
                                        <div class="thank-you-msg hide">
                                            <h2 class="tyHeading word-break"></h2>
                                            <div class="margin-top20 font14">
                                                <p class="margin-bottom10">You can provide your email id below to receive the price-list,brochure and other collaterals over email.</p>
                                                <div class="position-rel">
                                                    <span id="emailIcon" class="dealer-sprite dealer-email-icon-lg"></span>
                                                    <input class="customeremail form-control" type="text" id="custEmailOptional" placeholder="Enter Your Email" maxlength="50">
                                                    <span class="cwsprite error-icon err-email-icon hide"></span>
                                                    <div class="cw-blackbg-tooltip hide err-email-msg">Please enter your email</div>
                                                </div>
                                                <div class="margin-top10">
                                                    <input type="button" id="btnDone" class="btn btn-orange text-uppercase back-btn" name="price" value="Done">
                                                </div>
                                            </div>
                                            <div class="clear"></div>

                                        </div>
                                    </div>
                                    <!-- contact details ends here -->
                                </div>
                                <div class="clear"></div>
                                <%= !String.IsNullOrWhiteSpace(DealerMobileNo) ? "<div class='call-for-query'><h3>For further Queries call our Customer Consultation Team</h3><div class='dealer-customer-care margin-top5'><span class='dealer-sprite dealer-call-icon-red'></span><span class='font20 margin-left10'><strong>" + DealerMobileNo + "</strong></span><div class='clear'></div></div></div></div>" : "" %>
                            <!-- buying assistance div ends here -->
                            <!-- photo gallery div starts here -->
                            <div class="dealer-data hide grid-12" id="photogallery">
                                <div class="dealer-photo-gallery">
                                    <% if (countGalleryImages > 0)
                                       { %>
                                    <ul>
                                        <asp:Repeater ID="rptGalleryImages" runat="server">
                                            <ItemTemplate>
                                                <li style="width: 221px;">
                                                    <a href="<%# DataBinder.Eval(Container.DataItem, "ImgLargeUrl").ToString()%>" title="" class="cboxElement" name="front1" rel="slide">
                                                        <img border="0" src="<%# Carwale.Utility.ImageSizes.CreateImageUrl(((Carwale.Entity.Dealers.AboutUsImageEntity)Container.DataItem).HostUrl.ToString(), Carwale.Utility.ImageSizes._600X337,((Carwale.Entity.Dealers.AboutUsImageEntity)Container.DataItem).OriginalImgPath.ToString())%>" title="click to view" alt="">
                                                    </a>
                                                </li>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </ul>
                                    <% }
                                       else
                                       { %>
                                    <div class="temp-height">There are currently no images available for this dealership.</div>
                                    <% } %>
                                    <div class="clear"></div>
                                </div>
                            </div>
                        </div>
                        <!-- photo gallery div ends here -->
                    </div>
                </div>
                <!-- tabs ends here -->

                <!-- Image Loading and Black-Out Window code Starts  -->

                <div id="loadingCarImg" class="hide">
                    <div class="loading-popup">
                        <span class="loading-icon"></span>
                        <div class="clear"></div>
                    </div>
                </div>
                <!-- Image Loading and Black-Out Window code Starts -->
            </div>
            <div class="clear"></div>
        </section>
    </form>
    <!-- #include file="/includes/footer.aspx" -->
    <!-- #include file="/includes/global/footer-script.aspx" -->
    <!-- all other js plugins -->
    <script type="text/javascript">
        var comments = "";
        var modelName = '<%=InitialModelName%>';
        var modelId = '<%=InitialModelId%>';
        var latitude = '<%= DealerLatitude%>';
        var longitude = '<%= DealerLongitude%>';
        var dealerName = '<%=DealerName%>';
        var dealerMobileNo = '<%=DealerMobileNo%>';
        var address = "<%= Regex.Replace(DealerAddress, @"\t|\n|\r", "") + ", " + DealerCityName + ", " + StateName + " " + Pincode %>";
        var category = 'dealer_microsite_desktop';
        var isMapInitialized = false;
        var modelsCarousel;
        Common.showCityPopup = false;
        dealerShowroom.CITYID = "<%= DealerCityId%>";
        dealerShowroom.CITYNAME = "<%= CityName%>";
        dealerShowroom.MakeNAME = "<%= urlMakeName %>";
        Location.USERIP = "<%= UserIP%>";

        $(document).ready(function (e) {
            Common.utils.loadGoogleApi(null,null);
            trackingCode();
            modelCarousel();

            // lead submission
            $('#btnSubmitDealer').click(function () {
                processRequest(false);
            });

            $('#btnDone').click(function () {
                var custEmail = $.trim($('#custEmailOptional').val());
                if (custEmail.length > 0) {
                    processRequest(true);
                }
            });

            Location.utils.globalCityByUserAction(dealerShowroom.CITYID, dealerShowroom.CITYNAME, -1, "DealerLocatorCitySet");
        });

        function processRequest(emailSubmitted) {
           var targetFormDiv = ".dealer-right-sm";
           var dealerLeadObject = new Object();
           
           
            dealerLeadObject.Name = $.trim($('#custName').val());
            dealerLeadObject.Email = $.trim($('.customeremail').val());
            dealerLeadObject.Mobile = $.trim($('#custMobile').val());
            dealerLeadObject.MakeId = '<%=MakeIdFromQueryString%>';
            dealerLeadObject.modelId = modelId;
            dealerLeadObject.modelName = modelName;
            dealerLeadObject.VersionId = 0;
            dealerLeadObject.InquirySourceId = 96;
            dealerLeadObject.LeadClickSource = 105;
            dealerLeadObject.DealerId = '<%=CampaignId%>';
            dealerLeadObject.CityId = '<%=DealerCityId%>';
            if (!emailSubmitted) {
                dealerLeadObject.cwtccat = "DealerMicroSitePage";
                dealerLeadObject.cwtcact = "DealerMicroSiteLeadSubmit";
                dealerLeadObject.cwtclbl = 'make:' + dealerShowroom.MakeNAME + '|model:' + dealerLeadObject.modelName + '|city:' + dealerShowroom.CITYNAME;
            }
            //dealerLeadObject.TargetFormDiv = targetFormDiv;

            if (isValid(targetFormDiv)) {
                $(targetFormDiv).find('.tyHeading').empty();
                setCookies(targetFormDiv);
                SendNewCarRequestDealer(dealerLeadObject, targetFormDiv, emailSubmitted);
            }
            
        }
    </script>
</body>
</html>
