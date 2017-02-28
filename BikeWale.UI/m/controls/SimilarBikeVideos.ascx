<%@ Control Language="C#" AutoEventWireup="false"  Inherits="Bikewale.Mobile.Controls.SimilarBikeVideos" EnableViewState="false" %>
  <%if (SimilarBikeVideoList!=null){ %>    
 <div class="swiper-container card-container alternate-bikes-photo-swiper overlay-carousel">
            <div class="swiper-wrapper">
                <% foreach (var bike in SimilarBikeVideoList)
                   {
                       string bikeName = string.Format("{0} {1}",bike.Make.MakeName,bike.Model.ModelName); %>
                <div class="swiper-slide">
                    <div class="swiper-card">
                        <a href="/m<%=Bikewale.Utility.UrlFormatter.FormatVideoPageUrl(bike.Make.MaskingName,bike.Model.MaskingName) %>" title="<%= bikeName %> videos" class="block swiper-card-target">
                            <div class="swiper-image-preview">
                                <img class="swiper-lazy" data-src="<%= Bikewale.Utility.Image.GetPathToShowImages(bike.OriginalImagePath,bike.HostUrl,Bikewale.Utility.ImageSize._160x89) %>" alt="<%= bikeName %> videos" src="" />
                                <span class="swiper-lazy-preloader"></span>
                                <span class="black-overlay">
                                    <span class="bwmsprite video-play-icon"></span>
                                </span>
                            </div>
                            <div class="swiper-details-block">
                                <h3 class="margin-bottom5 text-truncate"><%= bikeName %></h3>
                                <p class="font14 text-default text-truncate"><%= bike.VideoCount %> <%= bike.VideoCount>1?"Videos":"Video" %></p>
                            </div>
                        </a>
                    </div>
                </div>
                <% } %>
            </div>
        </div>
<%} %>