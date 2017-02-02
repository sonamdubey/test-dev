<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.PopularBikesByBodyStyle" %>
<%if (FetchedRecordsCount > 0)
  {%>     
 <section>
            <div class="container box-shadow bg-white section-bottom-margin padding-bottom20">
                <h2 class="padding-top15 padding-right20 padding-bottom10 padding-left20">
                    Popular <%=Bikewale.Utility.BodyStyleLinks.BodyStyleFooterLink(BodyStyle)%>
                </h2>
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
                                        <p class="text-truncate text-light-grey font11">Ex-showroom price</p>
                                        <p class="text-default">
                                            <% if(bike.VersionPrice > 0) { %>
                                            <span class="bwmsprite inr-xsm-icon"></span>&nbsp;<span class="text-bold font16"><%= Bikewale.Utility.Format.FormatPrice(bike.VersionPrice.ToString()) %></span> 
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
                    <a href="<%=Bikewale.Utility.UrlFormatter.FormatGenericPageUrl(BodyStyle) %>" title="Best <%=Bikewale.Utility.BodyStyleLinks.BodyStyleFooterLink(BodyStyle)%> in India" >View the complete list<span class="bwmsprite blue-right-arrow-icon"></span></a>
                </div>
            </div>
        </section>
<%} %>

