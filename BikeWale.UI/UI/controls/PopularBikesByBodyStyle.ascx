<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.PopularBikesByBodyStyle" %>
    <ul class="sidebar-bike-list">
        <%foreach (var bike in popularBikes) { 
            string bikeName = string.Format("{0} {1}",bike.MakeName,bike.objModel.ModelName);  
              %>
        <li>
            <a href="<%= Bikewale.Utility.UrlFormatter.BikePageUrl(bike.MakeMaskingName,bike.objModel.MaskingName) %>" title="<%= bikeName %>" class="bike-target-link">
                <div class="bike-target-image inline-block">
                    <img src="<%= Bikewale.Utility.Image.GetPathToShowImages(bike.OriginalImagePath,bike.HostURL,Bikewale.Utility.ImageSize._110x61) %>" alt="<%= bikeName %>" />
                </div>
                <div class="bike-target-content inline-block padding-left10">
                    <h3><%= bikeName %></h3>
                    <% if(bike.VersionPrice > 0) { %>
                    <p class="font11 text-light-grey">Ex-showroom, <%= !string.IsNullOrEmpty(bike.CityName)? bike.CityName : Bikewale.Utility.BWConfiguration.Instance.DefaultName %></p>
                    <span class="bwsprite inr-md"></span><span class="font16 text-bold">&nbsp;<%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(bike.VersionPrice)) %></span>
                    <% } else { %>
                    <span class='font14 text-light-grey'>Price not available</span>
                    <% } %>
                </div>
            </a>
        </li>
        <% } %>
    </ul>
    <div class="view-all-btn-container padding-top10 padding-bottom10">
        <a href="<%=Bikewale.Utility.UrlFormatter.FormatGenericPageUrl(BodyStyle) %>" title="Best <%=BodyStyleLinkTitle%> in India" class="btn view-all-target-btn">View all bikes<span class="bwsprite teal-right"></span></a>
    </div>