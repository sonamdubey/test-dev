<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.GenericBikeInfoControl" EnableViewState="false" %>
<% if (bikeInfo != null)
    { %>
<%if (!SmallSlug)
    { %>
<div class="model-more-info-section">
    <%if (IsUpcoming)
        { %>
    <p class="model-ribbon-tag upcoming-ribbon">Upcoming</p>
    <%}
        else if (IsDiscontinued)
        { %>
    <p class="model-ribbon-tag discontinued-ribbon">Discontinued</p>
    <%} %>
    <div class="clear"></div>
    <a href="<%= bikeUrl%>" class="leftfloat text-default margin-bottom5" title="<%= bikeName %>">
        <h2><%= bikeName %></h2>
    </a>
     <div class="clear"></div>    
    <div class="margin-bottom10">
        <%if (RatingCount > 0)
            { %>
        <span class="rate-count-<%=Math.Round(Rating)%>">
            <span class="bwmsprite star-icon star-size-16"></span>
            <span class="font14 text-bold inline-block"><%=Rating.ToString("0.0").TrimEnd('0', '.')%></span>
        </span>
        &nbsp;<span class='font11 text-xt-light-grey inline-block padding-left3'>(<%=string.Format("{0} {1}", RatingCount, RatingCount > 1 ? "ratings" : "rating") %>)</span>
        <%if (UserReviewCount > 0)
            {  %>
        <a class='text-xt-light review-left-divider inline-block' href="<%=string.Format("{0}reviews/", bikeUrl) %>" title="<%=bikeName %> user reviews"><%=string.Format("{0} {1}", UserReviewCount, UserReviewCount > 1 ? "reviews" : "review") %></a>
        <% }
            }
            else
            { %>
        <div class="model-more-info-section model-slug-type-news">
            
        </div>
        <%}
            }%>
        <div class="margin-bottom10">
            <%if (SmallSlug)
                {
                    if (IsUpcoming)
                    { %>
            <p class="model-ribbon-tag upcoming-ribbon">Upcoming</p>
            <%}
                else if (IsDiscontinued)
                { %>
            <p class="model-ribbon-tag discontinued-ribbon">Discontinued</p>
            <%} %>
            <%} %>
            <div class="clear"></div>
            <a href="<%= bikeUrl%>" class="item-image-content vertical-top" title="<%= bikeName %>">
                <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(bikeInfo.OriginalImagePath,bikeInfo.HostUrl,Bikewale.Utility.ImageSize._110x61) %>" src="" alt="<%= bikeName %>" />
            </a>
            <div class="bike-details-block vertical-top">
                <%if (SmallSlug)
                    { %><a href="<%= bikeUrl%>" class="block text-default margin-bottom5" title="<%= bikeName %>"><h3 class="text-truncate"><%= bikeName %></h3>
                    </a><%} %>
                <% if (IsDiscontinued)
                    {%>
                <p class="price-label-size-12 text-truncate">Last known Ex-showroom price</p>
                <div>
                    <span class="bwmsprite inr-sm-icon"></span>
                    <span class="price-value-size-18"><%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(bikeInfo.BikePrice)) %></span>
                </div>
                <%}
                    else if (IsUpcoming)
                    {%>
                <p class="price-label-size-12">Expected price</p>
                <div>
                    <span class="bwmsprite inr-sm-icon"></span>
                    <span class="price-value-size-18">
                        <%= Bikewale.Utility.Format.FormatNumeric(Convert.ToString(bikeInfo.EstimatedPriceMin)) %> - <%= Bikewale.Utility.Format.FormatNumeric(Convert.ToString(bikeInfo.EstimatedPriceMax)) %>
                    </span>
                </div>
                <%} %>
                <%else
                    {
                        if (bikeInfo.PriceInCity > 0 && cityDetails != null)
                        { %>
                <p class="price-label-size-12 text-truncate"><%=String.Format("On-road price, {0}",cityDetails.CityName)%></p>
                <div>
                    <span class="bwmsprite inr-sm-icon"></span>
                    <span class="price-value-size-18"><%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(bikeInfo.PriceInCity)) %></span>
                </div>
                <% }
                    else
                    { %>
                <p class="price-label-size-12 text-truncate"><%=String.Format("Ex-showroom, {0}",Bikewale.Utility.BWConfiguration.Instance.DefaultName)%></p>
                <div>
                    <span class="bwmsprite inr-sm-icon"></span>
                    <span class="price-value-size-18"><%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(bikeInfo.BikePrice)) %></span>
                </div>
                <%}
                    } %>
            </div>
        </div>
        <%if (!IsDiscontinued && !IsUpcoming && bikeInfo.BikePrice > 0 && !SmallSlug)
            { %>
        <a href="/m<%=Bikewale.Utility.UrlFormatter.BikePageUrl(bikeInfo.Make.MaskingName,bikeInfo.Model.MaskingName)%>" title="<%=bikeName%>" class="btn btn-white btn-180-34  margin-bottom15">View model details <span class="bwmsprite btn-red-arrow"></span></a>
        <% } %>
        <%if (bikeInfo != null)
            {%>
        <ul class="item-more-details-list">
            <%foreach (var Tabsdetails in bikeInfo.Tabs)
                { %>
            <li>
                <a href="/m<%= Tabsdetails.URL%>" title="<%= String.Format("{0} {1}",bikeName, Tabsdetails.Title)%>">
                    <span class="bwmsprite <%=Tabsdetails.IconText%>-sm"></span>
                    <span class="icon-label"><%=Tabsdetails.TabText %></span>
                </a>
            </li>
            <%} %>
        </ul>
        <%} %>
        <div class="clear"></div>
        <%if (bikeInfo.Make != null && Series != null && Series.IsSeriesPageUrl && Series.ModelsCount > 1)
            {
                string bodyType = string.Format("{0}{1}", Series.BodyStyle.Equals(Bikewale.Entities.GenericBikes.EnumBikeBodyStyles.Scooter) ? "scooter" : "bike", Series.ModelsCount > 2 ? "s" : "");
               %>
        <div class="border-solid-bottom margin-top5 margin-bottom10"></div>
        <a href="/m<%=Bikewale.Utility.UrlFormatter.BikeSeriesUrl(bikeInfo.Make.MaskingName,Series.MaskingName) %>" title=" <%= string.Format("{0} {1} {2}", bikeInfo.Make.MakeName, Series.SeriesName, bodyType)%>" class="block text-default hover-no-underline">
            <span class="used-target-label inline-block">
                <span class="font14 text-bold"><%=string.Format("{0} more {1} {2} {3}", Series.ModelsCount - 1, bikeInfo.Make.MakeName, Series.SeriesName , bodyType) %></span><br />
                <span class="font12 text-light-grey">starting at <span class="bwmsprite inr-12-grey"></span><%=Bikewale.Utility.Format.FormatNumeric(Convert.ToString(Series.MinPrice))%></span>
            </span>
            <span class="bwmsprite next-grey-icon"></span>
        </a>
        <%} %>
    </div>
</div>
<% }  %>
