﻿<%@ Page Language="C#" Inherits="Bikewale.New.BrowseNewBikeDealerDetails_v2" AutoEventWireup="false" EnableViewState="false" %>

<!DOCTYPE html>
<html>
<head>
    <%      
        keywords = String.Format("{0} dealers city, {0} showrooms {1}, {1} bike dealers, {0} dealers, {1} bike showrooms, bike dealers, bike showrooms, dealerships", makeName, cityName);
        description = String.Format("{0} bike dealers/showrooms in {1}. Find {0} bike dealer information for more than 200 cities. Dealer information includes full address, phone numbers, email, pin code etc", makeName, cityName);
        title = String.Format("{0} Dealers in {1} city | {0} New bike Showrooms in {1} - BikeWale", makeName, cityName);
        canonical = String.Format("http://www.bikewale.com/{0}-bikes/dealers-in-{1}/", makeMaskingName, cityMaskingName);
        alternate = String.Format("http://www.bikewale.com/m/{0}-bikes/dealers-in-{1}/", makeMaskingName, cityMaskingName);
        AdId = "1395986297721";
        AdPath = "/1017752/BikeWale_New_";
        isAd970x90Shown = false;
        isAd300x250Shown = false;
        isAd300x250BTFShown = false;
        isAd970x90BottomShown = false;
        isHeaderFix = false;
    %>
    <!-- #include file="/includes/headscript_desktop_min.aspx" -->
    <style type="text/css">
        @charset "utf-8";.padding-14-20{padding:14px 20px}.padding-18-20{padding:18px 20px}#listing-left-column.grid-4{padding-right:20px;padding-left:20px;width:32.333333%;box-shadow:0 0 8px #ddd;z-index:1}#listing-right-column.grid-8{width:67.666667%}#dealersList li{padding-bottom:20px;border-top:1px solid #eee}#dealersList li:first-child{border-top:0}#dealersList h3{padding-top:18px}.dealer-card-target .dealer-name{display:block;text-align:left;text-overflow:ellipsis;white-space:nowrap;overflow:hidden}.dealer-card-target:hover{text-decoration:none}.featured-tag{width:74px;display:block;text-align:center;line-height:20px;background:#3799a7;z-index:1;font-weight:400;font-size:12px;color:#fff;border-radius:2px;position:relative;top:-4px}.vertical-top{display:inline-block;vertical-align:top}.dealership-card-details{width:92%}.dealer-map-wrapper{width:100%;height:530px;display:block;position:relative}#used-bikes-content .grid-6{display:inline-block;vertical-align:top;width:49%;float:none}.dealership-loc-icon{width:9px;height:12px;background-position:-52px -469px;position:relative;top:4px}.phone-black-icon{width:10px;height:10px;background-position:-73px -444px;position:relative;top:5px}.star-white{width:8px;height:8px;background-position:-222px -107px;margin-right:4px}.blue-right-arrow-icon{width:6px;height:10px;background-position:-74px -469px;position:relative;top:1px;left:7px}.btn.btn-size-2{padding:9px 20px}
    </style>
    <script src="http://maps.googleapis.com/maps/api/js?key=<%= Bikewale.Utility.BWConfiguration.Instance.GoogleMapApiKey %>&libraries=places"></script>
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_desktop.aspx" -->
    </script>
    
</head>
<body class="bg-light-grey">
    <form runat="server">
        <!-- #include file="/includes/headBW.aspx" -->
        <section>
            <div class="container padding-top10">
                <div class="grid-12">
                    <div class="breadcrumb margin-bottom15">
                        <!-- breadcrumb code starts here -->
                        <ul>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <a itemprop="url" href="/"><span itemprop="title">Home</span></a>
                            </li>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="bwsprite fa-angle-right margin-right10"></span><a itemprop="url" href="/new/"><span itemprop="title">New Bikes</span></a>
                            </li>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="bwsprite fa-angle-right margin-right10"></span><a itemprop="url" href="/new/locate-dealers/"><span itemprop="title">New Bike Dealer</span></a>
                            </li>
                            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="bwsprite fa-angle-right margin-right10"></span><a itemprop="url" href="/new/<%=makeMaskingName %>-dealers/"><span itemprop="title"><%=makeName%> Bikes Dealers</span></a>
                            </li>
                            <li class="current"><span class="bwsprite fa-angle-right margin-right10"></span><%=makeName%> Bikes Dealers in Mumbai</li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section>
            <div class="container margin-bottom20">
                <div class="grid-12">
                    <div class="content-box-shadow">
                        <div class="content-box-shadow padding-14-20">
                            <h1><%=makeName%> dealers in <%=cityName%></h1>
                        </div>
                        <p class="font14 text-light-grey content-inner-block-20">
                            <%=makeName%> has 10 authorized dealers in <%=cityName%>. Apart from the authorized dealerships, 
                            <%=makeName%> bikes are also available at unauthorized showrooms and broker outlets. 
                            BikeWale recommends buying bikes only from authorized <%=makeName%> dealer outlets in <%=cityName%>. 
                            For information on test rides, price, offers, etc. you may get in touch with any of 
                            the below mentioned authorized <%=makeName%> dealers in <%=cityName%>.
                        </p>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        
        <section>
            <div class="container margin-bottom20">
                <div class="grid-12">
                    <div class="content-box-shadow">
                        <p class="font18 text-black text-bold bg-white padding-18-20">24 Bajaj dealers in <%=cityName%></p>
                        <div id="listing-left-column" class="grid-4">
                            <ul id="dealersList">
                                <asp:Repeater ID="rptDealers" runat="server">
                                    <ItemTemplate>
                                        <li data-item-type="<%# (DataBinder.Eval(Container.DataItem,"DealerType")) %>" data-item-id="<%# DataBinder.Eval(Container.DataItem,"DealerId") %>" data-item-inquired="false" data-item-number="<%# DataBinder.Eval(Container.DataItem,"MaskingNumber") %>" data-lat="<%# DataBinder.Eval(Container.DataItem,"objArea.Latitude") %>" data-log="<%# DataBinder.Eval(Container.DataItem,"objArea.Longitude") %>" data-address="<%# DataBinder.Eval(Container.DataItem,"Address") %>" data-campid="<%# DataBinder.Eval(Container.DataItem,"CampaignId") %>">
                                            <a href="" title="<%# DataBinder.Eval(Container.DataItem,"Name") %>" class="dealer-card-target font14">
                                                <h3 class="margin-bottom5">
                                                    <div class="<%# ((DataBinder.Eval(Container.DataItem,"DealerType").ToString() == "3") || (DataBinder.Eval(Container.DataItem,"DealerType").ToString() == "2"))? "" : "hide" %>">
                                                        <span class="featured-tag margin-bottom5"><span class="bwsprite star-white"></span>Featured</span>
                                                    </div>
                                                    <p class="dealer-name text-black text-bold"><%# DataBinder.Eval(Container.DataItem,"Name") %></p>
                                                </h3>
                                                <p class="<%# (String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"Address").ToString()))?"hide":"text-light-grey margin-bottom5" %>">
                                                    <span class="bwsprite dealership-loc-icon vertical-top margin-right5"></span>
                                                    <span class="vertical-top dealership-card-details"><%# DataBinder.Eval(Container.DataItem,"Address") %></span>
                                                </p>
                                                <p class="<%# (String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"MaskingNumber").ToString()))?"hide":"" %>">
                                                    <span class="bwsprite phone-black-icon vertical-top margin-right5"></span>
                                                    <span class="vertical-top text-bold text-default dealership-card-details"><%# DataBinder.Eval(Container.DataItem,"MaskingNumber") %></span>
                                                </p>
                                            </a>
                                            <div class="<%# ((DataBinder.Eval(Container.DataItem,"DealerType").ToString() == "3") || (DataBinder.Eval(Container.DataItem,"DealerType").ToString() == "2"))? "margin-top20" : "hide" %>">
                                                <a data-item-id="<%# DataBinder.Eval(Container.DataItem,"DealerId") %>" href="Javascript:void(0)" leadSourceId="14" 
                                                    pqSourceId="<%= (int) Bikewale.Entities.PriceQuote.PQSourceEnum.Desktop_DealerLocator_GetOfferButton %>" class="btn btn-white btn-full-width font14 get-assistance-btn">Get offers from dealer</a>
                                            </div>
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                        </div>
                        <div id="listing-right-column" class="grid-8 alpha omega">
                            <div class="dealer-map-wrapper">
                                <div id="dealerMapWrapper" style="width: 661px; height: 530px;">
                                    <div id="dealersMap" style="width: 661px; height: 530px;"></div>
                                </div>
                            </div>
                        </div>
                        <div class="clear"></div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
            <div id="listing-footer"></div>
        </section>

        <section>
            <div class="container margin-bottom10">
                <div class="grid-12">
                    <div class="content-box-shadow">
                        <h2 class="font18 padding-18-20">Popular Bajaj bikes in Mumbai</h2>
                        <div class="jcarousel-wrapper inner-content-carousel padding-bottom20">
                            <div class="jcarousel">
                                <ul>                
                                    <li>
                                        <a href="" title="Hero Splendor Pro Classic" class="jcarousel-card">
                                            <div class="model-jcarousel-image-preview">
                                                <span class="card-image-block">
                                                    <img class="lazy" data-original="http://imgd1.aeplcdn.com//310x174//bw/models/hero-splendor-pro-classic-standard-730.jpg?20151209182019" alt="Hero Splendor Pro Classic" src="" border="0">
                                                </span>
                                            </div>
                                            <div class="card-desc-block">
                                                <p class="bikeTitle">Hero Splendor Pro Classic</p>
                                                <p class="text-xt-light-grey margin-bottom10">200 cc, 45 kmpl, 24 bhp, 150 kgs</p>
                                                <p class="text-light-grey margin-bottom5">Ex-showroom, Mumbai</p>
                                                <span class="bwsprite inr-lg"></span>&nbsp;<span class="font18 text-default text-bold">1,22,000</span>
                                            </div>
                                        </a>
                                        <div class="margin-left20 margin-bottom20">
                                            <a href="javascript:void(0)" class="btn btn-white font14 btn-size-2" rel="nofollow">View price in Mumbai</a>
                                        </div>                                        
                                    </li>

                                    <li>
                                        <a href="" title="Hero Passion X Pro" class="jcarousel-card">
                                            <div class="model-jcarousel-image-preview">
                                                <span class="card-image-block">
                                                    <img class="lazy" data-original="http://imgd1.aeplcdn.com//310x174//bw/models/hero-passion-x-pro-drum-self-spoke-254.jpg?20151209181806" alt="Hero Splendor Pro Classic" src="" border="0">
                                                </span>
                                            </div>
                                            <div class="card-desc-block">
                                                <p class="bikeTitle">Hero Passion X Pro</p>
                                                <p class="text-xt-light-grey margin-bottom10">200 cc, 45 kmpl, 24 bhp, 150 kgs</p>
                                                <p class="text-light-grey margin-bottom5">Ex-showroom, Mumbai</p>
                                                <span class="bwsprite inr-lg"></span>&nbsp;<span class="font18 text-default text-bold">1,22,000</span>
                                            </div>
                                        </a>
                                        <div class="margin-left20 margin-bottom20">
                                            <a href="javascript:void(0)" class="btn btn-white font14 btn-size-2" rel="nofollow">View price in Mumbai</a>
                                        </div>
                                    </li>

                                    <li>
                                        <a href="" title="TVS Victor" class="jcarousel-card">
                                            <div class="model-jcarousel-image-preview">
                                                <span class="card-image-block">
                                                    <img class="lazy" data-original="http://imgd3.aeplcdn.com//310x174//bw/models/tvs-victor.jpg?20162001153247" alt="TVS Victor" src="" border="0">
                                                </span>
                                            </div>
                                            <div class="card-desc-block">
                                                <p class="bikeTitle">TVS Victor</p>
                                                <p class="text-xt-light-grey margin-bottom10">200 cc, 45 kmpl, 24 bhp, 150 kgs</p>
                                                <p class="text-light-grey margin-bottom5">Ex-showroom, Mumbai</p>
                                                <span class="bwsprite inr-lg"></span>&nbsp;<span class="font18 text-default text-bold">1,22,000</span>
                                            </div>
                                        </a>
                                        <div class="margin-left20 margin-bottom20">
                                            <a href="javascript:void(0)" class="btn btn-white font14 btn-size-2" rel="nofollow">View price in Mumbai</a>
                                        </div>
                                    </li>

                                    <li>
                                        <a href="" title="Hero Passion X Pro" class="jcarousel-card">
                                            <div class="model-jcarousel-image-preview">
                                                <span class="card-image-block">
                                                    <img class="lazy" data-original="http://imgd1.aeplcdn.com//310x174//bw/models/hero-passion-x-pro-drum-self-spoke-254.jpg?20151209181806" alt="Hero Passion X Pro" src="" border="0">
                                                </span>
                                            </div>
                                            <div class="card-desc-block">
                                                <p class="bikeTitle">Hero Passion X Pro</p>
                                                <p class="text-xt-light-grey margin-bottom10">200 cc, 45 kmpl, 24 bhp, 150 kgs</p>
                                                <p class="text-light-grey margin-bottom5">Ex-showroom, Mumbai</p>
                                                <span class="bwsprite inr-lg"></span>&nbsp;<span class="font18 text-default text-bold">1,22,000</span>
                                            </div>
                                        </a>
                                        <div class="margin-left20 margin-bottom20">
                                            <a href="javascript:void(0)" class="btn btn-white font14 btn-size-2" rel="nofollow">View price in Mumbai</a>
                                        </div>
                                    </li>

                                    <li>
                                        <a href="" title="Hero Splendor Pro Classic" class="jcarousel-card">
                                            <div class="model-jcarousel-image-preview">
                                                <span class="card-image-block">
                                                    <img class="lazy" data-original="http://imgd1.aeplcdn.com//310x174//bw/models/hero-splendor-pro-classic-standard-730.jpg?20151209182019" alt="Hero Splendor Pro Classic" src="" border="0">
                                                </span>
                                            </div>
                                            <div class="card-desc-block">
                                                <p class="bikeTitle">Hero Splendor Pro Classic</p>
                                                <p class="text-xt-light-grey margin-bottom10">200 cc, 45 kmpl, 24 bhp, 150 kgs</p>
                                                <p class="text-light-grey margin-bottom5">Ex-showroom, Mumbai</p>
                                                <span class="bwsprite inr-lg"></span>&nbsp;<span class="font18 text-default text-bold">1,22,000</span>
                                            </div>
                                        </a>
                                        <div class="margin-left20 margin-bottom20">
                                            <a href="javascript:void(0)" class="btn btn-white font14 btn-size-2" rel="nofollow">View price in Mumbai</a>
                                        </div>
                                    </li>
                                </ul>
                            </div>
                            <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev inactive" rel="nofollow"></a></span>
                            <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next" rel="nofollow"></a></span>
                        </div>

                        <div class="margin-left10 margin-right10 border-solid-bottom"></div>

                        <div id="used-bikes-content" class="grid-12 padding-top20 padding-bottom20">
                            <div class="grid-8 font14">
                                <h2 class="font18 margin-bottom15">Used Bajaj bikes in Mumbai</h2>
                                <div class="grid-6 alpha margin-bottom20">
                                    <a href="" title="2015, Honda Activa Standard">2015, Honda Activa Standard</a>
                                    <p class="margin-top10"><span class="bwsprite inr-sm-dark"></span>&nbsp;61,000</p>
                                </div>
                                <div class="grid-6 alpha margin-bottom20">
                                    <a href="" title="2011, Honda CBR250R Standard">2011, Honda CBR250R Standard</a>
                                    <p class="margin-top10"><span class="bwsprite inr-sm-dark"></span>&nbsp;1,25,000</p>
                                </div>
                                <div class="grid-6 alpha margin-bottom20">
                                    <a href="" title="2013, Honda CB Unicorn GP E">2013, Honda CB Unicorn GP E</a>
                                    <p class="margin-top10"><span class="bwsprite inr-sm-dark"></span>&nbsp;48,000</p>
                                </div>
                                <div class="grid-6 alpha margin-bottom20">
                                    <a href="" title="2012, Honda Activa Deluxe">2012, Honda Activa Deluxe</a>
                                    <p class="margin-top10"><span class="bwsprite inr-sm-dark"></span>&nbsp;43,000</p>
                                </div>

                                <a href="">View all used bikes in Mumbai<span class="bwsprite blue-right-arrow-icon"></span></a>
                            </div>
                            <div class="grid-4 alpha">
                                <div class="rightfloat">
                                    <!-- Ad -->
                                </div>
                            </div>
                            <div class="clear"></div>
                        </div>
                        <div class="clear"></div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section>
            <div class="container margin-bottom30">
                <div class="grid-12 font12">
                    <span class="font14"><strong>Disclaimer</strong>:</span> The above mentioned information about <%=makeName%> dealership showrooms in <%=cityName%> is furnished to the best of our knowledge.                         All <%=makeName%> bike models and colour options may not be available at each of the <%=makeName%> dealers. 
                        We recommend that you call and check with your nearest <%=makeName%> dealer before scheduling a showroom visit.
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/src/frameworks.js?<%=staticFileVersion %>"></script>
        <!-- #include file="/includes/footerBW.aspx" -->        
        <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%= staticUrl != string.Empty ? "http://st2.aeplcdn.com" + staticUrl : string.Empty %>/src/common.min.js?<%= staticFileVersion %>"></script>        
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/dealer/listing.js?<%= staticFileVersion %>"></script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />
        <!--[if lt IE 9]>
            <script src="/src/html5.js"></script>
        <![endif]-->
    </form>
</body>
</html>
