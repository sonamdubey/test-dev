<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Default" EnableViewState="false" Trace="false" %>

<%@ Register Src="~/controls/News.ascx" TagName="News" TagPrefix="BW" %>
<%@ Register Src="~/controls/NewExpertReviews.ascx" TagName="ExpertReviews" TagPrefix="BW" %>
<%@ Register Src="~/controls/NewVideosControl.ascx" TagName="Videos" TagPrefix="BW" %>
<%@ Register Src="~/controls/ComparisonMin.ascx" TagName="CompareBikes" TagPrefix="BW" %>
<%@ Register Src="~/controls/OnRoadPriceQuote.ascx" TagName="OnRoadPriceQuote" TagPrefix="BW" %>
<%@ Register Src="~/controls/UpcomingBikes_new.ascx" TagName="UpcomingBikes" TagPrefix="BW" %>
<%@ Register Src="~/controls/NewLaunchedBikes_new.ascx" TagName="NewLaunchedBikes" TagPrefix="BW" %>
<%@ Register Src="~/controls/MostPopularBikes_new.ascx" TagName="MostPopularBikes" TagPrefix="BW" %>
<%@ Register Src="~/controls/BestBikes.ascx" TagName="BestBikes" TagPrefix="BW" %>
<%@ Register Src="~/controls/usedBikeModel.ascx" TagName="usedBikeModel" TagPrefix="BW" %>
<%@ Register Src="~/controls/usedBikeInCities.ascx" TagName="usedBikeInCities" TagPrefix="BW" %>

<!doctype html>
<html>
<head>
    <%
        title = "New Bikes, Used Bikes, Bike Prices, Reviews & Images in India";
        keywords = "new bikes, used bikes, buy used bikes, sell your bike, bikes prices, reviews, Images, news, compare bikes, Instant Bike On-Road Price";
        description = "BikeWale - India's favourite bike portal. Find new and used bikes, buy or sell your bikes, compare new bikes prices & values.";
        AdPath = "/1017752/BikeWale_HomePage_";
        AdId = "1395985604192";
        alternate = "https://www.bikewale.com/m/";
        canonical = "https://www.bikewale.com/";
        isAd970x90Shown = false;
        isAd970x90BottomShown = false;
        isAd300x250Shown = false;
        isAd300x250BTFShown = false;
        isAd976x400FirstShown = true;
        isAd976x400SecondShown = true;
        isAd976x204 = false;
        PopupWidget.Visible = true;        
        isTransparentHeader = true;
    %>
    <noscript><link rel="stylesheet" href="path/to/mystylesheet.css"></noscript>
    <!-- #include file="/includes/headscript_desktop_min.aspx" -->
    <link rel="stylesheet" type="text/css" href="/css/home.css" />
    <script type="text/javascript">
         <!-- #include file="\includes\gacode_desktop.aspx" -->
         ga_pg_id = '1'; 
    </script>

</head>
<body class="page-type-landing">
    <noscript id="asynced-css"><link rel="stylesheet" type="text/css" href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/css/bw-common-btf.css?<%=staticFileVersion %>" /></noscript>
    <form runat="server">
        <!-- #include file="/includes/headBW.aspx" -->
        <header class="home-top-banner">
            <div class="container top-campaign-banner-container">
                <div class="welcome-box">
                    <h1>Find your bike</h1>
                    <p class="banner-subheading font20">Get Comprehensive Information on Bike Prices, Specs, Reviews & More!</p>
                    <div class="margin-top60">
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
                            <ul id="new-global-recent-searches" class="recent-searches-dropdown"></ul>
                        </div>
                    </div>
                </div>
                <% if (bannerEntity != null)
                   { %>
                <%= bannerEntity.DesktopCss %>
                <%= bannerEntity.DesktopHtml %>
                <%= bannerEntity.DesktopJS %>
                <% } %>
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
                            <ul class="brand-collapsible-present">
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
                        <div class="bw-tabs-data collapsible-brand-content" id="discoverBrand">
                            <div id="brand-type-container" class="brand-type-container">
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
                            <div class="view-all-btn-container padding-top10 padding-bottom30">
                                <a href="javascript:void(0)" class="view-brandType btn view-all-target-btn rotate-arrow"><span class="btn-label">View more brands</span><span class="bwsprite teal-right"></span></a>
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
                    <div class="bw-tabs-panel newbike-discover-bike-container content-box-shadow">
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

        <section class="lazy home-getFinalPrice-banner" data-original="https://imgd3.aeplcdn.com/0x0/bw/static/landing-banners/d/get-final-price-banner.jpg">
            <BW:OnRoadPriceQuote ID="ctrlOnRoadPriceQuote" PageId="1" runat="server" />
        </section>
        <% if (isAd976x400SecondShown)
           { %>
        <section>
            <!-- #include file="/ads/Ad976x400_Second.aspx" -->
        </section>
        <%} %>

        <% if (ctrlBestBikes != null)
           { %>
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
            <!--  Discover your bike code starts here -->
            <div class="container">
                <div class="grid-12">
                    <h2 class="text-bold text-center margin-top30 margin-bottom20 font22">Find used bikes</h2>
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

                        <div class="bw-tabs-data <%=((ctrlusedBikeModel.FetchCount>0) ||( ctrlusedBikeInCities.objCitiesWithCount != null && ctrlusedBikeInCities.objCitiesWithCount.Count() > 0))?"hide":""%> padding-bottom15" id="usedByBudget">
                            <ul class="elevated-card-list">
                                <li>
                                    <a href="/used/bikes-in-india/#budget=0+35000" rel="nofollow">
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
                                    <a href="/used/bikes-in-india/#budget=35000+80000" rel="nofollow">
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
                                    <a href="/used/bikes-in-india/#budget=80000+200000" rel="nofollow">
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
                        </div>


                    </div>
                    <div class="clear"></div>
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
        <script type="text/javascript" defer src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : "" %>/src/frameworks.js?<%=staticFileVersion %>"></script>
        <script type="text/javascript" defer src="<%= staticUrl != "" ? "https://st.aeplcdn.com" + staticUrl : "" %>/src/Plugins.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" defer src="<%= staticUrl != "" ? "https://st.aeplcdn.com" + staticUrl : "" %>/src/common.js?<%= staticFileVersion %>"></script>

        <script type="text/javascript">          
            docReady(function () {
                $('#globalSearch').parent().hide();
                if (!<%=isNewsActive.ToString().ToLower() %>) $("#ctrlNews").addClass("hide");
                if (!<%=isExpertReviewActive.ToString().ToLower() %>) $("#ctrlExpertReviews").addClass("hide");
                if (!<%=isVideoActive.ToString().ToLower() %>) $("#ctrlVideos").addClass("hide");
            });
        </script>

        <!-- #include file="/includes/fontBW.aspx" -->
    </form>
</body>
</html>
