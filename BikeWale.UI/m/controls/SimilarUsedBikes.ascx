<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="SimilarUsedBikes.ascx.cs" Inherits="Bikewale.Mobile.Controls.SimilarUsedBikes" %>
<%@ Import Namespace="Bikewale.Entities.Used" %>
<% if(FetchedRecordsCount> 0)
   {  %>
        <div id="modelSimilar" class="bw-model-tabs-data padding-top20 padding-bottom15">
            
            <h2 class="margin-right20 margin-bottom15 margin-left20">Similar used <%= ModelName %> bikes</h2>
            
            <div id="similar-bike-swiper" class="swiper-container padding-top5 padding-bottom5">
                <div class="swiper-wrapper">
                    <% foreach (BikeDetailsMin bike in similarBikeList)
                       { %>
                        <div class="swiper-slide swiper-shadow">
                        <div class="model-swiper-image-preview">
                            <a href="/m/used/bikes-in-<%= CityMaskingName %>/<%= MakeMaskingName %>-<%= ModelMaskingName %>-<%= bike.ProfileId %>/">
                                <img class="swiper-lazy" data-src="<%= Bikewale.Utility.Image.GetPathToShowImages(bike.Photo.OriginalImagePath,bike.Photo.HostUrl,Bikewale.Utility.ImageSize._310x174) %>" title="" alt="" />
                                <span class="swiper-lazy-preloader"></span>
                            </a>
                        </div>
                        <div class="model-swiper-details font11">
                            <a href="/m/used/bikes-in-<%= CityMaskingName %>/<%= MakeMaskingName %>-<%= ModelMaskingName %>-<%= bike.ProfileId %>/" class="target-link font12 text-truncate margin-bottom5" title="<%= BikeName %>"><%= BikeName %></a>
                            <div class="grid-6 alpha padding-right5">
                                <span class="bwmsprite model-date-icon-xs"></span>
                                <span class="model-details-label"><%=bike.ModelYear.Year %> model</span>
                            </div>
                            <div class="grid-6 omega padding-left5">
                                <span class="bwmsprite kms-driven-icon-xs"></span>
                                <span class="model-details-label"><%= Bikewale.Common.CommonOpn.FormatPrice(bike.KmsDriven.ToString())%> kms</span>
                            </div>
                            <div class="grid-6 alpha padding-right5">
                                <span class="bwmsprite author-grey-icon-xs"></span>
                                <span class="model-details-label"><%= Bikewale.Utility.Ordinal.GetRank(Convert.ToUInt16(bike.OwnerType)) %> owner</span>
                            </div>
                            <div class="grid-6 omega padding-left5">
                                <span class="bwmsprite model-loc-icon-xs"></span>
                                <span class="model-details-label"><%=bike.RegisteredAt %></span>
                            </div>
                            <div class="clear"></div>
                            <p class="margin-top5"><span class="bwmsprite inr-xsm-icon"></span>&nbsp;<span class="font16 text-bold"><%= Bikewale.Common.CommonOpn.FormatPrice(bike.AskingPrice.ToString())%></span></p>
                            <a href="javascript:void(0)" class="btn btn-xs btn-full-width btn-white margin-top10 font14" rel="nofollow">Get seller details</a>
                        </div>
                    </div>
                    <% } %>
                </div>
            </div>
            <%--<div class="margin-top10 margin-right20 margin-left20">
                <a href="/m/used/bikes-in-<%= CityMaskingName %>/<%= MakeMaskingName %>-<%= ModelMaskingName %>/" title="" class="font14">View all <%= ModelName %> in <%= CityName %><span class="bwmsprite blue-right-arrow-icon"></span></a>
            </div>--%>
        </div>
        <div class="margin-right20 margin-left20 border-solid-bottom"></div>
<% } %>
