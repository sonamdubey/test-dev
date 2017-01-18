<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Content.ListReviews" Trace="false" %>

<%@ Import Namespace="Bikewale.Common" %>
<%@ Register TagPrefix="BW" TagName="BikeRating" Src="/Controls/BikeRatings.ascx" %>
<%@ Register TagPrefix="BW" TagName="RepeaterPagerReviews" Src="/content/userreviews/RepeaterPagerReviews.ascx" %>
<%@ Register TagPrefix="BP" TagName="InstantBikePrice" Src="/controls/instantbikeprice.ascx" %>
<%@ Register TagPrefix="LD" TagName="LocateDealer" Src="/controls/locatedealer.ascx" %>
<%@ Register TagPrefix="BW" TagName="MostPopularBikesMin" Src="~/controls/MostPopularBikesMin.ascx" %>
<%@ Register TagPrefix="BW" TagName="UserReviewSimilarBike" Src="~/controls/UserReviewSimilarBike.ascx" %>
<!DOCTYPE html>
<html>

<head>
<%
   
    title = pageMetas.Title;
    description = pageMetas.Description;
    keywords =pageMetas.Keywords;
    AdId = "1395986297721";
    AdPath = "/1017752/BikeWale_New_";
    alternate = pageMetas.AlternateUrl;
    canonical = pageMetas.CanonicalUrl;
    isAd300x250Shown = true;
    %>
    
    <!-- #include file="/includes/headscript_desktop_min.aspx" -->

    <link rel="stylesheet" type="text/css" href="/css/user-review/listing.css" />
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_desktop.aspx" -->
    </script>
    <script type="text/javascript" src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : "" %>/src/frameworks.js?<%=staticFileVersion %>"></script>
</head>

<body class="header-fixed-inner">
<form runat="server">
    <!-- #include file="/includes/headBW.aspx" -->
    <section class="bg-light-grey padding-top10" id="breadcrumb">
        <div class="container">
            <div class="grid-12">
                <div class="breadcrumb margin-bottom15">
                    <!-- breadcrumb code starts here -->
                    <ul>
                        <li itemscope itemtype="http://data-vocabulary.org/Breadcrumb"><a href="/" itemprop="url">
                            <span itemprop="title">Home</span></a>
                        </li>
                        <li itemscope itemtype="http://data-vocabulary.org/Breadcrumb">
                            <span class="bwsprite fa-angle-right margin-right10"></span>
                            <a href="/<%= MakeMaskingName %>-bikes/" itemprop="url">
                                <span itemprop="title"><%= MakeName%> Bikes</span>
                            </a>
                        </li>
                        <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                            <span class="bwsprite fa-angle-right margin-right10"></span>
                            <a href="/<%=  MakeMaskingName %>-bikes/<%= ModelMaskingName %>/" itemprop="url">
                                <span itemprop="title"> <%= MakeName%> <%= ModelName%></span>
                            </a>
                        </li>
                        <li>
                            <span class="bwsprite fa-angle-right margin-right10"></span>
                            <span>User Reviews</span>
                        </li>
                    </ul>
                    <div class="clear"></div>
                </div>
            </div>
            <div class="clear"></div>
        </div>
    </section>

    <section>
        <div class="container">
            <div class="grid-12">
            <div class="grid-8 alpha">
                <div class="content-box-shadow bg-white margin-bottom20">
                    <div class="content-box-shadow padding-14-20">
                        <div class="grid-9 alpha inline-block float-none">
                            <h1><%= BikeName %> User reviews</h1>
                        </div><div class="grid-3 text-right alpha omega inline-block float-none">
                            <a href="/content/userreviews/writereviews.aspx?bikem=<%= modelId %>" class="btn btn-teal btn-size-150">Write a review</a>
                        </div>
                        <div class="clear"></div>
                    </div>
            
                    <div class="content-inner-block-20">
                        <div class="grid-3 alpha">
                            <a href="/<%=  MakeMaskingName %>-bikes/<%= ModelMaskingName %>/" title="<%= BikeName %>">
                                <img src="<%= Bikewale.Common.MakeModelVersion.GetModelImage(HostUrl, OriginalImagePath,Bikewale.Utility.ImageSize._144x81) %>" title="<%= BikeName %>" />
                            </a>
                        </div>
                        <div class="grid-9 omega">
                            <h2 class="font14 text-default"><a href="/<%=  MakeMaskingName %>-bikes/<%= ModelMaskingName %>/" title="<%= BikeName %>" class="text-default"><%= BikeName %></a></h2>
                            <ul class="bike-review-features margin-top5">
                                <li><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(Convert.ToString(Math.Round(displacement,2))) %> cc</li>
                                <li><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(Convert.ToString(Math.Round(fuelEfficency,2))) %> kmpl</li>
                                <li><%=Bikewale.Utility.FormatMinSpecs.ShowAvailable(Convert.ToString(Math.Round(maxPower,2)))%> bhp</li>
                                <li><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(Convert.ToString(Math.Round(kerbWeight,2)) )%> kgs</li>
                            </ul>
                            <p class="margin-top10 text-light-grey font14">Ex-showroom price, <%=Bikewale.Utility.BWConfiguration.Instance.DefaultName%></p>
                            <div class="margin-top5">  
                                <span class="bwsprite inr-lg"></span>&nbsp;<span class="font18 text-bold"><%=Bikewale.Utility.Format.FormatPrice(ModelStartPrice) %></span>
                            </div>
                        </div>
                        <div class="clear"></div>

                        <div class="border-solid ratings margin-top15 display-table">
                            <div class="rating-box overall text-center content-inner-block-15">
                                <p class="text-bold font14 margin-bottom10">Overall Rating</p>
                                <div class="margin-bottom10">
                                    <span class="star-one-icon"></span>
                                    <div class="inline-block">
                                        <span class="font20 text-bold"><%=Math.Round(RatingOverall,1) %></span>
                                        <span class="padding-left2 font12 text-light-grey">/ 5</span>
                                    </div>
                                </div>
                                <p class="font14 text-light-grey"><%=ModelReviewCount%> Reviews</p>
                            </div>
                            <div class="rating-category-list-container content-inner-block-10 star-icon-sm">
                                <ul class="rating-category-list">
                                    <li>
                                        <span class="rating-category-label">Looks</span>
                                        <span>
                                            <span class="star-one-icon"></span>
                                            <span class="text-bold"><%=Math.Round(RatingLooks,1) %></span>
                                            <span class="font12"> / 5</span>
                                        </span>
                                    </li>
                                    <li>
                                        <span class="rating-category-label">Fuel Economy</span>
                                        <span>
                                            <span class="star-one-icon"></span>
                                            <span class="text-bold"><%=Math.Round(RatingFuelEconomy,1) %></span>
                                            <span class="font12"> / 5</span>
                                        </span>
                                    </li>
                                    <li>
                                        <span class="rating-category-label">Performance</span>
                                        <span>
                                            <span class="star-one-icon"></span>
                                            <span class="text-bold"><%=Math.Round(RatingPerformance,1)%></span>
                                            <span class="font12"> / 5</span>
                                        </span>
                                    </li>
                                    <li>
                                        <span class="rating-category-label">Value for Money</span>
                                        <span>
                                            <span class="star-one-icon"></span>
                                            <span class="text-bold"><%=Math.Round(RatingValueForMoney,1) %></span>
                                            <span class="font12"> / 5</span>
                                        </span>
                                    </li>
                                    <li>
                                        <span class="rating-category-label">Style/Comfort</span>
                                        <span>
                                            <span class="star-one-icon"></span>
                                            <span class="text-bold"><%=Math.Round(RatingComfort,1) %></span>
                                            <span class="font12"> / 5</span>
                                        </span>
                                    </li>
                                </ul>
                                <div class="clear"></div>
                            </div>
                        
                        </div>
                        <div class="clear"></div>
                    </div>
                </div>

                <div class="bg-white padding-18-20 content-box-shadow margin-bottom20">
                    <h2 class="font18 margin-bottom10"><%= totalReviewCount %> <%= BikeName %> User reviews</h2>
                 <div>
                         <%if(totalReviewCount>1){ %>  <div class="leftfloat review-dropdown margin-right20">
                            <div class="select-box select-box-no-input done size-small">
                                <p class="select-label">Show reviews for </p>
                                <asp:dropdownlist id="drpVersions" runat="server" autopostback="true"  CssClass="chosen-select" data-title="Show reviews for"></asp:dropdownlist>
                            </div>
                        </div>
                        <div class="leftfloat review-dropdown">
                            <div class="select-box select-box-no-input done size-small">
                                <p class="select-label">Sort by </p>
                                <asp:dropdownlist id="drpSort" runat="server" autopostback="true" CssClass="chosen-select" data-title="Sort by">
					                <asp:ListItem Selected="true" Text="Most Helpful" Value="1" />
					                <asp:ListItem Text="Most Read" Value="2" />
					                <asp:ListItem Text="Most Recent" Value="3" />
					                <asp:ListItem Text="Most Rated" Value="4" />
				                </asp:dropdownlist>
                            </div>
                        </div>
                        <div class="clear"></div>
                      <%} %>
                    </div>
                   
                    <ul>
                        <BW:RepeaterPagerReviews ID="rpgReviews" PageSize="10" ShowHeadersVisible="true" PagerPosition="Bottom" runat="server">
                        <asp:repeater id="rptReviews" runat="server">			
				            <itemtemplate>
                         <li class="padding-top15 border-solid-top padding-bottom15">
                            <div class="model-user-review-rating-container leftfloat">
                                <p class="font16 text-bold"><%# DataBinder.Eval(Container.DataItem, "OverallR") %></p>
                                <p class="margin-top5">
                                    <%# Bikewale.Utility.ReviewsRating.GetRateImage( Convert.ToDouble( DataBinder.Eval(Container.DataItem, "OverallR") ) ) %>
                                </p>
                            </div>
                            <div class="model-user-review-title-container">
                                <h3><a class="article-target-link line-height" title="<%#DataBinder.Eval(Container.DataItem, "Title") %>" href="/<%= MakeMaskingName %>-bikes/<%= ModelMaskingName %>/user-reviews/<%# DataBinder.Eval(Container.DataItem, "ReviewId")%>.html"><%# DataBinder.Eval(Container.DataItem, "Title").ToString() %></a></h3>
                                <div class="article-stats-left-grid">
                                    <span class="bwsprite calender-grey-sm-icon"></span>
                                    <span class="article-stats-content"><%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "EntryDateTime")).ToString("dd-MMM-yyyy") %></span>
                                </div>
                                <div class="article-stats-right-grid">
                                    <span class="bwsprite author-grey-sm-icon"></span>
                                    <span class="article-stats-content"><%# DataBinder.Eval(Container.DataItem, "CustomerName").ToString() %></span>
                                </div>
                                <p class="margin-top12 text-default font14">
                                    <%# Bikewale.Utility.FormatDescription.TruncateDescription(DataBinder.Eval(Container.DataItem, "subcomments").ToString()) %><a href="/<%= MakeMaskingName %>-bikes/<%= ModelMaskingName %>/user-reviews/<%# DataBinder.Eval(Container.DataItem, "ReviewId")%>.html">...Read more</a>
                                </p>
                            </div>
                            <div class="clear"></div>
                        </li>  
                                   </itemtemplate>			
			            </asp:repeater>
                    </BW:RepeaterPagerReviews>         
                    </ul>    
                </div>
            </div>
            <div class="grid-4 omega">
                 <%if(ctrlUserReviewSimilarBike.FetchCount>0){ %>
                 <div class="content-box-shadow padding-15-20-10 margin-bottom20">
                <h2>User reviews of similar bikes</h2>
                <BW:UserReviewSimilarBike ID="ctrlUserReviewSimilarBike" runat="server" />
    
                </div>
                  <%} %>
                <div>
                    <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
                    <!-- #include file="/ads/Ad300x250.aspx" -->
                </div>
                  <%if (ctrlPopularBikes.FetchedRecordsCount>0)
                          { %>
                  <BW:MostPopularBikesMin ID="ctrlPopularBikes" runat="server" />
                <%} %>
                <div>
                    <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
                    <!-- #include file="/ads/Ad300x250BTF.aspx" -->
                </div>
            </div>
            <div class="clear"></div>
            </div>
            <div class="clear"></div>
        </div>
    </section>

    <!-- #include file="/includes/footerBW.aspx" -->
    <link href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
     <!-- #include file="/includes/footerscript.aspx" -->
    <script>
        $('.chosen-select').chosen();
        // version dropdown
        var selectDropdownBox = $('.select-box-no-input');

        selectDropdownBox.each(function () {
            var text = $(this).find('.chosen-select').attr('data-title'),
                searchBox = $(this).find('.chosen-search')

            searchBox.empty().append('<p class="no-input-label">' + text + '</p>');
        });
    </script>
     <!-- #include file="/includes/fontBW.aspx" -->
</form>
</body>
</html>
