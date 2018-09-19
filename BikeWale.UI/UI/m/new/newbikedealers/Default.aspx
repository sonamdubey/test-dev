<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.New.DealerLocator.LocateNewBikesDealers" EnableViewState="false" %>
<%@ Register Src="~/UI/m/controls/MUpcomingBikes.ascx" TagName="MUpcomingBikes" TagPrefix="BW" %>
<%@ Register Src="~/UI/m/controls/MNewLaunchedBikes.ascx" TagName="MNewLaunchedBikes" TagPrefix="BW" %>

<!DOCTYPE html>
<html>
<head>
    <%
        title = "New Bike Showroom in India | Find Authorized Bike Dealers - BikeWale";
        keywords = "new bike dealers, new bike showrooms, bike dealers, bike showrooms, showrooms, dealerships";
        description = "Locate new bike showrooms and authorized bike dealers in India. Find new bike dealer information for more than 200 cities in India. ";
        canonical = "https://www.bikewale.com/dealer-showrooms/";
        AdPath = "/1017752/Bikewale_Mobile_NewBikes";
        AdId = "1398766302464";
        Ad_320x50 = false;
        Ad_Bot_320x50 = false;
        //menu = "10";
    %>
    <!-- #include file="/UI/includes/headscript_mobile_min.aspx" -->
    <link rel="stylesheet" type="text/css" href="/UI/m/css/dealer/landing.css" />
    <script type="text/javascript">
        <!-- #include file="\UI\includes\gacode_mobile.aspx" -->
    </script>
</head>
<body class="bg-light-grey page-type-landing">
    <form id="form1" runat="server">
        <!-- #include file="/UI/includes/headBW_Mobile.aspx" -->
        <section>
            <div class="container locator-landing-banner text-center">
                <h1 class="font24 text-uppercase text-white padding-top20 padding-bottom10">Showroom Locator</h1>
                <h2 class="font14 text-unbold text-white">Find new bike dealers across 200+ cities</h2>
            </div>
        </section>

        <section>
            <div class="container section-container">
                <div class="grid-12">
                    <div class="banner-box-shadow content-inner-block-20">
                        <h2 class="section-heading">Find bike dealers in your city</h2>
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
            <div class="container section-container">
                <h2 class="section-heading">Explore showroom by brand</h2>
                <div class="content-box-shadow padding-top25 padding-bottom20 collapsible-brand-content">
                    <div id="brand-type-container" class="brand-type-container">
                        <ul class="text-center">
                            <asp:Repeater ID="rptPopularBrands" runat="server">
                                <ItemTemplate>
                                    <li>
                                        <a href="/m/dealer-showrooms/<%# DataBinder.Eval(Container.DataItem, "MaskingName") %>/">
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
                                        <a href="/m/dealer-showrooms/<%# DataBinder.Eval(Container.DataItem, "MaskingName") %>/">
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
                    <div class="view-all-btn-container">
                        <a href="javascript:void(0)" class="view-brandType view-more-btn btn view-all-target-btn rotate-arrow" rel="nofollow"><span class="btn-label">View more brands</span><span class="bwmsprite teal-right"></span></a>
                    </div>
                </div>
            </div>
        </section>

        <section>
            <div class="container section-container">
                <h2 class="section-heading">New Bike Launches in India</h2>
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
                    <%if (mctrlNewLaunchedBikes.FetchedRecordsCount > 0)
                           { %>
                    <div class="padding-left10 view-all-btn-container margin-top10">
                            <a href="/m/new-bike-launches/" title="New Bike Launches in India" class="btn view-all-target-btn">View all launches<span class="bwmsprite teal-right"></span></a>
                               </div>
                    <%} %>
                </div>
            </div>
        </section>

        <section>
            <div class="container section-container">
                <h2 class="section-heading">Upcoming Bikes in India</h2>
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
                <div class="padding-left10 view-all-btn-container margin-top10">
                     <a href="/m/upcoming-bikes/" title="Upcoming Bikes in India" class="btn view-all-target-btn">View all bikes<span class="bwmsprite teal-right"></span></a>
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

        
        <script type="text/javascript" src="<%= staticUrl  %>/UI/m/src/frameworks.js?<%= staticFileVersion %>"></script>
        <!-- #include file="/UI/includes/footerBW_Mobile.aspx" -->
        <link href="<%= staticUrl  %>/UI/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <!-- #include file="/UI/includes/footerscript_mobile.aspx" -->
        <!-- #include file="/UI/includes/fontBW_Mobile.aspx" -->
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
                var obj = GetGlobalLocationObject();
                if (obj != null) {
                   selectCityObject(obj.CityId);
                }
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
                                $("div.locator-search-city-form span").text("Select a city");
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
                        $("div.locator-search-city-form span").text("Select a city");
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
                        bwcache.remove("userchangedlocation", true);
                        window.location.href = "/m/dealer-showrooms/" + ddlmakemasking + "/" + ddlcityMasking + "/";
                    }
                    else {
                        setError($("div.locator-search-city-form"), "Please select city !");
                    }
                }
                else {
                    setError($("div.locator-search-brand-form"), "Please Select bike brand !");
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
            };

            function selectCityObject(cityId) {
                var cityObj = {};
                $("#sliderCityList > li").each(function () {
                    if ($(this).attr('cityid') == cityId) {
                        $(this).trigger('click');
                        return false;
                    }
                });
                return cityObj;
            }
        </script>
    </form>
</body>
</html>