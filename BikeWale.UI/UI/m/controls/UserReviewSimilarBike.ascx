<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.UserReviewSimilarBike" EnableViewState="false" %>
<%if (userReviewList!=null){ %>
<div class="swiper-container padding-top5 padding-bottom5 user-reviews-type-carousel">
    <div class="swiper-wrapper">
        <%foreach (var UserDetails in userReviewList)
          { %>
        <div class="swiper-slide">
            <div class="swiper-card">
                <a href="<%=string.Format("/m/{0}-bikes/{1}/user-reviews/",UserDetails.MakeMaksingName,UserDetails.ModelMaskingName)%>" title="<%=string.Format("{0} {1} user reviews",UserDetails.MakeName,UserDetails.ModelName) %>">
                    <div class="swiper-image-preview">
                        <img class="swiper-lazy" data-src="<%=Bikewale.Utility.Image.GetPathToShowImages(UserDetails.OriginalImagePath,UserDetails.HostUrl,Bikewale.Utility.ImageSize._310x174) %>" alt="<%=string.Format("{0} {1}",UserDetails.MakeName,UserDetails.ModelName) %>" src="" />
                        <span class="swiper-lazy-preloader"></span>
                    </div>
                    <div class="swiper-details-block">
                        <h3 class="font12 text-black margin-bottom10 text-truncate"><%=string.Format("{0} {1}",UserDetails.MakeName,UserDetails.ModelName) %></h3>
                        <ul class="bike-review-features">
                            <li>
                                <span class="star-one-icon"></span>
                                <span class="font16 text-bold text-default"><%=Math.Round(UserDetails.OverAllRating,1) %></span><span class="font12 text-default"> / 5</span>
                            </li>
                            <li class="font14"><%=UserDetails.ReviewCounting+(UserDetails.ReviewCounting>1? " Reviews":" Review")%></li>
                        </ul>
                    </div>
                </a>
            </div>
        </div>
        <%} %>
    </div>
</div>
<%} %>