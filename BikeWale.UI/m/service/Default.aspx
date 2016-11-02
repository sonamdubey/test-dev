<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Service.LocateServiceCenter" EnableViewState="false" %>
<%@ Register Src="~/m/controls/MUpcomingBikes.ascx" TagName="MUpcomingBikes" TagPrefix="BW" %>
<%@ Register Src="~/m/controls/MNewLaunchedBikes.ascx" TagName="MNewLaunchedBikes" TagPrefix="BW" %>

<!DOCTYPE html>
<html>
<head>
    <%
        title = "New Bike Dealer Showrooms in India | Locate Authorized Bike Showrooms - BikeWale";
        keywords = "new bike dealers, new bike showrooms, bike dealers, bike showrooms, showrooms, dealerships";
        description = "Locate new bike showrooms and authorized bike dealers in India. Find new bike dealer information for more than 200 cities in India. ";
        canonical = "http://www.bikewale.com/dealer-showroom-locator/";
        AdPath = "/1017752/Bikewale_Mobile_NewBikes";
        AdId = "1398766302464";
        Ad_320x50 = false;
        Ad_Bot_320x50 = false;
        //menu = "10";
    %>
    <!-- #include file="/includes/headscript_mobile_min.aspx" -->
    <link href="/m/css/service/landing.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_mobile.aspx" -->
    </script>
</head>
<body class="bg-light-grey">
    <form id="form1" runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->
        <section>
            <div class="container locator-landing-banner text-center">
                <h1 class="font24 text-uppercase text-white">Bike service center locator</h1>
                <h2 class="font14 text-unbold text-white">Find bike service center across 200+ cities in India</h2>
            </div>
        </section>

        <section>
            <div class="container card-bottom-margin">
                <div class="grid-12">
                    <div class="banner-box-shadow padding-right20 padding-left20 padding-bottom20">
                        <h2 class="section-heading">Search service center</h2>
                        <div class="locator-search-container margin-bottom10 text-center">
                            <div class="locator-search-brand form-control-box margin-bottom20">
                                <div class="locator-search-brand-form locator-search-form"><span>Select brand</span></div>
                                <span class="bwmsprite error-icon errorIcon"></span>
                                <div class="bw-blackbg-tooltip errorText"></div>
                            </div>
                            <div class="locator-search-city form-control-box">
                                <div class="locator-search-city-form locator-search-form border-solid-left"><span>Select city</span></div>
                                <span class="bwmsprite error-icon errorIcon"></span>
                                <div class="bw-blackbg-tooltip errorText"></div>
                            </div>
                            <input type="button" class="btn btn-orange btn-lg locator-submit-btn" value="Search" />
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section>
            <h2 class="section-heading">Dealer showroom by brands</h2>
            <div class="container bg-white box-shadow card-bottom-margin padding-top25 padding-bottom20">
                <div class="brand-type-container">
                        <ul class="text-center">
                            <asp:Repeater ID="rptPopularBrands" runat="server">
                                <ItemTemplate>
                                    <li>
                                        <a href="/m/<%# DataBinder.Eval(Container.DataItem, "MaskingName") %>-dealer-showrooms-in-india/">
                                            <span class="brand-type">
                                                <span class="lazy brandlogosprite brand-<%# DataBinder.Eval(Container.DataItem, "MakeId") %>"></span>
                                            </span>
                                            <span class="brand-type-title"><%# DataBinder.Eval(Container.DataItem, "MakeName") %></span>
                                        </a>
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                        <ul class="brand-style-moreBtn brandTypeMore border-top1 padding-top25 text-center hide">
                            <asp:Repeater ID="rptOtherBrands" runat="server">
                                <ItemTemplate>
                                    <li>
                                        <a href="/m/<%# DataBinder.Eval(Container.DataItem, "MaskingName") %>-dealer-showrooms-in-india/">
                                            <span class="brand-type">
                                                <span class="lazy brandlogosprite brand-<%# DataBinder.Eval(Container.DataItem, "MakeId") %>"></span>
                                            </span>
                                            <span class="brand-type-title"><%# DataBinder.Eval(Container.DataItem, "MakeName") %></span>
                                        </a>
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                </div>
                <div class="view-brandType text-center">
                    <a href="javascript:void(0)" id="view-brandType" class="view-more-btn font14">View more brands</a>
                </div>
            </div>
        </section>

        <section>
            <h2 class="section-heading">Bike Care - Maintenance tips</h2>
            <div class="container bg-white box-shadow card-bottom-margin content-inner-block-20">
                <div class="margin-bottom20 font14">
                    <div class="review-image-wrapper">
                        <a href="" title="">
                            <img class="lazy" data-original="https://imgd1.aeplcdn.com//370x208//bw/ec/24006/Side-75983.jpg?wm=0" alt="New colour scheme for Bajaj Pulsar RS200 in Columbia" src="">
                        </a>
                    </div>
                    <div class="review-heading-wrapper">
                        <a href="" title="" class="target-link">New colour scheme for Bajaj Pulsar RS200</a>
                        <div class="grid-7 alpha padding-right5">
                            <span class="bwmsprite calender-grey-sm-icon"></span>
                            <span class="article-stats-content">Jun 07, 2016</span>
                        </div>
                        <div class="grid-5 alpha omega">
                            <span class="bwmsprite author-grey-sm-icon"></span>
                            <span class="article-stats-content">BikeWale Team</span>
                        </div>
                        <div class="clear"></div>
                    </div>
                    <p class="margin-top10">
                        The Bajaj Pulsar RS200 gets a new white and red colour scheme combo for the Columbian market. The RS200 is available in India in three solid colours namely yellow, red...
                    </p>
                </div>

                <div class="margin-bottom20 font14">
                    <div class="review-image-wrapper">
                        <a href="" title="">
                            <img class="lazy" data-original="https://imgd1.aeplcdn.com//370x208//bw/ec/23838/Bajaj-Avenger-150-Street-Front-threequarter-74921.jpg?wm=0" alt="New colour scheme for Bajaj Pulsar RS200 in Columbia" src="">
                        </a>
                    </div>
                    <div class="review-heading-wrapper">
                        <a href="" title="" class="target-link">New colour scheme for Bajaj Pulsar RS200 in Columbia</a>
                        <div class="grid-7 alpha padding-right5">
                            <span class="bwmsprite calender-grey-sm-icon"></span>
                            <span class="article-stats-content">Jun 07, 2016</span>
                        </div>
                        <div class="grid-5 alpha omega">
                            <span class="bwmsprite author-grey-sm-icon"></span>
                            <span class="article-stats-content">BikeWale Team</span>
                        </div>
                        <div class="clear"></div>
                    </div>
                    <p class="margin-top10">
                        The Bajaj Pulsar RS200 gets a new white and red colour scheme combo for the Columbian market. The RS200 is available in India in three solid colours namely yellow, red...
                    </p>
                </div>

                <div class="margin-bottom20 font14">
                    <div class="review-image-wrapper">
                        <a href="" title="">
                            <img class="lazy" data-original="https://imgd1.aeplcdn.com//370x208//bw/ec/23997/Side-75963.jpg?wm=0" alt="New colour scheme for Bajaj Pulsar RS200 in Columbia" src="">
                        </a>
                    </div>
                    <div class="review-heading-wrapper">
                        <a href="" title="" class="target-link">New colour scheme for Bajaj Pulsar RS200 in Columbia</a>
                        <div class="grid-7 alpha padding-right5">
                            <span class="bwmsprite calender-grey-sm-icon"></span>
                            <span class="article-stats-content">Jun 07, 2016</span>
                        </div>
                        <div class="grid-5 alpha omega">
                            <span class="bwmsprite author-grey-sm-icon"></span>
                            <span class="article-stats-content">BikeWale Team</span>
                        </div>
                        <div class="clear"></div>
                    </div>
                    <p class="margin-top10">
                        The Bajaj Pulsar RS200 gets a new white and red colour scheme combo for the Columbian market. The RS200 is available in India in three solid colours namely yellow, red...
                    </p>
                </div>
                
                <a href="" class="font14">Read all bike maintenance tips<span class="bwmsprite blue-right-arrow-icon"></span></a>
            </div>
        </section>

        <section>
            <h2 class="section-heading">Bike troubleshooting - FAQs</h2>
            <div class="container bg-white box-shadow card-bottom-margin padding-bottom20">
                <ul class="accordion-list">
                    <li>
                        <div class="accordion-head">
                            <p class="accordion-head-title">I have starting problem with my Vehicle.</p>
                            <span class="bwmsprite arrow-sm-down"></span>
                        </div>                        
                        <div class="accordion-body">
                            <p>The Bajaj Pulsar RS200 gets a new white and red colour scheme combo for the Columbian market. The RS200 is available in India in three solid colours namely yellow, red</p>
                        </div>
                    </li>
                    <li>
                        <div class="accordion-head">
                            <p class="accordion-head-title">The disc brakes are giving a noise when applied</p>
                            <span class="bwmsprite arrow-sm-down"></span>
                        </div>                        
                        <div class="accordion-body">
                            <p>The Bajaj Pulsar RS200 gets a new white and red colour scheme combo for the Columbian market. The RS200 is available in India in three solid colours namely yellow, red</p>
                        </div>
                    </li>
                    <li>
                        <div class="accordion-head">
                            <p class="accordion-head-title">Horn is very feeble</p>
                            <span class="bwmsprite arrow-sm-down"></span>
                        </div>                        
                        <div class="accordion-body">
                            <p>The Bajaj Pulsar RS200 gets a new white and red colour scheme combo for the Columbian market. The RS200 is available in India in three solid colours namely yellow, red</p>
                        </div>
                    </li>
                </ul>

                <div class="padding-left20">
                    <a href="" class="font14">Read all FAQs<span class="bwmsprite blue-right-arrow-icon"></span></a>
                </div>
            </div>
        </section>

        <section>
            <div class="container section-container">
                <h2 class="section-heading">Newly launched bikes</h2>
                <div class="content-box-shadow padding-top15 padding-bottom15">
                    <div class="swiper-container card-container">
                        <div class="swiper-wrapper discover-bike-carousel">
                            <!-- control -->
                                   <%if (mctrlNewLaunchedBikes.FetchedRecordsCount > 0)
                           { %>
                    <BW:MNewLaunchedBikes runat="server" ID="mctrlNewLaunchedBikes" />
                        <%} %>
                            
                        </div>
                    </div>
                </div>
            </div>
        </section>

        <section>
            <div class="container section-container">
                <h2 class="section-heading">Upcoming bikes</h2>
                <div class="content-box-shadow padding-top15 padding-bottom15">
                    <div class="swiper-container card-container">
                        <div class="swiper-wrapper discover-bike-carousel">
                            <!-- control -->
                                   <%if (mctrlUpcomingBikes.FetchedRecordsCount > 0)
                           { %>
                      <BW:MUpcomingBikes runat="server" ID="mctrlUpcomingBikes" />
                        <%} %>
                              
                        </div>
                    </div>
                </div>
            </div>
        </section>

        <div id="locatorSearchBar" class="bwm-fullscreen-popup">
            <div class="locator-brand-slider-wrapper bwm-brand-city-box form-control-box text-left">
                <div class="user-input-box">
                    <span class="back-arrow-box"><span class="bwmsprite back-long-arrow-left"></span></span>
                    <input class="form-control" type="text" id="locatorBrandInput" placeholder="Select brand" />
                </div>
                <ul id="sliderBrandList" class="slider-brand-list margin-top40">
                    <asp:Repeater ID="rptMakes" runat="server">
                        <ItemTemplate>
                            <li makeMaskingName="<%# DataBinder.Eval(Container.DataItem,"MaskingName") %>" makeId="<%# DataBinder.Eval(Container.DataItem,"MakeId") %>"><%# DataBinder.Eval(Container.DataItem,"MakeName") %> </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </div>
            <div class="locator-city-slider-wrapper bwm-brand-city-box form-control-box text-left">
                <div class="user-input-box">
                    <span class="back-arrow-box"><span class="bwmsprite back-long-arrow-left"></span></span>
                    <input class="form-control" type="text" id="locatorCityInput" placeholder="Select City" />
                </div>
                <ul id="sliderCityList" class="slider-city-list margin-top40">
                    <asp:Repeater ID="rptCities" runat="server">
                        <ItemTemplate>
                            <li class="<%# ((DataBinder.Eval(Container.DataItem,"CityId")).ToString() != cityId.ToString())?string.Empty:"activeCity" %>" cityMaskingName="<%# DataBinder.Eval(Container.DataItem,"CityMaskingName") %>" cityId="<%# DataBinder.Eval(Container.DataItem,"CityId") %>" ><%# DataBinder.Eval(Container.DataItem,"CityName") %></li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </div>
        </div>

        
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/common.min.js?<%= staticFileVersion %>"></script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />
        <script type="text/javascript">
            var locatorSearchBar = $("#locatorSearchBar"),
                $ddlCities = $("#sliderCityList"),
                $ddlMakes = $("#sliderBrandList"),
                searchBrandDiv = $(".locator-search-brand"),
                searchCityDiv = $(".locator-search-city");
                searchBrandDiv.on('click', function () {
                $('.locator-city-slider-wrapper').hide();
                $('.locator-brand-slider-wrapper').show();
                locatorSearchBar.addClass('open').animate({ 'left': '0px' }, 500);
                $(".user-input-box").animate({ 'left': '0px' }, 500);
                $("#locatorBrandInput").focus();
                hideError(searchBrandDiv.find("div.locator-search-brand-form"));
                appendHash("locatorsearch");
            });
            searchCityDiv.on('click', function () {
                if ($('#sliderCityList li').length > 0) {
                    $('.locator-brand-slider-wrapper').hide();
                    $('.locator-city-slider-wrapper').show();
                    locatorSearchBar.addClass('open').animate({ 'left': '0px' }, 500);
                    $(".user-input-box").animate({ 'left': '0px' }, 500);
                    $("#locatorCityInput").focus();
                    hideError(searchCityDiv.find("div.locator-search-city-form"));
                    appendHash("locatorsearch");
                }
                else {
                    setError($("div.locator-search-brand-form"), "Please select brand!");
                }
            });

            $(document).ready(function() {
                $('#locatorBrandInput').fastLiveFilter('#sliderBrandList');
            });

            var key = "dealerCitiesByMake_";
            lscache.flushExpired();
            lscache.setBucket('DLPage');
            var selCityId = '<%= (cityId > 0)?cityId:0%>';
            var selMakeId = 0;

            if (($ddlCities.find("li.activeCity")).length > 0) {
                $("div.locator-search-city-form span").text($ddlCities.find("li.activeCity:first").text());
            }
            $ddlMakes.on("click", "li", function () {
                var _self = $(this),
                        selectedElement = _self.text();
                setSelectedElement(_self, selectedElement);
                _self.addClass('activeBrand').siblings().removeClass('activeBrand');
                $("div.locator-search-brand-form").find("span").text(selectedElement);
                selMakeId = $(this).attr("makeId");
                getCities(selMakeId);
                $(".user-input-box").animate({ 'left': '100%' }, 500);

            });

            $ddlCities.on("click", "li", function () {
                var _self = $(this),
                    selectedElement = _self.text();
                setSelectedElement(_self, selectedElement);
                _self.addClass('activeCity').siblings().removeClass('activeCity');
                if (!isNaN(selMakeId) && selMakeId != "0") {
                    selCityId = $(this).attr("cityId");
                }
                $(".user-input-box").animate({ 'left': '100%' }, 500);
                $("div.locator-search-city-form span").text(selectedElement);
            });

            $(".bwm-brand-city-box .back-arrow-box").on("click", function () {
                locatorSearchBar.removeClass("open").animate({ 'left': '100%' }, 500);
                $(".user-input-box").animate({ 'left': '100%' }, 500);
            });

            function locatorSearchClose() {
                $(".bwm-brand-city-box .back-arrow-box").trigger("click");
            }

            function setSelectedElement(_self, selectedElement) {
                _self.parent().prev("input[type='text']").val(selectedElement);
                locatorSearchBar.addClass('open').animate({ 'left': '100%' }, 500);
            };

            $("#view-brandType").click(function () {
                $(".brandTypeMore").slideToggle();
                var targetLink = $(this);
                targetLink.text(targetLink.text() == 'View more brands' ? 'View less brands' : 'View more brands');
                if (targetLink.text() === "View more brands")
                    targetLink.attr("href", "#more");
                else
                    targetLink.attr("href", "javascript:void(0)");
            });

            function getCities(mId) {
                $ddlCities.empty();
                if (!isNaN(mId) && mId != "0") {
                    if (!checkCacheCityAreas(mId)) {
                        $.ajax({
                            type: "GET",
                            url: "/api/v2/DealerCity/?makeId=" + mId,
                            contentType: "application/json",
                            dataType: 'json',
                            beforeSend: function () {
                                $("div.locator-search-city-form span").text("Loading cities..");
                            },
                            success: function (data) {
                                lscache.set(key + mId, data.City, 30);
                                $("div.locator-search-city-form span").text("Select city");
                                setOptions(data.City);
                            },
                            complete: function (xhr) {
                                if (xhr.status != 200) {
                                    $("div.locator-search-city-form span").text("No cities available");
                                    lscache.set(key + mId, null, 30);
                                    setOptions(null);
                                }
                                $('#locatorCityInput').fastLiveFilter('#sliderCityList');
                            }
                        });
                    }
                    else {
                        $("div.locator-search-city-form span").text("Select city")
                        data = lscache.get(key + mId);
                        setOptions(data);
                    }

                }
            }

            $("input[type='button'].locator-submit-btn").click(function () {
                ddlmakemasking = $ddlMakes.find("li.activeBrand").attr("makeMaskingName");
                ddlcityId = $ddlCities.find("li.activeCity").attr("cityId");
                if (!isNaN(selMakeId) && selMakeId != "0") {
                    if (!isNaN(selCityId) && selCityId != "0") {
                        ddlcityMasking = $ddlCities.find("li.activeCity").attr("cityMaskingName");
                        //window.location.href = "/m/new/" + ddlmakemasking + "-dealers/" + ddlcityId + "-" + ddlcityMasking + ".html";
                        window.location.href = "/m/" + ddlmakemasking + "-dealer-showrooms-in-" + ddlcityMasking + "/";
                    }
                    else {
                        setError($("div.locator-search-city-form"), "Please select city!");
                    }
                }
                else {
                    setError($("div.locator-search-brand-form"), "Please select bike brand!");
                }
            });


            function checkCacheCityAreas(cityId) {
                bKey = key + cityId;
                if (lscache.get(bKey)) return true;
                else return false;
            }

            function setOptions(optList) {
                if (optList != null) {
                    $.each(optList, function (i, value) {
                        $ddlCities.append($('<li>').text(value.cityName).attr('cityId', value.cityId).attr('cityMaskingName', value.cityMaskingName));
                    });
                }
                else {
                    $("div.locator-search-city-form span").text("No cities available");
                }
            }

            var setError = function (element, msg) {
                element.addClass("border-red").siblings("span.errorIcon, div.errorText").show();
                element.siblings("div.errorText").text(msg);
            };

            var hideError = function (element) {
                element.removeClass("border-red").siblings("span.errorIcon, div.errorText").hide();
            };

            // faqs
            $('.accordion-list').on('click', '.accordion-head', function () {
                var element = $(this);

                if (!element.hasClass('active')) {
                    accordion.open(element);
                }
                else {
                    accordion.close(element);
                }
            });

            var accordion = {
                open: function (element) {
                    var elementSiblings = element.closest('.accordion-list').find('.accordion-head.active');
                    elementSiblings.removeClass('active').next('.accordion-body').slideUp();

                    element.addClass('active').next('.accordion-body').slideDown();
                },

                close: function (element) {
                    element.removeClass('active').next('.accordion-body').slideUp();
                }
            };

            jQuery.fn.fastLiveFilter = function (list, options) {
                // Options: input, list, timeout, callback
                options = options || {};
                list = jQuery(list);
                var input = this;
                var lastFilter = '', noResultLen = 0;
                var noResult = '<div class="noResult">No search found!</div>';
                var timeout = options.timeout || 100;
                var callback = options.callback || function (total) {
                    noResultLen = list.siblings(".noResult").length;

                    if (total == 0 && noResultLen < 1) {
                        list.after(noResult).show();
                    }
                    else if (total > 0 && noResultLen > 0) {
                        $('.noResult').remove();
                    }
                };

                var keyTimeout;
                var lis = list.children();
                var len = lis.length;
                var oldDisplay = len > 0 ? lis[0].style.display : "block";
                callback(len); // do a one-time callback on initialization to make sure everything's in sync

                input.change(function () {
                    // var startTime = new Date().getTime();
                    var filter = input.val().toLowerCase();
                    var li, innerText;
                    var numShown = 0;
                    for (var i = 0; i < len; i++) {
                        li = lis[i];
                        innerText = !options.selector ?
                            (li.textContent || li.innerText || "") :
                            $(li).find(options.selector).text();

                        if (innerText.toLowerCase().indexOf(filter) >= 0) {
                            if (li.style.display == "none") {
                                li.style.display = oldDisplay;
                            }
                            numShown++;
                        } else {
                            if (li.style.display != "none") {
                                li.style.display = "none";
                            }
                        }
                    }
                    callback(numShown);
                    return false;
                }).keydown(function () {
                    clearTimeout(keyTimeout);
                    keyTimeout = setTimeout(function () {
                        if (input.val() === lastFilter) return;
                        lastFilter = input.val();
                        input.change();
                    }, timeout);
                });
                return this; // maintain jQuery chainability
            }

        </script>
    </form>
</body>
</html>