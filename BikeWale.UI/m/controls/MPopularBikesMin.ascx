<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.MPopularBikesMin" %>
<%if(FetchedRecordsCount>0) {%>
<section>
            <div class="container box-shadow bg-white section-bottom-margin padding-bottom20">
                <h2 class="padding-top15 padding-right20 padding-bottom10 padding-left20">Popular <%=(!String.IsNullOrEmpty(makeName) ? makeName: "")%> bikes</h2>
                <div class="swiper-container card-container swiper-small">
                    <div class="swiper-wrapper">
                         <%foreach (var bike in objPopularBikes){
                     string bikeName = string.Format("{0} {1}", bike.objMake.MakeName, bike.objModel.ModelName);
                     %>
                        <div class="swiper-slide">
                            <div class="swiper-card">
                                <a href="<%= Bikewale.Utility.UrlFormatter.BikePageUrl(bike.objMake.MaskingName,bike.objModel.MaskingName) %>" title="<%=bikeName%>">
                                    <div class="swiper-image-preview position-rel">
                                        <img class="swiper-lazy" alt="<%=bikeName %>" data-src="<%= Bikewale.Utility.Image.GetPathToShowImages(bike.OriginalImagePath,bike.HostURL,Bikewale.Utility.ImageSize._310x174) %>" alt="<%=bikeName%>">
                                    </div>
                                    <div class="swiper-details-block">
                                        <h3 class="target-link font12 text-truncate margin-bottom5"><%=bikeName%></h3>
                                        <p class="text-truncate text-light-grey font11">Ex-showroom, <%= !string.IsNullOrEmpty(bike.CityName)? bike.CityName : System.Configuration.ConfigurationManager.AppSettings["DefaultName"]  %></p>
                                        <p class="text-default">
                                             <% if(bike.VersionPrice > 0) { %>
                                            <span class="bwmsprite inr-xsm-icon"></span>&nbsp;<span class="text-bold font16"><%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(bike.VersionPrice)) %></span> 
                                        <%} else{ %>
                                            <span class="font14">Price unavailable</span>
                                            <%} %>
                                        </p>
                                    </div>
                                </a>
                            </div>
                        </div>
                        <%} %>
                      
                    </div>
                </div>
                 <% if(!string.IsNullOrEmpty(makeMasking)) { %>
                <div class="margin-top15 margin-left20 font14">
                    <a href="/<%= makeMasking %>-bikes/" title="View all <%=(!String.IsNullOrEmpty(makeName) ? makeName: "") %> bikes">View all <%=(!String.IsNullOrEmpty(makeName) ? makeName: "") %> bikes<span class="bwmsprite blue-right-arrow-icon"></span></a>
                </div>
                <%} %>
            </div>
        </section>
<%} %>
 <%--<%if(FetchedRecordsCount>0) {%>
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
  <%} %>--%>
