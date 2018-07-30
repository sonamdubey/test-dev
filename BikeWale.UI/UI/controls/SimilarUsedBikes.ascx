<%@ Control Language="C#" AutoEventWireup="false"  Inherits="Bikewale.Controls.SimilarUsedBikes" %>
<% if (FetchedRecordsCount > 0)
   { %>
<div id="similarContent" class="bw-model-tabs-data font14">
    <div class="carousel-heading-content padding-top20">
        <div class="swiper-heading-left-grid inline-block">
            <h2>More second-hand <%= ModelName %> bikes in <%= CityName %></h2>
        </div><div class="swiper-heading-right-grid inline-block text-right">
            <a href="<%= Bikewale.Utility.UrlFormatter.UsedBikesUrlNoCity(MakeMaskingName,ModelMaskingName,CityMaskingName) %>" title="Used <%= String.Format("{0} {1}",MakeName,ModelName) %> Bikes in <%= CityName %>" class="btn view-all-target-btn">View all</a>
        </div>
        <div class="clear"></div>
    </div>
    <div class="jcarousel-wrapper inner-content-carousel used-bikes-carousel margin-bottom20">
        <div class="jcarousel">
            <ul>
                <% foreach (var bike in similarBikeList)
                       { %>
                <li>
                    <a href="/used/bikes-in-<%= CityMaskingName %>/<%= MakeMaskingName %>-<%= ModelMaskingName %>-<%= bike.ProfileId %>/" title="Used <%= String.Format("{0} {1}", bike.ModelYear.Year, bike.BikeName) %>" class="jcarousel-card">
                        <div class="model-jcarousel-image-preview">
                            <span class="card-image-block">
                                <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(bike.Photo.OriginalImagePath,bike.Photo.HostUrl,Bikewale.Utility.ImageSize._310x174) %>" title="Used <%=String.Format("{0} {1}", bike.ModelYear.Year, bike.BikeName) %>" alt="Used <%=String.Format("Used {0} {1}", bike.ModelYear.Year, bike.BikeName) %>" border="0">
                            </span>
                        </div>
                        <div class="card-desc-block">
                            <h3 class="bikeTitle text-truncate">Used <%= bike.BikeName %></h3>
                              <% if (bike.ModelYear!=null)
                                 { %>
                            <div class="grid-6 alpha">
                                <span class="bwsprite model-date-icon"></span>
                                <span class="model-details-label"><%=bike.ModelYear.Year %> model</span>
                            </div>
                              <% } %>
                              <% if ( bike.KmsDriven!=null )
                                 { %>
                            <div class="grid-6 alpha omega">
                                <span class="bwsprite kms-driven-icon"></span>
                                <span class="model-details-label"><%= Bikewale.Common.CommonOpn.FormatPrice(bike.KmsDriven.ToString())%> kms</span>
                            </div>
                                <% } %>
                              <% if (!string.IsNullOrEmpty(bike.OwnerType))
                                 { %>
                            <div class="grid-6 alpha">
                                <span class="bwsprite author-grey-sm-icon"></span>
                                <span class="model-details-label"><%= Bikewale.Utility.Format.AddNumberOrdinal(Convert.ToUInt16(bike.OwnerType),4) %> owner</span>
                            </div>
                                <% } %>
                            <% if (!string.IsNullOrEmpty(bike.RegisteredAt)) { %>
                            <div class="grid-6 alpha omega">
                                <span class="bwsprite model-loc-icon"></span>
                                <span class="model-details-label"><%=bike.RegisteredAt %></span>
                            </div>
                             <% } %>
                            <div class="clear"></div>

                            <span class="bwsprite inr-lg"></span>&nbsp;<span class="font18 text-default text-bold"><%= Bikewale.Common.CommonOpn.FormatPrice(bike.AskingPrice.ToString())%></span>
                        </div>
                    </a>
                    <div class="margin-left20 margin-bottom20">
                        <a href="javascript:void(0)" class="btn btn-sm btn-grey font14 used-bike-lead" rel="nofollow" data-profile-id="<%= bike.ProfileId %>" data-ga-cat="Used_Bike_Detail" data-ga-widgetname="Similar_Used_" data-ga-act="Get_Seller_Details_Clicked" data-ga-lab="<%= bike.ProfileId %>" >Get seller details</a>
                    </div>
                </li>
                <% } %>
            </ul>
        </div>
        <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev inactive" rel="nofollow"></a></span>
        <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next" rel="nofollow"></a></span>
    </div>
    <div class="margin-right10 margin-left10 border-solid-bottom"></div>
</div>
<% } %>
