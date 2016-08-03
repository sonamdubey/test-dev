<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.New.DealerLocator.LocateNewBikeDealers" EnableViewState="false" %>

<%@ Import Namespace="Bikewale.Common" %>
<!doctype html>
<html>
<head>
    <%
        title = "New Bike Dealers in India - Locate Authorized Showrooms - BikeWale";
        keywords = "new bike dealers, new bike showrooms, bike dealers, bike showrooms, showrooms, dealerships, price quote";
        description = "Locate New bike dealers and authorized bike showrooms in India. Find new bike dealer information for more than 200 cities. Authorized company showroom information includes full address, phone numbers, email address, pin code etc.";
        canonical = "http://www.bikewale.com/new/locate-dealers/";
        alternate = "http://www.bikewale.com/m/new/locate-dealers/";
        //AdId = "1395986297721";
        //AdPath = "/1017752/BikeWale_New_";
        isHeaderFix = false;
        isAd970x90Shown = false;
        isTransparentHeader = true;
    %>
    <!-- #include file="/includes/headscript.aspx" -->
    <style type="text/css">
        .dealerlocator-banner{background:#393a3e url(http://imgd1.aeplcdn.com/0x0/bw/static/landing-banners/d/dealer-locator-banner.jpg?v1=31Mar2016) no-repeat left top;height:489px;padding-top:1px}.locator-search-container{margin:0 auto;width:600px}.locator-search-container .form-control{padding:8px;border-radius:0}.locator-search-brand,.locator-search-city{width:233px;height:40px;float:left}.locator-search-brand select,.locator-search-city select{width:233px;height:38px;color:#555;border:none}.locator-search-city .chosen-container{border-left:1px solid #ccc!important}.locator-search-brand .chosen-container,.locator-search-city .chosen-container{border:0;border-radius:0}.locator-search-brand .chosen-container:first-child,.locator-search-city .chosen-container:first-child{border:0;border-radius:2px!important}.locator-search-btn{width:133px}.locator-search-btn.btn-lg{padding:7px 20px;padding:8px 20px 7px\9}.brand-type-container li{display:inline-block;*display:inline;*zoom:1;vertical-align:top;width:180px;height:85px;margin:0 5px 30px;text-align:center;font-size:14px;-moz-border-radius:2px;-webkit-border-radius:2px;-o-border-radius:2px;-ms-border-radius:2px;border-radius:2px}.brand-type{width:180px;height:50px;display:block;margin:0 auto}.brand-type-title{margin-top:10px;display:block}.brand-type-container a{text-decoration:none;color:#1a1a1a;display:inline-block}.brand-type-container li:hover span.brand-type-title{font-weight:700}.brand-bottom-border{overflow:hidden}.brandlogosprite { background:url('http://imgd3.aeplcdn.com/0x0/bw/static/sprites/d/brand-type-sprite.png?v=19May2016') no-repeat; display: inline-block; }.brand-aprilia, .brand-bajaj, .brand-benelli, .brand-bmw, .brand-ducati, .brand-escorts, .brand-harleydavidson, .brand-hero, .brand-heroelectric, .brand-herohonda, .brand-honda, .brand-husqvarna, .brand-hyosung, .brand-indian, .brand-kawasaki, .brand-kinetic, .brand-ktm, .brand-lml, .brand-mahindra, .brand-motoguzzi, .brand-mv-agusta, .brand-piaggio, .brand-royalenfield, .brand-suzuki, .brand-triumph, .brand-tvs, .brand-um, .brand-vespa, .brand-victory, .brand-yamaha, .brand-yo, .brand-eider { height:50px; }.brand-aprilia { width: 87px; background-position: 0 0; }.brand-honda { width: 56px; background-position: -96px 0; }.brand-bajaj { width: 88px; background-position: -162px 0; }.brand-hyosung { width: 100px; background-position: -260px 0; }.brand-suzuki { width: 67px; background-position: -370px 0; }.brand-benelli { width: 125px; background-position: -447px 0; }.brand-indian { width: 122px; background-position: -582px 0; }.brand-triumph { width: 121px; background-position: -714px 0; }.brand-bmw { width: 44px; background-position: -845px 0; }.brand-kawasaki { width: 86px; background-position: -899px 0; }.brand-tvs { width: 118px; background-position: -995px 0; }.brand-ducati { width: 43px; background-position: -1123px 0; }.brand-ktm { width: 99px; background-position: -1176px 0; }.brand-vespa { width: 117px; background-position: -1285px 0; }.brand-harleydavidson { width: 59px; background-position: -1412px 0; }.brand-lml { width: 122px; background-position: -1481px 0; }.brand-yamaha { width: 122px; background-position: -1613px 0; }.brand-hero { width: 63px; background-position: -1745px 0; }.brand-mahindra { width: 102px; background-position: -1818px 0; }.brand-yo { width: 127px; background-position: -1930px 0; }.brand-heroelectric { width: 89px; background-position: -2067px 0; }.brand-motoguzzi { width: 82px; background-position: -2166px 0; }.brand-royalenfield { width: 121px; background-position: -2258px 0; }.brand-mv-agusta { width:63px; background-position: -2389px 0; }.brand-um { width:64px; background-position:-2461px 0; }.brand-eider { width:71px; background-position:-2535px 0; }
        @media only screen and (max-width:1024px) { .brand-type-container li, .brand-type{width:170px;} }
    </style>
</head>
<body class="bg-white">
    <form runat="server">
        <!-- #include file="/includes/headBW.aspx" -->
        <header class="dealerlocator-banner">
            <div class="container">
                <div class="welcome-box">
                    <h1 class="font30 text-uppercase margin-bottom30">Dealer locator</h1>
                    <p class="font20 margin-bottom50">Locate dealers near you</p>
                    <div class="locator-search-container">
                        <div class="locator-search-brand form-control-box">
                            <select id="ddlMakes" class="form-control  chosen-select">
                                <option value="0" >Select a brand</option>
                                <asp:Repeater ID="rptMakes" runat="server">
                                    <ItemTemplate>
                                        <option maskingname="<%# DataBinder.Eval(Container.DataItem,"MaskingName") %>" value="<%# DataBinder.Eval(Container.DataItem,"MakeId") %>"><%# DataBinder.Eval(Container.DataItem,"MakeName") %> </option>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </select>
                            <div class="clear"></div>
                            <span class="bwsprite error-icon errorIcon hide"></span>
                            <div class="bw-blackbg-tooltip errorText hide"></div>
                        </div>
                        <div class="locator-search-city form-control-box">
                            <select id="ddlCities" class="form-control  chosen-select">
                                <option value="0" >Select a city</option>
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
                        <input type="button" id="applyFiltersBtn" class="btn btn-orange font16 btn-lg leftfloat locator-search-btn rounded-corner-no-left" value="Search" />
                        <div class="clear"></div>
                    </div>
                </div>
            </div>
        </header>

        <section class="container">
            <div class="grid-12">
                <h2 class="text-bold text-center margin-top40 margin-bottom30 font28">Locate dealers by brand</h2>
                <div class="brand-type-container">
                    <ul class="text-center">
                        <asp:Repeater ID="rptPopularBrands" runat="server">
                            <ItemTemplate>
                                <li>
                                    <a href="/new/<%# DataBinder.Eval(Container.DataItem, "MaskingName") %>-dealers/">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-<%# DataBinder.Eval(Container.DataItem, "MaskingName") %>"></span>
                                        </span>
                                        <span class="brand-type-title"><%# DataBinder.Eval(Container.DataItem, "MakeName") %></span>
                                    </a>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                    <div class="brand-bottom-border border-solid-top hide"></div>
                    <ul class="brand-style-moreBtn padding-top25 brandTypeMore hide margin-left5">
                        <asp:Repeater ID="rptOtherBrands" runat="server">
                            <ItemTemplate>
                                <li>
                                    <a href="/new/<%# DataBinder.Eval(Container.DataItem, "MaskingName") %>-dealers/">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-<%# DataBinder.Eval(Container.DataItem, "MaskingName") %>"></span>
                                        </span>
                                        <span class="brand-type-title"><%# DataBinder.Eval(Container.DataItem, "MakeName") %></span>
                                    </a>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                </div>
                <div class="view-brandType text-center padding-bottom30">
                    <a href="javascript:void(0)" id="view-brandType" class="view-more-btn font16" rel="nofollow">View <span>more</span> brands</a>
                </div>
            </div>
            <div class="clear"></div>
        </section>

        <!-- #include file="/includes/footerBW.aspx" -->
        <!-- #include file="/includes/footerscript.aspx" -->

        <script type="text/javascript"> 
            var $ddlCities = $("#ddlCities"), $ddlMakes = $("#ddlMakes");
            var bikeCityId = $("#ddlCities").val();
            lscache.flushExpired();
            var key = "dealerCitiesByMake_";
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
                            window.location.href = "/new/" + ddlmakemasking + "-dealers/" + ddlcityId + "-" + ddlcityMasking + ".html";
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
                                url: "/api/v2/DealerCity/?makeId=" + selMakeId,
                                contentType: "application/json",
                                dataType: 'json',
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
                toggleErrorMsg($ddlCities, false);
                if (optList != null) {
                    $ddlCities.append($('<option>').text(" Select City ").attr({ 'value': "0" }));
                    $.each(optList, function (i, value) {
                        $ddlCities.append($('<option>').text(value.cityName).attr({ 'value': value.cityId, 'maskingName': value.cityMaskingName }));
                    });
                }

                $ddlCities.trigger('chosen:updated');
                $("#ddlCities_chosen .chosen-single.chosen-default span").text("No cities available");
            }

            $ddlCities.chosen({ no_results_text: "No matches found!!" });
            $ddlMakes.chosen({ no_results_text: "No matches found!!" });
            $('div.chosen-container').attr('style', 'width:100%;border:0');                 

        </script>

    </form>
</body>
</html>
