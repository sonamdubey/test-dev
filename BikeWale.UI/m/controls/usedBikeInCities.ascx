<%@ Control Language="C#" AutoEventWireup="false"  Inherits="Bikewale.Mobile.Controls.UsedBikeInCities" EnableViewState="false" %>
 <% if (objCitiesWithCount != null)
    { %>
        <section>
            <div class="container text-center section-container">
                <h2 class="font18 section-heading">Search used bikes by cities</h2>
                <div class="bg-white box-shadow padding-top20 padding-bottom20">
                    <div class="swiper-container card-container swiper-city">
                        <div class="swiper-wrapper">
                                   <%foreach (var objCity in objCitiesWithCount)
                                     {%>
                                        <div class="swiper-slide">
                                <div class="swiper-card">
                                    <a href="/m/used/bikes-in-<%=objCity.CityMaskingName %>/" title="Used bikes in <%=objCity.CityName %>">
                                        <div class="swiper-image-preview">
                                            <span class="city-sprite c<%=objCity.CityId %>-icon"></span>
                                        </div>
                                        <div class="swiper-details-block">
                                            <h3 class="font14 margin-bottom5"><%=objCity.CityName %></h3>
                                            <p class="font14 text-light-grey"><%=objCity.BikesCount %> Used bikes</p>
                                        </div>
                                    </a>
                                </div>
                            </div>
                                    <%} %>
                         </div>
                    </div>
                    <a href="/m/used/browse-bikes-by-cities/" class="btn btn-inv-teal inv-teal-sm margin-top10">View all cities<span class="bwmsprite teal-next"></span></a>
                </div>
            </div>
        </section>
        <% } %>
