<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.ServiceCentersInNearbyCities" %>
<style>
    .bwm-brands-swiper .swiper-slide, .bwm-brands-swiper .swiper-card {
        width: 200px;
        min-height: 127px;
    }
    .city-map{
    height: 75px;
    width : 100%;
    line-height: 0;
    display: table;
    text-align: center;
    }
#nearbyCities.swiper-wrapper div div a div h3 {text-decoration:none;}
#nearbyCities.swiper-wrapper div div a div.swiper-details-block{height:60px;}   
</style>
   <%if (ServiceCentersNearbyCities != null  ){ %>
<div class="container bg-white box-shadow padding-top15 padding-bottom20 font14 active">
    <h2 class="font18 padding-bottom20 padding-left20"><%=String.Format("Explore {0} service centers in cities near {1}",makeName,cityName)%></h2>
 
<div class="swiper-container padding-top5 padding-bottom5 sw-0 swiper-container-horizontal bwm-brands-swiper">
        <div class="swiper-wrapper" id="nearbyCities">
  <%foreach (var centers in ServiceCentersNearbyCities)
              { %>
                <div class="swiper-slide swiper-slide-visible swiper-slide-active">
                    <div class="swiper-card">
                        <a href="/m<%= Bikewale.Utility.UrlFormatter.GetServiceCenterUrl(makeMaskingName,centers.CityMaskingName)%>" title="<%=String.Format("{0} Service Centers in {1}",makeName,centers.CityName)%>">
                            <div class="city-map" data-item-lat="<%=centers.Lattitude%>" data-item-long="<%=centers.Longitude%>">
                            </div>
                            <div class="swiper-details-block">
                                <h3 class="target-link font12 text-truncate margin-bottom5 padding-top5"><%=centers.CityName%></h3>
                                <p class="text-truncate text-light-grey font11 padding-bottom10"><%=centers.ServiceCenterCount%> <%=makeName %> Service Center<%=(centers.ServiceCenterCount)>1?"s":"" %></p>
                            </div>
                        </a>
                    </div>
                </div>
            <%} %>
        </div>
    </div>
</div>
    <%} %>