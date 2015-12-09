<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.New.Default" %>

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
<%@ Register Src="~/controls/PopupWidget.ascx" TagName="PopupWidget" TagPrefix="BW" %>

<!doctype html>
<html>
<head>
    <%
        title = "New Bikes - Bikes Reviews, Photos, Specs, Features, Tips & Advices - BikeWale";
        keywords = "new bikes, new bikes prices, new bikes comparisons, bikes dealers, on-road price, bikes research, bikes india, Indian bikes, bike reviews, bike photos, specs, features, tips & advices";
        description = "New bikes in India. Search for the right new bikes for you, know accurate on-road price and discounts. Compare new bikes and find dealers.";
        alternate = "http://www.bikewale.com/m/new/";
        AdId = "1395986297721";
        AdPath = "/1017752/BikeWale_New_";
        canonical = "http://www.bikewale.com/new/";
    %>
    <%
        isTransparentHeader = true;
        isAd970x90Shown = false;
        isAd970x90BottomShown = false;
        isAd976x400FirstShown = true;
        isAd976x400SecondShown = true;
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
                    <p class="font20">View all bikes under one roof</p>
                </div>
            </div>
        </header>

        <section class="container">
            <!--  Brand-Budget-Mileage-Style section code starts here -->
            <div class="grid-12">
                <div class="bw-tabs-panel brand-budget-mileage-style-wrapper content-box-shadow margin-minus50">
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
                            <a href="javascript:void(0)" id="view-brandType" class="view-more-btn font16">View <span>More</span> Brands</a>
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
                            <ul class="brand-style-moreBtn styleTypeMore hide text-center">
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
                                <li>
                                    <a href="/new/search.aspx#ridestyle=5">
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
        <section class="container <%= ((ctrlMostPopularBikes.FetchedRecordsCount + ctrlMostPopularBikes.FetchedRecordsCount + ctrlMostPopularBikes.FetchedRecordsCount) > 0 )?"":"hide" %> ">
            <!--  Discover bikes section code starts here -->
            <div class="grid-12">
                <h2 class="text-bold text-center margin-top50 margin-bottom30 font28">Featured bikes</h2>
                <div class="bw-tabs-panel newbike-discover-bike-container content-box-shadow">
                    <div class="bw-tabs bw-tabs-flex">
                        <ul>
                            <li class="active" style="<%= (ctrlMostPopularBikes.FetchedRecordsCount > 0)?"": "display:none" %>" data-tabs="ctrlMostPopularBikes">Most Popular</li>
                            <li style="<%= (ctrlNewLaunchedBikes.FetchedRecordsCount > 0)?"": "display:none" %>" data-tabs="ctrlNewLaunchedBikes">New launches</li>
                            <li style="<%= (ctrlUpcomingBikes.FetchedRecordsCount > 0)?"": "display:none" %>" data-tabs="ctrlUpcomingBikes">Upcoming </li>
                        </ul>
                    </div>
                    <div class="bw-tabs-data <%= (ctrlMostPopularBikes.FetchedRecordsCount > 0)?"":"hide" %>" id="ctrlMostPopularBikes">
                        <div class="jcarousel-wrapper discover-bike-carousel">
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
                        <div class="jcarousel-wrapper discover-bike-carousel">
                            <div class="jcarousel">
                                <ul>
                                    <BW:NewLaunchedBikes PageId="5" runat="server" ID="ctrlNewLaunchedBikes" />
                                    <!-- New Launched Bikes Control-->
                                </ul>
                            </div>
                            <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev"></a></span>
                            <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next"></a></span>
                        </div>
                    </div>

                    <div class="bw-tabs-data hide <%= (ctrlUpcomingBikes.FetchedRecordsCount > 0)?"":"hide" %>" id="ctrlUpcomingBikes">
                        <div class="jcarousel-wrapper discover-bike-carousel">
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
            <h2 class="text-bold text-center margin-top50 margin-bottom30 font28">Compare now</h2>
            <div class="content-box-shadow">
                <BW:CompareBikes ID="ctrlCompareBikes" runat="server" />
            </div>
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
                <h2 class="text-bold text-center margin-top50 margin-bottom30 font28">Tools you may need</h2>
                <div class="bw-tabs-panel tools-may-need-wrapper content-box-shadow">
                    <div class="bw-tabs bw-tabs-flex">
                        <ul class="tools-may-need-UL">
                            <li data-tabs="getFinal-price" class="active">On-Road Price</li>
                            <li data-tabs="locate-dealer" class="">Locate A Dealer</li>
                            <li data-tabs="calculate-emi" class="">Calculate EMI's</li>
                        </ul>
                    </div>
                    <div class="bw-tabs-data" id="getFinal-price">
                        <div class="getFinal-price-container text-center margin-bottom30">
                            <div class="margin-bottom40">
                                <span class="bw-circle-icon final-price-logo"></span>
                            </div>
                            <p class="font16 margin-bottom30">Get final price of the bike without filling any forms</p>
                            <BW:NBOnRoadPrice PageId="4" Id="NBOnRoadPrice" runat="server" />
                        </div>
                    </div>
                    <div class="bw-tabs-data hide" id="locate-dealer">
                        <BW:LocateDealer Id="ctrlLocateDealer" runat="server" />
                    </div>
                    <div class="bw-tabs-data hide" id="calculate-emi">
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
                    <h2 class="text-bold text-center margin-top50 margin-bottom30 font28">Latest updates from the industry</h2>
                    <div class="bw-tabs-panel content-box-shadow margin-bottom30">
                        <div class="text-center <%= reviewTabsCnt > 2 ? "" : ( reviewTabsCnt > 1 ? "margin-top30 margin-bottom30" : "margin-top10") %>">
                            <div class="bw-tabs <%= reviewTabsCnt > 2 ? "bw-tabs-flex" : ( reviewTabsCnt > 1 ? "home-tabs" : "hide") %>" id="reviewCount">
                                <ul>
                                    <li class="<%= isNewsActive ? "active" : "hide" %>" style="<%= (Convert.ToInt32(ctrlNews.FetchedRecordsCount) > 0) ? "": "display:none;" %>" data-tabs="ctrlNews">News</li>
                                    <li class="<%= isExpertReviewActive ? "active" : "hide" %>" style="<%= (Convert.ToInt32(ctrlExpertReviews.FetchedRecordsCount) > 0) ? "": "display:none;" %>" data-tabs="ctrlExpertReviews">Expert Reviews</li>
                                    <li class="<%= isVideoActive ? "active" : "hide" %>" style="<%= (Convert.ToInt32(ctrlVideos.FetchedRecordsCount) > 0) ? "": "display:none;" %>" data-tabs="ctrlVideos">Videos</li>
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

        <BW:PopupWidget Id="NBPopupWidget" runat="server" />
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
