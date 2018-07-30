<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.UsedRecentBikes" EnableViewState="false" %>
<%@ Import Namespace="Bikewale.Entities.Used" %>
<% if (viewModel != null && viewModel.RecentUsedBikes != null && viewModel.FetchedRecordsCount>0)
   {  %>
        <div class="container text-center section-container">
                <h2 class="font18 section-heading"><%=WidgetTitle %></h2>
                <div class="bg-white box-shadow padding-top20 padding-bottom20">
                    <div class="swiper-container card-container used-swiper">
                        <div class="swiper-wrapper">
                            <% foreach (OtherUsedBikeDetails bike in viewModel.RecentUsedBikes)
                               { %>
                            <div class="swiper-slide">
                                <div class="swiper-card">
                                    <a href="/m/used/bikes-in-<%=bike.CityMaskingName %>/<%= bike.MakeMaskingName %>-<%= bike.ModelMaskingName %>-<%= bike.ProfileId %>/" title="Used <%= bike.BikeName %>">
                                        <div class="swiper-image-preview">
                                            <div class="image-thumbnail">
                                                <img class="swiper-lazy" alt="<%= bike.BikeName %>" data-src="<%= Bikewale.Utility.Image.GetPathToShowImages(bike.Photo.OriginalImagePath,bike.Photo.HostUrl,Bikewale.Utility.ImageSize._310x174) %>"  />
                                                <span class="swiper-lazy-preloader"></span>
                                            </div>
                                        </div>
                                        <div class="swiper-details-block">
                                            <h3 class="target-link font12 margin-bottom5 text-truncate"><%= bike.BikeName %></h3>
                                            <% if (bike.ModelYear!=null)
                                            { %>
                                            <div class="grid-6 alpha omega">
                                                <span class="bwmsprite model-date-icon-xs"></span>
                                                <span class="model-details-label"><%=bike.ModelYear.Year %> model</span>
                                            </div>
                                            <% } %>
                                            <% if ( bike.KmsDriven!=null )
                                            { %>
                                            <div class="grid-6 alpha omega">
                                                <span class="bwmsprite author-grey-icon-xs"></span>
                                                <span class="model-details-label"><%= Bikewale.Common.CommonOpn.FormatPrice(bike.KmsDriven.ToString())%> kms</span>
                                            </div>
                                            <% } %>
                                            <% if (!string.IsNullOrEmpty(bike.OwnerType))
                                            { %>
                                            <div class="grid-6 alpha omega">
                                                <span class="bwmsprite author-grey-icon-xs"></span>
                                                <span class="model-details-label"><%= Bikewale.Utility.Format.AddNumberOrdinal(Convert.ToUInt16(bike.OwnerType),4) %> owner</span>
                                            </div>
                                            <% } %>
                                            <% if (!string.IsNullOrEmpty(bike.RegisteredAt)) { %>
                                            <div class="grid-6 alpha omega">
                                                <span class="bwmsprite model-loc-icon-xs"></span>
                                                <span class="model-details-label"><%=bike.RegisteredAt %></span>
                                            </div>
                                            <% } %>
                                            <div class="clear"></div>
                                            <p class="margin-top5 text-default"><span class="bwmsprite inr-xsm-icon"></span>&nbsp;<span class="font16 text-bold"><%= Bikewale.Common.CommonOpn.FormatPrice(bike.AskingPrice.ToString())%></span></p>
                                        </div>
                                    </a>
                                </div>
                            </div>
                            <% } %>
                        </div>
                    </div>
                    <a href="/m/used/bikes-in-india/" class="btn btn-inv-teal inv-teal-sm margin-top15">View all bikes<span class="bwmsprite teal-next"></span></a>
                </div>
            </div>
<% } %>
