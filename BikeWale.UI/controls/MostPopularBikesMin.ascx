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
                    <% if (bike.VersionPrice > 0)
                       { %>
                    <p class="font11 text-light-grey">Ex-showroom, <%= !string.IsNullOrEmpty(bike.CityName)? bike.CityName :  Bikewale.Common.Configuration.GetDefaultCityName %></p>
                    <span class="bwsprite inr-md"></span><span class="font16 text-bold">&nbsp;<%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(bike.VersionPrice)) %></span>
                    <% }
                       else
                       { %>
                    <span class='font14 text-light-grey'>Price not available</span>
                    <% } %>
                </div>
            </a>
        </li>
        <% } %>
    </ul>
    <% if (!string.IsNullOrEmpty(makeMasking))
       { %>
    <div class="view-all-btn-container padding-top10 padding-bottom10">
        <a href="/<%= makeMasking %>-bikes/" title="<%= makeName %> bikes" class="btn view-all-target-btn">View all bikes<span class="bwsprite teal-right"></span></a>
    </div>
    <% } %>
    <% else 
       { %>
    <div class="view-all-btn-container padding-top10 padding-bottom10">
        <a href="/best-bikes-in-india/" title="Best Bikes in India" class="btn view-all-target-btn">View all bikes<span class="bwsprite teal-right"></span></a>
    </div>
    <% } %>
</div>
<%} %>
