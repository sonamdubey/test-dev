﻿<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.GenericBikeInfoControl" EnableViewState="false" %>
<% if (bikeInfo != null)
   { %>

<div class="model-more-info-section">
    <div class="margin-bottom10">
        <a href="<%= bikeUrl%>" class="item-image-content inline-block" title="<%= bikeName %>">
            <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(bikeInfo.OriginalImagePath,bikeInfo.HostUrl,Bikewale.Utility.ImageSize._110x61) %>" src="" alt="<%= bikeName %>" />
        </a>
        <div class="bike-details-block inline-block">
            <h3 class="margin-bottom5"><a href="<%= bikeUrl%>" class="block text-bold text-default text-truncate" title="<%= bikeName %>"><%= bikeName %></a></h3>
            <ul class="key-specs-list font12 text-xx-light">
                <%if (bikeInfo.MinSpecs.Displacement != 0)
                  { %>
                <li>
                    <span><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeInfo.MinSpecs.Displacement.ToString(),"cc") %></span>
                </li>
                <% } %>
                <%if (bikeInfo.MinSpecs.FuelEfficiencyOverall != 0)
                  { %>
                <li>
                    <span><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeInfo.MinSpecs.FuelEfficiencyOverall.ToString(),"kmpl") %></span>
                </li>
                <% } %>
                <%if (bikeInfo.MinSpecs.MaxPower != 0)
                  { %>
                <li>
                    <span><%= Bikewale.Utility.FormatMinSpecs.ShowAvailable(bikeInfo.MinSpecs.MaxPower.ToString(),"bhp") %></span>
                </li>
                <% } %>
            </ul>
        </div>
    </div>
    <ul class="item-more-details-list">
        <% if (bikeInfo.ExpertReviewsCount > 0)
           { %>
        <li>
            <a href="/m<%= Bikewale.Utility.UrlFormatter.FormatExpertReviewUrl(bikeInfo.Make.MaskingName,bikeInfo.Model.MaskingName) %>" title="<%= bikeName %> Expert Reviews">
                <span class="bwmsprite reviews-sm"></span>
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
                <span class="bwmsprite videos-sm"></span>
                <span class="icon-label">Videos</span>
            </a>
        </li>
        <% } %>
        <% if (bikeInfo.IsSpecsAvailable)
           { %>
        <li>
            <a href="/m<%= Bikewale.Utility.UrlFormatter.ViewAllFeatureSpecs(bikeInfo.Make.MaskingName,bikeInfo.Model.MaskingName) %>" title="<%= bikeName %> Specification">
                <span class="bwmsprite specs-sm"></span>
                <span class="icon-label">Specs</span>
            </a>
        </li>
        <% } %>
    </ul>
    <div class="clear"></div>
    <div class="margin-top5 margin-bottom5">
        <p class="font13 text-grey">Ex-showroom, <%= Bikewale.Utility.BWConfiguration.Instance.DefaultName %></p>
        <div class="margin-bottom10">
            <span class="bwmsprite inr-xsm-icon"></span>
            <span class="font16 text-bold"><%= Bikewale.Utility.Format.FormatPrice(bikeInfo.BikePrice.ToString()) %></span>
        </div>
        <%if (bikeInfo.BikePrice > 0)
          { %>
        <button type="button" data-pagecatid="0"
            data-pqsourceid="<%= (int)pqSource %>" data-makename="<%= bikeInfo.Make.MakeName %>"
            data-modelname="<%= bikeInfo.Model.ModelName %>" data-modelid="<%= ModelId %>"
            class="btn btn-white font14 btn-size-180 getquotation">
            Check on-road price</button>
        <% } %>
    </div>
</div>
<% }  %>
