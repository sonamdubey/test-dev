<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.New.Default" %>

<link href="../css/newbikes.css" rel="stylesheet" type="text/css">
<%@ Register Src="~/controls/News_new.ascx" TagName="News" TagPrefix="BW" %>
<%@ Register Src="~/controls/UpcomingBikes_new.ascx" TagName="UpcomingBikes" TagPrefix="BW" %>
<%@ Register Src="~/controls/NewLaunchedBikes_new.ascx" TagName="NewLaunchedBikes" TagPrefix="BW" %>
<%@ Register Src="~/controls/MostPopularBikes_new.ascx" TagName="MostPopularBikes" TagPrefix="BW" %>
<%@ Register Src="~/controls/ExpertReviews.ascx" TagName="ExpertReviews" TagPrefix="BW" %>
<%@ Register Src="~/controls/VideosControl.ascx" TagName="Videos" TagPrefix="BW" %>
<%@ Register Src="~/controls/ComparisonMin.ascx" TagName="CompareBikes" TagPrefix="BW" %>
<html>
<head>
    <!-- #include file="/includes/headscript.aspx" -->
</head>
<body class="bg-white">
    <!-- #include file="/includes/headBW.aspx" -->
    <header class="new-bikes-top-banner">    	
        <div class="container">
            <div class="welcome-box">
                <h1 class="text-uppercase margin-bottom10">NEW BIKES</h1>
                <p class="font20">View every bike under one roof</p>
            </div>
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
                            <a href="/aprilia-bikes/">
                                <span class="brand-type">
                                    <span class="brandlogosprite brand-aprilia"></span>
                                </span>
                                <span class="brand-type-title">Aprilia</span>
                            </a>
                        </li>
                        <li>
                            <a href="/honda-bikes/">
                                <span class="brand-type">
                                    <span class="brandlogosprite brand-honda"></span>
                                </span>
                                <span class="brand-type-title">Hero</span>
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
                            <a href="/bajaj-bikes/">
                                <span class="brand-type">
                                    <span class="brandlogosprite brand-bajaj"></span>
                                </span>
                                <span class="brand-type-title">Bajaj</span>
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
                            <a href="/suzuki-bikes/">
                                <span class="brand-type">
                                    <span class="brandlogosprite brand-suzuki"></span>
                                </span>
                                <span class="brand-type-title">Suzuki</span>
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
                            <a href="/indian-bikes/">
                                <span class="brand-type">
                                    <span class="brandlogosprite brand-indian"></span>
                                </span>
                                <span class="brand-type-title">Indian</span>
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
                            <a href="/bmw-bikes/">
                                <span class="brand-type">
                                    <span class="brandlogosprite brand-bmw"></span>
                                </span>
                                <span class="brand-type-title">BMW</span>
                            </a>
                        </li>
                    </ul>
                    <div class="brand-bottom-border border-solid-top margin-left20 margin-right20 hide">
                    </div>
                    <ul class="brand-style-moreBtn padding-top25 brandTypeMore hide text-center">
                        <li>
                            <a href="/ducati-bikes/">
                                <span class="brand-type">
                                    <span class="brandlogosprite brand-ducati"></span>
                                </span>
                                <span class="brand-type-title">Ducati</span>
                            </a>
                        </li>
                        <li>
                            <a href="/harleydavidson-bikes/">
                                <span class="brand-type">
                                    <span class="brandlogosprite brand-harley"></span>
                                </span>
                                <span class="brand-type-title">Harley</span>
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
                            <a href="/heroelectric-bikes/">
                                <span class="brand-type">
                                    <span class="brandlogosprite brand-hero-elec"></span>
                                </span>
                                <span class="brand-type-title">Hero Electric</span>
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
                            <a href="/ktm-bikes/">
                                <span class="brand-type">
                                    <span class="brandlogosprite brand-ktm"></span>
                                </span>
                                <span class="brand-type-title">KTM</span>
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
                            <a href="/mahindra-bikes/">
                                <span class="brand-type">
                                    <span class="brandlogosprite brand-mahindra"></span>
                                </span>
                                <span class="brand-type-title">Mahindra</span>
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
                            <a href="/tvs-bikes/">
                                <span class="brand-type">
                                    <span class="brandlogosprite brand-tvs"></span>
                                </span>
                                <span class="brand-type-title">TVS</span>
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
                            <a href="/yamaha-bikes/">
                                <span class="brand-type">
                                    <span class="brandlogosprite brand-yamaha"></span>
                                </span>
                                <span class="brand-type-title">Yamaha</span>
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
                    <a href="#" id="view-brandType" class="view-more-btn font16">View <span>More</span> Brands</a>
                </div>
            </div>
            <div class="bw-tabs-data hide" id="discoverBudget">
                <div class="budget-container margin-bottom20">
                    <ul class="text-center">
                        <li>
                            <a href="#">
                                <span class="budget-title-box font16">Upto
                                </span>
                                <span class="budget-amount-box font20">
                                    <span class="fa fa-rupee"></span>
                                    <span class="font24">50,000</span>
                                </span>
                            </a>
                        </li>
                        <li>
                            <a href="#">
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
                            <a href="#">
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
                            <a href="#">
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
                            <a href="#">
                                <span class="mileage-title-box font16">Above
                                </span>
                                <span class="mileage-amount-box font24">
                                    <span>60 <span class="font16">Kmpl</span></span>
                                </span>
                            </a>
                        </li>
                        <li>
                            <a href="#">
                                <span class="mileage-title-box font16">Between
                                </span>
                                <span class="mileage-amount-box font24">
                                    <span>60</span>
                                    <span class="mileage-amount-text-box font16">Kmpl</span>
                                    <span>- 40</span>
                                    <span class="mileage-amount-text-box font16">Kmpl</span>
                                </span>
                            </a>
                        </li>
                        <li>
                            <a href="#">
                                <span class="mileage-title-box font16">Between
                                </span>
                                <span class="mileage-amount-box font24">
                                    <span>40</span>
                                    <span class="mileage-amount-text-box font16">Kmpl</span>
                                    <span>- 20</span>
                                    <span class="mileage-amount-text-box font16">Kmpl</span>
                                </span>
                            </a>
                        </li>
                        <li>
                            <a href="#">
                                <span class="mileage-title-box font16">Upto
                                </span>
                                <span class="mileage-amount-box font24">
                                    <span>20</span>
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
                            <a href="#">
                                <span class="style-type">
                                    <span class="styletypesprite style-scooters"></span>
                                </span>
                                <span class="style-type-title">Scooters</span>
                            </a>
                        </li>
                        <li>
                            <a href="#">
                                <span class="style-type">
                                    <span class="styletypesprite style-street"></span>
                                </span>
                                <span class="style-type-title">Street</span>
                            </a>
                        </li>
                        <li>
                            <a href="#">
                                <span class="style-type">
                                    <span class="styletypesprite style-cruiser"></span>
                                </span>
                                <span class="style-type-title">Cruiser</span>
                            </a>
                        </li>
                        <li>
                            <a href="#">
                                <span class="style-type">
                                    <span class="styletypesprite style-sports"></span>
                                </span>
                                <span class="style-type-title">Sports</span>
                            </a>
                        </li>
                    </ul>
                    <ul class="brand-style-moreBtn styleTypeMore hide text-center">
                        <li>
                            <a href="#">
                                <span class="style-type">
                                    <span class="styletypesprite style-cruiser"></span>
                                </span>
                                <span class="style-type-title">Cruiser</span>

                            </a>
                        </li>
                        <li>
                            <a href="#">
                                <span class="style-type">
                                    <span class="styletypesprite style-sports"></span>
                                </span>
                                <span class="style-type-title">Sports</span>

                            </a>
                        </li>
                        <li>
                            <a href="#">
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

<section class="container">
    <!--  Discover bikes section code starts here -->
    <div class="grid-12">
        <h2 class="text-bold text-center margin-top50 margin-bottom30 font28">Discover your bike</h2>
        <div class="bw-tabs-panel newbike-discover-bike-container content-box-shadow">
            <div class="bw-tabs bw-tabs-flex">
                <ul>
                    <li class="active" data-tabs="ctrlMostPopularBikes">Most Popular</li>
                    <li data-tabs="ctrlNewLaunchedBikes">New launches</li>
                    <li data-tabs="ctrlUpcomingBikes">Upcoming </li>
                </ul>
            </div>
            <BW:MostPopularBikes runat="server" ID="ctrlMostPopularBikes" />
            <!-- Most Popular Bikes Control-->

            <BW:NewLaunchedBikes runat="server" ID="ctrlNewLaunchedBikes" />
            <!-- New Launched Bikes Control-->

            <BW:UpcomingBikes runat="server" ID="ctrlUpcomingBikes" />
            <!-- Upcoming Bikes Control-->
        </div>
    </div>
    <div class="clear"></div>
</section>


<section class="container">
    <!--  Compare section code starts here -->
    <BW:CompareBikes ID="ctrlCompareBikes" runat="server" />
    <%--<div class="grid-12">
            <h2 class="text-bold text-center margin-top50 margin-bottom30">Compare now</h2>
            <div class="content-box-shadow padding-bottom20">
                <div class="grid-6 margin-top20">
                <div class="border-solid-right">
                    <h3 class="font16 text-center padding-bottom15">
                        <a href="#">Suzuki Gixxer SF Vs Pulsar AS 200</a>
                    </h3>
                    <div class="bike-preview margin-bottom10">
                        <img src="http://imgd1.aeplcdn.com//310x174//bw/bikecomparison/kawasaki_ninja300_vs_yamaha_yzf-r3.jpg?20151708125625" title="IMG title" alt="IMG title">
                    </div>
                    <div>
                        <div class="grid-6 alpha border-solid-right">
                            <div class="content-inner-block-5 text-center">
                                <div class="font18 margin-bottom5">
                                    <span class="fa fa-rupee"></span> 49,000
                                </div>
                                <div>
                                    <p class="margin-bottom5">
                                        <img src="images/ratings/1.png" alt="Rate">
                                        <img src="images/ratings/1.png" alt="Rate">
                                        <img src="images/ratings/half.png" alt="Rate">
                                        <img src="images/ratings/0.png" alt="Rate">
                                        <img src="images/ratings/0.png" alt="Rate">
                                    </p>
                                    <p class="font14"><a href="#" class="margin-left5"><span>3009</span> reviews</a></p>
                                </div>
                            </div>
                        </div>
                        <div class="grid-6 omega">
                            <div class="content-inner-block-5 text-center">
                                <div class="font18 margin-bottom5">
                                    <span class="fa fa-rupee"></span> 49,000
                                </div>
                                <div>
                                    <p class="margin-bottom5">
                                        <img src="images/ratings/1.png" alt="Rate">
                                        <img src="images/ratings/1.png" alt="Rate">
                                        <img src="images/ratings/half.png" alt="Rate">
                                        <img src="images/ratings/0.png" alt="Rate">
                                        <img src="images/ratings/0.png" alt="Rate">
                                    </p>
                                    <p class="font14"><a href="#" class="margin-left5">Write reviews</a></p>
                                </div>
                            </div>
                        </div>
                        <div class="clear"></div>
                    </div>
                </div>
                </div>
                <div class="grid-6 margin-top20">
                    <div class="compare-list-home">
                        <ul>
                            <li>
                                <p class="font16 text-center padding-bottom15">
                                    <a href="#">Honda Unicorn vs Bajaj Pulsar 180 DTSi</a>
                                </p>
                                <div class="font16 text-light-grey">
                                    <span class="margin-right50">
                                        <span class="fa fa-rupee"></span> <span>49,000</span>
                                    </span>
                                    <span class="fa fa-rupee"></span> <span>49,000</span>
                                </div>
                            </li>
                            <li>
                                <p class="font16 text-center padding-bottom15">
                                    <a href="#">Bajaj Discover 150F vs Suzuki Gixxer</a>
                                </p>
                                <div class="font16 text-light-grey">
                                    <span class="margin-right50">
                                        <span class="fa fa-rupee"></span> <span>49,000</span>
                                    </span>
                                    <span class="fa fa-rupee"></span> <span>49,000</span>
                                </div>
                            </li>
                            <li>
                                <p class="font16 text-center padding-bottom15">
                                    <a href="#">Hero Impulse vs KTM Duke 200</a>
                                </p>
                                <div class="font16 text-light-grey">
                                    <span class="margin-right50">
                                        <span class="fa fa-rupee"></span> <span>49,000</span>
                                    </span>
                                    <span class="fa fa-rupee"></span> <span>49,000</span>
                                </div>
                            </li>
                        </ul>
                        <div class="text-center margin-top20">
                            <a href="#" class="btn btn-orange">View more comparisons</a>
                        </div>
                    </div>
                </div>            
                <div class="clear"></div>
            </div>
        </div>
        <div class="clear"></div>--%>
</section>

<section class="container">
    <!--  Tools you may need section code starts here -->
    <div class="grid-12">
        <h2 class="text-bold text-center margin-top50 margin-bottom30 font28">Tools you may need</h2>
        <div class="bw-tabs-panel tools-may-need-wrapper content-box-shadow">
            <div class="bw-tabs bw-tabs-flex">
                <ul class="tools-may-need-UL">
                    <li data-tabs="getFinal-price" class="">On-Road Price</li>
                    <li data-tabs="locate-dealer" class="">Locate A Dealer</li>
                    <li data-tabs="calculate-emi" class="active">Calculate EMI's</li>
                </ul>
            </div>
            <div class="bw-tabs-data hide" id="getFinal-price">
                <div class="getFinal-price-container text-center margin-bottom50">
                    <div class="margin-bottom40">
                        <span class="bw-circle-icon final-price-logo"></span>
                    </div>
                    <p class="font16 margin-bottom30">Get final price of the bike without filling any forms</p>

                    <div class="final-price-search-container tools-price-dealer-container">
                        <div class="final-price-search tools-price-dealer-search">
                            <div class="final-price-bikeSelect">
                                <div class="form-control-box">
                                    <input class="form-control border-no rounded-corner0" type="text" placeholder="Type to select bike name Eg: Bajaj" id="finalPriceBikeSelect">
                                    <span class="fa fa-spinner fa-spin position-abt pos-right10 pos-top15 text-black" style="display: none"></span>
                                    <span class="bwsprite error-icon"></span>
                                    <div class="bw-blackbg-tooltip">Please select a bike</div>
                                </div>
                            </div>
                            <div class="final-price-citySelect">
                                <div class="form-control-box">
                                    <select class="form-control rounded-corner0">
                                        <option value="">Select city</option>
                                        <option value="">Mumbai</option>
                                        <option value="">Navi Mumbai</option>
                                        <option value="">Delhi</option>
                                        <option value="">Banglore</option>
                                    </select>
                                </div>
                            </div>
                            <div class="clear"></div>
                        </div>
                        <div class="get-final-price-btn">
                            <button class="font18 btn btn-orange btn-lg rounded-corner-no-left">Get final price</button>
                        </div>
                        <div class="clear"></div>
                    </div>

                </div>
            </div>
            <div class="bw-tabs-data hide" id="locate-dealer">
                <%--<div class="locate-dealer-container text-center margin-bottom50">
                    <div class="margin-bottom40">
                        <span class="bw-circle-icon locate-dealer-logo"></span>
                    </div>
                    <p class="font16 margin-bottom30">Find a car dealer near your current location</p>

                    <div class="locate-dealer-search-container">
                        <div class="locate-dealer-search">
                            <div class="locate-dealer-bikeSelect">
                                <div class="form-control-box">
                                    <input class="form-control border-no rounded-corner0" type="text" placeholder="Type to select bike name Eg: Bajaj" id="locateDealerBikeSelect">
                                    <span class="fa fa-spinner fa-spin position-abt pos-right10 pos-top15 text-black" style="display: none"></span>
                                    <span class="bwsprite error-icon"></span>
                                    <div class="bw-blackbg-tooltip">Please select a bike</div>
                                </div>
                            </div>
                            <div class="locate-dealer-citySelect">
                                <div class="form-control-box">
                                    <select class="form-control rounded-corner0">
                                        <option value="">Select city</option>
                                        <option value="">Mumbai</option>
                                        <option value="">Navi Mumbai</option>
                                        <option value="">Delhi</option>
                                        <option value="">Banglore</option>
                                    </select>
                                </div>
                            </div>
                            <div class="clear"></div>
                        </div>
                        <div class="locate-dealer-btn">
                            <button class="font18 btn btn-orange btn-lg rounded-corner-no-left">Locate dealer</button>
                        </div>

                        <div class="clear"></div>
                    </div>
                </div>--%>
                <BW:LocateDealer Id="ctrlLocateDealer" runat="server"/>
            </div>
            <div class="bw-tabs-data" id="calculate-emi">
                <BW:EmiCalc Id="ctrlEmiCalc" runat="server"></BW:EmiCalc>
                <%--<div class="calculate-emi-container text-center margin-bottom50">
                    <div class="margin-bottom40">
                        <span class="bw-circle-icon user-review-logo"></span>
                    </div>
                    <p class="font16 margin-bottom30">Instant calculate loan EMI</p>
                    <div class="calculate-emi-search-container">
                        <div class="calculate-emi-tool-search">
                            <div class="loan-amount-box">
                                <div class="form-control-box">
                                    <input class="form-control rounded-corner0 border-no" type="text" placeholder="Enter loan amount" id="txtLoanAmount">
                                    <span class="fa fa-spinner fa-spin position-abt pos-right10 pos-top15 text-black" style="display: none"></span>
                                    <span class="bwsprite error-icon"></span>
                                    <div class="bw-blackbg-tooltip">Please enter loan amount</div>
                                </div>
                            </div>
                            <div class="interest-rate-boxSelect">
                                <div class="form-control-box">
                                    <input class="form-control rounded-corner0 border-no" type="text" placeholder="Rate of Interest" id="txtRateOfInterest" />
                                    <span class="fa fa-spinner fa-spin position-abt pos-right10 pos-top15 text-black" style="display: none"></span>
                                    <span class="bwsprite error-icon"></span>
                                    <div class="bw-blackbg-tooltip">Please enter rate of interest</div>
                                </div>
                            </div>
                            <div class="clear"></div>
                        </div>
                        <div class="calculate-btn">
                            <button class="font18 btn btn-orange btn-lg rounded-corner-no-left">Calculate</button>
                        </div>
                        <div class="clear"></div>
                    </div>
                </div>--%>
                <div class="clear"></div>
            </div>
        </div>
    </div>
    <div class="clear"></div>
</section>

<section class="container">
    <!--  News Bikes latest updates code starts here -->
    <div class="newBikes-latest-updates-container">
        <div class="grid-12">
            <h2 class="text-bold text-center margin-top50 margin-bottom30 font28">Latest updates from the industry</h2>
            <div class="bw-tabs-panel content-box-shadow margin-bottom30">
                <div class="bw-tabs bw-tabs-flex">
                    <ul>
                        <li class="active" data-tabs="ctrlNews">News</li>
                        <li data-tabs="ctrlExpertReviews">Reviews</li>
                        <li data-tabs="ctrlVideos">Videos</li>
                    </ul>
                </div>
                <BW:News runat="server" ID="ctrlNews" />
                <BW:ExpertReviews runat="server" ID="ctrlExpertReviews" />
                <BW:Videos runat="server" ID="ctrlVideos" />
            </div>
        </div>
        <div class="clear"></div>
    </section>
	
    <section class="container"><!--  News Bikes latest updates code starts here -->
        <div class="newBikes-latest-updates-container">
            <div class="grid-12">
                <h2 class="text-bold text-center margin-top50 margin-bottom30">Latest updates from the industry</h2>
                <div class="bw-tabs-panel content-box-shadow margin-bottom30">
                    <div class="bw-tabs bw-tabs-flex">
                        <ul>
                            <li class="active" data-tabs="ctrlNews">News</li>
                            <li data-tabs="ctrlExpertReviews">Reviews</li>
                            <li data-tabs="ctrlVideos">Videos</li>
                        </ul>
                    </div>
                    <BW:News runat="server" ID="ctrlNews"/>
                    <BW:ExpertReviews runat="server" ID="ctrlExpertReviews"/>                    
                    <BW:Videos runat="server" ID="ctrlVideos"/>
                </div>        
            </div>
            <div class="clear"></div>
        </div>
    </section>

<!-- #include file="/includes/footerBW.aspx" -->
<!-- #include file="/includes/footerscript.aspx" -->
</body>
</html>
