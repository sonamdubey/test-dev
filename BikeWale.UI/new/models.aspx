<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.New.Model" Trace="false" EnableViewState="false" %>

<%@ Register Src="~/controls/News.ascx" TagName="LatestNews" TagPrefix="BW" %>
<%@ Register Src="~/controls/NewExpertReviews.ascx" TagName="NewExpertReviews" TagPrefix="BW" %>
<%@ Register Src="~/controls/NewVideosControl.ascx" TagName="Videos" TagPrefix="BW" %>
<%@ Register Src="~/controls/UpcomingBikes_new.ascx" TagName="UpcomingBikes" TagPrefix="BW" %>
<%@ Register Src="~/controls/MostPopularBikes_new.ascx" TagName="MostPopularBikes" TagPrefix="BW" %>
<%@ Register Src="~/controls/UsedBikes.ascx" TagName="MostRecentBikes" TagPrefix="BW" %>
<%@ Register Src="~/controls/DealerCard.ascx" TagName="DealerCard" TagPrefix="BW" %>
<%@ Register Src="~/controls/LeadCaptureControl.ascx" TagName="LeadCapture" TagPrefix="BW" %>
<!Doctype html>
<html>
<head>
    <%
        title = _make.MakeName + " Bikes Prices, Reviews, Mileage & Photos - BikeWale";
        description = _make.MakeName + " Price in India - Rs." + Bikewale.Utility.Format.FormatPrice(_minModelPrice.ToString()) + " - Rs." + Bikewale.Utility.Format.FormatPrice(_maxModelPrice.ToString()) + ". Check out " + _make.MakeName + " on road price, reviews, mileage, versions, news & photos at Bikewale.";
        alternate = "http://www.bikewale.com/m/" + _make.MaskingName + "-bikes/";
        canonical = "http://www.bikewale.com/" + _make.MaskingName + "-bikes/";
        TargetedMake = _make.MakeName;
        AdPath = "/1017752/Bikewale_NewBike_";
        AdId = "1442913773076";
        isAd970x90Shown = true;
        isAd300x250BTFShown = false;

        keywords = string.Format("{0}, {0} Bikes , {0} Bikes prices, {0} Bikes reviews, new {0} Bikes", _make.MakeName);
        isHeaderFix = false;
    %>
    <!-- #include file="/includes/headscript_desktop_min.aspx" -->
    <style type="text/css">
        @charset "utf-8";.content-inner-block-1420{padding:14px 20px}.text-x-black{color:#1a1a1a}#bikeMakeList .bikeDescWrapper{padding:15px 0}.inner-content-card li{width:294px;min-height:390px;margin-right:20px;margin-bottom:5px}.inner-content-card li.front{border:none}.inner-content-card li .imageWrapper{width:100%;height:166px}.inner-content-card li .imageWrapper a{display:table-cell;vertical-align:middle;background:url(http://imgd4.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif) center center no-repeat}.sort-div,.sort-selection-div{background:#fff;border:1px solid #e2e2e2}.sort-div{width:190px;height:36px;padding:7px;position:relative;cursor:pointer}.sort-by-title{width:150px}.sort-selection-div{width:190px;margin-top:10px;position:absolute;z-index:2}.sort-list-items ul::after,.sort-list-items ul::before{border-left:10px solid transparent;border-right:10px solid transparent;content:"";left:87px;position:absolute;z-index:1}.sort-selection-div ul li{padding:2px 0 2px 8px;margin-top:4px;margin-bottom:4px}.sort-selection-div ul li:hover{cursor:pointer;background:#82888b;color:#fff}.sort-selection-div ul li.selected{font-weight:700}.sort-list-items ul::before{border-bottom:10px solid #e2e2e2;top:-11px}.sort-list-items ul::after{border-bottom:10px solid #fff;top:-10px}#upDownArrow.fa-angle-down{transition:all .5s ease-in-out 0s;font-size:20px}.sort-div .fa-angle-down{transition:transform .3s;-moz-transition:transform .3s;-webkit-transition:transform .3s;-o-transition:transform .3s;-ms-transition:transform .3s}.sort-div.open .fa-angle-down{-moz-transform:rotateZ(180deg);-webkit-transform:rotateZ(180deg);-o-transform:rotateZ(180deg);-ms-transform:rotateZ(180deg);transform:rotateZ(180deg)}.sort-by-text p{height:35px;line-height:35px}#makeUpcomingBikesContent .jcarousel-control-left{left:0}#makeUpcomingBikesContent .jcarousel-control-right{right:0}.inner-content-carousel .jcarousel{width:934px;left:20px;padding:0}#makeDealersContent .inner-content-carousel .jcarousel{left:10px}.inner-content-carousel .jcarousel li{width:292px;height:auto;margin-right:28px}.inner-content-carousel h3 a.text-black{display:block}.inner-content-carousel .jcarousel-control-left{left:-10px;top:13%}.inner-content-carousel .jcarousel-control-right{right:10px;top:13%}.inner-content-carousel .jcarousel-control-next,.inner-content-carousel .jcarousel-control-prev{width:36px;height:68px}.inner-content-carousel .jcarousel-control-prev:hover{background-position:-36px -134px}.inner-content-carousel .jcarousel-control-next:hover{background-position:-63px -134px}.inner-content-carousel .jcarousel-control-prev{background-position:-36px -79px}.inner-content-carousel .jcarousel-control-next{background-position:-63px -79px}.inner-content-carousel .jcarousel-control-prev.inactive{background-position:-36px -24px}.inner-content-carousel .jcarousel-control-next.inactive{background-position:-63px -24px}.content-inner-block-2010{padding:20px 10px}#makeOverallTabsWrapper{width:100%;height:45px}.overall-floating-tabs{width:976px;background:#fff;border-bottom:1px solid #e2e2e2}#makeOverallTabs.fixed-tab{position:fixed;top:0;margin:0 auto;z-index:5;border-bottom:0;-moz-box-shadow:0 2px 2px #e2e2e2,0 1px 1px #f1f1f1;-webkit-box-shadow:0 2px 2px #e2e2e2,0 1px 1px #f1f1f1;-o-box-shadow:0 2px 2px #e2e2e2,0 1px 1px #f1f1f1;-ms-box-shadow:0 2px 2px #e2e2e2,0 1px 1px #f1f1f1;box-shadow:0 2px 2px #e2e2e2,0 1px 1px #f1f1f1}.mail-grey-icon,.phone-black-icon{margin-right:3px;position:relative}.overall-specs-tabs-wrapper{display:table;background:#fff}.overall-specs-tabs-wrapper a{padding:10px 20px;display:table-cell;font-size:14px;color:#82888b}.overall-specs-tabs-wrapper a:hover{text-decoration:none;color:#4d5057}.overall-specs-tabs-wrapper a.active{border-bottom:3px solid #ef3f30;font-weight:700;color:#4d5057}#makeTabsContentWrapper h2{margin-bottom:15px}#makeTabsContentWrapper h3{font-weight:700;margin-bottom:12px}#makeReviewsContent h3.model-section-subtitle{margin-bottom:20px}.preview-more-content{display:none}.border-divider{border-top:1px solid #e2e2e2}.line-height17{line-height:1.7}.model-preview-image-container{width:292px;height:164px;display:table;text-align:center}#makeUpcomingBikesContent .model-preview-image-container{border:none}.model-preview-image-container a{width:100%;height:164px;display:block;background:url(http://imgd4.aeplcdn.com/0x0/bw/static/sprites/d/loader.gif) center center no-repeat}.model-preview-image-container a img{width:100%;height:164px}.border-light-right{border-right:1px solid #f1f1f1}.text-truncate{width:100%;text-align:left;text-overflow:ellipsis;white-space:nowrap;overflow:hidden}#makeReviewsContent .article-target-link,#makeVideosContent .article-target-link{margin-bottom:8px;line-height:1.5}.blue-right-arrow-icon{width:6px;height:10px;background-position:-74px -469px;position:relative;top:1px;left:7px}.model-user-review-rating-container{width:148px;height:84px;border:1px solid #e2e2e2;background:#f9f9f9;padding-top:17px;text-align:center}.model-user-review-title-container{width:308px}#makeTabsContentWrapper .model-user-review-title-container h3{font-size:16px;color:#2a2a2a;line-height:1.7;margin-bottom:5px;position:relative;top:-6px}.dealer-jcarousel-image-preview{width:292px;height:114px;display:block;margin-bottom:15px;border:1px solid #f1f1f1;text-align:center;padding-top:10px}.city-sprite{background:url(http://imgd1.aeplcdn.com/0x0/bw/static/sprites/d/city-sprite.png?24062016) no-repeat;display:inline-block}.ahmedabad-icon,.bangalore-icon,.chandigarh-icon,.chennai-icon,.delhi-icon,.hyderabad-icon,.kolkata-icon,.lucknow-icon,.mumbai-icon,.pune-icon{height:92px}.mumbai-icon{width:130px;background-position:0 0}.pune-icon{width:186px;background-position:-140px 0}.bangalore-icon{width:136px;background-position:-336px 0}.delhi-icon{width:70px;background-position:-482px 0}.chennai-icon{width:53px;background-position:-562px 0}.hyderabad-icon{width:65px;background-position:-625px 0}.kolkata-icon{width:182px;background-position:-700px 0}.lucknow-icon{width:174px;background-position:-892px 0}.ahmedabad-icon,.chandigarh-icon{width:0;background-position:0 0}.dealership-loc-icon{width:8px;height:12px;background-position:-53px -469px;position:relative;top:4px}.dealership-card-details{width:92%}.vertical-top{display:inline-block;vertical-align:top}.dealership-address,.dealership-email{width:87%}.phone-black-icon{width:11px;height:15px;top:3px;background-position:-73px -444px}.mail-grey-icon{width:12px;height:10px;background-position:-92px -446px;top:6px}.dealer-no-city a{width:184px;min-height:210px;display:block;padding:20px}#makeUsedBikeContent .grid-4{display:inline-block;vertical-align:top;width:315px;float:none}.newBikes-latest-updates-container .home-tabs{background:#ccc}.newBikes-latest-updates-container .home-tabs li{width:170px;text-align:center;font-size:18px;padding:13px 14px;background:#f5f5f5}.newBikes-latest-updates-container .home-tabs li:first-child{margin-right:-2px}.newBikes-latest-updates-container .home-tabs li.active{background:#fff;color:#2a2a2a}@media only screen and (max-width:1024px){#makeUsedBikeContent .grid-4{width:300px}.inner-content-carousel .jcarousel li{width:281px!important}.dealer-jcarousel-image-preview{width:281px}.inner-content-carousel .jcarousel-control-right{right:-10px}}
    </style>

    <script type="text/javascript">
        <!-- #include file="\includes\gacode_desktop.aspx" -->
    </script>
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
                    <div id="bikeMakeList" class="padding-top25 padding-left20 rounded-corner2 inner-content-card">
                        <ul id="listitems" class="listitems">
                            <asp:Repeater ID="rptTop" runat="server">
                                <ItemTemplate>
                                    <li class="front" ind="<%#DataBinder.Eval(Container, "ItemIndex", "")%>" prc="<%# DataBinder.Eval(Container.DataItem, "VersionPrice") %>" mlg="<%# DataBinder.Eval(Container.DataItem, "Specs.FuelEfficiencyOverall") %>" pop="<%# DataBinder.Eval(Container.DataItem, "BikePopularityIndex") %>">
                                        <div class="contentWrapper">
                                            <div class="imageWrapper">
                                                <a class="modelurl" href='<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"objMake.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"objModel.MaskingName"))) %>'>
                                                    <img src="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImagePath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._310x174) %>" title="<%# DataBinder.Eval(Container.DataItem,"objMake.MakeName") + " " + DataBinder.Eval(Container.DataItem, "objModel.ModelName") %>" alt="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "objModel.ModelName")) %>" />
                                                </a>
                                            </div>
                                            <div class="bikeDescWrapper">
                                                <h3 class="bikeTitle margin-bottom10"><a class="modelurl" href='<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"objMake.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"objModel.MaskingName"))) %>' title="<%# DataBinder.Eval(Container.DataItem,"objMake.MakeName") + " " + DataBinder.Eval(Container.DataItem, "objModel.ModelName") %>"><%# DataBinder.Eval(Container.DataItem,"objMake.MakeName") + " " + DataBinder.Eval(Container.DataItem, "objModel.ModelName") %></a></h3>
                                                <div class="text-xt-light-grey font14 margin-bottom15">
                                                    <%# Bikewale.Utility.FormatMinSpecs.GetMinSpecs(Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.Displacement")),Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.FuelEfficiencyOverall")),Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.MaxPower")),Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.KerbWeight"))) %>
                                                </div>
                                                <div class="font14 text-light-grey margin-bottom5">Ex-showroom, <%=ConfigurationManager.AppSettings["defaultName"].ToString() %></div>
                                                <div class="text-bold">
                                                    <%# ShowEstimatedPrice(DataBinder.Eval(Container.DataItem, "VersionPrice")) %>
                                                </div>
                                                <a href="Javascript:void(0)" pagecatid="1" pqsourceid="<%= (int)Bikewale.Entities.PriceQuote.PQSourceEnum.Desktop_MakePage %>" makename="<%# DataBinder.Eval(Container.DataItem,"objMake.MakeName").ToString() %>" modelname="<%# DataBinder.Eval(Container.DataItem,"objModel.ModelName").ToString() %>" modelid="<%# DataBinder.Eval(Container.DataItem, "objModel.ModelId").ToString() %>" class="btn btn-grey btn-sm margin-top15 font14 fillPopupData">Check on-road price</a>
                                            </div>
                                        </div>
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
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
                                                <h3 class="bikeTitle margin-bottom10"><a class="modelurl" href='<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"objMake.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"objModel.MaskingName"))) %>' title="<%# DataBinder.Eval(Container.DataItem,"objMake.MakeName") + " " + DataBinder.Eval(Container.DataItem, "objModel.ModelName") %>"><%# DataBinder.Eval(Container.DataItem,"objMake.MakeName") + " " + DataBinder.Eval(Container.DataItem, "objModel.ModelName") %></a></h3>
                                                <div class="text-xt-light-grey font14 margin-bottom15">
                                                    <%# Bikewale.Utility.FormatMinSpecs.GetMinSpecs(Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.Displacement")),Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.FuelEfficiencyOverall")),Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.MaxPower")),Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.KerbWeight"))) %>
                                                </div>
                                                <div class="font14 text-light-grey margin-bottom5">Ex-showroom, <%=ConfigurationManager.AppSettings["defaultName"].ToString() %></div>
                                                <div class="text-bold">
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

        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/src/frameworks.js?<%=staticFileVersion %>"></script>

        <section class="<%= (ctrlUpcomingBikes.FetchedRecordsCount > 0) ? string.Empty : "hide" %>">
            <div id="makeUpcomingBikesContent" class="container margin-bottom20">
                <div class="grid-12">
                    <div class="content-box-shadow padding-top20 padding-bottom25">
                        <h2 class="padding-left20 padding-right20 text-x-black text-bold margin-bottom20">Upcoming <%= _make.MakeName %> Bikes</h2>
                        <div class="jcarousel-wrapper inner-content-carousel">
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

        <section class="container">
            <div id="makeTabsContentWrapper" class="grid-12 margin-bottom20">
                <div class="content-box-shadow">
                    <div id="makeOverallTabsWrapper">
                        <div id="makeOverallTabs" class="overall-floating-tabs">
                            <div class="overall-specs-tabs-wrapper">
                                <%
                                    if (_bikeDesc != null && _bikeDesc.FullDescription.Length > 0)
                                    {                                        
                                %>
                                <a href="#makeAboutContent" rel="nofollow">About</a>
                                <%} %>
                                <% if (ctrlNews.FetchedRecordsCount > 0)
                                   {%>
                                <a href="#modelNewsContent" rel="nofollow">News</a>
                                <%} %>
                                <% if (ctrlExpertReviews.FetchedRecordsCount > 0)
                                   { %>
                                <a href="#makeReviewsContent" rel="nofollow">Reviews</a>
                                <%} %>
                                <% if (ctrlVideos.FetchedRecordsCount > 0)
                                   {%>
                                <a href="#makeVideosContent" rel="nofollow">Videos</a>
                                <%} %>
                                <% if (ctrlDealerCard.showWidget)
                                   { %>
                                <a href="#makeDealersContent" rel="nofollow">Dealers</a>
                                <%} %>
                                <% if (ctrlRecentUsedBikes.showWidget)
                                   {%> <a href="#makeUsedBikeContent" rel="nofollow">Used</a><%} %>
                            </div>
                        </div>
                    </div>
                    <% if (_bikeDesc != null && _bikeDesc.FullDescription.Length > 0)
                       { %>
                    <div id="makeAboutContent" class="bw-model-tabs-data margin-right10 margin-left10 content-inner-block-2010 border-solid-bottom">
                        <div class="grid-8 alpha">
                            <h2><%= _make.MakeName %> Summary</h2>
                            <p class="font14 text-light-grey line-height17">
                                <span class="preview-main-content">
                                    <%= Bikewale.Utility.FormatDescription.TruncateDescription(_bikeDesc.FullDescription, 700) %>
                                </span>
                                <span class="preview-more-content hide" style="display: none;">
                                    <%= _bikeDesc.FullDescription %>
                                </span>
                                <% if (_bikeDesc.FullDescription.Length > 700)
                                   { %>
                                <a href="javascript:void(0)" class="read-more-bike-preview" rel="nofollow">Read more</a>
                                <% } %>
                            </p>
                        </div>
                        <div class="grid-4 text-center alpha omega">
                            <!-- #include file="/ads/Ad300x250.aspx" -->
                        </div>
                        <div class="clear"></div>
                    </div>
                    <% } %>
                    <!-- news control starts here -->
                    <% if (ctrlNews.FetchedRecordsCount > 0)
                       { %>
                    <BW:LatestNews runat="server" ID="ctrlNews" />
                    <% } %>
                    <!-- news control ends here -->
                    <% if (ctrlExpertReviews.FetchedRecordsCount > 0)
                       { %>
                    <div id="makeReviewsContent" class="bw-model-tabs-data margin-right10 margin-left10 padding-top20 padding-bottom20 border-solid-bottom font14">
                        <h2 class="padding-left10 padding-right10"><%= _make.MakeName %> Reviews</h2>
                        <!-- expert review starts-->
                        <BW:NewExpertReviews runat="server" ID="ctrlExpertReviews" />
                        <!-- expert review ends-->
                    </div>
                    <% } %>
                    <% if (ctrlVideos.FetchedRecordsCount > 0)
                       { %>
                    <div id="makeVideosContent" class="bw-model-tabs-data margin-right10 margin-left10 padding-top20 padding-bottom20 border-solid-bottom font14">
                        <!-- videos control starts -->
                        <BW:Videos runat="server" ID="ctrlVideos" />
                        <!-- videos control ends -->
                    </div>
                    <% } %>

                    <BW:DealerCard runat="server" ID="ctrlDealerCard" />

                    <BW:MostRecentBikes runat="server" ID="ctrlRecentUsedBikes" />

                    <div id="overallMakeDetailsFooter"></div>
                </div>
            </div>
            <div class="clear"></div>
        </section>

        <section>
            <div class="container">
                <div class="grid-12">
                    <% if (fetchedRecordsCount > 0)
                       { %>
                    <div id="discontinuedModels" class="margin-top10 padding20" style="display: block;">
                        <div class="content-inner-block-10 rounded-corner2 margin-bottom30 font14">
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
                    <% } %>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <script>

            $(document).ready(function () {
                $('#user-details-submit-btn').click(function () {
                    var bikeName = $('#getLeadBike :selected').text();
                    if (bikeName != 'Select a bike') {
                        var cityName = GetGlobalCityArea();
                        triggerGA('Make_Page', 'Lead_Submitted', bikeName + "_" + cityName);
                    }
                });

                $("img.lazy").lazyload();
                if ($("#discontinuedMore a").length > 4) {
                    $('#discontinuedMore').hide();
                }
                else {
                    $('#discontinuedLess').hide();
                }
                $("#spnContent").append($("#discontinuedMore a:eq(0)").clone()).append(", ").append($("#discontinuedMore a:eq(1)").clone()).append(", ").append($("#discontinuedMore a:eq(2)").clone()).append(", ").append($("#discontinuedMore a:eq(3)").clone());
                $("#spnContent").append("... <a class='f-small' onclick='ShowAllDisModels()'>View All</a>");
            });
            $(".upcoming-brand-bikes-container").on('jcarousel:visiblein', 'li', function (event, carousel) {
                $(this).find("img.lazy").trigger("imgLazyLoad");
            });
            function ShowAllDisModels() {
                $("#discontinuedLess").hide();
                $("#discontinuedMore").show();
                var xContents = $('#discontinuedMore').contents();
                xContents[xContents.length - 1].nodeValue = "";
            }

        </script>
        <BW:LeadCapture ID="ctrlLeadCapture" runat="server" />
        <!-- #include file="/includes/footerBW.aspx" -->
        <!--[if lt IE 9]>
            <script src="/src/html5.js"></script>
        <![endif]-->
        <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/src/common.min.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st.aeplcdn.com" + staticUrl : "" %>/src/new/bikemake.js?<%= staticFileVersion %>"></script>
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700' rel='stylesheet' type='text/css' />
    </form>
    <script type="text/javascript">
        ga_pg_id = '3';
        var _makeName = "<%= _make.MakeName %>";
        var clientIP = "<%= clientIP%>";
        var pageUrl = window.location.href;
        $(".leadcapturebtn").click(function (e) {
            ele = $(this);
            var leadOptions = {
                "dealerid": ele.attr('data-item-id'),
                "dealername": ele.attr('data-item-name'),
                "dealerarea": ele.attr('data-item-area'),
                "versionid": $("#versions a.active").attr("id"),
                "leadsourceid": ele.attr('data-leadsourceid'),
                "pqsourceid": ele.attr('data-pqsourceid'),
                "pageurl": pageUrl,
                "clientip": clientIP,
                "isdealerbikes": true,
                "campid": ele.attr('data-camp-id'),
                "isregisterpq": true
            };
            dleadvm.setOptions(leadOptions);
        });
    </script>
</body>
</html>
