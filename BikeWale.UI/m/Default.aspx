<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Default" %>
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
                        <input type="text" placeholder="Search your bike here, e.g. Honda Activa " id="newBikeList" class="rounded-corner2">
                        <button id="btnSearch" class="btn btn-orange btn-search"><span class="fa fa-search"></span></button>
                        <span  id="loaderMakeModel"  class="fa fa-spinner fa-spin position-abt pos-right55 pos-top15 text-black" style="display:none"></span>
                </div>
            </div>
        </div>
    </section>
    
    <section class="bg-light-grey">
    
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
        
    </section>
    
    <section><!--  Upcoming, New Launches and Top Selling code starts here -->
        <div class="container bg-white">
        	<div class="grid-12">
                <h2 class="text-center margin-top30 margin-bottom20">Discover your bike</h2>
                <div class="bw-tabs-panel padding-bottom20">
                    <div class="bw-tabs margin-bottom15">
                    	<div class="form-control-box">
                            
                            <select class="form-control">
                                <option value="discoverBrand">Brand</option>
                                <option value="discoverBudget">Budget</option>
                                <option value="discoverMileage">Mileage</option>
                                <option value="discoverStyle">Style</option>                                
                            </select>
                        </div>
                    </div>
                    <div class="bw-tabs-data" id="discoverBrand">
                        <div class="brand-type-container">
                            <ul class="text-center">
                                <li>
                                    <a href="/m/honda-bikes/">
                                        <span class="brand-type">
                                            <span class="brandlogosprite brand-honda"></span>
                                        </span>
                                        <span class="brand-type-title">Honda</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/m/bajaj-bikes/">
                                        <span class="brand-type">
                                             <span class="brandlogosprite brand-bajaj"></span>
                                        </span>
                                        <span class="brand-type-title">Bajaj
                                        </span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/m/hero-bikes/">
                                        <span class="brand-type">
                                            <span class="brandlogosprite brand-hero"></span>
                                        </span>
                                        <span class="brand-type-title">Hero</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/m/tvs-bikes/">
                                        <span class="brand-type">
                                            <span class="brandlogosprite brand-tvs"></span>
                                        </span>
                                        <span class="brand-type-title">TVS</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/m/royalenfield-bikes/">
                                        <span class="brand-type">
                                             <span class="brandlogosprite brand-royal"></span>
                                        </span>
                                        <span class="brand-type-title">Royal Enfield</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/m/yamaha-bikes/">
                                        <span class="brand-type">
                                            <span class="brandlogosprite brand-yamaha"></span>
                                        </span>
                                        <span class="brand-type-title">Yamaha</span>
                                    </a>
                                </li>
                            </ul>
                            <ul class="brand-style-moreBtn brandTypeMore border-top1 padding-top25 text-center hide">
                                
                                
                                <li>
                                    <a href="/m/aprilia-bikes/">
                                        <span class="brand-type">
                                           	<span class="brandlogosprite brand-aprilia"></span>
                                        </span>
                                        <span class="brand-type-title">Aprilia</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/m/benelli-bikes/">
                                        <span class="brand-type">
                                             <span class="brandlogosprite brand-benelli"></span>
                                        </span>
                                        <span class="brand-type-title">Benelli</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/m/bmw-bikes/">
                                        <span class="brand-type">
                                            <span class="brandlogosprite brand-bmw"></span>
                                        </span>
                                        <span class="brand-type-title">BMW</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/m/ducati-bikes/">
                                        <span class="brand-type">
                                            <span class="brandlogosprite brand-ducati"></span>
                                        </span>
                                        <span class="brand-type-title">Ducati</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/m/harleydavidson-bikes/">
                                        <span class="brand-type">
                                             <span class="brandlogosprite brand-harley"></span>
                                        </span>
                                        <span class="brand-type-title">Harley Davidson</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/m/heroelectric-bikes/">
                                        <span class="brand-type">
                                            <span class="brandlogosprite brand-hero-elec"></span>
                                        </span>
                                        <span class="brand-type-title">Hero Electric</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/m/hyosung-bikes/">
                                        <span class="brand-type">
                                            <span class="brandlogosprite brand-hyosung"></span>
                                        </span>
                                        <span class="brand-type-title">Hyosung</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/m/indian-bikes/">
                                        <span class="brand-type">
                                             <span class="brandlogosprite brand-indian"></span>
                                        </span>
                                        <span class="brand-type-title">Indian</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/m/kawasaki-bikes/">
                                        <span class="brand-type">
                                            <span class="brandlogosprite brand-kawasaki"></span>
                                        </span>
                                        <span class="brand-type-title">Kawasaki</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/m/ktm-bikes/">
                                        <span class="brand-type">
                                            <span class="brandlogosprite brand-ktm"></span>
                                        </span>
                                        <span class="brand-type-title">KTM</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/m/lml-bikes/">
                                        <span class="brand-type">
                                             <span class="brandlogosprite brand-lml"></span>
                                        </span>
                                        <span class="brand-type-title">LML</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/m/mahindra-bikes/">
                                        <span class="brand-type">
                                            <span class="brandlogosprite brand-mahindra"></span>
                                        </span>
                                        <span class="brand-type-title">Mahindra</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/m/motoguzzi-bikes/">
                                        <span class="brand-type">
                                            <span class="brandlogosprite brand-guzzi"></span>
                                        </span>
                                        <span class="brand-type-title">Moto Guzzi</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/m/suzuki-bikes/">
                                        <span class="brand-type">
                                             <span class="brandlogosprite brand-suzuki"></span>
                                        </span>
                                        <span class="brand-type-title">Suzuki</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/m/triumph-bikes/">
                                        <span class="brand-type">
                                            <span class="brandlogosprite brand-triumph"></span>
                                        </span>
                                        <span class="brand-type-title">Triumph</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/m/vespa-bikes/">
                                        <span class="brand-type">
                                            <span class="brandlogosprite brand-vespa"></span>
                                        </span>
                                        <span class="brand-type-title">Vespac</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="/m/yo-bikes/">
                                        <span class="brand-type">
                                             <span class="brandlogosprite brand-yo"></span>
                                        </span>
                                        <span class="brand-type-title">Yo</span>
                                    </a>
                                </li>
                            </ul>
                        </div>
                        <div class="view-brandType text-center padding-top10 clear">
                            <a href="javascript:void(0)" id="view-brandType" class="view-more-btn font16">View complete list</a>
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
                                    <a href="/m/new/search.aspx?budget=100000-150000">
                                    	<span class="budget-title-box font14">
                                            Between
                                        </span>
                                        <span class="budget-amount-box font12 text-bold">
                                            <span class="fa fa-rupee"></span>
                                            <span class="font14 text-bold">1</span>
                                            <span class="budget-amount-text-box font14 text-bold">Lac</span>
                                            <span class="font14 text-bold"> - </span>
                                            <span class="fa fa-rupee"></span>
                                            <span class="font14 text-bold">1.5</span>
                                            <span class="budget-amount-text-box font14 text-bold">Lac</span>
                                        </span>   
                                    </a>
                                </li>
                                <li>
                                    <a href="/m/new/search.aspx?budget=200000-">
                                    	<span class="budget-title-box font14">
                                            Above
                                        </span>
                                        <span class="budget-amount-box font12">
                                            <span class="fa fa-rupee"></span>
                                            <span class="font14 text-bold">2</span>
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
    <section class="home-getFinalPrice-banner"><!--  Get Final Price code starts here -->
        <BW:MOnRoadPricequote ID="MOnRoadPricequote" runat="server"/> 
    </section><!-- Get Final Price code Ends here -->
    <section class="home-compare"><!--  Compare section code starts here -->
        <BW:CompareBike ID="ctrlCompareBikes" runat="server"/>
    </section><!-- Compare code Ends here -->    
    
 <% 
            if (ctrlNews.FetchedRecordsCount > 0) { reviewTabsCnt++; }
            if (ctrlExpertReviews.FetchedRecordsCount > 0) { reviewTabsCnt++; }
            if (ctrlVideos.FetchedRecordsCount > 0) { reviewTabsCnt++; }
    %>
    <section class="container <%= reviewTabsCnt == 0 ? "hide" : "" %>">
            <!--  News, reviews and videos code starts here -->
            <div class="container">
                <div class="grid-12">
                    <h2 class="text-center margin-top30 margin-bottom20">Latest Updates</h2>
                    <div class="bw-tabs-panel">
                        <div class="bw-tabs margin-bottom15 <%= reviewTabsCnt == 1 ? "hide" : "" %>">
                            <div class="form-control-box">
                                <select class="form-control">
                                    <option class=" <%= (Convert.ToInt32(ctrlNews.FetchedRecordsCount) > 0) ? "" : "hide" %> active" value="ctrlNews">News</option>
                                    <option class="<%= (Convert.ToInt32(ctrlExpertReviews.FetchedRecordsCount) > 0) ? "" : "hide" %>" value="ctrlExpertReviews">Reviews</option>
                                    <option class="<%= (Convert.ToInt32(ctrlVideos.FetchedRecordsCount) > 0) ? "" : "hide" %>" value="ctrlVideos">Videos</option>
                                   
                                </select>
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
<script type="text/javascript" src="/m/src/home.js?<%= staticFileVersion %>"></script>
</form>
</body>
</html>
