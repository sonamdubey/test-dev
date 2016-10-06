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
                                <p class="text-truncate" data-item-id="" data-bind="attr: {'data-citymaskingname': cityMaskingName,'data-item-id':cityId}" >Select city</p>
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
                            <a data-bind="attr: {href:redirectUrl}" id="searchCityBudget" class="btn btn-orange text-bold">Search</a>
                        </div>
                        <a href="javascript:void(0)" id="profile-id-popup-target" class="font14 text-underline" rel="nofollow">Search by Profile ID</a>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

       <%-- <section>
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
        --%>
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
                    <div class="swiper-container card-container swiper-city">
                        <div class="swiper-wrapper">
                            <div class="swiper-slide">
                                <div class="swiper-card">
                                    <a href="" title="">
                                        <div class="swiper-image-preview">
                                            <span class="city-sprite mumbai-icon"></span>
                                        </div>
                                        <div class="swiper-details-block">
                                            <h3 class="font14 margin-bottom5">Mumbai</h3>
                                            <p class="font14 text-light-grey">132 Used bikes</p>
                                        </div>
                                    </a>
                                </div>
                            </div>

                            <div class="swiper-slide">
                                <div class="swiper-card">
                                    <a href="" title="">
                                        <div class="swiper-image-preview">
                                            <span class="city-sprite chennai-icon"></span>
                                        </div>
                                        <div class="swiper-details-block">
                                            <h3 class="font14 margin-bottom5">Chennai</h3>
                                            <p class="font14 text-light-grey">132 Used bikes</p>
                                        </div>
                                    </a>
                                </div>
                            </div>

                            <div class="swiper-slide">
                                <div class="swiper-card">
                                    <a href="" title="">
                                        <div class="swiper-image-preview">
                                            <span class="city-sprite pune-icon"></span>
                                        </div>
                                        <div class="swiper-details-block">
                                            <h3 class="font14 margin-bottom5">Pune</h3>
                                            <p class="font14 text-light-grey">132 Used bikes</p>
                                        </div>
                                    </a>
                                </div>
                            </div>
                        </div>
                    </div>
                    <a href="" class="btn btn-inv-teal inv-teal-sm margin-top10">View all cities<span class="bwmsprite teal-next"></span></a>
                </div>
            </div>
        </section>

        <section>
            <div class="container text-center section-container">
                <h2 class="font18 section-heading">Recently uploaded used bikes</h2>
                <div class="bg-white box-shadow padding-top20 padding-bottom20">
                    <div class="swiper-container card-container used-swiper">
                        <div class="swiper-wrapper">
                            <div class="swiper-slide">
                                <div class="swiper-card">
                                    <a href="" title="Bajaj Pulsar CS400">
                                        <div class="swiper-image-preview">
                                            <div class="image-thumbnail">
                                                <img class="swiper-lazy" alt="Bajaj Pulsar CS400" data-src="http://imgd4.aeplcdn.com//210x118//bw/used/S42611/42611_20160614111649006.jpg" />
                                                <span class="swiper-lazy-preloader"></span>
                                            </div>
                                        </div>
                                        <div class="swiper-details-block">
                                            <h3 class="target-link font12 margin-bottom5 text-truncate">Bajaj Pulsar CS400 Bajaj Pulsar CS400</h3>
                                            <div class="grid-6 alpha omega">
                                                <span class="bwmsprite model-date-icon-xs"></span>
                                                <span class="model-details-label">2012 model</span>
                                            </div>
                            
                                            <div class="grid-6 alpha omega">
                                                <span class="bwmsprite author-grey-icon-xs"></span>
                                                <span class="model-details-label">1st owner</span>
                                            </div>
                            
                                            <div class="grid-6 alpha omega">
                                                <span class="bwmsprite model-loc-icon-xs"></span>
                                                <span class="model-details-label">Ahmedabad</span>
                                            </div>
                            
                                            <div class="clear"></div>
                                            <p class="margin-top5 text-default"><span class="bwmsprite inr-xsm-icon"></span>&nbsp;<span class="font16 text-bold">40,000</span></p>
                                        </div>
                                    </a>
                                </div>
                            </div>

                            <div class="swiper-slide">
                                <div class="swiper-card">
                                    <a href="" title="Bajaj Pulsar CS400">
                                        <div class="swiper-image-preview">
                                            <div class="image-thumbnail">
                                                <img class="swiper-lazy" alt="Bajaj Pulsar CS400" data-src="http://imgd4.aeplcdn.com//210x118//bw/used/S42598/42598_20160613081003622.jpg" />
                                                <span class="swiper-lazy-preloader"></span>
                                            </div>
                                        </div>
                                        <div class="swiper-details-block">
                                            <h3 class="target-link font12 margin-bottom5 text-truncate">Bajaj Pulsar CS400</h3>
                                            <div class="grid-6 alpha omega">
                                                <span class="bwmsprite model-date-icon-xs"></span>
                                                <span class="model-details-label">2012 model</span>
                                            </div>
                            
                                            <div class="grid-6 alpha omega">
                                                <span class="bwmsprite kms-driven-icon-xs"></span>
                                                <span class="model-details-label">49,990 kms</span>
                                            </div>
                            
                                            <div class="grid-6 alpha omega">
                                                <span class="bwmsprite author-grey-icon-xs"></span>
                                                <span class="model-details-label">1st owner</span>
                                            </div>
                            
                                            <div class="grid-6 alpha omega">
                                                <span class="bwmsprite model-loc-icon-xs"></span>
                                                <span class="model-details-label">Ahmedabad</span>
                                            </div>
                            
                                            <div class="clear"></div>
                                            <p class="margin-top5 text-default"><span class="bwmsprite inr-xsm-icon"></span>&nbsp;<span class="font16 text-bold">40,000</span></p>
                                        </div>
                                    </a>
                                </div>
                            </div>
                        </div>
                    </div>
                    <a href="" class="btn btn-inv-teal inv-teal-sm margin-top15">View all bikes<span class="bwmsprite teal-next"></span></a>
                </div>
            </div>
        </section>
        
        <!-- city slider -->
        <div id="city-slider" class="bwm-fullscreen-popup">  
            <div class="city-slider-input position-rel">
                <span id="close-city-slider" class="slider-back-arrow back-arrow-box">
                    <span class="bwmsprite back-long-arrow-left"></span>
                </span>
                <input class="form-control border-solid" type="text" id="getCityInput" placeholder="Select City" />
            </div>
            <ul id="city-slider-list" class="slider-list">
                <%foreach(var city in cities){ %>
                <li id="selectedCity" data-item-id="<%=city.CityId%>" data-citymaskingname="<%=city.CityMaskingName%>"><%=city.CityName %></li>
                <%} %>
                </ul>
        </div>
                
        <!-- profile-id -->
        <div id="profile-id-popup" class="bwm-fullscreen-popup text-center size-small">
            <div class="bwmsprite close-btn position-abt pos-top20 pos-right20"></div>
            <div class="icon-outer-container rounded-corner50percent">
                <div class="icon-inner-container rounded-corner50percent">
                    <span class="used-sprite profile-id-icon margin-top15"></span>
                </div>
            </div>
            <p class="font18 text-bold margin-top10 margin-bottom10">Search by Profile ID</p>
            <p class="font14 text-light-grey margin-bottom30">If you like a certain listing you can search it by its Profile ID. The unique Profile ID is mentioned in the Ad details.</p>
            <div class="input-box form-control-box margin-bottom15">
                <input type="text" id="listingProfileId">
                <label for="listingProfileId">Profile ID</label>
                <span class="boundary"></span>
                <span class="error-text"></span>
            </div>
            <a class="btn btn-orange btn-fixed-width" id="search-profile-id-btn">Search</a>
        </div>
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/common.min.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/used/landing.js?<%= staticFileVersion %>"></script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />
    </form>
</body>
</html>
