<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.UsedBikeModel" EnableViewState="false" %>
<%if (FetchCount > 0)
  { %>
<%if (!string.IsNullOrEmpty(header) && !IsLandingPage)
  { %>
<div class="carousel-heading-content">
    <div class="swiper-heading-left-grid inline-block">
        <h2><%= header %></h2>
    </div>
    <div class="swiper-heading-right-grid inline-block text-right">
        <a href="<%=WidgetHref %>" title="<%=WidgetTitle%>" class="btn view-all-target-btn">View all</a>
    </div>
    <div class="clear"></div>
</div>
<%} %>
<div class="content-box-shadow padding-bottom20">
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
                            <h3 class="target-link font12 margin-bottom5 text-truncate"><%=string.Format("Used {0} {1}",bikeDetails.MakeName,bikeDetails.ModelName)%></h3>
                            <p class="font13 text-light-grey">
                                <%=Bikewale.Utility.Format.FormatPrice(Convert.ToString(bikeDetails.AvailableBikes)) %> Used bikes available
                            </p>
                            <div class="margin-bottom5 text-default">
                                <span class="bwmsprite inr-xsm-icon"></span>&nbsp;
                                <span class="font16 text-bold">
                                    <%=Bikewale.Utility.Format.FormatPrice(Convert.ToString(bikeDetails.BikePrice)) %>
                                </span>&nbsp;
                                <span class="font12">onwards</span>
                            </div>

                        </div>
                    </a>
                </div>
            </div>
            <%} %>
        </div>
    </div>
    <%if (IsLandingPage)
      { %>
    <div class="padding-left10 view-all-btn-container margin-top15">
        <a href="<%=WidgetHref%>" title="<%=WidgetTitle%>" class="btn view-all-target-btn">View all used bikes<span class="bwmsprite teal-right"></span></a>
    </div>
    <%} %>
</div>
<% } %>