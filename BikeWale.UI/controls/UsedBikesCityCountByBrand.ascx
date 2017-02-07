<%@ Control Language="C#" EnableViewState="false" AutoEventWireup="false" Inherits="Bikewale.Controls.UsedBikesCityCountByBrand" %>
<% if (viewModel != null && viewModel.BikesCountCityList != null && viewModel.BikesCountCityList.Count() > 0)
   { %>
<div class="jcarousel-wrapper inner-content-carousel city-model-carousel">
    <div class="jcarousel">
        <ul>
            <% foreach (var cityBikeCount in viewModel.BikesCountCityList)
               { %>
            <li>
                <a href="<%= string.Format("/used/{0}-bikes-in-{1}/", MakeMaskingName, cityBikeCount.CityMaskingName) %>" title="<%= string.Format("Used {0} bikes in {1}", MakeName, cityBikeCount.CityName) %>" class="card-target-block">
                    <div class="card-image-placeholder">
                        <span class="city-sm-sprite c<%= cityBikeCount.CityId %>-sm-icon"></span>
                    </div>
                    <div class="card-details-placeholder">
                        <h2 class="font14 text-truncate margin-bottom5"><%=cityBikeCount.CityName %></h2>
                        <p class="font14 text-light-grey text-truncate"><%=cityBikeCount.BikeCount%> used bikes</p>
                    </div>
                </a>
            </li>
            <% } %>
        </ul>
    </div>
    <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev" rel="nofollow"></a></span>
    <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next" rel="nofollow"></a></span>
</div>
<% } %>


