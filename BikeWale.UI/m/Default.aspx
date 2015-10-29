﻿<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Default" %>
<%@ Register Src="/m/controls/NewsWidget.ascx" TagName="News" TagPrefix="BW"  %>
<%@ Register Src="/m/controls/ExpertReviewsWidget.ascx" TagName="ExpertReviews" TagPrefix="BW"  %>
<%@ Register Src="/m/controls/VideosWidget.ascx" TagName="Videos" TagPrefix="BW"  %>
<%@ Register Src="/m/controls/CompareBikesMin.ascx" TagName="CompareBike" TagPrefix="BW" %> 
<%@ Register Src="/m/controls/MOnRoadPricequote.ascx" TagName="MOnRoadPricequote" TagPrefix="BW" %>
<!doctype html>
<html>
<head>
    <%
        title = "New Bikes, Used Bikes, Bike Prices, Reviews & Photos in India";
        keywords = "new bikes, used bikes, buy used bikes, sell your bike, bikes prices, reviews, photos, news, compare bikes, Instant Bike On-Road Price";
        description = "BikeWale - India's favourite bike portal. Find new and used bikes, buy or sell your bikes, compare new bikes prices & values.";
        canonical = "http://www.bikewale.com";
        AdPath = "/1017752/Bikewale_Mobile_Homepage";
        AdId = "1398766000399";
     %>
    <!-- #include file="/includes/headscript_mobile.aspx" -->
    <link href="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/m/css/home.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css">
    <link href="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/css/chosen.min.css?<%= staticFileVersion %>" type="text/css"rel="stylesheet" /> 
</head>
<body class="bg-light-grey">
    <form runat="server">
    <!-- #include file="/includes/headBW_Mobile.aspx" -->
    <section>
        <div class="container">
            <div class="banner-home content-inner-block-10">
                <!-- Top banner code starts here -->
                <h1 class="text-uppercase text-white text-center padding-top30">FIND YOUR RIDE</h1>
                <div class="text-white margin-top15 text-center font14">Get Exclusive Offers on your Bike Purchase</div>
                <div class="new-used-search new-bikes-search margin-top30 position-rel">
                        <input type="text" placeholder="Search your bike here, e.g. Honda Activa " id="newBikeList" autocomplete="off" class="rounded-corner2">
                        <button id="btnSearch" class="btn btn-orange btn-search"><span class="fa fa-search"></span></button>
                        <span  id="loaderMakeModel"  class="fa fa-spinner fa-spin position-abt pos-right55 pos-top15 text-black" style="display:none"></span>
                </div>
            </div>
        </div>
    </section>
    <%--<section class="bg-light-grey">
        <div class="container">
        <div class="grid-12 margin-bottom20">
            <h2 class="text-center margin-top30 padding-left30 padding-right30">Comforts of booking online</h2>
            <div class="jcarousel-wrapper bike-booking-online-wrapper">
                <div class="jcarousel">
                    <ul>
                        <li>
                        	<div class="booking-online-item">
                                <div class="booking-online-pic bg-white text-center">
                                    <div class="bookingcomforts-sprite get-price-icon"></div>
                                </div>
                                <div class="bg-white font20 booking-online-box">Get real prices upfront</div>
                            </div>
                        </li>
                        <li>
                        	<div class="booking-online-item">
                                <div class="booking-online-pic bg-white text-center">
                                    <div class="bookingcomforts-sprite get-deal-icon"></div>
                                </div>
                                <div class="bg-white font20 booking-online-box">Get best deals & offers</div>
                            </div>
                        </li>
                        <li>
                        	<div class="booking-online-item">
                                <div class="booking-online-pic bg-white text-center">
                                    <div class="bookingcomforts-sprite save-visit-icon"></div>
                                </div>
                                <div class="bg-white font20 booking-online-box">Save on dealer visits</div>
                            </div>
                        </li>
                        <li>
                        	<div class="booking-online-item">
                                <div class="booking-online-pic bg-white text-center">
                                    <div class="bookingcomforts-sprite buying-asst-icon"></div>
                                </div>
                                <div class="bg-white font20 booking-online-box">Complete buying assistance</div>
                            </div>
                        </li>
                    </ul>
                </div>
                <span class="jcarousel-control-left"><a class="bwmsprite jcarousel-control-prev"></a></span>
                <span class="jcarousel-control-right"><a class="bwmsprite jcarousel-control-next"></a></span>
                <p class="text-center jcarousel-pagination margin-top20 margin-bottom10"></p>
            </div>
        </div>
        <div class="clear"></div>
    </div>
    </section>--%>

    <section><!--  Upcoming, New Launches and Top Selling code starts here -->
        <div class="container bg-white">
        	<div class="grid-12 alpha omega">
                <h2 class="text-center margin-top30 margin-bottom20">Discover your bike</h2>
                <div class="bw-tabs-panel padding-bottom20">
                    
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
                                    <a href="/m/honda-bikes/">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-honda" data-original="http://img.aeplcdn.com/bikewaleimg/m/images/brand-type-sprite.png?<%= staticFileVersion %>"></span>
                                        </span>
                                        <span class="brand-type-title">Honda</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/m/bajaj-bikes/">
                                        <span class="brand-type">
                                             <span class="lazy brandlogosprite brand-bajaj" data-original="http://img.aeplcdn.com/bikewaleimg/m/images/brand-type-sprite.png?<%= staticFileVersion %>"></span>
                                        </span>
                                        <span class="brand-type-title">Bajaj
                                        </span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/m/hero-bikes/">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-hero" data-original="http://img.aeplcdn.com/bikewaleimg/m/images/brand-type-sprite.png?<%= staticFileVersion %>"></span>
                                        </span>
                                        <span class="brand-type-title">Hero</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/m/tvs-bikes/">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-tvs" data-original="http://img.aeplcdn.com/bikewaleimg/m/images/brand-type-sprite.png?<%= staticFileVersion %>"></span>
                                        </span>
                                        <span class="brand-type-title">TVS</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/m/royalenfield-bikes/">
                                        <span class="brand-type">
                                             <span class="lazy brandlogosprite brand-royal" data-original="http://img.aeplcdn.com/bikewaleimg/m/images/brand-type-sprite.png?<%= staticFileVersion %>"></span>
                                        </span>
                                        <span class="brand-type-title">Royal Enfield</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/m/yamaha-bikes/">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-yamaha" data-original="http://img.aeplcdn.com/bikewaleimg/m/images/brand-type-sprite.png?<%= staticFileVersion %>"></span>
                                        </span>
                                        <span class="brand-type-title">Yamaha</span>
                                    </a>
                                </li>
                            </ul>
                            <ul class="brand-style-moreBtn brandTypeMore border-top1 padding-top25 text-center hide">
                                
                                
                                <li>
                                    <a href="/m/aprilia-bikes/">
                                        <span class="brand-type">
                                           	<span class="lazy brandlogosprite brand-aprilia" data-original="http://img.aeplcdn.com/bikewaleimg/m/images/brand-type-sprite.png?<%= staticFileVersion %>"></span>
                                        </span>
                                        <span class="brand-type-title">Aprilia</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/m/benelli-bikes/">
                                        <span class="brand-type">
                                             <span class="lazy brandlogosprite brand-benelli" data-original="http://img.aeplcdn.com/bikewaleimg/m/images/brand-type-sprite.png?<%= staticFileVersion %>"></span>
                                        </span>
                                        <span class="brand-type-title">Benelli</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/m/bmw-bikes/">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-bmw" data-original="http://img.aeplcdn.com/bikewaleimg/m/images/brand-type-sprite.png?<%= staticFileVersion %>"></span>
                                        </span>
                                        <span class="brand-type-title">BMW</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/m/ducati-bikes/">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-ducati" data-original="http://img.aeplcdn.com/bikewaleimg/m/images/brand-type-sprite.png?<%= staticFileVersion %>"></span>
                                        </span>
                                        <span class="brand-type-title">Ducati</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/m/harleydavidson-bikes/">
                                        <span class="brand-type">
                                             <span class="lazy brandlogosprite brand-harley" data-original="http://img.aeplcdn.com/bikewaleimg/m/images/brand-type-sprite.png?<%= staticFileVersion %>"></span>
                                        </span>
                                        <span class="brand-type-title">Harley Davidson</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/m/heroelectric-bikes/">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-hero-elec" data-original="http://img.aeplcdn.com/bikewaleimg/m/images/brand-type-sprite.png?<%= staticFileVersion %>"></span>
                                        </span>
                                        <span class="brand-type-title">Hero Electric</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/m/hyosung-bikes/">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-hyosung" data-original="http://img.aeplcdn.com/bikewaleimg/m/images/brand-type-sprite.png?<%= staticFileVersion %>"></span>
                                        </span>
                                        <span class="brand-type-title">Hyosung</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/m/indian-bikes/">
                                        <span class="brand-type">
                                             <span class="lazy brandlogosprite brand-indian" data-original="http://img.aeplcdn.com/bikewaleimg/m/images/brand-type-sprite.png?<%= staticFileVersion %>"></span>
                                        </span>
                                        <span class="brand-type-title">Indian</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/m/kawasaki-bikes/">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-kawasaki" data-original="http://img.aeplcdn.com/bikewaleimg/m/images/brand-type-sprite.png?<%= staticFileVersion %>"></span>
                                        </span>
                                        <span class="brand-type-title">Kawasaki</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/m/ktm-bikes/">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-ktm" data-original="http://img.aeplcdn.com/bikewaleimg/m/images/brand-type-sprite.png?<%= staticFileVersion %>"></span>
                                        </span>
                                        <span class="brand-type-title">KTM</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/m/lml-bikes/">
                                        <span class="brand-type">
                                             <span class="lazy brandlogosprite brand-lml" data-original="http://img.aeplcdn.com/bikewaleimg/m/images/brand-type-sprite.png?<%= staticFileVersion %>"></span>
                                        </span>
                                        <span class="brand-type-title">LML</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/m/mahindra-bikes/">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-mahindra" data-original="http://img.aeplcdn.com/bikewaleimg/m/images/brand-type-sprite.png?<%= staticFileVersion %>"></span>
                                        </span>
                                        <span class="brand-type-title">Mahindra</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/m/motoguzzi-bikes/">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-guzzi" data-original="http://img.aeplcdn.com/bikewaleimg/m/images/brand-type-sprite.png?<%= staticFileVersion %>"></span>
                                        </span>
                                        <span class="brand-type-title">Moto Guzzi</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/m/suzuki-bikes/">
                                        <span class="brand-type">
                                             <span class="lazy brandlogosprite brand-suzuki" data-original="http://img.aeplcdn.com/bikewaleimg/m/images/brand-type-sprite.png?<%= staticFileVersion %>"></span>
                                        </span>
                                        <span class="brand-type-title">Suzuki</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/m/triumph-bikes/">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-triumph" data-original="http://img.aeplcdn.com/bikewaleimg/m/images/brand-type-sprite.png?<%= staticFileVersion %>"></span>
                                        </span>
                                        <span class="brand-type-title">Triumph</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/m/vespa-bikes/">
                                        <span class="brand-type">
                                            <span class="lazy brandlogosprite brand-vespa" data-original="http://img.aeplcdn.com/bikewaleimg/m/images/brand-type-sprite.png?<%= staticFileVersion %>"></span>
                                        </span>
                                        <span class="brand-type-title">Vespa</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/m/yo-bikes/">
                                        <span class="brand-type">
                                             <span class="lazy brandlogosprite brand-yo" data-original="http://img.aeplcdn.com/bikewaleimg/m/images/brand-type-sprite.png?<%= staticFileVersion %>"></span>
                                        </span>
                                        <span class="brand-type-title">Yo</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/m/mv-agusta-bikes/">
                                        <span class="brand-type">
                                             <span class="lazy brandlogosprite brand-agusta" data-original="http://img.aeplcdn.com/bikewaleimg/m/images/brand-type-sprite.png?<%= staticFileVersion %>"></span>
                                        </span>
                                        <span class="brand-type-title">MV Agusta</span>
                                    </a>
                                </li>
                            </ul>
                        </div>
                        <div class="view-brandType text-center padding-top10 clear">
                            <a href="javascript:void(0)" id="view-brandType" class="view-more-btn font16">View More Brands</a>
                        </div>
                	</div>
                    <div class="bw-tabs-data hide" id="discoverBudget">
                        <div class="budget-container margin-bottom20">
                            <ul class="text-center">
                                <li>
                                    <a href="/m/new/search.aspx?budget=0-50000">
                                    	<span class="budget-title-box font14">
                                            Upto
                                        </span>
                                        <span class="budget-amount-box font12">
                                            <span class="fa fa-rupee"></span>
                                            <span class="font14 text-bold">50,000</span>
                                        </span>   
                                    </a>
                                </li>
                                <li>
                                    <a href="/m/new/search.aspx?budget=50000-100000">
                                    	<span class="budget-title-box font14">
                                            Between
                                        </span>
                                        <span class="budget-amount-box font12">
                                            <span class="fa fa-rupee"></span>
                                            <span class="font14 text-bold">50,000 - </span>
                                            <span class="fa fa-rupee"></span>
                                            <span class="font14 text-bold">1</span>
                                            <span class="budget-amount-text-box font14 text-bold">Lac</span>
                                        </span>   
                                    </a>
                                </li>
                                <li>
                                    <a href="/m/new/search.aspx?budget=100000-250000">
                                    	<span class="budget-title-box font14">
                                            Between
                                        </span>
                                        <span class="budget-amount-box font12 text-bold">
                                            <span class="fa fa-rupee"></span>
                                            <span class="font14 text-bold">1</span>
                                            <span class="budget-amount-text-box font14 text-bold">Lac</span>
                                            <span class="font14 text-bold"> - </span>
                                            <span class="fa fa-rupee"></span>
                                            <span class="font14 text-bold">2.5</span>
                                            <span class="budget-amount-text-box font14 text-bold">Lac</span>
                                        </span>   
                                    </a>
                                </li>
                                <li>
                                    <a href="/m/new/search.aspx?budget=250000-">
                                    	<span class="budget-title-box font14">
                                            Above
                                        </span>
                                        <span class="budget-amount-box font12">
                                            <span class="fa fa-rupee"></span>
                                            <span class="font14 text-bold">2.5</span>
                                            <span class="budget-amount-text-box font14 text-bold">Lac</span>
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
                                    <a href="/m/new/search.aspx?mileage=1">
                                    	<span class="mileage-title-box font14">
                                            Above
                                        </span>
                                        <span class="mileage-amount-box">
                                            <span class="font14 text-bold">70 Kmpl</span>
                                        </span>   
                                    </a>
                                </li>
                                <li>
                                    <a href="/m/new/search.aspx?mileage=2">
                                    	<span class="mileage-title-box font14">
                                            Between
                                        </span>
                                        <span class="mileage-amount-box font14 text-bold">
                                            <span class="">70</span>
                                            <span class="mileage-amount-text-box">Kmpl</span>
                                            <span class=""> - 50</span>
                                            <span class="mileage-amount-text-box">Kmpl</span>
                                        </span>   
                                    </a>
                                </li>
                                <li>
                                    <a href="/m/new/search.aspx?mileage=3">
                                    	<span class="mileage-title-box font14">
                                            Between
                                        </span>
                                        <span class="mileage-amount-box font14 text-bold">
                                            <span class="">50</span>
                                            <span class="mileage-amount-text-box">Kmpl</span>
                                            <span class=""> - 30</span>
                                            <span class="mileage-amount-text-box">Kmpl</span>
                                        </span>   
                                    </a>
                                </li>
                                <li>
                                    <a href="/m/new/search.aspx?mileage=4">
                                    	<span class="mileage-title-box font14">
                                            Upto
                                        </span>
                                        <span class="mileage-amount-box">
                                            <span class="font14 text-bold">30 Kmpl</span>
                                        </span>  
                                    </a>
                                </li>
                            </ul>
                        </div>
                	</div>
                    <div class="bw-tabs-data hide margin-bottom20" id="discoverStyle">
                        <div class="style-type-container">
                            <ul class="text-center">
                                <li>
                                    <a href="/m/new/search.aspx?ridestyle=5">
                                        <span class="style-type">
                                            <span class="styletypesprite style-scooters"></span>
                                        </span>
                                        <span class="style-type-title">scooters</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/m/new/search.aspx?ridestyle=3">
                                        <span class="style-type">
                                             <span class="styletypesprite style-street"></span>
                                        </span>
                                        <span class="style-type-title">street</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/m/new/search.aspx?ridestyle=1">
                                        <span class="style-type">
                                            <span class="styletypesprite style-cruiser"></span>
                                        </span>
                                        <span class="style-type-title">cruiser</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/m/new/search.aspx?ridestyle=2">
                                        <span class="style-type">
                                             <span class="styletypesprite style-sports"></span>
                                        </span>
                                        <span class="style-type-title">sports</span>
                                    </a>
                                </li>
                            </ul>
                        </div>
                	</div>
                </div>        
        	</div>
            <div class="clear"></div>
        </div>
    </section><!-- Upcoming, new launches Ends here -->
    <section class="lazy home-getFinalPrice-banner" data-original="http://img.aeplcdn.com/bikewaleimg/m/images/onroad-price-banner.jpg" ><!--  Get Final Price code starts here -->
        <BW:MOnRoadPricequote PageId="5" ID="MOnRoadPricequote" runat="server"/> 
    </section><!-- Get Final Price code Ends here -->
    <section class="home-compare"><!--  Compare section code starts here -->
        <BW:CompareBike ID="ctrlCompareBikes" runat="server"/>
    </section><!-- Compare code Ends here -->    
    
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
                <div class="grid-12 alpha omega">
                    <h2 class="text-center margin-top30 margin-bottom20">Latest Updates</h2>
                    <div class="bw-tabs-panel">
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
                        <div class="grid-12">                       
                        <%if (!isNewsZero) { %>         <BW:News runat="server" ID="ctrlNews" />    <% } %>
                        <%if (!isExpertReviewZero) { %> <BW:ExpertReviews runat="server" ID="ctrlExpertReviews" />  <% } %>                         
                        <%if (!isVideoZero) { %>        <BW:Videos runat="server" ID="ctrlVideos" />    <% } %>
                       </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
    <!--  News, reviews and videos code ends here -->
    
    <section>
    	<div class="container">
        	<div id="bottom-ad-div" class="bottom-ad-div"><!--Bottom Ad banner code starts here -->
            	
            </div><!--Bottom Ad banner code ends here -->
        </div>
    </section>
<!-- #include file="/includes/footerBW_Mobile.aspx" -->
<!-- all other js plugins -->    
<!-- #include file="/includes/footerscript_Mobile.aspx" -->
    <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/m/src/home.js?<%= staticFileVersion %>"></script>
    <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/m/src/chosen-jquery-min-mobile.js?<%= staticFileVersion %>"></script>
    <script type="text/javascript">
         ga_pg_id = '1';
         if ('<%=isNewsActive%>' == "False") $("#ctrlNews").addClass("hide");
         if ('<%=isExpertReviewActive%>' == "False") $("#ctrlExpertReviews").addClass("hide");
         if ('<%=isVideoActive%>' == "False") $("#ctrlVideos").addClass("hide");
    </script>
</form>
</body>
</html>
