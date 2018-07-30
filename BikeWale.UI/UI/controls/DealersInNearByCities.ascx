<%@ Control Language="C#" Inherits="Bikewale.Controls.DealersInNearByCities" %>

<% if (DealerCountCityList != null)
   {%>
<div class="container section-container ">
    <div class="grid-12 margin-bottom20">
        <div class="content-box-shadow padding-top20 padding-bottom20">
            <h2 class="font18 padding-bottom20 padding-left20">Explore <%= MakeName %> showrooms in cities near <%= CityName %></h2>
            <div class="jcarousel-wrapper inner-content-carousel map-type-carousel">
                <div class="jcarousel">
                    <ul>
                        <% foreach (var Dealer in DealerCountCityList)
                          { %>
                        <li>
                            <a href="/dealer-showrooms/<%= MakeMaskingName %>/<%= Dealer.CityMaskingName %>/" title="<%= MakeName %> showrooms in <%= Dealer.CityName %>" class="jcarousel-card">
                                <div class="lazy dealer-location-img" data-original="<%= Dealer.GoogleMapImg %>" title="<%= MakeName %> showrooms in <%= Dealer.CityName %>" ></div>
                                <div class="card-desc-block">
                                    <p class="card-heading font14 text-bold text-black padding-top5"><%= Dealer.CityName %></p>
                                    <h3 class="text-unbold text-light-grey"><%= String.Format("{0} {1}", Dealer.DealersCount,MakeName)%> showroom<%if(Dealer.DealersCount > 1){%>s<%}%></h3>
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
</div>
<% } %>