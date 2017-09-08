﻿<%@ Control Language="C#" AutoEventWireup="false" EnableViewState="false" Inherits="Bikewale.Mobile.Controls.SimilarBikeWithPhotos" %>
<% if(FetchedRecordsCount > 0) { %>
<section>
    <div class="image-swiper container bg-white box-shadow section-bottom-margin">
        <div class="carousel-heading-content padding-15-20">
            <h2 class="swiper-heading-left-grid inline-block"><%=WidgetHeading%></h2>
        </div>
        <div class="swiper-container card-container alternate-bikes-photo-swiper">
            <div class="swiper-wrapper">
                <% foreach(var bike in objSimilarBikes) { string bikeName = string.Format("{0} {1}",bike.Make.MakeName,bike.Model.ModelName); %>
                <div class="swiper-slide">

                    <div class="swiper-card">
                         <a href="/m/<%= bike.Make.MaskingName %>-bikes/<%= bike.Model.MaskingName %>" title="<%= bikeName %>" class="block swiper-card-target">
                            <div class="swiper-image-preview">
                                <img class="swiper-lazy" data-src="<%= Bikewale.Utility.Image.GetPathToShowImages(bike.OriginalImagePath,bike.HostUrl,Bikewale.Utility.ImageSize._160x89) %>" alt="<%= bikeName %> images" src="" />
                            </div>
                                <div class="swiper-details-block">
                                    <h3 class="target-link font12 text-truncate margin-bottom5"><%= bikeName %></h3>


                                        <% string city = null; string showText = null;uint price = 0;
                                                    if (CityId > 0 && bike.OnRoadPriceInCity > 0) {  city = City;  showText = "On-Road Price,";  price = bike.OnRoadPriceInCity; }
                                                    else { city = "Mumbai";  showText = "Ex-showroom,";  price =  bike.ExShowroomPriceMumbai;  }
                                                  %>   
                                                <p class="text-truncate text-light-grey font11"><%= showText %>&nbsp;<%= city %></p>
                                                <p class="text-default font16 margin-bottom15">
                                                    &#x20b9;&nbsp;<span class="text-bold"><%= price %></span>&nbsp;<span class="font14">onwards</span>
                                                </p>
                                            </div>

                                </div>
                        </a>
                        <a href="/m/<%= bike.Make.MaskingName %>-bikes/<%= bike.Model.MaskingName %>/images/" title="<%= bikeName %> images" class="compare-with-target text-truncate">
                            <span class="bwmsprite photos-sm"></span><span class="margin-left5 font12 text-default">View all&nbsp<%= bike.PhotosCount %> images</span><span class="bwmsprite right-arrow"></span>
                        </a>
                    </div>
                               
                </div>
                 <% } %>
            </div>
        </div>
    </div>
</section>
<% } %>
