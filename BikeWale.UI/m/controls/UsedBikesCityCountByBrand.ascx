<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.m.controls.UsedBikesCityCountByBrand" %>

<% if (viewModel != null && viewModel.bikesCountCityList != null)
   { %>
<span id="close-city-model-carousel" class="bwmsprite cross-md-dark-grey cur-pointer"></span>

<div class="swiper-container card-container city-model-carousel">
    <div class="swiper-wrapper">
        <% foreach (var cityBikeCount in viewModel.bikesCountCityList)
           { %>
        <div class="swiper-slide">
            <div class="swiper-card">
                <a href="<%= string.Format("/m/used/{0}-bikes-in-{1}/", makeMaskingName, cityBikeCount.CityMaskingName) %>" title="<%= string.Format("Used {0} bikes in {1}", makeName, cityBikeCount.CityName) %>" class="card-target-block">
                    <div class="card-image-placeholder">
                        <span class="city-sm-sprite pune-sm-icon"></span>
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