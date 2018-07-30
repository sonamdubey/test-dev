<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.PopularBikesByBodyStyle" %>
 <div class="carousel-heading-content padding-top15">
                <div class="swiper-heading-left-grid inline-block">
                <h2> Popular <%=BodyStyleText%></h2>
                    </div>
     <div class="swiper-heading-right-grid inline-block text-right">
            <a href="/m<%=Bikewale.Utility.UrlFormatter.FormatGenericPageUrl(BodyStyle) %>" title="Best <%=BodyStyleLinkTitle%> in India" class="btn view-all-target-btn">View all</a>
        </div> 
<div class="clear"></div>             
  </div>    
<div class="swiper-container card-container swiper-small">
                    <div class="swiper-wrapper">

                        <%foreach (var bike in popularBikes)
                          {
                              string bikeName = string.Format("{0} {1}", bike.MakeName, bike.objModel.ModelName);
                         %>
                        <div class="swiper-slide">
                            <div class="swiper-card">
                                <a href="/m<%= Bikewale.Utility.UrlFormatter.BikePageUrl(bike.MakeMaskingName,bike.objModel.MaskingName) %>" title="<%=bikeName%>">
                                    <div class="swiper-image-preview position-rel">
                                        <img class="swiper-lazy" alt="<%=bikeName %>" data-src="<%= Bikewale.Utility.Image.GetPathToShowImages(bike.OriginalImagePath,bike.HostURL,Bikewale.Utility.ImageSize._174x98) %>" title="<%=bikeName %>">
                                    </div>
                                    <div class="swiper-details-block">
                                        <h3 class="target-link font12 text-truncate margin-bottom5"><%=bikeName %></h3>
                                            <% if(bike.VersionPrice > 0) { %>
                                             <p class="text-truncate text-light-grey font11">Ex-showroom, <%= !string.IsNullOrEmpty(bike.CityName)? bike.CityName : Bikewale.Utility.BWConfiguration.Instance.DefaultName  %></p>
                                             <p class="text-default">
                                            <span class="bwmsprite inr-xsm-icon"></span>&nbsp;<span class="text-bold font16"><%= Bikewale.Utility.Format.FormatPrice(bike.VersionPrice.ToString()) %></span> 
                                            </p>
                                                 <%}else { %>
                                           <p class="text-default">
                                            <span class="font14">Price Unavailable</span>
                                            </p>
                                            <% } %>                                        
                                    </div>
                                </a>
                            </div>
                        </div>
                        <%} %>
                       </div>
                </div>
               
