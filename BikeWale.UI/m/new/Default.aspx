﻿<%@ Page Language="C#" AutoEventWireup="False" Inherits="Bikewale.Mobile.New.Default" EnableViewState="true" %>

<%@ Register Src="~/m/controls/MUpcomingBikes.ascx" TagName="MUpcomingBikes" TagPrefix="BW" %>
<%@ Register Src="~/m/controls/MNewLaunchedBikes.ascx" TagName="MNewLaunchedBikes" TagPrefix="BW" %>
<%@ Register Src="~/m/controls/MMostPopularBikes.ascx" TagName="MMostPopularBikes" TagPrefix="BW" %>
<%@ Register Src="/m/controls/CompareBikesMin.ascx" TagName="CompareBike" TagPrefix="BW" %>
<%@ Register Src="/m/controls/NewNewsWidget.ascx" TagName="News" TagPrefix="BW" %>
<%@ Register Src="/m/controls/NewExpertReviewsWidget.ascx" TagName="ExpertReviews" TagPrefix="BW" %>
<%@ Register Src="/m/controls/NewVideosWidget.ascx" TagName="Videos" TagPrefix="BW" %>

<!doctype html>
<html>
<head>
    <%
        title = "New Bikes - Bikes Reviews, Photos, Specs, Features, Tips & Advices - BikeWale";
        keywords = "new bikes, new bikes prices, new bikes comparisons, bikes dealers, on-road price, bikes research, bikes india, Indian bikes, bike reviews, bike photos, specs, features, tips & advices";
        description = "New bikes in India. Search for the right new bikes for you, know accurate on-road price and discounts. Compare new bikes and find dealers.";
        canonical = "http://www.bikewale.com/new/";
        AdPath = "/1017752/Bikewale_Mobile_NewHome";
        AdId = "1450262524302";
        Ad320x150_I = true;
        Ad320x150_II = true;
    %>
    <!-- #include file="/includes/headscript_mobile.aspx" -->
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-newbikes.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css">
</head>
<body class="bg-light-grey">
    <form runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->
        <section>
            <div class="container">
                <div class="newbikes-banner-div">
                    <!-- Top banner code starts here -->
                    <h1 class="font22 text-uppercase text-white text-center padding-top25 font24">NEW Bikes</h1>
                    <p class=" font16 text-white text-center">View all bikes under one roof</p>
                </div>
                <!-- Top banner code ends here -->
            </div>
        </section>
        <section class="container">
            <!-- Brand section code starts here -->
            <div class="grid-12">
                <div class="bg-white brand-wrapper content-box-shadow margin-minus60">
                    <h2 class="content-inner-block-10 text-uppercase text-center margin-top30 margin-bottom20">Brand</h2>
                    <div class="brand-type-container">
                        <ul class="text-center">
                            <asp:Repeater ID="rptPopularBrand" runat="server">
                                <ItemTemplate>
                                    <li>
                                        <a href="/m/<%# DataBinder.Eval(Container.DataItem, "MaskingName") %>-bikes/">
                                            <span class="brand-type">
                                                <span class="lazy brandlogosprite brand-<%# DataBinder.Eval(Container.DataItem, "MakeId") %>"></span>
                                            </span>
                                            <span class="brand-type-title"><%# DataBinder.Eval(Container.DataItem, "MakeName") %></span>
                                        </a>
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                        <ul id="more-brand-nav" class="brand-style-moreBtn brandTypeMore border-top1 padding-top25 text-center hide">
                            <asp:Repeater ID="rptOtherBrands" runat="server">
                                <ItemTemplate>
                                    <li>
                                        <a href="/m/<%# DataBinder.Eval(Container.DataItem, "MaskingName") %>-bikes/">
                                            <span class="brand-type">
                                                <span class="lazy brandlogosprite brand-<%# DataBinder.Eval(Container.DataItem, "MakeId") %>"></span>
                                            </span>
                                            <span class="brand-type-title"><%# DataBinder.Eval(Container.DataItem, "MakeName") %></span>
                                        </a>
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>  
                    </div>
                    <div class="text-center padding-bottom20">
                        <a href="javascript:void(0)" id="more-brand-tab" class="view-more-btn font16">View <span>more</span> Brands</a>
                    </div>
                </div>
            </div>
            <div class="clear"></div>
        </section>

        <% if (Ad320x150_I)
           { %>
        <section>
            <!-- #include file="/ads/Ad320x150_First.aspx" -->
        </section>
        <% } %>
        <section>
            <!--  Upcoming, New Launches and Top Selling code starts here -->
            <div class="container <%= ((mctrlMostPopularBikes.FetchedRecordsCount + mctrlMostPopularBikes.FetchedRecordsCount + mctrlMostPopularBikes.FetchedRecordsCount) > 0 )?"":"hide" %> ">
                <div class="grid-12 alpha omega">
                    <h2 class="font18 text-center margin-top20 margin-bottom20">Featured bikes</h2>
                    <div class="featured-bikes-panel content-box-shadow padding-bottom15">
                        <div class="bw-tabs-panel">
                            <div class="bw-tabs bw-tabs-flex">
                                <ul>
                                    <li class="active" style="<%= (mctrlMostPopularBikes.FetchedRecordsCount > 0)?"": "display:none" %>" data-tabs="mctrlMostPopularBikes">Most Popular</li>
                                    <li style="<%= (mctrlNewLaunchedBikes.FetchedRecordsCount > 0)?"": "display:none" %>" data-tabs="mctrlNewLaunchedBikes">New launches</li>
                                    <li style="<%= (mctrlUpcomingBikes.FetchedRecordsCount > 0)?"": "display:none" %>" data-tabs="mctrlUpcomingBikes">Upcoming </li>
                                </ul>
                            </div>
                            <div class="grid-12 alpha omega">
                                <div class="bw-tabs-data features-bikes-container" id="mctrlMostPopularBikes">
                                    <div class="swiper-container card-container">
                                        <div class="swiper-wrapper discover-bike-carousel">
                                            <BW:MMostPopularBikes PageId="4" runat="server" ID="mctrlMostPopularBikes" />
                                        </div>
                                    </div>
                                </div>
                                <div class="bw-tabs-data hide features-bikes-container" id="mctrlNewLaunchedBikes">
                                    <div class="swiper-container card-container">
                                        <div class="swiper-wrapper discover-bike-carousel">
                                            <BW:MNewLaunchedBikes PageId="4" runat="server" ID="mctrlNewLaunchedBikes" />
                                        </div>
                                    </div>
                                </div>
                                <div class="bw-tabs-data hide features-bikes-container" id="mctrlUpcomingBikes">
                                    <div class="swiper-container card-container">
                                        <div class="swiper-wrapper discover-bike-carousel">
                                            <BW:MUpcomingBikes runat="server" ID="mctrlUpcomingBikes" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="clear"></div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section>

            <!--  Compare section code starts here -->
            <BW:CompareBike ID="ctrlCompareBikes" runat="server" />

        </section>

        <% if (Ad320x150_II)
           { %>
        <section>
            <!-- #include file="/ads/Ad320x150_Second.aspx" -->
        </section>
        <% } %>

        <section class="container">
            <!-- Tools you may need code starts here -->
            <div class="grid-12">
                <h2 class="font18 text-center margin-top20 margin-bottom20">Tools you may need</h2>
                <div class="tools-need-container margin-bottom20 text-center">
                    <ul>
                        <li class="bg-white content-inner-block-20 content-box-shadow margin-bottom20">
                            <a href="/m/pricequote/">
                                <span class="tools-need-logo">
                                    <span class="bwm-circle-icon getfinalprice-icon"></span>
                                </span>
                                <span class="tools-need-desc text-left">
                                    <span class="font18 text-bold">Get final price</span>
                                    <br>
                                    <span class="font14 tools-need-text">Get final price without filling any form</span>
                                </span>
                            </a>
                        </li>
                        <li class="bg-white content-inner-block-20 content-box-shadow margin-bottom20">
                            <a href="/m/dealer-showroom-locator/">
                                <span class="tools-need-logo">
                                    <span class="bwm-circle-icon locatedealer-icon"></span>
                                </span>
                                <span class="tools-need-desc text-left">
                                    <span class="font18 text-bold">Locate dealer</span>
                                    <br>
                                    <span class="font14 tools-need-text">Find a dealer near your current location</span>
                                </span>
                            </a>
                        </li>
                        <li class="bg-white content-inner-block-20 content-box-shadow margin-bottom20 hide">
                            <a href="javascript:void(0)">
                                <span class="tools-need-logo">
                                    <span class="bwm-circle-icon checkcarvalue-icon"></span>
                                </span>
                                <span class="tools-need-desc text-left">
                                    <span class="font18 text-bold">Calculate EMI's</span>
                                    <br>
                                    <span class="font14 tools-need-text">Instant calculate loan EMI</span>
                                </span>
                            </a>
                        </li>
                    </ul>
                </div>
            </div>
            <div class="clear"></div>
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
                    <h2 class="font18 text-center margin-top20 margin-bottom20">Latest updates</h2>
                    <div class="bw-tabs-panel">
                        <div class="bw-tabs">
                            <div class="text-center <%= reviewTabsCnt > 2 ? "" : ( reviewTabsCnt > 1 ? "margin-top30 margin-bottom30" : "margin-top10") %>">
                                <div class="bw-tabs <%= reviewTabsCnt > 2 ? "bw-tabs-flex" : ( reviewTabsCnt > 1 ? "home-tabs" : "hide") %>" id="reviewCount">
                                    <ul>
                                        <li class="<%= isNewsActive ? "active" : "hide" %>" style="<%= (Convert.ToInt32(ctrlNews.FetchedRecordsCount) > 0) ? "": "display:none;" %>" data-tabs="ctrlNews">News</li>
                                        <li class="<%= isExpertReviewActive ? "active" : "hide" %>" style="<%= (Convert.ToInt32(ctrlExpertReviews.FetchedRecordsCount) > 0) ? "": "display:none;" %>" data-tabs="ctrlExpertReviews">Expert Reviews</li>
                                        <li class="<%= isVideoActive ? "active" : "hide" %>" style="<%= (Convert.ToInt32(ctrlVideos.FetchedRecordsCount) > 0) ? "": "display:none;" %>" data-tabs="ctrlVideos">Videos</li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                        <div class="grid-12">
                            <%if (!isNewsZero)
                              { %>
                            <BW:News runat="server" ID="ctrlNews" />
                            <% } %>
                            <%if (!isExpertReviewZero)
                              { %>
                            <BW:ExpertReviews runat="server" ID="ctrlExpertReviews" />
                            <% } %>
                            <%if (!isVideoZero)
                              { %>
                            <BW:Videos runat="server" ID="ctrlVideos" />
                            <% } %>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <!--  News, reviews and videos code ends here -->
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <!-- all other js plugins -->
        <!-- #include file="/includes/footerscript_Mobile.aspx" -->
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/bwm-newbikes.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript">
            ga_pg_id = '4';
            if ('<%=isNewsActive%>' == "False") $("#ctrlNews").addClass("hide");
            if ('<%=isExpertReviewActive%>' == "False") $("#ctrlExpertReviews").addClass("hide");
            if ('<%=isVideoActive%>' == "False") $("#ctrlVideos").addClass("hide");    

        
        </script>
    </form>
</body>
</html>
