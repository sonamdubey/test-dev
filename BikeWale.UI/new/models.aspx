<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.New.Model" Trace="false"  EnableViewState="false"%>

<%@ Register Src="~/controls/News.ascx" TagName="LatestNews" TagPrefix="BW" %>
<%--<%@ Register Src="~/controls/News_new.ascx" TagName="News" TagPrefix="BW" %>--%>
<%@ Register Src="~/controls/NewExpertReviews.ascx" TagName="NewExpertReviews" TagPrefix="BW" %>
<%--<%@ Register Src="~/controls/ExpertReviews.ascx" TagName="ExpertReviews" TagPrefix="BW" %>--%>
<%@ Register Src="~/controls/NewVideosControl.ascx" TagName="Videos" TagPrefix="BW" %>
<%--<%@ Register Src="~/controls/VideosControl.ascx" TagName="Videos" TagPrefix="BW" %>--%>
<%@ Register Src="~/controls/UpcomingBikes_new.ascx" TagName="UpcomingBikes" TagPrefix="BW" %>
<%@ Register Src="~/controls/MostPopularBikes_new.ascx" TagName="MostPopularBikes" TagPrefix="BW" %>

<!Doctype html>
<html>
<head>
    <%
        title = _make.MakeName + " Price in India, Review, Mileage & Photos - Bikewale";
        description = _make.MakeName + " Price in India - Rs." + Bikewale.Utility.Format.FormatPrice(_minModelPrice.ToString()) + " - Rs." + Bikewale.Utility.Format.FormatPrice(_maxModelPrice.ToString()) + ". Check out " + _make.MakeName + " on road price, reviews, mileage, versions, news & photos at Bikewale.";
        alternate = "http://www.bikewale.com/m/" + _make.MaskingName + "-bikes/";
        canonical = "http://www.bikewale.com/" + _make.MaskingName + "-bikes/";
        TargetedMake = _make.MakeName;
        AdPath = "/1017752/Bikewale_NewBike_";
        AdId = "1442913773076";
        isAd970x90Shown = true;
        keywords = string.Format("{0}, {0} Bikes , {0} Bikes prices, {0} Bikes reviews, new {0} Bikes", _make.MakeName);
    %>
    <!-- #include file="/includes/headscript.aspx" -->
    <% isHeaderFix = false; %>
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/brand.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
</head>
<body class="bg-light-grey">
    <form runat="server">
        <!-- #include file="/includes/headBW.aspx" -->

        <section class="container padding-top10">
            <div class="grid-12">
                <div class="breadcrumb margin-bottom15">
                    <ul>
                        <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                            <a href="/" itemprop="url">
                                <span itemprop="title">Home</span>
                            </a>
                        </li>
                            <li><span class="bwsprite fa-angle-right margin-right10"></span><%= _make.MakeName %> Bikes</li>
                    </ul>
                    <div class="clear"></div>
                </div>
            </div>
            <div class="clear"></div>
        </section>      
        
        <section class="container margin-bottom20">
            <div class="grid-12">
                <div class="content-box-shadow">
                    <div class="content-box-shadow content-inner-block-1420">
                        <div class="grid-8 alpha">
                            <h1 class="leftfloat font24 text-x-black"><%= _make.MakeName %> Bikes</h1>
                        </div>
                        <div class="grid-4 rightfloat omega font14" id="sortByContainer">
                            <div class="leftfloat sort-by-text text-light-grey margin-left50">
                                <p>Sort by:</p>
                            </div>
                            <div class="rightfloat">
                                <div class="sort-div rounded-corner2">
                                    <div class="sort-by-title" id="sort-by-container">
                                        <span class="leftfloat sort-select-btn text-truncate">Price: Low to High</span>
                                        <span class="clear"></span>
                                    </div>
                                    <span id="upDownArrow" class="rightfloat fa fa-angle-down position-abt pos-top15 pos-right10"></span>
                                </div>
                                <div class="sort-selection-div sort-list-items hide">
                                    <ul id="sortbike">
                                        <li id="0" class="selected">Price: Low to High</li>
                                        <li id="1">Popular</li>
                                        <li id="2">Price: High to Low</li>
                                        <li id="3">Mileage: High to Low</li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                        <div class="clear"></div>
                    </div>
                    <div id="bikeMakeList" class="brand-bikes-list-container padding-top25 padding-left20 rounded-corner2">
                        <ul id="listitems" class="listitems">
                            <asp:Repeater ID="rptMostPopularBikes" runat="server">
                                <ItemTemplate>
                                    <li class="front" ind="<%#DataBinder.Eval(Container, "ItemIndex", "")%>" prc="<%# DataBinder.Eval(Container.DataItem, "VersionPrice") %>" mlg="<%# DataBinder.Eval(Container.DataItem, "Specs.FuelEfficiencyOverall") %>" pop="<%# DataBinder.Eval(Container.DataItem, "BikePopularityIndex") %>">
                                        <div class="contentWrapper">
                                            <div class="imageWrapper">
                                                <a class="modelurl" href='<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"objMake.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"objModel.MaskingName"))) %>'>
                                                    <img class="lazy" data-original="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImagePath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._310x174) %>" title="<%# DataBinder.Eval(Container.DataItem,"objMake.MakeName") + " " + DataBinder.Eval(Container.DataItem, "objModel.ModelName") %>" alt="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "objModel.ModelName")) %>" src="">
                                                </a>
                                            </div>
                                            <div class="bikeDescWrapper">
                                                <div class="bikeTitle margin-bottom10">
                                                    <h3><a class="modelurl" href='<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"objMake.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"objModel.MaskingName"))) %>' title="<%# DataBinder.Eval(Container.DataItem,"objMake.MakeName") + " " + DataBinder.Eval(Container.DataItem, "objModel.ModelName") %>"><%# DataBinder.Eval(Container.DataItem,"objMake.MakeName") + " " + DataBinder.Eval(Container.DataItem, "objModel.ModelName") %></a></h3>
                                                </div>
                                                <div class="text-xt-light-grey font14 margin-bottom15">
                                                    <%# Bikewale.Utility.FormatMinSpecs.GetMinSpecs(Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.Displacement")),Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.FuelEfficiencyOverall")),Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.MaxPower"))) %>
                                                </div>
                                                <div class="font14 text-light-grey margin-bottom5">Ex-showroom, <%=ConfigurationManager.AppSettings["defaultName"].ToString() %></div>
                                                <div class="font16 text-bold">
                                                    <%# ShowEstimatedPrice(DataBinder.Eval(Container.DataItem, "VersionPrice")) %>
                                                </div>
                                                <a href="Javascript:void(0)" pagecatid="1" pqsourceid="<%= (int)Bikewale.Entities.PriceQuote.PQSourceEnum.Desktop_MakePage %>" makename="<%# DataBinder.Eval(Container.DataItem,"objMake.MakeName").ToString() %>" modelname="<%# DataBinder.Eval(Container.DataItem,"objModel.ModelName").ToString() %>" modelid="<%# DataBinder.Eval(Container.DataItem, "objModel.ModelId").ToString() %>" class="btn btn-grey btn-sm margin-top15 font14 fillPopupData">Check on-road price</a>
                                            </div>
                                        </div>
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                    </div>
                </div>
            </div>
            <div class="clear"></div>            
        </section>
        
        <section id="makeUpcomingBikesContent" class="container margin-bottom20">
            <div class="grid-12">
                <div class="content-box-shadow padding-top20 padding-bottom25">
                    <h2 class="padding-left20 padding-right20 text-x-black text-bold margin-bottom20">Upcoming <%= _make.MakeName %> Bikes</h2>
                    <div class="jcarousel-wrapper bike-carousel-wrapper">
                        <div class="jcarousel">
                            <ul>
                                <li>
                                    <div class="model-preview-image-container margin-bottom15">
                                        <a href="">
                                            <img class="lazy" data-original="http://imgd1.aeplcdn.com//310x174//bw/upcoming/honda-cb500f-420.jpg?20151209054312" title="" alt="" />
                                        </a>
                                    </div>
                                    <a href="" class="block font16 text-bold margin-bottom10 text-black">Harley Davidson Heritage</a>
                                    <p class="font14 text-light-grey margin-bottom5">Expected launch</p>
                                    <p class="font16 text-bold margin-bottom15">June 2016</p>
                                    <p class="font14 text-light-grey margin-bottom5">Expected price</p>
                                    <div class="font16">
                                        <span class="fa fa-rupee"></span>&nbsp;<span class="text-bold">50,398</span>
                                    </div>
                                </li>
                                <li>
                                    <div class="model-preview-image-container margin-bottom15">
                                        <a href="">
                                            <img class="lazy" data-original="http://imgd1.aeplcdn.com//310x174//bw/models/honda-cbr1000rr-fireblade-c-abs-125.jpg?20151209184557" title="" alt="" />
                                        </a>
                                    </div>
                                    <a href="" class="block font16 text-bold margin-bottom10 text-black">Harley Davidson Heritage Softail Classic</a>
                                    <p class="font14 text-light-grey margin-bottom5">Expected launch</p>
                                    <p class="font16 text-bold margin-bottom15">June 2016</p>
                                    <p class="font14 text-light-grey margin-bottom5">Expected price</p>
                                    <div class="font16">
                                        <span class="fa fa-rupee"></span>&nbsp;<span class="text-bold">50,398</span>
                                    </div>
                                </li>
                                <li>
                                    <div class="model-preview-image-container margin-bottom15">
                                        <a href="">
                                            <img class="lazy" data-original="http://imgd1.aeplcdn.com//310x174//bw/models/honda-cbr250r-sports/red/black-color-std-121.jpg?20151209184646" title="" alt="" />
                                        </a>
                                    </div>
                                    <a href="" class="block font16 text-bold margin-bottom10 text-black">Harley Davidson Heritage</a>
                                    <p class="font14 text-light-grey margin-bottom5">Expected launch</p>
                                    <p class="font16 text-bold margin-bottom15">June 2016</p>
                                    <p class="font14 text-light-grey margin-bottom5">Expected price</p>
                                    <div class="font16">
                                        <span class="fa fa-rupee"></span>&nbsp;<span class="text-bold">50,398</span>
                                    </div>
                                </li>
                                <li>
                                    <div class="model-preview-image-container margin-bottom15">
                                        <a href="">
                                            <img class="lazy" data-original="http://imgd1.aeplcdn.com//310x174//bw/upcoming/honda-cb500f-420.jpg?20151209054312" title="" alt="" />
                                        </a>
                                    </div>
                                    <a href="" class="block font16 text-bold margin-bottom10 text-black">Harley Davidson Heritage Softail Classic</a>
                                    <p class="font14 text-light-grey margin-bottom5">Expected launch</p>
                                    <p class="font16 text-bold margin-bottom15">June 2016</p>
                                    <p class="font14 text-light-grey margin-bottom5">Expected price</p>
                                    <div class="font16">
                                        <span class="fa fa-rupee"></span>&nbsp;<span class="text-bold">50,398</span>
                                    </div>
                                </li>
                            </ul>
                        </div>
                        <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev" rel="nofollow"></a></span>
                        <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next" rel="nofollow"></a></span>
                    </div>
                </div>
            </div>
            <div class="clear"></div>
        </section>

        <section class="container">
            <div id="makeTabsContentWrapper" class="grid-12 margin-bottom20">
                <div class="content-box-shadow">
                    <div id="makeOverallTabsWrapper">
                        <div id="makeOverallTabs">
                            <div class="overall-specs-tabs-wrapper">
                                <a class="active" href="#makeAboutContent" rel="nofollow">About</a>
                                <a href="#modelNewsContent" rel="nofollow">News</a>
                                <a href="#makeReviewsContent" rel="nofollow">Reviews</a>
                                <a href="#makeDealersContent" rel="nofollow">Dealers</a>
                                <a href="#makeUsedBikeContent" rel="nofollow">Used</a>
                            </div>
                        </div>
                    </div>
                    <div id="makeAboutContent" class="bw-model-tabs-data margin-right10 margin-left10 content-inner-block-2010 border-solid-bottom">
                        <div class="grid-8 alpha">
                            <h2>Bajaj Pulsar RS200 Summary</h2>
                            <p class="font14 text-light-grey line-height17">
                                <span class="preview-main-content">After number of spy pictures doing the round of the internet, Bajaj Motorcycles has finally 
                                launched its first fully-faired motorcycle, the Pulsar RS 200 for the Indian market. Previously 
                                touted to be called as the Pulsar SS200, this bike has been the most anticipated launch from 
                                the company.<br /><br />
                                Marketed as the fastest Pulsar yet, the Pulsar RS200 designed to be a compact sportsbike
                                and features clip-on handlebars. Unlike other fully-faired motorcycle like the Yamaha YZF-R15,
                                the RS200 doesn’t have as aggressive riding stance as of a super sport motorcycle...
                                </span>
                                <span class="preview-more-content">After number of spy pictures doing the round of the internet, Bajaj Motorcycles has finally 
                                launched its first fully-faired motorcycle, the Pulsar RS 200 for the Indian market. Previously 
                                touted to be called as the Pulsar SS200, this bike has been the most anticipated launch from 
                                the company.<br /><br />
                                Marketed as the fastest Pulsar yet, the Pulsar RS200 designed to be a compact sportsbike
                                and features clip-on handlebars. Unlike other fully-faired motorcycle like the Yamaha YZF-R15,
                                the RS200 doesn’t have as aggressive riding stance as of a super sport motorcycle.<br /><br />
                                Marketed as the fastest Pulsar yet, the Pulsar RS200 designed to be a compact sportsbike
                                and features clip-on handlebars. Unlike other fully-faired motorcycle like the Yamaha YZF-R15,
                                the RS200 doesn’t have as aggressive riding stance as of a super sport motorcycle.
                                </span>
                                <a href="javascript:void(0)" class="read-more-bike-preview" rel="nofollow">Read more</a>
                            </p>
                        </div>
                        <div class="grid-4 text-center alpha omega">
                            <!-- #include file="/ads/Ad300x250.aspx" -->
                        </div>
                        <div class="clear"></div>
                    </div>

                    <!-- news control starts here -->
                    <% if(ctrlNews.FetchedRecordsCount > 0){ %>
                    <BW:LatestNews runat="server" ID="ctrlNews" />
                    <% } %>
                    <!-- news control ends here -->

                    <div id="makeReviewsContent" class="bw-model-tabs-data margin-right10 margin-left10 padding-top20 padding-bottom20 border-solid-bottom font14">
                        <h2 class="padding-left10 padding-right10"><%= _make.MakeName %> Reviews</h2>
                        <!-- expert review starts-->
                        <% if(ctrlExpertReviews.FetchedRecordsCount > 0){ %>
                        <BW:NewExpertReviews runat="server" ID="ctrlExpertReviews" />
                        <% } %>
                        <!-- expert review ends-->
                        <!-- user reviews control -->
                    </div>

                    <!-- videos control starts -->
                         <% if(ctrlVideos.FetchedRecordsCount > 0){ %>
                         <BW:Videos runat="server" ID="ctrlVideos" />
                         <% } %>
                        <!-- Videos review ends-->

                    <div id="makeDealersContent" class="bw-model-tabs-data margin-right10 margin-left10 padding-top20 padding-bottom20 border-solid-bottom font14">
                        <h2 class="padding-left10 padding-right10"><%= _make.MakeName %> Dealers in India</h2>
                        <div class="jcarousel-wrapper bike-carousel-wrapper margin-bottom15">
                            <div class="jcarousel">
                                <ul>
                                    <li>
                                        <a href="" class="dealer-jcarousel-image-preview">
                                            <span></span>
                                        </a>
                                        <h3 class="margin-bottom10"><a href="" class="text-default"><%= _make.MakeName %> dealers in Mumbai</a></h3>
                                        <p>24 showrooms</p>
                                    </li>
                                    <li>
                                        <a href="" class="dealer-jcarousel-image-preview">
                                            <span></span>
                                        </a>
                                        <h3 class="margin-bottom10"><a href="" class="text-default"><%= _make.MakeName %> dealers in Pune</a></h3>
                                        <p>24 showrooms</p>
                                    </li>
                                    <li>
                                        <a href="" class="dealer-jcarousel-image-preview">
                                            <span></span>
                                        </a>
                                        <h3 class="margin-bottom10"><a href="" class="text-default"><%= _make.MakeName %> dealers in Bengaluru</a></h3>
                                        <p>24 showrooms</p>
                                    </li>
                                    <li>
                                        <a href="" class="dealer-jcarousel-image-preview">
                                            <img />
                                        </a>
                                        <h3 class="margin-bottom10"><a href="" class="text-default"><%= _make.MakeName %> dealers in Mumbai</a></h3>
                                        <p>24 showrooms</p>
                                    </li>
                                </ul>
                            </div>
                            <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev" rel="nofollow"></a></span>
                            <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next" rel="nofollow"></a></span>
                        </div>
                        <div class="padding-left10">
                            <a href="javascript:void(0)">View all dealers<span class="bwsprite blue-right-arrow-icon"></span></a>
                        </div>
                    </div>

                    <div id="makeUsedBikeContent" class="bw-model-tabs-data margin-right10 margin-left10 padding-top20 padding-bottom20 font14">
                        <h2 class="padding-left10 padding-right10">Recently uploaded <%= _make.MakeName %> Used bikes</h2>
                        <!-- when city is not selected -->
                        <div class="grid-12 alpha omega text-black">
                            <div class="grid-4 margin-bottom20">
                                <a href=""><%= _make.MakeName %> Used bikes in Mumbai</a>
                                <p class="margin-top10">500 bikes available</p>
                            </div>
                            <div class="grid-4 margin-bottom20">
                                <a href=""><%= _make.MakeName %> Used bikes in Pune</a>
                                <p class="margin-top10">500 bikes available</p>
                            </div>
                            <div class="grid-4 margin-bottom20">
                                <a href=""><%= _make.MakeName %> Used bikes in Bengaluru</a>
                                <p class="margin-top10">500 bikes available</p>
                            </div>
                            <div class="grid-4 margin-bottom20">
                                <a href=""><%= _make.MakeName %> Used bikes in Delhi</a>
                                <p class="margin-top10">500 bikes available</p>
                            </div>
                            <div class="grid-4 margin-bottom20">
                                <a href=""><%= _make.MakeName %> Used bikes in Hyderabad</a>
                                <p class="margin-top10">500 bikes available</p>
                            </div>
                            <div class="grid-4 margin-bottom20">
                                <a href=""><%= _make.MakeName %> Used bikes in Chennai</a>
                                <p class="margin-top10">500 bikes available</p>
                            </div>
                            <div class="clear"></div>
                        </div>
                        <div class="clear"></div>

                        <!-- when city is selected -->
                        <div class="grid-12 alpha omega text-black">
                            <div class="grid-4 margin-bottom20">
                                <a href="">2009, Bajaj Pulsar 220 Fi Standard</a>
                                <p class="margin-top10"><span class="fa fa-rupee"></span> <span>1,67,673</span> in Mumbai</p>
                            </div>
                            <div class="grid-4 margin-bottom20">
                                <a href="">2009, Bajaj Pulsar 220 Fi Standard</a>
                                <p class="margin-top10"><span class="fa fa-rupee"></span> <span>1,67,673</span> in Mumbai</p>
                            </div>
                            <div class="grid-4 margin-bottom20">
                                <a href="">2009, Bajaj Pulsar 220 Fi Standard</a>
                                <p class="margin-top10"><span class="fa fa-rupee"></span> <span>1,67,673</span> in Mumbai</p>
                            </div>
                            <div class="clear"></div>
                        </div>
                        <div class="clear"></div>
                    </div>

                    <div id="overallMakeDetailsFooter"></div>
                </div>
            </div>
            <div class="clear"></div>
        </section>

        <section class="<%= (ctrlUpcomingBikes.FetchedRecordsCount > 0) ? string.Empty : "hide" %>">
            <div class="container">
                <div class="grid-12">
                    <h2 class="text-bold text-center margin-top50 margin-bottom30">Upcoming <%= _make.MakeName %> Bikes</h2>
                    <div class="content-box-shadow rounded-corner2">
                        <div class="jcarousel-wrapper upcoming-brand-bikes-container margin-top20">
                            <div class="jcarousel">
                                <ul>
                                    <BW:UpcomingBikes runat="server" ID="ctrlUpcomingBikes" />
                                </ul>
                            </div>
                            <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev" rel="nofollow"></a></span>
                            <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next" rel="nofollow"></a></span>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <section>
            <!--  News Bikes code starts here -->
            <div class="container newBikes-latest-updates-container <%= reviewTabsCnt == 0 ? "hide" : string.Empty %>">
                <div class="grid-12 margin-bottom20">
                    <h2 class="text-bold text-center margin-top50 margin-bottom30">Latest <%=_make.MakeName%> News & Reviews from the industry</h2>
                    <div class="bw-tabs-panel content-box-shadow margin-bottom30">
                        <div class="text-center <%= reviewTabsCnt > 2 ? "" : ( reviewTabsCnt > 1 ? "margin-top30 margin-bottom30" : "margin-top10") %>">
                            <div class="bw-tabs <%= reviewTabsCnt > 2 ? "bw-tabs-flex" : ( reviewTabsCnt > 1 ? "home-tabs" : "hide") %>">
                                <ul>
                                    <li class="<%= isNewsActive ? "active" : "hide" %>" style="<%= (Convert.ToInt32(ctrlNews.FetchedRecordsCount) > 0) ? "": "display:none;" %>" data-tabs="ctrlNews"><h3 class="text-unbold">News</h3></li>
                                    <li class="<%= isExpertReviewActive ? "active" : "hide" %>" style="<%= (Convert.ToInt32(ctrlExpertReviews.FetchedRecordsCount) > 0) ? "": "display:none;" %>" data-tabs="ctrlExpertReviews"><h3 class="text-unbold">Expert Reviews</h3></li>
                                    <li class="<%= isVideoActive ? "active" : "hide" %>" style="<%= (Convert.ToInt32(ctrlVideos.FetchedRecordsCount) > 0) ? "": "display:none;" %>" data-tabs="ctrlVideos"><h3 class="text-unbold">Videos</h3></li>
                                </ul>
                            </div>
                        </div>
                        <%--<%if (!isNewsZero)
                          { %>
                        <BW:LatestNews runat="server" ID="ctrlNews" />
                        <% } %>--%>
                        <%--<%if (!isExpertReviewZero)
                          { %>
                        <BW:ExpertReviews runat="server" ID="ctrlExpertReviews" />
                        <% } %>--%>
                        <%--<%if (!isVideoZero)
                          { %>
                        <BW:Videos runat="server" ID="ctrlVideos" />
                        <% } %>--%>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <section class="<%= (isDescription)? "": "hide" %>">
            <!-- About Brand code starts here-->
            <div class="container">
                <div class="grid-12" style="<%= (isDescription) ? "": "display:none;" %>">
                    <h2 class="text-bold text-center margin-top30 margin-bottom30">About <%= _make.MakeName %></h2>
                    <div class="content-box-shadow content-inner-block-10 rounded-corner2 margin-bottom30 font14">
                        <div class="brand-about-main">
                            <%= _bikeDesc !=null ? Bikewale.Utility.FormatDescription.TruncateDescription(_bikeDesc.FullDescription, 265) : ""%>
                        </div>
                        <div class="brand-about-more-desc hide">
                            <%= _bikeDesc !=null ? _bikeDesc.FullDescription : "" %>
                        </div>
                        <span><a href="javascript:void(0)" class="read-more-btn">Read <span>more</span></a></span>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <script>
            $("a.read-more-btn").click(function () {
                $("div.brand-about-more-desc").slideToggle();
                $("div.brand-about-main").slideToggle();
                var a = $(this).find("span");
                a.text(a.text() === "more" ? "less" : "more");
            });

            $(document).ready(function () { $("img.lazy").lazyload(); });
            $(".upcoming-brand-bikes-container").on('jcarousel:visiblein', 'li', function (event, carousel) {
                $(this).find("img.lazy").trigger("imgLazyLoad");
            });
           <%-- if ('<%=isNewsActive%>' == "False") $("#ctrlNews").addClass("hide");
            if ('<%=isExpertReviewActive%>' == "False") $("#ctrlExpertReviews").addClass("hide");
            if ('<%=isVideoActive%>' == "False") $("#ctrlVideos").addClass("hide");--%>

        </script>
        <!-- #include file="/includes/footerBW.aspx" -->
        <!-- #include file="/includes/footerscript.aspx" -->
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st.aeplcdn.com" + staticUrl : "" %>/src/new/bikemake.js?<%= staticFileVersion %>"></script>
    </form>
    <script type="text/javascript">
        ga_pg_id = '3';
        var _makeName = '<%= _make.MakeName %>';
    </script>
</body>
</html>
