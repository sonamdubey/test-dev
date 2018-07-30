<%@ Control Language="C#" AutoEventWireup="false"  Inherits="Bikewale.Controls.UserReviewSimilarBike" EnableViewState="false" %>
<%if (userReviewList!=null)
  { %>
                <ul class="sidebar-bike-list">
                    <%foreach(var BikeDetails in userReviewList){ %>
                    <li>
                        <a href="<%=string.Format("/{0}-bikes/{1}/reviews/",BikeDetails.MakeMaksingName,BikeDetails.ModelMaskingName)%>" title="<%=string.Format("{0} {1} user reviews",BikeDetails.MakeName,BikeDetails.ModelName) %>" class="bike-target-link text-default">
                            <div class="bike-target-image inline-block">
                                <img src="<%= Bikewale.Utility.Image.GetPathToShowImages(BikeDetails.OriginalImagePath,BikeDetails.HostUrl,Bikewale.Utility.ImageSize._310x174) %>" alt="<%=string.Format("{0} {1}",BikeDetails.MakeName,BikeDetails.ModelName) %>">
                            </div>
                            <div class="bike-target-content inline-block padding-left10">
                                <h3><%=string.Format("{0} {1}",BikeDetails.MakeName,BikeDetails.ModelName) %></h3>
                                <ul class="bike-review-features margin-top5">
                                    <li>
                                        <span class="star-one-icon"></span>
                                        <span class="font16 text-bold text-default"><%=Math.Round(BikeDetails.OverAllRating,1) %></span><span class="font12 text-default"> / 5</span>
                                    </li>
                                    <li class="font14"><%=BikeDetails.ReviewCounting+(BikeDetails.ReviewCounting>1? " Reviews":" Review")%></li>
                                </ul>
                            </div>
                        </a>
                    </li>        
                       <%} %>     
                </ul>
    <%} %>
              