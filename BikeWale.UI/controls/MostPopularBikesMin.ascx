<%@ Control Language="C#" AutoEventWireup="False" Inherits="Bikewale.Controls.MostPopularBikesMin" %>
<%if (FetchedRecordsCount > 0)
  { %>
<div class="content-box-shadow padding-15-20-10 margin-bottom20">
    <h2>Popular <%= !string.IsNullOrEmpty(makeName)? makeName : string.Empty %> bikes</h2>
    <ul class="sidebar-bike-list">
        <%foreach (var bike in popularBikes)
          {
              string bikeName = string.Format("{0} {1}", bike.objMake.MakeName, bike.objModel.ModelName);  
        %>
        <li>
            <a href="<%= Bikewale.Utility.UrlFormatter.BikePageUrl(bike.objMake.MaskingName,bike.objModel.MaskingName) %>" title="<%= bikeName %>" class="bike-target-link">
                <div class="bike-target-image inline-block">
                    <img src="<%= Bikewale.Utility.Image.GetPathToShowImages(bike.OriginalImagePath,bike.HostURL,Bikewale.Utility.ImageSize._110x61) %>" alt="<%= bikeName %>" />
                </div>
                <div class="bike-target-content inline-block padding-left10">
                    <h3><%= bikeName %></h3>
                    <p class="font11 text-light-grey">Ex-showroom <%= !string.IsNullOrEmpty(bike.CityName)? bike.CityName :  Bikewale.Common.Configuration.GetDefaultCityName %></p>
                    <% if (bike.VersionPrice > 0)
                       { %>
                    <span class="bwsprite inr-md"></span><span class="font16 text-bold">&nbsp;<%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(bike.VersionPrice)) %></span>
                    <% }
                       else
                       { %>
                    <span class='font14'>Price Unavailable</span>
                    <% } %>
                </div>
            </a>
        </li>
        <% } %>
    </ul>
    <% if (!string.IsNullOrEmpty(makeMasking))
       { %>
    <div class="margin-top10 margin-bottom10">
        <a href="/<%= makeMasking %>-bikes/" title="All <%= makeName %> bikes" class="font14">View all <%= makeName %> bikes<span class="bwsprite blue-right-arrow-icon"></span></a>
    </div>
    <% } %>
    <% else if (IsMakeAgnosticFooterNeeded)
       { %>
    <div class="margin-top10 margin-bottom10">
        <a href="/best-bikes-in-india/" title="Best Bikes in India" class="font14">View the complete list<span class="bwsprite blue-right-arrow-icon"></span></a>
    </div>
    <% } %>
</div>
<%} %>
