<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Default" %>
<%@ Register Src="~/controls/News_new.ascx" TagName="News" TagPrefix="BW" %>
<%@ Register Src="~/controls/ExpertReviews.ascx" TagName="ExpertReviews" TagPrefix="BW" %>
<%@ Register Src="~/controls/VideosControl.ascx" TagName="Videos" TagPrefix="BW" %>
<%@ Register Src="~/controls/ComparisonMin.ascx" TagName="CompareBikes" TagPrefix="BW" %>
<%@ Register Src="~/controls/PopularUsedBikes.ascx" TagName="PopularUsedBikes" TagPrefix="BW" %>
<%@ Register Src="~/controls/OnRoadPriceQuote.ascx" TagName="OnRoadPriceQuote" TagPrefix="BW" %>
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
    %>
    <!-- #include file="/includes/headscript.aspx" -->
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/home.css?<%= staticFileVersion%>" rel="stylesheet" type="text/css">
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/chosen.min.css?<%= staticFileVersion %>" rel="stylesheet" />
            
    <%  isTransparentHeader = true;   %>
</head>
<body class="bg-white">
<form runat="server">    
    <!-- #include file="/includes/headBW.aspx" -->    
    <header class="home-top-banner">
        <div class="container">
            <div class="welcome-box">
                <h1 class="text-uppercase margin-bottom10">FIND YOUR RIDE</h1>
                <p class="font20">Get Exclusive Offers, Discounts and Freebies on your Bike Purchase</p>
                <div class="margin-top60">
                    <div>
                        <div class="bike-search-container">
                            <div class="bike-search new-bike-search position-rel">
                                <input type="text" placeholder="Search your bike here, e.g. Honda Activa " id="newBikeList" autocomplete="off" tabindex="1">
                                <span class="fa fa-spinner fa-spin position-abt pos-right10 pos-top15 text-black" style="display: none"></span>
                            </div>
                            <div class="findBtn">
                                <button id="btnSearch" class="btn btn-orange btn-md font18" tabindex="2">Search</button>
                            </div>
                            <div class="clear"></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </header>

    <section class="bg-white">
        <!--  Discover your bike code starts here -->
        <div class="container">
            <div class="grid-12">
                <h2 class="text-bold text-center margin-top50 margin-bottom30 font28">Discover your bike</h2>
                <div class="bw-tabs-panel brand-budget-mileage-style-wrapper">
                    <div class="bw-tabs bw-tabs-flex">
                        <ul class="brand-budget-mileage-style-UL">
                            <li class="active" data-tabs="discoverBrand">Brand</li>
                            <li data-tabs="discoverBudget">Budget</li>
                            <li data-tabs="discoverMileage">Mileage</li>
                            <li data-tabs="discoverStyle">Style</li>
                        </ul>
                    </div>
                    <div class="bw-tabs-data" id="discoverBrand">
                        <div class="brand-type-container">
                            <ul class="text-center">
                                <li>
                                    <a href="/honda-bikes/">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-honda" data-original="http://img.aeplcdn.com/bikewaleimg/images/brand-type-sprite.png?<%= staticFileVersion %>"></span>
                                        </span>
                                        <span class="brand-type-title">Honda</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/bajaj-bikes/">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-bajaj" data-original="http://img.aeplcdn.com/bikewaleimg/images/brand-type-sprite.png?<%= staticFileVersion %>"></span>
                                        </span>
                                        <span class="brand-type-title">Bajaj</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/hero-bikes/">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-hero" data-original="http://img.aeplcdn.com/bikewaleimg/images/brand-type-sprite.png?<%= staticFileVersion %>"></span>
                                        </span>
                                        <span class="brand-type-title">Hero</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/tvs-bikes/">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-tvs" data-original="http://img.aeplcdn.com/bikewaleimg/images/brand-type-sprite.png?<%= staticFileVersion %>"></span>
                                        </span>
                                        <span class="brand-type-title">TVS</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/royalenfield-bikes/">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-royal" data-original="http://img.aeplcdn.com/bikewaleimg/images/brand-type-sprite.png?<%= staticFileVersion %>"></span>
                                        </span>
                                        <span class="brand-type-title">Royal Enfield</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/yamaha-bikes/">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-yamaha" data-original="http://img.aeplcdn.com/bikewaleimg/images/brand-type-sprite.png?<%= staticFileVersion %>"></span>
                                        </span>
                                        <span class="brand-type-title">Yamaha</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/suzuki-bikes/">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-suzuki" data-original="http://img.aeplcdn.com/bikewaleimg/images/brand-type-sprite.png?<%= staticFileVersion %>"></span>
                                        </span>
                                        <span class="brand-type-title">Suzuki</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/ktm-bikes/">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-ktm" data-original="http://img.aeplcdn.com/bikewaleimg/images/brand-type-sprite.png?<%= staticFileVersion %>"></span>
                                        </span>
                                        <span class="brand-type-title">KTM</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/mahindra-bikes/">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-mahindra" data-original="http://img.aeplcdn.com/bikewaleimg/images/brand-type-sprite.png?<%= staticFileVersion %>"></span>
                                        </span>
                                        <span class="brand-type-title">Mahindra</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/harleydavidson-bikes/">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-harley" data-original="http://img.aeplcdn.com/bikewaleimg/images/brand-type-sprite.png?<%= staticFileVersion %>"></span>
                                        </span>
                                        <span class="brand-type-title">Harley Davidson</span>
                                    </a>
                                </li>
                            </ul>
                            <div class="brand-bottom-border border-solid-top margin-left20 margin-right20 hide">
                            </div>
                            <ul class="brand-style-moreBtn padding-top25 brandTypeMore hide margin-left5">
                                <li>
                                    <a href="/aprilia-bikes/">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-aprilia" data-original="http://img.aeplcdn.com/bikewaleimg/images/brand-type-sprite.png?<%= staticFileVersion %>"></span>
                                        </span>
                                        <span class="brand-type-title">Aprilia</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/benelli-bikes/">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-benelli" data-original="http://img.aeplcdn.com/bikewaleimg/images/brand-type-sprite.png?<%= staticFileVersion %>"></span>
                                        </span>
                                        <span class="brand-type-title">Benelli</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/bmw-bikes/">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-bmw" data-original="http://img.aeplcdn.com/bikewaleimg/images/brand-type-sprite.png?<%= staticFileVersion %>"></span>
                                        </span>
                                        <span class="brand-type-title">BMW</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/ducati-bikes/">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-ducati" data-original="http://img.aeplcdn.com/bikewaleimg/images/brand-type-sprite.png?<%= staticFileVersion %>"></span>
                                        </span>
                                        <span class="brand-type-title">Ducati</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/heroelectric-bikes/">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-hero-elec" data-original="http://img.aeplcdn.com/bikewaleimg/images/brand-type-sprite.png?<%= staticFileVersion %>"></span>
                                        </span>
                                        <span class="brand-type-title">Hero Electric</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/hyosung-bikes/">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-hyosung" data-original="http://img.aeplcdn.com/bikewaleimg/images/brand-type-sprite.png?<%= staticFileVersion %>"></span>
                                        </span>
                                        <span class="brand-type-title">Hyosung</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/indian-bikes/">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-indian" data-original="http://img.aeplcdn.com/bikewaleimg/images/brand-type-sprite.png?<%= staticFileVersion %>"></span>
                                        </span>
                                        <span class="brand-type-title">Indian</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/kawasaki-bikes/">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-kawasaki" data-original="http://img.aeplcdn.com/bikewaleimg/images/brand-type-sprite.png?<%= staticFileVersion %>"></span>
                                        </span>
                                        <span class="brand-type-title">Kawasaki</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/lml-bikes/">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-lml" data-original="http://img.aeplcdn.com/bikewaleimg/images/brand-type-sprite.png?<%= staticFileVersion %>"></span>
                                        </span>
                                        <span class="brand-type-title">LML</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/motoguzzi-bikes/">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-guzzi" data-original="http://img.aeplcdn.com/bikewaleimg/images/brand-type-sprite.png?<%= staticFileVersion %>"></span>
                                        </span>
                                        <span class="brand-type-title">Moto Guzzi</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/triumph-bikes/">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-triumph" data-original="http://img.aeplcdn.com/bikewaleimg/images/brand-type-sprite.png?<%= staticFileVersion %>"></span>
                                        </span>
                                        <span class="brand-type-title">Triumph</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/vespa-bikes/">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-vespa" data-original="http://img.aeplcdn.com/bikewaleimg/images/brand-type-sprite.png?<%= staticFileVersion %>"></span>
                                        </span>
                                        <span class="brand-type-title">Vespa</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/yo-bikes/">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-yo" data-original="http://img.aeplcdn.com/bikewaleimg/images/brand-type-sprite.png?<%= staticFileVersion %>"></span>
                                        </span>
                                        <span class="brand-type-title">Yo</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/mv-agusta-bikes/">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-agusta" data-original="http://img.aeplcdn.com/bikewaleimg/images/brand-type-sprite.png?<%= staticFileVersion %>"></span>
                                        </span>
                                        <span class="brand-type-title">MV Agusta</span>
                                    </a>
                                </li>
                            </ul>
                        </div>
                        <div class="view-brandType text-center padding-top10 padding-bottom30">
                            <a href="#" id="view-brandType" class="view-more-btn font16">View <span>More</span> Brands</a>
                        </div>
                    </div>
                    <div class="bw-tabs-data hide" id="discoverBudget">
                        <div class="budget-container margin-bottom20">
                            <ul class="text-center">
                                <li>
                                    <a href="/new/search.aspx#budget=0-50000">
                                        <span class="budget-title-box font16">Upto
                                        </span>
                                        <span class="budget-amount-box font20">
                                            <span class="fa fa-rupee"></span>
                                            <span class="font24">50,000</span>
                                        </span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/new/search.aspx#budget=50000-100000">
                                        <span class="budget-title-box font16">Between
                                        </span>
                                        <span class="budget-amount-box font20">
                                            <span class="fa fa-rupee"></span>
                                            <span class="font24">50,000 - </span>
                                            <span class="fa fa-rupee"></span>
                                            <span class="font24">1</span>
                                            <span class="budget-amount-text-box font16">Lakhs</span>
                                        </span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/new/search.aspx#budget=100000-250000">
                                        <span class="budget-title-box font16">Between
                                        </span>
                                        <span class="budget-amount-box font20">
                                            <span class="fa fa-rupee"></span>
                                            <span class="font24">1</span>
                                            <span class="budget-amount-text-box font16">Lakhs</span>
                                            <span class="font24">- </span>
                                            <span class="fa fa-rupee"></span>
                                            <span>2.5</span>
                                            <span class="budget-amount-text-box font16">Lakhs</span>
                                        </span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/new/search.aspx#budget=250000-">
                                        <span class="budget-title-box font16">Above
                                        </span>
                                        <span class="budget-amount-box font20">
                                            <span class="fa fa-rupee"></span>
                                            <span class="font24">2.5</span>
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
                                        <span class="mileage-amount-box font24">
                                            <span>70 <span class="font16">Kmpl</span></span>
                                        </span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/new/search.aspx#mileage=2">
                                        <span class="mileage-title-box font16">Between
                                        </span>
                                        <span class="mileage-amount-box font24">
                                            <span>70</span>
                                            <span class="mileage-amount-text-box font16">Kmpl</span>
                                            <span>- 50</span>
                                            <span class="mileage-amount-text-box font16">Kmpl</span>
                                        </span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/new/search.aspx#mileage=3">
                                        <span class="mileage-title-box font16">Between
                                        </span>
                                        <span class="mileage-amount-box font24">
                                            <span>50</span>
                                            <span class="mileage-amount-text-box font16">Kmpl</span>
                                            <span>- 30</span>
                                            <span class="mileage-amount-text-box font16">Kmpl</span>
                                        </span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/new/search.aspx#mileage=4">
                                        <span class="mileage-title-box font16">Upto
                                        </span>
                                        <span class="mileage-amount-box font24">
                                            <span>30</span>
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
    <section class="lazy home-getFinalPrice-banner" data-original="http://img.aeplcdn.com/bikewaleimg/images/get-final-price-banner.jpg" >
      <BW:OnRoadPriceQuote ID="ctrlOnRoadPriceQuote" PageId="1" runat="server"/>
    </section>

    <section class="margin-bottom50">
        <!--  Compare section code starts here -->
        <div class="container">
        <h2 class="text-bold text-center margin-top50 margin-bottom30 font28">Compare now</h2>
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
                    <h2 class="text-bold text-center margin-top50 margin-bottom30 font28">Latest updates from the industry</h2>
                    <div class="bw-tabs-panel margin-bottom30 ">
                        <div class="text-center <%= reviewTabsCnt > 2 ? "" : ( reviewTabsCnt > 1 ? "margin-top30 margin-bottom30" : "margin-top10") %>">
                            <div class="bw-tabs <%= reviewTabsCnt > 2 ? "bw-tabs-flex" : ( reviewTabsCnt > 1 ? "home-tabs" : "hide") %>" id="reviewCount">
                                <ul>
                                    <li class="<%= isNewsActive ? "active" : "hide" %>" style="<%= (Convert.ToInt32(ctrlNews.FetchedRecordsCount) > 0) ? "": "display:none;" %>" data-tabs="ctrlNews">News</li>
                                    <li class="<%= isExpertReviewActive ? "active" : "hide" %>" style="<%= (Convert.ToInt32(ctrlExpertReviews.FetchedRecordsCount) > 0) ? "": "display:none;" %>" data-tabs="ctrlExpertReviews">Expert Reviews</li>                                   
                                    <li class="<%= isVideoActive ? "active" : "hide" %>" style="<%= (Convert.ToInt32(ctrlVideos.FetchedRecordsCount) > 0) ? "": "display:none;" %>" data-tabs="ctrlVideos">Videos</li>
                                </ul>
                            </div>
                        </div>
                        <%if (!isNewsZero) { %>         <BW:News runat="server" ID="ctrlNews" />    <% } %>
                        <%if (!isExpertReviewZero) { %> <BW:ExpertReviews runat="server" ID="ctrlExpertReviews" />  <% } %>                         
                        <%if (!isVideoZero) { %>        <BW:Videos runat="server" ID="ctrlVideos" />    <% } %>
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
