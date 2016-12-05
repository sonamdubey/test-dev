<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.ServiceCenterCard" %>
<% if(ServiceCenteList!=null) { %>
<div class="container section-bottom-margin">
                    <div class="grid-12">
                        <div class="content-box-shadow padding-bottom20">
                            <h2 class="section-h2-title padding-15-20-20"><%=headerText%></h2>
                            <%if(!string.IsNullOrEmpty(biLineText)) {%>
                            <p class="font14"><%=biLineText%></p>
                            <%} %>
                            <ul class="bw-horizontal-cards">
                                 <% foreach (var serviceCenter in ServiceCenteList)
                                    { %>
                                <li class="card">
                                    <a href="<%= Bikewale.Utility.UrlFormatter.GetServiceCenterUrl(makeMaskingName,cityMaskingName, serviceCenter.Name,serviceCenter.ServiceCenterId) %>" title="<%=serviceCenter.Name %>" class="card-target">
                                        <h3 class="text-black text-bold text-truncate margin-bottom5"><%=serviceCenter.Name %></h3>
                                        <% if (!string.IsNullOrEmpty(serviceCenter.Address)){ %>
                                        <p class="text-light-grey margin-bottom5">
                                            <span class="bwsprite dealership-loc-icon vertical-top margin-right5"></span>
                                            <span class="vertical-top details-column"><%=serviceCenter.Address %></span>
                                        </p>
                                        <% } %>
                                        <% if (!string.IsNullOrEmpty(serviceCenter.Phone)){ %>
                                        <p class="text-default">
                                            <span class="bwsprite phone-black-icon vertical-top margin-right5"></span>
                                            <span class="text-bold vertical-top details-column"><%=serviceCenter.Phone %></span>
                                        </p>
                                        <% } %>
                                    </a>
                                </li>
                                <% } %>
                            </ul>
                            <div class="clear"></div>
                            <div class="padding-left20 font14">
                                <a href="<%= Bikewale.Utility.UrlFormatter.GetServiceCenterUrl(makeMaskingName, cityMaskingName) %>" title="View all <%= makeName %> service centers in <%=cityName %>" >View all <%= makeName %> service centers in <%=cityName %><span class="bwsprite blue-right-arrow-icon"></span></a>
                            </div>
                        </div>
                    </div>
                    <div class="clear"></div>
</div>
<% } %>