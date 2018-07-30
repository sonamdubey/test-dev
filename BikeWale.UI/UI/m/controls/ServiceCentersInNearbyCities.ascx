<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.ServiceCentersInNearbyCities" %>
<%if (ServiceCentersNearbyCities != null  ){ %>
<div class="container bg-white box-shadow padding-top15 padding-bottom10 font14 active">
    <h2 class="padding-right20 padding-bottom15 padding-left20"><%=String.Format("Explore {0} service centers in cities near {1}",makeName,cityName)%></h2>
 
    <div class="swiper-container padding-top5 padding-bottom5 map-type-carousel">
        <div class="swiper-wrapper">
            <%foreach (var centers in ServiceCentersNearbyCities)
              { %>
                <div class="swiper-slide">
                    <div class="swiper-card">
                        <a href="/m<%= Bikewale.Utility.UrlFormatter.GetServiceCenterUrl(makeMaskingName,centers.CityMaskingName)%>" title="<%=String.Format("{0} Service Centers in {1}",makeName,centers.CityName)%>">
                            <div class="swiper-lazy service-location-img" data-background="<%= centers.GoogleMapImg %>" title="<%= String.Format("{0} Service Centers in {1}",makeName,centers.CityName) %>">
                            </div>
                            <div class="swiper-details-block">
                                <p class="text-bold text-black font12 margin-bottom5 padding-top5"><%=centers.CityName%></p>
                                <h3 class="text-unbold text-light-grey font11"><%= String.Format("{0} {1}",centers.ServiceCenterCount,makeName) %> service center<%=(centers.ServiceCenterCount)>1?"s":"" %></h3>
                            </div>
                        </a>
                    </div>
                </div>
            <%} %>
        </div>
    </div>
</div>
<%} %>