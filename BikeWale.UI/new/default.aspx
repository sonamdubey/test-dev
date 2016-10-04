﻿<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.New.Default" %>

<%@ Register Src="~/controls/News_new.ascx" TagName="News" TagPrefix="BW" %>
<%@ Register Src="~/controls/UpcomingBikes_new.ascx" TagName="UpcomingBikes" TagPrefix="BW" %>
<%@ Register Src="~/controls/NewLaunchedBikes_new.ascx" TagName="NewLaunchedBikes" TagPrefix="BW" %>
<%@ Register Src="~/controls/MostPopularBikes_new.ascx" TagName="MostPopularBikes" TagPrefix="BW" %>
<%@ Register Src="~/controls/ExpertReviews.ascx" TagName="ExpertReviews" TagPrefix="BW" %>
<%@ Register Src="~/controls/VideosControl.ascx" TagName="Videos" TagPrefix="BW" %>
<%@ Register Src="~/controls/ComparisonMin.ascx" TagName="CompareBikes" TagPrefix="BW" %>
<%@ Register Src="~/controls/EMICalculatorMin.ascx" TagName="EmiCalc" TagPrefix="BW" %>
<%@ Register Src="~/controls/LocateDealer_New.ascx" TagName="LocateDealer" TagPrefix="BW" %>
<%@ Register Src="~/controls/NewBikesOnRoadPrice.ascx" TagName="NBOnRoadPrice" TagPrefix="BW" %>

<!doctype html>
<html>
<head>
    <%
        title = "New Bikes - Bikes Reviews, Photos, Specs, Features, Tips & Advices - BikeWale";
        keywords = "new bikes, new bikes prices, new bikes comparisons, bikes dealers, on-road price, bikes research, bikes india, Indian bikes, bike reviews, bike photos, specs, features, tips & advices";
        description = "New bikes in India. Search for the right new bikes for you, know accurate on-road price and discounts. Compare new bikes and find dealers.";
        alternate = "http://www.bikewale.com/m/new/";
        AdId = "1395986297721";
        AdPath = "/1017752/BikeWale_NewBikesHome_";
        canonical = "http://www.bikewale.com/new/";
    %>
    <%
        isTransparentHeader = true;
        isAd970x90Shown = false;
        isAd970x90BottomShown = false;
        isAd976x400FirstShown = true;
        isAd976x400SecondShown = true;
        isAd300x250Shown = false;
        isAd300x250BTFShown = false;
    %>
    <!-- #include file="/includes/headscript.aspx" -->
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/newbikes.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css">
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/chosen.min.css?<%= staticFileVersion %>" rel="stylesheet" />
    <%  isTransparentHeader = true;
    %>
</head>
<body>
    <form runat="server">
        <!-- #include file="/includes/headBW.aspx" -->
        <header class="new-bikes-top-banner">
            <div class="container">
                <div class="welcome-box">
                    <h1 class="text-uppercase margin-bottom10">NEW BIKES</h1>
                    <h2 class="font20 text-unbold">View all bikes under one roof</h2>
                </div>
            </div>
        </header>

        <section class="container">
            <!--  Brand-Budget-Mileage-Style section code starts here -->
            <div class="grid-12">
                <div class="bw-tabs-panel brand-budget-mileage-style-wrapper content-box-shadow margin-minus50">
                    <div class="bw-tabs bw-tabs-flex">
                        <ul class="brand-budget-mileage-style-UL">
                            <li class="active" data-tabs="discoverBrand">
                                <h3>Brand</h3>
                            </li>
                            <li data-tabs="discoverBudget">
                                <h3>Budget</h3>
                            </li>
                            <li data-tabs="discoverMileage">
                                <h3>Mileage</h3>
                            </li>
                            <li data-tabs="discoverStyle">
                                <h3>Style</h3>
                            </li>
                        </ul>
                    </div>
                    <div class="bw-tabs-data" id="discoverBrand">
                        <div class="brand-type-container">
                            <ul class="text-center">
                                <asp:Repeater ID="rptPopularBrand" runat="server">
                                    <ItemTemplate>
                                        <li>
                                            <a href="/<%# DataBinder.Eval(Container.DataItem, "MaskingName").ToString() %>-bikes/">
                                                <span class="brand-type">
                                                    <span class="brandlogosprite brand-<%# DataBinder.Eval(Container.DataItem, "MakeId").ToString() %>"></span>
                                                </span>
                                                <span class="brand-type-title"><%# DataBinder.Eval(Container.DataItem, "MakeName").ToString() %></span>
                                            </a>
                                        </li>

                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                            <div class="brand-bottom-border border-solid-top margin-left20 margin-right20 hide">
                            </div>
                            <ul class="brand-style-moreBtn padding-top25 brandTypeMore hide margin-left5">
                                <asp:Repeater ID="rptOtherBrands" runat="server">
                                    <ItemTemplate>
                                        <li>
                                            <a href="/<%# DataBinder.Eval(Container.DataItem, "MaskingName").ToString() %>-bikes/">
                                                <span class="brand-type">
                                                    <span class="brandlogosprite brand-<%# DataBinder.Eval(Container.DataItem, "MakeId").ToString() %>"></span>
                                                </span>
                                                <span class="brand-type-title"><%# DataBinder.Eval(Container.DataItem, "MakeName").ToString() %></span>
                                            </a>
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                        </div>
                        <div class="view-brandType text-center padding-top10 padding-bottom30">
                            <a href="javascript:void(0)" id="view-brandType" class="view-more-btn font16">View <span>more</span> brands</a>
                        </div>
                    </div>
                    <div class="bw-tabs-data hide" id="discoverBudget">
                        <div class="budget-container margin-bottom20">
                            <ul class="text-center">
                                <li>
                                    <a href="/new/bike-search/#budget=0-50000">
                                        <span class="budget-title-box font16">Upto
                                        </span>
                                        <span class="budget-amount-box">
                                            <span class="bwsprite inr-budget"></span>
                                            <span class="font20">50,000</span>
                                        </span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/new/bike-search/#budget=50000-100000">
                                        <span class="budget-title-box font16">Between
                                        </span>
                                        <span class="budget-amount-box">
                                            <span class="bwsprite inr-budget"></span>
                                            <span class="font20">50,000 - </span>
                                            <span class="bwsprite inr-budget"></span>
                                            <span class="font20">1</span>
                                            <span class="budget-amount-text-box font16">Lakhs</span>
                                        </span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/new/bike-search/#budget=100000-250000">
                                        <span class="budget-title-box font16">Between
                                        </span>
                                        <span class="budget-amount-box">
                                            <span class="bwsprite inr-budget"></span>
                                            <span class="font20">1</span>
                                            <span class="budget-amount-text-box font16">Lakhs</span>
                                            <span>- </span>
                                            <span class="bwsprite inr-budget"></span>
                                            <span class="font20">2.5</span>
                                            <span class="budget-amount-text-box font16">Lakhs</span>
                                        </span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/new/bike-search/#budget=250000-">
                                        <span class="budget-title-box font16">Above
                                        </span>
                                        <span class="budget-amount-box">
                                            <span class="bwsprite inr-budget"></span>
                                            <span class="font20">2.5</span>
                                            <span class="budget-amount-text-box font16">Lakhs</span>
                                        </span>
                                    </a>
                                </li>
                            </ul>
                        </div>
                    </div>
                    <div class="bw-tabs-data hide" id="discoverMileage">
                        <div class="mileage-container margin-bottom20">
                            <ul class="text-center">
                                <li>
                                    <a href="/new/bike-search/#mileage=1">
                                        <span class="mileage-title-box font16">Above
                                        </span>
                                        <span class="mileage-amount-box">
                                            <span class="font20">70 <span class="font16">Kmpl</span></span>
                                        </span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/new/bike-search/#mileage=2">
                                        <span class="mileage-title-box font16">Between
                                        </span>
                                        <span class="mileage-amount-box">
                                            <span class="font20">70</span>
                                            <span class="mileage-amount-text-box font16">Kmpl</span>
                                            <span class="font20">- 50</span>
                                            <span class="mileage-amount-text-box font16">Kmpl</span>
                                        </span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/new/bike-search/#mileage=3">
                                        <span class="mileage-title-box font16">Between
                                        </span>
                                        <span class="mileage-amount-box">
                                            <span class="font20">50</span>
                                            <span class="mileage-amount-text-box font16">Kmpl</span>
                                            <span class="font20">- 30</span>
                                            <span class="mileage-amount-text-box font16">Kmpl</span>
                                        </span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/new/bike-search/#mileage=4">
                                        <span class="mileage-title-box font16">Upto
                                        </span>
                                        <span class="mileage-amount-box">
                                            <span class="font20">30</span>
                                            <span class="mileage-amount-text-box font16">Kmpl</span>
                                        </span>
                                    </a>
                                </li>
                            </ul>
                        </div>
                    </div>
                    <div class="bw-tabs-data hide" id="discoverStyle">
                        <div class="style-type-container margin-bottom20">
                            <ul class="text-center">
                                <li>
                                    <a href="/new/bike-search/#ridestyle=5">
                                        <span class="style-type">
                                            <span class="styletypesprite style-scooters"></span>
                                        </span>
                                        <span class="style-type-title">Scooters</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/new/bike-search/#ridestyle=3">
                                        <span class="style-type">
                                            <span class="styletypesprite style-street"></span>
                                        </span>
                                        <span class="style-type-title">Street</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/new/bike-search/#ridestyle=1">
                                        <span class="style-type">
                                            <span class="styletypesprite style-cruiser"></span>
                                        </span>
                                        <span class="style-type-title">Cruiser</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/new/bike-search/#ridestyle=2">
                                        <span class="style-type">
                                            <span class="styletypesprite style-sports"></span>
                                        </span>
                                        <span class="style-type-title">Sports</span>
                                    </a>
                                </li>
                            </ul>
                            <ul class="brand-style-moreBtn styleTypeMore hide text-center">
                                <li>
                                    <a href="/new/bike-search/#ridestyle=1">
                                        <span class="style-type">
                                            <span class="styletypesprite style-cruiser"></span>
                                        </span>
                                        <span class="style-type-title">Cruiser</span>

                                    </a>
                                </li>
                                <li>
                                    <a href="/new/bike-search/#ridestyle=2">
                                        <span class="style-type">
                                            <span class="styletypesprite style-sports"></span>
                                        </span>
                                        <span class="style-type-title">Sports</span>

                                    </a>
                                </li>
                                <li>
                                    <a href="/new/bike-search/#ridestyle=5">
                                        <span class="style-type">
                                            <span class="styletypesprite style-scooters"></span>
                                        </span>
                                        <span class="style-type-title">Scooters</span>

                                    </a>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
            <div class="clear"></div>
        </section>
        <% if (isAd976x400FirstShown)
           { %>
        <section>
            <!-- #include file="/ads/Ad976x400_First.aspx" -->
        </section>
        <%} %>
        <section class="container <%= ((ctrlMostPopularBikes.FetchedRecordsCount + ctrlNewLaunchedBikes.FetchedRecordsCount + ctrlUpcomingBikes.FetchedRecordsCount) > 0 )?"":"hide" %> ">
            <!--  Discover bikes section code starts here -->
            <div class="grid-12">
                <h2 class="text-bold text-center margin-top40 margin-bottom20 font22">Featured bikes</h2>
                <div class="bw-tabs-panel newbike-discover-bike-container content-box-shadow padding-bottom15">
                    <div class="bw-tabs bw-tabs-flex">
                        <ul>
                            <li class="active" style="<%= (ctrlMostPopularBikes.FetchedRecordsCount > 0)?"": "display:none" %>" data-tabs="ctrlMostPopularBikes">
                                <h3>Most Popular</h3>
                            </li>
                            <li style="<%= (ctrlNewLaunchedBikes.FetchedRecordsCount > 0)?"": "display:none" %>" data-tabs="ctrlNewLaunchedBikes">
                                <h3>New launches</h3>
                            </li>
                            <li style="<%= (ctrlUpcomingBikes.FetchedRecordsCount > 0)?"": "display:none" %>" data-tabs="ctrlUpcomingBikes">
                                <h3>Upcoming</h3>
                            </li>
                        </ul>
                    </div>
                    <div class="bw-tabs-data <%= (ctrlMostPopularBikes.FetchedRecordsCount > 0)?"":"hide" %>" id="ctrlMostPopularBikes">
                        <div class="jcarousel-wrapper inner-content-carousel">
                            <div class="jcarousel">
                                <ul>
                                    <BW:MostPopularBikes PageId="5" runat="server" ID="ctrlMostPopularBikes" />
                                    <!-- Most Popular Bikes Control-->
                                </ul>
                            </div>
                            <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev"></a></span>
                            <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next"></a></span>
                        </div>
                    </div>

                    <div class="bw-tabs-data hide <%= (ctrlNewLaunchedBikes.FetchedRecordsCount > 0)?"":"hide" %>" id="ctrlNewLaunchedBikes">
                        <BW:NewLaunchedBikes PageId="5" runat="server" ID="ctrlNewLaunchedBikes" />
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
                    </div>
                </div>
            </div>
            <div class="clear"></div>
        </section>

        <section class="container">
            <!--  Compare section code starts here -->
            <h2 class="text-bold text-center margin-top40 margin-bottom20 font22">Compare bikes</h2>
            <div class="grid-12">
                <div class="content-box-shadow">
                    <BW:CompareBikes ID="ctrlCompareBikes" runat="server" />
                </div>
            </div>
            <div class="clear"></div>
        </section>

        <% if (isAd976x400SecondShown)
           { %>
        <section>
            <!-- #include file="/ads/Ad976x400_Second.aspx" -->
        </section>
        <%} %>
        <section class="container">
            <!--  Tools you may need section code starts here -->
            <div class="grid-12">
                <h2 class="text-bold text-center margin-top40 margin-bottom20 font22">Tools you may need</h2>
                <div class="bw-tabs-panel tools-may-need-wrapper content-box-shadow">
                    <div class="bw-tabs bw-tabs-flex">
                        <ul class="tools-may-need-UL">
                            <li data-tabs="getFinal-price" class="active">
                                <h3>On-road price</h3>
                            </li>
                            <li data-tabs="locate-dealer" class="">
                                <h3>Locate a dealer</h3>
                            </li>
                            <li data-tabs="calculate-emi" class="">
                                <h3>Calculate EMI's</h3>
                            </li>
                        </ul>
                    </div>
                    <div class="bw-tabs-data padding-bottom30" id="getFinal-price">
                        <div class="getFinal-price-container text-center">
                            <div class="margin-bottom40">
                                <span class="bw-circle-icon final-price-logo"></span>
                            </div>
                            <p class="font16 margin-bottom30">Get final price of the bike without filling any forms</p>
                            <BW:NBOnRoadPrice PageId="4" Id="NBOnRoadPrice" runat="server" />
                        </div>
                    </div>
                    <div class="bw-tabs-data padding-bottom30 hide" id="locate-dealer">
                        <BW:LocateDealer Id="ctrlLocateDealer" runat="server" />
                    </div>
                    <div class="bw-tabs-data padding-bottom30 hide" id="calculate-emi">
                        <BW:EmiCalc Id="ctrlEmiCalc" runat="server"></BW:EmiCalc>
                        <div class="clear"></div>
                    </div>
                </div>
            </div>
            <div class="clear"></div>
        </section>

        <% 
            if (ctrlNews.FetchedRecordsCount > 0)
            {
                reviewTabsCnt++;
                isNewsZero = false;
                isNewsActive = true;
            }
            if (ctrlExpertReviews.FetchedRecordsCount > 0)
            {
                reviewTabsCnt++;
                isExpertReviewZero = false;
                if (!isNewsActive)
                {
                    isExpertReviewActive = true;
                }
            }
            if (ctrlVideos.FetchedRecordsCount > 0)
            {
                reviewTabsCnt++;
                isVideoZero = false;
                if (!isExpertReviewActive && !isNewsActive)
                {
                    isVideoActive = true;
                }
            }
        %>
        <section class="container <%= reviewTabsCnt == 0 ? "hide" : "" %>">
            <!--  News Bikes latest updates code starts here -->
            <div class="newBikes-latest-updates-container">
                <div class="grid-12">
                    <h2 class="text-bold text-center margin-top40 margin-bottom20 font22">Latest updates from the bike industry</h2>
                    <div class="bw-tabs-panel content-box-shadow margin-bottom30">
                        <div class="text-center <%= reviewTabsCnt > 2 ? "" : ( reviewTabsCnt > 1 ? "margin-top30 margin-bottom30" : "margin-top10") %>">
                            <div class="bw-tabs <%= reviewTabsCnt > 2 ? "bw-tabs-flex" : ( reviewTabsCnt > 1 ? "home-tabs" : "hide") %>" id="reviewCount">
                                <ul>
                                    <li class="<%= isNewsActive ? "active" : "hide" %>" style="<%= (Convert.ToInt32(ctrlNews.FetchedRecordsCount) > 0) ? "": "display:none;" %>" data-tabs="ctrlNews">
                                        <h3>News</h3>
                                    </li>
                                    <li class="<%= isExpertReviewActive ? "active" : "hide" %>" style="<%= (Convert.ToInt32(ctrlExpertReviews.FetchedRecordsCount) > 0) ? "": "display:none;" %>" data-tabs="ctrlExpertReviews">
                                        <h3>Expert Reviews</h3>
                                    </li>
                                    <li class="<%= isVideoActive ? "active" : "hide" %>" style="<%= (Convert.ToInt32(ctrlVideos.FetchedRecordsCount) > 0) ? "": "display:none;" %>" data-tabs="ctrlVideos">
                                        <h3>Videos</h3>
                                    </li>
                                </ul>
                            </div>
                        </div>
                        <%if (!isNewsZero)
                          { %>
                        <BW:News runat="server" ID="ctrlNews" />
                        <% } %>
                        <%if (!isExpertReviewZero)
                          { %>
                        <BW:ExpertReviews runat="server" ID="ctrlExpertReviews" />
                        <% } %>
                        <%if (!isVideoZero)
                          { %>
                        <BW:Videos runat="server" ID="ctrlVideos" />
                        <% } %>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <!-- #include file="/includes/footerBW.aspx" -->
        <!-- #include file="/includes/footerscript.aspx" -->
        <%--<script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/common/chosen.jquery.min.js"></script>--%>
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/newbikes.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st.aeplcdn.com" + staticUrl : "" %>/src/common/chosen.jquery.min.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript">
            ga_pg_id = '4';

            //for jquery chosen : knockout event 
            ko.bindingHandlers.chosen = {
                init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                    var $element = $(element);
                    var options = ko.unwrap(valueAccessor());
                    if (typeof options === 'object')
                        $element.chosen(options);

                    ['options', 'selectedOptions', 'value'].forEach(function (propName) {
                        if (allBindings.has(propName)) {
                            var prop = allBindings.get(propName);
                            if (ko.isObservable(prop)) {
                                prop.subscribe(function () {
                                    $element.trigger('chosen:updated');
                                });
                            }
                        }
                    });
                }
            }
            if ('<%=isNewsActive%>' == "False") $("#ctrlNews").addClass("hide");
            if ('<%=isExpertReviewActive%>' == "False") $("#ctrlExpertReviews").addClass("hide");
            if ('<%=isVideoActive%>' == "False") $("#ctrlVideos").addClass("hide");           
        </script>
    </form>
</body>
</html>
