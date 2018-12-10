<%@ Page Language="C#" Inherits="Carwale.UI.Editorial.ReadFullUserReview" AutoEventWireup="false" Debug="false" Trace="false" %>

<%@ Import Namespace="Carwale.UI.Common" %>
<%@ Register TagPrefix="CarWale" TagName="DiscussIt" Src="/Controls/DiscussIt.ascx" %>
<%@ Register TagPrefix="Ur" TagName="UserReviews" Src="/Controls/ReadUserReviews.ascx" %>
<%@ Register TagPrefix="uc" TagName="SubNavigation" Src="/Controls/subNavigation.ascx" %>
<%@ Register TagPrefix="uc" TagName="UsedCarsCount" Src="/Controls/UsedCarsCount.ascx" %>
<%@ Register TagPrefix="uc" TagName="RoadTestSpc" Src="/Controls/RoadTestSpc.ascx" %>

<!doctype html>
<html itemscope itemtype="http://schema.org/WebPage">
<head>


    <%
        // Define all the necessary meta-tags info here.
        // To know what are the available parameters,
        // check page, headerCommon.aspx in common folder.

        PageId = 43;
        Title = title + " - A Review on " + CarMake + " " + CarModel + " by " + reviewerName;
        Description = CarName + " User Review - " + "A review/feedback on " + CarName + " by " + reviewerName + ". Find out what " + reviewerName + " has to say about " + CarName + ".";
        Keywords = CarMake + " " + CarModel + " review, " + CarMake + " " + CarModel + " user review, car review, owner feedback, consumer review";
        Revisit = "15";
        DocumentState = "Static";
        OEM = oem;
        BodyType = bodyType;
        Segment = subSegment;

        canonical = "https://www.carwale.com/" + UrlRewrite.FormatSpecial(CarMake) + "-cars/" + MaskingName + "/userreviews/" + reviewId + "/";
        altUrl = "https://www.carwale.com/m/" + UrlRewrite.FormatSpecial(CarMake) + "-cars/" + MaskingName + "/userreviews/" + reviewId + "/";
        AdId = "1396440332273";
        AdPath = "/1017752/ReviewsNews_";
        targetKey = "Models";
        targetValue = CarModel.Trim();
    //hdnTest.Value = PageId.ToString();
        DeeplinkAlternatives = Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString();

    %>
    <!-- #include file="/includes/global/head-script.aspx" -->
    <!-- #include file="/includes/NewCarsScripts.aspx" -->
    <meta name="twitter:card" content="summary_large_image">
<meta name="twitter:site" content="@CarWale">
<meta name="twitter:title" content="<%=title %>">
<meta name="twitter:description" content="<%=(CarName + " User Review - " + "A review/feedback on " + CarName + " by " + reviewerName + ". Find out what " + reviewerName + " has to say about " + CarName + ".")%>">
<meta name="twitter:creator" content="@CarWale">
<meta name="twitter:image" content="<%=logoURL %>">
<meta property="og:title" content="<%=title %>" />
<meta property="og:type" content="article" />
<meta property="og:url" content="<%="https://www.carwale.com/" + UrlRewrite.FormatSpecial(CarMake) + "-cars/" + MaskingName + "/userreviews/" + reviewId + ".html" %>" />
<meta property="og:image" content="<%=logoURL%>" />
<meta property="og:description" content="<%=(CarName + " User Review - " + "A review/feedback on " + CarName + " by " + reviewerName + ". Find out what " + reviewerName + " has to say about " + CarName + ".")%>" />
<meta property="og:site_name" content="CarWale" />
<meta property="article:published_time" content="<%=entryDate.ToString("s")%>" />
<meta property="article:section" content="Car Reviews" />
<meta property="article:tag" content="<%=oem+" "+bodyType+" "+subSegment %>" />
<meta property="fb:admins" content="154881297559" />
<meta property="fb:pages" content="154881297559" />
<link rel="stylesheet" href="/static/css/research_new.css" type="text/css" >
<link rel="stylesheet" href="/static/css/css-research-photos.css" type="text/css" >
   <script type="text/javascript">
       var defaultCookieDomain = '@Carwale.UI.Common.CookiesCustomers.CookieDomain';
	</script>
<script  type="text/javascript"  src="/static/src/graybox.js" ></script>
<script  type="text/javascript"  src="/static/js/plugins.js" ></script>
<script   src="/static/js/contenttracking.js"  type="text/javascript"></script>

    <script type='text/javascript'>
        googletag.cmd.push(function () {
            googletag.defineSlot('<%= AdPath %>300x250', [300, 250], 'div-gpt-ad-<%= AdId %>-0').addService(googletag.pubads());
            googletag.defineSlot('<%= AdPath %>300x250_BTF', [300, 250], 'div-gpt-ad-<%= AdId %>-1').addService(googletag.pubads());
            googletag.defineSlot('<%= AdPath %>970x90', [[220, 90], [728, 90], [950, 90], [960, 90], [970, 66], [970, 90]], 'div-gpt-ad-<%= AdId %>-2').addService(googletag.pubads());
            googletag.defineSlot('<%= AdPath %>300x600', [[300, 600], [300, 250], [160, 600], [120, 600], [120, 240]], 'div-gpt-ad-<%= AdId %>-9').addService(googletag.pubads());
            <% if (Ad643 == true)
               { %>googletag.defineSlot('/7590/CarWale_NewCar/NewCar_Make_Page/NewCar_Model_Page/NewCar_Model_643x65', [643, 65], 'div-gpt-ad-1383197943786-0').addService(googletag.pubads()); <% } %>
            googletag.pubads().setTargeting("<%= targetKey %>", "<%= targetValue %>");
            googletag.pubads().setTargeting("City", "<%= CookiesCustomers.MasterCity.ToString() %>");
            googletag.pubads().setTargeting('UserModelHistory', '<%= CookiesCustomers.UserModelHistory.Replace('~', ',')%>');
            //googletag.pubads().enableSyncRendering();
            googletag.pubads().collapseEmptyDivs();
            googletag.pubads().enableSingleRequest();
            googletag.enableServices();
        });
        $(document).on("click", "#video .jcarousel-control-right", function () {
            $("#video img.lazy:in-viewport").lazyload();
        });
    </script>
   
    <style>
        /*----------------- Share with Friends ---------------------------*/
        .share-page {
            border: 1px solid #CCCCCC;
            background-color: #F3F3F3;
            text-align: right;
            width: 155px;
            margin-top: 10px;
            float: right;
            padding: 5px;
            vertical-align: middle;
            line-height: 100%;
        }

            .share-page span {
                float: left;
                line-height: 15px;
            }

        .share-frnds {
            background: url(https://img.carwale.com/icons/share.gif);
            background-repeat: no-repeat;
            display: inline-block;
        }

        .facebook {
            background-position: 0 0;
            height: 16px;
            width: 20px;
        }

        .twitter {
            background-position: -21px 0;
            height: 16px;
            width: 20px;
        }

        .mail-frnds {
            background-position: -41px 0;
            height: 16px;
            width: 16px;
        }

        p {
            margin:10px 0;
        }
        /* model pills list */
.list-type-pills__item {
    display: inline-block;
    vertical-align: middle;
    text-align: center;
    background: #efefef;
    border-radius: 15px;
    line-height: 11px;
    margin-left: 5px;
}
.list-type-pills__item:first-child {
    margin-left: 0;
}
.item--padding {
    padding: 5px 11px;
}
.item--360-padding {
    padding: 3px 13px;
}
.list-type-pills__item span {
    display: inline-block;
    vertical-align: middle;
}
.list-type-pills__item a {
    display: block;
}
.list-item__target a:hover {
    text-decoration: none;
}
.list-item__label {
    font-size: 13px;
    color: #565a5c;
    font-weight: 600;
}
.item--icon-right-margin {
    margin-right: 7px;
}
.media-image-icon {
    width: 15px;
    height: 16px;
    background: url('https://imgd.aeplcdn.com/0x0/cw/static/icons/media-image-15.png') no-repeat;
    display: inline-block;
}
.media-video-icon {
    width: 15px;
    height: 15px;
    background: url(https://imgd.aeplcdn.com/0x0/cw/static/icons/565a5c/video-15x15.png) no-repeat;
    display: inline-block;
}
    </style>
</head>

<body class="bg-white header-fixed-inner special-page special-skin-body no-bg-color">
    <form runat="server">
        <!-- #include file="/includes/header.aspx" -->
        <asp:HiddenField ID="HiddenField1" runat="server"></asp:HiddenField>
        <section class="container">
            <div class="grid-12">
                <div class="padding-bottom10 padding-top10 text-center">
                    <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 0, 90, 0, 0, true, 2) %>
                </div>
            </div>
        </section>
        <div class="clear"></div>
        <section class="bg-light-grey padding-top10 padding-bottom20 no-bg-color">
            <div class="container">
                <div class="grid-12">
                    <div class="breadcrumb">
                        <!-- breadcrumb code starts here -->
                        <ul  class="special-skin-text">
                            <li><a href="/">Home</a></li>
                            <li>&rsaquo; <a href="/<%= UrlRewrite.FormatSpecial(CarMake)%>-cars/"><%= CarMake%></a></li>
                            <li>&rsaquo; <a href="/<%= UrlRewrite.FormatSpecial(CarMake)%>-cars/<%= MaskingName%>/"><%= CarModel%></a></li>
                            <li><span class="fa fa-angle-right margin-right10"></span>User Reviews</li>
                        </ul>
                        <div class="clear"></div>
                    </div>
                    <h1 class="font30 text-black leftfloat margin-top10 special-skin-text"><%=title %></h1>
                    <div class="rightfloat">
                        <!-- #include file="/includes/share.html" -->
                    </div>
                    <div class="clear"></div>
                    <div class="border-solid-bottom margin-top10 margin-bottom15"></div>
                </div>
                <div class="clear"></div>
                <div class="grid-8">
                    <%if (showSubNavigation)
                      { %><uc:SubNavigation ID="SubNavigation" runat="server" IsOverviewPage="false" />
                    <%} %>
                    <div class="content-box-shadow content-inner-block-10 content-details" id="<%=reviewId %>" categoryid="4" categoryname="userreviews" >
                        <uc:UsedCarsCount ID="UsedCarsCount1" runat="server" Visible="false" />
                        <uc:RoadTestSpc ID="ucRoadTestSpc" runat="server" Visible="false" />
                        <h4>A user review on <a href="/<%= UrlRewrite.FormatSpecial(CarMake)%>-cars/<%= MaskingName%>/"><%= CarMake %> <%= CarModel %></a>. Written by
                            <%if (handleName != "")
                              {%>
                            <a href="/community/members/<%=handleName%>.html"><%=handleName%></a>
                            <%}
                              else
                              {%>
                            <%=reviewerName%>
                            <%}%>
		                    on <%= entryDate.ToString("dd MMMM, yyyy") %></h4>
                        <div class="mid-box margin-top20">
                            <div class="leftfloat" style="width: 237px;">
                               <a href="<%=galleryHref %>" data-role="click-tracking" data-event="CWInteractive" data-action="desktop_user_review_detail_model_image" data-cat="contentcons" data-label="<%= CarMake %> <%= CarModel %>" target="_blank">
                                    <img class="img-border" width="210" src="<%= logoURL %>" title="<%=CarMake%> <%=CarModel%>" onerror="this.onerror=null;this.src='<%= Carwale.Utility.CWConfiguration._imgHostUrl %>310x174/cw/cars/no-cars.jpg';" />
                               </a>
<ul class="list-type-pills text-center">
                        <% if(imageCount > 0) { %>
                             
                                 <li class="list-type-pills__item">
                         <a href="<%=galleryHref %>" data-role="click-tracking" data-event="CWInteractive" data-action="desktop_user_review_detail_model_image" data-cat="contentcons" data-label="<%= CarMake %> <%= CarModel %>" target="_blank">
                                     <div class="item--padding">
                                         <span class="media-image-icon"></span>
                                         <span class="list-item__label">Images (<%= imageCount %>)</span>
                                     </div>                                            
                         </a>
                                 </li>
                         <% } if(videoCount > 0) { %>
                                 <li class="list-type-pills__item">
                                        <div class="list-type-pills__item" data-role="click" data-event="CWInteractive" data-cat="desktop_video_linkages" data-action="desktop_user_review_detail_model_image" data-label="<%= CarMake %> <%= CarModel %>">
                                            <a href="<%=Carwale.BL.CMS.CMSCommon.GetVideoUrl(CarMake, MaskingName, null, 0, false) %>" class="item--padding" data-role="inview-imp" data-cat="desktop_video_linkages" data-action="desktop_user_review_detail_model_image" data-label="<%= CarMake %> <%= CarModel %>">
                                                <span class="media-video-icon"></span>
                                                <span class="list-item__label ">Videos (<%=videoCount %>)</span>
                                            </a>
                                        </div>
                                    </li>
                         <% } %>    
                             </ul> 
                                <div style="margin-top: 10px; line-height: 17px;">
                                    <%if(modelDetails.New) {%><span class="text-bold">Price: ₹ <%= ModelStartPrice %></span><br />
                                    <span class="ex-showroom">Ex-showroom, <%= ConfigurationManager.AppSettings["DefaultCityName"].ToString() %></span><br />
                                    <%}else { %><br /><span class="text-bold">This car is discontinued</span><%} %>
                                </div>
                            </div>
                            <div class="rightfloat" style="width: 380px;">
                                <div class="price2">
                                    <%if (handleName != "")
                                      {%><%=handleName%><%}
                                      else
                                      {%><%=reviewerName%><%}%>'s Ratings
                                </div>
                                <div>
                                    <div class="rightfloat" style="width: 140px;">
                                        <span class="f-small font12">Own a <span class="price2"><%= CarMake %> <%= CarModel %></span>?<br />Help others make an informed buying decision.</span>
                                        <p class="margin-top10"><a data-cwtccat="RatingLinkage" data-cwtcact="<%= string.Format("{0}LinkClick", Carwale.Entity.Enum.CwPages.UserReviewDetailDesktop) %>" data-cwtclbl="<%= string.Format("modelid={0}", ModelId) %>" class="btn btn-orange btn-xs" href="<%= Carwale.Utility.ManageCarUrl.CreateRatingPageUrl(Convert.ToInt32(ModelId)) %>" >Write a Review</a></p>
                                    </div>
                                    <ul class="model-ratings vr-dotted" style="padding-top: 8px; float: left;">
                                        <li><span><%= CommonOpn.GetRateImage(overallR) %></span>Overall</li>
                                        <li><span><%= CommonOpn.GetRateImage(styleR) %></span>Looks</li>
                                        <li><span><%= CommonOpn.GetRateImage(performanceR) %></span>Performance</li>
                                        <li><span><%= CommonOpn.GetRateImage(comfortR) %></span>Space/Comfort</li>
                                        <li><span><%= CommonOpn.GetRateImage(fuelEconomyR) %></span>Fuel Economy</li>
                                        <li><span><%= CommonOpn.GetRateImage(valueR) %></span>Value For Money</li>
                                    </ul>
                                    <div class="clear"></div>
                                </div>
                            </div>
                            <div class="clear"></div>
                        </div>
                        <%if (VersionId != "-1")
                          {%>
                        <div class="mid-box margin-top20">
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr id="trVerReviewed" runat="server">
                                    <td width="130"><span class="text-bold">Version Reviewed</span></td>
                                    <td width="10">:</td>
                                    <td><%= CarVersion %></td>
                                </tr>
                            </table>
                        </div>
                        <%}%>
                        <div class="mid-box margin-top20">
                            <table width="100%" cellpadding="3" cellspacing="0">
								<%if (!string.IsNullOrEmpty(PurchasedAs()))
								{%>
                                <tr>
                                    <td width="130"><span class="text-bold">Purchased As</span></td>
                                    <td width="10">:</td>
                                    <td><%= PurchasedAs() %></td>
                                </tr>
								<%}%>
								<%if (!string.IsNullOrEmpty(familiarity))
								{%>
                                <tr>
                                    <td><span class="text-bold">Familiarity</span></td>
                                    <td>:</td>
                                    <td><%= familiarity %></td>
                                </tr>
								<%}%>
								<% if (!string.IsNullOrEmpty(mileage) && !mileage.Equals("0"))
								{%>
                                <tr>
                                    <td><span class="text-bold">Fuel Economy</span></td>
                                    <td>:</td>
                                    <td><%= GetFuelEconomy() %></td>
                                </tr>
								<%}%>
                            </table>
                        </div>
						<%if (Carwale.UI.Common.Validations.IsValidProsCons(pros) || Carwale.UI.Common.Validations.IsValidProsCons(cons))
								{%>
                        <div class="mid-box margin-top20">
                            <table width="100%" cellpadding="3" cellspacing="0">
								<%if (Carwale.UI.Common.Validations.IsValidProsCons(pros))
								{%>
                                <tr>
                                    <td width="130"><span class="text-bold">Good</span></td>
                                    <td width="10">:</td>
                                    <td><%= pros %></td>
                                </tr>
								<%}%>
								<%if (Carwale.UI.Common.Validations.IsValidProsCons(cons))
								{%>
                                <tr>
                                    <td><span class="text-bold">Bad</span></td>
                                    <td>:</td>
                                    <td><%= cons %></td>
                                </tr>
								<%}%>
                            </table>
                        </div>
						<%}%>
                        <div class="mid-box margin-top20">
                            <h3 class="font14">Full Review <span><%= userLoggedIn ? "<a class='f-small' href='/userreviews/editreview.aspx?rid=" + reviewId + "'>Edit</a>" : "" %></span></h3>
                            <div><%= comments %></div>
                        </div>
                        <div class="mid-box margin-top20">
                            <!-- Facebook like button -->
                            <div id="fb-root"></div>
                            <script>
                                (function (d, s, id) {
                                    var js, fjs = d.getElementsByTagName(s)[0];
                                    if (d.getElementById(id)) return;
                                    js = d.createElement(s); js.id = id;
                                    js.src = "//connect.facebook.net/en_US/all.js#xfbml=1";
                                    fjs.parentNode.insertBefore(js, fjs);
                                }(document, 'script', 'facebook-jssdk'));
                            </script>
                            <div class="fb-like" data-send="true" data-width="450" data-show-faces="true"></div>
                            <!--End Facebook like button -->
                        </div>
                        <div class="hr-dotted"></div>
                        <div class="mid-box" style="padding-top: 5px;">
                            <div class="leftfloat vr-dotted" style="width: 320px;">
                                <div class="text-grey"><%=viewed%> Views, <span id="spnLiked"><%=liked%></span> of <span id="spnDisliked"><%= liked + disliked %></span> people found this review useful</div>
                                <div id="divHelpful">Was this review helpful to you? <a onclick='javascript:helpfulClick(<%= reviewId %>, "1")'>Yes</a> | <a onclick='javascript:helpfulClick(<%= reviewId %>, "0")'>No</a></div>
                            </div>
                            <div class="leftfloat" align="right" style="width: 280px;">
                                <p id="divAbuse">Inappropriate Review? <a onclick='javascript:abuseClick(<%= reviewId %>)'>Report Abuse</a> </p>
                            </div>
                            <div class="clear"></div>
                        </div>
                        <CarWale:DiscussIt ID="ucDiscuss" runat="server" />
                        <div class="mid-box hide">
                            <div class="left-grid-lt" style="width: 300px;"><%= Prev %></div>
                            <div class="left-grid-rt" align="right" style="width: 300px;"><%= Next %></div>
                        </div>
                        <div class="clear"></div>
                        <div class="gray-block content-inner-block-10 border-solid margin-top20">
                            <h2 class="font18">More Reviews on <%= CarMake %> <%= CarModel %></h2>
                            <asp:Repeater ID="rptMoreUserReviews" runat="server">
                                <ItemTemplate>
                                    <div class="mar-top-5 hr-dotted padding-bottom5">
                                        <span style="display: none;"><%# Carwale.UI.Common.CommonOpn.GetRateImage( Convert.ToDouble( DataBinder.Eval(Container.DataItem, "OverallR") ) ) %><br />
                                        </span>
                                        <a href="/<%# UrlRewrite.FormatSpecial(CarMake)%>-cars/<%# MaskingName%>/userreviews/<%# DataBinder.Eval(Container.DataItem, "ReviewId") %>/"><%# DataBinder.Eval(Container.DataItem, "Title") %></a> <span class="font11 text-light-grey">by <%# DataBinder.Eval(Container.DataItem, "CustomerName") %> on <%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "EntryDateTime")).ToString("dd MMM, yyyy") %></span>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                            <a class="rightfloat" href="/<%= UrlRewrite.FormatSpecial(CarMake)%>-cars/<%= MaskingName%>/userreviews/">All <%= CarMake %> <%= CarModel %> user reviews</a>
                            <span class="icon-sheet more-link"></span>
                            <div class="clear"></div>
                        </div>
                    </div>
                    <div id="video">
                    <% Carwale.UI.NewCars.RecommendCars.RazorPartialBridge.RenderAction("ModelVideos", "CarWidgets", param); %>
                    </div>
                </div>
                <div class="grid-4">
                        <div class="mid-box">
                            <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 300, 250, 20, 20, false, 0) %>
                        </div>
                        <div class="blue-block content-box-shadow content-inner-block-10 margin-bottom20">
                            <h2 class="hd2">User Car Reviews</h2>
                            <span class="font11">Read thousands of reviews on new cars.</span>
                            <Ur:UserReviews ID="ucUserReviews" runat="server" VerticalDisplay="true" />
                        </div>
                    
                        <div class="gray-block content-box-shadow content-inner-block-10">
                            <h2 class="font18 margin-top10">More on <%= CarMake %> <%= CarModel %></h2>
                            <ul class="ul-arrow">
                                <%if(modelDetails.New) {%>
                                    <li><a href="/new/<%= UrlRewrite.FormatSpecial(CarMake) %>-dealers/">Locate <%= CarMake %> dealers/showrooms</a></li>
                                <%} %>
                                <li><a href="/<%= UrlRewrite.FormatSpecial( CarMake )%>-cars/<%= MaskingName%>/">Check <%= CarMake %> <%= CarModel %> Price</a></li>
                                <li><a href="/used/<%= UrlRewrite.FormatSpecial(CarMake) %>-<%= MaskingName %>-cars/">Find Used <%= CarMake %> <%= CarModel %></a></li>
                                <li><a href="/insurance/">New Car Insurance</a></li>
                            </ul>
                        </div>
                    
                        <div class="mid-box margin-top20 margin-bottom20"> 
                            <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 300, 250, 0, 20, false, 1) %>
                        </div>
                    <div class="mid-box"> 
                            <%= Carwale.UI.HtmlHelpers.Helpers.GetAdBarForAspx(AdId, 300, 600, 20, 0, false, 9) %>
                        </div>
                </div>
                <div class="clear"></div>
                <div id="report-abuse" class="hide">
                    <span>Comments</span>
                    <p>
                        <textarea type="text" id="txtAbuseComments" rows="6" cols="35"></textarea>
                    </p>
                    <span id="spnAbuseComments" class="error"></span>
                    <br />
                    <a id="btnReportReviewAbuse" class="buttons-gray btn btn-orange" onclick="javascript:reportAbuse()">Report</a>
                </div>
            </div>
        </section>
        <div class="clear"></div>

        <script language="javascript">
            function helpfulClick(reviewId, helpful) {
                $.ajax({
                    type: "POST",
                    url: "/ajaxpro/CarwaleAjax.AjaxUserReviews,Carwale.ashx",
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
                    url: "/ajaxpro/CarwaleAjax.AjaxCommonPro,Carwale.ashx",
                    data: {},
                    beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "GetCurrentUserId"); },
                    success: function (response) {
                        var userId = eval('(' + response + ')');
                        if (userId.value != "-1") {
                            var caption = "Why do you want to report it abuse?";
                            var url = "reviewcomments.aspx";
                            var applyIframe = false;

                            GB_show(caption, url, 220, 350, applyIframe, $("#report-abuse").html());
                        } else {
                            location.href = "/users/login.aspx?returnUrl=/userreviews/reviewdetails.aspx?rid=" + reviewId;
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
                        url: "/ajaxpro/CarwaleAjax.AjaxUserReviews,Carwale.ashx",
                        data: '{"reviewId":"' + reviewId + '","comments":"' + commentsForAbuse.replace(/"/g, "'") + '"}',
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
        <!-- #include file="/includes/footer.aspx" -->
        <!-- all other js plugins -->
        <!-- #include file="/includes/global/footer-script.aspx" -->
    <script type="text/javascript">
        var articleBasicid = '<%= reviewId %>';
        Common.showCityPopup = false;
        $(document).ready(function () { ContentTracking.tracking.setUpTracking(1, 'UserReviews', '.content-details');  });
    </script>
    </form>
</body>
</html>
