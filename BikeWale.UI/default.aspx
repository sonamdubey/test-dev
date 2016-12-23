﻿<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Default" EnableViewState="false" Trace="false" %>

<%@ Register Src="~/controls/News.ascx" TagName="News" TagPrefix="BW" %>
<%@ Register Src="~/controls/NewExpertReviews.ascx" TagName="ExpertReviews" TagPrefix="BW" %>
<%@ Register Src="~/controls/NewVideosControl.ascx" TagName="Videos" TagPrefix="BW" %>
<%@ Register Src="~/controls/ComparisonMin.ascx" TagName="CompareBikes" TagPrefix="BW" %>
<%@ Register Src="~/controls/PopularUsedBikes.ascx" TagName="PopularUsedBikes" TagPrefix="BW" %>
<%@ Register Src="~/controls/OnRoadPriceQuote.ascx" TagName="OnRoadPriceQuote" TagPrefix="BW" %>
<%@ Register Src="~/controls/UpcomingBikes_new.ascx" TagName="UpcomingBikes" TagPrefix="BW" %>
<%@ Register Src="~/controls/NewLaunchedBikes_new.ascx" TagName="NewLaunchedBikes" TagPrefix="BW" %>
<%@ Register Src="~/controls/MostPopularBikes_new.ascx" TagName="MostPopularBikes" TagPrefix="BW" %>
<%@ Register Src="~/controls/BestBikes.ascx" TagName="BestBikes" TagPrefix="BW" %>

<!doctype html>
<html>
<head>
    <%
        title = "New Bikes, Used Bikes, Bike Prices, Reviews & Photos in India";
        keywords = "new bikes, used bikes, buy used bikes, sell your bike, bikes prices, reviews, photos, news, compare bikes, Instant Bike On-Road Price";
        description = "BikeWale - India's favourite bike portal. Find new and used bikes, buy or sell your bikes, compare new bikes prices & values.";
        AdPath = "/1017752/BikeWale_HomePage_";
        AdId = "1395985604192";
        alternate = "https://www.bikewale.com/m/";
        canonical = "https://www.bikewale.com/";
        isAd970x90Shown = false;
        isAd970x90BottomShown = false;
        isAd976x400FirstShown = true;
        isAd976x400SecondShown = true;
        isAd976x204 = true;
        isAd300x250Shown = false;
        isAd300x250BTFShown = false;
        PopupWidget.Visible = true;
    %>
    <!-- #include file="/includes/headscript.aspx" -->
    <link href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/css/home.css?<%= staticFileVersion%>" rel="stylesheet" type="text/css">
    <%  isTransparentHeader = true;
    %>
</head>
<body>
    <form runat="server">
        <!-- #include file="/includes/headBW.aspx" -->
        <header class="home-top-banner">
            <div class="container">
                <div class="welcome-box">
                    <h1>Find your bike</h1>
                    <p class="font20">Get Comprehensive Information on Bike Prices, Specs, Reviews & More!</p>
                    <div class="margin-top60">
                        <div>
                            <div class="bike-search-container position-rel">
                                <div class="bike-search new-bike-search position-rel">
                                    <input type="text" placeholder="Search your bike here, e.g. Honda Activa " id="newBikeList" autocomplete="off" tabindex="1">
                                    <span class="fa fa-spinner fa-spin position-abt pos-right10 pos-top15 text-black" style="display: none"></span>
                                </div>
                                <div class="findBtn">
                                    <input type="button" id="btnSearch" class="btn btn-orange btn-md font16" tabindex="2" value="Search" />
                                </div>
                                <div class="clear"></div>
                                <ul id="errNewBikeSearch" class="ui-autocomplete ui-front ui-menu hide">
                                    <li class="ui-menu-item" tabindex="-1">
                                        <span class="text-bold">Oops! No suggestions found</span><br />
                                        <span class="text-light-grey font12">Search by bike name e.g: Honda Activa</span>
                                    </li>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </header>
        <!--  Ends here -->
        <% if (isAd976x400FirstShown)
           { %>
        <section>
            <!-- #include file="/ads/Ad976x400_First.aspx" -->
        </section>
        <% } %>
        <section>
            <!--  Discover your bike code starts here -->
            <div class="container">
                <div class="grid-12">
                    <h2 class="text-bold text-center margin-top30 margin-bottom20 font22">Discover your bike</h2>
                    <div class="bw-tabs-panel content-box-shadow brand-budget-mileage-style-wrapper">
                        <div class="bw-tabs bw-tabs-flex">
                            <ul class="brand-budget-mileage-style-UL">
                                <li class="active" data-tabs="discoverBrand"><h3>Brand</h3></li>
                                <li data-tabs="discoverBudget"><h3>Budget</h3></li>
                                <li data-tabs="discoverMileage"><h3>Mileage</h3></li>
                                <li data-tabs="discoverStyle"><h3>Style</h3></li>
                            </ul>
                        </div>
                        <div class="bw-tabs-data" id="discoverBrand">
                            <div class="brand-type-container">
                                <ul class="text-center">
                                    <asp:Repeater ID="rptPopularBrand" runat="server">
                                        <ItemTemplate>
                                            <li>
                                                <a href="/<%# DataBinder.Eval(Container.DataItem, "MaskingName") %>-bikes/">
                                                    <span class="brand-type">                                                        
                                                	    <span class="brandlogosprite brand-<%# DataBinder.Eval(Container.DataItem, "MakeId") %>"></span>
                                                    </span>
                                                    <span class="brand-type-title"><%# DataBinder.Eval(Container.DataItem, "MakeName") %></span>
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
                                                <a href="/<%# DataBinder.Eval(Container.DataItem, "MaskingName") %>-bikes/">
                                                    <span class="brand-type">
                                                        <span class="brandlogosprite brand-<%# DataBinder.Eval(Container.DataItem, "MakeId") %>"></span>
                                                    </span>
                                                    <span class="brand-type-title"><%# DataBinder.Eval(Container.DataItem, "MakeName") %></span>
                                                </a>
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                            </div>
                            <div class="view-brandType text-center padding-top10 padding-bottom30">
                                <a href="#" id="view-brandType" class="view-more-btn font16">View <span>more</span> brands</a>
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
                                            <span class="mileage-amount-box font20">
                                                <span>70 <span class="font16">Kmpl</span></span>
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
                            <div class="style-type-container margin-bottom35">
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
                            </div>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        
        <section>
            <div class="container <%= ((ctrlMostPopularBikes.FetchedRecordsCount + ctrlNewLaunchedBikes.FetchedRecordsCount + ctrlUpcomingBikes.FetchedRecordsCount) > 0 )?" margin-bottom30":"hide" %> ">
                <!--  Discover bikes section code starts here -->
                <div class="grid-12">
                    <h2 class="text-bold text-center margin-top30 margin-bottom20 font22">Featured bikes</h2>
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
            </div>
        </section>

        <section class="lazy home-getFinalPrice-banner" data-original="https://imgd3.aeplcdn.com/0x0/bw/static/landing-banners/d/get-final-price-banner.jpg">
            <BW:OnRoadPriceQuote ID="ctrlOnRoadPriceQuote" PageId="1" runat="server" />
        </section>
        <% if (isAd976x400SecondShown)
           { %>
        <section>
            <!-- #include file="/ads/Ad976x400_Second.aspx" -->
        </section>
        <%} %>

        <% if(ctrlBestBikes!= null) { %>
        <section>
            <div class="container section-bottom-margin">
                <h2 class="text-center margin-top30 margin-bottom20 font22">Best bikes of <%= ctrlBestBikes.PrevMonthDate %></h2>
                <div class="grid-12">
                    <div class="content-box-shadow padding-top20 padding-bottom20">
                        <BW:BestBikes runat="server" ID="ctrlBestBikes" />
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <% } %>
        <section>
            <!--  Compare section code starts here -->
            <div class="container">
                <h2 class="text-bold text-center margin-top30 margin-bottom20 font22">Compare bikes</h2>
                <div class="grid-12">
                    <div class="content-box-shadow padding-top20 padding-bottom20">
                        <BW:CompareBikes ID="ctrlCompareBikes" runat="server" />
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <!-- Ends here -->
        <section>
            <div class="<%= (ctrlPopularUsedBikes.FetchedRecordsCount > 0)?"":"hide" %>">
                <BW:PopularUsedBikes runat="server" ID="ctrlPopularUsedBikes" />
            </div>
        </section>

        <!-- Ends here -->
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
        <section>
            <!--  News Bikes latest updates code starts here -->
            <div class="container <%= reviewTabsCnt == 0 ? "hide" : "" %>">
                <div class="grid-12">
                    <h2 class="text-bold text-center margin-top40 margin-bottom20 font22">Latest updates from the bike industry</h2>
                    <div class="bw-tabs-panel margin-bottom30 padding-bottom20 content-box-shadow article-control">
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
                        <div class="bw-tabs-data padding-left10 padding-right10" id="ctrlNews">
                            <%if (!isNewsZero)
                              { %>
                            <BW:News runat="server" ID="ctrlNews" />
                            <% } %>
                        </div>
                        <div class="bw-tabs-data hide padding-left10 padding-right10" id="ctrlExpertReviews">
                            <%if (!isExpertReviewZero)
                              { %>
                            <BW:ExpertReviews runat="server" ID="ctrlExpertReviews" />
                            <% } %>
                        </div>
                        <div class="bw-tabs-data hide padding-left10 padding-right10" id="ctrlVideos">
                            <%if (!isVideoZero)
                              { %>
                            <BW:Videos runat="server" ID="ctrlVideos" />
                            <% } %>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <!-- Ends here -->
        <!-- #include file="/includes/footerBW.aspx" -->
        <!-- #include file="/includes/footerscript.aspx" -->
        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st.aeplcdn.com" + staticUrl : "" %>/src/home.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st.aeplcdn.com" + staticUrl : "" %>/src/common/chosen.jquery.min.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript">
            ga_pg_id = '1';
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
