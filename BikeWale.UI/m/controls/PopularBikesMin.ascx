<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.PopularBikesMin" %>
<%if (FetchedRecordsCount > 0)
  {%>
<section>
    <div class="container box-shadow bg-white section-bottom-margin padding-bottom20 padding-top15">
        <div class="carousel-heading-content">
        <div class="swiper-heading-left-grid inline-block">
        <h2>Popular <%=(!String.IsNullOrEmpty(makeName) ? makeName: "")%> bikes</h2>
        </div>
            <% if (!string.IsNullOrEmpty(makeMasking))
           { %>
         <div class="swiper-heading-right-grid inline-block text-right">
            <a href="/m/<%= makeMasking %>-bikes/" title="<%=makeName %> Bikes" class="btn view-all-target-btn">View all</a>
        </div>
            
        <%} %>
        <% else 
           { %>
        <div class="swiper-heading-right-grid inline-block text-right">
            <a href="/m/best-bikes-in-india/" title="Best Bikes in India" class="btn view-all-target-btn">View all</a>
        </div>
        <% } %>
            <div class="clear"></div>
        </div>
        <div class="swiper-container card-container swiper-small">
            <div class="swiper-wrapper">
                <%foreach (var bike in objPopularBikes)
                  {
                      string bikeName = string.Format("{0} {1}", bike.objMake.MakeName, bike.objModel.ModelName);
                %>
                <div class="swiper-slide">
                    <div class="swiper-card">
                        <a href="/m<%= Bikewale.Utility.UrlFormatter.BikePageUrl(bike.objMake.MaskingName,bike.objModel.MaskingName) %>" title="<%=bikeName%>">
                            <div class="swiper-image-preview">
                                <img class="swiper-lazy" alt="<%=bikeName %>" data-src="<%= Bikewale.Utility.Image.GetPathToShowImages(bike.OriginalImagePath,bike.HostURL,Bikewale.Utility.ImageSize._174x98) %>" alt="<%=bikeName%>">
                            </div>
                            <div class="swiper-details-block">
                                <h3 class="target-link font12 text-truncate margin-bottom5"><%=bikeName%></h3>
                                    <% if (bike.VersionPrice > 0)
                                       { %>
                                    <p class="text-default text-truncate text-light-grey font11">Ex-showroom, <%= !string.IsNullOrEmpty(bike.CityName)? bike.CityName : System.Configuration.ConfigurationManager.AppSettings["DefaultName"]  %></p>
                                   <p class="text-default">    
                                   <span class="bwmsprite inr-xsm-icon"></span>&nbsp;<span class="text-bold font16"><%= Bikewale.Utility.Format.FormatPrice(Convert.ToString(bike.VersionPrice)) %></span>
                                    </p>
                                        <%}
                                       else
                                       { %>
                                  <p class="text-default">
                                    <span class="font14">Price unavailable</span>
                                  </p>
                                    <%} %>
                                </div>
                        </a>
                    </div>
                </div>
                <%} %>
            </div>
        </div>
           
    </div>
</section>
<%} %>
