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
        TargetedMake = _make.MakeName;
        AdPath = "/1017752/Bikewale_NewBike_";
        AdId = "1442913773076";
        isAd970x90Shown = true;
    %>
    <!-- #include file="/includes/headscript.aspx" -->
    <% isHeaderFix = false; %>
    <link href="<%= staticUrl != "" ? "http://st2.aeplcdn.com" + staticUrl : "" %>/css/brand.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css">
</head>
<body class="bg-light-grey">
    <form runat="server">
        <!-- #include file="/includes/headBW.aspx" -->

        <!-- Brand Page Starts Here-->
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
                    <div class="grid-12 alpha omega">
                        <div class="grid-8 alpha">
                            <h1 class="leftfloat font30 text-black margin-top10 margin-bottom15"><%= _make.MakeName %> bikes</h1>
                        </div>
                        <div class="grid-4 rightfloat margin-top10 omega" id="sortByContainer">
                            <div class="leftfloat sort-by-text margin-left50">
                                <p>Sort by:</p>
                            </div>
                            <div class="rightfloat">
                                <div class="sort-div rounded-corner2">
                                    <div class="sort-by-title" id="sort-by-container">
                                        <span class="leftfloat sort-select-btn">Price: Low to High</span>
                                        <span class="clear"></span>
                                    </div>
                                    <span id="upDownArrow" class="rightfloat fa fa-angle-down position-abt pos-top10 pos-right10"></span>
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
                    </div>
                    <div class="clear"></div>
                </div>
                <div class="clear"></div>
            </div>
        </section>

        <section>
            <div class="container">
                <div class="grid-12">
                    <div class="brand-bikes-list-container content-box-shadow content-inner-block-10 rounded-corner2">
                        <ul id="listitems" class="listitems">
                            <!-- Most Popular Bikes Starts here-->
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
                                                <div class="font20">
                                                    <%# ShowEstimatedPrice(DataBinder.Eval(Container.DataItem, "VersionPrice")) %>
                                                </div>
                                                <div class="font12 text-light-grey margin-bottom10">Ex-showroom, <%=ConfigurationManager.AppSettings["defaultName"].ToString() %></div>
                                                <div class="font14 margin-bottom10">
                                                    <%# Bikewale.Utility.FormatMinSpecs.GetMinSpecs(Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.Displacement")),Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.FuelEfficiencyOverall")),Convert.ToString(DataBinder.Eval(Container.DataItem, "Specs.MaxPower"))) %>
                                                </div>
                                                <div class="leftfloat">
                                                    <p class=" inline-block rating-stars-container border-solid-right padding-right10 <%# Convert.ToString(DataBinder.Eval(Container.DataItem,"ReviewCount")) != "0" ? "" : "hide" %>">
                                                        <%# Bikewale.Utility.ReviewsRating.GetRateImage(Convert.ToDouble(DataBinder.Eval(Container.DataItem,"ModelRating"))) %>
                                                    </p>
                                                </div>
                                                <div class="leftfloat rated-container margin-left10 font16 text-light-grey <%# Convert.ToString(DataBinder.Eval(Container.DataItem,"ReviewCount")) != "0" ? "" : "hide" %>">
                                                    <span><a href="/<%# Convert.ToString(DataBinder.Eval(Container.DataItem,"objMake.MaskingName"))%>-bikes/<%#Convert.ToString(DataBinder.Eval(Container.DataItem,"objModel.MaskingName")) %>/user-reviews/"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "ReviewCount")) %> Reviews</a></span>
                                                </div>

                                                <div class="leftfloat not-rated-container font16 text-light-grey <%# Convert.ToString(DataBinder.Eval(Container.DataItem,"ReviewCount")) == "0" ? "" : "hide" %>">
                                                    <span class="border-solid-right padding-right10">Not rated yet  </span><a href="/content/userreviews/writereviews.aspx?bikem=<%# DataBinder.Eval(Container.DataItem,"objModel.ModelId") %>"><span class="margin-left10">Write a review</span></a>
                                                </div>

                                                <div class="clear"></div>
                                                <a href="Javascript:void(0)" pagecatid="1" makename="<%# DataBinder.Eval(Container.DataItem,"objMake.MakeName").ToString() %>" modelname="<%# DataBinder.Eval(Container.DataItem,"objModel.ModelName").ToString() %>" modelid="<%# DataBinder.Eval(Container.DataItem, "objModel.ModelId").ToString() %>" class="btn btn-grey margin-top10 fillPopupData">Get on road price</a>
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
        <section class="<%= (ctrlUpcomingBikes.FetchedRecordsCount > 0) ? "" : "hide" %>">
            <!-- Upcoming bikes from brands -->
            <div class="container">
                <div class="grid-12">
                    <h2 class="text-bold text-center margin-top50 margin-bottom30">Upcoming bikes from <%= _make.MakeName %></h2>
                    <div class="content-box-shadow rounded-corner2">
                        <div class="jcarousel-wrapper upcoming-brand-bikes-container margin-top20">
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
        <section>
            <!--  News Bikes code starts here -->
            <div class="container newBikes-latest-updates-container <%= reviewTabsCnt == 0 ? "hide" : string.Empty %>">
                <div class="grid-12 margin-bottom20">
                    <h2 class="text-bold text-center margin-top50 margin-bottom30">Latest updates from the industry</h2>
                    <div class="bw-tabs-panel content-box-shadow margin-bottom30">
                        <div class="text-center <%= reviewTabsCnt > 2 ? "" : ( reviewTabsCnt > 1 ? "margin-top30 margin-bottom30" : "margin-top10") %>">
                            <div class="bw-tabs <%= reviewTabsCnt > 2 ? "bw-tabs-flex" : ( reviewTabsCnt > 1 ? "home-tabs" : "hide") %>">
                                <ul>
                                    <li class="<%= isNewsActive ? "active" : "hide" %>" style="<%= (Convert.ToInt32(ctrlNews.FetchedRecordsCount) > 0) ? "": "display:none;" %>" data-tabs="ctrlNews">News</li>
                                    <li class="<%= isExpertReviewActive ? "active" : "hide" %>" style="<%= (Convert.ToInt32(ctrlExpertReviews.FetchedRecordsCount) > 0) ? "": "display:none;" %>" data-tabs="ctrlExpertReviews">Expert Reviews</li>
                                    <li class="<%= isVideoActive ? "active" : "hide" %>" style="<%= (Convert.ToInt32(ctrlVideos.FetchedRecordsCount) > 0) ? "": "display:none;" %>" data-tabs="ctrlVideos">Videos</li>
                                </ul>
                            </div>
                        </div>
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
                <div class="clear"></div>
            </div>
        </section>
        <section class="<%= (isDescription)? "": "hide" %>">
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
        <script>
            $("a.read-more-btn").click(function () {
                $("span.brand-about-main").toggleClass('hide');
                $("span.brand-about-more-desc").toggleClass('hide');
                var a = $(this).find("span");
                a.text(a.text() === "more" ? "less" : "more");
            });

            $(document).ready(function () { $("img.lazy").lazyload(); });
            $(".upcoming-brand-bikes-container").on('jcarousel:visiblein', 'li', function (event, carousel) {
                $(this).find("img.lazy").trigger("imgLazyLoad");
            });
            if ('<%=isNewsActive%>' == "False") $("#ctrlNews").addClass("hide");
            if ('<%=isExpertReviewActive%>' == "False") $("#ctrlExpertReviews").addClass("hide");
            if ('<%=isVideoActive%>' == "False") $("#ctrlVideos").addClass("hide");

        </script>
        <!-- #include file="/includes/footerBW.aspx" -->
        <!-- #include file="/includes/footerscript.aspx" -->
        <script type="text/javascript" src="../src/new/bikemake.js?<%= staticFileVersion %>"></script>
    </form>
    <script type="text/javascript">
        ga_pg_id = '3';
        var _makeName = '<%= _make.MakeName %>';
    </script>
</body>
</html>
