<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Used.Default" EnableViewState="false"%>
<%@ Register Src="~/controls/UsedRecentBikes.ascx" TagPrefix="BW" TagName="RecentUsedBikes" %>
<!DOCTYPE html>

<html>
<head>
    <%
        title = "Used Bikes in India - Buy & Sell Second Hand Bikes";
        description = "With more than 10,000 used bikes listed for sale, BikeWale is India's largest source of used bikes in India. Find a second hand bike or list your bike for sale.";
        keywords = "Used bikes, used bike, used bikes for sale, second hand bikes, buy used bike";
        canonical = "http://www.bikewale.com/used/";
        alternate = "http://www.bikewale.com/m/used/";
        isHeaderFix = false;
        isAd970x90Shown = false;
        isTransparentHeader = true; 
    %>

    <title>Used Bikes in India</title>

    <!-- #include file="/includes/headscript_desktop_min.aspx" -->

    <link type="text/css" href="/css/used/landing.css" rel="stylesheet" />

    <script type="text/javascript">
        <!-- #include file="\includes\gacode_desktop.aspx" -->
    </script>
</head>
<body class="bg-light-grey">
    <form id="form1" runat="server">
        <!-- #include file="/includes/headBW.aspx" -->
        <header class="used-landing-banner">
            <div id="used-landing-box" class="container">
                <div class="welcome-box">
                    <h1 class="font30 text-uppercase margin-bottom30">Used bikes</h1>
                    <h2 class="font20 text-unbold text-white">View wide range of used bikes</h2>
                </div>
            </div>
        </header>     
        <section>
            <div class="container section-container">
                   <div id="search-used-bikes" class="grid-12">
                    <div class="content-box-shadow negative-50 text-center padding-25-30">
                        <h2 class="section-header">Search used bikes</h2>
                        <div class="usedbikes-search-container">
                            <div id="search-form-city" class="form-control-box">
                                 <select class="form-control chosen-select" id="drpCities">
                                     
                                    <option >Select a city</option>
                                    <% foreach(var city in viewModel.Cities){ %> 
                                    <option id="selectedCity" data-item-id="<%=city.CityId%>" data-citymaskingname="<%=city.CityMaskingName%>"><%=city.CityName %></option>
                                 <%} %>
                                </select>
                            </div>
                            <div id="search-form-budget" class="form-control-box">
                                <div id="min-max-budget-box" class="form-selection-box">
                                    <span id="budget-default-label">Select budget</span>
                                    <span id="min-amount"></span>
                                    <span id="max-amount"></span>
                                    <span id="upDownArrow" class="fa fa-angle-down position-abt pos-top18 pos-right20"></span>
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
                            <div class="clear"></div>
                            <a data-bind="attr: { href: redirectUrl }"  id="searchCityBudget" class="btn btn-orange btn-lg search-bikes-btn margin-bottom20" >Search</a>
                            <div>
                                <a href="javascript:void(0)" id="profile-id-popup-target" class="font14 text-underline" rel="nofollow">Search by Profile ID</a>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section>
            <div class="container section-container">
                <div class="grid-12">
                    <h2 class="section-header">Best way to sell your bike</h2>
                    <div class="content-box-shadow text-center padding-top25 padding-bottom25 padding-right20 padding-left20 font14">
                        <div class="margin-bottom20">
                            <div class="grid-3">
                                <span class="used-sprite free-cost"></span>
                                <p class="text-bold margin-bottom5">Free of cost</p>
                                <p>You can upload your bike ad absolutely free</p>
                            </div>
                            <div class="grid-3">
                                <span class="used-sprite buyer"></span>
                                <p class="text-bold margin-bottom5">Genuine buyers </p>
                                <p>We let only verified buyers to get in touch with you</p>
                            </div>
                            <div class="grid-3">
                                <span class="used-sprite listing-time"></span>
                                <p class="text-bold margin-bottom5">Unlimited listing duration</p>
                                <p>Your bike listing will stay active until it is sold</p>
                            </div>
                            <div class="grid-3">
                                <span class="used-sprite contact-buyer"></span>
                                <p class="text-bold margin-bottom5">Get buyer details</p>
                                <p>We will send you the details of buyers thorugh SMS and mail</p>
                            </div>
                            <div class="clear"></div>
                        </div>
                        <a href="/used/sell/" title="Sell your bike" id="sell-btn" class="btn btn-teal">Sell</a>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
     
           <% if( viewModel.TopMakeList!= null && viewModel.OtherMakeList!= null){  %>
        <section>
            <div class="container section-container">
                <div class="grid-12">
                    <h2 class="section-header">Search used bikes by brands</h2>
                    <div class="content-box-shadow padding-top20">
                        <div class="brand-type-container">
                            <ul class="text-center">
                                  <% foreach(var bike in viewModel.TopMakeList){ %>  
                                <li>
                                    <a href="/used/<%=bike.MaskingName %>-bikes-in-india/" title="<%=bike.MakeName %> used bikes in India">
                                        <span class="brand-type">
                                            <span class="brandlogosprite brand-<%=bike.MakeId %>"></span>
                                        </span>
                                        <span class="brand-type-title"><%=bike.MakeName %></span>
                                    </a>
                                </li>                                    
                                  <% } %>
                            </ul>
                            <div class="brand-bottom-border margin-right20 margin-left20 border-solid-top hide"></div>
                            <ul class="brand-style-moreBtn padding-top25 brandTypeMore hide margin-left5">
                                <% foreach(var bike in viewModel.OtherMakeList){ %> 
                                <li>
                                     <a href="/used/<%=bike.MaskingName %>-bikes-in-india/" title="<%=bike.MakeName %> used bikes in India">
                                        <span class="brand-type">
                                            <span class="brandlogosprite brand-<%=bike.MakeId %>"></span>
                                        </span>
                                        <span class="brand-type-title"><%=bike.MakeName %></span>
                                    </a>
                                </li>                                    
                                <%} %>
                            </ul>
                        </div>
                        <div class="view-brandType text-center padding-bottom25">
                            <a href="javascript:void(0)" id="view-brandType" class="view-more-btn font16" rel="nofollow">View <span>more</span> brands</a>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
               <%} %>
       
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
                                    <a href="/used/bikes-in-<%=objCity.CityMaskingName %>/" title="Used bikes in <%=objCity.CityName %>">
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
                    <a href="/used/browse-bikes-by-cities/" title="Browse used bike by cities" class="btn btn-inv-teal inv-teal-sm margin-top10">View all cities<span class="bwmsprite teal-next"></span></a>
                </div>
            </div>
        </section>
        <% } %>
                     

        <section>
               <BW:RecentUsedBikes ID="ctrlRecentUsedBikes" runat="server" />
        </section>

        <!-- profile-id -->
        <div id="profile-id-popup" class="bw-popup text-center size-small">
            <div class="bwsprite cross-lg-lgt-grey close-btn position-abt pos-top10 pos-right10 cur-pointer"></div>
            <div class="icon-outer-container rounded-corner50">
                <div class="icon-inner-container rounded-corner50">
                    <span class="used-sprite profile-id-icon margin-top25"></span>
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

        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/src/frameworks.js?<%=staticFileVersion %>"></script>

        <!-- #include file="/includes/footerBW.aspx" -->
        <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%= staticUrl != string.Empty ? "http://st2.aeplcdn.com" + staticUrl : string.Empty %>/src/common.min.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" src="<%= staticUrl != string.Empty ? "http://st2.aeplcdn.com" + staticUrl : string.Empty %>/src/used-landing.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" >  
        var CustomerId="<%=currentUser%>";
            </script>

        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />
        <!--[if lt IE 9]>
            <script src="/src/html5.js"></script>
        <![endif]-->
    </form>
</body>
</html>
