<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.New.DealerLocator.LocateNewBikesDealers" EnableViewState="false" %>
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
    <style type="text/css">
        @charset "utf-8";.locator-landing-banner{background:url(http://imgd1.aeplcdn.com/0x0/bw/static/landing-banners/m/dealer-locator-banner.jpg) center bottom no-repeat #8d8c8a;background-size:cover;height:130px;padding-top:1px}.section-container{margin-bottom:25px}h2.section-heading{font-size:18px;text-align:center;margin-bottom:15px}.banner-box-shadow{background:#fff;margin-top:-25px;-moz-box-shadow:2px 2px 2px rgba(0,0,0,.2);-webkit-box-shadow:2px 2px 2px rgba(0,0,0,.2);-o-box-shadow:2px 2px 2px rgba(0,0,0,.2);-ms-box-shadow:2px 2px 2px rgba(0,0,0,.2);box-shadow:2px 2px 2px rgba(0,0,0,.2);border-radius:2px}.locator-search-container .form-control{padding:8px}.locator-search-brand,.locator-search-city{width:100%;height:40px}.locator-search-city{margin-bottom:30px}.locator-search-form{padding:9px 25px 9px 8px;cursor:pointer;background:url(http://imgd1.aeplcdn.com/0x0/bw/static/design15/old-images/m/dropArrowBg.png?v1=19082015) 96% 50% no-repeat #fff;text-align:left;overflow:hidden;text-overflow:ellipsis;white-space:nowrap;border-radius:2px;width:100%;height:38px;color:#4d5057;border:1px solid #e2e2e2}.locator-search-brand select,.locator-search-city select{border:none;display:none}.locator-search-city select{border:1px solid #ccc}.locator-submit-btn{width:41%}.locator-submit-btn.btn-lg{padding:8px 24px}.locator-search-container .errorIcon,.locator-search-container .errorText{display:none}.brandlogosprite{background-image:url(http://imgd3.aeplcdn.com/0x0/bw/static/sprites/m/brand-type-sprite.png?22Mar2016v1);background-repeat:no-repeat;display:inline-block}.brand-type-container li{display:inline-block;vertical-align:top;width:90px;height:70px;margin-bottom:10px;text-align:center}.brand-type-container li:hover .brand-type-title{font-weight:700}.brand-type-container a{text-decoration:none;color:#565a5c;display:inline-block}.brand-type-title{display:block;text-transform:capitalize}#locatorSearchBar.bwm-fullscreen-popup{padding:0;background:#f5f5f5;z-index:11;position:fixed;left:100%;top:0;overflow-y:scroll;width:100%;height:100%}#locatorSearchBar li{border-top:1px solid #ccc;font-size:14px;padding:15px 10px;color:#333;cursor:pointer}#locatorSearchBar li:hover{background:#ededed}.booking-area-slider-wrapper{display:none}.bwm-brand-city-box .back-arrow-box,.bwm-brand-city-box .cross-box{height:30px;width:40px;position:absolute;top:5px;z-index:11;cursor:pointer}.bwm-brand-city-box span.back-long-arrow-left{position:absolute;top:7px;left:10px}.bwm-brand-city-box .back-arrow-box{position:absolute;left:5px}.bwm-brand-city-box .form-control{padding:10px 50px}.activeBrand,.activeCity{font-weight:700;background-color:#ddd}.brand-1,.brand-10,.brand-11,.brand-12,.brand-13,.brand-14,.brand-15,.brand-16,.brand-17,.brand-19,.brand-2,.brand-20,.brand-22,.brand-3,.brand-34,.brand-39,.brand-4,.brand-40,.brand-41,.brand-42,.brand-5,.brand-6,.brand-7,.brand-8,.brand-81,.brand-9{height:30px}.brand-2{width:52px;background-position:0 0}.brand-7{width:34px;background-position:-58px 0}.brand-1{width:53px;background-position:-97px 0}.brand-8{width:60px;background-position:-156px 0}.brand-12{width:40px;background-position:-222px 0}.brand-40{width:75px;background-position:-268px 0}.brand-34{width:73px;background-position:-349px 0}.brand-22{width:73px;background-position:-428px 0}.brand-3{width:26px;background-position:-507px 0}.brand-17{width:52px;background-position:-539px 0}.brand-15{width:71px;background-position:-597px 0}.brand-4{width:26px;background-position:-674px 0}.brand-9{width:59px;background-position:-706px 0}.brand-16{width:70px;background-position:-771px 0}.brand-5{width:35px;background-position:-847px 0}.brand-19{width:73px;background-position:-889px 0}.brand-13{width:73px;background-position:-968px 0}.brand-6{width:38px;background-position:-1047px 0}.brand-10{width:61px;background-position:-1091px 0}.brand-14{width:76px;background-position:-1159px 0}.brand-39{width:53px;background-position:-1242px 0}.brand-20{width:49px;background-position:-1300px 0}.brand-11{width:74px;background-position:-1354px 0}.brand-41{width:40px;background-position:-1432px 0}.brand-42{width:38px;background-position:-1481px 0}.brand-81{width:38px;background-position:-1529px 0}.noResult{font-size:14px;color:#82888b;padding:15px 10px}
    </style>
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_mobile.aspx" -->
    </script>
</head>
<body class="bg-light-grey">
    <form id="form1" runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->
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
                        <h2 class="section-heading">Search dealers</h2>
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
                <h2 class="section-heading">Dealer showroom by brands</h2>
                <div class="content-box-shadow padding-top25 padding-bottom20">
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
                        $("div.locator-search-city-form span").text("Select a city")
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
            }

        </script>
    </form>
</body>
</html>