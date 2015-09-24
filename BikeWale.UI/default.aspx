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
                                <input type="text" placeholder="Search your bike here, e.g. Honda Activa " id="newBikeList" tabindex="1">
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
    <section class="bg-light-grey">
        <!--  Booking online code starts here -->
        <div class="container ">
            <div class="grid-12 alpha omega margin-bottom30">
                <h2 class="text-bold text-center margin-top50 font28">Comforts of booking online</h2>
                <div class="grid-3 text-center">
                    <div class="booking-online-pic bg-white text-center">
                        <div class="bookingcomforts-sprite get-price-icon"></div>
                    </div>
                    <div class="bg-white font20 booking-online-box">Get real prices upfront</div>
                </div>
                <div class="grid-3 text-center">
                    <div class="booking-online-pic bg-white text-center">
                        <div class="bookingcomforts-sprite get-deal-icon"></div>
                    </div>
                    <div class="bg-white font20 booking-online-box">Get best deals & offers</div>
                </div>
                <div class="grid-3 text-center">
                    <div class="booking-online-pic bg-white text-center">
                        <div class="bookingcomforts-sprite save-visit-icon"></div>
                    </div>
                    <div class="bg-white font20 booking-online-box">
                        Save on<br />
                        dealer visits
                    </div>
                </div>
                <div class="grid-3 text-center">
                    <div class="booking-online-pic bg-white text-center">
                        <div class="bookingcomforts-sprite buying-asst-icon"></div>
                    </div>
                    <div class="bg-white font20 booking-online-box">
                        Complete<br />
                        buying assistance
                    </div>
                </div>
                <div class="clear"></div>
            </div>
            <div class="clear"></div>
        </div>
    </section>
    <!--  Ends here -->
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
                        </div>
                    </div>
                </div>
            </div>
            <div class="clear"></div>
        </div>
    </section>
    <!--  Ends here -->
    <section class="home-getFinalPrice-banner">
      <BW:OnRoadPriceQuote ID="ctrlOnRoadPriceQuote" runat="server"/>
    </section>

    <section class="margin-bottom50">
        <!--  Compare section code starts here -->
        <BW:CompareBikes ID="ctrlCompareBikes" runat="server" />
    </section>
    <!-- Ends here -->
    <section class="bg-light-grey if">
        <!--  Used Bikes code starts here -->
        <BW:PopularUsedBikes runat="server" ID="ctrlPopularUsedBikes" />
    </section>
    <!-- Ends here -->
    <section>
        <!--  News Bikes code starts here -->
        <div class="container">
            <div class="grid-12">
                <h2 class="text-bold text-center margin-top50 margin-bottom30 font28">Latest updates from the industry</h2>
                <div class="bw-tabs-panel">
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
        </div>
    </section>
    <!-- Ends here -->
    <!-- #include file="/includes/footerBW.aspx" -->
    <!-- #include file="/includes/footerscript.aspx" -->
    <script type="text/javascript" src="<%= staticUrl != "" ? "http://st.aeplcdn.com" + staticUrl : "" %>/src/home.js?<%= staticFileVersion %>"></script>
</form>
</body>
</html>
