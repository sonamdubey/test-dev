<%@ Control Language="C#" AutoEventWireup="false"  Inherits="Bikewale.Mobile.Controls.UsedBikeInCities" EnableViewState="false" %>
 <% if (objCitiesWithCount != null)
    { %>
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
<div class="padding-left10 view-all-btn-container  margin-top15">
                    <a href="<%=WidgetHref %>" title="<%=WidgetTitle %>" class="btn view-all-target-btn inv-teal-sm margin-top10">View all used bikes<span class="bwmsprite teal-right"></span></a>
               </div>
            </div>
            
        <% } %>