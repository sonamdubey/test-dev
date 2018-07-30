<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.OtherUsedBikeByCity" EnableViewState="false" %>
<% if (otherBikesinCity!=null && FetchedRecordsCount > 0)
   {  %>
        <div id="modelOtherBikes" class="bw-model-tabs-data padding-top20 padding-bottom15">
            
            <h2 class="margin-right20 margin-bottom15 margin-left20">Other used bikes in <%= viewModel.CityName %></h2>
            
            <div id="similar-bike-swiper" class="swiper-container padding-top5 padding-bottom5">
                <div class="swiper-wrapper">
                    <% foreach (var bike in otherBikesinCity)
                       { %>
                        <div class="swiper-slide swiper-shadow">
                        <div class="model-swiper-image-preview">
                            <a href="/m/used/bikes-in-<%=bike.CityMaskingName %>/<%= bike.MakeMaskingName %>-<%= bike.ModelMaskingName %>-<%= bike.ProfileId %>/">
                                <% if (String.IsNullOrEmpty(bike.Photo.OriginalImagePath)) { %>
                                <img class="swiper-lazy" data-src="<%= Bikewale.Utility.Image.GetPathToShowImages(bike.Photo.OriginalImagePath,bike.Photo.HostUrl,Bikewale.Utility.ImageSize._210x118) %>" />
                                <% } else { %>
                                <img class="swiper-lazy" data-src="<%= Bikewale.Utility.Image.GetPathToShowImages(bike.Photo.OriginalImagePath,bike.Photo.HostUrl,Bikewale.Utility.ImageSize._210x118) %>" title="Used <%= String.Format("{0} {1}", bike.ModelYear.Year,bike.BikeName) %>" alt="Used <%=String.Format("{0} {1}", bike.ModelYear.Year,bike.BikeName) %>" />
                                <% } %>
                                <span class="swiper-lazy-preloader"></span>
                            </a>
                        </div>
                        <div class="model-swiper-details font11">
                            <a href="/m/used/bikes-in-<%=bike.CityMaskingName %>/<%= bike.MakeMaskingName %>-<%= bike.ModelMaskingName %>-<%= bike.ProfileId %>/" class="target-link font12 text-truncate margin-bottom5" title="<%= bike.BikeName %>"><%= bike.BikeName %></a>
                              <% if (bike.ModelYear!=null)
                                 { %>
                            <div class="grid-6 alpha padding-right5">
                                <span class="bwmsprite model-date-icon-xs"></span>
                                <span class="model-details-label"><%=bike.ModelYear.Year %> model</span>
                            </div>
                            <% } %>
                              <% if ( bike.KmsDriven!=null )
                                 { %>
                            <div class="grid-6 omega padding-left5">
                                <span class="bwmsprite kms-driven-icon-xs"></span>
                                <span class="model-details-label"><%= Bikewale.Common.CommonOpn.FormatPrice(bike.KmsDriven.ToString())%> kms</span>
                            </div>
                            <% } %>
                              <% if (!string.IsNullOrEmpty(bike.OwnerType))
                                 { %>
                            <div class="grid-6 alpha padding-right5">
                                <span class="bwmsprite author-grey-icon-xs"></span>
                                <span class="model-details-label"><%= Bikewale.Utility.Format.AddNumberOrdinal(Convert.ToUInt16(bike.OwnerType),4) %> owner</span>
                            </div>
                            <% } %>
                            <% if (!string.IsNullOrEmpty(bike.RegisteredAt)) { %>
                            <div class="grid-6 omega padding-left5">
                                <span class="bwmsprite model-loc-icon-xs"></span>
                                <span class="model-details-label"><%=bike.RegisteredAt %></span>
                            </div>
                            <% } %>
                            <div class="clear"></div>
                            <p class="margin-top5"><span class="bwmsprite inr-xsm-icon"></span>&nbsp;<span class="font16 text-bold"><%= Bikewale.Common.CommonOpn.FormatPrice(bike.AskingPrice.ToString())%></span></p>
                            <a href="javascript:void(0)" class="btn btn-xs btn-full-width btn-white margin-top10 font14 used-bike-lead" rel="nofollow" data-profile-id="<%= bike.ProfileId %>" data-ga-cat="Used_Bike_Detail" data-ga-act="Get_Seller_Details_Clicked" data-ga-widgetname="Other_Used_" data-ga-lab="<%= bike.ProfileId %>">Get seller details</a>
                        </div>
                    </div>
                    <% } %>
                </div>
            </div>
            <div class="margin-top10 margin-right20 margin-left20">
                <a href="/m/used/bikes-in-<%=viewModel.CityMaskingName %>/" title="Used bikes in <%=viewModel.CityName %>" class="font14">View all used bikes in <%= viewModel.CityName %><span class="bwmsprite blue-right-arrow-icon"></span></a>
            </div>
        </div>
        <div class="margin-right20 margin-left20 border-solid-bottom"></div>
<% } %>
