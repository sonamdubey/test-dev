<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Default" EnableViewState="false"%>

<%@ Register Src="/m/controls/NewNewsWidget.ascx" TagName="News" TagPrefix="BW" %>
<%@ Register Src="/m/controls/NewExpertReviewsWidget.ascx" TagName="ExpertReviews" TagPrefix="BW" %>
<%@ Register Src="/m/controls/NewVideosWidget.ascx" TagName="Videos" TagPrefix="BW" %>
<%@ Register Src="/m/controls/CompareBikesMin.ascx" TagName="CompareBike" TagPrefix="BW" %>
<%@ Register Src="/m/controls/MOnRoadPricequote.ascx" TagName="MOnRoadPricequote" TagPrefix="BW" %>
<%@ Register Src="~/m/controls/MUpcomingBikes.ascx" TagName="MUpcomingBikes" TagPrefix="BW" %>
<%@ Register Src="~/m/controls/MNewLaunchedBikes.ascx" TagName="MNewLaunchedBikes" TagPrefix="BW" %>
<%@ Register Src="~/m/controls/MMostPopularBikes.ascx" TagName="MMostPopularBikes" TagPrefix="BW" %>
<%@ Register Src="~/m/controls/BestBikes.ascx" TagName="BestBikes" TagPrefix="BW" %>
<%@ Register Src="~/m/controls/usedBikeModel.ascx" TagName="usedBikeModel" TagPrefix="BW" %>
<%@ Register Src="~/m/controls/usedBikeInCities.ascx" TagName="usedBikeInCities" TagPrefix="BW" %>
<!doctype html>
<html>
<head>
    <%
        title = "New Bikes, Used Bikes, Bike Prices, Reviews & Images in India";
        keywords = "new bikes, used bikes, buy used bikes, sell your bike, bikes prices, reviews, Images, news, compare bikes, Instant Bike On-Road Price";
        description = "BikeWale - India's favourite bike portal. Find new and used bikes, buy or sell your bikes, compare new bikes prices & values.";
        canonical = "https://www.bikewale.com";
        AdPath = "/1017752/Bikewale_Mobile_Homepage";
        AdId = "1450262275060";
        Ad320x150_I = true;
        Ad320x150_II = true;
    %>
    <!-- #include file="/includes/headscript_mobile_min.aspx" -->
    <link rel="stylesheet" type="text/css" href="/m/css/home.css" />
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_mobile.aspx" -->
    </script>
</head>
<body class="bg-light-grey page-type-landing">
    <form runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->
        <section>
            <div class="banner-home">
                <div class="container banner-container text-center top-campaign-banner-container">
                    <!-- Top banner code starts here -->
                    <h1 class="font22 text-uppercase text-white">Find your bike</h1>
                    <h2 class="banner-subheading font14 text-unbold text-white">Get Comprehensive Information on Bikes!</h2>
                    <div class="new-bikes-search position-rel">
                        <input type="text" placeholder="Search your bike here, e.g. Honda Activa " id="newBikeList" autocomplete="off" class="rounded-corner2">
                        <a href="javascript:void(0);" id="btnSearch" class="btn btn-orange btn-search" rel="nofollow"><span class="bwmsprite search-bold-icon"></span></a>
                    </div>
                    <%= bannerEntity.MobileCss %>
                    <%= bannerEntity.MobileHtml %>
                    <%= bannerEntity.MobileJS %>
                </div>
            </div>
        </section>
        <% if (Ad320x150_I)
           { %>
        <section>
            <!-- #include file="/ads/Ad320x150_First.aspx" -->
        </section>
        <% } %>
        <section>
            <div class="container">
                <div class="grid-12 alpha omega">
                    <h2 class="font18 text-center margin-top20 margin-bottom10">Discover your bike</h2>
                    <div class="bw-tabs-panel padding-bottom20 content-box-shadow bg-white" id="discoverBikesContainer">

                        <div class="bw-tabs bw-tabs-flex">
                            <ul class="brand-collapsible-present">
                                <li class="active" data-tabs="discoverBrand">Brand</li>
                                <li data-tabs="discoverBudget">Budget</li>
                                <li data-tabs="discoverMileage">Mileage</li>
                                <li data-tabs="discoverStyle">Style</li>
                            </ul>
                        </div>

                        <div class="bw-tabs-data collapsible-brand-content" id="discoverBrand">
                            <div id="brand-type-container" class="brand-type-container">
                                <ul class="text-center">
                                    <asp:Repeater ID="rptPopularBrand" runat="server">
                                        <ItemTemplate>
                                            <li>
                                                <a href="/m/<%# DataBinder.Eval(Container.DataItem, "MaskingName") %>-bikes/">
                                                    <span class="brand-type">
                                                        <span class="brandlogosprite brand-<%# DataBinder.Eval(Container.DataItem, "MakeId") %>"></span>
                                                    </span>
                                                    <span class="brand-type-title"><%# DataBinder.Eval(Container.DataItem, "MakeName") %></span>
                                                </a>
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                                <ul class="brand-style-moreBtn brandTypeMore border-top1 padding-top25 text-center hide">
                                    <asp:Repeater ID="rptOtherBrands" runat="server">
                                        <ItemTemplate>
                                            <li>
                                                <a href="/m/<%# DataBinder.Eval(Container.DataItem, "MaskingName") %>-bikes/">
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
                            <div class="view-all-btn-container">
                                <a href="javascript:void(0)" class="view-brandType view-more-btn btn view-all-target-btn rotate-arrow" rel="nofollow"><span class="btn-label">View more brands</span><span class="bwmsprite teal-right"></span></a>
                            </div>
                        </div>
                        <div class="bw-tabs-data hide" id="discoverBudget">
                            <div class="budget-container">
                                <ul class="text-center">
                                    <li>
                                        <a href="/m/new/bike-search/?budget=0-50000">
                                            <span class="budget-title-box font14">Upto
                                            </span>
                                            <span class="budget-amount-box">
                                                <span class="bwmsprite inr-grey-xxsm-icon"></span>
                                                <span class="font14 text-bold">50,000</span>
                                            </span>
                                        </a>
                                    </li>
                                    <li>
                                        <a href="/m/new/bike-search/?budget=50000-100000">
                                            <span class="budget-title-box font14">Between
                                            </span>
                                            <span class="budget-amount-box font14 text-bold">
                                                <span class="bwmsprite inr-grey-xxsm-icon"></span>
                                                <span>50,000 - </span>
                                                <span class="bwmsprite inr-grey-xxsm-icon"></span>
                                                <span>1</span>
                                                <span class="budget-amount-text-box">Lac</span>
                                            </span>
                                        </a>
                                    </li>
                                    <li>
                                        <a href="/m/new/bike-search/?budget=100000-250000">
                                            <span class="budget-title-box font14">Between
                                            </span>
                                            <span class="budget-amount-box font14 text-bold">
                                                <span class="bwmsprite inr-grey-xxsm-icon"></span>
                                                <span>1</span>
                                                <span class="budget-amount-text-box">Lac</span>
                                                <span>- </span>
                                                <span class="bwmsprite inr-grey-xxsm-icon"></span>
                                                <span>2.5</span>
                                                <span class="budget-amount-text-box">Lac</span>
                                            </span>
                                        </a>
                                    </li>
                                    <li>
                                        <a href="/m/new/bike-search/?budget=250000-">
                                            <span class="budget-title-box font14">Above
                                            </span>
                                            <span class="budget-amount-box font14 text-bold">
                                                <span class="bwmsprite inr-grey-xxsm-icon"></span>
                                                <span>2.5</span>
                                                <span class="budget-amount-text-box">Lac</span>
                                            </span>
                                        </a>
                                    </li>
                                </ul>
                            </div>
                        </div>
                        <div class="bw-tabs-data hide" id="discoverMileage">
                            <div class="mileage-container">
                                <ul class="text-center">
                                    <li>
                                        <a href="/m/new/bike-search/?mileage=1">
                                            <span class="mileage-title-box font14">Above
                                            </span>
                                            <span class="mileage-amount-box">
                                                <span class="font14 text-bold">70 Kmpl</span>
                                            </span>
                                        </a>
                                    </li>
                                    <li>
                                        <a href="/m/new/bike-search/?mileage=2">
                                            <span class="mileage-title-box font14">Between
                                            </span>
                                            <span class="mileage-amount-box font14 text-bold">
                                                <span class="">70</span>
                                                <span class="mileage-amount-text-box">Kmpl</span>
                                                <span class="">- 50</span>
                                                <span class="mileage-amount-text-box">Kmpl</span>
                                            </span>
                                        </a>
                                    </li>
                                    <li>
                                        <a href="/m/new/bike-search/?mileage=3">
                                            <span class="mileage-title-box font14">Between
                                            </span>
                                            <span class="mileage-amount-box font14 text-bold">
                                                <span class="">50</span>
                                                <span class="mileage-amount-text-box">Kmpl</span>
                                                <span class="">- 30</span>
                                                <span class="mileage-amount-text-box">Kmpl</span>
                                            </span>
                                        </a>
                                    </li>
                                    <li>
                                        <a href="/m/new/bike-search/?mileage=4">
                                            <span class="mileage-title-box font14">Upto
                                            </span>
                                            <span class="mileage-amount-box">
                                                <span class="font14 text-bold">30 Kmpl</span>
                                            </span>
                                        </a>
                                    </li>
                                </ul>
                            </div>
                        </div>
                        <div class="bw-tabs-data hide" id="discoverStyle">
                            <div class="style-type-container">
                                <ul class="text-center">
                                    <li>
                                        <a href="/m/new/bike-search/?ridestyle=5">
                                            <span class="style-type">
                                                <span class="styletypesprite style-scooters"></span>
                                            </span>
                                            <span class="style-type-title">Scooters</span>
                                        </a>
                                    </li>
                                    <li>
                                        <a href="/m/new/bike-search/?ridestyle=3">
                                            <span class="style-type">
                                                <span class="styletypesprite style-street"></span>
                                            </span>
                                            <span class="style-type-title">Street</span>
                                        </a>
                                    </li>
                                    <li>
                                        <a href="/m/new/bike-search/?ridestyle=1">
                                            <span class="style-type">
                                                <span class="styletypesprite style-cruiser"></span>
                                            </span>
                                            <span class="style-type-title">Cruiser</span>
                                        </a>
                                    </li>
                                    <li>
                                        <a href="/m/new/bike-search/?ridestyle=2">
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
            <!--  Upcoming, New Launches and Top Selling code starts here -->
            <div class="container <%= ((mctrlMostPopularBikes.FetchedRecordsCount + mctrlMostPopularBikes.FetchedRecordsCount + mctrlMostPopularBikes.FetchedRecordsCount) > 0 )?"margin-bottom20":"hide" %> ">
                <div class="grid-12 alpha omega">
                    <h2 class="font18 text-center margin-top20 margin-bottom10">Featured bikes</h2>
                    <div class="featured-bikes-panel content-box-shadow padding-bottom15">
                        <div class="bw-tabs-panel">
                            <div class="bw-tabs bw-tabs-flex">
                                <ul>
                                    <li class="active" style="<%= (mctrlMostPopularBikes.FetchedRecordsCount > 0)?"": "display:none" %>" data-tabs="mctrlMostPopularBikes">Most Popular</li>
                                    <li style="<%= (mctrlNewLaunchedBikes.FetchedRecordsCount > 0)?"": "display:none" %>" data-tabs="mctrlNewLaunchedBikes">New launches</li>
                                    <li style="<%= (mctrlUpcomingBikes.FetchedRecordsCount > 0)?"": "display:none" %>" data-tabs="mctrlUpcomingBikes">Upcoming </li>
                                </ul>
                            </div>
                            <div class="grid-12 alpha omega">
                                <div class="bw-tabs-data features-bikes-container" id="mctrlMostPopularBikes">
                                    <div class="swiper-container card-container">
                                        <div class="swiper-wrapper discover-bike-carousel">
                                            <BW:MMostPopularBikes PageId="4" runat="server" ID="mctrlMostPopularBikes" />
                                        </div>
                                    </div>
                                    
                                </div>
                                <div class="padding-left10 view-all-btn-container margin-top10">
                            <a href="/m/best-bikes-in-india/" title="Popular Bikes in India" class="btn view-all-target-btn">View all bikes<span class="bwmsprite teal-right"></span></a>
                               </div>
                            </div>
                            <div class="bw-tabs-data hide features-bikes-container" id="mctrlNewLaunchedBikes">
                                <div class="swiper-container card-container">
                                    <div class="swiper-wrapper discover-bike-carousel">
                                        <BW:MNewLaunchedBikes PageId="4" runat="server" ID="mctrlNewLaunchedBikes" />
                                    </div>
                                </div>
                                <div class="padding-left10 view-all-btn-container margin-top10">
                            <a href="/m/new-bike-launches/" title="New Bike Launches in India" class="btn view-all-target-btn">View all launches<span class="bwmsprite teal-right"></span></a>
                               </div>
                            </div>
                            <div class="bw-tabs-data hide features-bikes-container" id="mctrlUpcomingBikes">
                                <div class="swiper-container card-container">
                                    <div class="swiper-wrapper discover-bike-carousel">
                                        <BW:MUpcomingBikes runat="server" ID="mctrlUpcomingBikes" />                                        
                                    </div>
                                </div>
                                <div class="padding-left10 view-all-btn-container margin-top10">
                            <a href="/m/upcoming-bikes/" title="Upcoming Bikes in India" class="btn view-all-target-btn">View all bikes<span class="bwmsprite teal-right"></span></a>
                               </div>
                            </div>
                        </div>
                        <div class="clear"></div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section class="lazy home-getFinalPrice-banner" data-original="https://imgd3.aeplcdn.com/0x0/bw/static/landing-banners/m/onroad-price-banner.jpg">
            <!--  Get Final Price code starts here -->
            <BW:MOnRoadPricequote PageId="5" ID="MOnRoadPricequote" runat="server" />
        </section>
        <!-- Get Final Price code Ends here -->
        <% if (Ad320x150_II)
           { %>
        <section>
            <!-- #include file="/ads/Ad320x150_Second.aspx" -->
        </section>
        <% } %>
        <% if (ctrlBestBikes != null)
           { %>
        <section>
            <div class="container">
                <h2 class="font18 text-center margin-top20 margin-bottom10">Best bikes of <%= ctrlBestBikes.PrevMonthDate %></h2>
                <div class="box-shadow bg-white padding-top10 padding-bottom10">
                    <BW:BestBikes runat="server" ID="ctrlBestBikes" />
                </div>
            </div>
        </section>
        <% } %>
        <section>
            <!--  Compare section code starts here -->
            <BW:CompareBike ID="ctrlCompareBikes" runat="server" />
        </section>
        <!-- Compare code Ends here -->

        <section>
            <div class="container">
                <h2 class="font18 text-center margin-top20 margin-bottom10">Find used bikes</h2>
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
            <!--  News, reviews and videos code starts here -->
            <div class="container">
                <div class="grid-12 alpha omega margin-bottom20">
                    <h2 class="font18 text-center margin-top20 margin-bottom10">Latest updates</h2>
                    <div class="content-box-shadow">
                        <div class="bw-tabs-panel article-control padding-bottom20">
                            <div class="bw-tabs">
                                <div class="text-center <%= reviewTabsCnt > 2 ? "" : ( reviewTabsCnt > 1 ? "margin-top30 margin-bottom30" : "margin-top10") %>">
                                    <div class="bw-tabs <%= reviewTabsCnt > 2 ? "bw-tabs-flex" : ( reviewTabsCnt > 1 ? "home-tabs" : "hide") %>" id="reviewCount">
                                        <ul>
                                            <li class="<%= isNewsActive ? "active" : "hide" %>" style="<%= (Convert.ToInt32(ctrlNews.FetchedRecordsCount) > 0) ? "": "display:none;" %>" data-tabs="ctrlNews">News</li>
                                            <li class="<%= isExpertReviewActive ? "active" : "hide" %>" style="<%= (Convert.ToInt32(ctrlExpertReviews.FetchedRecordsCount) > 0) ? "": "display:none;" %>" data-tabs="ctrlExpertReviews">Expert Reviews</li>
                                            <li class="<%= isVideoActive ? "active" : "hide" %>" style="<%= (Convert.ToInt32(ctrlVideos.FetchedRecordsCount) > 0) ? "": "display:none;" %>" data-tabs="ctrlVideos">Videos</li>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                            <div class="bw-tabs-data margin-right20 margin-left20" id="ctrlNews">
                                <%if (!isNewsZero)
                                  { %>
                                <BW:News runat="server" ID="ctrlNews" />
                                <% } %>
                            </div>
                            <div class="bw-tabs-data hide margin-right20 margin-left20" id="ctrlExpertReviews">
                                <%if (!isExpertReviewZero)
                                  { %>
                                <BW:ExpertReviews runat="server" ID="ctrlExpertReviews" />
                                <% } %>
                            </div>
                            <div class="bw-tabs-data hide margin-right20 margin-left20" id="ctrlVideos">
                                <%if (!isVideoZero)
                                  { %>
                                <BW:Videos runat="server" ID="ctrlVideos" />
                                <% } %>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <!--  News, reviews and videos code ends here -->
        
        <noscript id="asynced-css">
            <link rel="stylesheet" type="text/css" href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-common-btf.css?<%=staticFileVersion %>" />
        </noscript>
        <!-- #include file="/includes/footerbw_mobile.aspx" -->
        <script type="text/javascript" defer src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : "" %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" defer src="<%= staticUrl != "" ? "https://st.aeplcdn.com" + staticUrl : "" %>/m/src/Plugins.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" defer src="<%= staticUrl != "" ? "https://st.aeplcdn.com" + staticUrl : "" %>/m/src/common.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" defer src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : "" %>/m/src/chosen-jquery-min-mobile.js?<%= staticFileVersion %>"></script>

        <script type="text/javascript">
            ga_pg_id = '1';
            
            docReady(function () {
                $("#global-search").hide();
                if ('<%=isNewsActive%>' == "False") $("#ctrlNews").addClass("hide");
                if ('<%=isExpertReviewActive%>' == "False") $("#ctrlExpertReviews").addClass("hide");
                if ('<%=isVideoActive%>' == "False") $("#ctrlVideos").addClass("hide");
                $("img.lazy").lazyload();
                $("#newBikeList").on("click", function () {
                    $('#global-search').trigger("click");
                });
            });

        </script>
        <link href="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : "" %>/css/chosen.min.css?<%= staticFileVersion %>" type="text/css" rel="stylesheet" />
        <!-- #include file="/includes/fontBW_Mobile.aspx" -->

    </form>
</body>
</html>
