<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Service.Default" EnableViewState="false" %>
<%@ Register Src="~/controls/NewLaunchedBikes_new.ascx" TagName="NewLaunchedBikes" TagPrefix="BW" %>
<%@ Register Src="~/controls/UpcomingBikes_new.ascx" TagName="UpcomingBikes" TagPrefix="BW" %>

<%@ Import Namespace="Bikewale.Common" %>
<!doctype html>
<html>
<head>
    <%
        title = "New Bike Dealer Showrooms in India | Locate Authorized Bike Showrooms - BikeWale";
        keywords = "new bike dealers, new bike showrooms, bike dealers, bike showrooms, showrooms, dealerships";
        description = "Locate new bike showrooms and authorized bike dealers in India. Find new bike dealer information for more than 200 cities in India. ";
        canonical = "http://www.bikewale.com/dealer-showroom-locator/";
        alternate = "http://www.bikewale.com/m/dealer-showroom-locator/";
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
<body class="bg-light-grey">
    <form runat="server">
        <!-- #include file="/includes/headBW.aspx" -->
        <header class="service-center-banner">
            <div id="service-center-box" class="container">
                <div class="welcome-box">
                    <h1 class="font30 text-uppercase margin-bottom30">Bike Service Center Locator</h1>
                    <h2 class="font20 text-unbold text-white margin-bottom50">Find bike service center across 200+ cities in India</h2>
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
                                    <% foreach(var make in makes) {%>
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

        <section>
            <div class="container section-bottom-margin">
                <div class="grid-12">
                    <h2 class="section-heading">Service center by brands</h2>
                    <div class="content-box-shadow padding-top20">
                        <div class="brand-type-container">
                            <ul class="text-center">
                              <% foreach (var make in TopMakeList){ %>
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
                            <div class="brand-bottom-border border-solid-top hide"></div>
                            <ul class="brand-style-moreBtn padding-top25 brandTypeMore hide margin-left5">
                             <% foreach (var make in OtherMakeList){ %>
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
                        <div class="view-brandType text-center padding-bottom25">
                            <a href="javascript:void(0)" id="view-brandType" class="view-more-btn font16" rel="nofollow">View <span>more</span> brands</a>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section>
            <div class="container section-bottom-margin">
                <h2 class="section-heading">Bike Care - Maintenance Tips</h2>
                <div class="grid-12">
                    <div class="content-box-shadow content-inner-block-20">
                        <ul class="article-list">
                            <li>
                                <div class="grid-4 alpha">
                                    <div class="model-preview-image-container">
                                        <a href="" title="TVS Apache RTR 200 4V Carb vs Bajaj Pulsar AS200 vs Honda Hornet 160R: Comparison Test">
                                            <img class="lazy" data-original="https://imgd1.aeplcdn.com//310x174//bw/ec/23488/Side-73148.jpg?wm=0" alt="TVS Apache RTR 200 4V Carb vs Bajaj Pulsar AS200 vs Honda Hornet 160R: Comparison Test" src="">
                                        </a>
                                    </div>
                                </div>
                                <div class="grid-8 padding-left5 omega">
                                    <a href="" class="article-target-link">TVS Apache RTR 200 4V Carb vs Bajaj Pulsar AS200 vs Honda Hornet 160R: Comparison Test</a>
                                    <div class="article-stats-left-grid margin-bottom10">
                                        <span class="bwsprite calender-grey-sm-icon"></span>
                                        <span class="article-stats-content">Nov 15, 2016</span>
                                    </div>
                                    <div class="article-stats-right-grid margin-bottom10">
                                        <span class="bwsprite author-grey-sm-icon"></span>
                                        <span class="article-stats-content">Sagar Bhanushali</span>
                                    </div>
                                    <p class="font14 line-height17">Jagan Kumar from TVS and RACR’s Rajini Krishnan shared wins in the Indian Motorcycle Racing Championship’s (IMRC) Super Sport category at the Madras Motor Race Track. Ami Van Poederooijen won overall in the combined Pro Stock and Super Sport category...</p>
                                </div>
                                <div class="clear"></div>
                            </li>

                            <li>
                                <div class="grid-4 alpha">
                                    <div class="model-preview-image-container">
                                        <a href="" title="TVS Apache RTR 200 4V Carb vs Bajaj Pulsar AS200 vs Honda Hornet 160R: Comparison Test">
                                            <img class="lazy" data-original="https://imgd1.aeplcdn.com//310x174//bw/ec/21085/Honda-CB-Shine-Side-61910.jpg?wm=0&t=181939320&t=181939320" alt="TVS Apache RTR 200 4V Carb vs Bajaj Pulsar AS200 vs Honda Hornet 160R: Comparison Test" src="">
                                        </a>
                                    </div>
                                </div>
                                <div class="grid-8 padding-left5 omega">
                                    <a href="" class="article-target-link">Bajaj Avenger 220 Cruise vs Royal Enfield Thunderbird 350 : Comparison Test</a>
                                    <div class="article-stats-left-grid margin-bottom10">
                                        <span class="bwsprite calender-grey-sm-icon"></span>
                                        <span class="article-stats-content">Nov 15, 2016</span>
                                    </div>
                                    <div class="article-stats-right-grid margin-bottom10">
                                        <span class="bwsprite author-grey-sm-icon"></span>
                                        <span class="article-stats-content">Sagar Bhanushali</span>
                                    </div>
                                    <p class="font14 line-height17">Jagan Kumar from TVS and RACR’s Rajini Krishnan shared wins in the Indian Motorcycle Racing Championship’s (IMRC) Super Sport category...</p>
                                </div>
                                <div class="clear"></div>
                            </li>

                            <li>
                                <div class="grid-4 alpha">
                                    <div class="model-preview-image-container">
                                        <a href="" title="TVS Apache RTR 200 4V Carb vs Bajaj Pulsar AS200 vs Honda Hornet 160R: Comparison Test">
                                            <img class="lazy" data-original="https://imgd1.aeplcdn.com//310x174//bw/ec/23488/Side-73148.jpg?wm=0" alt="TVS Apache RTR 200 4V Carb vs Bajaj Pulsar AS200 vs Honda Hornet 160R: Comparison Test" src="">
                                        </a>
                                    </div>
                                </div>
                                <div class="grid-8 padding-left5 omega">
                                    <a href="" class="article-target-link">TVS Apache RTR 200 4V Carb vs Bajaj Pulsar AS200 vs Honda Hornet 160R: Comparison Test</a>
                                    <div class="article-stats-left-grid margin-bottom10">
                                        <span class="bwsprite calender-grey-sm-icon"></span>
                                        <span class="article-stats-content">Nov 15, 2016</span>
                                    </div>
                                    <div class="article-stats-right-grid margin-bottom10">
                                        <span class="bwsprite author-grey-sm-icon"></span>
                                        <span class="article-stats-content">Sagar Bhanushali</span>
                                    </div>
                                    <p class="font14 line-height17">Indian Motorcycle Racing Championship’s (IMRC) Super Sport category at the Madras Motor Race Track. Ami Van Poederooijen won overall in the combined Pro Stock and Super Sport category...</p>
                                </div>
                                <div class="clear"></div>
                            </li>
                        </ul>

                        <a href="" class="font14">Read all bike maintenance tips<span class="bwsprite blue-right-arrow-icon"></span></a>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section>
            <div class="container section-bottom-margin">
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
                                    <p>We've all been there at some point, and there's the obvious - jumper cables.<br /><br />Remember to take a jump from a battery that has a higher rating than yours, else you run the risk of two motorcycles that won’t start after your attempts. The next obvious thing to do is to either remove the battery, get it charged and reinstall it, or replace it with a fully charged one. There are a few other things you can do if jumper cables aren’t available, or you don’t know how to remove your battery.<br /><br />Note: if it is a large motorcycle (say over 400cc) do not attempt anything you read beyond this. If, however, you have a small motorcycle with a carburettor, here's exactly what you need to do: stick it in second, pull the clutch in, push the bike and release the clutch. As soon as it catches, pull the clutch in. If you have a helping hand, it is far safer to have one person sit on the motorcycle while the other pushes. Another trick that you can use for small motorcycles is putting them on the main stand – the same rules apply. Stick it in second, leave the ignition on and just give the rear wheel torque by pulling it in the correct direction. If you give it a hard enough tug, the bike should start.<br /><br />There is one other condition under which a push-start will not achieve any results at all: if you have fuel injection on your motorcycle, turn the key over to the ‘on’ position and put your ear near the fuel tank. If you hear a noise, however weak, it means that the fuel pump is still working enough to send fuel to the engine, and you have a chance of the bike starting. Pull the fuses to the headlamp to keep it from taking any more juice away from the fuel pump and try the push start. If you turn your key to ‘on’ and hear nothing at all, then don’t bother trying, your motorcycle won’t start no matter how much you push it.<br /><br />Revving the motorcycle to the redline will not make the battery charge faster – anything beyond 3000 rpm is a waste of fuel, so go for a 20-30 minute cruise to make sure the battery gets charged enough to crank the engine should you stall for any reason.</p>
                                </div>
                            </li>
                            <li>
                                <div class="accordion-head">
                                    <p class="accordion-head-title">What to do if your clutch cable breaks?</p>
                                    <span class="bwsprite arrow-sm-down"></span>
                                </div>                        
                                <div class="accordion-body">
                                    <p>If you have a scooter, obviously this isn’t a problem. However, this can be quite a big issue if you’ve got something that needs gears to be shifted manually.<br /><br />The best thing to do is to stick it in neutral and either push the motorcycle along or have someone tow you. If this isn’t possible, though, technology and a little bit of looking ahead can help you get to help. There’s something called ‘synchromesh’ that gearboxes have today, and that means that you can actually change gears without using the clutch lever. It will take a little bit of practice, though, especially while downshifting. Upshifts will be a lot smoother. The biggest problem will be coming to a halt and taking off from a halt. For this, the obvious solution will be to not do it at all, so you can either wait for a time when there won’t be traffic or use a route with little to no traffic or stop signals.<br /><br />If it cannot be avoided, though, you’ll have to slow down as much you can in first gear, and then try to put it into neutral while using the brakes to come to a complete halt. Starting it will be very tricky, because it will be almost impossible to get it going with just enough throttle to remain in control of the motorcycle. If you have a main stand, you can try putting it on the main stand, putting it in gear and then doing a running start with it.<br /><br />Remember – these are very risky manoeuvres, so please do not try them unless there is an emergency and you cannot afford to wait at all.</p>
                                </div>
                            </li>
                        </ul>

                        <a href="" class="font14 margin-left20">Read all FAQs<span class="bwsprite blue-right-arrow-icon"></span></a>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/src/frameworks.js?<%=staticFileVersion %>"></script>
        <!-- #include file="/includes/footerBW.aspx" -->
        <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%= staticUrl != string.Empty ? "http://st2.aeplcdn.com" + staticUrl : string.Empty %>/src/common.min.js?<%= staticFileVersion %>"></script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />
        <script type="text/javascript"> 
            var $ddlCities = $("#ddlCities"), $ddlMakes = $("#ddlMakes");
            var bikeCityId = $("#ddlCities").val();
            var selCityId = '<%= (cityId > 0)?cityId:0%>';
            lscache.flushExpired();
            var key = "ServiceCenterCitiesByMake_";
            lscache.setBucket('DLPage');  

            $(function () {
                $(window).on("scroll", function () {
                    if ($(window).scrollTop() > 40)
                        $('#header').removeClass("header-landing").addClass("header-fixed");
                    else
                        $('#header').removeClass("header-fixed").addClass("header-landing");
                });

                $("a.view-more-btn").click(function (e) {
                    var moreBrandList = $("ul.brand-style-moreBtn"),
                        moreText = $(this).find("span"),
                        borderDivider = $(".brand-bottom-border");                    
                    moreBrandList.slideToggle();
                    moreText.text(moreText.text() === "more" ? "less" : "more");
                    borderDivider.slideToggle();
                });


                $('select').prop('selectedIndex', 0);

                $("#applyFiltersBtn").click(function () {
                    ddlmakemasking = $("#ddlMakes option:selected").attr("maskingName");
                    ddlcityId = $("#ddlCities option:selected").val();
                    ddlmakeId = $("#ddlMakes option:selected").val();
                    if (!isNaN(ddlmakeId) && ddlmakeId != "0") {
                        if (!isNaN(ddlcityId) && ddlcityId != "0") {
                            ddlcityMasking = $("#ddlCities option:selected").attr("maskingName");
                            //window.location.href = "/new/" + ddlmakemasking + "-dealers/" + ddlcityId + "-" + ddlcityMasking + ".html";
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
        <!--[if lt IE 9]>
            <script src="/src/html5.js"></script>
        <![endif]-->

    </form>
</body>
</html>
