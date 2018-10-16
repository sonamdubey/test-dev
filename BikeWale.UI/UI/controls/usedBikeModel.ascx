<%@ Control Language="C#" AutoEventWireup="false"  Inherits="Bikewale.Controls.UsedBikeModel" EnableViewState="false"%>
<%if (FetchCount > 0)
  { %>
<%if(!IsLandingPage){ %>
        <div class="carousel-heading-content <%=IsLandingPage?"":"padding-top20" %>">
            <div class="swiper-heading-left-grid inline-block">
                <h2><%= header %></h2>
            </div><div class="swiper-heading-right-grid inline-block text-right">
                <a href="<%= WidgetHref %>" title="<%= WidgetTitle %>" class="btn view-all-target-btn">View all</a>
            </div>
          
            <div class="clear"></div>
        </div>
  <%} %>
        <div class="padding-bottom20">
            <div class="jcarousel-wrapper inner-content-carousel">
                <div class="jcarousel">
                    <ul>
                        <%foreach (var bikeDetails in UsedBikeModelInCityList) {%>
                            <li>
                                <a href="<%=Bikewale.Utility.UrlFormatter.UsedBikesUrlNoCity(bikeDetails.MakeMaskingName,bikeDetails.ModelMaskingName,cityDetails!=null && !string.IsNullOrEmpty(cityDetails.CityMaskingName) ?cityDetails.CityMaskingName:"india") %>" title="<%= string.Format("Used {0} {1} bikes in {2}", bikeDetails.MakeName, bikeDetails.ModelName, (cityDetails != null ? cityDetails.CityName : "India"))%>" class="jcarousel-card">
                                    <div class="model-jcarousel-image-preview">
                                        <div class="card-image-block">
                                            <img class="lazy" src="" data-original="<%=Bikewale.Utility.Image.GetPathToShowImages(bikeDetails.UsedOriginalImagePath,bikeDetails.UsedHostUrl,Bikewale.Utility.ImageSize._310x174) %>" alt="<%=string.Format("Used {0} {1} bikes in {2}",bikeDetails.MakeName,bikeDetails.ModelName,cityDetails!=null?cityDetails.CityName:"India") %>">
                                        </div>
                                    </div>
                                    <div class="card-desc-block text-truncate">
                                        <h3 class="bikeTitle"><%= string.Format("Used {0} {1}",bikeDetails.MakeName,bikeDetails.ModelName)%></h3>
                                        <p class="text-light-grey margin-bottom5">
                                            <%=Bikewale.Utility.Format.FormatPrice(Convert.ToString(bikeDetails.AvailableBikes)) %> Used bikes available
                                        </p>
                                        <div class="text-bold text-default">
                                            <span class="bwsprite inr-lg"></span>&nbsp;
                                            <span class="font18"><%=Bikewale.Utility.Format.FormatPrice(Convert.ToString(bikeDetails.BikePrice)) %></span>&nbsp;
                                                <span class="font14">onwards</span>
                                        </div>
                                    </div>
                                </a>
                            </li>
                        <%} %>
                    </ul>
                </div>
                <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev" rel="nofollow"></a></span>
                <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next" rel="nofollow"></a></span>
            </div>
          
        </div>
   <%if(IsLandingPage) {%>
        <div class="more-article-target view-all-btn-container"> 
            <a href="<%= WidgetHref %>" title="<%= WidgetTitle %>" class="btn view-all-target-btn">View all used bikes<span class="bwsprite teal-right"></span></a>
        </div>
    <%} %>
<%} %>
