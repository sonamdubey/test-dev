<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.UsedBikeByModels" EnableViewState="false" %>
	<div class="swiper-container card-container city-model-carousel">
                            <div class="swiper-wrapper">
                               
                                        <%foreach (var ModelDetails in UsedBikeModelInCityList)
                                          {%>
                                 <div class="swiper-slide">
                                    <div class="swiper-card">
                                        <a href="/m/used/<%=MakeMaskingName%>-<%=ModelDetails.ModelMaskingName%>-bikes-in-<%=ModelDetails.CityMaskingName %>/" title="Used <%=ModelDetails.ModelName %> bikes in <%=ModelDetails.CityName %>" class="card-target-block">
                                            <div class="card-image-placeholder">
                                                <img class="swiper-lazy" data-src="<%=Bikewale.Utility.Image.GetPathToShowImages(ModelDetails.OriginalImagePath,ModelDetails.HostUrl,Bikewale.Utility.ImageSize._227x128) %>" src="" alt="Used <%=ModelDetails.ModelName %> bikes in <%=ModelDetails.CityName %>" />
                                                <span class="swiper-lazy-preloader"></span>
                                            </div>
                                            <div class="card-details-placeholder">
                                                <h2 class="font14 text-truncate margin-bottom5"><%=ModelDetails.ModelName %></h2>
                                                <p class="font14 text-light-grey text-truncate"><%=ModelDetails.AvailableBikes %> used bikes</p>
                                            </div>
                                        </a>
                                           </div>
                                </div>
                                        <%} %>
                            </div>
                        </div>