﻿<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.BikeMakes" %>

<%@ Register Src="~/m/controls/MUpcomingBikes.ascx" TagName="MUpcomingBikes" TagPrefix="BW" %>
<%@ Register Src="/m/controls/NewsWidget.ascx" TagName="News" TagPrefix="BW" %>
<%@ Register Src="/m/controls/ExpertReviewsWidget.ascx" TagName="ExpertReviews" TagPrefix="BW" %>
<%@ Register Src="/m/controls/VideosWidget.ascx" TagName="Videos" TagPrefix="BW" %>
<%@ Register TagPrefix="BW" TagName="MPopupWidget" Src="/m/controls/MPopupWidget.ascx" %>

<!doctype html>
<html>
<head>
    <%
        title = _make.MakeName + " Price in India, Review, Mileage & Photos - Bikewale";
        description = _make.MakeName + " Price in India - Rs." + Bikewale.Utility.Format.FormatPrice(Bikewale.BindViewModels.Controls.BindMakePage.MinPrice.ToString()) +
           " to  Rs." + Bikewale.Utility.Format.FormatPrice(Bikewale.BindViewModels.Controls.BindMakePage.MaxPrice.ToString()) + ". Check out " + _make.MakeName +
           " on road price, reviews, mileage, variants, news & photos at Bikewale.";
        canonical = "http://www.bikewale.com/" + _make.MaskingName + "-bikes/";
        AdPath = "/1017752/Bikewale_Mobile_Make";
        AdId = "1017752";
        Ad_320x50 = true;
        Ad_Bot_320x50 = true;        
        TargetedMakes = _make.MakeName;
    %>
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-brand.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css">
    <link href="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/css/chosen.min.css?<%= staticFileVersion %>" type="text/css"rel="stylesheet" /> 
    <!-- #include file="/includes/headscript_mobile.aspx" -->
</head>
<body class="bg-light-grey">
    <form runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->

        <section>
            <!--  Used Search code starts here -->
            <div class="container">
                <div>
                    <!--  class="grid-12"-->
                    <h2 class="text-center margin-top20 margin-bottom20"><%= _make.MakeName %> Bikes</h2>
                    <div class="search-bike-container">
                        <div class="search-bike-item">
                            <!-- Most Popular Bikes Starts here-->
                            <asp:Repeater ID="rptMostPopularBikes" runat="server">
                                <ItemTemplate>
                                    <div class="front">
                                        <div class="contentWrapper">
                                            <div class="imageWrapper">
                                                <a href='/m<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"objMake.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"objModel.MaskingName"))) %>'>
                                                    <img class="lazy" data-original="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImagePath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._310x174) %>" title="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "objModel.ModelName")) %>" alt="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "objModel.ModelName")) %>" src="http://img1.aeplcdn.com/grey.gif" width="310" height="174">
                                                </a>
                                            </div>
                                            <div class="bikeDescWrapper">
                                                <div class="bikeTitle">
                                                    <h3><a href='/m<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"objMake.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"objModel.MaskingName"))) %>' title="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "objModel.ModelName")) %>"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "objModel.ModelName")) %></a></h3>
                                                </div>
                                                <div class="font22 text-grey margin-bottom5">
                                                    <span class="fa fa-rupee " style="<%# (Convert.ToString(DataBinder.Eval(Container.DataItem, "VersionPrice"))=="0")?"display:none;": "display:inline-block;"%>"></span>
                                                    <span class="font24"><%# ShowEstimatedPrice(DataBinder.Eval(Container.DataItem, "VersionPrice")) %></span>
                                                </div>
                                                <div class="margin-bottom10 font14 text-light-grey">Ex-showroom, <%=ConfigurationManager.AppSettings["defaultName"].ToString() %></div>
                                                <div class="font13 margin-bottom10">
                                                    <%# Bikewale.Utility.FormatMinSpecs.GetMinSpecs(Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.Displacement")),Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.FuelEfficiencyOverall")),Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.MaxPower"))) %>
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
                                                            <span class="font16 text-light-grey"><a href="/<%#DataBinder.Eval(Container.DataItem,"objMake.MaskingName").ToString()%>-bikes/<%#DataBinder.Eval(Container.DataItem,"objModel.MaskingName").ToString() %>/user-reviews/"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "ReviewCount")) %> Reviews</a></span>
                                                        </div>
                                                    </div>
                                                    <div class="clear"></div>
                                            <a href="javascript:void(0)" makeName="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "objMake.MakeName")) %>" modelName="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "objModel.ModelName")) %>"  pagecatid="1" modelId="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "objModel.ModelId")) %>" class="btn btn-sm btn-white margin-top10 fillPopupData">Get on road price</a>                                          
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="border-top1 margin-left20 margin-right20 padding-top20 clear"></div>
                                </ItemTemplate>
                            </asp:Repeater>
                            <!--- Most Popular Bikes Ends Here-->

                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>

        </section>
        <!-- Used Search code  Ends here -->

    <section class="<%= (Convert.ToInt32(ctrlUpcomingBikes.FetchedRecordsCount) > 0) ? "" : "hide" %>" ><!--  Upcoming, New Launches and Top Selling code starts here -->        
    	<div class="container" >
                <div class="grid-12 ">
                    <h2 class="text-center margin-top30 margin-bottom20">Upcoming <%= _make.MakeName %> bikes</h2>
                    <div class="jcarousel-wrapper upComingBikes">
                        <div class="jcarousel">
                            <ul>
                                <BW:MUpcomingBikes runat="server" ID="ctrlUpcomingBikes" />
                            </ul>
                        </div>
                        <span class="jcarousel-control-left"><a href="javascript:void(0)" class="bwmsprite jcarousel-control-prev"></a></span>
                        <span class="jcarousel-control-right"><a href="javascript:void(0)" class="bwmsprite jcarousel-control-next"></a></span>
                        <p class="text-center jcarousel-pagination"></p>
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
                <div class="grid-12">
                    <h2 class="text-center margin-top40 margin-bottom30 padding-left30 padding-right30">Latest Updates from <%= _make.MakeName %></h2>
                    <div class="bw-tabs-panel">
                        <div class="bw-tabs margin-bottom15 <%= reviewTabsCnt == 1 ? "hide" : "" %>">
                            <div class="form-control-box">
                                <select class="form-control">
                                    <option  class=" <%= (Convert.ToInt32(ctrlNews.FetchedRecordsCount) > 0) ? "" : "hide"  %> <%= isNewsActive ? "active" : "" %>" value="ctrlNews">News</option>
                                    <option class="<%= (Convert.ToInt32(ctrlExpertReviews.FetchedRecordsCount) > 0) ? "" : "hide" %> <%= isExpertReviewActive ? "active" : "" %>" value="ctrlExpertReviews">Reviews</option>
                                    <option class="<%= (Convert.ToInt32(ctrlVideos.FetchedRecordsCount) > 0) ? "" : "hide" %> <%= isVideoActive ? "active" : "" %>" value="ctrlVideos">Videos</option>
                                </select>
                            </div>
                        </div>
                        <%if (!isNewsZero) { %>         <BW:News runat="server" ID="ctrlNews" />    <% } %>
                        <%if (!isExpertReviewZero) { %> <BW:ExpertReviews runat="server" ID="ctrlExpertReviews" />  <% } %>                         
                        <%if (!isVideoZero) { %>        <BW:Videos runat="server" ID="ctrlVideos" />    <% } %>                       
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
                        <p>
                            <%= _bikeDesc.SmallDescription %>
                        </p>
                        <p class="margin-top10">
                            <a class="font14" href="javascript:void(0)">Read more</a>
                        </p>
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

        <BW:MPopupWidget runat="server" ID="MPopupWidget1" />
        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <!-- #include file="/includes/footerscript_Mobile.aspx" -->
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/m/src/bwm-brand.js?<%= staticFileVersion %>"></script>
        <script type="text/javascript" src="<%= staticUrl != "" ? "http://st1.aeplcdn.com" + staticUrl : "" %>/m/src/chosen-jquery-min-mobile.js?<%= staticFileVersion %>"></script>
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
            });
            if ('<%=isNewsActive%>' == "False") $("#ctrlNews").addClass("hide");
            if ('<%=isExpertReviewActive%>' == "False") $("#ctrlExpertReviews").addClass("hide");
            if ('<%=isVideoActive%>' == "False") $("#ctrlVideos").addClass("hide");
        </script>
    </form>
</body>
</html>

