<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bikewale.Controls.UsedBikeByModels" EnableViewState="false" %>
<%if (UsedBikeModelInCityList!=null){ %>
<div class="jcarousel-wrapper inner-content-carousel city-model-carousel type-model-carousel">
    <div class="jcarousel">
        <ul>
            <%foreach(var ModelDetails in UsedBikeModelInCityList) {%>
            <li>
                <a href="/used/<%=MakeMaskingName%>-<%=ModelDetails.ModelMaskingName%>-bikes-in-<%=ModelDetails.CityMaskingName %>/" title="Used <%=ModelDetails.ModelName %> bikes in <%=ModelDetails.CityName %>" class="card-target-block">
                    <div class="card-image-placeholder">
                        <img class="lazy" data-original="<%=Bikewale.Utility.Image.GetPathToShowImages(ModelDetails.OriginalImagePath,ModelDetails.HostUrl,Bikewale.Utility.ImageSize._227x128) %>" src="" alt="Used <%=ModelDetails.ModelName %> bikes in <%=ModelDetails.CityName %>"  />
                    </div>
                    <div class="card-details-placeholder">
                        <h2 class="font14 text-truncate margin-bottom5"><%=ModelDetails.ModelName %></h2>
                        <p class="font14 text-light-grey text-truncate"><%=ModelDetails.AvailableBikes %> used bikes</p>
                    </div>
                </a>
            </li>
            <%} %>
        </ul>
    </div>
    <span class="jcarousel-control-left"><a href="#" class="bwsprite jcarousel-control-prev" rel="nofollow"></a></span>
    <span class="jcarousel-control-right"><a href="#" class="bwsprite jcarousel-control-next" rel="nofollow"></a></span>
</div>
<%} %>