<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.MinGenericBikeInfoControl" EnableViewState="false" %>
<% if (bikeInfo != null)
   { %>
<div class="model-grid-8-slug">
    <%if (IsUpcoming)
  { %>
<p class="model-ribbon-tag upcoming-ribbon">Upcoming</p>
<%}
  else if (IsDiscontinued)
  { %>
<p class="model-ribbon-tag discontinued-ribbon">Discontinued</p>
<%} %>
    <div class="clear"></div>
    <div class="grid-8 alpha border-solid-right">
        <a href="<%= bikeUrl%>" title="<%=bikeName%>" class="model-image-target vertical-top">
            <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(bikeInfo.OriginalImagePath,bikeInfo.HostUrl,Bikewale.Utility.ImageSize._160x89) %>" src="" alt="<%=bikeName%>" />
        </a>
        <div class="model-details-block vertical-top">
            <a href="<%=bikeUrl%>" title="<%=bikeName%>" class="block text-default">
                <h3><%=bikeName%></h3>
            </a>
            <ul class="item-more-details-list inline-block">
                <% if (bikeInfo.Tabs != null)
                   {
                       foreach (var Tabsdetails in bikeInfo.Tabs)
                       { %>
                <li>
                    <a href="<%= Tabsdetails.URL%>" title="<%= String.Format("{0} {1}",bikeName, Tabsdetails.Title)%>">
                        <span class="bwsprite <%=Tabsdetails.IconText%>-sm"></span>
                        <span class="icon-label"><%=Tabsdetails.TabText %></span>
                    </a>
                </li>
                <%}
                   } %>
            </ul>
        </div>
    </div>
    <div class="grid-4 omega">
        <% if (IsDiscontinued)
           {%>
        <p class="font12 text-light-grey margin-bottom5 text-truncate" title="Last known Ex-showroom price">Last known Ex-showroom price</p>
        <span class="bwsprite inr-md"></span><span class="font16 text-bold"><%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(bikeInfo.BikePrice))%></span>
        <%}
           else if (IsUpcoming)
           {%>
        <p class="font12 text-light-grey margin-bottom5 text-truncate" title="Expected price">Expected price</p>
        <span class="bwsprite inr-md"></span><span class="font16 text-bold"><%= Bikewale.Utility.Format.FormatNumeric(Convert.ToString(bikeInfo.EstimatedPriceMin)) %> - <%= Bikewale.Utility.Format.FormatNumeric(Convert.ToString(bikeInfo.EstimatedPriceMax)) %></span>
        <%}
           else
           {
               if (bikeInfo.PriceInCity > 0 && cityDetails != null)
               { %>
        <p class="font12 text-light-grey margin-bottom5 text-truncate" title="<%=String.Format("On-road price, {0}",cityDetails.CityName)%>"><%=String.Format("Ex-showroom, {0}",cityDetails.CityName)%></p>
        <span class="bwsprite inr-md"></span><span class="font16 text-bold"><%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(bikeInfo.PriceInCity)) %></span>
        <% }
               else
               { %>
        <p class="font12 text-light-grey margin-bottom5 text-truncate" title="<%=String.Format("Ex-showroom, {0}",Bikewale.Utility.BWConfiguration.Instance.DefaultName)%>"><%=String.Format("Ex-showroom, {0}",Bikewale.Utility.BWConfiguration.Instance.DefaultName)%></p>
        <span class="bwsprite inr-md"></span><span class="font16 text-bold"><%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(bikeInfo.BikePrice)) %></span>
        <%}
           } %>
    </div>
    <div class="clear"></div>
    <%if (bikeInfo.UsedBikeCount > 0)
      { %>
    <div class="border-solid-bottom margin-top15 margin-bottom10"></div>
    <a href="<%=Bikewale.Utility.UrlFormatter.UsedBikesUrlNoCity(bikeInfo.Make.MaskingName,bikeInfo.Model.MaskingName,(cityDetails!=null)?cityDetails.CityMaskingName:"india") %>" title="<%=bikeInfo.UsedBikeCount %> Used <%=bikeName%> bikes" class="block text-default hover-no-underline">
        <span class="used-target-label inline-block">
            <span class="font14 text-bold"><%=bikeInfo.UsedBikeCount %> Used <%=bikeName%> bikes</span>
            <span class="font12 text-light-grey">starting at <span class="bwsprite inr-xsm-grey"></span><%=Bikewale.Utility.Format.FormatNumeric(Convert.ToString(bikeInfo.UsedBikeMinPrice))%></span>
        </span>
        <span class="bwsprite next-grey-icon"></span>
    </a>
    <%} %>
</div>
<%} %>