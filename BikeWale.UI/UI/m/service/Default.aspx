<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Service.LocateServiceCenter" EnableViewState="false" %>
<%@ Register Src="~/UI/m/controls/BikeCare.ascx" TagName="BikeCare" TagPrefix="BW" %>
<%@ Register Src="~/UI/m/controls/MUpcomingBikes.ascx" TagName="MUpcomingBikes" TagPrefix="BW" %>
<%@ Register Src="~/UI/m/controls/MNewLaunchedBikes.ascx" TagName="MNewLaunchedBikes" TagPrefix="BW" %>
<%@ Register Src="~/UI/m/controls/MMostPopularBikes.ascx" TagName="MMostPopularBikes" TagPrefix="BW" %>
<%@ Register Src="~/UI/m/controls/usedBikeModel.ascx" TagName="usedBikeModel" TagPrefix="BW" %>
<%@ Register Src="~/UI/m/controls/usedBikeInCities.ascx" TagName="usedBikeInCities" TagPrefix="BW" %>

<!DOCTYPE html>
<html>
<head>
    <%
        title = "Locate Authorised Bike Service Center | Bikes Servicing Center Nearby - BikeWale";
        keywords = "servicing, bike servicing, authorised service centers, bike service centers, servicing bikes, bike repairing, repair bikes";
        description = "Locate authorised service centers in India. Find authorised service centers of Hero, Honda, Bajaj, Royal Enfield, Harley Davidson, Yamaha, KTM, Aprilia and many more brands in more than 1000+ cities.";
        canonical = "https://www.bikewale.com/service-centers/";
        AdPath = "/1017752/Bikewale_Mobile_NewBikes";
        AdId = "1398766302464";
        Ad_320x50 = false;
        Ad_Bot_320x50 = false;
        //menu = "10";
    %>
    <!-- #include file="/UI/includes/headscript_mobile_min.aspx" -->
    <link rel="stylesheet" type="text/css" href="/UI/m/css/service/landing.css" />
    <script type="text/javascript">
        <!-- #include file="\UI\includes\gacode_mobile.aspx" -->
    </script>
</head>
<body class="bg-light-grey page-type-landing">
    <form id="form1" runat="server">
        <!-- #include file="/UI/includes/headBW_Mobile.aspx" -->
        <section>
            <div class="container locator-landing-banner text-center">
                <h1 class="font24 text-uppercase text-white">Bike service center locator</h1>
                <h2 class="font14 text-unbold text-white">Find bike service center across 1000+ cities in India</h2>
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
         <% if(TopMakeList!= null ){  %>
        <section>
            <h2 class="section-heading">Service Centers by brands</h2>
            <div class="container bg-white box-shadow padding-top25 padding-bottom20 collapsible-brand-content">
                <div id="brand-type-container" class="brand-type-container">
                        <ul class="text-center">
                           <%foreach(var bikebrand in TopMakeList) {%>
                                      <li>
                                        <a href="/m/service-centers/<%=bikebrand.MaskingName %>/" title="<%=bikebrand.MakeName %> Service Center in India">
                                            <span class="brand-type">
                                                <span class="lazy brandlogosprite brand-<%=bikebrand.MakeId %>"></span>
                                            </span>
                                            <span class="brand-type-title"><%=bikebrand.MakeName %></span>
                                        </a>
                                    </li>
                           <%} %>
                        </ul>
                    <%if(OtherMakeList!= null){ %>
                        <ul class="brand-style-moreBtn brandTypeMore border-top1 padding-top25 text-center hide">
                                 <%foreach (var bikebrand in OtherMakeList)
                                   {%>
                                      <li>
                                        <a href="/m/service-centers/<%=bikebrand.MaskingName %>/"  title="<%=bikebrand.MakeName %> Service Center in India">
                                            <span class="brand-type">
                                                <span class="lazy brandlogosprite brand-<%=bikebrand.MakeId %>"></span>
                                            </span>
                                            <span class="brand-type-title"><%=bikebrand.MakeName %></span>
                                        </a>
                                    </li>
                           <%} %>
                        </ul>

                </div>
                <div class="view-all-btn-container">
                        <a href="javascript:void(0)" class="view-brandType view-more-btn btn view-all-target-btn rotate-arrow" rel="nofollow"><span class="btn-label">View more brands</span><span class="bwmsprite teal-right"></span></a>
                    </div>
                  <%} %>
            </div>
        </section>
        <%} %>
        <%if(ctrlBikeCare.FetchedRecordsCount>0) {%>
        <section>
            <BW:BikeCare runat="server" ID="ctrlBikeCare" />
        </section>
        <%} %>
        <%if (ctrlMostPopularBikes.FetchedRecordsCount + ctrlMostPopularBikes.FetchedRecordsCount + ctrlMostPopularBikes.FetchedRecordsCount > 0)
          {%>
        <section>
            <!--  Upcoming, New Launches and Top Selling code starts here -->
            <div class="container">
                <div class="grid-12 alpha omega">
                    <h2 class="font18 text-center margin-top20 margin-bottom10">Most popular bikes</h2>
                    <div class="featured-bikes-panel content-box-shadow padding-bottom15">
                        <div class="bw-tabs-panel">
                        <div class="bw-tabs bw-tabs-flex">
                            <ul>
                                <%if(ctrlMostPopularBikes.FetchedRecordsCount > 0){ %><li class="active"  data-tabs="mctrlMostPopularBikes">Most Popular</li><%} %>
                                <%if(ctrlNewLaunchedBikes.FetchedRecordsCount > 0){ %> <li  data-tabs="mctrlNewLaunchedBikes">New launches</li><%} %>
                                <%if(ctrlUpcomingBikes.FetchedRecordsCount > 0){ %> <li  data-tabs="mctrlUpcomingBikes">Upcoming </li><%} %>

                            </ul>
                        </div>
                        <div class="grid-12 alpha omega">
                            <div class="bw-tabs-data features-bikes-container" id="mctrlMostPopularBikes">
                                <div class="swiper-container card-container">
                                    <div class="swiper-wrapper discover-bike-carousel">
                                        <BW:MMostPopularBikes PageId="4" runat="server" ID="ctrlMostPopularBikes" />
                                    </div>
                                </div>
                                <div class="padding-left10 view-all-btn-container margin-top10">
                            <a href="/m/best-bikes-in-india/" title="Popular Bikes in India" class="btn view-all-target-btn">View all bikes<span class="bwmsprite teal-right"></span></a>
                               </div>
                            </div>
                            <div class="bw-tabs-data hide features-bikes-container" id="mctrlNewLaunchedBikes">
                                <div class="swiper-container card-container">
                                    <div class="swiper-wrapper discover-bike-carousel">
                                        <BW:MNewLaunchedBikes PageId="4" runat="server" ID="ctrlNewLaunchedBikes" />
                                    </div>
                                </div>
                                <div class="padding-left10 view-all-btn-container margin-top10">
                            <a href="/m/new-bike-launches/" title="New Bike Launches in India" class="btn view-all-target-btn">View all launches<span class="bwmsprite teal-right"></span></a>
                               </div>
                            </div>
                            <div class="bw-tabs-data hide features-bikes-container" id="mctrlUpcomingBikes">
                                <div class="swiper-container card-container">
                                    <div class="swiper-wrapper discover-bike-carousel">
                                        <BW:MUpcomingBikes runat="server" ID="ctrlUpcomingBikes" />
                                    </div>
                                </div>
                                <div class="padding-left10 view-all-btn-container margin-top10">
                            <a href="/m/upcoming-bikes/" title="Upcoming Bikes in India" class="btn view-all-target-btn">View all bikes<span class="bwmsprite teal-right"></span></a>
                               </div>
                            </div>
                        </div>
                    </div>
                        <div class="clear"></div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <%} %>
         <section>
            <div class="container">
                <h2 class="section-heading">Find used bikes</h2>
                <div class="bw-tabs-panel content-box-shadow">
                    <div class="bw-tabs bw-tabs-flex tabs-bottom15">
                        <ul>
                            <% if (ctrlusedBikeModel.FetchCount > 0)
                               { %>
                            <li class="active" data-tabs="usedByModel">Model</li>
                            <%} %>
                           
                            <%if (ctrlusedBikeInCities.objCitiesWithCount != null && ctrlusedBikeInCities.objCitiesWithCount.Count() > 0)
                              { %><li class="<%=ctrlusedBikeModel.FetchCount > 0?"":"active"%>" data-tabs="usedByCity">City</li>
                            <%} %>
                             <li  class="<%=((ctrlusedBikeModel.FetchCount>0) ||( ctrlusedBikeInCities.objCitiesWithCount != null && ctrlusedBikeInCities.objCitiesWithCount.Count() > 0))?"":"active"%>" data-tabs="usedByBudget">Budget</li>
                        </ul>
                    </div>
                      <% if (ctrlusedBikeModel.FetchCount > 0)
                       { %>
                    <div class="bw-tabs-data" id="usedByModel">
                        <BW:usedBikeModel runat="server" ID="ctrlusedBikeModel" />
                    </div>
                    <% } %>
                     <div class="clear"></div>
                    <%if (ctrlusedBikeInCities.objCitiesWithCount != null && ctrlusedBikeInCities.objCitiesWithCount.Count() > 0)
                      { %>
                    <div class="bw-tabs-data <%=ctrlusedBikeModel.FetchCount > 0?"hide":""%>" id="usedByCity">
                        <BW:usedBikeInCities runat="server" ID="ctrlusedBikeInCities" />
                         
                    </div>
                    <%} %>
                    <div class="bw-tabs-data <%=((ctrlusedBikeModel.FetchCount>0) ||( ctrlusedBikeInCities.objCitiesWithCount != null && ctrlusedBikeInCities.objCitiesWithCount.Count() > 0))?"hide":""%>" id="usedByBudget">
                        <ul class="elevated-card-list padding-top5">
                            <li>
                                <a href="/m/used/bikes-in-india/#budget=0+35000" rel="nofollow">
                                    <div class="table-middle">
                                        <div class="tab-icon-container">
                                            <span class="bwmsprite budget-one"></span>
                                        </div>
                                        <span class="key-size-14">Upto</span><br />
                                        <span class="bwmsprite inr-xsm-icon"></span><span class="value-size-15">35K</span>
                                    </div>
                                </a>
                            </li>
                            <li>
                                <a href="/m/used/bikes-in-india/#budget=35000+80000" rel="nofollow">
                                    <div class="table-middle">
                                        <div class="tab-icon-container">
                                            <span class="bwmsprite budget-two"></span>
                                        </div>
                                        <span class="key-size-14">Between</span><br />
                                        <span class="bwmsprite inr-xsm-icon"></span><span class="value-size-15">35K -</span>
                                        <span class="bwmsprite inr-xsm-icon"></span><span class="value-size-15">80K</span>
                                    </div>
                                </a>
                            </li>
                            <li>
                                <a href="/m/used/bikes-in-india/#budget=80000+200000" rel="nofollow">
                                    <div class="table-middle">
                                        <div class="tab-icon-container">
                                            <span class="bwmsprite budget-three"></span>
                                        </div>
                                        <span class="key-size-14">Above</span><br />
                                        <span class="bwmsprite inr-xsm-icon"></span><span class="value-size-15">80K</span>
                                    </div>
                                </a>
                            </li>
                        </ul>

                        <div class="padding-left10 view-all-btn-container margin-top10 padding-bottom20">
                            <a href="<%=usedBikeLink %>" title="<%=usedBikeTitle%>" class="btn view-all-target-btn">View all used bikes<span class="bwmsprite teal-right"></span></a>
                               </div>

                    </div>

                  
                </div>

            </div>
        </section>
        <section>
            <h2 class="section-heading">Bike Troubleshooting - FAQs</h2>
            <div class="container bg-white box-shadow card-bottom-margin padding-bottom20">
                <ul class="accordion-list">
                    <li>
                        <div class="accordion-head">
                            <p class="accordion-head-title">What to do if you have a puncture?</p>
                            <span class="bwmsprite arrow-sm-down"></span>
                        </div>
                        <div class="accordion-body">
                            <p>There's two kinds of tyres available to the public, the tube type and the tubeless type. If you've got spoked rims, you've got a tyre with a tube. The puncture will leave you without air suddenly, and the best you can do is flag down a passing cab or rickshaw, go to the puncture repair shop and get the person to the motorcycle. Repeat the trip to the repair shop (this time with the wheel) and back again to the motorcycle. If that isn't possible, put the bike in first, engage the clutch and walk the motorcycle to the shop. Remember that you risk damaging your tyre this way.<br /><br />Tubeless tyres are a lot easier to deal with. The very thing that punctures the tyre also seals the hole, so the leak is far slower. You can ride the motorcycle, but be very careful. Ride it with too little air and you risk damaging the tyre beyond repair. If the nearest puncture repair shop isn't equipped with a tubeless puncture repair kit, make them fill a lot of air in the tyre and ride on. If you can't find a puncture repair shop, even a bicycle pump can help you fill air in the tyre.</p>
                        </div>
                    </li>
                    <li>
                        <div class="accordion-head">
                            <p class="accordion-head-title">What to do if your battery is weak and you have no kick start lever?</p>
                            <span class="bwmsprite arrow-sm-down"></span>
                        </div>
                        <div class="accordion-body">
                            <p>We've all been there at some point, and there's the obvious - jumper cables.<br /><br />Remember to take a jump from a battery that has a higher rating than yours, else you run the risk of two motorcycles that won't start after your attempts. The next obvious thing to do is to either remove the battery, get it charged and reinstall it, or replace it with a fully charged one. There are a few other things you can do if jumper cables aren't available, or you don't know how to remove your battery.<br /><br />Note: if it is a large motorcycle (say over 400cc) do not attempt anything you read beyond this. If, however, you have a small motorcycle with a carburettor, here's exactly what you need to do: stick it in second, pull the clutch in, push the bike and release the clutch. As soon as it catches, pull the clutch in. If you have a helping hand, it is far safer to have one person sit on the motorcycle while the other pushes. Another trick that you can use for small motorcycles is putting them on the main stand – the same rules apply. Stick it in second, leave the ignition on and just give the rear wheel torque by pulling it in the correct direction. If you give it a hard enough tug, the bike should start.<br /><br />There is one other condition under which a push-start will not achieve any results at all: if you have fuel injection on your motorcycle, turn the key over to the 'on' position and put your ear near the fuel tank. If you hear a noise, however weak, it means that the fuel pump is still working enough to send fuel to the engine, and you have a chance of the bike starting. Pull the fuses to the headlamp to keep it from taking any more juice away from the fuel pump and try the push start. If you turn your key to 'on' and hear nothing at all, then don't bother trying, your motorcycle won't start no matter how much you push it.<br /><br />Revving the motorcycle to the redline will not make the battery charge faster – anything beyond 3000 rpm is a waste of fuel, so go for a 20-30 minute cruise to make sure the battery gets charged enough to crank the engine should you stall for any reason.</p>
                        </div>
                    </li>
                    <li>
                        <div class="accordion-head">
                            <p class="accordion-head-title">What to do if your clutch cable breaks?</p>
                            <span class="bwmsprite arrow-sm-down"></span>
                        </div>
                        <div class="accordion-body">
                            <p>If you have a scooter, obviously this isn't a problem. However, this can be quite a big issue if you've got something that needs gears to be shifted manually.<br /><br />The best thing to do is to stick it in neutral and either push the motorcycle along or have someone tow you. If this isn't possible, though, technology and a little bit of looking ahead can help you get to help. There's something called 'synchromesh' that gearboxes have today, and that means that you can actually change gears without using the clutch lever. It will take a little bit of practice, though, especially while downshifting. Upshifts will be a lot smoother. The biggest problem will be coming to a halt and taking off from a halt. For this, the obvious solution will be to not do it at all, so you can either wait for a time when there won't be traffic or use a route with little to no traffic or stop signals.<br /><br />If it cannot be avoided, though, you'll have to slow down as much you can in first gear, and then try to put it into neutral while using the brakes to come to a complete halt. Starting it will be very tricky, because it will be almost impossible to get it going with just enough throttle to remain in control of the motorcycle. If you have a main stand, you can try putting it on the main stand, putting it in gear and then doing a running start with it.<br /><br />Remember – these are very risky manoeuvres, so please do not try them unless there is an emergency and you cannot afford to wait at all.</p>
                        </div>
                    </li>
                </ul>

                <div class="padding-left20">
                    <a href="/m/bike-troubleshooting/" title="Bike Troubleshooting- FAQs" target="_blank" rel="noopener" class="font14">Read all FAQs<span class="bwmsprite blue-right-arrow-icon"></span></a>
                </div>
            </div>
        </section>


        <%if (makes!=null) {%>
          <div id="locatorSearchBar" class="bwm-fullscreen-popup">
            <div class="locator-brand-slider-wrapper bwm-brand-city-box form-control-box text-left">
                <div class="user-input-box">
                    <span class="back-arrow-box"><span class="bwmsprite back-long-arrow-left"></span></span>
                    <input class="form-control" type="text" id="locatorBrandInput" placeholder="Select brand" />
                </div>
                <ul id="sliderBrandList" class="slider-brand-list margin-top40">
                  <%foreach (var bikebrand in makes)
                                   {%>
                            <li makeMaskingName="<%=bikebrand.MaskingName %>" makeId="<%=bikebrand.MakeId %>"><%=bikebrand.MakeName %> </li>
                     <%} %>
                </ul>
            </div>


            <div class="locator-city-slider-wrapper bwm-brand-city-box form-control-box text-left">
                <div class="user-input-box">
                    <span class="back-arrow-box"><span class="bwmsprite back-long-arrow-left"></span></span>
                    <input class="form-control" type="text" id="locatorCityInput" placeholder="Select City" />
                </div>
                <ul id="sliderCityList" class="slider-city-list margin-top40">
                    <%if(cities!=null){ %>
                    <%foreach (var city in cities)
                                   {%>
                            <li class="<%=(city.CityId != cityId)?string.Empty:"activeCity" %>" cityMaskingName="<%=city.CityMaskingName %>" cityId="<%=city.CityId %>"><%=city.CityName%></li>
                     <%} %>              <%} %>
                </ul>
            </div>

        </div>
        <%} %>


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

            var key = "ServiceCenterCitiesByMake_";
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

            function getCities(mId) {
                $ddlCities.empty();
                if (!isNaN(mId) && mId != "0") {
                    if (!checkCacheCityAreas(mId)) {
                        $.ajax({
                            type: "GET",
                            url: "/api/servicecenter/cities/make/"+ mId + "/",
                            contentType: "application/json",
                            dataType: 'json',
                            beforeSend: function () {
                                $("div.locator-search-city-form span").text("Loading cities..");
                            },
                            success: function (data) {
                                lscache.set(key + mId, data, 30);
                                $("div.locator-search-city-form span").text("Select city");
                                setOptions(data);
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
                        $("div.locator-search-city-form span").text("Select city");
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
                        ddlcityMasking = $ddlCities.find("li.activeCity").attr("citymaskingname");
                        bwcache.remove("userchangedlocation", true);
                        window.location.href = "/m/service-centers/" + ddlmakemasking + "/" + ddlcityMasking + "/";
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
                        $ddlCities.append($('<li>').text(value.cityName).attr('cityId', value.cityId).attr('citymaskingname', value.cityMaskingName));
                    });
                }
                else {
                    $("div.locator-search-city-form span").text("No cities available");
                }

                if (optList) {
                    var selectedElement = $.grep(optList, function (element, index) {
                        return element.cityId == selCityId;
                    });
                    if (selectedElement.length > 0) {
                        $("div.locator-search-city-form span").text(selectedElement[0].cityName);
                        $('#sliderCityList li[cityId="' + selectedElement[0].cityId + '"]').addClass('activeCity');
                    }
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
