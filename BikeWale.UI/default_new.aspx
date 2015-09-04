<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.default_new" %>
<%@ Register Src="~/controls/News_new.ascx" TagName="News" TagPrefix="BW"  %>
<%@ Register Src="~/controls/ExpertReviews.ascx" TagName="ExpertReviews" TagPrefix="BW"  %>
<%@ Register Src="~/controls/VideosControl.ascx" TagName="Videos" TagPrefix="BW"  %>
<!-- #include file="/includes/headBW.aspx" -->
<header class="home-top-banner">    	
        <div class="container">
        	<div class="welcome-box">
                <h1 class="text-uppercase margin-bottom10">BOOK YOUR DREAM BIKE</h1>
                <p class="font20">Get Exclusive Offers, Discounts and Freebies on your Bike Purchase</p>
                <div class="margin-top60">
                    <div>
                    	<div class="bike-search-container">
                            <div class="bike-search new-bike-search position-rel">
                               	<input type="text" placeholder="Search your bike here Ex. Bajaj" id="newBikeList">
                                <span class="fa fa-spinner fa-spin position-abt pos-right10 pos-top15 text-black" style="display:none"></span>
                            </div>
                            <div class="findBtn">
                                <button class="btn btn-orange btn-md font18">Search</button>
                            </div>
                            <div class="clear"></div>
                         </div>
                    </div>
                </div>
            </div>
        </div>
    </header>
    <section class="bg-light-grey"><!--  Booking online code starts here -->
        <div class="container">
        	<div class="grid-12 alpha omega">
                <h2 class="text-bold text-center margin-top50">Comforts of booking online</h2>
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
                    <div class="bg-white font20 booking-online-box">Save on<br /> dealer visits</div>
                </div>
                <div class="grid-3 text-center">
                	<div class="booking-online-pic bg-white text-center">
                    	<div class="bookingcomforts-sprite buying-asst-icon"></div>
                    </div>
                    <div class="bg-white font20 booking-online-box">Complete<br /> buying assistance</div>
                </div>
                <div class="clear"></div>
                <p class="font16 text-center margin-top20 margin-bottom30"><a href="#">Get more details</a></p>
            </div>
            <div class="clear"></div>
        </div>
    </section>
    <!--  Ends here -->
    <section class="bg-white"><!--  Discover your bike code starts here -->
        <div class="container">
        	<div class="grid-12">
                <h2 class="text-bold text-center margin-top50 margin-bottom30">Discover your bike</h2>
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
                                    <a href="#">
                                        <span class="brand-type">
                                            <span class="brandlogosprite brand-bajaj"></span>
                                        </span>
                                        <span class="brand-type-title">Bajaj</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="#">
                                        <span class="brand-type">
                                             <span class="brandlogosprite brand-hero"></span>
                                        </span>
                                        <span class="brand-type-title">Hero</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="#">
                                        <span class="brand-type">
                                            <span class="brandlogosprite brand-honda"></span>
                                        </span>
                                        <span class="brand-type-title">Honda</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="#">
                                        <span class="brand-type">
                                             <span class="brandlogosprite brand-yamaha"></span>
                                        </span>
                                        <span class="brand-type-title">Yamaha</span>
                                    </a>
                                </li>
                            </ul>
                            <ul class="brand-style-moreBtn brandTypeMore hide text-center">
                                <li>
                                    <a href="#">
                                        <span class="brand-type">
                                            <span class="brandlogosprite brand-honda"></span>
                                        </span>
                                        <span class="brand-type-title">Honda</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="#">
                                        <span class="brand-type">
                                             <span class="brandlogosprite brand-hero"></span>
                                        </span>
                                        <span class="brand-type-title">Hero</span>
                                    </a>
                                </li>
                                <li>
                                    <a href="#">
                                        <span class="brand-type">
                                            <span class="brandlogosprite brand-bajaj"></span>
                                        </span>
                                        <span class="brand-type-title">Bajaj</span>
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
                                    	<span class="budget-title-box font16">
                                            Upto
                                        </span>
                                        <span class="budget-amount-box font20">
                                            <span class="fa fa-rupee"></span>
                                            <span class="font24">50,000</span>
                                        </span>   
                                    </a>
                                </li>
                                <li>
                                    <a href="#">
                                    	<span class="budget-title-box font16">
                                            Between
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
                                    	<span class="budget-title-box font16">
                                            Between
                                        </span>
                                        <span class="budget-amount-box font20">
                                            <span class="fa fa-rupee"></span>
                                            <span class="font24">1</span>
                                            <span class="budget-amount-text-box font16">Lakhs</span>
                                            <span class="font24"> - </span>
                                            <span class="fa fa-rupee"></span>
                                            <span>1.5</span>
                                            <span class="budget-amount-text-box font16">Lakhs</span>
                                        </span>   
                                    </a>
                                </li>
                                <li>
                                    <a href="#">
                                    	<span class="budget-title-box font16">
                                            Above
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
                                    	<span class="mileage-title-box font16">
                                            Above
                                        </span>
                                        <span class="mileage-amount-box font24">
                                            <span>60 <span class="font16">Kmpl</span></span>
                                        </span>   
                                    </a>
                                </li>
                                <li>
                                    <a href="#">
                                    	<span class="mileage-title-box font16">
                                            Between
                                        </span>
                                        <span class="mileage-amount-box font24">
                                            <span>60</span>
                                            <span class="mileage-amount-text-box font16">Kmpl</span>
                                            <span> - 40</span>
                                            <span class="mileage-amount-text-box font16">Kmpl</span>
                                        </span>   
                                    </a>
                                </li>
                                <li>
                                    <a href="#">
                                    	<span class="mileage-title-box font16">
                                            Between
                                        </span>
                                        <span class="mileage-amount-box font24">
                                            <span>40</span>
                                            <span class="mileage-amount-text-box font16">Kmpl</span>
                                            <span> - 20</span>
                                            <span class="mileage-amount-text-box font16">Kmpl</span>
                                        </span>   
                                    </a>
                                </li>
                                <li>
                                    <a href="#">
                                    	<span class="mileage-title-box font16">
                                            Upto
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
                    	</div>
                	</div>
                </div>        
        	</div>
            <div class="clear"></div>
        </div>
    </section>
    <!--  Ends here -->
    <section class="home-getFinalPrice-banner"><!--  Get Final Price code starts here -->
        <div class="container">
        	<div class="grid-5 leftfloat">
            	<div class="bg-white content-inner-block-15 light-box-shadow rounded-corner2 margin-top70">
                	<h2 class="text-bold margin-bottom20">On road price</h2>
                    <div class="form-control-box margin-bottom20">
                    	<input class="form-control" type="text" placeholder="Search Make and Model" id="makemodelFinalPrice">
                        <span class="fa fa-spinner fa-spin position-abt pos-right10 pos-top15 text-black" style="display:none"></span>
                        <span class="bwsprite error-icon"></span>
                        <div class="bw-blackbg-tooltip">Please search a make</div>
                   	</div>
                    <div class="form-control-box margin-bottom20 finalPriceCitySelect">
                        <select class="form-control">
                            <option>Select city</option>
                            <option>Mumbai</option>
                            <option>Navi Mumbai</option>
                            <option>Delhi</option>
                            <option>Banglore</option>
                            <option>Kolkata</option>
                        </select>
                    </div>
                    <div class="form-control-box margin-bottom20 finalPriceAreaSelect hide">
                        <select class="form-control">
                            <option>Select area</option>
                            <option>Mumbai</option>
                            <option>Navi Mumbai</option>
                            <option>Delhi</option>
                            <option>Banglore</option>
                            <option>Kolkata</option>
                        </select>
                    </div>                    
                    <button class="btn btn-orange margin-bottom20">Get price quote</button>
                    <p>Its private, no need to share your number and email</p>
                </div>
            </div>
            <div class="clear"></div>
        </div>
    </section>
    <!--  Ends here -->
    <section class="margin-bottom50"><!--  Compare section code starts here -->
        <div class="container">
        	<h2 class="text-bold text-center margin-top50 margin-bottom30">Compare now</h2>
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
                                    <p class="margin-bottom10">
                                        <img src="images/ratings/1.png">
                                        <img src="images/ratings/1.png">
                                        <img src="images/ratings/1.png">
                                        <img src="images/ratings/1.png">
                                        <img src="images/ratings/0.png">
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
    </section>
    <!-- Ends here -->
    <section class="bg-light-grey"><!--  Used Bikes code starts here -->
        <div class="container">
        	<div class="grid-12">
                <h2 class="text-bold text-center margin-top50 margin-bottom30">Popular used bikes in Mumbai</h2>
                <div class="jcarousel-wrapper popular-used-bikes-container">
                    <div class="jcarousel used-bike-carousel">
                        <ul>
                            <li class="front">
                                <div class="contentWrapper">
                                    <div class="imageWrapper">
                                        <a href="#">
                                            <img src="http://imgd1.aeplcdn.com/600x337/bw/ec/19889/Yamaha-MT-03-Front-three-quarter-56371.jpg?wm=0" title="img title" alt="img title">
                                        </a>
                                    </div>
                                    <div class="bikeDescWrapper">
                                        <div class="bikeTitle margin-bottom15">
                                            <h3><a href="#" title="Hyundai i20 Active">Royal Enfield Thunderbird 500</a></h3>
                                        </div>
                                        <div class="margin-bottom10 font20">
                                            <span class="fa fa-rupee"></span>
                                            <span class="font22">1,60,000</span>
                                            <span class="font16">onwards</span>
                                        </div>
                                        <div class="font16 text-light-grey bikes-avaiable-count">
											<span>28 Bikes Avaiable</span>
                                        </div>
                                    </div>
                                </div>
                            </li>
                            <li class="front">
                                <div class="contentWrapper">
                                    <div class="imageWrapper">
                                        <a href="#">
                                            <img src="http://imgd8.aeplcdn.com/642x361//bikewaleimg/used/S32487/32487_20150713124853159_640x428.jpeg" title="img title" alt="img title">
                                        </a>
                                    </div>
                                    <div class="bikeDescWrapper">
                                        <div class="bikeTitle margin-bottom20">
                                            <h3><a href="#" title="Hyundai i20 Active">Royal Enfield Thunderbird 500</a></h3>
                                        </div>
                                        <div class="margin-bottom15 font20">
                                            <span class="fa fa-rupee"></span>
                                            <span class="font22">50,000</span>
                                            <span class="font16">onwards</span>
                                        </div>
                                        <div class="font16 text-light-grey bikes-avaiable-count">
											<span>28 Bikes Avaiable</span>
                                        </div>
                                    </div>
                                </div>
                            </li>
                            <li class="front">
                                <div class="contentWrapper">
                                    <div class="imageWrapper">
                                        <a href="#">
                                            <img src="http://imgd1.aeplcdn.com/600x337/bw/ec/19889/Yamaha-MT-03-Front-three-quarter-56371.jpg?wm=0" title="img title" alt="img title">
                                        </a>
                                    </div>
                                    <div class="bikeDescWrapper">
                                        <div class="bikeTitle margin-bottom20">
                                            <h3><a href="#" title="Hyundai i20 Active">Royal Enfield Thunderbird 500</a></h3>
                                        </div>
                                        <div class="margin-bottom15 font20">
                                            <span class="fa fa-rupee"></span>
                                            <span class="font22">1,60,000</span>
                                            <span class="font16">onwards</span>
                                        </div>
                                        <div class="font16 text-light-grey bikes-avaiable-count">
											<span>28 Bikes Avaiable</span>
                                        </div>
                                    </div>
                                </div>
                            </li>
                            <li class="front">
                                <div class="contentWrapper">
                                    <div class="imageWrapper">
                                        <a href="#">
                                            <img src="http://imgd8.aeplcdn.com/642x361//bikewaleimg/used/S32487/32487_20150713124853159_640x428.jpeg" title="img title" alt="img title">
                                        </a>
                                    </div>
                                    <div class="bikeDescWrapper">
                                        <div class="bikeTitle margin-bottom20">
                                            <h3><a href="#" title="Hyundai i20 Active">Royal Enfield Thunderbird 500</a></h3>
                                        </div>
                                        <div class="margin-bottom15 font20">
                                            <span class="fa fa-rupee"></span>
                                            <span class="font22">75,000</span>
                                            <span class="font16">onwards</span>
                                        </div>
                                        <div class="font16 text-light-grey bikes-avaiable-count">
											<span>28 Bikes Avaiable</span>
                                        </div>
                                    </div>
                                </div>
                            </li>
                            <li class="front">
                                <div class="contentWrapper">
                                    <div class="imageWrapper">
                                        <a href="#">
                                            <img src="http://imgd1.aeplcdn.com/600x337/bw/ec/19889/Yamaha-MT-03-Front-three-quarter-56371.jpg?wm=0" title="img title" alt="img title">
                                        </a>
                                    </div>
                                    <div class="bikeDescWrapper">
                                        <div class="bikeTitle margin-bottom20">
                                            <h3><a href="#" title="Hyundai i20 Active">Royal Enfield Thunderbird 500</a></h3>
                                        </div>
                                        <div class="margin-bottom15 font20">
                                            <span class="fa fa-rupee"></span>
                                            <span class="font22">1,60,000</span>
                                            <span class="font16">onwards</span>
                                        </div>
                                        <div class="font16 text-light-grey bikes-avaiable-count">
											<span>28 Bikes Avaiable</span>
                                        </div>
                                    </div>
                                </div>
                            </li>
                            <li class="front">
                                <div class="contentWrapper">
                                    <div class="imageWrapper">
                                        <a href="#">
                                            <img src="http://imgd8.aeplcdn.com/642x361//bikewaleimg/used/S32487/32487_20150713124853159_640x428.jpeg" title="img title" alt="img title">
                                        </a>
                                    </div>
                                    <div class="bikeDescWrapper">
                                        <div class="bikeTitle margin-bottom20">
                                            <h3><a href="#" title="Hyundai i20 Active">Royal Enfield Thunderbird 500</a></h3>
                                        </div>
                                        <div class="margin-bottom15 font20">
                                            <span class="fa fa-rupee"></span>
                                            <span class="font22">50,000</span>
                                            <span class="font16">onwards</span>
                                        </div>
                                        <div class="font16 text-light-grey bikes-avaiable-count">
											<span>28 Bikes Avaiable</span>
                                        </div>
                                    </div>
                                </div>
                            </li>
                        </ul>
                    </div>
                    <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev"></a></span>
                    <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next"></a></span>
                    <!--<p class="jcarousel-pagination"></p> -->
                </div>
                <div class="text-center margin-bottom30">
                    <a class="font16" href="#">View complete list</a>
                </div>
        	</div>
            <div class="clear"></div>
        </div>
    </section>
    <!-- Ends here -->
    <section><!--  News Bikes code starts here -->
        <div class="container">
            <div class="grid-12">
                <h2 class="text-bold text-center margin-top50 margin-bottom30">Latest updates from the industry</h2>
                <div class="bw-tabs-panel">
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
                    <%--<div class="bw-tabs-data hide" id="Videos"><!-- Videos data code starts here-->
                        <div class="padding-bottom30">
                            <div class="grid-4 alpha">
                                <div class="yt-iframe-preview">
                                	<iframe frameborder="0" allowtransparency="true" src="https://www.youtube.com/embed/lsSTQxIlOxU?rel=0&showinfo=0&autoplay=0"></iframe>
                                </div>
                            </div>
                            <div class="grid-8 omega">
                                <h2 class="margin-bottom10 font20"><a href="#" class="text-black">First Look Ford Figo Aspire</a></h2>
                                <p class="margin-bottom10 text-light-grey font14">Updated on <span>June 30, 2015</span></p>
                                <div class="margin-bottom15 text-light-grey"><span class="bwsprite review-sm-lgt-grey"></span> Views <span>398</span></div>
                                <div class="text-light-grey"><span class="fa fa-thumbs-o-up text-light-grey margin-right5"></span> Likes <span>120</span></div>
                            </div>
                            <div class="clear"></div>
                        </div>
                        <div class="padding-bottom30">
                            <div class="grid-4 alpha">
                                <div class="yt-iframe-preview">
                                	<iframe frameborder="0" allowtransparency="true" src="https://www.youtube.com/embed/lsSTQxIlOxU?rel=0&showinfo=0&autoplay=0"></iframe>
                                </div>
                            </div>
                            <div class="grid-8 omega">
                                <h2 class="margin-bottom10 font20"><a href="#" class="text-black">First Look Ford Figo Aspire</a></h2>
                                <p class="margin-bottom10 text-light-grey font14">Updated on <span>June 30, 2015</span></p>
                                <div class="margin-bottom15 text-light-grey"><span class="bwsprite review-sm-lgt-grey"></span> Views <span>398</span></div>
                                <div class="text-light-grey"><span class="fa fa-thumbs-o-up text-light-grey margin-right5"></span> Likes <span>120</span></div>
                            </div>
                            <div class="clear"></div>
                        </div>
                        <div class="padding-bottom30">
                            <div class="grid-4 alpha">
                                <div class="yt-iframe-preview">
                                	<iframe frameborder="0" allowtransparency="true" src="https://www.youtube.com/embed/lsSTQxIlOxU?rel=0&showinfo=0&autoplay=0"></iframe>
                                </div>
                            </div>
                            <div class="grid-8 omega">
                                <h2 class="margin-bottom10 font20"><a href="#" class="text-black">First Look Ford Figo Aspire</a></h2>
                                <p class="margin-bottom10 text-light-grey font14">Updated on <span>June 30, 2015</span></p>
                                <div class="margin-bottom15 text-light-grey"><span class="bwsprite review-sm-lgt-grey"></span> Views <span>398</span></div>
                                <div class="text-light-grey"><span class="fa fa-thumbs-o-up text-light-grey margin-right5"></span> Likes <span>120</span></div>
                            </div>
                            <div class="clear"></div>
                        </div>
                        
                        <div class="padding-bottom50 text-center">
                        	<a href="#" class="font16">View more videos</a>
                        </div>
                    </div><!-- Ends here-->--%>
                </div>        
            </div>
            <div class="clear"></div>
        </div>
    </section>
    <!-- Ends here -->
<!-- #include file="/includes/footerBW.aspx" -->