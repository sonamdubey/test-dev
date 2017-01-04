<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.GenericBikeInfoControl" EnableViewState="false" %>
<% if(bikeInfo!=null)
   { %>

<div class="model-slug-content">
    <div class="margin-bottom10">
        <a href="<%= bikeUrl%>" class="item-image-content inline-block"   title="<%= bikeName %>" >
            <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(bikeInfo.OriginalImagePath,bikeInfo.HostUrl,Bikewale.Utility.ImageSize._110x61) %>" src="" alt="Honda CB Shine" />
        </a>
        <div class="bike-details-block inline-block">
            <p class="font12 text-light-grey">More info about:</p>
            <a href="<%= bikeUrl%>" class="block text-bold text-default text-truncate"><%= bikeName %></a>
        </div>
    </div>
    <ul class="item-more-details-list">
         <% if (bikeInfo.ExpertReviewsCount > 0)
           { %>
        <li>
            <a href="/m<%= Bikewale.Utility.UrlFormatter.FormatExpertReviewUrl(bikeInfo.Make.MaskingName,bikeInfo.Model.MaskingName) %>" title="<%= bikeName %> Expert Reviews">
                <span class="generic-sprite reviews-sm"></span>
                <span class="icon-label">Reviews</span>
            </a>
        </li>
         <%} %>
        <% if (bikeInfo.PhotosCount > 0)
           { %>
        <li>
            <a href="/m<%= Bikewale.Utility.UrlFormatter.FormatPhotoPageUrl(bikeInfo.Make.MaskingName,bikeInfo.Model.MaskingName) %>" title="<%= bikeName %> Photos">
                <span class="bwmsprite photos-sm"></span>
                <span class="icon-label">Photos</span>
            </a>
        </li>
          <% } %>
        <% if (bikeInfo.VideosCount > 0)
           { %>
        <li>
            <a href="/m<%= Bikewale.Utility.UrlFormatter.FormatVideoPageUrl(bikeInfo.Make.MaskingName,bikeInfo.Model.MaskingName) %>" title="<%= bikeName %> Videos">
                <span class="generic-sprite videos-sm"></span>
                <span class="icon-label">Videos</span>
            </a>
        </li>
          <% } %>
        <% if(bikeInfo.IsSpecsAvailable) { %>
        <li>
            <a href="/m<%= Bikewale.Utility.UrlFormatter.ViewAllFeatureSpecs(bikeInfo.Make.MaskingName,bikeInfo.Model.MaskingName) %>" title="<%= bikeName %> Specification">
                <span class="generic-sprite specs-sm"></span>
                <span class="icon-label">Specs</span>
            </a>
        </li>
         <% } %>
    </ul>
    <div class="clear"></div>
</div>
<% }  %>
