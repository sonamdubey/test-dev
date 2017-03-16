<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.New.Model" Trace="false" EnableViewState="false" %>

<%@ Register Src="~/controls/News.ascx" TagName="LatestNews" TagPrefix="BW" %>
<%@ Register Src="~/controls/NewExpertReviews.ascx" TagName="NewExpertReviews" TagPrefix="BW" %>
<%@ Register Src="~/controls/NewVideosControl.ascx" TagName="Videos" TagPrefix="BW" %>
<%@ Register Src="~/controls/UpcomingBikes_new.ascx" TagName="UpcomingBikes" TagPrefix="BW" %>
<%@ Register Src="~/controls/MostPopularBikes_new.ascx" TagName="MostPopularBikes" TagPrefix="BW" %>
<%@ Register Src="~/controls/UsedPopularModels.ascx" TagName="PopularUsedBikes" TagPrefix="BW" %>
<%@ Register Src="~/controls/DealerCard.ascx" TagName="DealerCard" TagPrefix="BW" %>
<%@ Register Src="~/controls/LeadCaptureControl.ascx" TagName="LeadCapture" TagPrefix="BW" %>
<%@ Register Src="~/controls/ServiceCenterCard.ascx" TagName="ServiceCenterCard" TagPrefix="BW" %>
<!Doctype html>
<html>
<head>
    <%
        title = _make.MakeName + " Bikes Prices, Reviews, Mileage & Images - BikeWale";
        description = _make.MakeName + " Price in India - Rs." + Bikewale.Utility.Format.FormatPrice(_minModelPrice.ToString()) + " - Rs." + Bikewale.Utility.Format.FormatPrice(_maxModelPrice.ToString()) + ". Check out " + _make.MakeName + " on road price, reviews, mileage, versions, news & images at Bikewale.";
        alternate = "https://www.bikewale.com/m/" + _make.MaskingName + "-bikes/";
        canonical = "https://www.bikewale.com/" + _make.MaskingName + "-bikes/";
        TargetedMake = _make.MakeName;
        AdPath = "/1017752/Bikewale_NewBike_";
        AdId = "1442913773076";
        isAd970x90Shown = true;
        isAd300x250BTFShown = false;

        keywords = string.Format("{0}, {0} Bikes , {0} Bikes prices, {0} Bikes reviews, {0} Images, new {0} Bikes", _make.MakeName);
        isHeaderFix = false;
    %>
    <!-- #include file="/includes/headscript_desktop_min.aspx" -->
    <link rel="stylesheet" type="text/css" href="/css/brand.css" />
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
                                                <a href="Javascript:void(0)" data-pagecatid="1" data-pqsourceid="<%= (int)Bikewale.Entities.PriceQuote.PQSourceEnum.Desktop_MakePage %>" data-makename="<%# DataBinder.Eval(Container.DataItem,"objMake.MakeName").ToString() %>" data-modelname="<%# DataBinder.Eval(Container.DataItem,"objModel.ModelName").ToString() %>" data-modelid="<%# DataBinder.Eval(Container.DataItem, "objModel.ModelId").ToString() %>" class="btn btn-grey btn-sm margin-top15 font14 getquotation">Check on-road price</a>
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
                                                <a href="Javascript:void(0)" data-pagecatid="1" data-pqsourceid="<%= (int)Bikewale.Entities.PriceQuote.PQSourceEnum.Desktop_MakePage %>" data-makename="<%# DataBinder.Eval(Container.DataItem,"objMake.MakeName").ToString() %>" data-modelname="<%# DataBinder.Eval(Container.DataItem,"objModel.ModelName").ToString() %>" data-modelid="<%# DataBinder.Eval(Container.DataItem, "objModel.ModelId").ToString() %>" class="btn btn-grey btn-sm margin-top15 font14 getquotation">Check on-road price</a>
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
        <%if ((_bikeDesc != null && _bikeDesc.FullDescription.Length > 0) || (ctrlNews.FetchedRecordsCount > 0) || (ctrlExpertReviews.FetchedRecordsCount > 0) || (ctrlVideos.FetchedRecordsCount > 0) || (ctrlDealerCard.showWidget || (ctrlServiceCenterCard.showWidget && cityId > 0)) || (ctrlPopularUsedBikes.FetchedRecordsCount > 0)){ %>
        <section class="container">
            <div id="makeTabsContentWrapper" class="grid-12 margin-bottom20 content-details-wrapper">
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
                                <% if (ctrlDealerCard.showWidget || (ctrlServiceCenterCard.showWidget && cityId > 0))
                                   { %>
                                <a href="#dealerAndServiceContent" rel="nofollow"><% if (ctrlDealerCard.showWidget){%>Dealers<%} %>  <%if (ctrlDealerCard.showServiceCenter || (ctrlServiceCenterCard.showWidget && cityId > 0))
                                                                         { %><% if (ctrlDealerCard.showWidget){%> &<%}%> Service Centers<%} %></a>
                                <%} %>
                                <% if (ctrlPopularUsedBikes.FetchedRecordsCount > 0)
                                   {%> <a href="#makeUsedBikeContent" rel="nofollow">Used</a>
                                <%} %>
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
                    <%if (ctrlDealerCard.showWidget || (ctrlServiceCenterCard.showWidget && cityId > 0))
                      { %>
                    <div id="dealerAndServiceContent" class="bw-model-tabs-data">
                        <BW:DealerCard runat="server" ID="ctrlDealerCard" />

                        <% if(ctrlServiceCenterCard.showWidget&&cityId>0){ %>
                        <BW:ServiceCenterCard runat="server" ID="ctrlServiceCenterCard" />
                        <% } %>
                    </div>
                    <%} %>
                    <% if (ctrlPopularUsedBikes.FetchedRecordsCount > 0)
                       { %>
                    <BW:PopularUsedBikes runat="server" ID="ctrlPopularUsedBikes" />
                    <% } %>
                    <div id="overallMakeDetailsFooter"></div>
                </div>
            </div>
            <div class="clear"></div>
        </section>
        <%} %>
        <section>
            <div class="container">
                <div class="grid-12">
                    <% if (fetchedRecordsCount > 0)
                       { %>
                    <div id="discontinuedModels">
                        <div class="content-inner-block-10 margin-bottom20 font14">
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
        <BW:LeadCapture ID="ctrlLeadCapture" runat="server" />

        <!-- #include file="/includes/footerBW.aspx" -->
        <script type="text/javascript" defer src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : "" %>/src/frameworks.js?<%=staticFileVersion %>"></script>        
        <link href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript" defer src="<%= staticUrl != "" ? "https://st.aeplcdn.com" + staticUrl : "" %>/src/Plugins.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" defer  src="<%= staticUrl != "" ? "https://st.aeplcdn.com" + staticUrl : "" %>/src/common.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" defer src="<%= staticUrl != "" ? "https://st.aeplcdn.com" + staticUrl : "" %>/src/new/bikemake.js?<%= staticFileVersion %>"></script>
        <!-- #include file="/includes/fontBW.aspx" -->
    </form>
    <script type="text/javascript">
        ga_pg_id = '3';
        var _makeName = "<%= _make.MakeName %>";
        var clientIP = "<%= clientIP%>";
        var pageUrl = window.location.href;

        docReady(function () {

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

            if ($("#discontinuedMore a").length > 4) {
                $('#discontinuedMore').hide();
            }
            else {
                $('#discontinuedLess').hide();
            }
        });

    </script>
</body>
</html>
