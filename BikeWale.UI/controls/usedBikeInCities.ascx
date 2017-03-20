﻿<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.UsedBikeInCities" EnableViewState="false"%>
        <% if(objCitiesWithCount != null) { %>
     
                    <div id="city-jcarousel" class="content-box-shadow padding-top20 padding-bottom20">
                        <div class="jcarousel-wrapper inner-content-carousel">
                            <div class="jcarousel">
                                <ul>
                                    <%foreach (Bikewale.Entities.Used.UsedBikeCities objCity in objCitiesWithCount)
                                        {%>
                                        <li>
                                            <a href="/used/bikes-in-<%=objCity.CityMaskingName %>/" title="Used bikes in <%=objCity.CityName %>" class="city-card-target">
                                                <div class="city-image-preview">
                                                    <span class="city-sprite c<%=objCity.CityId %>-icon"></span>
                                                </div>
                                                <div class="text-left font14 padding-left20 padding-right20 padding-bottom25">
                                                    <p class="text-default text-bold margin-bottom5"><%=objCity.CityName %></p>
                                                    <p class="text-light-grey"><%=objCity.BikesCount %> Used bikes</p>
                                                </div>
                                            </a>
                                        </li>
                                    <%} %>
                                </ul>
                             </div>
                            <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev" rel="nofollow"></a></span>
                            <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next" rel="nofollow"></a></span>
                        </div>
                        <div class="more-article-target view-all-btn-container padding-top20"> 
                            <a href="/used/browse-bikes-by-cities/" title="Browse used bike by cities" class="btn view-all-target-btn">View all used bikes<span class="bwsprite teal-next"></span></a>
                        </div>
                    </div>
               
        <% } %>