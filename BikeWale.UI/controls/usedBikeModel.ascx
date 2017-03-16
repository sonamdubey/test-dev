<%@ Control Language="C#" AutoEventWireup="false"  Inherits="Bikewale.Controls.usedBikeModel" EnableViewState="false"%>
<%if (FetchCount > 0)
  { %>
<div class="container padding-top20">
    <div class="grid-12 ">
        <div class="carousel-heading-content">
            <div class="swiper-heading-left-grid inline-block">
                <h2><%= header %></h2>
            </div><div class="swiper-heading-right-grid inline-block text-right">
                <a href="<%= WidgetHref %>" title="<%= WidgetTitle %>" class="btn view-all-target-btn">View all</a>
            </div>
            <div class="clear"></div>
        </div>
        <div class="padding-bottom20">
            <div class="jcarousel-wrapper inner-content-carousel">
                <div class="jcarousel">
                    <ul>
                        <%foreach (var bikeDetails in UsedBikeModelInCityList) {%>
                                <li>
                                    <a href="<%=Bikewale.Utility.UrlFormatter.UsedBikesUrlNoCity(bikeDetails.MakeMaskingName,bikeDetails.ModelMaskingName,cityDetails!=null?cityDetails.CityMaskingName:"india") %>" title="<%=string.Format("Used {0} {1} bikes in {2}",bikeDetails.MakeName,bikeDetails.ModelName,cityDetails!=null?cityDetails.CityName:"India")%>" class="jcarousel-card">
                                    <div class="model-jcarousel-image-preview">
                                        <div class="card-image-block">
                                            <img class="lazy" src="" data-original="<%=Bikewale.Utility.Image.GetPathToShowImages(bikeDetails.UsedOriginalImagePath,bikeDetails.UsedHostUrl,Bikewale.Utility.ImageSize._310x174) %>" alt="<%=string.Format("Used {0} {1} bikes in {2}",bikeDetails.MakeName,bikeDetails.ModelName,cityDetails!=null?cityDetails.CityName:"India") %>">
                                        </div>
                                    </div>
                                    <div class="card-desc-block">
                                        <h3 class="bikeTitle"><%=string.Format("{0} {1}",bikeDetails.MakeName,bikeDetails.ModelName)%></h3>
                                        <div class="text-bold text-default margin-bottom10">
                                            <span class="bwsprite inr-lg"></span>
                                            <span class="font18"><%=Bikewale.Utility.Format.FormatPrice(Convert.ToString(bikeDetails.BikePrice)) %></span>
                                              <span class="font14">onwards</span>
                                        </div>
                                        <p class="font16">
                                            <%=Bikewale.Utility.Format.FormatPrice(Convert.ToString(bikeDetails.AvailableBikes)) %> Bikes Available
                                        </p>
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
    </div>
    <div class="clear"></div>
</div>
<%} %>
