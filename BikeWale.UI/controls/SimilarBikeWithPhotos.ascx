    <%@ Control Language="C#" AutoEventWireup="false" EnableViewState="false" Inherits="Bikewale.Controls.SimilarBikeWithPhotos" %>
<% if(FetchedRecordsCount > 0) { %>
<section>
            <div class="container margin-bottom20">
                <div class="grid-12">
                    <div class="content-box-shadow padding-top20 padding-bottom20">
                        <div class="carousel-heading-content margin-bottom15">
                            <h2 class="swiper-heading-left-grid font18  inline-block"><%=WidgetHeading%></h2>
                        </div>
                        <div class="jcarousel-wrapper inner-content-carousel alternate-photos-swiper">
                            <div class="jcarousel">
                                <ul>
                                    <%  string city = null; string showText = null; string price = null; string onwardText = null; string imageText = null;
                                        foreach (var bike in objSimilarBikes) { string bikeName = string.Format("{0} {1}", bike.Make.MakeName, bike.Model.ModelName);%>
                                    <li>
                                        <a href="/<%= bike.Make.MaskingName %>-bikes/<%= bike.Model.MaskingName %>/" title="<%= bikeName %>" class="jcarousel-card">
                                            <div class="model-jcarousel-image-preview">
                                                 <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(bike.OriginalImagePath,bike.HostUrl,Bikewale.Utility.ImageSize._310x174) %>" alt="<%= bikeName %> images"  border="0">
                                            </div>
                                            <div class="card-desc-block">
                                                <h3 class="bikeTitle"><%= bikeName %></h3>
                                                
                                                <% if (CityId > 0 && bike.OnRoadPriceInCity > 0) {  city = City;  showText = "On-Road Price,";  price = Bikewale.Utility.Format.FormatPrice(Convert.ToString(bike.OnRoadPriceInCity)); onwardText = null;}
                                                    else { city = "Mumbai";  showText = "Ex-showroom,";  price = Bikewale.Utility.Format.FormatPrice(Convert.ToString(bike.ExShowroomPriceMumbai)) ; onwardText = "onwards" ; }  %>   
                                                <p class="text-light-grey margin-bottom5"><%= showText %>&nbsp;<%= city %></p>
                                                <p class="font18 text-default inline-block">&#x20b9;<span class="text-bold">&nbsp;<%= price %>&nbsp;</span><span class="font14"><%= onwardText %></span></p>
                                            
                                            </div>
                                        </a>
                                        <% if (bike.PhotosCount > 1) { imageText = "images"; }
                                            else { imageText = "image"; } %>
                                        <a href="/<%= bike.Make.MaskingName %>-bikes/<%= bike.Model.MaskingName %>/images/" title="<%= bikeName %> images" class="margin-bottom5 compare-with-target text-truncate">
                                            <span class="bwsprite photos-sm"></span><span class="margin-left5 text-default inline-block">View all&nbsp;<%= bike.PhotosCount %>&nbsp;<%= imageText %></span><span class="bwsprite next-grey-icon"></span>
                                        </a>
                                    </li>   
                                    <% } %>     
                                </ul>
                            </div>
                            <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev inactive" rel="nofollow"></a></span>
                            <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next" rel="nofollow"></a></span>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
</section>
<%} %>