<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Content.ReviewDetails" %>
<%@ Register TagPrefix="BW" TagName="UserReviewSimilarBike" Src="~/m/controls/UserReviewSimilarBike.ascx" %>
<%@ Register Src="/m/controls/NewUserReviewList.ascx" TagPrefix="BW" TagName="UserReviews" %>
<%@ Register TagPrefix="BW" TagName="GenericBikeInfo" Src="~/m/controls/GenericBikeInfoControl.ascx" %>
<!DOCTYPE html>
<html>
<head>
    <%
        title = pageMetas.Title;
        description = pageMetas.Description;
        keywords = pageMetas.Keywords;
        canonical = pageMetas.CanonicalUrl;
        AdPath = "/1017752/Bikewale_Mobile_Model";
        AdId = "1398837216327";
        //menu = "9";
    %>
    <!-- #include file="/includes/headscript_mobile_min.aspx" -->
    <link rel="stylesheet" type="text/css" href="/m/css/user-review/listing.css" />
    <script type="text/javascript">
        <!-- #include file="\includes\gacode_mobile.aspx" -->
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <!-- #include file="/includes/headBW_Mobile.aspx" -->

       <%if(objReview!=null){ %> 
        <section>
            <div class="container box-shadow bg-white section-bottom-margin">
                <div class="box-shadow padding-15-20">
                    <h1 class="margin-bottom10"><%= objReview.ReviewEntity.ReviewTitle %></h1>
                    <div class="grid-6 alpha padding-right5">
                        <span class="bwmsprite calender-grey-sm-icon"></span>
                        <span class="article-stats-content"><%= Bikewale.Utility.FormatDate.GetFormatDate(Convert.ToString(objReview.ReviewEntity.ReviewDate), "MMMM dd, yyyy ")  %></span>
                    </div>
                    <div class="grid-6 alpha omega">
                        <span class="bwmsprite author-grey-sm-icon"></span>
                        <span class="article-stats-content"><%= objReview.ReviewEntity.WrittenBy %></span>
                    </div>
                    <div class="clear"></div>
                </div>

                <div class="content-inner-block-10">
                    <div class="content-inner-block-10 margin-bottom5">
                        <div class="model-review-image inline-block">
                            <a href="/m/<%=  objReview.BikeEntity.MakeEntity.MakeName%>-bikes/<%= objReview.BikeEntity.ModelEntity.ModelName %>/" title=" <%=  objReview.BikeEntity.MakeEntity.MakeName + " " + objReview.BikeEntity.ModelEntity.ModelName %>">
                                <img alt="<%=  objReview.BikeEntity.MakeEntity.MakeName + " " + objReview.BikeEntity.ModelEntity.ModelName %> Reviews" title=" <%=  objReview.BikeEntity.MakeEntity.MakeName + " " + objReview.BikeEntity.ModelEntity.ModelName %> Reviews" src="<%= Bikewale.Common.MakeModelVersion.GetModelImage( objReview.HostUrl, objReview.OriginalImagePath,Bikewale.Utility.ImageSize._110x61) %>">
                            </a>
                        </div>
                       <div class="model-review-details inline-block">
                            <h2 class="font14 margin-bottom5"><a href="/m/<%= objReview.BikeEntity.MakeEntity.MaskingName%>-bikes/<%= objReview.BikeEntity.ModelEntity.MaskingName%>/" title="<%= string.Format("{0} {1}",objReview.BikeEntity.MakeEntity.MakeName,objReview.BikeEntity.ModelEntity.ModelName)%>" class="text-default"><%=  objReview.BikeEntity.MakeEntity.MakeName + " " + objReview.BikeEntity.ModelEntity.ModelName%></a></h2>
                           <%if(objReview.New) {%> 
                           <p class="font11 text-light-grey">Ex-showroom, <%=Bikewale.Utility.BWConfiguration.Instance.DefaultName %></p>
                           <%}else{ %>
                            <p class="font11 text-light-grey">Last known Ex-showroom price</p>
                           <%} %>
                            <span class="bwmsprite inr-xsm-icon"></span>
                            <span class="font16 text-bold"><%=Bikewale.Utility.Format.FormatPrice(Convert.ToString(objReview.ModelBasePrice)) %></span>
                        </div>
                    </div>
                  <%if (objReview.ReviewRatingEntity != null )
                    { %>  <div class="border-solid rating-box-container display-table">
                        <div class="rating-box overall text-center">
                           <%if(objReview.ReviewRatingEntity.OverAllRating>0) {%> <p class="text-bold font14 margin-bottom10">Overall Rating</p>
                            <div class="margin-bottom10">
                                <span class="star-one-icon"></span>
                                <div class="inline-block">
                                    <span class="font20 text-bold"><%=Math.Round(objReview.ReviewRatingEntity.OverAllRating,1) %></span>
                                    <span class="padding-left2 font12 text-light-grey">/ 5</span>
                                </div>
                            </div>
                        </div><div class="rating-category-list-container content-inner-block-10 star-icon-sm">
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
                    </div><%} %>
                </div>

              <%if(objReview.ReviewEntity!=null) %>  <div class="padding-right20 padding-left20 margin-top10 font14 padding-bottom10">
                    <h2 class="review-category-title">Good about this bike</h2>
                    <p class="margin-bottom20"><%=  objReview.ReviewEntity.Pros %></p>

                    <h2 class="review-category-title">Not so good about this bike</h2>
                    <p class="margin-bottom20"><%=  objReview.ReviewEntity.Cons %></p>

                    <h2 class="review-category-title">Full Review</h2>
                    <div class="full-review-content">
                        <%= objReview.ReviewEntity.Comments %>
                    </div>

                    <div class="inline-block margin-right15">
                        <span class="bwmsprite views-grey-sm-icon"></span>
                        <span class="font12 text-light-grey"><%=objReview.ReviewEntity.Viewed%></span>
                    </div>
                    <div class="inline-block">
                        <div class="inline-block margin-right15 cur-pointer">
                            <span class="bwmsprite like-icon"></span>
                            <span id="spnLiked" class="font12 text-light-grey"><%=objReview.ReviewEntity.Liked%></span>
                        </div>
                        <div class="inline-block cur-pointer">
                            <span class="bwmsprite dislike-icon"></span>
                            <span  id="spnDisliked" class="font12 text-light-grey"><%= objReview.ReviewEntity.Disliked %></span>
                        </div>
                    </div>

                    <p id="divAbuse" class="font14 text-light-grey margin-top10 margin-bottom10">Inappropriate Review? <a onclick='javascript:abuseClick(<%= reviewId %>)' id="report-abuse-target">Report Abuse</a></p>

                    <div id="divHelpful" class="border-solid-top padding-top10 text-light-grey"><span class="margin-right10 feedback-label">Was this review helpful to you?</span><a onclick='javascript:helpfulClick(<%= reviewId %>, "1")' class="btn-transparent-sm margin-right5">Yes</a> <a onclick='javascript:helpfulClick(<%= reviewId %>, "0")' class="btn-transparent-sm">No</a></div>
                </div>   <%} %>             
            </div>
        </section><%} %>

        <section>
            <div class="container box-shadow bg-white section-bottom-margin padding-15-20 font14">
                  <BW:GenericBikeInfo ID="ctrlGenericBikeInfo" runat="server" />
       </div>
            <div class="container box-shadow bg-white section-bottom-margin padding-15-20 font14">
       
                   <%if (ctrlUserReviews.FetchedRecordsCount > 0)
                    { %>
                <BW:UserReviews runat="server" ID="ctrlUserReviews" />
                <% } %>
            </div>
        </section>
          <%if (ctrlUserReviewSimilarBike.FetchCount > 0)
          { %>
        <section>
            <div class="container bg-white box-shadow padding-bottom15 section-bottom-margin">
                <div class="carousel-heading-content padding-top15">
                <div class="swiper-heading-left-grid inline-block">
                <h2>Reviews of similar bikes</h2>
                </div>
                    <div class="swiper-heading-right-grid inline-block text-right">
            <a href="/m/user-reviews/" title="Bike User Reviews" class="btn view-all-target-btn">View all</a>
                </div>    
                    <div class="clear"></div>            
                    </div>
                 <BW:UserReviewSimilarBike ID="ctrlUserReviewSimilarBike" runat="server" />
            </div>
        </section>
<%} %>
        <div id="report-abuse-popup">
            <p class="font14 text-bold text-center margin-bottom10">Why do you want to report it abuse?</p>
            <div class="report-abuse-close-btn position-abt pos-top10 pos-right10 bwmsprite cross-lg-lgt-grey cur-pointer"></div>
            <div class="">
                <p class="margin-bottom10">Comment</p>
                <textarea id="txtAbuseComments"></textarea>
                <div class="text-center">
                    <button onclick="javascript:reportAbuse()" type="button" class="btn btn-orange">Report</button>
                </div>
            </div>
        </div>

        <div id="popup-background"></div>
        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : "" %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>

        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <link href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <!-- #include file="/includes/footerscript_mobile.aspx" -->
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
                    abuseClick("<%=reviewId%>")
                }
            });
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
                            location.href = "/users/login.aspx?returnUrl=/m/content/userreviews/reviewdetails.aspx?rid=" + reviewId+"&hash=report-abuse-popup";
                        }
                    }
                });
            }

            function reportAbuse() {
                var isError = false;

                if ($("#txtAbuseComments").val().trim() == "") {
                    $("#spnAbuseComments").html("Comments are required");
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
            $('#report-abuse-target').on('click', function () {
                reportAbusePopup.open();
                appendState('reportPopup');
            });

            $('.report-abuse-close-btn').on('click', function () {
                reportAbusePopup.close();
                history.back();
            });

            var reportAbusePopup = {
                open: function () {
                    $('#report-abuse-popup').show();
                    $('body').addClass('lock-browser-scroll');
                    $('#popup-background').show();
                },

                close: function () {
                    $('#report-abuse-popup').hide();
                    $('body').removeClass('lock-browser-scroll');
                    $('#popup-background').hide();
                }
            };

            /* popup state */
            var appendState = function (state) {
                window.history.pushState(state, '', '');
            };

            $(window).on('popstate', function (event) {
                if ($('#report-abuse-popup').is(':visible')) {
                    reportAbusePopup.close();
                }
            });
        </script>
        <!-- #include file="/includes/fontBW_Mobile.aspx" -->
    </form>
</body>
</html>
