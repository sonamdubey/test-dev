<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.ServiceCenter.Default" EnableViewState="false" %>
<%@ Register Src="~/controls/BikeCare.ascx" TagName="BikeCare" TagPrefix="BW" %>
<%@ Register Src="~/controls/UpcomingBikes_new.ascx" TagName="UpcomingBikes" TagPrefix="BW" %>
<%@ Register Src="~/controls/NewLaunchedBikes_new.ascx" TagName="NewLaunchedBikes" TagPrefix="BW" %>
<%@ Register Src="~/controls/MostPopularBikes_new.ascx" TagName="MostPopularBikes" TagPrefix="BW" %>
<%@ Register Src="~/controls/usedBikeModel.ascx" TagName="usedBikeModel" TagPrefix="BW" %>
<%@ Register Src="~/controls/usedBikeInCities.ascx" TagName="usedBikeInCities" TagPrefix="BW" %>
<%@ Import Namespace="Bikewale.Common" %>
<!doctype html>
<html>
<head>
    <%
        title = "Locate Authorised Bike Service Center | Bikes Servicing Center Nearby - BikeWale";
        keywords = "servicing, bike servicing, authorised service centers, bike service centers, servicing bikes, bike repairing, repair bikes";
        description = "Locate authorised service centers in India. Find authorised service centers of Hero, Honda, Bajaj, Royal Enfield, Harley Davidson, Yamaha, KTM, Aprilia and many more brands in more than 1000+ cities.";
        canonical = "https://www.bikewale.com/bike-service-center/";
        alternate = "https://www.bikewale.com/m/bike-service-center/";
        isHeaderFix = false;
        isAd970x90Shown = false;
        isTransparentHeader = true;
        isAd300x250Shown = false;
        isAd300x250BTFShown = false;
        isAd970x90BottomShown = false;
    %>
    <!-- #include file="/includes/headscript_desktop_min.aspx" -->
    <link rel="stylesheet" type="text/css" href="/css/service/landing.css" />
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_desktop.aspx" -->
    </script>
</head>
<body class="bg-light-grey page-type-landing">
    <form runat="server">
        <!-- #include file="/includes/headBW.aspx" -->
        <header class="service-center-banner">
            <div id="service-center-box" class="container">
                <div class="welcome-box">
                    <h1 class="font30 text-uppercase margin-bottom30">Bike Service Center Locator</h1>
                    <h2 class="font20 text-unbold text-white margin-bottom50">Find bike service center across 1000+ cities in India</h2>
                </div>
            </div>
        </header>
        <section>
            <div class="container">
                <div class="grid-12">
                    <div class="search-box content-box-shadow negative-50 text-center padding-top5 padding-bottom20">
                        <h2 class="section-heading">Search service centers</h2>
                        <div class="locator-search-container">
                            <div class="locator-search-brand form-control-box">
                                <select id="ddlMakes" class="form-control  chosen-select">
                                    <option value="0" >Select a brand</option>
                                    <% foreach(var make in objMakes) {%>
                                            <option maskingname="<%=make.MaskingName%>" value="<%=make.MakeId %>"><%=make.MakeName%> </option>
                                       <%} %>
                                </select>
                                <div class="clear"></div>
                                <span class="bwsprite error-icon errorIcon hide"></span>
                                <div class="bw-blackbg-tooltip errorText hide"></div>
                            </div>
                            <div class="locator-search-city form-control-box">
                                <select id="ddlCities" class="form-control  chosen-select">
                                    <option value="0" >Select a city</option>                               
                                </select>
                                <div class="clear"></div>
                                <span class="bwsprite error-icon errorIcon hide"></span>
                                <div class="bw-blackbg-tooltip errorText hide"></div>
                            </div>
                            <div class="clear"></div>
                            <input type="button" id="applyFiltersBtn" class="btn btn-orange btn-lg locator-search-btn margin-bottom5" value="Search" />
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <% if (objTopMakeList!=null){ %>
        <section>
            <div class="container section-bottom-margin">
                <div class="grid-12">
                    <h2 class="section-heading">Service center by brands</h2>
                    <div class="content-box-shadow padding-top20 collapsible-brand-content">
                        <div id="brand-type-container" class="brand-type-container">
                            <ul class="text-center">
                              <% foreach (var make in objTopMakeList)
                                 { %>
                                        <li>
                                            <a href="/<%=make.MaskingName %>-service-center-in-india/" title="<%=make.MakeName %> Service Center in India">
                                                <span class="brand-type">
                                                    <span class="lazy brandlogosprite brand-<%=make.MakeId%>"></span>
                                                </span>
                                                <span class="brand-type-title"><%=make.MakeName %></span>
                                            </a>
                                        </li>
                                  <%} %>
                            </ul>
                            <% if (objOtherMakeList!=null){ %>
                            <ul class="brand-style-moreBtn padding-top25 brandTypeMore hide margin-left5">
                             <% foreach (var make in objOtherMakeList){ %>
                                       <li>
                                            <a href="/<%=make.MaskingName %>-service-center-in-india/" title="<%=make.MakeName %> Service Center in India">
                                                <span class="brand-type">
                                                    <span class="lazy brandlogosprite brand-<%=make.MakeId%>"></span>
                                                </span>
                                                <span class="brand-type-title"><%=make.MakeName %></span>
                                            </a>
                                        </li>
                               <%} %>
                            </ul>
                        </div>
                        <div class="view-all-btn-container padding-bottom25">
                            <a href="javascript:void(0)" class="view-brandType btn view-all-target-btn rotate-arrow" rel="nofollow"><span class="btn-label">View more brands</span><span class="bwsprite teal-right"></span></a>
                        </div>
                        <%} %>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <%} %>
       <%if(ctrlBikeCare.FetchedRecordsCount>0) {%>
        <section>
            <BW:BikeCare runat="server" ID="ctrlBikeCare" />
        </section>
        <%} %>
        <%if (ctrlMostPopularBikes.FetchedRecordsCount + ctrlNewLaunchedBikes.FetchedRecordsCount + ctrlUpcomingBikes.FetchedRecordsCount>0)
          {%>
        <section>
            <div class="container section-bottom-margin">
                <!--  Discover bikes section code starts here -->
                <div class="grid-12">
                    <h2 class="section-heading">Most popular bikes</h2>
                    <div class="bw-tabs-panel newbike-discover-bike-container content-box-shadow">
                        <div class="bw-tabs bw-tabs-flex">
                            <ul>
                                <%if (ctrlMostPopularBikes.FetchedRecordsCount>0){ %>
                                <li class="active"  data-tabs="ctrlMostPopularBikes">
                                    <h3>Most Popular</h3>
                                </li>
                                <%} %>
                                <%if (ctrlNewLaunchedBikes.FetchedRecordsCount > 0){ %>
                                <li data-tabs="ctrlNewLaunchedBikes">
                                    <h3>New launches</h3>
                                </li>
                                <%} %>
                                 <%if (ctrlUpcomingBikes.FetchedRecordsCount>0)
                                   { %>
                                <li  data-tabs="ctrlUpcomingBikes">
                                    <h3>Upcoming</h3>
                                </li>
                                  <%} %>
                            </ul>
                        </div>
                        <div class="bw-tabs-data <%= (ctrlMostPopularBikes.FetchedRecordsCount > 0)?"":"hide" %>" id="ctrlMostPopularBikes">
                            <div class="jcarousel-wrapper inner-content-carousel carousel-height-360">
                                <div class="jcarousel">
                                    <ul>
                                        <BW:MostPopularBikes PageId="5" runat="server" ID="ctrlMostPopularBikes" />
                                        <!-- Most Popular Bikes Control-->
                                    </ul>
                                </div>
                                <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev"></a></span>
                                <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next"></a></span>
                            </div>
                            <div class="view-all-btn-container padding-top15 padding-bottom20">
                                <a href="/best-bikes-in-india/" class="btn view-all-target-btn" title="Popular Bikes in India">View all bikes<span class="bwsprite teal-right"></span></a>
                            </div>
                        </div>

                        <div class="bw-tabs-data hide <%= (ctrlNewLaunchedBikes.FetchedRecordsCount > 0)?"":"hide" %>" id="ctrlNewLaunchedBikes">
                            <BW:NewLaunchedBikes PageId="5" runat="server" ID="ctrlNewLaunchedBikes" />
                            <div class="view-all-btn-container padding-top15 padding-bottom20">
                                <a href="/new-bike-launches/" class="btn view-all-target-btn" title="New Bike Launches in India">View all launches<span class="bwsprite teal-right"></span></a>
                            </div>
                            <!-- New Launched Bikes Control-->
                        </div>

                        <div class="bw-tabs-data hide <%= (ctrlUpcomingBikes.FetchedRecordsCount > 0)?"":"hide" %>" id="ctrlUpcomingBikes">
                            <div class="jcarousel-wrapper inner-content-carousel">
                                <div class="jcarousel">
                                    <ul>
                                        <BW:UpcomingBikes runat="server" ID="ctrlUpcomingBikes" />
                                        <!-- Upcoming Bikes Control-->
                                    </ul>
                                </div>
                                <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev"></a></span>
                                <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next"></a></span>
                            </div>
                            <div class="view-all-btn-container padding-top15 padding-bottom20">
                                <a href="/upcoming-bikes/" class="btn view-all-target-btn" title="Upcoming Bikes in India">View all bikes<span class="bwsprite teal-right"></span></a>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <%} %>
     <section>
            <!--  Discover your bike code starts here -->
            <div class="container">
                <div class="grid-12">
                    <h2 class="section-heading">Find used bikes</h2>
                    <div class="bw-tabs-panel content-box-shadow">
                        <div class="bw-tabs bw-tabs-flex">
                            <ul>
                                <% if (ctrlusedBikeModel.FetchCount > 0)
                                   { %>
                                <li class="active" data-tabs="usedByModel">
                                    <h3>Model</h3>
                                </li>
                                <%} %>
                              
                                <%if (ctrlusedBikeInCities.objCitiesWithCount != null && ctrlusedBikeInCities.objCitiesWithCount.Count() > 0)
                                  { %>
                                <li  class="<%=ctrlusedBikeModel.FetchCount > 0?"":"active"%>" data-tabs="usedByCity">
                                    <h3>City</h3>
                                </li>
                                <%} %>
                                  <li  class="<%=((ctrlusedBikeModel.FetchCount>0) ||( ctrlusedBikeInCities.objCitiesWithCount != null && ctrlusedBikeInCities.objCitiesWithCount.Count() > 0))?"":"active"%>" data-tabs="usedByBudget">
                                    <h3>Budget</h3>
                                </li>
                            </ul>
                        </div>
                        
                        <% if (ctrlusedBikeModel.FetchCount > 0)
                           { %>
                        <div class="bw-tabs-data padding-bottom20" id="usedByModel">
                            <BW:usedBikeModel runat="server" ID="ctrlusedBikeModel" />
                        </div>
                        <%} %>
                        <%if (ctrlusedBikeInCities.objCitiesWithCount != null && ctrlusedBikeInCities.objCitiesWithCount.Count() > 0)
                          { %>

                        <section>
                            <div class="bw-tabs-data <%=ctrlusedBikeModel.FetchCount > 0?"hide":""%>" id="usedByCity">
                                <BW:usedBikeInCities runat="server" ID="ctrlusedBikeInCities" />
                            </div>

                        </section>
                        <%} %>

                        <div class="bw-tabs-data <%=((ctrlusedBikeModel.FetchCount>0) ||( ctrlusedBikeInCities.objCitiesWithCount != null && ctrlusedBikeInCities.objCitiesWithCount.Count() > 0))?"hide":""%> padding-bottom20" id="usedByBudget">
                            <ul class="elevated-card-list">
                                <li>
                                    <a href="/used/bikes-in-<%= String.IsNullOrEmpty(cityMaskingName) ? "india" : cityMaskingName %>/#budget=0+35000" rel="nofollow">
                                        <div class="table-middle">
                                            <div class="tab-icon-container">
                                                <span class="bwsprite budget-one"></span>
                                            </div>
                                            <span class="key-size-14">Upto</span><br />
                                            <span class="bwsprite inr-md"></span><span class="value-size-16">35,000</span>
                                        </div>
                                    </a>
                                </li>
                                <li>
                                    <a href="/used/bikes-in-<%= String.IsNullOrEmpty(cityMaskingName) ? "india" : cityMaskingName %>/#budget=35000+80000" rel="nofollow">
                                        <div class="table-middle">
                                            <div class="tab-icon-container">
                                                <span class="bwsprite budget-two"></span>
                                            </div>
                                            <span class="key-size-14">Between</span><br />
                                            <span class="bwsprite inr-md"></span><span class="value-size-16">35,000 -</span>
                                            <span class="bwsprite inr-md"></span><span class="value-size-16">80,000</span>
                                        </div>
                                    </a>
                                </li>
                                <li>
                                    <a href="/used/bikes-in-<%= String.IsNullOrEmpty(cityMaskingName) ? "india" : cityMaskingName %>/#budget=80000+200000" rel="nofollow">
                                        <div class="table-middle">
                                            <div class="tab-icon-container">
                                                <span class="bwsprite budget-three"></span>
                                            </div>
                                            <span class="key-size-14">Above</span><br />
                                            <span class="bwsprite inr-md"></span><span class="value-size-16">80,000</span>
                                        </div>
                                    </a>
                                </li>
                            </ul>

                            <div class="more-article-target view-all-btn-container padding-top10"> 
                            <a href="/used/bikes-in-india/" title="Second Hand Bikes in India" class="btn view-all-target-btn">View all used bikes<span class="bwsprite teal-right"></span></a>
                        </div>

                        </div>


                    </div>
                   
                </div>
                 <div class="clear"></div>
                </div>
        </section>
        <section>
            <div class="container section-bottom-margin margin-top15">
                <h2 class="section-heading">Bike Troubleshooting - FAQs</h2>
                <div class="grid-12">
                    <div class="content-box-shadow padding-bottom20">
                        <ul class="accordion-list">
                            <li>
	                            <div class="accordion-head">
		                            <p class="accordion-head-title">What to do if you have a puncture?</p>
		                            <span class="bwsprite arrow-sm-down"></span>
	                            </div>                        
	                            <div class="accordion-body">
		                            <p>There's two kinds of tyres available to the public, the tube type and the tubeless type. If you've got spoked rims, you've got a tyre with a tube. The puncture will leave you without air suddenly, and the best you can do is flag down a passing cab or rickshaw, go to the puncture repair shop and get the person to the motorcycle. Repeat the trip to the repair shop (this time with the wheel) and back again to the motorcycle. If that isn't possible, put the bike in first, engage the clutch and walk the motorcycle to the shop. Remember that you risk damaging your tyre this way.<br /><br />Tubeless tyres are a lot easier to deal with. The very thing that punctures the tyre also seals the hole, so the leak is far slower. You can ride the motorcycle, but be very careful. Ride it with too little air and you risk damaging the tyre beyond repair. If the nearest puncture repair shop isn't equipped with a tubeless puncture repair kit, make them fill a lot of air in the tyre and ride on. If you can't find a puncture repair shop, even a bicycle pump can help you fill air in the tyre.</p>
	                            </div>
                            </li>
                            <li>
	                            <div class="accordion-head">
		                            <p class="accordion-head-title">What to do if your battery is weak and you have no kick start lever?</p>
		                            <span class="bwsprite arrow-sm-down"></span>
	                            </div>                        
	                            <div class="accordion-body">
		                            <p>We've all been there at some point, and there's the obvious - jumper cables.<br /><br />Remember to take a jump from a battery that has a higher rating than yours, else you run the risk of two motorcycles that won't start after your attempts. The next obvious thing to do is to either remove the battery, get it charged and reinstall it, or replace it with a fully charged one. There are a few other things you can do if jumper cables aren't available, or you don't know how to remove your battery.<br /><br />Note: if it is a large motorcycle (say over 400cc) do not attempt anything you read beyond this. If, however, you have a small motorcycle with a carburettor, here's exactly what you need to do: stick it in second, pull the clutch in, push the bike and release the clutch. As soon as it catches, pull the clutch in. If you have a helping hand, it is far safer to have one person sit on the motorcycle while the other pushes. Another trick that you can use for small motorcycles is putting them on the main stand - the same rules apply. Stick it in second, leave the ignition on and just give the rear wheel torque by pulling it in the correct direction. If you give it a hard enough tug, the bike should start.<br /><br />There is one other condition under which a push-start will not achieve any results at all: if you have fuel injection on your motorcycle, turn the key over to the 'on' position and put your ear near the fuel tank. If you hear a noise, however weak, it means that the fuel pump is still working enough to send fuel to the engine, and you have a chance of the bike starting. Pull the fuses to the headlamp to keep it from taking any more juice away from the fuel pump and try the push start. If you turn your key to 'on' and hear nothing at all, then don't bother trying, your motorcycle won't start no matter how much you push it.<br /><br />Revving the motorcycle to the redline will not make the battery charge faster - anything beyond 3000 rpm is a waste of fuel, so go for a 20-30 minute cruise to make sure the battery gets charged enough to crank the engine should you stall for any reason.</p>
	                            </div>
                            </li>
                            <li>
	                            <div class="accordion-head">
		                            <p class="accordion-head-title">What to do if your clutch cable breaks?</p>
		                            <span class="bwsprite arrow-sm-down"></span>
	                            </div>                        
	                            <div class="accordion-body">
		                            <p>If you have a scooter, obviously this isn't a problem. However, this can be quite a big issue if you've got something that needs gears to be shifted manually.<br /><br />The best thing to do is to stick it in neutral and either push the motorcycle along or have someone tow you. If this isn't possible, though, technology and a little bit of looking ahead can help you get to help. There's something called 'synchromesh' that gearboxes have today, and that means that you can actually change gears without using the clutch lever. It will take a little bit of practice, though, especially while downshifting. Upshifts will be a lot smoother. The biggest problem will be coming to a halt and taking off from a halt. For this, the obvious solution will be to not do it at all, so you can either wait for a time when there won't be traffic or use a route with little to no traffic or stop signals.<br /><br />If it cannot be avoided, though, you'll have to slow down as much you can in first gear, and then try to put it into neutral while using the brakes to come to a complete halt. Starting it will be very tricky, because it will be almost impossible to get it going with just enough throttle to remain in control of the motorcycle. If you have a main stand, you can try putting it on the main stand, putting it in gear and then doing a running start with it.<br /><br />Remember - these are very risky manoeuvres, so please do not try them unless there is an emergency and you cannot afford to wait at all.</p>
	                            </div>
                            </li>
                        </ul>

                        <a href="/bike-troubleshooting/" target="_blank" title="Bike Troubleshooting- FAQs" class="font14 margin-left20">Read all FAQs<span class="bwsprite blue-right-arrow-icon"></span></a>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : "" %>/src/frameworks.js?<%=staticFileVersion %>"></script>
        <!-- #include file="/includes/footerBW.aspx" -->
        <link href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
        
        <!-- #include file="/includes/footerscript.aspx" -->
        <script type="text/javascript"> 
            var $ddlCities = $("#ddlCities"), $ddlMakes = $("#ddlMakes");
            var bikeCityId = $("#ddlCities").val();
            var selCityId = '<%= (cityId > 0)?cityId:0%>';
            lscache.flushExpired();
            var key = "ServiceCenterCitiesByMake_";
            lscache.setBucket('SCPage');  

            $(function () {
                
                $('select').prop('selectedIndex', 0);

                $("#applyFiltersBtn").click(function () {
                    ddlmakemasking = $("#ddlMakes option:selected").attr("maskingName");
                    ddlcityId = $("#ddlCities option:selected").val();
                    ddlmakeId = $("#ddlMakes option:selected").val();
                    if (!isNaN(ddlmakeId) && ddlmakeId != "0") {
                        if (!isNaN(ddlcityId) && ddlcityId != "0") {
                            ddlcityMasking = $("#ddlCities option:selected").attr("maskingName");
                            bwcache.remove("userchangedlocation", true);
                            window.location.href = "/" + ddlmakemasking + "-service-center-in-" + ddlcityMasking+ "/";
                        }
                        else {
                            toggleErrorMsg($ddlCities, true, "Choose a city");
                        }
                    }
                    else {
                        toggleErrorMsg($ddlMakes, true, "Choose a brand");
                    }
                });        

                $ddlMakes.change(function () {
                    toggleErrorMsg($ddlMakes, false);
                    selMakeId = $ddlMakes.val();
                    $ddlCities.empty();
                    if (!isNaN(selMakeId) && selMakeId != "0") {
                        if (!checkCacheCityAreas(selMakeId)) {
                            $.ajax({
                                type: "GET",
                                url: "/api/servicecenter/cities/make/" + selMakeId + "/",
                                contentType: "application/json",
                                dataType: 'json',
                                success: function (data) {
                                    lscache.set(key + selMakeId, data, 30);
                                    setOptions(data);
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

                $ddlCities.change(function () {
                    toggleErrorMsg($ddlCities, false);
                });

                if ($("#ddlCities option").length < 2) {
                    $ddlCities.empty();
                    $ddlCities.trigger('chosen:updated');
                    $("#ddlCities_chosen .chosen-single span").text("Select City");
                }
            });

            function checkCacheCityAreas(cityId) {
                bKey = key + cityId;
                if (lscache.get(bKey)) return true;
                else return false;
            }

            function setOptions(optList) {
                if (optList != null) {
                    $ddlCities.append($('<option>').text(" Select City ").attr({ 'value': "0" }));
                    $.each(optList, function (i, value) {
                        $ddlCities.append($('<option>').text(value.cityName).attr({ 'value': value.cityId, 'maskingName': value.cityMaskingName }));
                    });
                }
                else {
                  
                    $("#ddlCities_chosen .chosen-single.chosen-default span").text("No cities available");
                }
                if (optList) {
                    var selectedElement = $.grep(optList, function (element, index) {
                        return element.cityId == selCityId;
                    });
                    if (selectedElement.length > 0) {
                        $('#ddlCities').val(selectedElement[0].cityId);
                       
                    }
                }
                $ddlCities.trigger('chosen:updated');
                }

            $ddlCities.chosen({ no_results_text: "No matches found!!" });
            $ddlMakes.chosen({ no_results_text: "No matches found!!" });
            $('div.chosen-container').attr('style', 'width:100%;border:0');

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

        </script>
        
        <!-- #include file="/includes/fontBW.aspx" -->
    </form>
</body>
</html>
