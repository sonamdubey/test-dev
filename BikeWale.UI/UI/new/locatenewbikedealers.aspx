<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.New.DealerLocator.LocateNewBikeDealers" EnableViewState="false" %>
<%@ Register Src="~/UI/controls/NewLaunchedBikes_new.ascx" TagName="NewLaunchedBikes" TagPrefix="BW" %>
<%@ Register Src="~/UI/controls/UpcomingBikes_new.ascx" TagName="UpcomingBikes" TagPrefix="BW" %>
<%@ Register Src="~/UI/controls/BestBikes.ascx" TagName="BestBikes" TagPrefix="BW" %>
<%@ Import Namespace="Bikewale.Common" %>
<!doctype html>
<html>
<head>
    <%
        title = "New Bike Showroom in India | Find Authorized Bike Dealers - BikeWale";
        keywords = "new bike dealers, new bike showrooms, bike dealers, bike showrooms, showrooms, dealerships";
        description = "Locate new bike showrooms and authorized bike dealers in India. Find new bike dealer information for more than 200 cities in India. ";
        canonical = "https://www.bikewale.com/dealer-showrooms/";
        alternate = "https://www.bikewale.com/m/dealer-showrooms/";
        isHeaderFix = false;
        isAd970x90Shown = false;
        isTransparentHeader = true;
        isAd300x250Shown = false;
        isAd300x250BTFShown = false;
        isAd970x90BottomShown = false;
    %>
    <!-- #include file="/UI/includes/headscript_desktop_min.aspx" -->
    <link rel="stylesheet" type="text/css" href="/UI/css/dealer/landing.css" />
    <script type="text/javascript">
        <!-- #include file="\UI\includes\gacode_desktop.aspx" -->
    </script>
</head>
<body class="bg-light-grey page-type-landing">
    <form runat="server">
        <!-- #include file="/UI/includes/headBW.aspx" -->
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
                    <div class="content-box-shadow padding-top20 collapsible-brand-content">
                        <div id="brand-type-container" class="brand-type-container">
                            <ul class="text-center">
                                <asp:Repeater ID="rptPopularBrands" runat="server">
                                    <ItemTemplate>
                                        <li>
                                            <a href="/dealer-showrooms/<%# DataBinder.Eval(Container.DataItem, "MaskingName") %>/">
                                                <span class="brand-type">
                                                    <span class="lazy brandlogosprite brand-<%# DataBinder.Eval(Container.DataItem, "MakeId") %>"></span>
                                                </span>
                                                <span class="brand-type-title"><%# DataBinder.Eval(Container.DataItem, "MakeName") %></span>
                                            </a>
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                            <ul class="brand-style-moreBtn padding-top25 brandTypeMore hide margin-left5">
                                <asp:Repeater ID="rptOtherBrands" runat="server">
                                    <ItemTemplate>
                                        <li>
                                            <a href="/dealer-showrooms/<%# DataBinder.Eval(Container.DataItem, "MaskingName") %>/">
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
                        <div class="view-all-btn-container padding-bottom25">
                            <a href="javascript:void(0)" class="view-brandType btn view-all-target-btn rotate-arrow" rel="nofollow"><span class="btn-label">View more brands</span><span class="bwsprite teal-right"></span></a>
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
                    <h2 class="section-header">New Bike Launches in India</h2>
                    <div class="content-box-shadow padding-top20">
                        
                        <%if (ctrlNewLaunchedBikes.FetchedRecordsCount > 0)
                            { %>
                        <BW:NewLaunchedBikes runat="server" ID="ctrlNewLaunchedBikes" />
                        <div class="view-all-btn-container padding-top15 padding-bottom20">
                            <a href="/new-bike-launches/" class="btn view-all-target-btn" title="New Bike Launches in India">View all launches<span class="bwsprite teal-right"></span></a>
                        </div>
                        <%} %>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section>
            <div class="container section-container">
                <div class="grid-12">
                    <h2 class="section-header">Upcoming Bikes in India</h2>
                    <div class="content-box-shadow padding-top20">
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
                        <div class="view-all-btn-container padding-top15 padding-bottom20">
                            <a href="/upcoming-bikes/" class="btn view-all-target-btn" title="Upcoming Bikes in India">View all bikes<span class="bwsprite teal-right"></span></a>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>


        <script type="text/javascript" src="<%= staticUrl %>/UI/src/frameworks.js?<%=staticFileVersion %>"></script>
        <!-- #include file="/UI/includes/footerBW.aspx" -->
        <link href="<%= staticUrl  %>/UI/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />

        <!-- #include file="/UI/includes/footerscript.aspx" -->
        
        <script type="text/javascript"> 
            var $ddlCities = $("#ddlCities"), $ddlMakes = $("#ddlMakes");
            var bikeCityId = $("#ddlCities").val();
            lscache.flushExpired();
            var key = "dealerCitiesByMake_";
            lscache.setBucket('DLPage');  

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
                            window.location.href = "/dealer-showrooms/" + ddlmakemasking + "/" + ddlcityMasking + "/";
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

        <!-- #include file="/UI/includes/fontBW.aspx" -->
    </form>
</body>
</html>
