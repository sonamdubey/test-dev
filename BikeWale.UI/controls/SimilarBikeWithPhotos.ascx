<%@ Control Language="C#" AutoEventWireup="false" EnableViewState="false" Inherits="Bikewale.Controls.SimilarBikeWithPhotos" %>
<% if(FetchedRecordsCount > 0) { %>
<section>
            <div class="container margin-bottom20">
                <div class="grid-12">
                    <div class="content-box-shadow padding-top20 padding-bottom20">
                        <h2 class="font18 margin-left20 margin-bottom15"><%=WidgetHeading%></h2>
                        <div class="jcarousel-wrapper inner-content-carousel alternate-photos-swiper">
                            <div class="jcarousel">
                                <ul>
                                    <% foreach(var bike in objSimilarBikes) {
                                           string bikeName = string.Format("{0} {1}", bike.Make.MakeName, bike.Model.ModelName); %>
                                    <li>
                                        <a href="/<%= bike.Make.MaskingName %>-bikes/<%= bike.Model.MaskingName %>/images/" title="<%= bikeName %> images" class="jcarousel-card">
                                            <div class="model-jcarousel-image-preview">
                                                <img class="lazy" data-original="<%= Bikewale.Utility.Image.GetPathToShowImages(bike.OriginalImagePath,bike.HostUrl,Bikewale.Utility.ImageSize._310x174) %>" alt="<%= bikeName %> images" src="" border="0">
                                                <span class="black-overlay">
                                                    <span class="bwsprite photos-lg-white"></span>
                                                </span>
                                            </div>
                                            <div class="card-desc-block">
                                                <p class="bikeTitle"><%= bikeName %></p>
                                                <p class="text-light-grey"><%= bike.PhotosCount %> images</p>
                                            </div>
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