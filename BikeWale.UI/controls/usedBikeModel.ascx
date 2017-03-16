<%@ Control Language="C#" AutoEventWireup="false"  Inherits="Bikewale.Controls.usedBikeModel" EnableViewState="false"%>
<%if (FetchCount > 0)
  { %>
<div class="container">
    <div class="grid-12 ">
        <h2 class="padding-top10 padding-right10 padding-left10"><%= header %></h2>
        <div class="padding-top20 padding-bottom20">
            <div class="jcarousel-wrapper inner-content-carousel">
                <div class="jcarousel">
                    <ul>
                        <%foreach (var bikeDetails in UsedBikeModelInCityList) {%>
                            <li>
                                <a href="<%=Bikewale.Utility.UrlFormatter.UsedBikesUrlNoCity(bikeDetails.MakeMaskingName,bikeDetails.ModelMaskingName,cityDetails!=null?cityDetails.CityMaskingName:"india") %>" title="<%=string.Format("Used {0} {1} bikes in {2}",bikeDetails.MakeName,bikeDetails.ModelName,cityDetails!=null?cityDetails.CityName:"India")%>" class="jcarousel-card">
                                    <div class="model-jcarousel-image-preview">
                                        <div class="card-image-block">
                                            <img class="lazy" src="" data-original="<%=Bikewale.Utility.Image.GetPathToShowImages(bikeDetails.UsedOriginalImagePath,bikeDetails.UsedOriginalImagePath,Bikewale.Utility.ImageSize._310x174) %>" alt="<%=string.Format("Used {0} {1} bikes in {2}",bikeDetails.MakeName,bikeDetails.ModelName,cityDetails!=null?cityDetails.CityName:"India") %>">
                                        </div>
                                    </div>
                                    <div class="card-desc-block">
                                        <h3 class="bikeTitle"><%=string.Format("{0} {1}",bikeDetails.MakeName,bikeDetails.ModelName)%></h3>
                                        <p class="text-light-grey margin-bottom5">
                                            <%=Bikewale.Utility.Format.FormatPrice(Convert.ToString(bikeDetails.AvailableBikes)) %> Used bikes Available
                                        </p>
                                        <div class="text-bold text-default">
                                            <span class="bwsprite inr-lg"></span>
                                            <span class="font18"><%=Bikewale.Utility.Format.FormatPrice(Convert.ToString(bikeDetails.BikePrice)) %></span>
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
            <div class="view-all-btn-container margin-top15">
                <a class="btn view-all-target-btn" title="<%=WidgetTitle%>" href="<%=WidgetTitle %>">View complete list<span class="bwsprite teal-right"></span></a>
            </div>
        </div>
    </div>
    <div class="clear"></div>
</div>
<%} %>
