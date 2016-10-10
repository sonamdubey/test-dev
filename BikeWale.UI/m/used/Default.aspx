<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Used.Default" %>
<%@ Register Src="~/m/controls/UsedRecentBikes.ascx" TagPrefix="BW" TagName="RecentUsedBikes" %>
<!DOCTYPE html>
<html>
<head>
<%
    title="Used Bikes in India - Buy & Sell Second Hand Bikes";
    description="With more than 10,000 used bikes listed for sale, BikeWale is India's largest source of used bikes in India. Find a second hand bike or list your bike for sale.";
    keywords="Used bikes, used bike, used bikes for sale, second hand bikes, buy used bike";        
    canonical="http://www.bikewale.com/used/";
%>
    <!-- #include file="/includes/headscript_mobile_min.aspx" -->
    <style type="text/css">
        @charset "utf-8";.text-truncate,.used-swiper .model-details-label{text-overflow:ellipsis;white-space:nowrap;overflow:hidden}.used-bikes-banner{background:url(http://imgd3.aeplcdn.com/0x0/bw/static/landing-banners/m/used-landing-banner.jpg?v1=07Oct2016) center no-repeat #53442a;background-size:cover;height:130px}.text-white{color:#fff}.section-container{margin-bottom:25px}h2.section-heading{margin-bottom:15px}#search-used-bikes .content-box-shadow{margin-top:-25px;-moz-box-shadow:2px 2px 2px rgba(0,0,0,.2);-webkit-box-shadow:2px 2px 2px rgba(0,0,0,.2);-o-box-shadow:2px 2px 2px rgba(0,0,0,.2);-ms-box-shadow:2px 2px 2px rgba(0,0,0,.2);box-shadow:2px 2px 2px rgba(0,0,0,.2)}.form-selection-box{width:100%;padding:10px;border:1px solid #e2e2e2;border-radius:2px;position:relative;text-align:left;cursor:pointer;color:#82888b}#budget-list-box::after,#budget-list-box::before{border-left:10px solid transparent;border-right:10px solid transparent;content:"";left:46.5%;z-index:1}.fa-angle-down{background:url(http://imgd1.aeplcdn.com/0x0/bw/static/design15/old-images/d/dropArrowBg.png?v1=03Mar2016) no-repeat #fff;background-position:96% 50%!important}#city-slider,#search-form-budget li:hover{background:#f5f5f5}.text-underline{text-decoration:underline}#more-brand-tab:hover,.btn-inv-teal:hover,.btn-teal:hover,.swiper-card a:hover{text-decoration:none}.margin-bottom30{margin-bottom:30px}#budget-list-box{display:none;position:absolute;top:39px;left:0;z-index:1;width:100%;background:#fff;border:1px solid #e2e2e2;border-radius:0 0 2px 2px}.input-label-box{display:inline-block;margin:5px 0;padding:3px 8px;width:49%;font-size:14px;color:#82888b}#search-form-budget #min-input-label,#search-form-budget.max-active #max-input-label{color:#4d5057}#search-form-budget.max-active #min-input-label{color:#82888b}#max-budget-list{display:none}#search-form-budget li{display:block;padding:5px 20px;color:#82888b;cursor:pointer;background:0 0}#budget-list-box::before{border-bottom:10px solid #e2e2e2;position:absolute;top:-11px}#budget-list-box::after{border-bottom:10px solid #f5f5f5;position:absolute;top:-10px}#min-max-budget-box.open .fa-angle-down{-moz-transform:rotateZ(180deg);-webkit-transform:rotateZ(180deg);-o-transform:rotateZ(180deg);-ms-transform:rotateZ(180deg);transform:rotateZ(180deg)}#min-max-budget-box.open+#budget-list-box{display:block}#city-slider,#profile-id-popup{display:none}#city-slider.bwm-fullscreen-popup{padding:0;bottom:0}.city-slider-input{width:100%}.city-slider-input input{padding:10px 50px}.slider-back-arrow{height:30px;width:40px;position:absolute;left:5px;top:5px;z-index:11;cursor:pointer}.back-long-arrow-left{position:absolute;top:7px;left:10px}.slider-list{padding-bottom:40px}.noResult,.slider-list li{padding:15px 20px;color:#333;font-size:14px}.slider-list li{border-top:1px solid #e2e2e2;cursor:pointer}.slider-list li:first-child{border-top:0}.slider-list li:hover{background:#ededed}#city-slider.input-fixed{padding-top:41px}#city-slider.input-fixed .city-slider-input{position:fixed;top:0;left:0}.btn-teal{background:#41b4c4;color:#fff;border:1px solid #41b4c4}.btn-teal:hover{background:#58bdcb;border:1px solid #58bdcb}.btn-inv-teal{background:0 0;color:#3799a7;border:1px solid #3799a7}.btn-inv-teal.active{background:#3799a7;color:#fff}.inv-teal-sm{font-size:14px;padding:6px 19px}.teal-next{width:6px;height:10px;background-position:-69px -437px;margin-left:6px}#profile-id-popup.bwm-fullscreen-popup{padding:30px 30px 100px}.close-btn{width:16px;height:16px;cursor:pointer;background-position:-36px -226px}.size-small .icon-outer-container{width:72px;height:72px}.size-small .icon-inner-container{width:64px;height:64px;margin:3px auto}.btn-fixed-width{padding-right:0;padding-left:0;width:205px}input[type=text]:focus,input[type=number]:focus{outline:0;box-shadow:none}.input-box{height:60px;text-align:left}.input-box input{width:100%;display:block;padding:7px 0;border-bottom:1px solid #82888b;font-weight:700;color:#4d5057}.input-box label{position:absolute;top:4px;left:0;font-size:16px;color:#82888b;-webkit-transition:.2s ease all;-moz-transition:.2s ease all;-o-transition:.2s ease all;transition:.2s ease all}.input-number-box input{padding-left:25px}.input-number-prefix{display:none;position:absolute;top:7px;font-weight:700;color:#82888b}.boundary{position:relative;width:100%;display:block}.boundary:after,.boundary:before{content:'';position:absolute;bottom:0;width:0;height:2px;background-color:#41b4c4;-webkit-transition:.2s ease all;-moz-transition:.2s ease all;-o-transition:.2s ease all;transition:.2s ease all}.boundary:before{left:50%}.boundary:after{right:50%}.error-text{display:none;font-size:12px;position:relative;top:4px;left:0;color:#d9534f}.input-box.input-number-box input:focus~.input-number-prefix,.input-box.input-number-box.not-empty .input-number-prefix,.input-box.invalid .error-text,.used-sprite{display:inline-block}.input-box input:focus~label,.input-box.not-empty label{top:-14px;font-size:12px}.input-box input:focus~.boundary:after,.input-box input:focus~.boundary:before{width:50%}.input-box.invalid .boundary:after,.input-box.invalid .boundary:before{background-color:#d9534f;width:50%}.used-sprite{background:url(http://imgd2.aeplcdn.com/0x0/bw/static/sprites/m/used-sprite.png?v1=04Oct2016) no-repeat}.buyer,.contact-buyer,.free-cost,.listing-time{width:20px;height:20px;margin-right:10px;vertical-align:middle}.free-cost{background-position:0 -35px}.buyer{background-position:0 -60px}.listing-time{background-position:0 -83px}.contact-buyer{background-position:0 -108px}.profile-id-icon{width:30px;height:30px;background-position:0 0}#sell-benefit-list li{font-size:14px;text-align:left;margin-bottom:15px}#more-brand-nav{display:none}.brand-type-container ul li{display:inline-block;vertical-align:top;width:90px;height:65px;margin-bottom:15px}.brand-type-title{display:block;text-transform:capitalize}.brand-type-container li a{text-decoration:none;color:#565a5c;display:inline-block}.brand-type-container ul li:hover span.brand-type-title{font-weight:700}.brandlogosprite{background:url(http://imgd1.aeplcdn.com/0x0/bw/static/sprites/m/brand-type-sprite.png?v=10Oct2016) no-repeat;display:inline-block}.brand-1,.brand-10,.brand-11,.brand-12,.brand-13,.brand-14,.brand-15,.brand-16,.brand-17,.brand-18,.brand-19,.brand-2,.brand-20,.brand-22,.brand-3,.brand-34,.brand-37,.brand-39,.brand-4,.brand-40,.brand-41,.brand-42,.brand-5,.brand-6,.brand-7,.brand-71,.brand-8,.brand-81,.brand-9{height:30px}.brand-2{width:52px;background-position:0 0}.brand-7{width:34px;background-position:-58px 0}.brand-1{width:53px;background-position:-97px 0}.brand-8{width:60px;background-position:-156px 0}.brand-12{width:40px;background-position:-222px 0}.brand-40{width:75px;background-position:-268px 0}.brand-34{width:73px;background-position:-349px 0}.brand-22{width:73px;background-position:-428px 0}.brand-3{width:26px;background-position:-507px 0}.brand-17{width:52px;background-position:-539px 0}.brand-15{width:71px;background-position:-597px 0}.brand-4{width:26px;background-position:-674px 0}.brand-9{width:59px;background-position:-706px 0}.brand-16{width:70px;background-position:-771px 0}.brand-5{width:35px;background-position:-847px 0}.brand-19{width:73px;background-position:-889px 0}.brand-13{width:73px;background-position:-968px 0}.brand-6{width:38px;background-position:-1047px 0}.brand-10{width:61px;background-position:-1091px 0}.brand-14{width:76px;background-position:-1159px 0}.brand-39{width:53px;background-position:-1242px 0}.brand-20{width:49px;background-position:-1300px 0}.brand-11{width:74px;background-position:-1354px 0}.brand-41{width:40px;background-position:-1432px 0}.brand-42{width:38px;background-position:-1481px 0}.brand-81{width:38px;background-position:-1529px 0}.brand-71{width:23px;background-position:-1577px 0}.brand-37{width:70px;background-position:-1610px 0}.brand-18{width:37px;background-position:-1690px 0}.city-sprite{background:url(http://imgd3.aeplcdn.com/0x0/bw/static/sprites/m/bwm-city-sprite-v2.png?v07Oct2016) no-repeat;display:inline-block}.c1-icon,.c10-icon,.c105-icon,.c12-icon,.c176-icon,.c198-icon,.c2-icon,.c220-icon{height:48px}.c1-icon{width:78px;background-position:0 0}.c12-icon{width:112px;background-position:-84px 0}.c2-icon{width:82px;background-position:-202px 0}.c10-icon{width:38px;background-position:-290px 0}.c176-icon{width:33px;background-position:-334px 0}.c105-icon{width:34px;background-position:-373px 0}.c198-icon{width:110px;background-position:-413px 0}.c220-icon{width:106px;background-position:-529px 0}.card-container{padding-top:5px;padding-bottom:5px}.swiper-details-block{text-align:left}.swiper-city .swiper-slide{width:185px;height:155px;background:#fff}.swiper-city .swiper-card{width:185px;min-height:155px}.swiper-city .swiper-image-preview{height:80px;padding-top:20px}.swiper-city .swiper-details-block{padding:10px 20px 20px}.used-swiper .swiper-slide{width:200px;min-height:200px;background:#fff;text-align:left}.used-swiper .swiper-image-preview{width:100%;height:auto;padding:5px 5px 0;display:table;text-align:center}.used-swiper .image-thumbnail{position:relative;display:table-cell;vertical-align:middle;background:#f5f5f5}.used-swiper .swiper-image-preview img{height:101px}.used-swiper .swiper-details-block{padding:5px 10px 10px}.used-swiper .model-details-label{width:80%;font-size:11px;display:inline-block;vertical-align:middle;color:#82888b;text-align:left}.author-grey-icon-xs,.kms-driven-icon-xs,.model-date-icon-xs,.model-loc-icon-xs{width:9px;height:10px;margin-right:3px;vertical-align:middle}.model-date-icon-xs{background-position:-140px -502px}.kms-driven-icon-xs{background-position:-156px -502px}.model-loc-icon-xs{background-position:-173px -502px}.author-grey-icon-xs{background-position:-196px -486px}.swiper-card{margin-left:5px;width:200px;min-height:200px;border:1px solid #e2e2e2\9;background:#fff;-webkit-box-shadow:0 1px 4px rgba(0,0,0,.2);-moz-box-shadow:0 1px 4px rgba(0,0,0,.2);-ms-box-shadow:0 1px 4px rgba(0,0,0,.2);box-shadow:0 1px 4px rgba(0,0,0,.2);-webkit-border-radius:2px;-moz-border-radius:2px;-ms-border-radius:2px;border-radius:2px}.swiper-wrapper .swiper-slide:first-child{margin-left:10px}
    </style>
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_mobile.aspx" -->
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <% if(viewModel!= null){ %>
        <!-- #include file="/includes/headBW_Mobile.aspx" -->

        <section>
            <div class="container">
                <div class="used-bikes-banner text-center text-white">
                    <h1 class="font24 text-uppercase text-white padding-top20 margin-bottom10">Used Bikes</h1>
                    <h2 class="font14 text-white text-unbold">View wide range of used bikes</h2>
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
        </section> --%>
        <% if( viewModel.TopMakeList!= null && viewModel.OtherMakeList!= null){  %>
        <section>
            <div class="container text-center section-container">
                <h2 class="font18 section-heading">Search used bikes by brands</h2>
                <div class="bg-white box-shadow brand-type-container content-inner-block-20">
                    <ul id="main-brand-list">
                        <% foreach(var bike in viewModel.TopMakeList){ %>    
                        <li>
                            <a href="/m/used/<%=bike.MaskingName %>-bikes-in-india/" title="<%=bike.MakeName %> used bikes in India">
                                <span class="brand-type">
                                    <span class="brandlogosprite brand-<%=bike.MakeId %>"></span>
                                </span>
                                <span class="brand-type-title"><%=bike.MakeName %></span>
                            </a>
                        </li>
                        <% } %>
                    </ul>

                    <ul id="more-brand-nav" class="brand-style-moreBtn brandTypeMore border-top1 padding-top25 text-center">
                        <% foreach(var bike in viewModel.OtherMakeList){ %> 
                        <li>
                            <a href="/m/used/<%=bike.MaskingName %>-bikes-in-india/" title="<%=bike.MakeName %> used bikes in India">
                                <span class="brand-type">
                                    <span class="brandlogosprite brand-<%=bike.MakeId %>"></span>
                                </span>
                                <span class="brand-type-title"><%=bike.MakeName %></span>
                            </a>
                        </li>
                        <% } %>
                    </ul>

                    <div class="text-center">
                        <a href="javascript:void(0)" id="more-brand-tab" class="font14" rel="nofollow">View more brands</a>
                    </div>
                </div>
            </div>
        </section>
        <% } %>
        <% if(viewModel.objCitiesWithCount != null) { %>
        <section>
            <div class="container text-center section-container">
                <h2 class="font18 section-heading">Search used bikes by cities</h2>
                <div class="bg-white box-shadow padding-top20 padding-bottom20">
                    <div class="swiper-container card-container swiper-city">
                        <div class="swiper-wrapper">
                                   <%foreach (Bikewale.Entities.Used.UsedBikeCities objCity in viewModel.objCitiesWithCount)
                                     {%>
                                        <div class="swiper-slide">
                                <div class="swiper-card">
                                    <a href="/m/used/bikes-in-<%=objCity.CityMaskingName %>/" title="Used bikes in <%=objCity.CityName %>">
                                        <div class="swiper-image-preview">
                                            <span class="city-sprite c<%=objCity.CityId %>-icon"></span>
                                        </div>
                                        <div class="swiper-details-block">
                                            <h3 class="font14 margin-bottom5"><%=objCity.CityName %></h3>
                                            <p class="font14 text-light-grey"><%=objCity.bikesCount %> Used bikes</p>
                                        </div>
                                    </a>
                                </div>
                            </div>
                                    <%} %>
                         </div>
                    </div>
                    <a href="/m/used/browse-bikes-by-cities/" class="btn btn-inv-teal inv-teal-sm margin-top10">View all cities<span class="bwmsprite teal-next"></span></a>
                </div>
            </div>
        </section>
        <% } %>
        <section>
            <!-- Similar used bikes starts -->
            <BW:RecentUsedBikes ID="ctrlRecentUsedBikes" runat="server" />
            <!-- Similar used bikes ends -->
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
                <%foreach(var city in viewModel.Cities){ %>
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
        <% } %>
    </form>
</body>
</html>
