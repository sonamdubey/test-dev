<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.MUpcomingBikesMin" %>
<!-- Mobile Upcoming Bikes Starts here-->
 <%if(FetchedRecordsCount>0) {%>
    <div class="container box-shadow padding-top20 padding-bottom20 ">
        <h2 class="margin-bottom25 padding-right20 padding-left20">Upcoming <%=(!String.IsNullOrEmpty(makeName) ? makeName: "")%> bikes</h2>
        <div class="swiper-container">
            <div class="swiper-wrapper upcoming-carousel-content">
               
               <%foreach(var bike in objBikeList){ %>
                        <div class="swiper-slide bike-carousel-swiper">
                            <div class="bike-swiper-image-wrapper">
                                <a href="/m<%= Bikewale.Utility.UrlFormatter.BikePageUrl(bike.MakeBase.MaskingName,bike.ModelBase.MaskingName) %>>">
                                    <img class="swiper-lazy" data-src="<%= Bikewale.Utility.Image.GetPathToShowImages(bike.OriginalImagePath,bike.HostUrl,Bikewale.Utility.ImageSize._310x174) %>" alt="<%= bike.MakeBase.MakeName+ " " + bike.ModelBase.ModelName %>" />
                                    <span class="swiper-lazy-preloader"></span>
                                </a>
                            </div>
                            <div class="bike-swiper-details-wrapper">
                                <h3 class="bikeTitle margin-bottom5"><a href="/m<%= Bikewale.Utility.UrlFormatter.BikePageUrl(bike.MakeBase.MaskingName,bike.ModelBase.MaskingName) %>"><%=bike.MakeBase.MakeName +" " + bike.ModelBase.ModelName %></a></h3>
                                <p class="text-xx-light margin-bottom5 ">Expected price</p>
                                <div class="font16">
                                    <span class="bwmsprite inr-xsm-icon"></span>&nbsp;<span class="text-bold"><%= Bikewale.Utility.Format.FormatPrice(bike.EstimatedPriceMin.ToString()) %></span>
                                </div>
                            </div>
                        </div>
            <%} %>   
            </div>
                
        </div>
        <div class="margin-top10 margin-bottom10">
                                <a href="<%=upcomingBikesLink %>" title="View all upcoming <%=(!String.IsNullOrEmpty(makeName) ? makeName: "") %> bikes" class="font14">View all upcoming <%=(!String.IsNullOrEmpty(makeName) ? makeName: "") %> bikes<span class="bwsprite blue-right-arrow-icon"></span></a>
                            </div>
    </div>
  <%} %>
