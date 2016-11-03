<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.UsedRecentBikes" EnableViewState="false" %>
<%@ Import Namespace="Bikewale.Entities.Used" %>
<% if (viewModel != null && viewModel.RecentUsedBikes != null && viewModel.FetchedRecordsCount>0)
   {  %>
<div class="container section-container">
                <div class="grid-12">
                    <h2 class="section-header">Recently uploaded used bikes</h2>
                    <div id="recent-uploads" class="content-box-shadow padding-top20 padding-bottom20">
                        <div class="jcarousel-wrapper inner-content-carousel used-carousel">
                            <div class="jcarousel">
                                <ul>
                                         <% foreach (OtherUsedBikeDetails bike in viewModel.RecentUsedBikes)
                               { %>
                                    <li>
                                        <a href="/used/bikes-in-<%=bike.CityMaskingName %>/<%= bike.MakeMaskingName %>-<%= bike.ModelMaskingName %>-<%= bike.ProfileId %>/" title="Used <%= bike.BikeName %>" class="jcarousel-card">
                                            <div class="model-jcarousel-image-preview">
                                                <div class="card-image-block position-rel">
                                                    <img class="lazy" alt="<%= bike.BikeName %>" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(bike.Photo.OriginalImagePath,bike.Photo.HostUrl,Bikewale.Utility.ImageSize._310x174) %>" border="0">
                                                </div>
                                            </div>
                                              <% if (bike.ModelYear!=null)
                                            { %>
                                            <div class="card-desc-block">
                                                <h3 class="bikeTitle"><%= bike.BikeName %></h3>
                                                <div class="grid-6 alpha omega margin-bottom5"> 
                                                    <span class="bwsprite model-date-icon"></span>
                                                    <span class="model-details-label"><%=bike.ModelYear.Year %> model</span>
                                                </div>
                                                  <% } %>
                                                   <% if ( bike.KmsDriven!=null )
                                            { %>
                                                <div class="grid-6 alpha omega margin-bottom5">
                                                    <span class="bwsprite kms-driven-icon"></span>
                                                    <span class="model-details-label"><%= Bikewale.Common.CommonOpn.FormatPrice(bike.KmsDriven.ToString())%> kms</span>
                                                </div>
                                                <% } %>
                                                       <% if (!string.IsNullOrEmpty(bike.OwnerType))
                                            { %>
                                                <div class="grid-6 alpha omega margin-bottom5">
                                                    <span class="bwsprite author-grey-sm-icon"></span>
                                                    <span class="model-details-label"><%= Bikewale.Utility.Format.AddNumberOrdinal(Convert.ToUInt16(bike.OwnerType),4) %> Owner</span>
                                                </div>
                                                 <% } %>
                                                 <% if (!string.IsNullOrEmpty(bike.RegisteredAt)) { %>
                                                <div class="grid-6 alpha omega margin-bottom5">
                                                    <span class="bwsprite model-loc-icon"></span>
                                                    <span class="model-details-label"><%=bike.RegisteredAt %></span>
                                                </div> 
                                                 <% } %>                                               
                                                <div class="clear"></div>
                                                <div class="margin-top5">
                                                    <span class="bwsprite inr-lg"></span>
                                                    <span class="font18 text-default text-bold"><%= Bikewale.Common.CommonOpn.FormatPrice(bike.AskingPrice.ToString())%></span>

                                                </div>
                                            </div>
                                        </a>
                                    </li>
                                    <%} %>
                                   
                                </ul>
                            </div>
                            <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev" rel="nofollow"></a></span>
                            <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next" rel="nofollow"></a></span>
                        </div>

                        <div class="text-center margin-top15">
                            <a href="/used/bikes-in-india/" title="Used Bikes in India" class="btn btn-inv-teal inv-teal-sm">View all bikes<span class="bwsprite teal-next"></span></a>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
<%} %>