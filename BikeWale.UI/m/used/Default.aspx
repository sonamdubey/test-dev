<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Bikewale.Mobile.Used.Default" %>

<!DOCTYPE html>
<html>
<head>
    <title>Used bikes in India</title>
    <!-- #include file="/includes/headscript_mobile_min.aspx" -->
    <link href="/m/css/used/landing.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_mobile.aspx" -->
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->

        <section>
            <div class="container">
                <div class="used-bikes-banner text-center text-white">
                    <h1 class="font24 text-uppercase text-white padding-top20 margin-bottom10">Used Bikes</h1>
                    <p class="font14 text-white">View wide range of used bikes</p>
                </div>
                <!-- Top banner code ends here -->
            </div>
        </section>

        <section>
            <div class="container section-container">
                <div id="search-used-bikes" class="grid-12">
                    <div class="content-box-shadow bg-white content-inner-block-20 text-center">
                        <h2 class="font18 section-heading">Search used bikes</h2>
                        <div id="search-form-control-box" class="margin-top5 margin-bottom20">
                            <div id="search-form-city" class="form-selection-box margin-bottom20">
                                <p class="text-truncate">Select city</p>
                            </div>
                            <div id="search-form-budget" class="position-rel margin-bottom30">
                                <div id="min-max-budget-box" class="form-selection-box">
                                    <span id="budget-default-label">Select budget</span>
                                    <span id="min-amount"></span>
                                    <span id="max-amount"></span>
                                    <span id="upDownArrow" class="fa fa-angle-down position-abt pos-top15 pos-right10"></span>
                                    <div class="clear"></div>
                                </div>
                                <div id="budget-list-box">
                                    <div id="user-budget-input" class="bg-light-grey text-light-grey">
                                        <div id="min-input-label" class="input-label-box border-solid-right">Min</div><div id="max-input-label" class="input-label-box">Max</div>
                                    </div>
                                    <ul id="min-budget-list" class="text-left"></ul>
                                    <ul id="max-budget-list" class="text-right"></ul>
                                </div>                                
                            </div>
                            <a href="" class="btn btn-orange text-bold">Search</a>
                        </div>
                        <a href="" id="profile-id-popup-target" class="font14 text-underline">Search by Profile ID</a>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section>
            <div class="container text-center section-container">
                <h2 class="font18 section-heading">Best way to sell your bike</h2>
                <div class="bg-white box-shadow content-inner-block-20">
                    <ul id="sell-benefit-list">
                        <li>
                            <span class="used-sprite free-cost"></span>
                            <span class="inline-block">Free of cost</span>
                        </li>
                        <li>
                            <span class="used-sprite buyer"></span>
                            <span class="inline-block">Genuine buyers</span>
                        </li>
                        <li>
                            <span class="used-sprite listing-time"></span>
                            <span class="inline-block">Unlimited listing duration</span>
                        </li>
                        <li>
                            <span class="used-sprite contact-buyer"></span>
                            <span class="inline-block">Get contact details of buyers</span>
                        </li>
                    </ul>
                    <a href="" class="btn btn-teal margin-top5">Sell now</a>
                </div>
            </div>
        </section>
        
        <section>
            <div class="container text-center section-container">
                <h2 class="font18 section-heading">Search used bikes by brands</h2>
                <div class="bg-white box-shadow brand-type-container content-inner-block-20">
                    <ul id="main-brand-list">    
                        <li>
                            <a href="">
                                <span class="brand-type">
                                    <span class="brandlogosprite brand-honda"></span>
                                </span>
                                <span class="brand-type-title">Honda</span>
                            </a>
                        </li>
                                
                        <li>
                            <a href="">
                                <span class="brand-type">
                                    <span class="brandlogosprite brand-bajaj"></span>
                                </span>
                                <span class="brand-type-title">Bajaj</span>
                            </a>
                        </li>
                                
                        <li>
                            <a href="">
                                <span class="brand-type">
                                    <span class="brandlogosprite brand-hero"></span>
                                </span>
                                <span class="brand-type-title">Hero</span>
                            </a>
                        </li>
                                
                        <li>
                            <a href="">
                                <span class="brand-type">
                                    <span class="brandlogosprite brand-tvs"></span>
                                </span>
                                <span class="brand-type-title">TVS</span>
                            </a>
                        </li>
                                
                        <li>
                            <a href="">
                                <span class="brand-type">
                                    <span class="brandlogosprite brand-royalenfield"></span>
                                </span>
                                <span class="brand-type-title">Royal Enfield</span>
                            </a>
                        </li>
                                
                        <li>
                            <a href="">
                                <span class="brand-type">
                                    <span class="brandlogosprite brand-yamaha"></span>
                                </span>
                                <span class="brand-type-title">Yamaha</span>
                            </a>
                        </li>
                    </ul>

                    <ul id="more-brand-nav" class="brand-style-moreBtn brandTypeMore border-top1 padding-top25 text-center">    
                        <li>
                            <a href="">
                                <span class="brand-type">
                                    <span class="brandlogosprite brand-honda"></span>
                                </span>
                                <span class="brand-type-title">Honda</span>
                            </a>
                        </li>
                                
                        <li>
                            <a href="">
                                <span class="brand-type">
                                    <span class="brandlogosprite brand-bajaj"></span>
                                </span>
                                <span class="brand-type-title">Bajaj</span>
                            </a>
                        </li>
                                
                        <li>
                            <a href="">
                                <span class="brand-type">
                                    <span class="brandlogosprite brand-hero"></span>
                                </span>
                                <span class="brand-type-title">Hero</span>
                            </a>
                        </li>
                                
                        <li>
                            <a href="">
                                <span class="brand-type">
                                    <span class="brandlogosprite brand-tvs"></span>
                                </span>
                                <span class="brand-type-title">TVS</span>
                            </a>
                        </li>
                                
                        <li>
                            <a href="">
                                <span class="brand-type">
                                    <span class="brandlogosprite brand-royalenfield"></span>
                                </span>
                                <span class="brand-type-title">Royal Enfield</span>
                            </a>
                        </li>
                                
                        <li>
                            <a href="">
                                <span class="brand-type">
                                    <span class="brandlogosprite brand-yamaha"></span>
                                </span>
                                <span class="brand-type-title">Yamaha</span>
                            </a>
                        </li>
                    </ul>

                    <div class="text-center">
                        <a href="javascript:void(0)" id="more-brand-tab" class="font14" rel="nofollow">View more brands</a>
                    </div>
                </div>
            </div>
        </section>

        <section>
            <div class="container text-center section-container">
                <h2 class="font18 section-heading">Search used bikes by cities</h2>
                <div class="bg-white box-shadow padding-top20 padding-bottom20">

                    <a href="" class="btn btn-inv-teal inv-teal-sm">View all cities<span class="bwmsprite teal-next"></span></a>
                </div>
            </div>
        </section>

        <section>
            <div class="container text-center section-container">
                <h2 class="font18 section-heading">Recently uploaded used bikes</h2>
                <div class="bg-white box-shadow padding-top20 padding-bottom20">
                    <div class="swiper-container card-container swiper-200">
                        <div class="swiper-wrapper">
                            <div class="swiper-slide">
                                <div class="swiper-card">
                                    <a href="" title="Bajaj Pulsar CS400">
                                        <div class="swiper-image-preview">
                                            <img class="swiper-lazy" alt="Bajaj Pulsar CS400" data-src="http://imgd1.aeplcdn.com///310x174//bw/upcoming/bajaj-pulsarcs400-551.jpg?20151209053107" />
                                            <span class="swiper-lazy-preloader"></span>
                                        </div>
                                        <div class="swiper-details-block">
                                            <h3 class="target-link font12 margin-bottom5">Bajaj Pulsar CS400</h3>
                                            <div class="text-light-grey margin-bottom10"><p class="font11 text-light-grey">Expected launch</p><p class="font16 text-bold text-default">Jun 2016</p></div>
                                            <div>
                                                <div class=" font11 text-light-grey">Expected Price</div>
                                                <div class="text-default"><span class="bwmsprite inr-xsm-icon"></span> <span class="text-bold font16">1,60,000</span><span class="font14"> onwards</span></div>
                                            </div>
                                        </div>
                                    </a>
                                </div>
                            </div>
                        </div>
                    </div>
                    <a href="" class="btn btn-inv-teal inv-teal-sm">View all bikes<span class="bwmsprite teal-next"></span></a>
                </div>
            </div>
        </section>



        <div id="city-slider" class="bwm-fullscreen-popup">  
            <div class="city-slider-input position-rel">
                <span id="close-city-slider" class="slider-back-arrow back-arrow-box">
                    <span class="bwmsprite back-long-arrow-left"></span>
                </span>
                <input class="form-control border-solid" type="text" id="getCityInput" placeholder="Select City" />
            </div>
            <ul id="city-slider-list" class="slider-list">
                <li>Mumbai</li>
                <li>Bangalore</li>
                <li>Navi Mumbai</li>
                <li>Pune</li>
                <li>Thane</li>
                <li>Panvel</li>
                <li>Mumbai</li>
                <li>Bangalore</li>
                <li>Navi Mumbai</li>
                <li>Pune</li>
                <li>Thane</li>
                <li>Panvel</li>
                <li>Mumbai</li>
                <li>Bangalore</li>
                <li>Navi Mumbai</li>
                <li>Pune</li>
                <li>Thane</li>
                <li>Panvel</li>
            </ul>
        </div>


        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>
        <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/common.min.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/used/landing.js?<%= staticFileVersion %>"></script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />
    </form>
</body>
</html>
