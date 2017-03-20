<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.UpcomingBikesMin" %>
<%if(FetchedRecordsCount>0) {%>     
 <section>
            <div class="container box-shadow bg-white section-bottom-margin padding-bottom20 padding-top15">
                <div class="carousel-heading-content">
                <div class="swiper-heading-left-grid inline-block">
                <h2>Upcoming <%=(!String.IsNullOrEmpty(makeName) ? makeName: "")%> bikes</h2>
                 </div>
                <div class="swiper-heading-right-grid inline-block text-right">
            <a href="<%=upcomingBikesLink %>" title="Upcoming Bikes in India" class="btn view-all-target-btn">View all</a>
          </div>
                    <div class="clear"></div>
                </div>
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
                                        <% if(bike.EstimatedPriceMin > 0) { %>
                                         <p class="text-truncate text-light-grey font11">Expected price</p>
                                        <p class="text-default">    
                                        <span class="bwmsprite inr-xsm-icon"></span>&nbsp;<span class="text-bold font16"><%= Bikewale.Utility.Format.FormatPrice(bike.EstimatedPriceMin.ToString()) %> onwards</span> 
                                            </p>
                                                 <%}else { %>
                                            <p class="text-default">
                                            <span class="font14 text-light-grey">Price not available</span>
                                             </p>
                                            <% } %> 
                                    </div>
                                </a>
                            </div>
                        </div>
                        <%} %>
                       </div>
                </div>
               
            </div>
        </section>
<%} %>

