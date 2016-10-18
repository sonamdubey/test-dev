<%@ Control Language="C#" AutoEventWireup="false"  Inherits="Bikewale.Controls.OtherUsedBikeByCity" %>
<% if (FetchedRecordsCount > 0)
   { %>
<div id="usedContent" class="bw-model-tabs-data padding-top20 font14">
    <h2 class="padding-left20 margin-bottom15">Other used bikes in <%= viewModel.CityName %></h2>
    <div class="jcarousel-wrapper inner-content-carousel used-bikes-carousel">
        <div class="jcarousel">
            <ul>
                <% foreach (var bike in otherBikesinCity)
                       { %>
                <li>
                    <a href="/used/bikes-in-<%=bike.CityMaskingName %>/<%= bike.MakeMaskingName %>-<%= bike.ModelMaskingName %>-<%= bike.ProfileId %>/" title="Used <%=bike.ModelYear.Year %> <%= bike.BikeName %>" class="jcarousel-card">
                        <div class="model-jcarousel-image-preview">
                            <span class="card-image-block">
                                <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(bike.Photo.OriginalImagePath,bike.Photo.HostUrl,Bikewale.Utility.ImageSize._310x174) %>" title="Used <%=bike.ModelYear.Year %> <%= bike.BikeName %>" alt="Used <%=bike.ModelYear.Year %> <%= bike.BikeName %>" border="0">
                            </span>
                        </div>
                        <div class="card-desc-block">
                            <h3 class="bikeTitle"><%= bike.BikeName %></h3>
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
                        <a href="javascript:void(0)" class="btn btn-sm btn-grey font14 used-bike-lead" rel="nofollow" data-profile-id="<%= bike.ProfileId %>" data-ga-cat="Used_Bike_Detail" data-ga-widgetname="Other_Used_" data-ga-act="Get_Seller_Details_Clicked" data-ga-lab="<%= bike.ProfileId %>" >Get seller details</a>
                    </div>
                </li>
                <% } %>
            </ul>
        </div>
        <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev inactive" rel="nofollow"></a></span>
        <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next" rel="nofollow"></a></span>
    </div>
    <div class="padding-left20 margin-top15 padding-bottom20">
       <a href="/used/bikes-in-<%=viewModel.CityMaskingName %>/" title="Used bikes in <%=viewModel.CityName %>" class="font14">View all used bikes in <%= viewModel.CityName %><span class="bwsprite blue-right-arrow-icon"></span></a>
    </div>
    <div class="margin-right10 margin-left10 border-solid-bottom"></div>
</div>
<% } %>

