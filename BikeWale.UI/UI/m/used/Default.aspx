<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Used.Default" %>
<%@ Register Src="~/UI/m/controls/usedBikeModel.ascx" TagName="usedBikeModel" TagPrefix="BW" %>
<%@ Register Src="~/UI/m/controls/usedBikeInCities.ascx" TagName="usedBikeInCities" TagPrefix="BW" %>
<!DOCTYPE html>
<html>
<head>
<%
    title = "Used Bikes in India | Buy and Sell Used Bikes- BikeWale";
    description = "BikeWale is the India's largest portal for selling and buying used bikes with more than 10000+ listings. Buy and Sell your second-hand bikes online for FREE!";
    keywords="Used bikes, used bike, used bikes for sale, second hand bikes, buy used bike";        
    canonical="https://www.bikewale.com/used/";
%>
    <!-- #include file="/UI/includes/headscript_mobile_min.aspx" -->
	<link href="<%= staticUrl  %>/UI/m/css/used/landing.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
	<link href="<%= staticUrl  %>/UI/css/components/m-brand-logo.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        <!-- #include file="\UI\includes\gacode_mobile.aspx" -->
    </script>
</head>
<body class="page-type-landing">
    <form id="form1" runat="server">
        <% if(viewModel!= null){ %>
        <!-- #include file="/UI/includes/headBW_Mobile.aspx" -->

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
                            <a data-bind="click: redirectUrl" id="searchCityBudget" class="btn btn-orange text-bold" rel="nofollow">Search</a>
                        </div>
                        <a href="javascript:void(0)" id="profile-id-popup-target" class="font14 text-underline" rel="nofollow">Search by Profile ID</a>
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
                    <a href="/m/used/sell/" title="Sell Your bike" class="btn btn-teal margin-top5">Sell now</a>
                </div>
            </div>
        </section>
        <% if( viewModel.TopMakeList!= null ){  %>
        <section>
            <div class="container text-center section-container collapsible-brand-content">
                <h2 class="font18 section-heading">Search used bikes by brands</h2>
                <div id="brand-type-container" class="bg-white box-shadow brand-type-container content-inner-block-20">
                    <ul id="main-brand-list">
                        <% foreach(var bike in viewModel.TopMakeList){ %>    
                        <li>
                            <a href="/m<%= bike.Link%>" title="Used <%=bike.MakeName %> bikes">
                                <span class="brand-type">
                                    <span class="brandlogosprite brand-<%=bike.MakeId %>"></span>
                                </span>
                                <span class="brand-type-title"><%=bike.MakeName %></span>
                            </a>
                        </li>
                        <% } %>
                    </ul>
                    
                    <% if (viewModel.OtherMakeList!= null){%>
                    <ul id="more-brand-nav" class="brand-style-moreBtn brandTypeMore border-top1 padding-top25 text-center">
                        <% foreach(var bike in viewModel.OtherMakeList){ %> 
                        <li>
                            <a href="/m<%= bike.Link%>" title="Used <%=bike.MakeName %> bikes">
                                <span class="brand-type">
                                    <span class="brandlogosprite brand-<%=bike.MakeId %>"></span>
                                </span>
                                <span class="brand-type-title"><%=bike.MakeName %></span>
                            </a>
                        </li>
                        <% } %>
                    </ul>
                  
                    <div class="view-all-btn-container">
                        <a href="javascript:void(0)" class="view-brandType view-more-btn btn view-all-target-btn rotate-arrow" rel="nofollow"><span class="btn-label">View more brands</span><span class="bwmsprite teal-right"></span></a>
                    </div>
                      <%} %>
                </div>
            </div>
        </section>
        <% } %>
       <%if (ctrlusedBikeInCities.objCitiesWithCount != null && ctrlusedBikeInCities.objCitiesWithCount.Count()>0){ %>
          
                 <section>
            <div class="container text-center section-container">
                <h2 class="font18 section-heading">Search used bikes by cities</h2>
                
                    <BW:usedBikeInCities runat="server" ID="ctrlusedBikeInCities" />  
              
                </div>
                     </section>
              
                    <%} %>
                         <% if (ctrlusedBikeModel.FetchCount>0)
                       { %>
        <div class="container text-center section-container collapsible-brand-content">            
        <h2 class="font18 section-heading">Popular used bikes</h2>
            <div class="padding-top20 content-box-shadow">
                <BW:usedBikeModel runat="server" ID="ctrlusedBikeModel" />
            </div>
        </div>
                       
                    <% } %> 
        
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
        <script type="text/javascript" src="<%= staticUrl  %>/UI/m/src/frameworks.js?<%= staticFileVersion %>"></script>
        <!-- #include file="/UI/includes/footerBW_Mobile.aspx" -->
        <link href="<%= staticUrl  %>/UI/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <!-- #include file="/UI/includes/footerscript_mobile.aspx" -->
        <script type="text/javascript" src="<%= staticUrl  %>/UI/m/src/used/landing.js?<%= staticFileVersion %>">
             var gaObj = { 'id': '<%= (int)Bikewale.Entities.Pages.GAPages.Used_Bike_Landing%>', 'name': '<%= Bikewale.Entities.Pages.GAPages.Used_Bike_Landing%>' }
        </script>
        <!-- #include file="/UI/includes/fontBW_Mobile.aspx" -->
        <% } %>
    </form>
</body>
</html>
