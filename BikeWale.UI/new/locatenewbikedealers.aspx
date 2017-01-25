<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.New.DealerLocator.LocateNewBikeDealers" EnableViewState="false" %>
<%@ Register Src="~/controls/NewLaunchedBikes_new.ascx" TagName="NewLaunchedBikes" TagPrefix="BW" %>
<%@ Register Src="~/controls/UpcomingBikes_new.ascx" TagName="UpcomingBikes" TagPrefix="BW" %>
<%@ Register Src="~/controls/BestBikes.ascx" TagName="BestBikes" TagPrefix="BW" %>
<%@ Import Namespace="Bikewale.Common" %>
<!doctype html>
<html>
<head>
    <%
        title = "New Bike Showroom in India | Find Authorized Bike Dealers - BikeWale";
        keywords = "new bike dealers, new bike showrooms, bike dealers, bike showrooms, showrooms, dealerships";
        description = "Locate new bike showrooms and authorized bike dealers in India. Find new bike dealer information for more than 200 cities in India. ";
        canonical = "https://www.bikewale.com/dealer-showroom-locator/";
        alternate = "https://www.bikewale.com/m/dealer-showroom-locator/";
        isHeaderFix = false;
        isAd970x90Shown = false;
        isTransparentHeader = true;
        isAd300x250Shown = false;
        isAd300x250BTFShown = false;
        isAd970x90BottomShown = false;
    %>
    <!-- #include file="/includes/headscript_desktop_min.aspx" -->
    <style type="text/css">@charset "utf-8";#dealer-locator-box .welcome-box{margin-top:85px}.dealerlocator-banner{background:url(https://imgd1.aeplcdn.com/0x0/bw/static/landing-banners/d/dealer-locator-banner.jpg?v1=29Sep2016) center top no-repeat #393a3e;height:268px;padding-top:1px}.section-container{margin-bottom:25px}h2.section-header{font-size:18px;text-align:center;margin-bottom:20px}.padding-25-30{padding:25px 30px}.negative-50{margin-top:-50px}.locator-search-container{margin:0 auto;width:570px}.locator-search-container .form-control{padding:8px;border-radius:0}.locator-search-brand,.locator-search-city{height:40px;float:left}.locator-search-brand{width:334px}.locator-search-city{width:233px}.locator-search-brand select,.locator-search-city select{width:100%;height:38px;color:#555}.locator-search-city .chosen-container{border-left:0!important}.locator-search-brand .chosen-container,.locator-search-city .chosen-container{border:1px solid #e2e2e2!important;border-radius:0;padding:12px 20px}#applyFiltersBtn{margin-top:35px}.locator-search-btn.btn-lg{padding:8px 64px}.locator-search-container .error-icon{right:20px}.locator-search-container .bw-blackbg-tooltip{right:10px}.brand-type-container li{display:inline-block;vertical-align:top;width:180px;height:85px;margin:0 5px 30px;text-align:center;font-size:18px;-moz-border-radius:2px;-webkit-border-radius:2px;-o-border-radius:2px;-ms-border-radius:2px;border-radius:2px}.brand-1,.brand-10,.brand-11,.brand-12,.brand-13,.brand-14,.brand-15,.brand-16,.brand-17,.brand-18,.brand-19,.brand-2,.brand-20,.brand-22,.brand-23,.brand-24,.brand-3,.brand-34,.brand-37,.brand-38,.brand-39,.brand-4,.brand-40,.brand-41,.brand-42,.brand-5,.brand-6,.brand-7,.brand-71,.brand-8,.brand-81,.brand-9,.brand-type{height:50px}.brand-type{width:180px;display:block;margin:0 auto}.brand-type-title{margin-top:10px;display:block}.brand-type-container a{text-decoration:none;color:#82888b;display:inline-block}.brand-type-container li:hover span.brand-type-title{color:#4d5057;font-weight:700}.brand-bottom-border{overflow:hidden}.brandlogosprite{background:url(https://imgd2.aeplcdn.com/0x0/bw/static/sprites/d/brand-type-sprite.png?v=04Oct2016) no-repeat;display:inline-block}.brand-2{width:87px;background-position:0 0}.brand-7{width:56px;background-position:-96px 0}.brand-1{width:88px;background-position:-162px 0}.brand-8{width:100px;background-position:-260px 0}.brand-12{width:67px;background-position:-370px 0}.brand-40{width:125px;background-position:-447px 0}.brand-34{width:122px;background-position:-582px 0}.brand-22{width:121px;background-position:-714px 0}.brand-3{width:44px;background-position:-845px 0}.brand-17{width:86px;background-position:-899px 0}.brand-15{width:118px;background-position:-995px 0}.brand-4{width:43px;background-position:-1123px 0}.brand-9{width:99px;background-position:-1176px 0}.brand-16{width:117px;background-position:-1285px 0}.brand-5{width:59px;background-position:-1412px 0}.brand-19{width:122px;background-position:-1481px 0}.brand-13{width:122px;background-position:-1613px 0}.brand-6{width:63px;background-position:-1745px 0}.brand-10{width:102px;background-position:-1818px 0}.brand-14{width:127px;background-position:-1930px 0}.brand-39{width:89px;background-position:-2067px 0}.brand-20{width:82px;background-position:-2166px 0}.brand-11{width:121px;background-position:-2258px 0}.brand-41{width:63px;background-position:-2389px 0}.brand-42{width:64px;background-position:-2461px 0}.brand-81{width:71px;background-position:-2535px 0}.brand-71{width:39px;background-position:-2616px 0}@media only screen and (max-width:1024px){.brand-type,.brand-type-container li{width:170px}}</style>
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_desktop.aspx" -->
    </script>
</head>
<body class="bg-light-grey">
    <form runat="server">
        <!-- #include file="/includes/headBW.aspx" -->
        <header class="dealerlocator-banner">
            <div id="dealer-locator-box" class="container">
                <div class="welcome-box">
                    <h1 class="font30 text-uppercase margin-bottom30">Showroom Locator</h1>
                    <h2 class="font20 text-unbold text-white margin-bottom50">Find new bike dealers across 200+ cities</h2>
                </div>
            </div>
        </header>

        <section>
            <div class="container section-container">
                <div class="grid-12">
                    <div class="content-box-shadow negative-50 text-center padding-25-30">
                        <h2 class="section-header">Find bike dealers in your city</h2>
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
                            <div class="clear"></div>
                            <input type="button" id="applyFiltersBtn" class="btn btn-orange btn-lg locator-search-btn margin-bottom5" value="Search" />
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section>
            <div class="container section-container">
                <div class="grid-12">
                    <h2 class="section-header">Explore showroom by brand</h2>
                    <div class="content-box-shadow padding-top20">
                        <div class="brand-type-container">
                            <ul class="text-center">
                                <asp:Repeater ID="rptPopularBrands" runat="server">
                                    <ItemTemplate>
                                        <li>
                                            <a href="/<%# DataBinder.Eval(Container.DataItem, "MaskingName") %>-dealer-showrooms-in-india/">
                                                <span class="brand-type">
                                                    <span class="lazy brandlogosprite brand-<%# DataBinder.Eval(Container.DataItem, "MakeId") %>"></span>
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
                                            <a href="/<%# DataBinder.Eval(Container.DataItem, "MaskingName") %>-dealer-showrooms-in-india/">
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
                        <div class="view-brandType text-center padding-bottom25">
                            <a href="javascript:void(0)" id="view-brandType" class="view-more-btn font16" rel="nofollow">View <span>more</span> brands</a>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <% if(ctrlBestBikes!= null) { %>
        <section>
            <div class="container section-container">
                <div class="grid-12">
                    <h2 class="section-header">Best bikes of <%= ctrlBestBikes.PrevMonthDate %></h2>
                    <div class="content-box-shadow padding-top20 padding-bottom20">
                        <BW:BestBikes runat="server" ID="ctrlBestBikes" />
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <% } %>

        <section>
            <div class="container section-container">
                <div class="grid-12">
                    <h2 class="section-header">Newly launched bikes</h2>
                    <div class="content-box-shadow padding-top20 padding-bottom20">
                        
                        <%if (ctrlNewLaunchedBikes.FetchedRecordsCount > 0)
                            { %>
                            <BW:NewLaunchedBikes runat="server" ID="ctrlNewLaunchedBikes" />
                        <%} %>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section>
            <div class="container section-container">
                <div class="grid-12">
                    <h2 class="section-header">Upcoming bikes</h2>
                    <div class="content-box-shadow padding-top20 padding-bottom20">
                        <div class="jcarousel-wrapper inner-content-carousel">
                            <div class="jcarousel">
                                <ul>
                                   <%if (ctrlUpcomingBikes.FetchedRecordsCount > 0)
                                    { %>
                                        <BW:UpcomingBikes runat="server" ID="ctrlUpcomingBikes" />
                                    <%} %>
                                </ul>
                            </div>
                            <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev"></a></span>
                            <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next"></a></span>
                        </div>
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
                            bwcache.remove("userchangedlocation", true);
                            window.location.href = "/" + ddlmakemasking + "-dealer-showrooms-in-" + ddlcityMasking+ "/";
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
                    var obj = GetGlobalLocationObject();
                    if (obj != null) {
                        $ddlCities.val(obj.CityId);
                    }
                }

                $ddlCities.trigger('chosen:updated');
                $("#ddlCities_chosen .chosen-single.chosen-default span").text("No cities available");
            }

            $ddlCities.chosen({ no_results_text: "No matches found!!" });
            $ddlMakes.chosen({ no_results_text: "No matches found!!" });
            $('div.chosen-container').attr('style', 'width:100%;border:0');                 

        </script>

        <!-- #include file="/includes/fontBW.aspx" -->
    </form>
</body>
</html>
