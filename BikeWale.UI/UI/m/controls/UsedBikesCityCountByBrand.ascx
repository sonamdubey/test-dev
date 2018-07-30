<%@ Control Language="C#" EnableViewState="false" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.UsedBikesCityCountByBrand" %>

<% if (viewModel != null && viewModel.BikesCountCityList != null && viewModel.BikesCountCityList.Count() > 0)
   { %>
<div id="UsedBikesCityCountByBrand" class="swiper-container card-container city-model-carousel">
    <div class="swiper-wrapper">
        <% foreach (var cityBikeCount in viewModel.BikesCountCityList)
           { %>
        <div class="swiper-slide">
            <div class="swiper-card">
                <a href="<%= string.Format("/m/used/{0}-bikes-in-{1}/", MakeMaskingName, cityBikeCount.CityMaskingName) %>" title="<%= string.Format("Used {0} bikes in {1}", MakeName, cityBikeCount.CityName) %>" class="card-target-block">
                    <div class="card-image-placeholder">
                        <span class="city-sm-sprite c<%= cityBikeCount.CityId %>-sm-icon"></span>
                    </div>
                    <div class="card-details-placeholder">
                        <h2 class="font14 text-truncate margin-bottom5"><%=cityBikeCount.CityName %></h2>
                        <p class="font14 text-light-grey text-truncate"><%=cityBikeCount.BikeCount%> used bikes</p>
                    </div>
                </a>
            </div>
        </div>
        <% } %>
    </div>
</div>
<% } %>