﻿<%@ Page Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Content.ListReviews" %>

<%@ Register TagPrefix="BikeWale" TagName="Pager" Src="~/m/controls/LinkPagerControl.ascx" %>
<%@ Register TagPrefix="BW" TagName="UserReviewSimilarBike" Src="~/m/controls/UserReviewSimilarBike.ascx" %>

<!DOCTYPE html>
<html>
<head>
    <%  title = "User Reviews: " + objModelEntity.MakeBase.MakeName + " " + objModelEntity.ModelName;
        description = objModelEntity.MakeBase.MakeName + " " + objModelEntity.ModelName + " User Reviews - Read first-hand reviews of actual " + objModelEntity.MakeBase.MakeName + " " + objModelEntity.ModelName + " owners. Find out what buyers of " + objModelEntity.MakeBase.MakeName + " " + objModelEntity.ModelName + " have to say about the bike.";
        keywords = objModelEntity.MakeBase.MakeName + " " + objModelEntity.ModelName + " reviews, " + objModelEntity.MakeBase.MakeName + " " + objModelEntity.ModelName + " Users Reviews, " + objModelEntity.MakeBase.MakeName + " " + objModelEntity.ModelName + " customer reviews, " + objModelEntity.MakeBase.MakeName + " " + objModelEntity.ModelName + " customer feedback, " + objModelEntity.MakeBase.MakeName + " " + objModelEntity.ModelName + " owner feedback, user bike reviews, owner feedback, consumer feedback, buyer reviews";
        canonical = "https://www.bikewale.com/" + objModelEntity.MakeBase.MaskingName + "-bikes/" + objModelEntity.MaskingName + "/user-reviews";
        relPrevPageUrl = String.IsNullOrEmpty(prevPageUrl) ? "" : "https://www.bikewale.com" + prevPageUrl;
        relNextPageUrl = String.IsNullOrEmpty(nextPageUrl) ? "" : "https://www.bikewale.com" + nextPageUrl;
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

        <section>
            <div class="container box-shadow bg-white section-bottom-margin">
                <h1 class="box-shadow padding-15-20"><%= objModelEntity.MakeBase.MakeName  + " " + objModelEntity.ModelName%>  User reviews</h1>
                <div class="content-inner-block-10">
                    <div class="content-inner-block-10 margin-bottom5">
                        <div class="model-review-image inline-block">
                            <a href="" title="">
                                <img alt="<%= objModelEntity.MakeBase.MakeName + " " + objModelEntity.ModelName %> Reviews" title="<%= objModelEntity.MakeBase.MakeName + " " + objModelEntity.ModelName %> Reviews" src="<%= Bikewale.Common.MakeModelVersion.GetModelImage( objModelEntity.HostUrl, objModelEntity.OriginalImagePath,Bikewale.Utility.ImageSize._110x61) %>">
                            </a>
                        </div>
                        <div class="model-review-details inline-block">
                            <h2 class="font14 margin-bottom5"><a href="/m/<%=  objModelEntity.MakeBase.MaskingName %>-bikes/<%= objModelEntity.MaskingName %>/" title="<%= string.Format("{0} {1}",objModelEntity.MakeBase.MakeName,objModelEntity.ModelName)%>" class="text-default"><%= objModelEntity.MakeBase.MakeName  + " " + objModelEntity.ModelName%></a></h2>
                           <%if(objModelEntity.New){ %>
                             <p class="font11 text-light-grey">Ex-showroom, <%=Bikewale.Utility.BWConfiguration.Instance.DefaultName %></p>
                            <%}else{ %>
                            <p class="font11 text-light-grey">Last known price in, <%=Bikewale.Utility.BWConfiguration.Instance.DefaultName %></p>
                            <%} %>
                            <span class="bwmsprite inr-xsm-icon"></span>
                            <span class="font16 text-bold"><%=Bikewale.Utility.Format.FormatPrice(Convert.ToString(objModelEntity.MinPrice)) %></span>
                        </div>
                    </div>
                    <%if (objRating.OverAllRating > 0 || objRating.ModelRatingLooks > 0 || objRating.FuelEconomyRating > 0 || objRating.PerformanceRating > 0 || objRating.ValueRating > 0 || objRating.ComfortRating > 0)
                      { %>
                    <div class="border-solid rating-box-container display-table">
                        <div class="rating-box overall text-center">
                            <p class="text-bold font14 margin-bottom10">Overall Rating</p>
                            <%if (objRating.OverAllRating > 0)
                              { %><div class="margin-bottom10">
                                      <span class="star-one-icon"></span>
                                      <div class="inline-block">
                                          <span class="font20 text-bold"><%=Math.Round(objRating.OverAllRating,1) %></span>
                                          <span class="padding-left2 font12 text-light-grey">/ 5</span>
                                      </div>
                                  </div>
                            <%} %>
                            <p class="font14 text-light-grey"><%= totalReviews %> Reviews</p>
                        </div>
                        <div class="rating-category-list-container content-inner-block-10 star-icon-sm">
                            <ul class="rating-category-list">
                                <%if (objRating.ModelRatingLooks > 0)
                                  { %>
                                <li>
                                    <span class="rating-category-label">Looks</span><span>
                                        <span class="star-one-icon"></span>
                                        <span class="text-bold"><%=Math.Round(objRating.ModelRatingLooks,1) %></span>
                                        <span class="font12">/ 5</span>
                                    </span>
                                </li>
                                <%} %>
                                <%if (objRating.FuelEconomyRating > 0)
                                  {%>
                                <li>
                                    <span class="rating-category-label">Fuel Economy</span><span>
                                        <span class="star-one-icon"></span>
                                        <span class="text-bold"><%=Math.Round(objRating.FuelEconomyRating,1) %></span>
                                        <span class="font12">/ 5</span>
                                    </span>
                                </li>
                                <%} %>
                                <%if (objRating.PerformanceRating > 0)
                                  {%>
                                <li>
                                    <span class="rating-category-label">Performance</span><span>
                                        <span class="star-one-icon"></span>
                                        <span class="text-bold"><%=Math.Round(objRating.PerformanceRating,1) %></span>
                                        <span class="font12">/ 5</span>
                                    </span>
                                </li>
                                <%} %>
                                <%if (objRating.ValueRating > 0)
                                  {%>
                                <li>
                                    <span class="rating-category-label">Value for Money</span><span>
                                        <span class="star-one-icon"></span>
                                        <span class="text-bold"><%=Math.Round(objRating.ValueRating,1) %></span>
                                        <span class="font12">/ 5</span>
                                    </span>
                                </li>
                                <%} %>
                                <%if (objRating.ComfortRating > 0)
                                  {%>
                                <li>
                                    <span class="rating-category-label">Space/Comfort</span><span>
                                        <span class="star-one-icon"></span>
                                        <span class="text-bold"><%=Math.Round(objRating.ComfortRating,1) %></span>
                                        <span class="font12">/ 5</span>
                                    </span>
                                </li>
                                <%} %>
                            </ul>
                            <div class="clear"></div>
                        </div>
                    </div>
                    <%} %>
                </div>
            </div>
        </section>

        <%if (totalReviews > 0)
          { %>
        <section>
            <div class="container box-shadow bg-white section-bottom-margin padding-top15 padding-right20 padding-left20">
                <h2><%= totalReviews + " " + objModelEntity.ModelName %> User reviews</h2>
                <ul class="model-user-review-list">
                    <%foreach (var UserReviews in objReviewList)
                      {%>
                    <li>
                        <div class="model-user-review-rating-container">
                            <p class="font16 text-bold"><%=Math.Round(UserReviews.OverAllRating.OverAllRating,1) %></p>
                            <p class="inline-block margin-bottom5 margin-top5">
                                <%=Bikewale.Utility.ReviewsRating.GetRateImage(UserReviews.OverAllRating.OverAllRating )%>
                            </p>
                        </div>
                        <div class="model-user-review-title-container">
                            <h3>
                                <a class="target-link margin-bottom7" href="/m/<%= objModelEntity.MakeBase.MaskingName %>-bikes/<%= objModelEntity.MaskingName %>/user-reviews/<%=UserReviews.ReviewId %>.html" title="<%=UserReviews.ReviewTitle %>"><%=UserReviews.ReviewTitle %></a>
                            </h3>
                            <div class="grid-7 alpha padding-right5">
                                <span class="bwmsprite calender-grey-sm-icon"></span>
                                <span class="article-stats-content"><%=UserReviews.ReviewDate %></span>
                            </div>
                            <div class="grid-5 alpha omega">
                                <span class="bwmsprite author-grey-sm-icon"></span>
                                <span class="article-stats-content"><%=UserReviews.WrittenBy %></span>
                            </div>
                            <div class="clear"></div>
                        </div>
                        <p class="font14 margin-top10"><%=Bikewale.Utility.FormatDescription.TruncateDescription(UserReviews.Comments,180) %></p>
                    </li>
                    <%} %>
                </ul>
                <div class="padding-top15 padding-bottom15 border-solid-top font14">
                    <div class="grid-5 alpha omega text-light-grey font13">
                        <span class="text-bold text-default"><%=startIndex %>-<%=Math.Min(endIndex,totalReviews) %></span> of <span class="text-bold text-default"><%=totalReviews %></span> reviews
                    </div>
                    <div class="clear"></div>
                       <BikeWale:Pager ID="ctrlPager" runat="server" />
                </div>
            </div>
        </section>
        <% } %>
        <%if (ctrlUserReviewSimilarBike.FetchCount > 0)
          { %>
        <section>
            <div class="container bg-white box-shadow padding-top15 padding-bottom15">
                <h2 class="padding-right20 padding-bottom15 padding-left20">User reviews of similar bikes</h2>
                <BW:UserReviewSimilarBike ID="ctrlUserReviewSimilarBike" runat="server" />
            </div>
        </section>
        <%} %>
        <script type="text/javascript" src="<%= staticUrl != "" ? "https://st1.aeplcdn.com" + staticUrl : "" %>/m/src/frameworks.js?<%= staticFileVersion %>"></script>

        <!-- #include file="/includes/footerBW_Mobile.aspx" -->
        <link href="<%= staticUrl != "" ? "https://st2.aeplcdn.com" + staticUrl : "" %>/m/css/bwm-common-btf.css?<%= staticFileVersion %>" rel="stylesheet" type="text/css" />
        <!-- #include file="/includes/footerscript_mobile.aspx" -->
        <!-- #include file="/includes/fontBW_Mobile.aspx" -->
    </form>
</body>
</html>

