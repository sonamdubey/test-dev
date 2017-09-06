<%@ Control Language="C#" AutoEventWireup="false" EnableViewState="false" Inherits="Bikewale.Mobile.Controls.SimilarBikeWithPhotos" %>
<% if(FetchedRecordsCount > 0) { %>
<section>
    <div class="image-swiper container bg-white box-shadow section-bottom-margin">
        <div class="carousel-heading-content padding-15-20">
            <h2 class="swiper-heading-left-grid inline-block">Images for alternative bikes</h2>
            <div class="swiper-heading-right-grid inline-block text-right">
                <a href="" title="" class="btn view-all-target-btn">View all</a>
            </div>
        </div>
        <div class="swiper-container card-container alternate-bikes-photo-swiper">
            <div class="swiper-wrapper">
                <% foreach(var bike in objSimilarBikes) { string bikeName = string.Format("{0} {1}",bike.Make.MakeName,bike.Model.ModelName); %>
                <div class="swiper-slide">

                    <div class="swiper-card">
                         <a href="/m/<%= bike.Make.MaskingName %>-bikes/<%= bike.Model.MaskingName %>/images/" title="<%= bikeName %> images" class="block swiper-card-target">
                            <div class="swiper-image-preview">
                                <img class="swiper-lazy" data-src="<%= Bikewale.Utility.Image.GetPathToShowImages(bike.OriginalImagePath,bike.HostUrl,Bikewale.Utility.ImageSize._160x89) %>" alt="<%= bikeName %> images" src="" />
                            </div>
                                <div class="swiper-details-block">
                                    <h3 class="target-link font12 text-truncate margin-bottom5"><%= bikeName %></h3>
                                    <p class="text-truncate text-light-grey font11">Ex-showroom,&nbsp;Mumbai</p>
                                    <p class="text-default font16 margin-bottom15">
                                        &#x20b9;&nbsp;<span class="text-bold">1,20,198</span>
                                    </p>
                                </div>
                        </a>
                        <a class="compare-with-target text-truncate" href="" title="">
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
