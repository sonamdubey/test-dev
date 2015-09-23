<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.New.Model" %>

<%@ Register Src="~/controls/News_new.ascx" TagName="News" TagPrefix="BW" %>
<%@ Register Src="~/controls/ExpertReviews.ascx" TagName="ExpertReviews" TagPrefix="BW" %>
<%@ Register Src="~/controls/VideosControl.ascx" TagName="Videos" TagPrefix="BW" %>
<%@ Register Src="~/controls/UpcomingBikes_new.ascx" TagName="UpcomingBikes" TagPrefix="BW" %>
<%@ Register Src="~/controls/MostPopularBikes_new.ascx" TagName="MostPopularBikes" TagPrefix="BW" %>
<%@ Register TagPrefix="PW" TagName="PopupWidget" Src="/controls/PopupWidget.ascx" %>

<!Doctype html>
<html>
<head>
    <%
        title = _make.MakeName + " Price in India, Review, Mileage & Photos - Bikewale";
        description = _make.MakeName + " Price in India - Rs." + Bikewale.Utility.Format.FormatPrice(_minModelPrice.ToString()) + " - Rs." + Bikewale.Utility.Format.FormatPrice(_maxModelPrice.ToString()) + ". Check out " + _make.MakeName + " on road price, reviews, mileage, variants, news & photos at Bikewale.";
        alternate = "http://www.bikewale.com/m/" + _make.MaskingName + "-bikes/";
        canonical = "http://www.bikewale.com/" + _make.MaskingName + "-bikes/";
    %>
    <!-- #include file="/includes/headscript.aspx" -->
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/brand.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css">
</head>
<body class="bg-light-grey">
    <form runat="server">
        <!-- #include file="/includes/headBW.aspx" -->

        <!-- Brand Page Starts Here-->
        <section class="bg-white header-fixed-inner">
            <div class="container">
                <div class="grid-12">
                    <div class="padding-bottom15 text-center">
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section class="bg-light-grey padding-top10">
            <div class="container">
                <div class="grid-12">
                    <div class="breadcrumb margin-bottom15">
                        <!-- breadcrumb code starts here -->
                        <ul>
                            <li><a href="/">Home</a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span><%= _make.MakeName %> Bikes</li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                    <h1 class="font30 text-black margin-top10"><%= _make.MakeName %> bikes</h1>
                    <div class="border-solid-bottom margin-top10 margin-bottom15"></div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section>
            <div class="container">
                <div class="grid-12">
                    <div class="brand-bikes-list-container content-box-shadow content-inner-block-10 rounded-corner2">
                        <ul>
                            <!-- Most Popular Bikes Starts here-->
                            <asp:Repeater ID="rptMostPopularBikes" runat="server">
                                <ItemTemplate>
                                    <li class="front">
                                        <div class="contentWrapper">
                                            <div class="imageWrapper">
                                                <a href='<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"objMake.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"objModel.MaskingName"))) %>'>
                                                    <img class="lazy" data-original="<%# Bikewale.Utility.Image.GetPathToShowImages(DataBinder.Eval(Container.DataItem, "OriginalImagePath").ToString(),DataBinder.Eval(Container.DataItem, "HostUrl").ToString(),Bikewale.Utility.ImageSize._310x174) %>" title="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "objModel.ModelName")) %>" alt="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "objModel.ModelName")) %>" src="http://img1.aeplcdn.com/grey.gif" width="310" height="174">
                                                </a>
                                            </div>
                                            <div class="bikeDescWrapper">
                                                <div class="bikeTitle margin-bottom10">
                                                    <h3><a href='<%# Bikewale.Utility.UrlFormatter.BikePageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem,"objMake.MaskingName")),Convert.ToString(DataBinder.Eval(Container.DataItem,"objModel.MaskingName"))) %>' title="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "objModel.ModelName")) %>"><%# DataBinder.Eval(Container.DataItem, "objModel.ModelName").ToString() %></a></h3>
                                                </div>
                                                <div class="font20">
                                                    <span class="fa fa-rupee " style="display: <%# (Convert.ToString(DataBinder.Eval(Container.DataItem, "VersionPrice"))=="0")?"none":"inline-block"%>"></span>
                                                    <span class="font22"><%# ShowEstimatedPrice(DataBinder.Eval(Container.DataItem, "VersionPrice")) %></span>
                                                </div>
                                                <div class="font12 text-light-grey margin-bottom10">Ex-showroom, <%=ConfigurationManager.AppSettings["defaultName"].ToString() %></div>
                                                <div class="font14 margin-bottom10">
                                                    <%# Bikewale.Utility.FormatMinSpecs.GetMinSpecs(Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.Displacement")),Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.FuelEfficiencyOverall")),Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.MaxPower"))) %>
                                                </div>
                                                <div class="leftfloat">
                                                    <p class=" inline-block border-solid-right padding-right10 <%# Convert.ToString(DataBinder.Eval(Container.DataItem,"ReviewCount")) != "0" ? "" : "hide" %>">
                                                        <%# Bikewale.Utility.ReviewsRating.GetRateImage(Convert.ToDouble(DataBinder.Eval(Container.DataItem,"ModelRating"))) %>
                                                    </p>
                                                </div>
                                                <div class="leftfloat margin-left10 font16 text-light-grey <%# Convert.ToString(DataBinder.Eval(Container.DataItem,"ReviewCount")) != "0" ? "" : "hide" %>">
                                                    <span><a href="/<%# Convert.ToString(DataBinder.Eval(Container.DataItem,"objMake.MaskingName"))%>-bikes/<%#Convert.ToString(DataBinder.Eval(Container.DataItem,"objModel.MaskingName")) %>/user-reviews/"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "ReviewCount")) %> Reviews</a></span>
                                                </div>

                                                <div class="leftfloat font16 text-light-grey <%# Convert.ToString(DataBinder.Eval(Container.DataItem,"ReviewCount")) == "0" ? "" : "hide" %>">
                                                    <span class="border-solid-right">Not rated yet  </span><a href="/content/userreviews/writereviews.aspx?bikem=<%# DataBinder.Eval(Container.DataItem,"objModel.ModelId") %>"><span class="margin-left10">Write a review</span></a>
                                                </div>

                                                <div class="clear"></div>
                                                <a href="Javascript:void(0)" pageid="1" modelid="<%# DataBinder.Eval(Container.DataItem, "objModel.ModelId").ToString() %>" class="btn btn-grey margin-top10 fillPopupData">Get on road price</a>
                                            </div>
                                        </div>
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                            <!--- Most Popular Bikes Ends Here-->
                        </ul>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <section class="<%= (Convert.ToInt32(ctrlUpcomingBikes.FetchedRecordsCount) > 0) ? "" : "hide" %>">
            <!-- Upcoming bikes from brands -->
            <div class="container">
                <div class="grid-12">
                    <h2 class="text-bold text-center margin-top50 margin-bottom30">Upcoming bikes from <%= _make.MakeName %></h2>
                    <div class="content-box-shadow padding-top20 rounded-corner2">
                        <div class="jcarousel-wrapper upcoming-brand-bikes-container">
                            <div class="jcarousel">
                                <ul>
                                    <BW:UpcomingBikes runat="server" ID="ctrlUpcomingBikes" />
                                    <!-- Upcoming Bikes Control-->
                                </ul>
                            </div>
                            <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev"></a></span>
                            <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next"></a></span>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <% 
            if (ctrlNews.FetchedRecordsCount > 0) { reviewTabsCnt++; }
            if (ctrlExpertReviews.FetchedRecordsCount > 0) { reviewTabsCnt++; }
            if (ctrlVideos.FetchedRecordsCount > 0) { reviewTabsCnt++; }            
        %>
        <section>
            <!--  News Bikes code starts here -->
            <div class="container newBikes-latest-updates-container">
                <div class="grid-12 margin-bottom20">
                    <h2 class="text-bold text-center margin-top50 margin-bottom30">Latest updates from the industry</h2>
                    <div class="bw-tabs-panel content-box-shadow margin-bottom30">
                        <div class="text-center <%= reviewTabsCnt > 2 ? "" : ( reviewTabsCnt > 1 ? "margin-top30 margin-bottom30" : "margin-top10") %>">
                            <div class="bw-tabs <%= reviewTabsCnt > 2 ? "bw-tabs-flex" : ( reviewTabsCnt > 1 ? "home-tabs" : "hide") %>">
                                <ul>
                                    <li class="active" style="<%= (Convert.ToInt32(ctrlNews.FetchedRecordsCount) > 0) ? "": "display:none;" %>" data-tabs="ctrlNews">News</li>
                                    <li style="<%= (Convert.ToInt32(ctrlExpertReviews.FetchedRecordsCount) > 0) ? "": "display:none;" %>" data-tabs="ctrlExpertReviews">Reviews</li>
                                    <li style="<%= (Convert.ToInt32(ctrlVideos.FetchedRecordsCount) > 0) ? "": "display:none;" %>" data-tabs="ctrlVideos">Videos</li>
                                </ul>
                            </div>
                        </div>
                        <BW:News runat="server" ID="ctrlNews" />
                        <BW:ExpertReviews runat="server" ID="ctrlExpertReviews" />
                        <BW:Videos runat="server" ID="ctrlVideos" />
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
        <section>
            <!-- About Brand code starts here-->
            <div class="container">
                <div class="grid-12" style="<%= (isDescription) ? "": "display:none;" %>">
                    <h2 class="text-bold text-center margin-top30 margin-bottom30">About <%= _make.MakeName %></h2>
                    <div class="content-box-shadow content-inner-block-10 rounded-corner2 margin-bottom30 font14">
                        <span class="brand-about-main">
                            <%= _bikeDesc.SmallDescription %>
                        </span>
                        <span class="brand-about-more-desc hide">
                            <%= _bikeDesc.FullDescription %>
                        </span>
                        <span><a href="javascript:void(0)" class="read-more-btn">Read <span>more</span></a></span>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section>
            <!-- Popup Section goes here-->
            <PW:PopupWidget runat="server" ID="PopupWidget" />
            <!-- Popup Section Ends here-->
        </section>

        <script type="text/javascript">
            $(document).ready(function () { $("img.lazy").lazyload(); });
        </script>
        <!-- #include file="/includes/footerBW.aspx" -->
        <!-- #include file="/includes/footerscript.aspx" -->
    </form>
</body>
</html>
