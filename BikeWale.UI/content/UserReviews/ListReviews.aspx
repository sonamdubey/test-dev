<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Content.ListReviews" Trace="false" %>

<%@ Import Namespace="Bikewale.Common" %>
<%@ Register TagPrefix="BW" TagName="BikeRating" Src="/Controls/BikeRatings.ascx" %>
<%@ Register TagPrefix="BW" TagName="RepeaterPagerReviews" Src="/content/userreviews/RepeaterPagerReviews.ascx" %>
<%@ Register TagPrefix="BP" TagName="InstantBikePrice" Src="/controls/instantbikeprice.ascx" %>
<%@ Register TagPrefix="LD" TagName="LocateDealer" Src="/controls/locatedealer.ascx" %>

<!DOCTYPE html>
<html>

<head>
<%

    title = "User Reviews: " + BikeName;
    description = BikeName + " User Reviews - Read first-hand reviews of actual " + BikeName + " owners. Find out what buyers of " + BikeName + " have to say about the bike.";
    keywords = BikeName + " reviews, " + BikeName + " Users Reviews, " + BikeName + " customer reviews, " + BikeName + " customer feedback, " + BikeName + " owner feedback, user bike reviews, owner feedback, consumer feedback, buyer reviews";
    AdId = "1395986297721";
    AdPath = "/1017752/BikeWale_New_";
    alternate = "https://www.bikewale.com" + "/m/" + MakeMaskingName + "-bikes/" + ModelMaskingName + "/user-reviews/";
    canonical = "https://www.bikewale.com/" + MakeMaskingName + "-bikes/" + ModelMaskingName + "/user-reviews/";
    //modified by SajalGupta for unfilled impression of ads on 04 Aug 2016.
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
<div class="container">
    <div class="grid-12">
        <ul class="breadcrumb">
            <li>You are here: </li>
            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                <a href="/" itemprop="url">
                    <span itemprop="title">Home</span>
                </a>
            </li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                <a href="/<%= MakeMaskingName %>-bikes/" itemprop="url">
                    <span itemprop="title"><%= MakeName%> Bikes</span>
                </a>
            </li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                <a href="/<%=  MakeMaskingName %>-bikes/<%= ModelMaskingName %>/" itemprop="url">
                    <span itemprop="title"> <%= MakeName%> <%= ModelName%></span>
                </a>
            </li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li class="current"><strong>User Reviews</strong></li>
        </ul>
        <div class="clear"></div>
    </div>
    <div class="clear"></div>
    <div class="grid-8  margin-top10">
        <div class="bg-white">
            <div class="content-box-shadow padding-14-20">
                <h1 class="inline-block"><%= BikeName %> User Reviews</h1>
                <div class="rightfloat">
                    <div class="btn btn-teal"><a href="/content/userreviews/writereviews.aspx?bikem=<%= modelId %>">Write a Review</a></div>
                </div>
                <div class="clear"></div>
            </div>
            
            <div class="padding-14-20 content-box-shadow">
                <div class="grid-12 alpha omega">
                    <div class="grid-4 alpha">
                    <%--<img src="<%= Bikewale.Common.MakeModelVersion.GetModelImage(HostUrl, "/bikewaleimg/models/" + LargePic) %>" title="<%= BikeName %>"/>--%>
                    <img src="<%= Bikewale.Common.MakeModelVersion.GetModelImage(HostUrl, OriginalImagePath,Bikewale.Utility.ImageSize._210x118) %>" title="<%= BikeName %>" />
                    <%--<div style="margin-top: 5px; line-height: 17px;">
                        <span class="text-highlight"><%=!IsNew && IsUsed ? "Last Recorded Price Rs.":"Starts at Rs." %> <%= CommonOpn.FormatPrice( ModelStartPrice ) %></span><br />
                        <span class="margin-top5"><a title="<%= MakeName%> <%= ModelName%> details" href='/<%= MakeMaskingName %>-bikes/<%= ModelMaskingName %>/'><%= ModelName %> Details</a><% if (IsNew && IsUsed)                                                                                                                                                                        { %><span class="text-grey"> | </span><a class="getquotation" href="/pricequote/default.aspx?model=<%= modelId %>" pqSourceId="<%= (int)Bikewale.Entities.PriceQuote.PQSourceEnum.Desktop_UserReview_ModelPage %>" data-modelid="<%= modelId %>">On Road Price</a><% } %></span>
                    </div>--%>
                    </div>
                    <div class="grid-8 omega">
                        <h3>Harley Davidson Softtail Classic Heritage</h3>
                        <ul class="bike-review-features margin-top5">
                            <li>163 cc</li>
                            <li>62 kmpl</li>
                            <li>4.50 bhp</li>
                            <li>135 kgs</li>
                        </ul>
                        <p class="margin-top10 text-light-grey font14">Ex-showroom price, Mumbai</p>
                        <div class="margin-top5">  
                            <span class="bwsprite inr-lg"></span>&nbsp;<span class="font18 text-bold">1,26,970</span>
                        </div>
                    </div>
                    <div class="clear"></div>
                </div>

                <div class="grid-12 alpha omega margin-top10">
                    <div class="border-solid ratings">
                        <div class="rating-box overall text-center content-inner-block-15">
                            <div class="text-bold font14">Overall Rating</div>
                            <div>
                                <span class="star-one-icon margin-right5"></span>
                                <span class="font14 text-bold">2.0</span><span class="padding-left2 font12 text-light-grey">/5</span>
                            </div>
                            <div>23 Reviews</div>
                        </div>
                        <div class="content-inner-block-15 inline-block">
                            <div class="rating-box">
                                <div class="rightfloat">
                                    <span class="star-one-icon margin-right5"></span>
                                    <span class="font14 text-bold">2.0</span><span class="padding-left2 font12 text-light-grey">/5</span>
                                </div>
                                <div class="leftfloat">Looks</div>
                                <div class="clear"></div>
                                <%--<div class="rightfloat"><%= CommonOpn.GetRateImage(RatingPerformance) %></div>--%>
                                <div class="rightfloat">
                                    <span class="star-one-icon margin-right5"></span>
                                    <span class="font14 text-bold">1.0</span><span class="padding-left2 font12 text-light-grey">/5</span>
                                </div>
                                <div class="leftfloat">Performance</div>
                                <div class="clear"></div>
                                <div class="rightfloat">
                                    <span class="star-one-icon margin-right5"></span>
                                    <span class="font14 text-bold">3.0</span><span class="padding-left2 font12 text-light-grey">/5</span>
                                </div>
                                <div class="leftfloat">Space/Comfort</div>
                                <div class="clear"></div>
                            </div>
                            <div class="rating-box">
                                 <div class="rightfloat">
                                    <span class="star-one-icon margin-right5"></span>
                                    <span class="font14 text-bold">1.0</span><span class="padding-left2 font12 text-light-grey">/5</span>
                                </div>
                                <div class="leftfloat">Fuel Economy</div>
                                <div class="clear"></div>
                                <div class="rightfloat">
                                    <span class="star-one-icon margin-right5"></span>
                                    <span class="font14 text-bold">1.0</span><span class="padding-left2 font12 text-light-grey">/5</span>
                                </div>
                                <div class="leftfloat">Value For Money</div>
                                <div class="clear"></div>
                             </div>
                        </div>
                        
                    </div>
                   <%-- <div class="grid_3 alpha">
                       <h2 class="margin-bottom5"><%= BikeName %> Ratings</h2>
                    </div>--%>
                    <%--<div class="grid-2 omega margin-top20">
                        <p class="f-small">Own a <span class="price2"><%= BikeName %>?</span> Help others make an informed buying decision.</p>
                    </div>--%>
                </div>
                <div class="clear"></div>
            </div>
        </div>

        <div class="bg-white padding-14-20 margin-top15 content-box-shadow margin-bottom20">
            <h2><%= totalReviewCount %> <%= BikeName %> User Reviews Available</h2>
            <div class="gray-block-top">
            <div class="grey-bg content-block margin-top5 margin-bottom20">
                <div class="leftfloat review-dropdown">
                    <div class="select-box select-box-no-input done size-small">
                        <p class="select-label">Show reviews for </p>
                        <asp:dropdownlist id="drpVersions" runat="server" autopostback="true"  CssClass="chosen-select" data-title="Show reviews for"></asp:dropdownlist>
                    </div>
                </div>
                <div class="rightfloat review-dropdown">
                    
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
            </div>
            <div>
                 <div class="grid-12 alpha omega padding-top20 border-solid-top padding-bottom15">
                    <div class="model-user-review-rating-container leftfloat">
                        <p>4</p>
                        <p class="inline-block margin-bottom5 margin-top5">
                            <span class="star-one-icon"></span><span class="star-one-icon"></span><span class="star-one-icon"></span><span class="star-one-icon"></span><span class="star-zero-icon"></span>
                        </p>
                    </div>
                    <div class="model-user-review-title-container">
                        <a class="article-target-link line-height" title="Packers and Movers Hyderabad" href="/bajaj-bikes/avenger/user-reviews/1042.html">
                           Packers and Movers Hyderabad
                        </a>
                        <div class="article-stats-left-grid">
                            <span class="bwsprite calender-grey-sm-icon"></span>
                            <span class="article-stats-content">Apr 21, 2016</span>
                        </div>
                        <div class="article-stats-right-grid">
                            <span class="bwsprite author-grey-sm-icon"></span>
                            <span class="article-stats-content">Ritu Somani</span>
                        </div>
                        <p class="margin-top12 text-default font14">
                             I cannot truly enable but admire your weblog, your weblog is so adorable and great.http://packersmovershyderabadcity.in/
Packers And Movers Hyderabad Relocation, Main Office in Hyderabad<a href="#">...Read more</a>
                        </p>
                    </div>
                    <div class="clear"></div>
                </div>

                 <div class="grid-12 alpha omega padding-top20 border-solid-top padding-bottom15">
                    <div class="model-user-review-rating-container leftfloat">
                        <p>4</p>
                        <p class="inline-block margin-bottom5 margin-top5">
                            <span class="star-one-icon"></span><span class="star-one-icon"></span><span class="star-one-icon"></span><span class="star-one-icon"></span><span class="star-zero-icon"></span>
                        </p>
                    </div>
                    <div class="model-user-review-title-container">
                        <a class="article-target-link line-height" title="Packers and Movers Hyderabad" href="/bajaj-bikes/avenger/user-reviews/1042.html">
                           Packers and Movers Hyderabad
                        </a>
                        <div class="article-stats-left-grid">
                            <span class="bwsprite calender-grey-sm-icon"></span>
                            <span class="article-stats-content">Apr 21, 2016</span>
                        </div>
                        <div class="article-stats-right-grid">
                            <span class="bwsprite author-grey-sm-icon"></span>
                            <span class="article-stats-content">Ritu Somani</span>
                        </div>
                        <p class="margin-top12 text-default font14">
                             I cannot truly enable but admire your weblog, your weblog is so adorable and great.http://packersmovershyderabadcity.in/
Packers And Movers Hyderabad Relocation, Main Office in Hyderabad<a href="#">...Read more</a>
                        </p>
                    </div>
                    <div class="clear"></div>
                </div>

                <div id="footer-pagination" class="font14 padding-top10">
                    <div class="grid-5 alpha omega text-light-grey">
                        <p>Showing <span class="text-default text-bold">1-10</span> of <span class="text-default text-bold">20</span> articles</p>
                    </div>
				    
                    <div class="clear"></div>
                </div>
            </div>

            <BW:RepeaterPagerReviews ID="rpgReviews" PageSize="10" ShowHeadersVisible="true" PagerPosition="Bottom" runat="server">
                <asp:repeater id="rptReviews" runat="server">			
				    <itemtemplate>
					    <div class="margin-bottom15 margin-top20">	
                            <div class="anchor-title">				
						        <a href='/<%= MakeMaskingName %>-bikes/<%= ModelMaskingName %>/user-reviews/<%# DataBinder.Eval(Container.DataItem, "ReviewId")%>.html'><%# DataBinder.Eval(Container.DataItem, "Title") %></a>
                            </div>						
						    <div class="grid-5 alpha">
							    <%# CommonOpn.GetRateImage(Convert.ToDouble(DataBinder.Eval(Container.DataItem, "OverallR"))) %> by <%# HandleName(DataBinder.Eval(Container.DataItem, "CustomerName").ToString(), DataBinder.Eval(Container.DataItem, "HandleName").ToString())%>, <%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "EntryDateTime")).ToString("dd-MMM-yyyy") %> <a class="f-small hide" href="/forums/viewthread-<%# DataBinder.Eval(Container.DataItem, "ThreadId") %>.html"><%# DataBinder.Eval(Container.DataItem, "Comments") %> Comments</a>
						    </div>
						        <%# DataBinder.Eval(Container.DataItem, "Pros").ToString() == "" ? "" : "<br><strong>Good: </strong>" + DataBinder.Eval(Container.DataItem, "Pros").ToString() %>
						        <%# DataBinder.Eval(Container.DataItem, "Cons").ToString() == "" ? "" : "<br><strong>Bad: </strong>" + DataBinder.Eval(Container.DataItem, "Cons").ToString() %>
						    <div class="margin-top10">
							    <span id="spnComments" runat="server"><%# GetComments(DataBinder.Eval(Container.DataItem, "SubComments").ToString()) %></span>...
							    <a href='/<%= MakeMaskingName %>-bikes/<%= ModelMaskingName %>/user-reviews/<%# DataBinder.Eval(Container.DataItem, "ReviewId")%>.html'>read complete review</a>
						        </div>
					        </div>	
					        <div class="sept-dashed"></div>
				    </itemtemplate>			
			    </asp:repeater>
            </BW:RepeaterPagerReviews>
        </div>
        </div>
    </div>
    <div class="grid-4 margin-top10">
        <div>
            <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
            <!-- #include file="/ads/Ad300x250.aspx" -->
        </div>
        <div class="content-box-shadow padding-15-20-10 margin-bottom20">
        <h2>User reviews for similar bikes</h2>
        <ul class="sidebar-bike-list">
            <li>
                <a href="/honda-bikes/cbshine/" title="Honda CB Shine" class="bike-target-link text-default">
                    <div class="bike-target-image inline-block">
                        <img src="http://imgd1.aeplcdn.com//110x61//bw/models/honda-cb-shine-kick/drum/spokes-111.jpg?20151209184344" alt="Honda CB Shine">
                    </div>
                    <div class="bike-target-content inline-block padding-left10">
                        <h3>Honda CB Shine</h3>
                        <ul class="bike-review-features margin-top5">
                            <li>
                                <div>
                                    <span class="star-one-icon margin-right5"></span>
                                    <span class="font14 text-bold">2.0</span><span class="padding-left2 font12 text-light-grey">/5</span>
                                </div>
                            </li>
                            <li>62 kmpl</li>
                        </ul>
                    </div>
                </a>
            </li>
        
            <li>
                <a href="/bajaj-bikes/pulsarrs200/" title="Bajaj Pulsar RS200" class="bike-target-link text-default">
                    <div class="bike-target-image inline-block">
                        <img src="http://imgd2.aeplcdn.com//110x61//bw/models/bajaj-pulsar-rs200.jpg?20150710124439" alt="Bajaj Pulsar RS200">
                    </div>
                    <div class="bike-target-content inline-block padding-left10">
                        <h3>Bajaj Pulsar RS200</h3>
                         <ul class="bike-review-features margin-top5">
                            <li>
                                <div>
                                    <span class="star-one-icon margin-right5"></span>
                                    <span class="font14 text-bold">2.0</span><span class="padding-left2 font12 text-light-grey">/5</span>
                                </div>
                            </li>
                            <li>62 kmpl</li>
                        </ul>
                    </div>
                </a>
            </li>
        
            <li>
                <a href="/hero-bikes/hfdawn/" title="Hero HF Dawn" class="bike-target-link text-default">
                    <div class="bike-target-image inline-block">
                        <img src="https://imgd3.aeplcdn.com/110x61/bikewaleimg/images/noimage.png" alt="Hero HF Dawn">
                    </div>
                    <div class="bike-target-content inline-block padding-left10">
                        <h3>Hero HF Dawn</h3>
                         <ul class="bike-review-features margin-top5">
                            <li>
                                <div>
                                    <span class="star-one-icon margin-right5"></span>
                                    <span class="font14 text-bold">2.0</span><span class="padding-left2 font12 text-light-grey">/5</span>
                                </div>
                            </li>
                            <li>62 kmpl</li>
                        </ul>
                    </div>
                </a>
            </li>
        
            <li>
                <a href="/bajaj-bikes/discover-125-dts-i/" title="Bajaj Discover 125" class="bike-target-link text-default">
                    <div class="bike-target-image inline-block">
                        <img src="http://imgd1.aeplcdn.com//110x61//bw/models/bajaj-discover-125-drum-838.jpg?20151209174013" alt="Bajaj Discover 125">
                    </div>
                    <div class="bike-target-content inline-block padding-left10">
                        <h3>Bajaj Discover 125</h3>
                         <ul class="bike-review-features margin-top5">
                            <li>
                                <div>
                                    <span class="star-one-icon margin-right5"></span>
                                    <span class="font14 text-bold">2.0</span><span class="padding-left2 font12 text-light-grey">/5</span>
                                </div>
                            </li>
                            <li>62 kmpl</li>
                        </ul>
                    
                    </div>
                </a>
            </li>
        
        </ul>
    
</div>

        <div class="content-box-shadow padding-15-20-10 margin-bottom20 ">
        <h2>Popular  bikes</h2>
        <ul class="sidebar-bike-list">
            <li>
                <a href="/honda-bikes/cbshine/" title="Honda CB Shine" class="bike-target-link text-default">
                    <div class="bike-target-image inline-block">
                        <img src="http://imgd1.aeplcdn.com//110x61//bw/models/honda-cb-shine-kick/drum/spokes-111.jpg?20151209184344" alt="Honda CB Shine">
                    </div>
                    <div class="bike-target-content inline-block padding-left10">
                        <h3>Honda CB Shine</h3>
                        <p class="font11 text-light-grey">Ex-showroom Mumbai</p>
                        <span class="bwsprite inr-md"></span><span class="font16 text-bold">&nbsp;50,615</span>
                    </div>
                </a>
            </li>
        
            <li>
                <a href="/bajaj-bikes/pulsarrs200/" title="Bajaj Pulsar RS200" class="bike-target-link text-default">
                    <div class="bike-target-image inline-block">
                        <img src="http://imgd2.aeplcdn.com//110x61//bw/models/bajaj-pulsar-rs200.jpg?20150710124439" alt="Bajaj Pulsar RS200">
                    </div>
                    <div class="bike-target-content inline-block padding-left10">
                        <h3>Bajaj Pulsar RS200</h3>
                        <p class="font11 text-light-grey">Ex-showroom Mumbai</p>
                        <span class="bwsprite inr-md"></span><span class="font16 text-bold">&nbsp;12,489</span>
                    </div>
                </a>
            </li>
        
            <li>
                <a href="/hero-bikes/hfdawn/" title="Hero HF Dawn" class="bike-target-link text-default">
                    <div class="bike-target-image inline-block">
                        <img src="https://imgd3.aeplcdn.com/110x61/bikewaleimg/images/noimage.png" alt="Hero HF Dawn">
                    </div>
                    <div class="bike-target-content inline-block padding-left10">
                        <h3>Hero HF Dawn</h3>
                        <p class="font11 text-light-grey">Ex-showroom Mumbai</p>
                        <span class="font14">Price Unavailable</span>
                    </div>
                </a>
            </li>
        
            <li>
                <a href="/bajaj-bikes/discover-125-dts-i/" title="Bajaj Discover 125" class="bike-target-link text-default">
                    <div class="bike-target-image inline-block">
                        <img src="http://imgd1.aeplcdn.com//110x61//bw/models/bajaj-discover-125-drum-838.jpg?20151209174013" alt="Bajaj Discover 125">
                    </div>
                    <div class="bike-target-content inline-block padding-left10">
                        <h3>Bajaj Discover 125</h3>
                        <p class="font11 text-light-grey">Ex-showroom Mumbai</p>
                        <span class="bwsprite inr-md"></span><span class="font16 text-bold">&nbsp;54,001</span>
                    
                    </div>
                </a>
            </li>
        
        </ul>
    
</div>
        
        <div>
            <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
            <!-- #include file="/ads/Ad300x250BTF.aspx" -->
        </div>
    </div>

    <div class="clear"></div>
</div>
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
