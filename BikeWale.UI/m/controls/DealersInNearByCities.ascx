<%@ Control Language="C#" Inherits="Bikewale.m.Controls.DealersInNearByCities" %>

<%if (DealerCountCityList != null)
  { %>
<div class="container bg-white box-shadow padding-top15 padding-bottom20">
    <h2 class="padding-right20 padding-bottom15 padding-left20">Explore <%= MakeName %> showrooms in cities near <%= CityName %></h2>
    <div class="swiper-container padding-top5 padding-bottom5 map-type-carousel">
        <div class="swiper-wrapper">
            <% foreach (var Dealer in DealerCountCityList)
              { %>
            <div class="swiper-slide">
                <div class="swiper-card">
                    <a href="/m/dealer-showrooms/<%= MakeMaskingName %>/<%= Dealer.CityMaskingName %>/" title="<%= MakeName %> showrooms in <%= Dealer.CityName %>">
                        <div class="swiper-lazy dealer-location-img" data-background="<%= Dealer.GoogleMapImg %>" title="<%= MakeName %> showrooms in <%= Dealer.CityName %>" ></div>
                        <div class="swiper-details-block">
                            <p class="text-bold text-black font12 margin-bottom5 padding-top5"><%= Dealer.CityName%></p>
                            <h3 class="text-unbold text-light-grey font11"><%= String.Format("{0} {1}", Dealer.DealersCount, MakeName)%> showroom<%if(Dealer.DealersCount > 1){%>s<%}%></h3>
                        </div>
                    </a>
                </div>
            </div>
            <%} %>
        </div>
    </div>
</div>
<%} %>

