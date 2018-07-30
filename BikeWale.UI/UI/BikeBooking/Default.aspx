<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.BikeBooking.Default" %>  
<!DOCTYPE html>
<html>
<head>    
    <%
        
        title = "Book Bikes, Scooters in India and Avail Great Benefits - BikeWale";
        keywords = "book bikes, book scooters, buy bikes, buy scooters, bikes prices, avail offers, avail discounts, instant bike on-road price";
        description = "BikeWale - India's favourite bike portal. Book your bikes, scooters and avail exciting offers and benefits exclusively on BikeWale.";
        canonical = "https://www.bikewale.com/bikebooking/";
        isHeaderFix = false;
        isAd970x90Shown = false;
        isTransparentHeader = true;
        isAd300x250Shown = false;
        isAd300x250BTFShown = false;
        isAd970x90BottomShown = false;
         
    %>
    <!-- #include file="/includes/headscript.aspx" -->
    <link href="<%= staticUrl  %>/css/bookinglanding.css?<%= staticFileVersion%>" rel="stylesheet" type="text/css" />
</head>
<body class="bg-light-grey" data-contestslug="true">
    <form id="form1" runat="server">
        <!-- #include file="/includes/headBW.aspx" -->
        <script type="text/javascript">document.getElementById("header").children[1].innerHTML = "";</script>
        <header class="booking-landing-banner">
            <div class="container">
                <div class="welcome-box">
                    <h1 class="font30 text-uppercase margin-bottom30">Book your dream bike</h1>
                    <p class="font20">Online booking is now available in</p>
                    <p class="font22 text-bold margin-bottom45">Mumbai, Pune and Bangalore</p>
                    <div class="booking-landing-search-container">
                        <div class="booking-search-city form-control-box">
                            <asp:DropDownList ID="bookingCitiesList" class="form-control chosen-select" runat="server" />
                        </div>
                        <div class="booking-search-area form-control-box">
                            <asp:DropDownList ID="bookingAreasList" class="form-control chosen-select" runat="server" />
                        </div>
                        <input type="button" class="btn btn-orange font16 btn-lg leftfloat booking-landing-search-btn rounded-corner-no-left" value="Search" />
                        <div class="clear"></div>
                    </div>
                </div>
            </div>
        </header>

        <section>
            <div id="onlineBenefitsWrapper" class="container margin-bottom40">
                <div class="grid-12 text-center font18">
                    <h2 class="text-bold text-center margin-top40 margin-bottom30 font28">Benefits of booking online</h2>
                    <div class="grid-3">
                        <div class="benefit-icon-outer-container rounded-corner50">
                            <div class="benefit-icon-inner-container rounded-corner50">
                                <span class="benefits-icon-span booking-landing-sprite benefit-offers-icon margin-top25"></span>
                            </div>
                        </div>
                        <div class="margin-top20">
                            Exclusive<br />
                            offers
                        </div>
                    </div>
                    <div class="grid-3">
                        <div class="benefit-icon-outer-container rounded-corner50">
                            <div class="benefit-icon-inner-container rounded-corner50">
                                <span class="benefits-icon-span booking-landing-sprite benefit-dealer-icon margin-top30"></span>
                            </div>
                        </div>
                        <div class="margin-top20">
                            Save on<br />
                            dealer visits
                        </div>
                    </div>
                    <div class="grid-3">
                        <div class="benefit-icon-outer-container rounded-corner50">
                            <div class="benefit-icon-inner-container rounded-corner50">
                                <span class="benefits-icon-span booking-landing-sprite benefit-assistance-icon margin-top25"></span>
                            </div>
                        </div>
                        <div class="margin-top20">
                            Complete<br />
                            buying assistance
                        </div>
                    </div>
                    <div class="grid-3">
                        <div class="benefit-icon-outer-container rounded-corner50">
                            <div class="benefit-icon-inner-container rounded-corner50">
                                <span class="benefits-icon-span booking-landing-sprite benefit-cancellation-icon margin-top25"></span>
                            </div>
                        </div>
                        <div class="margin-top20">
                            Easy<br />
                            cancellation
                        </div>
                    </div>
                    <div class="clear"></div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section id="bookingWorksWrapper">
            <div class="container">
                <div class="grid-12 text-center text-white">
                    <h2 class="text-white text-bold margin-top40 margin-bottom30 font28">How it works?</h2>
                    <div class="grid-4">
                        <span class="booking-landing-sprite work-payment-icon margin-bottom15"></span>
                        <p class="font16">
                            Book your bike by paying<br />
                            booking amount
                        </p>
                        <span class="booking-landing-sprite work-arrow-icon"></span>
                    </div>
                    <div class="grid-4">
                        <span class="booking-landing-sprite work-document-icon margin-bottom15"></span>
                        <p class="font16">
                            Provide documents and<br />
                            payment at dealership
                        </p>
                        <span class="booking-landing-sprite work-arrow-icon"></span>
                    </div>
                    <div class="grid-4">
                        <span class="booking-landing-sprite work-delivery-icon margin-bottom15"></span>
                        <p class="font16">
                            Get your bike delivered and<br />
                            avail offers
                        </p>
                    </div>
                    <div class="clear"></div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section>
            <div id="faqsWraper" class="container margin-bottom30">
                <div class="grid-12">
                    <h2 class="text-bold text-center margin-top40 margin-bottom20 font28">FAQs</h2>
                    <div class="grid-4 content-box-shadow content-inner-block-20">
                        <p class="font16 margin-bottom20">How can I book a bike on BikeWale?</p>
                        <p class="font14 text-light-grey">To book a bike, you have to pay a fixed booking amount online mentioned against the vehicle of your interest. This amount will be adjusted...<a href="/faq.aspx#2" target="_blank" rel="noopener">read more</a></p>
                    </div>
                    <div class="grid-4 content-box-shadow content-inner-block-20 margin-left20 margin-right20">
                        <p class="font16 margin-bottom20">Where do I have to pay the balance amount? How much will it be?</p>
                        <p class="font14 text-light-grey">You will pay the balance amount directly to the assigned dealership during your visit to the showroom. The...<a href="/faq.aspx#14" target="_blank" rel="noopener">read more</a></p>
                    </div>
                    <div class="grid-4 content-box-shadow content-inner-block-20">
                        <p class="font16 margin-bottom20">How will I get the benefits of the offers?</p>
                        <p class="font14 text-light-grey">Depending upon the offer, you will get the benefit of some offers directly at the dealership, while taking...<a href="/faq.aspx#16" target="_blank" rel="noopener">read more</a></p>
                    </div>
                    <div class="clear"></div>
                    <div class="margin-top20 font14 text-center">
                        <p>We’re here to help. Read our <a href="/faq.aspx" target="_blank" rel="noopener" class="text-blue">FAQs</a> or <a href="mailto:contact@bikewale.com" target="_blank" rel="noopener" >Email Us</a> <%--or call us on <span class="text-bold text-default">(022) 6739 8888</span>--%></p>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <script src="<%= staticUrl  %>/src/lscache.min.js?<%= staticFileVersion%>"></script>

        <script>
            $(window).on("scroll", function () {
                if ($(window).scrollTop() > 40)
                    $('#header').removeClass("header-landing").addClass("header-fixed");
                else
                    $('#header').removeClass("header-fixed").addClass("header-landing");
            });

            var $ddlCities = $("#bookingCitiesList"), $ddlAreas = $("#bookingAreasList");
            var key = "bCity_";
            var gaObj = { 'id': '<%= (int)Bikewale.Entities.Pages.GAPages.Booking_Landing%>', 'name': '<%= Bikewale.Entities.Pages.GAPages.Booking_Landing%>' };
            lscache.setBucket('BLPage');
            $(function () {

                var selCityId = '<%= (cityId > 0)?cityId:0%>';
                var selAreaId = '<%= (areaId > 0)?areaId:0%>';

                $ddlCities.change(function () {
                    selCityId = $ddlCities.val();
                    $ddlAreas.empty();
                    selAreaId = "0";
                    if (!isNaN(selCityId) && selCityId !="0") {
                        if (!checkCacheCityAreas(selCityId)) {
                            $.ajax({
                                type: "GET",
                                url: "/api/BBAreaList/?cityId=" + selCityId,
                                dataType: 'json',
                                beforeSend: function () {
                                    return;
                                },
                                success: function (data) {                                    
                                    lscache.set(key + selCityId, data.areas, 30);
                                    setOptions(data.areas);
                                },
                                complete: function (xhr) {
                                    if (xhr.status == 404 || xhr.status == 204) {
                                        lscache.set(key + selCityId, null, 30);
                                        setOptions(null);
                                    }
                                }
                            });
                        }
                        else {
                            data = lscache.get(key + selCityId.toString());
                            setOptions(data);
                        }

                    }
                    else {
                        setOptions(null);
                    }
                });

                $ddlAreas.change(function(){
                    if (!isNaN(selCityId) && selCityId != "0")
                    {
                        selAreaId  = $ddlAreas.find("option:selected").val();
                    }

                });

                $("input[type='button'].booking-landing-search-btn").click(function () {
                    if (!isNaN(selCityId) && selCityId !="0") {
                        if (!isNaN(selAreaId) && selAreaId !="0") {
                            var CookieValue = selCityId + "_" + $ddlCities.find("option:selected").text() + '_' + selAreaId + "_" + $ddlAreas.find("option:selected").text();
                            SetCookieInDays("location", CookieValue, 365);
                            window.location.href = "/bikebooking/bookinglisting.aspx"
                        }
                        else {
                            alert("Please select area !");
                        }
                    }
                    else {
                        alert("Please Select City !")
                    }
                });

            });

            function checkCacheCityAreas(cityId) {
                bKey = key + cityId;
                if (lscache.get(bKey)) return true;
                else return false;
            }

            function setOptions(optList) {
                if (optList != null)
                {
                    $ddlAreas.append($('<option>').text(" Select Area ").attr({ 'value': "0" }));
                    $.each(optList, function (i, value) {
                        $ddlAreas.append($('<option>').text(value.areaName).attr('value', value.areaId));
                    });
                }
                                
                $ddlAreas.trigger('chosen:updated');
                $("#bookingAreasList_chosen .chosen-single.chosen-default span").text("No Areas available");
            }


        </script>

        <!-- #include file="/includes/footerBW.aspx" -->
        
        <script type="text/javascript">

           
            $ddlCities.chosen({ no_results_text: "No matches found!!" });
            $ddlAreas.chosen({ no_results_text: "No matches found!!" });
            $('.chosen-container').attr('style', 'width:100%;');
            $("#bookingAreasList_chosen .chosen-single.chosen-default span").text("Please Select City");
            
            $(document).ready(function(){
                var testimonialSlider = 1;
                $('.jcarousel').jcarousel({ wrap: 'circular' }).jcarouselAutoscroll({ interval: 7000, target: '+=1', autostart: true });
               


            })
        </script>
        <!-- #include file="/includes/footerscript.aspx" -->
    </form>
</body>
</html>

