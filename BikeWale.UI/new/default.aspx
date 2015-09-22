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
    <!-- #include file="/includes/headscript.aspx" -->
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/newbikes.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css">
</head>
<body class="bg-white">
    <form runat="server">
    <!-- #include file="/includes/headBW.aspx" -->
    <header class="new-bikes-top-banner">
        <div class="container">
            <div class="welcome-box">
                <h1 class="text-uppercase margin-bottom10">NEW BIKES</h1>
                <p class="font20">View every bike under one roof</p>
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
                                        <span class="brandlogosprite brand-honda"></span>
                                    </span>
                                    <span class="brand-type-title">Honda</span>
                                </a>
                            </li>
                            <li>
                                <a href="/bajaj-bikes/">
                                    <span class="brand-type">
                                        <span class="brandlogosprite brand-bajaj"></span>
                                    </span>
                                    <span class="brand-type-title">Bajaj</span>
                                </a>
                            </li>
                            <li>
                                <a href="/hero-bikes/">
                                    <span class="brand-type">
                                        <span class="brandlogosprite brand-hero"></span>
                                    </span>
                                    <span class="brand-type-title">Hero</span>
                                </a>
                            </li>
                            <li>
                                <a href="/tvs-bikes/">
                                    <span class="brand-type">
                                        <span class="brandlogosprite brand-tvs"></span>
                                    </span>
                                    <span class="brand-type-title">TVS</span>
                                </a>
                            </li>
                            <li>
                                <a href="/royalenfield-bikes/">
                                    <span class="brand-type">
                                        <span class="brandlogosprite brand-royal"></span>
                                    </span>
                                    <span class="brand-type-title">Royal Enfield</span>
                                </a>
                            </li>
                            <li>
                                <a href="/yamaha-bikes/">
                                    <span class="brand-type">
                                        <span class="brandlogosprite brand-yamaha"></span>
                                    </span>
                                    <span class="brand-type-title">Yamaha</span>
                                </a>
                            </li>
                            <li>
                                <a href="/suzuki-bikes/">
                                    <span class="brand-type">
                                        <span class="brandlogosprite brand-suzuki"></span>
                                    </span>
                                    <span class="brand-type-title">Suzuki</span>
                                </a>
                            </li>
                            <li>
                                <a href="/ktm-bikes/">
                                    <span class="brand-type">
                                        <span class="brandlogosprite brand-ktm"></span>
                                    </span>
                                    <span class="brand-type-title">KTM</span>
                                </a>
                            </li>
                            <li>
                                <a href="/mahindra-bikes/">
                                    <span class="brand-type">
                                        <span class="brandlogosprite brand-mahindra"></span>
                                    </span>
                                    <span class="brand-type-title">Mahindra</span>
                                </a>
                            </li>
                            <li>
                                <a href="/harleydavidson-bikes/">
                                    <span class="brand-type">
                                        <span class="brandlogosprite brand-harley"></span>
                                    </span>
                                    <span class="brand-type-title">Harley Davidson</span>
                                </a>
                            </li>
                        </ul>
                        <div class="brand-bottom-border border-solid-top margin-left20 margin-right20 hide">
                        </div>
                        <ul class="brand-style-moreBtn padding-top25 brandTypeMore hide text-center">
                            <li>
                                <a href="/aprilia-bikes/">
                                    <span class="brand-type">
                                        <span class="brandlogosprite brand-aprilia"></span>
                                    </span>
                                    <span class="brand-type-title">Aprilia</span>
                                </a>
                            </li>
                            <li>
                                <a href="/benelli-bikes/">
                                    <span class="brand-type">
                                        <span class="brandlogosprite brand-benelli"></span>
                                    </span>
                                    <span class="brand-type-title">Benelli</span>
                                </a>
                            </li>
                            <li>
                                <a href="/bmw-bikes/">
                                    <span class="brand-type">
                                        <span class="brandlogosprite brand-bmw"></span>
                                    </span>
                                    <span class="brand-type-title">BMW</span>
                                </a>
                            </li>
                            <li>
                                <a href="/ducati-bikes/">
                                    <span class="brand-type">
                                        <span class="brandlogosprite brand-ducati"></span>
                                    </span>
                                    <span class="brand-type-title">Ducati</span>
                                </a>
                            </li>
                            <li>
                                <a href="/heroelectric-bikes/">
                                    <span class="brand-type">
                                        <span class="brandlogosprite brand-hero-elec"></span>
                                    </span>
                                    <span class="brand-type-title">Hero Electric</span>
                                </a>
                            </li>
                            <li>
                                <a href="/hyosung-bikes/">
                                    <span class="brand-type">
                                        <span class="brandlogosprite brand-hyosung"></span>
                                    </span>
                                    <span class="brand-type-title">Hyosung</span>
                                </a>
                            </li>
                            <li>
                                <a href="/indian-bikes/">
                                    <span class="brand-type">
                                        <span class="brandlogosprite brand-indian"></span>
                                    </span>
                                    <span class="brand-type-title">Indian</span>
                                </a>
                            </li>
                            <li>
                                <a href="/kawasaki-bikes/">
                                    <span class="brand-type">
                                        <span class="brandlogosprite brand-kawasaki"></span>
                                    </span>
                                    <span class="brand-type-title">Kawasaki</span>
                                </a>
                            </li>
                            <li>
                                <a href="/lml-bikes/">
                                    <span class="brand-type">
                                        <span class="brandlogosprite brand-lml"></span>
                                    </span>
                                    <span class="brand-type-title">LML</span>
                                </a>
                            </li>
                            <li>
                                <a href="/motoguzzi-bikes/">
                                    <span class="brand-type">
                                        <span class="brandlogosprite brand-guzzi"></span>
                                    </span>
                                    <span class="brand-type-title">Moto Guzzi</span>
                                </a>
                            </li>
                            <li>
                                <a href="/triumph-bikes/">
                                    <span class="brand-type">
                                        <span class="brandlogosprite brand-triumph"></span>
                                    </span>
                                    <span class="brand-type-title">Triumph</span>
                                </a>
                            </li>
                            <li>
                                <a href="/vespa-bikes/">
                                    <span class="brand-type">
                                        <span class="brandlogosprite brand-vespa"></span>
                                    </span>
                                    <span class="brand-type-title">Vespa</span>
                                </a>
                            </li>
                            <li>
                                <a href="/yo-bikes/">
                                    <span class="brand-type">
                                        <span class="brandlogosprite brand-yo"></span>
                                    </span>
                                    <span class="brand-type-title">Yo</span>
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
                                <a href="/new/search.aspx#budget=100000-150000">
                                    <span class="budget-title-box font16">Between
                                    </span>
                                    <span class="budget-amount-box font20">
                                        <span class="fa fa-rupee"></span>
                                        <span class="font24">1</span>
                                        <span class="budget-amount-text-box font16">Lakhs</span>
                                        <span class="font24">- </span>
                                        <span class="fa fa-rupee"></span>
                                        <span>1.5</span>
                                        <span class="budget-amount-text-box font16">Lakhs</span>
                                    </span>
                                </a>
                            </li>
                            <li>
                                <a href="/new/search.aspx#budget=200000-">
                                    <span class="budget-title-box font16">Above
                                    </span>
                                    <span class="budget-amount-box font20">
                                        <span class="fa fa-rupee"></span>
                                        <span class="font24">2</span>
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

    <section class="container <%= ((ctrlMostPopularBikes.FetchedRecordsCount + ctrlMostPopularBikes.FetchedRecordsCount + ctrlMostPopularBikes.FetchedRecordsCount) > 0 )?"":"hide" %> ">
        <!--  Discover bikes section code starts here -->
        <div class="grid-12">
            <h2 class="text-bold text-center margin-top50 margin-bottom30">Discover your bike</h2>
            <div class="bw-tabs-panel newbike-discover-bike-container content-box-shadow">
                <div class="bw-tabs bw-tabs-flex">
                    <ul>
                        <li class="active" style="<%= (ctrlMostPopularBikes.FetchedRecordsCount > 0)?"":"display:none" %>" data-tabs="ctrlMostPopularBikes">Most Popular</li>
                        <li style="<%= (ctrlNewLaunchedBikes.FetchedRecordsCount > 0)?"":"display:none" %>"  data-tabs="ctrlNewLaunchedBikes">New launches</li>
                        <li style="<%= (ctrlUpcomingBikes.FetchedRecordsCount > 0)?"":"display:none" %>"  data-tabs="ctrlUpcomingBikes">Upcoming </li>
                    </ul>
                </div>
                <div class="bw-tabs-data <%= (ctrlMostPopularBikes.FetchedRecordsCount > 0)?"":"hide" %>" id="ctrlMostPopularBikes">
                    <div class="jcarousel-wrapper discover-bike-carousel">
                        <div class="jcarousel">
                            <ul>
                                <BW:MostPopularBikes runat="server" ID="ctrlMostPopularBikes" />
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
                                <BW:NewLaunchedBikes runat="server" ID="ctrlNewLaunchedBikes" />
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
        <BW:CompareBikes ID="ctrlCompareBikes" runat="server" />
    </section>

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
                    <div class="getFinal-price-container text-center margin-bottom50">
                        <div class="margin-bottom40">
                            <span class="bw-circle-icon final-price-logo"></span>
                        </div>
                        <p class="font16 margin-bottom30">Get final price of the bike without filling any forms</p>
                        <BW:NBOnRoadPrice Id="NBOnRoadPrice" runat="server" />
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
            if (ctrlNews.FetchedRecordsCount > 0) { reviewTabsCnt++; }
            if (ctrlExpertReviews.FetchedRecordsCount > 0) { reviewTabsCnt++; }
            if (ctrlVideos.FetchedRecordsCount > 0) { reviewTabsCnt++; }
        %>
        <section class="container <%= reviewTabsCnt == 0 ? "hide" : "" %>">
            <!--  News Bikes latest updates code starts here -->
            <div class="newBikes-latest-updates-container">
                <div class="grid-12">
                    <h2 class="text-bold text-center margin-top50 margin-bottom30">Latest updates from the industry</h2>
                    <div class="bw-tabs-panel content-box-shadow margin-bottom30">
                        <div class="text-center <%= reviewTabsCnt > 2 ? "" : ( reviewTabsCnt > 1 ? "margin-top30 margin-bottom30" : "margin-top10") %>">
                            <div class="bw-tabs <%= reviewTabsCnt > 2 ? "bw-tabs-flex" : ( reviewTabsCnt > 1 ? "home-tabs" : "hide") %>" id="reviewCount">
                                <ul>
                                    <li class="active" style="<%= (Convert.ToInt32(ctrlNews.FetchedRecordsCount) > 0) ? "": "display:none;" %>" data-tabs="ctrlNews">News</li>
                                    <li style="<%= (Convert.ToInt32(ctrlExpertReviews.FetchedRecordsCount) > 0) ? "": "display:none;" %>" data-tabs="ctrlExpertReviews">Reviews</li>                                   
                                    <li style="<%= (Convert.ToInt32(ctrlVideos.FetchedRecordsCount) > 0) ? "": "display:none;" %>" data-tabs="ctrlVideos">Videos</li>
                                </ul>
                            </div>
                        </div>
                        <BW:News runat="server" ID="ctrlNews" />
                        <BW:ExpertReviews runat="server" ID="ctrlExpertReviews" />                         
                        <BW:Videos runat="server" ID="ctrlVideos" />
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
    </form>
</body>
</html>
