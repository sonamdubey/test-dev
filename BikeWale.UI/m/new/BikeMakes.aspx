<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.BikeMakes" EnableViewState="false" %>
<%@ Register Src="~/m/controls/NewMUpcomingBikes.ascx" TagName="MUpcomingBikes" TagPrefix="BW" %>
<%@ Register Src="/m/controls/NewNewsWidget.ascx" TagName="News" TagPrefix="BW" %>
<%@ Register Src="/m/controls/NewExpertReviewsWidget.ascx" TagName="ExpertReviews" TagPrefix="BW" %>
<%@ Register Src="/m/controls/NewVideosWidget.ascx" TagName="Videos" TagPrefix="BW" %>
<%@ Register Src="~/m/controls/DealersCard.ascx" TagName="DealerCard" TagPrefix="BW" %>
<%@ Register Src="~/m/controls/LeadCaptureControl.ascx" TagName="LeadCapture" TagPrefix="BW" %>
<!doctype html>
<html>
<head>
    <%
        title = _make.MakeName + " Bikes Prices, Reviews, Mileage & Photos - BikeWale";
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
                        <div class="filter-sort-div font14 bg-white">
                            <div sc="1" so="">
                                <a data-title="sort" class="price-sort position-rel text-bold">Price <span class="hide" so="0" class="sort-text ">: Low</span>
                                </a>
                            </div>
                            <div sc="" class="border-solid-left">
                                <a data-title="sort" class="position-rel">Popularity 
                                </a>
                            </div>
                            <div sc="2" class="border-solid-left">
                                <a data-title="sort" class="position-rel">Mileage 
                                </a>
                            </div>
                        </div>
                    </div>
                    <!--  class="grid-12"-->
                    <div class="bg-white box-shadow content-inner-block-1520">
                        <h1><%= _make.MakeName %> Bikes</h1>
                    </div>
                    <div class="search-bike-container position-rel pos-top3 box-shadow">
                        <div class="search-bike-item">
                            <div id="listitems" class="listitems">
                                <asp:Repeater ID="rptMostPopularBikes" runat="server">
                                    <ItemTemplate>
                                        <div class="front" ind="<%#DataBinder.Eval(Container, "ItemIndex", "")%>" prc="<%# DataBinder.Eval(Container.DataItem, "VersionPrice") %>" mlg="<%# DataBinder.Eval(Container.DataItem, "Specs.FuelEfficiencyOverall") %>" pop="<%# DataBinder.Eval(Container.DataItem, "BikePopularityIndex") %>">
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
                                                        <%# Bikewale.Utility.FormatMinSpecs.GetMinSpecs(Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.Displacement")),Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.FuelEfficiencyOverall")),Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.MaxPower")),Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.Kerbweight"))) %>
                                                    </div>
                                                    <div class="margin-bottom5 font14 text-light-grey">Ex-showroom, <%=ConfigurationManager.AppSettings["defaultName"].ToString() %></div>
                                                    <div class="margin-bottom5">
                                                        <span class="bwmsprite inr-sm-icon" style="<%# (Convert.ToString(DataBinder.Eval(Container.DataItem, "VersionPrice"))=="0")?"display:none;": "display:inline-block;"%>"></span>
                                                        <span class="text-bold font18"><%# ShowEstimatedPrice(DataBinder.Eval(Container.DataItem, "VersionPrice")) %></span>
                                                    </div>
                                                    <a href="javascript:void(0)" makename="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "objMake.MakeName")) %>" modelname="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "objModel.ModelName")) %>" pagecatid="1" pqsourceid="<%= (int)Bikewale.Entities.PriceQuote.PQSourceEnum.Mobile_MakePage %>" modelid="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "objModel.ModelId")) %>" class="btn btn-sm btn-white font14 margin-top20 getquotation" rel="nofollow">Check on-road price</a>
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

        <% if (ctrlUpcomingBikes.FetchedRecordsCount > 0)
           { %>
        <BW:MUpcomingBikes runat="server" ID="ctrlUpcomingBikes" />
        <%} %>

        <section>
            <div id="makeTabsContentWrapper" class="container bg-white clearfix box-shadow margin-bottom20">
                <div id="makeOverallTabsWrapper">
                    <div id="overallSpecsTab" class="overall-specs-tabs-container">
                        <ul class="overall-specs-tabs-wrapper">
                             <% if (_bikeDesc != null && _bikeDesc.FullDescription.Length > 0) { %>
                            <li data-tabs="#makeAboutContent">About</li>
                            <% } %>
                            <% if (ctrlNews.FetchedRecordsCount > 0)
                               {%>
                            <li data-tabs="#makeNewsContent">News</li>
                            <% } %>
                            <% if (ctrlExpertReviews.FetchedRecordsCount > 0)
                               { %>
                            <li data-tabs="#makeReviewsContent">Reviews</li>
                            <%} %>
                            <% if (ctrlVideos.FetchedRecordsCount > 0)
                               { %>
                            <li data-tabs="#makeVideosContent">Videos</li>
                            <%} %>
                            <% if (ctrlDealerCard.showWidget)
                               { %>
                            <li data-tabs="#makeDealersContent">Dealers</li>
                            <%} %>
                        </ul>
                    </div>
                </div>

                <% if (_bikeDesc != null && _bikeDesc.FullDescription.Length > 0)
                   { %>
                <div id="makeAboutContent" class="bw-model-tabs-data padding-top15 padding-bottom15 border-solid-bottom">
                    <div class="margin-bottom20 padding-right20 padding-left20">
                        <h2>About <%= _make.MakeName %></h2>
                        <p class="font14 text-light-grey line-height17 margin-bottom15">
                            <span class="model-preview-main-content">
                                <%= Bikewale.Utility.FormatDescription.TruncateDescription(_bikeDesc.FullDescription, 700) %>
                            </span>
                            <span class="model-preview-more-content">
                                <%= _bikeDesc.FullDescription %>
                            </span>
                            <% if (_bikeDesc.FullDescription.Length > 700)
                               { %>
                            <a href="javascript:void(0)" class="read-more-model-preview" rel="nofollow">Read more</a>
                            <% } %>
                        </p>
                    </div>
                    <div class="margin-bottom20 text-center">
                        <!-- #include file="/ads/Ad300x250.aspx" -->
                    </div>
                </div>
                <% } %>
                <% if (ctrlNews.FetchedRecordsCount > 0)
                   {%>
                <BW:News runat="server" ID="ctrlNews" />
                <%} %>

                <% if (ctrlExpertReviews.FetchedRecordsCount > 0)
                   { %>
                <div class="bw-model-tabs-data margin-right20 margin-left20 padding-top15 padding-bottom20 border-solid-bottom font14" id="makeReviewsContent">
                    <h2><%=_make.MakeName %> Reviews</h2>
                    <BW:ExpertReviews runat="server" ID="ctrlExpertReviews" />
                </div>
                <% } %>
                <%if (ctrlVideos.FetchedRecordsCount > 0)
                  { %>
                <div id="makeVideosContent" class="bw-model-tabs-data margin-right20 margin-left20 padding-top15 padding-bottom20 border-solid-bottom font14">
                    <h2><%= _make.MakeName %> Videos</h2>
                    <BW:Videos runat="server" ID="ctrlVideos" />
                </div>
                <% } %>

                <% if (ctrlDealerCard.showWidget) { %>
                    <BW:DealerCard runat="server" ID="ctrlDealerCard" />
                <% }  %>
                <div id="makeSpecsFooter"></div>
            </div>
        </section>
        <!--  News, reviews and videos code ends here -->
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
                    <div id="discontinuedModels" style="display: block;">
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

           <BW:LeadCapture ID="ctrlLeadCapture" runat="server" />

        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <!-- #include file="/includes/footerscript_Mobile.aspx" -->
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/bwm-brand.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript">
            ga_pg_id = '3';
            var _makeName = '<%= _make.MakeName %>';
             
            var clientIP = '<%= Bikewale.Common.CommonOpn.GetClientIP() %>';
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
                    "isregisterpq": true,
                    "gaobject": {
                        cat: ele.attr('data-ga-cat'),
                        act: ele.attr('data-ga-act'),
                        lab: ele.attr('data-ga-lab')
                    }
                };

                dleadvm.setOptions(leadOptions);

            }); 

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
    <div class="back-to-top" id="back-to-top"></div>
</body>
</html>

