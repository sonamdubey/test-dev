<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.ServiceCentersInNearbyCities" %>
<%if (ServiceCentersNearbyCities != null) { %>
<div class="container section-container ">
<div class="grid-12 margin-bottom20">
    <div class="content-box-shadow padding-top20 padding-bottom20">
        <h2 class="font18 padding-bottom20 padding-left20"><%=String.Format("Explore {0} service centers in cities near {1}",makeName,cityName)%></h2>
        <div class="jcarousel-wrapper inner-content-carousel map-type-carousel">
            <div class="jcarousel">
                <ul>
                    <%foreach (var centers in ServiceCentersNearbyCities)
                    { %>
                    <li>
                        <a href="<%= Bikewale.Utility.UrlFormatter.GetServiceCenterUrl(makeMaskingName,centers.CityMaskingName)%>" title="<%=String.Format("{0} Service Centers in {1}",makeName,centers.CityName)%>" class="jcarousel-card">
                            <div class="lazy service-location-img" data-original="<%= centers.GoogleMapImg %>" title="<%= String.Format("{0} Service Centers in {1}",makeName,centers.CityName) %>"></div>
                            <div class="card-desc-block">
                                <p class="card-heading font14 text-bold text-black padding-top5"><%= centers.CityName%></p>
                                <h3 class="text-unbold text-light-grey"><%= String.Format("{0} {1}", centers.ServiceCenterCount,makeName) %> service center<%=(centers.ServiceCenterCount)>1?"s":"" %></h3>
                            </div>
                        </a>
                    </li>
                    <%} %>
                </ul>
            </div>
            <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev" rel="nofollow"></a></span>
            <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next" rel="nofollow"></a></span>
        </div>
    </div>
</div>
</div>
 <%} %>


