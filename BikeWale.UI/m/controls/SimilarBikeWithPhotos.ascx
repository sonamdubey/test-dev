<%@ Control Language="C#" AutoEventWireup="false" EnableViewState="false" Inherits="Bikewale.Mobile.Controls.SimilarBikeWithPhotos" %>
<% if(FetchedRecordsCount > 0) { %>
<section>
    <div class="container bg-white box-shadow section-bottom-margin">
        <h2 class="padding-15-20">Photos for alternative bikes</h2>
        <div class="swiper-container card-container alternate-bikes-photo-swiper">
            <div class="swiper-wrapper">
                <% foreach(var bike in objSimilarBikes) {
                       string bikeName = string.Format("{0} {1}",bike.Make.MakeName,bike.Model.ModelName); %>
                <div class="swiper-slide">
                    <div class="swiper-card">
                        <a href="/m/<%= bike.Make.MaskingName %>-bikes/<%= bike.Model.MaskingName %>/photos/" title="<%= bikeName %> Photos" class="block swiper-card-target">
                            <div class="swiper-image-preview">
                                <img class="swiper-lazy" data-src="<%= Bikewale.Utility.Image.GetPathToShowImages(bike.OriginalImagePath,bike.HostUrl,Bikewale.Utility.ImageSize._160x89) %>" alt="<%= bikeName %> Photos" src="" />
                                <span class="swiper-lazy-preloader"></span>
                                <span class="black-overlay">
                                    <span class="bwmsprite photos-white-icon"></span>
                                </span>
                            </div>
                            <div class="swiper-details-block">
                                <h3 class="margin-bottom5 text-truncate"><%= bikeName %></h3>
                                <p class="font14 text-default text-truncate"><%= bike.PhotosCount %> photos</p>
                            </div>
                        </a>
                    </div>
                </div>
                <% } %>
            </div>
        </div>
    </div>
</section>
<% } %>
