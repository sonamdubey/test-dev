<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.GenericBikeInfoControl" EnableViewState="false" %>
<% if (bikeInfo != null)
   { %>
<%if(IsUpcoming){ %>
<p class="model-ribbon-tag upcoming-ribbon">Upcoming</p>
<%}else if(IsDiscontinued){ %>
<p class="model-ribbon-tag discontinued-ribbon">Discontinued</p>
<%} %>
<div class="model-more-info-section">
    <a href="<%= bikeUrl%>" class="leftfloat text-default margin-bottom15" title="<%= bikeName %>"><h2 class="text-truncate"><%= bikeName %></h2></a>
    <div class="clear"></div>
    <div class="margin-bottom10">
        <a href="<%= bikeUrl%>" class="item-image-content vertical-top" title="<%= bikeName %>">
            <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(bikeInfo.OriginalImagePath,bikeInfo.HostUrl,Bikewale.Utility.ImageSize._110x61) %>" src="" alt="<%= bikeName %>" />
        </a>
        <div class="bike-details-block vertical-top">
            <% if(IsDiscontinued) {%>
                <p class="font12 text-light-grey"><%= String.Format("Last known Ex-showroom price in {0}",Bikewale.Utility.BWConfiguration.Instance.DefaultName) %></p>
                <div>
                    <span class="bwmsprite inr-sm-icon"></span>
                    <span class="font18 text-bold"><%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(bikeInfo.BikePrice)) %></span>
                </div>
                <%} else if(IsUpcoming){%>
                <p class="font12 text-light-grey">Expected price</p>
                <div>
                    <span class="bwmsprite inr-sm-icon"></span>
                    <span class="font18 text-bold">
                        <%= Bikewale.Utility.Format.FormatNumeric(Convert.ToString(bikeInfo.EstimatedPriceMin)) %> - <%= Bikewale.Utility.Format.FormatNumeric(Convert.ToString(bikeInfo.EstimatedPriceMax)) %>
                    </span>
                </div>
                <%} %>
            <%else{ %>
                <p class="font12 text-light-grey"><%=String.Format("Ex-showroom, {0}",Bikewale.Utility.BWConfiguration.Instance.DefaultName)%></p>
                <div>
                    <span class="bwmsprite inr-sm-icon"></span>
                    <span class="font18 text-bold"><%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(bikeInfo.BikePrice)) %></span>
                </div>
            <%} %>
        </div>
    </div>
    <%if (!IsDiscontinued && !IsUpcoming && bikeInfo.BikePrice > 0)
        { %>
    <button type="button" data-pagecatid="0"
        data-pqsourceid="<%= (int)pqSource %>" data-makename="<%= bikeInfo.Make.MakeName %>"
        data-modelname="<%= bikeInfo.Model.ModelName %>" data-modelid="<%= ModelId %>"
        class="btn btn-white btn-180-34 getquotation margin-bottom15">View model details <span class="bwmsprite btn-red-arrow"></span></button>
    <% } %>
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
            <a href="/m<%= Bikewale.Utility.UrlFormatter.FormatPhotoPageUrl(bikeInfo.Make.MaskingName,bikeInfo.Model.MaskingName) %>" title="<%= bikeName %> Images">
                <span class="bwmsprite photos-sm"></span>
                <span class="icon-label">Images</span>
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

    <div class="border-solid-bottom margin-top5 margin-bottom10"></div>
    <a href="" title="" class="block text-default hover-no-underline">
        <span class="used-target-label inline-block">
            <span class="font14 text-bold">53 Used Thunderbird bikes</span><br />
            <span class="font12 text-light-grey">starting at <span class="bwmsprite inr-12-grey"></span> 1,20,000</span>
        </span>
        <span class="bwmsprite next-grey-icon"></span>
    </a>
</div>
<% }  %>
