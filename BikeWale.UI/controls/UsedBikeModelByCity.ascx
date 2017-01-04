<%@ Control Language="C#" AutoEventWireup="false"  Inherits="Bikewale.Controls.UsedBikeModelByCity" EnableViewState="false" %>
<%if (UsedBikeInCityList != null){%>
<div class="jcarousel-wrapper inner-content-carousel city-model-carousel type-model-carousel">
    <div class="jcarousel">
        <ul>
               <%for (int index = 0; index < FetchCount; index++)
              {
                  var ModelDetails = UsedBikeInCityList[index];
            %>
            <li>
                <a href="<%=string.Format("/used/{0}-{1}-bikes-in-{2}/",ModelDetails.MakeMaskingName,ModelDetails.ModelMaskingName,CityMaskingName) %>" title="<%=string.Format("Used {0} {1} bikes in {2}",ModelDetails.MakeName,ModelDetails.ModelName,CityName)%>" class="card-target-block">
                    <div class="card-image-placeholder">
                        <%if (index < 3)
                          { %>
                        <img src="<%=Bikewale.Utility.Image.GetPathToShowImages(ModelDetails.OriginalImagePath,ModelDetails.HostUrl,Bikewale.Utility.ImageSize._227x128) %>" alt="<%=string.Format("Used {0} {1} bikes in {2}",ModelDetails.MakeName,ModelDetails.ModelName,CityName)%>" />
                        <% }
                          else
                          {%>
                        <img class="lazy" data-original="<%=Bikewale.Utility.Image.GetPathToShowImages(ModelDetails.OriginalImagePath,ModelDetails.HostUrl,Bikewale.Utility.ImageSize._227x128) %>" src="" alt="<%=string.Format("Used {0} {1} bikes in {2}",ModelDetails.MakeName,ModelDetails.ModelName,CityName)%>" />
                        <%} %>
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
<%}%>