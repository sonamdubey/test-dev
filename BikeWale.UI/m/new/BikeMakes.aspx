<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.BikeMakes" EnableViewState="false" %> 
<%@ Register Src="~/m/controls/MUpcomingBikes.ascx" TagName="MUpcomingBikes" TagPrefix="BW" %>
<%@ Register Src="/m/controls/NewsWidget.ascx" TagName="News" TagPrefix="BW" %>
<%@ Register Src="/m/controls/ExpertReviewsWidget.ascx" TagName="ExpertReviews" TagPrefix="BW" %>
<%@ Register Src="/m/controls/VideosWidget.ascx" TagName="Videos" TagPrefix="BW" %>

<!doctype html>
<html>
<head>
    <%
        title = _make.MakeName + " Price in India, Review, Mileage & Photos - Bikewale";
        description = _make.MakeName + " Price in India - Rs." + Bikewale.Utility.Format.FormatPrice(_minModelPrice.ToString()) +
           " to  Rs." + Bikewale.Utility.Format.FormatPrice(_maxModelPrice.ToString()) + ". Check out " + _make.MakeName +
           " on road price, reviews, mileage, versions, news & photos at Bikewale.";
        canonical = "http://www.bikewale.com/" + _make.MaskingName + "-bikes/";
        AdPath = "/1017752/Bikewale_Mobile_Make";
        AdId = "1444028878952";
        Ad_320x50 = true;
        Ad_Bot_320x50 = true;        
        TargetedMakes = _make.MakeName;
        keywords = string.Format("{0}, {0} Bikes , {0} Bikes prices, {0} Bikes reviews, new {0} Bikes", _make.MakeName);
    %>
    <!-- #include file="/includes/headscript_mobile.aspx" -->
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-brand.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
</head>
<body class="bg-light-grey">
    <form runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->

        <section>
            <div id="bikeByMakesListing" class="container bg-white margin-bottom20">
                <div>
                    <div class="hide" id="sort-by-div">
                    	<div  class="filter-sort-div font14 bg-white">
                            <div sc="1" so="">
                                <a data-title="sort" class="price-sort position-rel text-bold">
                                	Price <span class="hide" so="0" class="sort-text "> : Low</span>
                                </a>
                            </div>
                            <div sc="" class="border-solid-left">
                                <a data-title="sort" class="position-rel">
                                	Popularity 
                                </a>
                            </div>
                            <div sc="2" class="border-solid-left">
                                <a data-title="sort" class="position-rel">
                                	Mileage 
                                </a>
                            </div>
                        </div>
                    </div>
                    <!--  class="grid-12"-->
                    <div class="bg-white box-shadow content-inner-block-1520">
                        <h2 class="text-x-black"><%= _make.MakeName %> Bikes</h2>
                    </div>
                    <div class="search-bike-container position-rel pos-top3 box-shadow">
                        <div class="search-bike-item">
                            <div id="listitems" class="listitems">
                                <asp:Repeater ID="rptMostPopularBikes" runat="server">
                                    <ItemTemplate>
                                        <div class="front" ind="<%#DataBinder.Eval(Container, "ItemIndex", "")%>" prc="<%# DataBinder.Eval(Container.DataItem, "VersionPrice") %>" mlg="<%# DataBinder.Eval(Container.DataItem, "Specs.FuelEfficiencyOverall") %>"  pop="<%# DataBinder.Eval(Container.DataItem, "BikePopularityIndex") %>" >
                                            <div class="contentWrapper">
                                                <div class="imageWrapper">
                                                    <a class="modelurl" href='/m<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"objMake.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"objModel.MaskingName"))) %>'>
                                                        <img class="lazy" data-original="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImagePath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._310x174) %>" title="<%# DataBinder.Eval(Container.DataItem,"objMake.MakeName") + " " + DataBinder.Eval(Container.DataItem, "objModel.ModelName") %>" alt="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "objModel.ModelName")) %>" src="http://imgd3.aeplcdn.com/0x0/bw/static/sprites/m/circleloader.gif" width="310" height="174">
                                                    </a>
                                                </div>
                                                <div class="bikeDescWrapper">
                                                    <div class="bikeTitle margin-bottom10">
                                                        <h3><a class="modelurl" href='/m<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"objMake.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"objModel.MaskingName"))) %>' title="<%# DataBinder.Eval(Container.DataItem,"objMake.MakeName") + " " + DataBinder.Eval(Container.DataItem, "objModel.ModelName") %>"><%# DataBinder.Eval(Container.DataItem,"objMake.MakeName") + " " + DataBinder.Eval(Container.DataItem, "objModel.ModelName") %></a></h3>
                                                    </div>
                                                    <div class="font14 text-x-light margin-bottom10">
                                                        <%# Bikewale.Utility.FormatMinSpecs.GetMinSpecs(Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.Displacement")),Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.FuelEfficiencyOverall")),Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.MaxPower"))) %>
                                                    </div>
                                                    <div class="margin-bottom10 font14 text-light-grey">Ex-showroom, <%=ConfigurationManager.AppSettings["defaultName"].ToString() %></div>
                                                    <div class="margin-bottom5">
                                                        <span class="bwmsprite inr-sm-icon" style="<%# (Convert.ToString(DataBinder.Eval(Container.DataItem, "VersionPrice"))=="0")?"display:none;": "display:inline-block;"%>"></span>
                                                        <span class="text-bold font18"><%# ShowEstimatedPrice(DataBinder.Eval(Container.DataItem, "VersionPrice")) %></span>
                                                    </div>
                                                    <div class="padding-top5 clear">
                                                        <div class="grid-12 alpha <%# Convert.ToString(DataBinder.Eval(Container.DataItem,"ReviewCount")) == "0" ? "" : "hide" %>">
                                                            <div class="padding-left5 padding-right5 ">
                                                                <div>
                                                                    <span class="font16 text-light-grey">Not rated yet  </span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="leftfloat">
                                                            <div class="padding-left5 padding-right5 <%# Convert.ToString(DataBinder.Eval(Container.DataItem,"ReviewCount")) != "0" ? "" : "hide" %>">
                                                                <div>
                                                                    <span class="margin-bottom10 ">
                                                                        <%# Bikewale.Utility.ReviewsRating.GetRateImage(Convert.ToDouble(DataBinder.Eval(Container.DataItem,"ModelRating"))) %>
                                                                    </span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="leftfloat border-left1">
                                                            <div class="padding-left5 padding-right5 <%# Convert.ToString(DataBinder.Eval(Container.DataItem,"ReviewCount")) != "0" ? "" : "hide" %>">
                                                                <span class="font16 text-light-grey"><a href="/m/<%#DataBinder.Eval(Container.DataItem,"objMake.MaskingName").ToString()%>-bikes/<%#DataBinder.Eval(Container.DataItem,"objModel.MaskingName").ToString() %>/user-reviews/"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "ReviewCount")) %> Reviews</a></span>
                                                            </div>
                                                        </div>
                                                        <div class="clear"></div>
                                                        <a href="javascript:void(0)" makename="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "objMake.MakeName")) %>" modelname="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "objModel.ModelName")) %>" pagecatid="1" pqSourceId="<%= (int)Bikewale.Entities.PriceQuote.PQSourceEnum.Mobile_MakePage %>" modelid="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "objModel.ModelId")) %>" class="btn btn-sm btn-white margin-top10 fillPopupData" rel="nofollow">Check on-road price</a>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="border-top1 margin-left20 margin-right20 padding-top20 clear"></div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>

                            <div id="listItemsFooter"></div>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section id="bikeByMakesUpcoming" class="bg-white margin-bottom20">
            <div class="container box-shadow padding-top20 padding-bottom20">
                <h2 class="text-x-black font18 margin-bottom25 padding-right20 padding-left20">Upcoming <%= _make.MakeName %> bikes</h2>
                <div class="swiper-container">
                    <div class="swiper-wrapper upcoming-carousel-content">
                        <div class="swiper-slide bike-carousel-swiper">
                            <div class="bike-swiper-image-wrapper">
                                <a href="">
                                    <img class="swiper-lazy" alt="" title="" data-src="http://imgd1.aeplcdn.com//310x174//bw/models/royal-enfield-continental-gt-(cafe-racer)-standard-326.jpg?20151209202251" />
                                    <span class="swiper-lazy-preloader"></span>
                                </a>
                            </div>
                            <div class="bike-swiper-details-wrapper">
                                <a href="" class="block font14 text-bold text-black margin-bottom5">Honda Unicorn</a>
                                <p class="text-xx-light margin-bottom5">Expected launch</p>
                                <p class="margin-bottom10">June 2016</p>
                                <p class="text-xx-light margin-bottom5">Expected price</p>
                                <div class="font16">
                                    <span class="bwmsprite inr-xsm-icon"></span>&nbsp;<span class="text-bold">92,000</span>
                                </div>
                            </div>
                        </div>
                        <div class="swiper-slide bike-carousel-swiper">
                            <div class="bike-swiper-image-wrapper">
                                <a href="">
                                    <img class="swiper-lazy" alt="" title="" data-src="http://imgd1.aeplcdn.com//310x174//bw/models/royal-enfield-continental-gt-(cafe-racer)-standard-326.jpg?20151209202251" />
                                    <span class="swiper-lazy-preloader"></span>
                                </a>
                            </div>
                            <div class="bike-swiper-details-wrapper">
                                <a href="" class="block font14 text-bold text-black margin-bottom5">Honda Unicorn</a>
                                <p class="text-xx-light margin-bottom5">Expected launch</p>
                                <p class="margin-bottom10">June 2016</p>
                                <p class="text-xx-light margin-bottom5">Expected price</p>
                                <div class="font16">
                                    <span class="bwmsprite inr-xsm-icon"></span>&nbsp;<span class="text-bold">92,000</span>
                                </div>
                            </div>
                        </div>
                        <div class="swiper-slide bike-carousel-swiper">
                            <div class="bike-swiper-image-wrapper">
                                <a href="">
                                    <img class="swiper-lazy" alt="" title="" data-src="http://imgd1.aeplcdn.com//310x174//bw/models/royal-enfield-continental-gt-(cafe-racer)-standard-326.jpg?20151209202251" />
                                    <span class="swiper-lazy-preloader"></span>
                                </a>
                            </div>
                            <div class="bike-swiper-details-wrapper">
                                <a href="" class="block font14 text-bold text-black margin-bottom5">Honda Unicorn</a>
                                <p class="text-xx-light margin-bottom5">Expected launch</p>
                                <p class="margin-bottom10">June 2016</p>
                                <p class="text-xx-light margin-bottom5">Expected price</p>
                                <div class="font16">
                                    <span class="bwmsprite inr-xsm-icon"></span>&nbsp;<span class="text-bold">92,000</span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>

        <section>
            <div id="makeTabsContentWrapper" class="container bg-white clearfix box-shadow margin-bottom20">
                <div id="makeOverallTabsWrapper">
                    <div id="overallSpecsTab" class="overall-specs-tabs-container">
                        <ul class="overall-specs-tabs-wrapper">
                            <li class="active" data-tabs="#makeAboutContent"><h3>About</h3></li>
                            <li data-tabs="#makeNewsContent"><h3>News</h3></li>
                            <li data-tabs="#makeReviewsContent"><h3>Reviews</h3></li>
                            <li data-tabs="#makeDealersContent"><h3>Dealers</h3></li>
                            <li data-tabs="#makeUsedBikeContent"><h3>Used</h3></li>
                        </ul>
                    </div>
                </div>

                <div id="makeAboutContent" class="bw-model-tabs-data padding-top15">
                    <div class="margin-bottom20 padding-right20 padding-left20">
                        <h2>About <%= _make.MakeName %></h2>
                        <p class="font14 text-light-grey line-height17 margin-bottom15">
                            <span class="model-preview-main-content">After number of spy pictures doing the round of the internet, Bajaj Motorcycles has finally 
                            launched its first fully-faired motorcycle, the Pulsar RS 200 for the Indian market. Previously 
                            touted to be called as the Pulsar SS200, this bike has been the most anticipated...
                            </span>
                            <span class="model-preview-more-content">After number of spy pictures doing the round of the internet, Bajaj Motorcycles has finally 
                            launched its first fully-faired motorcycle, the Pulsar RS 200 for the Indian market. Previously 
                            touted to be called as the Pulsar SS200, this bike has been the most anticipated launch from 
                            the company.<br /><br />
                            Marketed as the fastest Pulsar yet, the Pulsar RS200 designed to be a compact sportsbike
                            and features clip-on handlebars. Unlike other fully-faired motorcycle like the Yamaha YZF-R15,
                            the RS200 doesn’t have as aggressive riding stance as of a super sport motorcycle.
                            </span>
                            <a href="javascript:void(0)" class="read-more-model-preview" rel="nofollow">Read more</a>
                        </p>
                    </div>
                    <div class="margin-bottom20 text-center">
                        <div style="width:300px; height:250px; background:#eee; margin:0 auto;"></div>
                    </div>
                </div>

                <div id="makeNewsContent" class="bw-model-tabs-data padding-right20 padding-left20">
                    <h2><%= _make.MakeName %> News</h2>
                    <div class="margin-bottom15">
                        <div class="news-image-wrapper">
                            <a href="">
                                <img src="http://imgd1.aeplcdn.com//370x208//bw/ec/23331/TVS-Victor-Action-72508.jpg?wm=2" />
                            </a>
                        </div>
                        <div class="news-heading-wrapper">
                            <h4>
                                <a href="" class="font12 text-black">
                                    PowerDrift Specials : Rajini's Academy of Competitive Racing...
                                </a>
                            </h4>
                            <p class="font10 text-truncate text-light-grey">April 15, 2016, by Sagar Bhanushali</p>
                        </div>
                    </div>
                    <ul id="makeNewsList">
                        <li>
                            <h4 class="margin-bottom5 red-bullet-point"><a href="" class="font12 text-black">Bajaj Avenger Cruise 220 proves popular with families</a></h4>
                            <p class="margin-left15 font10 text-truncate text-light-grey">March 16, 2016, by Firoze Irani</p>
                        </li>
                        <li>
                            <h4 class="margin-bottom5 red-bullet-point"><a href="" class="font12 text-black">Guy Martin's competition CBR1000RR auctioned for only Rs 14.3 lakh</a></h4>
                            <p class="margin-left15 font10 text-truncate text-light-grey">March 16, 2016, by Firoze Irani</p>
                        </li>
                    </ul>
                    <div>
                        <a href="javascript:void(0)" class="font14">Read all news<span class="bwmsprite blue-right-arrow-icon"></span></a>
                    </div>
                </div>

                <div class="margin-top20 margin-right20 margin-left20 border-divider"></div>

                <div id="makeReviewsContent" class="bw-model-tabs-data padding-top15 padding-right20 padding-left20 font14">
                    <h2><%= _make.MakeName %> Reviews</h2>
                    
                </div>

                <div class="margin-top20 margin-right20 margin-left20 border-divider"></div>

                <div id="makeDealersContent" class="bw-model-tabs-data padding-top15 font14">
                    <h2 class="padding-right20 padding-left20"><%= _make.MakeName %> dealers in India</h2>
                    <div class="swiper-container margin-bottom15"><!-- dealers by city -->
                        <div class="swiper-wrapper">
                            <div class="swiper-slide bike-carousel-swiper dealer-by-city">
                                <h4 class="margin-bottom5 text-truncate"><a href="" class="text-default">BikeWale Dealer 1</a></h4>
                                <p class="margin-bottom5 text-light-grey"><span class="bwmsprite dealership-loc-icon vertical-top margin-right5"></span><span class="vertical-top dealership-address">Pedestrian Overpass, Lokmanya Tilak</span></p>
                                <a href="tel:555555555" class="block margin-bottom5 text-default text-truncate"><span class="bwmsprite tel-sm-grey-icon"></span>555555555</a>
                                <a href="mailto:contact@bikewale.com" class="text-light-grey block margin-bottom20"><span class="bwmsprite mail-grey-icon vertical-top"></span><span class="text-truncate vertical-top dealership-email">contact@bikewale.com</span></a>
                                <a href="" class="btn btn-white btn-full-width font14">Get offers</a>
                            </div>
                            <div class="swiper-slide bike-carousel-swiper dealer-by-city">
                                <h4 class="margin-bottom5 text-truncate"><a href="" class="text-default">BikeWale Dealer 2</a></h4>
                                <p class="margin-bottom5 text-light-grey"><span class="bwmsprite dealership-loc-icon vertical-top margin-right5"></span><span class="vertical-top dealership-address">Pedestrian Overpass, Lokmanya Tilak</span></p>
                                <a href="tel:555555555" class="block margin-bottom5 text-default text-truncate"><span class="bwmsprite tel-sm-grey-icon"></span>555555555</a>
                                <a href="mailto:contact@bikewale.com" class="text-light-grey block margin-bottom20"><span class="bwmsprite mail-grey-icon vertical-top"></span><span class="text-truncate vertical-top dealership-email">contact@bikewale.com</span></a>
                                <a href="" class="btn btn-white btn-full-width font14">Get offers</a>
                            </div>
                            <div class="swiper-slide bike-carousel-swiper dealer-by-city">
                                <h4 class="margin-bottom5 text-truncate"><a href="" class="text-default">BikeWale Dealer 3</a></h4>
                                <p class="margin-bottom5 text-light-grey"><span class="bwmsprite dealership-loc-icon vertical-top margin-right5"></span><span class="vertical-top dealership-address">Pedestrian Overpass, Lokmanya Tilak</span></p>
                                <a href="tel:555555555" class="block margin-bottom5 text-default text-truncate"><span class="bwmsprite tel-sm-grey-icon"></span>555555555</a>
                                <a href="mailto:contact@bikewale.com" class="text-light-grey block margin-bottom20"><span class="bwmsprite mail-grey-icon vertical-top"></span><span class="text-truncate vertical-top dealership-email">contact@bikewale.com</span></a>
                                <a href="" class="btn btn-white btn-full-width font14">Get offers</a>
                            </div>
                        </div>
                    </div>
                    <%--<div class="swiper-container margin-bottom15"><!-- dealers when no city selected -->
                        <div class="swiper-wrapper">
                            <div class="swiper-slide bike-carousel-swiper dealer-no-city">
                                <a href="">
                                    <span></span>
                                    <h4 class="font14 text-bold text-default margin-bottom10"><%= _make.MakeName %> dealers in Mumbai</h4>
                                    <p class="font14 text-black">24 showrooms</p>
                                </a>
                            </div>
                            <div class="swiper-slide bike-carousel-swiper dealer-no-city">
                                <a href="">
                                    <span></span>
                                    <h4 class="font14 text-bold text-default margin-bottom10"><%= _make.MakeName %> dealers in Pune</h4>
                                    <p class="font14 text-black">30 showrooms</p>
                                </a>
                            </div>
                            <div class="swiper-slide bike-carousel-swiper dealer-no-city">
                                <a href="">
                                    <span></span>
                                    <h4 class="font14 text-bold text-default margin-bottom10"><%= _make.MakeName %> dealers in Mumbai</h4>
                                    <p class="font14 text-black">24 showrooms</p>
                                </a>
                            </div>
                        </div>
                    </div>--%>
                    <div class="padding-right20 padding-left20">
                        <a href="javascript:void(0)" class="font14">View all dealers<span class="bwmsprite blue-right-arrow-icon"></span></a>
                    </div>
                </div>

                <div class="margin-top20 margin-right20 margin-left20 border-divider"></div>

                <div id="makeUsedBikeContent" class="bw-model-tabs-data padding-top15 padding-right20 padding-bottom20 padding-left20 font14">
                    <h2><%= _make.MakeName %> used bikes</h2>
                    <ul>
                        <li>
                            <a href="" class="block margin-bottom10">2009, Bajaj Pulsar 220 Fi Standard</a>
                            <p class="text-black">
                                <span class="bwmsprite inr-xxsm-icon"></span><span>1,67,673 in Mumbai</span>
                            </p>
                        </li>
                        <li>
                            <a href="" class="block margin-bottom10">2009, Bajaj Pulsar 220 Fi Standard</a>
                            <p class="text-black">
                                <span class="bwmsprite inr-xxsm-icon"></span><span>1,67,673 in Mumbai</span>
                            </p>
                        </li>
                        <li>
                            <a href="" class="block margin-bottom10">2009, Bajaj Pulsar 220 Fi Standard</a>
                            <p class="text-black">
                                <span class="bwmsprite inr-xxsm-icon"></span><span>1,67,673 in Mumbai</span>
                            </p>
                        </li>
                    </ul>
                </div>

                <div id="makeSpecsFooter"></div>
            </div>
        </section>

        <section class="<%= (Convert.ToInt32(ctrlUpcomingBikes.FetchedRecordsCount) > 0) ? "" : "hide" %>" ><!--  Upcoming, New Launches and Top Selling code starts here -->        
    	    <div class="container" >
                <div class="grid-12 margin-bottom30">
                    <h2 class="text-center margin-top30 margin-bottom20">Upcoming <%= _make.MakeName %> bikes</h2>
                    <div class="swiper-container upComingBikes padding-bottom60">
                        <div class="swiper-wrapper">
                            <BW:MUpcomingBikes runat="server" ID="ctrlUpcomingBikes" />
                        </div>
                        <div class="text-center swiper-pagination"></div>
                        <div class="bwmsprite swiper-button-next hide"></div>
                        <div class="bwmsprite swiper-button-prev hide"></div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

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
                    <h2 class="text-center margin-top40 margin-bottom30 padding-left30 padding-right30">Latest <%= _make.MakeName %> News & Reviews</h2>
                    <div class="bw-tabs-panel">
                        <div class="bw-tabs">
                            <div class="text-center <%= reviewTabsCnt > 2 ? "" : ( reviewTabsCnt > 1 ? "margin-top30 margin-bottom30" : "margin-top10") %>">
                                <div class="bw-tabs <%= reviewTabsCnt > 2 ? "bw-tabs-flex" : ( reviewTabsCnt > 1 ? "home-tabs" : "hide") %>" id="reviewCount">
                                    <ul>
                                        <li class="<%= isNewsActive ? "active" : "hide" %>" style="<%= (Convert.ToInt32(ctrlNews.FetchedRecordsCount) > 0) ? "": "display:none;" %>" data-tabs="ctrlNews"><h3>News</h3></li>
                                        <li class="<%= isExpertReviewActive ? "active" : "hide" %>" style="<%= (Convert.ToInt32(ctrlExpertReviews.FetchedRecordsCount) > 0) ? "": "display:none;" %>" data-tabs="ctrlExpertReviews"><h3>Expert Reviews</h3></li>                                   
                                        <li class="<%= isVideoActive ? "active" : "hide" %>" style="<%= (Convert.ToInt32(ctrlVideos.FetchedRecordsCount) > 0) ? "": "display:none;" %>" data-tabs="ctrlVideos"><h3>Videos</h3></li>
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

    <section class="<%= (isDescription)? "": "hide" %>"><!--  About code starts here -->
            <div class="container">
                <div class="grid-12">
                    <div class="content-inner-block-10 content-box-shadow margin-bottom30">
                        <h2 class="text-center margin-top30 margin-bottom10">About <%= _make.MakeName %> bikes</h2>
                        <div>
                            <div class="brand-about-main">
                                <%= Bikewale.Utility.FormatDescription.TruncateDescription(_bikeDesc.FullDescription, 265)%>
                            </div>
                            <div class="brand-about-more-desc hide">
                                <%= _bikeDesc.FullDescription %>
                            </div>
                            <span><a href="javascript:void(0)" class="read-more-btn" rel="nofollow">Read <span>more</span></a></span>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <!--  About code ends here -->

        <section>
            <div class="container">
                <div id="bottom-ad-div" class="bottom-ad-div">
                    <!--Bottom Ad banner code starts here -->

                </div>
                <!--Bottom Ad banner code ends here -->
            </div>
        </section>

        <% if (fetchedRecordsCount > 0)
           { %>
        <section>
            <div class="container">
                <div class="content-inner-block-10 margin-bottom30">
                    <div id="discontinuedModels" class="margin-top10 padding10" style="display: block;">
                       <div id="discontinuedLess">
                                Discontinued <%=_make.MakeName %> models: - <span id="spnContent"></span>
                            </div>
                            <div id="discontinuedMore">
                                Discontinued <%=_make.MakeName %> models: - 
                                <asp:Repeater ID="rptDiscontinued" runat="server">
                                    <ItemTemplate>
                                        <a title="<%# DataBinder.Eval(Container.DataItem,"BikeName").ToString()%>" href="<%# DataBinder.Eval(Container.DataItem,"Href").ToString()%>"><%# DataBinder.Eval(Container.DataItem,"BikeName").ToString()%></a>,
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                    </div>
                </div>
            </div>
        </section>
        <% } %>
                
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <!-- #include file="/includes/footerscript_Mobile.aspx" -->
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/bwm-brand.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript">
            ga_pg_id = '3';
            var _makeName = '<%= _make.MakeName %>';
            $(document).ready(function () {
                jQuery('.jcarousel-wrapper.upComingBikes .jcarousel')    
                .on('jcarousel:targetin', 'li', function () {
                    $("img.lazy").lazyload({
                        threshold: 300
                    });
                });
                $('#sort-by-div').insertAfter('header');
                if ($("#discontinuedMore a").length > 4) {
                    $('#discontinuedMore').hide();
                }
                else {
                    $('#discontinuedLess').hide();
                }
                $("#spnContent").append($("#discontinuedMore a:eq(0)").clone()).append(", ").append($("#discontinuedMore a:eq(1)").clone()).append(", ").append($("#discontinuedMore a:eq(2)").clone()).append(", ").append($("#discontinuedMore a:eq(3)").clone());
                $("#spnContent").append("... <a class='f-small' onclick='ShowAllDisModels()'>View All</a>");
                
            });

            if ('<%=isNewsActive%>' == "False") $("#ctrlNews").addClass("hide");
            if ('<%=isExpertReviewActive%>' == "False") $("#ctrlExpertReviews").addClass("hide");
            if ('<%=isVideoActive%>' == "False") $("#ctrlVideos").addClass("hide");
            $('#sort-btn').removeClass('hide').addClass("show");
            $("a.read-more-btn").click(function () {
                $("div.brand-about-more-desc").slideToggle();
                $("div.brand-about-main").slideToggle();
                var a = $(this).find("span");
                a.text(a.text() === "more" ? "less" : "more");
            });

            function ShowAllDisModels() {
                $("#discontinuedLess").hide();
                $("#discontinuedMore").show();
                var xContents = $('#discontinuedMore').contents();
                xContents[xContents.length - 1].nodeValue = "";
            }
        </script>
    </form>
    <div class="back-to-top" id="back-to-top"><a><span></span></a></div>
</body>
</html>

