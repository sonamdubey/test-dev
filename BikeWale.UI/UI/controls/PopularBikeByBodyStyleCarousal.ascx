<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.PopularBikeByBodyStyleCarousal" EnableViewState="false" %>
<%if (popularBikes!=null) {%>
<div  class="jcarousel-wrapper inner-content-carousel padding-bottom20">
    <div  class="jcarousel">
        <ul>
            <%foreach (var bike in popularBikes)
              {
                  string bikeName = string.Format("{0} {1}", bike.MakeName, bike.objModel.ModelName);%>
            <li>
                <a href="<%= Bikewale.Utility.UrlFormatter.BikePageUrl(bike.MakeMaskingName,bike.objModel.MaskingName) %>" title="<%= bikeName %>" class="jcarousel-card">
                    <div class="model-jcarousel-image-preview">
                        <span class="card-image-block">
                            <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(bike.OriginalImagePath,bike.HostURL,Bikewale.Utility.ImageSize._310x174) %>" alt="<%= bikeName %>" src="" border="0">
                        </span>
                    </div>
                    <div class="card-desc-block">
                        <h3 class="bikeTitle"><%=bikeName%></h3>
                        <% if (bike.VersionPrice > 0)
                           { %>
                         <p class="font14 text-light-grey margin-bottom5">Ex-showroom,<%= !string.IsNullOrEmpty(bike.CityName)? bike.CityName :  Bikewale.Utility.BWConfiguration.Instance.DefaultName %></p>
                        <span class="bwsprite inr-md"></span><span class="font16 text-bold text-default">&nbsp;<%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(bike.VersionPrice)) %></span>
                        <% }
                           else
                           { %>
                        <span class="font16 text-default">Price not available</span>
                        <% } %>
                    </div>
                </a>
            </li>
            <%} %>
        </ul>
    </div>
    <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev inactive" rel="nofollow"></a></span>
    <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next" rel="nofollow"></a></span>
</div>
<%} %>