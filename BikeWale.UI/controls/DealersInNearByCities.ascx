<%@ Control Language="C#" Inherits="Bikewale.Controls.DealersInNearByCities" %>

<%if (DealerCountCityList != null)
  { %>
<style>
   .city-map{
   height: 100px;
   width : 290px;
   line-height: 0;
   display: table;
   text-align: center;
   }

#nearbyCities.jcarousel ul a div h3.bikeTitle {text-decoration:none;}
#carouselServiceCenter span.jcarousel-control-right,#carouselServiceCenter span.jcarousel-control-left {top: 31%;}
#card-block.card-desc-block { height: 80px; }
</style>
<% if (DealerCountCityList != null)
   {  %>
<div class="container section-container ">
    <div class="grid-12 margin-bottom20">
        <div class="content-box-shadow padding-top20 padding-bottom20">
            <h2 class="font18 padding-bottom20 padding-left20">Explore <%= MakeName %> showrooms in cities near <%= CityName %></h2>
            <div class="jcarousel-wrapper inner-content-carousel" id="carouselServiceCenter">
                <div id="nearbyCities" class="jcarousel" data-jcarousel="true">
                    <ul>
                        <% foreach (var Dealer in DealerCountCityList)
                          { %>
                        <li>
                            <a href="/<%= MakeMaskingName %>-dealer-showrooms-in-<%= Dealer.CityMaskingName %>/" title="<%= MakeName %> showrooms in <%= Dealer.CityName %>" class="jcarousel-card">
                                <div class="map_canvas city-map" data-lat="<%= Dealer.Lattitude %>" data-long="<%= Dealer.Longitude %>">
                                </div>
                                <div class="card-desc-block">
                                    <h3 class="bikeTitle padding-top15"><%= Dealer.CityName %></h3>

                                    <p class="font14 text-light-grey margin-bottom5"><%= Dealer.DealersCount %> <%= MakeName%> showroom<%if(Dealer.DealersCount > 1){%>s<%}%></p>

                                </div>
                            </a>
                        </li>
                        <%} %>
                    </ul>
                </div>
                <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev inactive" rel="nofollow"></a></span>
                <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next" rel="nofollow"></a></span>
            </div>
        </div>
    </div>
</div>
<% } %>
<%} %>


