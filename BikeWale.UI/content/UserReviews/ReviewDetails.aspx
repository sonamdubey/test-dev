<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Content.ReviewDetails" Trace="false" %>

<%@ Import Namespace="Bikewale.Common" %>
<%@ Register TagPrefix="BikeWale" TagName="DiscussIt" Src="/Controls/DiscussIt.ascx" %>
<%@ Register TagPrefix="BP" TagName="InstantBikePrice" Src="/controls/instantbikeprice.ascx" %>
<%@ Register TagPrefix="LD" TagName="LocateDealer" Src="/controls/locatedealer.ascx" %>

<!DOCTYPE html>
<html>

<head>

<%
    title = _title + " - A Review on " + BikeMake + " " + BikeModel + " by " + reviewerName;
    description = BikeMake + " User Review - " + "A review/feedback on " + BikeName + " by " + reviewerName + ". Find out what " + reviewerName + " has to say about " + BikeName + ".";
    keywords = BikeMake + " " + BikeModel + " review, " + BikeMake + " " + BikeModel + " user review, car review, owner feedback, consumer review";
    AdId = "1395986297721";
    AdPath = "/1017752/BikeWale_New_";
    alternate = "https://www.bikewale.com/m/" + MakeMaskingName + "-bikes/" + ModelMaskingName + "/user-reviews/" + reviewId + ".html";
    canonical = "https://www.bikewale.com/" + MakeMaskingName + "-bikes/" + ModelMaskingName + "/user-reviews/" + reviewId + ".html";
    //modified by SajalGupta for unfilled impression of ads on 04 Aug 2016.
    isAd300x250Shown = false;
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
                <a href="/<%= MakeMaskingName%>-bikes/" itemprop="url">
                    <span itemprop="title"><%= BikeMake%> Bikes</span>
                </a>
            </li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                <a href="/<%= MakeMaskingName%>-bikes/<%= ModelMaskingName%>/" itemprop="url">
                    <span itemprop="title"><%= BikeMake%> <%= BikeModel%></span>
                </a>
            </li>
            <li class="fwd-arrow">&rsaquo;</li>
            <li itemscope="" itemtype="http://data-vocabulary.org/Breadcrumb">
                <a href="/<%= MakeMaskingName%>-bikes/<%= ModelMaskingName%>/user-reviews/" itemprop="url">
                    <span itemprop="title">User Reviews</span>
                </a>
            </li>
             <li class="fwd-arrow">&rsaquo;</li>
            <li class="current"><strong><%= _title %></strong></li>
        </ul>
        <div class="clear"></div>
    </div>
    <div class="clear"></div>
    <div class="grid-12">
        <div class="grid-8 margin-top10 alpha">
            <div class="content-box-shadow bg-white margin-bottom20">
                <div class="content-box-shadow padding-14-20">
                <div class="grid-9 alpha inline-block float-none">
                    <h1 class="text-black"><%= _title %></h1>
                    <div>
                        <div class="article-stats-left-grid">
                        <span class="bwsprite calender-grey-sm-icon"></span>
                        <span class="article-stats-content"><%= entryDate %></span>
                    </div>
                    <div class="article-stats-right-grid">
                        <span class="bwsprite author-grey-sm-icon"></span>
                        <span class="article-stats-content">
                            <%if (handleName != "")
                              {%>
                                <%=handleName%>
                                <%}
                              else
                              {%>
                                <%=reviewerName%>
                            <%}%>
                        </span>
                    </div>
                    </div>
                </div><div class="grid-3 text-right alpha omega inline-block float-none">
                    <a href="/content/userreviews/writereviews.aspx?bikem=<%= ModelId %>" class="btn btn-teal btn-size-150">Write a review</a>
                </div>
                <div class="clear"></div>
            </div>

                <div class="content-inner-block-20">
                        <div class="grid-3 alpha">
                            <img src="<%= Bikewale.Common.MakeModelVersion.GetModelImage(HostUrl,OriginalImagePath,Bikewale.Utility.ImageSize._144x81) %>" title="<%=BikeMake%> <%=BikeModel%>" />
                        </div>
                        <div class="grid-9 omega">
                            <h2 class="font14 text-default">Bajaj Avenger</h2>
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

                        <div class="border-solid ratings margin-top15">
                            <div class="rating-box overall text-center content-inner-block-15">
                                <p class="text-bold font14 margin-bottom10">Overall Rating</p>
                                <div class="margin-bottom10">
                                    <span class="star-one-icon"></span>
                                    <div class="inline-block">
                                        <span class="font20 text-bold">2.0</span>
                                        <span class="padding-left2 font12 text-light-grey">/ 5</span>
                                    </div>
                                </div>
                                <p class="font14 text-light-grey">23 Reviews</p>
                            </div>
                            <div class="rating-category-list-container content-inner-block-10 inline-block star-icon-sm">
                                <ul class="rating-category-list">
                                    <li>
                                        <span class="rating-category-label">Looks</span>
                                        <span>
                                            <span class="star-one-icon"></span>
                                            <span class="text-bold">3</span>
                                            <span class="font12"> / 5</span>
                                        </span>
                                    </li>
                                    <li>
                                        <span class="rating-category-label">Fuel Economy</span>
                                        <span>
                                            <span class="star-one-icon"></span>
                                            <span class="text-bold">4</span>
                                            <span class="font12"> / 5</span>
                                        </span>
                                    </li>
                                    <li>
                                        <span class="rating-category-label">Performance</span>
                                        <span>
                                            <span class="star-one-icon"></span>
                                            <span class="text-bold">2</span>
                                            <span class="font12"> / 5</span>
                                        </span>
                                    </li>
                                    <li>
                                        <span class="rating-category-label">Value for Money</span>
                                        <span>
                                            <span class="star-one-icon"></span>
                                            <span class="text-bold">4</span>
                                            <span class="font12"> / 5</span>
                                        </span>
                                    </li>
                                    <li>
                                        <span class="rating-category-label">Style/Comfort</span>
                                        <span>
                                            <span class="star-one-icon"></span>
                                            <span class="text-bold">3.5</span>
                                            <span class="font12"> / 5</span>
                                        </span>
                                    </li>
                                </ul>
                                <div class="clear"></div>
                            </div>
                        </div>
                        <div class="clear"></div>
                    </div>

                <%--<div class="grid-4 alpha margin-bottom15 margin-top15">
                    <img src="<%= Bikewale.Common.MakeModelVersion.GetModelImage(HostUrl,OriginalImagePath,Bikewale.Utility.ImageSize._210x118) %>" title="<%=BikeMake%> <%=BikeModel%>" />
                    <div class="margin-top10">
                        <span class="text-highlight"><%=!IsNew && IsUsed ? "Last Recorded Price Rs: " : "Start at Rs: " %> <%= CommonOpn.FormatPrice( ModelStartPrice ) %></span><br />
                        <span><a title="<%= BikeMake %> <%= BikeModel %> details" href='/<%= MakeMaskingName%>-bikes/<%= ModelMaskingName %>/'><%=BikeModel%> Details</a><% if (IsNew && IsUsed)                                                                                                                                                                        { %><span class="text-grey"> | </span><a href="/pricequote/default.aspx?model=<%= ModelId %>" class="getquotation" data-pqsourceid="<%= (int)Bikewale.Entities.PriceQuote.PQSourceEnum.Desktop_UserReview_ModelPage %>" data-modelid="<%= ModelId %>">Check On-Road Price</a><% } %></span>
                    </div>
                </div>--%>
                <%--<div class="grid-8 omega margin-top10">
                <div class="grid-6 alpha omega">
                    <h3><%if (handleName != "")
                          {%><%=handleName%><%}
                          else
                          {%><%=reviewerName%><%}%>'s Ratings</h3>
                    <table width="100%" cellpadding="2" cellspacing="0" border="0" class="margin-top5">
                        <tr>
                            <td width="130">Overall</td>
                            <td><%= CommonOpn.GetRateImage(overallR) %></td>
                        </tr>
                        <tr>
                            <td>Looks</td>
                            <td><%= CommonOpn.GetRateImage(styleR) %></td>
                        </tr>
                        <tr>
                            <td>Performance</td>
                            <td><%= CommonOpn.GetRateImage(performanceR) %></td>
                        </tr>
                        <tr>
                            <td>Space/Comfort</td>
                            <td><%= CommonOpn.GetRateImage(comfortR) %></td>
                        </tr>
                        <tr>
                            <td>Fuel Economy</td>
                            <td><%= CommonOpn.GetRateImage(fuelEconomyR) %></td>
                        </tr>
                        <tr>
                            <td>Value For Money</td>
                            <td><%= CommonOpn.GetRateImage(valueR) %></td>
                        </tr>
                    </table>
                </div>
                <div class="grid-6 omega margin-top20">
                    Own a <%= BikeName %>? Help others make an informed buying decision. 
                </div>
                <div class="clear"></div>
            </div>--%>

                <div class="bg-white padding-18-20 padding-top0">
                    <div class="padding-top20 border-solid-top padding-bottom15">
                        <h3 class="text-black">Good about this bike</h3>
                        <p class="text-default font14 margin-top10"><%= pros %></p>
                    </div>
                    <div class="padding-top20 border-solid-top padding-bottom15">
                        <h3 class="text-black">Not so good about this bike</h3>
                        <p class="text-default font14 margin-top10"><%= cons %></p>
                    </div>
                     <div class="padding-top20 border-solid-top padding-bottom15">
                         <h3 class="text-black">Full Review <%--<span><%= userLoggedIn ? "<a class='f-small' href='/userreviews/editreview.aspx?rid=" + reviewId + "'>Edit</a>" : "" %></span>--%></h3>
                         <div class="margin-top15 format-content"><%= comments %></div>

                         <div class="margin-top10 content-block grey-bg border-light margin-bottom15">
                            <div class="grid-4 alpha ">
                                <div class="text-grey"><%=viewed%> Views, <span id="spnLiked"><%=liked%></span> of <span id="spnDisliked"><%= liked + disliked %></span> people found this review useful</div> 
                            </div>
                            <div class="grid-3 omega readmore">
                                <p id="divAbuse">Inappropriate Review? <a onclick='javascript:abuseClick(<%= reviewId %>)' class=" pointer">Report Abuse</a> </p>
                            </div>
                            <div class="clear"></div>
                        </div>
                         <div class="padding-top20 border-solid-top helpful">
                            <div id="divHelpful" class="readmore black-text">Was this review helpful to you? <a onclick='javascript:helpfulClick(<%= reviewId %>, "1")' class=" pointer">Yes</a>  <a onclick='javascript:helpfulClick(<%= reviewId %>, "0")' class=" pointer">No</a></div>
                         </div>
                     </div>
                </div>
                
           
            <%--<table class="margin-top15" width="100%" border="0" cellpadding="2" cellspacing="0">
                <tr id="trVerReviewed" runat="server" class="hide">
                    <td><span>Version Reviewed</span></td>
                    <td><%= BikeVersion %></td>
                </tr>
                <tr>
                    <td width="80"><span class="price2 black-text">Good :</span></td>
                    <td><%= pros %></td>
                </tr>
                <tr>
                    <td><span class="price2 black-text">Bad : </span></td>
                    <td><%= cons %></td>
                </tr>
            </table>--%>

            
            
            
            <BikeWale:DiscussIt ID="ucDiscuss" runat="server" />
            <div class="mid-box hide">
                <div class="left-grid-lt" style="width: 300px;"><%= Prev %></div>
                <div class="left-grid-rt" align="right" style="width: 300px;"><%= Next %></div>
            </div>
            <div class="clear"></div>
            <div class="gray-block hide">
                <h2 class="hd2">More Reviews on <%= BikeMake %> <%= BikeModel %></h2>
                <asp:repeater id="rptMoreUserReviews" runat="server">
			        <itemtemplate>
				        <div class="mar-top-5 hr-dotted" style="padding-bottom:5px;">
					        <span style="display:none;"><%# CommonOpn.GetRateImage( Convert.ToDouble( DataBinder.Eval(Container.DataItem, "OverallR") ) ) %><br /></span>
					        <a href="/<%# MakeMaskingName%>-bikes/<%# ModelMaskingName%>/user-reviews/<%# DataBinder.Eval(Container.DataItem, "ReviewId") %>.html"><%# DataBinder.Eval(Container.DataItem, "Title") %></a> <span class="text-grey">by <%# DataBinder.Eval(Container.DataItem, "CustomerName") %> on <%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "EntryDateTime")).ToString("dd MMM, yyyy") %></span>					
				        </div>
			        </itemtemplate>
		        </asp:repeater>
                <a href="/<%= MakeMaskingName%>-bikes/<%= ModelMaskingName%>/user-reviews/">All <%= BikeMake %> <%= BikeModel %> user reviews</a>
                <span class="icon-sheet more-link"></span>
                <div class="clear"></div>
            </div>
        </div>
            <div class="bg-white padding-18-20 padding-top0 margin-bottom20 content-box-shadow">
                <div class="alpha omega padding-top20 padding-bottom15">
                        <div class="model-user-review-rating-container leftfloat">
                            <p class="font16 text-bold">4</p>
                            <p class="margin-top5">
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
                <div class="alpha omega padding-top20 padding-bottom15">
                        <div class="model-user-review-rating-container leftfloat">
                            <p class="font16 text-bold">4</p>
                            <p class="margin-top5">
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
            </div>
        </div>
        <div class="grid-4">
                <%--<div class="margin-top15">
                    <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
                    <!-- #include file="/ads/Ad300x250.aspx" -->
                </div>--%>

                <div class="content-box-shadow padding-15-20-10 margin-top10">
                <h2>Popular bikes</h2>
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
            
                <!-- BikeWale_NewBike/BikeWale_NewBike_HP_300x250 -->
                <!-- #include file="/ads/Ad300x250BTF.aspx" -->
            </div>
        </div>
    </div>
    <div class="clear"></div>
</div>
<div id="report-abuse" class="hide">
    <span>Comments</span>
    <p>
        <textarea type="text" id="txtAbuseComments" rows="6" cols="35"></textarea>
    </p>
    <span id="spnAbuseComments" class="error"></span>
    <br />
    <a id="btnReportReviewAbuse" class="buttons" onclick="javascript:reportAbuse()">Report</a>
</div>

<!-- #include file="/includes/footerBW.aspx" -->
    <link href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
<!-- #include file="/includes/footerscript.aspx" -->

<%--<script type="text/javascript" src="<%= staticUrlPath != "" ? "https://st1.aeplcdn.com" + staticUrlPath : "" %>/src/graybox.js?v=1.0"></script>--%>
<script language="javascript">
    function helpfulClick(reviewId, helpful) {
        //alert(reviewId + "," + helpful);
        $.ajax({
            type: "POST",
            url: "/ajaxpro/Bikewale.Ajax.AjaxUserReviews,Bikewale.ashx",
            data: '{"reviewId":"' + reviewId + '","helpful":"' + helpful + '"}',
            beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "UpdateReviewHelpful"); },
            success: function (response) {
                var responseObj = eval('(' + response + ')');
                if (responseObj.value) {
                    var likedOrig = Number(document.getElementById("spnLiked").innerHTML);
                    var disliked = Number(document.getElementById("spnDisliked").innerHTML);

                    if (helpful == "1") {
                        document.getElementById("spnLiked").innerHTML = likedOrig + 1;
                    }

                    document.getElementById("spnDisliked").innerHTML = disliked + 1;
                    document.getElementById("divHelpful").innerHTML = "This review was " + (helpful == true ? "helpful" : "not helpful") + " to me.";
                } else {
                    document.getElementById("divHelpful").innerHTML = "You have already voted for this review.";
                }
            }
        });
    }

    function abuseClick(reviewId) {
        $.ajax({
            type: "POST",
            url: "/ajaxpro/Bikewale.Ajax.AjaxCommon,Bikewale.ashx",
            data: {},
            beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetCurrentUserId"); },
            success: function (response) {
                var userId = eval('(' + response + ')');
                //alert(userId.value);
                if (userId.value != "-1") {
                    var caption = "Why do you want to report it abuse?";
                    var url = "reviewcomments.aspx";
                    var applyIframe = false;

                    GB_show(caption, url, 180, 350, applyIframe, $("#report-abuse").html());
                } else {
                    location.href = "/users/login.aspx?returnUrl=/content/userreviews/reviewdetails.aspx?rid=" + reviewId;
                }
            }
        });
    }

    function reportAbuse() {
        var isError = false;

        if ($("#txtAbuseComments").val().trim() == "") {
            $("#spnAbuseComments").html("Comments is required");
            isError = true;
        } else {
            $("#spnAbuseComments").html("");
        }

        if (!isError) {
            var commentsForAbuse = $("#txtAbuseComments").val().trim();
            var reviewId = '<%= reviewId %>';

            $.ajax({
                type: "POST",
                url: "/ajaxpro/Bikewale.Ajax.AjaxUserReviews,Bikewale.ashx",
                data: '{"reviewId":"' + reviewId + '","comments":"' + commentsForAbuse + '"}',
                beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "UpdateReviewAbuse"); },
                success: function (response) {
                    var responseObj = eval('(' + response + ')');
                    if (responseObj.value == true) {
                        GB_hide();
                        document.getElementById("divAbuse").innerHTML = "Your request has been sent to the administrator.";
                    }
                }
            });
        }
    }
</script>
 <!-- #include file="/includes/fontBW.aspx" -->
</form>
</body>
</html>