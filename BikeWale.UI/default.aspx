<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Default" EnableViewState="false" %>

<%@ Register Src="~/controls/News_new.ascx" TagName="News" TagPrefix="BW" %>
<%@ Register Src="~/controls/ExpertReviews.ascx" TagName="ExpertReviews" TagPrefix="BW" %>
<%@ Register Src="~/controls/VideosControl.ascx" TagName="Videos" TagPrefix="BW" %>
<%@ Register Src="~/controls/ComparisonMin.ascx" TagName="CompareBikes" TagPrefix="BW" %>
<%@ Register Src="~/controls/PopularUsedBikes.ascx" TagName="PopularUsedBikes" TagPrefix="BW" %>
<%@ Register Src="~/controls/OnRoadPriceQuote.ascx" TagName="OnRoadPriceQuote" TagPrefix="BW" %>

<!doctype html>
<html>
<head>
    <%
        title = "New Bikes, Used Bikes, Bike Prices, Reviews & Photos in India";
        keywords = "new bikes, used bikes, buy used bikes, sell your bike, bikes prices, reviews, photos, news, compare bikes, Instant Bike On-Road Price";
        description = "BikeWale - India's favourite bike portal. Find new and used bikes, buy or sell your bikes, compare new bikes prices & values.";
        AdPath = "/1017752/BikeWale_HomePage_";
        AdId = "1395985604192";
        alternate = "http://www.bikewale.com/m/";
        canonical = "http://www.bikewale.com/";
        isAd970x90Shown = false;
        isAd970x90BottomShown = false;
        isAd976x400FirstShown = true;
        isAd976x400SecondShown = true;
        isAd976x204 = true;
        PopupWidget.Visible = true;
    %>
    <!-- #include file="/includes/headscript.aspx" -->
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/home.css?<%= staticFileVersion%>" rel="stylesheet" type="text/css">

    <%  isTransparentHeader = true;
    %>
</head>
<body class="bg-white">
    <form runat="server">
        <!-- #include file="/includes/headBW.aspx" -->
        <header class="home-top-banner">
            <div class="container">
                <div class="welcome-box">
                    <h1>Find your bike</h1>
                    <p class="font20">Get Exclusive Offers, Discounts and Freebies on your Bike Purchase</p>
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
        <% if (isAd976x400FirstShown)
           { %>
        <section>
            <!-- #include file="/ads/Ad976x400_First.aspx" -->
        </section>
        <% } %>
        <section class="bg-white">
            <!--  Discover your bike code starts here -->
            <div class="container">
                <div class="grid-12">
                    <h2 class="text-bold text-center margin-top40 margin-bottom20 font22">Discover your bike</h2>
                    <div class="bw-tabs-panel brand-budget-mileage-style-wrapper">
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
                                                	    <span class="brandlogosprite brand-<%# DataBinder.Eval(Container.DataItem, "MaskingName") %>"></span>
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
                                                        <span class="brandlogosprite brand-<%# DataBinder.Eval(Container.DataItem, "MaskingName") %>"></span>
                                                    </span>
                                                    <span class="brand-type-title"><%# DataBinder.Eval(Container.DataItem, "MakeName") %></span>
                                                </a>
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                            </div>
                            <div class="view-brandType text-center padding-top10 padding-bottom30">
                                <a href="#" id="view-brandType" class="view-more-btn btn btn-tertiary">View <span>more</span> brands</a>
                            </div>
                        </div>
                        <div class="bw-tabs-data hide" id="discoverBudget">
                            <div class="budget-container margin-bottom20">
                                <ul class="text-center">
                                    <li>
                                        <a href="/new/search.aspx#budget=0-50000">
                                            <span class="budget-title-box font16">Upto
                                            </span>
                                            <span class="budget-amount-box">
                                                <span class="bwsprite inr-budget"></span>
                                                <span class="font20">50,000</span>
                                            </span>
                                        </a>
                                    </li>
                                    <li>
                                        <a href="/new/search.aspx#budget=50000-100000">
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
                                        <a href="/new/search.aspx#budget=100000-250000">
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
                                        <a href="/new/search.aspx#budget=250000-">
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
                                        <a href="/new/search.aspx#mileage=1">
                                            <span class="mileage-title-box font16">Above
                                            </span>
                                            <span class="mileage-amount-box font20">
                                                <span>70 <span class="font16">Kmpl</span></span>
                                            </span>
                                        </a>
                                    </li>
                                    <li>
                                        <a href="/new/search.aspx#mileage=2">
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
                                        <a href="/new/search.aspx#mileage=3">
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
                                        <a href="/new/search.aspx#mileage=4">
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
                                        <a href="/new/search.aspx#ridestyle=5">
                                            <span class="style-type">
                                                <span class="styletypesprite style-scooters"></span>
                                            </span>
                                            <span class="style-type-title">Scooters</span>
                                        </a>
                                    </li>
                                    <li>
                                        <a href="/new/search.aspx#ridestyle=3">
                                            <span class="style-type">
                                                <span class="styletypesprite style-street"></span>
                                            </span>
                                            <span class="style-type-title">Street</span>
                                        </a>
                                    </li>
                                    <li>
                                        <a href="/new/search.aspx#ridestyle=1">
                                            <span class="style-type">
                                                <span class="styletypesprite style-cruiser"></span>
                                            </span>
                                            <span class="style-type-title">Cruiser</span>
                                        </a>
                                    </li>
                                    <li>
                                        <a href="/new/search.aspx#ridestyle=2">
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
        <!--  Ends here -->
        <section class="lazy home-getFinalPrice-banner" data-original="http://imgd3.aeplcdn.com/0x0/bw/static/landing-banners/d/get-final-price-banner.jpg">
            <BW:OnRoadPriceQuote ID="ctrlOnRoadPriceQuote" PageId="1" runat="server" />
        </section>
        <% if (isAd976x400SecondShown)
           { %>
        <section>
            <!-- #include file="/ads/Ad976x400_Second.aspx" -->
        </section>
        <%} %>
        <section class="margin-bottom50">
            <!--  Compare section code starts here -->
            <div class="container">
                <h2 class="text-bold text-center margin-top40 margin-bottom20 font22">Compare bikes</h2>
                <BW:CompareBikes ID="ctrlCompareBikes" runat="server" />
            </div>
        </section>
        <!-- Ends here -->
        <section class="bg-light-grey <%= (ctrlPopularUsedBikes.FetchedRecordsCount > 0)?"":"hide" %>">
            <!--  Used Bikes code starts here -->
            <BW:PopularUsedBikes runat="server" ID="ctrlPopularUsedBikes" />
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
        <section class="container <%= reviewTabsCnt == 0 ? "hide" : "" %>">
            <!--  News Bikes latest updates code starts here -->
            <div class="newBikes-latest-updates-container">
                <div class="grid-12">
                    <h2 class="text-bold text-center margin-top40 margin-bottom20 font22">Latest updates from the bike industry</h2>
                    <div class="bw-tabs-panel margin-bottom30 ">
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
        <!-- Ends here -->
        <!-- #include file="/includes/footerBW.aspx" -->
        <!-- #include file="/includes/footerscript.aspx" -->
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st.aeplcdn.com" + staticUrl : "" %>/src/home.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st.aeplcdn.com" + staticUrl : "" %>/src/common/chosen.jquery.min.js?<%= staticFileVersion %>"></script>
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
