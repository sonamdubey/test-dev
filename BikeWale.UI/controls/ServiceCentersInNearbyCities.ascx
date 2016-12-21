<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.ServiceCentersInNearbyCities" %>
<style>
    .city-map{
    height: 100px;
    width : 290px;
    line-height: 0;
    display: table;
    text-align: center;
    }
#carouselServiceCenter span.jcarousel-control-right,#carouselServiceCenter span.jcarousel-control-left {top: 31%;}
#nearbyCities.jcarousel ul a div h3.bikeTitle {text-decoration:none;}
#nearbyCities.jcarousel ul a div.card-desc-block { height: 80px; width:290px; }
</style>
    <%if (ServiceCentersNearbyCities != null) { %>
<div class="container section-container ">
<div class="grid-12 margin-bottom20">
    <div class="content-box-shadow padding-top20 padding-bottom20">
    <h2 class="font18 padding-bottom20 padding-left20"><%=String.Format("Explore {0} service centers in cities near {1}",makeName,cityName)%></h2>
    <div class="jcarousel-wrapper inner-content-carousel" id="carouselServiceCenter">
       <div class="jcarousel" id="nearbyCities" data-jcarousel="true">
                 <ul>

                        <%foreach (var centers in ServiceCentersNearbyCities)
                          { %>
                                <li>
                                    <a href="<%= Bikewale.Utility.UrlFormatter.GetServiceCenterUrl(makeMaskingName,centers.CityMaskingName)%>" title="<%=String.Format("{0} Service Centers in {1}",makeName,centers.CityName)%>" class="jcarousel-card">
                                      
                                        <div class="city-map" data-item-lat="<%=centers.Lattitude%>" data-item-long="<%=centers.Longitude%>">
                                         </div>
                                        <div class="card-desc-block">
                                            <h3 class="bikeTitle padding-top10"><%= centers.CityName%></h3>
                                          
                                            <p class="font14 text-light-grey margin-bottom5 padding-bottom10"><%=centers.ServiceCenterCount %> <%=makeName %> Service Center<%=(centers.ServiceCenterCount)>1?"s":"" %></p>
                                           
                                        </div>
                                    </a>
                                        </li>
                           <%} %>
                    </ul>
                 </div>

          <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev inactive" rel="nofollow"></a></span>
            <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next" rel="nofollow"></a></span>
            </div>
    </div>
    </div>
</div>
 <%} %>


