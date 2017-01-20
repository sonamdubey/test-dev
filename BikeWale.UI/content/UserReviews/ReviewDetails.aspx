<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Content.ReviewDetails" Trace="false" %>

<%@ Import Namespace="Bikewale.Common" %>
<%@ Register TagPrefix="BikeWale" TagName="DiscussIt" Src="/Controls/DiscussIt.ascx" %>
<%@ Register TagPrefix="BP" TagName="InstantBikePrice" Src="/controls/instantbikeprice.ascx" %>
<%@ Register TagPrefix="LD" TagName="LocateDealer" Src="/controls/locatedealer.ascx" %>
<%@ Register TagPrefix="BW" TagName="MostPopularBikesMin" Src="~/controls/MostPopularBikesMin.ascx" %>
<%@ Register TagPrefix="BW" TagName="UserReviewSimilarBike" Src="~/controls/UserReviewSimilarBike.ascx" %>
<%@ Register Src="~/controls/NewUserReviewsList.ascx" TagPrefix="BW" TagName="UserReviews" %>

<!DOCTYPE html>
<html>
<head>
    <%
        title = pageMetas.Title;
        description = pageMetas.Description;
        keywords = pageMetas.Keywords;
        AdId = "1395986297721";
        AdPath = "/1017752/BikeWale_New_";
        alternate = pageMetas.AlternateUrl;
        canonical =pageMetas.CanonicalUrl;
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
    
         <section class="bg-light-grey padding-top10" id="breadcrumb">
            <div class="container">
                <div class="grid-12">
                    <div class="breadcrumb margin-bottom15">
                        <!-- breadcrumb code starts here -->
                           <%if(objReview!=null&& objReview.ReviewEntity!=null&&objReview.BikeEntity!=null&&objReview.BikeEntity.ModelEntity!=null&&objReview.BikeEntity.MakeEntity!=null) {%>
                        <ul>
                            <li itemscope itemtype="http://data-vocabulary.org/Breadcrumb"><a href="/" itemprop="url">
                                <span itemprop="title">Home</span></a>
                            </li>
                            <li itemscope itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="bwsprite fa-angle-right margin-right10"></span>
                                <a href="/<%= objReview.BikeEntity.MakeEntity.MaskingName%>-bikes/" itemprop="url">
                                    <span itemprop="title"><%= objReview.BikeEntity.MakeEntity.MakeName%> Bikes</span>
                                </a>
                            </li>
                            <li itemscope itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="bwsprite fa-angle-right margin-right10"></span>
                                <a href="/<%= objReview.BikeEntity.MakeEntity.MaskingName%>-bikes/<%= objReview.BikeEntity.ModelEntity.MaskingName%>/" itemprop="url">
                                    <span itemprop="title"><%= objReview.BikeEntity.MakeEntity.MakeName%> <%= objReview.BikeEntity.ModelEntity.ModelName%> </span>
                                </a>
                            </li>
                            <li itemscope itemtype="http://data-vocabulary.org/Breadcrumb">
                                <span class="bwsprite fa-angle-right margin-right10"></span>
                                <a href="/<%= objReview.BikeEntity.MakeEntity.MaskingName%>-bikes/<%= objReview.BikeEntity.ModelEntity.MaskingName%>/user-reviews/" itemprop="url">
                                    <span itemprop="title">User Reviews</span>
                                </a>
                            </li>
                            <li><span class="bwsprite fa-angle-right margin-right10"></span>
                                <span><%= objReview.ReviewEntity.ReviewTitle %></span>
                            </li>
                        </ul>
                       
                        <div class="clear"></div>
                           <%} %>
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
                                <div class="grid-9 alpha inline-block">
                                    <h1 class="margin-bottom5"><%= objReview.ReviewEntity.ReviewTitle  %></h1>
                                    <div>
                                        <span class="bwsprite calender-grey-sm-icon"></span>
                                        <span class="article-stats-content margin-right20"><%= Bikewale.Utility.FormatDate.GetFormatDate(Convert.ToString(objReview.ReviewEntity.ReviewDate), "MMMM dd, yyyy hh:mm tt")  %></span>
                                        <span class="bwsprite author-grey-sm-icon"></span>
                                        <span class="article-stats-content text-capitalize">
                                            <%if (handleName != "")
                                              { %>
                                                <%=handleName%>
                                            <% }
                                              else
                                              { %>
                                                <%= objReview.ReviewEntity.WrittenBy %>
                                            <%} %>
                                        </span>
                                    </div>
                                </div>
                                <div class="grid-3 text-right alpha omega inline-block">
                                    <a href="/content/userreviews/writereviews.aspx?bikem=<%= objReview.BikeEntity.ModelEntity.ModelId %>" class="btn btn-teal btn-size-150">Write a review</a>
                                </div>
                            <div class="clear"></div>
                        </div>

                        <div class="content-inner-block-20">
                            <div class="grid-3 alpha">
                                <a href="/<%= objReview.BikeEntity.MakeEntity.MaskingName%>-bikes/<%= objReview.BikeEntity.ModelEntity.MaskingName%>/" title="<%= BikeName%>">
                                    <img src="<%= Bikewale.Common.MakeModelVersion.GetModelImage(objReview.HostUrl,objReview.OriginalImagePath,Bikewale.Utility.ImageSize._144x81) %>" title="<%=BikeName%>" />
                                </a>
                            </div>
                            <div class="grid-9 omega">
                                <h2 class="font14 text-default"><a href="/<%= objReview.BikeEntity.MakeEntity.MaskingName%>-bikes/<%= objReview.BikeEntity.ModelEntity.MaskingName%>/" title="<%= BikeName%>" class="text-default"><%= BikeName%></a></h2>
                              <%if (objReview.ModelSpecs != null && (objReview.ModelSpecs.Displacement > 0 || objReview.ModelSpecs.FuelEfficiencyOverall > 0 || objReview.ModelSpecs.KerbWeight>0 || objReview.ModelSpecs.MaxPower>0)){ %>
                                  <ul class="bike-review-features margin-top5">
                                   <%if(objReview.ModelSpecs.Displacement>0){ %> <li><%=objReview.ModelSpecs.Displacement %> cc</li><%} %>
                                   <%if(objReview.ModelSpecs.FuelEfficiencyOverall>0){ %>  <li><%=objReview.ModelSpecs.FuelEfficiencyOverall %> kmpl</li><%}%>
                                   <%if(objReview.ModelSpecs.MaxPower>0){ %>  <li><%=objReview.ModelSpecs.MaxPower %> bhp</li>                           <%}%>
                                   <%if(objReview.ModelSpecs.KerbWeight>0){ %>  <li><%=objReview.ModelSpecs.KerbWeight %> kgs</li>                       <%}%>
                                </ul>
                                <%} %>
                                <%if (!objReview.New) {%>
                          
                                <p class="margin-top10 text-light-grey font14">Ex-showroom price, <%=Bikewale.Utility.BWConfiguration.Instance.DefaultName %></p>
                                <div class="margin-top5">  
                                    <span class="bwsprite inr-lg"></span>&nbsp;<span class="font18 text-bold"><%=Bikewale.Utility.Format.FormatPrice(objReview.ModelBasePrice) %></span>
                                </div>
                                <%}
                            
                            else{ %>
                                       <p class="margin-top10 text-light-grey font14">Last known Ex-showroom price,</p>
                            <div class="margin-top5">  
                                <span class="bwsprite inr-lg"></span>&nbsp;<span class="font18 text-bold"><%=Bikewale.Utility.Format.FormatPrice(objReview.ModelBasePrice) %></span>
                            </div>
                            <%} %>
                            <div class="clear"></div>
                        </div>

                            <div class="border-solid ratings margin-top15 display-table">
                                <div class="rating-box overall text-center content-inner-block-15">
                                    <p class="text-bold font14 margin-bottom10">
                                        <span class="text-truncate inline-block">
                                        <%if (handleName != "")
                                            { %>
                                            <%=handleName%>
                                        <% }
else
                                            { %>
                                            <%=objReview.ReviewEntity.WrittenBy%><%}%>'s Rating</p>
                                    <%if (objReview.ReviewRatingEntity.OverAllRating>0)
                                    { %>
                                    <div>
                                        <span class="star-one-icon"></span>
                                        <div class="inline-block">
                                            <span class="font20 text-bold"><%=Math.Round(objReview.ReviewRatingEntity.OverAllRating,1) %></span>
                                            <span class="padding-left2 font12 text-light-grey">/ 5</span>
                                        </div>
                                    </div><%} %>
                                </div>
                                <div class="rating-category-list-container padding-top10 padding-bottom10 star-icon-sm">
                                    <ul class="rating-category-list">
                                    <%if (objReview.ReviewRatingEntity.ModelRatingLooks > 0)
                                    { %><li>
                                        <span class="rating-category-label">Looks</span>
                                        <span>
                                            <span class="star-one-icon"></span>
                                            <span class="text-bold"><%=Math.Round(objReview.ReviewRatingEntity.ModelRatingLooks,1) %></span>
                                            <span class="font12"> / 5</span>
                                        </span>
                                    </li>
                                        <%} %>
                                        <%if (objReview.ReviewRatingEntity.FuelEconomyRating > 0)
                                    { %>
                                    <li>
                                        <span class="rating-category-label">Fuel Economy</span>
                                        <span>
                                            <span class="star-one-icon"></span>
                                            <span class="text-bold"><%=Math.Round(objReview.ReviewRatingEntity.FuelEconomyRating,1) %></span>
                                            <span class="font12"> / 5</span>
                                        </span>
                                    </li>
                                        <%} %>
                                        <%if (objReview.ReviewRatingEntity.PerformanceRating > 0)
                                    { %>
                                    <li>
                                        <span class="rating-category-label">Performance</span>
                                        <span>
                                            <span class="star-one-icon"></span>
                                            <span class="text-bold"><%=Math.Round(objReview.ReviewRatingEntity.PerformanceRating,1)%></span>
                                            <span class="font12"> / 5</span>
                                        </span>
                                    </li>
                                        <%} %>
                                        <%if (objReview.ReviewRatingEntity.ValueRating > 0)
                                    { %>
                                    <li>
                                        <span class="rating-category-label">Value for Money</span>
                                        <span>
                                            <span class="star-one-icon"></span>
                                            <span class="text-bold"><%=Math.Round(objReview.ReviewRatingEntity.ValueRating,1) %></span>
                                            <span class="font12"> / 5</span>
                                        </span>
                                    </li>
                                        <%} %>
                                        <%if (objReview.ReviewRatingEntity.StyleRating > 0)
                                    { %>
                                    <li>
                                        <span class="rating-category-label">Style/Comfort</span>
                                        <span>
                                            <span class="star-one-icon"></span>
                                            <span class="text-bold"><%=Math.Round(objReview.ReviewRatingEntity.StyleRating,1) %></span>
                                            <span class="font12"> / 5</span>
                                        </span>
                                    </li>
                                        <%} %>
                                </ul>
                                    <div class="clear"></div>
                                </div>
                            </div>
                            <div class="padding-bottom20 font14">
                                <div class="padding-top20 border-solid-top padding-bottom20">
                                    <h2 class="font18 margin-bottom10">Good about this bike</h2>
                                    <p><%= objReview.ReviewEntity.Pros %></p>
                                </div>
                                <div class="padding-top20 border-solid-top padding-bottom20">
                                    <h2 class="font18 margin-bottom10">Not so good about this bike</h2>
                                    <p><%= objReview.ReviewEntity.Cons %></p>
                                </div>
                                <div class="padding-top20 border-solid-top">
                                    <h2 class="font18 margin-bottom10">Full Review</h2>
                                    <div class="format-content"><%= objReview.ReviewEntity.Comments %></div>

                                    <div class="margin-bottom15 text-light-grey">
                                        <div class="grid-6 alpha">
                                            <span class="bwsprite review-sm-lgt-grey"></span>
                                            <span class="article-stats-content margin-right15"><%=objReview.ReviewEntity.Viewed%></span>
                                            <span class="bwsprite like-icon"></span>
                                            <span id="spnLiked" class="article-stats-content margin-right15"><%=objReview.ReviewEntity.Liked%></span>
                                            <span class="bwsprite dislike-icon"></span>
                                            <span id="spnDisliked" class="article-stats-content"><%=objReview.ReviewEntity.Disliked %></span>
                                        </div>
                                        <div class="grid-6 omega readmore text-right">
                                            <p id="divAbuse">Inappropriate Review? <a onclick='javascript:abuseClick(<%= reviewId %>)' class="cur-pointer">Report Abuse</a> </p>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                    <div class="padding-top20 border-solid-top helpful text-light-grey">
                                        <div id="divHelpful" class="readmore black-text">Was this review helpful to you? <a onclick='javascript:helpfulClick(<%= reviewId %>, "1")' class="btn-transparent-sm">Yes</a>  <a onclick='javascript:helpfulClick(<%= reviewId %>, "0")' class="btn-transparent-sm">No</a>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <BikeWale:DiscussIt ID="ucDiscuss" runat="server" />
                        <div class="clear"></div>
                    </div>
                    </div>
                        <% if (ctrlUserReviews.FetchedRecordsCount > 0){ %>
                        <div class="content-box-shadow bg-white padding-18-20 margin-bottom20">
                            <!-- user reviews -->
                            <BW:UserReviews runat="server" ID="ctrlUserReviews" />
                            <!-- user reviews ends -->
                        </div>
                         <% } %>
                    </div>
                    <div class="grid-4 omega">
                        <%if(ctrlUserReviewSimilarBike.FetchCount>0){ %>
                        <div class="content-box-shadow padding-15-20-10 margin-bottom20">
                            <h2>User reviews of similar bikes</h2>
                                <BW:UserReviewSimilarBike ID="ctrlUserReviewSimilarBike" runat="server" />
                        </div>
                        <%} %>
                            <div class="margin-bottom20">
                        <!-- #include file="/ads/Ad300x250BTF.aspx" -->
                                </div>
                        <%if (ctrlPopularBikes.FetchedRecordsCount>0)
                            { %>
                    
                            <BW:MostPopularBikesMin ID="ctrlPopularBikes" runat="server" />
                      
                        <%} %>         
                       
                    </div>
                    <div class="clear"></div>
                </div>
                <div class="clear"></div>
            </div>
        </section>
       
      
        <div id="report-abuse">
            <p class="font20 text-bold text-center margin-bottom20">Why do you want to report it abuse?</p>
            <div class="report-abuse-close-btn position-abt pos-top10 pos-right10 bwsprite cross-lg-lgt-grey cur-pointer"></div>
            <p class="margin-bottom10 font14">Comment</p>
            <textarea id="txtAbuseComments"></textarea>
            <span id="spnAbuseComments" class="error font12 text-red"></span>
            <div class="text-center margin-top10">
                <a id="btnReportReviewAbuse" class="btn btn-orange" rel="nofollow" onclick="javascript:reportAbuse()">Report</a>
            </div>
        </div>

        <div id="popup-background"></div>
    
        <div id="report-abuse2" class="hide">
            <span>Comments</span>
            <p>
                <textarea type="text" id="txtAbuseComments2" rows="6" cols="35"></textarea>
            </p>
            <span id="spnAbuseComments2" class="error"></span>
            <br />
            <a id="btnReportReviewAbuse2" class="buttons" onclick="javascript:reportAbuse()">Report</a>
        </div>
        <!-- #include file="/includes/footerBW.aspx" -->
        <link href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/css/bw-common-btf.css?<%=staticFileVersion %>" rel="stylesheet" type="text/css" />
        <!-- #include file="/includes/footerscript.aspx" -->
        <script type="text/javascript">
            function helpfulClick(reviewId, helpful) {
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
                            else {
                                document.getElementById("spnDisliked").innerHTML = disliked + 1;
                            }
                            document.getElementById("divHelpful").innerHTML = "Thank you for the feedback!";
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
                            reportAbusePopup.open();
                            appendState('reportPopup');
                            var caption = "Why do you want to report it abuse?";
                            var url = "reviewcomments.aspx";
                            var applyIframe = false;

                           
                        } else {
                            location.href = "/users/login.aspx?returnUrl=/content/userreviews/reviewdetails.aspx?rid=" + reviewId + "&hash=report-abuse-popup";
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
                                reportAbusePopup.close();
                                document.getElementById("divAbuse").innerHTML = "Your request has been sent to the administrator.";
                            }
                        }
                    });
                }
            }

            $('.report-abuse-close-btn, #popup-background').on('click', function () {
                reportAbusePopup.close();
                history.back();
            });

            $(document).keydown(function (event) {
                if (event.keyCode == 27) {
                    if ($('#report-abuse').is(':visible')) {
                        reportAbusePopup.close();
                        history.back();
                    }
                }

            });
            $(function () {

                if (window.location.hash == "#report-abuse-popup") {
                    abuseClick("<%=reviewerId%>")
                }
            });
            var reportAbusePopup = {
                open: function () {
                    $('#report-abuse').show();
                    $('body').addClass('lock-browser-scroll');
                    $('#popup-background').show();
                },
                close: function () {
                    $('#report-abuse').hide();
                    $('body').removeClass('lock-browser-scroll');
                    $('#popup-background').hide();
                }
            };

            /* popup state */
            var appendState = function (state) {
                window.history.pushState(state, '', '');
            };
            $(window).on('popstate', function (event) {
                if ($('#report-abuse').is(':visible')) {
                    reportAbusePopup.close();
                }
            });
        </script>
        <!-- #include file="/includes/fontBW.aspx" -->
    </form>
</body>
</html>