<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.UsedBikeModel" EnableViewState="false" %>
<%if (FetchCount > 0)
  { %>
<%if(!string.IsNullOrEmpty(header)){ %>
<h2 class="padding-15-20"><%= header %></h2>
<%} %>
<div class="content-box-shadow <%=IsLandingPage?"padding-bottom15":"padding-top20"%>">
    <div class="swiper-container card-container used-swiper">
        <div class="swiper-wrapper">
            <%foreach (var bikeDetails in UsedBikeModelInCityList)
              {%>
            <div class="swiper-slide">
                <div class="swiper-card">
                    <a href="/m<%=Bikewale.Utility.UrlFormatter.UsedBikesUrlNoCity(bikeDetails.MakeMaskingName,bikeDetails.ModelMaskingName,cityDetails!=null?cityDetails.CityMaskingName:"india") %>" title="<%=string.Format("Used {0} {1} bikes in {2}",bikeDetails.MakeName,bikeDetails.ModelName,cityDetails!=null?cityDetails.CityName:"India")%>">
                        <div class="swiper-image-preview">
                            <div class="image-thumbnail">
                                <img class="swiper-lazy" data-src="<%=Bikewale.Utility.Image.GetPathToShowImages(bikeDetails.UsedOriginalImagePath,bikeDetails.HostUrl,Bikewale.Utility.ImageSize._310x174) %>" alt="<%=string.Format("Used {0} {1} bikes in {2}",bikeDetails.MakeName,bikeDetails.ModelName,cityDetails!=null?cityDetails.CityName:"India")%>" />
                                <span class="swiper-lazy-preloader"></span>
                            </div>
                        </div>
                        <div class="swiper-details-block">
                            <h3 class="target-link font12 margin-bottom5 text-truncate"><%=string.Format("{0} {1}",bikeDetails.MakeName,bikeDetails.ModelName)%></h3>
                            <div class="margin-bottom5 text-default">
                                <span class="bwmsprite inr-xsm-icon"></span>
                                <span class="font16 text-bold">
                                    <%=Bikewale.Utility.Format.FormatPrice(Convert.ToString(bikeDetails.BikePrice)) %>
                                </span>
                                <span class="font12">onwards</span>
                            </div>
                            <p class="font12">
                                <%=Bikewale.Utility.Format.FormatPrice(Convert.ToString(bikeDetails.AvailableBikes)) %> Bikes Available
                            </p>
                        </div>
                    </a>
                </div>
            </div>
            <%} %>
        </div>
    </div>
    <div class="view-all-btn-container margin-top10">
        <a class="btn view-all-target-btn" title="<%=WidgetTitle%>" href="<%=WidgetHref %>">View all used bikes<span class="bwmsprite teal-right"></span></a>
    </div>
</div>
<% } %>


