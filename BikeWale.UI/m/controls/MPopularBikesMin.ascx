<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.MPopularBikesMin" %>
<!-- Mobile Upcoming Bikes Starts here-->
 <%if(FetchedRecordsCount>0) {%>
    <div class="container box-shadow padding-top20 padding-bottom20 ">
        <h2 class="margin-bottom25 padding-right20 padding-left20">Popular <%=(!String.IsNullOrEmpty(makeName) ? makeName: "")%> bikes</h2>
        <div class="swiper-container">
            <div class="swiper-wrapper upcoming-carousel-content">
               
               <%foreach (var bike in objPopularBikes)
                 {
                     string bikeName = string.Format("{0} {1}", bike.objMake.MakeName, bike.objModel.ModelName);
                     %>
                <a href="<%= Bikewale.Utility.UrlFormatter.BikePageUrl(bike.objMake.MaskingName,bike.objModel.MaskingName) %>" title="<%= bikeName %>" class="bike-target-link">
                        <div class="swiper-slide bike-carousel-swiper">
                            <div class="bike-swiper-image-wrapper">
                                <a href="/m<%= Bikewale.Utility.UrlFormatter.BikePageUrl(bike.MakeMaskingName,bike.objModel.MaskingName) %>" title="<%= bikeName %>">
                                    <img class="swiper-lazy" data-src="<%= Bikewale.Utility.Image.GetPathToShowImages(bike.OriginalImagePath,bike.HostURL,Bikewale.Utility.ImageSize._310x174) %>" alt="<%=bikeName%>" />
                                    <span class="swiper-lazy-preloader"></span>
                                </a>
                            </div>
                            <div class="bike-swiper-details-wrapper">
                                <h3 class="bikeTitle margin-bottom5"><a href="/m<%= Bikewale.Utility.UrlFormatter.BikePageUrl(bike.MakeMaskingName,bike.objModel.MaskingName) %>"><%=bikeName %></a></h3>
                                <p class="text-xx-light margin-bottom5 ">Ex-showroom <%= !string.IsNullOrEmpty(bike.CityName)? bike.CityName : System.Configuration.ConfigurationManager.AppSettings["DefaultName"]  %></p>
                                <div class="font16">
                                    <% if(bike.VersionPrice > 0) { %>
                    <span class="bwmsprite inr-xsm-icon"></span>&nbsp;<span class="text-bold"><%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(bike.VersionPrice)) %></span>
                    <% } else { %>
                    <span class='font14'>Price Unavailable</span>
                    <% } %>
                            </div>
                            </div>
                        </div>
                    </a>
            <%} %>   
            </div>
        </div>
        <% if(!string.IsNullOrEmpty(makeMasking)) { %>
        <div class="margin-top10 margin-bottom10">
                                <a href="/<%= makeMasking %>-bikes/" title="View all <%=(!String.IsNullOrEmpty(makeName) ? makeName: "") %> bikes" class="font14">View all <%=(!String.IsNullOrEmpty(makeName) ? makeName: "") %> bikes<span class="bwsprite blue-right-arrow-icon"></span></a>
                            </div>
        <%} %>
    </div>
  <%} %>
<%--<div class="content-box-shadow padding-15-20-10 margin-bottom20">
    <h2>Popular <%= !string.IsNullOrEmpty(makeName)? makeName : string.Empty %> bikes</h2>
    <ul class="sidebar-bike-list">
        <%foreach (var bike in popularBikes) { 
            string bikeName = string.Format("{0} {1}",bike.objMake.MakeName,bike.objModel.ModelName);  
              %>
        <li>
            <a href="<%= Bikewale.Utility.UrlFormatter.BikePageUrl(bike.objMake.MaskingName,bike.objModel.MaskingName) %>" title="<%= bikeName %>" class="bike-target-link">
                <div class="bike-target-image inline-block">
                    <img src="<%= Bikewale.Utility.Image.GetPathToShowImages(bike.OriginalImagePath,bike.HostURL,Bikewale.Utility.ImageSize._110x61) %>" alt="<%= bikeName %>" />
                </div>
                <div class="bike-target-content inline-block padding-left10">
                    <h3><%= bikeName %></h3>
                    <p class="font11 text-light-grey">Ex-showroom <%= !string.IsNullOrEmpty(bike.CityName)? bike.CityName : System.Configuration.ConfigurationManager.AppSettings["DefaultCity"]  %></p>
                    <% if(bike.VersionPrice > 0) { %>
                    <span class="bwsprite inr-md"></span><span class="font16 text-bold">&nbsp;<%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(bike.VersionPrice)) %></span>
                    <% } else { %>
                    <span class='font14'>Price Unavailable</span>
                    <% } %>
                </div>
            </a>
        </li>
        <% } %>
    </ul>
    <% if(!string.IsNullOrEmpty(makeMasking)) { %>
    <div class="margin-top10 margin-bottom10">
        <a href="/<%= makeMasking %>-bikes/" title="All popular <%= makeName %> bikes" class="font14">View all popular <%= makeName %> bikes<span class="bwsprite blue-right-arrow-icon"></span></a>
    </div>
    <% } %>
</div>--%>