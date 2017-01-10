<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.UpcomingBikesMin" %>
<%if(FetchedRecordsCount>0) {%>     
 <section>
            <div class="container box-shadow bg-white section-bottom-margin padding-bottom20">
                <h2 class="padding-top15 padding-right20 padding-bottom10 padding-left20">Upcoming <%=(!String.IsNullOrEmpty(makeName) ? makeName: "")%> bikes</h2>
                <div class="swiper-container card-container swiper-small">
                    <div class="swiper-wrapper">

                        <%foreach(var bike in objBikeList){
                              string bikeName = string.Format("{0} {1}", bike.MakeBase.MakeName, bike.ModelBase.ModelName);
                         %>
                        <div class="swiper-slide">
                            <div class="swiper-card">
                                <a href="/m<%= Bikewale.Utility.UrlFormatter.BikePageUrl(bike.MakeBase.MaskingName,bike.ModelBase.MaskingName) %>" title="<%=bikeName%>">
                                    <div class="swiper-image-preview position-rel">
                                        <img class="swiper-lazy" alt="<%=bikeName %>" data-src="<%= Bikewale.Utility.Image.GetPathToShowImages(bike.OriginalImagePath,bike.HostUrl,Bikewale.Utility.ImageSize._174x98) %>" title="<%=bikeName %>">
                                    </div>
                                    <div class="swiper-details-block">
                                        <h3 class="target-link font12 text-truncate margin-bottom5"><%=bikeName %></h3>
                                        <p class="text-truncate text-light-grey font11">Expected price</p>
                                        <p class="text-default">
                                            <% if(bike.EstimatedPriceMin > 0) { %>
                                            <span class="bwmsprite inr-xsm-icon"></span>&nbsp;<span class="text-bold font16"><%= Bikewale.Utility.Format.FormatPrice(bike.EstimatedPriceMin.ToString()) %></span> 
                                            <%}else { %>
                                            <span class="font14">Price Unavailable</span>
                                            <% } %> 
                                        </p>
                                    </div>
                                </a>
                            </div>
                        </div>
                        <%} %>
                       </div>
                </div>
                <div class="margin-top15 margin-left20 font14">
                    <a href="<%=upcomingBikesLink %>" title="View all upcoming <%=(!String.IsNullOrEmpty(makeName) ? makeName: "") %> bikes" >View all upcoming <%=(!String.IsNullOrEmpty(makeName) ? makeName: "")%> bikes<span class="bwmsprite blue-right-arrow-icon"></span></a>
                </div>
            </div>
        </section>
<%} %>

