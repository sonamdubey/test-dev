<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Mobile.Controls.UsedBikeModelByCity" EnableViewState="false" %>
<%if (UsedBikeInCityList != null)
  { %>
<div class="swiper-container card-container city-model-carousel">
    <div class="swiper-wrapper">

        <%foreach (var ModelDetails in UsedBikeInCityList)
          {%>
        <div class="swiper-slide">
            <div class="swiper-card">
                <a href="<%=string.Format("/used-{0}-{1}-bikes-in-{2}/",MakeMaskingName,ModelDetails.ModelMaskingName,ModelDetails.CityMaskingName) %>" title="<%=string.Format("Used {0} bikes in {1}",ModelDetails.ModelName,ModelDetails.CityName)%>" class="card-target-block">
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
<%} %>